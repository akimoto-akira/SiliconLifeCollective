# PluginDemo-11 : Anti-patron P/Invoke et code unsafe interdits

## Aperçu

Ce plugin démontre les opérations P/Invoke et de code unsafe **interdites** dans le système de plugins SiliconLife. Contrairement aux autres catégories interdites (E/S fichier, réseau, processus, réflexion) qui disposent d'alternatives sécurisées, P/Invoke et le code unsafe sont **strictement prohibés** — sans alternative sûre et non exemptables par aucune déclaration `PluginCapability`.

## Pourquoi P/Invoke est-il la menace ultime ?

P/Invoke et le code unsafe représentent la **menace la plus fondamentale** car ils opèrent **entièrement en dehors du runtime géré** :

- Le code natif s'exécute avec les pleins privilèges du processus
- Aucune sécurité de type, sécurité mémoire ou ramasse-miettes géré
- Impossible d'intercepter, auditer ou isoler les appels natifs
- Crash du code natif = crash de tout le processus (pas de gestion d'exceptions)
- Accès possible à toute adresse mémoire de l'espace du processus

## Mécanisme de triple assurance

PluginLoader utilise **trois couches de détection indépendantes** pour garantir que P/Invoke et le code unsafe ne passent jamais :

### Couche 1 : Scan de la table TypeRef

Détecte les références directes aux types interdits dans les métadonnées PE :

| Type interdit | Espace de noms | Menace |
|---------------|----------------|--------|
| `DllImportAttribute` | System.Runtime.InteropServices | Déclare un import de fonction native |
| `Marshal` | System.Runtime.InteropServices | Pont mémoire géré/non géré |
| `NativeMemory` | System.Runtime.InteropServices | Allocation heap natif malloc/free |
| `NativeLibrary` | System.Runtime.InteropServices | Chargement dynamique de bibliothèques natives |
| `GCHandle` | System.Runtime.InteropServices | Épingler un objet géré, exposer un pointeur |
| `Unsafe` | System.Runtime.CompilerServices | Classe utilitaire Unsafe |
| `UnverifiableCodeAttribute` | System.Security | Marqueur de code non vérifiable |

### Couche 2 : Scan des marqueurs Unsafe (ScanUnsafeMarkers)

Détecte les marqueurs générés par le compilateur indépendamment des références de type :

| Marqueur | Méthode de détection | Source |
|----------|---------------------|--------|
| `[assembly: UnverifiableCode]` | Table CustomAttribute de l'assembly | Mot-clé C# `unsafe` |
| `[module: UnverifiableCode]` | Table CustomAttribute du module | Mot-clé C# `unsafe` |
| `MethodAttributes.PinvokeImpl` | Flag de la table MethodDef | Attribut `[DllImport]` |

### Couche 3 : Scan des chaînes IL (heap #US)

Capture les constantes de chaîne référençant des types InteropServices :

```
"System.Runtime.InteropServices.Marshal"  → Marqué
"System.Runtime.InteropServices.*"        → Marqué par correspondance de préfixe
```

## Violations démontrées

### Violation 1 : Déclaration [DllImport]

```csharp
// ❌ INTERDIT
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();
```

**Détection :**
- `[TypeRef] System.Runtime.InteropServices.DllImportAttribute`
- `[PInvoke] GetTickCount64 (native interop)` (flag PinvokeImpl)

### Violation 2 : Utilisation de Marshal

```csharp
// ❌ INTERDIT
IntPtr ptr = Marshal.AllocHGlobal(1024);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.FreeHGlobal(ptr);
```

**Détection :** `[TypeRef] System.Runtime.InteropServices.Marshal`

### Violation 3 : Utilisation de NativeMemory

```csharp
// ❌ INTERDIT
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Free(buffer);
}
```

**Détection :**
- `[TypeRef] System.Runtime.InteropServices.NativeMemory`
- `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### Violation 4 : Épinglage GCHandle

```csharp
// ❌ INTERDIT
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

**Détection :** `[TypeRef] System.Runtime.InteropServices.GCHandle`

### Violation 5 : Bloc unsafe

```csharp
// ❌ INTERDIT
unsafe
{
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
}
```

**Détection :** `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### Violation 6 : Chargement NativeLibrary

```csharp
// ❌ INTERDIT
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
NativeLibrary.Free(lib);
```

**Détection :** `[TypeRef] System.Runtime.InteropServices.NativeLibrary`

## Aucune alternative sûre — Comparaison

| Catégorie interdite | Wrapper sûr | Auditable | Déclarable via PluginCapability |
|--------------------|-------------|-----------|--------------------------------|
| E/S fichier | PermissionedStreamFactory | ✅ Oui | ✅ Capability.FileIO |
| Réseau | NetworkExecutor | ✅ Oui | ✅ Capability.Network |
| Processus | CommandLineExecutor | ✅ Oui | ✅ Capability.Process |
| Réflexion | ITypeRegistry + IObjectFactory | ✅ Oui | ❌ Toujours interdit |
| **P/Invoke & unsafe** | **❌ Aucun** | **❌ Impossible** | **❌ Toujours interdit** |

## Si un plugin a vraiment besoin de code natif

Si une bibliothèque utilise légitimement P/Invoke ou du code unsafe :

1. **Audit manuel par le mainteneur du projet** requis
2. **Ajout à la liste blanche `TrustedAssemblies`** dans PluginLoader requis
3. **Identification par `AssemblyDefinition.Name` des métadonnées PE** (pas le nom de fichier — prévient les attaques par renommage)

## Fichiers

- `Plugin.cs` - Plugin de démonstration anti-patron
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Ce fichier (Français)
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Exemples connexes

- **04-SafeSystemIO** : Types sûrs de la liste blanche System.IO
- **06-TrustedDependency** : Mécanisme de liste blanche TrustedAssemblies
- **10-ForbiddenReflection** : Opérations de réflexion interdites
- **12-ForbiddenStringBypass** : Tentatives de contournement par chaînes de réflexion
