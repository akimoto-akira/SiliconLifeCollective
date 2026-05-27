# Démo System.IO sécurisé

Démontre les types System.IO sur la liste blanche `SystemIOAllowedTypes` : `MemoryStream`, `BinaryReader`/`BinaryWriter`, `GZipStream`. Explique pourquoi `FileStream` nécessite `PermissionedStreamFactory`.

## Liste blanche SystemIOAllowedTypes

Le runtime de plugin bloque par défaut l'espace de noms `System.IO`, mais exempte les types qui **n'effectuent pas d'E/S fichier directes** :

| Catégorie | Types autorisés | Pourquoi sûr |
|-----------|----------------|-------------|
| Abstractions de flux | `Stream` | Classe de base abstraite, pas d'E/S propre |
| Flux en mémoire | `MemoryStream` | Opération purement en mémoire |
| Flux de compression | `GZipStream`, `DeflateStream`, `ZLibStream` | Enveloppent un autre flux, n'ouvrent pas de fichiers |
| Enveloppeurs binaires | `BinaryReader`, `BinaryWriter` | Enveloppent n'importe quel flux, n'ouvrent pas de fichiers |
| Énumérations | `SeekOrigin`, `FileMode`, `FileAccess`, `FileShare`, `CompressionMode`, `CompressionLevel` | Types valeur uniquement |
| Exceptions | `IOException`, `InvalidDataException`, `EndOfStreamException` | Types d'erreur uniquement |

### Types absents de la liste blanche

Ces types **accèdent directement au système de fichiers** et sont **bloqués** dans le code de plugin :

| Type bloqué | Raison | Alternative sûre |
|------------|--------|-----------------|
| `FileStream` | Ouvre des fichiers directement | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | Opérations de fichier statiques | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | Opérations de répertoire statiques | `SafePath` (vérification des permissions) |
| `FileInfo` | Encapsule les chemins de fichiers | `SafePath` |
| `DirectoryInfo` | Encapsule les chemins de répertoires | `SafePath` |
| `StreamReader` | Ouvre des fichiers directement | `PermissionedStreamFactory` + envelopper `PermissionedStream` |
| `StreamWriter` | Ouvre des fichiers directement | `PermissionedStreamFactory` + envelopper `PermissionedStream` |

## Pourquoi PermissionedStreamFactory pour FileStream

`FileStream` ouvre directement des fichiers sur disque — un risque de sécurité majeur dans un système de plugins. `PermissionedStreamFactory` impose :

1. **Vérification des permissions** — le `PermissionManager` de l'appelant doit accorder `FileAccess` pour le chemin
2. **Journalisation d'audit** — chaque ouverture de fichier est enregistrée avec l'ID being de l'appelant
3. **Validation du chemin** — les chemins vides/invalides sont rejetés avant toute E/S

```
❌ new FileStream("path", FileMode.Open)           → Bloqué par le scanner TypeRef
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → Vérification des permissions réussie
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → Vérification des permissions réussie
```

## Pipeline de démonstration

Cette démo construit un pipeline de données en mémoire complet en utilisant uniquement les types de la liste blanche :

```
┌─────────────────────────────────────────────────────────────────┐
│  Démo 1 : MemoryStream                                          │
│  └─ Écrire octets → Lire octets → Décoder chaîne                │
│                                                                  │
│  Démo 2 : Pipeline de compression                                │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(compresser) → MemoryStream(compressé)           │
│     → GZipStream(décompresser) → MemoryStream(brut)              │
│     → UTF8 → string (aller-retour)                               │
│                                                                  │
│  Démo 3 : BinaryReader/Writer                                    │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → Vérifier l'aller-retour       │
└─────────────────────────────────────────────────────────────────┘
```

## Cette démo

> **⚠️ Remarque :** Cette démo utilise **uniquement** les types de la liste blanche `SystemIOAllowedTypes`. Aucune E/S fichier n'est effectuée. Pour l'accès aux fichiers, voir l'API `PermissionedStreamFactory`.

| Classe | Rôle |
|--------|------|
| `SafeSystemIOPlugin` | Implémentation `IPlugin` — démontre l'utilisation sûre de System.IO |

## Note de sécurité

L'espace de noms `System.IO` est bloqué par le scanner TypeRef du plugin. Seuls les types de la liste blanche passent. Pour l'accès réel aux fichiers, `PermissionedStreamFactory` doit être utilisé, qui effectue des vérifications de permissions et une journalisation d'audit. Voir la [documentation de sécurité](../../docs/fr-FR/security.md).
