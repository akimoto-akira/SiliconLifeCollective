# 開發指南

> **版本：v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | **繁體中文** | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | [Русский](../ru-RU/development-guide.md)

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
└── docs/                            # 多語言文件
```

**依賴方向**：
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core`（單向）

**版本角色說明**：
- **SiliconLife.Default**：預設實作，主要用於驗證架構可行性。提供簡單可靠的檔案系統儲存實作，適合開發除錯和架構驗證。
- **SiliconLife.Fast**：主推生產版本。在 Default 驗證的架構基礎上，採用 SpeedyPack 記憶體儲存 + 非同步持久化，提供極致效能最佳化，是長期執行和實際生產環境的首選。

## 核心概念

### 1. 矽基生命體（矽基生命體）

每個 AI 智慧體由以下部分組成：
- **身體**（`DefaultSiliconBeing`）：維持存活狀態，檢測觸發場景
- **大腦**（`ContextManager`）：載入歷史、呼叫 AI、執行工具、持久化回應

### 2. 工具系統

工具透過反射自動發現和註冊：

```csharp
// 所有工具實作 ITool 介面
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
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → 預設拒絕)
```

### 4. 服務定位器

全域服務註冊和檢索：
```csharp
// 註冊
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// 取得
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## 擴展系統

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
        // 解析參數
        var param1 = call.Parameters["param1"]?.ToString();

        // 執行邏輯
        var result = await DoSomething(param1);

        // 傳回結果
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

4. （可選）標記工具可用場景：
```csharp
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class MyTool : ITool { ... }
```

5. （可選）標記為僅聊天場景可用：
```csharp
[ChatOnly]
public class HelpTool : ITool { ... }
```

6. （可選）標記為僅專案場景可用：
```csharp
[ToolScenario(ToolScenarioFlag.Project)]
[SiliconManagerOnly]
public class ProjectWorkTool : ITool { ... }
```

### 新增新 AI 用戶端

1. 在 `src/SiliconLife.Common/AI/` 中實作 `IAIClient`：

```csharp
public class MyAIClient : IAIClient
{
    public string Endpoint => "https://api.example.com";
    public string DefaultModel => "my-model";
    public bool? StreamingMode => null;
    public bool? SupportsToolCalls => true;
    public int? ContextWindowTokens => 8192;
    public bool? SupportsVision => false;
    public bool? SupportsAudio => false;

    public AIResponse Chat(AIRequest request)
    {
        // 呼叫您的 AI API
        var response = CallMyAPI(request);

        return new AIResponse
        {
            Content = response.Message,
            ToolCalls = response.ToolCalls,
            Usage = response.Usage
        };
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

1. 在 `src/SiliconLife.Default/Storage/`（檔案系統實作）或 `src/SiliconLife.Fast/Storage/`（SpeedyPack 適配器）中實作 `IStorage` 和 `ITimeStorage`：

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // 從您的資料庫讀取
    }

    public async Task WriteAsync(string key, string value)
    {
        // 寫入您的資料庫
    }

    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // 時間索引查詢
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

3. （可選）實作 `ISkillProvider` 介面以透過外掛程式提供技能：

```csharp
public class MyPluginSkills : ISkillProvider
{
    public IEnumerable<SkillDefinition> GetSkills()
    {
        yield return new SkillDefinition
        {
            Id = "my_plugin_skill",
            Description = "A skill provided by my plugin",
            SystemPromptTemplate = "請執行任務：{task}",
            // ... 其餘中繼資料
        };
    }
}
```

外掛程式技能在生命體初始化時由 `SkillManager.ScanAllPluginAssemblies()` 自動註冊（來源標記為 `Plugin`，熱重載不會覆寫）。

4. 將編譯後的 DLL 放入外掛程式目錄，`PluginLoader` 將自動載入。

> **安全限制**：外掛程式預設不能引用 `System.IO`、`System.Net.Http`、`System.Net.WebSockets`、`System.Net.Sockets`、`Microsoft.CodeAnalysis` 等命名空間。但外掛程式可透過 `[PluginCapability]` 屬性宣告所需能力（Network、FileIO、Process、AI），載入器據此放寬對應命名空間的安全掃描規則。不可宣告的能力（P/Invoke、Unsafe、反射發射等）始終被阻止。外掛程式透過 `AssemblyLoadContext` 隔離載入。

### 新增新佈景主題

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
            /* Your custom styles */
        ";
    }
}
```

2. 佈景主題由 `SkinManager` 自動發現。

## 程式碼風格指南

### 命名約定

- **類別**：PascalCase，帶功能前綴（例如 `DefaultSiliconBeing`）
- **介面**：以 `I` 開頭（例如 `IAIClient`、`ITool`）
- **實作**：以介面名結尾（例如 `OllamaClient` 實作 `IAIClient`）
- **工具**：以 `Tool` 結尾（例如 `CalendarTool`、`ChatTool`）
- **檢視模型**：以 `ViewModel` 結尾（例如 `BeingViewModel`）

### 程式碼組織

```
SiliconLife.Common/
├── AI/                    # AI 用戶端與工廠實作（Ollama、DashScope、VolcengineArk、Herdsman、LongCat、QiniuAI、DeepSeek、Zhipu、Ernie、Hunyuan、MiniMax、Moonshot、SiliconFlow）
├── Calendar/              # 32 種日曆實作
├── Localization/          # 在地化基底類別與 34 種語言變體實作
├── Security/              # 權限管理器
├── SiliconBeing/          # 預設矽基生命體實作
├── Tools/                 # 共享的內建工具（25 個）
├── Web/                   # Web 基礎設施
└── WebView/               # Playwright WebView 實作

SiliconLife.App/          # Default 與 Fast 共享的應用層
├── Config/                # 應用程式設定
├── Help/                  # 說明文件在地化
├── Project/               # 專案系統（工作流引擎、專案角色）
└── Web/                   # Web UI 實作
    ├── Component/         # 27 個 UI 元件
    ├── Controllers/       # 27 個路由控制器
    ├── Models/            # 檢視模型
    ├── Views/             # HTML 檢視
    └── Skins/             # 7 個佈景主題

SiliconLife.Default/      # 版本特有目錄
├── Config/                # 預設設定資料
├── Knowledge/             # 知識網絡實作
├── Logging/               # 日誌提供者實作（主控台 + 檔案系統）
├── Project/               # 專案系統實作
└── Storage/               # 檔案系統儲存實作

SiliconLife.Fast/         # 版本特有目錄
├── Config/                # Fast 版本設定資料
├── Logging/               # 日誌提供者實作（主控台 + 檔案系統）
├── Storage/               # SpeedyPack 儲存適配器
└── Tray/                  # 系統匣在地化
```

### 文件

- 所有公共 API 必須有 XML 文件註解
- 所有原始檔案使用 Apache 2.0 授權標頭
- 利用 .NET 9 特性（隱式 using、可空參考類型）

## 開發工作流程

### 1. 設定開發環境

```bash
# 複製儲存庫
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# 還原依賴
dotnet restore

# 建置
dotnet build
```

### 2. 執行測試

```bash
# 執行所有測試
dotnet test

# 執行特定測試專案
dotnet test tests/SiliconLife.Core.Tests
```

### 3. 除錯

```bash
# 以偵錯輸出執行
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. 程式碼格式化

```bash
# 格式化程式碼
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
        // 您的轉換邏輯
        return new CalendarDate(year, month, day);
    }

    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // 反向轉換
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

### 範例：新增自訂工作流範本

```csharp
public class MyWorkflowTemplate : WorkflowTemplate
{
    public override string Name => "my_workflow";
    public override string Description => "A custom workflow template";

    public override void DefineStates()
    {
        AddState("start", "開始", isInitial: true);
        AddState("processing", "處理中");
        AddState("review", "審核");
        AddState("done", "完成", isFinal: true);
    }

    public override void DefineTransitions()
    {
        AddTransition("start", "processing", "開始處理");
        AddTransition("processing", "review", "提交審核");
        AddTransition("review", "done", "審核通過");
        AddTransition("review", "processing", "審核退回");
    }
}
```

### 範例：新增專案角色

專案角色透過 `ProjectTool` 的 `assign_role` 和 `remove_role` 操作管理。角色名稱是自訂字串，用於在工作流和任務分配中區分矽基生命體的職責。

## 測試指南

### 單元測試

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // 安排
        var tool = new MyCustomTool();
        var call = new ToolCall
        {
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object>
            {
                ["param1"] = "test"
            }
        };

        // 執行
        var result = await tool.ExecuteAsync(call);

        // 斷言
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### 整合測試

測試完整流程：
1. AI 傳回工具呼叫
2. 工具執行
3. 結果回饋給 AI
4. AI 傳回最終回應

## 效能考量

### 儲存系統

- Default 版本使用基於檔案的 JSON 儲存
- Fast 版本使用 SpeedyPack 記憶體儲存引擎（.spk 格式）
- SpeedyPack 採用記憶體目錄映射 + 條目快取 + 非同步寫入佇列
- 時間索引查詢使用 `ITimeStorage` 介面

### 主迴圈排程器

- 基於時鐘的時切片公平排程
- 看門狗定時器用於檢測卡死操作
- 熔斷器用於防止級聯失敗

## 最佳實踐

### 1. 始終驗證權限

任何 AI 發起的操作必須透過權限鏈：

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return Result.Denied("Permission denied");
}
```

### 2. 使用服務定位器

全域註冊和檢索服務：

```csharp
// 初始化期間
ServiceLocator.Instance.Register<ICustomService>(myService);

// 需要時
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

示例：
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## 下一步

- 📚 閱讀[架構指南](architecture.md)
- 📖 探索[API 參考](api-reference.md)
- 🔒 檢視[安全文件](security.md)
- 🚀 檢視[快速開始指南](getting-started.md)
