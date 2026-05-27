# PluginDemo-20: SpeedyPack — Структурированное хранилище данных

## Обзор

Этот плагин демонстрирует использование `SpeedyPack` для структурированного хранения данных **без какой-либо декларации возможности**. SpeedyPack — **рекомендуемый** способ сохранения данных плагинами.

## Почему SpeedyPack?

| Функция | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|---------|-----------|--------------------------|------------------------------|
| Требуемая возможность | **Нет** | Нет | `Capability.FileIO` |
| Кэширование | ✅ Встроенное | ❌ | ❌ |
| WAL (восстановление после сбоя) | ✅ | ❌ | ❌ |
| Транзакции | ✅ `IPackTransaction` | ❌ | ❌ |
| Потокобезопасность | ✅ | ❌ | ❌ |
| Структурированная сериализация | ✅ `Read<T>` | ❌ Сырые байты | ❌ Вручную |
| Аудиторский след | ✅ Автоматический | ✅ Автоматический | ❌ Вручную |

## Базовый CRUD

```csharp
// Открыть файл данных SpeedyPack
using var pack = SpeedyPack.Open("mydata.spk");

// Записать пары ключ-значение
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// Прочитать значения (типизированные)
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// Удалить ключ
pack.Delete("user:age");

// Проверить существование
bool exists = pack.Contains("user:name");  // true
```

## Типизированный доступ со структурированными объектами

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// Записать структурированный объект
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// Прочитать типизированный объект
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## Транзакции

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // Атомарно — обе записи сохраняются или ни одна
    }
    catch
    {
        tx.Rollback();  // Отменить все записи в этой транзакции
    }
}
```

### Методы IPackTransaction

| Метод | Описание |
|-------|----------|
| `Write(key, value)` | Поставить операцию записи в очередь |
| `Delete(key)` | Поставить операцию удаления в очередь |
| `Commit()` | Атомарно применить все операции в очереди |
| `Rollback()` | Отменить все операции в очереди |

## Конфигурация с SpeedyPackOptions

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 МБ кэша
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### Свойства SpeedyPackOptions

| Свойство | Тип | По умолчанию | Описание |
|---------|-----|-------------|----------|
| `MaxCacheSize` | `long` | 64 МБ | Максимальный размер кэша в памяти |
| `AutoFlushInterval` | `TimeSpan` | 10 секунд | Интервал сброса кэша на диск |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | Уровень сжатия хранимых данных |

## Примечание по безопасности

SpeedyPack **не требует** декларации возможности. Это безопасная контролируемая точка входа для хранения данных, которая:
- Проверяет все пути относительно границ рабочего пространства
- Обеспечивает полный аудиторский след всех операций чтения/записи
- Предотвращает атаки обхода каталогов
- Автоматически управляет жизненным циклом ресурсов

## Файлы

- `Plugin.cs` — Демо-плагин SpeedyPack
- `README.md` — Этот файл (Английский)
- `README.zh-CN.md` — Упрощённый китайский
- Переводы: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Связанные примеры

- **04-SafeSystemIO**: Разрешённые типы System.IO в памяти (декларация не нужна)
- **07-ForbiddenFileIO**: Антипаттерн заблокированных файловых операций
- **14-CapabilityFileIO**: Когда SpeedyPack недостаточно
