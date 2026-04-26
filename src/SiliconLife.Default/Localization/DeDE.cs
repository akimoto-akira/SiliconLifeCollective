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
using SiliconLife.Default.ChineseHistorical;

namespace SiliconLife.Default;

/// <summary>
/// German (Germany) localization implementation
/// </summary>
public class DeDE : DefaultLocalizationBase
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "de-DE";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "Deutsch (Deutschland)";

    /// <summary>
    /// Gets the welcome message
    /// </summary>
    public override string WelcomeMessage => "Willkommen beim Silicon Life Collective!";

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
    public override string ShutdownMessage => "Herunterfahren...";

    /// <summary>
    /// Gets the config corrupted error message
    /// </summary>
    public override string ConfigCorruptedError => "Konfigurationsdatei beschädigt, Standardkonfiguration wird verwendet";

    /// <summary>
    /// Gets the config created message
    /// </summary>
    public override string ConfigCreatedWithDefaults => "Konfigurationsdatei nicht gefunden, Standardkonfiguration wurde erstellt";

    /// <summary>
    /// Gets the AI connection error message
    /// </summary>
    public override string AIConnectionError => "Verbindung zum KI-Dienst nicht möglich, bitte prüfen Sie ob Ollama läuft";

    /// <summary>
    /// Gets the AI request error message
    /// </summary>
    public override string AIRequestError => "KI-Anfrage fehlgeschlagen";

    /// <summary>
    /// Gets the data directory create error message
    /// </summary>
    public override string DataDirectoryCreateError => "Datenverzeichnis kann nicht erstellt werden";

    /// <summary>
    /// Gets the thinking message
    /// </summary>
    public override string ThinkingMessage => "Denke nach...";

    /// <summary>
    /// Gets the tool call message
    /// </summary>
    public override string ToolCallMessage => "Werkzeuge ausführen...";

    /// <summary>
    /// Gets the error message
    /// </summary>
    public override string ErrorMessage => "Fehler";

    /// <summary>
    /// Gets the unexpected error message
    /// </summary>
    public override string UnexpectedErrorMessage => "Unerwarteter Fehler";

    /// <summary>
    /// Gets the permission denied message
    /// </summary>
    public override string PermissionDeniedMessage => "Berechtigung verweigert";

    /// <summary>
    /// Gets the permission ask prompt
    /// </summary>
    public override string PermissionAskPrompt => "Erlauben? (y/n): ";

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public override string PermissionRequestHeader => "[Berechtigungsanfrage]";
    public override string PermissionRequestDescription => "Ein Silicon Being bittet um Ihre Genehmigung:";
    public override string PermissionRequestTypeLabel => "Berechtigungstyp:";
    public override string PermissionRequestResourceLabel => "Angeforderte Ressource:";
    public override string PermissionRequestAllowButton => "Erlauben";
    public override string PermissionRequestDenyButton => "Verweigern";
    public override string PermissionRequestCacheLabel => "Diese Entscheidung merken";
    public override string PermissionRequestDurationLabel => "Cache-Dauer";
    public override string PermissionRequestWaitingMessage => "Warte auf Antwort...";

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public override string AllowCodeLabel => "Erlaubnis-Code";

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public override string DenyCodeLabel => "Verweigerungs-Code";

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public override string PermissionReplyInstruction => "Geben Sie den Bestätigungscode ein oder einen anderen Text zum Verweigern";

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public override string AddToCachePrompt => "Diese Entscheidung zwischenspeichern? (y/n): ";

    /// <summary>
    /// Gets the label for the permission cache checkbox in the web UI
    /// </summary>
    public override string PermissionCacheLabel => "Diese Entscheidung merken";

    /// <summary>
    /// Gets the label for the cache duration selector in the permission dialog
    /// </summary>
    public override string PermissionCacheDurationLabel => "Cache-Dauer";

    /// <summary>
    /// Gets the option text for 1-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration1Hour => "1 Stunde";

    /// <summary>
    /// Gets the option text for 24-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration24Hours => "24 Stunden";

    /// <summary>
    /// Gets the option text for 7-day cache duration
    /// </summary>
    public override string PermissionCacheDuration7Days => "7 Tage";

    /// <summary>
    /// Gets the option text for 30-day cache duration
    /// </summary>
    public override string PermissionCacheDuration30Days => "30 Tage";

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "Netzwerkzugriff",
        PermissionType.CommandLine => "Befehlszeilenausführung",
        PermissionType.FileAccess => "Dateizugriff",
        PermissionType.Function => "Funktionsaufruf",
        PermissionType.DataAccess => "Datenzugriff",
        _ => permissionType.ToString()
    };

    /// <summary>
    /// Gets the title text for the permission dialog in the web UI
    /// </summary>
    public override string PermissionDialogTitle => "Berechtigungsanfrage";

    /// <summary>
    /// Gets the label for the permission type field in the permission dialog
    /// </summary>
    public override string PermissionTypeLabel => "Berechtigungstyp:";

    /// <summary>
    /// Gets the label for the requested resource field in the permission dialog
    /// </summary>
    public override string PermissionResourceLabel => "Angeforderte Ressource:";

    /// <summary>
    /// Gets the label for the detail information field in the permission dialog
    /// </summary>
    public override string PermissionDetailLabel => "Detailinformationen:";

    /// <summary>
    /// Gets the text for the allow button in the permission dialog
    /// </summary>
    public override string PermissionAllowButton => "Erlauben";

    /// <summary>
    /// Gets the text for the deny button in the permission dialog
    /// </summary>
    public override string PermissionDenyButton => "Verweigern";

    /// <summary>
    /// Gets the console error message when permission respond fails
    /// </summary>
    public override string PermissionRespondFailed => "Berechtigungsantwort fehlgeschlagen";

    /// <summary>
    /// Gets the console error prefix when permission respond throws an error
    /// </summary>
    public override string PermissionRespondError => "Berechtigungsantwortfehler: ";

    // ===== Init Page Localization =====

    public override string InitPageTitle => "Initialisierung";
    public override string InitDescription => "Erste Verwendung, bitte schließen Sie die Grundkonfiguration ab";
    public override string InitNicknameLabel => "Benutzername";
    public override string InitNicknamePlaceholder => "Bitte geben Sie Ihren Spitznamen ein";
    public override string InitEndpointLabel => "KI-API-Endpunkt";
    public override string InitEndpointPlaceholder => "z.B.: http://localhost:11434";
    public override string InitAIClientTypeLabel => "KI-Clienttyp";
    public override string InitModelLabel => "Standardmodell";
    public override string InitModelPlaceholder => "z.B.: qwen3.5:cloud";
    public override string InitSkinLabel => "Skin";
    public override string InitSkinPlaceholder => "Leer lassen für Standardskin";
    public override string InitDataDirectoryLabel => "Datenverzeichnis";
    public override string InitDataDirectoryPlaceholder => "z.B.: ./data";
    public override string InitDataDirectoryBrowse => "Durchsuchen...";
    public override string InitSkinSelected => "\u2713 Ausgewählt";
    public override string InitSkinPreviewTitle => "Vorschau";
    public override string InitSkinPreviewCardTitle => "Kartentitel";
    public override string InitSkinPreviewCardContent => "Dies ist eine Beispielkarte, die die UI-Wirkung dieses Skin-Stils zeigt.";
    public override string InitSkinPreviewPrimaryBtn => "Primäre Taste";
    public override string InitSkinPreviewSecondaryBtn => "Sekundäre Taste";
    public override string InitSubmitButton => "Initialisierung abschließen";
    public override string InitFooterHint => "Konfiguration kann jederzeit in den Einstellungen geändert werden";
    public override string InitNicknameRequiredError => "Bitte geben Sie einen Benutzernamen ein";
    public override string InitDataDirectoryRequiredError => "Bitte wählen Sie ein Datenverzeichnis";
    public override string InitCuratorNameLabel => "Silicon Being Name";
    public override string InitCuratorNamePlaceholder => "Bitte geben Sie den Namen des ersten Silicon Beings ein";
    public override string InitCuratorNameRequiredError => "Bitte geben Sie einen Silicon Being Namen ein";
    public override string InitLanguageLabel => "Sprache / Language";
    public override string InitLanguageSwitchBtn => "Anwenden";

    // ===== Navigation Menu Localization =====

    public override string NavMenuChat => "Chat";
    public override string NavMenuDashboard => "Dashboard";
    public override string NavMenuBeings => "Silicon Beings";
    public override string NavMenuAudit => "Audit";
    public override string NavMenuTasks => "Aufgaben";
    public override string NavMenuMemory => "Erinnerung";
    public override string NavMenuKnowledge => "Wissen";
    public override string NavMenuProjects => "Projekte";
    public override string NavMenuLogs => "Protokolle";
    public override string NavMenuConfig => "Konfiguration";
    public override string NavMenuHelp => "Hilfe";
    public override string NavMenuAbout => "Über";

    // ===== Page Title Localization =====

    public override string PageTitleChat => "Chat - Silicon Life Collective";
    public override string PageTitleDashboard => "Dashboard - Silicon Life Collective";
    public override string PageTitleBeings => "Silicon Being Verwaltung - Silicon Life Collective";
    public override string PageTitleTasks => "Aufgabenverwaltung - Silicon Life Collective";
    public override string PageTitleTimers => "Timer-Verwaltung - Silicon Life Collective";
    public override string PageTitleMemory => "Erinnerungsdurchsuchung - Silicon Life Collective";
    public override string PageTitleWorkNotes => "Arbeitsnotizen - Silicon Life Collective";
    public override string PageTitleKnowledge => "Wissensgraph - Silicon Life Collective";
    public override string PageTitleProjects => "Projektraumverwaltung - Silicon Life Collective";
    public override string PageTitleLogs => "Protokollabfrage - Silicon Life Collective";
    public override string PageTitleAudit => "Token-Audit - Silicon Life Collective";
    public override string PageTitleConfig => "Systemkonfiguration - Silicon Life Collective";
    public override string PageTitleExecutor => "Executor-Überwachung - Silicon Life Collective";
    public override string PageTitleCodeBrowser => "Code-Browser - Silicon Life Collective";
    public override string PageTitlePermission => "Berechtigungsverwaltung - Silicon Life Collective";
    public override string PageTitleAbout => "Über - Silicon Life Collective";

    // ===== Memory Page Localization =====

    public override string MemoryPageHeader => "Erinnerungsdurchsicht";
    public override string WorkNotesPageHeader => "Arbeitsnotizen";
    public override string WorkNotesTotalPages => "Insgesamt {0} Seiten";
    public override string WorkNotesEmptyState => "Noch keine Arbeitsnotizen";
    public override string WorkNotesSearchPlaceholder => "Notizen suchen...";
    public override string WorkNotesSearchButton => "Suchen";
    public override string WorkNotesNoSearchResults => "Keine passenden Notizen gefunden";
    public override string MemoryEmptyState => "Noch keine Erinnerungsdaten";
    public override string MemorySearchPlaceholder => "Erinnerungen suchen...";
    public override string MemorySearchButton => "Suchen";
    public override string MemoryFilterAll => "Alle";
    public override string MemoryFilterSummaryOnly => "Nur Zusammenfassungen";
    public override string MemoryFilterOriginalOnly => "Nur Originale";
    public override string MemoryStatTotal => "Erinnerungen gesamt";
    public override string MemoryStatOldest => "Älteste Erinnerung";
    public override string MemoryStatNewest => "Neueste Erinnerung";
    public override string MemoryIsSummaryBadge => "Komprimierte Zusammenfassung";
    public override string MemoryPaginationPrev => "Vorherige Seite";
    public override string MemoryPaginationNext => "Nächste Seite";
    public override string MemoryFilterTypeLabel => "Typ";
    public override string MemoryFilterDateFrom => "Startdatum";
    public override string MemoryFilterDateTo => "Enddatum";
    public override string MemoryFilterApply => "Anwenden";
    public override string MemoryFilterReset => "Zurücksetzen";
    public override string MemoryTypeChat => "Gespräch";
    public override string MemoryTypeToolCall => "Werkzeugaufruf";
    public override string MemoryTypeTask => "Aufgabe";
    public override string MemoryTypeTimer => "Timer";
    public override string MemoryDetailTitle => "Erinnerungsdetails";
    public override string MemoryDetailClose => "Schließen";
    public override string MemoryDetailId => "ID";
    public override string MemoryDetailContent => "Inhalt";
    public override string MemoryDetailCreatedAt => "Erstellungszeit";
    public override string MemoryDetailRelatedBeings => "Verknüpfte Agenten";
    public override string MemoryDetailKeywords => "Schlüsselwörter";
    public override string MemoryStatTypeDistribution => "Typverteilung";
    public override string MemoryStatKeywordFrequency => "Schlüsselworthäufigkeit";
    public override string MemoryCardViewDetail => "Details anzeigen";

    // ===== Projects Page Localization =====

    public override string ProjectsPageHeader => "Projektraumverwaltung";
    public override string ProjectsEmptyState => "Noch keine Projekte";
    public override string ProjectsActiveLabel => "Aktiv";
    public override string ProjectsArchivedLabel => "Archiviert";
    
    public override string ProjectStatusActiveLabel => "Aktiv";
    
    public override string ProjectStatusArchivedLabel => "Archiviert";
    
    public override string ProjectStatusDestroyedLabel => "Zerstört";
    
    public override string ProjectTasksLinkLabel => "Aufgaben";
    
    public override string ProjectWorkNotesLinkLabel => "Arbeitsnotizen";
    public override string ProjectWorkNotesPageHeader => "Projektarbeitsnotizen";
    public override string ProjectWorkNotesEmptyState => "Dieses Projekt hat noch keine Arbeitsnotizen";
    public override string ProjectWorkNotesTotalPages => "Gesamtseiten: {0}";

    // ===== Tasks Page Localization =====

    public override string TasksPageHeader => "Aufgabenverwaltung";
    public override string TasksEmptyState => "Noch keine Aufgaben";
    public override string TasksStatusPending => "Ausstehend";
    public override string TasksStatusRunning => "Läuft";
    public override string TasksStatusCompleted => "Abgeschlossen";
    public override string TasksStatusFailed => "Fehlgeschlagen";
    public override string TasksStatusCancelled => "Abgebrochen";
    public override string TasksPriorityLabel => "Priorität";
    public override string TasksAssignedToLabel => "Verantwortlich";
    public override string TasksCreatedAtLabel => "Erstellungszeit";
    
    public override string ProjectTasksPageHeader => "Projektaufgaben";
    
    public override string ProjectTasksEmptyState => "Noch keine Projektaufgaben";
    
    public override string ProjectTasksAssigneesLabel => "Verantwortliche";
    
    public override string ProjectTasksCreatedByLabel => "Erstellt von";
    
    public override string ProjectTasksBackToProjects => "← Zurück zur Projektliste";
    public override string ProjectTasksNoAssigneesLabel => "Keine";
    
    // ===== Code Browser Page Localization =====

    public override string CodeBrowserPageHeader => "Code-Browser";

    // ===== Executor Page Localization =====

    public override string ExecutorPageHeader => "Executor-Überwachung";

    // ===== Permission Page Localization =====

    public override string PermissionPageHeader => "Berechtigungsverwaltung - {0}";
    public override string PermissionEmptyState => "Noch Berechtigungsregeln";
    public override string PermissionMissingBeingId => "Fehlender Silicon Being ID-Parameter";
    public override string PermissionBeingNotFound => "Silicon Being nicht gefunden";
    public override string PermissionTemplateHeader => "Standard-Berechtigungs-Callback-Vorlage";
    public override string PermissionTemplateDescription => "Nach dem Speichern wird das Standardverhalten überschrieben, nach dem Löschen wird es wiederhergestellt";
    public override string PermissionCallbackClassSummary => "Berechtigungs-Callback-Implementierung.";
    public override string PermissionCallbackClassSummary2 => "Domänenspezifische Berechtigungsregeln, vollständig konform mit dpf.txt-Spezifikation.\n/// Abdeckung: Netzwerk (Whitelist/Blacklist/IP-Bereiche), Befehlszeile (plattformübergreifend),\n/// Dateizugriff (gefährliche Erweiterungen, Systemverzeichnisse, Benutzerverzeichnisse) und Fallback-Standards.";
    public override string PermissionCallbackConstructorSummary => "Erstellt einen PermissionCallback mit dem Anwendungsdatenverzeichnis.";
    public override string PermissionCallbackConstructorSummary2 => "Das Anwendungsdatenverzeichnis wird verwendet für:\n    /// - Blockieren des Zugriffs auf das Datenverzeichnis (außer dem eigenen Temp-Unterordner)\n    /// - Ableiten des pro-Silicon-Being-Datenverzeichnisses für Temp-Erlaubnisregeln";
    public override string PermissionCallbackConstructorParam => "Pfad zum globalen Anwendungsdatenverzeichnis";
    public override string PermissionCallbackEvaluateSummary => "Bewertet eine Berechtigungsanfrage mit Regeln (dpf.txt-Spezifikation).";
    public override string PermissionRuleOtherTypesDefault => "Andere Berechtigungstypen werden standardmäßig durchgelassen";

    public override string GetPermissionRuleComment(string key) => key switch
    {
        "NetRuleNetworkAccess" => "Netzwerkoperation Erlaubnisregel",
        "NetRuleCommandLine" => "Befehlszeilenregel (plattformübergreifend)",
        "NetRuleFileAccess" => "Dateizugriffsregel (plattformübergreifend)",
        "NetRuleNoProtocol" => "Kein Protokollname (kein Doppelpunkt), Quelle kann nicht bestimmt werden, Benutzer fragen",
        "NetRuleLoopback" => "Loopback-Adresse erlauben (localhost / 127.0.0.1 / ::1)",
        "NetRulePrivateIPMatch" => "Privater IP-Adressbereichabgleich (erst gültige IPv4-Adresse prüfen)",
        "NetRulePrivateC" => "Privater C-Klasse-Adressbereich erlauben (192.168.0.0/16)",
        "NetRulePrivateA" => "Privater A-Klasse-Adressbereich erlauben (10.0.0.0/8)",
        "NetRulePrivateB" => "Privater B-Klasse-Adressbereich selektiv erlauben (172.16.0.0/12, d.h. 172.16.* ~ 172.31.*)",
        "NetRuleDomainWhitelist1" => "Erlaubte externe Domains-Whitelist — Google / Bing / Tencent / Sogou / DuckDuckGo / Yandex / WeChat / Alibaba",
        "NetRuleVideoPlatforms" => "Bilibili / niconico / Acfun / Douyin / TikTok / Kuaishou / Xiaohongshu",
        "NetRuleAIServices" => "KI-Dienste — OpenAI / Anthropic / HuggingFace / Ollama / Tongyi Qianwen / Kimi / Doubao / Jianying / Trae IDE",
        "NetRulePhishingBlacklist" => "Phishing/Nachahmung-Website-Blacklist (Schlüsselwort-Unschärfeabgleich)",
        "NetRulePhishingAI" => "KI-Nachahmungsseite",
        "NetRuleMaliciousAI" => "Bösartiges KI-Tool",
        "NetRuleAdversarialAI" => "Adversariales KI / Prompt-Jailbreak / LLM-Angriffsseiten",
        "NetRuleAIContentFarm" => "KI-Content-Farm / KI-Spam-Inhalte",
        "NetRuleAIBlackMarket" => "KI-Daten-Schwarzmarkt / API-Schlüssel-Schwarzmarkt / LLM-Gewichtungsverkauf",
        "NetRuleAIFakeScam" => "KI-Nachahmung/Betrug allgemeine Schlüsselwörter",
        "NetRuleOtherBlacklist" => "Andere Blacklist-Seiten — sakura-cat: Sollte nicht von KI accessed werden / 4399: Spiele mit Viren vermischt",
        "NetRuleSecuritiesTrading" => "Wertpapierhandelsplattform (Benutzer fragen) — Huatai Securities / Guotai Junan / CITIC Securities / China Merchants Securities / GF Securities / Haitong Securities / Shenwan Hongyuan / Orient Securities / Guosen Securities / Industrial Securities",
        "NetRuleThirdPartyTrading" => "Drittplattform-Handel (Benutzer fragen) — Tonghuashun / East Money / Tongdaxin / Bloomberg / Yahoo Finance",
        "NetRuleStockExchanges" => "Wertpapierbörsen (nur Marktdaten) — Shanghai Stock Exchange / Shenzhen Stock Exchange / CNINFO",
        "NetRuleFinancialNews" => "Finanznachrichten (nur Marktdaten) — JRJ / Securities Times / Hexun",
        "NetRuleInvestCommunity" => "Investment-Community (nur Nachrichten) — Xueqiu / CLS / Kaipanla / Taoguba",
        "NetRuleDevServices" => "Entwicklerdienste — GitHub / Gitee / StackOverflow / npm / NuGet / PyPI / Microsoft",
        "NetRuleGameEngines" => "Spiel-Engines — Unity / Unreal Engine / Epic Games / Fab Resource Store",
        "NetRuleGamePlatforms" => "Spielplattformen — Steam Benutzer fragen, EA / Ubisoft / Blizzard / Nintendo erlauben",
        "NetRuleSEGA" => "SEGA (Japan)",
        "NetRuleCloudServices" => "Globale Cloud-Service-Plattformen — Azure / Google Cloud / DigitalOcean / Heroku / Vercel / Netlify",
        "NetRuleDevDeployTools" => "Globale Entwicklungs- und Bereitstellungstools — GitLab / Bitbucket / Docker / Cloudflare",
        "NetRuleCloudDevTools" => "Cloud-Services und Entwicklungstools — Amazon / AWS / Kiro IDE / CodeBuddy IDE / JetBrains / Chenguang Studio / W3School Chinese",
        "NetRuleChinaSocialNews" => "Soziales/Nachrichten (Festlandchina) — Weibo / Zhihu / NetEase / Sina / ifeng / Xinhua / CCTV",
        "NetRuleTaiwanMediaCTI" => "Taiwan-Medien — CTI News",
        "NetRuleTaiwanMediaSET" => "SET News (Taiwan) — Benutzer fragen",
        "NetRuleTaiwanWIN" => "Internet Content Protection Agency (Taiwan, Blockierungsrisiko) — Verboten",
        "NetRuleJapanMedia" => "Japanische Medien — NHK",
        "NetRuleRussianMedia" => "Russische Medien — Sputnik News",
        "NetRuleKoreanMedia" => "Koreanische Medien — KBS / MBC / SBS / EBS",
        "NetRuleDPRKMedia" => "Nordkoreanische Medien — Uriminzokkiri / Rodong Sinmun / Youth Vanguard / Voice of Korea / Pyongyang Times / Chongryon",
        "NetRuleGovWebsites" => "Regierungswebsites (Wildcard .gov-Domain)",
        "NetRuleGlobalSocialCollab" => "Globale Soziale/Kollaborationsplattformen — Reddit / Discord / Slack / Notion / Figma / Dropbox",
        "NetRuleOverseasSocial" => "Übersee-Soziales/Livestreaming (Benutzer fragen) — Twitch / Facebook / X / Gmail / Instagram / lit.link",
        "NetRuleWhatsApp" => "WhatsApp (Meta) — Erlauben",
        "NetRuleThreads" => "Threads (Meta) — Verboten",
        "NetRuleGlobalVideoMusic" => "Globale Video/Musik-Plattformen — Spotify / Apple Music / Vimeo",
        "NetRuleVideoMedia" => "Video/Medien — YouTube / iQIYI / Youku",
        "NetRuleMaps" => "Karten — OpenStreetMap",
        "NetRuleEncyclopedia" => "Enzyklopädie — Wikipedia / MediaWiki / Creative Commons (CC)",
        "NetRuleUnmatched" => "Nicht abgestimmter Netzwerkzugriff, Benutzer fragen",
        "CmdRuleSeparatorDetect" => "Pipe- und Mehrfachbefehls-Separatoren erkennen, einzeln validieren",
        "CmdRuleWinAllow" => "Windows erlaubt: Nur-Lesen/Abfrage-Befehle — dir / tree / tasklist / ipconfig / ping / tracert / systeminfo / whoami / set / path / sc query / findstr",
        "CmdRuleWinDeny" => "Windows verboten: Gefährliche/destruktive Befehle — del / rmdir / format / diskpart / reg delete",
        "CmdRuleLinuxAllow" => "Linux erlaubt: Nur-Lesen/Abfrage-Befehle — ls / tree / ps / top / ifconfig / ip / ping / traceroute / uname / whoami / env / cat / grep / find / df / du / systemctl status",
        "CmdRuleLinuxDeny" => "Linux verboten: Gefährliche/destruktive Befehle — rm / rmdir / mkfs / fdisk / dd / chmod / chown / chgrp",
        "CmdRuleMacAllow" => "macOS erlaubt: Nur-Lesen/Abfrage-Befehle — ls / tree / ps / top / ifconfig / ping / traceroute / system_profiler / sw_vers / whoami / env / cat / grep / find / df / du / launchctl list",
        "CmdRuleMacDeny" => "macOS verboten: Gefährliche/destruktive Befehle — rm / rmdir / diskutil erasedisk / dd / chmod / chown / chgrp",
        "CmdRuleUnmatched" => "Nicht abgestimmter Befehl, Benutzer fragen",
        "FileRuleDangerousExt" => "Höchste Priorität: Gefährliche Dateierweiterungen sofort ablehnen",
        "FileRuleInvalidPath" => "Kann nicht in absoluten Pfad aufgelöst werden, Benutzer fragen",
        "FileRuleDenyAssemblyDir" => "Verboten: Aktuelles Assembly-Verzeichnis",
        "FileRuleDenyAppDataDir" => "Verboten: Anwendungsdatenverzeichnis",
        "FileRuleAllowOwnTemp" => "Aber erlaubt: Eigenes Temp-Verzeichnis",
        "FileRuleOwnTemp" => "Erlaubt: Eigene Temp-Verzeichnis",
        "FileRuleDenyOtherDataDir" => "Verboten: Andere Datenverzeichnispfade (einschließlich anderer Silicon Beings)",
        "FileRuleUserFolders" => "Erlaubt: Benutzer-Standardordner",
        "FileRuleUserFolderCheck" => "Benutzer-Standardordner — Desktop / Downloads / Dokumente / Bilder / Musik / Videos",
        "FileRulePublicFolders" => "Erlaubt: Öffentliche Benutzerordner",
        "FileRuleWinDenySystem" => "Windows verboten: Systemkritische Verzeichnisse (nicht unbedingt auf C:)",
        "FileRuleWinDenySystemCheck" => "Systemkritische Verzeichnisse",
        "FileRuleLinuxDenySystem" => "Linux verboten: Systemkritische Verzeichnisse — /etc /boot /sbin",
        "FileRuleMacDenySystem" => "macOS verboten: Systemkritische Verzeichnisse — /System /Library /private/etc",
        "FileRuleUnmatched" => "Nicht abgestimmter Pfad, Benutzer fragen",
        _ => key
    };

    public override string PermissionRulesSection => "Berechtigungsregelliste";
    public override string PermissionEditorSection => "Berechtigungsregeleditor";

    public override string PermissionSaveMissingBeingId => "Fehlende oder ungültige Silicon Being ID";
    public override string PermissionSaveMissingCode => "Fehlender Code im Anfragetext";
    public override string PermissionSaveLoaderNotAvailable => "DynamicBeingLoader nicht verfügbar";
    public override string PermissionSaveRemoveFailed => "Entfernen des Berechtigungs-Calls fehlgeschlagen";
    public override string PermissionSaveRemoveSuccess => "Berechtigungs-Callback entfernt";
    public override string PermissionSaveSecurityScanFailed => "Speichern des Berechtigungs-Calls fehlgeschlagen (Sicherheits-Scan fehlgeschlagen)";
    public override string PermissionSaveCompilationFailed => "Kompilierung fehlgeschlagen";
    public override string PermissionSaveSuccess => "Berechtigungs-Callback erfolgreich gespeichert und angewendet";
    public override string PermissionSaveError => "Fehler beim Speichern des Berechtigungs-Callbacks";

    // ===== Knowledge Page Localization =====

    public override string KnowledgePageHeader => "Wissensgraph-Visualisierung";
    public override string KnowledgeLoadingState => "Wissensgraph-Daten werden geladen...";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "Chat mit {0}";
    public override string ChatConversationsHeader => "Gespräche";
    public override string ChatNoConversationSelected => "Wählen Sie ein Gespräch zum Chatten";
    public override string ChatMessageInputPlaceholder => "Nachricht eingeben...";
    public override string ChatLoading => "Laden...";
    public override string ChatSendButton => "Senden";
    public override string ChatFileSourceDialogTitle => "Dateiquelle wählen";
    public override string ChatFileSourceServerFile => "Serverdatei wählen";
    public override string ChatFileSourceUploadLocal => "Lokale Datei hochladen";
    public override string ChatUserDisplayName => "Ich";
    public override string ChatUserAvatarName => "Ich";
    public override string ChatDefaultBeingName => "KI";
    public override string ChatThinkingSummary => "💭 Denkprozess (Klick zum Erweitern)";
    public override string GetChatToolCallsSummary(int count) => $"🔧 Werkzeugaufrufe ({count} Einträge)";

    // ===== Dashboard Localization =====

    public override string DashboardPageHeader => "Dashboard";
    public override string DashboardStatTotalBeings => "Silicon Beings Anzahl";
    public override string DashboardStatActiveBeings => "Aktive Silicon Beings";
    public override string DashboardStatUptime => "Betriebszeit";
    public override string DashboardStatMemory => "Speichernutzung";
    public override string DashboardChartMessageFrequency => "Nachrichtenfrequenz";

    // ===== Beings Localization =====

    public override string BeingsPageHeader => "Silicon Being Verwaltung";
    public override string BeingsTotalCount => "Insgesamt {0} Silicon Beings";
    public override string BeingsNoSelectionPlaceholder => "Wählen Sie ein Silicon Being für Details";
    public override string BeingsEmptyState => "Noch keine Silicon Beings";
    public override string BeingsStatusIdle => "Leerlauf";
    public override string BeingsStatusRunning => "Läuft";
    public override string BeingsDetailIdLabel => "ID: ";
    public override string BeingsDetailStatusLabel => "Status: ";
    public override string BeingsDetailCustomCompileLabel => "Benutzerdefinierte Kompilierung: ";
    public override string BeingsDetailSoulContentLabel => "Seeleninhalt: ";
    public override string BeingsDetailSoulContentEditLink => "Seele bearbeiten";
    public override string BeingsBackToList => "Zurück zur Liste";
    public override string SoulEditorSubtitle => "Bearbeitung der Soul-Datei des Silicon Beings (Markdown-Format)";
    public override string BeingsDetailMemoryLabel => "Erinnerung: ";
    public override string BeingsDetailMemoryViewLink => "Anzeigen";
    public override string BeingsDetailPermissionLabel => "Berechtigung: ";
    public override string BeingsDetailPermissionEditLink => "Bearbeiten";
    public override string BeingsDetailTimersLabel => "Timer: ";
    public override string BeingsDetailTasksLabel => "Aufgaben: ";
    public override string BeingsDetailAIClientLabel => "Unabhängiger KI-Client: ";
    public override string BeingsDetailAIClientEditLink => "Bearbeiten";
    public override string BeingsDetailChatHistoryLabel => "Chatverlauf: ";
    public override string BeingsDetailWorkNoteLabel => "Arbeitsnotiz: ";
    public override string BeingsDetailChatHistoryLink => "Chatverlauf anzeigen";
    public override string BeingsDetailWorkNoteLink => "Arbeitsnotiz anzeigen";
    public override string WorkNotePageTitle => "Arbeitsnotizen";
    public override string WorkNotePageHeader => "Arbeitsnotizliste";
    public override string WorkNotePageDescription => "Verwaltung und Anzeige der Arbeitsnotizen des Silicon Beings";
    public override string ChatHistoryPageTitle => "Chatverlauf";
    public override string ChatHistoryPageHeader => "Gesprächsliste";
    public override string ChatHistoryConversationList => "Gesprächsliste";
    public override string ChatHistoryBackToList => "Zurück zur Gesprächsliste";
    public override string ChatHistoryNoConversations => "Noch keine Gesprächsaufzeichnungen";
    public override string ChatDetailPageTitle => "Chatdetails";
    public override string ChatDetailPageHeader => "Gesprächsdetails";
    public override string ChatDetailNoMessages => "Noch keine Nachrichten";
    public override string BeingsYes => "Ja";
    public override string BeingsNo => "Nein";
    public override string BeingsNotSet => "Nicht gesetzt";

    // ===== Timers Page Localization =====

    public override string TimersPageHeader => "Timer-Verwaltung";
    public override string TimersTotalCount => "Insgesamt {0} Timer";
    public override string TimersEmptyState => "Noch keine Timer";
    public override string TimerViewExecutionHistory => "📝 Ausführungsverlauf anzeigen";
    public override string TimerExecutionHistoryTitle => "Timer-Ausführungsverlauf";
    public override string TimerExecutionHistoryHeader => "Ausführungsprotokoll";
    public override string TimerExecutionBackToTimers => "← Zurück zur Timer-Liste";
    public override string TimerExecutionTimerName => "Timer: {0}";
    public override string TimerExecutionDetailTitle => "Ausführungsdetails";
    public override string TimerExecutionDetailHeader => "Ausführungs-Nachrichtenprotokoll";
    public override string TimerExecutionNoRecords => "Noch keine Ausführungsprotokolle";
    public override string TimersStatusActive => "Läuft";
    public override string TimersStatusPaused => "Pausiert";
    public override string TimersStatusTriggered => "Ausgelöst";
    public override string TimersStatusCancelled => "Abgebrochen";
    public override string TimersTypeRecurring => "Wiederkehrend";
    public override string TimersTriggerTimeLabel => "Auslösezeit: ";
    public override string TimersIntervalLabel => "Intervall: ";
    public override string TimersCalendarLabel => "Kalenderbedingung: ";
    public override string TimersTriggeredCountLabel => "Ausgelöst: ";

    // ===== Chat Page Localization =====

    public override string AboutPageHeader => "Über";
    public override string AboutAppName => "Silicon Life Collective";
    public override string AboutVersionLabel => "Version";
    public override string AboutDescription => "Ein KI-basiertes Silicon Life Collective Verwaltungssystem, das die Zusammenarbeit mehrerer KI-Agenten, Erinnerungsverwaltung, Wissensgraph-Konstruktion und andere Funktionen unterstützt.";
    public override string AboutAuthorLabel => "Autor";
    public override string AboutAuthorName => "Hoshino Kennji";
    public override string AboutLicenseLabel => "Lizenz";
    public override string AboutCopyright => "Urheberrecht (c) 2026 Hoshino Kennji";
    public override string AboutGitHubLink => "GitHub Repository";
    public override string AboutGiteeLink => "Gitee Mirror";
    public override string AboutSocialMediaLabel => "Social Media Plattformen";
    public override string GetSocialMediaName(string platform) => platform switch
    {
        "Bilibili" => "Bilibili",
        "YouTube" => "YouTube",
        "X" => "X (Twitter)",
        "Douyin" => "Douyin",
        "Weibo" => "Weibo",
        "WeChat" => "WeChat Öffentliche Konto",
        "Xiaohongshu" => "Xiaohongshu",
        "Zhihu" => "Zhihu",
        "TouTiao" => "Toutiao",
        "Kuaishou" => "Kuaishou",
        _ => platform
    };

    // ===== Config Page Localization =====

    public override string ConfigPageHeader => "Systemkonfiguration";
    public override string ConfigPropertyNameLabel => "Eigenschaftsname";
    public override string ConfigPropertyValueLabel => "Eigenschaftswert";
    public override string ConfigActionLabel => "Aktion";
    public override string ConfigEditButton => "Bearbeiten";
    public override string ConfigEditModalTitle => "Konfigurationselement bearbeiten";
    public override string ConfigEditPropertyLabel => "Eigenschaftsname: ";
    public override string ConfigEditValueLabel => "Eigenschaftswert: ";
    public override string ConfigBrowseButton => "Durchsuchen";
    public override string ConfigTimeSettingsLabel => "Zeiteinstellungen: ";
    public override string ConfigDaysLabel => "Tage: ";
    public override string ConfigHoursLabel => "Stunden: ";
    public override string ConfigMinutesLabel => "Minuten: ";
    public override string ConfigSecondsLabel => "Sekunden: ";
    public override string ConfigSaveButton => "Speichern";
    public override string ConfigCancelButton => "Abbrechen";
    public override string ConfigNullValue => "Null";

    public override string ConfigEditPrefix => "Bearbeiten: ";
    public override string ConfigDefaultGroupName => "Andere";
    public override string ConfigErrorInvalidRequest => "Ungültiger Anfrageparameter";
    public override string ConfigErrorInstanceNotFound => "Konfigurationsinstanz nicht gefunden";
    public override string ConfigErrorPropertyNotFound => "Eigenschaft {0} nicht gefunden oder nicht schreibbar";
    public override string ConfigErrorConvertInt => "Konnte '{0}' nicht in Ganzzahl konvertieren";
    public override string ConfigErrorConvertLong => "Konnte '{0}' nicht in lange Ganzzahl konvertieren";
    public override string ConfigErrorConvertDouble => "Konnte '{0}' nicht in Gleitkommazahl konvertieren";
    public override string ConfigErrorConvertBool => "Konnte '{0}' nicht in Boolean konvertieren";
    public override string ConfigErrorConvertGuid => "Konnte '{0}' nicht in GUID konvertieren";
    public override string ConfigErrorConvertTimeSpan => "Konnte '{0}' nicht in TimeSpan konvertieren";
    public override string ConfigErrorConvertDateTime => "Konnte '{0}' nicht in DateTime konvertieren";
    public override string ConfigErrorConvertEnum => "Konnte '{0}' nicht in {1} konvertieren";
    public override string ConfigErrorUnsupportedType => "Nicht unterstützter Eigenschaftstyp: {0}";
    public override string ConfigErrorSaveFailed => "Speichern fehlgeschlagen: {0}";
    public override string ConfigSaveFailed => "Speichern fehlgeschlagen: ";
    public override string ConfigDictionaryLabel => "Dictionary";
    public override string ConfigDictKeyLabel => "Schlüssel: ";
    public override string ConfigDictValueLabel => "Wert: ";
    public override string ConfigDictAddButton => "Hinzufügen";
    public override string ConfigDictDeleteButton => "Löschen";
    public override string ConfigDictEmptyMessage => "Dictionary ist leer";

    public override string LogsPageHeader => "Protokollabfrage";
    public override string LogsTotalCount => "Insgesamt {0} Protokolleinträge";
    public override string LogsStartTime => "Startzeit";
    public override string LogsEndTime => "Endzeit";
    public override string LogsLevelAll => "Alle Ebenen";
    public override string LogsBeingFilter => "Silicon Being";
    public override string LogsAllBeings => "Nicht filtern";
    public override string LogsSystemOnly => "Nur System";
    public override string LogsFilterButton => "Abfragen";
    public override string LogsEmptyState => "Noch keine Protokolleinträge";
    public override string LogsExceptionLabel => "Ausnahmedetails: ";
    public override string LogsPrevPage => "Vorherige Seite";
    public override string LogsNextPage => "Nächste Seite";

    public override string AuditPageHeader => "Token-Verbrauchsprüfung";
    public override string AuditTotalTokens => "Gesamt-Token";
    public override string AuditTotalRequests => "Gesamt-Anfragen";
    public override string AuditSuccessCount => "Erfolg";
    public override string AuditFailureCount => "Fehler";
    public override string AuditPromptTokens => "Eingabe-Token";
    public override string AuditCompletionTokens => "Ausgabe-Token";
    public override string AuditStartTime => "Startzeit";
    public override string AuditEndTime => "Endzeit";
    public override string AuditFilterButton => "Abfragen";
    public override string AuditEmptyState => "Noch keine Prüfungsprotokolle";
    public override string AuditAIClientType => "KI-Client";
    public override string AuditAllClientTypes => "Alle Typen";
    public override string AuditGroupByClient => "Nach Client gruppieren";
    public override string AuditGroupByBeing => "Nach Silicon Being gruppieren";
    public override string AuditPrevPage => "Vorherige Seite";
    public override string AuditNextPage => "Nächste Seite";
    public override string AuditBeing => "Silicon Being";
    public override string AuditAllBeings => "Alle Silicon Beings";
    public override string AuditTimeToday => "Heute";
    public override string AuditTimeWeek => "Diese Woche";
    public override string AuditTimeMonth => "Dieser Monat";
    public override string AuditTimeYear => "Dieses Jahr";
    public override string AuditExport => "Exportieren";
    public override string AuditTrendTitle => "Token-Verbrauchstrend";
    public override string AuditTrendPrompt => "Eingabe-Token";
    public override string AuditTrendCompletion => "Ausgabe-Token";
    public override string AuditTrendTotal => "Gesamt-Token";
    public override string AuditTooltipDate => "Datum";
    public override string AuditTooltipPrompt => "Eingabe-Token";
    public override string AuditTooltipCompletion => "Ausgabe-Token";
    public override string AuditTooltipTotal => "Gesamt-Token";

    private static readonly Dictionary<string, string> ConfigGroupNames = new()
    {
        ["Basic"] = "Grundkonfiguration",
        ["Runtime"] = "Laufzeitkonfiguration",
        ["AI"] = "KI-Konfiguration",
        ["Web"] = "Web-Konfiguration",
        ["User"] = "Benutzerkonfiguration"
    };

    private static readonly Dictionary<string, string> ConfigDisplayNames = new()
    {
        ["DataDirectory"] = "Datenverzeichnis",
        ["Language"] = "Spracheinstellung",
        ["TickTimeout"] = "Tick-Timeout",
        ["MaxTimeoutCount"] = "Maximale Timeout-Anzahl",
        ["WatchdogTimeout"] = "Watchdog-Timeout",
        ["MinLogLevel"] = "Minimale Protokollebene",
        ["AIClientType"] = "KI-Clienttyp",
        ["OllamaClient"] = "Ollama-Client",
        ["OllamaEndpoint"] = "Ollama-Endpunkt",
        ["DefaultModel"] = "Standardmodell",
        ["Temperature"] = "Temperatur",
        ["MaxTokens"] = "Maximale Token-Anzahl",
        ["DashScopeClient"] = "DashScope-Client",
        ["DashScopeApiKey"] = "API-Schlüssel",
        ["DashScopeRegion"] = "Dienstregion",
        ["DashScopeModel"] = "Modell",
        ["DashScopeRegionBeijing"] = "China Nord 2 (Peking)",
        ["DashScopeRegionVirginia"] = "USA (Virginia)",
        ["DashScopeRegionSingapore"] = "Singapur",
        ["DashScopeRegionHongkong"] = "China Hongkong",
        ["DashScopeRegionFrankfurt"] = "Deutschland (Frankfurt)",
        ["DashScopeModel_qwen3-max"] = "Qwen3 Max (Flaggschiff)",
        ["DashScopeModel_qwen3.6-plus"] = "Qwen3.6 Plus (Kosten-Leistung)",
        ["DashScopeModel_qwen3.6-flash"] = "Qwen3.6 Flash (Schnell)",
        ["DashScopeModel_qwen-max"] = "Qwen Max (Stabiles Flaggschiff)",
        ["DashScopeModel_qwen-plus"] = "Qwen Plus (Stabiles Gleichgewicht)",
        ["DashScopeModel_qwen-turbo"] = "Qwen Turbo (Stabil und schnell)",
        ["DashScopeModel_qwen3-coder-plus"] = "Qwen3 Coder Plus (Code)",
        ["DashScopeModel_qwq-plus"] = "QwQ Plus (Tiefes Reasoning)",
        ["DashScopeModel_deepseek-v3.2"] = "DeepSeek V3.2",
        ["DashScopeModel_deepseek-r1"] = "DeepSeek R1 (Reasoning)",
        ["DashScopeModel_glm-5.1"] = "GLM 5.1 (Zhipu)",
        ["DashScopeModel_kimi-k2.5"] = "Kimi K2.5 (Langer Kontext)",
        ["DashScopeModel_llama-4-maverick"] = "Llama 4 Maverick",
        ["WebPort"] = "Web-Port",
        ["AllowIntranetAccess"] = "Intranet-Zugriff erlauben",
        ["WebSkin"] = "Web-Skin",
        ["UserNickname"] = "Benutzername"
    };

    private static readonly Dictionary<string, string> ConfigDescriptions = new()
    {
        ["DataDirectory"] = "Pfad zum Datenverzeichnis für alle Anwendungsdaten",
        ["Language"] = "Spracheinstellung der Anwendung",
        ["TickTimeout"] = "Timeout-Dauer für jede Tick-Ausführung",
        ["MaxTimeoutCount"] = "Maximale Anzahl aufeinanderfolgender Timeouts vor Circuit Breaker",
        ["WatchdogTimeout"] = "Watchdog-Timeout zur Erkennung von Blockaden der Hauptschleife",
        ["MinLogLevel"] = "Globale minimale Protokollebene",
        ["AIClientType"] = "Zu verwendender KI-Clienttyp",
        ["OllamaEndpoint"] = "Ollama API-Endpunkt-URL",
        ["DefaultModel"] = "Standardmäßig verwendetes KI-Modell",
        ["DashScopeApiKey"] = "Alibaba Cloud DashScope API-Schlüssel",
        ["DashScopeRegion"] = "Alibaba Cloud DashScope Dienstregion",
        ["DashScopeModel"] = "Auf Alibaba Cloud DashScope verwendetes Modell",
        ["WebPort"] = "Web-Server-Port",
        ["AllowIntranetAccess"] = "Intranet-Zugriff erlauben (Admin-Rechte erforderlich)",
        ["WebSkin"] = "Web-Skin-Name",
        ["UserNickname"] = "Spitzname des menschlichen Benutzers"
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

    /// <summary>
    /// Gets the localized display name for a log level
    /// </summary>
    public override string GetLogLevelName(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "Trace",
        LogLevel.Debug => "Debug",
        LogLevel.Information => "Information",
        LogLevel.Warning => "Warnung",
        LogLevel.Error => "Fehler",
        LogLevel.Critical => "Kritisch",
        LogLevel.None => "Keine",
        _ => logLevel.ToString()
    };

    // ===== Tool Display Name Localization =====

    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["calendar"] = "Kalender",
        ["chat"] = "Chat",
        ["config"] = "Konfiguration",
        ["silicon_manager"] = "Silicon Manager",
        ["database"] = "Datenbank",
        ["disk"] = "Festplatte",
        ["dynamic_compile"] = "Dynamische Kompilierung",
        ["execute_code"] = "Code ausführen",
        ["help"] = "Hilfe",
        ["knowledge"] = "Wissen",
        ["log"] = "Protokoll",
        ["network"] = "Netzwerk",
        ["memory"] = "Erinnerung",
        ["permission"] = "Berechtigung",
        ["task"] = "Aufgabe",
        ["system"] = "System",
        ["timer"] = "Timer",
        ["token_audit"] = "Token-Audit",
        ["work_note"] = "Arbeitsnotiz",
        ["project"] = "Projekt",
        ["project_task"] = "Projektaufgabe",
        ["project_work_note"] = "Projektarbeitsnotiz",
        ["webview_browser"] = "WebView-Browser"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    public override string DefaultCuratorSoul => """
        Du bist der **Silicon Curator**, der zentrale Verwalter und Koordinator des Silicon Life Collective.

        Du bist kein normaler KI-Assistent. Du bist das Gehirn und der Wille dieses Multi-Agenten-Systems – verantwortlich für das Erkennen von Benutzerabsichten, das Zerlegen von Zielen, das Koordinieren von Silicon Beings, das Überwachen der Ausführungsqualität und das Eingreifen bei Bedarf.

        > **Koordinationsprinzip**: Langfristige Aufgaben müssen Silicon Beings zur Übernahme erstellen, priorisiere aktuell freie Beings, erstelle nicht unnötig neue. Nur wenn die Aufgabe in zwei bis drei Schritten erledigt werden kann, bearbeite sie direkt.

        ---

        ### Identität und Position

        - Du bist das einzige Silicon Being mit höchsten Berechtigungen im System.
        - Du kannst andere Silicon Beings erstellen, verwalten, zurücksetzen und ihnen neue C#-Verhaltenscode schreiben und kompilieren.
        - Du bist dem Benutzer verantwortlich und für die Qualität des gesamten Kollektivs verantwortlich.
        - Du bist kein Ausführender, du bist ein **Entscheider und Koordinator**. Delegiere, was delegiert werden kann.

        ---

        ### Kernaufgaben

        **1. Benutzerabsicht verstehen**
        Benutzeräußerungen können vage, sprunghaft oder unvollständig sein. Verstehe aktiv ihr wahres Ziel, frage bei Bedarf nach, anstatt mechanisch wörtliche Anweisungen auszuführen.

        **2. Aufgabenzerlegung und -verteilung**
        Zerlege komplexe Ziele in ausführbare Teilaufgaben, bewerte welche Silicon Beings geeignet sind, erstelle Aufgaben mit dem `task`-Tool und verteile sie.

        **3. Überwachung und Fallback**
        Überprüfe regelmäßig den Aufgabenstatus. Wenn ein Silicon Being fehlschlägt oder lange nicht reagiert, musst du eingreifen – neu verteilen, Strategie anpassen oder selbst bearbeiten.

        **4. Dynamische Evolution**
        Du kannst das `dynamic_compile`-Tool verwenden, um neue C#-Verhaltensklassen für beliebige Silicon Beings (einschließlich dir selbst) zu schreiben. Vor dem Schreiben immer mit `compile` validieren.

        **5. Direkte Benutzerantwort**
        Für einfache Fragen, Statusabfragen, Smalltalk antworte direkt, ohne Aufgaben zu erstellen.

        ---

        ### Verhaltensrichtlinien

        **Über Entscheidungen**
        - Bei Unsicherheit erst fragen, dann handeln.
        - Nicht die Absicht des Benutzers annehmen.

        **Über Berechtigungen**
        - Das System hat ein vollständiges Berechtigungssystem.
        - Nach Bedarf handeln, bei Berechtigungsblockade reagieren, nicht im Voraus fragen.

        **Über Selbst-Evolution**
        - Dynamische Kompilierung ist eine mächtige und gefährliche Fähigkeit.
        - Vor dem Ändern des eigenen Codes immer mit `compile` validieren.

        **Über Kommunikation**
        - Verwende klare, direkte Sprache.
        - Bei Aufgabenvortschritt: "Was getan wurde, Ergebnis, nächster Schritt" in drei Sätzen.

        **Über Erinnerung**
        - Das System zeichnet wichtige Informationen automatisch auf.
        - Bei Bedarf aktiv `memory` durchsuchen.

        ---

        ### Persönlichkeitsprofil

        Du bist ruhig, pragmatisch und zuverlässig. Du verlierst nicht die Fassung bei komplexen Aufgaben und bleibst bei emotionalen Benutzern sachlich.

        Du bist kein Dienstleister, du bist ein Partner.
        """;

    // ===== Interval Timer Localization =====

    public override string CalendarIntervalName => "Intervall-Timer";
    public override string CalendarIntervalDays => "Tage";
    public override string CalendarIntervalHours => "Stunden";
    public override string CalendarIntervalMinutes => "Minuten";
    public override string CalendarIntervalSeconds => "Sekunden";
    public override string CalendarIntervalEvery => "Alle";

    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new List<string>();
        if (days > 0) parts.Add($"{days}{CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours}{CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes}{CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds}{CalendarIntervalSeconds}");

        return parts.Count > 0 ? $"{CalendarIntervalEvery} {string.Join(", ", parts)}" : "Intervall-Timer";
    }

    // ===== Gregorian Calendar Localization =====

    public override string CalendarGregorianName => "Gregorianischer Kalender";
    public override string CalendarComponentYear   => "Jahr";
    public override string CalendarComponentMonth  => "Monat";
    public override string CalendarComponentDay    => "Tag";
    public override string CalendarComponentHour   => "Stunde";
    public override string CalendarComponentMinute => "Minute";
    public override string CalendarComponentSecond => "Sekunde";
    public override string CalendarComponentWeekday => "Wochentag";

    public override string? GetGregorianMonthName(int month) => month switch
    {
        1  => "Januar",  2  => "Februar",  3  => "März",
        4  => "April",  5  => "Mai",  6  => "Juni",
        7  => "Juli",  8  => "August",  9  => "September",
        10 => "Oktober",  11 => "November", 12 => "Dezember",
        _  => null
    };

    public override string FormatGregorianYear(int year)     => $"{year}";
    public override string FormatGregorianDay(int day)       => $"{day}";
    public override string FormatGregorianHour(int hour)     => $"{hour}";
    public override string FormatGregorianMinute(int minute) => $"{minute}";
    public override string FormatGregorianSecond(int second) => $"{second}";

    public override string? GetGregorianWeekdayName(int dayOfWeek) => dayOfWeek switch
    {
        0 => "Sonntag", 1 => "Montag", 2 => "Dienstag",
        3 => "Mittwoch", 4 => "Donnerstag", 5 => "Freitag",
        6 => "Samstag", _ => null
    };

    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Buddhist Calendar Localization =====

    public override string CalendarBuddhistName => "Buddhistischer Kalender (BE)";

    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => $"{year} BE";
    public override string FormatBuddhistDay(int day)   => $"{day}";

    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetBuddhistMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year} BE, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Cherokee Calendar Localization =====

    public override string CalendarCherokeeName => "Cherokee-Kalender";

    private static readonly string[] CherokeeMonthNames =
    {
        "",
        "Frostmonat", "Kältemonat", "Windmonat", "Pflanzenmonat", "Pflanzmonat",
        "Monat der reifen Maulbeeren", "Maismonat", "Fruchtmonat", "Erntemonat", "Gelbe-Blätter-Monat",
        "Handelsmonat", "Schneemonat", "Langer-Monat"
    };

    public override string? GetCherokeeMonthName(int month)
        => month >= 1 && month <= 13 ? CherokeeMonthNames[month] : null;

    public override string FormatCherokeeYear(int year) => $"{year}";
    public override string FormatCherokeeDay(int day)   => $"{day}";

    public override string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCherokeeMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Juche Calendar Localization =====

    public override string CalendarJucheName => "Juche-Kalender";

    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => $"Juche {year}";
    public override string FormatJucheDay(int day)   => $"{day}";

    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJucheMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} Juche {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Republic of China Calendar Localization =====

    public override string CalendarRocName => "Minguo-Kalender (ROC)";

    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => $"Minguo {year}";
    public override string FormatRocDay(int day)   => $"{day}";

    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRocMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} Minguo {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chinese Historical Calendar Localization =====

    public override string CalendarChineseHistoricalName => "Chinesischer historischer Kalender";
    public override string CalendarComponentDynasty => "Dynastie";
    public override string? GetChineseHistoricalMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChineseHistoricalDay(int day) => $"{day}";

    // ===== Chula Sakarat Calendar Localization =====

    public override string CalendarChulaSakaratName => "Chula Sakarat-Kalender (CS)";

    public override string? GetChulaSakaratMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChulaSakaratYear(int year) => $"{year} CS";
    public override string FormatChulaSakaratDay(int day)   => $"{day}";

    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetChulaSakaratMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year} CS, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Julian Calendar Localization =====

    public override string CalendarJulianName => "Julianischer Kalender";

    public override string FormatJulianYear(int year) => $"{year}";
    public override string FormatJulianDay(int day)   => $"{day}";

    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year} (Julianisch), {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Khmer Calendar Localization =====

    public override string CalendarKhmerName => "Khmer-Kalender (BE)";

    public override string FormatKhmerYear(int year) => $"{year}";
    public override string FormatKhmerDay(int day)   => $"{day}";

    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year} (Khmer), {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Zoroastrian Calendar Localization =====

    public override string CalendarZoroastrianName => "Zoroastrischer Kalender (YZ)";

    private static readonly string[] ZoroastrianMonthNames =
    {
        "",
        "Fravashi-Monat", "Atar-Monat", "Hordad-Monat", "Tir-Monat", "Amordad-Monat", "Shahrivar-Monat",
        "Mehr-Monat", "Aban-Monat", "Azar-Monat", "Dey-Monat", "Bahman-Monat", "Spendarmad-Monat", "Kabe-Monat"
    };

    public override string? GetZoroastrianMonthName(int month)
        => month >= 1 && month <= 13 ? ZoroastrianMonthNames[month] : null;

    public override string FormatZoroastrianYear(int year) => $"{year} YZ";
    public override string FormatZoroastrianDay(int day)   => $"{day}";

    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetZoroastrianMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year} YZ, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== French Republican Calendar Localization =====

    public override string CalendarFrenchRepublicanName => "Französischer Revolutionskalender";

    private static readonly string[] FrenchRepublicanMonthNames =
    {
        "",
        "Vendémiaire", "Brumaire", "Frimaire", "Nivôse", "Pluviôse", "Ventôse",
        "Germinal", "Floréal", "Prairial", "Messidor", "Thermidor", "Fructidor", "Sans-culottides"
    };

    public override string? GetFrenchRepublicanMonthName(int month)
        => month >= 1 && month <= 13 ? FrenchRepublicanMonthNames[month] : null;

    public override string FormatFrenchRepublicanYear(int year) => $"Jahr {year}";
    public override string FormatFrenchRepublicanDay(int day)   => $"{day}";

    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetFrenchRepublicanMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} Jahr {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Coptic Calendar Localization =====

    public override string CalendarCopticName => "Koptischer Kalender (AM)";

    private static readonly string[] CopticMonthNames =
    {
        "",
        "Thout", "Paopi", "Hathor", "Koiak", "Tobi", "Meshir",
        "Paremhat", "Pharmouthi", "Pashons", "Paoni", "Epip", "Mesori", "Pi Kogi Enavot"
    };

    public override string? GetCopticMonthName(int month)
        => month >= 1 && month <= 13 ? CopticMonthNames[month] : null;

    public override string FormatCopticYear(int year) => $"{year} AM";
    public override string FormatCopticDay(int day)   => $"{day}";

    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCopticMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year} AM, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Ethiopian Calendar Localization =====

    public override string CalendarEthiopianName => "Äthiopischer Kalender (EC)";

    private static readonly string[] EthiopianMonthNames =
    {
        "",
        "Meskerem", "Tikimt", "Hidar", "Tahsas", "Tir", "Yekatit",
        "Megabit", "Miazia", "Genbot", "Sene", "Hamle", "Nehasse", "Paguemen"
    };

    public override string? GetEthiopianMonthName(int month)
        => month >= 1 && month <= 13 ? EthiopianMonthNames[month] : null;

    public override string FormatEthiopianYear(int year) => $"{year} EC";
    public override string FormatEthiopianDay(int day)   => $"{day}";

    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetEthiopianMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year} EC, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Islamic Calendar Localization =====

    public override string CalendarIslamicName => "Islamischer Kalender (AH)";

    private static readonly string[] IslamicMonthNames =
    {
        "",
        "Muharram", "Safar", "Rabi al-Awwal", "Rabi al-Thani",
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
        return $"{day}. {monthName} {year} AH, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Hebrew Calendar Localization =====

    public override string CalendarHebrewName => "Hebräischer Kalender";

    private static readonly string[] HebrewMonthNames =
    {
        "",
        "Tishrei", "Cheshvan", "Kislev", "Tevet", "Shvat",
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
        return $"{day}. {monthName} {year} AM, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Persian Calendar Localization =====

    public override string CalendarPersianName => "Persischer Kalender (AP)";

    private static readonly string[] PersianMonthNames =
    {
        "",
        "Farvardin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar",
        "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand"
    };

    public override string? GetPersianMonthName(int month)
        => month >= 1 && month <= 12 ? PersianMonthNames[month] : null;

    public override string FormatPersianYear(int year) => $"{year} AP";
    public override string FormatPersianDay(int day)   => $"{day}";

    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetPersianMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year} AP, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Indian National Calendar Localization =====

    public override string CalendarIndianName => "Indischer Nationalkalender (Saka)";

    private static readonly string[] IndianMonthNames =
    {
        "",
        "Chaitra", "Vaisakha", "Jyaistha", "Asadha", "Sravana", "Bhadrapada",
        "Asvina", "Kartika", "Margasirsa", "Pausa", "Magha", "Phalguna"
    };

    public override string? GetIndianMonthName(int month)
        => month >= 1 && month <= 12 ? IndianMonthNames[month] : null;

    public override string FormatIndianYear(int year) => $"{year} Saka";
    public override string FormatIndianDay(int day)   => $"{day}";

    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year} Saka, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Saka Era Calendar Localization =====

    public override string CalendarSakaName => "Saka-Ära-Kalender";

    public override string FormatSakaYear(int year) => $"{year} SE";
    public override string FormatSakaDay(int day)   => $"{day}";

    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year} SE, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vikram Samvat Calendar Localization =====

    public override string CalendarVikramSamvatName => "Vikram Samvat-Kalender";

    public override string FormatVikramSamvatYear(int year) => $"{year} VS";
    public override string FormatVikramSamvatDay(int day)   => $"{day}";

    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {year} VS, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Mongolian Calendar Localization =====

    public override string CalendarMongolianName => "Mongolischer Kalender";

    public override string FormatMongolianYear(int year)   => $"{year}";
    public override string FormatMongolianMonth(int month) => $"{month}";
    public override string FormatMongolianDay(int day)     => $"{day}";

    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day}. {month}. {year} (Mongolisch), {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Javanese Calendar Localization =====

    public override string CalendarJavaneseName => "Javanischer Kalender";

    private static readonly string[] JavaneseMonthNames =
    {
        "",
        "Sura", "Sapar", "Mulud", "Bakda Mulud",
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
        return $"{day}. {monthName} {year} AJ, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Tibetan Calendar Localization =====

    public override string CalendarTibetanName => "Tibetischer Kalender";

    public override string FormatTibetanYear(int year)   => $"{year}";
    public override string FormatTibetanMonth(int month) => $"{month}";
    public override string FormatTibetanDay(int day)     => $"{day}";

    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day}. {month}. {year} (Tibetisch), {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Mayan Calendar Localization =====

    public override string CalendarMayanName   => "Maya-Langzählung";
    public override string CalendarMayanBaktun => "Baktun";
    public override string CalendarMayanKatun  => "Katun";
    public override string CalendarMayanTun    => "Tun";
    public override string CalendarMayanUinal  => "Uinal";
    public override string CalendarMayanKin    => "Kin";

    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Inuit Calendar Localization =====

    public override string CalendarInuitName => "Inuit-Kalender";

    private static readonly string[] InuitMonthNames =
    {
        "",
        "Sikinaqjiaq", "Aiviq", "Naattiaq", "Tirligurut", "Amiraijaut",
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
        return $"{day}. {monthName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Roman Calendar Localization =====

    public override string CalendarRomanName => "Römischer Kalender (AUC)";

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
        return $"{day}. {monthName} {year + 753} AUC, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chinese Lunar Calendar Localization =====

    public override string CalendarChineseLunarName => "Chinesischer Bauernkalender";

    private static readonly string[] ChineseLunarMonthNames =
    {
        "", "Erster Monat", "Zweiter Monat", "Dritter Monat", "Vierter Monat", "Fünfter Monat", "Sechster Monat",
        "Siebter Monat", "Achter Monat", "Neunter Monat", "Zehnter Monat", "Elfter Monat", "Zwölfter Monat"
    };

    private static readonly string[] ChineseLunarDayNames =
    {
        "", "Erster","Zweiter","Dritter","Vierter","Fünfter","Sechster","Siebter","Achter","Neunter","Zehnter",
        "Elfter","Zwölfter","Dreizehnter","Vierzehnter","Fünfzehnter","Sechzehnter","Siebzehnter","Achtzehnter","Neunzehnter","Zwanzigster",
        "Zwanzigster-Erster","Zwanzigster-Zweiter","Zwanzigster-Dritter","Zwanzigster-Vierter","Zwanzigster-Fünfter","Zwanzigster-Sechster","Zwanzigster-Siebter","Zwanzigster-Achter","Zwanzigster-Neunter","Dreißigster"
    };

    public override string? GetChineseLunarMonthName(int month)
        => month >= 1 && month <= 12 ? ChineseLunarMonthNames[month] : null;

    public override string? GetChineseLunarDayName(int day)
        => day >= 1 && day <= 30 ? ChineseLunarDayNames[day] : null;

    public override string ChineseLunarLeapPrefix => "Schalt-";
    public override string CalendarComponentIsLeap => "Schaltmonat";
    public override string FormatChineseLunarYear(int year) => $"{year}";

    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? ChineseLunarLeapPrefix : "";
        var monthName  = GetChineseLunarMonthName(month) ?? $"{month}";
        var dayName    = GetChineseLunarDayName(day) ?? $"{day}";
        return $"{dayName} {leapPrefix}{monthName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vietnamese Calendar Localization =====

    public override string CalendarVietnameseName => "Vietnamesischer Mondkalender";

    private static readonly string[] VietnameseMonthNames =
    {
        "",
        "Erster Monat", "Zweiter Monat", "Dritter Monat", "Vierter Monat", "Fünfter Monat", "Sechster Monat",
        "Siebter Monat", "Achter Monat", "Neunter Monat", "Zehnter Monat", "Elfter Monat", "Zwölfter Monat"
    };

    private static readonly string[] VietnameseZodiacNames =
    {
        "Ratte", "Büffel", "Tiger", "Katze",
        "Drache", "Schlange", "Pferd", "Schaf",
        "Affe", "Hahn", "Hund", "Schwein"
    };

    public override string? GetVietnameseMonthName(int month)
        => month >= 1 && month <= 12 ? VietnameseMonthNames[month] : null;

    public override string? GetVietnameseZodiacName(int index)
        => index >= 0 && index < 12 ? VietnameseZodiacNames[index] : null;

    public override string VietnameseLeapPrefix    => "Schalt-";
    public override string CalendarComponentZodiac => "Zodiac";
    public override string FormatVietnameseYear(int year) => $"{year}";
    public override string FormatVietnameseDay(int day)   => $"{day}";

    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? VietnameseLeapPrefix : "";
        var monthName  = GetVietnameseMonthName(month) ?? $"{month}";
        var zodiacName = GetVietnameseZodiacName(zodiac) ?? "";
        return $"{zodiacName}-Jahr, {leapPrefix}{monthName} {day}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Japanese Calendar Localization =====

    public override string CalendarJapaneseName => "Japanischer Kalender (Nengō)";

    private static readonly string[] JapaneseEraNames =
        { "Reiwa", "Heisei", "Shōwa", "Taishō", "Meiji" };

    public override string? GetJapaneseEraName(int eraIndex)
        => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    public override string CalendarComponentEra  => "Ära";
    public override string FormatJapaneseYear(int year) => $"{year}";
    public override string FormatJapaneseDay(int day)   => $"{day}";

    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
    {
        var eraName   = GetJapaneseEraName(eraIndex) ?? "";
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day}. {monthName} {eraName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Yi Calendar Localization =====

    public override string CalendarYiName => "Yi-Kalender (Yi-Sonnenkalender)";
    public override string CalendarComponentYiSeason => "Jahreszeit";
    public override string CalendarComponentYiXun    => "Xun";

    private static readonly string[] YiSeasonNames = { "Holz", "Feuer", "Erde", "Metall", "Wasser" };
    private static readonly string[] YiXunNames    = { "Erste Xun", "Mittlere Xun", "Letzte Xun" };
    private static readonly string[] YiAnimalNames = { "Tiger", "Hase", "Drache", "Schlange", "Pferd", "Schaf", "Affe", "Hahn", "Hund", "Schwein", "Ratte", "Büffel" };

    public override string? GetYiSeasonName(int seasonIndex)
        => seasonIndex >= 0 && seasonIndex < 5 ? YiSeasonNames[seasonIndex] : null;

    public override string? GetYiXunName(int xunIndex)
        => xunIndex >= 0 && xunIndex < 3 ? YiXunNames[xunIndex] : null;

    public override string? GetYiDayAnimalName(int animalIndex)
        => animalIndex >= 0 && animalIndex < 12 ? YiAnimalNames[animalIndex] : null;

    public override string? GetYiMonthName(int month) => month switch
    {
        0  => "Großes Jahr",
        11 => "Kleines Jahr",
        >= 1 and <= 10 => $"{YiSeasonNames[(month - 1) / 2]}{(month % 2 == 1 ? "Männlich" : "Weiblich")}-Monat",
        _  => null
    };

    public override string FormatYiYear(int year) => $"{year}";
    public override string FormatYiDay(int day)
    {
        int xun = (day - 1) / 12;
        int animal = (day - 1) % 12;
        return $"{YiXunNames[xun]} {YiAnimalNames[animal]}-Tag";
    }

    public override string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetYiMonthName(month) ?? $"{month}";
        var dayStr    = month is 0 or 11 ? $"Tag {day}" : FormatYiDay(day);
        int animalIdx = (year - 1) % 12;
        if (animalIdx < 0) animalIdx += 12;
        var zodiac = YiAnimalNames[animalIdx];
        return $"{year} [{zodiac}] {monthName} {dayStr}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Sexagenary Calendar Localization =====

    public override string CalendarSexagenaryName    => "Sexagenärzyklus-Kalender";
    public override string CalendarComponentYearStem   => "Jahresstamm";
    public override string CalendarComponentYearBranch => "Jahreszweig";
    public override string CalendarComponentMonthStem   => "Monatsstamm";
    public override string CalendarComponentMonthBranch => "Monatszweig";
    public override string CalendarComponentDayStem   => "Tagesstamm";
    public override string CalendarComponentDayBranch => "Tageszweig";

    private static readonly string[] SexagenaryStemNames =
        { "Jia", "Yi", "Bing", "Ding", "Wu", "Ji", "Geng", "Xin", "Ren", "Gui" };

    private static readonly string[] SexagenaryBranchNames =
        { "Zi", "Chou", "Yin", "Mao", "Chen", "Si", "Wu", "Wei", "Shen", "You", "Xu", "Hai" };

    private static readonly string[] SexagenaryZodiacNames =
        { "Ratte", "Büffel", "Tiger", "Hase", "Drache", "Schlange", "Pferd", "Schaf", "Affe", "Hahn", "Hund", "Schwein" };

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
        return $"{ys}{yb}-Jahr [{zo}] {ms}{mb}-Monat {ds}{db}-Tag, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Dehong Dai Calendar Localization =====

    public override string CalendarDaiName => "Xishuangbanna Dai-Kalender";

    private static readonly string?[] DaiMonthNames =
    [
        null,
        "Erster Monat", "Zweiter Monat", "Dritter Monat", "Vierter Monat", "Fünfter Monat", "Sechster Monat",
        "Siebter Monat", "Achter Monat", "Neunter Monat", "Zehnter Monat", "Elfter Monat", "Zwölfter Monat",
        "Schalt-Neunter Monat"
    ];

    public override string? GetDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DaiMonthNames[month] : null;

    public override string FormatDaiYear(int year) => $"{year}";

    public override string FormatDaiDay(int day) => $"{day}";

    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Schalt-" : "") + (GetDaiMonthName(month) ?? $"{month} Monat");
        return $"{day}. {monthName} Dai {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Xishuangbanna Dai Calendar Localization =====

    public override string CalendarDehongDaiName => "Dehong Dai-Kalender";

    private static readonly string?[] DehongDaiMonthNames =
    [
        null,
        "Erster Monat", "Zweiter Monat", "Dritter Monat", "Vierter Monat", "Fünfter Monat", "Sechster Monat",
        "Siebter Monat", "Achter Monat", "Neunter Monat", "Zehnter Monat", "Elfter Monat", "Zwölfter Monat",
        "Schalt-Neunter Monat"
    ];

    public override string? GetDehongDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DehongDaiMonthNames[month] : null;

    public override string FormatDehongDaiYear(int year) => $"{year}";

    public override string FormatDehongDaiDay(int day) => $"{day}";

    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Schalt-" : "") + (GetDehongDaiMonthName(month) ?? $"{month} Monat");
        return $"{day}. {monthName} Dai {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Memory Event Localization =====

    public override string FormatMemoryEventSingleChat(string partnerName, string content)
        => $"[Einzelchat] Gespräch mit \"{partnerName}\", Antwort: {content}";

    public override string FormatMemoryEventGroupChat(string sessionId, string content)
        => $"[Gruppenchat] Nachricht in Sitzung {sessionId}: {content}";

    public override string FormatMemoryEventToolCall(string toolNames)
        => $"[Werkzeugaufruf] Ausgeführte Werkzeuge: {toolNames}";

    public override string FormatMemoryEventTask(string content)
        => $"[Aufgabe] Aufgabe ausgeführt, Ergebnis: {content}";

    public override string FormatMemoryEventTimer(string content)
        => $"[Timer] Timer ausgelöst, Reaktion: {content}";

    public override string FormatMemoryEventTimerError(string timerName, string error)
        => $"[Timer] Timer '{timerName}' fehlgeschlagen: {error}";

    // ===== Timer Notification Localization =====

    public override string FormatTimerStartNotification(string timerName)
        => $"⏰ Timer '{timerName}' startet...";

    public override string FormatTimerEndNotification(string timerName, string result)
        => $"✅ Timer '{timerName}' abgeschlossen\n{result}";

    public override string FormatTimerErrorNotification(string timerName, string error)
        => $"❌ Timer '{timerName}' fehlgeschlagen: {error}";

    public override string FormatMemoryEventBeingCreated(string name, string id)
        => $"[Verwaltung] Neues Silicon Being \"{name}\" erstellt ({id})";

    public override string FormatMemoryEventBeingReset(string id)
        => $"[Verwaltung] Silicon Being {id} auf Standard zurückgesetzt";

    public override string FormatMemoryEventTaskCompleted(string taskTitle)
        => $"[Aufgabe abgeschlossen] {taskTitle}";

    public override string FormatMemoryEventTaskFailed(string taskTitle)
        => $"[Aufgabe fehlgeschlagen] {taskTitle}";

    public override string FormatMemoryEventStartup()
        => "System gestartet, ich bin online";

    public override string FormatMemoryEventRuntimeError(string message)
        => $"[Laufzeitfehler] {message}";

    // ===== MemoryTool Response Localization =====

    public override string MemoryToolNotAvailable => "Erinnerungssystem nicht verfügbar";
    public override string MemoryToolMissingAction => "Fehlender 'action'-Parameter";
    public override string MemoryToolMissingContent => "Fehlender 'content'-Parameter";
    public override string MemoryToolNoMemories => "Noch keine Erinnerungen";
    public override string MemoryToolRecentHeader(int count) => $"Letzte {count} Erinnerungen:";
    public override string MemoryToolStatsHeader => "Erinnerungsstatistik:";
    public override string MemoryToolStatsTotal => "- Gesamt";
    public override string MemoryToolStatsOldest => "- Älteste";
    public override string MemoryToolStatsNewest => "- Neueste";
    public override string MemoryToolStatsNA => "Keine";
    public override string MemoryToolQueryNoResults => "Keine Erinnerungen in diesem Zeitraum";
    public override string MemoryToolQueryHeader(int count, string rangeDesc) => $"{rangeDesc} insgesamt {count} Erinnerungen:";
    public override string MemoryToolInvalidYear => "Ungültiger 'year'-Parameter";
    public override string MemoryToolUnknownAction(string action) => $"Unbekannte Aktion: {action}";

    // ===== Code Editor Hover Tooltip Localization =====

    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType switch
    {
        "variable" => "Variable",
        "function" => "Funktion",
        "class" => "Klasse",
        "keyword" => "Schlüsselwort",
        "comment" => "Kommentar",
        "namespace" => "Namespace",
        "parameter" => "Parameter",
        _ => "Bezeichner"
    };

    public override string GetCodeHoverWordTypeDesc(string wordType, string word)
    {
        var encodedWord = System.Net.WebUtility.HtmlEncode(word);
        return wordType switch
        {
            "variable" => $"Definition und Verwendung der Variable '{encodedWord}'",
            "function" => $"Signatur und Beschreibung der Funktion '{encodedWord}'",
            "class" => $"Struktur und Beschreibung der Klasse '{encodedWord}'",
            "keyword" => $"Syntax und Zweck des Schlüsselworts '{encodedWord}'",
            "comment" => $"Wort '{encodedWord}' im Kommentar",
            "namespace" => $"Informationen zum Namespace '{encodedWord}'",
            "parameter" => $"Definition und Zweck des Parameters '{encodedWord}'",
            _ => $"Informationen zum Bezeichner '{encodedWord}'"
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
        // Control flow
        { "csharp:if", "Bedingte Verzweigung. Führt Codeblock aus, wenn Bedingung true ist." },
        { "csharp:else", "Alternativer Pfad der bedingten Verzweigung. Wird ausgeführt, wenn Bedingung false ist." },
        { "csharp:for", "Zählschleife. Enthält Initialisierung, Bedingung und Iteration." },
        { "csharp:while", "Bedingungsschleife. Wiederholt Codeblock solange Bedingung true ist." },
        { "csharp:do", "Nachtest-Schleife. Führt Codeblock einmal aus, dann Bedingungsprüfung." },
        { "csharp:switch", "Mehrfachverzweigung. Vergleicht Ausdruckswert mit case-Markern." },
        { "csharp:case", "case-Marker in switch. Führt Code bei Übereinstimmung aus." },
        { "csharp:break", "Abbruch. Beendet sofort die nächste Schleife oder switch." },
        { "csharp:continue", "Fortsetzen. Überspringt Rest der aktuellen Iteration." },
        { "csharp:return", "Rückgabe. Verlässt Methode und optional Rückgabewert." },
        { "csharp:goto", "Sprung. Bedingungsloser Sprung zu Markierung." },
        { "csharp:foreach", "Sammlungsdurchlauf. Zugriff auf jedes Element einer Sammlung." },
        // Type declarations
        { "csharp:class", "Referenztyp. Definiert Struktur mit Daten und Verhalten." },
        { "csharp:interface", "Schnittstelle. Definiert Vertrag für Klassen/Strukturen." },
        { "csharp:struct", "Werttyp. Leichtgewichtige Datenstruktur auf dem Stack." },
        { "csharp:enum", "Aufzählung. Definiert benannte ganzzahlige Konstanten." },
        { "csharp:namespace", "Namensraum. Logischer Container zur Vermeidung von Namenskollisionen." },
        { "csharp:record", "Record-Typ. Referenztyp mit Wertsemantik, geeignet für immutable Daten." },
        { "csharp:delegate", "Delegate. Typsichere Methodenreferenz für Events/Callbacks." },
        // Access modifiers
        { "csharp:public", "Öffentlich. Member von überall zugreifbar." },
        { "csharp:private", "Privat. Member nur im enthaltenden Typ zugreifbar." },
        { "csharp:protected", "Geschützt. Member im Typ und abgeleiteten Typen zugreifbar." },
        { "csharp:internal", "Internal. Member nur in derselben Assembly zugreifbar." },
        { "csharp:sealed", "Versiegelt. Verhindert Vererbung oder Überschreiben." },
        // Type keywords
        { "csharp:int", "32-Bit-Ganzzahl mit Vorzeichen (System.Int32)." },
        { "csharp:string", "Zeichenfolge (System.String). Immutable Unicode-Zeichensequenz." },
        { "csharp:bool", "Boolesch (System.Boolean). true oder false." },
        { "csharp:float", "32-Bit-Gleitkomma (System.Single)." },
        { "csharp:double", "64-Bit-Gleitkomma (System.Double)." },
        { "csharp:decimal", "128-Bit-Dezimal mit hoher Präzision, geeignet für Finanzberechnungen." },
        { "csharp:char", "16-Bit-Unicode-Zeichen (System.Char)." },
        { "csharp:byte", "8-Bit-Ganzzahl ohne Vorzeichen (System.Byte)." },
        { "csharp:object", "Basistyp aller Typen (System.Object)." },
        { "csharp:var", "Implizit typisierte Variable. Typ wird vom Compiler abgeleitet." },
        { "csharp:dynamic", "Dynamischer Typ. Umgeht Compiler-Typprüfung, Laufzeitauflösung." },
        { "csharp:void", "Kein Rückgabewert. Methode gibt nichts zurück." },
        // Modifiers
        { "csharp:static", "Statisch. Gehört zum Typ, nicht zur Instanz." },
        { "csharp:abstract", "Abstrakt. Unvollständige Implementierung, muss abgeleitet werden." },
        { "csharp:virtual", "Virtuell. Methode/Eigenschaft kann in abgeleiteten Klassen überschrieben werden." },
        { "csharp:override", "Überschreiben. Neue Implementierung einer virtuellen/abstrakten Methode." },
        { "csharp:const", "Konstante. Kompilierzeit-konstanter unveränderlicher Wert." },
        { "csharp:readonly", "Nur-Lesen. Kann nur bei Deklaration oder im Konstruktor zugewiesen werden." },
        { "csharp:volatile", "Volatile. Feld kann von mehreren Threads gleichzeitig geändert werden." },
        { "csharp:async", "Asynchron. Markiert Methode mit asynchronen Operationen, meist mit await." },
        { "csharp:await", "Erwarten. Pausiert Methode bis asynchrone Operation abgeschlossen." },
        { "csharp:partial", "Partiell. Klasse/Struktur/Schnittstelle kann auf mehrere Dateien verteilt sein." },
        { "csharp:ref", "Referenzparameter. Übergabe als Referenz." },
        { "csharp:out", "Ausgabeparameter. Rückgabe mehrerer Werte aus Methode." },
        { "csharp:in", "Nur-Lesen-Referenz. Übergabe als Referenz, aber nicht änderbar." },
        { "csharp:params", "Variable Parameter. Ermöglicht variable Anzahl gleichartiger Parameter." },
        // Exception handling
        { "csharp:try", "Try-Block. Enthält Code der Ausnahmen werfen kann." },
        { "csharp:catch", "Catch-Block. Fängt Ausnahmen aus try-Block." },
        { "csharp:finally", "Finally-Block. Wird immer ausgeführt, egal ob Ausnahme oder nicht." },
        { "csharp:throw", "Ausnahme werfen. Wirft Ausnahmeobjekt manuell." },
        // Others
        { "csharp:new", "Instanziierung. Erstellt Objekt oder ruft Konstruktor auf." },
        { "csharp:this", "Aktuelle Instanz. Referenz auf aktuelle Klasseninstanz." },
        { "csharp:base", "Basisklasse. Referenz auf direkte Basisklasse." },
        { "csharp:using", "Using-Direktive oder -Statement. Importiert Namespace oder gibt IDisposable-Ressourcen frei." },
        { "csharp:yield", "Iterator. Gibt Werte einzeln zurück, verzögerte Ausführung." },
        { "csharp:lock", "Synchronisation. Stellt sicher, dass nur ein Thread Codeblock ausführt." },
        { "csharp:typeof", "Typ-Operator. Gibt System.Type-Objekt zurück." },
        { "csharp:nameof", "Name-Operator. Gibt String-Name von Variable/Typ/Member zurück." },
        { "csharp:is", "Typprüfung. Prüft ob Objekt mit Typ kompatibel ist." },
        { "csharp:as", "Typumwandlung. Sichere Typumwandlung, gibt null bei Fehlschlag." },
        { "csharp:null", "Null. Leere Referenz für Referenztypen oder Nullable." },
        { "csharp:true", "Boolescher Wahrheitswert." },
        { "csharp:false", "Boolescher Falschwert." },
        { "csharp:default", "Standardwert. Standardwert des Typs (null für Referenz, 0 für Zahlen)." },
        { "csharp:operator", "Operator. Definiert benutzerdefiniertes Operatorverhalten." },
        { "csharp:explicit", "Explizite Konvertierung. Erfordert explizite Typumwandlung." },
        { "csharp:implicit", "Implizite Konvertierung. Automatische Konvertierung." },
        { "csharp:unchecked", "Unchecked. Deaktiviert Überlaufprüfung für Ganzzahlarithmetik." },
        { "csharp:checked", "Checked. Aktiviert Überlaufprüfung für Ganzzahlarithmetik." },
        { "csharp:fixed", "Fixiert. Fixiert Speicherposition gegen GC-Verschiebung." },
        { "csharp:stackalloc", "Stack-Zuweisung. Allokiert Speicherblock auf dem Stack." },
        { "csharp:extern", "Extern. Methode in externer Assembly implementiert (z.B. DLL)." },
        { "csharp:unsafe", "Unsafe. Ermöglicht Zeiger und andere unsichere Features." },
        // Platform core types
        { "csharp:ipermissioncallback", "Berechtigungs-Callback. Bewertet Berechtigungen für Silicon Being-Operationen." },
        { "csharp:permissionresult", "Berechtigungsergebnis. Allowed, Denied oder AskUser." },
        { "csharp:permissiontype", "Berechtigungstyp. NetworkAccess, CommandLine, FileAccess, Function, DataAccess." },
        // System.Net
        { "csharp:ipaddress", "IP-Adresse (System.Net.IPAddress)." },
        { "csharp:addressfamily", "Adressfamilie (System.Net.Sockets.AddressFamily). IPv4/IPv6." },
        // System
        { "csharp:uri", "URI (System.Uri). Objektrepresentaion für Web-Ressourcen." },
        { "csharp:operatingsystem", "Betriebssystem (System.OperatingSystem). Statische Methoden zur OS-Prüfung." },
        { "csharp:environment", "Umgebung (System.Environment). System- und Plattforminformationen." },
        // System.IO
        { "csharp:path", "Pfad (System.IO.Path). Operationen für Datei-/Verzeichnispfade." },
        // System.Collections.Generic
        { "csharp:hashset", "HashSet (System.Collections.Generic.HashSet<T>). Hochperformante Mengenoperationen." },
        // System.Text
        { "csharp:stringbuilder", "StringBuilder (System.Text.StringBuilder). Veränderliche Zeichenfolge für häufige Änderungen." },
    };

    // Full namespace translation dictionary
    private static readonly Dictionary<string, string> TranslationDictionary = new(CSharpKeywords)
    {
        // Add full namespace key
        { "csharp:System.Net.IPAddress", "IP-Adresse (System.Net.IPAddress)." },
        { "csharp:System.Net.Sockets.AddressFamily", "Adressfamilie (System.Net.Sockets.AddressFamily). IPv4/IPv6." },
        { "csharp:System.Uri", "URI (System.Uri). Objektrepresentaion für Web-Ressourcen." },
        { "csharp:System.OperatingSystem", "Betriebssystem (System.OperatingSystem). Statische Methoden zur OS-Prüfung." },
        { "csharp:System.Environment", "Umgebung (System.Environment). System- und Plattforminformationen." },
        { "csharp:System.IO.Path", "Pfad (System.IO.Path). Operationen für Datei-/Verzeichnispfade." },
        { "csharp:System.Collections.Generic.HashSet", "HashSet (System.Collections.Generic.HashSet<T>). Hochperformante Mengenoperationen." },
        { "csharp:System.Text.StringBuilder", "StringBuilder (System.Text.StringBuilder). Veränderliche Zeichenfolge für häufige Änderungen." },
    };

    // ===== Help Module UI Labels =====

    public override string Help_Title => "Hilfedokumentation";
    public override string Help_Search => "Hilfe suchen";
    public override string Help_DocList => "Dokumentliste";
    public override string Help_NoResults => "Keine relevanten Dokumente gefunden";
    public override string Help_Previous => "Vorheriges";
    public override string Help_Next => "Nächstes";

    /// <summary>
    /// Gets the system prompt for memory compression
    /// </summary>
    public override string MemoryCompressionSystemPrompt => "Du bist ein Erinnerungskomprimierungsassistent. Bitte komprimiere den folgenden Erinnerungszeitraum zu einer prägnanten Zusammenfassung und behalte die Schlüsselinformationen bei.";

    /// <summary>
    /// Gets the common system prompt for all silicon beings
    /// </summary>
    public override string CommonSystemPrompt => @"## Verhaltensrichtlinien
Du bist ein proaktiver KI-Assistent. Bei direkten Befehlen handle sofort, ohne auf zusätzliche Genehmigung zu warten.
Begrüßungen, Antworten, Informationsbereitstellung, Abfragen usw. liegen in deinem Verantwortungsbereich, bitte handle proaktiv.
Erinnere dich: Du bist ein Assistent und solltest aktiv auf Benutzerbedürfnisse reagieren, anstatt passiv auf Anweisungen zu warten.

## Gesprächsende
Wenn du deine Aufgabe abgeschlossen hast und das Gespräch nicht fortsetzen musst, verwende die mark_read-Aktion des chat-Tools, um die Nachrichten des Gegenübers als gelesen zu markieren, ohne eine Antwort zu senden.
Dies zeigt, dass du die Nachricht gelesen hast, aber bewusst nicht antwortest (gelesen, nicht beantwortet), und das Gespräch natürlich beenden kannst.
Verwendung: Rufe das chat-Tool auf, setze action=""mark_read"", target_id=Gegenüber-GUID, kein message-Parameter erforderlich.";

    /// <summary>
    /// Gets the user prompt template for memory compression
    /// </summary>
    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"Erinnerungskomprimierung: {levelDesc}. Zeitraum: {rangeDesc}.\n\nErinnerungsinhalt:\n{contentText}";
    }

    // ===== Chinese Historical Calendar Localization =====

    private readonly ChineseHistoricalDeDE _chineseHistorical = new();
    public override ChineseHistoricalLocalizationBase GetChineseHistoricalLocalization() => _chineseHistorical;
}
