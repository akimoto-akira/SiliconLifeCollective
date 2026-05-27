# PluginDemo-15: Capability.Process — Deklarative Prozess-Berechtigung

## Übersicht

Dieses Plugin demonstriert die Verwendung von `[PluginCapability(Capability.Process)]` zur Deklaration der Fähigkeit, Kindprozesse zu starten. Mit dieser Deklaration erhält das Plugin Zugriff auf `System.Diagnostics.Process` und verwandte Typen.

## Deklarationssyntax

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Befreiungsbereich von Capability.Process

### TypeRef-Befreiung

Nur Process-bezogene Typen unter `System.Diagnostics` werden befreit:

| Befreiter Typ | Verwendung |
|--------------|-----------|
| `Process` | Kindprozesse starten, verwalten und überwachen |
| `ProcessStartInfo` | Prozessstartparameter konfigurieren |
| `ProcessThread` | Auf Prozessthread-Informationen zugreifen |
| `ProcessModule` | Auf Prozessmodul-Informationen zugreifen |
| `ProcessPriorityClass` | Prozesspriorität festlegen |
| `ProcessWindowStyle` | Prozessfensterstil konfigurieren |

Immer erlaubte Typen (niemals in der Verbotsliste): `Stopwatch`, `Debug`, `Trace`, `Activity`

### ILString-Befreiung

- Zeichenfolgen, die mit `"System.Diagnostics.Process"` beginnen, werden nicht markiert

## Vergleich mit 09-ForbiddenProcess

| Aspekt | 09-ForbiddenProcess | 15-CapabilityProcess |
|--------|-------------------|---------------------|
| Deklaration | Keine | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ ABGELEHNT | ✅ ERLAUBT |
| ProcessStartInfo | ❌ ABGELEHNT | ✅ ERLAUBT |

## Empfehlung: CommandLineExecutor

Auch mit `Capability.Process` wird `CommandLineExecutor` bevorzugt empfohlen:

| Funktion | CommandLineExecutor | Direkter Process |
|---------|-------------------|-----------------|
| Capability-Deklaration nötig | Nein | Ja |
| Sandboxing | Befehls-Whitelist | Keine |
| Timeouts | Integriert | Manuell |
| Ausgabeerfassung | Strukturiert | Manuell |
| Audit-Logging | Automatisch | Manuell |

Verwenden Sie `Capability.Process` + direkten `Process` nur, wenn Sie eine feinkörnige Kontrolle über I/O-Streams benötigen, Prozessereignisse verarbeiten müssen oder die Befehls-Whitelist von CommandLineExecutor zu restriktiv ist.

## Sicherheitsbest Practices

1. **CommandLineExecutor bevorzugen**: Kontrollierten Einstiegspunkt verwenden, wenn möglich
2. **Klare Reason angeben**: „Launch build tools for CI pipeline" statt vagem „process access"
3. **Alle Eingaben validieren**: Niemals nicht vertrauenswürdige Eingaben direkt an ProcessStartInfo übergeben
4. **WaitForExit verwenden**: Immer auf Prozessabschluss warten, um Zombie-Prozesse zu vermeiden
5. **Streams umleiten**: `RedirectStandardOutput = true` und `UseShellExecute = false` setzen

## Dateien

- `Plugin.cs` — Demo-Plugin mit Capability.Process-Deklaration
- `README.md` — Diese Datei (Englisch)
- `README.zh-CN.md` — Vereinfachtes Chinesisch
- Übersetzungen: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Verwandte Beispiele

- **09-ForbiddenProcess**: Antimuster für blockierte Prozessoperationen
- **18-CapabilityDenied**: Antimuster für nicht deklarierbare Fähigkeiten
