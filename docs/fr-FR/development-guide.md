# Guide de développement

> **Version : v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | **Français** | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | [Русский](../ru-RU/development-guide.md)

## Aperçu de l'architecture

SiliconLifeCollective suit une **architecture corps-cerveau**, avec une séparation stricte entre les interfaces principales et les implémentations par défaut.

### Structure du projet

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces, classes abstraites, infrastructure commune
│   ├── SiliconLife.Common/          # Implémentations partagées (communes aux deux versions)
│   ├── SiliconLife.Default/         # Implémentation par défaut, point d'entrée (vérification de faisabilité)
│   ├── SiliconLife.Fast/            # Implémentation haute performance, point d'entrée (version de production)
│   ├── SiliconLife.Speedy/          # Moteur de stockage haute performance SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Outil de gestion SpeedyPack (Windows Forms)
└── docs/                            # Documentation multilingue
```

**Direction des dépendances** :
- `SiliconLife.Default` → `SiliconLife.Core` (unidirectionnel)
- `SiliconLife.Fast` → `SiliconLife.Core` (unidirectionnel)
- `SiliconLife.Common` → `SiliconLife.Core` (unidirectionnel)

**Description des rôles des versions** :
- **SiliconLife.Default** : Implémentation par défaut, principalement pour la vérification de faisabilité architecturale. Fournit une implémentation de stockage en système de fichiers simple et fiable, adaptée au débogage de développement et à la vérification architecturale.
- **SiliconLife.Fast** : Version principale de production. Sur la base de l'architecture vérifiée par Default, adopte le stockage en mémoire SpeedyPack + persistance asynchrone, offrant une optimisation des performances extrême, le meilleur choix pour l'exploitation à long terme et les véritables environnements de production.

## Concepts fondamentaux

### 1. Silicon Being

Chaque agent IA est composé de :
- **Corps** (`DefaultSiliconBeing`) : Maintient l'état de survie, détecte les scénarios de déclenchement
- **Cerveau** (`ContextManager`) : Charge l'historique, appelle l'IA, exécute les outils, persiste les réponses

### 2. Système d'outils

Les outils sont automatiquement découverts et enregistrés via réflexion :

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Système de permissions

Chaîne de vérification des permissions à 3 niveaux en structure ramifiée :
```
IsCurator → UserFrequencyCache → (IPermissionCallback | GlobalACL) → IPermissionAskHandler
```
- **IsCurator** : Le Silicon Curateur contourne toutes les vérifications
- **UserFrequencyCache** : Cache des autorisations/refus fréquents de l'utilisateur ; en cas de cache absent, bifurque vers :
  - **IPermissionCallback** : Fonction de rappel de permission personnalisée du Being (si définie)
  - **GlobalACL** : Liste de contrôle d'accès globale (si aucun callback défini)
- **IPermissionAskHandler** : Demander à l'utilisateur (si les niveaux précédents n'ont pas pris de décision)

### 4. Localisateur de services

Enregistrement et récupération globale des services :
```csharp
// Enregistrement
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Récupération
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Système d'extension

### Ajouter un nouvel outil

1. Créer une nouvelle classe dans `src/SiliconLife.Common/Tools/` (outils partagés entre les deux versions) ou `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (outils spécifiques à une version) :

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description de ce que fait cet outil";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Analyser les paramètres
        var param1 = call.Parameters["param1"]?.ToString();
        
        // Exécuter la logique
        var result = await DoSomething(param1);
        
        // Retourner le résultat
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. L'outil est automatiquement découvert via réflexion — pas d'enregistrement manuel !

3. (Optionnel) Marquer comme réservé aux administrateurs :
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### Ajouter un nouveau client IA

1. Implémenter `IAIClient` dans `src/SiliconLife.Common/AI/` :

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Appeler votre API IA
        var response = await CallMyAPI(request);
        
        return new AIResponse
        {
            Content = response.Message,
            ToolCalls = response.ToolCalls,
            Usage = response.Usage
        };
    }
    
    public async IAsyncEnumerable<string> StreamChatAsync(AIRequest request)
    {
        // Implémenter le streaming
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. Créer la fabrique :

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. La fabrique est automatiquement découverte et enregistrée.

### Ajouter un nouveau backend de stockage

1. Implémenter `IStorage` et `ITimeStorage` dans `src/SiliconLife.Default/Storage/` (implémentation système de fichiers) ou `src/SiliconLife.Fast/Storage/` (adaptateur SpeedyPack) :

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // Lire depuis votre base de données
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // Écrire dans votre base de données
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // Requête par index temporel
    }
}
```

### Ajouter un nouveau plugin

1. Créer un projet de bibliothèque de classes, implémenter l'interface `IPlugin` :

```csharp
using SiliconLife.Collective;
using SiliconLife.Collective.Localization;
using SiliconLife.Collective.Tools;

public class MyPlugin : IPlugin
{
    public string Id => "my-plugin";
    public string Version => "1.0.0";
    
    public string GetName(Language language) => "My Plugin";
    public string GetDescription(Language language) => "Un plugin personnalisé";
    public string GetAuthor(Language language) => "Nom de l'auteur";
    
    public void OnLoad() { }
    public void OnStart() { }
    public void OnStop() { }
    public void OnUnload() { }
}
```

2. (Optionnel) Implémenter l'interface `ITool` dans le plugin pour enregistrer des outils personnalisés :

```csharp
public class MyPluginTool : ITool
{
    public string Name => "my_plugin_tool";
    public string Description => "Un outil fourni par mon plugin";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        return new ToolResult { Success = true, Output = "Terminé" };
    }
}
```

3. Placer la DLL compilée dans le répertoire des plugins, `PluginLoader` la chargera automatiquement.

> **Restrictions de sécurité** : Les plugins ne peuvent pas référencer les espaces de noms `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`, etc. Les plugins sont chargés de manière isolée via `AssemblyLoadContext`.

### Ajouter un nouveau skin

1. Implémenter `ISkin` dans `src/SiliconLife.App/Web/Skins/` :

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "Description d'un skin personnalisé";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
            }
            /* Vos styles personnalisés */
        ";
    }
}
```

2. Le skin est automatiquement découvert par `SkinManager`.

## Guide du style de code

### Conventions de nommage

- **Classes** : PascalCase, avec préfixe fonctionnel (ex. `DefaultSiliconBeing`)
- **Interfaces** : Commencent par `I` (ex. `IAIClient`, `ITool`)
- **Implémentations** : Se terminent par le nom de l'interface (ex. `OllamaClient` implémente `IAIClient`)
- **Outils** : Se terminent par `Tool` (ex. `CalendarTool`, `ChatTool`)
- **Modèles de vue** : Se terminent par `ViewModel` (ex. `BeingViewModel`)

### Organisation du code

```
SiliconLife.Common/
├── AI/                    # Implémentations des clients IA et fabriques
├── Calendar/              # 32 implémentations de calendriers
├── Localization/          # Classe de base de localisation et 33 variantes linguistiques
├── Security/              # Gestionnaire de permissions
├── SiliconBeing/          # Implémentation du Silicon Being par défaut
├── Tools/                 # Outils intégrés partagés
├── Web/                   # Infrastructure Web
└── WebView/               # Implémentation Playwright WebView

SiliconLife.App/          # Couche applicative partagée entre Default et Fast
├── Config/                # Configuration applicative
├── Help/                  # Localisation de la documentation d'aide
└── Web/                   # Implémentation de l'interface Web
    ├── Component/         # Bibliothèque de composants UI
    ├── Controllers/       # Contrôleurs de routage
    ├── Models/            # Modèles de vue
    ├── Views/             # Vues HTML
    └── Skins/             # Thèmes de skins

SiliconLife.Default/      # Répertoires spécifiques à la version
├── Config/                # Données de configuration par défaut
├── IM/                    # Fournisseur WebUI
├── Knowledge/             # Implémentation du réseau de connaissances
├── Logging/               # Implémentations des fournisseurs de journaux
├── Project/               # Implémentation du système de projets
├── Security/              # Rappels de permissions par défaut
├── Storage/               # Implémentation du stockage en système de fichiers
└── Tools/                 # Outils spécifiques à la version (HelpTool)
```

### Documentation

- Toutes les API publiques doivent avoir des commentaires de documentation XML
- Tous les fichiers source utilisent l'en-tête de licence Apache 2.0
- Exploiter les fonctionnalités .NET 9 (using implicites, types de référence nullables)

## Flux de travail de développement

### 1. Configurer l'environnement de développement

```bash
# Cloner le dépôt
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Restaurer les dépendances
dotnet restore

# Construire
dotnet build
```

### 2. Exécuter les tests

```bash
# Exécuter tous les tests
dotnet test

# Exécuter un projet de test spécifique
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Déboguer

```bash
# Exécuter avec sortie de débogage
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Formater le code

```bash
# Formater le code
dotnet format
```

## Construire des fonctionnalités personnalisées

### Exemple : Ajouter un calendrier personnalisé

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Votre logique de conversion
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Conversion inverse
        return new GregorianDate(year, month, day);
    }
}
```

### Exemple : Ajouter un exécuteur personnalisé

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        // D'abord vérifier les permissions
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        // Exécuter l'opération
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

## Guide des tests

### Tests unitaires

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Arrangement
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object> 
            { 
                ["param1"] = "test" 
            }
        };
        
        // Exécution
        var result = await tool.ExecuteAsync(call);
        
        // Assertion
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### Tests d'intégration

Tester le flux complet :
1. L'IA retourne un appel d'outil
2. L'outil s'exécute
3. Le résultat est renvoyé à l'IA
4. L'IA retourne la réponse finale

## Considérations de performance

### Système de stockage

- La version Default utilise le stockage JSON basé sur les fichiers
- La version Fast utilise le moteur de stockage en mémoire SpeedyPack (format .spk)
- SpeedyPack adopte le mappage de répertoire en mémoire + cache d'entrées + file d'attente d'écriture asynchrone
- Les requêtes par index temporel utilisent l'interface `ITimeStorage`

### Ordonnanceur de la boucle principale

- Ordonnancement équitable par time-slice basé sur l'horloge
- Chien de garde pour détecter les opérations bloquées
- Disjoncteur pour prévenir les défaillances en cascade

## Meilleures pratiques

### 1. Toujours vérifier les permissions

Toute opération initiée par l'IA doit passer par la chaîne de permissions :

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
}
```

### 2. Utiliser le localisateur de services

Enregistrement et récupération globale des services :

```csharp
// Pendant l'initialisation
ServiceLocator.Instance.Register<ICustomService>(myService);

// Quand nécessaire
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Suivre la séparation corps-cerveau

- Le corps gère l'état et les déclenchements
- Le cerveau gère les interactions IA et l'exécution des outils

### 4. Implémenter une gestion des erreurs appropriée

```csharp
try
{
    var result = await operation();
    return Result.Success(result);
}
catch (Exception ex)
{
    Logger.Error($"Operation failed: {ex.Message}");
    return Result.Failure(ex.Message);
}
```

## Guide de contribution

1. Forker le dépôt
2. Créer une branche de fonctionnalité (`git checkout -b feature/amazing-feature`)
3. Committer vos modifications avec des commits conventionnels
4. Pousser vers la branche (`git push origin feature/amazing-feature`)
5. Ouvrir une Pull Request

### Format des messages de commit

```
<type>(<portée>): <description>

Exemples :
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
