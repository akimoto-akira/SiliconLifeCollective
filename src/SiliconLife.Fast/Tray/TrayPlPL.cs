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
/// Polish tray localization implementation
/// </summary>
public class TrayPlPL : TrayLocalizationBase
{
    /// <summary>
    /// Gets the fast edition name
    /// </summary>
    public override string SoftwareName => "Silicon Life Collective";

    /// <summary>
    /// Gets the status label
    /// </summary>
    public override string Status => "Stan";

    /// <summary>
    /// Gets the uptime label
    /// </summary>
    public override string Uptime => "Czas działania";

    /// <summary>
    /// Gets the running status text
    /// </summary>
    public override string Running => "Uruchomiony";

    /// <summary>
    /// Gets the shutting down status text
    /// </summary>
    public override string ShuttingDown => "Zamykanie";

    /// <summary>
    /// Gets the silicon beings label
    /// </summary>
    public override string SiliconBeings => "Bycia krzemowe";

    /// <summary>
    /// Gets the active status text
    /// </summary>
    public override string Active => "aktywne";

    /// <summary>
    /// Gets the name label
    /// </summary>
    public override string Name => "Nazwa";

    /// <summary>
    /// Gets the AI model label
    /// </summary>
    public override string AIModel => "Model AI";

    /// <summary>
    /// Gets the memory label
    /// </summary>
    public override string Memory => "Pamięć";

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
    public override string DoubleClick => "Podwójne kliknięcie";

    /// <summary>
    /// Gets the right-click action text
    /// </summary>
    public override string RightClick => "Prawy przycisk";

    /// <summary>
    /// Gets the show menu text
    /// </summary>
    public override string ShowMenu => "Pokaż menu";

    /// <summary>
    /// Gets the open web interface menu item text
    /// </summary>
    public override string OpenWebInterface => "Otwórz interfejs webowy";

    /// <summary>
    /// Gets the dashboard menu item text
    /// </summary>
    public override string Dashboard => "Panel";

    /// <summary>
    /// Gets the manage silicon beings menu item text
    /// </summary>
    public override string ManageSiliconBeings => "Zarządzaj byciami krzemowymi";

    /// <summary>
    /// Gets the configuration menu item text
    /// </summary>
    public override string Configuration => "Konfiguracja";

    /// <summary>
    /// Gets the Speedy Pack Manager menu item text
    /// </summary>
    public override string SpeedyPackManager => "Menedżer Speedy Pack";

    /// <summary>
    /// Gets the exit menu item text
    /// </summary>
    public override string Exit => "Zakończ";

    /// <summary>
    /// Gets the web server startup error title
    /// </summary>
    public override string WebServerStartupErrorTitle => "Błąd uruchamiania serwera webowego";

    /// <summary>
    /// Gets the web server startup error message
    /// </summary>
    public override string WebServerStartupErrorMessage => "Nie udało się uruchomić serwera webowego. Aplikacja zostanie zamknięta.\n\nBłąd: {0}";
}
