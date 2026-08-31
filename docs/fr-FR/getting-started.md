# Démarrage rapide

> **Version : v0.2.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md) | [Русский](../ru-RU/getting-started.md)

## Choisir une version

Ce projet propose deux versions d'implémentation :

### SiliconLife.Default (version par défaut)
- **Positionnement** : Implémentation par défaut, principalement utilisée pour valider la faisabilité de l'architecture
- **Mode d'exécution** : Application console
- **Mode de stockage** : Stockage JSON sur système de fichiers
- **Scénarios d'utilisation** : Sécurité des données prioritaire, faible volume de données, débogage de développement, validation de l'architecture
- **Support de plateforme** : Windows, Linux, macOS
- **Rôle** : Implémentation de référence pour la validation de l'architecture, offrant un mode d'exécution simple et fiable, adaptée aux premiers contacts avec le projet ou au débogage de développement

### SiliconLife.Fast (version haute performance)
- **Positionnement** : Version de production recommandée
- **Mode d'exécution** : Application de bureau (barre d'état système Windows/macOS / fenêtre d'état Linux)
- **Mode de stockage** : Stockage en mémoire SpeedyPack + persistance asynchrone (format de fichier .spk)
- **Scénarios d'utilisation** : Concurrence élevée, faible latence, grand volume de données, exécution en production à long terme
- **Support de plateforme** : Windows/macOS (fonctionnalités complètes, incluant la barre d'état système) ; Linux (fenêtre d'état, sans icône de barre d'état)
- **Rôle** : Implémentation de niveau production profondément optimisée, choix recommandé pour l'exécution à long terme et les environnements de production réels

> **Conseil pour les débutants** : Pour une première utilisation, il est recommandé de commencer par **SiliconLife.Default** pour valider rapidement la faisabilité de l'architecture ; une fois familiarisé avec le système, il est fortement conseillé de migrer vers **SiliconLife.Fast** comme version d'exécution en production.

## Prérequis

- **.NET 9 SDK** - [Télécharger](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Télécharger](https://git-scm.com/)
- **Ollama** (facultatif, pour l'IA locale) - [Télécharger](https://ollama.com/)
- **Clé API Bailian** (facultatif, pour l'IA cloud) - [Demander](https://bailian.console.aliyun.com/)
- **Clé API Volcengine Ark** (facultatif, pour l'IA cloud) - [Demander](https://console.volcengine.com/ark)
- **Herdsman** (facultatif, moteur d'inférence local/cloud) - Sans authentification, compatible avec le format API OpenAI
- **Clé API Meituan LongCat** (facultatif, pour l'IA cloud) - Authentification par clé API
- **Clé API Qiniu Cloud AI** (facultatif, pour l'IA cloud) - Authentification par clé API
- **Clé API DeepSeek** (facultatif, pour l'IA cloud) - [Demander](https://platform.deepseek.com/)
- **Clé API Zhipu AI** (facultatif, pour l'IA cloud) - [Demander](https://open.bigmodel.cn/)
- **Clé API Baidu Qianfan** (facultatif, pour l'IA cloud) - [Demander](https://qianfan.baidubce.com/)
- **Clé API Tencent Hunyuan** (facultatif, pour l'IA cloud) - [Demander](https://hunyuan.tencent.com/)
- **Clé API MiniMax** (facultatif, pour l'IA cloud) - [Demander](https://platform.minimaxi.com/)
- **Clé API Moonshot** (facultatif, pour l'IA cloud) - [Demander](https://platform.moonshot.cn/)
- **Clé API SiliconFlow** (facultatif, pour l'IA cloud) - [Demander](https://cloud.siliconflow.cn/)

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

Éditez `src/SiliconLife.Default/Config/DefaultConfigData.cs` ou modifiez la configuration via l'interface Web UI à l'exécution.

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

#### Option B : Bailian (cloud)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "beijing"
    }
  }
}
```

> **Régions disponibles** : `beijing` (Pékin), `virginia` (Virginie), `singapore` (Singapour), `hongkong` (Hong Kong), `frankfurt` (Francfort)

#### Option C : Volcengine Ark (cloud)

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **Note** : Le paramètre Model de Volcengine Ark accepte un ID de point d'accès d'inférence (par exemple `ep-20241212123456-abcde`), et non un nom de modèle.

#### Option D : Herdsman (Local/Cloud)

```json
{
  "AIClients": {
    "Herdsman": {
      "Endpoint": "http://localhost:8000",
      "Model": "nom-du-modèle"
    }
  }
}
```

> **Caractéristiques** : Sans authentification, compatible avec le format API OpenAI, prend en charge les appels d'outils et le contenu de raisonnement.

#### Option E : Meituan LongCat (Cloud)

```json
{
  "AIClients": {
    "LongCat": {
      "ApiKey": "clé-api",
      "Endpoint": "https://api.longcat.chat/openai",
      "Model": "LongCat-2.0"
    }
  }
}
```

#### Option F : Qiniu Cloud AI (Cloud)

```json
{
  "AIClients": {
    "QiniuAI": {
      "ApiKey": "clé-api",
      "Endpoint": "https://api.qiniu.com/v1/chat/completions",
      "Model": "nom-du-modèle"
    }
  }
}
```

#### Option G : DeepSeek (Cloud)

```json
{
  "AIClients": {
    "DeepSeek": {
      "ApiKey": "clé-api",
      "Endpoint": "https://api.deepseek.com",
      "Model": "deepseek-v4-flash"
    }
  }
}
```

> **Caractéristiques** : Mode thinking activé par défaut, 1M contexte, contrôle de l'effort de raisonnement (`reasoningEffort`).

#### Option H : Zhipu AI / GLM (Cloud)

```json
{
  "AIClients": {
    "Zhipu": {
      "ApiKey": "clé-api",
      "Endpoint": "https://open.bigmodel.cn/api/paas/v4",
      "Model": "glm-4-flash"
    }
  }
}
```

> **Caractéristiques** : Mode thinking pour GLM-5, vision selon le modèle (glm-4v/glm-5v), modèle gratuit `glm-4-flash`, 1M contexte.

#### Option I : Baidu Qianfan / ERNIE (Cloud)

```json
{
  "AIClients": {
    "Ernie": {
      "ApiKey": "clé-api",
      "Endpoint": "https://qianfan.baidubce.com/v2",
      "Model": "ernie-5.1"
    }
  }
}
```

> **Caractéristiques** : Modèles gratuits disponibles (`ernie-speed`, `ernie-tiny`), 131K contexte.

#### Option J : Tencent Hunyuan (Cloud)

```json
{
  "AIClients": {
    "Hunyuan": {
      "ApiKey": "clé-api",
      "Endpoint": "https://tokenhub.tencentmaas.com/v1",
      "Model": "hy3"
    }
  }
}
```

> **Endpoints disponibles** : TokenHub (recommandé) : `https://tokenhub.tencentmaas.com/v1` ; Legacy : `https://api.hunyuan.cloud.tencent.com/v1`. Mode thinking pour hy3/hy-2.0, 262K contexte.

#### Option K : MiniMax (Cloud)

```json
{
  "AIClients": {
    "MiniMax": {
      "ApiKey": "clé-api",
      "Endpoint": "https://api.minimaxi.com/v1",
      "Model": "MiniMax-M3"
    }
  }
}
```

> **Caractéristiques** : Multimodal natif (image + vidéo pour M3), mode thinking adaptatif, 1M contexte.

#### Option L : Moonshot / Kimi (Cloud)

```json
{
  "AIClients": {
    "Moonshot": {
      "ApiKey": "clé-api",
      "Endpoint": "https://api.moonshot.cn/v1",
      "Model": "kimi-k2.6"
    }
  }
}
```

> **Caractéristiques** : Mode thinking (k2.5/k2.6/k2.7), multimodal, 262K contexte.

#### Option M : SiliconFlow (Cloud — Agrégateur)

```json
{
  "AIClients": {
    "SiliconFlow": {
      "ApiKey": "clé-api",
      "Endpoint": "https://api.siliconflow.cn/v1",
      "Model": "deepseek-ai/DeepSeek-V3.2"
    }
  }
}
```

> **Caractéristiques** : Agrège 100+ modèles open source de multiples fournisseurs, liste de modèles dynamique, 1M contexte.

### 4. Exécuter l'application

#### Exécuter la version Default

```bash
cd src/SiliconLife.Default
dotnet run
```

Le serveur Web démarrera sur `http://localhost:8080`

#### Exécuter la version Fast

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS** : L'application démarrera en mode fenêtré, se réduira dans la barre d'état système, et le serveur Web démarrera également sur `http://localhost:8080`

**Linux** : L'application affichera une fenêtre d'état (sans icône de barre d'état système) et ouvrira automatiquement le navigateur pour accéder à l'interface Web UI. Vous pouvez également utiliser le paramètre `--no-tray` pour éviter l'ouverture automatique du navigateur :

```bash
dotnet run -- --no-tray
```

### 5. Accéder à l'interface Web UI

Ouvrez votre navigateur et accédez à :

```
http://localhost:8080
```

Vous verrez un tableau de bord contenant :
- Gestion des Êtres de Silicium
- Interface de chat
- Panneau de configuration
- Surveillance du système

## Premier Être de Silicium

### Créer votre premier être

1. Dans l'interface Web UI, naviguez vers **Gestion des êtres**
2. Cliquez sur **Créer un nouvel être**
3. Configurez le Fichier d'Âme (`soul.md`), incluant personnalité et comportement
4. Démarrez l'être

### Exemple de soul.md

```markdown
# My First Silicon Being

## Personality
You are a helpful assistant specializing in code review.

## Capabilities
- Review code quality
- Suggest improvements
- Explain complex concepts

## Behavior
- Always provide constructive feedback
- Use clear examples
- Be concise but thorough
```

## Questions fréquentes

### Connexion Ollama refusée

**Problème** : Impossible de se connecter à Ollama sur `http://localhost:11434`

**Solution** :
```bash
# Vérifier si Ollama est en cours d'exécution
ollama list

# Démarrer Ollama si nécessaire
ollama serve
```

### Modèle introuvable

**Problème** : `model "qwen2.5:7b" not found`

**Solution** :
```bash
# Tirer le modèle requis
ollama pull qwen2.5:7b
```

### Port déjà occupé

**Problème** : `HttpListenerException: Address already in use`

**Solution** :
- Changer le port dans la configuration
- Ou terminer le processus utilisant le port 8080 :

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md) pour comprendre la conception du système
- 🛠️ Consulter le [guide de développement](development-guide.md) pour étendre le système
- 📖 Explorer la [référence API](api-reference.md) pour les détails d'intégration
- 🔒 Consulter la [documentation de sécurité](security.md) pour le système d'autorisations
- 🧰 Consulter la [référence des outils](tools-reference.md) pour tous les outils intégrés
- 🌐 Consulter le [guide de l'interface Web UI](web-ui-guide.md) pour les fonctionnalités de l'interface

## Structure du projet

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces et classes abstraites principales
│   ├── SiliconLife.Common/          # Implémentations partagées (communes aux deux versions)
│   ├── SiliconLife.App/             # Couche applicative partagée entre Default et Fast
│   ├── SiliconLife.Default/         # Implémentation par défaut + point d'entrée (version console)
│   ├── SiliconLife.Fast/            # Implémentation haute performance + point d'entrée (version fenêtrée)
│   ├── SiliconLife.Speedy/          # Moteur de stockage haute performance SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Outil de gestion SpeedyPack (Avalonia UI)
├── docs/                            # Documentation (multilingue, 34 variantes linguistiques)
│   ├── en/                          # Anglais
│   ├── zh-CN/                       # Chinois simplifié
│   ├── zh-HK/                       # Chinois traditionnel
│   ├── es-ES/                       # Espagnol
│   ├── ja-JP/                       # Japonais
│   ├── ko-KR/                       # Coréen
│   └── cs-CZ/                       # Tchèque
├── 总文档/                           # Documents de besoins et d'architecture (chinois)
└── README.md                        # Description du projet
```

## Besoin d'aide ?

- 📖 Consulter le [système de documentation d'aide](web-ui-guide.md#système-de-documentation-daide-nouveau) (support multilingue)
- 📚 Lire la [documentation complète](docs/)
- 🐛 Signaler les problèmes sur [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Participer aux discussions de la communauté
