# Démarrage rapide

> **Version : v0.1.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | **Français** | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md)

## Choisir une version

Ce projet propose deux versions d'implémentation :

### SiliconLife.Default (Version standard)
- **Positionnement** : Implémentation standard, principalement pour la vérification de faisabilité architecturale
- **Mode d'exécution** : Application console
- **Stockage** : Stockage JSON en système de fichiers
- **Scénario d'utilisation** : Priorité à la sécurité des données, petit volume de données, débogage de développement, vérification architecturale
- **Prise en charge de plateforme** : Windows, Linux, macOS
- **Description du rôle** : Implémentation de référence pour la vérification architecturale, offre une exécution simple et fiable, adaptée au premier contact ou au débogage de développement

### SiliconLife.Fast (Version haute performance)
- **Positionnement** : Version principale de production
- **Mode d'exécution** : Application Windows Forms (barre d'état système)
- **Stockage** : Stockage en mémoire SpeedyPack + persistance asynchrone (format de fichier .spk)
- **Scénario d'utilisation** : Concurrence élevée, faible latence, grand volume de données, exploitation en production à long terme
- **Prise en charge de plateforme** : Windows uniquement
- **Description du rôle** : Implémentation prête pour la production avec optimisation approfondie, le meilleur choix pour l'exploitation à long terme et les véritables environnements de production

> **Recommandation pour les débutants** : Les nouveaux utilisateurs devraient commencer par **SiliconLife.Default** pour vérifier rapidement la faisabilité architecturale. Après s être familiarisé avec le système, une migration vers **SiliconLife.Fast** est fortement recommandée.

## Prérequis

- **.NET 9 SDK** - [Télécharger](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Télécharger](https://git-scm.com/)
- **Ollama** (optionnel, pour l'IA locale) - [Télécharger](https://ollama.com/)
- **Clé API DashScope** (optionnel, pour l'IA cloud) - [Demander](https://bailian.console.aliyun.com/)

## Démarrage rapide

### 1. Cloner le dépôt

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Construire le projet

```bash
dotnet build
```

### 3. Configurer le backend IA

Modifiez `src/SiliconLife.Default/Config/DefaultConfigData.cs` ou changez la configuration à l'exécution via l'interface Web.

#### Option A : Ollama (local)

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    }
  }
}
```

#### Option B : DashScope (cloud)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "votre-clé-api-ici",
      "Model": "qwen-plus",
      "Region": "cn-hangzhou"
    }
  }
}
```

### 4. Exécuter l'application

#### Exécuter la version Default

```bash
cd src/SiliconLife.Default
dotnet run
```

Le serveur Web démarre sur `http://localhost:8080`

#### Exécuter la version Fast (Windows uniquement)

```bash
cd src/SiliconLife.Fast
dotnet run
```

L'application démarre en mode Forms, minimisée dans la barre d'état système, avec le serveur Web également sur `http://localhost:8080`

### 5. Accéder à l'interface Web

Ouvrez un navigateur et accédez à :

```
http://localhost:8080
```

Vous verrez un tableau de bord avec :
- Gestion des Silicon Beings
- Interface de chat
- Panneau de configuration
- Surveillance système

## Votre premier Silicon Being

### Créer votre premier Being

1. Dans l'interface Web, naviguez vers **Gestion des Beings**
2. Cliquez sur **Créer un nouveau Being**
3. Configurez le fichier âme (`soul.md`) avec personnalité et comportement
4. Démarrez le Being

### Exemple soul.md

```markdown
# Mon premier Silicon Being

## Personnalité
Vous êtes un assistant utile spécialisé dans la revue de code.

## Capacités
- Examiner la qualité du code
- Suggérer des améliorations
- Expliquer les concepts complexes

## Comportement
- Toujours fournir un retour constructif
- Utiliser des exemples clairs
- Être concis mais exhaustif
```

## Questions fréquentes

### Connexion Ollama refusée

**Problème** : Impossible de se connecter à Ollama sur `http://localhost:11434`

**Solution** :
```bash
# Vérifier si Ollama fonctionne
ollama list

# Démarrer Ollama si nécessaire
ollama serve
```

### Modèle non trouvé

**Problème** : `model "qwen2.5:7b" not found`

**Solution** :
```bash
# Télécharger le modèle requis
ollama pull qwen2.5:7b
```

### Port déjà occupé

**Problème** : `HttpListenerException: Address already in use`

**Solution** :
- Changer le port dans la configuration
- Ou terminer le processus sur le port 8080 :

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md) pour comprendre la conception système
- 🛠️ Consulter le [guide de développement](development-guide.md) pour étendre le système
- 📖 Explorer la [référence API](api-reference.md) pour les détails d'intégration
- 🔒 Consulter la [documentation de sécurité](security.md) pour le système de permissions
- 🧰 Parcourir la [référence des outils](tools-reference.md) pour tous les outils intégrés
- 🌐 Lire le [guide de l'interface Web](web-ui-guide.md) pour les fonctionnalités de l'interface

## Structure du projet

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces principales et classes abstraites
│   ├── SiliconLife.Common/          # Implémentation commune (utilisée par les deux versions)
│   ├── SiliconLife.Default/         # Implémentation standard + point d'entrée (version console)
│   ├── SiliconLife.Fast/            # Implémentation haute performance + point d'entrée (version Forms)
│   ├── SiliconLife.Speedy/          # Moteur de stockage haute performance SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Outil de gestion SpeedyPack (WPF)
├── docs/                            # Documentation (multilingue, 24 variantes linguistiques)
│   ├── en/                          # Anglais
│   ├── zh-CN/                       # Chinois simplifié
│   ├── zh-HK/                       # Chinois traditionnel
│   ├── de-DE/                       # Allemand
│   ├── fr-FR/                       # Français
│   ├── es-ES/                       # Espagnol
│   ├── ja-JP/                       # Japonais
│   ├── ko-KR/                       # Coréen
│   └── cs-CZ/                       # Tchèque
├── 总文档/                           # Documents d'exigences et d'architecture (Chinois)
└── README.md                        # Aperçu du projet
```

## Besoin d'aide ?

- 📖 Consulter le [système de documentation d'aide](web-ui-guide.md#帮助文档系统新增) (prise en charge multilingue)
- 📚 Lire la [documentation complète](docs/)
- 🐛 Signaler les problèmes sur [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Participer aux discussions de la communauté
