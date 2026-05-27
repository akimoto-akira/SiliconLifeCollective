# Demo Plugin di Workflow

Dimostra l'implementazione simultanea di `IPlugin` e `IWorkflowPlugin` in un'unica classe. Mostra come registrare modelli di workflow personalizzati e iniettare regole di transizione in modelli esistenti.

## Interfaccia IWorkflowPlugin

`IWorkflowPlugin` è un'interfaccia opzionale che i plugin possono implementare insieme a `IPlugin` per contribuire definizioni di workflow al `WorkflowEngine` dell'host.

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| Membro | Scopo |
|--------|-------|
| `PluginId` | Identificatore univoco — deve corrispondere a `IPlugin.Id` |
| `RegisterTemplates()` | Restituisce nuovi modelli di workflow per la registrazione da parte del motore |
| `InjectTransitions(template)` | Inietta regole di transizione aggiuntive in un modello esistente |

## Come implementare entrambe le interfacce

Una singola classe implementa sia `IPlugin` che `IWorkflowPlugin`：

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  Membri IPlugin:                                                    │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  Membri IWorkflowPlugin:                                            │
│    PluginId  →  restituisce Id (deve corrispondere a IPlugin.Id)   │
│    RegisterTemplates()  →  crea il workflow PluginApproval         │
│    InjectTransitions()  →  aggiunge FastTrack al workflow CodeReview│
└──────────────────────────────────────────────────────────────────────┘
```

### Punti chiave

1. **PluginId deve corrispondere a IPlugin.Id** — l'host correlaziona il plugin con i suoi contributi di workflow usando questo identificatore.

2. **RegisterTemplates() viene chiamato una volta all'avvio** — l'host raccoglie tutti i modelli da tutte le implementazioni `IWorkflowPlugin` e li registra con `WorkflowEngine.RegisterTemplate()`.

3. **InjectTransitions() viene chiamato per ogni modello registrato** — l'host itera attraverso tutti i modelli registrati e passa ciascuno a ogni `IWorkflowPlugin`, consentendo estensioni tra plugin.

4. **L'ordine di chiamata è importante** — prima viene chiamato `RegisterTemplates()` per tutti i plugin, poi `InjectTransitions()` per ogni modello. Questo garantisce che le transizioni iniettate possano fare riferimento agli stati definiti dal modello.

## Workflow demo: PluginApproval

Questa demo crea un semplice workflow di approvazione a 4 stati：

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (stato terminale)
                             │
                             └──Reject──▶ Rejected (stato terminale)
```

| Stato | Descrizione | Tipo |
|-------|-------------|------|
| `Submitted` | Stato iniziale — il plugin è stato inviato | Normale |
| `Reviewing` | In revisione da uno Screener | Normale |
| `Approved` | Plugin approvato | Terminale |
| `Rejected` | Plugin rifiutato | Terminale |

### Definizioni dei ruoli

| Ruolo | Min | Max | Descrizione |
|-------|-----|-----|-------------|
| Screener | 1 | 3 | Revisiona le invii di plugin |

### Transizioni

| Nome | Da → A | Condizione | Timeout | Ruoli richiesti |
|------|--------|-----------|---------|----------------|
| StartReview | Submitted → Reviewing | Automatico (Screener assegnato) | 2 giorni | Screener |
| Approve | Reviewing → Approved | `ApprovedBy` nei metadati | 5 giorni | Screener |
| Reject | Reviewing → Rejected | `RejectedBy` nei metadati | — | — |

## Demo InjectTransitions: FastTrack

Il plugin inietta una transizione "FastTrack" nel workflow `CodeReview` integrato：

```
Draft ──FastTrack──▶ Approved  (salta lo stato Reviewing)
```

Questo dimostra come un plugin può estendere workflow definiti da altri plugin o dall'host. La transizione `FastTrack` è abilitata solo quando i metadati dell'istanza contengono `TrustedAuthor = true`.

## Sequenza di chiamate

```
Avvio dell'host
  │
  ├── Carica DLL → PluginLoader scansiona e crea WorkflowPluginDemo
  │
  ├── OnLoad()
  │     └── Il plugin prepara le definizioni di workflow
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── Restituisce il modello PluginApproval
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(modello CodeReview)
  │     └── Aggiunge la transizione FastTrack
  │
  ├── InjectTransitions(modello PluginApproval)
  │     └── (nessuna operazione — questo plugin non estende il proprio modello)
  │
  └── OnStart()
        └── Il plugin è completamente operativo
```

## Questa demo

| Classe | Ruolo |
|--------|-------|
| `WorkflowPluginDemo` | Implementa sia `IPlugin` che `IWorkflowPlugin` — registra il workflow PluginApproval e inietta FastTrack in CodeReview |

## Vedi anche

- [Interfaccia IPlugin](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — interfaccia plugin base
- [Interfaccia IWorkflowPlugin](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — interfaccia plugin workflow
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — definizione modello
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — regole di transizione di stato
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — esempio di workflow integrato
