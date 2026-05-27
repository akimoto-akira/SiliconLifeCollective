# ワークフロープラグインデモ

単一のクラスで `IPlugin` と `IWorkflowPlugin` の両方を実装する方法を示します。カスタムワークフローテンプレートの登録と、既存テンプレートへのトランジション注入の方法を解説します。

## IWorkflowPlugin インターフェース

`IWorkflowPlugin` は、プラグインが `IPlugin` と一緒に実装できるオプションのインターフェースで、ホストの `WorkflowEngine` にワークフロー定義を提供します。

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| メンバー | 目的 |
|----------|------|
| `PluginId` | 一意識別子 — `IPlugin.Id` と一致する必要があります |
| `RegisterTemplates()` | エンジンに登録する新しいワークフローテンプレートを返します |
| `InjectTransitions(template)` | 既存のテンプレートに追加のトランジションルールを注入します |

## 2つのインターフェースの実装方法

1つのクラスで `IPlugin` と `IWorkflowPlugin` の両方を実装します：

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  IPlugin メンバー:                                                   │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  IWorkflowPlugin メンバー:                                           │
│    PluginId  →  Id を返す（IPlugin.Id と一致する必要があります）    │
│    RegisterTemplates()  →  PluginApproval ワークフローを作成        │
│    InjectTransitions()  →  CodeReview に FastTrack を追加          │
└──────────────────────────────────────────────────────────────────────┘
```

### 重要ポイント

1. **PluginId は IPlugin.Id と一致する必要があります** — ホストはこの識別子を使用してプラグインとワークフロー貢献を関連付けます。

2. **RegisterTemplates() は起動時に1回呼び出されます** — ホストは全 `IWorkflowPlugin` 実装からテンプレートを収集し、`WorkflowEngine.RegisterTemplate()` で登録します。

3. **InjectTransitions() は登録済みの各テンプレートに対して呼び出されます** — ホストは全登録テンプレートを反復処理し、各テンプレートを各 `IWorkflowPlugin` に渡します。これによりクロスプラグイン拡張が可能です。

4. **呼び出し順序が重要** — まず全プラグインの `RegisterTemplates()` が呼ばれ、その後各テンプレートの `InjectTransitions()` が呼ばれます。これにより、注入されたトランジションがテンプレート定義の状態を参照できます。

## デモワークフロー：PluginApproval

このデモはシンプルな4状態の承認ワークフローを作成します：

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (終了状態)
                             │
                             └──Reject──▶ Rejected (終了状態)
```

| 状態 | 説明 | タイプ |
|------|------|--------|
| `Submitted` | 初期状態 — プラグインが提出されました | 通常 |
| `Reviewing` | Screener がレビュー中 | 通常 |
| `Approved` | プラグインが承認されました | 終了 |
| `Rejected` | プラグインが拒否されました | 終了 |

### ロール定義

| ロール | 最小 | 最大 | 説明 |
|--------|------|------|------|
| Screener | 1 | 3 | プラグイン提出のレビュー |

### トランジション

| 名前 | 从 → 到 | 条件 | タイムアウト | 必要ロール |
|------|---------|------|-------------|-----------|
| StartReview | Submitted → Reviewing | 自動（Screener 配置済み） | 2日 | Screener |
| Approve | Reviewing → Approved | メタデータに `ApprovedBy` | 5日 | Screener |
| Reject | Reviewing → Rejected | メタデータに `RejectedBy` | — | — |

## InjectTransitions デモ：FastTrack

プラグインは組み込みの `CodeReview` ワークフローに "FastTrack" トランジションを注入します：

```
Draft ──FastTrack──▶ Approved  （Reviewing 状態をスキップ）
```

これは、あるプラグインが他のプラグインやホスト定義のワークフローを拡張する方法を示しています。`FastTrack` トランジションは、ワークフローインスタンスのメタデータに `TrustedAuthor = true` が含まれる場合のみ有効になります。

## 呼び出し順序

```
ホスト起動
  │
  ├── DLL 読み込み → PluginLoader がスキャンして WorkflowPluginDemo を作成
  │
  ├── OnLoad()
  │     └── プラグインがワークフロー定義を準備
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── PluginApproval テンプレートを返す
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(CodeReview テンプレート)
  │     └── FastTrack トランジションを追加
  │
  ├── InjectTransitions(PluginApproval テンプレート)
  │     └── （何もしない — 自分のテンプレートは拡張しない）
  │
  └── OnStart()
        └── プラグインが完全に稼働可能
```

## このデモ

| クラス | 役割 |
|--------|------|
| `WorkflowPluginDemo` | `IPlugin` と `IWorkflowPlugin` の両方を実装 — PluginApproval ワークフローを登録し、CodeReview に FastTrack を注入 |

## 関連リファレンス

- [IPlugin インターフェース](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — 基本プラグインインターフェース
- [IWorkflowPlugin インターフェース](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — ワークフロープラグインインターフェース
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — テンプレート定義
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — 状態トランジションルール
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — 組み込みワークフロー例
