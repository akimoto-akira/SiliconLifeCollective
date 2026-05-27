# TypeRegistry Usage Demo

Demonstrates `ITypeRegistry` registration and lookup: register custom types in `OnLoad`, discover them with `FindSubtypesOf` in `OnStart`.

## ITypeRegistry Interface Overview

`ITypeRegistry` replaces `AppDomain.CurrentDomain.GetAssemblies()` reflection scanning. Plugins explicitly register their exposed types during `IPlugin.OnLoad`, and the runtime only looks up types from the registry.

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

### Method Summary

| Method | Description |
|--------|-------------|
| `RegisterType(Type)` | Registers a single type |
| `RegisterTypes(IEnumerable<Type>)` | Registers multiple types at once |
| `RegisterFromAssembly(Assembly, Type)` | Registers all non-abstract subtypes of `baseType` from the given assembly |
| `FindType(string)` | Finds a type by its full name; supports generic type name resolution |
| `FindSubtypesOf(Type)` | Finds all non-abstract subtypes of the specified base type |
| `FindImplementationsOf(Type)` | Finds all non-abstract types implementing the specified interface |

## Registration & Query Flow

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Get ITypeRegistry from ServiceLocator                    │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  Alternative: RegisterFromAssembly                           │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → registers all DemoTool subtypes at once                │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ Iterate results → GreetingTool, FarewellTool, StatusTool │
└──────────────────────────────────────────────────────────────┘
```

## RegisterFromAssembly Usage

`RegisterFromAssembly` scans an assembly and registers all non-abstract types that are subclasses of the specified base type:

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // the assembly to scan
    typeof(DemoTool)                            // only register DemoTool subtypes
);
```

This is equivalent to calling `RegisterType` for each subtype individually, but more concise when a plugin defines many types sharing a common base class.

## This Demo

> **⚠️ Important:** `DemoTool` is a **custom type defined solely for this demo** to demonstrate `ITypeRegistry` registration and lookup. It has **nothing to do** with the system's `ITool` interface (`SiliconLife.Collective.ITool`) used for AI tool registration. The name "Tool" is coincidental — any custom class hierarchy would work the same way.

| Class | Role |
|-------|------|
| `DemoTool` | Custom abstract base class — the registration anchor (not related to `ITool`) |
| `GreetingTool` | Concrete subtype registered in `OnLoad` |
| `FarewellTool` | Concrete subtype registered in `OnLoad` |
| `StatusTool` | Concrete subtype registered in `OnLoad` |
| `TypeRegistryUsagePlugin` | `IPlugin` implementation — registers and queries types |

## Security Note

`ITypeRegistry` is part of the controlled-access security model. Plugins must **not** use `AppDomain.CurrentDomain.GetAssemblies()` or `Assembly.GetTypes()` to discover types — they must go through `ITypeRegistry` instead. See the [Security Documentation](../../docs/en/security.md) for details.
