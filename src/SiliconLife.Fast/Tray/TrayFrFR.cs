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
/// French (France) tray localization implementation
/// </summary>
public class TrayFrFR : TrayLocalizationBase
{
    /// <summary>
    /// Gets the fast edition name
    /// </summary>
    public override string SoftwareName => "Silicon Life Collective";

    /// <summary>
    /// Gets the status label
    /// </summary>
    public override string Status => "Statut";

    /// <summary>
    /// Gets the uptime label
    /// </summary>
    public override string Uptime => "Temps de fonctionnement";

    /// <summary>
    /// Gets the running status text
    /// </summary>
    public override string Running => "En cours";

    /// <summary>
    /// Gets the shutting down status text
    /// </summary>
    public override string ShuttingDown => "Arrêt en cours";

    /// <summary>
    /// Gets the silicon beings label
    /// </summary>
    public override string SiliconBeings => "Silicon Beings";

    /// <summary>
    /// Gets the active status text
    /// </summary>
    public override string Active => "actif";

    /// <summary>
    /// Gets the name label
    /// </summary>
    public override string Name => "Nom";

    /// <summary>
    /// Gets the AI model label
    /// </summary>
    public override string AIModel => "Modèle IA";

    /// <summary>
    /// Gets the memory label
    /// </summary>
    public override string Memory => "Mémoire";

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
    public override string DoubleClick => "Double-clic";

    /// <summary>
    /// Gets the right-click action text
    /// </summary>
    public override string RightClick => "Clic droit";

    /// <summary>
    /// Gets the show menu text
    /// </summary>
    public override string ShowMenu => "Afficher le menu";

    /// <summary>
    /// Gets the open web interface menu item text
    /// </summary>
    public override string OpenWebInterface => "Ouvrir l'interface Web";

    /// <summary>
    /// Gets the dashboard menu item text
    /// </summary>
    public override string Dashboard => "Tableau de bord";

    /// <summary>
    /// Gets the manage silicon beings menu item text
    /// </summary>
    public override string ManageSiliconBeings => "Gérer les Silicon Beings";

    /// <summary>
    /// Gets the configuration menu item text
    /// </summary>
    public override string Configuration => "Configuration";

    /// <summary>
    /// Gets the Speedy Pack Manager menu item text
    /// </summary>
    public override string SpeedyPackManager => "Gestionnaire Speedy Pack";

    /// <summary>
    /// Gets the exit menu item text
    /// </summary>
    public override string Exit => "Quitter";

    /// <summary>
    /// Gets the web server startup error title
    /// </summary>
    public override string WebServerStartupErrorTitle => "Échec du démarrage du serveur Web";

    /// <summary>
    /// Gets the web server startup error message
    /// </summary>
    public override string WebServerStartupErrorMessage => "Le serveur Web n'a pas pu démarrer. L'application va se fermer.\n\nErreur : {0}";
}
