# PluginDemo-17: Stakování schopností — Více deklarativních oprávnění

## Přehled

Tento plugin demonstruje stakování více atributů `[PluginCapability]` na jedné třídě pluginu. `PluginCapabilityAttribute` má `AllowMultiple = true`, takže můžete deklarovat libovolný počet schopností.

## Syntaxe stakování

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## Jak PluginLoader zpracovává stakované schopnosti

1. **Načte všechny deklarace** z tabulky CustomAttribute metadat PE
2. **Sloučí** pravidla výjimek ze všech deklarovaných schopností
3. **Zaznamenává nezávisle** každou deklaraci s vlastním polem Reason
4. **Nadále vynucuje** zákazy nedeklarovatelných schopností bez ohledu na stakování

## Sloučená pravidla výjimek

Při stakování `Capability.Network` + `Capability.AI`:

| Zdroj | Výjimka |
|-------|--------|
| Capability.Network | System.Net.Http.*, System.Net.WebSockets.*, System.Net.Sockets.*, System.Net.Mail.*, System.Net.NetworkInformation.*, System.Net.Security.*, System.Net (zákazy podle typu) |
| Capability.AI | Vkládání IAIService povoleno |
| **Kombinováno** | Plugin může používat HttpClient A IAIService |

## Stakování nedává neomezenou moc

I s více stakovanými schopnostmi zůstávají tyto **vždy zablokovány**:

- ❌ P/Invoke (`DllImport`, `Marshal`, `NativeMemory`)
- ❌ Nebezpečný kód (`UnverifiableCodeAttribute`, `Unsafe`)
- ❌ IL emise (`System.Reflection.Emit.*`)
- ❌ Načítání sestavení (`System.Runtime.Loader`, `Assembly.Load*`)
- ❌ Registr (`Microsoft.Win32.*`）

Pro tyto neexistuje hodnota výčtu `Capability` — jsou **nedeklarovatelné záměrem**.

## Auditní stopa pro stakované schopnosti

Každá schopnost je zaznamenána nezávisle:

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## Soubory

- `Plugin.cs` — Demo plugin se stakem Capability.Network + Capability.AI
- `README.md` — Tento soubor (Angličtina)
- `README.zh-CN.md` — Zjednodušená čínština
- Překlady: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Související příklady

- **13-CapabilityNetwork**: Jednotlivá schopnost Network
- **16-CapabilityAI**: Jednotlivá schopnost AI
- **18-CapabilityDenied**: Antipattern nedeklarovatelných schopností
