# PluginDemo-20: SpeedyPack — Archiviazione dati strutturati

## Panoramica

Questo plugin dimostra l'uso di `SpeedyPack` per l'archiviazione di dati strutturati **senza alcuna dichiarazione di capacità**. SpeedyPack è il metodo **consigliato** per la persistenza dei dati dei plugin.

## Perché SpeedyPack?

| Funzionalità | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|-------------|-----------|--------------------------|------------------------------|
| Capacità necessaria | **Nessuna** | Nessuna | `Capability.FileIO` |
| Cache | ✅ Integrata | ❌ | ❌ |
| WAL (recupero crash) | ✅ | ❌ | ❌ |
| Transazioni | ✅ `IPackTransaction` | ❌ | ❌ |
| Thread-safe | ✅ | ❌ | ❌ |
| Serializzazione strutturata | ✅ `Read<T>` | ❌ Byte grezzi | ❌ Manuale |
| Traccia di audit | ✅ Automatica | ✅ Automatica | ❌ Manuale |

## CRUD di base

```csharp
// Aprire un file dati SpeedyPack
using var pack = SpeedyPack.Open("mydata.spk");

// Scrivere coppie chiave-valore
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// Leggere valori (tipizzati)
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// Eliminare una chiave
pack.Delete("user:age");

// Verificare l'esistenza
bool exists = pack.Contains("user:name");  // true
```

## Accesso tipizzato con oggetti strutturati

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// Scrivere oggetto strutturato
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// Leggere oggetto tipizzato
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## Transazioni

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // Atomico — entrambe le scritture vengono persistite o nessuna
    }
    catch
    {
        tx.Rollback();  // Scartare tutte le scritture di questa transazione
    }
}
```

### Metodi IPackTransaction

| Metodo | Descrizione |
|--------|-------------|
| `Write(key, value)` | Accodare un'operazione di scrittura |
| `Delete(key)` | Accodare un'operazione di eliminazione |
| `Commit()` | Applicare atomicamente tutte le operazioni accodate |
| `Rollback()` | Scartare tutte le operazioni accodate |

## Configurazione con SpeedyPackOptions

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 MB di cache
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### Proprietà SpeedyPackOptions

| Proprietà | Tipo | Predefinito | Descrizione |
|-----------|------|------------|-------------|
| `MaxCacheSize` | `long` | 64 MB | Dimensione massima della cache in memoria |
| `AutoFlushInterval` | `TimeSpan` | 10 secondi | Intervallo di svuotamento della cache su disco |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | Livello di compressione per i dati archiviati |

## Nota di sicurezza

SpeedyPack **non richiede** alcuna dichiarazione di capacità. È un punto di ingresso sicuro e controllato per l'archiviazione dei dati che:
- Valida tutti i percorsi rispetto ai limiti dell'area di lavoro
- Fornisce traccia di audit completa di tutte le operazioni di lettura/scrittura
- Previene attacchi di traversal delle directory
- Gestisce automaticamente il ciclo di vita delle risorse

## File

- `Plugin.cs` — Plugin demo SpeedyPack
- `README.md` — Questo file (Inglese)
- `README.zh-CN.md` — Cinese semplificato
- Traduzioni: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Esempi correlati

- **04-SafeSystemIO**: Tipi System.IO in memoria consentiti (nessuna dichiarazione necessaria)
- **07-ForbiddenFileIO**: Anti-pattern di operazioni su file bloccate
- **14-CapabilityFileIO**: Quando SpeedyPack non è sufficiente
