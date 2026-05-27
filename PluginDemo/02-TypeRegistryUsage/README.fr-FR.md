# Démo ITypeRegistry – Enregistrement et requête

Démontre l'enregistrement et la recherche via `ITypeRegistry` : enregistrer des types personnalisés dans `OnLoad`, les découvrir avec `FindSubtypesOf` dans `OnStart`.

## Présentation de l'interface ITypeRegistry

`ITypeRegistry` remplace le scan par réflexion `AppDomain.CurrentDomain.GetAssemblies()`. Les plugins enregistrent explicitement leurs types exposés dans `IPlugin.OnLoad`, et le runtime ne recherche les types que via le registre.

```csharp
public interface ITypeRegistry
{
    void RegisterType(Type type);
    void RegisterTypes(IEnumerable<Type> types);
    void RegisterFromAssembly(System.Reflection.Assembly assembly, Type baseType);
    Type? FindType(string fullName);
    IEnumerable<Type> FindSubtypesOf(Type baseType);
    IEnumerable<Type> FindImplementationsOf(Type interfaceType);
}
```

### Résumé des méthodes

| Méthode | Description |
|---------|-------------|
| `RegisterType(Type)` | Enregistre un seul type |
| `RegisterTypes(IEnumerable<Type>)` | Enregistre plusieurs types à la fois |
| `RegisterFromAssembly(Assembly, Type)` | Enregistre tous les sous-types non abstraits de `baseType` depuis l'assembly spécifié |
| `FindType(string)` | Trouve un type par son nom complet ; prend en charge la résolution des noms de types génériques |
| `FindSubtypesOf(Type)` | Trouve tous les sous-types non abstraits du type de base spécifié |
| `FindImplementationsOf(Type)` | Trouve tous les types non abstraits implémentant l'interface spécifiée |

## Flux d'enregistrement et de requête

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Obtenir ITypeRegistry depuis ServiceLocator              │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  Alternative : RegisterFromAssembly                          │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → enregistrer tous les sous-types DemoTool d'un coup     │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ Itérer les résultats → GreetingTool, FarewellTool, …    │
└──────────────────────────────────────────────────────────────┘
```

## Utilisation de RegisterFromAssembly

`RegisterFromAssembly` scanne un assembly et enregistre tous les sous-types non abstraits du type de base spécifié :

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // l'assembly à scanner
    typeof(DemoTool)                            // enregistrer uniquement les sous-types DemoTool
);
```

Cela équivaut à appeler `RegisterType` pour chaque sous-type individuellement, mais est plus concis lorsqu'un plugin définit de nombreux types partageant une classe de base commune.

## Cette démo

> **⚠️ Important :** `DemoTool` est un **type personnalisé défini uniquement pour cette démo** afin d'illustrer l'enregistrement et la recherche via `ITypeRegistry`. Il n'a **aucun rapport** avec l'interface `ITool` du système (`SiliconLife.Collective.ITool`) utilisée pour l'enregistrement des outils IA. Le nom « Tool » est une coïncidence — toute hiérarchie de classes personnalisée fonctionnerait de la même manière.

| Classe | Rôle |
|--------|------|
| `DemoTool` | Classe de base abstraite personnalisée — ancre d'enregistrement (sans rapport avec `ITool`) |
| `GreetingTool` | Sous-type concret enregistré dans `OnLoad` |
| `FarewellTool` | Sous-type concret enregistré dans `OnLoad` |
| `StatusTool` | Sous-type concret enregistré dans `OnLoad` |
| `TypeRegistryUsagePlugin` | Implémentation `IPlugin` — enregistre et interroge les types |

## Note de sécurité

`ITypeRegistry` fait partie du modèle de sécurité à accès contrôlé. Les plugins ne doivent **pas** utiliser `AppDomain.CurrentDomain.GetAssemblies()` ou `Assembly.GetTypes()` pour découvrir des types — ils doivent passer par `ITypeRegistry`. Voir la [documentation de sécurité](../../docs/fr-FR/security.md).
