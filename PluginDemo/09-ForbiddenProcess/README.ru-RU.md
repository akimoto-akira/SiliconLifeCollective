# PluginDemo-09: Антипаттерн запрещённых операций с процессами

## Обзор

Этот плагин демонстрирует **запрещённые** операции выполнения процессов в системе плагинов SiliconLife. Он служит справочником антипаттернов, показывая, чего НЕ следует делать, и предоставляя правильные альтернативы для каждого нарушения.

## Почему типы Process запрещены?

`System.Diagnostics.Process` и `ProcessStartInfo` заблокированы в плагинах, поскольку прямое выполнение процессов представляет серьёзные риски безопасности:

1. **Произвольное выполнение команд**: Плагины могут выполнять любую команду без аудита или проверки разрешений
2. **Запуск вредоносного ПО**: Вредоносные плагины могут запускать нежелательные приложения или скрипты
3. **Доступ к системным ресурсам**: Процессы могут получить доступ к конфиденциальным ресурсам за пределами песочницы плагина
4. **Отсутствие валидации команд**: Прямой Process.Start не имеет встроенной защиты от инъекции команд
5. **Отсутствие аудиторского следа**: Прямые операции с процессами обходят систему аудита безопасности
6. **Повышение привилегий**: Возможность создания процессов с более высокими привилегиями

## Какие типы запрещены?

Запрещены только типы, связанные с Process, **НЕ весь пространство имён System.Diagnostics**:

| Запрещённый тип | Заблокированный метод | Уровень риска |
|----------------|----------------------|--------------|
| `Process` | `Start()`, `Kill()`, `WaitForExit()` | 🔴 Критический |
| `ProcessStartInfo` | Конструктор, все свойства | 🔴 Критический |
| `Process` | `StandardInput`, `StandardOutput`, `StandardError` | 🔴 Критический |
| `Process` | `GetProcesses()`, `GetProcessesByName()` | 🟡 Высокий |

## Какие типы разрешены?

Другие типы `System.Diagnostics`, не связанные с выполнением процессов, остаются доступными:

| Разрешённый тип | Использование | Почему безопасно |
|----------------|--------------|-----------------|
| `Stopwatch` | Измерение времени | Нет выполнения процессов |
| `Debug` | Отладочный вывод | Нет риска безопасности |
| `Trace` | Трассировка/логирование | Нет риска безопасности |
| `PerformanceCounter` | Мониторинг производительности | Только чтение, аудируется |

## Как безопасно выполнять команды?

### Использование CommandLineExecutor (единственный безопасный способ)

`CommandLineExecutor` — это **контролируемая точка входа** для выполнения команд в плагинах:

```csharp
// ✅ ПРАВИЛЬНО: Выполнение команды
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"Ошибка: {result.Error}");
}
```

**Что предоставляет CommandLineExecutor:**
1. **Защита от инъекции команд**: Блокирует опасные разделители (`||`, `&&`, `|`, `&`, `;`)
2. **Принудительный таймаут**: Таймаут по умолчанию 30 секунд (настраиваемый)
3. **Журнал аудита**: Все выполнения команд записываются для проверки безопасности
4. **Захват вывода**: Автоматический захват stdout и stderr
5. **Кроссплатформенность**: Использует `cmd.exe` в Windows, `/bin/bash` в Unix
6. **Обработка ошибок**: Возвращает структурированный результат со статусом успеха/неудачи

## Демонстрируемые нарушения

Этот плагин показывает 5 распространённых нарушений выполнения процессов:

### Нарушение 1: Process.Start

```csharp
// ❌ ЗАПРЕЩЕНО
Process.Start("notepad.exe");

// ✅ ПРАВИЛЬНО
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**Заблокированный TypeRef**: `System.Diagnostics.Process::Start(System.String)`

### Нарушение 2: ProcessStartInfo

```csharp
// ❌ ЗАПРЕЩЕНО
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ ПРАВИЛЬНО
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**Заблокированный TypeRef**: `System.Diagnostics.ProcessStartInfo::.ctor()`

### Нарушение 3: Process с аргументами

```csharp
// ❌ ЗАПРЕЩЕНО
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ ПРАВИЛЬНО
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**Заблокированный TypeRef**: `System.Diagnostics.Process::Start(ProcessStartInfo)`

### Нарушение 4: Перенаправление вывода процесса

```csharp
// ❌ ЗАПРЕЩЕНО
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ ПРАВИЛЬНО
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**Заблокированный TypeRef**: `System.Diagnostics.Process::StandardOutput`

### Нарушение 5: Process.Kill

```csharp
// ❌ ЗАПРЕЩЕНО
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ ПРАВИЛЬНО
// По соображениям безопасности CommandLineExecutor не поддерживает завершение процессов.
// При необходимости обратитесь к системному администратору.
```

**Заблокированный TypeRef**: `System.Diagnostics.Process::Kill()`

## Почему только Process, а не весь System.Diagnostics?

Система плагинов использует **хирургический подход** к безопасности:

- **Блокировать только опасные типы**: Process/ProcessStartInfo позволяют выполнять произвольный код
- **Разрешать безопасные типы**: Stopwatch, Debug, Trace не имеют проблем с безопасностью
- **Минимизировать влияние**: Разработчики могут использовать безрисковые диагностические инструменты
- **Чёткая граница**: Запрещены только типы, способные создавать/завершать процессы

## Механизм безопасности PluginLoader

Когда PluginLoader сканирует этот плагин:

1. **Сканирование TypeRef**: Обнаруживает ссылки на запрещённые типы `Process`/`ProcessStartInfo`
2. **Сканирование MemberRef**: Обнаруживает вызовы заблокированных методов (напр., `Process.Start`)
3. **Сканирование IL-строк**: Обнаруживает попытки рефлексии через строки
4. **Отклонение**: Плагин отклоняется при загрузке с подробным сообщением об ошибке

## Лучшие практики

1. **Всегда использовать CommandLineExecutor**: Никогда не использовать `Process.Start` напрямую
2. **Устанавливать разумные таймауты**: Предотвращать бесконечное зависание команд
3. **Проверять результаты**: Всегда проверять `result.Success` перед использованием вывода
4. **Санитизировать ввод**: Никогда не передавать пользовательский ввод напрямую в команды
5. **Объявлять Capability при необходимости**: Если действительно нужно неограниченное выполнение процессов, объявить `Capability.Process` (см. 15-CapabilityProcess)

## Файлы

- `Plugin.cs` - Плагин демонстрации антипаттерна
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Этот файл (Русский)
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Связанные примеры

- **08-ForbiddenNetwork**: Запрещённые сетевые операции
- **15-CapabilityProcess**: Декларативное разрешение Process
- **10-ForbiddenReflection**: Запрещённые операции рефлексии
- **12-ForbiddenStringBypass**: Попытки обхода через строковую рефлексию
