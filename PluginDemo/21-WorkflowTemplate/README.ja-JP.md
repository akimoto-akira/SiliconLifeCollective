# ワークフローテンプレートデモ — 完全なビジネスワークフロー

`WorkflowTemplate` を使用して完全なビジネスワークフローを定義する方法を示します。ステートマシン設計、非同期条件/アクション、ロールベースのアクセス制御、タイムアウト処理、トランジション注入を含みます。シンプルな [05-WorkflowPlugin](../05-WorkflowPlugin/) デモ（`IWorkflowPlugin` インターフェース実装のみを示す）とは異なり、本例は実際のビジネスロジックを持つ完全な "PluginOnboarding" ワークフローを構築します。

## WorkflowTemplate ステートマシンパターン

`WorkflowTemplate` は宣言的なステートマシンを定義します。プロパティ：`Name`、`States`、`TerminalStates`、`Transitions`、`RoleDefinitions`、`Metadata`。`States[0]` が自動的に `InitialState` となります。

## 状態フロー図

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (終端)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (終端)
     │                                                        ▲
     └─── (タイムアウト: 2日) ──▶ Blocked                     │
                                                   FailTesting─┘
```

## Transition 非同期メカニズム

各 `Transition` には2つの非同期デリゲートがあります：`Condition`（bool を返す、各 Tick/60秒で呼び出し）と `Action`（状態変更後の副作用）。インスタンスごとに Tick あたり最大1つのトランジションが実行され、`Priority` 順で評価されます。

## RoleDefinition — ロール制約

ロール定義は必要なリソースを宣言します：`Screener`（min=1, max=3）と `Tester`（min=1, max=0/無制限）。メソッド：`IsSatisfied()`、`GetStaffingStatus()`、`GetStatusText()`、`ValidateRoleAssignments()`。

## RequiredRoles — ランタイムチェックとブロードキャスト

トランジションは `RequiredRoles` を宣言できます。エンジンは `Condition` 評価前にロール充足を確認します。未充足のロールはトランジションをブロックし、ブロードキャスト通知を発行します（スロットル：トランジションごとに24時間に1回）。

## TimeoutDays — 停滞状態の検出

`(now - LastProgressAt).TotalDays > TimeoutDays` の場合、インスタンスは `"Blocked"` とマークされ、`WorkflowLog` レコードが作成され、キュレーター being に通知されます。

## WorkflowTickObject と MainLoop 統合

`WorkflowTickObject` は `TickObject` のサブクラスで、60秒ごとに優先度90でワークフローエンジンを駆動します（beings=100より低く、バックグラウンドタスクより高い）。

## 05-WorkflowPlugin との違い

| 側面 | 05-WorkflowPlugin | 21-WorkflowTemplate（本デモ） |
|------|-------------------|-------------------------------|
| 焦点 | インターフェース実装パターン | 完全なビジネスワークフロー設計 |
| 状態 | 4つのシンプルな状態 | 5つの状態、現実的なフロー |
| ロール | 1ロール | 2ロール、異なる MaxCount 戦略 |
| RequiredRoles | 2つのトランジション | クロスステージゲーティング |
| ビジネスロジック | 最小限 | 完全なオンボーディングライフサイクル |

## 関連項目

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — よりシンプルな IWorkflowPlugin インターフェースデモ
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — テンプレート定義
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — 状態遷移ルール
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — ロール制約
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — エンジンコア
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — Tick ドライバー
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — 組み込みワークフロー
