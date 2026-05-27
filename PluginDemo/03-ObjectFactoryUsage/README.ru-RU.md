# Демо IObjectFactory — Регистрация и создание экземпляров

Демонстрирует регистрацию и создание экземпляров с помощью `IObjectFactory`: регистрация типов через `RegisterAutoFactory` в `OnLoad`, создание экземпляров через `CreateInstance` в `OnStart`.

## Обзор интерфейса IObjectFactory

`IObjectFactory` заменяет `Activator.CreateInstance()`. Плагины регистрируют делегаты фабрик в `IPlugin.OnLoad`, а среда выполнения создаёт экземпляры только через зарегистрированные делегаты, предотвращая произвольное создание типов.

```csharp
public interface IObjectFactory
{
    void RegisterFactory(Type type, Func<object?[], object> factory);
    void RegisterFactory<T>(Func<object?[], T> factory) where T : class;
    void RegisterAutoFactory(Type type);
    void RegisterAutoFactoryFromAssembly(System.Reflection.Assembly assembly, Type baseType);
    object? CreateInstance(Type type, params object?[] args);
    T? CreateInstance<T>(params object?[] args) where T : class;
    bool IsRegistered(Type type);
}
```

### Краткий обзор методов

| Метод | Описание |
|-------|----------|
| `RegisterFactory(Type, Func)` | Регистрирует пользовательский делегат фабрики для типа |
| `RegisterFactory<T>(Func)` | Обобщённая версия `RegisterFactory` |
| `RegisterAutoFactory(Type)` | Автоматически анализирует конструкторы типа и регистрирует фабрику |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | Автоматически регистрирует фабрики для всех неабстрактных подтипов в сборке |
| `CreateInstance(Type, args)` | Создаёт экземпляр через зарегистрированную фабрику (необобщённый) |
| `CreateInstance<T>(args)` | Создаёт экземпляр через зарегистрированную фабрику (обобщённый) |
| `IsRegistered(Type)` | Проверяет, зарегистрирована ли фабрика для типа |

## Почему IObjectFactory заменяет Activator.CreateInstance

`Activator.CreateInstance` допускает произвольное создание экземпляров, что является угрозой безопасности в плагинной системе. `IObjectFactory` принудительно использует модель белого списка:

- Только типы с **зарегистрированной фабрикой** могут быть созданы
- Фабрики регистрируются явно в `OnLoad`, хост имеет полный контроль
- `RegisterAutoFactory` — удобный метод, анализирующий конструкторы, но регистрация обязательна

```
❌ Activator.CreateInstance(typeof(SomeType))     → угроза безопасности
✅ factory.CreateInstance(typeof(SomeType))         → только зарегистрированные типы
✅ factory.CreateInstance<SomeType>()               → обобщённый удобный метод
```

## Как работает RegisterAutoFactory

`RegisterAutoFactory` проверяет конструкторы типа и генерирует делегат фабрики:

1. **Без аргументов** → вызывает конструктор без параметров
2. **С аргументами** → сопоставляет параметры конструктора по типу, откат к конструктору без параметров
3. **Абстрактные типы/интерфейсы** → отклоняются с предупреждением

## Поток регистрации и создания

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Получить IObjectFactory из ServiceLocator                │
│  ├─ RegisterAutoFactory(typeof(SimpleService))               │
│  └─ RegisterAutoFactory(typeof(ConfiguredService))           │
│                                                              │
│  OnStart                                                     │
│  ├─ CreateInstance(typeof(SimpleService))                    │
│  │  → "SimpleService created via parameterless constructor"  │
│  └─ CreateInstance<ConfiguredService>("DemoPlugin")          │
│     → "ConfiguredService created with name='DemoPlugin'"     │
└──────────────────────────────────────────────────────────────┘
```

## Это демо

> **⚠️ Примечание:** `SimpleService` и `ConfiguredService` — **пользовательские типы, определённые исключительно для этого демо**. Они не связаны с какими-либо системными интерфейсами служб.

| Класс | Роль |
|-------|------|
| `SimpleService` | Демо-тип, конструктор без параметров |
| `ConfiguredService` | Демо-тип, параметризованный конструктор `(string name)` |
| `ObjectFactoryUsagePlugin` | Реализация `IPlugin` — регистрирует фабрики и создаёт экземпляры |

## Примечание по безопасности

`IObjectFactory` является частью модели безопасности контролируемого доступа. Плагины **не должны** использовать `Activator.CreateInstance` для создания объектов — они должны регистрировать фабрики и использовать `CreateInstance`. См. [документацию по безопасности](../../docs/ru-RU/security.md).
