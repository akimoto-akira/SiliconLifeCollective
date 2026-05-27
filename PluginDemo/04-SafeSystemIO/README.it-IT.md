# Demo System.IO sicuro

Dimostra i tipi System.IO nella whitelist `SystemIOAllowedTypes`: `MemoryStream`, `BinaryReader`/`BinaryWriter`, `GZipStream`. Spiega perché `FileStream` richiede `PermissionedStreamFactory`.

## Whitelist SystemIOAllowedTypes

Il runtime dei plugin blocca lo spazio dei nomi `System.IO` per impostazione predefinita, ma esime i tipi che **non eseguono direttamente I/O su file**:

| Categoria | Tipi consentiti | Perché sicuro |
|-----------|----------------|--------------|
| Astrazioni stream | `Stream` | Classe base astratta, nessun I/O proprio |
| Stream in memoria | `MemoryStream` | Operazione puramente in memoria |
| Stream di compressione | `GZipStream`, `DeflateStream`, `ZLibStream` | Avvolgono un altro stream, non aprono file |
| Wrapper binari | `BinaryReader`, `BinaryWriter` | Avvolgono qualsiasi stream, non aprono file |
| Enumerazioni | `SeekOrigin`, `FileMode`, `FileAccess`, `FileShare`, `CompressionMode`, `CompressionLevel` | Solo tipi valore |
| Eccezioni | `IOException`, `InvalidDataException`, `EndOfStreamException` | Solo tipi errore |

### Tipi non nella whitelist

Questi tipi **accedono direttamente al file system** e sono **bloccati** nel codice dei plugin:

| Tipo bloccato | Motivo | Alternativa sicura |
|--------------|--------|-------------------|
| `FileStream` | Apre file direttamente | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | Operazioni file statiche | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | Operazioni directory statiche | `SafePath` (verifica permessi) |
| `FileInfo` | Incapsula percorsi file | `SafePath` |
| `DirectoryInfo` | Incapsula percorsi directory | `SafePath` |
| `StreamReader` | Apre file direttamente | `PermissionedStreamFactory` + avvolgere `PermissionedStream` |
| `StreamWriter` | Apre file direttamente | `PermissionedStreamFactory` + avvolgere `PermissionedStream` |

## Perché PermissionedStreamFactory per FileStream

`FileStream` apre direttamente file su disco — un grave rischio di sicurezza in un sistema di plugin. `PermissionedStreamFactory` impone:

1. **Verifica dei permessi** — il `PermissionManager` del chiamante deve concedere `FileAccess` per il percorso
2. **Log di audit** — ogni apertura di file viene registrata con l'ID being del chiamante
3. **Validazione del percorso** — percorsi vuoti/non validi vengono rifiutati prima di qualsiasi I/O

```
❌ new FileStream("path", FileMode.Open)           → Bloccato dallo scanner TypeRef
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → Verifica permessi superata
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → Verifica permessi superata
```

## Pipeline della demo

Questa demo costruisce una pipeline dati completa in memoria usando solo tipi della whitelist:

```
┌─────────────────────────────────────────────────────────────────┐
│  Demo 1: MemoryStream                                           │
│  └─ Scrivi byte → Leggi byte → Decodifica stringa               │
│                                                                  │
│  Demo 2: Pipeline di compressione                                │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(comprimi) → MemoryStream(compresso)             │
│     → GZipStream(decomprimi) → MemoryStream(grezzo)              │
│     → UTF8 → string (andata e ritorno)                           │
│                                                                  │
│  Demo 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → Verifica andata e ritorno     │
└─────────────────────────────────────────────────────────────────┘
```

## Questa demo

> **⚠️ Nota:** Questa demo usa **solo** tipi della whitelist `SystemIOAllowedTypes`. Non viene eseguito alcun I/O su file. Per l'accesso ai file, vedere l'API `PermissionedStreamFactory`.

| Classe | Ruolo |
|--------|-------|
| `SafeSystemIOPlugin` | Implementazione `IPlugin` — dimostra l'uso sicuro di System.IO |

## Nota di sicurezza

Lo spazio dei nomi `System.IO` è bloccato dallo scanner TypeRef dei plugin. Solo i tipi della whitelist passano. Per l'accesso effettivo ai file, è necessario usare `PermissionedStreamFactory`, che esegue verifiche dei permessi e log di audit. Vedere la [documentazione sulla sicurezza](../../docs/it-IT/security.md).
