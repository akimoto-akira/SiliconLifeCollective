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
}
