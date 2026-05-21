# Tabella di marcia

> **Versione : v0.2.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [Français](../fr-FR/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md) | **Italiano**

## Tabella di marcia a doppia versione

### SiliconLife.Default (Versione standard)
- **Posizionamento** : Implementazione standard, principalmente per la verifica di fattibilità architettonica
- **Stato attuale** : Fasi 1-10.6 completate, il sistema funziona stabilmente
- **Descrizione del ruolo** : Implementazione di riferimento per la verifica architettonica, garantisce la correttezza e la fattibilità del progetto architettonico principale

### SiliconLife.Fast (Versione ad alte prestazioni)
- **Posizionamento** : Versione principale di produzione
- **Stato attuale** : Porting di base dell'architettura completato, motore di storage SpeedyPack e sistema di plugin implementati
- **Descrizione del ruolo** : Basato sull'architettura verificata nella versione Default, esegue un'ottimizzazione approfondita delle prestazioni e rafforza le funzionalità di produzione, la scelta migliore per il deployment reale

**Piano di sviluppo della versione Fast** :
- ✅ Fase 1 : Porting della struttura di progetto di base e del sistema di configurazione
- ✅ Fase 2 : Porting dell'interfaccia Web e dei controller
- ✅ Fase 3 : Ottimizzazione del sistema di storage (storage in memoria SpeedyPack + persistenza asincrona)
- ✅ Fase 3.5 : Strumento di gestione SpeedyPack (applicazione Avalonia UI SiliconLife.Speedy.Manager)
- 📋 Fase 5 : Ottimizzazione delle prestazioni (connection pool, object pool, concorrenza senza lock)
- 📋 Fase 6 : Sostituzione del server Web Kestrel
- 📋 Fase 7 : Serializzazione binaria MessagePack

---

## Principi guida

Ogni fase si conclude con un sistema **funzionante e osservabile**. Nessuna fase produce "molta infrastruttura senza nulla di visibile".

---

## ~~Fase 1 : Sa chattare~~ ✅ Completato

**Obiettivo** : Input console → Chiamata IA → Output console. Unità minima verificabile.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 1.1 | Struttura di soluzione e progetto | Creare `SiliconLifeCollective.sln`, con `src/SiliconLife.Core/` (libreria principale) e `src/SiliconLife.Default/` (implementazione standard + punto di ingresso) |
| 1.2 | Configurazione (minima) | Singleton + deserializzazione JSON. Legge `config.json`. Genera automaticamente i valori predefiniti se assente |
| 1.3 | Localizzazione (minima) | Classe astratta `LocalizationBase`, implementazione `ZhCN`. Aggiungere `Language` alla configurazione |
| 1.4 | OllamaClient (minimo) | Interfaccia `IAIClient`, chiamata HTTP a Ollama locale `/api/chat`. Ancora nessuno streaming, nessuna chiamata di strumenti |
| 1.5 | I/O console | `while(true) + Console.ReadLine()`, leggere input → chiamare IA → stampare risposta |
| 1.6 | Intestazione copyright | Aggiungere intestazione Apache 2.0 a tutti i file sorgente C# |

**Consegnabile** : Programma di chat console per conversare con il modello Ollama locale.

**Verifica** : Eseguire il programma, digitare "ciao", vedere la risposta dell'IA.

---

## ~~Fase 2 : Ha uno scheletro~~ ✅ Completato

**Obiettivo** : Sostituire il "ciclo nudo" con una struttura di framework. Comportamento invariato.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 2.1 | Storage (minimo) | Interfaccia `IStorage` (Read/Write/Exists/Delete, coppie chiave-valore). Implementazione `FileSystemStorage`. Classe di istanza (non statica). Accesso diretto al filesystem —— **L'IA non può controllare IStorage** |
| 2.2 | Ciclo principale + Oggetto timer | Ciclo infinito, intervallo di clock preciso (`Stopwatch` + `Thread.Sleep`). Scheduling per priorità |
| 2.3 | Standardizzazione IAIClient | Interfaccia `IAIClientFactory`. OllamaClient rifattorizzato per l'interfaccia standard |
| 2.4 | Migrazione console | Migrare `while(true)` verso un oggetto timer pilotato dal ciclo principale. Comportamento identico alla Fase 1 |

**Consegnabile** : Il ciclo principale esegue il timer, la chat console funziona ancora.

**Verifica** : Registrare un oggetto timer di test, conta i tick ogni secondo; la chat console funziona ancora.

---

## ~~Fase 3 : Ha un'anima~~ ✅ Completato

**Obiettivo** : Il primo Silicon Being vive nel framework.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Classe base astratta con Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. `Tick()` e `ExecuteOneRound()` astratti |
| 3.2 | Caricamento del file anima | `SoulFileManager` : Legge `soul.md` dalla directory dei dati del Being |
| 3.3 | ContextManager (minimo) | Concatena file anima + messaggi recenti → chiama IA → ottiene risposta. Ancora nessuna chiamata di strumenti, nessuna persistenza |
| 3.4 | ISiliconBeingFactory | Interfaccia factory per creare istanze di Being |
| 3.5 | SiliconBeingManager (minimo) | Eredita dall'oggetto timer (Priorità=0). Itera tutti i Being, chiama il loro Tick sequenzialmente |
| 3.6 | DefaultSiliconBeing | Implementazione del comportamento standard. Verifica messaggi non letti → crea ContextManager → ExecuteOneRound → output |
| 3.7 | Struttura della directory Being | `DataDirectory/SiliconManager/{GUID}/`, contiene `soul.md` e `state.json` |

**Consegnabile** : Silicon Being pilotato dal ciclo principale, riceve input console, carica il file anima, chiama l'IA.

**Verifica** : Input console → Tick del timer del ciclo principale attivato → il Being elabora (con comportamento guidato dal file anima) → risposta dell'IA. Lo stile di risposta dovrebbe differire dalla Fase 1.

---

## ~~Fase 4 : Ha una memoria~~ ✅ Completato

**Obiettivo** : Le conversazioni persistono dopo il riavvio.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 4.1 | ChatSystem | Concetto di canale (due GUID = un canale). Modello di messaggio con persistenza. Ancora nessun chat di gruppo |
| 4.2 | IIMProvider + IMManager | Interfaccia `IIMProvider`. `ConsoleProvider` come canale IM formale. `IMManager` instrada i messaggi |
| 4.3 | Estensione ContextManager | Preleva la cronologia dal sistema di chat. Persiste le risposte dell'IA. Supporta la continuazione di chiamate di strumenti multi-livello |
| 4.4 | Modello IMessage | Modello di messaggio unificato condiviso tra sistema di chat e gestore IM |

**Consegnabile** : Sistema di chat con storage persistente.

**Verifica** : Chattare più turni → Uscire → Riavviare → Chiedere "Di cosa abbiamo parlato ?" → Il Being può rispondere.

---

## ~~Fase 5 : Può agire (Sistema di strumenti)~~ ✅ Completato

**Obiettivo** : I Silicon Beings possono eseguire azioni, non solo chattare.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interfaccia `ITool` con Name, Description, Execute. `ToolResult` con Success, Message, Data |
| 5.2 | ToolManager | Istanza per Being. Scoperta di strumenti basata sulla reflection. Supporto per l'attributo `[SiliconManagerOnly]` |
| 5.3 | IAIClient : Supporto chiamate strumenti | Analizza i tool_calls dell'IA. Ciclo : eseguire strumenti → restituire risultati → IA continua → fino a testo puro |
| 5.4 | Classe base Executor | Classe base astratta con proprio thread dispatcher, coda di richieste, controllo timeout |
| 5.5 | NetworkExecutor | Richieste HTTP tramite executor. Timeout, coda |
| 5.6 | CommandLineExecutor | Esecuzione shell tramite executor. Rilevamento separatori multipiattaforma |
| 5.7 | DiskExecutor | Operazioni sui file tramite executor. Ancora nessun controllo permessi (Fase 6) |
| 5.8–5.12 | Strumenti integrati | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Consegnabile** : I Silicon Beings possono chiamare strumenti per eseguire azioni.

**Verifica** : Chiedere "Che giorno è oggi" → CalendarTool risponde; Chiedere "Controlla i processi" → SystemTool esegue; Chiedere al Being di inviare un messaggio a un altro Being → ChatTool funziona.

---

## ~~Fase 6 : Segue le regole (Sistema di permessi)~~ ✅ Completato

**Obiettivo** : I Silicon Beings non possono accedere a risorse sensibili senza autorizzazione.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 6.1 | PermissionManager | Istanza privata per Being. Catena di permessi a 3 livelli: UserFrequencyCache → IPermissionCallback → (Curatore→IPermissionAskHandler / Non-curatore→GlobalACL→Negazione predefinita). Risultato ternario (Allowed/Deny/AskUser) |
| 6.2 | Enum PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Whitelist/blacklist rete, classificazione CLI, regole di sicurezza percorso file |
| 6.4 | GlobalACL | Tabella di regole per corrispondenza prefisso, persistita nello storage |
| 6.5 | UserFrequencyCache | Liste HighAllow/HighDeny. Scelta utente (nessun rilevamento automatico). Corrispondenza prefisso, solo memoria, expiration configurabile |
| 6.6 | Meccanismo UserAsk (Console) | In caso di ritorno AskUser, prompt console s/n |
| 6.7 | Integrazione permessi executor | Tutti gli executor verificano il permesso prima dell'esecuzione |
| 6.8 | Nota isolamento IStorage | IStorage è la persistenza sistema interna —— accesso file diretto, **non** instradato tramite executor, **non** controllabile dall'IA. Gli executor gestiscono solo I/O avviati dagli strumenti IA |
| 6.9 | Log di audit | Registra tutte le decisioni di permessi con timestamp, richiedente, risorsa, risultato |

**Consegnabile** : Prompt di permesso quando il Being tenta un'operazione sensibile.

**Verifica** : Chiedere al Being di eliminare un file → La console mostra un prompt di permesso → Digitare `n` → Operazione rifiutata. Chiedere al Being di visitare un sito in whitelist → Immediatamente autorizzato.

---

## ~~Fase 7 : Può evolversi (Compilazione dinamica)~~ ✅ Completato

**Obiettivo** : I Silicon Beings possono riscrivere il proprio codice.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 7.1 | CodeEncryption | Crittografia/decifratura AES-256. Chiave PBKDF2 derivata dal GUID |
| 7.2 | DynamicCompilationExecutor | Sandbox di compilazione in memoria basato su Roslyn. Controllo dei riferimenti assembly in compilazione (difesa principale : escludere System.IO, Reflection, ecc.) |
| 7.3 | Analisi di sicurezza | Analisi statica a runtime di pattern di codice pericolosi (difesa secondaria). Blocca il caricamento se l'analisi fallisce |
| 7.4 | Estensione ciclo di vita Being | Caricamento : Decifrare → Analizzare → Compilare → Istanzia. Esecuzione : Compilare in memoria → Sostituzione atomica → Persistere crittografato |
| 7.5 | SiliconCurator | Classe base astratta del Curator. IsCurator=true. Permesso più alto |
| 7.6 | DefaultCurator | Implementazione standard del Curator con file anima integrato e strumenti admin |
| 7.7 | CuratorTool | Strumenti `[SiliconManagerOnly]` : list_beings, create_being, get_code, reset |
| 7.8 | Override callback permessi | I Being possono compilare callback di permessi personalizzati |
| 7.9 | Estensione SiliconBeingManager | Metodo Replace (scambio istanza a runtime). MigrateState (trasferimento stato tra vecchia e nuova istanza) |

**Consegnabile** : I Silicon Beings possono compilare e sostituirsi con nuovo codice generato dall'IA.

**Verifica** : Chiedere al Being "Aggiungiti una nuova funzionalità" → Osservare la compilazione → Riavvio → La nuova funzionalità funziona.

---

## ~~Fase 8 : Memoria e pianificazione~~ ✅ Completato

**Obiettivo** : Storage a lungo termine, gestione compiti, trigger timer.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Storage segmentato breve termine/lungo termine. Scadenza temporale. Compressione (fusione di ricordi simili). Ricerca multidimensionale |
| 8.2 | TaskSystem | Compiti singoli + dipendenze DAG. Scheduling per priorità. Monitoraggio stato |
| 8.3 | TimerSystem | Sveglia singola + timer periodici. Precisione al millisecondo. Persistito nello storage |
| 8.4 | IncompleteDate | Struttura di intervallo di date approssimative (es. "aprile 2026", "primavera 2026") |
| 8.5–8.7 | Strumenti memoria/compiti/timer | Strumenti per i Being per consultare ricordi, gestire compiti, impostare timer |

**Consegnabile** : I Being possono ricordare punti chiave, creare/monitorare compiti, impostare sveglie.

**Verifica** : Creare un compito → Verificare la lista dei compiti → Impostare una sveglia di un minuto → Ricevere notifica allo scatto.

---

## ~~Fase 9 : Framework completato~~ ✅ Completato

**Obiettivo** : Punto di ingresso unificato, collaborazione multi-Being.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Host unificato con pattern Builder. Arresto graceful (Ctrl+C / SIGTERM) |
| 9.2 | Refactoring di Program.Main | Migrazione al pattern CoreHostBuilder |
| 9.3 | Estensione SiliconBeingManager | Risposta Curator-first. Isolamento eccezioni. Persistenza regolare |
| 9.4 | Caricamento multi-Being | Carica più Being dalla directory dati. Comunicazione Being-a-Being tramite ChatTool |
| 9.5 | Monitoraggio prestazioni | Tracciamento del tempo di esecuzione per oggetto timer |
| 9.6 | ServiceLocator | Localizzatore di servizi globale con metodi Register/Get |

**Consegnabile** : Più Being funzionano simultaneamente, collaborano, gestiti da CoreHost.

**Verifica** : Creare due Being → A invia un messaggio a B → B riceve e risponde → Scheduling del framework senza errori. Il Curator risponde per primo ai messaggi utente.

---

## ~~Fase 10 : Verso il Web~~ ✅ Completato

**Obiettivo** : Migrare dalla console all'interfaccia browser.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 10.1 | Router | Router di richieste HTTP. Routing per parametri sequenziali e servizio file statici |
| 10.2 | Classe base Controller | Contesto richiesta/risposta. Supporto per risposte HTML e JSON |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | Builder lato server C#. Zero dipendenza framework frontend |
| 10.6 | SSE (Server-Sent Events) | Aggiornamenti in tempo reale push per chat, stato dei Being ed eventi di sistema. Più semplice di WebSocket, con riconnessione automatica del client |
| 10.7 | WebUIProvider | Canale IM in tempo reale basato su SSE. Sostituisce la console come interfaccia principale |
| 10.8 | Sicurezza Web | Blacklist/whitelist IP. Attributo `[WebCode]`. Aggiornamenti dinamici |
| 10.9–10.17 | Controller Web | Chat, Dashboard, Beings, Compiti, Permessi, Richieste di permessi, Executor, Log, Configurazione, Memoria, Timer, Inizializzazione, Informazioni, Browser di codice, Conoscenze, Progetti, Audit |

**Consegnabile** : Interfaccia Web completa, accessibile dal browser.

**Verifica** : Aprire il browser → Chattare con un Being → Vedere la dashboard → Gestire i permessi → Tutto funziona.

---

## ~~Fase 10.5 : Estensioni incrementali~~ ✅ Completato

**Obiettivo** : Estendere il sistema esistente con nuove funzionalità scoperte durante lo sviluppo.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Nuovo tipo di sessione per annunci di sistema. ID canale fisso, abbonamento dinamico, filtraggio messaggi in attesa |
| 10.5.2 | Estensione ChatMessage | Campi ToolCallId, ToolCallsJson, Thinking per contesto IA ; PromptTokens, CompletionTokens, TotalTokens per monitoraggio token ; tipo di messaggio SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Monitoraggio consumo token per tutti i Being per richiesta. Statistiche aggregate, query serie temporali, storage persistente |
| 10.5.4 | TokenAuditTool | Strumento `[SiliconManagerOnly]` per il Curator per consultare e riassumere l'utilizzo dei token |
| 10.5.5 | ConfigTool | Strumento `[SiliconManagerOnly]` per il Curator per leggere e modificare la configurazione di sistema |
| 10.5.6 | AuditController | Dashboard Web per audit utilizzo token con grafici di tendenza ed esportazione dati |
| 10.5.7 | Estensione sistema calendario | 32 implementazioni di calendari, che coprono i sistemi calendariali mondiali (Buddista, Lunare cinese, Islamico, Ebraico, Giapponese, Persiano, Maya, ecc.) |
| 10.5.8 | Estensione DiskTool | Nuove operazioni : count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Estensione SystemTool | Nuove operazioni : find_process (con supporto caratteri jolly), resource_usage |
| 10.5.10 | Estensione CalendarTool | Nuove operazioni : diff, list_calendars, get_components, get_now_components, convert (conversione inter-calendari) |
| 10.5.11 | DashScopeClient | Client IA Alibaba Cloud DashScope, compatibile API OpenAI. Supporta streaming, chiamate strumenti, contenuto di ragionamento |
| 10.5.12 | DashScopeClientFactory | Factory per creare client DashScope. Scoperta dinamica modelli tramite API. Supporto multi-regione (Pechino, Virginia, Singapore, Hong Kong, Francoforte) |
| 10.5.13 | Sistema configurazione client IA | Configurazione client IA per Being. Opzioni di chiavi di configurazione dinamiche (modello, regione). Nomi visualizzati localizzati |
| 10.5.14 | Estensione localizzazione | Localizzazione in cinese semplificato, cinese tradizionale, inglese e giapponese per le opzioni di configurazione DashScope, nomi di modelli e nomi di regioni |

**Consegnabile** : Strumenti estesi, osservabilità, copertura calendari e supporto multi-backend IA.

**Verifica** : Il Curator interroga l'utilizzo dei token tramite TokenAuditTool → La dashboard di audit mostra le tendenze → CalendarTool converte la data tra 32 sistemi di calendario → Cambiare backend IA su DashScope → Chattare con il modello Qwen tramite API cloud.

---

## ~~Fase 10.6 : Raffinamento e ottimizzazione~~ ✅ Completato

**Obiettivo** : Raffinare le funzionalità di sistema, aggiungere nuove funzionalità, ottimizzare l'esperienza utente.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Strumento di automazione browser multipiattaforma basato su Playwright, con modalità headless, isolamento individuale, supporto JS/CSS completo |
| 10.6.2 | HelpTool | Strumento del sistema di documentazione di aiuto, supporta la consultazione e la visualizzazione di documentazione multilingua |
| 10.6.3 | ProjectWorkNoteTool | Strumento di note di lavoro di progetto, supporta la registrazione di lavoro collegato al progetto e la gestione |
| 10.6.4 | ProjectTaskTool | Strumento di gestione compiti di progetto, supporta l'assegnazione di compiti, il monitoraggio progressi |
| 10.6.5 | KnowledgeTool | Strumento di rete di conoscenze, supporta il CRUD di conoscenze in triplette e la ricerca di percorsi |
| 10.6.6 | ChatHistoryController | Controller di visualizzazione cronologia chat, supporta l'elenco delle sessioni e i dettagli dei messaggi |
| 10.6.7 | CodeHoverController | Controller di suggerimenti al passaggio del mouse sul codice, supporta la colorazione sintattica e il completamento del codice |
| 10.6.8 | WorkNoteController | Controller di gestione delle note di lavoro, supporta la ricerca e la generazione di directory |
| 10.6.9 | TimerExecutionHistory | Funzionalità di cronologia esecuzione timer, registra la cronologia degli scatti e permette la consultazione |
| 10.6.10 | Estensione localizzazione | Aggiunta del supporto di localizzazione ceca (cs-CZ), totale di 24 varianti linguistiche |
| 10.6.11 | Ottimizzazione interfaccia Web | Supporto upload file, indicatore di caricamento, ottimizzazione rendering chiamate strumenti, correzione modale note di lavoro |
| 10.6.12 | Estensione gestione memoria | Filtraggio avanzato, statistiche, vista dettagliata, ottimizzazione algoritmo compressione |
| 10.6.13 | Refactoring sistema log | Separazione log sistema/Silicon Being, API lettura log, filtro per Being |
| 10.6.14 | Estensione sistema permessi | Pre-convalida callback permessi, convalida riferimenti assembly, whitelist servizio meteo wttr.in |

**Consegnabile** : Automazione completa browser WebView, sistema di documentazione di aiuto, spazio progetto, rete di conoscenze, visualizzazione cronologia chat e altre funzionalità avanzate.

**Verifica** : Il Silicon Being può usare il browser tramite WebViewBrowserTool → Ottenere la documentazione di aiuto tramite HelpTool → Gestire le note di lavoro e i compiti di progetto → Consultare la rete di conoscenze → Vedere la cronologia chat.

---

## Fase 11 : Integrazione IM esterna

**Obiettivo** : Connessione a piattaforme di messaggistica esterne per una maggiore accessibilità utente.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 11.1 | FeishuProvider | Integrazione bot Feishu (Lark) con supporto schede |
| 11.2 | WhatsAppProvider | Integrazione API WhatsApp Business |
| 11.3 | TelegramProvider | Integrazione API Telegram Bot con supporto tastiera inline |
| 11.4 | Estensione IMManager | Routing multi-provider, formato messaggio unificato, gestione richieste permessi multipiattaforma |

---

## Fase 12 : Ecosistema di competenze

**Obiettivo** : Creare un mercato di plugin e un sistema di distribuzione di competenze.

| # | Modulo | Descrizione |
|---|--------|-------------|
| 12.1 | SkillPackage | Formato pacchetto competenze, metadati, verifica firma |
| 12.2 | SkillMarketplace | Interfaccia Web di navigazione e installazione competenze |
| 12.3 | SkillRuntime | Ambiente di esecuzione competenze isolato, gestione ciclo di vita |
| 12.4 | CommunityContrib | Sistema di invio e revisione della comunità |
