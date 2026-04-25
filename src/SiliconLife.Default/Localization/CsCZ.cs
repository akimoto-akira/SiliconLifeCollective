// Copyright (c) 2024 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

using SiliconLife.Collective;
using SiliconLife.Default.ChineseHistorical;

namespace SiliconLife.Default;

/// <summary>
/// Czech (Czech Republic) localization
/// </summary>
public class CsCZ : DefaultLocalizationBase
{
    public override string LanguageCode => "cs-CZ";
    public override string LanguageName => "Čeština (Česká republika)";

    // ===== Basic UI =====
    public override string WelcomeMessage => "Vítejte v Silicon Life Collective!";
    public override string BrandName => "Silicon Life Collective";
    public override string InputPrompt => "Zadejte příkaz...";
    public override string ShutdownMessage => "Vypínání...";
    public override string ThinkingMessage => "Přemýšlím...";
    public override string ToolCallMessage => "Volám nástroje...";
    public override string ErrorMessage => "Chyba";
    public override string UnexpectedErrorMessage => "Došlo k neočekávané chybě";
    public override string PermissionDeniedMessage => "Přístup odepřen";
    public override string PermissionAskPrompt => "Požadavek na oprávnění";

    // ===== Config Messages =====
    public override string ConfigCorruptedError => "Konfigurační soubor je poškozen";
    public override string ConfigCreatedWithDefaults => "Konfigurační soubor vytvořen s výchozími hodnotami";
    public override string AIConnectionError => "Nepodařilo se připojit k AI klientovi";
    public override string AIRequestError => "Požadavek AI selhal";
    public override string DataDirectoryCreateError => "Nepodařilo se vytvořit adresář dat";

    // ===== Permission System =====
    public override string PermissionRequestHeader => "Žádost o oprávnění";
    public override string PermissionRequestDescription => "Prosím zkontrolujte následující požadavek na oprávnění";
    public override string PermissionRequestTypeLabel => "Typ oprávnění";
    public override string PermissionRequestResourceLabel => "Požadovaný zdroj";
    public override string PermissionRequestAllowButton => "Povolit";
    public override string PermissionRequestDenyButton => "Odmítnout";
    public override string PermissionRequestCacheLabel => "Rozhodnutí uložit do mezipaměti";
    public override string PermissionRequestDurationLabel => "Doba trvání";
    public override string PermissionRequestWaitingMessage => "Čekání na odpověď uživatele...";
    public override string AllowCodeLabel => "Kód pro povolení";
    public override string DenyCodeLabel => "Kód pro odmítnutí";
    public override string PermissionReplyInstruction => "Odpovězte kódem pro povolení nebo odmítnutí";
    public override string AddToCachePrompt => "Uložit toto rozhodnutí do mezipaměti?";
    public override string PermissionCacheLabel => "Oprávnění do mezipaměti";
    public override string PermissionCacheDurationLabel => "Doba trvání mezipaměti";
    public override string PermissionCacheDuration1Hour => "1 hodina";
    public override string PermissionCacheDuration24Hours => "24 hodin";
    public override string PermissionCacheDuration7Days => "7 dní";
    public override string PermissionCacheDuration30Days => "30 dní";
    public override string PermissionDialogTitle => "Žádost o oprávnění";
    public override string PermissionTypeLabel => "Typ";
    public override string PermissionResourceLabel => "Zdroj";
    public override string PermissionDetailLabel => "Detail";
    public override string PermissionAllowButton => "Povolit";
    public override string PermissionDenyButton => "Odmítnout";
    public override string PermissionRespondFailed => "Odpověď na oprávnění selhala";
    public override string PermissionRespondError => "Chyba při odpovídání na oprávnění";

    // ===== Init Page =====
    public override string InitPageTitle => "Inicializace";
    public override string InitDescription => "Nakonfigurujte svého prvního silikonového bytí";
    public override string InitNicknameLabel => "Přezdívka";
    public override string InitNicknamePlaceholder => "Zadejte přezdívku";
    public override string InitEndpointLabel => "Koncový bod AI API";
    public override string InitEndpointPlaceholder => "http://localhost:11434";
    public override string InitAIClientTypeLabel => "Typ AI klienta";
    public override string InitModelLabel => "Výchozí model";
    public override string InitModelPlaceholder => "llama3";
    public override string InitSkinLabel => "Vzhled";
    public override string InitSkinPlaceholder => "Vyberte vzhled";
    public override string InitDataDirectoryLabel => "Adresář dat";
    public override string InitDataDirectoryPlaceholder => "Cesta k adresáři dat";
    public override string InitDataDirectoryBrowse => "Procházet";
    public override string InitSkinSelected => "již vybrán";
    public override string InitSkinPreviewTitle => "Náhled";
    public override string InitSkinPreviewCardTitle => "Ukázka karty";
    public override string InitSkinPreviewCardContent => "Toto je ukázka obsahu karty";
    public override string InitSkinPreviewPrimaryBtn => "Hlavní tlačítko";
    public override string InitSkinPreviewSecondaryBtn => "Vedlejší tlačítko";
    public override string InitSubmitButton => "Odeslat";
    public override string InitFooterHint => "Tato nastavení můžete později upravit v konfiguraci";
    public override string InitNicknameRequiredError => "Přezdívka je vyžadována";
    public override string InitDataDirectoryRequiredError => "Adresář dat je vyžadován";
    public override string InitCuratorNameLabel => "Jméno kurátora";
    public override string InitCuratorNamePlaceholder => "Zadejte jméno kurátora";
    public override string InitCuratorNameRequiredError => "Jméno kurátora je vyžadováno";
    public override string InitLanguageLabel => "Jazyk";
    public override string InitLanguageSwitchBtn => "Použít";

    // ===== Navigation Menu =====
    public override string NavMenuChat => "Chat";
    public override string NavMenuDashboard => "Panel";
    public override string NavMenuBeings => "Bytí";
    public override string NavMenuAudit => "Audit";
    public override string NavMenuTasks => "Úkoly";
    public override string NavMenuMemory => "Paměť";
    public override string NavMenuKnowledge => "Znalosti";
    public override string NavMenuProjects => "Projekty";
    public override string NavMenuLogs => "Logy";
    public override string NavMenuConfig => "Konfigurace";
    public override string NavMenuHelp => "Nápověda";
    public override string NavMenuAbout => "O aplikaci";

    // ===== Page Titles =====
    public override string PageTitleChat => "Chat";
    public override string PageTitleDashboard => "Ovládací panel";
    public override string PageTitleBeings => "Silikonová bytí";
    public override string PageTitleTasks => "Úkoly";
    public override string PageTitleTimers => "Časovače";
    public override string PageTitleMemory => "Paměť";
    public override string PageTitleWorkNotes => "Pracovní poznámky";
    public override string PageTitleKnowledge => "Znalosti";
    public override string PageTitleProjects => "Projekty";
    public override string PageTitleLogs => "Logy";
    public override string PageTitleConfig => "Konfigurace";
    public override string PageTitleAbout => "O aplikaci";
    public override string PageTitleAudit => "Audit";
    public override string PageTitleExecutor => "Executor";
    public override string PageTitleCodeBrowser => "Prohlížeč kódu";
    public override string PageTitlePermission => "Oprávnění";

    // ===== Dashboard =====
    public override string DashboardPageHeader => "Ovládací panel";
    public override string DashboardStatTotalBeings => "Celkem bytí";
    public override string DashboardStatActiveBeings => "Aktivní bytí";
    public override string DashboardStatUptime => "Doba provozu";
    public override string DashboardStatMemory => "Využití paměti";
    public override string DashboardChartMessageFrequency => "Frekvence zpráv";

    // ===== Beings Page =====
    public override string BeingsPageHeader => "Silikonová bytí";
    public override string BeingsTotalCount => "Celkem";
    public override string BeingsNoSelectionPlaceholder => "Vyberte bytí pro zobrazení detailů";
    public override string BeingsEmptyState => "Žádná bytí nebyla vytvořena";
    public override string BeingsStatusIdle => "Nečinné";
    public override string BeingsStatusRunning => "Běží";
    public override string BeingsDetailIdLabel => "ID";
    public override string BeingsDetailStatusLabel => "Stav";
    public override string BeingsDetailCustomCompileLabel => "Vlastní kompilace";
    public override string BeingsDetailSoulContentLabel => "Obsah duše";
    public override string BeingsDetailSoulContentEditLink => "Upravit";
    public override string BeingsBackToList => "Zpět na seznam";
    public override string SoulEditorSubtitle => "Editor duše";
    public override string BeingsDetailMemoryLabel => "Paměť";
    public override string BeingsDetailMemoryViewLink => "Zobrazit";
    public override string BeingsDetailPermissionLabel => "Oprávnění";
    public override string BeingsDetailPermissionEditLink => "Upravit";
    public override string BeingsDetailTimersLabel => "Časovače";
    public override string BeingsDetailTasksLabel => "Úkoly";
    public override string BeingsDetailAIClientLabel => "AI klient";
    public override string BeingsDetailAIClientEditLink => "Upravit";
    public override string BeingsDetailChatHistoryLink => "Historie chatu";
    public override string BeingsDetailWorkNoteLabel => "Pracovní poznámka";
    public override string BeingsDetailWorkNoteLink => "Zobrazit";
    public override string WorkNotePageTitle => "Pracovní poznámka";
    public override string WorkNotePageHeader => "Pracovní poznámka";
    public override string WorkNotePageDescription => "Poznámky pro toto bytí";
    public override string WorkNotesPageHeader => "Pracovní poznámky";
    public override string WorkNotesTotalPages => "Celkem stránek";
    public override string WorkNotesSearchPlaceholder => "Hledat poznámky...";
    public override string WorkNotesSearchButton => "Hledat";
    public override string WorkNotesEmptyState => "Žádné pracovní poznámky";
    public override string WorkNotesNoSearchResults => "Žádné výsledky hledání";
    public override string BeingsDetailChatHistoryLabel => "Historie chatu";
    public override string ChatHistoryPageTitle => "Historie chatu";
    public override string ChatHistoryPageHeader => "Historie chatu";
    public override string ChatHistoryConversationList => "Seznam konverzací";
    public override string ChatHistoryBackToList => "Zpět na seznam";
    public override string ChatHistoryNoConversations => "Žádné konverzace";
    public override string ChatDetailPageTitle => "Detail chatu";
    public override string ChatDetailPageHeader => "Detail chatu";
    public override string ChatDetailNoMessages => "Žádné zprávy";
    public override string BeingsYes => "Ano";
    public override string BeingsNo => "Ne";
    public override string BeingsNotSet => "Nenastaveno";

    // ===== Timers Page =====
    public override string TimersPageHeader => "Časovače";
    public override string TimersTotalCount => "Celkem";
    public override string TimersEmptyState => "Žádné časovače";
    public override string TimerViewExecutionHistory => "Zobrazit historii";
    public override string TimerExecutionHistoryTitle => "Historie spuštění";
    public override string TimerExecutionHistoryHeader => "Historie spuštění časovače";
    public override string TimerExecutionBackToTimers => "Zpět na časovače";
    public override string TimerExecutionTimerName => "Název";
    public override string TimerExecutionDetailTitle => "Detail spuštění";
    public override string TimerExecutionDetailHeader => "Detail spuštění";
    public override string TimerExecutionNoRecords => "Žádné záznamy";
    public override string TimersStatusActive => "Aktivní";
    public override string TimersStatusPaused => "Pozastaveno";
    public override string TimersStatusTriggered => "Spuštěno";
    public override string TimersStatusCancelled => "Zrušeno";
    public override string TimersTypeRecurring => "Opakující se";
    public override string TimersTriggerTimeLabel => "Čas spuštění";
    public override string TimersIntervalLabel => "Interval";
    public override string TimersCalendarLabel => "Kalendář";
    public override string TimersTriggeredCountLabel => "Počet spuštění";

    // ===== Chat Page =====
    public override string ChatConversationsHeader => "Konverzace";
    public override string ChatNoConversationSelected => "Není vybrána žádná konverzace";
    public override string ChatMessageInputPlaceholder => "Napište zprávu...";
    public override string ChatLoading => "Načítání...";
    public override string ChatSendButton => "Odeslat";
    public override string ChatFileSourceDialogTitle => "Vybrat zdroj souboru";
    public override string ChatFileSourceServerFile => "Serverový soubor";
    public override string ChatFileSourceUploadLocal => "Nahrát místní soubor";
    public override string ChatUserDisplayName => "Uživatel";
    public override string ChatUserAvatarName => "Uživatel";
    public override string ChatDefaultBeingName => "Bytí";
    public override string ChatThinkingSummary => "Přemýšlení";

    // ===== About Page =====
    public override string AboutPageHeader => "O aplikaci";
    public override string AboutAppName => "Silicon Life Collective";
    public override string AboutVersionLabel => "Verze";
    public override string AboutDescription => "Platforma pro správu silikonových bytí";
    public override string AboutAuthorLabel => "Autor";
    public override string AboutAuthorName => "Hoshino Kennji";
    public override string AboutLicenseLabel => "Licence";
    public override string AboutCopyright => "Copyright © 2024 Silicon Life Collective";
    public override string AboutSocialMediaLabel => "Sociální média";

    // ===== Memory Page =====
    public override string MemoryPageHeader => "Paměť";
    public override string MemoryEmptyState => "Žádné paměti";
    public override string MemorySearchPlaceholder => "Hledat paměti...";
    public override string MemorySearchButton => "Hledat";
    public override string MemoryFilterDateFrom => "Od";
    public override string MemoryFilterDateTo => "Do";
    public override string MemoryFilterOriginalOnly => "Pouze originály";
    public override string MemoryTypeChat => "Chat";
    public override string MemoryTypeTask => "Úkol";
    public override string MemoryTypeTimer => "Časovač";
    public override string MemoryDetailClose => "Zavřít";
    public override string MemoryStatTotal => "Celkem";
    public override string MemoryStatOldest => "Nejstarší";
    public override string MemoryStatNewest => "Nejnovější";
    public override string MemoryCardViewDetail => "Zobrazit detail";
    public override string MemoryToolNotAvailable => "Nástroj paměti není k dispozici";
    public override string MemoryToolMissingAction => "Chybějící akce";
    public override string MemoryToolInvalidYear => "Neplatný rok";
    public override string MemoryToolStatsNA => "N/A";
    public override string MemoryToolQueryHeader(int count, string query) => $"Nalezeno {count} výsledků pro: {query}";
    public override string MemoryToolQueryNoResults => "Žádné výsledky";
    public override string MemoryToolUnknownAction(string action) => $"Neznámá akce: {action}";

    // ===== Tasks Page =====
    public override string TasksPageHeader => "Úkoly";
    public override string TasksEmptyState => "Žádné úkoly";
    public override string TasksStatusPending => "Čeká";
    public override string TasksStatusRunning => "Běží";
    public override string TasksStatusCompleted => "Dokončeno";
    public override string TasksStatusFailed => "Selhalo";
    public override string TasksAssignedToLabel => "Přiřazeno";
    public override string TasksCreatedAtLabel => "Vytvořeno";

    // ===== Logs Page =====
    public override string LogsPageHeader => "Logy";
    public override string LogsTotalCount => "Celkem";
    public override string LogsEmptyState => "Žádné logy";
    public override string LogsStartTime => "Čas";
    public override string LogsBeingFilter => "Filtr bytí";
    public override string LogsAllBeings => "Všechna bytí";

    // ===== Config Page =====
    public override string ConfigPageHeader => "Konfigurace";
    public override string ConfigSaveButton => "Uložit";
    public override string ConfigCancelButton => "Zrušit";
    public override string ConfigSaveFailed => "Uložení konfigurace selhalo";
    public override string ConfigActionLabel => "Akce";
    public override string ConfigPropertyNameLabel => "Název vlastnosti";
    public override string ConfigPropertyValueLabel => "Hodnota";
    public override string ConfigEditPrefix => "Upravit";
    public override string ConfigDefaultGroupName => "Výchozí";
    public override string ConfigTimeSettingsLabel => "Nastavení času";
    public override string ConfigErrorConvertInt => "Nelze převést na celé číslo";
    public override string ConfigErrorConvertGuid => "Nelze převést na GUID";
    public override string ConfigErrorInvalidRequest => "Neplatný požadavek";
    public override string ConfigErrorInstanceNotFound => "Instance nebyla nalezena";

    // ===== Audit Page =====
    public override string AuditPageHeader => "Audit";
    public override string AuditTotalRequests => "Celkem požadavků";
    public override string AuditFilterButton => "Filtr";

    public override string AuditExport => "Export";
    public override string AuditTrendTitle => "Trend využití tokenů";
    public override string AuditTrendPrompt => "Tokeny promptu";
    public override string AuditTrendCompletion => "Tokeny dokončení";
    public override string AuditTrendTotal => "Celkem";
    public override string AuditAllClientTypes => "Všechny typy klientů";

    // ===== Calendar Components =====
    public override string CalendarComponentYear => "Rok";
    public override string CalendarComponentMonth => "Měsíc";
    public override string CalendarComponentDay => "Den";
    public override string CalendarComponentHour => "Hodina";
    public override string CalendarComponentMinute => "Minuta";
    public override string CalendarComponentSecond => "Sekunda";
    public override string CalendarComponentWeekday => "Den v týdnu";
    public override string CalendarComponentEra => "Éra";
    public override string CalendarComponentDayStem => "Nebeský kmen dne";
    public override string CalendarComponentDayBranch => "Zemská větev dne";
    public override string CalendarComponentMonthStem => "Nebeský kmen měsíce";
    public override string CalendarComponentMonthBranch => "Zemská větev měsíce";
    public override string CalendarComponentYiSeason => "Roční období Iů";
    public override string CalendarComponentYiXun => "Sün (dekáda)";

    // ===== Calendar Names =====
    public override string CalendarGregorianName => "Gregoriánský kalendář";
    public override string CalendarChineseLunarName => "Čínský lunární kalendář";
    public override string CalendarChineseHistoricalName => "Čínský historický kalendář";
    public override string CalendarJapaneseName => "Japonský kalendář";

    public override string CalendarVietnameseName => "Vietnamský kalendář";
    public override string CalendarIslamicName => "Islámský kalendář";
    public override string CalendarHebrewName => "Hebrejský kalendář";
    public override string CalendarIndianName => "Indický kalendář";
    public override string CalendarPersianName => "Perský kalendář";
    public override string CalendarCopticName => "Koptský kalendář";
    public override string CalendarEthiopianName => "Etiopský kalendář";
    public override string CalendarJulianName => "Juliánský kalendář";
    public override string CalendarMayanName => "Mayský kalendář";
    public override string CalendarJavaneseName => "Jávský kalendář";
    public override string CalendarBuddhistName => "Buddhistický kalendář";
    public override string CalendarJucheName => "Kalendář Čučche";
    public override string CalendarRocName => "Kalendář republiky Čína";
    public override string CalendarMongolianName => "Mongolský kalendář";
    public override string CalendarTibetanName => "Tibetský kalendář";
    public override string CalendarCherokeeName => "Kalendář Čerokíů";
    public override string CalendarYiName => "Kalendář Iů";
    public override string CalendarInuitName => "Kalendář Inuitů";

    public override string CalendarVikramSamvatName => "Kalendář Vikram Samvat";
    public override string CalendarSakaName => "Kalendář Šaka";
    public override string CalendarZoroastrianName => "Zoroastriánský kalendář";
    public override string CalendarFrenchRepublicanName => "Francouzský republikánský kalendář";
    public override string CalendarChulaSakaratName => "Kalendář Čula Sakarat";
    public override string CalendarKhmerName => "Khmerský kalendář";

    public override string CalendarDaiName => "Kalendář Tajů";
    public override string CalendarDehongDaiName => "Kalendář Techung Tajů";
    public override string CalendarRomanName => "Římský kalendář";
    public override string CalendarIntervalName => "Intervalový kalendář";

    // ===== Interval Calendar =====

    public override string CalendarIntervalDays => "d";
    public override string CalendarIntervalHours => "h";
    public override string CalendarIntervalMinutes => "m";
    public override string CalendarIntervalSeconds => "s";
    public override string CalendarIntervalEvery => "Každých";

    // ===== Permission Save Messages =====
    public override string PermissionSaveSuccess => "Oprávnění úspěšně uloženo";
    public override string PermissionSaveError => "Chyba při ukládání oprávnění";
    public override string PermissionSaveMissingCode => "Chybějící kód";
    public override string PermissionSaveSecurityScanFailed => "Bezpečnostní sken selhal";
    public override string PermissionSaveCompilationFailed => "Kompilace selhala";
    public override string PermissionSaveLoaderNotAvailable => "Načítač není k dispozici";
    public override string PermissionSaveRemoveSuccess => "Úspěšně odebráno";
    public override string PermissionSaveRemoveFailed => "Odebrání selhalo";

    // ===== Permission Callback =====
    public override string PermissionCallbackClassSummary => "Třída zpětného volání oprávnění";
    public override string PermissionCallbackConstructorParam => "Parametr konstruktoru";
    public override string PermissionCallbackEvaluateSummary => "Vyhodnotit oprávnění";

    // ===== Permission Editor =====
    public override string PermissionEditorSection => "Editor oprávnění";
    public override string PermissionRulesSection => "Pravidla";
    public override string PermissionTemplateHeader => "Šablona";

    // ===== Memory Filter =====
    public override string MemoryFilterTypeLabel => "Typ";
    public override string MemoryFilterSummaryOnly => "Pouze souhrny";
    public override string MemoryFilterAll => "Vše";
    public override string MemoryFilterApply => "Použít";
    public override string MemoryFilterReset => "Resetovat";
    public override string MemoryTypeToolCall => "Volání nástroje";
    public override string MemoryIsSummaryBadge => "Souhrn";

    // ===== Memory Pagination =====
    public override string MemoryPaginationNext => "Další";
    public override string MemoryPaginationPrev => "Předchozí";

    // ===== Memory Stats =====
    public override string MemoryStatKeywordFrequency => "Frekvence klíčových slov";
    public override string MemoryToolStatsOldest => "Nejstarší";

    // ===== Config Dictionary =====
    public override string ConfigDictionaryLabel => "Slovník";
    public override string ConfigDictValueLabel => "Hodnota";
    public override string ConfigDaysLabel => "Dny";
    public override string ConfigSecondsLabel => "Sekundy";
    public override string ConfigHoursLabel => "Hodiny";
    public override string ConfigMinutesLabel => "Minuty";

    // ===== Config Errors =====
    public override string ConfigErrorSaveFailed => "Uložení selhalo";
    public override string ConfigErrorPropertyNotFound => "Vlastnost nenalezena";
    public override string ConfigErrorConvertBool => "Nelze převést na boolean";
    public override string ConfigErrorConvertDouble => "Nelze převést na double";
    public override string ConfigErrorConvertDateTime => "Nelze převést na DateTime";

    // ===== Config Edit =====
    public override string ConfigEditValueLabel => "Hodnota";
    public override string ConfigEditModalTitle => "Upravit vlastnost";
    public override string ConfigDictKeyLabel => "Klíč";

    // ===== Logs =====
    public override string LogsLevelAll => "Všechny úrovně";
    public override string LogsExceptionLabel => "Výjimka";
    public override string LogsPrevPage => "Předchozí";
    public override string LogsNextPage => "Další";


    // ===== Knowledge =====
    public override string KnowledgePageHeader => "Znalosti";
    public override string KnowledgeLoadingState => "Načítání...";

    // ===== Projects =====
    public override string ProjectsPageHeader => "Projekty";

    // ===== Executor =====
    public override string ExecutorPageHeader => "Executor";

    // ===== Default Values =====
    public override string DefaultCuratorSoul => "Jsem silikonové bytí vytvořené pro pomoc lidem.";

    // ===== Memory Compression =====
    public override string MemoryCompressionSystemPrompt => "Jsi expert na kompresi paměti. Shrň následující informace.";

    // ===== Calendar Format Methods =====
    public override string FormatGregorianYear(int year) => year.ToString();
    public override string FormatGregorianDay(int day) => day.ToString();
    public override string FormatGregorianHour(int hour) => hour.ToString();
    public override string FormatGregorianMinute(int minute) => minute.ToString();
    public override string FormatGregorianSecond(int second) => second.ToString();
    public override string FormatChineseLunarYear(int year) => year.ToString();
    public override string FormatJapaneseYear(int year) => year.ToString();
    public override string FormatIslamicYear(int year) => year.ToString();
    public override string FormatHebrewYear(int year) => year.ToString();
    public override string FormatHebrewDay(int day) => day.ToString();
    public override string FormatIndianYear(int year) => year.ToString();
    public override string FormatIndianDay(int day) => day.ToString();
    public override string FormatPersianYear(int year) => year.ToString();
    public override string FormatPersianDay(int day) => day.ToString();
    public override string FormatCopticYear(int year) => year.ToString();
    public override string FormatCopticDay(int day) => day.ToString();
    public override string FormatEthiopianYear(int year) => year.ToString();
    public override string FormatEthiopianDay(int day) => day.ToString();
    public override string FormatJulianYear(int year) => year.ToString();
    public override string FormatJulianDay(int day) => day.ToString();
    public override string FormatJavaneseYear(int year) => year.ToString();
    public override string FormatJavaneseDay(int day) => day.ToString();
    public override string FormatBuddhistYear(int year) => year.ToString();
    public override string FormatBuddhistDay(int day) => day.ToString();
    public override string FormatJucheYear(int year) => year.ToString();
    public override string FormatJucheDay(int day) => day.ToString();
    public override string FormatRocYear(int year) => year.ToString();
    public override string FormatRocDay(int day) => day.ToString();
    public override string FormatMongolianYear(int year) => year.ToString();
    public override string FormatMongolianDay(int day) => day.ToString();
    public override string FormatTibetanYear(int year) => year.ToString();
    public override string FormatTibetanDay(int day) => day.ToString();
    public override string FormatCherokeeYear(int year) => year.ToString();
    public override string FormatCherokeeDay(int day) => day.ToString();
    public override string FormatYiYear(int year) => year.ToString();
    public override string FormatYiDay(int day) => day.ToString();
    public override string FormatInuitYear(int year) => year.ToString();
    public override string FormatInuitDay(int day) => day.ToString();
    public override string FormatVikramSamvatYear(int year) => year.ToString();
    public override string FormatVikramSamvatDay(int day) => day.ToString();
    public override string FormatSakaYear(int year) => year.ToString();
    public override string FormatSakaDay(int day) => day.ToString();
    public override string FormatZoroastrianYear(int year) => year.ToString();
    public override string FormatZoroastrianDay(int day) => day.ToString();
    public override string FormatFrenchRepublicanYear(int year) => year.ToString();
    public override string FormatFrenchRepublicanDay(int day) => day.ToString();
    public override string FormatChulaSakaratYear(int year) => year.ToString();
    public override string FormatChulaSakaratDay(int day) => day.ToString();
    public override string FormatKhmerYear(int year) => year.ToString();
    public override string FormatKhmerDay(int day) => day.ToString();
    public override string FormatVietnameseYear(int year) => year.ToString();
    public override string FormatVietnameseDay(int day) => day.ToString();
    public override string FormatChineseHistoricalDay(int day) => day.ToString();
    public override string FormatIslamicDay(int day) => day.ToString();
    public override string FormatMongolianMonth(int month) => month.ToString();
    public override string FormatTibetanMonth(int month) => month.ToString();
    public override string FormatJapaneseDay(int day) => day.ToString();
    public override string FormatRomanYear(int year) => year.ToString();
    public override string FormatRomanDay(int day) => day.ToString();
    public override string FormatDaiYear(int year) => year.ToString();
    public override string FormatDaiDay(int day) => day.ToString();
    public override string FormatDehongDaiYear(int year) => year.ToString();
    public override string FormatDehongDaiDay(int day) => day.ToString();

    // ===== Memory Event Formatting =====
    public override string FormatMemoryEventStartup() => "Systém spuštěn";
    public override string FormatMemoryEventBeingCreated(string name, string model) => $"Bytí '{name}' vytvořeno (model: {model})";
    public override string FormatMemoryEventBeingReset(string name) => $"Bytí '{name}' resetováno";
    public override string FormatMemoryEventSingleChat(string being, string message) => $"Jednoduchý chat: {being} - {message}";
    public override string FormatMemoryEventGroupChat(string group, string message) => $"Skupinový chat: {group} - {message}";
    public override string FormatMemoryEventTask(string task) => $"Úkol: {task}";
    public override string FormatMemoryEventTaskCompleted(string task) => $"Úkol dokončen: {task}";
    public override string FormatMemoryEventTaskFailed(string task) => $"Úkol selhal: {task}";
    public override string FormatMemoryEventToolCall(string tool) => $"Volání nástroje: {tool}";
    public override string FormatMemoryEventTimerError(string timer, string error) => $"Chyba časovače '{timer}': {error}";
    public override string FormatMemoryEventRuntimeError(string error) => $"Chyba běhu: {error}";

    // ===== Timer Notifications =====
    public override string FormatTimerEndNotification(string timerName, string taskName) => $"Časovač '{timerName}' dokončil úkol: {taskName}";
    public override string FormatTimerErrorNotification(string timerName, string error) => $"Chyba časovače '{timerName}': {error}";

    // ===== Chat Methods =====
    public override string GetChatToolCallsSummary(int count) => $"{count} volání nástrojů";
    public override string SingleChatNameFormat => "{0} - Chat";

    // ===== Permission Type =====
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType.ToString();

    // ===== Social Media =====
    public override string GetSocialMediaName(string platform) => platform;

    // ===== Config Methods =====
    public override string GetConfigDisplayName(string key, out bool isAdvanced) { isAdvanced = false; return key; }
    public override string GetConfigGroupName(string key) => "Výchozí";
    public override string GetConfigDescription(string key) => string.Empty;

    // ===== Tool Display Name =====
    public override string GetToolDisplayName(string toolName) => toolName;

    // ===== Chinese Historical Calendar =====
    private readonly ChineseHistoricalCsCZ _chineseHistorical = new();
    public override ChineseHistoricalLocalizationBase GetChineseHistoricalLocalization() => _chineseHistorical;

    // ===== Sexagenary Cycle =====
    private static readonly string[] SexagenaryStemNames =
        { "Ťia", "I", "Ping", "Ting", "Wu", "Ťi", "Keng", "Sin", "Žen", "Kuej" };
    private static readonly string[] SexagenaryBranchNames =
        { "C', Chou", "Jin", "Mao", "Čchen", "S', Wu", "Wei", "Šen", "Jou", "Sü", "Chaj" };

    public override string? GetSexagenaryStemName(int index) => index >= 0 && index < SexagenaryStemNames.Length ? SexagenaryStemNames[index] : null;
    public override string? GetSexagenaryBranchName(int index) => index >= 0 && index < SexagenaryBranchNames.Length ? SexagenaryBranchNames[index] : null;

    // ===== Calendar Localize Methods =====
    // TODO: Add correct Localize methods based on compilation errors

    // ===== Calendar Format Methods =====


    // ===== Gregorian Calendar Names =====
    private static readonly string[] GregorianMonthNames =
        { "Leden", "Únor", "Březen", "Duben", "Květen", "Červen", "Červenec", "Srpen", "Září", "Říjen", "Listopad", "Prosinec" };
    private static readonly string[] GregorianWeekdayNames =
        { "Neděle", "Pondělí", "Úterý", "Středa", "Čtvrtek", "Pátek", "Sobota" };

    public override string? GetGregorianMonthName(int month) => month >= 1 && month <= 12 ? GregorianMonthNames[month - 1] : null;
    public override string? GetGregorianWeekdayName(int day) => day >= 0 && day <= 6 ? GregorianWeekdayNames[day] : null;

    // ===== Chinese Lunar Calendar Names =====
    private static readonly string[] ChineseLunarMonthNames =
        { "První", "Druhý", "Třetí", "Čtvrtý", "Pátý", "Šestý", "Sedmý", "Osmý", "Devátý", "Desátý", "Jedenáctý", "Dvanáctý" };
    private static readonly string[] ChineseLunarDayNames =
        { "První", "Druhý", "Třetí", "Čtvrtý", "Pátý", "Šestý", "Sedmý", "Osmý", "Devátý", "Desátý" };

    public override string? GetChineseLunarMonthName(int month) => month >= 1 && month <= 12 ? ChineseLunarMonthNames[month - 1] : null;
    public override string? GetChineseLunarDayName(int day) => day >= 1 && day <= 30 ? ChineseLunarDayNames[(day - 1) % 10] : null;

    // ===== Japanese Calendar Names =====
    private static readonly string[] JapaneseEraNames =
        { "Meidži", "Taišó", "Šówa", "Heisei", "Reiwa" };

    public override string? GetJapaneseEraName(int eraIndex) => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    // ===== Islamic Calendar Names =====
    private static readonly string[] IslamicMonthNames =
        { "Muharram", "Safar", "Rabi' al-Avval", "Rabi' ath-Thani", "Džumada l-Ula", "Džumada th-Thani", "Radžab", "Ša'bán", "Ramadán", "Šavval", "Dhu l-Qa'da", "Dhu l-Hiddža" };

    public override string? GetIslamicMonthName(int month) => month >= 1 && month <= 12 ? IslamicMonthNames[month - 1] : null;

    // ===== Hebrew Calendar Names =====
    private static readonly string[] HebrewMonthNames =
        { "Tišri", "Chešvan", "Kislev", "Tevet", "Ševat", "Adar", "Nisan", "Ijar", "Sivan", "Tammuz", "Av", "Elul" };

    public override string? GetHebrewMonthName(int month) => month >= 1 && month <= 12 ? HebrewMonthNames[month - 1] : null;

    // ===== Indian Calendar Names =====
    private static readonly string[] IndianMonthNames =
        { "Čaitra", "Vaisákha", "Džjaištha", "Ášádha", "Šrávana", "Bhádrapada", "Ášvina", "Kártika", "Márgaširša", "Pauša", "Mágha", "Phálguna" };

    public override string? GetIndianMonthName(int month) => month >= 1 && month <= 12 ? IndianMonthNames[month - 1] : null;

    // ===== Persian Calendar Names =====
    private static readonly string[] PersianMonthNames =
        { "Farvardin", "Ordibehešt", "Chordád", "Tir", "Mordád", "Šahrivár", "Mehr", "Ábán", "Ázar", "Dej", "Bahman", "Esfand" };

    public override string? GetPersianMonthName(int month) => month >= 1 && month <= 12 ? PersianMonthNames[month - 1] : null;

    // ===== Coptic Calendar Names =====
    private static readonly string[] CopticMonthNames =
        { "Thout", "Paopi", "Hathor", "Koiak", "Tobi", "Meshir", "Paremhat", "Pharmouthi", "Pashons", "Paoni", "Epip", "Mesori" };

    public override string? GetCopticMonthName(int month) => month >= 1 && month <= 12 ? CopticMonthNames[month - 1] : null;

    // ===== Ethiopian Calendar Names =====
    private static readonly string[] EthiopianMonthNames =
        { "Meskerem", "Tekemt", "Hidar", "Tahsas", "Ter", "Yekatit", "Megabit", "Miazia", "Genbot", "Sene", "Hamle", "Nehasse" };

    public override string? GetEthiopianMonthName(int month) => month >= 1 && month <= 12 ? EthiopianMonthNames[month - 1] : null;

    // ===== Mayan Calendar Names =====
    public override string CalendarMayanBaktun => "Baktun";
    public override string CalendarMayanKatun => "Katun";
    public override string CalendarMayanTun => "Tun";
    public override string CalendarMayanUinal => "Uinal";
    public override string CalendarMayanKin => "Kin";

    // ===== Javanese Calendar Names =====
    private static readonly string[] JavaneseMonthNames =
        { "Sura", "Sapar", "Mulud", "Bakda Mulud", "Djumadilawal", "Djumadilakir", "Redjeb", "Ruwah", "Pasa", "Sawal", "Dulkaidah", "Duldjedjah" };

    public override string? GetJavaneseMonthName(int month) => month >= 1 && month <= 12 ? JavaneseMonthNames[month - 1] : null;

    // ===== Buddhist Calendar Names =====
    private static readonly string[] BuddhistMonthNames =
        { "Makara", "Kumbha", "Mina", "Mesha", "Vrishabha", "Mithuna", "Karkata", "Simha", "Kanya", "Tula", "Vrishchika", "Dhanus" };

    public override string? GetBuddhistMonthName(int month) => month >= 1 && month <= 12 ? BuddhistMonthNames[month - 1] : null;

    // ===== Juche Calendar Names =====
    private static readonly string[] JucheMonthNames =
        { "První", "Druhý", "Třetí", "Čtvrtý", "Pátý", "Šestý", "Sedmý", "Osmý", "Devátý", "Desátý", "Jedenáctý", "Dvanáctý" };

    public override string? GetJucheMonthName(int month) => month >= 1 && month <= 12 ? JucheMonthNames[month - 1] : null;

    // ===== Mongolian Calendar Names =====
    private static readonly string[] MongolianMonthNames =
        { "První", "Druhý", "Třetí", "Čtvrtý", "Pátý", "Šestý", "Sedmý", "Osmý", "Devátý", "Desátý", "Jedenáctý", "Dvanáctý" };

    // ===== Tibetan Calendar Names =====
    private static readonly string[] TibetanMonthNames =
        { "První", "Druhý", "Třetí", "Čtvrtý", "Pátý", "Šestý", "Sedmý", "Osmý", "Devátý", "Desátý", "Jedenáctý", "Dvanáctý" };

    // ===== Cherokee Calendar Names =====
    private static readonly string[] CherokeeMonthNames =
        { "První", "Druhý", "Třetí", "Čtvrtý", "Pátý", "Šestý", "Sedmý", "Osmý", "Devátý", "Desátý", "Jedenáctý", "Dvanáctý" };

    public override string? GetCherokeeMonthName(int month) => month >= 1 && month <= 12 ? CherokeeMonthNames[month - 1] : null;

    // ===== Yi Calendar Names =====
    private static readonly string[] YiMonthNames =
        { "První", "Druhý", "Třetí", "Čtvrtý", "Pátý", "Šestý", "Sedmý", "Osmý", "Devátý", "Desátý", "Jedenáctý", "Dvanáctý" };
    private static readonly string[] YiDayAnimalNames =
        { "Myš", "Kráva", "Tygr", "Králík", "Drak", "Had", "Kůň", "Ovce", "Opice", "Kohout", "Pes", "Prase" };
    private static readonly string[] YiXunNames =
        { "První sün", "Druhý sün", "Třetí sün" };

    public override string? GetYiMonthName(int month) => month >= 1 && month <= 12 ? YiMonthNames[month - 1] : null;
    public override string? GetYiDayAnimalName(int index) => index >= 0 && index < YiDayAnimalNames.Length ? YiDayAnimalNames[index] : null;
    public override string? GetYiXunName(int index) => index >= 0 && index < YiXunNames.Length ? YiXunNames[index] : null;

    // ===== Inuit Calendar Names =====
    private static readonly string[] InuitMonthNames =
        { "První", "Druhý", "Třetí", "Čtvrtý", "Pátý", "Šestý", "Sedmý", "Osmý", "Devátý", "Desátý", "Jedenáctý", "Dvanáctý" };

    public override string? GetInuitMonthName(int month) => month >= 1 && month <= 12 ? InuitMonthNames[month - 1] : null;

    // ===== Vikram Samvat Calendar Names =====
    private static readonly string[] VikramSamvatMonthNames =
        { "Čaitra", "Vaisákha", "Džjaištha", "Ášádha", "Šrávana", "Bhádrapada", "Ášvina", "Kártika", "Márgaširša", "Pauša", "Mágha", "Phálguna" };

    // ===== Saka Calendar Names =====
    private static readonly string[] SakaMonthNames =
        { "Čaitra", "Vaisákha", "Džjaištha", "Ášádha", "Šrávana", "Bhádrapada", "Ášvina", "Kártika", "Márgaširša", "Pauša", "Mágha", "Phálguna" };

    // ===== Zoroastrian Calendar Names =====
    private static readonly string[] ZoroastrianMonthNames =
        { "Farvardin", "Ordibehešt", "Chordád", "Tir", "Mordád", "Šahrivár", "Mehr", "Ábán", "Ázar", "Dej", "Bahman", "Esfand" };

    public override string? GetZoroastrianMonthName(int month) => month >= 1 && month <= 12 ? ZoroastrianMonthNames[month - 1] : null;

    // ===== French Republican Calendar Names =====
    private static readonly string[] FrenchRepublicanMonthNames =
        { "Vendémiaire", "Brumaire", "Frimaire", "Nivôse", "Pluviôse", "Ventôse", "Germinal", "Floréal", "Prairial", "Messidor", "Thermidor", "Fructidor" };

    public override string? GetFrenchRepublicanMonthName(int month) => month >= 1 && month <= 12 ? FrenchRepublicanMonthNames[month - 1] : null;

    // ===== Chula Sakarat Calendar Names =====
    private static readonly string[] ChulaSakaratMonthNames =
        { "První", "Druhý", "Třetí", "Čtvrtý", "Pátý", "Šestý", "Sedmý", "Osmý", "Devátý", "Desátý", "Jedenáctý", "Dvanáctý" };

    public override string? GetChulaSakaratMonthName(int month) => month >= 1 && month <= 12 ? ChulaSakaratMonthNames[month - 1] : null;

    // ===== Roman Calendar Names =====
    private static readonly string[] RomanMonthNames =
        { "Ianuarius", "Februarius", "Martius", "Aprilis", "Maius", "Iunius", "Iulius", "Augustus", "September", "October", "November", "December" };

    public override string? GetRomanMonthName(int month) => month >= 1 && month <= 12 ? RomanMonthNames[month - 1] : null;

    // ===== Vietnamese Calendar Names =====
    private static readonly string[] VietnameseZodiacNames =
        { "Myš", "Kráva", "Tygr", "Králík", "Drak", "Had", "Kůň", "Ovce", "Opice", "Kohout", "Pes", "Prase" };

    public override string GetVietnameseZodiacName(int index) => index >= 0 && index < VietnameseZodiacNames.Length ? VietnameseZodiacNames[index] : null;

    // ===== Missing Properties - Batch 1 =====
    public override string AuditTimeToday => "Dnes";
    public override string AuditTimeWeek => "Týden";
    public override string AuditTimeMonth => "Měsíc";
    public override string AuditTimeYear => "Rok";
    public override string AuditAIClientType => "Typ AI klienta";
    public override string AuditBeing => "Bytí";
    public override string AuditStartTime => "Čas začátku";
    public override string AuditEndTime => "Čas konce";
    public override string AuditPrevPage => "Předchozí";
    public override string AuditNextPage => "Další";
    public override string AuditEmptyState => "Žádné audity";
    public override string AuditTotalTokens => "Celkem tokenů";
    public override string AuditPromptTokens => "Tokeny promptu";
    public override string AuditCompletionTokens => "Tokeny dokončení";
    public override string AuditSuccessCount => "Úspěšné";
    public override string AuditFailureCount => "Selhané";
    public override string AuditTooltipPrompt => "Tokeny promptu";
    public override string AuditTooltipCompletion => "Tokeny dokončení";
    public override string AuditTooltipTotal => "Celkem tokenů";
    public override string AuditTooltipDate => "Datum";
    public override string AuditGroupByBeing => "Podle bytí";
    public override string AuditGroupByClient => "Podle klienta";
    public override string AuditAllBeings => "Všechna bytí";

    public override string CalendarSexagenaryName => "Sexagenární cyklus";
    public override string CalendarComponentYearStem => "Nebeský kmen roku";
    public override string CalendarComponentYearBranch => "Zemská větev roku";
    public override string CalendarComponentDynasty => "Dynastie";
    public override string CalendarComponentZodiac => "Zvěrokruh";

    public override string ChineseLunarLeapPrefix => "Přestupný";
    public override string VietnameseLeapPrefix => "Přestupný";

    public override string MemoryDetailId => "ID";
    public override string MemoryDetailTitle => "Název";
    public override string MemoryDetailContent => "Obsah";
    public override string MemoryDetailCreatedAt => "Vytvořeno";
    public override string MemoryDetailKeywords => "Klíčová slova";
    public override string MemoryDetailRelatedBeings => "Související bytí";
    public override string MemoryStatTypeDistribution => "Rozdělení typů";
    public override string MemoryToolStatsHeader => "Statistiky";
    public override string MemoryToolStatsTotal => "Celkem";

    public override string ConfigBrowseButton => "Procházet";
    public override string ConfigEditButton => "Upravit";
    public override string ConfigEditPropertyLabel => "Vlastnost";
    public override string ConfigDictAddButton => "Přidat";
    public override string ConfigDictDeleteButton => "Smazat";
    public override string ConfigDictEmptyMessage => "Žádné položky";
    public override string ConfigNullValue => "Null";
    public override string ConfigErrorUnsupportedType => "Nepodporovaný typ";
    public override string ConfigErrorConvertEnum => "Nelze převést na enum";
    public override string ConfigErrorConvertTimeSpan => "Nelze převést na TimeSpan";
    public override string ConfigErrorConvertLong => "Nelze převést na long";

    public override string LogsEndTime => "Čas konce";
    public override string LogsFilterButton => "Filtr";
    public override string LogsSystemOnly => "Pouze systém";

    public override string TasksPriorityLabel => "Priorita";
    public override string TasksStatusCancelled => "Zrušeno";

    public override string ProjectsEmptyState => "Žádné projekty";

    public override string PermissionPageHeader => "Oprávnění";
    public override string PermissionEmptyState => "Žádná oprávnění";
    public override string PermissionBeingNotFound => "Bytí nenalezeno";
    public override string PermissionMissingBeingId => "Chybějící ID bytí";
    public override string PermissionSaveMissingBeingId => "Chybějící ID bytí";
    public override string PermissionRuleOtherTypesDefault => "Výchozí pro ostatní";
    public override string PermissionTemplateDescription => "Popis šablony";
    public override string PermissionCallbackClassSummary2 => "Třída zpětného volání";
    public override string PermissionCallbackConstructorSummary => "Konstruktor";
    public override string PermissionCallbackConstructorSummary2 => "Konstruktor zpětného volání";

    public override string AboutGitHubLink => "GitHub";
    public override string AboutGiteeLink => "Gitee";

    // ===== Missing Methods - Batch 2 =====

    // ===== Localize Date Methods =====
    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00} {hour:00}:{minute:00}:{second:00}";
    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeJapaneseDate(int year, int month, int day, int era, int eraYear, int hour, int minute) => $"{eraYear} {era}";
    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int hour, int minute, int second, int zodiac) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second) => $"{baktun}.{katun}.{tun}.{uinal}.{kin}";
    public override string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeYiDate(int year, int month, int day, int season, int hour, int minute) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeRomanDate(int year, int month, int day, int hour, int minute, int second) => $"{year}-{month:00}-{day:00}";
    public override string LocalizeSexagenaryDate(int year, int month, int day, int yearStem, int yearBranch, int monthStem, int monthBranch, int hour, int minute) => $"{year}-{month:00}-{day:00}";

    public override string? GetVietnameseMonthName(int month) => month >= 1 && month <= 12 ? $"Měsíc {month}" : null;
    public override string? GetChineseHistoricalMonthName(int month) => month >= 1 && month <= 12 ? $"Měsíc {month}" : null;
    public override string? GetDaiMonthName(int month) => month >= 1 && month <= 12 ? $"Měsíc {month}" : null;
    public override string? GetDehongDaiMonthName(int month) => month >= 1 && month <= 12 ? $"Měsíc {month}" : null;
    public override string? GetRocMonthName(int month) => month >= 1 && month <= 12 ? $"Měsíc {month}" : null;

    public override string FormatTimerStartNotification(string timerName) => $"Časovač '{timerName}' spuštěn";
    public override string FormatMemoryEventTimer(string timer) => $"Časovač: {timer}";

    public override string GetCodeHoverKeywordDesc(string keyword, string context) => $"{keyword}: {context}";
    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType;

    public override string GetMemoryCompressionUserPrompt(string summary, string original, string context) => $"Shrň: {summary}";
    public override string CommonSystemPrompt => "Jsi asistent.";

    public override string? GetSexagenaryZodiacName(int index)
    {
        var zodiacNames = new[] { "Myš", "Kráva", "Tygr", "Králík", "Drak", "Had", "Kůň", "Ovce", "Opice", "Kohout", "Pes", "Prase" };
        return index >= 0 && index < zodiacNames.Length ? zodiacNames[index] : null;
    }

    public override string? GetYiSeasonName(int index)
    {
        var seasonNames = new[] { "Jaro", "Léto", "Podzim", "Zima" };
        return index >= 0 && index < seasonNames.Length ? seasonNames[index] : null;
    }

    public override string GetLogLevelName(LogLevel level) => level.ToString();

    public override string GetTranslation(string key) => key;



    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new List<string>();
        if (days > 0) parts.Add($"{days}{CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours}{CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes}{CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds}{CalendarIntervalSeconds}");
        return string.Join(", ", parts);
    }


    public override string CalendarComponentIsLeap => "Přestupný";
    public override string MemoryToolNoMemories => "Žádné paměti";
    public override string MemoryToolMissingContent => "Chybějící obsah";
    public override string MemoryToolStatsNewest => "Nejnovější";
    public override string MemoryToolRecentHeader(int count) => $"Posledních {count} pamětí";

    // ===== Code Browser =====
    public override string CodeBrowserPageHeader => "Prohlížeč kódu";
    public override string GetCodeHoverWordTypeDesc(string wordType, string word) => $"{wordType}: {word}";
}
