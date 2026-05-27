# PluginDemo-20 : SpeedyPack — Stockage de données structurées

## Aperçu

Ce plugin illustre l'utilisation de `SpeedyPack` pour le stockage de données structurées **sans aucune déclaration de capacité**. SpeedyPack est la méthode **recommandée** pour la persistance des données des plugins.

## Pourquoi SpeedyPack ?

| Fonctionnalité | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|---------------|-----------|--------------------------|------------------------------|
| Capacité requise | **Aucune** | Aucune | `Capability.FileIO` |
| Cache | ✅ Intégré | ❌ | ❌ |
| WAL (récupération après crash) | ✅ | ❌ | ❌ |
| Transactions | ✅ `IPackTransaction` | ❌ | ❌ |
| Thread-safe | ✅ | ❌ | ❌ |
| Sérialisation structurée | ✅ `Read<T>` | ❌ Octets bruts | ❌ Manuel |
| Piste d'audit | ✅ Automatique | ✅ Automatique | ❌ Manuel |

## CRUD de base

```csharp
// Ouvrir un fichier de données SpeedyPack
using var pack = SpeedyPack.Open("mydata.spk");

// Écrire des paires clé-valeur
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// Lire les valeurs (typées)
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// Supprimer une clé
pack.Delete("user:age");

// Vérifier l'existence
bool exists = pack.Contains("user:name");  // true
```

## Accès typé avec objets structurés

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// Écrire un objet structuré
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// Lire un objet typé
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## Transactions

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // Atomique — les deux écritures sont persistées ou aucune
    }
    catch
    {
        tx.Rollback();  // Annuler toutes les écritures de cette transaction
    }
}
```

### Méthodes IPackTransaction

| Méthode | Description |
|---------|-------------|
| `Write(key, value)` | Mettre en file d'attente une opération d'écriture |
| `Delete(key)` | Mettre en file d'attente une opération de suppression |
| `Commit()` | Appliquer atomiquement toutes les opérations en file d'attente |
| `Rollback()` | Annuler toutes les opérations en file d'attente |

## Configuration avec SpeedyPackOptions

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 Mo de cache
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### Propriétés SpeedyPackOptions

| Propriété | Type | Par défaut | Description |
|-----------|------|-----------|-------------|
| `MaxCacheSize` | `long` | 64 Mo | Taille maximale du cache en mémoire |
| `AutoFlushInterval` | `TimeSpan` | 10 secondes | Intervalle de vidage du cache vers le disque |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | Niveau de compression pour les données stockées |

## Note de sécurité

SpeedyPack ne nécessite **aucune** déclaration de capacité. C'est un point d'entrée de stockage de données sûr et contrôlé qui :
- Valide tous les chemins par rapport aux limites de l'espace de travail
- Fournit une piste d'audit complète de toutes les opérations de lecture/écriture
- Prévient les attaques par traversée de répertoire
- Gère automatiquement le cycle de vie des ressources

## Fichiers

- `Plugin.cs` — Plugin de démonstration SpeedyPack
- `README.md` — Ce fichier (Anglais)
- `README.zh-CN.md` — Chinois simplifié
- Traductions : zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemples connexes

- **04-SafeSystemIO** : Types System.IO en mémoire autorisés (aucune déclaration nécessaire)
- **07-ForbiddenFileIO** : Anti-pattern des opérations de fichier bloquées
- **14-CapabilityFileIO** : Quand SpeedyPack ne suffit pas
