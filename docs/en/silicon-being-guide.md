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
