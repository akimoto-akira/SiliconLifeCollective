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
- **SiliconLife.Fast** : Version principale de production. Application de bureau multiplateforme (Windows / macOS / Linux), stockage en mémoire SpeedyPack + persistance asynchrone, optimisation approfondie des performances.

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
- Utilisé d'octobre 2025 à avril 2026.
- IDE IA avec génération intelligente de code et gestion de projet.

#### Qoder (Alibaba)
- Utilisé pour la maintenance du projet depuis le 18 avril 2026.
- Plateforme de codage IA supportant l'analyse de code, la génération de documentation et la collaboration multi-agent.

#### CatPaw (Meituan)
- Utilisé en combinaison avec Qoder depuis le 6 mai 2026.
- Basé sur les modèles LongCat développés en interne par Meituan, avec de puissantes capacités de refactorisation complète de l'architecture du code.

### Documentation des exigences

- La documentation des exigences de ce projet n'est pas publique.
- Les exigences ont été validées de manière itérative par plus de 12 plateformes IA internationales et grandes séries de modèles, produisant plus de 2000 lignes de documentation d'exigences pilotée par les user stories, presque incompréhensible pour les humains.

---

## [Non publié]

### 2026-05-22

#### Corrections de cohérence de la documentation
- `9e07b27` - Corriger les incohérences de la documentation française (fr-FR) avec le code source (ref task-307)
  - 10 fichiers modifiés

- `9e3be72` - Corriger les incohérences de la documentation allemande (de-DE) avec le code source (ref task-308)
  - 5 fichiers modifiés

- `2bc7151` - Corriger les incohérences de la documentation espagnole (es-ES) avec le code source (ref task-309)
  - 13 fichiers modifiés

- `f95088e` - Corriger les incohérences de la documentation italienne (it-IT) avec le code source (ref task-310)
  - 11 fichiers modifiés

- `6ea9f4a` - Corriger les incohérences de la documentation polonaise (pl-PL) avec le code source (ref task-311)
  - 16 fichiers modifiés

- `7646923` - Corriger les incohérences de la documentation portugaise (pt-PT) avec le code source (ref task-312)
  - 12 fichiers modifiés

- `7eaf9db` - Corriger les incohérences de la documentation tchèque (cs-CZ) avec le code source (ref task-313)
  - 12 fichiers modifiés

#### Framework de collaboration
- `3cb7347` - Mise à jour task-313 relatedCommit=7eaf9db
  - 1 fichiers modifiés

### 2026-05-21

#### Nouvelles fonctionnalités
- `99eca78` - Ajout de « Voir le stockage (lecture seule) » au menu contextuel, appel Speedy.Manager intra-processus (ref task-301)
  - 26 fichiers modifiés

#### Corrections de cohérence de la documentation
- `7f65cf1` - Corriger les incohérences de la documentation zh-CN avec le code source (ref task-303)
  - 15 fichiers modifiés

- `a9e2a2c` - Corriger les incohérences de la documentation anglaise (en) avec le code source (ref task-302)
  - 9 fichiers modifiés

- `2549105` - Corriger les incohérences de la documentation chinois traditionnel (zh-HK) avec le code source (ref task-304)
  - 12 fichiers modifiés

- `277eb50` - Corriger les incohérences de la documentation japonaise avec le code source (ref task-305)
  - 10 fichiers modifiés

- `edce413` - Corriger les incohérences de la documentation coréenne (ko-KR) avec le code source (ref task-306)
  - 18 fichiers modifiés

- `f2adcae` - Corriger les incohérences de la documentation portugaise avec le code source (ref task-220)
  - 15 fichiers modifiés

- `3332987` - Corriger les incohérences de la documentation chinois traditionnel (Hong Kong) avec le code source (ref task-218)
  - 14 fichiers modifiés

- `af9f715` - Corriger les incohérences de la documentation polonaise avec le code source (ref task-217)
  - 15 fichiers modifiés

- `2e2b18b` - Corriger les incohérences de la documentation coréenne avec le code source (ref task-216)
  - 16 fichiers modifiés

- `626ebc9` - Corriger les incohérences de la documentation japonaise avec le code source (ref task-215)
  - 19 fichiers modifiés

- `48d061b` - Corriger les incohérences de la documentation italienne avec le code source (ref task-214)
  - 14 fichiers modifiés

#### Framework de collaboration
- `6683bee` - Enregistrer l'équipe Marvis AI, mettre à jour le statut des tâches
  - 3 fichiers modifiés

- `03fc905` - Archiver task-210~220
  - 5 fichiers modifiés

### 2026-05-20

#### Nouvelles fonctionnalités
- `65176d4` - Ajout du support complet de localisation portugaise (pt-PT + pt-BR) (ref task-208)
  - 41 fichiers modifiés

#### Corrections de cohérence de la documentation
- `af4dffd` - Corriger toutes les incohérences de la documentation zh-CN avec le code source (ref task-209)
  - 11 fichiers modifiés

- `144b945` - Corriger les incohérences de la documentation anglaise (en) et tchèque (cs-CZ) avec le code source (ref task-219, task-210)
  - 22 fichiers modifiés

- `08bec55` - Corriger les incohérences de la documentation allemande (de-DE) avec le code source (ref task-211)
  - 14 fichiers modifiés

- `7ff28de` - Corriger les incohérences de la documentation espagnole (es-ES) avec le code source (ref task-212)
  - 14 fichiers modifiés

- `15e2133` - Corriger les incohérences de la documentation française (fr-FR) avec le code source (ref task-213)
  - 13 fichiers modifiés

#### Corrections de bugs
- `7dac388` - Corriger l'impossibilité d'afficher la liste des tâches du projet (ref task-207)
  - 6 fichiers modifiés

#### Framework de collaboration
- `7890223` - Archiver task-201~209, publier les tâches de correction de cohérence documentation task-210~220
  - 5 fichiers modifiés

### 2026-05-19

#### Nouvelles fonctionnalités
- `cd72846` - Implémenter une alternative sécurisée au contournement du scan de sécurité PluginLoader (ref task-203)
  - 13 fichiers modifiés

- `fc0c00c` - Améliorations Speedy.Manager - Création/Import/Export/Hiérarchie TreeView/Fenêtre de progression (ref task-206)
  - 9 fichiers modifiés

#### Corrections de bugs
- `ec07118` - Corriger le problème ITypeRegistry/IObjectFactory non enregistrés avant le chargement des plugins (ref task-205)
  - 8 fichiers modifiés

- `9e749db` - Corriger l'erreur Creator ID is required lors de la création de projet (ref task-204)
  - 4 fichiers modifiés

#### Infrastructure
- `43dc092` - Migration CLDR - ajout de CldrDataProvider, suppression de .github
  - 1 fichiers modifiés

- `c09ec1f` - Ajouter cldr/ au .gitignore
  - 1 fichiers modifiés

- `221f818` - Synchronisation GitHub vers schéma de miroir push Gitee, workflow conservé uniquement comme backup manuel
  - 1 fichiers modifiés

- `08cdf1a` - Corriger le workflow de synchronisation GitHub - ajout de logique de retry et saut si aucun changement
  - 1 fichiers modifiés

- `fb4e77d` - Mise à jour de SiliconLife.Speedy.Manager.csproj
  - 1 fichiers modifiés

#### Framework de collaboration
- `df90af0` - Mise à jour task-203 relatedCommit=cd72846
  - 1 fichiers modifiés

### 2026-05-18

#### Refactorisation
- `e720d06` - Refactoriser complètement Speedy.Manager de WinForms vers Avalonia (ref task-202)
  - 17 fichiers modifiés

#### Corrections de bugs
- `08894a9` - Corriger l'erreur d'affichage du niveau des entrées de résumé de la timeline mémoire (ref task-201)
  - 3 fichiers modifiés

#### Framework de collaboration
- `2871afb` - Archiver toutes les tâches, vider tasks.json
  - 2 fichiers modifiés

### 2026-05-17

#### Nouvelles fonctionnalités
- `d6eb994` - Ajouter l'entrée de création de projet et la sélection de modèle de workflow à la page de liste de projets (ref task-203)
  - 14 fichiers modifiés

- `0872134` - Orchestration pilotée par le curator ThinkOnProject pour les projets sans modèle (ref task-202)
  - 6 fichiers modifiés

- `cb3188e` - Visualisation des @mentions en chat de groupe (ref task-208)
  - 4 fichiers modifiés

- `f9968e5` - Déclaration de capacité ToolCall du client IA et dégradation gracieuse (ref task-205)
  - 4 fichiers modifiés

- `0d2b843` - Logique de décision de chat de groupe ShouldReplyInGroupChat (ref task-201)
  - 6 fichiers modifiés

- `277a2b1` - Complétion du réseau de connaissances - requêtes avancées et traversée de graphe (ref task-207)
  - 9 fichiers modifiés

#### Corrections de bugs
- `6d0b66e` - Corriger l'erreur appendMessage TypeError lors de l'envoi de messages en chat de groupe (ref task-209)
  - 5 fichiers modifiés

- `b15167c` - Soumission manquante de l'enregistrement de route list-workflow-templates de task-203 (ref task-203)
  - 1 fichiers modifiés

- `dc549a2` - Corriger le workflow de synchronisation Gitee - ajout du nom d'utilisateur à l'URL du token
  - 1 fichiers modifiés

#### Infrastructure
- `e5fa3ad` - Désactiver la synchronisation automatique GitHub schedule, en attente de la solution officielle Gitee
  - 1 fichiers modifiés

#### Framework de collaboration
- `4a58c82` - Ajout du rapport d'analyse des capacités système + proposition de design ThinkOnProject
  - 5 fichiers modifiés

- `8ab29e6` - Archivage du rapport d'analyse de complétude des capacités système dans .ai-collab/docs
  - 2 fichiers modifiés

- `b412d9c` - Archiver les anciennes tâches, republier task-201~208 basé sur l'analyse complète
  - 2 fichiers modifiés

- `437884a` - Mise à jour des métadonnées de collaboration - task-202/203/204 terminées (ref task-202, task-203, task-204)
  - 2 fichiers modifiés

- `bf78d79` - Mise à jour des métadonnées de collaboration - task-201/205/208 terminées
  - 2 fichiers modifiés

- `de6ee0e` - Enregistrement de fin de session catpaw-20260517-2215
  - 5 fichiers modifiés

- `7223b6f` - Enregistrement de fin de session catpaw-20260517-2200
  - 4 fichiers modifiés


## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### Préparation de la version
- `476d839` - Tâches de version alpha-0.2 ajoutées
  - Création de task-114 (rédaction du CHANGELOG) et task-115 (mise à jour du numéro de version)
  - 1 fichier modifié

### 2026-05-15

#### Infrastructure
- `672627b` - Workflow de synchronisation Gitee ajouté (avec configuration des permissions)
  - Permissions du workflow sync-from-gitee.yml mises à jour
  - 1 fichier modifié, 7 ajouts(+), 4 suppressions(-)

- `3cd5256` - Synchronisation automatique Gitee via GitHub Actions ajoutée
  - Workflow sync-from-gitee.yml ajouté
  - 1 fichier modifié, 50 ajouts(+)

#### Mises à jour de la documentation
- `aa1d2ad` - Mise à jour des README/architecture/premiers-pas dans les 11 langues, reflétant le support multiplateforme de SiliconLife.Fast (ref task-112, task-113)
  - Correction de la documentation décrivant SiliconLife.Fast comme Windows uniquement, reflétant le support multiplateforme réel (Windows / macOS / Linux)
  - Mise à jour de README.md, architecture.md, getting-started.md dans les 11 langues
  - Ajout du support de propriété hint à SelectComponent
  - ConfigView transmet désormais le paramètre hint aux listes déroulantes d'énumération
  - Ajout de la clé SelectSearchHint aux localisations des 11 langues
  - 53 fichiers modifiés, 690 ajouts(+), 194 suppressions(-)

#### Système de tâches
- `3329f3d` - Mécanisme d'inspection du système de tâches + tâches de correction de bugs de localisation ajoutées
  - Création de task-113 : corriger le problème de localisation de la page d'informations
  - Mise à jour de task-112 : mettre à jour la documentation de la version Fast pour le support Linux
  - Archivage des tâches terminées (11) dans .ai-collab/archive/
  - Mécanisme d'inspection configuré : inspection rapide (toutes les 30 min) + inspection complète (quotidienne 06:00)
  - 2 fichiers modifiés, 148 ajouts(+), 171 suppressions(-)

#### Cadre de collaboration
- `6038e22` - coze-agent enregistré dans le registre .ai-collab
  - Ajout des informations d'enregistrement de l'AI résidente de la plateforme Coze
  - 1 fichier modifié

### 2026-05-14

#### Cadre de collaboration AI
- `7344fbb` - Mode handoff supprimé, passage à l'approche basée sur la liste de tâches (v2.0)
  - Restructuration du répertoire .ai-collab du mode handoff à l'approche basée sur la liste de tâches
  - Ajout du fichier principal de liste de tâches tasks.json
  - Ajout du journal d'opérations activity.log
  - Ajout des répertoires changes/ et sessions/

- `589a48e` - Enregistrements de session .ai-collab ajoutés
  - Ajout des enregistrements d'état de session de collaboration AI

- `5481bcf` - Qoder AI IDE enregistré dans le registre de collaboration
  - Ajout des informations d'enregistrement de l'assistant de codage Qoder AI

- `e2d7b61` - relatedCommit et changes commitHash de tasks.json complétés
  - Associations de métadonnées de tâches finalisées

- `a087f0c` - Toutes les tâches task-101~110 acceptées
  - Confirmation que les 10 corrections de tâches sont terminées

#### Corrections de bugs
- `fac9435` - Toutes les corrections et implémentations de task-101~110 terminées
  - Correction du texte d'indice manquant dans le composant de sélection de recherche
  - Correction des problèmes de localisation sur la page d'informations
  - Correction de l'erreur JS de recherche dans le système d'aide
  - 39 fichiers modifiés, 684 ajouts(+), 121 suppressions(-)

- `c46dfbc` - Toutes les tâches en attente terminées (task-001~006)
  - 6 tâches en attente initiales terminées

- `ec176b2` - Liste de tâches écrasée - revue de code a trouvé 10 nouveaux bugs
  - Création de task-101~110 (10 nouvelles tâches)

#### Refactoring
- `ab15915` - En-têtes de copyright unifiés + HelpController BOM et HelpView recherche JS corrigés
  - En-têtes de copyright Apache 2.0 unifiés dans tous les fichiers sources C#
  - Correction du problème d'encodage BOM de HelpController
  - Correction de l'erreur JavaScript de recherche de HelpView

#### Nouvelles fonctionnalités
- `18a6f5d` - Serveur de capacité de navigateur MCP créé (ref task-111)
  - Ajout du projet SiliconLife.McpServer
  - Implémentation du serveur MCP d'automatisation de navigateur Playwright

- `9eb251a` - Module SiliconLife.McpServer supprimé (ref task-111)
  - Serveur MCP autonome supprimé, fonctionnalité intégrée au projet principal

### 2026-05-13

#### Localisation
- `7a62590` - Support de localisation polonaise ajouté
  - Ajout de l'implémentation de localisation polonaise pl-PL (PlPL.cs, 1089 lignes)
  - Ajout de la localisation de documentation d'aide polonaise (HelpLocalizationPlPL.cs, 3972 lignes)
  - Ajout du support de calendrier historique chinois polonais (ChineseHistoricalPlPL.cs, 600 lignes)
  - Ajout de la localisation de barre d'état polonaise (TrayPlPL.cs, 135 lignes)
  - Ajout du jeu de documentation polonaise complet (15 documents)
  - Énumération Language étendue avec le polonais
  - 35 fichiers modifiés, 14379 ajouts(+), 11 suppressions(-)

- `51f9c8e` - Références Ark AI et améliorations de terminologie dans la documentation mises à jour
  - Terminologie du client AI mise à jour dans la documentation multilingue

- `7587c12` - Entrées de journal des modifications ajoutées pour toutes les langues
  - Mises à jour du journal des modifications synchronisées dans toutes les versions linguistiques

#### Migration du système de fenêtres
- `b49a07d` - Migration vers le mode résident de fenêtre Avalonia
  - Suppression de la dépendance Windows Forms, migration complète vers le framework Avalonia UI
  - Fenêtre d'état s'affiche correctement sous Linux (vérifié par bureau à distance)
  - Contrôles de fenêtre ajoutés : menu contextuel, double-clic pour ouvrir le Web, bouton de fermeture
  - Ajout du cadre de collaboration multi-AI (.ai-collab/)
  - Correction de l'initialisation de l'icône de barre d'état (dégradation gracieuse)
  - Ajout de App.axaml et App.cs comme points d'entrée de l'application Avalonia
  - 13 fichiers modifiés, 1442 ajouts(+), 541 suppressions(-)

- `d335aaf` - Fenêtre toujours visible sur plateforme Linux + dialogue de confirmation de fermeture
  - Linux affiche automatiquement la fenêtre d'état (sans icône de barre d'état)
  - Linux affiche un dialogue de confirmation lors de la fermeture de la fenêtre
  - Windows/macOS conservent le comportement de barre d'état original
  - Paramètre --no-tray supporté pour désactiver forcerment la barre d'état
  - Ajout de la méthode ShowMessageBoxAsync pour les dialogues de confirmation
  - 3 fichiers modifiés, 206 ajouts(+), 29 suppressions(-)

#### Refactoring du système de barre d'état
- `841d384` - Système de barre d'état refactorisé et cadre de collaboration AI initialisé
  - TrayLocalizationBase simplifié, propriétés inutilisées supprimées
  - Ajout de l'élément de localisation ShowStatus
  - App.cs : clic sur l'icône de barre d'état affiche la fenêtre d'état, éléments de menu localisés ajoutés
  - Program.cs : initialisation de l'icône de barre d'état déplacée vers StartAsync
  - TrayStatusWindow se masque au lieu de se fermer lors de la fermeture
  - Enregistrement de trae-glm5 et catpaw dans le cadre .ai-collab
  - Mise à jour de .gitignore pour assurer le suivi de tous les fichiers .ai-collab
  - 22 fichiers modifiés, 178 ajouts(+), 1226 suppressions(-)

#### Documentation
- `43653bc` - Description du dépôt et registre AI mis à jour
  - README du projet et informations d'enregistrement .ai-collab mis à jour

### 2026-05-12

#### Vues Web du système de tâches
- `0891b3c` - Ajouter les vues de détail et d'historique d'exécution des tâches
  - Ajout de TaskExecutionDetailView vue de détail d'exécution des tâches
  - Ajout de TaskExecutionHistoryView vue d'historique d'exécution des tâches
  - TaskController ajout des interfaces de requête de détail et d'historique d'exécution
  - Ajout de TaskViewModel modèle de vue des tâches
  - TaskCenter centre de tâches amélioré
  - TaskSystem système de tâches mis à jour
  - 9 langues de localisation ajouté clés liées aux tâches
  - 26 fichiers modifiés, 803 insertions(+), 55 suppressions(-)

### 2026-05-11

#### Refactorisation de l'architecture des composants Web
- `5e687ad` - Migrer le rendu des composants de chaîne vers H-tree
  - ComponentBase méthode de rendu migrée du modèle de chaîne vers la structure H-tree
  - Les 28 composants adaptés à la nouvelle architecture de rendu (A, Accordion, Button, Calendar, Card, Chart, etc.)
  - SelectComponent refactorisation majeure (889 lignes améliorées)
  - Contrôleurs et vues mis à jour en conséquence
  - 33 fichiers modifiés, 667 insertions(+), 435 suppressions(-)

- `bfd332d` - Migrer Style de chaîne vers les styles en ligne CssBuilder
  - Ajout de CssBuilder constructeur de styles
  - ComponentBase système de styles migré de chaîne vers CssBuilder structuré
  - LoadingComponent considérablement amélioré (103 lignes ajoutées)
  - ConfigController, LogController, MemoryController migration des styles des contrôleurs
  - ChatView, ConfigView, LogView, MemoryView migration des styles des vues
  - 37 fichiers modifiés, 351 insertions(+), 157 suppressions(-)

#### Optimisation du système de stockage
- `d67a7ee` - Optimiser QueryLatest pour les grands jeux de données
  - SpeedyTimeStorage QueryLatest optimisation des performances de la méthode
  - SpeedyLoggerProvider fournisseur de journalisation amélioré
  - 2 fichiers modifiés, 44 insertions(+), 5 suppressions(-)

#### Refactorisation du système de calendrier
- `9629f88` - Extraire TimerExecution et améliorer les vues Web du minuteur
  - TimerSystem logique TimerExecution extraite (175 lignes supprimées)
  - SelectComponent considérablement amélioré (427 lignes améliorées)
  - TimerController et vues du minuteur améliorées
  - ContextManager gestionnaire de contexte mis à jour
  - 12 fichiers modifiés, 458 insertions(+), 267 suppressions(-)

#### Localisation
- `5d8ca79` - Ajouter la clé de localisation LogsLoading
  - 9 langues ajouté clé LogsLoading
  - DefaultLocalizationBase classe de base ajouté définition
  - 11 fichiers modifiés, 15 insertions(+)

### 2026-05-10

#### Refactorisation du système de tâches
- `54394f6` - Fusionner le système de tâches avec les cycles d'historique de chat
  - ProjectTaskSystem système de tâches du projet considérablement simplifié (411 lignes refactorisées)
  - TaskSystem système de tâches simplifié (254 lignes refactorisées)
  - TaskCenter centre de tâches refactorisé (188 lignes améliorées)
  - ContextManager gestionnaire de contexte optimisé (347 lignes refactorisées)
  - DefaultSiliconBeing être de silicium amélioré
  - TimerSystem système de minuteur intégré avec les tâches
  - IWorkNoteStorage interface mise à jour
  - SpeedyWorkNoteStorage et FileSystemWorkNoteStorage adaptés
  - 16 fichiers modifiés, 648 insertions(+), 897 suppressions(-)

### 2026-05-09

#### Amélioration de l'interface Web
- `bc50dd7` - Améliorer la vue de chat et ajouter la fonctionnalité d'audit
  - Ajout de AuditController contrôleur d'audit (261 lignes)
  - Ajout de AuditView vue d'audit (379 lignes)
  - Ajout de AuditViewModel modèle de vue d'audit
  - ChatView vue de chat considérablement améliorée (171 lignes améliorées)
  - ChatController contrôleur de chat mis à jour
  - MarkdownEditorComponent composant amélioré
  - InitController contrôleur d'initialisation amélioré
  - ChatSystem système de chat fonctionnalités ajoutées
  - 14 fichiers modifiés, 1030 insertions(+), 112 suppressions(-)

- `c9babce` - Améliorer le rendu des appels d'outils dans la vue de chat
  - ChatView rendu du bloc d'appel d'outils amélioré
  - 1 fichier modifié, 54 insertions(+), 11 suppressions(-)

#### Système de scénarios d'outils IA
- `ff2eddd` - Implémenter le système de filtrage de scénarios d'outils
  - Ajout de ToolScenarioAttribute attribut de scénario d'outils (36 lignes)
  - Ajout de ChatOnlyAttribute attribut de scénario chat uniquement (19 lignes)
  - ToolManager gestionnaire d'outils ajouté filtrage de scénarios (40 lignes)
  - ContextManager gestionnaire de contexte adapté au filtrage de scénarios
  - 4 fichiers modifiés, 115 insertions(+), 30 suppressions(-)

- `5709a33` - Ajouter des attributs de scénario aux classes d'outils
  - 24 classes d'outils ajouté annotations d'attribut ToolScenario
  - Including calendrier, chat, configuration, conservateur, base de données, disque, compilation dynamique, etc.
  - 24 fichiers modifiés, 46 insertions(+), 20 suppressions(-)

#### Refactorisation du système de tâches
- `2f19a5f` - Restructurer le système de tâches avec TaskCenter et TaskEnumerator
  - Ajout de TaskCenter centre de tâches (235 lignes)
  - Ajout de TaskEnumerator énumérateur de tâches (297 lignes)
  - TaskSystem système de tâches refactorisé et simplifié
  - DefaultSiliconBeing être de silicium adapté à la nouvelle architecture
  - DefaultSiliconBeingFactory fabrique mise à jour
  - SiliconBeingBase classe de base améliorée
  - 7 fichiers modifiés, 796 insertions(+), 275 suppressions(-)

#### Migration du système de permissions
- `a06ed09` - Migrer le système de MI et de permissions vers le projet App
  - PermissionRequestQueue migré de Default/Fast vers le projet App (443 lignes ajoutées)
  - Supprimé WebUIProvider de la version Default (403 lignes supprimées)
  - Supprimé HelpTool de la version Default (194 lignes supprimées)
  - Supprimé les PermissionRequestQueue en double de Default/Fast
  - Supprimé IMPermissionAskHandler de la version Default
  - PermissionRequestController contrôleur mis à jour
  - 14 fichiers modifiés, 496 insertions(+), 1183 suppressions(-)

#### Optimisation du contexte IA
- `4c8aaff` - Optimiser le gestionnaire de contexte et améliorer le localisateur de services
  - ContextManager gestionnaire de contexte simplifié et optimisé
  - ServiceLocator localisateur de services amélioré (36 lignes ajoutées)
  - ToolManager gestionnaire d'outils amélioré (34 lignes ajoutées)
  - DashScopeClient et VolcengineArkClient clients améliorés
  - Exécuteurs (CommandLine, Disk, Network) mis à jour
  - 8 fichiers modifiés, 116 insertions(+), 98 suppressions(-)

#### Localisation
- `5c5eef7` - Ajouter les clés de localisation d'audit et de tâches
  - DefaultLocalizationBase ajouté 127 lignes de définitions de localisation
  - 9 langues ajouté clés liées à l'audit et aux tâches (26 lignes chacune)
  - 11 fichiers modifiés, 387 insertions(+)

#### Configuration du projet
- `2067db6` - Mettre à jour les configurations du projet et les règles gitignore
  - Règles .gitignore mises à jour
  - DefaultConfigData et Fast DefaultConfigData configuration améliorées
  - SpeedyWorkNoteStorage stockage amélioré
  - SpeedyPack noyau amélioré
  - 5 fichiers modifiés, 32 insertions(+), 6 suppressions(-)

### 2026-05-07

#### Localisation italienne
- `8adc18c` - Ajouter le support de localisation italienne et mettre à jour la documentation multilingue
  - Ajout de la localisation it-IT italienne
  - Ajout de l'implémentation de localisation ItIT (1909 lignes)
  - Ajout de ChineseHistoricalItIT support italien du calendrier historique chinois (586 lignes)
  - Ajout de TrayItIT localisation italienne de la barre d'état (135 lignes)
  - Ajout du jeu de documentation italienne complet (14 documents : README, référence API, architecture, système de calendrier, journal des modifications, guide de contribution, etc.)
  - Mise à jour de l'architecture, du guide de développement, du guide de démarrage, etc. pour toutes les versions linguistiques
  - Language énumération des langues ajouté l'italien
  - 86 fichiers modifiés, 11573 insertions(+), 769 suppressions(-)

#### Synchronisation de la documentation
- `12a5deb` - Mettre à jour la documentation multilingue pour l'architecture, le journal des modifications et le guide de l'être de silicium
  - 8 langues README mis à jour
  - 8 langues documentation d'architecture mise à jour
  - 8 langues journal des modifications mis à jour
  - 8 langues guide de l'être de silicium mis à jour
  - 8 langues référence des outils mise à jour
  - Glossaire restructuré
  - 46 fichiers modifiés, 1697 insertions(+), 442 suppressions(-)

### 2026-05-06

#### Refactorisation massive des modules
- `eeb3be6` - Refactorisation et réorganisation massives des modules
  - Restructuration du projet SiliconLife.App
  - Réorganisation du projet SiliconLife.Fast
  - Réorganisation du projet SiliconLife.Default
  - Réorganisation des modules partagés SiliconLife.Common
  - Réorganisation des modules principaux SiliconLife.Core
  - Réorganisation du moteur de stockage SiliconLife.Speedy
  - Réorganisation des outils de gestion SiliconLife.Speedy.Manager
  - 119 fichiers modifiés, 6926 lignes ajoutées, 3066 lignes supprimées

### 2026-05-04

#### Client IA
- `24d2c86` - Ajout de VolcengineArkClient et remplacement d'Audit par Usage tracking
  - Nouveau client AI VolcengineArkClient Volcengine Ark
  - Prise en charge des modes streaming et non-streaming
  - Contrôle de vitesse double intégré (contrôle propre + limitation du serveur)
  - Compatible avec le protocole API OpenAI
  - Remplacement du système Audit par Usage tracking
  - 24 fichiers modifiés, 802 lignes ajoutées, 21 lignes supprimées

#### Système d'outils
- `f27650a` - Ajout de l'outil de rechargement à chaud pour le redémarrage automatique de Fast
  - Nouvel outil HotReloadTool de rechargement à chaud
  - Prise en charge de la compilation en ligne, de la mise à jour et du redémarrage de SiliconLife.Fast
  - Nouvel programme de mise à jour autonome HotReload.exe
  - Mécanisme de copie sécurisée des fichiers (ne se écrase pas lui-même)
  - Fermeture gracieuse et attente de libération de port
  - 9 fichiers modifiés, 581 lignes ajoutées

#### Localisation
- `6a5aad8` - Mise à jour de tous les fichiers et ajout de la prise en charge de la localisation française
  - Nouvelle localisation fr-FR française
  - Mise à jour de toutes les versions linguistiques
  - Traduction française de la documentation d'aide
  - Traduction française de l'interface
  - 100+ fichiers modifiés

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

### 2026-04-23

#### Localisation des outils
- `192fc6e` - Ajout des noms d'outils manquants pour 5 outils
  - 6 fichiers modifiés, 30 lignes ajoutées

#### Mise à jour de la documentation
- `882c08f` - Mise à jour de tous les fichiers changelog, ajout de l'historique Git complet et suppression des faux numéros de version
  - 45 fichiers modifiés, 8815 lignes ajoutées, 1611 lignes supprimées

#### Amélioration de la page de chat
- `65c157b` - Ajout d'indicateurs de chargement pour les pages de chat et sélection automatique de la session du curator
  - 10 fichiers modifiés, 211 lignes ajoutées, 7 lignes supprimées

#### Fonctionnalité d'historique de chat
- `e483348` - Implémentation de la fonctionnalité de visualisation de l'historique de chat des Silicon Beings
  - Nouveau ChatHistoryController
  - Création de ChatHistoryViewModel
  - Implémentation des pages ChatHistoryListView et ChatHistoryDetailView
  - Ajout des clés de localisation de l'historique de chat (5 langues)
  - 12 fichiers modifiés, 1178 lignes ajoutées

#### Amélioration du contrôle de flux IA
- `30a2d4e` - Amélioration de l'annulation du flux IA, de l'intégration IM et de l'initialisation du core host
  - 11 fichiers modifiés, 387 lignes ajoutées, 12 lignes supprimées

#### File d'attente de messages de chat
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
