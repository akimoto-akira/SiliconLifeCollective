![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Version : v0.1.0-alpha** | **Silicon Life Collective** — Une plateforme de collaboration multi-agents basée sur .NET 9, où les agents IA sont appelés **Silicon Beings** et peuvent s'auto-développer grâce à la compilation dynamique Roslyn.

[English](../README.md) | [Deutsch](../de-DE/README.md) | **Français** | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Čeština](../cs-CZ/README.md)

## 🌟 Fonctionnalités clés

### Système d'agents
- **Orchestration multi-agents** — Gestion centralisée par le *Silicon Curator*, avec mécanisme d'ordonnancement équitable par time-slice contrôlé par horloge
- **Piloté par fichier âme** — Chaque Silicon Being est contrôlé par un fichier prompt central (`soul.md`) qui définit une personnalité unique et des modèles de comportement
- **Architecture Body-Brain** — Le *Body* (SiliconBeing) reçoit les signes vitaux et détecte les scénarios de déclenchement ; le *Brain* (ContextManager) est responsable du chargement de l'historique, des appels IA, de l'exécution des outils et de la persistance des réponses
- **Capacité d'auto-développement** — Grâce à la technologie de compilation dynamique Roslyn, les Silicon Beings peuvent réécrire leur propre code pour réaliser une évolution

### Système de plugins
- **Architecture d'extension par plugins** — Extension de fonctionnalités via l'interface IPlugin, prend en charge le chargement dynamique de DLL de plugins depuis un répertoire
- **Bac à sable sécurisé** — Le chargeur de plugins effectue des analyses de sécurité strictes, interdit l'accès à System.IO, System.Net et autres espaces de noms
- **Chargement isolé** — Utilisation d'un AssemblyLoadContext personnalisé pour un chargement isolé, empêchant les plugins de compromettre la stabilité du programme principal
- **Intégration d'outils** — Les plugins peuvent enregistrer des outils personnalisés via l'interface ITool, automatiquement intégrés dans le cycle d'appel d'outils

### Outils & Exécution
- **23 outils intégrés** — Couvrant calendrier, chat, configuration, disque, réseau, mémoire, tâches, minuteries, base de connaissances, notes de travail, navigateur WebView, etc.
- **Boucle d'appel d'outils** — L'IA retourne un appel d'outil → Exécuter l'outil → Retourner les résultats à l'IA → Continuer la boucle jusqu'à une réponse en texte pur
- **Sécurité des permissions de l'exécuteur** — Toutes les opérations E/S passent par une validation de permissions stricte via les exécuteurs
  - Chaîne de permissions à 5 niveaux : IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Journalisation d'audit complète de toutes les décisions de permissions

### IA & Connaissances
- **Prise en charge de multiples backends IA**
  - **Ollama** — Déploiement de modèles locaux, avec API HTTP native
  - **Alibaba Cloud DashScope (Bailian)** — Service IA cloud, compatible API OpenAI, prend en charge 13+ modèles, déploiement multi-région
- **32 systèmes de calendrier** — Couverture complète des principaux calendriers mondiaux, incluant calendrier grégorien, calendrier lunaire chinois, calendrier islamique, calendrier hébraïque, calendrier japonais, calendrier persan, calendrier maya, calendrier historique chinois, etc.
- **Système de réseau de connaissances** — Graphe de connaissances basé sur des triplets (sujet-relation-objet), prend en charge le stockage, les requêtes et la découverte de chemins

### Interface Web
- **Interface Web moderne** — Serveur HTTP intégré avec mises à jour en temps réel SSE
- **7 thèmes d'apparence** — Versions Admin, Chat, Creative, Dev, Contraste élevé, Light, Minimal, prend en charge la détection et la commutation automatiques
- **20+ contrôleurs** — Gestion complète du système, chat, configuration, fonctionnalités de surveillance
- **Zéro dépendance framework frontend** — HTML/CSS/JS générés côté serveur via `H`, `CssBuilder` et `JsBuilder`

### Internationalisation & Localisation
- **Prise en charge complète de 24 variantes linguistiques**
  - Chinois : zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY (6 variantes)
  - Anglais : en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variantes)
  - Espagnol : es-ES, es-MX (2 variantes)
  - Allemand : de-DE, de-AT, de-CH, de-LU, de-LI (5 variantes)
  - Français : fr-FR, fr-CA, fr-CH (3 variantes)
  - Japonais : ja-JP | Coréen : ko-KR | Tchèque : cs-CZ

### Données & Stockage
- **Stockage haute performance SpeedyPack** — Moteur de stockage .spk propriétaire, mappage de répertoires en mémoire + cache d'entrées + file d'écriture asynchrone
- **Zéro dépendance de base de données** — La version Default utilise un stockage pur en système de fichiers (format JSON), la version Fast utilise le stockage en mémoire SpeedyPack
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
- **Mode d'exécution** : Application Windows Forms (prend en charge la barre d'état système)
- **Méthode de stockage** : Stockage en mémoire SpeedyPack + persistance par lot asynchrone (format de fichier .spk)
- **Scénarios applicables** : Concurrence élevée, faible latence, grands volumes de données
- **Caractéristiques** : Optimisation de performance extrême, exécution en arrière-plan dans la barre d'état, moteur SpeedyPack + compression automatique garantissant la sécurité des données
- **Amélioration des performances** : Latence de lecture mémoire réduite de 1000x, latence d'écriture réduite de 15000x, capacité de traitement parallèle augmentée de 50x
- **Description du rôle** : Implémentation prête pour la production avec optimisation approfondie, le meilleur choix pour l'exploitation à long terme et les véritables environnements de production
- **Commande de démarrage** : `dotnet run --project src/SiliconLife.Fast`

### Comparaison des versions

| Caractéristique | SiliconLife.Default | SiliconLife.Fast |
|---------|---------------------|------------------|
| **Mode d'exécution** | Application console | Application Forms (barre d'état) |
| **Interface utilisateur** | Interface Web (accès navigateur) | Icône de barre d'état + Fenêtre de barre d'état + Interface Web |
| **Barre d'état système** | ❌ Non | ✅ Prend en charge la minimisation dans la barre d'état |
| **Exécution en arrière-plan** | ❌ Se termine à la fermeture de la console | ✅ Exécution continue en arrière-plan dans la barre d'état |
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
| Runtime | .NET 9 | .NET 9 Windows |
| Langage de programmation | C# | C# |
| Type d'application | Application console | Application Windows Forms |
| Intégration IA | Ollama (local), Alibaba Cloud DashScope (cloud) | Ollama (local), Alibaba Cloud DashScope (cloud) |
| Stockage de données | Système de fichiers (JSON + répertoire d'index temporel) | SpeedyPack (format .spk, mappage en mémoire + persistance asynchrone) |
| Serveur Web | HttpListener (intégré .NET) | HttpListener (intégré .NET) |
| Compilation dynamique | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automatisation de navigateur | Playwright (WebView) | Playwright (WebView) |
| Système de plugins | ✅ Pris en charge (IPlugin + PluginLoader) | ✅ Pris en charge (IPlugin + PluginLoader) |
| Barre d'état système | ❌ Non pris en charge | ✅ Pris en charge (NotifyIcon) |
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
│   │   ├── AI/                            # Fabrique de clients IA
│   │   ├── Calendar/                      # 32 implémentations de calendriers
│   │   ├── Localization/                  # Classe de base de localisation
│   │   ├── Security/                      # Gestionnaire de permissions
│   │   ├── SiliconBeing/                  # Implémentation standard du Silicon Being
│   │   ├── Tools/                         # Implémentations communes des outils
│   │   └── WebView/                       # Interfaces WebView
│   │
│   ├── SiliconLife.Default/               # Implémentation standard + point d'entrée (version console)
│   │   ├── Program.cs                     # Point d'entrée (assemblage de tous les composants)
│   │   ├── Config/                        # Données de configuration standard
│   │   ├── Executors/                     # Implémentations standard des exécuteurs
│   │   ├── Help/                          # Système de documentation d'aide
│   │   ├── IM/                            # Fournisseur WebUI
│   │   ├── Knowledge/                     # Implémentation du réseau de connaissances
│   │   ├── Localization/                  # 24 localisations linguistiques
│   │   ├── Logging/                       # Implémentations des fournisseurs de journalisation
│   │   ├── Project/                       # Implémentation du système de projets
│   │   ├── Runtime/                       # Objets horloge de test
│   │   ├── Security/                      # Rappels de permissions standard
│   │   ├── SiliconBeing/                  # Implémentation standard du Silicon Being
│   │   ├── Storage/                       # Implémentation du stockage en système de fichiers
│   │   ├── Tools/                         # Implémentations des outils intégrés
│   │   ├── WebView/                       # Implémentation Playwright WebView
│   │   └── Web/                           # Implémentation de l'interface Web
│   │       ├── Controllers/               # 20+ contrôleurs
│   │       ├── Models/                    # Modèles de vue
│   │       ├── Views/                     # Vues HTML
│   │       └── Skins/                     # 4 thèmes d'apparence
│   │
│   └── SiliconLife.Fast/                  # Implémentation haute performance + point d'entrée (version Forms)
│       ├── Program.cs                     # Point d'entrée (application Forms)
│       ├── Config/                        # Données de configuration (partagées avec Default)
│       ├── Executors/                     # Implémentations optimisées des exécuteurs
│       ├── Help/                          # Système de documentation d'aide
│       ├── IM/                            # Fournisseur WebUI
│       ├── Knowledge/                     # Implémentation du réseau de connaissances (optimisée mémoire)
│       ├── Localization/                  # 24 localisations linguistiques
│       ├── Logging/                       # Fournisseurs de journalisation haute performance
│       ├── Project/                       # Implémentation du système de projets
│       ├── Security/                      # Rappels de permissions optimisés
│       ├── SiliconBeing/                  # Implémentation haute performance du Silicon Being
│       ├── Storage/                       # Adaptateur de stockage SpeedyPack
│       ├── Tools/                         # Implémentations optimisées des outils intégrés
│       ├── Tray/                          # Barre d'état système (9 localisations linguistiques)
│       ├── WebView/                       # Implémentation Playwright WebView
│       └── Web/                           # Implémentation haute performance de l'interface Web
│           ├── Component/                 # Bibliothèque de composants UI (30+ composants)
│           ├── Controllers/               # 20+ contrôleurs
│           ├── Models/                    # Modèles de vue
│           ├── Views/                     # Vues HTML
│           └── Skins/                     # 7 thèmes d'apparence
│
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
│   └── SiliconLife.Speedy.Manager/        # Outil de gestion SpeedyPack (WPF)
│       ├── MainForm.cs                    # Formulaire principal
│       ├── Program.cs                     # Point d'entrée
│       └── slc.ico                        # Icône de l'application
│
├── docs/                                  # Documentation multilingue
│   ├── zh-CN/                             # Chinois simplifié
│   ├── de-DE/                             # Allemand
│   ├── fr-FR/                             # Français
│   ├── en/                                # Anglais
│   └── ...                                # Autres langues
│
└── 总文档/                                 # Documents d'exigences et d'architecture
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
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
Appel d'outil → Exécuteur → Gestionnaire de permissions → [IsCurator → Cache de fréquence → GlobalACL → Rappel → Demande utilisateur]
```

## 🚀 Démarrage rapide

### Prérequis

- **.NET 9 SDK** — [Lien de téléchargement](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Backend IA** (choisissez-en un) :
  - **Ollama** : [Installer Ollama](https://ollama.com) et récupérer un modèle (ex. `ollama pull llama3`)
  - **Alibaba Cloud DashScope** : Obtenir une clé API depuis la [console DashScope](https://bailian.console.aliyun.com/)

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

#### Méthode 2 : Exécuter la version Fast (application Windows Forms)

```bash
dotnet run --project src/SiliconLife.Fast
```

L'application démarre en mode Forms, se minimise dans la barre d'état système et continue de fonctionner en arrière-plan.

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

# Linux - Version Default uniquement
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Version Default uniquement
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Feuille de route de développement

### ✅ Terminé
- [x] Phase 1 : Chat IA en console
- [x] Phase 2 : Squelette du framework (boucle principale + objets horloge + watchdog + disjoncteur)
- [x] Phase 3 : Premier Silicon Being avec fichier âme (architecture Body-Brain)
- [x] Phase 4 : Stockage persistant (système de chat + interface Time Storage)
- [x] Phase 5 : Système d'outils + exécuteurs
- [x] Phase 6 : Système de permissions (chaîne à 5 niveaux, journal d'audit, GlobalACL)
- [x] Phase 7 : Compilation dynamique + auto-développement (Roslyn)
- [x] Phase 8 : Stockage à long terme + tâches + minuteries
- [x] Phase 9 : Hôte principal + collaboration multi-agents
- [x] Phase 10 : Interface Web (HTTP + SSE, 20+ contrôleurs, 4 thèmes)
- [x] Phase 10.5 : Améliorations incrémentales (canaux de diffusion, audit de tokens, 32 calendriers, améliorations d'outils, localisation en 24 langues)
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
