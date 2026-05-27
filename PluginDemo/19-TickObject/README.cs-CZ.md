# PluginDemo-19: TickObject — Periodický úkol v MainLoop

## Přehled

Tento plugin demonstruje použití `TickObject` pro integraci s `MainLoop` pro periodickou/kontinuální logiku. TickObject je základní třída pro objekty, které mohou být tikovány hlavní smyčkou MainLoop, poskytující jednotnou alternativu k `System.Threading.Timer` nebo `Task.Delay`.

## Životní cyklus TickObject

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → MainLoop.Register(this) voláno automaticky v konstruktoru
    │
    ├── autoRegister=false → zavolat MainLoop.Register(this) ručně později
    │
    ▼
MainLoop.Tick() smyčka
    │
    ├── Seřadit všechny registrované TickObjects podle Priority (vzestupně)
    ├── Akumulovat elapsedTime pro každý TickObject
    ├── Pokud elapsedTime >= Interval → zavolat OnTick(deltaTime)
    │
    ├── Jistič: pokud OnTick překročí TickTimeout → zvýšit počítadlo timeoutů
    │   └── Po maxTimeoutCount po sobě jdoucích timeoutech → 1minutové ochlazení
    │
    ▼
MainLoop.Unregister(tickObject) — úklid v OnStop
```

## Klíčové vlastnosti

| Vlastnost | Typ | Výchozí | Popis |
|----------|-----|--------|-------|
| `Interval` | `TimeSpan` | Vyžadováno | Jak často je voláno OnTick |
| `Priority` | `int` | 100 | Pořadí provedení (nižší = vyšší priorita) |
| `autoRegister` | `bool` | `true` | Automaticky registrovat do MainLoop v konstruktoru |

## Klíčové metody

| Metoda | Popis |
|--------|-------|
| `OnTick(TimeSpan deltaTime)` | Přepsat pro implementaci periodické logiky |
| `MainLoop.Register(TickObject)` | Ruční registrace do MainLoop |
| `MainLoop.Unregister(TickObject)` | Odebrání z MainLoop (úklid) |

| Scénář | Popis |
|--------|-------|
| 1. Základní časovač | autoRegister=true, vypisuje stav každých 5 sekund |
| 2. Ruční registrace | autoRegister=false, registrace v OnStart |
| 3. Prioritní řazení | Dva TickObjects s různými Priority, pozorovat pořadí provedení |
| 4. Úklid | MainLoop.Unregister v OnStop |

## Jistič MainLoop

MainLoop má vestavěný jistič, aby zabránil pomalým TickObjects blokovat celou smyčku:

1. Pokud `OnTick` překročí `TickTimeout` (výchozí 1 sekunda) → počítadlo timeoutů se zvýší
2. Po `maxTimeoutCount` (výchozí 3) po sobě jdoucích timeoutech → jistič se spustí
3. Spuštěný TickObject je **přeskočen** po dobu 1minutového ochlazení
4. Po ochlazení TickObject dostane další šanci

## TickObject vs System.Threading.Timer

| Aspekt | TickObject + MainLoop | System.Threading.Timer |
|--------|----------------------|----------------------|
| Model vláken | Jedno vlákno hlavní smyčky | Vlákna z fondu vláken |
| Pořadí provedení | Deterministické (podle Priority) | Nedeterministické |
| Jistič | Vestavěný | Žádný |
| Ladění | Snadné (jedno vlákno) | Obtížné (konkurenční podmínky) |
| Využití prostředků | Minimální (bez fondu vláken) | Režie fondu vláken |
| Přesnost intervalu | Best-effort (ovlivněno jinými TickObjects) | Přesnější |

## Bezpečnostní poznámka

TickObject samotný **nevyžaduje** žádnou deklaraci schopnosti. Je to bezpečný vestavěný mechanismus frameworku.

## Soubory

- `Plugin.cs` — Demo plugin TickObject
- `README.md` — Tento soubor (Angličtina)
- `README.zh-CN.md` — Zjednodušená čínština
- Překlady: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Související příklady

- **13-CapabilityNetwork**: Deklarace Capability.Network
- **20-SpeedyPack**: Úložiště dat bez Capability.FileIO
