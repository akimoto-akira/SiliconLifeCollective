# Przewodnik deweloperski

> **Wersja: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | [Русский](../ru-RU/development-guide.md)

## Przegląd architektury

SiliconLifeCollective stosuje **architekturę Ciało-Mózg**, z rygorystycznym rozdzieleniem głównych interfejsów i implementacji domyślnej.

### Struktura projektu

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfejsy, klasy abstrakcyjne, ogólna infrastruktura
│   ├── SiliconLife.Common/          # Współdzielone implementacje (wspólne dla obu wersji)
│   ├── SiliconLife.Default/         # Domyślna implementacja, punkt wejścia (weryfikacja wykonalności architektury)
│   ├── SiliconLife.Fast/            # Wysokowydajna implementacja, punkt wejścia (zalecana wersja produkcyjna)
│   ├── SiliconLife.Speedy/          # Wysokowydajny silnik przechowywania SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Narzędzie zarządzania SpeedyPack (Avalonia UI)
└── docs/                            # Dokumentacja wielojęzyczna
```

**Kierunek zależności**:
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core` (jednokierunkowe)

**Opis ról wersji**:
- **SiliconLife.Default**: domyślna implementacja, głównie do weryfikacji wykonalności architektury. Zapewnia prosta i niezawodną implementację przechowywania w systemie plików, odpowiednia do debugowania deweloperskiego i weryfikacji architektury.
- **SiliconLife.Fast**: zalecana wersja produkcyjna. Na podstawie architektury zweryfikowanej przez Default, wykorzystuje przechowywanie w pamięci SpeedyPack + asynchroniczną trwałość, zapewniając ekstremalną optymalizację wydajności, będąc preferowanym wyborem do długotrwałego działania i rzeczywistych środowisk produkcyjnych.

## Kluczowe koncepcje

### 1. Istota Krzemowa

Każdy agent AI składa się z:
- **Ciało** (`DefaultSiliconBeing`): utrzymuje stan życiowy, wykrywa scenariusze wyzwalania
- **Mózg** (`ContextManager`): ładuje historię, wywołuje AI, wykonuje narzędzia, utrwala odpowiedzi

### 2. System narzędzi

Narzędzia są automatycznie odkrywane i rejestrowane przez refleksję:

```csharp
// Wszystkie narzędzia implementują interfejs ITool
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. System uprawnień

3-poziomowy łańcuch weryfikacji uprawnień:
```
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → domyślnie odmowa)
```

### 4. Lokalizator usług

Globalna rejestracja i wyszukiwanie usług:
```csharp
// Rejestracja
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Pobieranie
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## System rozszerzeń

### Dodawanie nowego narzędzia

1. Utwórz nową klasę w `src/SiliconLife.Common/Tools/` (narzędzia współdzielone przez obie wersje):

> **Uwaga**: `SiliconLife.Default` i `SiliconLife.Fast` nie mają już niezależnych katalogów `Tools/`, wszystkie współdzielone narzędzia są umieszczane w `SiliconLife.Common/Tools/`.

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Parsowanie parametrów
        var param1 = call.Parameters["param1"]?.ToString();
        
        // Logika wykonania
        var result = await DoSomething(param1);
        
        // Zwrócenie wyniku
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. Narzędzie jest automatycznie odkrywane przez refleksję — brak konieczności ręcznej rejestracji!

3. (Opcjonalnie) Oznacz jako dostępne tylko dla administratora:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

4. (Opcjonalnie) Oznacz dostępne scenariusze narzędzia:
```csharp
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class MyTool : ITool { ... }
```

5. (Opcjonalnie) Oznacz jako dostępne tylko w scenariuszu czatu:
```csharp
[ChatOnly]
public class HelpTool : ITool { ... }
```

6. (Opcjonalnie) Oznacz jako dostępne tylko w scenariuszu projektu:
```csharp
[ToolScenario(ToolScenarioFlag.Project)]
[SiliconManagerOnly]
public class ProjectWorkTool : ITool { ... }
```

### Dodawanie nowego klienta AI

1. Zaimplementuj `IAIClient` w `src/SiliconLife.Common/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Wywołanie Twojego API AI
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
        // Implementacja przesyłania strumieniowego
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. Utwórz fabrykę:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. Fabryka jest automatycznie odkrywana i rejestrowana.

### Dodawanie nowego backendu przechowywania

1. Zaimplementuj `IStorage` i `ITimeStorage` w `src/SiliconLife.Default/Storage/` (implementacja systemu plików) lub `src/SiliconLife.Fast/Storage/` (adapter SpeedyPack):

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // Odczyt z Twojej bazy danych
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // Zapis do Twojej bazy danych
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // Zapytanie indeksowane czasowo
    }
}
```

### Dodawanie nowej wtyczki

1. Utwórz projekt biblioteki klas, implementujący interfejs `IPlugin`:

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

2. (Opcjonalnie) Zaimplementuj interfejs `ITool` we wtyczce, aby zarejestrować niestandardowe narzędzie:

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

3. Umieść skompilowany DLL w katalogu wtyczek, `PluginLoader` załaduje go automatycznie.

> **Ograniczenia bezpieczeństwa**: domyślnie wtyczki nie mogą odwoływać się do przestrzeni nazw takich jak `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`. Jednakże wtyczki mogą deklarować wymagane możliwości poprzez atrybut `[PluginCapability]` (Network, FileIO, Process, AI), a ładowarka na tej podstawie łagodzi reguły skanowania bezpieczeństwa odpowiednich przestrzeni nazw. Możliwości niezdeklarowalne (P/Invoke, Unsafe, Reflection Emit itp.) są zawsze blokowane. Wtyczki są ładowane izolowanie przez `AssemblyLoadContext`.

### Dodawanie nowej skórki

1. Zaimplementuj `ISkin` w `src/SiliconLife.App/Web/Skins/`:

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

2. Skórka jest automatycznie odkrywana przez `SkinManager`.

## Przewodnik stylu kodu

### Konwencje nazewnictwa

- **Klasy**: PascalCase, z przedrostkiem funkcjonalnym (np. `DefaultSiliconBeing`)
- **Interfejsy**: zaczynające się od `I` (np. `IAIClient`, `ITool`)
- **Implementacje**: kończące się nazwą interfejsu (np. `OllamaClient` implementuje `IAIClient`)
- **Narzędzia**: kończące się na `Tool` (np. `CalendarTool`, `ChatTool`)
- **Modele widoków**: kończące się na `ViewModel` (np. `BeingViewModel`)

### Organizacja kodu

```
SiliconLife.Common/
├── AI/                    # Implementacje klientów i fabryk AI (Ollama, DashScope, VolcengineArk, Herdsman, LongCat, QiniuAI, DeepSeek, Zhipu, Ernie, Hunyuan, MiniMax, Moonshot, SiliconFlow)
├── Calendar/              # 32 implementacje kalendarzy
├── Localization/          # Klasa bazowa lokalizacji i 34 implementacje wariantów językowych
├── Security/              # Menedżer Uprawnień
├── SiliconBeing/          # Domyślna implementacja Istoty Krzemowej
├── Tools/                 # Współdzielone narzędzia wbudowane (25)
├── Web/                   # Infrastruktura Web
└── WebView/               # Implementacja Playwright WebView

SiliconLife.App/          # Warstwa aplikacji współdzielona przez Default i Fast
├── Config/                # Konfiguracja aplikacji
├── Help/                  # Lokalizacja dokumentacji pomocy
├── Project/               # System projektowy (silnik przepływu pracy, role projektowe)
└── Web/                   # Implementacja Web UI
    ├── Component/         # 27 komponentów UI
    ├── Controllers/       # 24 kontrolery trasowania
    ├── Models/            # Modele widoków
    ├── Views/             # Widoki HTML
    └── Skins/             # 7 motywów skórek

SiliconLife.Default/      # Katalogi specyficzne dla wersji
├── Config/                # Domyślne dane konfiguracyjne
├── Knowledge/             # Implementacja Sieci Wiedzy
├── Logging/               # Implementacja dostawców logowania (konsola + system plików)
├── Project/               # Implementacja systemu projektów
└── Storage/               # Implementacja przechowywania w systemie plików

SiliconLife.Fast/         # Katalogi specyficzne dla wersji
├── Config/                # Dane konfiguracyjne wersji Fast
├── Logging/               # Implementacja dostawców logowania (konsola + system plików)
├── Storage/               # Adaptery przechowywania SpeedyPack
└── Tray/                  # Lokalizacja zasobnika systemowego
```

### Dokumentacja

- Wszystkie publiczne API muszą mieć komentarze dokumentacyjne XML
- Wszystkie pliki źródłowe używają nagłówka licencji Apache 2.0
- Wykorzystanie funkcji .NET 9 (niejawne using, typy referencyjne nullable)

## Przepływ pracy deweloperskiej

### 1. Konfiguracja środowiska deweloperskiego

```bash
# Klonowanie repozytorium
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Przywrócenie zależności
dotnet restore

# Budowanie
dotnet build
```

### 2. Uruchamianie testów

```bash
# Uruchomienie wszystkich testów
dotnet test

# Uruchomienie konkretnego projektu testowego
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Debugowanie

```bash
# Uruchomienie z wyjściem debugowania
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Formatowanie kodu

```bash
# Formatowanie kodu
dotnet format
```

## Budowanie niestandardowych funkcji

### Przykład: dodawanie niestandardowego kalendarza

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Twoja logika konwersji
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Konwersja odwrotna
        return new GregorianDate(year, month, day);
    }
}
```

### Przykład: dodawanie niestandardowego wykonawcy

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

### Przykład: dodawanie niestandardowego szablonu przepływu pracy

```csharp
public class MyWorkflowTemplate : WorkflowTemplate
{
    public override string Name => "my_workflow";
    public override string Description => "A custom workflow template";
    
    public override void DefineStates()
    {
        AddState("start", "Rozpoczęcie", isInitial: true);
        AddState("processing", "Przetwarzanie");
        AddState("review", "Przegląd");
        AddState("done", "Ukończenie", isFinal: true);
    }
    
    public override void DefineTransitions()
    {
        AddTransition("start", "processing", "Rozpoczęcie przetwarzania");
        AddTransition("processing", "review", "Przesłanie do przeglądu");
        AddTransition("review", "done", "Przegląd zatwierdzony");
        AddTransition("review", "processing", "Przegląd odrzucony");
    }
}
```

### Przykład: dodawanie roli projektowej

Role projektowe są zarządzane przez operacje `assign_role` i `remove_role` narzędzia `ProjectTool`. Nazwy ról są niestandardowymi ciągami znaków, używanymi do rozróżnienia obowiązków Istot Krzemowych w przepływach pracy i przypisywaniu zadań.

## Przewodnik testowania

### Testy jednostkowe

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Rozmieszczenie
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object> 
            { 
                ["param1"] = "test" 
            }
        };
        
        // Wykonanie
        var result = await tool.ExecuteAsync(call);
        
        // Asercja
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### Testy integracyjne

Testowanie pełnego przepływu:
1. AI zwraca wywołanie narzędzia
2. Narzędzie wykonuje się
3. Wynik jest przekazywany do AI
4. AI zwraca końcową odpowiedź

## Rozważania dotyczące wydajności

### System przechowywania

- Wersja Default wykorzystuje przechowywanie JSON oparte na plikach
- Wersja Fast wykorzystuje silnik przechowywania w pamięci SpeedyPack (format .spk)
- SpeedyPack stosuje mapowanie katalogów w pamięci + pamięć podręczną wpisów + asynchroniczną kolejkę zapisu
- Zapytania indeksowane czasowo wykorzystują interfejs `ITimeStorage`

### Planista pętli głównej

- Sprawiedliwe planowanie w szczelinach czasowych sterowane zegarem
- Timer watchdog do wykrywania zablokowanych operacji
- Circuit breaker do zapobiegania kaskadowym awariom

## Najlepsze praktyki

### 1. Zawsze weryfikuj uprawnienia

Każda operacja inicjowana przez AI musi przejść przez łańcuch uprawnień:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return Result.Denied("Permission denied");
}
```

### 2. Używaj lokalizatora usług

Globalna rejestracja i wyszukiwanie usług:

```csharp
// Podczas inicjalizacji
ServiceLocator.Instance.Register<ICustomService>(myService);

// Gdy potrzebne
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Przestrzegaj separacji Ciało-Mózg

- Ciało obsługuje stan i wyzwalacze
- Mózg obsługuje interakcję z AI i wykonywanie narzędzi

### 4. Implementuj odpowiednią obsługę błędów

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

## Przewodnik współpracy

1. Forkuj repozytorium
2. Utwórz gałąź funkcjonalną (`git checkout -b feature/amazing-feature`)
3. Zatwierdź zmiany używając konwencjonalnych commitów
4. Wypchnij do gałęzi (`git push origin feature/amazing-feature`)
5. Otwórz Pull Request

### Format komunikatów commitów

```
<type>(<scope>): <description>

Przykłady:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Następne kroki

- 📚 Przeczytaj [przewodnik architektury](architecture.md)
- 📖 Przeglądaj [referencję API](api-reference.md)
- 🔒 Zobacz [dokumentację bezpieczeństwa](security.md)
- 🚀 Zobacz [przewodnik szybkiego startu](getting-started.md)
