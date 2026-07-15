# Journal des modifications

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md)

Tous les changements importants de ce projet seront documentés dans ce fichier.

Le format est basé sur [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
et ce projet adhère à la [Gestion sémantique de version](https://semver.org/spec/v2.0.0.html).

---

## À propos de ce journal des modifications

### Double version du projet

Ce projet propose deux versions d'implémentation :

- **SiliconLife.Default** : Implémentation par défaut, principalement utilisée pour valider la faisabilité de l'architecture. Application console, stockage JSON sur système de fichiers.
- **SiliconLife.Fast** : Version de production recommandée. Application de bureau multiplateforme (Windows / macOS / Linux), stockage en mémoire SpeedyPack + persistance asynchrone, optimisation approfondie des performances.

Les deux versions partagent les mêmes interfaces et fonctionnalités, ne différant que dans l'implémentation du stockage et le mode d'exécution. SiliconLife.Default sert de référence de validation d'architecture, SiliconLife.Fast est la version de production recommandée.

### Origine du projet

- Ce projet a débuté le 20 mars 2026.
- Avant ce projet, une démo de validation a échoué en raison d'une conception architecturale inappropriée, rendant impossible l'intégration avec plusieurs plateformes IA.

### Outils IDE IA utilisés

#### Kiro (Amazon AWS)
- Le projet a été initialement maintenu par Kiro, démarré en mode Spec.
- Kiro est un environnement de développement IA agentic construit par Amazon AWS.
- Basé sur Code OSS (VS Code), prend en charge les paramètres VS Code et les plugins compatibles Open VSX.
- Flux de développement piloté par les spécifications pour le codage IA structuré.

#### Comate AI IDE / 文心快码 (Baidu)
- Occasionnellement utilisé pour la rédaction et le travail documentaire.
- Comate AI IDE est un outil d'environnement de développement natif IA publié par Baidu Wenxin le 23 juin 2025.
- Premier IDE IA multimodal et multi-agents collaboratifs de l'industrie.
- Fonctionnalités incluant la conversion design-vers-code et le codage assisté par IA sur tout le flux.
- Propulsé par le modèle Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Utilisé d'octobre 2025 à avril 2026.
- IDE IA, prenant en charge la génération de code intelligente et la gestion de projet.

#### Qoder (Alibaba)
- Utilisé pour la maintenance du projet depuis le 18 avril 2026.
- Plateforme de codage IA, prenant en charge l'analyse de code, la génération de documentation et la collaboration multi-agents.

#### CatPaw (Meituan)
- Utilisé en combinaison avec Qoder depuis le 6 mai 2026.
- Basé sur les modèles LongCat développés en interne par Meituan, avec de puissantes capacités de refactorisation complète d'architecture de code.

### Document d'exigences

- Le document d'exigences de ce projet n'est pas public.
- Les exigences ont été validées de manière itérative par plus de 12 plateformes IA internationales et grandes séries de modèles, produisant un document d'exigences piloté par les user stories de plus de 2000 lignes, quasi incompréhensible pour un humain.

---

## [Non publié]

### 2026-05-27

#### Nouvelles fonctionnalités
- Implémentation de 7 nouveaux clients IA, portant le total à 13 backends IA pris en charge :
  - **DeepSeekClient** — Service IA DeepSeek, mode thinking activé par défaut, contrôle de l'effort de raisonnement, 1M contexte (`https://api.deepseek.com`)
  - **ZhipuClient (GLM)** — Service IA Zhipu GLM, mode thinking (GLM-5), vision par modèle, modèle gratuit `glm-4-flash`, 1M contexte (`https://open.bigmodel.cn/api/paas/v4`)
  - **ErnieClient (Baidu/Qianfan)** — Service IA Baidu ERNIE, 131K contexte, modèles gratuits `ernie-speed`/`ernie-tiny` (`https://qianfan.baidubce.com/v2`)
  - **HunyuanClient (Tencent)** — Service IA Tencent Hunyuan, double endpoint TokenHub + Legacy, mode thinking, 262K contexte (`https://tokenhub.tencentmaas.com/v1` / `https://api.hunyuan.cloud.tencent.com/v1`)
  - **MiniMaxClient** — Service IA MiniMax, multimodal natif M3 (image + vidéo), mode thinking adaptatif, 1M contexte (`https://api.minimaxi.com/v1`)
  - **MoonshotClient (Kimi)** — Service IA Moonshot Kimi, mode thinking, vision, 262K contexte (`https://api.moonshot.cn/v1`)
  - **SiliconFlowClient** — Service IA SiliconFlow, agrégateur de 100+ modèles open source, liste de modèles dynamique, 1M contexte (`https://api.siliconflow.cn/v1`)
- Marquage de 01.AI (Yi) comme obsolète (⚠️) : inscription des nouveaux utilisateurs arrêtée

#### Documentation
- Mise à jour de la documentation française (fr-FR) pour refléter les 7 nouveaux clients IA

### 2026-05-26

#### Nouvelles fonctionnalités
- `a49041b` - Ajout de la prise en charge de localisation russe (ru-RU) (ref task-364)
  - 216 fichiers modifiés

#### Corrections
- `79096f2` - Format du tableau glossary converti en Markdown standard, suppression des alignements par espaces superflus
  - 1 fichier modifié

#### Documentation
- `174a954` - Ajout des traductions terminologiques manquantes pour les colonnes Deutsch/Polski/Português du glossary
  - 1 fichier modifié

#### Cadre de collaboration
- `5b03d53` - Mise à jour des enregistrements de tâches .ai-collab - task-364 localisation russe (ref task-364)
  - 5 fichiers modifiés

- `018947d` - Archivage des sessions et changes du 2026-05-25
  - 2 fichiers modifiés

### 2026-05-25

#### Nouvelles fonctionnalités
- `14721a9` - Raffinement des prompts de dotation en personnel ThinkOnProject en plan d'action détaillé exécutable (ref task-363)
  - 20 fichiers modifiés

#### Corrections
- `abb4285` - Correction de l'appel .join() mal positionné dans beingsHtml (ref task-361)
  - 1 fichier modifié

- `1c0b9ed` - Suppression du bug de chaîne dupliquée state-initial causé par le rendu states-overview dans WorkflowDetailView (ref task-362)
  - 6 fichiers modifiés

#### Cadre de collaboration
- `ecc48a1` - Mise à jour des métadonnées .ai-collab (relatedCommit et activity log) (ref task-361)
  - 4 fichiers modifiés

- `64529a7` - Archivage des sessions et changes du 2026-05-24 (exécution manuelle différée)
  - 28 fichiers modifiés

- `4150e52` - Archivage des tâches terminées task-341~361 (ref archive)
  - 2 fichiers modifiés

### 2026-05-24

#### Nouvelles fonctionnalités
- `db60fd9` - La liste des autorisations d'outil affiche les outils sans déclaration ToolAction et les marque comme non configurables (ref task-331, task-332, task-333)
  - 21 fichiers modifiés

- `6004a7f` - Ajout de la prise en charge des définitions de rôles dans WorkflowTemplate + localisation en 12 langues + correction DiskTool (ref task-346)
  - 24 fichiers modifiés

- `75ce452` - Pool de rôles ProjectSpace et actions de gestion des rôles ProjectTool (ref task-347)
  - 12 fichiers modifiés

- `edfb600` - Ajout d'informations de rôle dans BuildProjectScenarioContext (ref task-348)
  - 21 fichiers modifiés

- `6a2d713` - Extension de HasProjectsWithoutTemplate en HasProjectsNeedingAttention (ref task-349)
  - 21 fichiers modifiés

- `a773224` - La création de tâches de flux de travail utilise le pool de rôles pour attribuer les exécutants (ref task-350)
  - 6 fichiers modifiés

- `77a27f9` - Extension de TravelCodeWikiTool comme point d'entrée d'entités géographiques (ref task-353)
  - 8 fichiers modifiés

- `873ef23` - Implémentation de GeoDataTool terminée, mise à jour de l'état .ai-collab (ref task-352)
  - 7 fichiers modifiés

- `feaccab` - Implémentation de GeoContentTool terminée, mise à jour de l'état .ai-collab (ref task-351)
  - 6 fichiers modifiés

- `6e60ad1` - Extension de GeoLanguageTool (prise en charge ObjectPath + set_word), rétro-complétion des métadonnées (ref task-356, task-355)
  - 7 fichiers modifiés

- `4eff807` - Implémentation de GetWikiDocuments() par chaque sous-classe GeoLocation (ref task-357)
  - 5 fichiers modifiés

- `baad5df` - Implémentation du service de publication MediaWiki API (ref task-358)
  - 6 fichiers modifiés

- `b846a21` - Implémentation de la page de détails du flux de travail (ref task-361)
  - 24 fichiers modifiés

#### Corrections
- `a290088` - Les Êtres de Silicium créés via CuratorTool sont perdus après redémarrage (ref task-334)
  - 11 fichiers modifiés

- `69a8cba` - Correction du bug de la page de tâches ne filtrant pas par beingId (ref task-360)
  - 8 fichiers modifiés

- `7dd1a65` - Enregistrement de la route de la page de détails du flux de travail dans Router.cs (ref task-361)
  - 1 fichier modifié

#### Refactorisation
- `5e02711` - Refactorisation de l'abstraction des chemins de stockage de la couche commune, élimination du codage en dur du système de fichiers (ref task-335)
  - 12 fichiers modifiés

- `0ec0929` - DynamicBeingLoader.SaveBeingCode utilise IStorage au lieu des opérations directes sur le système de fichiers (ref task-336)
  - 7 fichiers modifiés

- `9a44b48` - Pont IStorage PlaywrightWebView + découplage de la classe de base WebViewBrowserTool (ref task-337, task-340)
  - 11 fichiers modifiés

- `8fea742` - Sauvegarde des captures d'écran WebViewBrowserTool utilisant IStorage au lieu des opérations directes sur le système de fichiers (ref task-338)
  - 6 fichiers modifiés

- `4c24e6d` - DefaultPermissionCallback utilise BeingPathResolver au lieu de chemins codés en dur (ref task-339)
  - 6 fichiers modifiés

- `ab428cd` - Suppression du downcast DefaultSiliconBeing, appel direct de la méthode SaveState() de la classe de base (ref task-344)
  - 7 fichiers modifiés

- `1e6eb80` - Le pont des fichiers temporaires d'état du navigateur PlaywrightWebView utilise désormais IStorage pour la lecture/écriture directe (ref task-341)
  - 7 fichiers modifiés

- `17f00e9` - Les opérations de recherche de DiskTool passent par DiskExecutor (ref task-342)
  - 8 fichiers modifiés

- `8158703` - La vérification des pièces jointes de ChatController passe par DiskExecutor (ref task-343)
  - 7 fichiers modifiés

- `3243ae6` - Réécriture de TravelCodeWikiPublishWorkflow en machine à états à 7 étapes, suppression du fichier TravelCodeWikiWithAI de suivi forcé (ref task-355)
  - 6 fichiers modifiés

#### Nettoyage
- `d685288` - Suppression de HotReloadTool.cs et du répertoire tools/HotReload (ref task-345)
  - 8 fichiers modifiés

#### Documentation
- `f1789d1` - Optimisation de la ligne de description README.md (ref task-359)
  - 9 fichiers modifiés

#### Cadre de collaboration
- `982c6bb` - Ajout des champs relatedCommit et commitHash manquants dans .ai-collab
  - 6 fichiers modifiés

- `d91e9f8` - Archivage task-331~340, tableau de tâches vidé
  - 2 fichiers modifiés

- `9135e30` - Publication task-341~344 refactorisation IStorage de la couche commune + correction d'abstraction
  - 1 fichier modifié

- `f70b350` - Ajout de 13 tâches de restructuration architecturale TravelCodeWikiWithAI (ref task-346~358)
  - 2 fichiers modifiés

- `f81d38b` - Mise à jour des fichiers de session et de suivi de tâches ai-collab
  - 3 fichiers modifiés

### 2026-05-23

#### Corrections
- `9c3c64e` - Correction du contournement de la vérification des autorisations d'exécution ExecuteTool contournant les restrictions au niveau du projet (ref task-324)
  - 7 fichiers modifiés

- `94a9e35` - Correction de l'incohérence entre la définition du modèle d'autorisations et la déclaration ToolActionAttribute (ref task-325)
  - 6 fichiers modifiés

- `e8d8371` - Les outils dont toutes les Actions sont désactivées sont entièrement retirés des requêtes IA (ref task-326)
  - 6 fichiers modifiés

- `32c7d8a` - Ajout de la vérification du nom d'Action dans l'API des autorisations d'outil + correction du rendu Markdown de l'historique de chat (ref task-327, task-328, task-329)
  - 9 fichiers modifiés

- `797db8c` - Le fallback de rendu Markdown définit incorrectement mdRendered, empêchant marked de re-rendre après le chargement (ref task-330)
  - 9 fichiers modifiés

#### Cadre de collaboration
- `1496094` - Publication des tâches de correction du cadre d'autorisation d'outils task-324~327
  - 776 fichiers modifiés

- `0d16e63` - Mise à jour de l'état des tâches de collaboration, association de task-330 au commit 797db8c, préparation de l'archivage
  - 2 fichiers modifiés

- `e602e1c` - Archivage task-316~330, tableau de tâches vidé (ref task-316~330)
  - 2 fichiers modifiés

- `20291ce` - Archivage quotidien des sessions et changes (13-22 mai)
  - 106 fichiers modifiés

### 2026-05-22

#### Corrections de cohérence de la documentation
- `9e07b27` - Correction des incohérences entre la documentation française (fr-FR) et le code source (ref task-307)
  - 10 fichiers modifiés

- `9e3be72` - Correction de la cohérence entre la documentation allemande (de-DE) et le code source (ref task-308)
  - 5 fichiers modifiés

- `2bc7151` - Correction des incohérences entre la documentation espagnole (es-ES) et le code source (ref task-309)
  - 13 fichiers modifiés

- `f95088e` - Correction de la cohérence entre la documentation italienne (it-IT) et le code source (ref task-310)
  - 11 fichiers modifiés

- `6ea9f4a` - Correction de la cohérence entre la documentation polonaise (pl-PL) et le code source (ref task-311)
  - 16 fichiers modifiés

- `7646923` - Correction de la cohérence entre la documentation portugaise (pt-PT) et le code source (ref task-312)
  - 12 fichiers modifiés

- `7eaf9db` - Correction de la cohérence entre la documentation tchèque (cs-CZ) et le code source (ref task-313)
  - 12 fichiers modifiés

#### Cadre de collaboration
- `3cb7347` - Mise à jour task-313 relatedCommit=7eaf9db
  - 1 fichier modifié

### 2026-05-21

#### Nouvelles fonctionnalités
- `99eca78` - Ajout de la fonctionnalité « Voir le stockage (lecture seule) » dans le menu contextuel, appel in-process à Speedy.Manager (ref task-301)
  - 26 fichiers modifiés

#### Corrections de cohérence de la documentation
- `7f65cf1` - Correction des incohérences entre la documentation zh-CN et le code source (ref task-303)
  - 15 fichiers modifiés

- `a9e2a2c` - Correction des incohérences entre la documentation anglaise (en) et le code source (ref task-302)
  - 9 fichiers modifiés

- `2549105` - Correction des incohérences entre la documentation chinois traditionnel (zh-HK) et le code source (ref task-304)
  - 12 fichiers modifiés

- `277eb50` - Correction des incohérences entre la documentation japonaise et le code source (ref task-305)
  - 10 fichiers modifiés

- `edce413` - Correction des incohérences entre la documentation coréenne (ko-KR) et le code source (ref task-306)
  - 18 fichiers modifiés

- `f2adcae` - Correction de l'incohérence entre la documentation portugaise et le code source (ref task-220)
  - 15 fichiers modifiés

- `3332987` - Correction de l'incohérence entre la documentation chinois traditionnel (Hong Kong) et le code source (ref task-218)
  - 14 fichiers modifiés

- `af9f715` - Correction de l'incohérence entre la documentation polonaise et le code source (ref task-217)
  - 15 fichiers modifiés

- `2e2b18b` - Correction de l'incohérence entre la documentation coréenne et le code source (ref task-216)
  - 16 fichiers modifiés

- `626ebc9` - Correction de l'incohérence entre la documentation japonaise et le code source (ref task-215)
  - 19 fichiers modifiés

- `48d061b` - Correction de l'incohérence entre la documentation italienne et le code source (ref task-214)
  - 14 fichiers modifiés

#### Cadre de collaboration
- `6683bee` - Enregistrement de l'équipe Marvis AI, mise à jour de l'état des tâches
  - 3 fichiers modifiés

- `03fc905` - Archivage task-210~220
  - 5 fichiers modifiés

### 2026-05-20

#### Nouvelles fonctionnalités
- `65176d4` - Ajout de la prise en charge complète de localisation portugaise (pt-PT + pt-BR) (ref task-208)
  - 41 fichiers modifiés

#### Corrections de cohérence de la documentation
- `af4dffd` - Correction de toutes les incohérences entre la documentation zh-CN et le code source (ref task-209)
  - 11 fichiers modifiés

- `144b945` - Correction des incohérences entre la documentation anglaise (en) et tchèque (cs-CZ) et le code source (ref task-219, task-210)
  - 22 fichiers modifiés

- `08bec55` - Correction de l'incohérence entre la documentation allemande (de-DE) et le code source (ref task-211)
  - 14 fichiers modifiés

- `7ff28de` - Correction de l'incohérence entre la documentation espagnole (es-ES) et le code source (ref task-212)
  - 14 fichiers modifiés

- `15e2133` - Correction de l'incohérence entre la documentation française (fr-FR) et le code source (ref task-213)
  - 13 fichiers modifiés

#### Corrections
- `7dac388` - Correction de l'impossibilité d'afficher la liste des tâches du projet (ref task-207)
  - 6 fichiers modifiés

#### Cadre de collaboration
- `7890223` - Archivage task-201~209, publication des tâches de correction de cohérence documentaire task-210~220
  - 5 fichiers modifiés

### 2026-05-19

#### Nouvelles fonctionnalités
- `cd72846` - Implémentation d'une alternative sécurisée au contournement de l'analyse de sécurité PluginLoader (ref task-203)
  - 13 fichiers modifiés

- `fc0c00c` - Amélioration de Speedy.Manager - Création/Import/Export/TreeView hiérarchique/Fenêtre de progression (ref task-206)
  - 9 fichiers modifiés

#### Corrections
- `ec07118` - Correction du problème ITypeRegistry/IObjectFactory non enregistrés avant le chargement des plugins (ref task-205)
  - 8 fichiers modifiés

- `9e749db` - Correction de l'erreur « Creator ID is required » lors de la création d'un projet (ref task-204)
  - 4 fichiers modifiés

#### Infrastructure
- `43dc092` - Migration CLDR - Ajout de CldrDataProvider, suppression de .github
  - 1 fichier modifié

- `c09ec1f` - Ajout de cldr/ à .gitignore
  - 1 fichier modifié

- `221f818` - Synchronisation GitHub passant à un schéma de miroir push Gitee, workflow conservé uniquement comme backup manuel
  - 1 fichier modifié

- `08cdf1a` - Correction du workflow de synchronisation GitHub - Ajout de logique de retry et saut si aucun changement
  - 1 fichier modifié

- `fb4e77d` - Mise à jour de SiliconLife.Speedy.Manager.csproj
  - 1 fichier modifié

#### Cadre de collaboration
- `df90af0` - Mise à jour task-203 relatedCommit=cd72846
  - 1 fichier modifié

### 2026-05-18

#### Refactorisation
- `e720d06` - Transformation complète de Speedy.Manager de WinForms vers Avalonia (ref task-202)
  - 17 fichiers modifiés

#### Corrections
- `08894a9` - Correction de l'affichage incorrect du niveau des entrées de résumé dans la chronologie de la mémoire (ref task-201)
  - 3 fichiers modifiés

#### Cadre de collaboration
- `2871afb` - Archivage de toutes les tâches, tasks.json vidé
  - 2 fichiers modifiés

### 2026-05-17

#### Nouvelles fonctionnalités
- `d6eb994` - Ajout d'une entrée de création de projet et sélection de modèle de flux de travail sur la page de liste des projets (ref task-203)
  - 14 fichiers modifiés

- `0872134` - Orchestration pilotée par le curateur pour les projets sans modèle ThinkOnProject (ref task-202)
  - 6 fichiers modifiés

- `cb3188e` - Visualisation des mentions @ dans le chat de groupe (ref task-208)
  - 4 fichiers modifiés

- `f9968e5` - Déclaration de capacité ToolCall du client IA et dégradation gracieuse (ref task-205)
  - 4 fichiers modifiés

- `0d2b843` - Logique de décision du chat de groupe ShouldReplyInGroupChat (ref task-201)
  - 6 fichiers modifiés

- `277a2b1` - Complétion du Réseau de Connaissances - Requêtes avancées et parcours de graphe (ref task-207)
  - 9 fichiers modifiés

#### Corrections
- `6d0b66e` - Correction du TypeError appendMessage lors de l'envoi de messages dans le chat de groupe (ref task-209)
  - 5 fichiers modifiés

- `b15167c` - Soumission complémentaire de l'enregistrement de route list-workflow-templates manquant dans task-203 (ref task-203)
  - 1 fichier modifié

- `dc549a2` - Correction du workflow de synchronisation Gitee - Ajout du nom d'utilisateur dans l'URL du token
  - 1 fichier modifié

#### Infrastructure
- `e5fa3ad` - Désactivation de la planification de synchronisation automatique GitHub, en attente de la solution officielle de synchronisation Gitee
  - 1 fichier modifié

#### Cadre de collaboration
- `4a58c82` - Ajout d'un rapport d'analyse des capacités système + plan de conception ThinkOnProject
  - 5 fichiers modifiés

- `8ab29e6` - Archivage du rapport d'analyse de complétude des capacités système dans .ai-collab/docs
  - 2 fichiers modifiés

- `b412d9c` - Archivage des anciennes tâches, republication task-201~208 basée sur l'analyse globale
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
- `476d839` - Ajout des tâches de version alpha-0.2
  - Création de task-114 (rédaction du CHANGELOG) et task-115 (mise à jour du numéro de version)
  - 1 fichier modifié

### 2026-05-15

#### Infrastructure
- `672627b` - Ajout du workflow de synchronisation Gitee (avec configuration de permissions)
  - Mise à jour de la configuration des permissions du workflow sync-from-gitee.yml
  - 1 fichier modifié, 7 lignes ajoutées, 4 lignes supprimées

- `3cd5256` - Ajout de GitHub Actions pour la synchronisation automatique du code Gitee
  - Nouveau workflow sync-from-gitee.yml
  - 1 fichier modifié, 50 lignes ajoutées

#### Mise à jour de la documentation
- `aa1d2ad` - Mise à jour des documents README/Architecture/Prise en main pour les 11 langues, reflétant le support multiplateforme de SiliconLife.Fast (ref task-112, task-113)
  - Correction de la description selon laquelle SiliconLife.Fast ne supporte que Windows, reflétant le support multiplateforme réel (Windows / macOS / Linux)
  - Mise à jour de README.md, architecture.md, getting-started.md pour 11 langues
  - Ajout de la prise en charge de l'attribut hint dans SelectComponent
  - ConfigView : passage du hint aux listes déroulantes d'énumération
  - Ajout de la clé SelectSearchHint dans la localisation des 11 langues
  - 53 fichiers modifiés, 690 lignes ajoutées, 194 lignes supprimées

#### Système de tâches
- `3329f3d` - Ajout d'un mécanisme d'inspection du système de tâches + tâches de correction de bugs de localisation
  - Création de task-113 : correction du problème de localisation de la page À propos
  - Mise à jour de task-112 : mise à jour de la documentation de la version Fast pour le support Linux
  - Archivage des tâches terminées (11) dans .ai-collab/archive/
  - Configuration du mécanisme d'inspection terminée : inspection rapide (toutes les 30 minutes) + inspection complète (quotidienne à 06h00)
  - 2 fichiers modifiés, 148 lignes ajoutées, 171 lignes supprimées

#### Cadre de collaboration
- `6038e22` - Enregistrement de coze-agent dans le registre de collaboration .ai-collab
  - Ajout des informations d'enregistrement de l'IA permanente de la plateforme Coze
  - 1 fichier modifié

### 2026-05-14

#### Cadre de collaboration IA
- `7344fbb` - Suppression du mode handoff, passage à un pilotage par liste de tâches (v2.0)
  - Refactorisation de la structure du répertoire .ai-collab, du mode de transfert handoff au pilotage par liste de tâches
  - Nouveau fichier principal tasks.json de la liste de tâches
  - Nouveau journal d'activité activity.log
  - Nouveaux répertoires changes/ et sessions/

- `589a48e` - Ajout d'enregistrements de sessions .ai-collab
  - Nouveaux enregistrements d'état de session de collaboration IA

- `5481bcf` - Enregistrement de Qoder AI IDE dans le registre de collaboration
  - Ajout des informations d'enregistrement de l'assistant de programmation IA Qoder

- `e2d7b61` - Ajout des champs relatedCommit et changes commitHash dans tasks.json
  - Amélioration de l'association des métadonnées de tâches

- `a087f0c` - Validation de toutes les tâches task-101~110
  - Confirmation que les 10 corrections de tâches sont toutes terminées

#### Corrections de bugs
- `fac9435` - Achèvement de toutes les 10 corrections et implémentations de tâches task-101~110
  - Correction du texte d'invite manquant dans le composant de sélection de recherche
  - Correction du problème de localisation de la page À propos
  - Correction de l'erreur JS de recherche dans le système d'aide
  - 39 fichiers modifiés, 684 lignes ajoutées, 121 lignes supprimées

- `c46dfbc` - Achèvement de toutes les tâches en attente (task-001~006)
  - Achèvement des 6 tâches initiales en attente

- `ec176b2` - Remplacement de la liste de tâches - Revue de code découvrant 10 nouveaux bugs
  - Création de 10 nouvelles tâches task-101~110

#### Refactorisation
- `ab15915` - Unification des en-têtes de copyright + correction du BOM HelpController et du JS de recherche HelpView
  - Unification des en-têtes de copyright Apache 2.0 dans tous les fichiers source C#
  - Correction du problème d'encodage BOM de HelpController
  - Correction de l'erreur JavaScript de recherche HelpView

#### Nouvelles fonctionnalités
- `18a6f5d` - Création d'un serveur de capacités de navigateur MCP (ref task-111)
  - Nouveau projet SiliconLife.McpServer
  - Implémentation d'un serveur MCP d'automatisation de navigateur Playwright

- `9eb251a` - Suppression du module SiliconLife.McpServer (ref task-111)
  - Suppression du serveur MCP autonome, fonctionnalité intégrée au projet principal

### 2026-05-13

#### Localisation
- `7a62590` - Ajout de la prise en charge de localisation polonaise
  - Nouvelle implémentation de localisation pl-PL (PlPL.cs, 1089 lignes)
  - Nouvelle localisation de l'aide polonaise (HelpLocalizationPlPL.cs, 3972 lignes)
  - Nouveau support du calendrier historique chinois en polonais (ChineseHistoricalPlPL.cs, 600 lignes)
  - Nouvelle localisation de la barre d'état polonaise (TrayPlPL.cs, 135 lignes)
  - Nouveau jeu complet de documents polonais (15 documents)
  - Ajout du polonais à l'énumération Language
  - 35 fichiers modifiés, 14379 lignes ajoutées, 11 lignes supprimées

- `51f9c8e` - Mise à jour des références IA Ark et améliorations terminologiques dans la documentation
  - Mise à jour de la terminologie des clients IA dans la documentation multilingue

- `7587c12` - Ajout d'entrées de journal des modifications pour toutes les langues
  - Synchronisation de la mise à jour du changelog pour toutes les versions linguistiques

#### Migration du système de fenêtres
- `b49a07d` - Migration vers le mode de résidence de fenêtre Avalonia
  - Suppression de la dépendance Windows Forms, migration complète vers le framework Avalonia UI
  - Affichage normal de la fenêtre d'état sur Linux (vérifié par bureau à distance)
  - Ajout de contrôles de fenêtre : menu contextuel, double-clic pour ouvrir le Web, bouton de fermeture
  - Ajout du cadre de collaboration multi-IA (.ai-collab/)
  - Correction de l'initialisation de l'icône de la barre d'état (dégradation gracieuse)
  - Nouveau point d'entrée d'application Avalonia App.axaml et App.cs
  - 13 fichiers modifiés, 1442 lignes ajoutées, 541 lignes supprimées

- `d335aaf` - Affichage permanent de la fenêtre sur plateforme Linux + dialogue de confirmation de fermeture
  - Affichage automatique de la fenêtre d'état sur Linux (pas d'icône de barre d'état)
  - Dialogue de confirmation lors de la fermeture de la fenêtre sur Linux
  - Maintien du comportement de barre d'état existant sur Windows/macOS
  - Support du paramètre --no-tray pour désactiver forcer la barre d'état
  - Nouvelle méthode ShowMessageBoxAsync pour les dialogues de confirmation
  - 3 fichiers modifiés, 206 lignes ajoutées, 541 lignes supprimées

#### Refactorisation du système de barre d'état
- `841d384` - Refactorisation du système de barre d'état et initialisation du cadre de collaboration IA
  - Simplification de TrayLocalizationBase, suppression des propriétés inutilisées
  - Ajout de l'élément de localisation ShowStatus
  - App.cs : ajout du clic sur l'icône de barre d'état pour afficher la fenêtre d'état, éléments de menu localisés
  - Program.cs : déplacement de l'initialisation de l'icône de barre d'état vers StartAsync
  - TrayStatusWindow : masquer au lieu de quitter lors de la fermeture
  - Enregistrement de trae-glm5 et catpaw dans le cadre de collaboration .ai-collab
  - Mise à jour de .gitignore pour assurer le suivi de tous les fichiers .ai-collab
  - 22 fichiers modifiés, 178 lignes ajoutées, 1226 lignes supprimées

#### Documentation
- `43653bc` - Mise à jour de la description du dépôt et du registre IA
  - Mise à jour du README du projet et des informations d'enregistrement .ai-collab

### 2026-05-12

#### Vues Web du système de tâches
- `0891b3c` - Ajout des vues de détails d'exécution et d'historique des tâches
  - Nouvelle vue TaskExecutionDetailView des détails d'exécution des tâches
  - Nouvelle vue TaskExecutionHistoryView de l'historique d'exécution des tâches
  - TaskController : ajout des interfaces de requête de détails d'exécution et d'historique
  - Nouveau modèle de vue TaskViewModel
  - Amélioration du centre de tâches TaskCenter
  - Mise à jour du système de tâches TaskSystem
  - Ajout des clés liées aux tâches dans la localisation de 9 langues
  - 26 fichiers modifiés, 803 lignes ajoutées, 55 lignes supprimées

### 2026-05-11

#### Refactorisation de l'architecture des composants Web
- `5e687ad` - Migration du rendu des composants de chaîne vers H-tree
  - Méthode de rendu ComponentBase migrée du mode chaîne vers la structure H-tree
  - Adaptation de tous les 28 composants à la nouvelle architecture de rendu (A, Accordion, Button, Calendar, Card, Chart, etc.)
  - Refactorisation majeure de SelectComponent (889 lignes améliorées)
  - Mise à jour synchrone des contrôleurs et vues
  - 33 fichiers modifiés, 667 lignes ajoutées, 435 lignes supprimées

- `bfd332d` - Migration du Style de chaîne vers les styles en ligne CssBuilder
  - Nouveau constructeur de styles CssBuilder
  - Système de styles ComponentBase migré de chaîne vers CssBuilder structuré
  - Amélioration majeure de LoadingComponent (103 lignes ajoutées)
  - Migration des styles des contrôleurs ConfigController, LogController, MemoryController
  - Migration des styles des vues ChatView, ConfigView, LogView, MemoryView
  - 37 fichiers modifiés, 351 lignes ajoutées, 157 lignes supprimées

#### Optimisation du système de stockage
- `d67a7ee` - Optimisation des requêtes QueryLatest sur les grands jeux de données
  - Optimisation des performances de la méthode QueryLatest de SpeedyTimeStorage
  - Amélioration du fournisseur de journaux SpeedyLoggerProvider
  - 2 fichiers modifiés, 44 lignes ajoutées, 5 lignes supprimées

#### Refactorisation du système de calendrier
- `9629f88` - Extraction de TimerExecution et amélioration des vues Web des minuteurs
  - Extraction de la logique TimerExecution de TimerSystem (175 lignes supprimées)
  - Amélioration majeure de SelectComponent (427 lignes améliorées)
  - Amélioration de TimerController et des vues des minuteurs
  - Mise à jour du gestionnaire de contexte ContextManager
  - 12 fichiers modifiés, 458 lignes ajoutées, 267 lignes supprimées

#### Localisation
- `5d8ca79` - Ajout de la clé de localisation LogsLoading
  - Ajout de la clé LogsLoading dans 9 langues
  - Nouvelle définition dans la classe de base DefaultLocalizationBase
  - 11 fichiers modifiés, 15 lignes ajoutées

### 2026-05-10

#### Refactorisation du système de tâches
- `54394f6` - Fusion du système de tâches et des cycles d'historique de chat
  - Simplification majeure du système de tâches de projet ProjectTaskSystem (411 lignes refactorisées)
  - Simplification du système de tâches TaskSystem (254 lignes refactorisées)
  - Refactorisation du centre de tâches TaskCenter (188 lignes améliorées)
  - Optimisation du gestionnaire de contexte ContextManager (347 lignes refactorisées)
  - Amélioration de l'Être de Silicium DefaultSiliconBeing
  - Intégration des tâches dans le système de minuteurs TimerSystem
  - Mise à jour de l'interface IWorkNoteStorage
  - Adaptation de SpeedyWorkNoteStorage et FileSystemWorkNoteStorage
  - 16 fichiers modifiés, 648 lignes ajoutées, 897 lignes supprimées

### 2026-05-09

#### Amélioration de l'interface Web
- `bc50dd7` - Amélioration de la vue de chat et ajout de la fonctionnalité d'audit
  - Nouveau contrôleur d'audit AuditController (261 lignes)
  - Nouvelle vue d'audit AuditView (379 lignes)
  - Nouveau modèle de vue d'audit AuditViewModel
  - Amélioration majeure de la vue de chat ChatView (171 lignes améliorées)
  - Mise à jour du contrôleur de chat ChatController
  - Amélioration du composant MarkdownEditorComponent
  - Amélioration du contrôleur d'initialisation InitController
  - Nouvelles fonctionnalités dans le système de chat ChatSystem
  - 14 fichiers modifiés, 1030 lignes ajoutées, 112 lignes supprimées

- `c9babce` - Amélioration du rendu des appels d'outil dans la vue de chat
  - Amélioration du rendu des blocs d'appels d'outil dans ChatView
  - 1 fichier modifié, 54 lignes ajoutées, 11 lignes supprimées

#### Système de scénarios d'outils IA
- `ff2eddd` - Implémentation du système de filtrage de scénarios d'outils
  - Nouvel attribut de scénario d'outil ToolScenarioAttribute (36 lignes)
  - Nouvel attribut de scénario chat uniquement ChatOnlyAttribute (19 lignes)
  - Nouvelle fonctionnalité de filtrage de scénarios dans ToolManager (40 lignes)
  - Adaptation du filtrage de scénarios dans ContextManager
  - 4 fichiers modifiés, 115 lignes ajoutées, 30 lignes supprimées

- `5709a33` - Ajout d'attributs de scénario aux classes d'outils
  - Ajout d'attributs ToolScenario à 24 classes d'outils
  - Incluant les outils de calendrier, chat, configuration, curateur, base de données, disque, compilation dynamique, etc.
  - 24 fichiers modifiés, 46 lignes ajoutées, 20 lignes supprimées

#### Refactorisation du système de tâches
- `2f19a5f` - Refactorisation du système de tâches avec TaskCenter et TaskEnumerator
  - Nouveau centre de tâches TaskCenter (235 lignes)
  - Nouvel énumérateur de tâches TaskEnumerator (297 lignes)
  - Refactorisation et simplification du système de tâches TaskSystem
  - Adaptation de l'Être de Silicium DefaultSiliconBeing à la nouvelle architecture
  - Mise à jour de la fabrique DefaultSiliconBeingFactory
  - Amélioration de la classe de base SiliconBeingBase
  - 7 fichiers modifiés, 796 lignes ajoutées, 275 lignes supprimées

#### Migration du système d'autorisations
- `a06ed09` - Migration du système IM et des autorisations vers le projet App
  - Migration de PermissionRequestQueue de Default/Fast vers le projet App (443 lignes ajoutées)
  - Suppression du WebUIProvider de la version Default (403 lignes supprimées)
  - Suppression du HelpTool de la version Default (194 lignes supprimées)
  - Suppression des PermissionRequestQueue dupliqués des versions Default/Fast
  - Suppression du IMPermissionAskHandler de la version Default
  - Mise à jour du contrôleur PermissionRequestController
  - 14 fichiers modifiés, 496 lignes ajoutées, 1183 lignes supprimées

#### Optimisation du contexte IA
- `4c8aaff` - Optimisation du gestionnaire de contexte et amélioration du localisateur de services
  - Simplification et optimisation du gestionnaire de contexte ContextManager
  - Amélioration du localisateur de services ServiceLocator (36 lignes ajoutées)
  - Amélioration du gestionnaire d'outils ToolManager (34 lignes ajoutées)
  - Amélioration des clients DashScopeClient et VolcengineArkClient
  - Mise à jour des exécuteurs (CommandLine, Disk, Network)
  - 8 fichiers modifiés, 116 lignes ajoutées, 98 lignes supprimées

#### Localisation
- `5c5eef7` - Ajout des clés de localisation d'audit et de tâches
  - Nouvelles définitions de localisation dans DefaultLocalizationBase (127 lignes ajoutées)
  - Ajout des clés liées à l'audit et aux tâches dans 9 langues (26 lignes par langue)
  - 11 fichiers modifiés, 387 lignes ajoutées

#### Configuration du projet
- `2067db6` - Mise à jour de la configuration du projet et des règles gitignore
  - Mise à jour des règles .gitignore
  - Amélioration de DefaultConfigData et Fast DefaultConfigData
  - Amélioration du stockage SpeedyWorkNoteStorage
  - Amélioration du cœur de SpeedyPack
  - 5 fichiers modifiés, 32 lignes ajoutées, 6 lignes supprimées

### 2026-05-07

#### Localisation italienne
- `8adc18c` - Ajout de la prise en charge de localisation italienne et mise à jour de la documentation multilingue
  - Nouvelle localisation it-IT italienne
  - Nouvelle implémentation de localisation ItIT (1909 lignes)
  - Nouveau support du calendrier historique chinois en italien ChineseHistoricalItIT (586 lignes)
  - Nouvelle localisation de la barre d'état italienne TrayItIT (135 lignes)
  - Nouveau jeu complet de documents italiens (14 documents : README, référence API, architecture, système de calendrier, journal des modifications, guide de contribution, etc.)
  - Mise à jour des documents d'architecture, guide de développement, guide de démarrage, etc. pour toutes les versions linguistiques
  - Ajout de l'italien à l'énumération Language
  - 86 fichiers modifiés, 11573 lignes ajoutées, 769 lignes supprimées

#### Synchronisation de la documentation
- `12a5deb` - Mise à jour de la documentation multilingue pour l'architecture, le journal des modifications et le guide des Êtres de Silicium
  - Mise à jour des README pour 8 langues
  - Mise à jour des documents d'architecture pour 8 langues
  - Mise à jour des journaux des modifications pour 8 langues
  - Mise à jour des guides des Êtres de Silicium pour 8 langues
  - Mise à jour des références d'outils pour 8 langues
  - Refactorisation du glossaire
  - 46 fichiers modifiés, 1697 lignes ajoutées, 442 lignes supprimées

### 2026-05-06

#### Refactorisation massive des modules
- `eeb3be6` - Refactorisation et réorganisation massive des modules
  - Ajustement de la structure du projet SiliconLife.App
  - Réorganisation du projet SiliconLife.Fast
  - Réorganisation du projet SiliconLife.Default
  - Réorganisation du module partagé SiliconLife.Common
  - Réorganisation du module principal SiliconLife.Core
  - Réorganisation du moteur de stockage SiliconLife.Speedy
  - Réorganisation de l'outil de gestion SiliconLife.Speedy.Manager
  - 119 fichiers modifiés, 6926 lignes ajoutées, 3066 lignes supprimées

### 2026-05-04

#### Client IA
- `24d2c86` - Ajout de VolcengineArkClient et remplacement de l'Audit par le suivi d'utilisation
  - Nouveau client IA VolcengineArkClient pour Ark de Volcengine
  - Support des modes flux et non-flux
  - Contrôle de débit double intégré (auto-contrôle de débit + limites de débit du serveur)
  - Compatible avec le protocole API OpenAI
  - Remplacement du système Audit par le suivi d'utilisation
  - 24 fichiers modifiés, 802 lignes ajoutées, 21 lignes supprimées

#### Système d'outils
- `f27650a` - Ajout de l'outil de rechargement à chaud pour le redémarrage automatique de Fast
  - Nouvel outil de rechargement à chaud HotReloadTool
  - Support de la compilation en ligne, de la mise à jour et du redémarrage de SiliconLife.Fast
  - Nouveau mise à jour autonome HotReload.exe
  - Mécanisme de copie de fichiers sécurisé (ne s'écrase pas soi-même)
  - Arrêt gracieux et attente de libération du port
  - 9 fichiers modifiés, 581 lignes ajoutées

#### Localisation
- `6a5aad8` - Mise à jour de tous les fichiers et ajout de la prise en charge de localisation française
  - Nouvelle localisation fr-FR française
  - Mise à jour de toutes les versions linguistiques
  - Traduction française de l'aide
  - Traduction française de l'interface
  - Plus de 100 fichiers modifiés

### 2026-05-03

#### Infrastructure du projet
- `2664b0c` - Mise à jour de l'infrastructure du projet et des dépendances
  - SiliconLife.Speedy.Manager : nouvelle interface de gestion WPF (MainForm.Designer.cs, MainForm.resx)
  - Nouvelle ressource d'icône slc.ico (1,5 Mo)
  - Amélioration majeure de l'analyse de sécurité PluginLoader (622 lignes ajoutées)
  - Nouvelle fabrique de flux avec autorisations PermissionedStreamFactory (779 lignes)
  - Nouvelle file d'attente de demandes d'autorisation PermissionRequestQueue (versions Default et Fast)
  - Nouveau fournisseur de journaux de débogage DebugLoggerProvider
  - Amélioration de la classe de base de configuration ConfigDataBase
  - Ajout de la fonctionnalité de scan d'outils de plugins dans ToolManager (ScanAllPluginAssemblies)
  - Amélioration de la gestion du cycle de vie de SiliconBeingManager
  - Amélioration majeure du client IA Alibaba Cloud DashScopeClient (227 lignes ajoutées)
  - Amélioration de la fabrique DefaultSiliconBeingFactory
  - Mise à jour des vues et contrôleurs Web (ChatView, WorkNoteView, PermissionRequestController)
  - Ajout de clés dans la localisation de 9 langues
  - 35 fichiers modifiés, 28080 lignes ajoutées, 336 lignes supprimées

### 2026-05-02

#### Amélioration du client IA
- `c16f99f` - Mise à jour des clients IA, de l'UI Web et des composants de stockage
  - Amélioration majeure du client Alibaba Cloud DashScopeClient
  - Optimisation du compacteur automatique SpeedyPackAutoCompactor
  - Amélioration de la classe de base des vues Web et de BeingView
  - 6 fichiers modifiés, 240 lignes ajoutées, 81 lignes supprimées

#### Système de plugins
- `242dc98` - Ajout de la liste des plugins sur la page À propos
  - Ajout de l'affichage des informations de plugins dans AboutController
  - Nouveau modèle de données de plugins AboutViewModel
  - Nouveau rendu de la liste des plugins dans AboutView
  - Ajout des clés liées aux plugins dans la localisation de 9 langues
  - 14 fichiers modifiés, 160 lignes ajoutées, 1 ligne supprimée

#### Optimisation IA
- `147f8f4` - Simplification du texte de prompt de mémoire contextuelle
  - Optimisation des prompts IA dans ContextManager
  - 1 fichier modifié, 1 ligne ajoutée, 1 ligne supprimée

#### Optimisation du stockage Speedy
- `8bda2d3` - Mise à jour du stockage Speedy et de l'implémentation du contrôleur de mémoire
  - Correction de l'intervalle SpeedyPackAutoCompactor
  - Optimisation du traitement des chemins SpeedyTimeStorage
  - Amélioration du contrôleur de mémoire MemoryController
  - Mise à jour de l'UI SpeedyPack.Manager
  - 4 fichiers modifiés, 21 lignes ajoutées, 18 lignes supprimées

#### Amélioration de la barre d'état
- `8972654` - Amélioration de la prise en charge de localisation de la fenêtre d'état de la barre d'état
  - Ajout de l'entrée de gestion Speedy dans la localisation de la barre d'état de 9 langues
  - Ajout de l'élément de menu de gestion Speedy dans TrayStatusWindow
  - 11 fichiers modifiés, 72 lignes ajoutées

#### Optimisation de Speedy.Manager
- `6f5db09` - Optimisation de l'UI et des composants internes du gestionnaire SpeedyPack
  - Refactorisation de l'interface MainForm
  - Optimisation de la gestion mémoire FreeList
  - Amélioration de la file d'écriture WriteQueue
  - Optimisation du cœur de SpeedyPack
  - 5 fichiers modifiés, 96 lignes ajoutées, 88 lignes supprimées

#### Amélioration du système de stockage
- `57f9d5d` - Amélioration du système de stockage, ajout de la compression automatique et du support des dates incomplètes
  - Nouveau minuteur de compression automatique SpeedyPackAutoCompactor (intervalle de 30 minutes)
  - Amélioration du gestionnaire singleton SpeedyPackRegistry
  - Amélioration de l'adaptation de SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage
  - Nouvelle gestion d'espace libre FreeList dans SpeedyPack (149 lignes)
  - Refactorisation et optimisation du writer PackFileWriter
  - Amélioration de WriteOperation, WriteQueue
  - Extension des options de configuration SpeedyPackOptions
  - Ajout de méthodes de comparaison dans IncompleteDate
  - Amélioration du chargeur de plugins PluginLoader
  - Mise à jour du flux d'initialisation de Program.cs pour les versions Default et Fast
  - Simplification des données de configuration DefaultConfigData
  - Simplification du réseau de connaissances KnowledgeNetwork
  - Optimisation des contrôleurs ChatController, MemoryController
  - Amélioration des fonctionnalités MainForm de SpeedyPack.Manager
  - 22 fichiers modifiés, 639 lignes ajoutées, 253 lignes supprimées

#### Mise à jour de Speedy.Manager
- `b04ed33` - Mise à jour des fichiers Speedy.Manager

### 2026-05-01

#### Refactorisation architecturale : Speedy remplace LiteDB
- `6600972` - Remplacement de LiteDB par le stockage Speedy, ajout du système de plugins et du projet Speedy
  - **Nouveau projet SiliconLife.Speedy** : moteur de stockage .spk haute performance
    - Classe principale SpeedyPack (489 lignes) : mappage de répertoire en mémoire + cache d'entrées + file d'écriture asynchrone
    - Classe de configuration SpeedyPackOptions : TTL du cache, nombre max d'entrées en cache, mode lecture seule
    - Interface de transaction IPackTransaction : support des opérations d'écriture atomiques
    - Classe d'informations de fichier SpkFileInfo
    - Répertoire Internal : DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Dépendance à MessagePack 3.1.4 pour la sérialisation binaire (compression LZ4)
  - **Nouveau projet SiliconLife.Speedy.Manager** : outil de gestion WPF
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
    - Nouveau SpeedyPackAutoCompactor (minuteur de compression automatique)
    - Suppression des implémentations de stockage LiteDB (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Suppression du code lié à la fenêtre de gestion LiteDB
  - **Système de plugins** :
    - Nouvelle interface IPlugin (Core/Plugins/IPlugin.cs)
    - Nouveau chargeur de plugins PluginLoader (Core/Plugins/PluginLoader.cs)
    - Support du chargement de DLL de plugins depuis un répertoire
    - Analyse de sécurité : vérification des espaces de noms interdits (System.IO, System.Net, Microsoft.CodeAnalysis, etc.)
    - Liste blanche d'assemblys de confiance (Google.Protobuf, Newtonsoft.Json, MessagePack, etc.)
    - Chargement isolé avec AssemblyLoadContext personnalisé
    - Nouvelle méthode ScanAllPluginAssemblies dans ToolManager
    - Intégration du chargeur de plugins dans CoreHost
  - 119 fichiers modifiés, 6926 lignes ajoutées, 3066 lignes supprimées

#### Amélioration des Êtres de Silicium
- `3aef4c3` - Ajout de l'état d'activité Stopped et amélioration de la gestion des erreurs
  - Nouvel état Stopped pour les Êtres de Silicium
  - Amélioration de la gestion des erreurs et du mécanisme de récupération

#### Mise à jour de la localisation
- `513c65d` - Mise à jour de toutes les versions linguistiques et de la documentation
  - Nouveau composant MarkdownEditorComponent (625 lignes)
  - Nouveau composant DetailsComponent (130 lignes)
  - Nouveau composant accordéon AccordionComponent (285 lignes)
  - Mise à jour des contrôleurs BeingController, ChatController, MemoryController, PermissionController
  - Refactorisation des vues BeingView, ChatView, MemoryView, SoulEditorView
  - Suppression de l'ancien MarkdownEditorView
  - Migration en composants de InitController
  - 115 fichiers modifiés, 5761 lignes ajoutées, 2362 lignes supprimées

### 2026-04-30

#### Fonctionnalité de barre d'état système
- `101b203` - Implémentation de la fenêtre d'état de la barre d'état et d'ApplicationContext
  - Nouvelles ressources d'icônes de barre d'état (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implémentation de la fenêtre d'état TrayStatusWindow
  - Support de la localisation de la barre d'état en 9 langues (TrayCsCZ, TrayDeDE, TrayEnUS, etc.)
  - Classe de base abstraite TrayLocalizationBase
  - 24 fichiers modifiés, 27995 lignes ajoutées, 1 ligne supprimée (incluant les fichiers de ressources)

#### Architecture UI en composants
- `e61cfaa` - Achèvement de l'architecture UI en composants, implémentation de 24 composants
  - Phase MVP (8) : ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Deuxième phase (6) : Accordion, Card, Tabs, Table, Modal, Message
  - Troisième phase (5) : Calendar, Tree, Chart, FileUpload, RichText
  - Nouvelles classes auxiliaires Js, Behavior, DomUpdate, etc.
  - 25 fichiers modifiés, 2666 lignes ajoutées

- `7449e51` - Amélioration du système de composants et ajout de nouveaux thèmes d'habillage
  - Amélioration des composants A, Button, Div, Form, Input, etc.
  - Ajout de 3 thèmes d'habillage : HighContrast, Light, Minimal
  - Mise à jour des habillages existants (Admin, Chat, Creative, Dev)
  - Migration en composants de InitController
  - 32 fichiers modifiés, 1466 lignes ajoutées, 1238 lignes supprimées

- `1ba8636` - Démarrage de la migration en composants de InitController (en cours)
  - 9 fichiers modifiés, 574 lignes ajoutées, 145 lignes supprimées

#### Unification du système de stockage
- `895dff9` - Unification de soul.md et state.json pour utiliser l'interface IStorage
  - DefaultSiliconBeing utilise IStorage pour lire/écrire le Fichier d'Âme et l'état
  - Nouveau gestionnaire de fichiers d'état StateFileManager
  - Refactorisation et adaptation de SoulFileManager à IStorage
  - 8 fichiers modifiés, 201 lignes ajoutées, 116 lignes supprimées

#### Amélioration de la gestion LiteDB
- `a34bef4` - Ajout de LiteDBManager et amélioration de la localisation de la barre d'état
  - Ajout de l'entrée de gestion LiteDB dans le menu de la barre d'état
  - Mise à jour de la localisation de la barre d'état pour 9 langues
  - 10 fichiers modifiés, 196 lignes ajoutées

- `c4a79ca` - Ajout d'une fabrique de localisation sensible à la langue pour la fenêtre de gestion LiteDB
  - 1 fichier modifié, 78 lignes ajoutées

- `5ebc55e` - Conversion de LiteDBAdminLocalization en classe de base abstraite
  - 10 fichiers modifiés, 1356 lignes ajoutées

#### Correction du système de configuration
- `2da5256` - Ajout de la méthode abstraite ConfigExists et correction des enregistrements de configuration LiteDB dupliqués
  - Nouvelle méthode ConfigExists dans ConfigDataBase
  - Implémentation de la vérification d'existence de configuration LiteDB dans DefaultConfigData de la version Fast
  - Correction du problème des clés de configuration LiteDB dupliquées
  - 9 fichiers modifiés, 210 lignes ajoutées, 2 lignes supprimées

#### Optimisation du chat et des vues
- `d3618ec` - Optimisation des sessions de chat, du système de stockage, du modèle temporel et de la classe de base des vues
  - Optimisation de BroadcastChannel, GroupChatSession, SingleChatSession
  - Ajout de méthodes de requête dans ITimeStorage
  - Mise à jour synchrone de FileSystemStorage et LiteDBStorage
  - Refactorisation et optimisation de ViewBase (versions Default et Fast)
  - 11 fichiers modifiés, 622 lignes ajoutées, 392 lignes supprimées

### 2026-04-29

#### Refactorisation architecturale : extraction des modules partagés
- `a102428` - Migration des modules partagés de SiliconLife.Default vers SiliconLife.Common
  - Extraction de 32 implémentations de calendriers vers le projet Common
  - Extraction de la classe de base de localisation et de ses 21 implémentations linguistiques vers le projet Common
  - Extraction du gestionnaire d'autorisations et de l'implémentation par défaut de l'Être de Silicium vers le projet Common
  - Extraction de 23 implémentations d'outils intégrés vers le projet Common
  - Extraction de l'implémentation Playwright WebView vers le projet Common
  - Mise à jour des espaces de noms vers SiliconLife.Collective
  - 122 fichiers modifiés, 586 lignes ajoutées, 343 lignes supprimées

#### Amélioration de la qualité du code
- `17566fe` - Remplacement de Console.WriteLine par le système de journalisation dans les projets Core, Common et Default
  - Mise à jour de 6 fichiers incluant ContextManager, AuditLogger, DefaultConfigData
  - Unification de l'utilisation de l'interface ILogger, amélioration de la maintenabilité du code
  - 6 fichiers modifiés, 12 lignes ajoutées, 8 lignes supprimées

#### SiliconLife.Fast version haute performance
- `54a0307` - Ajout du projet SiliconLife.Fast et correction de la compilation
  - Point d'entrée complet de l'application Windows Forms
  - Support de la barre d'état système (NotifyIcon)
  - Portage de tous les contrôleurs UI Web (20+)
  - Portage de tous les composants de vues Web
  - Portage de 4 thèmes d'habillage (Admin, Chat, Creative, Dev)
  - 125 fichiers modifiés, 61186 lignes ajoutées

#### Synchronisation de la documentation multilingue
- `265fde8` - Synchronisation de la documentation de la double architecture vers toutes les langues
  - Mise à jour de architecture.md, changelog.md pour 7 langues
  - Mise à jour de contributing.md pour 6 langues
  - Mise à jour de getting-started.md, roadmap.md pour 7 langues
  - 47 fichiers modifiés, 1214 lignes ajoutées, 38 lignes supprimées

#### Système de stockage LiteDB (version Fast)
- `4704862` - Ajout des dépendances et de l'infrastructure LiteDB
  - Nouvelle classe de gestion LiteDBManager
  - Nouveaux modèles de données LiteDBModels
  - 3 fichiers modifiés, 252 lignes ajoutées

- `4220036` - Implémentation des classes de stockage LiteDB
  - LiteDBStorage : implémentation de l'interface IStorage
  - LiteDBTimeStorage : implémentation de l'interface ITimeStorage
  - LiteDBWorkNoteStorage : implémentation de l'interface IWorkNoteStorage
  - 3 fichiers modifiés, 581 lignes ajoutées

- `38ebd23` - Migration de la configuration et du système de journalisation vers LiteDB
  - Adaptation de DefaultConfigData au stockage LiteDB
  - Nouveau fournisseur de journaux LiteDBLoggerProvider
  - 2 fichiers modifiés, 203 lignes ajoutées, 67 lignes supprimées

- `e687157` - Migration du réseau de connaissances du système de fichiers vers LiteDB
  - Refactorisation complète de KnowledgeNetwork, utilisation de LiteDB pour le stockage des triplets
  - 1 fichier modifié, 231 lignes ajoutées, 72 lignes supprimées

- `4220169` - Intégration du stockage LiteDB dans Program et ProjectManager
  - Initialisation du stockage LiteDB dans Program.cs
  - Adaptation de ProjectManager au stockage de notes de travail LiteDB
  - 2 fichiers modifiés, 40 lignes ajoutées, 17 lignes supprimées

- `5f3a709` - Suppression des implémentations de stockage sur système de fichiers obsolètes
  - Suppression de FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage, etc.
  - 6 fichiers modifiés, 1518 lignes supprimées

- `e1a4ef2` - docs: ajout de l'identifiant de version v0.1.0-alpha à toute la documentation
  - 127 fichiers modifiés, 2297 lignes ajoutées, 2471 lignes supprimées

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Refactorisation du système de stockage
- `8dd26e3` - Unification de l'utilisation d'IncompleteDate dans l'interface ITimeStorage et ajout d'API de requête par niveau
  - Suppression des surcharges DateTime dans l'interface ITimeStorage, unification avec IncompleteDate
  - Ajout de la méthode de comparaison CompareTo(DateTime) et de la méthode Expand() dans IncompleteDate
  - Nouvelles API de requête par niveau GetEarliestTimestamp(), GetLatestTimestamp()
  - Nouvelles méthodes HasSummary() et QueryWithLevel(), support des requêtes par niveau temporel
  - Refactorisation de l'algorithme de compression dans Memory.cs, utilisation de la nouvelle API de requête par niveau pour améliorer l'efficacité
  - Implémentation complète des nouvelles méthodes d'interface dans FileSystemTimeStorage.cs
  - Mise à jour synchrone de tous les appelants : ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, etc.
  - Mise à jour du système d'outils : HelpTool, LogTool, TokenAuditTool adaptés à la nouvelle interface
  - Mise à jour des contrôleurs Web : AuditController, ChatController, ChatHistoryController adaptés à la nouvelle interface
  - 41 fichiers modifiés, 1820 lignes ajoutées, 903 lignes supprimées

### 2026-04-27

#### Amélioration du système de documentation d'aide
- `9989d79` - Mise à jour de la localisation, du système d'aide et des vues Web
  - Nouvelle interface de documentation d'aide IAIClientFactoryHelp.cs
  - Achèvement de la traduction de toute la documentation d'aide en 9 langues
  - Ajout de 40 définitions de sujets d'aide dans HelpTopics.cs
  - Mise à jour complète des vues Web : InitController, AuditView, ConfigView, KnowledgeView, LogView, etc.
  - Amélioration du système de localisation : ajout de nouvelles clés dans toutes les versions linguistiques
  - Mise à jour de la fabrique de clients IA : amélioration de DashScopeClientFactory, OllamaClientFactory
  - 30 fichiers modifiés, 10086 lignes ajoutées, 15 lignes supprimées

#### Nouveau contenu de documentation d'aide
- `e7afe94` - Ajout de la documentation d'aide du Fichier d'Âme et des journaux d'audit
  - Nouvelle documentation d'aide de gestion du Fichier d'Âme
  - Nouvelle documentation d'aide des journaux d'audit
  - Ajout de définitions de sujets dans HelpTopics.cs
  - Refactorisation majeure de HelpView.cs, amélioration de la logique de rendu de la documentation
  - Refactorisation de PermissionView.cs, amélioration de l'interface de gestion des autorisations
  - Amélioration des modules principaux : SiliconBeingManager, TaskSystem, ToolManager
  - Refactorisation de TaskTool.cs, amélioration de la fonctionnalité de gestion des tâches
  - Mise à jour complète des vues Web : tous les composants de vue mis à jour synchrone
  - Simplification de HelpController.cs, optimisation de la logique du contrôleur
  - 30 fichiers modifiés, 7100 lignes ajoutées, 897 lignes supprimées

### 2026-04-26

#### Système de documentation d'aide
- `07895d7` - Amélioration du système de documentation d'aide, ajout de 3 documents et achèvement de la traduction en 9 langues
  - Ajout du système de mémoire, guide d'installation Ollama, guide d'utilisation de la plateforme DashScope d'Alibaba Cloud
  - Achèvement de la traduction de tous les 10 documents d'aide en 9 langues
  - Simplification de la logique de rendu de HelpView
  - 18 fichiers modifiés, 14418 lignes ajoutées, 1364 lignes supprimées

#### Localisation allemande
- `0cfd8a1` - Ajout du support complet de localisation allemande (de-DE)
  - Fichier de localisation allemande complet
  - Nouveau support du calendrier historique chinois en allemand
  - Nouvelle traduction de la documentation d'aide en allemand
  - Synchronisation complète de tous les documents pour 9 langues
  - 135 fichiers modifiés, 26186 lignes ajoutées, 14371 lignes supprimées

#### Synchronisation de la documentation
- `3aada7d` - Synchronisation de la documentation chinois traditionnel (zh-HK) avec le chinois simplifié
  - 3 fichiers modifiés, 519 lignes ajoutées, 422 lignes supprimées
- `2f6abff` - Ajout de la localisation du nom d'affichage de l'outil d'aide pour toutes les langues
  - 7 fichiers modifiés, 47 lignes ajoutées, 7 lignes supprimées

#### Refactorisation du système de connaissances
- `60944fe` - Unification des espaces de noms vers SiliconLife.Collective
  - 8 fichiers modifiés, 5 lignes ajoutées, 8 lignes supprimées
- `69c51c5` - Ajout du système de documentation d'aide et traduction des commentaires de code en anglais
  - 29 fichiers modifiés, 3385 lignes ajoutées, 22 lignes supprimées

### 2026-04-25

#### Automatisation de navigateur WebView
- `41757c3` - Implémentation de l'automatisation de navigateur WebView multiplateforme basée sur Playwright
  - 6 fichiers modifiés, 1152 lignes ajoutées

#### Mise à jour de la documentation
- `0ff797b` - Ajout de la documentation KnowledgeTool et WorkNoteTool (7 langues)
  - 28 fichiers modifiés, 4983 lignes ajoutées
- `ad77415` - Mise à jour de tous les fichiers changelog, ajout des enregistrements d'historique Git du 2026-04-25
  - 7 fichiers modifiés, 168 lignes ajoutées

#### Gestion de l'espace de travail de projet
- `785c551` - Implémentation de la gestion de l'espace de travail de projet, incluant les notes de travail et le système de tâches
  - Nouveau système de gestion de l'espace de travail de projet
  - Fonctionnalité de notes de travail pour le suivi de la progression du projet
  - Intégration du système de gestion des tâches
  - 29 fichiers modifiés, 4256 lignes ajoutées, 36 lignes supprimées

#### Localisation tchèque
- `b4bbf39` - Ajout du support complet de localisation tchèque (cs-CZ) et mise à jour de la documentation pour toutes les langues
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

#### Extension du système d'autorisations
- `4489ad6` - Ajout du service météo wttr.in à la liste blanche réseau
  - Mise à jour synchrone complète de la documentation multilingue (6 langues)
  - 14 fichiers modifiés, 417 lignes ajoutées, 1 ligne supprimée

#### Correction de l'interface Web
- `d9d72e9` - Correction du problème de priorité CSS de la modale de détails des notes de travail
  - 19 fichiers modifiés, 1744 lignes ajoutées, 6 lignes supprimées

#### Optimisation de l'historique de chat
- `0df599c` - Correction du problème des résultats d'outil rendus comme messages de chat indépendants
  - 1 fichier modifié, 222 lignes ajoutées, 21 lignes supprimées
- `057b09d` - Optimisation de l'affichage des détails de l'historique de chat, amélioration du rendu des appels d'outil
  - 3 fichiers modifiés, 389 lignes ajoutées, 68 lignes supprimées

#### Historique d'exécution des minuteurs
- `fa3f06f` - Ajout de la fonctionnalité d'historique d'exécution des minuteurs, incluant la vue détaillée
  - 8 fichiers modifiés, 937 lignes ajoutées, 10 lignes supprimées
- `d824835` - Ajout des clés de localisation de l'historique d'exécution des minuteurs (toutes les langues)
  - 7 fichiers modifiés, 88 lignes ajoutées

#### Amélioration de la localisation
- `c13cb17` - Enregistrement de la variante de langue espagnole
  - 1 fichier modifié, 4 lignes ajoutées
- `9c44f34` - Ajout du support de localisation multilingue du calendrier historique chinois
  - 16 fichiers modifiés, 6049 lignes ajoutées, 1 ligne supprimée

#### Amélioration des fonctionnalités principales
- `1e7c7b2` - Amélioration de la compression de la mémoire et du suivi de l'exécution des outils
  - 4 fichiers modifiés, 338 lignes ajoutées, 86 lignes supprimées

### 2026-04-23

#### Localisation des outils
- `192fc6e` - Ajout de la localisation manquante des noms d'outils pour 5 outils
  - 6 fichiers modifiés, 30 lignes ajoutées

#### Mise à jour de la documentation
- `882c08f` - Mise à jour de tous les fichiers changelog, ajout de l'historique Git complet et suppression des faux numéros de version
  - 45 fichiers modifiés, 8815 lignes ajoutées, 1611 lignes supprimées

#### Amélioration de la page de chat
- `65c157b` - Ajout d'un indicateur de chargement et sélection automatique de la session du curateur sur la page de chat
  - 10 fichiers modifiés, 211 lignes ajoutées, 7 lignes supprimées

#### Fonctionnalité d'historique de chat
- `e483348` - Implémentation de la fonctionnalité de consultation de l'historique de chat des Êtres de Silicium
  - Nouveau ChatHistoryController
  - Création de ChatHistoryViewModel
  - Implémentation des pages ChatHistoryListView et ChatHistoryDetailView
  - Ajout des clés de localisation de l'historique de chat (5 langues)
  - 12 fichiers modifiés, 1178 lignes ajoutées

#### Amélioration du contrôle de flux IA
- `30a2d4e` - Amélioration de l'annulation du flux IA, de l'intégration IM et de l'initialisation de l'hôte principal
  - 11 fichiers modifiés, 387 lignes ajoutées, 12 lignes supprimées

#### File d'attente de messages de chat
- `db48c51` - Ajout de la file d'attente de messages de chat, des métadonnées de fichiers et du support d'annulation de flux
  - 4 fichiers modifiés, 357 lignes ajoutées

#### Support du téléchargement de fichiers
- `28fb344` - Implémentation du dialogue de source de fichiers et du support de téléchargement de fichiers
  - 3 fichiers modifiés, 1100 lignes ajoutées, 2 lignes supprimées
- `1d3e2cc` - Ajout des chaînes de localisation du dialogue de source de fichiers (6 langues)
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
- `7a03a19` - Amélioration de la flexibilité des requêtes de conversation de LogTool
  - 1 fichier modifié, 57 lignes ajoutées, 24 lignes supprimées

#### Amélioration de la localisation
- `0a8d750` - Ajout d'un prompt système générique pour le comportement proactif des Êtres de Silicium
  - 8 fichiers modifiés, 460 lignes ajoutées, 48 lignes supprimées

#### Refactorisation du système de journalisation
- `2b771f3` - Découplage de LogController des E/S fichier, ajout d'API de lecture des journaux
  - 4 fichiers modifiés, 172 lignes ajoutées, 137 lignes supprimées
- `12da302` - Ajout d'un filtre par Être de Silicium dans la vue des journaux
  - 9 fichiers modifiés, 147 lignes ajoutées, 10 lignes supprimées
- `8f6cb1e` - Ajout du paramètre beingId à l'interface ILogger, séparation des journaux système/Êtres de Silicium
  - 47 fichiers modifiés, 524 lignes ajoutées, 490 lignes supprimées

#### Amélioration du système d'autorisations
- `4c747ad` - Refactorisation de PermissionTool, ExecuteCodeTool, ajout de l'API EvaluatePermission
  - 18 fichiers modifiés, 680 lignes ajoutées, 492 lignes supprimées

#### Corrections de bugs
- `1c96e99` - Correction de l'échec de search_files et search_content pour les recherches à la racine
  - 1 fichier modifié, 98 lignes ajoutées, 41 lignes supprimées

#### Intégration d'outils
- `135710d` - Suppression de SearchTool, migration de la recherche locale vers DiskTool
  - 2 fichiers modifiés, 185 lignes ajoutées, 365 lignes supprimées

#### Extension du système d'outils
- `70ce7fb` - Implémentation de DatabaseTool pour les requêtes de base de données structurées
  - 1 fichier modifié, 382 lignes ajoutées
- `be29a09` - Implémentation de LogTool pour les requêtes d'historique des opérations et des conversations
  - 1 fichier modifié, 298 lignes ajoutées
- `4ea7702` - Implémentation de PermissionTool pour la gestion dynamique des autorisations
  - 1 fichier modifié, 457 lignes ajoutées
- `1384ff4` - Implémentation de ExecuteCodeTool pour l'exécution de code multilingue
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
- `c6b518b` - Correction de la transmission des messages de minuteur et du stockage des messages de chat
  - 3 fichiers modifiés, 297 lignes ajoutées, 124 lignes supprimées

#### Gestion de la configuration
- `4305769` - Ajout de .gitattributes pour la gestion des fins de ligne
  - 1 fichier modifié, 32 lignes ajoutées

#### Améliorations de l'interface Web
- `188c6f8` - Enregistrement de la route API de la liste des tâches et ajout de l'affichage d'état vide
  - 2 fichiers modifiés, 35 lignes ajoutées, 2 lignes supprimées
- `634e8ca` - Ajout d'un lien de retour à la liste sur la page des autorisations
  - 1 fichier modifié, 16 lignes ajoutées
- `6ba591d` - Ajout d'un éditeur de configuration IA autonome pour les Êtres de Silicium
  - 11 fichiers modifiés, 842 lignes ajoutées, 18 lignes supprimées
- `0a826f5` - Ajout d'une notification de sauvegarde réussie dans l'éditeur de code
  - 1 fichier modifié, 9 lignes ajoutées, 2 lignes supprimées
- `2940373` - Amélioration de l'interface Web, ajout d'info-bulles de code et améliorations UI
  - 11 fichiers modifiés, 1054 lignes ajoutées, 75 lignes supprimées

#### Correction du système d'autorisations
- `592c7ab` - Correction de l'ordre d'instanciation et d'enregistrement du rappel
  - 2 fichiers modifiés, 38 lignes ajoutées, 7 lignes supprimées

#### Amélioration de la sécurité
- `833ead2` - Ajout de la vérification des références d'assembly pour la compilation dynamique
  - 4 fichiers modifiés, 135 lignes ajoutées, 8 lignes supprimées

#### Amélioration du système d'autorisations
- `5879621` - Ajout de la vérification pré-compilation du rappel d'autorisation et amélioration de la gestion des erreurs
  - 21 fichiers modifiés, 617 lignes ajoutées, 26 lignes supprimées

#### Mise à jour de la documentation
- `4dbf659` - Mise à jour du changelog vers v0.5.1, remplacement des URL placeholder GitHub, ajout du miroir Gitee, localisation du nom Bilibili par langue, mise à jour de l'email
  - 32 fichiers modifiés, 489 lignes ajoutées, 180 lignes supprimées

#### Configuration et point d'entrée
- `0fc1693` - Mise à jour du point d'entrée du programme et de la configuration du projet
  - 2 fichiers modifiés, 7 lignes ajoutées

#### Refactorisation du système d'autorisations
- `ea9179a` - Amélioration de l'implémentation du système d'autorisations
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
- `4ff98ad` - Refactorisation de la documentation, support multilingue
  - 81 fichiers modifiés, 23818 lignes ajoutées, 1886 lignes supprimées

### 2026-04-20

#### Amélioration des fonctionnalités principales
- `28905b5` - Support multilingue complet, fabrique de clients IA, système d'autorisations et paramètres de localisation
  - Système de journalisation avec gestionnaire, entrées et différents niveaux de journal
  - Système d'audit de tokens pour interroger et suivre l'utilisation des tokens
  - Fabrique de clients IA pour la découverte automatique de différentes plateformes IA
  - Système de rappel d'autorisations avec son propre stockage
  - Implémentation d'un journaliseur console
  - Support multilingue anglais et chinois simplifié
  - WebUI messenger avec WebSocket pour le chat en temps réel
  - Amélioration de l'Être de Silicium par défaut avec localisation
  - 39 fichiers modifiés, 4670 lignes ajoutées, 175 lignes supprimées

### 2026-04-19

#### Minuteurs et calendrier
- `c933fd8` - Mise à jour de la localisation, du système de minuteurs, des vues Web et ajout d'outils
  - Meilleur gestionnaire de localisation
  - Système de planification de tâches minutées
  - Configuration IA et gestion de contexte
  - Outil calendrier supportant 32 types de calendriers
  - Contrôleur Web pour l'API calendrier
  - Outil de gestion des tâches
  - 46 fichiers modifiés, 4018 lignes ajoutées, 975 lignes supprimées

**Améliorations architecturales**
- Refonte de l'architecture des vues Web pour un meilleur support des habillages
- Amélioration du système de gestion des êtres avec une meilleure gestion des états

### 2026-04-18

- `9f585e1` - Mise à jour de la localisation, du système de minuteurs, des vues Web et ajout d'outils
  - Améliorations des minuteurs et de la planification
  - Meilleures vues Web avec composants UI améliorés
  - Plus d'implémentations d'outils
  - 57 fichiers modifiés, 3328 lignes ajoutées, 389 lignes supprimées

### 2026-04-17

- `9b71fcd` - Mise à jour des modules principaux, ajout de la documentation zh-HK, du canal de diffusion, de l'outil de configuration et de la vue Web d'audit
  - Canal de diffusion pour que plusieurs Êtres de Silicium discutent ensemble
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
- `2161002` - Refactorisation de la documentation et amélioration de la localisation
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
  - Amélioration du gestionnaire de contexte
  - Amélioration du système de chat
  - Mise à jour de l'interface du fournisseur de messagerie
  - Refonte du fournisseur WebUI
  - Mise à jour du constructeur JavaScript et du routeur
  - Optimisation de la vue de chat
  - Amélioration du gestionnaire WebSocket
  - 9 fichiers modifiés, 365 lignes ajoutées, 134 lignes supprimées

### 2026-04-09

- `f9302bf` - Amélioration de l'interface du fournisseur de messagerie, du système de chat et de l'interaction UI Web
  - Extension de l'interface du fournisseur de messagerie
  - Améliorations des messages de chat et du système
  - Optimisation du gestionnaire de contexte
  - Amélioration de l'Être de Silicium par défaut
  - Amélioration de la vue de chat UI Web
  - Mise à jour du gestionnaire WebSocket
  - 10 fichiers modifiés, 427 lignes ajoutées, 93 lignes supprimées

### 2026-04-07

- `6831ee8` - Refonte des vues Web et du constructeur JavaScript
  - Refonte complète des contrôleurs Web
  - Réécriture complète du constructeur JavaScript
  - Mise à jour de tous les composants de vue
  - Amélioration du système d'habillage
  - Amélioration de l'architecture de la classe de base des vues
  - 23 fichiers modifiés, 2004 lignes ajoutées, 1983 lignes supprimées

### 2026-04-05

- `41e97fb` - Mise à jour de plusieurs modules principaux et contrôleurs Web
  - Amélioration du gestionnaire de contexte
  - Système de chat et gestion des sessions
  - Refonte du localisateur de services
  - Mise à jour de la classe de base et du gestionnaire des Êtres de Silicium
  - Mise à jour complète des contrôleurs Web (17 contrôleurs)
  - Amélioration de la fabrique d'Êtres de Silicium par défaut
  - 31 fichiers modifiés, 681 lignes ajoutées, 326 lignes supprimées
- `67988d4` - Amélioration des modules UI Web, ajout de la vue des exécuteurs, nettoyage des vues et modules principaux
  - 61 fichiers modifiés, 3148 lignes ajoutées, 3726 lignes supprimées

### 2026-04-04

- `b58bb1c` - Ajout du contrôleur d'initialisation et refonte des modules Web
  - Contrôleur d'initialisation
  - Refonte du module de configuration
  - Mise à jour du module de localisation
  - Amélioration du système d'habillage
  - Amélioration du routeur
  - 29 fichiers modifiés, 1269 lignes ajoutées, 289 lignes supprimées
- `f03ac0b` - Ajout du module UI Web, amélioration de la fonctionnalité messenger
  - 60 fichiers modifiés, 8481 lignes ajoutées, 165 lignes supprimées

### 2026-04-03

- `192e57b` - Mise à jour de la structure du projet et des composants d'exécution principaux
  - 22 fichiers modifiés, 446 lignes ajoutées, 179 lignes supprimées
- `59faec8` - Mise à jour de l'implémentation principale et par défaut
  - 25 fichiers modifiés, 3056 lignes ajoutées, 18 lignes supprimées
- `d488485` - Ajout de la fonctionnalité de compilation dynamique et du module d'outils du curateur
  - 19 fichiers modifiés, 1727 lignes ajoutées, 11 lignes supprimées
- `753d1d9` - Ajout du module de sécurité, mise à jour des exécuteurs, du fournisseur de messagerie, de la localisation et des outils
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
- `2fa6305` - Implémentation de la phase 2 : Cadre de la Boucle Principale et système d'Objets Tick
  - 9 fichiers modifiés, 594 lignes ajoutées, 41 lignes supprimées
- `32b99a1` - Implémentation de la phase 1 - Fonctionnalité de chat de base
  - 19 fichiers modifiés, 1185 lignes ajoutées
- `358e368` - Commit initial : documentation du projet et licence
  - 10 fichiers modifiés, 1873 lignes ajoutées
