# Запрещённые файловые операции ввода/вывода — Антипаттерн

Демонстрирует **запрещённые** файловые операции ввода/вывода в системе плагинов. Этот пример служит справочником антипаттернов, показывая, чего делать НЕЛЬЗЯ, и предоставляя правильные альтернативы для каждого нарушения.

## Почему System.IO полностью запрещён?

Всё пространство имён `System.IO` заблокировано на уровне плагинов, поскольку прямой доступ к файлам создаёт серьёзные риски безопасности:

1. **Несанкционированный доступ к файлам**: Плагины могут читать конфиденциальные файлы за пределами рабочего пространства (пароли, ключи, персональные данные)
2. **Атаки перезаписи файлов**: Вредоносные плагины могут перезаписать критически важные системные или конфигурационные файлы
3. **Обход каталогов**: Плагины могут использовать пути `../` для выхода за границы рабочего пространства
4. **Исчерпание ресурсов**: Неконтролируемое создание файлов может заполнить дисковое пространство
5. **Отсутствие аудита**: Прямые файловые операции обходят систему аудита безопасности плагинов

## Запрещённые типы

Все типы `System.IO`, напрямую обращающиеся к файловой системе, заблокированы:

| Запрещённый тип | Заблокированный метод | Уровень риска |
|----------------|----------------------|--------------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` и др. | 🔴 Критический |
| `FileStream` | Конструктор с путём к файлу | 🔴 Критический |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 Критический |
| `StreamReader` | Конструктор с путём (строка) | 🔴 Критический |
| `StreamWriter` | Конструктор с путём (строка) | 🔴 Критический |
| `FileInfo` | Все методы | 🔴 Критический |
| `DirectoryInfo` | Все методы | 🔴 Критический |

## Разрешённые типы (исключения белого списка)

Типы, выполняющие **чисто операции в памяти** (без прямого доступа к файловой системе), разрешены:

| Разрешённый тип | Назначение | Почему безопасен |
|----------------|------------|-----------------|
| `MemoryStream` | Потоковая обработка байтов в памяти | Нет доступа к файловой системе |
| `BinaryReader` | Чтение из существующего потока | Обёртка потока, не открывает файлы |
| `BinaryWriter` | Запись в существующий поток | Обёртка потока, не создаёт файлы |
| `GZipStream` | Сжатие/распаковка | Обёртка потока, нет доступа к файлам |
| `StreamReader` | Конструктор с параметром `Stream` | Безопасен при обёртке аудированного потока |
| `StreamWriter` | Конструктор с параметром `Stream` | Безопасен при обёртке аудированного потока |

См. пример **04-SafeSystemIO** для разрешённых типов.

## Безопасный доступ к файлам через PermissionedStreamFactory

`PermissionedStreamFactory` — это **контролируемая точка входа** для файловых операций в плагинах:

```csharp
// ✅ Правильно: чтение файла
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ Правильно: запись файла
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("Данные журнала");
```

**PermissionedStreamFactory предоставляет:**
1. **Валидация пути**: Предотвращает атаки обхода каталогов (`../`)
2. **Проверка разрешений**: Гарантирует, что файл находится в разрешённом рабочем пространстве
3. **Журнал аудита**: Все обращения к файлам записываются для проверки безопасности
4. **Очистка ресурсов**: Отслеживает открытые потоки и предотвращает утечки

## Нарушения в данном примере

### Нарушение 1: File.ReadAllText

```csharp
// ❌ Запрещено — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ Правильная альтернатива
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### Нарушение 2: File.WriteAllText

```csharp
// ❌ Запрещено — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ Правильная альтернатива
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### Нарушение 3: Прямой FileStream

```csharp
// ❌ Запрещено — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ Правильная альтернатива
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### Нарушение 4: Directory.GetFiles

```csharp
// ❌ Запрещено — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ Правильная альтернатива (с использованием SpeedyPack)
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### Нарушение 5: StreamReader с прямым путём

```csharp
// ❌ Запрещено — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ Правильная альтернатива
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## Сравнение с другими примерами

| Пример | Фокус | Требуемое разрешение |
|--------|-------|---------------------|
| **04-SafeSystemIO** | Разрешённые типы памяти (MemoryStream, GZipStream) | Нет |
| **07-ForbiddenFileIO** | Запрещённые паттерны доступа к файлам (данный пример) | Не применимо (заблокировано) |
| **14-CapabilityFileIO** | Объявление capability FileIO для обхода ограничений | `Capability.FileIO` |

## Механизм сканирования безопасности PluginLoader

При сканировании этого плагина PluginLoader:

1. **Сканирование TypeRef**: Обнаруживает ссылки на запрещённые типы `System.IO`
2. **Сканирование MemberRef**: Обнаруживает вызовы заблокированных методов
3. **Сканирование IL-строк**: Обнаруживает попытки обхода через рефлексию на основе строк
4. **Отклонение**: Плагин отклоняется при загрузке с подробным сообщением об ошибке

Обход через конкатенацию строк, рефлексию, динамическую загрузку или обфускацию невозможен — всё это перехватывается сканированием на уровне IL (см. **12-ForbiddenStringBypass**).

## Замечание по безопасности

Если вам действительно необходим неограниченный доступ к файлам, вы можете объявить `Capability.FileIO` (см. 14-CapabilityFileIO). Однако лучшие практики:
- Предпочитать **SpeedyPack** для хранения структурированных данных (не требует объявления разрешений)
- Использовать **PermissionedStreamFactory** когда нужен доступ к файлам (контролируемая точка входа)
- Объявлять `Capability.FileIO` только если вышеуказанные решения недостаточны
