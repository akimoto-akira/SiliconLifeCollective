# Architettura

> **Versione: v0.1.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [Français](../fr-FR/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md) | **Italiano**

## Architettura a doppia versione

Questo progetto offre due versioni di implementazione, che condividono lo stesso design architetturale, ma differiscono nell'archiviazione e nell'ottimizzazione delle prestazioni:

### SiliconLife.Default (Versione standard)
- **Posizionamento**: Implementazione standard, principalmente per la verifica della fattibilità architetturale
- **Modalità di esecuzione**: Applicazione console
- **Metodo di archiviazione**: Archiviazione JSON pura su file system
- **Scenari applicabili**: Requisiti elevati di sicurezza dei dati, risorse memoria limitate, piccolo volume di dati
- **Descrizione del ruolo**: Implementazione di riferimento per la verifica architetturale, offre un'esecuzione semplice e affidabile, adatta al primo contatto, al debug di sviluppo o agli scenari con priorità alla sicurezza dei dati

### SiliconLife.Fast (Versione ad alte prestazioni)
- **Posizionamento**: Versione principale di produzione
- **Modalità di esecuzione**: Applicazione desktop (Windows/macOS barra di stato di sistema / Linux finestra di stato)
- **Metodo di archiviazione**: Archiviazione in memoria SpeedyPack + persistenza batch asincrona (formato file .spk)
- **Scenari applicabili**: Alta concorrenza, bassa latenza, grandi volumi di dati
- **Supporto piattaforma**: Windows (funzionalità complete, inclusa barra di stato), Linux (finestra di stato, nessuna icona nella barra)
- **Caratteristiche**:
  - Windows/macOS esecuzione in background nella barra di stato, monitoraggio in tempo reale tramite la finestra di stato; Linux finestra di stato visualizzata direttamente
  - Motore SpeedyPack + compressione automatica che garantisce la sicurezza dei dati
  - Architettura Component UI, 30+ componenti dichiarativi
  - 7 temi d'aspetto, supporta rilevamento e commutazione automatici
  - Strumento di hot reload per aggiornamenti e riavvii online
  - Linux apre automaticamente il browser per l'accesso alla Web UI, supporta il parametro `--no-tray`
- **Miglioramento delle prestazioni**: Latenza di lettura ridotta di 1000x, latenza di scrittura ridotta di 15000x
- **Descrizione del ruolo**: Implementazione pronta per la produzione con ottimizzazione approfondita, con esecuzione in background nella barra di stato, motore SpeedyPack + compressione automatica, la scelta migliore per lo sfruttamento a lungo termine e i veri ambienti di produzione

> **Nota**: L'architettura descritta in questo documento si applica a entrambe le versioni, solo le implementazioni di archiviazione differiscono. SiliconLife.Default funge da riferimento per la verifica architetturale, SiliconLife.Fast è la versione principale raccomandata per la produzione.

---

## Concetti fondamentali

### Silicon Being

Ogni agente IA del sistema è un **Silicon Being** — un'entità autonoma con la propria identità, personalità e capacità. Ogni Silicon Being è guidato da un **file dell'anima** (prompt Markdown) che definisce i suoi modelli di comportamento.

### Silicon Curator

Il **Silicon Curator** è un Silicon Being speciale con i permessi di sistema più elevati. Agisce come amministratore di sistema:

- Creazione e gestione degli altri Silicon Beings
- Analisi delle richieste utente e scomposizione in attività
- Distribuzione delle attività ai Silicon Beings appropriati
- Monitoraggio della qualità dell'esecuzione e gestione degli errori
- Risposta ai messaggi utente con **pianificazione prioritaria** (vedi sotto)

### File dell'anima

File Markdown archiviato nella directory dati di ogni Silicon Being (`soul.md`). Viene iniettato come prompt di sistema in ogni richiesta IA, definendo la personalità, i modelli decisionali e i vincoli comportamentali del Being.

---

## Scheduling: Pianificazione equa per time-slice

### Loop principale + Oggetti orologio

Il sistema esegue un **loop principale guidato da orologio** su un thread dedicato:

```
Loop principale (thread dedicato, watchdog + circuit breaker)
  └── Oggetto orologio A (Priorità=0, Intervallo=100ms)
  └── Oggetto orologio B (Priorità=1, Intervallo=500ms)
  └── SiliconBeingManager (attivato da orologio del loop principale)
        └── SiliconBeingRunner → Silicon Being 1 → Attivazione orologio → Esecuzione di un ciclo
        └── SiliconBeingRunner → Silicon Being 2 → Attivazione orologio → Esecuzione di un ciclo
        └── SiliconBeingRunner → Silicon Being 3 → Attivazione orologio → Esecuzione di un ciclo
        └── ...
```

Decisioni di progettazione chiave:

- **I Silicon Beings non ereditano l'oggetto orologio.** Hanno il proprio metodo `Tick()`, chiamato da `SiliconBeingManager` tramite `SiliconBeingRunner`, e non registrati direttamente nel loop principale.
- **SiliconBeingManager** è attivato direttamente dall'orologio del loop principale e agisce come proxy unico per tutti i Beings.
- **SiliconBeingRunner** incapsula il `Tick()` di ogni Being su un thread temporaneo, con timeout e circuit breaker per Being (3 timeout consecutivi → 1 minuto di raffreddamento).
- L'esecuzione di ogni Being è limitata a **un ciclo** di richiesta IA + chiamata strumento per attivazione orologio, garantendo che nessun Being possa monopolizzare il loop principale.
- **Performance monitor** tiene traccia dei tempi di esecuzione dell'orologio per l'osservabilità.

### Risposta prioritaria del Curator

Quando un utente invia un messaggio al Silicon Curator:

1. Il Being corrente (ad esempio Being A) termina il suo ciclo in corso — **nessuna interruzione**.
2. Il gestore **salta il resto della coda**.
3. Il loop **riprende dal Curator**, permettendogli di eseguirsi immediatamente.

Questo garantisce la reattività alle interazioni utente senza interrompere le attività in corso.

---

## Architettura dei componenti

```
┌─────────────────────────────────────────────────────────┐
│                        Host principale                   │
│  (Host unificato — assemblea e gestisce tutti i componenti)│
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Loop     │  │ Localizzatore│  │   Configurazione  │  │
│  │ principale│  │ di servizi  │  │                   │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │     SiliconBeingManager (oggetto orologio)        │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curator   │ │Being A  │ │Being B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Servizi condivisi                    │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Sistema  │  │ Storage  │  │ Gestore          │  │   │
│  │  │ chat     │  │          │  │ permessi         │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ Client   │  │Esecutore │  │ Gestore          │  │   │
│  │  │ IA       │  │          │  │ strumenti        │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │ Caricatore│ │ Rete    │                        │   │
│  │  │ plugin    │ │ conn.   │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Esecutori                       │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Esecutore│  │ Esecutore│  │  Esecutore      │  │   │
│  │  │ disco    │  │ rete     │  │  riga comando   │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │          Provider messaggistica istantanea        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Provider │  │ Provider │  │  Feishu / ...    │  │   │
│  │  │ console  │  │ Web      │  │  Provider        │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Localizzatore di servizi

`ServiceLocator` è un registro singleton thread-safe che fornisce l'accesso a tutti i servizi principali:

| Proprietà | Tipo | Descrizione |
|-----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Gestore centrale delle sessioni chat |
| `IMManager` | `IMManager` | Router dei provider di messaggistica istantanea |
| `AuditLogger` | `AuditLogger` | Traccia di audit dei permessi |
| `GlobalAcl` | `GlobalACL` | Lista di controllo accessi globale |
| `BeingFactory` | `ISiliconBeingFactory` | Factory per creazione Beings |
| `BeingManager` | `SiliconBeingManager` | Gestore ciclo di vita dei Beings attivi |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Caricatore compilazione dinamica |
| `TokenUsageAudit` | `ITokenUsageAudit` | Monitoraggio utilizzo token |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Report utilizzo token |

Mantiene anche un registro di `PermissionManager` per Being, indicizzato dal GUID del Being.

---

## Sistema di chat

### Tipi di sessioni

Il sistema chat supporta tre tipi di sessioni tramite `SessionBase`:

| Tipo | Classe | Descrizione |
|------|--------|-------------|
| `SingleChat` | `SingleChatSession` | Conversazione uno-a-uno tra due partecipanti |
| `GroupChat` | `GroupChatSession` | Chat di gruppo multi-partecipante |
| `Broadcast` | `BroadcastChannel` | Canale aperto con ID fisso; i Beings si abbonano dinamicamente e ricevono solo i messaggi dopo l'abbonamento |

### Canali broadcast

`BroadcastChannel` è un tipo di sessione speciale per annunci a livello di sistema:

- **ID canale fisso** — A differenza di `SingleChatSession` e `GroupChatSession`, l'ID del canale è una costante nota, non derivata dai GUID dei membri.
- **Abbonamento dinamico** — I Beings si abbonano/disabbonano in fase di esecuzione; ricevono solo i messaggi pubblicati dopo il loro abbonamento.
- **Filtraggio messaggi in attesa** — `GetPendingMessages()` restituisce solo i messaggi pubblicati dopo l'ora di abbonamento del Being e non ancora letti.
- **Gestito dal sistema chat** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Messaggi chat

Il modello `ChatMessage` contiene campi per il contesto di conversazione IA e il monitoraggio dei token:

| Campo | Tipo | Descrizione |
|-------|------|-------------|
| `Id` | `Guid` | Identificatore univoco del messaggio |
| `SenderId` | `Guid` | Identificatore univoco del mittente |
| `ChannelId` | `Guid` | Identificatore del canale/conversazione |
| `Content` | `string` | Contenuto del messaggio |
| `Timestamp` | `DateTime` | Ora di invio del messaggio |
| `Type` | `MessageType` | Testo, immagine, file o notifica di sistema |
| `ReadBy` | `List<Guid>` | ID dei partecipanti che hanno letto questo messaggio |
| `Role` | `MessageRole` | Ruolo conversazione IA (utente, assistente, strumento) |
| `ToolCallId` | `string?` | ID chiamata strumento per messaggi risultato strumento |
| `ToolCallsJson` | `string?` | JSON serializzato delle chiamate strumento per messaggi assistente |
| `Thinking` | `string?` | Ragionamento catena di pensiero dell'IA |
| `PromptTokens` | `int?` | Numero token nel prompt (input) |
| `CompletionTokens` | `int?` | Numero token nella completazione (output) |
| `TotalTokens` | `int?` | Numero totale token utilizzati (input + output) |
| `FileMetadata` | `FileMetadata?` | Metadati file allegato (se il messaggio contiene un file) |

### Coda messaggi chat

`ChatMessageQueue` è un sistema di coda messaggi thread-safe per la gestione asincrona dei messaggi chat:

- **Thread-safe** — Utilizza meccanismi di blocco per garantire la sicurezza dell'accesso concorrente
- **Elaborazione asincrona** — Supporta l'accodamento e la rimozione asincrona dei messaggi
- **Ordinamento messaggi** — Mantiene l'ordine cronologico dei messaggi
- **Operazioni batch** — Supporta il recupero batch dei messaggi

### Metadati file

`FileMetadata` gestisce le informazioni sui file allegati ai messaggi chat:

- **Informazioni file** — Nome, dimensione, tipo, percorso
- **Timestamp caricamento** — Timestamp del caricamento del file
- **Caricatore** — ID dell'utente o del Silicon Being che ha caricato il file

### Gestore annullamento stream

`StreamCancellationManager` fornisce un meccanismo di annullamento per le risposte IA in streaming:

- **Controllo flusso** — Supporta l'annullamento delle risposte IA in streaming in corso
- **Pulizia risorse** — Pulizia corretta delle risorse associate durante l'annullamento
- **Sicurezza concorrente** — Supporta la gestione simultanea di più stream

### Cronologia chat

La funzionalità cronologia chat consente agli utenti di sfogliare le conversazioni passate dei Silicon Beings:

- **Elenco sessioni** — Mostra tutte le sessioni storiche
- **Dettagli messaggi** — Mostra la cronologia completa dei messaggi
- **Vista cronologica** — Presenta i messaggi in ordine cronologico
- **Supporto API** — Fornisce un'API RESTful per recuperare i dati di sessioni e messaggi

---

## Sistema di client IA

Il sistema supporta molteplici backend IA tramite l'interfaccia `IAIClient`:

### OllamaClient

- **Tipo**: Servizio IA locale
- **Protocollo**: API HTTP Ollama nativa (`/api/chat`, `/api/generate`)
- **Funzionalità**: Streaming, chiamate strumento, hosting modelli locali
- **Configurazione**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud DashScope)

- **Tipo**: Servizio IA cloud
- **Protocollo**: API compatibile OpenAI (`/compatible-mode/v1/chat/completions`)
- **Autenticazione**: Bearer token (chiave API)
- **Funzionalità**: Streaming, chiamate strumento, contenuto ragionamento (catena pensiero), distribuzione multi-regione
- **Regioni supportate**:
  - `beijing` — Cina Nord 2 (Pechino)
  - `virginia` — Stati Uniti (Virginia)
  - `singapore` — Singapore
  - `hongkong` — Hong Kong, Cina
  - `frankfurt` — Germania (Francoforte)
- **Modelli supportati** (scoperta dinamica tramite API, con elenco di fallback):
  - **Serie Qwen**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Ragionamento**: qwq-plus
  - **Terze parti**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Configurazione**: `apiKey`, `region`, `model`
- **Scoperta modelli**: Recupero modelli disponibili dall'API DashScope in fase di esecuzione; fallback a un elenco selezionato in caso di guasto di rete

### VolcengineArkClient (Volcengine Ark)

- **Tipo**: Servizio IA cloud
- **Protocollo**: API compatibile OpenAI
- **Autenticazione**: Bearer token (chiave API)
- **Funzionalità**: Supporto modalità streaming e non-streaming, controllo velocità doppio integrato
  - Controllo velocità proprio: Applicazione intervallo minimo tra le richieste
  - Limitazione velocità server: Gestione errori 429, nuovo tentativo con backoff esponenziale
- **Configurazione**: `apiKey`, `endpoint`, `model`
- **Caratteristiche**: Servizio IA di ByteDance, supporta vari modelli Doubao

### Pattern Factory dei client IA

Ogni tipo di client IA ha una corrispondente implementazione factory di `IAIClientFactory`:

- `OllamaClientFactory` — Crea istanze OllamaClient
- `DashScopeClientFactory` — Crea istanze DashScopeClient
- `VolcengineArkClientFactory` — Crea istanze VolcengineArkClient

La factory fornisce:
- `CreateClient(Dictionary<string, object> config)` — Istanzia un client dalla configurazione
- `GetConfigKeyOptions(string key, ...)` — Restituisce opzioni dinamiche per una chiave di configurazione (es. modelli disponibili, regioni)
- `GetDisplayName()` — Nome visualizzato localizzato del tipo di client

### Elenco piattaforme IA supportate

#### Legenda stati
- ✅ Implementato
- 🚧 In sviluppo
- 📋 Pianificato
- 💡 In considerazione

*Nota: A causa dell'ambiente di rete dello sviluppatore, la connessione ai servizi IA cloud internazionali contrassegnati [In considerazione] potrebbe richiedere strumenti proxy di rete e il processo di debug potrebbe essere instabile.*

#### Elenco piattaforme

| Piattaforma | Stato | Tipo | Descrizione |
|------------|--------|------|-------------|
| Ollama | ✅ | Locale | Servizio IA locale, supporta distribuzione modelli locali |
| DashScope (Alibaba Cloud) | ✅ | Cloud | Servizio IA DashScope di Alibaba Cloud, distribuzione multi-regione |
| Baidu Qianfan (Wenxin) | 📋 | Cloud | Servizio IA Wenxin di Baidu |
| Zhipu AI (GLM) | 📋 | Cloud | Servizio IA Zhipu Qingyan |
| Moonshot (Kimi) | 📋 | Cloud | Servizio IA Kimi di Moonshot AI |
| Volcengine Ark.Doubao | ✅ | Cloud | Servizio IA Doubao di ByteDance |
| DeepSeek (connessione diretta) | 📋 | Cloud | Servizio IA DeepSeek |
| 01.AI (Yi) | 📋 | Cloud | Servizio IA 01.AI |
| Tencent Hunyuan | 📋 | Cloud | Servizio IA Tencent Hunyuan |
| SiliconFlow | 📋 | Cloud | Servizio IA SiliconFlow |
| MiniMax | 📋 | Cloud | Servizio IA MiniMax |
| OpenAI | 💡 | Cloud | Servizio API OpenAI (serie GPT) |
| Anthropic | 💡 | Cloud | Servizio IA Anthropic Claude |
| Google DeepMind | 💡 | Cloud | Servizio Google Gemini |
| Mistral AI | 💡 | Cloud | Servizio IA Mistral |
| Groq | 💡 | Cloud | Servizio inferenza IA alta velocità Groq |
| Together AI | 💡 | Cloud | Servizio modelli open source Together AI |
| xAI | 💡 | Cloud | Servizio xAI Grok |
| Cohere | 💡 | Cloud | Servizio NLP aziendale Cohere |
| Replicate | 💡 | Cloud | Piattaforma hosting modelli open source Replicate |
| Hugging Face | 💡 | Cloud | Comunità IA open source e piattaforma modelli Hugging Face |
| Cerebras | 💡 | Cloud | Servizio inferenza IA ottimizzato Cerebras |
| Databricks | 💡 | Cloud | Piattaforma IA aziendale Databricks (MosaicML) |
| Perplexity AI | 💡 | Cloud | Servizio ricerca-risposta IA Perplexity |
| NVIDIA NIM | 💡 | Cloud | Microservizio inferenza IA NVIDIA |

---

## Decisioni di progettazione chiave

### Storage come classe di istanza (non statico)

`IStorage` è progettato come un'istanza injettabile, non un'utility statica. Questo garantisce:

- Accesso diretto al file system — IStorage è il canale di persistenza interna del sistema, **non** instradato tramite gli esecutori.
- **L'IA non può controllare IStorage** — Gli esecutori gestiscono gli I/O avviati dagli strumenti IA; IStorage gestisce le letture/scritture interne del framework. Sono preoccupazioni fondamentalmente diverse.
- Testabile con implementazioni simulate.
- Supporto futuro per diversi backend di storage senza modificare i consumatori.

### Esecutori come confine di sicurezza

Gli esecutori sono l'**unico** percorso per le operazioni di I/O. Gli strumenti che richiedono accesso a disco, rete o riga di comando **devono** passare attraverso gli esecutori. Questa progettazione impone:

- Ogni esecutore ha un **thread di scheduling separato**, con blocco del thread per la validazione dei permessi.
- Verifica centralizzata dei permessi — gli esecutori interrogano il **gestore permessi privato** del Being.
- Coda di richieste con supporto priorità e controllo timeout.
- Registrazione audit per tutte le operazioni esterne.
- Isolamento eccezioni — il fallimento di un esecutore non influisce sugli altri.
- Circuit breaker — i fallimenti consecutivi fermano temporaneamente l'esecutore per prevenire guasti a cascata.

### ContextManager come oggetto leggero

Ogni `ExecuteOneRound()` crea una nuova istanza di `ContextManager`:

1. Carica il file dell'anima + la cronologia chat recente.
2. Invia la richiesta al client IA.
3. Elabora in loop le chiamate strumento fino a quando l'IA restituisce testo puro.
4. Persiste la risposta nel sistema chat.
5. Libera le risorse.

Questo mantiene ogni ciclo isolato e senza stato.

### Auto-evoluzione tramite riscrittura classe

I Silicon Beings possono riscrivere le proprie classi C# in fase di esecuzione:

1. L'IA genera un nuovo codice di classe (deve ereditare da `SiliconBeingBase`).
2. **Controllo riferimenti in compilazione** (difesa principale): il compilatore riceve solo l'elenco di assembly autorizzati — `System.IO`, `System.Reflection`, ecc. sono esclusi, rendendo il codice pericoloso impossibile a livello di tipo.
3. **Analisi statica in esecuzione** (difesa secondaria): `SecurityScanner` scansiona il codice per pattern pericolosi dopo la compilazione riuscita.
4. Roslyn compila il codice in memoria.
5. In caso di successo: `SiliconBeingManager.ReplaceBeing()` scambia l'istanza corrente, migra lo stato e persiste il codice crittografato su disco.
6. In caso di fallimento: il nuovo codice viene rifiutato, l'implementazione esistente viene conservata.

Implementazioni personalizzate di `IPermissionCallback` possono anche essere compilate e iniettate tramite `ReplacePermissionCallback()`, consentendo ai Beings di personalizzare la propria logica di permessi.

Il codice viene archiviato su disco crittografato in AES-256. La chiave di crittografia è derivata dal GUID del Being (maiuscolo) tramite PBKDF2.

---

## Audit utilizzo token

`TokenUsageAuditManager` tiene traccia del consumo di token IA di tutti i Beings:

- `TokenUsageRecord` — Registrazione per richiesta (ID Being, modello, token prompt, token completamento, timestamp)
- `TokenUsageSummary` — Statistiche aggregate
- `TokenUsageQuery` — Parametri di query per filtrare le registrazioni
- Persistenza tramite `ITimeStorage` per query di serie temporali
- Accessibile tramite interfaccia Web (UsageController) e `TokenAuditTool` (solo Curator)

---

### Sistema di calendario

Il sistema comprende **32 implementazioni di calendari**, derivate dalla classe astratta `CalendarBase`, che coprono i principali sistemi calendariali del mondo:

| Calendario | ID | Descrizione |
|------------|-----|-------------|
| BuddhistCalendar | `buddhist` | Calendario buddista (BE), anno + 543 |
| CherokeeCalendar | `cherokee` | Sistema calendario cherokee |
| ChineseLunarCalendar | `lunar` | Calendario lunare cinese, con mesi intercalari |
| ChineseHistoricalCalendar | `chinese_historical` | Calendario storico cinese, ciclo Ganzhi ed ere imperiali |
| ChulaSakaratCalendar | `chula_sakarat` | Calendario Chula Sakarat (CS), anno - 638 |
| CopticCalendar | `coptic` | Calendario copto |
| DaiCalendar | `dai` | Calendario Dai con calcolo lunare completo |
| DehongDaiCalendar | `dehong_dai` | Variante Dai Dehong |
| EthiopianCalendar | `ethiopian` | Calendario etiope |
| FrenchRepublicanCalendar | `french_republican` | Calendario repubblicano francese |
| GregorianCalendar | `gregorian` | Calendario gregoriano standard |
| HebrewCalendar | `hebrew` | Calendario ebraico (giudaico) |
| IndianCalendar | `indian` | Calendario nazionale indiano |
| InuitCalendar | `inuit` | Sistema calendario inuit |
| IslamicCalendar | `islamic` | Calendario islamico (Egira) |
| JapaneseCalendar | `japanese` | Calendario delle ere giapponesi (Nengo) |
| JavaneseCalendar | `javanese` | Calendario islamico giavanese |
| JucheCalendar | `juche` | Calendario Juche (Corea del Nord), anno - 1911 |
| JulianCalendar | `julian` | Calendario giuliano |
| KhmerCalendar | `khmer` | Calendario khmer |
| MayanCalendar | `mayan` | Calendario lungo maya |
| MongolianCalendar | `mongolian` | Calendario mongolo |
| PersianCalendar | `persian` | Calendario persiano (Egira solare) |
| RepublicOfChinaCalendar | `roc` | Calendario della Repubblica di Cina (Minguo), anno - 1911 |
| RomanCalendar | `roman` | Calendario romano |
| SakaCalendar | `saka` | Calendario Saka (Indonesia) |
| SexagenaryCalendar | `sexagenary` | Calendario Ganzhi cinese (sessagesimale) |
| TibetanCalendar | `tibetan` | Calendario tibetano |
| VietnameseCalendar | `vietnamese` | Calendario lunare vietnamita (variante zodiaco del Gatto) |
| VikramSamvatCalendar | `vikram_samvat` | Calendario Vikram Samvat |
| YiCalendar | `yi` | Sistema calendario Yi |
| ZoroastrianCalendar | `zoroastrian` | Calendario zoroastriano |

`CalendarTool` fornisce le operazioni: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (conversione date tra calendari).

---

## Architettura dell'interfaccia Web

### Sistema di skin

L'interfaccia Web dispone di un **sistema di skin inseribili**, che consente la personalizzazione completa dell'interfaccia senza modificare la logica applicativa:

- **Interfaccia ISkin** — Definisce il contratto per tutti gli skin, includendo:
  - Metodi di rendering principali (`RenderHtml`, `RenderError`)
  - 20+ metodi componenti UI (pulsanti, input, schede, tabelle, badge, bolle, progressione, schede, ecc.)
  - Generazione CSS tematico tramite `CssBuilder`
  - `SkinPreviewInfo` — Palette colori e icona per il selettore skin della pagina iniziale

- **Skin integrati** — 7 skin pronti per la produzione:
  - **Admin** — Interfaccia amministrazione professionale, orientata ai dati
  - **Chat** — Design conversazionale, centrato sui messaggi, per interazione IA
  - **Creative** — Layout artistico e visivamente ricco per flussi di lavoro creativi
  - **Dev** — Interfaccia centrata sullo sviluppatore e sul codice, con colorazione sintassi
  - **HighContrast** — Tema accessibilità ad alto contrasto
  - **Light** — Tema chiaro e pulito
  - **Minimal** — Tema minimalista

- **Scoperta skin** — `SkinManager` scopre e registra automaticamente tutte le implementazioni `ISkin` tramite riflessione

### Costruttori HTML / CSS / JS

L'interfaccia Web evita interamente i file di template, generando tutto il markup in C#:

- **`H`** — DSL costruttore HTML fluente, per costruire alberi HTML nel codice
- **`CssBuilder`** — Costruttore CSS con supporto selettori e media query
- **`JsBuilder` (`JsSyntax`)** — Costruttore JavaScript per script in linea

### Sistema di controller

L'interfaccia Web segue un **pattern di tipo MVC**, con 22 controller che gestiscono diversi aspetti:

| Controller | Scopo |
|------------|--------|
| About | Pagina Informazioni e informazioni progetto |
| Being | Gestione e stato dei Silicon Beings |
| Chat | Interfaccia chat in tempo reale con SSE |
| ChatHistory | Cronologia chat, elenco sessioni e dettagli messaggi |
| CodeBrowser | Consultazione e modifica codice |
| CodeHover | Suggerimenti codice con colorazione sintassi |
| Config | Gestione configurazione sistema |
| Dashboard | Panoramica sistema e metriche |
| Executor | Stato e gestione esecutori |
| Help | Sistema documentazione aiuto, supporto multilingue |
| Init | Assistente inizializzazione prima esecuzione |
| Knowledge | Visualizzazione e query grafo conoscenza |
| Log | Visualizzatore log sistema, filtraggio per Being |
| Memory | Navigatore memoria lungo termine, filtraggio avanzato, statistiche e vista dettaglio |
| Permission | Gestione permessi |
| PermissionRequest | Coda richieste permessi |
| Project | Gestione progetti, con note di lavoro e sistema attività |
| System | Monitoraggio prestazioni sistema e metriche |
| Task | Interfaccia sistema attività |
| Timer | Gestione sistema timer, con cronologia esecuzione |
| Usage | Dashboard utilizzo token, con grafici tendenza ed export |
| WorkNote | Gestione note di lavoro, ricerca e generazione sommario |

### Aggiornamenti in tempo reale

- **SSE (Server-Sent Events)** — Invia aggiornamenti messaggi chat, stato Beings ed eventi sistema tramite `SSEHandler`
- **Niente WebSocket** — Architettura più semplice che utilizza SSE per la maggior parte delle esigenze in tempo reale
- **Riconnessione automatica** — Logica di riconnessione lato client per connessioni resilienti

### Localizzazione

Il sistema supporta la localizzazione completa di **30 varianti linguistiche**:
- **Cinese (6)**: zh-CN (semplificato), zh-HK (tradizionale), zh-SG (Singapore), zh-MO (Macao), zh-TW (Taiwan), zh-MY (Malesia)
- **Inglese (10)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Tedesco (5)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **Francese (3)**: fr-FR, fr-CA, fr-CH
- **Italiano (1)**: it-IT
- **Altri (3)**: ja-JP (giapponese), ko-KR (coreano), cs-CZ (ceco)

---

## Stati di attività dei Silicon Beings

I Silicon Beings hanno i seguenti stati di attività:

| Stato | Descrizione |
|------|------|
| `Idle` | Stato inattivo, in attesa del trigger dell'orologio |
| `Working` | In esecuzione di un ciclo richiesta IA + chiamata strumento |
| `Error` | Si è verificato un errore durante l'esecuzione |
| `Stopped` | Arrestato, a causa di errori consecutivi o arresto manuale |

**Meccanismo stato Stopped**:
- Quando un Silicon Being subisce 10 errori consecutivi, entra automaticamente nello stato `Stopped`
- Una volta nello stato Stopped, il Being non eseguirà più alcuna attività
- È richiesto un intervento manuale per riavviare

Transizioni di stato:
```
Idle → Working → Idle (terminazione normale)
Working → Error → Working (recupero errore)
Working → Stopped (10 errori consecutivi o arresto manuale)
Stopped → Idle (riavvio)
```
