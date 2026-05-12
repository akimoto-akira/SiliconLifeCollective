# Przewodnik programistyczny

> **Wersja: v0.1.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | [Polski](../pl-PL/development-guide.md)

## Przegląd architektury

SiliconLifeCollective jest zgodny z **architekturą ciało-mózg**, z rygorystycznym oddzieleniem rdzennych interfejsów od domyślnej implementacji.

### Struktura projektu

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfejsy, klasy abstrakcyjne, ogólna infrastruktura
│   ├── SiliconLife.Common/          # Współdzielona implementacja (wspólna dla obu wersji)
│   ├── SiliconLife.Default/         # Domyślna implementacja, punkt wejścia (weryfikacja wykonalności architektury)
│   ├── SiliconLife.Fast/            # Wysokowydajna implementacja, punkt wejścia (główna wersja produkcyjna)
│   ├── SiliconLife.Speedy/          # Wysokowydajny silnik przechowywania SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Narzędzie zarządzania SpeedyPack (Windows Forms)
└── docs/                            # Dokumentacja wielojęzyczna
```

**Kierunek zależności**:
- `SiliconLife.Default` → `SiliconLife.Core` (jednokierunkowe)
- `SiliconLife.Fast` → `SiliconLife.Core` (jednokierunkowe)
- `SiliconLife.Common` → `SiliconLife.Core` (jednokierunkowe)

**Opis ról wersji**:
- **SiliconLife.Default**: domyślna implementacja, głównie do weryfikacji wykonalności architektury. Oferuje prostą i niezawodną implementację przechowywania w systemie plików, odpowiednią do debugowania rozwoju i weryfikacji architektury.
- **SiliconLife.Fast**: główna wersja produkcyjna. Na podstawie architektury zweryfikowanej przez Default, przyjmuje pamięć SpeedyPack + asynchroniczną trwałość, oferując ekstremalną optymalizację wydajności, preferowaną do długotrwałego działania i rzeczywistych środowisk produkcyjnych.

## Kluczowe koncepcje

### 1. Istota Krzemowa

Każdy agent AI składa się z:
- **Ciało** (`DefaultSiliconBeing`): utrzymuje stan życiowy, wykrywa scenariusze wyzwalające
- **Mózg** (`ContextManager`): ładuje historię, wywołuje AI, wykonuje narzędzia, utrwała odpowiedzi

### 2. System narzędzi

Narzędzia są automatycznie odkrywane i rejestrowane poprzez refleksję:

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

Łańcuch weryfikacji uprawnień 5 poziomów:
```
IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
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

1. Utwórz nową klasę w `src/SiliconLife.Common/Tools/` (narzędzia współdzielone przez obie wersje) lub `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (narzędzia specyficzne dla wersji):

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Opis tego, co to narzędzie robi";

    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Analiza parametrów
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

2. Narzędzia są automatycznie odkrywane poprzez refleksję — nie wymaga ręcznej rejestracji!

3. (Opcjonalnie) Oznacz jako dostępne tylko dla administratora:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
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
        // Implementacja strumieniowania
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
        // Zapytanie z indeksem czasowym
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

    public string GetName(Language language) => "Moja wtyczka";
    public string GetDescription(Language language) => "Niestandardowa wtyczka";
    public string GetAuthor(Language language) => "Imię Autora";

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
    public string Description => "Narzędzie dostarczane przez moją wtyczkę";

    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        return new ToolResult { Success = true, Output = "Gotowe" };
    }
}
```

3. Umieść skompilowany plik DLL w katalogu wtyczek, `PluginLoader` załaduje go automatycznie.

> **Ograniczenia bezpieczeństwa**: wtyczki nie mogą odwoływać się do przestrzeni nazw takich jak `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis` itp. Wtyczki są ładowane w izolacji przez `AssemblyLoadContext`.

### Dodawanie nowej skórki

1. Zaimplementuj `ISkin` w `src/SiliconLife.App/Web/Skins/`:

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "Opis niestandardowej skórki";

    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
            }
            /* Twoje niestandardowe style */
        ";
    }
}
```

2. Skórki są automatycznie odkrywane przez `SkinManager`.

## Przewodnik stylu kodu

### Konwencje nazewnictwa

- **Klasy**: PascalCase, z przedrostkiem funkcjonalnym (np. `DefaultSiliconBeing`)
- **Interfejsy**: zaczynają się od `I` (np. `IAIClient`, `ITool`)
- **Implementacje**: kończą się nazwą interfejsu (np. `OllamaClient` implementuje `IAIClient`)
- **Narzędzia**: kończą się na `Tool` (np. `CalendarTool`, `ChatTool`)
- **Modele widoków**: kończą się na `ViewModel` (np. `BeingViewModel`)

### Organizacja kodu

```
SiliconLife.Common/
├── AI/                    # Implementacje klientów i fabryk AI
├── Calendar/              # 32 implementacje kalendarzy
├── Localization/          # Klasy bazowe lokalizacji i 29 implementacji językowych
├── Security/              # Menedżer uprawnień
├── SiliconBeing/          # Domyślna implementacja istoty krzemowej
├── Tools/                 # Współdzielone wbudowane narzędzia
├── Web/                   # Infrastruktura Web
└── WebView/               # Implementacja Playwright WebView

SiliconLife.App/          # Warstwa aplikacji współdzielona przez Default i Fast
├── Config/                # Konfiguracja aplikacji
├── Help/                  # Lokalizacja dokumentacji pomocy
└── Web/                   # Implementacja Web UI
    ├── Component/         # Biblioteka komponentów UI
    ├── Controllers/       # Kontrolery routingu
    ├── Models/            # Modele widoków
    ├── Views/             # Widoki HTML
    └── Skins/             # Motywy skórek

SiliconLife.Default/      # Katalogi specyficzne dla wersji
├── Config/                # Domyślne dane konfiguracyjne
├── IM/                    # Dostawca WebUI
├── Knowledge/             # Implementacja sieci wiedzy
├── Logging/               # Implementacja dostawcy logowania
├── Project/               # Implementacja systemu projektów
├── Security/              # Domyślne wywołania zwrotne uprawnień
├── Storage/               # Implementacja przechowywania w systemie plików
└── Tools/                 # Narzędzia specyficzne dla wersji (HelpTool)
```

### Dokumentacja

- Wszystkie publiczne API muszą mieć komentarze dokumentacyjne XML
- Wszystkie pliki źródłowe używają nagłówka licencji Apache 2.0
- Wykorzystanie funkcji .NET 9 (niejawne using, typy referencyjne dopuszczające wartość null)

## Przepływ pracy programistycznej

### 1. Konfiguracja środowiska programistycznego

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

### Przykład: dodanie niestandardowego kalendarza

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
        // Odwrotna konwersja
        return new GregorianDate(year, month, day);
    }
}
```

### Przykład: dodanie niestandardowego wykonawcy

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";

    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        // Najpierw zweryfikuj uprawnienia
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }

        // Wykonaj operację
        var result = await PerformOperation(request);

        return ExecutorResult.Success(result);
    }
}
```

## Przewodnik testowania

### Testy jednostkowe

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Przygotowanie
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
2. Narzędzie jest wykonywane
3. Wynik jest przekazywany z powrotem do AI
4. AI zwraca ostateczną odpowiedź

## Rozważania dotyczące wydajności

### System przechowywania

- Wersja Default używa przechowywania JSON opartego na plikach
- Wersja Fast używa silnika przechowywania w pamięci SpeedyPack (format .spk)
- SpeedyPack stosuje mapowanie katalogów w pamięci + pamięć podręczna wpisów + asynchroniczna kolejka zapisu
- Zapytania z indeksem czasowym używają interfejsu `ITimeStorage`

### Harmonogramator pętli głównej

- Sprawiedliwe planowanie oparte na czasowych plasterkach zegara
- Timer watchog do wykrywania zablokowanych operacji
- Bezpiecznik zapobiegający kaskadowym awariom

## Najlepsze praktyki

### 1. Zawsze weryfikuj uprawnienia

Każda operacja zainicjowana przez AI musi przejść przez łańcuch uprawnień:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
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

### 3. Przestrzegaj separacji ciało-mózg

- Ciało obsługuje stan i wyzwalacze
- Mózg obsługuje interakcje AI i wykonywanie narzędzi

### 4. Implementuj odpowiednią obsługę błędów

```csharp
try
{
    var result = await operation();
    return Result.Success(result);
}
catch (Exception ex)
{
    Logger.Error($"Operacja nie powiodła się: {ex.Message}");
    return Result.Failure(ex.Message);
}
```

## Przewodnik współtworzenia

1. Sforkuj repozytorium
2. Utwórz gałąź funkcjonalną (`git checkout -b feature/amazing-feature`)
3. Zatwierdź swoje zmiany używając konwencjonalnych commitów
4. Wypchnij do gałęzi (`git push origin feature/amazing-feature`)
5. Otwórz żądanie pull

### Format komunikatów commitów

```
<typ>(<zakres>): <opis>

Przykłady:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Następne kroki

- 📚 Przeczytaj [przewodnik po architekturze](architecture.md)
- 📖 Poznaj [referencję API](api-reference.md)
- 🔒 Zobacz [dokumentację bezpieczeństwa](security.md)
- 🚀 Zobacz [przewodnik szybkiego startu](getting-started.md)
