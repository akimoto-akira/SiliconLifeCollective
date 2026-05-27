# PluginDemo-12 : Anti-Pattern de Contournement par Chaînes de Réflexion Interdites

## Aperçu

Ce plugin démontre des tentatives **interdites** de contournement par chaînes de réflexion dans le système de plugins SiliconLife. Il montre pourquoi la concaténation, l'interpolation, l'encodage et les autres techniques d'obfuscation **ne peuvent pas** contourner le scan du tas #US (User String) de PluginLoader — la **dernière ligne de défense**.

## Qu'est-ce que le tas #US ?

Dans les métadonnées .NET PE (Portable Executable), le **tas #US (User String)** stocke tous les opérandes de littéraux de chaîne utilisés par les instructions IL `ldstr`. Chaque fois que vous écrivez un littéral de chaîne en C#, le compilateur le stocke dans ce tas.

```
Source C# :   string s = "System.IO.File";
    ↓ compilation
Code IL :     ldstr "System.IO.File"    ← référence un token dans le tas #US
    ↓ scan PluginLoader
Tas #US :     [..., "System.IO.File", ...]  ← DÉTECTÉ par correspondance de préfixe !
```

La méthode `ScanUserStrings()` de PluginLoader parcourt **chaque entrée** du tas #US, vérifiant si une chaîne commence par un préfixe interdit.

## Préfixes de chaînes interdits

Les préfixes suivants déclenchent des violations `[ILString]` lorsqu'ils sont trouvés dans le tas #US :

| Préfixe | Catégorie |
|---------|-----------|
| `System.IO.` | Types de système de fichiers |
| `System.Net.Http` | Client HTTP |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | Sockets bruts |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | Sondage réseau |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | Processus/ligne de commande |
| `Microsoft.CodeAnalysis` | Compilateur Roslyn |
| `System.Reflection.Emit` | Émission IL |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | CodeDom hérité |
| `Microsoft.Win32` | Registre Windows |

## Violations démontrées

### Violation 1 : Chaîne de nom de type directe

```csharp
// ❌ INTERDIT — la chaîne complète est dans le tas #US
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**Violation** : `[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### Violation 2 : Concaténation de chaînes (compilation)

```csharp
// ❌ INTERDIT — le compilateur replie const+const en une entrée #US
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
```

**Violation** : `[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### Violation 3 : Interpolation de chaînes

```csharp
// ❌ INTERDIT — les parties littérales sont stockées dans le tas #US
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
```

**Violation** : `[ILString] "System.IO." matches forbidden prefix "System.IO."`

### Violation 4 : Champs Const

```csharp
// ❌ INTERDIT — les valeurs const sont inlinées → apparaissent dans le tas #US
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**Violation** : `[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### Violation 5 : Fragments de chaînes partielles

```csharp
// ❌ INTERDIT — chaque partie est un ldstr séparé, scanné indépendamment
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
```

**Violation** : `[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

## Pourquoi les techniques d'obfuscation échouent toutes

| Technique | Pourquoi elle échoue |
|-----------|---------------------|
| Concaténation const | Le compilateur replie en une seule entrée #US |
| Interpolation de chaînes | Les parties littérales stockées dans le tas #US |
| Champs const | Valeurs inlinées → apparaissent dans #US |
| Division en variables | Chaque opérande `ldstr` scanné indépendamment |
| Encodage Base64 | Le décodage nécessite des méthodes runtime, mais `Type.GetType` est bloqué par MemberRef |
| Construction par tableau de char | Pas de `ldstr` émis, mais `Type.GetType` reste bloqué par MemberRef |
| Chiffrement XOR | Chaîne chiffrée illisible dans #US, mais déchiffrement + `Type.GetType` = MemberRef bloqué |

**Insight clé** : Le scan #US bloque la **chaîne**. Le scan MemberRef bloque la **méthode**. Pour charger dynamiquement un type, vous avez besoin des DEUX. PluginLoader bloque les DEUX indépendamment.

## La chaîne de défense complète

| Étape | Mécanisme | Ce qui est détecté |
|-------|-----------|-------------------|
| 1 | Table TypeRef | Références directes aux types interdits |
| 2 | Table ExportedType | Types redirigés depuis des espaces de noms interdits |
| 3 | Table MemberRef | Appels à `Type.GetType`, `Assembly.Load`, `Activator.CreateInstance` |
| 4 | Marqueurs Unsafe | `[DllImport]`, blocs unsafe, drapeau PinvokeImpl |
| **5** | **Scan du tas #US** | **Constantes de chaîne correspondant aux préfixes interdits (ce démo)** |

## Fichiers

- `Plugin.cs` - Plugin de démonstration anti-pattern
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

- **10-ForbiddenReflection** : Méthodes de réflexion interdites (scan MemberRef)
- **11-ForbiddenPInvoke** : P/Invoke et code unsafe interdits
- **02-TypeRegistryUsage** : Utilisation correcte de ITypeRegistry
- **03-ObjectFactoryUsage** : Utilisation correcte de IObjectFactory
