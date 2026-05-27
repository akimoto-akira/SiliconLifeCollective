# Démo de dépendance de confiance

Démontre l'utilisation de `Newtonsoft.Json` — une bibliothèque qui utilise intensivement la réflexion en interne — comme assembly de confiance. Le scanner de sécurité du PluginLoader ignore complètement les assemblies de confiance, permettant aux plugins de les référencer sans déclencher de violations.

## Mécanisme de liste blanche TrustedAssemblies

Le `PluginLoader` maintient une liste blanche statique de bibliothèques open-source qui sont **de confiance par défaut** :

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // Sérialisation
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← Cette démo utilise cette bibliothèque
    "MessagePack",
    "YamlDotNet",

    // Journalisation
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // Accès aux données / mapping
    "Dapper", "AutoMapper",

    // Validation et distribution de messages
    "FluentValidation", "MediatR",
};
```

### Critères d'admission

Une bibliothèque peut être ajoutée à `TrustedAssemblies` si elle remplit **les trois** critères :

| # | Critère | Justification |
|---|---------|---------------|
| 1 | Projet open-source largement utilisé (MIT / Apache 2.0 / BSD) | Code auditable publiquement |
| 2 | Code source accessible publiquement | La surveillance communautaire garantit l'absence de comportement malveillant |
| 3 | Package NuGet maintenu par un fournisseur/communauté de confiance | Intégrité de la chaîne d'approvisionnement |

### Base d'identification

Le scanner identifie les assemblies de confiance par leur `AssemblyDefinition.Name` dans les métadonnées PE — **pas par le nom du fichier DLL**. Cela empêche les attaquants de renommer une DLL malveillante en `Newtonsoft.Json.dll` pour contourner les vérifications.

## CollectTrustedTypeRefs — Exemption transitive

Lorsque le PluginLoader charge un répertoire de plugin, il effectue un scan en deux phases :

```
Phase 1 : CollectTrustedTypeRefs(pluginDir)
├── Énumérer tous les fichiers *.dll dans le répertoire du plugin
├── Pour chaque DLL : lire les métadonnées PE → vérifier AssemblyDefinition.Name
├── Si nom ∈ TrustedAssemblies :
│   └── Collecter TOUTES les entrées TypeReference → paires (namespace, typeName)
└── Retourne : HashSet<(string Namespace, string Name)>

Phase 2 : ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── Couche 0 :   Sortie rapide liste blanche (si DLL principale est de confiance → passe)
├── Couche 0.5 : Exemption transitive (ignorer les TypeRefs dans l'ensemble trustedTypeRefs)
├── Couche 1 :   Scan de la table TypeRef
├── Couche 2 :   Scan de la table ExportedType
├── Couche 3 :   Scan de la table MemberRef (méthodes dangereuses)
├── Couche 4 :   Marqueurs de code non sûr + P/Invoke
└── Couche 5 :   Scan du tas de chaînes #US
```

### Pourquoi l'exemption transitive est importante

Newtonsoft.Json référence en interne des types comme `System.Reflection.MemberInfo`, `System.IO.TextReader`, etc. Quand votre plugin référence Newtonsoft.Json, le compilateur peut incorporer ces TypeRefs transitifs dans la DLL de **votre** plugin. Sans exemption transitive, votre plugin serait signalé pour avoir référencé `System.IO.TextReader` — même si vous ne l'utilisez jamais directement.

`CollectTrustedTypeRefs` résout ce problème en pré-collectant tous les TypeRefs des DLL de confiance et en les marquant comme « connus sûrs » pendant le scan principal.

## Comment ajouter une nouvelle dépendance de confiance

Pour ajouter une nouvelle bibliothèque à la liste blanche :

1. Vérifier qu'elle remplit les trois critères d'admission ci-dessus
2. Ajouter une ligne au HashSet `TrustedAssemblies` dans `PluginLoader.cs` :
   ```csharp
   "YourLibraryName",  // Brève description de pourquoi elle est de confiance
   ```
3. Placer la DLL de la bibliothèque dans le répertoire du plugin (à côté de la DLL principale du plugin)
4. Le scanner collectera automatiquement ses TypeRefs et les exemptera

> **⚠️ Important :** Ajouter une bibliothèque à `TrustedAssemblies` signifie que le scanner **ne** vérifiera **pas** son code interne. N'ajoutez que des bibliothèques auxquelles vous faites pleinement confiance.

## Cette démo

Ce plugin utilise Newtonsoft.Json sans aucune déclaration `PluginCapability` :

| Fonction | Comportement interne de Newtonsoft.Json | Pourquoi ça fonctionne |
|----------|------------------------------------------|------------------------|
| `JsonConvert.SerializeObject` | Utilise la réflexion pour énumérer les propriétés | La DLL Newtonsoft.Json passe la liste blanche de couche 0 |
| `JsonConvert.DeserializeObject<T>` | Appelle `Activator.CreateInstance`, définit les propriétés via réflexion | TypeRefs transitifs exemptés en couche 0.5 |
| Manipulation `JObject` / `JArray` | Utilise `System.Linq.Expressions`, dispatch dynamique | Toutes les refs internes collectées par `CollectTrustedTypeRefs` |

### Différence clé avec PluginCapability

| Mécanisme | Portée | Cas d'utilisation |
|-----------|--------|-------------------|
| `TrustedAssemblies` | Exempte une **bibliothèque** entière (et ses refs transitives) du scan | Dépendances open-source connues |
| `PluginCapability` | Exempte le **code de votre plugin** des interdictions de namespaces spécifiques | Le plugin a besoin d'un accès direct à System.Net/IO/Process |

Un plugin utilisant uniquement des dépendances de confiance **n'a pas besoin** de déclaration `PluginCapability`. Le scanner gère tout automatiquement.

## Note de sécurité

Les assemblies de confiance sont exemptées du scan de sécurité car ce sont des projets open-source auditables. Cependant, **le code de votre plugin** est toujours entièrement scanné. Si votre plugin référence directement `System.IO.File` ou `System.Net.Http.HttpClient`, il sera toujours bloqué — sauf si vous déclarez le `PluginCapability` correspondant. Voir la [documentation de sécurité](../../docs/fr-FR/security.md).
