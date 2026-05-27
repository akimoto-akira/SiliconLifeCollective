# PluginDemo-19: TickObject — Periodische Aufgabe in MainLoop

## Übersicht

Dieses Plugin demonstriert die Verwendung von `TickObject` zur Integration mit `MainLoop` für periodische/fortlaufende Logik. TickObject ist die Basisklasse für Objekte, die von MainLoops Hauptschleife getickt werden können, und bietet eine einheitliche Alternative zu `System.Threading.Timer` oder `Task.Delay`.

## TickObject-Lebenszyklus

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → MainLoop.Register(this) wird automatisch im Konstruktor aufgerufen
    │
    ├── autoRegister=false → MainLoop.Register(this) später manuell aufrufen
    │
    ▼
MainLoop.Tick()-Schleife
    │
    ├── Alle registrierten TickObjects nach Priority aufsteigend sortieren
    ├── elapsedTime für jedes TickObject akkumulieren
    ├── Wenn elapsedTime >= Interval → OnTick(deltaTime) aufrufen
    │
    ├── Circuit-Breaker: Wenn OnTick TickTimeout überschreitet → Timeout-Zähler erhöhen
    │   └── Nach maxTimeoutCount aufeinanderfolgenden Timeouts → 1-Minuten-Abkühlphase
    │
    ▼
MainLoop.Unregister(tickObject) — Bereinigung in OnStop
```

## Schlüsseleigenschaften

| Eigenschaft | Typ | Standard | Beschreibung |
|------------|-----|----------|-------------|
| `Interval` | `TimeSpan` | Erforderlich | Wie oft OnTick aufgerufen wird |
| `Priority` | `int` | 100 | Ausführungsreihenfolge (niedriger = höhere Priorität) |
| `autoRegister` | `bool` | `true` | Automatisch bei MainLoop im Konstruktor registrieren |

## Schlüsselmethoden

| Methode | Beschreibung |
|---------|-------------|
| `OnTick(TimeSpan deltaTime)` | Überschreiben, um periodische Logik zu implementieren |
| `MainLoop.Register(TickObject)` | Manuell bei MainLoop registrieren |
| `MainLoop.Unregister(TickObject)` | Von MainLoop entfernen (Bereinigung) |

## Demo-Szenarien

### 1. Basis-Timer (autoRegister=true)
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

### 2. Manuelle Registrierung (autoRegister=false)
```csharp
// Im Konstruktor: nicht automatisch registrieren
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// In OnStart: manuell registrieren
MainLoop.Register(_heartbeatTimer);
```

### 3. Prioritätsreihenfolge
- `Priority = 10` → Hohe Priorität, wird zuerst ausgeführt
- `Priority = 200` → Niedrige Priorität, wird danach ausgeführt

### 4. Bereinigung
```csharp
// In OnStop: immer abmelden, um Lecks zu verhindern
MainLoop.Unregister(_statusTimer);
```

## MainLoop Circuit-Breaker

MainLoop hat einen eingebauten Circuit-Breaker, um zu verhindern, dass langsame TickObjects die gesamte Schleife blockieren:

1. Wenn `OnTick` `TickTimeout` (Standard 1 Sekunde) überschreitet → Timeout-Zähler erhöht sich
2. Nach `maxTimeoutCount` (Standard 3) aufeinanderfolgenden Timeouts → Circuit-Breaker löst aus
3. Ausgelöstes TickObject wird für 1 Minute Abkühlphase **übersprungen**
4. Nach der Abkühlphase erhält das TickObject eine weitere Chance

## TickObject vs System.Threading.Timer

| Aspekt | TickObject + MainLoop | System.Threading.Timer |
|--------|----------------------|----------------------|
| Thread-Modell | Einzelner Hauptschleifen-Thread | Thread-Pool-Threads |
| Ausführungsreihenfolge | Deterministisch (nach Priority) | Nicht-deterministisch |
| Circuit-Breaker | Eingebaut | Keiner |
| Debugging | Einfach (ein Thread) | Schwierig (Race Conditions) |
| Ressourcenverbrauch | Minimal (kein Thread-Pool) | Thread-Pool-Overhead |
| Intervallgenauigkeit | Best-Effort (beeinflusst durch andere TickObjects) | Präziser |

## Sicherheitshinweis

TickObject selbst erfordert **keine** Capability-Deklaration. Es ist ein sicheres, eingebautes Framework-Mechanismus.

## Dateien

- `Plugin.cs` — Demo-Plugin für TickObject
- `README.md` — Diese Datei (Englisch)
- `README.zh-CN.md` — Vereinfachtes Chinesisch
- Übersetzungen: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Verwandte Beispiele

- **13-CapabilityNetwork**: Capability.Network-Deklaration
- **20-SpeedyPack**: Datenspeicherung ohne Capability.FileIO
