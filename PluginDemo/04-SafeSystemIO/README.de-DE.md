# Sicheres System.IO – Demo

Demonstriert System.IO-Typen auf der `SystemIOAllowedTypes`-Whitelist: `MemoryStream`, `BinaryReader`/`BinaryWriter`, `GZipStream`. Erklärt, warum `FileStream` die `PermissionedStreamFactory` erfordert.

## SystemIOAllowedTypes-Whitelist

Die Plugin-Laufzeitumgebung blockiert standardmäßig den `System.IO`-Namespace, exemptiert jedoch Typen, die **kein direktes Datei-I/O ausführen**:

| Kategorie | Erlaubte Typen | Warum sicher |
|-----------|---------------|-------------|
| Stream-Abstraktionen | `Stream` | Abstrakte Basisklasse, kein eigenes I/O |
| In-Memory-Streams | `MemoryStream` | Reine Speicheroperation |
| Komprimierungsstreams | `GZipStream`, `DeflateStream`, `ZLibStream` | Wrappen einen anderen Stream, öffnen keine Dateien |
| Binär-Wrapper | `BinaryReader`, `BinaryWriter` | Wrappen beliebige Streams, öffnen keine Dateien |
| Enums | `SeekOrigin`, `FileMode`, `FileAccess`, `FileShare`, `CompressionMode`, `CompressionLevel` | Nur Werttypen |
| Ausnahmen | `IOException`, `InvalidDataException`, `EndOfStreamException` | Nur Fehlertypen |

### Nicht auf der Whitelist

Diese Typen **greifen direkt auf das Dateisystem zu** und werden im Plugin-Code **blockiert**:

| Blockierter Typ | Grund | Sichere Alternative |
|----------------|-------|---------------------|
| `FileStream` | Öffnet Dateien direkt | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | Statische Dateioperationen | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | Statische Verzeichnisoperationen | `SafePath` (berechtigungsgeprüft) |
| `FileInfo` | Wickelt Dateipfade ein | `SafePath` |
| `DirectoryInfo` | Wickelt Verzeichnispfade ein | `SafePath` |
| `StreamReader` | Öffnet Dateien direkt | `PermissionedStreamFactory` + `PermissionedStream` wrappen |
| `StreamWriter` | Öffnet Dateien direkt | `PermissionedStreamFactory` + `PermissionedStream` wrappen |

## Warum PermissionedStreamFactory für FileStream

`FileStream` öffnet direkt Dateien auf der Festplatte — ein großes Sicherheitsrisiko in einem Plugin-System. `PermissionedStreamFactory` erzwingt:

1. **Berechtigungsprüfung** — Der `PermissionManager` des Aufrufers muss `FileAccess` für den Pfad gewähren
2. **Audit-Logging** — Jedes Dateiöffnen wird mit der Being-ID des Aufrufers protokolliert
3. **Pfadvalidierung** — Leere/ungültige Pfade werden vor jedem I/O abgelehnt

```
❌ new FileStream("path", FileMode.Open)           → Vom TypeRef-Scanner blockiert
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → Berechtigungsprüfung bestanden
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → Berechtigungsprüfung bestanden
```

## Demo-Pipeline

Diese Demo erstellt eine vollständige In-Memory-Datenpipeline nur mit Whitelist-Typen:

```
┌─────────────────────────────────────────────────────────────────┐
│  Demo 1: MemoryStream                                           │
│  └─ Bytes schreiben → Bytes lesen → String dekodieren            │
│                                                                  │
│  Demo 2: Komprimierungspipeline                                  │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(komprimieren) → MemoryStream(komprimiert)       │
│     → GZipStream(dekomprimieren) → MemoryStream(roh)             │
│     → UTF8 → string (Roundtrip)                                  │
│                                                                  │
│  Demo 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → Roundtrip verifizieren         │
└─────────────────────────────────────────────────────────────────┘
```

## Diese Demo

> **⚠️ Hinweis:** Diese Demo verwendet **nur** `SystemIOAllowedTypes`-Whitelist-Typen. Es wird kein Datei-I/O durchgeführt. Für Dateizugriff siehe `PermissionedStreamFactory`-API.

| Klasse | Rolle |
|--------|-------|
| `SafeSystemIOPlugin` | `IPlugin`-Implementierung — demonstriert sichere System.IO-Verwendung |

## Sicherheitshinweis

Der `System.IO`-Namespace wird vom Plugin-TypeRef-Scanner blockiert. Nur Whitelist-Typen passieren. Für tatsächlichen Dateizugriff muss `PermissionedStreamFactory` verwendet werden, das Berechtigungsprüfungen und Audit-Logging durchführt. Siehe [Sicherheitsdokumentation](../../docs/de-DE/security.md).
