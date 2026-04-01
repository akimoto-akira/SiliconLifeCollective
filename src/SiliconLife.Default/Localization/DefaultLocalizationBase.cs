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
    /// Gets the error message when an operation fails
    /// </summary>
    public abstract string ErrorMessage { get; }

    /// <summary>
    /// Gets the error message when an unexpected error occurs
    /// </summary>
    public abstract string UnexpectedErrorMessage { get; }
}
