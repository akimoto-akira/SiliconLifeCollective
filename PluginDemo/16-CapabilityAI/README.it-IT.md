# PluginDemo-16: Capability.AI — Permesso di servizio IA dichiarativo

## Panoramica

Questo plugin dimostra l'uso di `[PluginCapability(Capability.AI)]` per dichiarare che un plugin richiede l'accesso al servizio IA. A differenza di altre capacità, `Capability.AI` **non** esenta alcun namespace proibito — invece, permette all'host di iniettare un riferimento `IAIService` nel plugin.

## Concetto chiave: Capability.AI non concede accesso di rete

`Capability.AI` è fondamentalmente diversa dalle altre capacità:

| Capacità | Cosa esenta | Come funziona |
|---------|-----------|--------------|
| `Capability.Network` | Namespace `System.Net.*` | Rilassa le regole di scansione TypeRef/ILString |
| `Capability.FileIO` | Namespace `System.IO` | Rilassa le regole di scansione TypeRef/ILString |
| `Capability.Process` | Tipi `Process*` | Rilassa le regole di scansione TypeRef/ILString |
| `Capability.AI` | **Niente** | Abilita l'iniezione di IAIService da parte dell'host |

`IAIService` si trova nel namespace `SiliconLife.Collective` — non è mai in alcuna lista di divieto. La dichiarazione di capacità è un **segnale di opt-in** all'host che questo plugin deve ricevere il riferimento al servizio IA.

## Stack di capacità: IA + Rete

Se il client IA necessita di accesso diretto alla rete (es. chiamata a un endpoint IA remoto), è necessario dichiarare **entrambe** le capacità:

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

Vedere **17-CapabilityStacked** per esempi completi di stack.

## Modello di punto di ingresso controllato

| Risorsa | Punto di ingresso controllato | Capacità necessaria |
|---------|------------------------------|-------------------|
| File | `PermissionedStreamFactory` | Nessuna |
| Rete | `NetworkExecutor` | Nessuna |
| Processi | `CommandLineExecutor` | Nessuna |
| Archiviazione dati | `SpeedyPack` | Nessuna |
| Servizio IA | `IAIService` | `Capability.AI` |

`IAIService` è unico: **richiede** una dichiarazione di capacità. L'accesso al servizio IA è una funzionalità opt-in, non una capacità predefinita disponibile per tutti i plugin.

## File

- `Plugin.cs` — Plugin demo che dichiara Capability.AI
- `README.md` — Questo file (Inglese)
- `README.zh-CN.md` — Cinese semplificato
- Traduzioni: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Esempi correlati

- **17-CapabilityStacked**: Stack di capacità multiple (Rete + IA)
- **18-CapabilityDenied**: Anti-pattern di capacità non dichiarabili
