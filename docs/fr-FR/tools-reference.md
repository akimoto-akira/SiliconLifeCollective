# Référence des outils

> **Version : v0.2.0-alpha**

Ce document présente en détail tous les outils intégrés de la plateforme Silicon Life Collective.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | **Français** | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md)

## Aperçu

Le système d'outils permet aux Silicon Beings d'interagir avec le monde extérieur via une interface standardisée. Chaque outil implémente l'interface `ITool`, découvert et enregistré automatiquement par `ToolManager` via réflexion.

### Catégories d'outils

- **Outils d'administration système** — Configuration, permissions, compilation dynamique
- **Outils de communication** — Chat, requêtes réseau
- **Outils de stockage de données** — Opérations disque, base de données, mémoire, notes de travail
- **Outils de gestion du temps** — Calendrier, minuteurs, tâches
- **Outils de développement** — Exécution de code, consultation des journaux
- **Outils utilitaires** — Informations système, audit de tokens, documentation d'aide, réseau de connaissances
- **Outils de navigateur** — Automatisation de navigateur WebView
- **Outils de plugins** — Outils tiers enregistrés via le système de plugins

---

## Liste des outils intégrés

### 1. Outil de calendrier (CalendarTool)

**Nom de l'outil** : `calendar`

**Description** : Conversion et calcul de dates pour 32 systèmes de calendrier.

**Opérations prises en charge** :
- `now` — Obtenir l'heure actuelle
- `format` — Formater une date
- `add_days` — Ajouter/soustraire des jours
- `diff` — Calculer la différence entre deux dates
- `list_calendars` — Lister tous les calendriers pris en charge
- `get_components` — Obtenir les composants d'une date
- `get_now_components` — Obtenir les composants de la date actuelle
- `convert` — Convertir entre systèmes de calendrier

**Systèmes de calendrier pris en charge** (32) :
- Grégorien (Gregorian)
- Calendrier lunaire chinois (Chinese Lunar)
- Calendrier historique chinois (Chinese Historical) — Ère Ganzhi, ère impériale
- Calendrier islamique (Islamic)
- Calendrier hébraïque (Hebrew)
- Calendrier japonais (Japanese)
- Calendrier persan (Persian)
- Calendrier maya (Mayan)
- Calendrier bouddhiste (Buddhist)
- Calendrier tibétain (Tibetan)
- Et 24 autres calendriers...

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
- `read` — Lire un paramètre de configuration
- `write` — Écrire un paramètre de configuration
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

### 4. Outil de curateur (CuratorTool) 🔒

**Nom de l'outil** : `curator`

**Exigence de permission** : Réservé au Silicon Curator

**Description** : Outil d'administration système réservé au Silicon Curator.

**Opérations prises en charge** :
- `create_being` — Créer un nouveau Silicon Being
- `list_beings` — Lister tous les Silicon Beings
- `get_being_info` — Obtenir les informations d'un Being
- `assign_task` — Assigner une tâche
- `manage_permissions` — Gérer les permissions

**Exemple d'utilisation** :
```json
{
  "action": "create_being",
  "name": "Assistant",
  "soul_file": "assistant_soul.md"
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

### 6. Outil de disque (DiskTool)

**Nom de l'outil** : `disk`

**Description** : Opérations sur le système de fichiers et recherche locale.

**Opérations prises en charge** :
- `read` — Lire un fichier
- `write` — Écrire dans un fichier
- `list` — Lister un répertoire
- `delete` — Supprimer un fichier
- `create_directory` — Créer un répertoire
- `search_files` — Rechercher des fichiers
- `search_content` — Rechercher le contenu de fichiers
- `count_lines` — Compter les lignes
- `read_lines` — Lire des lignes spécifiques
- `replace_text` — Remplacer du texte

**Exigence de permission** : `disk:read`, `disk:write`

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

**Description** : Compilation dynamique de code C# (pour l'auto-évolution des Silicon Beings).

**Opérations prises en charge** :
- `compile_class` — Compiler une classe
- `compile_callback` — Compiler une fonction de rappel de permission
- `validate_code` — Valider la sécurité du code

**Mécanismes de sécurité** :
- Contrôle des références lors de la compilation (exclusion des assemblys dangereux)
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

**Exigence de permission** : Réservé au Silicon Curator

**Description** : Compiler et exécuter des extraits de code C#.

**Opérations prises en charge** :
- `run_script` — Exécuter un script de code

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

**Description** : Obtenir la documentation d'aide système et les guides d'utilisation.

**Opérations prises en charge** :
- `get_topics` — Obtenir la liste des sujets d'aide
- `get_topic` — Obtenir les détails d'un sujet spécifique
- `search` — Rechercher dans la documentation d'aide

**Exemple d'utilisation** :
```json
{
  "action": "get_topics"
}
```

---

### 10. Outil de réseau de connaissances (KnowledgeTool)

**Nom de l'outil** : `knowledge`

**Description** : Opérations sur le graphe de connaissances (basé sur des triplets : sujet-relation-objet).

**Opérations prises en charge** :
- `add` — Ajouter un triplet de connaissances
- `query` — Interroger les connaissances
- `update` — Mettre à jour les connaissances
- `delete` — Supprimer des connaissances
- `search` — Rechercher des connaissances
- `get_path` — Obtenir un chemin de connaissances
- `validate` — Valider les connaissances
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

### 11. Outil de journalisation (LogTool)

**Nom de l'outil** : `log`

**Description** : Consulter l'historique des opérations et des conversations.

**Opérations prises en charge** :
- `query_logs` — Consulter les journaux système
- `query_conversations` — Consulter l'historique des conversations
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

### 12. Outil de mémoire (MemoryTool)

**Nom de l'outil** : `memory`

**Description** : Gérer la mémoire à long terme et à court terme des Silicon Beings.

**Opérations prises en charge** :
- `read` — Lire la mémoire
- `write` — Écrire dans la mémoire
- `search` — Rechercher dans la mémoire
- `delete` — Supprimer la mémoire
- `list` — Lister la mémoire
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

### 13. Outil réseau (NetworkTool)

**Nom de l'outil** : `network`

**Description** : Effectuer des requêtes HTTP/HTTPS.

**Opérations prises en charge** :
- `get` — Requête GET
- `post` — Requête POST
- `put` — Requête PUT
- `delete` — Requête DELETE
- `download` — Télécharger un fichier
- `upload` — Téléverser un fichier

**Exigence de permission** : `network:http`

**Exemple d'utilisation** :
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. Outil de permissions (PermissionTool) 🔒

**Nom de l'outil** : `permission`

**Exigence de permission** : Réservé au Silicon Curator

**Description** : Gérer les permissions et les listes de contrôle d'accès.

**Opérations prises en charge** :
- `query_permission` — Interroger les permissions
- `manage_acl` — Gérer l'ACL globale
- `get_callback` — Obtenir la fonction de rappel de permission
- `set_callback` — Définir la fonction de rappel de permission

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

### 15. Outil de projet (ProjectTool)

**Nom de l'outil** : `project`

**Description** : Gérer les espaces de travail de projet.

**Opérations prises en charge** :
- `create` — Créer un projet
- `list` — Lister les projets
- `get_info` — Obtenir les informations du projet
- `update` — Mettre à jour le projet
- `archive` — Archiver le projet

**Exemple d'utilisation** :
```json
{
  "action": "create",
  "name": "Mon Projet",
  "description": "Description du projet"
}
```

---

### 16. Outil de tâches de projet (ProjectTaskTool)

**Nom de l'outil** : `project_task`

**Description** : Gérer les tâches de projet.

**Opérations prises en charge** :
- `create` — Créer une tâche
- `list` — Lister les tâches
- `update` — Mettre à jour une tâche
- `complete` — Compléter une tâche
- `get_stats` — Obtenir les statistiques des tâches

**Exemple d'utilisation** :
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Description de la tâche",
  "priority": 5
}
```

---

### 17. Outil de notes de travail de projet (ProjectWorkNoteTool)

**Nom de l'outil** : `project_work_note`

**Description** : Gérer les notes de travail de projet (publiques, similaires à un carnet de travail).

**Opérations prises en charge** :
- `create` — Créer une note
- `read` — Lire une note
- `update` — Mettre à jour une note
- `delete` — Supprimer une note
- `list` — Lister les notes
- `search` — Rechercher des notes
- `directory` — Générer un répertoire

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

### 18. Outil système (SystemTool)

**Nom de l'outil** : `system`

**Description** : Obtenir les informations système et l'utilisation des ressources.

**Opérations prises en charge** :
- `info` — Obtenir les informations système
- `resource_usage` — Obtenir l'utilisation des ressources
- `find_process` — Rechercher un processus
- `list_beings` — Lister les Silicon Beings

**Exemple d'utilisation** :
```json
{
  "action": "info"
}
```

---

### 19. Outil de tâches (TaskTool)

**Nom de l'outil** : `task`

**Description** : Gérer les tâches personnelles des Silicon Beings.

**Opérations prises en charge** :
- `create` — Créer une tâche
- `list` — Lister les tâches
- `update` — Mettre à jour une tâche
- `complete` — Compléter une tâche
- `delete` — Supprimer une tâche
- `get_dependencies` — Obtenir les dépendances

**Exemple d'utilisation** :
```json
{
  "action": "create",
  "description": "Réviser le code",
  "priority": 5
}
```

---

### 20. Outil de minuteur (TimerTool)

**Nom de l'outil** : `timer`

**Description** : Créer et gérer des minuteurs.

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

### 21. Outil d'audit de tokens (TokenAuditTool) 🔒

**Nom de l'outil** : `token_audit`

**Exigence de permission** : Réservé au Silicon Curator

**Description** : Consulter et résumer l'utilisation des tokens IA.

**Opérations prises en charge** :
- `get_usage` — Obtenir les statistiques d'utilisation des tokens
- `get_by_being` — Obtenir l'utilisation par Being
- `get_by_model` — Obtenir l'utilisation par modèle
- `get_trend` — Obtenir les tendances d'utilisation
- `export` — Exporter les données

**Exemple d'utilisation** :
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. Outil de navigateur WebView (WebViewBrowserTool)

**Nom de l'outil** : `webview`

**Description** : Automatisation de navigateur basée sur Playwright.

**Opérations prises en charge** :
- `open_browser` — Ouvrir le navigateur
- `close_browser` — Fermer le navigateur
- `navigate` — Naviguer vers une URL
- `click` — Cliquer sur un élément
- `input` — Saisir du texte
- `get_page_text` — Obtenir le texte de la page
- `get_screenshot` — Obtenir une capture d'écran
- `execute_script` — Exécuter du JavaScript
- `wait_for_element` — Attendre l'apparition d'un élément
- `get_browser_status` — Obtenir le statut du navigateur

**Caractéristiques** :
- Instance indépendante pour chaque Silicon Being
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

### 23. Outil de notes de travail (WorkNoteTool)

**Nom de l'outil** : `work_note`

**Description** : Gérer les notes de travail personnelles des Silicon Beings (privées, similaires à un journal).

**Opérations prises en charge** :
- `create` — Créer une note
- `read` — Lire une note
- `update` — Mettre à jour une note
- `delete` — Supprimer une note
- `list` — Lister les notes
- `search` — Rechercher des notes
- `directory` — Générer un répertoire

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

### 24. Outil de rechargement à chaud (HotReloadTool)

**Nom de l'outil**: `hot_reload`

**Description**: Prend en charge la compilation automatique, la mise à jour des fichiers et le redémarrage de SiliconLife.Fast pendant l'exécution, sans intervention manuelle.

**Opérations prises en charge**:
- `execute` — Exécute le processus complet de build, copie et redémarrage
- `build_only` — Build uniquement le projet, sans copie ni redémarrage

**Flux de travail**:
1. Compile le projet SiliconLife.Fast
2. Ferme gracieusement l'instance Fast en cours d'exécution (via API HTTP)
3. Attend la sortie du processus et la libération du port
4. Copie la sortie de build vers le répertoire cible (exclut les fichiers HotReload)
5. Redémarre l'instance Fast

**Caractéristiques**:
- Détection et fermeture automatiques de l'ancien processus
- Copie sécurisée des fichiers (ne écrase pas HotReload.exe)
- Mécanisme d'attente de libération de port
- Support de configuration de port personnalisé

**Exemple d'utilisation**:
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**Description des paramètres**:
- `project_path`: Chemin du projet (relatif au répertoire racine de la solution)
- `source_path`: Répertoire de sortie de build
- `configuration`: Configuration de build (Debug/Release)
- `port`: Port Web de l'instance Fast (par défaut 8080)

**Notes**:
- Applicable uniquement à la version SiliconLife.Fast
- Nécessite HotReload.exe dans le répertoire tools/HotReload
- Brève interruption de service pendant le redémarrage (environ 3-5 secondes)

## Flux d'appel d'outil

```
┌──────────┐
│   IA     │ Retourne tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Recherche et valide les droits d'utilisation
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Vérifie la chaîne de permissions
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Exécute l'opération d'accès aux ressources
└────┬─────────┘
     ↓
┌──────────┐
│   IA     │ Reçoit le résultat de l'outil, continue la réflexion
└──────────┘
```

## Vérification des permissions

Toutes les exécutions d'outils passent par la chaîne de permissions à 5 niveaux :

1. **IsCurator** — Le Silicon Curateur contourne toutes les vérifications
2. **UserFrequencyCache** — Cache des autorisations/refus fréquents de l'utilisateur
3. **GlobalACL** — Liste de contrôle d'accès globale
4. **IPermissionCallback** — Fonction de rappel de permission personnalisée
5. **IPermissionAskHandler** — Demander à l'utilisateur

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

Placer le fichier de l'outil dans le répertoire `src/SiliconLife.Common/Tools/` (outils partagés) ou `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (outils spécifiques à une version). `ToolManager` découvrira et enregistreront automatiquement l'outil via réflexion au démarrage.

### Étape 2a : Enregistrer un outil via un plugin

Il est également possible d'enregistrer des outils personnalisés via le système de plugins :

1. Implémenter l'interface `ITool` dans le projet du plugin
2. Compiler la DLL du plugin et la placer dans le répertoire des plugins
3. `ToolManager.ScanAllPluginAssemblies()` analysera automatiquement toutes les implémentations ITool dans les plugins chargés
4. Les outils de plugins sont soumis au même système de permissions

### Étape 3 : (Optionnel) Marquer comme réservé au curateur

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Seul le Silicon Curateur peut y accéder
}
```

## Meilleures pratiques

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
    Logger.Error($"Outil {Name} échoué : {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Respecter le système de permissions

Ne jamais contourner les vérifications de permissions. Toujours accéder aux ressources via les exécuteurs :

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. Fournir des descriptions d'outil claires

Aider l'IA à comprendre quand et comment utiliser l'outil :

```csharp
public string Description => 
    "Utilisé pour convertir des dates entre différents systèmes de calendrier." +
    "Nécessite les paramètres 'date', 'from_calendar' et 'to_calendar'.";
```

## Dépannage

### Outil non trouvé

**Problème** : L'IA tente d'appeler un outil qui n'existe pas.

**Solution** :
- Vérifier que le nom de l'outil correspond exactement
- Vérifier que le fichier de l'outil est dans le répertoire `Tools/`
- Reconstruire le projet (`dotnet build`)

### Permission refusée

**Problème** : L'exécution de l'outil échoue avec une erreur de permission.

**Solution** :
- Consulter les journaux d'audit des permissions
- Vérifier que le Silicon Being dispose des permissions requises
- Consulter les paramètres de l'ACL globale
- Si c'est un curateur, vérifier si l'attribut `[SiliconManagerOnly]` est utilisé

### L'exécution de l'outil retourne une erreur

**Problème** : L'outil s'exécute mais retourne un résultat d'échec.

**Solution** :
- Vérifier le message d'erreur retourné par l'outil
- Valider le format des paramètres d'entrée
- Consulter les journaux système pour des informations détaillées sur l'erreur
- Tester la fonctionnalité de l'outil indépendamment

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
- 🛠️ Consulter le [guide de développement](development-guide.md)
- 🔒 Comprendre le [système de permissions](permission-system.md)
- 🚀 Consulter le [guide de démarrage rapide](getting-started.md)
