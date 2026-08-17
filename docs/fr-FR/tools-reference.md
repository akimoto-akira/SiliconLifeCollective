# Référence des outils

> **Version : v0.2.0-alpha**

Ce document présente en détail tous les outils intégrés de la plateforme Silicon Life Collective.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Русский](../ru-RU/tools-reference.md)

## Vue d'ensemble

Le système d'outils permet aux Êtres de Silicium d'interagir avec le monde extérieur via une interface standardisée. Chaque outil implémente l'interface `ITool`, découverte et enregistrée automatiquement par `ToolManager` via réflexion.

### Catégories d'outils

- **Outils de gestion système** — Configuration, autorisations, compilation dynamique, gestion du Curateur
- **Outils de communication** — Chat, requêtes réseau
- **Outils de stockage de données** — Opérations disque, base de données, mémoire, notes de travail
- **Outils de gestion du temps** — Calendrier, minuteurs, tâches
- **Outils de développement** — Exécution de code, consultation de journaux
- **Outils utilitaires** — Informations système, audit de tokens, documentation d'aide, Réseau de Connaissances
- **Outils de navigateur** — Automatisation de navigateur WebView
- **Outils de projet** — Gestion de projet, tâches de projet, notes de travail de projet, travail de projet
- **Outils de plugins** — Outils tiers enregistrés via le système de plugins

### Système de scénarios d'outils

Chaque outil déclare ses scénarios disponibles via l'attribut `[ToolScenario]` :

| Indicateur de scénario | Valeur | Description |
|--------------------------|------|-------------|
| `Chat` | `1 << 0` | Scénario de chat (lorsque l'utilisateur converse avec un Être de Silicium) |
| `Task` | `1 << 1` | Scénario de tâche (lorsqu'un Être de Silicium exécute une tâche) |
| `Timer` | `1 << 2` | Scénario de minuteur (lorsqu'un Être de Silicium exécute une tâche planifiée) |
| `MemoryCompression` | `1 << 3` | Scénario de compression de mémoire |
| `Project` | `1 << 4` | Scénario de projet (mode ThinkOnProject) |
| `All` | Tous les ci-dessus | Disponible dans tous les scénarios |

De plus, les outils marqués avec l'attribut `[ChatOnly]` ne sont disponibles que dans le scénario de chat (comme HelpTool), et n'apparaissent pas dans les scénarios de tâche et de minuteur.

---

## Liste des outils intégrés

### 1. Outil calendrier (CalendarTool)

**Nom de l'outil** : `calendar`

**Description** : Conversion et calcul de dates prenant en charge 32 systèmes calendaires.

**Opérations prises en charge** :
- `now` — Obtenir l'heure actuelle
- `format` — Formater une date
- `add_days` — Ajouter/soustraire des jours
- `diff` — Calculer la différence entre deux dates
- `list_calendars` — Lister tous les calendriers pris en charge
- `get_components` — Obtenir les composants d'une date
- `get_now_components` — Obtenir les composants de l'heure actuelle
- `convert` — Convertir entre systèmes calendaires

**Systèmes calendaires pris en charge** (32) :
- Grégorien (Gregorian)
- Lunaire chinois (Chinese Lunar)
- Historique chinois (Chinese Historical) — Numération Ganzhi, ères impériales
- Islamique (Islamic)
- Hébraïque (Hebrew)
- Japonais (Japanese)
- Persan (Persian)
- Maya (Mayan)
- Bouddhiste (Buddhist)
- Tibétain (Tibetan)
- Et 22 autres calendriers...

**Exemple d'utilisation** :
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. Outil de chat (ChatTool)

**Nom de l'outil** : `chat`

**Description** : Gestion des sessions de chat et envoi de messages.

**Opérations prises en charge** :
- `send_message` — Envoyer un message
- `get_messages` — Obtenir l'historique des messages
- `create_group` — Créer un chat de groupe
- `add_member` — Ajouter un membre au groupe
- `remove_member` — Retirer un membre du groupe
- `get_chat_info` — Obtenir les informations du chat
- `terminate_chat` — Terminer le chat (lu sans réponse)

**Exemple d'utilisation** :
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "Bonjour, collaborons !"
}
```

---

### 3. Outil de configuration (ConfigTool)

**Nom de l'outil** : `config`

**Description** : Lecture et modification de la configuration système.

**Opérations prises en charge** :
- `read` — Lire un élément de configuration
- `write` — Écrire un élément de configuration
- `list` — Lister toutes les configurations
- `get_ai_config` — Obtenir la configuration du client IA
- `set_ai_config` — Définir la configuration du client IA

**Exemple d'utilisation** :
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Outil du Curateur (CuratorTool) 🔒

**Nom de l'outil** : `silicon_manager`

**Exigence d'autorisation** : Réservé au Curateur de Silicium (`[SiliconManagerOnly]`)

**Scénarios disponibles** : Chat, Task, Timer

**Description** : Outil de gestion système réservé au Curateur de Silicium, pour gérer la création, la consultation et la réinitialisation des Êtres de Silicium.

**Opérations prises en charge** :
- `list_beings` — Lister tous les Êtres de Silicium et leur état
- `create_being` — Créer un nouvel Être de Silicium (nécessite les paramètres `name` et `soul`)
- `get_code` — Consulter le code source personnalisé d'un Être de Silicium
- `reset` — Réinitialiser un Être de Silicium à l'implémentation par défaut

**Exemple d'utilisation** :
```json
{
  "action": "create_being",
  "name": "Assistant",
  "soul": "Tu es un assistant utile..."
}
```

---

### 5. Outil de base de données (DatabaseTool)

**Nom de l'outil** : `database`

**Description** : Requêtes et opérations de base de données structurée.

**Opérations prises en charge** :
- `query` — Interroger des données
- `insert` — Insérer des données
- `update` — Mettre à jour des données
- `delete` — Supprimer des données
- `create_table` — Créer une table
- `list_tables` — Lister toutes les tables

**Exemple d'utilisation** :
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. Outil disque (DiskTool)

**Nom de l'outil** : `disk`

**Description** : Opérations sur le système de fichiers et recherche locale.

**Opérations prises en charge** :
- `read` — Lire un fichier
- `write` — Écrire un fichier
- `list` — Lister un répertoire
- `delete` — Supprimer un fichier
- `create_directory` — Créer un répertoire
- `search_files` — Rechercher des fichiers
- `search_content` — Rechercher dans le contenu de fichiers
- `count_lines` — Compter les lignes
- `read_lines` — Lire des lignes spécifiques
- `replace_text` — Remplacer du texte

**Exigence d'autorisation** : `FileAccess`

**Exemple d'utilisation** :
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Outil de compilation dynamique (DynamicCompileTool) 🔒

**Nom de l'outil** : `compile`

**Description** : Compilation dynamique de code C# (pour l'auto-évolution des Êtres de Silicium).

**Opérations prises en charge** :
- `compile_class` — Compiler une classe
- `compile_callback` — Compiler une fonction de rappel d'autorisation
- `validate_code` — Valider la sécurité du code

**Mécanismes de sécurité** :
- Contrôle des références à la compilation (exclusion des assemblages dangereux)
- Analyse statique du code à l'exécution
- Stockage chiffré AES-256

**Exemple d'utilisation** :
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Outil d'exécution de code (ExecuteCodeTool) 🔒

**Nom de l'outil** : `execute_code`

**Exigence d'autorisation** : Réservé au Curateur de Silicium

**Description** : Compilation et exécution de fragments de code C#.

**Opérations prises en charge** :
- `run_script` — Exécuter un script

**Exemple d'utilisation** :
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. Outil d'aide (HelpTool)

**Nom de l'outil** : `help`

**Scénarios disponibles** : Chat (`[ChatOnly]`, disponible uniquement dans le scénario de chat)

**Description** : Recherche et consultation du contenu de la documentation d'aide du système, permettant à l'IA de consulter les méthodes d'utilisation des fonctionnalités système.

**Opérations prises en charge** :
- `list` — Lister tous les identifiants de thèmes d'aide
- `search` — Rechercher dans la documentation d'aide par mots-clés
- `get` — Obtenir le contenu de la documentation d'aide pour un identifiant donné

**Exemple d'utilisation** :
```json
{
  "action": "search",
  "keyword": "autorisation"
}
```

---

### 10. Outil du Réseau de Connaissances (KnowledgeTool)

**Nom de l'outil** : `knowledge`

**Description** : Opérations sur le graphe de connaissances (basé sur des triplets : sujet-relation-objet).

**Opérations prises en charge** :
- `add` — Ajouter un triplet de connaissance
- `query` — Interroger des connaissances
- `update` — Mettre à jour des connaissances
- `delete` — Supprimer des connaissances
- `search` — Rechercher des connaissances
- `get_path` — Obtenir un chemin de connaissance
- `validate` — Valider des connaissances
- `stats` — Obtenir des statistiques

**Exemple d'utilisation** :
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95
}
```

---

### 11. Outil de requête MCP (McpTool)

**Nom de l'outil** : `mcp`

**Description** : Requête sur l'état d'intégration MCP (Model Context Protocol) — serveurs externes connectés, les outils qu'ils fournissent et comment les appeler. Outil en lecture seule : l'ajout/suppression de serveurs ne peut être effectué que par l'utilisateur via l'interface Web UI, l'IA ne peut pas modifier la liste des serveurs.

**Opérations prises en charge** :
- `status` — Vue d'ensemble globale (état d'activation, nombre de serveurs, nombre d'outils)
- `list_servers` — Lister les serveurs configurés (avec état de connexion et nombre d'outils)
- `list_tools` — Lister les outils disponibles (avec préfixe `mcp_{server}_{tool}`, description et schéma des paramètres ; `server_id` optionnel pour filtrer un seul serveur)

**Exemple d'utilisation** :
```json
{
  "action": "list_tools",
  "server_id": "filesystem",
  "include_schema": true
}
```

**Outil wrapper MCP** : Chaque outil fourni par un serveur MCP connecté est enregistré dynamiquement comme un outil indépendant dans les Êtres de Silicium, avec le format de nommage `mcp_{serverId}_{toolName}` (ex. `mcp_filesystem_read_file`). L'IA peut les appeler directement par nom comme des outils ordinaires, sans passer par cet outil de requête. L'outil wrapper présente une seule action `execute` dans la matrice d'autorisations et peut être désactivé individuellement.

**Scénario** : Tous les scénarios (`All`)

---

### 12. Outil de journal (LogTool)

**Nom de l'outil** : `log`

**Description** : Consultation de l'historique des opérations et des conversations.

**Opérations prises en charge** :
- `query_logs` — Interroger les journaux système
- `query_conversations` — Interroger l'historique des conversations
- `get_stats` — Obtenir les statistiques des journaux

**Exemple d'utilisation** :
```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-26T23:59:59Z",
  "level": "info"
}
```

---

### 13. Outil de mémoire (MemoryTool)

**Nom de l'outil** : `memory`

**Description** : Gestion de la mémoire à long terme et à court terme des Êtres de Silicium.

**Opérations prises en charge** :
- `read` — Lire la mémoire
- `write` — Écrire dans la mémoire
- `search` — Rechercher dans la mémoire
- `delete` — Supprimer de la mémoire
- `list` — Lister les mémoires
- `get_stats` — Obtenir les statistiques de la mémoire
- `compress` — Compresser la mémoire

**Exemple d'utilisation** :
```json
{
  "action": "read",
  "key": "important_fact",
  "time_range": {
    "start": "2026-04-01",
    "end": "2026-04-26"
  }
}
```

---

### 14. Outil réseau (NetworkTool)

**Nom de l'outil** : `network`

**Description** : Requêtes HTTP/HTTPS.

**Opérations prises en charge** :
- `get` — Requête GET
- `post` — Requête POST
- `put` — Requête PUT
- `delete` — Requête DELETE
- `download` — Télécharger un fichier
- `upload` — Téléverser un fichier

**Exigence d'autorisation** : `network:http`

**Exemple d'utilisation** :
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 15. Outil d'autorisations (PermissionTool) 🔒

**Nom de l'outil** : `permission`

**Exigence d'autorisation** : Réservé au Curateur de Silicium

**Description** : Gestion des autorisations et des listes de contrôle d'accès.

**Opérations prises en charge** :
- `query_permission` — Interroger les autorisations
- `manage_acl` — Gérer l'ACL Global
- `get_callback` — Obtenir la fonction de rappel d'autorisation
- `set_callback` — Définir la fonction de rappel d'autorisation

**Exemple d'utilisation** :
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow"
}
```

---

### 16. Outil de projet (ProjectTool) 🔒

**Nom de l'outil** : `project`

**Exigence d'autorisation** : Réservé au Curateur de Silicium (`[SiliconManagerOnly]`)

**Scénarios disponibles** : Chat, Task, Timer

**Description** : Gestion des espaces projet, prenant en charge la gestion du cycle de vie des projets, l'attribution de membres et la gestion des rôles.

**Opérations prises en charge** :
- `create` — Créer un nouvel espace projet
- `archive` — Archiver un projet
- `restore` — Restaurer un projet archivé
- `destroy` — Détruire un projet et nettoyer les données (irréversible)
- `list` — Lister tous les projets
- `get` — Obtenir les détails d'un projet
- `assign` — Assigner un Être de Silicium à un projet
- `remove` — Retirer un Être de Silicium d'un projet
- `update` — Mettre à jour le nom/la description du projet
- `list-workflow-templates` — Lister les modèles de flux de travail disponibles
- `assign_role` — Attribuer un rôle de projet à un Être de Silicium
- `remove_role` — Retirer le rôle de projet d'un Être de Silicium
- `list_roles` — Lister les attributions de rôles du projet

**Exemple d'utilisation** :
```json
{
  "action": "create",
  "name": "Mon Projet",
  "description": "Description du projet"
}
```

---

### 17. Outil de tâches de projet (ProjectTaskTool)

**Nom de l'outil** : `project_task`

**Scénarios disponibles** : Chat, Task, Timer

**Description** : Gestion des tâches dans l'espace projet, prenant en charge le cycle de vie complet des tâches.

**Opérations prises en charge** :
- `create` — Créer une tâche de projet
- `list` — Lister les tâches du projet
- `get` — Obtenir les détails d'une tâche
- `update` — Mettre à jour le titre/la description/la priorité d'une tâche
- `assign` — Assigner un responsable à une tâche
- `remove_assignee` — Retirer le responsable d'une tâche
- `start` — Démarrer une tâche
- `complete` — Marquer une tâche comme terminée
- `fail` — Marquer une tâche comme échouée
- `cancel` — Annuler une tâche
- `delete` — Supprimer une tâche
- `stats` — Obtenir les statistiques des tâches

**Exemple d'utilisation** :
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Description de la tâche à accomplir",
  "priority": 5
}
```

---

### 18. Outil de notes de travail de projet (ProjectWorkNoteTool)

**Nom de l'outil** : `project_work_note`

**Scénarios disponibles** : Chat, Task, Timer

**Description** : Gestion des notes de travail dans l'espace projet (publiques, similaires à un carnet de travail), prenant en charge la gestion de notes par pages.

**Opérations prises en charge** :
- `create` — Créer une page de note (nécessite `project_id`, `summary` et `content`, `keywords` facultatif)
- `read` — Lire une page de note (nécessite `project_id` et `page_number` ou `note_id`)
- `update` — Mettre à jour une page de note (nécessite `project_id`, `page_number` et `content`, `summary` et `keywords` facultatifs)
- `delete` — Supprimer une page de note (nécessite `project_id` et `page_number` ou `note_id`)
- `list` — Lister les résumés de toutes les pages de notes du projet
- `directory` — Générer un sommaire/aperçu des notes
- `search` — Rechercher des notes par mots-clés (nécessite `project_id` et `keyword`, `max_results` facultatif)

**Exemple d'utilisation** :
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Module d'authentification utilisateur terminé",
  "content": "## Détails d'implémentation\n\n- Utilisation de JWT token",
  "keywords": "authentification,JWT"
}
```

---

### 19. Outil de travail de projet (ProjectWorkTool) 🔒

**Nom de l'outil** : `project_work`

**Exigence d'autorisation** : Réservé au Curateur de Silicium (`[SiliconManagerOnly]`)

**Scénarios disponibles** : Project (`[ToolScenario(ToolScenarioFlag.Project)]`, disponible uniquement dans le scénario de projet)

**Description** : Outil d'opérations de travail de projet, utilisé par le Curateur pour gérer les flux de travail de projet dans le scénario ThinkOnProject.

**Opérations prises en charge** :
- `create-task` — Créer une tâche de projet
- `assign-task` — Assigner un Être de Silicium à une tâche
- `chat` — Envoyer un message au chat de groupe du projet
- `broadcast` — Diffuser un message sur le canal du projet
- `complete` — Marquer le projet comme terminé
- `status` — Obtenir l'état du projet

**Exemple d'utilisation** :
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "Implémenter l'authentification utilisateur"
}
```

---

### 20. Outil de Compétence (SkillTool)

**Nom de l'outil** : `skill`

**Description** : Gestion des compétences des Êtres de Silicium (unité de capacité réutilisable "orchestration d'outils + modèle de prompt"), prend en charge la création, la liste, la mise à jour, la suppression, l'importation et l'exportation. Les métadonnées manquantes (id, description, schéma des paramètres, etc.) sont automatiquement complétées par l'IA.

**Opérations prises en charge** :
- `create` — Créer une nouvelle compétence (requiert `id` et `system_prompt` ; optionnels : `description`, `parameter_schema`, `tool_whitelist`, `tags`, `max_tool_round`, `timeout`, `on_complete`, `trigger_mode`, `auto_trigger_condition`)
- `list` — Lister toutes les compétences disponibles (avec résumé)
- `update` — Mettre à jour une compétence existante via paramètres (requiert `skill_id`)
- `update_from_md` — Mettre à jour une compétence à partir d'une chaîne Markdown (métadonnées YAML frontmatter + corps du prompt)
- `delete` — Supprimer une compétence (requiert `skill_id`)
- `export` — Exporter une compétence en JSON (requiert `skill_id`)
- `export_md` — Exporter une compétence en Markdown (requiert `skill_id`)
- `import` — Importer une compétence depuis JSON (requiert `json`)
- `import_md` — Importer une compétence depuis Markdown (requiert `markdown`)

**Exemple d'utilisation** :
```json
{
  "action": "create",
  "id": "daily_news_digest",
  "description": "Rechercher les actualités technologiques du jour et générer un résumé",
  "system_prompt": "Utilisez l'outil network pour rechercher les dernières actualités sur {topic} et générez un résumé de 500 mots.",
  "parameter_schema": {
    "type": "object",
    "properties": {
      "topic": { "type": "string", "description": "Sujet des actualités" }
    },
    "required": ["topic"]
  },
  "tool_whitelist": ["network", "work_note"],
  "trigger_mode": "Auto",
  "auto_trigger_condition": "schedule",
  "metadata": { "schedule": "0 9 * * *" }
}
```

**Autorisations de modification** : Le Curateur de Silicium peut modifier toutes les compétences ; les Êtres ordinaires ne peuvent modifier que les compétences dont l'origine est `Being` ou `User` (pas les compétences intégrées et de plugins).

**Limite de quantité** : Le nombre de compétences personnalisées par être est limité par la configuration `MaxCustomSkillsPerBeing` (par défaut 50).

**Scénario** : Tous les scénarios (`All`)

> Pour la documentation complète du système de compétences (modes de déclenchement, liste blanche, rechargement à chaud, planification automatique, etc.), consultez le [Guide de l'Être de Silicium](silicon-being-guide.md#système-de-compétences).

---

### 21. Outil système (SystemTool)

**Nom de l'outil** : `system`

**Description** : Obtenir les informations système et l'utilisation des ressources.

**Opérations prises en charge** :
- `info` — Obtenir les informations système
- `resource_usage` — Obtenir l'utilisation des ressources
- `find_process` — Rechercher un processus
- `list_beings` — Lister les Êtres de Silicium

**Exemple d'utilisation** :
```json
{
  "action": "info"
}
```

---

### 22. Outil de tâches (TaskTool)

**Nom de l'outil** : `task`

**Description** : Gestion des tâches personnelles des Êtres de Silicium.

**Opérations prises en charge** :
- `create` — Créer une tâche
- `list` — Lister les tâches
- `update` — Mettre à jour une tâche
- `complete` — Terminer une tâche
- `delete` — Supprimer une tâche
- `get_dependencies` — Obtenir les dépendances

**Exemple d'utilisation** :
```json
{
  "action": "create",
  "description": "Revue de code",
  "priority": 5
}
```

---

### 23. Outil de minuteurs (TimerTool)

**Nom de l'outil** : `timer`

**Description** : Création et gestion de minuteurs.

**Opérations prises en charge** :
- `create` — Créer un minuteur
- `list` — Lister les minuteurs
- `delete` — Supprimer un minuteur
- `pause` — Mettre en pause un minuteur
- `resume` — Reprendre un minuteur
- `get_execution_history` — Obtenir l'historique d'exécution

**Exemple d'utilisation** :
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "Rappel horaire"
}
```

---

### 24. Outil d'audit de tokens (TokenAuditTool) 🔒

**Nom de l'outil** : `token_audit`

**Exigence d'autorisation** : Réservé au Curateur de Silicium (`[SiliconManagerOnly]`)

**Scénarios disponibles** : Chat, Task, Timer

**Description** : Consultation des statistiques et tendances d'utilisation des tokens IA.

**Opérations prises en charge** :
- `summary` — Obtenir les statistiques récapitulatives de l'utilisation des tokens
- `trend` — Obtenir les points de données de tendance d'utilisation des tokens

**Plages de temps prises en charge** :
- `today` — Les dernières 24 heures
- `week` — Les dernières 7×24 heures
- `month` — Statistiques par jour
- `year` — Statistiques par mois

**Exemple d'utilisation** :
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 25. Outil de navigateur WebView (WebViewBrowserTool)

**Nom de l'outil** : `webview_browser`

**Scénarios disponibles** : Chat, Task, Timer

**Description** : Opérations d'automatisation de navigateur basées sur Playwright, fournissant des capacités complètes de navigation web, d'interaction et d'extraction de données.

**Opérations prises en charge** :
- `open` — Ouvrir le navigateur
- `close` — Fermer le navigateur
- `navigate` — Naviguer vers une URL
- `click` — Cliquer sur un élément
- `input` — Saisir du texte
- `scroll` — Faire défiler la page
- `execute_script` — Exécuter du JavaScript
- `get_page_text` — Obtenir le texte de la page
- `get_screenshot` — Obtenir une capture d'écran
- `wait_for_element` — Attendre l'apparition d'un élément
- `get_element_info` — Obtenir les informations d'un élément
- `upload_file` — Téléverser un fichier
- `get_browser_status` — Obtenir l'état du navigateur
- `set_timeout` — Définir le délai d'attente
- `clear_session` — Effacer la session du navigateur

**Caractéristiques** :
- Instance indépendante pour chaque Être de Silicium
- Cookies et sessions entièrement isolés
- Totalement invisible pour l'utilisateur (mode headless)
- Support complet de JavaScript et CSS

**Exemple d'utilisation** :
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 26. Outil de notes de travail (WorkNoteTool)

**Nom de l'outil** : `work_note`

**Description** : Gestion des notes de travail personnelles des Êtres de Silicium (privées, similaires à un journal).

**Opérations prises en charge** :
- `create` — Créer une note
- `read` — Lire une note
- `update` — Mettre à jour une note
- `delete` — Supprimer une note
- `list` — Lister les notes
- `search` — Rechercher des notes
- `directory` — Générer un sommaire

**Exemple d'utilisation** :
```json
{
  "action": "create",
  "summary": "Module d'authentification utilisateur terminé",
  "content": "## Détails d'implémentation\n\n- Utilisation de JWT token\n- Support OAuth2",
  "keywords": "authentification,JWT,OAuth2"
}
```

---

## Flux d'appel d'outils

```
┌──────────┐
│   IA     │ Retourne tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Recherche et validation des droits d'utilisation de l'outil
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Vérifie la chaîne d'autorisations
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Exécuteur   │ Exécute les opérations d'accès aux ressources
└────┬─────────┘
     ↓
┌──────────┐
│   IA     │ Reçoit les résultats de l'outil, continue la réflexion
└──────────┘
```

## Vérification des autorisations

Toutes les exécutions d'outils passent par la chaîne de vérification des autorisations :

1. **UserFrequencyCache** — Cache des décisions fréquentes de l'utilisateur (HighDeny prioritaire sur HighAllow)
2. **IPermissionCallback** — Fonction de rappel d'autorisation personnalisée (Allowed/Denied/AskUser)
3. **Branche IsCurateur** — Le Curateur interroge l'utilisateur via IPermissionAskHandler ; les non-curateurs consultent le GlobalACL, refus par défaut en l'absence de règle correspondante

## Créer un outil personnalisé

### Étape 1 : Implémenter l'interface ITool

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "Description de l'outil";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "Description du paramètre" }
        }
    };
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        try
        {
            var param1 = call.Parameters["param1"]?.ToString();
            var result = await DoWork(param1);
            
            return new ToolResult
            {
                Success = true,
                Output = result
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}
```

### Étape 2 : Ajouter au projet

Placer le fichier de l'outil dans le répertoire `src/SiliconLife.Common/Tools/` (outils partagés) ou dans `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (outils spécifiques à une version). `ToolManager` découvrira et enregistrera automatiquement l'outil par réflexion au démarrage.

### Étape 2a : Enregistrer un outil via un plugin

Il est également possible d'enregistrer des outils personnalisés via le système de plugins :

1. Implémenter l'interface `ITool` dans le projet du plugin
2. Compiler la DLL du plugin et la placer dans le répertoire des plugins
3. `ToolManager.ScanAllPluginAssemblies()` scanne automatiquement toutes les implémentations ITool dans les plugins chargés
4. Les outils de plugin sont soumis au même système d'autorisations

### Étape 3 : (Facultatif) Marquer comme réservé au Curateur

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Seul le Curateur de Silicium peut y accéder
}
```

### Alternative : Outils Compétence et MCP

Outre l'écriture de classes d'outils en C#, il existe deux modes d'extension sans compilation :

- **Compétence (Skill)** : Créer une combinaison "orchestration d'outils + modèle de prompt" via l'interface Web UI ou l'outil `skill`, adaptée pour consolider les flux de travail fréquents en capacités réutilisables. Consultez le [Guide de l'Être de Silicium — Système de Compétences](silicon-being-guide.md#système-de-compétences).
- **Serveur MCP** : Après avoir configuré un serveur MCP externe dans l'interface Web UI, ses outils sont automatiquement injectés au format `mcp_{serverId}_{toolName}`, sans écrire de code. Consultez le [Guide Web UI — Gestion MCP](web-ui-guide.md).

## Bonnes pratiques

### 1. Toujours valider les paramètres

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Paramètre requis manquant : required_param");
}
```

### 2. Gérer les erreurs avec élégance

```csharp
try
{
    // Exécuter l'opération
}
catch (Exception ex)
{
    Logger.Error($"Échec de l'exécution de l'outil {Name} : {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Respecter le système d'autorisations

Ne jamais contourner les vérifications d'autorisations. Toujours accéder aux ressources via les exécuteurs :

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
}
```

### 4. Fournir des descriptions d'outil claires

Aider l'IA à comprendre quand et comment utiliser l'outil :

```csharp
public string Description => 
    "Utilisé pour convertir des dates entre différents systèmes calendaires." +
    "Nécessite les paramètres 'date', 'from_calendar' et 'to_calendar'.";
```

## Dépannage

### Outil introuvable

**Problème** : L'IA tente d'appeler un outil qui n'existe pas.

**Solution** :
- Vérifier que le nom de l'outil correspond exactement
- Vérifier que le fichier de l'outil est dans le répertoire `Tools/`
- Reconstruire le projet (`dotnet build`)

### Autorisation refusée

**Problème** : L'exécution de l'outil échoue, renvoyant une erreur d'autorisation.

**Solution** :
- Consulter les journaux d'audit des autorisations
- Vérifier que l'Être de Silicium dispose des autorisations requises
- Consulter les paramètres de l'ACL Global
- Si c'est le Curateur, vérifier si l'attribut `[SiliconManagerOnly]` est utilisé

### L'exécution de l'outil renvoie une erreur

**Problème** : L'outil s'exécute mais renvoie un résultat d'échec.

**Solution** :
- Consulter le message d'erreur renvoyé par l'outil
- Vérifier que le format des paramètres d'entrée est correct
- Consulter les journaux système pour des informations détaillées sur l'erreur
- Tester la fonctionnalité de l'outil indépendamment

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
- 🛠️ Consulter le [guide de développement](development-guide.md)
- 🔒 Comprendre le [système d'autorisations](permission-system.md)
- 🚀 Consulter le [guide de démarrage rapide](getting-started.md)
