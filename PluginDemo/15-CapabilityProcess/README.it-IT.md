# PluginDemo-15: Capability.Process — Permesso dichiarativo di processo

## Panoramica

Questo plugin dimostra l'uso di `[PluginCapability(Capability.Process)]` per dichiarare che un plugin richiede la capacità di avviare processi figli. Con questa dichiarazione, il plugin accede a `System.Diagnostics.Process` e ai tipi correlati.

## Sintassi di dichiarazione

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Ambito di esenzione di Capability.Process

### Esenzioni TypeRef

Solo i tipi relativi a Process sotto `System.Diagnostics` sono esentati:

| Tipo esentato | Utilizzo |
|--------------|---------|
| `Process` | Avviare, gestire e monitorare processi figli |
| `ProcessStartInfo` | Configurare i parametri di avvio del processo |
| `ProcessThread` | Accedere alle informazioni sui thread del processo |
| `ProcessModule` | Accedere alle informazioni sui moduli del processo |
| `ProcessPriorityClass` | Impostare la priorità del processo |
| `ProcessWindowStyle` | Configurare lo stile della finestra del processo |

Tipi sempre permessi (mai nella lista di divieto): `Stopwatch`, `Debug`, `Trace`, `Activity`

### Esenzione ILString

- Le stringhe che iniziano con `"System.Diagnostics.Process"` non vengono segnalate

## Confronto con 09-ForbiddenProcess

| Aspetto | 09-ForbiddenProcess | 15-CapabilityProcess |
|---------|-------------------|---------------------|
| Dichiarazione | Nessuna | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ RIFIUTATO | ✅ PERMESSO |
| ProcessStartInfo | ❌ RIFIUTATO | ✅ PERMESSO |

## Raccomandazione: CommandLineExecutor

Anche con `Capability.Process`, si raccomanda di privilegiare `CommandLineExecutor`:

| Funzionalità | CommandLineExecutor | Process diretto |
|-------------|-------------------|----------------|
| Dichiarazione di capacità necessaria | No | Sì |
| Sandbox | Lista consentita di comandi | Nessuna |
| Timeout | Integrato | Manuale |
| Cattura dell'output | Strutturata | Manuale |
| Registrazione di audit | Automatica | Manuale |

Usare `Capability.Process` + `Process` diretto solo quando è necessario un controllo dettagliato sui flussi I/O, la gestione degli eventi del processo, o quando la lista consentita di CommandLineExecutor è troppo restrittiva.

## Buone pratiche di sicurezza

1. **Preferire CommandLineExecutor**: Usare punto di ingresso controllato quando possibile
2. **Fornire una Reason chiara**: "Launch build tools for CI pipeline" vs vago "process access"
3. **Validare tutti gli input**: Non passare mai input non fidati direttamente a ProcessStartInfo
4. **Usare WaitForExit**: Attendere sempre il completamento del processo per prevenire processi zombie
5. **Reindirizzare i flussi**: Impostare `RedirectStandardOutput = true` e `UseShellExecute = false`

## File

- `Plugin.cs` — Plugin demo che dichiara Capability.Process
- `README.md` — Questo file (Inglese)
- `README.zh-CN.md` — Cinese semplificato
- Traduzioni: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Esempi correlati

- **09-ForbiddenProcess**: Anti-pattern di operazioni di processo bloccate
- **18-CapabilityDenied**: Anti-pattern di capacità non dichiarabili
