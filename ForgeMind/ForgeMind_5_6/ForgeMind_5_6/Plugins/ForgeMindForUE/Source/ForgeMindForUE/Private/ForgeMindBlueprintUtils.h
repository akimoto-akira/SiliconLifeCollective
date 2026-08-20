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
#include "Dom/JsonValue.h"
#include "EdGraph/EdGraphPin.h"
#include "UObject/CoreNetTypes.h"

class UBlueprint;
class FProperty;
class UClass;
class FJsonObject;
struct FBPVariableDescription;

/**
 * Helpers shared by the blueprint-variable bridge commands
 * (add/set_default/update/list/remove_blueprint_variable). The text
 * vocabularies produced and consumed here are mutually compatible:
 * type text round-trips through add_blueprint_variable, replication
 * condition text through update_blueprint_variable and default value
 * text through set_blueprint_variable_default.
 */
namespace ForgeMindBlueprintUtils
{
	/**
	 * Verifies a compiled variable's default really landed in the CDO and
	 * writes it there when the compiler left the property untouched
	 * (the same write path the Details panel uses for existing variables).
	 */
	bool EnsureCompiledVariableDefault(UBlueprint* Blueprint, const FString& VarName, const FString& ExpectedDefault, FString& OutReason);

	/**
	 * Converts a JSON default value into the text form the blueprint
	 * compiler expects (engine-verified): struct defaults are parsed by
	 * FBlueprintEditorUtils::PropertyValueFromString, which for FVector/
	 * FRotator wants comma separated components (rotator order
	 * Pitch,Yaw,Roll), for FTransform the pipe separated
	 * "tx,ty,tz|pitch,yaw,roll|sx,sy,sz" form and for any other struct
	 * the property text form (Prop=Value,...). Unknown struct member
	 * names are collected into OutWarnings instead of being dropped
	 * silently.
	 */
	bool BlueprintDefaultFromJsonValue(const TSharedPtr<FJsonValue>& DefaultJson, const FEdGraphPinType& PinType, FString& OutDefault, FString& OutReason, TArray<FString>& OutWarnings);

	/** Attaches non-fatal warnings to a response object (and the editor log). */
	void AppendWarningsToResult(const TSharedPtr<FJsonObject>& Result, const TArray<FString>& Warnings);

	/** Optional key reader: key presence decides, wrong-typed values are rejected. */
	bool TryGetOptionalBool(const TSharedPtr<FJsonObject>& Payload, const TCHAR* Key, bool& OutValue, bool& bPresent, FString& OutReason);
	bool TryGetOptionalString(const TSharedPtr<FJsonObject>& Payload, const TCHAR* Key, FString& OutValue, bool& bPresent, FString& OutReason);

	/** Replication condition text <-> enum (camelCase names round-trip). */
	bool ParseReplicationCondition(const FString& InText, ELifetimeCondition& OutCondition, FString& OutReason);
	const TCHAR* ReplicationConditionToText(ELifetimeCondition Condition);

	/** Sets or clears a single flag bit when the payload key was present. */
	bool ApplyBoolFlag(uint64& PropertyFlags, uint64 Flag, bool bEnable, bool bPresent);

	/**
	 * Type text in the vocabulary add_blueprint_variable accepts, so a
	 * reported type can be fed straight back into add.
	 */
	FString PinTypeToText(const FEdGraphPinType& PinType);
	FString PropertyTypeToText(const FProperty* Property);

	/** Response entry builders for list_blueprint_variables. */
	void AddVariableSettingsToJson(FJsonObject& Entry, uint64 PropertyFlags, ELifetimeCondition Condition,
		bool bPrivate, bool bExposeOnSpawn, const FString& DeprecatedMessage, bool bHasDeprecatedMessage,
		FName RepNotifyFunc);
	TSharedRef<FJsonObject> BlueprintVariableToJson(const FBPVariableDescription& Variable, const UClass* GeneratedClass);
	TSharedRef<FJsonObject> NativePropertyToJson(const FProperty* Property, const UClass* OwnerClass);
}
