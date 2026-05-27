# PluginDemo-09: 禁止的进程操作反例

## 概述

本插件演示了 SiliconLife 插件系统中**被禁止**的进程执行操作。它作为反例参考，展示哪些操作不可做，并为每个违规提供正确的替代方案。

## 为什么禁止 Process 类型？

`System.Diagnostics.Process` 和 `ProcessStartInfo` 在插件中被封禁，因为直接进程执行存在严重安全风险：

1. **任意命令执行**：插件可以在无审计或权限检查的情况下运行任何命令
2. **恶意软件启动**：恶意插件可以执行不需要的应用程序或脚本
3. **系统资源访问**：进程可以访问插件沙箱之外的敏感系统资源
4. **无命令验证**：直接 Process.Start 没有内置的命令注入防护
5. **无审计跟踪**：直接进程操作绕过了插件安全审计系统
6. **权限提升**：可能以高于插件应有权限的级别生成进程

## 哪些类型被禁止？

只有 Process 相关类型被禁止，**不是整个 System.Diagnostics 命名空间**：

| 禁止的类型 | 被拦截的方法 | 风险等级 |
|------------|-------------|---------|
| `Process` | `Start()`、`Kill()`、`WaitForExit()` | 🔴 严重 |
| `ProcessStartInfo` | 构造函数、所有属性 | 🔴 严重 |
| `Process` | `StandardInput`、`StandardOutput`、`StandardError` | 🔴 严重 |
| `Process` | `GetProcesses()`、`GetProcessesByName()` | 🟡 高 |

## 哪些类型被允许？

其他不涉及进程执行的 `System.Diagnostics` 类型仍然可用：

| 允许的类型 | 用途 | 为什么安全 |
|-----------|------|-----------|
| `Stopwatch` | 计时测量 | 不涉及进程执行 |
| `Debug` | 调试输出 | 无安全风险 |
| `Trace` | 跟踪/日志 | 无安全风险 |
| `PerformanceCounter` | 性能监控 | 只读、受审计 |

## 如何安全执行命令？

### 使用 CommandLineExecutor（唯一安全的方式）

`CommandLineExecutor` 是插件中命令执行的**受控入口**：

```csharp
// ✅ 正确：执行命令
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"错误: {result.Error}");
}
```

**CommandLineExecutor 提供的能力：**
1. **命令注入防护**：拦截危险分隔符（`||`、`&&`、`|`、`&`、`;`）
2. **超时强制**：默认 30 秒超时（可配置）
3. **审计日志**：所有命令执行均被记录供安全审查
4. **输出捕获**：自动捕获标准输出和标准错误
5. **跨平台支持**：Windows 上使用 `cmd.exe`，Unix 上使用 `/bin/bash`
6. **错误处理**：返回带有成功/失败状态的结构化结果

## 演示的违规操作

本插件展示了 5 种常见的进程执行违规：

### 违规 1：Process.Start

```csharp
// ❌ 禁止
Process.Start("notepad.exe");

// ✅ 正确
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**被拦截的 TypeRef**：`System.Diagnostics.Process::Start(System.String)`

### 违规 2：ProcessStartInfo

```csharp
// ❌ 禁止
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ 正确
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**被拦截的 TypeRef**：`System.Diagnostics.ProcessStartInfo::.ctor()`

### 违规 3：带参数的 Process

```csharp
// ❌ 禁止
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ 正确
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**被拦截的 TypeRef**：`System.Diagnostics.Process::Start(ProcessStartInfo)`

### 违规 4：进程输出重定向

```csharp
// ❌ 禁止
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ 正确
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**被拦截的 TypeRef**：`System.Diagnostics.Process::StandardOutput`

### 违规 5：Process.Kill

```csharp
// ❌ 禁止
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ 正确
// 出于安全原因，CommandLineExecutor 不支持终止进程。
// 如有需要请联系系统管理员。
```

**被拦截的 TypeRef**：`System.Diagnostics.Process::Kill()`

## 为什么只禁止 Process 而非整个 System.Diagnostics？

插件系统采用**精准打击**安全策略：

- **只拦截危险类型**：Process/ProcessStartInfo 能执行任意代码
- **允许安全类型**：Stopwatch、Debug、Trace 无安全隐患
- **最小影响**：开发者仍然可以使用不构成风险的诊断工具
- **清晰边界**：只有能生成/终止进程的类型才被禁止

这与 `System.IO` 不同——后者**整体被禁止**，因为大多数 IO 类型直接访问文件系统。

## 与其他示例的对比

| 示例 | 焦点 | 所需权限 |
|------|------|---------|
| **09-ForbiddenProcess** | 禁止的进程模式（本示例） | 不适用（被拦截） |
| **15-CapabilityProcess** | 声明式 Process 权限绕过 | `Capability.Process` |

**关键区别：**
- **09-ForbiddenProcess**：展示你不能做什么（直接进程执行）
- **15-CapabilityProcess**：展示如何声明式请求进程执行权限

## PluginLoader 安全机制

当 PluginLoader 扫描此插件时：

1. **TypeRef 扫描**：检测对禁止的 `Process`/`ProcessStartInfo` 类型的引用
2. **MemberRef 扫描**：检测对被拦截方法的调用（如 `Process.Start`）
3. **IL 字符串扫描**：检测基于字符串的反射尝试加载禁止类型
4. **拒绝**：插件在加载期间被拒绝，并给出详细错误信息

**无法通过以下方式绕过：**
- 字符串拼接（`"System.Diagnostics" + ".Process"`）
- 反射（`Type.GetType("System.Diagnostics.Process")`）
- 动态加载（`Assembly.Load`）
- 混淆或加密

这些绕过尝试会被 IL 级扫描捕获（见 **12-ForbiddenStringBypass**）。

## 最佳实践

1. **始终使用 CommandLineExecutor**：永远不要直接使用 `Process.Start`
2. **设置合理超时**：防止命令无限挂起
3. **检查结果**：使用输出前始终验证 `result.Success`
4. **净化输入**：永远不要将用户输入直接传递给命令
5. **必要时声明 Capability**：如果确实需要不受限制的进程执行，请声明 `Capability.Process`（见 15-CapabilityProcess）

## 文件

- `Plugin.cs` - 反例演示插件
- `README.md` - 英文（English）
- `README.zh-CN.md` - 本文件（简体中文）
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## 相关示例

- **08-ForbiddenNetwork**：禁止的网络操作
- **15-CapabilityProcess**：声明式 Process 权限
- **10-ForbiddenReflection**：禁止的反射操作
- **12-ForbiddenStringBypass**：基于字符串的反射绕过尝试
