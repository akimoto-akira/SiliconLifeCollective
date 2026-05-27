# PluginDemo-15: Capability.Process — 声明式进程权限

## 概述

本插件演示如何使用 `[PluginCapability(Capability.Process)]` 声明插件需要启动子进程的能力。声明此能力后，插件可以访问 `System.Diagnostics.Process` 及相关类型。

## 声明语法

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Capability.Process 放行范围

### TypeRef 放行

仅放行 `System.Diagnostics` 下 Process 相关类型：

| 放行类型 | 用途 |
|---------|------|
| `Process` | 启动、管理和监控子进程 |
| `ProcessStartInfo` | 配置进程启动参数 |
| `ProcessThread` | 访问进程线程信息 |
| `ProcessModule` | 访问进程模块信息 |
| `ProcessPriorityClass` | 设置进程优先级 |
| `ProcessWindowStyle` | 配置进程窗口样式 |

始终允许的类型（从不在禁止列表中）：`Stopwatch`、`Debug`、`Trace`、`Activity`

### ILString 放行

- 以 `"System.Diagnostics.Process"` 开头的字符串不会被标记

## 与 09-ForbiddenProcess 的对比

| 方面 | 09-ForbiddenProcess | 15-CapabilityProcess |
|------|-------------------|---------------------|
| 声明 | 无 | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ 被拒绝 | ✅ 允许 |
| ProcessStartInfo | ❌ 被拒绝 | ✅ 允许 |

## 推荐：CommandLineExecutor

即使有 `Capability.Process`，推荐优先使用 `CommandLineExecutor`：

| 特性 | CommandLineExecutor | 直接 Process |
|------|-------------------|-------------|
| 需要能力声明 | 否 | 是 |
| 沙箱 | 命令白名单 | 无 |
| 超时 | 内置 | 手动 |
| 输出捕获 | 结构化 | 手动 |
| 审计日志 | 自动 | 手动 |

当需要对 I/O 流进行精细控制、处理进程事件，或 CommandLineExecutor 的命令白名单过于严格时，才使用 `Capability.Process` + 直接 `Process`。

## 安全最佳实践

1. **优先使用 CommandLineExecutor**：尽可能使用受控入口
2. **提供清晰的 Reason**："Launch build tools for CI pipeline" 而非模糊的 "process access"
3. **验证所有输入**：永远不要将不受信任的输入直接传递给 ProcessStartInfo
4. **使用 WaitForExit**：始终等待进程完成以防僵尸进程
5. **重定向流**：设置 `RedirectStandardOutput = true` 和 `UseShellExecute = false`

## 文件

- `Plugin.cs` — 声明 Capability.Process 的演示插件
- `README.md` — 本文件（英文）
- `README.zh-CN.md` — 简体中文
- 翻译版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相关示例

- **09-ForbiddenProcess**：被阻止的进程操作反例
- **18-CapabilityDenied**：不可声明能力反例
