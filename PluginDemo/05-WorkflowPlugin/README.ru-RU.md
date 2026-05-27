# Демо плагина рабочего процесса

Демонстрирует одновременную реализацию `IPlugin` и `IWorkflowPlugin` в одном классе. Показывает, как регистрировать пользовательские шаблоны рабочих процессов и внедрять правила перехода в существующие шаблоны.

## Интерфейс IWorkflowPlugin

`IWorkflowPlugin` — необязательный интерфейс, который плагины могут реализовать вместе с `IPlugin` для предоставления определений рабочих процессов `WorkflowEngine` хоста.

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| Член | Назначение |
|------|-----------|
| `PluginId` | Уникальный идентификатор — должен совпадать с `IPlugin.Id` |
| `RegisterTemplates()` | Возвращает новые шаблоны рабочих процессов для регистрации движком |
| `InjectTransitions(template)` | Внедряет дополнительные правила перехода в существующий шаблон |

## Как реализовать оба интерфейса

Один класс реализует и `IPlugin`, и `IWorkflowPlugin`：

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  Члены IPlugin:                                                     │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  Члены IWorkflowPlugin:                                             │
│    PluginId  →  возвращает Id (должен совпадать с IPlugin.Id)      │
│    RegisterTemplates()  →  создаёт рабочий процесс PluginApproval   │
│    InjectTransitions()  →  добавляет FastTrack в CodeReview        │
└──────────────────────────────────────────────────────────────────────┘
```

### Ключевые моменты

1. **PluginId должен совпадать с IPlugin.Id** — хост использует этот идентификатор для связи плагина с его вкладом в рабочие процессы.

2. **RegisterTemplates() вызывается один раз при запуске** — хост собирает все шаблоны от всех реализаций `IWorkflowPlugin` и регистрирует их через `WorkflowEngine.RegisterTemplate()`.

3. **InjectTransitions() вызывается для каждого зарегистрированного шаблона** — хост перебирает все зарегистрированные шаблоны и передаёт каждый каждому `IWorkflowPlugin`, допуская межплагинные расширения.

4. **Порядок вызовов важен** — сначала вызывается `RegisterTemplates()` для всех плагинов, затем `InjectTransitions()` для каждого шаблона. Это гарантирует, что внедрённые переходы могут ссылаться на состояния, определённые шаблоном.

## Демо-рабочий процесс: PluginApproval

Это демо создаёт простой 4-состоянийный рабочий процесс утверждения：

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (конечное состояние)
                             │
                             └──Reject──▶ Rejected (конечное состояние)
```

| Состояние | Описание | Тип |
|-----------|----------|-----|
| `Submitted` | Начальное состояние — плагин отправлен | Обычное |
| `Reviewing` | Проверяется Screener'ом | Обычное |
| `Approved` | Плагин одобрен | Конечное |
| `Rejected` | Плагин отклонён | Конечное |

### Определения ролей

| Роль | Мин | Макс | Описание |
|------|-----|------|----------|
| Screener | 1 | 3 | Проверяет отправленные плагины |

### Переходы

| Имя | Из → В | Условие | Таймаут | Требуемые роли |
|-----|--------|---------|---------|---------------|
| StartReview | Submitted → Reviewing | Автоматически (Screener назначен) | 2 дня | Screener |
| Approve | Reviewing → Approved | `ApprovedBy` в метаданных | 5 дней | Screener |
| Reject | Reviewing → Rejected | `RejectedBy` в метаданных | — | — |

## Демо InjectTransitions: FastTrack

Плагин внедряет переход "FastTrack" во встроенный рабочий процесс `CodeReview`：

```
Draft ──FastTrack──▶ Approved  (пропускает состояние Reviewing)
```

Это демонстрирует, как плагин может расширять рабочие процессы, определённые другими плагинами или хостом. Переход `FastTrack` активируется только когда метаданные экземпляра содержат `TrustedAuthor = true`.

## Последовательность вызовов

```
Запуск хоста
  │
  ├── Загрузка DLL → PluginLoader сканирует и создаёт WorkflowPluginDemo
  │
  ├── OnLoad()
  │     └── Плагин готовит определения рабочих процессов
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── Возвращает шаблон PluginApproval
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(шаблон CodeReview)
  │     └── Добавляет переход FastTrack
  │
  ├── InjectTransitions(шаблон PluginApproval)
  │     └── (нет операции — этот плагин не расширяет свой собственный шаблон)
  │
  └── OnStart()
        └── Плагин полностью готов к работе
```

## Это демо

| Класс | Роль |
|-------|------|
| `WorkflowPluginDemo` | Реализует и `IPlugin`, и `IWorkflowPlugin` — регистрирует рабочий процесс PluginApproval и внедряет FastTrack в CodeReview |

## Смотрите также

- [Интерфейс IPlugin](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — базовый интерфейс плагина
- [Интерфейс IWorkflowPlugin](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — интерфейс плагина рабочего процесса
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — определение шаблона
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — правила перехода состояний
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — пример встроенного рабочего процесса
