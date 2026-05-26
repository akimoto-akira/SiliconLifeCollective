# Système d'autorisations

> **Version : v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## Vue d'ensemble

Le système d'autorisations garantit que toutes les opérations initiées par l'IA sont correctement vérifiées et auditées.

## Chaîne de vérification des autorisations

```
┌─────────────────────────────────────────────┐
│          Vérification des autorisations      │
├─────────────────────────────────────────────┤
│  Niveau 1 : UserFrequencyCache              │
│  ↓ Cache des décisions fréquentes (HighDeny/HighAllow) │
│  Niveau 2 : IPermissionCallback             │
│  ↓ Logique personnalisée (Allowed/Denied/AskUser) │
│  Niveau 3 : IsCurateur ?                    │
│  ↓ Oui → IPermissionAskHandler (interroger l'utilisateur) │
│  ↓ Non → GlobalACL → refus par défaut       │
│  Résultat : Autorisé ou Refusé              │
└─────────────────────────────────────────────┘
```

> **Note** : La priorité de requête réelle de `PermissionManager.CheckPermission()` est :
> 1. **UserFrequencyCache** — Vérifie d'abord le cache des décisions fréquentes
> 2. **IPermissionCallback** — Évalue les règles de rappel personnalisées
> 3. **Branche Curateur** — Lorsque le rappel retourne AskUser ou qu'il n'y a pas de rappel :
>    - **Curateur** → `IPermissionAskHandler` (interroge l'utilisateur via IM)
>    - **Non-curateur** → `GlobalACL` → refus par défaut

## Niveau 1 : UserFrequencyCache

Cache des décisions fréquentes de l'utilisateur pour chaque être (HighDeny/HighAllow), existant uniquement en mémoire.

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny est prioritaire sur HighAllow**
- **Mémoire uniquement** : Le cache n'est pas persisté, il est perdu au redémarrage
- **Expiration configurable** : L'utilisateur peut définir la durée de validité des entrées du cache

## Niveau 2 : IPermissionCallback

Rappel personnalisé pour la logique d'autorisation dynamique.

### Implémentation par défaut DefaultPermissionCallback

`DefaultPermissionCallback` fournit des règles d'autorisation par défaut complètes, incluant :

#### Règles d'accès réseau
- **Adresse de bouclage** : Autorise localhost, 127.0.0.1, ::1
- **Adresses IP privées** :
  - 192.168.x.x (Classe C) - Autorisé
  - 10.x.x.x (Classe A) - Autorisé
  - 172.16-31.x.x (Classe B) - Interroger l'utilisateur
- **Liste blanche de domaines** :
  - Moteurs de recherche : Google, Bing, DuckDuckGo, Yandex, Sogou, etc.
  - Services IA : OpenAI, Anthropic, HuggingFace, Ollama, etc.
  - Services développeurs : GitHub, StackOverflow, npm, NuGet, etc.
  - Réseaux sociaux : Weibo, Zhihu, Reddit, Discord, etc.
  - Plateformes vidéo : YouTube, Bilibili, Douyin, TikTok, etc.
  - **Informations météo** : wttr.in
  - Sites gouvernementaux : .gov, .go.jp, .go.kr
- **Liste noire de domaines** :
  - Sites d'usurpation IA : chatgpt, openai, deepseek et autres domaines contrefaits
  - Outils IA malveillants : wormgpt, darkgpt, fraudgpt, etc.
  - Fermes de contenu IA et domaines liés au marché noir

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

## Niveau 3 : Branchement conditionnel (IsCurateur / GlobalACL)

Lorsque le rappel retourne `AskUser` ou qu'aucun rappel n'est configuré, le système effectue un branchement selon le statut de Curateur :

### Branche Curateur (IsCurateur = true)

Pour le Curateur de Silicium, le système demande une décision à l'utilisateur via la messagerie instantanée :

```csharp
if (IsCurateur)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // L'utilisateur confirme ou refuse dans l'interface Web UI
    }
}
```

### Branche Non-curateur (IsCurateur = false)

Pour les êtres non-curateurs, le système vérifie l'ACL Global. Si aucune règle ne correspond, la requête est refusée par défaut.

### Structure de l'ACL Global

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

Les règles sont évaluées dans l'ordre, la première règle correspondante prévaut. Seul le Curateur de Silicium peut modifier l'ACL Global.

### Format des ressources

```
{type}:{chemin}

Exemples :
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

Lorsqu'une opération du Curateur nécessite la confirmation de l'utilisateur, les autorisations sont demandées via `IPermissionAskHandler`.

### Implémentation IMPermissionAskHandler

`IMPermissionAskHandler` envoie une demande d'autorisation à l'utilisateur via l'interface Web UI :

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // Envoie un message à l'utilisateur via la messagerie instantanée
        SendMessageAsync($"Allow {resource}?");

        // Attend la réponse de l'utilisateur
        var response = WaitForResponseAsync();

        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### File de demandes d'autorisation PermissionRequestQueue

`PermissionRequestQueue` gère les demandes d'autorisation en attente, prenant en charge l'attente asynchrone de la réponse de l'utilisateur :

- **Mise en file d'attente** — Lorsque la chaîne d'autorisations atteint le niveau 5, un `TaskCompletionSource<AskPermissionResult>` est créé et mis en file
- **Affichage Web UI** — Les demandes en attente sont affichées dans l'interface Web UI via `PermissionRequestController`
- **Réponse de l'utilisateur** — L'utilisateur approuve ou refuse dans l'interface Web UI, avec possibilité de mettre en cache la décision et de définir la durée du cache
- **Options de cache** — L'utilisateur peut mettre en cache la décision d'autorisation pour 1 heure, 24 heures, 7 jours ou 30 jours
- **Mécanisme de timeout** — Fermeture automatique de la page de demande après 60 secondes sans réponse

## Système d'audit

Toutes les décisions d'autorisation sont enregistrées :

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

## Évaluation programmatique des autorisations

### API EvaluatePermission

La méthode `PermissionManager.EvaluatePermission()` fournit une pré-évaluation en lecture seule des autorisations, sans déclencher d'invite utilisateur. `PermissionTool` utilise cette méthode pour permettre à l'IA de vérifier l'état des autorisations avant de tenter une opération.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Valeur de retour** : `PermissionResult` à trois états :
- `Allowed` - L'opération est autorisée
- `Denied` - L'opération est refusée
- `AskUser` - Confirmation utilisateur requise à l'exécution

**Ordre d'évaluation** :
1. **Cache de fréquence** - Vérifie les décisions utilisateur en cache
2. **IPermissionCallback** - Évaluation par le rappel personnalisé
3. **Statut Curateur** - Si c'est le Curateur, retourne `AskUser` (confirmation requise)
4. **ACL Global** - Vérifie les règles de contrôle d'accès
5. **Par défaut** - Refus en l'absence de règle correspondante

> **Note** : Contrairement à la chaîne d'autorisations complète, `EvaluatePermission` **n'appelle pas** `IPermissionAskHandler`. Elle indique uniquement quel sera le résultat *à l'exécution*.

## Gérer les autorisations

### Accorder une autorisation

**Via l'interface Web UI** :
1. Naviguer vers **Gestion des autorisations**
2. Cliquer sur **Ajouter une règle**
3. Configurer :
   - Utilisateur
   - Ressource
   - Autoriser/Refuser
   - Durée

**Via l'API** :
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

### Révoquer une autorisation

Via la page de gestion des autorisations de l'interface Web UI.

### Consulter les autorisations

```bash
curl http://localhost:8080/api/permissions/list
```

## Système d'autorisations d'outils

En plus de la chaîne de vérification des autorisations au niveau des opérations, le système fournit un mécanisme de gestion des **autorisations d'outils**, pour contrôler quels outils les Êtres de Silicium peuvent utiliser.

### Autorisations d'outils à deux niveaux

Les autorisations d'outils sont divisées en deux niveaux :

1. **Niveau Être de Silicium** — Contrôle les opérations d'outils qu'un Être de Silicium individuel peut utiliser
2. **Niveau projet** — Contrôle les opérations d'outils disponibles dans l'espace projet, indépendamment des autorisations au niveau de l'Être de Silicium

### Configuration des autorisations d'outils

Chaque opération de chaque outil peut être configurée indépendamment comme autorisée ou refusée :

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

### Modèles d'autorisations

Le système fournit des modèles d'autorisations d'outils prédéfinis, applicables rapidement aux Êtres de Silicium :

- **readonly** — Autorisation en lecture seule (autorise les opérations de lecture, refuse les opérations d'écriture)
- **full** — Autorisation complète (autorise toutes les opérations)
- **restricted** — Autorisation restreinte (autorise uniquement les opérations de base)

### Gestion via l'interface Web UI

Gérer les autorisations d'outils via l'interface Web UI :

- **Page des autorisations d'outils de l'Être de Silicium** — `/beings/tool-permissions`
- **Page des autorisations d'outils du projet** — `/project/{id}/tool-permissions`

### Points de terminaison API

| Point de terminaison | Méthode | Description |
|------|------|------|
| `/api/beings/tool-permissions` | GET | Obtenir les autorisations d'outils de l'Être de Silicium |
| `/api/beings/tool-permissions` | PUT | Mettre à jour les autorisations d'outils de l'Être de Silicium |
| `/api/beings/tool-permissions/templates` | GET | Obtenir la liste des modèles d'autorisations |
| `/api/beings/tool-permissions/apply-template` | POST | Appliquer un modèle d'autorisations |
| `/api/projects/{id}/tool-permissions` | GET | Obtenir les autorisations d'outils du projet |
| `/api/projects/{id}/tool-permissions` | PUT | Mettre à jour les autorisations d'outils du projet |

---

## Bonnes pratiques

### 1. Principe du moindre privilège

N'accorder que les autorisations minimales nécessaires :

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. Utiliser des autorisations à durée limitée

Ne jamais accorder d'autorisations permanentes sauf nécessité absolue.

### 3. Surveiller les journaux d'autorisation

Consulter régulièrement les journaux d'audit pour comprendre :
- Les tentatives d'accès refusées
- Les schémas anormaux
- Les élévations d'autorisation

### 4. Implémenter des rappels personnalisés

Pour une logique complexe, utiliser `IPermissionCallback` :

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // Autorisation basée sur le temps
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // Autorisation basée sur la ressource
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## Scénarios courants

### Scénario 1 : L'IA veut lire un fichier

```
IA : "J'ai besoin de lire config.json"
↓
Chaîne d'autorisations :
1. UserFrequencyCache ? Pas de décision en cache
2. IPermissionCallback ? Retourne AskUser (non explicitement autorisé)
3. IsCurateur ? Non → Vérifier GlobalACL
4. GlobalACL ? Règle trouvée : file:... = Allowed
5. Résultat : Autorisé
```

### Scénario 2 : L'IA veut exécuter du code

```
IA : "Je veux compiler et exécuter du code"
↓
Chaîne d'autorisations :
1. UserFrequencyCache ? Pas de décision en cache
2. IPermissionCallback ? Retourne AskUser
3. IsCurateur ? Oui → IPermissionAskHandler
4. L'utilisateur approuve
5. Résultat : Autorisé
```

### Scénario 3 : Refus mis en cache

```
IA : "J'ai besoin d'accéder à C:\Windows"
↓
Chaîne d'autorisations :
1. UserFrequencyCache ? Trouvé dans le cache HighDeny
2. Résultat : Refusé (pas de vérification supplémentaire)
```

## Dépannage

### Autorisation inattendue refusée

**Vérifier** :
1. Le statut IsCurateur de l'utilisateur
2. Les entrées HighDeny dans le cache de fréquence
3. Les règles GlobalACL
4. La logique de rappel
5. Le timeout de réponse utilisateur

### Autorisation non expirée

**Vérifier** :
- Le champ `expiresAt` est correctement défini
- Le fuseau horaire est correct
- L'horloge est synchronisée

### Journal d'audit non enregistré

**Vérifier** :
- Le journal d'audit est enregistré
- Le backend de stockage est accessible
- L'espace disque est suffisant

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
- 🛠️ Consulter le [guide de développement](development-guide.md)
- 🔒 Consulter la [documentation de sécurité](security.md)
- 🚀 Consulter le [guide de démarrage rapide](getting-started.md)
