# 開発ガイド

## アーキテクチャ概要

SiliconLifeCollective は、コアインターフェースとデフォルト実装の厳格な分離を伴う**ボディ-ブレインアーキテクチャ**に従います。

### プロジェクト構造

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # インターフェース、抽象クラス、共通インフラ
│   └── SiliconLife.Default/   # 具象実装、エントリポイント
└── docs/                      # 多言語ドキュメント
```

**依存方向**：`SiliconLife.Default` → `SiliconLife.Core`（一方向）

## 核心概念

### 1. シリコンビーイング（硅基生命体）

各 AI エージェントは以下で構成されます：
- **ボディ**（`DefaultSiliconBeing`）：生存状態を維持し、トリガーシナリオを検出
- **ブレイン**（`ContextManager`）：履歴をロードし、AI を呼び出し、ツールを実行し、レスポンスを永続化

### 2. ツールシステム

ツールはリフレクションを介して自動的に検出および登録されます：

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

5レベルの権限検証チェーン：
```
IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
```

### 4. サービスロケーター

グローバルサービス登録および取得：
```csharp
// 登録
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// 取得
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## システムの拡張

### 新しいツールを追加

1. `src/SiliconLife.Default/Tools/` に新しいクラスを作成：

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "このツールの機能の説明";
    
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

2. ツールはリフレクションを介して自動的に検出されます - 手動登録は不要！

3. （オプション）マネージャー専用としてマーク：
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### 新しい AI クライアントを追加

1. `src/SiliconLife.Default/AI/` に `IAIClient` を実装：

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // AI API をコール
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

2. ファクトリを作成：

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. ファクトリは自動的に検出および登録されます。

### 新しいストレージバックエンドを追加

1. `src/SiliconLife.Default/Storage/` に `IStorage` および `ITimeStorage` を実装：

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
        // 時刻インデックス付きクエリ
    }
}
```

### 新しいスキンを追加

1. `src/SiliconLife.Default/Web/Skins/` に `ISkin` を実装：

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "カスタムスキンの説明";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
            }
            /* カスタムスタイル */
        ";
    }
}
```

2. スキンは `SkinManager` によって自動的に検出されます。

## コードスタイルガイドライン

### 命名規則

- **クラス**：機能プレフィックス付きの PascalCase（例：`DefaultSiliconBeing`）
- **インターフェース**：`I` で始まる（例：`IAIClient`、`ITool`）
- **実装**：インターフェース名で終わる（例：`OllamaClient` は `IAIClient` を実装）
- **ツール**：`Tool` で終わる（例：`CalendarTool`、`ChatTool`）
- **ViewModel**：`ViewModel` で終わる（例：`BeingViewModel`）

### コード構成

```
SiliconLife.Default/
├── AI/                    # AI クライアント実装
├── Calendar/              # カレンダー実装
├── Config/                # デフォルト設定データ
├── Executors/             # エグゼキューター実装
├── IM/                    # IM プロバイダー実装
├── Localization/          # ローカライゼーション実装
├── Logging/               # ログプロバイダー実装
├── Runtime/               # ランタイムコンポーネント
├── Security/              # セキュリティ実装
├── SiliconBeing/          # デフォルトシリコンビーイング実装
├── Storage/               # ストレージ実装
├── Tools/                 # 組み込みツール
└── Web/                   # Web UI 実装
    ├── Controllers/       # ルートコントローラー
    ├── Models/            # ビューモデル
    ├── Views/             # HTML ビュー
    └── Skins/             # スキンテーマ
```

### ドキュメント

- すべてのパブリック API に XML ドキュメントコメントが必要
- すべてのソースファイルに Apache 2.0 ライセンスヘッダーを使用
- .NET 9 機能を活用（暗黙的 using、null 許容参照型）

## 開発ワークフロー

### 1. 開発環境の設定

```bash
# リポジトリのクローン
git clone https://github.com/your-org/SiliconLifeCollective.git
cd SiliconLifeCollective

# 依存関係の復元
dotnet restore

# ビルド
dotnet build
```

### 2. テストの実行

```bash
# すべてのテストを実行
dotnet test

# 特定のテストプロジェクトを実行
dotnet test tests/SiliconLife.Core.Tests
```

### 3. デバッグ

```bash
# デバッグ出力で実行
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. コードフォーマット

```bash
# コードをフォーマット
dotnet format
```

## カスタム機能の構築

### 例：カスタムカレンダーの追加

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // 変換ロジック
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // 逆変換
        return new GregorianDate(year, month, day);
    }
}
```

### 例：カスタムエグゼキューターの追加

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        // まず権限を検証
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        // 操作を実行
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

## テストガイドライン

### ユニットテスト

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // 手配
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object> 
            { 
                ["param1"] = "test" 
            }
        };
        
        // 実行
        var result = await tool.ExecuteAsync(call);
        
        // 検証
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### 統合テスト

完全なフローをテスト：
1. AI がツールコールを返す
2. ツールが実行
3. 結果が AI にフィードバック
4. AI が最終レスポンスを返す

## パフォーマンスの考慮事項

### ストレージシステム

- ストレージシステムは**パフォーマンスよりも機能**を優先
- デフォルトでファイルベースの JSON ストレージを使用
- 時刻インデックス付きクエリはディレクトリ構造を使用

### MainLoop スケジューラー

- 公平スケジューリングのためのティックベースのタイムスライス
- 停止した操作を検出するためのウォッチドッグタイマー
- カスケード障害を防止するためのサーキットブレーカー

## ベストプラクティス

### 1. 常に権限を検証

AI が開始した操作は必ず権限チェーンを通過する必要があります：

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
}
```

### 2. サービスロケーターを使用

サービスをグローバルに登録および取得：

```csharp
// 初期化時
ServiceLocator.Instance.Register<ICustomService>(myService);

// 必要時
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. ボディ-ブレイン分離に従う

- ボディは状態とトリガーを処理
- ブレインは AI インタラクションとツール実行を処理

### 4. 適切なエラーハンドリングを実装

```csharp
try
{
    var result = await operation();
    return Result.Success(result);
}
catch (Exception ex)
{
    Logger.Error($"操作に失敗しました：{ex.Message}");
    return Result.Failure(ex.Message);
}
```

## 貢献ガイドライン

1. リポジトリをフォーク
2. 機能ブランチを作成（`git checkout -b feature/amazing-feature`）
3. 慣例的コミットを使用して変更をコミット
4. ブランチにプッシュ（`git push origin feature/amazing-feature`）
5. プルリクエストを開く

### コミットメッセージ形式

```
<type>(<scope>): <description>

例：
feat(tool): カスタムカレンダーツールを追加
fix(permission): コールバックのヌルポインタを修正
docs: 開発ガイドを更新
```

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 📖 [API リファレンス](api-reference.md)を探索
- 🔒 [セキュリティドキュメント](security.md)を確認
- 🚀 [はじめにガイド](getting-started.md)を確認
