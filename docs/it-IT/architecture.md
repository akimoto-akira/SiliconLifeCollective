# Architettura

> **Versione: v0.2.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md) | [Русский](../ru-RU/architecture.md) | **Italiano**

## Architettura a Doppia Versione

Questo progetto fornisce due versioni di implementazione che condividono la stessa architettura, ma differiscono nell'archiviazione e nell'ottimizzazione delle prestazioni:

### SiliconLife.Default (Versione Predefinita)
- **Posizionamento**: Implementazione predefinita, utilizzata principalmente per verificare la fattibilità dell'architettura
- **Modalità di esecuzione**: Applicazione console
- **Archiviazione**: Archiviazione JSON su file system
- **Scenari di utilizzo**: Scenari con elevati requisiti di sicurezza dei dati, risorse di memoria limitate, piccolo volume di dati
- **Descrizione del ruolo**: Come implementazione di riferimento per la validazione dell'architettura, offre un modo semplice e affidabile per l'esecuzione, adatto per chi si avvicina al progetto per la prima volta, per lo sviluppo e il debug o per scenari in cui la sicurezza dei dati è prioritaria

### SiliconLife.Fast (Versione ad Alte Prestazioni)
- **Posizionamento**: Versione di produzione raccomandata
- **Modalità di esecuzione**: Applicazione desktop (area di notifica di Windows / finestra di stato su Linux)
- **Archiviazione**: Archiviazione in memoria SpeedyPack + persistenza batch asincrona (formato file .spk)
- **Scenari di utilizzo**: Scenari ad alta concorrenza, bassa latenza, grande volume di dati
- **Supporto piattaforma**: Windows/macOS (funzionalità complete, inclusa area di notifica), Linux (finestra di stato, senza icona nell'area di notifica)
- **Caratteristiche**:
  - Esecuzione in background nell'area di notifica di Windows/macOS, finestra di stato dell'area di notifica per monitoraggio in tempo reale; finestra di stato diretta su Linux
  - Motore SpeedyPack + compressione automatica per garantire la sicurezza dei dati
  - Architettura Component UI, 27 componenti dichiarativi
  - 7 temi skin, supporto per scoperta automatica e cambio
  - Supporto al hot-reload degli strumenti per aggiornamenti e riavvio online → Linux apre automaticamente il browser per accedere alla Web UI, supporta il parametro `--no-tray`
  - Su Linux apre automaticamente il browser per accedere alla Web UI, supporta il parametro `--no-tray`
- **Miglioramento delle prestazioni**: Latenza di lettura dell'archiviazione ridotta di 1000 volte, latenza di scrittura ridotta di 15000 volte
- **Descrizione del ruolo**: Implementazione di livello produttivo profondamente ottimizzata, con funzionalità come l'esecuzione in background nell'area di notifica, motore SpeedyPack + compressione automatica, è la scelta preferita per l'esecuzione a lungo termine e ambienti di produzione reali

> **Nota**: L'architettura descritta in questo documento si applica a entrambe le versioni, con differenze solo nella parte di implementazione dell'archiviazione. SiliconLife.Default funge da riferimento per la validazione dell'architettura, mentre SiliconLife.Fast è la versione di produzione raccomandata.

---

## Concetti Fondamentali

### Essere di Silicio

Ogni agente AI nel sistema è un **Essere di Silicio** — un'entità autonoma con la propria identità, personalità e capacità. Ogni Essere di Silicio è guidato da un **File dell'Anima** (prompt Markdown) che ne definisce i modelli di comportamento.

### Curatore di Silicio

Il **Curatore di Silicio** è un Essere di Silicio speciale con i più alti privilegi di sistema. Agisce come amministratore di sistema:

- Crea e gestisce altri Esseri di Silicio
- Analizza le richieste degli utenti e le scompone in attività
- Distribuisce le attività agli Esseri di Silicio appropriati
- Monitora la qualità dell'esecuzione e gestisce i fallimenti
- Risponde ai messaggi degli utenti utilizzando la **pianificazione prioritaria** (vedi sotto)

### File dell'Anima

Un file Markdown memorizzato nella directory dati di ogni Essere di Silicio (`soul.md`). Viene iniettato come prompt di sistema in ogni richiesta AI, definendo la personalità, i modelli decisionali e i vincoli comportamentali dell'essere.

---

## Pianificazione: Pianificazione Equa a Slot Temporali

### Ciclo Principale + Oggetti Tick

Il sistema esegue un **ciclo principale guidato da clock** su un thread dedicato in background:

```
Ciclo principale (thread dedicato, watchdog + interruttore)
  └── Oggetto Tick A (priorità=0, intervallo=100ms)
  └── Oggetto Tick B (priorità=1, intervallo=500ms)
  └── Gestore di Esseri di Silicio (attivato direttamente dal clock del ciclo principale)
        └── Runner Essere di Silicio → Essere di Silicio 1 → Tick → esecuzione di un ciclo
        └── Runner Essere di Silicio → Essere di Silicio 2 → Tick → esecuzione di un ciclo
        └── Runner Essere di Silicio → Essere di Silicio 3 → Tick → esecuzione di un ciclo
        └── ...
```

Decisioni di progettazione chiave:

- **Gli Esseri di Silicio non ereditano dagli oggetti Tick.** Hanno il proprio metodo `Tick()`, chiamato da `SiliconBeingManager` tramite `SiliconBeingRunner`, non registrato direttamente al ciclo principale.
- **Il Gestore di Esseri di Silicio** è attivato direttamente dal clock del ciclo principale e agisce come unico proxy per tutti gli esseri.
- **Il Runner dell'Essere di Silicio** avvolge il `Tick()` di ogni essere su un thread temporaneo, con timeout e interruttore per essere (3 timeout consecutivi → 1 minuto di raffreddamento).
- L'esecuzione di ogni essere è limitata a **un ciclo** di richiesta AI + chiamata strumento per attivazione Tick, garantendo che nessun essere possa monopolizzare il ciclo principale.
- **Il Monitor di Prestazioni** traccia i tempi di esecuzione del clock per garantire l'osservabilità.

### Risposta Prioritaria del Curatore

Quando un utente invia un messaggio al Curatore di Silicio:

1. L'essere corrente (ad es. Essere A) completa il suo ciclo attuale — **nessuna interruzione**.
2. Il gestore **salta la coda rimanente**.
3. Il ciclo **ricomincia dal Curatore**, facendolo eseguire immediatamente.

Questo garantisce la reattività alle interazioni degli utenti senza interferire con le attività in corso.

---

## Architettura dei Componenti

```
┌─────────────────────────────────────────────────────────┐
│                        Host Principale                   │
│  (Host unificato — assembla e gestisce tutti i componenti)│
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │Ciclo      │  │ Localizzatore│  │    Configurazione │  │
│  │Principale │  │  di Servizi  │  │                   │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │        Gestore di Esseri di Silicio (Oggetto Tick) │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curatore  │ │Essere A │ │Essere B │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Servizi Condivisi                     │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Sistema   │  │Archivio  │  │Gestore dei       │  │   │
│  │  │di Chat   │  │          │  │Permessi          │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │Client AI │  │Esecutore │  │Gestore degli     │  │   │
│  │  │          │  │          │  │Strumenti         │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Caricatore│  │Rete di   │                        │   │
│  │  │di Plugin │  │Conoscenza│                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Esecutori                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Esecutore │  │Esecutore │  │Esecutore da     │  │   │
│  │  │Disco     │  │di Rete   │  │Riga di Comando  │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │          Fornitori di Messaggistica Istantanea    │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Fornitore │  │Fornitore │  │Fornitore        │  │   │
│  │  │Console   │  │Web       │  │Feishu / ...     │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Localizzatore di Servizi

Il `ServiceLocator` è un registro singleton thread-safe che fornisce accesso a tutti i servizi principali:

| Proprietà | Tipo | Descrizione |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Gestore centrale delle sessioni di chat |
| `IMManager` | `IMManager` | Router dei fornitori di messaggistica istantanea |
| `AuditLogger` | `AuditLogger` | Traccia di audit delle autorizzazioni |
| `GlobalAcl` | `GlobalACL` | Lista di controllo degli accessi globale |
| `BeingFactory` | `ISiliconBeingFactory` | Factory per la creazione di esseri |
| `BeingManager` | `SiliconBeingManager` | Gestore del ciclo di vita degli esseri attivi |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Caricatore dinamico con compilazione |
| `TokenUsageAudit` | `ITokenUsageAudit` | Tracciamento dell'utilizzo dei token |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Report sull'utilizzo dei token |

Mantiene anche un registro di `PermissionManager` per ogni essere, indicizzato per GUID dell'essere.

---

## Sistema di Chat

### Tipi di Sessione

Il sistema di chat supporta tre tipi di sessione tramite `SessionBase`:

| Tipo | Classe | Descrizione |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Conversazione uno-a-uno tra due partecipanti |
| `GroupChat` | `GroupChatSession` | Chat di gruppo multi-partecipante |
| `Broadcast` | `BroadcastChannel` | Canale aperto con ID fisso; gli esseri si iscrivono dinamicamente e ricevono messaggi solo dopo l'iscrizione |

### Canale di Trasmissione

Il `BroadcastChannel` è un tipo di sessione speciale per annunci a livello di sistema:

- **ID canale fisso** — A differenza di `SingleChatSession` e `GroupChatSession`, l'ID del canale è una costante nota, non derivato dai GUID dei membri.
- **Iscrizione dinamica** — Gli esseri si iscrivono/disiscrivono a runtime; ricevono solo i messaggi pubblicati dopo l'iscrizione.
- **Filtraggio messaggi in sospeso** — `GetPendingMessages()` restituisce solo i messaggi pubblicati dopo il tempo di iscrizione dell'essere e non ancora letti.
- **Gestito dal sistema di chat** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Messaggio di Chat

Il modello `ChatMessage` contiene campi per il contesto di conversazione AI e il tracciamento dei token:

| Campo | Tipo | Descrizione |
|-------|------|-------------|
| `Id` | `Guid` | Identificatore univoco del messaggio |
| `SenderId` | `Guid` | Identificatore univoco del mittente |
| `ChannelId` | `Guid` | Identificatore del canale/conversazione |
| `Content` | `string` | Contenuto del messaggio |
| `Timestamp` | `DateTime` | Orario di invio del messaggio |
| `Type` | `MessageType` | Testo, immagine, file o notifica di sistema |
| `ReadBy` | `List<Guid>` | ID dei partecipanti che hanno letto questo messaggio |
| `Role` | `MessageRole` | Ruolo nella conversazione AI (utente, assistente, strumento) |
| `ToolCallId` | `string?` | ID della chiamata strumento per i messaggi di risultato |
| `ToolCallsJson` | `string?` | JSON serializzato delle chiamate strumento per i messaggi dell'assistente |
| `Thinking` | `string?` | Ragionamento della catena di pensiero dell'AI |
| `PromptTokens` | `int?` | Numero di token nel prompt (input) |
| `CompletionTokens` | `int?` | Numero di token nel completamento (output) |
| `TotalTokens` | `int?` | Numero totale di token utilizzati (input + output) |
| `FileMetadata` | `FileMetadata?` | Metadati del file allegato (se il messaggio contiene un file) |

### Coda dei Messaggi di Chat

La `ChatMessageQueue` è un sistema di coda dei messaggi thread-safe per la gestione dell'elaborazione asincrona dei messaggi di chat:

- **Thread-safe** - Utilizza meccanismi di blocco per garantire la sicurezza dell'accesso concorrente
- **Elaborazione asincrona** - Supporta l'accodamento e lo scodamento asincrono dei messaggi
- **Ordinamento dei messaggi** - Mantiene l'ordine temporale dei messaggi
- **Operazioni batch** - Supporta il recupero batch dei messaggi

### Metadati dei File

`FileMetadata` gestisce le informazioni sui file allegati ai messaggi di chat:

- **Informazioni sul file** - Nome, dimensione, tipo, percorso del file
- **Orario di caricamento** - Timestamp del caricamento del file
- **Caricatore** - ID dell'utente o dell'Essere di Silicio che ha caricato il file

### Gestore di Annullamento dello Stream

`StreamCancellationManager` fornisce un meccanismo di annullamento per le risposte streaming AI:

- **Controllo dello stream** - Supporta l'annullamento delle risposte streaming AI in corso
- **Pulizia delle risorse** - Pulizia corretta delle risorse correlate all'annullamento
- **Sicurezza della concorrenza** - Supporta la gestione simultanea di più stream

### Visualizzazione Cronologia Chat

La nuova funzionalità di visualizzazione della cronologia chat consente agli utenti di sfogliare le conversazioni storiche degli Esseri di Silicio:

- **Elenco delle sessioni** - Mostra tutte le sessioni storiche
- **Dettagli dei messaggi** - Visualizza la cronologia completa dei messaggi
- **Vista timeline** - Mostra i messaggi in ordine cronologico
- **Supporto API** - Fornisce API RESTful per ottenere dati di sessioni e messaggi

---

## Sistema Client AI

Il sistema supporta multipli backend AI tramite l'interfaccia `IAIClient`:

### OllamaClient

- **Tipo**: Servizio AI locale
- **Protocollo**: API HTTP nativa di Ollama (`/api/chat`, `/api/generate`)
- **Funzionalità**: Streaming, chiamate strumento, hosting di modelli locali
- **Configurazione**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Tipo**: Servizio AI cloud
- **Protocollo**: API compatibile OpenAI (`/compatible-mode/v1/chat/completions`)
- **Autenticazione**: Bearer token (chiave API)
- **Funzionalità**: Streaming, chiamate strumento, contenuto di ragionamento (catena di pensiero), distribuzione multi-regione
- **Regioni supportate**:
  - `beijing` — Cina Nord 2 (Pechino)
  - `virginia` — Stati Uniti (Virginia)
  - `singapore` — Singapore
  - `hongkong` — Hong Kong, Cina
  - `frankfurt` — Germania (Francoforte)
- **Modelli supportati** (scoperti dinamicamente tramite API, con elenco di fallback):
  - **Serie Qwen**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Ragionamento**: qwq-plus
  - **Terze parti**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Configurazione**: `apiKey`, `region`, `model`
- **Scoperta dei modelli**: Recupera i modelli disponibili dall'API Bailian a runtime; in caso di guasto di rete, utilizza un elenco curato

### VolcengineArkClient (Volcengine Ark)

- **Tipo**: Servizio AI cloud
- **Protocollo**: API compatibile OpenAI
- **Autenticazione**: Bearer token (chiave API)
- **Funzionalità**: Supporto modalità streaming e non streaming, controllo di velocità a due livelli integrato
  - Auto-controllo della velocità: impone un intervallo minimo tra le richieste
  - Limite di velocità del server: gestisce errori 429, retry con backoff esponenziale
- **Configurazione**: `apiKey`, `endpoint`, `model`
- **Caratteristiche**: Servizio AI di ByteDance, supporta vari modelli Doubao

### DeepSeekClient

- **Tipo**: Servizio AI cloud
- **Protocollo**: API compatibile OpenAI (`https://api.deepseek.com`)
- **Autenticazione**: Bearer token (chiave API)
- **Funzionalità**: Streaming, chiamate strumento, modalità thinking (reasoning_content), parametro reasoning_effort
- **Finestra di contesto**: 1.048.576 token
- **Configurazione**: `apiKey`, `model`

### ZhipuClient (GLM)

- **Tipo**: Servizio AI cloud
- **Protocollo**: API compatibile OpenAI (`https://open.bigmodel.cn/api/paas/v4`)
- **Autenticazione**: Bearer token (chiave API)
- **Funzionalità**: Streaming, chiamate strumento, modalità thinking, supporto visione per modello
- **Finestra di contesto**: 1.048.576 token
- **Configurazione**: `apiKey`, `model`

### ErnieClient (Baidu Qianfan/Wenxin Yiyan)

- **Tipo**: Servizio AI cloud
- **Protocollo**: API compatibile OpenAI (`https://qianfan.baidubce.com/v2`)
- **Autenticazione**: Bearer token (chiave API)
- **Funzionalità**: Streaming, chiamate strumento, supporto visione per modello
- **Finestra di contesto**: 131.072 token
- **Configurazione**: `apiKey`, `model`

### HunyuanClient (Tencent Hunyuan)

- **Tipo**: Servizio AI cloud
- **Protocollo**: API compatibile OpenAI (endpoint doppio: TokenHub raccomandato + Legacy `https://api.hunyuan.cloud.tencent.com/v1`)
- **Autenticazione**: Bearer token (chiave API)
- **Funzionalità**: Streaming, chiamate strumento per modello, nessun supporto visione
- **Finestra di contesto**: 262.144 token
- **Modelli supportati**: hy3 (raccomandato), hy3-preview
- **Configurazione**: `apiKey`, `model`

### MiniMaxClient

- **Tipo**: Servizio AI cloud
- **Protocollo**: API compatibile OpenAI (`https://api.minimaxi.com/v1`)
- **Autenticazione**: Bearer token (chiave API)
- **Funzionalità**: Streaming, chiamate strumento, supporto visione per modello
- **Finestra di contesto**: 1.048.576 token
- **Configurazione**: `apiKey`, `model`

### MoonshotClient (Kimi)

- **Tipo**: Servizio AI cloud
- **Protocollo**: API compatibile OpenAI (`https://api.moonshot.cn/v1`)
- **Autenticazione**: Bearer token (chiave API)
- **Funzionalità**: Streaming, chiamate strumento, supporto visione per modello
- **Finestra di contesto**: 262.144 token
- **Configurazione**: `apiKey`, `model`

### SiliconFlowClient

- **Tipo**: Servizio AI cloud (piattaforma di aggregazione)
- **Protocollo**: API compatibile OpenAI (`https://api.siliconflow.cn/v1`)
- **Autenticazione**: Bearer token (chiave API)
- **Funzionalità**: Streaming, chiamate strumento, supporto visione per modello, lista dinamica dei modelli disponibili (interfaccia /models)
- **Finestra di contesto**: 1.048.576 token
- **Configurazione**: `apiKey`, `model`

### Pattern Factory dei Client

Ogni tipo di client AI ha una corrispondente implementazione factory `IAIClientFactory`:

- `OllamaClientFactory` — Crea istanze di OllamaClient
- `DashScopeClientFactory` — Crea istanze di DashScopeClient
- `VolcengineArkClientFactory` — Crea istanze di VolcengineArkClient
- `HerdsmanClientFactory` — Crea istanze di HerdsmanClient
- `LongCatClientFactory` — Crea istanze di LongCatClient
- `QiniuAIClientFactory` — Crea istanze di QiniuAIClient
- `DeepSeekClientFactory` — Crea istanze di DeepSeekClient
- `ZhipuClientFactory` — Crea istanze di ZhipuClient
- `ErnieClientFactory` — Crea istanze di ErnieClient
- `HunyuanClientFactory` — Crea istanze di HunyuanClient
- `MiniMaxClientFactory` — Crea istanze di MiniMaxClient
- `MoonshotClientFactory` — Crea istanze di MoonshotClient
- `SiliconFlowClientFactory` — Crea istanze di SiliconFlowClient

La factory fornisce:
- `CreateClient(Dictionary<string, object> config)` — Istanzia un client dalla configurazione
- `GetConfigKeyOptions(string key, ...)` — Restituisce opzioni dinamiche per le chiavi di configurazione (es. modelli disponibili, regioni)
- `GetDisplayName()` — Nome visualizzato localizzato del tipo di client

### Interfaccia delle capacità IAIClient

L'interfaccia `IAIClient` definisce gli attributi di dichiarazione delle capacità del client AI, e il `ContextManager` adatta il comportamento di conseguenza:

| Proprietà | Tipo | Descrizione |
|-----------|------|-------------|
| `StreamingMode` | `bool?` | Supporto modalità streaming: true=solo streaming, false=solo non streaming, null=entrambi (predefinito: streaming) |
| `SupportsToolCalls` | `bool?` | Supporto chiamate strumenti: true=supportato, false=non supportato (ignora iniezione strumenti), null=sconosciuto (predefinito: supportato) |
| `ContextWindowTokens` | `int?` | Dimensione della finestra di contesto (numero di token), utilizzata per il taglio del budget di token al posto di MaxContextMessages fisso |
| `SupportsVision` | `bool?` | Supporto input visione: true=supporta immagini, false=non supportato, null=sconosciuto (predefinito: non supportato) |
| `SupportsAudio` | `bool?` | Supporto input audio: true=supporta audio, false=non supportato, null=sconosciuto (predefinito: non supportato) |

### Inventario del Supporto delle Piattaforme AI

#### Legenda degli Stati
- ✅ Implementato
- 🚧 In sviluppo
- 📋 Pianificato
- 💡 In considerazione
- ⚠️ Deprecato

*Nota: A causa dell'ambiente di rete dello sviluppatore, l'integrazione con servizi AI cloud esteri [in considerazione] potrebbe richiedere l'uso di strumenti proxy di rete per l'accesso, e il processo di debug potrebbe presentare instabilità.*

#### Elenco delle Piattaforme

| Piattaforma | Stato | Tipo | Descrizione |
|------|------|------|------|
| Ollama | ✅ | Locale | Servizio AI locale, supporta distribuzione di modelli locali |
| DashScope (Alibaba Cloud Bailian) | ✅ | Cloud | Servizio AI Alibaba Cloud Bailian, supporta distribuzione multi-regione |
| Baidu Qianfan (Wenxin Yiyan) | ✅ | Cloud | Servizio AI Baidu Wenxin Yiyan — ErnieClient |
| Zhipu AI (GLM) | ✅ | Cloud | Servizio AI Zhipu Qingyan — ZhipuClient |
| Moonshot (Kimi) | ✅ | Cloud | Servizio AI Moonshot Kimi — MoonshotClient |
| Volcengine Ark Doubao | ✅ | Cloud | Servizio AI ByteDance Doubao |
| Herdsman | ✅ | Locale/Cloud | Motore di inferenza senza autenticazione, compatibile con il formato API OpenAI |
| Meituan LongCat | ✅ | Cloud | Modello di grandi dimensioni sviluppato autonomamente da Meituan, compatibile con il formato API OpenAI, autenticazione con chiave API |
| Qiniu Cloud AI | ✅ | Cloud | Servizio di inferenza di modelli di grandi dimensioni di Qiniu Cloud, compatibile con il formato API OpenAI, autenticazione con chiave API |
| DeepSeek (Connessione diretta) | ✅ | Cloud | Servizio AI DeepSeek — DeepSeekClient, supporta modalità thinking |
| 01.AI | ⚠️ | Cloud | Servizio AI 01.AI (Deprecato: registrazione nuovi utenti interrotta) |
| Tencent Hunyuan | ✅ | Cloud | Servizio AI Tencent Hunyuan — HunyuanClient, endpoint doppio TokenHub/Legacy |
| SiliconFlow | ✅ | Cloud | Servizio AI SiliconFlow — SiliconFlowClient, lista dinamica modelli |
| MiniMax | ✅ | Cloud | Servizio AI MiniMax — MiniMaxClient |
| OpenAI | 💡 | Cloud | Servizio API OpenAI (serie GPT) |
| Anthropic | 💡 | Cloud | Servizio AI Anthropic Claude |
| Google DeepMind | 💡 | Cloud | Servizio AI Google Gemini |
| Mistral AI | 💡 | Cloud | Servizio AI Mistral AI |
| Groq | 💡 | Cloud | Servizio di inferenza AI ad alta velocità Groq |
| Together AI | 💡 | Cloud | Servizio di modelli open source Together AI |
| xAI | 💡 | Cloud | Servizio xAI Grok |
| Cohere | 💡 | Cloud | Servizio NLP enterprise Cohere |
| Replicate | 💡 | Cloud | Piattaforma di hosting modelli open source Replicate |
| Hugging Face | 💡 | Cloud | Community AI open source e piattaforma di modelli Hugging Face |
| Cerebras | 💡 | Cloud | Servizio di inferenza AI ottimizzato Cerebras |
| Databricks | 💡 | Cloud | Piattaforma AI enterprise Databricks (MosaicML) |
| Perplexity AI | 💡 | Cloud | Servizio di ricerca e risposta AI Perplexity AI |
| NVIDIA NIM | 💡 | Cloud | Microservizio di inferenza AI NVIDIA |

---

## Decisioni di Progettazione Chiave

### Archiviazione come Classe di Istanza (non statica)

`IStorage` è progettata come un'istanza iniettabile, non come un'utilità statica. Questo garantisce:

- Accesso diretto al file system — IStorage è il canale di persistenza interno del sistema, **non** instradato attraverso l'esecutore.
- **L'AI non può controllare IStorage** — L'esecutore gestisce l'I/O avviato dagli strumenti AI; IStorage gestisce la lettura/scrittura dei dati interni del framework. Questi sono aspetti fondamentalmente diversi.
- Testabilità con implementazioni mock.
- Supporto futuro per diversi backend di archiviazione senza modificare i consumer.

### Esecutore come Confine di Sicurezza

Gli esecutori sono l'**unico** percorso per le operazioni I/O. Gli strumenti che richiedono accesso a disco, rete o riga di comando **devono** passare attraverso l'esecutore. Questo design impone:

- **Thread di pianificazione indipendente** per ogni esecutore, con blocco del thread per la verifica dei permessi.
- Controllo centralizzato dei permessi — L'esecutore interroga il **Gestore dei Permessi privato** dell'essere.
- Coda di richieste con supporto per priorità e controllo del timeout.
- Log di audit per tutte le operazioni esterne.
- Isolamento delle eccezioni — Il fallimento di un esecutore non influenza gli altri.
- Interruttore — Fallimenti consecutivi fermano temporaneamente l'esecutore per prevenire fallimenti a cascata.

### ContextManager come Oggetto Leggero

Ogni `ExecuteOneRound()` crea una nuova istanza di `ContextManager`:

1. Carica il File dell'Anima + la cronologia chat recente.
2. Invia la richiesta al client AI.
3. Elabora in ciclo le chiamate strumento finché l'AI restituisce testo puro.
4. Persiste la risposta nel sistema di chat.
5. Rilascia.

Questo mantiene ogni ciclo isolato e stateless.

### Auto-evoluzione tramite Sovrascrittura di Classe

Gli Esseri di Silicio possono sovrascrivere le proprie classi C# a runtime:

1. L'AI genera nuovo codice di classe (deve ereditare da `SiliconBeingBase`).
2. **Controllo dei riferimenti in fase di compilazione** (difesa primaria): Il compilatore riceve solo l'elenco degli assembly consentiti — `System.IO`, `System.Reflection` ecc. sono esclusi, quindi il codice pericoloso è impossibile a livello di tipo.
3. **Analisi statica a runtime** (difesa secondaria): `SecurityScanner` scansiona il codice per pattern pericolosi dopo la compilazione riuscita.
4. Roslyn compila il codice in memoria.
5. In caso di successo: `SiliconBeingManager.ReplaceBeing()` scambia l'istanza corrente, migra lo stato e persiste il codice crittografato su disco.
6. In caso di fallimento: Il nuovo codice viene scartato, l'implementazione esistente viene mantenuta.

Anche un'implementazione personalizzata di `IPermissionCallback` può essere compilata e iniettata tramite `ReplacePermissionCallback()`, permettendo agli esseri di personalizzare la propria logica di permessi.

Il codice è memorizzato su disco con crittografia AES-256. La chiave di crittografia è derivata dal GUID dell'essere (maiuscolo) tramite PBKDF2.

---

## Audit dell'Utilizzo dei Token

`TokenUsageAuditManager` traccia il consumo di token AI di tutti gli esseri:

- `TokenUsageRecord` — Record per richiesta (ID essere, modello, token prompt, token completamento, timestamp)
- `TokenUsageSummary` — Statistiche aggregate
- `TokenUsageQuery` — Parametri di query per filtrare i record
- Persistenza tramite `ITimeStorage` per query time-series
- Accessibile tramite Web UI (UsageController) e `TokenAuditTool` (solo Curatore)

---

### Sistema di Calendario

Il sistema include **32 implementazioni di calendario**, derivate dalla classe astratta `CalendarBase`, che coprono i principali sistemi calendariali del mondo:

| Calendario | ID | Descrizione |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Calendario buddista (BE), anno + 543 |
| CherokeeCalendar | `cherokee` | Sistema calendariale Cherokee |
| ChineseLunarCalendar | `lunar` | Calendario lunare cinese, con mese intercalare |
| ChineseHistoricalCalendar | `chinese_historical` | Calendario storico cinese, supporta cronologia Ganzhi e ere imperiali |
| ChulaSakaratCalendar | `chula_sakarat` | Calendario Chula Sakarat (CS), anno - 638 |
| CopticCalendar | `coptic` | Calendario copto |
| DaiCalendar | `dai` | Calendario Dai, con calcolo lunare completo |
| DehongDaiCalendar | `dehong_dai` | Variante del calendario Dai di Dehong |
| EthiopianCalendar | `ethiopian` | Calendario etiope |
| FrenchRepublicanCalendar | `french_republican` | Calendario repubblicano francese |
| GregorianCalendar | `gregorian` | Calendario gregoriano standard |
| HebrewCalendar | `hebrew` | Calendario ebraico |
| IndianCalendar | `indian` | Calendario nazionale indiano |
| InuitCalendar | `inuit` | Sistema calendariale Inuit |
| IslamicCalendar | `islamic` | Calendario islamico Hijri |
| JapaneseCalendar | `japanese` | Calendario delle ere giapponesi (Nengo) |
| JavaneseCalendar | `javanese` | Calendario islamico giavanese |
| JucheCalendar | `juche` | Calendario Juche (Corea del Nord), anno - 1911 |
| JulianCalendar | `julian` | Calendario giuliano |
| KhmerCalendar | `khmer` | Calendario khmer |
| MayanCalendar | `mayan` | Calendario lungo maya |
| MongolianCalendar | `mongolian` | Calendario mongolo |
| PersianCalendar | `persian` | Calendario persiano (Hijri solare) |
| RepublicOfChinaCalendar | `roc` | Calendario della Repubblica di Cina, anno - 1911 |
| RomanCalendar | `roman` | Calendario romano |
| SakaCalendar | `saka` | Calendario Saka (Indonesia) |
| SexagenaryCalendar | `sexagenary` | Calendario Ganzhi cinese (Sessagenario) |
| TibetanCalendar | `tibetan` | Calendario tibetano |
| VietnameseCalendar | `vietnamese` | Calendario lunare vietnamita (variante con il Gatto come zodiaco) |
| VikramSamvatCalendar | `vikram_samvat` | Calendario Vikram Samvat |
| YiCalendar | `yi` | Sistema calendariale Yi |
| ZoroastrianCalendar | `zoroastrian` | Calendario zoroastriano |

`CalendarTool` fornisce le operazioni: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (conversione di date tra calendari).

---

## Architettura Web UI

### Sistema Skin

La Web UI dispone di un **sistema di skin collegabile** che permette la personalizzazione completa dell'interfaccia senza modificare la logica dell'applicazione:

- **Interfaccia ISkin** — Definisce il contratto per tutte le skin, inclusi:
  - Metodi di rendering principali (`RenderHtml`, `RenderError`)
  - 20+ metodi di componenti UI (pulsanti, input, schede, tabelle, badge, bolle, progresso, tab, ecc.)
  - Generazione CSS del tema tramite `CssBuilder`
  - `SkinPreviewInfo` — Palette e icone per il selettore skin della pagina di inizializzazione

- **Skin integrate** — 7 skin pronte per la produzione:
  - **Admin** — Interfaccia di amministrazione professionale e focalizzata sui dati
  - **Chat** — Design conversazionale e incentrato sui messaggi per l'interazione AI
  - **Creative** — Layout artistico e visivamente ricco per flussi di lavoro creativi
  - **Dev** — Interfaccia incentrata sullo sviluppatore e sul codice con evidenziazione della sintassi
  - **HighContrast** — Tema ad alto contrasto per l'accessibilità
  - **Light** — Tema chiaro e fresco
  - **Minimal** — Tema minimalista

- **Scoperta delle skin** — `SkinManager` scopre e registra automaticamente tutte le implementazioni di `ISkin` tramite reflection

### Builder HTML / CSS / JS

La Web UI evita completamente i file template, generando tutto il markup in C#:

- **`H`** — DSL builder HTML streaming per costruire alberi HTML nel codice
- **`CssBuilder`** — Builder CSS con supporto per selettori e media query
- **`JsBuilder`** (`JsSyntax`) — Builder JavaScript per script inline

### Sistema Controller

La Web UI segue un **pattern simile a MVC**, con 24 controller che gestiscono aspetti diversi:

| Controller | Scopo |
|------------|---------|
| About | Pagina informazioni e informazioni sul progetto |
| Audit | Dashboard di audit dell'utilizzo dei token |
| Being | Gestione e stato degli Esseri di Silicio |
| Chat | Interfaccia di chat in tempo reale con SSE |
| ChatHistory | Visualizzazione cronologia chat, con elenco sessioni e dettagli messaggi |
| CodeBrowser | Visualizzazione e modifica del codice |
| CodeHover | Tooltip del codice con evidenziazione della sintassi |
| Config | Gestione configurazione di sistema |
| Dashboard | Panoramica di sistema e metriche |
| Executor | Stato e gestione degli esecutori |
| Help | Sistema di documentazione di aiuto, supporto multilingua |
| Init | Procedura guidata di inizializzazione alla prima esecuzione |
| Knowledge | Visualizzazione e interrogazione del grafo di conoscenza |
| Log | Visualizzatore log di sistema, con filtro per Essere di Silicio |
| Memory | Browser della memoria a lungo termine, con filtro avanzato, statistiche e vista dettagli |
| Permission | Gestione dei permessi |
| PermissionRequest | Coda delle richieste di permesso |
| Project | Gestione progetti, con note di lavoro, sistema di attività e permessi degli strumenti |
| System | Amministrazione di sistema e monitoraggio runtime |
| Task | Interfaccia del sistema di attività |
| Timer | Gestione del sistema timer, con cronologia esecuzione |
| ToolPermission | Gestione permessi strumenti, supporto configurazione permessi a livello di Essere di Silicio e progetto |
| Usage | Dashboard di audit dell'utilizzo dei token, con grafici di tendenza ed esportazione |
| WorkNote | Gestione note di lavoro, con ricerca e generazione indice |

### Aggiornamenti in Tempo Reale

- **SSE (Server-Sent Events)** — Aggiornamenti push per messaggi di chat, stato degli esseri ed eventi di sistema tramite `SSEHandler`
- **Nessun WebSocket necessario** — Architettura più semplice che utilizza SSE per la maggior parte delle esigenze in tempo reale
- **Riconnessione automatica** — Logica di riconnessione client per connessioni resilienti

### Localizzazione

Il sistema supporta la localizzazione completa in **34 varianti linguistiche**:
- **Cinese (6)**: zh-CN (semplificato), zh-HK (tradizionale), zh-SG (Singapore), zh-MO (Macao), zh-TW (Taiwan), zh-MY (Malesia)
- **Inglese (10)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Spagnolo (2)**: es-ES, es-MX
- **Tedesco (5)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **Francese (3)**: fr-FR, fr-CA, fr-CH
- **Altro (8)**: ja-JP (giapponese), ko-KR (coreano), cs-CZ (ceco), it-IT (italiano), pl-PL (polacco), pt-PT (portoghese), pt-BR (portoghese brasiliano), ru-RU (russo)

La locale attiva è selezionata tramite `DefaultConfigData.Language` e risolta tramite `LocalizationManager`.

---

### Sistema di Automazione Browser WebView (Nuovo)

Il sistema integra funzionalità di automazione browser WebView basate su **Playwright**:

- **Isolamento individuale**: Ogni Essere di Silicio possiede un'istanza browser indipendente, cookie e archiviazione di sessione, completamente isolati senza interferenze reciproche.
- **Modalità headless**: Il browser opera in modalità headless completamente invisibile all'utente, con gli Esseri di Silicio che operano autonomamente in background.
- **WebViewBrowserTool**: Fornisce capacità complete di operazione browser, tra cui:
  - Navigazione pagine, clic, inserimento testo, ottenimento contenuto pagina
  - Esecuzione JavaScript, acquisizione screenshot, attesa comparsa elementi
  - Gestione stato browser e pulizia risorse
- **Controllo di sicurezza**: Tutte le operazioni del browser devono passare attraverso la catena di verifica dei permessi, prevenendo accessi malevoli al web.

### Sistema Rete di Conoscenza (Nuovo)

Il sistema include un sistema di grafo di conoscenza basato su **struttura a tripla**:

- **Rappresentazione della conoscenza**: Utilizza struttura a tripla "soggetto-relazione-oggetto" (es: Python-is_a-programming_language)
- **KnowledgeTool**: Fornisce gestione del ciclo di vita completo della conoscenza:
  - `add`/`query`/`update`/`delete` - Operazioni CRUD di base
  - `search` - Ricerca full-text e corrispondenza per parole chiave
  - `get_path` - Scoperta del percorso di associazione tra due concetti
  - `validate` - Verifica dell'integrità della conoscenza
  - `stats` - Statistiche analitiche della rete di conoscenza
- **Archiviazione persistente**: Le triple di conoscenza sono persistite nel file system, con supporto per query indicizzate nel tempo.
- **Punteggio di confidenza**: Ogni voce di conoscenza ha un punteggio di confidenza (0-1), supportando corrispondenza fuzzy e ordinamento della conoscenza.
- **Classificazione per tag**: Supporta l'aggiunta di tag alla conoscenza per facilitare la categorizzazione e il recupero.

---

## Struttura della Directory dei Dati

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # File dell'Anima del Curatore
    │   ├── state.json       # Stato runtime
    │   ├── code.enc         # Codice della classe personalizzata crittografato AES
    │   └── permission.enc   # Callback dei permessi personalizzato crittografato AES
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## Motore di Archiviazione SpeedyPack

SiliconLife.Fast utilizza il motore di archiviazione SpeedyPack proprietario (formato .spk), che sostituisce la precedente soluzione LiteDB, ottenendo prestazioni di lettura/scrittura estreme.

### Design dell'Architettura

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (Mappa dir.   │  │  (Cache       │  │ (Coda scrittura│  │
│  │  in memoria)  │  │   voci)       │  │  asincrona)   │  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (Lettore/Scrittore file pacchetto)       │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              File .spk (MessagePack + compressione LZ4)│  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (Gestione     │  │ AutoCompactor│                      │
│  │  spazi liberi)│  │ (Compattaz.  │                      │
│  │              │  │  automatica)  │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Componenti Principali

| Componente | Descrizione |
|------|------|
| `SpeedyPack` | Classe principale, combina DirectoryMap, EntryCache e WriteQueue per fornire lettura/scrittura a bassa latenza |
| `DirectoryMap` | Mappa directory in memoria, mantiene la mappatura tra percorsi virtuali e voci di file |
| `EntryCache` | Cache delle voci, cache delle voci accessate di recente basata su TTL |
| `WriteQueue` | Coda di scrittura asincrona, accoda le operazioni di scrittura per l'esecuzione su thread in background |
| `FreeList` | Gestione degli spazi liberi, traccia lo spazio riutilizzabile nei file .spk |
| `PackFileReader` | Lettore di file pacchetto, legge dati dai file .spk |
| `PackFileWriter` | Scrittore di file pacchetto, scrive dati nei file .spk |
| `SpeedyPackAutoCompactor` | Timer di compattazione automatica, compatta periodicamente i file .spk per recuperare spazio libero |
| `SpeedyPackRegistry` | Gestore singleton a livello di processo, garantisce che l'intera applicazione utilizzi la stessa istanza SpeedyPack |

### Adattatori di Archiviazione

SiliconLife.Fast integra SpeedyPack nelle interfacce di sistema tramite i seguenti adattatori:

| Adattatore | Interfaccia | Descrizione |
|--------|------|------|
| `SpeedyStorage` | `IStorage` | Adattatore di archiviazione chiave-valore generico |
| `SpeedyTimeStorage` | `ITimeStorage` | Adattatore di archiviazione con indice temporale |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Adattatore di archiviazione delle note di lavoro |

### Opzioni di Configurazione

`SpeedyPackOptions` fornisce le seguenti configurazioni:

| Opzione | Tipo | Predefinito | Descrizione |
|------|------|--------|------|
| `CacheTtl` | `TimeSpan` | 5 minuti | Tempo di vita delle voci nella cache |
| `MaxCacheEntries` | `int` | 1000 | Numero massimo di voci nella cache |
| `ReadOnly` | `bool` | false | Modalità sola lettura |

### Supporto alle Transazioni

SpeedyPack supporta operazioni di scrittura atomica tramite l'interfaccia `IPackTransaction`:

- `SpeedyTransaction` implementa il meccanismo transazionale
- Supporta l'atomicità delle scritture batch
- Al commit della transazione, tutte le operazioni di scrittura riescono completamente o vengono completamente annullate

---

## Sistema di Plugin

SiliconLife supporta l'estensione delle funzionalità tramite un sistema di plugin, permettendo agli sviluppatori terzi di aggiungere nuove funzionalità alla piattaforma.

### Interfaccia Principale

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Caricatore di Plugin

`PluginLoader` è responsabile del caricamento delle DLL dei plugin dalla directory specificata e dell'esecuzione di controlli di sicurezza rigorosi:

1. **Scansione directory** — Scansiona tutti i file .dll nella directory dei plugin
2. **Scansione di sicurezza** — Verifica che i plugin non facciano riferimento a namespace proibiti
3. **Caricamento isolato** — Carica i plugin in isolamento utilizzando `AssemblyLoadContext` personalizzato
4. **Gestione del ciclo di vita** — Chiama i metodi OnLoad, OnStart, OnStop, OnUnload dei plugin

### Sandbox di Sicurezza

Il caricatore di plugin esegue i seguenti controlli di sicurezza:

| Elemento di controllo | Descrizione |
|--------|------|
| Namespace proibiti | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Whitelist assembly attendibili | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Controllo tipi proibiti | Scansione di tipi pericolosi referenziati nel plugin |
| Controllo membri proibiti | Scansione di metodi pericolosi richiamati nel plugin |

### Integrazione degli Strumenti

I plugin possono registrare strumenti personalizzati implementando l'interfaccia `ITool`:

- Il metodo `ToolManager.ScanAllPluginAssemblies()` scansiona le implementazioni ITool in tutti i plugin caricati
- Gli strumenti dei plugin si integrano automaticamente nel ciclo di chiamata degli strumenti
- Gli strumenti dei plugin sono soggetti allo stesso sistema di permessi

### Ciclo di Vita dei Plugin

```
Caricamento (OnLoad) → Avvio (OnStart) → In esecuzione → Arresto (OnStop) → Scaricamento (OnUnload)
```

---

## Stato di Attività degli Esseri di Silicio

Gli Esseri di Silicio hanno i seguenti stati di attività:

| Stato | Descrizione |
|------|------|
| `Idle` | Stato inattivo, in attesa di attivazione Tick |
| `SingleChat` | Chat uno-a-uno in corso |
| `GroupChat` | Chat di gruppo in corso |
| `Task` | Esecuzione di un'attività in corso |
| `Timer` | Esecuzione di un timer in corso |
| `Stopped` | Fermato, per errori consecutivi o arresto manuale |

**Meccanismo dello stato Stopped**:
- Quando un Essere di Silicio subisce 10 errori consecutivi, entra automaticamente nello stato `Stopped`
- Dopo essere entrato nello stato Stopped, l'essere non eseguirà più alcuna attività
- Quando arriva un nuovo messaggio di chat, il contatore degli errori viene resettato e l'essere riprende l'esecuzione

Transizioni di stato:
```
Idle → SingleChat → Idle (chat completata)
Idle → GroupChat → Idle (chat di gruppo completata)
Idle → Task → Idle (attività completata)
Idle → Timer → Idle (timer completato)
Qualsiasi → Stopped (10 errori consecutivi)
Stopped → Idle (nuovo messaggio di chat o riavvio manuale)
```

---

## Motore di Workflow

Il motore di workflow è un sistema a stati basato su template, utilizzato per guidare i processi di collaborazione degli Esseri di Silicio nello spazio di progetto:

### Componenti Principali

| Componente | Descrizione |
|------|------|
| `WorkflowEngine` | Motore di workflow principale, gestisce template e istanze, esegue transizioni di stato guidate da Tick |
| `WorkflowTemplate` | Template di workflow, definisce l'insieme degli stati e le regole di transizione |
| `WorkflowInstance` | Istanza di workflow, collegata a un progetto specifico, traccia lo stato corrente |
| `WorkflowLog` | Log del workflow, registra la cronologia delle transizioni di stato |

### Meccanismo di Funzionamento

- **Registrazione template**: Registrazione dei template di workflow tramite `RegisterTemplate()`, definizione di stati e regole di transizione
- **Creazione istanze**: Creazione di istanze dai template, collegamento allo spazio di progetto
- **Guidata da Tick**: Le transizioni di stato sono guidate dal meccanismo Tick del ciclo principale
- **Registrazione log**: Tutte le transizioni di stato sono registrate automaticamente nel log

---

## Meccanismo di Sfumatura della Memoria

`MemoryFadeService` è un servizio di decadimento temporizzato che simula le caratteristiche di oblio della memoria biologica:

### Meccanismo di Funzionamento

- **Esecuzione temporizzata**: Eredita da `TickObject`, esegue un ciclo di decadimento ogni ora per impostazione predefinita
- **Decadimento dell'importanza**: Applica un algoritmo di decadimento alle voci di memoria di ogni Essere di Silicio, riducendo il punteggio di importanza
- **Archiviazione automatica**: Le memorie con importanza inferiore alla soglia vengono archiviate automaticamente (`ArchiveFadingMemories()`)
- **Tracciamento statistico**: Registra il numero di cicli di decadimento, il numero di voci con cambio di stato, ecc.

### Flusso di Decadimento

```
MemoryFadeService.OnTick()
  └── Itera su tutti gli Esseri di Silicio
       └── being.Memory.ApplyDecay()      # Applica il decadimento dell'importanza
       └── being.Memory.ArchiveFadingMemories()  # Archivia le memorie a bassa importanza
```

---

## Sistema di Area di Lavoro di Progetto

L'area di lavoro di progetto è un meccanismo di gestione dello spazio che supporta la collaborazione multi-Essere di Silicio:

### Funzionalità Principali

- **Ciclo di vita del progetto**: Creazione → Attivo → Archiviato → Distrutto
- **Assegnazione ruoli**: Supporta l'assegnazione di ruoli di progetto agli Esseri di Silicio
- **Isolamento dei permessi degli strumenti**: Configurazione dei permessi degli strumenti a livello di progetto, indipendente dai permessi a livello di Essere di Silicio
- **Note di lavoro**: Sistema di note a pagine nello spazio di progetto, con supporto per generazione indice e ricerca per parole chiave
- **Tracciamento attività**: Gestione delle attività a livello di progetto, con supporto per creazione, assegnazione e tracciamento dello stato
- **Integrazione workflow**: I progetti possono essere collegati a template di workflow per guidare i processi di collaborazione

### Strumenti Correlati

| Strumento | Scopo |
|------|------|
| `ProjectTool` | Gestione dello spazio di progetto (creazione, archiviazione, distruzione, assegnazione ruoli) |
| `ProjectTaskTool` | Gestione attività di progetto (creazione, assegnazione, aggiornamento stato) |
| `ProjectWorkNoteTool` | Note di lavoro di progetto (creazione, ricerca, generazione indice) |
| `ProjectWorkTool` | Operazioni di lavoro di progetto (creazione attività, chat di gruppo, trasmissione, completamento progetto) |
