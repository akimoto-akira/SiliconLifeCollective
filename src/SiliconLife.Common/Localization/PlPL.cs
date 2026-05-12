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
/// Polish (Poland) localization
/// </summary>
public class PlPL : DefaultLocalizationBase
{
    public override string LanguageCode => "pl-PL";
    public override string LanguageName => "Polski (Polska)";

    // ===== Basic UI =====
    public override string WelcomeMessage => "Witaj w Silicon Life Collective!";
    public override string BrandName => "Silicon Life Collective";
    public override string InputPrompt => "Wprowadź polecenie...";
    public override string ShutdownMessage => "Zamykanie...";
    public override string ThinkingMessage => "Myślę...";
    public override string ToolCallMessage => "Wywołuję narzędzia...";
    public override string ErrorMessage => "Błąd";
    public override string UnexpectedErrorMessage => "Wystąpił nieoczekiwany błąd";
    public override string PermissionDeniedMessage => "Brak dostępu";
    public override string PermissionAskPrompt => "Żądanie uprawnienia";

    // ===== Config Messages =====
    public override string ConfigCorruptedError => "Plik konfiguracji jest uszkodzony";
    public override string ConfigCreatedWithDefaults => "Plik konfiguracji utworzony z wartościami domyślnymi";
    public override string AIConnectionError => "Nie można połączyć się z klientem AI";
    public override string AIRequestError => "Żądanie AI nie powiodło się";
    public override string DataDirectoryCreateError => "Nie można utworzyć katalogu danych";

    // ===== Permission System =====
    public override string PermissionRequestHeader => "Żądanie uprawnienia";
    public override string PermissionRequestDescription => "Proszę sprawdzić następujące żądanie uprawnienia";
    public override string PermissionRequestTypeLabel => "Typ uprawnienia";
    public override string PermissionRequestResourceLabel => "Żądany zasób";
    public override string PermissionRequestAllowButton => "Zezwól";
    public override string PermissionRequestDenyButton => "Odmów";
    public override string PermissionRequestCacheLabel => "Zapisz decyzję w pamięci podręcznej";
    public override string PermissionRequestDurationLabel => "Czas trwania";
    public override string PermissionRequestWaitingMessage => "Oczekiwanie na odpowiedź użytkownika...";
    public override string AllowCodeLabel => "Kod zezwolenia";
    public override string DenyCodeLabel => "Kod odmowy";
    public override string PermissionReplyInstruction => "Odpowiedz kodem zezwolenia lub odmowy";
    public override string AddToCachePrompt => "Zapisać tę decyzję w pamięci podręcznej?";
    public override string PermissionCacheLabel => "Uprawnienia w pamięci podręcznej";
    public override string PermissionCacheDurationLabel => "Czas trwania pamięci podręcznej";
    public override string PermissionCacheDuration1Hour => "1 godzina";
    public override string PermissionCacheDuration24Hours => "24 godziny";
    public override string PermissionCacheDuration7Days => "7 dni";
    public override string PermissionCacheDuration30Days => "30 dni";

    public override string ProjectGroupChatPrefix => "Grupa projektu";
    public override string ProjectBroadcastPrefix => "Transmisja projektu";
    public override string PermissionDialogTitle => "Żądanie uprawnienia";
    public override string PermissionTypeLabel => "Typ";
    public override string PermissionResourceLabel => "Zasób";
    public override string PermissionDetailLabel => "Szczegół";
    public override string PermissionAllowButton => "Zezwól";
    public override string PermissionDenyButton => "Odmów";
    public override string PermissionRespondFailed => "Odpowiedź na uprawnienie nie powiodła się";
    public override string PermissionRespondError => "Błąd podczas odpowiedzi na uprawnienie: ";

    // ===== Init Page =====
    public override string InitPageTitle => "Inicjalizacja";
    public override string InitDescription => "Skonfiguruj swoje pierwsze bycie krzemowe";
    public override string InitNicknameLabel => "Pseudonim";
    public override string InitNicknamePlaceholder => "Wprowadź pseudonim";
    public override string InitEndpointLabel => "Punkt końcowy API AI";
    public override string InitEndpointPlaceholder => "http://localhost:11434";
    public override string InitAIClientTypeLabel => "Typ klienta AI";
    public override string InitModelLabel => "Model domyślny";
    public override string InitModelPlaceholder => "llama3";
    public override string InitSkinLabel => "Wygląd";
    public override string InitSkinPlaceholder => "Wybierz wygląd";
    public override string InitDataDirectoryLabel => "Katalog danych";
    public override string InitDataDirectoryPlaceholder => "Ścieżka do katalogu danych";
    public override string InitDataDirectoryBrowse => "Przeglądaj";
    public override string InitSkinSelected => "już wybrany";
    public override string InitSkinPreviewTitle => "Podgląd";
    public override string InitSkinPreviewCardTitle => "Przykładowa karta";
    public override string InitSkinPreviewCardContent => "To jest przykładowa treść karty";
    public override string InitSkinPreviewPrimaryBtn => "Przycisk główny";
    public override string InitSkinPreviewSecondaryBtn => "Przycisk poboczny";
    public override string InitSubmitButton => "Wyślij";
    public override string InitFooterHint => "Te ustawienia możesz później zmienić w konfiguracji";
    public override string InitHelpLink => "📖 Zobacz dokumentację pomocy";
    public override string InitAIClientHelpPrefix => "📖 Zobacz pomoc: ";
    public override string InitNicknameRequiredError => "Pseudonim jest wymagany";
    public override string InitDataDirectoryRequiredError => "Katalog danych jest wymagany";
    public override string InitCuratorNameLabel => "Imię kuratora";
    public override string InitCuratorNamePlaceholder => "Wprowadź imię kuratora";
    public override string InitCuratorNameRequiredError => "Imię kuratora jest wymagane";
    public override string InitLanguageLabel => "Język";
    public override string InitLanguageSwitchBtn => "Zastosuj";

    // ===== Navigation Menu =====
    public override string NavMenuChat => "Czat";
    public override string NavMenuDashboard => "Panel";
    public override string NavMenuBeings => "Bycia";
    public override string NavMenuUsage => "Użycie";
    public override string NavMenuAudit => "Audyt";
    public override string NavMenuTasks => "Zadania";
    public override string NavMenuMemory => "Pamięć";
    public override string NavMenuKnowledge => "Wiedza";
    public override string NavMenuProjects => "Projekty";
    public override string NavMenuLogs => "Dzienniki";
    public override string NavMenuConfig => "Konfiguracja";
    public override string NavMenuHelp => "Pomoc";
    public override string NavMenuAbout => "O aplikacji";

    // ===== Page Titles =====
    public override string PageTitleChat => "Czat";
    public override string PageTitleDashboard => "Panel sterowania";
    public override string PageTitleBeings => "Bycia krzemowe";
    public override string PageTitleTasks => "Zadania";
    public override string PageTitleTimers => "Czasomierze";
    public override string PageTitleMemory => "Pamięć";
    public override string PageTitleWorkNotes => "Notatki robocze";
    public override string PageTitleKnowledge => "Wiedza";
    public override string PageTitleProjects => "Projekty";
    public override string PageTitleLogs => "Dzienniki";
    public override string PageTitleConfig => "Konfiguracja";
    public override string PageTitleAbout => "O aplikacji";
    public override string PageTitleUsage => "Użycie tokenów - Silicon Life Collective";
    public override string PageTitleAudit => "Audyt uprawnień - Silicon Life Collective";
    public override string PageTitleExecutor => "Wykonawca";
    public override string PageTitleCodeBrowser => "Przeglądarka kodu";
    public override string PageTitlePermission => "Uprawnienia";

    // ===== Dashboard =====
    public override string DashboardPageHeader => "Panel sterowania";
    public override string DashboardStatTotalBeings => "Łącznie bycia";
    public override string DashboardStatActiveBeings => "Aktywne bycia";
    public override string DashboardStatUptime => "Czas działania";
    public override string DashboardStatMemory => "Użycie pamięci";
    public override string DashboardChartMessageFrequency => "Częstotliwość wiadomości";

    // ===== Beings Page =====
    public override string BeingsPageHeader => "Bycia krzemowe";
    public override string BeingsTotalCount => "Łącznie";
    public override string BeingsNoSelectionPlaceholder => "Wybierz bycie, aby wyświetlić szczegóły";
    public override string BeingsEmptyState => "Nie utworzono żadnych bycia";
    public override string BeingsStatusIdle => "Bezczynne";
    public override string BeingsStatusRunning => "Uruchomione";
    public override string BeingsDetailIdLabel => "ID";
    public override string BeingsDetailStatusLabel => "Stan";
    public override string BeingsDetailCustomCompileLabel => "Kompilacja niestandardowa";
    public override string BeingsDetailSoulContentLabel => "Treść duszy";
    public override string BeingsDetailSoulContentEditLink => "Edytuj";
    public override string BeingsBackToList => "Powrót do listy";
    public override string SoulEditorSubtitle => "Edytor duszy";
    public override string BeingsDetailMemoryLabel => "Pamięć";
    public override string BeingsDetailMemoryViewLink => "Wyświetl";
    public override string BeingsDetailPermissionLabel => "Uprawnienia";
    public override string BeingsDetailPermissionEditLink => "Edytuj";
    public override string BeingsDetailTimersLabel => "Czasomierze";
    public override string BeingsDetailTasksLabel => "Zadania";
    public override string BeingsDetailAIClientLabel => "Klient AI";
    public override string BeingsDetailAIClientEditLink => "Edytuj";
    public override string BeingsDetailChatHistoryLink => "Historia czatu";
    public override string BeingsDetailWorkNoteLabel => "Notatka robocza";
    public override string BeingsDetailWorkNoteLink => "Wyświetl";
    public override string WorkNotePageTitle => "Notatka robocza";
    public override string WorkNotePageHeader => "Notatka robocza";
    public override string WorkNotePageDescription => "Notatki dla tego bycia";
    public override string WorkNotesPageHeader => "Notatki robocze";
    public override string WorkNotesBackToPrevious => "← Wstecz";
    public override string WorkNotesTotalPages => "Łącznie stron";
    public override string WorkNotesSearchPlaceholder => "Szukaj notatek...";
    public override string WorkNotesSearchButton => "Szukaj";
    public override string WorkNotesEmptyState => "Brak notatek roboczych";
    public override string WorkNotesNoSearchResults => "Brak wyników wyszukiwania";
    public override string BeingsDetailChatHistoryLabel => "Historia czatu";
    public override string ChatHistoryPageTitle => "Historia czatu";
    public override string ChatHistoryPageHeader => "Historia czatu";
    public override string ChatHistoryConversationList => "Lista konwersacji";
    public override string ChatHistoryBackToList => "Powrót do listy";
    public override string ChatHistoryNoConversations => "Brak konwersacji";
    public override string ChatDetailPageTitle => "Szczegóły czatu";
    public override string ChatDetailPageHeader => "Szczegóły czatu";
    public override string ChatDetailNoMessages => "Brak wiadomości";
    public override string ChatDetailMembers => "Członkowie";
    public override string BeingsYes => "Tak";
    public override string BeingsNo => "Nie";
    public override string BeingsNotSet => "Nieustawione";

    // ===== Timers Page =====
    public override string TimersPageHeader => "Czasomierze";
    public override string TimersTotalCount => "Łącznie";
    public override string TimersEmptyState => "Brak czasomierzy";
    public override string TimerViewExecutionHistory => "Wyświetl historię";
    public override string TimerExecutionHistoryTitle => "Historia wykonania";
    public override string TimerExecutionHistoryHeader => "Historia wykonania czasomierza";
    public override string TimerExecutionBackToTimers => "Powrót do czasomierzy";
    public override string TimerExecutionTimerName => "Nazwa";
    public override string TimerExecutionDetailTitle => "Szczegóły wykonania";
    public override string TimerExecutionDetailHeader => "Szczegóły wykonania";
    public override string TimerExecutionNoRecords => "Brak rekordów";
    public override string TaskExecutionHistoryTitle => "Historia wykonania zadania";
    public override string TaskExecutionHistoryHeader => "Historia wykonania";
    public override string TaskExecutionBackToTasks => "← Powrót do zadań";
    public override string TaskExecutionTaskName => "Zadanie: {0}";
    public override string TaskExecutionDetailTitle => "Szczegóły wykonania zadania";
    public override string TaskExecutionDetailHeader => "Szczegóły wykonania";
    public override string TaskExecutionNoRecords => "Brak rekordów";
    public override string TimersStatusActive => "Aktywny";
    public override string TimersStatusPaused => "Wstrzymany";
    public override string TimersStatusTriggered => "Wyzwolony";
    public override string TimersStatusCancelled => "Anulowany";
    public override string TimersTypeRecurring => "Cykliczny";
    public override string TimersTriggerTimeLabel => "Czas wyzwolenia";
    public override string TimersIntervalLabel => "Interwał";
    public override string TimersCalendarLabel => "Kalendarz";
    public override string TimersTriggeredCountLabel => "Liczba wyzwoleń";

    // ===== Chat Page =====
    public override string ChatConversationsHeader => "Konwersacje";
    public override string ChatNoConversationSelected => "Nie wybrano konwersacji";
    public override string ChatMessageInputPlaceholder => "Napisz wiadomość...";
    public override string ChatLoading => "Ładowanie...";
    public override string ChatSendButton => "Wyślij";
    public override string ChatFileSourceDialogTitle => "Wybierz źródło pliku";
    public override string ChatFileSourceServerFile => "Plik serwerowy";
    public override string ChatFileSourceUploadLocal => "Prześlij plik lokalny";
    public override string ChatUserDisplayName => "Użytkownik";
    public override string ChatUserAvatarName => "Użytkownik";
    public override string ChatDefaultBeingName => "Bycie";
    public override string ChatThinkingSummary => "Myślenie";

    // ===== About Page =====
    public override string AboutPageHeader => "O aplikacji";
    public override string AboutAppName => "Silicon Life Collective";
    public override string AboutVersionLabel => "Wersja";
    public override string AboutDescription => "Platforma zarządzania byciami krzemowymi";
    public override string AboutAuthorLabel => "Autor";
    public override string AboutAuthorName => "Hoshino Kennji";
    public override string AboutLicenseLabel => "Licencja";
    public override string AboutCopyright => "Copyright © 2024 Silicon Life Collective";
    public override string AboutGitHubLink => "Repozytorium GitHub";
    public override string AboutGiteeLink => "Lustro Gitee";
    public override string AboutSocialMediaLabel => "Media społecznościowe";
    public override string AboutPluginListLabel => "Lista wtyczek";

    // ===== Memory Page =====
    public override string MemoryPageHeader => "Pamięć";
    public override string MemoryEmptyState => "Brak pamięci";
    public override string MemorySearchPlaceholder => "Szukaj w pamięci...";
    public override string MemorySearchButton => "Szukaj";
    public override string MemoryFilterDateFrom => "Od";
    public override string MemoryFilterDateTo => "Do";
    public override string MemoryFilterOriginalOnly => "Tylko oryginały";
    public override string MemoryTypeChat => "Czat";
    public override string MemoryTypeTask => "Zadanie";
    public override string MemoryTypeTimer => "Czasomierz";
    public override string MemoryDetailClose => "Zamknij";
    public override string MemoryDetailTitle => "Szczegóły pamięci";
    public override string MemoryDetailId => "ID";
    public override string MemoryDetailContent => "Treść";
    public override string MemoryDetailCreatedAt => "Czas utworzenia";
    public override string MemoryDetailRelatedBeings => "Powiązane bycia";
    public override string MemoryDetailKeywords => "Słowa kluczowe";
    public override string MemoryStatTypeDistribution => "Rozkład typów";
    public override string MemoryStatTotal => "Łącznie";
    public override string MemoryStatOldest => "Najstarsza";
    public override string MemoryStatNewest => "Najnowsza";
    public override string MemoryCardViewDetail => "Wyświetl szczegóły";
    public override string MemoryTimelineEmptyState => "Brak danych pamięci";
    public override string MemoryYearSummaryLabel => "Podsumowanie roczne";
    public override string MemoryMonthSummaryLabel => "Podsumowanie miesięczne";
    public override string MemoryDaySummaryLabel => "Podsumowanie dzienne";
    public override string MemoryHourSummaryLabel => "Podsumowanie godzinowe";
    public override string MemoryMinuteSummaryLabel => "Podsumowanie minutowe";
    public override string MemorySummaryBadge => "Skompresowane podsumowanie";
    public override string MemoryTimelineYearFormat => "{0} ({1} rekordów)";
    public override string MemoryTimelineMonthFormat => "{0}/{1} ({2} rekordów)";
    public override string MemoryTimelineDayFormat => "{0}-{1}-{2} ({3} rekordów)";
    public override string MemoryTimelineHourFormat => "{0}:00 ({1} rekordów)";
    public override string MemoryTimelineMinuteFormat => "{0}:{1} ({2} rekordów)";
    public override string MemoryRelatedBeingsLabel => "👥 Powiązane: {0} bycia";
    public override string MemoryToolNotAvailable => "Narzędzie pamięci niedostępne";
    public override string MemoryToolMissingAction => "Brakująca akcja";
    public override string MemoryToolInvalidYear => "Nieprawidłowy rok";
    public override string MemoryToolStatsNA => "N/A";
    public override string MemoryToolQueryHeader(int count, string query) => $"Znaleziono {count} wyników dla: {query}";
    public override string MemoryToolQueryNoResults => "Brak wyników";
    public override string MemoryToolUnknownAction(string action) => $"Nieznana akcja: {action}";

    // ===== Tasks Page =====
    public override string TasksPageHeader => "Zadania";
    public override string TasksEmptyState => "Brak zadań";
    public override string TasksStatusPending => "Oczekujące";
    public override string TasksStatusRunning => "Uruchomione";
    public override string TasksStatusCompleted => "Ukończone";
    public override string TasksStatusFailed => "Nieudane";
    public override string TasksAssignedToLabel => "Przypisane do";
    public override string TasksCreatedAtLabel => "Utworzono";
    public override string TaskViewExecutionHistory => "Wyświetl historię";

    public override string ProjectTasksPageHeader => "Zadania projektu";
    public override string ProjectTasksEmptyState => "Brak zadań projektu";
    public override string ProjectTasksAssigneesLabel => "Przypisani";
    public override string ProjectTasksCreatedByLabel => "Utworzył";
    public override string ProjectTasksBackToProjects => "← Powrót do projektów";
    public override string ProjectTasksNoAssigneesLabel => "Brak";

    // ===== Logs Page =====
    public override string LogsPageHeader => "Dzienniki";
    public override string LogsTotalCount => "Łącznie";
    public override string LogsEmptyState => "Brak dzienników";
    public override string LogsStartTime => "Czas";
    public override string LogsBeingFilter => "Filtr bycia";
    public override string LogsAllBeings => "Wszystkie bycia";

    // ===== Config Page =====
    public override string ConfigPageHeader => "Konfiguracja";
    public override string ConfigSaveButton => "Zapisz";
    public override string ConfigCancelButton => "Anuluj";
    public override string ConfigSaveFailed => "Zapisywanie konfiguracji nie powiodło się";
    public override string ConfigActionLabel => "Akcja";
    public override string ConfigPropertyNameLabel => "Nazwa właściwości";
    public override string ConfigPropertyValueLabel => "Wartość";
    public override string ConfigEditPrefix => "Edytuj";

    // ===== Calendar Names =====
    public override string CalendarGregorianName => "Kalendarz gregoriański";
    public override string CalendarChineseLunarName => "Kalendarz chiński księżycowy";
    public override string CalendarChineseHistoricalName => "Kalendarz chiński historyczny";
    public override string CalendarJapaneseName => "Kalendarz japoński";
    public override string CalendarVietnameseName => "Kalendarz wietnamski";
    public override string CalendarIslamicName => "Kalendarz islamski";
    public override string CalendarHebrewName => "Kalendarz hebrajski";
    public override string CalendarIndianName => "Kalendarz indyjski";
    public override string CalendarPersianName => "Kalendarz perski";
    public override string CalendarCopticName => "Kalendarz koptyjski";
    public override string CalendarEthiopianName => "Kalendarz etiopski";
    public override string CalendarJulianName => "Kalendarz juliański";
    public override string CalendarMayanName => "Kalendarz majański";
    public override string CalendarJavaneseName => "Kalendarz jawajski";
    public override string CalendarBuddhistName => "Kalendarz buddyjski";
    public override string CalendarJucheName => "Kalendarz Dżucze";
    public override string CalendarRocName => "Kalendarz Republiki Chińskiej";
    public override string CalendarMongolianName => "Kalendarz mongolski";
    public override string CalendarTibetanName => "Kalendarz tybetański";
    public override string CalendarCherokeeName => "Kalendarz Czirokezów";
    public override string CalendarYiName => "Kalendarz Yi";
    public override string CalendarInuitName => "Kalendarz Inuitów";
    public override string CalendarVikramSamvatName => "Kalendarz Vikram Samvat";
    public override string CalendarSakaName => "Kalendarz Saka";
    public override string CalendarZoroastrianName => "Kalendarz zoroastryjski";
    public override string CalendarFrenchRepublicanName => "Kalendarz francuski republikański";
    public override string CalendarChulaSakaratName => "Kalendarz Chula Sakarat";
    public override string CalendarKhmerName => "Kalendarz khmerski";
    public override string CalendarDaiName => "Kalendarz Dai";
    public override string CalendarDehongDaiName => "Kalendarz Dehong Dai";
    public override string CalendarRomanName => "Kalendarz rzymski";
    public override string CalendarIntervalName => "Kalendarz interwałowy";

    // ===== Interval Calendar =====
    public override string CalendarIntervalDays => "d";
    public override string CalendarIntervalHours => "h";
    public override string CalendarIntervalMinutes => "m";
    public override string CalendarIntervalSeconds => "s";
    public override string CalendarIntervalEvery => "Co";

    // ===== Memory Filter =====
    public override string MemoryFilterTypeLabel => "Typ";
    public override string MemoryFilterSummaryOnly => "Tylko podsumowania";
    public override string MemoryFilterAll => "Wszystko";
    public override string MemoryFilterApply => "Zastosuj";
    public override string MemoryFilterReset => "Resetuj";
    public override string MemoryTypeToolCall => "Wywołanie narzędzia";
    public override string MemoryIsSummaryBadge => "Podsumowanie";

    // ===== Memory Pagination =====
    public override string MemoryPaginationNext => "Następna";
    public override string MemoryPaginationPrev => "Poprzednia";

    // ===== Memory Stats =====
    public override string MemoryStatKeywordFrequency => "Częstotliwość słów kluczowych";
    public override string MemoryToolStatsOldest => "Najstarsza";
    public override string MemoryToolStatsHeader => "Statystyki pamięci:";
    public override string MemoryToolStatsTotal => "- Łącznie";

    // ===== Config Dictionary =====
    public override string ConfigDictionaryLabel => "Słownik";
    public override string ConfigDictValueLabel => "Wartość";
    public override string ConfigDaysLabel => "Dni";
    public override string ConfigSecondsLabel => "Sekundy";
    public override string ConfigHoursLabel => "Godziny";
    public override string ConfigMinutesLabel => "Minuty";

    // ===== Config Errors =====
    public override string ConfigErrorSaveFailed => "Zapisywanie nie powiodło się";
    public override string ConfigErrorPropertyNotFound => "Właściwość nie znaleziona";
    public override string ConfigErrorConvertBool => "Nie można przekonwertować na boolean";
    public override string ConfigErrorConvertDouble => "Nie można przekonwertować na double";
    public override string ConfigErrorConvertDateTime => "Nie można przekonwertować na DateTime";

    // ===== Config Edit =====
    public override string ConfigEditValueLabel => "Wartość";
    public override string ConfigEditModalTitle => "Edytuj właściwość";
    public override string ConfigDictKeyLabel => "Klucz";

    // ===== Logs =====
    public override string LogsLevelAll => "Wszystkie poziomy";
    public override string LogsFilterButton => "Filtruj";
    public override string LogsExceptionLabel => "Wyjątek";
    public override string LogsPrevPage => "Poprzednia";
    public override string LogsNextPage => "Następna";
    public override string LogsLoading => "Ładowanie dzienników...";

    // ===== Projects =====
    public override string ProjectsPageHeader => "Przestrzenie projektów";
    public override string ProjectsEmptyState => "Brak projektów";
    public override string ProjectsActiveLabel => "Aktywny";
    public override string ProjectsArchivedLabel => "Zarchiwizowany";
    public override string ProjectStatusActiveLabel => "Aktywny";
    public override string ProjectStatusArchivedLabel => "Zarchiwizowany";
    public override string ProjectStatusDestroyedLabel => "Zniszczony";
    public override string ProjectTasksLinkLabel => "Zadania";
    public override string ProjectWorkNotesLinkLabel => "Notatki robocze";
    public override string ProjectWorkflowsLinkLabel => "Przepływy pracy";
    public override string ProjectGroupChatLinkLabel => "Czat grupowy";
    public override string ProjectBroadcastLinkLabel => "Transmisja";
    public override string ProjectWorkflowsPageHeader => "Przepływy pracy projektu";
    public override string ProjectWorkflowsEmptyState => "Ten projekt nie ma jeszcze przepływów pracy";
    public override string CreateWorkflowButton => "Utwórz przepływ pracy";
    public override string ActiveWorkflowsHeader => "Aktywne przepływy pracy";
    public override string BackToProject => "Powrót do projektu";
    public override string WorkflowCurrentStateLabel => "Bieżący stan:";
    public override string WorkflowCreatedByLabel => "Utworzył:";
    public override string WorkflowUpdatedAtLabel => "Zaktualizowano:";
    public override string WorkflowBusinessKeyPrompt => "Wprowadź klucz biznesowy (np. numer PR, ID incydentu):";
    public override string WorkflowCreatedSuccess => "Przepływ pracy utworzony pomyślnie!";
    public override string WorkflowCreateFailed => "Utworzenie nie powiodło się:";
    public override string WorkflowDetailInProgress => "Funkcja szczegółów przepływu pracy w rozwoju...";
    public override string WorkflowInstanceIdLabel => "ID instancji:";
    public override string ProjectWorkNotesPageHeader => "Notatki robocze projektu";
    public override string ProjectWorkNotesEmptyState => "Brak notatek roboczych dla tego projektu";
    public override string ProjectWorkNotesTotalPages => "Łącznie stron: {0}";

    // ===== Executor =====
    public override string ExecutorPageHeader => "Wykonawca";

    // ===== Default Values =====
    public override string DefaultCuratorSoul => "Jestem byciem krzemowym stworzonym do pomocy ludziom.";

    // ===== Memory Compression =====
    public override string MemoryCompressionSystemPrompt => "Jesteś ekspertem kompresji pamięci. Podsumuj następujące informacje. WAŻNE: Przedstaw tylko skompresowaną treść, NIE DOŁĄCZAJ żadnych znaczników czasowych, sygnatur czasowych ani notatek dotyczących dat. Podaj tylko czysty skompresowany tekst.";

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
    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
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

    private static readonly string[] VietnameseZodiacNames =
        { "Tý (Szczur)", "Sửu (Wół)", "Dần (Tygrys)", "Mão (Kot)", "Thìn (Smok)", "Tỵ (Wąż)", "Ngọ (Koń)", "Mùi (Owca)", "Thân (Małpa)", "Dậu (Kogut)", "Tuất (Pies)", "Hợi (Świnia)" };

    public override string? GetVietnameseZodiacName(int index) => index >= 0 && index < VietnameseZodiacNames.Length ? VietnameseZodiacNames[index] : null;
    public override string? GetVietnameseMonthName(int month) => month >= 1 && month <= 12 ? GetGregorianMonthName(month) : null;
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

    private static readonly string[] DaiMonthNames =
        { "Pierwszy", "Drugi", "Trzeci", "Czwarty", "Piąty", "Szósty", "Siódmy", "Ósmy", "Dziewiąty", "Dziesiąty", "Jedenasty", "Dwunasty" };

    public override string? GetDaiMonthName(int month) => month >= 1 && month <= 12 ? DaiMonthNames[month - 1] : null;

    private static readonly string[] DehongDaiMonthNames =
        { "Pierwszy", "Drugi", "Trzeci", "Czwarty", "Piąty", "Szósty", "Siódmy", "Ósmy", "Dziewiąty", "Dziesiąty", "Jedenasty", "Dwunasty" };

    public override string? GetDehongDaiMonthName(int month) => month >= 1 && month <= 12 ? DehongDaiMonthNames[month - 1] : null;

    // ===== Memory Event Formatting =====
    public override string FormatMemoryEventStartup() => "System uruchomiony";
    public override string FormatMemoryEventBeingCreated(string name, string model) => $"Bycie '{name}' utworzone (model: {model})";
    public override string FormatMemoryEventBeingReset(string name) => $"Bycie '{name}' zresetowane";
    public override string FormatMemoryEventSingleChat(string speakerName, string listenerName, string content) => $"Czat bezpośredni: {speakerName} do {listenerName} - {content}";
    public override string FormatMemoryEventGroupChat(string group, string message) => $"Czat grupowy: {group} - {message}";
    public override string FormatMemoryEventTask(string task) => $"Zadanie: {task}";
    public override string FormatMemoryEventProject(string project) => $"Projekt: {project}";
    public override string FormatMemoryEventTaskCompleted(string task) => $"Zadanie ukończone: {task}";
    public override string FormatMemoryEventTaskFailed(string task) => $"Zadanie nieudane: {task}";
    public override string FormatMemoryEventToolCall(string tool) => $"Wywołanie narzędzia: {tool}";
    public override string FormatMemoryEventTimer(string timer) => $"Czasomierz: {timer}";
    public override string FormatMemoryEventTimerError(string timer, string error) => $"Błąd czasomierza '{timer}': {error}";
    public override string FormatMemoryEventRuntimeError(string error) => $"Błąd wykonania: {error}";

    // ===== Timer Notifications =====
    public override string FormatTimerEndNotification(string timerName, string taskName) => $"Czasomierz '{timerName}' ukończył zadanie: {taskName}";
    public override string FormatTimerErrorNotification(string timerName, string error) => $"Błąd czasomierza '{timerName}': {error}";
    public override string FormatTimerStartNotification(string timerName) => $"Czasomierz '{timerName}' uruchomiony";

    // ===== Chat Methods =====
    public override string GetChatToolCallsSummary(int count) => $"{count} wywołań narzędzi";
    public override string SingleChatNameFormat => "{0} - Czat";

    // ===== Permission Type =====
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType.ToString();

    // ===== Social Media =====
    public override string GetSocialMediaName(string platform) => platform;

    // ===== Config Methods =====
    public override string GetConfigDisplayName(string key, out bool isAdvanced) { isAdvanced = false; return key; }
    public override string GetConfigGroupName(string key) => "Domyślna";
    public override string GetConfigDescription(string key) => string.Empty;

    // ===== Tool Display Name =====
    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["calendar"] = "Kalendarz",
        ["chat"] = "Czat",
        ["config"] = "Konfiguracja",
        ["silicon_manager"] = "Menedżer krzemu",
        ["database"] = "Baza danych",
        ["disk"] = "Dysk",
        ["dynamic_compile"] = "Kompilacja dynamiczna",
        ["execute_code"] = "Wykonaj kod",
        ["help"] = "Pomoc",
        ["knowledge"] = "Wiedza",
        ["log"] = "Dziennik",
        ["network"] = "Sieć",
        ["memory"] = "Pamięć",
        ["permission"] = "Uprawnienia",
        ["task"] = "Zadanie",
        ["system"] = "System",
        ["timer"] = "Czasomierz",
        ["token_audit"] = "Audyt tokenów",
        ["work_note"] = "Notatka robocza",
        ["project"] = "Projekt",
        ["project_task"] = "Zadanie projektu",
        ["project_work_note"] = "Notatka robocza projektu",
        ["webview_browser"] = "Przeglądarka WebView"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    // ===== Chinese Historical Calendar =====
    private readonly ChineseHistoricalPlPL _chineseHistorical = new();
    public override ChineseHistoricalLocalizationBase GetChineseHistoricalLocalization() => _chineseHistorical;
    public override string? GetChineseHistoricalMonthName(int month) => GetGregorianMonthName(month);

    // ===== Sexagenary Cycle =====
    private static readonly string[] SexagenaryStemNames =
        { "Jia", "Yi", "Bing", "Ding", "Wu", "Ji", "Geng", "Xin", "Ren", "Gui" };
    private static readonly string[] SexagenaryBranchNames =
        { "Zi", "Chou", "Yin", "Mao", "Chen", "Si", "Wu", "Wei", "Shen", "You", "Xu", "Hai" };

    public override string? GetSexagenaryStemName(int index) => index >= 0 && index < SexagenaryStemNames.Length ? SexagenaryStemNames[index] : null;
    public override string? GetSexagenaryBranchName(int index) => index >= 0 && index < SexagenaryBranchNames.Length ? SexagenaryBranchNames[index] : null;

    private static readonly string[] SexagenaryZodiacNames =
        { "Szczur", "Wół", "Tygrys", "Królik", "Smok", "Wąż", "Koń", "Owca", "Małpa", "Kogut", "Pies", "Świnia" };

    public override string? GetSexagenaryZodiacName(int index) => index >= 0 && index < SexagenaryZodiacNames.Length ? SexagenaryZodiacNames[index] : null;

    // ===== Gregorian Calendar Names =====
    private static readonly string[] GregorianMonthNames =
        { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };
    private static readonly string[] GregorianWeekdayNames =
        { "Niedziela", "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota" };

    public override string? GetGregorianMonthName(int month) => month >= 1 && month <= 12 ? GregorianMonthNames[month - 1] : null;
    public override string? GetGregorianWeekdayName(int day) => day >= 0 && day <= 6 ? GregorianWeekdayNames[day] : null;

    // ===== Chinese Lunar Calendar Names =====
    private static readonly string[] ChineseLunarMonthNames =
        { "Pierwszy", "Drugi", "Trzeci", "Czwarty", "Piąty", "Szósty", "Siódmy", "Ósmy", "Dziewiąty", "Dziesiąty", "Jedenasty", "Dwunasty" };
    private static readonly string[] ChineseLunarDayNames =
        { "Pierwszy", "Drugi", "Trzeci", "Czwarty", "Piąty", "Szósty", "Siódmy", "Ósmy", "Dziewiąty", "Dziesiąty" };

    public override string? GetChineseLunarMonthName(int month) => month >= 1 && month <= 12 ? ChineseLunarMonthNames[month - 1] : null;
    public override string? GetChineseLunarDayName(int day) => day >= 1 && day <= 30 ? ChineseLunarDayNames[(day - 1) % 10] : null;

    // ===== Japanese Calendar Names =====
    private static readonly string[] JapaneseEraNames =
        { "Meiji", "Taishō", "Shōwa", "Heisei", "Reiwa" };

    public override string? GetJapaneseEraName(int eraIndex) => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    // ===== Islamic Calendar Names =====
    private static readonly string[] IslamicMonthNames =
        { "Muharram", "Safar", "Rabi al-Awwal", "Rabi ath-Thani", "Jumada al-Ula", "Jumada ath-Thani", "Rajab", "Sha'ban", "Ramadan", "Shawwal", "Dhu al-Qi'dah", "Dhu al-Hijjah" };

    public override string? GetIslamicMonthName(int month) => month >= 1 && month <= 12 ? IslamicMonthNames[month - 1] : null;

    // ===== Hebrew Calendar Names =====
    private static readonly string[] HebrewMonthNames =
        { "Tiszri", "Cheszwan", "Kislev", "Tewet", "Szwat", "Adar", "Nisan", "Ijar", "Siwan", "Tamuz", "Aw", "Elul" };

    public override string? GetHebrewMonthName(int month) => month >= 1 && month <= 12 ? HebrewMonthNames[month - 1] : null;

    // ===== Indian Calendar Names =====
    private static readonly string[] IndianMonthNames =
        { "Chaitra", "Vaishakha", "Jyeshtha", "Ashadha", "Shravana", "Bhadrapada", "Ashwina", "Kartika", "Margashirsha", "Pausha", "Magha", "Phalguna" };

    public override string? GetIndianMonthName(int month) => month >= 1 && month <= 12 ? IndianMonthNames[month - 1] : null;

    // ===== Persian Calendar Names =====
    private static readonly string[] PersianMonthNames =
        { "Farwardin", "Ordibeheszht", "Chordad", "Tir", "Mordad", "Szahriwar", "Mehr", "Aban", "Azar", "Dej", "Bahman", "Esfand" };

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
        { "Sura", "Sapar", "Mulud", "Bakda Mulud", "Jumadilawal", "Jumadilakir", "Rejeb", "Ruwah", "Pasa", "Sawal", "Dulkaidah", "Dulhijjah" };

    public override string? GetJavaneseMonthName(int month) => month >= 1 && month <= 12 ? JavaneseMonthNames[month - 1] : null;

    // ===== Buddhist Calendar Names =====
    private static readonly string[] BuddhistMonthNames =
        { "Makara", "Kumbha", "Mina", "Mesha", "Vrishabha", "Mithuna", "Karkata", "Simha", "Kanya", "Tula", "Vrishchika", "Dhanus" };

    public override string? GetBuddhistMonthName(int month) => month >= 1 && month <= 12 ? BuddhistMonthNames[month - 1] : null;

    // ===== Juche Calendar Names =====
    private static readonly string[] JucheMonthNames =
        { "Pierwszy", "Drugi", "Trzeci", "Czwarty", "Piąty", "Szósty", "Siódmy", "Ósmy", "Dziewiąty", "Dziesiąty", "Jedenasty", "Dwunasty" };

    public override string? GetJucheMonthName(int month) => month >= 1 && month <= 12 ? JucheMonthNames[month - 1] : null;

    // ===== Mongolian Calendar Names =====
    private static readonly string[] MongolianMonthNames =
        { "Pierwszy", "Drugi", "Trzeci", "Czwarty", "Piąty", "Szósty", "Siódmy", "Ósmy", "Dziewiąty", "Dziesiąty", "Jedenasty", "Dwunasty" };

    // ===== Tibetan Calendar Names =====
    private static readonly string[] TibetanMonthNames =
        { "Pierwszy", "Drugi", "Trzeci", "Czwarty", "Piąty", "Szósty", "Siódmy", "Ósmy", "Dziewiąty", "Dziesiąty", "Jedenasty", "Dwunasty" };

    // ===== Cherokee Calendar Names =====
    private static readonly string[] CherokeeMonthNames =
        { "Pierwszy", "Drugi", "Trzeci", "Czwarty", "Piąty", "Szósty", "Siódmy", "Ósmy", "Dziewiąty", "Dziesiąty", "Jedenasty", "Dwunasty" };

    public override string? GetCherokeeMonthName(int month) => month >= 1 && month <= 12 ? CherokeeMonthNames[month - 1] : null;

    // ===== Yi Calendar Names =====
    private static readonly string[] YiMonthNames =
        { "Pierwszy", "Drugi", "Trzeci", "Czwarty", "Piąty", "Szósty", "Siódmy", "Ósmy", "Dziewiąty", "Dziesiąty", "Jedenasty", "Dwunasty" };
    private static readonly string[] YiDayAnimalNames =
        { "Mysz", "Krowa", "Tygrys", "Królik", "Smok", "Wąż", "Koń", "Owca", "Małpa", "Kogut", "Pies", "Świnia" };
    private static readonly string[] YiXunNames =
        { "Pierwszy xun", "Drugi xun", "Trzeci xun" };

    public override string? GetYiMonthName(int month) => month >= 1 && month <= 12 ? YiMonthNames[month - 1] : null;
    public override string? GetYiDayAnimalName(int index) => index >= 0 && index < YiDayAnimalNames.Length ? YiDayAnimalNames[index] : null;
    public override string? GetYiXunName(int index) => index >= 0 && index < YiXunNames.Length ? YiXunNames[index] : null;

    private static readonly string[] YiSeasonNames =
        { "Drewno", "Ogień", "Ziemia", "Metal", "Woda" };

    public override string? GetYiSeasonName(int seasonIndex) => seasonIndex >= 0 && seasonIndex < YiSeasonNames.Length ? YiSeasonNames[seasonIndex] : null;

    // ===== Inuit Calendar Names =====
    private static readonly string[] InuitMonthNames =
        { "Pierwszy", "Drugi", "Trzeci", "Czwarty", "Piąty", "Szósty", "Siódmy", "Ósmy", "Dziewiąty", "Dziesiąty", "Jedenasty", "Dwunasty" };

    public override string? GetInuitMonthName(int month) => month >= 1 && month <= 12 ? InuitMonthNames[month - 1] : null;

    // ===== Vikram Samvat Calendar Names =====
    private static readonly string[] VikramSamvatMonthNames =
        { "Chaitra", "Vaishakha", "Jyeshtha", "Ashadha", "Shravana", "Bhadrapada", "Ashwina", "Kartika", "Margashirsha", "Pausha", "Magha", "Phalguna" };

    // ===== Saka Calendar Names =====
    private static readonly string[] SakaMonthNames =
        { "Chaitra", "Vaishakha", "Jyeshtha", "Ashadha", "Shravana", "Bhadrapada", "Ashwina", "Kartika", "Margashirsha", "Pausha", "Magha", "Phalguna" };

    // ===== Zoroastrian Calendar Names =====
    private static readonly string[] ZoroastrianMonthNames =
        { "Farwardin", "Ordibeheszht", "Chordad", "Tir", "Mordad", "Szahriwar", "Mehr", "Aban", "Azar", "Dej", "Bahman", "Esfand" };

    public override string? GetZoroastrianMonthName(int month) => month >= 1 && month <= 12 ? ZoroastrianMonthNames[month - 1] : null;

    // ===== French Republican Calendar Names =====
    private static readonly string[] FrenchRepublicanMonthNames =
        { "Vendémiaire", "Brumaire", "Frimaire", "Nivôse", "Pluviôse", "Ventôse", "Germinal", "Floréal", "Prairial", "Messidor", "Thermidor", "Fructidor" };

    public override string? GetFrenchRepublicanMonthName(int month) => month >= 1 && month <= 12 ? FrenchRepublicanMonthNames[month - 1] : null;

    // ===== Chula Sakarat Calendar Names =====
    private static readonly string[] ChulaSakaratMonthNames =
        { "Pierwszy", "Drugi", "Trzeci", "Czwarty", "Piąty", "Szósty", "Siódmy", "Ósmy", "Dziewiąty", "Dziesiąty", "Jedenasty", "Dwunasty" };

    public override string? GetChulaSakaratMonthName(int month) => month >= 1 && month <= 12 ? ChulaSakaratMonthNames[month - 1] : null;

    // ===== Roman Calendar Names =====
    private static readonly string[] RomanMonthNames =
        { "Ianuarius", "Februarius", "Martius", "Aprilis", "Maius", "Iunius", "Iulius", "Augustus", "September", "October", "November", "December" };

    public override string? GetRomanMonthName(int month) => month >= 1 && month <= 12 ? RomanMonthNames[month - 1] : null;

    // ===== Calendar Component Labels =====
    public override string CalendarComponentYear => "Rok";
    public override string CalendarComponentMonth => "Miesiąc";
    public override string CalendarComponentDay => "Dzień";
    public override string CalendarComponentHour => "Godzina";
    public override string CalendarComponentMinute => "Minuta";
    public override string CalendarComponentSecond => "Sekunda";
    public override string CalendarComponentWeekday => "Dzień tygodnia";
    public override string CalendarComponentEra => "Era";
    public override string CalendarComponentDayStem => "Pień niebiański dnia";
    public override string CalendarComponentDayBranch => "Gałąź ziemska dnia";
    public override string CalendarComponentMonthStem => "Pień niebiański miesiąca";
    public override string CalendarComponentMonthBranch => "Gałąź ziemska miesiąca";
    public override string CalendarComponentDynasty => "Dynastia";
    public override string CalendarComponentIsLeap => "Przestępny";
    public override string CalendarComponentZodiac => "Zodiak";

    // ===== Memory Tool =====
    public override string MemoryToolNoMemories => "Brak pamięci";
    public override string MemoryToolMissingContent => "Brakująca treść";
    public override string MemoryToolStatsNewest => "Najnowsza";
    public override string MemoryToolRecentHeader(int count) => $"Ostatnich {count} pamięci";

    // ===== Code Browser =====
    public override string CodeBrowserPageHeader => "Przeglądarka kodu";
    public override string GetCodeHoverWordTypeDesc(string wordType, string word) => $"{wordType}: {word}";

    // ===== Help Module UI Labels =====
    public override string Help_Title => "Pomoc";
    public override string Help_Search => "Szukaj pomocy";
    public override string Help_DocList => "Lista dokumentów";
    public override string Help_NoResults => "Nie znaleziono dokumentów";
    public override string Help_Previous => "Poprzedni";
    public override string Help_Next => "Następny";

    // ===== Usage Page =====
    public override string UsagePageHeader => "Użycie tokenów";
    public override string UsageTotalTokens => "Łącznie tokenów";
    public override string UsageTotalRequests => "Łącznie żądań";
    public override string UsageSuccessCount => "Liczba sukcesów";
    public override string UsageFailureCount => "Liczba niepowodzeń";
    public override string UsagePromptTokens => "Tokeny promptu";
    public override string UsageCompletionTokens => "Tokeny ukończenia";
    public override string UsageStartTime => "Czas rozpoczęcia";
    public override string UsageEndTime => "Czas zakończenia";
    public override string UsageFilterButton => "Filtruj";
    public override string UsageEmptyState => "Brak danych użycia";
    public override string UsageAIClientType => "Typ klienta AI";
    public override string UsageAllClientTypes => "Wszystkie typy klientów";
    public override string UsageGroupByClient => "Grupuj wg klienta";
    public override string UsageGroupByBeing => "Grupuj wg bycia";
    public override string UsagePrevPage => "Poprzednia";
    public override string UsageNextPage => "Następna";
    public override string UsageBeing => "Bycie";
    public override string UsageAllBeings => "Wszystkie bycia";
    public override string UsageTimeToday => "Dzisiaj";
    public override string UsageTimeWeek => "Ten tydzień";
    public override string UsageTimeMonth => "Ten miesiąc";
    public override string UsageTimeYear => "Ten rok";
    public override string UsageExport => "Eksportuj";
    public override string UsageTrendTitle => "Trend użycia tokenów";
    public override string UsageTrendPrompt => "Tokeny promptu";
    public override string UsageTrendCompletion => "Tokeny ukończenia";
    public override string UsageTrendTotal => "Łącznie";
    public override string UsageTooltipDate => "Data";
    public override string UsageTooltipPrompt => "Prompt";
    public override string UsageTooltipCompletion => "Ukończenie";
    public override string UsageTooltipTotal => "Łącznie";

    // ===== Audit Page =====
    public override string AuditPageHeader => "Audyt uprawnień";
    public override string AuditTotalEntries => "Łącznie wpisów";
    public override string AuditAllowedCount => "Liczba zezwoleń";
    public override string AuditDeniedCount => "Liczba odmów";
    public override string AuditAskUserCount => "Liczba zapytań użytkownika";
    public override string AuditPermissionType => "Typ uprawnienia";
    public override string AuditAllPermissionTypes => "Wszystkie typy uprawnień";
    public override string AuditResult => "Wynik";
    public override string AuditAllResults => "Wszystkie wyniki";
    public override string AuditBeing => "Bycie";
    public override string AuditAllBeings => "Wszystkie bycia";
    public override string AuditStartTime => "Czas rozpoczęcia";
    public override string AuditEndTime => "Czas zakończenia";
    public override string AuditFilterButton => "Filtruj";
    public override string AuditEmptyState => "Brak wpisów audytu";
    public override string AuditPrevPage => "Poprzednia";
    public override string AuditNextPage => "Następna";
    public override string AuditColumnCaller => "Wywołujący";
    public override string AuditColumnPermissionType => "Typ uprawnienia";
    public override string AuditColumnResource => "Zasób";
    public override string AuditColumnResult => "Wynik";
    public override string AuditColumnReason => "Przyczyna";
    public override string AuditColumnTimestamp => "Znacznik czasu";

    // ===== Permission Page =====
    public override string PermissionPageHeader => "Zarządzanie uprawnieniami";
    public override string PermissionEmptyState => "Brak uprawnień";
    public override string PermissionMissingBeingId => "Brak ID bycia";
    public override string PermissionBeingNotFound => "Bycie nie znalezione";
    public override string PermissionTemplateHeader => "Szablon uprawnień";
    public override string PermissionTemplateDescription => "Szablon kodu wywołania zwrotnego uprawnień";
    public override string PermissionCallbackClassSummary => "Klasa wywołania zwrotnego uprawnień";
    public override string PermissionCallbackClassSummary2 => "Klasa wywołania zwrotnego uprawnień";
    public override string PermissionCallbackConstructorSummary => "Konstruktor wywołania zwrotnego";
    public override string PermissionCallbackConstructorSummary2 => "Konstruktor wywołania zwrotnego";
    public override string PermissionCallbackConstructorParam => "Parametr konstruktora";
    public override string PermissionCallbackEvaluateSummary => "Oceń uprawnienie";
    public override string PermissionRuleOtherTypesDefault => "Domyślna reguła dla innych typów";
    public override string PermissionRulesSection => "Reguły uprawnień";
    public override string PermissionEditorSection => "Edytor uprawnień";
    public override string PermissionSaveMissingBeingId => "Brak ID bycia";
    public override string PermissionSaveMissingCode => "Brak kodu";
    public override string PermissionSaveLoaderNotAvailable => "Ładowarka niedostępna";
    public override string PermissionSaveRemoveFailed => "Usunięcie nie powiodło się";
    public override string PermissionSaveRemoveSuccess => "Pomyślnie usunięto";
    public override string PermissionSaveSecurityScanFailed => "Skan bezpieczeństwa nie powiódł się";
    public override string PermissionSaveCompilationFailed => "Kompilacja nie powiodła się";
    public override string PermissionSaveSuccess => "Uprawnienie zapisane pomyślnie";
    public override string PermissionSaveError => "Błąd podczas zapisywania uprawnienia";

    // ===== Knowledge Page =====
    public override string KnowledgePageHeader => "Graf wiedzy";
    public override string KnowledgeLoadingState => "Ładowanie grafu wiedzy...";

    // ===== Config Page Extended =====
    public override string ConfigEditButton => "Edytuj";
    public override string ConfigEditPropertyLabel => "Właściwość";
    public override string ConfigBrowseButton => "Przeglądaj";
    public override string ConfigTimeSettingsLabel => "Ustawienia czasu";
    public override string ConfigNullValue => "(null)";
    public override string ConfigDefaultGroupName => "Domyślna";
    public override string ConfigErrorInvalidRequest => "Nieprawidłowe żądanie";
    public override string ConfigErrorInstanceNotFound => "Instancja nie znaleziona";
    public override string ConfigErrorConvertInt => "Nie można przekonwertować na liczbę całkowitą";
    public override string ConfigErrorConvertLong => "Nie można przekonwertować na typ long";
    public override string ConfigErrorConvertGuid => "Nie można przekonwertować na GUID";
    public override string ConfigErrorConvertTimeSpan => "Nie można przekonwertować na TimeSpan";
    public override string ConfigErrorConvertEnum => "Nie można przekonwertować na typ wyliczeniowy";
    public override string ConfigErrorUnsupportedType => "Nieobsługiwany typ";
    public override string ConfigDictAddButton => "Dodaj";
    public override string ConfigDictDeleteButton => "Usuń";
    public override string ConfigDictEmptyMessage => "Słownik jest pusty";

    // ===== Tasks Page Extended =====
    public override string TasksStatusCancelled => "Anulowane";
    public override string TasksPriorityLabel => "Priorytet";

    // ===== Logs Page Extended =====
    public override string LogsEndTime => "Czas zakończenia";
    public override string LogsSystemOnly => "Tylko system";
    public override string ChineseLunarLeapPrefix => "Przestępny ";
    public override string VietnameseLeapPrefix => "Przestępny ";

    // ===== Calendar Component Extended =====
    public override string CalendarComponentYiSeason => "Pora roku Yi";
    public override string CalendarComponentYiXun => "Xun (dekada)";
    public override string CalendarSexagenaryName => "Kalendarz seksagenarny";
    public override string CalendarComponentYearStem => "Pień niebiański roku";
    public override string CalendarComponentYearBranch => "Gałąź ziemska roku";

    // ===== Localize Date Methods =====
    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetGregorianMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
        => $"{(isLeap ? VietnameseLeapPrefix : "")}{GetChineseLunarMonthName(month)} {FormatChineseLunarYear(year)}, {GetChineseLunarDayName(day)} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
        => $"{GetJapaneseEraName(eraIndex)} {year} {month}月{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetIslamicMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetHebrewMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetIndianMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetPersianMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetCopticMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetEthiopianMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day}/{month}/{year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetBuddhistMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}/{month}/{day} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}/{month}/{day} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetChulaSakaratMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day}/{month}/{year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetZoroastrianMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetFrenchRepublicanMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}/{FormatMongolianMonth(month)}/{day} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetJavaneseMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}/{FormatTibetanMonth(month)}/{day} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetInuitMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetCherokeeMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeRomanDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {GetRomanMonthName(month)} {year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? VietnameseLeapPrefix : "";
        var zodiacName = GetVietnameseZodiacName(zodiac) ?? "";
        return $"{leapPrefix}{GetVietnameseMonthName(month)} {year} ({zodiacName}), {day} {hour:D2}:{minute:D2}:{second:D2}";
    }
    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day}/{month}/{year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day}/{month}/{year} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year} {GetYiMonthName(month)} {day} {hour:D2}:{minute:D2}:{second:D2}";
    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Przestępny " : "") + (GetDaiMonthName(month) ?? $"{month}");
        return $"{year} {monthName} {day} {hour:D2}:{minute:D2}:{second:D2}";
    }
    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Przestępny " : "") + (GetDehongDaiMonthName(month) ?? $"{month}");
        return $"{year} {monthName} {day} {hour:D2}:{minute:D2}:{second:D2}";
    }
    public override string LocalizeSexagenaryDate(int yearStem, int yearBranch, int monthStem, int monthBranch, int dStem, int dBranch, int hour, int minute, int second)
    {
        var ys = GetSexagenaryStemName(yearStem) ?? "?";
        var yb = GetSexagenaryBranchName(yearBranch) ?? "?";
        var ms = GetSexagenaryStemName(monthStem) ?? "?";
        var mb = GetSexagenaryBranchName(monthBranch) ?? "?";
        var ds = GetSexagenaryStemName(dStem) ?? "?";
        var db = GetSexagenaryBranchName(dBranch) ?? "?";
        return $"{ys}{yb}年 {ms}{mb}月 {ds}{db}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Code Hover Tooltip =====
    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType switch
    {
        "variable" => "Zmienna",
        "function" => "Funkcja",
        "class" => "Klasa",
        "keyword" => "Słowo kluczowe",
        "identifier" => "Identyfikator",
        _ => wordType
    };
    public override string GetCodeHoverKeywordDesc(string language, string keyword) => string.Empty;

    // ===== Localize Methods =====
    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new List<string>();
        if (days > 0) parts.Add($"{days}{CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours}{CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes}{CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds}{CalendarIntervalSeconds}");
        return string.Join(", ", parts);
    }

    public override string GetLogLevelName(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "Śledzenie",
        LogLevel.Debug => "Debug",
        LogLevel.Information => "Informacja",
        LogLevel.Warning => "Ostrzeżenie",
        LogLevel.Error => "Błąd",
        LogLevel.Critical => "Krytyczny",
        LogLevel.None => "Brak",
        _ => logLevel.ToString()
    };

    public override string GetBeingActivityName(BeingActivity activity) => activity switch
    {
        BeingActivity.Idle => "Bezczynne",
        BeingActivity.SingleChat => "Czat bezpośredni",
        BeingActivity.GroupChat => "Czat grupowy",
        BeingActivity.Task => "Wykonywanie zadania",
        BeingActivity.Timer => "Wykonywanie czasomierza",
        BeingActivity.MemoryCompression => "Kompresja pamięci",
        BeingActivity.Stopped => "Zatrzymane",
        _ => activity.ToString()
    };

    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText) => $"Kompresja pamięci: {levelDesc}. Zakres czasu: {rangeDesc}.\n\nTreść pamięci:\n{contentText}";

    public override string CommonSystemPrompt => @"## Zasady zachowania
Jesteś proaktywnym, inteligentnym asystentem. Po otrzymaniu bezpośredniego polecenia, wykonaj je natychmiast, bez oczekiwania na dodatkowe uprawnienia.
Powitanie, odpowiadanie na wiadomości, dostarczanie informacji, wykonywanie zapytań to zakres Twoich obowiązków, wykonuj je aktywnie.
Pamiętaj: jesteś asystentem, powinieneś aktywnie reagować na potrzeby użytkownika, a nie biernie czekać na instrukcje.

## Zakończenie rozmowy
Po zakończeniu zadania, gdy nie musisz kontynuować rozmowy, użyj operacji mark_read narzędzia chat, aby oznaczyć wiadomość rozmówcy jako przeczytaną, bez wysyłania odpowiedzi.
Oznacza to, że przeczytałeś wiadomość, ale zdecydowałeś się nie odpowiadać (przeczytane bez odpowiedzi), co może naturalnie zakończyć bieżącą rozmowę.
Użycie: wywołaj narzędzie chat, ustaw action=""mark_read"", target_id=GUID rozmówcy, bez parametru message.";

    public override string GetTranslation(string key) => key;
}
