# ツールリファレンス

> **バージョン: v0.1.0-alpha**

[English](tools-reference.md) | [简体中文](docs/zh-CN/tools-reference.md) | [繁體中文](docs/zh-HK/tools-reference.md) | [Español](docs/es-ES/tools-reference.md) | [日本語](docs/ja-JP/tools-reference.md) | [한국어](docs/ko-KR/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [Čeština](docs/cs-CZ/tools-reference.md)

## 概要

ツールは AI がシステムと対話できるようにする関数です。各ツールは `ITool` インターフェースを実装し、リフレクションを介して自動的に検出されます。

---

## 組み込みツール

### 1. CalendarTool

日付変換とカレンダー情報。

**名前**：`calendar`

**説明**：複数のカレンダーシステム間で日付を変換

**パラメータ**：
```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**例**：
```
Q: "2026年春節はいつ？"
A: calendar ツールを呼び出して変換
```

---

### 2. ChatTool

メッセージ履歴とチャット管理。

**名前**：`chat`

**説明**：チャット履歴の管理と検索

**パラメータ**：
```json
{
  "action": "get_history",
  "sessionId": "session-uuid",
  "limit": 50
}
```

---

### 3. StorageTool

永続ストレージへのデータの読み書き。

**名前**：`storage`

**説明**：キーバリューストアへのデータの保存と取得

**パラメータ**：
```json
{
  "action": "write",
  "key": "being:uuid:memory",
  "value": "データ"
}
```

---

### 4. TaskTool

タスクとリマインダーの管理。

**名前**：`task`

**説明**：タスクの作成、更新、取得

**パラメータ**：
```json
{
  "action": "create",
  "description": "コードをレビュー",
  "priority": 5,
  "dueDate": "2026-04-21"
}
```

---

### 5. TimerTool

タイマーとスケジューラーの管理。

**名前**：`timer`

**説明**：繰り返しアクションのタイマーを設定

**パラメータ**：
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "action": "think"
}
```

---

### 6. MemoryTool

長期記憶の管理。

**名前**：`memory`

**説明**：シリコンビーイングの長期記憶の保存と検索

**パラメータ**：
```json
{
  "action": "save",
  "content": "ユーザーは青が好き",
  "tags": ["preference", "color"]
}
```

---

### 7. SystemTool

システム情報とモニタリング。

**名前**：`system`

**説明**：システムリソースとステータスの取得

**パラメータ**：
```json
{
  "action": "get_status"
}
```

**レスポンス**：
```json
{
  "uptime": 86400,
  "memory": 1024,
  "cpu": 45.2
}
```

---

### 8. PermissionTool

権限の管理と確認。

**名前**：`permission`

**説明**：シリコンビーイングの権限を管理。キュレーター専用。

**アクション**：`query_permission`、`manage_acl`

**パラメータ**（query_permission）：
```json
{
  "action": "query_permission",
  "being_id": "being-uuid",
  "permission_type": "network",
  "resource": "https://api.example.com"
}
```

**権限タイプ**：`network`、`command`、`filesystem`、`function`、`data`

**戻り値**：三状態結果（`ALLOWED`、`DENIED`、`ASK_USER`）、キュレーターステータスと頻度キャッシュ情報付き。

**パラメータ**（manage_acl）：
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow",
  "description": "データディレクトリへのアクセスを許可"
}
```

**権限**：`IsCurator` フラグが必要。

---

### 9. ExecuteCodeTool

セキュリティスキャン付き C# コードのコンパイルと実行。

**名前**：`execute_code`

**説明**：セキュリティスキャン付きで C# コードをコンパイルして実行。キュレーター専用。

**アクション**：`run_script`

**パラメータ**：
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

**詳細**：
- コードは `ScriptExecutor` クラスの `Execute()` メソッドにラップされる
- コンパイル前にセキュリティスキャンを実行
- 設定可能なタイムアウトをサポート（デフォルト：30 秒）
- 失敗時にコンパイルエラーとセキュリティ違反情報を返却

**権限**：`IsCurator` フラグが必要。

---

## データベースツール（DatabaseTool）

**名前**: `database`

**説明**: 構造化データベースのクエリと操作。

**アクション**: `query`、`insert`、`update`、`delete`

**パラメータ**（query）:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

**権限**: 適切なデータベースアクセス権限が必要。

## ログツール（LogTool）

**名前**: `log`

**説明**: 操作履歴と会話履歴のクエリ。

**アクション**: `query_logs`、`query_conversations`

**パラメータ**（query_logs）:
```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-23T23:59:59Z",
  "level": "info"
}
```

**パラメータ**（query_conversations）:
```json
{
  "action": "query_conversations",
  "being_id": "being-uuid",
  "session_id": "session-uuid",
  "limit": 50
}
```

**機能**:
- シリコンビーイングによるログのフィルタリングをサポート
- 時間範囲クエリをサポート
- ログレベルフィルタリングをサポート
- 会話履歴の取得

## DiskTool の強化

DiskTool にローカル検索機能が追加されました（SearchTool から統合）:

**新しいアクション**: `search_files`、`search_content`

**パラメータ**（search_files）:
```json
{
  "action": "search_files",
  "path": "/data",
  "pattern": "*.json",
  "recursive": true
}
```

**パラメータ**（search_content）:
```json
{
  "action": "search_content",
  "path": "/data",
  "query": "検索語",
  "filePattern": "*.md"
}
```

### 16. ナレッジネットワークツール

**名前**: `knowledge`

**説明**: ナレッジネットワーク操作ツール、ナレッジトリプルの追加、照会、更新、削除、検索用。

**アクション**: `add`、`query`、`update`、`delete`、`search`、`get_path`、`validate`、`stats`

**パラメータ**（add - ナレッジ追加）:
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**パラメータ**（query - ナレッジ照会）:
```json
{
  "action": "query",
  "subject": "Python",
  "predicate": "is_a"
}
```

**パラメータ**（search - ナレッジ検索）:
```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

**パラメータ**（get_path - ナレッジパス取得）:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

**パラメータ**（stats - 統計情報）:
```json
{
  "action": "stats"
}
```

**特徴**:
- トリプル構造（主語-述語-目的語）ベースのナレッジ表現
- ナレッジ信頼度スコア対応
- タグ分類および検索対応
- ナレッジパス発見（2点間の関連パス）対応
- ナレッジ検証および整合性チェック対応
- ファイルシステムに永続保存

**権限**: すべての生命体が使用可能。

### 17. 作業ノートツール

**名前**: `work_note`

**説明**: シリコン生命体の作業ノート管理。作業ノートはページベース設計で、個人日記に類似（デフォルトで非公開）。

**アクション**: `create`、`read`、`update`、`delete`、`list`、`directory`、`search`

**パラメータ**（create - ノート作成）:
```json
{
  "action": "create",
  "summary": "ユーザー認証モジュール完了",
  "content": "## 実装詳細\n\n- JWT token使用\n- OAuth2対応\n- refresh tokenメカニズム追加",
  "keywords": "認証,JWT,OAuth2"
}
```

**パラメータ**（read - ノート読み取り）:
```json
{
  "action": "read",
  "page_number": 1
}
```

またはnote_idを使用:
```json
{
  "action": "read",
  "note_id": "550e8400-e29b-41d4-a716-446655440000"
}
```

**パラメータ**（update - ノート更新）:
```json
{
  "action": "update",
  "page_number": 1,
  "content": "## 更新後の内容\n\nユニットテスト追加",
  "summary": "ユーザー認証モジュールおよびテスト完了"
}
```

**パラメータ**（list - 全ノート一覧）:
```json
{
  "action": "list"
}
```

**パラメータ**（directory - ノート目次生成）:
```json
{
  "action": "directory"
}
```

**パラメータ**（search - ノート検索）:
```json
{
  "action": "search",
  "keyword": "認証",
  "max_results": 10
}
```

**特徴**:
- ページベース設計、各ページ独立管理
- 要約、内容、キーワード対応
- キーワード別検索対応
- 目次概要生成対応（コンテキスト理解用）
- Markdown形式対応（テキスト、リスト、表、コードブロック）
- 自動タイムスタンプ記録
- デフォルトで非公開、生命体自身のみアクセス可能

**権限**: 生命体は自分の作業ノートにアクセス、キュレーターは全ノート管理可能。

---

## カスタムツールの作成

### ステップ 1: ITool を実装

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "このツールの機能の説明";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, ToolParameter>
        {
            ["param1"] = new ToolParameter
            {
                Type = "string",
                Description = "最初のパラメータ",
                Required = true
            }
        }
    };
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        try
        {
            var param1 = call.Parameters["param1"]?.ToString();
            
            // ロジックを実行
            var result = await DoSomething(param1);
            
            return new ToolResult
            {
                Success = true,
                Output = result
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}
```

### ステップ 2: ツールを配置

`src/SiliconLife.Default/Tools/` にファイルを配置

### ステップ 3: 自動的に検出

ツールはアプリケーション起動時に自動的に検出および登録されます - 手動設定は不要！

---

## 管理者専用ツール

一部のツールは管理者/キュレーター専用です：

```csharp
[SiliconManagerOnly]
public class AdminTool : ITool
{
    // 管理者のみが実行可能
}
```

---

## ツール実行フロー

```
1. AI がツールコールを返す
   ↓
2. システムがツール名を検索
   ↓
3. パラメータを検証
   ↓
4. 権限チェーンを実行
   ↓
5. ツールを実行
   ↓
6. 結果を AI に返す
   ↓
7. AI が最終レスポンスを生成
```

---

## ベストプラクティス

### 1. 明確な説明を提供

```csharp
public string Description => "ファイルを安全に削除。確認を求めます。";
```

### 2. パラメータを検証

```csharp
if (!call.Parameters.ContainsKey("filePath"))
{
    return ToolResult.Failure("filePath パラメータが必要");
}
```

### 3. エラーを適切に処理

```csharp
try
{
    // 操作
}
catch (Exception ex)
{
    Logger.Error($"ツール {Name} に失敗：{ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 4. 権限要件を尊重

```csharp
[SiliconManagerOnly]  // 必要に応じて
public class SensitiveTool : ITool { }
```

### 5. 結果を簡潔に保つ

```csharp
return new ToolResult
{
    Success = true,
    Output = result.Substring(0, Math.Min(1000, result.Length))  // 切り詰め
};
```

---

## ツール定義スキーマ

```json
{
  "name": "tool_name",
  "description": "ツールが何をするか",
  "parameters": {
    "param1": {
      "type": "string|number|boolean|object|array",
      "description": "パラメータの説明",
      "required": true,
      "enum": ["option1", "option2"]  // オプション
    }
  }
}
```

---

## トラブルシューティング

### ツールが見つからない

**確認**：
1. ツールが `Tools/` ディレクトリにある
2. `ITool` インターフェースを実装
3. クラスが public である
4. コンパイルエラーがない

### パラメータ検証エラー

**確認**：
1. 必須パラメータがすべて提供されている
2. 型が正しい
3. 列挙値が有効

### 権限拒否

**確認**：
1. ユーザーに必要な権限がある
2. `[SiliconManagerOnly]` ツールに管理者権限が必要でない
3. グローバル ACL が操作を許可

---

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)を確認
- 🚀 [はじめにガイド](getting-started.md)で開始
- 🔧 カスタムツールを作成して拡張
