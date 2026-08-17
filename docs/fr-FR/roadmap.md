# Feuille de route

> **Version : v0.2.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md) | [Русский](../ru-RU/roadmap.md)

## Feuille de route en double version

### SiliconLife.Default (version par défaut)
- **Positionnement** : Implémentation par défaut, principalement utilisée pour valider la faisabilité de l'architecture
- **État actuel** : Phases 1 à 10.6 terminées, le système fonctionne de manière stable
- **Description du rôle** : Sert d'implémentation de référence pour la validation de l'architecture, garantissant la correction et la faisabilité de la conception architecturale principale

### SiliconLife.Fast (version haute performance)
- **Positionnement** : Version de production recommandée
- **État actuel** : Migration de l'architecture de base terminée, moteur de stockage SpeedyPack et système de plugins implémentés
- **Description du rôle** : Sur la base de l'architecture validée par la version Default, optimisation approfondie des performances et amélioration des fonctionnalités de niveau production, c'est le choix privilégié pour le déploiement réel

**Plan de développement de la version Fast** :
- ✅ Phase 1 : Structure de projet de base et migration du système de configuration
- ✅ Phase 2 : Migration de l'UI Web et des contrôleurs
- ✅ Phase 3 : Optimisation du système de stockage (Stockage en mémoire SpeedyPack + persistance asynchrone)
- ✅ Phase 3.5 : Outil de gestion SpeedyPack (application Avalonia UI SiliconLife.Speedy.Manager)
- ✅ Phase 3.6 : Système de plugins (interface IPlugin, Sandbox de sécurité, isolation AssemblyLoadContext)
- ✅ Phase 4 : Application fenêtrée Avalonia (application de bureau multiplateforme, barre d'état système Windows/macOS, fenêtre d'état Linux)

---

## Principes directeurs

Chaque phase se termine par un système **exécutable et observable**. Aucune phase ne produit « beaucoup d'infrastructure sans rien à montrer ».

---

## ~~Phase 1 : Peut discuter~~ ✅ Terminée

**Objectif** : Entrée console → Appel IA → Sortie console. Unité minimale vérifiable.

| # | Module | Description |
|---|--------|-------------|
| 1.1 | Solution et structure de projet | Créer `SiliconLifeCollective.sln`, contenant `src/SiliconLife.Core/` (bibliothèque principale) et `src/SiliconLife.Default/` (implémentation par défaut + point d'entrée) |
| 1.2 | Configuration (minimale) | Singleton + désérialisation JSON. Lire `config.json`. Auto-générer les valeurs par défaut si absent |
| 1.3 | Localisation (minimale) | Classe abstraite `LocalizationBase`, implémentation `ZhCN`. Ajouter `Language` à la configuration |
| 1.4 | OllamaClient (minimal) | Interface `IAIClient`, appel HTTP vers Ollama local `/api/chat`. Pas de streaming, pas d'appel d'outil |
| 1.5 | E/S console | `while(true) + Console.ReadLine()`, lire l'entrée → appeler l'IA → afficher la réponse |
| 1.6 | En-tête de copyright | Ajouter l'en-tête Apache 2.0 à tous les fichiers source C# |

**Livrable** : Programme de chat console dialoguant avec le modèle Ollama local.

**Validation** : Exécuter le programme, saisir « hello », voir la réponse de l'IA.

---

## ~~Phase 2 : A un squelette~~ ✅ Terminée

**Objectif** : Remplacer la « boucle nue » par une structure de cadre. Comportement inchangé.

| # | Module | Description |
|---|--------|-------------|
| 2.1 | Stockage (minimal) | Interface `IStorage` (Read/Write/Exists/Delete, clé-valeur). Implémentation `FileSystemStorage`. Classe d'instance (non statique). Accès direct au système de fichiers — **l'IA ne peut pas contrôler IStorage** |
| 2.2 | Boucle Principale + Objet Tick | Boucle infinie, intervalle de temps précis (`Stopwatch` + `Thread.Sleep`). Ordonnancement par priorité |
| 2.3 | Standardisation IAIClient | Interface `IAIClientFactory`. OllamaClient refactoré pour implémenter l'interface standard |
| 2.4 | Migration console | Migrer `while(true)` vers un objet Tick piloté par la Boucle Principale. Comportement identique à la Phase 1 |

**Livrable** : La Boucle Principale exécute les Ticks, le chat console fonctionne toujours.

**Validation** : Enregistrer un objet Tick de test, afficher le comptage des Ticks chaque seconde ; le chat console fonctionne toujours.

---

## ~~Phase 3 : A une âme~~ ✅ Terminée

**Objectif** : Le premier Être de Silicium vit dans le cadre.

| # | Module | Description |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Classe abstraite de base, contenant Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. Méthodes abstraites `Tick()` et `ExecuteOneRound()` |
| 3.2 | Chargement du Fichier d'Âme | `SoulFileManager` : lire `soul.md` depuis le répertoire de données de l'être |
| 3.3 | Gestionnaire de Contexte (minimal) | Connecter le Fichier d'Âme + messages récents → appeler l'IA → obtenir la réponse. Pas d'appel d'outil, pas de persistance |
| 3.4 | ISiliconBeingFactory | Interface de fabrique pour créer des instances d'êtres |
| 3.5 | SiliconBeingManager (minimal) | Hérite de l'Objet Tick (priorité=0). Itère tous les êtres, appelle leur Tick séquentiellement |
| 3.6 | DefaultSiliconBeing | Implémentation de comportement standard. Vérifie les messages non lus → crée un Gestionnaire de Contexte → ExecuteOneRound → sortie |
| 3.7 | Structure du répertoire de l'être | `DataDirectory/SiliconManager/{GUID}/`, contenant `soul.md` et `state.json` |

**Livrable** : Être de Silicium piloté par la Boucle Principale, recevant l'entrée console, chargeant le Fichier d'Âme, appelant l'IA.

**Validation** : Entrée console → Tick de la Boucle Principale déclenché → l'être traite (comportement guidé par le Fichier d'Âme) → réponse IA. Le style de réponse devrait différer de la Phase 1.

---

## ~~Phase 4 : A une mémoire~~ ✅ Terminée

**Objectif** : Les conversations persistent après le redémarrage.

| # | Module | Description |
|---|--------|-------------|
| 4.1 | Système de Chat | Concept de canal (deux GUID = un canal). Modèle de message avec persistance. Pas de chat de groupe |
| 4.2 | IIMProvider + IMManager | Interface `IIMProvider`. `ConsoleProvider` comme canal de messagerie officiel. `IMManager` route les messages |
| 4.3 | Amélioration du Gestionnaire de Contexte | Extraire l'historique du Système de Chat. Persister les réponses IA. Prendre en charge la continuation multi-tours des appels d'outil |
| 4.4 | Modèle IMessage | Modèle de message unifié partagé entre le Système de Chat et le Gestionnaire IM |

**Livrable** : Système de chat avec mémoire persistante.

**Validation** : Discuter quelques tours → quitter → redémarrer → demander « De quoi avons-nous discuté ? » → l'être peut répondre.

---

## ~~Phase 5 : Peut agir (Système d'outils)~~ ✅ Terminée

**Objectif** : Les Êtres de Silicium peuvent exécuter des actions, pas seulement discuter.

| # | Module | Description |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interface `ITool`, contenant Name, Description, Execute. `ToolResult` contient Success, Message, Data |
| 5.2 | ToolManager | Instance par être. Découverte d'outils basée sur la réflexion. Prise en charge de l'attribut `[SiliconManagerOnly]` |
| 5.3 | IAIClient : Prise en charge des appels d'outil | Analyser les tool_calls de l'IA. Boucle : exécuter l'outil → renvoyer le résultat → l'IA continue → jusqu'à texte pur |
| 5.4 | Classe de base Exécuteur | Classe abstraite de base, avec thread de répartition indépendant, file d'attente de requêtes, contrôle du délai |
| 5.5 | NetworkExecutor | Requêtes HTTP via l'exécuteur. Délai, mise en file d'attente |
| 5.6 | CommandLineExecutor | Exécution shell via l'exécuteur. Détection de séparateur multiplateforme |
| 5.7 | DiskExecutor | Opérations de fichiers via l'exécuteur. Pas de vérification d'autorisation (Phase 6) |
| 5.8–5.12 | Outils intégrés | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Livrable** : Les Êtres de Silicium peuvent appeler des outils pour exécuter des actions.

**Validation** : Demander « Quel jour sommes-nous » → CalendarTool répond ; demander « Vérifier les processus » → SystemTool exécute ; dire à l'être d'envoyer un message à un autre être → ChatTool fonctionne.

---

## ~~Phase 6 : Respecte les règles (Système d'autorisations)~~ ✅ Terminée

**Objectif** : Les Êtres de Silicium ne peuvent pas accéder aux ressources sensibles sans autorisation.

| # | Module | Description |
|---|--------|-------------|
| 6.1 | PermissionManager | Instance privée par être. Basé sur le rappel, résultat ternaire (Allowed/Deny/AskUser). Priorité de requête : HighDeny → HighAllow → Rappel. Indicateur IsCurator |
| 6.2 | Énumération PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Liste blanche/noire réseau, classification CLI, règles de sécurité de chemin de fichier |
| 6.4 | GlobalACL | Table de règles de correspondance de préfixe, persistée dans le stockage |
| 6.5 | UserFrequencyCache | Listes HighAllow/HighDeny. Choix de l'utilisateur (pas détection automatique). Correspondance de préfixe, mémoire uniquement, expiration configurable |
| 6.6 | Mécanisme UserAsk (console) | Invite console y/n lorsque AskUser est renvoyé |
| 6.7 | Intégration autorisation-exécuteur | Tous les exécuteurs vérifient les autorisations avant l'exécution |
| 6.8 | Note d'isolation IStorage | IStorage est une persistance interne au système — accès direct aux fichiers, **non** routé via les exécuteurs, **non** contrôlable par l'IA. Les exécuteurs ne gèrent que les E/S initiées par les outils IA |
| 6.9 | Journal d'audit | Enregistrer toutes les décisions d'autorisation, avec horodatage, demandeur, ressource, résultat |

**Livrable** : Invite d'autorisation lorsque l'être tente une opération sensible.

**Validation** : Dire à l'être de supprimer un fichier → la console affiche une invite d'autorisation → saisir `n` → l'opération est refusée. Dire à l'être d'accéder à un site en liste blanche → immédiatement autorisé.

---

## ~~Phase 7 : Peut évoluer (Compilation dynamique)~~ ✅ Terminée

**Objectif** : Les Êtres de Silicium peuvent réécrire leur propre code.

| # | Module | Description |
|---|--------|-------------|
| 7.1 | CodeEncryption | Chiffrement/déchiffrement AES-256. Dérivation de clé PBKDF2 depuis le GUID |
| 7.2 | DynamicCompilationExecutor | Bac à sable de compilation en mémoire basé sur Roslyn. Contrôle des références d'assembly à la compilation (défense principale : exclure System.IO, Reflection, etc.) |
| 7.3 | Analyse de sécurité | Analyse statique à l'exécution des motifs de code dangereux (défense secondaire). Bloquer le chargement si l'analyse échoue |
| 7.4 | Amélioration du cycle de vie de l'être | Chargement : déchiffrer → analyser → compiler → instancier. Exécution : compiler en mémoire → Remplacement Atomique → persister le chiffré |
| 7.5 | SiliconCurator | Classe abstraite de base du curateur. IsCurator=true. Autorité maximale |
| 7.6 | DefaultCurator | Implémentation par défaut du curateur, avec Fichier d'Âme intégré et outils de gestion |
| 7.7 | CuratorTool | Outil `[SiliconManagerOnly]` : list_beings, create_being, get_code, reset |
| 7.8 | Remplacement du rappel d'autorisation | L'être peut compiler un rappel d'autorisation personnalisé |
| 7.9 | Amélioration de SiliconBeingManager | Méthode Replace (échange d'instance à l'exécution). MigrateState (transfert d'état entre ancienne et nouvelle instance) |

**Livrable** : Les Êtres de Silicium peuvent générer du nouveau code via l'IA, le compiler et se remplacer.

**Validation** : Dire à l'être « ajoute-toi une nouvelle fonctionnalité » → observer la compilation → redémarrer → la nouvelle fonctionnalité fonctionne.

---

## ~~Phase 8 : Mémoire et planification~~ ✅ Terminée

**Objectif** : Mémoire à long terme, gestion des tâches, déclenchement minuté.

| # | Module | Description |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Stockage segmenté court terme/long terme. Décroissance temporelle. Compression (fusion de souvenirs similaires). Recherche multidimensionnelle |
| 8.2 | TaskSystem | Tâches ponctuelles + dépendances DAG. Ordonnancement par priorité. Suivi d'état |
| 8.3 | TimerSystem | Alarmes ponctuelles + minuteurs périodiques. Précision en millisecondes. Persistance dans le stockage |
| 8.4 | IncompleteDate | Structure de plage de dates floues (par ex. « avril 2026 », « printemps 2026 ») |
| 8.5–8.7 | Outils Mémoire/Tâche/Minuteur | Outils pour que l'être interroge les souvenirs, gère les tâches, définisse des alarmes |

**Livrable** : L'être peut mémoriser les points clés, créer/suivre des tâches, définir des alarmes.

**Validation** : Créer une tâche → vérifier la liste des tâches → définir une alarme de 1 minute → recevoir une notification à l'échéance.

---

## ~~Phase 9 : Cadre terminé~~ ✅ Terminée

**Objectif** : Point d'entrée unifié, collaboration multi-êtres.

| # | Module | Description |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Hôte unifié avec le modèle constructeur. Arrêt gracieux (Ctrl+C / SIGTERM) |
| 9.2 | Refactorisation de Program.Main | Migration vers le modèle CoreHostBuilder |
| 9.3 | Amélioration de SiliconBeingManager | Réponse prioritaire du curateur. Isolation des exceptions. Persistance périodique |
| 9.4 | Chargement multi-êtres | Charger plusieurs êtres depuis le répertoire de données. Communication inter-êtres via ChatTool |
| 9.5 | Surveillance des performances | Suivi du temps d'exécution de chaque Objet Tick |
| 9.6 | ServiceLocator | Localisateur de services global, avec méthodes Register/Get |

**Livrable** : Plusieurs êtres fonctionnant simultanément, collaborant, gérés par CoreHost.

**Validation** : Créer deux êtres → A envoie un message à B → B reçoit et répond → le cadre ordonnance sans erreur. Le curateur répond en priorité lorsqu'un message utilisateur arrive.

---

## ~~Phase 10 : Vers le Web~~ ✅ Terminée

**Objectif** : Migrer de la console vers l'interface navigateur.

| # | Module | Description |
|---|--------|-------------|
| 10.1 | Router | Routeur de requêtes HTTP. Routage de paramètres séquentiels et service de fichiers statiques |
| 10.2 | Classe de base Controller | Contexte requête/réponse. Prise en charge des réponses HTML et JSON |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | Constructeurs côté serveur C#. Zéro dépendance framework frontend |
| 10.6 | SSE (Server-Sent Events) | Mises à jour en temps réel push pour le chat, l'état des êtres et les événements système. Plus simple que WebSocket, avec reconnexion automatique côté client |
| 10.7 | WebUIProvider | Canal de messagerie instantanée basé sur SSE. Remplace la console comme interface principale |
| 10.8 | Sécurité Web | Liste noire/blanche IP. Attribut `[WebCode]`. Mise à jour dynamique |
| 10.9–10.17 | Contrôleurs Web | Chat, tableau de bord, êtres, tâches, autorisations, requêtes d'autorisation, exécuteurs, journaux, configuration, mémoire, minuteurs, initialisation, à propos, navigateur de code, connaissances, projets, audit |

**Livrable** : UI Web complète accessible depuis le navigateur.

**Validation** : Ouvrir le navigateur → discuter avec un être → voir le tableau de bord → gérer les autorisations → toutes les fonctionnalités fonctionnent.

---

## ~~Phase 10.5 : Améliorations incrémentales~~ ✅ Terminée

**Objectif** : Améliorer le système existant avec de nouvelles fonctionnalités découvertes pendant le développement.

| # | Module | Description |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Nouveau type de session pour les annonces à l'échelle du système. ID de canal fixe, abonnement dynamique, filtrage des messages en attente |
| 10.5.2 | Amélioration de ChatMessage | Champs ToolCallId, ToolCallsJson, Thinking pour le contexte IA ; PromptTokens, CompletionTokens, TotalTokens pour le suivi des tokens ; type de message SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Suivi de la consommation de tokens par requête pour tous les êtres. Statistiques agrégées, requêtes de séries temporelles, stockage persisté |
| 10.5.4 | TokenAuditTool | Outil `[SiliconManagerOnly]`, permettant au curateur de requêter et résumer l'utilisation des tokens |
| 10.5.5 | ConfigTool | Outil `[SiliconManagerOnly]`, permettant au curateur de lire et modifier la configuration système |
| 10.5.6 | AuditController | Tableau de bord Web pour l'audit d'utilisation des tokens, avec graphiques de tendance et export de données |
| 10.5.7 | Extension du système de calendrier | 32 implémentations calendaires, couvrant les systèmes calendaires du monde (bouddhiste, lunaire, islamique, hébraïque, japonais, persan, maya, etc.) |
| 10.5.8 | Amélioration de DiskTool | Nouvelles opérations : count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Amélioration de SystemTool | Nouvelles opérations : find_process (prise en charge des caractères génériques), resource_usage |
| 10.5.10 | Amélioration de CalendarTool | Nouvelles opérations : diff, list_calendars, get_components, get_now_components, convert (conversion inter-calendrier) |
| 10.5.11 | DashScopeClient | Client IA DashScope Alibaba Cloud, compatible API OpenAI. Prise en charge du streaming, des appels d'outil, du contenu de raisonnement |
| 10.5.12 | DashScopeClientFactory | Fabrique pour créer des clients DashScope. Découverte dynamique de modèles via API. Prise en charge multi-région (Pékin, Virginie, Singapour, Hong Kong, Francfort) |
| 10.5.13 | Système de configuration du client IA | Configuration du client IA par être. Options de clé de configuration dynamique (modèle, région). Nom d'affichage localisé |
| 10.5.14 | Extension de localisation | Localisation en chinois simplifié, chinois traditionnel, anglais et japonais pour les options de configuration DashScope, les noms de modèles et les noms de régions |

**Livrable** : Outils améliorés, observabilité, couverture calendaire et prise en charge multi-backends IA.

**Validation** : Le curateur interroge l'utilisation des tokens via TokenAuditTool → le tableau de bord d'audit affiche les tendances → CalendarTool convertit les dates entre 32 systèmes calendaires → basculer le backend IA vers DashScope → discuter avec le modèle Qwen via l'API cloud.

---

## ~~Phase 10.6 : Affinage et optimisation~~ ✅ Terminée

**Objectif** : Affiner les fonctionnalités du système, ajouter de nouvelles caractéristiques, optimiser l'expérience utilisateur.

| # | Module | Description |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Outil d'automatisation de navigateur multiplateforme basé sur Playwright, prenant en charge le mode headless, l'isolation individuelle, le support complet JS/CSS |
| 10.6.2 | HelpTool | Outil de système de documentation d'aide, prenant en charge la consultation et l'affichage de documentation multilingue |
| 10.6.3 | ProjectWorkNoteTool | Outil de notes de travail de projet, prenant en charge l'enregistrement et la gestion de travail par dimension de projet |
| 10.6.4 | ProjectTaskTool | Outil de gestion de tâches de projet, prenant en charge l'assignation de tâches et le suivi de progression |
| 10.6.5 | KnowledgeTool | Outil du Réseau de Connaissances, prenant en charge les opérations CRUD sur les triplets de connaissances et la découverte de chemins |
| 10.6.6 | ChatHistoryController | Contrôleur de consultation de l'historique de chat, prenant en charge la liste de sessions et les détails des messages |
| 10.6.7 | CodeHoverController | Contrôleur d'info-bulles de code, prenant en charge la coloration syntaxique et les suggestions de code |
| 10.6.8 | WorkNoteController | Contrôleur de gestion des notes de travail, prenant en charge la recherche et la génération de répertoire |
| 10.6.9 | TimerExecutionHistory | Fonctionnalité d'historique d'exécution des minuteurs, enregistrement et consultation de l'historique des déclenchements |
| 10.6.10 | Extension de localisation | Ajout de la prise en charge de localisation tchèque (cs-CZ), total de 21 variantes linguistiques |
| 10.6.11 | Optimisation de l'UI Web | Prise en charge du téléchargement de fichiers, indicateurs de chargement, optimisation du rendu des appels d'outil, correction de la modale des notes de travail |
| 10.6.12 | Amélioration de la gestion de la mémoire | Filtrage avancé, statistiques, vue détaillée, optimisation de l'algorithme de compression |
| 10.6.13 | Refactorisation du système de journalisation | Séparation des journaux système/Êtres de Silicium, API de lecture des journaux, filtre par Être de Silicium |
| 10.6.14 | Amélioration du système d'autorisations | Vérification pré-compilation du rappel d'autorisation, vérification des références d'assembly, liste blanche du service météo wttr.in |

**Livrable** : Automatisation complète du navigateur WebView, système de documentation d'aide, espace de travail de projet, Réseau de Connaissances, consultation de l'historique de chat et autres améliorations.

**Validation** : Les Êtres de Silicium peuvent utiliser le WebViewBrowserTool pour opérer le navigateur → obtenir la documentation d'aide via HelpTool → gérer les notes de travail et les tâches de projet → interroger le Réseau de Connaissances → consulter l'historique de chat.

---

## ~~Phase 10.7 : Collaboration de projet et flux de travail~~ ✅ Terminée

**Objectif** : Ajouter un espace de travail de projet, un moteur de flux de travail, un mécanisme de déclin de la mémoire et un système d'autorisations d'outils.

| # | Module | Description |
|---|--------|-------------|
| 10.7.1 | Gestion des rôles de projet | Ajout des opérations assign_role, remove_role, list_roles à ProjectTool |
| 10.7.2 | Moteur de flux de travail | Moteur principal WorkflowEngine, prenant en charge la définition de modèles, les transitions d'état, l'exécution pilotée par Tick |
| 10.7.3 | Modèles de flux de travail | Classe de base WorkflowTemplate, définissant les ensembles d'états et les règles de transition |
| 10.7.4 | Instances de flux de travail | Gestion d'instances WorkflowInstance, liées à un projet spécifique, suivi de l'état courant |
| 10.7.5 | Journaux de flux de travail | WorkflowLog enregistrant l'historique des transitions d'état |
| 10.7.6 | Mécanisme de déclin de la mémoire | MemoryFadeService service de décroissance minuté, déclin automatique de l'importance et archivage des souvenirs toutes les heures |
| 10.7.7 | Système d'autorisations d'outils | Autorisations d'outil à deux niveaux (niveau Être de Silicium + niveau projet), modèles d'autorisation, contrôle de granularité d'opération |
| 10.7.8 | ToolPermissionController | Contrôleur Web de gestion des autorisations d'outil |
| 10.7.9 | ProjectWorkTool | Outil d'opérations de travail de projet ([SiliconManagerOnly], [ToolScenario(Project)]) |
| 10.7.10 | Système de scénarios d'outils | ToolScenarioAttribute et ChatOnlyAttribute, prenant en charge le filtrage par scénarios Chat/Task/Timer/MemoryCompression/Project |
| 10.7.11 | Extension de localisation | Ajout des localisations russe, portugais, italien, néerlandais, polonais, suédois, total de 34 variantes linguistiques |

**Livrable** : Système complet de collaboration de projet, moteur de flux de travail, mécanisme de déclin de la mémoire et gestion des autorisations d'outils.

**Validation** : Créer un projet → assigner des rôles → lier un modèle de flux de travail → les êtres collaborent dans l'espace projet → déclin automatique et archivage de la mémoire → isolation des autorisations d'outils effective.

---

## Phase 11 : Intégration de messagerie instantanée externe

**Objectif** : Se connecter à des plateformes de messagerie externes pour une accessibilité utilisateur plus large.

| # | Module | Description |
|---|--------|-------------|
| 11.1 | FeishuProvider | Intégration du robot Feishu (Lark), prenant en charge les cartes |
| 11.2 | WhatsAppProvider | Intégration de l'API WhatsApp Business |
| 11.3 | TelegramProvider | Intégration de l'API Telegram Bot, prenant en charge les claviers en ligne |
| 11.4 | Amélioration de IMManager | Routage multi-fournisseur, format de message unifié, traitement des demandes d'autorisation multiplateforme |

**Livrable** : Les utilisateurs peuvent interagir avec les Êtres de Silicium via des plateformes de messagerie instantanée externes.

---

## Phase 11.5 : Système de Compétences et Intégration MCP

**Objectif** : Couche d'abstraction de capacité réutilisable et accès à l'écosystème d'outils externes.

| # | Module | Description |
|---|--------|-------------|
| 11.5.1 | ~~Système de Compétences~~ ✅ Terminée | Couche d'abstraction réutilisable pour l'orchestration d'outils + modèle de prompt (SkillManager, double mode de déclenchement, rechargement à chaud, archivage des versions, complétion des métadonnées par l'IA) |
| 11.5.2 | ~~Intégration MCP~~ ✅ Terminée | Accès aux outils de serveurs MCP externes (double transport stdio/http, injection avec nommage `mcp_{serverId}_{toolName}`, page de gestion Web, intégration de la matrice d'autorisations) |

**Livrables** : Page de gestion des compétences (/skill), page de gestion MCP (/mcp), outils intégrés `skill` et `mcp`, documentation d'aide compétences/MCP.

---

## Phase 12 : Fonctionnalités avancées

**Objectif** : Fonctionnalités avancées optionnelles pour des capacités améliorées.

| # | Module | Description |
|---|--------|-------------|
| 12.1 | ~~Réseau de Connaissances~~ ✅ Terminé | Graph de connaissances à structure triplet (sujet-prédicat-objet), prenant en charge les opérations CRUD, la découverte de chemins, les requêtes avancées et le parcours de graphe |
| 12.2 | ~~Système de plugins~~ ✅ Terminé | Chargement de plugins externes, avec vérifications de sécurité et bac à sable (interface IPlugin, PluginLoader, isolation AssemblyLoadContext) |
| 12.3 | Écosystème de compétences | Marché de compétences réutilisables pour les capacités des êtres |
