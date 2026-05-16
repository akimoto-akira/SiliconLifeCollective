# Système de permissions

> **Version : v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | **Français** | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md)

## Aperçu

Le système de permissions garantit que toutes les opérations initiées par l'IA sont correctement vérifiées et auditées.

## Chaîne de permissions à 5 niveaux

```
┌─────────────────────────────────────────────┐
│          Vérification des permissions        │
├─────────────────────────────────────────────┤
│  Niveau 1 : IsCurator                       │
│  ↓ Si vrai, contourner                     │
│  Niveau 2 : UserFrequencyCache              │
│  ↓ Limitation du débit                      │
│  Niveau 3 : GlobalACL                       │
│  ↓ Liste de contrôle d'accès                │
│  Niveau 4 : IPermissionCallback             │
│  ↓ Logique personnalisée                    │
│  Niveau 5 : IPermissionAskHandler           │
│  ↓ Demander à l'utilisateur                 │
│  Résultat : Autorisé ou Refusé              │
└─────────────────────────────────────────────┘
```

## Niveau 1 : IsCurator

Les administrateurs/curateurs contournent toutes les vérifications de permissions.

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Curator access");
}
```

## Niveau 2 : UserFrequencyCache

Limitation du débit par utilisateur pour prévenir les abus.

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## Niveau 3 : GlobalACL

La liste de contrôle d'accès globale définit des règles explicites.

### Structure ACL

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

### Format des ressources

```
{type}:{action}

Exemples :
- disk:read
- disk:write
- network:http
- compile:execute
- system:info
```

## Niveau 4 : IPermissionCallback

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
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // Logique personnalisée
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }
        
        return PermissionResult.Undecided("Needs user confirmation");
    }
}
```

## Niveau 5 : IPermissionAskHandler

Demander la permission à l'utilisateur lorsque tous les autres niveaux sont indécis.

### Implémentation IMPermissionAskHandler

`IMPermissionAskHandler` envoie des demandes de permission à l'utilisateur via l'interface Web :

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // Envoyer un message à l'utilisateur via messagerie instantanée
        await SendMessageAsync($"Autoriser {request.Resource} ?");
        
        // Attendre la réponse de l'utilisateur
        var response = await WaitForResponseAsync();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### File d'attente des demandes de permission PermissionRequestQueue

`PermissionRequestQueue` gère les demandes de permission en attente, prenant en charge l'attente asynchrone des réponses utilisateur :

- **Mise en file d'attente** — Lorsque la chaîne de permissions atteint le niveau 5, crée un `TaskCompletionSource<AskPermissionResult>` et le met en file d'attente
- **Affichage Web UI** — Affiche les demandes de permission en attente via `PermissionRequestController` dans l'interface Web
- **Réponse utilisateur** — L'utilisateur approuve ou refuse dans l'interface Web, avec possibilité de mettre en cache la décision et de définir la durée du cache
- **Options de cache** — L'utilisateur peut mettre en cache la décision de permission pour 1 heure, 24 heures, 7 jours ou 30 jours
- **Mécanisme de délai d'attente** — Fermeture automatique de la page de demande après 60 secondes sans réponse

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
3. **Statut de curateur** - Si curateur, retourner `AskUser` (confirmation requise)
4. **ACL globale** - Vérifier les règles de contrôle d'accès
5. **Par défaut** - Refuser si aucune règle ne correspond

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
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### Révoquer une permission

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### Consulter les permissions

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
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
public async Task<PermissionResult> CheckAsync(PermissionRequest request)
{
    // Permissions basées sur le temps
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Outside business hours");
    }
    
    // Permissions basées sur les ressources
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.Undecided("Requires approval");
    }
    
    return PermissionResult.Allowed();
}
```

## Scénarios courants

### Scénario 1 : L'IA veut lire un fichier

```
IA : "Je dois lire config.json"
↓
Chaîne de permissions :
1. IsCurator ? Non
2. Limitation de débit ? Normal
3. GlobalACL ? Règle trouvée : disk:read = Autorisé
4. Résultat : Autorisé
```

### Scénario 2 : L'IA veut exécuter du code

```
IA : "Je veux compiler et exécuter du code"
↓
Chaîne de permissions :
1. IsCurator ? Non
2. Limitation de débit ? Normal
3. GlobalACL ? Aucune règle trouvée
4. Rappel ? Retourne Indécis
5. Demander à l'utilisateur ? Utilisateur approuve
6. Résultat : Autorisé
```

### Scénario 3 : Dépassement de la limitation de débit

```
IA : "Je dois faire 100 requêtes HTTP"
↓
Chaîne de permissions :
1. IsCurator ? Non
2. Limitation de débit ? Dépassé
3. Résultat : Refusé
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
