// Copyright Epic Games, Inc. All Rights Reserved.

#include "ForgeMindForUE.h"

#include "ForgeMindBridgeClient.h"

#define LOCTEXT_NAMESPACE "FForgeMindForUEModule"

void FForgeMindForUEModule::StartupModule()
{
	// Editor-only bridge; never connect when running a packaged game
	if (IsRunningGame())
	{
		return;
	}

	BridgeClient = MakeUnique<FForgeMindBridgeClient>();
	BridgeClient->Start();

	// The client needs game-thread ticks for connect retries and message dispatch
	TickerHandle = FTSTicker::GetCoreTicker().AddTicker(
		FTickerDelegate::CreateLambda([this](float DeltaTime)
		{
			if (BridgeClient.IsValid())
			{
				BridgeClient->Tick(DeltaTime);
			}
			return true;
		}));
}

void FForgeMindForUEModule::ShutdownModule()
{
	FTSTicker::GetCoreTicker().RemoveTicker(TickerHandle);
	if (BridgeClient.IsValid())
	{
		BridgeClient->Stop();
		BridgeClient.Reset();
	}
}

#undef LOCTEXT_NAMESPACE
	
IMPLEMENT_MODULE(FForgeMindForUEModule, ForgeMindForUE)