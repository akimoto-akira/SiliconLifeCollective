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
    public abstract string SoftwareName { get; }

    public abstract string Status { get; }

    public abstract string Uptime { get; }

    public abstract string ShuttingDown { get; }

    public abstract string SiliconBeings { get; }

    public abstract string Active { get; }

    public abstract string Name { get; }

    public abstract string AIModel { get; }

    public abstract string Memory { get; }

    public abstract string CPU { get; }

    public abstract string Web { get; }

    public abstract string ShowStatus { get; }

    public abstract string OpenWebInterface { get; }

    public abstract string Dashboard { get; }

    public abstract string ManageSiliconBeings { get; }

    public abstract string Configuration { get; }

    public abstract string Exit { get; }

    public abstract string ExitConfirmation { get; }

    public abstract string Yes { get; }

    public abstract string No { get; }
}
