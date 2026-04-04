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
/// Abstract base class for default localization implementation
/// Contains all UI text and error messages
/// </summary>
public abstract class DefaultLocalizationBase : LocalizationBase
{
    /// <summary>
    /// Gets the welcome message displayed when the application starts
    /// </summary>
    public abstract string WelcomeMessage { get; }

    /// <summary>
    /// Gets the prompt displayed when waiting for user input
    /// </summary>
    public abstract string InputPrompt { get; }

    /// <summary>
    /// Gets the message displayed when the application is shutting down
    /// </summary>
    public abstract string ShutdownMessage { get; }

    /// <summary>
    /// Gets the error message when configuration file is corrupted
    /// </summary>
    public abstract string ConfigCorruptedError { get; }

    /// <summary>
    /// Gets the message when configuration file is created with default values
    /// </summary>
    public abstract string ConfigCreatedWithDefaults { get; }

    /// <summary>
    /// Gets the error message when AI client fails to connect
    /// </summary>
    public abstract string AIConnectionError { get; }

    /// <summary>
    /// Gets the error message when AI request fails
    /// </summary>
    public abstract string AIRequestError { get; }

    /// <summary>
    /// Gets the error message when data directory cannot be created
    /// </summary>
    public abstract string DataDirectoryCreateError { get; }

    /// <summary>
    /// Gets the message displayed when a silicon being is thinking
    /// </summary>
    public abstract string ThinkingMessage { get; }

    /// <summary>
    /// Gets the message displayed when a silicon being executes tool calls
    /// </summary>
    public abstract string ToolCallMessage { get; }

    /// <summary>
    /// Gets the error message when an operation fails
    /// </summary>
    public abstract string ErrorMessage { get; }

    /// <summary>
    /// Gets the error message when an unexpected error occurs
    /// </summary>
    public abstract string UnexpectedErrorMessage { get; }

    /// <summary>
    /// Gets the message when a permission request is denied
    /// </summary>
    public abstract string PermissionDeniedMessage { get; }

    /// <summary>
    /// Gets the prompt for user permission decision
    /// </summary>
    public abstract string PermissionAskPrompt { get; }

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public abstract string PermissionRequestHeader { get; }

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public abstract string AllowCodeLabel { get; }

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public abstract string DenyCodeLabel { get; }

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public abstract string PermissionReplyInstruction { get; }

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public abstract string AddToCachePrompt { get; }

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    /// <param name="permissionType">The permission type</param>
    /// <returns>The localized display name</returns>
    public abstract string GetPermissionTypeName(PermissionType permissionType);

    // ===== Init Page Localization =====

    /// <summary>
    /// Gets the page title for the initialization page
    /// </summary>
    public abstract string InitPageTitle { get; }

    /// <summary>
    /// Gets the description text displayed below the title on the init page
    /// </summary>
    public abstract string InitDescription { get; }

    /// <summary>
    /// Gets the label for the nickname field
    /// </summary>
    public abstract string InitNicknameLabel { get; }

    /// <summary>
    /// Gets the placeholder for the nickname input
    /// </summary>
    public abstract string InitNicknamePlaceholder { get; }

    /// <summary>
    /// Gets the label for the AI API endpoint field
    /// </summary>
    public abstract string InitEndpointLabel { get; }

    /// <summary>
    /// Gets the placeholder for the AI API endpoint input
    /// </summary>
    public abstract string InitEndpointPlaceholder { get; }

    /// <summary>
    /// Gets the label for the skin field
    /// </summary>
    public abstract string InitSkinLabel { get; }

    /// <summary>
    /// Gets the placeholder for the skin input
    /// </summary>
    public abstract string InitSkinPlaceholder { get; }

    /// <summary>
    /// Gets the label for the data directory field
    /// </summary>
    public abstract string InitDataDirectoryLabel { get; }

    /// <summary>
    /// Gets the placeholder text for the data directory input
    /// </summary>
    public abstract string InitDataDirectoryPlaceholder { get; }

    /// <summary>
    /// Gets the text for the browse folder button
    /// </summary>
    public abstract string InitDataDirectoryBrowse { get; }

    /// <summary>
    /// Gets the text shown after "already selected" on a skin card
    /// </summary>
    public abstract string InitSkinSelected { get; }

    /// <summary>
    /// Gets the preview section title
    /// </summary>
    public abstract string InitSkinPreviewTitle { get; }

    /// <summary>
    /// Gets the preview card title text
    /// </summary>
    public abstract string InitSkinPreviewCardTitle { get; }

    /// <summary>
    /// Gets the preview card content text
    /// </summary>
    public abstract string InitSkinPreviewCardContent { get; }

    /// <summary>
    /// Gets the preview primary button text
    /// </summary>
    public abstract string InitSkinPreviewPrimaryBtn { get; }

    /// <summary>
    /// Gets the preview secondary button text
    /// </summary>
    public abstract string InitSkinPreviewSecondaryBtn { get; }

    /// <summary>
    /// Gets the text for the submit button
    /// </summary>
    public abstract string InitSubmitButton { get; }

    /// <summary>
    /// Gets the footer hint text about modifying settings later
    /// </summary>
    public abstract string InitFooterHint { get; }

    /// <summary>
    /// Gets the validation error message when nickname is empty
    /// </summary>
    public abstract string InitNicknameRequiredError { get; }

    /// <summary>
    /// Gets the validation error message when data directory is empty
    /// </summary>
    public abstract string InitDataDirectoryRequiredError { get; }

    /// <summary>
    /// Gets the label for the curator (first silicon being) name field
    /// </summary>
    public abstract string InitCuratorNameLabel { get; }

    /// <summary>
    /// Gets the placeholder for the curator name input
    /// </summary>
    public abstract string InitCuratorNamePlaceholder { get; }

    /// <summary>
    /// Gets the validation error message when curator name is empty
    /// </summary>
    public abstract string InitCuratorNameRequiredError { get; }

    /// <summary>
    /// Gets the label for the language selector field
    /// </summary>
    public abstract string InitLanguageLabel { get; }

    /// <summary>
    /// Gets the button text for applying language switch
    /// </summary>
    public abstract string InitLanguageSwitchBtn { get; }
}
