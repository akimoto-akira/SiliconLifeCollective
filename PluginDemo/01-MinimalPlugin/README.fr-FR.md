# Démo de Plugin Minimal

Une implémentation minimale de `IPlugin` qui illustre le cycle de vie du plugin avec des valeurs codées en dur.

## Aperçu de l'interface IPlugin

Chaque plugin SiliconLife doit implémenter l'interface `IPlugin` définie dans `SiliconLife.Collective`：

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Résumé des propriétés

| Membre | Type | Description |
|--------|------|-------------|
| `Id` | `string` | Identifiant unique, doit être stable entre les versions (ex：`"com.siliconlife.demo.minimal"`) |
| `GetName(Language)` | `string` | Nom d'affichage lisible, localisé par l'énumération `Language` |
| `Version` | `string` | Chaîne de version sémantique (ex：`"1.0.0"`) |
| `GetDescription(Language)` | `string` | Courte description de la fonctionnalité du plugin |
| `GetAuthor(Language)` | `string` | Nom de l'auteur ou de l'organisation |

## Ordre d'appel du cycle de vie

L'hôte appelle les méthodes du cycle de vie dans un ordre strict：

```
OnLoad → OnStart → [En cours d'exécution] → OnStop → OnUnload
```

| Méthode | Quand elle est appelée | Utilisation typique |
|---------|----------------------|---------------------|
| `OnLoad()` | Une fois, lorsque la DLL du plugin est chargée dans l'hôte | Valider la configuration, enregistrer les types, préparer les ressources |
| `OnStart()` | Lorsque l'hôte est entièrement démarré et tous les plugins chargés | Interagir avec d'autres plugins, démarrer des tâches en arrière-plan |
| `OnStop()` | Lorsque l'hôte s'arrête proprement | Libérer les ressources, vider les tampons, sauvegarder l'état |
| `OnUnload()` | Lorsque le plugin est déchargé du processus hôte | Nettoyage final |

## Cette démo

Ce plugin renvoie des valeurs codées en dur pour toutes les propriétés et laisse les méthodes du cycle de vie vides. C'est le point de départ le plus simple pour le développement de plugins.

## Note de sécurité

Les plugins sont chargés dans un `AssemblyLoadContext` isolé et analysés pour les références d'espaces de noms interdits (ex：`System.IO`, `System.Net.Http`). Voir la[documentation de sécurité](../../docs/fr-FR/security.md) pour plus de détails.
