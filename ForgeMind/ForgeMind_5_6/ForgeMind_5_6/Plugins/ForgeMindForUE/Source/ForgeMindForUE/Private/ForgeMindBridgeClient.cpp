// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#include "ForgeMindBridgeClient.h"

#include "AssetRegistry/AssetRegistryModule.h"
#include "Dom/JsonValue.h"
#include "Editor.h"
#include "EdGraph/EdGraphPin.h"
#include "EdGraphSchema_K2.h"
#include "Engine/Blueprint.h"
#include "Engine/BlueprintGeneratedClass.h"
#include "Kismet2/BlueprintEditorUtils.h"
#include "Kismet2/KismetEditorUtilities.h"
#include "Misc/PackageName.h"
#include "Components/ActorComponent.h"
#include "Components/SceneComponent.h"
#include "GameFramework/Actor.h"
#include "GameFramework/GameModeBase.h"
#include "HAL/PlatformProcess.h"
#include "HAL/Runnable.h"
#include "HAL/RunnableThread.h"
#include "JsonObjectConverter.h"
#include "PlayInEditorDataTypes.h"
#include "Settings/LevelEditorPlaySettings.h"
#include "Subsystems/EditorActorSubsystem.h"
#include "Interfaces/IMainFrameModule.h"
#include "Misc/EngineVersion.h"
#include "Misc/FileHelper.h"
#include "Misc/Paths.h"
#include "Serialization/JsonReader.h"
#include "Serialization/JsonSerializer.h"
#include "Serialization/JsonWriter.h"
#include "SocketSubsystem.h"
#include "Sockets.h"
#include "Subsystems/AssetEditorSubsystem.h"
#include "UObject/UObjectGlobals.h"
#include "UObject/UObjectIterator.h"
#include "UObject/Package.h"
#include "UObject/UnrealType.h"

// Defined here, declared extern in ForgeMindBlueprintUtils.h so the split
// implementation files share one category.
DEFINE_LOG_CATEGORY(LogForgeMindBridge);

namespace
{
	/** Hard cap for a single frame payload (guards against corrupt headers). */
	constexpr int32 MaxFrameBytes = 16 * 1024 * 1024;

	/** Keepalive interval - well under the host's 90s silence sweep. */
	constexpr double KeepaliveSeconds = 30.0;

	/** Upper bound of the exponential connect backoff. */
	constexpr double MaxBackoffSeconds = 60.0;

	/** Wraps a member loop as an FRunnable for FRunnableThread::Create. */
	class FBridgeThreadTask : public FRunnable
	{
		TFunction<uint32()> Body;

	public:
		explicit FBridgeThreadTask(TFunction<uint32()> InBody)
			: Body(MoveTemp(InBody))
		{
		}

		virtual uint32 Run() override
		{
			return Body ? Body() : 0u;
		}
	};

	FString SerializeJsonObject(const TSharedRef<FJsonObject>& Object)
	{
		FString Output;
		TSharedRef<TJsonWriter<>> Writer = TJsonWriterFactory<>::Create(&Output);
		FJsonSerializer::Serialize(Object, Writer);
		return Output;
	}

	TSharedRef<FJsonObject> MakeEnvelope(const FString& Type, const FString& Name, const FString& Id = FString())
	{
		TSharedRef<FJsonObject> Envelope = MakeShared<FJsonObject>();
		Envelope->SetNumberField(TEXT("v"), 1);
		Envelope->SetStringField(TEXT("type"), Type);
		if (!Id.IsEmpty())
		{
			Envelope->SetStringField(TEXT("id"), Id);
		}
		Envelope->SetStringField(TEXT("name"), Name);
		return Envelope;
	}

	/**
	 * Converts a frame payload (raw UTF-8 bytes, NOT null-terminated) to an FString.
	 * The bytes are copied with an explicit trailing zero first: with a non-terminated
	 * buffer the string converter reads one byte past the end (StringConv.h Init passes
	 * SourceLen + 1 to Convert), which emits one spurious trailing character and breaks
	 * strict JSON parsing.
	 */
	FString PayloadToString(const TArray<uint8>& Payload, const int32 Length)
	{
		TArray<uint8> Terminated;
		Terminated.SetNumUninitialized(Length + 1);
		FMemory::Memcpy(Terminated.GetData(), Payload.GetData(), Length);
		Terminated[Length] = 0;
		return FString(FUTF8ToTCHAR(reinterpret_cast<const ANSICHAR*>(Terminated.GetData()), Length + 1).Get());
	}

	/**
	 * Builds a complete FText from host-provided information. The host only
	 * transmits the raw ingredients; the actual FText assembly happens here so
	 * the result carries proper localization identity:
	 *   - JSON string                              -> culture-invariant literal
	 *   - { stringTable, key }                     -> string table reference
	 *   - { format, args:{name:value} }            -> FText::Format result
	 *   - { text, [namespace], [key] }             -> text with gatherable identity
	 * Returns false when the payload cannot describe a text at all.
	 */
	bool TryBuildTextFromJson(const TSharedPtr<FJsonValue>& JsonValue, FText& OutText, FString& OutError)
	{
		if (!JsonValue.IsValid())
		{
			OutError = TEXT("Missing value for the FText property");
			return false;
		}

		// Plain string: a culture-invariant literal without localization identity.
		if (JsonValue->Type == EJson::String)
		{
			OutText = FText::FromString(JsonValue->AsString());
			return true;
		}

		const TSharedPtr<FJsonObject>* ObjectPtr = nullptr;
		if (JsonValue->Type != EJson::Object
			|| !JsonValue->TryGetObject(ObjectPtr) || !ObjectPtr->IsValid())
		{
			OutError = TEXT("FText value must be a string or an object "
				"({ text, [namespace], [key] }, { stringTable, key } or { format, args })");
			return false;
		}
		const TSharedRef<FJsonObject> Object = ObjectPtr->ToSharedRef();

		// String table reference - resolves against the loaded tables and
		// follows the table's translations at display time.
		FString TableId;
		FString TableKey;
		if (Object->TryGetStringField(TEXT("stringTable"), TableId))
		{
			if (!Object->TryGetStringField(TEXT("key"), TableKey) || TableKey.IsEmpty())
			{
				OutError = TEXT("FText string table value needs both 'stringTable' and 'key'");
				return false;
			}
			OutText = FText::FromStringTable(FName(*TableId), TableKey);
			return true;
		}

		// Format pattern with named arguments - each argument is itself built
		// through this converter, so nested descriptions work too.
		FString FormatPattern;
		if (Object->TryGetStringField(TEXT("format"), FormatPattern))
		{
			FFormatNamedArguments Arguments;
			const TSharedPtr<FJsonObject>* ArgsObject = nullptr;
			if (Object->TryGetObjectField(TEXT("args"), ArgsObject) && ArgsObject->IsValid())
			{
				for (const TPair<FString, TSharedPtr<FJsonValue>>& Arg : (*ArgsObject)->Values)
				{
					FText ArgumentText;
					if (!TryBuildTextFromJson(Arg.Value, ArgumentText, OutError))
					{
						return false;
					}
					Arguments.Add(Arg.Key, ArgumentText);
				}
			}
			OutText = FText::Format(FText::FromString(FormatPattern), Arguments);
			return true;
		}

		// Source text with optional localization identity. ChangeKey stamps
		// namespace/key onto the text so the localization pipeline can gather
		// and replace it; without them the result stays a literal.
		FString SourceText;
		if (!Object->TryGetStringField(TEXT("text"), SourceText))
		{
			OutError = TEXT("FText object value needs 'text', or 'stringTable'+'key', or 'format'");
			return false;
		}

		FString Namespace;
		FString Key;
		const bool bHasNamespace = Object->TryGetStringField(TEXT("namespace"), Namespace);
		const bool bHasKey = Object->TryGetStringField(TEXT("key"), Key);
		OutText = (bHasNamespace || bHasKey)
			? FText::ChangeKey(Namespace, Key, FText::FromString(SourceText))
			: FText::FromString(SourceText);
		return true;
	}
}

FForgeMindBridgeClient::FForgeMindBridgeClient()
{
	AdvertisedCommands = { TEXT("ping"), TEXT("get_status"), TEXT("open_asset"), TEXT("exec_console"), TEXT("quit_editor"), TEXT("list_class_hierarchy"), TEXT("get_current_level"), TEXT("set_pie"), TEXT("list_level_actors"), TEXT("get_actor_details"), TEXT("delete_actor"), TEXT("spawn_actor"), TEXT("set_actor_property"), TEXT("create_blueprint"), TEXT("add_blueprint_variable"), TEXT("set_blueprint_variable_default"), TEXT("update_blueprint_variable"), TEXT("list_blueprint_variables"), TEXT("remove_blueprint_variable") };

	RequestHandlers.Add(TEXT("ping"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandlePing(Payload); });
	RequestHandlers.Add(TEXT("get_status"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleGetStatus(Payload); });
	RequestHandlers.Add(TEXT("open_asset"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleOpenAsset(Payload); });
	RequestHandlers.Add(TEXT("exec_console"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleExecConsole(Payload); });
	RequestHandlers.Add(TEXT("quit_editor"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleQuitEditor(Payload); });
	RequestHandlers.Add(TEXT("list_class_hierarchy"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleListClassHierarchy(Payload); });
	RequestHandlers.Add(TEXT("get_current_level"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleGetCurrentLevel(Payload); });
	RequestHandlers.Add(TEXT("set_pie"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleSetPie(Payload); });
	RequestHandlers.Add(TEXT("list_level_actors"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleListLevelActors(Payload); });
	RequestHandlers.Add(TEXT("get_actor_details"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleGetActorDetails(Payload); });
	RequestHandlers.Add(TEXT("delete_actor"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleDeleteActor(Payload); });
	RequestHandlers.Add(TEXT("spawn_actor"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleSpawnActor(Payload); });
	RequestHandlers.Add(TEXT("set_actor_property"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleSetActorProperty(Payload); });
	RequestHandlers.Add(TEXT("create_blueprint"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleCreateBlueprint(Payload); });
	RequestHandlers.Add(TEXT("add_blueprint_variable"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleAddBlueprintVariable(Payload); });
	RequestHandlers.Add(TEXT("set_blueprint_variable_default"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleSetBlueprintVariableDefault(Payload); });
	RequestHandlers.Add(TEXT("update_blueprint_variable"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleUpdateBlueprintVariable(Payload); });
	RequestHandlers.Add(TEXT("list_blueprint_variables"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleListBlueprintVariables(Payload); });
	RequestHandlers.Add(TEXT("remove_blueprint_variable"), [this](const TSharedPtr<FJsonObject>& Payload) { return HandleRemoveBlueprintVariable(Payload); });
}

FForgeMindBridgeClient::~FForgeMindBridgeClient()
{
	Stop();
}

void FForgeMindBridgeClient::Start()
{
	bStopping = false;
	bStopped = false;
	bConnectionLost = false;
	NextAttemptTime = FPlatformTime::Seconds(); // first attempt immediately
	PreExitHandle = FEditorDelegates::OnEditorPreExit.AddRaw(this, &FForgeMindBridgeClient::OnPreExit);
	UE_LOG(LogForgeMindBridge, Log, TEXT("Bridge client started - will connect to the ForgeMind host when it publishes bridge.json"));
}

void FForgeMindBridgeClient::Stop()
{
	if (bStopped)
	{
		return;
	}
	bStopping = true;

	// Short grace period so a pending editor_closing frame can still flush
	const double Deadline = FPlatformTime::Seconds() + 1.0;
	while (FPlatformTime::Seconds() < Deadline)
	{
		{
			FScopeLock Lock(&SendQueueLock);
			if (SendQueue.Num() == 0)
			{
				break;
			}
		}
		FPlatformProcess::Sleep(0.02f);
	}

	if (ReceiveThread.IsValid())
	{
		ReceiveThread->Kill();
		ReceiveThread.Reset();
	}
	if (SendThread.IsValid())
	{
		SendThread->Kill();
		SendThread.Reset();
	}

	if (Socket != nullptr)
	{
		Socket->Close();
		ISocketSubsystem::Get(PLATFORM_SOCKETSUBSYSTEM)->DestroySocket(Socket);
		Socket = nullptr;
	}
	bConnected = false;
	bStopped = true;

	if (PreExitHandle.IsValid())
	{
		FEditorDelegates::OnEditorPreExit.RemoveAll(this);
		PreExitHandle.Reset();
	}
	UE_LOG(LogForgeMindBridge, Log, TEXT("Bridge client stopped"));
}

void FForgeMindBridgeClient::OnPreExit()
{
	// Best-effort closing notice; Stop() gives it a short flush window
	QueueSend(SerializeJsonObject(MakeEnvelope(TEXT("event"), TEXT("editor_closing"))));
	Stop();
}

void FForgeMindBridgeClient::Tick(float DeltaTime)
{
	if (bStopping)
	{
		return;
	}

	// Deferred editor shutdown requested via quit_editor - the acknowledgement
	// response was queued last tick and has been flushed by the send thread.
	// RequestCloseEditor() runs the unsaved-packages prompt and may be refused
	// (save/GC/slow-task in flight, or the user cancels the save dialog).
	if (bQuitRequested)
	{
		bQuitRequested = false;
		UE_LOG(LogForgeMindBridge, Log, TEXT("quit_editor received - requesting editor close"));
		IMainFrameModule& MainFrame = FModuleManager::LoadModuleChecked<IMainFrameModule>(TEXT("MainFrame"));
		MainFrame.RequestCloseEditor();
	}

	// Connection lost reported by a worker thread - tear down and back off
	if (bConnectionLost)
	{
		bConnectionLost = false;
		HandleDisconnect();
	}

	if (!bConnected && FPlatformTime::Seconds() >= NextAttemptTime)
	{
		TryConnect();
	}

	if (!bConnected)
	{
		return;
	}

	// Keepalive keeps this session out of the host's silent-session sweep
	const double Now = FPlatformTime::Seconds();
	if (Now - LastKeepaliveTime >= KeepaliveSeconds)
	{
		LastKeepaliveTime = Now;
		QueueSend(SerializeJsonObject(MakeEnvelope(TEXT("request"), TEXT("ping"), FGuid::NewGuid().ToString(EGuidFormats::Digits))));
	}

	// Drain inbound messages on the game thread
	TArray<TSharedRef<FJsonObject>> Pending;
	{
		FScopeLock Lock(&GameQueueLock);
		Pending = MoveTemp(GameQueue);
	}
	for (const TSharedRef<FJsonObject>& Message : Pending)
	{
		FString Type = Message->GetStringField(TEXT("type"));
		if (Type == TEXT("request"))
		{
			ProcessRequest(Message);
		}
		else if (Type == TEXT("event"))
		{
			ProcessEvent(Message);
		}
		// responses - replies to our keepalive pings; nothing to do
	}
}

void FForgeMindBridgeClient::TryConnect()
{
	if (ConnectOnce())
	{
		bConnected = true;
		BackoffSeconds = 1.0;
		LastKeepaliveTime = FPlatformTime::Seconds();
		StartMessageThreads();

		// Announce ourselves once the channel is live
		QueueSend(SerializeJsonObject(MakeEnvelope(TEXT("event"), TEXT("editor_ready"))));
	}
	else
	{
		BackoffSeconds = FMath::Min(BackoffSeconds * 2.0, MaxBackoffSeconds);
	}
	NextAttemptTime = FPlatformTime::Seconds() + BackoffSeconds;
}

bool FForgeMindBridgeClient::ConnectOnce()
{
	const FDiscovery Discovery = ReadDiscovery();
	if (!Discovery.bValid)
	{
		return false; // no host published yet (or stale/corrupt file) - logged inside ReadDiscovery
	}

	ISocketSubsystem* Subsystem = ISocketSubsystem::Get(PLATFORM_SOCKETSUBSYSTEM);
	Socket = Subsystem->CreateSocket(NAME_Stream, TEXT("ForgeMindBridge"), false);
	if (Socket == nullptr)
	{
		return false;
	}

	TSharedRef<FInternetAddr> Address = Subsystem->CreateInternetAddr();
	// SetLoopbackAddress() handles byte order internally - SetIp(uint32) applies
	// htonl itself despite its header comment, so a pre-converted constant gets
	// double-swapped and silently targets a bogus external address.
	Address->SetLoopbackAddress();
	Address->SetPort(Discovery.Port);

	if (!Socket->Connect(*Address))
	{
		const ESocketErrors SocketError = Subsystem->GetLastErrorCode();
		UE_LOG(LogForgeMindBridge, Warning, TEXT("Connect to 127.0.0.1:%d failed - error %d (%s)"),
			Discovery.Port, static_cast<int32>(SocketError), Subsystem->GetSocketError(SocketError));
		Subsystem->DestroySocket(Socket);
		Socket = nullptr;
		return false;
	}

	Token = Discovery.Token;

	// Auth + handshake happen synchronously before the worker threads exist.
	// The host answers only the handshake; a dropped connection means bad auth.
	// UE 5.6 has no FSocket receive timeout - poll for the reply with a deadline.
	const double HandshakeDeadline = FPlatformTime::Seconds() + 10.0;

	const FString AuthJson = [&]
	{
		TSharedRef<FJsonObject> Envelope = MakeEnvelope(TEXT("request"), TEXT("auth"), TEXT("auth"));
		TSharedRef<FJsonObject> AuthPayload = MakeShared<FJsonObject>();
		AuthPayload->SetStringField(TEXT("token"), Token);
		Envelope->SetObjectField(TEXT("payload"), AuthPayload);
		return SerializeJsonObject(Envelope);
	}();

	const FString HandshakeJson = [&]
	{
		TSharedRef<FJsonObject> Envelope = MakeEnvelope(TEXT("request"), TEXT("handshake"), TEXT("handshake"));
		TSharedRef<FJsonObject> Info = MakeShared<FJsonObject>();
		Info->SetStringField(TEXT("projectFile"), FPaths::GetProjectFilePath());
		const FEngineVersion& Version = FEngineVersion::Current();
		Info->SetStringField(TEXT("engineVersion"), FString::Printf(TEXT("%u.%u.%u"), Version.GetMajor(), Version.GetMinor(), Version.GetPatch()));
		Info->SetNumberField(TEXT("pid"), FPlatformProcess::GetCurrentProcessId());
		TArray<TSharedPtr<FJsonValue>> Commands;
		for (const FString& Command : AdvertisedCommands)
		{
			Commands.Add(MakeShared<FJsonValueString>(Command));
		}
		Info->SetArrayField(TEXT("commands"), Commands);
		Envelope->SetObjectField(TEXT("payload"), Info);
		return SerializeJsonObject(Envelope);
	}();

	if (!SendFrame(AuthJson))
	{
		UE_LOG(LogForgeMindBridge, Warning, TEXT("Handshake failed - could not send auth frame"));
		Subsystem->DestroySocket(Socket);
		Socket = nullptr;
		return false;
	}
	if (!SendFrame(HandshakeJson))
	{
		UE_LOG(LogForgeMindBridge, Warning, TEXT("Handshake failed - could not send handshake frame"));
		Subsystem->DestroySocket(Socket);
		Socket = nullptr;
		return false;
	}

	// Await the handshake response frame
	uint32 Pending = 0;
	while (!Socket->HasPendingData(Pending) || Pending < 4)
	{
		if (FPlatformTime::Seconds() >= HandshakeDeadline)
		{
			UE_LOG(LogForgeMindBridge, Warning, TEXT("Handshake failed - no reply from host within 10s"));
			Subsystem->DestroySocket(Socket);
			Socket = nullptr;
			return false;
		}
		FPlatformProcess::Sleep(0.02f);
	}

	uint8 Header[4];
	if (!RecvFull(Header, sizeof(Header)))
	{
		UE_LOG(LogForgeMindBridge, Warning, TEXT("Handshake failed - host rejected auth or timed out"));
		Subsystem->DestroySocket(Socket);
		Socket = nullptr;
		return false;
	}
	const int32 Length = Header[0] | (Header[1] << 8) | (Header[2] << 16) | (Header[3] << 24);
	if (Length <= 0 || Length > MaxFrameBytes)
	{
		UE_LOG(LogForgeMindBridge, Warning, TEXT("Handshake failed - bogus reply length %d"), Length);
		Subsystem->DestroySocket(Socket);
		Socket = nullptr;
		return false;
	}
	TArray<uint8> Payload;
	Payload.SetNumUninitialized(Length);
	if (!RecvFull(Payload.GetData(), Length))
	{
		UE_LOG(LogForgeMindBridge, Warning, TEXT("Handshake failed - reply truncated at %d bytes"), Length);
		Subsystem->DestroySocket(Socket);
		Socket = nullptr;
		return false;
	}

	const FString ResponseJson = PayloadToString(Payload, Length);
	TSharedPtr<FJsonObject> Response;
	if (!FJsonSerializer::Deserialize(TJsonReaderFactory<>::Create(ResponseJson), Response) || !Response.IsValid())
	{
		UE_LOG(LogForgeMindBridge, Warning, TEXT("Handshake failed - reply is not JSON: %s"), *ResponseJson.Left(200));
		Subsystem->DestroySocket(Socket);
		Socket = nullptr;
		return false;
	}

	bool bOk = Response->GetStringField(TEXT("type")) == TEXT("response")
		&& Response->GetStringField(TEXT("name")) == TEXT("handshake")
		&& !Response->HasTypedField<EJson::String>(TEXT("error"));
	if (!bOk)
	{
		UE_LOG(LogForgeMindBridge, Warning, TEXT("Handshake refused by host: %s"), *ResponseJson);
		Subsystem->DestroySocket(Socket);
		Socket = nullptr;
		return false;
	}

	// Worker-thread mode: non-blocking socket polled via HasPendingData
	Socket->SetNonBlocking(true);
	UE_LOG(LogForgeMindBridge, Log, TEXT("Connected to ForgeMind host on 127.0.0.1:%d"), Discovery.Port);
	return true;
}

FForgeMindBridgeClient::FDiscovery FForgeMindBridgeClient::ReadDiscovery() const
{
	FDiscovery Result;

	// UserHomeDir() = %USERPROFILE% on Windows. NOT UserDir() - that one returns
	// the Documents folder, which does not match the host's publish location.
	const FString Path = FPaths::Combine(FPlatformProcess::UserHomeDir(), TEXT(".forgemind"), TEXT("bridge.json"));
	FString Contents;
	if (!FFileHelper::LoadFileToString(Contents, *Path))
	{
		UE_LOG(LogForgeMindBridge, Warning, TEXT("Discovery failed - cannot read %s"), *Path);
		return Result;
	}

	TSharedPtr<FJsonObject> Json;
	if (!FJsonSerializer::Deserialize(TJsonReaderFactory<>::Create(Contents), Json) || !Json.IsValid())
	{
		UE_LOG(LogForgeMindBridge, Warning, TEXT("Discovery failed - %s is not valid JSON"), *Path);
		return Result;
	}

	int32 Port = 0;
	FString DiscoveredToken;
	if (!Json->TryGetNumberField(TEXT("port"), Port) || !Json->TryGetStringField(TEXT("token"), DiscoveredToken))
	{
		UE_LOG(LogForgeMindBridge, Warning, TEXT("Discovery failed - %s misses 'port' or 'token'"), *Path);
		return Result;
	}

	Result.Port = Port;
	Result.Token = DiscoveredToken;
	Result.bValid = Port > 0 && !DiscoveredToken.IsEmpty();
	return Result;
}

void FForgeMindBridgeClient::StartMessageThreads()
{
	ReceiveThread = TSharedPtr<FRunnableThread>(FRunnableThread::Create(
		new FBridgeThreadTask([this] { return ReceiveLoop(); }), TEXT("ForgeMindBridgeRecv")));
	SendThread = TSharedPtr<FRunnableThread>(FRunnableThread::Create(
		new FBridgeThreadTask([this] { return SendLoop(); }), TEXT("ForgeMindBridgeSend")));
}

void FForgeMindBridgeClient::HandleDisconnect()
{
	if (ReceiveThread.IsValid())
	{
		ReceiveThread->Kill();
		ReceiveThread.Reset();
	}
	if (SendThread.IsValid())
	{
		SendThread->Kill();
		SendThread.Reset();
	}

	if (Socket != nullptr)
	{
		Socket->Close();
		ISocketSubsystem::Get(PLATFORM_SOCKETSUBSYSTEM)->DestroySocket(Socket);
		Socket = nullptr;
	}
	bConnected = false;
	BackoffSeconds = FMath::Min(BackoffSeconds * 2.0, MaxBackoffSeconds);
	NextAttemptTime = FPlatformTime::Seconds() + BackoffSeconds;

	UE_LOG(LogForgeMindBridge, Log, TEXT("Connection lost - retrying in %.0fs"), BackoffSeconds);
}

uint32 FForgeMindBridgeClient::ReceiveLoop()
{
	while (!bStopping && !bConnectionLost)
	{
		uint8 Header[4];
		if (!RecvFull(Header, sizeof(Header)))
		{
			break;
		}

		const int32 Length = Header[0] | (Header[1] << 8) | (Header[2] << 16) | (Header[3] << 24);
		if (Length <= 0 || Length > MaxFrameBytes)
		{
			UE_LOG(LogForgeMindBridge, Warning, TEXT("Received corrupt frame length %d - dropping connection"), Length);
			break;
		}

		TArray<uint8> Payload;
		Payload.SetNumUninitialized(Length);
		if (!RecvFull(Payload.GetData(), Length))
		{
			break;
		}

		const FString Json = PayloadToString(Payload, Length);
		TSharedPtr<FJsonObject> Message;
		if (FJsonSerializer::Deserialize(TJsonReaderFactory<>::Create(Json), Message) && Message.IsValid())
		{
			FScopeLock Lock(&GameQueueLock);
			GameQueue.Add(Message.ToSharedRef());
		}
		else
		{
			UE_LOG(LogForgeMindBridge, Verbose, TEXT("Ignoring non-JSON frame (%d bytes)"), Length);
		}
	}

	if (!bStopping)
	{
		bConnectionLost.AtomicSet(true);
	}
	return 0u;
}

uint32 FForgeMindBridgeClient::SendLoop()
{
	while (!bStopping && !bConnectionLost)
	{
		FString Json;
		{
			FScopeLock Lock(&SendQueueLock);
			if (SendQueue.Num() > 0)
			{
				Json = MoveTemp(SendQueue[0]);
				SendQueue.RemoveAt(0);
				if (SendQueue.Num() == 0)
				{
					SendQueueSignal = false;
				}
			}
		}

		if (Json.IsEmpty())
		{
			FPlatformProcess::Sleep(0.05f);
			continue;
		}

		if (!SendFrame(Json))
		{
			break;
		}
	}

	if (!bStopping)
	{
		bConnectionLost.AtomicSet(true);
	}
	return 0u;
}

bool FForgeMindBridgeClient::RecvFull(uint8* Buffer, int32 Size)
{
	int32 Received = 0;
	while (Received < Size)
	{
		if (bStopping)
		{
			return false;
		}

		uint32 Pending = 0;
		Socket->HasPendingData(Pending);
		if (Pending == 0)
		{
			FPlatformProcess::Sleep(0.02f);
			continue;
		}

		int32 Read = 0;
		if (!Socket->Recv(Buffer + Received, Size - Received, Read) || Read <= 0)
		{
			return false;
		}
		Received += Read;
	}
	return true;
}

bool FForgeMindBridgeClient::SendFrame(const FString& Json)
{
	if (Socket == nullptr)
	{
		return false;
	}

	const FTCHARToUTF8 Converter(*Json);
	const int32 PayloadLength = Converter.Length();
	if (PayloadLength > MaxFrameBytes)
	{
		return false;
	}

	TArray<uint8> Frame;
	Frame.SetNumUninitialized(sizeof(int32) + PayloadLength);
	Frame[0] = static_cast<uint8>(PayloadLength & 0xFF);
	Frame[1] = static_cast<uint8>((PayloadLength >> 8) & 0xFF);
	Frame[2] = static_cast<uint8>((PayloadLength >> 16) & 0xFF);
	Frame[3] = static_cast<uint8>((PayloadLength >> 24) & 0xFF);
	FMemory::Memcpy(Frame.GetData() + sizeof(int32), Converter.Get(), PayloadLength);

	// Blocking sockets can still perform partial sends - loop until done
	int32 Sent = 0;
	while (Sent < Frame.Num())
	{
		int32 Chunk = 0;
		if (!Socket->Send(Frame.GetData() + Sent, Frame.Num() - Sent, Chunk) || Chunk <= 0)
		{
			return false;
		}
		Sent += Chunk;
	}
	return true;
}

void FForgeMindBridgeClient::QueueSend(const FString& Json)
{
	FScopeLock Lock(&SendQueueLock);
	SendQueue.Add(Json);
	SendQueueSignal.AtomicSet(true);
}

void FForgeMindBridgeClient::ProcessRequest(TSharedRef<FJsonObject> Message)
{
	const FString Id = Message->GetStringField(TEXT("id"));
	const FString Name = Message->GetStringField(TEXT("name"));
	TSharedPtr<FJsonObject> Payload;
	const TSharedPtr<FJsonObject>* PayloadPtr = nullptr;
	if (Message->TryGetObjectField(TEXT("payload"), PayloadPtr) && PayloadPtr != nullptr)
	{
		Payload = *PayloadPtr;
	}

	TSharedPtr<FJsonObject> Result;
	FString Error;
	if (const TFunction<TSharedPtr<FJsonObject>(const TSharedPtr<FJsonObject>&)>* Handler = RequestHandlers.Find(Name))
	{
		Result = (*Handler)(Payload);
	}
	else
	{
		Error = FString::Printf(TEXT("Unknown command '%s'"), *Name);
	}

	TSharedRef<FJsonObject> Response = MakeEnvelope(TEXT("response"), Name, Id);
	if (Result.IsValid())
	{
		Response->SetObjectField(TEXT("payload"), Result);
	}
	if (!Error.IsEmpty())
	{
		Response->SetStringField(TEXT("error"), Error);
	}
	QueueSend(SerializeJsonObject(Response));
}

void FForgeMindBridgeClient::ProcessEvent(TSharedRef<FJsonObject> Message)
{
	// Host-originated events are not part of the initial command set - log only
	UE_LOG(LogForgeMindBridge, Verbose, TEXT("Host event '%s' received"), *Message->GetStringField(TEXT("name")));
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandlePing(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();
	Result->SetBoolField(TEXT("pong"), true);
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleGetStatus(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();
	Result->SetStringField(TEXT("projectFile"), FPaths::GetProjectFilePath());
	const FEngineVersion& Version = FEngineVersion::Current();
	Result->SetStringField(TEXT("engineVersion"), FString::Printf(TEXT("%u.%u.%u"), Version.GetMajor(), Version.GetMinor(), Version.GetPatch()));
	Result->SetNumberField(TEXT("pid"), FPlatformProcess::GetCurrentProcessId());
	Result->SetBoolField(TEXT("isPlaying"), GEditor != nullptr && GEditor->PlayWorld != nullptr);
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleGetCurrentLevel(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	UWorld* World = GEditor != nullptr ? GEditor->GetEditorWorldContext().World() : nullptr;
	if (World == nullptr)
	{
		Result->SetBoolField(TEXT("found"), false);
		Result->SetStringField(TEXT("reason"), TEXT("No editor world is loaded"));
		return Result;
	}

	Result->SetBoolField(TEXT("found"), true);
	// Package path such as /Game/Maps/MyLevel; a fresh unsaved map lives in /Temp
	Result->SetStringField(TEXT("levelPath"), World->GetOutermost()->GetName());
	// Map name with any PIE prefix already stripped
	Result->SetStringField(TEXT("levelName"), World->GetMapName());
	Result->SetBoolField(TEXT("isDirty"), World->GetOutermost()->IsDirty());
	// The editor world's own WorldType is always Editor - PIE runs in a
	// separate world, so ask GEditor for it (same check as get_status).
	Result->SetBoolField(TEXT("isPlayInEditor"), GEditor->PlayWorld != nullptr);
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleSetPie(const TSharedPtr<FJsonObject>& Payload)
{
	// Every field is optional: an empty payload starts PIE with the user's
	// configured settings, matching the toolbar Play button.
	bool bWantRunning = true;
	if (Payload.IsValid())
	{
		Payload->TryGetBoolField(TEXT("running"), bWantRunning);
	}

	const bool bIsPlaying = GEditor != nullptr && GEditor->PlayWorld != nullptr;

	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();
	if (bWantRunning == bIsPlaying)
	{
		Result->SetBoolField(TEXT("requested"), false);
		Result->SetBoolField(TEXT("isPlaying"), bIsPlaying);
		Result->SetStringField(TEXT("note"), bIsPlaying ? TEXT("PIE is already running") : TEXT("PIE is not running"));
		return Result;
	}

	if (!bWantRunning)
	{
		GEditor->RequestEndPlayMap();
		Result->SetBoolField(TEXT("requested"), true);
		Result->SetBoolField(TEXT("isPlaying"), bIsPlaying);
		Result->SetStringField(TEXT("note"), TEXT("PIE stop was requested; the state changes over the next frames - poll get_current_level to confirm"));
		return Result;
	}

	FRequestPlaySessionParams Params;
	if (Payload.IsValid())
	{
		FString Value;
		if (Payload->TryGetStringField(TEXT("destination"), Value))
		{
			if (Value.Equals(TEXT("inProcess"), ESearchCase::IgnoreCase))
			{
				Params.SessionDestination = EPlaySessionDestinationType::InProcess;
			}
			else if (Value.Equals(TEXT("newProcess"), ESearchCase::IgnoreCase))
			{
				Params.SessionDestination = EPlaySessionDestinationType::NewProcess;
			}
			else if (Value.Equals(TEXT("launcher"), ESearchCase::IgnoreCase))
			{
				Params.SessionDestination = EPlaySessionDestinationType::Launcher;
			}
			else
			{
				Result->SetBoolField(TEXT("requested"), false);
				Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("Unknown destination '%s' (valid: inProcess, newProcess, launcher)"), *Value));
				return Result;
			}
		}

		if (Payload->TryGetStringField(TEXT("worldType"), Value))
		{
			if (Value.Equals(TEXT("pie"), ESearchCase::IgnoreCase) || Value.Equals(TEXT("playInEditor"), ESearchCase::IgnoreCase))
			{
				Params.WorldType = EPlaySessionWorldType::PlayInEditor;
			}
			else if (Value.Equals(TEXT("sie"), ESearchCase::IgnoreCase) || Value.Equals(TEXT("simulateInEditor"), ESearchCase::IgnoreCase))
			{
				Params.WorldType = EPlaySessionWorldType::SimulateInEditor;
			}
			else
			{
				Result->SetBoolField(TEXT("requested"), false);
				Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("Unknown worldType '%s' (valid: pie, sie)"), *Value));
				return Result;
			}
		}

		if (Payload->TryGetStringField(TEXT("preview"), Value))
		{
			EPlaySessionPreviewType Preview;
			if (Value.Equals(TEXT("none"), ESearchCase::IgnoreCase))
			{
				Preview = EPlaySessionPreviewType::NoPreview;
			}
			else if (Value.Equals(TEXT("mobile"), ESearchCase::IgnoreCase))
			{
				Preview = EPlaySessionPreviewType::MobilePreview;
			}
			else if (Value.Equals(TEXT("vulkan"), ESearchCase::IgnoreCase))
			{
				Preview = EPlaySessionPreviewType::VulkanPreview;
			}
			else if (Value.Equals(TEXT("vr"), ESearchCase::IgnoreCase))
			{
				Preview = EPlaySessionPreviewType::VRPreview;
			}
			else
			{
				Result->SetBoolField(TEXT("requested"), false);
				Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("Unknown preview '%s' (valid: none, mobile, vulkan, vr)"), *Value));
				return Result;
			}
			Params.SessionPreviewTypeOverride = Preview;
		}

		if (Payload->TryGetStringField(TEXT("map"), Value) && !Value.IsEmpty())
		{
			Params.GlobalMapOverride = Value;
		}

		if (Payload->TryGetStringField(TEXT("gameMode"), Value) && !Value.IsEmpty())
		{
			// GameMode classes carry the C++ prefix in reflection (AGameModeBase),
			// so tolerate both spellings like list_class_hierarchy does.
			UClass* GameModeClass = FindFirstObject<UClass>(*Value);
			if (GameModeClass == nullptr && Value.StartsWith(TEXT("GameMode")))
			{
				GameModeClass = FindFirstObject<UClass>(*(TEXT("A") + Value));
			}
			if (GameModeClass == nullptr || !GameModeClass->IsChildOf(AGameModeBase::StaticClass()))
			{
				Result->SetBoolField(TEXT("requested"), false);
				Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("'%s' is not a GameMode class"), *Value));
				return Result;
			}
			Params.GameModeOverride = GameModeClass;
		}

		// Reads {x, y, z} / {pitch, yaw, roll} numeric fields; any other
		// representation is ignored so the GameMode PlayerStart is used.
		auto TryGetVector = [&Payload](const TCHAR* Field, FVector& Out) -> bool
		{
			const TSharedPtr<FJsonObject>* Obj;
			if (!Payload.IsValid() || !Payload->TryGetObjectField(Field, Obj) || !Obj->IsValid())
			{
				return false;
			}
			double X, Y, Z;
			if (!(*Obj)->TryGetNumberField(TEXT("x"), X) || !(*Obj)->TryGetNumberField(TEXT("y"), Y) || !(*Obj)->TryGetNumberField(TEXT("z"), Z))
			{
				return false;
			}
			Out = FVector(X, Y, Z);
			return true;
		};
		FVector Location;
		if (TryGetVector(TEXT("startLocation"), Location))
		{
			Params.StartLocation = Location;
			const TSharedPtr<FJsonObject>* RotObj;
			if (Payload->TryGetObjectField(TEXT("startRotation"), RotObj) && RotObj->IsValid())
			{
				double Pitch = 0.0, Yaw = 0.0, Roll = 0.0;
				(*RotObj)->TryGetNumberField(TEXT("pitch"), Pitch);
				(*RotObj)->TryGetNumberField(TEXT("yaw"), Yaw);
				(*RotObj)->TryGetNumberField(TEXT("roll"), Roll);
				Params.StartRotation = FRotator(Pitch, Yaw, Roll);
			}
		}

		if (Payload->TryGetStringField(TEXT("standaloneArgs"), Value) && !Value.IsEmpty())
		{
			// Only consumed when destination is newProcess.
			Params.AdditionalStandaloneCommandLineParameters = Value;
		}

		double NumPlayers;
		if (Payload->TryGetNumberField(TEXT("numPlayers"), NumPlayers))
		{
			const int32 Clamped = FMath::Clamp(FMath::RoundToInt32(NumPlayers), 1, 4); // 4 == MAX_LOCAL_PLAYERS
			// Same object the PIE preferences UI edits; persisted with user settings.
			GetMutableDefault<ULevelEditorPlaySettings>()->SetPlayNumberOfClients(Clamped);
		}
	}

	// The session starts over the following frames.
	GEditor->RequestPlaySession(Params);

	Result->SetBoolField(TEXT("requested"), true);
	Result->SetBoolField(TEXT("isPlaying"), bIsPlaying);
	Result->SetStringField(TEXT("note"), TEXT("PIE start was requested; the state changes over the next frames - poll get_current_level to confirm"));
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleOpenAsset(const TSharedPtr<FJsonObject>& Payload)
{
	FString AssetPath;
	if (!Payload.IsValid() || !Payload->TryGetStringField(TEXT("path"), AssetPath) || AssetPath.IsEmpty())
	{
		TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();
		Result->SetBoolField(TEXT("opened"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Missing 'path' payload field"));
		return Result;
	}

	UObject* Asset = LoadObject<UObject>(nullptr, *AssetPath);
	bool bOpened = false;
	if (Asset != nullptr && GEditor != nullptr)
	{
		UAssetEditorSubsystem* AssetEditor = GEditor->GetEditorSubsystem<UAssetEditorSubsystem>();
		bOpened = AssetEditor != nullptr && AssetEditor->OpenEditorForAsset(Asset);
	}

	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();
	Result->SetBoolField(TEXT("opened"), bOpened);
	Result->SetStringField(TEXT("path"), AssetPath);
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleExecConsole(const TSharedPtr<FJsonObject>& Payload)
{
	FString Command;
	if (!Payload.IsValid() || !Payload->TryGetStringField(TEXT("command"), Command) || Command.IsEmpty())
	{
		TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();
		Result->SetBoolField(TEXT("executed"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Missing 'command' payload field"));
		return Result;
	}

	if (GEditor != nullptr)
	{
		GEditor->Exec(nullptr, *Command, *GLog);
	}

	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();
	Result->SetBoolField(TEXT("executed"), true);
	Result->SetStringField(TEXT("command"), Command);
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleQuitEditor(const TSharedPtr<FJsonObject>& Payload)
{
	// Plugin-initiated graceful shutdown: same path as closing the main window
	// (unsaved-package prompt, debugger/GC/slow-task guards). Never a host-side
	// process kill. The close itself is deferred to the next tick so this
	// acknowledgement reaches the host before the editor starts shutting down.
	bQuitRequested.AtomicSet(true);

	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();
	Result->SetBoolField(TEXT("exitRequested"), true);
	Result->SetStringField(TEXT("note"), TEXT("Editor close starts on the next frame; if unsaved assets exist a prompt appears and the editor only proceeds to unload after the user decides (cancelling keeps it running), otherwise it unloads directly"));
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleListClassHierarchy(const TSharedPtr<FJsonObject>& Payload)
{
	FString ClassName;
	if (!Payload.IsValid() || !Payload->TryGetStringField(TEXT("class"), ClassName) || ClassName.IsEmpty())
	{
		TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();
		Result->SetBoolField(TEXT("found"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Missing 'class' payload field"));
		return Result;
	}

	double ParentDepth = 0.0;
	double ChildDepth = 0.0;
	// Absent depth fields fall back to useful defaults: parents walk up to
	// the UObject cap (chains are short anyway), children get one level.
	// An explicitly passed 0 still means "skip that direction".
	const bool bHasParentLevels = Payload->TryGetNumberField(TEXT("parentLevels"), ParentDepth);
	const bool bHasChildLevels = Payload->TryGetNumberField(TEXT("childLevels"), ChildDepth);
	const int32 ParentLevels = bHasParentLevels ? FMath::Max(0, FMath::RoundToInt32(ParentDepth)) : INT32_MAX;
	const int32 ChildLevels = bHasChildLevels ? FMath::Max(0, FMath::RoundToInt32(ChildDepth)) : 1;

	// Reflection classes carry no namespace; tolerate the C++ type prefix
	// (AActor -> Actor) since class objects are registered without it.
	bool bPrefixStripped = false;
	UClass* Class = FindFirstObject<UClass>(*ClassName);
	if (Class == nullptr && ClassName.Len() > 1 && (ClassName[0] == 'U' || ClassName[0] == 'A' || ClassName[0] == 'F'))
	{
		Class = FindFirstObject<UClass>(*ClassName.RightChop(1));
		bPrefixStripped = Class != nullptr;
	}

	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();
	if (Class == nullptr)
	{
		Result->SetBoolField(TEXT("found"), false);
		Result->SetStringField(TEXT("class"), ClassName);
		Result->SetStringField(TEXT("reason"), TEXT("No UClass with that reflection name is loaded"));
		return Result;
	}

	// Parents: nearest first. UObject is the top of the world relevant to
	// Blueprint - even if C++ reflection could expose anything above it, it is
	// never reported.
	TArray<TSharedPtr<FJsonValue>> Parents;
	UClass* Super = Class->GetSuperClass();
	for (int32 Depth = 0; Super != nullptr && Depth < ParentLevels; ++Depth)
	{
		Parents.Add(MakeShared<FJsonValueString>(Super->GetName()));
		if (Super == UObject::StaticClass())
		{
			break;
		}
		Super = Super->GetSuperClass();
	}

	// Children: breadth-first over every loaded UClass (native and Blueprint
	// generated alike), level by level, names sorted for stable output.
	TArray<TSharedPtr<FJsonValue>> Children;
	TArray<UClass*> CurrentLevel;
	CurrentLevel.Add(Class);
	for (int32 Depth = 1; Depth <= ChildLevels && CurrentLevel.Num() > 0; ++Depth)
	{
		TArray<UClass*> NextLevel;
		for (TObjectIterator<UClass> It; It; ++It)
		{
			UClass* Candidate = *It;
			if (Candidate->GetSuperClass() != nullptr && CurrentLevel.Contains(Candidate->GetSuperClass()))
			{
				NextLevel.Add(Candidate);
			}
		}
		NextLevel.Sort([](UClass& Lhs, UClass& Rhs) { return Lhs.GetName() < Rhs.GetName(); });
		for (UClass* Child : NextLevel)
		{
			TSharedRef<FJsonObject> Entry = MakeShared<FJsonObject>();
			Entry->SetNumberField(TEXT("depth"), Depth);
			Entry->SetStringField(TEXT("name"), Child->GetName());
			Children.Add(MakeShared<FJsonValueObject>(Entry));
		}
		CurrentLevel = MoveTemp(NextLevel);
	}

	Result->SetBoolField(TEXT("found"), true);
	Result->SetStringField(TEXT("class"), ClassName);
	Result->SetStringField(TEXT("reflectionName"), Class->GetName());
	if (bPrefixStripped)
	{
		Result->SetStringField(TEXT("note"), FString::Printf(
			TEXT("UE reflection names drop the C++ type prefix - '%s' is registered as '%s'"),
			*ClassName, *Class->GetName()));
	}
	Result->SetArrayField(TEXT("parents"), Parents);
	Result->SetArrayField(TEXT("children"), Children);
	Result->SetNumberField(TEXT("childCount"), Children.Num());
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleListLevelActors(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	UWorld* World = GEditor != nullptr ? GEditor->GetEditorWorldContext().World() : nullptr;
	ULevel* Level = World != nullptr ? World->GetCurrentLevel() : nullptr;
	if (Level == nullptr)
	{
		Result->SetBoolField(TEXT("found"), false);
		Result->SetStringField(TEXT("reason"), TEXT("No editor world is loaded"));
		return Result;
	}

	TArray<TSharedPtr<FJsonValue>> Actors;
	Actors.Reserve(Level->Actors.Num());
	for (const TObjectPtr<AActor>& Actor : Level->Actors)
	{
		if (!IsValid(Actor))
		{
			continue;
		}
		TSharedRef<FJsonObject> Entry = MakeShared<FJsonObject>();
		Entry->SetStringField(TEXT("name"), Actor->GetName());
		Entry->SetStringField(TEXT("type"), Actor->GetClass()->GetName());
		Actors.Add(MakeShared<FJsonValueObject>(Entry));
	}

	Result->SetBoolField(TEXT("found"), true);
	Result->SetNumberField(TEXT("count"), Actors.Num());
	Result->SetArrayField(TEXT("actors"), Actors);
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleGetActorDetails(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	FString ActorName;
	if (!Payload.IsValid() || !Payload->TryGetStringField(TEXT("name"), ActorName) || ActorName.IsEmpty())
	{
		Result->SetBoolField(TEXT("found"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Missing required field: name (get exact names via list_level_actors)"));
		return Result;
	}

	UWorld* World = GEditor != nullptr ? GEditor->GetEditorWorldContext().World() : nullptr;
	ULevel* Level = World != nullptr ? World->GetCurrentLevel() : nullptr;

	AActor* Actor = nullptr;
	if (Level != nullptr)
	{
		for (const TObjectPtr<AActor>& Candidate : Level->Actors)
		{
			if (IsValid(Candidate) && Candidate->GetName() == ActorName)
			{
				Actor = Candidate;
				break;
			}
		}
	}

	if (Actor == nullptr)
	{
		Result->SetBoolField(TEXT("found"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No actor named '%s' in the current level"), *ActorName));
		return Result;
	}

	Result->SetBoolField(TEXT("found"), true);
	Result->SetStringField(TEXT("name"), Actor->GetName());
	Result->SetStringField(TEXT("type"), Actor->GetClass()->GetName());

	// Properties = actor-layer properties + root component properties.
	// Dump the root component first, then the actor on top so that on any
	// duplicate key the actor layer wins.
	TSharedRef<FJsonObject> Properties = MakeShared<FJsonObject>();
	if (USceneComponent* Root = Actor->GetRootComponent())
	{
		FJsonObjectConverter::UStructToJsonObject(Root->GetClass(), Root, Properties);
	}
	FJsonObjectConverter::UStructToJsonObject(Actor->GetClass(), Actor, Properties);
	Result->SetObjectField(TEXT("properties"), Properties);

	// Component list (name + reflection type), sorted by name for stable output.
	TArray<TPair<FString, FString>> ComponentList;
	for (const UActorComponent* Component : Actor->GetComponents())
	{
		if (IsValid(Component))
		{
			ComponentList.Add(MakeTuple(Component->GetName(), Component->GetClass()->GetName()));
		}
	}
	ComponentList.Sort([](const TPair<FString, FString>& Lhs, const TPair<FString, FString>& Rhs) { return Lhs.Key < Rhs.Key; });

	TArray<TSharedPtr<FJsonValue>> Components;
	Components.Reserve(ComponentList.Num());
	for (const TPair<FString, FString>& Item : ComponentList)
	{
		TSharedRef<FJsonObject> Entry = MakeShared<FJsonObject>();
		Entry->SetStringField(TEXT("name"), Item.Key);
		Entry->SetStringField(TEXT("type"), Item.Value);
		Components.Add(MakeShared<FJsonValueObject>(Entry));
	}
	Result->SetArrayField(TEXT("components"), Components);
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleDeleteActor(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	FString ActorName;
	if (!Payload.IsValid() || !Payload->TryGetStringField(TEXT("name"), ActorName) || ActorName.IsEmpty())
	{
		Result->SetBoolField(TEXT("deleted"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Missing required field: name (get exact names via list_level_actors)"));
		return Result;
	}

	UWorld* World = GEditor != nullptr ? GEditor->GetEditorWorldContext().World() : nullptr;
	ULevel* Level = World != nullptr ? World->GetCurrentLevel() : nullptr;

	AActor* Actor = nullptr;
	if (Level != nullptr)
	{
		for (const TObjectPtr<AActor>& Candidate : Level->Actors)
		{
			if (IsValid(Candidate) && Candidate->GetName() == ActorName)
			{
				Actor = Candidate;
				break;
			}
		}
	}

	if (Actor == nullptr)
	{
		Result->SetBoolField(TEXT("deleted"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No actor named '%s' in the current level"), *ActorName));
		return Result;
	}

	// Official editor path: notifies editor subsystems and participates in undo.
	UEditorActorSubsystem* ActorSubsystem = GEditor->GetEditorSubsystem<UEditorActorSubsystem>();
	const bool bDeleted = ActorSubsystem != nullptr && ActorSubsystem->DestroyActor(Actor);

	Result->SetBoolField(TEXT("deleted"), bDeleted);
	Result->SetStringField(TEXT("name"), ActorName);
	if (!bDeleted)
	{
		Result->SetStringField(TEXT("reason"), TEXT("EditorActorSubsystem::DestroyActor rejected the actor"));
	}
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleSpawnActor(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	FString ClassName;
	if (!Payload.IsValid() || !Payload->TryGetStringField(TEXT("type"), ClassName) || ClassName.IsEmpty())
	{
		Result->SetBoolField(TEXT("spawned"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Missing required field: type (find classes via list_class_hierarchy)"));
		return Result;
	}

	// Reflection names usually drop the C++ 'A' prefix (Actor, PointLight),
	// so tolerate both spellings like the set_pie gameMode lookup does.
	UClass* ActorClass = FindFirstObject<UClass>(*ClassName);
	if (ActorClass == nullptr && ClassName.StartsWith(TEXT("A")))
	{
		ActorClass = FindFirstObject<UClass>(*(ClassName.RightChop(1)));
	}
	if (ActorClass == nullptr || !ActorClass->IsChildOf(AActor::StaticClass()) || ActorClass->HasAnyClassFlags(CLASS_Abstract))
	{
		Result->SetBoolField(TEXT("spawned"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("'%s' is not a spawnable (non-abstract) Actor class"), *ClassName));
		return Result;
	}

	// Official editor path: registers the actor with the current level and
	// notifies editor subsystems; bTransient = false keeps it saveable.
	UEditorActorSubsystem* ActorSubsystem = GEditor->GetEditorSubsystem<UEditorActorSubsystem>();
	AActor* NewActor = ActorSubsystem != nullptr
		? ActorSubsystem->SpawnActorFromClass(ActorClass, FVector::ZeroVector)
		: nullptr;

	if (NewActor == nullptr)
	{
		Result->SetBoolField(TEXT("spawned"), false);
		Result->SetStringField(TEXT("reason"), TEXT("EditorActorSubsystem::SpawnActorFromClass returned no actor"));
		return Result;
	}

	Result->SetBoolField(TEXT("spawned"), true);
	Result->SetStringField(TEXT("name"), NewActor->GetName());
	Result->SetStringField(TEXT("type"), NewActor->GetClass()->GetName());
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleSetActorProperty(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	FString ActorName;
	FString PropertyName;
	// 'name' is the canonical actor key; accept 'actor' as an alias because
	// that is the term the command name itself uses and models tend to emit it.
	if (!Payload.IsValid()
		|| (!Payload->TryGetStringField(TEXT("name"), ActorName)
			&& !Payload->TryGetStringField(TEXT("actor"), ActorName)) || ActorName.IsEmpty()
		|| !Payload->TryGetStringField(TEXT("property"), PropertyName) || PropertyName.IsEmpty()
		|| !Payload->HasField(TEXT("value")))
	{
		Result->SetBoolField(TEXT("set"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Required fields: name (the actor name from list_level_actors), property, value (property names via get_actor_details)"));
		return Result;
	}

	UWorld* World = GEditor != nullptr ? GEditor->GetEditorWorldContext().World() : nullptr;
	ULevel* Level = World != nullptr ? World->GetCurrentLevel() : nullptr;

	AActor* Actor = nullptr;
	if (Level != nullptr)
	{
		for (const TObjectPtr<AActor>& Candidate : Level->Actors)
		{
			if (IsValid(Candidate) && Candidate->GetName() == ActorName)
			{
				Actor = Candidate;
				break;
			}
		}
	}

	if (Actor == nullptr)
	{
		Result->SetBoolField(TEXT("set"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No actor named '%s' in the current level"), *ActorName));
		return Result;
	}

	// Same merge scope as get_actor_details: actor layer first, root component
	// as fallback - so any property the details dump shows is settable here.
	UObject* Owner = Actor;
	FProperty* Property = FindFProperty<FProperty>(Actor->GetClass(), *PropertyName);
	if (Property == nullptr)
	{
		if (USceneComponent* Root = Actor->GetRootComponent())
		{
			Property = FindFProperty<FProperty>(Root->GetClass(), *PropertyName);
			Owner = Root;
		}
	}
	if (Property == nullptr)
	{
		Result->SetBoolField(TEXT("set"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("Property '%s' not found on actor '%s' or its root component"), *PropertyName, *ActorName));
		return Result;
	}

	const TSharedPtr<FJsonValue> JsonValue = Payload->TryGetField(TEXT("value"));
	if (!JsonValue.IsValid())
	{
		Result->SetBoolField(TEXT("set"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Missing required field: value"));
		return Result;
	}

	void* ValueData = Property->ContainerPtrToValuePtr<void>(Owner);
	bool bImported = false;

	// FText properties get a dedicated conversion: the host only passes the
	// describing information (source text / namespace / key / string table /
	// format arguments) and the full FText - including its localization
	// identity - is assembled here. ImportText_Direct would only ever produce
	// identity-less literals, so it is bypassed for text properties.
	if (FTextProperty* TextProperty = CastField<FTextProperty>(Property))
	{
		FText NewText;
		FString TextError;
		if (!TryBuildTextFromJson(JsonValue, NewText, TextError))
		{
			Result->SetBoolField(TEXT("set"), false);
			Result->SetStringField(TEXT("reason"), TextError);
			return Result;
		}
		TextProperty->SetPropertyValue(ValueData, NewText);
		bImported = true;
	}
	else if (JsonValue->Type == EJson::Object || JsonValue->Type == EJson::Array)
	{
		// Struct/container properties don't accept JSON through ImportText, so
		// write them via the JsonUtilities converter instead.
		bImported = FJsonObjectConverter::JsonValueToUProperty(JsonValue, Property, ValueData);
	}
	else
	{
		// Scalar values go through the property text import path so that
		// strings get full property parsing. Object properties accept both a
		// bare object path (/Game/Props/SM_Chair.SM_Chair) and the exported
		// wrapper form (/Script/Engine.StaticMesh'/Game/.../SM_Chair...');
		// unloaded assets are resolved/loaded by the import machinery.
		FString ImportText;
		if (JsonValue->Type == EJson::String)
		{
			ImportText = JsonValue->AsString();
		}
		else if (JsonValue->Type == EJson::Number)
		{
			ImportText = FString::SanitizeFloat(JsonValue->AsNumber());
		}
		else if (JsonValue->Type == EJson::Boolean)
		{
			ImportText = JsonValue->AsBool() ? TEXT("true") : TEXT("false");
		}
		else
		{
			Result->SetBoolField(TEXT("set"), false);
			Result->SetStringField(TEXT("reason"), TEXT("Unsupported JSON value type for property import"));
			return Result;
		}

		const TCHAR* TextPtr = *ImportText;
		bImported = Property->ImportText_Direct(TextPtr, ValueData, Owner, PPF_PropertyWindow) != nullptr;

		if (!bImported && CastField<FObjectPropertyBase>(Property) != nullptr)
		{
			// Give asset references a clearer diagnosis than the generic
			// "cannot be assigned" - the usual culprits are a bad path, a
			// missing asset or a class that does not match the property.
			Result->SetBoolField(TEXT("set"), false);
			Result->SetStringField(TEXT("reason"), FString::Printf(
				TEXT("Object reference '%s' could not be resolved for property '%s' - check the asset path (%s) exists and its class matches"),
				*ImportText, *PropertyName, *CastField<FObjectPropertyBase>(Property)->PropertyClass->GetName()));
			return Result;
		}
	}

	if (!bImported)
	{
		Result->SetBoolField(TEXT("set"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("Value cannot be assigned to property '%s'"), *PropertyName));
		return Result;
	}

	// Editor-visible change: refresh details panel and feed the undo buffer.
	FPropertyChangedEvent ChangedEvent(Property);
	Owner->PreEditChange(Property);
	Owner->PostEditChangeProperty(ChangedEvent);

	Result->SetBoolField(TEXT("set"), true);
	Result->SetStringField(TEXT("name"), ActorName);
	Result->SetStringField(TEXT("property"), PropertyName);
	Result->SetStringField(TEXT("layer"), Owner == Actor ? TEXT("actor") : TEXT("rootComponent"));
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleCreateBlueprint(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	FString ClassName;
	FString AssetPath;
	// 'path' is the canonical asset key; accept 'assetPath' as an alias since
	// the tool's own top-level 'path' carries the .uproject instead.
	if (!Payload.IsValid()
		|| !Payload->TryGetStringField(TEXT("parentClass"), ClassName) || ClassName.IsEmpty()
		|| (!Payload->TryGetStringField(TEXT("path"), AssetPath)
			&& !Payload->TryGetStringField(TEXT("assetPath"), AssetPath)) || AssetPath.IsEmpty())
	{
		Result->SetBoolField(TEXT("created"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Required fields: parentClass (reflection class name, e.g. Actor), path (project asset path, e.g. /Game/Blueprints/BP_Thing)"));
		return Result;
	}

	// Same resolution rules as list_class_hierarchy: reflection classes carry
	// no namespace, tolerate the C++ type prefix (AActor -> Actor).
	UClass* ParentClass = FindFirstObject<UClass>(*ClassName);
	if (ParentClass == nullptr && ClassName.Len() > 1 && (ClassName[0] == 'U' || ClassName[0] == 'A' || ClassName[0] == 'F'))
	{
		ParentClass = FindFirstObject<UClass>(*ClassName.RightChop(1));
	}
	if (ParentClass == nullptr)
	{
		Result->SetBoolField(TEXT("created"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No UClass named '%s' is loaded (valid names via list_class_hierarchy)"), *ClassName));
		return Result;
	}
	if (!FKismetEditorUtilities::CanCreateBlueprintOfClass(ParentClass))
	{
		Result->SetBoolField(TEXT("created"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("Class '%s' cannot be used as a blueprint parent"), *ParentClass->GetName()));
		return Result;
	}

	// Path may arrive as /Game/Dir/BP_Name or /Game/Dir/BP_Name.BP_Name; keep
	// the package part and derive the asset name from its last element.
	FString PackagePath = AssetPath;
	int32 DotIndex = INDEX_NONE;
	if (PackagePath.FindChar(TEXT('.'), DotIndex))
	{
		PackagePath = PackagePath.Left(DotIndex);
	}
	if (!FPackageName::IsValidLongPackageName(PackagePath) || !PackagePath.StartsWith(TEXT("/Game/")))
	{
		Result->SetBoolField(TEXT("created"), false);
		Result->SetStringField(TEXT("reason"), TEXT("path must be a project content path like /Game/Blueprints/BP_Thing"));
		return Result;
	}
	const FString AssetName = FPaths::GetCleanFilename(PackagePath);

	// Refuse to clobber an existing asset - loaded package or one on disk.
	if (FindPackage(nullptr, *PackagePath) != nullptr || FPackageName::DoesPackageExist(PackagePath))
	{
		Result->SetBoolField(TEXT("created"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("An asset already exists at '%s'"), *PackagePath));
		return Result;
	}

	UPackage* Package = CreatePackage(*PackagePath);
	// Argument order follows the engine's own usage (Kismet2.cpp): blueprint
	// class first, generated class second - swapping them feeds nullptr into
	// the TSubclassOf conversion and crashes NewObject with a null class.
	UBlueprint* Blueprint = FKismetEditorUtilities::CreateBlueprint(
		ParentClass, Package, FName(*AssetName), BPTYPE_Normal,
		UBlueprint::StaticClass(), UBlueprintGeneratedClass::StaticClass());
	if (Blueprint == nullptr)
	{
		Result->SetBoolField(TEXT("created"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("Blueprint creation failed for parent '%s' at '%s'"), *ParentClass->GetName(), *PackagePath));
		return Result;
	}

	// Make it immediately usable and visible, then mark it modified and keep
	// it dirty: the dirty flag is what drives the editor's persistence - it
	// shows the modified marker in the content browser and triggers the save
	// prompt on close, which is what actually writes the .uasset to disk.
	// Without it nothing gets persisted.
	FKismetEditorUtilities::CompileBlueprint(Blueprint);
	FAssetRegistryModule::AssetCreated(Blueprint);
	Blueprint->MarkPackageDirty();

	Result->SetBoolField(TEXT("created"), true);
	Result->SetBoolField(TEXT("dirty"), Blueprint->GetPackage()->IsDirty());
	Result->SetStringField(TEXT("path"), FString::Printf(TEXT("%s.%s"), *PackagePath, *AssetName));
	Result->SetStringField(TEXT("parentClass"), ParentClass->GetName());
	return Result;
}

