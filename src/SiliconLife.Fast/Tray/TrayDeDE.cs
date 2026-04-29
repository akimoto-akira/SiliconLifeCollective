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
/// German (Germany) tray localization implementation
/// </summary>
public class TrayDeDE : TrayLocalizationBase
{
    /// <summary>
    /// Gets the fast edition name
    /// </summary>
    public override string SoftwareName => "Silicon Life Collective";

    /// <summary>
    /// Gets the status label
    /// </summary>
    public override string Status => "Status";

    /// <summary>
    /// Gets the uptime label
    /// </summary>
    public override string Uptime => "Betriebszeit";

    /// <summary>
    /// Gets the running status text
    /// </summary>
    public override string Running => "Läuft";

    /// <summary>
    /// Gets the shutting down status text
    /// </summary>
    public override string ShuttingDown => "Herunterfahren";

    /// <summary>
    /// Gets the silicon beings label
    /// </summary>
    public override string SiliconBeings => "Silizium-Lebewesen";

    /// <summary>
    /// Gets the active status text
    /// </summary>
    public override string Active => "aktiv";

    /// <summary>
    /// Gets the name label
    /// </summary>
    public override string Name => "Name";

    /// <summary>
    /// Gets the AI model label
    /// </summary>
    public override string AIModel => "KI-Modell";

    /// <summary>
    /// Gets the memory label
    /// </summary>
    public override string Memory => "Speicher";

    /// <summary>
    /// Gets the CPU label
    /// </summary>
    public override string CPU => "CPU";

    /// <summary>
    /// Gets the web label
    /// </summary>
    public override string Web => "Web";

    /// <summary>
    /// Gets the double-click action text
    /// </summary>
    public override string DoubleClick => "Doppelklick";

    /// <summary>
    /// Gets the right-click action text
    /// </summary>
    public override string RightClick => "Rechtsklick";

    /// <summary>
    /// Gets the show menu text
    /// </summary>
    public override string ShowMenu => "Menü anzeigen";

    /// <summary>
    /// Gets the open web interface menu item text
    /// </summary>
    public override string OpenWebInterface => "Web-Oberfläche öffnen";

    /// <summary>
    /// Gets the dashboard menu item text
    /// </summary>
    public override string Dashboard => "Dashboard";

    /// <summary>
    /// Gets the manage silicon beings menu item text
    /// </summary>
    public override string ManageSiliconBeings => "Silizium-Lebewesen verwalten";

    /// <summary>
    /// Gets the configuration menu item text
    /// </summary>
    public override string Configuration => "Konfiguration";

    /// <summary>
    /// Gets the exit menu item text
    /// </summary>
    public override string Exit => "Beenden";
}
