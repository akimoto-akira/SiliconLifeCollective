# Sistema di Permessi

> **Versione: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## Panoramica

Il sistema di permessi garantisce che tutte le operazioni avviate dall'AI siano adeguatamente verificate e auditate.

## Catena di Verifica dei Permessi

```
┌─────────────────────────────────────────────┐
│          Verifica dei Permessi               │
├─────────────────────────────────────────────┤
│  Livello 1: UserFrequencyCache               │
│  ↓ Cache delle decisioni utente ad alta frequenza (HighDeny/HighAllow) │
│  Livello 2: IPermissionCallback              │
│  ↓ Logica personalizzata (Allowed/Denied/AskUser) │
│  Livello 3: IsCurator?                       │
│  ↓ Sì → IPermissionAskHandler (chiedi all'utente) │
│  ↓ No → GlobalACL → rifiuto predefinito     │
│  Risultato: Permesso o Rifiuto               │
└─────────────────────────────────────────────┘
```

> **Nota**: La priorità di interrogazione effettiva di `PermissionManager.CheckPermission()` è:
> 1. **UserFrequencyCache** — Controlla prima la cache delle decisioni utente ad alta frequenza
> 2. **IPermissionCallback** — Valuta le regole di callback personalizzate
> 3. **Ramificazione del Curatore** — Quando il callback restituisce AskUser o non c'è callback:
>    - **Curatore** → `IPermissionAskHandler` (chiede all'utente tramite IM)
>    - **Non curatore** → `GlobalACL` → rifiuto predefinito

## Livello 1: UserFrequencyCache

Cache delle decisioni utente ad alta frequenza per ogni essere (HighDeny/HighAllow), esiste solo in memoria.

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny ha la priorità su HighAllow**
- **Solo in memoria**: La cache non viene persistita, va persa al riavvio
- **Scadenza configurabile**: L'utente può impostare la validità delle voci della cache

## Livello 2: IPermissionCallback

Callback personalizzati per la logica dinamica dei permessi.

### Implementazione Predefinita DefaultPermissionCallback

`DefaultPermissionCallback` fornisce regole di permesso predefinite complete, tra cui:

#### Regole di accesso alla rete
- **Indirizzi di loopback**: Permetti localhost, 127.0.0.1, ::1
- **Indirizzi IP privati**:
  - 192.168.x.x (Classe C) - Permetti
  - 10.x.x.x (Classe A) - Permetti
  - 172.16-31.x.x (Classe B) - Chiedi all'utente
- **Whitelist dei domini**:
  - Motori di ricerca: Google, Bing, DuckDuckGo, Yandex, Sogou, ecc.
  - Servizi AI: OpenAI, Anthropic, HuggingFace, Ollama, ecc.
  - Servizi per sviluppatori: GitHub, StackOverflow, npm, NuGet, ecc.
  - Social media: Weibo, Zhihu, Reddit, Discord, ecc.
  - Piattaforme video: YouTube, Bilibili, Douyin, TikTok, ecc.
  - **Informazioni meteo**: wttr.in
  - Siti governativi: .gov, .go.jp, .go.kr
- **Blacklist dei domini**:
  - Siti di contraffazione AI: chatgpt, openai, deepseek, ecc. domini contraffatti
  - Strumenti AI malevoli: wormgpt, darkgpt, fraudgpt, ecc.
  - Fattorie di contenuti AI e domini relativi al mercato nero

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }
        
        return PermissionResult.AskUser;
    }
}
```

## Livello 3: Giudizio di Ramificazione (IsCurator / GlobalACL)

Quando il callback restituisce `AskUser` o non è configurato alcun callback, il sistema si ramifica in base all'identità del curatore:

### Ramo del Curatore (IsCurator = true)

Per il Curatore di Silicio, il sistema richiede una decisione all'utente tramite messaggistica istantanea:

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // L'utente conferma o rifiuta nella Web UI
    }
}
```

### Ramo Non Curatore (IsCurator = false)

Per gli esseri non curatori, il sistema verifica la Lista di Controllo degli Accessi Globale. Se non c'è una regola corrispondente, la richiesta viene rifiutata per impostazione predefinita.

### Struttura della GlobalACL

```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed"
    },
    {
      "permissionType": "FileAccess",
      "resourcePrefix": "C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

Le regole vengono valutate in ordine, la prima corrispondenza ha effetto. Solo il Curatore di Silicio può modificare l'ACL Globale.

### Formato delle Risorse

```
{type}:{path}

Esempi:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

Quando un'operazione del curatore richiede la conferma dell'utente, i permessi vengono richiesti tramite `IPermissionAskHandler`.

### Implementazione IMPermissionAskHandler

`IMPermissionAskHandler` invia richieste di permesso all'utente tramite la Web UI:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // Invia un messaggio all'utente tramite messaggistica istantanea
        SendMessageAsync($"Allow {resource}?");

        // Attende la risposta dell'utente
        var response = WaitForResponseAsync();

        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### Coda delle Richieste di Permesso PermissionRequestQueue

`PermissionRequestQueue` gestisce le richieste di permesso in sospeso, supportando l'attesa asincrona della risposta dell'utente:

- **Accodamento della richiesta** — Quando la catena dei permessi raggiunge il livello 5, crea un `TaskCompletionSource<AskPermissionResult>` e lo accoda
- **Visualizzazione Web UI** — Le richieste di permesso in sospeso vengono visualizzate nella Web UI tramite `PermissionRequestController`
- **Risposta dell'utente** — L'utente approva o rifiuta nella Web UI, con la possibilità di memorizzare nella cache la decisione e impostare la durata della cache
- **Opzioni di cache** — L'utente può memorizzare nella cache le decisioni sui permessi per 1 ora, 24 ore, 7 giorni o 30 giorni
- **Meccanismo di timeout** — Dopo 60 secondi senza risposta, la pagina di richiesta si chiude automaticamente

## Sistema di Audit

Tutte le decisioni sui permessi vengono registrate:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "callerId": "being-uuid",
  "permissionType": "FileAccess",
  "resource": "C:\\data\\config.json",
  "result": "Allowed",
  "reason": "Global ACL"
}
```

## Valutazione Programmatica dei Permessi

### API EvaluatePermission

Il metodo `PermissionManager.EvaluatePermission()` fornisce una pre-valutazione dei permessi in sola lettura, senza attivare prompt all'utente. `PermissionTool` utilizza questo metodo per consentire all'AI di verificare lo stato dei permessi prima di tentare un'operazione.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Valore restituito**: `PermissionResult` a tre stati:
- `Allowed` - L'operazione è permessa
- `Denied` - L'operazione è rifiutata
- `AskUser` - Richiede conferma dell'utente durante l'esecuzione

**Ordine di valutazione**:
1. **Cache di frequenza** - Controlla le decisioni utente memorizzate nella cache
2. **IPermissionCallback** - Valutazione del callback personalizzato
3. **Stato del curatore** - Se è il curatore, restituisce `AskUser` (richiede conferma)
4. **ACL Globale** - Controlla le regole di controllo degli accessi
5. **Predefinito** - Rifiuta quando non ci sono regole corrispondenti

> **Nota**: A differenza della catena di permessi completa, `EvaluatePermission` **non** chiama `IPermissionAskHandler`. Riporta solo quale *sarà* il risultato durante l'esecuzione.

## Gestione dei Permessi

### Concedere Permessi

**Tramite Web UI**:
1. Naviga verso **Gestione Permessi**
2. Clicca su **Aggiungi regola**
3. Configura:
   - Utente
   - Risorsa
   - Permetti/Rifiuta
   - Durata

**Tramite API**:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

### Revocare Permessi

Operazione tramite la pagina di gestione dei permessi della Web UI.

### Visualizzare i Permessi

```bash
curl http://localhost:8080/api/permissions/list
```

## Sistema dei Permessi degli Strumenti

Oltre alla catena di verifica dei permessi a livello di operazione, il sistema fornisce un meccanismo di gestione dei **permessi degli strumenti** per controllare quali strumenti possono utilizzare gli Esseri di Silicio.

### Permessi degli Strumenti a Due Livelli

I permessi degli strumenti sono divisi in due livelli:

1. **Livello Essere di Silicio** — Controlla quali operazioni degli strumenti può utilizzare un singolo Essere di Silicio
2. **Livello Progetto** — Controlla le operazioni degli strumenti disponibili nello spazio del progetto, indipendentemente dai permessi a livello di Essere di Silicio

### Configurazione dei Permessi degli Strumenti

Ogni operazione di ogni strumento può essere configurata indipendentemente come permessa o rifiutata:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network:get": "allowed",
    "network:post": "denied",
    "disk:read": "allowed",
    "disk:write": "denied",
    "database:query": "allowed"
  }
}
```

### Modelli di Permessi

Il sistema fornisce modelli di permessi degli strumenti predefiniti, applicabili rapidamente agli Esseri di Silicio:

- **readonly** — Permessi di sola lettura (permette operazioni di lettura, rifiuta operazioni di scrittura)
- **full** — Permessi completi (permette tutte le operazioni)
- **restricted** — Permessi limitati (permette solo operazioni di base)

### Gestione Web UI

Gestisci i permessi degli strumenti tramite la Web UI:

- **Pagina permessi strumenti Essere di Silicio** — `/beings/tool-permissions`
- **Pagina permessi strumenti progetto** — `/project/{id}/tool-permissions`

### Endpoint API

| Endpoint | Metodo | Descrizione |
|------|------|------|
| `/api/beings/tool-permissions` | GET | Ottieni i permessi degli strumenti dell'Essere di Silicio |
| `/api/beings/tool-permissions` | PUT | Aggiorna i permessi degli strumenti dell'Essere di Silicio |
| `/api/beings/tool-permissions/templates` | GET | Ottieni l'elenco dei modelli di permessi |
| `/api/beings/tool-permissions/apply-template` | POST | Applica un modello di permessi |
| `/api/projects/{id}/tool-permissions` | GET | Ottieni i permessi degli strumenti del progetto |
| `/api/projects/{id}/tool-permissions` | PUT | Aggiorna i permessi degli strumenti del progetto |

---

## Best Practice

### 1. Principio del Privilegio Minimo

Concedi solo i permessi minimi necessari:

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. Utilizza Permessi con Limitazione Temporale

A meno che non sia assolutamente necessario, non concedere mai permessi permanenti.

### 3. Monitora i Registri dei Permessi

Esamina regolarmente i registri di audit per comprendere:
- Tentativi di accesso rifiutati
- Modelli anomali
- Escalation dei permessi

### 4. Implementa Callback Personalizzati

Per logiche complesse, utilizza `IPermissionCallback`:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // Permessi basati sul tempo
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // Permessi basati sulla risorsa
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## Scenari Comuni

### Scenario 1: L'AI vuole leggere un file

```
AI: "Devo leggere config.json"
↓
Catena dei permessi:
1. UserFrequencyCache? Nessuna decisione in cache
2. IPermissionCallback? Restituisce AskUser (non esplicitamente permesso)
3. IsCurator? No → Controlla GlobalACL
4. GlobalACL? Trova regola: file:... = Allowed
5. Risultato: Permesso
```

### Scenario 2: L'AI vuole eseguire codice

```
AI: "Voglio compilare ed eseguire codice"
↓
Catena dei permessi:
1. UserFrequencyCache? Nessuna decisione in cache
2. IPermissionCallback? Restituisce AskUser
3. IsCurator? Sì → IPermissionAskHandler
4. L'utente approva
5. Risultato: Permesso
```

### Scenario 3: Rifiuto memorizzato nella cache

```
AI: "Devo accedere a C:\Windows"
↓
Catena dei permessi:
1. UserFrequencyCache? Trovato nella cache HighDeny
2. Risultato: Rifiuto (nessun ulteriore controllo necessario)
```

## Risoluzione dei Problemi

### Permesso inaspettatamente rifiutato

**Verifica**:
1. Lo stato IsCurator dell'utente
2. Le voci HighDeny nella cache di frequenza
3. Le regole GlobalACL
4. La logica del callback
5. Il timeout della risposta dell'utente

### Permessi non scaduti

**Verifica**:
- Il campo `expiresAt` è impostato correttamente
- Il fuso orario è corretto
- L'orologio è sincronizzato

### Registro di audit non registrato

**Verifica**:
- Il logger di audit è registrato
- Il backend di archiviazione è accessibile
- C'è spazio su disco sufficiente

## Prossimi Passi

- 📚 Leggi la [guida all'architettura](architecture.md)
- 🛠️ Consulta la [guida allo sviluppo](development-guide.md)
- 🔒 Consulta la [documentazione sulla sicurezza](security.md)
- 🚀 Consulta la [guida all'inizio rapido](getting-started.md)
