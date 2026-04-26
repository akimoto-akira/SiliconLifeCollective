# 開発ガイド

[English](../en/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | **日本語** | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md)

## アーキテクチャ概要

SiliconLifeCollective は**身体-大脳アーキテクチャ**に従い、コアインターフェースとデフォルト実装は厳密に分離。

### プロジェクト構造

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # インターフェース、抽象クラス、共通インフラ
│   └── SiliconLife.Default/   # 具体実装、エントリーポイント
└── docs/                      # 多言語ドキュメント
```

**依存方向**：`SiliconLife.Default` → `SiliconLife.Core`（単方向）

## コアコンセプト

### 1. シリコン生命体

各 AI エージェントは以下で構成：
- **身体**（`DefaultSiliconBeing`）：生存状態を維持。トリガーシナリオを検出
- **大脳**（`ContextManager`）：履歴の読み込み、AI 呼び出し、ツール実行、応答の永続化

### 2. ツールシステム

ツールはリフレクションを介して自動的に発見および登録：

```csharp
// すべてのツールは ITool インターフェースを実装
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. 権限システム

5段階権限検証チェーン：
```
IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
```

### 4. サービスロケーター

グローバルサービス登録と検索：
```csharp
// 登録
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// 取得
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## システムの拡張

### 新ツールの追加

1. `src/SiliconLife.Default/Tools/` に新クラスを作成：

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // パラメータを解析
        var param1 = call.Parameters["param1"]?.ToString();
        
        // ロジックを実行
        var result = await DoSomething(param1);
        
        // 結果を返す
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. ツールはリフレクションを介して自動発見 - 手動登録は不要！

3. （オプション）管理者のみ使用としてマーク：
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### 新 AI クライアントの追加

1. `src/SiliconLife.Default/AI/` で `IAIClient` を実装：

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // AI API を呼び出し
        var response = await CallMyAPI(request);
        
        return new AIResponse
        {
            Content = response.Message,
            ToolCalls = response.ToolCalls,
            Usage = response.Usage
        };
    }
    
    public async IAsyncEnumerable<string> StreamChatAsync(AIRequest request)
    {
        // ストリーミングを実装
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. ファクトリーを作成：

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. ファクトリーは自動的に発見および登録。

### 新ストレージバックエンドの追加

1. `src/SiliconLife.Default/Storage/` で `IStorage` と `ITimeStorage` を実装：

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // データベースから読み取り
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // データベースに書き込み
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // 時間インデックスクエリ
    }
}
```

### 新スキンの追加

1. `src/SiliconLife.Default/Web/Skins/` で `ISkin` を実装：

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "A custom skin description";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
            }
            /* Your custom styles */
        ";
    }
}
```

2. スキンは `SkinManager` によって自動発見。

## コードスタイルガイド

### 命名規則

- **クラス**：PascalCase。機能プレフィックス付き（例：`DefaultSiliconBeing`）
- **インターフェース**：`I` で開始（例：`IAIClient`、`ITool`）
- **メソッド**：PascalCase（例：`ExecuteAsync`）
- **プロパティ**：PascalCase（例：`BaseUrl`）
- **パラメータ**：camelCase（例：`request`）
- **プライベートフィールド**：`_` プレフィックス + camelCase（例：`_client`）

### ドキュメントコメント

すべての public/protected メンバーに XML ドキュメントコメント：

```csharp
/// <summary>
/// AI クライアントとのチャットを実行。
/// </summary>
/// <param name="request">AI リクエスト</param>
/// <returns>AI 応答</returns>
public async Task<AIResponse> ChatAsync(AIRequest request)
{
    // 実装
}
```

### エラー処理

- 非同期操作には `async/await` を使用
- 具体的な例外タイプをスロー
- 意味のあるエラーメッセージを提供
- 上位レベルで例外を適切にキャッチ

```csharp
public async Task<string> ReadFileAsync(string path)
{
    if (string.IsNullOrEmpty(path))
    {
        throw new ArgumentException("パスを指定してください", nameof(path));
    }
    
    try
    {
        return await File.ReadAllTextAsync(path);
    }
    catch (FileNotFoundException ex)
    {
        throw new InvalidOperationException($"ファイルが見つかりません: {path}", ex);
    }
}
```

## テスト

### 単体テスト

xUnit を使用して単体テストを作成：

```csharp
public class CalendarToolTests
{
    [Fact]
    public async Task Convert_ShouldReturnCorrectDate()
    {
        // 配置
        var tool = new CalendarTool();
        
        // 実行
        var result = await tool.ExecuteAsync(new ToolCall
        {
            Parameters = new Dictionary<string, object>
            {
                ["action"] = "convert",
                ["date"] = "2026-04-20",
                ["from_calendar"] = "gregorian",
                ["to_calendar"] = "chinese_lunar"
            }
        });
        
        // 検証
        Assert.True(result.Success);
        Assert.Contains("旧暦", result.Output);
    }
}
```

### テストの実行

```bash
dotnet test
```

## デバッグ

### ログ

組み込みログシステムを使用：

```csharp
var logger = ServiceLocator.Instance.Get<ILogger>();
logger.Info("Operation started");
logger.Warning("Something might be wrong");
logger.Error("Operation failed", exception);
```

### デバッガー

Visual Studio または VS Code でデバッグ：

1. ブレークポイントを設定
2. F5 でデバッグ開始
3. 変数を検査
4. ステップ実行

## パフォーマンス

### 最適化のヒント

1. **非同期 I/O**：すべての I/O 操作に `async/await` を使用
2. **キャッシュ**：頻繁にアクセスされるデータをキャッシュ
3. **バッチ処理**：複数の操作をバッチ
4. **メモリ管理**：大きなオブジェクトを適切に破棄

### プロファイリング

```bash
# .NET プロファイラーを使用
dotnet trace collect --process-id <PID>
dotnet counters monitor --process-id <PID>
```

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 📖 [API リファレンス](api-reference.md)を探る
- 🔧 [ツールリファレンス](tools-reference.md)を見る
- 🚀 [クイックスタートガイド](getting-started.md)で始める
