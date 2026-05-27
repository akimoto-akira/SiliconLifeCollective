# PluginDemo-14 : Capability.FileIO — Permission d'E/S fichier déclarative

## Aperçu

Ce plugin illustre l'utilisation de `[PluginCapability(Capability.FileIO)]` pour déclarer l'accès direct au système de fichiers. Avec cette déclaration, le plugin accède à tous les types `System.IO` au-delà de la liste blanche `SystemIOAllowedTypes`.

## Syntaxe de déclaration PluginCapability

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Fonctionnement de Capability.FileIO

1. **État par défaut** : L'espace de noms `System.IO` est interdit globalement ; seuls les types de la liste blanche `SystemIOAllowedTypes` sont autorisés (MemoryStream, BinaryReader, GZipStream, etc.)
2. **Avec déclaration** : L'interdiction de tout l'espace de noms `System.IO` est levée — File, FileStream, Directory, StreamReader(string), etc. deviennent accessibles
3. **Exemption ILString** : Les constantes de chaîne commençant par `"System.IO."` ne sont pas signalées
4. **Limites non déclarables** : P/Invoke, Unsafe, Reflection.Emit, etc. restent bloqués

## Portée d'exemption de Capability.FileIO

### Exemptions TypeRef

Tous les types `System.IO` sont exemptés :

| Catégorie | Types exemptés |
|-----------|---------------|
| Opérations sur les fichiers | `File`, `FileInfo` |
| Opérations sur les répertoires | `Directory`, `DirectoryInfo` |
| Types de flux | `FileStream`, `StreamReader(path)`, `StreamWriter(path)` |
| Système de fichiers | `FileSystemWatcher`, `DriveInfo`, `Path` |

### Exemption ILString

- Les chaînes commençant par `"System.IO."` ne sont pas signalées

### Ce qui reste interdit

| Catégorie | Toujours bloqué |
|-----------|----------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` |
| Code unsafe | `UnverifiableCodeAttribute`, `Unsafe` |
| Émission IL | `System.Reflection.Emit.*` |
| Chargement d'assembly | `System.Runtime.Loader`, `Assembly.Load*` |
| Registre | `Microsoft.Win32.*` |

## Comparaison avec d'autres exemples

| Exemple | Déclaration | Accès fichier | Remarques |
|---------|------------|--------------|----------|
| **04-SafeSystemIO** | Aucune | MemoryStream, BinaryReader, GZipStream | Utilise uniquement les types de la liste blanche |
| **07-ForbiddenFileIO** | Aucune | ❌ REJETÉ | Anti-pattern |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ Accès complet à System.IO | Cet exemple |
| **20-SpeedyPack** | Aucune | Via l'API SpeedyPack (aucune Capability requise) | Stockage de données recommandé |

## Ordre de priorité pour l'accès aux fichiers

1. **SpeedyPack** — Aucune déclaration de capacité nécessaire. Cache intégré, WAL, transactions. **Recommandé pour le stockage de données structurées.**
2. **PermissionedStreamFactory** — Aucune déclaration nécessaire. Accès audité avec validation de chemin et contrôle d'accès.
3. **Capability.FileIO + System.IO direct** — Uniquement lorsque les options ci-dessus ne suffisent pas.

## Pourquoi préférer PermissionedStreamFactory / SpeedyPack ?

Même avec `Capability.FileIO`, l'utilisation de points d'entrée contrôlés est recommandée car :

1. **Piste d'audit** : Tous les accès sont journalisés et traçables
2. **Validation de chemin** : Prévient les attaques de traversée de répertoire (`../`)
3. **Contrôle d'accès** : Application des limites de l'espace de travail
4. **Suivi des ressources** : Prévient les fuites de flux et l'épuisement des ressources
5. **Conformité** : Les modèles d'accès contrôlé facilitent les revues de sécurité

## Bonnes pratiques de sécurité

1. **Ne déclarer FileIO que si véritablement nécessaire** : SpeedyPack ou PermissionedStreamFactory peuvent-ils suffire ?
2. **Fournir une Reason claire** : « Direct log file access for audit trail » est meilleur que « file access »
3. **Valider les chemins soi-même** : Même avec Capability.FileIO, valider tous les chemins de fichiers avant utilisation
4. **Utiliser les instructions using** : Toujours disposer FileStream/StreamReader/StreamWriter
5. **Principe du moindre privilège** : Ne déclarer que les capacités réellement nécessaires au plugin

## Fichiers

- `Plugin.cs` — Plugin de démonstration déclarant Capability.FileIO
- `README.md` — Ce fichier (Anglais)
- `README.zh-CN.md` — Chinois simplifié
- Traductions : zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemples connexes

- **04-SafeSystemIO** : Types System.IO en mémoire autorisés (aucune déclaration requise)
- **07-ForbiddenFileIO** : Anti-pattern des opérations fichier bloquées
- **20-SpeedyPack** : Stockage de données recommandé sans déclaration de capacité
- **18-CapabilityDenied** : Anti-pattern des capacités non déclarables
