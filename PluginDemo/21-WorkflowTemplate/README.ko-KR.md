# 워크플로 템플릿 데모 — 완전한 비즈니스 워크플로

`WorkflowTemplate`을 사용하여 완전한 비즈니스 워크플로를 정의하는 방법을 보여줍니다. 상태 머신 설계, 비동기 조건/액션, 역할 기반 접근 제어, 타임아웃 처리, 트랜지션 주입을 포함합니다. 더 간단한 [05-WorkflowPlugin](../05-WorkflowPlugin/) 데모(`IWorkflowPlugin` 인터페이스 구현만 보여줌)와 달리, 이 예제는 실제 비즈니스 로직을 가진 완전한 "PluginOnboarding" 워크플로를 구축합니다.

## WorkflowTemplate 상태 머신 패턴

`WorkflowTemplate`은 선언적 상태 머신을 정의합니다. 속성: `Name`, `States`, `TerminalStates`, `Transitions`, `RoleDefinitions`, `Metadata`. `States[0]`이 자동으로 `InitialState`가 됩니다.

## 상태 흐름도

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (종료)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (종료)
     │                                                        ▲
     └─── (타임아웃: 2일) ──▶ Blocked                         │
                                                   FailTesting─┘
```

## Transition 비동기 메커니즘

각 `Transition`에는 두 개의 비동기 델리게이트가 있습니다: `Condition`(bool 반환, 매 Tick/60초마다 호출)과 `Action`(상태 변경 후 부작용). 인스턴스당 Tick당 최대 하나의 트랜지션이 실행되며, `Priority` 순서로 평가됩니다.

## RoleDefinition — 역할 제약

역할 정의는 필요한 리소스를 선언합니다: `Screener`(min=1, max=3)와 `Tester`(min=1, max=0/무제한). 메서드: `IsSatisfied()`, `GetStaffingStatus()`, `GetStatusText()`, `ValidateRoleAssignments()`.

## RequiredRoles — 런타임 검사 및 브로드캐스트

트랜지션은 `RequiredRoles`를 선언할 수 있습니다. 엔진은 `Condition` 평가 전에 역할 충족을 확인합니다. 미충족 역할은 트랜지션을 차단하고 브로드캐스트 알림을 발행합니다(스로틀: 트랜지션당 24시간에 1회).

## TimeoutDays — 정체 상태 감지

`(now - LastProgressAt).TotalDays > TimeoutDays`이면 인스턴스가 `"Blocked"`로 표시되고, `WorkflowLog` 레코드가 생성되며, 큐레이터 being에게 알림이 전송됩니다.

## WorkflowTickObject와 MainLoop 통합

`WorkflowTickObject`는 `TickObject`의 서브클래스로, 60초마다 우선순위 90으로 워크플로 엔진을 구동합니다(beings=100보다 낮고, 백그라운드 작업보다 높음).

## 05-WorkflowPlugin과의 차이

| 측면 | 05-WorkflowPlugin | 21-WorkflowTemplate (이 데모) |
|------|-------------------|-------------------------------|
| 초점 | 인터페이스 구현 패턴 | 완전한 비즈니스 워크플로 설계 |
| 상태 | 4개 단순 상태 | 5개 상태, 현실적 흐름 |
| 역할 | 1개 역할 | 2개 역할, 다른 MaxCount 전략 |
| RequiredRoles | 2개 트랜지션 | 크로스 스테이지 게이팅 |
| 비즈니스 로직 | 최소 | 완전한 온보딩 라이프사이클 |

## 참조

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — 더 간단한 IWorkflowPlugin 인터페이스 데모
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — 템플릿 정의
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — 상태 전이 규칙
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — 역할 제약
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — 엔진 코어
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — Tick 드라이버
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — 내장 워크플로
