![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Version : v0.2.0-alpha** | **Silicon Life Collective** — Une plateforme de collaboration multi-agents basée sur .NET 9, où les agents IA sont appelés **Êtres de Silicium**, capables d'auto-évolution grâce à la compilation dynamique Roslyn.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | **Français** | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md) | [Русский](../ru-RU/README.md)

## 🌟 Caractéristiques principales

### Système d'agents
- **Orchestration multi-agents** — Gérée uniformément par le *Curateur de Silicium*, avec un mécanisme d'ordonnancement équitable par créneaux temporels piloté par horloge
- **Piloté par Fichier d'Âme** — Chaque Être de Silicium est piloté par un fichier d'invite principal (`soul.md`), définissant sa personnalité unique et ses schémas comportementaux
- **Architecture Corps-Cerveau** — Le *Corps* (SiliconBeing) maintient les signes vitaux et détecte les scénarios de déclenchement ; le *Cerveau* (ContextManager) charge l'historique, appelle l'IA, exécute les outils et persiste les réponses
- **Capacité d'auto-évolution** — Grâce à la compilation dynamique Roslyn, les Êtres de Silicium peuvent réécrire leur propre code pour évoluer
- **Gestion des états d'activité** — Prend en charge quatre états d'activité : Idle (inactif), Working (en travail), Error (erreur), Stopped (arrêté) ; après 10 erreurs consécutives, passage automatique à l'état Stopped

### Système de plugins
- **Architecture d'extension par plugins** — Extension des fonctionnalités via l'interface IPlugin, avec chargement dynamique des DLL de plugins depuis un répertoire
- **Sandbox de sécurité** — Le chargeur de plugins effectue des analyses de sécurité strictes, interdisant l'accès aux espaces de noms System.IO, System.Net, etc.
- **Chargement isolé** — Utilisation d'un AssemblyLoadContext personnalisé pour le chargement isolé, empêchant les plugins d'affecter la stabilité du programme principal
- **Intégration d'outils** — Les plugins peuvent enregistrer des outils personnalisés via l'interface ITool, automatiquement intégrés dans la boucle d'appel d'outils

### Outils et exécution
- **24 outils intégrés** — Couvrant le calendrier, le chat, la configuration, le disque, le réseau, la mémoire, les tâches, les minuteurs, la base de connaissances, les notes de travail, l'espace projet, le navigateur WebView, le rechargement à chaud, etc.
- **Isolation des scénarios d'outils** — Chaque outil déclare ses scénarios disponibles via l'attribut `ToolScenario` (Chat, Task, Timer, MemoryCompression, Project) ; l'attribut `ChatOnly` restreint l'outil au seul scénario de chat
- **Outil de rechargement à chaud** — Prend en charge la compilation automatique, la mise à jour des fichiers et le redémarrage de SiliconLife.Fast en cours d'exécution, sans intervention manuelle
- **Boucle d'appel d'outils** — L'IA retourne un appel d'outil → exécution de l'outil → résultat renvoyé à l'IA → boucle continue jusqu'à l'obtention d'une réponse en texte brut
- **Sécurité Exécuteur-Autorisation** — Toutes les opérations d'E/S passent par des exécuteurs avec vérification stricte des autorisations
  - Chaîne de vérification des autorisations à 3 niveaux : UserFrequencyCache → IPermissionCallback → (IsCurator : IPermissionAskHandler | Non-curateur : GlobalACL → refus par défaut)
  - Journal d'audit complet enregistrant toutes les décisions d'autorisation

### IA et connaissances
- **Support multi-backends IA**
  - **Ollama** — Déploiement de modèles locaux, utilisation de l'API HTTP native
  - **Alibaba Cloud Bailian (DashScope)** — Service IA cloud, compatible API OpenAI, prenant en charge plus de 13 modèles, déploiement multi-régions
  - **Volcengine Ark** — Service IA cloud de ByteDance, prenant en charge les modes flux et non-flux, avec contrôle de débit intégré
- **32 systèmes calendaires** — Couverture complète des principaux calendriers du monde, incluant grégorien, lunaire chinois, islamique, hébraïque, japonais, persan, maya, calendriers historiques chinois, etc.
- **Système de Réseau de Connaissances** — Graphe de connaissances basé sur des triplets (sujet-relation-objet), prenant en charge le stockage, la recherche et la découverte de chemins
- **Espace de projet** — Gestion d'espaces projet, prenant en charge la création/archivage/destruction de projets, l'attribution de rôles, les notes de travail, le suivi des tâches et l'isolation des autorisations d'outils
- **Moteur de flux de travail** — Moteur à machine à états basé sur des modèles, prenant en charge les modèles de flux de travail personnalisés, les transitions d'état, l'exécution pilotée par Tick et la gestion du cycle de vie des instances
- **Mécanisme d'oubli de la mémoire** — Service de dégradation temporelle (MemoryFadeService), appliquant automatiquement chaque heure une dégradation de l'importance et un archivage automatique des mémoires de tous les Êtres de Silicium

### Interface Web
- **Interface Web moderne** — Serveur HTTP intégré, prenant en charge les mises à jour en temps réel via SSE
- **7 thèmes d'apparence** — Gestion, Chat, Créatif, Développement, Contraste élevé, Clair, Minimaliste, avec découverte automatique et changement de thème
- **24 contrôleurs** — Fonctionnalités complètes de gestion système, chat, configuration et surveillance
- **Zéro dépendance de framework frontend** — Génération de HTML/CSS/JS côté serveur via `H`, `CssBuilder` et `JsBuilder`

### Internationalisation et localisation
- **34 variantes linguistiques** entièrement prises en charge, couvrant 2 systèmes d'écriture et plusieurs variantes régionales
  - **Chinois simplifié** : zh-CN (Chine continentale), zh-SG (Singapour), zh-MY (Malaisie) (3 variantes)
  - **Chinois traditionnel** : zh-HK (Hong Kong), zh-TW (Taïwan), zh-MO (Macao) (3 variantes)
  - **Anglais** : en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variantes)
  - **Espagnol** : es-ES, es-MX (2 variantes)
  - **Allemand** : de-DE, de-AT, de-CH, de-LU, de-LI (5 variantes)
  - **Français** : fr-FR, fr-CA, fr-CH (3 variantes)
  - **Japonais** : ja-JP | **Coréen** : ko-KR | **Tchèque** : cs-CZ (3 variantes)
  - **Italien** : it-IT | **Polonais** : pl-PL | **Portugais** : pt-PT, pt-BR (4 variantes)

### Données et stockage
- **Stockage haute performance SpeedyPack** — La version Fast utilise le moteur de stockage .spk propriétaire, avec cartographie de répertoires en mémoire + cache d'entrées + file d'écriture asynchrone
- **Stockage sur système de fichiers** — La version Default utilise le stockage JSON pur sur système de fichiers
- **Requêtes par index temporel** — Via l'interface `ITimeStorage`, requêtes efficaces par plage de dates
- **Compression automatique** — SpeedyPack prend en charge la compression automatique planifiée, récupérant l'espace inutilisé
- **Dépendances minimales** — La bibliothèque principale dépend uniquement de Microsoft.CodeAnalysis.CSharp pour la compilation dynamique

## 🔄 Architecture double version

Ce projet propose deux versions d'implémentation, répondant à différents scénarios :

### SiliconLife.Default (version par défaut)
- **Positionnement** : Implémentation par défaut, principalement utilisée pour valider la faisabilité de l'architecture
- **Mode d'exécution** : Application console
- **Mode de stockage** : Stockage JSON pur sur système de fichiers
- **Scénarios d'utilisation** : Exigences élevées de sécurité des données, ressources mémoire limitées, faible volume de données
- **Caractéristiques** : Simple et fiable, persistance instantanée des données, aucun risque de perte en mémoire
- **Rôle** : Implémentation de référence pour la validation de l'architecture, adaptée aux premiers contacts, au débogage de développement ou aux scénarios privilégiant la sécurité des données
- **Commande de lancement** : `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (version haute performance)
- **Positionnement** : Version de production recommandée
- **Mode d'exécution** : Application de bureau (barre d'état système Windows/macOS / fenêtre d'état Linux)
- **Mode de stockage** : Stockage en mémoire SpeedyPack + persistance par lots asynchrone (format de fichier .spk)
- **Scénarios d'utilisation** : Concurrence élevée, faible latence, grand volume de données
- **Support de plateforme** : Windows/macOS (fonctionnalités complètes, incluant la barre d'état système) ; Linux (fenêtre d'état, sans icône de barre d'état)
- **Caractéristiques** :
  - Optimisation de performance extrême
  - Fonctionnement en arrière-plan via la barre d'état Windows/macOS, avec surveillance en temps réel via la fenêtre d'état ; affichage direct de la fenêtre d'état sous Linux
  - Moteur SpeedyPack + compression automatique garantissant la sécurité des données
  - Architecture Component UI, 27 composants déclaratifs
  - 7 thèmes d'apparence, avec découverte automatique et changement
  - Prise en charge du rechargement à chaud pour les mises à jour et redémarrages en ligne
- **Amélioration des performances** : Latence de lecture réduite de 1000 fois, latence d'écriture réduite de 15000 fois, capacité de traitement concurrent améliorée de 50 fois
- **Rôle** : Implémentation de niveau production profondément optimisée, choix recommandé pour l'exécution à long terme et les environnements de production réels
- **Commande de lancement** : `dotnet run --project src/SiliconLife.Fast`

### Comparaison des versions

| Caractéristique | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **Mode d'exécution** | Application console | Application de bureau (barre d'état système Windows/macOS / fenêtre d'état Linux) |
| **Interface utilisateur** | Web UI (accès navigateur) | Windows/macOS : icône de barre d'état + fenêtre de barre d'état + Web UI ; Linux : fenêtre d'état + Web UI |
| **Barre d'état système** | ❌ Non | ✅ Windows/macOS prennent en charge la réduction dans la barre d'état ; Linux sans icône de barre d'état |
| **Exécution en arrière-plan** | ❌ Fermeture de la console = fermeture de l'application | ✅ Windows/macOS fonctionnement continu en arrière-plan via la barre d'état ; Linux via la fenêtre d'état |
| **Mode de stockage** | Stockage JSON sur système de fichiers | Stockage en mémoire SpeedyPack + persistance asynchrone |
| **Moteur de stockage** | E/S système de fichiers | SiliconLife.Speedy (format .spk) |
| **Latence de lecture** | ~10ms (E/S disque) | ~0.01ms (opération en mémoire) |
| **Latence d'écriture** | ~15ms (écriture synchrone) | ~0.001ms (écriture asynchrone) |
| **Capacité de concurrence** | ~100 req/s | ~5000 req/s |
| **Empreinte mémoire** | ~200MB | ~500MB |
| **Sécurité des données** | Très élevée (persistance instantanée) | Élevée (persistance asynchrone + compression automatique) |
| **Scénarios d'utilisation** | Sécurité des données prioritaire, faible volume de données | Performance prioritaire, grand volume de données, concurrence élevée |

## 🛠️ Stack technique

| Composant | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| Langage de programmation | C# | C# |
| Type d'application | Application console | Application de bureau (barre d'état système Windows/macOS / fenêtre d'état Linux) |
| Intégration IA | Ollama (local), Alibaba Cloud Bailian (cloud), Volcengine Ark (cloud) | Ollama (local), Alibaba Cloud Bailian (cloud), Volcengine Ark (cloud) |
| Stockage de données | Système de fichiers (JSON + répertoires d'index temporel) | SpeedyPack (format .spk, cartographie en mémoire + persistance asynchrone) |
| Serveur Web | HttpListener (intégré .NET) | HttpListener (intégré .NET) |
| Compilation dynamique | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automatisation de navigateur | Playwright (WebView) | Playwright (WebView) |
| Système de plugins | ✅ Pris en charge (IPlugin + PluginLoader) | ✅ Pris en charge (IPlugin + PluginLoader) |
| Barre d'état système | ❌ Non pris en charge | ✅ Windows/macOS pris en charge (NotifyIcon) ; Linux sans icône de barre d'état |
| Licence | Apache-2.0 | Apache-2.0 |

## 📁 Structure du projet

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Bibliothèque principale (interfaces, classes abstraites)
│   │   ├── AI/                            # Interfaces client IA, Gestionnaire de Contexte, modèles de messages
│   │   ├── Audit/                         # Système d'audit d'utilisation des tokens
│   │   ├── Chat/                          # Système de chat, gestion des sessions, Canal de Diffusion
│   │   ├── Compilation/                   # Compilation dynamique, Scanner de Sécurité, Chiffrement de Code
│   │   ├── Config/                        # Système de gestion de configuration
│   │   ├── Executors/                     # Exécuteurs (disque, réseau, ligne de commande)
│   │   ├── IM/                            # Interface du Fournisseur IM
│   │   ├── Knowledge/                     # Système de Réseau de Connaissances
│   │   ├── Localization/                  # Système de localisation
│   │   ├── Logging/                       # Système de journalisation
│   │   ├── Plugins/                       # Système de plugins (interface IPlugin, chargeur PluginLoader)
│   │   ├── Project/                       # Système de gestion de projets
│   │   ├── Runtime/                       # Boucle Principale, Objets Tick, Hôte Principal
│   │   ├── Security/                      # Système de gestion des autorisations
│   │   ├── SiliconBeing/                  # Classe de base Être de Silicium, Gestionnaire, Fabrique
│   │   ├── Storage/                       # Interfaces de stockage
│   │   ├── Time/                          # Date incomplète (requêtes par plage de temps)
│   │   ├── Tools/                         # Interface d'outil et Gestionnaire d'Outils
│   │   ├── WebView/                       # Interface du navigateur WebView
│   │   ├── Workflow/                      # Moteur de flux de travail (modèles, instances, transitions d'état)
│   │   └── ServiceLocator.cs              # Localisateur de services global
│   │
│   ├── SiliconLife.Common/                # Implémentations partagées (communes aux deux versions)
│   │   ├── AI/                            # Clients IA et fabriques (Ollama, DashScope, VolcengineArk)
│   │   ├── Calendar/                      # 32 implémentations de calendriers
│   │   ├── Localization/                  # Classe de base de localisation et 34 variantes linguistiques/régionales
│   │   ├── Resources/                     # Fichiers de ressources partagées
│   │   ├── Security/                      # Gestionnaire d'Autorisations
│   │   ├── SiliconBeing/                  # Implémentation par défaut de l'Être de Silicium
│   │   ├── Tools/                         # 23 implémentations d'outils communs
│   │   ├── Web/                           # Infrastructure Web
│   │   └── WebView/                       # Implémentation Playwright WebView
│   │
│   ├── SiliconLife.App/                   # Couche applicative (Web UI + documentation d'aide, partagée entre Default et Fast)
│   │   ├── Config/                        # Configuration de l'application
│   │   ├── Data/                          # Répertoire de données
│   │   ├── Help/                          # Localisation de la documentation d'aide (multilingue)
│   │   ├── Tools/                         # HelpTool (outil de recherche dans la documentation d'aide)
│   │   └── Web/                           # Implémentation Web UI
│   │       ├── Component/                 # Bibliothèque de composants UI (27 composants)
│   │       ├── Controllers/               # 24 contrôleurs
│   │       ├── Models/                    # Modèles de vue
│   │       ├── Views/                     # Vues HTML
│   │       └── Skins/                     # 7 thèmes d'apparence
│   │
│   ├── SiliconLife.Default/               # Implémentation par défaut + point d'entrée de l'application (version console)
│   │   ├── Program.cs                     # Point d'entrée (assemblage de tous les composants)
│   │   ├── Config/                        # Données de configuration par défaut
│   │   ├── Knowledge/                     # Implémentation du Réseau de Connaissances
│   │   ├── Logging/                       # Implémentation du Fournisseur de Journal (console + système de fichiers)
│   │   ├── Project/                       # Implémentation du système de projet
│   │   └── Storage/                       # Implémentation du stockage sur système de fichiers
│   │
│   ├── SiliconLife.Fast/                  # Implémentation haute performance + point d'entrée de l'application (version fenêtrée)
│   │   ├── Program.cs                     # Point d'entrée (application fenêtrée)
│   │   ├── App.axaml / App.cs             # Définition de l'application Avalonia
│   │   ├── Config/                        # Données de configuration (partagées avec Default)
│   │   ├── Knowledge/                     # Implémentation du Réseau de Connaissances (optimisation mémoire)
│   │   ├── Logging/                       # Fournisseur de journal haute performance
│   │   ├── Project/                       # Implémentation du système de projet
│   │   ├── Storage/                       # Adaptateurs de stockage SpeedyPack
│   │   └── Tray/                          # Barre d'état système (localisation en 34 variantes linguistiques)
│   │
│   ├── SiliconLife.Speedy/                # Moteur de stockage haute performance SpeedyPack
│   │   ├── SpeedyPack.cs                  # Classe principale (cartographie de répertoires en mémoire + cache + écriture asynchrone)
│   │   ├── SpeedyPackOptions.cs           # Options de configuration (TTL du cache, nombre max d'entrées, etc.)
│   │   ├── IPackTransaction.cs            # Interface de transaction
│   │   ├── SpkFileInfo.cs                 # Informations de fichier
│   │   └── Internal/                      # Implémentation interne
│   │       ├── DirectoryMap.cs            # Cartographie de répertoires en mémoire
│   │       ├── EntryCache.cs              # Cache d'entrées
│   │       ├── FreeList.cs                # Gestion de l'espace libre
│   │       ├── PackFileReader.cs          # Lecteur de fichiers de paquet
│   │       ├── PackFileWriter.cs          # Enregistreur de fichiers de paquet
│   │       ├── WriteQueue.cs              # File d'écriture asynchrone
│   │       ├── WriteOperation.cs          # Opération d'écriture
│   │       ├── SpeedyTransaction.cs       # Implémentation de transaction
│   │       ├── SpkHeader.cs               # En-tête de fichier de paquet
│   │       └── PathNormalizer.cs          # Normalisation de chemin
│   │
│   └── SiliconLife.Speedy.Manager/        # Outil de gestion SpeedyPack (Avalonia UI)
│       ├── MainForm.cs                    # Formulaire principal
│       ├── Program.cs                     # Point d'entrée
│       └── slc.ico                        # Icône de l'application
│
├── docs/                                  # Documentation multilingue
│   ├── zh-CN/                             # Documentation en chinois simplifié
│   ├── en/                                # Documentation en anglais
│   └── ...                                # Documentation dans d'autres langues
│
└── 总文档/                                 # Documents de besoins et d'architecture
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ Aperçu de l'architecture

### Architecture d'ordonnancement
```
Boucle Principale (thread dédié, Chien de Garde + Disjoncteur)
  └── Objet Tick (trié par priorité)
       └── Gestionnaire d'Êtres de Silicium
            └── Exécuteur d'Être de Silicium (thread temporaire, timeout + Disjoncteur)
                 └── Être de Silicium.Tick()
                      └── Gestionnaire de Contexte.Penser()
                           └── Client IA.Chat()
                                └── Boucle d'appel d'outils → Persistance dans le Système de Chat
```

### Architecture de sécurité
Toutes les opérations d'E/S initiées par l'IA doivent passer par une chaîne de sécurité stricte :

```
Appel d'outil → Exécuteur → Gestionnaire d'Autorisations → [Cache de fréquence → Rappel → (IsCurateur : Interroger l'utilisateur | Non-curateur : ACL Global)]
```

## 🚀 Démarrage rapide

### Prérequis

- **.NET 9 SDK** — [Lien de téléchargement](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Backend IA** (choisir l'un des trois) :
  - **Ollama** : [Installer Ollama](https://ollama.com) et tirer un modèle (par exemple `ollama pull llama3`)
  - **Alibaba Cloud Bailian** : Obtenir une clé API depuis la [console Bailian](https://bailian.console.aliyun.com/)
  - **Volcengine Ark** : Obtenir une clé API depuis la [console Volcengine](https://console.volcengine.com/ark)

### Construire le projet

```bash
dotnet restore
dotnet build
```

### Exécuter le système

#### Méthode 1 : Exécuter la version Default (application console)

```bash
dotnet run --project src/SiliconLife.Default
```

L'application démarrera le serveur Web et ouvrira automatiquement l'interface Web UI dans le navigateur.

**Scénarios d'utilisation** :
- ✅ Exigences de sécurité des données très élevées
- ✅ Ressources mémoire limitées (RAM < 2 Go)
- ✅ Faible volume de données, utilisation à court terme
- ✅ Phase de développement et de débogage

#### Méthode 2 : Exécuter la version Fast (application de bureau)

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS** : L'application démarrera en mode fenêtré, se réduira dans la barre d'état système et fonctionnera en arrière-plan en continu.

**Linux** : L'application affichera une fenêtre d'état (sans icône de barre d'état système) et ouvrira automatiquement le navigateur pour accéder à l'interface Web UI. Vous pouvez également utiliser le paramètre `--no-tray` pour éviter l'ouverture automatique du navigateur :

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

**Scénarios d'utilisation** :
- ✅ Scénarios de haute concurrence (> 5 utilisateurs)
- ✅ Grand volume de données (utilisation de plus de 3 mois)
- ✅ Besoin de réponses à faible latence
- ✅ Besoin de fonctionnement en arrière-plan via la barre d'état système

### Publication en fichier unique

```bash
# Windows - Version Default
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Version Fast
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - Version Default
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux - Version Fast
dotnet publish src/SiliconLife.Fast -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Version Default
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS - Version Fast
dotnet publish src/SiliconLife.Fast -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Feuille de route de développement

### ✅ Terminé
- [x] Phase 1 : Chat IA en console
- [x] Phase 2 : Squelette du framework (Boucle Principale + Objets Tick + Chien de Garde + Disjoncteur)
- [x] Phase 3 : Premier Être de Silicium avec Fichier d'Âme (architecture Corps-Cerveau)
- [x] Phase 4 : Mémoire persistante (Système de Chat + Interface de Stockage Temporel)
- [x] Phase 5 : Système d'outils + Exécuteurs
- [x] Phase 6 : Système d'autorisations (chaîne à 5 niveaux, journal d'audit, ACL Global)
- [x] Phase 7 : Compilation dynamique + Auto-évolution (Roslyn)
- [x] Phase 8 : Mémoire à long terme + Tâches + Minuteurs
- [x] Phase 9 : Hôte Principal + Collaboration multi-agents
- [x] Phase 10 : Web UI (HTTP + SSE, 24 contrôleurs, 7 thèmes)
- [x] Phase 10.5 : Améliorations incrémentales (Canal de Diffusion, Audit de Tokens, 32 calendriers, amélioration des outils, localisation en 34 variantes linguistiques)
- [x] Phase 10.6 : Perfectionnement et optimisation (WebView, système d'aide, espace projet, Réseau de Connaissances, moteur de flux de travail)
- [x] Phase 11 : Moteur de stockage SpeedyPack (remplacement de LiteDB, cartographie en mémoire, file d'écriture asynchrone, compression automatique)
- [x] Phase 12 : Système de plugins (interface IPlugin, Sandbox de sécurité PluginLoader, chargement isolé, intégration d'outils)

### 🚧 Planifié
- [ ] Phase 13 : Intégration de messagerie instantanée externe (Feishu / WhatsApp / Telegram)
- [ ] Phase 14 : Écosystème de compétences (place de marché de plugins, distribution de packs de compétences)

## 📚 Documentation

- [Conception de l'architecture](architecture.md) — Conception du système, mécanisme d'ordonnancement, architecture des composants
- [Modèle de sécurité](security.md) — Modèle d'autorisations, exécuteurs, sécurité de la compilation dynamique
- [Guide de développement](development-guide.md) — Développement d'outils, guide d'extension
- [Référence API](api-reference.md) — Documentation des points de terminaison de l'API Web
- [Référence des outils](tools-reference.md) — Description détaillée des outils intégrés
- [Guide de l'interface Web UI](web-ui-guide.md) — Guide d'utilisation de l'interface Web
- [Guide de l'Être de Silicium](silicon-being-guide.md) — Guide de développement des agents
- [Système d'autorisations](permission-system.md) — Explication détaillée de la gestion des autorisations
- [Système de calendriers](calendar-system.md) — Description des 32 systèmes calendaires
- [Démarrage rapide](getting-started.md) — Guide d'introduction détaillé
- [Dépannage](troubleshooting.md) — Foire aux questions
- [Feuille de route](roadmap.md) — Plan de développement complet
- [Journal des modifications](changelog.md) — Historique des mises à jour de version
- [Guide de contribution](contributing.md) — Comment participer au projet

## 🤝 Contribuer

Nous accueillons toutes les formes de contribution ! Consultez le [guide de contribution](contributing.md) pour plus de détails.

### Flux de travail de développement
1. Forker ce dépôt
2. Créer une branche de fonctionnalité (`git checkout -b feature/AmazingFeature`)
3. Commiter les modifications (`git commit -m 'feat: add some AmazingFeature'`)
4. Pousser vers la branche (`git push origin feature/AmazingFeature`)
5. Soumettre une Pull Request

## 💡 Guide de choix de version

### Quelle version devrais-je utiliser ?

**SiliconLife.Default (implémentation par défaut — validation de la faisabilité de l'architecture) :**
- 📌 Vous découvrez ce projet pour la première fois et souhaitez comprendre rapidement l'architecture du système
- 📌 Vous êtes en phase de développement et de débogage, nécessitant un mode d'exécution simple et direct
- 📌 La sécurité des données est votre priorité
- 📌 Votre système dispose de moins de 4 Go de mémoire
- 📌 Vous avez besoin d'une utilisation mono-utilisateur ou d'un faible volume de données

**SiliconLife.Fast (version de production recommandée) :**
- ⚡ Vous avez besoin d'un environnement de production fonctionnant de manière stable à long terme
- ⚡ Vous êtes déjà familier avec l'architecture du système et prêt pour un déploiement en production
- ⚡ Vous devez prendre en charge l'accès concurrentiel multi-utilisateurs
- ⚡ Vous avez besoin d'un fonctionnement en arrière-plan via la barre d'état système
- ⚡ Vous recherchez l'expérience de performance ultime

> **Recommandation générale** : SiliconLife.Default convient comme validation d'architecture et expérience d'initiation ; pour les environnements de production réels, l'utilisation de SiliconLife.Fast est fortement recommandée.

### Peut-on migrer de Default à Fast ?

**Absolument !** Les deux versions partagent les mêmes éléments :
- ✅ Format des fichiers de configuration (config.json)
- ✅ Interfaces d'outils
- ✅ Configuration des Êtres (Being)
- ✅ Interface Web UI

**Étapes de migration :**
1. Sauvegarder votre répertoire de données Default
2. Démarrer la version Fast avec le même répertoire de données
3. Fast importera automatiquement les données existantes dans le moteur de stockage SpeedyPack
4. Après vérification du bon fonctionnement, vous pouvez utiliser la version Fast au quotidien

### Les deux versions peuvent-elles coexister ?

**Oui !** La stratégie de déploiement suivante est recommandée :

**Stratégie 1 : Default pour la validation, Fast pour la production**
```
Environnement de développement/validation : SiliconLife.Default (validation de l'architecture, débogage des fonctionnalités)
Environnement de production : SiliconLife.Fast (haute performance, fonctionnement en arrière-plan, traitement des requêtes en temps réel)
```

**Stratégie 2 : Fast pour l'exécution principale, Default pour les sauvegardes régulières**
```
SiliconLife.Fast (utilisation quotidienne, traitement des requêtes en temps réel)
    ↓ Sauvegarde régulière
SiliconLife.Default (archivage des données froides, sécurité des données en dernier recours)
```

## 📄 Licence

Ce projet est sous licence Apache License 2.0 — Voir le fichier [LICENSE](../../LICENSE) pour plus de détails.

## 👨‍💻 Auteur

**天源垦骥**

- GitHub : [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee : [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube : [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili : [617827040](https://space.bilibili.com/617827040)

## 🙏 Remerciements

Merci à tous les développeurs et fournisseurs de plateformes IA ayant contribué à ce projet.

---

**Silicon Life Collective** — Faire en sorte que les agents IA « vivent » véritablement
