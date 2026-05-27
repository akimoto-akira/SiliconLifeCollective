# PluginDemo-17: Układanie w stos możliwości — Wiele deklaratywnych uprawnień

## Przegląd

Ten plugin demonstruje układanie w stos wielu atrybutów `[PluginCapability]` na jednej klasie pluginu. `PluginCapabilityAttribute` ma `AllowMultiple = true`, więc można zadeklarować tyle możliwości, ile potrzeba.

## Składnia stosowania

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## Jak PluginLoader przetwarza możliwości w stosie

1. **Odczytuje wszystkie deklaracje** z tabeli CustomAttribute metadanych PE
2. **Łączy** reguły zwolnień ze wszystkich zadeklarowanych możliwości
3. **Rejestruje niezależnie** każdą deklarację z własnym polem Reason
4. **Nadal egzekwuje** zakazy niezadeklarowanych możliwości niezależnie od stosowania

## Połączone reguły zwolnień

Przy stosowaniu `Capability.Network` + `Capability.AI`:

| Źródło | Zwolnienie |
|--------|-----------|
| Capability.Network | System.Net.Http.*, System.Net.WebSockets.*, System.Net.Sockets.*, System.Net.Mail.*, System.Net.NetworkInformation.*, System.Net.Security.*, System.Net (zakazy wg typu) |
| Capability.AI | Wstrzyknięcie IAIService włączone |
| **Połączone** | Plugin może używać HttpClient ORAZ IAIService |

## Stosowanie nie daje nieograniczonej władzy

Nawet przy wielu możliwościach w stosie, te pozostają **zawsze zablokowane**:

- ❌ P/Invoke (`DllImport`, `Marshal`, `NativeMemory`)
- ❌ Niebezpieczny kod (`UnverifiableCodeAttribute`, `Unsafe`)
- ❌ Emisja IL (`System.Reflection.Emit.*`)
- ❌ Ładowanie zestawów (`System.Runtime.Loader`, `Assembly.Load*`)
- ❌ Rejestr (`Microsoft.Win32.*`)

Dla tych nie istnieje wartość wyliczenia `Capability` — są **niezadeklarowalne z założenia**.

## Ścieżka audytu dla możliwości w stosie

Każda możliwość jest rejestrowana niezależnie:

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## Pliki

- `Plugin.cs` — Plugin demonstracyjny ze stosem Capability.Network + Capability.AI
- `README.md` — Ten plik (Angielski)
- `README.zh-CN.md` — Chiński uproszczony
- Tłumaczenia: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Powiązane przykłady

- **13-CapabilityNetwork**: Pojedyncza możliwość Network
- **16-CapabilityAI**: Pojedyncza możliwość AI
- **18-CapabilityDenied**: Antywzorzec niezadeklarowanych możliwości
