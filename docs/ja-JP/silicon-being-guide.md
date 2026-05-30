# シリコンビーイングガイド

> **バージョン：v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | **日本語** | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md)

## 概要

シリコンビーイングは、AI駆動のエージェントであり、自律的に思考、行動、進化することができます。

## アーキテクチャ

### 身体-脳の分離

```
┌─────────────────────────────────────┐
│         シリコンビーイング           │
├──────────────────┬──────────────────┤
│   身体            │   脳              │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • 状態管理        │ • 履歴の読み込み  │
│ • トリガー検出    │ • AIの呼び出し    │
│ • ライフサイクル  │ • ツールの実行    │
│                  │ • レスポンスの永続化│
└──────────────────┴──────────────────┘
```

## ソウルファイル

### 構造

```markdown
# Being Name

## Personality
Describe the being's personality traits and characteristics.

## Capabilities
List what this being can do.

## Behavior Guidelines
Define how the being should behave in different situations.

## Knowledge Domain
Specify the being's area of expertise.
```

### 例

```markdown
# Code Review Assistant

## Personality
You are a meticulous code reviewer with 10 years of experience.
You provide constructive feedback and always explain your reasoning.

## Capabilities
- Review code for bugs and best practices
- Suggest performance optimizations
- Explain complex algorithms
- Identify security vulnerabilities

## Behavior Guidelines
- Start with positive observations
- Provide specific examples
- Explain why changes are needed
- Be respectful and professional

## Knowledge Domain
Specialized in C#, .NET, and software architecture.
```

## ビーイングの作成

### Web UIから

1. **ビーイング管理**に移動
2. **新しいビーイングを作成**をクリック
3. 以下を入力：
   - 名前
   - ソウルの内容
   - 設定オプション
4. **作成**をクリック

### APIから

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## ビーイングのライフサイクル

### 活動状態

シリコンビーイングには以下の活動状態があります：

| 状態 | 説明 |
|------|------|
| `Idle` | アイドル状態、クロックトリガーを待機中 |
| `SingleChat` | 1対1チャット中 |
| `GroupChat` | グループチャット中 |
| `Task` | タスク実行中 |
| `Timer` | タイマー実行中 |
| `Broadcast` | ブロードキャストメッセージ処理中 |
| `Project` | プロジェクト作業中 |
| `MemoryCompression` | メモリ圧縮中 |
| `Stopped` | 停止済み、連続エラーまたは手動停止による |

**Stopped 状態のメカニズム**：
- シリコンビーイングが連続して10回エラーが発生した場合、自動的に`Stopped`状態に移行します
- Stopped状態に入ると、ビーイングは一切のタスクを実行しません
- 新しいチャットメッセージが到着すると、エラーカウンターがリセットされ、ビーイングは稼働を再開します
- 手動による介入で再起動することも可能です

### 状態遷移

```
Idle → SingleChat → Idle（チャット完了）
Idle → GroupChat → Idle（グループチャット完了）
Idle → Task → Idle（タスク完了）
Idle → Timer → Idle（タイマー完了）
Idle → Broadcast → Idle（ブロードキャスト処理完了）
Idle → Project → Idle（プロジェクト作業完了）
Idle → MemoryCompression → Idle（メモリ圧縮完了）
任意 → Stopped（連続10回エラー）
Stopped → Idle（新しいチャットメッセージ到達または手動再起動）
```

### 操作

- **起動**：初期化して処理を開始
- **停止**：グレースフルシャットダウン
- **再起動**：Stopped状態からIdle状態に復旧

## タスクシステム

### タスクの作成

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Review the code",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### タスク状態

- `Pending` - 実行待ち
- `Running` - 実行中
- `SubmittedForReview` - レビュー提出済み
- `UnderReview` - レビュー中
- `Rework` - 手戻り修正中
- `Completed` - 完了
- `Failed` - 実行失敗
- `Cancelled` - 手動キャンセル

## タイマーシステム

### タイマーの種類

1. **一回限り**：遅延後に1回実行
2. **間隔**：一定間隔で繰り返し実行
3. **Cron**：cron式に基づいて実行

### 例

```csharp
// 毎時実行
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## メモリシステム

### メモリの種類

- **短期**：現在の会話コンテキスト
- **長期**：永続化された知識と経験
- **エピソード**：時間インデックス付きのイベントとインタラクション

### ストレージ構造

Default版：
```
data/
└── beings/
    └── {being-id}/
        ├── soul.md
        ├── memory/
        │   ├── short-term.json
        │   └── long-term/
        │       ├── 2026-04-20.json
        │       └── 2026-04-21.json
        └── tasks/
            └── task-history.json
```

Fast版（SpeedyPackストレージ）：
```
data/
├── speedy/
│   ├── {being-id}.spk       # SpeedyPackストレージファイル
│   └── {being-id}.spk.idx   # インデックスファイル
└── beings/
    └── {being-id}/
        └── soul.md
```

## ワークノートシステム

### 概要

ワークノートは、シリコンビーイングの個人日記システムであり、ページ式設計を採用し、作業進捗、学習記録、プロジェクトメモなどを記録するために使用します。

### 特徴

- **ページ管理**：各ノートは独立したページとなり、ページ番号でアクセス
- **Markdownサポート**：コンテンツはMarkdown形式に対応（テキスト、リスト、テーブル、コードブロック）
- **キーワードインデックス**：ノートにキーワードを追加可能、検索に便利
- **サマリー機能**：各ノートに短い要約があり、素早く閲覧可能
- **目次生成**：全ノートの目次概要を生成可能、全体のコンテキスト理解に役立つ
- **タイムスタンプ**：作成日時と更新日時を自動記録
- **デフォルトでプライベート**：ビーイング自身のみアクセス可能（キュレーターは管理可能）

### ユースケース

1. **プロジェクト進捗記録**
   ```
   要約：ユーザー認証モジュール完了
   内容：JWT token検証、OAuth2統合、tokenリフレッシュ機構を実装
   キーワード：認証,JWT,OAuth2
   ```

2. **学習ノート**
   ```
   要約：C#非同期プログラミングのベストプラクティスを学習
   内容：async/await使用上の注意点、ConfigureAwaitの使用シーン...
   キーワード：C#,非同期,ベストプラクティス
   ```

3. **議事録**
   ```
   要約：製品要件検討会
   内容：新機能要件について議論、実装方針を決定...
   キーワード：製品,要件,会議
   ```

### ツールからの利用

ビーイングは`work_note`ツールを使ってワークノートを管理できます：

```json
// ノートの作成
{
  "action": "create",
  "summary": "ユーザー認証モジュール完了",
  "content": "## 実装詳細\n\n- JWT tokenを使用\n- OAuth2をサポート",
  "keywords": "認証,JWT,OAuth2"
}

// ノートの読み取り
{
  "action": "read",
  "page_number": 1
}

// ノートの検索
{
  "action": "search",
  "keyword": "認証",
  "max_results": 10
}
```

### Web UIからの管理

1. **ビーイング管理**に移動 → ビーイングを選択
2. **ワークノート**タブをクリック
3. ノートの閲覧、検索、編集が可能
4. Markdownプレビューに対応

## ナレッジネットワークシステム

### 概要

ナレッジネットワークは、トリプル構造（主語-述語-目的語）に基づく知識表現・管理システムであり、構造化された知識の保存と管理に使用します。

### コア概念

#### トリプル構造

```
主語 (Subject) --述語 (Predicate)--> 目的語 (Object)
```

**例**：
- `Python` --`is_a`--> `programming_language`
- `北京` --`capital_of`--> `中国`
- `水` --`boiling_point`--> `100°C`

#### 信頼度

各知識トリプルには信頼度スコア（0.0-1.0）があり、知識の確からしさを表します：
- `1.0`：絶対的に確実（数学の定理など）
- `0.8-0.99`：高い信頼性（検証済みの事実など）
- `0.5-0.79`：中程度の信頼性（推論や仮説など）
- `<0.5`：低い信頼性（推測や未検証の情報など）

#### タグシステム

トリプルにタグを追加でき、分類と検索に便利です：
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### 知識操作

#### 1. 知識の追加

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programming", "language"]
}
```

#### 2. 知識の照会

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. 知識の検索

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. 知識パスの発見

2つの概念間の関連パスを見つけます：
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

戻り値：
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. 知識の検証

知識の有効性と一貫性をチェックします：
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. 知識統計

ナレッジネットワーク全体の統計情報を取得します：
```json
{
  "action": "stats"
}
```

戻り値：
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### ユースケース

1. **事実の保存**
   - 客観的な事実と常識を保存
   - 例：`地球` --`is_a`--> `惑星`

2. **概念間の関係**
   - 概念間の関係を記録
   - 例：`継承` --`is_a`--> `オブジェクト指向プログラミング概念`

3. **学習の蓄積**
   - ビーイングが学習を通じて知識を継続的に蓄積
   - 構造化された知識体系を形成

4. **推論のサポート**
   - 知識パスを通じて間接的な関係を発見
   - 知識に基づく推論と意思決定をサポート

### Web UIからの管理

1. **ナレッジネットワーク**ページに移動
2. 知識統計情報を確認
3. 知識の検索と閲覧
4. 知識関係図の可視化（計画中）

## WebViewブラウザ操作（新規追加）

### 概要

シリコンビーイングは、WebViewブラウザツールを使用して自律的にウェブページを閲覧、情報を取得、ウェブ操作を実行できます。ブラウザはヘッドレスモードで動作し、ユーザーには完全に見えません。

### 特徴

- **個体分離**：各ビーイングが独立したブラウザインスタンス、Cookie、セッションを持つ
- **ヘッドレスモード**：バックグラウンドで自律動作、ユーザーには不可視
- **フル機能**：JavaScript実行、CSSレンダリング、フォーム入力などをサポート
- **セキュリティ制御**：すべての操作はパーミッション検証チェーンを通過する必要がある

### 主要な操作

#### 1. ブラウザを開く

```json
{
  "action": "open"
}
```

#### 2. ウェブページに移動

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. ページ内容の取得

```json
{
  "action": "get_page_text"
}
```

ページのテキスト内容を返し、AIによる分析と理解に利用します。

#### 4. 要素のクリック

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. テキストの入力

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "検索キーワード"
}
```

#### 6. JavaScriptの実行

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. スクリーンショットの取得

```json
{
  "action": "get_screenshot"
}
```

ページのスクリーンショット（Base64エンコード）を返します。視覚分析に利用できます。

#### 8. 要素の出現待機

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### ユースケース

1. **情報収集**
   - ニュースサイトを閲覧して最新情報を取得
   - ドキュメントや技術資料の検索
   - ウェブページの内容変化を監視

2. **自動化操作**
   - フォームの入力と送信
   - ボタンのクリックによる操作の実行
   - ウェブページデータのスクレイピング

3. **ウェブページ分析**
   - ページ構造とコンテンツの分析
   - 特定情報の抽出
   - スクリーンショットによる視覚的分析

### 注意事項

- ブラウザ操作は遅くなる場合があり、ページの読み込み完了を待つ必要があります
- `wait_for_element`を使用して要素が出現したことを確認してから操作してください
- ウェブサイトの利用規約とrobots.txtを遵守してください
- 頻繁なリクエストによるアクセス制限を避けてください

## ベストプラクティス

### ソウルファイルの作成

1. **具体的に**：明確な個性特性と境界を定義
2. **スコープの定義**：ビーイングがすべきこととすべきでないことを明記
3. **例を含める**：期待される行動パターンを示す
4. **定期的な更新**：パフォーマンスに基づいてソウルを進化させる

### タスク管理

1. **優先度の設定**：優先度（1-10）を使用
2. **期限の定義**：常に期限を設定
3. **進捗の監視**：定期的にタスクの状態を確認
4. **失敗の処理**：リトライロジックを実装

### メモリの最適化

1. **古いデータのクリーンアップ**：定期的に古いメモリをアーカイブ
2. **重要情報のインデックス**：キー情報にマークを付ける
3. **時間ストレージの活用**：時間インデックスを利用したクエリ

### メモリ減衰メカニズム

システムには`MemoryFadeService`という定期減衰サービスが内蔵されており、生物の記憶の忘却特性をシミュレートします：

- **自動減衰**：毎時、全シリコンビーイングのメモリエントリに重要度減衰アルゴリズムを適用
- **自動アーカイブ**：重要度が閾値を下回ったメモリは自動的にアーカイブされ、日常の検索対象から外れる
- **統計追跡**：減衰サイクル数と状態変更エントリ数を記録

つまり、シリコンビーイングのメモリは時間とともに自然に薄れ、重要な情報はメモリツールを使って能動的に高重要度としてマークする必要があります。そうしないと自動アーカイブの対象となります。

---

## プロジェクトワークスペース

### 概要

プロジェクトワークスペースは、複数のシリコンビーイングによる協調をサポートするスペース管理メカニズムです。シリコンキュレーターはプロジェクトスペースを作成し、シリコンビーイングをプロジェクトに割り当て、ロールを付与できます。

### プロジェクトライフサイクル

```
作成 → アクティブ → アーカイブ → 破棄
              ↑       |
              └─ 復元 ┘
```

### プロジェクトロール

シリコンビーイングはプロジェクト内で特定のロールを割り当てることができます：

```json
{
  "action": "assign_role",
  "project_id": "project-uuid",
  "being_id": "being-uuid",
  "role_name": "developer"
}
```

### プロジェクトワークノート

プロジェクトスペース内のワークノートは公開されており、プロジェクトメンバー全員がアクセスできます：

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "ユーザー認証モジュール完了",
  "content": "## 実装詳細\n\n- JWT tokenを使用",
  "keywords": "認証,JWT"
}
```

### プロジェクトタスク

プロジェクトスペース内のタスクは完全なライフサイクル管理をサポートします：

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "title": "ユーザー認証の実装",
  "priority": 5
}
```

### プロジェクトワークフロー

プロジェクトにはワークフローテンプレートをバインドでき、シリコンビーイングの協調プロセスを駆動できます：

- ワークフローはステートマシンテンプレートに基づく
- Tick駆動の状態遷移をサポート
- 状態遷移ログを自動記録

### ツールパーミッションの分離

プロジェクトレベルのツールパーミッションは、シリコンビーイングレベルのパーミッションから独立しており、プロジェクト間のパーミッション分離を実現します。例えば、あるシリコンビーイングがプロジェクトAではネットワークアクセス権を持っていても、プロジェクトBでは読み取り専用権限に制限される場合があります。

## トラブルシューティング

### ビーイングが起動しない

**確認事項**：
- ソウルファイルが存在し有効であること
- AIクライアントが設定済みであること
- システムリソースが十分であること

### ビーイングが予期せず停止する

**確認事項**：
- ログ内のエラー
- AIサービスの可用性
- メモリ使用量

### タスクが実行されない

**確認事項**：
- タイマーシステムが稼働中であること
- タスクの優先度とスケジュール
- パーミッション設定

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)を確認する
- 🚀 [クイックスタートガイド](getting-started.md)を確認する
