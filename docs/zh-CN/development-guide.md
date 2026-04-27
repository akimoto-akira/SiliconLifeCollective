# 开发指南

> **版本：v0.1.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | **中文** | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md)

## 架构概述

SiliconLifeCollective 遵循**身体-大脑架构**，核心接口和默认实现严格分离。

### 项目结构

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # 接口、抽象类、通用基础设施
│   └── SiliconLife.Default/   # 具体实现、入口点
└── docs/                      # 多语言文档
```

**依赖方向**：`SiliconLife.Default` → `SiliconLife.Core`（单向）

## 核心概念

### 1. 硅基生命体（硅基生命体）

每个 AI 智能体由以下部分组成：
- **身体**（`DefaultSiliconBeing`）：维持存活状态，检测触发场景
- **大脑**（`ContextManager`）：加载历史、调用 AI、执行工具、持久化响应

### 2. 工具系统

工具通过反射自动发现和注册：

```csharp
// 所有工具实现 ITool 接口
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. 权限系统

5 级权限验证链：
```
IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
```

### 4. 服务定位器

全局服务注册和检索：
```csharp
// 注册
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// 获取
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## 扩展系统

### 添加新工具

1. 在 `src/SiliconLife.Default/Tools/` 中创建新类：

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // 解析参数
        var param1 = call.Parameters["param1"]?.ToString();
        
        // 执行逻辑
        var result = await DoSomething(param1);
        
        // 返回结果
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. 工具通过反射自动发现 - 无需手动注册！

3. （可选）标记为仅管理员可用：
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### 添加新 AI 客户端

1. 在 `src/SiliconLife.Default/AI/` 中实现 `IAIClient`：

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
        // 实现流式传输
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. 创建工厂：

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. 工厂自动发现并注册。

### 添加新存储后端

1. 在 `src/SiliconLife.Default/Storage/` 中实现 `IStorage` 和 `ITimeStorage`：

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // 从您的数据库读取
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // 写入您的数据库
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // 时间索引查询
    }
}
```

### 添加新皮肤

1. 在 `src/SiliconLife.Default/Web/Skins/` 中实现 `ISkin`：

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

2. 皮肤由 `SkinManager` 自动发现。

## 代码风格指南

### 命名约定

- **类**：PascalCase，带功能前缀（例如 `DefaultSiliconBeing`）
- **接口**：以 `I` 开头（例如 `IAIClient`、`ITool`）
- **实现**：以接口名结尾（例如 `OllamaClient` 实现 `IAIClient`）
- **工具**：以 `Tool` 结尾（例如 `CalendarTool`、`ChatTool`）
- **视图模型**：以 `ViewModel` 结尾（例如 `BeingViewModel`）

### 代码组织

```
SiliconLife.Default/
├── AI/                    # AI 客户端实现
├── Calendar/              # 日历实现
├── Config/                # 默认配置数据
├── Executors/             # 执行器实现
├── IM/                    # 即时通讯提供者实现
├── Localization/          # 本地化实现
├── Logging/               # 日志提供者实现
├── Runtime/               # 运行时组件
├── Security/              # 安全实现
├── SiliconBeing/          # 默认硅基生命体实现
├── Storage/               # 存储实现
├── Tools/                 # 内置工具
└── Web/                   # Web UI 实现
    ├── Controllers/       # 路由控制器
    ├── Models/            # 视图模型
    ├── Views/             # HTML 视图
    └── Skins/             # 皮肤主题
```

### 文档

- 所有公共 API 必须有 XML 文档注释
- 所有源文件使用 Apache 2.0 许可证头
- 利用 .NET 9 特性（隐式 using、可空引用类型）

## 开发工作流程

### 1. 设置开发环境

```bash
# 克隆仓库
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# 恢复依赖
dotnet restore

# 构建
dotnet build
```

### 2. 运行测试

```bash
# 运行所有测试
dotnet test

# 运行特定测试项目
dotnet test tests/SiliconLife.Core.Tests
```

### 3. 调试

```bash
# 以调试输出运行
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. 代码格式化

```bash
# 格式化代码
dotnet format
```

## 构建自定义功能

### 示例：添加自定义日历

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

### 示例：添加自定义执行器

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        // 首先验证权限
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        // 执行操作
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

## 测试指南

### 单元测试

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
        
        // 执行
        var result = await tool.ExecuteAsync(call);
        
        // 断言
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### 集成测试

测试完整流程：
1. AI 返回工具调用
2. 工具执行
3. 结果反馈给 AI
4. AI 返回最终响应

## 性能考虑

### 存储系统

- 存储系统优先考虑**功能而非性能**
- 默认使用基于文件的 JSON 存储
- 时间索引查询使用目录结构

### 主循环调度器

- 基于时钟的时切片公平调度
- 看门狗定时器用于检测卡死操作
- 熔断器用于防止级联失败

## 最佳实践

### 1. 始终验证权限

任何 AI 发起的操作必须通过权限链：

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
}
```

### 2. 使用服务定位器

全局注册和检索服务：

```csharp
// 初始化期间
ServiceLocator.Instance.Register<ICustomService>(myService);

// 需要时
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. 遵循身体-大脑分离

- 身体处理状态和触发
- 大脑处理 AI 交互和工具执行

### 4. 实现适当的错误处理

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

## 贡献指南

1. Fork 仓库
2. 创建功能分支（`git checkout -b feature/amazing-feature`）
3. 使用约定式提交提交您的更改
4. 推送到分支（`git push origin feature/amazing-feature`）
5. 打开拉取请求

### 提交消息格式

```
<type>(<scope>): <description>

示例：
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## 下一步

- 📚 阅读[架构指南](architecture.md)
- 📖 探索[API 参考](api-reference.md)
- 🔒 查看[安全文档](security.md)
- 🚀 查看[快速开始指南](getting-started.md)
