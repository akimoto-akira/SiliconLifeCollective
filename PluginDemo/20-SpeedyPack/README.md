# PluginDemo-20: SpeedyPack — Structured Data Storage

## Overview

This plugin demonstrates using `SpeedyPack` for structured data storage **without any capability declaration**. SpeedyPack is the **recommended** way for plugins to persist data.

## Why SpeedyPack?

| Feature | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|---------|-----------|--------------------------|------------------------------|
| Capability needed | **None** | None | `Capability.FileIO` |
| Caching | ✅ Built-in | ❌ | ❌ |
| WAL (crash recovery) | ✅ | ❌ | ❌ |
| Transactions | ✅ `IPackTransaction` | ❌ | ❌ |
| Thread-safe | ✅ | ❌ | ❌ |
| Structured serialization | ✅ `Read<T>` | ❌ Raw bytes | ❌ Manual |
| Audit trail | ✅ Automatic | ✅ Automatic | ❌ Manual |

## Basic CRUD

```csharp
// Open a SpeedyPack data file
using var pack = SpeedyPack.Open("mydata.spk");

// Write key-value pairs
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// Read values (typed)
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// Delete a key
pack.Delete("user:age");

// Check existence
bool exists = pack.Contains("user:name");  // true
```

## Typed Access with Structured Objects

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// Write structured object
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// Read typed object
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## Transactions

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // atomic — both writes persist or neither
    }
    catch
    {
        tx.Rollback();  // discard all writes in this transaction
    }
}
```

### IPackTransaction Methods

| Method | Description |
|--------|-------------|
| `Write(key, value)` | Queue a write operation |
| `Delete(key)` | Queue a delete operation |
| `Commit()` | Atomically apply all queued operations |
| `Rollback()` | Discard all queued operations |

## Configuration with SpeedyPackOptions

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 MB cache
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### SpeedyPackOptions Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `MaxCacheSize` | `long` | 64 MB | Maximum in-memory cache size |
| `AutoFlushInterval` | `TimeSpan` | 10 seconds | How often to flush cache to disk |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | Compression level for stored data |

## Security Note

SpeedyPack requires **no** capability declaration. It is a safe, controlled entry point for data storage that:
- Validates all paths against workspace boundaries
- Provides full audit trail of all read/write operations
- Prevents directory traversal attacks
- Manages resource lifecycle automatically

## Related Examples

- **04-SafeSystemIO**: Allowed in-memory System.IO types (no declaration needed)
- **07-ForbiddenFileIO**: Anti-pattern showing blocked file operations
- **14-CapabilityFileIO**: When SpeedyPack is not sufficient
