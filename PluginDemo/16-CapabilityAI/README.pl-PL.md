# PluginDemo-16: Capability.AI — Deklaratywne uprawnienie usługi AI

## Przegląd

Ten plugin demonstruje użycie `[PluginCapability(Capability.AI)]` do deklaracji, że plugin wymaga dostępu do usługi AI. W przeciwieństwie do innych możliwości, `Capability.AI` **nie** zwalnia żadnych zabronionych przestrzeni nazw — zamiast tego pozwala hostowi wstrzyknąć referencję `IAIService` do pluginu.

## Kluczowa koncepcja: Capability.AI nie przyznaje dostępu do sieci

`Capability.AI` fundamentalnie różni się od innych możliwości:

| Możliwość | Co zwalnia | Jak działa |
|----------|-----------|-----------|
| `Capability.Network` | Przestrzenie nazw `System.Net.*` | Łagodzi reguły skanowania TypeRef/ILString |
| `Capability.FileIO` | Przestrzeń nazw `System.IO` | Łagodzi reguły skanowania TypeRef/ILString |
| `Capability.Process` | Typy `Process*` | Łagodzi reguły skanowania TypeRef/ILString |
| `Capability.AI` | **Nic** | Umożliwia wstrzyknięcie IAIService przez hosta |

`IAIService` znajduje się w przestrzeni nazw `SiliconLife.Collective` — nigdy nie ma go na żadnej liście zakazów. Deklaracja możliwości to **sygnał opt-in** do hosta, że ten plugin powinien otrzymać referencję usługi AI.

## Stosowanie możliwości: AI + Sieć

Jeśli klient AI wymaga bezpośredniego dostępu do sieci (np. wywołanie zdalnego endpointu AI), musisz zadeklarować **obie** możliwości:

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

Pełne przykłady stosowania możliwości w **17-CapabilityStacked**.

## Wzorzec kontrolowanego punktu wejścia

| Zasób | Kontrolowany punkt wejścia | Wymagana możliwość |
|-------|--------------------------|-------------------|
| Pliki | `PermissionedStreamFactory` | Brak |
| Sieć | `NetworkExecutor` | Brak |
| Procesy | `CommandLineExecutor` | Brak |
| Magazyn danych | `SpeedyPack` | Brak |
| Usługa AI | `IAIService` | `Capability.AI` |

`IAIService` jest wyjątkowy: **wymaga** deklaracji możliwości. Dostęp do usługi AI to funkcja opt-in, a nie domyślna możliwość dostępna dla wszystkich pluginów.

## Pliki

- `Plugin.cs` — Plugin demonstracyjny deklarujący Capability.AI
- `README.md` — Ten plik (Angielski)
- `README.zh-CN.md` — Chiński uproszczony
- Tłumaczenia: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Powiązane przykłady

- **17-CapabilityStacked**: Stosowanie wielu możliwości (Sieć + AI)
- **18-CapabilityDenied**: Antywzorzec niezadeklarowanych możliwości
