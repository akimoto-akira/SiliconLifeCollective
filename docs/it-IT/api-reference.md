# Riferimento API

> **Versione: v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md) | **Italiano**

## Endpoint Web API

URL di base: `http://localhost:8080`

### Autenticazione

La maggior parte degli endpoint richiede l'autenticazione tramite cookie di sessione gestiti dalla Web UI. Prima dell'inizializzazione del sistema, tutte le richieste, ad eccezione della pagina di aiuto, verranno reindirizzate alla pagina di inizializzazione.

---

## Dashboard

### Ottieni Statistiche Dashboard

**GET** `/api/dashboard/stats`

Restituisce dati di panoramica del sistema (numero di esseri, stato di esecuzione, ecc.).

### Ottieni Metriche delle Prestazioni

**GET** `/api/dashboard/metrics`

Restituisce dati delle metriche delle prestazioni in tempo reale.

---

## Sistema di Chat

### Pagina Chat

**GET** `/chat`

Restituisce la pagina dell'interfaccia di chat.

### Chat in Streaming (SSE)

**GET** `/api/chat/stream`

Chat in streaming tramite Server-Sent Events (SSE).

**Risposta**: Flusso di eventi inviati dal server

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Ottieni Elenco delle Conversazioni

**GET** `/api/chat/conversations`

Restituisce l'elenco di tutte le sessioni di chat attive.

**Esempio di risposta**:
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Chat con XiaoYou",
      "lastMessage": "Contenuto dell'ultimo messaggio",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### Ottieni Cronologia dei Messaggi

**GET** `/api/chat/messages`

Parametro di query: `channelId` — ID canale/sessione

Restituisce la cronologia dei messaggi della sessione specificata.

### Ottieni Cronologia Chat

**GET** `/api/chat/history`

Restituisce la cronologia globale delle chat.

### Invia Messaggio

**POST** `/api/chat/send`

**Corpo della richiesta**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "Contenuto del messaggio di test"
}
```

**Risposta**:
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### Ferma il Ragionamento dell'AI

**POST** `/api/chat/stop`

Ferma la generazione della risposta AI in corso.

**Corpo della richiesta**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### Carica File

**POST** `/api/chat/upload`

Carica un file nella sessione di chat (supporta multipart/form-data).

---

## Gestione degli Esseri di Silicio

### Pagina Gestione Esseri

**GET** `/beings`

Restituisce la pagina dell'interfaccia di gestione degli Esseri di Silicio.

### Ottieni Elenco degli Esseri

**GET** `/api/beings` o **GET** `/api/beings/list`

Restituisce l'elenco di tutti gli Esseri di Silicio registrati.

**Esempio di risposta**:
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

**Valori dello stato**: `idle` | `running` | `waiting_permission` | `stopped`

### Ottieni Dettagli dell'Essere

**GET** `/api/beings/detail`

Parametro di query: `beingId` — ID dell'Essere di Silicio

Restituisce informazioni dettagliate sull'Essere di Silicio specificato.

### Ottieni Stato di Attività degli Esseri

**GET** `/api/beings/activity`

Restituisce le informazioni sullo stato di attività di ciascun Essere di Silicio.

### Pagina Editor del File dell'Anima

**GET** `/beings/soul`

Restituisce l'interfaccia dell'editor del File dell'Anima.

### Salva il File dell'Anima

**POST** `/api/beings/soul/save`

**Corpo della richiesta**:
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### Pagina Editor Configurazione AI

**GET** `/beings/ai-config`

Restituisce l'interfaccia dell'editor della configurazione AI.

### Salva Configurazione AI

**POST** `/api/beings/ai-config/save`

**Corpo della richiesta**:
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

### Ottieni Elenco dei Modelli AI Disponibili

**GET** `/api/beings/ai-config/models`

Parametri di query: `clientType`, `apiKey`, `region`

Restituisce l'elenco dei modelli disponibili per il client AI specificato.

---

## Visualizzazione Cronologia Chat

### Pagina Cronologia Chat

**GET** `/chat-history`

Restituisce la pagina principale della cronologia chat.

### Pagina Dettagli Cronologia Chat

**GET** `/chat-history-detail`

Restituisce la pagina dei dettagli della cronologia chat per la sessione specificata.

### Pagina Dettagli Cronologia Chat di Gruppo

**GET** `/group-chat-history-detail`

Restituisce la pagina dei dettagli della cronologia della chat di gruppo.

### Pagina Dettagli Cronologia Trasmissioni

**GET** `/broadcast-history-detail`

Restituisce la pagina dei dettagli della cronologia del canale di trasmissione.

### Ottieni Elenco delle Sessioni Storiche

**GET** `/api/chat-history/conversations`

Restituisce l'elenco di tutte le sessioni storiche.

### Ottieni Messaggi Storici

**GET** `/api/chat-history/messages`

Parametro di query: `sessionId` — ID sessione

Restituisce i record dei messaggi della sessione storica specificata.

---

## Gestione dei Timer

### Pagina Timer

**GET** `/timers`

Restituisce la pagina dell'interfaccia di gestione dei timer.

### Ottieni Elenco dei Timer

**GET** `/api/timers/list`

Restituisce l'elenco di tutti i timer.

### Pagina Dettagli Ciclo del Timer

**GET** `/timer-cycles/{timerId}`

Restituisce la pagina dei dettagli del ciclo di esecuzione del timer specificato.

### Ottieni Elenco dei Cicli del Timer

**GET** `/api/timer-cycles/list`

Parametro di query: `timerId` — ID timer

Restituisce l'elenco di tutti i cicli di esecuzione del timer specificato.

### Pagina Dettagli Singolo Ciclo di Esecuzione

**GET** `/timer-cycle/{cycleIndex}`

Restituisce la pagina dei dettagli di una singola esecuzione.

### Ottieni Messaggi del Ciclo

**GET** `/api/timer-cycle/messages`

Parametro di query: `cycleIndex` — Indice del ciclo

Restituisce i messaggi relativi al ciclo di esecuzione specificato.

---

## Gestione delle Attività

### Pagina Attività

**GET** `/tasks`

Restituisce la pagina dell'interfaccia di gestione delle attività.

### Ottieni Elenco delle Attività

**GET** `/api/tasks/list`

Restituisce l'elenco di tutte le attività.

### Pagina Dettagli Ciclo dell'Attività

**GET** `/task-cycles/{taskId}`

Restituisce la pagina dei dettagli del ciclo di esecuzione dell'attività specificata.

### Ottieni Elenco dei Cicli dell'Attività

**GET** `/api/task-cycles/list`

Parametro di query: `taskId` — ID attività

Restituisce l'elenco di tutti i cicli di esecuzione dell'attività specificata.

### Pagina Dettagli Singolo Ciclo di Esecuzione

**GET** `/task-cycle/{cycleIndex}`

Restituisce la pagina dei dettagli di una singola esecuzione dell'attività.

### Ottieni Messaggi del Ciclo

**GET** `/api/task-cycle/messages`

Parametro di query: `cycleIndex` — Indice del ciclo

Restituisce i messaggi relativi al ciclo di esecuzione dell'attività specificata.

---

## Sistema di Permessi

### Pagina Gestione Permessi

**GET** `/permissions`

Restituisce la pagina dell'interfaccia di gestione dei permessi.

### Ottieni Elenco delle Regole di Permesso

**GET** `/api/permissions/list`

Restituisce tutte le regole di permesso attualmente configurate.

**Esempio di risposta**:
```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed",
      "description": "Allow GitHub API access"
    }
  ]
}
```

### Salva Regola di Permesso

**POST** `/api/permissions/save`

**Corpo della richiesta**:
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
}
```

### Pagina Richiesta di Permesso

**GET** `/permission/request`

Mostra la pagina di richiesta di permesso, permettendo all'utente di approvare o rifiutare le richieste di permesso degli Esseri di Silicio.

**Parametri di query**:

| Parametro | Tipo | Descrizione |
|-----------|------|-------------|
| `userId` | `Guid` | ID dell'Essere di Silicio che richiede il permesso |
| `type` | `string` | Tipo di permesso |
| `resource` | `string` | Percorso della risorsa richiesta |
| `allowCode` | `string` | Codice identificativo per l'operazione di consenso |
| `denyCode` | `string` | Codice identificativo per l'operazione di rifiuto |

### Verifica Richieste di Permesso in Sospeso

**GET** `/permission/check`

Parametro di query: `userId` — ID dell'Essere di Silicio

**Risposta**:
```json
{
  "pending": true
}
```

### Rispondi a una Richiesta di Permesso

**GET** `/permission/respond`

**Parametri di query**:

| Parametro | Tipo | Descrizione |
|-----------|------|-------------|
| `userId` | `Guid` | ID dell'Essere di Silicio |
| `allowed` | `bool` | Se consentire |
| `addToCache` | `bool` | Se memorizzare nella cache la decisione |
| `cacheDuration` | `double` | Durata della cache (ore) |

**Risposta**:
```json
{
  "success": true
}
```

---

## Sistema di Log

### Pagina Log

**GET** `/logs`

Restituisce la pagina dell'interfaccia di visualizzazione dei log.

### Ottieni Elenco dei Log

**GET** `/api/logs/list`

I parametri di query supportano il filtraggio per livello e intervallo temporale.

**Esempio di risposta**:
```json
{
  "logs": [
    {
      "timestamp": "2026-04-20T10:30:00Z",
      "level": "error",
      "message": "Failed to connect to AI service",
      "source": "OllamaClient"
    }
  ]
}
```

### Ottieni Log Raggruppati per Essere

**GET** `/api/logs/beings`

Statistiche dei log raggruppate per Essere di Silicio.

### Ottieni Livelli di Log Disponibili

**GET** `/api/logs/levels`

Restituisce l'elenco dei livelli di log disponibili nel sistema.

---

## Statistiche di Utilizzo

### Pagina Statistiche di Utilizzo

**GET** `/usage`

Restituisce la pagina dell'interfaccia delle statistiche di utilizzo.

### Ottieni Riepilogo Utilizzo

**GET** `/api/usage/summary`

Restituisce un riepilogo dell'utilizzo dei token e dei costi.

### Ottieni Dati di Tendenza

**GET** `/api/usage/trend`

Parametri di query: `startDate`, `endDate`

Restituisce i dati di tendenza dell'utilizzo per il periodo specificato.

### Esporta Dati di Utilizzo

**GET** `/api/usage/export`

Esporta i dati di utilizzo in un formato scaricabile.

---

## Traccia di Audit

### Pagina di Audit

**GET** `/audit`

Restituisce la pagina dell'interfaccia della traccia di audit.

### Ottieni Elenco di Audit

**GET** `/api/audit/list`

Restituisce l'elenco delle voci del log di audit.

### Ottieni Riepilogo di Audit

**GET** `/api/audit/summary`

Restituisce statistiche riepilogative dei dati di audit.

### Ottieni Audit Raggruppato per Essere

**GET** `/api/audit/beings`

Statistiche di audit raggruppate per Essere di Silicio.

---

## Gestione della Configurazione

### Pagina Configurazione

**GET** `/config`

Restituisce la pagina dell'interfaccia di configurazione del sistema.

### Salva Configurazione

**POST** `/config/save`

**Corpo della richiesta**:
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

### Ottieni Opzioni di Configurazione AI

**GET** `/config/aioptions`

Restituisce i tipi di client AI disponibili e le relative opzioni dinamiche (modelli disponibili, regioni, ecc.).

---

## Sistema di Memoria

### Pagina Memoria

**GET** `/memory`

Restituisce la pagina dell'interfaccia di gestione della memoria.

### Ottieni Elenco della Memoria

**GET** `/api/memory/list`

Restituisce l'elenco delle voci di memoria degli Esseri di Silicio.

### Ottieni Dettagli della Memoria

**GET** `/api/memory/detail/{id}`

Parametro di percorso: `id` — ID voce di memoria

Restituisce il contenuto completo della voce di memoria specificata.

### Ottieni Statistiche della Memoria

**GET** `/api/memory/stats`

Restituisce informazioni statistiche del sistema di memoria.

### Cerca nella Memoria

**GET** `/api/memory/search`

Parametro di query: `keyword` — Parola chiave di ricerca

Cerca voci di memoria corrispondenti.

### Ottieni Memoria Raggruppata per Essere

**GET** `/api/memory/beings`

Statistiche della memoria raggruppate per Essere di Silicio.

### Ottieni Tracciamento della Memoria

**GET** `/api/memory/trace/{id}`

Parametro di percorso: `id` — ID voce di memoria

Restituisce la catena di tracciamento delle origini della voce di memoria specificata.

### Ottieni HTML della Timeline della Memoria

**GET** `/api/memory/timeline-html`

Restituisce la vista HTML della timeline della memoria.

---

## Note di Lavoro

### Pagina Note di Lavoro

**GET** `/work-notes`

Restituisce la pagina dell'interfaccia delle note di lavoro.

### Ottieni Elenco delle Note di Lavoro

**GET** `/api/work-notes/list`

Restituisce l'elenco delle note di lavoro.

### Leggi una Nota di Lavoro

**GET** `/api/work-notes/read`

Parametro di query: `noteId` — ID nota

Restituisce il contenuto della nota specificata.

### Ottieni Indice delle Note

**GET** `/api/work-notes/directory`

Restituisce la struttura dell'indice delle note.

### Cerca nelle Note di Lavoro

**GET** `/api/work-notes/search`

Parametro di query: `keyword` — Parola chiave di ricerca

Cerca note di lavoro corrispondenti.

### Crea una Nota di Lavoro

**POST** `/api/work-notes/create`

**Corpo della richiesta**:
```json
{
  "title": "Titolo della nota",
  "content": "Contenuto della nota",
  "keywords": ["parola chiave 1", "parola chiave 2"]
}
```

### Aggiorna una Nota di Lavoro

**POST** `/api/work-notes/update`

**Corpo della richiesta**:
```json
{
  "noteId": "note-uuid",
  "title": "Titolo aggiornato",
  "content": "Contenuto aggiornato"
}
```

### Elimina una Nota di Lavoro

**POST** `/api/work-notes/delete`

**Corpo della richiesta**:
```json
{
  "noteId": "note-uuid"
}
```

---

## Rete di Conoscenza

### Pagina Rete di Conoscenza

**GET** `/knowledge`

Restituisce la pagina dell'interfaccia di gestione della rete di conoscenza.

### Ottieni Grafo di Conoscenza

**GET** `/api/knowledge/graph`

Restituisce i dati del grafo delle triple di conoscenza (soggetto-relazione-oggetto).

---

## Gestione dei Progetti

### Pagina Progetti

**GET** `/project`

Restituisce la pagina dell'interfaccia di gestione dei progetti.

### Pagina Note di Lavoro di Progetto

**GET** `/project/{id}/work-notes`

Parametro di percorso: `id` — ID progetto

Restituisce la pagina delle note di lavoro del progetto specificato.

### Pagina Attività di Progetto

**GET** `/project/{id}/tasks`

Parametro di percorso: `id` — ID progetto

Restituisce la pagina di gestione delle attività del progetto specificato.

### Pagina Permessi Strumenti di Progetto

**GET** `/project/{id}/tool-permissions`

Parametro di percorso: `id` — ID progetto

Restituisce la pagina di gestione dei permessi degli strumenti del progetto specificato.

### Pagina Workflow di Progetto

**GET** `/project/{id}/workflow`

Parametro di percorso: `id` — ID progetto

Restituisce la pagina di gestione del workflow del progetto specificato.

### Ottieni Dettagli Workflow di Progetto

**GET** `/api/projects/workflow-detail`

Parametro di query: `projectId` — ID progetto

Restituisce i dettagli del workflow associato al progetto.

### Assegna Ruolo di Progetto

**POST** `/api/projects/assign-role`

**Corpo della richiesta**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Rimuovi Ruolo di Progetto

**POST** `/api/projects/remove-role`

**Corpo della richiesta**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Ottieni Elenco dei Progetti

**GET** `/api/projects/list`

Restituisce l'elenco di tutti i progetti.

### Ottieni Elenco dei Template di Workflow di Progetto

**GET** `/api/projects/list-workflow-templates`

Restituisce l'elenco dei template di workflow disponibili.

### Crea un Progetto

**POST** `/api/projects/create`

**Corpo della richiesta**:
```json
{
  "name": "Il mio Progetto",
  "description": "Descrizione del progetto"
}
```

### Archivia un Progetto

**POST** `/api/projects/{id}/archive`

Parametro di percorso: `id` — ID progetto

Archivia il progetto specificato.

### Ripristina un Progetto

**POST** `/api/projects/{id}/restore`

Parametro di percorso: `id` — ID progetto

Ripristina un progetto archiviato.

### Distruggi un Progetto

**POST** `/api/projects/{id}/destroy`

Parametro di percorso: `id` — ID progetto

Elimina definitivamente il progetto specificato (irreversibile).

### Ottieni Dettagli del Progetto

**GET** `/api/projects/detail`

Parametro di query: `projectId` — ID progetto

Restituisce informazioni dettagliate sul progetto.

### Aggiorna un Progetto

**POST** `/api/projects/update`

**Corpo della richiesta**:
```json
{
  "projectId": "project-uuid",
  "name": "Nome Aggiornato",
  "description": "Descrizione aggiornata"
}
```

### Assegna un Membro al Progetto

**POST** `/api/projects/assign`

**Corpo della richiesta**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Rimuovi un Membro dal Progetto

**POST** `/api/projects/remove`

**Corpo della richiesta**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Ottieni Elenco delle Note di Lavoro di Progetto

**GET** `/api/projects/{id}/work-notes/list`

Parametro di percorso: `id` — ID progetto

Restituisce l'elenco delle note di lavoro del progetto specificato.

### Leggi Note di Lavoro di Progetto

**GET** `/api/projects/{id}/work-notes/read`

Parametro di percorso: `id` — ID progetto

Restituisce il contenuto delle note di lavoro del progetto specificato.

### Crea Note di Lavoro di Progetto

**POST** `/api/projects/{id}/work-notes/create`

Parametro di percorso: `id` — ID progetto

Crea nuove note di lavoro nel progetto specificato.

### Aggiorna Note di Lavoro di Progetto

**POST** `/api/projects/{id}/work-notes/update`

Parametro di percorso: `id` — ID progetto

Aggiorna le note di lavoro nel progetto specificato.

### Elimina Note di Lavoro di Progetto

**POST** `/api/projects/{id}/work-notes/delete`

Parametro di percorso: `id` — ID progetto

Elimina le note di lavoro nel progetto specificato.

### Ottieni Elenco delle Attività di Progetto

**GET** `/api/projects/{id}/tasks/list`

Parametro di percorso: `id` — ID progetto

Restituisce l'elenco delle attività del progetto specificato.

### Crea un'Attività di Progetto

**POST** `/api/projects/{id}/tasks/create`

Parametro di percorso: `id` — ID progetto

Crea una nuova attività nel progetto specificato.

### Aggiorna un'Attività di Progetto

**POST** `/api/projects/{id}/tasks/update`

Parametro di percorso: `id` — ID progetto

Aggiorna un'attività nel progetto specificato.

### Elimina un'Attività di Progetto

**POST** `/api/projects/{id}/tasks/delete`

Parametro di percorso: `id` — ID progetto

Elimina un'attività nel progetto specificato.

### Assegna un Responsabile all'Attività

**POST** `/api/projects/{id}/tasks/assign`

Parametro di percorso: `id` — ID progetto

Assegna un responsabile all'attività del progetto.

### Rimuovi il Responsabile dell'Attività

**POST** `/api/projects/{id}/tasks/remove-assignee`

Parametro di percorso: `id` — ID progetto

Rimuove il responsabile dall'attività del progetto.

### Contrassegna un'Attività come Completata

**POST** `/api/projects/{id}/tasks/complete`

Parametro di percorso: `id` — ID progetto

Contrassegna l'attività del progetto come completata.

### Contrassegna un'Attività come Fallita

**POST** `/api/projects/{id}/tasks/fail`

Parametro di percorso: `id` — ID progetto

Contrassegna l'attività del progetto come fallita.

### Annulla un'Attività

**POST** `/api/projects/{id}/tasks/cancel`

Parametro di percorso: `id` — ID progetto

Annulla l'attività del progetto.

---

## Gestione dei Permessi degli Strumenti

### Ottieni Permessi Strumenti dell'Essere di Silicio

**GET** `/api/beings/tool-permissions`

Parametro di query: `beingId` — ID dell'Essere di Silicio

Restituisce la configurazione dei permessi degli strumenti per l'Essere di Silicio specificato.

### Aggiorna Permessi Strumenti dell'Essere di Silicio

**PUT** `/api/beings/tool-permissions`

**Corpo della richiesta**:
```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

### Ottieni Template dei Permessi degli Strumenti

**GET** `/api/beings/tool-permissions/templates`

Restituisce l'elenco dei template di permessi degli strumenti disponibili.

### Applica un Template di Permessi degli Strumenti

**POST** `/api/beings/tool-permissions/apply-template`

**Corpo della richiesta**:
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### Ottieni Permessi Strumenti di Progetto

**GET** `/api/projects/{id}/tool-permissions`

Parametro di percorso: `id` — ID progetto

Restituisce la configurazione dei permessi degli strumenti per il progetto specificato.

### Aggiorna Permessi Strumenti di Progetto

**PUT** `/api/projects/{id}/tool-permissions`

Parametro di percorso: `id` — ID progetto

**Corpo della richiesta**:
```json
{
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

---

## Gestione degli Esecutori

### Pagina Esecutori

**GET** `/executor`

Restituisce la pagina dell'interfaccia di gestione degli esecutori.

### Ottieni Stato degli Esecutori

**GET** `/api/executors/status`

Restituisce lo stato di esecuzione di ciascun esecutore (disco, rete, riga di comando).

---

## Browser del Codice

### Pagina Browser del Codice

**GET** `/code`

Restituisce la pagina dell'interfaccia del browser del codice.

### Ottieni Elenco dei Tipi di Codice

**GET** `/api/code/types`

Restituisce l'elenco dei tipi/linguaggi di codice supportati.

### Ottieni Dettagli del Codice

**GET** `/api/code/detail`

Parametri di query: `filePath`, `lineNumber`

Restituisce i dettagli del codice del file specificato.

---

## Tooltip del Codice

### Ottieni Tooltip Sospeso

**GET** `/api/code/hover`
**POST** `/api/code/hover`

Ottieni informazioni di tooltip sospeso per la posizione del codice (simile ai suggerimenti intelligenti dell'IDE).

### Registra Posizione del Codice

**POST** `/api/code/register`

Registra una posizione del codice da monitorare.

### Aggiorna Posizione del Codice

**POST** `/api/code/update`

Aggiorna le informazioni di una posizione del codice registrata.

### Deregistra Posizione del Codice

**POST** `/api/code/unregister`

Deregistra il monitoraggio di una posizione del codice non più necessaria.

---

## Sistema di Documentazione di Aiuto

### Pagina di Aiuto

**GET** `/help` o **GET** `/help/index`

Restituisce la pagina principale della documentazione di aiuto.

### Pagina Argomento di Aiuto

**GET** `/help/{topic}`

Parametro di percorso: `topic` — Identificatore dell'argomento

Restituisce la pagina della documentazione di aiuto per l'argomento specificato.

### Cerca nella Documentazione di Aiuto

**GET** `/api/help/search`

Parametro di query: `keyword` — Parola chiave di ricerca

Cerca argomenti della documentazione di aiuto corrispondenti.

---

## Inizializzazione

### Pagina Procedura Guidata di Inizializzazione

**GET** `/init`

Restituisce la pagina della procedura guidata di inizializzazione alla prima esecuzione.

### Invia Inizializzazione

**POST** `/init`

Invia la configurazione di inizializzazione per la prima esecuzione.

### Sfoglia per Selezionare la Directory dei Dati

**GET** `/init/browse`

Apre il browser delle directory per selezionare la posizione di archiviazione dei dati.

### Ottieni Metadati della Configurazione AI

**GET** `/init/ai-config-metadata`

Restituisce i tipi di client AI disponibili e i metadati dei campi di configurazione.

---

## Controllo di Sistema

### Chiusura Elegante

**POST** `/api/system/shutdown`

> **Nota**: Sono consentite solo le richieste provenienti da localhost

Attiva la procedura di chiusura elegante dell'applicazione:

1. Ferma il ciclo principale (MainLoop)
2. Salva la configurazione corrente
3. Chiude il listener HTTP

**Risposta**:
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

Restituisce la pagina delle informazioni, contenente informazioni di sistema e l'elenco dei plugin caricati.

**Dati dell'elenco dei plugin**:
```json
{
  "plugins": {
    "plugin-id": {
      "name": "My Plugin",
      "version": "1.0.0",
      "description": "Plugin description",
      "author": "Author Name"
    }
  }
}
```

---

## Risposte di Errore

Tutti gli endpoint restituiscono risposte di errore standardizzate:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
  }
}
```

### Codici di Errore Comuni

| Codice | Stato HTTP | Descrizione |
|--------|------------|-------------|
| `PERMISSION_DENIED` | 403 | Permessi insufficienti |
| `NOT_FOUND` | 404 | Risorsa non trovata |
| `VALIDATION_ERROR` | 400 | Parametri della richiesta non validi |
| `INTERNAL_ERROR` | 500 | Errore interno del server |
| `SERVICE_UNAVAILABLE` | 503 | Servizio AI non disponibile |

---

## Eventi SSE

Gli eventi inviati dal server vengono utilizzati per aggiornamenti in tempo reale:

### Eventi di Chat

```javascript
const eventSource = new EventSource('/api/chat/stream');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);
  
  switch(data.type) {
    case 'chunk':
      console.log('Streaming:', data.content);
      break;
    case 'tool_call':
      console.log('Tool executing:', data.tool);
      break;
    case 'complete':
      console.log('Chat complete, session:', data.sessionId);
      break;
    case 'error':
      console.error('Error:', data.message);
      break;
  }
};
```

---

## Interfaccia del Client AI

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

## Interfaccia del Sistema degli Strumenti

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

## Prossimi Passi

- 🚀 Consulta la [Guida Rapida](getting-started.md)
- 🛠️ Leggi la [Guida allo Sviluppo](development-guide.md)
- 📚 Consulta la [Documentazione sull'Architettura](architecture.md)
- 🔒 Comprendi il [Modello di Sicurezza](security.md)
