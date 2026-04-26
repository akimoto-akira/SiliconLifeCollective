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
    public override string Config_Title => "Configuration Management";
    public override string FAQ_Title => "FAQ";
    public override string Memory_Title => "Memory System";
    public override string OllamaSetup_Title => "Ollama Installation and Model Download";
    public override string BailianDashScope_Title => "Alibaba Cloud Bailian Platform User Guide";

    public override string[] GettingStarted_Tags => new[] { "install", "start", "setup", "quick start", "getting started", "begin", "launch", "initialize" };
    public override string[] BeingManagement_Tags => new[] { "being", "create", "config", "being management", "silicon being", "profile", "setup", "manage" };
    public override string[] ChatSystem_Tags => new[] { "chat", "message", "conversation", "chat system", "dialog", "communicate", "talk", "discussion" };
    public override string[] TaskTimer_Tags => new[] { "task", "timer", "schedule", "tasks and timers", "cron", "automation", "recurring", "reminder" };
    public override string[] Permission_Tags => new[] { "permission", "security", "access control", "permission management", "auth", "authorization", "privacy", "protection" };
    public override string[] Config_Tags => new[] { "config", "settings", "options", "configuration", "preferences", "customization", "system", "parameters" };
    public override string[] FAQ_Tags => new[] { "faq", "help", "questions", "frequently asked", "support", "troubleshooting", "guide", "assistance" };
    public override string[] Memory_Tags => new[] { "memory", "history", "records", "memory system", "activity", "trace", "search", "log" };
    public override string[] OllamaSetup_Tags => new[] { "Ollama", "install", "model", "download", "local AI", "setup", "configure", "run" };
    public override string[] BailianDashScope_Tags => new[] { "Bailian", "DashScope", "Alibaba Cloud", "cloud AI", "API", "configuration", "model", "billing" };
    
    public override string GettingStarted => @"
# Quick Start

## Starting the System

### Double-click to Start (Recommended)

Find the program file and double-click to start:
- **Windows**: `SiliconLife.Default.exe`
- The system will automatically start and **automatically open the browser**

It's that simple! No configuration needed.

## First Use

On first startup, the system will **automatically complete all initialization**:
- ✅ Automatically create the Silicon Curator
- ✅ Use built-in soul file (prompt)
- ✅ Automatically save configuration
- ✅ All services automatically ready

You just need to wait for the browser to open, and you're ready to go!

## Interface Overview

The system interface is divided into two main parts:

### Left Navigation Bar

Contains the following functional modules:

- **💬 Chat** - Chat with silicon beings
- **📊 Dashboard** - View system status
- **🧠 Beings** - View and manage silicon beings
- **🔍 Audit** - View operation records
- **📚 Knowledge** - Manage knowledge graph
- **📁 Projects** - Manage code projects
- **📝 Logs** - View system logs
- **⚙ Config** - System settings
- **❓ Help** - This document
- **ℹ About** - System information

### Main Content Area

Displays the content of the current page, which changes based on the functional module you select.

## Quick Start

### 1. Chat with a Silicon Being

This is the most commonly used feature:

1. Click the **💬 Chat** icon on the left
2. Select a silicon being from the left list (the Silicon Curator is available by default)
3. Type your message in the input box at the bottom
4. Press `Enter` to send
5. AI will reply to you in real-time

**Tips:** 
- Press `Shift + Enter` for a new line
- Click the ⏹ button to stop AI reply

### 2. View Silicon Being Information

Want to see detailed information about a silicon being:

1. Click the **🧠 Beings** icon on the left
2. Click any silicon being card
3. Detailed information will be displayed on the right:
   - Status (Idle/Running)
   - Number of timers and tasks
   - Links to memory, permissions, chat history, etc.

### 3. Modify System Settings

Need to adjust system configuration:

1. Click the **⚙ Config** icon on the left
2. Find the configuration item you want to modify
3. Click the ""Edit"" button
4. Enter the new value and save

**Common Settings:**
- Change interface language
- Change theme skin
- Adjust AI model
- Modify access port

## File Upload

Let AI analyze file content:

1. Click the **📁** button in the chat interface
2. Enter the full path of the file
   - For example: `C:\Users\YourUsername\Documents\Report.pdf`
3. Click ""Confirm Upload""
4. AI will read and analyze the file

**Supported File Types:**
- Text files: .txt, .md, .json
- Code files: .cs, .js, .py
- Configuration files: .yml, .yaml
- Others: .csv, .log, etc.

## View Chat History

Want to review previous conversations:

1. Go to the **🧠 Beings** page
2. Click the silicon being you want to view
3. Click the ""Chat History"" link
4. Browse all historical sessions

## Get Help

When you encounter problems:

- **View Help**: Click the **❓ Help** icon on the left
- **View Logs**: Click the **📝 Logs** icon on the left
- **Restart System**: Many problems can be solved by restarting

## Next Steps

Now that you understand the basic operations, you can:

- 📖 Read other help documents to learn about detailed features
- 💬 Chat with the Silicon Curator and let it help you complete tasks
- ⚙ Explore configuration options to personalize your system

Enjoy using the system!
";

    public override string BeingManagement => @"
# Being Management

## What is a Silicon Being?

A Silicon Being is the core entity of the system. Each silicon being is an independent AI agent with:
- **Soul File**: Core prompt that defines behavior patterns, personality, and capabilities
- **Memory System**: Saves conversation history and important information
- **Task System**: Executes scheduled tasks and automated operations
- **Tool Set**: Various callable functional tools

## View Silicon Beings

### Being List

Entering the ""Beings"" page shows all silicon beings displayed in card format:
- **Name**: The display name of the silicon being
- **Status**: Idle (green) or Running (blue)
- **Type**: If custom compiled code is loaded, a type tag will be shown

### View Being Details

Click any silicon being card, and detailed information will be displayed on the right:
- **ID**: Unique identifier of the silicon being
- **Status**: Current running status
- **Custom Compilation**: Whether custom code is loaded
- **Timer Count**: Click to view timer management
- **Task Count**: Click to view task list
- **Memory**: Click to view memory system
- **Permissions**: Click to view permission configuration
- **Chat History**: View historical conversation records
- **Work Notes**: View work notes
- **AI Client**: Click to view and modify AI configuration
- **Soul File**: Click to view and edit the prompt

## Edit Silicon Being

### Edit Soul File

The soul file determines the silicon being's behavior patterns and capabilities.

1. Click the ""Soul File"" link in the being details page
2. Enter the soul file editor (supports Markdown format)
3. Modify the prompt content
4. Save changes

### Edit AI Configuration

You can configure independent AI services for each silicon being:

1. Click the ""AI Client"" link in the being details page
2. Select the AI client type (such as Ollama, OpenAI, etc.)
3. Configure API endpoint, model, key, and other parameters
4. Takes effect immediately after saving

## Soul File Writing Guide

### Basic Structure

```markdown
# Role Setting

You are a [role description], skilled in:
- Skill 1
- Skill 2
- Skill 3

# Behavior Guidelines

1. Guideline 1
2. Guideline 2
3. Guideline 3

# Workflow

When receiving a task:
1. Understand the requirements
2. Analyze the approach
3. Execute operations
4. Report results
```

### Writing Tips

1. **Clear Role Definition**: Clearly define the silicon being's responsibilities and expertise
2. **Set Behavior Boundaries**: Explain what can be done and what should not be done
3. **Provide Workflows**: Guide the silicon being on how to handle tasks
4. **Use Markdown Format**: Supports headings, lists, code blocks, etc.

### Example: Programming Assistant

```markdown
# Role Setting

You are a professional full-stack development assistant, skilled in:
- C# / .NET development
- Architecture design and code review
- Database design and optimization
- Web front-end development

# Behavior Guidelines

1. Always provide runnable code examples
2. Explain key code logic and design thinking
3. Provide best practice recommendations
4. When uncertain, clearly inform the user

# Code Standards

- Follow SOLID principles
- Use clear naming
- Add necessary comments
- Consider error handling and edge cases
```

## Silicon Being Status

### Running Status

- **Idle**: Waiting for tasks or conversations (green indicator)
- **Running**: Currently executing a task or in conversation (blue indicator)

### Monitor Silicon Beings

Through the dashboard, you can view:
- Total number of current silicon beings
- Task execution status for each being
- Resource usage statistics

## Best Practices

1. **Separation of Responsibilities**: Different beings handle different domains (e.g., programming assistant, customer service assistant, data analysis, etc.)
2. **Continuous Optimization**: Continuously optimize soul files based on actual usage feedback to improve being performance
3. **Backup Configuration**: It is recommended to back up soul files for important beings

## Troubleshooting

### Q: Silicon being is not responding?

Check:
1. Is the AI service running normally?
2. Is the network connection normal?
3. Is the soul file configured correctly?
4. Check system logs for detailed error information

### Q: How to change the AI model for a silicon being?

Click the ""AI Client"" link in the being details page, select a new AI model and configure it. Takes effect immediately after saving. New conversations will use the new model.

### Q: Silicon being behavior doesn't meet expectations?

1. Check if the soul file is clear and explicit
2. Add more behavior guidelines and constraints
3. Provide specific workflow guidance
4. Test and continuously optimize
";

    public override string ChatSystem => @"
# Chat System

## Start a Conversation

1. Click the **💬 Chat** icon in the left navigation bar
2. Select the silicon being you want to chat with from the left list
3. Type your message in the input box at the bottom
4. Press `Enter` or click the ""Send"" button
5. AI will reply in real-time (text appears character by character)

## Interface Description

### Interface Layout

- **Left List**: Shows all silicon beings, click to switch conversation target
- **Center Area**: Displays conversation messages
  - Your messages appear on the right
  - AI replies appear on the left
- **Bottom Input Area**: Input box and send button

### Button Description

- **Send Button**: Send your entered message
- **⏹ Stop Button**: Appears when AI is replying, click to interrupt AI reply
- **📁 File Button**: Upload files for AI analysis

## Basic Operations

### Send Message

- Press `Enter` to send after typing your message
- Press `Shift + Enter` for a new line

### Stop Reply

If AI is currently replying, you can:
- Click the ""⏹ Stop"" button
- Or send a new message (will automatically interrupt the current reply)

### Upload File

Let AI analyze file content:

1. Click the **📁** button next to the input box
2. Enter the file path in the popup panel
   - For example: `C:\Users\YourUsername\Documents\Report.pdf`
3. Click ""Confirm Upload""
4. AI will read and analyze the file

**Supported File Types**:
- Text files: .txt, .md, .json, .xml
- Code files: .cs, .js, .py, .java, etc.
- Configuration files: .yml, .yaml, .ini, .conf
- Other files: .csv, .log, etc.

## Conversation Features

### Real-time Streaming Display

AI replies will be displayed character by character, so you don't need to wait for the complete reply to see the content.

### Multi-turn Conversation

- The system automatically saves conversation history
- AI remembers what was said earlier
- You can directly reference previous conversations

### Tool Calling

AI may automatically call tools during conversation to:
- Query calendar
- Manage system configuration
- Execute code
- Read files
- Search help
- Create notes
- Query memory

When AI calls a tool, you will see the tool name and execution result.

### Multi-language Conversation

You can converse with AI in any language, and AI will automatically reply in the same language.

## View Chat History

If you want to view past conversation records:

1. Click the **🧠 Beings** icon in the left navigation bar
2. Click the silicon being card you want to view
3. Find the ""Chat History"" link in the right details
4. Click to enter and view all historical sessions

## Common Questions

### Q: What if AI replies slowly?

**Possible reasons**:
- The model being used is large and requires more computation time
- Network latency (when using cloud models)
- Long conversation history

**Solutions**:
- Try using local models (like Ollama)
- Choose a lighter model

### Q: AI is not calling tools?

**Check the following**:
1. Confirm if the tool is enabled
2. Check if there are permission restrictions
3. Confirm if the AI model supports tool calling

### Q: How to upload files?

Click the ""📁"" button next to the input box, enter the full path of the file (e.g., `C:\Documents\file.pdf`), then click ""Confirm Upload"".

### Q: How to view previous conversations?

On the ""Beings"" page, click the ""Chat History"" link for the corresponding silicon being to see all historical sessions.

## Usage Suggestions

1. **Express Clearly**: Describe your needs in clear language
2. **Ask Step by Step**: Break complex questions into multiple smaller questions
3. **Provide Context**: Explain relevant background information when necessary
4. **Use File Upload**: When you need AI to analyze files, directly provide the file path
5. **Pay Attention to Tool Calls**: Note the tools AI calls to ensure operations meet expectations
";

    public override string TaskTimer => @"
# Tasks and Timers

## Overview

The task and timer system records the automated execution status of silicon beings. You can view the task list and timer status to understand what the silicon being is doing and when it executes.

## Task System

### What is a Task?

Tasks are work items that a silicon being is executing or has completed, such as:
- Processing tasks automatically created by AI
- Work items generated by the system
- Execution tasks triggered by timers

### View Task List

**Method 1: View All Tasks**

1. Click the ""Tasks"" icon in the left navigation bar (if available)
2. The page will display the task list for all silicon beings

**Method 2: View Tasks for a Specific Being**

1. Go to the **🧠 Beings** page
2. Click the silicon being you want to view
3. Find the ""Tasks"" link in the details
4. Click to enter the task page

### Task Information

Each task displays the following information:

- **Task Name**: The title of the task
- **Status**:
  - Waiting (yellow)
  - Running (blue)
  - Completed (green)
  - Failed (red)
  - Cancelled (gray)
- **Priority**: The priority level of the task
- **Assigned To**: The silicon being executing this task
- **Created Time**: When the task was created
- **Description**: Detailed description of the task

### Task Status Explanation

- **Waiting**: Task has been created, waiting for execution
- **Running**: Task is currently executing
- **Completed**: Task successfully completed
- **Failed**: Task execution failed, error information can be viewed
- **Cancelled**: Task was cancelled

## Timer System

### What is a Timer?

A timer is an automatic triggering mechanism that allows silicon beings to perform operations at specific times. The system uses a calendar system to define trigger conditions.

### View Timer List

**Method 1: View All Timers**

1. Click the ""Timers"" icon in the left navigation bar (if available)
2. The page will display the timer list for all silicon beings

**Method 2: View Timers for a Specific Being**

1. Go to the **🧠 Beings** page
2. Click the silicon being you want to view
3. Find the ""Timers"" link in the details
4. Click to enter the timer page

### Timer Information

Each timer displays the following information:

- **Timer Name**: Identifier for the timer
- **Status**: Running or Stopped
- **Type**: Trigger type of the timer
- **Trigger Time**: Next trigger time
- **Calendar System**: Calendar used (e.g., Gregorian, Lunar, etc.)
- **Trigger Count**: Total number of times triggered
- **Created Time**: When the timer was created
- **Last Trigger Time**: Time of last trigger

### Timer Types

The system supports multiple trigger methods:

- **Interval Trigger**: Triggers at fixed intervals
  - For example: Every 2 hours, every 30 minutes
  
- **Calendar Trigger**: Triggers based on calendar conditions
  - For example: Every day at 9 AM, every Monday, 1st of each month
  - Supports multiple calendar systems including Gregorian, Lunar, etc.

## View Execution History

### Timer Execution History

Understand the execution status of timers:

1. Go to the timer page
2. Find the timer you want to view
3. Click the ""Execution History"" link
4. View all trigger records

### Execution Details

Detailed information for each execution:

1. Find a specific execution in the execution history
2. Click to view details
3. You can see:
   - Execution time
   - Execution result
   - Related conversation messages
   - Error information (if failed)

### Execution Messages

View the complete conversation for a specific execution:

1. Find the ""Messages"" link in the execution details page
2. View the complete conversation between AI and user
3. Understand how AI handled this trigger

## Common Questions

### Q: How to create a new task?

**A:** Tasks are automatically generated by the system and do not support manual creation. When a silicon being needs to perform a task, it will automatically create one.

### Q: How to create a new timer?

**A:** Timers are automatically managed by silicon beings and do not support manual creation. Silicon beings will set up timers as needed to execute periodic tasks.

### Q: Can I delete tasks or timers?

**A:** The system does not provide manual deletion functionality. Tasks and timers are automatically managed by silicon beings.

### Q: What to do if a task shows ""Failed""?

**Suggestions:**
1. View the error information for the task
2. Understand the reason for failure
3. If it's a temporary issue, the task may retry
4. If it continues to fail, you can chat with the silicon being to understand the situation

### Q: Timer is not triggering?

**Check:**
1. Is the timer in running status?
2. Are the trigger conditions met?
3. Is the silicon being running normally?
4. View execution history to understand the situation

### Q: How to know what the silicon being is doing?

**Methods:**
1. View the task list to understand current executing tasks
2. View the timer list to understand upcoming triggered operations
3. View execution history to understand past activities
4. Directly chat with the silicon being to ask

### Q: What does task priority mean?

**A:** Priority indicates the importance level of a task. The smaller the number, the higher the priority. High-priority tasks will be processed first.

## Usage Suggestions

1. **Check Regularly**: Understand the automated execution status of silicon beings
2. **Focus on Failed Tasks**: Handle abnormal situations promptly
3. **View Execution History**: Understand AI's work patterns
4. **Combine with Chat**: Discuss task and timer status with silicon beings

## Technical Notes

### Data Storage

Task and timer data are stored in the system data directory, associated with silicon beings:
```
data/
  beings/
    {BeingID}/
      tasks/      (Task data)
      timers/     (Timer data)
```

### Automatic Management

The system will automatically:
- Create and manage tasks
- Trigger timers
- Record execution history
- Clean up expired data

You don't need to manage manually; the system will handle everything.
";

    public override string Permission => @"
# Permission Management

## What is the Permission System?

The permission system protects your system security and prevents AI from executing unauthorized operations. When AI attempts to perform certain operations (such as accessing files, running commands, etc.), the system checks whether it is allowed.

## How Do Permissions Work?

### Automatic Permission Popup

When AI attempts to perform an operation that requires permission, the system will popup to ask you:

**Popup content includes:**
- Permission type (e.g., file access, command execution, etc.)
- Requested resource (e.g., file path)
- Detailed information

**You can choose:**
- **Allow**: Execute this operation
- **Deny**: Block this operation

### Permission Verification Order

The system checks permissions in the following order:

1. **Silicon Curator**: If it's the curator's operation, automatically allow
2. **Frequency Limit**: Prevent a large number of requests in a short time
3. **Global Rules**: Predefined allow/deny rules
4. **Custom Rules**: Permission rules you've written (if any)
5. **Ask User**: If none of the above can decide, popup to ask you

## Built-in Permission Rules

The system has predefined some safe permission rules:

### File Access Rules

**Allowed access:**
- Silicon being's own temporary directory
- User's common folders (Desktop, Downloads, Documents, Pictures, Music, Videos)
- Public user folders

**Denied access:**
- System critical directories (Windows system directory, Linux /etc /boot, etc.)
- Other silicon beings' data directories

**Unmatched paths:**
- Will popup to ask if you allow it

## Custom Permission Rules (Advanced Feature)

If you need more granular permission control, you can write custom permission rules.

### Access Permission Edit Page

1. Go to the **🧠 Beings** page
2. Click the silicon being you want to configure
3. Find the ""Permissions"" link in the details
4. Enter the permission code editor

### Permission Code Editor

The permission editor is a code editing interface that supports:
- C# code syntax highlighting
- Code auto-completion
- Real-time saving
- Security scanning (to prevent malicious code)

**Save method:**
- Click the ""Save"" button in the editor
- The system will first compile and check
- Takes effect only after passing security scanning

### Default Template

If there is no custom permission code yet, the system will provide a default template. You can modify it based on the template.

## View Permission Rules

### View Current Rule List

1. Go to the permission edit page
2. The page will display all permission rules for this silicon being
3. Each rule includes:
   - Permission type
   - Resource path
   - Allow/Deny
   - Description

## Permission Request History

All permission requests are recorded in the audit log:

1. Click the **🔍 Audit** icon on the left
2. Filter permission-related records
3. View historical requests and your decisions

## Common Questions

### Q: Why was the AI operation denied?

**Possible reasons:**
- The operation is in a deny rule
- Frequency limit triggered
- You previously chose to deny

**Solutions:**
1. Check the audit log to understand the specific reason
2. Modify permission rules if needed
3. Re-execute the operation

### Q: What if there are too many permission popups?

**Suggestions:**
- For commonly used safe operations, consider writing custom rules to automatically allow
- Check if you can reduce popups by modifying rules

### Q: Is custom permission code dangerous?

**Security guarantees:**
- Code will go through security scanning
- Malicious code will be rejected
- Compilation failures will not take effect

**Suggestions:**
- If you're not familiar with programming, it's recommended to use default rules
- Backup original code before modifying
- Test before applying to production environment

### Q: Permission configuration error causes inability to use?

**Solutions:**
1. Operate as the Silicon Curator (curator has highest permission)
2. Delete custom permission code (clear code and save)
3. The system will restore default rules

### Q: Can I set different permissions for different beings?

**A:** Yes. Each silicon being has independent permission configuration, and they don't affect each other.

## Security Recommendations

1. **Be cautious with sensitive operations**: Such as deleting files, executing commands, etc.
2. **Regularly check audit logs**: Understand AI's operation history
3. **Don't modify permission rules casually**: Unless you understand their impact
4. **Keep the system updated**: Get the latest security protection

## Permission Type Description

The system supports the following permission types:

- **Network Access**: AI attempts to access network resources
- **Command Execution**: AI attempts to run command-line programs
- **File Access**: AI attempts to read or write files
- **Function Call**: AI attempts to call specific functions
- **Data Access**: AI attempts to access system data

Each type has different security levels and handling methods.
";

    public override string Config => @"
# Configuration Management

## What is Configuration Management?

The configuration management page allows you to adjust various system settings, including AI services, network, language, interface themes, etc.

## How to Use the Configuration Page?

1. Click the **⚙ Config** icon in the left navigation bar
2. The page will display multiple configuration groups, each containing several configuration items
3. Find the configuration item you want to modify, click the ""Edit"" button on the right
4. Enter the new value in the popup edit box
5. Click the ""Save"" button

## Configuration Group Description

### Basic Settings

Contains basic system configuration:

- **Data Directory**: The folder location for storing all system data
  - Default: `./data`
  - Recommendation: Keep default unless you have special needs

- **Language**: The language displayed in the system interface
  - Supported: Simplified Chinese, Traditional Chinese, English, Japanese, Korean, German, Spanish, etc.
  - After modification: The page will automatically refresh to apply the new language

### AI Settings

Configure AI service connection and model:

- **AI Client Type**: Select the AI service to use
  - Ollama (runs locally, recommended)
  - OpenAI (cloud service)
  - Other services compatible with OpenAI API

- **AI Configuration**: Detailed configuration for AI service
  - `endpoint`: API address (e.g., `http://localhost:11434`)
  - `model`: Model name to use (e.g., `qwen3.5:cloud`)
  - `temperature`: Reply creativity level (0-1, default 0.7)
  - `maxTokens`: Maximum reply length (default 4096)

**Edit AI Configuration**:
1. Click the ""AI Configuration"" edit button
2. A dictionary editor will open
3. You can add, modify, or delete configuration items
4. Click ""Save"" to apply

### Runtime Settings

Control system runtime behavior:

- **Execution Timeout**: Maximum execution time for a single task
  - Default: 10 minutes
  - Recommendation: Keep default unless tasks are particularly complex

- **Max Timeout Count**: How many consecutive timeouts trigger protection mechanism
  - Default: 3 times
  - Purpose: Prevent infinite retry loops

- **Watchdog Timeout**: How long to wait before restarting when system is unresponsive
  - Default: 10 minutes
  - Purpose: Automatically recover stuck systems

- **Minimum Log Level**: Which log levels to record
  - Trace: Most detailed (includes all debug information)
  - Debug: Debug information
  - Info: General information (recommended)
  - Warning: Only warnings
  - Error: Only errors

### Web Settings

Configure web server parameters:

- **Web Port**: System access port
  - Default: 8080
  - Access address: `http://localhost:8080`
  - After modification: System restart required to take effect

- **Allow LAN Access**: Whether to allow other devices on the local network to access
  - Off (default): Only accessible from this machine
  - On: Other devices on the same network can also access
  - Note: Administrator privileges required when enabled

- **Web Skin**: Interface theme
  - You can choose different skins to change the interface appearance
  - Takes effect immediately after modification

### User Settings

- **User Nickname**: Your display name in the system
  - Default: User
  - Can be changed to any name you prefer

## Edit Configuration Items

### Editing Methods for Different Types

The system will display different editing interfaces based on the configuration item type:

**Text Type**:
- Displays a text input box
- Enter the new value directly

**Number Type**:
- Displays a number input box
- Can enter integers or decimals

**Boolean Type (Yes/No)**:
- Displays a checkbox
- Checked means ""Yes"", unchecked means ""No""

**Enum Type (Dropdown Selection)**:
- Displays a dropdown list
- Select one from preset options

**Time Interval**:
- Displays four input boxes for days, hours, minutes, and seconds
- Fill in the corresponding values separately

**Directory Path**:
- Displays a path input box and ""Browse"" button
- Click ""Browse"" to select a folder
- Or enter the path directly

**Dictionary Type (Key-Value Pairs)**:
- Displays a key-value pair editor
- Can add multiple rows of key-value pairs
- Click ""Add"" button to add a new row
- Click ""Delete"" button to remove a row

### Save Configuration

- Click ""Save"" after each configuration modification
- Most configurations take effect immediately
- Some configurations (like port) require system restart

## Common Questions

### Q: Cannot access the system after modifying the port?

**Solution**:
1. Check if the port is occupied by another program
2. Confirm if the firewall allows this port
3. Access using the new port: `http://localhost:NewPort`

### Q: How to restore default configuration?

**Method 1**: Manual modification
1. Go to the configuration page
2. Change configuration items back to default values one by one

**Method 2**: Delete configuration file
1. Close the system
2. Delete the `config.json` file
3. Restart the system (will automatically create default configuration)

### Q: What to do if AI connection fails?

**Check the following**:
1. Is the AI service running normally?
2. Is the endpoint address correct?
3. If using cloud service, is the API key correct?
4. Is the network connection normal?

**Solution**:
1. Go to the ""AI Settings"" group
2. Click the ""AI Configuration"" edit button
3. Check if `endpoint` and `model` are correct
4. Save after modification

### Q: When do configuration changes take effect?

- **Immediate effect**: Language, skin, AI configuration, user nickname, etc.
- **Requires restart**: Web port, LAN access settings

### Q: Where is the configuration file?

The configuration file is the `config.json` file in the system runtime directory.

## Usage Suggestions

1. **Modify with caution**: Keep uncertain configuration items at default values
2. **Record changes**: Record what and why you modified after changing configuration
3. **Backup configuration**: You can copy the `config.json` file as backup before important modifications
4. **Test environment**: If possible, verify configuration in a test environment first
5. **Security first**: Ensure network security before enabling LAN access
";

    public override string FAQ => @"
# Frequently Asked Questions

## Getting Started

### Q: How to start the system?

**A:** Double-click the program file to start. The system will automatically open the browser and enter the interface.

### Q: What do I need to do on first startup?

**A:** Nothing! The system will automatically complete initialization, including creating the Silicon Curator. You just need to wait for the browser to open and start using it.

### Q: The browser didn't open after system startup?

**A:** Manually visit `http://localhost:8080`.

## AI Conversation

### Q: What if AI replies slowly?

**Possible reasons:**
- The model being used is large
- Network latency (when using cloud AI)
- Long conversation history

**Solutions:**
- Use local AI services (like Ollama)
- Choose a lighter model

### Q: AI's response doesn't meet expectations?

**Suggestions:**
1. Check if the soul file is clear and explicit
2. Provide more background information in the conversation
3. Try to describe your needs more specifically

### Q: AI is not calling tools?

**Check:**
1. Confirm the tool is enabled
2. Check if there are permission restrictions
3. Confirm the AI model supports tool calling

### Q: How to make AI analyze files?

**Method:**
1. Click the ""📁 File"" button in the chat interface
2. Enter the full path of the file (e.g., `C:\Documents\report.pdf`)
3. Click ""Confirm Upload""
4. AI will read and analyze the file

## Silicon Beings

### Q: How to create a new silicon being?

**A:** Currently, the system does not support directly creating silicon beings. The Silicon Curator can create and manage other beings. You can chat with the curator and ask it to help you create one.

### Q: How to modify a silicon being's behavior?

**Method:**
1. Go to the ""Beings"" page
2. Click the being you want to modify
3. Click the ""Soul File"" link
4. Modify the prompt content
5. Save

### Q: How to configure different AI for different beings?

**Method:**
1. Go to the ""Beings"" page
2. Click the target being
3. Click the ""AI Client"" link
4. Select AI service and configure
5. Save

### Q: Silicon being is not responding?

**Check:**
1. Is the AI service running normally?
2. Is the network connection normal?
3. Check system logs for detailed errors

## System Settings

### Q: How to change the system language?

**Method:**
1. Click the ""⚙ Config"" icon on the left
2. Find the ""Language"" configuration item
3. Click ""Edit""
4. Select language from the dropdown list
5. Save (page will automatically refresh)

### Q: How to change the interface theme?

**Method:**
1. Go to the ""Config"" page
2. Find the ""Web Skin"" configuration item
3. Click ""Edit""
4. Select your preferred theme
5. Save

### Q: How to modify the access port?

**Method:**
1. Go to the ""Config"" page
2. Find the ""Web Port"" configuration item
3. Click ""Edit""
4. Enter the new port number (e.g., 9000)
5. Save and restart the system

**Note:** After modifying the port, you need to use the new port to access, e.g., `http://localhost:9000`

### Q: How to allow other devices on the local network to access?

**Method:**
1. Go to the ""Config"" page
2. Find the ""Allow LAN Access"" configuration item
3. Click ""Edit""
4. Check ""Yes""
5. Save

**Note:** Administrator privileges required. After modification, other devices can access via `http://YourIP:8080`

## Chat History

### Q: How to view past conversations?

**Method:**
1. Go to the ""Beings"" page
2. Click the being you want to view
3. Find the ""Chat History"" link in the details
4. Click to enter and browse all historical sessions

### Q: How to delete conversation history?

**A:** The system currently does not provide functionality to delete conversation history. Conversation history is automatically saved so that silicon beings can remember previous conversations.

## Data and Storage

### Q: Where is data stored?

**A:** By default, it's stored in the `data` folder under the program runtime directory.

### Q: How to backup data?

**Method:** Simply copy the entire `data` folder to a safe location.

### Q: How to migrate to a new computer?

**Steps:**
1. Close the system
2. Copy the entire `data` folder
3. Install the system on the new computer
4. Put the `data` folder in the program directory of the new computer
5. Start the system

## Configuration File

### Q: Where is the configuration file?

**A:** The `config.json` file in the program runtime directory.

### Q: Can I edit the configuration file directly?

**A:** Yes, but not recommended. It's recommended to modify through the configuration page on the web interface, which is safer and less error-prone.

### Q: What if I made a mistake in configuration?

**Solution:**
1. Close the system
2. Delete the `config.json` file
3. Restart the system (will automatically create default configuration)

**Or:** If you have a backup, you can restore the backed-up configuration file.

## Performance Issues

### Q: System is running slowly?

**Suggestions:**
- Use local AI services (like Ollama)
- Choose lighter AI models
- Reduce the number of concurrently running tasks

### Q: High memory usage?

**Suggestions:**
- Use lighter AI models
- Regularly clean up unnecessary data

## Get Help

### Q: What to do when encountering problems?

**Suggested steps:**
1. **View help documentation**: Click the ""❓ Help"" icon on the left
2. **View logs**: Check system runtime logs on the ""📝 Logs"" page
3. **Restart system**: Many problems can be solved by restarting

### Q: How to view system logs?

**Method:**
1. Click the ""📝 Logs"" icon on the left
2. Browse the log list
3. You can filter by level (errors, warnings, etc.)

## Other Questions

### Q: What languages does the system support?

**A:** Supports Simplified Chinese, Traditional Chinese, English, Japanese, Korean, German, Spanish, and multiple other languages.

### Q: Do I need internet connection to use it?

**A:** It depends on the AI service you use:
- **Local AI (like Ollama)**: No internet connection required
- **Cloud AI (like OpenAI)**: Internet connection required

### Q: Is the system secure?

**A:** Yes. The system has built-in permission management mechanisms. All AI operations go through permission verification, and sensitive operations will request your confirmation.

### Q: Can I customize features?

**A:** The system supports extending functionality through code writing, but this requires some programming knowledge. Regular users are recommended to use the features provided by the system.
";

    public override string Memory => @"
# Memory System

## What is the Memory System?

The memory system records all activity history of silicon beings, including conversations, tool calls, system events, etc. Through the memory system, you can understand what the silicon being has done, when it did it, and the results.

## How to Access the Memory System?

Access through the being page:

1. Click the **🧠 Beings** icon on the left
2. Click the silicon being card you want to view
3. Find the ""Memory"" link in the right details
4. Click to enter the memory page

## Memory Page Description

### Page Layout

- **Top**: Being selector and statistics
- **Filter Area**: Filter conditions for type, time, keywords, etc.
- **List Area**: Displays the memory entry list
- **Detail Area**: Shows detailed content after clicking a memory entry

### Memory Types

The system records the following types of memories:

- **Conversation**: Conversations between user and AI
- **Tool Call**: Execution records of AI calling tools
- **System Event**: Important events during system operation
- **Summary**: Compressed summaries of conversations or events

## View Memories

### Browse Memory List

1. Select the being you want to view
2. The page will display the memory list for this being
3. Each memory shows:
   - Type icon
   - Content summary
   - Time
   - Status (Success/Failure)

### View Memory Details

Click any memory entry to display:
- Full content
- Timestamp
- Related parameters
- Execution result (if it's a tool call)

### Trace Original Context

For certain memory entries, the system provides a ""Trace"" function:
1. Click the ""Trace"" button in the memory details
2. The system will display the complete context when this memory occurred
3. Helps you understand why AI did this at that time

## Filter Memories

### Filter by Type

Click the type filter to select the memory type to view:
- Only conversations
- Only tool calls
- Only system events
- Only summaries

### Filter by Time

You can select a time range:
- Enter start date
- Enter end date
- Only show memories within that time period

### Keyword Search

Enter keywords in the search box:
- Supports Chinese and English
- Will search the full content of memories
- Automatically displays matching results after input

**Search tips:**
- Using specific keywords makes it easier to find results
- Can combine with type and time filters
- If there are too many results, try more specific keywords

### Display Summary or Original Records

- **Show All**: Display all memories
- **Summary Only**: Only show compressed summary records
- **Original Only**: Only show original detailed records

## Memory Statistics

The top of the page displays statistical information:
- Total number of memories
- Number of memories by type
- Storage usage

Through these statistics, you can understand:
- Being activity level
- Main types of activities
- Whether old memories need to be cleaned up

## Paginated Browsing

If there are many memories, the system will display them in pages:
- Default 20 records per page
- Use page number buttons to flip pages
- Can adjust the number of records displayed per page

## Common Questions

### Q: How to find a specific conversation?

**Method:**
1. Enter keywords from the conversation in the search box
2. Select ""Conversation"" in the type filter
3. If you know the approximate time, you can set the time range
4. Browse the search results

### Q: What if memories take up too much space?

**Suggestions:**
- Memories are automatically managed and usually don't require manual intervention
- The system creates summaries to compress historical records
- If really necessary, you can contact the system administrator

### Q: Can I delete memories?

**A:** The system does not provide functionality to delete memories. Memories are important history for silicon beings, and keeping memories helps AI better understand and answer questions.

### Q: Can I export memories?

**A:** The current version does not support export functionality. Memory data is stored in the system data directory.

### Q: Why are some memories ""Summaries""?

**A:** The system automatically compresses longer conversations or events into summaries to save storage space and improve query efficiency. Summaries retain key information but omit details.

### Q: How to view detailed information about AI tool calls?

**Method:**
1. Select ""Tool Call"" in the type filter
2. Find the corresponding tool call record
3. Click to view detailed information
4. You can see tool name, parameters, execution results, etc.

### Q: Memory search returns no results?

**Suggestions:**
1. Check if the keyword is correct
2. Try different keywords
3. Check if the time range is set correctly
4. Confirm the selected being is correct
5. Try viewing all memories without setting filters

## Usage Suggestions

1. **Check regularly**: Understand the activity status of silicon beings
2. **Use filters**: Quickly locate the information you need
3. **Use trace**: Understand AI's decision-making process
4. **Pay attention to statistics**: Understand system operation status

## Technical Notes

### Data Storage

Memory data is stored in the system data directory:
```
data/
  beings/
    {BeingID}/
      memory/
        (Memory files)
```

### Automatic Management

The system will automatically:
- Record important activities
- Create conversation summaries
- Maintain time indexes
- Optimize query performance

You don't need to manually manage memories; the system will handle everything.
";

    public override string OllamaSetup => @"
# Ollama Installation and Model Download

## What is Ollama?

Ollama is an open-source local AI model runtime tool that allows you to run large language models on your own computer without internet connection (after downloading the model).

**Advantages:**
- Runs completely locally, protecting privacy
- Supports multiple AI models
- Easy to install and use
- Free and open-source

## Download and Install Ollama

### Windows System

**Step 1: Download the installation package**

Visit the Ollama official website download page:
- URL: https://ollama.com/download
- Automatically downloads the Windows installer (ollama-setup.exe)

**Step 2: Run the installer**

1. Double-click the downloaded `ollama-setup.exe` file
2. Follow the installation wizard prompts to complete installation
3. After installation, Ollama will automatically start

**Step 3: Verify installation**

1. Open Command Prompt (press `Win + R`, type `cmd`, press Enter)
2. Enter the following command:
   ```
   ollama --version
   ```
3. If a version number is displayed, installation was successful

### Mac System

**Method 1: Download the installation package**

1. Visit https://ollama.com/download
2. Download the Mac installer
3. Double-click the installer, drag to Applications folder

**Method 2: Install using Terminal**

Open Terminal and enter:
```bash
brew install ollama
```

**Verify installation:**
```bash
ollama --version
```

### Linux System

**One-command installation:**

Open Terminal and run:
```bash
curl -fsSL https://ollama.com/install.sh | sh
```

**Verify installation:**
```bash
ollama --version
```

## Start Ollama

### Windows

- Ollama automatically starts after installation
- You can see the Ollama icon in the system tray (bottom-right corner)
- Right-click the icon to manage

### Mac / Linux

Run in terminal:
```bash
ollama serve
```

Or directly run:
```bash
ollama
```

This will open an interactive menu.

## Download and Run Models

### What is a Model?

A model is the ""brain"" of AI, determining AI's capabilities. Different models have different characteristics:
- **Different sizes**: Larger models are more capable but require more memory
- **Different specialties**: Some are good at conversation, some at programming

### Model Intelligence (B Unit)

AI model ""intelligence"" is usually measured in **B (Billion, billion parameters)**:
- **7B-8B**: Basic level, can complete simple tasks, but may perform poorly in complex scenarios
- **13B-14B**: Medium level, performs well for most daily tasks
- **32B and above**: Higher level, stronger complex reasoning and long text understanding capabilities

**This system recommends using models above 8B** for a better user experience.

### Local Models vs Cloud Models

Ollama supports two model running methods:

**Local models:**
- Model files are downloaded to your computer
- Runs completely locally, no internet required (after download)
- Limited by your hardware configuration (memory, graphics card)
- Usually 4B-70B parameters
- Free to use, no restrictions

**Cloud models:**
- Models run on Ollama cloud servers
- Only need to download model identifier (very small)
- Can run ultra-large models that home computers cannot handle (usually 200B+)
- Requires internet connection
- Has usage quota limits (**refreshes weekly**)
- Enable Ollama client's cloud features to use

### Hardware Configuration Recommendations

### Recommended Models

Here are commonly used free models:

| Model Name | Intelligence | Size | Features | Suitable Scenarios |
|-----------|--------------|------|----------|-------------------|
| **qwen3.5:8b** | 8B | About 4-5GB | Strong Chinese capability, good overall performance | Daily conversation, writing, translation |
| **qwen3.5:14b** | 14B | About 8-9GB | Stronger Chinese capability, improved reasoning | Complex tasks, long text processing |
| **qwen3.5:32b** | 32B | About 18-20GB | High intelligence, excellent complex reasoning | Professional tasks, deep analysis |
| **llama3:8b** | 8B | About 4-5GB | Strong English capability, good versatility | English conversation, general tasks |
| **llama3:70b** | 70B | About 40GB | Ultra-high intelligence, top English performance | Difficult English tasks |
| **gemma3:4b** | 4B | About 2-3GB | Lightweight, fast speed | Quick response, low-spec computers |
| **gemma3:12b** | 12B | About 7-8GB | Balance performance and resources | Daily use |
| **mistral:7b** | 7B | About 4GB | Balance performance and speed | General scenarios |
| **codellama:7b** | 7B | About 4GB | Good at programming | Code generation, debugging |
| **codellama:13b** | 13B | About 7-8GB | Stronger programming capability | Complex code tasks |

**Recommended for Chinese users: qwen3.5:8b or qwen3.5:14b**

### Download Models

**Method 1: Command line download**

Open Terminal (or Command Prompt) and enter:

```bash
ollama pull qwen3.5
```

The system will automatically download the model, which takes some time (depending on network speed and model size).

**Method 2: Run and auto-download**

```bash
ollama run qwen3.5
```

If the model is not downloaded, it will automatically start downloading.

### Run Models

After downloading, run the model:

```bash
ollama run qwen3.5
```

This will open an interactive conversation interface where you can directly chat with AI.

**Example conversation:**
```
>>> Hello!
Hello! I'm Qwen, how can I help you?

>>> Please write a poem about spring
Spring breeze blows, flowers bloom,
Green and fragrant fills the garden.
Swallows return to find old nests,
April is good time in the world.
```

Press `Ctrl + D` or type `/bye` to exit the conversation.

### View Downloaded Models

```bash
ollama list
```

Will display a list of all downloaded models.

### Delete Unneeded Models

```bash
ollama rm qwen3.5
```

## Use Ollama in Silicon Life

### Configure Connection

1. Ensure Ollama is started and running
2. Open Silicon Life system
3. Go to the **⚙ Config** page
4. Find ""AI Client Type"" and select `OllamaClient`
5. In ""AI Configuration"" set:
   - **endpoint**: `http://localhost:11434` (default)
   - **model**: `qwen3.5` (or other models you downloaded)
6. Save configuration

### Test Connection

1. Go to the **💬 Chat** page
2. Select a silicon being
3. Send a message
4. If you receive a reply, the connection is successful

## Common Questions

### Q: Ollama download is very slow?

**Solution:**
- Model files are usually large (2-8GB), downloading takes time
- Ensure stable network connection
- Can download at night or when network is idle

### Q: What if download is interrupted?

**Solution:**
Re-run the download command, it will continue:
```bash
ollama pull qwen3.5
```

### Q: How do I know what size model my computer can run?

**Memory and model size matching suggestions:**
- **4GB RAM**: Recommend models under 2GB (about 2B-3B)
- **8GB RAM**: Can run 4GB models (about 7B-8B)
- **16GB RAM**: Can run 8GB models (about 13B-14B)
- **32GB RAM**: Can run 16GB models, but will experience noticeable lag and increased heating (about 32B)
- **64GB and above**: Can smoothly run larger models

**Important reminder:**
- Laptops with 32GB RAM running models around 16B will experience **noticeable lag** and **increased heating**
- This is not a malfunction, but normal behavior due to insufficient hardware resources
- **Recommendation**: In this case, choose smaller models (8B-14B) or upgrade to higher-spec hardware

**Start testing with lightweight models**, and if running smoothly, try larger models.

### Q: What if Ollama fails to start?

**Check:**
1. Is port 11434 occupied by another program?
2. Reinstall Ollama
3. Check Ollama logs for error information

### Q: What if the model runs slowly?

**Suggestions:**
- Use a smaller model (e.g., gemma3 instead of qwen3.5)
- Close other memory-intensive programs
- Check if computer configuration meets requirements

### Q: Can I use multiple models in Silicon Life simultaneously?

**A:** Yes. Download multiple models in Ollama, then select different models for different beings in Silicon Life's being AI configuration.

### Q: Does Ollama require internet connection?

**A:** 
- **When downloading models**: Internet connection required
- **When running models**: No internet required (model is downloaded locally)

### Q: How much disk space do models occupy?

**A:** 
- Small models: About 2-4GB
- Medium models: About 4-8GB
- Large models: 8GB and above

Recommend keeping sufficient disk space.

## Get More Help

- **Ollama Official Website**: https://ollama.com
- **Ollama Documentation**: https://docs.ollama.com
- **Model Library**: https://ollama.com/library (view all available models)

## Next Steps

After installing Ollama and downloading models, you can:
- Configure and use local AI in Silicon Life
- Enjoy completely local AI services
- Protect your privacy and data security

Enjoy using the system!
";

    public override string BailianDashScope => @"
# Alibaba Cloud Bailian Platform User Guide

## What is Alibaba Cloud Bailian?

Alibaba Cloud Bailian (DashScope) is a large model service platform provided by Alibaba Cloud, offering multiple high-quality AI models including Tongyi Qianwen, DeepSeek, GLM, Kimi, etc.

**Advantages:**
- High model intelligence (up to hundreds of B)
- No local hardware required, runs in the cloud
- Supports multiple top-tier AI models
- Pay-per-use, cost controllable
- Compatible with OpenAI API format

## Registration and Service Activation

### Step 1: Register Alibaba Cloud Account

1. Visit Alibaba Cloud official website: https://www.aliyun.com
2. Click ""Free Registration""
3. Complete registration following the prompts (supports phone number, email registration)
4. Complete real-name authentication (requires Alipay or bank card)

### Step 2: Activate Bailian Service

1. Log in to Alibaba Cloud Console
2. Search for ""Bailian"" or ""DashScope""
3. Click to enter the Bailian product page
4. Click ""Activate Now""
5. Read and agree to the service agreement
6. Complete activation

### Step 3: Obtain API Key

1. Enter Bailian Console
2. Find ""API Key Management"" or ""Key Management"" in the left menu
3. Click ""Create API Key""
4. Name the key (e.g., ""SiliconLife"")
5. Copy and save the API Key (**displayed only once, please save it properly**)

## Configure Bailian in Silicon Life

### Configuration Steps

1. Open Silicon Life system
2. Go to the **⚙ Config** page
3. Find ""AI Client Type"" and select `DashScopeClient`
4. In ""AI Configuration"" fill in:
   - **API Key**: Paste the API Key you copied
   - **Region**: Select server region (e.g., beijing)
   - **Model**: Select model (**after filling in API Key and selecting region, the system will automatically fetch all available models for that region**)
5. Save configuration

**Tip:**
- You must first fill in the API Key and select a region before the model dropdown list will load
- If the model list fails to load, a recommended model list will be displayed

### Region Selection

The system supports the following regions:

| Region | Location | Description |
|--------|----------|-------------|
| **beijing** | Beijing, China | Default recommendation, fast domestic access speed |
| **virginia** | Virginia, USA | Suitable for overseas users |
| **singapore** | Singapore | Asia-Pacific region |
| **hongkong** | Hong Kong, China | Asia-Pacific region |
| **frankfurt** | Frankfurt, Germany | Europe region |

**Region selection suggestions:**
- **Mainland China users**: Choose beijing (Beijing), fastest access speed, **but weaker translation support**
- **Need high-quality translation**: Choose singapore (Singapore) or hongkong (Hong Kong), better translation quality, accessible to mainland China users as well
- **Overseas users**: Choose the region closest to you

### Model Selection

When configuring the model, the system will automatically fetch available models from the Bailian platform. If fetching fails, recommended models will be displayed:

**Recommended models:**

| Model Name | Intelligence | Features | Suitable Scenarios |
|-----------|--------------|----------|-------------------|
| **qwen3-max** | Ultra-large | Strongest Tongyi Qianwen version | Complex reasoning, professional tasks |
| **qwen3.6-plus** | Large | Balance performance and cost | Daily use (recommended) |
| **qwen3.6-flash** | Medium | Fast speed, low cost | Quick response |
| **qwen-max** | Ultra-large | Previous generation flagship model | Difficult tasks |
| **qwen-plus** | Large | Balanced performance | General scenarios |
| **qwen-turbo** | Medium | Fastest speed | Simple tasks |
| **qwen3-coder-plus** | Large | Good at programming | Code generation, debugging |
| **qwq-plus** | Large | Strong reasoning capability | Math, logical reasoning |
| **deepseek-v3.2** | Ultra-large | DeepSeek latest version | Strong comprehensive capability |
| **deepseek-r1** | Ultra-large | Specialized in reasoning | Complex reasoning |
| **glm-5.1** | Large | Zhipu AI model | Chinese scenarios |
| **kimi-k2.5** | Large | Moonshot model | Long text processing |
| **llama-4-maverick** | Large | Meta open-source model | English scenarios |

**Recommended for Chinese users: qwen3.6-plus or qwen3-max**

## Cost Explanation

### Billing Method

The Bailian platform uses **pay-per-use** billing:
- Billed by input token count
- Different models have different prices
- Higher model intelligence, higher price

### Free Quota

- New users usually have free trial quota
- Some models have free call quota
- Specific quota subject to Bailian platform announcements

### View Usage

1. Log in to Bailian Console
2. Go to ""Usage Query"" or ""Billing Management""
3. View call count and costs

### Cost Saving Suggestions

- Choose appropriate models (not necessarily the most expensive)
- Avoid sending overly long text
- Regularly check usage to control costs

### Token Usage Statistics Explanation

**Important note:** Since the Alibaba Cloud Bailian platform (DashScope) API response does **not stably return token usage fields**, this system **cannot statistics** token usage when using Bailian models.

**This means:**
- The system will not display how many tokens were used
- Cannot view historical token consumption records in the system
- Cannot perform cost analysis based on token usage

**How to view usage:**
- Please log in to Alibaba Cloud Bailian Console to view actual usage and costs
- Bailian Console provides detailed call statistics and billing information

## Common Questions

### Q: Where to get the API Key?

**A:** 
1. Log in to Alibaba Cloud Bailian Console
2. Find ""API Key Management""
3. Create a new API Key
4. Copy and save it properly

### Q: What if the API Key is leaked?

**A:**
1. Immediately log in to Bailian Console
2. Delete the leaked API Key
3. Create a new API Key
4. Update configuration in Silicon Life

### Q: Which region is best to choose?

**A:**
- **Domestic users**: Choose beijing (Beijing), fastest speed
- **Overseas users**: Choose the region closest to you
- Region doesn't affect model quality, only access speed

### Q: Why does the model list fail to load?

**Possible reasons:**
1. API Key is incorrect or expired
2. Network connection issue
3. Bailian service exception

**Solutions:**
1. Check if API Key is correct
2. Check network connection
3. Try again later

### Q: Can I use multiple models?

**A:** Yes. In Silicon Life, you can configure different Bailian models for different silicon beings.

### Q: What's the difference between Bailian and Ollama?

| Feature | Bailian (DashScope) | Ollama |
|---------|---------------------|--------|
| Running location | Cloud | Local computer |
| Hardware requirements | None | Requires higher configuration |
| Model size | Up to hundreds of B | Usually 4B-70B |
| Cost | Pay-per-use | Free |
| Internet | Must be connected | Not required after download |
| Privacy | Data sent to cloud | Completely local |

**Selection suggestions:**
- Low hardware configuration → Choose Bailian
- Need ultra-high intelligence model → Choose Bailian
- Value privacy, don't want to spend money → Choose Ollama
- Use in offline environment → Choose Ollama

### Q: What if call fails?

**Check:**
1. Is API Key correct?
2. Is account balance sufficient?
3. Has free quota been exceeded?
4. Check error information in Bailian Console

### Q: How to control costs?

**Suggestions:**
1. Set budget alerts (Bailian Console)
2. Regularly check usage
3. Choose cost-effective models
4. Avoid frequent calls to large models

## Best Practices

### 1. Choose Appropriate Models

- **Daily conversation**: qwen3.6-plus (balanced)
- **Complex reasoning**: qwen3-max or deepseek-r1
- **Programming tasks**: qwen3-coder-plus
- **Quick response**: qwen3.6-flash or qwen-turbo

### 2. Manage API Keys

- Keep properly, don't share publicly
- Rotate regularly (create new, delete old)
- Create different Keys for different purposes

### 3. Monitor Usage

- Check usage once a week
- Set budget alerts
- Investigate abnormal usage promptly

### 4. Optimize Usage

- Streamline input content, reduce unnecessary text
- Reasonably set conversation history length
- Use appropriate models, don't blindly pursue large models

## Get More Help

- **Bailian Official Website**: https://bailian.console.aliyun.com
- **API Documentation**: https://help.aliyun.com/zh/model-studio
- **Model List**: https://help.aliyun.com/zh/model-studio/models
- **Pricing**: https://www.aliyun.com/price/product#/dashscope
- **Technical Support**: Submit ticket or contact Alibaba Cloud customer service

## Next Steps

After configuring Bailian, you can:
- Use high-quality cloud AI models in Silicon Life
- Experience ultra-high intelligence AI services
- No need to worry about local hardware configuration

Enjoy using the system!
";
    
    #endregion
}
