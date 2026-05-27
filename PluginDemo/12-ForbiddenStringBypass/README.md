# PluginDemo-12: Forbidden String Bypass Anti-Pattern

## Overview

This plugin demonstrates **FORBIDDEN** string-based reflection bypass attempts in the SiliconLife plugin system. It shows why string concatenation, interpolation, encoding, and other obfuscation techniques **cannot** bypass PluginLoader's #US (User String) heap scanning — the **last line of defense**.

## What is the #US Heap?

In .NET PE (Portable Executable) metadata, the **#US (User String) heap** stores all string literal operands used by `ldstr` IL instructions. Every time you write a string literal in C# code, the compiler stores it in this heap.

```
C# source:  string s = "System.IO.File";
    ↓ compilation
IL code:    ldstr "System.IO.File"    ← references token in #US heap
    ↓ PluginLoader scan
#US heap:   [..., "System.IO.File", ...]  ← CAUGHT by prefix matching!
```

PluginLoader's `ScanUserStrings()` method iterates **every single entry** in the #US heap, checking if any string starts with a forbidden prefix.

## Forbidden String Prefixes

The following prefixes trigger `[ILString]` violations when found in the #US heap:

| Prefix | Category |
|--------|----------|
| `System.IO.` | File system types |
| `System.Net.Http` | HTTP client |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | Raw sockets |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | Network probing |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | Process/command line |
| `Microsoft.CodeAnalysis` | Roslyn compiler |
| `System.Reflection.Emit` | IL emission |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | Legacy CodeDom |
| `Microsoft.Win32` | Windows Registry |

## Violations Demonstrated

### Violation 1: Direct Type Name String

```csharp
// ❌ FORBIDDEN — the full string is in #US heap
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**Violation**: `[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### Violation 2: String Concatenation (Compile-Time)

```csharp
// ❌ FORBIDDEN — compiler folds const+const into one #US entry
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
// Both the parts AND the folded result are in #US heap!
```

**Violation**: `[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### Violation 3: String Interpolation

```csharp
// ❌ FORBIDDEN — literal parts stored in #US heap
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
// "System.IO." alone matches the prefix!
```

**Violation**: `[ILString] "System.IO." matches forbidden prefix "System.IO."`

### Violation 4: Const Fields

```csharp
// ❌ FORBIDDEN — const values inlined at usage site → appear in #US heap
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**Violation**: `[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### Violation 5: Partial String Fragments

```csharp
// ❌ FORBIDDEN — each part is a separate ldstr, scanned independently
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
// "System.Reflection.Emit" alone matches the prefix!
```

**Violation**: `[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

### Violation 6: Multiple Forbidden Targets

```csharp
// ❌ FORBIDDEN — ALL strings in the entire assembly are scanned
string tcp = "System.Net.Sockets.TcpClient";
string registry = "Microsoft.Win32.Registry";
string loader = "System.Runtime.Loader.AssemblyLoadContext";
```

**Multiple violations** — the scanner does NOT stop at the first match.

## Why Obfuscation Techniques All Fail

| Technique | Why It Fails |
|-----------|-------------|
| Const concatenation | Compiler folds into single #US entry |
| String interpolation | Literal parts stored in #US heap |
| Const fields | Values inlined at usage → appear in #US |
| Split into variables | Each `ldstr` operand scanned independently |
| Base64 encoding | Decode needs runtime methods, BUT `Type.GetType` is MemberRef-blocked |
| Char array building | No `ldstr` emitted, BUT `Type.GetType` is still MemberRef-blocked |
| XOR encryption | Encrypted string unreadable in #US, BUT decryption + `Type.GetType` = MemberRef blocked |
| Reverse string | Reversed literal may still match prefix, AND reverse + GetType = MemberRef blocked |

**Key Insight**: The #US scan blocks the **string**. The MemberRef scan blocks the **method**. To dynamically load a type, you need BOTH. PluginLoader blocks BOTH independently.

## The Complete Defense Chain

PluginLoader's 5-step scanning creates an unbreakable defense:

| Step | Mechanism | What It Catches |
|------|-----------|-----------------|
| 1 | TypeRef Table | Direct references to forbidden types |
| 2 | ExportedType Table | Forwarded types from forbidden namespaces |
| 3 | MemberRef Table | Calls to `Type.GetType`, `Assembly.Load`, `Activator.CreateInstance` |
| 4 | Unsafe Markers | `[DllImport]`, unsafe blocks, PinvokeImpl flag |
| **5** | **#US Heap Scan** | **String constants matching forbidden prefixes (THIS DEMO)** |

Step 5 is the **last line of defense** because:
- Steps 1-4 catch **active code** (types, methods, attributes)
- Step 5 catches **passive data** (strings that COULD be used for bypass)
- Even if an unknown method is missed by MemberRef scan, the forbidden type name string MUST exist in the assembly → #US scan catches it

## Trade-offs

The #US heap scan may produce **minor false positives**:
- Log messages: `logger.Info("Failed to connect to System.Net.Http endpoint")` → flagged
- Documentation strings: comments compiled into XML docs are NOT in #US heap (safe)
- `nameof()` expressions: compile-time strings that don't match prefixes are safe

**Solution for false positives**: Rephrase string literals to avoid forbidden prefixes.

## Comparison with Other Examples

| Example | Focus | Relationship |
|---------|-------|-------------|
| **10-ForbiddenReflection** | Forbidden reflection methods (MemberRef scan) | Catches the METHOD |
| **11-ForbiddenPInvoke** | Forbidden P/Invoke and unsafe code | Different threat vector |
| **12-ForbiddenStringBypass** | String-based bypass attempts (this example) | Catches the STRING |
| **02-TypeRegistryUsage** | Safe type discovery | Correct alternative |
| **03-ObjectFactoryUsage** | Safe object creation | Correct alternative |

## Best Practices

1. **Avoid forbidden prefixes in ALL strings** — even in log messages and comments compiled to IL
2. **Use ITypeRegistry/IObjectFactory** — the safe alternative to string-based reflection
3. **Understand that splitting strings doesn't help** — each fragment is scanned independently
4. **Runtime construction (char[]) bypasses #US** — but is blocked by MemberRef scan of Type.GetType
5. **Defense-in-depth works** — even bypassing ONE layer, the others still catch you

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

- **10-ForbiddenReflection**: Forbidden reflection methods (MemberRef scan)
- **11-ForbiddenPInvoke**: Forbidden P/Invoke and unsafe code
- **02-TypeRegistryUsage**: ITypeRegistry correct usage
- **03-ObjectFactoryUsage**: IObjectFactory correct usage
