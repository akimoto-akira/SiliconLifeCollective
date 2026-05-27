# PluginDemo-12: Запрещённый обход через строки рефлексии (анти-паттерн)

## Обзор

Этот плагин демонстрирует **запрещённые** попытки обхода на основе строк рефлексии в системе плагинов SiliconLife. Он показывает, почему конкатенация, интерполяция, кодирование и другие методы обфускации **не могут** обойти сканирование кучи #US (User String) PluginLoader — **последнюю линию обороны**.

## Что такое куча #US?

В метаданных .NET PE (Portable Executable) **куча #US (User String)** хранит все строковые литеральные операнды, используемые инструкциями IL `ldstr`. Каждый раз, когда вы пишете строковый литерал в коде C#, компилятор сохраняет его в этой куче.

```
Исходник C#:  string s = "System.IO.File";
    ↓ компиляция
IL код:       ldstr "System.IO.File"    ← ссылается на токен в куче #US
    ↓ сканирование PluginLoader
Куча #US:     [..., "System.IO.File", ...]  ← ОБНАРУЖЕНО по совпадению префикса!
```

Метод `ScanUserStrings()` PluginLoader перебирает **каждую запись** в куче #US, проверяя, начинается ли какая-либо строка с запрещённого префикса.

## Запрещённые строковые префиксы

Следующие префиксы вызывают нарушения `[ILString]` при обнаружении в куче #US:

| Префикс | Категория |
|---------|-----------|
| `System.IO.` | Типы файловой системы |
| `System.Net.Http` | HTTP-клиент |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | Сырые сокеты |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | Зондирование сети |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | Процесс/командная строка |
| `Microsoft.CodeAnalysis` | Компилятор Roslyn |
| `System.Reflection.Emit` | Эмиссия IL |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | Устаревший CodeDom |
| `Microsoft.Win32` | Реестр Windows |

## Продемонстрированные нарушения

### Нарушение 1: Прямая строка имени типа

```csharp
// ❌ ЗАПРЕЩЕНО — полная строка находится в куче #US
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**Нарушение**: `[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### Нарушение 2: Конкатенация строк (время компиляции)

```csharp
// ❌ ЗАПРЕЩЕНО — компилятор сворачивает const+const в одну запись #US
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
```

**Нарушение**: `[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### Нарушение 3: Интерполяция строк

```csharp
// ❌ ЗАПРЕЩЕНО — литеральные части хранятся в куче #US
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
```

**Нарушение**: `[ILString] "System.IO." matches forbidden prefix "System.IO."`

### Нарушение 4: Поля Const

```csharp
// ❌ ЗАПРЕЩЕНО — значения const инлайнятся в месте использования → появляются в куче #US
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**Нарушение**: `[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### Нарушение 5: Фрагменты частичных строк

```csharp
// ❌ ЗАПРЕЩЕНО — каждая часть — отдельный ldstr, сканируемый независимо
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
```

**Нарушение**: `[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

## Почему все методы обфускации терпят неудачу

| Метод | Почему не работает |
|-------|-------------------|
| Конкатенация const | Компилятор сворачивает в одну запись #US |
| Интерполяция строк | Литеральные части хранятся в куче #US |
| Поля const | Значения инлайнятся → появляются в #US |
| Разделение на переменные | Каждый операнд `ldstr` сканируется независимо |
| Кодирование Base64 | Декодирование требует runtime-методов, но `Type.GetType` заблокирован MemberRef |
| Построение char-массива | Не генерирует `ldstr`, но `Type.GetType` всё ещё заблокирован MemberRef |
| Шифрование XOR | Зашифрованная строка нечитаема в #US, но расшифровка + `Type.GetType` = MemberRef блокировка |

**Ключевой инсайт**: Сканирование #US блокирует **строку**. Сканирование MemberRef блокирует **метод**. Для динамической загрузки типа нужны ОБА. PluginLoader блокирует ОБА независимо.

## Полная цепь обороны

| Шаг | Механизм | Что обнаруживает |
|-----|----------|-----------------|
| 1 | Таблица TypeRef | Прямые ссылки на запрещённые типы |
| 2 | Таблица ExportedType | Перенаправленные типы из запрещённых пространств имён |
| 3 | Таблица MemberRef | Вызовы `Type.GetType`, `Assembly.Load`, `Activator.CreateInstance` |
| 4 | Маркеры Unsafe | `[DllImport]`, блоки unsafe, флаг PinvokeImpl |
| **5** | **Сканирование кучи #US** | **Строковые константы, совпадающие с запрещёнными префиксами (это демо)** |

## Файлы

- `Plugin.cs` - Плагин-демонстрация анти-паттерна
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

- **10-ForbiddenReflection**: Запрещённые методы рефлексии (сканирование MemberRef)
- **11-ForbiddenPInvoke**: Запрещённый P/Invoke и unsafe-код
- **02-TypeRegistryUsage**: Правильное использование ITypeRegistry
- **03-ObjectFactoryUsage**: Правильное использование IObjectFactory
