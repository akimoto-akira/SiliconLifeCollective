# PluginDemo-17: 能力叠加 — 多个声明式权限

## 概述

本插件演示在单个插件类上叠加多个 `[PluginCapability]` 特性。`PluginCapabilityAttribute` 的 `AllowMultiple = true`，可以声明任意数量的能力。

## 叠加语法

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## PluginLoader 如何处理叠加能力

1. 从 PE 元数据 CustomAttribute 表读取**所有**声明
2. **合并**所有声明能力的放行规则
3. 每个声明**独立记录**其 Reason 字段
4. 仍然**强制执行**不可声明能力的禁令

## 合并放行规则

叠加 `Capability.Network` + `Capability.AI` 时：

| 来源 | 放行内容 |
|------|---------|
| Capability.Network | System.Net.Http.*、System.Net.WebSockets.*、System.Net.Sockets.*、System.Net.Mail.*、System.Net.NetworkInformation.*、System.Net.Security.*、System.Net（按类型禁止） |
| Capability.AI | IAIService 注入启用 |
| **合并结果** | 插件可以同时使用 HttpClient 和 IAIService |

## 叠加不等于无限权限

即使叠加多个能力，以下始终被阻止：

- ❌ P/Invoke（`DllImport`、`Marshal`、`NativeMemory`）
- ❌ 不安全代码（`UnverifiableCodeAttribute`、`Unsafe`）
- ❌ IL 发射（`System.Reflection.Emit.*`）
- ❌ 程序集加载（`System.Runtime.Loader`、`Assembly.Load*`）
- ❌ 注册表（`Microsoft.Win32.*`）

`Capability` 枚举中没有对应这些能力的值——它们是**设计上不可声明**的。

## 审计跟踪

每个能力独立记录：

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## 文件

- `Plugin.cs` — 叠加 Capability.Network + Capability.AI 的演示插件
- `README.md` — 本文件（英文）
- `README.zh-CN.md` — 简体中文
- 翻译版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相关示例

- **13-CapabilityNetwork**：单个 Network 能力
- **16-CapabilityAI**：单个 AI 能力
- **18-CapabilityDenied**：不可声明能力反例
