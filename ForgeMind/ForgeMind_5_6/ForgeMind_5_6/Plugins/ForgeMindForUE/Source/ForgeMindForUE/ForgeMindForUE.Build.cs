// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class ForgeMindForUE : ModuleRules
{
	public ForgeMindForUE(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
		
		PublicIncludePaths.AddRange(
			new string[] {
				// ... add public include paths required here ...
			}
			);
				
		
		PrivateIncludePaths.AddRange(
			new string[] {
				// ... add other private include paths required here ...
			}
			);
			
		
		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
				// ... add other public dependencies that you statically link with here ...
			}
			);
			
		
		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
				"CoreUObject",
				"Engine",
				"Slate",
				"SlateCore",
				// ForgeMind bridge: TCP channel, JSON envelope, .uproject path
				"Networking",
				"Sockets",
				"Json",
				// get_actor_details: FJsonObjectConverter reflection-based property dump
				"JsonUtilities",
				"Projects",
				// Editor state + command handlers (open_asset / exec_console)
				"UnrealEd",
				// quit_editor: IMainFrameModule::RequestCloseEditor graceful shutdown
				"MainFrame",
				// create_blueprint: FAssetRegistryModule::AssetCreated registration
				"AssetRegistry",
				// add_blueprint_variable: UEdGraphSchema_K2 pin category constants
				"BlueprintGraph",
				// ... add private dependencies that you statically link with here ...	
			}
			);
		
		
		DynamicallyLoadedModuleNames.AddRange(
			new string[]
			{
				// ... add any modules that your module loads dynamically here ...
			}
			);
	}
}
