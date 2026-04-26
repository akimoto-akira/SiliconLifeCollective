# Entwicklungsleitfaden

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md)

## Architekturübersicht

SiliconLifeCollective folgt der **Body-Brain-Architektur**, mit strenger Trennung von Kernschnittstellen und Standardimplementierungen.

### Projektstruktur

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # Schnittstellen, abstrakte Klassen, allgemeine Infrastruktur
│   └── SiliconLife.Default/   # Konkrete Implementierungen, Einstiegspunkte
└── docs/                      # Mehrsprachige Dokumentation
```

**Abhängigkeitsrichtung**: `SiliconLife.Default` → `SiliconLife.Core` (einfach gerichtet)

## Kernkonzepte

### 1. Silicon Being

Jeder KI-Agent besteht aus:
- **Body** (`DefaultSiliconBeing`): Hält Am-Leben-Status, erkennt Triggerszenarien
- **Brain** (`ContextManager`): Lädt Verlauf, ruft KI auf, führt Tools aus, persistiert Antworten

### 2. Tool-System

Tools werden durch Reflektion automatisch entdeckt und registriert:

```csharp
// Alle Tools implementieren ITool-Schnittstelle
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Berechtigungssystem

5-stufige Berechtigungsvalidierungskette:
```
IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
```

### 4. Service Locator

Globale Dienstregistrierung und -abfrage:
```csharp
// Registrieren
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Abrufen
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Systemerweiterung

### Neues Tool hinzufügen

1. Neue Klasse in `src/SiliconLife.Default/Tools/` erstellen:

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Parameter parsen
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

2. Tools werden automatisch durch Reflektion entdeckt - keine manuelle Registrierung nötig!

3. (Optional) Als nur-Administrator markieren:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### Neuen KI-Client hinzufügen

1. `IAIClient` in `src/SiliconLife.Default/AI/` implementieren:

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

2. Factory erstellen:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. Factory wird automatisch entdeckt und registriert.

### Neues Storage-Backend hinzufügen

1. `IStorage` und `ITimeStorage` in `src/SiliconLife.Default/Storage/` implementieren:

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
        // Zeitindex-Abfrage
    }
}
```

### Neuen Skin hinzufügen

1. `ISkin` in `src/SiliconLife.Default/Web/Skins/` implementieren:

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

2. Skins werden automatisch von `SkinManager` entdeckt.

## Code-Style-Richtlinien

### Namenskonventionen

- **Klassen**: PascalCase, mit Funktionspräfix (z.B. `DefaultSiliconBeing`)
- **Schnittstellen**: Beginnen mit `I` (z.B. `IAIClient`, `ITool`)
- **Implementierungen**: Enden mit Schnittstellennamen (z.B. `OllamaClient` implementiert `IAIClient`)
- **Tools**: Enden mit `Tool` (z.B. `CalendarTool`, `ChatTool`)
- **ViewModels**: Enden mit `ViewModel` (z.B. `BeingViewModel`)

### Code-Organisation

```
SiliconLife.Default/
├── AI/                    # KI-Client-Implementierungen
├── Calendar/              # Kalender-Implementierungen
├── Config/                # Standard-Konfigurationsdaten
├── Executors/             # Executor-Implementierungen
├── IM/                    # IM-Provider-Implementierungen
├── Localization/          # Lokalisierungs-Implementierungen
├── Logging/               # Logging-Provider-Implementierungen
├── Runtime/               # Runtime-Komponenten
├── Security/              # Sicherheits-Implementierungen
├── SiliconBeing/          # Standard-Silicon-Being-Implementierung
├── Storage/               # Storage-Implementierungen
├── Tools/                 # Integrierte Tools
└── Web/                   # Web-UI-Implementierung
    ├── Controllers/       # Route-Controller
    ├── Models/            # ViewModels
    ├── Views/             # HTML-Ansichten
    └── Skins/             # Skin-Themes
```

### Dokumentation

- Alle öffentlichen APIs müssen XML-Dokumentationskommentare haben
- Alle Quelldateien verwenden Apache 2.0 Lizenzheader
- .NET 9-Features nutzen (implizite Usings, nullable Referenztypen)

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

### 3. Debuggen

```bash
# Mit Debug-Ausgabe ausführen
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Code formatieren

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
        // Rückwärtskonvertierung
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
        // Zuerst Berechtigung prüfen
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        // Operation ausführen
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

## Testrichtlinien

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

Vollständigen Ablauf testen:
1. KI gibt Tool-Aufruf zurück
2. Tool-Ausführung
3. Ergebnis an KI zurückgeben
4. KI gibt finale Antwort

## Performance-Überlegungen

### Storage-System

- Storage-System priorisiert **Funktionalität über Performance**
- Standardmäßig dateibasierte JSON-Speicherung
- Zeitindex-Abfragen verwenden Verzeichnisstruktur

### Hauptschleifen-Scheduler

- Clock-basiertes Time-Slice-Fair-Scheduling
- Watchdog-Timer zur Erkennung blockierter Operationen
- Circuit Breaker zur Verhinderung von Kaskadenfehlern

## Best Practices

### 1. Immer Berechtigungen validieren

Jede von KI initiierte Operation muss Berechtigungskette durchlaufen:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
}
```

### 2. Service Locator verwenden

Dienste global registrieren und abrufen:

```csharp
// Während Initialisierung
ServiceLocator.Instance.Register<ICustomService>(myService);

// Bei Bedarf
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Body-Brain-Trennung befolgen

- Body verarbeitet Status und Trigger
- Brain verarbeitet KI-Interaktion und Tool-Ausführung

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

## Beitragsrichtlinien

1. Repository forken
2. Feature-Branch erstellen (`git checkout -b feature/amazing-feature`)
3. Änderungen mit Conventional Commits committen
4. Zum Branch pushen (`git push origin feature/amazing-feature`)
5. Pull Request öffnen

### Commit-Nachrichtenformat

```
<type>(<scope>): <description>

Beispiele:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Nächste Schritte

- 📚 [Architekturleitfaden](architecture.md) lesen
- 📖 [API-Referenz](api-reference.md) erkunden
- 🔒 [Sicherheitsdokumentation](security.md) prüfen
- 🚀 [Schnellstart-Leitfaden](getting-started.md) ansehen
