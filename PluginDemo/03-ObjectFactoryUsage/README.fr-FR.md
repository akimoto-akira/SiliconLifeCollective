# Démo IObjectFactory – Enregistrement et création d'instances

Démontre l'enregistrement et la création d'instances avec `IObjectFactory` : enregistrer les types avec `RegisterAutoFactory` dans `OnLoad`, créer des instances avec `CreateInstance` dans `OnStart`.

## Présentation de l'interface IObjectFactory

`IObjectFactory` remplace `Activator.CreateInstance()`. Les plugins enregistrent des délégués de fabrique dans `IPlugin.OnLoad`, et le runtime crée des instances uniquement via les délégués enregistrés, empêchant l'instanciation arbitraire de types.

```csharp
public interface IObjectFactory
{
    void RegisterFactory(Type type, Func<object?[], object> factory);
    void RegisterFactory<T>(Func<object?[], T> factory) where T : class;
    void RegisterAutoFactory(Type type);
    void RegisterAutoFactoryFromAssembly(System.Reflection.Assembly assembly, Type baseType);
    object? CreateInstance(Type type, params object?[] args);
    T? CreateInstance<T>(params object?[] args) where T : class;
    bool IsRegistered(Type type);
}
```

### Résumé des méthodes

| Méthode | Description |
|---------|-------------|
| `RegisterFactory(Type, Func)` | Enregistre un délégué de fabrique personnalisé pour un type |
| `RegisterFactory<T>(Func)` | Version générique de `RegisterFactory` |
| `RegisterAutoFactory(Type)` | Analyse automatiquement les constructeurs du type et enregistre une fabrique |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | Enregistre automatiquement des fabriques pour tous les sous-types non abstraits d'un assembly |
| `CreateInstance(Type, args)` | Crée une instance via une fabrique enregistrée (non générique) |
| `CreateInstance<T>(args)` | Crée une instance via une fabrique enregistrée (générique) |
| `IsRegistered(Type)` | Vérifie si une fabrique est enregistrée pour un type |

## Pourquoi IObjectFactory remplace Activator.CreateInstance

`Activator.CreateInstance` permet l'instanciation arbitraire de types, ce qui constitue un risque de sécurité dans un système de plugins. `IObjectFactory` impose un modèle de liste blanche :

- Seuls les types avec une **fabrique enregistrée** peuvent être instanciés
- Les fabriques sont enregistrées explicitement dans `OnLoad`, l'hôte garde le contrôle total
- `RegisterAutoFactory` est une méthode pratique qui analyse les constructeurs mais nécessite toujours l'enregistrement

```
❌ Activator.CreateInstance(typeof(SomeType))     → risque de sécurité
✅ factory.CreateInstance(typeof(SomeType))         → types enregistrés uniquement
✅ factory.CreateInstance<SomeType>()               → méthode générique pratique
```

## Fonctionnement de RegisterAutoFactory

`RegisterAutoFactory` inspecte les constructeurs du type et génère un délégué de fabrique :

1. **Sans arguments** → appelle le constructeur sans paramètres
2. **Avec arguments** → correspond aux paramètres du constructeur par type, retour au constructeur sans paramètres en cas d'échec
3. **Types abstraits/interfaces** → rejetés avec avertissement

## Flux d'enregistrement et de création

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Obtenir IObjectFactory depuis ServiceLocator             │
│  ├─ RegisterAutoFactory(typeof(SimpleService))               │
│  └─ RegisterAutoFactory(typeof(ConfiguredService))           │
│                                                              │
│  OnStart                                                     │
│  ├─ CreateInstance(typeof(SimpleService))                    │
│  │  → "SimpleService created via parameterless constructor"  │
│  └─ CreateInstance<ConfiguredService>("DemoPlugin")          │
│     → "ConfiguredService created with name='DemoPlugin'"     │
└──────────────────────────────────────────────────────────────┘
```

## Cette démo

> **⚠️ Remarque :** `SimpleService` et `ConfiguredService` sont des **types personnalisés définis uniquement pour cette démo**. Ils n'ont aucun lien avec les interfaces de service du système.

| Classe | Rôle |
|--------|------|
| `SimpleService` | Type de démo, constructeur sans paramètres |
| `ConfiguredService` | Type de démo, constructeur paramétré `(string name)` |
| `ObjectFactoryUsagePlugin` | Implémentation `IPlugin` — enregistre les fabriques et crée des instances |

## Note de sécurité

`IObjectFactory` fait partie du modèle de sécurité à accès contrôlé. Les plugins ne doivent **pas** utiliser `Activator.CreateInstance` pour créer des objets — ils doivent enregistrer des fabriques et utiliser `CreateInstance`. Voir la [documentation de sécurité](../../docs/fr-FR/security.md).
