# Guide de développement

> **Version : v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | [Русский](../ru-RU/development-guide.md)

## Vue d'ensemble de l'architecture

SiliconLifeCollective suit une **architecture Corps-Cerveau**, avec une séparation stricte entre les interfaces principales et les implémentations par défaut.

### Structure du projet

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces, classes abstraites, infrastructure commune
│   ├── SiliconLife.Common/          # Implémentations partagées (communes aux deux versions)
│   ├── SiliconLife.Default/         # Implémentation par défaut, point d'entrée (validation de l'architecture)
│   ├── SiliconLife.Fast/            # Implémentation haute performance, point d'entrée (version de production recommandée)
│   ├── SiliconLife.Speedy/          # Moteur de stockage haute performance SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Outil de gestion SpeedyPack (Avalonia UI)
└── docs/                            # Documentation multilingue
```

**Direction des dépendances** :
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core` (unidirectionnel)

**Rôle des versions** :
- **SiliconLife.Default** : Implémentation par défaut, principalement utilisée pour valider la faisabilité de l'architecture. Fournit une implémentation de stockage sur système de fichiers simple et fiable, adaptée au débogage de développement et à la validation de l'architecture.
- **SiliconLife.Fast** : Version de production recommandée. Sur la base de l'architecture validée par Default, adopte le stockage en mémoire SpeedyPack + persistance asynchrone, offrant des optimisations de performance extrêmes, c'est le choix recommandé pour l'exécution à long terme et les environnements de production réels.

## Concepts fondamentaux

### 1. Être de Silicium

Chaque agent IA est composé des éléments suivants :
- **Corps** (`DefaultSiliconBeing`) : Maintient les signes vitaux, détecte les scénarios de déclenchement
- **Cerveau** (`ContextManager`) : Charge l'historique, appelle l'IA, exécute les outils, persiste les réponses

### 2. Système d'outils

Les outils sont découverts et enregistrés automatiquement par réflexion :

```csharp
// Tous les outils implémentent l'interface ITool
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Système d'autorisations

Chaîne de vérification des autorisations à 3 niveaux :
```
UserFrequencyCache → IPermissionCallback → (IsCurateur : IPermissionAskHandler | Non-curateur : GlobalACL → refus par défaut)
```

### 4. Localisateur de services

Enregistrement et récupération globaux des services :
```csharp
// Enregistrer
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Obtenir
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Système d'extension

### Ajouter un nouvel outil

1. Créer une nouvelle classe dans `src/SiliconLife.Common/Tools/` (outils partagés entre les deux versions) :

> **Note** : `SiliconLife.Default` et `SiliconLife.Fast` n'ont plus de répertoire `Tools/` indépendant, tous les outils partagés sont placés dans `SiliconLife.Common/Tools/`.

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

2. L'outil est découvert automatiquement par réflexion — pas besoin d'enregistrement manuel !

3. (Facultatif) Marquer comme réservé à l'administrateur :
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

4. (Facultatif) Marquer les scénarios disponibles de l'outil :
```csharp
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class MyTool : ITool { ... }
```

5. (Facultatif) Marquer comme disponible uniquement dans le scénario de chat :
```csharp
[ChatOnly]
public class HelpTool : ITool { ... }
```

6. (Facultatif) Marquer comme disponible uniquement dans le scénario de projet :
```csharp
[ToolScenario(ToolScenarioFlag.Project)]
[SiliconManagerOnly]
public class ProjectWorkTool : ITool { ... }
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

2. Créer une fabrique :

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. La fabrique est découverte et enregistrée automatiquement.

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
    public string GetDescription(Language language) => "A custom plugin";
    public string GetAuthor(Language language) => "Author Name";
    
    public void OnLoad() { }
    public void OnStart() { }
    public void OnStop() { }
    public void OnUnload() { }
}
```

2. (Facultatif) Implémenter l'interface `ITool` dans le plugin pour enregistrer des outils personnalisés :

```csharp
public class MyPluginTool : ITool
{
    public string Name => "my_plugin_tool";
    public string Description => "A tool provided by my plugin";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        return new ToolResult { Success = true, Output = "Done" };
    }
}
```

3. Placer la DLL compilée dans le répertoire des plugins, `PluginLoader` la chargera automatiquement.

> **Restrictions de sécurité** : Les plugins ne peuvent pas référencer les espaces de noms `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`, etc. Les plugins sont chargés de manière isolée via `AssemblyLoadContext`.

### Ajouter un nouveau thème

1. Implémenter `ISkin` dans `src/SiliconLife.App/Web/Skins/` :

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "A custom skin description";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
            }
            /* Your custom styles */
        ";
    }
}
```

2. Le thème est découvert automatiquement par `SkinManager`.

## Guide de style de code

### Conventions de nommage

- **Classes** : PascalCase, avec préfixe fonctionnel (par ex. `DefaultSiliconBeing`)
- **Interfaces** : Commencent par `I` (par ex. `IAIClient`, `ITool`)
- **Implémentations** : Se terminent par le nom de l'interface (par ex. `OllamaClient` implémente `IAIClient`)
- **Outils** : Se terminent par `Tool` (par ex. `CalendarTool`, `ChatTool`)
- **Modèles de vue** : Se terminent par `ViewModel` (par ex. `BeingViewModel`)

### Organisation du code

```
SiliconLife.Common/
├── AI/                    # Implémentations des clients IA et fabriques
├── Calendar/              # 32 implémentations de calendriers
├── Localization/          # Classe de base de localisation et 34 variantes linguistiques
├── Security/              # Gestionnaire d'Autorisations
├── SiliconBeing/          # Implémentation par défaut de l'Être de Silicium
├── Tools/                 # Outils intégrés partagés (25)
├── Web/                   # Infrastructure Web
└── WebView/               # Implémentation Playwright WebView

SiliconLife.App/          # Couche applicative partagée entre Default et Fast
├── Config/                # Configuration de l'application
├── Help/                  # Localisation de la documentation d'aide
├── Project/               # Système de projet (moteur de flux de travail, rôles de projet)
└── Web/                   # Implémentation Web UI
    ├── Component/         # 27 composants UI
    ├── Controllers/       # 24 contrôleurs de routage
    ├── Models/            # Modèles de vue
    ├── Views/             # Vues HTML
    └── Skins/             # 7 thèmes d'apparence

SiliconLife.Default/      # Répertoires spécifiques à la version
├── Config/                # Données de configuration par défaut
├── Knowledge/             # Implémentation du Réseau de Connaissances
├── Logging/               # Implémentation du fournisseur de journal (console + système de fichiers)
├── Project/               # Implémentation du système de projet
└── Storage/               # Implémentation du stockage sur système de fichiers

SiliconLife.Fast/         # Répertoires spécifiques à la version
├── Config/                # Données de configuration de la version Fast
├── Logging/               # Implémentation du fournisseur de journal (console + système de fichiers)
├── Storage/               # Adaptateurs de stockage SpeedyPack
└── Tray/                  # Localisation de la barre d'état système
```

### Documentation

- Toutes les API publiques doivent avoir des commentaires de documentation XML
- Tous les fichiers source utilisent l'en-tête de licence Apache 2.0
- Utiliser les fonctionnalités .NET 9 (using implicites, types de référence nullables)

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
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

### Exemple : Ajouter un modèle de flux de travail personnalisé

```csharp
public class MyWorkflowTemplate : WorkflowTemplate
{
    public override string Name => "my_workflow";
    public override string Description => "A custom workflow template";
    
    public override void DefineStates()
    {
        AddState("start", "Début", isInitial: true);
        AddState("processing", "En cours de traitement");
        AddState("review", "En revue");
        AddState("done", "Terminé", isFinal: true);
    }
    
    public override void DefineTransitions()
    {
        AddTransition("start", "processing", "Démarrer le traitement");
        AddTransition("processing", "review", "Soumettre pour revue");
        AddTransition("review", "done", "Revue approuvée");
        AddTransition("review", "processing", "Revue rejetée");
    }
}
```

### Exemple : Ajouter un rôle de projet

Les rôles de projet sont gérés via les opérations `assign_role` et `remove_role` de `ProjectTool`. Les noms de rôles sont des chaînes personnalisées utilisées pour distinguer les responsabilités des Êtres de Silicium dans les flux de travail et l'attribution des tâches.

## Guide de test

### Tests unitaires

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Préparer
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object> 
            { 
                ["param1"] = "test" 
            }
        };
        
        // Exécuter
        var result = await tool.ExecuteAsync(call);
        
        // Vérifier
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

- La version Default utilise le stockage JSON basé sur des fichiers
- La version Fast utilise le moteur de stockage en mémoire SpeedyPack (format .spk)
- SpeedyPack utilise la cartographie de répertoires en mémoire + cache d'entrées + file d'écriture asynchrone
- Les requêtes par index temporel utilisent l'interface `ITimeStorage`

### Ordonnanceur de la Boucle Principale

- Ordonnancement équitable par créneaux temporels piloté par horloge
- Chien de garde pour détecter les opérations bloquées
- Disjoncteur pour prévenir les défaillances en cascade

## Bonnes pratiques

### 1. Toujours vérifier les autorisations

Toute opération initiée par l'IA doit passer par la chaîne d'autorisations :

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return Result.Denied("Permission denied");
}
```

### 2. Utiliser le localisateur de services

Enregistrement et récupération globaux des services :

```csharp
// Pendant l'initialisation
ServiceLocator.Instance.Register<ICustomService>(myService);

// Lorsque nécessaire
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Suivre la séparation Corps-Cerveau

- Le Corps gère l'état et les déclenchements
- Le Cerveau gère les interactions IA et l'exécution des outils

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
3. Commiter vos modifications avec des commits conventionnels
4. Pousser vers la branche (`git push origin feature/amazing-feature`)
5. Ouvrir une Pull Request

### Format des messages de commit

```
<type>(<scope>): <description>

Exemples :
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Prochaines étapes

- 📚 Lire le [guide d'architecture](architecture.md)
- 📖 Explorer la [référence API](api-reference.md)
- 🔒 Consulter la [documentation de sécurité](security.md)
- 🚀 Consulter le [guide de démarrage rapide](getting-started.md)
