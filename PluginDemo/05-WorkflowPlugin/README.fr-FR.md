# Démo de Plugin de Workflow

Démontre l'implémentation simultanée de `IPlugin` et `IWorkflowPlugin` dans une seule classe. Montre comment enregistrer des modèles de workflow personnalisés et injecter des règles de transition dans des modèles existants.

## Interface IWorkflowPlugin

`IWorkflowPlugin` est une interface optionnelle que les plugins peuvent implémenter alongside `IPlugin` pour contribuer des définitions de workflow au `WorkflowEngine` de l'hôte.

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| Membre | Objectif |
|--------|----------|
| `PluginId` | Identifiant unique — doit correspondre à `IPlugin.Id` |
| `RegisterTemplates()` | Retourne de nouveaux modèles de workflow pour l'enregistrement par le moteur |
| `InjectTransitions(template)` | Injecte des règles de transition supplémentaires dans un modèle existant |

## Comment implémenter les deux interfaces

Une seule classe implémente à la fois `IPlugin` et `IWorkflowPlugin` :

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  Membres IPlugin :                                                  │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  Membres IWorkflowPlugin :                                          │
│    PluginId  →  retourne Id (doit correspondre à IPlugin.Id)       │
│    RegisterTemplates()  →  crée le workflow PluginApproval         │
│    InjectTransitions()  →  ajoute FastTrack au workflow CodeReview │
└──────────────────────────────────────────────────────────────────────┘
```

### Points clés

1. **PluginId doit correspondre à IPlugin.Id** — l'hôte utilise cet identifiant pour corréler le plugin avec ses contributions de workflow.

2. **RegisterTemplates() est appelé une fois au démarrage** — l'hôte collecte tous les modèles de toutes les implémentations `IWorkflowPlugin` et les enregistre via `WorkflowEngine.RegisterTemplate()`.

3. **InjectTransitions() est appelé pour chaque modèle enregistré** — l'hôte itère à travers tous les modèles enregistrés et passe chacun à chaque `IWorkflowPlugin`, permettant les extensions inter-plugins.

4. **L'ordre d'appel est important** — `RegisterTemplates()` est d'abord appelé pour tous les plugins, puis `InjectTransitions()` pour chaque modèle. Cela garantit que les transitions injectées peuvent référencer les états définis par le modèle.

## Workflow de démo : PluginApproval

Cette démo crée un workflow d'approbation simple à 4 états :

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (état terminal)
                             │
                             └──Reject──▶ Rejected (état terminal)
```

| État | Description | Type |
|------|-------------|------|
| `Submitted` | État initial — le plugin a été soumis | Normal |
| `Reviewing` | En cours de révision par un Screener | Normal |
| `Approved` | Plugin approuvé | Terminal |
| `Rejected` | Plugin rejeté | Terminal |

### Définitions de rôles

| Rôle | Min | Max | Description |
|------|-----|-----|-------------|
| Screener | 1 | 3 | Révise les soumissions de plugins |

### Transitions

| Nom | De → Vers | Condition | Délai | Rôles requis |
|-----|-----------|-----------|-------|-------------|
| StartReview | Submitted → Reviewing | Automatique (Screener affecté) | 2 jours | Screener |
| Approve | Reviewing → Approved | `ApprovedBy` dans les métadonnées | 5 jours | Screener |
| Reject | Reviewing → Rejected | `RejectedBy` dans les métadonnées | — | — |

## Démo InjectTransitions : FastTrack

Le plugin injecte une transition "FastTrack" dans le workflow `CodeReview` intégré :

```
Draft ──FastTrack──▶ Approved  (ignore l'état Reviewing)
```

Cela montre comment un plugin peut étendre des workflows définis par d'autres plugins ou par l'hôte. La transition `FastTrack` n'est activée que lorsque les métadonnées de l'instance de workflow contiennent `TrustedAuthor = true`.

## Séquence d'appels

```
Démarrage de l'hôte
  │
  ├── Charger la DLL → PluginLoader scanne et crée WorkflowPluginDemo
  │
  ├── OnLoad()
  │     └── Le plugin prépare les définitions de workflow
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── Retourne le modèle PluginApproval
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(modèle CodeReview)
  │     └── Ajoute la transition FastTrack
  │
  ├── InjectTransitions(modèle PluginApproval)
  │     └── (aucune opération — ce plugin n'étend pas son propre modèle)
  │
  └── OnStart()
        └── Le plugin est pleinement opérationnel
```

## Cette démo

| Classe | Rôle |
|--------|------|
| `WorkflowPluginDemo` | Implémente à la fois `IPlugin` et `IWorkflowPlugin` — enregistre le workflow PluginApproval et injecte FastTrack dans CodeReview |

## Voir aussi

- [Interface IPlugin](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — interface de plugin de base
- [Interface IWorkflowPlugin](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — interface de plugin de workflow
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — définition de modèle
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — règles de transition d'état
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — exemple de workflow intégré
