# PluginDemo-09: Verbotene Prozessoperationen (Anti-Pattern)

## Überblick

Dieses Plugin demonstriert **verbotene** Prozessausführungsoperationen im SiliconLife-Plugin-System. Es dient als Anti-Pattern-Referenz und zeigt, was NICHT getan werden sollte, sowie korrekte Alternativen für jeden Verstoß.

## Warum sind Process-Typen verboten?

`System.Diagnostics.Process` und `ProcessStartInfo` sind in Plugins blockiert, da direkte Prozessausführung schwerwiegende Sicherheitsrisiken birgt:

1. **Beliebige Befehlsausführung**: Plugins könnten jeden Befehl ohne Audit oder Berechtigungsprüfung ausführen
2. **Malware-Start**: Bösartige Plugins könnten unerwünschte Anwendungen oder Skripte ausführen
3. **Systemressourcen-Zugriff**: Prozesse könnten auf sensible Ressourcen außerhalb der Plugin-Sandbox zugreifen
4. **Keine Befehlsvalidierung**: Direktes Process.Start hat keinen eingebauten Schutz gegen Command Injection
5. **Kein Audit-Trail**: Direkte Prozessoperationen umgehen das Plugin-Sicherheits-Audit-System
6. **Privilegien-Eskalation**: Könnte Prozesse mit höheren Berechtigungen als das Plugin starten

## Welche Typen sind verboten?

Nur Process-bezogene Typen sind verboten, **NICHT der gesamte System.Diagnostics-Namespace**:

| Verbotener Typ | Blockierte Methode | Risikostufe |
|----------------|-------------------|-------------|
| `Process` | `Start()`, `Kill()`, `WaitForExit()` | 🔴 Kritisch |
| `ProcessStartInfo` | Konstruktor, alle Eigenschaften | 🔴 Kritisch |
| `Process` | `StandardInput`, `StandardOutput`, `StandardError` | 🔴 Kritisch |
| `Process` | `GetProcesses()`, `GetProcessesByName()` | 🟡 Hoch |

## Welche Typen sind erlaubt?

Andere `System.Diagnostics`-Typen, die keine Prozessausführung betreffen, bleiben verfügbar:

| Erlaubter Typ | Verwendung | Warum sicher |
|---------------|-----------|--------------|
| `Stopwatch` | Zeitmessung | Keine Prozessausführung |
| `Debug` | Debug-Ausgabe | Kein Sicherheitsrisiko |
| `Trace` | Tracing/Logging | Kein Sicherheitsrisiko |
| `PerformanceCounter` | Leistungsüberwachung | Nur-Lesen, auditiert |

## Wie führt man Befehle sicher aus?

### CommandLineExecutor verwenden (der einzig sichere Weg)

`CommandLineExecutor` ist der **kontrollierte Einstiegspunkt** für Befehlsausführung in Plugins:

```csharp
// ✅ KORREKT: Befehl ausführen
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"Fehler: {result.Error}");
}
```

**Was CommandLineExecutor bietet:**
1. **Command-Injection-Schutz**: Blockiert gefährliche Separatoren (`||`, `&&`, `|`, `&`, `;`)
2. **Timeout-Erzwingung**: Standard 30 Sekunden Timeout (konfigurierbar)
3. **Audit-Logging**: Alle Befehlsausführungen werden für Sicherheitsüberprüfung protokolliert
4. **Ausgabe-Erfassung**: Automatische Erfassung von stdout und stderr
5. **Plattformübergreifend**: Verwendet `cmd.exe` unter Windows, `/bin/bash` unter Unix
6. **Fehlerbehandlung**: Gibt strukturiertes Ergebnis mit Erfolg/Fehler-Status zurück

## Demonstrierte Verstöße

Dieses Plugin zeigt 5 häufige Prozessausführungs-Verstöße:

### Verstoß 1: Process.Start

```csharp
// ❌ VERBOTEN
Process.Start("notepad.exe");

// ✅ KORREKT
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**Blockierter TypeRef**: `System.Diagnostics.Process::Start(System.String)`

### Verstoß 2: ProcessStartInfo

```csharp
// ❌ VERBOTEN
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ KORREKT
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**Blockierter TypeRef**: `System.Diagnostics.ProcessStartInfo::.ctor()`

### Verstoß 3: Process mit Argumenten

```csharp
// ❌ VERBOTEN
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ KORREKT
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**Blockierter TypeRef**: `System.Diagnostics.Process::Start(ProcessStartInfo)`

### Verstoß 4: Prozess-Ausgabeumleitung

```csharp
// ❌ VERBOTEN
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ KORREKT
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**Blockierter TypeRef**: `System.Diagnostics.Process::StandardOutput`

### Verstoß 5: Process.Kill

```csharp
// ❌ VERBOTEN
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ KORREKT
// Aus Sicherheitsgründen unterstützt CommandLineExecutor das Beenden von Prozessen nicht.
// Kontaktieren Sie bei Bedarf den Systemadministrator.
```

**Blockierter TypeRef**: `System.Diagnostics.Process::Kill()`

## Warum nur Process und nicht ganz System.Diagnostics?

Das Plugin-System verfolgt einen **chirurgischen Ansatz** bei der Sicherheit:

- **Nur gefährliche Typen blockieren**: Process/ProcessStartInfo ermöglichen beliebige Codeausführung
- **Sichere Typen erlauben**: Stopwatch, Debug, Trace haben keine Sicherheitsimplikationen
- **Auswirkungen minimieren**: Entwickler können weiterhin risikolose Diagnosetools verwenden
- **Klare Grenze**: Nur Typen, die Prozesse erzeugen/beenden können, sind verboten

## PluginLoader-Sicherheitsmechanismus

Wenn PluginLoader dieses Plugin scannt:

1. **TypeRef-Scan**: Erkennt Referenzen auf verbotene `Process`/`ProcessStartInfo`-Typen
2. **MemberRef-Scan**: Erkennt Aufrufe blockierter Methoden (z.B. `Process.Start`)
3. **IL-String-Scan**: Erkennt string-basierte Reflexionsversuche zum Laden verbotener Typen
4. **Ablehnung**: Plugin wird beim Laden mit detaillierter Fehlermeldung abgelehnt

## Best Practices

1. **Immer CommandLineExecutor verwenden**: Niemals `Process.Start` direkt verwenden
2. **Angemessene Timeouts setzen**: Verhindern, dass Befehle endlos hängen
3. **Ergebnisse prüfen**: Immer `result.Success` vor Verwendung der Ausgabe prüfen
4. **Eingaben bereinigen**: Niemals Benutzereingaben direkt an Befehle übergeben
5. **Bei Bedarf Capability deklarieren**: Wenn uneingeschränkte Prozessausführung benötigt wird, `Capability.Process` deklarieren (siehe 15-CapabilityProcess)

## Dateien

- `Plugin.cs` - Anti-Pattern-Demonstrationsplugin
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Diese Datei (Deutsch)
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Verwandte Beispiele

- **08-ForbiddenNetwork**: Verbotene Netzwerkoperationen
- **15-CapabilityProcess**: Deklarative Process-Berechtigung
- **10-ForbiddenReflection**: Verbotene Reflexionsoperationen
- **12-ForbiddenStringBypass**: String-basierte Reflexionsumgehungsversuche
