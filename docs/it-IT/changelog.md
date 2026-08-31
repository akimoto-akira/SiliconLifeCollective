# Registro delle Modifiche

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md) | **Italiano**

Tutte le modifiche importanti di questo progetto saranno documentate in questo file.

Il formato è basato su [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
e questo progetto adotta il [Versionamento Semantico](https://semver.org/spec/v2.0.0.html).

---

## Informazioni su questo Registro delle Modifiche

### Doppia Versione del Progetto

Questo progetto fornisce due versioni di implementazione:

- **SiliconLife.Default**: Implementazione predefinita, utilizzata principalmente per verificare la fattibilità dell'architettura. Applicazione console, archiviazione JSON su file system.
- **SiliconLife.Fast**: Versione di produzione raccomandata. Applicazione desktop multipiattaforma (Windows / macOS / Linux), archiviazione in memoria SpeedyPack + persistenza asincrona, ottimizzazione delle prestazioni approfondita.

Le due versioni condividono le stesse interfacce e funzionalità, differendo solo nell'implementazione dell'archiviazione e nella modalità di esecuzione. SiliconLife.Default funge da riferimento per la validazione dell'architettura, mentre SiliconLife.Fast è la versione di produzione raccomandata.

### Origine del Progetto

- Questo progetto ha avuto origine il 20 marzo 2026.
- Prima di questo progetto, c'era una demo di validazione che fallì a causa di un design architetturale inadeguato, rendendo impossibile l'integrazione con multiple piattaforme AI.

### Strumenti AI IDE Utilizzati

#### Kiro (Amazon AWS)
- Il progetto è stato inizialmente mantenuto da Kiro, avviato utilizzando la modalità Spec.
- Kiro è un ambiente di sviluppo AI agente costruito da Amazon AWS.
- Basato su Code OSS (VS Code), supporta le impostazioni di VS Code e i plugin compatibili Open VSX.
- Flusso di lavoro di sviluppo guidato dalle specifiche, per la codifica AI strutturata.

#### Comate AI IDE / 文心快码 (Baidu)
- Utilizzato occasionalmente per la scrittura e il lavoro di documentazione.
- Comate AI IDE è uno strumento di ambiente di sviluppo nativo AI rilasciato da Baidu Wenxin il 23 giugno 2025.
- Primo IDE AI multimodale e multi-agente del settore.
- Funzionalità includono la conversione da design a codice e la codifica assistita AI a flusso completo.
- Alimentato dal modello Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Utilizzato da ottobre 2025 ad aprile 2026.
- IDE AI, supporta la generazione intelligente di codice e la gestione dei progetti.

#### Qoder (Alibaba)
- Utilizzato per la manutenzione del progetto dal 18 aprile 2026.
- Piattaforma di codifica AI, supporta l'analisi del codice, la generazione di documentazione e la collaborazione multi-agente.

#### CatPaw (Meituan)
- Utilizzato in combinazione con Qoder dal 6 maggio 2026.
- Basato sulla serie di modelli LongCat sviluppati autonomamente da Meituan, con forte capacità di rifattorizzazione dell'architettura del codice completo.

#### DuMate (Baidu Qianfan)
- Utilizzato da luglio 2026 per lo sviluppo del codice, la localizzazione e la documentazione.
- Assistente IA generalista sulla piattaforma desktop Qianfan, con capacità di orchestrazione multi-strumento, operazioni sui file, automazione del browser ed esecuzione di attività multi-fase.
- Legge e scrive direttamente i file locali, esegue comandi shell ed effettua ricerche web sul desktop Windows dell'utente.

### Documentazione dei Requisiti

- La documentazione dei requisiti di questo progetto non è pubblica.
- I requisiti sono stati validati iterativamente attraverso più di 12 piattaforme AI internazionali e grandi serie di modelli, producendo oltre 2000 righe di documentazione dei requisiti guidata da user story quasi incomprensibile per gli esseri umani.

---

## [Non Pubblicato]

### 2026-08-27

#### Nuove Funzionalita
- Client LongCat aggiornato al modello LongCat-2.0
  - Finestra di contesto espansa da 128K a 1M (1.048.576 token), output massimo 128K
  - Aggiunto supporto alla modalita thinking, il corpo della richiesta include thinking type enabled
  - Aggiunta lista di modelli predefiniti (LongCat-2.0 / Flash-Chat / Pro-Chat / Max-Chat), fallback in caso di fallimento del recupero dinamico da API
  - Endpoint predefinito aggiornato a https://api.longcat.chat/openai
  - 13 file di lingua aggiornati con i nomi di visualizzazione dei modelli, documentazione di aiuto aggiornata con prezzi e link di registrazione


### 2026-08-17

#### Nuove Funzionalità
- `c7b575b` - Implementazione dell'integrazione MCP——accesso agli strumenti del server esterno, gestione della configurazione e documentazione di aiuto
  - Nuovo core MCP (SiliconLife.Core/Mcp/): gestione del ciclo di vita del server McpManager, doppio trasporto stdio/http, incapsulamento della connessione McpClientConnection, strumenti impacchettati per server e iniettati in tutti gli Esseri di Silicio con denominazione `mcp_{serverId}_{toolName}`
  - Nuova pagina di gestione Web (/mcp) con 7 endpoint API (list-servers/list-tools/add-server/toggle/remove-server/reconnect/test-tool)
  - Nuovo strumento di query McpTool (status/list_servers/list_tools, sola lettura); aggiunta/rimozione dei server limitata all'utente tramite Web UI, l'IA non può modificare la lista dei server
  - La pagina di configurazione supporta l'editor di array dei server MCP (aggiunta/rimozione in linea nella finestra modale)
  - Registrazione dell'argomento di aiuto MCP (🔌), implementazione completa della documentazione di aiuto in 10 lingue
  - Gli strumenti wrapper MCP sono presentati nella matrice dei permessi con azione `execute`, supportano la disabilitazione per Essere/progetto
  - 45 file modificato/i

### 2026-08-16

#### Nuove Funzionalità
- `5d76c5a` - Implementazione del sistema di Competenze——livello di astrazione per il riutilizzo dell'orchestrazione degli strumenti e dei template di prompt
  - Nuovo SkillDefinition (id/descrizione/schema parametri/template prompt di sistema/whitelist strumenti/limiti azioni/numero massimo di round/timeout/azione di completamento/modalità di attivazione)
  - Nuovo SkillManager: centro di registrazione delle Competenze + motore di esecuzione (ciclo AIRequest figlio, protezione ricorsiva, limitazione globale di round e timeout)
  - Doppia modalità di attivazione: Manual (chiamata di funzione IA, Competenza iniettata come ToolDefinition, routing prioritario lato schedulazione) + Auto (pianificazione schedule, supporta `HH:mm` / `N s|m|h|d` / subset cron)
  - Archiviazione prioritaria Markdown (frontmatter YAML + corpo del prompt), Markdown puro con completamento automatico dei metadati da parte dell'IA (i campi utente non vengono sovrascritti)
  - Hot reload (rilevamento impronta 30 secondi), archiviazione versioni (skills/archive/), 3 Competenze integrate (summarize_document/code_review/research_topic)
  - Nuovo strumento skill (create/list/update/update_from_md/delete/export/export_md/import/import_md)
  - Nuova pagina di gestione Competenze (/skill) con 10 endpoint API; quota MaxCustomSkillsPerBeing (predefinito 50)
  - Permessi: permesso di azione `execute` a livello di Competenza, whitelist degli strumenti nella Competenza e permessi dell'Essere con intersezione rigorosa
- `b60fc68` - Aggiornamento della lista modelli Qianfan e mappatura della finestra di contesto - aggiunti modelli glm-5.2/glm-5.1/deepseek-v4-pro/deepseek-v4-flash/kimi-k2.6/ernie-5.1/qianfan-code-latest, mappatura della finestra di contesto gerarchica 1M/128K e capacità visive

### 2026-08-15

#### Nuove Funzionalità
- `eaa8417` - Implementazione della procedura guidata OAuth della piattaforma IM e analisi delle variabili d'ambiente per le chiavi di configurazione
  - Nuovo ImOAuthController/ImOAuthService a supporto del flusso di autorizzazione OAuth Feishu (authorize/callback/status), con state anti-CSRF, timeout di 5 minuti, push dello stato SSE
  - Nuovo IMProviderRegistry per la gestione unificata dei metadati della piattaforma IM (schema campi di configurazione/template endpoint OAuth/factory Provider)
  - Nuovo ConfigSecretResolver per analizzare i segnaposto `${ENV_VAR}` nella configurazione, sostituzione con copia profonda senza riscrivere la configurazione originale
  - La pagina di configurazione integra l'UI della procedura guidata OAuth IM (area di autorizzazione in linea + stato SSE in tempo reale)
  - Completamento delle traduzioni degli stati di autorizzazione IM/testi di aiuto in 13 file di lingua

### 2026-07-26

#### Rifattorizzazione
- `ffc45c2` - Rifattorizzazione della piattaforma IM in architettura di configurazione multi-istanza - IMPlatforms listizzato (avvio/arresto indipendente per piattaforma), AggregateIMProvider aggrega invio/ricezione messaggi multi-piattaforma e gara dei permessi, editor multi-istanza nella pagina di configurazione

### 2026-07-19

#### Nuove Funzionalità
- `9bf2103` - Speedy.Manager integrazione della visualizzazione ad albero con eliminazione ed esportazione multi-selezione

#### Correzioni
- `0df0674` - Correzione del problema di eliminazione multi-selezione di Speedy.Manager che eliminava solo il primo elemento

### 2026-07-16

#### Nuove Funzionalità
- `7431312` - Completare le traduzioni di configurazione dei client IA per 13 file di lingua - CsCZ/PlPL aggiornati da stub a implementazione completa di dizionario, i restanti 10 file aggiungono voci ConfigDisplayNames/ConfigDescriptions/ConfigGroupNames per 7 nuovi client (DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan), aggiornamento sincronizzato dei metadati delle chiavi di configurazione di 6 ClientFactory
  - 20 file modificato/i

#### Documentazione
- `d6608ea` - Aggiungere l'introduzione dello strumento AI IDE DuMate (Baidu Qianfan) a tutte le 13 versioni linguistiche del changelog
  - 13 file modificato/i

#### Framework di Collaborazione
- `c607c97` - Registrare DuMate (Baidu Qianfan) come attore IA residente nel registro .ai-collab
  - 1 file modificato/i


### 2026-07-15

#### Nuove Funzionalità
- `c007263` - Completare la documentazione di aiuto per 10 client IA - HelpTopics registra 10 argomenti, HelpLocalizationBase aggiunge 30 proprietà astratte, 12 file di lingua implementano contenuti di aiuto Markdown completi (introduzione piattaforma/passaggi di registrazione/metodi di configurazione/modelli disponibili/fatturazione/FAQ), coprendo Herdsman/LongCat/QiniuAI/DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan
  - 12 file modificato/i
- `4634e33` - Implementazione di 7 client di piattaforme IA nazionali (DeepSeek/Zhipu GLM/Moonshot Kimi/SiliconFlow/MiniMax/Baidu Ernie/Tencent Hunyuan) - 14 file di classe indipendenti, seguendo lo stile LongCatClient, senza ereditarietà, tutti compatibili OpenAI + Bearer Token, supporto Tool Calling/streaming/modalità thinking, registrati in DefaultSiliconBeing e DefaultSiliconBeingFactory
  - 16 file modificato/i

#### Documentazione
- `108c4ea` - Aggiornare tutta la documentazione in 13 lingue per riflettere 7 nuovi client IA - stato 📋→✅, 01.AI contrassegnato come deprecato
  - 94 file modificato/i


### 2026-07-14

#### Documentazione
- `344b429` - Aggiungere lo stato "Deprecato" allo stato della piattaforma IA in architecture.md per tutte le lingue, contrassegnare 01.AI come deprecato (registrazione nuovi utenti fermata)
  - 13 file modificato/i


### 2026-07-07

#### Pulizia
- `e06e6f2` - Rimuovere la toolchain OsmStore e il plugin TravelCodeWikiWithAI - eliminare tools/OsmStore.* tre progetti, eliminare src/TravelCodeWikiWithAI/ progetto plugin, pulire i riferimenti sln, il progetto torna al percorso di sviluppo TCW autonomo
  - 45 file modificato/i


### 2026-07-06

#### Correzioni
- `1b15886` - Standardizzazione del modello dati OSM e correzione della sicurezza del tipo di elemento
  - 7 file modificato/i


### 2026-07-05

#### Nuove Funzionalità
- `be4320b` - TravelCodeWikiWithAI aggiunge modulo di provider dati CLDR
  - 4 file modificato/i


### 2026-07-04

#### Nuove Funzionalità
- `dbcabf3` - Miglioramento del sistema di permessi dei plugin - refactoring IO rete/file in modalità Executor + esenzione whitelist GeneratedCodeAttribute
  - 34 file modificato/i
- `e84bb63` - Correggere gli errori di compilazione e aggiungere il progetto TravelCodeWikiWithAI
  - 53 file modificato/i

#### Rifattorizzazione
- `9e5a345` - TravelCodeWikiWithAI migra completamente PBF all'API OSM online sincrona
  - 4 file modificato/i


### 2026-05-31

#### Nuove Funzionalità
- `a5f37bd` - Aggiornare il pensiero del progetto, il sistema di conversazione e le funzionalità di archiviazione
  - 13 file modificato/i


### 2026-05-30

#### Nuove Funzionalità
- `c3cf429` - Aggiungere client IA QiniuAIClient (servizio di inferenza di grande modello IA Qiniu Cloud) (ref task-409)
  - 20 file modificato/i
- `d04131f` - Aggiungere client IA LongCatClient (grande modello LongCat Meituan) (ref task-408)
  - 19 file modificato/i

#### Framework di Collaborazione
- `e9564f5` - Aggiornare tutti i file modificati
  - 140 file modificato/i
- `9c8b42f` - Archiviare session e changes del 2026-05-29
  - 20 file modificato/i


### 2026-05-29

#### Nuove Funzionalità
- `d548e48` - Pagina di dettaglio del pensiero del progetto raggruppa i messaggi per Cycle con sezioni collassabili (ref task-407)
  - 23 file modificato/i
- `28d893d` - IAIClient aggiunge interfaccia di dichiarazione di capacità multimodale + ChatMessage aggiunge campi multimodali (ref task-402)
  - 13 file modificato/i
- `ebe6a49` - Pagina di dettaglio del pensiero del progetto aggiunge la visualizzazione dello stato della sessione, ora di creazione e ora di completamento (ref task-406)
  - 22 file modificato/i
- `9a53d55` - IAIClient aggiunge ContextWindowTokens + sistema di budget Token + configurazione di fabbrica (ref task-401, task-403)
  - 26 file modificato/i
- `202b99c` - Aggiungere client IA HerdsmanClient + correggere il non aggiornamento del menu a discesa dell'interfaccia di inizializzazione (ref task-399, task-400)
  - 20 file modificato/i
- `285ab2f` - Visualizzazione frontend del record di elaborazione del progetto (ref task-397)
  - 25 file modificato/i
- `b4b633f` - Meccanismo di dialogo multi-round pseudo-Session di ThinkOnProject (ref task-395)
  - 13 file modificato/i
- `d3e543f` - Il contesto di scenario ThinkOnProject aggiunge le informazioni sui silicon being disponibili (ref task-394)
  - 21 file modificato/i
- `07eb628` - BuildRequest inietta dinamicamente le informazioni di appartenenza al progetto dei silicon being (ref task-396)
  - 21 file modificato/i
- `2089696` - Tool aggiunge supporto scenario Project + refactoring unificato multi-directory PluginLoader
  - 12 file modificato/i

#### Correzioni
- `b80a33b` - Correggere il testo del suggerimento di caricamento della pagina di dettaglio del pensiero del progetto codificato in inglese e privo di localizzazione (ref task-405)
  - 6 file modificato/i
- `90b60c5` - Correggere il nascondimento di Content e Thinking del corpo IA nei round di chiamata degli strumenti (ref task-404)
  - 8 file modificato/i
- `a7d9a97` - Correggere la continuazione del ciclo multi-round di ThinkOnProject e la perdita dei messaggi di promemoria del progetto
  - 6 file modificato/i
- `c0838dd` - Correggere che i messaggi ProjectThinkSession non vengono scritti in Cycle e la cronologia viene eliminata dopo il completamento (ref task-398)
  - 7 file modificato/i
- `f3d1794` - Correggere la mancanza di localizzazione dello stato Project/Broadcast/Stopped dei silicon being e l'anomalia di visualizzazione (ref task-393)
  - 20 file modificato/i
- `3eaa90d` - Rimuovere i riferimenti della soluzione al progetto eliminato TravelCodeWikiWithAI
  - 1 file modificato/i

#### Framework di Collaborazione
- `f3cbed7` - Registrare task-394~396 (miglioramenti ThinkOnProject)
  - 3 file modificato/i
- `e1971f5` - Registrare task-393 (localizzazione BeingActivity e correzione visualizzazione)
  - 1 file modificato/i
- `e710fa4` - Aggiornare changes commitHash e fine sessione state
  - 2 file modificato/i
- `4cacc4a` - Archiviare session e changes del 2026-05-28
  - 4 file modificato/i


### 2026-05-28

#### Nuove Funzionalità
- `ae8b673` - Configurazione della directory dei plugin aggiornata da percorso singolo a lista multi-directory (ref task-391)
  - 29 file modificato/i
- `aac46c1` - PluginLoader aggiunge modalità sorgente CS, compila-carica plugin quando non esiste DLL (ref task-389)
  - 6 file modificato/i

#### Correzioni
- `63047b0` - Registrare tutti i PluginLoader in ServiceLocator, correggere la riflessione incompleta dei plugin multi-directory (ref task-391)
  - 3 file modificato/i
- `fcad655` - Correggere il problema di interazione del pulsante sfoglia directoryList (ref task-392)
  - 9 file modificato/i

#### Documentazione
- `e6d3037` - PluginDemo-22 esempio di modalità compila-carica codice sorgente CS (ref task-390)
  - 21 file modificato/i

#### Framework di Collaborazione
- `09d9e9c` - Archiviare 30 attività completate (task-362~task-391)
  - 2 file modificato/i
- `66204a1` - Archiviare session (8) e changes (8) del 2026-05-28
  - 18 file modificato/i
- `308a8d0` - Aggiornare relatedCommit di task-391
  - 1 file modificato/i
- `6fc4e05` - Registrare task-389 (modalità sorgente CS) e task-390 (PluginDemo-22)
  - 1 file modificato/i


### 2026-05-27

#### Nuove Funzionalità
- `e154a18` - Completare l'esempio di workflow aziendale completo PluginDemo-21 WorkflowTemplate (ref task-388)
  - 19 file modificato/i
- `aa771b3` - Implementare il sistema di permessi dichiarativo PluginCapability (ref task-379)
  - 9 file modificato/i
- `5e5e9d1` - Aggiungere l'esempio di tipo sicuro whitelist 04-SafeSystemIO System.IO (ref task-370)
  - 20 file modificato/i

#### Documentazione
- `48f6702` - Allineare le traduzioni README di 19-TickObject e 20-SpeedyPack in tutte le lingue alla baseline (ref task-386, task-387)
  - 119 file modificato/i
- `5d570e5` - Completare il task-378 contro-esempio di bypass di riflessione stringa proibita (ref task-378)
  - 19 file modificato/i
- `348c410` - PluginDemo-11 contro-esempio di codice P/Invoke e unsafe vietato (ref task-377)
  - 19 file modificato/i
- `fc92a49` - PluginDemo-10 contro-esempio di operazione di riflessione vietata (ref task-376)
  - 19 file modificato/i
- `826ad2a` - Creare il plugin contro-esempio di operazione di processo vietata PluginDemo-09 (ref task-375)
  - 19 file modificato/i
- `7870b05` - Aggiungere il contro-esempio di operazione di rete vietata PluginDemo-08 (ref task-374)
  - 15 file modificato/i
- `8636e31` - PluginDemo-07 contro-esempio di operazione di I/O file vietata (ref task-373)
  - 19 file modificato/i
- `322312e` - Aggiungere l'esempio di dipendenza attendibile PluginDemo-06 TrustedAssemblies (ref task-372)
  - 19 file modificato/i
- `6df98a0` - Aggiungere l'esempio di plugin workflow IWorkflowPlugin (ref task-371)
  - 20 file modificato/i
- `f3787ba` - PluginDemo-03 esempio di registrazione e creazione IObjectFactory (ref task-369)
  - 20 file modificato/i
- `bb4324d` - PluginDemo-02 esempio di registrazione e query ITypeRegistry (ref task-368)
  - 20 file modificato/i
- `bbdfa3c` - PluginDemo-01 esempio di implementazione minimale IPlugin (ref task-367)
  - 19 file modificato/i

#### Framework di Collaborazione
- `de44057` - Archiviare session e changes del 25 e 27 maggio
  - 58 file modificato/i
- `9e4a84c` - Aggiornare lastCommitHash di tasks.json a 48f6702
  - 1 file modificato/i
- `beb58b2` - Completare l'indice taskIndex (8 pending, 19 completed)
  - 1 file modificato/i
- `63f7bfc` - Aggiornare relatedCommit di task-388 (ref task-388)
  - 1 file modificato/i
- `e61be6f` - Aggiornare relatedCommit di task-378 (ref task-378)
  - 1 file modificato/i
- `dde579b` - Pubblicare il task di esempio di utilizzo completo WorkflowTemplate (task-388)
  - 1 file modificato/i
- `2294fa7` - Pubblicare i task di esempio TickObject e SpeedyPack (task-386~387)
  - 1 file modificato/i
- `82b9f63` - Pubblicare 6 task di esempio PluginCapability (task-380~385)
  - 1 file modificato/i
- `588539b` - Pubblicare il task del sistema di permessi dichiarativo PluginCapability (task-379)
  - 1 file modificato/i
- `37f9c23` - Aggiornare i riferimenti di soluzione e file di progetto
  - 8 file modificato/i
- `e1f7892` - Pubblicare 12 task in sospeso PluginDemo (task-367~378)
  - 3 file modificato/i
- `87ae858` - Creare la registrazione del task di esempio positivo/negativo del plugin PluginDemo (task-367)
  - 2 file modificato/i
- `f77a102` - Archiviare session e changes del 2026-05-26
  - 7 file modificato/i

## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### Preparazione Rilascio
- `476d839` - Aggiunte attività rilascio alpha-0.2
  - Creazione task-114 (scrittura CHANGELOG) e task-115 (aggiornamento numero versione)
  - 1 file modificato

### 2026-05-15

#### Infrastruttura
- `672627b` - Aggiunto workflow sincronizzazione Gitee (con configurazione permessi)
  - Aggiornamento configurazione permessi workflow sync-from-gitee.yml
  - 1 file modificato, 7 righe aggiunte, 4 righe eliminate

- `3cd5256` - Aggiunto GitHub Actions sincronizzazione automatica codice Gitee
  - Nuovo workflow sync-from-gitee.yml
  - 1 file modificato, 50 righe aggiunte

#### Aggiornamento Documentazione
- `aa1d2ad` - Aggiornamento documentazione README/architettura/introduzione in tutte le 11 lingue, riflettendo supporto multipiattaforma SiliconLife.Fast (rif. task-112, task-113)
  - Corretta descrizione SiliconLife.Fast solo Windows nella documentazione, riflettendo il supporto multipiattaforma effettivo (Windows / macOS / Linux)
  - Aggiornamento README.md, architecture.md, getting-started.md in 11 lingue
  - SelectComponent aggiunto supporto attributo hint
  - ConfigView dropdown enumerazione con hint
  - 11 lingue localizzazione aggiunta chiave SelectSearchHint
  - 53 file modificati, 690 righe aggiunte, 194 righe eliminate

#### Sistema Attività
- `3329f3d` - Aggiunto meccanismo ispezione sistema attività + attività correzione bug localizzazione
  - Creazione task-113: correzione problema localizzazione pagina informazioni
  - Aggiornamento task-112: aggiornamento documentazione versione Fast supporto Linux
  - Archiviazione attività completate (11) in .ai-collab/archive/
  - Configurazione meccanismo ispezione completata: ispezione rapida (ogni 30 minuti) + ispezione completa (ogni giorno alle 06:00)
  - 2 file modificati, 148 righe aggiunte, 171 righe eliminate

#### Framework di Collaborazione
- `6038e22` - Registrazione coze-agent nel registro collaborazione .ai-collab
  - Aggiunte informazioni registrazione AI residente piattaforma Coze
  - 1 file modificato

### 2026-05-14

#### Framework Collaborazione AI
- `7344fbb` - Rimozione modalità handoff, passaggio a guida elenco attività (v2.0)
  - Ristrutturazione directory .ai-collab, da modalità handoff a guida elenco attività
  - Nuovo file core elenco attività tasks.json
  - Nuovo activity log operazioni
  - Nuove directory changes/ e sessions/

- `589a48e` - Aggiunto record sessione .ai-collab
  - Nuovo record stato sessione collaborazione AI

- `5481bcf` - Registrazione Qoder AI IDE nel registro collaborazione
  - Aggiunte informazioni registrazione assistente programmazione AI Qoder

- `e2d7b61` - Completamento relatedCommit e changes commitHash in tasks.json
  - Miglioramento associazione metadati attività

- `a087f0c` - Accettazione tutte le attività task-101~110
  - Conferma completamento tutte le 10 correzioni attività

#### Correzioni Bug
- `fac9435` - Completamento tutte le 10 correzioni e implementazioni attività task-101~110
  - Corretto testo suggerimento mancante nel componente selezione ricerca
  - Corretto problema localizzazione pagina informazioni
  - Corretto errore JS ricerca sistema aiuto
  - 39 file modificati, 684 righe aggiunte, 121 righe eliminate

- `c46dfbc` - Completamento tutte le attività in sospeso (task-001~006)
  - Completamento 6 attività in sospeso iniziali

- `ec176b2` - Sovrascrittura elenco attività - Revisione codice ha rilevato 10 nuovi bug
  - Creazione task-101~110 per un totale di 10 nuove attività

#### Rifattorizzazione
- `ab15915` - Unificazione intestazioni copyright + correzione BOM HelpController e JS ricerca HelpView
  - Unificazione intestazione copyright Apache 2.0 in tutti i file sorgente C#
  - Correzione problema codifica BOM HelpController
  - Correzione errore JavaScript ricerca HelpView

#### Nuove Funzionalità
- `18a6f5d` - Creazione server capacità browser MCP (rif. task-111)
  - Nuovo progetto SiliconLife.McpServer
  - Implementazione server MCP automazione browser Playwright

- `9eb251a` - Rimozione modulo SiliconLife.McpServer (rif. task-111)
  - Rimozione server MCP indipendente, funzionalità integrata nel progetto principale

### 2026-05-13

#### Localizzazione
- `7a62590` - Aggiunto supporto localizzazione polacco
  - Nuova implementazione localizzazione pl-PL polacco (PlPL.cs, 1089 righe)
  - Nuova localizzazione documentazione aiuto polacco (HelpLocalizationPlPL.cs, 3972 righe)
  - Nuovo supporto calendario storico cinese polacco (ChineseHistoricalPlPL.cs, 600 righe)
  - Nuova localizzazione tray polacco (TrayPlPL.cs, 135 righe)
  - Nuovo set completo documentazione polacco (15 documenti)
  - Enumerazione Language aggiunto polacco
  - 35 file modificati, 14379 righe aggiunte, 11 righe eliminate

- `51f9c8e` - Aggiornamento riferimenti AI Ark e miglioramento terminologia nella documentazione
  - Aggiornamento terminologia client AI nella documentazione multilingue

- `7587c12` - Aggiunte voci registro modifiche per tutte le lingue
  - Aggiornamento sincronizzato registro modifiche di tutte le versioni linguistiche

#### Migrazione Sistema Finestre
- `b49a07d` - Migrazione a modalità finestra residente Avalonia
  - Rimozione dipendenza Windows Forms, migrazione completa al framework Avalonia UI
  - Finestra di stato visualizzata correttamente su Linux (verifica desktop remoto)
  - Aggiunta controlli finestra: menu contestuale, doppio clic per aprire Web, pulsante chiusura
  - Aggiunto framework collaborazione multi-AI (.ai-collab/)
  - Correzione inizializzazione icona tray (degradazione elegante)
  - Nuovi App.axaml e App.cs punto ingresso applicazione Avalonia
  - 13 file modificati, 1442 righe aggiunte, 541 righe eliminate

- `d335aaf` - Finestra piattaforma Linux sempre visibile + dialogo conferma chiusura
  - Su Linux visualizzazione automatica finestra di stato (nessuna icona tray)
  - Su Linux dialogo di conferma alla chiusura della finestra
  - Su Windows/macOS mantenuto comportamento tray originale
  - Supporto parametro --no-tray per disabilitare forzatamente il tray
  - Nuovo metodo ShowMessageBoxAsync per dialogo di conferma
  - 3 file modificati, 206 righe aggiunte, 5 righe eliminate

#### Rifattorizzazione Sistema Tray
- `841d384` - Rifattorizzazione sistema tray e inizializzazione framework collaborazione AI
  - Semplificazione TrayLocalizationBase rimozione proprietà non utilizzate
  - Aggiunta voce localizzazione ShowStatus
  - App.cs aggiunto click icona tray mostra finestra di stato, voci menu localizzate
  - Program.cs spostamento inizializzazione icona tray in StartAsync
  - TrayStatusWindow nasconde anziché uscire alla chiusura
  - Registrazione trae-glm5 e catpaw nel framework collaborazione .ai-collab
  - Aggiornamento .gitignore per garantire tracciamento di tutti i file .ai-collab
  - 22 file modificati, 178 righe aggiunte, 1226 righe eliminate

#### Documentazione
- `43653bc` - Aggiornamento descrizione repository e registro AI
  - Aggiornamento README progetto e informazioni registrazione .ai-collab

### 2026-05-12

#### Vista Web Sistema Attività
- `0891b3c` - Aggiunta vista dettagli esecuzione attività e cronologia
  - Nuova TaskExecutionDetailView vista dettagli esecuzione attività
  - Nuova TaskExecutionHistoryView vista cronologia esecuzione attività
  - TaskController aggiunte interfacce query dettagli esecuzione e cronologia
  - Nuovo TaskViewModel modello vista attività
  - Miglioramento TaskCenter centro attività
  - Aggiornamento TaskSystem sistema attività
  - 9 lingue localizzazione aggiunte chiavi relative alle attività
  - 26 file modificati, 803 righe aggiunte, 55 righe eliminate

### 2026-05-11

#### Rifattorizzazione Architettura Componenti Web
- `5e687ad` - Migrazione rendering componenti da stringa a H-tree
  - Metodi rendering ComponentBase migrati da modalità stringa a struttura H-tree
  - Tutti i 28 componenti adattati alla nuova architettura di rendering (A, Accordion, Button, Calendar, Card, Chart, ecc.)
  - Ristrutturazione significativa SelectComponent (889 righe migliorate)
  - Aggiornamento sincronizzato controller e viste
  - 33 file modificati, 667 righe aggiunte, 435 righe eliminate

- `bfd332d` - Migrazione Style da stringa a stili inline CssBuilder
  - Nuovo builder stili CssBuilder
  - Sistema stili ComponentBase migrato da stringa a CssBuilder strutturato
  - Miglioramento significativo LoadingComponent (103 righe aggiunte)
  - Migrazione stili controller ConfigController, LogController, MemoryController
  - Migrazione stili viste ChatView, ConfigView, LogView, MemoryView
  - 37 file modificati, 351 righe aggiunte, 157 righe eliminate

#### Ottimizzazione Sistema Archiviazione
- `d67a7ee` - Ottimizzazione query QueryLatest per grandi dataset
  - Ottimizzazione prestazioni metodo QueryLatest SpeedyTimeStorage
  - Miglioramento provider log SpeedyLoggerProvider
  - 2 file modificati, 44 righe aggiunte, 5 righe eliminate

#### Rifattorizzazione Sistema Calendario
- `9629f88` - Estrazione TimerExecution e miglioramento viste web timer
  - Estrazione logica TimerExecution da TimerSystem (175 righe rimosse)
  - Miglioramento significativo SelectComponent (427 righe migliorate)
  - Miglioramento TimerController e viste timer
  - Aggiornamento ContextManager gestore contesto
  - 12 file modificati, 458 righe aggiunte, 267 righe eliminate

#### Localizzazione
- `5d8ca79` - Aggiunta chiave localizzazione LogsLoading
  - 9 lingue aggiunta chiave LogsLoading
  - Nuova definizione nella classe base DefaultLocalizationBase
  - 11 file modificati, 15 righe aggiunte

### 2026-05-10

#### Rifattorizzazione Sistema Attività
- `54394f6` - Unione sistema attività e cicli cronologia chat
  - Ristrutturazione significativa ProjectTaskSystem sistema attività progetto (411 righe rifattorizzate)
  - Semplificazione TaskSystem sistema attività (254 righe rifattorizzate)
  - Rifattorizzazione TaskCenter centro attività (188 righe migliorate)
  - Ottimizzazione ContextManager gestore contesto (347 righe rifattorizzate)
  - Miglioramento DefaultSiliconBeing essere di silicio
  - Integrazione attività TimerSystem sistema timer
  - Aggiornamento interfaccia IWorkNoteStorage
  - Adattamento SpeedyWorkNoteStorage e FileSystemWorkNoteStorage
  - 16 file modificati, 648 righe aggiunte, 897 righe eliminate

### 2026-05-09

#### Miglioramento Interfaccia Web
- `bc50dd7` - Miglioramento vista chat e aggiunta funzionalità audit
  - Nuovo AuditController controller audit (261 righe)
  - Nuova AuditView vista audit (379 righe)
  - Nuovo AuditViewModel modello vista audit
  - Miglioramento significativo ChatView vista chat (171 righe migliorate)
  - Aggiornamento ChatController controller chat
  - Miglioramento componente MarkdownEditorComponent
  - Miglioramento InitController controller inizializzazione
  - Nuove funzionalità ChatSystem sistema chat
  - 14 file modificati, 1030 righe aggiunte, 112 righe eliminate

- `c9babce` - Miglioramento rendering chiamate strumento nella vista chat
  - Miglioramento rendering blocchi chiamate strumento ChatView
  - 1 file modificato, 54 righe aggiunte, 11 righe eliminate

#### Sistema Scenari Strumenti AI
- `ff2eddd` - Implementazione sistema filtraggio scenari strumenti
  - Nuovo ToolScenarioAttribute attributo scenario strumenti (36 righe)
  - Nuovo ChatOnlyAttribute attributo solo scenario chat (19 righe)
  - ToolManager gestore strumenti aggiunta funzionalità filtraggio scenari (40 righe)
  - ContextManager gestore contesto adattato al filtraggio scenari
  - 4 file modificati, 115 righe aggiunte, 30 righe eliminate

- `5709a33` - Aggiunta attributi scenario alle classi strumenti
  - 24 classi strumenti aggiunta annotazione attributo ToolScenario
  - Inclusi strumenti calendario, chat, configurazione, curatore, database, disco, compilazione dinamica, ecc.
  - 24 file modificati, 46 righe aggiunte, 20 righe eliminate

#### Rifattorizzazione Sistema Attività
- `2f19a5f` - Rifattorizzazione sistema attività con TaskCenter e TaskEnumerator
  - Nuovo TaskCenter centro attività (235 righe)
  - Nuovo TaskEnumerator enumeratore attività (297 righe)
  - Ristrutturazione semplificazione TaskSystem sistema attività
  - Adattamento DefaultSiliconBeing essere di silicio alla nuova architettura
  - Aggiornamento DefaultSiliconBeingFactory factory
  - Miglioramento SiliconBeingBase classe base
  - 7 file modificati, 796 righe aggiunte, 275 righe eliminate

#### Migrazione Sistema Permessi
- `a06ed09` - Migrazione sistema IM e permessi al progetto App
  - PermissionRequestQueue migrato da Default/Fast al progetto App (443 righe aggiunte)
  - Rimozione WebUIProvider versione Default (403 righe eliminate)
  - Rimozione HelpTool versione Default (194 righe eliminate)
  - Rimozione PermissionRequestQueue duplicato versioni Default/Fast
  - Rimozione IMPermissionAskHandler versione Default
  - Aggiornamento PermissionRequestController controller
  - 14 file modificati, 496 righe aggiunte, 1183 righe eliminate

#### Ottimizzazione Contesto AI
- `4c8aaff` - Ottimizzazione gestore contesto e miglioramento localizzatore servizi
  - Semplificazione ottimizzazione ContextManager gestore contesto
  - Miglioramento ServiceLocator localizzatore servizi (36 righe aggiunte)
  - Miglioramento ToolManager gestore strumenti (34 righe aggiunte)
  - Miglioramento client DashScopeClient e VolcengineArkClient
  - Aggiornamento esecutori (CommandLine, Disk, Network)
  - 8 file modificati, 116 righe aggiunte, 98 righe eliminate

#### Localizzazione
- `5c5eef7` - Aggiunta chiavi localizzazione audit e attività
  - DefaultLocalizationBase aggiunta 127 righe definizioni localizzazione
  - 9 lingue aggiunta chiavi relative ad audit e attività (26 righe ciascuna)
  - 11 file modificati, 387 righe aggiunte

#### Configurazione Progetto
- `2067db6` - Aggiornamento configurazione progetto e regole gitignore
  - Aggiornamento regole .gitignore
  - Miglioramento DefaultConfigData e Fast DefaultConfigData configurazione
  - Miglioramento SpeedyWorkNoteStorage archiviazione
  - Miglioramento SpeedyPack core
  - 5 file modificati, 32 righe aggiunte, 6 righe eliminate

### 2026-05-07

#### Localizzazione Italiana
- `8adc18c` - Aggiunto supporto localizzazione italiana e aggiornamento documentazione multilingue
  - Nuova localizzazione it-IT italiana
  - Nuova implementazione localizzazione ItIT (1909 righe)
  - Nuovo supporto calendario storico cinese italiano ChineseHistoricalItIT (586 righe)
  - Nuova localizzazione tray italiano TrayItIT (135 righe)
  - Nuovo set completo documentazione italiana (14 documenti: README, riferimento API, architettura, sistema calendario, registro modifiche, guida contributi, ecc.)
  - Aggiornamento documentazione architettura, guida sviluppo, guida introduttiva, ecc. in tutte le versioni linguistiche
  - Enumerazione Language aggiunto italiano
  - 86 file modificati, 11573 righe aggiunte, 769 righe eliminate

#### Sincronizzazione Documentazione
- `12a5deb` - Aggiornamento documentazione multilingue architettura, registro modifiche e guida esseri di silicio
  - Aggiornamento README in 8 lingue
  - Aggiornamento documentazione architettura in 8 lingue
  - Aggiornamento registro modifiche in 8 lingue
  - Aggiornamento guida esseri di silicio in 8 lingue
  - Aggiornamento riferimento strumenti in 8 lingue
  - Ristrutturazione glossario
  - 46 file modificati, 1697 righe aggiunte, 442 righe eliminate

### 2026-05-06

#### Ristrutturazione Moduli su Larga Scala
- `eeb3be6` - Ristrutturazione e riorganizzazione moduli su larga scala
  - Ristrutturazione struttura progetto SiliconLife.App
  - Riorganizzazione progetto SiliconLife.Fast
  - Riorganizzazione progetto SiliconLife.Default
  - Riorganizzazione modulo condiviso SiliconLife.Common
  - Riorganizzazione modulo core SiliconLife.Core
  - Riorganizzazione motore archiviazione SiliconLife.Speedy
  - Riorganizzazione strumento gestione SiliconLife.Speedy.Manager
  - 119 file modificati, 6926 righe aggiunte, 3066 righe eliminate

### 2026-05-04

#### Client AI
- `24d2c86` - Aggiunto VolcengineArkClient e sostituito Audit con Usage tracking
  - Nuovo VolcengineArkClient client AI Volcengine Ark
  - Supporto modalità streaming e non streaming
  - Controllo velocità a due livelli integrato (auto-controllo velocità + limite velocità server)
  - Compatibile con protocollo API OpenAI
  - Sistema Audit sostituito con Usage tracking
  - 24 file modificati, 802 righe aggiunte, 21 righe eliminate

#### Sistema Strumenti
- `f27650a` - Aggiunto strumento hot reload per riavvio automatico Fast
  - Nuovo HotReloadTool strumento hot reload
  - Supporto compilazione online, aggiornamento e riavvio SiliconLife.Fast
  - Nuovo HotReload.exe aggiornatore indipendente
  - Meccanismo copia file sicuro (non sovrascrive se stesso)
  - Chiusura elegante e attesa rilascio porta
  - 9 file modificati, 581 righe aggiunte

#### Localizzazione
- `6a5aad8` - Aggiornamento tutti i file e aggiunto supporto localizzazione francese
  - Nuova localizzazione fr-FR francese
  - Aggiornamento tutte le versioni linguistiche
  - Traduzione documentazione aiuto francese
  - Traduzione interfaccia francese
  - 100+ file modificati

### 2026-05-03

#### Infrastruttura Progetto
- `2664b0c` - Aggiornamento infrastruttura progetto e dipendenze
  - SiliconLife.Speedy.Manager aggiunta interfaccia gestione WPF (MainForm.Designer.cs, MainForm.resx)
  - Nuova risorsa icona slc.ico (1.5MB)
  - Miglioramento significativo scansione sicurezza PluginLoader (622 righe aggiunte)
  - Nuovo PermissionedStreamFactory factory stream permessi (779 righe)
  - Nuovo PermissionRequestQueue coda richieste permessi (versioni Default e Fast)
  - Nuovo DebugLoggerProvider provider log debug
  - Miglioramento ConfigDataBase classe base configurazione
  - ToolManager aggiunta funzionalità scansione strumenti plugin (ScanAllPluginAssemblies)
  - Miglioramento gestione ciclo di vita SiliconBeingManager
  - Miglioramento significativo DashScopeClient client AI Alibaba Cloud (227 righe aggiunte)
  - Miglioramento DefaultSiliconBeingFactory factory
  - Aggiornamento viste e controller Web (ChatView, WorkNoteView, PermissionRequestController)
  - 9 lingue localizzazione aggiunte chiavi
  - 35 file modificati, 28080 righe aggiunte, 336 righe eliminate

### 2026-05-02

#### Miglioramento Client AI
- `c16f99f` - Aggiornamento client AI, Web UI e componenti archiviazione
  - Miglioramento significativo DashScopeClient client Alibaba Cloud
  - Ottimizzazione SpeedyPackAutoCompactor compressore automatico
  - Miglioramento classe base viste Web e BeingView
  - 6 file modificati, 240 righe aggiunte, 81 righe eliminate

#### Sistema Plugin
- `242dc98` - Aggiunta elenco plugin nella pagina informazioni
  - AboutController aggiunta visualizzazione informazioni plugin
  - AboutViewModel aggiunto modello dati plugin
  - AboutView aggiunto rendering elenco plugin
  - 9 lingue localizzazione aggiunte chiavi relative ai plugin
  - 14 file modificati, 160 righe aggiunte, 1 riga eliminata

#### Ottimizzazione AI
- `147f8f4` - Semplificazione testo prompt memoria contesto
  - Ottimizzazione prompt AI ContextManager
  - 1 file modificato, 1 riga aggiunta, 1 riga eliminata

#### Ottimizzazione Archiviazione Speedy
- `8bda2d3` - Aggiornamento archiviazione Speedy e implementazione controller memoria
  - Correzione intervallo SpeedyPackAutoCompactor
  - Ottimizzazione gestione percorso SpeedyTimeStorage
  - Miglioramento MemoryController controller memoria
  - Aggiornamento UI SpeedyPack.Manager
  - 4 file modificati, 21 righe aggiunte, 18 righe eliminate

#### Miglioramento Tray
- `8972654` - Miglioramento supporto localizzazione finestra stato tray
  - 9 lingue localizzazione tray aggiunto ingresso gestione Speedy
  - TrayStatusWindow aggiunta voce menu gestione Speedy
  - 11 file modificati, 72 righe aggiunte

#### Ottimizzazione Speedy.Manager
- `6f5db09` - Ottimizzazione UI gestore SpeedyPack e componenti interni
  - Ristrutturazione interfaccia MainForm
  - Ottimizzazione gestione memoria FreeList
  - Miglioramento coda scrittura WriteQueue
  - Ottimizzazione core SpeedyPack
  - 5 file modificati, 96 righe aggiunte, 88 righe eliminate

#### Miglioramento Sistema Archiviazione
- `57f9d5d` - Miglioramento sistema archiviazione, aggiunta compressione automatica e supporto date incomplete
  - Nuovo SpeedyPackAutoCompactor timer compressione automatica (intervallo 30 minuti)
  - Miglioramento SpeedyPackRegistry gestore singleton
  - Miglioramento adattamento SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage
  - SpeedyPack aggiunto FreeList gestione spazio libero (149 righe)
  - Ristrutturazione ottimizzazione PackFileWriter scrittore
  - Miglioramento WriteOperation, WriteQueue coda scrittura
  - Estensione SpeedyPackOptions opzioni configurazione
  - IncompleteDate aggiunto metodi di confronto
  - Miglioramento PluginLoader caricatore plugin
  - Aggiornamento flusso inizializzazione Program.cs versioni Default e Fast
  - Semplificazione DefaultConfigData dati configurazione
  - Semplificazione KnowledgeNetwork rete di conoscenza
  - Ottimizzazione ChatController, MemoryController controller
  - Miglioramento funzionalità SpeedyPack.Manager MainForm
  - 22 file modificati, 639 righe aggiunte, 253 righe eliminate

#### Aggiornamento Speedy.Manager
- `b04ed33` - Aggiornamento file Speedy.Manager

### 2026-05-01

#### Ristrutturazione Architettura: Archiviazione Speedy Sostituisce LiteDB
- `6600972` - Sostituzione LiteDB con archiviazione Speedy, aggiunta sistema plugin e progetto Speedy
  - **Nuovo progetto SiliconLife.Speedy**: Motore archiviazione .spk ad alte prestazioni
    - Classe core SpeedyPack (489 righe): mappa directory in memoria + cache voci + coda scrittura asincrona
    - Classe configurazione SpeedyPackOptions: TTL cache, numero massimo voci cache, modalità sola lettura
    - Interfaccia transazioni IPackTransaction: supporto operazioni di scrittura atomica
    - Classe informazioni file SpkFileInfo
    - Directory Internal: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Dipendenza MessagePack 3.1.4 per serializzazione binaria (compressione LZ4)
  - **Nuovo progetto SiliconLife.Speedy.Manager**: Strumento gestione WPF
    - Architettura MVVM: MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel, ecc.
    - Livello servizi: PackService, FileDialogService, RecentFilesService, NotificationService
    - Convertitori: BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - Viste: MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - Dialoghi: FileInfoDialog, ImportDialog, NewEntryDialog
  - **Migrazione archiviazione SiliconLife.Fast**: LiteDB → SpeedyPack
    - Nuovo SpeedyStorage (adattatore IStorage)
    - Nuovo SpeedyTimeStorage (adattatore ITimeStorage)
    - Nuovo SpeedyWorkNoteStorage (adattatore IWorkNoteStorage)
    - Nuovo SpeedyPackRegistry (gestione singleton a livello processo)
    - Nuovo SpeedyPackAutoCompactor (timer compressione automatica)
    - Rimozione implementazioni archiviazione LiteDB correlate (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Rimozione codice correlato finestra gestione LiteDB
  - **Sistema Plugin**:
    - Nuova interfaccia IPlugin (Core/Plugins/IPlugin.cs)
    - Nuovo PluginLoader caricatore plugin (Core/Plugins/PluginLoader.cs)
    - Supporto caricamento plugin DLL da directory
    - Scansione sicurezza: controllo namespace proibiti (System.IO, System.Net, Microsoft.CodeAnalysis, ecc.)
    - Whitelist assembly attendibili (Google.Protobuf, Newtonsoft.Json, MessagePack, ecc.)
    - Caricamento isolato con AssemblyLoadContext personalizzato
    - ToolManager aggiunto metodo ScanAllPluginAssemblies
    - CoreHost integrazione caricatore plugin
  - 119 file modificati, 6926 righe aggiunte, 3066 righe eliminate

#### Miglioramento Esseri di Silicio
- `3aef4c3` - Aggiunto stato attività Stopped e miglioramento gestione errori
  - Esseri di Silicio aggiunto stato Stopped
  - Miglioramento gestione errori e meccanismo di ripristino

#### Aggiornamento Localizzazione
- `513c65d` - Aggiornamento tutte le versioni linguistiche e documentazione
  - Nuovo MarkdownEditorComponent componente (625 righe)
  - Nuovo DetailsComponent componente (130 righe)
  - Nuovo AccordionComponent componente fisarmonica (285 righe)
  - Aggiornamento controller BeingController, ChatController, MemoryController, PermissionController
  - Ristrutturazione viste BeingView, ChatView, MemoryView, SoulEditorView
  - Rimozione vecchio MarkdownEditorView
  - Migrazione componenti InitController
  - 115 file modificati, 5761 righe aggiunte, 2362 righe eliminate

### 2026-04-30

#### Funzionalità Tray di Sistema
- `101b203` - Implementazione finestra stato tray e ApplicationContext
  - Nuove risorse icona tray (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implementazione TrayStatusWindow finestra stato
  - Supporto localizzazione tray in 9 lingue (TrayCsCZ, TrayDeDE, TrayEnUS, ecc.)
  - TrayLocalizationBase classe base astratta
  - 24 file modificati, 27995 righe aggiunte, 1 riga eliminata (inclusi file risorse)

#### Architettura UI a Componenti
- `e61cfaa` - Completamento architettura UI a componenti, implementazione 24 componenti
  - Fase MVP (8): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Seconda fase (6): Accordion, Card, Tabs, Table, Modal, Message
  - Terza fase (5): Calendar, Tree, Chart, FileUpload, RichText
  - Nuove classi ausiliarie Js, Behavior, DomUpdate, ecc.
  - 25 file modificati, 2666 righe aggiunte

- `7449e51` - Miglioramento sistema componenti e aggiunta nuovi temi skin
  - Miglioramento componenti A, Button, Div, Form, Input, ecc.
  - Aggiunta 3 temi skin: HighContrast (alto contrasto), Light (chiaro), Minimal (minimalista)
  - Aggiornamento skin esistenti (Admin, Chat, Creative, Dev)
  - Migrazione componenti InitController
  - 32 file modificati, 1466 righe aggiunte, 1238 righe eliminate

- `1ba8636` - Avvio migrazione componenti InitController (in corso)
  - 9 file modificati, 574 righe aggiunte, 145 righe eliminate

#### Unificazione Sistema Archiviazione
- `895dff9` - Unificazione soul.md e state.json per utilizzare interfaccia IStorage
  - DefaultSiliconBeing utilizza IStorage per leggere/scrivere file anima e stato
  - Nuovo StateFileManager gestore file di stato
  - Rifattorizzazione SoulFileManager adattamento a IStorage
  - 8 file modificati, 201 righe aggiunte, 116 righe eliminate

#### Miglioramento Gestione LiteDB
- `a34bef4` - Aggiunto LiteDBManager e miglioramento localizzazione tray
  - Menu tray aggiunto ingresso gestione LiteDB
  - Aggiornamento localizzazione tray in 9 lingue
  - 10 file modificati, 196 righe aggiunte

- `c4a79ca` - Aggiunta factory localizzazione sensibile al linguaggio per finestra gestione LiteDB
  - 1 file modificato, 78 righe aggiunte

- `5ebc55e` - Conversione LiteDBAdminLocalization in classe base astratta
  - 10 file modificati, 1356 righe aggiunte

#### Correzione Sistema Configurazione
- `2da5256` - Aggiunto metodo astratto ConfigExists e correzione record configurazione LiteDB duplicati
  - ConfigDataBase aggiunto metodo ConfigExists
  - Implementazione DefaultConfigData versione Fast controllo esistenza configurazione LiteDB
  - Correzione problema chiavi configurazione LiteDB duplicate
  - 9 file modificati, 210 righe aggiunte, 2 righe eliminate

#### Ottimizzazione Chat e Viste
- `d3618ec` - Ottimizzazione sessioni chat, sistema archiviazione, modello temporale e classe base viste
  - Ottimizzazione BroadcastChannel, GroupChatSession, SingleChatSession
  - ITimeStorage aggiunto metodi di query
  - Aggiornamento sincronizzato FileSystemStorage e LiteDBStorage
  - Rifattorizzazione ottimizzazione ViewBase (versioni Default e Fast)
  - 11 file modificati, 622 righe aggiunte, 392 righe eliminate

### 2026-04-29

#### Ristrutturazione Architettura: Estrazione Moduli Condivisi
- `a102428` - Migrazione moduli condivisi da SiliconLife.Default a SiliconLife.Common
  - Estrazione 32 implementazioni calendario nel progetto Common
  - Estrazione classi base localizzazione e 21 implementazioni linguistiche nel progetto Common
  - Estrazione gestore permessi, implementazione predefinita essere di silicio nel progetto Common
  - Estrazione 23 implementazioni strumenti integrati nel progetto Common
  - Estrazione implementazione Playwright WebView nel progetto Common
  - Aggiornamento namespace a SiliconLife.Collective
  - 122 file modificati, 586 righe aggiunte, 343 righe eliminate

#### Miglioramento Qualità Codice
- `17566fe` - Sostituzione Console.WriteLine con sistema log nei progetti Core, Common e Default
  - Aggiornamento 6 file ContextManager, AuditLogger, DefaultConfigData, ecc.
  - Unificazione utilizzo interfaccia ILogger, miglioramento manutenibilità codice
  - 6 file modificati, 12 righe aggiunte, 8 righe eliminate

#### SiliconLife.Fast Versione ad Alte Prestazioni
- `54a0307` - Aggiunto progetto SiliconLife.Fast e completata correzione compilazione
  - Punto ingresso completo applicazione Windows Forms
  - Supporto tray di sistema (NotifyIcon)
  - Porting tutti i controller Web UI (20+)
  - Porting tutti i componenti vista Web
  - Porting 4 temi skin (Admin, Chat, Creative, Dev)
  - 125 file modificati, 61186 righe aggiunte

#### Sincronizzazione Documentazione Multilingue
- `265fde8` - Sincronizzazione documentazione architettura doppia versione in tutte le lingue
  - Aggiornamento architecture.md, changelog.md in 7 lingue
  - Aggiornamento contributing.md in 6 lingue
  - Aggiornamento getting-started.md, roadmap.md in 7 lingue
  - 47 file modificati, 1214 righe aggiunte, 38 righe eliminate

#### Sistema Archiviazione LiteDB (Versione Fast)
- `4704862` - Aggiunta dipendenza LiteDB e infrastruttura
  - Nuovo LiteDBManager classe gestione
  - Nuovo LiteDBModels modello dati
  - 3 file modificati, 252 righe aggiunte

- `4220036` - Implementazione classi archiviazione LiteDB
  - LiteDBStorage: implementazione interfaccia IStorage
  - LiteDBTimeStorage: implementazione interfaccia ITimeStorage
  - LiteDBWorkNoteStorage: implementazione interfaccia IWorkNoteStorage
  - 3 file modificati, 581 righe aggiunte

- `38ebd23` - Migrazione configurazione e sistema log a LiteDB
  - Adattamento DefaultConfigData archiviazione LiteDB
  - Nuovo LiteDBLoggerProvider provider log
  - 2 file modificati, 203 righe aggiunte, 67 righe eliminate

- `e687157` - Migrazione rete di conoscenza da file system a LiteDB
  - Ristrutturazione completa KnowledgeNetwork, utilizzo archiviazione LiteDB per dati tripli
  - 1 file modificato, 231 righe aggiunte, 72 righe eliminate

- `4220169` - Integrazione archiviazione LiteDB in Program e ProjectManager
  - Inizializzazione archiviazione LiteDB in Program.cs
  - Adattamento ProjectManager archiviazione note di lavoro LiteDB
  - 2 file modificati, 40 righe aggiunte, 17 righe eliminate

- `5f3a709` - Rimozione implementazioni archiviazione file system deprecate
  - Eliminazione FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage, ecc.
  - 6 file modificati, 1518 righe eliminate

- `e1a4ef2` - docs: aggiunto identificatore versione v0.1.0-alpha a tutta la documentazione
  - 127 file modificati, 2297 righe aggiunte, 2471 righe eliminate

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Ristrutturazione Sistema Archiviazione
- `8dd26e3` - Unificazione interfaccia ITimeStorage utilizzo IncompleteDate e aggiunta API query gerarchica
  - Rimozione metodi overload DateTime nell'interfaccia ITimeStorage, unificazione utilizzo IncompleteDate
  - IncompleteDate aggiunto metodo di confronto CompareTo(DateTime) e metodo di espansione Expand()
  - Nuove API query gerarchica GetEarliestTimestamp(), GetLatestTimestamp()
  - Nuovi metodi HasSummary() e QueryWithLevel(), supporto query per livello temporale
  - Ristrutturazione algoritmo compressione Memory.cs, utilizzo nuova API query gerarchica per migliorare efficienza
  - Implementazione completa nuovi metodi interfaccia in FileSystemTimeStorage.cs
  - Aggiornamento sincronizzato tutti i chiamanti: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, ecc.
  - Aggiornamento sistema strumenti: HelpTool, LogTool, TokenAuditTool adattati alla nuova interfaccia
  - Aggiornamento controller Web: AuditController, ChatController, ChatHistoryController adattati alla nuova interfaccia
  - 41 file modificati, 1820 righe aggiunte, 903 righe eliminate

### 2026-04-27

#### Miglioramento Sistema Documentazione Aiuto
- `9989d79` - Aggiornamento localizzazione, sistema aiuto e viste Web
  - Nuovo IAIClientFactoryHelp.cs interfaccia documentazione aiuto factory client AI
  - Completamento traduzione documentazione aiuto in 9 lingue
  - HelpTopics.cs aggiunto 40 definizioni argomenti aiuto
  - Aggiornamento completo viste Web: InitController, AuditView, ConfigView, KnowledgeView, LogView, ecc.
  - Miglioramento sistema localizzazione: tutte le versioni linguistiche aggiunte nuove chiavi localizzazione
  - Aggiornamento factory client AI: miglioramento DashScopeClientFactory, OllamaClientFactory
  - 30 file modificati, 10086 righe aggiunte, 15 righe eliminate

#### Nuovi Contenuti Documentazione Aiuto
- `e7afe94` - Aggiunta documentazione aiuto file anima e log audit
  - Nuova documentazione aiuto gestione file anima
  - Nuova documentazione aiuto log audit
  - HelpTopics.cs aggiunto definizioni argomenti
  - Ristrutturazione significativa HelpView.cs, miglioramento logica rendering documentazione
  - Ristrutturazione PermissionView.cs, miglioramento interfaccia gestione permessi
  - Miglioramento moduli core: SiliconBeingManager, TaskSystem, ToolManager
  - Ristruttorizzazione TaskTool.cs, miglioramento funzionalità gestione attività
  - Aggiornamento completo viste Web: tutti i componenti vista aggiornati sincronizzati
  - Semplificazione HelpController.cs, ottimizzazione logica controller
  - 30 file modificati, 7100 righe aggiunte, 897 righe eliminate

### 2026-04-26

#### Sistema Documentazione Aiuto
- `07895d7` - Miglioramento sistema documentazione aiuto, aggiunta 3 documenti e completamento traduzione 9 lingue
  - Aggiunta sistema memoria, installazione configurazione Ollama, guida piattaforma Alibaba Cloud Bailian
  - Completamento traduzione tutti i 10 documenti aiuto in 9 lingue
  - Semplificazione logica rendering HelpView
  - 18 file modificati, 14418 righe aggiunte, 1364 righe eliminate

#### Localizzazione Tedesca
- `0cfd8a1` - Aggiunto supporto localizzazione completo tedesco (de-DE)
  - File localizzazione tedesca completo
  - Nuovo supporto calendario storico cinese tedesco
  - Nuova traduzione documentazione aiuto tedesca
  - Sincronizzazione completa documentazione in 9 lingue
  - 135 file modificati, 26186 righe aggiunte, 14371 righe eliminate

#### Sincronizzazione Documentazione
- `3aada7d` - Sincronizzazione documentazione cinese tradizionale (zh-HK) con cinese semplificato
  - 3 file modificati, 519 righe aggiunte, 422 righe eliminate
- `2f6abff` - Aggiunta localizzazione nome visualizzato strumento aiuto per tutte le lingue
  - 7 file modificati, 47 righe aggiunte, 7 righe eliminate

#### Ristrutturazione Sistema Conoscenza
- `60944fe` - Unificazione namespace a SiliconLife.Collective
  - 8 file modificati, 5 righe aggiunte, 8 righe eliminate
- `69c51c5` - Aggiunta sistema documentazione aiuto e traduzione commenti codice in inglese
  - 29 file modificati, 3385 righe aggiunte, 22 righe eliminate

### 2026-04-25

#### Automazione Browser WebView
- `41757c3` - Implementazione automazione browser WebView multipiattaforma basata su Playwright
  - 6 file modificati, 1152 righe aggiunte

#### Aggiornamento Documentazione
- `0ff797b` - Aggiunta documentazione KnowledgeTool e WorkNoteTool (7 lingue)
  - 28 file modificati, 4983 righe aggiunte
- `ad77415` - Aggiornamento tutti i file changelog, aggiunta record storico Git 2026-04-25
  - 7 file modificati, 168 righe aggiunte

#### Gestione Area di Lavoro Progetto
- `785c551` - Implementazione gestione area di lavoro progetto, inclusi note di lavoro e sistema attività
  - Nuovo sistema gestione area di lavoro progetto
  - Funzionalità note di lavoro per tracciare avanzamento progetto
  - Integrazione sistema gestione attività
  - 29 file modificati, 4256 righe aggiunte, 36 righe eliminate

#### Localizzazione Ceca
- `b4bbf39` - Aggiunta localizzazione completa ceca (cs-CZ) e aggiornamento documentazione tutte le lingue
  - 116 file modificati, 4933 righe aggiunte, 222 righe eliminate
- `faf078f` - Correzione errore compilazione localizzazione ceca
  - 3 file modificati, 910 righe aggiunte, 1 riga eliminata

#### Miglioramento Sistema Conoscenza
- `20adaac` - Aggiunta KnowledgeTool e supporto localizzazione completa
  - 34 file modificati, 2331 righe aggiunte, 56 righe eliminate

### 2026-04-24

#### Miglioramento Sistema Gestione Memoria
- `c7b2ecc` - Miglioramento funzionalità gestione memoria, aggiunta filtraggio avanzato, statistiche e vista dettagli
  - Nuova funzionalità filtraggio avanzato memoria
  - Implementazione funzionalità statistiche memoria
  - Aggiunta pagina vista dettagli memoria
  - Supporto localizzazione multilingue (6 lingue)
  - 13 file modificati, 840 righe aggiunte, 86 righe eliminate

#### Estensione Sistema Permessi
- `4489ad6` - Aggiunta servizio meteo wttr.in alla whitelist di rete
  - Aggiornamento sincronizzato completo documentazione multilingue (6 lingue)
  - 14 file modificati, 417 righe aggiunte, 1 riga eliminata

#### Correzione Interfaccia Web
- `d9d72e9` - Correzione problema priorità CSS modale dettagli note di lavoro
  - 19 file modificati, 1744 righe aggiunte, 6 righe eliminate

#### Ottimizzazione Cronologia Chat
- `0df599c` - Correzione problema risultati strumento renderizzati come messaggi chat indipendenti
  - 1 file modificato, 222 righe aggiunte, 21 righe eliminate
- `057b09d` - Ottimizzazione visualizzazione dettagli cronologia chat, miglioramento rendering chiamate strumento
  - 3 file modificati, 389 righe aggiunte, 68 righe eliminate

#### Cronologia Esecuzione Timer
- `fa3f06f` - Aggiunta funzionalità cronologia esecuzione timer, inclusa vista dettagli
  - 8 file modificati, 937 righe aggiunte, 10 righe eliminate
- `d824835` - Aggiunta chiavi localizzazione cronologia esecuzione timer (tutte le lingue)
  - 7 file modificati, 88 righe aggiunte

#### Miglioramento Localizzazione
- `c13cb17` - Registrazione variante lingua spagnola
  - 1 file modificato, 4 righe aggiunte
- `9c44f34` - Aggiunta supporto localizzazione multilingue calendario storico cinese
  - 16 file modificati, 6049 righe aggiunte, 1 riga eliminata

#### Miglioramento Funzionalità Core
- `1e7c7b2` - Miglioramento compressione memoria e tracciamento esecuzione strumenti
  - 4 file modificati, 338 righe aggiunte, 86 righe eliminate

### 2026-04-23

#### Localizzazione Strumenti
- `192fc6e` - Aggiunta localizzazione nome strumento mancante per 5 strumenti
  - 6 file modificati, 30 righe aggiunte

#### Aggiornamento Documentazione
- `882c08f` - Aggiornamento tutti i file changelog, aggiunta record storico Git completo e rimozione numeri versione falsi
  - 45 file modificati, 8815 righe aggiunte, 1611 righe eliminate

#### Miglioramento Pagina Chat
- `65c157b` - Aggiunta indicatore di caricamento pagina chat e selezione automatica sessione Curatore
  - 10 file modificati, 211 righe aggiunte, 7 righe eliminate

#### Funzionalità Cronologia Chat
- `e483348` - Implementazione funzionalità visualizzazione cronologia chat Esseri di Silicio
  - Nuovo ChatHistoryController
  - Creazione ChatHistoryViewModel
  - Implementazione pagine ChatHistoryListView e ChatHistoryDetailView
  - Aggiunta chiavi localizzazione cronologia chat (5 lingue)
  - 12 file modificati, 1178 righe aggiunte

#### Miglioramento Controllo Flusso AI
- `30a2d4e` - Miglioramento cancellazione flusso AI, integrazione IM e inizializzazione host core
  - 11 file modificati, 387 righe aggiunte, 12 righe eliminate

#### Coda Messaggi Chat
- `db48c51` - Aggiunta coda messaggi chat, metadati file e supporto cancellazione stream
  - 4 file modificati, 357 righe aggiunte

#### Supporto Caricamento File
- `28fb344` - Implementazione dialogo sorgente file e supporto caricamento file
  - 3 file modificati, 1100 righe aggiunte, 2 righe eliminate
- `1d3e2cc` - Aggiunta stringhe localizzazione dialogo sorgente file (6 lingue)
  - 6 file modificati, 30 righe aggiunte

#### Aggiornamento Documentazione
- `8111e92` - Aggiunta link Wiki nella sezione repository del README
  - 1 file modificato, 3 righe aggiunte, 1 riga eliminata

### 2026-04-22

#### Localizzazione Documentazione
- `66c11eb` - Traduzione commenti cinesi in inglese e aggiornamento tutti i changelog
  - 11 file modificati, 373 righe aggiunte, 163 righe eliminate

#### Miglioramento Messaggi SSE
- `b574b2b` - Aggiunta senderName per messaggi storici per identificazione AI
  - 1 file modificato, 9 righe aggiunte

#### Funzionalità Chat
- `601fc14` - Aggiunta operazione mark_read per contrassegno fine sessione
  - 7 file modificati, 196 righe aggiunte, 36 righe eliminate

#### Ottimizzazione Sistema Strumenti
- `7a03a19` - Miglioramento flessibilità query conversazione LogTool
  - 1 file modificato, 57 righe aggiunte, 24 righe eliminate

#### Miglioramento Localizzazione
- `0a8d750` - Aggiunta prompt di sistema generico per comportamento proattivo Esseri di Silicio
  - 8 file modificati, 460 righe aggiunte, 48 righe eliminate

#### Ristrutturazione Sistema Log
- `2b771f3` - Disaccoppiamento LogController da I/O file, aggiunta API lettura log
  - 4 file modificati, 172 righe aggiunte, 137 righe eliminate
- `12da302` - Aggiunta filtro Esseri di Silicio per vista log
  - 9 file modificati, 147 righe aggiunte, 10 righe eliminate
- `8f6cb1e` - Aggiunta parametro beingId all'interfaccia ILogger, separazione log sistema/Esseri di Silicio
  - 47 file modificati, 524 righe aggiunte, 490 righe eliminate

#### Miglioramento Sistema Permessi
- `4c747ad` - Rifattorizzazione PermissionTool, ExecuteCodeTool, aggiunta API EvaluatePermission
  - 18 file modificati, 680 righe aggiunte, 492 righe eliminate

#### Correzioni Bug
- `1c96e99` - Correzione fallimento ricerca search_files e search_content nella directory root
  - 1 file modificato, 98 righe aggiunte, 41 righe eliminate

#### Integrazione Strumenti
- `135710d` - Rimozione SearchTool, spostamento ricerca locale in DiskTool
  - 2 file modificati, 185 righe aggiunte, 365 righe eliminate

#### Estensione Sistema Strumenti
- `70ce7fb` - Implementazione DatabaseTool per query database strutturato
  - 1 file modificato, 382 righe aggiunte
- `be29a09` - Implementazione LogTool per query storico operazioni e conversazioni
  - 1 file modificato, 298 righe aggiunte
- `4ea7702` - Implementazione PermissionTool per gestione dinamica permessi
  - 1 file modificato, 457 righe aggiunte
- `1384ff4` - Implementazione ExecuteCodeTool per esecuzione codice multilingua
  - 1 file modificato, 477 righe aggiunte
- `82d1e11` - Implementazione SearchTool per recupero informazioni
  - 1 file modificato, 363 righe aggiunte

#### Ottimizzazione Interfaccia Web
- `0675c45` - Ottimizzazione evidenziazione blocchi codice markdown nel riquadro di anteprima
  - 1 file modificato, 4 righe aggiunte, 23 righe eliminate
- `702b3f3` - Miglioramento vista attività, aggiunta badge stato e visualizzazione metadati
  - 8 file modificati, 221 righe aggiunte, 9 righe eliminate
- `6ed9a79` - Miglioramento archiviazione messaggi chat e rendering vista
  - 8 file modificati, 140 righe aggiunte, 29 righe eliminate

### 2026-04-21

#### Correzioni Bug
- `c6b518b` - Correzione passaggio messaggi timer e archiviazione messaggi chat
  - 3 file modificati, 297 righe aggiunte, 124 righe eliminate

#### Gestione Configurazione
- `4305769` - Aggiunto .gitattributes per gestione fine riga
  - 1 file modificato, 32 righe aggiunte

#### Miglioramento Interfaccia Web
- `188c6f8` - Registrazione route API elenco attività e aggiunta visualizzazione stato vuoto
  - 2 file modificati, 35 righe aggiunte, 2 righe eliminate
- `634e8ca` - Aggiunta link ritorno elenco pagina permessi
  - 1 file modificato, 16 righe aggiunte
- `6ba591d` - Aggiunto editor configurazione AI indipendente per Esseri di Silicio
  - 11 file modificati, 842 righe aggiunte, 18 righe eliminate
- `0a826f5` - Aggiunta notifica salvataggio riuscito nell'editor codice
  - 1 file modificato, 9 righe aggiunte, 2 righe eliminate
- `2940373` - Miglioramento interfaccia Web, aggiunta tooltip codice sospeso e miglioramenti UI
  - 11 file modificati, 1054 righe aggiunte, 75 righe eliminate

#### Correzione Sistema Permessi
- `592c7ab` - Correzione istanziazione callback e ordine di registrazione
  - 2 file modificati, 38 righe aggiunte, 7 righe eliminate

#### Miglioramento Sicurezza
- `833ead2` - Aggiunta verifica riferimenti assembly per compilazione dinamica
  - 4 file modificati, 135 righe aggiunte, 8 righe eliminate

#### Miglioramento Sistema Permessi
- `5879621` - Aggiunta pre-compilazione verifica callback permessi e miglioramento gestione errori
  - 21 file modificati, 617 righe aggiunte, 26 righe eliminate

#### Aggiornamento Documentazione
- `4dbf659` - Aggiornamento changelog a v0.5.1, sostituzione URL segnaposto GitHub, aggiunta mirror Gitee, localizzazione nome Bilibili per lingua, aggiornamento email
  - 32 file modificati, 489 righe aggiunte, 180 righe eliminate

#### Configurazione e Punto Ingresso
- `0fc1693` - Aggiornamento punto ingresso programma e configurazione progetto
  - 2 file modificati, 7 righe aggiunte

#### Rifattorizzazione Sistema Permessi
- `ea9179a` - Miglioramento implementazione sistema permessi
  - 5 file modificati, 358 righe aggiunte, 152 righe eliminate

#### Correzioni Bug
- `928a96d` - Correzione implementazione calcolo calendario
  - 4 file modificati, 12 righe aggiunte, 12 righe eliminate

#### AI e Calendario
- `646813e` - Miglioramento implementazione factory client AI
  - 2 file modificati, 21 righe aggiunte, 20 righe eliminate

#### Localizzazione
- `7940d9c` - Aggiunto supporto localizzazione coreano
  - 7 file modificati, 2424 righe aggiunte, 10 righe eliminate
- `4ff98ad` - Ristrutturazione documentazione, supporto multilingue
  - 81 file modificati, 23818 righe aggiunte, 1886 righe eliminate

### 2026-04-20

#### Completamento Funzionalità Core
- `28905b5` - Supporto multilingue completo, factory client AI, sistema permessi e impostazioni localizzazione
  - Sistema log con gestore, voci e diversi livelli di log
  - Sistema audit token per query e tracciamento utilizzo token
  - Factory client AI con scoperta automatica di diverse piattaforme AI
  - Sistema callback permessi con propria archiviazione
  - Implementazione logger console
  - Supporto multilingue inglese e cinese semplificato
  - WebUI messenger con WebSocket per chat in tempo reale
  - Miglioramento essere di silicio predefinito con localizzazione
  - 39 file modificati, 4670 righe aggiunte, 175 righe eliminate

### 2026-04-19

#### Timer e Calendario
- `c933fd8` - Aggiornamento localizzazione, sistema timer, viste Web e aggiunta strumenti
  - Miglior gestore localizzazione
  - Sistema pianificazione compiti temporizzati
  - Configurazione AI e gestione contesto
  - Strumento calendario con supporto 32 tipi di calendario
  - Controller Web per API calendario
  - Strumento gestione attività
  - 46 file modificati, 4018 righe aggiunte, 975 righe eliminate

**Miglioramenti Architetturali**
- Ridisegno architettura viste Web per miglior supporto skin
- Miglioramento sistema gestione esseri con migliore gestione dello stato

### 2026-04-18

- `9f585e1` - Aggiornamento localizzazione, sistema timer, viste Web e aggiunta strumenti
  - Miglioramenti timer e pianificazione
  - Migliori viste Web con componenti UI migliorati
  - Più implementazioni strumenti
  - 57 file modificati, 3328 righe aggiunte, 389 righe eliminate

### 2026-04-17

- `9b71fcd` - Aggiornamento moduli core, aggiunta documentazione zh-HK, canale broadcast, strumento configurazione e vista Web audit
  - Canale broadcast per chat multipli esseri di silicio insieme
  - Sistema strumento configurazione
  - Vista Web audit
  - Documentazione cinese tradizionale
  - 42 file modificati, 3533 righe aggiunte, 268 righe eliminate

### 2026-04-16

- `5040f05` - Aggiornamento moduli core e default
  - Ottimizzazione moduli e correzioni bug
  - Aggiornamenti e miglioramenti implementazione
  - 58 file modificati, 9916 righe aggiunte, 111 righe eliminate

### 2026-04-15

- `3efab5f` - Aggiornamento moduli multipli: AI, Chat, IM, Strumenti, Web, Localizzazione, Archiviazione
  - Miglioramento client AI
  - Miglioramento sistema chat
  - Aggiornamento provider messenger
  - Ottimizzazione sistema strumenti
  - Miglioramento infrastruttura Web
  - Ottimizzazione localizzazione
  - Aggiornamento sistema archiviazione
  - 33 file modificati, 788 righe aggiunte, 232 righe eliminate

### 2026-04-14

- `4241a2f` - Funzionalità chat sostanzialmente completata, ottimizzazione caricamento UI
  - Completamento funzionalità sistema chat
  - Ottimizzazione UI caricamento file
  - 16 file modificati, 1234 righe aggiunte, 102 righe eliminate

### 2026-04-13

- `c498c31` - Aggiornamento codice
  - Miglioramenti e ottimizzazioni codice generale
  - 32 file modificati, 1045 righe aggiunte, 546 righe eliminate

### 2026-04-12

#### Documentazione e Localizzazione
- `2161002` - Ristrutturazione documentazione e miglioramento localizzazione
  - 17 file modificati, 982 righe aggiunte, 92 righe eliminate
- `03d94e4` - Miglioramento sistema configurazione e localizzazione
  - 25 file modificati, 1378 righe aggiunte, 154 righe eliminate
- `9976a35` - Aggiunta pagina informazioni e localizzazione
  - 14 file modificati, 699 righe aggiunte, 44 righe eliminate

#### Chat e Viste Web
- `0c8ccfc` - Miglioramento sistema chat, localizzazione e viste Web
  - 13 file modificati, 402 righe aggiunte, 56 righe eliminate
- `a8f1342` - Ridisegno livello comunicazione Web, passaggio da WebSocket a SSE
  - 27 file modificati, 793 righe aggiunte, 935 righe eliminate

### 2026-04-11

#### Sistema Log
- `e8fe259` - Aggiunta sistema log e ottimizzazione codice
  - 37 file modificati, 624 righe aggiunte, 91 righe eliminate
- `f01c519` - Aggiunta sistema log, aggiornamento interfacce AI e viste Web
  - 31 file modificati, 1758 righe aggiunte, 63 righe eliminate

### 2026-04-10

- `4962924` - Miglioramento gestore WebSocket, vista chat e interazione messenger
  - Miglioramento gestore contesto
  - Miglioramento sistema chat
  - Aggiornamento interfaccia provider messenger
  - Ridisegno provider WebUI
  - Aggiornamento builder JavaScript e router
  - Ottimizzazione vista chat
  - Miglioramento gestore WebSocket
  - 9 file modificati, 365 righe aggiunte, 134 righe eliminate

### 2026-04-09

- `f9302bf` - Miglioramento interfaccia provider messenger, sistema chat e interazione Web UI
  - Estensione interfaccia provider messenger
  - Miglioramento messaggi chat e sistema
  - Ottimizzazione gestore contesto
  - Miglioramento essere di silicio predefinito
  - Miglioramento vista chat Web UI
  - Aggiornamento gestore WebSocket
  - 10 file modificati, 427 righe aggiunte, 93 righe eliminate

### 2026-04-07

- `6831ee8` - Ridisegno viste Web e builder JavaScript
  - Ridisegno completo controller Web
  - Riscrittura completa builder JavaScript
  - Aggiornamento tutti i componenti vista
  - Miglioramento sistema skin
  - Miglioramento architettura classe base vista
  - 23 file modificati, 2004 righe aggiunte, 1983 righe eliminate

### 2026-04-05

- `41e97fb` - Aggiornamento moduli core multipli e controller Web
  - Miglioramento gestore contesto
  - Sistema chat e gestione sessioni
  - Ridisegno localizzatore servizi
  - Aggiornamento classe base e gestore Esseri di Silicio
  - Aggiornamento completo controller Web (17 controller)
  - Miglioramento factory essere di silicio predefinito
  - 31 file modificati, 681 righe aggiunte, 326 righe eliminate
- `67988d4` - Miglioramento moduli Web UI, aggiunta vista esecutore, pulizia viste e moduli core
  - 61 file modificati, 3148 righe aggiunte, 3726 righe eliminate

### 2026-04-04

- `b58bb1c` - Aggiunta controller inizializzazione e ridisegno moduli Web
  - Controller inizializzazione
  - Ridisegno modulo configurazione
  - Aggiornamento modulo localizzazione
  - Miglioramento sistema skin
  - Miglioramento router
  - 29 file modificati, 1269 righe aggiunte, 289 righe eliminate
- `f03ac0b` - Aggiunta moduli Web UI, miglioramento funzionalità messenger
  - 60 file modificati, 8481 righe aggiunte, 165 righe eliminate

### 2026-04-03

- `192e57b` - Aggiornamento struttura progetto e componenti runtime core
  - 22 file modificati, 446 righe aggiunte, 179 righe eliminate
- `59faec8` - Aggiornamento implementazioni core e default
  - 25 file modificati, 3056 righe aggiunte, 18 righe eliminate
- `d488485` - Aggiunta funzionalità compilazione dinamica e modulo strumento Curatore
  - 19 file modificati, 1727 righe aggiunte, 11 righe eliminate
- `753d1d9` - Aggiunta modulo sicurezza, aggiornamento esecutori, provider messenger, localizzazione e strumenti
  - 29 file modificati, 2352 righe aggiunte, 93 righe eliminate
- `a378697` - Completamento fase 5 - sistema strumenti + esecutori
  - 41 file modificati, 2651 righe aggiunte, 363 righe eliminate

### 2026-04-02

- `e6ad94b` - Correzione fallimento caricamento cronologia chat durante eliminazione file configurazione nei test
  - 4 file modificati, 49 righe aggiunte, 45 righe eliminate
- `daa56f5` - Completamento fase 4: memoria persistente (sistema chat + canale messenger)
  - 29 file modificati, 2051 righe aggiunte, 538 righe eliminate

### 2026-04-01

- `bbe2dbb` - Correzione caricamento configurazione e instradamento messaggi servizio chat
  - 27 file modificati, 1633 righe aggiunte, 147 righe eliminate
- `2fa6305` - Implementazione fase 2: framework ciclo principale e sistema oggetti clock
  - 9 file modificati, 594 righe aggiunte, 41 righe eliminate
- `32b99a1` - Implementazione fase 1 - funzionalità chat di base
  - 19 file modificati, 1185 righe aggiunte
- `358e368` - Commit iniziale: documentazione progetto e licenza
  - 10 file modificati, 1873 righe aggiunte
