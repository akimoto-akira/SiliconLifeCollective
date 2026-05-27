# ObjectFactory Usage Demo

Demonstrates `IObjectFactory` registration and instance creation: register types with `RegisterAutoFactory` in `OnLoad`, create instances with `CreateInstance` in `OnStart`.

## IObjectFactory Interface Overview

`IObjectFactory` replaces `Activator.CreateInstance()`. Plugins register factory delegates during `IPlugin.OnLoad`, and the runtime only creates instances through registered delegates, preventing arbitrary type instantiation.

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

### Method Summary

| Method | Description |
|--------|-------------|
| `RegisterFactory(Type, Func)` | Registers a custom factory delegate for a type |
| `RegisterFactory<T>(Func)` | Generic version of `RegisterFactory` |
| `RegisterAutoFactory(Type)` | Auto-registers a factory by analyzing the type's constructors |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | Auto-registers factories for all non-abstract subtypes in an assembly |
| `CreateInstance(Type, args)` | Creates an instance using a registered factory (non-generic) |
| `CreateInstance<T>(args)` | Creates an instance using a registered factory (generic) |
| `IsRegistered(Type)` | Checks whether a factory is registered for a type |

## Why IObjectFactory Replaces Activator.CreateInstance

`Activator.CreateInstance` allows arbitrary type instantiation, which is a security risk in a plugin system. `IObjectFactory` enforces a whitelist model:

- Only types with a **registered factory** can be instantiated
- Factories are registered explicitly in `OnLoad`, giving the host full control
- `RegisterAutoFactory` is a convenience that analyzes constructors but still goes through the registration gate

```
❌ Activator.CreateInstance(typeof(SomeType))     → security risk
✅ factory.CreateInstance(typeof(SomeType))         → only if registered
✅ factory.CreateInstance<SomeType>()               → generic convenience
```

## How RegisterAutoFactory Works

`RegisterAutoFactory` inspects the type's constructors and generates a factory delegate:

1. **No arguments** → calls the parameterless constructor
2. **With arguments** → matches constructor parameters by type, falls back to parameterless if no match
3. **Abstract/interface types** → rejected with a warning

```
┌──────────────────────────────────────────────────────────────┐
│  RegisterAutoFactory(typeof(SimpleService))                  │
│  → finds parameterless constructor                           │
│  → factory: args => new SimpleService()                      │
│                                                              │
│  RegisterAutoFactory(typeof(ConfiguredService))              │
│  → finds constructor (string name)                           │
│  → factory: args => new ConfiguredService((string)args[0])   │
└──────────────────────────────────────────────────────────────┘
```

## Registration & Creation Flow

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Get IObjectFactory from ServiceLocator                   │
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

## This Demo

> **⚠️ Note:** `SimpleService` and `ConfiguredService` are **custom types defined solely for this demo**. They are not related to any system service interfaces.

| Class | Role |
|-------|------|
| `SimpleService` | Demo type with parameterless constructor |
| `ConfiguredService` | Demo type with parameterized constructor `(string name)` |
| `ObjectFactoryUsagePlugin` | `IPlugin` implementation — registers factories and creates instances |

## Security Note

`IObjectFactory` is part of the controlled-access security model. Plugins must **not** use `Activator.CreateInstance` to create objects — they must register factories and use `CreateInstance` instead. See the [Security Documentation](../../docs/en/security.md) for details.
