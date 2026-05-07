# Architecture

> **Version : v0.1.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | **Français** | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md)

## Architecture en double version

Ce projet propose deux versions d'implémentation, partageant la même conception architecturale, mais différant dans le stockage et l'optimisation des performances :

### SiliconLife.Default (Version standard)
- **Positionnement** : Implémentation standard, principalement pour la vérification de faisabilité architecturale
- **Mode d'exécution** : Application console
- **Méthode de stockage** : Stockage JSON pur en système de fichiers
- **Scénarios applicables** : Exigences élevées de sécurité des données, ressources mémoire limitées, petit volume de données
- **Description du rôle** : Implémentation de référence pour la vérification architecturale, offre une exécution simple et fiable, adaptée au premier contact, au débogage de développement ou aux scénarios prioritaires en sécurité des données

### SiliconLife.Fast (Version haute performance)
- **Positionnement** : Version principale de production
- **Mode d'exécution** : Application Windows Forms (prend en charge la barre d'état système)
- **Méthode de stockage** : Stockage en mémoire SpeedyPack + persistance par lot asynchrone (format de fichier .spk)
- **Scénarios applicables** : Concurrence élevée, faible latence, grands volumes de données
- **Caractéristiques** :
  - Exécution en arrière-plan dans la barre d'état, surveillance en temps réel via la fenêtre d'état
  - Moteur SpeedyPack + compression automatique garantissant la sécurité des données
  - Architecture Component UI, 30+ composants déclaratifs
  - 7 thèmes d'apparence, prend en charge la détection et la commutation automatiques
  - Outil de rechargement à chaud pour les mises à jour et redémarrages en ligne
- **Amélioration des performances** : Latence de lecture réduite de 1000x, latence d'écriture réduite de 15000x
- **Description du rôle** : Implémentation prête pour la production avec optimisation approfondie, avec exécution en arrière-plan dans la barre d'état, moteur SpeedyPack + compression automatique, le meilleur choix pour l'exploitation à long terme et les véritables environnements de production

> **Note** : L'architecture décrite dans ce document s'applique aux deux versions, seules les implémentations de stockage diffèrent. SiliconLife.Default sert de référence pour la vérification architecturale, SiliconLife.Fast est la version principale recommandée pour la production.

---

## Concepts fondamentaux

### Silicon Being

Chaque agent IA du système est un **Silicon Being** — une entité autonome avec sa propre identité, personnalité et capacités. Chaque Silicon Being est piloté par un **fichier âme** (prompt Markdown) qui définit ses modèles de comportement.

### Silicon Curator

Le **Silicon Curator** est un Silicon Being spécial avec les permissions système les plus élevées. Il agit comme administrateur système :

- Création et gestion des autres Silicon Beings
- Analyse des requêtes utilisateur et décomposition en tâches
- Distribution des tâches aux Silicon Beings appropriés
- Surveillance de la qualité d'exécution et gestion des échecs
- Réponse aux messages utilisateur avec **ordonnancement prioritaire** (voir ci-dessous)

### Fichier âme

Fichier Markdown stocké dans le répertoire de données de chaque Silicon Being (`soul.md`). Il est injecté comme prompt système dans chaque requête IA, définissant la personnalité, les modèles de décision et les contraintes comportementales du Being.

---

## Ordonnancement : Ordonnancement équitable par time-slice

### Boucle principale + Objets horloge

Le système exécute une **boucle principale pilotée par horloge** sur un thread dédié :

```
Boucle principale (thread dédié, chien de garde + disjoncteur)
  └── Objet horloge A (Priorité=0, Intervalle=100ms)
  └── Objet horloge B (Priorité=1, Intervalle=500ms)
  └── SiliconBeingManager (déclenché par horloge de la boucle principale)
        └── SiliconBeingRunner → Silicon Being 1 → Déclenchement horloge → Exécution d'un cycle
        └── SiliconBeingRunner → Silicon Being 2 → Déclenchement horloge → Exécution d'un cycle
        └── SiliconBeingRunner → Silicon Being 3 → Déclenchement horloge → Exécution d'un cycle
        └── ...
```

Décisions de conception clés :

- **Les Silicon Beings n'héritent pas de l'objet horloge.** Ils ont leur propre méthode `Tick()`, appelée par `SiliconBeingManager` via `SiliconBeingRunner`, et non enregistrés directement dans la boucle principale.
- **SiliconBeingManager** est déclenché directement par l'horloge de la boucle principale et agit comme proxy unique pour tous les Beings.
- **SiliconBeingRunner** encapsule le `Tick()` de chaque Being sur un thread temporaire, avec timeout et disjoncteur par Being (3 timeouts consécutifs → 1 minute de refroidissement).
- L'exécution de chaque Being est limitée à **un cycle** de requête IA + appel d'outils par déclenchement d'horloge, garantissant qu'aucun Being ne peut monopoliser la boucle principale.
- **Moniteur de performance** suit les temps d'exécution de l'horloge pour l'observabilité.

### Réponse prioritaire du Curator

Lorsqu'un utilisateur envoie un message au Silicon Curator :

1. Le Being actuel (par exemple Being A) termine son cycle en cours — **pas d'interruption**.
2. Le gestionnaire **saute le reste de la file d'attente**.
3. La boucle **reprend depuis le Curator**, lui permettant de s'exécuter immédiatement.

Cela garantit la réactivité aux interactions utilisateur sans perturber les tâches en cours.

---

## Architecture des composants

```
┌─────────────────────────────────────────────────────────┐
│                        Hôte principal                    │
│  (Hôte unifié — assemble et gère tous les composants)    │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Boucle    │  │ Localisateur │  │    Configuration  │  │
│  │ principale│  │ de services  │  │                   │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │     SiliconBeingManager (objet horloge)           │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curator   │ │Being A  │ │Being B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Services partagés                    │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Système  │  │ Stockage │  │ Gestionnaire     │  │   │
│  │  │ de chat  │  │          │  │ de permissions   │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ Client   │  │Exécuteur │  │ Gestionnaire     │  │   │
│  │  │ IA       │  │          │  │ d'outils         │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │ Chargeur │  │ Réseau   │                        │   │
│  │  │ de plugins│  │ de conn. │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Exécuteurs                       │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Exécuteur│  │ Exécuteur│  │  Exécuteur       │  │   │
│  │  │ disque   │  │ réseau   │  │  ligne de cmd.   │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │          Fournisseurs de messagerie instantanée   │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Fourn.   │  │ Fourn.   │  │  Feishu / ...    │  │   │
│  │  │ console  │  │ Web      │  │  Fournisseur     │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Localisateur de services

`ServiceLocator` est un registre singleton thread-safe fournissant l'accès à tous les services principaux :

| Propriété | Type | Description |
|-----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Gestionnaire central de sessions de chat |
| `IMManager` | `IMManager` | Routeur de fournisseurs de messagerie instantanée |
| `AuditLogger` | `AuditLogger` | Piste d'audit des permissions |
| `GlobalAcl` | `GlobalACL` | Liste de contrôle d'accès globale |
| `BeingFactory` | `ISiliconBeingFactory` | Fabrique de création de Beings |
| `BeingManager` | `SiliconBeingManager` | Gestionnaire de cycle de vie des Beings actifs |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Chargeur de compilation dynamique |
| `TokenUsageAudit` | `ITokenUsageAudit` | Suivi de l'utilisation des tokens |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Rapports d'utilisation des tokens |

Il maintient également un registre de `PermissionManager` par Being, indexé par le GUID du Being.

---

## Système de chat

### Types de sessions

Le système de chat prend en charge trois types de sessions via `SessionBase` :

| Type | Classe | Description |
|------|--------|-------------|
| `SingleChat` | `SingleChatSession` | Conversation en tête-à-tête entre deux participants |
| `GroupChat` | `GroupChatSession` | Chat de groupe multi-participants |
| `Broadcast` | `BroadcastChannel` | Canal ouvert avec ID fixe ; les Beings s'abonnent dynamiquement et ne reçoivent les messages qu'après abonnement |

### Canaux de diffusion

`BroadcastChannel` est un type de session spécial pour les annonces à l'échelle du système :

- **ID de canal fixe** — Contrairement à `SingleChatSession` et `GroupChatSession`, l'ID du canal est une constante connue, et non dérivé des GUID des membres.
- **Abonnement dynamique** — Les Beings s'abonnent/se désabonnent à l'exécution ; ils ne reçoivent que les messages publiés après leur abonnement.
- **Filtrage des messages en attente** — `GetPendingMessages()` ne retourne que les messages publiés après l'heure d'abonnement du Being et non encore lus.
- **Géré par le système de chat** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Messages de chat

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
| `Role` | `MessageRole` | Rôle de conversation IA (utilisateur, assistant, outil) |
| `ToolCallId` | `string?` | ID d'appel d'outil pour les messages de résultat d'outil |
| `ToolCallsJson` | `string?` | JSON sérialisé des appels d'outils pour les messages assistant |
| `Thinking` | `string?` | Raisonnement en chaîne de pensée de l'IA |
| `PromptTokens` | `int?` | Nombre de tokens dans le prompt (entrée) |
| `CompletionTokens` | `int?` | Nombre de tokens dans la complétion (sortie) |
| `TotalTokens` | `int?` | Nombre total de tokens utilisés (entrée + sortie) |
| `FileMetadata` | `FileMetadata?` | Métadonnées de fichier jointes (si le message contient un fichier) |

### File d'attente de messages de chat

`ChatMessageQueue` est un système de file d'attente de messages thread-safe pour la gestion asynchrone des messages de chat :

- **Thread-safe** — Utilise des mécanismes de verrouillage pour garantir la sécurité d'accès concurrentiel
- **Traitement asynchrone** — Prend en charge l'enfilement et le défilement asynchrones des messages
- **Ordonnancement des messages** — Maintient l'ordre chronologique des messages
- **Opérations par lot** — Prend en charge la récupération par lot des messages

### Métadonnées de fichier

`FileMetadata` gère les informations sur les fichiers joints aux messages de chat :

- **Informations sur le fichier** — Nom, taille, type, chemin
- **Horodatage de téléchargement** — Timestamp du téléchargement du fichier
- **Téléchargeur** — ID de l'utilisateur ou du Silicon Being ayant téléchargé le fichier

### Gestionnaire d'annulation de flux

`StreamCancellationManager` fournit un mécanisme d'annulation pour les réponses IA en streaming :

- **Contrôle de flux** — Prend en charge l'annulation des réponses IA en streaming en cours
- **Nettoyage des ressources** — Nettoyage correct des ressources associées lors de l'annulation
- **Sécurité concurrentielle** — Prend en charge la gestion simultanée de plusieurs flux

### Historique de chat

La fonctionnalité d'historique de chat permet aux utilisateurs de parcourir les conversations passées des Silicon Beings :

- **Liste des sessions** — Affiche toutes les sessions historiques
- **Détails des messages** — Affiche l'historique complet des messages
- **Vue chronologique** — Présente les messages dans l'ordre chronologique
- **Support API** — Fournit une API RESTful pour récupérer les données de sessions et de messages

---

## Système de clients IA

Le système prend en charge plusieurs backends IA via l'interface `IAIClient` :

### OllamaClient

- **Type** : Service IA local
- **Protocole** : API HTTP Ollama native (`/api/chat`, `/api/generate`)
- **Fonctionnalités** : Streaming, appels d'outils, hébergement de modèles locaux
- **Configuration** : `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud DashScope)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI (`/compatible-mode/v1/chat/completions`)
- **Authentification** : Bearer token (clé API)
- **Fonctionnalités** : Streaming, appels d'outils, contenu de raisonnement (chaîne de pensée), déploiement multi-région
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
- **Découverte de modèles** : Récupération des modèles disponibles depuis l'API DashScope à l'exécution ; repli vers une liste sélectionnée en cas de panne réseau

### VolcengineArkClient (Volcengine Ark)

- **Type** : Service IA cloud
- **Protocole** : API compatible OpenAI
- **Authentification** : Bearer token (clé API)
- **Fonctionnalités** : Prise en charge des modes streaming et non-streaming, contrôle de vitesse double intégré
  - Contrôle de vitesse propre : Application d'un intervalle minimum entre les requêtes
  - Limitation de vitesse du serveur : Gestion des erreurs 429, nouvelle tentative avec backoff exponentiel
- **Configuration** : `apiKey`, `endpoint`, `model`
- **Caractéristiques** : Service IA de ByteDance, prend en charge divers modèles Doubao

### Pattern Factory de clients IA

Chaque type de client IA a une implémentation factory correspondante de `IAIClientFactory` :

- `OllamaClientFactory` — Crée des instances OllamaClient
- `DashScopeClientFactory` — Crée des instances DashScopeClient
- `VolcengineArkClientFactory` — Crée des instances VolcengineArkClient

La factory fournit :
- `CreateClient(Dictionary<string, object> config)` — Instancie un client depuis la configuration
- `GetConfigKeyOptions(string key, ...)` — Retourne les options dynamiques pour une clé de configuration (ex. modèles disponibles, régions)
- `GetDisplayName()` — Nom d'affichage localisé du type de client

### Liste de support des plateformes IA

#### Légende des statuts
- ✅ Implémenté
- 🚧 En développement
- 📋 Planifié
- 💡 En considération

*Note : En raison de l'environnement réseau du développeur, la connexion aux services IA cloud internationaux marqués [En considération] peut nécessiter des outils proxy réseau, et le processus de débogage peut être instable.*

#### Liste des plateformes

| Plateforme | Statut | Type | Description |
|------------|--------|------|-------------|
| Ollama | ✅ | Local | Service IA local, prend en charge le déploiement de modèles locaux |
| DashScope (Alibaba Cloud) | ✅ | Cloud | Service IA DashScope d'Alibaba Cloud, déploiement multi-région |
| Baidu Qianfan (Wenxin) | 📋 | Cloud | Service IA Wenxin de Baidu |
| Zhipu AI (GLM) | 📋 | Cloud | Service IA Zhipu Qingyan |
| Moonshot (Kimi) | 📋 | Cloud | Service IA Kimi de Moonshot AI |
| Volcengine Ark.Doubao | 📋 | Cloud | Service IA Doubao de ByteDance |
| DeepSeek (connexion directe) | 📋 | Cloud | Service IA DeepSeek |
| 01.AI (Yi) | 📋 | Cloud | Service IA 01.AI |
| Tencent Hunyuan | 📋 | Cloud | Service IA Tencent Hunyuan |
| SiliconFlow | 📋 | Cloud | Service IA SiliconFlow |
| MiniMax | 📋 | Cloud | Service IA MiniMax |
| OpenAI | 💡 | Cloud | Service API OpenAI (série GPT) |
| Anthropic | 💡 | Cloud | Service IA Anthropic Claude |
| Google DeepMind | 💡 | Cloud | Service Google Gemini |
| Mistral AI | 💡 | Cloud | Service IA Mistral |
| Groq | 💡 | Cloud | Service d'inférence IA haute vitesse Groq |
| Together AI | 💡 | Cloud | Service de modèles open source Together AI |
| xAI | 💡 | Cloud | Service xAI Grok |
| Cohere | 💡 | Cloud | Service NLP entreprise Cohere |
| Replicate | 💡 | Cloud | Plateforme d'hébergement de modèles open source Replicate |
| Hugging Face | 💡 | Cloud | Communauté IA open source et plateforme de modèles Hugging Face |
| Cerebras | 💡 | Cloud | Service d'inférence IA optimisé Cerebras |
| Databricks | 💡 | Cloud | Plateforme IA entreprise Databricks (MosaicML) |
| Perplexity AI | 💡 | Cloud | Service de recherche-réponse IA Perplexity |
| NVIDIA NIM | 💡 | Cloud | Microservice d'inférence IA NVIDIA |

---

## Décisions de conception clés

### Stockage comme classe d'instance (et non statique)

`IStorage` est conçu comme une instance injectable, et non un utilitaire statique. Cela garantit :

- Accès direct au système de fichiers — IStorage est le canal de persistance interne du système, **non** routé via les exécuteurs.
- **L'IA ne peut pas contrôler IStorage** — Les exécuteurs gèrent les E/S initiés par les outils IA ; IStorage gère les lectures/écritures internes du framework. Ce sont des préoccupations fondamentalement différentes.
- Testable avec des implémentations simulées.
- Support futur de différents backends de stockage sans modification des consommateurs.

### Exécuteurs comme frontière de sécurité

Les exécuteurs sont le **seul** chemin pour les opérations d'E/S. Les outils nécessitant un accès disque, réseau ou ligne de commande **doivent** passer par les exécuteurs. Cette conception impose :

- Chaque exécuteur possède un **thread d'ordonnancement séparé**, avec verrouillage de thread pour la validation des permissions.
- Vérification centralisée des permissions — les exécuteurs interrogent le **gestionnaire de permissions privé** du Being.
- File d'attente de requêtes avec support de priorité et contrôle de timeout.
- Journalisation d'audit pour toutes les opérations externes.
- Isolation des exceptions — l'échec d'un exécuteur n'affecte pas les autres.
- Disjoncteur — les échecs consécutifs arrêtent temporairement l'exécuteur pour prévenir les défaillances en cascade.

### ContextManager comme objet léger

Chaque `ExecuteOneRound()` crée une nouvelle instance de `ContextManager` :

1. Charge le fichier âme + l'historique de chat récent.
2. Envoie la requête au client IA.
3. Traite en boucle les appels d'outils jusqu'à ce que l'IA retourne du texte pur.
4. Persiste la réponse dans le système de chat.
5. Libère les ressources.

Cela maintient chaque cycle isolé et sans état.

### Auto-évolution par réécriture de classe

Les Silicon Beings peuvent réécrire leurs propres classes C# à l'exécution :

1. L'IA génère un nouveau code de classe (doit hériter de `SiliconBeingBase`).
2. **Contrôle des références à la compilation** (défense principale) : le compilateur ne reçoit que la liste d'assemblies autorisés — `System.IO`, `System.Reflection`, etc. sont exclus, rendant le code dangereux impossible au niveau du type.
3. **Analyse statique à l'exécution** (défense secondaire) : `SecurityScanner` scanne le code pour des motifs dangereux après compilation réussie.
4. Roslyn compile le code en mémoire.
5. En cas de succès : `SiliconBeingManager.ReplaceBeing()` échange l'instance actuelle, migre l'état, et persiste le code chiffré sur disque.
6. En cas d'échec : le nouveau code est rejeté, l'implémentation existante est conservée.

Des implémentations personnalisées de `IPermissionCallback` peuvent également être compilées et injectées via `ReplacePermissionCallback()`, permettant aux Beings de personnaliser leur propre logique de permissions.

Le code est stocké sur disque chiffré en AES-256. La clé de chiffrement est dérivée du GUID du Being (majuscules) via PBKDF2.

---

## Audit d'utilisation des tokens

`TokenUsageAuditManager` suit la consommation de tokens IA de tous les Beings :

- `TokenUsageRecord` — Enregistrement par requête (ID du Being, modèle, tokens prompt, tokens complétion, horodatage)
- `TokenUsageSummary` — Statistiques agrégées
- `TokenUsageQuery` — Paramètres de requête pour filtrer les enregistrements
- Persistance via `ITimeStorage` pour les requêtes de séries temporelles
- Accessible via l'interface Web (UsageController) et `TokenAuditTool` (Curator uniquement)

---

### Système de calendrier

Le système comprend **32 implémentations de calendriers**, dérivées de la classe abstraite `CalendarBase`, couvrant les principaux systèmes calendaires du monde :

| Calendrier | ID | Description |
|------------|-----|-------------|
| BuddhistCalendar | `buddhist` | Calendrier bouddhiste (BE), année + 543 |
| CherokeeCalendar | `cherokee` | Système de calendrier cherokee |
| ChineseLunarCalendar | `lunar` | Calendrier lunaire chinois, avec mois intercalaires |
| ChineseHistoricalCalendar | `chinese_historical` | Calendrier historique chinois, cycle Ganzhi et ères impériales |
| ChulaSakaratCalendar | `chula_sakarat` | Calendrier Chula Sakarat (CS), année - 638 |
| CopticCalendar | `coptic` | Calendrier copte |
| DaiCalendar | `dai` | Calendrier Dai avec calcul lunaire complet |
| DehongDaiCalendar | `dehong_dai` | Variante Dai Dehong |
| EthiopianCalendar | `ethiopian` | Calendrier éthiopien |
| FrenchRepublicanCalendar | `french_republican` | Calendrier républicain français |
| GregorianCalendar | `gregorian` | Calendrier grégorien standard |
| HebrewCalendar | `hebrew` | Calendrier hébraïque (juif) |
| IndianCalendar | `indian` | Calendrier national indien |
| InuitCalendar | `inuit` | Système de calendrier inuit |
| IslamicCalendar | `islamic` | Calendrier islamique (Hégire) |
| JapaneseCalendar | `japanese` | Calendrier des ères japonaises (Nengo) |
| JavaneseCalendar | `javanese` | Calendrier islamique javanais |
| JucheCalendar | `juche` | Calendrier Juche (Corée du Nord), année - 1911 |
| JulianCalendar | `julian` | Calendrier julien |
| KhmerCalendar | `khmer` | Calendrier khmer |
| MayanCalendar | `mayan` | Calendrier long maya |
| MongolianCalendar | `mongolian` | Calendrier mongol |
| PersianCalendar | `persian` | Calendrier persan (Hégire solaire) |
| RepublicOfChinaCalendar | `roc` | Calendrier de la République de Chine (Minguo), année - 1911 |
| RomanCalendar | `roman` | Calendrier romain |
| SakaCalendar | `saka` | Calendrier Saka (Indonésie) |
| SexagenaryCalendar | `sexagenary` | Calendrier Ganzhi chinois (sexagésimal) |
| TibetanCalendar | `tibetan` | Calendrier tibétain |
| VietnameseCalendar | `vietnamese` | Calendrier lunaire vietnamien (variante zodiaque du Chat) |
| VikramSamvatCalendar | `vikram_samvat` | Calendrier Vikram Samvat |
| YiCalendar | `yi` | Système de calendrier Yi |
| ZoroastrianCalendar | `zoroastrian` | Calendrier zoroastrien |

`CalendarTool` fournit les opérations : `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (conversion de dates entre calendriers).

---

## Architecture de l'interface Web

### Système de skins

L'interface Web dispose d'un **système de skins enfichable**, permettant une personnalisation complète de l'interface sans modifier la logique applicative :

- **Interface ISkin** — Définit le contrat pour tous les skins, incluant :
  - Méthodes de rendu principales (`RenderHtml`, `RenderError`)
  - 20+ méthodes de composants UI (boutons, entrées, cartes, tables, badges, bulles, progression, onglets, etc.)
  - Génération de CSS thématique via `CssBuilder`
  - `SkinPreviewInfo` — Palette de couleurs et icône pour le sélecteur de skins de la page d'initialisation

- **Skins intégrés** — 7 skins prêts pour la production :
  - **Admin** — Interface d'administration professionnelle, axée sur les données
  - **Chat** — Design conversationnel, centré sur les messages, pour l'interaction IA
  - **Creative** — Disposition artistique et visuellement riche pour les flux de travail créatifs
  - **Dev** — Interface centrée sur le développeur et le code, avec coloration syntaxique
  - **HighContrast** — Thème d'accessibilité à contraste élevé
  - **Light** — Thème clair et épuré
  - **Minimal** — Thème minimaliste

- **Découverte de skins** — `SkinManager` découvre et enregistre automatiquement toutes les implémentations `ISkin` via réflexion

### Constructeurs HTML / CSS / JS

L'interface Web évite entièrement les fichiers de modèles, générant tout le balisage en C# :

- **`H`** — DSL de constructeur HTML en flux, pour construire des arbres HTML dans le code
- **`CssBuilder`** — Constructeur CSS avec support des sélecteurs et media queries
- **`JsBuilder` (`JsSyntax`)** — Constructeur JavaScript pour les scripts en ligne

### Système de contrôleurs

L'interface Web suit un **pattern de type MVC**, avec 22 contrôleurs gérant différents aspects :

| Contrôleur | Objectif |
|------------|----------|
| About | Page À propos et informations du projet |
| Being | Gestion et statut des Silicon Beings |
| Chat | Interface de chat en temps réel avec SSE |
| ChatHistory | Historique de chat, liste de sessions et détails des messages |
| CodeBrowser | Consultation et édition de code |
| CodeHover | Info-bulles de code avec coloration syntaxique |
| Config | Gestion de la configuration système |
| Dashboard | Vue d'ensemble du système et métriques |
| Executor | Statut et gestion des exécuteurs |
| Help | Système de documentation d'aide, support multilingue |
| Init | Assistant d'initialisation pour la première exécution |
| Knowledge | Visualisation et requêtes du graphe de connaissances |
| Log | Visualiseur de journaux système, filtrage par Being |
| Memory | Navigateur de mémoire à long terme, filtrage avancé, statistiques et vue détaillée |
| Permission | Gestion des permissions |
| PermissionRequest | File d'attente des requêtes de permissions |
| Project | Gestion de projets, avec notes de travail et système de tâches |
| System | Surveillance des performances système et métriques |
| Task | Interface du système de tâches |
| Timer | Gestion du système de minuteries, avec historique d'exécution |
| Usage | Tableau de bord d'utilisation des tokens, avec graphiques de tendance et export |
| WorkNote | Gestion des notes de travail, recherche et génération de sommaire |

### Mises à jour en temps réel

- **SSE (Server-Sent Events)** — Pousse les mises à jour des messages de chat, statut des Beings et événements système via `SSEHandler`
- **Pas de WebSocket** — Architecture plus simple utilisant SSE pour la plupart des besoins en temps réel
- **Reconnexion automatique** — Logique de reconnexion côté client pour des connexions résilientes

### Localisation

Le système prend en charge la localisation complète de **29 variantes linguistiques** :
- **Chinois (6)** : zh-CN (simplifié), zh-HK (traditionnel), zh-SG (Singapour), zh-MO (Macao), zh-TW (Taïwan), zhMY (Malaisie)
- **Anglais (10)** : en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Allemand (5)** : de-DE, de-AT, de-CH, de-LU, de-LI
- **Français (3)** : fr-FR, fr-CA, fr-CH
- **Autres (3)** : ja-JP (japonais), ko-KR (coréen), cs-CZ (tchèque)

---

## États d'activité des Silicon Beings

Les Silicon Beings ont les états d'activité suivants :

| État | Description |
|------|------|
| `Idle` | État inactif, en attente du déclencheur d'horloge |
| `Working` | En cours d'exécution d'un tour de requête IA + appel d'outil |
| `Error` | Une erreur s'est produite pendant l'exécution |
| `Stopped` | Arrêté, dû à des erreurs consécutives ou un arrêt manuel |

**Mécanisme d'état Stopped** :
- Lorsqu'un Silicon Being subit 10 erreurs consécutives, il entre automatiquement dans l'état `Stopped`
- Une fois dans l'état Stopped, le Being n'exécutera plus aucune tâche
- Une intervention manuelle est requise pour redémarrer

Transitions d'état :
```
Idle → Working → Idle (terminaison normale)
Working → Error → Working (récupération d'erreur)
Working → Stopped (10 erreurs consécutives ou arrêt manuel)
Stopped → Idle (redémarrage)
```
