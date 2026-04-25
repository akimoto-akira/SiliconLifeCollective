# Vývojový Průvodce

[English](../en/development-guide.md) | [中文文档](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md)

## Přehled Architektury

SiliconLifeCollective následuje **architekturu Tělo-Mozek** s přísným oddělením core rozhraní a výchozích implementací.

### Struktura Projektu

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # Rozhraní, abstraktní třídy, sdílená infrastruktura
│   └── SiliconLife.Default/   # Konkrétní implementace, vstupní bod
└── docs/                      # Vícejazyčná dokumentace
```

**Směr závislosti**: `SiliconLife.Default` → `SiliconLife.Core` (jednosměrný)

## Core Koncepty

### 1. Křemíková Bytost

Každý AI agent se skládá z:
- **Tělo** (`DefaultSiliconBeing`): Udržuje stav naživu, detekuje spouštěcí scénáře
- **Mozek** (`ContextManager`): Načítá historii, volá AI, provádí nástroje, persistuje odpovědi

### 2. Systém Nástrojů

Nástroje jsou automaticky objevovány a registrovány prostřednictvím reflexe:

```csharp
// Všechny nástroje implementují rozhraní ITool
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Systém Oprávnění

5-úrovňový řetězec ověřování oprávnění:
```
IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
```

### 4. Service Locator

Globální registrace a získávání služeb:
```csharp
// Registrace
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Získání
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Rozšíření Systému

### Přidání Nového Nástroje

1. Vytvořte novou třídu v `src/SiliconLife.Default/Tools/`:

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Popis toho, co tento nástroj dělá";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Parsování parametrů
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

2. Nástroj je automaticky objeven prostřednictvím reflexe - žádná manuální registrace!

3. (Volitelné) Označte jako pouze pro správce:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### Přidání Nového AI Klienta

1. Implementujte `IAIClient` v `src/SiliconLife.Default/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Zavolejte vaše AI API
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

2. Vytvořte factory:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. Factory je automaticky objevena a registrována.

### Přidání Nového Storage Backend

1. Implementujte `IStorage` a `ITimeStorage` v `src/SiliconLife.Default/Storage/`:

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
        // Dotaz s časovým indexem
    }
}
```

### Přidání Nového Skinu

1. Implementujte `ISkin` v `src/SiliconLife.Default/Web/Skins/`:

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "Popis custom skinu";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #vas-barva;
                --bg-color: #vas-bg;
            }
            /* Vaše custom styly */
        ";
    }
}
```

2. Skin je automaticky objeven `SkinManager`.

## Průvodce Stylem Kódu

### Konvence Pojmenování

- **Třídy**: PascalCase s funkčním prefixem (např. `DefaultSiliconBeing`)
- **Rozhraní**: Začíná `I` (např. `IAIClient`, `ITool`)
- **Implementace**: Končí názvem rozhraní (např. `OllamaClient` implementuje `IAIClient`)
- **Nástroje**: Končí `Tool` (např. `CalendarTool`, `ChatTool`)
- **ViewModely**: Končí `ViewModel` (např. `BeingViewModel`)

### Organizace Kódu

```
SiliconLife.Default/
├── AI/                    # Implementace AI klientů
├── Calendar/              # Implementace kalendářů
├── Config/                # Výchozí konfigurační data
├── Executors/             # Implementace executorů
├── IM/                    # Implementace IM providerů
├── Localization/          # Implementace lokalizace
├── Logging/               # Implementace logging providerů
├── Runtime/               # Runtime komponenty
├── Security/              # Implementace bezpečnosti
├── SiliconBeing/          # Výchozí implementace křemíkových bytostí
├── Storage/               # Implementace úložiště
├── Tools/                 # Vestavěné nástroje
└── Web/                   # Implementace Web UI
    ├── Controllers/       # Routovací controllery
    ├── Models/            # ViewModely
    ├── Views/             # HTML pohledy
    └── Skins/             # Témata skinů
```

### Dokumentace

- Všechna veřejná API musí mít XML dokumentační komentáře
- Všechny zdrojové soubory používají Apache 2.0 licenci v hlavičce
- Využívejte .NET 9 funkce (implicitní using, nullable reference typy)

## Vývojový Workflow

### 1. Nastavení Vývojového Prostředí

```bash
# Klonování repozitáře
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Obnovení závislostí
dotnet restore

# Build
dotnet build
```

### 2. Spuštění Testů

```bash
# Spuštění všech testů
dotnet test

# Spuštění konkrétního testovacího projektu
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Debugging

```bash
# Spuštění v debug output
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Formátování Kódu

```bash
# Formátování kódu
dotnet format
```

## Build Custom Funkcí

### Příklad: Přidání Custom Kalendáře

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Vaše převodní logika
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Reverzní převod
        return new GregorianDate(year, month, day);
    }
}
```

### Příklad: Přidání Custom Executoru

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        // Nejprve ověřte oprávnění
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        // Proveďte operaci
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

## Testovací Průvodce

### Unit Testy

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

### Integrační Testy

Testujte kompletní workflow:
1. AI vrací volání nástroje
2. Provedení nástroje
3. Výsledek vrácen AI
4. AI vrací finální odpověď

## Výkonové Úvahy

### Storage Systém

- Storage systém upřednostňuje **funkcionalitu před výkonem**
- Výchozí použití JSON úložiště založeného na souborech
- Dotazy s časovým indexem používají strukturu adresářů

### Hlavní Smyčka Scheduler

- Clock-driven time-slice fair scheduling
- Watchdog timer pro detekci zaseknutých operací
- Jistič pro prevenci kaskádových selhání

## Best Practices

### 1. Vždy Ověřujte Oprávnění

Jakákoliv AI iniciovaná operace musí procházet řetězcem oprávnění:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
}
```

### 2. Používejte Service Locator

Globální registrace a získávání služeb:

```csharp
// Během inicializace
ServiceLocator.Instance.Register<ICustomService>(myService);

// Když je potřeba
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Následujte Oddělení Tělo-Mozek

- Tělo zpracovává stav a spouštění
- Mozek zpracovává AI interakce a provádění nástrojů

### 4. Implementujte Správné Zpracování Chyb

```csharp
try
{
    var result = await operation();
    return Result.Success(result);
}
catch (Exception ex)
{
    Logger.Error($"Operace selhala: {ex.Message}");
    return Result.Failure(ex.Message);
}
```

## Přispívání

1. Forkněte repozitář
2. Vytvořte feature branch (`git checkout -b feature/amazing-feature`)
3. Commitněte vaše změny s conventional commits
4. Pushněte na branch (`git push origin feature/amazing-feature`)
5. Otevřete Pull Request

### Formát Commit Zpráv

```
<type>(<scope>): <description>

Příklady:
feat(tool): přidání custom kalendářového nástroje
fix(permission): oprava null pointer v callbacku
docs: aktualizace vývojového průvodce
```

## Další Kroky

- 📚 Přečtěte si [Průvodce Architektury](architecture.md)
- 📖 Prozkoumejte [API Reference](api-reference.md)
- 🔒 Podívejte se na [Dokumentaci Bezpečnosti](security.md)
- 🚀 Podívejte se na [Rychlý Start](getting-started.md)
