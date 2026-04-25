// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

namespace SiliconLife.Default.Help;

/// <summary>
/// English help documentation implementation.
/// </summary>
public class HelpLocalizationEnUS : HelpLocalizationBase
{
    #region Help Documents
    
    public override string GettingStarted_Title => "Quick Start";
    public override string BeingManagement_Title => "Being Management";
    public override string ChatSystem_Title => "Chat System";
    public override string TaskTimer_Title => "Tasks and Timers";
    public override string Permission_Title => "Permission Management";
    public override string Config_Title => "Configuration";
    public override string FAQ_Title => "FAQ";

    public override string[] GettingStarted_Tags => new[] { "install", "start", "setup", "quick start", "getting started", "begin", "launch", "initialize" };
    public override string[] BeingManagement_Tags => new[] { "being", "create", "config", "being management", "silicon being", "profile", "setup", "manage" };
    public override string[] ChatSystem_Tags => new[] { "chat", "message", "conversation", "chat system", "dialog", "communicate", "talk", "discussion" };
    public override string[] TaskTimer_Tags => new[] { "task", "timer", "schedule", "tasks and timers", "cron", "automation", "recurring", "reminder" };
    public override string[] Permission_Tags => new[] { "permission", "security", "access control", "permission management", "auth", "authorization", "privacy", "protection" };
    public override string[] Config_Tags => new[] { "config", "settings", "options", "configuration", "preferences", "customization", "system", "parameters" };
    public override string[] FAQ_Tags => new[] { "faq", "help", "questions", "frequently asked", "support", "troubleshooting", "guide", "assistance" };
    
    public override string GettingStarted => @"
# Quick Start

## Starting the System

Run the program in command line. The system will start and listen on port 8080.

```bash
dotnet run
```

## Accessing Web Interface

Open browser and visit `http://localhost:8080` to access the web management interface.

## First Startup

On first startup, the system will automatically create the Silicon Curator. You need to:

1. Set the Curator name
2. Configure the soul file (prompt)
3. Select an AI model

## Basic Operations

- **Dashboard**: View system status and statistics
- **Beings**: Create and manage silicon beings
- **Chat**: Converse with silicon beings
- **Tasks**: Set up scheduled tasks
- **Config**: Adjust system settings
- **Help**: View this documentation

## Keyboard Shortcuts

- `F1` - Open help documentation
- `Ctrl+F` - Focus search box
";

    public override string BeingManagement => @"
# Being Management

## Creating a Silicon Being

1. Navigate to the ""Beings"" page
2. Click ""Create New Being""
3. Fill in the following information:
   - **Name**: Display name for the being
   - **Soul File**: Core prompt that determines behavior (supports Markdown)
   - **AI Model**: Select the AI model to use
4. Click ""Create"" to finish

## Configuring Soul File

The soul file is the core prompt of a silicon being, determining its behavior patterns, personality traits, and capabilities.

### Writing Guidelines

```markdown
# Role Setting

You are a professional programming assistant, skilled in:
- C# development
- Architecture design
- Code review

# Behavior Guidelines

1. Always provide runnable code examples
2. Explain key parts of the code
3. Provide best practice recommendations
```

## Managing Beings

- **Edit**: Modify name, soul file, and other configurations
- **Delete**: Permanently delete a being (irreversible)
- **Clone**: Create a new version based on an existing being
";

    public override string ChatSystem => @"
# Chat System

## Starting a Conversation

1. Select a silicon being to chat with
2. Type your message in the input box
3. Press Enter or click the send button

## Chat Features

- **Real-time Response**: Streaming output using SSE technology
- **Tool Calling**: AI can call tools to perform operations
- **Context Memory**: Saves conversation history
- **Multi-language**: Supports conversations in multiple languages

## Tool Usage

Silicon beings can automatically call tools to:
- Query calendar information
- Manage system configurations
- Execute code
- Access file system (requires permission)

## Interrupting Conversation

If AI is thinking, you can:
- Click the ""Stop"" button
- Or send a new message to auto-interrupt
";

    public override string TaskTimer => @"
# Tasks and Timers

## Creating Scheduled Tasks

1. Navigate to the ""Tasks"" page
2. Click ""New Task""
3. Configure the task:
   - **Task Name**: Descriptive name
   - **Trigger**: Cron expression
   - **Action**: Select the operation to execute
   - **Target Being**: Select the executor

## Cron Expression

```
minute (0-59)
| hour (0-23)
| | day of month (1-31)
| | | month (1-12)
| | | | day of week (0-6)
| | | | |
* * * * *
```

### Examples

- `0 * * * *` - Every hour on the hour
- `0 9 * * *` - Every day at 9 AM
- `*/5 * * * *` - Every 5 minutes
- `0 9 * * 1-5` - Weekdays at 9 AM

## Task Management

- **Enable/Disable**: Temporarily deactivate tasks
- **Edit**: Modify task configuration
- **Delete**: Remove tasks
- **Execution History**: View task execution records
";

    public override string Permission => @"
# Permission Management

## Permission Levels

The system uses a 5-level permission control:

1. **IsCurator**: Curator has highest permission
2. **UserFrequencyCache**: User frequency cache limits
3. **GlobalACL**: Global access control list
4. **IPermissionCallback**: Custom permission callback
5. **IPermissionAskHandler**: Ask user permission

## Permission Types

- **Read**: View information and data
- **Write**: Modify and create data
- **Execute**: Run tools and commands
- **Manage**: Manage system configurations

## Configuring Permissions

1. Navigate to ""Config"" page
2. Select ""Permission Settings""
3. Configure permissions:
   - Allow/deny specific operations
   - Set frequency limits
   - Configure whitelist/blacklist

## Security Recommendations

- Regularly review permission configurations
- Restrict access to sensitive operations
- Enable operation logging
- Follow the principle of least privilege
";

    public override string Config => @"
# Configuration

## System Configuration

### AI Model Configuration

```json
{
  ""AI"": {
    ""DefaultProvider"": ""ollama"",
    ""Models"": {
      ""ollama"": {
        ""Endpoint"": ""http://localhost:11434"",
        ""Model"": ""llama3""
      }
    }
  }
}
```

### Network Configuration

- **Port**: Default 8080
- **Host**: Default localhost
- **HTTPS**: Optional

### Storage Configuration

- **Data Directory**: Silicon being data storage location
- **Log Level**: Debug/Info/Warning/Error
- **Backup Strategy**: Automatic backup frequency

## Skin Themes

The system supports multiple interface themes:

- **Light**: Light theme (default)
- **Dark**: Dark theme
- **Custom**: Create your own theme

## Localization

Supported languages:
- Simplified Chinese (zh-CN)
- Traditional Chinese (zh-HK)
- English (en-US, en-GB)
- Japanese (ja-JP)
- Korean (ko-KR)
- Spanish (es-ES, es-MX)
- Czech (cs-CZ)
";

    public override string FAQ => @"
# Frequently Asked Questions

## System Related

### Q: What if the system fails to start?

A: Check the following:
1. Is port 8080 already in use?
2. Is .NET 9 runtime correctly installed?
3. Check log files for detailed error messages

### Q: How to change the listening port?

A: Modify `WebHost:Port` in the configuration file, or use command line arguments.

### Q: Where is data stored?

A: By default in the `data` folder under the silicon being root directory.

## AI Related

### Q: AI response is slow, what to do?

A: Possible reasons:
1. Large model requires more computing resources
2. Network latency (when using cloud models)
3. Consider using local models (like Ollama)

### Q: How to switch AI models?

A: Modify `AI:DefaultProvider` in the configuration file, or select different models in the being configuration.

### Q: AI cannot call tools?

A: Check:
1. Are tools correctly registered?
2. Do permissions allow tool calls?
3. Does the AI model support tool calling?

## Being Related

### Q: How to backup being data?

A: Copy all files under the being directory to a backup location.

### Q: Can I import/export soul files?

A: Yes. You can import/export soul files in Markdown format on the being edit page.

### Q: How to clone a being?

A: Select ""Clone"" in the being list, the system will create a copy that you can modify.

## Getting Help

If the above doesn't solve your problem:

1. Check [project documentation](https://github.com/your-repo/docs)
2. Submit an issue to report problems
3. Join community discussions
";
    
    #endregion
}
