# Trusted Dependency Demo

Demonstrates using `Newtonsoft.Json` — a library that internally relies on heavy reflection — as a trusted assembly. The PluginLoader security scanner skips trusted assemblies entirely, allowing plugins to reference them without triggering violations.

## TrustedAssemblies Whitelist Mechanism

The `PluginLoader` maintains a static whitelist of open-source libraries that are **trusted by default**:

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // Serialization
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← This demo uses this
    "MessagePack",
    "YamlDotNet",

    // Logging
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // Data access / mapping
    "Dapper", "AutoMapper",

    // Validation and message dispatch
    "FluentValidation", "MediatR",
};
```

### Admission Criteria

A library can be added to `TrustedAssemblies` if it meets **all three** criteria:

| # | Criterion | Rationale |
|---|-----------|-----------|
| 1 | Widely-used open-source project (MIT / Apache 2.0 / BSD) | Publicly auditable code |
| 2 | Publicly auditable source code | Community oversight ensures no malicious behavior |
| 3 | Maintained NuGet package from trusted vendor/community | Supply-chain integrity |

### Identification Basis

The scanner identifies trusted assemblies by their `AssemblyDefinition.Name` in PE metadata — **not the DLL filename**. This prevents attackers from renaming a malicious DLL to `Newtonsoft.Json.dll` to bypass checks.

## CollectTrustedTypeRefs — Transitive Exemption

When the PluginLoader loads a plugin directory, it performs two-phase scanning:

```
Phase 1: CollectTrustedTypeRefs(pluginDir)
├── Enumerate all *.dll files in the plugin directory
├── For each DLL: read PE metadata → check AssemblyDefinition.Name
├── If name ∈ TrustedAssemblies:
│   └── Collect ALL TypeReference entries → (namespace, typeName) pairs
└── Return: HashSet<(string Namespace, string Name)>

Phase 2: ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── Layer 0:   Whitelist early exit (if main DLL itself is trusted → pass)
├── Layer 0.5: Transitive exemption (skip TypeRefs in trustedTypeRefs set)
├── Layer 1:   TypeRef table scan
├── Layer 2:   ExportedType table scan
├── Layer 3:   MemberRef table scan (dangerous methods)
├── Layer 4:   Unsafe code markers + P/Invoke
└── Layer 5:   #US string heap scan
```

### Why Transitive Exemption Matters

Newtonsoft.Json internally references types like `System.Reflection.MemberInfo`, `System.IO.TextReader`, etc. When your plugin references Newtonsoft.Json, the compiler may embed these transitive TypeRefs into **your** plugin DLL. Without transitive exemption, your plugin would be flagged for referencing `System.IO.TextReader` — even though you never use it directly.

`CollectTrustedTypeRefs` solves this by pre-collecting all TypeRefs from trusted DLLs and marking them as "known safe" during the main scan.

## How to Add a New Trusted Dependency

To add a new library to the whitelist:

1. Verify it meets the three admission criteria above
2. Add a single line to the `TrustedAssemblies` HashSet in `PluginLoader.cs`:
   ```csharp
   "YourLibraryName",  // Brief description of why it's trusted
   ```
3. Place the library DLL in your plugin directory alongside your plugin DLL
4. The scanner will automatically collect its TypeRefs and exempt them

> **⚠️ Important:** Adding a library to `TrustedAssemblies` means the scanner will **not** check its internal code at all. Only add libraries you fully trust.

## This Demo

This plugin uses Newtonsoft.Json without any `PluginCapability` declaration:

| Feature | Newtonsoft.Json Internals | Why It Works |
|---------|--------------------------|--------------|
| `JsonConvert.SerializeObject` | Uses reflection to enumerate properties | Newtonsoft.Json DLL passes Layer 0 whitelist |
| `JsonConvert.DeserializeObject<T>` | Calls `Activator.CreateInstance`, sets properties via reflection | Transitive TypeRefs exempted at Layer 0.5 |
| `JObject` / `JArray` manipulation | Uses `System.Linq.Expressions`, dynamic dispatch | All internal refs collected by `CollectTrustedTypeRefs` |

### Key Difference from PluginCapability

| Mechanism | Scope | Use Case |
|-----------|-------|----------|
| `TrustedAssemblies` | Exempts an entire **library** (and its transitive refs) from scanning | Well-known open-source dependencies |
| `PluginCapability` | Exempts your **plugin code** from specific namespace bans | Plugin needs direct access to System.Net/IO/Process |

A plugin using only trusted dependencies needs **no** `PluginCapability` declaration. The scanner handles everything automatically.

## Security Note

Trusted assemblies are exempted from security scanning because they are auditable open-source projects. However, **your plugin code** is still fully scanned. If your plugin directly references `System.IO.File` or `System.Net.Http.HttpClient`, it will still be blocked — unless you declare the appropriate `PluginCapability`. See the [Security Documentation](../../docs/en/security.md) for details.
