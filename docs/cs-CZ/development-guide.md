# Vývojový Průvodce

> **Verze: v0.1.0-alpha**

[English](../en/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | **Čeština**

## Přehled architektury

SiliconLifeCollective následuje **architekturu tělo-mozek** s přísným oddělením základních rozhraní a výchozích implementací.

### Struktura projektu

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # Rozhraní, abstraktní třídy, obecná infrastruktura
│   └── SiliconLife.Default/   # Konkrétní implementace, vstupní body
└── docs/                      # Vícejazyčná dokumentace
```

**Směr závislosti**: `SiliconLife.Default` → `SiliconLife.Core` (jednosměrný)

## Základní koncepty

### 1. Silikonová bytost (Silikonová bytost)

Každý AI agent se skládá z:
- **Tělo** (`DefaultSiliconBeing`): Udržuje stav存活, detekuje spouštěcí scénáře
- **Mozek** (`ContextManager`): Načítá historii, volá AI, provádí nástroje, persistuje odpovědi

### 2. Systém nástrojů

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

### 3. Systém oprávnění

5-úrovňový řetězec ověřování oprávnění:
```
IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
```

### 4. Service Locator

Globální registrace a načítání služeb:
```csharp
// Registrace
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Načtení
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Rozšíření systému

### Přidání nového nástroje

1. Vytvořte novou třídu v `src/SiliconLife.Default/Tools/`:

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Popis toho, co tento nástroj dělá";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Analyzujte parametry
        var param1 = call.Parameters["param1"]?.ToString();
        
        // Proveďte logiku
        var result = await DoSomething(param1);
        
        // Vrátíte výsledek
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. Nástroje jsou automaticky objevovány reflexí - žádná manuální registrace není potřeba!

3. (Volitelné) Označte jako pouze pro správce:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### Přidání nového AI klienta

1. Implementujte `IAIClient` v `src/SiliconLife.Default/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Zavolejte své AI API
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
        // Implementujte streamování
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

3. Továrnou je automaticky objevována a registrována.

### Přidání nového backendu úložiště

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

### Přidání nového skinu

1. Implementujte `ISkin` v `src/SiliconLife.Default/Web/Skins/`:

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "Popis vlastního skinu";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #vaše-barva;
                --bg-color: #vaše-pozadí;
            }
            /* Vaše vlastní styly */
        ";
    }
}
```

2. Skiny jsou automaticky objevovány `SkinManager`.

## Pravidla stylu kódu

### Konvence pojmenování

- **Třídy**: PascalCase s funkčním prefixem (např. `DefaultSiliconBeing`)
- **Rozhraní**: Začínají `I` (např. `IAIClient`, `ITool`)
- **Implementace**: Končí názvem rozhraní (např. `OllamaClient` implementuje `IAIClient`)
- **Nástroje**: Končí `Tool` (např. `CalendarTool`, `ChatTool`)
- **View modely**: Končí `ViewModel` (např. `BeingViewModel`)

### Organizace kódu

```
SiliconLife.Default/
├── AI/                    # Implementace AI klientů
├── Calendar/              # Implementace kalendářů
├── Config/                # Výchozí konfigurační data
├── Executors/             # Implementace exekutorů
├── IM/                    # Implementace poskytovatelů IM
├── Localization/          # Implementace lokalizace
├── Logging/               # Implementace poskytovatelů logů
├── Runtime/               # Komponenty runtime
├── Security/              # Implementace zabezpečení
├── SiliconBeing/          # Výchozí implementace silikonových bytostí
├── Storage/               # Implementace úložiště
├── Tools/                 # Vestavěné nástroje
└── Web/                   # Implementace Web UI
    ├── Controllers/       # Řadiče routování
    ├── Models/            # View modely
    ├── Views/             # HTML pohledy
    └── Skins/             # Témata skinů
```

### Dokumentace

- Všechna veřejná API musí mít XML dokumentační komentáře
- Všechny zdrojové soubory používají záhlaví licence Apache 2.0
- Využijte funkce .NET 9 (implicitní using, nullable reference typy)

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
# Spuštění s výstupem ladění
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
        // Vaše logika převodu
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Reverzní převod
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

## Testovací pravidla

### Unit testy

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
        
        // Akce
        var result = await tool.ExecuteAsync(call);
        
        // Kontrola
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### Integrační testy

Testování kompletního toku:
1. AI vrací volání nástroje
2. Provedení nástroje
3. Výsledek je vrácen AI
4. AI vrací finální odpověď

## Úvahy o výkonu

### Systém úložiště

- Systém úložiště upřednostňuje **funkčnost před výkonem**
- Výchozí používá JSON úložiště založené na souborech
- Dotazy s časovým indexem používají strukturu adresářů

### Hlavní smyčkový scheduler

- Fair scheduling založený na časových řezech hodin
- Watchdog timer pro detekci zamrznutých operací
- Circuit breaker pro prevenci kaskádových selhání

## Nejlepší postupy

### 1. Vždy ověřujte oprávnění

Jakákoli operace iniciovaná AI musí projít řetězcem oprávnění:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
}
```

### 2. Používejte Service Locator

Globální registrace a načítání služeb:

```csharp
// Během inicializace
ServiceLocator.Instance.Register<ICustomService>(myService);

// Když je potřeba
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Následujte oddělení tělo-mozek

- Tělo zpracovává stav a spouštěče
- Mozek zpracovává AI interakce a provádění nástrojů

### 4. Implementujte správné zpracování chyb

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

## Pravidla příspěvků

1. Forkněte repozitář
2. Vytvořte větev funkce (`git checkout -b feature/amazing-feature`)
3. Commitněte své změny s konvenčními commity
4. Pushněte do větve (`git push origin feature/amazing-feature`)
5. Otevřete Pull Request

### Formát zpráv commitu

```
<type>(<scope>): <description>

Příklady:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Další kroky

- 📚 Přečtěte si [Průvodce architekturou](architecture.md)
- 📖 Prozkoumejte [API Reference](api-reference.md)
- 🔒 Podívejte se na [Bezpečnostní dokumentaci](security.md)
- 🚀 Podívejte se na [Průvodce rychlým startem](getting-started.md)
