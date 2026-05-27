# PluginDemo-16: Capability.AI — Deklarativní oprávnění služby AI

## Přehled

Tento plugin demonstruje použití `[PluginCapability(Capability.AI)]` k deklaraci, že plugin vyžaduje přístup ke službě AI. Na rozdíl od jiných schopností `Capability.AI` **neuvolňuje** žádné zakázané jmenné prostory — místo toho umožňuje hostovi vložit referenci `IAIService` do pluginu.

## Klíčový koncept: Capability.AI neposkytuje síťový přístup

`Capability.AI` se zásadně liší od ostatních schopností:

| Schopnost | Co vyjímá | Jak funguje |
|----------|----------|------------|
| `Capability.Network` | Jmenné prostory `System.Net.*` | Uvolňuje pravidla skenování TypeRef/ILString |
| `Capability.FileIO` | Jmenný prostor `System.IO` | Uvolňuje pravidla skenování TypeRef/ILString |
| `Capability.Process` | Typy `Process*` | Uvolňuje pravidla skenování TypeRef/ILString |
| `Capability.AI` | **Nic** | Umožňuje vkládání IAIService hostitelem |

`IAIService` se nachází v jmenném prostoru `SiliconLife.Collective` — nikdy není na žádném seznamu zákazů. Deklarace schopnosti je **signál opt-in** hostiteli, že tento plugin by měl obdržet referenci služby AI.

## Stakování schopností: AI + Síť

Pokud váš AI klient vyžaduje přímý síťový přístup (např. volání vzdáleného AI endpointu), musíte deklarovat **obě** schopnosti:

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

Viz **17-CapabilityStacked** pro úplné příklady stakování.

## Vzor řízeného vstupního bodu

| Zdroj | Řízený vstupní bod | Vyžadovaná schopnost |
|-------|------------------|---------------------|
| Soubory | `PermissionedStreamFactory` | Žádná |
| Síť | `NetworkExecutor` | Žádná |
| Procesy | `CommandLineExecutor` | Žádná |
| Datové úložiště | `SpeedyPack` | Žádná |
| Služba AI | `IAIService` | `Capability.AI` |

`IAIService` je jedinečný: **vyžaduje** deklaraci schopnosti. Přístup ke službě AI je funkce opt-in, nikoli výchozí schopnost dostupná pro všechny pluginy.

## Soubory

- `Plugin.cs` — Demo plugin deklarující Capability.AI
- `README.md` — Tento soubor (Angličtina)
- `README.zh-CN.md` — Zjednodušená čínština
- Překlady: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Související příklady

- **17-CapabilityStacked**: Stakování více schopností (Síť + AI)
- **18-CapabilityDenied**: Antipattern nedeklarovatelných schopností
