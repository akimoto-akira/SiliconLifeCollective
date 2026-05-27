# PluginDemo-14: Capability.FileIO — Декларативное разрешение файлового ввода-вывода

## Обзор

Этот плагин демонстрирует использование `[PluginCapability(Capability.FileIO)]` для объявления прямого доступа к файловой системе. С этой декларацией плагин получает доступ ко всем типам `System.IO` за пределами встроенного белого списка `SystemIOAllowedTypes`.

## Синтаксис объявления PluginCapability

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Как работает Capability.FileIO

1. **Состояние по умолчанию**: Пространство имён `System.IO` полностью запрещено; разрешены только типы из белого списка `SystemIOAllowedTypes` (MemoryStream, BinaryReader, GZipStream и т.д.)
2. **С декларацией**: Запрет на всё пространство имён `System.IO` снимается — File, FileStream, Directory, StreamReader(string) и т.д. становятся доступными
3. **Освобождение ILString**: Строковые константы, начинающиеся с `"System.IO."`, не помечаются
4. **Недекларируемые ограничения**: P/Invoke, Unsafe, Reflection.Emit и т.д. остаются заблокированными

## Область освобождения Capability.FileIO

### Освобождение TypeRef

Все типы `System.IO` освобождаются:

| Категория | Освобождённые типы |
|-----------|-------------------|
| Файловые операции | `File`, `FileInfo` |
| Операции с каталогами | `Directory`, `DirectoryInfo` |
| Типы потоков | `FileStream`, `StreamReader(path)`, `StreamWriter(path)` |
| Файловая система | `FileSystemWatcher`, `DriveInfo`, `Path` |

### Освобождение ILString

- Строки, начинающиеся с `"System.IO."`, не помечаются

### Что остаётся запрещённым

| Категория | По-прежнему заблокировано |
|-----------|--------------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` |
| Небезопасный код | `UnverifiableCodeAttribute`, `Unsafe` |
| IL-генерация | `System.Reflection.Emit.*` |
| Загрузка сборок | `System.Runtime.Loader`, `Assembly.Load*` |
| Реестр | `Microsoft.Win32.*` |

## Сравнение с другими примерами

| Пример | Декларация | Доступ к файлам | Примечания |
|--------|-----------|----------------|-----------|
| **04-SafeSystemIO** | Нет | MemoryStream, BinaryReader, GZipStream | Использует только типы из белого списка |
| **07-ForbiddenFileIO** | Нет | ❌ ОТКЛОНЕНО | Антипаттерн |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ Полный доступ к System.IO | Этот пример |
| **20-SpeedyPack** | Нет | Через API SpeedyPack (Capability не требуется) | Рекомендуемое хранилище данных |

## Приоритет доступа к файлам

1. **SpeedyPack** — Без декларации возможности. Встроенное кэширование, WAL, транзакции. **Рекомендуется для хранения структурированных данных.**
2. **PermissionedStreamFactory** — Без декларации. Аудитируемый доступ с валидацией пути и контролем доступа.
3. **Capability.FileIO + прямой System.IO** — Только когда вышеперечисленного недостаточно.

## Почему стоит предпочесть PermissionedStreamFactory / SpeedyPack?

Даже с `Capability.FileIO` рекомендуется использовать контролируемые точки входа, потому что:

1. **Аудиторский след**: Все обращения регистрируются и отслеживаются
2. **Валидация путей**: Предотвращает атаки обхода каталогов (`../`)
3. **Контроль доступа**: Принудительное соблюдение границ рабочего пространства
4. **Отслеживание ресурсов**: Предотвращает утечки потоков и истощение ресурсов
5. **Соответствие требованиям**: Контролируемые шаблоны доступа облегчают прохождение проверок безопасности

## Рекомендации по безопасности

1. **Объявлять FileIO только при реальной необходимости**: Можно ли использовать SpeedyPack или PermissionedStreamFactory?
2. **Указывать чёткую Reason**: «Direct log file access for audit trail» лучше, чем «file access»
3. **Самостоятельно валидировать пути**: Даже с Capability.FileIO проверять все пути к файлам перед использованием
4. **Использовать инструкции using**: Всегда освобождать FileStream/StreamReader/StreamWriter
5. **Принцип наименьших привилегий**: Объявлять только те возможности, которые реально нужны плагину

## Файлы

- `Plugin.cs` — Демо-плагин с декларацией Capability.FileIO
- `README.md` — Этот файл (Английский)
- `README.zh-CN.md` — Упрощённый китайский
- Переводы: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Связанные примеры

- **04-SafeSystemIO**: Разрешённые типы System.IO в памяти (без декларации)
- **07-ForbiddenFileIO**: Антипаттерн заблокированных файловых операций
- **20-SpeedyPack**: Рекомендуемое хранилище данных без декларации возможностей
- **18-CapabilityDenied**: Антипаттерн недекларируемых возможностей
