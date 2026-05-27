# PluginDemo-19: TickObject — Attività periodica in MainLoop

## Panoramica

Questo plugin dimostra come usare `TickObject` per integrarsi con `MainLoop` per logica periodica/continua. TickObject è la classe base per oggetti che possono essere tickati dal ciclo principale di MainLoop, fornendo un'alternativa unificata a `System.Threading.Timer` o `Task.Delay`.

## Ciclo di vita di TickObject

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → MainLoop.Register(this) chiamato automaticamente nel costruttore
    │
    ├── autoRegister=false → chiamare MainLoop.Register(this) manualmente dopo
    │
    ▼
MainLoop.Tick() ciclo
    │
    ├── Ordinare tutti i TickObject registrati per Priority (crescente)
    ├── Accumulare elapsedTime per ogni TickObject
    ├── Se elapsedTime >= Interval → chiamare OnTick(deltaTime)
    │
    ├── Circuit breaker: se OnTick supera TickTimeout → incrementare contatore timeout
    │   └── Dopo maxTimeoutCount timeout consecutivi → periodo di raffreddamento di 1 minuto
    │
    ▼
MainLoop.Unregister(tickObject) — pulizia in OnStop
```

## Proprietà chiave

| Proprietà | Tipo | Predefinito | Descrizione |
|-----------|------|------------|-------------|
| `Interval` | `TimeSpan` | Obbligatorio | Ogni quanto viene chiamato OnTick |
| `Priority` | `int` | 100 | Ordine di esecuzione (più basso = priorità più alta) |
| `autoRegister` | `bool` | `true` | Auto-registrazione a MainLoop nel costruttore |

## Metodi chiave

| Metodo | Descrizione |
|--------|-------------|
| `OnTick(TimeSpan deltaTime)` | Sovrascrivere per implementare logica periodica |
| `MainLoop.Register(TickObject)` | Registrare manualmente a MainLoop |
| `MainLoop.Unregister(TickObject)` | Rimuovere da MainLoop (pulizia) |

## Scenari dimostrativi

### 1. Timer base (autoRegister=true)
```csharp
public class StatusTimer : TickObject
{
    public StatusTimer() : base(interval: TimeSpan.FromSeconds(5), autoRegister: true)
    {
        Priority = 100;
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        Console.WriteLine($"Tick, deltaTime={deltaTime.TotalMilliseconds:F0}ms");
    }
}
```

### 2. Registrazione manuale (autoRegister=false)
```csharp
// Nel costruttore: non auto-registrare
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// In OnStart: registrare manualmente
MainLoop.Register(_heartbeatTimer);
```

### 3. Ordine di priorità
- `Priority = 10` → Alta priorità, eseguito per primo
- `Priority = 200` → Bassa priorità, eseguito dopo

### 4. Pulizia
```csharp
// In OnStop: sempre deregistrare per prevenire leak
MainLoop.Unregister(_statusTimer);
```

## Circuit breaker di MainLoop

MainLoop ha un circuit breaker integrato per impedire che TickObject lenti blocchino l'intero ciclo:

1. Se `OnTick` supera `TickTimeout` (1 secondo predefinito) → il contatore di timeout aumenta
2. Dopo `maxTimeoutCount` (3 predefinito) timeout consecutivi → il circuit breaker scatta
3. Il TickObject scattato è **saltato** per 1 minuto di raffreddamento
4. Dopo il raffreddamento, il TickObject riceve un'altra possibilità

## TickObject vs System.Threading.Timer

| Aspetto | TickObject + MainLoop | System.Threading.Timer |
|---------|----------------------|----------------------|
| Modello di thread | Thread ciclo principale singolo | Thread del pool di thread |
| Ordine di esecuzione | Deterministico (per Priority) | Non deterministico |
| Circuit breaker | Integrato | Nessuno |
| Debug | Facile (thread singolo) | Difficile (race conditions) |
| Utilizzo risorse | Minimo (nessun pool di thread) | Overhead del pool di thread |
| Precisione intervallo | Best-effort (influenzato da altri TickObject) | Più preciso |

## Nota di sicurezza

TickObject stesso **non richiede** alcuna dichiarazione di capacità. È un meccanismo di framework integrato sicuro.

## File

- `Plugin.cs` — Plugin demo TickObject
- `README.md` — Questo file (Inglese)
- `README.zh-CN.md` — Cinese semplificato
- Traduzioni: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Esempi correlati

- **13-CapabilityNetwork**: Dichiarazione Capability.Network
- **20-SpeedyPack**: Archiviazione dati senza Capability.FileIO
