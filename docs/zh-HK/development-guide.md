# 開發指南

> **版本：v0.2.0-alpha**

[English](../en/development-guide.md) | [中文](../zh-CN/development-guide.md) | **繁體中文** | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/security.md)

## 架構概述

SiliconLifeCollective 遵循**身體-大腦架構**，核心介面和預設實作嚴格分離。

### 專案結構

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # 介面、抽象類別、通用基礎設施
│   ├── SiliconLife.Common/          # 共享實作（兩個版本共用）
│   ├── SiliconLife.Default/         # 預設實作、入口點（驗證架構可行性）
│   ├── SiliconLife.Fast/            # 高效能實作、入口點（主推生產版本）
│   ├── SiliconLife.Speedy/          # SpeedyPack 高效能儲存引擎
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack 管理工具（Avalonia UI）
└── docs/                            # 多語言文檔
```

**依賴方向**：
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core`（單向）

**版本角色說明**：
- **SiliconLife.Default**：預設實作，主要用於驗證架構可行性。提供簡單可靠的檔案系統儲存實作，適合開發除錯和架構驗證。
- **SiliconLife.Fast**：主推生產版本。在 Default 驗證的架構基礎上，採用 SpeedyPack 記憶體儲存 + 非同步持久化，提供極致效能最佳化，是長期執行和實際生產環境的首選。

## 核心概念

### 1. 矽基生命體

每個 AI 智慧體由以下部分組成：
- **身體**（`DefaultSiliconBeing`）：維持存活狀態，偵測觸發場景
- **大腦**（`ContextManager`）：載入歷史、呼叫 AI、執行工具、持久化回應

### 2. 工具系統

工具透過反射自動發現和註冊：

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. 權限系統

3 級權限驗證鏈：
```
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL)
```

### 4. 服務定位器

全域服務註冊和檢索：
```csharp
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

var client = ServiceLocator.Instance.Get<IAIClient>();
```

## 擴充系統

### 新增新工具

1. 在 `src/SiliconLife.Common/Tools/` 中建立新類別（兩個版本共享的工具）：

> **注意**：`SiliconLife.Default` 和 `SiliconLife.Fast` 不再有獨立的 `Tools/` 目錄，所有共享工具統一放在 `SiliconLife.Common/Tools/` 中。

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        var param1 = call.Parameters["param1"]?.ToString();
        
        var result = await DoSomething(param1);
        
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. 工具透過反射自動發現 - 無需手動註冊！

3. （可選）標記為僅管理員可用：
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### 新增新 AI 客戶端

1. 在 `src/SiliconLife.Common/AI/` 中實作 `IAIClient`：

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
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
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. 建立工廠：

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. 工廠自動發現並註冊。

### 新增新儲存後端

1. 在 `src/SiliconLife.Default/Storage/`（檔案系統實作）或 `src/SiliconLife.Fast/Storage/`（SpeedyPack 配接器）中實作 `IStorage` 和 `ITimeStorage`：

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
    }
    
    public async Task WriteAsync(string key, string value)
    {
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
    }
}
```

### 新增新外掛程式

1. 建立一個類別庫專案，實作 `IPlugin` 介面：

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

2. （可選）在外掛程式中實作 `ITool` 介面以註冊自訂工具：

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

3. 將編譯後的 DLL 放入外掛程式目錄，`PluginLoader` 將自動載入。

> **安全限制**：外掛程式不能引用 `System.IO`、`System.Net.Http`、`System.Net.WebSockets`、`System.Net.Sockets`、`Microsoft.CodeAnalysis` 等命名空間。外掛程式透過 `AssemblyLoadContext` 隔離載入。

### 新增新皮膚

1. 在 `src/SiliconLife.App/Web/Skins/` 中實作 `ISkin`：

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
        ";
    }
}
```

2. 皮膚由 `SkinManager` 自動發現。

## 程式碼風格指南

### 命名約定

- **類別**：PascalCase，帶功能前綴（例如 `DefaultSiliconBeing`）
- **介面**：以 `I` 開頭（例如 `IAIClient`、`ITool`）
- **實作**：以介面名結尾（例如 `OllamaClient` 實作 `IAIClient`）
- **工具**：以 `Tool` 結尾（例如 `CalendarTool`、`ChatTool`）
- **視圖模型**：以 `ViewModel` 結尾（例如 `BeingViewModel`）

### 程式碼組織

```
SiliconLife.Common/
├── AI/                    # AI 客戶端與工廠實作
├── Calendar/              # 32 種日曆實作
├── Localization/          # 本地化基類與 33 種語言變體實作
├── Security/              # 權限管理器
├── SiliconBeing/          # 預設矽基生命體實作
├── Tools/                 # 共享的內建工具
├── Web/                   # Web 基礎設施
└── WebView/               # Playwright WebView 實作

SiliconLife.App/          # Default 與 Fast 共享的應用層
├── Config/                # 應用配置
├── Help/                  # 幫助文檔本地化
└── Web/                   # Web UI 實作
    ├── Component/         # UI 組件庫
    ├── Controllers/       # 路由控制器
    ├── Models/            # 視圖模型
    ├── Views/             # HTML 視圖
    └── Skins/             # 皮膚主題

SiliconLife.Default/      # 版本特有目錄
├── Config/                # 預設配置資料
├── Knowledge/             # 知識網絡實作
├── Logging/               # 日誌提供者實作（主控台 + 檔案系統）
├── Project/               # 專案系統實作
└── Storage/               # 檔案系統儲存實作
```

### 文檔

- 所有公共 API 必須有 XML 文檔註釋
- 所有原始檔使用 Apache 2.0 授權標頭
- 利用 .NET 9 特性（隱式 using、可空引用類型）

## 開發工作流程

### 1. 設定開發環境

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

dotnet restore

dotnet build
```

### 2. 執行測試

```bash
dotnet test

dotnet test tests/SiliconLife.Core.Tests
```

### 3. 除錯

```bash
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. 程式碼格式化

```bash
dotnet format
```

## 建置自訂功能

### 範例：新增自訂日曆

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        return new GregorianDate(year, month, day);
    }
}
```

### 範例：新增自訂執行器

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

## 測試指南

### 單元測試

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object> 
            { 
                ["param1"] = "test" 
            }
        };
        
        var result = await tool.ExecuteAsync(call);
        
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### 整合測試

測試完整流程：
1. AI 回傳工具呼叫
2. 工具執行
3. 結果回饋給 AI
4. AI 回傳最終回應

## 效能考量

### 儲存系統

- Default 版本使用基於檔案的 JSON 儲存
- Fast 版本使用 SpeedyPack 記憶體儲存引擎（.spk 格式）
- SpeedyPack 採用記憶體目錄映射 + 條目快取 + 非同步寫入佇列
- 時間索引查詢使用 `ITimeStorage` 介面

### 主循環排程器

- 基於時鐘的時切片公平排程
- 看門狗計時器用於偵測卡死操作
- 熔斷器用於防止級聯失敗

## 最佳實踐

### 1. 始終驗證權限

任何 AI 發起的操作必須透過權限鏈：

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
}
```

### 2. 使用服務定位器

全域註冊和檢索服務：

```csharp
ServiceLocator.Instance.Register<ICustomService>(myService);

var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. 遵循身體-大腦分離

- 身體處理狀態和觸發
- 大腦處理 AI 互動和工具執行

### 4. 實作適當的錯誤處理

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

## 貢獻指南

1. Fork 儲存庫
2. 建立功能分支（`git checkout -b feature/amazing-feature`）
3. 使用約定式提交提交您的變更
4. 推送到分支（`git push origin feature/amazing-feature`）
5. 開啟拉取請求

### 提交訊息格式

```
<type>(<scope>): <description>

範例：
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## 下一步

- 📚 閱讀[架構指南](architecture.md)
- 📖 探索[API 參考](api-reference.md)
- 🔒 查看[安全文檔](security.md)
- 🚀 查看[快速開始指南](getting-started.md)
