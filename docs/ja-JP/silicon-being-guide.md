# シリコン生命体ガイド

> **バージョン: v0.1.0-alpha**

[English](../en/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | **日本語** | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md)

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

## ナレッジネットワーク

### 概要

ナレッジネットワークは、三つ組（主語-関係-目的語）に基づくナレッジグラフシステム。

### ナレッジ表現

```
主体 - 関係 - 客体

例：
- Python - is_a - プログラミング言語
- C# - belongs_to - .NET エコシステム
- AI - related_to - 機械学習
```

### KnowledgeTool 操作

```json
// ナレッジを追加
{
  "action": "add",
  "subject": "Python",
  "relation": "is_a",
  "object": "プログラミング言語",
  "confidence": 1.0
}

// ナレッジを検索
{
  "action": "query",
  "subject": "Python"
}

// パスを発見
{
  "action": "get_path",
  "from": "Python",
  "to": "プログラミング"
}
```

## WebView ブラウザ

### 機能

各シリコン生命体は独自の WebView ブラウザインスタンスを持つ：

- **個別分離**：各生命体は独立したブラウザ、Cookie、セッション
- **ヘッドレスモード**：ユーザーには不可視。生命体がバックグラウンドで操作
- **完全なブラウザ操作**：
  - ページナビゲーション
  - クリックと入力
  - コンテンツ取得
  - JavaScript 実行
  - スクリーンショット

### 使用例

```json
{
  "action": "navigate",
  "url": "https://example.com"
}

{
  "action": "click",
  "selector": "#submit-button"
}

{
  "action": "get_content"
}
```

## ベストプラクティス

### 1. 明確なソウルファイル

詳細で明確なソウルファイルを作成：
- 個性と行動を定義
- 能力と制限を指定
- 具体的な例を提供

### 2. 適度なタスク割り当て

- タスクの説明を明確に
- 現実的な期限を設定
- 優先度を適切に割り当てる

### 3. 定期的なメモリ管理

- 古いメモリをクリーンアップ
- 重要な情報を長期メモリに保存
- コンテキストウィンドウを最適化

### 4. 効果的なノート使用

- 作業ノートを定期的に更新
- 検索可能なキーワードを追加
- 重要な決定を記録

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)をチェック
- 📖 [ツールリファレンス](tools-reference.md)を見る
- 🚀 [クイックスタートガイド](getting-started.md)で始める
