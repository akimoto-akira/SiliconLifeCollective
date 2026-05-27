# PluginDemo-17 : Empilement de capacités — Permissions déclaratives multiples

## Aperçu

Ce plugin illustre l'empilement de plusieurs attributs `[PluginCapability]` sur une seule classe de plugin. `PluginCapabilityAttribute` a `AllowMultiple = true`, vous pouvez donc déclarer autant de capacités que nécessaire.

## Syntaxe d'empilage

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## Comment PluginLoader traite les capacités empilées

1. **Lit toutes les déclarations** depuis la table CustomAttribute des métadonnées PE
2. **Fusionne** les règles d'exemption de toutes les capacités déclarées
3. **Journalise indépendamment** chaque déclaration avec son propre champ Reason
4. **Applique toujours** les interdictions de capacités non déclarables quel que soit l'empilage

## Règles d'exemption fusionnées

Lors de l'empilage `Capability.Network` + `Capability.AI` :

| Source | Exemption |
|--------|----------|
| Capability.Network | System.Net.Http.*, System.Net.WebSockets.*, System.Net.Sockets.*, System.Net.Mail.*, System.Net.NetworkInformation.*, System.Net.Security.*, System.Net (interdictions par type) |
| Capability.AI | Injection IAIService activée |
| **Combiné** | Le plugin peut utiliser HttpClient ET IAIService |

## L'empilage n'accorde pas un pouvoir illimité

Même avec plusieurs capacités empilées, celles-ci restent **toujours bloquées** :

- ❌ P/Invoke (`DllImport`, `Marshal`, `NativeMemory`)
- ❌ Code unsafe (`UnverifiableCodeAttribute`, `Unsafe`)
- ❌ Émission IL (`System.Reflection.Emit.*`)
- ❌ Chargement d'assembly (`System.Runtime.Loader`, `Assembly.Load*`)
- ❌ Registre (`Microsoft.Win32.*`)

Aucune valeur d'énumération `Capability` n'existe pour celles-ci — elles sont **non déclarables par conception**.

## Piste d'audit pour les capacités empilées

Chaque capacité est journalisée indépendamment :

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## Fichiers

- `Plugin.cs` — Plugin de démonstration avec empilage Capability.Network + Capability.AI
- `README.md` — Ce fichier (Anglais)
- `README.zh-CN.md` — Chinois simplifié
- Traductions : zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemples connexes

- **13-CapabilityNetwork** : Capacité Network unique
- **16-CapabilityAI** : Capacité IA unique
- **18-CapabilityDenied** : Anti-pattern des capacités non déclarables
