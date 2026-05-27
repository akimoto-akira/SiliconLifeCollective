# Operazioni I/O file vietate — Anti-pattern

Dimostra le operazioni di I/O file **vietate** nel sistema plugin. Questo esempio serve come riferimento anti-pattern, mostrando cosa NON fare e fornendo le alternative corrette per ogni violazione.

## Perché System.IO è vietato globalmente?

L'intero namespace `System.IO` è bloccato a livello plugin perché l'accesso diretto ai file presenta gravi rischi di sicurezza:

1. **Accesso non autorizzato ai file**: I plugin potrebbero leggere file sensibili fuori dal workspace (password, chiavi, dati personali)
2. **Attacchi di sovrascrittura**: Plugin malevoli potrebbero sovrascrivere file di sistema o configurazione critici
3. **Attraversamento directory**: I plugin potrebbero usare percorsi `../` per uscire dai confini del workspace
4. **Esaurimento risorse**: La creazione non controllata di file potrebbe riempire lo spazio su disco
5. **Nessuna traccia di audit**: Le operazioni dirette sui file aggirano il sistema di audit di sicurezza dei plugin

## Tipi vietati

Tutti i tipi `System.IO` che accedono direttamente al file system sono bloccati:

| Tipo vietato | Metodo bloccato | Livello di rischio |
|-------------|-----------------|-------------------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` ecc. | 🔴 Critico |
| `FileStream` | Costruttore con percorso file | 🔴 Critico |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 Critico |
| `StreamReader` | Costruttore con percorso (stringa) | 🔴 Critico |
| `StreamWriter` | Costruttore con percorso (stringa) | 🔴 Critico |
| `FileInfo` | Tutti i metodi | 🔴 Critico |
| `DirectoryInfo` | Tutti i metodi | 🔴 Critico |

## Tipi consentiti (eccezioni whitelist)

I tipi che eseguono **operazioni puramente in memoria** (nessun accesso diretto al file system) sono consentiti:

| Tipo consentito | Utilizzo | Perché è sicuro |
|----------------|----------|-----------------|
| `MemoryStream` | Flusso di byte in memoria | Nessun accesso al file system |
| `BinaryReader` | Lettura da flusso esistente | Avvolge il flusso, non apre file |
| `BinaryWriter` | Scrittura in flusso esistente | Avvolge il flusso, non crea file |
| `GZipStream` | Compressione/decompressione | Avvolge il flusso, nessun accesso file |
| `StreamReader` | Costruttore con parametro `Stream` | Sicuro quando avvolge flussi verificati |
| `StreamWriter` | Costruttore con parametro `Stream` | Sicuro quando avvolge flussi verificati |

Vedi l'esempio **04-SafeSystemIO** per i tipi consentiti.

## Accesso sicuro ai file tramite PermissionedStreamFactory

`PermissionedStreamFactory` è il **punto di ingresso controllato** per le operazioni sui file nei plugin:

```csharp
// ✅ Corretto: leggere un file
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ Corretto: scrivere un file
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("Dati di log");
```

**PermissionedStreamFactory fornisce:**
1. **Validazione del percorso**: Previene attacchi di attraversamento directory (`../`)
2. **Controllo permessi**: Assicura che il file sia all'interno del workspace consentito
3. **Log di audit**: Tutti gli accessi ai file sono registrati per la revisione di sicurezza
4. **Pulizia risorse**: Traccia i flussi aperti e previene le perdite

## Violazioni in questo esempio

### Violazione 1: File.ReadAllText

```csharp
// ❌ Vietato — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ Alternativa corretta
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### Violazione 2: File.WriteAllText

```csharp
// ❌ Vietato — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ Alternativa corretta
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### Violazione 3: FileStream diretto

```csharp
// ❌ Vietato — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ Alternativa corretta
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### Violazione 4: Directory.GetFiles

```csharp
// ❌ Vietato — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ Alternativa corretta (usando SpeedyPack)
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### Violazione 5: StreamReader con percorso diretto

```csharp
// ❌ Vietato — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ Alternativa corretta
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## Confronto con altri esempi

| Esempio | Focus | Permesso richiesto |
|---------|-------|-------------------|
| **04-SafeSystemIO** | Tipi di memoria consentiti (MemoryStream, GZipStream) | Nessuno |
| **07-ForbiddenFileIO** | Pattern di accesso file vietati (questo esempio) | Non applicabile (bloccato) |
| **14-CapabilityFileIO** | Dichiarare la capability FileIO per aggirare le restrizioni | `Capability.FileIO` |

## Meccanismo di scansione di sicurezza del PluginLoader

Quando PluginLoader scansiona questo plugin:

1. **Scansione TypeRef**: Rileva riferimenti a tipi `System.IO` vietati
2. **Scansione MemberRef**: Rileva chiamate a metodi bloccati
3. **Scansione stringhe IL**: Rileva tentativi di elusione tramite riflessione basata su stringhe
4. **Rifiuto**: Il plugin viene rifiutato durante il caricamento con un messaggio di errore dettagliato

L'elusione tramite concatenazione di stringhe, riflessione, caricamento dinamico o offuscamento è impossibile — questi vengono catturati dalla scansione a livello IL (vedi **12-ForbiddenStringBypass**).

## Nota di sicurezza

Se hai davvero bisogno di accesso illimitato ai file, puoi dichiarare `Capability.FileIO` (vedi 14-CapabilityFileIO). Tuttavia, le best practice sono:
- Preferire **SpeedyPack** per l'archiviazione di dati strutturati (nessuna dichiarazione di permesso necessaria)
- Usare **PermissionedStreamFactory** quando l'accesso ai file è necessario (punto di ingresso controllato)
- Dichiarare `Capability.FileIO` solo se le soluzioni sopra non sono sufficienti
