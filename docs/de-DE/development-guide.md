# Entwicklungshandbuch

> **Version: v0.2.0-alpha**

[English](../en/development-guide.md) | **Deutsch** | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | [Русский](../ru-RU/development-guide.md)

## Architekturübersicht

SiliconLifeCollective folgt der **Körper-Gehirn-Architektur**, bei der Kernschnittstellen und Standardimplementierungen strikt getrennt sind.

### Projektstruktur

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Schnittstellen, abstrakte Klassen, allgemeine Infrastruktur
│   ├── SiliconLife.Common/          # Gemeinsame Implementierungen (von beiden Versionen genutzt)
│   ├── SiliconLife.Default/         # Standardimplementierung, Einstiegspunkt (Verifizierung der Architektur)
│   ├── SiliconLife.Fast/            # Hochleistungsimplementierung, Einstiegspunkt (empfohlene Produktivversion)
│   ├── SiliconLife.Speedy/          # SpeedyPack Hochleistungs-Speicher-Engine
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack-Verwaltungswerkzeug (Avalonia UI)
└── docs/                            # Mehrsprachige Dokumentation
```

**Abhängigkeitsrichtung**:
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core` (unidirektional)

**Rollen der Versionen**:
- **SiliconLife.Default**: Standardimplementierung, hauptsächlich zur Verifizierung der Machbarkeit der Architektur. Bietet eine einfache und zuverlässige Dateisystem-Speicherimplementierung, geeignet für Entwicklung, Debugging und Architekturverifizierung.
- **SiliconLife.Fast**: Empfohlene Produktivversion. Aufbauend auf der durch Default verifizierten Architektur, verwendet SpeedyPack-In-Memory-Speicherung + asynchrone Persistierung und bietet extreme Leistungsoptimierung. Dies ist die erste Wahl für den langfristigen Betrieb und echte Produktivumgebungen.

## Kernkonzepte

### 1. Silicon Being (Silicon Being)

Jeder KI-Agent besteht aus folgenden Teilen:
- **Körper** (`DefaultSiliconBeing`): Hält den Lebenszustand aufrecht, erkennt Auslöserszenarien
- **Gehirn** (`ContextManager`): Lädt Historie, ruft KI auf, führt Werkzeuge aus, persistiert Antworten

### 2. Werkzeugsystem

Werkzeuge werden durch Reflexion automatisch entdeckt und registriert:

```csharp
// Alle Werkzeuge implementieren die ITool-Schnittstelle
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Berechtigungssystem

3-stufige Berechtigungsprüfkette:
```
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → Standardmäßig verweigern)
```

### 4. Service-Locator

Globale Serviceregistrierung und -abfrage:
```csharp
// Registrieren
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Abrufen
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Erweiterungssystem

### Neues Werkzeug hinzufügen

1. Neue Klasse in `src/SiliconLife.Common/Tools/` erstellen (von beiden Versionen gemeinsam genutztes Werkzeug):

> **Hinweis**: `SiliconLife.Default` und `SiliconLife.Fast` haben keine eigenen `Tools/`-Verzeichnisse mehr. Alle gemeinsamen Werkzeuge sind einheitlich in `SiliconLife.Common/Tools/` abgelegt.

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Parameter analysieren
        var param1 = call.Parameters["param1"]?.ToString();
        
        // Logik ausführen
        var result = await DoSomething(param1);
        
        // Ergebnis zurückgeben
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. Werkzeuge werden durch Reflexion automatisch entdeckt – keine manuelle Registrierung erforderlich!

3. (Optional) Als nur für Administratoren verfügbar markieren:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

4. (Optional) Verfügbare Szenarien des Werkzeugs markieren:
```csharp
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class MyTool : ITool { ... }
```

5. (Optional) Als nur für Chat-Szenarien verfügbar markieren:
```csharp
[ChatOnly]
public class HelpTool : ITool { ... }
```

6. (Optional) Als nur für Projekt-Szenarien verfügbar markieren:
```csharp
[ToolScenario(ToolScenarioFlag.Project)]
[SiliconManagerOnly]
public class ProjectWorkTool : ITool { ... }
```

### Neuen KI-Client hinzufügen

1. `IAIClient` in `src/SiliconLife.Common/AI/` implementieren:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Ihre KI-API aufrufen
        var response = await CallMyAPI(request);
        
        return new AIResponse
        {
            Content = response.Message,
            ToolCalls = response.ToolCalls,
            Usage = response.Usage
        };
    }
    
    public async IAsyncEnumerable<string> StreamChatAsync(AIRequest request)
    {
        // Streaming implementieren
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. Fabrik erstellen:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. Fabrik wird automatisch entdeckt und registriert.

### Neues Speicher-Backend hinzufügen

1. `IStorage` und `ITimeStorage` in `src/SiliconLife.Default/Storage/` (Dateisystemimplementierung) oder `src/SiliconLife.Fast/Storage/` (SpeedyPack-Adapter) implementieren:

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // Aus Ihrer Datenbank lesen
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // In Ihre Datenbank schreiben
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // Zeitindexabfrage
    }
}
```

### Neues Plugin hinzufügen

1. Ein Klassenbibliotheksprojekt erstellen und die `IPlugin`-Schnittstelle implementieren:

```csharp
using SiliconLife.Collective;
using SiliconLife.Collective.Localization;
using SiliconLife.Collective.Tools;

public class MyPlugin : IPlugin
{
    public string Id => "my-plugin";
    public string Version => "1.0.0";
    
    public string GetName(Language language) => "My Plugin";
    public string GetDescription(Language language) => "A custom plugin";
    public string GetAuthor(Language language) => "Author Name";
    
    public void OnLoad() { }
    public void OnStart() { }
    public void OnStop() { }
    public void OnUnload() { }
}
```

2. (Optional) Die `ITool`-Schnittstelle im Plugin implementieren, um benutzerdefinierte Werkzeuge zu registrieren:

```csharp
public class MyPluginTool : ITool
{
    public string Name => "my_plugin_tool";
    public string Description => "A tool provided by my plugin";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        return new ToolResult { Success = true, Output = "Done" };
    }
}
```

3. Die kompilierte DLL im Plugin-Verzeichnis ablegen. Der `PluginLoader` lädt sie automatisch.

> **Sicherheitsbeschränkung**: Plugins dürfen nicht auf die Namespaces `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis` usw. verweisen. Plugins werden durch `AssemblyLoadContext` isoliert geladen.

### Neuen Skin hinzufügen

1. `ISkin` in `src/SiliconLife.App/Web/Skins/` implementieren:

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "A custom skin description";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
            }
            /* Your custom styles */
        ";
    }
}
```

2. Skins werden vom `SkinManager` automatisch entdeckt.

## Code-Stil-Leitfaden

### Namenskonventionen

- **Klassen**: PascalCase, mit funktionalem Präfix (z. B. `DefaultSiliconBeing`)
- **Schnittstellen**: Beginnen mit `I` (z. B. `IAIClient`, `ITool`)
- **Implementierungen**: Enden mit dem Schnittstellennamen (z. B. `OllamaClient` implementiert `IAIClient`)
- **Werkzeuge**: Enden mit `Tool` (z. B. `CalendarTool`, `ChatTool`)
- **ViewModels**: Enden mit `ViewModel` (z. B. `BeingViewModel`)

### Code-Organisation

```
SiliconLife.Common/
├── AI/                    # KI-Client- und Fabrikinplementierungen
├── Calendar/              # 32 Kalenderimplementierungen
├── Localization/          # Lokalisierungsbasisklasse und 34 Sprachvarianten
├── Security/              # Berechtigungsmanager
├── SiliconBeing/          # Standard-Silicon-Being-Implementierung
├── Tools/                 # Gemeinsame eingebaute Werkzeuge (25)
├── Web/                   # Web-Infrastruktur
└── WebView/               # Playwright WebView-Implementierung

SiliconLife.App/          # Von Default und Fast gemeinsam genutzte Anwendungsschicht
├── Config/                # Anwendungskonfiguration
├── Help/                  # Hilfedokumentation-Lokalisierung
├── Project/               # Projektsystem (Workflow-Engine, Projektrollen)
└── Web/                   # Web UI-Implementierung
    ├── Component/         # 27 UI-Komponenten
    ├── Controllers/       # 24 Routen-Controller
    ├── Models/            # ViewModels
    ├── Views/             # HTML-Ansichten
    └── Skins/             # 7 Skin-Themes

SiliconLife.Default/      # Versionsspezifische Verzeichnisse
├── Config/                # Standardkonfigurationsdaten
├── Knowledge/             # Wissensnetzwerk-Implementierung
├── Logging/               # Protokoll-Provider-Implementierung (Konsole + Dateisystem)
├── Project/               # Projektsystem-Implementierung
└── Storage/               # Dateisystem-Speicherimplementierung

SiliconLife.Fast/         # Versionsspezifische Verzeichnisse
├── Config/                # Fast-Versionskonfigurationsdaten
├── Logging/               # Protokoll-Provider-Implementierung (Konsole + Dateisystem)
├── Storage/               # SpeedyPack-Speicheradapter
└── Tray/                  # System-Tray-Lokalisierung
```

### Dokumentation

- Alle öffentlichen APIs müssen XML-Dokumentationskommentare haben
- Alle Quelldateien verwenden den Apache 2.0-Lizenzheader
- .NET 9-Features nutzen (implizite Usings, Nullable-Referenztypen)

## Entwicklungs-Workflow

### 1. Entwicklungsumgebung einrichten

```bash
# Repository klonen
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Abhängigkeiten wiederherstellen
dotnet restore

# Bauen
dotnet build
```

### 2. Tests ausführen

```bash
# Alle Tests ausführen
dotnet test

# Spezifisches Testprojekt ausführen
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Debugging

```bash
# Mit Debug-Ausgabe ausführen
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Code-Formatierung

```bash
# Code formatieren
dotnet format
```

## Benutzerdefinierte Funktionen erstellen

### Beispiel: Benutzerdefinierten Kalender hinzufügen

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Ihre Konvertierungslogik
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Rückkonvertierung
        return new GregorianDate(year, month, day);
    }
}
```

### Beispiel: Benutzerdefinierten Executor hinzufügen

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

### Beispiel: Benutzerdefinierte Workflow-Vorlage hinzufügen

```csharp
public class MyWorkflowTemplate : WorkflowTemplate
{
    public override string Name => "my_workflow";
    public override string Description => "A custom workflow template";
    
    public override void DefineStates()
    {
        AddState("start", "Gestartet", isInitial: true);
        AddState("processing", "In Bearbeitung");
        AddState("review", "Überprüfung");
        AddState("done", "Abgeschlossen", isFinal: true);
    }
    
    public override void DefineTransitions()
    {
        AddTransition("start", "processing", "Bearbeitung starten");
        AddTransition("processing", "review", "Zur Überprüfung einreichen");
        AddTransition("review", "done", "Überprüfung bestanden");
        AddTransition("review", "processing", "Überprüfung abgelehnt");
    }
}
```

### Beispiel: Projektrolle hinzufügen

Projektrollen werden über die Operationen `assign_role` und `remove_role` des `ProjectTool` verwaltet. Rollennamen sind benutzerdefinierte Zeichenfolgen, die verwendet werden, um die Zuständigkeiten von Silicon Beings in Workflows und Aufgabenzuweisungen zu unterscheiden.

## Testleitfaden

### Unit-Tests

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object> 
            { 
                ["param1"] = "test" 
            }
        };
        
        // Act
        var result = await tool.ExecuteAsync(call);
        
        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### Integrationstests

Testen des vollständigen Ablaufs:
1. KI gibt einen Werkzeugaufruf zurück
2. Werkzeug wird ausgeführt
3. Ergebnis wird an die KI zurückgemeldet
4. KI gibt die endgültige Antwort zurück

## Leistungsaspekte

### Speichersystem

- Die Default-Version verwendet dateibasierte JSON-Speicherung
- Die Fast-Version verwendet die SpeedyPack-In-Memory-Speicher-Engine (.spk-Format)
- SpeedyPack nutzt In-Memory-Verzeichnisabbildung + Eintrags-Cache + asynchrone Schreibwarteschlange
- Zeitindexabfragen verwenden die `ITimeStorage`-Schnittstelle

### Hauptschleifen-Scheduler

- Uhrzeitbasierte Fair-Scheduling-Zeitscheiben
- Watchdog-Timer zur Erkennung von hängenden Operationen
- Circuit Breaker zur Verhinderung von Kaskadenausfällen

## Best Practices

### 1. Berechtigungen immer prüfen

Jede von der KI initiierte Operation muss die Berechtigungskette durchlaufen:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return Result.Denied("Permission denied");
}
```

### 2. Service-Locator verwenden

Globale Registrierung und Abfrage von Diensten:

```csharp
// Während der Initialisierung
ServiceLocator.Instance.Register<ICustomService>(myService);

// Bei Bedarf
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Körper-Gehirn-Trennung befolgen

- Der Körper verarbeitet Zustände und Auslöser
- Das Gehirn verarbeitet KI-Interaktionen und Werkzeugausführungen

### 4. Angemessene Fehlerbehandlung implementieren

```csharp
try
{
    var result = await operation();
    return Result.Success(result);
}
catch (Exception ex)
{
    Logger.Error($"Operation failed: {ex.Message}");
    return Result.Failure(ex.Message);
}
```

## Beitragshandbuch

1. Repository forken
2. Feature-Branch erstellen (`git checkout -b feature/amazing-feature`)
3. Änderungen mit Conventional Commits committen
4. Zum Branch pushen (`git push origin feature/amazing-feature`)
5. Pull Request erstellen

### Commit-Nachrichtenformat

```
<type>(<scope>): <description>

Beispiele:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Nächste Schritte

- 📚 [Architekturhandbuch](architecture.md) lesen
- 📖 [API-Referenz](api-reference.md) erkunden
- 🔒 [Sicherheitsdokumentation](security.md) ansehen
- 🚀 [Schnellstartanleitung](getting-started.md) ansehen
