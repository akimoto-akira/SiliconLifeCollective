# PluginDemo-13 : Capability.Network — Permission réseau déclarative

## Aperçu

Ce plugin illustre l'utilisation de `[PluginCapability(Capability.Network)]` pour déclarer l'accès réseau. Cette déclaration permet d'accéder aux types `System.Net.*` qui seraient autrement bloqués par l'analyse de sécurité de PluginLoader.

## Syntaxe de déclaration

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**Éléments clés :**
- **Cible de l'attribut** : Doit être placé sur la classe qui implémente directement `IPlugin`
- **AllowMultiple = true** : Plusieurs attributs `[PluginCapability]` peuvent être empilés (voir 17-CapabilityStacked)
- **Champ Reason** : Explication lisible par l'humain écrite dans le journal d'audit de sécurité au chargement. **Fournir un Reason clair est fortement recommandé pour tous les plugins en production.**

## Comment PluginLoader traite les déclarations de capacité

1. **Lecture des métadonnées PE** : PluginLoader lit les déclarations de capacité depuis la table CustomAttribute du fichier PE **avant** le début de l'analyse de sécurité
2. **Assouplissement des règles de scan** : Les capacités déclarées exemptent les références de type correspondantes des vérifications d'espaces de noms et de types interdits
3. **Journalisation d'audit** : Toutes les déclarations (y compris Reason) sont écrites dans le journal d'audit de sécurité
4. **Capacités non déclarables** : P/Invoke, Unsafe, Reflection.Emit, etc. restent bloqués quelle que soit la déclaration

## Portée d'exemption de Capability.Network

### Exemptions TypeRef

Lorsque `Capability.Network` est déclarée, les règles d'interdiction basées sur les espaces de noms et les types suivantes sont assouplies :

| Namespace exempté | Types autorisés |
|------------------|----------------|
| `System.Net.Http` | `HttpClient`, `HttpRequestMessage`, `HttpResponseMessage`, etc. |
| `System.Net.WebSockets` | `ClientWebSocket`, `WebSocket`, etc. |
| `System.Net.Sockets` | `TcpClient`, `UdpClient`, `Socket`, etc. |
| `System.Net.Mail` | `SmtpClient`, `MailMessage`, etc. |
| `System.Net.NetworkInformation` | `Ping`, `NetworkInterface`, etc. |
| `System.Net.Security` | `SslStream`, etc. |
| `System.Net` (interdictions par type) | `HttpWebRequest`, `WebClient`, `Dns`, `FtpWebRequest`, etc. |

### Exemptions ILString

Les constantes de chaîne commençant par ces préfixes ne sont pas signalées lors de l'analyse du tas #US :
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### Ce qui reste interdit

Même avec `Capability.Network`, ces capacités sont **toujours** bloquées (capacités non déclarables) :

| Catégorie | Types bloqués | Pourquoi non déclarable |
|----------|-------------|----------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` | Ne peut pas être audité de manière sécurisée à l'exécution |
| Code unsafe | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Contourne les garanties de sécurité de type |
| Émission IL | `System.Reflection.Emit.*` | Peut générer du code arbitraire à l'exécution |
| Chargement d'assembly | `System.Runtime.Loader`, `Assembly.Load*` | Peut contourner l'analyse de sécurité en chargeant des DLL non vérifiées |
| Registre | `Microsoft.Win32.*` | Accès système au niveau OS en dehors du bac à sable du plugin |

## Champ Reason — Rôle d'audit

Le champ `Reason` sert de **piste d'audit** pour les déclarations de capacité :

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Pourquoi Reason est important :**
1. **Revue de sécurité** : Les auditeurs peuvent vérifier que les capacités déclarées correspondent au comportement réel du plugin
2. **Principe du moindre privilège** : Oblige les auteurs de plugins à justifier chaque capacité nécessaire
3. **Conformité** : Requis pour les certifications de sécurité et les enquêtes sur les incidents
4. **Surveillance à l'exécution** : Les outils de sécurité peuvent alerter si l'utilisation de la capacité déclarée dépasse la raison indiquée

## Comparaison avec 08-ForbiddenNetwork

| Aspect | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|--------|-------------------|---------------------|
| Déclaration | Aucune | `[PluginCapability(Capability.Network)]` |
| Résultat du chargement | ❌ Rejeté | ✅ Chargé avec succès |
| Utilisation HttpClient | Bloqué par l'analyse TypeRef | Exempté par la capacité |
| Utilisation TcpClient | Bloqué par l'analyse TypeRef | Exempté par la capacité |
| Reason | Non applicable | Écrit dans le journal d'audit |

**Différence clé** : 08-ForbiddenNetwork montre ce qui se passe lorsqu'on utilise des types réseau **sans** déclarer la capacité. 13-CapabilityNetwork montre la manière **correcte** de demander déclarativement l'accès réseau.

## Bonnes pratiques de sécurité

1. **Ne déclarer que le nécessaire** : Si vous n'avez besoin que de HTTP, ne déclarez pas Capability.Network juste parce que c'est possible — mais notez que Capability.Network est la seule capacité liée au réseau ; il n'y a pas d'option plus fine
2. **Préférer NetworkExecutor** : `NetworkExecutor` est le point d'entrée contrôlé pour l'accès réseau et ne nécessite aucune déclaration de capacité
3. **Fournir un Reason clair** : Les raisons vagues comme « accès réseau » sont un signal d'alarme lors des revues de sécurité
4. **Respecter les limites non déclarables** : Aucune déclaration de capacité ne peut contourner les interdictions P/Invoke, Unsafe ou Reflection.Emit

## Fichiers

- `Plugin.cs` — Plugin de démonstration déclarant Capability.Network
- `README.md` — English
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 日本語
- `README.ko-KR.md` — 한국어
- `README.de-DE.md` — Deutsch
- `README.fr-FR.md` — Ce fichier (Français)
- `README.es-ES.md` — Español
- `README.it-IT.md` — Italiano
- `README.ru-RU.md` — Русский
- `README.pt-PT.md` — Português
- `README.pl-PL.md` — Polski
- `README.cs-CZ.md` — Čeština

## Exemples connexes

- **08-ForbiddenNetwork** : Anti-pattern montrant des opérations réseau bloquées
- **14-CapabilityFileIO** : Capacité FileIO déclarative
- **15-CapabilityProcess** : Capacité Process déclarative
- **16-CapabilityAI** : Capacité de service IA déclarative
- **17-CapabilityStacked** : Empilement de capacités multiples
- **18-CapabilityDenied** : Anti-pattern de capacité non déclarable
