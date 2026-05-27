# Demo pluginu pracovního postupu

Demonstruje současnou implementaci `IPlugin` a `IWorkflowPlugin` v jedné třídě. Ukazuje, jak registrovat vlastní šablony pracovních postupů a vkládat pravidla přechodů do existujících šablon.

## Rozhraní IWorkflowPlugin

`IWorkflowPlugin` je volitelné rozhraní, které mohou pluginy implementovat spolu s `IPlugin` pro přispívání definic pracovních postupů do `WorkflowEngine` hostitele.

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| Člen | Účel |
|------|-------|
| `PluginId` | Jedinečný identifikátor — musí odpovídat `IPlugin.Id` |
| `RegisterTemplates()` | Vrací nové šablony pracovních postupů pro registraci enginem |
| `InjectTransitions(template)` | Vkládá další pravidla přechodů do existující šablony |

## Jak implementovat obě rozhraní

Jedna třída implementuje jak `IPlugin`, tak `IWorkflowPlugin`：

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  Členové IPlugin:                                                   │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  Členové IWorkflowPlugin:                                           │
│    PluginId  →  vrací Id (musí odpovídat IPlugin.Id)              │
│    RegisterTemplates()  →  vytváří pracovní postup PluginApproval  │
│    InjectTransitions()  →  přidává FastTrack do CodeReview        │
└──────────────────────────────────────────────────────────────────────┘
```

### Klíčové body

1. **PluginId musí odpovídat IPlugin.Id** — hostitel koreluje plugin s jeho příspěvky k pracovním postupům pomocí tohoto identifikátoru.

2. **RegisterTemplates() je volána jednou při spuštění** — hostitel shromažďuje všechny šablony ze všech implementací `IWorkflowPlugin` a registruje je pomocí `WorkflowEngine.RegisterTemplate()`.

3. **InjectTransitions() je volána pro každou registrovanou šablonu** — hostitel iteruje přes všechny registrované šablony a předává každou každému `IWorkflowPlugin`, umožňuje rozšíření mezi pluginy.

4. **Pořadí volání je důležité** — nejprve je volána `RegisterTemplates()` pro všechny pluginy, poté `InjectTransitions()` pro každou šablonu. To zajišťuje, že vložené přechody mohou odkazovat na stavy definované šablonou.

## Demo pracovní postup: PluginApproval

Toto demo vytváří jednoduchý 4-stavový schvalovací pracovní postup：

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (konečný stav)
                             │
                             └──Reject──▶ Rejected (konečný stav)
```

| Stav | Popis | Typ |
|------|-------|-----|
| `Submitted` | Počáteční stav — plugin byl odeslán | Normální |
| `Reviewing` | Probíhá kontrola Screenerem | Normální |
| `Approved` | Plugin schválen | Konečný |
| `Rejected` | Plugin zamítnut | Konečný |

### Definice rolí

| Role | Min | Max | Popis |
|------|-----|-----|-------|
| Screener | 1 | 3 | Kontroluje odeslané pluginy |

### Přechody

| Název | Z → Do | Podmínka | Časový limit | Vyžadované role |
|-------|--------|----------|-------------|----------------|
| StartReview | Submitted → Reviewing | Automaticky (Screener přiřazen) | 2 dny | Screener |
| Approve | Reviewing → Approved | `ApprovedBy` v metadatech | 5 dní | Screener |
| Reject | Reviewing → Rejected | `RejectedBy` v metadatech | — | — |

## Demo InjectTransitions: FastTrack

Plugin vkládá přechod "FastTrack" do vestavěného pracovního postupu `CodeReview`：

```
Draft ──FastTrack──▶ Approved  (přeskočí stav Reviewing)
```

To ukazuje, jak plugin může rozšiřovat pracovní postupy definované jinými pluginy nebo hostitelem. Přechod `FastTrack` je aktivován pouze když metadata instance obsahují `TrustedAuthor = true`.

## Posloupnost volání

```
Spuštění hostitele
  │
  ├── Načíst DLL → PluginLoader skenuje a vytváří WorkflowPluginDemo
  │
  ├── OnLoad()
  │     └── Plugin připravuje definice pracovních postupů
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── Vrací šablonu PluginApproval
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(šablona CodeReview)
  │     └── Přidává přechod FastTrack
  │
  ├── InjectTransitions(šablona PluginApproval)
  │     └── (žádná operace — tento plugin nerozšiřuje vlastní šablonu)
  │
  └── OnStart()
        └── Plugin je plně provozuschopný
```

## Toto demo

| Třída | Role |
|-------|------|
| `WorkflowPluginDemo` | Implementuje jak `IPlugin`, tak `IWorkflowPlugin` — registruje pracovní postup PluginApproval a vkládá FastTrack do CodeReview |

## Viz také

- [Rozhraní IPlugin](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — základní rozhraní pluginu
- [Rozhraní IWorkflowPlugin](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — rozhraní pluginu pracovního postupu
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — definice šablony
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — pravidla přechodů stavů
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — příklad vestavěného pracovního postupu
