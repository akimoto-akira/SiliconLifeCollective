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
/// Czech tray localization implementation
/// </summary>
public class TrayCsCZ : TrayLocalizationBase
{
    /// <summary>
    /// Gets the fast edition name
    /// </summary>
    public override string SoftwareName => "Silicon Life Collective";

    /// <summary>
    /// Gets the status label
    /// </summary>
    public override string Status => "Stav";

    /// <summary>
    /// Gets the uptime label
    /// </summary>
    public override string Uptime => "Doba běhu";

    /// <summary>
    /// Gets the running status text
    /// </summary>
    public override string Running => "Běží";

    /// <summary>
    /// Gets the shutting down status text
    /// </summary>
    public override string ShuttingDown => "Vypíná se";

    /// <summary>
    /// Gets the silicon beings label
    /// </summary>
    public override string SiliconBeings => "Křemíkové bytosti";

    /// <summary>
    /// Gets the active status text
    /// </summary>
    public override string Active => "aktivní";

    /// <summary>
    /// Gets the name label
    /// </summary>
    public override string Name => "Název";

    /// <summary>
    /// Gets the AI model label
    /// </summary>
    public override string AIModel => "AI model";

    /// <summary>
    /// Gets the memory label
    /// </summary>
    public override string Memory => "Paměť";

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
    public override string DoubleClick => "Dvojklik";

    /// <summary>
    /// Gets the right-click action text
    /// </summary>
    public override string RightClick => "Pravé tlačítko";

    /// <summary>
    /// Gets the show menu text
    /// </summary>
    public override string ShowMenu => "Zobrazit nabídku";

    /// <summary>
    /// Gets the open web interface menu item text
    /// </summary>
    public override string OpenWebInterface => "Otevřít webové rozhraní";

    /// <summary>
    /// Gets the dashboard menu item text
    /// </summary>
    public override string Dashboard => "Dashboard";

    /// <summary>
    /// Gets the manage silicon beings menu item text
    /// </summary>
    public override string ManageSiliconBeings => "Spravovat křemíkové bytosti";

    /// <summary>
    /// Gets the configuration menu item text
    /// </summary>
    public override string Configuration => "Konfigurace";

    /// <summary>
    /// Gets the exit menu item text
    /// </summary>
    public override string Exit => "Ukončit";

    /// <summary>
    /// Gets the web server startup error title
    /// </summary>
    public override string WebServerStartupErrorTitle => "Chyba spuštění webového serveru";

    /// <summary>
    /// Gets the web server startup error message
    /// </summary>
    public override string WebServerStartupErrorMessage => "Webový server se nepodařilo spustit. Aplikace bude ukončena.\n\nChyba: {0}";
}
