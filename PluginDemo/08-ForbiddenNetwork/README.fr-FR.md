# PluginDemo-08: Anti-Pattern des Opérations Réseau Interdites

## Vue d'ensemble

Ce plugin démontre les opérations réseau **INTERDITES** dans le système de plugins SiliconLife. Il sert de référence anti-pattern, montrant ce qu'il NE faut PAS faire et fournissant les alternatives correctes.

## Pourquoi l'accès réseau direct est-il interdit globalement ?

Les modèles d'accès réseau direct sont bloqués au niveau du plugin :

1. **Connexion à des serveurs malveillants** : Les plugins pourraient se connecter à des serveurs malveillants
2. **Exfiltration de données** : Les plugins pourraient fuir des données sensibles depuis le bac à sable
3. **Attaques DNS Rebinding** : Les plugins pourraient contourner les vérifications de sécurité
4. **Contournement ACL réseau** : L'accès réseau direct contourne le système ACL global

## Types interdits

Tous les types `System.Net` qui accèdent directement au réseau sont bloqués :

| Type interdit | Espace de noms bloqué | Niveau de risque |
|----------|----------------|----------|
| `HttpClient` | `System.Net.Http` | 🔴 Critique |
| `TcpClient` | `System.Net.Sockets` | 🔴 Critique |
| `Socket` | `System.Net.Sockets` | 🔴 Critique |
| `Dns` | `System.Net` | 🔴 Critique |
| `WebClient` | `System.Net` | 🔴 Critique |

## Méthodes d'accès sécurisé

### NetworkExecutor (Recommandé)

`NetworkExecutor` est le **point d'entrée contrôlé** pour les opérations réseau :

```csharp
// ✅ CORRECT : Requête GET simple
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

**Ce que NetworkExecutor fournit :**
1. Vérification des permissions
2. Journalisation d'audit
3. Disjoncteur
4. Contrôle du délai d'attente
5. File d'attente des requêtes

## Violations démontrées

### Violation 1 : HttpClient

```csharp
// ❌ INTERDIT
using var client = new HttpClient();

// ✅ CORRECT
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

### Violation 2 : TcpClient

```csharp
// ❌ INTERDIT
using var client = new TcpClient("example.com", 8080);

// ✅ CORRECT
// Utiliser NetworkExecutor ou déclarer Capability.Network
```

## Mécanisme de sécurité PluginLoader

PluginLoader analyse ce plugin et :
1. **Analyse TypeRef** : Détecte les références aux types interdits
2. **Analyse MemberRef** : Détecte les appels aux méthodes bloquées
3. **Analyse de chaîne IL** : Détecte les tentatives de réflexion
4. **Rejet** : Le plugin est rejeté au chargement

## Fichiers

- `Plugin.cs` - Plugin de démonstration anti-pattern
- `README.md` - Ce fichier (Anglais)
- `README.fr-FR.md` - Ce fichier (Français)
- Autres versions linguistiques...

## Exemples connexes

- **13-CapabilityNetwork** : Capacité réseau déclarative
- **07-ForbiddenFileIO** : Modèles d'accès fichiers interdits