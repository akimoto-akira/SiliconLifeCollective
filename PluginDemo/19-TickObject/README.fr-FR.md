# PluginDemo-19 : TickObject — Tâche périodique dans MainLoop

## Aperçu

Ce plugin illustre l'utilisation de `TickObject` pour s'intégrer à `MainLoop` afin d'implémenter une logique périodique/continue. TickObject est la classe de base pour les objets pouvant être tickés par la boucle principale de MainLoop, offrant une alternative unifiée à `System.Threading.Timer` ou `Task.Delay`.

## Cycle de vie de TickObject

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → MainLoop.Register(this) appelé automatiquement dans le constructeur
    │
    ├── autoRegister=false → appeler MainLoop.Register(this) manuellement plus tard
    │
    ▼
MainLoop.Tick() boucle
    │
    ├── Trier tous les TickObjects enregistrés par Priority (croissant)
    ├── Accumuler elapsedTime pour chaque TickObject
    ├── Si elapsedTime >= Interval → appeler OnTick(deltaTime)
    │
    ├── Disjoncteur : si OnTick dépasse TickTimeout → incrémenter le compteur de timeout
    │   └── Après maxTimeoutCount timeouts consécutifs → période de refroidissement de 1 minute
    │
    ▼
MainLoop.Unregister(tickObject) — nettoyage dans OnStop
```

## Propriétés clés

| Propriété | Type | Par défaut | Description |
|-----------|------|-----------|-------------|
| `Interval` | `TimeSpan` | Requis | Fréquence d'appel de OnTick |
| `Priority` | `int` | 100 | Ordre d'exécution (plus bas = priorité plus élevée) |
| `autoRegister` | `bool` | `true` | Auto-enregistrement à MainLoop dans le constructeur |

## Méthodes clés

| Méthode | Description |
|---------|-------------|
| `OnTick(TimeSpan deltaTime)` | Surcharger pour implémenter la logique périodique |
| `MainLoop.Register(TickObject)` | Enregistrer manuellement à MainLoop |
| `MainLoop.Unregister(TickObject)` | Retirer de MainLoop (nettoyage) |

## Scénarios de démonstration

### 1. Minuterie basique (autoRegister=true)
```csharp
public class StatusTimer : TickObject
{
    public StatusTimer() : base(interval: TimeSpan.FromSeconds(5), autoRegister: true)
    {
        Priority = 100;
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        Console.WriteLine($"Tick, deltaTime={deltaTime.TotalMilliseconds:F0}ms");
    }
}
```

### 2. Enregistrement manuel (autoRegister=false)
```csharp
// Dans le constructeur : ne pas auto-enregistrer
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// Dans OnStart : enregistrer manuellement
MainLoop.Register(_heartbeatTimer);
```

### 3. Ordre de priorité
- `Priority = 10` → Haute priorité, exécuté en premier
- `Priority = 200` → Basse priorité, exécuté après

### 4. Nettoyage
```csharp
// Dans OnStop : toujours désenregistrer pour éviter les fuites
MainLoop.Unregister(_statusTimer);
```

## Disjoncteur de MainLoop

MainLoop possède un disjoncteur intégré pour empêcher les TickObjects lents de bloquer toute la boucle :

1. Si `OnTick` dépasse `TickTimeout` (1 seconde par défaut) → le compteur de timeout augmente
2. Après `maxTimeoutCount` (3 par défaut) timeouts consécutifs → le disjoncteur se déclenche
3. Le TickObject déclenché est **ignoré** pendant 1 minute de refroidissement
4. Après refroidissement, le TickObject reçoit une autre chance

## TickObject vs System.Threading.Timer

| Aspect | TickObject + MainLoop | System.Threading.Timer |
|--------|----------------------|----------------------|
| Modèle de thread | Thread de boucle principale unique | Threads du pool de threads |
| Ordre d'exécution | Déterministe (par Priority) | Non déterministe |
| Disjoncteur | Intégré | Aucun |
| Débogage | Facile (thread unique) | Difficile (conditions de concurrence) |
| Utilisation des ressources | Minimale (pas de pool de threads) | Surcharge du pool de threads |
| Précision de l'intervalle | Best-effort (affecté par d'autres TickObjects) | Plus précis |

## Note de sécurité

TickObject lui-même ne nécessite **aucune** déclaration de capacité. C'est un mécanisme de framework intégré sûr.

## Fichiers

- `Plugin.cs` — Plugin de démonstration TickObject
- `README.md` — Ce fichier (Anglais)
- `README.zh-CN.md` — Chinois simplifié
- Traductions : zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemples connexes

- **13-CapabilityNetwork** : Déclaration Capability.Network
- **20-SpeedyPack** : Stockage de données sans Capability.FileIO
