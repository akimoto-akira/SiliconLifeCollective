// Copyright (c) 2026 ForgeMind. All rights reserved.

#include "TestActor.h"
#include "Components/SceneComponent.h"

ATestActor::ATestActor()
{
    PrimaryActorTick.bCanEverTick = false;

    RootSceneComponent = CreateDefaultSubobject<USceneComponent>(TEXT("RootSceneComponent"));
    RootComponent = RootSceneComponent;

    bTestFlag = false;
}
