# PluginDemo-18: Capability Denied — Undeclarable Capability Anti-Pattern

## Overview

This plugin is an **anti-pattern** demonstrating that declaring a capability does NOT bypass undeclarable capability bans. Even with `[PluginCapability(Capability.Network)]`, P/Invoke, Unsafe, Reflection.Emit, and Registry access are **always** blocked.

## Declarable vs. Undeclarable Capabilities

### ✅ Declarable (Capability enum values exist)

| Capability | What it exempts |
|-----------|----------------|
| `Capability.Network` | System.Net.* namespaces and per-type bans |
| `Capability.FileIO` | System.IO namespace (beyond whitelist) |
| `Capability.Process` | Process* types under System.Diagnostics |
| `Capability.AI` | Enables IAIService injection (no TypeRef exemption) |

### ❌ Undeclarable (NO Capability enum value exists)

| Category | Blocked Types | Why Undeclarable |
|----------|-------------|-----------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory`, `NativeLibrary` | Cannot audit arbitrary native code at runtime |
| Unsafe code | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Bypasses CLR type safety and bounds checking |
| IL Emission | `System.Reflection.Emit.*` | Can generate arbitrary IL at runtime |
| Assembly Loading | `System.Runtime.Loader`, `Assembly.Load*` | Can load unchecked DLLs, bypassing security scan |
| Registry | `Microsoft.Win32.*` | OS-level system access outside plugin sandbox |
| Dynamic Compilation | `Microsoft.CodeAnalysis.*` | Can compile and execute arbitrary code |
| Dangerous Reflection | `Type.GetType(string)`, `Activator.CreateInstance` | Can instantiate forbidden types by string |

## Why These Capabilities Cannot Be Declared

The fundamental reason: **they cannot be safely audited at runtime.**

1. **P/Invoke**: Once native code is called, the CLR has no visibility into what happens — no safety guarantees
2. **Unsafe**: Bypasses the type safety system that the plugin security model depends on
3. **Reflection.Emit**: Can generate new IL at runtime that was never scanned by PluginLoader
4. **AssemblyLoadContext**: Can load DLLs that were never security-scanned
5. **Registry**: Provides access to OS-level configuration outside the plugin sandbox

## PluginLoader's "Declaration Invalid" Processing

When PluginLoader encounters a capability declaration:

1. Reads the int32 enum value from the CustomAttribute blob
2. Checks `Enum.IsDefined(typeof(Capability), value)`
3. If the value is not a defined Capability member → **silently ignored**
4. If the value is defined → exemption rules are applied
5. **Undeclarable checks are ALWAYS enforced** regardless of any declared capability

This prevents plugins from declaring "future" capabilities that don't exist yet.

## Comparison with 13-CapabilityNetwork

| Aspect | 13-CapabilityNetwork (positive) | 18-CapabilityDenied (anti-pattern) |
|--------|-------------------------------|-----------------------------------|
| Declaration | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| Uses HttpClient | ✅ Exempted | ✅ Exempted |
| Uses DllImport | N/A | ❌ ALWAYS blocked |
| Uses Unsafe | N/A | ❌ ALWAYS blocked |
| Load result | ✅ LOADED | ❌ REJECTED |

## Related Examples

- **13-CapabilityNetwork**: Positive example of Capability.Network
- **11-ForbiddenPInvoke**: P/Invoke anti-pattern (no capability can help)
- **10-ForbiddenReflection**: Reflection anti-pattern (no capability can help)
