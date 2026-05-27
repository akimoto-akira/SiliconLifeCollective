# Safe System.IO Demo

Demonstrates System.IO types on the `SystemIOAllowedTypes` whitelist: `MemoryStream`, `BinaryReader`/`BinaryWriter`, `GZipStream`. Shows why `FileStream` requires `PermissionedStreamFactory`.

## SystemIOAllowedTypes Whitelist

The plugin runtime blocks the `System.IO` namespace by default, but exempts types that **do not directly perform file I/O**:

| Category | Allowed Types | Why Safe |
|----------|---------------|----------|
| Stream abstractions | `Stream` | Abstract base class — no I/O by itself |
| In-memory streams | `MemoryStream` | Pure in-memory operation |
| Compression streams | `GZipStream`, `DeflateStream`, `ZLibStream` | Wrap another stream, don't open files |
| Binary wrappers | `BinaryReader`, `BinaryWriter` | Wrap any stream, don't open files |
| Enums | `SeekOrigin`, `FileMode`, `FileAccess`, `FileShare`, `CompressionMode`, `CompressionLevel` | Value types only |
| Exceptions | `IOException`, `InvalidDataException`, `EndOfStreamException` | Error types only |

### Types NOT on the Whitelist

These types **directly access the file system** and are **blocked** in plugin code:

| Blocked Type | Why Blocked | Safe Alternative |
|-------------|-------------|-----------------|
| `FileStream` | Opens files directly | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | Static file operations | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | Static directory operations | `SafePath` (permission-checked) |
| `FileInfo` | Wraps file paths | `SafePath` |
| `DirectoryInfo` | Wraps directory paths | `SafePath` |
| `StreamReader` | Opens files directly | `PermissionedStreamFactory` + wrap `PermissionedStream` |
| `StreamWriter` | Opens files directly | `PermissionedStreamFactory` + wrap `PermissionedStream` |

## Why PermissionedStreamFactory for FileStream

`FileStream` directly opens files on disk — a major security risk in a plugin system. `PermissionedStreamFactory` enforces:

1. **Permission check** — the caller's `PermissionManager` must grant `FileAccess` for the path
2. **Audit logging** — every file open is logged with the caller's being ID
3. **Path validation** — empty/invalid paths are rejected before any I/O

```
❌ new FileStream("path", FileMode.Open)           → BLOCKED by TypeRef scanner
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → Permission-checked
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → Permission-checked
```

## Demo Pipeline

This demo builds a complete in-memory data pipeline using only whitelist types:

```
┌─────────────────────────────────────────────────────────────────┐
│  Demo 1: MemoryStream                                           │
│  └─ Write bytes → Read bytes → Decode string                    │
│                                                                  │
│  Demo 2: Compression Pipeline                                    │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(compress) → MemoryStream(compressed)            │
│     → GZipStream(decompress) → MemoryStream(raw)                 │
│     → UTF8 → string (roundtrip)                                  │
│                                                                  │
│  Demo 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → Verify roundtrip               │
└─────────────────────────────────────────────────────────────────┘
```

## This Demo

> **⚠️ Note:** This demo uses **only** `SystemIOAllowedTypes` whitelist types. No file I/O is performed. For file access, see the `PermissionedStreamFactory` API.

| Class | Role |
|-------|------|
| `SafeSystemIOPlugin` | `IPlugin` implementation — demonstrates safe System.IO usage |

## Security Note

The `System.IO` namespace is blocked by the plugin TypeRef scanner. Only whitelist types pass. For actual file access, plugins must use `PermissionedStreamFactory` which performs permission checks and audit logging. See the [Security Documentation](../../docs/en/security.md) for details.
