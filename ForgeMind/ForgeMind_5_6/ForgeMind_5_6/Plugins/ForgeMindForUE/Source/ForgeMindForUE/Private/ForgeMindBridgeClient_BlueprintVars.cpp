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

// Blueprint variable commands split out of ForgeMindBridgeClient.cpp -
// same class, shared helpers live in ForgeMindBlueprintUtils.

#include "ForgeMindBridgeClient.h"

#include "ForgeMindBlueprintUtils.h"

#include "Dom/JsonObject.h"
#include "Dom/JsonValue.h"
#include "EdGraphSchema_K2.h"
#include "Engine/Blueprint.h"
#include "Engine/BlueprintGeneratedClass.h"
#include "Kismet2/BlueprintEditorUtils.h"
#include "Kismet2/KismetEditorUtilities.h"
#include "Misc/Paths.h"
#include "UObject/UObjectGlobals.h"
#include "UObject/UnrealType.h"

using namespace ForgeMindBlueprintUtils;

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleAddBlueprintVariable(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	FString AssetPath;
	FString VarName;
	FString TypeName;
	// 'path' is the canonical blueprint key; accept 'assetPath' as an alias
	// since the tool's own top-level 'path' carries the .uproject instead.
	if (!Payload.IsValid()
		|| (!Payload->TryGetStringField(TEXT("path"), AssetPath)
			&& !Payload->TryGetStringField(TEXT("assetPath"), AssetPath)) || AssetPath.IsEmpty()
		|| !Payload->TryGetStringField(TEXT("name"), VarName) || VarName.IsEmpty()
		|| !Payload->TryGetStringField(TEXT("type"), TypeName) || TypeName.IsEmpty())
	{
		Result->SetBoolField(TEXT("added"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Required fields: path (blueprint asset path, e.g. /Game/Blueprints/BP_Thing), name (variable name), type (bool, byte, int, int64, float, double, name, string, text, vector, rotator, transform, or a UClass name for an object reference); optional category (variable category, default 'Default'), instanceEditable (bool) and exposeOnSpawn (bool)"));
		return Result;
	}

	// Normalize to the full object path (package.asset) for LoadObject.
	FString PackagePath = AssetPath;
	int32 DotIndex = INDEX_NONE;
	if (PackagePath.FindChar(TEXT('.'), DotIndex))
	{
		PackagePath = PackagePath.Left(DotIndex);
	}
	const FString ObjectPath = FString::Printf(TEXT("%s.%s"), *PackagePath, *FPaths::GetCleanFilename(PackagePath));

	UBlueprint* Blueprint = LoadObject<UBlueprint>(nullptr, *ObjectPath);
	if (Blueprint == nullptr)
	{
		Result->SetBoolField(TEXT("added"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No blueprint found at '%s' (create one via create_blueprint first)"), *ObjectPath));
		return Result;
	}

	// Map the type description onto an FEdGraphPinType. Keywords cover the
	// common primitives and math structs; anything else is treated as a
	// UClass name and becomes an object-reference variable.
	FEdGraphPinType PinType;
	PinType.ContainerType = EPinContainerType::None;
	const FString TypeLower = TypeName.ToLower();
	if (TypeLower == TEXT("bool") || TypeLower == TEXT("boolean"))
	{
		PinType.PinCategory = UEdGraphSchema_K2::PC_Boolean;
	}
	else if (TypeLower == TEXT("byte"))
	{
		PinType.PinCategory = UEdGraphSchema_K2::PC_Byte;
	}
	else if (TypeLower == TEXT("int") || TypeLower == TEXT("int32") || TypeLower == TEXT("integer"))
	{
		PinType.PinCategory = UEdGraphSchema_K2::PC_Int;
	}
	else if (TypeLower == TEXT("int64") || TypeLower == TEXT("long"))
	{
		PinType.PinCategory = UEdGraphSchema_K2::PC_Int64;
	}
	else if (TypeLower == TEXT("float") || TypeLower == TEXT("real"))
	{
		// UE 5.6's compiler (FKismetCompilerUtilities::CreatePrimitiveProperty)
		// has no PinCategory == PC_Float branch: floating-point variables must
		// be declared as PC_Real with a PC_Float/PC_Double subcategory, anything
		// else silently falls back to an FIntProperty (which then rejects '1.5').
		PinType.PinCategory = UEdGraphSchema_K2::PC_Real;
		PinType.PinSubCategory = UEdGraphSchema_K2::PC_Float;
	}
	else if (TypeLower == TEXT("double"))
	{
		PinType.PinCategory = UEdGraphSchema_K2::PC_Real;
		PinType.PinSubCategory = UEdGraphSchema_K2::PC_Double;
	}
	else if (TypeLower == TEXT("name"))
	{
		PinType.PinCategory = UEdGraphSchema_K2::PC_Name;
	}
	else if (TypeLower == TEXT("string"))
	{
		PinType.PinCategory = UEdGraphSchema_K2::PC_String;
	}
	else if (TypeLower == TEXT("text"))
	{
		PinType.PinCategory = UEdGraphSchema_K2::PC_Text;
	}
	else if (TypeLower == TEXT("vector") || TypeLower == TEXT("rotator") || TypeLower == TEXT("transform"))
	{
		UScriptStruct* Struct = (TypeLower == TEXT("vector")) ? TBaseStructure<FVector>::Get()
			: (TypeLower == TEXT("rotator")) ? TBaseStructure<FRotator>::Get()
			: TBaseStructure<FTransform>::Get();
		PinType.PinCategory = UEdGraphSchema_K2::PC_Struct;
		PinType.PinSubCategoryObject = Struct;
	}
	else
	{
		// Same resolution rules as list_class_hierarchy: tolerate the C++
		// type prefix (UStaticMesh -> StaticMesh).
		UClass* ObjectClass = FindFirstObject<UClass>(*TypeName);
		if (ObjectClass == nullptr && TypeName.Len() > 1 && (TypeName[0] == 'U' || TypeName[0] == 'A' || TypeName[0] == 'F'))
		{
			ObjectClass = FindFirstObject<UClass>(*TypeName.RightChop(1));
		}
		if (ObjectClass == nullptr)
		{
			Result->SetBoolField(TEXT("added"), false);
			Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("'%s' is neither a known variable type keyword nor a loaded UClass name"), *TypeName));
			return Result;
		}
		PinType.PinCategory = UEdGraphSchema_K2::PC_Object;
		PinType.PinSubCategoryObject = ObjectClass;
	}

	// The optional default arrives as JSON and is normalized to the engine's
	// text form for the pin type (structs need special handling, see
	// BlueprintDefaultFromJsonValue).
	FString DefaultValue;
	const TSharedPtr<FJsonValue> DefaultJson = Payload->TryGetField(TEXT("default"));
	TArray<FString> DefaultWarnings;
	if (DefaultJson.IsValid())
	{
		FString DefaultReason;
		if (!BlueprintDefaultFromJsonValue(DefaultJson, PinType, DefaultValue, DefaultReason, DefaultWarnings))
		{
			Result->SetBoolField(TEXT("added"), false);
			Result->SetStringField(TEXT("reason"), DefaultReason);
			AppendWarningsToResult(Result, DefaultWarnings);
			return Result;
		}
	}

	// Cannot mask a variable in any superclass - the engine rejects that.
	if (!FBlueprintEditorUtils::AddMemberVariable(Blueprint, FName(*VarName), PinType, DefaultValue))
	{
		Result->SetBoolField(TEXT("added"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("Failed to add variable '%s' to '%s' (the name may collide with a superclass variable)"), *VarName, *ObjectPath));
		return Result;
	}

	// Optional category, instance-editability and expose-on-spawn tweak the
	// description/metadata before the compile below: the compiler copies
	// Category onto the property and derives CPF_ExposeOnSpawn from the
	// MD_ExposeOnSpawn metadata entry (KismetCompiler.cpp). AddMemberVariable
	// defaults to CPF_DisableEditOnInstance, so instanceEditable:true clears it.
	FString CategoryText;
	bool bExposeOnSpawn = false;
	bool bInstanceEditable = false;
	const bool bHasCategory = Payload->TryGetStringField(TEXT("category"), CategoryText);
	const bool bHasExposeOnSpawn = Payload->TryGetBoolField(TEXT("exposeOnSpawn"), bExposeOnSpawn);
	const bool bHasInstanceEditable = Payload->TryGetBoolField(TEXT("instanceEditable"), bInstanceEditable);
	if (bHasCategory || bHasInstanceEditable || (bHasExposeOnSpawn && bExposeOnSpawn))
	{
		FBPVariableDescription* Variable = Blueprint->NewVariables.FindByPredicate(
			[&VarName](const FBPVariableDescription& Desc) { return Desc.VarName == FName(*VarName); });
		if (Variable != nullptr)
		{
			if (bHasCategory)
			{
				Variable->Category = FText::FromString(CategoryText);
			}
			if (bHasInstanceEditable)
			{
				if (bInstanceEditable)
				{
					Variable->PropertyFlags &= ~CPF_DisableEditOnInstance;
				}
				else
				{
					Variable->PropertyFlags |= CPF_DisableEditOnInstance;
				}
			}
			if (bHasExposeOnSpawn && bExposeOnSpawn)
			{
				Variable->SetMetaData(FBlueprintMetadata::MD_ExposeOnSpawn, TEXT("true"));
			}
		}
	}

	// Recompile so the generated class carries the new property, then keep
	// the package dirty so the editor's save flow persists it to disk.
	FKismetEditorUtilities::CompileBlueprint(Blueprint);
	Blueprint->MarkPackageDirty();

	// Verify the default really landed in the compiled CDO when one was given.
	if (!DefaultValue.IsEmpty())
	{
		FString VerifyReason;
		if (!EnsureCompiledVariableDefault(Blueprint, VarName, DefaultValue, VerifyReason))
		{
			Result->SetBoolField(TEXT("added"), false);
			Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("Variable '%s' was added but %s"), *VarName, *VerifyReason));
			return Result;
		}
	}

	Result->SetBoolField(TEXT("added"), true);
	Result->SetBoolField(TEXT("dirty"), Blueprint->GetPackage()->IsDirty());
	Result->SetStringField(TEXT("variable"), VarName);
	Result->SetStringField(TEXT("type"), TypeName);
	Result->SetStringField(TEXT("path"), ObjectPath);
	AppendWarningsToResult(Result, DefaultWarnings);
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleUpdateBlueprintVariable(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	FString AssetPath;
	FString VarName;
	// 'path' is the canonical blueprint key; accept 'assetPath' as an alias
	// since the tool's own top-level 'path' carries the .uproject instead.
	if (!Payload.IsValid()
		|| (!Payload->TryGetStringField(TEXT("path"), AssetPath)
			&& !Payload->TryGetStringField(TEXT("assetPath"), AssetPath)) || AssetPath.IsEmpty()
		|| !Payload->TryGetStringField(TEXT("name"), VarName) || VarName.IsEmpty())
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Required fields: path (blueprint asset path), name (variable name); optional settings: category (string), instanceEditable, private, config, transient, saveGame, advancedDisplay, deprecated, exposeOnSpawn, exposeToCinematics (booleans), deprecatedMessage (string), replication ('none' or 'replicated'), replicationCondition (e.g. none, initialOnly, ownerOnly)"));
		return Result;
	}

	// Normalize to the full object path (package.asset) for LoadObject.
	FString PackagePath = AssetPath;
	int32 DotIndex = INDEX_NONE;
	if (PackagePath.FindChar(TEXT('.'), DotIndex))
	{
		PackagePath = PackagePath.Left(DotIndex);
	}
	const FString ObjectPath = FString::Printf(TEXT("%s.%s"), *PackagePath, *FPaths::GetCleanFilename(PackagePath));

	UBlueprint* Blueprint = LoadObject<UBlueprint>(nullptr, *ObjectPath);
	if (Blueprint == nullptr)
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No blueprint found at '%s' (create one via create_blueprint first)"), *ObjectPath));
		return Result;
	}

	const int32 VarIndex = FBlueprintEditorUtils::FindNewVariableIndex(Blueprint, FName(*VarName));
	if (VarIndex == INDEX_NONE)
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No variable named '%s' on '%s' (add one via add_blueprint_variable first)"), *VarName, *ObjectPath));
		return Result;
	}
	FBPVariableDescription& Variable = Blueprint->NewVariables[VarIndex];

	// Read the optional settings first so a malformed payload changes nothing.
	FString KeyReason;
	bool bInstanceEditable = false, bInstanceEditablePresent = false;
	bool bPrivate = false, bPrivatePresent = false;
	bool bConfig = false, bConfigPresent = false;
	bool bTransient = false, bTransientPresent = false;
	bool bSaveGame = false, bSaveGamePresent = false;
	bool bAdvancedDisplay = false, bAdvancedDisplayPresent = false;
	bool bDeprecated = false, bDeprecatedPresent = false;
	bool bExposeOnSpawn = false, bExposeOnSpawnPresent = false;
	bool bExposeToCinematics = false, bExposeToCinematicsPresent = false;
	FString CategoryText; bool bCategoryPresent = false;
	FString DeprecatedMessage; bool bDeprecatedMessagePresent = false;
	FString ReplicationMode; bool bReplicationPresent = false;
	FString ConditionText; bool bConditionPresent = false;
	if (!TryGetOptionalBool(Payload, TEXT("instanceEditable"), bInstanceEditable, bInstanceEditablePresent, KeyReason)
		|| !TryGetOptionalBool(Payload, TEXT("private"), bPrivate, bPrivatePresent, KeyReason)
		|| !TryGetOptionalBool(Payload, TEXT("config"), bConfig, bConfigPresent, KeyReason)
		|| !TryGetOptionalBool(Payload, TEXT("transient"), bTransient, bTransientPresent, KeyReason)
		|| !TryGetOptionalBool(Payload, TEXT("saveGame"), bSaveGame, bSaveGamePresent, KeyReason)
		|| !TryGetOptionalBool(Payload, TEXT("advancedDisplay"), bAdvancedDisplay, bAdvancedDisplayPresent, KeyReason)
		|| !TryGetOptionalBool(Payload, TEXT("deprecated"), bDeprecated, bDeprecatedPresent, KeyReason)
		|| !TryGetOptionalBool(Payload, TEXT("exposeOnSpawn"), bExposeOnSpawn, bExposeOnSpawnPresent, KeyReason)
		|| !TryGetOptionalBool(Payload, TEXT("exposeToCinematics"), bExposeToCinematics, bExposeToCinematicsPresent, KeyReason)
		|| !TryGetOptionalString(Payload, TEXT("category"), CategoryText, bCategoryPresent, KeyReason)
		|| !TryGetOptionalString(Payload, TEXT("deprecatedMessage"), DeprecatedMessage, bDeprecatedMessagePresent, KeyReason)
		|| !TryGetOptionalString(Payload, TEXT("replication"), ReplicationMode, bReplicationPresent, KeyReason)
		|| !TryGetOptionalString(Payload, TEXT("replicationCondition"), ConditionText, bConditionPresent, KeyReason))
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), KeyReason);
		return Result;
	}

	const bool bAnyKeyPresent = bInstanceEditablePresent || bPrivatePresent || bConfigPresent || bTransientPresent
		|| bSaveGamePresent || bAdvancedDisplayPresent || bDeprecatedPresent || bExposeOnSpawnPresent
		|| bExposeToCinematicsPresent || bCategoryPresent || bDeprecatedMessagePresent || bReplicationPresent || bConditionPresent;
	if (!bAnyKeyPresent)
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), TEXT("No optional settings provided - pass at least one of category, instanceEditable, private, config, transient, saveGame, advancedDisplay, deprecated, deprecatedMessage, exposeOnSpawn, exposeToCinematics, replication, replicationCondition"));
		return Result;
	}

	// Validate the replication mode before mutating anything.
	const FString ReplicationLower = ReplicationMode.ToLower();
	if (bReplicationPresent && ReplicationLower != TEXT("none") && ReplicationLower != TEXT("replicated"))
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("replication '%s' is not supported (use 'none' or 'replicated'; 'repNotify' needs an OnRep function graph - set that up in the editor)"), *ReplicationMode));
		return Result;
	}
	ELifetimeCondition NewCondition = COND_None;
	if (bConditionPresent && !ParseReplicationCondition(ConditionText, NewCondition, KeyReason))
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), KeyReason);
		return Result;
	}

	TArray<FString> Changed;

	// Property flags follow the editor's variable details panel semantics
	// (BlueprintDetailsCustomization / BlueprintEditorUtils flag setters):
	// 'instanceEditable' clears CPF_DisableEditOnInstance, 'exposeToCinematics'
	// is CPF_Interp, the rest map 1:1. ('private' is metadata, see below.)
	if (ApplyBoolFlag(Variable.PropertyFlags, CPF_DisableEditOnInstance, !bInstanceEditable, bInstanceEditablePresent))
	{
		Changed.Add(TEXT("instanceEditable"));
	}
	if (ApplyBoolFlag(Variable.PropertyFlags, CPF_Config, bConfig, bConfigPresent))
	{
		Changed.Add(TEXT("config"));
	}
	if (ApplyBoolFlag(Variable.PropertyFlags, CPF_Transient, bTransient, bTransientPresent))
	{
		Changed.Add(TEXT("transient"));
	}
	if (ApplyBoolFlag(Variable.PropertyFlags, CPF_SaveGame, bSaveGame, bSaveGamePresent))
	{
		Changed.Add(TEXT("saveGame"));
	}
	if (ApplyBoolFlag(Variable.PropertyFlags, CPF_AdvancedDisplay, bAdvancedDisplay, bAdvancedDisplayPresent))
	{
		Changed.Add(TEXT("advancedDisplay"));
	}
	if (ApplyBoolFlag(Variable.PropertyFlags, CPF_Deprecated, bDeprecated, bDeprecatedPresent))
	{
		Changed.Add(TEXT("deprecated"));
	}
	if (ApplyBoolFlag(Variable.PropertyFlags, CPF_Interp, bExposeToCinematics, bExposeToCinematicsPresent))
	{
		Changed.Add(TEXT("exposeToCinematics"));
	}

	// Replication mirrors OnChangeReplication: 'none' clears CPF_Net, the
	// rep-notify function and the condition; 'replicated' sets CPF_Net.
	if (bReplicationPresent)
	{
		if (ReplicationLower == TEXT("none"))
		{
			Variable.PropertyFlags &= ~(CPF_Net | CPF_RepNotify);
			Variable.RepNotifyFunc = NAME_None;
			Variable.ReplicationCondition = COND_None;
		}
		else
		{
			Variable.PropertyFlags |= CPF_Net;
			Variable.PropertyFlags &= ~CPF_RepNotify;
			Variable.RepNotifyFunc = NAME_None;
		}
		Changed.Add(TEXT("replication"));
	}
	if (bConditionPresent)
	{
		Variable.ReplicationCondition = NewCondition;
		Changed.Add(TEXT("replicationCondition"));
	}
	if (bCategoryPresent)
	{
		Variable.Category = FText::FromString(CategoryText);
		Changed.Add(TEXT("category"));
	}

	// Metadata-backed settings go through the editor utilities so they fire
	// the same change notifications as the details panel (ExposeOnSpawn is
	// derived into CPF_ExposeOnSpawn by the compiler, DeprecationMessage
	// feeds the deprecation compiler warning). 'private' is the MD_Private
	// metadata entry (OnPrivateChanged) - 5.6 has no CPF_Private flag.
	if (bPrivatePresent)
	{
		if (bPrivate)
		{
			FBlueprintEditorUtils::SetBlueprintVariableMetaData(Blueprint, FName(*VarName), nullptr, FBlueprintMetadata::MD_Private, TEXT("true"));
		}
		else
		{
			FBlueprintEditorUtils::RemoveBlueprintVariableMetaData(Blueprint, FName(*VarName), nullptr, FBlueprintMetadata::MD_Private);
		}
		Changed.Add(TEXT("private"));
	}
	if (bExposeOnSpawnPresent)
	{
		if (bExposeOnSpawn)
		{
			FBlueprintEditorUtils::SetBlueprintVariableMetaData(Blueprint, FName(*VarName), nullptr, FBlueprintMetadata::MD_ExposeOnSpawn, TEXT("true"));
		}
		else
		{
			FBlueprintEditorUtils::RemoveBlueprintVariableMetaData(Blueprint, FName(*VarName), nullptr, FBlueprintMetadata::MD_ExposeOnSpawn);
		}
		Changed.Add(TEXT("exposeOnSpawn"));
	}
	if (bDeprecatedMessagePresent)
	{
		if (DeprecatedMessage.IsEmpty())
		{
			FBlueprintEditorUtils::RemoveBlueprintVariableMetaData(Blueprint, FName(*VarName), nullptr, FBlueprintMetadata::MD_DeprecationMessage);
		}
		else
		{
			FBlueprintEditorUtils::SetBlueprintVariableMetaData(Blueprint, FName(*VarName), nullptr, FBlueprintMetadata::MD_DeprecationMessage, DeprecatedMessage);
		}
		Changed.Add(TEXT("deprecatedMessage"));
	}

	// Flags feed the generated class, so treat this as a structural change,
	// recompile, and keep the package dirty so the editor's save flow
	// persists it to disk.
	FBlueprintEditorUtils::MarkBlueprintAsStructurallyModified(Blueprint);
	FKismetEditorUtilities::CompileBlueprint(Blueprint);
	Blueprint->MarkPackageDirty();

	TArray<TSharedPtr<FJsonValue>> ChangedValues;
	for (const FString& Key : Changed)
	{
		ChangedValues.Add(MakeShared<FJsonValueString>(Key));
	}
	Result->SetBoolField(TEXT("updated"), true);
	Result->SetBoolField(TEXT("dirty"), Blueprint->GetPackage()->IsDirty());
	Result->SetStringField(TEXT("variable"), VarName);
	Result->SetArrayField(TEXT("changed"), ChangedValues);
	Result->SetStringField(TEXT("path"), ObjectPath);
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleListBlueprintVariables(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	FString AssetPath;
	// 'path' is the canonical blueprint key; accept 'assetPath' as an alias
	// since the tool's own top-level 'path' carries the .uproject instead.
	if (!Payload.IsValid()
		|| (!Payload->TryGetStringField(TEXT("path"), AssetPath)
			&& !Payload->TryGetStringField(TEXT("assetPath"), AssetPath)) || AssetPath.IsEmpty())
	{
		Result->SetBoolField(TEXT("found"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Required field: path (blueprint asset path, e.g. /Game/Blueprints/BP_Thing); optional includeInherited (bool, default false) also lists variables inherited from parent blueprints and C++ classes"));
		return Result;
	}

	bool bIncludeInherited = false;
	if (Payload->HasField(TEXT("includeInherited")) && !Payload->TryGetBoolField(TEXT("includeInherited"), bIncludeInherited))
	{
		Result->SetBoolField(TEXT("found"), false);
		Result->SetStringField(TEXT("reason"), TEXT("'includeInherited' must be a boolean"));
		return Result;
	}

	// Normalize to the full object path (package.asset) for LoadObject.
	FString PackagePath = AssetPath;
	int32 DotIndex = INDEX_NONE;
	if (PackagePath.FindChar(TEXT('.'), DotIndex))
	{
		PackagePath = PackagePath.Left(DotIndex);
	}
	const FString ObjectPath = FString::Printf(TEXT("%s.%s"), *PackagePath, *FPaths::GetCleanFilename(PackagePath));

	UBlueprint* Blueprint = LoadObject<UBlueprint>(nullptr, *ObjectPath);
	if (Blueprint == nullptr)
	{
		Result->SetBoolField(TEXT("found"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No blueprint found at '%s' (create one via create_blueprint first)"), *ObjectPath));
		return Result;
	}

	TArray<TSharedPtr<FJsonValue>> Variables;
	TSet<FName> SeenNames;

	// The blueprint's own variables come straight from NewVariables - the
	// descriptions are the source of truth the details panel edits, and only
	// these can be changed via update/remove_blueprint_variable.
	for (const FBPVariableDescription& Variable : Blueprint->NewVariables)
	{
		Variables.Add(MakeShared<FJsonValueObject>(BlueprintVariableToJson(Variable, Blueprint->GeneratedClass)));
		SeenNames.Add(Variable.VarName);
	}

	int32 InheritedCount = 0;
	if (bIncludeInherited)
	{
		// Walk the parent chain the way the MyBlueprint panel's "Show
		// Inherited Variables" does: blueprint parents contribute their
		// NewVariables descriptions, native classes their compiled
		// CPF_BlueprintVisible properties (same visibility filter).
		UClass* ParentClass = Blueprint->ParentClass;
		while (ParentClass != nullptr)
		{
			if (Cast<UBlueprintGeneratedClass>(ParentClass) != nullptr)
			{
				const UBlueprint* ParentBlueprint = Cast<UBlueprint>(ParentClass->ClassGeneratedBy);
				if (ParentBlueprint != nullptr)
				{
					for (const FBPVariableDescription& Variable : ParentBlueprint->NewVariables)
					{
						if (SeenNames.Contains(Variable.VarName))
						{
							continue;
						}
						TSharedRef<FJsonObject> Entry = BlueprintVariableToJson(Variable, ParentBlueprint->GeneratedClass);
						Entry->SetStringField(TEXT("inheritedFrom"), ParentBlueprint->GetPathName());
						Variables.Add(MakeShared<FJsonValueObject>(Entry));
						SeenNames.Add(Variable.VarName);
						++InheritedCount;
					}
				}
			}
			else
			{
				for (TFieldIterator<FProperty> It(ParentClass, EFieldIteratorFlags::ExcludeSuper); It; ++It)
				{
					FProperty* Property = *It;
					if (Property->HasAnyPropertyFlags(CPF_Parm)
						|| !Property->HasAllPropertyFlags(CPF_BlueprintVisible)
						|| Property->IsA(FDelegateProperty::StaticClass())
						|| Property->IsA(FMulticastDelegateProperty::StaticClass())
						|| SeenNames.Contains(Property->GetFName()))
					{
						continue;
					}
					TSharedRef<FJsonObject> Entry = NativePropertyToJson(Property, ParentClass);
					Entry->SetStringField(TEXT("inheritedFrom"), ParentClass->GetName());
					Variables.Add(MakeShared<FJsonValueObject>(Entry));
					SeenNames.Add(Property->GetFName());
					++InheritedCount;
				}
			}
			ParentClass = ParentClass->GetSuperClass();
		}
	}

	Result->SetBoolField(TEXT("found"), true);
	Result->SetStringField(TEXT("path"), ObjectPath);
	Result->SetNumberField(TEXT("count"), Variables.Num());
	Result->SetNumberField(TEXT("inheritedCount"), InheritedCount);
	Result->SetArrayField(TEXT("variables"), Variables);
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleSetBlueprintVariableDefault(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	FString AssetPath;
	FString VarName;
	// 'path' is the canonical blueprint key; accept 'assetPath' as an alias
	// since the tool's own top-level 'path' carries the .uproject instead.
	if (!Payload.IsValid()
		|| (!Payload->TryGetStringField(TEXT("path"), AssetPath)
			&& !Payload->TryGetStringField(TEXT("assetPath"), AssetPath)) || AssetPath.IsEmpty()
		|| !Payload->TryGetStringField(TEXT("name"), VarName) || VarName.IsEmpty())
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Required fields: path (blueprint asset path, e.g. /Game/Blueprints/BP_Thing), name (variable name), default (new default value)"));
		return Result;
	}

	// The default arrives as JSON and is normalized to the engine's text
	// form for the variable's pin type (structs need special handling, see
	// BlueprintDefaultFromJsonValue).
	FString DefaultValue;
	const TSharedPtr<FJsonValue> DefaultJson = Payload->TryGetField(TEXT("default"));
	if (!DefaultJson.IsValid())
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Missing required field 'default' (the new default value - a scalar for plain types, or {\"X\":x,\"Y\":y,\"Z\":z} / [x,y,z] / \"x,y,z\" for vectors, {\"Pitch\":p,\"Yaw\":y,\"Roll\":r} for rotators, {\"Translation\":{...},\"Rotation\":{...},\"Scale\":{...}} for transforms)"));
		return Result;
	}

	// Normalize to the full object path (package.asset) for LoadObject.
	FString PackagePath = AssetPath;
	int32 DotIndex = INDEX_NONE;
	if (PackagePath.FindChar(TEXT('.'), DotIndex))
	{
		PackagePath = PackagePath.Left(DotIndex);
	}
	const FString ObjectPath = FString::Printf(TEXT("%s.%s"), *PackagePath, *FPaths::GetCleanFilename(PackagePath));

	UBlueprint* Blueprint = LoadObject<UBlueprint>(nullptr, *ObjectPath);
	if (Blueprint == nullptr)
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No blueprint found at '%s' (create one via create_blueprint first)"), *ObjectPath));
		return Result;
	}

	// DefaultValue is plain importable text on FBPVariableDescription; the
	// recompile below validates it against the variable's pin type.
	FBPVariableDescription* Variable = Blueprint->NewVariables.FindByPredicate(
		[&VarName](const FBPVariableDescription& Desc) { return Desc.VarName == FName(*VarName); });
	if (Variable == nullptr)
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No variable named '%s' on '%s' (add one via add_blueprint_variable first)"), *VarName, *ObjectPath));
		return Result;
	}

	// Struct defaults need the variable's pin type for normalization.
	FString DefaultReason;
	TArray<FString> DefaultWarnings;
	if (!BlueprintDefaultFromJsonValue(DefaultJson, Variable->VarType, DefaultValue, DefaultReason, DefaultWarnings))
	{
		Result->SetBoolField(TEXT("updated"), false);
		Result->SetStringField(TEXT("reason"), DefaultReason);
		AppendWarningsToResult(Result, DefaultWarnings);
		return Result;
	}

	Variable->DefaultValue = DefaultValue;

	// Default values feed the generated class, so treat this as a structural
	// change, recompile, and keep the package dirty so the editor's save
	// flow persists it to disk.
	FBlueprintEditorUtils::MarkBlueprintAsStructurallyModified(Blueprint);
	FKismetEditorUtilities::CompileBlueprint(Blueprint);
	Blueprint->MarkPackageDirty();

	// Verify the default really landed in the compiled CDO.
	{
		FString VerifyReason;
		if (!EnsureCompiledVariableDefault(Blueprint, VarName, DefaultValue, VerifyReason))
		{
			Result->SetBoolField(TEXT("updated"), false);
			Result->SetStringField(TEXT("reason"), VerifyReason);
			return Result;
		}
	}

	Result->SetBoolField(TEXT("updated"), true);
	Result->SetBoolField(TEXT("dirty"), Blueprint->GetPackage()->IsDirty());
	Result->SetStringField(TEXT("variable"), VarName);
	Result->SetStringField(TEXT("default"), DefaultValue);
	Result->SetStringField(TEXT("path"), ObjectPath);
	AppendWarningsToResult(Result, DefaultWarnings);
	return Result;
}

TSharedPtr<FJsonObject> FForgeMindBridgeClient::HandleRemoveBlueprintVariable(const TSharedPtr<FJsonObject>& Payload)
{
	TSharedPtr<FJsonObject> Result = MakeShared<FJsonObject>();

	FString AssetPath;
	FString VarName;
	// 'path' is the canonical blueprint key; accept 'assetPath' as an alias
	// since the tool's own top-level 'path' carries the .uproject instead.
	if (!Payload.IsValid()
		|| (!Payload->TryGetStringField(TEXT("path"), AssetPath)
			&& !Payload->TryGetStringField(TEXT("assetPath"), AssetPath)) || AssetPath.IsEmpty()
		|| !Payload->TryGetStringField(TEXT("name"), VarName) || VarName.IsEmpty())
	{
		Result->SetBoolField(TEXT("removed"), false);
		Result->SetStringField(TEXT("reason"), TEXT("Required fields: path (blueprint asset path, e.g. /Game/Blueprints/BP_Thing), name (variable name)"));
		return Result;
	}

	// Normalize to the full object path (package.asset) for LoadObject.
	FString PackagePath = AssetPath;
	int32 DotIndex = INDEX_NONE;
	if (PackagePath.FindChar(TEXT('.'), DotIndex))
	{
		PackagePath = PackagePath.Left(DotIndex);
	}
	const FString ObjectPath = FString::Printf(TEXT("%s.%s"), *PackagePath, *FPaths::GetCleanFilename(PackagePath));

	UBlueprint* Blueprint = LoadObject<UBlueprint>(nullptr, *ObjectPath);
	if (Blueprint == nullptr)
	{
		Result->SetBoolField(TEXT("removed"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No blueprint found at '%s'"), *ObjectPath));
		return Result;
	}

	if (!Blueprint->NewVariables.ContainsByPredicate(
		[&VarName](const FBPVariableDescription& Desc) { return Desc.VarName == FName(*VarName); }))
	{
		Result->SetBoolField(TEXT("removed"), false);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("No variable named '%s' on '%s'"), *VarName, *ObjectPath));
		return Result;
	}

	// The editor's own guard before removing a variable (MyBlueprint panel):
	// refuse while any graph still references it.
	if (FBlueprintEditorUtils::IsVariableUsed(Blueprint, FName(*VarName)))
	{
		Result->SetBoolField(TEXT("removed"), false);
		Result->SetBoolField(TEXT("inUse"), true);
		Result->SetStringField(TEXT("reason"), FString::Printf(TEXT("Variable '%s' is still referenced by graphs in '%s'; disconnect or remove those references before deleting it"), *VarName, *ObjectPath));
		return Result;
	}

	FBlueprintEditorUtils::RemoveMemberVariable(Blueprint, FName(*VarName));

	// Removing a member changes the generated class layout - recompile and
	// keep the package dirty so the editor's save flow persists the removal.
	FBlueprintEditorUtils::MarkBlueprintAsStructurallyModified(Blueprint);
	FKismetEditorUtilities::CompileBlueprint(Blueprint);
	Blueprint->MarkPackageDirty();

	Result->SetBoolField(TEXT("removed"), true);
	Result->SetBoolField(TEXT("dirty"), Blueprint->GetPackage()->IsDirty());
	Result->SetStringField(TEXT("variable"), VarName);
	Result->SetStringField(TEXT("path"), ObjectPath);
	return Result;
}
