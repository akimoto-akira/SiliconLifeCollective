# 開発ガイド

> **バージョン: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | **日本語** | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md)

## アーキテクチャ概要

SiliconLifeCollective は**身体-大脳アーキテクチャ**に従い、コアインターフェースとデフォルト実装は厳密に分離。

### プロジェクト構造

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # インターフェース、抽象クラス、共通インフラ
│   ├── SiliconLife.Common/          # 共有実装（両バージョンで共用）
│   ├── SiliconLife.Default/         # デフォルト実装、エントリーポイント（アーキテクチャ実現可能性検証）
│   ├── SiliconLife.Fast/            # 高性能実装、エントリーポイント（主力本番バージョン）
│   ├── SiliconLife.Speedy/          # SpeedyPack 高性能ストレージエンジン
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack 管理ツール（Avalonia UI）
└── docs/                            # 多言語ドキュメント
```

**依存方向**：
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core`（単方向）

**バージョン役割説明**：
- **SiliconLife.Default**：デフォルト実装、アーキテクチャの実現可能性検証に主に使用。シンプルで信頼性の高いファイルシステムストレージ実装を提供し、開発デバッグとアーキテクチャ検証に適しています。
- **SiliconLife.Fast**：主力本番バージョン。Default で検証されたアーキテクチャの基盤上に、SpeedyPack メモリストレージ + 非同期永続化を採用し、極限のパフォーマンス最適化を提供します。長期運用と実際の本番環境の第一選択です。

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
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | 非主理人: GlobalACL)
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

1. `src/SiliconLife.Common/Tools/` に新クラスを作成（2つのバージョンで共有するツール）：

> **注意**：`SiliconLife.Default` と `SiliconLife.Fast` には独立した `Tools/` ディレクトリはもうありません。すべての共有ツールは `SiliconLife.Common/Tools/` に統一されています。

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

1. `src/SiliconLife.Common/AI/` で `IAIClient` を実装：

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

1. `src/SiliconLife.Default/Storage/`（ファイルシステム実装）または `src/SiliconLife.Fast/Storage/`（SpeedyPack アダプター）で `IStorage` と `ITimeStorage` を実装：

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

### 新プラグインの追加

1. クラスライブラリプロジェクトを作成し、`IPlugin` インターフェースを実装：

```csharp
using SiliconLife.Collective;
using SiliconLife.Collective.Localization;
using SiliconLife.Collective.Tools;

public class MyPlugin : IPlugin
{
    public string Id => "my-plugin";
    public string Version => "1.0.0";
    
    public string GetName(Language language) => "My Plugin";
    public string GetDescription(Language language) => "A custom plugin";
    public string GetAuthor(Language language) => "Author Name";
    
    public void OnLoad() { }
    public void OnStart() { }
    public void OnStop() { }
    public void OnUnload() { }
}
```

2. （オプション）プラグイン内で `ITool` インターフェースを実装してカスタムツールを登録：

```csharp
public class MyPluginTool : ITool
{
    public string Name => "my_plugin_tool";
    public string Description => "A tool provided by my plugin";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        return new ToolResult { Success = true, Output = "Done" };
    }
}
```

3. コンパイル済み DLL をプラグインディレクトリに配置すると、`PluginLoader` が自動的にロードします。

> **セキュリティ制限**：プラグインは `System.IO`、`System.Net.Http`、`System.Net.WebSockets`、`System.Net.Sockets`、`Microsoft.CodeAnalysis` などの名前空間を参照できません。プラグインは `AssemblyLoadContext` を介して分離ロードされます。

### 新スキンの追加

1. `src/SiliconLife.App/Web/Skins/` で `ISkin` を実装：

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
- **実装**：インターフェース名で終了（例：`OllamaClient` は `IAIClient` を実装）
- **ツール**：`Tool` で終了（例：`CalendarTool`、`ChatTool`）
- **ビューモデル**：`ViewModel` で終了（例：`BeingViewModel`）

### コード構織

```
SiliconLife.Common/
├── AI/                    # AI クライアントとファクトリー実装
├── Calendar/              # 32種類のカレンダー実装
├── Localization/          # ローカライゼーションベースクラスと33種類の言語変種実装
├── Security/              # 権限マネージャー
├── SiliconBeing/          # デフォルトシリコン生命体実装
├── Tools/                 # 共有の内蔵ツール
├── Web/                   # Web 基盤
└── WebView/               # Playwright WebView 実装

SiliconLife.App/          # Default と Fast で共有のアプリケーション層
├── Config/                # アプリケーション設定
├── Help/                  # ヘルプドキュメントローカライゼーション
└── Web/                   # Web UI 実装
    ├── Component/         # UI コンポーネントライブラリ
    ├── Controllers/       # ルートコントローラー
    ├── Models/            # ビューモデル
    ├── Views/             # HTML ビュー
    └── Skins/             # スキンテーマ

SiliconLife.Default/      # バージョン固有ディレクトリ
├── Config/                # デフォルト設定データ
├── Knowledge/             # ナレッジネットワーク実装
├── Logging/               # ログプロバイダー実装（コンソール + ファイルシステム）
├── Project/               # プロジェクトシステム実装
└── Storage/               # ファイルシステムストレージ実装
```

### ドキュメント

- すべてのパブリック API には XML ドキュメントコメントが必要
- すべてのソースファイルは Apache 2.0 ライセンスヘッダーを使用
- .NET 9 の機能を活用（暗黙的 using、null 許容参照型）

## 開発ワークフロー

### 1. 開発環境のセットアップ

```bash
# リポジトリをクローン
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# 依存関係を復元
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

## カスタム機能のビルド

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

## テストガイド

### 単体テスト

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // 配置
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
1. AI がツール呼び出しを返す
2. ツールが実行
3. 結果が AI にフィードバック
4. AI が最終応答を返す

## パフォーマンス考慮事項

### ストレージシステム

- Default 版はファイルベースの JSON ストレージを使用
- Fast 版は SpeedyPack メモリストレージエンジンを使用（.spk 形式）
- SpeedyPack はメモリディレクトリマッピング + エントリキャッシュ + 非同期書き込みキューを採用
- 時間インデックスクエリは `ITimeStorage` インターフェースを使用

### メインループスケジューラ

- クロックベースのタイムスライス公平スケジューリング
- ウォッチドッグタイマーでスタック操作を検出
- ヒューズでカスケード障害を防止

## ベストプラクティス

### 1. 常に権限を検証

AI が開始するすべての操作は権限チェーンを通過する必要があります：

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
}
```

### 2. サービスロケーターを使用

グローバルにサービスを登録および検索：

```csharp
// 初期化時
ServiceLocator.Instance.Register<ICustomService>(myService);

// 必要時
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. 身体-大脳分離に従う

- 身体は状態とトリガーを処理
- 大脳は AI インタラクションとツール実行を処理

### 4. 適切なエラー処理を実装

```csharp
try
{
    var result = await operation();
    return Result.Success(result);
}
catch (Exception ex)
{
    Logger.Error($"Operation failed: {ex.Message}");
    return Result.Failure(ex.Message);
}
```

## コントリビューションガイド

1. リポジトリをフォーク
2. フィーチャーブランチを作成（`git checkout -b feature/amazing-feature`）
3. Conventional Commits で変更をコミット
4. ブランチにプッシュ（`git push origin feature/amazing-feature`）
5. プルリクエストを作成

### コミットメッセージ形式

```
<type>(<scope>): <description>

例：
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 📖 [API リファレンス](api-reference.md)を探る
- 🔒 [セキュリティドキュメント](security.md)を見る
- 🚀 [クイックスタートガイド](getting-started.md)で始める
