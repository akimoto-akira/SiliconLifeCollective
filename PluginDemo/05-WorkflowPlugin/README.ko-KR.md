# 워크플로 플러그인 데모

단일 클래스에서 `IPlugin`과 `IWorkflowPlugin`을 모두 구현하는 방법을 보여줍니다. 사용자 정의 워크플로 템플릿 등록 및 기존 템플릿에 전이 규칙 주입 방법을 시연합니다.

## IWorkflowPlugin 인터페이스

`IWorkflowPlugin`은 플러그인이 `IPlugin`과 함께 구현할 수 있는 선택적 인터페이스로, 호스트의 `WorkflowEngine`에 워크플로 정의를 제공합니다.

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| 멤버 | 용도 |
|------|------|
| `PluginId` | 고유 식별자 — `IPlugin.Id`와 일치해야 함 |
| `RegisterTemplates()` | 엔진에 등록할 새 워크플로 템플릿 반환 |
| `InjectTransitions(template)` | 기존 템플릿에 추가 전이 규칙 주입 |

## 두 인터페이스를 동시에 구현하는 방법

하나의 클래스가 `IPlugin`과 `IWorkflowPlugin`을 모두 구현합니다：

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  IPlugin 멤버:                                                       │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  IWorkflowPlugin 멤버:                                               │
│    PluginId  →  Id 반환 (IPlugin.Id와 일치해야 함)                  │
│    RegisterTemplates()  →  PluginApproval 워크플로 생성             │
│    InjectTransitions()  →  CodeReview에 FastTrack 추가             │
└──────────────────────────────────────────────────────────────────────┘
```

### 핵심 포인트

1. **PluginId는 IPlugin.Id와 일치해야 합니다** — 호스트는 이 식별자를 사용하여 플러그인과 워크플로 기여를 연결합니다.

2. **RegisterTemplates()는 시작 시 한 번 호출됩니다** — 호스트는 모든 `IWorkflowPlugin` 구현에서 템플릿을 수집하여 `WorkflowEngine.RegisterTemplate()`로 등록합니다.

3. **InjectTransitions()는 등록된 각 템플릿에 대해 호출됩니다** — 호스트는 모든 등록된 템플릿을 순회하며 각각을 모든 `IWorkflowPlugin`에 전달하여 교차 플러그인 확장을 허용합니다.

4. **호출 순서가 중요합니다** — 모든 플러그인의 `RegisterTemplates()`가 먼저 호출되고, 그 다음 각 템플릿에 대해 `InjectTransitions()`가 호출됩니다. 이렇게 하면 주입된 전이가 템플릿에 정의된 상태를 참조할 수 있습니다.

## 데모 워크플로: PluginApproval

이 데모는 간단한 4상태 승인 워크플로를 생성합니다：

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (종료 상태)
                             │
                             └──Reject──▶ Rejected (종료 상태)
```

| 상태 | 설명 | 유형 |
|------|------|------|
| `Submitted` | 초기 상태 — 플러그인이 제출됨 | 일반 |
| `Reviewing` | Screener가 검토 중 | 일반 |
| `Approved` | 플러그인 승인됨 | 종료 |
| `Rejected` | 플러그인 거부됨 | 종료 |

### 역할 정의

| 역할 | 최소 | 최대 | 설명 |
|------|------|------|------|
| Screener | 1 | 3 | 플러그인 제출물 검토 |

### 전이 규칙

| 이름 | 에서 → 로 | 조건 | 타임아웃 | 필수 역할 |
|------|-----------|------|---------|----------|
| StartReview | Submitted → Reviewing | 자동 (Screener 배치됨) | 2일 | Screener |
| Approve | Reviewing → Approved | 메타데이터에 `ApprovedBy` | 5일 | Screener |
| Reject | Reviewing → Rejected | 메타데이터에 `RejectedBy` | — | — |

## InjectTransitions 데모: FastTrack

플러그인이 내장 `CodeReview` 워크플로에 "FastTrack" 전이를 주입합니다：

```
Draft ──FastTrack──▶ Approved  (Reviewing 상태 건너뛰기)
```

이는 한 플러그인이 다른 플러그인이나 호스트에서 정의한 워크플로를 확장하는 방법을 보여줍니다. `FastTrack` 전이는 워크플로 인스턴스 메타데이터에 `TrustedAuthor = true`가 포함된 경우에만 활성화됩니다.

## 호출 순서

```
호스트 시작
  │
  ├── DLL 로드 → PluginLoader가 스캔하여 WorkflowPluginDemo 생성
  │
  ├── OnLoad()
  │     └── 플러그인이 워크플로 정의 준비
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── PluginApproval 템플릿 반환
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(CodeReview 템플릿)
  │     └── FastTrack 전이 추가
  │
  ├── InjectTransitions(PluginApproval 템플릿)
  │     └── (작업 없음 — 자체 템플릿은 확장하지 않음)
  │
  └── OnStart()
        └── 플러그인이 완전히 가동 준비 완료
```

## 이 데모

| 클래스 | 역할 |
|--------|------|
| `WorkflowPluginDemo` | `IPlugin`과 `IWorkflowPlugin` 모두 구현 — PluginApproval 워크플로 등록 및 CodeReview에 FastTrack 주입 |

## 관련 참조

- [IPlugin 인터페이스](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — 기본 플러그인 인터페이스
- [IWorkflowPlugin 인터페이스](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — 워크플로 플러그인 인터페이스
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — 템플릿 정의
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — 상태 전이 규칙
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — 내장 워크플로 예시
