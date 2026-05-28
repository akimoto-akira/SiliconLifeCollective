# CS-Quellcode-Kompilierungs-Lademodus Demo

Ein Plugin, das aus rohen `.cs`-Quellcode-Dateien statt einer vorkompilierten DLL geladen wird. Demonstriert den CS-Quellcode-Kompilierungsmodus von PluginLoader (eingeführt durch task-389).

## Funktionsweise des CS-Quellcode-Modus

Wenn PluginLoader ein Plugin-Verzeichnis scannt und **keine DLL findet**, wechselt es automatisch in den CS-Quellcode-Modus:

```
1. PluginLoader scannt Plugin-Verzeichnis → keine DLL
2. Wechselt in den CS-Quellcode-Modus
3. cs.txt gefunden → Zeile für Zeile lesen, nur aufgelistete .cs-Dateien laden
   (Keine cs.txt → alle *.cs-Dateien im Verzeichnis laden)
4. Geschwister-DLLs scannen → Vertrauenswürdige DLLs direkt als Referenz hinzufügen;
   nicht vertrauenswürdige DLLs müssen ScanForbiddenReferences bestehen
5. CompilationCore (eingeschränkter Modus) kompiliert .cs-Dateien zu In-Memory-DLL
6. In-Memory-DLL-Bytes in temporäre Datei schreiben, ScanForbiddenReferences ausführen
7. Scan bestanden → Reflexion findet IPlugin-Implementierung → Instanziierung
8. Log zeigt: "Plugin loaded [CS-Source]: {Id} v{Version} from {DirName}"
```

## cs.txt — Selektive Lade-Whitelist

Die Datei `cs.txt` gibt an, welche `.cs`-Dateien kompiliert werden sollen, ein Dateiname pro Zeile:

```
Plugin.cs
```

- **Aufgelistete Dateien**: Werden kompiliert und geladen (z.B. `Plugin.cs`)
- **Nicht aufgelistete Dateien**: Werden vom Compiler ignoriert (z.B. `Helpers.cs`)
- **Zeilen beginning mit `#`**: Werden als Kommentare behandelt
- **Leere Zeilen**: Werden ignoriert
- **Keine cs.txt**: Alle `*.cs`-Dateien im Verzeichnis werden geladen

## CS-Quellcode-Modus vs DLL-Modus

| Aspekt | DLL-Modus | CS-Quellcode-Modus |
|--------|-----------|-------------------|
| Plugin-Format | Vorkompilierte `.dll` | Rohe `.cs`-Quellcode-Dateien |
| Lade-Trigger | DLL im Plugin-Verzeichnis gefunden | Keine DLL, `.cs`-Dateien vorhanden |
| Kompilierung | Zur Build-Zeit | Zur Ladezeit durch PluginLoader |
| Leistung | Kein Kompilierungs-Overhead | Roslyn-Kompilierungs-Overhead beim Start |
| Sicherheits-Scan | Direkter PE-Metadaten-Scan | Kompilierung → temporäre DLL → PE-Metadaten-Scan |
| Log-Präfix | `Plugin loaded:` | `Plugin loaded [CS-Source]:` |
| Am besten für | Produktionsbereitstellung | Entwicklungsideation |

## Fehlerbehandlung

| Szenario | Verhalten |
|----------|-----------|
| Keine DLL, keine .cs-Dateien | Warnung: "No DLL and no CS source files found" |
| Kompilierungsfehler | Fehler: Detaillierte Diagnosemeldungen protokolliert |
| Sicherheits-Scan fehlgeschlagen | Fehler: Alle Verstöße aufgelistet, Plugin abgelehnt |
| cs.txt-Eintrag nicht gefunden | Warnung: "cs.txt entry not found or not a .cs file" |
| Geschwister-DLL-Scan fehlgeschlagen | Warnung: DLL nicht als Referenz hinzugefügt, Kompilierung fortgesetzt |

## Sicherheitshinweis

CS-Quellcode-Modus-Plugins durchlaufen **denselben Sicherheits-Scan** wie DLL-Modus-Plugins. Die kompilierte Assembly wird in eine temporäre DLL-Datei geschrieben und mit `ScanForbiddenReferences` gescannt — derselbe Scan, den vorkompilierte DLLs erhalten. Alle verbotenen Namespace/Typ/Mitglied/String-Regeln gelten gleichermaßen.

Plugins werden weiterhin in einem isolierten Kontext geladen und auf verbotene Namespace-Referenzen gescannt (z.B. `System.IO`, `System.Net.Http`). Siehe [Sicherheitsdokumentation](../../docs/de-DE/security.md) für Details.
