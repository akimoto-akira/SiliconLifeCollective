# Development Guide

> **Version: v0.2.0-alpha**

**English** | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [Čeština](../cs-CZ/development-guide.md)

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
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack management tool (Windows Forms)
└── docs/                            # Multi-language documentation
```

**Dependency direction**:
- `SiliconLife.Default` → `SiliconLife.Core` + `SiliconLife.Common` + `SiliconLife.App`
- `SiliconLife.Fast` → `SiliconLife.Core` + `SiliconLife.Common` + `SiliconLife.App` + `SiliconLife.Speedy`
- `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.App` → `SiliconLife.Core` + `SiliconLife.Common`

**Version Role Description**:
- **SiliconLife.Default**: Default implementation, primarily used for architecture feasibility verification. Provides a simple and reliable file system storage implementation, suitable for development debugging and architecture verification.
- **SiliconLife.Fast**: Main production version. Based on the architecture verified by Default, adopts SpeedyPack in-memory storage + asynchronous persistence, providing extreme performance optimization, the first choice for long-term operation and actual production environments.

## Core Concepts

### 1. Silicon Beings

Each AI agent consists of:
- **Body** (`DefaultSiliconBeing`): Maintains alive state, detects trigger scenarios
- **Brain** (`ContextManager`): Loads history, calls AI, executes tools, persists responses

### 2. Tool System

Tools are automatically discovered and registered via reflection:

```csharp
// All tools implement ITool interface
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Permission System

5-level permission verification chain:
```
UserFrequencyCache → IPermissionCallback → (Curator→IPermissionAskHandler / NonCurator→GlobalACL→Deny)
```

### 4. Service Locator

Global service registration and retrieval:
```csharp
// Register
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Get
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Extending the System

### Adding a New Tool

1. Create a new class in `src/SiliconLife.Common/Tools/` (shared tools for both versions):

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Parse parameters
        var param1 = call.Parameters["param1"]?.ToString();
        
        // Execute logic
        var result = await DoSomething(param1);
        
        // Return result
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

### Adding a New AI Client

1. Implement `IAIClient` in `src/SiliconLife.Common/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Call your AI API
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
        // Implement streaming
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
        // Read from your database
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // Write to your database
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // Time-indexed query
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

> **Security Restrictions**: Plugins cannot reference `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis` and other namespaces. Plugins are loaded in isolation via `AssemblyLoadContext`.

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
├── AI/                    # AI clients and factory implementations
├── Calendar/              # 32 calendar implementations
├── Localization/          # Localization base classes and 29 language implementations
├── Security/              # Permission manager
├── SiliconBeing/          # Default silicon being implementation
├── Tools/                 # Shared built-in tools
├── Web/                   # Web infrastructure
└── WebView/               # Playwright WebView implementation

SiliconLife.App/          # Application layer shared by Default and Fast
├── Config/                # Application configuration
├── Help/                  # Help documentation localization
└── Web/                   # Web UI implementation
    ├── Component/         # UI component library
    ├── Controllers/       # Route controllers
    ├── Models/            # View models
    ├── Views/             # HTML views
    └── Skins/             # Skin themes

SiliconLife.Default/      # Version-specific directory
├── Config/                # Default configuration data
├── IM/                    # WebUI provider
├── Knowledge/             # Knowledge network implementation
├── Logging/               # Log provider implementations
├── Project/               # Project system implementation
├── Security/              # Default permission callbacks
├── Storage/               # File system storage implementation
└── Tools/                 # Version-specific tools (HelpTool)
```

### Documentation

- All public APIs must have XML documentation comments
- All source files use Apache 2.0 license header
- Leverage .NET 9 features (implicit usings, nullable reference types)

## Development Workflow

### 1. Setup Development Environment

```bash
# Clone repository
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Restore dependencies
dotnet restore

# Build
dotnet build
```

### 2. Run Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Debug

```bash
# Run with debug output
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Format Code

```bash
# Format code
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
        // Your conversion logic
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Reverse conversion
        return new GregorianDate(year, month, day);
    }
}
```

### Example: Adding a Custom Executor

```csharp
// Executors are currently static classes (DiskExecutor, NetworkExecutor, CommandLineExecutor)
// that check permissions via ServiceLocator before executing.
// ExecutorBase provides an abstract base with background thread and request queue support.

public static class CustomExecutor
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    public static ExecutorResult Execute(ExecutorRequest request, TimeSpan? timeout = null)
    {
        // Check permission first
        PermissionManager? pm = ServiceLocator.Instance.GetPermissionManager(request.CallerId);
        if (pm == null || !pm.CheckPermission(request.CallerId, PermissionType.Function, request.ResourcePath))
        {
            return ExecutorResult.Failed($"Permission denied: {request.ResourcePath}");
        }

        TimeSpan actualTimeout = timeout ?? DefaultTimeout;

        try
        {
            Task<ExecutorResult> task = Task.Run(() => ExecuteCore(request));
            if (task.Wait(actualTimeout))
            {
                return task.Result;
            }
            return ExecutorResult.Failed("Operation timed out");
        }
        catch (AggregateException ex)
        {
            return ExecutorResult.Failed(ex.InnerException?.Message ?? ex.Message);
        }
    }

    private static ExecutorResult ExecuteCore(ExecutorRequest request)
    {
        // Execute operation
        var result = PerformOperation(request);
        return ExecutorResult.Successful(result);
    }
}
```

## Testing Guidelines

### Unit Tests

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
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
- Fast version uses SpeedyPack in-memory storage engine (.spk format)
- SpeedyPack uses in-memory directory mapping + entry cache + asynchronous write queue
- Time-indexed queries use `ITimeStorage` interface

### Main Loop Scheduler

- Clock-based time-slice fair scheduling
- Watchdog timers for detecting stuck operations
- Circuit breakers for preventing cascade failures

## Best Practices

### 1. Always Validate Permissions

Any AI-initiated operation must go through permission chain:

```csharp
PermissionManager? pm = ServiceLocator.Instance.GetPermissionManager(callerId);
if (pm == null || !pm.CheckPermission(callerId, permissionType, resource))
{
    return ExecutorResult.Failed("Permission denied");
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
