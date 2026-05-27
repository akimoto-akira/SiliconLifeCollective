# PluginDemo-07: Forbidden File I/O Anti-Pattern

## Overview

This plugin demonstrates **FORBIDDEN** file I/O operations in the SiliconLife plugin system. It serves as an anti-pattern reference, showing what NOT to do and providing correct alternatives for each violation.

## Why is System.IO Globally Banned?

The entire `System.IO` namespace is blocked at the plugin level because direct file access poses severe security risks:

1. **Unauthorized File Access**: Plugins could read sensitive files outside the workspace (passwords, keys, personal data)
2. **File Overwrite Attacks**: Malicious plugins could overwrite critical system or configuration files
3. **Directory Traversal**: Plugins could escape workspace boundaries using `../` paths
4. **Resource Exhaustion**: Uncontrolled file creation could fill disk space
5. **No Audit Trail**: Direct file operations bypass the plugin security audit system

## What Types are Forbidden?

All `System.IO` types that directly access the file system are blocked:

| Forbidden Type | Blocked Method | Risk Level |
|----------------|----------------|------------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText`, etc. | 🔴 Critical |
| `FileStream` | Constructor with file path | 🔴 Critical |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 Critical |
| `StreamReader` | Constructor with file path (string) | 🔴 Critical |
| `StreamWriter` | Constructor with file path (string) | 🔴 Critical |
| `FileInfo` | All methods | 🔴 Critical |
| `DirectoryInfo` | All methods | 🔴 Critical |

## What Types are Allowed?

Types that perform **pure in-memory operations** (no direct file system access) are allowed:

| Allowed Type | Usage | Why Safe |
|--------------|-------|----------|
| `MemoryStream` | In-memory byte stream | No file system access |
| `BinaryReader` | Read from existing stream | Wraps stream, doesn't open files |
| `BinaryWriter` | Write to existing stream | Wraps stream, doesn't create files |
| `GZipStream` | Compression/decompression | Wraps stream, no file access |
| `StreamReader` | Constructor with `Stream` (not string) | Safe when wrapping audited stream |
| `StreamWriter` | Constructor with `Stream` (not string) | Safe when wrapping audited stream |

See **04-SafeSystemIO** for examples of allowed types.

## How to Access Files Safely?

### Option 1: PermissionedStreamFactory (Recommended for File Access)

`PermissionedStreamFactory` is the **controlled entry point** for file operations in plugins:

```csharp
// ✅ CORRECT: Read file
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ CORRECT: Write file
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("Log entry");
```

**What PermissionedStreamFactory Provides:**
1. **Path Validation**: Prevents directory traversal attacks (`../`)
2. **Permission Checking**: Ensures file is within allowed workspace
3. **Audit Logging**: All file access is recorded for security review
4. **Resource Cleanup**: Tracks open streams and prevents leaks

### Option 2: SpeedyPack (Recommended for Structured Data)

For structured data storage, prefer `SpeedyPack` over direct file access:

```csharp
// ✅ CORRECT: Use SpeedyPack for data storage
using var pack = SpeedyPack.Open("data.spk");

// Write data
pack.WriteText("config.json", "{\"key\": \"value\"}");
pack.Write("settings.dat", myObject);

// Read data
string config = pack.ReadText("config.json");
var settings = pack.Read<Settings>("settings.dat");

// List entries
var entries = pack.ListEntries("/");
```

**Advantages of SpeedyPack:**
- ✅ No `Capability.FileIO` declaration needed
- ✅ WAL (Write-Ahead Logging) for crash consistency
- ✅ Built-in caching and transaction support
- ✅ Atomic operations (commit/rollback)
- ✅ Full audit trail

## Violations Demonstrated

This plugin shows 5 common file I/O violations:

### Violation 1: File.ReadAllText

```csharp
// ❌ FORBIDDEN
string content = File.ReadAllText("config.json");

// ✅ CORRECT
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

**Blocked TypeRef**: `System.IO.File::ReadAllText(System.String)`

### Violation 2: File.WriteAllText

```csharp
// ❌ FORBIDDEN
File.WriteAllText("output.log", "some data");

// ✅ CORRECT
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

**Blocked TypeRef**: `System.IO.File::WriteAllText(System.String, System.String)`

### Violation 3: Direct FileStream

```csharp
// ❌ FORBIDDEN
using var fs = new FileStream("data.bin", FileMode.Open);
fs.Read(buffer, 0, buffer.Length);

// ✅ CORRECT
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
fs.Read(buffer, 0, buffer.Length);
```

**Blocked TypeRef**: `System.IO.FileStream::.ctor(System.String, System.IO.FileMode)`

### Violation 4: Directory.GetFiles

```csharp
// ❌ FORBIDDEN
string[] files = Directory.GetFiles("./logs", "*.txt");
foreach (var file in files) { ... }

// ✅ CORRECT (using SpeedyPack)
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

**Blocked TypeRef**: `System.IO.Directory::GetFiles(System.String)`

### Violation 5: StreamReader with Direct Path

```csharp
// ❌ FORBIDDEN
using var reader = new StreamReader("config.json");
string line = reader.ReadLine();

// ✅ CORRECT
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string line = reader.ReadLine();
```

**Blocked TypeRef**: `System.IO.StreamReader::.ctor(System.String)`

## Comparison with Other Examples

| Example | Focus | Permission Required |
|---------|-------|---------------------|
| **04-SafeSystemIO** | Allowed in-memory types (MemoryStream, GZipStream) | None |
| **07-ForbiddenFileIO** | Forbidden file access patterns (this example) | N/A (blocked) |
| **14-CapabilityFileIO** | Declaring FileIO capability to bypass restrictions | `Capability.FileIO` |

**Key Difference:**
- **04-SafeSystemIO**: Shows what you CAN do without any special permissions (in-memory only)
- **07-ForbiddenFileIO**: Shows what you CANNOT do (direct file access)
- **14-CapabilityFileIO**: Shows how to DECLARATIVELY request file access permission

## PluginLoader Security Mechanism

When PluginLoader scans this plugin:

1. **TypeRef Scanning**: Detects references to forbidden `System.IO` types
2. **MemberRef Scanning**: Detects calls to blocked methods (e.g., `File.ReadAllText`)
3. **IL String Scanning**: Detects string-based reflection attempts to load forbidden types
4. **Rejection**: Plugin is rejected during loading with detailed error message

**Cannot be bypassed by:**
- String concatenation (`"System.IO" + ".File"`)
- Reflection (`Type.GetType("System.IO.File")`)
- Dynamic loading (`Assembly.Load`)
- Obfuscation or encryption

These bypass attempts are caught by IL-level scanning (see **12-ForbiddenStringBypass**).

## Best Practices

1. **Default to SpeedyPack**: For structured data, always use SpeedyPack
2. **Use PermissionedStreamFactory**: When you need direct file access
3. **Declare Capability if Necessary**: If you truly need unrestricted file access, declare `Capability.FileIO` (see 14-CapabilityFileIO)
4. **Avoid System.IO Directly**: Never use `File`, `Directory`, or direct `FileStream` constructors
5. **Remember**: In-memory types (MemoryStream, BinaryReader/Writer) are always safe

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

- **04-SafeSystemIO**: Allowed in-memory System.IO types
- **14-CapabilityFileIO**: Declarative FileIO capability
- **20-SpeedyPack**: Structured data storage with SpeedyPack
- **12-ForbiddenStringBypass**: String-based reflection bypass attempts
