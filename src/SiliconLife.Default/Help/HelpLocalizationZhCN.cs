// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

namespace SiliconLife.Default.Help;

/// <summary>
/// Simplified Chinese help documentation implementation.
/// </summary>
public class HelpLocalizationZhCN : HelpLocalizationBase
{
    #region Help Documents
    
    public override string GettingStarted_Title => "快速入门";
    public override string BeingManagement_Title => "硅基人管理";
    public override string ChatSystem_Title => "聊天功能";
    public override string TaskTimer_Title => "任务与定时器";
    public override string Permission_Title => "权限管理";
    public override string Config_Title => "配置管理";
    public override string FAQ_Title => "常见问题";

    public override string[] GettingStarted_Tags => new[] { "安装", "启动", "入门", "快速入门", "开始使用", "初始化", "运行", "配置环境" };
    public override string[] BeingManagement_Tags => new[] { "硅基人", "创建", "配置", "硅基人管理", "生命体", "档案", "设置", "管理" };
    public override string[] ChatSystem_Tags => new[] { "聊天", "对话", "消息", "聊天功能", "交流", "通讯", "交谈", "讨论" };
    public override string[] TaskTimer_Tags => new[] { "任务", "定时器", "调度", "任务与定时器", "自动化", "定时", "提醒", "周期" };
    public override string[] Permission_Tags => new[] { "权限", "安全", "访问控制", "权限管理", "授权", "隐私", "保护", "认证" };
    public override string[] Config_Tags => new[] { "配置", "设置", "选项", "配置管理", "偏好", "自定义", "系统", "参数" };
    public override string[] FAQ_Tags => new[] { "常见问题", "帮助", "问题", "支持", "故障排除", "指南", "协助", "解答" };
    
    public override string GettingStarted => @"
# 快速入门

## 启动系统

在命令行中运行程序，系统会自动启动并监听 8080 端口。

```bash
dotnet run
```

## 访问Web界面

打开浏览器，访问 `http://localhost:8080` 即可进入Web管理界面。

## 首次启动

首次启动时，系统会自动创建硅基主理人（Curator），您需要：

1. 设置主理人名称
2. 配置灵魂文件（提示词）
3. 选择AI模型

## 基本操作

- **仪表板**: 查看系统状态和统计信息
- **硅基人**: 创建和管理硅基生命体
- **聊天**: 与硅基人对话
- **任务**: 设置定时任务
- **配置**: 调整系统设置
- **帮助**: 查看本文档

## 快捷键

- `F1` - 打开帮助文档
- `Ctrl+F` - 聚焦搜索框
";

    public override string BeingManagement => @"
# 硅基人管理

## 创建硅基人

1. 进入""硅基人""页面
2. 点击""创建新硅基人""
3. 填写以下信息：
   - **名称**: 硅基人的显示名称
   - **灵魂文件**: 核心提示词，决定行为模式（支持Markdown格式）
   - **AI模型**: 选择使用的AI模型
4. 点击""创建""完成

## 配置灵魂文件

灵魂文件是硅基人的核心提示词，决定其行为模式、性格特点和能力范围。

### 编写建议

```markdown
# 角色设定

你是一个专业的编程助手，擅长：
- C# 开发
- 架构设计
- 代码审查

# 行为准则

1. 始终提供可运行的代码示例
2. 解释代码的关键部分
3. 提供最佳实践建议
```

## 管理硅基人

- **编辑**: 修改名称、灵魂文件等配置
- **删除**: 永久删除硅基人（不可恢复）
- **克隆**: 基于现有硅基人创建新版本
";

    public override string ChatSystem => @"
# 聊天功能

## 开始对话

1. 选择要对话的硅基人
2. 在输入框中输入消息
3. 按 Enter 或点击发送按钮

## 对话特性

- **实时响应**: 使用SSE技术实现流式输出
- **工具调用**: AI可以调用工具执行操作
- **上下文记忆**: 保存对话历史
- **多语言**: 支持多种语言对话

## 工具使用

硅基人可以自动调用工具来：
- 查询日历信息
- 管理系统配置
- 执行代码
- 访问文件系统（需权限）

## 中断对话

如果AI正在思考，您可以：
- 点击""停止""按钮
- 或发送新消息自动中断
";

    public override string TaskTimer => @"
# 任务与定时器

## 创建定时任务

1. 进入""任务""页面
2. 点击""新建任务""
3. 配置任务：
   - **任务名称**: 描述性名称
   - **触发条件**: 时间表达式（Cron格式）
   - **执行动作**: 选择要执行的操作
   - **目标硅基人**: 选择执行者

## Cron表达式

```
分钟 (0-59)
| 小时 (0-23)
| | 日期 (1-31)
| | | 月份 (1-12)
| | | | 星期 (0-6)
| | | | |
* * * * *
```

### 示例

- `0 * * * *` - 每小时整点
- `0 9 * * *` - 每天上午9点
- `*/5 * * * *` - 每5分钟
- `0 9 * * 1-5` - 工作日早上9点

## 任务管理

- **启用/禁用**: 临时停用任务
- **编辑**: 修改任务配置
- **删除**: 移除任务
- **执行历史**: 查看任务执行记录
";

    public override string Permission => @"
# 权限管理

## 权限级别

系统采用5级权限控制：

1. **IsCurator**: 主理人拥有最高权限
2. **UserFrequencyCache**: 用户频率缓存限制
3. **GlobalACL**: 全局访问控制列表
4. **IPermissionCallback**: 自定义权限回调
5. **IPermissionAskHandler**: 询问用户权限

## 权限类型

- **读取**: 查看信息和数据
- **写入**: 修改和创建数据
- **执行**: 运行工具和命令
- **管理**: 管理系统配置

## 配置权限

1. 进入""配置""页面
2. 选择""权限设置""
3. 配置各项权限：
   - 允许/拒绝特定操作
   - 设置频率限制
   - 配置白名单/黑名单

## 安全建议

- 定期审查权限配置
- 限制敏感操作的访问
- 启用操作日志记录
- 使用最小权限原则
";

    public override string Config => @"
# 配置管理

## 系统配置

### AI模型配置

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

### 网络配置

- **端口**: 默认 8080
- **主机**: 默认 localhost
- **HTTPS**: 可选启用

### 存储配置

- **数据目录**: 硅基人数据存储位置
- **日志级别**: Debug/Info/Warning/Error
- **备份策略**: 自动备份频率

## 皮肤主题

系统支持多种界面主题：

- **Light**: 浅色主题（默认）
- **Dark**: 深色主题
- **自定义**: 创建自己的主题

## 本地化

支持的语言：
- 简体中文 (zh-CN)
- 繁体中文 (zh-HK)
- 英文 (en-US, en-GB)
- 日文 (ja-JP)
- 韩文 (ko-KR)
- 西班牙文 (es-ES, es-MX)
- 捷克文 (cs-CZ)
";

    public override string FAQ => @"
# 常见问题

## 系统相关

### Q: 系统启动失败怎么办？

A: 检查以下几点：
1. 8080 端口是否被占用
2. .NET 9 运行时是否正确安装
3. 查看日志文件获取详细错误信息

### Q: 如何修改监听端口？

A: 在配置文件中修改 `WebHost:Port` 设置，或使用命令行参数。

### Q: 数据存储在哪个目录？

A: 默认存储在硅基人根目录下的 `data` 文件夹。

## AI相关

### Q: AI响应很慢怎么办？

A: 可能的原因：
1. 模型较大，需要更多计算资源
2. 网络延迟（使用云端模型时）
3. 考虑使用本地模型（如 Ollama）

### Q: 如何切换AI模型？

A: 在配置文件中修改 `AI:DefaultProvider`，或在硅基人配置中选择不同模型。

### Q: AI无法调用工具？

A: 检查：
1. 工具是否正确注册
2. 权限是否允许工具调用
3. AI模型是否支持工具调用功能

## 硅基人相关

### Q: 如何备份硅基人数据？

A: 复制硅基人目录下的所有文件到备份位置。

### Q: 可以导入导出灵魂文件吗？

A: 支持。在硅基人编辑页面可以导入/导出Markdown格式的灵魂文件。

### Q: 如何克隆硅基人？

A: 在硅基人列表中选择""克隆""，系统会创建一个副本，您可以修改配置。

## 获取帮助

如果以上无法解决您的问题：

1. 查看[项目文档](https://github.com/your-repo/docs)
2. 提交Issue报告问题
3. 加入社区讨论
";
    
    #endregion
}
