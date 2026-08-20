// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include "Containers/Ticker.h"
#include "Modules/ModuleManager.h"

class FForgeMindBridgeClient;

class FForgeMindForUEModule : public IModuleInterface
{
public:

	/** IModuleInterface implementation */
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;

private:
	/** TCP bridge client towards the SiliconLife host. */
	TUniquePtr<FForgeMindBridgeClient> BridgeClient;
	FTSTicker::FDelegateHandle TickerHandle;
};
