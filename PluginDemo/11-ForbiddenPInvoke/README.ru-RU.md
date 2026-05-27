# PluginDemo-11: Запрещённые операции P/Invoke и unsafe-кода

## Обзор

Этот плагин демонстрирует **запрещённые** операции P/Invoke и unsafe-кода в системе плагинов SiliconLife. В отличие от других запрещённых категорий (файловый ввод/вывод, сеть, процессы, рефлексия), имеющих безопасные обёртки-альтернативы, P/Invoke и unsafe-код являются **абсолютным запретом** — без безопасной альтернативы и без возможности освобождения через любую декларацию `PluginCapability`.

## Почему P/Invoke — абсолютная угроза?

P/Invoke и unsafe-код представляют **самую фундаментальную угрозу** безопасности плагинов, поскольку они работают **полностью за пределами управляемой среды выполнения**:

- Нативный код выполняется с полными привилегиями процесса
- Отсутствие управляемой типобезопасности, безопасности памяти или сборки мусора
- Невозможно перехватить, аудировать или изолировать нативные вызовы
- Сбой нативного кода = сбой всего процесса (без обработки исключений)
- Доступ к любому адресу памяти в пространстве процесса

## Механизм тройной страховки

PluginLoader использует **три независимых уровня обнаружения**:

### Уровень 1: Сканирование таблицы TypeRef

Обнаруживает прямые ссылки на запрещённые типы в метаданных PE:

| Запрещённый тип | Пространство имён | Угроза |
|-----------------|-------------------|--------|
| `DllImportAttribute` | System.Runtime.InteropServices | Объявление импорта нативной функции |
| `Marshal` | System.Runtime.InteropServices | Мост управляемой/неуправляемой памяти |
| `NativeMemory` | System.Runtime.InteropServices | Нативная куча malloc/free |
| `NativeLibrary` | System.Runtime.InteropServices | Динамическая загрузка нативных библиотек |
| `GCHandle` | System.Runtime.InteropServices | Закрепление управляемого объекта |
| `Unsafe` | System.Runtime.CompilerServices | Вспомогательный класс Unsafe |
| `UnverifiableCodeAttribute` | System.Security | Маркер неверифицируемого кода |

### Уровень 2: Сканирование маркеров Unsafe (ScanUnsafeMarkers)

| Маркер | Метод обнаружения | Источник |
|--------|-------------------|----------|
| `[assembly: UnverifiableCode]` | Таблица CustomAttribute сборки | Ключевое слово C# `unsafe` |
| `[module: UnverifiableCode]` | Таблица CustomAttribute модуля | Ключевое слово C# `unsafe` |
| `MethodAttributes.PinvokeImpl` | Флаг таблицы MethodDef | Атрибут `[DllImport]` |

### Уровень 3: Сканирование строк IL (куча #US)

```
"System.Runtime.InteropServices.Marshal"  → Помечено
"System.Runtime.InteropServices.*"        → Помечено по совпадению префикса
```

## Демонстрируемые нарушения

### Нарушение 1: Объявление [DllImport]

```csharp
// ❌ ЗАПРЕЩЕНО
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();
```

### Нарушение 2: Использование Marshal

```csharp
// ❌ ЗАПРЕЩЕНО
IntPtr ptr = Marshal.AllocHGlobal(1024);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.FreeHGlobal(ptr);
```

### Нарушение 3: Использование NativeMemory

```csharp
// ❌ ЗАПРЕЩЕНО
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Free(buffer);
}
```

### Нарушение 4: Закрепление GCHandle

```csharp
// ❌ ЗАПРЕЩЕНО
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

### Нарушение 5: Блок unsafe

```csharp
// ❌ ЗАПРЕЩЕНО
unsafe
{
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
}
```

### Нарушение 6: Загрузка NativeLibrary

```csharp
// ❌ ЗАПРЕЩЕНО
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
NativeLibrary.Free(lib);
```

## Нет безопасной альтернативы — Сравнение

| Запрещённая категория | Безопасная обёртка | Аудируемо | Декларируемо через PluginCapability |
|----------------------|-------------------|-----------|-------------------------------------|
| Файловый ввод/вывод | PermissionedStreamFactory | ✅ Да | ✅ Capability.FileIO |
| Сеть | NetworkExecutor | ✅ Да | ✅ Capability.Network |
| Процесс | CommandLineExecutor | ✅ Да | ✅ Capability.Process |
| Рефлексия | ITypeRegistry + IObjectFactory | ✅ Да | ❌ Всегда запрещено |
| **P/Invoke и unsafe** | **❌ Нет** | **❌ Невозможно** | **❌ Всегда запрещено** |

## Если плагину действительно нужен нативный код

1. **Ручной аудит сопровождающим проекта**
2. **Добавление в белый список `TrustedAssemblies`** в PluginLoader
3. **Идентификация по `AssemblyDefinition.Name` метаданных PE** (не имя файла)

## Файлы

- `Plugin.cs` - Демонстрационный плагин-антипаттерн
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

- **04-SafeSystemIO**: Безопасные типы белого списка System.IO
- **06-TrustedDependency**: Механизм белого списка TrustedAssemblies
- **10-ForbiddenReflection**: Запрещённые операции рефлексии
- **12-ForbiddenStringBypass**: Попытки обхода через строки рефлексии
