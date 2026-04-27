# 開發指南

> **版本：v0.1.0-alpha**

[English](../en/development-guide.md) | [中文](../zh-CN/development-guide.md) | **繁體中文** | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md)

## 架構概述

Silicon Life Collective 採用分層架構設計：

```
┌─────────────────────────────────────────┐
│   SiliconLife.Default (實現 + 入口點)    │
├─────────────────────────────────────────┤
│   SiliconLife.Core (介面 + 抽象類別)     │
└─────────────────────────────────────────┘
```

### 專案結構

- **SiliconLife.Core** - 核心介面、抽象類別和通用基礎設施
- **SiliconLife.Default** - 具體實現、業務邏輯和應用程式入口點

依賴方向：Default → Core（單向依賴）

---

## 擴充系統

### 建立自訂工具

工具是矽基生命體與外部世界互動的主要方式。

#### 步驟 1: 實現 ITool 介面

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "工具描述";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "參數說明" }
        }
    };
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        try
        {
            var param1 = call.Parameters["param1"]?.ToString();
            var result = await DoWork(param1);
            
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

#### 步驟 2: 添加到專案

將工具檔案放置在 `src/SiliconLife.Default/Tools/` 目錄中。`ToolManager` 會在啟動時通過反射自動發現並註冊。

#### 步驟 3: （可選）標記為主理人專用

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // 僅矽基主理人可存取
}
```

### 建立自訂執行器

執行器管理 I/O 操作並強制執行權限檢查。

#### 步驟 1: 繼承 ExecutorBase

```csharp
public class MyCustomExecutor : ExecutorBase
{
    public MyCustomExecutor(PermissionManager permissionManager) 
        : base(permissionManager)
    {
    }
    
    public async Task<ExecutorResult> ExecuteMyOperation(ExecutorRequest request)
    {
        // 1. 建立權限請求
        var permRequest = new PermissionRequest
        {
            PermissionType = PermissionType.CustomOperation,
            Resource = request.Resource,
            BeingId = request.BeingId
        };
        
        // 2. 檢查權限
        var permission = await CheckPermissionAsync(permRequest);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        // 3. 執行操作
        var result = await PerformOperation(request);
        return ExecutorResult.Success(result);
    }
}
```

#### 步驟 2: 註冊執行器

在 `Program.cs` 中註冊您的執行器：

```csharp
var myExecutor = new MyCustomExecutor(permissionManager);
ServiceLocator.Instance.Register<MyCustomExecutor>(myExecutor);
```

### 建立自訂 AI 客戶端

#### 步驟 1: 實現 IAIClient

```csharp
public class MyAIClient : IAIClient
{
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // 實現您的 AI 客戶端邏輯
        var response = await CallMyAI(request);
        
        return new AIResponse
        {
            Content = response.Text,
            ToolCalls = response.ToolCalls,
            PromptTokens = response.Usage.Prompt,
            CompletionTokens = response.Usage.Completion
        };
    }
}
```

#### 步驟 2: 建立客戶端工廠

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        var apiKey = config["apiKey"]?.ToString();
        var model = config["model"]?.ToString();
        
        return new MyAIClient(apiKey, model);
    }
    
    public string GetDisplayName() => "我的 AI 服務";
}
```

#### 步驟 3: 註冊工廠

在配置中添加您的 AI 客戶端類型，系統會自動發現並使用。

---

## 擴充日曆系統

### 建立新日曆

#### 步驟 1: 繼承 CalendarBase

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "My Custom Calendar";
    public override string Id => "my_custom";
    
    public override CalendarDate ConvertFromGregorian(DateTime gregorianDate)
    {
        // 實現從公曆的轉換邏輯
        return new CalendarDate
        {
            Year = CalculateYear(gregorianDate),
            Month = CalculateMonth(gregorianDate),
            Day = CalculateDay(gregorianDate)
        };
    }
    
    public override DateTime ConvertToGregorian(CalendarDate date)
    {
        // 實現轉換為公曆的邏輯
        return CalculateGregorianDate(date);
    }
}
```

#### 步驟 2: 自動註冊

日曆系統通過反射自動發現所有繼承自 `CalendarBase` 的類別，無需手動註冊。

---

## 建立自訂皮膚

### 步驟 1: 實現 ISkin 介面

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "My Custom Skin";
    public string Id => "my_custom";
    
    public string RenderHtml(Controller controller, string viewName, object model)
    {
        // 使用 H 建構器生成 HTML
        return H.Html(
            H.Head(
                H.Link("stylesheet", "/skins/my-custom.css")
            ),
            H.Body(
                RenderContent(controller, viewName, model)
            )
        ).ToString();
    }
    
    public string GenerateCss()
    {
        var css = new CssBuilder();
        css.Add("body", b => b
            .Set("background-color", "#f0f0f0")
            .Set("color", "#333")
        );
        return css.ToString();
    }
}
```

### 步驟 2: 自動發現

`SkinManager` 會通過反射自動發現並註冊所有 `ISkin` 實現。

---

## 本地化開發

### 添加新語言

#### 步驟 1: 建立本地化類別

```csharp
public class MyLanguageLocalization : DefaultLocalizationBase
{
    public override Language Language => Language.MyCustom;
    
    public override string GetString(LocalizationKey key)
    {
        return key switch
        {
            LocalizationKey.Hello => "Hello in My Language",
            LocalizationKey.Goodbye => "Goodbye in My Language",
            // ... 其他字串
            _ => base.GetString(key)
        };
    }
}
```

#### 步驟 2: 添加到 Language 列舉

在 `Language.cs` 中添加新的語言列舉值。

---

## 測試指南

### 單元測試

```csharp
[Test]
public async Task MyTool_ExecuteAsync_ReturnsSuccess()
{
    // Arrange
    var tool = new MyCustomTool();
    var call = new ToolCall
    {
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = "test"
        }
    };
    
    // Act
    var result = await tool.ExecuteAsync(call);
    
    // Assert
    Assert.IsTrue(result.Success);
    Assert.IsNotNull(result.Output);
}
```

### 整合測試

測試完整的工具調用循環：

```csharp
[Test]
public async Task FullToolCycle_WithPermission_Checks()
{
    // 1. 建立測試生命體
    var being = CreateTestBeing();
    
    // 2. 設置權限
    SetupPermissions(being);
    
    // 3. 執行工具
    var result = await ExecuteTool(being, "my_tool", parameters);
    
    // 4. 驗證結果
    Assert.IsTrue(result.Success);
    VerifyAuditLog(being);
}
```

---

## 效能最佳化

### 1. 快取策略

- 使用 `UserFrequencyCache` 快取頻繁的權限決策
- 快取日曆轉換結果
- 避免重複載入靈魂文件

### 2. 非同步操作

- 所有 I/O 操作使用 `async/await`
- 避免阻塞主循環
- 使用 `CancellationToken` 支援取消

### 3. 記憶體管理

- 及時釋放不再使用的物件
- 避免大型物件長期存活
- 使用物件池重用頻繁建立的物件

---

## 除錯技巧

### 啟用詳細日誌

```csharp
LogManager.SetLogLevel(LogLevel.Debug);
```

### 檢查權限審計

```csharp
var auditLogs = ServiceLocator.Instance.AuditLogger.GetLogs(beingId);
foreach (var log in auditLogs)
{
    Console.WriteLine($"[{log.Timestamp}] {log.PermissionType}: {log.Result}");
}
```

### 監控效能

```csharp
var monitor = ServiceLocator.Instance.PerformanceMonitor;
var stats = monitor.GetStats();
Console.WriteLine($"Average Tick Time: {stats.AverageTickTime}ms");
```

---

## 貢獻指南

1. Fork 儲存庫
2. 建立功能分支
3. 編寫測試
4. 確保所有測試通過
5. 提交 Pull Request

詳細資訊請參閱 [貢獻指南](contributing.md)。
