# Sistema di permessi

> **Versione: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [Français](../fr-FR/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | **Italiano**

## Panoramica

Il sistema di permessi garantisce che tutte le operazioni avviate dall'IA siano correttamente verificate e sottoposte ad audit.

## Catena di permessi a 3 livelli

```
┌─────────────────────────────────────────────┐
│          Verifica dei permessi              │
├─────────────────────────────────────────────┤
│  Livello 1 : UserFrequencyCache             │
│  ↓ Decisioni utente in cache (HighDeny/HighAllow)│
│  Livello 2 : IPermissionCallback            │
│  ↓ Logica personalizzata (Consentito/Negato/ChiediUtente)│
│  Livello 3 : IsCurator?                     │
│  ↓ Sì → IPermissionAskHandler (chiedi all'utente)│
│  ↓ No → GlobalACL → Negazione predefinita   │
│  Risultato : Consentito o Negato            │
└─────────────────────────────────────────────┘
```

> **Nota**: La priorità di consultazione effettiva in `PermissionManager.CheckPermission()` è:
> 1. **UserFrequencyCache** — Verificare prima le decisioni utente ad alta frequenza in cache
> 2. **IPermissionCallback** — Valutare le regole callback personalizzate
> 3. **Biforcazione curatore** — Se il callback restituisce AskUser o non c'è callback configurato:
>    - **Curatore** → `IPermissionAskHandler` (chiedere all'utente tramite IM)
>    - **Non-curatore** → `GlobalACL` → negazione predefinita

## Livello 1: UserFrequencyCache

Cache per essere, solo in memoria, delle decisioni utente ad alta frequenza (HighDeny/HighAllow).

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny** ha priorità su **HighAllow**
- **Solo memoria**: Le cache non vengono persistite, vengono perse al riavvio
- **Scadenza configurabile**: Gli utenti possono impostare il periodo di validità per le voci della cache

## Livello 2: IPermissionCallback

Callback personalizzati per la logica di permessi dinamica.

### Implementazione predefinita DefaultPermissionCallback

`DefaultPermissionCallback` fornisce regole di permessi predefinite complete, includendo:

#### Regole di accesso rete
- **Indirizzi di loopback**: Consenti localhost, 127.0.0.1, ::1
- **Indirizzi IP privati**:
  - 192.168.x.x (Classe C) - Consentito
  - 10.x.x.x (Classe A) - Consentito
  - 172.16-31.x.x (Classe B) - Chiedi all'utente
- **Whitelist domini**:
  - Motori di ricerca: Google, Bing, DuckDuckGo, Yandex, Sogou, ecc.
  - Servizi IA: OpenAI, Anthropic, HuggingFace, Ollama, ecc.
  - Servizi sviluppatori: GitHub, StackOverflow, npm, NuGet, ecc.
  - Social media: Weibo, Zhihu, Reddit, Discord, ecc.
  - Piattaforme video: YouTube, Bilibili, Douyin, TikTok, ecc.
  - **Informazioni meteo**: wttr.in
  - Siti governativi: .gov, .go.jp, .go.kr
- **Blacklist domini**:
  - Siti di contraffazione IA: chatgpt, openai, deepseek e altri domini contraffatti
  - Strumenti IA dannosi: wormgpt, darkgpt, fraudgpt, ecc.
  - Fattorie contenuti IA e domini mercato nero

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(PermissionRequest request)
    {
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }
        
        return PermissionResult.AskUser("Needs user confirmation");
    }
}
```

## Livello 3: Biforcazione curatore (IsCurator / GlobalACL)

Quando i livelli 1 e 2 non hanno preso una decisione (il callback restituisce AskUser o non c'è callback configurato), il sistema si ramifica in base all'identità del chiamante:

### Ramo curatore (IsCurator = true)

Se il chiamante è il curatore, il callback restituisce AskUser e il sistema richiede conferma dell'utente tramite `IPermissionAskHandler`:

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        var result = _askHandler.AskUser(request);
    }
}
```

### Ramo non-curatore (IsCurator = false)

Se il chiamante non è il curatore, verifica la `GlobalACL` lista di controllo accessi. Se non c'è una regola corrispondente, l'accesso viene negato per impostazione predefinita.

### Struttura GlobalACL

```json
{
  "rules": [
    {
      "prefix": "disk:read",
      "result": "Allowed"
    },
    {
      "prefix": "network:http",
      "result": "Allowed"
    },
    {
      "prefix": "compile:execute",
      "result": "Denied"
    }
  ]
}
```

> **Nota**: GlobalACL utilizza la corrispondenza per prefisso. La regola `"prefix": "disk:read"` corrisponde a tutte le risorse che iniziano con `disk:read`. Se non c'è alcuna regola corrispondente, l'accesso viene negato per impostazione predefinita.

### Formato delle risorse

```
{type}:{action}

Esempi:
- disk:read
- disk:write
- network:http
- compile:execute
- system:info
```

## IPermissionAskHandler

Quando un'operazione del curatore richiede la conferma dell'utente, questa viene richiesta tramite `IPermissionAskHandler`.

### Implementazione IMPermissionAskHandler

`IMPermissionAskHandler` invia richieste di permesso all'utente tramite la Web UI:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(PermissionRequest request)
    {
        // Inviare un messaggio all'utente tramite messaggistica istantanea
        SendMessage($"Consentire {request.Resource}?");
        
        // Attendere la risposta dell'utente
        var response = WaitForResponse();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### Coda richieste permessi PermissionRequestQueue

`PermissionRequestQueue` gestisce le richieste di permessi in attesa, supportando l'attesa asincrona delle risposte utente:

- **Accodamento** — Quando la catena permessi raggiunge il livello 3 (ramo curatore), crea un `TaskCompletionSource<AskPermissionResult>` e lo accoda
- **Visualizzazione Web UI** — Mostra le richieste di permesso in attesa tramite `PermissionRequestController` nell'interfaccia Web
- **Risposta utente** — L'utente approva o rifiuta nell'interfaccia Web, con possibilità di mettere in cache la decisione e impostare la durata della cache
- **Opzioni cache** — L'utente può memorizzare nella cache la decisione del permesso per 1 ora, 24 ore, 7 giorni o 30 giorni
- **Meccanismo timeout** — Chiusura automatica della pagina di richiesta dopo 60 secondi senza risposta

## Sistema di audit

Tutte le decisioni sui permessi vengono registrate:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "beingId": "being-uuid",
  "resource": "disk:write",
  "result": "Allowed",
  "level": "GlobalACL",
  "reason": "Prefix rule matched"
}
```

## Valutazione programmatica dei permessi

### API EvaluatePermission

Il metodo `PermissionManager.EvaluatePermission()` fornisce una pre-valutazione in sola lettura dei permessi senza attivare richieste utente. `PermissionTool` utilizza questo metodo per consentire all'IA di verificare lo stato dei permessi prima di tentare un'operazione.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Valore di ritorno**: `PermissionResult` a tre stati:
- `Allowed` - L'operazione è consentita
- `Denied` - L'operazione è negata
- `AskUser` - Conferma utente richiesta durante l'esecuzione

**Ordine valutazione**:
1. **Cache frequenza** - Verificare decisioni utente in cache
2. **IPermissionCallback** - Valutazione tramite callback personalizzato
3. **Stato curatore** - Se curatore, restituire `AskUser` (conferma richiesta)
4. **ACL globale** - Verificare regole di controllo accesso
5. **Predefinito** - Negare se nessuna regola corrisponde

> **Nota**: A differenza della catena permessi completa, `EvaluatePermission` **non chiama** `IPermissionAskHandler`. Segnala solo quale sarà il risultato *al momento dell'esecuzione*.

## Gestire i permessi

### Concedere un permesso

**Tramite interfaccia Web**:
1. Navigare verso **Gestione permessi**
2. Cliccare su **Aggiungi regola**
3. Configurare:
   - Utente
   - Risorsa
   - Consenti/Nega
   - Durata

**Tramite API**:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "prefix": "disk:write",
    "result": "Allowed",
    "duration": 3600
  }'
```

### Revocare un permesso

Tramite l'interfaccia Web della gestione permessi.

### Consultare i permessi

```bash
curl http://localhost:8080/api/permissions/list
```

## Buone pratiche

### 1. Principio del minimo privilegio

Concedere solo i permessi minimi necessari:

```json
{
  "prefix": "disk:read",
  "result": "Allowed"
}
```

> **Nota**: Utilizzare prefissi specifici come `disk:read` anziché `disk:*` per seguire il principio del minimo privilegio.

### 2. Utilizzare permessi a durata limitata

Non concedere mai permessi permanenti salvo assoluta necessità.

### 3. Monitorare i log dei permessi

Consultare regolarmente i log di audit per identificare:
- Tentativi di accesso negati
- Pattern insoliti
- Elevazioni di permessi

### 4. Implementare callback personalizzati

Per logica complessa, utilizzare `IPermissionCallback`:

```csharp
public PermissionResult Evaluate(PermissionRequest request)
{
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Outside business hours");
    }
    
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.AskUser("Requires approval");
    }
    
    return PermissionResult.Allowed();
}
```

## Scenari comuni

### Scenario 1: L'IA vuole leggere un file

```
IA: "Devo leggere config.json"
↓
Catena permessi:
1. Cache frequenza? Nessuna voce in cache
2. Callback? Restituisce AskUser
3. IsCurator? No → GlobalACL? Regola trovata: disk:read = Consentito
4. Risultato: Consentito
```

### Scenario 2: L'IA vuole eseguire codice

```
IA: "Voglio compilare ed eseguire codice"
↓
Catena permessi:
1. Cache frequenza? Nessuna voce in cache
2. Callback? Restituisce AskUser
3. IsCurator? Sì → IPermissionAskHandler → Utente approva
4. Risultato: Consentito
```

### Scenario 3: Decisione in cache

```
IA: "Devo fare 100 richieste HTTP"
↓
Catena permessi:
1. Cache frequenza? HighDeny trovato
2. Risultato: Negato
```

## Risoluzione problemi

### Permesso inaspettatamente negato

**Verificare**:
1. Lo stato IsCurator dell'utente
2. Le voci nella cache frequenza (HighDeny ha priorità su HighAllow)
3. Le regole GlobalACL (negazione predefinita se nessuna regola corrisponde)
4. La logica del callback
5. Il timeout della risposta utente

### Permesso non scaduto

**Verificare**:
- Le voci nella cache frequenza non sono scadute
- Il fuso orario è corretto
- L'orologio è sincronizzato

### Log audit non registrati

**Verificare**:
- Il logger di audit è registrato
- Il backend di archiviazione è accessibile
- Lo spazio su disco è sufficiente

## Prossimi passi

- 📚 Leggere la [guida architettura](architecture.md)
- 🛠️ Consultare la [guida sviluppo](development-guide.md)
- 🔒 Consultare la [documentazione sicurezza](security.md)
- 🚀 Consultare la [guida avvio rapido](getting-started.md)
