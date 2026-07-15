# Руководство разработчика

> **Версия: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | **Русский**

## Обзор архитектуры

SiliconLifeCollective следует **архитектуре Тело-Мозг**, со строгим разделением основных интерфейсов и реализаций по умолчанию.

### Структура проекта

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Интерфейсы, абстрактные классы, общая инфраструктура
│   ├── SiliconLife.Common/          # Общие реализации (используются обеими версиями)
│   ├── SiliconLife.Default/         # Реализация по умолчанию, точка входа (проверка архитектуры)
│   ├── SiliconLife.Fast/            # Высокопроизводительная реализация, точка входа (основная производственная версия)
│   ├── SiliconLife.Speedy/          # Высокопроизводительный Движок Хранения SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Менеджер SpeedyPack (Avalonia UI)
└── docs/                            # Многоязычная документация
```

**Направление зависимостей**:
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core` (однонаправленная)

**Описание ролей версий**:
- **SiliconLife.Default**: реализация по умолчанию, в основном для проверки жизнеспособности архитектуры. Предоставляет простую и надёжную реализацию хранения в файловой системе, подходит для отладки разработки и проверки архитектуры.
- **SiliconLife.Fast**: основная производственная версия. На основе архитектуры, проверенной Default, использует хранение в памяти SpeedyPack + асинхронную персистентность для экстремальной оптимизации производительности, является предпочтительным выбором для длительной работы и реальных производственных сред.

## Основные концепции

### 1. Кремниевое Существо

Каждый AI-агент состоит из:
- **Тело** (`DefaultSiliconBeing`): поддерживает жизненные показатели, обнаруживает триггерные сценарии
- **Мозг** (`ContextManager`): загружает историю, вызывает AI, выполняет инструменты, сохраняет ответы

### 2. Система инструментов

Инструменты автоматически обнаруживаются и регистрируются через рефлексию:

```csharp
// Все инструменты реализуют интерфейс ITool
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Система разрешений

3-уровневая цепочка проверки разрешений:
```
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → отказ по умолчанию)
```

### 4. Локатор служб

Глобальная регистрация и получение служб:
```csharp
// Регистрация
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Получение
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Система расширений

### Добавление нового инструмента

1. Создайте новый класс в `src/SiliconLife.Common/Tools/` (инструменты, общие для обеих версий):

> **Примечание**: `SiliconLife.Default` и `SiliconLife.Fast` больше не имеют собственных каталогов `Tools/`, все общие инструменты размещены в `SiliconLife.Common/Tools/`.

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Разбор параметров
        var param1 = call.Parameters["param1"]?.ToString();
        
        // Логика выполнения
        var result = await DoSomething(param1);
        
        // Возврат результата
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. Инструмент автоматически обнаруживается через рефлексию — ручная регистрация не требуется!

3. (Опционально) Пометка как доступный только администратору:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

4. (Опционально) Пометка доступных сценариев инструмента:
```csharp
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class MyTool : ITool { ... }
```

5. (Опционально) Пометка как доступный только в сценарии чата:
```csharp
[ChatOnly]
public class HelpTool : ITool { ... }
```

6. (Опционально) Пометка как доступный только в сценарии проекта:
```csharp
[ToolScenario(ToolScenarioFlag.Project)]
[SiliconManagerOnly]
public class ProjectWorkTool : ITool { ... }
```

### Добавление нового AI-клиента

1. Реализуйте `IAIClient` в `src/SiliconLife.Common/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Вызов вашего AI API
        var response = await CallMyAPI(request);
        
        return new AIResponse
        {
            Content = response.Message,
            ToolCalls = response.ToolCalls,
            Usage = response.Usage
        };
    }
    
    public async IAsyncEnumerable<string> StreamChatAsync(AIRequest request)
    {
        // Реализация потоковой передачи
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. Создайте фабрику:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. Фабрика автоматически обнаруживается и регистрируется.

### Добавление нового бэкенда хранения

1. Реализуйте `IStorage` и `ITimeStorage` в `src/SiliconLife.Default/Storage/` (реализация файловой системы) или `src/SiliconLife.Fast/Storage/` (адаптер SpeedyPack):

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // Чтение из вашей базы данных
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // Запись в вашу базу данных
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // Запрос с временным индексом
    }
}
```

### Добавление нового плагина

1. Создайте проект библиотеки классов, реализующий интерфейс `IPlugin`:

```csharp
using SiliconLife.Collective;
using SiliconLife.Collective.Localization;
using SiliconLife.Collective.Tools;

public class MyPlugin : IPlugin
{
    public string Id => "my-plugin";
    public string Version => "1.0.0";
    
    public string GetName(Language language) => "My Plugin";
    public string GetDescription(Language language) => "A custom plugin";
    public string GetAuthor(Language language) => "Author Name";
    
    public void OnLoad() { }
    public void OnStart() { }
    public void OnStop() { }
    public void OnUnload() { }
}
```

2. (Опционально) Реализуйте интерфейс `ITool` в плагине для регистрации пользовательского инструмента:

```csharp
public class MyPluginTool : ITool
{
    public string Name => "my_plugin_tool";
    public string Description => "A tool provided by my plugin";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        return new ToolResult { Success = true, Output = "Done" };
    }
}
```

3. Поместите скомпилированную DLL в каталог плагинов, `PluginLoader` загрузит её автоматически.

> **Ограничения безопасности**: плагины по умолчанию не могут ссылаться на пространства имён `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis` и др. Однако плагины могут объявить необходимые возможности через атрибут `[PluginCapability]` (Network, FileIO, Process, AI), и загрузчик на этом основании ослабляет соответствующие правила безопасности пространств имён. Необъявляемые возможности (P/Invoke, Unsafe, Reflection Emit и т.д.) всегда блокируются. Плагины загружаются изолированно через `AssemblyLoadContext`.

### Добавление новой темы

1. Реализуйте `ISkin` в `src/SiliconLife.App/Web/Skins/`:

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "A custom skin description";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
            }
            /* Your custom styles */
        ";
    }
}
```

2. Тема автоматически обнаруживается `SkinManager`.

## Руководство по стилю кода

### Соглашения об именовании

- **Классы**: PascalCase с функциональным префиксом (например, `DefaultSiliconBeing`)
- **Интерфейсы**: начинаются с `I` (например, `IAIClient`, `ITool`)
- **Реализации**: оканчиваются именем интерфейса (например, `OllamaClient` реализует `IAIClient`)
- **Инструменты**: оканчиваются на `Tool` (например, `CalendarTool`, `ChatTool`)
- **Модели представления**: оканчиваются на `ViewModel` (например, `BeingViewModel`)

### Организация кода

```
SiliconLife.Common/
├── AI/                    # Реализации AI-клиентов и фабрик (Ollama, DashScope, VolcengineArk, Herdsman, LongCat, QiniuAI, DeepSeek, Zhipu, Ernie, Hunyuan, MiniMax, Moonshot, SiliconFlow)
├── Calendar/              # 32 календарные реализации
├── Localization/          # Базовый класс локализации и 34 языковых варианта
├── Security/              # Менеджер Разрешений
├── SiliconBeing/          # Реализация Кремниевого Существа по умолчанию
├── Tools/                 # Общие встроенные инструменты (25)
├── Web/                   # Веб-инфраструктура
└── WebView/               # Реализация Playwright WebView

SiliconLife.App/          # Уровень приложения, общий для Default и Fast
├── Config/                # Конфигурация приложения
├── Help/                  # Локализация справочной документации
├── Project/               # Система проектов (движок рабочих процессов, роли проекта)
└── Web/                   # Реализация Web UI
    ├── Component/         # 27 UI-компонентов
    ├── Controllers/       # 24 маршрутизирующих контроллера
    ├── Models/            # Модели представлений
    ├── Views/             # HTML-представления
    └── Skins/             # 7 тем оформления

SiliconLife.Default/      # Версионно-специфичные каталоги
├── Config/                # Данные конфигурации по умолчанию
├── Knowledge/             # Реализация Сети Знаний
├── Logging/               # Реализация Провайдера Журналирования (консоль + файловая система)
├── Project/               # Реализация Системы Проектов
└── Storage/               # Реализация хранения в файловой системе

SiliconLife.Fast/         # Версионно-специфичные каталоги
├── Config/                # Данные конфигурации версии Fast
├── Logging/               # Реализация Провайдера Журналирования (консоль + файловая система)
├── Storage/               # Адаптеры хранения SpeedyPack
└── Tray/                  # Локализация системного трея
```

### Документация

- Все публичные API должны иметь XML-комментарии документации
- Все исходные файлы используют заголовок лицензии Apache 2.0
- Использование возможностей .NET 9 (неявные using, ссылочные типы, допускающие null)

## Рабочий процесс разработки

### 1. Настройка среды разработки

```bash
# Клонирование репозитория
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Восстановление зависимостей
dotnet restore

# Сборка
dotnet build
```

### 2. Запуск тестов

```bash
# Запуск всех тестов
dotnet test

# Запуск определённого тестового проекта
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Отладка

```bash
# Запуск с отладочным выводом
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Форматирование кода

```bash
# Форматирование кода
dotnet format
```

## Создание пользовательских функций

### Пример: добавление пользовательского календаря

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Ваша логика преобразования
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Обратное преобразование
        return new GregorianDate(year, month, day);
    }
}
```

### Пример: добавление пользовательского Исполнителя

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

### Пример: добавление пользовательского шаблона рабочего процесса

```csharp
public class MyWorkflowTemplate : WorkflowTemplate
{
    public override string Name => "my_workflow";
    public override string Description => "A custom workflow template";
    
    public override void DefineStates()
    {
        AddState("start", "Начало", isInitial: true);
        AddState("processing", "Обработка");
        AddState("review", "Проверка");
        AddState("done", "Завершено", isFinal: true);
    }
    
    public override void DefineTransitions()
    {
        AddTransition("start", "processing", "Начать обработку");
        AddTransition("processing", "review", "Отправить на проверку");
        AddTransition("review", "done", "Проверка пройдена");
        AddTransition("review", "processing", "Вернуть на доработку");
    }
}
```

### Пример: добавление проектной роли

Проектные роли управляются через операции `assign_role` и `remove_role` инструмента `ProjectTool`. Имена ролей — пользовательские строки, используемые для различения обязанностей Кремниевых Существ в рабочих процессах и назначении задач.

## Руководство по тестированию

### Модульные тесты

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Подготовка
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object> 
            { 
                ["param1"] = "test" 
            }
        };
        
        // Выполнение
        var result = await tool.ExecuteAsync(call);
        
        // Проверка
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### Интеграционные тесты

Тестирование полного процесса:
1. AI возвращает вызов инструмента
2. Инструмент выполняется
3. Результат передаётся AI
4. AI возвращает окончательный ответ

## Вопросы производительности

### Система хранения

- Версия Default использует хранение JSON на основе файлов
- Версия Fast использует движок хранения SpeedyPack в памяти (формат .spk)
- SpeedyPack использует отображение каталогов в памяти + кэш записей + асинхронную очередь записи
- Запросы с временным индексом используют интерфейс `ITimeStorage`

### Планировщик Основного Цикла

- Справедливое планирование с временными интервалами на основе часов
- Сторожевой Таймер для обнаружения зависших операций
- Автоматический Выключатель для предотвращения каскадных сбоев

## Лучшие практики

### 1. Всегда проверяйте разрешения

Любая операция, инициированная AI, должна проходить через цепочку разрешений:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return Result.Denied("Permission denied");
}
```

### 2. Используйте Локатор Служб

Глобальная регистрация и получение служб:

```csharp
// Во время инициализации
ServiceLocator.Instance.Register<ICustomService>(myService);

// При необходимости
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Следуйте разделению Тело-Мозг

- Тело обрабатывает состояние и триггеры
- Мозг обрабатывает AI-взаимодействие и выполнение инструментов

### 4. Реализуйте надлежащую обработку ошибок

```csharp
try
{
    var result = await operation();
    return Result.Success(result);
}
catch (Exception ex)
{
    Logger.Error($"Operation failed: {ex.Message}");
    return Result.Failure(ex.Message);
}
```

## Руководство по участию

1. Форкните репозиторий
2. Создайте ветку функции (`git checkout -b feature/amazing-feature`)
3. Зафиксируйте изменения с использованием конвенциональных коммитов
4. Отправьте в ветку (`git push origin feature/amazing-feature`)
5. Откройте Pull Request

### Формат сообщений коммитов

```
<type>(<scope>): <description>

Примеры:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Следующие шаги

- 📚 Прочитайте [руководство по архитектуре](architecture.md)
- 📖 Ознакомьтесь со [справочником API](api-reference.md)
- 🔒 Ознакомьтесь с [документацией по безопасности](security.md)
- 🚀 Ознакомьтесь с [руководством быстрого старта](getting-started.md)
