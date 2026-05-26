# Vývojářská příručka

> **Verze: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | **Čeština** | [Русский](../ru-RU/development-guide.md)

## Přehled architektury

SiliconLifeCollective se řídí **architekturou Tělo-Mozek**, s přísným oddělením základních rozhraní a výchozí implementace.

### Struktura projektu

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Rozhraní, abstraktní třídy, obecná infrastruktura
│   ├── SiliconLife.Common/          # Sdílená implementace (společná pro obě verze)
│   ├── SiliconLife.Default/         # Výchozí implementace, vstupní bod (ověření proveditelnosti architektury)
│   ├── SiliconLife.Fast/            # Výkonnostní implementace, vstupní bod (hlavní produkční verze)
│   ├── SiliconLife.Speedy/          # SpeedyPack vysoce výkonný úložný engine
│   └── SiliconLife.Speedy.Manager/  # Správa SpeedyPack (Avalonia UI)
└── docs/                            # Vícejazyčná dokumentace
```

**Směr závislostí**:
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core` (jednosměrné)

**Popis rolí verzí**:
- **SiliconLife.Default**: Výchozí implementace, primárně pro ověření proveditelnosti architektury. Poskytuje jednoduchou a spolehlivou implementaci souborového systému úložiště, vhodnou pro vývoj, ladění a ověření architektury.
- **SiliconLife.Fast**: Hlavní produkční verze. Na architektuře ověřené verzí Default staví s využitím SpeedyPack paměťového úložiště + asynchronní perzistence, poskytuje extrémní optimalizaci výkonu a je první volbou pro dlouhodobý provoz a skutečné produkční prostředí.

## Základní koncepty

### 1. Křemíková Bytost

Každý AI agent se skládá z:
- **Tělo** (`DefaultSiliconBeing`): udržuje životní funkce, detekuje spouštěcí scénáře
- **Mozek** (`ContextManager`): načítá historii, volá AI, provádí nástroje, perzistuje odpovědi

### 2. Systém nástrojů

Nástroje jsou automaticky objevovány a registrovány pomocí reflexe:

```csharp
// Všechny nástroje implementují rozhraní ITool
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Systém oprávnění

3-úrovňový řetězec ověřování oprávnění:
```
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → výchozí zamítnutí)
```

### 4. Lokátor služeb

Globální registrace a vyhledávání služeb:
```csharp
// Registrace
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Získání
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Systém rozšíření

### Přidání nového nástroje

1. Vytvořte novou třídu v `src/SiliconLife.Common/Tools/` (nástroje sdílené oběma verzemi):

> **Poznámka**: `SiliconLife.Default` a `SiliconLife.Fast` již nemají nezávislé adresáře `Tools/`, všechny sdílené nástroje jsou sjednoceny v `SiliconLife.Common/Tools/`.

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Analýza parametrů
        var param1 = call.Parameters["param1"]?.ToString();
        
        // Provedení logiky
        var result = await DoSomething(param1);
        
        // Návrat výsledku
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. Nástroj je automaticky objeven pomocí reflexe — není nutná manuální registrace!

3. (Volitelné) Označení jako dostupné pouze pro správce:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

4. (Volitelné) Označení dostupných scénářů nástroje:
```csharp
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class MyTool : ITool { ... }
```

5. (Volitelné) Označení jako dostupné pouze ve scénáři chatu:
```csharp
[ChatOnly]
public class HelpTool : ITool { ... }
```

6. (Volitelné) Označení jako dostupné pouze v projektovém scénáři:
```csharp
[ToolScenario(ToolScenarioFlag.Project)]
[SiliconManagerOnly]
public class ProjectWorkTool : ITool { ... }
```

### Přidání nového AI klienta

1. Implementujte `IAIClient` v `src/SiliconLife.Common/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Volání vašeho AI API
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
        // Implementace streamování
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. Vytvořte továrnu:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. Továrna je automaticky objevena a zaregistrována.

### Přidání nového úložného backendu

1. Implementujte `IStorage` a `ITimeStorage` v `src/SiliconLife.Default/Storage/` (implementace souborového systému) nebo `src/SiliconLife.Fast/Storage/` (adaptéry SpeedyPack):

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // Čtení z vaší databáze
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // Zápis do vaší databáze
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // Časově indexovaný dotaz
    }
}
```

### Přidání nového zásuvného modulu

1. Vytvořte projekt knihovny tříd, implementujte rozhraní `IPlugin`:

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

2. (Volitelné) Implementujte rozhraní `ITool` v zásuvném modulu pro registraci vlastního nástroje:

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

3. Vložte zkompilovanou DLL do adresáře zásuvných modulů, `PluginLoader` ji automaticky načte.

> **Bezpečnostní omezení**: Zásuvné moduly nemohou odkazovat na jmenné prostory `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis` atd. Zásuvné moduly jsou izolovaně načítány pomocí `AssemblyLoadContext`.

### Přidání nového skinu

1. Implementujte `ISkin` v `src/SiliconLife.App/Web/Skins/`:

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

2. Skin je automaticky objeven pomocí `SkinManager`.

## Průvodce stylem kódu

### Konvence pojmenování

- **Třídy**: PascalCase, s funkční předponou (např. `DefaultSiliconBeing`)
- **Rozhraní**: Začínají písmenem `I` (např. `IAIClient`, `ITool`)
- **Implementace**: Končí názvem rozhraní (např. `OllamaClient` implementuje `IAIClient`)
- **Nástroje**: Končí příponou `Tool` (např. `CalendarTool`, `ChatTool`)
- **Modely pohledů**: Končí příponou `ViewModel` (např. `BeingViewModel`)

### Organizace kódu

```
SiliconLife.Common/
├── AI/                    # Implementace AI klientů a továren
├── Calendar/              # 32 implementací kalendáře
├── Localization/          # Základ lokalizace a 34 jazykových variant
├── Security/              # Správce oprávnění
├── SiliconBeing/          # Výchozí implementace Křemíkové Bytosti
├── Tools/                 # Sdílené vestavěné nástroje (25)
├── Web/                   # Webová infrastruktura
└── WebView/               # Implementace Playwright WebView

SiliconLife.App/          # Aplikační vrstva sdílená mezi Default a Fast
├── Config/                # Konfigurace aplikace
├── Help/                  # Lokalizace nápovědy
├── Project/               # Projektový systém (engine pracovních postupů, projektové role)
└── Web/                   # Implementace Web UI
    ├── Component/         # 27 UI komponent
    ├── Controllers/       # 24 směrovacích kontrolerů
    ├── Models/            # Modely pohledů
    ├── Views/             # HTML pohledy
    └── Skins/             # 7 skinových témat

SiliconLife.Default/      # Adresáře specifické pro verzi
├── Config/                # Výchozí konfigurační data
├── Knowledge/             # Implementace znalostní sítě
├── Logging/               # Implementace poskytovatele protokolů (konzole + souborový systém)
├── Project/               # Implementace projektového systému
└── Storage/               # Implementace souborového systému úložiště

SiliconLife.Fast/         # Adresáře specifické pro verzi
├── Config/                # Konfigurační data verze Fast
├── Logging/               # Implementace poskytovatele protokolů (konzole + souborový systém)
├── Storage/               # Adaptéry úložiště SpeedyPack
└── Tray/                  # Lokalizace systémové lišty
```

### Dokumentace

- Všechna veřejná API musí mít XML dokumentační komentáře
- Všechny zdrojové soubory používají hlavičku licence Apache 2.0
- Využití funkcí .NET 9 (implicitní using, nullable referenční typy)

## Vývojový pracovní postup

### 1. Nastavení vývojového prostředí

```bash
# Klonování repozitáře
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Obnovení závislostí
dotnet restore

# Sestavení
dotnet build
```

### 2. Spuštění testů

```bash
# Spuštění všech testů
dotnet test

# Spuštění konkrétního testovacího projektu
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Ladění

```bash
# Spuštění s ladicím výstupem
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Formátování kódu

```bash
# Formátování kódu
dotnet format
```

## Vytváření vlastních funkcí

### Příklad: Přidání vlastního kalendáře

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Vaše konverzní logika
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Zpětná konverze
        return new GregorianDate(year, month, day);
    }
}
```

### Příklad: Přidání vlastního exekutoru

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

### Příklad: Přidání vlastní šablony pracovního postupu

```csharp
public class MyWorkflowTemplate : WorkflowTemplate
{
    public override string Name => "my_workflow";
    public override string Description => "A custom workflow template";
    
    public override void DefineStates()
    {
        AddState("start", "Začátek", isInitial: true);
        AddState("processing", "Zpracování");
        AddState("review", "Kontrola");
        AddState("done", "Dokončeno", isFinal: true);
    }
    
    public override void DefineTransitions()
    {
        AddTransition("start", "processing", "Zahájit zpracování");
        AddTransition("processing", "review", "Odeslat ke kontrole");
        AddTransition("review", "done", "Kontrola schválena");
        AddTransition("review", "processing", "Kontrola zamítnuta");
    }
}
```

### Příklad: Přidání projektové role

Projektové role jsou spravovány pomocí operací `assign_role` a `remove_role` nástroje `ProjectTool`. Názvy rolí jsou vlastní řetězce používané k odlišení odpovědností Křemíkových Bytostí v pracovních postupech a přiřazování úkolů.

## Průvodce testováním

### Jednotkové testy

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Uspořádání
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object> 
            { 
                ["param1"] = "test" 
            }
        };
        
        // Provedení
        var result = await tool.ExecuteAsync(call);
        
        // Tvrzení
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### Integrační testy

Testování kompletního průběhu:
1. AI vrací volání nástroje
2. Nástroj se provede
3. Výsledek je předán AI
4. AI vrací konečnou odpověď

## Aspekty výkonu

### Úložný systém

- Verze Default používá souborové JSON úložiště
- Verze Fast používá SpeedyPack paměťový úložný engine (formát .spk)
- SpeedyPack využívá mapování adresářů v paměti + mezipaměť záznamů + asynchronní frontu zápisů
- Časově indexované dotazy používají rozhraní `ITimeStorage`

### Plánovač Hlavní Smyčky

- Hodinami řízené spravedlivé plánování časových slotů
- Hlídač pro detekci zaseknutých operací
- Jistič pro zabránění kaskádovým selháním

## Osvědčené postupy

### 1. Vždy ověřujte oprávnění

Jakákoliv operace iniciovaná AI musí projít řetězcem oprávnění:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return Result.Denied("Permission denied");
}
```

### 2. Používejte Lokátor Služeb

Globální registrace a vyhledávání služeb:

```csharp
// Během inicializace
ServiceLocator.Instance.Register<ICustomService>(myService);

// Při potřebě
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Dodržujte oddělení Tělo-Mozek

- Tělo zpracovává stav a spouštěče
- Mozek zpracovává AI interakce a provádění nástrojů

### 4. Implementujte odpovídající zpracování chyb

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

## Příručka přispívání

1. Forkněte repozitář
2. Vytvořte větev pro funkci (`git checkout -b feature/amazing-feature`)
3. Potvrďte změny pomocí konvenčních commitů
4. Pushněte do větve (`git push origin feature/amazing-feature`)
5. Otevřete Pull Request

### Formát commit zpráv

```
<typ>(<rozsah>): <popis>

Příklady:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Další kroky

- 📚 Přečtěte [příručku architektury](architecture.md)
- 📖 Prozkoumejte [API referenci](api-reference.md)
- 🔒 Prohlédněte [dokumentaci zabezpečení](security.md)
- 🚀 Prohlédněte [příručku rychlého startu](getting-started.md)
