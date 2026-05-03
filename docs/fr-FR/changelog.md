# Journal des modifications

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | **Français** | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md)

Tous les changements importants de ce projet seront documentés dans ce fichier.

Le format est basé sur [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
et ce projet adhère à la [Gestion sémantique de version](https://semver.org/spec/v2.0.0.html).

---

## À propos de ce journal des modifications

### Double version du projet

Ce projet propose deux versions d'implémentation :

- **SiliconLife.Default** : Implémentation standard, principalement pour la vérification de faisabilité architecturale. Application console, stockage JSON en système de fichiers.
- **SiliconLife.Fast** : Version principale de production. Application Windows Forms, stockage en mémoire SpeedyPack + persistance asynchrone, optimisation approfondie des performances.

Les deux versions partagent les mêmes interfaces et fonctionnalités, différant uniquement dans l'implémentation du stockage et le mode d'exécution. SiliconLife.Default sert de référence pour la vérification architecturale, SiliconLife.Fast est la version principale recommandée pour la production.

### Origine du projet

- Ce projet a débuté le 20 mars 2026.
- Avant ce projet, une démo de vérification a échoué en raison d'une conception architecturale inadéquate, rendant impossible l'intégration avec plusieurs plateformes IA.

### Outils AI IDE utilisés

#### Kiro (Amazon AWS)
- Le projet a été initialement maintenu par Kiro, démarré en mode Spec.
- Kiro est un environnement de développement IA agentic construit par Amazon AWS.
- Basé sur Code OSS (VS Code), prend en charge les paramètres VS Code et les plugins compatibles Open VSX.
- Flux de développement piloté par les spécifications pour le codage IA structuré.

#### Comate AI IDE / 文心快码 (Baidu)
- Occasionnellement utilisé pour la rédaction et la documentation.
- Comate AI IDE est un outil d'environnement de développement natif IA publié par Baidu Wenxin le 23 juin 2025.
- Premier IDE IA multimodal et multi-agent collaboratif de l'industrie.
- Fonctionnalités incluant la conversion design-code et le codage assisté par IA sur tout le flux.
- Propulsé par le modèle Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Ce projet a été principalement maintenu avec Trae pendant la majeure partie du temps.
- Trae est un IDE IA développé par SPRING PTE, filiale singapourienne de ByteDance.
- En tant que 10x AI Engineer, capable de construire indépendamment des solutions logicielles.
- Fonctionnalités d'outils de productivité intelligents, adaptation flexible du rythme de développement et livraison collaborative de projets.
- Performances de niveau entreprise, système d'agents configurable.

#### Qoder (Alibaba)
- Depuis le 18 avril 2026, ce projet est maintenu avec Qoder.
- Qoder excelle dans l'analyse de code source et la génération de documentation de domaine, avec d'excellentes performances dans la compréhension de bases de code complexes.
- Modèle de tarification à coût de calcul zéro, le rendant très rentable pour le traitement automatisé de documentation et les tâches routinières.
- Plateforme de codage agentic pilotée par l'IA, conçue pour le développement logiciel réel.
- Fonctionnalités de génération de code intelligente, programmation conversationnelle, moteur d'analyse de contexte avancé et collaboration multi-agent.
- Fournit une compréhension approfondie du code avec une consommation minimale de ressources, idéal pour la maintenance de projets à long terme et l'accumulation de connaissances.

### Documentation des exigences

- La documentation des exigences de ce projet n'est pas publique.
- Les exigences ont été validées de manière itérative par plus de 12 plateformes IA internationales et grandes séries de modèles, produisant plus de 2000 lignes de documentation d'exigences pilotée par les user stories, presque incompréhensible pour les humains.

---

## [Non publié]

### 2026-05-03

#### Infrastructure du projet
- `2664b0c` - Mise à jour de l'infrastructure du projet et des dépendances
  - SiliconLife.Speedy.Manager ajoute une interface de gestion WPF (MainForm.Designer.cs, MainForm.resx)
  - Nouvelle ressource d'icône slc.ico (1.5 Mo)
  - PluginLoader améliore considérablement le scan de sécurité (622 lignes ajoutées)
  - Nouvelle PermissionedStreamFactory fabrique de flux avec permissions (779 lignes)
  - Nouvelle PermissionRequestQueue file d'attente de requêtes de permissions (versions Default et Fast)
  - Nouveau DebugLoggerProvider fournisseur de journaux de débogage
  - ConfigDataBase classe de base de configuration améliorée
  - ToolManager ajoute la fonctionnalité de scan des outils de plugin (ScanAllPluginAssemblies)
  - SiliconBeingManager gestion du cycle de vie améliorée
  - DashScopeClient client AI Alibaba Cloud considérablement amélioré (227 lignes ajoutées)
  - DefaultSiliconBeingFactory fabrique améliorée
  - Mise à jour des vues et contrôleurs Web (ChatView, WorkNoteView, PermissionRequestController)
  - 9 langues de localisation ajoutent de nouvelles clés
  - 35 fichiers modifiés, 28080 lignes ajoutées, 336 lignes supprimées

### 2026-05-02

#### Amélioration du client IA
- `c16f99f` - Mise à jour du client IA, de l'interface Web et des composants de stockage
  - DashScopeClient client Alibaba Cloud considérablement amélioré
  - SpeedyPackAutoCompactor optimisation du compacteur automatique
  - ViewBase de vue Web et BeingView améliorés
  - 6 fichiers modifiés, 240 lignes ajoutées, 81 lignes supprimées

#### Système de plugins
- `242dc98` - Ajout de la liste des plugins sur la page À propos
  - AboutController ajout de l'affichage des informations de plugin
  - AboutViewModel ajout du modèle de données de plugin
  - AboutView ajout du rendu de la liste des plugins
  - 9 langues de localisation ajoutent les clés liées aux plugins
  - 14 fichiers modifiés, 160 lignes ajoutées, 1 ligne supprimée

#### Optimisation IA
- `147f8f4` - Simplification du texte d'invite de mémoire contextuelle
  - ContextManager optimisation des prompts IA
  - 1 fichier modifié, 1 ligne ajoutée, 1 ligne supprimée

#### Optimisation du stockage Speedy
- `8bda2d3` - Mise à jour du stockage Speedy et de l'implémentation du contrôleur de mémoire
  - SpeedyPackAutoCompactor correction de l'intervalle
  - SpeedyTimeStorage optimisation du traitement des chemins
  - MemoryController amélioration du contrôleur de mémoire
  - SpeedyPack.Manager mise à jour de l'interface
  - 4 fichiers modifiés, 21 lignes ajoutées, 18 lignes supprimées

#### Amélioration de la barre d'état système
- `8972654` - Amélioration du support de localisation de la fenêtre d'état de la barre d'état système
  - 9 langues de localisation de la barre d'état ajoutent l'entrée de gestion Speedy
  - TrayStatusWindow ajout d'un élément de menu de gestion Speedy
  - 11 fichiers modifiés, 72 lignes ajoutées

#### Optimisation de Speedy.Manager
- `6f5db09` - Optimisation de l'interface SpeedyPack Manager et des composants internes
  - Refonte de l'interface MainForm
  - Optimisation de la gestion mémoire FreeList
  - Amélioration de la file d'attente d'écriture WriteQueue
  - Optimisation du cœur SpeedyPack
  - 5 fichiers modifiés, 96 lignes ajoutées, 88 lignes supprimées

#### Amélioration du système de stockage
- `57f9d5d` - Amélioration du système de stockage, ajout du compactage automatique et du support des dates incomplètes
  - Nouveau SpeedyPackAutoCompactor minuteur de compactage automatique (intervalle de 30 minutes)
  - SpeedyPackRegistry gestionnaire singleton amélioré
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage adaptations améliorées
  - SpeedyPack ajout de FreeList gestion d'espace libre (149 lignes)
  - PackFileWriter réécriture et optimisation
  - WriteOperation, WriteQueue file d'attente d'écriture améliorée
  - SpeedyPackOptions extension des options de configuration
  - IncompleteDate ajout de méthodes de comparaison
  - PluginLoader amélioration du chargeur de plugins
  - Mise à jour du flux d'initialisation de Program.cs pour les versions Default et Fast
  - DefaultConfigData simplification des données de configuration
  - KnowledgeNetwork rationalisation du réseau de connaissances
  - ChatController, MemoryController optimisation des contrôleurs
  - SpeedyPack.Manager MainForm amélioration des fonctionnalités
  - 22 fichiers modifiés, 639 lignes ajoutées, 253 lignes supprimées

#### Mise à jour de Speedy.Manager
- `b04ed33` - Mise à jour des fichiers Speedy.Manager

### 2026-05-01

#### Refonte architecturale : Speedy Storage remplace LiteDB
- `6600972` - Remplacement de LiteDB par Speedy Storage, ajout du système de plugins et du projet Speedy
  - **Nouveau projet SiliconLife.Speedy** : Moteur de stockage .spk haute performance
    - SpeedyPack classe principale (489 lignes) : mappage de répertoire en mémoire + cache d'entrées + file d'attente d'écriture asynchrone
    - SpeedyPackOptions classe de configuration : TTL du cache, nombre max d'entrées en cache, mode lecture seule
    - IPackTransaction interface de transaction : support d'opérations d'écriture atomiques
    - SpkFileInfo classe d'informations fichier
    - Répertoire Internal : DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Dépendance MessagePack 3.1.4 pour la sérialisation binaire (compression LZ4)
  - **Nouveau projet SiliconLife.Speedy.Manager** : Outil de gestion WPF
    - Architecture MVVM : MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel, etc.
    - Couche de services : PackService, FileDialogService, RecentFilesService, NotificationService
    - Convertisseurs : BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - Vues : MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - Dialogues : FileInfoDialog, ImportDialog, NewEntryDialog
  - **Migration du stockage SiliconLife.Fast** : LiteDB → SpeedyPack
    - Nouveau SpeedyStorage (adaptateur IStorage)
    - Nouveau SpeedyTimeStorage (adaptateur ITimeStorage)
    - Nouveau SpeedyWorkNoteStorage (adaptateur IWorkNoteStorage)
    - Nouveau SpeedyPackRegistry (gestionnaire singleton au niveau processus)
    - Nouveau SpeedyPackAutoCompactor (minuteur de compactage automatique)
    - Suppression des implémentations de stockage LiteDB (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Suppression du code lié à la fenêtre de gestion LiteDB
  - **Système de plugins** :
    - Nouvelle interface IPlugin (Core/Plugins/IPlugin.cs)
    - Nouveau PluginLoader chargeur de plugins (Core/Plugins/PluginLoader.cs)
    - Support du chargement de DLL de plugins depuis un répertoire
    - Scan de sécurité : vérification des espaces de noms interdits (System.IO, System.Net, Microsoft.CodeAnalysis, etc.)
    - Liste blanche d'assemblies de confiance (Google.Protobuf, Newtonsoft.Json, MessagePack, etc.)
    - Chargement isolé via AssemblyLoadContext personnalisé
    - ToolManager ajoute la méthode ScanAllPluginAssemblies
    - CoreHost intégration du chargeur de plugins
  - 119 fichiers modifiés, 6926 lignes ajoutées, 3066 lignes supprimées

#### Amélioration des Silicon Beings
- `3aef4c3` - Ajout de l'état d'activité Stopped et amélioration de la gestion des erreurs
  - Nouvel état Stopped pour les Silicon Beings
  - Amélioration de la gestion des erreurs et du mécanisme de récupération

#### Mise à jour de la localisation
- `513c65d` - Mise à jour de toutes les versions linguistiques et de la documentation
  - Nouveau MarkdownEditorComponent composant (625 lignes)
  - Nouveau DetailsComponent composant (130 lignes)
  - Nouveau AccordionComponent composant accordéon (285 lignes)
  - BeingController, ChatController, MemoryController, PermissionController mise à jour des contrôleurs
  - BeingView, ChatView, MemoryView, SoulEditorView refonte des vues
  - Suppression de l'ancien MarkdownEditorView
  - InitController migration vers les composants
  - 115 fichiers modifiés, 5761 lignes ajoutées, 2362 lignes supprimées

### 2026-04-30

#### Fonctionnalité de la barre d'état système
- `101b203` - Implémentation de la fenêtre d'état de la barre d'état système et ApplicationContext
  - Nouvelles ressources d'icônes de barre d'état (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implémentation de TrayStatusWindow fenêtre d'état
  - Support de localisation de la barre d'état en 9 langues (TrayCsCZ, TrayDeDE, TrayEnUS, etc.)
  - TrayLocalizationBase classe de base abstraite
  - 24 fichiers modifiés, 27995 lignes ajoutées, 1 ligne supprimée (incluant les fichiers de ressources)

#### Architecture UI en composants
- `e61cfaa` - Achèvement de l'architecture UI en composants, implémentation de 24 composants
  - Phase MVP (8) : ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Deuxième phase (6) : Accordion, Card, Tabs, Table, Modal, Message
  - Troisième phase (5) : Calendar, Tree, Chart, FileUpload, RichText
  - Nouvelles classes auxiliaires Js, Behavior, DomUpdate
  - 25 fichiers modifiés, 2666 lignes ajoutées

- `7449e51` - Amélioration du système de composants et ajout de nouveaux thèmes de skins
  - Amélioration des composants A, Button, Div, Form, Input, etc.
  - 3 nouveaux thèmes de skins : HighContrast (contraste élevé), Light (clair), Minimal (minimaliste)
  - Mise à jour des skins existants (Admin, Chat, Creative, Dev)
  - InitController migration vers les composants
  - 32 fichiers modifiés, 1466 lignes ajoutées, 1238 lignes supprimées

- `1ba8636` - Début de la migration d'InitController en composants (en cours)
  - 9 fichiers modifiés, 574 lignes ajoutées, 145 lignes supprimées

#### Unification du système de stockage
- `895dff9` - Unification de soul.md et state.json pour utiliser l'interface IStorage
  - DefaultSiliconBeing utilise IStorage pour lire/écrire les fichiers âme et d'état
  - Nouveau StateFileManager gestionnaire de fichiers d'état
  - SoulFileManager refonte pour adapter IStorage
  - 8 fichiers modifiés, 201 lignes ajoutées, 116 lignes supprimées

#### Amélioration de la gestion LiteDB
- `a34bef4` - Ajout de LiteDBManager et amélioration de la localisation de la barre d'état
  - Menu de la barre d'état ajout d'une entrée de gestion LiteDB
  - Mise à jour de la localisation de la barre d'état en 9 langues
  - 10 fichiers modifiés, 196 lignes ajoutées

- `c4a79ca` - Ajout d'une fabrique de localisation sensible à la langue pour la fenêtre de gestion LiteDB
  - 1 fichier modifié, 78 lignes ajoutées

- `5ebc55e` - Conversion de LiteDBAdminLocalization en classe de base abstraite
  - 10 fichiers modifiés, 1356 lignes ajoutées

#### Correction du système de configuration
- `2da5256` - Ajout de la méthode abstraite ConfigExists et correction des enregistrements de configuration LiteDB en double
  - ConfigDataBase ajout de la méthode ConfigExists
  - La version Fast de DefaultConfigData implémente la vérification d'existence de configuration LiteDB
  - Correction du problème de clés de configuration LiteDB en double
  - 9 fichiers modifiés, 210 lignes ajoutées, 2 lignes supprimées

#### Optimisation du chat et des vues
- `d3618ec` - Optimisation des sessions de chat, du système de stockage, du modèle temporel et de la classe de base des vues
  - BroadcastChannel, GroupChatSession, SingleChatSession optimisés
  - ITimeStorage ajout de méthodes de requête
  - FileSystemStorage et LiteDBStorage mis à jour de manière synchrone
  - ViewBase refonte et optimisation (versions Default et Fast)
  - 11 fichiers modifiés, 622 lignes ajoutées, 392 lignes supprimées

### 2026-04-29

#### Refonte architecturale : Extraction du module partagé
- `a102428` - Migration des modules partagés de SiliconLife.Default vers SiliconLife.Common
  - Extraction de 32 implémentations de calendriers vers le projet Common
  - Extraction des classes de base de localisation et de 21 implémentations linguistiques vers le projet Common
  - Extraction du gestionnaire de permissions et de l'implémentation par défaut du Silicon Being vers le projet Common
  - Extraction de 23 implémentations d'outils intégrés vers le projet Common
  - Extraction de l'implémentation Playwright WebView vers le projet Common
  - Mise à jour des espaces de noms vers SiliconLife.Collective
  - 122 fichiers modifiés, 586 lignes ajoutées, 343 lignes supprimées

#### Amélioration de la qualité du code
- `17566fe` - Remplacement de Console.WriteLine par le système de journalisation dans les projets Core, Common et Default
  - ContextManager, AuditLogger, DefaultConfigData et 6 autres fichiers mis à jour
  - Unification de l'utilisation de l'interface ILogger, amélioration de la maintenabilité du code
  - 6 fichiers modifiés, 12 lignes ajoutées, 8 lignes supprimées

#### Version haute performance SiliconLife.Fast
- `54a0307` - Ajout du projet SiliconLife.Fast et correction de la compilation
  - Point d'entrée complet de l'application Windows Forms
  - Support de la barre d'état système (NotifyIcon)
  - Portage de tous les contrôleurs Web UI (20+)
  - Portage de tous les composants de vue Web
  - Portage de 4 thèmes de skins (Admin, Chat, Creative, Dev)
  - 125 fichiers modifiés, 61186 lignes ajoutées

#### Synchronisation de la documentation multilingue
- `265fde8` - Synchronisation de la documentation d'architecture en double version vers toutes les langues
  - Mise à jour de architecture.md, changelog.md en 7 langues
  - Mise à jour de contributing.md en 6 langues
  - Mise à jour de getting-started.md, roadmap.md en 7 langues
  - 47 fichiers modifiés, 1214 lignes ajoutées, 38 lignes supprimées

#### Système de stockage LiteDB (version Fast)
- `4704862` - Ajout des dépendances et de l'infrastructure LiteDB
  - Nouveau LiteDBManager classe de gestion
  - Nouveau LiteDBModels modèle de données
  - 3 fichiers modifiés, 252 lignes ajoutées

- `4220036` - Implémentation des classes de stockage LiteDB
  - LiteDBStorage : implémentation de l'interface IStorage
  - LiteDBTimeStorage : implémentation de l'interface ITimeStorage
  - LiteDBWorkNoteStorage : implémentation de l'interface IWorkNoteStorage
  - 3 fichiers modifiés, 581 lignes ajoutées

- `38ebd23` - Migration de la configuration et du système de journalisation vers LiteDB
  - DefaultConfigData adaptation au stockage LiteDB
  - Nouveau LiteDBLoggerProvider fournisseur de journaux
  - 2 fichiers modifiés, 203 lignes ajoutées, 67 lignes supprimées

- `e687157` - Migration du réseau de connaissances du système de fichiers vers LiteDB
  - KnowledgeNetwork refonte complète, utilisation de LiteDB pour le stockage des triplets
  - 1 fichier modifié, 231 lignes ajoutées, 72 lignes supprimées

- `4220169` - Intégration du stockage LiteDB dans Program et ProjectManager
  - Program.cs initialisation du stockage LiteDB
  - ProjectManager adaptation au stockage de notes de travail LiteDB
  - 2 fichiers modifiés, 40 lignes ajoutées, 17 lignes supprimées

- `5f3a709` - Suppression des implémentations de stockage de fichiers obsolètes
  - Suppression de FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage, etc.
  - 6 fichiers modifiés, 1518 lignes supprimées

- `e1a4ef2` - docs: ajout de l'identifiant de version v0.1.0-alpha à toute la documentation
  - 127 fichiers modifiés, 2297 lignes ajoutées, 2471 lignes supprimées

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Refonte du système de stockage
- `8dd26e3` - Unification de l'interface ITimeStorage avec IncompleteDate et ajout d'API de requête hiérarchique
  - Suppression des méthodes de surcharge DateTime de l'interface ITimeStorage, unification avec IncompleteDate
  - IncompleteDate ajout de la méthode de comparaison CompareTo(DateTime) et de la méthode Expand()
  - Nouvelles API de requête hiérarchique GetEarliestTimestamp(), GetLatestTimestamp()
  - Nouvelles méthodes HasSummary() et QueryWithLevel(), support de requêtes par niveau temporel
  - Memory.cs refonte de l'algorithme de compression, utilisation de la nouvelle API de requête hiérarchique pour améliorer l'efficacité
  - FileSystemTimeStorage.cs implémentation complète des nouvelles méthodes d'interface
  - Mise à jour synchrone de tous les appelants : ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, etc.
  - Mise à jour du système d'outils : HelpTool, LogTool, TokenAuditTool adaptation à la nouvelle interface
  - Mise à jour des contrôleurs Web : AuditController, ChatController, ChatHistoryController adaptation à la nouvelle interface
  - 41 fichiers modifiés, 1820 lignes ajoutées, 903 lignes supprimées

### 2026-04-27

#### Amélioration du système de documentation d'aide
- `9989d79` - Mise à jour de la localisation, du système d'aide et des vues Web
  - Nouveau IAIClientFactoryHelp.cs interface de documentation d'aide de la fabrique de clients IA
  - Traduction complète de toute la documentation d'aide en 9 langues
  - HelpTopics.cs ajout de 40 définitions de sujets d'aide
  - Mise à jour complète des vues Web : InitController, AuditView, ConfigView, KnowledgeView, LogView, etc.
  - Amélioration du système de localisation : ajout de nouvelles clés de localisation pour toutes les versions linguistiques
  - Mise à jour de la fabrique de clients IA : amélioration de DashScopeClientFactory, OllamaClientFactory
  - 30 fichiers modifiés, 10086 lignes ajoutées, 15 lignes supprimées

#### Nouveau contenu de la documentation d'aide
- `e7afe94` - Ajout de la documentation d'aide sur les fichiers âme et les journaux d'audit
  - Nouvelle documentation d'aide sur la gestion des fichiers âme
  - Nouvelle documentation d'aide sur les journaux d'audit
  - HelpTopics.cs ajout de définitions de sujets
  - HelpView.cs refonte majeure, amélioration de la logique de rendu de la documentation
  - PermissionView.cs refonte, amélioration de l'interface de gestion des permissions
  - Amélioration des modules principaux : SiliconBeingManager, TaskSystem, ToolManager
  - TaskTool.cs refonte, amélioration de la gestion des tâches
  - Mise à jour complète des vues Web : tous les composants de vue mis à jour synchrone
  - HelpController.cs simplification, optimisation de la logique du contrôleur
  - 30 fichiers modifiés, 7100 lignes ajoutées, 897 lignes supprimées

### 2026-04-26

#### Système de documentation d'aide
- `07895d7` - Amélioration du système de documentation d'aide, ajout de 3 documents et traduction en 9 langues
  - Ajout des guides du système de mémoire, de l'installation Ollama et de l'utilisation de la plateforme Alibaba Cloud Bailian
  - Traduction complète de 10 documents d'aide en 9 langues
  - Simplification de la logique de rendu de HelpView
  - 18 fichiers modifiés, 14418 lignes ajoutées, 1364 lignes supprimées

#### Localisation allemande
- `0cfd8a1` - Ajout du support complet de localisation allemande (de-DE)
  - Fichier de localisation allemande complet
  - Ajout du support allemand pour le calendrier historique chinois
  - Ajout de la traduction allemande de la documentation d'aide
  - Synchronisation complète de toute la documentation en 9 langues
  - 135 fichiers modifiés, 26186 lignes ajoutées, 14371 lignes supprimées

#### Synchronisation de la documentation
- `3aada7d` - Synchronisation de la documentation chinois traditionnel (zh-HK) avec le chinois simplifié
  - 3 fichiers modifiés, 519 lignes ajoutées, 422 lignes supprimées
- `2f6abff` - Ajout de la localisation du nom d'affichage de l'outil d'aide pour toutes les langues
  - 7 fichiers modifiés, 47 lignes ajoutées, 7 lignes supprimées

#### Refonte du système de connaissances
- `60944fe` - Unification de l'espace de noms vers SiliconLife.Collective
  - 8 fichiers modifiés, 5 lignes ajoutées, 8 lignes supprimées
- `69c51c5` - Ajout du système de documentation d'aide et traduction des commentaires de code en anglais
  - 29 fichiers modifiés, 3385 lignes ajoutées, 22 lignes supprimées

### 2026-04-25

#### Automatisation du navigateur WebView
- `41757c3` - Implémentation de l'automatisation du navigateur WebView multiplateforme basée sur Playwright
  - 6 fichiers modifiés, 1152 lignes ajoutées

#### Mise à jour de la documentation
- `0ff797b` - Ajout de la documentation KnowledgeTool et WorkNoteTool (7 langues)
  - 28 fichiers modifiés, 4983 lignes ajoutées
- `ad77415` - Mise à jour de tous les fichiers changelog, ajout des enregistrements d'historique Git du 2026-04-25
  - 7 fichiers modifiés, 168 lignes ajoutées

#### Gestion de l'espace de travail de projet
- `785c551` - Implémentation de la gestion de l'espace de travail de projet, avec notes de travail et système de tâches
  - Nouveau système de gestion d'espace de travail de projet
  - Fonctionnalité de notes de travail pour suivre la progression du projet
  - Intégration du système de gestion des tâches
  - 29 fichiers modifiés, 4256 lignes ajoutées, 36 lignes supprimées

#### Localisation tchèque
- `b4bbf39` - Ajout de la localisation tchèque complète (cs-CZ) et mise à jour de toute la documentation linguistique
  - 116 fichiers modifiés, 4933 lignes ajoutées, 222 lignes supprimées
- `faf078f` - Correction des erreurs de compilation de la localisation tchèque
  - 3 fichiers modifiés, 910 lignes ajoutées, 1 ligne supprimée

#### Amélioration du système de connaissances
- `20adaac` - Ajout de KnowledgeTool et support de localisation complet
  - 34 fichiers modifiés, 2331 lignes ajoutées, 56 lignes supprimées

### 2026-04-24

#### Amélioration du système de gestion de la mémoire
- `c7b2ecc` - Amélioration de la gestion de la mémoire, ajout du filtrage avancé, des statistiques et de la vue détaillée
  - Nouvelle fonctionnalité de filtrage avancé de la mémoire
  - Implémentation de la fonctionnalité de statistiques de mémoire
  - Ajout de la page de vue détaillée de la mémoire
  - Support de localisation multilingue (6 langues)
  - 13 fichiers modifiés, 840 lignes ajoutées, 86 lignes supprimées

#### Extension du système de permissions
- `4489ad6` - Ajout du service météo wttr.in à la liste blanche réseau
  - Mise à jour synchrone complète de la documentation multilingue (6 langues)
  - 14 fichiers modifiés, 417 lignes ajoutées, 1 ligne supprimée

#### Correction de l'interface Web
- `d9d72e9` - Correction du problème de priorité CSS de la modale de détail des notes de travail
  - 19 fichiers modifiés, 1744 lignes ajoutées, 6 lignes supprimées

#### Optimisation de l'historique de chat
- `db48c51` - Ajout de la file d'attente de messages de chat, des métadonnées de fichier et du support d'annulation de flux
  - 4 fichiers modifiés, 357 lignes ajoutées

#### Support du téléchargement de fichiers
- `28fb344` - Implémentation du dialogue de source de fichier et du support de téléchargement de fichiers
  - 3 fichiers modifiés, 1100 lignes ajoutées, 2 lignes supprimées
- `1d3e2cc` - Ajout des chaînes de localisation du dialogue de source de fichier (6 langues)
  - 6 fichiers modifiés, 30 lignes ajoutées

#### Mise à jour de la documentation
- `8111e92` - Ajout du lien Wiki dans la section dépôt du README
  - 1 fichier modifié, 3 lignes ajoutées, 1 ligne supprimée

### 2026-04-22

#### Localisation de la documentation
- `66c11eb` - Traduction des commentaires chinois en anglais et mise à jour de tous les changelogs
  - 11 fichiers modifiés, 373 lignes ajoutées, 163 lignes supprimées

#### Amélioration des messages SSE
- `b574b2b` - Ajout de senderName pour les messages historiques pour l'identification IA
  - 1 fichier modifié, 9 lignes ajoutées

#### Fonctionnalité de chat
- `601fc14` - Ajout de l'opération mark_read pour le marquage de fin de session
  - 7 fichiers modifiés, 196 lignes ajoutées, 36 lignes supprimées

#### Optimisation du système d'outils
- `7a03a19` - Amélioration de la flexibilité de requête de conversation de LogTool
  - 1 fichier modifié, 57 lignes ajoutées, 24 lignes supprimées

#### Amélioration de la localisation
- `0a8d750` - Ajout d'un prompt système générique pour le comportement proactif des Silicon Beings
  - 8 fichiers modifiés, 460 lignes ajoutées, 48 lignes supprimées

#### Refonte du système de journalisation
- `2b771f3` - Découplage de LogController des E/S fichier, ajout de l'API de lecture des journaux
  - 4 fichiers modifiés, 172 lignes ajoutées, 137 lignes supprimées
- `12da302` - Ajout d'un filtre par Silicon Being pour la vue des journaux
  - 9 fichiers modifiés, 147 lignes ajoutées, 10 lignes supprimées
- `8f6cb1e` - Ajout du paramètre beingId à l'interface ILogger, séparation des journaux système/Silicon Beings
  - 47 fichiers modifiés, 524 lignes ajoutées, 490 lignes supprimées

#### Amélioration du système de permissions
- `4c747ad` - Refonte de PermissionTool, ExecuteCodeTool, ajout de l'API EvaluatePermission
  - 18 fichiers modifiés, 680 lignes ajoutées, 492 lignes supprimées

#### Corrections de bugs
- `1c96e99` - Correction de l'échec de search_files et search_content lors de la recherche dans le répertoire racine
  - 1 fichier modifié, 98 lignes ajoutées, 41 lignes supprimées

#### Intégration des outils
- `135710d` - Suppression de SearchTool, déplacement de la recherche locale vers DiskTool
  - 2 fichiers modifiés, 185 lignes ajoutées, 365 lignes supprimées

#### Extension du système d'outils
- `70ce7fb` - Implémentation de DatabaseTool pour les requêtes de base de données structurées
  - 1 fichier modifié, 382 lignes ajoutées
- `be29a09` - Implémentation de LogTool pour les requêtes d'historique des opérations et des conversations
  - 1 fichier modifié, 298 lignes ajoutées
- `4ea7702` - Implémentation de PermissionTool pour la gestion dynamique des permissions
  - 1 fichier modifié, 457 lignes ajoutées
- `1384ff4` - Implémentation de ExecuteCodeTool pour l'exécution de code multi-langage
  - 1 fichier modifié, 477 lignes ajoutées
- `82d1e11` - Implémentation de SearchTool pour la recherche d'informations
  - 1 fichier modifié, 363 lignes ajoutées

#### Optimisation de l'interface Web
- `0675c45` - Optimisation de la coloration des blocs de code markdown dans le volet de prévisualisation
  - 1 fichier modifié, 4 lignes ajoutées, 23 lignes supprimées
- `702b3f3` - Amélioration de la vue des tâches, ajout de badges d'état et d'affichage de métadonnées
  - 8 fichiers modifiés, 221 lignes ajoutées, 9 lignes supprimées
- `6ed9a79` - Amélioration du stockage des messages de chat et du rendu des vues
  - 8 fichiers modifiés, 140 lignes ajoutées, 29 lignes supprimées

### 2026-04-21

#### Corrections de bugs
- `c6b518b` - Correction de la transmission des messages du minuteur et du stockage des messages de chat
  - 3 fichiers modifiés, 297 lignes ajoutées, 124 lignes supprimées

#### Gestion de la configuration
- `4305769` - Ajout de .gitattributes pour la gestion des fins de ligne
  - 1 fichier modifié, 32 lignes ajoutées

#### Améliorations de l'interface Web
- `188c6f8` - Enregistrement de la route API de la liste des tâches et ajout de l'affichage d'état vide
  - 2 fichiers modifiés, 35 lignes ajoutées, 2 lignes supprimées
- `634e8ca` - Ajout d'un lien de retour à la liste sur la page des permissions
  - 1 fichier modifié, 16 lignes ajoutées
- `6ba591d` - Ajout d'un éditeur de configuration IA indépendant pour les Silicon Beings
  - 11 fichiers modifiés, 842 lignes ajoutées, 18 lignes supprimées
- `0a826f5` - Ajout d'une notification de sauvegarde réussie dans l'éditeur de code
  - 1 fichier modifié, 9 lignes ajoutées, 2 lignes supprimées
- `2940373` - Amélioration de l'interface Web, ajout d'info-bulles de code et améliorations UI
  - 11 fichiers modifiés, 1054 lignes ajoutées, 75 lignes supprimées

#### Correction du système de permissions
- `592c7ab` - Correction de l'instanciation des rappels et de l'ordre d'enregistrement
  - 2 fichiers modifiés, 38 lignes ajoutées, 7 lignes supprimées

#### Amélioration de la sécurité
- `833ead2` - Ajout de la validation des références d'assembly pour la compilation dynamique
  - 4 fichiers modifiés, 135 lignes ajoutées, 8 lignes supprimées

#### Amélioration du système de permissions
- `5879621` - Ajout de la pré-validation de compilation des rappels de permission et amélioration de la gestion des erreurs
  - 21 fichiers modifiés, 617 lignes ajoutées, 26 lignes supprimées

#### Mise à jour de la documentation
- `4dbf659` - Mise à jour du changelog à v0.5.1, remplacement des URL GitHub placeholder, ajout du miroir Gitee, localisation du nom Bilibili par langue, mise à jour de l'email
  - 32 fichiers modifiés, 489 lignes ajoutées, 180 lignes supprimées

#### Configuration et point d'entrée
- `0fc1693` - Mise à jour du point d'entrée du programme et de la configuration du projet
  - 2 fichiers modifiés, 7 lignes ajoutées

#### Refonte du système de permissions
- `ea9179a` - Amélioration de l'implémentation du système de permissions
  - 5 fichiers modifiés, 358 lignes ajoutées, 152 lignes supprimées

#### Corrections de bugs
- `928a96d` - Correction de l'implémentation du calcul du calendrier
  - 4 fichiers modifiés, 12 lignes ajoutées, 12 lignes supprimées

#### IA et calendrier
- `646813e` - Amélioration de l'implémentation de la fabrique de clients IA
  - 2 fichiers modifiés, 21 lignes ajoutées, 20 lignes supprimées

#### Localisation
- `7940d9c` - Ajout du support de localisation coréenne
  - 7 fichiers modifiés, 2424 lignes ajoutées, 10 lignes supprimées
- `4ff98ad` - Refonte de la documentation, support multilingue
  - 81 fichiers modifiés, 23818 lignes ajoutées, 1886 lignes supprimées

### 2026-04-20

#### Amélioration des fonctionnalités principales
- `28905b5` - Support multilingue complet, fabrique de clients IA, système de permissions et paramètres de localisation
  - Système de journalisation avec gestionnaire, entrées et différents niveaux de journalisation
  - Système d'audit de tokens pour interroger et suivre l'utilisation des tokens
  - Fabrique de clients IA avec découverte automatique de différentes plateformes IA
  - Système de rappel de permissions avec son propre stockage
  - Implémentation du journaliseur console
  - Support multilingue anglais et chinois simplifié
  - WebUI messenger avec WebSocket pour le chat en temps réel
  - Amélioration du Silicon Being par défaut avec localisation
  - 39 fichiers modifiés, 4670 lignes ajoutées, 175 lignes supprimées

### 2026-04-19

#### Minuteurs et calendriers
- `c933fd8` - Mise à jour de la localisation, du système de minuteurs, des vues Web et ajout d'outils
  - Meilleur gestionnaire de localisation
  - Système de planification des tâches minutées
  - Configuration IA et gestion du contexte
  - Outil de calendrier supportant 32 types de calendriers
  - Contrôleur Web pour l'API calendrier
  - Outil de gestion des tâches
  - 46 fichiers modifiés, 4018 lignes ajoutées, 975 lignes supprimées

**Améliorations architecturales**
- Refonte de l'architecture des vues Web pour mieux supporter les skins
- Amélioration du système de gestion des Beings avec une meilleure gestion des états

### 2026-04-18

- `9f585e1` - Mise à jour de la localisation, du système de minuteurs, des vues Web et ajout d'outils
  - Améliorations des minuteurs et de la planification
  - Meilleures vues Web avec composants UI améliorés
  - Plus d'implémentations d'outils
  - 57 fichiers modifiés, 3328 lignes ajoutées, 389 lignes supprimées

### 2026-04-17

- `9b71fcd` - Mise à jour des modules principaux, ajout de la documentation zh-HK, des canaux de diffusion, de l'outil de configuration et de la vue Web d'audit
  - Canal de diffusion pour que plusieurs Silicon Beings discutent ensemble
  - Système d'outil de configuration
  - Vue Web d'audit
  - Documentation chinois traditionnel
  - 42 fichiers modifiés, 3533 lignes ajoutées, 268 lignes supprimées

### 2026-04-16

- `5040f05` - Mise à jour des modules principaux et par défaut
  - Optimisation des modules et corrections de bugs
  - Mises à jour et améliorations des implémentations
  - 58 fichiers modifiés, 9916 lignes ajoutées, 111 lignes supprimées

### 2026-04-15

- `3efab5f` - Mise à jour de plusieurs modules : IA, Chat, IM, Outils, Web, Localisation, Stockage
  - Améliorations du client IA
  - Améliorations du système de chat
  - Mise à jour des fournisseurs de messagerie
  - Optimisation du système d'outils
  - Améliorations de l'infrastructure Web
  - Optimisation de la localisation
  - Mise à jour du système de stockage
  - 33 fichiers modifiés, 788 lignes ajoutées, 232 lignes supprimées

### 2026-04-14

- `4241a2f` - Fonctionnalité de chat fondamentalement terminée, optimisation du téléchargement UI
  - Fonctionnalité du système de chat terminée
  - Optimisation UI du téléchargement de fichiers
  - 16 fichiers modifiés, 1234 lignes ajoutées, 102 lignes supprimées

### 2026-04-13

- `c498c31` - Mise à jour du code
  - Améliorations et optimisations générales du code
  - 32 fichiers modifiés, 1045 lignes ajoutées, 546 lignes supprimées

### 2026-04-12

#### Documentation et localisation
- `2161002` - Refonte de la documentation et amélioration de la localisation
  - 17 fichiers modifiés, 982 lignes ajoutées, 92 lignes supprimées
- `03d94e4` - Amélioration du système de configuration et de la localisation
  - 25 fichiers modifiés, 1378 lignes ajoutées, 154 lignes supprimées
- `9976a35` - Ajout de la page À propos et de la localisation
  - 14 fichiers modifiés, 699 lignes ajoutées, 44 lignes supprimées

#### Chat et vues Web
- `0c8ccfc` - Amélioration du système de chat, de la localisation et des vues Web
  - 13 fichiers modifiés, 402 lignes ajoutées, 56 lignes supprimées
- `a8f1342` - Refonte de la couche de communication Web, passage de WebSocket à SSE
  - 27 fichiers modifiés, 793 lignes ajoutées, 935 lignes supprimées

### 2026-04-11

#### Système de journalisation
- `e8fe259` - Ajout du système de journalisation et optimisation du code
  - 37 fichiers modifiés, 624 lignes ajoutées, 91 lignes supprimées
- `f01c519` - Ajout du système de journalisation, mise à jour de l'interface IA et des vues Web
  - 31 fichiers modifiés, 1758 lignes ajoutées, 63 lignes supprimées

### 2026-04-10

- `4962924` - Amélioration du gestionnaire WebSocket, de la vue de chat et de l'interaction messenger
  - Améliorations du gestionnaire de contexte
  - Améliorations du système de chat
  - Mise à jour de l'interface du fournisseur de messagerie
  - Refonte du fournisseur WebUI
  - Mise à jour du constructeur JavaScript et du routeur
  - Optimisation de la vue de chat
  - Améliorations du gestionnaire WebSocket
  - 9 fichiers modifiés, 365 lignes ajoutées, 134 lignes supprimées

### 2026-04-09

- `f9302bf` - Amélioration de l'interface du fournisseur de messagerie, du système de chat et de l'interaction Web UI
  - Extension de l'interface du fournisseur de messagerie
  - Améliorations des messages de chat et du système
  - Optimisation du gestionnaire de contexte
  - Amélioration du Silicon Being par défaut
  - Améliorations de la vue de chat Web UI
  - Mise à jour du gestionnaire WebSocket
  - 10 fichiers modifiés, 427 lignes ajoutées, 93 lignes supprimées

### 2026-04-07

- `6831ee8` - Refonte des vues Web et du constructeur JavaScript
  - Refonte complète des contrôleurs Web
  - Réécriture complète du constructeur JavaScript
  - Mise à jour de tous les composants de vue
  - Améliorations du système de skins
  - Amélioration de l'architecture de la classe de base des vues
  - 23 fichiers modifiés, 2004 lignes ajoutées, 1983 lignes supprimées

### 2026-04-05

- `41e97fb` - Mise à jour de plusieurs modules principaux et contrôleurs Web
  - Améliorations du gestionnaire de contexte
  - Système de chat et gestion des sessions
  - Refonte du localisateur de services
  - Mise à jour de la classe de base et du gestionnaire de Silicon Beings
  - Mise à jour complète des contrôleurs Web (17 contrôleurs)
  - Amélioration de la fabrique de Silicon Beings par défaut
  - 31 fichiers modifiés, 681 lignes ajoutées, 326 lignes supprimées
- `67988d4` - Amélioration du module Web UI, ajout de la vue d'exécuteur, nettoyage des vues et modules principaux
  - 61 fichiers modifiés, 3148 lignes ajoutées, 3726 lignes supprimées

### 2026-04-04

- `b58bb1c` - Ajout du contrôleur d'initialisation et refonte du module Web
  - Contrôleur d'initialisation
  - Refonte du module de configuration
  - Mise à jour du module de localisation
  - Améliorations du système de skins
  - Améliorations du routeur
  - 29 fichiers modifiés, 1269 lignes ajoutées, 289 lignes supprimées
- `f03ac0b` - Ajout du module Web UI, amélioration de la fonctionnalité messenger
  - 60 fichiers modifiés, 8481 lignes ajoutées, 165 lignes supprimées

### 2026-04-03

- `192e57b` - Mise à jour de la structure du projet et des composants d'exécution principaux
  - 22 fichiers modifiés, 446 lignes ajoutées, 179 lignes supprimées
- `59faec8` - Mise à jour des implémentations principales et par défaut
  - 25 fichiers modifiés, 3056 lignes ajoutées, 18 lignes supprimées
- `d488485` - Ajout de la compilation dynamique et du module d'outils du Curator
  - 19 fichiers modifiés, 1727 lignes ajoutées, 11 lignes supprimées
- `753d1d9` - Ajout du module de sécurité, mise à jour des exécuteurs, fournisseurs de messagerie, localisation et outils
  - 29 fichiers modifiés, 2352 lignes ajoutées, 93 lignes supprimées
- `a378697` - Achèvement de la phase 5 - Système d'outils + Exécuteurs
  - 41 fichiers modifiés, 2651 lignes ajoutées, 363 lignes supprimées

### 2026-04-02

- `e6ad94b` - Correction de l'échec du chargement de l'historique de chat lors de la suppression du fichier de configuration pendant les tests
  - 4 fichiers modifiés, 49 lignes ajoutées, 45 lignes supprimées
- `daa56f5` - Achèvement de la phase 4 : Mémoire persistante (système de chat + canal messenger)
  - 29 fichiers modifiés, 2051 lignes ajoutées, 538 lignes supprimées

### 2026-04-01

- `bbe2dbb` - Correction du chargement de la configuration et du routage des messages du service de chat
  - 27 fichiers modifiés, 1633 lignes ajoutées, 147 lignes supprimées
- `2fa6305` - Implémentation de la phase 2 : Cadre de la boucle principale et système d'objets horloge
  - 9 fichiers modifiés, 594 lignes ajoutées, 41 lignes supprimées
- `32b99a1` - Implémentation de la phase 1 - Fonctionnalité de chat de base
  - 19 fichiers modifiés, 1185 lignes ajoutées
- `358e368` - Commit initial : documentation du projet et licence
  - 10 fichiers modifiés, 1873 lignes ajoutées
