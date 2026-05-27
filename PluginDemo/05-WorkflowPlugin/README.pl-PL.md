# Demo wtyczki przepływu pracy

Demonstruje jednoczesną implementację `IPlugin` i `IWorkflowPlugin` w jednej klasie. Pokazuje, jak rejestrować niestandardowe szablony przepływu pracy i wstrzykiwać reguły przejść do istniejących szablonów.

## Interfejs IWorkflowPlugin

`IWorkflowPlugin` to opcjonalny interfejs, który wtyczki mogą implementować razem z `IPlugin`, aby dostarczać definicje przepływów pracy do `WorkflowEngine` hosta.

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| Członek | Cel |
|---------|-----|
| `PluginId` | Unikalny identyfikator — musi być zgodny z `IPlugin.Id` |
| `RegisterTemplates()` | Zwraca nowe szablony przepływu pracy do rejestracji przez silnik |
| `InjectTransitions(template)` | Wstrzykuje dodatkowe reguły przejść do istniejącego szablonu |

## Jak zaimplementować oba interfejsy

Jedna klasa implementuje zarówno `IPlugin`, jak i `IWorkflowPlugin`：

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  Członkowie IPlugin:                                                │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  Członkowie IWorkflowPlugin:                                        │
│    PluginId  →  zwraca Id (musi być zgodny z IPlugin.Id)           │
│    RegisterTemplates()  →  tworzy przepływ PluginApproval          │
│    InjectTransitions()  →  dodaje FastTrack do CodeReview          │
└──────────────────────────────────────────────────────────────────────┘
```

### Kluczowe punkty

1. **PluginId musi być zgodny z IPlugin.Id** — host używa tego identyfikatora do korelacji wtyczki z jej wkładem w przepływy pracy.

2. **RegisterTemplates() jest wywoływana raz podczas uruchamiania** — host zbiera wszystkie szablony ze wszystkich implementacji `IWorkflowPlugin` i rejestruje je za pomocą `WorkflowEngine.RegisterTemplate()`.

3. **InjectTransitions() jest wywoływana dla każdego zarejestrowanego szablonu** — host iteruje przez wszystkie zarejestrowane szablony i przekazuje każdy do każdego `IWorkflowPlugin`, umożliwiając rozszerzenia między wtyczkami.

4. **Kolejność wywołań jest ważna** — najpierw wywoływana jest `RegisterTemplates()` dla wszystkich wtyczek, a następnie `InjectTransitions()` dla każdego szablonu. Gwarantuje to, że wstrzyknięte przejścia mogą odwoływać się do stanów zdefiniowanych przez szablon.

## Demo przepływu pracy: PluginApproval

To demo tworzy prosty 4-stanowy przepływ zatwierdzania：

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (stan końcowy)
                             │
                             └──Reject──▶ Rejected (stan końcowy)
```

| Stan | Opis | Typ |
|------|------|-----|
| `Submitted` | Stan początkowy — wtyczka została przesłana | Zwykły |
| `Reviewing` | W trakcie przeglądu przez Screenera | Zwykły |
| `Approved` | Wtyczka zatwierdzona | Końcowy |
| `Rejected` | Wtyczka odrzucona | Końcowy |

### Definicje ról

| Rola | Min | Max | Opis |
|------|-----|-----|------|
| Screener | 1 | 3 | Przegląda przesłane wtyczki |

### Przejścia

| Nazwa | Z → Do | Warunek | Limit czasu | Wymagane role |
|-------|--------|---------|-------------|--------------|
| StartReview | Submitted → Reviewing | Automatyczne (Screener przypisany) | 2 dni | Screener |
| Approve | Reviewing → Approved | `ApprovedBy` w metadanych | 5 dni | Screener |
| Reject | Reviewing → Rejected | `RejectedBy` w metadanych | — | — |

## Demo InjectTransitions: FastTrack

Wtyczka wstrzykuje przejście "FastTrack" do wbudowanego przepływu pracy `CodeReview`：

```
Draft ──FastTrack──▶ Approved  (pomija stan Reviewing)
```

To pokazuje, jak wtyczka może rozszerzać przepływy pracy zdefiniowane przez inne wtyczki lub hosta. Przejście `FastTrack` jest aktywne tylko wtedy, gdy metadane instancji zawierają `TrustedAuthor = true`.

## Sekwencja wywołań

```
Uruchomienie hosta
  │
  ├── Załaduj DLL → PluginLoader skanuje i tworzy WorkflowPluginDemo
  │
  ├── OnLoad()
  │     └── Wtyczka przygotowuje definicje przepływów pracy
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── Zwraca szablon PluginApproval
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(szablon CodeReview)
  │     └── Dodaje przejście FastTrack
  │
  ├── InjectTransitions(szablon PluginApproval)
  │     └── (brak operacji — ta wtyczka nie rozszerza własnego szablonu)
  │
  └── OnStart()
        └── Wtyczka jest w pełni operacyjna
```

## To demo

| Klasa | Rola |
|-------|------|
| `WorkflowPluginDemo` | Implementuje zarówno `IPlugin`, jak i `IWorkflowPlugin` — rejestruje przepływ PluginApproval i wstrzykuje FastTrack do CodeReview |

## Zobacz także

- [Interfejs IPlugin](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — podstawowy interfejs wtyczki
- [Interfejs IWorkflowPlugin](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — interfejs wtyczki przepływu pracy
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — definicja szablonu
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — reguły przejść stanów
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — przykład wbudowanego przepływu pracy
