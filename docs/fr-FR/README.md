![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Version : v0.2.0-alpha** | **Silicon Life Collective** — Une plateforme de collaboration multi-agents basée sur .NET 9, où les agents IA sont appelés **Silicon Beings** et peuvent s'auto-développer grâce à la compilation dynamique Roslyn.

[English](../README.md) | [Deutsch](../de-DE/README.md) | **Français** | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md) | [Русский](../ru-RU/README.md)

## 🌟 Fonctionnalités clés

### Système d'agents
- **Orchestration multi-agents** — Gestion centralisée par le *Silicon Curator*, avec mécanisme d'ordonnancement équitable par time-slice contrôlé par horloge
- **Piloté par fichier âme** — Chaque Silicon Being est contrôlé par un fichier prompt central (`soul.md`) qui définit une personnalité unique et des modèles de comportement
- **Architecture Body-Brain** — Le *Body* (SiliconBeing) reçoit les signes vitaux et détecte les scénarios de déclenchement ; le *Brain* (ContextManager) est responsable du chargement de l'historique, des appels IA, de l'exécution des outils et de la persistance des réponses
- **Capacité d'auto-développement** — Grâce à la technologie de compilation dynamique Roslyn, les Silicon Beings peuvent réécrire leur propre code pour réaliser une évolution
- **Gestion des états d'activité** — Prise en charge de quatre états d'activité : Idle (inactif), Working (en travail), Error (erreur), Stopped (arrêté). Entrée automatique dans l'état Stopped après 10 erreurs consécutives

### Système de plugins
- **Architecture d'extension par plugins** — Extension de fonctionnalités via l'interface IPlugin, prend en charge le chargement dynamique de DLL de plugins depuis un répertoire
- **Bac à sable sécurisé** — Le chargeur de plugins effectue des analyses de sécurité strictes, interdit l'accès à System.IO, System.Net et autres espaces de noms
- **Chargement isolé** — Utilisation d'un AssemblyLoadContext personnalisé pour un chargement isolé, empêchant les plugins de compromettre la stabilité du programme principal
- **Intégration d'outils** — Les plugins peuvent enregistrer des outils personnalisés via l'interface ITool, automatiquement intégrés dans le cycle d'appel d'outils

### Outils & Exécution
- **24 outils intégrés** — Couvrant calendrier, chat, configuration, disque, réseau, mémoire, tâches, minuteries, base de connaissances, notes de travail, navigateur WebView, rechargement à chaud, etc.
- **Outil de rechargement à chaud** — Prend en charge la compilation automatique, la mise à jour des fichiers et le redémarrage de SiliconLife.Fast pendant l'exécution, sans intervention manuelle
- **Boucle d'appel d'outils** — L'IA retourne un appel d'outil → Exécuter l'outil → Retourner les résultats à l'IA → Continuer la boucle jusqu'à une réponse en texte pur
- **Sécurité des permissions de l'exécuteur** — Toutes les opérations E/S passent par une validation de permissions stricte via les exécuteurs
  - Chaîne de permissions à 5 niveaux : UserFrequencyCache → IPermissionCallback → (Curateur→IPermissionAskHandler / NonCurateur→GlobalACL→Deny)
  - Journalisation d'audit complète de toutes les décisions de permissions

### IA & Connaissances
- **Prise en charge de multiples backends IA**
  - **Ollama** — Déploiement de modèles locaux, avec API HTTP native
  - **Alibaba Cloud DashScope (Bailian)** — Service IA cloud, compatible API OpenAI, prend en charge 13+ modèles, déploiement multi-région
  - **Volcengine Ark (VolcengineArk)** — Service IA cloud de ByteDance, prend en charge les modes streaming et non-streaming, contrôle de vitesse intégré
- **32 systèmes de calendrier** — Couverture complète des principaux calendriers mondiaux, incluant calendrier grégorien, calendrier lunaire chinois, calendrier islamique, calendrier hébraïque, calendrier japonais, calendrier persan, calendrier maya, calendrier historique chinois, etc.
- **Système de réseau de connaissances** — Graphe de connaissances basé sur des triplets (sujet-relation-objet), prend en charge le stockage, les requêtes et la découverte de chemins

### Interface Web
- **Interface Web moderne** — Serveur HTTP intégré avec mises à jour en temps réel SSE
- **7 thèmes d'apparence** — Versions Admin, Chat, Creative, Dev, Contraste élevé, Light, Minimal, prend en charge la détection et la commutation automatiques
- **23 contrôleurs** — Gestion complète du système, chat, configuration, fonctionnalités de surveillance
- **Zéro dépendance framework frontend** — HTML/CSS/JS générés côté serveur via `H`, `CssBuilder` et `JsBuilder`

### Internationalisation & Localisation
- **33 variantes linguistiques** en prise en charge complète, couvrant 2 systèmes d'écriture et multiples variantes régionales
  - **Chinois simplifié** : zh-CN (Chine continentale), zh-SG (Singapour), zh-MY (Malaisie) (3 variantes)
  - **Chinois traditionnel** : zh-HK (Hong Kong), zh-TW (Taïwan), zh-MO (Macao) (3 variantes)
  - **Anglais** : en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variantes)
  - **Espagnol** : es-ES, es-MX (2 variantes)
  - **Allemand** : de-DE, de-AT, de-CH, de-LU, de-LI (5 variantes)
  - **Français** : fr-FR, fr-CA, fr-CH (3 variantes)
  - **Japonais** : ja-JP | **Coréen** : ko-KR | **Tchèque** : cs-CZ (3 variantes)
  - **Italien** : it-IT | **Polonais** : pl-PL | **Portugais** : pt-PT, pt-BR (4 variantes)

### Données & Stockage
- **Stockage haute performance SpeedyPack** — La version Fast utilise le moteur de stockage .spk propriétaire, mappage de répertoires en mémoire + cache d'entrées + file d'écriture asynchrone
- **Stockage en système de fichiers** — La version Default utilise un stockage pur en système de fichiers JSON
- **Requêtes par index temporel** — Requêtes efficaces par plage de temps via l'interface `ITimeStorage`
- **Compression automatique** — SpeedyPack prend en charge la compression automatique planifiée pour récupérer l'espace de stockage
- **Dépendances minimales** — La bibliothèque principale dépend uniquement de Microsoft.CodeAnalysis.CSharp pour la compilation dynamique

## 🔄 Architecture en double version

Ce projet propose deux versions d'implémentation pour répondre à différents besoins de scénarios :

### SiliconLife.Default (Version standard)
- **Positionnement** : Implémentation standard, principalement pour la vérification de faisabilité architecturale
- **Mode d'exécution** : Application console
- **Méthode de stockage** : Stockage JSON pur en système de fichiers
- **Scénarios applicables** : Exigences élevées de sécurité des données, ressources mémoire limitées, petit volume de données
- **Caractéristiques** : Simple et fiable, persistance immédiate des données, aucun risque de perte de données
- **Description du rôle** : Implémentation de référence pour la vérification architecturale, adaptée au premier contact, au débogage de développement ou aux scénarios prioritaires en sécurité des données
- **Commande de démarrage** : `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (Version haute performance)
- **Positionnement** : Version principale de production
- **Mode d'exécution** : Application de bureau (Windows/macOS barre d'état système / Linux fenêtre d'état)
- **Méthode de stockage** : Stockage en mémoire SpeedyPack + persistance par lot asynchrone (format de fichier .spk)
- **Scénarios applicables** : Concurrence élevée, faible latence, grands volumes de données
- **Prise en charge des plateformes** : Windows/macOS (fonctionnalités complètes, y compris barre d'état système), Linux (fenêtre d'état, pas d'icône dans la barre d'état)
- **Caractéristiques** :
  - Optimisation de performance extrême
  - Windows/macOS exécution en arrière-plan dans la barre d'état avec surveillance en temps réel ; Linux fenêtre d'état affichée directement
  - Moteur SpeedyPack + compression automatique garantissant la sécurité des données
    - Architecture Component UI, 27 composants déclaratifs
  - 7 thèmes d'apparence, prend en charge la détection et la commutation automatiques
  - Outil de rechargement à chaud pour les mises à jour et redémarrages en ligne
- **Amélioration des performances** : Latence de lecture mémoire réduite de 1000x, latence d'écriture réduite de 15000x, capacité de traitement parallèle augmentée de 50x
- **Description du rôle** : Implémentation prête pour la production avec optimisation approfondie, le meilleur choix pour l'exploitation à long terme et les véritables environnements de production
- **Commande de démarrage** : `dotnet run --project src/SiliconLife.Fast`

### Comparaison des versions

| Caractéristique | SiliconLife.Default | SiliconLife.Fast |
|---------|---------------------|------------------|
| **Mode d'exécution** | Application console | Application de bureau (Windows/macOS barre d'état système / Linux fenêtre d'état) |
| **Interface utilisateur** | Interface Web (accès navigateur) | Windows/macOS : Icône de barre d'état + Fenêtre de barre d'état + Interface Web ; Linux : Fenêtre d'état + Interface Web |
| **Barre d'état système** | ❌ Non | ✅ Windows/macOS prend en charge la minimisation dans la barre d'état ; Linux pas d'icône dans la barre d'état |
| **Exécution en arrière-plan** | ❌ Se termine à la fermeture de la console | ✅ Windows/macOS exécution continue en arrière-plan dans la barre d'état ; Linux exécution dans la fenêtre d'état |
| **Méthode de stockage** | Stockage JSON en système de fichiers | Stockage en mémoire SpeedyPack + persistance asynchrone |
| **Moteur de stockage** | E/S système de fichiers | SiliconLife.Speedy (format .spk) |
| **Latence de lecture** | ~10ms (E/S disque) | ~0.01ms (opération mémoire) |
| **Latence d'écriture** | ~15ms (écriture synchrone) | ~0.001ms (écriture asynchrone) |
| **Concurrence** | ~100 req/s | ~5000 req/s |
| **Utilisation mémoire** | ~200Mo | ~500Mo |
| **Sécurité des données** | Extrêmement élevée (persistance immédiate) | Élevée (persistance asynchrone + compression automatique) |
| **Scénarios applicables** | Sécurité des données prioritaire, petites données | Performance prioritaire, grandes données, haute concurrence |

## 🛠️ Stack technologique

| Composant | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| Langage de programmation | C# | C# |
| Type d'application | Application console | Application de bureau (Windows/macOS barre d'état système / Linux fenêtre d'état) |
| Intégration IA | Ollama (local), Alibaba Cloud DashScope (cloud) | Ollama (local), Alibaba Cloud DashScope (cloud), Volcengine Ark (cloud) |
| Stockage de données | Système de fichiers (JSON + répertoire d'index temporel) | SpeedyPack (format .spk, mappage en mémoire + persistance asynchrone) |
| Serveur Web | HttpListener (intégré .NET) | HttpListener (intégré .NET) |
| Compilation dynamique | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automatisation de navigateur | Playwright (WebView) | Playwright (WebView) |
| Système de plugins | ✅ Pris en charge (IPlugin + PluginLoader) | ✅ Pris en charge (IPlugin + PluginLoader) |
| Barre d'état système | ❌ Non pris en charge | ✅ Windows/macOS pris en charge (NotifyIcon) ; Linux pas d'icône dans la barre d'état |
| Licence | Apache-2.0 | Apache-2.0 |

## 📁 Structure du projet

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Bibliothèque principale (interfaces, classes abstraites)
│   │   ├── AI/                            # Interfaces client IA, gestionnaire de contexte, modèles de messages
│   │   ├── Audit/                         # Système d'audit d'utilisation des tokens
│   │   ├── Chat/                          # Système de chat, gestion de sessions, canaux de diffusion
│   │   ├── Compilation/                   # Compilation dynamique, analyse de sécurité, chiffrement de code
│   │   ├── Config/                        # Système de gestion de configuration
│   │   ├── Executors/                     # Exécuteurs (disque, réseau, ligne de commande)
│   │   ├── IM/                            # Interfaces de fournisseurs de messagerie instantanée
│   │   ├── Knowledge/                     # Système de réseau de connaissances
│   │   ├── Localization/                  # Système de localisation
│   │   ├── Logging/                       # Système de journalisation
│   │   ├── Plugins/                       # Système de plugins (interface IPlugin, PluginLoader)
│   │   ├── Project/                       # Système de gestion de projets
│   │   ├── Runtime/                       # Boucle principale, objets horloge, hôte principal
│   │   ├── Security/                      # Système de gestion des permissions
│   │   ├── SiliconBeing/                  # Classe de base Silicon Being, gestionnaire, fabrique
│   │   ├── Storage/                       # Interfaces de stockage
│   │   ├── Time/                          # Dates incomplètes (requêtes par plage de temps)
│   │   ├── Tools/                         # Interfaces d'outils et gestionnaire d'outils
│   │   ├── WebView/                       # Interfaces du navigateur WebView
│   │   └── ServiceLocator.cs              # Localisateur de services global
│   │
│   ├── SiliconLife.Common/                # Implémentation commune (les deux versions)
│   │   ├── AI/                            # Clients IA et fabriques (Ollama, DashScope, VolcengineArk)
│   │   ├── Calendar/                      # 32 implémentations de calendriers
│   │   ├── Localization/                  # Classe de base de localisation et 33 variantes linguistiques/régionales
│   │   ├── Resources/                     # Fichiers de ressources partagés
│   │   ├── Security/                      # Gestionnaire de permissions
│   │   ├── SiliconBeing/                  # Implémentation standard du Silicon Being
│   │   ├── Tools/                         # 24 outils communs (dont outil de rechargement à chaud)
│   │   ├── Web/                           # Infrastructure Web
│   │   └── WebView/                       # Implémentation Playwright WebView
│   │
│   ├── SiliconLife.App/                   # Couche applicative (Web UI + aide, partagée entre Default et Fast)
│   │   ├── Config/                        # Configuration applicative
│   │   ├── Data/                          # Répertoire de données
│   │   ├── Help/                          # Localisation de la documentation d'aide (multilingue)
│   │   └── Web/                           # Implémentation de l'interface Web
│   │       ├── Component/                 # Bibliothèque de composants UI (27 composants)
│   │       ├── Controllers/               # 23 contrôleurs
│   │       ├── Models/                    # Modèles de vue
│   │       ├── Views/                     # Vues HTML
│   │       └── Skins/                     # 7 thèmes d'apparence
│   │
│   ├── SiliconLife.Default/               # Implémentation standard + point d'entrée (version console)
│   │   ├── Program.cs                     # Point d'entrée (assemblage de tous les composants)
│   │   ├── Config/                        # Données de configuration standard
│   │   ├── IM/                            # Fournisseur WebUI
│   │   ├── Knowledge/                     # Implémentation du réseau de connaissances
│   │   ├── Logging/                       # Implémentations des fournisseurs de journalisation
│   │   ├── Project/                       # Implémentation du système de projets
│   │   ├── Security/                      # Rappels de permissions standard
│   │   ├── Storage/                       # Implémentation du stockage en système de fichiers
│   │   └── Tools/                         # Outils spécifiques à la version (HelpTool)
│   │
│   ├── SiliconLife.Fast/                  # Implémentation haute performance + point d'entrée (version Forms)
│   │   ├── Program.cs                     # Point d'entrée (application Forms)
│   │   ├── Config/                        # Données de configuration (partagées avec Default)
│   │   ├── IM/                            # Fournisseur WebUI
│   │   ├── Knowledge/                     # Implémentation du réseau de connaissances (optimisée mémoire)
│   │   ├── Logging/                       # Fournisseurs de journalisation haute performance
│   │   ├── Project/                       # Implémentation du système de projets
│   │   ├── Security/                      # Rappels de permissions optimisés
│   │   ├── Storage/                       # Adaptateur de stockage SpeedyPack
│   │   ├── Tools/                         # Outils spécifiques à la version (HelpTool)
│   │   └── Tray/                          # Barre d'état système (33 variantes linguistiques)
│   │
│   ├── SiliconLife.Speedy/                # Moteur de stockage haute performance SpeedyPack
│   │   ├── SpeedyPack.cs                  # Classe principale (mappage de répertoires en mémoire + cache + écriture asynchrone)
│   │   ├── SpeedyPackOptions.cs           # Options de configuration (TTL du cache, max entrées, etc.)
│   │   ├── IPackTransaction.cs            # Interface de transaction
│   │   ├── SpkFileInfo.cs                 # Informations sur les fichiers
│   │   └── Internal/                      # Implémentation interne
│       │   ├── DirectoryMap.cs            # Mappage de répertoires en mémoire
│       │   ├── EntryCache.cs              # Cache d'entrées
│       │   ├── FreeList.cs                # Gestion de l'espace libre
│       │   ├── PackFileReader.cs          # Lecteur de fichiers paquet
│       │   ├── PackFileWriter.cs          # Écrivain de fichiers paquet
│       │   ├── WriteQueue.cs              # File d'écriture asynchrone
│       │   ├── WriteOperation.cs          # Opération d'écriture
│       │   ├── SpeedyTransaction.cs       # Implémentation de transaction
│       │   ├── SpkHeader.cs              # En-tête de fichier paquet
│       │   └── PathNormalizer.cs          # Normalisation de chemin
│   │
│   └── SiliconLife.Speedy.Manager/        # Outil de gestion SpeedyPack (Avalonia UI)
│       ├── MainWindow.axaml.cs           # Fenêtre principale
│       ├── App.axaml.cs                  # Point d'entrée de l'application
│       ├── Program.cs                     # Point d'entrée
│       ├── ProgressWindow.axaml.cs       # Fenêtre de progression
│       ├── ContentViewerWindow.axaml.cs  # Fenêtre de visualisation de contenu
│       └── slc.ico                        # Icône de l'application
│
├── docs/                                  # Documentation multilingue
│   ├── zh-CN/                             # Chinois simplifié
│   ├── en/                                # Anglais
│   └── ...                                # Autres langues
```

## 🏗️ Aperçu de l'architecture

### Architecture d'ordonnancement
```
Boucle principale (thread dédié, watchdog + disjoncteur)
  └── Objet horloge (trié par priorité)
       └── Gestionnaire de Silicon Beings
            └── Exécuteur de Silicon Being (thread temporaire, timeout + disjoncteur)
                 └── SiliconBeing.Tick()
                      └── ContextManager.Penser()
                           └── Client-IA.Chat()
                                └── Boucle d'appel d'outils → Persistance dans le système de chat
```

### Architecture de sécurité
Toutes les opérations E/S initiées par l'IA doivent traverser une chaîne de sécurité stricte :

```
Appel d'outil → Exécuteur → Gestionnaire de permissions → [Cache de fréquence → Rappel → (Curateur : Demande utilisateur | Non-curateur : GlobalACL)]
```

## 🚀 Démarrage rapide

### Prérequis

- **.NET 9 SDK** — [Lien de téléchargement](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Backend IA** (choisissez-en un) :
  - **Ollama** : [Installer Ollama](https://ollama.com) et récupérer un modèle (ex. `ollama pull llama3`)
  - **Alibaba Cloud DashScope** : Obtenir une clé API depuis la [console DashScope](https://bailian.console.aliyun.com/)
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

L'application démarre le serveur Web et ouvre automatiquement l'interface Web dans le navigateur.

**Scénarios applicables** :
- ✅ Exigences de sécurité des données extrêmement élevées
- ✅ Ressources mémoire limitées (RAM < 2Go)
- ✅ Petit volume de données, utilisation à court terme
- ✅ Phase de développement et de débogage

#### Méthode 2 : Exécuter la version Fast (Application de bureau)

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS** : L'application démarre en mode fenêtre, se minimise dans la barre d'état système et continue de fonctionner en arrière-plan.

**Linux** : L'application affiche une fenêtre d'état (pas d'icône dans la barre d'état système) et ouvre automatiquement le navigateur pour accéder à l'interface Web. Vous pouvez utiliser le paramètre `--no-tray` pour ignorer l'ouverture automatique du navigateur :

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

**Scénarios applicables** :
- ✅ Scénarios hautement parallèles (> 5 utilisateurs)
- ✅ Grands volumes de données (utilisation sur plus de 3 mois)
- ✅ Exigences de faible latence
- ✅ Exécution en arrière-plan dans la barre d'état requise

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
- [x] Phase 2 : Squelette du framework (boucle principale + objets horloge + watchdog + disjoncteur)
- [x] Phase 3 : Premier Silicon Being avec fichier âme (architecture Body-Brain)
- [x] Phase 4 : Stockage persistant (système de chat + interface Time Storage)
- [x] Phase 5 : Système d'outils + exécuteurs
- [x] Phase 6 : Système de permissions (chaîne à 3 niveaux en structure ramifiée, journal d'audit, GlobalACL)
- [x] Phase 7 : Compilation dynamique + auto-développement (Roslyn)
- [x] Phase 8 : Stockage à long terme + tâches + minuteries
- [x] Phase 9 : Hôte principal + collaboration multi-agents
- [x] Phase 10 : Interface Web (HTTP + SSE, 23 contrôleurs, 7 thèmes)
- [x] Phase 10.5 : Améliorations incrémentales (canaux de diffusion, audit de tokens, 32 calendriers, améliorations d'outils, localisation en 33 variantes linguistiques)
- [x] Phase 10.6 : Complétion & Optimisation (WebView, système d'aide, espace de projet, réseau de connaissances)
- [x] Phase 11 : Moteur de stockage SpeedyPack (remplacement de LiteDB, mappage en mémoire, file d'écriture asynchrone, compression automatique)
- [x] Phase 12 : Système de plugins (interface IPlugin, bac à sable de sécurité PluginLoader, chargement isolé, intégration d'outils)

### 🚧 Planifié
- [ ] Phase 13 : Intégration IM externe (Feishu / WhatsApp / Telegram)
- [ ] Phase 14 : Écosystème de compétences (marketplace de plugins, distribution de packs de compétences)

## 📚 Documentation

- [Conception architecturale](architecture.md) — Conception système, mécanismes d'ordonnancement, architecture des composants
- [Modèle de sécurité](security.md) — Modèle de permissions, exécuteurs, sécurité de la compilation dynamique
- [Guide de développement](development-guide.md) — Développement d'outils, guide d'extension
- [Référence API](api-reference.md) — Documentation des endpoints API Web
- [Référence des outils](tools-reference.md) — Description détaillée des outils intégrés
- [Guide de l'interface Web](web-ui-guide.md) — Guide utilisateur de l'interface Web
- [Guide du Silicon Being](silicon-being-guide.md) — Guide de développement d'agents
- [Système de permissions](permission-system.md) — Gestion des permissions en détail
- [Système de calendrier](calendar-system.md) — Description des 32 systèmes de calendrier
- [Démarrage rapide](getting-started.md) — Guide détaillé pour les débutants
- [Dépannage](troubleshooting.md) — Questions fréquentes
- [Feuille de route](roadmap.md) — Plan de développement complet
- [Journal des modifications](changelog.md) — Historique des mises à jour de version
- [Guide de contribution](contributing.md) — Comment participer au projet

## 💡 Guide de choix de version

### Quelle version utiliser ?

**SiliconLife.Default (implémentation standard — vérification de faisabilité architecturale) :**
- 📌 Vous découvrez ce projet pour la première fois et souhaitez comprendre rapidement l'architecture système
- 📌 Vous êtes en phase de développement et de débogage et avez besoin d'une méthode d'exécution simple et directe

**SiliconLife.Fast (version haute performance — utilisation en production) :**
- 📌 Vous avez besoin d'une exploitation à long terme et de hautes performances
- 📌 Vous avez de grands volumes de données ou des exigences de haute concurrence
- 📌 Vous souhaitez que l'application fonctionne en arrière-plan dans la barre d'état système

## 🤝 Contribuer

Nous accueillons toutes les contributions ! Veuillez lire le [guide de contribution](contributing.md) pour plus de détails.

### Flux de travail de développement
1. Forker ce dépôt
2. Créer une branche de fonctionnalité (`git checkout -b feature/AmazingFeature`)
3. Commiter les modifications (`git commit -m 'feat: add some AmazingFeature'`)
4. Pousser vers la branche (`git push origin feature/AmazingFeature`)
5. Soumettre une Pull Request

## 📄 Licence

Ce projet est sous licence Apache License 2.0 — voir le fichier [LICENSE](../../LICENSE).

## 👨‍💻 Auteur

**Hoshino Kennji**

- GitHub : [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee : [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube : [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili : [617827040](https://space.bilibili.com/617827040)

## 🙏 Remerciements

Merci à tous les développeurs et fournisseurs de plateformes IA qui ont contribué à ce projet.

---

**Silicon Life Collective** — Rendre les agents IA véritablement « vivants »
