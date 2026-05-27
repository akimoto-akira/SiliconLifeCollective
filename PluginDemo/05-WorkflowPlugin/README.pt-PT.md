# Demo de Plugin de Fluxo de Trabalho

Demonstra a implementação simultânea de `IPlugin` e `IWorkflowPlugin` numa única classe. Mostra como registar modelos de fluxo de trabalho personalizados e injetar regras de transição em modelos existentes.

## Interface IWorkflowPlugin

`IWorkflowPlugin` é uma interface opcional que os plugins podem implementar junto com `IPlugin` para contribuir definições de fluxo de trabalho ao `WorkflowEngine` do host.

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| Membro | Propósito |
|--------|-----------|
| `PluginId` | Identificador único — deve corresponder a `IPlugin.Id` |
| `RegisterTemplates()` | Retorna novos modelos de fluxo de trabalho para registo pelo motor |
| `InjectTransitions(template)` | Injeta regras de transição adicionais num modelo existente |

## Como implementar ambas as interfaces

Uma única classe implementa tanto `IPlugin` como `IWorkflowPlugin`：

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  Membros de IPlugin:                                                │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  Membros de IWorkflowPlugin:                                        │
│    PluginId  →  retorna Id (deve corresponder a IPlugin.Id)        │
│    RegisterTemplates()  →  cria o fluxo PluginApproval             │
│    InjectTransitions()  →  adiciona FastTrack ao fluxo CodeReview  │
└──────────────────────────────────────────────────────────────────────┘
```

### Pontos-chave

1. **PluginId deve corresponder a IPlugin.Id** — o host correlaciona o plugin com as suas contribuições de fluxo de trabalho usando este identificador.

2. **RegisterTemplates() é chamado uma vez durante o arranque** — o host recolhe todos os modelos de todas as implementações `IWorkflowPlugin` e regista-os com `WorkflowEngine.RegisterTemplate()`.

3. **InjectTransitions() é chamado para cada modelo registado** — o host itera por todos os modelos registados e passa cada um a cada `IWorkflowPlugin`, permitindo extensões entre plugins.

4. **A ordem de chamada é importante** — primeiro é chamado `RegisterTemplates()` para todos os plugins, depois `InjectTransitions()` para cada modelo. Isto garante que as transições injetadas podem referenciar estados definidos pelo modelo.

## Fluxo de trabalho de demonstração: PluginApproval

Esta demo cria um fluxo de aprovação simples de 4 estados：

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (estado terminal)
                             │
                             └──Reject──▶ Rejected (estado terminal)
```

| Estado | Descrição | Tipo |
|--------|-----------|------|
| `Submitted` | Estado inicial — o plugin foi submetido | Normal |
| `Reviewing` | Em revisão por um Screener | Normal |
| `Approved` | Plugin aprovado | Terminal |
| `Rejected` | Plugin rejeitado | Terminal |

### Definições de funções

| Função | Mín | Máx | Descrição |
|--------|-----|-----|-----------|
| Screener | 1 | 3 | Revê as submissões de plugins |

### Transições

| Nome | De → Para | Condição | Tempo limite | Funções requeridas |
|------|-----------|----------|-------------|-------------------|
| StartReview | Submitted → Reviewing | Automático (Screener atribuído) | 2 dias | Screener |
| Approve | Reviewing → Approved | `ApprovedBy` nos metadados | 5 dias | Screener |
| Reject | Reviewing → Rejected | `RejectedBy` nos metadados | — | — |

## Demo InjectTransitions: FastTrack

O plugin injeta uma transição "FastTrack" no fluxo de trabalho `CodeReview` integrado：

```
Draft ──FastTrack──▶ Approved  (ignora o estado Reviewing)
```

Isto demonstra como um plugin pode estender fluxos de trabalho definidos por outros plugins ou pelo host. A transição `FastTrack` só é ativada quando os metadados da instância contêm `TrustedAuthor = true`.

## Sequência de chamadas

```
Arranque do host
  │
  ├── Carregar DLL → PluginLoader analisa e cria WorkflowPluginDemo
  │
  ├── OnLoad()
  │     └── O plugin prepara as definições de fluxo de trabalho
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── Retorna o modelo PluginApproval
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(modelo CodeReview)
  │     └── Adiciona a transição FastTrack
  │
  ├── InjectTransitions(modelo PluginApproval)
  │     └── (sem operação — este plugin não estende o seu próprio modelo)
  │
  └── OnStart()
        └── O plugin está totalmente operacional
```

## Esta demo

| Classe | Função |
|--------|--------|
| `WorkflowPluginDemo` | Implementa tanto `IPlugin` como `IWorkflowPlugin` — regista o fluxo PluginApproval e injeta FastTrack no CodeReview |

## Ver também

- [Interface IPlugin](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — interface base de plugin
- [Interface IWorkflowPlugin](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — interface de plugin de fluxo de trabalho
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — definição de modelo
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — regras de transição de estados
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — exemplo de fluxo de trabalho integrado
