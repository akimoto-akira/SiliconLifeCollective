# PluginDemo-16 : Capability.AI — Permission de service IA déclarative

## Aperçu

Ce plugin illustre l'utilisation de `[PluginCapability(Capability.AI)]` pour déclarer qu'un plugin nécessite l'accès au service IA. Contrairement aux autres capacités, `Capability.AI` n'exempte **aucun** espace de noms interdit — elle permet à l'hôte d'injecter une référence `IAIService` dans le plugin.

## Concept clé : Capability.AI n'accorde pas d'accès réseau

`Capability.AI` est fondamentalement différente des autres capacités :

| Capacité | Ce qu'elle exempte | Comment elle fonctionne |
|---------|-------------------|------------------------|
| `Capability.Network` | Espaces de noms `System.Net.*` | Assouplit les règles de scan TypeRef/ILString |
| `Capability.FileIO` | Espace de noms `System.IO` | Assouplit les règles de scan TypeRef/ILString |
| `Capability.Process` | Types `Process*` | Assouplit les règles de scan TypeRef/ILString |
| `Capability.AI` | **Rien** | Active l'injection IAIService par l'hôte |

`IAIService` se trouve dans l'espace de noms `SiliconLife.Collective` — il n'est jamais dans aucune liste d'interdiction. La déclaration de capacité est un **signal d'opt-in** à l'hôte indiquant que ce plugin doit recevoir la référence du service IA.

## Empilage de capacités : IA + Réseau

Si votre client IA nécessite un accès réseau direct (par ex., appel d'un endpoint IA distant), vous devez déclarer **les deux** capacités :

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

Voir **17-CapabilityStacked** pour des exemples complets d'empilage.

## Modèle de point d'entrée contrôlé

| Ressource | Point d'entrée contrôlé | Capacité requise |
|----------|----------------------|-----------------|
| Fichiers | `PermissionedStreamFactory` | Aucune |
| Réseau | `NetworkExecutor` | Aucune |
| Processus | `CommandLineExecutor` | Aucune |
| Stockage de données | `SpeedyPack` | Aucune |
| Service IA | `IAIService` | `Capability.AI` |

`IAIService` est unique : elle **requiert** une déclaration de capacité. L'accès au service IA est une fonctionnalité opt-in, pas une capacité par défaut disponible pour tous les plugins.

## Fichiers

- `Plugin.cs` — Plugin de démonstration déclarant Capability.AI
- `README.md` — Ce fichier (Anglais)
- `README.zh-CN.md` — Chinois simplifié
- Traductions : zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemples connexes

- **17-CapabilityStacked** : Empilage de capacités multiples (Réseau + IA)
- **18-CapabilityDenied** : Anti-pattern des capacités non déclarables
