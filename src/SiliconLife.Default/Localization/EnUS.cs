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

using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// English (United States) localization implementation
/// </summary>
public class EnUS : DefaultLocalizationBase
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "en-US";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "English (US)";

    /// <summary>
    /// Gets the welcome message
    /// </summary>
    public override string WelcomeMessage => "Welcome to Silicon Life Collective!";

    /// <summary>
    /// Gets the input prompt
    /// </summary>
    public override string InputPrompt => "> ";

    /// <summary>
    /// Gets the shutdown message
    /// </summary>
    public override string ShutdownMessage => "Shutting down...";

    /// <summary>
    /// Gets the config corrupted error message
    /// </summary>
    public override string ConfigCorruptedError => "Configuration file corrupted, using default configuration";

    /// <summary>
    /// Gets the config created message
    /// </summary>
    public override string ConfigCreatedWithDefaults => "Configuration file not found, created with default values";

    /// <summary>
    /// Gets the AI connection error message
    /// </summary>
    public override string AIConnectionError => "Cannot connect to AI service, please check if Ollama is running";

    /// <summary>
    /// Gets the AI request error message
    /// </summary>
    public override string AIRequestError => "AI request failed";

    /// <summary>
    /// Gets the data directory create error message
    /// </summary>
    public override string DataDirectoryCreateError => "Cannot create data directory";

    /// <summary>
    /// Gets the thinking message
    /// </summary>
    public override string ThinkingMessage => "Thinking...";

    /// <summary>
    /// Gets the tool call message
    /// </summary>
    public override string ToolCallMessage => "Executing tools...";

    /// <summary>
    /// Gets the error message
    /// </summary>
    public override string ErrorMessage => "Error";

    /// <summary>
    /// Gets the unexpected error message
    /// </summary>
    public override string UnexpectedErrorMessage => "Unexpected error";

    /// <summary>
    /// Gets the permission denied message
    /// </summary>
    public override string PermissionDeniedMessage => "Permission denied";

    /// <summary>
    /// Gets the permission ask prompt
    /// </summary>
    public override string PermissionAskPrompt => "Allow? (y/n): ";

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public override string PermissionRequestHeader => "[PERMISSION]";

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public override string AllowCodeLabel => "Allow code";

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public override string DenyCodeLabel => "Deny code";

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public override string PermissionReplyInstruction => "Reply code to confirm, or anything else to deny";

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public override string AddToCachePrompt => "Add to cache? (y/n): ";

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "Network access",
        PermissionType.CommandLine => "Command execution",
        PermissionType.FileAccess => "File access",
        PermissionType.Function => "Function invocation",
        PermissionType.DataAccess => "Data access",
        _ => permissionType.ToString()
    };

    /// <summary>
    /// Gets the system prompt for memory compression
    /// </summary>
    public override string MemoryCompressionSystemPrompt => "You are a memory compression assistant. Please compress the following memories into a concise summary while retaining key information.";

    /// <summary>
    /// Gets the user prompt template for memory compression
    /// </summary>
    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"Memory compression: {levelDesc}. Time range: {rangeDesc}.\n\nMemory content:\n{contentText}";
    }

    // ===== Init Page Localization =====

    public override string InitPageTitle => "Setup";
    public override string InitDescription => "First time setup, please complete the basic configuration";
    public override string InitNicknameLabel => "Nickname";
    public override string InitNicknamePlaceholder => "Enter your nickname";
    public override string InitEndpointLabel => "AI API Endpoint";
    public override string InitEndpointPlaceholder => "e.g. http://localhost:11434";
    public override string InitSkinLabel => "Skin";
    public override string InitSkinPlaceholder => "Leave empty for default skin";
    public override string InitDataDirectoryLabel => "Data Directory";
    public override string InitDataDirectoryPlaceholder => "e.g. ./data";
    public override string InitDataDirectoryBrowse => "Browse...";
    public override string InitSkinSelected => "\u2713 Selected";
    public override string InitSkinPreviewTitle => "Preview";
    public override string InitSkinPreviewCardTitle => "Card Title";
    public override string InitSkinPreviewCardContent => "This is a sample card showing the UI style of this skin.";
    public override string InitSkinPreviewPrimaryBtn => "Primary";
    public override string InitSkinPreviewSecondaryBtn => "Secondary";
    public override string InitSubmitButton => "Complete Setup";
    public override string InitFooterHint => "You can modify settings at any time on the settings page";
    public override string InitNicknameRequiredError => "Please enter a nickname";
    public override string InitDataDirectoryRequiredError => "Please select a data directory";
    public override string InitCuratorNameLabel => "Silicon Being Name";
    public override string InitCuratorNamePlaceholder => "Enter a name for the first silicon being";
    public override string InitCuratorNameRequiredError => "Please enter a silicon being name";
    public override string InitLanguageLabel => "Language / 语言";
    public override string InitLanguageSwitchBtn => "Apply";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "Chat with {0}";
}
