# Architecture

> **Version : v0.2.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md) | [Русский](../ru-RU/architecture.md)

## Architecture double version

Ce projet propose deux versions d'implémentation, partageant la même conception architecturale, mais différant dans le stockage et l'optimisation des performances :

### SiliconLife.Default (version par défaut)
- **Positionnement** : Implémentation par défaut, principalement utilisée pour valider la faisabilité de l'architecture
- **Mode d'exécution** : Application console
- **Mode de stockage** : Stockage JSON pur sur système de fichiers
- **Scénarios d'utilisation** : Exigences élevées de sécurité des données, ressources mémoire limitées, faible volume de données
- **Rôle** : Implémentation de référence pour la validation de l'architecture, offrant un mode d'exécution simple et fiable, adaptée aux premiers contacts avec le projet, au débogage de développement ou aux scénarios privilégiant la sécurité des données

### SiliconLife.Fast (version haute performance)
- **Positionnement** : Version de production recommandée
- **Mode d'exécution** : Application de bureau (barre d'état système Windows / fenêtre d'état Linux)
- **Mode de stockage** : Stockage en mémoire SpeedyPack + persistance par lots asynchrone (format de fichier .spk)
- **Scénarios d'utilisation** : Concurrence élevée, faible latence, grand volume de données
- **Support de plateforme** : Windows/macOS (fonctionnalités complètes, incluant la barre d'état système) ; Linux (fenêtre d'état, sans icône de barre d'état)
- **Caractéristiques** :
  - Fonctionnement en arrière-plan via la barre d'état Windows/macOS, surveillance en temps réel via la fenêtre d'état ; affichage direct de la fenêtre d'état sous Linux
  - Moteur SpeedyPack + compression automatique garantissant la sécurité des données
  - Architecture Component UI, 27 composants déclaratifs
  - 7 thèmes d'apparence, avec découverte automatique et changement
  - Prise en charge du rechargement à chaud pour les mises à jour et redémarrages en ligne
  - Ouverture automatique du navigateur sous Linux pour accéder à l'interface Web UI, prise en charge du paramètre `--no-tray`
- **Amélioration des performances** : Latence de lecture réduite de 1000 fois, latence d'écriture réduite de 15000 fois
- **Rôle** : Implémentation de niveau production profondément optimisée, dotée de fonctionnalités telles que le fonctionnement en arrière-plan via la barre d'état système, le moteur SpeedyPack + compression automatique, choix recommandé pour l'exécution à long terme et les environnements de production réels

> **Note** : L'architecture décrite dans ce document s'applique aux deux versions, à l'exception de l'implémentation du stockage. SiliconLife.Default sert de référence pour la validation de l'architecture, SiliconLife.Fast est la version de production recommandée.

---

## Concepts fondamentaux

### Être de Silicium

Chaque agent IA du système est un **Être de Silicium** — une entité autonome dotée de sa propre identité, personnalité et capacités. Chaque Être de Silicium est piloté par un **Fichier d'Âme** (invite Markdown), définissant ses schémas comportementaux.

### Curateur de Silicium

Le **Curateur de Silicium** est un Être de Silicium spécial doté des autorisations système les plus élevées. Il agit comme administrateur du système :

- Crée et gère les autres Êtres de Silicium
- Analyse les requêtes des utilisateurs et les décompose en tâches
- Distribue les tâches aux Êtres de Silicium appropriés
- Surveille la qualité d'exécution et gère les échecs
- Répond aux messages des utilisateurs avec un **ordonnancement prioritaire** (voir ci-dessous)

### Fichier d'Âme

Fichier Markdown (`soul.md`) stocké dans le répertoire de données de chaque Être de Silicium. Il est injecté comme invite système dans chaque requête IA, définissant la personnalité, les schémas de décision et les contraintes comportementales de l'être.

---

## Ordonnancement : Ordonnancement équitable par créneaux temporels

### Boucle Principale + Objets Tick

Le système exécute une **Boucle Principale pilotée par horloge** sur un thread d'arrière-plan dédié :

```
Boucle Principale (thread dédié, Chien de Garde + Disjoncteur)
  └── Objet Tick A (priorité=0, intervalle=100ms)
  └── Objet Tick B (priorité=1, intervalle=500ms)
  └── Gestionnaire d'Êtres de Silicium (déclenché par horloge directement par la Boucle Principale)
        └── Exécuteur d'Être de Silicium → Être de Silicium 1 → Déclenchement par horloge → Exécution d'un cycle
        └── Exécuteur d'Être de Silicium → Être de Silicium 2 → Déclenchement par horloge → Exécution d'un cycle
        └── Exécuteur d'Être de Silicium → Être de Silicium 3 → Déclenchement par horloge → Exécution d'un cycle
        └── ...
```

Décisions de conception clés :

- **Les Êtres de Silicium n'héritent pas de l'Objet Tick.** Ils ont leur propre méthode `Tick()`, appelée par `SiliconBeingManager` via `SiliconBeingRunner`, et non enregistrée directement dans la Boucle Principale.
- **Le Gestionnaire d'Êtres de Silicium** est déclenché directement par l'horloge de la Boucle Principale et agit comme proxy unique pour tous les êtres.
- **L'Exécuteur d'Être de Silicium** encapsule le `Tick()` de chaque être sur un thread temporaire, avec un timeout et un Disjoncteur propre à chaque être (3 timeouts consécutifs → 1 minute de refroidissement).
- L'exécution de chaque être est limitée à **un cycle** de requête IA + appel d'outils par déclenchement d'horloge, garantissant qu'aucun être ne peut monopoliser la Boucle Principale.
- Le **Moniteur de Performance** suit les temps d'exécution des horloges pour la observabilité.

### Réponse prioritaire du Curateur

Lorsqu'un utilisateur envoie un message au Curateur de Silicium :

1. L'être en cours (par exemple l'Être A) termine son cycle actuel — **sans interruption**.
2. Le gestionnaire **saute le reste de la file**.
3. La boucle **reprend depuis le Curateur**, lui permettant de s'exécuter immédiatement.

Cela garantit la réactivité aux interactions utilisateur sans perturber les tâches en cours.

---

## Architecture des composants

```
┌─────────────────────────────────────────────────────────┐
│                        Hôte Principal                    │
│  (Hôte unifié — assemblage et gestion de tous les composants) │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Boucle    │  │ Localisateur │  │   Configuration   │  │
│  │ Principale│  │ de services  │  │                   │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │      Gestionnaire d'Êtres de Silicium (Objet Tick) │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curateur  │ │ Être A  │ │ Être B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Services partagés                     │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Système   │  │ Stockage │  │ Gestionnaire     │  │   │
│  │  │de Chat   │  │          │  │ d'Autorisations  │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ Client   │  │Exécuteur │  │ Gestionnaire     │  │   │
│  │  │ IA       │  │          │  │ d'Outils         │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Chargeur  │  │Réseau de │                        │   │
│  │  │de Plugins│  │Connaiss. │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Exécuteurs                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Exécuteur│  │ Exécuteur│  │ Exécuteur        │  │   │
│  │  │ Disque   │  │ Réseau   │  │ Ligne de commande│  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              Fournisseurs IM                       │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Fournisseur│ │Fournisseur│ │ Feishu / ...     │  │   │
│  │  │Console    │  │ Web      │  │ Fournisseur      │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Localisateur de services

`ServiceLocator` est un registre singleton thread-safe fournissant l'accès à tous les services principaux :

| Propriété | Type | Description |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Gestionnaire central de sessions de chat |
| `IMManager` | `IMManager` | Routeur de fournisseurs de messagerie instantanée |
| `AuditLogger` | `AuditLogger` | Piste d'audit des autorisations |
| `GlobalAcl` | `GlobalACL` | ACL Global |
| `BeingFactory` | `ISiliconBeingFactory` | Fabrique de création d'êtres |
| `BeingManager` | `SiliconBeingManager` | Gestionnaire de cycle de vie des êtres actifs |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Chargeur de compilation dynamique |
| `TokenUsageAudit` | `ITokenUsageAudit` | Suivi de l'utilisation des tokens |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Rapports d'utilisation des tokens |

Il maintient également un registre de `PermissionManager` par être, indexé par le GUID de l'être.

---

## Système de Chat

### Types de sessions

Le Système de Chat prend en charge trois types de sessions via `SessionBase` :

| Type | Classe | Description |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Conversation en tête-à-tête entre deux participants |
| `GroupChat` | `GroupChatSession` | Chat de groupe multi-participants |
| `Broadcast` | `BroadcastChannel` | Canal ouvert avec ID fixe ; les êtres s'abonnent dynamiquement et ne reçoivent les messages qu'après leur abonnement |

### Canal de Diffusion

`BroadcastChannel` est un type de session spécial utilisé pour les annonces à l'échelle du système :

- **ID de canal fixe** — Contrairement à `SingleChatSession` et `GroupChatSession`, l'ID du canal est une constante connue, et non dérivé des GUID des membres.
- **Abonnement dynamique** — Les êtres s'abonnent/se désabonnent à l'exécution ; ils ne reçoivent que les messages publiés après leur abonnement.
- **Filtrage des messages en attente** — `GetPendingMessages()` ne retourne que les messages publiés après l'heure d'abonnement de l'être et non encore lus.
- **Géré par le Système de Chat** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Message de Chat

Le modèle `ChatMessage` contient des champs pour le contexte de conversation IA et le suivi des tokens :

| Champ | Type | Description |
|-------|------|-------------|
| `Id` | `Guid` | Identifiant unique du message |
| `SenderId` | `Guid` | Identifiant unique de l'expéditeur |
| `ChannelId` | `Guid` | Identifiant du canal/conversation |
| `Content` | `string` | Contenu du message |
| `Timestamp` | `DateTime` | Heure d'envoi du message |
| `Type` | `MessageType` | Texte, image, fichier ou notification système |
| `ReadBy` | `List<Guid>` | IDs des participants ayant lu ce message |
| `Role` | `MessageRole` | Rôle dans la conversation IA (utilisateur, assistant, outil) |
| `ToolCallId` | `string?` | ID de l'appel d'outil pour les messages de résultat d'outil |
| `ToolCallsJson` | `string?` | JSON sérialisé des appels d'outil pour les messages assistant |
| `Thinking` | `string?` | Raisonnement en chaîne de pensée de l'IA |
| `PromptTokens` | `int?` | Nombre de tokens dans l'invite (entrée) |
| `CompletionTokens` | `int?` | Nombre de tokens dans la complétion (sortie) |
| `TotalTokens` | `int?` | Nombre total de tokens utilisés (entrée + sortie) |
| `FileMetadata` | `FileMetadata?` | Métadonnées du fichier joint (si le message contient un fichier) |

### File de messages de chat

`ChatMessageQueue` est un système de file de messages thread-safe pour la gestion asynchrone des messages de chat :

- **Thread-safe** — Utilisation de mécanismes de verrouillage pour garantir la sécurité de l'accès concurrentiel
- **Traitement asynchrone** — Prise en charge de l'enfilement et du défilement asynchrone des messages
- **Ordonnancement des messages** — Maintien de l'ordre chronologique des messages
- **Opérations par lots** — Prise en charge de la récupération par lots des messages

### Métadonnées de fichier

`FileMetadata` gère les informations sur les fichiers joints aux messages de chat :

- **Informations sur le fichier** — Nom, taille, type, chemin du fichier
- **Heure de téléchargement** — Horodatage du téléchargement du fichier
- **Téléchargeur** — ID de l'utilisateur ou de l'Être de Silicium ayant téléchargé le fichier

### Gestionnaire d'annulation de flux

`StreamCancellationManager` fournit un mécanisme d'annulation pour les réponses en flux de l'IA :

- **Contrôle de flux** — Prise en charge de l'annulation des réponses en flux IA en cours
- **Nettoyage des ressources** — Nettoyage correct des ressources associées lors de l'annulation
- **Sécurité concurrentielle** — Prise en charge de la gestion simultanée de plusieurs flux

### Consultation de l'historique de chat

La fonctionnalité de consultation de l'historique de chat permet aux utilisateurs de parcourir les conversations historiques des Êtres de Silicium :

- **Liste des sessions** — Affiche toutes les sessions historiques
- **Détails des messages** — Consultation de l'historique complet des messages
- **Vue chronologique** — Affichage des messages dans l'ordre chronologique
- **Support API** — Fourniture d'API RESTful pour récupérer les données de sessions et de messages

---

## Système de clients IA

Le système prend en charge plusieurs backends IA via l'interface `IAIClient` :

### OllamaClient

- **Type** : Service IA local
- **Protocole** : API HTTP native Ollama (`/api/chat`, `/api/generate`)
- **Fonctionnalités** : Streaming, appels d'outils, hébergement de modèles locaux
- **Configuration** : `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI (`/compatible-mode/v1/chat/completions`)
- **Authentification** : Bearer token (clé API)
- **Fonctionnalités** : Streaming, appels d'outils, contenu de raisonnement (chaîne de pensée), déploiement multi-régions
- **Régions prises en charge** :
  - `beijing` — Chine du Nord 2 (Pékin)
  - `virginia` — États-Unis (Virginie)
  - `singapore` — Singapour
  - `hongkong` — Hong Kong, Chine
  - `frankfurt` — Allemagne (Francfort)
- **Modèles pris en charge** (découverte dynamique via API, avec liste de repli) :
  - **Série Qwen** : qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Raisonnement** : qwq-plus
  - **Tiers** : deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Configuration** : `apiKey`, `region`, `model`
- **Découverte de modèles** : Récupération des modèles disponibles depuis l'API Bailian à l'exécution ; repli vers une liste éditorialisée en cas de panne réseau

### VolcengineArkClient (Volcengine Ark)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI
- **Authentification** : Bearer token (clé API)
- **Fonctionnalités** : Modes flux et non-flux, contrôle de débit double intégré
  - Auto-contrôle de débit : application d'un intervalle minimum entre les requêtes
  - Limite de débit serveur : gestion des erreurs 429, retry avec backoff exponentiel
- **Configuration** : `apiKey`, `endpoint`, `model`
- **Particularité** : Service IA de ByteDance, prenant en charge plusieurs modèles Doubao

### HerdsmanClient

- **Type** : Moteur d'inférence local/cloud
- **Protocole** : API compatible OpenAI
- **Authentification** : Aucune
- **Fonctionnalités** : Streaming, appels d'outils, contenu de raisonnement
- **Configuration** : `endpoint`, `model`

### LongCatClient (Meituan LongCat)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI
- **Authentification** : Bearer token (clé API)
- **Fonctionnalités** : Streaming, appels d'outils
- **Configuration** : `apiKey`, `endpoint`, `model`

### QiniuAIClient (Qiniu Cloud AI)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI
- **Authentification** : Bearer token (clé API)
- **Fonctionnalités** : Streaming, appels d'outils
- **Configuration** : `apiKey`, `endpoint`, `model`

### DeepSeekClient (DeepSeek)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI
- **Authentification** : Bearer token (clé API)
- **Endpoint** : `https://api.deepseek.com`
- **Fonctionnalités** : Streaming, appels d'outils, mode thinking (raisonnement), contenu de raisonnement (`reasoning_content`), contrôle de l'effort de raisonnement (`reasoning_effort`)
- **Fenêtre de contexte** : Jusqu'à 1M tokens (1 048 576) pour deepseek-v4, 131K pour les autres modèles
- **Modèle par défaut** : `deepseek-v4-flash`
- **Vision** : Non prise en charge
- **Configuration** : `apiKey`, `endpoint`, `model`, `thinkingEnabled`, `reasoningEffort`

### ZhipuClient (Zhipu AI / GLM)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI
- **Authentification** : Bearer token (clé API)
- **Endpoint** : `https://open.bigmodel.cn/api/paas/v4`
- **Fonctionnalités** : Streaming, appels d'outils, mode thinking (GLM-5 uniquement), contenu de raisonnement, modèle gratuit disponible (`glm-4-flash`)
- **Fenêtre de contexte** : Jusqu'à 1M tokens pour glm-5.2/glm-4-long, 200K pour glm-4.7/glm-4.6, 131K pour les autres
- **Modèle par défaut** : `glm-4-flash`
- **Vision** : Prise en charge selon le modèle (`glm-4v*`, `glm-5v*`, `*vision` → oui ; autres → non)
- **Configuration** : `apiKey`, `endpoint`, `model`, `thinkingEnabled`

### ErnieClient (Baidu Qianfan / ERNIE)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI (Qianfan v2)
- **Authentification** : Bearer token (clé API)
- **Endpoint** : `https://qianfan.baidubce.com/v2`
- **Fonctionnalités** : Streaming, appels d'outils, modèles gratuits disponibles (`ernie-speed`, `ernie-tiny`)
- **Fenêtre de contexte** : 131K tokens (131 072) pour ernie-5/ernie-speed-128k, 8K pour les autres
- **Modèle par défaut** : `ernie-5.1`
- **Vision** : Prise en charge pour ernie-5, non pour les autres
- **Configuration** : `apiKey`, `endpoint`, `model`

### HunyuanClient (Tencent Hunyuan)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI
- **Authentification** : Bearer token (clé API)
- **Endpoints** :
  - **TokenHub** (recommandé) : `https://tokenhub.tencentmaas.com/v1`
  - **Legacy** : `https://api.hunyuan.cloud.tencent.com/v1`
- **Fonctionnalités** : Streaming, appels d'outils, mode thinking (hy3/hy-2.0), contenu de raisonnement
- **Fenêtre de contexte** : 262K tokens (262 144) pour hy3/hunyuan-lite/hunyuan-t1, 131K pour hunyuan-turbos/hunyuan-a13b/hy-2.0, 32K pour hunyuan-pro/standard
- **Modèle par défaut** : `hy3`
- **Appels d'outils** : Pris en charge pour hy3, hunyuan-turbos, hunyuan-t1, hunyuan-turbo ; non pris en charge pour hunyuan-lite
- **Vision** : Non prise en charge
- **Configuration** : `apiKey`, `endpoint`, `model`, `thinkingEnabled`

### MiniMaxClient (MiniMax)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI
- **Authentification** : Bearer token (clé API)
- **Endpoint** : `https://api.minimaxi.com/v1`
- **Fonctionnalités** : Streaming, appels d'outils, mode thinking adaptatif (M3), séparation du raisonnement (`reasoning_split`), multimodal natif (image + vidéo pour M3)
- **Fenêtre de contexte** : 1M tokens (1 048 576) pour M3/M1, ~200K pour M2.x
- **Modèle par défaut** : `MiniMax-M3`
- **Vision** : Prise en charge pour M3 (multimodal natif), non pour M2.x
- **Configuration** : `apiKey`, `endpoint`, `model`

### MoonshotClient (Moonshot AI / Kimi)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI
- **Authentification** : Bearer token (clé API)
- **Endpoint** : `https://api.moonshot.cn/v1`
- **Fonctionnalités** : Streaming, appels d'outils, mode thinking (k2.5/k2.6/k2.7), contenu de raisonnement, multimodal
- **Fenêtre de contexte** : 262K tokens (262 144) pour kimi-k2, 131K pour v1-128k, 32K pour v1-32k, 8K pour v1-8k
- **Modèle par défaut** : `kimi-k2.6`
- **Vision** : Prise en charge pour kimi-k2.5/k2.6/k2.7, non pour moonshot-v1
- **Configuration** : `apiKey`, `endpoint`, `model`

### SiliconFlowClient (SiliconFlow / SiliconCloud)

- **Type** : Service IA cloud (agrégateur)
- **Protocole** : API compatible OpenAI
- **Authentification** : Bearer token (clé API)
- **Endpoint** : `https://api.siliconflow.cn/v1`
- **Fonctionnalités** : Streaming, appels d'outils, contenu de raisonnement, liste de modèles dynamique (agrège 100+ modèles open source de multiples fournisseurs)
- **Fenêtre de contexte** : 1M tokens pour deepseek-v4/glm-5.2/minimax-m3, varie selon le modèle
- **Modèle par défaut** : `deepseek-ai/DeepSeek-V3.2`
- **Vision** : Prise en charge selon le modèle (qwen3.5-9b, qwen3.6, glm-5v, kimi-k2.5/2.6/2.7, *vision* → oui)
- **Configuration** : `apiKey`, `endpoint`, `model`

### Interface des capacités IAIClient

L'interface `IAIClient` définit les capacités de chaque client IA, permettant au `ContextManager` d'adapter son comportement :

| Capacité | Type de retour | Description |
|----------|---------------|-------------|
| `StreamingMode` | `StreamingMode` | Mode de streaming pris en charge (None/Streaming/Reasoning) |
| `SupportsToolCalls` | `bool` | Prise en charge des appels d'outils |
| `ContextWindowTokens` | `int` | Taille de la fenêtre de contexte en tokens |
| `SupportsVision` | `bool` | Prise en charge de la vision (images) |
| `SupportsAudio` | `bool` | Prise en charge de l'audio |

### Modèle de fabrique de clients

Chaque type de client IA dispose d'une implémentation de fabrique correspondante `IAIClientFactory` :

- `OllamaClientFactory` — Crée des instances OllamaClient
- `DashScopeClientFactory` — Crée des instances DashScopeClient
- `DeepSeekClientFactory` — Crée des instances DeepSeekClient
- `ZhipuClientFactory` — Crée des instances ZhipuClient
- `ErnieClientFactory` — Crée des instances ErnieClient
- `HunyuanClientFactory` — Crée des instances HunyuanClient
- `MiniMaxClientFactory` — Crée des instances MiniMaxClient
- `MoonshotClientFactory` — Crée des instances MoonshotClient
- `SiliconFlowClientFactory` — Crée des instances SiliconFlowClient
- `VolcengineArkClientFactory` — Crée des instances VolcengineArkClient
- `HerdsmanClientFactory` — Crée des instances HerdsmanClient
- `LongCatClientFactory` — Crée des instances LongCatClient
- `QiniuAIClientFactory` — Crée des instances QiniuAIClient

Les fabriques fournissent :
- `CreateClient(Dictionary<string, object> config)` — Instancie un client à partir de la configuration
- `GetConfigKeyOptions(string key, ...)` — Retourne les options dynamiques pour une clé de configuration (par exemple modèles disponibles, régions)
- `GetDisplayName()` — Nom d'affichage localisé du type de client

### Liste de support des plateformes IA

#### Légende des statuts
- ✅ Implémenté
- 🚧 En développement
- 📋 Planifié
- 💡 En considération
- ⚠️ Obsolète

*Note : En raison de l'environnement réseau du développeur, l'accès aux services IA cloud étrangers [en considération] peut nécessiter des outils de proxy réseau, et le processus de débogage peut être instable.*

#### Liste des plateformes

| Plateforme | Statut | Type | Description |
|------|------|------|------|
| Ollama | ✅ | Local | Service IA local, prenant en charge le déploiement de modèles locaux |
| DashScope (Alibaba Cloud Bailian) | ✅ | Cloud | Service IA Alibaba Cloud Bailian, prenant en charge le déploiement multi-régions |
| DeepSeek (connexion directe) | ✅ | Cloud | Service IA DeepSeek, mode thinking, 1M contexte |
| Zhipu AI (GLM) | ✅ | Cloud | Service IA Zhipu GLM, mode thinking (GLM-5), vision par modèle, 1M contexte |
| Baidu Qianfan (ERNIE) | ✅ | Cloud | Service IA Baidu ERNIE, 131K contexte |
| Tencent Hunyuan | ✅ | Cloud | Service IA Tencent Hunyuan, double endpoint (TokenHub + Legacy), 262K contexte |
| MiniMax | ✅ | Cloud | Service IA MiniMax, multimodal natif (M3), 1M contexte |
| Moonshot (Kimi) | ✅ | Cloud | Service IA Kimi de Moonshot, mode thinking, vision, 262K contexte |
| SiliconFlow | ✅ | Cloud | Service IA SiliconFlow, liste de modèles dynamique (100+ modèles open source), 1M contexte |
| Volcengine Ark · Doubao | ✅ | Cloud | Service IA Doubao de ByteDance |
| Herdsman | ✅ | Local/Cloud | Moteur d'inférence sans authentification, compatible avec le format API OpenAI |
| Meituan LongCat | ✅ | Cloud | Grand modèle développé en interne par Meituan, LongCat-2.0 prend en charge 1M de contexte et mode thinking, compatible avec le format API OpenAI |
| Qiniu Cloud AI | ✅ | Cloud | Service IA cloud de Qiniu, authentification par clé API |
| 01.AI (Yi) | ⚠️ | Cloud | Service IA 01.AI (Obsolète : inscription des nouveaux utilisateurs arrêtée) |
| OpenAI | 💡 | Cloud | Service API OpenAI (série GPT) |
| Anthropic | 💡 | Cloud | Service IA Anthropic Claude |
| Google DeepMind | 💡 | Cloud | Service IA Google Gemini |
| Mistral AI | 💡 | Cloud | Service IA Mistral |
| Groq | 💡 | Cloud | Service d'inférence IA haute vitesse Groq |
| Together AI | 💡 | Cloud | Service de modèles open source Together AI |
| xAI | 💡 | Cloud | Service xAI Grok |
| Cohere | 💡 | Cloud | Service NLP entreprise Cohere |
| Replicate | 💡 | Cloud | Plateforme d'hébergement de modèles open source Replicate |
| Hugging Face | 💡 | Cloud | Communauté IA open source et plateforme de modèles Hugging Face |
| Cerebras | 💡 | Cloud | Service d'optimisation d'inférence IA Cerebras |
| Databricks | 💡 | Cloud | Plateforme IA entreprise Databricks (MosaicML) |
| Perplexity AI | 💡 | Cloud | Service de recherche-réponse IA Perplexity |
| NVIDIA NIM | 💡 | Cloud | Microservice d'inférence IA NVIDIA |

---

## Décisions de conception clés

### Le stockage comme classe d'instance (et non statique)

`IStorage` est conçu comme une instance injectable, et non comme un utilitaire statique. Cela garantit :

- Accès direct au système de fichiers — IStorage est le canal de persistance interne du système, il n'est **pas** routé via les exécuteurs.
- **L'IA ne peut pas contrôler IStorage** — Les exécuteurs gèrent les E/S initiées par les outils IA ; IStorage gère les lectures/écritures de données internes du framework. Ce sont des préoccupations fondamentalement différentes.
- Testabilité avec des implémentations mock.
- Support futur de différents backends de stockage sans modification des consommateurs.

### Les exécuteurs comme frontière de sécurité

Les exécuteurs sont le **seul** chemin pour les opérations d'E/S. Les outils nécessitant un accès disque, réseau ou ligne de commande **doivent** passer par les exécuteurs. Cette conception impose :

- Chaque exécuteur dispose d'un **thread de répartition indépendant**, avec verrouillage de thread pour la vérification des autorisations.
- Vérification centralisée des autorisations — Les exécuteurs interrogent le **Gestionnaire d'Autorisations** privé de l'être.
- File d'attente avec support de priorité et contrôle de timeout.
- Journal d'audit pour toutes les opérations externes.
- Isolation des exceptions — L'échec d'un exécuteur n'affecte pas les autres.
- Disjoncteur — Les échecs consécutifs arrêtent temporairement l'exécuteur pour prévenir les défaillances en cascade.

### ContextManager comme objet léger

Chaque `ExecuteOneRound()` crée une nouvelle instance de `ContextManager` :

1. Charge le Fichier d'Âme + l'historique de chat récent.
2. Envoie la requête au client IA.
3. Traite en boucle les appels d'outils jusqu'à ce que l'IA retourne du texte brut.
4. Persiste la réponse dans le Système de Chat.
5. Libère les ressources.

Cela maintient chaque cycle isolé et sans état.

### Auto-évolution par réécriture de classe

Les Êtres de Silicium peuvent réécrire leurs propres classes C# à l'exécution :

1. L'IA génère un nouveau code de classe (doit hériter de `SiliconBeingBase`).
2. **Contrôle des références à la compilation** (défense principale) : le compilateur ne reçoit que la liste d'assemblages autorisés — `System.IO`, `System.Reflection`, etc. sont exclus, donc le code dangereux est impossible au niveau du type.
3. **Analyse statique à l'exécution** (défense secondaire) : `SecurityScanner` scanne le code à la recherche de motifs dangereux après une compilation réussie.
4. Roslyn compile le code en mémoire.
5. En cas de succès : `SiliconBeingManager.ReplaceBeing()` échange l'instance actuelle, migre l'état et persiste le code chiffré sur disque.
6. En cas d'échec : le nouveau code est rejeté, l'implémentation existante est conservée.

Une implémentation `IPermissionCallback` personnalisée peut également être compilée et injectée via `ReplacePermissionCallback()`, permettant aux êtres de personnaliser leur propre logique d'autorisation.

Le code est stocké sur disque chiffré en AES-256. La clé de chiffrement est dérivée du GUID de l'être (en majuscules) via PBKDF2.

---

## Audit d'utilisation des tokens

`TokenUsageAuditManager` suit la consommation de tokens IA de tous les êtres :

- `TokenUsageRecord` — Enregistrement par requête (ID de l'être, modèle, tokens d'invite, tokens de complétion, horodatage)
- `TokenUsageSummary` — Statistiques agrégées
- `TokenUsageQuery` — Paramètres de requête pour filtrer les enregistrements
- Persisté via `ITimeStorage` pour les requêtes de séries temporelles
- Accessible via l'interface Web UI (UsageController) et `TokenAuditTool` (Curateur uniquement)

---

### Système de calendriers

Le système comprend **32 implémentations de calendriers**, dérivées de la classe abstraite `CalendarBase`, couvrant les principaux systèmes calendaires du monde :

| Calendrier | ID | Description |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Calendrier bouddhiste (BE), année + 543 |
| CherokeeCalendar | `cherokee` | Système de calendrier cherokee |
| ChineseLunarCalendar | `lunar` | Calendrier lunaire chinois, avec mois intercalaire |
| ChineseHistoricalCalendar | `chinese_historical` | Calendrier historique chinois, prenant en charge la numération Ganzhi et les ères impériales |
| ChulaSakaratCalendar | `chula_sakarat` | Calendrier Chula Sakarat (CS), année - 638 |
| CopticCalendar | `coptic` | Calendrier copte |
| DaiCalendar | `dai` | Calendrier Dai, avec calcul lunaire complet |
| DehongDaiCalendar | `dehong_dai` | Variante du calendrier Dai de Dehong |
| EthiopianCalendar | `ethiopian` | Calendrier éthiopien |
| FrenchRepublicanCalendar | `french_republican` | Calendrier républicain français |
| GregorianCalendar | `gregorian` | Calendrier grégorien standard |
| HebrewCalendar | `hebrew` | Calendrier hébraïque (juif) |
| IndianCalendar | `indian` | Calendrier national indien |
| InuitCalendar | `inuit` | Système de calendrier inuit |
| IslamicCalendar | `islamic` | Calendrier islamique (Hégire) |
| JapaneseCalendar | `japanese` | Calendrier japonais (ère Nengo) |
| JavaneseCalendar | `javanese` | Calendrier islamique javanais |
| JucheCalendar | `juche` | Calendrier Juche (Corée du Nord), année - 1911 |
| JulianCalendar | `julian` | Calendrier julien |
| KhmerCalendar | `khmer` | Calendrier khmer |
| MayanCalendar | `mayan` | Compte long maya |
| MongolianCalendar | `mongolian` | Calendrier mongol |
| PersianCalendar | `persian` | Calendrier persan (Hégire solaire) |
| RepublicOfChinaCalendar | `roc` | Calendrier de la République de Chine, année - 1911 |
| RomanCalendar | `roman` | Calendrier romain |
| SakaCalendar | `saka` | Calendrier Saka (Indonésie) |
| SexagenaryCalendar | `sexagenary` | Calendrier sexagésimal chinois (Ganzhi) |
| TibetanCalendar | `tibetan` | Calendrier tibétain |
| VietnameseCalendar | `vietnamese` | Calendrier lunaire vietnamien (variante zodiac du chat) |
| VikramSamvatCalendar | `vikram_samvat` | Calendrier Vikram Samvat |
| YiCalendar | `yi` | Système de calendrier Yi |
| ZoroastrianCalendar | `zoroastrian` | Calendrier zoroastrien |

`CalendarTool` fournit les opérations : `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (conversion de dates entre calendriers).

---

## Architecture de l'interface Web UI

### Système de thèmes

L'interface Web UI dispose d'un **système de thèmes enfichable**, permettant une personnalisation complète de l'interface sans modifier la logique applicative :

- **Interface ISkin** — Définit le contrat pour tous les thèmes, incluant :
  - Méthodes de rendu principales (`RenderHtml`, `RenderError`)
  - Plus de 20 méthodes de composants UI (boutons, entrées, cartes, tableaux, badges, bulles, progression, onglets, etc.)
  - Génération de CSS thématique via `CssBuilder`
  - `SkinPreviewInfo` — Palette de couleurs et icônes pour le sélecteur de thèmes de la page d'initialisation

- **Thèmes intégrés** — 7 thèmes prêts pour la production :
  - **Admin** — Interface de gestion système professionnelle, axée sur les données
  - **Chat** — Design conversationnel, centré sur les messages, pour l'interaction IA
  - **Creative** — Layout créatif et artistique, visuellement riche
  - **Dev** — Interface centrée sur le développeur et le code, avec coloration syntaxique
  - **HighContrast** — Thème d'accessibilité à contraste élevé
  - **Light** — Thème clair et épuré
  - **Minimal** — Thème minimaliste

- **Découverte de thèmes** — `SkinManager` découvre et enregistre automatiquement toutes les implémentations `ISkin` par réflexion

### Constructeurs HTML / CSS / JS

L'interface Web UI évite entièrement les fichiers de modèles, générant tout le balisage en C# :

- **`H`** — DSL de constructeur HTML en flux, pour construire des arbres HTML en code
- **`CssBuilder`** — Constructeur CSS, prenant en charge les sélecteurs et media queries
- **`JsBuilder` (`JsSyntax`)** — Constructeur JavaScript, pour les scripts inline

### Système de contrôleurs

L'interface Web UI suit un **motif de type MVC**, avec 24 contrôleurs gérant différents aspects :

| Contrôleur | Usage |
|------------|---------|
| About | Page À propos et informations sur le projet |
| Audit | Tableau de bord d'audit d'utilisation des tokens |
| Being | Gestion et état des Êtres de Silicium |
| Chat | Interface de chat en temps réel avec SSE |
| ChatHistory | Consultation de l'historique de chat, avec liste de sessions et détails des messages |
| CodeBrowser | Consultation et édition de code |
| CodeHover | Infobulles de code, avec coloration syntaxique |
| Config | Gestion de la configuration système |
| Dashboard | Vue d'ensemble du système et métriques |
| Executor | État et gestion des exécuteurs |
| Help | Système de documentation d'aide, support multilingue |
| Init | Assistant d'initialisation pour la première exécution |
| Knowledge | Visualisation et requêtes du graphe de connaissances |
| Log | Consultation des journaux système, avec filtrage par Être de Silicium |
| Memory | Navigateur de mémoire à long terme, avec filtrage avancé, statistiques et vue détaillée |
| Permission | Gestion des autorisations |
| PermissionRequest | File des demandes d'autorisation |
| Project | Gestion de projet, incluant notes de travail, système de tâches et autorisations d'outils |
| System | Administration système et surveillance à l'exécution |
| Task | Interface du système de tâches |
| Timer | Gestion du système de minuteurs, incluant l'historique d'exécution |
| ToolPermission | Gestion des autorisations d'outils, prenant en charge la configuration au niveau des Êtres de Silicium et des projets |
| Usage | Tableau de bord d'audit d'utilisation des tokens, avec graphiques de tendance et export |
| WorkNote | Gestion des notes de travail, avec recherche et génération de sommaire |

### Mises à jour en temps réel

- **SSE (Server-Sent Events)** — Pousse les mises à jour des messages de chat, de l'état des êtres et des événements système via `SSEHandler`
- **Pas de WebSocket** — Architecture plus simple utilisant SSE pour la plupart des besoins en temps réel
- **Reconnexion automatique** — Logique de reconnexion côté client pour des connexions résilientes

### Localisation

Le système prend en charge la localisation complète en **34 variantes linguistiques** :
- **Chinois (6)** : zh-CN (simplifié), zh-HK (traditionnel), zh-SG (Singapour), zh-MO (Macao), zh-TW (Taïwan), zh-MY (Malaisie)
- **Anglais (10)** : en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Espagnol (2)** : es-ES, es-MX
- **Allemand (5)** : de-DE, de-AT, de-CH, de-LU, de-LI
- **Français (3)** : fr-FR, fr-CA, fr-CH
- **Autres (8)** : ja-JP (japonais), ko-KR (coréen), cs-CZ (tchèque), it-IT (italien), pl-PL (polonais), pt-PT (portugais), pt-BR (portugais brésilien), ru-RU (russe)

La langue active est sélectionnée via `DefaultConfigData.Language` et résolue par `LocalizationManager`.

---

### Système d'automatisation de navigateur WebView (nouveau)

Le système intègre des fonctionnalités d'automatisation de navigateur WebView basées sur **Playwright** :

- **Isolation individuelle** : Chaque Être de Silicium possède sa propre instance de navigateur, ses cookies et son stockage de session, entièrement isolés et sans interférence mutuelle.
- **Mode headless** : Le navigateur fonctionne en mode headless totalement invisible pour l'utilisateur, les Êtres de Silicium opèrent de manière autonome en arrière-plan.
- **WebViewBrowserTool** : Fournit des capacités complètes de navigation, incluant :
  - Navigation de pages, clics, saisie de texte, récupération du contenu de page
  - Exécution de JavaScript, capture d'écran, attente d'apparition d'éléments
  - Gestion de l'état du navigateur et nettoyage des ressources
- **Contrôle de sécurité** : Toutes les opérations du navigateur doivent passer par la chaîne de vérification des autorisations, empêchant l'accès malveillant aux pages web.

### Système de Réseau de Connaissances (nouveau)

Le système intègre un graphe de connaissances basé sur une **structure de triplets** :

- **Représentation des connaissances** : Utilise une structure de triplet « sujet-relation-objet » (par exemple : Python-is_a-programming_language)
- **KnowledgeTool** : Fournit la gestion du cycle de vie complet des connaissances :
  - `add`/`query`/`update`/`delete` - Opérations CRUD de base
  - `search` - Recherche en texte intégral et correspondance par mots-clés
  - `get_path` - Découverte du chemin d'association entre deux concepts
  - `validate` - Vérification de l'intégrité des connaissances
  - `stats` - Statistiques du Réseau de Connaissances
- **Stockage persistant** : Les triplets de connaissances sont persistés sur le système de fichiers, prenant en charge les requêtes par index temporel.
- **Score de confiance** : Chaque entrée de connaissance possède un score de confiance (0-1), prenant en charge la correspondance floue et le classement des connaissances.
- **Système de tags** : Prise en charge de l'ajout de tags aux connaissances, facilitant la catégorisation et la recherche.

---

## Structure du répertoire de données

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Fichier d'Âme du Curateur
    │   ├── state.json       # État à l'exécution
    │   ├── code.enc         # Code de classe personnalisé chiffré AES
    │   └── permission.enc   # Rappel d'autorisation personnalisé chiffré AES
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## Moteur de stockage SpeedyPack

SiliconLife.Fast utilise le moteur de stockage SpeedyPack propriétaire (format .spk), remplaçant la solution LiteDB précédente, pour des performances de lecture/écriture extrêmes.

### Conception de l'architecture

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (cartographie │  │  (cache       │  │ (file d'écrit.│  │
│  │  répertoires) │  │   d'entrées)  │  │  asynchrone)  │  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (lecteur/enregistreur de fichiers paquet) │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              Fichier .spk (MessagePack + compression LZ4) │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (gestion      │  │ AutoCompactor│                      │
│  │  espace libre)│  │ (compression │                      │
│  │              │  │  automatique) │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Composants principaux

| Composant | Description |
|------|------|
| `SpeedyPack` | Classe principale, combinant DirectoryMap, EntryCache et WriteQueue pour des lectures/écritures à faible latence |
| `DirectoryMap` | Cartographie de répertoires en mémoire, maintenant la correspondance entre chemins virtuels et entrées de fichiers |
| `EntryCache` | Cache d'entrées, cache des entrées récemment accédées basé sur TTL |
| `WriteQueue` | File d'écriture asynchrone, mettant en file d'attente les opérations d'écriture pour exécution sur un thread d'arrière-plan |
| `FreeList` | Gestion de l'espace libre, suivant l'espace réutilisable dans les fichiers .spk |
| `PackFileReader` | Lecteur de fichiers de paquet, lisant les données depuis les fichiers .spk |
| `PackFileWriter` | Enregistreur de fichiers de paquet, écrivant les données dans les fichiers .spk |
| `SpeedyPackAutoCompactor` | Minuteur de compression automatique, compressant périodiquement les fichiers .spk pour récupérer l'espace libre |
| `SpeedyPackRegistry` | Gestionnaire singleton au niveau processus, garantissant que toute l'application utilise la même instance SpeedyPack |

### Adaptateurs de stockage

SiliconLife.Fast intègre SpeedyPack dans les interfaces système via les adaptateurs suivants :

| Adaptateur | Interface | Description |
|--------|------|------|
| `SpeedyStorage` | `IStorage` | Adaptateur de stockage clé-valeur générique |
| `SpeedyTimeStorage` | `ITimeStorage` | Adaptateur de stockage à index temporel |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Adaptateur de stockage des notes de travail |

### Options de configuration

`SpeedyPackOptions` fournit les configurations suivantes :

| Option | Type | Valeur par défaut | Description |
|------|------|--------|------|
| `CacheTtl` | `TimeSpan` | 5 minutes | Durée de vie des entrées en cache |
| `MaxCacheEntries` | `int` | 1000 | Nombre maximum d'entrées en cache |
| `ReadOnly` | `bool` | false | Mode lecture seule |

### Support des transactions

SpeedyPack prend en charge les opérations d'écriture atomiques via l'interface `IPackTransaction` :

- `SpeedyTransaction` implémente le mécanisme de transaction
- Prise en charge de l'atomicité des écritures par lots
- À la validation de la transaction, toutes les écritures réussissent ou échouent ensemble

---

## Système de plugins

SiliconLife prend en charge l'extension des fonctionnalités via un système de plugins, permettant aux développeurs tiers d'ajouter de nouvelles fonctionnalités à la plateforme.

### Interface principale

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Chargeur de plugins

`PluginLoader` est responsable du chargement des DLL de plugins depuis un répertoire spécifié et effectue des vérifications de sécurité strictes :

1. **Scan de répertoire** — Scanne tous les fichiers .dll dans le répertoire des plugins
2. **Analyse de sécurité** — Vérifie si le plugin référence des espaces de noms interdits
3. **Chargement isolé** — Charge le plugin de manière isolée via un `AssemblyLoadContext` personnalisé
4. **Gestion du cycle de vie** — Appelle les méthodes OnLoad, OnStart, OnStop, OnUnload du plugin

### Sandbox de sécurité

Le chargeur de plugins effectue les vérifications de sécurité suivantes :

| Vérification | Description |
|--------|------|
| Espaces de noms interdits | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Liste blanche d'assemblages de confiance | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Vérification des types interdits | Scan des types dangereux référencés dans le plugin |
| Vérification des membres interdits | Scan des méthodes dangereuses appelées dans le plugin |

### Intégration d'outils

Les plugins peuvent enregistrer des outils personnalisés en implémentant l'interface `ITool` :

- La méthode `ToolManager.ScanAllPluginAssemblies()` scanne toutes les implémentations ITool dans les plugins chargés
- Les outils de plugin sont automatiquement intégrés dans la boucle d'appel d'outils
- Les outils de plugin sont soumis au même système d'autorisations

### Cycle de vie des plugins

```
Chargement (OnLoad) → Démarrage (OnStart) → En cours d'exécution → Arrêt (OnStop) → Déchargement (OnUnload)
```

---

## États d'activité des Êtres de Silicium

Les Êtres de Silicium ont les états d'activité suivants :

| État | Description |
|------|------|
| `Idle` | État inactif, en attente de déclenchement par l'horloge |
| `SingleChat` | Chat en tête-à-tête en cours |
| `GroupChat` | Chat de groupe en cours |
| `Task` | Exécution d'une tâche en cours |
| `Timer` | Exécution d'un minuteur en cours |
| `Stopped` | Arrêté, en raison d'erreurs consécutives ou d'un arrêt manuel |

**Mécanisme de l'état Stopped** :
- Lorsqu'un Être de Silicium subit 10 erreurs consécutives, il passe automatiquement à l'état `Stopped`
- Une fois dans l'état Stopped, l'être n'exécute plus aucune tâche
- Lorsqu'un nouveau message de chat arrive, le compteur d'erreurs est réinitialisé et l'être reprend son fonctionnement

Transitions d'état :
```
Idle → SingleChat → Idle (chat terminé)
Idle → GroupChat → Idle (chat de groupe terminé)
Idle → Task → Idle (tâche terminée)
Idle → Timer → Idle (minuteur terminé)
Tout → Stopped (10 erreurs consécutives)
Stopped → Idle (nouveau message de chat ou redémarrage manuel)
```

---

## Moteur de flux de travail

Le moteur de flux de travail est un système à machine à états basé sur des modèles, utilisé pour piloter les processus de collaboration des Êtres de Silicium dans les espaces projet :

### Composants principaux

| Composant | Description |
|------|------|
| `WorkflowEngine` | Cœur du moteur de flux de travail, gérant les modèles et instances, exécutant les transitions d'état pilotées par Tick |
| `WorkflowTemplate` | Modèle de flux de travail, définissant l'ensemble d'états et les règles de transition |
| `WorkflowInstance` | Instance de flux de travail, liée à un projet spécifique, suivant l'état actuel |
| `WorkflowLog` | Journal de flux de travail, enregistrant l'historique des transitions d'état |

### Mécanisme de fonctionnement

- **Enregistrement de modèles** : Enregistrement des modèles de flux de travail via `RegisterTemplate()`, définissant les états et règles de transition
- **Création d'instances** : Création d'instances à partir de modèles, liées à un espace projet
- **Pilotage par Tick** : Les transitions d'état sont pilotées par le mécanisme Tick de la Boucle Principale
- **Journalisation** : Toutes les transitions d'état sont automatiquement enregistrées dans le journal

---

## Mécanisme d'oubli de la mémoire

`MemoryFadeService` est un service de dégradation temporelle simulant les caractéristiques d'oubli de la mémoire biologique :

### Mécanisme de fonctionnement

- **Exécution planifiée** : Hérite de `TickObject`, exécute un cycle de dégradation par défaut toutes les heures
- **Dégradation de l'importance** : Applique un algorithme de dégradation aux entrées de mémoire de chaque Être de Silicium, réduisant le score d'importance
- **Archivage automatique** : Les mémoires dont l'importance est inférieure au seuil sont automatiquement archivées (`ArchiveFadingMemories()`)
- **Suivi statistique** : Enregistrement du nombre de cycles de dégradation, du nombre d'entrées avec changement d'état, etc.

### Processus de dégradation

```
MemoryFadeService.OnTick()
  └── Parcours de tous les Êtres de Silicium
       └── being.Memory.ApplyDecay()      # Application de la dégradation de l'importance
       └── being.Memory.ArchiveFadingMemories()  # Archivage des mémoires de faible importance
```

---

## Système d'espace de projet

L'espace de projet est un mécanisme de gestion d'espace prenant en charge la collaboration multi-Êtres de Silicium :

### Fonctionnalités principales

- **Cycle de vie du projet** : Création → Actif → Archivage → Destruction
- **Attribution de rôles** : Prise en charge de l'attribution de rôles de projet aux Êtres de Silicium
- **Isolation des autorisations d'outils** : Configuration des autorisations d'outils au niveau du projet, indépendante des autorisations au niveau de l'Être de Silicium
- **Notes de travail** : Système de notes par pages dans l'espace projet, prenant en charge la génération de sommaires et la recherche par mots-clés
- **Suivi des tâches** : Gestion des tâches au niveau du projet, prenant en charge la création, l'attribution et le suivi de l'état
- **Intégration de flux de travail** : Les projets peuvent être liés à des modèles de flux de travail, pilotant les processus de collaboration

### Outils associés

| Outil | Usage |
|------|------|
| `ProjectTool` | Gestion de l'espace projet (création, archivage, destruction, attribution de rôles) |
| `ProjectTaskTool` | Gestion des tâches de projet (création, attribution, mise à jour de l'état) |
| `ProjectWorkNoteTool` | Notes de travail de projet (création, recherche, génération de sommaires) |
| `ProjectWorkTool` | Opérations de travail de projet (création de tâches, chat de groupe, diffusion, achèvement du projet) |

---

## Système de Compétences

Les compétences (Compétence) sont une couche d'abstraction réutilisable de « orchestration d'outils + modèles de prompts », encapsulant les flux de travail courants en unités de capacité déclarables, évolutives et ordonnançables.

### Structure en couches

| Couche | Emplacement | Responsabilité |
|------|------|------|
| Couche cœur | `SiliconLife.Core/Skills/` | SkillDefinition, SkillManager (enregistrement + moteur d'exécution), SkillMarkdownParser, SkillFileManager, AutoSkillTickObject, SkillMetadataCompleter |
| Couche commune | `SiliconLife.Common` | BuiltinSkills (3 compétences intégrées), SkillTool (outil `skill`) |
| Couche applicative | `SiliconLife.App/Web/` | SkillController + SkillView (page de gestion des compétences) |

### Flux d'exécution

```
Appel de fonction IA (id de compétence) ou déclenchement par ordonnanceur
        ↓
SkillManager.ExecuteSkill
  ├─ Vérification : commutateur global / autorisations / protection anti-récursion
  ├─ Limitation des paramètres : maxToolRound = Min(valeur compétence, GlobalMaxToolRound)
  │            timeout = Min(valeur compétence, GlobalSkillTimeoutSeconds)
  ├─ MergePermissions : autorisations de l'Être ∪ restrictions de compétence (côté strict gagne)
  ├─ FillTemplate : remplissage des espaces réservés {param} → sous-AIRequest
  └─ Sous-boucle (maximum maxToolRound tours) : IA ↔ outils (liste blanche uniquement)
        ↓
HandleCompletion (OnCompleteAction)
  none / write_memory / notify_curator / broadcast
```

### Conceptions clés

- **Ordonnancement transparent** : les compétences sont injectées sous forme de `ToolDefinition` dans `AIRequest.Tools`, l'IA n'en a pas conscience ; dans `ContextManager.ExecuteToolCalls`, les appels de compétence sont prioritaires sur les outils de même nom
- **Quatre sources** : `Builtin` (framework) / `Plugin` (ISkillProvider) / `Being` (Être de Silicium à l'exécution) / `User` (UI Web), le rechargement à chaud conserve les deux premières catégories et remplace les deux dernières
- **Markdown prioritaire** : `skills/{id}.md` (front-matter YAML + corps) prioritaire sur `.json` ; lors de la sauvegarde en Markdown pur, l'IA complète les métadonnées (les champs utilisateur ne sont pas écrasés)
- **Ordonnancement automatique** : `AutoSkillTickObject` (intervalle de vérification de 30 secondes) prend en charge trois expressions d'ordonnancement : `HH:mm`, `N s|m|h|d`, sous-ensemble cron, avec protection anti-réentrée
- **Garde-fous multiples** : commutateur global, quota personnalisé (`MaxCustomSkillsPerBeing`, 50 par défaut), limite globale de tours/délai d'expiration, autorisation d'action `execute` au niveau compétence, liste blanche d'outils, protection anti-récursion

---

## Intégration MCP

L'intégration MCP (Model Context Protocol) permet aux Êtres de Silicium d'appeler les outils fournis par des serveurs MCP externes, étendant les capacités sans nécessiter de code.

### Architecture

```
Utilisateur (UI Web /mcp) ──ajouter/activer/désactiver/supprimer──→ McpManager (singleton)
                                          │
                              ┌───────────┼───────────┐
                              ↓           ↓           ↓
                        McpClientConnection × N (stdio / http)
                              │
                              └→ ListTools → encapsulé en SiliconLife.Collective.McpTool
                                            nommé mcp_{serverId}_{toolName}
                                                  │
                          McpManager.SyncToolsForBeing(being) injection
                                                  ↓
                                    ToolManager (même traitement que les outils intégrés)
```

### Conceptions clés

- **Double transport** : `stdio` (sous-processus local : command + arguments + env) et `http` (point de terminaison distant)
- **Isolation par nommage** : le préfixe `mcp_{serverId}_{toolName}` évite les conflits avec les outils intégrés/plugins
- **Souveraineté utilisateur** : l'ajout, la suppression et l'activation/désactivation de serveurs ne sont possibles que via l'UI Web ; l'outil `mcp` côté IA ne fournit que des requêtes en lecture seule (status/list_servers/list_tools)
- **Cohérence des autorisations** : les outils encapsulés déclarent automatiquement une seule action `execute`, intégrés dans la matrice d'autorisations d'outils, désactivables par Être de Silicium/projet
- **Persistance de configuration** : la liste `McpServers` est stockée dans config.json, `McpEnabled` est le commutateur global

---

## Architecture Multi-instance de la Plateforme IM

La plateforme IM adopte une architecture « configuration multi-instance + fournisseur agrégé », permettant la connexion simultanée à plusieurs plateformes de chat.

### Composants principaux

| Composant | Responsabilité |
|------|------|
| `IMPlatformConfig` | Configuration d'instance unique (platform/enabled/dictionnaire de configuration), `IMPlatforms` est une liste, chaque instance avec activation/désactivation indépendante |
| `IMProviderRegistry` | Registre des métadonnées de plateforme : schema des champs de configuration, modèles de points de terminaison OAuth, fabrique de Provider, liens d'aide |
| `AggregateIMProvider` | Agrégation multi-plateforme : réception de messages (déclenchement par toute plateforme), envoi de messages (diffusion, échec d'une plateforme isolé silencieusement), demande d'autorisation (le premier répondant gagne la course) |
| `ImOAuthService` | Assistant d'autorisation OAuth (singleton) : protection state anti-CSRF, délai d'expiration de 5 minutes, écriture des jetons dans la configuration, push d'état SSE |
| `ConfigSecretResolver` | Analyse des espaces réservés `${ENV_VAR}` : remplacement par copie profonde, les clés en clair ne sont pas réécrites dans config.json |
| `IMManager` | Routage des messages : file d'attente par ChannelId (traitement séquentiel) → ChatSystem → déclenchement de la réflexion de l'Être de Silicium |

### Plateformes prises en charge

| Plateforme | AuthModes | Accès aux événements | Remarques |
|------|-----------|---------|------|
| UI Web | manual | SSE (intégré) | Toujours disponible, complétée automatiquement |
| Feishu | manual / **oauth** | Rappel HTTP (vérification de signature + déchiffrement AES) | Prise en charge de l'assistant d'autorisation OAuth en un clic |
| WeChat Enterprise | manual | Rappel HTTP (WXBizMsgCrypt) | Nécessite un rappel sur réseau public |
| DingTalk | manual | Stream (WebSocket) / HTTP | Mode Stream par défaut, sans réseau public requis |

### Flux de messages

```
Feishu/WeChat Enterprise/DingTalk/WebUI (entrant)
  → IIMProvider.MessageReceived
  → IMManager.OnMessageReceived (file d'attente par ChannelId, séquentiel)
  → ChatSystem.AddMessage → réflexion IA de l'Être de Silicium
  → IMManager.SendMessageAsync / SendStreamChunkAsync (sortant)
  → AggregateIMProvider diffuse vers toutes les plateformes activées
```
