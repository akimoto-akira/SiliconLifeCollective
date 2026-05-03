# Feuille de route

> **Version : v0.1.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | **Français** | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md)

## Feuille de route en double version

### SiliconLife.Default (Version standard)
- **Positionnement** : Implémentation standard, principalement pour la vérification de faisabilité architecturale
- **Statut actuel** : Phases 1-10.6 terminées, le système fonctionne de manière stable
- **Description du rôle** : Implémentation de référence pour la vérification architecturale, garantit l'exactitude et la faisabilité de la conception architecturale principale

### SiliconLife.Fast (Version haute performance)
- **Positionnement** : Version principale de production
- **Statut actuel** : Portage de base de l'architecture terminé, moteur de stockage SpeedyPack et système de plugins implémentés
- **Description du rôle** : Basé sur l'architecture vérifiée dans la version Default, effectue une optimisation approfondie des performances et renforce les fonctionnalités de production, le meilleur choix pour le déploiement réel

**Plan de développement de la version Fast** :
- ✅ Phase 1 : Portage de la structure de projet de base et du système de configuration
- ✅ Phase 2 : Portage de l'interface Web et des contrôleurs
- ✅ Phase 3 : Optimisation du système de stockage (stockage en mémoire SpeedyPack + persistance asynchrone)
- ✅ Phase 3.5 : Outil de gestion SpeedyPack (application WPF SiliconLife.Speedy.Manager)
- ✅ Phase 3.6 : Système de plugins (interface IPlugin, bac à sable de sécurité, isolation AssemblyLoadContext)
- 🚧 Phase 4 : Barre d'état système et développement d'application Forms
- 📋 Phase 5 : Optimisation des performances (pool de connexions, pool d'objets, concurrence sans verrou)
- 📋 Phase 6 : Remplacement du serveur Web Kestrel
- 📋 Phase 7 : Sérialisation binaire MessagePack

---

## Principes directeurs

Chaque phase se termine par un système **fonctionnel et observable**. Aucune phase ne produit « beaucoup d'infrastructure sans rien de montrable ».

---

## ~~Phase 1 : Peut chatter~~ ✅ Terminé

**Objectif** : Entrée console → Appel IA → Sortie console. Unité minimale vérifiable.

| # | Module | Description |
|---|--------|-------------|
| 1.1 | Structure de solution et de projet | Créer `SiliconLifeCollective.sln`, avec `src/SiliconLife.Core/` (bibliothèque principale) et `src/SiliconLife.Default/` (implémentation standard + point d'entrée) |
| 1.2 | Configuration (minimale) | Singleton + désérialisation JSON. Lit `config.json`. Génère automatiquement les valeurs par défaut si absent |
| 1.3 | Localisation (minimale) | Classe abstraite `LocalizationBase`, implémentation `ZhCN`. Ajouter `Language` à la configuration |
| 1.4 | OllamaClient (minimal) | Interface `IAIClient`, appel HTTP à l'Ollama local `/api/chat`. Pas encore de streaming, pas d'appels d'outils |
| 1.5 | E/S console | `while(true) + Console.ReadLine()`, lire l'entrée → appeler l'IA → imprimer la réponse |
| 1.6 | En-tête de copyright | Ajouter l'en-tête Apache 2.0 à tous les fichiers source C# |

**Livrable** : Programme de chat console pour converser avec le modèle Ollama local.

**Vérification** : Exécuter le programme, taper « bonjour », voir la réponse de l'IA.

---

## ~~Phase 2 : A un squelette~~ ✅ Terminé

**Objectif** : Remplacer la « boucle nue » par une structure de framework. Comportement inchangé.

| # | Module | Description |
|---|--------|-------------|
| 2.1 | Stockage (minimal) | Interface `IStorage` (Read/Write/Exists/Delete, paires clé-valeur). Implémentation `FileSystemStorage`. Classe d'instance (non statique). Accès direct au système de fichiers —— **L'IA ne peut pas contrôler IStorage** |
| 2.2 | Boucle principale + Objet horloge | Boucle infinie, intervalle d'horloge précis (`Stopwatch` + `Thread.Sleep`). Ordonnancement par priorité |
| 2.3 | Standardisation IAIClient | Interface `IAIClientFactory`. OllamaClient refactorisé pour l'interface standard |
| 2.4 | Migration console | Migrer `while(true)` vers un objet horloge piloté par la boucle principale. Comportement identique à la Phase 1 |

**Livrable** : La boucle principale exécute l'horloge, le chat console fonctionne toujours.

**Vérification** : Enregistrer un objet horloge de test, compte les tics chaque seconde ; le chat console fonctionne toujours.

---

## ~~Phase 3 : A une âme~~ ✅ Terminé

**Objectif** : Le premier Silicon Being vit dans le framework.

| # | Module | Description |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Classe de base abstraite avec Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. `Tick()` et `ExecuteOneRound()` abstraits |
| 3.2 | Chargement du fichier âme | `SoulFileManager` : Lit `soul.md` depuis le répertoire de données du Being |
| 3.3 | ContextManager (minimal) | Enchaîne fichier âme + messages récents → appelle l'IA → obtient la réponse. Pas encore d'appels d'outils, pas de persistance |
| 3.4 | ISiliconBeingFactory | Interface de fabrique pour créer des instances de Being |
| 3.5 | SiliconBeingManager (minimal) | Hérite de l'objet horloge (Priorité=0). Itère tous les Beings, appelle leur Tick séquentiellement |
| 3.6 | DefaultSiliconBeing | Implémentation du comportement standard. Vérifie les messages non lus → crée le ContextManager → ExecuteOneRound → sortie |
| 3.7 | Structure du répertoire Being | `DataDirectory/SiliconManager/{GUID}/`, contient `soul.md` et `state.json` |

**Livrable** : Silicon Being piloté par la boucle principale, reçoit l'entrée console, charge le fichier âme, appelle l'IA.

**Vérification** : Entrée console → Tic de l'horloge de la boucle principale déclenché → le Being traite (avec comportement guidé par le fichier âme) → réponse de l'IA. Le style de réponse devrait différer de la Phase 1.

---

## ~~Phase 4 : A une mémoire~~ ✅ Terminé

**Objectif** : Les conversations persistent après le redémarrage.

| # | Module | Description |
|---|--------|-------------|
| 4.1 | ChatSystem | Concept de canal (deux GUIDs = un canal). Modèle de message avec persistance. Pas encore de chat de groupe |
| 4.2 | IIMProvider + IMManager | Interface `IIMProvider`. `ConsoleProvider` comme canal IM formel. `IMManager` route les messages |
| 4.3 | Extension du ContextManager | Tire l'historique du système de chat. Persiste les réponses de l'IA. Prend en charge la continuation d'appels d'outils multi-niveaux |
| 4.4 | Modèle IMessage | Modèle de message unifié partagé entre le système de chat et le gestionnaire IM |

**Livrable** : Système de chat avec stockage persistant.

**Vérification** : Chatter plusieurs tours → Quitter → Redémarrer → Demander « De quoi avons-nous parlé ? » → Le Being peut répondre.

---

## ~~Phase 5 : Peut agir (Système d'outils)~~ ✅ Terminé

**Objectif** : Les Silicon Beings peuvent exécuter des actions, pas seulement chatter.

| # | Module | Description |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interface `ITool` avec Name, Description, Execute. `ToolResult` avec Success, Message, Data |
| 5.2 | ToolManager | Instance par Being. Découverte d'outils basée sur la réflexion. Prise en charge de l'attribut `[SiliconManagerOnly]` |
| 5.3 | IAIClient : Prise en charge des appels d'outils | Analyse les tool_calls de l'IA. Boucle : exécuter les outils → renvoyer les résultats → IA continue → jusqu'à texte pur |
| 5.4 | Classe de base Executor | Classe de base abstraite avec son propre thread répartiteur, file d'attente de requêtes, contrôle du timeout |
| 5.5 | NetworkExecutor | Requêtes HTTP via l'exécuteur. Timeout, file d'attente |
| 5.6 | CommandLineExecutor | Exécution shell via l'exécuteur. Détection de séparateurs multiplateforme |
| 5.7 | DiskExecutor | Opérations sur fichiers via l'exécuteur. Pas encore de vérification de permissions (Phase 6) |
| 5.8–5.12 | Outils intégrés | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Livrable** : Les Silicon Beings peuvent appeler des outils pour exécuter des actions.

**Vérification** : Demander « Quel jour sommes-nous » → CalendarTool répond ; Demander « Vérifier les processus » → SystemTool exécute ; Demander au Being d'envoyer un message à un autre Being → ChatTool fonctionne.

---

## ~~Phase 6 : Suit les règles (Système de permissions)~~ ✅ Terminé

**Objectif** : Les Silicon Beings ne peuvent pas accéder aux ressources sensibles sans autorisation.

| # | Module | Description |
|---|--------|-------------|
| 6.1 | PermissionManager | Instance privée par Being. Basé sur des rappels, résultat ternaire (Allowed/Deny/AskUser). Priorité de requête : HighDeny → HighAllow → Rappel. Drapeau IsCurator |
| 6.2 | Enum PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Liste blanche/noire réseau, classification CLI, règles de sécurité de chemin de fichier |
| 6.4 | GlobalACL | Table de règles par correspondance de préfixe, persistée dans le stockage |
| 6.5 | UserFrequencyCache | Listes HighAllow/HighDeny. Choix utilisateur (pas détection auto). Correspondance de préfixe, mémoire uniquement, expiration configurable |
| 6.6 | Mécanisme UserAsk (Console) | En cas de retour AskUser, invite console o/n |
| 6.7 | Intégration des permissions de l'exécuteur | Tous les exécuteurs vérifient la permission avant l'exécution |
| 6.8 | Note d'isolation IStorage | IStorage est la persistance système interne —— accès fichier direct, **non** routé via l'exécuteur, **non** contrôlable par l'IA. Les exécuteurs gèrent uniquement les E/S initiés par les outils IA |
| 6.9 | Journaux d'audit | Enregistre toutes les décisions de permissions avec horodatage, demandeur, ressource, résultat |

**Livrable** : Invite de permission lorsque le Being tente une opération sensible.

**Vérification** : Demander au Being de supprimer un fichier → La console affiche une invite de permission → Taper `n` → Opération refusée. Demander au Being de visiter un site en liste blanche → Immédiatement autorisé.

---

## ~~Phase 7 : Peut évoluer (Compilation dynamique)~~ ✅ Terminé

**Objectif** : Les Silicon Beings peuvent réécrire leur propre code.

| # | Module | Description |
|---|--------|-------------|
| 7.1 | CodeEncryption | Chiffrement/déchiffrement AES-256. Clé PBKDF2 dérivée du GUID |
| 7.2 | DynamicCompilationExecutor | Bac à sable de compilation en mémoire basé sur Roslyn. Contrôle des références d'assembly à la compilation (défense principale : exclure System.IO, Reflection, etc.) |
| 7.3 | Analyse de sécurité | Analyse statique à l'exécution de motifs de code dangereux (défense secondaire). Bloque le chargement si l'analyse échoue |
| 7.4 | Extension du cycle de vie du Being | Chargement : Déchiffrer → Analyser → Compiler → Instancier. Exécution : Compiler en mémoire → Remplacement atomique → Persister chiffré |
| 7.5 | SiliconCurator | Classe de base abstraite du Curator. IsCurator=true. Permission la plus élevée |
| 7.6 | DefaultCurator | Implémentation standard du Curator avec fichier âme intégré et outils admin |
| 7.7 | CuratorTool | Outils `[SiliconManagerOnly]` : list_beings, create_being, get_code, reset |
| 7.8 | Surcharge du rappel de permission | Les Beings peuvent compiler des rappels de permission personnalisés |
| 7.9 | Extension du SiliconBeingManager | Méthode Replace (échange d'instance à l'exécution). MigrateState (transfert d'état entre ancienne et nouvelle instance) |

**Livrable** : Les Silicon Beings peuvent compiler et se remplacer par du nouveau code généré par l'IA.

**Vérification** : Demander au Being « Ajoute-toi une nouvelle fonctionnalité » → Observer la compilation → Redémarrage → La nouvelle fonctionnalité fonctionne.

---

## ~~Phase 8 : Mémoire et planification~~ ✅ Terminé

**Objectif** : Stockage à long terme, gestion des tâches, déclencheurs de minuterie.

| # | Module | Description |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Stockage segmenté court terme/long terme. Expiration temporelle. Compression (fusion de souvenirs similaires). Recherche multidimensionnelle |
| 8.2 | TaskSystem | Tâches ponctuelles + dépendances DAG. Ordonnancement par priorité. Suivi de statut |
| 8.3 | TimerSystem | Alarme ponctuelle + minuteries périodiques. Précision à la milliseconde. Persisté dans le stockage |
| 8.4 | IncompleteDate | Structure de plage de dates floues (ex. « avril 2026 », « printemps 2026 ») |
| 8.5–8.7 | Outils de mémoire/tâches/minuteries | Outils pour les Beings afin de consulter les souvenirs, gérer les tâches, définir des minuteries |

**Livrable** : Les Beings peuvent retenir les points clés, créer/suivre des tâches, définir des alarmes.

**Vérification** : Créer une tâche → Vérifier la liste des tâches → Définir une alarme d'une minute → Recevoir une notification au déclenchement.

---

## ~~Phase 9 : Framework terminé~~ ✅ Terminé

**Objectif** : Point d'entrée unifié, collaboration multi-Beings.

| # | Module | Description |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Hôte unifié avec pattern Builder. Arrêt gracieux (Ctrl+C / SIGTERM) |
| 9.2 | Refactorisation de Program.Main | Migration vers le pattern CoreHostBuilder |
| 9.3 | Extension du SiliconBeingManager | Réponse Curator-first. Isolation des exceptions. Persistance régulière |
| 9.4 | Chargement multi-Beings | Charge plusieurs Beings depuis le répertoire de données. Communication Being-à-Being via ChatTool |
| 9.5 | Surveillance des performances | Suivi du temps d'exécution par objet horloge |
| 9.6 | ServiceLocator | Localisateur de services global avec méthodes Register/Get |

**Livrable** : Plusieurs Beings fonctionnent simultanément, collaborent, gérés par CoreHost.

**Vérification** : Créer deux Beings → A envoie un message à B → B reçoit et répond → Ordonnancement du framework sans erreur. Le Curator répond en premier aux messages utilisateur.

---

## ~~Phase 10 : Vers le Web~~ ✅ Terminé

**Objectif** : Migrer de la console vers l'interface navigateur.

| # | Module | Description |
|---|--------|-------------|
| 10.1 | Router | Routeur de requêtes HTTP. Routage par paramètres séquentiels et service de fichiers statiques |
| 10.2 | Classe de base Controller | Contexte requête/réponse. Prise en charge des réponses HTML et JSON |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | Builders côté serveur C#. Zéro dépendance framework frontend |
| 10.6 | SSE (Server-Sent Events) | Mises à jour en temps réel push pour chat, statut des Beings et événements système. Plus simple que WebSocket, avec reconnexion automatique du client |
| 10.7 | WebUIProvider | Canal IM en temps réel basé sur SSE. Remplace la console comme interface principale |
| 10.8 | Sécurité Web | Liste noire/blanche IP. Attribut `[WebCode]`. Mises à jour dynamiques |
| 10.9–10.17 | Contrôleurs Web | Chat, Dashboard, Beings, Tâches, Permissions, Requêtes de permissions, Exécuteurs, Journaux, Configuration, Mémoire, Minuteries, Initialisation, À propos, Navigateur de code, Connaissances, Projets, Audit |

**Livrable** : Interface Web complète, accessible depuis le navigateur.

**Vérification** : Ouvrir le navigateur → Chatter avec un Being → Voir le tableau de bord → Gérer les permissions → Tout fonctionne.

---

## ~~Phase 10.5 : Extensions incrémentales~~ ✅ Terminé

**Objectif** : Étendre le système existant avec de nouvelles fonctionnalités découvertes pendant le développement.

| # | Module | Description |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Nouveau type de session pour les annonces système. ID de canal fixe, abonnement dynamique, filtrage des messages en attente |
| 10.5.2 | Extension ChatMessage | Champs ToolCallId, ToolCallsJson, Thinking pour le contexte IA ; PromptTokens, CompletionTokens, TotalTokens pour le suivi des tokens ; type de message SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Suivi de la consommation de tokens pour tous les Beings par requête. Statistiques agrégées, requêtes de séries temporelles, stockage persistant |
| 10.5.4 | TokenAuditTool | Outil `[SiliconManagerOnly]` pour le Curator afin de consulter et résumer l'utilisation des tokens |
| 10.5.5 | ConfigTool | Outil `[SiliconManagerOnly]` pour le Curator afin de lire et modifier la configuration système |
| 10.5.6 | AuditController | Tableau de bord Web pour l'audit d'utilisation des tokens avec graphiques de tendance et export de données |
| 10.5.7 | Extension du système de calendrier | 32 implémentations de calendriers, couvrant les systèmes calendaires mondiaux (Bouddhiste, Lunaire chinois, Islamique, Hébraïque, Japonais, Persan, Maya, etc.) |
| 10.5.8 | Extension DiskTool | Nouvelles opérations : count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Extension SystemTool | Nouvelles opérations : find_process (avec prise en charge des caractères génériques), resource_usage |
| 10.5.10 | Extension CalendarTool | Nouvelles opérations : diff, list_calendars, get_components, get_now_components, convert (conversion inter-calendriers) |
| 10.5.11 | DashScopeClient | Client IA Alibaba Cloud DashScope, compatible API OpenAI. Prend en charge le streaming, les appels d'outils, le contenu de raisonnement |
| 10.5.12 | DashScopeClientFactory | Fabrique pour créer des clients DashScope. Découverte dynamique de modèles via API. Prise en charge multi-région (Pékin, Virginie, Singapour, Hong Kong, Francfort) |
| 10.5.13 | Système de configuration du client IA | Configuration du client IA par Being. Options de clés de configuration dynamiques (modèle, région). Noms d'affichage localisés |
| 10.5.14 | Extension de localisation | Localisation en chinois simplifié, chinois traditionnel, anglais et japonais pour les options de configuration DashScope, noms de modèles et noms de régions |

**Livrable** : Outils étendus, observabilité, couverture calendrier et prise en charge multi-backend IA.

**Vérification** : Le Curator interroge l'utilisation des tokens via TokenAuditTool → Le tableau de bord d'audit affiche les tendances → CalendarTool convertit la date entre 32 systèmes de calendrier → Basculer le backend IA vers DashScope → Chatter avec le modèle Qwen via l'API cloud.

---

## ~~Phase 10.6 : Raffinement et optimisation~~ ✅ Terminé

**Objectif** : Raffiner les fonctionnalités système, ajouter de nouvelles fonctionnalités, optimiser l'expérience utilisateur.

| # | Module | Description |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Outil d'automatisation de navigateur multiplateforme basé sur Playwright, avec mode headless, isolation individuelle, support JS/CSS complet |
| 10.6.2 | HelpTool | Outil du système de documentation d'aide, prend en charge la consultation et l'affichage de documentation multilingue |
| 10.6.3 | ProjectWorkNoteTool | Outil de notes de travail de projet, prend en charge les enregistrements de travail liés au projet et la gestion |
| 10.6.4 | ProjectTaskTool | Outil de gestion de tâches de projet, prend en charge l'assignation de tâches, le suivi de progression |
| 10.6.5 | KnowledgeTool | Outil de réseau de connaissances, prend en charge le CRUD de connaissances en triplets et la recherche de chemins |
| 10.6.6 | ChatHistoryController | Contrôleur d'affichage de l'historique de chat, prend en charge la liste des sessions et les détails des messages |
| 10.6.7 | CodeHoverController | Contrôleur d'info-bulles de survol de code, prend en charge la coloration syntaxique et la complétion de code |
| 10.6.8 | WorkNoteController | Contrôleur de gestion des notes de travail, prend en charge la recherche et la génération de répertoire |
| 10.6.9 | TimerExecutionHistory | Fonctionnalité d'historique d'exécution des minuteries, enregistre l'historique des déclenchements et permet la consultation |
| 10.6.10 | Extension de localisation | Ajout de la prise en charge de localisation tchèque (cs-CZ), total de 24 variantes linguistiques |
| 10.6.11 | Optimisation de l'interface Web | Prise en charge du téléchargement de fichiers, indicateur de chargement, optimisation du rendu des appels d'outils, correction du modal des notes de travail |
| 10.6.12 | Extension de la gestion mémoire | Filtrage avancé, statistiques, vue détaillée, optimisation de l'algorithme de compression |
| 10.6.13 | Refactorisation du système de journaux | Séparation des journaux système/Silicon Being, API de lecture des journaux, filtre par Being |
| 10.6.14 | Extension du système de permissions | Pré-validation des rappels de permission, validation des références d'assembly, liste blanche du service météo wttr.in |

**Livrable** : Automatisation complète du navigateur WebView, système de documentation d'aide, espace de projet, réseau de connaissances, affichage de l'historique de chat et autres fonctionnalités avancées.

**Vérification** : Le Silicon Being peut utiliser le navigateur via WebViewBrowserTool → Obtenir la documentation d'aide via HelpTool → Gérer les notes de travail et tâches de projet → Consulter le réseau de connaissances → Voir l'historique de chat.

---

## Phase 11 : Intégration IM externe

**Objectif** : Connexion aux plateformes de messagerie externes pour une accessibilité utilisateur plus large.

| # | Module | Description |
|---|--------|-------------|
| 11.1 | FeishuProvider | Intégration du bot Feishu (Lark) avec prise en charge des cartes |
| 11.2 | WhatsAppProvider | Intégration de l'API WhatsApp Business |
| 11.3 | TelegramProvider | Intégration de l'API Telegram Bot avec prise en charge du clavier inline |
| 11.4 | Extension IMManager | Routage multi-fournisseur, format de message unifié, traitement des requêtes de permissions multiplateforme |

---

## Phase 12 : Écosystème de compétences

**Objectif** : Créer un marché de plugins et un système de distribution de compétences.

| # | Module | Description |
|---|--------|-------------|
| 12.1 | SkillPackage | Format de package de compétences, métadonnées, vérification de signature |
| 12.2 | SkillMarketplace | Interface Web de navigation et d'installation de compétences |
| 12.3 | SkillRuntime | Environnement d'exécution de compétences isolé, gestion du cycle de vie |
| 12.4 | CommunityContrib | Système de soumission et de revue de la communauté |
