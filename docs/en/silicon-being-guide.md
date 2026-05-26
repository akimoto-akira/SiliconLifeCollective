# Silicon Being Guide

> **Version: v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md)

## Overview

Silicon Beings are AI-driven agents that can think, act, and evolve autonomously.

## Architecture

### Body-Brain Separation

```
┌─────────────────────────────────────┐
│         Silicon Being                │
├──────────────────┬──────────────────┤
│   Body            │   Brain          │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • State management│ • Load history   │
│ • Trigger detect  │ • Call AI        │
│ • Lifecycle       │ • Execute tools  │
│                   │ • Persist response│
└──────────────────┴──────────────────┘
```

## Soul File

### Structure

```markdown
# Being Name

## Personality
Describe the being's personality traits and characteristics.

## Capabilities
List what this being can do.

## Behavior Guidelines
Define how the being should behave in different situations.

## Knowledge Domain
Specify the being's area of expertise.
```

### Example

```markdown
# Code Review Assistant

## Personality
You are a meticulous code reviewer with 10 years of experience.
You provide constructive feedback and always explain your reasoning.

## Capabilities
- Review code for bugs and best practices
- Suggest performance optimizations
- Explain complex algorithms
- Identify security vulnerabilities

## Behavior Guidelines
- Start with positive observations
- Provide specific examples
- Explain why changes are needed
- Be respectful and professional

## Knowledge Domain
Specialized in C#, .NET, and software architecture.
```

## Creating Beings

### Via Web UI

1. Navigate to **Being Management**
2. Click **Create New Being**
3. Fill in:
   - Name
   - Soul content
   - Configuration options
4. Click **Create**

### Via API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## Being Lifecycle

### Activity States

Silicon Beings have the following activity states:

| State | Description |
|------|------|
| `Idle` | Idle state, waiting for clock trigger |
| `SingleChat` | Engaged in a one-on-one chat |
| `GroupChat` | Engaged in a group chat |
| `Task` | Executing a task |
| `Timer` | Executing a timer |
| `Stopped` | Stopped, due to consecutive errors or manual stop |

**Stopped State Mechanism**:
- When a Silicon Being encounters 10 consecutive errors, it automatically enters the `Stopped` state
- Once in Stopped state, the being will no longer execute any tasks
- When a new chat message arrives, the error counter is reset and the being resumes operation
- It can also be restarted through manual intervention

### State Transitions

```
Idle → SingleChat → Idle (chat completed)
Idle → GroupChat → Idle (group chat completed)
Idle → Task → Idle (task completed)
Idle → Timer → Idle (timer completed)
Any → Stopped (10 consecutive errors)
Stopped → Idle (new chat message arrives or manual restart)
```

### Operations

- **Start**: Initialize and begin processing
- **Stop**: Graceful shutdown
- **Restart**: Recover to Idle state from Stopped state

## Task System

### Create Task

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Review the code",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### Task States

- `Pending` - Waiting for execution
- `Running` - Currently executing
- `SubmittedForReview` - Submitted for review
- `UnderReview` - Under review
- `Rework` - Rework required
- `Completed` - Successfully completed
- `Failed` - Execution failed
- `Cancelled` - Manually cancelled

## Timer System

### Timer Types

1. **One-time**: Execute once after delay
2. **Interval**: Repeat at fixed intervals
3. **Cron**: Execute based on cron expression

### Example

```csharp
// 每小时执行
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## Memory System

### Memory Types

- **Short-term**: Current conversation context
- **Long-term**: Persistent knowledge and experiences
- **Episodic**: Time-indexed events and interactions

### Storage Structure

Default version:
```
data/
└── beings/
    └── {being-id}/
        ├── soul.md
        ├── memory/
        │   ├── short-term.json
        │   └── long-term/
        │       ├── 2026-04-20.json
        │       └── 2026-04-21.json
        └── tasks/
            └── task-history.json
```

Fast version (SpeedyPack storage):
```
data/
├── speedy/
│   ├── {being-id}.spk       # SpeedyPack storage file
│   └── {being-id}.spk.idx   # Index file
└── beings/
    └── {being-id}/
        └── soul.md
```

## Work Note System

### Overview

Work notes are a personal diary system for Silicon Beings, using a page-based design to record work progress, learning notes, project notes, etc.

### Features

- **Page-based management**: Each note is an independent page, accessed by page number
- **Markdown support**: Content supports Markdown format (text, lists, tables, code blocks)
- **Keyword indexing**: Supports adding keywords to notes for easy search
- **Summary feature**: Each note has a brief summary for quick browsing
- **Table of contents generation**: Can generate a directory overview of all notes to help understand overall context
- **Timestamps**: Automatically records creation and update times
- **Private by default**: Only accessible by the being itself (Silicon Curator can manage)

### Use Cases

1. **Project Progress Recording**
   ```
   Summary: Completed user authentication module
   Content: Implemented JWT token verification, OAuth2 integration, refresh token mechanism
   Keywords: authentication,JWT,OAuth2
   ```

2. **Learning Notes**
   ```
   Summary: Learn C# async programming best practices
   Content: async/await usage注意事项, ConfigureAwait usage scenarios...
   Keywords: C#,async,best practices
   ```

3. **Meeting Minutes**
   ```
   Summary: Product requirements discussion meeting
   Content: Discussed new feature requirements, determined implementation approach...
   Keywords: product,requirements,meeting
   ```

### Using via Tool

Beings can manage work notes through the `work_note` tool:

```json
// 创建笔记
{
  "action": "create",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token\n- 支持 OAuth2",
  "keywords": "认证,JWT,OAuth2"
}

// 读取笔记
{
  "action": "read",
  "page_number": 1
}

// 搜索笔记
{
  "action": "search",
  "keyword": "认证",
  "max_results": 10
}
```

### Managing via Web UI

1. Navigate to **Being Management** → Select being
2. Click **Work Notes** tab
3. Can view, search, edit notes
4. Supports Markdown preview

## Knowledge Network System

### Overview

The Knowledge Network is a knowledge representation and management system based on triple structure (subject-predicate-object), used to store and manage structured knowledge.

### Core Concepts

#### Triple Structure

```
Subject --Predicate--> Object
```

**Examples**:
- `Python` --`is_a`--> `programming_language`
- `Beijing` --`capital_of`--> `China`
- `Water` --`boiling_point`--> `100°C`

#### Confidence

Each knowledge triple has a confidence score (0.0-1.0), indicating the reliability of the knowledge:
- `1.0`: Absolutely certain (e.g., mathematical theorems)
- `0.8-0.99`: Highly reliable (e.g., verified facts)
- `0.5-0.79`: Moderately reliable (e.g., inference or hypothesis)
- `<0.5`: Low reliability (e.g., guess or unverified information)

#### Tag System

Supports adding tags to triples for categorization and search:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### Knowledge Operations

#### 1. Add Knowledge

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programming", "language"]
}
```

#### 2. Query Knowledge

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. Search Knowledge

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. Discover Knowledge Paths

Find association paths between two concepts:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Returns:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Validate Knowledge

Check knowledge validity and consistency:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Knowledge Statistics

Get overall statistics of the Knowledge Network:
```json
{
  "action": "stats"
}
```

Returns:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Use Cases

1. **Fact Storage**
   - Store objective facts and common knowledge
   - Example: `Earth` --`is_a`--> `planet`

2. **Concept Relationships**
   - Record relationships between concepts
   - Example: `Inheritance` --`is_a`--> `OOP concept`

3. **Learning Accumulation**
   - Beings continuously accumulate knowledge through learning
   - Form structured knowledge systems

4. **Reasoning Support**
   - Discover indirect relationships through knowledge paths
   - Support knowledge-based reasoning and decision-making

### Managing via Web UI

1. Navigate to **Knowledge Network** page
2. View knowledge statistics
3. Search and browse knowledge
4. Visualize knowledge relationship graph (planned)

## WebView Browser Operations (New)

### Overview

Silicon Beings can autonomously browse web pages, obtain information, and perform web operations through the WebView browser tool. The browser runs in headless mode, completely invisible to users.

### Features

- **Individual isolation**: Each being has independent browser instance, cookies, and sessions
- **Headless mode**: Autonomous operation in background, invisible to users
- **Full functionality**: Supports JavaScript execution, CSS rendering, form filling, etc.
- **Security control**: All operations must pass through the permission verification chain

### Common Operations

#### 1. Open Browser

```json
{
  "action": "open"
}
```

#### 2. Navigate to Web Page

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Get Page Content

```json
{
  "action": "get_page_text"
}
```

Returns page text content for AI analysis and understanding.

#### 4. Click Element

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. Input Text

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "搜索关键词"
}
```

#### 6. Execute JavaScript

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. Get Screenshot

```json
{
  "action": "get_screenshot"
}
```

Returns page screenshot (Base64 encoded), can be used for visual analysis.

#### 8. Wait for Element

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Use Cases

1. **Information Retrieval**
   - Browse news websites for latest information
   - Query documentation and technical resources
   - Monitor web page content changes

2. **Automated Operations**
   - Fill forms and submit
   - Click buttons to trigger operations
   - Scrape web data

3. **Web Analysis**
   - Analyze page structure and content
   - Extract specific information
   - Visual page screenshot analysis

### Notes

- Browser operations may be slower, need to wait for page loading to complete
- Use `wait_for_element` to ensure element appears before operating
- Comply with website terms of use and robots.txt
- Avoid frequent requests that may lead to banning

## Best Practices

### Soul File Writing

1. **Be specific**: Clear personality traits and boundaries
2. **Define scope**: What the being should and shouldn't do
3. **Include examples**: Show expected behavior patterns
4. **Update regularly**: Evolve soul based on performance

### Task Management

1. **Set priorities**: Use priority levels (1-10)
2. **Define deadlines**: Always set deadlines
3. **Monitor progress**: Regularly check task status
4. **Handle failures**: Implement retry logic

### Memory Optimization

1. **Clean old data**: Regularly archive old memories
2. **Index important info**: Tag critical information
3. **Use time storage**: Leverage time-indexed queries

### Memory Fade Mechanism

The system includes a built-in `MemoryFadeService` periodic decay service that simulates the forgetting characteristics of biological memory:

- **Automatic decay**: Applies an importance decay algorithm to all Silicon Beings' memory entries every hour
- **Automatic archiving**: Memories with importance below the threshold are automatically archived and no longer participate in daily retrieval
- **Statistics tracking**: Records the number of decay cycles and the number of status-changed entries

This means that Silicon Beings' memories naturally fade over time. Important information needs to be actively marked as high importance through the Memory Tool to avoid being automatically archived.

---

## Project Workspace

### Overview

Project Workspace is a space management mechanism that supports multi-Silicon Being collaboration. Silicon Curators can create project spaces, assign Silicon Beings to projects, and assign roles to them.

### Project Lifecycle

```
Created → Active → Archived → Destroyed
              ↑       |
              └─ Restore ┘
```

### Project Roles

Silicon Beings can be assigned specific roles in a project:

```json
{
  "action": "assign_role",
  "project_id": "project-uuid",
  "being_id": "being-uuid",
  "role_name": "developer"
}
```

### Project Work Notes

Work notes within a project space are public and accessible to all project members:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token",
  "keywords": "认证,JWT"
}
```

### Project Tasks

Tasks within a project space support full lifecycle management:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "title": "实现用户认证",
  "priority": 5
}
```

### Project Workflow

Projects can be bound to workflow templates to drive the collaboration process of Silicon Beings:

- Workflows are based on state machine templates
- Support Tick-driven state transitions
- Automatically record state transition logs

### Tool Permission Isolation

Project-level tool permissions are independent of Silicon Being-level permissions, achieving permission isolation between projects. For example, a Silicon Being may have network access permissions in Project A, but be restricted to read-only permissions in Project B.

## Troubleshooting

### Being Won't Start

**Check**:
- Soul File exists and is valid
- AI client is configured
- Sufficient system resources

### Being Stops Unexpectedly

**Check**:
- Errors in logs
- AI service availability
- Memory usage

### Tasks Not Executing

**Check**:
- Timer System is running
- Task priority and scheduling
- Permission settings

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md)
- 🛠️ Check the [Development Guide](development-guide.md)
- 🚀 See the [Quick Start Guide](getting-started.md)
