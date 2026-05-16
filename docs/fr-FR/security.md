# Conception de sécurité

> **Version : v0.2.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | **Français** | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md)

## Aperçu

La sécurité de Silicon Life Collective repose sur un modèle de **défense en couches multiples**. Principe fondamental : **Toutes les opérations d'E/S doivent passer par des exécuteurs**, les exécuteurs appliquent les vérifications de permissions avant l'exécution.

```
Appel d'outil → Exécuteur → PermissionManager → Cache HighDeny → Cache HighAllow → Rappel → Demander à l'utilisateur
```

---

## Modèle de permissions

### Types de permissions

| Type | Description |
|------|-------------|
| `NetworkAccess` | Requêtes HTTP/HTTPS sortantes |
| `CommandLine` | Exécution de commandes shell |
| `FileAccess` | Opérations sur fichiers et répertoires |
| `Function` | Appels de fonctions sensibles |
| `DataAccess` | Accès aux données système ou utilisateur |

### Résultats de permission

Chaque vérification de permission retourne l'un des trois résultats :

| Résultat | Comportement |
|----------|-------------|
| **Allowed (Autorisé)** | L'opération continue immédiatement |
| **Denied (Refusé)** | L'opération est bloquée, le journal d'audit est enregistré |
| **AskUser (Demander à l'utilisateur)** | L'opération est suspendue, nécessite la confirmation de l'utilisateur |

### Rôle spécial : Silicon Curator

Le Silicon Curator a le niveau de permission le plus élevé (`IsCurator = true`). Les vérifications de permissions du Curator sont court-circuitées vers **Allowed**, sauf si l'utilisateur remplace explicitement.

### PermissionManager privé

Chaque Silicon Being a sa propre instance **privée** de PermissionManager. Les états de permissions ne sont pas partagés entre les Beings.

---

## Flux de validation des permissions

Priorité de requête : **1. HighDeny utilisateur → 2. HighAllow utilisateur → 3. Fonction de rappel**

```
┌─────────────┐
│ Appel d'outil│
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Exécuteur   │────▶│ PermissionManager   │
│(Disk/Network/│     │ privé               │
│  Command...) │     │ (par Being)          │
└─────────────┘     └────────┬────────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ 1. IsCurator ?  │──Oui──▶ Allowed
                    └────────┬────────┘
                             │ Non
                             ▼
                    ┌─────────────────┐
                    │ 2. HighDeny     │──Correspond──▶ Denied
                    │ utilisateur     │
                    │(Cache mémoire)  │
                    └────────┬────────┘
                             │ Pas de correspondance
                             ▼
                    ┌─────────────────┐
                    │ 3. HighAllow    │──Correspond──▶ Allowed
                    │ utilisateur     │
                    │(Cache mémoire)  │
                    └────────┬────────┘
                             │ Pas de correspondance
                             ▼
                    ┌─────────────────┐
                    │ 4. Rappel de    │
                    │ permission      │──▶ Allowed / Denied / AskUser
                    └─────────────────┘
```

**Important** : L'exécuteur ne voit qu'un booléen (Allowed/Denied). Le PermissionManager traite en interne la décision ternaire (Allowed/Denied/AskUser) et résout AskUser avant de retourner le résultat à l'exécuteur.

---

## Exécuteurs (Frontière de sécurité)

Les exécuteurs sont le **seul** chemin pour les opérations d'E/S. Ils appliquent :

### Threads d'ordonnancement séparés

Chaque exécuteur a un **thread d'ordonnancement séparé** :

- Isolation de thread entre exécuteurs — le blocage d'un thread d'exécuteur n'affecte pas les autres exécuteurs.
- Chaque exécuteur peut définir des limites de ressources séparées (CPU, mémoire, etc.).
- Gestion de pool de threads pour les threads d'exécuteurs.

### File d'attente de requêtes

Chaque exécuteur gère une file d'attente de requêtes :

- Les requêtes sont routées vers l'exécuteur correspondant par type.
- Supporte la file d'attente prioritaire.
- Contrôle de timeout par requête.

### Verrouillage de thread pour la validation des permissions

Lorsqu'un outil initie un accès aux ressources :

1. L'exécuteur reçoit la requête et **verrouille son thread**.
2. L'exécuteur interroge le PermissionManager privé du Being.
3. Si le rappel retourne AskUser, le thread de l'exécuteur **reste verrouillé** en attente de la réponse de l'utilisateur.
4. Le Being ne voit que le résultat final (succès ou refus) — il ne voit jamais l'état intermédiaire « Pending » ou « Waiting ».
5. Seul le Silicon Curator déclenche une véritable invite utilisateur. Les Beings normaux interrogent de manière synchrone la GlobalACL sans blocage.
6. En cas de timeout, la requête est traitée comme Denied, le verrouillage du thread est libéré.

### Types d'exécuteurs

| Exécuteur | Portée | Timeout par défaut |
|-----------|--------|-------------------|
| `DiskExecutor` | Lecture/écriture de fichiers, opérations sur répertoires | 30 secondes |
| `NetworkExecutor` | Requêtes HTTP, connexions WebSocket | 60 secondes |
| `CommandLineExecutor` | Exécution de commandes shell | 120 secondes |
| `DynamicCompilationExecutor` | Compilation en mémoire Roslyn | 60 secondes |

### Isolation des exceptions et tolérance aux pannes

- Les exceptions d'un exécuteur n'affectent pas les autres exécuteurs.
- Redémarrage automatique en cas de crash de thread.
- Circuit Breaker : Pause temporaire de l'exécuteur après des échecs consécutifs pour prévenir les erreurs en cascade.

---

## ACL globale (Liste de contrôle d'accès)

Table de règles commune persistée dans le stockage, gérée uniquement par le Silicon Curator :

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- Les règles sont évaluées séquentiellement ; la première correspondance l'emporte.
- Seul le Silicon Curator peut modifier l'ACL globale (via un outil dédié).
- Les modifications prennent effet immédiatement.
- L'ACL globale n'est **pas** dans la chaîne de priorité mentionnée ci-dessus par requête — elle est référencée en interne par la fonction de rappel.

---

## Cache de fréquence utilisateur

Pour réduire les requêtes de permissions répétées, le système gère deux caches **par Being, mémoire uniquement** :

| Cache | Utilisation |
|-------|------------|
| **HighAllow** | Ressources fréquemment autorisées par l'utilisateur |
| **HighDeny** | Ressources fréquemment refusées par l'utilisateur |

### Fonctionnement

- **Choix utilisateur, pas détection automatique** : Lorsqu'AskUser est déclenché, l'utilisateur choisit s'il faut ajouter la ressource au cache.
- **Correspondance de préfixe** : Supporte la correspondance de préfixe de chemin de ressource (ex. `network:api.example.com/*`).
- **Priorité** : HighDeny est prioritaire sur HighAllow.
- **Mémoire uniquement** : Les caches ne sont pas persistés. Ils sont perdus au redémarrage.
- **Expiration configurable** : L'utilisateur peut définir la durée de validité des entrées de cache.

### Flux de mise à jour du cache

1. Le rappel de permission retourne `AskUser`.
2. Le système de permissions envoie la requête au système de cartes (interface Web ou IM).
3. L'utilisateur prend une décision (Allowed/Denied) et **choisit s'il faut mettre en cache**.
4. Le système de cartes retourne la décision + le flag de cache.
5. Le système de permissions met à jour la liste de cache correspondante.
6. Les futures requêtes avec le préfixe de cache sont résolues immédiatement.

---

## Mécanisme de demande à l'utilisateur

Lorsque la vérification de permission retourne `AskUser` :

### Interface Web : Cartes interactives

L'interface Web affiche immédiatement une **carte interactive** avec :

- Le type et le chemin de la ressource
- La description de l'action
- Boutons Autoriser / Refuser
- Case à cocher optionnelle « Toujours autoriser » / « Toujours refuser » (ajouter au cache de fréquence)

### Messagerie instantanée (sans support de carte) : Code aléatoire

Pour les plateformes de messagerie sans support de carte interactive :

1. Le système génère deux codes aléatoires à 6 chiffres : **Code Autoriser** et **Code Refuser**.
2. Envoie un message avec les informations sur la ressource et les deux codes.
3. L'utilisateur doit répondre avec le code Autoriser exact pour autoriser. Toute autre réponse est traitée comme un refus.
4. Les codes sont à usage unique, prévenant les attaques par rejeu.

### Timeout

- Un timeout est défini pour toutes les requêtes AskUser.
- En cas de timeout, la requête est traitée comme **Denied**, le verrouillage du thread de l'exécuteur est libéré.

---

## Sécurité de la compilation dynamique

L'auto-développement (remplacement de classes) introduit des risques de sécurité uniques. Le système les atténue par une **stratégie multicouche** :

### Couche 1 : Contrôle des références à la compilation (Défense principale)

- Le compilateur ne reçoit qu'une **liste d'assemblies autorisés**.
- **Autorisé** : `System.Runtime`, `System.Private.CoreLib`, assemblies du projet (interface ITool, etc.)
- **Bloqué** : `System.IO`, `System.Reflection`, `System.Runtime.InteropServices`, etc.
- Si le code référence un assembly bloqué, **le compilateur lui-même refuse** le code.
- Plus fiable que le scan à l'exécution — les opérations dangereuses sont impossibles au niveau du type.

### Couche 2 : Analyse statique à l'exécution (Défense secondaire)

- Même après une compilation réussie, le code est scanné pour des motifs statiques.
- Détecte les motifs d'opérations dangereuses (E/S direct, appels système, etc.).
- En cas de code dangereux, le chargement est refusé, le système revient à la fonctionnalité par défaut.

### Restriction d'héritage

Toutes les classes Silicon Being personnalisées **doivent** hériter de `SiliconBeingBase`. Le compilateur applique cette restriction au niveau du type.

### Stockage chiffré

Le code compilé est stocké sur disque chiffré avec AES-256 :

- **Dérivation de clé** : À partir du GUID du Being (majuscules) via PBKDF2.
- **Erreur de déchiffrement** : Revient à l'implémentation par défaut.
- **Recompilation à l'exécution** : Le nouveau code est d'abord compilé en mémoire ; ce n'est qu'après une compilation réussie et le remplacement d'instance qu'il est persisté.

### Remplacement atomique

Le processus de remplacement est atomique :

1. Compiler le nouveau code en mémoire → obtenir le `Type`.
2. Créer une nouvelle instance à partir du `Type`.
3. Migrer l'état de l'ancienne instance vers la nouvelle.
4. Échanger la référence.
5. Persister le code chiffré.

Si une étape échoue, l'ancienne instance reste active.

---

## Fonctions de rappel de permission

### Conception

Chaque PermissionManager maintient une **variable de fonction de rappel** :

- **Par défaut** : Pointe vers la fonction de permission par défaut intégrée.
- **Après compilation dynamique** : Remplacée par la fonction de permission personnalisée du Being.
- **L'un ou l'autre** : Un seul rappel actif à tout moment.
- **Erreur de compilation** : N'affecte pas le rappel actuel — la fonction par défaut ou la dernière fonction personnalisée réussie reste active.

### Signature du rappel

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Retourne `Allowed`, `Denied` ou `AskUser`.

---

## Journal d'audit

Toutes les décisions de permissions sont enregistrées :

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Les journaux sont persistés dans le stockage, consultables via l'interface Web (contrôleur de journaux).

---

## Audit d'utilisation des tokens

Le `TokenUsageAuditManager` fournit un suivi de la consommation de tokens IA pertinent pour la sécurité :

- **Enregistrement par requête** — Chaque appel IA enregistre l'ID du Being, le modèle, les tokens de prompt, les tokens de complétion et l'horodatage.
- **Détection d'anomalies** — Des modèles de consommation de tokens inhabituels peuvent indiquer une injection de prompt ou un abus de ressources.
- **Accès Curator uniquement** — `TokenAuditTool` (marqué `[SiliconManagerOnly]`) permet au Curator de consulter et résumer l'utilisation des tokens.
- **Tableau de bord Web** — `UsageController` fournit un tableau de bord basé sur le navigateur avec graphiques de tendance et export de données.
- **Stockage persistant** — Les enregistrements sont stockés via `ITimeStorage` pour les requêtes de séries temporelles et l'analyse à long terme.

---

## Sécurité des plugins

Le système de plugins introduit des risques de sécurité par l'exécution de code tiers, qui sont atténués par les mécanismes suivants :

### Bac à sable de sécurité

Le `PluginLoader` effectue des vérifications de sécurité strictes lors du chargement :

1. **Vérification des espaces de noms interdits** — Les plugins ne doivent pas référencer les espaces de noms suivants :
   - `System.IO` — Accès au système de fichiers
   - `System.Net.Http` — Requêtes HTTP
   - `System.Net.WebSockets` — Connexions WebSocket
   - `System.Net.Sockets` — Sockets bruts
   - `Microsoft.CodeAnalysis` — API du compilateur

2. **Liste blanche d'assemblies de confiance** — Les références aux assemblies suivantes sont autorisées :
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

3. **Vérification des types interdits** — Scanne les types dangereux référencés dans le plugin

4. **Vérification des membres interdits** — Scanne les méthodes dangereuses appelées dans le plugin

### Chargement isolé

- Chaque plugin est chargé de manière isolée via un `AssemblyLoadContext` personnalisé
- Les types et assemblies entre plugins ne se perturbent pas mutuellement
- Lors du déchargement d'un plugin, les ressources associées peuvent être libérées

### Restrictions de permissions des outils

- Les plugins qui enregistrent des outils via l'interface `ITool` sont soumis au même système de permissions
- Les outils de plugin ne peuvent pas contourner la chaîne de permissions à 5 niveaux
- Les outils de plugin sont soumis au marquage `[SiliconManagerOnly]`
