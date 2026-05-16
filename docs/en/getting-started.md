# Quick Start

> **Version: v0.2.0-alpha**

**English** | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [Čeština](../cs-CZ/getting-started.md)

## Choose Version

This project provides two implementation versions:

### SiliconLife.Default (Default Version)
- **Positioning**: Default implementation, primarily used for architecture feasibility verification
- **Runtime Mode**: Console application
- **Storage**: File system JSON storage
- **Use Case**: Data security priority, small data volume, development debugging, architecture verification
- **Platform Support**: Windows, Linux, macOS
- **Role Description**: Serves as the baseline implementation for architecture verification, providing a simple and reliable runtime mode, suitable for first-time contact with this project or development debugging

### SiliconLife.Fast (High-Performance Version)
- **Positioning**: Main production version
- **Runtime Mode**: Desktop application (Windows/macOS system tray / Linux status window)
- **Storage**: SpeedyPack in-memory storage + asynchronous persistence (.spk file format)
- **Use Case**: High concurrency, low latency, large data volume, long-term production operation
- **Platform Support**: Windows/macOS (full features, including system tray), Linux (status window, no tray icon)
- **Role Description**: A production-grade implementation with deep optimization, the first choice for long-term operation and actual production environments

> **Beginner Suggestion**: First-time users are recommended to start with **SiliconLife.Default** to quickly verify architecture feasibility; after becoming familiar with the system, we strongly recommend migrating to **SiliconLife.Fast** as the production environment runtime version.

## Prerequisites

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Download](https://git-scm.com/)
- **Ollama** (optional, for local AI) - [Download](https://ollama.com/)
- **DashScope API Key** (optional, for cloud AI) - [Apply](https://bailian.console.aliyun.com/)

## Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Build the Project

```bash
dotnet build
```

### 3. Configure AI Backend

Edit `src/SiliconLife.Default/Config/DefaultConfigData.cs` or modify configuration at runtime through the Web UI.

#### Option A: Ollama (Local)

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    }
  }
}
```

#### Option B: DashScope (Cloud)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "cn-hangzhou"
    }
  }
}
```

### 4. Run the Application

#### Run Default Version

```bash
cd src/SiliconLife.Default
dotnet run
```

The web server will start at `http://localhost:8080`

#### Run Fast Version

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: The application will start in form mode, minimized to system tray, with the web server also starting at `http://localhost:8080`

**Linux**: The application will display a status window (no system tray icon) and automatically open the browser to access the Web UI. You can also use the `--no-tray` parameter to skip auto-opening the browser:

```bash
dotnet run -- --no-tray
```

### 5. Access the Web UI

Open your browser and navigate to:

```
http://localhost:8080
```

You will see a dashboard with:
- Silicon Being Management
- Chat Interface
- Configuration Panel
- System Monitoring

## Your First Silicon Being

### Create Your First Being

1. Navigate to **Being Management** in the Web UI
2. Click **Create New Being**
3. Configure the soul file (`soul.md`) with personality and behavior
4. Start the being

### Example soul.md

```markdown
# My First Silicon Being

## Personality
You are a helpful assistant specializing in code review.

## Capabilities
- Review code quality
- Suggest improvements
- Explain complex concepts

## Behavior
- Always provide constructive feedback
- Use clear examples
- Be concise but thorough
```

## Common Issues

### Ollama Connection Refused

**Problem**: Cannot connect to Ollama at `http://localhost:11434`

**Solution**:
```bash
# Check if Ollama is running
ollama list

# Start Ollama if needed
ollama serve
```

### Model Not Found

**Problem**: `model "qwen2.5:7b" not found`

**Solution**:
```bash
# Pull the required model
ollama pull qwen2.5:7b
```

### Port Already in Use

**Problem**: `HttpListenerException: Address already in use`

**Solution**:
- Change the port in configuration
- Or kill the process using port 8080:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md) to understand system design
- 🛠️ Check the [Development Guide](development-guide.md) to extend the system
- 📖 Explore the [API Reference](api-reference.md) for integration details
- 🔒 Review the [Security Documentation](security.md) for the permission system
- 🧰 See the [Tools Reference](tools-reference.md) for all built-in tools
- 🌐 Check the [Web UI Guide](web-ui-guide.md) for interface features

## Project Structure

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Core interfaces and abstractions
│   ├── SiliconLife.Common/          # Shared implementations (used by both versions)
│   ├── SiliconLife.App/             # Application layer shared by Default and Fast
│   ├── SiliconLife.Default/         # Default implementation + entry point (console version)
│   ├── SiliconLife.Fast/            # High-performance implementation + entry point (forms version)
│   ├── SiliconLife.Speedy/          # SpeedyPack high-performance storage engine
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack management tool (Windows Forms)
├── docs/                            # Documentation (multi-language, 29 language variants)
│   ├── en/                          # English
│   ├── zh-CN/                       # Simplified Chinese
│   ├── zh-HK/                       # Traditional Chinese
│   ├── es-ES/                       # Spanish
│   ├── ja-JP/                       # Japanese
│   ├── ko-KR/                       # Korean
│   └── cs-CZ/                       # Czech
├── 总文档/                           # Requirements and architecture docs
└── README.md                        # Project readme
```

## Need Help?

- 📖 Check the [Help Documentation System](web-ui-guide.md#help-documentation-system-new) (multi-language support)
- 📚 Read the [Full Documentation](docs/)
- 🐛 Report issues on [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Join community discussions
