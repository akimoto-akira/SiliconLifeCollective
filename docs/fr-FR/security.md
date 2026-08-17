# Conception de sécurité

> **Version : v0.2.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md) | [Русский](../ru-RU/security.md)

## Aperçu

La sécurité de Silicon Life Collective repose sur un modèle de **défense en profondeur**. Principe fondamental : **toutes les opérations d'E/S doivent passer par des exécuteurs**, qui appliquent des vérifications d'autorisation avant l'exécution.

```
Appel d'Outil → Exécuteur → Gestionnaire d'Autorisations → Cache de fréquence → Rappel → (IsCurator : demander à l'utilisateur | Non-curateur : ACL Global)
```

---

## Modèle d'autorisations

### Types d'autorisation

| Type | Description |
|------|-------------|
| `NetworkAccess` | Requêtes HTTP/HTTPS sortantes |
| `CommandLine` | Exécution de commandes shell |
| `FileAccess` | Opérations sur fichiers et répertoires |
| `Function` | Appels de fonctions sensibles |
| `DataAccess` | Accès aux données système ou utilisateur |

### Résultats d'autorisation

Chaque vérification d'autorisation renvoie l'un des trois résultats suivants :

| Résultat | Comportement |
|----------|-------------|
| **Allowed (Autorisé)** | L'opération se poursuit immédiatement |
| **Denied (Refusé)** | L'opération est bloquée, journal d'audit enregistré |
| **AskUser (Demander à l'utilisateur)** | L'opération est suspendue, nécessite la confirmation de l'utilisateur |

### Rôle spécial : Curateur de Silicium

Le Curateur de Silicium possède le niveau d'autorisation le plus élevé (`IsCurator = true`). Lorsque la chaîne d'autorisations atteint le point de branchement, les opérations du curateur passent par `IPermissionAskHandler` pour demander la confirmation de l'utilisateur, plutôt que d'être court-circuitées en autorisé. Les non-curateurs interrogent l'ACL Global.

### Gestionnaire d'autorisations privé

Chaque Être de Silicium possède sa propre instance **privée de PermissionManager**. L'état des autorisations n'est pas partagé entre les êtres.

---

## Flux de vérification des autorisations

La priorité de requête est : **1. Cache de fréquence → 2. Fonction de rappel → 3. Branchement (IsCurator/GlobalACL)**

```
┌─────────────┐
│ Appel d'outil│
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Exécuteur  │────▶│ Gestionnaire        │
│ (Disque/    │     │ d'autorisations     │
│  Réseau/    │     │ privé (par être)    │
│  Ligne de   │     └────────┬────────────┘
│  commande)  │              │
└─────────────┘              ▼
                    ┌─────────────────┐
                    │ 1. Cache de     │──Correspondance──▶ Autorisé / Refusé
                    │    fréquence    │
                    │ (Refus élevé    │
                    │  prioritaire    │
                    │  sur Autorisé   │
                    │  élevé)         │
                    └────────┬────────┘
                             │ Non correspondant
                             ▼
                    ┌─────────────────┐
                    │ 2. Fonction de  │
                    │    rappel       │──▶ Autorisé / Refusé / Demander
                    │    d'autorisation│    à l'utilisateur
                    └────────┬────────┘
                             │ Demander à l'utilisateur
                             ▼
                    ┌─────────────────┐
                    │ 3. IsCurator ?  │
                    └────────┬────────┘
                             │
                   ┌─────────┴─────────┐
                   │                   │
                   ▼ Oui               ▼ Non
            ┌─────────────┐    ┌─────────────┐
            │ Demander à  │    │ ACL Global  │
            │ l'utilisateur│    │ Requête de  │
            │ (AskHandler) │    │ règles      │
            └─────────────┘    └─────────────┘
```

**Point clé** : L'exécuteur ne voit qu'une valeur booléenne (autorisé/refusé). Le gestionnaire d'autorisations gère en interne la décision ternaire (autorisé/refusé/demander à l'utilisateur) et résout la demande avant de renvoyer le résultat à l'exécuteur.

---

## Exécuteurs (frontière de sécurité)

Les exécuteurs sont le **seul** chemin pour les opérations d'E/S. Ils appliquent :

### Thread de répartition indépendant

Chaque exécuteur possède un **thread de répartition indépendant** :

- Isolation des threads entre exécuteurs — le blocage du thread d'un exécuteur n'affecte pas les autres.
- Chaque exécuteur peut définir des limites de ressources indépendantes (CPU, mémoire, etc.).
- Gestion du pool de threads pour les threads d'exécuteur.

### File d'attente de requêtes

Chaque exécuteur maintient une file d'attente de requêtes :

- Les requêtes sont routées vers l'exécuteur approprié selon leur type.
- Prise en charge de la file prioritaire.
- Contrôle du délai d'attente pour chaque requête.

### Verrouillage de thread pour la vérification des autorisations

Lorsqu'un outil initie un accès aux ressources :

1. L'exécuteur reçoit la requête et **verrouille son thread**.
2. L'exécuteur interroge le gestionnaire d'autorisations privé de l'être.
3. Si le rappel renvoie « demander à l'utilisateur », le thread de l'exécuteur **reste verrouillé** en attente de la réponse de l'utilisateur.
4. L'être ne voit que le résultat final (succès ou refus) — il ne voit jamais l'état intermédiaire « en attente ».
5. Seul le Curateur de Silicium déclenche une véritable invite utilisateur. Les êtres ordinaires interrogent l'ACL Global de manière synchrone sans bloquer.
6. En cas de dépassement de délai, la requête est traitée comme refusée et le verrou du thread est libéré.

### Types d'exécuteurs

| Exécuteur | Portée | Délai par défaut |
|------------|--------|------------------|
| `DiskExecutor` | Lecture/écriture de fichiers, opérations sur répertoires | 30 secondes |
| `NetworkExecutor` | Requêtes HTTP, connexions WebSocket | 60 secondes |
| `CommandLineExecutor` | Exécution de commandes shell | 120 secondes |

> **Note** : Le `DynamicCompilationExecutor` (dans l'espace de noms `SiliconLife.Core.Compilation`) est responsable de la compilation en mémoire Roslyn et n'appartient pas à la catégorie des exécuteurs d'E/S, mais est soumis au même système d'autorisations.

### Isolation des exceptions et tolérance aux pannes

- Les exceptions d'un exécuteur n'affectent pas les autres exécuteurs.
- Redémarrage automatique en cas de crash de thread.
- Disjoncteur : arrêt temporaire de l'exécuteur après des échecs consécutifs pour prévenir les défaillances en cascade.

---

## ACL Global (Liste de Contrôle d'Accès)

Table de règles partagée persistée dans le stockage, gérée uniquement par le Curateur de Silicium :

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- Les règles sont évaluées dans l'ordre ; la première correspondance l'emporte.
- Seul le Curateur de Silicium peut modifier l'ACL Global (via son outil dédié).
- Les modifications prennent effet immédiatement.
- L'ACL Global **n'est pas** dans la chaîne de priorité de chaque requête ci-dessus — elle est référencée en interne par la fonction de rappel.

---

## Cache de fréquence utilisateur

Pour réduire les invites d'autorisation répétitives, le système maintient deux caches **par être, en mémoire uniquement** :

| Cache | Objectif |
|-------|---------|
| **HighAllow (Autorisation élevée)** | Ressources fréquemment autorisées par l'utilisateur |
| **HighDeny (Refus élevé)** | Ressources fréquemment refusées par l'utilisateur |

### Fonctionnement

- **Choix de l'utilisateur, pas détection automatique** : Lorsque « demander à l'utilisateur » est déclenché, l'utilisateur choisit d'ajouter ou non la ressource au cache.
- **Correspondance de préfixe** : Prend en charge la correspondance de préfixe de chemin de ressource (par ex. `network:api.example.com/*`).
- **Priorité** : Le refus élevé a une priorité supérieure à l'autorisation élevée.
- **Mémoire uniquement** : Le cache n'est pas persisté. Il est perdu au redémarrage.
- **Expiration configurable** : L'utilisateur peut définir la durée de validité des entrées du cache.

### Flux de mise à jour du cache

1. Le rappel d'autorisation renvoie `AskUser`.
2. Le système d'autorisations envoie une requête au système de cartes (UI Web ou messagerie instantanée).
3. L'utilisateur prend une décision (autoriser/refuser) et **choisit de mettre en cache ou non**.
4. Le système de cartes renvoie la décision + l'indicateur de cache.
5. Le système d'autorisations met à jour la liste de cache correspondante.
6. Les requêtes futures correspondant au préfixe du cache sont résolues immédiatement.

---

## Mécanisme de demande à l'utilisateur

Lorsque la vérification des autorisations renvoie `AskUser` :

### UI Web : Carte interactive

Le frontend Web affiche immédiatement une **carte interactive** montrant :

- Le type et le chemin de la ressource
- La description de l'opération
- Boutons Autoriser / Refuser
- Cases à cocher optionnelles « Toujours autoriser » / « Toujours refuser » (ajout au cache de fréquence)

### Messagerie instantanée (sans support de carte) : Code aléatoire

Pour les plateformes de messagerie ne prenant pas en charge les cartes interactives :

1. Le système génère deux codes aléatoires à 6 chiffres : un **code d'autorisation** et un **code de refus**.
2. Envoie un message contenant les informations sur la ressource et les deux codes.
3. L'utilisateur doit répondre avec le code d'autorisation exact pour autoriser. Toute autre réponse est traitée comme un refus.
4. Les codes sont à usage unique pour prévenir les attaques par rejeu.

### Délai d'attente

- Un délai d'attente est défini pour toutes les requêtes « demander à l'utilisateur ».
- En cas de dépassement de délai, la requête est traitée comme **refusée** et le verrou du thread de l'exécuteur est libéré.

---

## Sécurité de la compilation dynamique

L'auto-évolution (réécriture de classe) introduit des risques de sécurité uniques. Le système les atténue grâce à une **stratégie en couches** :

### Couche 1 : Contrôle des références à la compilation (défense principale)

- Le compilateur ne reçoit que la **liste des références d'assembly autorisées**.
- **Autorisé** : `System.Runtime`, `System.Private.CoreLib`, assemblys du projet (interface ITool, etc.)
- **Bloqué** : `System.IO`, `System.Reflection`, `System.Runtime.InteropServices`, etc.
- Si le code référence un assembly bloqué, **le compilateur lui-même refuse** le code.
- Ceci est plus fiable que l'analyse à l'exécution — les opérations dangereuses sont impossibles au niveau du type.

### Couche 2 : Analyse statique à l'exécution (défense secondaire)

- Même après une compilation réussie, le code est soumis à une analyse de motifs statiques.
- Détection de motifs d'opérations dangereuses (E/S directes, appels système, etc.).
- Si du code dangereux est détecté, le chargement est refusé et le système revient à la fonctionnalité par défaut.

### Contrainte d'héritage

Toutes les classes personnalisées d'Êtres de Silicium **doivent** hériter de `SiliconBeingBase`. Le compilateur applique cette contrainte au niveau du type.

### Stockage chiffré

Le code compilé est stocké sur disque avec un chiffrement AES-256 :

- **Dérivation de clé** : À partir du GUID de l'être (en majuscules) en utilisant PBKDF2.
- **Échec de déchiffrement** : Repli vers l'implémentation par défaut.
- **Recompilation à l'exécution** : Le nouveau code est d'abord compilé en mémoire ; ce n'est qu'après une compilation réussie et le remplacement de l'instance qu'il est persisté.

### Remplacement atomique

Le processus de remplacement est atomique :

1. Compiler le nouveau code en mémoire → obtenir le `Type`.
2. Créer une nouvelle instance à partir du `Type`.
3. Migrer l'état de l'ancienne instance vers la nouvelle.
4. Échanger les références.
5. Persister le code chiffré.

Si une étape échoue, l'ancienne instance reste active.

---

## Fonction de rappel d'autorisation

### Conception

Chaque PermissionManager possède une **variable de fonction de rappel** :

- **Par défaut** : Pointe vers la fonction d'autorisation par défaut intégrée.
- **Après compilation dynamique** : Remplacée par la fonction d'autorisation personnalisée de l'être.
- **Exclusivité** : Un seul rappel est actif à la fois.
- **Échec de compilation** : N'affecte pas le rappel actuel — la fonction par défaut ou la dernière fonction personnalisée réussie reste en vigueur.

### Signature du rappel

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Renvoie `Allowed`, `Denied` ou `AskUser`.

---

## Journal d'audit

Toutes les décisions d'autorisation sont journalisées :

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Les journaux sont persistés dans le stockage et consultables via l'UI Web (contrôleur de journaux).

---

## Audit d'utilisation des Tokens

Le `TokenUsageAuditManager` fournit le suivi de la consommation de tokens IA liée à la sécurité :

- **Enregistrement par requête** — Chaque appel IA enregistre l'ID de l'être, le modèle, les tokens d'invite, les tokens de complétion et l'horodatage.
- **Détection d'anomalies** — Des modèles de consommation de tokens inhabituels peuvent indiquer une injection d'invite ou un abus de ressources.
- **Accès curateur uniquement** — Le `TokenAuditTool` (marqué `[SiliconManagerOnly]`) permet au curateur de requêter et de résumer l'utilisation des tokens.
- **Tableau de bord Web** — Le `UsageController` fournit un tableau de bord basé sur le navigateur avec graphiques de tendance et export de données.
- **Stockage persisté** — Les enregistrements sont stockés via `ITimeStorage` pour les requêtes de séries temporelles et l'analyse à long terme.

---

## Sécurité des plugins

Le système de plugins introduit des risques de sécurité liés à l'exécution de code tiers, atténués par les mécanismes suivants :

### Sandbox de sécurité et déclaration des capacités

Le `PluginLoader` effectue des analyses de sécurité lors du chargement des plugins et prend en charge simultanément le mécanisme de déclaration des capacités :

1. **Capacités déclarables** — Les plugins déclarent les capacités nécessaires via l'attribut `[PluginCapability]` :
   - `Network` — Accès réseau (autorise les références à `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`)
   - `FileIO` — Lecture/écriture de fichiers (autorise les références à `System.IO`)
   - `Process` — Gestion des processus
   - `AI` — Appels IA

2. **Capacités non déclarables** — Les capacités suivantes sont toujours bloquées :
   - P/Invoke (`System.Runtime.InteropServices`)
   - Code Unsafe (`System.Runtime.CompilerServices.Unsafe`)
   - Reflection Emit (`System.Reflection.Emit`)
   - API du compilateur (`Microsoft.CodeAnalysis`)

3. **Liste blanche d'assemblys de confiance** — Les références aux assemblys suivants sont autorisées :
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

4. **Vérification des types interdits** — Analyse des types dangereux référencés dans le plugin

5. **Vérification des membres interdits** — Analyse des méthodes dangereuses appelées dans le plugin

### Chargement isolé

- Chaque plugin est chargé de manière isolée en utilisant un `AssemblyLoadContext` personnalisé
- Les types et assemblys entre plugins ne s'interfèrent pas mutuellement
- Les ressources associées peuvent être libérées lors du déchargement du plugin

### Contraintes d'autorisation des outils

- Les outils enregistrés par les plugins via l'interface `ITool` sont soumis au même système d'autorisations
- Les outils de plugin ne peuvent pas contourner la chaîne de vérification des autorisations
- Les outils de plugin sont soumis au marquage `[SiliconManagerOnly]`

---

## Sécurité des autorisations d'outils

Le système d'autorisations d'outils fournit une couche de sécurité supplémentaire contrôlant les opérations d'outil que les Êtres de Silicium peuvent utiliser :

### Isolation d'autorisation à deux niveaux

1. **Niveau Être de Silicium** — Chaque Être de Silicium dispose d'une configuration d'autorisation d'outil indépendante
2. **Niveau projet** — Les autorisations d'outil dans l'espace projet sont indépendantes du niveau Être de Silicium, réalisant l'isolation des autorisations entre projets

### Modèles d'autorisation

Le système fournit des modèles d'autorisation prédéfinis garantissant une ligne de base de sécurité :

- **readonly** — Autorisations minimales, autorise uniquement les opérations de lecture
- **restricted** — Autorisations restreintes, autorise uniquement les opérations de base
- **full** — Autorisations complètes (réservées au curateur uniquement)

### Caractéristiques de sécurité

- **Refus par défaut** — Les opérations d'outil non explicitement autorisées sont refusées par défaut
- **Granularité d'opération** — Chaque opération de chaque outil est contrôlée indépendamment (par ex. `network:get` autorisé mais `network:post` refusé)
- **Gestion par le curateur** — Les autorisations d'outil ne peuvent être configurées que par le Curateur de Silicium
- **Piste d'audit** — Les modifications d'autorisations d'outil sont enregistrées dans le journal d'audit

---

## Sécurité des compétences

Le système de compétences réutilise le système de permissions d'outils et fournit plusieurs garde-fous :

### Permissions d'exécution

- L'id de la compétence sert de nom d'outil, intégré à la matrice de permissions `ToolActionPermissionConfig` avec l'action `execute`
- Les compétences désactivées n'apparaissent pas dans les définitions d'outils visibles par l'IA (double garantie : filtrage au niveau du schéma + vérification à l'exécution)
- Le Curateur de Silicium peut toujours exécuter ; les Êtres ordinaires nécessitent `IsActionAllowed(skillId, "execute")`

### Liste blanche d'outils et union de permissions

- Pendant l'exécution de la compétence, seuls les outils de la `ToolWhitelist` sont autorisés (liste vide = héritage de tous les outils de l'Être)
- Les restrictions d'actions de la compétence et les permissions de l'Être sont en **union stricte** (`MergePermissions`) : la compétence ne peut que restreindre davantage les permissions, jamais les élargir
- Les appels d'outils hors liste blanche échouent directement (`Tool not in whitelist`)

### Garde-fous de consommation de ressources

- **Commutateur global** : `SkillEnabled` désactive d'un clic tout le système de compétences
- **Quota de quantité** : le nombre de compétences personnalisées par Être est limité par `MaxCustomSkillsPerBeing` (50 par défaut)
- **Limite de tours** : `maxToolRound = Min(valeur déclarée par la compétence, GlobalMaxToolRound, 10 par défaut)`, pour éviter les boucles incontrôlées
- **Limite de délai d'attente** : `timeout = Min(valeur déclarée par la compétence, GlobalSkillTimeoutSeconds, 300s par défaut)`
- **Protection contre la récursion** : une compétence en cours d'exécution ne peut pas s'appeler elle-même

### Permissions de modification

- Le Curateur de Silicium peut modifier toutes les compétences ; les Êtres ordinaires ne peuvent modifier que les compétences dont la source est `Being`/`User`
- La complétion automatique des métadonnées ne remplit que les champs manquants ; les champs fournis par l'utilisateur ne sont jamais écrasés par l'IA

---

## Sécurité MCP

L'intégration MCP suit le principe de « souveraineté de l'utilisateur + cohérence des permissions » :

### Souveraineté de l'utilisateur

- L'ajout, la suppression, l'activation/désactivation et la reconnexion des serveurs MCP **ne peuvent être effectués que par l'utilisateur via l'UI Web** (/mcp ou la page de configuration)
- L'outil `mcp` côté IA est une requête en lecture seule (status/list_servers/list_tools), incapable de modifier la liste des serveurs
- Le commutateur global `McpEnabled` permet de couper d'un clic tous les outils externes

### Isolation des outils et permissions

- Les outils encapsulés sont nommés `mcp_{serverId}_{toolName}`, isolés de l'espace de noms des outils intégrés/plugins
- Chaque outil encapsulé déclare automatiquement une seule action `execute`, intégrée à la matrice de permissions d'outils à deux niveaux, désactivable individuellement par Être/projet
- Après désactivation d'un serveur, ses outils sont immédiatement désactivés de tous les Êtres

### Frontières de transport et de processus

- Les serveurs `stdio` s'exécutent en tant que sous-processus, n'héritant que des variables d'environnement explicitement configurées (champ `env`)
- Les serveurs `http` communiquent via l'endpoint configuré ; en cas d'échec de connexion, ils passent automatiquement en état error et exposent `lastError`

---

## Sécurité des clés IM

### Espaces réservés de variables d'environnement

Les valeurs de configuration des plateformes IM prennent en charge les espaces réservés `${ENV_VAR}` (par ex. `"${FEISHU_APP_SECRET}"`) :

- `ConfigSecretResolver` résout les espaces réservés sur une **copie profonde**, le fichier `config.json` original conserve toujours les espaces réservés tels quels
- Les `SaveConfig` ultérieurs ne réécrivent pas les clés secrètes en clair résolues sur le disque
- Prise en charge des espaces réservés en valeur entière et des espaces réservés intégrés dans la valeur (par ex. `prefix-${VAR}`)

### Sécurité de l'autorisation OAuth

- **state anti-CSRF** : nombre aléatoire cryptographique de 16 octets, vérifié strictement lors du rappel
- **Délai d'attente de 5 minutes** : les sessions d'autorisation expirées sont automatiquement invalidées ; lors du remplacement d'une ancienne session, celle-ci est immédiatement annulée
- **Stockage des jetons** : accessToken/refreshToken/tokenExpiresAt sont réécrits dans la configuration de la plateforme et persistés, `authMode` est marqué comme `oauth`
- L'URL de rappel prend en charge la configuration `redirectBaseUrl` (scénario de rappel public)

### Sécurité des messages

- Feishu : vérification de signature (`X-Lark-Signature`, SHA256) + déchiffrement d'événements AES-256-CBC + déduplication d'événements (fenêtre de 10 minutes)
- WeChat Enterprise : chiffrement/déchiffrement WXBizMsgCrypt et vérification de signature
- DingTalk : le mode Stream utilise un WebSocket chiffré ; le mode HTTP utilise la vérification de rappel
