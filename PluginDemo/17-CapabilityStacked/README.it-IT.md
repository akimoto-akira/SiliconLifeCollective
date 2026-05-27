# PluginDemo-17: Impilamento di capacità — Permessi dichiarativi multipli

## Panoramica

Questo plugin dimostra l'impilamento di più attributi `[PluginCapability]` su una singola classe di plugin. `PluginCapabilityAttribute` ha `AllowMultiple = true`, quindi è possibile dichiarare quante capacità si desidera.

## Sintassi di impilamento

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## Come PluginLoader elabora le capacità impilate

1. **Legge tutte le dichiarazioni** dalla tabella CustomAttribute dei metadati PE
2. **Unisce** le regole di esenzione di tutte le capacità dichiarate
3. **Registra indipendentemente** ogni dichiarazione con il proprio campo Reason
4. **Applica ancora** i divieti di capacità non dichiarabili indipendentemente dall'impilamento

## Regole di esenzione unite

Impilando `Capability.Network` + `Capability.AI`:

| Fonte | Esenzione |
|-------|----------|
| Capability.Network | System.Net.Http.*, System.Net.WebSockets.*, System.Net.Sockets.*, System.Net.Mail.*, System.Net.NetworkInformation.*, System.Net.Security.*, System.Net (divieti per tipo) |
| Capability.AI | Iniezione IAIService abilitata |
| **Combinato** | Il plugin può usare HttpClient E IAIService |

## L'impilamento non concede potere illimitato

Anche con più capacità impilate, queste rimangono **sempre bloccate**:

- ❌ P/Invoke (`DllImport`, `Marshal`, `NativeMemory`)
- ❌ Codice unsafe (`UnverifiableCodeAttribute`, `Unsafe`)
- ❌ Emissione IL (`System.Reflection.Emit.*`)
- ❌ Caricamento assembly (`System.Runtime.Loader`, `Assembly.Load*`)
- ❌ Registro (`Microsoft.Win32.*`)

Nessun valore dell'enum `Capability` esiste per questi — sono **non dichiarabili per progetto**.

## Traccia di audit per capacità impilate

Ogni capacità è registrata indipendentemente:

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## File

- `Plugin.cs` — Plugin demo con impilamento Capability.Network + Capability.AI
- `README.md` — Questo file (Inglese)
- `README.zh-CN.md` — Cinese semplificato
- Traduzioni: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Esempi correlati

- **13-CapabilityNetwork**: Capacità Network singola
- **16-CapabilityAI**: Capacità IA singola
- **18-CapabilityDenied**: Anti-pattern di capacità non dichiarabili
