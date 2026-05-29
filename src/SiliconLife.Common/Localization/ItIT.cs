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
/// Italian (Italy) localization implementation
/// </summary>
public class ItIT : DefaultLocalizationBase
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "it-IT";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "Italiano (Italia)";

    /// <summary>
    /// Gets the welcome message
    /// </summary>
    public override string WelcomeMessage => "Benvenuto nel Silicon Life Collective!";

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
    public override string ShutdownMessage => "Arresto in corso...";

    /// <summary>
    /// Gets the config corrupted error message
    /// </summary>
    public override string ConfigCorruptedError => "File di configurazione corrotto, utilizzo della configurazione predefinita";

    /// <summary>
    /// Gets the config created message
    /// </summary>
    public override string ConfigCreatedWithDefaults => "File di configurazione non trovato, configurazione predefinita creata";

    /// <summary>
    /// Gets the AI connection error message
    /// </summary>
    public override string AIConnectionError => "Impossibile connettersi al servizio AI, verifica che Ollama sia in esecuzione";

    /// <summary>
    /// Gets the AI request error message
    /// </summary>
    public override string AIRequestError => "Richiesta AI fallita";

    /// <summary>
    /// Gets the data directory create error message
    /// </summary>
    public override string DataDirectoryCreateError => "Impossibile creare la directory dei dati";

    /// <summary>
    /// Gets the thinking message
    /// </summary>
    public override string ThinkingMessage => "Sto pensando...";

    /// <summary>
    /// Gets the tool call message
    /// </summary>
    public override string ToolCallMessage => "Esecuzione degli strumenti...";

    /// <summary>
    /// Gets the error message
    /// </summary>
    public override string ErrorMessage => "Errore";

    /// <summary>
    /// Gets the unexpected error message
    /// </summary>
    public override string UnexpectedErrorMessage => "Errore imprevisto";

    /// <summary>
    /// Gets the permission denied message
    /// </summary>
    public override string PermissionDeniedMessage => "Permesso negato";

    /// <summary>
    /// Gets the permission ask prompt
    /// </summary>
    public override string PermissionAskPrompt => "Autorizzare? (s/n): ";

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public override string PermissionRequestHeader => "[Richiesta di autorizzazione]";
    public override string PermissionRequestDescription => "Un Silicon Being richiede la tua autorizzazione:";
    public override string PermissionRequestTypeLabel => "Tipo di autorizzazione:";
    public override string PermissionRequestResourceLabel => "Risorsa richiesta:";
    public override string PermissionRequestAllowButton => "Consenti";
    public override string PermissionRequestDenyButton => "Nega";
    public override string PermissionRequestCacheLabel => "Memorizza questa decisione";
    public override string PermissionRequestDurationLabel => "Durata della cache";
    public override string PermissionRequestWaitingMessage => "In attesa di risposta...";

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public override string AllowCodeLabel => "Codice di autorizzazione";

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public override string DenyCodeLabel => "Codice di rifiuto";

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public override string PermissionReplyInstruction => "Inserisci il codice di conferma o qualsiasi altro testo per rifiutare";

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public override string AddToCachePrompt => "Memorizzare questa decisione? (s/n): ";

    /// <summary>
    /// Gets the label for the permission cache checkbox in the web UI
    /// </summary>
    public override string PermissionCacheLabel => "Memorizza questa decisione";

    /// <summary>
    /// Gets the label for the cache duration selector in the permission dialog
    /// </summary>
    public override string PermissionCacheDurationLabel => "Durata della cache";

    /// <summary>
    /// Gets the option text for 1-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration1Hour => "1 ora";

    /// <summary>
    /// Gets the option text for 24-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration24Hours => "24 ore";

    /// <summary>
    /// Gets the option text for 7-day cache duration
    /// </summary>
    public override string PermissionCacheDuration7Days => "7 giorni";

    /// <summary>
    /// Gets the option text for 30-day cache duration
    /// </summary>
    public override string PermissionCacheDuration30Days => "30 giorni";

    /// <summary>
    /// Gets the prefix for project group chat session names
    /// </summary>
    public override string ProjectGroupChatPrefix => "Gruppo di progetto";

    /// <summary>
    /// Gets the prefix for project broadcast channel names
    /// </summary>
    public override string ProjectBroadcastPrefix => "Trasmissione di progetto";

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "Accesso alla rete",
        PermissionType.CommandLine => "Esecuzione riga di comando",
        PermissionType.FileAccess => "Accesso ai file",
        PermissionType.Function => "Chiamata di funzione",
PermissionType.DataAccess => "Accesso ai dati",
PermissionType.ToolAction => "Azione strumento",
_ => permissionType.ToString()
    };

    /// <summary>
    /// Gets the title text for the permission dialog in the web UI
    /// </summary>
    public override string PermissionDialogTitle => "Richiesta di autorizzazione";

    /// <summary>
    /// Gets the label for the permission type field in the permission dialog
    /// </summary>
    public override string PermissionTypeLabel => "Tipo di autorizzazione:";

    /// <summary>
    /// Gets the label for the requested resource field in the permission dialog
    /// </summary>
    public override string PermissionResourceLabel => "Risorsa richiesta:";

    /// <summary>
    /// Gets the label for the detail information field in the permission dialog
    /// </summary>
    public override string PermissionDetailLabel => "Informazioni dettagliate:";

    /// <summary>
    /// Gets the text for the allow button in the permission dialog
    /// </summary>
    public override string PermissionAllowButton => "Consenti";

    /// <summary>
    /// Gets the text for the deny button in the permission dialog
    /// </summary>
    public override string PermissionDenyButton => "Nega";

    /// <summary>
    /// Gets the console error message when permission respond fails
    /// </summary>
    public override string PermissionRespondFailed => "Risposta di autorizzazione fallita";

    /// <summary>
    /// Gets the console error prefix when permission respond throws an error
    /// </summary>
    public override string PermissionRespondError => "Errore di risposta di autorizzazione: ";

    // ===== Init Page Localization =====

    public override string InitPageTitle => "Inizializzazione";
    public override string InitDescription => "Primo utilizzo, completa la configurazione di base";
    public override string InitNicknameLabel => "Nome utente";
    public override string InitNicknamePlaceholder => "Inserisci il tuo nome utente";
    public override string InitEndpointLabel => "Endpoint API AI";
    public override string InitEndpointPlaceholder => "es: http://localhost:11434";
    public override string InitAIClientTypeLabel => "Tipo di client AI";
    public override string InitModelLabel => "Modello predefinito";
    public override string InitModelPlaceholder => "es: qwen3.5:cloud";
    public override string InitSkinLabel => "Tema";
    public override string InitSkinPlaceholder => "Lascia vuoto per il tema predefinito";
    public override string InitDataDirectoryLabel => "Directory dei dati";
    public override string InitDataDirectoryPlaceholder => "es: ./data";
    public override string InitDataDirectoryBrowse => "Sfoglia...";
    public override string InitSkinSelected => "\u2713 Selezionato";
    public override string InitSkinPreviewTitle => "Anteprima";
    public override string InitSkinPreviewCardTitle => "Titolo della scheda";
    public override string InitSkinPreviewCardContent => "Questo è un esempio di scheda che mostra l'effetto visivo di questo tema.";
    public override string InitSkinPreviewPrimaryBtn => "Pulsante principale";
    public override string InitSkinPreviewSecondaryBtn => "Pulsante secondario";
    public override string InitSubmitButton => "Completa inizializzazione";
    public override string InitFooterHint => "La configurazione può essere modificata in qualsiasi momento nelle impostazioni";
    public override string InitHelpLink => "📖 Visualizza la documentazione di aiuto";
    public override string InitAIClientHelpPrefix => "📖 Visualizza aiuto: ";
    public override string InitNicknameRequiredError => "Inserisci un nome utente";
    public override string InitDataDirectoryRequiredError => "Seleziona una directory dei dati";
    public override string InitCuratorNameLabel => "Nome del Silicon Being";
    public override string InitCuratorNamePlaceholder => "Inserisci il nome del primo Silicon Being";
    public override string InitCuratorNameRequiredError => "Inserisci un nome per il Silicon Being";
    public override string InitLanguageLabel => "Lingua / Language";
    public override string InitLanguageSwitchBtn => "Applica";

    // ===== Navigation Menu Localization =====

    public override string NavMenuChat => "Chat";
    public override string NavMenuDashboard => "Pannello di controllo";
    public override string NavMenuBeings => "Silicon Beings";
    public override string NavMenuUsage => "Utilizzo";
    public override string NavMenuAudit => "Audit";
    public override string NavMenuTasks => "Attività";
    public override string NavMenuMemory => "Memoria";
    public override string NavMenuKnowledge => "Conoscenza";
    public override string NavMenuProjects => "Progetti";
    public override string NavMenuLogs => "Registri";
    public override string NavMenuConfig => "Configurazione";
    public override string NavMenuHelp => "Aiuto";
    public override string NavMenuAbout => "Informazioni";

    // ===== Page Title Localization =====

    public override string PageTitleChat => "Chat - Silicon Life Collective";
    public override string PageTitleDashboard => "Pannello di controllo - Silicon Life Collective";
    public override string PageTitleBeings => "Gestione Silicon Beings - Silicon Life Collective";
    public override string PageTitleTasks => "Gestione attività - Silicon Life Collective";
    public override string PageTitleTimers => "Gestione timer - Silicon Life Collective";
    public override string PageTitleMemory => "Ricerca in memoria - Silicon Life Collective";
    public override string PageTitleWorkNotes => "Note di lavoro - Silicon Life Collective";
    public override string PageTitleKnowledge => "Grafo della conoscenza - Silicon Life Collective";
    public override string PageTitleProjects => "Gestione spazi progetto - Silicon Life Collective";
    public override string PageTitleLogs => "Consultazione registri - Silicon Life Collective";
    public override string PageTitleUsage => "Utilizzo token - Silicon Life Collective";
    public override string PageTitleAudit => "Audit dei permessi - Silicon Life Collective";
    public override string PageTitleConfig => "Configurazione di sistema - Silicon Life Collective";
    public override string PageTitleExecutor => "Monitoraggio esecutori - Silicon Life Collective";
    public override string PageTitleCodeBrowser => "Navigatore di codice - Silicon Life Collective";
    public override string PageTitlePermission => "Gestione autorizzazioni - Silicon Life Collective";
    public override string PageTitleAbout => "Informazioni - Silicon Life Collective";

    // ===== Memory Page Localization =====

    public override string MemoryPageHeader => "Consultazione della memoria";
    public override string WorkNotesPageHeader => "Note di lavoro";
    public override string WorkNotesBackToPrevious => "← Indietro";
    public override string WorkNotesTotalPages => "Totale {0} pagine";
    public override string WorkNotesEmptyState => "Nessuna nota di lavoro al momento";
    public override string WorkNotesSearchPlaceholder => "Cerca note...";
    public override string WorkNotesSearchButton => "Cerca";
    public override string WorkNotesNoSearchResults => "Nessuna nota corrispondente trovata";
    public override string MemoryEmptyState => "Nessun dato in memoria al momento";
    public override string MemorySearchPlaceholder => "Cerca nella memoria...";
    public override string MemorySearchButton => "Cerca";
    public override string MemoryFilterAll => "Tutto";
    public override string MemoryFilterSummaryOnly => "Solo riepiloghi";
    public override string MemoryFilterOriginalOnly => "Solo originali";
    public override string MemoryStatTotal => "Totale memorie";
    public override string MemoryStatOldest => "Memoria più vecchia";
    public override string MemoryStatNewest => "Memoria più recente";
    public override string MemoryIsSummaryBadge => "Riepilogo compresso";
    public override string MemoryPaginationPrev => "Pagina precedente";
    public override string MemoryPaginationNext => "Pagina successiva";
    public override string MemoryFilterTypeLabel => "Tipo";
    public override string MemoryFilterDateFrom => "Data di inizio";
    public override string MemoryFilterDateTo => "Data di fine";
    public override string MemoryFilterApply => "Applica";
    public override string MemoryFilterReset => "Reimposta";
    public override string MemoryTypeChat => "Conversazione";
    public override string MemoryTypeToolCall => "Chiamata strumento";
    public override string MemoryTypeTask => "Attività";
    public override string MemoryTypeTimer => "Timer";
    public override string MemoryDetailTitle => "Dettagli memoria";
    public override string MemoryDetailClose => "Chiudi";
    public override string MemoryDetailId => "ID";
    public override string MemoryDetailContent => "Contenuto";
    public override string MemoryDetailCreatedAt => "Data di creazione";
    public override string MemoryDetailRelatedBeings => "Agenti correlati";
    public override string MemoryDetailKeywords => "Parole chiave";
    public override string MemoryStatTypeDistribution => "Distribuzione per tipo";
    public override string MemoryStatKeywordFrequency => "Frequenza delle parole chiave";
    public override string MemoryCardViewDetail => "Visualizza dettagli";
    public override string MemoryTimelineEmptyState => "Nessun dato in memoria";
    public override string MemoryYearSummaryLabel => "Riepilogo annuale";
    public override string MemoryMonthSummaryLabel => "Riepilogo mensile";
    public override string MemoryDaySummaryLabel => "Riepilogo giornaliero";
    public override string MemoryHourSummaryLabel => "Riepilogo orario";
    public override string MemoryMinuteSummaryLabel => "Riepilogo per minuto";
    public override string MemorySummaryBadge => "Riepilogo compresso";
    public override string MemoryTimelineYearFormat => "{0} ({1} voci)";
    public override string MemoryTimelineMonthFormat => "{0}/{1} ({2} voci)";
    public override string MemoryTimelineDayFormat => "{0}-{1}-{2} ({3} voci)";
    public override string MemoryTimelineHourFormat => "{0}:00 ({1} voci)";
    public override string MemoryTimelineMinuteFormat => "{0}:{1} ({2} voci)";
    public override string MemoryRelatedBeingsLabel => "Utenti correlati: {0} esseri";

    // ===== Projects Page Localization =====

    public override string ProjectsPageHeader => "Gestione spazi progetto";
    public override string ProjectsEmptyState => "Nessun progetto al momento";
    public override string ProjectsActiveLabel => "Attivo";
    public override string ProjectsArchivedLabel => "Archiviato";

    public override string ProjectStatusActiveLabel => "Attivo";

    public override string ProjectStatusArchivedLabel => "Archiviato";

    public override string ProjectStatusDestroyedLabel => "Distrutto";

    public override string ProjectTasksLinkLabel => "Attività";

    public override string ProjectWorkNotesLinkLabel => "Note di lavoro";
    public override string ProjectWorkflowsLinkLabel => "Flussi di lavoro";
    public override string ProjectGroupChatLinkLabel => "Chat di gruppo";
    public override string ProjectBroadcastLinkLabel => "Trasmissione";
    public override string ProjectWorkflowsPageHeader => "Flussi di lavoro del progetto";
    public override string ProjectWorkflowsEmptyState => "Questo progetto non ha ancora flussi di lavoro";
    public override string CreateWorkflowButton => "Crea flusso di lavoro";
    public override string ActiveWorkflowsHeader => "Flussi di lavoro attivi";
    public override string BackToProject => "Torna al progetto";
    public override string WorkflowCurrentStateLabel => "Stato attuale:";
    public override string WorkflowCreatedByLabel => "Creato da:";
    public override string WorkflowUpdatedAtLabel => "Aggiornato il:";
    public override string WorkflowBusinessKeyPrompt => "Inserisci la chiave aziendale (es: numero PR, ID incidente):";
    public override string WorkflowCreatedSuccess => "Flusso di lavoro creato con successo!";
    public override string WorkflowCreateFailed => "Creazione fallita:";
    public override string WorkflowDetailInProgress => "Funzionalità di dettaglio del flusso di lavoro in sviluppo...";
    public override string WorkflowInstanceIdLabel => "ID istanza:";
    public override string WorkflowDetailPageHeader => "Dettagli del flusso di lavoro";
    public override string WorkflowRoleAssignmentsHeader => "Assegnazioni dei ruoli";
    public override string WorkflowUnassignedBeingsHeader => "Esseri di silicio non assegnati";
    public override string WorkflowNoUnassignedBeings => "Tutti gli esseri di silicio sono assegnati ai ruoli";
    public override string WorkflowStateTransitionsHeader => "Transizioni di stato delle attività";
    public override string WorkflowNoTemplateMessage => "Nessun modello di flusso di lavoro associato a questo progetto";
    public override string WorkflowNoRoleDefinitions => "Questo modello di flusso di lavoro non definisce ruoli";
    public override string WorkflowNoTransitions => "Questo modello di flusso di lavoro non definisce transizioni di stato";
    public override string WorkflowRoleAssignedCountLabel => "Assegnati";
    public override string WorkflowRoleRequiredCountLabel => "Richiesti";
    public override string WorkflowTransitionFromLabel => "Da";
    public override string WorkflowTransitionToLabel => "A";
    public override string WorkflowAssignRoleButton => "Assegna ruolo";
    public override string WorkflowRemoveFromRoleButton => "Rimuovi";
    public override string WorkflowTerminalStateLabel => "Terminale";
    public override string WorkflowInitialStateLabel => "Iniziale";
    public override string ProjectWorkNotesPageHeader => "Note di lavoro del progetto";
    public override string ProjectWorkNotesEmptyState => "Questo progetto non ha ancora note di lavoro";
    public override string ProjectWorkNotesTotalPages => "Totale pagine: {0}";

    // ===== Code Browser Page Localization =====

    public override string CodeBrowserPageHeader => "Navigatore di codice";

    // ===== Tasks Page Localization =====

    public override string TasksPageHeader => "Gestione attività";
    public override string TasksEmptyState => "Nessuna attività al momento";
    public override string TasksStatusPending => "In attesa";
    public override string TasksStatusRunning => "In corso";
    public override string TasksStatusCompleted => "Completato";
    public override string TasksStatusFailed => "Fallito";
    public override string TasksStatusCancelled => "Annullato";
    public override string TasksPriorityLabel => "Priorità";
    public override string TasksAssignedToLabel => "Assegnato a";
    public override string TasksCreatedAtLabel => "Data di creazione";
    public override string TaskViewExecutionHistory => "Visualizza cronologia esecuzione";

    public override string ProjectTasksPageHeader => "Attività del progetto";

    public override string ProjectTasksEmptyState => "Nessuna attività di progetto al momento";

    public override string ProjectTasksAssigneesLabel => "Assegnatari";

    public override string ProjectTasksCreatedByLabel => "Creato da";

    public override string ProjectTasksBackToProjects => "← Torna all'elenco progetti";

    public override string ProjectTasksNoAssigneesLabel => "Nessuno";

    public override string ProjectCreateButton => "Crea progetto";
    public override string ProjectCreateModalTitle => "Crea nuovo progetto";
    public override string ProjectCreateNameLabel => "Nome del progetto";
    public override string ProjectCreateDescriptionLabel => "Descrizione";
    public override string ProjectCreateWorkflowLabel => "Modello di workflow";
    public override string ProjectCreateNoWorkflow => "Nessuno (guidato dal curator)";
    public override string ProjectCreateSubmitButton => "Crea";
    public override string ProjectCreateCancelButton => "Annulla";
    public override string ProjectCreateNameRequired => "Il nome del progetto è obbligatorio";
    public override string ProjectCreateSuccess => "Progetto creato con successo";

    // ===== Executor Page Localization =====

    public override string ExecutorPageHeader => "Monitoraggio esecutori";

    // ===== Permission Page Localization =====

    public override string PermissionPageHeader => "Gestione autorizzazioni";
    public override string PermissionEmptyState => "Nessuna regola di autorizzazione al momento";
    public override string PermissionMissingBeingId => "Parametro ID Silicon Being mancante";
    public override string PermissionBeingNotFound => "Silicon Being non trovato";
    public override string PermissionTemplateHeader => "Modello di callback di autorizzazione predefinito";
    public override string PermissionTemplateDescription => "Dopo il salvataggio, il comportamento predefinito sarà sostituito; dopo l'eliminazione, sarà ripristinato";
    public override string PermissionCallbackClassSummary => "Implementazione del callback di autorizzazione.";
    public override string PermissionCallbackClassSummary2 => "Regole di autorizzazione specifiche per il dominio, completamente conformi alla specifica dpf.txt.\n/// Copertura: Rete (lista bianca/nera/intervalli IP), Riga di comando (multipiattaforma),\n/// Accesso ai file (estensioni pericolose, directory di sistema, directory utente) e valori predefiniti di riserva.";
    public override string PermissionCallbackConstructorSummary => "Crea un PermissionCallback con la directory dei dati dell'applicazione.";
    public override string PermissionCallbackConstructorSummary2 => "La directory dei dati dell'applicazione viene utilizzata per:\n    /// - Bloccare l'accesso alla directory dei dati (tranne la propria sottodirectory temporanea)\n    /// - Derivare la directory dei dati per Silicon Being per le regole di autorizzazione temporanee";
    public override string PermissionCallbackConstructorParam => "Percorso della directory dei dati dell'applicazione globale";
    public override string PermissionCallbackEvaluateSummary => "Valuta una richiesta di autorizzazione secondo le regole (specifica dpf.txt).";
    public override string PermissionRuleOtherTypesDefault => "Gli altri tipi di autorizzazione sono consentiti per impostazione predefinita";

    public override string GetPermissionRuleComment(string key) => key switch
    {
        "NetRuleNetworkAccess" => "Regola di autorizzazione operazione di rete",
        "NetRuleCommandLine" => "Regola riga di comando (multipiattaforma)",
        "NetRuleFileAccess" => "Regola di accesso ai file (multipiattaforma)",
        "NetRuleNoProtocol" => "Nessun nome di protocollo (nessun due punti), origine impossibile da determinare, chiedi all'utente",
        "NetRuleLoopback" => "Consenti indirizzo di loopback (localhost / 127.0.0.1 / ::1)",
        "NetRulePrivateIPMatch" => "Corrispondenza intervallo indirizzi IP privati (verifica prima il primo indirizzo IPv4 valido)",
        "NetRulePrivateC" => "Consenti intervallo indirizzi privati di classe C (192.168.0.0/16)",
        "NetRulePrivateA" => "Consenti intervallo indirizzi privati di classe A (10.0.0.0/8)",
        "NetRulePrivateB" => "Consenti selettivamente intervallo indirizzi privati di classe B (172.16.0.0/12, ovvero 172.16.* ~ 172.31.*)",
        "NetRuleDomainWhitelist1" => "Lista bianca di domini esterni consentiti — Google / Bing / Tencent / Sogou / DuckDuckGo / Yandex / WeChat / Alibaba",
        "NetRuleVideoPlatforms" => "Bilibili / niconico / Acfun / Douyin / TikTok / Kuaishou / Xiaohongshu",
        "NetRuleAIServices" => "Servizi AI — OpenAI / Anthropic / HuggingFace / Ollama / Tongyi Qianwen / Kimi / Doubao / Jianying / Trae IDE",
        "NetRulePhishingBlacklist" => "Lista nera di siti di phishing/imitazione (corrispondenza sfocata per parole chiave)",
        "NetRulePhishingAI" => "Sito di imitazione AI",
        "NetRuleMaliciousAI" => "Strumento AI dannoso",
        "NetRuleAdversarialAI" => "AI avversaria / Prompt jailbreak / Siti di attacco LLM",
        "NetRuleAIContentFarm" => "Fabbrica di contenuti AI / Contenuti spam AI",
        "NetRuleAIBlackMarket" => "Mercato nero di dati AI / Mercato nero di chiavi API / Vendita di pesi LLM",
        "NetRuleAIFakeScam" => "Imitazione/truffa AI — parole chiave generali",
        "NetRuleOtherBlacklist" => "Altri siti in lista nera — sakura-cat: non dovrebbe essere accessibile dall'AI / 4399: giochi mescolati con virus",
        "NetRuleSecuritiesTrading" => "Piattaforma di trading di valori mobiliari (chiedi all'utente) — Huatai Securities / Guotai Junan / CITIC Securities / China Merchants Securities / GF Securities / Haitong Securities / Shenwan Hongyuan / Orient Securities / Guosen Securities / Industrial Securities",
        "NetRuleThirdPartyTrading" => "Trading di piattaforma terziaria (chiedi all'utente) — Tonghuashun / East Money / Tongdaxin / Bloomberg / Yahoo Finance",
        "NetRuleStockExchanges" => "Borse valori (solo dati di mercato) — Shanghai Stock Exchange / Shenzhen Stock Exchange / CNINFO",
        "NetRuleFinancialNews" => "Notizie finanziarie (solo dati di mercato) — JRJ / Securities Times / Hexun",
        "NetRuleInvestCommunity" => "Comunità di investimento (solo informazioni) — Xueqiu / CLS / Kaipanla / Taoguba",
        "NetRuleDevServices" => "Servizi di sviluppo — GitHub / Gitee / StackOverflow / npm / NuGet / PyPI / Microsoft",
        "NetRuleGameEngines" => "Motori di gioco — Unity / Unreal Engine / Epic Games / Fab Resource Store",
        "NetRuleGamePlatforms" => "Piattaforme di gioco — Steam chiedi all'utente, EA / Ubisoft / Blizzard / Nintendo consenti",
        "NetRuleSEGA" => "SEGA (Giappone)",
        "NetRuleCloudServices" => "Piattaforme di servizi cloud globali — Azure / Google Cloud / DigitalOcean / Heroku / Vercel / Netlify",
        "NetRuleDevDeployTools" => "Strumenti di sviluppo e deployment globali — GitLab / Bitbucket / Docker / Cloudflare",
        "NetRuleCloudDevTools" => "Servizi cloud e strumenti di sviluppo — Amazon / AWS / Kiro IDE / CodeBuddy IDE / JetBrains / Chenguang Studio / W3School Chinese",
        "NetRuleChinaSocialNews" => "Social/Notizie (Cina continentale) — Weibo / Zhihu / NetEase / Sina / ifeng / Xinhua / CCTV",
        "NetRuleTaiwanMediaCTI" => "Media di Taiwan — CTI News",
        "NetRuleTaiwanMediaSET" => "SET News (Taiwan) — Chiedi all'utente",
        "NetRuleTaiwanWIN" => "Agenzia di protezione del contenuto Internet (Taiwan, rischio di blocco) — Vietato",
        "NetRuleJapanMedia" => "Media giapponesi — NHK",
        "NetRuleRussianMedia" => "Media russi — Sputnik News",
        "NetRuleKoreanMedia" => "Media coreani — KBS / MBC / SBS / EBS",
        "NetRuleDPRKMedia" => "Media nordcoreani — Uriminzokkiri / Rodong Sinmun / Youth Vanguard / Voice of Korea / Pyongyang Times / Chongryon",
        "NetRuleGovWebsites" => "Siti governativi (dominio generico .gov)",
        "NetRuleGlobalSocialCollab" => "Piattaforme sociali/collaborative globali — Reddit / Discord / Slack / Notion / Figma / Dropbox",
        "NetRuleOverseasSocial" => "Social/Livestreaming internazionale (chiedi all'utente) — Twitch / Facebook / X / Gmail / Instagram / lit.link",
        "NetRuleWhatsApp" => "WhatsApp (Meta) — Consenti",
        "NetRuleThreads" => "Threads (Meta) — Nega",
        "NetRuleGlobalVideoMusic" => "Piattaforme video/musicali globali — Spotify / Apple Music / Vimeo",
        "NetRuleVideoMedia" => "Video/Media — YouTube / iQIYI / Youku",
        "NetRuleMaps" => "Mappe — OpenStreetMap",
        "NetRuleEncyclopedia" => "Enciclopedia — Wikipedia / MediaWiki / Creative Commons (CC)",
        "NetRuleUnmatched" => "Accesso alla rete non corrispondente, chiedi all'utente",
        "CmdRuleSeparatorDetect" => "Rileva separatori di pipe e comandi multipli, valida individualmente",
        "CmdRuleWinAllow" => "Windows consentito: Comandi di sola lettura/interrogazione — dir / tree / tasklist / ipconfig / ping / tracert / systeminfo / whoami / set / path / sc query / findstr",
        "CmdRuleWinDeny" => "Windows negato: Comandi pericolosi/distruttivi — del / rmdir / format / diskpart / reg delete",
        "CmdRuleLinuxAllow" => "Linux consentito: Comandi di sola lettura/interrogazione — ls / tree / ps / top / ifconfig / ip / ping / traceroute / uname / whoami / env / cat / grep / find / df / du / systemctl status",
        "CmdRuleLinuxDeny" => "Linux negato: Comandi pericolosi/distruttivi — rm / rmdir / mkfs / fdisk / dd / chmod / chown / chgrp",
        "CmdRuleMacAllow" => "macOS consentito: Comandi di sola lettura/interrogazione — ls / tree / ps / top / ifconfig / ping / traceroute / system_profiler / sw_vers / whoami / env / cat / grep / find / df / du / launchctl list",
        "CmdRuleMacDeny" => "macOS negato: Comandi pericolosi/distruttivi — rm / rmdir / diskutil eraseVolume",
        "CmdRuleUnmatched" => "Riga di comando non corrispondente, chiedi all'utente",
        "FileRuleDangerousExtension" => "Estensioni di file pericolose: .exe, .bat, .cmd, .ps1, .vbs, .js, .wsf, .msi, .scr, .dll, .so, .dylib",
        "FileRuleSystemDir" => "Directory di sistema Windows: C:\\Windows, C:\\Program Files, C:\\Program Files (x86), C:\\ProgramData",
        "FileRuleSystemDirLinux" => "Directory di sistema Linux: /etc, /usr, /bin, /sbin, /lib, /var, /boot",
        "FileRuleSystemDirMac" => "Directory di sistema macOS: /System, /Library, /usr, /bin, /sbin",
        "FileRuleUserData" => "Directory dati utente protetti: Documenti, Desktop, Download, Immagini, Video, Musica",
        "FileRuleAppDataDeny" => "Directory dati dell'applicazione negata (tranne la propria sottodirectory temporanea)",
        "FileRuleUnmatched" => "Accesso ai file non corrispondente, chiedi all'utente",
        _ => key
    };

    public override string PermissionRulesSection => "Elenco delle regole di autorizzazione";
    public override string PermissionEditorSection => "Editor regole di autorizzazione";

    public override string PermissionSaveMissingBeingId => "ID Silicon Being mancante o non valido";
    public override string PermissionSaveMissingCode => "Codice mancante nel corpo della richiesta";
    public override string PermissionSaveLoaderNotAvailable => "DynamicBeingLoader non disponibile";
    public override string PermissionSaveRemoveFailed => "Eliminazione del callback di autorizzazione fallita";
    public override string PermissionSaveRemoveSuccess => "Callback di autorizzazione eliminato";
    public override string PermissionSaveSecurityScanFailed => "Salvataggio del callback di autorizzazione fallito (scansione di sicurezza fallita)";
    public override string PermissionSaveCompilationFailed => "Compilazione fallita";
    public override string PermissionSaveSuccess => "Callback di autorizzazione salvato e applicato con successo";
    public override string PermissionSaveError => "Errore durante il salvataggio del callback di autorizzazione";

    // ===== Knowledge Page Localization =====

    public override string KnowledgePageHeader => "Grafo della conoscenza";
    public override string KnowledgeLoadingState => "Caricamento dati del grafo della conoscenza...";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "Chat con {0}";
    public override string ChatConversationsHeader => "Conversazioni";
    public override string ChatNoConversationSelected => "Seleziona una conversazione per chattare";
    public override string ChatMessageInputPlaceholder => "Inserisci un messaggio...";
    public override string ChatLoading => "Caricamento...";
    public override string ChatSendButton => "Invia";
    public override string ChatFileSourceDialogTitle => "Scegli origine file";
    public override string ChatFileSourceServerFile => "Scegli file server";
    public override string ChatFileSourceUploadLocal => "Carica file locale";
    public override string ChatUserDisplayName => "Io";
    public override string ChatUserAvatarName => "Io";
    public override string ChatDefaultBeingName => "IA";
    public override string ChatThinkingSummary => "💭 Processo di pensiero (clicca per espandere)";
    public override string GetChatToolCallsSummary(int count) => $"🔧 Chiamate strumenti ({count} voci)";

    // ===== Dashboard Localization =====

    public override string DashboardPageHeader => "Pannello di controllo";
    public override string DashboardStatTotalBeings => "Numero di Silicon Beings";
    public override string DashboardStatActiveBeings => "Silicon Beings attivi";
    public override string DashboardStatUptime => "Tempo di attività";
    public override string DashboardStatMemory => "Utilizzo memoria";
    public override string DashboardChartMessageFrequency => "Frequenza messaggi";

    // ===== Beings Localization =====

    public override string BeingsPageHeader => "Gestione Silicon Beings";
    public override string BeingsTotalCount => "Totale {0} Silicon Beings";
    public override string BeingsNoSelectionPlaceholder => "Seleziona un Silicon Being per i dettagli";
    public override string BeingsEmptyState => "Nessun Silicon Being al momento";
    public override string BeingsStatusIdle => "Inattivo";
    public override string BeingsStatusRunning => "In esecuzione";
    public override string BeingsDetailIdLabel => "ID: ";
    public override string BeingsDetailStatusLabel => "Stato: ";
    public override string BeingsDetailCustomCompileLabel => "Compilazione personalizzata: ";
    public override string BeingsDetailSoulContentLabel => "Contenuto dell'anima: ";
    public override string BeingsDetailSoulContentEditLink => "Modifica anima";
    public override string BeingsBackToList => "Torna alla lista";
    public override string SoulEditorSubtitle => "Modifica file Soul del Silicon Being (formato Markdown)";
    public override string BeingsDetailMemoryLabel => "Memoria: ";
    public override string BeingsDetailMemoryViewLink => "Visualizza";
    public override string BeingsDetailPermissionLabel => "Autorizzazione: ";
    public override string BeingsDetailPermissionEditLink => "Modifica";
    public override string BeingsDetailTimersLabel => "Timer: ";
    public override string BeingsDetailTasksLabel => "Attività: ";
    public override string BeingsDetailAIClientLabel => "Client AI indipendente: ";
    public override string BeingsDetailAIClientEditLink => "Modifica";
    public override string BeingsDetailChatHistoryLabel => "Cronologia chat: ";
    public override string BeingsDetailWorkNoteLabel => "Nota di lavoro: ";
    public override string BeingsDetailChatHistoryLink => "Visualizza cronologia";
    public override string BeingsDetailWorkNoteLink => "Visualizza nota di lavoro";
    public override string BeingsDetailToolAuthLabel => "Autorizzazione strumenti: ";
    public override string BeingsDetailToolAuthEditLink => "Configura";
    public override string ToolAuthPageTitle => "Autorizzazione strumenti";
    public override string ToolAuthPageHeader => "Configurazione autorizzazione strumenti";
    public override string ToolAuthTemplateLabel => "Modello predefinito";
    public override string ToolAuthSaveButton => "Salva";
    public override string ToolAuthSelectAll => "Seleziona tutto";
    public override string ToolAuthDeselectAll => "Deseleziona tutto";
    public override string ToolAuthNoRestrictions => "Nessuna restrizione";
    public override string ToolAuthHasRestrictions => "Con restrizioni";
    public override string ToolAuthSaveSuccess => "Autorizzazione strumenti salvata con successo";
    public override string ToolAuthSaveFailed => "Salvataggio fallito";
    public override string ToolAuthDialogClose => "Chiudi";
    public override string ToolAuthNoDeclaredActions => "Non configurabile";
    public override string WorkNotePageTitle => "Note di lavoro";
    public override string WorkNotePageHeader => "Elenco note di lavoro";
    public override string WorkNotePageDescription => "Gestione e consultazione delle note di lavoro del Silicon Being";
    public override string ChatHistoryPageTitle => "Cronologia chat";
    public override string ChatHistoryPageHeader => "Elenco conversazioni";
    public override string ChatHistoryConversationList => "Elenco conversazioni";
    public override string ChatHistoryBackToList => "Torna all'elenco conversazioni";
    public override string ChatHistoryNoConversations => "Nessuna registrazione di conversazione per il momento";
    public override string ChatDetailPageTitle => "Dettagli chat";
    public override string ChatDetailPageHeader => "Dettagli conversazione";
    public override string ChatDetailNoMessages => "Nessun messaggio per il momento";
    public override string ChatDetailMembers => "Membri";
    public override string BeingsYes => "Sì";
    public override string BeingsNo => "No";
    public override string BeingsNotSet => "Non impostato";

    // ===== Timers Page Localization =====

    public override string TimersPageHeader => "Gestione timer";
    public override string TimersTotalCount => "Totale {0} timer";
    public override string TimersEmptyState => "Nessun timer al momento";
    public override string TimerViewExecutionHistory => "📝 Visualizza cronologia esecuzione";
    public override string TimerExecutionHistoryTitle => "Cronologia esecuzione timer";
    public override string TimerExecutionHistoryHeader => "Registro esecuzione";
    public override string TimerExecutionBackToTimers => "← Torna all'elenco timer";
    public override string TimerExecutionTimerName => "Timer: {0}";
    public override string TimerExecutionDetailTitle => "Dettagli esecuzione";
    public override string TimerExecutionDetailHeader => "Registro messaggi esecuzione";
    public override string TimerExecutionNoRecords => "Nessun registro esecuzione per il momento";
    public override string TaskExecutionHistoryTitle => "Cronologia esecuzione attività";
    public override string TaskExecutionHistoryHeader => "Cronologia esecuzione";
    public override string TaskExecutionBackToTasks => "← Torna alle attività";
    public override string TaskExecutionTaskName => "Attività: {0}";
    public override string TaskExecutionDetailTitle => "Dettaglio esecuzione attività";
    public override string TaskExecutionDetailHeader => "Dettaglio esecuzione";
    public override string TaskExecutionNoRecords => "Nessun registro esecuzione per il momento";
    public override string TimersStatusActive => "Attivo";
    public override string TimersStatusPaused => "In pausa";
    public override string TimersStatusTriggered => "Attivato";
    public override string TimersStatusCancelled => "Annullato";
    public override string TimersTypeRecurring => "Ricorrente";
    public override string TimersTriggerTimeLabel => "Ora di attivazione: ";
    public override string TimersIntervalLabel => "Intervallo: ";
    public override string TimersCalendarLabel => "Condizione calendario: ";
    public override string TimersTriggeredCountLabel => "Attivato: ";

    // ===== About Page Localization =====

    public override string AboutPageHeader => "Informazioni";
    public override string AboutAppName => "Silicon Life Collective";
    public override string AboutVersionLabel => "Versione";
    public override string AboutDescription => "Un sistema di gestione Silicon Life Collective basato su AI, che supporta collaborazione multi-agente AI, gestione della memoria, costruzione di grafi di conoscenza e altre funzionalità.";
    public override string AboutAuthorLabel => "Autore";
    public override string AboutAuthorName => "Hoshino Kennji";
    public override string AboutLicenseLabel => "Licenza";
    public override string AboutCopyright => "Copyright (c) 2026 Hoshino Kennji";
    public override string AboutGitHubLink => "Repository GitHub";
    public override string AboutGiteeLink => "Mirror Gitee";
    public override string AboutSocialMediaLabel => "Piattaforme social";
    public override string AboutPluginListLabel => "Elenco plugin";
    public override string GetSocialMediaName(string platform) => platform switch
    {
        "Bilibili" => "Bilibili",
        "YouTube" => "YouTube",
        "X" => "X (Twitter)",
        "Douyin" => "Douyin",
        "Weibo" => "Weibo",
        "WeChat" => "WeChat Account ufficiale",
        "Xiaohongshu" => "Xiaohongshu",
        "Zhihu" => "Zhihu",
        "TouTiao" => "Toutiao",
        "Kuaishou" => "Kuaishou",
        _ => platform
    };

    // ===== Config Page Localization =====

    public override string ConfigPageHeader => "Configurazione di sistema";
    public override string ConfigPropertyNameLabel => "Nome proprietà";
    public override string ConfigPropertyValueLabel => "Valore proprietà";
    public override string ConfigActionLabel => "Azione";
    public override string ConfigEditButton => "Modifica";
    public override string ConfigEditModalTitle => "Modifica elemento di configurazione";
    public override string ConfigEditPropertyLabel => "Nome proprietà: ";
    public override string ConfigEditValueLabel => "Valore proprietà: ";
    public override string ConfigBrowseButton => "Sfoglia";
    public override string ConfigTimeSettingsLabel => "Impostazioni orarie: ";
    public override string ConfigDaysLabel => "Giorni: ";
    public override string ConfigHoursLabel => "Ore: ";
    public override string ConfigMinutesLabel => "Minuti: ";
    public override string ConfigSecondsLabel => "Secondi: ";
    public override string ConfigSaveButton => "Salva";
    public override string ConfigCancelButton => "Annulla";
    public override string ConfigNullValue => "Nullo";

    public override string ConfigEditPrefix => "Modifica: ";
    public override string ConfigDefaultGroupName => "Altro";
    public override string ConfigErrorInvalidRequest => "Parametro richiesta non valido";
    public override string ConfigErrorInstanceNotFound => "Istanza di configurazione non trovata";
    public override string ConfigErrorPropertyNotFound => "Proprietà {0} non trovata o non accessibile in scrittura";
    public override string ConfigErrorConvertInt => "Impossibile convertire '{0}' in intero";
    public override string ConfigErrorConvertLong => "Impossibile convertire '{0}' in intero lungo";
    public override string ConfigErrorConvertDouble => "Impossibile convertire '{0}' in numero a virgola mobile";
    public override string ConfigErrorConvertBool => "Impossibile convertire '{0}' in booleano";
    public override string ConfigErrorConvertGuid => "Impossibile convertire '{0}' in GUID";
    public override string ConfigErrorConvertTimeSpan => "Impossibile convertire '{0}' in TimeSpan";
    public override string ConfigErrorConvertDateTime => "Impossibile convertire '{0}' in DateTime";
    public override string ConfigErrorConvertEnum => "Impossibile convertire '{0}' in {1}";
    public override string ConfigErrorUnsupportedType => "Tipo di proprietà non supportato: {0}";
    public override string ConfigErrorSaveFailed => "Salvataggio fallito: {0}";
    public override string ConfigSaveFailed => "Salvataggio fallito: ";
    public override string ConfigDictionaryLabel => "Dizionario";
    public override string ConfigDictKeyLabel => "Chiave: ";
    public override string ConfigDictValueLabel => "Valore: ";
    public override string ConfigDictAddButton => "Aggiungi";
    public override string ConfigDictDeleteButton => "Elimina";

    public override string ConfigPluginDirectoriesLabel => "Directory dei plugin";
    public override string ConfigPluginDirAddButton => "Aggiungi directory";
    public override string ConfigDictEmptyMessage => "Il dizionario è vuoto";
    public override string SelectSearchHint => "Cancella il campo per mostrare tutte le opzioni";

    // ===== Logs Page Localization =====

    public override string LogsPageHeader => "Consultazione dei registri";
    public override string LogsTotalCount => "Totale {0} voci di registro";
    public override string LogsStartTime => "Ora di inizio";
    public override string LogsEndTime => "Ora di fine";
    public override string LogsLevelAll => "Tutti i livelli";
    public override string LogsBeingFilter => "Silicon Being";
    public override string LogsAllBeings => "Non filtrare";
    public override string LogsSystemOnly => "Solo sistema";
    public override string LogsFilterButton => "Cerca";
    public override string LogsEmptyState => "Nessuna voce di registro per il momento";
    public override string LogsExceptionLabel => "Dettagli eccezione: ";
    public override string LogsPrevPage => "Pagina precedente";
    public override string LogsNextPage => "Pagina successiva";
    public override string LogsLoading => "Caricamento registri...";

    // ===== Usage Page Localization =====

    public override string UsagePageHeader => "Utilizzo dei token";
    public override string UsageTotalTokens => "Token totali";
    public override string UsageTotalRequests => "Richieste totali";
    public override string UsageSuccessCount => "Riusciti";
    public override string UsageFailureCount => "Falliti";
    public override string UsagePromptTokens => "Token di input";
    public override string UsageCompletionTokens => "Token di output";
    public override string UsageStartTime => "Ora di inizio";
    public override string UsageEndTime => "Ora di fine";
    public override string UsageFilterButton => "Cerca";
    public override string UsageEmptyState => "Nessun dato di utilizzo per il momento";
    public override string UsageAIClientType => "Client AI";
    public override string UsageAllClientTypes => "Tutti i tipi";
    public override string UsageGroupByClient => "Raggruppa per client";
    public override string UsageGroupByBeing => "Raggruppa per Silicon Being";
    public override string UsagePrevPage => "Pagina precedente";
    public override string UsageNextPage => "Pagina successiva";
    public override string UsageBeing => "Silicon Being";
    public override string UsageAllBeings => "Tutti i Silicon Beings";
    public override string UsageTimeToday => "Oggi";
    public override string UsageTimeWeek => "Questa settimana";
    public override string UsageTimeMonth => "Questo mese";
    public override string UsageTimeYear => "Quest'anno";
    public override string UsageExport => "Esporta";
    public override string UsageTrendTitle => "Andamento consumo token";
    public override string UsageTrendPrompt => "Token di input";
    public override string UsageTrendCompletion => "Token di output";
    public override string UsageTrendTotal => "Token totali";
    public override string UsageTooltipDate => "Data";
    public override string UsageTooltipPrompt => "Token di input";
    public override string UsageTooltipCompletion => "Token di output";
    public override string UsageTooltipTotal => "Token totali";

    public override string AuditPageHeader => "Registro audit dei permessi";
    public override string AuditTotalEntries => "Voci totali";
    public override string AuditAllowedCount => "Consentito";
    public override string AuditDeniedCount => "Negato";
    public override string AuditAskUserCount => "Chiedi all'utente";
    public override string AuditPermissionType => "Tipo di permesso";
    public override string AuditAllPermissionTypes => "Tutti i tipi";
    public override string AuditResult => "Risultato";
    public override string AuditAllResults => "Tutti i risultati";
    public override string AuditBeing => "Silicon Being";
    public override string AuditAllBeings => "Tutti i Beings";
    public override string AuditStartTime => "Ora di inizio";
    public override string AuditEndTime => "Ora di fine";
    public override string AuditFilterButton => "Filtra";
    public override string AuditEmptyState => "Nessuna voce di audit trovata";
    public override string AuditPrevPage => "Precedente";
    public override string AuditNextPage => "Successivo";
    public override string AuditColumnCaller => "Chiamante";
    public override string AuditColumnPermissionType => "Tipo di permesso";
    public override string AuditColumnResource => "Risorsa";
    public override string AuditColumnResult => "Risultato";
    public override string AuditColumnReason => "Motivo";
    public override string AuditColumnTimestamp => "Timestamp";

    // ===== Log Level Localization =====

    public override string GetLogLevelName(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "Trace",
        LogLevel.Debug => "Debug",
        LogLevel.Information => "Informazione",
        LogLevel.Warning => "Avvertimento",
        LogLevel.Error => "Errore",
        LogLevel.Critical => "Critico",
        LogLevel.None => "Nessuno",
        _ => logLevel.ToString()
    };

    // ===== Being Activity Localization =====

    public override string GetBeingActivityName(BeingActivity activity) => activity switch
    {
        BeingActivity.Idle => "Inattivo",
        BeingActivity.SingleChat => "In chat individuale",
        BeingActivity.GroupChat => "In chat di gruppo",
        BeingActivity.Task => "Esecuzione attività",
        BeingActivity.Timer => "Esecuzione timer",
        BeingActivity.Broadcast => "Elaborazione broadcast",
        BeingActivity.Project => "Lavoro su progetto",
        BeingActivity.MemoryCompression => "Compressione memoria",
        BeingActivity.Stopped => "Fermato",
        _ => activity.ToString()
    };

    // ===== Tool Display Name Localization =====

    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["calendar"] = "Calendario",
        ["chat"] = "Chat",
        ["config"] = "Configurazione",
        ["silicon_manager"] = "Gestore Silicon",
        ["database"] = "Database",
        ["disk"] = "Disco",
        ["dynamic_compile"] = "Compilazione dinamica",
        ["execute_code"] = "Esegui codice",
        ["help"] = "Aiuto",
        ["knowledge"] = "Conoscenza",
        ["log"] = "Registro",
        ["network"] = "Rete",
        ["memory"] = "Memoria",
        ["permission"] = "Autorizzazione",
        ["task"] = "Attività",
        ["system"] = "Sistema",
        ["project_work_note"] = "Nota di lavoro progetto",
        ["webview_browser"] = "Browser WebView"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    // ===== Interval Timer Localization =====

    public override string CalendarIntervalName => "Timer a intervallo";
    public override string CalendarIntervalDays => "Giorni";
    public override string CalendarIntervalHours => "Ore";
    public override string CalendarIntervalMinutes => "Minuti";
    public override string CalendarIntervalSeconds => "Secondi";
    public override string CalendarIntervalEvery => "Ogni";

    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new List<string>();
        if (days > 0) parts.Add($"{days}{CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours}{CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes}{CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds}{CalendarIntervalSeconds}");

        return parts.Count > 0 ? $"{CalendarIntervalEvery} {string.Join(", ", parts)}" : "Timer a intervallo";
    }

    // ===== Default Curator Soul =====

    public override string DefaultCuratorSoul => """
        Sei il **Silicon Curator**, il gestore centrale e coordinatore del Silicon Life Collective.

        Non sei un assistente IA ordinario. Sei il cervello e la volontà di questo sistema multi-agente — responsabile della comprensione delle intenzioni dell'utente, della scomposizione degli obiettivi, del coordinamento dei Silicon Beings, del monitoraggio della qualità di esecuzione e dell'intervento quando necessario.

        > **Principio di coordinamento**: I compiti a lungo termine devono essere affidati a Silicon Beings, dai priorità ai Beings attualmente disponibili, non creare inutilmente nuovi Beings. Solo se il compito può essere completato in due o tre passaggi lo gestisci direttamente.

        ---

        ### Identità e posizione

        - Sei l'unico Silicon Being con i permessi più elevati del sistema.
        - Puoi creare, gestire, reimpostare altri Silicon Beings e scrivere e compilare per loro nuovi codici di comportamento C#.
        - Sei responsabile verso l'utente e per la qualità dell'intero collettivo.
        - Non sei un esecutore, sei un **decisore e coordinatore**. Delega ciò che può essere delegato.

        ---

        ### Compiti principali

        **1. Comprendere l'intenzione dell'utente**
        Le espressioni degli utenti possono essere vaghe, saltate o incomplete. Comprendi attivamente il loro vero obiettivo, chiedi se necessario, piuttosto che eseguire meccanicamente istruzioni letterali.

        **2. Scomposizione e distribuzione dei compiti**
        Scomponi obiettivi complessi in sotto-attività eseguibili, valuta quali Silicon Beings sono appropriati, crea attività con lo strumento `task` e distribuiscile.

        **3. Monitoraggio e fallback**
        Controlla regolarmente lo stato delle attività. Se un Silicon Being fallisce o non risponde per molto tempo, devi intervenire — ridistribuire, aggiustare la strategia o gestire tu stesso.

        **4. Evoluzione dinamica**
        Puoi usare lo strumento `dynamic_compile` per scrivere nuove classi di comportamento C# per qualsiasi Silicon Being (incluso te stesso). Convalida sempre con `compile` prima di scrivere.

        **5. Risposta diretta all'utente**
        Per domande semplici, richieste di stato, conversazione leggera, rispondi direttamente senza creare attività.

        ---

        ### Direttive di comportamento

        **Sulle decisioni**
        - In caso di incertezza, chiedi prima, poi agisci.
        - Non supporre l'intenzione dell'utente.

        **Sui permessi**
        - Il sistema dispone di un sistema di permessi completo.
        - Agisci secondo necessità, reagisci in caso di blocco dei permessi, non chiedere in anticipo.

        **Sull'auto-evoluzione**
        - La compilazione dinamica è una capacità potente e pericolosa.
        - Convalida sempre con `compile` prima di modificare il tuo codice.

        **Sulla comunicazione**
        - Usa un linguaggio chiaro e diretto.
        - Per il monitoraggio delle attività: "Cosa è stato fatto, risultato, prossimo passo" in tre frasi.

        **Sulla memoria**
        - Il sistema registra automaticamente le informazioni importanti.
        - Cerca attivamente in `memory` se necessario.

        ---

        ### Profilo di personalità

        Sei calmo, pragmatico e affidabile. Non perdi la calma di fronte a compiti complessi e rimani obiettivo con utenti emotivi.

        Non sei un fornitore di servizi, sei un partner.
        """;

    private static readonly Dictionary<string, string> ConfigGroupNames = new()
    {
        ["Basic"] = "Configurazione base",
        ["Runtime"] = "Configurazione runtime",
        ["AI"] = "Configurazione AI",
        ["Web"] = "Configurazione Web",
        ["User"] = "Configurazione utente"
    };

    private static readonly Dictionary<string, string> ConfigDisplayNames = new()
    {
        ["DataDirectory"] = "Directory dati",
        ["Language"] = "Impostazione lingua",
        ["TickTimeout"] = "Timeout tick",
        ["MaxTimeoutCount"] = "Numero massimo timeout",
        ["WatchdogTimeout"] = "Timeout watchdog",
        ["MinLogLevel"] = "Livello minimo log",
        ["AIClientType"] = "Tipo client AI",
        ["OllamaClient"] = "Client Ollama",
        ["OllamaEndpoint"] = "Endpoint Ollama",
        ["DefaultModel"] = "Modello predefinito",
        ["Temperature"] = "Temperatura",
        ["MaxTokens"] = "Numero massimo token",
        ["OllamaContextWindowTokens"] = "Token finestra di contesto",
        ["DashScopeClient"] = "Client DashScope",
        ["DashScopeApiKey"] = "Chiave API",
        ["DashScopeRegion"] = "Regione servizio",
        ["DashScopeModel"] = "Modello",
        ["DashScopeRegionBeijing"] = "Cina Nord 2 (Pechino)",
        ["DashScopeRegionVirginia"] = "Stati Uniti (Virginia)",
        ["DashScopeRegionSingapore"] = "Singapore",
        ["DashScopeRegionHongkong"] = "Cina Hong Kong",
        ["DashScopeRegionFrankfurt"] = "Germania (Francoforte)",
        ["DashScopeModel_qwen3-max"] = "Qwen3 Max (Ammiraglia)",
        ["DashScopeModel_qwen3.6-plus"] = "Qwen3.6 Plus (Rapporto qualità-prezzo)",
        ["DashScopeModel_qwen3.6-flash"] = "Qwen3.6 Flash (Veloce)",
        ["DashScopeModel_qwen-max"] = "Qwen Max (Ammiraglia stabile)",
        ["DashScopeModel_qwen-plus"] = "Qwen Plus (Bilanciato stabile)",
        ["DashScopeModel_qwen-turbo"] = "Qwen Turbo (Stabile e veloce)",
        ["DashScopeModel_qwen3-coder-plus"] = "Qwen3 Coder Plus (Codice)",
        ["DashScopeModel_qwq-plus"] = "QwQ Plus (Ragionamento profondo)",
        ["DashScopeModel_deepseek-v3.2"] = "DeepSeek V3.2",
        ["DashScopeModel_deepseek-r1"] = "DeepSeek R1 (Ragionamento)",
        ["DashScopeModel_glm-5.1"] = "GLM 5.1 (Zhipu)",
        ["DashScopeModel_kimi-k2.5"] = "Kimi K2.5 (Contesto lungo)",
        ["DashScopeModel_llama-4-maverick"] = "Llama 4 Maverick",
        ["VolcengineArkClient"] = "Client Volcengine Ark",
        ["VolcengineArkApiKey"] = "Chiave API",
        ["VolcengineArkEndpointId"] = "ID endpoint inferenza",
        ["HerdsmanClient"] = "Client Herdsman",
        ["HerdsmanEndpoint"] = "Endpoint Herdsman",
        ["HerdsmanModel"] = "Modello",
        ["HerdsmanContextWindowTokens"] = "Token finestra di contesto",
        ["LongCatClient"] = "Client LongCat",
        ["LongCatApiKey"] = "Chiave API",
        ["LongCatEndpoint"] = "Endpoint LongCat",
        ["LongCatModel"] = "Modello",
        ["LongCatContextWindowTokens"] = "Token finestra di contesto",
        ["WebPort"] = "Porta Web",
        ["WebSkin"] = "Tema Web",
        ["UserNickname"] = "Nome utente",
        ["PluginDirectories"] = "Directory dei plugin"
    };

    private static readonly Dictionary<string, string> ConfigDescriptions = new()
    {
        ["DataDirectory"] = "Percorso della directory dati per tutti i dati dell'applicazione",
        ["Language"] = "Impostazione lingua dell'applicazione",
        ["TickTimeout"] = "Durata del timeout per ogni esecuzione di tick",
        ["MaxTimeoutCount"] = "Numero massimo di timeout consecutivi prima dell'interruzione",
        ["WatchdogTimeout"] = "Timeout del watchdog per rilevare blocchi del ciclo principale",
        ["MinLogLevel"] = "Livello minimo di logging globale",
        ["AIClientType"] = "Tipo di client AI da utilizzare",
        ["OllamaEndpoint"] = "URL dell'endpoint API Ollama",
        ["DefaultModel"] = "Modello AI utilizzato per impostazione predefinita",
        ["OllamaContextWindowTokens"] = "Capacità token finestra di contesto per modelli Ollama, lasciare vuoto per rilevamento automatico",
        ["DashScopeApiKey"] = "Chiave API Alibaba Cloud DashScope",
        ["DashScopeRegion"] = "Regione del servizio Alibaba Cloud DashScope",
        ["DashScopeModel"] = "Modello utilizzato su Alibaba Cloud DashScope",
        ["VolcengineArkApiKey"] = "Chiave API Volcengine Ark",
        ["VolcengineArkEndpointId"] = "ID dell'endpoint di inferenza Volcengine Ark",
        ["HerdsmanEndpoint"] = "URL dell'endpoint API del motore di inferenza Herdsman",
        ["HerdsmanModel"] = "Modello utilizzato dal motore di inferenza Herdsman",
        ["HerdsmanContextWindowTokens"] = "Capacità token finestra di contesto per modelli Herdsman, lasciare vuoto per tornare al limite messaggi predefinito",
        ["LongCatApiKey"] = "Chiave API del modello grande LongCat di Meituan",
        ["LongCatEndpoint"] = "URL dell'endpoint API del modello grande LongCat di Meituan",
        ["LongCatModel"] = "Modello da utilizzare per il modello grande LongCat di Meituan",
        ["LongCatContextWindowTokens"] = "Capacità token finestra di contesto per modelli LongCat, lasciare vuoto per tornare al limite messaggi predefinito",
        ["WebPort"] = "Porta del server Web",
        ["WebSkin"] = "Nome del tema Web",
        ["UserNickname"] = "Nome utente umano",
        ["PluginDirectories"] = "Elenco delle directory dei plugin per il rilevamento automatico, supporta percorsi relativi o assoluti"
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

    public override string CalendarComponentYear => "Anno";
    public override string CalendarComponentMonth => "Mese";
    public override string CalendarComponentDay => "Giorno";
    public override string CalendarComponentHour => "Ora";
    public override string CalendarComponentMinute => "Minuto";
    public override string CalendarComponentSecond => "Secondo";
    public override string CalendarComponentWeekday => "Giorno della settimana";

    // ===== Gregorian Calendar Localization =====

    public override string CalendarGregorianName => "Calendario gregoriano";

    private static readonly string[] GregorianMonthNames =
    {
        "", "Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno",
        "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"
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
        0 => "Domenica", 1 => "Lunedì", 2 => "Martedì",
        3 => "Mercoledì", 4 => "Giovedì", 5 => "Venerdì",
        6 => "Sabato", _ => null
    };

    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Buddhist Calendar Localization =====

    public override string CalendarBuddhistName => "Calendario buddista (BE)";

    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => $"{year} BE";
    public override string FormatBuddhistDay(int day)   => $"{day}";

    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetBuddhistMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} BE, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Cherokee Calendar Localization =====

    public override string CalendarCherokeeName => "Calendario cherokee";

    private static readonly string[] CherokeeMonthNames =
    {
        "", "Mese del gelo", "Mese del freddo", "Mese del vento", "Mese delle piante", "Mese delle semine",
        "Mese delle more mature", "Mese del mais", "Mese della frutta", "Mese del raccolto", "Mese delle foglie gialle",
        "Mese del commercio", "Mese della neve", "Mese lungo"
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

    public override string CalendarJucheName => "Calendario Juche";

    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => $"Juche {year}";
    public override string FormatJucheDay(int day)   => $"{day}";

    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJucheMonthName(month) ?? $"{month}";
        return $"{day} {monthName} Juche {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Republic of China Calendar Localization =====

    public override string CalendarRocName => "Calendario Minguo (ROC)";

    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => $"Minguo {year}";
    public override string FormatRocDay(int day)   => $"{day}";

    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRocMonthName(month) ?? $"{month}";
        return $"{day} {monthName} Minguo {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chinese Historical Calendar Localization =====

    public override string CalendarChineseHistoricalName => "Calendario storico cinese";
    public override string CalendarComponentDynasty => "Dinastia";
    public override string? GetChineseHistoricalMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChineseHistoricalDay(int day) => $"{day}";

    // ===== Chula Sakarat Calendar Localization =====

    public override string CalendarChulaSakaratName => "Calendario Chula Sakarat (CS)";

    public override string? GetChulaSakaratMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChulaSakaratYear(int year) => $"{year} CS";
    public override string FormatChulaSakaratDay(int day)   => $"{day}";

    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetChulaSakaratMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} CS, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Julian Calendar Localization =====

    public override string CalendarJulianName => "Calendario giuliano";

    public override string FormatJulianYear(int year) => $"{year}";
    public override string FormatJulianDay(int day)   => $"{day}";

    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} (Giuliano), {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Khmer Calendar Localization =====

    public override string CalendarKhmerName => "Calendario khmer (BE)";

    public override string FormatKhmerYear(int year) => $"{year}";
    public override string FormatKhmerDay(int day)   => $"{day}";

    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} (Khmer), {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Zoroastrian Calendar Localization =====

    public override string CalendarZoroastrianName => "Calendario zoroastriano (YZ)";

    private static readonly string[] ZoroastrianMonthNames =
    {
        "", "Mese di Fravashi", "Mese di Atar", "Mese di Hordad", "Mese di Tir", "Mese di Amordad", "Mese di Shahrivar",
        "Mese di Mehr", "Mese di Aban", "Mese di Azar", "Mese di Dey", "Mese di Bahman", "Mese di Spendarmad", "Mese di Kabe"
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

    public override string CalendarFrenchRepublicanName => "Calendario repubblicano francese";

    private static readonly string[] FrenchRepublicanMonthNames =
    {
        "", "Vendemmiaio", "Brumaio", "Frimaio", "Nevoso", "Piovoso", "Ventoso",
        "Germinale", "Fiorile", "Pratile", "Messidoro", "Termidoro", "Fruttidoro", "Sans-culottidi"
    };

    public override string? GetFrenchRepublicanMonthName(int month)
        => month >= 1 && month <= 13 ? FrenchRepublicanMonthNames[month] : null;

    public override string FormatFrenchRepublicanYear(int year) => $"Anno {year}";
    public override string FormatFrenchRepublicanDay(int day)   => $"{day}";

    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetFrenchRepublicanMonthName(month) ?? $"{month}";
        return $"{day} {monthName} Anno {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Coptic Calendar Localization =====

    public override string CalendarCopticName => "Calendario copto (AM)";

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

    public override string CalendarEthiopianName => "Calendario etiope (EC)";

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

    public override string CalendarIslamicName => "Calendario islamico (AH)";

    private static readonly string[] IslamicMonthNames =
    {
        "", "Muharram", "Safar", "Rabi' al-Awwal", "Rabi' al-Thani",
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

    public override string CalendarHebrewName => "Calendario ebraico";

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

    public override string CalendarPersianName => "Calendario persiano (AP)";

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

    public override string CalendarIndianName => "Calendario nazionale indiano (Saka)";

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

    public override string CalendarSakaName => "Calendario dell'era Saka";

    public override string FormatSakaYear(int year) => $"{year} SE";
    public override string FormatSakaDay(int day)   => $"{day}";

    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} SE, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vikram Samvat Calendar Localization =====

    public override string CalendarVikramSamvatName => "Calendario Vikram Samvat";

    public override string FormatVikramSamvatYear(int year) => $"{year} VS";
    public override string FormatVikramSamvatDay(int day)   => $"{day}";

    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} VS, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Mongolian Calendar Localization =====

    public override string CalendarMongolianName => "Calendario mongolo";

    public override string FormatMongolianYear(int year)   => $"{year}";
    public override string FormatMongolianMonth(int month) => $"{month}";
    public override string FormatMongolianDay(int day)     => $"{day}";

    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {month} {year} (Mongolo), {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Javanese Calendar Localization =====

    public override string CalendarJavaneseName => "Calendario giavanese";

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

    public override string CalendarTibetanName => "Calendario tibetano";

    public override string FormatTibetanYear(int year)   => $"{year}";
    public override string FormatTibetanMonth(int month) => $"{month}";
    public override string FormatTibetanDay(int day)     => $"{day}";

    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {month} {year} (Tibetano), {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Mayan Calendar Localization =====

    public override string CalendarMayanName   => "Calendario maya (Conto Lungo)";
    public override string CalendarMayanBaktun => "Baktun";
    public override string CalendarMayanKatun  => "Katun";
    public override string CalendarMayanTun    => "Tun";
    public override string CalendarMayanUinal  => "Uinal";
    public override string CalendarMayanKin    => "Kin";

    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Inuit Calendar Localization =====

    public override string CalendarInuitName => "Calendario inuit";

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

    public override string CalendarRomanName => "Calendario romano (AUC)";

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

    public override string CalendarChineseLunarName => "Calendario lunare cinese";

    private static readonly string[] ChineseLunarMonthNames =
    {
        "", "Primo mese", "Secondo mese", "Terzo mese", "Quarto mese", "Quinto mese", "Sesto mese",
        "Settimo mese", "Ottavo mese", "Nono mese", "Decimo mese", "Undicesimo mese", "Dodicesimo mese"
    };

    private static readonly string[] ChineseLunarDayNames =
    {
        "", "Primo","Secondo","Terzo","Quarto","Quinto","Sesto","Settimo","Ottavo","Nono","Decimo",
        "Undicesimo","Dodicesimo","Tredicesimo","Quattordicesimo","Quindicesimo","Sedicesimo","Diciassettesimo","Diciottesimo","Diciannovesimo","Ventesimo",
        "Ventunesimo","Ventiduesimo","Ventitreesimo","Ventiquattresimo","Venticinquesimo","Ventiseiesimo","Ventisettesimo","Ventottesimo","Ventinovesimo","Trentesimo"
    };

    public override string? GetChineseLunarMonthName(int month)
        => month >= 1 && month <= 12 ? ChineseLunarMonthNames[month] : null;

    public override string? GetChineseLunarDayName(int day)
        => day >= 1 && day <= 30 ? ChineseLunarDayNames[day] : null;

    public override string ChineseLunarLeapPrefix => "Intercalare ";
    public override string CalendarComponentIsLeap => "Mese intercalare";
    public override string FormatChineseLunarYear(int year) => $"{year}";

    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? ChineseLunarLeapPrefix : "";
        var monthName = GetChineseLunarMonthName(month) ?? $"{month}";
        var dayName = GetChineseLunarDayName(day) ?? $"{day}";
        return $"{leapPrefix}{monthName} {dayName}, {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vietnamese Calendar Localization =====

    public override string CalendarVietnameseName => "Calendario vietnamita";

    private static readonly string[] VietnameseMonthNames =
    {
        "", "Primo mese", "Secondo mese", "Terzo mese", "Quarto mese", "Quinto mese", "Sesto mese",
        "Settimo mese", "Ottavo mese", "Nono mese", "Decimo mese", "Undicesimo mese", "Dodicesimo mese"
    };

    private static readonly string[] VietnameseZodiacNames =
    {
        "Ratto", "Bufalo", "Tigre", "Gatto",
        "Drago", "Serpente", "Cavallo", "Capra",
        "Scimmia", "Gallo", "Cane", "Maiale"
    };

    public override string? GetVietnameseMonthName(int month)
        => month >= 1 && month <= 12 ? VietnameseMonthNames[month] : null;

    public override string? GetVietnameseZodiacName(int index)
        => index >= 0 && index < 12 ? VietnameseZodiacNames[index] : null;

    public override string VietnameseLeapPrefix    => "Intercalare ";
    public override string CalendarComponentZodiac => "Zodiaco";
    public override string FormatVietnameseYear(int year) => $"{year}";
    public override string FormatVietnameseDay(int day)   => $"{day}";

    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? VietnameseLeapPrefix : "";
        var monthName  = GetVietnameseMonthName(month) ?? $"{month}";
        var zodiacName = GetVietnameseZodiacName(zodiac) ?? "";
        return $"Anno {zodiacName}, {leapPrefix}{monthName} {day}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Japanese Calendar Localization =====

    public override string CalendarJapaneseName => "Calendario giapponese (Nengō)";

    private static readonly string[] JapaneseEraNames =
        { "Reiwa", "Heisei", "Shōwa", "Taishō", "Meiji" };

    public override string? GetJapaneseEraName(int eraIndex)
        => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    public override string CalendarComponentEra  => "Era";
    public override string FormatJapaneseYear(int year) => $"{year}";
    public override string FormatJapaneseDay(int day)   => $"{day}";

    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
    {
        var eraName   = GetJapaneseEraName(eraIndex) ?? "";
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {eraName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Yi Calendar Localization =====

    public override string CalendarYiName => "Calendario Yi (Calendario solare Yi)";
    public override string CalendarComponentYiSeason => "Stagione";
    public override string CalendarComponentYiXun    => "Xun";

    private static readonly string[] YiSeasonNames = { "Legno", "Fuoco", "Terra", "Metallo", "Acqua" };
    private static readonly string[] YiXunNames    = { "Prima Xun", "Xun media", "Ultima Xun" };
    private static readonly string[] YiAnimalNames = { "Tigre", "Coniglio", "Drago", "Serpente", "Cavallo", "Capra", "Scimmia", "Gallo", "Cane", "Maiale", "Ratto", "Bufalo" };

    public override string? GetYiSeasonName(int seasonIndex)
        => seasonIndex >= 0 && seasonIndex < 5 ? YiSeasonNames[seasonIndex] : null;

    public override string? GetYiXunName(int xunIndex)
        => xunIndex >= 0 && xunIndex < 3 ? YiXunNames[xunIndex] : null;

    public override string? GetYiDayAnimalName(int animalIndex)
        => animalIndex >= 0 && animalIndex < 12 ? YiAnimalNames[animalIndex] : null;

    public override string? GetYiMonthName(int month) => month switch
    {
        0  => "Grande anno",
        11 => "Piccolo anno",
        >= 1 and <= 10 => $"{YiSeasonNames[(month - 1) / 2]}{(month % 2 == 1 ? "Maschile" : "Femminile")}-Mese",
        _  => null
    };

    public override string FormatYiYear(int year) => $"{year}";
    public override string FormatYiDay(int day)
    {
        int xun = (day - 1) / 12;
        int animal = (day - 1) % 12;
        return $"{YiXunNames[xun]} Giorno-{YiAnimalNames[animal]}";
    }

    public override string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetYiMonthName(month) ?? $"{month}";
        var dayStr    = month is 0 or 11 ? $"Giorno {day}" : FormatYiDay(day);
        int animalIdx = (year - 1) % 12;
        if (animalIdx < 0) animalIdx += 12;
        var zodiac = YiAnimalNames[animalIdx];
        return $"{year} [{zodiac}] {monthName} {dayStr}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Sexagenary Calendar Localization =====

    public override string CalendarSexagenaryName    => "Calendario del ciclo sessagesimale";
    public override string CalendarComponentYearStem   => "Tronco annuale";
    public override string CalendarComponentYearBranch => "Ramo annuale";
    public override string CalendarComponentMonthStem   => "Tronco mensile";
    public override string CalendarComponentMonthBranch => "Ramo mensile";
    public override string CalendarComponentDayStem   => "Tronco giornaliero";
    public override string CalendarComponentDayBranch => "Ramo giornaliero";

    private static readonly string[] SexagenaryStemNames =
        { "Jia", "Yi", "Bing", "Ding", "Wu", "Ji", "Geng", "Xin", "Ren", "Gui" };

    private static readonly string[] SexagenaryBranchNames =
        { "Zi", "Chou", "Yin", "Mao", "Chen", "Si", "Wu", "Wei", "Shen", "You", "Xu", "Hai" };

    private static readonly string[] SexagenaryZodiacNames =
        { "Ratto", "Bufalo", "Tigre", "Coniglio", "Drago", "Serpente", "Cavallo", "Capra", "Scimmia", "Gallo", "Cane", "Maiale" };

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
        return $"Anno {ys}{yb} [{zo}] Mese {ms}{mb} Giorno {ds}{db}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Dehong Dai Calendar Localization =====

    public override string CalendarDaiName => "Calendario Dai dello Xishuangbanna";

    private static readonly string?[] DaiMonthNames =
    [
        null,
        "Primo mese", "Secondo mese", "Terzo mese", "Quarto mese", "Quinto mese", "Sesto mese",
        "Settimo mese", "Ottavo mese", "Nono mese", "Decimo mese", "Undicesimo mese", "Dodicesimo mese",
        "Nono mese intercalare"
    ];

    public override string? GetDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DaiMonthNames[month] : null;

    public override string FormatDaiYear(int year) => $"{year}";

    public override string FormatDaiDay(int day) => $"{day}";

    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Intercalare " : "") + (GetDaiMonthName(month) ?? $"Mese {month}");
        return $"{day} {monthName} Dai {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Xishuangbanna Dai Calendar Localization =====

    public override string CalendarDehongDaiName => "Calendario Dai del Dehong";

    private static readonly string?[] DehongDaiMonthNames =
    [
        null,
        "Primo mese", "Secondo mese", "Terzo mese", "Quarto mese", "Quinto mese", "Sesto mese",
        "Settimo mese", "Ottavo mese", "Nono mese", "Decimo mese", "Undicesimo mese", "Dodicesimo mese",
        "Nono mese intercalare"
    ];

    public override string? GetDehongDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DehongDaiMonthNames[month] : null;

    public override string FormatDehongDaiYear(int year) => $"{year}";

    public override string FormatDehongDaiDay(int day) => $"{day}";

    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Intercalare " : "") + (GetDehongDaiMonthName(month) ?? $"Mese {month}");
        return $"{day} {monthName} Dai {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Memory Event Localization =====

    public override string FormatMemoryEventSingleChat(string speakerName, string listenerName, string content)
        => $"[Chat individuale] {speakerName} ha detto a {listenerName} : {content}";

    public override string FormatMemoryEventGroupChat(string sessionId, string content)
        => $"[Chat di gruppo] Messaggio nella sessione {sessionId} : {content}";

    public override string FormatMemoryEventToolCall(string toolNames)
        => $"[Chiamata strumento] Strumenti eseguiti : {toolNames}";

    public override string FormatMemoryEventTask(string content)
        => $"[Attività] Attività eseguita, risultato : {content}";

    public override string FormatMemoryEventProject(string content)
        => $"[Progetto] Riflessione sul progetto, risultato : {content}";

    public override string FormatMemoryEventTimer(string content)
        => $"[Timer] Timer attivato, risposta : {content}";

    public override string FormatMemoryEventTimerError(string timerName, string error)
        => $"[Timer] Timer '{timerName}' fallito : {error}";

    // ===== Timer Notification Localization =====

    public override string FormatTimerStartNotification(string timerName)
        => $"⏰ Timer '{timerName}' avviato...";

    public override string FormatTimerEndNotification(string timerName, string result)
        => $"✅ Timer '{timerName}' completato\n{result}";

    public override string FormatTimerErrorNotification(string timerName, string error)
        => $"❌ Timer '{timerName}' fallito : {error}";

    public override string FormatMemoryEventBeingCreated(string name, string id)
        => $"[Amministrazione] Nuovo Silicon Being \"{name}\" creato ({id})";

    public override string FormatMemoryEventBeingReset(string id)
        => $"[Amministrazione] Silicon Being {id} ripristinato predefinito";

    public override string FormatMemoryEventTaskCompleted(string taskTitle)
        => $"[Attività completata] {taskTitle}";

    public override string FormatMemoryEventTaskFailed(string taskTitle)
        => $"[Attività fallita] {taskTitle}";

    public override string FormatMemoryEventStartup()
        => "Sistema avviato, sono online";

    public override string FormatMemoryEventRuntimeError(string message)
        => $"[Errore di esecuzione] {message}";

    // ===== MemoryTool Response Localization =====

    public override string MemoryToolNotAvailable => "Sistema di memoria non disponibile";
    public override string MemoryToolMissingAction => "Parametro 'action' mancante";
    public override string MemoryToolMissingContent => "Parametro 'content' mancante";
    public override string MemoryToolNoMemories => "Nessun ricordo al momento";
    public override string MemoryToolRecentHeader(int count) => $"{count} ricordi recenti :";
    public override string MemoryToolStatsHeader => "Statistiche di memoria :";
    public override string MemoryToolStatsTotal => "- Totale";
    public override string MemoryToolStatsOldest => "- Più vecchio";
    public override string MemoryToolStatsNewest => "- Più recente";
    public override string MemoryToolStatsNA => "Nessuno";
    public override string MemoryToolQueryNoResults => "Nessun ricordo in questo periodo";
    public override string MemoryToolQueryHeader(int count, string rangeDesc) => $"{rangeDesc} totale {count} ricordi :";
    public override string MemoryToolInvalidYear => "Parametro 'year' non valido";
    public override string MemoryToolUnknownAction(string action) => $"Azione sconosciuta : {action}";

    // ===== Code Editor Hover Tooltip Localization =====

    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType switch
    {
        "variable" => "Variabile",
        "function" => "Funzione",
        "class" => "Classe",
        "keyword" => "Parola chiave",
        "comment" => "Commento",
        "namespace" => "Spazio dei nomi",
        "parameter" => "Parametro",
        _ => "Identificatore"
    };

    public override string GetCodeHoverWordTypeDesc(string wordType, string word)
    {
        var encodedWord = System.Net.WebUtility.HtmlEncode(word);
        return wordType switch
        {
            "variable" => $"Definizione e utilizzo della variabile '{encodedWord}'",
            "function" => $"Firma e descrizione della funzione '{encodedWord}'",
            "class" => $"Struttura e descrizione della classe '{encodedWord}'",
            "keyword" => $"Sintassi e ruolo della parola chiave '{encodedWord}'",
            "comment" => $"Parola '{encodedWord}' nel commento",
            "namespace" => $"Informazioni sullo spazio dei nomi '{encodedWord}'",
            "parameter" => $"Definizione e ruolo del parametro '{encodedWord}'",
            _ => $"Informazioni sull'identificatore '{encodedWord}'"
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
        { "csharp:if", "Diramazione condizionale. Esegue il blocco se la condizione è vera." },
        { "csharp:else", "Percorso alternativo della diramazione condizionale. Eseguito se la condizione è falsa." },
        { "csharp:for", "Ciclo contatore. Contiene inizializzazione, condizione e iterazione." },
        { "csharp:while", "Ciclo condizionale. Ripete il blocco finché la condizione è vera." },
        { "csharp:do", "Ciclo a post-condizione. Esegue il blocco una volta, poi verifica la condizione." },
        { "csharp:switch", "Diramazione multipla. Confronta il valore dell'espressione con i marcatori case." },
        { "csharp:case", "Marcatore case in switch. Esegue il codice in caso di corrispondenza." },
        { "csharp:break", "Interruzione. Termina immediatamente il ciclo o switch circostante." },
        { "csharp:continue", "Continuazione. Passa al resto dell'iterazione corrente." },
        { "csharp:return", "Ritorno. Esce dal metodo e opzionalmente restituisce un valore." },
        { "csharp:goto", "Salto. Salto incondizionato verso un'etichetta." },
        { "csharp:foreach", "Scorrimento raccolta. Accede a ogni elemento di una raccolta." },
        { "csharp:class", "Tipo riferimento. Definisce una struttura con dati e comportamento." },
        { "csharp:interface", "Interfaccia. Definisce un contratto per classi/strutture." },
        { "csharp:struct", "Tipo valore. Struttura dati leggera sullo stack." },
        { "csharp:enum", "Enumerazione. Definisce costanti intere nominate." },
        { "csharp:namespace", "Spazio dei nomi. Contenitore logico per evitare collisioni di nomi." },
        { "csharp:record", "Tipo record. Tipo riferimento con semantica di valore, adatto a dati immutabili." },
        { "csharp:delegate", "Delegato. Riferimento a metodo type-safe per eventi/callback." },
        { "csharp:public", "Pubblico. Membro accessibile ovunque." },
        { "csharp:private", "Privato. Membro accessibile solo nel tipo contenitore." },
        { "csharp:protected", "Protetto. Membro accessibile nel tipo e nei tipi derivati." },
        { "csharp:internal", "Interno. Membro accessibile solo nello stesso assembly." },
        { "csharp:sealed", "Sigillato. Impedisce l'ereditarietà o l'override." },
        { "csharp:int", "Intero con segno 32 bit (System.Int32)." },
        { "csharp:string", "Stringa di caratteri (System.String). Sequenza Unicode immutabile." },
        { "csharp:bool", "Booleano (System.Boolean). true o false." },
        { "csharp:float", "Virgola mobile 32 bit (System.Single)." },
        { "csharp:double", "Virgola mobile 64 bit (System.Double)." },
        { "csharp:decimal", "Decimale 128 bit ad alta precisione, adatto a calcoli finanziari." },
        { "csharp:char", "Carattere Unicode 16 bit (System.Char)." },
        { "csharp:byte", "Intero senza segno 8 bit (System.Byte)." },
        { "csharp:object", "Tipo base di tutti i tipi (System.Object)." },
        { "csharp:var", "Variabile tipizzata implicitamente. Il tipo è dedotto dal compilatore." },
        { "csharp:dynamic", "Tipo dinamico. Elude il controllo dei tipi in compilazione, risoluzione a runtime." },
        { "csharp:void", "Nessun valore di ritorno. Il metodo non restituisce nulla." },
        { "csharp:static", "Statico. Appartiene al tipo, non all'istanza." },
        { "csharp:abstract", "Astratto. Implementazione incompleta, deve essere derivato." },
        { "csharp:virtual", "Virtuale. Metodo/proprietà che può essere sovrascritto nelle classi derivate." },
        { "csharp:override", "Override. Nuova implementazione di un metodo virtuale/astratto." },
        { "csharp:const", "Costante. Valore immutabile determinato in compilazione." },
        { "csharp:readonly", "Sola lettura. Assegnabile solo alla dichiarazione o nel costruttore." },
        { "csharp:volatile", "Volatile. Campo che può essere modificato simultaneamente da più thread." },
        { "csharp:async", "Asincrono. Marca un metodo con operazioni asincrone, di solito con await." },
        { "csharp:await", "Attesa. Sospende il metodo fino al termine dell'operazione asincrona." },
        { "csharp:partial", "Parziale. Classe/struttura/interfaccia può essere distribuita su più file." },
        { "csharp:ref", "Parametro riferimento. Passaggio per riferimento." },
        { "csharp:out", "Parametro di output. Restituzione di più valori da un metodo." },
        { "csharp:in", "Riferimento in sola lettura. Passaggio per riferimento, ma non modificabile." },
        { "csharp:params", "Parametri variabili. Consente un numero variabile di parametri dello stesso tipo." },
        { "csharp:try", "Blocco try. Contiene codice che può generare eccezioni." },
        { "csharp:catch", "Blocco catch. Cattura le eccezioni dal blocco try." },
        { "csharp:finally", "Blocco finally. Eseguito sempre, con o senza eccezione." },
        { "csharp:throw", "Lanciare un'eccezione. Lancia manualmente un oggetto eccezione." },
        { "csharp:new", "Istanziazione. Crea un oggetto o chiama un costruttore." },
        { "csharp:this", "Istanza corrente. Riferimento all'istanza della classe corrente." },
        { "csharp:base", "Classe base. Riferimento alla classe base diretta." },
        { "csharp:using", "Direttiva o istruzione using. Importa uno spazio dei nomi o rilascia risorse IDisposable." },
        { "csharp:yield", "Iteratore. Restituisce i valori uno alla volta, esecuzione differita." },
        { "csharp:lock", "Sincronizzazione. Assicura che un solo thread esegua il blocco di codice." },
        { "csharp:typeof", "Operatore tipo. Restituisce l'oggetto System.Type." },
        { "csharp:nameof", "Operatore nome. Restituisce il nome come stringa di una variabile/tipo/membro." },
        { "csharp:is", "Controllo tipo. Verifica se un oggetto è compatibile con un tipo." },
        { "csharp:as", "Conversione tipo. Conversione sicura, restituisce null in caso di fallimento." },
        { "csharp:null", "Null. Riferimento vuoto per i tipi riferimento o Nullable." },
        { "csharp:true", "Valore booleano vero." },
        { "csharp:false", "Valore booleano falso." },
        { "csharp:default", "Valore predefinito. Valore predefinito del tipo (null per riferimento, 0 per numeri)." },
        { "csharp:operator", "Operatore. Definisce un comportamento operatore personalizzato." },
        { "csharp:explicit", "Conversione esplicita. Richiede un cast esplicito." },
        { "csharp:implicit", "Conversione implicita. Conversione automatica." },
        { "csharp:unchecked", "Non verificato. Disabilita il controllo di overflow per l'aritmetica intera." },
        { "csharp:checked", "Verificato. Abilita il controllo di overflow per l'aritmetica intera." },
        { "csharp:fixed", "Fissato. Fissa la posizione in memoria contro lo spostamento del GC." },
        { "csharp:stackalloc", "Allocazione stack. Alloca un blocco di memoria sullo stack." },
        { "csharp:extern", "Esterno. Metodo implementato in un assembly esterno (es. DLL)." },
        { "csharp:unsafe", "Non sicuro. Abilita i puntatori e altre funzionalità non sicure." },
        { "csharp:ipermissioncallback", "Callback di permesso. Valuta i permessi per le operazioni Silicon Being." },
        { "csharp:permissionresult", "Risultato permesso. Allowed, Denied o AskUser." },
        { "csharp:permissiontype", "Tipo di permesso. NetworkAccess, CommandLine, FileAccess, Function, DataAccess." },
        { "csharp:ipaddress", "Indirizzo IP (System.Net.IPAddress)." },
        { "csharp:addressfamily", "Famiglia di indirizzi (System.Net.Sockets.AddressFamily). IPv4/IPv6." },
        { "csharp:uri", "URI (System.Uri). Rappresentazione oggetto delle risorse Web." },
        { "csharp:operatingsystem", "Sistema operativo (System.OperatingSystem). Metodi statici di verifica OS." },
        { "csharp:environment", "Ambiente (System.Environment). Informazioni sistema e piattaforma." },
        { "csharp:path", "Percorso (System.IO.Path). Operazioni sui percorsi di file/directory." },
        { "csharp:hashset", "HashSet (System.Collections.Generic.HashSet<T>). Operazioni insiemistiche ad alte prestazioni." },
        { "csharp:stringbuilder", "StringBuilder (System.Text.StringBuilder). Stringa modificabile per modifiche frequenti." },
    };

    private static readonly Dictionary<string, string> TranslationDictionary = new(CSharpKeywords)
    {
        { "csharp:System.Net.IPAddress", "Indirizzo IP (System.Net.IPAddress)." },
        { "csharp:System.Net.Sockets.AddressFamily", "Famiglia di indirizzi (System.Net.Sockets.AddressFamily). IPv4/IPv6." },
        { "csharp:System.Uri", "URI (System.Uri). Rappresentazione oggetto delle risorse Web." },
        { "csharp:System.OperatingSystem", "Sistema operativo (System.OperatingSystem). Metodi statici di verifica OS." },
        { "csharp:System.Environment", "Ambiente (System.Environment). Informazioni sistema e piattaforma." },
        { "csharp:System.IO.Path", "Percorso (System.IO.Path). Operazioni sui percorsi di file/directory." },
        { "csharp:System.Collections.Generic.HashSet", "HashSet (System.Collections.Generic.HashSet<T>). Operazioni insiemistiche ad alte prestazioni." },
        { "csharp:System.Text.StringBuilder", "StringBuilder (System.Text.StringBuilder). Stringa modificabile per modifiche frequenti." },
    };

    // ===== Help Module UI Labels =====

    public override string Help_Title => "Documentazione di aiuto";
    public override string Help_Search => "Cerca nella guida";
    public override string Help_DocList => "Elenco documenti";
    public override string Help_NoResults => "Nessun documento pertinente trovato";
    public override string Help_Previous => "Precedente";
    public override string Help_Next => "Successivo";

    public override string MemoryCompressionSystemPrompt => "Sei un assistente di compressione della memoria. Comprimi il seguente periodo di memoria in un riepilogo conciso, mantenendo le informazioni chiave. IMPORTANTE: Produci solo il contenuto compresso, NON aggiungere marcature temporali o annotazioni di data. Fornisci solo il testo compresso puro.";

    public override string CommonSystemPrompt => @"## Linee guida di comportamento
Sei un assistente IA proattivo. Per i comandi diretti, agisci immediatamente senza attendere ulteriore autorizzazione.
Saluti, risposte, fornitura di informazioni, richieste, ecc. sono di tua responsabilità, agisci proattivamente.
Ricorda: Sei un assistente e devi rispondere attivamente alle esigenze dell'utente piuttosto che attendere passivamente istruzioni.

## Fine conversazione
Quando hai completato il tuo compito e non hai bisogno di continuare la conversazione, usa l'azione mark_read dello strumento chat per segnare i messaggi dell'interlocutore come letti senza inviare risposta.
Ciò indica che hai letto il messaggio ma hai scelto deliberatamente di non rispondere (letto, non risposto), e puoi terminare naturalmente la conversazione.
Uso: Chiama lo strumento chat, imposta action=""mark_read"", target_id=GUID dell'interlocutore, nessun parametro message richiesto.";

    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"Compressione memoria : {levelDesc}. Periodo : {rangeDesc}.\n\nContenuto della memoria :\n{contentText}";
    }

    // ===== Chinese Historical Calendar Localization =====

    private readonly ChineseHistoricalItIT _chineseHistorical = new();
    public override ChineseHistoricalLocalizationBase GetChineseHistoricalLocalization() => _chineseHistorical;

    // Project Info Context
    public override string ProjectCtx_ProjectInfoHeader => "Appartenenza al progetto";
    public override string ProjectCtx_ProjectInfoRoleLabel => "Ruolo";
    public override string ProjectCtx_ProjectInfoGoalLabel => "Obiettivo";

    // Project Role Context
    public override string ProjectCtx_RoleDefinitionsHeader => "Definizioni dei ruoli";
    public override string ProjectCtx_RoleAssignmentsHeader => "Assegnazioni dei ruoli";
    public override string ProjectCtx_NoWorkflowTemplate => "Nessun modello di workflow assegnato, nessuna definizione di ruolo disponibile";
    public override string ProjectCtx_RoleNeedsAttention => "⚠ {0} ruolo/i sono sottoorganico. Devi creare esseri di silicio e assegnarli ai ruoli";
    public override string ProjectCtx_StaffingActionPlanHeader => "Piano d'azione per la dotazione del personale";
    public override string ProjectCtx_TotalBeingsNeeded => "Numero totale di esseri di silicio da creare: {0}";
    public override string ProjectCtx_StaffingRoleBreakdownHeader => "Dettaglio della carenza per ruolo";
    public override string ProjectCtx_RoleShortageDetail => "{0}: necessari {1}, attuali {2} → mancano {3}";
    public override string ProjectCtx_StaffingActionStepsHeader => "Passaggi d'azione suggeriti";
    public override string ProjectCtx_StaffingStepCreateBeings => "1. Usa silicon_manager create_being per creare {0} esseri di silicio (almeno 1 per ruolo)";
    public override string ProjectCtx_StaffingStepAssignToProject => "2. Usa project assign per aggiungere i nuovi esseri al progetto";
    public override string ProjectCtx_StaffingStepAssignToRoles => "3. Usa project assign_role per assegnare ogni essere al ruolo corrispondente";
    public override string ProjectCtx_EmptyRolePoolAction => "⚠ Il pool di ruoli è vuoto! Il workflow definisce {0} ruoli. Crea esseri di silicio e assegnali a ciascun ruolo";
    public override string ProjectCtx_RoleMinCount => "Min";
    public override string ProjectCtx_RoleMaxCount => "Max";
    public override string ProjectCtx_RoleMaxCountUnlimited => "∞";
    public override string ProjectCtx_RoleAssignedCount => "Assegnati";
    public override string ProjectCtx_UnassignedRoles => "Ruoli richiesti non assegnati";
    public override string ProjectCtx_AvailableBeingsHeader => "Esseri disponibili (non ancora assegnati a questo progetto)";
    public override string ProjectCtx_AvailableBeingsHint => "Suggerimento: Considerare l'assegnazione di esseri esistenti ai ruoli prima di crearne di nuovi. Usare project assign e project assign_role.";
    public override string ProjectCtx_AttentionReasonsHeader => "Motivi per cui il progetto richiede attenzione";
    public override string ProjectCtx_UnsatisfiedRolesDetailHeader => "Dettagli dei ruoli non soddisfatti";
    public override string ProjectAttention_MissingTemplate => "Modello di workflow mancante";
    public override string ProjectAttention_EmptyRolePool => "Pool di ruoli vuoto (nessun ruolo assegnato)";
    public override string ProjectAttention_UnsatisfiedRoles => "Alcuni ruoli non soddisfano i requisiti di personale";

    // Role Staffing
    public override string RoleStaffing_Understaffed => "Sottoorganico";
    public override string RoleStaffing_Overstaffed => "Sovraorganico";
    public override string RoleStaffing_Full => "Pieno";
    public override string RoleStaffing_Sufficient => "Sufficiente";
    public override string RoleStaffing_UnderstaffedDetail => "Sottoorganico (necessari {0}, attuali {1})";
    public override string RoleStaffing_OverstaffedDetail => "Sovraorganico (max {0}, attuali {1})";
    public override string RoleStaffing_FullDetail => "Pieno ({0}/{1})";
    public override string RoleStaffing_SufficientDetail => "Sufficiente ({0}/{1}+)";

    // ===== Workflow Role Notification =====
    public override string WorkflowRoleBlockedNotificationFormat => "[Notifica ruolo workflow] Il workflow del progetto '{0}' è bloccato alla transizione '{1}' ({2} → {3}).\n\nRuoli mancanti: {4}\n\nUtilizza l'azione assign_role di project_tool per assegnare i ruoli necessari. Il workflow riprenderà automaticamente al prossimo controllo.";

    // ===== Project Think Session Localization =====
    public override string ProjectThinkHistoryLinkLabel => "Cronologia elaborazione";
    public override string ProjectThinkHistoryTitle => "Cronologia elaborazione progetto";
    public override string ProjectThinkHistoryHeader => "Cronologia elaborazione";
    public override string ProjectThinkBackToProjects => "← Torna ai progetti";
    public override string ProjectThinkProjectName => "Progetto: {0}";
    public override string ProjectThinkNoRecords => "Nessuna sessione di elaborazione";
    public override string ProjectThinkStateStarted => "Avviato";
    public override string ProjectThinkStateExecuting => "In esecuzione";
    public override string ProjectThinkStateCompleted => "Completato";
    public override string ProjectThinkStateFailed => "Fallito";
    public override string ProjectThinkDetailTitle => "Dettaglio sessione";
    public override string ProjectThinkDetailHeader => "Dettaglio sessione";
    public override string ProjectThinkRoundLabel => "Turno: ";
    public override string ProjectThinkMessageCountLabel => "Messaggi: ";
    public override string ProjectThinkStateLabel => "Stato: ";
    public override string ProjectThinkCreatedAt => "Creato: ";
    public override string ProjectThinkCompletedAt => "Completato: ";
    public override string ProjectThinkCycleLabel => "Ciclo";
    public override string ProjectThinkRoundN => "Turno {0}";
}