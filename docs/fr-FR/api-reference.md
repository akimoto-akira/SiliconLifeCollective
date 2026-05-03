# Référence API

> **Version : v0.1.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | **Français** | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md)

## Points de terminaison de l'API Web

URL de base : `http://localhost:8080`

### Authentification

La plupart des points de terminaison nécessitent une authentification via des cookies de session gérés par l'interface Web.

---

## Gestion des Silicon Beings

### Obtenir tous les Beings

**GET** `/api/beings`

**Réponse** :
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "status": "running",
      "soul": "path/to/soul.md"
    }
  ]
}
```

**Valeurs de statut** : `idle` | `running` | `waiting_permission` | `stopped`

### Créer un Being

**POST** `/api/beings`

**Requête** :
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**Réponse** : `201 Created`

### Démarrer un Being

**POST** `/api/beings/{id}/start`

### Arrêter un Being

**POST** `/api/beings/{id}/stop`

### Obtenir les détails d'un Being

**GET** `/api/beings/{id}`

---

## Système de chat

### Envoyer un message

**POST** `/api/chat`

**Requête** :
```json
{
  "beingId": "being-uuid",
  "message": "Bonjour, comment allez-vous ?",
  "sessionId": "optional-session-id"
}
```

**Réponse** (non streamé) :
```json
{
  "reply": "Je vais bien, merci !",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Chat en streaming (SSE)

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**Réponse** : Flux Server-Sent Events

```
data: {"type": "chunk", "content": "Je"}
data: {"type": "chunk", "content": " réfléchis"}
data: {"type": "chunk", "content": " suis..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Obtenir l'historique de chat

**GET** `/api/chat/{sessionId}/history`

**Réponse** :
```json
{
  "messages": [
    {
      "role": "user",
      "content": "Bonjour",
      "timestamp": "2026-04-20T10:30:00Z"
    },
    {
      "role": "assistant",
      "content": "Salut !",
      "timestamp": "2026-04-20T10:30:05Z"
    }
  ]
}
```

---

## Configuration

### Obtenir la configuration

**GET** `/api/config`

**Réponse** :
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

### Mettre à jour la configuration

**POST** `/api/config`

**Requête** :
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

## Système de permissions

### Obtenir les permissions

**GET** `/api/permissions`

**Réponse** :
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

### Accorder une permission

**POST** `/api/permissions`

**Requête** :
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### Révoquer une permission

**DELETE** `/api/permissions/{id}`

### Vérifier une permission

**POST** `/api/permissions/check`

**Requête** :
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**Réponse** :
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

---

## Système de tâches et de minuteries

### Créer une tâche

**POST** `/api/tasks`

**Requête** :
```json
{
  "beingId": "being-uuid",
  "description": "Review code",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### Obtenir les tâches

**GET** `/api/tasks?beingId={id}&status=pending`

### Mettre à jour le statut d'une tâche

**PATCH** `/api/tasks/{id}`

**Requête** :
```json
{
  "status": "completed"
}
```

### Créer une minuterie

**POST** `/api/timers`

**Requête** :
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### Supprimer une minuterie

**DELETE** `/api/timers/{id}`

---

## Audit et journalisation

### Obtenir l'utilisation des tokens

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**Réponse** :
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

### Obtenir les journaux

**GET** `/api/logs?level=error&limit=100`

**Réponse** :
```json
{
  "logs": [
    {
      "timestamp": "2026-04-20T10:30:00Z",
      "level": "error",
      "message": "Échec de connexion au service IA",
      "source": "OllamaClient"
    }
  ]
}
```

---

## API de stockage

### Lire une valeur

**GET** `/api/storage?key={key}`

**Réponse** :
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Écrire une valeur

**POST** `/api/storage`

**Requête** :
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### Requête par plage temporelle

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**Réponse** :
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

## Informations système

### Obtenir la page À propos

**GET** `/about`

Retourne la page À propos avec les informations système et la liste des plugins chargés.

**Données de la liste de plugins** :
```json
{
  "plugins": {
    "plugin-id": {
      "name": "Mon Plugin",
      "version": "1.0.0",
      "description": "Description du plugin",
      "author": "Nom de l'auteur"
    }
  }
}
```

### Requête de permission

**GET** `/permission/request?userId={id}&type={type}&resource={resource}`

Affiche la page de requête de permission permettant aux utilisateurs d'approuver ou de refuser les requêtes de permission des Silicon Beings.

**Paramètres de requête** :

| Paramètre | Type | Description |
|-----------|------|-------------|
| `userId` | `Guid` | ID du Silicon Being demandant la permission |
| `type` | `string` | Type de permission |
| `resource` | `string` | Chemin de la ressource demandée |
| `allowCode` | `string` | ID de code pour l'opération d'autorisation |
| `denyCode` | `string` | ID de code pour l'opération de refus |

**Vérifier les requêtes de permission en attente** :

**GET** `/permission/check?userId={id}`

**Réponse** :
```json
{
  "pending": true
}
```

**Répondre à une requête de permission** :

**GET** `/permission/respond?userId={id}&allowed={bool}&addToCache={bool}&cacheDuration={hours}`

**Paramètres de requête** :

| Paramètre | Type | Description |
|-----------|------|-------------|
| `userId` | `Guid` | ID du Silicon Being |
| `allowed` | `bool` | Si autorisé |
| `addToCache` | `bool` | Si la décision doit être mise en cache |
| `cacheDuration` | `double` | Durée du cache (heures) |

**Réponse** :
```json
{
  "success": true
}
```

### Obtenir les données du tableau de bord

**GET** `/api/dashboard`

**Réponse** :
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

### Obtenir le statut du système

**GET** `/api/status`

**Réponse** :
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
}
```

---

## Réponses d'erreur

Tous les points de terminaison retournent des réponses d'erreur standardisées :

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "Vous n'avez pas la permission d'accéder à cette ressource",
    "details": "Requis : disk:write, Actuel : disk:read"
  }
}
```

### Codes d'erreur courants

| Code | Statut HTTP | Description |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | Permissions insuffisantes |
| `NOT_FOUND` | 404 | Ressource non trouvée |
| `VALIDATION_ERROR` | 400 | Paramètres de requête invalides |
| `INTERNAL_ERROR` | 500 | Erreur interne du serveur |
| `SERVICE_UNAVAILABLE` | 503 | Service IA non disponible |

---

## Événements SSE

Server-Sent Events pour les mises à jour en temps réel :

### Événements de chat

```javascript
const eventSource = new EventSource('/api/chat/stream?beingId=xxx&message=xxx');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);
  
  switch(data.type) {
    case 'chunk':
      console.log('Streaming:', data.content);
      break;
    case 'tool_call':
      console.log('Outil en cours:', data.tool);
      break;
    case 'complete':
      console.log('Chat terminé, session:', data.sessionId);
      break;
    case 'error':
      console.error('Erreur:', data.message);
      break;
  }
};
```

### Événements de statut des Beings

```javascript
const beingEvents = new EventSource('/api/beings/events');

beingEvents.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(`Being ${data.beingId} statut : ${data.status}`);
};
```

---

## API du client IA

### Interface IAIClient

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### Structure AIRequest

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

### Structure AIResponse

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

## API des notes de travail

### Obtenir la liste des notes de travail

**GET** `/api/beings/{id}/work-notes`

**Réponse** :
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "Module d'authentification utilisateur terminé",
      "keywords": ["authentification", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### Obtenir les détails d'une note

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**Réponse** :
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "Module d'authentification utilisateur terminé",
  "content": "## Détails d'implémentation\n\n- Utilisation de JWT token\n- Support OAuth2",
  "keywords": ["authentification", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### Créer une nouvelle note

**POST** `/api/beings/{id}/work-notes`

**Requête** :
```json
{
  "summary": "Module d'authentification utilisateur terminé",
  "content": "## Détails d'implémentation\n\n- Utilisation de JWT token",
  "keywords": "authentification,JWT,OAuth2"
}
```

**Réponse** : `201 Created`

### Mettre à jour une note

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**Requête** :
```json
{
  "summary": "Module d'authentification utilisateur et tests terminés",
  "content": "## Contenu mis à jour\n\nAjout de tests unitaires",
  "keywords": "authentification,JWT,OAuth2,tests"
}
```

### Supprimer une note

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### Rechercher des notes

**GET** `/api/beings/{id}/work-notes/search?keyword=authentification&maxResults=10`

### Obtenir le répertoire des notes

**GET** `/api/beings/{id}/work-notes/directory`

---

## API du réseau de connaissances

### Obtenir les statistiques de connaissances

**GET** `/api/knowledge/stats`

**Réponse** :
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Ajouter un triplet de connaissance

**POST** `/api/knowledge/triples`

**Requête** :
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**Réponse** : `201 Created`

### Consulter les connaissances

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**Réponse** :
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

### Rechercher des connaissances

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### Obtenir un chemin de connaissance

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**Réponse** :
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### Valider une connaissance

**POST** `/api/knowledge/validate`

**Requête** :
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### Supprimer une connaissance

**DELETE** `/api/knowledge/triples/{id}`

---

## API de documentation d'aide

### Obtenir la liste de la documentation d'aide

**GET** `/api/help`

**Réponse** :
```json
{
  "topics": [
    {
      "id": "getting-started",
      "title": "Démarrage rapide",
      "category": "Guide de démarrage"
    }
  ]
}
```

### Obtenir les détails d'un sujet d'aide

**GET** `/api/help/{topicId}`

**Réponse** :
```json
{
  "id": "getting-started",
  "title": "Démarrage rapide",
  "content": "# Démarrage rapide\n\n...",
  "category": "Guide de démarrage"
}
```

---

## API du navigateur WebView

### Obtenir le statut du navigateur

**GET** `/api/beings/{id}/browser/status`

**Réponse** :
```json
{
  "is_open": true,
  "current_url": "https://example.com",
  "page_title": "Example Page",
  "is_loading": false,
  "last_operation_time": "2026-04-26T10:00:00Z"
}
```

### Ouvrir le navigateur

**POST** `/api/beings/{id}/browser/open`

### Fermer le navigateur

**POST** `/api/beings/{id}/browser/close`

### Naviguer vers une URL

**POST** `/api/beings/{id}/browser/navigate`

**Requête** :
```json
{
  "url": "https://example.com"
}
```

### Exécuter du JavaScript

**POST** `/api/beings/{id}/browser/execute-script`

**Requête** :
```json
{
  "script": "return document.title;"
}
```

### Obtenir une capture d'écran de la page

**GET** `/api/beings/{id}/browser/screenshot`

---

## API de l'espace de projet

### Obtenir la liste des projets

**GET** `/api/projects`

**Réponse** :
```json
{
  "projects": [
    {
      "id": "project-uuid",
      "name": "Mon Projet",
      "description": "Description du projet",
      "createdAt": "2026-04-25T10:00:00Z"
    }
  ]
}
```

### Créer un projet

**POST** `/api/projects`

**Requête** :
```json
{
  "name": "Mon Projet",
  "description": "Description du projet"
}
```

### Obtenir les détails d'un projet

**GET** `/api/projects/{id}`

### Mettre à jour un projet

**PUT** `/api/projects/{id}`

### Supprimer un projet

**DELETE** `/api/projects/{id}`

---

## API du système d'outils

### Interface ITool

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### Structure ToolCall

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### Structure ToolResult

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## Prochaines étapes

- 🚀 Voir le [guide de démarrage rapide](getting-started.md)
- 🛠️ Lire le [guide de développement](development-guide.md)
- 📚 Consulter la [documentation d'architecture](architecture.md)
- 🔒 Comprendre le [modèle de sécurité](security.md)
