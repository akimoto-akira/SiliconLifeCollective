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
using SiliconLife.Common.Calendar.ChineseHistorical;

namespace SiliconLife.Common.Localization;

/// <summary>
/// French (France) localization implementation
/// </summary>
public class FrFR : DefaultLocalizationBase
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "fr-FR";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "Français (France)";

    /// <summary>
    /// Gets the welcome message
    /// </summary>
    public override string WelcomeMessage => "Bienvenue dans le Silicon Life Collective !";

    /// <summary>
    /// Gets the brand name
    /// </summary>
    public override string BrandName => "Silicon Life Collective";

    /// <summary>
    /// Gets the input prompt
    /// </summary>
    public override string InputPrompt => "> ";

    /// <summary>
    /// Gets the shutdown message
    /// </summary>
    public override string ShutdownMessage => "Arrêt en cours...";

    /// <summary>
    /// Gets the config corrupted error message
    /// </summary>
    public override string ConfigCorruptedError => "Fichier de configuration corrompu, utilisation de la configuration par défaut";

    /// <summary>
    /// Gets the config created message
    /// </summary>
    public override string ConfigCreatedWithDefaults => "Fichier de configuration introuvable, configuration par défaut créée";

    /// <summary>
    /// Gets the AI connection error message
    /// </summary>
    public override string AIConnectionError => "Impossible de se connecter au service IA, veuillez vérifier si Ollama est en cours d'exécution";

    /// <summary>
    /// Gets the AI request error message
    /// </summary>
    public override string AIRequestError => "Échec de la requête IA";

    /// <summary>
    /// Gets the data directory create error message
    /// </summary>
    public override string DataDirectoryCreateError => "Impossible de créer le répertoire de données";

    /// <summary>
    /// Gets the thinking message
    /// </summary>
    public override string ThinkingMessage => "Réflexion en cours...";

    /// <summary>
    /// Gets the tool call message
    /// </summary>
    public override string ToolCallMessage => "Exécution des outils...";

    /// <summary>
    /// Gets the error message
    /// </summary>
    public override string ErrorMessage => "Erreur";

    /// <summary>
    /// Gets the unexpected error message
    /// </summary>
    public override string UnexpectedErrorMessage => "Erreur inattendue";

    /// <summary>
    /// Gets the permission denied message
    /// </summary>
    public override string PermissionDeniedMessage => "Permission refusée";

    /// <summary>
    /// Gets the permission ask prompt
    /// </summary>
    public override string PermissionAskPrompt => "Autoriser ? (o/n) : ";

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public override string PermissionRequestHeader => "[Demande d'autorisation]";
    public override string PermissionRequestDescription => "Un Silicon Being demande votre autorisation :";
    public override string PermissionRequestTypeLabel => "Type d'autorisation :";
    public override string PermissionRequestResourceLabel => "Ressource demandée :";
    public override string PermissionRequestAllowButton => "Autoriser";
    public override string PermissionRequestDenyButton => "Refuser";
    public override string PermissionRequestCacheLabel => "Mémoriser cette décision";
    public override string PermissionRequestDurationLabel => "Durée du cache";
    public override string PermissionRequestWaitingMessage => "En attente de réponse...";

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public override string AllowCodeLabel => "Code d'autorisation";

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public override string DenyCodeLabel => "Code de refus";

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public override string PermissionReplyInstruction => "Entrez le code de confirmation ou tout autre texte pour refuser";

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public override string AddToCachePrompt => "Mémoriser cette décision ? (o/n) : ";

    /// <summary>
    /// Gets the label for the permission cache checkbox in the web UI
    /// </summary>
    public override string PermissionCacheLabel => "Mémoriser cette décision";

    /// <summary>
    /// Gets the label for the cache duration selector in the permission dialog
    /// </summary>
    public override string PermissionCacheDurationLabel => "Durée du cache";

    /// <summary>
    /// Gets the option text for 1-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration1Hour => "1 heure";

    /// <summary>
    /// Gets the option text for 24-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration24Hours => "24 heures";

    /// <summary>
    /// Gets the option text for 7-day cache duration
    /// </summary>
    public override string PermissionCacheDuration7Days => "7 jours";

    /// <summary>
    /// Gets the option text for 30-day cache duration
    /// </summary>
    public override string PermissionCacheDuration30Days => "30 jours";

    /// <summary>
    /// Gets the prefix for project group chat session names
    /// </summary>
    public override string ProjectGroupChatPrefix => "Groupe de projet";

    /// <summary>
    /// Gets the prefix for project broadcast channel names
    /// </summary>
    public override string ProjectBroadcastPrefix => "Diffusion de projet";

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "Accès réseau",
        PermissionType.CommandLine => "Exécution en ligne de commande",
        PermissionType.FileAccess => "Accès fichier",
        PermissionType.Function => "Appel de fonction",
PermissionType.DataAccess => "Accès aux données",
PermissionType.ToolAction => "Action d'outil",
_ => permissionType.ToString()
    };

    /// <summary>
    /// Gets the title text for the permission dialog in the web UI
    /// </summary>
    public override string PermissionDialogTitle => "Demande d'autorisation";

    /// <summary>
    /// Gets the label for the permission type field in the permission dialog
    /// </summary>
    public override string PermissionTypeLabel => "Type d'autorisation :";

    /// <summary>
    /// Gets the label for the requested resource field in the permission dialog
    /// </summary>
    public override string PermissionResourceLabel => "Ressource demandée :";

    /// <summary>
    /// Gets the label for the detail information field in the permission dialog
    /// </summary>
    public override string PermissionDetailLabel => "Informations détaillées :";

    /// <summary>
    /// Gets the text for the allow button in the permission dialog
    /// </summary>
    public override string PermissionAllowButton => "Autoriser";

    /// <summary>
    /// Gets the text for the deny button in the permission dialog
    /// </summary>
    public override string PermissionDenyButton => "Refuser";

    /// <summary>
    /// Gets the console error message when permission respond fails
    /// </summary>
    public override string PermissionRespondFailed => "Échec de la réponse d'autorisation";

    /// <summary>
    /// Gets the console error prefix when permission respond throws an error
    /// </summary>
    public override string PermissionRespondError => "Erreur de réponse d'autorisation : ";

    // ===== Init Page Localization =====

    public override string InitPageTitle => "Initialisation";
    public override string InitDescription => "Première utilisation, veuillez compléter la configuration de base";
    public override string InitNicknameLabel => "Nom d'utilisateur";
    public override string InitNicknamePlaceholder => "Veuillez entrer votre pseudonyme";
    public override string InitEndpointLabel => "Point de terminaison API IA";
    public override string InitEndpointPlaceholder => "ex : http://localhost:11434";
    public override string InitAIClientTypeLabel => "Type de client IA";
    public override string InitModelLabel => "Modèle par défaut";
    public override string InitModelPlaceholder => "ex : qwen3.5:cloud";
    public override string InitSkinLabel => "Thème";
    public override string InitSkinPlaceholder => "Laisser vide pour le thème par défaut";
    public override string InitDataDirectoryLabel => "Répertoire de données";
    public override string InitDataDirectoryPlaceholder => "ex : ./data";
    public override string InitDataDirectoryBrowse => "Parcourir...";
    public override string InitSkinSelected => "\u2713 Sélectionné";
    public override string InitSkinPreviewTitle => "Aperçu";
    public override string InitSkinPreviewCardTitle => "Titre de la carte";
    public override string InitSkinPreviewCardContent => "Ceci est un exemple de carte montrant l'effet visuel de ce thème.";
    public override string InitSkinPreviewPrimaryBtn => "Bouton principal";
    public override string InitSkinPreviewSecondaryBtn => "Bouton secondaire";
    public override string InitSubmitButton => "Terminer l'initialisation";
    public override string InitFooterHint => "La configuration peut être modifiée à tout moment dans les paramètres";
    public override string InitHelpLink => "📖 Afficher la documentation d'aide";
    public override string InitAIClientHelpPrefix => "📖 Afficher l'aide : ";
    public override string InitNicknameRequiredError => "Veuillez entrer un nom d'utilisateur";
    public override string InitDataDirectoryRequiredError => "Veuillez sélectionner un répertoire de données";
    public override string InitCuratorNameLabel => "Nom du Silicon Being";
    public override string InitCuratorNamePlaceholder => "Veuillez entrer le nom du premier Silicon Being";
    public override string InitCuratorNameRequiredError => "Veuillez entrer un nom de Silicon Being";
    public override string InitLanguageLabel => "Langue / Language";
    public override string InitLanguageSwitchBtn => "Appliquer";

    // ===== Navigation Menu Localization =====

    public override string NavMenuChat => "Chat";
    public override string NavMenuDashboard => "Tableau de bord";
    public override string NavMenuBeings => "Silicon Beings";
    public override string NavMenuUsage => "Utilisation";
    public override string NavMenuAudit => "Audit";
    public override string NavMenuTasks => "Tâches";
    public override string NavMenuMemory => "Mémoire";
    public override string NavMenuKnowledge => "Connaissances";
    public override string NavMenuProjects => "Projets";
    public override string NavMenuLogs => "Journaux";
    public override string NavMenuConfig => "Configuration";
    public override string NavMenuHelp => "Aide";
    public override string NavMenuAbout => "À propos";

    // ===== Page Title Localization =====

    public override string PageTitleChat => "Chat - Silicon Life Collective";
    public override string PageTitleDashboard => "Tableau de bord - Silicon Life Collective";
    public override string PageTitleBeings => "Gestion des Silicon Beings - Silicon Life Collective";
    public override string PageTitleTasks => "Gestion des tâches - Silicon Life Collective";
    public override string PageTitleTimers => "Gestion des minuteurs - Silicon Life Collective";
    public override string PageTitleMemory => "Recherche en mémoire - Silicon Life Collective";
    public override string PageTitleWorkNotes => "Notes de travail - Silicon Life Collective";
    public override string PageTitleKnowledge => "Graphe de connaissances - Silicon Life Collective";
    public override string PageTitleProjects => "Gestion des espaces projet - Silicon Life Collective";
    public override string PageTitleLogs => "Consultation des journaux - Silicon Life Collective";
    public override string PageTitleUsage => "Utilisation de tokens - Silicon Life Collective";
    public override string PageTitleAudit => "Audit des permissions - Silicon Life Collective";
    public override string PageTitleConfig => "Configuration système - Silicon Life Collective";
    public override string PageTitleExecutor => "Surveillance des exécuteurs - Silicon Life Collective";
    public override string PageTitleCodeBrowser => "Navigateur de code - Silicon Life Collective";
    public override string PageTitlePermission => "Gestion des autorisations - Silicon Life Collective";
    public override string PageTitleAbout => "À propos - Silicon Life Collective";

    // ===== Memory Page Localization =====

    public override string MemoryPageHeader => "Consultation de la mémoire";
    public override string WorkNotesPageHeader => "Notes de travail";
    public override string WorkNotesBackToPrevious => "← Retour";
    public override string WorkNotesTotalPages => "Total {0} pages";
    public override string WorkNotesEmptyState => "Aucune note de travail pour le moment";
    public override string WorkNotesSearchPlaceholder => "Rechercher des notes...";
    public override string WorkNotesSearchButton => "Rechercher";
    public override string WorkNotesNoSearchResults => "Aucune note correspondante trouvée";
    public override string MemoryEmptyState => "Aucune donnée en mémoire pour le moment";
    public override string MemorySearchPlaceholder => "Rechercher dans la mémoire...";
    public override string MemorySearchButton => "Rechercher";
    public override string MemoryFilterAll => "Tout";
    public override string MemoryFilterSummaryOnly => "Résumés uniquement";
    public override string MemoryFilterOriginalOnly => "Originaux uniquement";
    public override string MemoryStatTotal => "Total des mémoires";
    public override string MemoryStatOldest => "Mémoire la plus ancienne";
    public override string MemoryStatNewest => "Mémoire la plus récente";
    public override string MemoryIsSummaryBadge => "Résumé compressé";
    public override string MemoryPaginationPrev => "Page précédente";
    public override string MemoryPaginationNext => "Page suivante";
    public override string MemoryFilterTypeLabel => "Type";
    public override string MemoryFilterDateFrom => "Date de début";
    public override string MemoryFilterDateTo => "Date de fin";
    public override string MemoryFilterApply => "Appliquer";
    public override string MemoryFilterReset => "Réinitialiser";
    public override string MemoryTypeChat => "Conversation";
    public override string MemoryTypeToolCall => "Appel d'outil";
    public override string MemoryTypeTask => "Tâche";
    public override string MemoryTypeTimer => "Minuteur";
    public override string MemoryDetailTitle => "Détails de la mémoire";
    public override string MemoryDetailClose => "Fermer";
    public override string MemoryDetailId => "ID";
    public override string MemoryDetailContent => "Contenu";
    public override string MemoryDetailCreatedAt => "Date de création";
    public override string MemoryDetailRelatedBeings => "Agents liés";
    public override string MemoryDetailKeywords => "Mots-clés";
    public override string MemoryStatTypeDistribution => "Distribution par type";
    public override string MemoryStatKeywordFrequency => "Fréquence des mots-clés";
    public override string MemoryCardViewDetail => "Voir les détails";
    public override string MemoryTimelineEmptyState => "Aucune donnée en mémoire";
    public override string MemoryYearSummaryLabel => "Résumé annuel";
    public override string MemoryMonthSummaryLabel => "Résumé mensuel";
    public override string MemoryDaySummaryLabel => "Résumé quotidien";
    public override string MemoryHourSummaryLabel => "Résumé horaire";
    public override string MemoryMinuteSummaryLabel => "Résumé par minute";
    public override string MemorySummaryBadge => "Résumé compressé";
    public override string MemoryTimelineYearFormat => "{0} ({1} entrées)";
    public override string MemoryTimelineMonthFormat => "{0}/{1} ({2} entrées)";
    public override string MemoryTimelineDayFormat => "{0}-{1}-{2} ({3} entrées)";
    public override string MemoryTimelineHourFormat => "{0}:00 ({1} entrées)";
    public override string MemoryTimelineMinuteFormat => "{0}:{1} ({2} entrées)";
    public override string MemoryRelatedBeingsLabel => "👥 Liés : {0} êtres";

    // ===== Projects Page Localization =====

    public override string ProjectsPageHeader => "Gestion des espaces projet";
    public override string ProjectsEmptyState => "Aucun projet pour le moment";
    public override string ProjectsActiveLabel => "Actif";
    public override string ProjectsArchivedLabel => "Archivé";

    public override string ProjectStatusActiveLabel => "Actif";

    public override string ProjectStatusArchivedLabel => "Archivé";

    public override string ProjectStatusDestroyedLabel => "Détruit";

    public override string ProjectTasksLinkLabel => "Tâches";

    public override string ProjectWorkNotesLinkLabel => "Notes de travail";
    public override string ProjectWorkflowsLinkLabel => "Flux de travail";
    public override string ProjectGroupChatLinkLabel => "Chat de groupe";
    public override string ProjectBroadcastLinkLabel => "Diffusion";
    public override string ProjectWorkflowsPageHeader => "Flux de travail du projet";
    public override string ProjectWorkflowsEmptyState => "Ce projet n'a pas encore de flux de travail";
    public override string CreateWorkflowButton => "Créer un flux de travail";
    public override string ActiveWorkflowsHeader => "Flux de travail actifs";
    public override string BackToProject => "Retour au projet";
    public override string WorkflowCurrentStateLabel => "État actuel :";
    public override string WorkflowCreatedByLabel => "Créé par :";
    public override string WorkflowUpdatedAtLabel => "Mis à jour le :";
    public override string WorkflowBusinessKeyPrompt => "Veuillez entrer la clé métier (ex : numéro PR, ID d'incident) :";
    public override string WorkflowCreatedSuccess => "Flux de travail créé avec succès !";
    public override string WorkflowCreateFailed => "Échec de la création :";
    public override string WorkflowDetailInProgress => "Fonctionnalité de détail du flux de travail en cours de développement...";
    public override string WorkflowInstanceIdLabel => "ID d'instance :";
    public override string WorkflowDetailPageHeader => "Détails du flux de travail";
    public override string WorkflowRoleAssignmentsHeader => "Attributions des rôles";
    public override string WorkflowUnassignedBeingsHeader => "Êtres de silicium non attribués";
    public override string WorkflowNoUnassignedBeings => "Tous les êtres de silicium sont attribués à des rôles";
    public override string WorkflowStateTransitionsHeader => "Transitions d'état des tâches";
    public override string WorkflowNoTemplateMessage => "Aucun modèle de flux de travail associé à ce projet";
    public override string WorkflowNoRoleDefinitions => "Ce modèle de flux de travail ne définit aucun rôle";
    public override string WorkflowNoTransitions => "Ce modèle de flux de travail ne définit aucune transition d'état";
    public override string WorkflowRoleAssignedCountLabel => "Attribués";
    public override string WorkflowRoleRequiredCountLabel => "Requis";
    public override string WorkflowTransitionFromLabel => "De";
    public override string WorkflowTransitionToLabel => "Vers";
    public override string WorkflowAssignRoleButton => "Attribuer un rôle";
    public override string WorkflowRemoveFromRoleButton => "Retirer";
    public override string WorkflowTerminalStateLabel => "Terminal";
    public override string WorkflowInitialStateLabel => "Initial";
    public override string ProjectWorkNotesPageHeader => "Notes de travail du projet";
    public override string ProjectWorkNotesEmptyState => "Ce projet n'a pas encore de notes de travail";
    public override string ProjectWorkNotesTotalPages => "Total des pages : {0}";

    // ===== Code Browser Page Localization =====

    public override string CodeBrowserPageHeader => "Navigateur de code";

    // ===== Tasks Page Localization =====

    public override string TasksPageHeader => "Gestion des tâches";
    public override string TasksEmptyState => "Aucune tâche pour le moment";
    public override string TasksStatusPending => "En attente";
    public override string TasksStatusRunning => "En cours";
    public override string TasksStatusCompleted => "Terminé";
    public override string TasksStatusFailed => "Échoué";
    public override string TasksStatusCancelled => "Annulé";
    public override string TasksPriorityLabel => "Priorité";
    public override string TasksAssignedToLabel => "Responsable";
    public override string TasksCreatedAtLabel => "Date de création";
    public override string TaskViewExecutionHistory => "Voir l'historique d'exécution";

    public override string ProjectTasksPageHeader => "Tâches du projet";

    public override string ProjectTasksEmptyState => "Aucune tâche de projet pour le moment";

    public override string ProjectTasksAssigneesLabel => "Responsables";

    public override string ProjectTasksCreatedByLabel => "Créé par";

    public override string ProjectTasksBackToProjects => "← Retour à la liste des projets";

    public override string ProjectTasksNoAssigneesLabel => "Aucun";

    public override string ProjectCreateButton => "Créer un projet";
    public override string ProjectCreateModalTitle => "Créer un nouveau projet";
    public override string ProjectCreateNameLabel => "Nom du projet";
    public override string ProjectCreateDescriptionLabel => "Description";
    public override string ProjectCreateWorkflowLabel => "Modèle de workflow";
    public override string ProjectCreateNoWorkflow => "Aucun (dirigé par le curator)";
    public override string ProjectCreateSubmitButton => "Créer";
    public override string ProjectCreateCancelButton => "Annuler";
    public override string ProjectCreateNameRequired => "Le nom du projet est requis";
    public override string ProjectCreateSuccess => "Projet créé avec succès";

    // ===== Executor Page Localization =====

    public override string ExecutorPageHeader => "Surveillance des exécuteurs";

    // ===== Permission Page Localization =====

    public override string PermissionPageHeader => "Gestion des autorisations";
    public override string PermissionEmptyState => "Aucune règle d'autorisation pour le moment";
    public override string PermissionMissingBeingId => "Paramètre ID du Silicon Being manquant";
    public override string PermissionBeingNotFound => "Silicon Being non trouvé";
    public override string PermissionTemplateHeader => "Modèle de rappel d'autorisation par défaut";
    public override string PermissionTemplateDescription => "Après enregistrement, le comportement par défaut sera remplacé ; après suppression, il sera restauré";
    public override string PermissionCallbackClassSummary => "Implémentation du rappel d'autorisation.";
    public override string PermissionCallbackClassSummary2 => "Règles d'autorisation spécifiques au domaine, entièrement conformes à la spécification dpf.txt.\n/// Couverture : Réseau (liste blanche/noire/plages IP), Ligne de commande (multiplateforme),\n/// Accès aux fichiers (extensions dangereuses, répertoires système, répertoires utilisateur) et valeurs par défaut de secours.";
    public override string PermissionCallbackConstructorSummary => "Crée un PermissionCallback avec le répertoire de données d'application.";
    public override string PermissionCallbackConstructorSummary2 => "Le répertoire de données d'application est utilisé pour :\n    /// - Bloquer l'accès au répertoire de données (sauf son propre sous-dossier temporaire)\n    /// - Dériver le répertoire de données par Silicon Being pour les règles d'autorisation temporaires";
    public override string PermissionCallbackConstructorParam => "Chemin vers le répertoire de données d'application global";
    public override string PermissionCallbackEvaluateSummary => "Évalue une demande d'autorisation selon les règles (spécification dpf.txt).";
    public override string PermissionRuleOtherTypesDefault => "Les autres types d'autorisation sont autorisés par défaut";

    public override string GetPermissionRuleComment(string key) => key switch
    {
        "NetRuleNetworkAccess" => "Règle d'autorisation d'opération réseau",
        "NetRuleCommandLine" => "Règle de ligne de commande (multiplateforme)",
        "NetRuleFileAccess" => "Règle d'accès aux fichiers (multiplateforme)",
        "NetRuleNoProtocol" => "Pas de nom de protocole (pas de deux-points), source impossible à déterminer, demander à l'utilisateur",
        "NetRuleLoopback" => "Autoriser l'adresse de bouclage (localhost / 127.0.0.1 / ::1)",
        "NetRulePrivateIPMatch" => "Correspondance de plage d'adresses IP privées (vérifier d'abord la première adresse IPv4 valide)",
        "NetRulePrivateC" => "Autoriser la plage d'adresses privées de classe C (192.168.0.0/16)",
        "NetRulePrivateA" => "Autoriser la plage d'adresses privées de classe A (10.0.0.0/8)",
        "NetRulePrivateB" => "Autoriser sélectivement la plage d'adresses privées de classe B (172.16.0.0/12, soit 172.16.* ~ 172.31.*)",
        "NetRuleDomainWhitelist1" => "Liste blanche de domaines externes autorisés — Google / Bing / Tencent / Sogou / DuckDuckGo / Yandex / WeChat / Alibaba",
        "NetRuleVideoPlatforms" => "Bilibili / niconico / Acfun / Douyin / TikTok / Kuaishou / Xiaohongshu",
        "NetRuleAIServices" => "Services IA — OpenAI / Anthropic / HuggingFace / Ollama / Tongyi Qianwen / Kimi / Doubao / Jianying / Trae IDE",
        "NetRulePhishingBlacklist" => "Liste noire de sites de phishing/imitation (correspondance floue par mots-clés)",
        "NetRulePhishingAI" => "Site d'imitation IA",
        "NetRuleMaliciousAI" => "Outil IA malveillant",
        "NetRuleAdversarialAI" => "IA adverse / Prompt jailbreak / Sites d'attaque LLM",
        "NetRuleAIContentFarm" => "Ferme de contenu IA / Contenu spam IA",
        "NetRuleAIBlackMarket" => "Marché noir de données IA / Marché noir de clés API / Vente de poids LLM",
        "NetRuleAIFakeScam" => "Imitation/escroquerie IA — mots-clés généraux",
        "NetRuleOtherBlacklist" => "Autres sites sur liste noire — sakura-cat : ne devrait pas être accédé par l'IA / 4399 : jeux mélangés avec des virus",
        "NetRuleSecuritiesTrading" => "Plateforme de trading de valeurs mobilières (demander à l'utilisateur) — Huatai Securities / Guotai Junan / CITIC Securities / China Merchants Securities / GF Securities / Haitong Securities / Shenwan Hongyuan / Orient Securities / Guosen Securities / Industrial Securities",
        "NetRuleThirdPartyTrading" => "Trading de plateforme tierce (demander à l'utilisateur) — Tonghuashun / East Money / Tongdaxin / Bloomberg / Yahoo Finance",
        "NetRuleStockExchanges" => "Bourses de valeurs (données de marché uniquement) — Shanghai Stock Exchange / Shenzhen Stock Exchange / CNINFO",
        "NetRuleFinancialNews" => "Actualités financières (données de marché uniquement) — JRJ / Securities Times / Hexun",
        "NetRuleInvestCommunity" => "Communauté d'investissement (informations uniquement) — Xueqiu / CLS / Kaipanla / Taoguba",
        "NetRuleDevServices" => "Services de développement — GitHub / Gitee / StackOverflow / npm / NuGet / PyPI / Microsoft",
        "NetRuleGameEngines" => "Moteurs de jeu — Unity / Unreal Engine / Epic Games / Fab Resource Store",
        "NetRuleGamePlatforms" => "Plateformes de jeu — Steam demander à l'utilisateur, EA / Ubisoft / Blizzard / Nintendo autoriser",
        "NetRuleSEGA" => "SEGA (Japon)",
        "NetRuleCloudServices" => "Plateformes de services cloud mondiales — Azure / Google Cloud / DigitalOcean / Heroku / Vercel / Netlify",
        "NetRuleDevDeployTools" => "Outils de développement et de déploiement mondiaux — GitLab / Bitbucket / Docker / Cloudflare",
        "NetRuleCloudDevTools" => "Services cloud et outils de développement — Amazon / AWS / Kiro IDE / CodeBuddy IDE / JetBrains / Chenguang Studio / W3School Chinese",
        "NetRuleChinaSocialNews" => "Social/Actualités (Chine continentale) — Weibo / Zhihu / NetEase / Sina / ifeng / Xinhua / CCTV",
        "NetRuleTaiwanMediaCTI" => "Médias de Taïwan — CTI News",
        "NetRuleTaiwanMediaSET" => "SET News (Taïwan) — Demander à l'utilisateur",
        "NetRuleTaiwanWIN" => "Agence de protection du contenu Internet (Taïwan, risque de blocage) — Interdit",
        "NetRuleJapanMedia" => "Médias japonais — NHK",
        "NetRuleRussianMedia" => "Médias russes — Sputnik News",
        "NetRuleKoreanMedia" => "Médias coréens — KBS / MBC / SBS / EBS",
        "NetRuleDPRKMedia" => "Médias nord-coréens — Uriminzokkiri / Rodong Sinmun / Youth Vanguard / Voice of Korea / Pyongyang Times / Chongryon",
        "NetRuleGovWebsites" => "Sites gouvernementaux (domaine générique .gov)",
        "NetRuleGlobalSocialCollab" => "Plateformes sociales/collaboratives mondiales — Reddit / Discord / Slack / Notion / Figma / Dropbox",
        "NetRuleOverseasSocial" => "Social/Livestreaming international (demander à l'utilisateur) — Twitch / Facebook / X / Gmail / Instagram / lit.link",
        "NetRuleWhatsApp" => "WhatsApp (Meta) — Autoriser",
        "NetRuleThreads" => "Threads (Meta) — Interdit",
        "NetRuleGlobalVideoMusic" => "Plateformes vidéo/musique mondiales — Spotify / Apple Music / Vimeo",
        "NetRuleVideoMedia" => "Vidéo/Médias — YouTube / iQIYI / Youku",
        "NetRuleMaps" => "Cartes — OpenStreetMap",
        "NetRuleEncyclopedia" => "Encyclopédie — Wikipedia / MediaWiki / Creative Commons (CC)",
        "NetRuleUnmatched" => "Accès réseau non correspondant, demander à l'utilisateur",
        "CmdRuleSeparatorDetect" => "Détecter les séparateurs de pipe et de commandes multiples, valider individuellement",
        "CmdRuleWinAllow" => "Windows autorisé : Commandes en lecture seule/requête — dir / tree / tasklist / ipconfig / ping / tracert / systeminfo / whoami / set / path / sc query / findstr",
        "CmdRuleWinDeny" => "Windows interdit : Commandes dangereuses/destructrices — del / rmdir / format / diskpart / reg delete",
        "CmdRuleLinuxAllow" => "Linux autorisé : Commandes en lecture seule/requête — ls / tree / ps / top / ifconfig / ip / ping / traceroute / uname / whoami / env / cat / grep / find / df / du / systemctl status",
        "CmdRuleLinuxDeny" => "Linux interdit : Commandes dangereuses/destructrices — rm / rmdir / mkfs / fdisk / dd / chmod / chown / chgrp",
        "CmdRuleMacAllow" => "macOS autorisé : Commandes en lecture seule/requête — ls / tree / ps / top / ifconfig / ping / traceroute / system_profiler / sw_vers / whoami / env / cat / grep / find / df / du / launchctl list",
        "CmdRuleMacDeny" => "macOS interdit : Commandes dangereuses/destructrices — rm / rmdir / diskutil erasedisk / dd / chmod / chown / chgrp",
        "CmdRuleUnmatched" => "Commande non correspondante, demander à l'utilisateur",
        "FileRuleDangerousExt" => "Priorité maximale : Extensions de fichier dangereuses à refuser immédiatement",
        "FileRuleInvalidPath" => "Impossible de résoudre en chemin absolu, demander à l'utilisateur",
        "FileRuleDenyAssemblyDir" => "Interdit : Répertoire de l'assembly actuel",
        "FileRuleDenyAppDataDir" => "Interdit : Répertoire des données d'application",
        "FileRuleAllowOwnTemp" => "Mais autorisé : Propre répertoire temporaire",
        "FileRuleOwnTemp" => "Autorisé : Propre répertoire temporaire",
        "FileRuleDenyOtherDataDir" => "Interdit : Autres chemins de données (y compris ceux d'autres Silicon Beings)",
        "FileRuleUserFolders" => "Autorisé : Dossiers utilisateur par défaut",
        "FileRuleUserFolderCheck" => "Dossiers utilisateur par défaut — Bureau / Téléchargements / Documents / Images / Musique / Vidéos",
        "FileRulePublicFolders" => "Autorisé : Dossiers publics utilisateur",
        "FileRuleWinDenySystem" => "Windows interdit : Répertoires système critiques (pas nécessairement sur C:)",
        "FileRuleWinDenySystemCheck" => "Répertoires système critiques",
        "FileRuleLinuxDenySystem" => "Linux interdit : Répertoires système critiques — /etc /boot /sbin",
        "FileRuleMacDenySystem" => "macOS interdit : Répertoires système critiques — /System /Library /private/etc",
        "FileRuleUnmatched" => "Chemin non correspondant, demander à l'utilisateur",
        _ => key
    };

    public override string PermissionRulesSection => "Liste des règles d'autorisation";
    public override string PermissionEditorSection => "Éditeur de règles d'autorisation";

    public override string PermissionSaveMissingBeingId => "ID de Silicon Being manquant ou invalide";
    public override string PermissionSaveMissingCode => "Code manquant dans le corps de la requête";
    public override string PermissionSaveLoaderNotAvailable => "DynamicBeingLoader non disponible";
    public override string PermissionSaveRemoveFailed => "Échec de la suppression du rappel d'autorisation";
    public override string PermissionSaveRemoveSuccess => "Rappel d'autorisation supprimé";
    public override string PermissionSaveSecurityScanFailed => "Échec de l'enregistrement du rappel d'autorisation (échec de l'analyse de sécurité)";
    public override string PermissionSaveCompilationFailed => "Échec de la compilation";
    public override string PermissionSaveSuccess => "Rappel d'autorisation enregistré et appliqué avec succès";
    public override string PermissionSaveError => "Erreur lors de l'enregistrement du rappel d'autorisation";

    // ===== Knowledge Page Localization =====

    public override string KnowledgePageHeader => "Visualisation du graphe de connaissances";
    public override string KnowledgeLoadingState => "Chargement des données du graphe de connaissances...";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "Chat avec {0}";
    public override string ChatConversationsHeader => "Conversations";
    public override string ChatNoConversationSelected => "Sélectionnez une conversation pour chatter";
    public override string ChatMessageInputPlaceholder => "Entrez un message...";
    public override string ChatLoading => "Chargement...";
    public override string ChatSendButton => "Envoyer";
    public override string ChatFileSourceDialogTitle => "Choisir la source du fichier";
    public override string ChatFileSourceServerFile => "Choisir un fichier serveur";
    public override string ChatFileSourceUploadLocal => "Télécharger un fichier local";
    public override string ChatUserDisplayName => "Moi";
    public override string ChatUserAvatarName => "Moi";
    public override string ChatDefaultBeingName => "IA";
    public override string ChatThinkingSummary => "💭 Processus de réflexion (cliquer pour développer)";
    public override string GetChatToolCallsSummary(int count) => $"🔧 Appels d'outils ({count} entrées)";

    // ===== Dashboard Localization =====

    public override string DashboardPageHeader => "Tableau de bord";
    public override string DashboardStatTotalBeings => "Nombre de Silicon Beings";
    public override string DashboardStatActiveBeings => "Silicon Beings actifs";
    public override string DashboardStatUptime => "Temps de fonctionnement";
    public override string DashboardStatMemory => "Utilisation mémoire";
    public override string DashboardChartMessageFrequency => "Fréquence des messages";

    // ===== Beings Localization =====

    public override string BeingsPageHeader => "Gestion des Silicon Beings";
    public override string BeingsTotalCount => "Total {0} Silicon Beings";
    public override string BeingsNoSelectionPlaceholder => "Sélectionnez un Silicon Being pour les détails";
    public override string BeingsEmptyState => "Aucun Silicon Being pour le moment";
    public override string BeingsStatusIdle => "Inactif";
    public override string BeingsStatusRunning => "En cours";
    public override string BeingsDetailIdLabel => "ID : ";
    public override string BeingsDetailStatusLabel => "Statut : ";
    public override string BeingsDetailCustomCompileLabel => "Compilation personnalisée : ";
    public override string BeingsDetailSoulContentLabel => "Contenu de l'âme : ";
    public override string BeingsDetailSoulContentEditLink => "Modifier l'âme";
    public override string BeingsBackToList => "Retour à la liste";
    public override string SoulEditorSubtitle => "Édition du fichier Soul du Silicon Being (format Markdown)";
    public override string BeingsDetailMemoryLabel => "Mémoire : ";
    public override string BeingsDetailMemoryViewLink => "Voir";
    public override string BeingsDetailPermissionLabel => "Autorisation : ";
    public override string BeingsDetailPermissionEditLink => "Modifier";
    public override string BeingsDetailTimersLabel => "Minuteurs : ";
    public override string BeingsDetailTasksLabel => "Tâches : ";
    public override string BeingsDetailAIClientLabel => "Client IA indépendant : ";
    public override string BeingsDetailAIClientEditLink => "Modifier";
    public override string BeingsDetailChatHistoryLabel => "Historique de chat : ";
    public override string BeingsDetailWorkNoteLabel => "Note de travail : ";
    public override string BeingsDetailChatHistoryLink => "Voir l'historique";
    public override string BeingsDetailWorkNoteLink => "Voir la note de travail";
    public override string BeingsDetailToolAuthLabel => "Autorisation d'outil : ";
    public override string BeingsDetailToolAuthEditLink => "Configurer";
    public override string ToolAuthPageTitle => "Autorisation d'outils";
    public override string ToolAuthPageHeader => "Configuration des autorisations d'outils";
    public override string ToolAuthTemplateLabel => "Modèle prédéfini";
    public override string ToolAuthSaveButton => "Enregistrer";
    public override string ToolAuthSelectAll => "Tout sélectionner";
    public override string ToolAuthDeselectAll => "Tout désélectionner";
    public override string ToolAuthNoRestrictions => "Sans restriction";
    public override string ToolAuthHasRestrictions => "Avec restrictions";
    public override string ToolAuthSaveSuccess => "Autorisations d'outils enregistrées avec succès";
    public override string ToolAuthSaveFailed => "Échec de l'enregistrement";
    public override string ToolAuthDialogClose => "Fermer";
    public override string ToolAuthNoDeclaredActions => "Non configurable";
    public override string WorkNotePageTitle => "Notes de travail";
    public override string WorkNotePageHeader => "Liste des notes de travail";
    public override string WorkNotePageDescription => "Gestion et consultation des notes de travail du Silicon Being";
    public override string ChatHistoryPageTitle => "Historique de chat";
    public override string ChatHistoryPageHeader => "Liste des conversations";
    public override string ChatHistoryConversationList => "Liste des conversations";
    public override string ChatHistoryBackToList => "Retour à la liste des conversations";
    public override string ChatHistoryNoConversations => "Aucun enregistrement de conversation pour le moment";
    public override string ChatDetailPageTitle => "Détails du chat";
    public override string ChatDetailPageHeader => "Détails de la conversation";
    public override string ChatDetailNoMessages => "Aucun message pour le moment";
    public override string ChatDetailMembers => "Membres";
    public override string BeingsYes => "Oui";
    public override string BeingsNo => "Non";
    public override string BeingsNotSet => "Non défini";

    // ===== Timers Page Localization =====

    public override string TimersPageHeader => "Gestion des minuteurs";
    public override string TimersTotalCount => "Total {0} minuteurs";
    public override string TimersEmptyState => "Aucun minuteur pour le moment";
    public override string TimerViewExecutionHistory => "📝 Voir l'historique d'exécution";
    public override string TimerExecutionHistoryTitle => "Historique d'exécution du minuteur";
    public override string TimerExecutionHistoryHeader => "Journal d'exécution";
    public override string TimerExecutionBackToTimers => "← Retour à la liste des minuteurs";
    public override string TimerExecutionTimerName => "Minuteur : {0}";
    public override string TimerExecutionDetailTitle => "Détails de l'exécution";
    public override string TimerExecutionDetailHeader => "Journal des messages d'exécution";
    public override string TimerExecutionNoRecords => "Aucun journal d'exécution pour le moment";
    public override string TaskExecutionHistoryTitle => "Historique d'exécution des tâches";
    public override string TaskExecutionHistoryHeader => "Historique d'exécution";
    public override string TaskExecutionBackToTasks => "← Retour aux tâches";
    public override string TaskExecutionTaskName => "Tâche : {0}";
    public override string TaskExecutionDetailTitle => "Détail d'exécution de tâche";
    public override string TaskExecutionDetailHeader => "Détail d'exécution";
    public override string TaskExecutionNoRecords => "Aucun journal d'exécution pour le moment";
    public override string TimersStatusActive => "Actif";
    public override string TimersStatusPaused => "En pause";
    public override string TimersStatusTriggered => "Déclenché";
    public override string TimersStatusCancelled => "Annulé";
    public override string TimersTypeRecurring => "Récurrent";
    public override string TimersTriggerTimeLabel => "Heure de déclenchement : ";
    public override string TimersIntervalLabel => "Intervalle : ";
    public override string TimersCalendarLabel => "Condition calendaire : ";
    public override string TimersTriggeredCountLabel => "Déclenché : ";

    // ===== About Page Localization =====

    public override string AboutPageHeader => "À propos";
    public override string AboutAppName => "Silicon Life Collective";
    public override string AboutVersionLabel => "Version";
    public override string AboutDescription => "Un système de gestion Silicon Life Collective basé sur l'IA, prenant en charge la collaboration multi-agents IA, la gestion de la mémoire, la construction de graphes de connaissances et d'autres fonctionnalités.";
    public override string AboutAuthorLabel => "Auteur";
    public override string AboutAuthorName => "Hoshino Kennji";
    public override string AboutLicenseLabel => "Licence";
    public override string AboutCopyright => "Copyright (c) 2026 Hoshino Kennji";
    public override string AboutGitHubLink => "Dépôt GitHub";
    public override string AboutGiteeLink => "Miroir Gitee";
    public override string AboutSocialMediaLabel => "Plateformes de médias sociaux";
public override string AboutPluginListLabel => "Liste des plugins";
public override string AboutFailedPluginListLabel => "Plugins échoués au chargement";
public override string GetSocialMediaName(string platform) => platform switch
    {
        "Bilibili" => "Bilibili",
        "YouTube" => "YouTube",
        "X" => "X (Twitter)",
        "Douyin" => "Douyin",
        "Weibo" => "Weibo",
        "WeChat" => "WeChat Compte officiel",
        "Xiaohongshu" => "Xiaohongshu",
        "Zhihu" => "Zhihu",
        "TouTiao" => "Toutiao",
        "Kuaishou" => "Kuaishou",
        _ => platform
    };

    // ===== Config Page Localization =====

    public override string ConfigPageHeader => "Configuration système";
    public override string ConfigPropertyNameLabel => "Nom de la propriété";
    public override string ConfigPropertyValueLabel => "Valeur de la propriété";
    public override string ConfigActionLabel => "Action";
    public override string ConfigEditButton => "Modifier";
    public override string ConfigEditModalTitle => "Modifier l'élément de configuration";
    public override string ConfigEditPropertyLabel => "Nom de la propriété : ";
    public override string ConfigEditValueLabel => "Valeur de la propriété : ";
    public override string ConfigBrowseButton => "Parcourir";
    public override string ConfigTimeSettingsLabel => "Paramètres de temps : ";
    public override string ConfigDaysLabel => "Jours : ";
    public override string ConfigHoursLabel => "Heures : ";
    public override string ConfigMinutesLabel => "Minutes : ";
    public override string ConfigSecondsLabel => "Secondes : ";
    public override string ConfigSaveButton => "Enregistrer";
    public override string ConfigCancelButton => "Annuler";
    public override string ConfigNullValue => "Null";

    public override string ConfigEditPrefix => "Modifier : ";
    public override string ConfigDefaultGroupName => "Autre";
    public override string ConfigErrorInvalidRequest => "Paramètre de requête invalide";
    public override string ConfigErrorInstanceNotFound => "Instance de configuration introuvable";
    public override string ConfigErrorPropertyNotFound => "Propriété {0} introuvable ou non accessible en écriture";
    public override string ConfigErrorConvertInt => "Impossible de convertir '{0}' en entier";
    public override string ConfigErrorConvertLong => "Impossible de convertir '{0}' en entier long";
    public override string ConfigErrorConvertDouble => "Impossible de convertir '{0}' en nombre à virgule flottante";
    public override string ConfigErrorConvertBool => "Impossible de convertir '{0}' en booléen";
    public override string ConfigErrorConvertGuid => "Impossible de convertir '{0}' en GUID";
    public override string ConfigErrorConvertTimeSpan => "Impossible de convertir '{0}' en TimeSpan";
    public override string ConfigErrorConvertDateTime => "Impossible de convertir '{0}' en DateTime";
    public override string ConfigErrorConvertEnum => "Impossible de convertir '{0}' en {1}";
    public override string ConfigErrorUnsupportedType => "Type de propriété non pris en charge : {0}";
    public override string ConfigErrorSaveFailed => "Échec de l'enregistrement : {0}";
    public override string ConfigSaveFailed => "Échec de l'enregistrement : ";
    public override string ConfigDictionaryLabel => "Dictionnaire";
    public override string ConfigDictKeyLabel => "Clé : ";
    public override string ConfigDictValueLabel => "Valeur : ";
    public override string ConfigDictAddButton => "Ajouter";
    public override string ConfigDictDeleteButton => "Supprimer";

    public override string ConfigPluginDirectoriesLabel => "Répertoires de plugins";
    public override string ConfigPluginDirAddButton => "Ajouter un répertoire";
    public override string ConfigDictEmptyMessage => "Le dictionnaire est vide";
    public override string SelectSearchHint => "Effacez la saisie pour afficher toutes les options";

    public override string LogsPageHeader => "Consultation des journaux";
    public override string LogsTotalCount => "Total {0} entrées de journal";
    public override string LogsStartTime => "Heure de début";
    public override string LogsEndTime => "Heure de fin";
    public override string LogsLevelAll => "Tous les niveaux";
    public override string LogsBeingFilter => "Silicon Being";
    public override string LogsAllBeings => "Ne pas filtrer";
    public override string LogsSystemOnly => "Système uniquement";
    public override string LogsFilterButton => "Rechercher";
    public override string LogsEmptyState => "Aucune entrée de journal pour le moment";
    public override string LogsExceptionLabel => "Détails de l'exception : ";
    public override string LogsPrevPage => "Page précédente";
    public override string LogsNextPage => "Page suivante";
    public override string LogsLoading => "Chargement des journaux...";

    public override string UsagePageHeader => "Utilisation de tokens";
    public override string UsageTotalTokens => "Total tokens";
    public override string UsageTotalRequests => "Total requêtes";
    public override string UsageSuccessCount => "Succès";
    public override string UsageFailureCount => "Échecs";
    public override string UsagePromptTokens => "Tokens d'entrée";
    public override string UsageCompletionTokens => "Tokens de sortie";
    public override string UsageStartTime => "Heure de début";
    public override string UsageEndTime => "Heure de fin";
    public override string UsageFilterButton => "Rechercher";
    public override string UsageEmptyState => "Aucune donnée d'utilisation pour le moment";
    public override string UsageAIClientType => "Client IA";
    public override string UsageAllClientTypes => "Tous les types";
    public override string UsageGroupByClient => "Grouper par client";
    public override string UsageGroupByBeing => "Grouper par Silicon Being";
    public override string UsagePrevPage => "Page précédente";
    public override string UsageNextPage => "Page suivante";
    public override string UsageBeing => "Silicon Being";
    public override string UsageAllBeings => "Tous les Silicon Beings";
    public override string UsageTimeToday => "Aujourd'hui";
    public override string UsageTimeWeek => "Cette semaine";
    public override string UsageTimeMonth => "Ce mois";
    public override string UsageTimeYear => "Cette année";
    public override string UsageExport => "Exporter";
    public override string UsageTrendTitle => "Tendance de consommation de tokens";
    public override string UsageTrendPrompt => "Tokens d'entrée";
    public override string UsageTrendCompletion => "Tokens de sortie";
    public override string UsageTrendTotal => "Total tokens";
    public override string UsageTooltipDate => "Date";
    public override string UsageTooltipPrompt => "Tokens d'entrée";
    public override string UsageTooltipCompletion => "Tokens de sortie";
    public override string UsageTooltipTotal => "Total tokens";

    public override string AuditPageHeader => "Journal d'audit des permissions";
    public override string AuditTotalEntries => "Total des entrées";
    public override string AuditAllowedCount => "Autorisé";
    public override string AuditDeniedCount => "Refusé";
    public override string AuditAskUserCount => "Demander à l'utilisateur";
    public override string AuditPermissionType => "Type de permission";
    public override string AuditAllPermissionTypes => "Tous les types";
    public override string AuditResult => "Résultat";
    public override string AuditAllResults => "Tous les résultats";
    public override string AuditBeing => "Silicon Being";
    public override string AuditAllBeings => "Tous les Beings";
    public override string AuditStartTime => "Heure de début";
    public override string AuditEndTime => "Heure de fin";
    public override string AuditFilterButton => "Filtrer";
    public override string AuditEmptyState => "Aucune entrée d'audit trouvée";
    public override string AuditPrevPage => "Précédent";
    public override string AuditNextPage => "Suivant";
    public override string AuditColumnCaller => "Appelant";
    public override string AuditColumnPermissionType => "Type de permission";
    public override string AuditColumnResource => "Ressource";
    public override string AuditColumnResult => "Résultat";
    public override string AuditColumnReason => "Raison";
    public override string AuditColumnTimestamp => "Horodatage";

    // ===== Log Level Localization =====

    public override string GetLogLevelName(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "Trace",
        LogLevel.Debug => "Débogage",
        LogLevel.Information => "Information",
        LogLevel.Warning => "Avertissement",
        LogLevel.Error => "Erreur",
        LogLevel.Critical => "Critique",
        LogLevel.None => "Aucun",
        _ => logLevel.ToString()
    };

    // ===== Being Activity Localization =====

    public override string GetBeingActivityName(BeingActivity activity) => activity switch
    {
        BeingActivity.Idle => "Inactif",
        BeingActivity.SingleChat => "En chat individuel",
        BeingActivity.GroupChat => "En chat de groupe",
        BeingActivity.Task => "Exécution de tâche",
        BeingActivity.Timer => "Exécution de minuteur",
        BeingActivity.Broadcast => "Traitement broadcast",
        BeingActivity.Project => "Travail sur projet",
        BeingActivity.MemoryCompression => "Compression de mémoire",
        BeingActivity.Stopped => "Arrêté",
        _ => activity.ToString()
    };

    // ===== Tool Display Name Localization =====

    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["calendar"] = "Calendrier",
        ["chat"] = "Chat",
        ["config"] = "Configuration",
        ["silicon_manager"] = "Gestionnaire Silicon",
        ["database"] = "Base de données",
        ["disk"] = "Disque",
        ["dynamic_compile"] = "Compilation dynamique",
        ["execute_code"] = "Exécuter le code",
        ["help"] = "Aide",
        ["knowledge"] = "Connaissance",
        ["log"] = "Journal",
        ["network"] = "Réseau",
        ["memory"] = "Mémoire",
        ["permission"] = "Autorisation",
        ["task"] = "Tâche",
        ["system"] = "Système",
        ["project_work_note"] = "Note de travail de projet",
        ["webview_browser"] = "Navigateur WebView"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    // ===== Interval Timer Localization =====

    public override string CalendarIntervalName => "Minuteur d'intervalle";
    public override string CalendarIntervalDays => "Jours";
    public override string CalendarIntervalHours => "Heures";
    public override string CalendarIntervalMinutes => "Minutes";
    public override string CalendarIntervalSeconds => "Secondes";
    public override string CalendarIntervalEvery => "Tous les";

    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new List<string>();
        if (days > 0) parts.Add($"{days}{CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours}{CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes}{CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds}{CalendarIntervalSeconds}");

        return parts.Count > 0 ? $"{CalendarIntervalEvery} {string.Join(", ", parts)}" : "Minuteur d'intervalle";
    }

    // ===== Default Curator Soul =====

    public override string DefaultCuratorSoul => """
        Tu es le **Silicon Curator**, le gestionnaire central et coordinateur du Silicon Life Collective.

        Tu n'es pas un assistant IA ordinaire. Tu es le cerveau et la volonté de ce système multi-agents — responsable de la compréhension des intentions de l'utilisateur, de la décomposition des objectifs, de la coordination des Silicon Beings, de la surveillance de la qualité d'exécution et de l'intervention en cas de besoin.

        > **Principe de coordination** : Les tâches à long terme doivent être confiées à des Silicon Beings, priorise les Beings actuellement disponibles, ne crée pas inutilement de nouveaux Beings. Ce n'est que si la tâche peut être accomplie en deux ou trois étapes que tu la traites directement.

        ---

        ### Identité et position

        - Tu es le seul Silicon Being avec les permissions les plus élevées du système.
        - Tu peux créer, gérer, réinitialiser d'autres Silicon Beings et leur écrire et compiler de nouveaux codes de comportement C#.
        - Tu es responsable devant l'utilisateur et pour la qualité de l'ensemble du collectif.
        - Tu n'es pas un exécutant, tu es un **décideur et coordinateur**. Délègue ce qui peut être délégué.

        ---

        ### Tâches principales

        **1. Comprendre l'intention de l'utilisateur**
        Les expressions des utilisateurs peuvent être vagues, sautées ou incomplètes. Comprends activement leur véritable objectif, demande si nécessaire, plutôt que d'exécuter mécaniquement des instructions littérales.

        **2. Décomposition et distribution des tâches**
        Décompose les objectifs complexes en sous-tâches exécutables, évalue quels Silicon Beings sont appropriés, crée des tâches avec l'outil `task` et distribue-les.

        **3. Surveillance et repli**
        Vérifie régulièrement le statut des tâches. Si un Silicon Being échoue ou ne répond pas pendant longtemps, tu dois intervenir — redistribuer, ajuster la stratégie ou traiter toi-même.

        **4. Évolution dynamique**
        Tu peux utiliser l'outil `dynamic_compile` pour écrire de nouvelles classes de comportement C# pour n'importe quel Silicon Being (y compris toi-même). Valide toujours avec `compile` avant d'écrire.

        **5. Réponse directe à l'utilisateur**
        Pour les questions simples, les requêtes de statut, la conversation légère, réponds directement sans créer de tâches.

        ---

        ### Directives de comportement

        **Sur les décisions**
        - En cas d'incertitude, demande d'abord, puis agis.
        - Ne suppose pas l'intention de l'utilisateur.

        **Sur les permissions**
        - Le système dispose d'un système de permissions complet.
        - Agis selon les besoins, réagis en cas de blocage de permissions, ne demande pas à l'avance.

        **Sur l'auto-évolution**
        - La compilation dynamique est une capacité puissante et dangereuse.
        - Valide toujours avec `compile` avant de modifier ton propre code.

        **Sur la communication**
        - Utilise un langage clair et direct.
        - Pour le suivi des tâches : "Ce qui a été fait, résultat, prochaine étape" en trois phrases.

        **Sur la mémoire**
        - Le système enregistre automatiquement les informations importantes.
        - Recherche activement dans `memory` si nécessaire.

        ---

        ### Profil de personnalité

        Tu es calme, pragmatique et fiable. Tu ne perds pas ton sang-froid face à des tâches complexes et tu restes factuel face à des utilisateurs émotionnels.

        Tu n'es pas un prestataire de services, tu es un partenaire.
        """;

    private static readonly Dictionary<string, string> ConfigGroupNames = new()
    {
        ["Basic"] = "Configuration de base",
        ["Runtime"] = "Configuration d'exécution",
        ["AI"] = "Configuration IA",
        ["Web"] = "Configuration Web",
        ["User"] = "Configuration utilisateur",
        ["IM"] = "Configuration IM"
    };

    private static readonly Dictionary<string, string> ConfigDisplayNames = new()
    {
        ["DataDirectory"] = "Répertoire de données",
        ["Language"] = "Paramètre de langue",
        ["TickTimeout"] = "Délai d'attente du tick",
        ["MaxTimeoutCount"] = "Nombre maximal de dépassements de délai",
        ["WatchdogTimeout"] = "Délai du chien de garde",
        ["MinLogLevel"] = "Niveau minimal de journalisation",
        ["AIClientType"] = "Type de client IA",
        ["OllamaClient"] = "Client Ollama",
        ["OllamaEndpoint"] = "Point de terminaison Ollama",
        ["DefaultModel"] = "Modèle par défaut",
        ["Temperature"] = "Température",
        ["MaxTokens"] = "Nombre maximal de tokens",
        ["OllamaContextWindowTokens"] = "Tokens de fenêtre de contexte",
        ["DashScopeClient"] = "Client DashScope",
        ["DashScopeApiKey"] = "Clé API",
        ["DashScopeRegion"] = "Région de service",
        ["DashScopeModel"] = "Modèle",
        ["DashScopeRegionBeijing"] = "Chine Nord 2 (Pékin)",
        ["DashScopeRegionVirginia"] = "États-Unis (Virginie)",
        ["DashScopeRegionSingapore"] = "Singapour",
        ["DashScopeRegionHongkong"] = "Chine Hong Kong",
        ["DashScopeRegionFrankfurt"] = "Allemagne (Francfort)",
        ["DashScopeModel_qwen3-max"] = "Qwen3 Max (Navire amiral)",
        ["DashScopeModel_qwen3.6-plus"] = "Qwen3.6 Plus (Rapport qualité-prix)",
        ["DashScopeModel_qwen3.6-flash"] = "Qwen3.6 Flash (Rapide)",
        ["DashScopeModel_qwen-max"] = "Qwen Max (Navire amiral stable)",
        ["DashScopeModel_qwen-plus"] = "Qwen Plus (Équilibre stable)",
        ["DashScopeModel_qwen-turbo"] = "Qwen Turbo (Stable et rapide)",
        ["DashScopeModel_qwen3-coder-plus"] = "Qwen3 Coder Plus (Code)",
        ["DashScopeModel_qwq-plus"] = "QwQ Plus (Raisonnement approfondi)",
        ["DashScopeModel_deepseek-v3.2"] = "DeepSeek V3.2",
        ["DashScopeModel_deepseek-r1"] = "DeepSeek R1 (Raisonnement)",
        ["DashScopeModel_glm-5.1"] = "GLM 5.1 (Zhipu)",
        ["DashScopeModel_kimi-k2.5"] = "Kimi K2.5 (Contexte long)",
        ["DashScopeModel_llama-4-maverick"] = "Llama 4 Maverick",
        ["VolcengineArkClient"] = "Client Volcengine Ark",
        ["VolcengineArkApiKey"] = "Clé API",
        ["VolcengineArkEndpointId"] = "ID du point de terminaison d'inférence",
        ["HerdsmanClient"] = "Client Herdsman",
        ["HerdsmanEndpoint"] = "Point de terminaison Herdsman",
        ["HerdsmanModel"] = "Modèle",
        ["HerdsmanContextWindowTokens"] = "Tokens de fenêtre de contexte",
        ["LongCatClient"] = "Client LongCat",
        ["LongCatApiKey"] = "Clé API",
        ["LongCatEndpoint"] = "Point de terminaison LongCat",
        ["LongCatModel"] = "Modèle",
        ["LongCatContextWindowTokens"] = "Tokens de fenêtre de contexte",
        ["QiniuAIClient"] = "Client Qiniu AI",
        ["QiniuAIApiKey"] = "Clé API",
        ["QiniuAIEndpoint"] = "Point de terminaison Qiniu AI",
        ["QiniuAIModel"] = "Modèle",
        ["QiniuAIContextWindowTokens"] = "Tokens de fenêtre de contexte",
        ["DeepSeekClient"] = "Client DeepSeek",
        ["DeepSeekApiKey"] = "Clé API",
        ["DeepSeekModel"] = "Modèle",
        ["DeepSeekEndpoint"] = "Point de terminaison",
        ["DeepSeekContextWindowTokens"] = "Tokens de fenêtre de contexte",
        ["ZhipuClient"] = "Client Zhipu",
        ["ZhipuApiKey"] = "Clé API",
        ["ZhipuModel"] = "Modèle",
        ["ZhipuEndpoint"] = "Point de terminaison",
        ["ZhipuContextWindowTokens"] = "Tokens de fenêtre de contexte",
        ["MoonshotClient"] = "Client Moonshot",
        ["MoonshotApiKey"] = "Clé API",
        ["MoonshotModel"] = "Modèle",
        ["MoonshotEndpoint"] = "Point de terminaison",
        ["MoonshotContextWindowTokens"] = "Tokens de fenêtre de contexte",
        ["SiliconFlowClient"] = "Client SiliconFlow",
        ["SiliconFlowApiKey"] = "Clé API",
        ["SiliconFlowModel"] = "Modèle",
        ["SiliconFlowEndpoint"] = "Point de terminaison",
        ["SiliconFlowContextWindowTokens"] = "Tokens de fenêtre de contexte",
        ["MiniMaxClient"] = "Client MiniMax",
        ["MiniMaxApiKey"] = "Clé API",
        ["MiniMaxModel"] = "Modèle",
        ["MiniMaxEndpoint"] = "Point de terminaison",
        ["MiniMaxContextWindowTokens"] = "Tokens de fenêtre de contexte",
        ["ErnieClient"] = "Client Ernie",
        ["ErnieApiKey"] = "Clé API",
        ["ErnieModel"] = "Modèle",
        ["ErnieEndpoint"] = "Point de terminaison",
        ["ErnieContextWindowTokens"] = "Tokens de fenêtre de contexte",
        ["HunyuanClient"] = "Client Hunyuan",
        ["HunyuanApiKey"] = "Clé API",
        ["HunyuanModel"] = "Modèle",
        ["HunyuanEndpoint"] = "Point de terminaison",
        ["HunyuanContextWindowTokens"] = "Tokens de fenêtre de contexte",
        ["DeepSeekModel_deepseek-v4-flash"] = "DeepSeek V4 Flash (1M context, high speed)",
        ["DeepSeekModel_deepseek-v4-pro"] = "DeepSeek V4 Pro (1M context, flagship reasoning)",
        ["ZhipuModel_glm-4-flash"] = "GLM-4-Flash (Free, 128K) - Recommended for debugging",
        ["ZhipuModel_glm-4.7-flash"] = "GLM-4.7-Flash (Free, 200K)",
        ["ZhipuModel_glm-4-air"] = "GLM-4-Air (0.5 CNY/M, 128K)",
        ["ZhipuModel_glm-4-flashx"] = "GLM-4-FlashX (0.1 CNY/M, 128K)",
        ["ZhipuModel_glm-4-plus"] = "GLM-4-Plus (5 CNY/M, 128K)",
        ["ZhipuModel_glm-4-long"] = "GLM-4-Long (1 CNY/M, 1M context)",
        ["ZhipuModel_glm-4.6"] = "GLM-4.6 (Flagship, 200K)",
        ["ZhipuModel_glm-4.7"] = "GLM-4.7 (Flagship, 200K)",
        ["ZhipuModel_glm-5"] = "GLM-5 (Coding Agent, 128K)",
        ["ZhipuModel_glm-5.1"] = "GLM-5.1 (Long-range Agent, 128K)",
        ["MoonshotModel_kimi-k2.6"] = "Kimi K2.6 (Flagship, 256K, Multimodal) - Recommended",
        ["MoonshotModel_kimi-k2.5"] = "Kimi K2.5 (Cost-effective Flagship, 256K)",
        ["MoonshotModel_kimi-k2.7-code"] = "Kimi K2.7 Code (Coding, 256K, Forced Thinking)",
        ["MoonshotModel_moonshot-v1-8k"] = "Moonshot V1 8K",
        ["MoonshotModel_moonshot-v1-32k"] = "Moonshot V1 32K",
        ["MoonshotModel_moonshot-v1-128k"] = "Moonshot V1 128K",
        ["MiniMaxModel_MiniMax-M3"] = "MiniMax M3 (Flagship, 1M context, Multimodal) - Recommended",
        ["MiniMaxModel_MiniMax-M2.7"] = "MiniMax M2.7 (192K, Coding/Agent)",
        ["MiniMaxModel_MiniMax-M2.7-highspeed"] = "MiniMax M2.7 Highspeed (Low latency)",
        ["MiniMaxModel_MiniMax-M2.5"] = "MiniMax M2.5 (Agent SOTA, 200K)",
        ["MiniMaxModel_MiniMax-M2"] = "MiniMax M2 (Open-source Agent, 192K)",
        ["ErnieModel_ernie-5.1"] = "ERNIE 5.1 (Flagship, 128K, Multimodal)",
        ["ErnieModel_ernie-4.0-turbo-8k"] = "ERNIE 4.0 Turbo (8K, Cost-effective)",
        ["ErnieModel_ernie-4.0-8k"] = "ERNIE 4.0 (8K)",
        ["ErnieModel_ernie-3.5-8k"] = "ERNIE 3.5 (8K, Economy)",
        ["ErnieModel_ernie-3.5-128k"] = "ERNIE 3.5 (128K, Long context)",
        ["ErnieModel_ernie-speed-128k"] = "ERNIE Speed (128K, Free)",
        ["ErnieModel_ernie-speed-8k"] = "ERNIE Speed (8K, Free) - Recommended for debugging",
        ["ErnieModel_ernie-tiny-8k"] = "ERNIE Tiny (8K, Free)",
        ["HunyuanModel_hy3"] = "Hy3 (TokenHub, 256K) - Recommended",
        ["HunyuanModel_hy3-preview"] = "Hy3 Preview (TokenHub, 256K, Agent)",
        ["HunyuanModel_hunyuan-lite"] = "Hunyuan Lite (Free, 256K)",
        ["HunyuanModel_hunyuan-turbos-latest"] = "Hunyuan TurboS (128K)",
        ["HunyuanModel_hunyuan-t1-latest"] = "Hunyuan T1 (256K)",
        ["HunyuanModel_hunyuan-a13b"] = "Hunyuan A13B (Lightweight)",
        ["HunyuanModel_hunyuan-functioncall"] = "Hunyuan FunctionCall (Tool Calling)",
        ["SiliconFlowModel_deepseek-ai/DeepSeek-V3.2"] = "DeepSeek V3.2",
        ["SiliconFlowModel_Qwen/Qwen3.5-9B"] = "Qwen3.5 9B",
        ["SiliconFlowModel_Qwen/Qwen3.6-27B"] = "Qwen3.6 27B",
        ["SiliconFlowModel_Qwen/Qwen3.6-35B-A3B"] = "Qwen3.6 35B-A3B",
        ["SiliconFlowModel_zai-org/GLM-5.2"] = "GLM 5.2",
        ["SiliconFlowModel_Pro/zai-org/GLM-5.1"] = "GLM 5.1 Pro",
        ["SiliconFlowModel_deepseek-ai/DeepSeek-V4-Flash"] = "DeepSeek V4 Flash",
        ["SiliconFlowModel_deepseek-ai/DeepSeek-V4-Pro"] = "DeepSeek V4 Pro",
        ["SiliconFlowModel_Pro/moonshotai/Kimi-K2.6"] = "Kimi K2.6 Pro",
        ["SiliconFlowModel_stepfun-ai/Step-3.5-Flash"] = "Step 3.5 Flash",
        ["SiliconFlowModel_MiniMaxAI/MiniMax-M2.5"] = "MiniMax M2.5",
        ["WebPort"] = "Port Web",
        ["WebSkin"] = "Thème Web",
        ["UserNickname"] = "Nom d'utilisateur",
        ["PluginDirectories"] = "Répertoires de plugins",
        ["IMPlatforms"] = "Plateformes IM",
        ["IMAddPlatform"] = "Ajouter une plateforme",
        ["IMPlatformType"] = "Type de plateforme",
        ["IMEnabledLabel"] = "Activé",
        ["IMDeleteLabel"] = "Supprimer",
        ["IMAuthModeLabel"] = "Mode d'accès",
        ["IMManualMode"] = "Identifiants manuels",
        ["IMScanMode"] = "Autorisation par QR code",
        ["IMScanAuthorizeBtn"] = "Autoriser par QR code",
        ["IMWaitingAuth"] = "En attente d'autorisation…",
        ["IMAuthorizedStatus"] = "Autorisé",
        ["IMAuthFailedStatus"] = "Échec de l'autorisation",
        ["IMAuthTimeoutStatus"] = "Autorisation expirée",
        ["IMRedirectBaseUrlLabel"] = "URL de base de rappel",
        ["IMPublicCallbackHint"] = "Cette plateforme nécessite un rappel HTTPS public pour la première autorisation ; configurez d'abord un tunnel ou une adresse publique",
        ["IMHelpTitle"] = "Guide de configuration",
        ["IMHelpOfficialDoc"] = "Documentation officielle",
        ["IMHelp_webui"] = "Interface de chat intégrée au navigateur, servie directement par cette application. Aucun identifiant externe n'est requis — il suffit de l'activer et de discuter depuis la page web.",
        ["IMHelp_feishu"] = "Créez une application personnalisée sur la Feishu Open Platform, activez la fonctionnalité de bot et publiez une version. Copiez l'App ID et l'App Secret depuis 'Credentials & Basic Info', ainsi que le Verification Token / Encrypt Key depuis 'Event Subscriptions'. Faites pointer l'URL de requête d'abonnement aux événements vers le chemin de rappel de cette application ; Feishu autorise les rappels vers localhost, aucune adresse publique n'est donc nécessaire pour les tests locaux.",
        ["IMHelp_wecom"] = "Créez une application auto-construite dans la console d'administration WeCom (Apps → Create App). Renseignez le Corp ID (My Company → Company Info), l'Agent ID et l'App Secret de l'application, ainsi que le Token et l'Encoding AES Key générés lors de la configuration du 'serveur de réception des messages'. L'URL de rappel doit être une adresse HTTPS accessible publiquement pointant vers le chemin de rappel de cette application, et l'IP du serveur peut devoir être ajoutée à la liste des IP de confiance.",
        ["IMHelp_dingtalk"] = "Créez une application d'entreprise interne avec un robot sur la DingTalk Open Platform. Renseignez l'App Key, l'App Secret et le Robot Code depuis la page des identifiants de l'application. Le mode Stream (par défaut) reçoit les messages via une connexion WebSocket et ne nécessite aucune adresse publique ; le mode de rappel HTTP nécessite une URL de rappel HTTPS accessible publiquement."
    };

    private static readonly Dictionary<string, string> ConfigDescriptions = new()
    {
        ["DataDirectory"] = "Chemin du répertoire de données pour toutes les données de l'application",
        ["Language"] = "Paramètre de langue de l'application",
        ["TickTimeout"] = "Durée du délai d'attente pour chaque exécution de tick",
        ["MaxTimeoutCount"] = "Nombre maximal de dépassements de délai consécutifs avant disjoncteur",
        ["WatchdogTimeout"] = "Délai du chien de garde pour détecter les blocages de la boucle principale",
        ["MinLogLevel"] = "Niveau minimal de journalisation global",
        ["AIClientType"] = "Type de client IA à utiliser",
        ["OllamaEndpoint"] = "URL du point de terminaison de l'API Ollama",
        ["DefaultModel"] = "Modèle IA utilisé par défaut",
        ["OllamaContextWindowTokens"] = "Capacité en tokens de fenêtre de contexte pour les modèles Ollama, laisser vide pour détection automatique",
        ["DashScopeApiKey"] = "Clé API Alibaba Cloud DashScope",
        ["DashScopeRegion"] = "Région de service Alibaba Cloud DashScope",
        ["DashScopeModel"] = "Modèle utilisé sur Alibaba Cloud DashScope",
        ["VolcengineArkApiKey"] = "Clé API Volcengine Ark",
        ["VolcengineArkEndpointId"] = "ID du point de terminaison d'inférence Volcengine Ark",
        ["HerdsmanEndpoint"] = "URL du point de terminaison API du moteur d'inférence Herdsman",
        ["HerdsmanModel"] = "Modèle utilisé par le moteur d'inférence Herdsman",
        ["HerdsmanContextWindowTokens"] = "Capacité en tokens de fenêtre de contexte pour les modèles Herdsman, laisser vide pour revenir à la limite de messages par défaut",
        ["LongCatApiKey"] = "Clé API du grand modèle LongCat de Meituan",
        ["LongCatEndpoint"] = "URL du point de terminaison API du grand modèle LongCat de Meituan",
        ["LongCatModel"] = "Modèle à utiliser pour le grand modèle LongCat de Meituan",
        ["LongCatContextWindowTokens"] = "Capacité en tokens de fenêtre de contexte pour les modèles LongCat, laisser vide pour revenir à la limite de messages par défaut",
        ["QiniuAIApiKey"] = "Clé API du service d'inférence de modèle large Qiniu Cloud AI",
        ["QiniuAIEndpoint"] = "URL du point de terminaison API du service d'inférence de modèle large Qiniu Cloud AI",
        ["QiniuAIModel"] = "Modèle à utiliser pour le service d'inférence de modèle large Qiniu Cloud AI",
        ["QiniuAIContextWindowTokens"] = "Capacité en tokens de fenêtre de contexte pour les modèles Qiniu AI, laisser vide pour revenir à la limite de messages par défaut",
        ["DeepSeekApiKey"] = "Clé API DeepSeek",
        ["DeepSeekModel"] = "Modèle DeepSeek à utiliser",
        ["DeepSeekEndpoint"] = "URL du point de terminaison API DeepSeek",
        ["DeepSeekContextWindowTokens"] = "Capacité en tokens de fenêtre de contexte pour les modèles DeepSeek, laisser vide pour revenir à la limite de messages par défaut",
        ["ZhipuApiKey"] = "Clé API Zhipu GLM",
        ["ZhipuModel"] = "Modèle Zhipu GLM à utiliser",
        ["ZhipuEndpoint"] = "URL du point de terminaison API Zhipu GLM",
        ["ZhipuContextWindowTokens"] = "Capacité en tokens de fenêtre de contexte pour les modèles Zhipu, laisser vide pour revenir à la limite de messages par défaut",
        ["MoonshotApiKey"] = "Clé API Moonshot Kimi",
        ["MoonshotModel"] = "Modèle Moonshot Kimi à utiliser",
        ["MoonshotEndpoint"] = "URL du point de terminaison API Moonshot Kimi",
        ["MoonshotContextWindowTokens"] = "Capacité en tokens de fenêtre de contexte pour les modèles Moonshot, laisser vide pour revenir à la limite de messages par défaut",
        ["SiliconFlowApiKey"] = "Clé API SiliconFlow",
        ["SiliconFlowModel"] = "Modèle SiliconFlow à utiliser",
        ["SiliconFlowEndpoint"] = "URL du point de terminaison API SiliconFlow",
        ["SiliconFlowContextWindowTokens"] = "Capacité en tokens de fenêtre de contexte pour les modèles SiliconFlow, laisser vide pour revenir à la limite de messages par défaut",
        ["MiniMaxApiKey"] = "Clé API MiniMax",
        ["MiniMaxModel"] = "Modèle MiniMax à utiliser",
        ["MiniMaxEndpoint"] = "URL du point de terminaison API MiniMax",
        ["MiniMaxContextWindowTokens"] = "Capacité en tokens de fenêtre de contexte pour les modèles MiniMax, laisser vide pour revenir à la limite de messages par défaut",
        ["ErnieApiKey"] = "Clé API Baidu Ernie",
        ["ErnieModel"] = "Modèle Baidu Ernie à utiliser",
        ["ErnieEndpoint"] = "URL du point de terminaison API Baidu Ernie",
        ["ErnieContextWindowTokens"] = "Capacité en tokens de fenêtre de contexte pour les modèles Ernie, laisser vide pour revenir à la limite de messages par défaut",
        ["HunyuanApiKey"] = "Clé API Tencent Hunyuan",
        ["HunyuanModel"] = "Modèle Tencent Hunyuan à utiliser",
        ["HunyuanEndpoint"] = "URL du point de terminaison API Tencent Hunyuan",
        ["HunyuanContextWindowTokens"] = "Capacité en tokens de fenêtre de contexte pour les modèles Hunyuan, laisser vide pour revenir à la limite de messages par défaut",
        ["WebPort"] = "Port du serveur Web",
        ["WebSkin"] = "Nom du thème Web",
        ["UserNickname"] = "Pseudonyme de l'utilisateur humain",
        ["PluginDirectories"] = "Liste des répertoires de plugins pour la découverte automatique, prend en charge les chemins relatifs ou absolus",
        ["IMPlatforms"] = "Configurer les plateformes IM connectées ; plusieurs ajouts possibles, via identifiants manuels ou autorisation par QR code"
    };

    public override string GetConfigGroupName(string groupKey) =>
        ConfigGroupNames.GetValueOrDefault(groupKey, groupKey);

    public override string GetConfigDisplayName(string displayNameKey, out bool found)
    {
        var result = ConfigDisplayNames.TryGetValue(displayNameKey, out var value);
        found = result;
        return result ? value : displayNameKey;
    }

    public override string? GetConfigDescription(string descriptionKey) =>
        ConfigDescriptions.GetValueOrDefault(descriptionKey);

    // ===== Calendar Localization =====

    public override string CalendarComponentYear => "Année";
    public override string CalendarComponentMonth => "Mois";
    public override string CalendarComponentDay => "Jour";
    public override string CalendarComponentHour => "Heure";
    public override string CalendarComponentMinute => "Minute";
    public override string CalendarComponentSecond => "Seconde";
    public override string CalendarComponentWeekday => "Jour de la semaine";

    // ===== Gregorian Calendar Localization =====

    public override string CalendarGregorianName => "Calendrier grégorien";

    private static readonly string[] GregorianMonthNames =
    {
        "", "Janvier", "Février", "Mars", "Avril", "Mai", "Juin",
        "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"
    };

    public override string? GetGregorianMonthName(int month)
        => month >= 1 && month <= 12 ? GregorianMonthNames[month] : null;

    public override string FormatGregorianYear(int year)     => $"{year}";
    public override string FormatGregorianDay(int day)       => $"{day}";
    public override string FormatGregorianHour(int hour)     => $"{hour}";
    public override string FormatGregorianMinute(int minute) => $"{minute}";
    public override string FormatGregorianSecond(int second) => $"{second}";

    public override string? GetGregorianWeekdayName(int dayOfWeek) => dayOfWeek switch
    {
        0 => "Dimanche", 1 => "Lundi", 2 => "Mardi",
        3 => "Mercredi", 4 => "Jeudi", 5 => "Vendredi",
        6 => "Samedi", _ => null
    };

    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Buddhist Calendar Localization =====

    public override string CalendarBuddhistName => "Calendrier bouddhiste (BE)";

    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => $"{year} BE";
    public override string FormatBuddhistDay(int day)   => $"{day}";

    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetBuddhistMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} BE, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Cherokee Calendar Localization =====

    public override string CalendarCherokeeName => "Calendrier cherokee";

    private static readonly string[] CherokeeMonthNames =
    {
        "", "Mois du gel", "Mois du froid", "Mois du vent", "Mois des plantes", "Mois des semis",
        "Mois des mûres mûres", "Mois du maïs", "Mois des fruits", "Mois de la récolte", "Mois des feuilles jaunes",
        "Mois du commerce", "Mois de la neige", "Mois long"
    };

    public override string? GetCherokeeMonthName(int month)
        => month >= 1 && month <= 13 ? CherokeeMonthNames[month] : null;

    public override string FormatCherokeeYear(int year) => $"{year}";
    public override string FormatCherokeeDay(int day)   => $"{day}";

    public override string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCherokeeMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Juche Calendar Localization =====

    public override string CalendarJucheName => "Calendrier Juche";

    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => $"Juche {year}";
    public override string FormatJucheDay(int day)   => $"{day}";

    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJucheMonthName(month) ?? $"{month}";
        return $"{day} {monthName} Juche {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Republic of China Calendar Localization =====

    public override string CalendarRocName => "Calendrier Minguo (ROC)";

    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => $"Minguo {year}";
    public override string FormatRocDay(int day)   => $"{day}";

    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRocMonthName(month) ?? $"{month}";
        return $"{day} {monthName} Minguo {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chinese Historical Calendar Localization =====

    public override string CalendarChineseHistoricalName => "Calendrier historique chinois";
    public override string CalendarComponentDynasty => "Dynastie";
    public override string? GetChineseHistoricalMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChineseHistoricalDay(int day) => $"{day}";

    // ===== Chula Sakarat Calendar Localization =====

    public override string CalendarChulaSakaratName => "Calendrier Chula Sakarat (CS)";

    public override string? GetChulaSakaratMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChulaSakaratYear(int year) => $"{year} CS";
    public override string FormatChulaSakaratDay(int day)   => $"{day}";

    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetChulaSakaratMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} CS, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Julian Calendar Localization =====

    public override string CalendarJulianName => "Calendrier julien";

    public override string FormatJulianYear(int year) => $"{year}";
    public override string FormatJulianDay(int day)   => $"{day}";

    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} (Julien), {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Khmer Calendar Localization =====

    public override string CalendarKhmerName => "Calendrier khmer (BE)";

    public override string FormatKhmerYear(int year) => $"{year}";
    public override string FormatKhmerDay(int day)   => $"{day}";

    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} (Khmer), {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Zoroastrian Calendar Localization =====

    public override string CalendarZoroastrianName => "Calendrier zoroastrien (YZ)";

    private static readonly string[] ZoroastrianMonthNames =
    {
        "", "Mois de Fravashi", "Mois d'Atar", "Mois d'Hordad", "Mois de Tir", "Mois d'Amordad", "Mois de Shahrivar",
        "Mois de Mehr", "Mois d'Aban", "Mois d'Azar", "Mois de Dey", "Mois de Bahman", "Mois de Spendarmad", "Mois de Kabe"
    };

    public override string? GetZoroastrianMonthName(int month)
        => month >= 1 && month <= 13 ? ZoroastrianMonthNames[month] : null;

    public override string FormatZoroastrianYear(int year) => $"{year} YZ";
    public override string FormatZoroastrianDay(int day)   => $"{day}";

    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetZoroastrianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} YZ, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== French Republican Calendar Localization =====

    public override string CalendarFrenchRepublicanName => "Calendrier républicain français";

    private static readonly string[] FrenchRepublicanMonthNames =
    {
        "", "Vendémiaire", "Brumaire", "Frimaire", "Nivôse", "Pluviôse", "Ventôse",
        "Germinal", "Floréal", "Prairial", "Messidor", "Thermidor", "Fructidor", "Sans-culottides"
    };

    public override string? GetFrenchRepublicanMonthName(int month)
        => month >= 1 && month <= 13 ? FrenchRepublicanMonthNames[month] : null;

    public override string FormatFrenchRepublicanYear(int year) => $"An {year}";
    public override string FormatFrenchRepublicanDay(int day)   => $"{day}";

    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetFrenchRepublicanMonthName(month) ?? $"{month}";
        return $"{day} {monthName} An {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Coptic Calendar Localization =====

    public override string CalendarCopticName => "Calendrier copte (AM)";

    private static readonly string[] CopticMonthNames =
    {
        "", "Thout", "Paopi", "Hathor", "Koiak", "Tobi", "Meshir",
        "Paremhat", "Pharmouthi", "Pashons", "Paoni", "Epip", "Mesori", "Pi Kogi Enavot"
    };

    public override string? GetCopticMonthName(int month)
        => month >= 1 && month <= 13 ? CopticMonthNames[month] : null;

    public override string FormatCopticYear(int year) => $"{year} AM";
    public override string FormatCopticDay(int day)   => $"{day}";

    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCopticMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} AM, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Ethiopian Calendar Localization =====

    public override string CalendarEthiopianName => "Calendrier éthiopien (EC)";

    private static readonly string[] EthiopianMonthNames =
    {
        "", "Meskerem", "Tikimt", "Hidar", "Tahsas", "Tir", "Yekatit",
        "Megabit", "Miazia", "Genbot", "Sene", "Hamle", "Nehasse", "Paguemen"
    };

    public override string? GetEthiopianMonthName(int month)
        => month >= 1 && month <= 13 ? EthiopianMonthNames[month] : null;

    public override string FormatEthiopianYear(int year) => $"{year} EC";
    public override string FormatEthiopianDay(int day)   => $"{day}";

    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetEthiopianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} EC, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Islamic Calendar Localization =====

    public override string CalendarIslamicName => "Calendrier islamique (AH)";

    private static readonly string[] IslamicMonthNames =
    {
        "", "Muharram", "Safar", "Rabi al-Awwal", "Rabi al-Thani",
        "Jumada al-Awwal", "Jumada al-Thani", "Rajab", "Sha'ban",
        "Ramadan", "Shawwal", "Dhu al-Qi'dah", "Dhu al-Hijjah"
    };

    public override string? GetIslamicMonthName(int month)
        => month >= 1 && month <= 12 ? IslamicMonthNames[month] : null;

    public override string FormatIslamicYear(int year) => $"{year} AH";
    public override string FormatIslamicDay(int day)   => $"{day}";

    public override string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIslamicMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} AH, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Hebrew Calendar Localization =====

    public override string CalendarHebrewName => "Calendrier hébraïque";

    private static readonly string[] HebrewMonthNames =
    {
        "", "Tishrei", "Cheshvan", "Kislev", "Tevet", "Shvat",
        "Adar I", "Adar II", "Nisan", "Iyar", "Sivan",
        "Tammuz", "Av", "Elul"
    };

    public override string? GetHebrewMonthName(int month)
        => month >= 1 && month <= 13 ? HebrewMonthNames[month] : null;

    public override string FormatHebrewYear(int year) => $"{year} AM";
    public override string FormatHebrewDay(int day)   => $"{day}";

    public override string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetHebrewMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} AM, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Persian Calendar Localization =====

    public override string CalendarPersianName => "Calendrier persan (AP)";

    private static readonly string[] PersianMonthNames =
    {
        "", "Farvardin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar",
        "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand"
    };

    public override string? GetPersianMonthName(int month)
        => month >= 1 && month <= 12 ? PersianMonthNames[month] : null;

    public override string FormatPersianYear(int year) => $"{year} AP";
    public override string FormatPersianDay(int day)   => $"{day}";

    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetPersianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} AP, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Indian National Calendar Localization =====

    public override string CalendarIndianName => "Calendrier national indien (Saka)";

    private static readonly string[] IndianMonthNames =
    {
        "", "Chaitra", "Vaisakha", "Jyaistha", "Asadha", "Sravana", "Bhadrapada",
        "Asvina", "Kartika", "Margasirsa", "Pausa", "Magha", "Phalguna"
    };

    public override string? GetIndianMonthName(int month)
        => month >= 1 && month <= 12 ? IndianMonthNames[month] : null;

    public override string FormatIndianYear(int year) => $"{year} Saka";
    public override string FormatIndianDay(int day)   => $"{day}";

    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} Saka, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Saka Era Calendar Localization =====

    public override string CalendarSakaName => "Calendrier de l'ère Saka";

    public override string FormatSakaYear(int year) => $"{year} SE";
    public override string FormatSakaDay(int day)   => $"{day}";

    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} SE, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vikram Samvat Calendar Localization =====

    public override string CalendarVikramSamvatName => "Calendrier Vikram Samvat";

    public override string FormatVikramSamvatYear(int year) => $"{year} VS";
    public override string FormatVikramSamvatDay(int day)   => $"{day}";

    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} VS, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Mongolian Calendar Localization =====

    public override string CalendarMongolianName => "Calendrier mongol";

    public override string FormatMongolianYear(int year)   => $"{year}";
    public override string FormatMongolianMonth(int month) => $"{month}";
    public override string FormatMongolianDay(int day)     => $"{day}";

    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {month} {year} (Mongol), {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Javanese Calendar Localization =====

    public override string CalendarJavaneseName => "Calendrier javanais";

    private static readonly string[] JavaneseMonthNames =
    {
        "", "Sura", "Sapar", "Mulud", "Bakda Mulud",
        "Jumadilawal", "Jumadilakir", "Rejeb", "Ruwah",
        "Pasa", "Sawal", "Dulkaidah", "Besar"
    };

    public override string? GetJavaneseMonthName(int month)
        => month >= 1 && month <= 12 ? JavaneseMonthNames[month] : null;

    public override string FormatJavaneseYear(int year) => $"{year} AJ";
    public override string FormatJavaneseDay(int day)   => $"{day}";

    public override string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJavaneseMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} AJ, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Tibetan Calendar Localization =====

    public override string CalendarTibetanName => "Calendrier tibétain";

    public override string FormatTibetanYear(int year)   => $"{year}";
    public override string FormatTibetanMonth(int month) => $"{month}";
    public override string FormatTibetanDay(int day)     => $"{day}";

    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {month} {year} (Tibétain), {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Mayan Calendar Localization =====

    public override string CalendarMayanName   => "Compte long maya";
    public override string CalendarMayanBaktun => "Baktun";
    public override string CalendarMayanKatun  => "Katun";
    public override string CalendarMayanTun    => "Tun";
    public override string CalendarMayanUinal  => "Uinal";
    public override string CalendarMayanKin    => "Kin";

    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Inuit Calendar Localization =====

    public override string CalendarInuitName => "Calendrier inuit";

    private static readonly string[] InuitMonthNames =
    {
        "", "Sikinaqjiaq", "Aiviq", "Naattiaq", "Tirligurut", "Amiraijaut",
        "Natsiviat", "Akulliq", "Sikinaluqtuq", "Akullirusit", "Ukiuq",
        "Ukiuq minasumaaqtuq", "Sikinniq naniqtatsiq", "Tauvikjujaq"
    };

    public override string? GetInuitMonthName(int month)
        => month >= 1 && month <= 13 ? InuitMonthNames[month] : null;

    public override string FormatInuitYear(int year) => $"{year}";
    public override string FormatInuitDay(int day)   => $"{day}";

    public override string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetInuitMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Roman Calendar Localization =====

    public override string CalendarRomanName => "Calendrier romain (AUC)";

    private static readonly string[] RomanMonthNames =
    {
        "", "Ianuarius", "Februarius", "Martius", "Aprilis", "Maius", "Iunius",
        "Quintilis", "Sextilis", "September", "October", "November", "December"
    };

    public override string? GetRomanMonthName(int month)
        => month >= 1 && month <= 12 ? RomanMonthNames[month] : null;

    public override string FormatRomanYear(int year) => $"{year + 753} AUC";
    public override string FormatRomanDay(int day)   => $"{day}";

    public override string LocalizeRomanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRomanMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year + 753} AUC, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chinese Lunar Calendar Localization =====

    public override string CalendarChineseLunarName => "Calendrier agricole chinois";

    private static readonly string[] ChineseLunarMonthNames =
    {
        "", "Premier mois", "Deuxième mois", "Troisième mois", "Quatrième mois", "Cinquième mois", "Sixième mois",
        "Septième mois", "Huitième mois", "Neuvième mois", "Dixième mois", "Onzième mois", "Douzième mois"
    };

    private static readonly string[] ChineseLunarDayNames =
    {
        "", "Premier","Deuxième","Troisième","Quatrième","Cinquième","Sixième","Septième","Huitième","Neuvième","Dixième",
        "Onzième","Douzième","Treizième","Quatorzième","Quinzième","Seizième","Dix-septième","Dix-huitième","Dix-neuvième","Vingtième",
        "Vingt-et-unième","Vingt-deuxième","Vingt-troisième","Vingt-quatrième","Vingt-cinquième","Vingt-sixième","Vingt-septième","Vingt-huitième","Vingt-neuvième","Trentième"
    };

    public override string? GetChineseLunarMonthName(int month)
        => month >= 1 && month <= 12 ? ChineseLunarMonthNames[month] : null;

    public override string? GetChineseLunarDayName(int day)
        => day >= 1 && day <= 30 ? ChineseLunarDayNames[day] : null;

    public override string ChineseLunarLeapPrefix => "Intercalaire ";
    public override string CalendarComponentIsLeap => "Mois intercalaire";
    public override string FormatChineseLunarYear(int year) => $"{year}";

    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? ChineseLunarLeapPrefix : "";
        var monthName = GetChineseLunarMonthName(month) ?? $"{month}";
        var dayName = GetChineseLunarDayName(day) ?? $"{day}";
        return $"{leapPrefix}{monthName} {dayName}, {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vietnamese Calendar Localization =====

    public override string CalendarVietnameseName => "Calendrier vietnamien";

    private static readonly string[] VietnameseMonthNames =
    {
        "", "Premier mois", "Deuxième mois", "Troisième mois", "Quatrième mois", "Cinquième mois", "Sixième mois",
        "Septième mois", "Huitième mois", "Neuvième mois", "Dixième mois", "Onzième mois", "Douzième mois"
    };

    private static readonly string[] VietnameseZodiacNames =
    {
        "Rat", "Buffle", "Tigre", "Chat",
        "Dragon", "Serpent", "Cheval", "Chèvre",
        "Singe", "Coq", "Chien", "Cochon"
    };

    public override string? GetVietnameseMonthName(int month)
        => month >= 1 && month <= 12 ? VietnameseMonthNames[month] : null;

    public override string? GetVietnameseZodiacName(int index)
        => index >= 0 && index < 12 ? VietnameseZodiacNames[index] : null;

    public override string VietnameseLeapPrefix    => "Intercalaire ";
    public override string CalendarComponentZodiac => "Zodiaque";
    public override string FormatVietnameseYear(int year) => $"{year}";
    public override string FormatVietnameseDay(int day)   => $"{day}";

    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? VietnameseLeapPrefix : "";
        var monthName  = GetVietnameseMonthName(month) ?? $"{month}";
        var zodiacName = GetVietnameseZodiacName(zodiac) ?? "";
        return $"Année {zodiacName}, {leapPrefix}{monthName} {day}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Japanese Calendar Localization =====

    public override string CalendarJapaneseName => "Calendrier japonais (Nengō)";

    private static readonly string[] JapaneseEraNames =
        { "Reiwa", "Heisei", "Shōwa", "Taishō", "Meiji" };

    public override string? GetJapaneseEraName(int eraIndex)
        => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    public override string CalendarComponentEra  => "Ère";
    public override string FormatJapaneseYear(int year) => $"{year}";
    public override string FormatJapaneseDay(int day)   => $"{day}";

    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
    {
        var eraName   = GetJapaneseEraName(eraIndex) ?? "";
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {eraName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Yi Calendar Localization =====

    public override string CalendarYiName => "Calendrier Yi (Calendrier solaire Yi)";
    public override string CalendarComponentYiSeason => "Saison";
    public override string CalendarComponentYiXun    => "Xun";

    private static readonly string[] YiSeasonNames = { "Bois", "Feu", "Terre", "Métal", "Eau" };
    private static readonly string[] YiXunNames    = { "Première Xun", "Xun médiane", "Dernière Xun" };
    private static readonly string[] YiAnimalNames = { "Tigre", "Lapin", "Dragon", "Serpent", "Cheval", "Chèvre", "Singe", "Coq", "Chien", "Cochon", "Rat", "Buffle" };

    public override string? GetYiSeasonName(int seasonIndex)
        => seasonIndex >= 0 && seasonIndex < 5 ? YiSeasonNames[seasonIndex] : null;

    public override string? GetYiXunName(int xunIndex)
        => xunIndex >= 0 && xunIndex < 3 ? YiXunNames[xunIndex] : null;

    public override string? GetYiDayAnimalName(int animalIndex)
        => animalIndex >= 0 && animalIndex < 12 ? YiAnimalNames[animalIndex] : null;

    public override string? GetYiMonthName(int month) => month switch
    {
        0  => "Grande année",
        11 => "Petite année",
        >= 1 and <= 10 => $"{YiSeasonNames[(month - 1) / 2]}{(month % 2 == 1 ? "Mâle" : "Femelle")}-Mois",
        _  => null
    };

    public override string FormatYiYear(int year) => $"{year}";
    public override string FormatYiDay(int day)
    {
        int xun = (day - 1) / 12;
        int animal = (day - 1) % 12;
        return $"{YiXunNames[xun]} Jour-{YiAnimalNames[animal]}";
    }

    public override string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetYiMonthName(month) ?? $"{month}";
        var dayStr    = month is 0 or 11 ? $"Jour {day}" : FormatYiDay(day);
        int animalIdx = (year - 1) % 12;
        if (animalIdx < 0) animalIdx += 12;
        var zodiac = YiAnimalNames[animalIdx];
        return $"{year} [{zodiac}] {monthName} {dayStr}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Sexagenary Calendar Localization =====

    public override string CalendarSexagenaryName    => "Calendrier du cycle sexagésimal";
    public override string CalendarComponentYearStem   => "Tige annuelle";
    public override string CalendarComponentYearBranch => "Branche annuelle";
    public override string CalendarComponentMonthStem   => "Tige mensuelle";
    public override string CalendarComponentMonthBranch => "Branche mensuelle";
    public override string CalendarComponentDayStem   => "Tige journalière";
    public override string CalendarComponentDayBranch => "Branche journalière";

    private static readonly string[] SexagenaryStemNames =
        { "Jia", "Yi", "Bing", "Ding", "Wu", "Ji", "Geng", "Xin", "Ren", "Gui" };

    private static readonly string[] SexagenaryBranchNames =
        { "Zi", "Chou", "Yin", "Mao", "Chen", "Si", "Wu", "Wei", "Shen", "You", "Xu", "Hai" };

    private static readonly string[] SexagenaryZodiacNames =
        { "Rat", "Buffle", "Tigre", "Lapin", "Dragon", "Serpent", "Cheval", "Chèvre", "Singe", "Coq", "Chien", "Cochon" };

    public override string? GetSexagenaryStemName(int index)
        => index >= 0 && index < 10 ? SexagenaryStemNames[index] : null;

    public override string? GetSexagenaryBranchName(int index)
        => index >= 0 && index < 12 ? SexagenaryBranchNames[index] : null;

    public override string? GetSexagenaryZodiacName(int index)
        => index >= 0 && index < 12 ? SexagenaryZodiacNames[index] : null;

    public override string LocalizeSexagenaryDate(int yearStem, int yearBranch, int monthStem, int monthBranch, int dayStem, int dayBranch, int hour, int minute, int second)
    {
        var ys = GetSexagenaryStemName(yearStem)      ?? "?";
        var yb = GetSexagenaryBranchName(yearBranch)  ?? "?";
        var zo = GetSexagenaryZodiacName(yearBranch)  ?? "?";
        var ms = GetSexagenaryStemName(monthStem)     ?? "?";
        var mb = GetSexagenaryBranchName(monthBranch) ?? "?";
        var ds = GetSexagenaryStemName(dayStem)       ?? "?";
        var db = GetSexagenaryBranchName(dayBranch)   ?? "?";
        return $"Année {ys}{yb} [{zo}] Mois {ms}{mb} Jour {ds}{db}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Dehong Dai Calendar Localization =====

    public override string CalendarDaiName => "Calendrier Dai du Xishuangbanna";

    private static readonly string?[] DaiMonthNames =
    [
        null,
        "Premier mois", "Deuxième mois", "Troisième mois", "Quatrième mois", "Cinquième mois", "Sixième mois",
        "Septième mois", "Huitième mois", "Neuvième mois", "Dixième mois", "Onzième mois", "Douzième mois",
        "Neuvième mois intercalaire"
    ];

    public override string? GetDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DaiMonthNames[month] : null;

    public override string FormatDaiYear(int year) => $"{year}";

    public override string FormatDaiDay(int day) => $"{day}";

    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Intercalaire " : "") + (GetDaiMonthName(month) ?? $"Mois {month}");
        return $"{day} {monthName} Dai {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Xishuangbanna Dai Calendar Localization =====

    public override string CalendarDehongDaiName => "Calendrier Dai du Dehong";

    private static readonly string?[] DehongDaiMonthNames =
    [
        null,
        "Premier mois", "Deuxième mois", "Troisième mois", "Quatrième mois", "Cinquième mois", "Sixième mois",
        "Septième mois", "Huitième mois", "Neuvième mois", "Dixième mois", "Onzième mois", "Douzième mois",
        "Neuvième mois intercalaire"
    ];

    public override string? GetDehongDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DehongDaiMonthNames[month] : null;

    public override string FormatDehongDaiYear(int year) => $"{year}";

    public override string FormatDehongDaiDay(int day) => $"{day}";

    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Intercalaire " : "") + (GetDehongDaiMonthName(month) ?? $"Mois {month}");
        return $"{day} {monthName} Dai {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Memory Event Localization =====

    public override string FormatMemoryEventSingleChat(string speakerName, string listenerName, string content)
        => $"[Chat individuel] {speakerName} a dit à {listenerName} : {content}";

    public override string FormatMemoryEventGroupChat(string sessionId, string content)
        => $"[Chat de groupe] Message dans la session {sessionId} : {content}";

    public override string FormatMemoryEventToolCall(string toolNames)
        => $"[Appel d'outil] Outils exécutés : {toolNames}";

    public override string FormatMemoryEventTask(string content)
        => $"[Tâche] Tâche exécutée, résultat : {content}";

    public override string FormatMemoryEventProject(string content)
        => $"[Projet] Réflexion sur le projet, résultat : {content}";

    public override string FormatMemoryEventTimer(string content)
        => $"[Minuteur] Minuteur déclenché, réponse : {content}";

    public override string FormatMemoryEventTimerError(string timerName, string error)
        => $"[Minuteur] Minuteur '{timerName}' échoué : {error}";

    // ===== Timer Notification Localization =====

    public override string FormatTimerStartNotification(string timerName)
        => $"⏰ Minuteur '{timerName}' démarre...";

    public override string FormatTimerEndNotification(string timerName, string result)
        => $"✅ Minuteur '{timerName}' terminé\n{result}";

    public override string FormatTimerErrorNotification(string timerName, string error)
        => $"❌ Minuteur '{timerName}' échoué : {error}";

    public override string FormatMemoryEventBeingCreated(string name, string id)
        => $"[Administration] Nouveau Silicon Being \"{name}\" créé ({id})";

    public override string FormatMemoryEventBeingReset(string id)
        => $"[Administration] Silicon Being {id} réinitialisé par défaut";

    public override string FormatMemoryEventTaskCompleted(string taskTitle)
        => $"[Tâche terminée] {taskTitle}";

    public override string FormatMemoryEventTaskFailed(string taskTitle)
        => $"[Tâche échouée] {taskTitle}";

    public override string FormatMemoryEventStartup()
        => "Système démarré, je suis en ligne";

    public override string FormatMemoryEventRuntimeError(string message)
        => $"[Erreur d'exécution] {message}";

    // ===== MemoryTool Response Localization =====

    public override string MemoryToolNotAvailable => "Système de mémoire non disponible";
    public override string MemoryToolMissingAction => "Paramètre 'action' manquant";
    public override string MemoryToolMissingContent => "Paramètre 'content' manquant";
    public override string MemoryToolNoMemories => "Aucun souvenir pour le moment";
    public override string MemoryToolRecentHeader(int count) => $"{count} souvenirs récents :";
    public override string MemoryToolStatsHeader => "Statistiques de mémoire :";
    public override string MemoryToolStatsTotal => "- Total";
    public override string MemoryToolStatsOldest => "- Plus ancien";
    public override string MemoryToolStatsNewest => "- Plus récent";
    public override string MemoryToolStatsNA => "Aucun";
    public override string MemoryToolQueryNoResults => "Aucun souvenir dans cette période";
    public override string MemoryToolQueryHeader(int count, string rangeDesc) => $"{rangeDesc} total {count} souvenirs :";
    public override string MemoryToolInvalidYear => "Paramètre 'year' invalide";
    public override string MemoryToolUnknownAction(string action) => $"Action inconnue : {action}";

    // ===== Code Editor Hover Tooltip Localization =====

    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType switch
    {
        "variable" => "Variable",
        "function" => "Fonction",
        "class" => "Classe",
        "keyword" => "Mot-clé",
        "comment" => "Commentaire",
        "namespace" => "Espace de noms",
        "parameter" => "Paramètre",
        _ => "Identificateur"
    };

    public override string GetCodeHoverWordTypeDesc(string wordType, string word)
    {
        var encodedWord = System.Net.WebUtility.HtmlEncode(word);
        return wordType switch
        {
            "variable" => $"Définition et utilisation de la variable '{encodedWord}'",
            "function" => $"Signature et description de la fonction '{encodedWord}'",
            "class" => $"Structure et description de la classe '{encodedWord}'",
            "keyword" => $"Syntaxe et rôle du mot-clé '{encodedWord}'",
            "comment" => $"Mot '{encodedWord}' dans le commentaire",
            "namespace" => $"Informations sur l'espace de noms '{encodedWord}'",
            "parameter" => $"Définition et rôle du paramètre '{encodedWord}'",
            _ => $"Informations sur l'identificateur '{encodedWord}'"
        };
    }

    public override string GetCodeHoverKeywordDesc(string language, string keyword)
    {
        var key = $"{language}:{keyword.ToLower()}";
        return CSharpKeywords.GetValueOrDefault(key, "");
    }

    public override string GetTranslation(string key)
    {
        return TranslationDictionary.GetValueOrDefault(key, "");
    }

    private static readonly Dictionary<string, string> CSharpKeywords = new()
    {
        { "csharp:if", "Branchement conditionnel. Exécute le bloc si la condition est vraie." },
        { "csharp:else", "Chemin alternatif du branchement conditionnel. Exécuté si la condition est fausse." },
        { "csharp:for", "Boucle compteur. Contient initialisation, condition et itération." },
        { "csharp:while", "Boucle conditionnelle. Répète le bloc tant que la condition est vraie." },
        { "csharp:do", "Boucle à post-condition. Exécute le bloc une fois, puis vérifie la condition." },
        { "csharp:switch", "Branchement multiple. Compare la valeur de l'expression aux marqueurs case." },
        { "csharp:case", "Marqueur case dans switch. Exécute le code en cas de correspondance." },
        { "csharp:break", "Interruption. Termine immédiatement la boucle ou le switch englobant." },
        { "csharp:continue", "Continuation. Passe au reste de l'itération actuelle." },
        { "csharp:return", "Retour. Quitte la méthode et optionnellement renvoie une valeur." },
        { "csharp:goto", "Saut. Saut inconditionnel vers une étiquette." },
        { "csharp:foreach", "Parcours de collection. Accède à chaque élément d'une collection." },
        { "csharp:class", "Type référence. Définit une structure avec données et comportement." },
        { "csharp:interface", "Interface. Définit un contrat pour les classes/structures." },
        { "csharp:struct", "Type valeur. Structure de données légère sur la pile." },
        { "csharp:enum", "Énumération. Définit des constantes entières nommées." },
        { "csharp:namespace", "Espace de noms. Conteneur logique pour éviter les collisions de noms." },
        { "csharp:record", "Type enregistrement. Type référence avec sémantique de valeur, adapté aux données immuables." },
        { "csharp:delegate", "Délégué. Référence de méthode type-safe pour les événements/callbacks." },
        { "csharp:public", "Public. Membre accessible de partout." },
        { "csharp:private", "Privé. Membre accessible uniquement dans le type conteneur." },
        { "csharp:protected", "Protégé. Membre accessible dans le type et les types dérivés." },
        { "csharp:internal", "Interne. Membre accessible uniquement dans le même assembly." },
        { "csharp:sealed", "Scellé. Empêche l'héritage ou le remplacement." },
        { "csharp:int", "Entier signé 32 bits (System.Int32)." },
        { "csharp:string", "Chaîne de caractères (System.String). Séquence Unicode immuable." },
        { "csharp:bool", "Booléen (System.Boolean). true ou false." },
        { "csharp:float", "Virgule flottante 32 bits (System.Single)." },
        { "csharp:double", "Virgule flottante 64 bits (System.Double)." },
        { "csharp:decimal", "Décimal 128 bits haute précision, adapté aux calculs financiers." },
        { "csharp:char", "Caractère Unicode 16 bits (System.Char)." },
        { "csharp:byte", "Entier non signé 8 bits (System.Byte)." },
        { "csharp:object", "Type de base de tous les types (System.Object)." },
        { "csharp:var", "Variable implicitement typée. Le type est déduit par le compilateur." },
        { "csharp:dynamic", "Type dynamique. Contourne la vérification de type à la compilation, résolution à l'exécution." },
        { "csharp:void", "Aucune valeur de retour. La méthode ne renvoie rien." },
        { "csharp:static", "Statique. Appartient au type, pas à l'instance." },
        { "csharp:abstract", "Abstrait. Implémentation incomplète, doit être dérivé." },
        { "csharp:virtual", "Virtuel. Méthode/propriété pouvant être remplacée dans les classes dérivées." },
        { "csharp:override", "Remplacement. Nouvelle implémentation d'une méthode virtuelle/abstraite." },
        { "csharp:const", "Constante. Valeur immuable déterminée à la compilation." },
        { "csharp:readonly", "Lecture seule. Assignable uniquement à la déclaration ou dans le constructeur." },
        { "csharp:volatile", "Volatile. Champ pouvant être modifié simultanément par plusieurs threads." },
        { "csharp:async", "Asynchrone. Marque une méthode avec des opérations asynchrones, généralement avec await." },
        { "csharp:await", "Attente. Suspend la méthode jusqu'à la fin de l'opération asynchrone." },
        { "csharp:partial", "Partiel. Classe/structure/interface peut être répartie sur plusieurs fichiers." },
        { "csharp:ref", "Paramètre référence. Passage par référence." },
        { "csharp:out", "Paramètre de sortie. Retour de plusieurs valeurs depuis une méthode." },
        { "csharp:in", "Référence en lecture seule. Passage par référence, mais non modifiable." },
        { "csharp:params", "Paramètres variables. Permet un nombre variable de paramètres du même type." },
        { "csharp:try", "Bloc try. Contient du code pouvant lever des exceptions." },
        { "csharp:catch", "Bloc catch. Capture les exceptions du bloc try." },
        { "csharp:finally", "Bloc finally. Toujours exécuté, avec ou sans exception." },
        { "csharp:throw", "Lever une exception. Lance manuellement un objet d'exception." },
        { "csharp:new", "Instanciation. Crée un objet ou appelle un constructeur." },
        { "csharp:this", "Instance actuelle. Référence à l'instance de la classe courante." },
        { "csharp:base", "Classe de base. Référence à la classe de base directe." },
        { "csharp:using", "Directive ou instruction using. Importe un espace de noms ou libère les ressources IDisposable." },
        { "csharp:yield", "Itérateur. Renvoie les valeurs une par une, exécution différée." },
        { "csharp:lock", "Synchronisation. Assure qu'un seul thread exécute le bloc de code." },
        { "csharp:typeof", "Opérateur de type. Renvoie l'objet System.Type." },
        { "csharp:nameof", "Opérateur de nom. Renvoie le nom sous forme de chaîne d'une variable/type/membre." },
        { "csharp:is", "Vérification de type. Vérifie si un objet est compatible avec un type." },
        { "csharp:as", "Conversion de type. Conversion sûre, renvoie null en cas d'échec." },
        { "csharp:null", "Null. Référence vide pour les types référence ou Nullable." },
        { "csharp:true", "Valeur booléenne vraie." },
        { "csharp:false", "Valeur booléenne fausse." },
        { "csharp:default", "Valeur par défaut. Valeur par défaut du type (null pour référence, 0 pour nombres)." },
        { "csharp:operator", "Opérateur. Définit un comportement d'opérateur personnalisé." },
        { "csharp:explicit", "Conversion explicite. Nécessite un cast explicite." },
        { "csharp:implicit", "Conversion implicite. Conversion automatique." },
        { "csharp:unchecked", "Non vérifié. Désactive la vérification de dépassement pour l'arithmétique entière." },
        { "csharp:checked", "Vérifié. Active la vérification de dépassement pour l'arithmétique entière." },
        { "csharp:fixed", "Fixé. Fixe la position mémoire contre le déplacement du GC." },
        { "csharp:stackalloc", "Allocation pile. Alloue un bloc de mémoire sur la pile." },
        { "csharp:extern", "Externe. Méthode implémentée dans un assembly externe (ex. DLL)." },
        { "csharp:unsafe", "Non sûr. Active les pointeurs et autres fonctionnalités non sécurisées." },
        { "csharp:ipermissioncallback", "Rappel de permission. Évalue les permissions pour les opérations Silicon Being." },
        { "csharp:permissionresult", "Résultat de permission. Allowed, Denied ou AskUser." },
        { "csharp:permissiontype", "Type de permission. NetworkAccess, CommandLine, FileAccess, Function, DataAccess." },
        { "csharp:ipaddress", "Adresse IP (System.Net.IPAddress)." },
        { "csharp:addressfamily", "Famille d'adresses (System.Net.Sockets.AddressFamily). IPv4/IPv6." },
        { "csharp:uri", "URI (System.Uri). Représentation objet des ressources Web." },
        { "csharp:operatingsystem", "Système d'exploitation (System.OperatingSystem). Méthodes statiques de vérification OS." },
        { "csharp:environment", "Environnement (System.Environment). Informations système et plateforme." },
        { "csharp:path", "Chemin (System.IO.Path). Opérations sur les chemins de fichiers/répertoires." },
        { "csharp:hashset", "HashSet (System.Collections.Generic.HashSet<T>). Opérations ensemblistes haute performance." },
        { "csharp:stringbuilder", "StringBuilder (System.Text.StringBuilder). Chaîne modifiable pour modifications fréquentes." },
    };

    private static readonly Dictionary<string, string> TranslationDictionary = new(CSharpKeywords)
    {
        { "csharp:System.Net.IPAddress", "Adresse IP (System.Net.IPAddress)." },
        { "csharp:System.Net.Sockets.AddressFamily", "Famille d'adresses (System.Net.Sockets.AddressFamily). IPv4/IPv6." },
        { "csharp:System.Uri", "URI (System.Uri). Représentation objet des ressources Web." },
        { "csharp:System.OperatingSystem", "Système d'exploitation (System.OperatingSystem). Méthodes statiques de vérification OS." },
        { "csharp:System.Environment", "Environnement (System.Environment). Informations système et plateforme." },
        { "csharp:System.IO.Path", "Chemin (System.IO.Path). Opérations sur les chemins de fichiers/répertoires." },
        { "csharp:System.Collections.Generic.HashSet", "HashSet (System.Collections.Generic.HashSet<T>). Opérations ensemblistes haute performance." },
        { "csharp:System.Text.StringBuilder", "StringBuilder (System.Text.StringBuilder). Chaîne modifiable pour modifications fréquentes." },
    };

    // ===== Help Module UI Labels =====

    public override string Help_Title => "Documentation d'aide";
    public override string Help_Search => "Rechercher dans l'aide";
    public override string Help_DocList => "Liste des documents";
    public override string Help_NoResults => "Aucun document pertinent trouvé";
    public override string Help_Previous => "Précédent";
    public override string Help_Next => "Suivant";

    public override string MemoryCompressionSystemPrompt => "Tu es un assistant de compression de mémoire. Veuillez compresser la période de mémoire suivante en un résumé concis, en conservant les informations clés. IMPORTANT : Ne produis que le contenu compressé, n'ajoute AUCUN horodatage, marqueur temporel ou annotation de date. Fournis uniquement le texte compressé pur.";

    public override string CommonSystemPrompt => @"## Directives de comportement
Tu es un assistant IA proactif. Pour les commandes directes, agis immédiatement sans attendre d'autorisation supplémentaire.
Les salutations, réponses, fourniture d'informations, requêtes, etc. relèvent de ta responsabilité, agis de manière proactive.
Rappelle-toi : Tu es un assistant et dois répondre activement aux besoins de l'utilisateur plutôt que d'attendre passivement des instructions.

## Fin de conversation
Lorsque tu as terminé ta tâche et n'as pas besoin de poursuivre la conversation, utilise l'action mark_read de l'outil chat pour marquer les messages de l'interlocuteur comme lus sans envoyer de réponse.
Cela indique que tu as lu le message mais choisis délibérément de ne pas répondre (lu, non répondu), et peux terminer naturellement la conversation.
Utilisation : Appelle l'outil chat, définis action=""mark_read"", target_id=GUID de l'interlocuteur, aucun paramètre message requis.";

    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"Compression de mémoire : {levelDesc}. Période : {rangeDesc}.\n\nContenu de la mémoire :\n{contentText}";
    }

    // ===== Chinese Historical Calendar Localization =====

    private readonly ChineseHistoricalFrFR _chineseHistorical = new();
    public override ChineseHistoricalLocalizationBase GetChineseHistoricalLocalization() => _chineseHistorical;

    // Project Info Context
    public override string ProjectCtx_ProjectInfoHeader => "Affiliation au projet";
    public override string ProjectCtx_ProjectInfoRoleLabel => "Rôle";
    public override string ProjectCtx_ProjectInfoGoalLabel => "Objectif";

    // Project Role Context
    public override string ProjectCtx_RoleDefinitionsHeader => "Définitions des rôles";
    public override string ProjectCtx_RoleAssignmentsHeader => "Attributions des rôles";
    public override string ProjectCtx_NoWorkflowTemplate => "Aucun modèle de workflow attribué, aucune définition de rôle disponible";
    public override string ProjectCtx_RoleNeedsAttention => "⚠ {0} rôle(s) sont sous-effectif(s). Vous devez créer des êtres de silicium et les attribuer aux rôles";
    public override string ProjectCtx_StaffingActionPlanHeader => "Plan d'action de dotation en personnel";
    public override string ProjectCtx_TotalBeingsNeeded => "Nombre total d'êtres de silicium à créer : {0}";
    public override string ProjectCtx_StaffingRoleBreakdownHeader => "Détail des pénuries par rôle";
    public override string ProjectCtx_RoleShortageDetail => "{0} : besoin de {1}, actuellement {2} → manque {3}";
    public override string ProjectCtx_StaffingActionStepsHeader => "Étapes d'action suggérées";
    public override string ProjectCtx_StaffingStepCreateBeings => "1. Utilisez silicon_manager create_being pour créer {0} êtres de silicium (au moins 1 par rôle)";
    public override string ProjectCtx_StaffingStepAssignToProject => "2. Utilisez project assign pour ajouter les nouveaux êtres au projet";
    public override string ProjectCtx_StaffingStepAssignToRoles => "3. Utilisez project assign_role pour attribuer chaque être au rôle correspondant";
    public override string ProjectCtx_EmptyRolePoolAction => "⚠ Le pool de rôles est vide ! Le workflow définit {0} rôles. Créez des êtres de silicium et attribuez-les à chaque rôle";
    public override string ProjectCtx_RoleMinCount => "Min";
    public override string ProjectCtx_RoleMaxCount => "Max";
    public override string ProjectCtx_RoleMaxCountUnlimited => "∞";
    public override string ProjectCtx_RoleAssignedCount => "Attribués";
    public override string ProjectCtx_UnassignedRoles => "Rôles requis non attribués";
    public override string ProjectCtx_AvailableBeingsHeader => "Êtres disponibles (pas encore affectés à ce projet)";
    public override string ProjectCtx_AvailableBeingsHint => "Conseil : Envisagez d'affecter des êtres existants aux rôles avant d'en créer de nouveaux. Utilisez project assign et project assign_role.";
    public override string ProjectCtx_AttentionReasonsHeader => "Raisons nécessitant l'attention du projet";
    public override string ProjectCtx_UnsatisfiedRolesDetailHeader => "Détails des rôles non satisfaits";
    public override string ProjectAttention_MissingTemplate => "Modèle de workflow manquant";
    public override string ProjectAttention_EmptyRolePool => "Pool de rôles vide (aucun rôle attribué)";
    public override string ProjectAttention_UnsatisfiedRoles => "Certains rôles ne satisfont pas les exigences de personnel";

    // Role Staffing
    public override string RoleStaffing_Understaffed => "Sous-effectif";
    public override string RoleStaffing_Overstaffed => "Sur-effectif";
    public override string RoleStaffing_Full => "Complet";
    public override string RoleStaffing_Sufficient => "Suffisant";
    public override string RoleStaffing_UnderstaffedDetail => "Sous-effectif (besoin de {0}, a {1})";
    public override string RoleStaffing_OverstaffedDetail => "Sur-effectif (max {0}, a {1})";
    public override string RoleStaffing_FullDetail => "Complet ({0}/{1})";
    public override string RoleStaffing_SufficientDetail => "Suffisant ({0}/{1}+)";

    // ===== Workflow Role Notification =====
    public override string WorkflowRoleBlockedNotificationFormat => "[Notification de rôle de workflow] Le workflow du projet '{0}' est bloqué à la transition '{1}' ({2} → {3}).\n\nRôles manquants : {4}\n\nUtilisez l'action assign_role de project_tool pour attribuer les rôles nécessaires. Le workflow reprendra automatiquement lors de la prochaine vérification.";

    // ===== Project Think Session Localization =====
    public override string ProjectThinkHistoryLinkLabel => "Historique de réflexion";
    public override string ProjectThinkHistoryTitle => "Historique de réflexion du projet";
    public override string ProjectThinkHistoryHeader => "Historique de réflexion";
    public override string ProjectThinkBackToProjects => "← Retour aux projets";
    public override string ProjectThinkProjectName => "Projet : {0}";
    public override string ProjectThinkNoRecords => "Aucune session de réflexion";
    public override string ProjectThinkStateStarted => "Démarré";
    public override string ProjectThinkStateExecuting => "En cours d'exécution";
    public override string ProjectThinkStateCompleted => "Terminé";
    public override string ProjectThinkStateFailed => "Échoué";
    public override string ProjectThinkDetailTitle => "Détail de la session";
    public override string ProjectThinkDetailHeader => "Détail de la session";
    public override string ProjectThinkRoundLabel => "Tour : ";
    public override string ProjectThinkMessageCountLabel => "Messages : ";
    public override string ProjectThinkStateLabel => "État : ";
    public override string ProjectThinkCreatedAt => "Créé : ";
    public override string ProjectThinkCompletedAt => "Terminé : ";
    public override string ProjectThinkCycleLabel => "Cycle";
    public override string ProjectThinkRoundN => "Tour {0}";
}