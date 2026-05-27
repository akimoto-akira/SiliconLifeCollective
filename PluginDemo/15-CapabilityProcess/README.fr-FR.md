# PluginDemo-15 : Capability.Process — Permission de processus déclarative

## Aperçu

Ce plugin illustre l'utilisation de `[PluginCapability(Capability.Process)]` pour déclarer la capacité d'un plugin à lancer des processus enfants. Avec cette déclaration, le plugin accède à `System.Diagnostics.Process` et aux types associés.

## Syntaxe de déclaration

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Portée d'exemption de Capability.Process

### Exemptions TypeRef

Seuls les types liés à Process sous `System.Diagnostics` sont exemptés :

| Type exempté | Utilisation |
|-------------|------------|
| `Process` | Démarrer, gérer et surveiller les processus enfants |
| `ProcessStartInfo` | Configurer les paramètres de démarrage du processus |
| `ProcessThread` | Accéder aux informations de thread du processus |
| `ProcessModule` | Accéder aux informations de module du processus |
| `ProcessPriorityClass` | Définir la priorité du processus |
| `ProcessWindowStyle` | Configurer le style de fenêtre du processus |

Types toujours autorisés (jamais dans la liste d'interdiction) : `Stopwatch`, `Debug`, `Trace`, `Activity`

### Exemption ILString

- Les chaînes commençant par `"System.Diagnostics.Process"` ne sont pas signalées

## Comparaison avec 09-ForbiddenProcess

| Aspect | 09-ForbiddenProcess | 15-CapabilityProcess |
|--------|-------------------|---------------------|
| Déclaration | Aucune | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ REJETÉ | ✅ AUTORISÉ |
| ProcessStartInfo | ❌ REJETÉ | ✅ AUTORISÉ |

## Recommandation : CommandLineExecutor

Même avec `Capability.Process`, l'utilisation de `CommandLineExecutor` est recommandée en priorité :

| Fonctionnalité | CommandLineExecutor | Processus direct |
|---------------|-------------------|-----------------|
| Déclaration de capacité requise | Non | Oui |
| Bac à sable | Liste blanche de commandes | Aucun |
| Délais d'attente | Intégré | Manuel |
| Capture de sortie | Structurée | Manuelle |
| Journalisation d'audit | Automatique | Manuelle |

Utilisez `Capability.Process` + `Process` direct uniquement lorsque vous avez besoin d'un contrôle fin sur les flux I/O, du traitement des événements de processus, ou lorsque la liste blanche de CommandLineExecutor est trop restrictive.

## Bonnes pratiques de sécurité

1. **Préférer CommandLineExecutor** : Utiliser un point d'entrée contrôlé lorsque possible
2. **Fournir une Reason claire** : « Launch build tools for CI pipeline » plutôt qu'un vague « process access »
3. **Valider toutes les entrées** : Ne jamais transmettre d'entrée non fiable directement à ProcessStartInfo
4. **Utiliser WaitForExit** : Toujours attendre la fin du processus pour éviter les processus zombies
5. **Rediriger les flux** : Définir `RedirectStandardOutput = true` et `UseShellExecute = false`

## Fichiers

- `Plugin.cs` — Plugin de démonstration déclarant Capability.Process
- `README.md` — Ce fichier (Anglais)
- `README.zh-CN.md` — Chinois simplifié
- Traductions : zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemples connexes

- **09-ForbiddenProcess** : Anti-pattern des opérations de processus bloquées
- **18-CapabilityDenied** : Anti-pattern des capacités non déclarables
