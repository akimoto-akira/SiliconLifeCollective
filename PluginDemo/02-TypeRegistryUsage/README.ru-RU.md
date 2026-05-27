# Демо ITypeRegistry – Регистрация и запросы

Демонстрирует регистрацию и поиск через `ITypeRegistry`: зарегистрировать пользовательские типы в `OnLoad`, обнаружить их через `FindSubtypesOf` в `OnStart`.

## Обзор интерфейса ITypeRegistry

`ITypeRegistry` заменяет сканирование через рефлексию `AppDomain.CurrentDomain.GetAssemblies()`. Плагины явно регистрируют свои типы в `IPlugin.OnLoad`, а среда выполнения ищет типы только через реестр.

```csharp
public interface ITypeRegistry
{
    void RegisterType(Type type);
    void RegisterTypes(IEnumerable<Type> types);
    void RegisterFromAssembly(System.Reflection.Assembly assembly, Type baseType);
    Type? FindType(string fullName);
    IEnumerable<Type> FindSubtypesOf(Type baseType);
    IEnumerable<Type> FindImplementationsOf(Type interfaceType);
}
```

### Сводка методов

| Метод | Описание |
|-------|----------|
| `RegisterType(Type)` | Регистрирует один тип |
| `RegisterTypes(IEnumerable<Type>)` | Регистрирует несколько типов за один вызов |
| `RegisterFromAssembly(Assembly, Type)` | Регистрирует все неабстрактные подтипы `baseType` из указанной сборки |
| `FindType(string)` | Находит тип по полному имени; поддерживает разрешение имён обобщённых типов |
| `FindSubtypesOf(Type)` | Находит все неабстрактные подтипы указанного базового типа |
| `FindImplementationsOf(Type)` | Находит все неабстрактные типы, реализующие указанный интерфейс |

## Поток регистрации и запросов

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Получить ITypeRegistry из ServiceLocator                 │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  Альтернатива: RegisterFromAssembly                          │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → зарегистрировать все подтипы DemoTool за один раз      │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ Итерация результатов → GreetingTool, FarewellTool, …    │
└──────────────────────────────────────────────────────────────┘
```

## Использование RegisterFromAssembly

`RegisterFromAssembly` сканирует сборку и регистрирует все неабстрактные подтипы указанного базового типа:

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // сборка для сканирования
    typeof(DemoTool)                            // регистрировать только подтипы DemoTool
);
```

Это эквивалентно вызову `RegisterType` для каждого подтипа по отдельности, но более лаконично, когда плагин определяет множество типов с общим базовым классом.

## Это демо

> **⚠️ Важно:** `DemoTool` — это **пользовательский тип, определённый исключительно для этого демо**, чтобы продемонстрировать регистрацию и запросы через `ITypeRegistry`. Он **не имеет никакого отношения** к системному интерфейсу `ITool` (`SiliconLife.Collective.ITool`), используемому для регистрации AI-инструментов. Название «Tool» — совпадение; любая пользовательская иерархия классов работает так же.

| Класс | Роль |
|-------|------|
| `DemoTool` | Пользовательский абстрактный базовый класс — якорь регистрации (не связан с `ITool`) |
| `GreetingTool` | Конкретный подтип, зарегистрированный в `OnLoad` |
| `FarewellTool` | Конкретный подтип, зарегистрированный в `OnLoad` |
| `StatusTool` | Конкретный подтип, зарегистрированный в `OnLoad` |
| `TypeRegistryUsagePlugin` | Реализация `IPlugin` — регистрация и запрос типов |

## Примечание по безопасности

`ITypeRegistry` является частью модели безопасности контролируемого доступа. Плагины **не должны** использовать `AppDomain.CurrentDomain.GetAssemblies()` или `Assembly.GetTypes()` для обнаружения типов — они должны использовать `ITypeRegistry`. См. [документацию по безопасности](../../docs/ru-RU/security.md).
