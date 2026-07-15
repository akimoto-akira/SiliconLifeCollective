# Development Guide

> **Version: v0.2.0-alpha**

[**English**](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | [Русский](../ru-RU/development-guide.md)

## Architecture Overview

SiliconLifeCollective follows a **body-brain architecture** with strict separation between core interfaces and default implementations.

### Project Structure

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces, abstract classes, common infrastructure
│   ├── SiliconLife.Common/          # Shared implementations (used by both versions)
│   ├── SiliconLife.Default/         # Default implementation, entry points (architecture feasibility verification)
│   ├── SiliconLife.Fast/            # High-performance implementation, entry points (main production version)
│   ├── SiliconLife.Speedy/          # SpeedyPack high-performance storage engine
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack management tool (Avalonia UI)
└── docs/                            # Multi-language documentation
```

**Dependency direction**:
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core` (unidirectional)

**Version Role Description**:
- **SiliconLife.Default**: Default implementation, primarily used for architecture feasibility verification. Provides a simple and reliable file system storage implementation, suitable for development debugging and architecture verification.
- **SiliconLife.Fast**: Main production version. Based on the architecture verified by Default, adopts SpeedyPack in-memory storage + asynchronous persistence, providing extreme performance optimization, the first choice for long-term operation and actual production environments.

## Core Concepts

### 1. Silicon Beings (Silicon Being)

Each AI agent consists of:
- **Body** (`DefaultSiliconBeing`): Maintains alive state, detects trigger scenarios
- **Brain** (`Context Manager`): Loads history, calls AI, executes tools, persists responses

### 2. Tool System

Tools are automatically discovered and registered via reflection:

```csharp
// 所有工具实现 ITool 接口
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Permission System

3-level permission verification chain:
```
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → default deny)
```

### 4. Service Locator

Global service registration and retrieval:
```csharp
// 注册
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// 获取
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Extension System

### Adding a New Tool

1. Create a new class in `src/SiliconLife.Common/Tools/` (shared tools for both versions):

> **Note**: `SiliconLife.Default` and `SiliconLife.Fast` no longer have independent `Tools/` directories. All shared tools are placed in `SiliconLife.Common/Tools/`.

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

2. Tools are auto-discovered via reflection - no manual registration needed!

3. (Optional) Mark as manager-only:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

4. (Optional) Mark tool available scenarios:
```csharp
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class MyTool : ITool { ... }
```

5. (Optional) Mark as chat-only:
```csharp
[ChatOnly]
public class HelpTool : ITool { ... }
```

6. (Optional) Mark as project-only:
```csharp
[ToolScenario(ToolScenarioFlag.Project)]
[SiliconManagerOnly]
public class ProjectWorkTool : ITool { ... }
```

### Adding a New AI Client

1. Implement `IAIClient` in `src/SiliconLife.Common/AI/`:

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

2. Create a factory:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. Factory is auto-discovered and registered.

### Adding a New Storage Backend

1. Implement `IStorage` and `ITimeStorage` in `src/SiliconLife.Default/Storage/` (file system implementation) or `src/SiliconLife.Fast/Storage/` (SpeedyPack adapter):

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

### Adding a New Plugin

1. Create a class library project implementing the `IPlugin` interface:

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

2. (Optional) Implement the `ITool` interface in the plugin to register custom tools:

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

3. Place the compiled DLL in the plugins directory, `PluginLoader` will automatically load it.

> **Security Restrictions**: By default, plugins cannot reference `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis` and other namespaces. However, if a plugin declares required capabilities (Network, FileIO, Process, AI) via the `[PluginCapability]` attribute, the loader relaxes security scanning rules accordingly. Non-declarable capabilities (P/Invoke, Unsafe, Reflection Emit, etc.) are always blocked. Plugins are loaded in isolation via `AssemblyLoadContext`.

### Adding a New Skin

1. Implement `ISkin` in `src/SiliconLife.App/Web/Skins/`:

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

2. Skin is auto-discovered by `SkinManager`.

## Code Style Guidelines

### Naming Conventions

- **Classes**: PascalCase with functional prefix (e.g., `DefaultSiliconBeing`)
- **Interfaces**: Start with `I` (e.g., `IAIClient`, `ITool`)
- **Implementations**: End with interface name (e.g., `OllamaClient` implements `IAIClient`)
- **Tools**: End with `Tool` (e.g., `CalendarTool`, `ChatTool`)
- **ViewModels**: End with `ViewModel` (e.g., `BeingViewModel`)

### Code Organization

```
SiliconLife.Common/
├── AI/                    # AI clients and factory implementations (Ollama, DashScope, DeepSeek, Zhipu, Ernie, Hunyuan, MiniMax, Moonshot, SiliconFlow, VolcengineArk, Herdsman, LongCat, QiniuAI)
├── Calendar/              # 32 calendar implementations
├── Localization/          # Localization base classes and 34 language variant implementations
├── Security/              # Permission Manager
├── SiliconBeing/          # Default Silicon Being implementation
├── Tools/                 # Shared built-in tools (25)
├── Web/                   # Web infrastructure
└── WebView/               # Playwright WebView implementation

SiliconLife.App/          # Application layer shared by Default and Fast
├── Config/                # Application configuration
├── Help/                  # Help documentation localization
├── Project/               # Project System (workflow engine, project roles)
└── Web/                   # Web UI implementation
    ├── Component/         # 27 UI components
    ├── Controllers/       # 24 route controllers
    ├── Models/            # View models
    ├── Views/             # HTML views
    └── Skins/             # 7 skin themes

SiliconLife.Default/      # Version-specific directory
├── Config/                # Default configuration data
├── Knowledge/             # Knowledge Network implementation
├── Logging/               # Logger Provider implementations (console + file system)
├── Project/               # Project System implementation
└── Storage/               # File system storage implementation

SiliconLife.Fast/         # Version-specific directory
├── Config/                # Fast version configuration data
├── Logging/               # Logger Provider implementations (console + file system)
├── Storage/               # SpeedyPack storage adapter
└── Tray/                  # System tray localization
```

### Documentation

- All public APIs must have XML documentation comments
- All source files use Apache 2.0 license header
- Leverage .NET 9 features (implicit usings, nullable reference types)

## Development Workflow

### 1. Setup Development Environment

```bash
# 克隆仓库
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# 恢复依赖
dotnet restore

# 构建
dotnet build
```

### 2. Run Tests

```bash
# 运行所有测试
dotnet test

# 运行特定测试项目
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Debug

```bash
# 以调试输出运行
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Format Code

```bash
# 格式化代码
dotnet format
```

## Building Custom Features

### Example: Adding a Custom Calendar

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

### Example: Adding a Custom Executor

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

### Example: Adding a Custom Workflow Template

```csharp
public class MyWorkflowTemplate : WorkflowTemplate
{
    public override string Name => "my_workflow";
    public override string Description => "A custom workflow template";

    public override void DefineStates()
    {
        AddState("start", "开始", isInitial: true);
        AddState("processing", "处理中");
        AddState("review", "审核");
        AddState("done", "完成", isFinal: true);
    }

    public override void DefineTransitions()
    {
        AddTransition("start", "processing", "开始处理");
        AddTransition("processing", "review", "提交审核");
        AddTransition("review", "done", "审核通过");
        AddTransition("review", "processing", "审核退回");
    }
}
```

### Example: Adding Project Roles

Project roles are managed through `ProjectTool`'s `assign_role` and `remove_role` operations. Role names are custom strings used to distinguish Silicon Beings' responsibilities in workflows and task assignments.

## Testing Guidelines

### Unit Tests

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

### Integration Tests

Test complete flows:
1. AI returns tool calls
2. Tool executes
3. Results fed back to AI
4. AI returns final response

## Performance Considerations

### Storage System

- Default version uses file-based JSON storage
- Fast version uses SpeedyPack Storage Engine (.spk format)
- SpeedyPack uses in-memory Directory Map + Entry Cache + Write Queue
- Time-indexed queries use `ITimeStorage` interface

### Main Loop Scheduler

- Clock-based time-slice fair scheduling
- Watchdog timers for detecting stuck operations
- Circuit Breaker for preventing cascade failures

## Best Practices

### 1. Always Validate Permissions

Any AI-initiated operation must go through the permission chain:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return Result.Denied("Permission denied");
}
```

### 2. Use Service Locator

Register and retrieve services globally:

```csharp
// During initialization
ServiceLocator.Instance.Register<ICustomService>(myService);

// When needed
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Follow Body-Brain Separation

- Body handles state and triggers
- Brain handles AI interaction and tool execution

### 4. Implement Proper Error Handling

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

## Contributing Guidelines

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes using conventional commits
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Commit Message Format

```
<type>(<scope>): <description>

Examples:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md)
- 📖 Explore the [API Reference](api-reference.md)
- 🔒 Review the [Security Documentation](security.md)
- 🚀 See the [Quick Start Guide](getting-started.md)
