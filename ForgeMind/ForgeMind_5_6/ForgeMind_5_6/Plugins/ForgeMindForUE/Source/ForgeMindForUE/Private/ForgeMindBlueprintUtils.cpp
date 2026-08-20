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

#include "ForgeMindBlueprintUtils.h"

#include "ForgeMindBridgeClient.h"

#include "Dom/JsonObject.h"
#include "EdGraphSchema_K2.h"
#include "Engine/Blueprint.h"
#include "Engine/BlueprintGeneratedClass.h"
#include "Kismet2/BlueprintEditorUtils.h"
#include "UObject/UnrealType.h"

namespace ForgeMindBlueprintUtils
{
	// JSON numbers can arrive as strings ("1.5"); accept both forms.
	static bool TryGetNumberLoose(const TSharedPtr<FJsonObject>& Object, const FString& Key, double& OutValue)
	{
		if (Object->TryGetNumberField(Key, OutValue))
		{
			return true;
		}
		FString AsText;
		if (Object->TryGetStringField(Key, AsText) && AsText.IsNumeric())
		{
			OutValue = FCString::Atod(*AsText);
			return true;
		}
		return false;
	}

	// JSON field lookup tolerant to casing (x/X, pitch/Pitch, ...).
	static bool TryGetNumberCaseInsensitive(const TSharedPtr<FJsonObject>& Object, const TArray<FString>& Casings, double& OutValue)
	{
		for (const FString& Key : Casings)
		{
			if (TryGetNumberLoose(Object, Key, OutValue))
			{
				return true;
			}
		}
		return false;
	}

	// Three components from {named fields}, [n, n, n] or a bare "a,b,c" string.
	static bool JsonToComponents3(const TSharedPtr<FJsonValue>& JsonValue, const TArray<FString>& KeyA, const TArray<FString>& KeyB, const TArray<FString>& KeyC, double* Out)
	{
		if (JsonValue->Type == EJson::Object)
		{
			const TSharedPtr<FJsonObject>& Object = JsonValue->AsObject();
			return TryGetNumberCaseInsensitive(Object, KeyA, Out[0])
				&& TryGetNumberCaseInsensitive(Object, KeyB, Out[1])
				&& TryGetNumberCaseInsensitive(Object, KeyC, Out[2]);
		}
		if (JsonValue->Type == EJson::Array)
		{
			const TArray<TSharedPtr<FJsonValue>>& Items = JsonValue->AsArray();
			if (Items.Num() != 3)
			{
				return false;
			}
			for (int32 Index = 0; Index < 3; ++Index)
			{
				if (Items[Index]->Type == EJson::Number)
				{
					Out[Index] = Items[Index]->AsNumber();
				}
				else if (Items[Index]->Type == EJson::String && Items[Index]->AsString().IsNumeric())
				{
					Out[Index] = FCString::Atod(*Items[Index]->AsString());
				}
				else
				{
					return false;
				}
			}
			return true;
		}
		if (JsonValue->Type == EJson::String)
		{
			TArray<FString> Parts;
			JsonValue->AsString().ParseIntoArray(Parts, TEXT(","), true);
			if (Parts.Num() != 3)
			{
				return false;
			}
			for (int32 Index = 0; Index < 3; ++Index)
			{
				Parts[Index].TrimStartAndEndInline();
				if (!Parts[Index].IsNumeric())
				{
					return false;
				}
				Out[Index] = FCString::Atod(*Parts[Index]);
			}
			return true;
		}
		return false;
	}

	// Delimited join - FString::Join does not exist in 5.6.
	static FString JoinStrings(const TArray<FString>& Items, const TCHAR* Delim)
	{
		FString Out;
		for (int32 Index = 0; Index < Items.Num(); ++Index)
		{
			if (Index > 0)
			{
				Out += Delim;
			}
			Out += Items[Index];
		}
		return Out;
	}

	// Warns about object keys that match no known struct member - models
	// occasionally hallucinate member names, and silently dropping them would
	// hide the mistake. Non-object inputs (arrays, engine text strings) carry
	// no member names to check.
	static void WarnUnknownMemberKeys(const TSharedPtr<FJsonValue>& JsonValue, const TArray<FString>& KnownKeys, const FString& Context, TArray<FString>& OutWarnings)
	{
		if (!JsonValue.IsValid() || JsonValue->Type != EJson::Object)
		{
			return;
		}
		for (const TPair<FString, TSharedPtr<FJsonValue>>& Pair : JsonValue->AsObject()->Values)
		{
			if (!KnownKeys.Contains(Pair.Key))
			{
				OutWarnings.Add(FString::Printf(TEXT("%s: unknown struct member '%s' ignored (valid: %s)"),
					*Context, *Pair.Key, *JoinStrings(KnownKeys, TEXT(", "))));
			}
		}
	}

	bool EnsureCompiledVariableDefault(UBlueprint* Blueprint, const FString& VarName, const FString& ExpectedDefault, FString& OutReason)
	{
		UBlueprintGeneratedClass* GeneratedClass = Cast<UBlueprintGeneratedClass>(Blueprint->GeneratedClass);
		if (GeneratedClass == nullptr)
		{
			OutReason = TEXT("the blueprint has no compiled class yet, so the default value could not be applied");
			return false;
		}
		UObject* CDO = GeneratedClass->GetDefaultObject(false);
		if (CDO == nullptr)
		{
			OutReason = TEXT("the compiled class has no default object yet, so the default value could not be applied");
			return false;
		}
		FProperty* Property = FindFProperty<FProperty>(GeneratedClass, FName(*VarName));
		if (Property == nullptr)
		{
			OutReason = FString::Printf(TEXT("compilation produced no property named '%s' on the generated class"), *VarName);
			return false;
		}

		// Same parser the compiler uses; parse the expected text into scratch
		// memory and compare it against what the CDO actually holds.
		const int32 PropertySize = Property->GetSize();
		uint8* ParsedValue = static_cast<uint8*>(FMemory::Malloc(PropertySize));
		uint8* ActualValue = static_cast<uint8*>(FMemory::Malloc(PropertySize));
		Property->InitializeValue(ParsedValue);
		Property->InitializeValue(ActualValue);
		const bool bParsed = FBlueprintEditorUtils::PropertyValueFromString_Direct(Property, ExpectedDefault, ParsedValue, CDO);
		if (!bParsed)
		{
			FMemory::Free(ParsedValue);
			FMemory::Free(ActualValue);
			OutReason = FString::Printf(TEXT("default '%s' could not be parsed for variable '%s' of type '%s'"), *ExpectedDefault, *VarName, *Property->GetClass()->GetName());
			return false;
		}
		Property->CopyCompleteValue(ActualValue, Property->ContainerPtrToValuePtr<const void>(CDO));
		const bool bAlreadyApplied = Property->Identical(ParsedValue, ActualValue);
		if (!bAlreadyApplied)
		{
			// The compiler left the property untouched - write the value into the
			// CDO ourselves so the default still takes effect.
			Property->CopyCompleteValue(Property->ContainerPtrToValuePtr<void>(CDO), ParsedValue);
			Blueprint->MarkPackageDirty();
		}
		FMemory::Free(ParsedValue);
		FMemory::Free(ActualValue);
		return true;
	}

	bool BlueprintDefaultFromJsonValue(const TSharedPtr<FJsonValue>& DefaultJson, const FEdGraphPinType& PinType, FString& OutDefault, FString& OutReason, TArray<FString>& OutWarnings)
	{
		if (!DefaultJson.IsValid())
		{
			OutReason = TEXT("'default' must not be null");
			return false;
		}

		const UScriptStruct* Struct = (PinType.PinCategory == UEdGraphSchema_K2::PC_Struct)
			? Cast<UScriptStruct>(PinType.PinSubCategoryObject.Get()) : nullptr;

		if (Struct != nullptr)
		{
			// String defaults pass through untouched - they may already use an
			// engine text form like "1,2,3" or "tx,ty,tz|p,y,r|sx,sy,sz".
			if (DefaultJson->Type == EJson::String)
			{
				OutDefault = DefaultJson->AsString();
				return true;
			}

			double Components[3];
			if (Struct == TBaseStructure<FVector>::Get())
			{
				WarnUnknownMemberKeys(DefaultJson, { TEXT("X"), TEXT("x"), TEXT("Y"), TEXT("y"), TEXT("Z"), TEXT("z") }, TEXT("vector default"), OutWarnings);
				if (!JsonToComponents3(DefaultJson, { TEXT("X"), TEXT("x") }, { TEXT("Y"), TEXT("y") }, { TEXT("Z"), TEXT("z") }, Components))
				{
					OutReason = TEXT("a vector default must be {\"X\":x,\"Y\":y,\"Z\":z}, [x,y,z] or \"x,y,z\"");
					return false;
				}
				OutDefault = FString::Printf(TEXT("%f,%f,%f"), Components[0], Components[1], Components[2]);
				return true;
			}
			if (Struct == TBaseStructure<FRotator>::Get())
			{
				// ParseRotator maps the three comma components onto Pitch,Yaw,Roll.
				WarnUnknownMemberKeys(DefaultJson, { TEXT("Pitch"), TEXT("pitch"), TEXT("Yaw"), TEXT("yaw"), TEXT("Roll"), TEXT("roll") }, TEXT("rotator default"), OutWarnings);
				if (!JsonToComponents3(DefaultJson, { TEXT("Pitch"), TEXT("pitch") }, { TEXT("Yaw"), TEXT("yaw") }, { TEXT("Roll"), TEXT("roll") }, Components))
				{
					OutReason = TEXT("a rotator default must be {\"Pitch\":p,\"Yaw\":y,\"Roll\":r}, [p,y,r] or \"p,y,r\"");
					return false;
				}
				OutDefault = FString::Printf(TEXT("%f,%f,%f"), Components[0], Components[1], Components[2]);
				return true;
			}
			if (Struct == TBaseStructure<FTransform>::Get())
			{
				// FTransform::InitFromString wants "translation|rotation|scale".
				if (DefaultJson->Type != EJson::Object)
				{
					OutReason = TEXT("a transform default must be {\"Translation\":{X,Y,Z},\"Rotation\":{Pitch,Yaw,Roll},\"Scale\":{X,Y,Z}} or the \"tx,ty,tz|p,y,r|sx,sy,sz\" string form");
					return false;
				}
				const TSharedPtr<FJsonObject>& Object = DefaultJson->AsObject();
				WarnUnknownMemberKeys(DefaultJson, { TEXT("Translation"), TEXT("translation"), TEXT("Rotation"), TEXT("rotation"), TEXT("Scale"), TEXT("scale") }, TEXT("transform default"), OutWarnings);
				FVector Translation = FVector::ZeroVector;
				FRotator Rotation = FRotator::ZeroRotator;
				FVector Scale = FVector::OneVector;
				// FindRef returns a value copy (invalid share ptr when absent).
				TSharedPtr<FJsonValue> SubValue = Object->Values.FindRef(TEXT("Translation"));
				if (!SubValue.IsValid())
				{
					SubValue = Object->Values.FindRef(TEXT("translation"));
				}
				if (SubValue.IsValid())
				{
					WarnUnknownMemberKeys(SubValue, { TEXT("X"), TEXT("x"), TEXT("Y"), TEXT("y"), TEXT("Z"), TEXT("z") }, TEXT("transform Translation"), OutWarnings);
					if (!JsonToComponents3(SubValue, { TEXT("X"), TEXT("x") }, { TEXT("Y"), TEXT("y") }, { TEXT("Z"), TEXT("z") }, Components))
					{
						OutReason = TEXT("transform 'Translation' must be {X,Y,Z}, [x,y,z] or \"x,y,z\"");
						return false;
					}
					Translation.Set(Components[0], Components[1], Components[2]);
				}
				SubValue = Object->Values.FindRef(TEXT("Rotation"));
				if (!SubValue.IsValid())
				{
					SubValue = Object->Values.FindRef(TEXT("rotation"));
				}
				if (SubValue.IsValid())
				{
					WarnUnknownMemberKeys(SubValue, { TEXT("Pitch"), TEXT("pitch"), TEXT("Yaw"), TEXT("yaw"), TEXT("Roll"), TEXT("roll") }, TEXT("transform Rotation"), OutWarnings);
					if (!JsonToComponents3(SubValue, { TEXT("Pitch"), TEXT("pitch") }, { TEXT("Yaw"), TEXT("yaw") }, { TEXT("Roll"), TEXT("roll") }, Components))
					{
						OutReason = TEXT("transform 'Rotation' must be {Pitch,Yaw,Roll}, [p,y,r] or \"p,y,r\"");
						return false;
					}
					Rotation = FRotator(Components[0], Components[1], Components[2]);
				}
				SubValue = Object->Values.FindRef(TEXT("Scale"));
				if (!SubValue.IsValid())
				{
					SubValue = Object->Values.FindRef(TEXT("scale"));
				}
				if (SubValue.IsValid())
				{
					WarnUnknownMemberKeys(SubValue, { TEXT("X"), TEXT("x"), TEXT("Y"), TEXT("y"), TEXT("Z"), TEXT("z") }, TEXT("transform Scale"), OutWarnings);
					if (!JsonToComponents3(SubValue, { TEXT("X"), TEXT("x") }, { TEXT("Y"), TEXT("y") }, { TEXT("Z"), TEXT("z") }, Components))
					{
						OutReason = TEXT("transform 'Scale' must be {X,Y,Z}, [x,y,z] or \"x,y,z\"");
						return false;
					}
					Scale.Set(Components[0], Components[1], Components[2]);
				}
				OutDefault = FTransform(Rotation, Translation, Scale).ToString();
				return true;
			}

			// Generic struct: build the property text form (Prop=Value,...),
			// which is what UScriptStruct::ImportText parses.
			if (DefaultJson->Type != EJson::Object)
			{
				OutReason = FString::Printf(TEXT("a '%s' struct default must be an object with one numeric field per struct member, or an engine property text string"), *Struct->GetName());
				return false;
			}
			FString Text = TEXT("(");
			bool bFirst = true;
			for (const TPair<FString, TSharedPtr<FJsonValue>>& Pair : DefaultJson->AsObject()->Values)
			{
				// Skip members the struct does not have (case-sensitive, like the
				// engine's ImportText matching) - warned, not silently dropped.
				if (Struct->FindPropertyByName(FName(*Pair.Key)) == nullptr)
				{
					TArray<FString> ValidNames;
					for (TFieldIterator<FProperty> MemberIt(Struct); MemberIt; ++MemberIt)
					{
						ValidNames.Add(MemberIt->GetName());
					}
					OutWarnings.Add(FString::Printf(TEXT("'%s' struct default: unknown member '%s' ignored (valid: %s)"),
						*Struct->GetName(), *Pair.Key, *JoinStrings(ValidNames, TEXT(", "))));
					continue;
				}
				FString Member;
				switch (Pair.Value.IsValid() ? Pair.Value->Type : EJson::None)
				{
				case EJson::Number:
					Member = FString::Printf(TEXT("%g"), Pair.Value->AsNumber());
					break;
				case EJson::Boolean:
					Member = Pair.Value->AsBool() ? TEXT("true") : TEXT("false");
					break;
				case EJson::String:
					Member = Pair.Value->AsString();
					break;
				default:
					OutReason = FString::Printf(TEXT("struct member '%s' must be a scalar value"), *Pair.Key);
					return false;
				}
				Text += FString::Printf(TEXT("%s%s=%s"), bFirst ? TEXT("") : TEXT(","), *Pair.Key, *Member);
				bFirst = false;
			}
			OutDefault = Text + TEXT(")");
			return true;
		}

		// Non-struct variables take scalar defaults only.
		switch (DefaultJson->Type)
		{
		case EJson::String:
			OutDefault = DefaultJson->AsString();
			// Integer pins need a plain integer literal - models often quote
			// numbers, and values like "1e9" or "5.0" would fail ParseInt64.
			if ((PinType.PinCategory == UEdGraphSchema_K2::PC_Int
				|| PinType.PinCategory == UEdGraphSchema_K2::PC_Int64
				|| PinType.PinCategory == UEdGraphSchema_K2::PC_Byte)
				&& OutDefault.IsNumeric())
			{
				// Plain integer literals round-trip exactly through Atoi64 (needed
				// for int64 values beyond double precision); anything else is
				// rounded via double since ParseInt64 would reject it outright.
				const bool bPlainInteger = !OutDefault.Contains(TEXT(".")) && !OutDefault.Contains(TEXT("e")) && !OutDefault.Contains(TEXT("E"));
				OutDefault = bPlainInteger
					? FString::Printf(TEXT("%lld"), FCString::Atoi64(*OutDefault))
					: FString::Printf(TEXT("%lld"), static_cast<int64>(FCString::Atod(*OutDefault)));
			}
			return true;
		case EJson::Boolean:
			OutDefault = DefaultJson->AsBool() ? TEXT("true") : TEXT("false");
			return true;
		case EJson::Number:
			// Integer pins need a plain integer literal: FDefaultValueHelper::
			// ParseInt64 rejects scientific notation, and %g switches to it (and
			// loses precision) for large int64 values.
			if (PinType.PinCategory == UEdGraphSchema_K2::PC_Int
				|| PinType.PinCategory == UEdGraphSchema_K2::PC_Int64
				|| PinType.PinCategory == UEdGraphSchema_K2::PC_Byte)
			{
				OutDefault = FString::Printf(TEXT("%lld"), static_cast<int64>(DefaultJson->AsNumber()));
			}
			else if (PinType.PinCategory == UEdGraphSchema_K2::PC_Real
				&& PinType.PinSubCategory == UEdGraphSchema_K2::PC_Double)
			{
				// doubles need 17 significant digits to survive the JSON round trip.
				OutDefault = FString::Printf(TEXT("%.17g"), DefaultJson->AsNumber());
			}
			else
			{
				OutDefault = FString::Printf(TEXT("%g"), DefaultJson->AsNumber());
			}
			return true;
		default:
			OutReason = TEXT("'default' must be a scalar value (string, number or boolean) for this variable type");
			return false;
		}
	}

	// Surfaces non-fatal warnings (unknown struct members) in the response and
	// the editor log so hallucinated member names get corrected instead of
	// silently dropped.
	void AppendWarningsToResult(const TSharedPtr<FJsonObject>& Result, const TArray<FString>& Warnings)
	{
		if (Warnings.Num() == 0)
		{
			return;
		}
		TArray<TSharedPtr<FJsonValue>> WarningValues;
		WarningValues.Reserve(Warnings.Num());
		for (const FString& Warning : Warnings)
		{
			UE_LOG(LogForgeMindBridge, Warning, TEXT("%s"), *Warning);
			WarningValues.Add(MakeShared<FJsonValueString>(Warning));
		}
		Result->SetArrayField(TEXT("warnings"), WarningValues);
	}

	// Optional key readers for update_blueprint_variable: presence of the key
	// decides whether the setting changes; a wrong-typed value is rejected.
	bool TryGetOptionalBool(const TSharedPtr<FJsonObject>& Payload, const TCHAR* Key, bool& OutValue, bool& bPresent, FString& OutReason)
	{
		bPresent = Payload->HasField(Key);
		if (bPresent && !Payload->TryGetBoolField(Key, OutValue))
		{
			OutReason = FString::Printf(TEXT("'%s' must be a boolean"), Key);
			return false;
		}
		return true;
	}

	bool TryGetOptionalString(const TSharedPtr<FJsonObject>& Payload, const TCHAR* Key, FString& OutValue, bool& bPresent, FString& OutReason)
	{
		bPresent = Payload->HasField(Key);
		if (bPresent && !Payload->TryGetStringField(Key, OutValue))
		{
			OutReason = FString::Printf(TEXT("'%s' must be a string"), Key);
			return false;
		}
		return true;
	}

	// Replication condition text -> enum. Accepts the engine identifiers with or
	// without the COND_ prefix (case-insensitive); hidden conditions (Dynamic,
	// Never, NetGroup) and COND_Max are refused.
	bool ParseReplicationCondition(const FString& InText, ELifetimeCondition& OutCondition, FString& OutReason)
	{
		FString Name = InText.TrimStartAndEnd().ToLower();
		if (Name.StartsWith(TEXT("cond_")))
		{
			Name = Name.RightChop(5);
		}

		static const struct { const TCHAR* Name; ELifetimeCondition Condition; } Conditions[] =
		{
			{ TEXT("none"), COND_None },
			{ TEXT("always"), COND_None },
			{ TEXT("initialonly"), COND_InitialOnly },
			{ TEXT("owneronly"), COND_OwnerOnly },
			{ TEXT("skipowner"), COND_SkipOwner },
			{ TEXT("simulatedonly"), COND_SimulatedOnly },
			{ TEXT("autonomousonly"), COND_AutonomousOnly },
			{ TEXT("simulatedorphysics"), COND_SimulatedOrPhysics },
			{ TEXT("initialorowner"), COND_InitialOrOwner },
			{ TEXT("custom"), COND_Custom },
			{ TEXT("replayorowner"), COND_ReplayOrOwner },
			{ TEXT("replayonly"), COND_ReplayOnly },
			{ TEXT("simulatedonlynoreplay"), COND_SimulatedOnlyNoReplay },
			{ TEXT("simulatedorphysicsnoreplay"), COND_SimulatedOrPhysicsNoReplay },
			{ TEXT("skipreplay"), COND_SkipReplay },
		};
		for (const auto& Entry : Conditions)
		{
			if (Name == Entry.Name)
			{
				OutCondition = Entry.Condition;
				return true;
			}
		}
		OutReason = FString::Printf(TEXT("unknown replicationCondition '%s' (expected e.g. none, initialOnly, ownerOnly, skipOwner, simulatedOnly, autonomousOnly, replayOnly)"), *InText);
		return false;
	}

	// Inverse of ParseReplicationCondition: enum -> the same camelCase names
	// update_blueprint_variable accepts, so reported values round-trip.
	const TCHAR* ReplicationConditionToText(const ELifetimeCondition Condition)
	{
		switch (Condition)
		{
		case COND_InitialOnly: return TEXT("initialOnly");
		case COND_OwnerOnly: return TEXT("ownerOnly");
		case COND_SkipOwner: return TEXT("skipOwner");
		case COND_SimulatedOnly: return TEXT("simulatedOnly");
		case COND_AutonomousOnly: return TEXT("autonomousOnly");
		case COND_SimulatedOrPhysics: return TEXT("simulatedOrPhysics");
		case COND_InitialOrOwner: return TEXT("initialOrOwner");
		case COND_Custom: return TEXT("custom");
		case COND_ReplayOrOwner: return TEXT("replayOrOwner");
		case COND_ReplayOnly: return TEXT("replayOnly");
		case COND_SimulatedOnlyNoReplay: return TEXT("simulatedOnlyNoReplay");
		case COND_SimulatedOrPhysicsNoReplay: return TEXT("simulatedOrPhysicsNoReplay");
		case COND_SkipReplay: return TEXT("skipReplay");
		case COND_None:
		default: return TEXT("none");
		}
	}

	// Sets or clears a single bit when the corresponding payload key was
	// present; returns whether the key was applied.
	bool ApplyBoolFlag(uint64& PropertyFlags, const uint64 Flag, const bool bEnable, const bool bPresent)
	{
		if (!bPresent)
		{
			return false;
		}
		if (bEnable)
		{
			PropertyFlags |= Flag;
		}
		else
		{
			PropertyFlags &= ~Flag;
		}
		return true;
	}

	// Renders an FEdGraphPinType with the same vocabulary add_blueprint_variable
	// accepts ('float', 'double', 'vector', ... or a class name for object
	// references), so a reported type can be fed straight back into add.
	FString PinTypeToText(const FEdGraphPinType& PinType)
	{
		FString Base;
		if (PinType.PinCategory == UEdGraphSchema_K2::PC_Boolean)
		{
			Base = TEXT("bool");
		}
		else if (PinType.PinCategory == UEdGraphSchema_K2::PC_Byte)
		{
			const UEnum* Enum = Cast<UEnum>(PinType.PinSubCategoryObject.Get());
			Base = (Enum != nullptr) ? FString::Printf(TEXT("byte<%s>"), *Enum->GetName()) : TEXT("byte");
		}
		else if (PinType.PinCategory == UEdGraphSchema_K2::PC_Int)
		{
			Base = TEXT("int");
		}
		else if (PinType.PinCategory == UEdGraphSchema_K2::PC_Int64)
		{
			Base = TEXT("int64");
		}
		else if (PinType.PinCategory == UEdGraphSchema_K2::PC_Real)
		{
			// PC_Real carries the precision in the subcategory (see the
			// CreatePrimitiveProperty note in HandleAddBlueprintVariable).
			Base = (PinType.PinSubCategory == UEdGraphSchema_K2::PC_Float) ? TEXT("float") : TEXT("double");
		}
		else if (PinType.PinCategory == UEdGraphSchema_K2::PC_Name)
		{
			Base = TEXT("name");
		}
		else if (PinType.PinCategory == UEdGraphSchema_K2::PC_String)
		{
			Base = TEXT("string");
		}
		else if (PinType.PinCategory == UEdGraphSchema_K2::PC_Text)
		{
			Base = TEXT("text");
		}
		else if (PinType.PinCategory == UEdGraphSchema_K2::PC_Struct)
		{
			const UScriptStruct* Struct = Cast<UScriptStruct>(PinType.PinSubCategoryObject.Get());
			if (Struct == TBaseStructure<FVector>::Get()) { Base = TEXT("vector"); }
			else if (Struct == TBaseStructure<FRotator>::Get()) { Base = TEXT("rotator"); }
			else if (Struct == TBaseStructure<FTransform>::Get()) { Base = TEXT("transform"); }
			else { Base = FString::Printf(TEXT("struct<%s>"), (Struct != nullptr) ? *Struct->GetName() : TEXT("?")); }
		}
		else if (PinType.PinCategory == UEdGraphSchema_K2::PC_Object)
		{
			const UClass* ObjectClass = Cast<UClass>(PinType.PinSubCategoryObject.Get());
			Base = (ObjectClass != nullptr) ? ObjectClass->GetName() : TEXT("object");
		}
		else
		{
			Base = PinType.PinCategory.ToString();
		}

		switch (PinType.ContainerType)
		{
		case EPinContainerType::Array: return Base + TEXT("[]");
		case EPinContainerType::Set: return FString::Printf(TEXT("set<%s>"), *Base);
		case EPinContainerType::Map:
		{
			// The map key arrives as a terminal type; wrap it for recursion.
			FEdGraphPinType KeyType;
			KeyType.PinCategory = PinType.PinValueType.TerminalCategory;
			KeyType.PinSubCategory = PinType.PinValueType.TerminalSubCategory;
			KeyType.PinSubCategoryObject = PinType.PinValueType.TerminalSubCategoryObject;
			return FString::Printf(TEXT("map<%s,%s>"), *PinTypeToText(KeyType), *Base);
		}
		case EPinContainerType::None:
		default: return Base;
		}
	}

	// Type text for an already-compiled or native property (inherited variables),
	// using the same vocabulary as PinTypeToText.
	FString PropertyTypeToText(const FProperty* Property)
	{
		// Unwrap containers first so the suffixes match PinTypeToText.
		const FProperty* Value = Property;
		int32 Container = 0; // 0 = none, 1 = array, 2 = set, 3 = map
		FString MapKeyText;
		if (const FArrayProperty* ArrayProp = CastField<FArrayProperty>(Property)) { Value = ArrayProp->Inner; Container = 1; }
		else if (const FSetProperty* SetProp = CastField<FSetProperty>(Property)) { Value = SetProp->ElementProp; Container = 2; }
		else if (const FMapProperty* MapProp = CastField<FMapProperty>(Property)) { Value = MapProp->ValueProp; MapKeyText = PropertyTypeToText(MapProp->KeyProp); Container = 3; }

		FString Base;
		if (CastField<FBoolProperty>(Value) != nullptr)
		{
			Base = TEXT("bool");
		}
		else if (const FByteProperty* ByteProp = CastField<FByteProperty>(Value))
		{
			Base = (ByteProp->Enum != nullptr) ? FString::Printf(TEXT("byte<%s>"), *ByteProp->Enum->GetName()) : TEXT("byte");
		}
		else if (CastField<FIntProperty>(Value) != nullptr)
		{
			Base = TEXT("int");
		}
		else if (CastField<FInt64Property>(Value) != nullptr)
		{
			Base = TEXT("int64");
		}
		else if (CastField<FFloatProperty>(Value) != nullptr)
		{
			Base = TEXT("float");
		}
		else if (CastField<FDoubleProperty>(Value) != nullptr)
		{
			Base = TEXT("double");
		}
		else if (CastField<FNameProperty>(Value) != nullptr)
		{
			Base = TEXT("name");
		}
		else if (CastField<FStrProperty>(Value) != nullptr)
		{
			Base = TEXT("string");
		}
		else if (CastField<FTextProperty>(Value) != nullptr)
		{
			Base = TEXT("text");
		}
		else if (const FEnumProperty* EnumProp = CastField<FEnumProperty>(Value))
		{
			Base = FString::Printf(TEXT("byte<%s>"), (EnumProp->GetEnum() != nullptr) ? *EnumProp->GetEnum()->GetName() : TEXT("?"));
		}
		else if (const FStructProperty* StructProp = CastField<FStructProperty>(Value))
		{
			const UScriptStruct* Struct = StructProp->Struct;
			if (Struct == TBaseStructure<FVector>::Get()) { Base = TEXT("vector"); }
			else if (Struct == TBaseStructure<FRotator>::Get()) { Base = TEXT("rotator"); }
			else if (Struct == TBaseStructure<FTransform>::Get()) { Base = TEXT("transform"); }
			else { Base = FString::Printf(TEXT("struct<%s>"), (Struct != nullptr) ? *Struct->GetName() : TEXT("?")); }
		}
		else if (const FSoftObjectProperty* SoftProp = CastField<FSoftObjectProperty>(Value))
		{
			Base = FString::Printf(TEXT("soft<%s>"), (SoftProp->PropertyClass != nullptr) ? *SoftProp->PropertyClass->GetName() : TEXT("object"));
		}
		else if (const FObjectPropertyBase* ObjProp = CastField<FObjectPropertyBase>(Value))
		{
			Base = (ObjProp->PropertyClass != nullptr) ? ObjProp->PropertyClass->GetName() : TEXT("object");
		}
		else
		{
			Base = Value->GetClass()->GetName();
		}

		switch (Container)
		{
		case 1: return Base + TEXT("[]");
		case 2: return FString::Printf(TEXT("set<%s>"), *Base);
		case 3: return FString::Printf(TEXT("map<%s,%s>"), *MapKeyText, *Base);
		default: return Base;
		}
	}

	// The settings block shared by every listed variable - it mirrors the
	// update_blueprint_variable key set, so reported values can be fed straight
	// back into update (own variables only, inherited ones are read-only).
	void AddVariableSettingsToJson(FJsonObject& Entry, const uint64 PropertyFlags, const ELifetimeCondition Condition,
		const bool bPrivate, const bool bExposeOnSpawn, const FString& DeprecatedMessage, const bool bHasDeprecatedMessage,
		const FName RepNotifyFunc)
	{
		Entry.SetBoolField(TEXT("instanceEditable"), (PropertyFlags & CPF_DisableEditOnInstance) == 0);
		Entry.SetBoolField(TEXT("private"), bPrivate);
		Entry.SetBoolField(TEXT("config"), (PropertyFlags & CPF_Config) != 0);
		Entry.SetBoolField(TEXT("transient"), (PropertyFlags & CPF_Transient) != 0);
		Entry.SetBoolField(TEXT("saveGame"), (PropertyFlags & CPF_SaveGame) != 0);
		Entry.SetBoolField(TEXT("advancedDisplay"), (PropertyFlags & CPF_AdvancedDisplay) != 0);
		Entry.SetBoolField(TEXT("deprecated"), (PropertyFlags & CPF_Deprecated) != 0);
		if (bHasDeprecatedMessage)
		{
			Entry.SetStringField(TEXT("deprecatedMessage"), DeprecatedMessage);
		}
		Entry.SetBoolField(TEXT("exposeOnSpawn"), bExposeOnSpawn);
		Entry.SetBoolField(TEXT("exposeToCinematics"), (PropertyFlags & CPF_Interp) != 0);

		if (PropertyFlags & CPF_RepNotify)
		{
			Entry.SetStringField(TEXT("replication"), TEXT("repNotify"));
		}
		else if (PropertyFlags & CPF_Net)
		{
			Entry.SetStringField(TEXT("replication"), TEXT("replicated"));
		}
		else
		{
			Entry.SetStringField(TEXT("replication"), TEXT("none"));
		}
		if (RepNotifyFunc != NAME_None)
		{
			Entry.SetStringField(TEXT("repNotifyFunc"), RepNotifyFunc.ToString());
		}
		Entry.SetStringField(TEXT("replicationCondition"), ReplicationConditionToText(Condition));
	}

	// Entry built from a variable description (the blueprint's own variables and
	// those of blueprint parents - the descriptions are what the editor's
	// variable details panel edits). The default however is read off the
	// generated class CDO: a full compile moves the staged default onto the
	// CDO and empties FBPVariableDescription::DefaultValue (KismetCompiler.cpp),
	// so the CDO is the source of truth once compiled; the staged text only
	// survives for variables that haven't seen a full compile yet.
	TSharedRef<FJsonObject> BlueprintVariableToJson(const FBPVariableDescription& Variable, const UClass* GeneratedClass)
	{
		TSharedRef<FJsonObject> Entry = MakeShared<FJsonObject>();
		Entry->SetStringField(TEXT("name"), Variable.VarName.ToString());
		Entry->SetStringField(TEXT("type"), PinTypeToText(Variable.VarType));

		FString DefaultText = Variable.DefaultValue;
		if (GeneratedClass != nullptr)
		{
			if (const FProperty* Property = FindFProperty<FProperty>(GeneratedClass, Variable.VarName))
			{
				FBlueprintEditorUtils::PropertyValueToString(Property, reinterpret_cast<const uint8*>(GeneratedClass->GetDefaultObject()), DefaultText);
			}
		}
		Entry->SetStringField(TEXT("defaultValue"), DefaultText);
		Entry->SetStringField(TEXT("category"), Variable.Category.ToString());

		FString DeprecatedMessage;
		const bool bHasDeprecatedMessage = Variable.HasMetaData(FBlueprintMetadata::MD_DeprecationMessage);
		if (bHasDeprecatedMessage)
		{
			DeprecatedMessage = Variable.GetMetaData(FBlueprintMetadata::MD_DeprecationMessage);
		}
		AddVariableSettingsToJson(*Entry, Variable.PropertyFlags, Variable.ReplicationCondition,
			Variable.HasMetaData(FBlueprintMetadata::MD_Private),
			Variable.HasMetaData(FBlueprintMetadata::MD_ExposeOnSpawn),
			DeprecatedMessage, bHasDeprecatedMessage, Variable.RepNotifyFunc);
		return Entry;
	}

	// Entry built from a compiled property (variables inherited from C++ parent
	// classes); the default is read off the owner's CDO in the same text form
	// PropertyValueFromString parses.
	TSharedRef<FJsonObject> NativePropertyToJson(const FProperty* Property, const UClass* OwnerClass)
	{
		TSharedRef<FJsonObject> Entry = MakeShared<FJsonObject>();
		Entry->SetStringField(TEXT("name"), Property->GetName());
		Entry->SetStringField(TEXT("type"), PropertyTypeToText(Property));

		FString DefaultText;
		if (OwnerClass != nullptr)
		{
			FBlueprintEditorUtils::PropertyValueToString(Property, reinterpret_cast<const uint8*>(OwnerClass->GetDefaultObject()), DefaultText);
		}
		Entry->SetStringField(TEXT("defaultValue"), DefaultText);

		if (Property->HasMetaData(TEXT("Category")))
		{
			Entry->SetStringField(TEXT("category"), Property->GetMetaData(TEXT("Category")));
		}

		FString DeprecatedMessage;
		const bool bHasDeprecatedMessage = Property->HasMetaData(FBlueprintMetadata::MD_DeprecationMessage);
		if (bHasDeprecatedMessage)
		{
			DeprecatedMessage = Property->GetMetaData(FBlueprintMetadata::MD_DeprecationMessage);
		}
		AddVariableSettingsToJson(*Entry, Property->PropertyFlags, Property->GetBlueprintReplicationCondition(),
			Property->HasMetaData(FBlueprintMetadata::MD_Private),
			Property->HasMetaData(FBlueprintMetadata::MD_ExposeOnSpawn) || Property->HasAnyPropertyFlags(CPF_ExposeOnSpawn),
			DeprecatedMessage, bHasDeprecatedMessage, Property->RepNotifyFunc);
		return Entry;
	}
}