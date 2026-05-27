# Opérations d'E/S fichier interdites — Anti-modèle

Démontre les opérations d'E/S fichier **interdites** dans le système de plugins. Cet exemple sert de référence d'anti-modèle, montrant ce qu'il ne faut PAS faire et fournissant les alternatives correctes pour chaque violation.

## Pourquoi System.IO est-il globalement interdit ?

L'ensemble du namespace `System.IO` est bloqué au niveau plugin car l'accès direct aux fichiers présente des risques de sécurité graves :

1. **Accès non autorisé aux fichiers** : Les plugins pourraient lire des fichiers sensibles hors du workspace (mots de passe, clés, données personnelles)
2. **Attaques par écrasement** : Des plugins malveillants pourraient écraser des fichiers système ou de configuration critiques
3. **Traversée de répertoires** : Les plugins pourraient utiliser des chemins `../` pour sortir des limites du workspace
4. **Épuisement des ressources** : La création non contrôlée de fichiers pourrait saturer l'espace disque
5. **Absence de piste d'audit** : Les opérations directes sur fichiers contournent le système d'audit de sécurité des plugins

## Types interdits

Tous les types `System.IO` accédant directement au système de fichiers sont bloqués :

| Type interdit | Méthode bloquée | Niveau de risque |
|--------------|-----------------|-----------------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` etc. | 🔴 Critique |
| `FileStream` | Constructeur avec chemin de fichier | 🔴 Critique |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 Critique |
| `StreamReader` | Constructeur avec chemin (chaîne) | 🔴 Critique |
| `StreamWriter` | Constructeur avec chemin (chaîne) | 🔴 Critique |
| `FileInfo` | Toutes les méthodes | 🔴 Critique |
| `DirectoryInfo` | Toutes les méthodes | 🔴 Critique |

## Types autorisés (exceptions de la liste blanche)

Les types effectuant des **opérations purement en mémoire** (sans accès direct au système de fichiers) sont autorisés :

| Type autorisé | Usage | Pourquoi c'est sûr |
|--------------|-------|---------------------|
| `MemoryStream` | Flux d'octets en mémoire | Aucun accès au système de fichiers |
| `BinaryReader` | Lecture depuis un flux existant | Enveloppe un flux, n'ouvre pas de fichiers |
| `BinaryWriter` | Écriture dans un flux existant | Enveloppe un flux, ne crée pas de fichiers |
| `GZipStream` | Compression/décompression | Enveloppe un flux, aucun accès fichier |
| `StreamReader` | Constructeur avec paramètre `Stream` | Sûr lorsqu'il enveloppe un flux audité |
| `StreamWriter` | Constructeur avec paramètre `Stream` | Sûr lorsqu'il enveloppe un flux audité |

Voir l'exemple **04-SafeSystemIO** pour les types autorisés.

## Accès sécurisé aux fichiers via PermissionedStreamFactory

`PermissionedStreamFactory` est le **point d'entrée contrôlé** pour les opérations fichier dans les plugins :

```csharp
// ✅ Correct : lire un fichier
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ Correct : écrire un fichier
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("Données de log");
```

**PermissionedStreamFactory fournit :**
1. **Validation du chemin** : Empêche les attaques par traversée de répertoires (`../`)
2. **Vérification des permissions** : Garantit que le fichier se trouve dans le workspace autorisé
3. **Journal d'audit** : Tous les accès fichier sont enregistrés pour revue de sécurité
4. **Nettoyage des ressources** : Suit les flux ouverts et prévient les fuites

## Violations dans cet exemple

### Violation 1 : File.ReadAllText

```csharp
// ❌ Interdit — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ Alternative correcte
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### Violation 2 : File.WriteAllText

```csharp
// ❌ Interdit — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ Alternative correcte
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### Violation 3 : FileStream direct

```csharp
// ❌ Interdit — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ Alternative correcte
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### Violation 4 : Directory.GetFiles

```csharp
// ❌ Interdit — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ Alternative correcte (avec SpeedyPack)
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### Violation 5 : StreamReader avec chemin direct

```csharp
// ❌ Interdit — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ Alternative correcte
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## Comparaison avec d'autres exemples

| Exemple | Focus | Permission requise |
|---------|-------|-------------------|
| **04-SafeSystemIO** | Types mémoire autorisés (MemoryStream, GZipStream) | Aucune |
| **07-ForbiddenFileIO** | Modèles d'accès fichier interdits (cet exemple) | Non applicable (bloqué) |
| **14-CapabilityFileIO** | Déclarer la capacité FileIO pour contourner les restrictions | `Capability.FileIO` |

## Mécanisme de scan de sécurité du PluginLoader

Lorsque PluginLoader scanne ce plugin :

1. **Scan TypeRef** : Détecte les références aux types `System.IO` interdits
2. **Scan MemberRef** : Détecte les appels aux méthodes bloquées
3. **Scan de chaînes IL** : Détecte les tentatives de contournement par réflexion basée sur les chaînes
4. **Rejet** : Le plugin est rejeté lors du chargement avec un message d'erreur détaillé

Le contournement par concaténation de chaînes, réflexion, chargement dynamique ou obfuscation est impossible — ceux-ci sont capturés par le scan au niveau IL (voir **12-ForbiddenStringBypass**).

## Note de sécurité

Si vous avez vraiment besoin d'un accès fichier sans restriction, vous pouvez déclarer `Capability.FileIO` (voir 14-CapabilityFileIO). Cependant, les bonnes pratiques sont :
- Privilégier **SpeedyPack** pour le stockage de données structurées (aucune déclaration de permission nécessaire)
- Utiliser **PermissionedStreamFactory** quand l'accès fichier est nécessaire (point d'entrée contrôlé)
- Ne déclarer `Capability.FileIO` que si les solutions ci-dessus ne suffisent pas
