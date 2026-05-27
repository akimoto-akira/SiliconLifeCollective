# Demo Minimálního Pluginu

Minimální implementace `IPlugin`, která demonstruje životní cyklus pluginu s napevno zadanými hodnotami.

## Přehled rozhraní IPlugin

Každý plugin SiliconLife musí implementovat rozhraní `IPlugin` definované v `SiliconLife.Collective`：

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Shrnutí vlastností

| Člen | Typ | Popis |
|------|-----|-------|
| `Id` | `string` | Jedinečný identifikátor, musí být stabilní mezi verzemi (např. `"com.siliconlife.demo.minimal"`) |
| `GetName(Language)` | `string` | Čitelný zobrazovaný název, lokalizovaný výčtem `Language` |
| `Version` | `string` | Řetězec sémantické verze (např. `"1.0.0"`) |
| `GetDescription(Language)` | `string` | Stručný popis funkčnosti pluginu |
| `GetAuthor(Language)` | `string` | Jméno autora nebo organizace |

## Pořadí volání životního cyklu

Host volá metody životního cyklu v přísném pořadí：

```
OnLoad → OnStart → [Běžící] → OnStop → OnUnload
```

| Metoda | Kdy je volána | Typické použití |
|--------|--------------|-----------------|
| `OnLoad()` | Jednou, když je DLL pluginu načteno do hostitele | Validace konfigurace, registrace typů, příprava zdrojů |
| `OnStart()` | Když je hostitel plně spuštěn a všechny pluginy načteny | Interakce s ostatními pluginy, spuštění úloh na pozadí |
| `OnStop()` | Když se hostitel korektně vypíná | Uvolnění zdrojů, vyprázdnění bufferů, uložení stavu |
| `OnUnload()` | Když je plugin uvolňován z procesu hostitele | Konečné čištění |

## Tato demo

Tento plugin vrací napevno zadané hodnoty pro všechny vlastnosti a ponechává metody životního cyklu prázdné. Je to nejjednodušší výchozí bod pro vývoj pluginů.

## Bezpečnostní poznámka

Pluginy jsou načítány v izolovaném `AssemblyLoadContext` a skenovány na zakázané odkazy na jmenné prostory (např. `System.IO`, `System.Net.Http`). Podrobnosti viz[dokumentace zabezpečení](../../docs/cs-CZ/security.md).
