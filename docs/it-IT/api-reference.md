# Riferimento API

> **Versione : v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [Français](../fr-FR/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | **Italiano**

## Endpoint dell'API Web

URL di base : `http://localhost:8080`

### Autenticazione

La maggior parte degli endpoint richiede l'autenticazione tramite cookie di sessione gestiti dall'interfaccia Web. Prima dell'inizializzazione del sistema, tutte le richieste verranno reindirizzate alla pagina di inizializzazione, ad eccezione della pagina di aiuto.

---

## Dashboard

### Ottenere le statistiche della dashboard

**GET** `/api/dashboard/stats`

Restituisce i dati di panoramica del sistema (numero di esseri, stato di esecuzione, ecc.).

### Ottenere le metriche delle prestazioni

**GET** `/api/dashboard/metrics`

Restituisce i dati delle metriche delle prestazioni in tempo reale.

---

## Sistema di chat

### Pagina della chat

**GET** `/chat`

Restituisce la pagina dell'interfaccia della chat.

### Chat in streaming (SSE)

**GET** `/api/chat/stream`

Chat in streaming tramite Server-Sent Events (SSE).

**Risposta** : Flusso Server-Sent Events

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Ottenere l'elenco delle conversazioni

**GET** `/api/chat/conversations`

Restituisce l'elenco di tutte le sessioni di chat attive.

**Esempio di risposta** :
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Chat con Assistente",
      "lastMessage": "Contenuto dell'ultimo messaggio",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### Ottenere la cronologia dei messaggi

**GET** `/api/chat/messages`

Parametro di query : `channelId` — ID del canale/sessione

Restituisce la cronologia dei messaggi della sessione specificata.

### Ottenere la cronologia chat

**GET** `/api/chat/history`

Restituisce la cronologia globale della chat.

### Inviare un messaggio

**POST** `/api/chat/send`

**Corpo della richiesta** :
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "Contenuto del messaggio di test"
}
```

**Risposta** :
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### Interrompere il pensiero dell'IA

**POST** `/api/chat/stop`

Interrompe la generazione della risposta IA in corso.

**Corpo della richiesta** :
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### Caricare un file

**POST** `/api/chat/upload`

Carica un file nella sessione di chat (supporta multipart/form-data).

---

## Gestione dei Silicon Beings

### Pagina di gestione dei Being

**GET** `/beings`

Restituisce la pagina dell'interfaccia di gestione dei Silicon Beings.

### Ottenere l'elenco dei Being

**GET** `/api/beings` o **GET** `/api/beings/list`

Restituisce l'elenco di tutti i Silicon Beings registrati.

**Esempio di risposta** :
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "status": "running",
      "soulPath": "path/to/soul.md"
    }
  ]
}
```

**Valori di stato** : `idle` | `running` | `waiting_permission` | `stopped`

### Ottenere i dettagli di un Being

**GET** `/api/beings/detail`

Parametro di query : `beingId` — ID del Silicon Being

Restituisce le informazioni dettagliate del Silicon Being specificato.

### Ottenere lo stato di attività dei Being

**GET** `/api/beings/activity`

Restituisce le informazioni sullo stato di attività di ciascun Silicon Being.

### Pagina dell'editor del file anima (soul)

**GET** `/beings/soul`

Restituisce l'interfaccia dell'editor del file anima.

### Salvare il file anima

**POST** `/api/beings/soul/save`

**Corpo della richiesta** :
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### Pagina dell'editor della configurazione IA

**GET** `/beings/ai-config`

Restituisce l'interfaccia dell'editor della configurazione IA.

### Salvare la configurazione IA

**POST** `/api/beings/ai-config/save`

**Corpo della richiesta** :
```json
{
  "beingId": "being-uuid",
  "aiClientType": "DashScope",
  "config": {
    "apiKey": "...",
    "region": "beijing",
    "model": "qwen3.6-plus"
  }
}
```

### Ottenere l'elenco dei modelli IA disponibili

**GET** `/api/beings/ai-config/models`

Parametri di query : `clientType`, `apiKey`, `region`

Restituisce l'elenco dei modelli disponibili per il client IA specificato.

---

## Visualizzazione cronologia chat

### Pagina della cronologia chat

**GET** `/chat-history`

Restituisce la pagina principale della cronologia chat.

### Pagina dei dettagli della cronologia chat

**GET** `/chat-history-detail`

Restituisce la pagina dei dettagli della cronologia chat per una sessione specificata.

### Pagina dei dettagli della cronologia chat di gruppo

**GET** `/group-chat-history-detail`

Restituisce la pagina dei dettagli della cronologia della chat di gruppo.

### Pagina dei dettagli della cronologia broadcast

**GET** `/broadcast-history-detail`

Restituisce la pagina dei dettagli della cronologia del canale broadcast.

### Ottenere l'elenco delle sessioni storiche

**GET** `/api/chat-history/conversations`

Restituisce l'elenco di tutte le sessioni storiche.

### Ottenere i messaggi storici

**GET** `/api/chat-history/messages`

Parametro di query : `sessionId` — ID della sessione

Restituisce i record dei messaggi della sessione storica specificata.

---

## Gestione dei timer

### Pagina dei timer

**GET** `/timers`

Restituisce la pagina dell'interfaccia di gestione dei timer.

### Ottenere l'elenco dei timer

**GET** `/api/timers/list`

Restituisce l'elenco di tutti i timer.

### Pagina dei dettagli dei cicli del timer

**GET** `/timer-cycles/{timerId}`

Restituisce la pagina dei dettagli dei cicli di esecuzione del timer specificato.

### Ottenere l'elenco dei cicli del timer

**GET** `/api/timer-cycles/list`

Parametro di query : `timerId` — ID del timer

Restituisce l'elenco di tutti i cicli di esecuzione del timer specificato.

### Pagina dei dettagli di un singolo ciclo di esecuzione

**GET** `/timer-cycle/{cycleIndex}`

Restituisce la pagina dettagliata di una singola esecuzione.

### Ottenere i messaggi del ciclo

**GET** `/api/timer-cycle/messages`

Parametro di query : `cycleIndex` — Indice del ciclo

Restituisce i messaggi relativi al ciclo di esecuzione specificato.

---

## Gestione dei compiti

### Pagina dei compiti

**GET** `/tasks`

Restituisce la pagina dell'interfaccia di gestione dei compiti.

### Ottenere l'elenco dei compiti

**GET** `/api/tasks/list`

Restituisce l'elenco di tutti i compiti.

### Pagina dei dettagli dei cicli dei compiti

**GET** `/task-cycles/{taskId}`

Restituisce la pagina dei dettagli dei cicli di esecuzione del compito specificato.

### Ottenere l'elenco dei cicli dei compiti

**GET** `/api/task-cycles/list`

Parametro di query : `taskId` — ID del compito

Restituisce l'elenco di tutti i cicli di esecuzione del compito specificato.

### Pagina dei dettagli di un singolo ciclo di esecuzione

**GET** `/task-cycle/{cycleIndex}`

Restituisce la pagina dettagliata di una singola esecuzione del compito.

### Ottenere i messaggi del ciclo

**GET** `/api/task-cycle/messages`

Parametro di query : `cycleIndex` — Indice del ciclo

Restituisce i messaggi relativi al ciclo di esecuzione del compito specificato.

---

## Sistema di permessi

### Pagina di gestione dei permessi

**GET** `/permissions`

Restituisce la pagina dell'interfaccia di gestione dei permessi.

### Ottenere l'elenco delle regole di permesso

**GET** `/api/permissions/list`

Restituisce tutte le regole di permesso attualmente configurate.

**Esempio di risposta** :
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

### Salvare una regola di permesso

**POST** `/api/permissions/save`

**Corpo della richiesta** :
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### Pagina di richiesta di permesso

**GET** `/permission/request`

Mostra la pagina di richiesta di permesso che permette agli utenti di approvare o rifiutare le richieste di permesso dei Silicon Beings.

**Parametri di query** :

| Parametro | Tipo | Descrizione |
|-----------|------|-------------|
| `userId` | `Guid` | ID del Silicon Being che richiede il permesso |
| `type` | `string` | Tipo di permesso |
| `resource` | `string` | Percorso della risorsa richiesta |
| `allowCode` | `string` | ID codice per l'operazione di autorizzazione |
| `denyCode` | `string` | ID codice per l'operazione di rifiuto |

### Verificare le richieste di permesso in attesa

**GET** `/permission/check`

Parametro di query : `userId` — ID del Silicon Being

**Risposta** :
```json
{
  "pending": true
}
```

### Rispondere a una richiesta di permesso

**GET** `/permission/respond`

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

---

## Sistema di log

### Pagina dei log

**GET** `/logs`

Restituisce la pagina dell'interfaccia di visualizzazione dei log.

### Ottenere l'elenco dei log

**GET** `/api/logs/list`

I parametri di query supportano il filtro per livello e intervallo di tempo.

**Esempio di risposta** :
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

### Ottenere i log raggruppati per Being

**GET** `/api/logs/beings`

Statistiche dei log raggruppate per Silicon Being.

### Ottenere i livelli di log disponibili

**GET** `/api/logs/levels`

Restituisce l'elenco dei livelli di log disponibili nel sistema.

---

## Statistiche di utilizzo

### Pagina delle statistiche di utilizzo

**GET** `/usage`

Restituisce la pagina dell'interfaccia delle statistiche di utilizzo.

### Ottenere il riepilogo dell'utilizzo

**GET** `/api/usage/summary`

Restituisce il riepilogo dell'utilizzo dei token e dei costi.

### Ottenere i dati di tendenza

**GET** `/api/usage/trend`

Parametri di query : `startDate`, `endDate`

Restituisce i dati di tendenza dell'utilizzo per il periodo specificato.

### Esportare i dati di utilizzo

**GET** `/api/usage/export`

Esporta i dati di utilizzo in un formato scaricabile.

---

## Audit trail

### Pagina di audit

**GET** `/audit`

Restituisce la pagina dell'interfaccia dell'audit trail.

### Ottenere l'elenco di audit

**GET** `/api/audit/list`

Restituisce l'elenco delle voci del log di audit.

### Ottenere il riepilogo di audit

**GET** `/api/audit/summary`

Restituisce le statistiche di riepilogo dei dati di audit.

### Ottenere l'audit raggruppato per Being

**GET** `/api/audit/beings`

Statistiche di audit raggruppate per Silicon Being.

---

## Gestione della configurazione

### Pagina della configurazione

**GET** `/config`

Restituisce la pagina dell'interfaccia di configurazione del sistema.

### Salvare la configurazione

**POST** `/config/save`

**Corpo della richiesta** :
```json
{
  "language": "ZhCN",
  "port": 8080,
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    },
    "DashScope": {
      "apiKey": "...",
      "region": "beijing",
      "model": "qwen3.6-plus"
    }
  }
}
```

### Ottenere le opzioni di configurazione IA

**GET** `/config/aioptions`

Restituisce i tipi di client IA disponibili e le relative opzioni dinamiche (modelli disponibili, regioni, ecc.).

---

## Sistema di memoria

### Pagina della memoria

**GET** `/memory`

Restituisce la pagina dell'interfaccia di gestione della memoria.

### Ottenere l'elenco delle memorie

**GET** `/api/memory/list`

Restituisce l'elenco delle voci di memoria dei Silicon Beings.

### Ottenere i dettagli di una memoria

**GET** `/api/memory/detail/{id}`

Parametro di percorso : `id` — ID della voce di memoria

Restituisce il contenuto completo della voce di memoria specificata.

### Ottenere le statistiche della memoria

**GET** `/api/memory/stats`

Restituisce le informazioni statistiche del sistema di memoria.

### Cercare nella memoria

**GET** `/api/memory/search`

Parametro di query : `keyword` — Parola chiave di ricerca

Cerca le voci di memoria corrispondenti.

### Ottenere le memorie raggruppate per Being

**GET** `/api/memory/beings`

Statistiche della memoria raggruppate per Silicon Being.

### Ottenere la tracciabilità della memoria

**GET** `/api/memory/trace/{id}`

Parametro di percorso : `id` — ID della voce di memoria

Restituisce la catena di tracciabilità delle origini della voce di memoria specificata.

### Ottenere la timeline della memoria in HTML

**GET** `/api/memory/timeline-html`

Restituisce la vista HTML della timeline della memoria.

---

## Note di lavoro

### Pagina delle note di lavoro

**GET** `/work-notes`

Restituisce la pagina dell'interfaccia delle note di lavoro.

### Ottenere l'elenco delle note di lavoro

**GET** `/api/work-notes/list`

Restituisce l'elenco delle note di lavoro.

### Leggere una nota di lavoro

**GET** `/api/work-notes/read`

Parametro di query : `noteId` — ID della nota

Restituisce il contenuto della nota specificata.

### Ottenere la directory delle note

**GET** `/api/work-notes/directory`

Restituisce la struttura della directory delle note.

### Cercare nelle note di lavoro

**GET** `/api/work-notes/search`

Parametro di query : `keyword` — Parola chiave di ricerca

Cerca le note di lavoro corrispondenti.

### Creare una nota di lavoro

**POST** `/api/work-notes/create`

**Corpo della richiesta** :
```json
{
  "title": "Titolo della nota",
  "content": "Contenuto della nota",
  "keywords": ["parola1", "parola2"]
}
```

### Aggiornare una nota di lavoro

**POST** `/api/work-notes/update`

**Corpo della richiesta** :
```json
{
  "noteId": "note-uuid",
  "title": "Titolo aggiornato",
  "content": "Contenuto aggiornato"
}
```

### Eliminare una nota di lavoro

**POST** `/api/work-notes/delete`

**Corpo della richiesta** :
```json
{
  "noteId": "note-uuid"
}
```

---

## Rete di conoscenze

### Pagina della rete di conoscenze

**GET** `/knowledge`

Restituisce la pagina dell'interfaccia di gestione della rete di conoscenze.

### Ottenere il grafo delle conoscenze

**GET** `/api/knowledge/graph`

Restituisce i dati del grafo delle triple di conoscenza (soggetto-relazione-oggetto).

---

## Gestione dei progetti

### Pagina dei progetti

**GET** `/project`

Restituisce la pagina dell'interfaccia di gestione dei progetti.

### Pagina delle note di lavoro del progetto

**GET** `/project/{id}/work-notes`

Parametro di percorso : `id` — ID del progetto

Restituisce la pagina delle note di lavoro del progetto specificato.

### Pagina dei compiti del progetto

**GET** `/project/{id}/tasks`

Parametro di percorso : `id` — ID del progetto

Restituisce la pagina di gestione dei compiti del progetto specificato.

### Ottenere l'elenco dei progetti

**GET** `/api/projects/list`

Restituisce l'elenco di tutti i progetti.

### Ottenere l'elenco dei modelli di flusso di lavoro

**GET** `/api/projects/list-workflow-templates`

Restituisce l'elenco dei modelli di flusso di lavoro disponibili.

### Creare un progetto

**POST** `/api/projects/create`

**Corpo della richiesta** :
```json
{
  "name": "Il Mio Progetto",
  "description": "Descrizione del progetto"
}
```

### Archiviare un progetto

**POST** `/api/projects/{id}/archive`

Parametro di percorso : `id` — ID del progetto

Archivia il progetto specificato.

### Ripristinare un progetto

**POST** `/api/projects/{id}/restore`

Parametro di percorso : `id` — ID del progetto

Ripristina un progetto precedentemente archiviato.

### Distruggere un progetto

**POST** `/api/projects/{id}/destroy`

Parametro di percorso : `id` — ID del progetto

Elimina definitivamente il progetto specificato (irreversibile).

### Ottenere i dettagli di un progetto

**GET** `/api/projects/detail`

Parametro di query : `projectId` — ID del progetto

Restituisce le informazioni dettagliate del progetto.

### Aggiornare un progetto

**POST** `/api/projects/update`

**Corpo della richiesta** :
```json
{
  "projectId": "project-uuid",
  "name": "Nome aggiornato",
  "description": "Descrizione aggiornata"
}
```

### Assegnare un membro a un progetto

**POST** `/api/projects/assign`

**Corpo della richiesta** :
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Rimuovere un membro da un progetto

**POST** `/api/projects/remove`

**Corpo della richiesta** :
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Ottenere l'elenco delle note di lavoro del progetto

**GET** `/api/projects/{id}/work-notes/list`

Parametro di percorso : `id` — ID del progetto

Restituisce l'elenco delle note di lavoro del progetto specificato.

### Leggere una nota di lavoro del progetto

**GET** `/api/projects/{id}/work-notes/read`

Parametro di percorso : `id` — ID del progetto

Restituisce il contenuto della nota di lavoro del progetto specificato.

### Creare una nota di lavoro del progetto

**POST** `/api/projects/{id}/work-notes/create`

Parametro di percorso : `id` — ID del progetto

Crea una nuova nota di lavoro nel progetto specificato.

### Aggiornare una nota di lavoro del progetto

**POST** `/api/projects/{id}/work-notes/update`

Parametro di percorso : `id` — ID del progetto

Aggiorna una nota di lavoro nel progetto specificato.

### Eliminare una nota di lavoro del progetto

**POST** `/api/projects/{id}/work-notes/delete`

Parametro di percorso : `id` — ID del progetto

Elimina una nota di lavoro dal progetto specificato.

### Ottenere l'elenco dei compiti del progetto

**GET** `/api/projects/{id}/tasks/list`

Parametro di percorso : `id` — ID del progetto

Restituisce l'elenco dei compiti del progetto specificato.

### Creare un compito del progetto

**POST** `/api/projects/{id}/tasks/create`

Parametro di percorso : `id` — ID del progetto

Crea un nuovo compito nel progetto specificato.

### Aggiornare un compito del progetto

**POST** `/api/projects/{id}/tasks/update`

Parametro di percorso : `id` — ID del progetto

Aggiorna un compito nel progetto specificato.

### Eliminare un compito del progetto

**POST** `/api/projects/{id}/tasks/delete`

Parametro di percorso : `id` — ID del progetto

Elimina un compito dal progetto specificato.

### Assegnare un responsabile al compito

**POST** `/api/projects/{id}/tasks/assign`

Parametro di percorso : `id` — ID del progetto

Assegna un responsabile al compito del progetto.

### Rimuovere il responsabile del compito

**POST** `/api/projects/{id}/tasks/remove-assignee`

Parametro di percorso : `id` — ID del progetto

Rimuove il responsabile del compito del progetto.

### Contrassegnare un compito come completato

**POST** `/api/projects/{id}/tasks/complete`

Parametro di percorso : `id` — ID del progetto

Contrassegna il compito del progetto come completato.

### Contrassegnare un compito come fallito

**POST** `/api/projects/{id}/tasks/fail`

Parametro di percorso : `id` — ID del progetto

Contrassegna il compito del progetto come fallito.

### Annullare un compito

**POST** `/api/projects/{id}/tasks/cancel`

Parametro di percorso : `id` — ID del progetto

Annulla il compito del progetto.

---

## Gestione degli esecutori

### Pagina degli esecutori

**GET** `/executor`

Restituisce la pagina dell'interfaccia di gestione degli esecutori.

### Ottenere lo stato degli esecutori

**GET** `/api/executors/status`

Restituisce lo stato di esecuzione di ciascun esecutore (disco, rete, riga di comando).

---

## Browser del codice

### Pagina del browser del codice

**GET** `/code`

Restituisce la pagina dell'interfaccia del browser del codice.

### Ottenere l'elenco dei tipi di codice

**GET** `/api/code/types`

Restituisce l'elenco dei tipi/linguaggi di codice supportati.

### Ottenere i dettagli del codice

**GET** `/api/code/detail`

Parametri di query : `filePath`, `lineNumber`

Restituisce i dettagli del codice del file specificato.

---

## Suggerimenti del codice (hover)

### Ottenere suggerimenti hover

**GET** `/api/code/hover`
**POST** `/api/code/hover`

Ottiene le informazioni di suggerimento hover per la posizione del codice (simile agli smart hint degli IDE).

### Registrare una posizione del codice

**POST** `/api/code/register`

Registra le posizioni del codice da monitorare.

### Aggiornare una posizione del codice

**POST** `/api/code/update`

Aggiorna le informazioni delle posizioni del codice registrate.

### Deregistrare una posizione del codice

**POST** `/api/code/unregister`

Deregistra il monitoraggio delle posizioni del codice non più necessarie.

---

## Sistema di documentazione di aiuto

### Pagina di aiuto

**GET** `/help` o **GET** `/help/index`

Restituisce la pagina principale della documentazione di aiuto.

### Pagina di un argomento di aiuto

**GET** `/help/{topic}`

Parametro di percorso : `topic` — Identificatore dell'argomento

Restituisce la pagina della documentazione di aiuto per l'argomento specificato.

### Cercare nella documentazione di aiuto

**GET** `/api/help/search`

Parametro di query : `keyword` — Parola chiave di ricerca

Cerca gli argomenti della documentazione di aiuto corrispondenti.

---

## Inizializzazione

### Pagina della procedura guidata di inizializzazione

**GET** `/init`

Restituisce la pagina della procedura guidata di inizializzazione per la prima esecuzione.

### Inviare l'inizializzazione

**POST** `/init`

Invia la configurazione di inizializzazione per la prima esecuzione.

### Sfogliare per selezionare la directory dei dati

**GET** `/init/browse`

Apre il browser delle directory per selezionare la posizione di archiviazione dei dati.

### Ottenere i metadati della configurazione IA

**GET** `/init/ai-config-metadata`

Restituisce i tipi di client IA disponibili e i metadati dei campi di configurazione.

---

## Controllo del sistema

### Spegnimento elegante

**POST** `/api/system/shutdown`

> **Nota** : Sono consentite solo le richieste provenienti da localhost

Attiva la procedura di spegnimento elegante dell'applicazione :

1. Ferma il ciclo principale (MainLoop)
2. Salva la configurazione corrente
3. Chiude il listener HTTP

**Risposta** :
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## Informazioni

### Pagina Informazioni

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
const eventSource = new EventSource('/api/chat/stream');

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

---

## Interfaccia del client IA

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

## Interfaccia del sistema di strumenti

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
