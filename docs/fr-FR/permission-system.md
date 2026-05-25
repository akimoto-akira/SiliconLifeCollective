# Système de permissions

> **Version : v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | **Français** | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## Aperçu

Le système de permissions garantit que toutes les opérations initiées par l'IA sont correctement vérifiées et auditées.

## Chaîne de permissions à 5 niveaux

```
┌─────────────────────────────────────────────┐
│          Vérification des permissions        │
├─────────────────────────────────────────────┤
│  Niveau 1 : UserFrequencyCache              │
│  ↓ Décisions utilisateur en cache (HighDeny/HighAllow) │
│  Niveau 2 : IPermissionCallback             │
│  ↓ Logique personnalisée (Allowed/Denied/AskUser) │
│  Niveau 3 : IsCurator ?                     │
│  ↓ Oui → IPermissionAskHandler (demander à l'utilisateur) │
│  ↓ Non → GlobalACL → Refus par défaut       │
│  Résultat : Autorisé ou Refusé              │
└─────────────────────────────────────────────┘
```

> **Note** : La priorité réelle d'interrogation dans `PermissionManager.CheckPermission()` est :
> 1. **UserFrequencyCache** — Vérifier d'abord les décisions utilisateur à haute fréquence en cache
> 2. **IPermissionCallback** — Évaluer les règles de rappel personnalisées
> 3. **Branche curateur** — Si le rappel retourne AskUser ou pas de rappel :
>    - **Curateur** → `IPermissionAskHandler` (demander à l'utilisateur via messagerie instantanée)
>    - **Non-curateur** → `GlobalACL` → refus par défaut

## Niveau 1 : UserFrequencyCache

Cache par Being, en mémoire uniquement, des décisions utilisateur à haute fréquence (HighDeny/HighAllow).

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny** a priorité sur **HighAllow**
- **Mémoire uniquement** : Les caches ne sont pas persistés, perdus au redémarrage
- **Expiration configurable** : Les utilisateurs peuvent définir la durée de validité des entrées du cache

## Niveau 2 : IPermissionCallback

Rappels personnalisés pour la logique de permissions dynamique.

### Implémentation par défaut DefaultPermissionCallback

`DefaultPermissionCallback` fournit des règles de permissions par défaut complètes, incluant :

#### Règles d'accès réseau
- **Adresses de bouclage** : Autoriser localhost, 127.0.0.1, ::1
- **Adresses IP privées** :
  - 192.168.x.x (Classe C) - Autorisé
  - 10.x.x.x (Classe A) - Autorisé
  - 172.16-31.x.x (Classe B) - Demander à l'utilisateur
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

## Niveau 3 : Branche curateur (IsCurator → AskHandler / GlobalACL)

Lorsque le rappel retourne `AskUser` ou qu'aucun rappel n'est configuré, le système effectue un branchement selon le statut de curateur :

### Branche curateur : IPermissionAskHandler

Pour le curateur silicon, le système demande une décision à l'utilisateur via messagerie instantanée.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        SendMessage($"Autoriser {resource} ?");

        var response = WaitForResponse();

        return new AskPermissionResult
        {
            Allowed = response.Approved,
            AddToCache = response.AddToCache,
            CacheDuration = response.CacheDuration
        };
    }
}
```

### Branche non-curateur : GlobalACL → Refus par défaut

Pour les Beings non-curateurs, le système vérifie la liste de contrôle d'accès globale. Si aucune règle correspondante n'est trouvée, la requête est refusée par défaut.

### Structure GlobalACL

```json
{
  "rules": [
    {
      "prefix": "network:api.github.com",
      "result": "Allowed"
    },
    {
      "prefix": "file:C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

Les règles sont évaluées dans l'ordre ; la première correspondance l'emporte. Seul le curateur silicon peut modifier l'ACL globale.

### Format des ressources

```
{type}:{path}

Exemples :
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## File d'attente des demandes de permission PermissionRequestQueue

`PermissionRequestQueue` gère les demandes de permission en attente, prenant en charge l'attente asynchrone des réponses utilisateur :

- **Mise en file d'attente** — Lorsque la chaîne de permissions atteint le niveau 3 (branche curateur), crée un `TaskCompletionSource<AskPermissionResult>` et le met en file d'attente
- **Affichage Web UI** — Affiche les demandes de permission en attente via `PermissionRequestController` dans l'interface Web
- **Réponse utilisateur** — L'utilisateur approuve ou refuse dans l'interface Web, avec possibilité de mettre en cache la décision et de définir la durée du cache
- **Options de cache** — L'utilisateur peut mettre en cache la décision de permission pour 1 heure, 24 heures, 7 jours ou 30 jours
- **Mécanisme de délai d'attente** — Fermeture automatique de la page de demande après 30 minutes sans réponse

## Système d'audit

Toutes les décisions de permissions sont enregistrées :

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

## Évaluation programmatique des permissions

### API EvaluatePermission

La méthode `PermissionManager.EvaluatePermission()` fournit une pré-évaluation en lecture seule des permissions sans déclencher d'invite utilisateur. `PermissionTool` utilise cette méthode pour permettre à l'IA de vérifier l'état des permissions avant de tenter une opération.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Valeur de retour** : `PermissionResult` à trois états :
- `Allowed` - L'opération est autorisée
- `Denied` - L'opération est refusée
- `AskUser` - Confirmation utilisateur requise lors de l'exécution

**Ordre d'évaluation** :
1. **Cache de fréquence** - Vérifier les décisions utilisateur en cache
2. **IPermissionCallback** - Évaluation par rappel personnalisé
3. **Branche curateur** - Si curateur, retourne `AskUser` (confirmation requise) ; si non-curateur, vérifie **GlobalACL**, puis refus par défaut

> **Note** : Contrairement à la chaîne de permissions complète, `EvaluatePermission` **n'appelle pas** `IPermissionAskHandler`. Il signale uniquement ce que le résultat *sera* lors de l'exécution.

## Gérer les permissions

### Accorder une permission

**Via l'interface Web** :
1. Naviguer vers **Gestion des permissions**
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
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### Révoquer une permission

Via la page de gestion des permissions dans le Web UI.

### Consulter les permissions

```bash
curl http://localhost:8080/api/permissions/list
```

## Meilleures pratiques

### 1. Principe du moindre privilège

Accorder uniquement les permissions minimales nécessaires :

```json
{
  "resource": "disk:read",  // Pas disk:*
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // Toujours définir une expiration
}
```

### 2. Utiliser des permissions à durée limitée

Ne jamais accorder de permissions permanentes sauf nécessité absolue.

### 3. Surveiller les journaux de permissions

Consulter régulièrement les journaux d'audit pour identifier :
- Les tentatives d'accès refusées
- Les modèles inhabituels
- Les élévations de permissions

### 4. Implémenter des rappels personnalisés

Pour une logique complexe, utiliser `IPermissionCallback` :

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // Permissions basées sur le temps
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // Permissions basées sur les ressources
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
IA : "Je dois lire config.json"
↓
Chaîne de permissions :
1. UserFrequencyCache ? Pas de décision en cache
2. IPermissionCallback ? Retourne AskUser (non explicitement autorisé)
3. IsCurator ? Non → Vérifier GlobalACL
4. GlobalACL ? Règle trouvée : file:... = Autorisé
5. Résultat : Autorisé
```

### Scénario 2 : L'IA veut exécuter du code

```
IA : "Je veux compiler et exécuter du code"
↓
Chaîne de permissions :
1. UserFrequencyCache ? Pas de décision en cache
2. IPermissionCallback ? Retourne AskUser
3. IsCurator ? Oui → IPermissionAskHandler
4. L'utilisateur approuve
5. Résultat : Autorisé
```

### Scénario 3 : Refus en cache

```
IA : "Je dois accéder à C:\Windows"
↓
Chaîne de permissions :
1. UserFrequencyCache ? Trouvé dans le cache HighDeny
2. Résultat : Refusé (aucune vérification supplémentaire nécessaire)
```

## Dépannage

### Permission inattendument refusée

**Vérifier** :
1. Le statut IsCurator de l'utilisateur
2. Les paramètres de limitation de débit
3. Les règles GlobalACL
4. La logique de rappel
5. Le délai d'attente de la réponse utilisateur

### Permission non expirée

**Vérifier** :
- Le champ `expiresAt` est correctement défini
- Le fuseau horaire est correct
- L'horloge est synchronisée

### Journaux d'audit non enregistrés

**Vérifier** :
- L'enregistreur d'audit est enregistré
- Le backend de stockage est accessible
- L'espace disque est suffisant

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
- 🛠️ Consulter le [guide de développement](development-guide.md)
- 🔒 Consulter la [documentation de sécurité](security.md)
- 🚀 Consulter le [guide de démarrage rapide](getting-started.md)
