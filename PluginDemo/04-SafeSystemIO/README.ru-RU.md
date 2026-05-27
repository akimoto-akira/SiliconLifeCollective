# Демо безопасного System.IO

Демонстрирует типы System.IO из белого списка `SystemIOAllowedTypes`: `MemoryStream`, `BinaryReader`/`BinaryWriter`, `GZipStream`. Объясняет, почему `FileStream` требует `PermissionedStreamFactory`.

## Белый список SystemIOAllowedTypes

Среда выполнения плагинов по умолчанию блокирует пространство имён `System.IO`, но освобождает типы, которые **не выполняют прямой файловый ввод-вывод**:

| Категория | Разрешённые типы | Почему безопасно |
|-----------|-----------------|-----------------|
| Абстракции потоков | `Stream` | Абстрактный базовый класс, собственного I/O нет |
| Потоки в памяти | `MemoryStream` | Чисто内存ная операция |
| Потоки сжатия | `GZipStream`, `DeflateStream`, `ZLibStream` | Оборачивают другой поток, не открывают файлы |
| Бинарные обёртки | `BinaryReader`, `BinaryWriter` | Оборачивают любой поток, не открывают файлы |
| Перечисления | `SeekOrigin`, `FileMode`, `FileAccess`, `FileShare`, `CompressionMode`, `CompressionLevel` | Только типы значений |
| Исключения | `IOException`, `InvalidDataException`, `EndOfStreamException` | Только типы ошибок |

### Типы не в белом списке

Эти типы **напрямую обращаются к файловой системе** и **блокируются** в коде плагинов:

| Заблокированный тип | Причина | Безопасная альтернатива |
|--------------------|--------|------------------------|
| `FileStream` | Напрямую открывает файлы | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | Статические файловые операции | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | Статические операции с каталогами | `SafePath` (проверка разрешений) |
| `FileInfo` | Обёртка путей файлов | `SafePath` |
| `DirectoryInfo` | Обёртка путей каталогов | `SafePath` |
| `StreamReader` | Напрямую открывает файлы | `PermissionedStreamFactory` + обёртка `PermissionedStream` |
| `StreamWriter` | Напрямую открывает файлы | `PermissionedStreamFactory` + обёртка `PermissionedStream` |

## Почему FileStream требует PermissionedStreamFactory

`FileStream` напрямую открывает файлы на диске — серьёзный риск безопасности в плагинной системе. `PermissionedStreamFactory` принудительно выполняет:

1. **Проверку разрешений** — `PermissionManager` вызывающего должен предоставить `FileAccess` для пути
2. **Аудиторский журнал** — каждое открытие файла записывается с ID being вызывающего
3. **Валидацию пути** — пустые/недопустимые пути отклоняются до любого I/O

```
❌ new FileStream("path", FileMode.Open)           → Заблокировано сканером TypeRef
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → Проверка разрешений пройдена
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → Проверка разрешений пройдена
```

## Пайплайн демо

Это демо строит полный внутрипамятный конвейер данных, используя только типы из белого списка:

```
┌─────────────────────────────────────────────────────────────────┐
│  Демо 1: MemoryStream                                           │
│  └─ Запись байтов → Чтение байтов → Декодирование строки        │
│                                                                  │
│  Демо 2: Конвейер сжатия                                         │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(сжатие) → MemoryStream(сжатые)                  │
│     → GZipStream(распаковка) → MemoryStream(исходные)            │
│     → UTF8 → string (круговой обход)                             │
│                                                                  │
│  Демо 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → Проверка кругового обхода     │
└─────────────────────────────────────────────────────────────────┘
```

## Это демо

> **⚠️ Примечание:** Это демо использует **только** типы из белого списка `SystemIOAllowedTypes`. Файловый I/O не выполняется. Для доступа к файлам см. API `PermissionedStreamFactory`.

| Класс | Роль |
|-------|------|
| `SafeSystemIOPlugin` | Реализация `IPlugin` — демонстрирует безопасное использование System.IO |

## Примечание по безопасности

Пространство имён `System.IO` блокируется сканером TypeRef плагинов. Проходят только типы из белого списка. Для реального доступа к файлам необходимо использовать `PermissionedStreamFactory`, который выполняет проверки разрешений и аудиторское журналирование. См. [документацию по безопасности](../../docs/ru-RU/security.md).
