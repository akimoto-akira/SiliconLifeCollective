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
/// Spanish (Spain) tray localization implementation
/// </summary>
public class TrayEsES : TrayLocalizationBase
{
    /// <summary>
    /// Gets the fast edition name
    /// </summary>
    public override string SoftwareName => "Silicon Life Collective";

    /// <summary>
    /// Gets the status label
    /// </summary>
    public override string Status => "Estado";

    /// <summary>
    /// Gets the uptime label
    /// </summary>
    public override string Uptime => "Tiempo de actividad";

    /// <summary>
    /// Gets the running status text
    /// </summary>
    public override string Running => "En ejecución";

    /// <summary>
    /// Gets the shutting down status text
    /// </summary>
    public override string ShuttingDown => "Cerrando";

    /// <summary>
    /// Gets the silicon beings label
    /// </summary>
    public override string SiliconBeings => "Seres de Silicio";

    /// <summary>
    /// Gets the active status text
    /// </summary>
    public override string Active => "activo";

    /// <summary>
    /// Gets the name label
    /// </summary>
    public override string Name => "Nombre";

    /// <summary>
    /// Gets the AI model label
    /// </summary>
    public override string AIModel => "Modelo de IA";

    /// <summary>
    /// Gets the memory label
    /// </summary>
    public override string Memory => "Memoria";

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
    public override string DoubleClick => "Doble clic";

    /// <summary>
    /// Gets the right-click action text
    /// </summary>
    public override string RightClick => "Clic derecho";

    /// <summary>
    /// Gets the show menu text
    /// </summary>
    public override string ShowMenu => "Mostrar menú";

    /// <summary>
    /// Gets the open web interface menu item text
    /// </summary>
    public override string OpenWebInterface => "Abrir interfaz web";

    /// <summary>
    /// Gets the dashboard menu item text
    /// </summary>
    public override string Dashboard => "Panel de control";

    /// <summary>
    /// Gets the manage silicon beings menu item text
    /// </summary>
    public override string ManageSiliconBeings => "Gestionar seres de silicio";

    /// <summary>
    /// Gets the configuration menu item text
    /// </summary>
    public override string Configuration => "Configuración";

    /// <summary>
    /// Gets the Speedy Pack Manager menu item text
    /// </summary>
    public override string SpeedyPackManager => "Administrador Speedy Pack";

    /// <summary>
    /// Gets the exit menu item text
    /// </summary>
    public override string Exit => "Salir";

    /// <summary>
    /// Gets the web server startup error title
    /// </summary>
    public override string WebServerStartupErrorTitle => "Error al iniciar el servidor web";

    /// <summary>
    /// Gets the web server startup error message
    /// </summary>
    public override string WebServerStartupErrorMessage => "El servidor web no pudo iniciarse. La aplicación se cerrará.\n\nError: {0}";
}
