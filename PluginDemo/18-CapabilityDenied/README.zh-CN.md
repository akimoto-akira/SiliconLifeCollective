# PluginDemo-18: 能力被拒 — 不可声明能力反例

## 概述

本插件是**反例**，演示声明能力**不能**绕过不可声明能力的禁令。即使声明了 `[PluginCapability(Capability.Network)]`，P/Invoke、Unsafe、Reflection.Emit 和注册表访问**始终**被阻止。

## 可声明 vs. 不可声明能力

### ✅ 可声明（Capability 枚举值存在）

| 能力 | 放行内容 |
|------|---------|
| `Capability.Network` | System.Net.* 命名空间和类型级禁止 |
| `Capability.FileIO` | System.IO 命名空间（超出白名单） |
| `Capability.Process` | System.Diagnostics 下 Process* 类型 |
| `Capability.AI` | 使能 IAIService 注入（无 TypeRef 放行） |

### ❌ 不可声明（无 Capability 枚举值）

| 类别 | 被阻止类型 | 不可声明原因 |
|------|-----------|------------|
| P/Invoke | `DllImportAttribute`、`Marshal`、`NativeMemory`、`NativeLibrary` | 运行时无法审计任意本地代码 |
| Unsafe 代码 | `UnverifiableCodeAttribute`、`System.Runtime.CompilerServices.Unsafe` | 绕过 CLR 类型安全和边界检查 |
| IL 发射 | `System.Reflection.Emit.*` | 可在运行时生成任意 IL |
| 程序集加载 | `System.Runtime.Loader`、`Assembly.Load*` | 可加载未扫描的 DLL，绕过安全扫描 |
| 注册表 | `Microsoft.Win32.*` | 插件沙箱外的 OS 级系统访问 |
| 动态编译 | `Microsoft.CodeAnalysis.*` | 可编译并执行任意代码 |
| 危险反射 | `Type.GetType(string)`、`Activator.CreateInstance` | 可通过字符串实例化被禁止的类型 |

## 为什么这些能力不可声明

根本原因：**运行时无法安全审计。**

1. **P/Invoke**：一旦调用本地代码，CLR 无法追踪执行内容——无安全保证
2. **Unsafe**：绕过插件安全模型所依赖的类型安全系统
3. **Reflection.Emit**：可在运行时生成未经 PluginLoader 扫描的新 IL
4. **AssemblyLoadContext**：可加载未经安全扫描的 DLL
5. **Registry**：提供对插件沙箱外的 OS 级配置的访问

## PluginLoader 的"声明无效"处理

当 PluginLoader 遇到能力声明时：

1. 从 CustomAttribute blob 中读取 int32 枚举值
2. 检查 `Enum.IsDefined(typeof(Capability), value)`
3. 如果值不是已定义的 Capability 成员 → **静默忽略**
4. 如果值已定义 → 应用放行规则
5. **不可声明检查始终强制执行**，无论声明了什么能力

这防止插件声明尚不存在的"未来"能力。

## 与 13-CapabilityNetwork 的对比

| 方面 | 13-CapabilityNetwork（正例） | 18-CapabilityDenied（反例） |
|------|---------------------------|---------------------------|
| 声明 | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| 使用 HttpClient | ✅ 放行 | ✅ 放行 |
| 使用 DllImport | 不适用 | ❌ 始终阻止 |
| 使用 Unsafe | 不适用 | ❌ 始终阻止 |
| 加载结果 | ✅ 成功 | ❌ 被拒绝 |

## 文件

- `Plugin.cs` — 演示不可声明能力反例的插件
- `README.md` — 本文件（英文）
- `README.zh-CN.md` — 简体中文
- 翻译版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相关示例

- **13-CapabilityNetwork**：Capability.Network 正例
- **11-ForbiddenPInvoke**：P/Invoke 反例（无能力可帮助）
- **10-ForbiddenReflection**：Reflection 反例（无能力可帮助）
