# Roadmap

> **Versione: v0.2.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md) | [Русский](../ru-RU/roadmap.md) | **Italiano**

## Roadmap della Doppia Versione

### SiliconLife.Default (Versione Predefinita)
- **Posizionamento**: Implementazione predefinita, utilizzata principalmente per verificare la fattibilità dell'architettura
- **Stato attuale**: Fasi 1-10.6 completate, sistema stabile e funzionante
- **Descrizione del ruolo**: Come implementazione di riferimento per la validazione dell'architettura, garantisce la correttezza e la fattibilità del design architetturale core

### SiliconLife.Fast (Versione ad Alte Prestazioni)
- **Posizionamento**: Versione di produzione raccomandata
- **Stato attuale**: Trapianto dell'architettura di base completato, motore di archiviazione SpeedyPack e sistema di plugin implementati
- **Descrizione del ruolo**: Sulla base dell'architettura validata dalla versione Default, esegue ottimizzazioni profonde delle prestazioni e miglioramenti di livello produttivo, ed è la scelta preferita per il deployment effettivo

**Piano di sviluppo della versione Fast**:
- ✅ Fase 1: Struttura del progetto di base e trapianto del sistema di configurazione
- ✅ Fase 2: Trapianto della Web UI e dei controller
- ✅ Fase 3: Ottimizzazione del sistema di archiviazione (archiviazione in memoria SpeedyPack + persistenza asincrona)
- ✅ Fase 3.5: Strumento di gestione SpeedyPack (applicazione Avalonia UI SiliconLife.Speedy.Manager)
- ✅ Fase 3.6: Sistema di plugin (interfaccia IPlugin, sandbox di sicurezza, isolamento AssemblyLoadContext)
- ✅ Fase 4: Applicazione desktop Avalonia (applicazione desktop multipiattaforma, area di notifica Windows/macOS, finestra di stato Linux)

---

## Principi Guida

Ogni fase si conclude con un sistema **eseguibile e osservabile**. Nessuna fase produce "un mucchio di infrastruttura senza nulla da mostrare".

---

## ~~Fase 1: Possiamo chattare~~ ✅ Completata

**Obiettivo**: Input da console → chiamata AI → output su console. Unità minima verificabile.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 1.1 | Soluzione e struttura del progetto | Creazione di `SiliconLifeCollective.sln`, con `src/SiliconLife.Core/` (libreria core) e `src/SiliconLife.Default/` (implementazione predefinita + punto di ingresso) |
| 1.2 | Configurazione (minima) | Singleton + deserializzazione JSON. Legge `config.json`. Genera automaticamente i valori predefiniti se mancante |
| 1.3 | Localizzazione (minima) | Classe astratta `LocalizationBase`, implementazione `ZhCN`. Aggiunta di `Language` nella configurazione |
| 1.4 | OllamaClient (minimo) | Interfaccia `IAIClient`, chiamata HTTP a Ollama locale `/api/chat`. Nessuno streaming, nessuna chiamata strumento |
| 1.5 | I/O console | `while(true) + Console.ReadLine()`, legge input → chiama AI → stampa risposta |
| 1.6 | Intestazione copyright | Aggiunta dell'intestazione Apache 2.0 a tutti i file sorgente C# |

**Deliverable**: Programma di chat da console che dialoga con un modello Ollama locale.

**Verifica**: Eseguire il programma, inserire "ciao", vedere la risposta AI.

---

## ~~Fase 2: C'è uno scheletro~~ ✅ Completata

**Obiettivo**: Sostituire il "ciclo nudo" con una struttura a framework. Comportamento invariato.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 2.1 | Archiviazione (minima) | Interfaccia `IStorage` (Read/Write/Exists/Delete, chiave-valore). Implementazione `FileSystemStorage`. Classe di istanza (non statica). Accesso diretto al file system — **l'AI non può controllare IStorage** |
| 2.2 | Ciclo principale + oggetti Tick | Ciclo infinito, intervallo di clock preciso (`Stopwatch` + `Thread.Sleep`). Pianificazione con priorità |
| 2.3 | Standardizzazione IAIClient | Interfaccia `IAIClientFactory`. OllamaClient rifattorizzato per implementare l'interfaccia standard |
| 2.4 | Migrazione console | Migrazione del `while(true)` verso oggetti Tick guidati dal ciclo principale. Comportamento identico alla Fase 1 |

**Deliverable**: Ciclo principale che esegue Tick, chat da console ancora funzionante.

**Verifica**: Registrare un oggetto Tick di test che stampa il contatore ogni secondo; chat da console ancora funzionante.

---

## ~~Fase 3: C'è un'anima~~ ✅ Completata

**Obiettivo**: Il primo Essere di Silicio vive nel framework.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Classe base astratta, con Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. `Tick()` e `ExecuteOneRound()` astratti |
| 3.2 | Caricamento del File dell'Anima | `SoulFileManager`: legge `soul.md` dalla directory dati dell'essere |
| 3.3 | ContextManager (minimo) | Collega il File dell'Anima + i messaggi recenti → chiama AI → ottiene risposta. Nessuna chiamata strumento, nessuna persistenza |
| 3.4 | ISiliconBeingFactory | Interfaccia factory per creare istanze di esseri |
| 3.5 | SiliconBeingManager (minimo) | Eredita da oggetto Tick (priorità=0). Itera su tutti gli esseri, chiama il loro Tick sequenzialmente |
| 3.6 | DefaultSiliconBeing | Implementazione comportamentale standard. Controlla messaggi non letti → crea ContextManager → ExecuteOneRound → output |
| 3.7 | Struttura directory dell'essere | `DataDirectory/SiliconManager/{GUID}/`, con `soul.md` e `state.json` |

**Deliverable**: Essere di Silicio guidato dal ciclo principale, riceve input da console, carica il File dell'Anima, chiama l'AI.

**Verifica**: Input da console → Tick del ciclo principale attiva → l'essere elabora (con comportamento guidato dal File dell'Anima) → risposta AI. Lo stile di risposta dovrebbe differire dalla Fase 1.

---

## ~~Fase 4: C'è memoria~~ ✅ Completata

**Obiettivo**: Le conversazioni persistono dopo il riavvio.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 4.1 | ChatSystem | Concetto di canale (due GUID = un canale). Modello di messaggi con persistenza. Nessuna chat di gruppo |
| 4.2 | IIMProvider + IMManager | Interfaccia `IIMProvider`. `ConsoleProvider` come canale di messaggistica istantanea formale. `IMManager` instrada i messaggi |
| 4.3 | Miglioramento ContextManager | Estrae la cronologia dal sistema di chat. Persiste le risposte AI. Supporta la continuazione delle chiamate strumento multi-turno |
| 4.4 | Modello IMessage | Modello di messaggio unificato condiviso dal sistema di chat e dal gestore di messaggistica istantanea |

**Deliverable**: Sistema di chat con memoria persistente.

**Verifica**: Chattare per alcuni turni → uscire → riavviare → chiedere "Di cosa abbiamo parlato?" → l'essere può rispondere.

---

## ~~Fase 5: Può agire (sistema degli strumenti)~~ ✅ Completata

**Obiettivo**: Gli Esseri di Silicio possono eseguire azioni, non solo chattare.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interfaccia `ITool` con Name, Description, Execute. `ToolResult` con Success, Message, Data |
| 5.2 | ToolManager | Istanza per ogni essere. Scoperta degli strumenti basata su reflection. Supporto attributo `[SiliconManagerOnly]` |
| 5.3 | IAIClient: supporto chiamate strumento | Analizza le tool_calls AI. Ciclo: esegui strumento → invia risultato → l'AI continua → fino a testo puro |
| 5.4 | Classe base esecutore | Classe base astratta con thread di pianificazione indipendente, coda di richieste, controllo timeout |
| 5.5 | NetworkExecutor | Richieste HTTP tramite esecutore. Timeout, accodamento |
| 5.6 | CommandLineExecutor | Esecuzione shell tramite esecutore. Rilevamento separatore multipiattaforma |
| 5.7 | DiskExecutor | Operazioni su file tramite esecutore. Nessun controllo permessi (Fase 6) |
| 5.8–5.12 | Strumenti integrati | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Deliverable**: Gli Esseri di Silicio possono chiamare strumenti per eseguire azioni.

**Verifica**: Chiedere "Che giorno è oggi" → CalendarTool risponde; chiedere "Controlla i processi" → SystemTool esegue; dire all'essere di inviare un messaggio a un altro essere → ChatTool funziona.

---

## ~~Fase 6: Segue le regole (sistema dei permessi)~~ ✅ Completata

**Obiettivo**: Gli Esseri di Silicio non possono accedere a risorse sensibili senza autorizzazione.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 6.1 | PermissionManager | Istanza privata per ogni essere. Basato su callback, risultato ternario (Allowed/Denied/AskUser). Priorità di interrogazione: HighDeny → HighAllow → Callback. Flag IsCurator |
| 6.2 | Enum PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Whitelist/blacklist di rete, classificazione CLI, regole di sicurezza dei percorsi file |
| 6.4 | GlobalACL | Tabella di regole con corrispondenza per prefisso, persistita nell'archivio |
| 6.5 | UserFrequencyCache | Liste HighAllow/HighDeny. Scelta dell'utente (non rilevamento automatico). Corrispondenza per prefisso, solo in memoria, scadenza configurabile |
| 6.6 | Meccanismo UserAsk (console) | Prompt y/n nella console quando viene restituito AskUser |
| 6.7 | Integrazione permessi esecutore | Tutti gli esecutori controllano i permessi prima dell'esecuzione |
| 6.8 | Nota isolamento IStorage | IStorage è la persistenza interna del sistema — accesso diretto ai file, **non** instradato tramite l'esecutore, **non** controllabile dall'AI. L'esecutore gestisce solo l'I/O avviato dagli strumenti AI |
| 6.9 | Log di audit | Registra tutte le decisioni sui permessi, con timestamp, richiedente, risorsa, risultato |

**Deliverable**: Prompt di permesso quando un essere tenta operazioni sensibili.

**Verifica**: Dire all'essere di eliminare un file → la console mostra il prompt di permesso → inserire `n` → operazione rifiutata. Dire all'essere di accedere a un sito in whitelist → consentito immediatamente.

---

## ~~Fase 7: Può evolversi (compilazione dinamica)~~ ✅ Completata

**Obiettivo**: Gli Esseri di Silicio possono riscrivere il proprio codice.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 7.1 | CodeEncryption | Crittografia/decrittografia AES-256. Derivazione chiave PBKDF2 dal GUID |
| 7.2 | DynamicCompilationExecutor | Sandbox di compilazione in memoria basata su Roslyn. Controllo dei riferimenti assembly in fase di compilazione (difesa primaria: esclusione di System.IO, Reflection, ecc.) |
| 7.3 | Scansione di sicurezza | Analisi statica a runtime di pattern di codice pericolosi (difesa secondaria). Blocca il caricamento se la scansione fallisce |
| 7.4 | Miglioramento ciclo di vita dell'essere | Caricamento: decrittografa → scansiona → compila → istanzia. Runtime: compila in memoria → sostituzione atomica → persisti crittografato |
| 7.5 | SiliconCurator | Classe base astratta del Curatore. IsCurator=true. Privilegi massimi |
| 7.6 | DefaultCurator | Implementazione predefinita del Curatore, con File dell'Anima integrato e strumenti di gestione |
| 7.7 | CuratorTool | Strumento `[SiliconManagerOnly]`: list_beings, create_being, get_code, reset |
| 7.8 | Sovrascrittura callback permessi | Gli esseri possono compilare callback di permessi personalizzati |
| 7.9 | Miglioramento SiliconBeingManager | Metodo Replace (scambio di istanze a runtime). MigrateState (trasferimento stato tra vecchia e nuova istanza) |

**Deliverable**: Gli Esseri di Silicio possono generare nuovo codice tramite AI, compilarlo e sostituire se stessi.

**Verifica**: Dire all'essere "aggiungi una nuova funzionalità a te stesso" → osservare la compilazione → riavviare → nuova funzionalità funzionante.

---

## ~~Fase 8: Memoria e pianificazione~~ ✅ Completata

**Obiettivo**: Memoria a lungo termine, gestione delle attività, attivazione temporizzata.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Archiviazione segmentata a breve/lungo termine. Decadimento temporale. Compressione (unione di memorie simili). Ricerca multidimensionale |
| 8.2 | TaskSystem | Attività monouso + dipendenze DAG. Pianificazione con priorità. Tracciamento dello stato |
| 8.3 | TimerSystem | Sveglia monouso + timer periodico. Precisione millisecondo. Persistenza nell'archivio |
| 8.4 | IncompleteDate | Struttura di intervallo di date fuzzy (es. "aprile 2026", "primavera 2026") |
| 8.5–8.7 | Strumenti memoria/attività/timer | Strumenti per gli esseri per interrogare la memoria, gestire le attività, impostare i timer |

**Deliverable**: Gli esseri possono ricordare i punti chiave, creare/tracciare le attività, impostare sveglie.

**Verifica**: Creare un'attività → controllare l'elenco delle attività → impostare una sveglia di 1 minuto → ricevere la notifica allo scadere.

---

## ~~Fase 9: Framework completato~~ ✅ Completata

**Obiettivo**: Punto di ingresso unificato, collaborazione multi-essere.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Host unificato con pattern builder. Chiusura elegante (Ctrl+C / SIGTERM) |
| 9.2 | Rifattorizzazione Program.Main | Migrazione al pattern CoreHostBuilder |
| 9.3 | Miglioramento SiliconBeingManager | Risposta prioritaria del Curatore. Isolamento delle eccezioni. Persistenza periodica |
| 9.4 | Caricamento multi-essere | Caricamento di più esseri dalla directory dati. Comunicazione tra esseri tramite ChatTool |
| 9.5 | Monitoraggio prestazioni | Tracciamento del tempo di esecuzione per ogni oggetto Tick |
| 9.6 | ServiceLocator | Localizzatore di servizi globale con metodi Register/Get |

**Deliverable**: Più esseri in esecuzione simultanea, collaborativi, gestiti da CoreHost.

**Verifica**: Creare due esseri → A invia un messaggio a B → B riceve e risponde → la pianificazione del framework è priva di errori. Il Curatore risponde con priorità quando arriva un messaggio utente.

---

## ~~Fase 10: Verso il Web~~ ✅ Completata

**Obiettivo**: Migrazione da console a interfaccia browser.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 10.1 | Router | Router di richieste HTTP. Instradamento parametrico seriale e servizio file statici |
| 10.2 | Classe base Controller | Contesto richiesta/risposta. Supporto risposte HTML e JSON |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | Builder lato server C#. Zero dipendenze da framework frontend |
| 10.6 | SSE (Server-Sent Events) | Aggiornamenti push in tempo reale per chat, stato degli esseri ed eventi di sistema. Più semplice di WebSocket, con riconnessione automatica client |
| 10.7 | WebUIProvider | Canale di messaggistica istantanea basato su SSE. Sostituisce la console come interfaccia principale |
| 10.8 | Sicurezza Web | Blacklist/whitelist IP. Attributo `[WebCode]`. Aggiornamento dinamico |
| 10.9–10.17 | Controller Web | Chat, dashboard, esseri, attività, permessi, richieste di permesso, esecutori, log, configurazione, memoria, timer, inizializzazione, informazioni, browser codice, conoscenza, progetto, audit |

**Deliverable**: Web UI completa accessibile dal browser.

**Verifica**: Aprire il browser → chattare con gli esseri → visualizzare la dashboard → gestire i permessi → tutto funziona.

---

## ~~Fase 10.5: Miglioramento incrementale~~ ✅ Completata

**Obiettivo**: Migliorare il sistema esistente con nuove funzionalità scoperte durante lo sviluppo.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Nuovo tipo di sessione per annunci a livello di sistema. ID canale fisso, iscrizione dinamica, filtraggio messaggi in sospeso |
| 10.5.2 | Miglioramento ChatMessage | Campi ToolCallId, ToolCallsJson, Thinking per il contesto AI; PromptTokens, CompletionTokens, TotalTokens per il tracciamento dei token; tipo di messaggio SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Tracciamento del consumo di token per richiesta su tutti gli esseri. Statistiche aggregate, query time-series, archiviazione persistita |
| 10.5.4 | TokenAuditTool | Strumento `[SiliconManagerOnly]` per il Curatore per interrogare e riepilogare l'utilizzo dei token |
| 10.5.5 | ConfigTool | Strumento `[SiliconManagerOnly]` per il Curatore per leggere e modificare la configurazione di sistema |
| 10.5.6 | AuditController | Dashboard web per l'audit dell'utilizzo dei token, con grafici di tendenza ed esportazione dati |
| 10.5.7 | Estensione sistema calendario | 32 implementazioni calendariali, che coprono i sistemi calendariali del mondo (buddhista, lunare, islamico, ebraico, giapponese, persiano, maya, ecc.) |
| 10.5.8 | Miglioramento DiskTool | Nuove operazioni: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Miglioramento SystemTool | Nuove operazioni: find_process (supporto wildcard), resource_usage |
| 10.5.10 | Miglioramento CalendarTool | Nuove operazioni: diff, list_calendars, get_components, get_now_components, convert (conversione tra calendari) |
| 10.5.11 | DashScopeClient | Client AI Alibaba Cloud Bailian, compatibile con API OpenAI. Supporto streaming, chiamate strumento, contenuto di ragionamento |
| 10.5.12 | DashScopeClientFactory | Factory per creare client Bailian. Scoperta dinamica dei modelli tramite API. Supporto multi-regione (Pechino, Virginia, Singapore, Hong Kong, Francoforte) |
| 10.5.13 | Sistema di configurazione client AI | Configurazione del client AI per ogni essere. Opzioni dinamiche delle chiavi di configurazione (modelli, regioni). Nomi visualizzati localizzati |
| 10.5.14 | Estensione localizzazione | Localizzazione in cinese semplificato, cinese tradizionale, inglese e giapponese per le opzioni di configurazione Bailian, nomi dei modelli e nomi delle regioni |

**Deliverable**: Strumenti migliorati, osservabilità, copertura calendariale e supporto multi-backend AI.

**Verifica**: Il Curatore interroga l'utilizzo dei token tramite TokenAuditTool → la dashboard di audit mostra le tendenze → CalendarTool converte le date tra 32 sistemi calendariali → si passa al backend Bailian → si chatta con il modello Qwen tramite API cloud.

---

## ~~Fase 10.6: Affinamento e ottimizzazione~~ ✅ Completata

**Obiettivo**: Affinare le funzionalità di sistema, aggiungere nuove caratteristiche, ottimizzare l'esperienza utente.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Strumento di automazione browser multipiattaforma basato su Playwright, supporto modalità headless, isolamento individuale, supporto completo JS/CSS |
| 10.6.2 | HelpTool | Strumento del sistema di documentazione di aiuto, supporto query e visualizzazione di documentazione multilingua |
| 10.6.3 | ProjectWorkNoteTool | Strumento delle note di lavoro di progetto, supporto registrazione e gestione del lavoro a livello di progetto |
| 10.6.4 | ProjectTaskTool | Strumento di gestione attività di progetto, supporto assegnazione attività, tracciamento avanzamento |
| 10.6.5 | KnowledgeTool | Strumento della rete di conoscenza, supporto CRUD delle triple di conoscenza e scoperta dei percorsi |
| 10.6.6 | ChatHistoryController | Controller di visualizzazione cronologia chat, supporto elenco sessioni e dettagli messaggi |
| 10.6.7 | CodeHoverController | Controller tooltip codice sospeso, supporto evidenziazione sintassi e suggerimenti codice |
| 10.6.8 | WorkNoteController | Controller gestione note di lavoro, supporto ricerca e generazione indice |
| 10.6.9 | TimerExecutionHistory | Funzionalità cronologia esecuzione timer, registrazione e visualizzazione storico attivazioni |
| 10.6.10 | Estensione localizzazione | Aggiunto supporto localizzazione ceco (cs-CZ), totale 21 varianti linguistiche |
| 10.6.11 | Ottimizzazione Web UI | Supporto caricamento file, indicatore di caricamento, ottimizzazione rendering chiamate strumento, correzione modale note di lavoro |
| 10.6.12 | Miglioramento gestione memoria | Filtraggio avanzato, statistiche, vista dettagli, ottimizzazione algoritmo compressione |
| 10.6.13 | Rifattorizzazione sistema log | Separazione log sistema/Esseri di Silicio, API lettura log, filtro per Essere di Silicio |
| 10.6.14 | Miglioramento sistema permessi | Pre-verifica compilazione callback permessi, verifica riferimenti assembly, whitelist servizio meteo wttr.in |

**Deliverable**: Automazione browser WebView completa, sistema documentazione aiuto, area di lavoro progetto, rete di conoscenza, visualizzazione cronologia chat e altri miglioramenti.

**Verifica**: Gli Esseri di Silicio possono operare il browser tramite WebViewBrowserTool → ottenere documentazione di aiuto tramite HelpTool → gestire note di lavoro e attività di progetto → interrogare la rete di conoscenza → visualizzare la cronologia chat.

---

## ~~Fase 10.7: Collaborazione di progetto e workflow~~ ✅ Completata

**Obiettivo**: Aggiungere area di lavoro di progetto, motore di workflow, meccanismo di sfumatura della memoria e sistema di permessi degli strumenti.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 10.7.1 | Gestione ruoli di progetto | ProjectTool aggiunge operazioni assign_role, remove_role, list_roles |
| 10.7.2 | Motore di workflow | Motore core WorkflowEngine, supporto definizione template, transizioni di stato, esecuzione guidata da Tick |
| 10.7.3 | Template di workflow | Classe base WorkflowTemplate, definisce insiemi di stati e regole di transizione |
| 10.7.4 | Istanza di workflow | Gestione istanze WorkflowInstance, collegata a un progetto specifico, tracciamento dello stato corrente |
| 10.7.5 | Log del workflow | WorkflowLog registra la cronologia delle transizioni di stato |
| 10.7.6 | Meccanismo di sfumatura della memoria | MemoryFadeService servizio di decadimento temporizzato, decadimento automatico dell'importanza e archiviazione della memoria ogni ora |
| 10.7.7 | Sistema permessi strumenti | Permessi strumenti a due livelli (livello Essere di Silicio + livello progetto), template permessi, controllo granularità operazioni |
| 10.7.8 | ToolPermissionController | Controller Web gestione permessi strumenti |
| 10.7.9 | ProjectWorkTool | Strumento operazioni di lavoro di progetto ([SiliconManagerOnly], [ToolScenario(Project)]) |
| 10.7.10 | Sistema scenari strumenti | ToolScenarioAttribute e ChatOnlyAttribute, supporto filtraggio scenari Chat/Task/Timer/MemoryCompression/Project |
| 10.7.11 | Estensione localizzazione | Aggiunta localizzazione russo, portoghese, italiano, olandese, polacco, svedese, totale 34 varianti linguistiche |

**Deliverable**: Sistema completo di collaborazione di progetto, motore di workflow, meccanismo di sfumatura della memoria e gestione permessi strumenti.

**Verifica**: Creare progetto → assegnare ruoli → collegare template workflow → gli esseri collaborano nello spazio di progetto → la memoria decade e si archivia automaticamente → l'isolamento dei permessi strumenti funziona.

---

## Fase 11: Integrazione messaggistica istantanea esterna

**Obiettivo**: Connettersi a piattaforme di messaggistica esterne per un'accessibilità più ampia agli utenti.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 11.1 | FeishuProvider | Integrazione bot Feishu (Lark), supporto schede |
| 11.2 | WhatsAppProvider | Integrazione WhatsApp Business API |
| 11.3 | TelegramProvider | Integrazione Telegram Bot API, supporto tastiera inline |
| 11.4 | Miglioramento IMManager | Routing multi-provider, formato messaggi unificato, gestione richieste permessi multipiattaforma |

**Deliverable**: Gli utenti possono interagire con gli Esseri di Silicio tramite piattaforme di messaggistica istantanea esterne.

---

## Fase 12: Funzionalità avanzate

**Obiettivo**: Funzionalità avanzate opzionali per migliorare le capacità.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 12.1 | ~~Rete di conoscenza~~ ✅ Completata | Grafo di conoscenza con struttura a tripla (soggetto-predicato-oggetto), supporto CRUD, scoperta percorsi, query avanzate e attraversamento del grafo |
| 12.2 | ~~Sistema di plugin~~ ✅ Completata | Caricamento di plugin esterni con controlli di sicurezza e sandbox (interfaccia IPlugin, PluginLoader, isolamento AssemblyLoadContext) |
| 12.3 | Ecosistema di competenze | Marketplace di competenze riutilizzabili per le capacità degli esseri |
