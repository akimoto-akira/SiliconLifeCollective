# Design della Sicurezza

> **Versione: v0.2.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md) | [Русский](../ru-RU/security.md) | **Italiano**

## Panoramica

La sicurezza di Silicon Life Collective si fonda su un modello di **difesa a strati**. Principio fondamentale: **tutte le operazioni I/O devono passare attraverso l'esecutore**, che impone il controllo dei permessi prima dell'esecuzione.

```
Chiamata strumento → Esecutore → Gestore dei Permessi → Cache frequenza → Callback → (IsCurator: chiedi all'utente | Non-curatore: ACL globale)
```

---

## Modello dei Permessi

### Tipi di Permesso

| Tipo | Descrizione |
|------|-------------|
| `NetworkAccess` | Richieste HTTP/HTTPS in uscita |
| `CommandLine` | Esecuzione di comandi shell |
| `FileAccess` | Operazioni su file e directory |
| `Function` | Chiamate di funzioni sensibili |
| `DataAccess` | Accesso a dati di sistema o utente |

### Risultati dei Permessi

Ogni controllo dei permessi restituisce uno di tre risultati:

| Risultato | Comportamento |
|--------|----------|
| **Allowed (Consentito)** | L'operazione procede immediatamente |
| **Denied (Negato)** | L'operazione viene bloccata, registrata nel log di audit |
| **AskUser (Chiedi all'utente)** | L'operazione viene sospesa, richiede conferma dell'utente |

### Ruolo Speciale: Curatore di Silicio

Il Curatore di Silicio possiede il livello di autorizzazione più alto (`IsCurator = true`). Quando la catena dei permessi raggiunge il punto di diramazione, le operazioni del Curatore vengono sottoposte alla conferma dell'utente tramite `IPermissionAskHandler`, anziché essere automaticamente consentite. I non-Curatori interrogano invece l'ACL globale.

### Gestore dei Permessi Privato

Ogni Essere di Silicio possiede la propria istanza **privata di PermissionManager**. Lo stato dei permessi non è condiviso tra gli esseri.

---

## Flusso di Verifica dei Permessi

La priorità di interrogazione è: **1. Cache frequenza → 2. Funzione callback → 3. Diramazione (IsCurator/GlobalACL)**

```
┌─────────────┐
│ Chiamata     │
│ strumento    │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Esecutore   │────▶│ Gestore dei          │
│(disco/rete/  │     │ Permessi privato     │
│ riga di      │     │ (per ogni essere)    │
│ comando...)  │     └────────┬────────────┘
└─────────────┘              │
                               ▼
                      ┌─────────────────┐
                      │ 1. Cache         │──corrisponde──▶ Consentito / Negato
                      │    frequenza     │
                      │ (alto rifiuto    │
                      │  precedenza su   │
                      │  alto consenso)  │
                      └────────┬────────┘
                               │ Non corrisponde
                               ▼
                      ┌─────────────────┐
                      │ 2. Funzione     │
                      │    callback     │──▶ Consentito / Negato / Chiedi utente
                      │    permessi     │
                      └────────┬────────┘
                               │ Chiedi utente
                               ▼
                      ┌─────────────────┐
                      │ 3. IsCurator?   │
                      └────────┬────────┘
                               │
                     ┌─────────┴─────────┐
                     │                   │
                     ▼ Sì               ▼ No
              ┌─────────────┐    ┌─────────────┐
              │ Chiedi       │    │ ACL globale  │
              │ all'utente   │    │ Interroga    │
              │ (AskHandler) │    │ le regole    │
              └─────────────┘    └─────────────┘
```

**Punto chiave**: L'esecutore vede solo un valore booleano (consentito/negato). Il Gestore dei Permessi gestisce internamente la decisione ternaria (consentito/negato/chiedi utente) e risolve la richiesta all'utente prima di restituire il risultato all'esecutore.

---

## Esecutore (Confine di Sicurezza)

L'esecutore è l'**unico** percorso per le operazioni I/O. Impone:

### Thread di Pianificazione Indipendente

Ogni esecutore possiede un **thread di pianificazione indipendente**:

- Isolamento dei thread tra esecutori — il blocco del thread di un esecutore non influenza gli altri.
- Ogni esecutore può impostare limiti di risorse indipendenti (CPU, memoria, ecc.).
- Gestione del pool di thread per i thread dell'esecutore.

### Coda delle Richieste

Ogni esecutore mantiene una coda di richieste:

- Le richieste vengono instradate all'esecutore appropriato in base al tipo.
- Supporto per l'accodamento con priorità.
- Controllo del timeout per ogni richiesta.

### Blocco del Thread per la Verifica dei Permessi

Quando uno strumento avvia un accesso alle risorse:

1. L'esecutore riceve la richiesta e **blocca il proprio thread**.
2. L'esecutore interroga il Gestore dei Permessi privato dell'essere.
3. Se il callback restituisce "chiedi utente", il thread dell'esecutore **rimane bloccato** in attesa della risposta dell'utente.
4. L'essere vede solo il risultato finale (successo o rifiuto) — non vede mai lo stato intermedio di "in sospeso" o "in attesa".
5. Solo il Curatore di Silicio attiva una vera richiesta all'utente. Gli esseri ordinari interrogano l'ACL globale in modo sincrono senza bloccarsi.
6. In caso di timeout, la richiesta viene considerata rifiutata e il blocco del thread viene rilasciato.

### Tipi di Esecutore

| Esecutore | Ambito | Timeout Predefinito |
|----------|-------|-----------------|
| `DiskExecutor` | Lettura/scrittura file, operazioni su directory | 30 secondi |
| `NetworkExecutor` | Richieste HTTP, connessioni WebSocket | 60 secondi |
| `CommandLineExecutor` | Esecuzione di comandi shell | 120 secondi |

> **Nota**: Il `DynamicCompilationExecutor` (nel namespace `SiliconLife.Core.Compilation`) gestisce la compilazione in memoria Roslyn, non rientra nella categoria degli esecutori I/O, ma è comunque soggetto al sistema di permessi.

### Isolamento delle Eccezioni e Tolleranza ai Guasti

- Le eccezioni di un esecutore non influenzano gli altri.
- Riavvio automatico in caso di crash del thread.
| Interruttore: Dopo fallimenti consecutivi, l'esecutore viene temporaneamente fermato per prevenire fallimenti a cascata.

---

## ACL Globale (Lista di Controllo degli Accessi)

Tabella di regole condivisa persistita nell'archivio, gestita solo dal Curatore di Silicio:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- Le regole vengono valutate in ordine; la prima corrispondenza vince.
- Solo il Curatore di Silicio può modificare l'ACL globale (tramite il suo strumento dedicato).
- Le modifiche hanno effetto immediato.
- L'ACL globale **non è** nella catena di priorità di ogni interrogazione sopra descritta — è referenziata internamente dalla funzione callback.

---

## Cache della Frequenza Utente

Per ridurre le richieste di permesso ripetute, il sistema mantiene due cache **per essere, solo in memoria**:

| Cache | Scopo |
|-------|---------|
| **HighAllow (Alto consenso)** | Risorse che l'utente concede frequentemente |
| **HighDeny (Alto rifiuto)** | Risorse che l'utente nega frequentemente |

### Come Funziona

- **Scelta dell'utente, non rilevamento automatico**: Quando viene attivata la richiesta all'utente, l'utente sceglie se aggiungere la risorsa alla cache.
- **Corrispondenza per prefisso**: Supporta la corrispondenza del prefisso del percorso della risorsa (es. `network:api.example.com/*`).
- **Priorità**: L'alto rifiuto ha la precedenza sull'alto consenso.
- **Solo in memoria**: La cache non è persistita. Viene persa al riavvio.
- **Scadenza configurabile**: L'utente può impostare la durata di validità delle voci della cache.

### Flusso di Aggiornamento della Cache

1. Il callback dei permessi restituisce `AskUser`.
2. Il sistema dei permessi invia una richiesta al sistema delle schede (Web UI o messaggistica istantanea).
3. L'utente prende una decisione (consenti/nega) e **sceglie se memorizzare nella cache**.
4. Il sistema delle schede restituisce la decisione + il flag di cache.
5. Il sistema dei permessi aggiorna la rispettiva lista della cache.
6. Le richieste future che corrispondono al prefisso della cache vengono risolte immediatamente.

---

## Meccanismo di Richiesta all'Utente

Quando il controllo dei permessi restituisce `AskUser`:

### Web UI: Scheda Interattiva

Il frontend web mostra immediatamente una **scheda interattiva**, che visualizza:

- Tipo di risorsa e percorso
- Descrizione dell'operazione
- Pulsanti Consenti / Nega
| Casella di controllo opzionale "Consenti sempre" / "Nega sempre" (aggiunge alla cache della frequenza)

### Messaggistica Istantanea (senza supporto schede): Codice Casuale

Per le piattaforme di messaggistica che non supportano schede interattive:

1. Il sistema genera due codici casuali a 6 cifre: **codice di consenso** e **codice di rifiuto**.
2. Invia un messaggio contenente le informazioni sulla risorsa e i due codici.
3. L'utente deve rispondere con il codice di consenso esatto per autorizzare. Qualsiasi altra risposta viene considerata come rifiuto.
4. I codici sono monouso, per prevenire attacchi di riproduzione.

### Timeout

- Un timeout è impostato per tutte le richieste all'utente.
| In caso di timeout, la richiesta viene considerata **rifiutata** e il blocco del thread dell'esecutore viene rilasciato.

---

## Sicurezza della Compilazione Dinamica

L'auto-evoluzione (sovrascrittura di classe) introduce rischi di sicurezza unici. Il sistema li mitiga con una **strategia a strati**:

### Livello 1: Controllo dei Riferimenti in Fase di Compilazione (Difesa Primaria)

- Il compilatore riceve solo l'**elenco dei riferimenti ad assembly consentiti**.
- **Consentiti**: `System.Runtime`, `System.Private.CoreLib`, assembly del progetto (interfaccia ITool, ecc.)
- **Bloccati**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices`, ecc.
- Se il codice fa riferimento a un assembly bloccato, **il compilatore stesso rifiuta** il codice.
- Questo è più affidabile della scansione a runtime — le operazioni pericolose sono impossibili a livello di tipo.

### Livello 2: Analisi Statica a Runtime (Difesa Secondaria)

- Anche dopo una compilazione riuscita, il codice viene sottoposto a scansione di pattern statici.
- Rileva pattern di operazioni pericolose (I/O diretto, chiamate di sistema, ecc.).
- Se viene trovato codice pericoloso, il caricamento viene rifiutato e il sistema torna alla funzionalità predefinita.

### Vincolo di Ereditarietà

Tutte le classi personalizzate di Esseri di Silicio **devono** ereditare da `SiliconBeingBase`. Il compilatore impone questo vincolo a livello di tipo.

### Archiviazione Crittografata

Il codice compilato è archiviato su disco con crittografia AES-256:

- **Derivazione della chiave**: Dal GUID dell'essere (maiuscolo) tramite PBKDF2.
- **Fallimento della decrittografia**: Fallback all'implementazione predefinita.
- **Ricompilazione a runtime**: Il nuovo codice viene prima compilato in memoria; solo dopo compilazione riuscita e sostituzione dell'istanza viene persistito.

### Sostituzione Atomica

Il processo di sostituzione è atomico:

1. Compila il nuovo codice in memoria → ottieni il `Type`.
2. Crea una nuova istanza dal `Type`.
3. Migra lo stato dalla vecchia istanza alla nuova.
4. Scambia i riferimenti.
5. Persisti il codice crittografato.

Se un qualsiasi passo fallisce, la vecchia istanza rimane attiva.

---

## Funzione di Callback dei Permessi

### Design

Ogni PermissionManager detiene una **variabile della funzione di callback**:

- **Predefinito**: Punta alla funzione di permessi predefinita integrata.
- **Dopo compilazione dinamica**: Viene sovrascritta dalla funzione di permessi personalizzata dell'essere.
- **Alternativa**: In qualsiasi momento, un solo callback è attivo.
- **Fallimento della compilazione**: Non influenza il callback corrente — la funzione predefinita o l'ultima funzione personalizzata riuscita rimane efficace.

### Firma del Callback

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Restituisce `Allowed`, `Denied` o `AskUser`.

---

## Log di Audit

Tutte le decisioni sui permessi vengono registrate:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

I log vengono persistiti nell'archivio e possono essere consultati tramite la Web UI (controller dei log).

---

## Audit dell'Utilizzo dei Token

`TokenUsageAuditManager` fornisce il tracciamento del consumo di token AI correlato alla sicurezza:

- **Record per richiesta** — Ogni chiamata AI registra l'ID dell'essere, il modello, i token del prompt, i token di completamento e il timestamp.
- **Rilevamento anomalie** — Pattern di consumo dei token anomali possono indicare iniezione di prompt o abuso di risorse.
- **Accesso solo per il Curatore** — `TokenAuditTool` (contrassegnato con `[SiliconManagerOnly]`) permette al Curatore di interrogare e riepilogare l'utilizzo dei token.
- **Dashboard web** — `UsageController` fornisce una dashboard basata su browser con grafici di tendenza ed esportazione dati.
- **Archiviazione persistita** — I record sono archiviati tramite `ITimeStorage` per query time-series e analisi a lungo termine.

---

## Sicurezza dei Plugin

Il sistema di plugin introduce rischi di sicurezza legati all'esecuzione di codice di terze parti, mitigati attraverso i seguenti meccanismi:

### Sandbox di Sicurezza e Dichiarazione delle Capacità

`PluginLoader` esegue scansioni di sicurezza durante il caricamento dei plugin e supporta contemporaneamente il meccanismo di dichiarazione delle capacità:

1. **Capacità dichiarabili** — I plugin dichiarano le capacità necessarie tramite l'attributo `[PluginCapability]`:
   - `Network` — Accesso alla rete (consente riferimenti a `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`)
   - `FileIO` — Lettura/scrittura file (consente riferimenti a `System.IO`)
   - `Process` — Gestione dei processi
   - `AI` — Chiamate AI

2. **Capacità non dichiarabili** — Le seguenti capacità sono sempre bloccate:
   - P/Invoke (`System.Runtime.InteropServices`)
   - Codice Unsafe (`System.Runtime.CompilerServices.Unsafe`)
   - Reflection Emit (`System.Reflection.Emit`)
   - API del compilatore (`Microsoft.CodeAnalysis`)

3. **Whelist degli assembly attendibili** — I riferimenti ai seguenti assembly sono consentiti:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

4. **Controllo dei tipi proibiti** — Scansione dei tipi pericolosi referenziati nel plugin

5. **Controllo dei membri proibiti** — Scansione dei metodi pericolosi richiamati nel plugin

### Caricamento Isolato

- Ogni plugin viene caricato in isolamento utilizzando un `AssemblyLoadContext` personalizzato
- I tipi e gli assembly tra plugin non interferiscono tra loro
- Alla scaricamento del plugin è possibile rilasciare le risorse correlate

### Vincoli dei Permessi degli Strumenti

- Gli strumenti registrati dai plugin tramite l'interfaccia `ITool` sono soggetti allo stesso sistema di permessi
- Gli strumenti dei plugin non possono aggirare la catena di verifica dei permessi
- Gli strumenti dei plugin sono soggetti al marcatore `[SiliconManagerOnly]`

---

## Sicurezza dei Permessi degli Strumenti

Il sistema dei permessi degli strumenti fornisce un ulteriore livello di sicurezza che controlla quali operazioni degli strumenti gli Esseri di Silicio possono utilizzare:

### Isolamento dei Permessi a Due Livelli

1. **Livello dell'Essere di Silicio** — Ogni Essere di Silicio ha una configurazione dei permessi degli strumenti indipendente
2. **Livello del progetto** — I permessi degli strumenti nello spazio di progetto sono indipendenti dal livello dell'Essere di Silicio, realizzando l'isolamento dei permessi tra progetti

### Template dei Permessi

Il sistema fornisce template di permessi predefiniti per garantire una baseline di sicurezza:

- **readonly** — Permessi minimi, consente solo operazioni di lettura
- **restricted** — Permessi limitati, consente solo operazioni di base
- **full** — Permessi completi (solo per il Curatore)

### Caratteristiche di Sicurezza

- **Rifiuto predefinito** — Le operazioni degli strumenti non esplicitamente consentite vengono rifiutate per impostazione predefinita
- **Granularità delle operazioni** — Ogni operazione di ogni strumento è controllata indipendentemente (es. `network:get` consentito ma `network:post` negato)
- **Gestione del Curatore** — I permessi degli strumenti possono essere configurati solo dal Curatore di Silicio
- **Traccia di audit** — Le modifiche ai permessi degli strumenti sono registrate nel log di audit

---

## Sicurezza delle Competenze

Il sistema delle competenze riutilizza il sistema dei permessi degli strumenti e fornisce multiple protezioni:

### Permesso di Esecuzione

- L'id della competenza funge da nome dello strumento, incluso nella matrice dei permessi `ToolActionPermissionConfig` con l'azione `execute`
- Le competenze disabilitate non compaiono nelle definizioni degli strumenti visibili all'AI (doppia garanzia: filtro a livello Schema + verifica a runtime)
- Il Curatore può sempre eseguire; gli Esseri ordinari richiedono `IsActionAllowed(skillId, "execute")`

### Whitelist degli Strumenti e Unione dei Permessi

- Durante l'esecuzione della competenza sono consentiti solo gli strumenti nella `ToolWhitelist` (lista vuota = eredita tutti gli strumenti dell'Essere)
- Le restrizioni delle azioni della competenza e i permessi dell'Essere **prendono l'unione sul lato restrittivo** (`MergePermissions`): la competenza può solo restringere ulteriormente i permessi, mai ampliarli
- Le chiamate a strumenti fuori dalla whitelist falliscono direttamente (`Tool not in whitelist`)

### Guardrail di Consumo delle Risorse

- **Interruttore globale**: `SkillEnabled` disabilita con un clic l'intero sistema di competenze
- **Quota numerica**: il numero di competenze personalizzate per Essere è limitato da `MaxCustomSkillsPerBeing` (predefinito 50)
- **Limitazione dei round**: `maxToolRound = Min(valore dichiarato competenza, GlobalMaxToolRound predefinito 10)`, previene loop fuori controllo
- **Limitazione del timeout**: `timeout = Min(valore dichiarato competenza, GlobalSkillTimeoutSeconds predefinito 300s)`
- **Protezione contro la ricorsione**: una competenza in esecuzione non può richiamare se stessa

### Modifica dei Permessi

- Il Curatore può modificare tutte le competenze; gli Esseri ordinari possono modificare solo le competenze con fonte `Being`/`User`
- Il completamento automatico dei metadati riempie solo i campi mancanti, i campi forniti dall'utente non vengono mai sovrascritti dall'AI

---

## Sicurezza MCP

L'integrazione MCP segue il principio "sovranità dell'utente + coerenza dei permessi":

### Sovranità dell'Utente

- L'aggiunta, l'eliminazione, l'abilitazione/disabilitazione e la riconnessione dei server MCP **possono essere eseguite solo dall'utente tramite la Web UI** (/mcp o pagina di configurazione)
- Lo strumento `mcp` lato AI è una query in sola lettura (status/list_servers/list_tools), non può modificare l'elenco dei server
- L'interruttore globale `McpEnabled` può tagliare con un clic tutti gli strumenti esterni

### Isolamento degli Strumenti e Permessi

- Gli strumenti wrapper sono denominati `mcp_{serverId}_{toolName}`, isolati dal namespace degli strumenti integrati/plugin
- Ogni strumento wrapper dichiara automaticamente una singola azione `execute`, inclusa nella matrice dei permessi degli strumenti a due livelli, disabilitabile singolarmente per Essere/progetto
- Dopo la disabilitazione del server, i suoi strumenti vengono immediatamente deregistrati da tutti gli Esseri

### Confine di Trasporto e Processo

- I server `stdio` vengono eseguiti come sottoprocessi, ereditando solo le variabili di ambiente esplicitamente configurate (campo `env`)
- I server `http` comunicano tramite l'endpoint configurato, in caso di fallimento della connessione passano automaticamente allo stato error ed espongono `lastError`

---

## Sicurezza delle Chiavi IM

### Placeholder delle Variabili di Ambiente

I valori di configurazione della piattaforma IM supportano i placeholder `${ENV_VAR}` (es. `"${FEISHU_APP_SECRET}"`):

- `ConfigSecretResolver` risolve i placeholder su una **copia profonda**, il `config.json` originale mantiene sempre i placeholder intatti
- I successivi `SaveConfig` non riscrivono le chiavi in chiaro risolte sul disco
- Supporta placeholder a valore intero e placeholder incorporati nel valore (es. `prefix-${VAR}`)

### Sicurezza dell'Autorizzazione OAuth

- **state anti-CSRF**: 16 byte di numero casuale crittografico, verificato rigorosamente al callback
- **Timeout di 5 minuti**: la sessione di autorizzazione viene automaticamente annullata al timeout, le sessioni precedenti vengono annullate immediatamente quando sovrascritte
- **Archiviazione token**: accessToken/refreshToken/tokenExpiresAt vengono riscritti nella configurazione della piattaforma e resi persistenti, `authMode` viene contrassegnato come `oauth`
- L'URL di callback supporta la configurazione `redirectBaseUrl` (scenari di callback su rete pubblica)

### Sicurezza dei Messaggi

- Feishu: verifica della firma (`X-Lark-Signature`, SHA256) + decrittazione eventi AES-256-CBC + deduplicazione eventi (finestra di 10 minuti)
- WeChat Enterprise: crittografia/decrittografia e verifica della firma WXBizMsgCrypt
- DingTalk: la modalità Stream utilizza WebSocket crittografato; la modalità HTTP verifica il callback
