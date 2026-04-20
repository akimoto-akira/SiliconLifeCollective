# 開發指南

## 架構概述

SiliconLifeCollective 遵循**身体-大脑架構**，核心介面和默認實現嚴格分离。

### 項目結構

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # 介面、抽象類別、通用基础設施
│   └── SiliconLife.Default/   # 具体實現、入口点
└── docs/                      # 多语言文檔
```

**依赖方向**：`SiliconLife.Default` → `SiliconLife.Core`（单向）

## 核心概念

### 1. 硅基生命体（硅基生命体）

每個 AI 智能体由以下部分群組成：
- **身体**（`DefaultSiliconBeing`）：维持存活狀態，检测触發場景
- **大脑**（`ContextManager`）：載入歷史、调用 AI、執行工具、持久化回應

### 2. 工具系統

工具通過反射自動發現和註冊：

```csharp
// 所有工具實現 ITool 介面
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. 權限系統

5 級權限驗證链：
```
IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
```

### 4. 服務定位器

全局服務註冊和检索：
```csharp
// 註冊
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// 获取
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## 擴充系統

### 添加新工具

1. 在 `src/SiliconLife.Default/Tools/` 中建立新類別：

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // 解析參數
        var param1 = call.Parameters["param1"]?.ToString();
        
        // 執行逻辑
        var result = await DoSomething(param1);
        
        // 返回結果
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. 工具通過反射自動發現 - 无需手動註冊！

3. （可选）标記為僅管理員可用：
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### 添加新 AI 客戶端

1. 在 `src/SiliconLife.Default/AI/` 中實現 `IAIClient`：

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // 调用您的 AI API
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
        // 實現流式傳输
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

### 添加新儲存後端

1. 在 `src/SiliconLife.Default/Storage/` 中實現 `IStorage` 和 `ITimeStorage`：

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // 從您的資料庫读取
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

### 添加新皮肤

1. 在 `src/SiliconLife.Default/Web/Skins/` 中實現 `ISkin`：

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

2. 皮肤由 `SkinManager` 自動發現。

## 程式碼風格指南

### 命名約定

- **類別**：PascalCase，带功能前缀（例如 `DefaultSiliconBeing`）
- **介面**：以 `I` 開頭（例如 `IAIClient`、`ITool`）
- **實現**：以介面名结尾（例如 `OllamaClient` 實現 `IAIClient`）
- **工具**：以 `Tool` 结尾（例如 `CalendarTool`、`ChatTool`）
- **檢視模型**：以 `ViewModel` 结尾（例如 `BeingViewModel`）

### 程式碼組織

```
SiliconLife.Default/
├── AI/                    # AI 客戶端實現
├── Calendar/              # 日歷實現
├── Config/                # 默認設定資料
├── Executors/             # 執行器實現
├── IM/                    # 即时通訊提供者實現
├── Localization/          # 在地化實現
├── Logging/               # 記錄提供者實現
├── Runtime/               # 執行时元件
├── Security/              # 安全實現
├── SiliconBeing/          # 默認硅基生命体實現
├── Storage/               # 儲存實現
├── Tools/                 # 內置工具
└── Web/                   # Web UI 實現
    ├── Controllers/       # 路由控制器
    ├── Models/            # 檢視模型
    ├── Views/             # HTML 檢視
    └── Skins/             # 皮肤主題
```

### 文檔

- 所有公共 API 必須有 XML 文檔註解
- 所有源檔案使用 Apache 2.0 授權證頭
- 利用 .NET 9 特性（隐式 using、可空引用類型）

## 開發工作流程

### 1. 設定開發环境

```bash
# 克隆倉程式庫
git clone https://github.com/your-org/SiliconLifeCollective.git
cd SiliconLifeCollective

# 復原依赖
dotnet restore

# 构建
dotnet build
```

### 2. 執行測試

```bash
# 執行所有測試
dotnet test

# 執行特定測試項目
dotnet test tests/SiliconLife.Core.Tests
```

### 3. 偵錯

```bash
# 以偵錯输出執行
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. 程式碼格式化

```bash
# 格式化程式碼
dotnet format
```

## 构建自定義功能

### 示例：添加自定義日歷

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // 您的转换逻辑
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // 反向转换
        return new GregorianDate(year, month, day);
    }
}
```

### 示例：添加自定義執行器

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        // 首先驗證權限
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        // 執行操作
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

## 測試指南

### 单元測試

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
        
        // 断言
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### 集成測試

測試完整流程：
1. AI 返回工具调用
2. 工具執行
3. 結果回饋给 AI
4. AI 返回最终回應

## 效能考虑

### 儲存系統

- 儲存系統優先考虑**功能而非效能**
- 默認使用基於檔案的 JSON 儲存
- 時間索引查詢使用目錄結構

### 主循环排程器

- 基於时钟的时切片公平排程
- 看門狗定时器用於检测卡死操作
- 熔断器用於防止級联失敗

## 最佳实践

### 1. 始终驗證權限

任何 AI 發起的操作必須通過權限链：

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
}
```

### 2. 使用服務定位器

全局註冊和检索服務：

```csharp
// 初始化期间
ServiceLocator.Instance.Register<ICustomService>(myService);

// 需要时
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. 遵循身体-大脑分离

- 身体處理狀態和触發
- 大脑處理 AI 交互和工具執行

### 4. 實現适當的錯誤處理

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

## 貢献指南

1. Fork 倉程式庫
2. 建立功能分支（`git checkout -b feature/amazing-feature`）
3. 使用約定式提交提交您的更改
4. 推送到分支（`git push origin feature/amazing-feature`）
5. 打開拉取要求

### 提交訊息格式

```
<type>(<scope>): <description>

示例：
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## 下一步

- 📚 阅读[架構指南](architecture.md)
- 📖 探索[API 参考](api-reference.md)
- 🔒 查看[安全文檔](security.md)
- 🚀 查看[快速開始指南](getting-started.md)
