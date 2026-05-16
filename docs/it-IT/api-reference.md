# Riferimento API

> **Versione : v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [Français](../fr-FR/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | **Italiano**

## Endpoint dell'API Web

URL di base : `http://localhost:8080`

### Autenticazione

La maggior parte degli endpoint richiede l'autenticazione tramite cookie di sessione gestiti dall'interfaccia Web.

---

## Gestione dei Silicon Beings

### Ottenere tutti i Being

**GET** `/api/beings`

**Risposta** :
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistente",
      "status": "running",
      "soul": "path/to/soul.md"
    }
  ]
}
```

**Valori di stato** : `idle` | `running` | `waiting_permission` | `stopped`

### Creare un Being

**POST** `/api/beings`

**Richiesta** :
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**Risposta** : `201 Created`

### Avviare un Being

**POST** `/api/beings/{id}/start`

### Fermare un Being

**POST** `/api/beings/{id}/stop`

### Ottenere i dettagli di un Being

**GET** `/api/beings/{id}`

---

## Sistema di chat

### Inviare un messaggio

**POST** `/api/chat`

**Richiesta** :
```json
{
  "beingId": "being-uuid",
  "message": "Ciao, come stai ?",
  "sessionId": "optional-session-id"
}
```

**Risposta** (non in streaming) :
```json
{
  "reply": "Sto bene, grazie !",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Chat in streaming (SSE)

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**Risposta** : Flusso Server-Sent Events

```
data: {"type": "chunk", "content": "Sto"}
data: {"type": "chunk", "content": " riflettendo"}
data: {"type": "chunk", "content": " bene..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Ottenere la cronologia chat

**GET** `/api/chat/{sessionId}/history`

**Risposta** :
```json
{
  "messages": [
    {
      "role": "user",
      "content": "Ciao",
      "timestamp": "2026-04-20T10:30:00Z"
    },
    {
      "role": "assistant",
      "content": "Salve !",
      "timestamp": "2026-04-20T10:30:05Z"
    }
  ]
}
```

---

## Configurazione

### Ottenere la configurazione

**GET** `/api/config`

**Risposta** :
```json
{
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    }
  },
  "storage": {
    "basePath": "./data"
  }
}
```

### Aggiornare la configurazione

**POST** `/api/config`

**Richiesta** :
```json
{
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:14b"
    }
  }
}
```

---

## Sistema di permessi

### Ottenere i permessi

**GET** `/api/permissions`

**Risposta** :
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

### Concedere un permesso

**POST** `/api/permissions`

**Richiesta** :
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### Revocare un permesso

**DELETE** `/api/permissions/{id}`

### Verificare un permesso

**POST** `/api/permissions/check`

**Richiesta** :
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**Risposta** :
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

---

## Sistema di compiti e timer

### Creare un compito

**POST** `/api/tasks`

**Richiesta** :
```json
{
  "beingId": "being-uuid",
  "description": "Revisionare il codice",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### Ottenere i compiti

**GET** `/api/tasks?beingId={id}&status=pending`

### Aggiornare lo stato di un compito

**PATCH** `/api/tasks/{id}`

**Richiesta** :
```json
{
  "status": "completed"
}
```

### Creare un timer

**POST** `/api/timers`

**Richiesta** :
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### Eliminare un timer

**DELETE** `/api/timers/{id}`

---

## Audit e logging

### Ottenere l'utilizzo dei token

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**Risposta** :
```json
{
  "summary": {
    "totalTokens": 150000,
    "promptTokens": 100000,
    "completionTokens": 50000,
    "totalCost": 0.15
  },
  "byModel": {
    "qwen2.5:7b": {
      "tokens": 100000,
      "cost": 0.10
    }
  }
}
```

### Ottenere i log

**GET** `/api/logs?level=error&limit=100`

**Risposta** :
```json
{
  "logs": [
    {
      "timestamp": "2026-04-20T10:30:00Z",
      "level": "error",
      "message": "Connessione al servizio IA fallita",
      "source": "OllamaClient"
    }
  ]
}
```

---

## API di storage

### Leggere un valore

**GET** `/api/storage?key={key}`

**Risposta** :
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Scrivere un valore

**POST** `/api/storage`

**Richiesta** :
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### Query per intervallo temporale

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**Risposta** :
```json
{
  "entries": [
    {
      "key": "being:uuid:chat:2026-04-20",
      "value": "{...}",
      "timestamp": "2026-04-20T10:30:00Z"
    }
  ]
}
```

---

## Informazioni di sistema

### Ottenere la pagina Informazioni

**GET** `/about`

Restituisce la pagina Informazioni con le informazioni di sistema e la lista dei plugin caricati.

**Dati della lista plugin** :
```json
{
  "plugins": {
    "plugin-id": {
      "name": "Il Mio Plugin",
      "version": "1.0.0",
      "description": "Descrizione del plugin",
      "author": "Nome dell'autore"
    }
  }
}
```

### Richiesta di permesso

**GET** `/permission/request?userId={id}&type={type}&resource={resource}`

Mostra la pagina di richiesta di permesso che permette agli utenti di approvare o rifiutare le richieste di permesso dei Silicon Beings.

**Parametri di query** :

| Parametro | Tipo | Descrizione |
|-----------|------|-------------|
| `userId` | `Guid` | ID del Silicon Being che richiede il permesso |
| `type` | `string` | Tipo di permesso |
| `resource` | `string` | Percorso della risorsa richiesta |
| `allowCode` | `string` | ID codice per l'operazione di autorizzazione |
| `denyCode` | `string` | ID codice per l'operazione di rifiuto |

**Verificare le richieste di permesso in attesa** :

**GET** `/permission/check?userId={id}`

**Risposta** :
```json
{
  "pending": true
}
```

**Rispondere a una richiesta di permesso** :

**GET** `/permission/respond?userId={id}&allowed={bool}&addToCache={bool}&cacheDuration={hours}`

**Parametri di query** :

| Parametro | Tipo | Descrizione |
|-----------|------|-------------|
| `userId` | `Guid` | ID del Silicon Being |
| `allowed` | `bool` | Se autorizzato |
| `addToCache` | `bool` | Se la decisione deve essere memorizzata nella cache |
| `cacheDuration` | `double` | Durata della cache (ore) |

**Risposta** :
```json
{
  "success": true
}
```

### Ottenere i dati della dashboard

**GET** `/api/dashboard`

**Risposta** :
```json
{
  "beings": {
    "total": 5,
    "running": 3,
    "stopped": 2
  },
  "performance": {
    "cpu": 45.2,
    "memory": 1024,
    "uptime": 86400
  },
  "aiUsage": {
    "todayTokens": 50000,
    "todayCost": 0.05
  }
}
```

### Ottenere lo stato del sistema

**GET** `/api/status`

**Risposta** :
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
}
```

---

## Risposte di errore

Tutti gli endpoint restituiscono risposte di errore standardizzate :

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "Non hai il permesso di accedere a questa risorsa",
    "details": "Richiesto : disk:write, Attuale : disk:read"
  }
}
```

### Codici di errore comuni

| Codice | Stato HTTP | Descrizione |
|--------|------------|-------------|
| `PERMISSION_DENIED` | 403 | Permessi insufficienti |
| `NOT_FOUND` | 404 | Risorsa non trovata |
| `VALIDATION_ERROR` | 400 | Parametri di richiesta non validi |
| `INTERNAL_ERROR` | 500 | Errore interno del server |
| `SERVICE_UNAVAILABLE` | 503 | Servizio IA non disponibile |

---

## Eventi SSE

Server-Sent Events per aggiornamenti in tempo reale :

### Eventi chat

```javascript
const eventSource = new EventSource('/api/chat/stream?beingId=xxx&message=xxx');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);
  
  switch(data.type) {
    case 'chunk':
      console.log('Streaming:', data.content);
      break;
    case 'tool_call':
      console.log('Strumento in esecuzione:', data.tool);
      break;
    case 'complete':
      console.log('Chat completata, sessione:', data.sessionId);
      break;
    case 'error':
      console.error('Errore:', data.message);
      break;
  }
};
```

### Eventi di stato dei Being

```javascript
const beingEvents = new EventSource('/api/beings/events');

beingEvents.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(`Being ${data.beingId} stato : ${data.status}`);
};
```

---

## API del client IA

### Interfaccia IAIClient

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### Struttura AIRequest

```csharp
public class AIRequest
{
    public List<Message> Messages { get; set; }
    public List<ToolDefinition> Tools { get; set; }
    public double Temperature { get; set; } = 0.7;
    public int MaxTokens { get; set; } = 2000;
    public string Model { get; set; }
}
```

### Struttura AIResponse

```csharp
public class AIResponse
{
    public string Content { get; set; }
    public List<ToolCall> ToolCalls { get; set; }
    public TokenUsage Usage { get; set; }
    public string Model { get; set; }
}
```

---

## API delle note di lavoro

### Ottenere la lista delle note di lavoro

**GET** `/api/beings/{id}/work-notes`

**Risposta** :
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "Modulo autenticazione utente completato",
      "keywords": ["autenticazione", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### Ottenere i dettagli di una nota

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**Risposta** :
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "Modulo autenticazione utente completato",
  "content": "## Dettagli implementazione\n\n- Utilizzo di JWT token\n- Supporto OAuth2",
  "keywords": ["autenticazione", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### Creare una nuova nota

**POST** `/api/beings/{id}/work-notes`

**Richiesta** :
```json
{
  "summary": "Modulo autenticazione utente completato",
  "content": "## Dettagli implementazione\n\n- Utilizzo di JWT token",
  "keywords": "autenticazione,JWT,OAuth2"
}
```

**Risposta** : `201 Created`

### Aggiornare una nota

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**Richiesta** :
```json
{
  "summary": "Modulo autenticazione utente e test completati",
  "content": "## Contenuto aggiornato\n\nAggiunta test unitari",
  "keywords": "autenticazione,JWT,OAuth2,test"
}
```

### Eliminare una nota

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### Cercare note

**GET** `/api/beings/{id}/work-notes/search?keyword=autenticazione&maxResults=10`

### Ottenere la directory delle note

**GET** `/api/beings/{id}/work-notes/directory`

---

## API della rete di conoscenze

### Ottenere le statistiche delle conoscenze

**GET** `/api/knowledge/stats`

**Risposta** :
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Aggiungere una tripletta di conoscenza

**POST** `/api/knowledge/triples`

**Richiesta** :
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**Risposta** : `201 Created`

### Interrogare le conoscenze

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**Risposta** :
```json
{
  "triples": [
    {
      "subject": "Python",
      "predicate": "is_a",
      "object": "programming_language",
      "confidence": 0.95,
      "tags": ["programming", "language"]
    }
  ]
}
```

### Cercare conoscenze

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### Ottenere un percorso di conoscenza

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**Risposta** :
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### Validare una conoscenza

**POST** `/api/knowledge/validate`

**Richiesta** :
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### Eliminare una conoscenza

**DELETE** `/api/knowledge/triples/{id}`

---

## API di documentazione di aiuto

### Ottenere la lista della documentazione di aiuto

**GET** `/api/help`

**Risposta** :
```json
{
  "topics": [
    {
      "id": "getting-started",
      "title": "Guida rapida",
      "category": "Guida introduttiva"
    }
  ]
}
```

### Ottenere i dettagli di un argomento di aiuto

**GET** `/api/help/{topicId}`

**Risposta** :
```json
{
  "id": "getting-started",
  "title": "Guida rapida",
  "content": "# Guida rapida\n\n...",
  "category": "Guida introduttiva"
}
```

---

## API del browser WebView

### Ottenere lo stato del browser

**GET** `/api/beings/{id}/browser/status`

**Risposta** :
```json
{
  "is_open": true,
  "current_url": "https://example.com",
  "page_title": "Example Page",
  "is_loading": false,
  "last_operation_time": "2026-04-26T10:00:00Z"
}
```

### Aprire il browser

**POST** `/api/beings/{id}/browser/open`

### Chiudere il browser

**POST** `/api/beings/{id}/browser/close`

### Navigare verso un URL

**POST** `/api/beings/{id}/browser/navigate`

**Richiesta** :
```json
{
  "url": "https://example.com"
}
```

### Eseguire JavaScript

**POST** `/api/beings/{id}/browser/execute-script`

**Richiesta** :
```json
{
  "script": "return document.title;"
}
```

### Ottenere uno screenshot della pagina

**GET** `/api/beings/{id}/browser/screenshot`

---

## API dello spazio di progetto

### Ottenere la lista dei progetti

**GET** `/api/projects`

**Risposta** :
```json
{
  "projects": [
    {
      "id": "project-uuid",
      "name": "Il Mio Progetto",
      "description": "Descrizione del progetto",
      "createdAt": "2026-04-25T10:00:00Z"
    }
  ]
}
```

### Creare un progetto

**POST** `/api/projects`

**Richiesta** :
```json
{
  "name": "Il Mio Progetto",
  "description": "Descrizione del progetto"
}
```

### Ottenere i dettagli di un progetto

**GET** `/api/projects/{id}`

### Aggiornare un progetto

**PUT** `/api/projects/{id}`

### Eliminare un progetto

**DELETE** `/api/projects/{id}`

---

## API del sistema di strumenti

### Interfaccia ITool

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### Struttura ToolCall

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### Struttura ToolResult

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## Prossimi passi

- 🚀 Vedere la [guida rapida](getting-started.md)
- 🛠️ Leggere la [guida di sviluppo](development-guide.md)
- 📚 Consultare la [documentazione di architettura](architecture.md)
- 🔒 Comprendere il [modello di sicurezza](security.md)
