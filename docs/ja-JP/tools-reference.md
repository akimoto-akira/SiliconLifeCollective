# ツールリファレンス

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
