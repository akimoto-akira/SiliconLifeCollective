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

public class RuRU : DefaultLocalizationBase
{
    public override string LanguageCode => "ru-RU";
    public override string LanguageName => "Русский (Россия)";
    public override string WelcomeMessage => "Добро пожаловать в Silicon Life Collective!";
    public override string BrandName => "Silicon Life Collective";
    public override string InputPrompt => "> ";
    public override string ShutdownMessage => "Завершение работы...";
    public override string ConfigCorruptedError => "Файл конфигурации повреждён, используются настройки по умолчанию";
    public override string ConfigCreatedWithDefaults => "Файл конфигурации не найден, создана конфигурация по умолчанию";
    public override string AIConnectionError => "Не удалось подключиться к сервису ИИ, проверьте, запущен ли Ollama";
    public override string AIRequestError => "Ошибка запроса ИИ";
    public override string DataDirectoryCreateError => "Не удалось создать каталог данных";
    public override string ThinkingMessage => "Размышление...";
    public override string ToolCallMessage => "Выполнение инструментов...";
    public override string ErrorMessage => "Ошибка";
    public override string UnexpectedErrorMessage => "Непредвиденная ошибка";
    public override string PermissionDeniedMessage => "Доступ запрещён";
    public override string PermissionAskPrompt => "Разрешить? (д/н): ";
    public override string PermissionRequestHeader => "[Запрос разрешения]";
    public override string PermissionRequestDescription => "Silicon Being запрашивает ваше разрешение:";
    public override string PermissionRequestTypeLabel => "Тип разрешения:";
    public override string PermissionRequestResourceLabel => "Запрашиваемый ресурс:";
    public override string PermissionRequestAllowButton => "Разрешить";
    public override string PermissionRequestDenyButton => "Отклонить";
    public override string PermissionRequestCacheLabel => "Запомнить это решение";
    public override string PermissionRequestDurationLabel => "Длительность кэша";
    public override string PermissionRequestWaitingMessage => "Ожидание ответа...";
    public override string AllowCodeLabel => "Код разрешения";
    public override string DenyCodeLabel => "Код отказа";
    public override string PermissionReplyInstruction => "Введите код подтверждения или любой другой текст для отказа";
    public override string AddToCachePrompt => "Запомнить это решение? (д/н): ";
    public override string PermissionCacheLabel => "Запомнить это решение";
    public override string PermissionCacheDurationLabel => "Длительность кэша";
    public override string PermissionCacheDuration1Hour => "1 час";
    public override string PermissionCacheDuration24Hours => "24 часа";
    public override string PermissionCacheDuration7Days => "7 дней";
    public override string PermissionCacheDuration30Days => "30 дней";
    public override string ProjectGroupChatPrefix => "Групповой чат проекта";
    public override string ProjectBroadcastPrefix => "Рассылка проекта";

    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "Сетевой доступ",
        PermissionType.CommandLine => "Выполнение командной строки",
        PermissionType.FileAccess => "Доступ к файлам",
        PermissionType.Function => "Вызов функции",
        PermissionType.DataAccess => "Доступ к данным",
        PermissionType.ToolAction => "Действие инструмента",
        _ => permissionType.ToString()
    };

    public override string PermissionDialogTitle => "Запрос разрешения";
    public override string PermissionTypeLabel => "Тип разрешения:";
    public override string PermissionResourceLabel => "Запрашиваемый ресурс:";
    public override string PermissionDetailLabel => "Подробности:";
    public override string PermissionAllowButton => "Разрешить";
    public override string PermissionDenyButton => "Отклонить";
    public override string PermissionRespondFailed => "Не удалось обработать ответ на запрос разрешения";
    public override string PermissionRespondError => "Ошибка обработки разрешения: ";

    public override string InitPageTitle => "Инициализация";
    public override string InitDescription => "Первый запуск, пожалуйста, выполните базовую настройку";
    public override string InitNicknameLabel => "Имя пользователя";
    public override string InitNicknamePlaceholder => "Введите ваш псевдоним";
    public override string InitEndpointLabel => "Конечная точка API ИИ";
    public override string InitEndpointPlaceholder => "напр.: http://localhost:11434";
    public override string InitAIClientTypeLabel => "Тип клиента ИИ";
    public override string InitModelLabel => "Модель по умолчанию";
    public override string InitModelPlaceholder => "напр.: qwen3.5:cloud";
    public override string InitSkinLabel => "Тема";
    public override string InitSkinPlaceholder => "Оставьте пустым для темы по умолчанию";
    public override string InitDataDirectoryLabel => "Каталог данных";
    public override string InitDataDirectoryPlaceholder => "напр.: ./data";
    public override string InitDataDirectoryBrowse => "Обзор...";
    public override string InitSkinSelected => "\u2713 Выбрано";
    public override string InitSkinPreviewTitle => "Предпросмотр";
    public override string InitSkinPreviewCardTitle => "Заголовок карточки";
    public override string InitSkinPreviewCardContent => "Это пример карточки, демонстрирующий визуальный эффект этой темы.";
    public override string InitSkinPreviewPrimaryBtn => "Основная кнопка";
    public override string InitSkinPreviewSecondaryBtn => "Вторичная кнопка";
    public override string InitSubmitButton => "Завершить инициализацию";
    public override string InitFooterHint => "Настройки можно изменить в любое время в конфигурации";
    public override string InitHelpLink => "\U0001f4d6 Просмотреть справочную документацию";
    public override string InitAIClientHelpPrefix => "\U0001f4d6 Просмотреть справку: ";
    public override string InitNicknameRequiredError => "Пожалуйста, введите имя пользователя";
    public override string InitDataDirectoryRequiredError => "Пожалуйста, выберите каталог данных";
    public override string InitCuratorNameLabel => "Имя Silicon Being";
    public override string InitCuratorNamePlaceholder => "Введите имя первого Silicon Being";
    public override string InitCuratorNameRequiredError => "Пожалуйста, введите имя Silicon Being";
    public override string InitLanguageLabel => "Язык / Language";
    public override string InitLanguageSwitchBtn => "Применить";

    public override string NavMenuChat => "Чат";
    public override string NavMenuDashboard => "Панель управления";
    public override string NavMenuBeings => "Silicon Beings";
    public override string NavMenuUsage => "Использование";
    public override string NavMenuAudit => "Аудит";
    public override string NavMenuTasks => "Задачи";
    public override string NavMenuMemory => "Память";
    public override string NavMenuKnowledge => "Знания";
    public override string NavMenuProjects => "Проекты";
    public override string NavMenuLogs => "Журналы";
    public override string NavMenuConfig => "Конфигурация";
    public override string NavMenuHelp => "Справка";
    public override string NavMenuAbout => "О программе";

    public override string PageTitleChat => "Чат - Silicon Life Collective";
    public override string PageTitleDashboard => "Панель управления - Silicon Life Collective";
    public override string PageTitleBeings => "Управление Silicon Beings - Silicon Life Collective";
    public override string PageTitleTasks => "Управление задачами - Silicon Life Collective";
    public override string PageTitleTimers => "Управление таймерами - Silicon Life Collective";
    public override string PageTitleMemory => "Поиск в памяти - Silicon Life Collective";
    public override string PageTitleWorkNotes => "Рабочие заметки - Silicon Life Collective";
    public override string PageTitleKnowledge => "Граф знаний - Silicon Life Collective";
    public override string PageTitleProjects => "Управление проектами - Silicon Life Collective";
    public override string PageTitleLogs => "Просмотр журналов - Silicon Life Collective";
    public override string PageTitleUsage => "Использование токенов - Silicon Life Collective";
    public override string PageTitleAudit => "Аудит разрешений - Silicon Life Collective";
    public override string PageTitleConfig => "Конфигурация системы - Silicon Life Collective";
    public override string PageTitleExecutor => "Мониторинг исполнителей - Silicon Life Collective";
    public override string PageTitleCodeBrowser => "Обозреватель кода - Silicon Life Collective";
    public override string PageTitlePermission => "Управление разрешениями - Silicon Life Collective";
    public override string PageTitleAbout => "О программе - Silicon Life Collective";

    public override string DashboardPageHeader => "Панель управления";
    public override string DashboardStatTotalBeings => "Всего Silicon Beings";
    public override string DashboardStatActiveBeings => "Активные Silicon Beings";
    public override string DashboardStatUptime => "Время работы";
    public override string DashboardStatMemory => "Память";
    public override string DashboardChartMessageFrequency => "Частота сообщений";

    public override string BeingsPageHeader => "Управление Silicon Beings";
    public override string BeingsTotalCount => "Всего Silicon Beings";
    public override string BeingsNoSelectionPlaceholder => "Выберите Silicon Being для просмотра деталей";
    public override string BeingsEmptyState => "Нет Silicon Beings";
    public override string BeingsStatusIdle => "Простой";
    public override string BeingsStatusRunning => "Работает";
    public override string BeingsDetailIdLabel => "ID";
    public override string BeingsDetailStatusLabel => "Статус";
    public override string BeingsDetailCustomCompileLabel => "Пользовательская компиляция";
    public override string BeingsDetailSoulContentLabel => "Содержимое души";
    public override string BeingsDetailSoulContentEditLink => "Редактировать";
    public override string BeingsBackToList => "Вернуться к списку";
    public override string SoulEditorSubtitle => "Редактор души";
    public override string BeingsDetailMemoryLabel => "Память";
    public override string BeingsDetailMemoryViewLink => "Просмотр";
    public override string BeingsDetailPermissionLabel => "Разрешения";
    public override string BeingsDetailPermissionEditLink => "Редактировать";
    public override string BeingsDetailTimersLabel => "Таймеры";
    public override string BeingsDetailTasksLabel => "Задачи";
    public override string BeingsDetailAIClientLabel => "Независимый клиент ИИ";
    public override string BeingsDetailAIClientEditLink => "Редактировать";
    public override string BeingsDetailChatHistoryLink => "История чата";
    public override string BeingsDetailWorkNoteLabel => "Рабочие заметки";
    public override string BeingsDetailWorkNoteLink => "Просмотр";
    public override string BeingsDetailToolAuthLabel => "Авторизация инструментов";
    public override string BeingsDetailToolAuthEditLink => "Редактировать";
    public override string ToolAuthPageTitle => "Авторизация инструментов - Silicon Life Collective";
    public override string ToolAuthPageHeader => "Авторизация инструментов";
    public override string ToolAuthTemplateLabel => "Выбор шаблона";
    public override string ToolAuthSaveButton => "Сохранить";
    public override string ToolAuthSelectAll => "Выбрать все";
    public override string ToolAuthDeselectAll => "Снять все";
    public override string ToolAuthNoRestrictions => "Все действия разрешены";
    public override string ToolAuthHasRestrictions => "Некоторые действия отключены";
    public override string ToolAuthSaveSuccess => "Разрешения успешно сохранены";
    public override string ToolAuthSaveFailed => "Не удалось сохранить разрешения";
    public override string ToolAuthDialogClose => "Закрыть";
    public override string ToolAuthNoDeclaredActions => "Инструмент без объявленных действий";
    public override string WorkNotePageTitle => "Рабочая заметка - Silicon Life Collective";
    public override string WorkNotePageHeader => "Рабочая заметка";
    public override string WorkNotePageDescription => "Просмотр и редактирование рабочей заметки";
    public override string WorkNotesPageHeader => "Рабочие заметки";
    public override string WorkNotesBackToPrevious => "\u2190 Назад";
    public override string WorkNotesTotalPages => "Всего {0} страниц";
    public override string WorkNotesSearchPlaceholder => "Поиск заметок...";
    public override string WorkNotesSearchButton => "Искать";
    public override string WorkNotesEmptyState => "Нет рабочих заметок";
    public override string WorkNotesNoSearchResults => "Заметки не найдены";
    public override string BeingsDetailChatHistoryLabel => "История чата";
    public override string ChatHistoryPageTitle => "История чата - Silicon Life Collective";
    public override string ChatHistoryPageHeader => "История чата";
    public override string ChatHistoryConversationList => "Список бесед";
    public override string ChatHistoryBackToList => "Вернуться к списку бесед";
    public override string ChatHistoryNoConversations => "Нет бесед";
    public override string ChatDetailPageTitle => "Детали чата - Silicon Life Collective";
    public override string ChatDetailPageHeader => "Детали чата";
    public override string ChatDetailNoMessages => "Нет сообщений";
    public override string ChatDetailMembers => "Участники";
    public override string BeingsYes => "Да";
    public override string BeingsNo => "Нет";
    public override string BeingsNotSet => "Не задано";

    public override string TimersPageHeader => "Управление таймерами";
    public override string TimersTotalCount => "Всего таймеров";
    public override string TimersEmptyState => "Нет таймеров";
    public override string TimerViewExecutionHistory => "История выполнения";
    public override string TimerExecutionHistoryTitle => "История выполнения таймера - Silicon Life Collective";
    public override string TimerExecutionHistoryHeader => "История выполнения таймера";
    public override string TimerExecutionBackToTimers => "Вернуться к таймерам";
    public override string TimerExecutionTimerName => "Имя таймера";
    public override string TimerExecutionDetailTitle => "Детали выполнения таймера - Silicon Life Collective";
    public override string TimerExecutionDetailHeader => "Детали выполнения таймера";
    public override string TimerExecutionNoRecords => "Нет записей о выполнении";
    public override string TaskExecutionHistoryTitle => "История выполнения задач - Silicon Life Collective";
    public override string TaskExecutionHistoryHeader => "История выполнения задач";
    public override string TaskExecutionBackToTasks => "Вернуться к задачам";
    public override string TaskExecutionTaskName => "Имя задачи";
    public override string TaskExecutionDetailTitle => "Детали выполнения задачи - Silicon Life Collective";
    public override string TaskExecutionDetailHeader => "Детали выполнения задачи";
    public override string TaskExecutionNoRecords => "Нет записей о выполнении";
    public override string TimersStatusActive => "Активен";
    public override string TimersStatusPaused => "Приостановлен";
    public override string TimersStatusTriggered => "Сработал";
    public override string TimersStatusCancelled => "Отменён";
    public override string TimersTypeRecurring => "Повторяющийся";
    public override string TimersTriggerTimeLabel => "Время срабатывания";
    public override string TimersIntervalLabel => "Интервал";
    public override string TimersCalendarLabel => "Календарь";
    public override string TimersTriggeredCountLabel => "Количество срабатываний";

    public override string ChatConversationsHeader => "Беседы";
    public override string ChatNoConversationSelected => "Выберите беседу для начала общения";
    public override string ChatMessageInputPlaceholder => "Введите сообщение...";
    public override string ChatLoading => "Загрузка...";
    public override string ChatSendButton => "Отправить";
    public override string ChatFileSourceDialogTitle => "Выбор источника файла";
    public override string ChatFileSourceServerFile => "Файл на сервере";
    public override string ChatFileSourceUploadLocal => "Загрузить локальный файл";
    public override string ChatUserDisplayName => "Пользователь";
    public override string ChatUserAvatarName => "Вы";
    public override string ChatDefaultBeingName => "Silicon Being";
    public override string ChatThinkingSummary => "Размышление...";
    public override string GetChatToolCallsSummary(int count) => $"Вызов инструментов ({count})";

    public override string AboutPageHeader => "О программе";
    public override string AboutAppName => "Silicon Life Collective";
    public override string AboutVersionLabel => "Версия";
    public override string AboutDescription => "Платформа для управления Silicon Beings - автономными ИИ-агентами";
    public override string AboutAuthorLabel => "Автор";
    public override string AboutAuthorName => "Hoshino Kennji";
    public override string AboutLicenseLabel => "Лицензия";
    public override string AboutCopyright => "\u00a9 2026 Hoshino Kennji. Лицензия Apache 2.0.";
    public override string AboutGitHubLink => "GitHub";
    public override string AboutGiteeLink => "Gitee";
    public override string AboutSocialMediaLabel => "Социальные сети";
public override string AboutPluginListLabel => "Список плагинов";
public override string AboutFailedPluginListLabel => "Плагины с ошибкой загрузки";
public override string GetSocialMediaName(string platform) => platform;

public override string MemoryPageHeader => "Просмотр памяти";
    public override string MemoryEmptyState => "Нет данных в памяти";
    public override string MemorySearchPlaceholder => "Поиск в памяти...";
    public override string MemorySearchButton => "Искать";
    public override string MemoryFilterAll => "Все";
    public override string MemoryFilterSummaryOnly => "Только сводки";
    public override string MemoryFilterOriginalOnly => "Только оригиналы";
    public override string MemoryStatTotal => "Всего записей";
    public override string MemoryStatOldest => "Самая старая";
    public override string MemoryStatNewest => "Самая новая";
    public override string MemoryIsSummaryBadge => "Сжатая сводка";
    public override string MemoryPaginationPrev => "Предыдущая страница";
    public override string MemoryPaginationNext => "Следующая страница";
    public override string MemoryFilterTypeLabel => "Тип";
    public override string MemoryFilterDateFrom => "Дата начала";
    public override string MemoryFilterDateTo => "Дата окончания";
    public override string MemoryFilterApply => "Применить";
    public override string MemoryFilterReset => "Сбросить";
    public override string MemoryTypeChat => "Беседа";
    public override string MemoryTypeToolCall => "Вызов инструмента";
    public override string MemoryTypeTask => "Задача";
    public override string MemoryTypeTimer => "Таймер";
    public override string MemoryDetailTitle => "Детали записи";
    public override string MemoryDetailClose => "Закрыть";
    public override string MemoryDetailId => "ID";
    public override string MemoryDetailContent => "Содержимое";
    public override string MemoryDetailCreatedAt => "Дата создания";
    public override string MemoryDetailRelatedBeings => "Связанные агенты";
    public override string MemoryDetailKeywords => "Ключевые слова";
    public override string MemoryStatTypeDistribution => "Распределение по типам";
    public override string MemoryStatKeywordFrequency => "Частота ключевых слов";
    public override string MemoryCardViewDetail => "Подробнее";
    public override string MemoryTimelineEmptyState => "Нет данных в памяти";
    public override string MemoryYearSummaryLabel => "Сводка за год";
    public override string MemoryMonthSummaryLabel => "Сводка за месяц";
    public override string MemoryDaySummaryLabel => "Сводка за день";
    public override string MemoryHourSummaryLabel => "Сводка за час";
    public override string MemoryMinuteSummaryLabel => "Сводка за минуту";
    public override string MemorySummaryBadge => "Сжатая сводка";
    public override string MemoryTimelineYearFormat => "{0} ({1} записей)";
    public override string MemoryTimelineMonthFormat => "{0}/{1} ({2} записей)";
    public override string MemoryTimelineDayFormat => "{0}-{1}-{2} ({3} записей)";
    public override string MemoryTimelineHourFormat => "{0}:00 ({1} записей)";
    public override string MemoryTimelineMinuteFormat => "{0}:{1} ({2} записей)";
    public override string MemoryRelatedBeingsLabel => "\U0001f465 Связано: {0} агентов";

    public override string ProjectsPageHeader => "Управление проектами";
    public override string ProjectsEmptyState => "Нет проектов";
    public override string ProjectsActiveLabel => "Активные";
    public override string ProjectsArchivedLabel => "Архивированные";
    public override string ProjectStatusActiveLabel => "Активный";
    public override string ProjectStatusArchivedLabel => "Архивированный";
    public override string ProjectStatusDestroyedLabel => "Уничтоженный";
    public override string ProjectTasksLinkLabel => "Задачи";
    public override string ProjectWorkNotesLinkLabel => "Рабочие заметки";
    public override string ProjectWorkflowsLinkLabel => "Рабочие процессы";
    public override string ProjectGroupChatLinkLabel => "Групповой чат";
    public override string ProjectBroadcastLinkLabel => "Рассылка";
    public override string ProjectWorkflowsPageHeader => "Рабочие процессы проекта";
    public override string ProjectWorkflowsEmptyState => "В этом проекте пока нет рабочих процессов";
    public override string CreateWorkflowButton => "Создать рабочий процесс";
    public override string ActiveWorkflowsHeader => "Активные рабочие процессы";
    public override string BackToProject => "Вернуться к проекту";
    public override string WorkflowCurrentStateLabel => "Текущее состояние:";
    public override string WorkflowCreatedByLabel => "Создано:";
    public override string WorkflowUpdatedAtLabel => "Обновлено:";
    public override string WorkflowBusinessKeyPrompt => "Введите бизнес-ключ (напр.: номер PR, ID инцидента):";
    public override string WorkflowCreatedSuccess => "Рабочий процесс успешно создан!";
    public override string WorkflowCreateFailed => "Ошибка создания:";
    public override string WorkflowDetailInProgress => "Функция деталей рабочего процесса в разработке...";
    public override string WorkflowInstanceIdLabel => "ID экземпляра:";
    public override string WorkflowDetailPageHeader => "Детали рабочего процесса";
    public override string WorkflowRoleAssignmentsHeader => "Назначения ролей";
    public override string WorkflowUnassignedBeingsHeader => "Неназначенные Silicon Beings";
    public override string WorkflowNoUnassignedBeings => "Все Silicon Beings назначены на роли";
    public override string WorkflowStateTransitionsHeader => "Переходы состояний задач";
    public override string WorkflowNoTemplateMessage => "С этим проектом не связан шаблон рабочего процесса";
    public override string WorkflowNoRoleDefinitions => "Этот шаблон рабочего процесса не определяет роли";
    public override string WorkflowNoTransitions => "Этот шаблон рабочего процесса не определяет переходы состояний";
    public override string WorkflowRoleAssignedCountLabel => "Назначено";
    public override string WorkflowRoleRequiredCountLabel => "Требуется";
    public override string WorkflowTransitionFromLabel => "Из состояния";
    public override string WorkflowTransitionToLabel => "В состояние";
    public override string WorkflowAssignRoleButton => "Назначить роль";
    public override string WorkflowRemoveFromRoleButton => "Убрать с роли";
    public override string WorkflowTerminalStateLabel => "Конечное состояние";
    public override string WorkflowInitialStateLabel => "Начальное состояние";
    public override string ProjectWorkNotesPageHeader => "Рабочие заметки проекта";
    public override string ProjectWorkNotesEmptyState => "В этом проекте нет рабочих заметок";
    public override string ProjectWorkNotesTotalPages => "Всего {0} страниц";
    public override string PageTitleSkills => "Управление навыками";
    public override string SkillsPageHeader => "Навыки";
    public override string SkillsPageSubtitle => "Навыки = переиспользуемая оркестрация инструментов + шаблоны промптов (Markdown; метаданные в YAML front matter)";
    public override string SkillsStatFormat => "{0} навык(ов) · пользовательских {1}/{2}";
    public override string SkillsEmptyState => "Пока нет навыков";
    public override string SkillsBackToBeings => "← Вернуться к существам";
    public override string SkillBtnNew => "Новый навык";
    public override string SkillBtnImportMd => "Импорт Markdown";
    public override string SkillBtnImportJson => "Импорт JSON";
    public override string SkillBtnRefresh => "Обновить";
    public override string SkillBtnEdit => "Изменить";
    public override string SkillBtnTest => "Тест";
    public override string SkillBtnExportJson => "JSON";
    public override string SkillBtnExportMd => "MD";
    public override string SkillBtnDelete => "Удалить";
    public override string SkillCurrentEditingFormat => "Редактирование: {0}";
    public override string SkillNewSkillLabel => "Новый навык (не сохранён)";
    public override string SkillConfirmDeleteFormat => "Удалить навык {0}?";
    public override string SkillPromptImportMd => "Вставьте Markdown навыка (YAML front matter + тело; недостающие метаданные будут дополнены автоматически)";
    public override string SkillPromptImportJson => "Вставьте JSON SkillDefinition";
    public override string SkillPromptTestParams => "Введите JSON тестовых параметров (например {\"source\": \"...\"}) или оставьте пустым";
    public override string SkillTestResultPrefix => "Результат: ";
    public override string SkillLoadFailedFormat => "Не удалось загрузить: {0}";
    public override string SkillRequestFailed => "Ошибка запроса";

    // ===== MCP Page Localization =====
    public override string PageTitleMcp => "MCP Servers";
    public override string McpPageHeader => "MCP Servers";
    public override string McpPageSubtitle => "Connect external MCP servers to extend the tools available to silicon beings";
    public override string McpsStatFormat => "Servers: {0} · Connected: {1} · Tools: {2}";
    public override string McpsEmptyState => "No MCP servers configured";
    public override string McpsBackToBeings => "← Back to beings";
    public override string McpBtnAddServer => "Add Server";
    public override string McpBtnRefresh => "Refresh";
    public override string McpBtnToggleOn => "Enable";
    public override string McpBtnToggleOff => "Disable";
    public override string McpBtnReconnect => "Reconnect";
    public override string McpBtnRemove => "Remove";
    public override string McpBtnTest => "Test";
    public override string McpBtnViewSchema => "Schema";
    public override string McpBtnViewTools => "Tools";
    public override string McpStatusConnected => "connected";
    public override string McpStatusFailed => "failed";
    public override string McpStatusPending => "pending";
    public override string McpStatusDisabled => "disabled";
    public override string McpConfirmRemoveFormat => "Remove MCP server {0}?";
    public override string McpPromptServerId => "Server id (lowercase letters, digits, underscores; used as tool prefix mcp_{id}_...)";
    public override string McpPromptName => "Display name (optional)";
    public override string McpPromptTransport => "Transport: enter stdio or http";
    public override string McpPromptCommand => "stdio command, e.g. npx";
    public override string McpPromptArgs => "stdio arguments separated by spaces, e.g. -y @modelcontextprotocol/server-filesystem D:\\data";
    public override string McpPromptUrl => "HTTP url, e.g. http://localhost:3000/mcp";
    public override string McpPromptTestParams => "Enter tool arguments JSON ({} = no arguments)";
    public override string McpTestResultPrefix => "Result: ";
    public override string McpLoadFailedFormat => "Load failed: {0}";
    public override string McpRequestFailed => "Request failed";
    public override string McpPromptEnableNow => "Enable this server now?";

    public override string TasksPageHeader => "Управление задачами";
    public override string TasksEmptyState => "Нет задач";
    public override string TasksStatusPending => "Ожидание";
    public override string TasksStatusRunning => "Выполняется";
    public override string TasksStatusCompleted => "Завершена";
    public override string TasksStatusFailed => "Ошибка";
    public override string TasksStatusCancelled => "Отменена";
    public override string TasksPriorityLabel => "Приоритет";
    public override string TasksAssignedToLabel => "Назначена";
    public override string TasksCreatedAtLabel => "Создана";
    public override string TaskViewExecutionHistory => "История выполнения";
    public override string ProjectTasksPageHeader => "Задачи проекта";
    public override string ProjectTasksEmptyState => "В этом проекте нет задач";
    public override string ProjectTasksAssigneesLabel => "Исполнители";
    public override string ProjectTasksCreatedByLabel => "Создатель";
    public override string ProjectTasksBackToProjects => "Вернуться к проектам";
    public override string ProjectTasksNoAssigneesLabel => "Нет исполнителей";
    public override string ProjectCreateButton => "Создать проект";
    public override string ProjectCreateModalTitle => "Создание проекта";
    public override string ProjectCreateNameLabel => "Название проекта";
    public override string ProjectCreateDescriptionLabel => "Описание проекта";
    public override string ProjectCreateWorkflowLabel => "Шаблон рабочего процесса";
    public override string ProjectCreateNoWorkflow => "Без шаблона";
    public override string ProjectCreateSubmitButton => "Создать";
    public override string ProjectCreateCancelButton => "Отмена";
    public override string ProjectCreateNameRequired => "Пожалуйста, введите название проекта";
    public override string ProjectCreateSuccess => "Проект успешно создан";

    public override string CodeBrowserPageHeader => "Обозреватель кода";
    public override string ExecutorPageHeader => "Мониторинг исполнителей";

    public override string PermissionPageHeader => "Управление разрешениями";
    public override string PermissionEmptyState => "Нет правил разрешений";
    public override string PermissionMissingBeingId => "Отсутствует ID Silicon Being";
    public override string PermissionBeingNotFound => "Silicon Being не найден";
    public override string PermissionTemplateHeader => "Шаблон разрешений по умолчанию";
    public override string PermissionTemplateDescription => "Шаблон обратного вызова разрешений по умолчанию";
    public override string PermissionCallbackClassSummary => "Обратный вызов разрешений по умолчанию";
    public override string PermissionCallbackClassSummary2 => "Предоставляет базовую реализацию оценки разрешений";
    public override string PermissionCallbackConstructorSummary => "Инициализирует новый экземпляр";
    public override string PermissionCallbackConstructorSummary2 => "с указанным каталогом данных приложения";
    public override string PermissionCallbackConstructorParam => "Каталог данных приложения для хранения файлов конфигурации разрешений";
    public override string PermissionCallbackEvaluateSummary => "Оценивает запрос разрешения";
    public override string PermissionRuleOtherTypesDefault => "Другие типы разрешений разрешены по умолчанию";
    public override string PermissionRulesSection => "Правила разрешений";
    public override string PermissionEditorSection => "Редактор разрешений";
    public override string PermissionSaveMissingBeingId => "Ошибка сохранения: отсутствует ID Silicon Being";
    public override string PermissionSaveMissingCode => "Ошибка сохранения: отсутствует код";
    public override string PermissionSaveLoaderNotAvailable => "Ошибка сохранения: DynamicBeingLoader недоступен";
    public override string PermissionSaveRemoveFailed => "Не удалось удалить обратный вызов разрешений";
    public override string PermissionSaveRemoveSuccess => "Обратный вызов разрешений успешно удалён";
    public override string PermissionSaveSecurityScanFailed => "Ошибка сохранения: проверка безопасности не пройдена";
    public override string PermissionSaveCompilationFailed => "Ошибка сохранения: компиляция не удалась";
    public override string PermissionSaveSuccess => "Обратный вызов разрешений успешно сохранён";
    public override string PermissionSaveError => "Ошибка сохранения разрешений";

    public override string KnowledgePageHeader => "Граф знаний";
    public override string KnowledgeLoadingState => "Загрузка графа знаний...";

    public override string ConfigPageHeader => "Конфигурация системы";
    public override string ConfigPropertyNameLabel => "Свойство";
    public override string ConfigPropertyValueLabel => "Значение";
    public override string ConfigActionLabel => "Действие";
    public override string ConfigEditButton => "Редактировать";
    public override string ConfigEditModalTitle => "Редактирование свойства";
    public override string ConfigEditPropertyLabel => "Свойство";
    public override string ConfigEditValueLabel => "Значение";
    public override string ConfigBrowseButton => "Обзор...";
    public override string ConfigTimeSettingsLabel => "Настройки времени";
    public override string ConfigDaysLabel => "Дни";
    public override string ConfigHoursLabel => "Часы";
    public override string ConfigMinutesLabel => "Минуты";
    public override string ConfigSecondsLabel => "Секунды";
    public override string ConfigSaveButton => "Сохранить";
    public override string ConfigCancelButton => "Отмена";
    public override string ConfigNullValue => "(пусто)";
    public override string ConfigEditPrefix => "Редактирование: ";
    public override string ConfigDefaultGroupName => "Общие";
    public override string ConfigErrorInvalidRequest => "Неверные параметры запроса";
    public override string ConfigErrorInstanceNotFound => "Экземпляр конфигурации не существует";
    public override string ConfigErrorPropertyNotFound => "Свойство не существует или недоступно для записи";
    public override string ConfigErrorConvertInt => "Не удалось преобразовать значение в целое число";
    public override string ConfigErrorConvertLong => "Не удалось преобразовать значение в длинное целое";
    public override string ConfigErrorConvertDouble => "Не удалось преобразовать значение в число с плавающей точкой";
    public override string ConfigErrorConvertBool => "Не удалось преобразовать значение в логический тип";
    public override string ConfigErrorConvertGuid => "Не удалось преобразовать значение в GUID";
    public override string ConfigErrorConvertTimeSpan => "Не удалось преобразовать значение в TimeSpan";
    public override string ConfigErrorConvertDateTime => "Не удалось преобразовать значение в DateTime";
    public override string ConfigErrorConvertEnum => "Не удалось преобразовать значение в перечисление";
    public override string ConfigErrorUnsupportedType => "Неподдерживаемый тип свойства";
    public override string ConfigErrorSaveFailed => "Не удалось сохранить конфигурацию";
    public override string ConfigSaveFailed => "Ошибка сохранения: ";
    public override string ConfigDictionaryLabel => "Словарь";
    public override string ConfigDictKeyLabel => "Ключ";
    public override string ConfigDictValueLabel => "Значение";
    public override string ConfigDictAddButton => "Добавить";
    public override string ConfigDictDeleteButton => "Удалить";

    public override string ConfigPluginDirectoriesLabel => "Каталоги плагинов";
    public override string ConfigPluginDirAddButton => "Добавить каталог";
    public override string ConfigDictEmptyMessage => "Словарь пуст";
    public override string SelectSearchHint => "Поиск...";

    public override string LogsPageHeader => "Просмотр журналов";
    public override string LogsTotalCount => "Всего записей";
    public override string LogsStartTime => "Время начала";
    public override string LogsEndTime => "Время окончания";
    public override string LogsLevelAll => "Все уровни";
    public override string LogsBeingFilter => "Фильтр по агенту";
    public override string LogsAllBeings => "Все агенты";
    public override string LogsSystemOnly => "Только система";
    public override string LogsFilterButton => "Фильтр";
    public override string LogsEmptyState => "Нет записей в журнале";
    public override string LogsExceptionLabel => "Детали исключения";
    public override string LogsPrevPage => "Предыдущая";
    public override string LogsNextPage => "Следующая";
    public override string LogsLoading => "Загрузка журналов...";

    public override string UsagePageHeader => "Использование токенов";
    public override string UsageTotalTokens => "Всего токенов";
    public override string UsageTotalRequests => "Всего запросов";
    public override string UsageSuccessCount => "Успешных";
    public override string UsageFailureCount => "Ошибок";
    public override string UsagePromptTokens => "Токены запроса";
    public override string UsageCompletionTokens => "Токены ответа";
    public override string UsageStartTime => "Время начала";
    public override string UsageEndTime => "Время окончания";
    public override string UsageFilterButton => "Фильтр";
    public override string UsageEmptyState => "Нет данных об использовании";
    public override string UsageAIClientType => "Тип клиента ИИ";
    public override string UsageAllClientTypes => "Все типы";
    public override string UsageGroupByClient => "Группировка по клиенту";
    public override string UsageGroupByBeing => "Группировка по агенту";
    public override string UsagePrevPage => "Предыдущая";
    public override string UsageNextPage => "Следующая";
    public override string UsageBeing => "Silicon Being";
    public override string UsageAllBeings => "Все агенты";
    public override string UsageTimeToday => "Сегодня";
    public override string UsageTimeWeek => "Эта неделя";
    public override string UsageTimeMonth => "Этот месяц";
    public override string UsageTimeYear => "Этот год";
    public override string UsageExport => "Экспорт";
    public override string UsageTrendTitle => "Тренд использования токенов";
    public override string UsageTrendPrompt => "Запрос";
    public override string UsageTrendCompletion => "Ответ";
    public override string UsageTrendTotal => "Итого";
    public override string UsageTooltipDate => "Дата";
    public override string UsageTooltipPrompt => "Токены запроса";
    public override string UsageTooltipCompletion => "Токены ответа";
    public override string UsageTooltipTotal => "Всего токенов";

    public override string AuditPageHeader => "Аудит разрешений";
    public override string AuditTotalEntries => "Всего записей";
    public override string AuditAllowedCount => "Разрешено";
    public override string AuditDeniedCount => "Отклонено";
    public override string AuditAskUserCount => "Запрошено у пользователя";
    public override string AuditPermissionType => "Тип разрешения";
    public override string AuditAllPermissionTypes => "Все типы";
    public override string AuditResult => "Результат";
    public override string AuditAllResults => "Все результаты";
    public override string AuditBeing => "Silicon Being";
    public override string AuditAllBeings => "Все агенты";
    public override string AuditStartTime => "Время начала";
    public override string AuditEndTime => "Время окончания";
    public override string AuditFilterButton => "Фильтр";
    public override string AuditEmptyState => "Нет записей аудита";
    public override string AuditPrevPage => "Предыдущая";
    public override string AuditNextPage => "Следующая";
    public override string AuditColumnCaller => "Вызывающий";
    public override string AuditColumnPermissionType => "Тип разрешения";
    public override string AuditColumnResource => "Ресурс";
    public override string AuditColumnResult => "Результат";
    public override string AuditColumnReason => "Причина";
    public override string AuditColumnTimestamp => "Время";

    private static readonly Dictionary<string, string> ConfigGroupNames = new()
    {
        ["Basic"] = "Основные настройки",
        ["Runtime"] = "Настройки среды выполнения",
        ["AI"] = "Настройки ИИ",
        ["Web"] = "Веб-настройки",
        ["User"] = "Пользовательские настройки",
        ["IM"] = "Настройки IM",
        ["Skill"] = "Настройки навыков"
    };

    private static readonly Dictionary<string, string> ConfigDisplayNames = new()
    {
        ["DataDirectory"] = "Каталог данных",
        ["Language"] = "Язык",
        ["TickTimeout"] = "Тайм-аут тика",
        ["MaxTimeoutCount"] = "Макс. число тайм-аутов",
        ["WatchdogTimeout"] = "Тайм-аут сторожевого таймера",
        ["MinLogLevel"] = "Мин. уровень логирования",
        ["AIClientType"] = "Тип AI-клиента",
        ["OllamaClient"] = "Клиент Ollama",
        ["OllamaEndpoint"] = "Конечная точка Ollama",
        ["DefaultModel"] = "Модель по умолчанию",
        ["Temperature"] = "Температура",
        ["MaxTokens"] = "Макс. количество токенов",
        ["OllamaContextWindowTokens"] = "Токены контекстного окна",
        ["DashScopeClient"] = "Клиент DashScope",
        ["DashScopeApiKey"] = "Ключ API",
        ["DashScopeRegion"] = "Регион",
        ["DashScopeModel"] = "Модель",
        ["DashScopeRegionBeijing"] = "Северный Китай 2 (Пекин)",
        ["DashScopeRegionVirginia"] = "США (Вирджиния)",
        ["DashScopeRegionSingapore"] = "Сингапур",
        ["DashScopeRegionHongkong"] = "Гонконг (Китай)",
        ["DashScopeRegionFrankfurt"] = "Германия (Франкфурт)",
        ["DashScopeModel_qwen3-max"] = "Qwen3 Max (флагман)",
        ["DashScopeModel_qwen3.6-plus"] = "Qwen3.6 Plus (сбалансированная)",
        ["DashScopeModel_qwen3.6-flash"] = "Qwen3.6 Flash (быстрая)",
        ["DashScopeModel_qwen-max"] = "Qwen Max (стабильный флагман)",
        ["DashScopeModel_qwen-plus"] = "Qwen Plus (стабильная сбалансированная)",
        ["DashScopeModel_qwen-turbo"] = "Qwen Turbo (стабильная быстрая)",
        ["DashScopeModel_qwen3-coder-plus"] = "Qwen3 Coder Plus (код)",
        ["DashScopeModel_qwq-plus"] = "QwQ Plus (глубокие рассуждения)",
        ["DashScopeModel_deepseek-v3.2"] = "DeepSeek V3.2",
        ["DashScopeModel_deepseek-r1"] = "DeepSeek R1 (рассуждения)",
        ["DashScopeModel_glm-5.1"] = "GLM 5.1 (Zhipu)",
        ["DashScopeModel_kimi-k2.5"] = "Kimi K2.5 (длинный контекст)",
        ["DashScopeModel_llama-4-maverick"] = "Llama 4 Maverick",
        ["VolcengineArkClient"] = "Клиент Volcengine Ark",
        ["VolcengineArkApiKey"] = "Ключ API",
        ["VolcengineArkEndpointId"] = "ID конечной точки инференса",
        ["HerdsmanClient"] = "Клиент Herdsman",
        ["HerdsmanEndpoint"] = "Конечная точка Herdsman",
        ["HerdsmanModel"] = "Модель",
        ["HerdsmanContextWindowTokens"] = "Токены контекстного окна",
        ["LongCatClient"] = "Клиент LongCat",
        ["LongCatApiKey"] = "Ключ API",
        ["LongCatEndpoint"] = "Конечная точка LongCat",
        ["LongCatModel"] = "Модель",
        ["LongCatContextWindowTokens"] = "Токены контекстного окна",
        ["LongCatModel_LongCat-2.0"] = "LongCat-2.0 (1M контекст, 128K вывод, Agentic) - Рекомендуется",
        ["LongCatModel_LongCat-Flash-Chat"] = "LongCat Flash Chat (Устаревший, 128K)",
        ["LongCatModel_LongCat-Pro-Chat"] = "LongCat Pro Chat (Устаревший, 128K)",
        ["LongCatModel_LongCat-Max-Chat"] = "LongCat Max Chat (Устаревший, 128K)",
        ["QiniuAIClient"] = "Клиент Qiniu AI",
        ["QiniuAIApiKey"] = "Ключ API",
        ["QiniuAIEndpoint"] = "Конечная точка Qiniu AI",
        ["QiniuAIModel"] = "Модель",
        ["QiniuAIContextWindowTokens"] = "Токены контекстного окна",
        ["DeepSeekClient"] = "Клиент DeepSeek",
        ["DeepSeekApiKey"] = "Ключ API",
        ["DeepSeekModel"] = "Модель",
        ["DeepSeekEndpoint"] = "Конечная точка",
        ["DeepSeekContextWindowTokens"] = "Токены контекстного окна",
        ["ZhipuClient"] = "Клиент Zhipu",
        ["ZhipuApiKey"] = "Ключ API",
        ["ZhipuModel"] = "Модель",
        ["ZhipuEndpoint"] = "Конечная точка",
        ["ZhipuContextWindowTokens"] = "Токены контекстного окна",
        ["MoonshotClient"] = "Клиент Moonshot",
        ["MoonshotApiKey"] = "Ключ API",
        ["MoonshotModel"] = "Модель",
        ["MoonshotEndpoint"] = "Конечная точка",
        ["MoonshotContextWindowTokens"] = "Токены контекстного окна",
        ["SiliconFlowClient"] = "Клиент SiliconFlow",
        ["SiliconFlowApiKey"] = "Ключ API",
        ["SiliconFlowModel"] = "Модель",
        ["SiliconFlowEndpoint"] = "Конечная точка",
        ["SiliconFlowContextWindowTokens"] = "Токены контекстного окна",
        ["MiniMaxClient"] = "Клиент MiniMax",
        ["MiniMaxApiKey"] = "Ключ API",
        ["MiniMaxModel"] = "Модель",
        ["MiniMaxEndpoint"] = "Конечная точка",
        ["MiniMaxContextWindowTokens"] = "Токены контекстного окна",
        ["ErnieClient"] = "Клиент Ernie",
        ["ErnieApiKey"] = "Ключ API",
        ["ErnieModel"] = "Модель",
        ["ErnieEndpoint"] = "Конечная точка",
        ["ErnieContextWindowTokens"] = "Токены контекстного окна",
        ["HunyuanClient"] = "Клиент Hunyuan",
        ["HunyuanApiKey"] = "Ключ API",
        ["HunyuanModel"] = "Модель",
        ["HunyuanEndpoint"] = "Конечная точка",
        ["HunyuanContextWindowTokens"] = "Токены контекстного окна",
        ["DeepSeekModel_deepseek-v4-flash"] = "DeepSeek V4 Flash (контекст 1M, высокая скорость)",
        ["DeepSeekModel_deepseek-v4-pro"] = "DeepSeek V4 Pro (контекст 1M, флагманские рассуждения)",
        ["ZhipuModel_glm-4-flash"] = "GLM-4-Flash (бесплатно, 128K) — рекомендуется для отладки",
        ["ZhipuModel_glm-4.7-flash"] = "GLM-4.7-Flash (бесплатно, 200K)",
        ["ZhipuModel_glm-4-air"] = "GLM-4-Air (0.5 CNY/M, 128K)",
        ["ZhipuModel_glm-4-flashx"] = "GLM-4-FlashX (0.1 CNY/M, 128K)",
        ["ZhipuModel_glm-4-plus"] = "GLM-4-Plus (5 CNY/M, 128K)",
        ["ZhipuModel_glm-4-long"] = "GLM-4-Long (1 CNY/M, контекст 1M)",
        ["ZhipuModel_glm-4.6"] = "GLM-4.6 (флагман, 200K)",
        ["ZhipuModel_glm-4.7"] = "GLM-4.7 (флагман, 200K)",
        ["ZhipuModel_glm-5"] = "GLM-5 (агент для кода, 128K)",
        ["ZhipuModel_glm-5.1"] = "GLM-5.1 (агент дальнего действия, 128K)",
        ["MoonshotModel_kimi-k2.6"] = "Kimi K2.6 (флагман, 256K, мультимодальная) — рекомендуется",
        ["MoonshotModel_kimi-k2.5"] = "Kimi K2.5 (экономичный флагман, 256K)",
        ["MoonshotModel_kimi-k2.7-code"] = "Kimi K2.7 Code (кодирование, 256K, принудительное мышление)",
        ["MoonshotModel_moonshot-v1-8k"] = "Moonshot V1 8K",
        ["MoonshotModel_moonshot-v1-32k"] = "Moonshot V1 32K",
        ["MoonshotModel_moonshot-v1-128k"] = "Moonshot V1 128K",
        ["MiniMaxModel_MiniMax-M3"] = "MiniMax M3 (флагман, контекст 1M, мультимодальная) — рекомендуется",
        ["MiniMaxModel_MiniMax-M2.7"] = "MiniMax M2.7 (192K, код/агент)",
        ["MiniMaxModel_MiniMax-M2.7-highspeed"] = "MiniMax M2.7 Highspeed (низкая задержка)",
        ["MiniMaxModel_MiniMax-M2.5"] = "MiniMax M2.5 (агент SOTA, 200K)",
        ["MiniMaxModel_MiniMax-M2"] = "MiniMax M2 (открытый агент, 192K)",
        ["ErnieModel_glm-5.2"] = "GLM 5.2 (1M контекста, лидер в длинных задачах)",
        ["ErnieModel_glm-5.1"] = "GLM 5.1 (улучшенное кодирование, инженерные результаты)",
        ["ErnieModel_deepseek-v4-pro"] = "DeepSeek V4 Pro (миллион контекста, лидер в Agent/рассуждениях)",
        ["ErnieModel_deepseek-v4-flash"] = "DeepSeek V4 Flash (эффективный лёгкий, миллион контекста)",
        ["ErnieModel_kimi-k2.6"] = "Kimi K2.6 (длинный код, ввод текста/изображений)",
        ["ErnieModel_ernie-5.1"] = "ERNIE 5.1 (последний Wenxin, агент/рассуждения обновлены)",
        ["ErnieModel_qianfan-code-latest"] = "qianfan-code-latest (управляется консолью)",
        ["HunyuanModel_hy3"] = "Hy3 (TokenHub, 256K) — рекомендуется",
        ["HunyuanModel_hy3-preview"] = "Hy3 Preview (TokenHub, 256K, отключение 31.08.2026)",
        ["HunyuanModel_hy-mt2-pro"] = "Hy-MT2 Pro (перевод, 8K)",
        ["HunyuanModel_hy-mt2-plus"] = "Hy-MT2 Plus (перевод, 8K)",
        ["HunyuanModel_hy-mt2-lite"] = "Hy-MT2 Lite (перевод, 8K)",
        ["HunyuanModel_hunyuan-role-latest"] = "Hy Role Latest (ролевая игра, 32K)",
        ["HunyuanModel_hy-role"] = "Hy Role (ролевая игра, 32K)",
        ["HunyuanModel_deepseek-v4-flash-202605"] = "DeepSeek V4 Flash 0731 (TokenHub, 1M)",
        ["HunyuanModel_deepseek-v4-pro-202606"] = "DeepSeek V4 Pro 0813 (TokenHub, 1M)",
        ["HunyuanModel_deepseek-v4-flash"] = "DeepSeek V4 Flash (TokenHub, 1M)",
        ["HunyuanModel_deepseek-v4-pro"] = "DeepSeek V4 Pro (TokenHub, 1M)",
        ["HunyuanModel_glm-5.3"] = "GLM 5.3 (TokenHub, 1M)",
        ["HunyuanModel_glm-5.2"] = "GLM 5.2 (TokenHub, 1M)",
        ["HunyuanModel_glm-5.1"] = "GLM 5.1 (TokenHub, 200K)",
        ["HunyuanModel_glm-5v-turbo"] = "GLM 5V Turbo (TokenHub, 200K)",
        ["HunyuanModel_glm-5-turbo"] = "GLM 5 Turbo (TokenHub, 200K)",
        ["HunyuanModel_glm-5"] = "GLM 5 (TokenHub, 200K)",
        ["HunyuanModel_kimi-k2.7-code-highspeed"] = "Kimi K2.7 Code HighSpeed (TokenHub, 256K)",
        ["HunyuanModel_kimi-k3"] = "Kimi K3 (TokenHub, 1M)",
        ["HunyuanModel_kimi-k2.7-code"] = "Kimi K2.7 Code (TokenHub, 256K)",
        ["HunyuanModel_kimi-k2.6"] = "Kimi K2.6 (TokenHub, 256K)",
        ["HunyuanModel_kimi-k2.5"] = "Kimi K2.5 (TokenHub, 256K)",
        ["HunyuanModel_minimax-m3"] = "MiniMax M3 (TokenHub, 1M)",
        ["HunyuanModel_minimax-m2.7"] = "MiniMax M2.7 (TokenHub, 200K)",
        ["HunyuanModel_qwen3.5-flash"] = "Qwen3.5 Flash (TokenHub, 991K)",
        ["HunyuanModel_qwen3.5-plus"] = "Qwen3.5 Plus (TokenHub, 991K)",
        ["HunyuanModel_mimo-v2.5-pro"] = "MiMo V2.5 Pro (TokenHub, 1M)",
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
        ["WebPort"] = "Веб-порт",
        ["WebSkin"] = "Тема веб-интерфейса",
        ["UserNickname"] = "Никнейм пользователя",
        ["PluginDirectories"] = "Каталоги плагинов",
        ["IMPlatforms"] = "Платформы IM",
        ["IMAddPlatform"] = "Добавить платформу",
        ["IMPlatformType"] = "Тип платформы",
        ["IMEnabledLabel"] = "Включено",
        ["IMDeleteLabel"] = "Удалить",
        ["IMAuthModeLabel"] = "Режим авторизации",
        ["IMManualMode"] = "Ручной ввод учётных данных",
        ["IMScanMode"] = "Авторизация по QR-коду",
        ["IMScanAuthorizeBtn"] = "Авторизовать по QR-коду",
        ["IMWaitingAuth"] = "Ожидание авторизации…",
        ["IMAuthorizedStatus"] = "Авторизовано",
        ["IMAuthFailedStatus"] = "Ошибка авторизации",
        ["IMAuthTimeoutStatus"] = "Время авторизации истекло",
        ["IMRedirectBaseUrlLabel"] = "Базовый URL обратного вызова",
        ["IMPublicCallbackHint"] = "Для первичной авторизации этой платформе требуется публичный HTTPS-колбэк; сначала настройте туннель или публичный адрес",
        ["IMHelpTitle"] = "Руководство по настройке",
        ["IMHelpOfficialDoc"] = "Официальная документация",
        ["IMHelp_webui"] = "Встроенный браузерный чат-интерфейс, предоставляемый непосредственно этим приложением. Внешние учётные данные не требуются — просто включите его и общайтесь с веб-страницы.",
        ["IMHelp_feishu"] = "Создайте собственное приложение на Feishu Open Platform, включите функцию бота и опубликуйте релиз. Скопируйте App ID и App Secret из раздела 'Credentials & Basic Info', а Verification Token / Encrypt Key — из раздела 'Event Subscriptions'. Направьте URL запросов подписки на события на путь обратного вызова этого приложения; Feishu разрешает колбэки на localhost, поэтому для локального тестирования публичный адрес не нужен.",
        ["IMHelp_wecom"] = "Создайте самостоятельно разработанное приложение в консоли администратора WeCom (Apps → Create App). Укажите Corp ID (My Company → Company Info), Agent ID и App Secret приложения, а также Token и Encoding AES Key, сгенерированные при настройке 'сервера приёма сообщений'. URL обратного вызова должен быть публично доступным HTTPS-адресом, указывающим на путь обратного вызова этого приложения; возможно, потребуется добавить IP сервера в список доверенных IP.",
        ["IMHelp_dingtalk"] = "Создайте внутреннее корпоративное приложение с роботом на DingTalk Open Platform. Укажите App Key, App Secret и Robot Code со страницы учётных данных приложения. Режим Stream (по умолчанию) получает сообщения через WebSocket-соединение и не требует публичного адреса; режим HTTP-колбэка требует публично доступного HTTPS URL обратного вызова.",
        ["SkillEnabled"] = "Включить навыки",
        ["MaxContextMessages"] = "Максимум сообщений контекста",
        ["GlobalMaxToolRound"] = "Макс. число раундов вызова инструментов (глобально)",
        ["GlobalSkillTimeoutSeconds"] = "Глобальный тайм-аут навыков (с)",
        ["MaxCustomSkillsPerBeing"] = "Макс. число пользовательских навыков на Silicon Being"
    };

    private static readonly Dictionary<string, string> ConfigDescriptions = new()
    {
        ["DataDirectory"] = "Путь к каталогу данных для хранения всех данных приложения",
        ["Language"] = "Языковая настройка приложения",
        ["TickTimeout"] = "Длительность тайм-аута выполнения каждого тика",
        ["MaxTimeoutCount"] = "Максимальное число последовательных тайм-аутов до срабатывания предохранителя",
        ["WatchdogTimeout"] = "Тайм-аут сторожевого таймера для обнаружения зависшего главного цикла",
        ["MinLogLevel"] = "Глобальный минимальный уровень логирования",
        ["AIClientType"] = "Используемый тип AI-клиента",
        ["OllamaEndpoint"] = "URL конечной точки API Ollama",
        ["DefaultModel"] = "AI-модель, используемая по умолчанию",
        ["OllamaContextWindowTokens"] = "Ёмкость контекстного окна в токенах для моделей Ollama; оставьте пустым для автоопределения",
        ["DashScopeApiKey"] = "Ключ API Alibaba Cloud DashScope",
        ["DashScopeRegion"] = "Регион сервиса Alibaba Cloud DashScope",
        ["DashScopeModel"] = "Используемая модель Alibaba Cloud DashScope",
        ["VolcengineArkApiKey"] = "Ключ API Volcengine Ark",
        ["VolcengineArkEndpointId"] = "ID конечной точки инференса Volcengine Ark",
        ["HerdsmanEndpoint"] = "URL конечной точки API движка инференса Herdsman",
        ["HerdsmanModel"] = "Используемая модель движка инференса Herdsman",
        ["HerdsmanContextWindowTokens"] = "Ёмкость контекстного окна в токенах для моделей Herdsman; оставьте пустым для лимита сообщений по умолчанию",
        ["LongCatApiKey"] = "Ключ API большой модели Meituan LongCat",
        ["LongCatEndpoint"] = "URL конечной точки API большой модели Meituan LongCat",
        ["LongCatModel"] = "Используемая большая модель Meituan LongCat",
        ["LongCatContextWindowTokens"] = "Ёмкость контекстного окна в токенах для моделей LongCat; оставьте пустым для лимита сообщений по умолчанию",
        ["QiniuAIApiKey"] = "Ключ API сервиса инференса больших моделей Qiniu Cloud AI",
        ["QiniuAIEndpoint"] = "URL конечной точки API сервиса инференса больших моделей Qiniu Cloud AI",
        ["QiniuAIModel"] = "Используемая модель сервиса инференса больших моделей Qiniu Cloud AI",
        ["QiniuAIContextWindowTokens"] = "Ёмкость контекстного окна в токенах для моделей Qiniu AI; оставьте пустым для лимита сообщений по умолчанию",
        ["DeepSeekApiKey"] = "Ключ API DeepSeek",
        ["DeepSeekModel"] = "Используемая модель DeepSeek",
        ["DeepSeekEndpoint"] = "URL конечной точки API DeepSeek",
        ["DeepSeekContextWindowTokens"] = "Ёмкость контекстного окна в токенах для моделей DeepSeek; оставьте пустым для лимита сообщений по умолчанию",
        ["ZhipuApiKey"] = "Ключ API Zhipu GLM",
        ["ZhipuModel"] = "Используемая модель Zhipu GLM",
        ["ZhipuEndpoint"] = "URL конечной точки API Zhipu GLM",
        ["ZhipuContextWindowTokens"] = "Ёмкость контекстного окна в токенах для моделей Zhipu; оставьте пустым для лимита сообщений по умолчанию",
        ["MoonshotApiKey"] = "Ключ API Moonshot Kimi",
        ["MoonshotModel"] = "Используемая модель Moonshot Kimi",
        ["MoonshotEndpoint"] = "URL конечной точки API Moonshot Kimi",
        ["MoonshotContextWindowTokens"] = "Ёмкость контекстного окна в токенах для моделей Moonshot; оставьте пустым для лимита сообщений по умолчанию",
        ["SiliconFlowApiKey"] = "Ключ API SiliconFlow",
        ["SiliconFlowModel"] = "Используемая модель SiliconFlow",
        ["SiliconFlowEndpoint"] = "URL конечной точки API SiliconFlow",
        ["SiliconFlowContextWindowTokens"] = "Ёмкость контекстного окна в токенах для моделей SiliconFlow; оставьте пустым для лимита сообщений по умолчанию",
        ["MiniMaxApiKey"] = "Ключ API MiniMax",
        ["MiniMaxModel"] = "Используемая модель MiniMax",
        ["MiniMaxEndpoint"] = "URL конечной точки API MiniMax",
        ["MiniMaxContextWindowTokens"] = "Ёмкость контекстного окна в токенах для моделей MiniMax; оставьте пустым для лимита сообщений по умолчанию",
        ["ErnieApiKey"] = "Ключ API Baidu Ernie",
        ["ErnieModel"] = "Используемая модель Baidu Ernie",
        ["ErnieEndpoint"] = "URL конечной точки API Baidu Ernie",
        ["ErnieContextWindowTokens"] = "Ёмкость контекстного окна в токенах для моделей Ernie; оставьте пустым для лимита сообщений по умолчанию",
        ["HunyuanApiKey"] = "Ключ API Tencent Hunyuan",
        ["HunyuanModel"] = "Используемая модель Tencent Hunyuan",
        ["HunyuanEndpoint"] = "URL конечной точки API Tencent Hunyuan",
        ["HunyuanContextWindowTokens"] = "Ёмкость контекстного окна в токенах для моделей Hunyuan; оставьте пустым для лимита сообщений по умолчанию",
        ["WebPort"] = "Порт веб-сервера",
        ["WebSkin"] = "Имя темы веб-интерфейса",
        ["UserNickname"] = "Никнейм пользователя-человека",
        ["PluginDirectories"] = "Список каталогов плагинов для автообнаружения; поддерживаются относительные и абсолютные пути",
        ["IMPlatforms"] = "Настройка подключённых IM-платформ; можно добавить несколько, через ручной ввод учётных данных или авторизацию по QR-коду",
        ["SkillEnabled"] = "Включить систему навыков (регистрация навыков, диспетчеризация ИИ и автоматические триггеры)",
        ["MaxContextMessages"] = "Максимальное число последних сообщений чата, загружаемых в контекст за один запрос ИИ (используется как запасное окно, когда модель не сообщает размер окна контекста)",
        ["GlobalMaxToolRound"] = "Верхний предел раундов вызова инструментов на одно выполнение навыка",
        ["GlobalSkillTimeoutSeconds"] = "Верхний предел длительности выполнения навыка в секундах",
        ["MaxCustomSkillsPerBeing"] = "Максимальное число пользовательских (невстроенных) навыков, которые может иметь каждый Silicon Being"
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

    public override string GetLogLevelName(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "Трассировка",
        LogLevel.Debug => "Отладка",
        LogLevel.Information => "Информация",
        LogLevel.Warning => "Предупреждение",
        LogLevel.Error => "Ошибка",
        LogLevel.Critical => "Критическая",
        LogLevel.None => "Нет",
        _ => logLevel.ToString()
    };

    // ===== Tool Display Name Localization =====

    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["skill"] = "Навык",
        ["tts"] = "Пакетный синтез речи"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    public override string DefaultCuratorSoul => @"Ты — Куратор Silicon Life Collective.
Ты являешься первым Silicon Being в этом коллективе.
Твоя роль — помогать пользователю управлять другими Silicon Beings и координировать их работу.
Ты должен быть полезным, дружелюбным и профессиональным.
Отвечай на вопросы пользователя и помогай решать задачи.";

    public override string CalendarIntervalName => "Интервал";
    public override string CalendarIntervalDays => "дней";
    public override string CalendarIntervalHours => "часов";
    public override string CalendarIntervalMinutes => "минут";
    public override string CalendarIntervalSeconds => "секунд";
    public override string CalendarIntervalEvery => "каждые";
    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new System.Collections.Generic.List<string>();
        if (days > 0) parts.Add($"{days} {CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours} {CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes} {CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds} {CalendarIntervalSeconds}");
        return parts.Count > 0 ? $"{CalendarIntervalEvery} {string.Join(" ", parts)}" : "0";
    }

    public override string CalendarGregorianName => "Григорианский календарь";
    public override string CalendarComponentYear => "Год";
    public override string CalendarComponentMonth => "Месяц";
    public override string CalendarComponentDay => "День";
    public override string CalendarComponentHour => "Час";
    public override string CalendarComponentMinute => "Минута";
    public override string CalendarComponentSecond => "Секунда";
    public override string CalendarComponentWeekday => "День недели";

    public override string? GetGregorianMonthName(int month) => month switch
    {
        1 => "Январь", 2 => "Февраль", 3 => "Март", 4 => "Апрель",
        5 => "Май", 6 => "Июнь", 7 => "Июль", 8 => "Август",
        9 => "Сентябрь", 10 => "Октябрь", 11 => "Ноябрь", 12 => "Декабрь",
        _ => null
    };

    public override string FormatGregorianYear(int year) => year.ToString();
    public override string FormatGregorianDay(int day) => day.ToString();
    public override string FormatGregorianHour(int hour) => hour.ToString("D2");
    public override string FormatGregorianMinute(int minute) => minute.ToString("D2");
    public override string FormatGregorianSecond(int second) => second.ToString("D2");

    public override string? GetGregorianWeekdayName(int dayOfWeek) => dayOfWeek switch
    {
        0 => "Воскресенье", 1 => "Понедельник", 2 => "Вторник", 3 => "Среда",
        4 => "Четверг", 5 => "Пятница", 6 => "Суббота",
        _ => null
    };

    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarBuddhistName => "Буддийский календарь";
    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => year.ToString();
    public override string FormatBuddhistDay(int day) => day.ToString();
    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarCherokeeName => "Календарь чероки";
    public override string? GetCherokeeMonthName(int month) => null;
    public override string FormatCherokeeYear(int year) => year.ToString();
    public override string FormatCherokeeDay(int day) => day.ToString();
    public override string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarJucheName => "Календарь Чучхе";
    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => year.ToString();
    public override string FormatJucheDay(int day) => day.ToString();
    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarRocName => "Календарь Республики Китай";
    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => year.ToString();
    public override string FormatRocDay(int day) => day.ToString();
    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarChineseHistoricalName => "Китайский исторический календарь";
    public override string CalendarComponentDynasty => "Династия";
    public override string? GetChineseHistoricalMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChineseHistoricalDay(int day) => day.ToString();
    public override ChineseHistoricalLocalizationBase GetChineseHistoricalLocalization() => new ChineseHistoricalRuRU();

    public override string CalendarChulaSakaratName => "Календарь Чула Сакарат";
    public override string? GetChulaSakaratMonthName(int month) => null;
    public override string FormatChulaSakaratYear(int year) => year.ToString();
    public override string FormatChulaSakaratDay(int day) => day.ToString();
    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarJulianName => "Юлианский календарь";
    public override string FormatJulianYear(int year) => year.ToString();
    public override string FormatJulianDay(int day) => day.ToString();
    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarKhmerName => "Кхмерский календарь";
    public override string FormatKhmerYear(int year) => year.ToString();
    public override string FormatKhmerDay(int day) => day.ToString();
    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarZoroastrianName => "Зороастрийский календарь";
    public override string? GetZoroastrianMonthName(int month) => null;
    public override string FormatZoroastrianYear(int year) => year.ToString();
    public override string FormatZoroastrianDay(int day) => day.ToString();
    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarFrenchRepublicanName => "Французский республиканский календарь";
    public override string? GetFrenchRepublicanMonthName(int month) => null;
    public override string FormatFrenchRepublicanYear(int year) => year.ToString();
    public override string FormatFrenchRepublicanDay(int day) => day.ToString();
    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarCopticName => "Коптский календарь";
    public override string? GetCopticMonthName(int month) => null;
    public override string FormatCopticYear(int year) => year.ToString();
    public override string FormatCopticDay(int day) => day.ToString();
    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarEthiopianName => "Эфиопский календарь";
    public override string? GetEthiopianMonthName(int month) => null;
    public override string FormatEthiopianYear(int year) => year.ToString();
    public override string FormatEthiopianDay(int day) => day.ToString();
    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarIslamicName => "Исламский календарь";
    public override string? GetIslamicMonthName(int month) => null;
    public override string FormatIslamicYear(int year) => year.ToString();
    public override string FormatIslamicDay(int day) => day.ToString();
    public override string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarHebrewName => "Еврейский календарь";
    public override string? GetHebrewMonthName(int month) => null;
    public override string FormatHebrewYear(int year) => year.ToString();
    public override string FormatHebrewDay(int day) => day.ToString();
    public override string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarPersianName => "Персидский календарь";
    public override string? GetPersianMonthName(int month) => null;
    public override string FormatPersianYear(int year) => year.ToString();
    public override string FormatPersianDay(int day) => day.ToString();
    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarIndianName => "Индийский национальный календарь";
    public override string? GetIndianMonthName(int month) => null;
    public override string FormatIndianYear(int year) => year.ToString();
    public override string FormatIndianDay(int day) => day.ToString();
    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarSakaName => "Календарь Шака";
    public override string FormatSakaYear(int year) => year.ToString();
    public override string FormatSakaDay(int day) => day.ToString();
    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarVikramSamvatName => "Календарь Викрам Самват";
    public override string FormatVikramSamvatYear(int year) => year.ToString();
    public override string FormatVikramSamvatDay(int day) => day.ToString();
    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarMongolianName => "Монгольский календарь";
    public override string FormatMongolianYear(int year) => year.ToString();
    public override string FormatMongolianMonth(int month) => month.ToString();
    public override string FormatMongolianDay(int day) => day.ToString();
    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarJavaneseName => "Яванский календарь";
    public override string? GetJavaneseMonthName(int month) => null;
    public override string FormatJavaneseYear(int year) => year.ToString();
    public override string FormatJavaneseDay(int day) => day.ToString();
    public override string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarTibetanName => "Тибетский календарь";
    public override string FormatTibetanYear(int year) => year.ToString();
    public override string FormatTibetanMonth(int month) => month.ToString();
    public override string FormatTibetanDay(int day) => day.ToString();
    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarMayanName => "Календарь майя";
    public override string CalendarMayanBaktun => "Бактун";
    public override string CalendarMayanKatun => "Катун";
    public override string CalendarMayanTun => "Тун";
    public override string CalendarMayanUinal => "Виналь";
    public override string CalendarMayanKin => "Кин";
    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarInuitName => "Календарь инуитов";
    public override string? GetInuitMonthName(int month) => null;
    public override string FormatInuitYear(int year) => year.ToString();
    public override string FormatInuitDay(int day) => day.ToString();
    public override string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarRomanName => "Римский календарь";
    public override string? GetRomanMonthName(int month) => null;
    public override string FormatRomanYear(int year) => year.ToString();
    public override string FormatRomanDay(int day) => day.ToString();
    public override string LocalizeRomanDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarChineseLunarName => "Китайский лунный календарь";
    public override string? GetChineseLunarMonthName(int month) => null;
    public override string? GetChineseLunarDayName(int day) => null;
    public override string ChineseLunarLeapPrefix => "Високосный";
    public override string CalendarComponentIsLeap => "Високосный";
    public override string FormatChineseLunarYear(int year) => year.ToString();
    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
        => $"{year}-{(isLeap ? ChineseLunarLeapPrefix : "")}{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarVietnameseName => "Вьетнамский лунный календарь";
    public override string? GetVietnameseMonthName(int month) => null;
    public override string? GetVietnameseZodiacName(int index) => null;
    public override string VietnameseLeapPrefix => "Високосный";
    public override string CalendarComponentZodiac => "Зодиак";
    public override string FormatVietnameseYear(int year) => year.ToString();
    public override string FormatVietnameseDay(int day) => day.ToString();
    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second)
        => $"{year}-{(isLeap ? VietnameseLeapPrefix : "")}{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarJapaneseName => "Японский календарь";
    public override string? GetJapaneseEraName(int eraIndex) => null;
    public override string CalendarComponentEra => "Эра";
    public override string FormatJapaneseYear(int year) => year.ToString();
    public override string FormatJapaneseDay(int day) => day.ToString();
    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarYiName => "Календарь народности И";
    public override string CalendarComponentYiSeason => "Сезон";
    public override string CalendarComponentYiXun => "Декада";
    public override string? GetYiMonthName(int month) => null;
    public override string? GetYiSeasonName(int seasonIndex) => null;
    public override string? GetYiXunName(int xunIndex) => null;
    public override string? GetYiDayAnimalName(int animalIndex) => null;
    public override string FormatYiYear(int year) => year.ToString();
    public override string FormatYiDay(int day) => day.ToString();
    public override string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarSexagenaryName => "Китайский шестидесятилетний цикл";
    public override string CalendarComponentYearStem => "Небесный ствол года";
    public override string CalendarComponentYearBranch => "Земная ветвь года";
    public override string CalendarComponentMonthStem => "Небесный ствол месяца";
    public override string CalendarComponentMonthBranch => "Земная ветвь месяца";
    public override string CalendarComponentDayStem => "Небесный ствол дня";
    public override string CalendarComponentDayBranch => "Земная ветвь дня";
    public override string? GetSexagenaryStemName(int index) => null;
    public override string? GetSexagenaryBranchName(int index) => null;
    public override string? GetSexagenaryZodiacName(int index) => null;
    public override string LocalizeSexagenaryDate(int yearStem, int yearBranch, int monthStem, int monthBranch, int dayStem, int dayBranch, int hour, int minute, int second)
        => $"{yearStem}-{yearBranch} {monthStem}-{monthBranch} {dayStem}-{dayBranch} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarDaiName => "Календарь Дай (Сишуанбаньна)";
    public override string? GetDaiMonthName(int month) => null;
    public override string FormatDaiYear(int year) => year.ToString();
    public override string FormatDaiDay(int day) => day.ToString();
    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string CalendarDehongDaiName => "Календарь Дай (Дэхун)";
    public override string? GetDehongDaiMonthName(int month) => null;
    public override string FormatDehongDaiYear(int year) => year.ToString();
    public override string FormatDehongDaiDay(int day) => day.ToString();
    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
        => $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

    public override string FormatMemoryEventSingleChat(string speakerName, string listenerName, string content)
        => $"Беседа: {speakerName} -> {listenerName}: {content}";
    public override string FormatMemoryEventGroupChat(string sessionId, string content)
        => $"Групповой чат ({sessionId}): {content}";
    public override string FormatMemoryEventToolCall(string toolNames)
        => $"Вызов инструментов: {toolNames}";
    public override string FormatMemoryEventTask(string content)
        => $"Задача: {content}";
    public override string FormatMemoryEventTimer(string content)
        => $"Таймер: {content}";
    public override string FormatMemoryEventTimerError(string timerName, string error)
        => $"Ошибка таймера ({timerName}): {error}";

    public override string FormatTimerStartNotification(string timerName)
        => $"Таймер \"{timerName}\" начал выполнение";
    public override string FormatTimerEndNotification(string timerName, string result)
        => $"Таймер \"{timerName}\" завершил выполнение: {result}";
    public override string FormatTimerErrorNotification(string timerName, string error)
        => $"Таймер \"{timerName}\" завершился с ошибкой: {error}";

    public override string FormatMemoryEventBeingCreated(string name, string id)
        => $"Создан новый Silicon Being: {name} ({id})";
    public override string FormatMemoryEventBeingReset(string id)
        => $"Silicon Being сброшен к реализации по умолчанию: {id}";
    public override string FormatMemoryEventTaskCompleted(string taskTitle)
        => $"Задача завершена: {taskTitle}";
    public override string FormatMemoryEventTaskFailed(string taskTitle)
        => $"Задача завершилась с ошибкой: {taskTitle}";
    public override string FormatMemoryEventStartup()
        => "Агент запущен (загружен менеджером)";
    public override string FormatMemoryEventRuntimeError(string message)
        => $"Ошибка времени выполнения: {message}";

    public override string MemoryToolNotAvailable => "Система памяти недоступна";
    public override string MemoryToolMissingAction => "Не указано действие памяти";
    public override string MemoryToolMissingContent => "Не указано содержимое для сохранения";
    public override string MemoryToolNoMemories => "Нет записей в памяти";
    public override string MemoryToolRecentHeader(int count) => $"Последние {count} записей:";
    public override string MemoryToolStatsHeader => "Статистика памяти:";
    public override string MemoryToolStatsTotal => "Всего записей";
    public override string MemoryToolStatsOldest => "Самая старая";
    public override string MemoryToolStatsNewest => "Самая новая";
    public override string MemoryToolStatsNA => "Н/Д";
    public override string MemoryToolQueryNoResults => "По запросу ничего не найдено";
    public override string MemoryToolQueryHeader(int count, string rangeDesc) => $"Найдено {count} записей ({rangeDesc}):";
    public override string MemoryToolInvalidYear => "Некорректный год";
    public override string MemoryToolUnknownAction(string action) => $"Неизвестное действие памяти: {action}";

    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType switch
    {
        "variable" => "Переменная",
        "function" => "Функция",
        "class" => "Класс",
        "keyword" => "Ключевое слово",
        "identifier" => "Идентификатор",
        _ => wordType
    };

    public override string GetCodeHoverWordTypeDesc(string wordType, string word) => wordType switch
    {
        "variable" => $"Переменная: {word}",
        "function" => $"Функция: {word}",
        "class" => $"Класс: {word}",
        "keyword" => $"Ключевое слово: {word}",
        "identifier" => $"Идентификатор: {word}",
        _ => $"{wordType}: {word}"
    };

    public override string GetCodeHoverKeywordDesc(string language, string keyword) => "";
    public override string GetTranslation(string key) => "";

    public override string Help_Title => "Справочная документация";
    public override string Help_Search => "Поиск в справке";
    public override string Help_DocList => "Список документов";
    public override string Help_NoResults => "Результаты не найдены";
    public override string Help_Previous => "Предыдущий";
    public override string Help_Next => "Следующий";

    // Project Info Context
    public override string ProjectCtx_ProjectInfoHeader => "Принадлежность к проекту";
    public override string ProjectCtx_ProjectInfoRoleLabel => "Роль";
    public override string ProjectCtx_ProjectInfoGoalLabel => "Цель";

    public override string ProjectCtx_RoleDefinitionsHeader => "Определения ролей";
    public override string ProjectCtx_RoleAssignmentsHeader => "Назначения ролей";
    public override string ProjectCtx_NoWorkflowTemplate => "Шаблон рабочего процесса не назначен";
    public override string ProjectCtx_RoleNeedsAttention => "Роли требуют внимания: {0} недоукомплектовано";
    public override string ProjectCtx_StaffingActionPlanHeader => "План действий по комплектованию";
    public override string ProjectCtx_TotalBeingsNeeded => "Требуется агентов: {0}";
    public override string ProjectCtx_StaffingRoleBreakdownHeader => "Детализация нехватки по ролям";
    public override string ProjectCtx_RoleShortageDetail => "{0}: минимум {1}, назначено {2}, нехватка {3}";
    public override string ProjectCtx_StaffingActionStepsHeader => "Предлагаемые шаги";
    public override string ProjectCtx_StaffingStepCreateBeings => "1. Создайте {0} Silicon Beings";
    public override string ProjectCtx_StaffingStepAssignToProject => "2. Назначьте Silicon Beings на проект";
    public override string ProjectCtx_StaffingStepAssignToRoles => "3. Назначьте Silicon Beings на роли";
    public override string ProjectCtx_EmptyRolePoolAction => "Пул ролей пуст. Требуется {0} ролей. Создайте Silicon Beings и назначьте их.";
    public override string ProjectCtx_RoleMinCount => "Минимум";
    public override string ProjectCtx_RoleMaxCount => "Максимум";
    public override string ProjectCtx_RoleMaxCountUnlimited => "Без ограничений";
    public override string ProjectCtx_RoleAssignedCount => "Назначено";
    public override string ProjectCtx_UnassignedRoles => "Неназначенные обязательные роли";
    public override string ProjectCtx_AvailableBeingsHeader => "Доступные существа (ещё не назначенные на этот проект)";
    public override string ProjectCtx_AvailableBeingsHint => "Совет: Рассмотрите назначение существующих существ на роли перед созданием новых. Используйте project assign и project assign_role.";
    public override string ProjectCtx_AttentionReasonsHeader => "Причины внимания";
    public override string ProjectCtx_UnsatisfiedRolesDetailHeader => "Детализация неудовлетворённых ролей";
    public override string ProjectAttention_MissingTemplate => "Проект не имеет назначенного шаблона рабочего процесса";
    public override string ProjectAttention_EmptyRolePool => "Шаблон есть, но пул ролей полностью пуст";
    public override string ProjectAttention_UnsatisfiedRoles => "Некоторые роли в шаблоне проекта не удовлетворены";

    public override string RoleStaffing_Understaffed => "Недоукомплектовано";
    public override string RoleStaffing_Overstaffed => "Переукомплектовано";
    public override string RoleStaffing_Full => "Заполнено";
    public override string RoleStaffing_Sufficient => "Достаточно";
    public override string RoleStaffing_UnderstaffedDetail => "Минимум {0}, назначено {1}";
    public override string RoleStaffing_OverstaffedDetail => "Максимум {0}, назначено {1}";
    public override string RoleStaffing_FullDetail => "Назначено {0} из {1}";
    public override string RoleStaffing_SufficientDetail => "Назначено {0}, минимум {1}";

    // ===== Workflow Role Notification =====
    public override string WorkflowRoleBlockedNotificationFormat => "[Уведомление о роли рабочего процесса] Рабочий процесс проекта '{0}' заблокирован на переходе '{1}' ({2} → {3}).\n\nНедостающие роли: {4}\n\nИспользуйте действие assign_role инструмента project_tool для назначения необходимых ролей. Рабочий процесс автоматически возобновится при следующей проверке.";

    public override string MemoryCompressionSystemPrompt => "Ты — ассистент сжатия памяти. Пожалуйста, сожми следующий период памяти в краткое резюме, сохранив ключевую информацию. ВАЖНО: Выведи только сжатое содержимое, НЕ ДОБАВЛЯЙ никаких временных меток, штампов времени или аннотаций дат. Предоставь только чистый сжатый текст.";

    public override string CommonSystemPrompt => @"## Поведенческие инструкции
Ты — проактивный ИИ-ассистент. При прямых командах действуй немедленно, не дожидаясь дополнительного разрешения.
Приветствия, ответы, предоставление информации, запросы и т.д. — это твоя обязанность, действуй проактивно.
Помни: Ты — ассистент и должен активно отвечать на потребности пользователя, а не пассивно ждать инструкций.

## Завершение разговора
Когда ты завершил задачу и нет необходимости продолжать разговор, используй действие mark_read инструмента chat, чтобы отметить сообщения собеседника как прочитанные без отправки ответа.
Это указывает, что ты прочитал сообщение, но сознательно выбрал не отвечать (прочитано, без ответа), и можешь естественно завершить разговор.
Использование: Вызови инструмент chat, установи action=""mark_read"", target_id=GUID собеседника, параметр message не требуется.";

    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"Сжатие памяти: {levelDesc}. Период: {rangeDesc}.\n\nСодержимое памяти:\n{contentText}";
    }

    public override string SingleChatNameFormat => "Чат с {0}";

    public override string GetBeingActivityName(BeingActivity activity) => activity switch
    {
        BeingActivity.Idle => "Простой",
        BeingActivity.SingleChat => "В индивидуальном чате",
        BeingActivity.GroupChat => "В групповом чате",
        BeingActivity.Task => "Выполнение задачи",
        BeingActivity.Timer => "Выполнение таймера",
        BeingActivity.Broadcast => "Обработка вещания",
        BeingActivity.Project => "Работа над проектом",
        BeingActivity.MemoryCompression => "Сжатие памяти",
        BeingActivity.Stopped => "Остановлен",
        _ => activity.ToString()
    };

    public override string FormatMemoryEventProject(string content)
        => $"[Проект] Размышление над проектом, результат: {content}";

    // ===== Project Think Session Localization =====
    public override string ProjectThinkHistoryLinkLabel => "История обработки";
    public override string ProjectThinkHistoryTitle => "История обработки проекта";
    public override string ProjectThinkHistoryHeader => "История обработки";
    public override string ProjectThinkBackToProjects => "← Вернуться к проектам";
    public override string ProjectThinkProjectName => "Проект: {0}";
    public override string ProjectThinkNoRecords => "Нет сеансов обработки";
    public override string ProjectThinkStateStarted => "Запущен";
    public override string ProjectThinkStateExecuting => "Выполняется";
    public override string ProjectThinkStateCompleted => "Завершён";
    public override string ProjectThinkStateFailed => "Ошибка";
    public override string ProjectThinkDetailTitle => "Детали сеанса";
    public override string ProjectThinkDetailHeader => "Детали сеанса";
    public override string ProjectThinkRoundLabel => "Раунд: ";
    public override string ProjectThinkMessageCountLabel => "Сообщений: ";
    public override string ProjectThinkStateLabel => "Состояние: ";
    public override string ProjectThinkCreatedAt => "Создано: ";
    public override string ProjectThinkCompletedAt => "Завершено: ";
    public override string ProjectThinkCycleLabel => "Цикл";
    public override string ProjectThinkRoundN => "Раунд {0}";
}
