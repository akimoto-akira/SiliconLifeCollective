# Référence API

> **Version : v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md)

## Points de terminaison Web API

URL de base : `http://localhost:8080`

### Authentification

La plupart des points de terminaison nécessitent une authentification via un cookie de session géré par l'UI Web. Avant l'initialisation du système, toutes les requêtes à l'exception de la page d'aide seront redirigées vers la page d'initialisation.

---

## Tableau de bord

### Obtenir les statistiques du tableau de bord

**GET** `/api/dashboard/stats`

Retourne les données de synthèse du système (nombre d'Êtres de Silicium, état de fonctionnement, etc.).

### Obtenir les métriques de performance

**GET** `/api/dashboard/metrics`

Retourne les métriques de performance en temps réel.

---

## Système de Chat

### Page de chat

**GET** `/chat`

Retourne la page de l'interface de chat.

### Chat en flux (SSE)

**GET** `/api/chat/stream`

Chat en flux via les événements envoyés par le serveur (SSE).

**Réponse** : Flux d'événements envoyés par le serveur

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Obtenir la liste des sessions

**GET** `/api/chat/conversations`

Retourne la liste de toutes les sessions de chat actives.

**Exemple de réponse** :
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Chat avec Xiaoyou",
      "lastMessage": "Contenu du dernier message",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### Obtenir l'historique des messages

**GET** `/api/chat/messages`

Paramètre de requête : `channelId` — ID du canal/session

Retourne l'historique des messages de la session spécifiée.

### Obtenir l'historique de chat

**GET** `/api/chat/history`

Retourne l'historique global de chat.

### Envoyer un message

**POST** `/api/chat/send`

**Corps de la requête** :
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "Contenu du message de test"
}
```

**Réponse** :
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### Arrêter la réflexion de l'IA

**POST** `/api/chat/stop`

Arrête la génération de réponse IA en cours.

**Corps de la requête** :
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### Télécharger un fichier

**POST** `/api/chat/upload`

Télécharge un fichier dans la session de chat (prend en charge multipart/form-data).

---

## Gestion des Êtres de Silicium

### Page de gestion des Êtres de Silicium

**GET** `/beings`

Retourne la page de l'interface de gestion des Êtres de Silicium.

### Obtenir la liste des Êtres de Silicium

**GET** `/api/beings` ou **GET** `/api/beings/list`

Retourne la liste de tous les Êtres de Silicium enregistrés.

**Exemple de réponse** :
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

**Valeurs d'état** : `idle` | `running` | `waiting_permission` | `stopped`

### Obtenir les détails d'un Être de Silicium

**GET** `/api/beings/detail`

Paramètre de requête : `beingId` — ID de l'Être de Silicium

Retourne les informations détaillées de l'Être de Silicium spécifié.

### Obtenir l'état d'activité des Êtres de Silicium

**GET** `/api/beings/activity`

Retourne les informations d'état d'activité de chaque Être de Silicium.

### Page de l'éditeur du Fichier d'Âme

**GET** `/beings/soul`

Retourne l'interface de l'éditeur du Fichier d'Âme.

### Sauvegarder le Fichier d'Âme

**POST** `/api/beings/soul/save`

**Corps de la requête** :
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### Page de l'éditeur de configuration IA

**GET** `/beings/ai-config`

Retourne l'interface de l'éditeur de configuration IA.

### Sauvegarder la configuration IA

**POST** `/api/beings/ai-config/save`

**Corps de la requête** :
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

### Obtenir la liste des modèles IA disponibles

**GET** `/api/beings/ai-config/models`

Paramètres de requête : `clientType`, `apiKey`, `region`

Retourne la liste des modèles disponibles pour le client IA spécifié.

---

## Consultation de l'historique de chat

### Page de l'historique de chat

**GET** `/chat-history`

Retourne la page principale de l'historique de chat.

### Page de détail de l'historique de chat

**GET** `/chat-history-detail`

Retourne la page de détail de l'historique de chat pour la session spécifiée.

### Page de détail de l'historique de chat de groupe

**GET** `/group-chat-history-detail`

Retourne la page de détail de l'historique du chat de groupe.

### Page de détail de l'historique de diffusion

**GET** `/broadcast-history-detail`

Retourne la page de détail de l'historique du canal de diffusion.

### Obtenir la liste des sessions historiques

**GET** `/api/chat-history/conversations`

Retourne la liste de toutes les sessions historiques.

### Obtenir les messages historiques

**GET** `/api/chat-history/messages`

Paramètre de requête : `sessionId` — ID de la session

Retourne les messages de la session historique spécifiée.

---

## Gestion des minuteurs

### Page des minuteurs

**GET** `/timers`

Retourne la page de l'interface de gestion des minuteurs.

### Obtenir la liste des minuteurs

**GET** `/api/timers/list`

Retourne la liste de tous les minuteurs.

### Page de détail des cycles du minuteur

**GET** `/timer-cycles/{timerId}`

Retourne la page de détail des cycles d'exécution du minuteur spécifié.

### Obtenir la liste des cycles du minuteur

**GET** `/api/timer-cycles/list`

Paramètre de requête : `timerId` — ID du minuteur

Retourne la liste de tous les cycles d'exécution du minuteur spécifié.

### Page de détail d'un cycle d'exécution unique

**GET** `/timer-cycle/{cycleIndex}`

Retourne la page de détail d'une exécution unique.

### Obtenir les messages du cycle

**GET** `/api/timer-cycle/messages`

Paramètre de requête : `cycleIndex` — Index du cycle

Retourne les messages associés au cycle d'exécution spécifié.

---

## Gestion des tâches

### Page des tâches

**GET** `/tasks`

Retourne la page de l'interface de gestion des tâches.

### Obtenir la liste des tâches

**GET** `/api/tasks/list`

Retourne la liste de toutes les tâches.

### Page de détail des cycles de tâche

**GET** `/task-cycles/{taskId}`

Retourne la page de détail des cycles d'exécution de la tâche spécifiée.

### Obtenir la liste des cycles de tâche

**GET** `/api/task-cycles/list`

Paramètre de requête : `taskId` — ID de la tâche

Retourne la liste de tous les cycles d'exécution de la tâche spécifiée.

### Page de détail d'un cycle d'exécution unique

**GET** `/task-cycle/{cycleIndex}`

Retourne la page de détail d'une exécution de tâche unique.

### Obtenir les messages du cycle

**GET** `/api/task-cycle/messages`

Paramètre de requête : `cycleIndex` — Index du cycle

Retourne les messages associés au cycle d'exécution de tâche spécifié.

---

## Système d'autorisations

### Page de gestion des autorisations

**GET** `/permissions`

Retourne la page de l'interface de gestion des autorisations.

### Obtenir la liste des règles d'autorisation

**GET** `/api/permissions/list`

Retourne toutes les règles d'autorisation actuellement configurées.

**Exemple de réponse** :
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

### Sauvegarder une règle d'autorisation

**POST** `/api/permissions/save`

**Corps de la requête** :
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
}
```

### Page de demande d'autorisation

**GET** `/permission/request`

Affiche la page de demande d'autorisation, permettant à l'utilisateur d'approuver ou de refuser les demandes d'autorisation des Êtres de Silicium.

**Paramètres de requête** :

| Paramètre | Type | Description |
|-----------|------|-------------|
| `userId` | `Guid` | ID de l'Être de Silicium demandant l'autorisation |
| `type` | `string` | Type d'autorisation |
| `resource` | `string` | Chemin de la ressource demandée |
| `allowCode` | `string` | Identifiant de code pour l'opération autorisée |
| `denyCode` | `string` | Identifiant de code pour l'opération refusée |

### Vérifier les demandes d'autorisation en attente

**GET** `/permission/check`

Paramètre de requête : `userId` — ID de l'Être de Silicium

**Réponse** :
```json
{
  "pending": true
}
```

### Répondre à une demande d'autorisation

**GET** `/permission/respond`

**Paramètres de requête** :

| Paramètre | Type | Description |
|-----------|------|-------------|
| `userId` | `Guid` | ID de l'Être de Silicium |
| `allowed` | `bool` | Autoriser ou non |
| `addToCache` | `bool` | Mettre en cache la décision ou non |
| `cacheDuration` | `double` | Durée du cache (en heures) |

**Réponse** :
```json
{
  "success": true
}
```

---

## Système de journalisation

### Page des journaux

**GET** `/logs`

Retourne la page de l'interface de consultation des journaux.

### Obtenir la liste des journaux

**GET** `/api/logs/list`

Les paramètres de requête prennent en charge le filtrage par niveau et par plage de temps.

**Exemple de réponse** :
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

### Obtenir les journaux groupés par Être de Silicium

**GET** `/api/logs/beings`

Statistiques des journaux groupées par Être de Silicium.

### Obtenir les niveaux de journalisation disponibles

**GET** `/api/logs/levels`

Retourne la liste des niveaux de journalisation disponibles dans le système.

---

## Statistiques d'utilisation

### Page des statistiques d'utilisation

**GET** `/usage`

Retourne la page de l'interface des statistiques d'utilisation.

### Obtenir le résumé d'utilisation

**GET** `/api/usage/summary`

Retourne le résumé de l'utilisation et des coûts des tokens.

### Obtenir les données de tendance

**GET** `/api/usage/trend`

Paramètres de requête : `startDate`, `endDate`

Retourne les données de tendance d'utilisation pour la période spécifiée.

### Exporter les données d'utilisation

**GET** `/api/usage/export`

Exporte les données d'utilisation dans un format téléchargeable.

---

## Piste d'audit

### Page d'audit

**GET** `/audit`

Retourne la page de l'interface de la piste d'audit.

### Obtenir la liste d'audit

**GET** `/api/audit/list`

Retourne la liste des entrées du journal d'audit.

### Obtenir le résumé d'audit

**GET** `/api/audit/summary`

Retourne les statistiques agrégées des données d'audit.

### Obtenir l'audit groupé par Être de Silicium

**GET** `/api/audit/beings`

Statistiques d'audit groupées par Être de Silicium.

---

## Gestion de la configuration

### Page de configuration

**GET** `/config`

Retourne la page de l'interface de configuration système.

### Sauvegarder la configuration

**POST** `/config/save`

**Corps de la requête** :
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
    },
    "VolcengineArk": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "Herdsman": {
      "endpoint": "http://localhost:8000",
      "model": "..."
    },
    "LongCat": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "QiniuAI": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    }
  }
}
```

### Obtenir les options de configuration IA

**GET** `/config/aioptions`

Retourne les types de clients IA disponibles et leurs options dynamiques (modèles disponibles, régions, etc.).

---

## Système de mémoire

### Page de mémoire

**GET** `/memory`

Retourne la page de l'interface de gestion de la mémoire.

### Obtenir la liste des mémoires

**GET** `/api/memory/list`

Retourne la liste des entrées de mémoire des Êtres de Silicium.

### Obtenir les détails d'une mémoire

**GET** `/api/memory/detail/{id}`

Paramètre de chemin : `id` — ID de l'entrée de mémoire

Retourne le contenu complet de l'entrée de mémoire spécifiée.

### Obtenir les statistiques de mémoire

**GET** `/api/memory/stats`

Retourne les statistiques du système de mémoire.

### Rechercher dans la mémoire

**GET** `/api/memory/search`

Paramètre de requête : `keyword` — Mot-clé de recherche

Recherche les entrées de mémoire correspondantes.

### Obtenir la mémoire groupée par Être de Silicium

**GET** `/api/memory/beings`

Statistiques de mémoire groupées par Être de Silicium.

### Obtenir la traçabilité d'une mémoire

**GET** `/api/memory/trace/{id}`

Paramètre de chemin : `id` — ID de l'entrée de mémoire

Retourne la chaîne de traçabilité de la source de l'entrée de mémoire spécifiée.

### Obtenir la chronologie de la mémoire en HTML

**GET** `/api/memory/timeline-html`

Retourne la vue HTML de la chronologie de la mémoire.

---

## Notes de travail

### Page des notes de travail

**GET** `/work-notes`

Retourne la page de l'interface des notes de travail.

### Obtenir la liste des notes de travail

**GET** `/api/work-notes/list`

Retourne la liste des notes de travail.

### Lire une note de travail

**GET** `/api/work-notes/read`

Paramètre de requête : `noteId` — ID de la note

Retourne le contenu de la note spécifiée.

### Obtenir le répertoire des notes

**GET** `/api/work-notes/directory`

Retourne la structure du répertoire des notes.

### Rechercher dans les notes de travail

**GET** `/api/work-notes/search`

Paramètre de requête : `keyword` — Mot-clé de recherche

Recherche les notes de travail correspondantes.

### Créer une note de travail

**POST** `/api/work-notes/create`

**Corps de la requête** :
```json
{
  "title": "Titre de la note",
  "content": "Contenu de la note",
  "keywords": ["mot-clé1", "mot-clé2"]
}
```

### Mettre à jour une note de travail

**POST** `/api/work-notes/update`

**Corps de la requête** :
```json
{
  "noteId": "note-uuid",
  "title": "Titre mis à jour",
  "content": "Contenu mis à jour"
}
```

### Supprimer une note de travail

**POST** `/api/work-notes/delete`

**Corps de la requête** :
```json
{
  "noteId": "note-uuid"
}
```

---

## Réseau de Connaissances

### Page du Réseau de Connaissances

**GET** `/knowledge`

Retourne la page de l'interface de gestion du Réseau de Connaissances.

### Obtenir le graphe de connaissances

**GET** `/api/knowledge/graph`

Retourne les données du graphe de triplets de connaissances (sujet-relation-objet).

---

## Gestion de projet

### Page de projet

**GET** `/project`

Retourne la page de l'interface de gestion de projet.

### Page des notes de travail du projet

**GET** `/project/{id}/work-notes`

Paramètre de chemin : `id` — ID du projet

Retourne la page des notes de travail du projet spécifié.

### Page des tâches du projet

**GET** `/project/{id}/tasks`

Paramètre de chemin : `id` — ID du projet

Retourne la page de gestion des tâches du projet spécifié.

### Page des autorisations d'outil du projet

**GET** `/project/{id}/tool-permissions`

Paramètre de chemin : `id` — ID du projet

Retourne la page de gestion des autorisations d'outil du projet spécifié.

### Page du flux de travail du projet

**GET** `/project/{id}/workflow`

Paramètre de chemin : `id` — ID du projet

Retourne la page de gestion du flux de travail du projet spécifié.

### Obtenir les détails du flux de travail du projet

**GET** `/api/projects/workflow-detail`

Paramètre de requête : `projectId` — ID du projet

Retourne les détails du flux de travail associé au projet.

### Attribuer un rôle au projet

**POST** `/api/projects/assign-role`

**Corps de la requête** :
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Retirer un rôle du projet

**POST** `/api/projects/remove-role`

**Corps de la requête** :
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Obtenir la liste des projets

**GET** `/api/projects/list`

Retourne la liste de tous les projets.

### Obtenir la liste des modèles de flux de travail

**GET** `/api/projects/list-workflow-templates`

Retourne la liste des modèles de flux de travail disponibles.

### Créer un projet

**POST** `/api/projects/create`

**Corps de la requête** :
```json
{
  "name": "Mon Projet",
  "description": "Description du projet"
}
```

### Archiver un projet

**POST** `/api/projects/{id}/archive`

Paramètre de chemin : `id` — ID du projet

Archive le projet spécifié.

### Restaurer un projet

**POST** `/api/projects/{id}/restore`

Paramètre de chemin : `id` — ID du projet

Restaure un projet archivé.

### Détruire un projet

**POST** `/api/projects/{id}/destroy`

Paramètre de chemin : `id` — ID du projet

Supprime définitivement le projet spécifié (irréversible).

### Obtenir les détails d'un projet

**GET** `/api/projects/detail`

Paramètre de requête : `projectId` — ID du projet

Retourne les informations détaillées du projet.

### Mettre à jour un projet

**POST** `/api/projects/update`

**Corps de la requête** :
```json
{
  "projectId": "project-uuid",
  "name": "Nom mis à jour",
  "description": "Description mise à jour"
}
```

### Attribuer un membre au projet

**POST** `/api/projects/assign`

**Corps de la requête** :
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Retirer un membre du projet

**POST** `/api/projects/remove`

**Corps de la requête** :
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Obtenir la liste des notes de travail du projet

**GET** `/api/projects/{id}/work-notes/list`

Paramètre de chemin : `id` — ID du projet

Retourne la liste des notes de travail du projet spécifié.

### Lire les notes de travail du projet

**GET** `/api/projects/{id}/work-notes/read`

Paramètre de chemin : `id` — ID du projet

Retourne le contenu des notes de travail du projet spécifié.

### Créer une note de travail du projet

**POST** `/api/projects/{id}/work-notes/create`

Paramètre de chemin : `id` — ID du projet

Crée une nouvelle note de travail dans le projet spécifié.

### Mettre à jour une note de travail du projet

**POST** `/api/projects/{id}/work-notes/update`

Paramètre de chemin : `id` — ID du projet

Met à jour une note de travail dans le projet spécifié.

### Supprimer une note de travail du projet

**POST** `/api/projects/{id}/work-notes/delete`

Paramètre de chemin : `id` — ID du projet

Supprime une note de travail dans le projet spécifié.

### Obtenir la liste des tâches du projet

**GET** `/api/projects/{id}/tasks/list`

Paramètre de chemin : `id` — ID du projet

Retourne la liste des tâches du projet spécifié.

### Créer une tâche du projet

**POST** `/api/projects/{id}/tasks/create`

Paramètre de chemin : `id` — ID du projet

Crée une nouvelle tâche dans le projet spécifié.

### Mettre à jour une tâche du projet

**POST** `/api/projects/{id}/tasks/update`

Paramètre de chemin : `id` — ID du projet

Met à jour une tâche dans le projet spécifié.

### Supprimer une tâche du projet

**POST** `/api/projects/{id}/tasks/delete`

Paramètre de chemin : `id` — ID du projet

Supprime une tâche dans le projet spécifié.

### Attribuer un responsable de tâche

**POST** `/api/projects/{id}/tasks/assign`

Paramètre de chemin : `id` — ID du projet

Attribue un responsable à la tâche du projet.

### Retirer le responsable d'une tâche

**POST** `/api/projects/{id}/tasks/remove-assignee`

Paramètre de chemin : `id` — ID du projet

Retire le responsable de la tâche du projet.

### Marquer une tâche comme terminée

**POST** `/api/projects/{id}/tasks/complete`

Paramètre de chemin : `id` — ID du projet

Marque la tâche du projet comme terminée.

### Marquer une tâche comme échouée

**POST** `/api/projects/{id}/tasks/fail`

Paramètre de chemin : `id` — ID du projet

Marque la tâche du projet comme échouée.

### Annuler une tâche

**POST** `/api/projects/{id}/tasks/cancel`

Paramètre de chemin : `id` — ID du projet

Annule la tâche du projet.

---

## Gestion des autorisations d'outils

### Obtenir les autorisations d'outil d'un Être de Silicium

**GET** `/api/beings/tool-permissions`

Paramètre de requête : `beingId` — ID de l'Être de Silicium

Retourne la configuration des autorisations d'outil de l'Être de Silicium spécifié.

### Mettre à jour les autorisations d'outil d'un Être de Silicium

**PUT** `/api/beings/tool-permissions`

**Corps de la requête** :
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

### Obtenir les modèles d'autorisations d'outil

**GET** `/api/beings/tool-permissions/templates`

Retourne la liste des modèles d'autorisations d'outil disponibles.

### Appliquer un modèle d'autorisations d'outil

**POST** `/api/beings/tool-permissions/apply-template`

**Corps de la requête** :
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### Obtenir les autorisations d'outil d'un projet

**GET** `/api/projects/{id}/tool-permissions`

Paramètre de chemin : `id` — ID du projet

Retourne la configuration des autorisations d'outil du projet spécifié.

### Mettre à jour les autorisations d'outil d'un projet

**PUT** `/api/projects/{id}/tool-permissions`

Paramètre de chemin : `id` — ID du projet

**Corps de la requête** :
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

## Gestion des exécuteurs

### Page des exécuteurs

**GET** `/executor`

Retourne la page de l'interface de gestion des exécuteurs.

### Obtenir l'état des exécuteurs

**GET** `/api/executors/status`

Retourne l'état de fonctionnement de chaque exécuteur (disque, réseau, ligne de commande).

---

## Navigateur de code

### Page du navigateur de code

**GET** `/code`

Retourne la page de l'interface du navigateur de code.

### Obtenir la liste des types de code

**GET** `/api/code/types`

Retourne la liste des types/langages de code pris en charge.

### Obtenir les détails du code

**GET** `/api/code/detail`

Paramètres de requête : `filePath`, `lineNumber`

Retourne les détails du code du fichier spécifié.

---

## Info-bulles de code

### Obtenir les info-bulles

**GET** `/api/code/hover`
**POST** `/api/code/hover`

Obtient les informations d'info-bulle pour une position de code (similaire à l'IntelliSense des IDE).

### Enregistrer une position de code

**POST** `/api/code/register`

Enregistre une position de code à surveiller.

### Mettre à jour une position de code

**POST** `/api/code/update`

Met à jour les informations d'une position de code enregistrée.

### Désenregistrer une position de code

**POST** `/api/code/unregister`

Désenregistre la surveillance d'une position de code qui n'est plus nécessaire.

---

## Système de documentation d'aide

### Page d'aide

**GET** `/help` ou **GET** `/help/index`

Retourne la page principale de la documentation d'aide.

### Page de sujet d'aide

**GET** `/help/{topic}`

Paramètre de chemin : `topic` — Identifiant du sujet

Retourne la page de documentation d'aide pour le sujet spécifié.

### Rechercher dans la documentation d'aide

**GET** `/api/help/search`

Paramètre de requête : `keyword` — Mot-clé de recherche

Recherche les sujets de documentation d'aide correspondants.

---

## Initialisation

### Page de l'assistant d'initialisation

**GET** `/init`

Retourne la page de l'assistant d'initialisation pour la première exécution.

### Soumettre l'initialisation

**POST** `/init`

Soumet la configuration d'initialisation pour la première exécution.

### Parcourir pour sélectionner le répertoire de données

**GET** `/init/browse`

Ouvre le navigateur de répertoires pour sélectionner l'emplacement de stockage des données.

### Obtenir les métadonnées de configuration IA

**GET** `/init/ai-config-metadata`

Retourne les types de clients IA disponibles et les métadonnées de leurs champs de configuration.

---

## Contrôle système

### Arrêt gracieux

**POST** `/api/system/shutdown`

> **Note** : Seules les requêtes provenant de localhost sont autorisées

Déclenche le processus d'arrêt gracieux de l'application :

1. Arrêter la Boucle Principale (MainLoop)
2. Sauvegarder la configuration actuelle
3. Fermer l'écouteur HTTP

**Réponse** :
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## À propos

### Page À propos

**GET** `/about`

Retourne la page À propos, contenant les informations système et la liste des plugins chargés.

**Données de la liste des plugins** :
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

## Réponses d'erreur

Tous les points de terminaison retournent des réponses d'erreur standardisées :

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
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
| `SERVICE_UNAVAILABLE` | 503 | Service IA indisponible |

---

## Événements SSE

Les événements envoyés par le serveur sont utilisés pour les mises à jour en temps réel :

### Événements de chat

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

## Interface du client IA

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

## Interface du système d'outils

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

- 🚀 Consulter le [guide de démarrage rapide](getting-started.md)
- 🛠️ Lire le [guide de développement](development-guide.md)
- 📚 Consulter la [documentation d'architecture](architecture.md)
- 🔒 Comprendre le [modèle de sécurité](security.md)
