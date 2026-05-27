# PluginDemo-08: Anti-Pattern delle Operazioni di Rete Vietate

## Panoramica

Questo plugin dimostra le operazioni di rete **VIETATE** nel sistema di plugin SiliconLife. Serve come riferimento anti-pattern, mostrando cosa NON fare e fornendo le alternative corrette.

## Perché l'accesso diretto alla rete è vietato a livello globale?

I modelli di accesso diretto alla rete sono bloccati a livello di plugin:

1. **Connessione a server dannosi**: I plugin potrebbero connettersi a server dannosi
2. **Esfiltrazione di dati**: I plugin potrebbero esfiltrare dati sensibili dalla sandbox
3. **Attacchi DNS Rebinding**: I plugin potrebbero eludere i controlli di sicurezza
4. **Bypass ACL di rete**: L'accesso diretto alla rete ignora il sistema ACL globale

## Tipi vietati

Tutti i tipi `System.Net` che accedono direttamente alla rete sono bloccati:

| Tipo vietato | Spazio dei nomi bloccato | Livello di rischio |
|----------|----------------|----------|
| `HttpClient` | `System.Net.Http` | 🔴 Critico |
| `TcpClient` | `System.Net.Sockets` | 🔴 Critico |
| `Socket` | `System.Net.Sockets` | 🔴 Critico |
| `Dns` | `System.Net` | 🔴 Critico |
| `WebClient` | `System.Net` | 🔴 Critico |

## Metodi di accesso sicuro

### NetworkExecutor (Consigliato)

`NetworkExecutor` è il **punto di ingresso controllato** per le operazioni di rete:

```csharp
// ✅ CORRETTO: Richiesta GET semplice
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

**Cosa fornisce NetworkExecutor:**
1. Verifica delle autorizzazioni
2. Registrazione di audit
3. Interruttore automatico
4. Controllo del timeout
5. Coda delle richieste

## Violazioni dimostrate

### Violazione 1: HttpClient

```csharp
// ❌ VIETATO
using var client = new HttpClient();

// ✅ CORRETTO
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

### Violazione 2: TcpClient

```csharp
// ❌ VIETATO
using var client = new TcpClient("example.com", 8080);

// ✅ CORRETTO
// Usare NetworkExecutor o dichiarare Capability.Network
```

## Meccanismo di sicurezza PluginLoader

PluginLoader analizza questo plugin e:
1. **Scansione TypeRef**: Rileva riferimenti a tipi vietati
2. **Scansione MemberRef**: Rileva chiamate a metodi bloccati
3. **Scansione stringa IL**: Rileva tentativi di riflessione
4. **Rifiuto**: Il plugin viene rifiutato al caricamento

## File

- `Plugin.cs` - Plugin dimostrativo anti-pattern
- `README.md` - Questo file (Inglese)
- `README.it-IT.md` - Questo file (Italiano)
- Altre versioni linguistiche...

## Esempi correlati

- **13-CapabilityNetwork**: Capacità di rete dichiarativa
- **07-ForbiddenFileIO**: Pattern di accesso ai file vietati