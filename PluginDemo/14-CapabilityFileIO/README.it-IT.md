# PluginDemo-14: Capability.FileIO — Permesso dichiarativo di I/O file

## Panoramica

Questo plugin dimostra l'uso di `[PluginCapability(Capability.FileIO)]` per dichiarare l'accesso diretto al file system. Con questa dichiarazione, il plugin accede a tutti i tipi `System.IO` oltre la whitelist `SystemIOAllowedTypes`.

## Sintassi di dichiarazione PluginCapability

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Funzionamento di Capability.FileIO

1. **Stato predefinito**: Il namespace `System.IO` è vietato globalmente; solo i tipi della whitelist `SystemIOAllowedTypes` sono permessi (MemoryStream, BinaryReader, GZipStream, ecc.)
2. **Con dichiarazione**: Il divieto dell'intero namespace `System.IO` viene rimosso — File, FileStream, Directory, StreamReader(string), ecc. diventano accessibili
3. **Esenzione ILString**: Le costanti stringa che iniziano con `"System.IO."` non vengono segnalate
4. **Limiti non dichiarabili**: P/Invoke, Unsafe, Reflection.Emit, ecc. rimangono bloccati

## Ambito di esenzione di Capability.FileIO

### Esenzioni TypeRef

Tutti i tipi `System.IO` sono esentati:

| Categoria | Tipi esentati |
|-----------|--------------|
| Operazioni su file | `File`, `FileInfo` |
| Operazioni su directory | `Directory`, `DirectoryInfo` |
| Tipi di stream | `FileStream`, `StreamReader(path)`, `StreamWriter(path)` |
| File system | `FileSystemWatcher`, `DriveInfo`, `Path` |

### Esenzione ILString

- Le stringhe che iniziano con `"System.IO."` non vengono segnalate

### Ciò che rimane vietato

| Categoria | Ancora bloccato |
|-----------|----------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` |
| Codice unsafe | `UnverifiableCodeAttribute`, `Unsafe` |
| Emissione IL | `System.Reflection.Emit.*` |
| Caricamento assembly | `System.Runtime.Loader`, `Assembly.Load*` |
| Registro | `Microsoft.Win32.*` |

## Confronto con altri esempi

| Esempio | Dichiarazione | Accesso ai file | Note |
|---------|-------------|----------------|------|
| **04-SafeSystemIO** | Nessuna | MemoryStream, BinaryReader, GZipStream | Usa solo tipi della whitelist |
| **07-ForbiddenFileIO** | Nessuna | ❌ RIFIUTATO | Esempio di anti-pattern |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ Accesso completo a System.IO | Questo esempio |
| **20-SpeedyPack** | Nessuna | Tramite API SpeedyPack (nessuna Capability richiesta) | Archiviazione dati consigliata |

## Ordine di priorità per l'accesso ai file

1. **SpeedyPack** — Nessuna dichiarazione di capacità necessaria. Cache integrata, WAL, transazioni. **Consigliato per l'archiviazione di dati strutturati.**
2. **PermissionedStreamFactory** — Nessuna dichiarazione necessaria. Accesso controllato con validazione percorso e controllo degli accessi.
3. **Capability.FileIO + System.IO diretto** — Solo quando le opzioni sopra non bastano.

## Perché preferire PermissionedStreamFactory / SpeedyPack?

Anche con `Capability.FileIO`, l'uso di punti di ingresso controllati è raccomandato perché:

1. **Tracciabilità di audit**: Tutti gli accessi sono registrati e tracciabili
2. **Validazione del percorso**: Previene attacchi di directory traversal (`../`)
3. **Controllo degli accessi**: Applicazione dei confini dell'area di lavoro
4. **Tracciamento delle risorse**: Previene perdite di stream ed esaurimento delle risorse
5. **Conformità**: I pattern di accesso controllato facilitano le revisioni di sicurezza

## Buone pratiche di sicurezza

1. **Dichiarare FileIO solo se realmente necessario**: Si può usare SpeedyPack o PermissionedStreamFactory?
2. **Fornire una Reason chiara**: "Direct log file access for audit trail" è meglio di "file access"
3. **Validare i percorsi autonomamente**: Anche con Capability.FileIO, validare tutti i percorsi dei file prima dell'uso
4. **Usare istruzioni using**: Smaltire sempre FileStream/StreamReader/StreamWriter
5. **Principio del minimo privilegio**: Dichiarare solo le capacità effettivamente necessarie al plugin

## File

- `Plugin.cs` — Plugin demo che dichiara Capability.FileIO
- `README.md` — Questo file (Inglese)
- `README.zh-CN.md` — Cinese semplificato
- Traduzioni: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Esempi correlati

- **04-SafeSystemIO**: Tipi System.IO in memoria permessi (nessuna dichiarazione necessaria)
- **07-ForbiddenFileIO**: Anti-pattern di operazioni file bloccate
- **20-SpeedyPack**: Archiviazione dati consigliata senza dichiarazione di capacità
- **18-CapabilityDenied**: Anti-pattern di capacità non dichiarabili
