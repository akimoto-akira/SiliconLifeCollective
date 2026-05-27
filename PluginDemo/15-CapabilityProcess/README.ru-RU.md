# PluginDemo-15: Capability.Process — Декларативное разрешение процесса

## Обзор

Этот плагин демонстрирует использование `[PluginCapability(Capability.Process)]` для объявления способности плагина запускать дочерние процессы. С этой декларацией плагин получает доступ к `System.Diagnostics.Process` и связанным типам.

## Синтаксис объявления

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Область освобождения Capability.Process

### Освобождение TypeRef

Освобождаются только типы, связанные с Process, в пространстве имён `System.Diagnostics`:

| Освобождённый тип | Использование |
|------------------|--------------|
| `Process` | Запуск, управление и мониторинг дочерних процессов |
| `ProcessStartInfo` | Настройка параметров запуска процесса |
| `ProcessThread` | Доступ к информации о потоках процесса |
| `ProcessModule` | Доступ к информации о модулях процесса |
| `ProcessPriorityClass` | Установка приоритета процесса |
| `ProcessWindowStyle` | Настройка стиля окна процесса |

Типы, всегда разрешённые (никогда не входящие в список запретов): `Stopwatch`, `Debug`, `Trace`, `Activity`

### Освобождение ILString

- Строки, начинающиеся с `"System.Diagnostics.Process"`, не помечаются

## Сравнение с 09-ForbiddenProcess

| Аспект | 09-ForbiddenProcess | 15-CapabilityProcess |
|--------|-------------------|---------------------|
| Декларация | Нет | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ ОТКЛОНЕНО | ✅ РАЗРЕШЕНО |
| ProcessStartInfo | ❌ ОТКЛОНЕНО | ✅ РАЗРЕШЕНО |

## Рекомендация: CommandLineExecutor

Даже с `Capability.Process` рекомендуется предпочитать `CommandLineExecutor`:

| Функция | CommandLineExecutor | Прямой Process |
|---------|-------------------|---------------|
| Декларация возможности | Не требуется | Требуется |
| Песочница | Белый список команд | Нет |
| Таймауты | Встроены | Вручную |
| Захват вывода | Структурированный | Вручную |
| Аудиторское логирование | Автоматическое | Вручную |

Используйте `Capability.Process` + прямой `Process` только тогда, когда нужен детальный контроль над потоками I/O, обработка событий процесса или когда белый список команд CommandLineExecutor слишком ограничителен.

## Рекомендации по безопасности

1. **Предпочитать CommandLineExecutor**: Использовать контролируемую точку входа, когда возможно
2. **Указывать чёткую Reason**: «Launch build tools for CI pipeline» вместо расплывчатого «process access»
3. **Валидировать все входные данные**: Никогда не передавать недоверенные данные напрямую в ProcessStartInfo
4. **Использовать WaitForExit**: Всегда дожидаться завершения процесса для предотвращения зомби-процессов
5. **Перенаправлять потоки**: Устанавливать `RedirectStandardOutput = true` и `UseShellExecute = false`

## Файлы

- `Plugin.cs` — Демо-плагин с декларацией Capability.Process
- `README.md` — Этот файл (Английский)
- `README.zh-CN.md` — Упрощённый китайский
- Переводы: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Связанные примеры

- **09-ForbiddenProcess**: Антипаттерн заблокированных процессных операций
- **18-CapabilityDenied**: Антипаттерн недекларируемых возможностей
