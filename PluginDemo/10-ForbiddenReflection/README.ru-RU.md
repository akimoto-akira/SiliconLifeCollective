# PluginDemo-10: Антипаттерн запрещённых операций рефлексии

## Обзор

Этот плагин демонстрирует **запрещённые** операции рефлексии в системе плагинов SiliconLife. Он служит справочником антипаттернов, показывая, чего НЕ следует делать, и предоставляя правильные альтернативы для каждого нарушения.

## Почему рефлексия — основная угроза?

Обход через рефлексию — это **наиболее критическая угроза** для сканирования безопасности PluginLoader. В то время как сканирование TypeRef перехватывает прямые ссылки на типы во время компиляции, методы рефлексии могут разрешать типы во **время выполнения** с помощью строк — полностью невидимые для статического сканирования метаданных.

Если плагин может вызвать `Type.GetType("System.IO.File, System.Runtime")`, он может получить доступ к ЛЮБОМУ запрещённому типу, при этом ссылка не появится в таблице TypeRef метаданных PE.

## Какие методы запрещены?

Все запрещённые методы обнаруживаются через **сканирование MemberRef** (не блокировка на уровне пространства имён или типа):

| Запрещённый метод | Сигнатура | Угроза |
|------------------|-----------|--------|
| `Type.GetType` | `System.Type::GetType(System.String)` | Разрешение произвольного типа по имени во время выполнения |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | Создание экземпляра произвольного типа |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | Загрузка сборки по имени/байтам |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | Загрузка сборки с диска |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | Загрузка сборки по пути |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | Строковое разрешение типа |

## Что безопасно?

Не вся рефлексия запрещена. Следующие паттерны **безопасны**, так как ссылаются на типы, известные во время компиляции:

| Безопасный паттерн | Пример | Почему безопасно |
|-------------------|--------|-----------------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | Тип известен при компиляции, виден в TypeRef |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | Инспекция известного типа, новые типы не вводятся |
| Обобщённые ограничения | `FindSubtypesOf(typeof(BaseTool))` | Параметр обобщения — тип времени компиляции |
| `nameof()` | `nameof(MyClass.MyMethod)` | Строка компиляции, нет разрешения во время выполнения |

**Ключевое различие:**
- `typeof(X).Assembly` → **Безопасно** (ссылка компиляции, сканируется PluginLoader)
- `Assembly.Load("X")` → **Запрещено** (строка времени выполнения, обходит все сканирования)

## Как безопасно заменить рефлексию?

### Использование ITypeRegistry (Заменяет Type.GetType + сканирование AppDomain)

```csharp
// ❌ ЗАПРЕЩЕНО: Разрешение типа по строке во время выполнения
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ ПРАВИЛЬНО: Использовать ITypeRegistry для поиска зарегистрированных типов
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// Только типы, зарегистрированные во время OnLoad, обнаруживаемы
```

### Использование IObjectFactory (Заменяет Activator.CreateInstance)

```csharp
// ❌ ЗАПРЕЩЕНО: Создание произвольного экземпляра
object? instance = Activator.CreateInstance(someType);

// ✅ ПРАВИЛЬНО: Использовать IObjectFactory с зарегистрированной фабрикой
var instance = objectFactory.CreateInstance<MyService>();
// Только типы с зарегистрированными фабриками могут быть созданы
```

## Продемонстрированные нарушения

### Нарушение 1: Type.GetType(string)

```csharp
// ❌ ЗАПРЕЩЕНО
Type? fileType = Type.GetType("System.IO.File, System.Runtime");

// ✅ ПРАВИЛЬНО
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**Заблокированная MemberRef**: `System.Type::GetType(System.String)`

### Нарушение 2: Activator.CreateInstance

```csharp
// ❌ ЗАПРЕЩЕНО
object? client = Activator.CreateInstance(httpClientType!);

// ✅ ПРАВИЛЬНО
var instance = objectFactory.CreateInstance<MyService>();
```

**Заблокированная MemberRef**: `System.Activator::CreateInstance`

### Нарушение 3: Assembly.Load

```csharp
// ❌ ЗАПРЕЩЕНО
Assembly asm = Assembly.Load("System.Net.Http");

// ✅ ПРАВИЛЬНО
Assembly myAsm = typeof(MyPlugin).Assembly;  // Безопасно: известно при компиляции
```

**Заблокированная MemberRef**: `System.Reflection.Assembly::Load(System.String)`

### Нарушение 4: Assembly.LoadFile / LoadFrom

```csharp
// ❌ ЗАПРЕЩЕНО
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");

// ✅ ПРАВИЛЬНО
// Все зависимости должны находиться в каталоге плагина и сканироваться PluginLoader.
```

**Заблокированная MemberRef**: `System.Reflection.Assembly::LoadFile(System.String)`

### Нарушение 5: Assembly.GetType(string)

```csharp
// ❌ ЗАПРЕЩЕНО
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ ПРАВИЛЬНО
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
```

**Заблокированная MemberRef**: `System.Reflection.Assembly::GetType(System.String)`

## Почему typeof(X).Assembly безопасно, а Assembly.Load — нет

| Операция | Видимость | Безопасность |
|---------|-----------|-------------|
| `typeof(X).Assembly` | Тип X в таблице TypeRef → PluginLoader сканирует его | ✅ Безопасно |
| `Assembly.Load("X")` | Строка "X" существует только во время выполнения → невидима для TypeRef | ❌ Запрещено |
| `obj.GetType()` | Возвращает тип существующего экземпляра → новый тип не вводится | ✅ Безопасно |
| `Type.GetType("X")` | Разрешает произвольный тип из строки → обходит TypeRef | ❌ Запрещено |

## Лучшие практики

1. **Регистрировать типы в OnLoad**: Использовать `ITypeRegistry.RegisterType` / `RegisterFromAssembly`
2. **Использовать IObjectFactory для динамического создания**: Никогда не использовать `Activator.CreateInstance`
3. **Использовать typeof(X).Assembly**: Безопасный доступ к собственной сборке
4. **Избегать строковых имён типов**: Активирует сканирование IL-строк
5. **Проектировать для статической обнаруживаемости**: Невидимо в метаданных = подозрительно

## Файлы

- `Plugin.cs` - Демонстрационный плагин антипаттерна
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

- **02-TypeRegistryUsage**: Правильное использование ITypeRegistry
- **03-ObjectFactoryUsage**: Правильное использование IObjectFactory
- **11-ForbiddenPInvoke**: Запрещённые P/Invoke и unsafe-код
- **12-ForbiddenStringBypass**: Попытки обхода через строковую рефлексию
