# Sistema di permessi

> **Versione: v0.1.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [Français](../fr-FR/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | **Italiano**

## Panoramica

Il sistema di permessi garantisce che tutte le operazioni avviate dall'IA siano correttamente verificate e sottoposte ad audit.

## Catena di permessi a 5 livelli

```
┌─────────────────────────────────────────────┐
│          Verifica dei permessi              │
├─────────────────────────────────────────────┤
│  Livello 1 : IsCurator                       │
│  ↓ Se vero, bypass                          │
│  Livello 2 : UserFrequencyCache              │
│  ↓ Limitazione della frequenza              │
│  Livello 3 : GlobalACL                       │
│  ↓ Lista di controllo accessi               │
│  Livello 4 : IPermissionCallback             │
│  ↓ Logica personalizzata                     │
│  Livello 5 : IPermissionAskHandler           │
│  ↓ Chiedi all'utente                        │
│  Risultato : Consentito o Negato             │
└─────────────────────────────────────────────┘
```

## Livello 1: IsCurator

Gli amministratori/curatori bypassano tutte le verifiche dei permessi.

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Curator access");
}
```

## Livello 2: UserFrequencyCache

Limitazione della frequenza per utente per prevenire abusi.

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## Livello 3: GlobalACL

La lista di controllo accessi globale definisce regole esplicite.

### Struttura ACL

```json
{
  "rules": [
    {
      "userId": "user-uuid",
      "resource": "disk:read",
      "allowed": true,
      "expiresAt": "2026-04-21T00:00:00Z"
    }
  ]
}
```

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

## Livello 4: IPermissionCallback

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
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // Logica personalizzata
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }
        
        return PermissionResult.Undecided("Needs user confirmation");
    }
}
```

## Livello 5: IPermissionAskHandler

Chiedere il permesso all'utente quando tutti gli altri livelli sono indecisi.

### Implementazione IMPermissionAskHandler

`IMPermissionAskHandler` invia richieste di permesso all'utente tramite l'interfaccia Web:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // Inviare un messaggio all'utente tramite messaggistica istantanea
        await SendMessageAsync($"Consentire {request.Resource}?");
        
        // Attendere la risposta dell'utente
        var response = await WaitForResponseAsync();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### Coda richieste permessi PermissionRequestQueue

`PermissionRequestQueue` gestisce le richieste di permessi in attesa, supportando l'attesa asincrona delle risposte utente:

- **Accodamento** — Quando la catena permessi raggiunge il livello 5, crea un `TaskCompletionSource<AskPermissionResult>` e lo accoda
- **Visualizzazione Web UI** — Mostra le richieste di permesso in attesa tramite `PermissionRequestController` nell'interfaccia Web
- **Risposta utente** — L'utente approva o rifiuta nell'interfaccia Web, con possibilità di mettere in cache la decisione e impostare la durata della cache
- **Opzioni cache** — L'utente può memorizzare nella cache la decisione del permesso per 1 ora, 24 ore, 7 giorni o 30 giorni
- **Meccanismo timeout** — Chiusura automatica della pagina di richiesta dopo 60 secondi senza risposta

## Sistema di audit

Tutte le decisioni sui permessi vengono registrate:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "level": "GlobalACL",
  "reason": "Explicit rule granted"
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
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### Revocare un permesso

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### Consultare i permessi

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## Buone pratiche

### 1. Principio del minimo privilegio

Concedere solo i permessi minimi necessari:

```json
{
  "resource": "disk:read",  // Non disk:*
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // Impostare sempre una scadenza
}
```

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
public async Task<PermissionResult> CheckAsync(PermissionRequest request)
{
    // Permessi basati sul tempo
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Outside business hours");
    }
    
    // Permessi basati sulle risorse
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.Undecided("Requires approval");
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
1. IsCurator? No
2. Limitazione frequenza? Normale
3. GlobalACL? Regola trovata: disk:read = Consentito
4. Risultato: Consentito
```

### Scenario 2: L'IA vuole eseguire codice

```
IA: "Voglio compilare ed eseguire codice"
↓
Catena permessi:
1. IsCurator? No
2. Limitazione frequenza? Normale
3. GlobalACL? Nessuna regola trovata
4. Callback? Restituisce Indeciso
5. Chiedi all'utente? Utente approva
6. Risultato: Consentito
```

### Scenario 3: Superamento limite frequenza

```
IA: "Devo fare 100 richieste HTTP"
↓
Catena permessi:
1. IsCurator? No
2. Limitazione frequenza? Superata
3. Risultato: Negato
```

## Risoluzione problemi

### Permesso inaspettatamente negato

**Verificare**:
1. Lo stato IsCurator dell'utente
2. Le impostazioni di limitazione frequenza
3. Le regole GlobalACL
4. La logica del callback
5. Il timeout della risposta utente

### Permesso non scaduto

**Verificare**:
- Il campo `expiresAt` è correttamente impostato
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
