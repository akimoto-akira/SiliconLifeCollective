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

namespace SiliconLife.Fast.Tray;

/// <summary>
/// Abstract base class for tray manager localization
/// Contains all system tray UI text
/// </summary>
public abstract class TrayLocalizationBase
{
    /// <summary>
    /// Gets the localized software name
    /// </summary>
    public abstract string SoftwareName { get; }

    /// <summary>
    /// Gets the status label
    /// </summary>
    public abstract string Status { get; }

    /// <summary>
    /// Gets the uptime label
    /// </summary>
    public abstract string Uptime { get; }

    /// <summary>
    /// Gets the running status text
    /// </summary>
    public abstract string Running { get; }

    /// <summary>
    /// Gets the shutting down status text
    /// </summary>
    public abstract string ShuttingDown { get; }

    /// <summary>
    /// Gets the silicon beings label
    /// </summary>
    public abstract string SiliconBeings { get; }

    /// <summary>
    /// Gets the active status text
    /// </summary>
    public abstract string Active { get; }

    /// <summary>
    /// Gets the name label
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Gets the AI model label
    /// </summary>
    public abstract string AIModel { get; }

    /// <summary>
    /// Gets the memory label
    /// </summary>
    public abstract string Memory { get; }

    /// <summary>
    /// Gets the CPU label
    /// </summary>
    public abstract string CPU { get; }

    /// <summary>
    /// Gets the web label
    /// </summary>
    public abstract string Web { get; }

    /// <summary>
    /// Gets the double-click action text
    /// </summary>
    public abstract string DoubleClick { get; }

    /// <summary>
    /// Gets the right-click action text
    /// </summary>
    public abstract string RightClick { get; }

    /// <summary>
    /// Gets the show menu text
    /// </summary>
    public abstract string ShowMenu { get; }

    /// <summary>
    /// Gets the open web interface menu item text
    /// </summary>
    public abstract string OpenWebInterface { get; }

    /// <summary>
    /// Gets the dashboard menu item text
    /// </summary>
    public abstract string Dashboard { get; }

    /// <summary>
    /// Gets the manage silicon beings menu item text
    /// </summary>
    public abstract string ManageSiliconBeings { get; }

    /// <summary>
    /// Gets the configuration menu item text
    /// </summary>
    public abstract string Configuration { get; }

    /// <summary>
    /// Gets the Speedy Pack Manager menu item text
    /// </summary>
    public abstract string SpeedyPackManager { get; }

    /// <summary>
    /// Gets the exit menu item text
    /// </summary>
    public abstract string Exit { get; }

    /// <summary>
    /// Gets the web server startup error title
    /// </summary>
    public abstract string WebServerStartupErrorTitle { get; }

    /// <summary>
    /// Gets the web server startup error message
    /// </summary>
    public abstract string WebServerStartupErrorMessage { get; }
}
