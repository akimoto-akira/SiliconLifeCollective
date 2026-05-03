# シリコン生命体ガイド

> **バージョン: v0.1.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | **日本語** | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md)

## 概要

シリコン生命体は AI 駆動の自律エージェント。自ら思考し、行動し、進化することができます。

## アーキテクチャ

### 身体-大脳分離

```
┌─────────────────────────────────────┐
│         シリコン生命体               │
├──────────────────┬──────────────────┤
│   身体            │   大脳            │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • 状態管理        │ • 履歴の読み込み  │
│ • トリガー検出    │ • AI 呼び出し     │
│ • ライフサイクル  │ • ツール実行      │
│                  │ • 応答の永続化    │
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

## 生命体の作成

### Web UI 経由

1. **生命体管理**に移動
2. **新生命体を作成**をクリック
3. 入力：
   - 名前
   - ソウルコンテンツ
   - 設定オプション
4. **作成**をクリック

### API 経由

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## 生命体ライフサイクル

### 状態

```
Created → Starting → Running → Stopping → Stopped
                    ↓
                  Error
```

### 操作

- **起動**：初期化して処理を開始
- **停止**：グレースフルシャットダウン
- **一時停止**：一時的なサスペンド（状態を保持）
- **再開**：一時停止状態から継続

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
- `Completed` - 正常完了
- `Failed` - 実行失敗
- `Cancelled` - 手動キャンセル

## タイマーシステム

### タイマータイプ

1. **ワンタイム**：遅延後に1回実行
2. **インターバル**：固定間隔で繰り返し実行
3. **Cron**：cron 式に基づいて実行

### 例

```csharp
// 毎時間実行
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

### メモリタイプ

- **短期**：現在の会話コンテキスト
- **長期**：永続化された知識と経験
- **エピソード**：時間インデックス付きのイベントとインタラクション

### ストレージ構造

Default 版：
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

Fast 版（SpeedyPack ストレージ）：
```
data/
├── speedy/
│   ├── {being-id}.spk       # SpeedyPack ストレージファイル
│   └── {being-id}.spk.idx   # インデックスファイル
└── beings/
    └── {being-id}/
        └── soul.md
```

## 作業ノートシステム

### 概要

作業ノートはシリコン生命体の個人日記システム。ページ式设计で、作業進捗、学習メモ、プロジェクトノートなどを記録。

### 機能

- **ページ管理**：各ノートは独立したページ。ページ番号でアクセス
- **Markdown サポート**：コンテンツは Markdown 形式をサポート（テキスト、リスト、テーブル、コードブロック）
- **キーワードインデックス**：ノートにキーワードを追加可能。検索に便利
- **要約機能**：各ノートに簡単な要約。素早い閲覧
- **目次生成**：すべてのノートの目次概览を生成。全体のコンテキスト理解を支援
- **タイムスタンプ**：作成・更新時間を自動記録
- **デフォルトプライベート**：生命体自身のみアクセス可能（管理人は管理可能）

### 使用シナリオ

1. **プロジェクト進捗記録**
   ```
   要約：ユーザー認証モジュールを完了
   コンテンツ：JWT token 認証、OAuth2 統合、リフレッシュ token メカニズムを実装
   キーワード：認証,JWT,OAuth2
   ```

2. **学習ノート**
   ```
   要約：C# 非同期プログラミングのベストプラクティスを学習
   コンテンツ：async/await 使用上の注意事項、ConfigureAwait の使用シナリオ...
   キーワード：C#,非同期,ベストプラクティス
   ```

3. **会議記録**
   ```
   要約：製品要件ディスカッション会議
   コンテンツ：新機能要件を議論。実装方案を決定...
   キーワード：製品,要件,会議
   ```

### ツール経由での使用

生命体は `work_note` ツールを介して作業ノートを管理可能：

```json
// ノートを作成
{
  "action": "create",
  "summary": "ユーザー認証モジュールを完了",
  "content": "## 実装詳細\n\n- JWT token を使用\n- OAuth2 をサポート",
  "keywords": "認証,JWT,OAuth2"
}

// ノートを読み取り
{
  "action": "read",
  "page_number": 1
}

// ノートを検索
{
  "action": "search",
  "keyword": "認証",
  "max_results": 10
}
```

### Web UI 経由での管理

1. **プロジェクト**ページに移動
2. **作業ノート**セクションをクリック
3. ノート管理：
   - 新ノートを作成
   - タイムラインで閲覧
   - キーワードで検索
   - 目次を生成

## ナレッジネットワークシステム

### 概要

ナレッジネットワークは、三つ組構造（主語-述語-目的語）に基づくナレッジ表現および管理システム。構造化されたナレッジの保存と管理に使用。

### コアコンセプト

#### 三つ組構造

```
主語 (Subject) --述語 (Predicate)--> 目的語 (Object)
```

**例**：
- `Python` --`is_a`--> `programming_language`
- `北京` --`capital_of`--> `中国`
- `水` --`boiling_point`--> `100°C`

#### 信頼度

各ナレッジ三つ組には信頼度スコア（0.0-1.0）があり、ナレッジの信頼性を示す：
- `1.0`：絶対的に確実（数学定理など）
- `0.8-0.99`：高い信頼性（検証済みの事実など）
- `0.5-0.79`：中程度の信頼性（推論や仮説など）
- `<0.5`：低い信頼性（推測や未検証情報など）

#### タグシステム

三つ組にタグの追加をサポート。分類と検索に便利：
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### ナレッジ操作

#### 1. ナレッジの追加

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

#### 2. ナレッジのクエリ

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. ナレッジの検索

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. ナレッジパスの発見

2つの概念間の関連パスを見つける：
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

返却：
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. ナレッジの検証

ナレッジの有効性と一貫性をチェック：
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. ナレッジ統計

ナレッジネットワーク全体の統計情報を取得：
```json
{
  "action": "stats"
}
```

返却：
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### 使用シナリオ

1. **事実の保存**
   - 客観的事実と常識を保存
   - 例：`地球` --`is_a`--> `惑星`

2. **概念関係**
   - 概念間の関係を記録
   - 例：`継承` --`is_a`--> `オブジェクト指向プログラミング概念`

3. **学習の蓄積**
   - 生命体は学習を通じてナレッジを継続的に蓄積
   - 構造化されたナレッジ体系を形成

4. **推論サポート**
   - ナレッジパスの発見で間接的な関係を見出す
   - ナレッジに基づく推論と意思決定をサポート

### Web UI 経由での管理

1. **ナレッジネットワーク**ページに移動
2. ナレッジ統計情報を表示
3. ナレッジを検索および閲覧
4. ナレッジ関係図の可視化（計画中）

## WebView ブラウザ操作

### 概要

シリコン生命体は WebView ブラウザツールを介して自律的にウェブページを閲覧、情報を取得、ウェブ操作を実行できます。ブラウザはヘッドレスモードで動作し、ユーザーには完全に不可視。

### 特性

- **個体分離**：各生命体は独立したブラウザインスタンス、Cookie、セッションを持つ
- **ヘッドレスモード**：バックグラウンドで自律操作。ユーザーには不可視
- **完全機能**：JavaScript 実行、CSS レンダリング、フォーム入力などをサポート
- **セキュリティ制御**：すべての操作は権限検証チェーンを通過する必要がある

### 一般的な操作

#### 1. ブラウザを開く

```json
{
  "action": "open_browser"
}
```

#### 2. ウェブページにナビゲート

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. ページコンテンツを取得

```json
{
  "action": "get_page_text"
}
```

ページのテキストコンテンツを返却。AI の分析と理解に使用。

#### 4. 要素をクリック

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. テキストを入力

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "検索キーワード"
}
```

#### 6. JavaScript を実行

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. スクリーンショットを取得

```json
{
  "action": "get_screenshot"
}
```

ページのスクリーンショット（Base64 エンコード）を返却。視覚分析に使用可能。

#### 8. 要素の出現を待機

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### 使用シナリオ

1. **情報取得**
   - ニュースサイトを閲覧して最新情報を取得
   - ドキュメントや技術資料を検索
   - ウェブページのコンテンツ変化を監視

2. **自動化操作**
   - フォームに入力して送信
   - ボタンをクリックして操作をトリガー
   - ウェブページデータをスクレイピング

3. **ウェブ分析**
   - ページ構造とコンテンツを分析
   - 特定の情報を抽出
   - スクリーンショットで視覚的分析

### 注意事項

- ブラウザ操作は遅くなる可能性がある。ページの読み込み完了を待つ必要がある
- `wait_for_element` を使用して要素が出現してから操作
- ウェブサイトの利用規約と robots.txt を遵守
- 頻繁なリクエストによるアクセス制限を回避

## ベストプラクティス

### ソウルファイルの作成

1. **具体的**：明確な個性特性と境界
2. **スコープの定義**：生命体がすべきこととすべきでないこと
3. **例を含める**：期待される行動パターンを示す
4. **定期的な更新**：パフォーマンスに基づいてソウルを進化

### タスク管理

1. **優先度の設定**：優先度（1-10）を使用
2. **期限の定義**：常に期限を設定
3. **進捗のモニタリング**：定期的にタスク状態を確認
4. **失敗の処理**：リトライロジックを実装

### メモリ最適化

1. **古いデータのクリーンアップ**：定期的に古いメモリをアーカイブ
2. **重要情報のインデックス**：キー情報をマーク
3. **時間ストレージの使用**：時間インデックスクエリを活用

## トラブルシューティング

### 生命体が起動しない

**確認**：
- ソウルファイルが存在し有効
- AI クライアントが設定済み
- システムリソースが十分

### 生命体が予期せず停止

**確認**：
- ログのエラー
- AI サービスの可用性
- メモリ使用量

### タスクが実行されない

**確認**：
- タイマーシステムが実行中
- タスクの優先度とスケジュール
- 権限設定

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)をチェック
- 🚀 [クイックスタートガイド](getting-started.md)を見る
