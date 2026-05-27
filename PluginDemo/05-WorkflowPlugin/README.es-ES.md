# Demo de Plugin de Flujo de Trabajo

Demuestra la implementación simultánea de `IPlugin` e `IWorkflowPlugin` en una sola clase. Muestra cómo registrar plantillas de flujo de trabajo personalizadas e inyectar reglas de transición en plantillas existentes.

## Interfaz IWorkflowPlugin

`IWorkflowPlugin` es una interfaz opcional que los plugins pueden implementar junto con `IPlugin` para contribuir definiciones de flujo de trabajo al `WorkflowEngine` del host.

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| Miembro | Propósito |
|---------|-----------|
| `PluginId` | Identificador único — debe coincidir con `IPlugin.Id` |
| `RegisterTemplates()` | Devuelve nuevas plantillas de flujo de trabajo para que el motor las registre |
| `InjectTransitions(template)` | Inyecta reglas de transición adicionales en una plantilla existente |

## Cómo implementar ambas interfaces

Una sola clase implementa tanto `IPlugin` como `IWorkflowPlugin`：

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  Miembros de IPlugin:                                               │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  Miembros de IWorkflowPlugin:                                       │
│    PluginId  →  devuelve Id (debe coincidir con IPlugin.Id)        │
│    RegisterTemplates()  →  crea el flujo PluginApproval            │
│    InjectTransitions()  →  añade FastTrack al flujo CodeReview     │
└──────────────────────────────────────────────────────────────────────┘
```

### Puntos clave

1. **PluginId debe coincidir con IPlugin.Id** — el host correlaciona el plugin con sus contribuciones de flujo de trabajo usando este identificador.

2. **RegisterTemplates() se llama una vez durante el inicio** — el host recopila todas las plantillas de todas las implementaciones `IWorkflowPlugin` y las registra con `WorkflowEngine.RegisterTemplate()`.

3. **InjectTransitions() se llama para cada plantilla registrada** — el host itera por todas las plantillas registradas y pasa cada una a cada `IWorkflowPlugin`, permitiendo extensiones entre plugins.

4. **El orden de llamada es importante** — primero se llama a `RegisterTemplates()` para todos los plugins, luego `InjectTransitions()` para cada plantilla. Esto asegura que las transiciones inyectadas puedan referenciar estados definidos por la plantilla.

## Flujo de trabajo de demostración: PluginApproval

Esta demo crea un flujo de aprobación simple de 4 estados：

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (estado terminal)
                             │
                             └──Reject──▶ Rejected (estado terminal)
```

| Estado | Descripción | Tipo |
|--------|-------------|------|
| `Submitted` | Estado inicial — el plugin ha sido enviado | Normal |
| `Reviewing` | En revisión por un Screener | Normal |
| `Approved` | Plugin aprobado | Terminal |
| `Rejected` | Plugin rechazado | Terminal |

### Definiciones de roles

| Rol | Mín | Máx | Descripción |
|-----|-----|-----|-------------|
| Screener | 1 | 3 | Revisa los envíos de plugins |

### Transiciones

| Nombre | De → A | Condición | Tiempo límite | Roles requeridos |
|--------|--------|-----------|---------------|-----------------|
| StartReview | Submitted → Reviewing | Automático (Screener asignado) | 2 días | Screener |
| Approve | Reviewing → Approved | `ApprovedBy` en metadatos | 5 días | Screener |
| Reject | Reviewing → Rejected | `RejectedBy` en metadatos | — | — |

## Demo InjectTransitions: FastTrack

El plugin inyecta una transición "FastTrack" en el flujo de trabajo `CodeReview` integrado：

```
Draft ──FastTrack──▶ Approved  (omite el estado Reviewing)
```

Esto demuestra cómo un plugin puede extender flujos de trabajo definidos por otros plugins o por el host. La transición `FastTrack` solo se habilita cuando los metadatos de la instancia contienen `TrustedAuthor = true`.

## Secuencia de llamadas

```
Inicio del host
  │
  ├── Cargar DLL → PluginLoader escanea y crea WorkflowPluginDemo
  │
  ├── OnLoad()
  │     └── El plugin prepara las definiciones de flujo de trabajo
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── Devuelve la plantilla PluginApproval
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(plantilla CodeReview)
  │     └── Añade la transición FastTrack
  │
  ├── InjectTransitions(plantilla PluginApproval)
  │     └── (sin operación — este plugin no extiende su propia plantilla)
  │
  └── OnStart()
        └── El plugin está completamente operativo
```

## Esta demo

| Clase | Rol |
|-------|-----|
| `WorkflowPluginDemo` | Implementa tanto `IPlugin` como `IWorkflowPlugin` — registra el flujo PluginApproval e inyecta FastTrack en CodeReview |

## Ver también

- [Interfaz IPlugin](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — interfaz base de plugin
- [Interfaz IWorkflowPlugin](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — interfaz de plugin de flujo de trabajo
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — definición de plantilla
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — reglas de transición de estados
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — ejemplo de flujo de trabajo integrado
