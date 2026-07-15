# Guida allo Sviluppo

> **Versione: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | [Русский](../ru-RU/development-guide.md) | **Italiano**

## Panoramica dell'Architettura

SiliconLifeCollective segue un'**architettura Corpo-Cervello**, con separazione rigorosa tra interfacce core e implementazioni predefinite.

### Struttura del Progetto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfacce, classi astratte, infrastruttura generica
│   ├── SiliconLife.Common/          # Implementazioni condivise (comuni a entrambe le versioni)
│   ├── SiliconLife.Default/         # Implementazione predefinita, punto di ingresso (validazione architettura)
│   ├── SiliconLife.Fast/            # Implementazione ad alte prestazioni, punto di ingresso (versione di produzione)
│   ├── SiliconLife.Speedy/          # Motore di archiviazione SpeedyPack ad alte prestazioni
│   └── SiliconLife.Speedy.Manager/  # Strumento di gestione SpeedyPack (Avalonia UI)
└── docs/                            # Documentazione multilingua
```

**Direzione delle dipendenze**:
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core` (unidirezionale)

**Descrizione dei ruoli delle versioni**:
- **SiliconLife.Default**: Implementazione predefinita, utilizzata principalmente per verificare la fattibilità dell'architettura. Fornisce un'implementazione di archiviazione su file system semplice e affidabile, adatta per lo sviluppo, il debug e la validazione dell'architettura.
- **SiliconLife.Fast**: Versione di produzione raccomandata. Sulla base dell'architettura validata da Default, adotta l'archiviazione in memoria SpeedyPack + persistenza asincrona, offrendo ottimizzazioni estreme delle prestazioni, ed è la scelta preferita per l'esecuzione a lungo termine e gli ambienti di produzione reali.

## Concetti Fondamentali

### 1. Essere di Silicio

Ogni agente AI è composto da:
- **Corpo** (`DefaultSiliconBeing`): Mantiene lo stato vitale, rileva gli scenari di attivazione
- **Cervello** (`ContextManager`): Carica la cronologia, chiama l'AI, esegue gli strumenti, persiste le risposte

### 2. Sistema degli Strumenti

Gli strumenti sono automaticamente scoperti e registrati tramite reflection:

```csharp
// Tutti gli strumenti implementano l'interfaccia ITool
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Sistema dei Permessi

Catena di verifica dei permessi a 3 livelli:
```
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curatore: GlobalACL → rifiuto predefinito)
```

### 4. Localizzatore di Servizi

Registrazione e recupero globale dei servizi:
```csharp
// Registrazione
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Recupero
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Sistema di Estensione

### Aggiungere un Nuovo Strumento

1. Crea una nuova classe in `src/SiliconLife.Common/Tools/` (strumento condiviso tra le versioni):

> **Nota**: `SiliconLife.Default` e `SiliconLife.Fast` non hanno più directory `Tools/` indipendenti; tutti gli strumenti condivisi sono collocati uniformemente in `SiliconLife.Common/Tools/`.

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Descrizione di ciò che fa questo strumento";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Analisi dei parametri
        var param1 = call.Parameters["param1"]?.ToString();
        
        // Logica di esecuzione
        var result = await DoSomething(param1);
        
        // Restituzione del risultato
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. Lo strumento viene automaticamente scoperto tramite reflection - nessuna registrazione manuale necessaria!

3. (Opzionale) Contrassegna come solo amministratore:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

4. (Opzionale) Contrassegna gli scenari disponibili dello strumento:
```csharp
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class MyTool : ITool { ... }
```

5. (Opzionale) Contrassegna come disponibile solo nello scenario chat:
```csharp
[ChatOnly]
public class HelpTool : ITool { ... }
```

6. (Opzionale) Contrassegna come disponibile solo nello scenario progetto:
```csharp
[ToolScenario(ToolScenarioFlag.Project)]
[SiliconManagerOnly]
public class ProjectWorkTool : ITool { ... }
```

### Aggiungere un Nuovo Client AI

1. Implementa `IAIClient` in `src/SiliconLife.Common/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Chiama la tua API AI
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
        // Implementa lo streaming
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. Crea una factory:

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

### Aggiungere un Nuovo Backend di Archiviazione

1. Implementa `IStorage` e `ITimeStorage` in `src/SiliconLife.Default/Storage/` (implementazione file system) o `src/SiliconLife.Fast/Storage/` (adattatore SpeedyPack):

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // Leggi dal tuo database
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // Scrivi nel tuo database
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // Query con indice temporale
    }
}
```

### Aggiungere un Nuovo Plugin

1. Crea un progetto di libreria di classi che implementi l'interfaccia `IPlugin`:

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
    public string GetAuthor(Language language) => "Nome Autore";
    
    public void OnLoad() { }
    public void OnStart() { }
    public void OnStop() { }
    public void OnUnload() { }
}
```

2. (Opzionale) Implementa l'interfaccia `ITool` nel plugin per registrare strumenti personalizzati:

```csharp
public class MyPluginTool : ITool
{
    public string Name => "my_plugin_tool";
    public string Description => "Uno strumento fornito dal mio plugin";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        return new ToolResult { Success = true, Output = "Done" };
    }
}
```

3. Posiziona la DLL compilata nella directory dei plugin; `PluginLoader` la caricherà automaticamente.

> **Limitazioni di sicurezza**: Per impostazione predefinita, i plugin non possono fare riferimento ai namespace `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`, ecc. Tuttavia, se un plugin dichiara le capacità necessarie (Network, FileIO, Process, AI) tramite l'attributo `[PluginCapability]`, il caricatore riduce le regole di scansione di sicurezza di conseguenza. Le capacità non dichiarabili (P/Invoke, Unsafe, Reflection Emit, ecc.) sono sempre bloccate. I plugin vengono caricati in isolamento tramite `AssemblyLoadContext`.

### Aggiungere una Nuova Skin

1. Implementa `ISkin` in `src/SiliconLife.App/Web/Skins/`:

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "Descrizione di una skin personalizzata";
    
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

2. La skin viene automaticamente scoperta da `SkinManager`.

## Guida allo Stile del Codice

### Convenzioni di Nomenclatura

- **Classi**: PascalCase, con prefisso funzionale (es. `DefaultSiliconBeing`)
- **Interfacce**: Iniziano con `I` (es. `IAIClient`, `ITool`)
- **Implementazioni**: Terminano con il nome dell'interfaccia (es. `OllamaClient` implementa `IAIClient`)
- **Strumenti**: Terminano con `Tool` (es. `CalendarTool`, `ChatTool`)
- **Modelli di vista**: Terminano con `ViewModel` (es. `BeingViewModel`)

### Organizzazione del Codice

```
SiliconLife.Common/
├── AI/                    # Client e factory AI (Ollama, DashScope, VolcengineArk, Herdsman, LongCat, QiniuAI, DeepSeek, Zhipu, Ernie, Hunyuan, MiniMax, Moonshot, SiliconFlow)
├── Calendar/              # 32 implementazioni calendariali
├── Localization/          # Classi base di localizzazione e 34 varianti linguistiche
├── Security/              # Gestore dei permessi
├── SiliconBeing/          # Implementazione predefinita dell'Essere di Silicio
├── Tools/                 # Strumenti integrati condivisi (25)
├── Web/                   # Infrastruttura Web
└── WebView/               # Implementazione Playwright WebView

SiliconLife.App/          # Livello applicativo condiviso tra Default e Fast
├── Config/                # Configurazione applicazione
├── Help/                  # Localizzazione della documentazione di aiuto
├── Project/               # Sistema di progetto (motore workflow, ruoli di progetto)
└── Web/                   # Implementazione Web UI
    ├── Component/         # 27 componenti UI
    ├── Controllers/       # 24 controller di routing
    ├── Models/            # Modelli di vista
    ├── Views/             # Viste HTML
    └── Skins/             # 7 temi skin

SiliconLife.Default/      # Directory specifica della versione
├── Config/                # Dati di configurazione predefiniti
├── Knowledge/             # Implementazione della rete di conoscenza
├── Logging/               # Implementazione del provider di log (console + file system)
├── Project/               # Implementazione del sistema di progetto
└── Storage/               # Implementazione dell'archiviazione su file system

SiliconLife.Fast/         # Directory specifica della versione
├── Config/                # Dati di configurazione della versione Fast
├── Logging/               # Implementazione del provider di log (console + file system)
├── Storage/               # Adattatori di archiviazione SpeedyPack
└── Tray/                  # Localizzazione dell'area di notifica di sistema
```

### Documentazione

- Tutte le API pubbliche devono avere commenti di documentazione XML
- Tutti i file sorgente utilizzano l'intestazione della licenza Apache 2.0
- Sfruttare le funzionalità di .NET 9 (using impliciti, tipi di riferimento nullable)

## Flusso di Lavoro di Sviluppo

### 1. Configurazione dell'Ambiente di Sviluppo

```bash
# Clona il repository
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Ripristina le dipendenze
dotnet restore

# Compila
dotnet build
```

### 2. Esecuzione dei Test

```bash
# Esegui tutti i test
dotnet test

# Esegui un progetto di test specifico
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Debug

```bash
# Esegui con output di debug
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Formattazione del Codice

```bash
# Formatta il codice
dotnet format
```

## Costruire Funzionalità Personalizzate

### Esempio: Aggiungere un Calendario Personalizzato

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

### Esempio: Aggiungere un Esecutore Personalizzato

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

### Esempio: Aggiungere un Template di Workflow Personalizzato

```csharp
public class MyWorkflowTemplate : WorkflowTemplate
{
    public override string Name => "my_workflow";
    public override string Description => "Un template di workflow personalizzato";
    
    public override void DefineStates()
    {
        AddState("start", "Inizio", isInitial: true);
        AddState("processing", "Elaborazione");
        AddState("review", "Revisione");
        AddState("done", "Completato", isFinal: true);
    }
    
    public override void DefineTransitions()
    {
        AddTransition("start", "processing", "Inizia elaborazione");
        AddTransition("processing", "review", "Invia per revisione");
        AddTransition("review", "done", "Revisione approvata");
        AddTransition("review", "processing", "Revisione respinta");
    }
}
```

### Esempio: Aggiungere un Ruolo di Progetto

I ruoli di progetto vengono gestiti tramite le operazioni `assign_role` e `remove_role` di `ProjectTool`. I nomi dei ruoli sono stringhe personalizzate utilizzate per distinguere le responsabilità degli Esseri di Silicio nei workflow e nell'assegnazione delle attività.

## Guida ai Test

### Test Unitari

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

### Test di Integrazione

Testa il flusso completo:
1. L'AI restituisce una chiamata strumento
2. Lo strumento viene eseguito
3. Il risultato viene restituito all'AI
4. L'AI restituisce la risposta finale

## Considerazioni sulle Prestazioni

### Sistema di Archiviazione

- La versione Default utilizza l'archiviazione JSON basata su file
- La versione Fast utilizza il motore di archiviazione in memoria SpeedyPack (formato .spk)
- SpeedyPack adotta la mappa directory in memoria + cache delle voci + coda di scrittura asincrona
- Le query con indice temporale utilizzano l'interfaccia `ITimeStorage`

### Pianificatore del Ciclo Principale

- Pianificazione equa basata su slot temporali guidati da clock
- Timer watchdog per rilevare operazioni bloccate
- Interruttore per prevenire fallimenti a cascata

## Best Practices

### 1. Verificare Sempre i Permessi

Qualsiasi operazione avviata dall'AI deve passare attraverso la catena dei permessi:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return Result.Denied("Permission denied");
}
```

### 2. Utilizzare il Localizzatore di Servizi

Registra e recupera servizi a livello globale:

```csharp
// Durante l'inizializzazione
ServiceLocator.Instance.Register<ICustomService>(myService);

// Quando necessario
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Seguire la Separazione Corpo-Cervello

- Il corpo gestisce lo stato e i trigger
- Il cervello gestisce l'interazione AI e l'esecuzione degli strumenti

### 4. Implementare una Gestione degli Errori Appropriata

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

## Guida ai Contributi

1. Fai fork del repository
2. Crea un branch di funzionalità (`git checkout -b feature/amazing-feature`)
3. Committa le tue modifiche utilizzando commit convenzionali
4. Pusha sul branch (`git push origin feature/amazing-feature`)
5. Apri una Pull Request

### Formato dei Messaggi di Commit

```
<type>(<scope>): <description>

Esempi:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Prossimi Passi

- 📚 Leggi la [Guida all'Architettura](architecture.md)
- 📖 Esplora il [Riferimento API](api-reference.md)
- 🔒 Consulta la [Documentazione sulla Sicurezza](security.md)
- 🚀 Consulta la [Guida Rapida](getting-started.md)
