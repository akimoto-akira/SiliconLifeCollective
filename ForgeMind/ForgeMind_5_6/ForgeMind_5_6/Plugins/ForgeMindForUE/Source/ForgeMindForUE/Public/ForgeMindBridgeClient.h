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

#pragma once

#include "CoreMinimal.h"
#include "Dom/JsonObject.h"

/** Shared by every bridge implementation file (defined in ForgeMindBridgeClient.cpp). */
DECLARE_LOG_CATEGORY_EXTERN(LogForgeMindBridge, Log, All);

class FSocket;

/**
 * TCP bridge client connecting this editor to the SiliconLife host's
 * ForgeMind bridge server.
 *
 * Discovery: reads %USERPROFILE%/.forgemind/bridge.json ({port, token, pid}).
 * Protocol: 4-byte little-endian length prefix + UTF-8 JSON envelope
 * ({v, type, id, name, payload, error}). Flow: connect -> auth -> handshake
 * -> message loop. Host-originated requests are dispatched to command
 * handlers on the game thread; responses come back synchronously.
 *
 * Heartbeat is host-driven: the host pings periodically and this client
 * answers; a dead host closes our socket via its own sweep, triggering
 * reconnection with exponential backoff.
 */
class FForgeMindBridgeClient
{
public:
	FForgeMindBridgeClient();
	~FForgeMindBridgeClient();

	/** Starts the connect retry timer (call from StartupModule). */
	void Start();

	/** Stops all threads and closes the socket (call from ShutdownModule). */
	void Stop();

	/** Drains game-thread work: queued host messages + connect retries. */
	void Tick(float DeltaTime);

	/** FEditorDelegates::OnEditorPreExit callback - best-effort closing notice, then teardown. */
	void OnPreExit();

private:
	/** Values parsed from bridge.json. */
	struct FDiscovery
	{
		int32 Port = 0;
		FString Token;
		bool bValid = false;
	};

	void TryConnect();
	bool ConnectOnce();
	FDiscovery ReadDiscovery() const;
	void StartMessageThreads();
	void HandleDisconnect();
	uint32 ReceiveLoop();
	uint32 SendLoop();
	bool RecvFull(uint8* Buffer, int32 Size);
	bool SendFrame(const FString& Json);
	void ProcessRequest(TSharedRef<FJsonObject> Message);
	void ProcessEvent(TSharedRef<FJsonObject> Message);
	void QueueSend(const FString& Json);

	// Command handlers - always executed on the game thread.
	TSharedPtr<FJsonObject> HandlePing(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleGetStatus(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleOpenAsset(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleExecConsole(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleQuitEditor(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleListClassHierarchy(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleGetCurrentLevel(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleSetPie(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleListLevelActors(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleGetActorDetails(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleDeleteActor(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleSpawnActor(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleSetActorProperty(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleCreateBlueprint(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleAddBlueprintVariable(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleSetBlueprintVariableDefault(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleUpdateBlueprintVariable(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleListBlueprintVariables(const TSharedPtr<FJsonObject>& Payload);
	TSharedPtr<FJsonObject> HandleRemoveBlueprintVariable(const TSharedPtr<FJsonObject>& Payload);

	TMap<FString, TFunction<TSharedPtr<FJsonObject>(const TSharedPtr<FJsonObject>&)>> RequestHandlers;
	TArray<FString> AdvertisedCommands;

	FSocket* Socket = nullptr;
	bool bConnected = false;
	bool bStopping = false;
	bool bStopped = false;
	FString Token;

	/** Set by a worker thread when the connection drops; drained in Tick. */
	FThreadSafeBool bConnectionLost;

	/** Set by quit_editor; drained in Tick so the acknowledgement is flushed first. */
	FThreadSafeBool bQuitRequested;

	// Exponential backoff between connect attempts (1s .. 60s).
	double NextAttemptTime = 0.0;
	double BackoffSeconds = 1.0;

	// Keepalive: host sweeps sessions silent for 90s, ping every 30s.
	double LastKeepaliveTime = 0.0;

	TSharedPtr<FRunnableThread> ReceiveThread;
	TSharedPtr<FRunnableThread> SendThread;

	// Outbound queue (any thread -> send thread).
	FCriticalSection SendQueueLock;
	FThreadSafeBool SendQueueSignal;
	TArray<FString> SendQueue;

	// Inbound queue (receive thread -> game thread).
	FCriticalSection GameQueueLock;
	TArray<TSharedRef<FJsonObject>> GameQueue;

	FDelegateHandle PreExitHandle;
};
