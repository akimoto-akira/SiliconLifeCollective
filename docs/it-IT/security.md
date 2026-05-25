# Progettazione della sicurezza

> **Versione: v0.2.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | [Français](../fr-FR/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md) | [Русский](../ru-RU/security.md) | **Italiano**

## Panoramica

La sicurezza di Silicon Life Collective si basa su un modello di **difesa a strati multipli**. Principio fondamentale: **Tutte le operazioni I/O devono passare attraverso esecutori**, gli esecutori applicano le verifiche dei permessi prima dell'esecuzione.

```
Chiamata strumento → Esecutore → Gestore permessi → UserFrequencyCache → IPermissionCallback → (Curatore→IPermissionAskHandler / Non-curatore→GlobalACL→Negazione predefinita)
```

---

## Modello di permessi

### Tipi di permessi

|| Tipo | Descrizione |
|------|-------------|
| `NetworkAccess` | Richieste HTTP/HTTPS in uscita |
| `CommandLine` | Esecuzione comandi shell |
| `FileAccess` | Operazioni su file e directory |
| `Function` | Chiamate a funzioni sensibili |
| `DataAccess` | Accesso ai dati di sistema o utente |

### Risultati del permesso

Ogni verifica del permesso restituisce uno dei tre risultati:

|| Risultato | Comportamento |
|----------|-------------|
| **Allowed (Consentito)** | L'operazione continua immediatamente |
| **Denied (Negato)** | L'operazione viene bloccata, il registro audit viene salvato |
| **AskUser (Chiedi all'utente)** | L'operazione viene sospesa, richiede la conferma dell'utente |

### Ruolo speciale: Silicon Curator

Il Silicon Curator ha il livello di permesso più elevato (`IsCurator = true`). Quando la catena di permessi raggiunge il livello 3 e il callback restituisce AskUser, le operazioni del curatore vengono sottoposte a conferma utente tramite `IPermissionAskHandler`. I non-curatori consultano invece la GlobalACL e, se non c'è una regola corrispondente, l'accesso viene negato per impostazione predefinita.

### PermissionManager privato

Ogni Silicon Being ha la propria istanza **privata** di PermissionManager. Gli stati dei permessi non sono condivisi tra i Beings.

---

## Flusso di validazione dei permessi

Priorità richiesta: **1. UserFrequencyCache → 2. IPermissionCallback → 3. Biforcazione curatore (IsCurator/GlobalACL)**

```
┌─────────────┐
│ Chiamata    │
│ strumento  │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Esecutore   │────▶│ PermissionManager   │
│(Disco/Rete/  │     │ privato             │
│  Comando...) │     │ (per Being)         │
└─────────────┘     └────────┬────────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ 1. Cache        │──Corrisponde──▶ Consentito / Negato
                    │    frequenza    │
                    │(rifiuto prio su │
                    │   consentito)   │
                    └────────┬────────┘
                             │ Nessuna corrispondenza
                             ▼
                    ┌─────────────────┐
                    │ 2. Callback     │──▶ Consentito / Negato / Chiedi utente
                    │    permessi     │
                    └────────┬────────┘
                             │ Chiedi utente
                             ▼
                    ┌─────────────────┐
                    │ 3. IsCurator?   │
                    └────────┬────────┘
                             │
                 ┌───────────┴───────────┐
                 │                       │
                 ▼ Sì                    ▼ No
          ┌─────────────┐    ┌─────────────┐
          │ Chiedi       │    │ GlobalACL   │
          │ utente       │    │ Verifica    │
          │ (AskHandler) │    │ regole      │
          └─────────────┘    └─────────────┘
```

**Punto chiave**: L'esecutore vede solo un booleano (consentito/negato). Il PermissionManager gestisce internamente la decisione ternaria (consentito/negato/chiedi utente) e risolve "chiedi utente" prima di restituire il risultato all'esecutore.

---

## Esecutori (Confine di sicurezza)

Gli esecutori sono l'**unico** percorso per le operazioni I/O. Applicano:

### Thread di scheduling separati

Ogni esecutore ha un **thread di scheduling separato**:

- Isolamento thread tra esecutori — il blocco di un thread esecutore non influisce sugli altri esecutori.
- Ogni esecutore può definire limiti di risorse separati (CPU, memoria, ecc.).
- Gestione pool di thread per i thread degli esecutori.

### Coda di richieste

Ogni esecutore gestisce una coda di richieste:

- Le richieste vengono instradate all'esecutore corrispondente per tipo.
- Supporta la coda prioritaria.
- Controllo timeout per richiesta.

### Blocco thread per validazione permessi

Quando uno strumento avvia un accesso alle risorse:

1. L'esecutore riceve la richiesta e **blocca il suo thread**.
2. L'esecutore interroga il PermissionManager privato del Being.
3. Se il callback restituisce Chiedi utente, il thread dell'esecutore **rimane bloccato** in attesa della risposta dell'utente.
4. Il Being vede solo il risultato finale (successo o rifiuto) — non vede mai lo stato intermedio "In attesa" o "Sospeso".
5. Solo il Silicon Curator attiva una vera richiesta utente. I Beings normali interrogano in modo sincrono la GlobalACL senza blocco.
6. In caso di timeout, la richiesta viene trattata come negata, il blocco del thread viene rilasciato.

### Tipi di esecutori

| Esecutore | Ambito | Timeout predefinito |
|-----------|--------|-------------------|
| `DiskExecutor` | Lettura/scrittura file, operazioni directory | 30 secondi |
| `NetworkExecutor` | Richieste HTTP, connessioni WebSocket | 30 secondi |
| `CommandLineExecutor` | Esecuzione comandi shell | 30 secondi |
| `DynamicCompilationExecutor` | Compilazione in memoria Roslyn | N/A (delegato a CompilationCore) |

### Isolamento eccezioni e tolleranza guasti

- Le eccezioni di un esecutore non influenzano gli altri esecutori.
- Riavvio automatico in caso di crash del thread.
- Circuit Breaker: Pausa temporanea dell'esecutore dopo fallimenti consecutivi per prevenire errori a cascata.

---

## ACL globale (Lista di controllo accessi)

Tabella di regole comune persistita nello storage, gestita unicamente dal Silicon Curator:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- Le regole vengono valutate sequenzialmente; la prima corrispondenza vince.
- Solo il Silicon Curator può modificare l'ACL globale (tramite uno strumento dedicato).
- Le modifiche hanno effetto immediato.
- L'ACL globale **è** nella catena di priorità menzionata sopra — viene consultata al livello 3 quando il chiamante non è un curatore.

---

## Cache frequenza utente

Per ridurre le richieste di permessi ripetute, il sistema gestisce due cache **per Being, solo in memoria**:

|| Cache | Utilizzo |
|-------|------------|
| **HighAllow** | Risorse frequentemente autorizzate dall'utente |
| **HighDeny** | Risorse frequentemente negate dall'utente |

### Funzionamento

- **Scelta utente, non rilevamento automatico**: Quando Chiedi utente viene attivato, l'utente sceglie se aggiungere la risorsa alla cache.
- **Corrispondenza prefisso**: Supporta la corrispondenza di prefisso del percorso risorsa (es. `network:api.example.com/*`).
- **Priorità**: HighDeny è prioritario su HighAllow.
- **Solo memoria**: Le cache non vengono persistite. Vengono perse al riavvio.
- **Scadenza configurabile**: L'utente può impostare la durata di validità delle voci cache.

### Flusso aggiornamento cache

1. Il callback permessi restituisce `AskUser`.
2. Il sistema permessi invia la richiesta al sistema di schede (interfaccia Web o IM).
3. L'utente prende una decisione (Consentito/Negato) e **sceglie se mettere in cache**.
4. Il sistema schede restituisce la decisione + il flag di cache.
5. Il sistema permessi aggiorna la lista cache corrispondente.
6. Le future richieste con il prefisso cache vengono risolte immediatamente.

---

## Meccanismo di richiesta all'utente

Quando la verifica del permesso restituisce `AskUser`:

### Interfaccia Web: Schede interattive

L'interfaccia Web mostra immediatamente una **scheda interattiva** con:

- Il tipo e il percorso della risorsa
- La descrizione dell'azione
- Pulsanti Consenti / Nega
- Casella opzionale "Consenti sempre" / "Nega sempre" (aggiungi alla cache frequenza)

### Messaggistica istantanea (senza supporto scheda): Codice casuale

Per le piattaforme di messaggistica senza supporto di scheda interattiva:

1. Il sistema genera due codici casuali a 6 cifre: **Codice Consenti** e **Codice Nega**.
2. Invia un messaggio con le informazioni sulla risorsa e i due codici.
3. L'utente deve rispondere con il codice Consenti esatto per consentire. Qualsiasi altra risposta viene trattata come rifiuto.
4. I codici sono monouso, prevenendo attacchi di replay.

### Timeout

- Un timeout è definito per tutte le richieste Chiedi utente.
- In caso di timeout, la richiesta viene trattata come **Negato**, il blocco del thread dell'esecutore viene rilasciato.

---

## Sicurezza della compilazione dinamica

L'auto-sviluppo (sostituzione classi) introduce rischi di sicurezza unici. Il sistema li attenua con una **strategia multistrato**:

### Livello 1: Controllo riferimenti in compilazione (Difesa principale)

- Il compilatore riceve solo un **elenco di assembly autorizzati**.
- **Autorizzato**: `System.Runtime`, `System.Private.CoreLib`, assembly del progetto (interfaccia ITool, ecc.)
- **Bloccato**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices`, ecc.
- Se il codice referenzia un assembly bloccato, **il compilatore stesso rifiuta** il codice.
- Più affidabile della scansione in esecuzione — le operazioni pericolose sono impossibili a livello di tipo.

### Livello 2: Analisi statica in esecuzione (Difesa secondaria)

- Anche dopo una compilazione riuscita, il codice viene scansionato per pattern statici.
- Rileva pattern di operazioni pericolose (I/O diretto, chiamate di sistema, ecc.).
- In caso di codice pericoloso, il caricamento viene rifiutato, il sistema torna alla funzionalità predefinita.

### Restrizione ereditarietà

Tutte le classi Silicon Being personalizzate **devono** ereditare da `SiliconBeingBase`. Il compilatore applica questa restrizione a livello di tipo.

### Archiviazione crittografata

Il codice compilato viene archiviato su disco crittografato con AES-256:

- **Derivazione chiave**: Dal GUID del Being (maiuscolo) tramite PBKDF2.
- **Errore decrittazione**: Ritorno all'implementazione predefinita.
- **Ricompilazione in esecuzione**: Il nuovo codice viene prima compilato in memoria; solo dopo una compilazione riuscita e la sostituzione dell'istanza viene persistito.

### Sostituzione atomica

Il processo di sostituzione è atomico:

1. Compilare il nuovo codice in memoria → ottenere il `Type`.
2. Creare una nuova istanza dal `Type`.
3. Migrare lo stato dalla vecchia istanza alla nuova.
4. Scambiare il riferimento.
5. Persistere il codice crittografato.

Se un passaggio fallisce, la vecchia istanza rimane attiva.

---

## Funzioni di callback dei permessi

### Progettazione

Ogni PermissionManager mantiene una **variabile di funzione di callback**:

- **Predefinita**: Punta alla funzione di permesso predefinita integrata.
- **Dopo compilazione dinamica**: Sostituita dalla funzione di permesso personalizzata del Being.
- **L'uno o l'altro**: Un solo callback attivo in qualsiasi momento.
- **Errore di compilazione**: Non influisce sul callback attuale — la funzione predefinita o l'ultima funzione personalizzata riuscita rimane attiva.

### Firma del callback

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Restituisce `Allowed`, `Denied` o `AskUser`.

---

## Registro audit

Tutte le decisioni sui permessi vengono registrate:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

I log vengono persistiti nello storage, consultabili tramite l'interfaccia Web (controller log).

---

## Audit utilizzo token

Il `TokenUsageAuditManager` fornisce il monitoraggio del consumo di token IA rilevante per la sicurezza:

- **Registrazione per richiesta** — Ogni chiamata IA registra l'ID del Being, il modello, i token prompt, i token completamento e il timestamp.
- **Rilevamento anomalie** — Pattern di consumo token insoliti possono indicare injection del prompt o abuso delle risorse.
- **Accesso solo Curator** — `TokenAuditTool` (marcato `[SiliconManagerOnly]`) consente al Curator di consultare e riassumere l'utilizzo dei token.
- **Dashboard Web** — `UsageController` fornisce una dashboard basata su browser con grafici di tendenza ed esportazione dati.
- **Archiviazione persistente** — I record vengono archiviati tramite `ITimeStorage` per query di serie temporali e analisi a lungo termine.

---

## Sicurezza dei plugin

Il sistema di plugin introduce rischi di sicurezza attraverso l'esecuzione di codice di terze parti, che vengono attenuati dai seguenti meccanismi:

### Sandbox di sicurezza

Il `PluginLoader` esegue verifiche di sicurezza rigorose durante il caricamento:

1. **Verifica namespace vietati** — I plugin non devono referenziare i seguenti namespace:
   - `System.IO` — Accesso al file system
   - `System.Net.Http` — Richieste HTTP
   - `System.Net.WebSockets` — Connessioni WebSocket
   - `System.Net.Sockets` — Socket raw
   - `Microsoft.CodeAnalysis` — API del compilatore

2. **Whitelist assembly fidati** — I riferimenti ai seguenti assembly sono consentiti:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

3. **Verifica tipi vietati** — Scansiona i tipi pericolosi referenziati nel plugin

4. **Verifica membri vietati** — Scansiona i metodi pericolosi chiamati nel plugin

### Caricamento isolato

- Ogni plugin viene caricato in modo isolato tramite un `AssemblyLoadContext` personalizzato
- I tipi e gli assembly tra plugin non si disturbano a vicenda
- Durante lo scaricamento di un plugin, le risorse associate possono essere liberate

### Restrizioni permessi strumenti

- I plugin che registrano strumenti tramite l'interfaccia `ITool` sono soggetti allo stesso sistema di permessi
- Gli strumenti plugin non possono bypassare la catena di permessi a 3 livelli
- Gli strumenti plugin sono soggetti alla marcatura `[SiliconManagerOnly]`
