# Workflow-Plugin-Demo

Demonstriert die gleichzeitige Implementierung von `IPlugin` und `IWorkflowPlugin` in einer einzigen Klasse. Zeigt, wie benutzerdefinierte Workflow-Vorlagen registriert und Übergangsregeln in bestehende Vorlagen injiziert werden.

## IWorkflowPlugin-Schnittstelle

`IWorkflowPlugin` ist eine optionale Schnittstelle, die Plugins zusammen mit `IPlugin` implementieren können, um Workflow-Definitionen zum `WorkflowEngine` des Hosts beizutragen.

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| Mitglied | Zweck |
|-----------|-------|
| `PluginId` | Eindeutige Kennung — muss mit `IPlugin.Id` übereinstimmen |
| `RegisterTemplates()` | Gibt neue Workflow-Vorlagen zur Registrierung durch die Engine zurück |
| `InjectTransitions(template)` | Injiziert zusätzliche Übergangsregeln in eine bestehende Vorlage |

## Implementierung beider Schnittstellen

Eine einzige Klasse implementiert sowohl `IPlugin` als auch `IWorkflowPlugin`：

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  IPlugin-Mitglieder:                                                │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  IWorkflowPlugin-Mitglieder:                                        │
│    PluginId  →  gibt Id zurück (muss mit IPlugin.Id übereinstimmen) │
│    RegisterTemplates()  →  erstellt PluginApproval-Workflow         │
│    InjectTransitions()  →  fügt FastTrack zu CodeReview hinzu      │
└──────────────────────────────────────────────────────────────────────┘
```

### Wichtige Punkte

1. **PluginId muss mit IPlugin.Id übereinstimmen** — der Host verknüpft das Plugin mit seinen Workflow-Beiträgen über diese Kennung.

2. **RegisterTemplates() wird beim Start einmal aufgerufen** — der Host sammelt alle Vorlagen von allen `IWorkflowPlugin`-Implementierungen und registriert sie mit `WorkflowEngine.RegisterTemplate()`.

3. **InjectTransitions() wird für jede registrierte Vorlage aufgerufen** — der Host iteriert durch alle registrierten Vorlagen und übergibt jede an jedes `IWorkflowPlugin`, was plattformübergreifende Erweiterungen ermöglicht.

4. **Die Aufrufreihenfolge ist wichtig** — zuerst wird `RegisterTemplates()` für alle Plugins aufgerufen, dann `InjectTransitions()` für jede Vorlage. Dies stellt sicher, dass injizierte Übergänge auf von der Vorlage definierte Zustände verweisen können.

## Demo-Workflow: PluginApproval

Dieses Demo erstellt einen einfachen 4-Zustands-Genehmigungsworkflow：

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (Endzustand)
                             │
                             └──Reject──▶ Rejected (Endzustand)
```

| Zustand | Beschreibung | Typ |
|---------|-------------|-----|
| `Submitted` | Initialzustand — Plugin wurde eingereicht | Normal |
| `Reviewing` | Wird von einem Screener geprüft | Normal |
| `Approved` | Plugin genehmigt | Terminal |
| `Rejected` | Plugin abgelehnt | Terminal |

### Rollendefinitionen

| Rolle | Min | Max | Beschreibung |
|-------|-----|-----|-------------|
| Screener | 1 | 3 | Prüft Plugin-Einreichungen |

### Übergänge

| Name | Von → Nach | Bedingung | Timeout | Erforderliche Rollen |
|------|-----------|-----------|---------|---------------------|
| StartReview | Submitted → Reviewing | Automatisch (Screener besetzt) | 2 Tage | Screener |
| Approve | Reviewing → Approved | `ApprovedBy` in Metadaten | 5 Tage | Screener |
| Reject | Reviewing → Rejected | `RejectedBy` in Metadaten | — | — |

## InjectTransitions-Demo: FastTrack

Das Plugin injiziert einen "FastTrack"-Übergang in den integrierten `CodeReview`-Workflow：

```
Draft ──FastTrack──▶ Approved  (überspringt Reviewing-Zustand)
```

Dies zeigt, wie ein Plugin Workflows erweitern kann, die von anderen Plugins oder vom Host definiert wurden. Der `FastTrack`-Übergang ist nur aktiv, wenn die Workflow-Instanz-Metadaten `TrustedAuthor = true` enthalten.

## Aufrufreihenfolge

```
Host-Start
  │
  ├── DLL laden → PluginLoader scannt und erstellt WorkflowPluginDemo
  │
  ├── OnLoad()
  │     └── Plugin bereitet Workflow-Definitionen vor
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── Gibt PluginApproval-Vorlage zurück
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(CodeReview-Vorlage)
  │     └── Fügt FastTrack-Übergang hinzu
  │
  ├── InjectTransitions(PluginApproval-Vorlage)
  │     └── (Keine Operation — dieses Plugin erweitert nicht seine eigene Vorlage)
  │
  └── OnStart()
        └── Plugin ist vollständig betriebsbereit
```

## Dieses Demo

| Klasse | Rolle |
|--------|-------|
| `WorkflowPluginDemo` | Implementiert sowohl `IPlugin` als auch `IWorkflowPlugin` — registriert PluginApproval-Workflow und injiziert FastTrack in CodeReview |

## Siehe auch

- [IPlugin-Schnittstelle](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — Basis-Plugin-Schnittstelle
- [IWorkflowPlugin-Schnittstelle](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — Workflow-Plugin-Schnittstelle
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — Vorlagendefinition
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — Zustandsübergangsregeln
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — Integriertes Workflow-Beispiel
