# PluginDemo-14: Capability.FileIO — Declarative File I/O Permission

## Overview

This plugin demonstrates how to use `[PluginCapability(Capability.FileIO)]` to declare that a plugin requires direct file system access. By declaring this capability, the plugin gains access to all `System.IO` types beyond the built-in `SystemIOAllowedTypes` whitelist.

## PluginCapability Declaration Syntax

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## How Capability.FileIO Works

1. **Default state**: `System.IO` namespace is blanket-banned; only `SystemIOAllowedTypes` whitelist types are allowed (MemoryStream, BinaryReader, GZipStream, etc.)
2. **With declaration**: The entire `System.IO` namespace ban is lifted — File, FileStream, Directory, StreamReader(string), etc. all become accessible
3. **ILString exemption**: String constants starting with `"System.IO."` are not flagged in #US heap scanning
4. **Undeclarable limits**: P/Invoke, Unsafe, Reflection.Emit, etc. remain blocked

## Capability.FileIO Exemption Scope

### TypeRef Exemptions

All `System.IO` types are exempted from the namespace ban:

| Category | Exempted Types |
|----------|---------------|
| File operations | `File`, `FileInfo` |
| Directory operations | `Directory`, `DirectoryInfo` |
| Stream types | `FileStream`, `StreamReader(path)`, `StreamWriter(path)` |
| File system | `FileSystemWatcher`, `DriveInfo`, `Path` |

### ILString Exemptions

- Strings starting with `"System.IO."` are not flagged

### What Remains Forbidden

| Category | Still Blocked |
|----------|-------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` |
| Unsafe code | `UnverifiableCodeAttribute`, `Unsafe` |
| IL Emission | `System.Reflection.Emit.*` |
| Assembly Loading | `System.Runtime.Loader`, `Assembly.Load*` |
| Registry | `Microsoft.Win32.*` |

## Comparison with Other Examples

| Example | Declaration | File Access | Notes |
|---------|------------|-------------|-------|
| **04-SafeSystemIO** | None | MemoryStream, BinaryReader, GZipStream only | Uses only whitelist types |
| **07-ForbiddenFileIO** | None | ❌ REJECTED | Shows what NOT to do |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ Full System.IO access | This example |
| **20-SpeedyPack** | None | Via SpeedyPack API (no Capability needed) | Recommended for data storage |

## Priority Order for File Access

1. **SpeedyPack** — No capability needed. Built-in caching, WAL, transactions. **Recommended for structured data storage.**
2. **PermissionedStreamFactory** — No capability needed. Audited access with path validation and access control.
3. **Capability.FileIO + direct System.IO** — Only when above options don't suffice.

## Why Prefer PermissionedStreamFactory / SpeedyPack?

Even with `Capability.FileIO`, using controlled entry points is recommended because:

1. **Audit trail**: All access is logged and traceable
2. **Path validation**: Prevents directory traversal attacks (`../`)
3. **Access control**: Workspace boundary enforcement
4. **Resource tracking**: Prevents stream leaks and resource exhaustion
5. **Compliance**: Easier to pass security reviews with controlled access patterns

## Security Best Practices

1. **Only declare FileIO when truly necessary**: Can you use SpeedyPack or PermissionedStreamFactory instead?
2. **Provide a clear Reason**: "Direct log file access for audit trail" is better than "file access"
3. **Validate paths yourself**: Even with Capability.FileIO, validate all file paths before use
4. **Use using statements**: Always dispose FileStream/StreamReader/StreamWriter
5. **Consider the principle of least privilege**: Declare only the capabilities your plugin actually needs

## Files

- `Plugin.cs` — Demo plugin declaring Capability.FileIO
- `README.md` — This file (English)
- `README.zh-CN.md` — 简体中文
- Translations: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Related Examples

- **04-SafeSystemIO**: Allowed in-memory System.IO types (no declaration needed)
- **07-ForbiddenFileIO**: Anti-pattern showing blocked file operations
- **20-SpeedyPack**: Recommended data storage without any capability declaration
- **18-CapabilityDenied**: Undeclarable capability anti-pattern
