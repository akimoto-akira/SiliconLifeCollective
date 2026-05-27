# PluginDemo-10: Forbidden Reflection Anti-Pattern

## Overview

This plugin demonstrates **FORBIDDEN** reflection operations in the SiliconLife plugin system. It serves as an anti-pattern reference, showing what NOT to do and providing correct alternatives for each violation.

## Why is Reflection the Core Threat?

Reflection bypass is the **most critical threat** to PluginLoader's security scanning. While TypeRef scanning catches direct type references at compile time, reflection methods can resolve types at **runtime** using strings — completely invisible to static metadata scanning.

If a plugin can call `Type.GetType("System.IO.File, System.Runtime")`, it can access ANY forbidden type without ever referencing it in the PE metadata TypeRef table.

## What Methods are Forbidden?

All forbidden methods are detected via **MemberRef scanning** (not namespace or type-level blocking):

| Forbidden Method | Signature | Threat |
|------------------|-----------|--------|
| `Type.GetType` | `System.Type::GetType(System.String)` | Resolve any type by name at runtime |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | Instantiate arbitrary types |
| `Activator.CreateInstanceFrom` | `System.Activator::CreateInstanceFrom(...)` | Create from DLL path |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | Load assembly by name/bytes |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | Load assembly from disk |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | Load assembly from path |
| `Assembly.UnsafeLoadFrom` | `System.Reflection.Assembly::UnsafeLoadFrom(...)` | Load without security checks |
| `Assembly.LoadWithPartialName` | `System.Reflection.Assembly::LoadWithPartialName(...)` | Load by partial name |
| `Assembly.ReflectionOnlyLoad` | `System.Reflection.Assembly::ReflectionOnlyLoad(...)` | Inspection loading |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | String-based type resolution |

## What is Safe?

Not all reflection is forbidden. The following patterns are **SAFE** because they reference compile-time known types:

| Safe Pattern | Example | Why Safe |
|--------------|---------|----------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | Type is compile-time known, visible in TypeRef |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | Inspecting known type, no new types introduced |
| `typeof(X).GetMethods()` | `typeof(IPlugin).GetMethods()` | Member inspection on known types |
| Generic constraints | `FindSubtypesOf(typeof(BaseTool))` | Generic parameter is compile-time type |
| `nameof()` | `nameof(MyClass.MyMethod)` | Compile-time string, no runtime resolution |

**Key Distinction:**
- `typeof(X).Assembly` → **SAFE** (compile-time reference, scanned by PluginLoader)
- `Assembly.Load("X")` → **FORBIDDEN** (runtime string, bypasses all scanning)

## How to Safely Replace Reflection?

### Use ITypeRegistry (Replaces Type.GetType + AppDomain scanning)

```csharp
// ❌ FORBIDDEN: Resolve type by string at runtime
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ CORRECT: Use ITypeRegistry to find registered types
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// Only types registered during OnLoad are discoverable
```

**ITypeRegistry provides:**
1. **FindType(string)**: Find a type by full name (only from registered types)
2. **FindSubtypesOf(Type)**: Find all non-abstract subtypes of a base class
3. **FindImplementationsOf(Type)**: Find all implementations of an interface
4. **RegisterType(Type)**: Register a single type during OnLoad
5. **RegisterFromAssembly(Assembly, Type)**: Register all subtypes from an assembly

### Use IObjectFactory (Replaces Activator.CreateInstance)

```csharp
// ❌ FORBIDDEN: Create arbitrary instance
object? instance = Activator.CreateInstance(someType);

// ✅ CORRECT: Use IObjectFactory with registered factory
var instance = objectFactory.CreateInstance<MyService>();
// Only types with registered factories can be instantiated
```

**IObjectFactory provides:**
1. **RegisterAutoFactory(Type)**: Auto-register factory by analyzing constructors
2. **RegisterFactory&lt;T&gt;(Func)**: Register custom factory delegate
3. **CreateInstance(Type, args)**: Create instance using registered factory
4. **CreateInstance&lt;T&gt;(args)**: Generic version of CreateInstance
5. **IsRegistered(Type)**: Check if a factory exists for a type

## Violations Demonstrated

This plugin shows 5 common reflection violations:

### Violation 1: Type.GetType(string)

```csharp
// ❌ FORBIDDEN
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
var method = fileType?.GetMethod("ReadAllText");
method?.Invoke(null, new object[] { "secret.txt" });

// ✅ CORRECT
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**Blocked MemberRef**: `System.Type::GetType(System.String)`

### Violation 2: Activator.CreateInstance

```csharp
// ❌ FORBIDDEN
Type? httpClientType = Type.GetType("System.Net.Http.HttpClient, System.Net.Http");
object? client = Activator.CreateInstance(httpClientType!);

// ✅ CORRECT
var instance = objectFactory.CreateInstance<MyService>();
```

**Blocked MemberRef**: `System.Activator::CreateInstance`

### Violation 3: Assembly.Load

```csharp
// ❌ FORBIDDEN
Assembly asm = Assembly.Load("System.Net.Http");
Type? httpType = asm.GetType("System.Net.Http.HttpClient");
object? client = Activator.CreateInstance(httpType!);

// ✅ CORRECT
Assembly myAsm = typeof(MyPlugin).Assembly;  // Safe: compile-time known
Type? type = typeRegistry.FindType("MyPlugin.SomeType");
```

**Blocked MemberRef**: `System.Reflection.Assembly::Load(System.String)`

### Violation 4: Assembly.LoadFile / LoadFrom

```csharp
// ❌ FORBIDDEN
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");
Assembly asm2 = Assembly.LoadFrom(@"\\network\share\trojan.dll");

// ✅ CORRECT
// All dependencies must be in the plugin directory and scanned by PluginLoader.
// Use ITypeRegistry.RegisterFromAssembly in OnLoad for your own assembly.
```

**Blocked MemberRef**: `System.Reflection.Assembly::LoadFile(System.String)` / `LoadFrom(System.String)`

### Violation 5: Assembly.GetType(string)

```csharp
// ❌ FORBIDDEN
Assembly runtime = typeof(object).Assembly;
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ CORRECT
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
// Forbidden types are never registered, so they can never be found
```

**Blocked MemberRef**: `System.Reflection.Assembly::GetType(System.String)`

## PluginLoader Security Mechanism

When PluginLoader scans this plugin:

1. **MemberRef Scanning**: Detects calls to forbidden methods (`Type.GetType`, `Activator.CreateInstance`, `Assembly.Load`, etc.)
2. **TypeRef Scanning**: Detects direct references to forbidden types (supplementary check)
3. **IL String Scanning**: Detects string constants that match forbidden type patterns (defense in depth)
4. **Rejection**: Plugin is rejected during loading with detailed error message listing all violations

**Multi-layer defense:**
- **Layer 1 (TypeRef)**: Catches direct type references in metadata
- **Layer 3 (MemberRef)**: Catches calls to forbidden methods (this demo's focus)
- **Layer 5 (#US Heap)**: Catches string constants used for runtime type resolution

## Why typeof(X).Assembly is Safe but Assembly.Load is Not

| Operation | Visibility | Security |
|-----------|-----------|----------|
| `typeof(X).Assembly` | Type X is in TypeRef table → PluginLoader scans it | ✅ Safe |
| `Assembly.Load("X")` | String "X" only exists at runtime → invisible to TypeRef scan | ❌ Forbidden |
| `obj.GetType()` | Returns type of existing instance → no new type introduced | ✅ Safe |
| `Type.GetType("X")` | Resolves arbitrary type from string → bypasses TypeRef | ❌ Forbidden |

## Comparison with Other Examples

| Example | Focus | Safe Alternative |
|---------|-------|------------------|
| **10-ForbiddenReflection** | Forbidden reflection patterns (this example) | ITypeRegistry + IObjectFactory |
| **11-ForbiddenPInvoke** | Forbidden P/Invoke and unsafe code | Hard prohibition (no alternative) |
| **12-ForbiddenStringBypass** | String-based reflection bypass attempts | N/A (demonstrates why strings are scanned) |
| **02-TypeRegistryUsage** | ITypeRegistry correct usage | — |
| **03-ObjectFactoryUsage** | IObjectFactory correct usage | — |

## Best Practices

1. **Register types in OnLoad**: Use `ITypeRegistry.RegisterType` / `RegisterFromAssembly`
2. **Use IObjectFactory for dynamic creation**: Never use `Activator.CreateInstance`
3. **typeof(X).Assembly is your friend**: Safe way to reference your own assembly
4. **Avoid string-based type names**: They trigger IL string scanning and may be flagged
5. **Design for static discoverability**: If PluginLoader can't see it in metadata, it's suspicious

## Files

- `Plugin.cs` - Anti-pattern demonstration plugin
- `README.md` - This file (English)
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Related Examples

- **02-TypeRegistryUsage**: ITypeRegistry correct usage
- **03-ObjectFactoryUsage**: IObjectFactory correct usage
- **11-ForbiddenPInvoke**: Forbidden P/Invoke and unsafe code
- **12-ForbiddenStringBypass**: String-based reflection bypass attempts
