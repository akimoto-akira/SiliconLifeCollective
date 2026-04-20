# Getting Started

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

Edit `src/SiliconLife.Default/Config/DefaultConfigData.cs` or modify the configuration at runtime through the Web UI.

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

```bash
cd src/SiliconLife.Default
dotnet run
```

The web server will start at `http://localhost:8080`

### 5. Access the Web UI

Open your browser and navigate to:

```
http://localhost:8080
```

You'll see the dashboard with:
- Silicon Being management
- Chat interface
- Configuration panel
- System monitoring

## First Silicon Being

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

- 📚 Read the [Architecture Guide](architecture.md) to understand the system design
- 🛠️ Check the [Development Guide](development-guide.md) to extend the system
- 📖 Explore the [API Reference](api-reference.md) for integration details
- 🔒 Review the [Security Documentation](security.md) for permission system

## Project Structure

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # Core interfaces and abstractions
│   └── SiliconLife.Default/   # Default implementations + entry point
├── docs/                      # Documentation (multi-language)
│   ├── en/                    # English
│   ├── zh-CN/                 # Simplified Chinese
│   ├── zh-HK/                 # Traditional Chinese
│   ├── ja-JP/                 # Japanese
│   └── ko-KR/                 # Korean
└── README.md                  # This file
```

## Need Help?

- 📖 Check the [Documentation](docs/)
- 🐛 Report issues on GitHub
- 💬 Join the community discussions
