// Copyright (c) 2026 ForgeMind. All rights reserved.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Camera/CameraActor.h"
#include "Engine/StaticMesh.h"
#include "TestActor.generated.h"

UCLASS(Blueprintable)
class FORGEMIND_5_6_API ATestActor : public AActor
{
    GENERATED_BODY()

public:
    ATestActor();

    // Root scene component so the actor has a transform in the world.
    UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Test")
    TObjectPtr<USceneComponent> RootSceneComponent;

    // Blueprint-controllable flag for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    bool bTestFlag = false;

    // Byte (uint8) value for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    uint8 TestByteValue = 0;

    // Integer (int32) value for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    int32 TestIntValue = 0;

    // Long integer (int64) value for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    int64 TestInt64Value = 0;

    // Single-precision floating point (float) value for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    float TestFloatValue = 0.0f;

    // Name value for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    FName TestNameValue;

    // String value for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    FString TestStringValue;

    // Text value for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    FText TestTextValue;

    // Vector value for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    FVector TestVectorValue;

    // Rotator value for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    FRotator TestRotatorValue;

    // Transform value for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    FTransform TestTransformValue;

    // Camera actor reference for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    TObjectPtr<ACameraActor> TestCameraActorValue;

    // Static mesh asset reference for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test")
    TObjectPtr<UStaticMesh> TestStaticMeshValue;

    // ---- Array versions ----

    // Boolean array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<bool> TestBoolArray;

    // Byte (uint8) array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<uint8> TestByteArray;

    // Integer (int32) array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<int32> TestIntArray;

    // Long integer (int64) array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<int64> TestInt64Array;

    // Float array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<float> TestFloatArray;

    // Name array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<FName> TestNameArray;

    // String array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<FString> TestStringArray;

    // Text array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<FText> TestTextArray;

    // Vector array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<FVector> TestVectorArray;

    // Rotator array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<FRotator> TestRotatorArray;

    // Transform array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<FTransform> TestTransformArray;

    // Camera actor reference array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<TObjectPtr<ACameraActor>> TestCameraActorArray;

    // Static mesh asset reference array for scene testing.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Arrays")
    TArray<TObjectPtr<UStaticMesh>> TestStaticMeshArray;

    // ---- Map versions ----

    // Integer key → String value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<int32, FString> TestIntStringMap;

    // Name key → Integer value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<FName, int32> TestNameIntMap;

    // String key → Float value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<FString, float> TestStringFloatMap;

    // Integer key → Vector value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<int32, FVector> TestIntVectorMap;

    // Name key → Text value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<FName, FText> TestNameTextMap;

    // String key → Boolean value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<FString, bool> TestStringBoolMap;

    // Integer key → Int64 value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<int32, int64> TestIntInt64Map;

    // Name key → Rotator value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<FName, FRotator> TestNameRotatorMap;

    // String key → Transform value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<FString, FTransform> TestStringTransformMap;

    // Integer key → StaticMesh reference map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<int32, TObjectPtr<UStaticMesh>> TestIntStaticMeshMap;

    // Name key → CameraActor reference map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<FName, TObjectPtr<ACameraActor>> TestNameCameraActorMap;

    // Byte (uint8) key → Name value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<uint8, FName> TestByteNameMap;

    // Byte (uint8) key → String value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<uint8, FString> TestByteStringMap;

    // String key → Int32 value map.
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Test|Maps")
    TMap<FString, int32> TestStringIntMap;
};
