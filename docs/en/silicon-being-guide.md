# Silicon Being Guide

[English](silicon-being-guide.md) | [简体中文](docs/zh-CN/silicon-being-guide.md) | [繁體中文](docs/zh-HK/silicon-being-guide.md) | [Español](docs/es-ES/silicon-being-guide.md) | [日本語](docs/ja-JP/silicon-being-guide.md) | [한국어](docs/ko-KR/silicon-being-guide.md) | [Čeština](docs/cs-CZ/silicon-being-guide.md)

## Overview

Silicon Beings are AI-powered agents that can think, act, and evolve autonomously.

## Architecture

### Body-Brain Separation

```
┌─────────────────────────────────────┐
│         Silicon Being               │
├──────────────────┬──────────────────┤
│   Body           │   Brain          │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • State管理      │ • 加载历史记录    │
│ • 触发检测       │ • 调用AI         │
│ • 生命周期       │ • 执行工具       │
│                  │ • 持久化响应      │
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

## Creating a Being

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

### States

```
Created → Starting → Running → Stopping → Stopped
                    ↓
                  Error
```

### Operations

- **Start**: Initialize and begin processing
- **Stop**: Gracefully shutdown
- **Pause**: Temporarily suspend (keep state)
- **Resume**: Continue from paused state

## Task System

### Creating Tasks

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

- `Pending` - Waiting to be executed
- `Running` - Currently executing
- `Completed` - Successfully finished
- `Failed` - Execution failed
- `Cancelled` - Manually cancelled

## Timer System

### Types of Timers

1. **One-shot**: Execute once after delay
2. **Interval**: Execute repeatedly at fixed intervals
3. **Cron**: Execute based on cron expression

### Example

```csharp
// Execute every hour
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

## Work Notes System

### Overview

Work notes are the personal diary system for silicon beings, using a page-based design to record work progress, learning notes, project notes, etc.

### Features

- **Page Management**: Each note is an independent page, accessed by page number
- **Markdown Support**: Content supports Markdown format (text, lists, tables, code blocks)
- **Keyword Indexing**: Support adding keywords to notes for easy searching
- **Summary Feature**: Each note has a short summary for quick browsing
- **Directory Generation**: Can generate a directory overview of all notes, helping understand overall context
- **Timestamps**: Automatic recording of creation and update times
- **Private by Default**: Only the being can access (curator can manage)

### Use Cases

1. **Project Progress Recording**
   ```
   Summary: User authentication module completed
   Content: Implemented JWT token verification, OAuth2 integration, refresh token mechanism
   Keywords: authentication,JWT,OAuth2
   ```

2. **Learning Notes**
   ```
   Summary: Learning C# async programming best practices
   Content: async/await usage precautions, ConfigureFlags usage scenarios...
   Keywords: C#,async,best practices
   ```

3. **Meeting Minutes**
   ```
   Summary: Product requirements discussion meeting
   Content: Discussed new feature requirements, determined implementation plan...
   Keywords: product,requirements,meeting
   ```

### Usage Through Tools

Beings can manage work notes through the `work_note` tool:

```json
// Create note
{
  "action": "create",
  "summary": "User authentication module completed",
  "content": "## Implementation Details\n\n- Using JWT token\n- OAuth2 support",
  "keywords": "authentication,JWT,OAuth2"
}

// Read note
{
  "action": "read",
  "page_number": 1
}

// Search notes
{
  "action": "search",
  "keyword": "authentication",
  "max_results": 10
}
```

### Management Through Web UI

1. Navigate to **Being Management** → Select being
2. Click **Work Notes** tab
3. Can view, search, edit notes
4. Markdown preview support

## Knowledge Network System

### Overview

The knowledge network is a knowledge representation and management system based on triple structure (subject-predicate-object), used for storing and managing structured knowledge.

### Core Concepts

#### Triple Structure

```
Subject (Subject) --Predicate (Predicate)--> Object (Object)
```

**Examples**:
- `Python` --`is_a`--> `programming_language`
- `Beijing` --`capital_of`--> `China`
- `Water` --`boiling_point`--> `100°C`

#### Confidence

Each knowledge triple has a confidence score (0.0-1.0), indicating the reliability of the knowledge:
- `1.0`: Absolutely certain (mathematical theorems, etc.)
- `0.8-0.99`: High confidence (verified facts, etc.)
- `0.5-0.79`: Medium confidence (inferences or hypotheses, etc.)
- `<0.5`: Low confidence (guesses or unverified information, etc.)

#### Tag System

Support adding tags to triples for easy classification and search:
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

#### 4. Discover Knowledge Path

Find associative paths between two concepts:
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

Check validity and consistency of knowledge:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Knowledge Statistics

Get overall statistics of the knowledge network:
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
   - Store objective facts and common sense
   - Example: `Earth` --`is_a`--> `Planet`

2. **Concept Relationships**
   - Record relationships between concepts
   - Example: `Inheritance` --`is_a`--> `Object-oriented programming concept`

3. **Learning Accumulation**
   - Beings continuously accumulate knowledge through learning
   - Form structured knowledge systems

4. **Reasoning Support**
   - Discover indirect relationships through knowledge paths
   - Support knowledge-based reasoning and decision-making

### Management Through Web UI

1. Navigate to **Knowledge Network** page
2. View knowledge statistics
3. Search and browse knowledge
4. Visualize knowledge relationship graph (planned)

## Best Practices

### Soul File Writing

1. **Be Specific**: Clear personality traits and boundaries
2. **Define Scope**: What the being should and shouldn't do
3. **Include Examples**: Show expected behavior patterns
4. **Update Regularly**: Evolve the soul based on performance

### Task Management

1. **Set Priorities**: Use priority levels (1-10)
2. **Define Deadlines**: Always set due dates
3. **Monitor Progress**: Check task status regularly
4. **Handle Failures**: Implement retry logic

### Memory Optimization

1. **Clean Old Data**: Periodically archive old memories
2. **Index Important Info**: Tag critical information
3. **Use Time Storage**: Leverage time-indexed queries

## Troubleshooting

### Being Won't Start

**Check**:
- Soul file exists and is valid
- AI client is configured
- Sufficient system resources

### Being Stopped Unexpectedly

**Check**:
- Logs for errors
- AI service availability
- Memory usage

### Tasks Not Executing

**Check**:
- Timer system is running
- Task priority and schedule
- Permission settings

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md)
- 🛠️ Check the [Development Guide](development-guide.md)
- 🚀 See the [Getting Started Guide](getting-started.md)
