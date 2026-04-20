# Development Guide

## Architecture Overview

SiliconLifeCollective follows a **Body-Brain Architecture** with strict separation between core interfaces and default implementations.

### Project Structure

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # Interfaces, abstractions, common infrastructure
│   └── SiliconLife.Default/   # Concrete implementations, entry point
└── docs/                      # Multi-language documentation
```

**Dependency Direction**: `SiliconLife.Default` → `SiliconLife.Core` (one-way)

## Core Concepts

### 1. Silicon Being (硅基生命体)

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

5-level permission validation chain:
```
IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
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

### Add a New Tool

1. Create a new class in `src/SiliconLife.Default/Tools/`:

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

2. Tools are automatically discovered via reflection - no manual registration needed!

3. (Optional) Mark as manager-only:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### Add a New AI Client

1. Implement `IAIClient` in `src/SiliconLife.Default/AI/`:

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

3. Factories are automatically discovered and registered.

### Add a New Storage Backend

1. Implement `IStorage` and `ITimeStorage` in `src/SiliconLife.Default/Storage/`:

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

### Add a New Skin

1. Implement `ISkin` in `src/SiliconLife.Default/Web/Skins/`:

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

2. Skins are automatically discovered by the `SkinManager`.

## Code Style Guidelines

### Naming Conventions

- **Classes**: PascalCase with functional prefix (e.g., `DefaultSiliconBeing`)
- **Interfaces**: Start with `I` (e.g., `IAIClient`, `ITool`)
- **Implementations**: End with interface name (e.g., `OllamaClient` implements `IAIClient`)
- **Tools**: End with `Tool` (e.g., `CalendarTool`, `ChatTool`)
- **ViewModels**: End with `ViewModel` (e.g., `BeingViewModel`)

### Code Organization

```
SiliconLife.Default/
├── AI/                    # AI client implementations
├── Calendar/              # Calendar implementations
├── Config/                # Default configuration data
├── Executors/             # Executor implementations
├── IM/                    # IM provider implementations
├── Localization/          # Localization implementations
├── Logging/               # Logger provider implementations
├── Runtime/               # Runtime components
├── Security/              # Security implementations
├── SiliconBeing/          # Default silicon being implementation
├── Storage/               # Storage implementations
├── Tools/                 # Built-in tools
└── Web/                   # Web UI implementation
    ├── Controllers/       # Route controllers
    ├── Models/            # View models
    ├── Views/             # HTML views
    └── Skins/             # Skin themes
```

### Documentation

- All public APIs must have XML documentation comments
- Use Apache 2.0 license header in all source files
- Leverage .NET 9 features (implicit usings, nullable reference types)

## Development Workflow

### 1. Set Up Development Environment

```bash
# Clone repository
git clone https://github.com/your-org/SiliconLifeCollective.git
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

### 4. Code Formatting

```bash
# Format code
dotnet format
```

## Building Custom Features

### Example: Add a Custom Calendar

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

### Example: Add a Custom Executor

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        // Validate permissions first
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        // Execute operation
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
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

Test the complete flow:
1. AI returns tool call
2. Tool executes
3. Result feeds back to AI
4. AI returns final response

## Performance Considerations

### Storage System

- The storage system prioritizes **functionality over performance**
- File-based JSON storage is used by default
- Time-indexed queries use directory structure

### MainLoop Scheduler

- Tick-based time-slicing for fair scheduling
- Watchdog timer for detecting stuck operations
- Circuit breaker for preventing cascade failures

## Best Practices

### 1. Always Validate Permissions

Any AI-initiated operation must go through the permission chain:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
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

## Contribution Guidelines

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
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
- 🚀 Check the [Getting Started Guide](getting-started.md)
