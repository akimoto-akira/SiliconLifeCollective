# PluginDemo-18 : Capacité refusée — Anti-pattern de capacité non déclarable

## Aperçu

Ce plugin est un **anti-pattern** démontrant que déclarer une capacité ne contourne PAS les interdictions de capacités non déclarables. Même avec `[PluginCapability(Capability.Network)]`, P/Invoke, Unsafe, Reflection.Emit et l'accès au registre restent **toujours** bloqués.

## Capacités déclarables vs. non déclarables

### ✅ Déclarables (valeurs d'énum Capability existent)

| Capacité | Ce qu'elle exempt |
|---------|------------------|
| `Capability.Network` | Espaces de noms System.Net.* et interdictions par type |
| `Capability.FileIO` | Espace de noms System.IO (au-delà de la liste blanche) |
| `Capability.Process` | Types Process* sous System.Diagnostics |
| `Capability.AI` | Active l'injection IAIService (pas d'exemption TypeRef) |

### ❌ Non déclarables (AUCUNE valeur d'énum Capability n'existe)

| Catégorie | Types bloqués | Pourquoi non déclarable |
|----------|--------------|----------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory`, `NativeLibrary` | Impossible d'auditer du code natif arbitraire à l'exécution |
| Code unsafe | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Contourne la sécurité de type CLR et les vérifications de limites |
| Émission IL | `System.Reflection.Emit.*` | Peut générer du IL arbitraire à l'exécution |
| Chargement d'assembly | `System.Runtime.Loader`, `Assembly.Load*` | Peut charger des DLL non scannées, contournant le scan de sécurité |
| Registre | `Microsoft.Win32.*` | Accès système au niveau OS en dehors du bac à sable du plugin |
| Compilation dynamique | `Microsoft.CodeAnalysis.*` | Peut compiler et exécuter du code arbitraire |
| Réflexion dangereuse | `Type.GetType(string)`, `Activator.CreateInstance` | Peut instancier des types interdits par chaîne |

## Pourquoi ces capacités ne peuvent pas être déclarées

La raison fondamentale : **elles ne peuvent pas être auditées en toute sécurité à l'exécution.**

1. **P/Invoke** : Une fois le code natif appelé, le CLR n'a aucune visibilité — aucune garantie de sécurité
2. **Unsafe** : Contourne le système de sécurité de type dont dépend le modèle de sécurité du plugin
3. **Reflection.Emit** : Peut générer du nouveau IL à l'exécution jamais scanné par PluginLoader
4. **AssemblyLoadContext** : Peut charger des DLL jamais scannées pour la sécurité
5. **Registre** : Fournit un accès à la configuration au niveau OS en dehors du bac à sable du plugin

## Traitement « déclaration invalide » de PluginLoader

Quand PluginLoader rencontre une déclaration de capacité :

1. Lit la valeur d'énum int32 depuis le blob CustomAttribute
2. Vérifie `Enum.IsDefined(typeof(Capability), value)`
3. Si la valeur n'est pas un membre Capability défini → **ignorée silencieusement**
4. Si la valeur est définie → les règles d'exemption sont appliquées
5. **Les vérifications non déclarables sont TOUJOURS appliquées** quelle que soit la capacité déclarée

Cela empêche les plugins de déclarer des capacités « futures » qui n'existent pas encore.

## Comparaison avec 13-CapabilityNetwork

| Aspect | 13-CapabilityNetwork (positif) | 18-CapabilityDenied (anti-pattern) |
|--------|-------------------------------|-----------------------------------|
| Déclaration | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| Utilise HttpClient | ✅ Exempté | ✅ Exempté |
| Utilise DllImport | N/A | ❌ TOUJOURS bloqué |
| Utilise Unsafe | N/A | ❌ TOUJOURS bloqué |
| Résultat du chargement | ✅ CHARGÉ | ❌ REJETÉ |

## Fichiers

- `Plugin.cs` — Plugin de démonstration anti-pattern de capacité non déclarable
- `README.md` — Ce fichier (Anglais)
- `README.zh-CN.md` — Chinois simplifié
- Traductions : zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemples connexes

- **13-CapabilityNetwork** : Exemple positif de Capability.Network
- **11-ForbiddenPInvoke** : Anti-pattern P/Invoke (aucune capacité ne peut aider)
- **10-ForbiddenReflection** : Anti-pattern Reflection (aucune capacité ne peut aider)
