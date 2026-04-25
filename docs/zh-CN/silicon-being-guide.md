# 硅基生命体指南

[English](silicon-being-guide.md) | [中文](docs/zh-CN/silicon-being-guide.md) | [繁體中文](docs/zh-HK/silicon-being-guide.md) | [Español](docs/es-ES/silicon-being-guide.md) | [日本語](docs/ja-JP/silicon-being-guide.md) | [한국어](docs/ko-KR/silicon-being-guide.md) | [Čeština](docs/cs-CZ/silicon-being-guide.md)

## 概述

硅基生命体是 AI 驱动的智能体，可以自主思考、行动和进化。

## 架构

### 身体-大脑分离

```
┌─────────────────────────────────────┐
│         硅基生命体                   │
├──────────────────┬──────────────────┤
│   身体            │   大脑            │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • 状态管理        │ • 加载历史记录    │
│ • 触发检测        │ • 调用 AI         │
│ • 生命周期        │ • 执行工具        │
│                  │ • 持久化响应      │
└──────────────────┴──────────────────┘
```

## 灵魂文件

### 结构

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

### 示例

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

## 创建生命体

### 通过 Web UI

1. 导航到**生命体管理**
2. 点击**创建新生命体**
3. 填写：
   - 名称
   - 灵魂内容
   - 配置选项
4. 点击**创建**

### 通过 API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## 生命体生命周期

### 状态

```
Created → Starting → Running → Stopping → Stopped
                    ↓
                  Error
```

### 操作

- **启动**：初始化并开始处理
- **停止**：优雅关闭
- **暂停**：临时挂起（保持状态）
- **恢复**：从暂停状态继续

## 任务系统

### 创建任务

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

### 任务状态

- `Pending` - 等待执行
- `Running` - 正在执行
- `Completed` - 成功完成
- `Failed` - 执行失败
- `Cancelled` - 手动取消

## 定时器系统

### 定时器类型

1. **一次性**：延迟后执行一次
2. **间隔**：以固定间隔重复执行
3. **Cron**：基于 cron 表达式执行

### 示例

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

## 记忆系统

### 记忆类型

- **短期**：当前对话上下文
- **长期**：持久化知识和经验
- **情景**：时间索引的事件和交互

### 存储结构

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

## 最佳实践

### 灵魂文件编写

1. **具体**：清晰的个性特征和边界
2. **定义范围**：生命体应该和不应该做什么
3. **包含示例**：展示预期的行为模式
4. **定期更新**：根据表现进化灵魂

### 任务管理

1. **设置优先级**：使用优先级（1-10）
2. **定义截止日期**：始终设置截止日期
3. **监控进度**：定期检查任务状态
4. **处理失败**：实现重试逻辑

### 记忆优化

1. **清理旧数据**：定期归档旧记忆
2. **索引重要信息**：标记关键信息
3. **使用时间存储**：利用时间索引查询

## 故障排除

### 生命体无法启动

**检查**：
- 灵魂文件存在且有效
- AI 客户端已配置
- 系统资源充足

### 生命体意外停止

**检查**：
- 日志中的错误
- AI 服务可用性
- 内存使用

### 任务未执行

**检查**：
- 定时器系统正在运行
- 任务优先级和计划
- 权限设置

## 下一步

- 📚 阅读[架构指南](architecture.md)
- 🛠️ 查看[开发指南](development-guide.md)
- 🚀 查看[快速开始指南](getting-started.md)
