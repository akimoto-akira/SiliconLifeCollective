# Guida di sviluppo

> **Versione: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [Français](../fr-FR/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | **Italiano**

## Panoramica architettura

SiliconLifeCollective segue un'**architettura corpo-cervello**, con una separazione rigorosa tra le interfacce principali e le implementazioni predefinite.

### Struttura del progetto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfacce, classi astratte, infrastruttura comune
│   ├── SiliconLife.Common/          # Implementazioni condivise (comuni a entrambe le versioni)
│   ├── SiliconLife.Default/         # Implementazione predefinita, punto ingresso (verifica fattibilità)
│   ├── SiliconLife.Fast/            # Implementazione alte prestazioni, punto ingresso (versione produzione)
│   ├── SiliconLife.Speedy/          # Motore storage alte prestazioni SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Strumento gestione SpeedyPack (Windows Forms)
└── docs/                            # Documentazione multilingue
```

**Direzione dipendenze**:
- `SiliconLife.Default` → `SiliconLife.Core` (unidirezionale)
- `SiliconLife.Fast` → `SiliconLife.Core` (unidirezionale)
- `SiliconLife.Common` → `SiliconLife.Core` (unidirezionale)

**Descrizione ruoli versioni**:
- **SiliconLife.Default**: Implementazione predefinita, principalmente per verifica fattibilità architetturale. Fornisce un'implementazione di storage su file system semplice e affidabile, adatta al debug di sviluppo e alla verifica architetturale.
- **SiliconLife.Fast**: Versione principale di produzione. Sulla base dell'architettura verificata da Default, adotta lo storage in memoria SpeedyPack + persistenza asincrona, offrendo un'ottimizzazione delle prestazioni estrema, la scelta migliore per lo sfruttamento a lungo termine e i veri ambienti di produzione.

## Concetti fondamentali

### 1. Silicon Being

Ogni agente IA è composto da:
- **Corpo** (`DefaultSiliconBeing`): Mantiene lo stato di sopravvivenza, rileva gli scenari di attivazione
- **Cervello** (`ContextManager`): Carica la cronologia, chiama l'IA, esegue gli strumenti, persiste le risposte

### 2. Sistema di strumenti

Gli strumenti vengono automaticamente scoperti e registrati tramite riflessione:

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Sistema di permessi

Catena di verifica dei permessi a 5 livelli:
```
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL)
```

### 4. Localizzatore di servizi

Registrazione e recupero globale dei servizi:
```csharp
// Registrazione
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Recupero
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Sistema di estensione

### Aggiungere un nuovo strumento

1. Creare una nuova classe in `src/SiliconLife.Common/App/Tools/` (strumenti condivisi tra entrambe le versioni) o `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (strumenti specifici di versione):

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Descrizione di cosa fa questo strumento";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Analizzare parametri
        var param1 = call.Parameters["param1"]?.ToString();
        
        // Eseguire logica
        var result = await DoSomething(param1);
        
        // Restituire risultato
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. Lo strumento viene automaticamente scoperto tramite riflessione — nessuna registrazione manuale!

3. (Opzionale) Marcare come riservato agli amministratori:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### Aggiungere un nuovo client IA

1. Implementare `IAIClient` in `src/SiliconLife.Common/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Chiamare la tua API IA
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
        // Implementare lo streaming
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. Creare la factory:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. La factory viene automaticamente scoperta e registrata.

### Aggiungere un nuovo backend di storage

1. Implementare `IStorage` e `ITimeStorage` in `src/SiliconLife.Default/Storage/` (implementazione file system) o `src/SiliconLife.Fast/Storage/` (adattatore SpeedyPack):

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // Leggere dal tuo database
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // Scrivere nel tuo database
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // Query per indice temporale
    }
}
```

### Aggiungere un nuovo plugin

1. Creare un progetto libreria di classi, implementare l'interfaccia `IPlugin`:

```csharp
using SiliconLife.Collective;
using SiliconLife.Collective.Localization;
using SiliconLife.Collective.Tools;

public class MyPlugin : IPlugin
{
    public string Id => "my-plugin";
    public string Version => "1.0.0";
    
    public string GetName(Language language) => "My Plugin";
    public string GetDescription(Language language) => "Un plugin personalizzato";
    public string GetAuthor(Language language) => "Nome autore";
    
    public void OnLoad() { }
    public void OnStart() { }
    public void OnStop() { }
    public void OnUnload() { }
}
```

2. (Opzionale) Implementare l'interfaccia `ITool` nel plugin per registrare strumenti personalizzati:

```csharp
public class MyPluginTool : ITool
{
    public string Name => "my_plugin_tool";
    public string Description => "Uno strumento fornito dal mio plugin";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        return new ToolResult { Success = true, Output = "Completato" };
    }
}
```

3. Posizionare la DLL compilata nella directory dei plugin, `PluginLoader` la caricherà automaticamente.

> **Restrizioni sicurezza**: I plugin non possono referenziare i namespace `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`, ecc. I plugin vengono caricati in modo isolato tramite `AssemblyLoadContext`.

### Aggiungere un nuovo skin

1. Implementare `ISkin` in `src/SiliconLife.App/Web/Skins/`:

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "Descrizione di uno skin personalizzato";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
            }
            /* I tuoi stili personalizzati */
        ";
    }
}
```

2. Lo skin viene automaticamente scoperto da `SkinManager`.

## Guida dello stile codice

### Convenzioni di denominazione

- **Classi**: PascalCase, con prefisso funzionale (es. `DefaultSiliconBeing`)
- **Interfacce**: Iniziano con `I` (es. `IAIClient`, `ITool`)
- **Implementazioni**: Terminano con il nome dell'interfaccia (es. `OllamaClient` implementa `IAIClient`)
- **Strumenti**: Terminano con `Tool` (es. `CalendarTool`, `ChatTool`)
- **ViewModel**: Terminano con `ViewModel` (es. `BeingViewModel`)

### Organizzazione del codice

```
SiliconLife.Common/
├── AI/                    # Implementazioni client IA e factory
├── Calendar/              # 32 implementazioni calendari
├── Localization/          # Classe base localizzazione e 33 implementazioni linguistiche
├── Security/              # Permission manager
├── SiliconBeing/          # Implementazione Silicon Being predefinita
├── Tools/                 # Strumenti integrati condivisi
├── Web/                   # Infrastruttura Web
└── WebView/               # Implementazione Playwright WebView

SiliconLife.App/          # Livello applicativo condiviso tra Default e Fast
├── Config/                # Configurazione applicativa
├── Help/                  # Localizzazione documentazione aiuto
└── Web/                   # Implementazione interfaccia Web
    ├── Component/         # Libreria componenti UI
    ├── Controllers/       # Controller di routing
    ├── Models/            # ViewModel
    ├── Views/             # Viste HTML
    └── Skins/             # Temi skin

SiliconLife.Default/      # Directory specifiche della versione
├── Config/                # Dati configurazione predefiniti
├── IM/                    # Provider WebUI
├── Knowledge/             # Implementazione rete conoscenza
├── Logging/               # Implementazioni provider log
├── Project/               # Implementazione sistema progetti
├── Security/              # Callback permessi predefiniti
├── Storage/               # Implementazione storage su file system
└── Tools/                 # Strumenti specifici versione (HelpTool)
```

### Documentazione

- Tutte le API pubbliche devono avere commenti di documentazione XML
- Tutti i file sorgente utilizzano l'intestazione licenza Apache 2.0
- Sfruttare le funzionalità .NET 9 (using impliciti, tipi riferimento nullable)

## Flusso di lavoro sviluppo

### 1. Configurare ambiente sviluppo

```bash
# Clonare repository
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Restaurare dipendenze
dotnet restore

# Compilare
dotnet build
```

### 2. Eseguire test

```bash
# Eseguire tutti i test
dotnet test

# Eseguire un progetto test specifico
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Debbugare

```bash
# Eseguire con output debug
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Formattare codice

```bash
# Formattare codice
dotnet format
```

## Creare funzionalità personalizzate

### Esempio: Aggiungere calendario personalizzato

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // La tua logica di conversione
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Conversione inversa
        return new GregorianDate(year, month, day);
    }
}
```

### Esempio: Aggiungere esecutore personalizzato

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        // Prima verificare i permessi
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        // Eseguire operazione
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

## Guida test

### Test unitari

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Preparazione
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object> 
            { 
                ["param1"] = "test" 
            }
        };
        
        // Esecuzione
        var result = await tool.ExecuteAsync(call);
        
        // Verifica
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### Test integrazione

Testare il flusso completo:
1. L'IA restituisce una chiamata strumento
2. Lo strumento si esegue
3. Il risultato viene reinviato all'IA
4. L'IA restituisce la risposta finale

## Considerazioni prestazioni

### Sistema di storage

- La versione Default utilizza storage JSON basato su file
- La versione Fast utilizza il motore storage in memoria SpeedyPack (formato .spk)
- SpeedyPack adotta mappatura directory in memoria + cache voci + coda scrittura asincrona
- Le query per indice temporale utilizzano l'interfaccia `ITimeStorage`

### Scheduler loop principale

- Scheduling equo per time-slice basato su orologio
- Watchdog per rilevare operazioni bloccate
- Circuit breaker per prevenire guasti a cascata

## Buone pratiche

### 1. Verificare sempre i permessi

Qualsiasi operazione avviata dall'IA deve passare attraverso la catena di permessi:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
}
```

### 2. Utilizzare il localizzatore di servizi

Registrazione e recupero globale dei servizi:

```csharp
// Durante inizializzazione
ServiceLocator.Instance.Register<ICustomService>(myService);

// Quando necessario
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Seguire la separazione corpo-cervello

- Il corpo gestisce lo stato e le attivazioni
- Il cervello gestisce le interazioni IA e l'esecuzione degli strumenti

### 4. Implementare una corretta gestione errori

```csharp
try
{
    var result = await operation();
    return Result.Success(result);
}
catch (Exception ex)
{
    Logger.Error($"Operazione fallita: {ex.Message}");
    return Result.Failure(ex.Message);
}
```

## Guida contribuzione

1. Forkare il repository
2. Creare un branch per la funzionalità (`git checkout -b feature/amazing-feature`)
3. Committare le modifiche con commit convenzionali
4. Pushare verso il branch (`git push origin feature/amazing-feature`)
5. Aprire una Pull Request

### Formato messaggi commit

```
<type>(<ambito>): <descrizione>

Esempi:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Prossimi passi

- 📚 Leggere la [guida architettura](architecture.md)
