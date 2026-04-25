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

## 工作笔记系统

### 概述

工作笔记是硅基生命体的个人日记系统，采用页式设计，用于记录工作进展、学习心得、项目笔记等。

### 特性

- **页式管理**：每条笔记独立成页，按页码访问
- **Markdown 支持**：内容支持 Markdown 格式（文本、列表、表格、代码块）
- **关键词索引**：支持为笔记添加关键词，便于搜索
- **摘要功能**：每条笔记有简短摘要，快速浏览
- **目录生成**：可生成所有笔记的目录概览，帮助理解整体上下文
- **时间戳**：自动记录创建和更新时间
- **默认私有**：仅生命体自身可访问（主理人可管理）

### 使用场景

1. **项目进展记录**
   ```
   摘要：完成用户认证模块
   内容：实现了 JWT token 验证、OAuth2 集成、刷新 token 机制
   关键词：认证,JWT,OAuth2
   ```

2. **学习笔记**
   ```
   摘要：学习 C# 异步编程最佳实践
   内容：async/await 使用注意事项、ConfigureAwait 的使用场景...
   关键词：C#,异步,最佳实践
   ```

3. **会议纪要**
   ```
   摘要：产品需求讨论会
   内容：讨论了新功能需求，确定了实现方案...
   关键词：产品,需求,会议
   ```

### 通过工具使用

生命体可以通过 `work_note` 工具管理工作笔记：

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

### 通过 Web UI 管理

1. 导航到**生命体管理** → 选择生命体
2. 点击**工作笔记**标签页
3. 可以查看、搜索、编辑笔记
4. 支持 Markdown 预览

## 知识网络系统

### 概述

知识网络是基于三元组结构（主语-谓语-宾语）的知识表示和管理系统，用于存储和管理结构化的知识。

### 核心概念

#### 三元组结构

```
主语 (Subject) --谓语 (Predicate)--> 宾语 (Object)
```

**示例**：
- `Python` --`is_a`--> `programming_language`
- `北京` --`capital_of`--> `中国`
- `水` --`boiling_point`--> `100°C`

#### 置信度

每个知识三元组都有置信度评分（0.0-1.0），表示知识的可信程度：
- `1.0`：绝对确定（如数学定理）
- `0.8-0.99`：高度可信（如经过验证的事实）
- `0.5-0.79`：中等可信（如推断或假设）
- `<0.5`：低可信（如猜测或未验证信息）

#### 标签系统

支持为三元组添加标签，便于分类和搜索：
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### 知识操作

#### 1. 添加知识

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

#### 2. 查询知识

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. 搜索知识

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. 发现知识路径

找出两个概念之间的关联路径：
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

返回：
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. 知识验证

检查知识的有效性和一致性：
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. 知识统计

获取知识网络的整体统计信息：
```json
{
  "action": "stats"
}
```

返回：
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### 使用场景

1. **事实存储**
   - 存储客观事实和常识
   - 示例：`地球` --`is_a`--> `行星`

2. **概念关系**
   - 记录概念之间的关系
   - 示例：`继承` --`is_a`--> `面向对象编程概念`

3. **学习积累**
   - 生命体通过学习不断积累知识
   - 形成结构化的知识体系

4. **推理支持**
   - 通过知识路径发现间接关系
   - 支持基于知识的推理和决策

### 通过 Web UI 管理

1. 导航到**知识网络**页面
2. 查看知识统计信息
3. 搜索和浏览知识
4. 可视化知识关系图（计划中）

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
