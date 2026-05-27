# PluginDemo-10: Anti-patron de réflexion interdite

## Vue d'ensemble

Ce plugin démontre les opérations de réflexion **interdites** dans le système de plugins SiliconLife. Il sert de référence anti-patron, montrant ce qu'il ne faut PAS faire et fournissant les alternatives correctes pour chaque violation.

## Pourquoi la réflexion est-elle la menace principale ?

Le contournement par réflexion est la **menace la plus critique** pour le scanning de sécurité du PluginLoader. Alors que le scanning TypeRef capture les références de types directes au moment de la compilation, les méthodes de réflexion peuvent résoudre des types à l'**exécution** en utilisant des chaînes — complètement invisibles au scanning statique des métadonnées.

Si un plugin peut appeler `Type.GetType("System.IO.File, System.Runtime")`, il peut accéder à N'IMPORTE QUEL type interdit sans qu'aucune référence n'apparaisse dans la table TypeRef des métadonnées PE.

## Quelles méthodes sont interdites ?

Toutes les méthodes interdites sont détectées via le **scanning MemberRef** (pas de blocage au niveau du namespace ou du type) :

| Méthode interdite | Signature | Menace |
|-------------------|-----------|--------|
| `Type.GetType` | `System.Type::GetType(System.String)` | Résoudre un type arbitraire par nom à l'exécution |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | Instancier des types arbitraires |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | Charger un assembly par nom/bytes |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | Charger un assembly depuis le disque |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | Charger un assembly depuis un chemin |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | Résolution de type basée sur les chaînes |

## Qu'est-ce qui est sûr ?

Toute la réflexion n'est pas interdite. Les modèles suivants sont **sûrs** car ils référencent des types connus à la compilation :

| Modèle sûr | Exemple | Pourquoi c'est sûr |
|------------|---------|-------------------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | Type connu à la compilation, visible dans TypeRef |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | Inspection d'un type connu, aucun nouveau type |
| Contraintes génériques | `FindSubtypesOf(typeof(BaseTool))` | Paramètre générique est un type de compilation |
| `nameof()` | `nameof(MyClass.MyMethod)` | Chaîne de compilation, pas de résolution à l'exécution |

**Distinction clé :**
- `typeof(X).Assembly` → **Sûr** (référence de compilation, scanné par PluginLoader)
- `Assembly.Load("X")` → **Interdit** (chaîne d'exécution, contourne tous les scans)

## Comment remplacer la réflexion en toute sécurité ?

### Utiliser ITypeRegistry (Remplace Type.GetType + scanning AppDomain)

```csharp
// ❌ INTERDIT : Résoudre un type par chaîne à l'exécution
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ CORRECT : Utiliser ITypeRegistry pour trouver les types enregistrés
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// Seuls les types enregistrés pendant OnLoad sont découvrables
```

### Utiliser IObjectFactory (Remplace Activator.CreateInstance)

```csharp
// ❌ INTERDIT : Créer une instance arbitraire
object? instance = Activator.CreateInstance(someType);

// ✅ CORRECT : Utiliser IObjectFactory avec une factory enregistrée
var instance = objectFactory.CreateInstance<MyService>();
// Seuls les types avec des factories enregistrées peuvent être instanciés
```

## Violations démontrées

### Violation 1 : Type.GetType(string)

```csharp
// ❌ INTERDIT
Type? fileType = Type.GetType("System.IO.File, System.Runtime");

// ✅ CORRECT
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**MemberRef bloquée** : `System.Type::GetType(System.String)`

### Violation 2 : Activator.CreateInstance

```csharp
// ❌ INTERDIT
object? client = Activator.CreateInstance(httpClientType!);

// ✅ CORRECT
var instance = objectFactory.CreateInstance<MyService>();
```

**MemberRef bloquée** : `System.Activator::CreateInstance`

### Violation 3 : Assembly.Load

```csharp
// ❌ INTERDIT
Assembly asm = Assembly.Load("System.Net.Http");

// ✅ CORRECT
Assembly myAsm = typeof(MyPlugin).Assembly;  // Sûr : connu à la compilation
```

**MemberRef bloquée** : `System.Reflection.Assembly::Load(System.String)`

### Violation 4 : Assembly.LoadFile / LoadFrom

```csharp
// ❌ INTERDIT
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");

// ✅ CORRECT
// Toutes les dépendances doivent être dans le répertoire du plugin et scannées par PluginLoader.
```

**MemberRef bloquée** : `System.Reflection.Assembly::LoadFile(System.String)`

### Violation 5 : Assembly.GetType(string)

```csharp
// ❌ INTERDIT
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ CORRECT
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
```

**MemberRef bloquée** : `System.Reflection.Assembly::GetType(System.String)`

## Pourquoi typeof(X).Assembly est sûr et Assembly.Load ne l'est pas

| Opération | Visibilité | Sécurité |
|-----------|-----------|----------|
| `typeof(X).Assembly` | Type X dans la table TypeRef → PluginLoader le scanne | ✅ Sûr |
| `Assembly.Load("X")` | Chaîne "X" n'existe qu'à l'exécution → invisible au scan TypeRef | ❌ Interdit |
| `obj.GetType()` | Retourne le type d'une instance existante → pas de nouveau type | ✅ Sûr |
| `Type.GetType("X")` | Résout un type arbitraire depuis une chaîne → contourne TypeRef | ❌ Interdit |

## Bonnes pratiques

1. **Enregistrer les types dans OnLoad** : Utiliser `ITypeRegistry.RegisterType` / `RegisterFromAssembly`
2. **Utiliser IObjectFactory pour la création dynamique** : Ne jamais utiliser `Activator.CreateInstance`
3. **Utiliser typeof(X).Assembly** : Accès sûr à votre propre assembly
4. **Éviter les noms de types basés sur des chaînes** : Déclenche le scanning IL
5. **Concevoir pour la découvrabilité statique** : Invisible dans les métadonnées = suspect

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

- **02-TypeRegistryUsage** : Utilisation correcte d'ITypeRegistry
- **03-ObjectFactoryUsage** : Utilisation correcte d'IObjectFactory
- **11-ForbiddenPInvoke** : P/Invoke et code unsafe interdits
- **12-ForbiddenStringBypass** : Tentatives de contournement par réflexion via chaînes
