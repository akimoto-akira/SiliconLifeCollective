# PluginDemo-11: Forbidden P/Invoke and Unsafe Code Anti-Pattern

## Overview

This plugin demonstrates **FORBIDDEN** P/Invoke and unsafe code operations in the SiliconLife plugin system. Unlike other forbidden categories (file I/O, network, process, reflection) that have safe wrapper alternatives, P/Invoke and unsafe code are **hard prohibitions** with no safe alternative — they cannot be exempted by any `PluginCapability` declaration.

## Why is P/Invoke the Ultimate Threat?

P/Invoke and unsafe code represent the **most fundamental threat** to plugin security because they operate **outside the managed runtime entirely**:

- Native code executes with full process privileges
- No managed type safety, memory safety, or garbage collection
- Impossible to intercept, audit, or sandbox native calls
- A crash in native code crashes the entire process (no exception handling)
- Can access any memory address in the process space

## The Triple Insurance Mechanism

PluginLoader uses **three independent detection layers** to ensure P/Invoke and unsafe code can never slip through:

### Layer 1: TypeRef Table Scanning

Detects direct references to forbidden types in PE metadata:

| Forbidden Type | Namespace | Threat |
|----------------|-----------|--------|
| `DllImportAttribute` | System.Runtime.InteropServices | Declares native function import |
| `UnmanagedFunctionPointerAttribute` | System.Runtime.InteropServices | Native function pointer |
| `SuppressGCTransitionAttribute` | System.Runtime.InteropServices | Skip GC transition |
| `Marshal` | System.Runtime.InteropServices | Managed/unmanaged memory bridge |
| `MemoryMarshal` | System.Runtime.InteropServices | Span and raw memory interchange |
| `NativeMemory` | System.Runtime.InteropServices | Native heap malloc/free |
| `NativeLibrary` | System.Runtime.InteropServices | Dynamic native library loading |
| `GCHandle` | System.Runtime.InteropServices | Pin managed object, expose pointer |
| `SafeHandle` | System.Runtime.InteropServices | Native resource handle base |
| `Unsafe` | System.Runtime.CompilerServices | Unsafe helper class |
| `UnverifiableCodeAttribute` | System.Security | Unverifiable code marker |
| `SuppressUnmanagedCodeSecurityAttribute` | System.Security | Suppress security check |

### Layer 2: Unsafe Marker Scanning (ScanUnsafeMarkers)

Detects compiler-generated markers independent of type references:

| Marker | Detection Method | Source |
|--------|-----------------|--------|
| `[assembly: UnverifiableCode]` | Assembly CustomAttribute table | C# `unsafe` keyword |
| `[module: UnverifiableCode]` | Module CustomAttribute table | C# `unsafe` keyword |
| `MethodAttributes.PinvokeImpl` | MethodDef table flags | `[DllImport]` attribute |

### Layer 3: IL String Scanning (#US Heap)

Catches string constants that reference InteropServices types:

```
"System.Runtime.InteropServices.Marshal"  → Flagged
"System.Runtime.InteropServices.*"        → Flagged by prefix match
```

## Why Three Layers?

Each layer catches what others might miss:

| Bypass Attempt | Layer 1 | Layer 2 | Layer 3 |
|----------------|---------|---------|---------|
| Normal `[DllImport]` usage | ✅ Catches DllImportAttribute TypeRef | ✅ Catches PinvokeImpl flag | — |
| Obfuscated attribute name | ❌ Might miss | ✅ PinvokeImpl is a raw flag, can't be hidden | — |
| Type.GetType("...Marshal...") | ❌ No direct TypeRef | — | ✅ String scan catches it |
| unsafe block without P/Invoke | — | ✅ UnverifiableCode attribute | — |

**Result: No single bypass defeats all three layers.**

## Violations Demonstrated

### Violation 1: [DllImport] Declaration

```csharp
// ❌ FORBIDDEN
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();

[DllImport("user32.dll", CharSet = CharSet.Unicode)]
private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
```

**Caught by:**
- `[TypeRef] System.Runtime.InteropServices.DllImportAttribute`
- `[PInvoke] GetTickCount64 (native interop)` (PinvokeImpl flag)

### Violation 2: Marshal Usage

```csharp
// ❌ FORBIDDEN
IntPtr ptr = Marshal.AllocHGlobal(1024);
Marshal.WriteByte(ptr, 0xFF);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.StructureToPtr(data, ptr, false);
Marshal.FreeHGlobal(ptr);
```

**Caught by:** `[TypeRef] System.Runtime.InteropServices.Marshal`

### Violation 3: NativeMemory Usage

```csharp
// ❌ FORBIDDEN
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Clear(buffer, 4096);
    buffer = NativeMemory.Realloc(buffer, 8192);
    NativeMemory.Free(buffer);
}
```

**Caught by:**
- `[TypeRef] System.Runtime.InteropServices.NativeMemory`
- `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### Violation 4: GCHandle Pinning

```csharp
// ❌ FORBIDDEN
byte[] managedArray = new byte[1024];
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
// Pass raw pointer to native code...
handle.Free();
```

**Caught by:** `[TypeRef] System.Runtime.InteropServices.GCHandle`

### Violation 5: unsafe Block

```csharp
// ❌ FORBIDDEN
unsafe
{
    int value = 42;
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
    int* next = ptr + 1;  // Pointer arithmetic!
}
```

**Caught by:** `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### Violation 6: NativeLibrary Loading

```csharp
// ❌ FORBIDDEN
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
var func = Marshal.GetDelegateForFunctionPointer<Action>(funcPtr);
func();  // Execute arbitrary native code!
NativeLibrary.Free(lib);
```

**Caught by:** `[TypeRef] System.Runtime.InteropServices.NativeLibrary`

## No Safe Alternative — Comparison

| Forbidden Category | Safe Wrapper | Auditable | Can be declared via PluginCapability |
|-------------------|--------------|-----------|--------------------------------------|
| File I/O | PermissionedStreamFactory | ✅ Yes | ✅ Capability.FileIO |
| Network | NetworkExecutor | ✅ Yes | ✅ Capability.Network |
| Process | CommandLineExecutor | ✅ Yes | ✅ Capability.Process |
| Reflection | ITypeRegistry + IObjectFactory | ✅ Yes | ❌ Always forbidden |
| **P/Invoke & unsafe** | **❌ NONE** | **❌ Impossible** | **❌ Always forbidden** |

## What If a Plugin Genuinely Needs Native Code?

If a library legitimately uses P/Invoke or unsafe code (e.g., Google.Protobuf uses `MemoryMarshal` for zero-copy serialization), it must be:

1. **Audited** by the project maintainer
2. **Added to `TrustedAssemblies`** whitelist in `PluginLoader`
3. **Identified by PE metadata `AssemblyDefinition.Name`** (not filename — prevents renaming attacks)

Example trusted libraries:
- `Google.Protobuf` (uses MemoryMarshal/Unsafe for performance)
- `Newtonsoft.Json` (uses heavy reflection)
- `MessagePack` (uses Unsafe for zero-copy)

## Comparison with Other Examples

| Example | Focus | Safe Alternative |
|---------|-------|------------------|
| **07-ForbiddenFileIO** | File I/O operations | PermissionedStreamFactory |
| **08-ForbiddenNetwork** | Network operations | NetworkExecutor |
| **09-ForbiddenProcess** | Process operations | CommandLineExecutor |
| **10-ForbiddenReflection** | Reflection operations | ITypeRegistry + IObjectFactory |
| **11-ForbiddenPInvoke** | P/Invoke & unsafe (this example) | ❌ None (hard prohibition) |
| **12-ForbiddenStringBypass** | String-based bypass attempts | N/A |

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

- **04-SafeSystemIO**: System.IO whitelist safe types
- **06-TrustedDependency**: TrustedAssemblies whitelist mechanism
- **10-ForbiddenReflection**: Forbidden reflection patterns
- **12-ForbiddenStringBypass**: String-based reflection bypass attempts
