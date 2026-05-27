# PluginDemo-13: Capability.Network — 声明式网络权限

## 概述

本插件演示如何使用 `[PluginCapability(Capability.Network)]` 声明插件需要网络访问权限。声明此能力后，插件可以访问 `System.Net.*` 类型，否则这些类型会被 PluginLoader 安全扫描阻止。

## PluginCapability 声明语法

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**关键要素：**
- **特性目标**：必须标注在直接实现 `IPlugin` 的类上
- **AllowMultiple = true**：可以叠加多个 `[PluginCapability]` 特性（参见 17-CapabilityStacked）
- **Reason 字段**：人类可读的说明，在加载时写入安全审计日志。**强烈建议所有生产插件提供清晰的 Reason。**

## PluginLoader 如何处理能力声明

1. **PE 元数据读取**：PluginLoader 在安全扫描开始**之前**从 PE 文件的 CustomAttribute 表读取能力声明
2. **扫描规则放宽**：声明的能力免除对应类型引用的禁止命名空间和禁止类型检查
3. **审计日志**：所有声明（包括 Reason）写入安全审计日志
4. **不可声明的能力**：P/Invoke、Unsafe、Reflection.Emit 等无论是否声明都始终被阻止

## Capability.Network 放行范围

### TypeRef 放行

声明 `Capability.Network` 后，以下基于命名空间和类型的禁止规则被放宽：

| 放行命名空间 | 允许的类型 |
|-------------|-----------|
| `System.Net.Http` | `HttpClient`、`HttpRequestMessage`、`HttpResponseMessage` 等 |
| `System.Net.WebSockets` | `ClientWebSocket`、`WebSocket` 等 |
| `System.Net.Sockets` | `TcpClient`、`UdpClient`、`Socket` 等 |
| `System.Net.Mail` | `SmtpClient`、`MailMessage` 等 |
| `System.Net.NetworkInformation` | `Ping`、`NetworkInterface` 等 |
| `System.Net.Security` | `SslStream` 等 |
| `System.Net`（类型级禁止） | `HttpWebRequest`、`WebClient`、`Dns`、`FtpWebRequest` 等 |

### ILString 放行

以这些前缀开头的字符串常量不会在 #US 堆扫描中被标记：
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### 仍被禁止的能力

即使声明了 `Capability.Network`，以下能力**始终**被阻止（不可声明的能力）：

| 类别 | 被阻止的类型 | 为什么不可声明 |
|------|------------|--------------|
| P/Invoke | `DllImportAttribute`、`Marshal`、`NativeMemory` | 运行时无法安全审计 |
| Unsafe 代码 | `UnverifiableCodeAttribute`、`System.Runtime.CompilerServices.Unsafe` | 绕过类型安全保证 |
| IL 发射 | `System.Reflection.Emit.*` | 可在运行时生成任意代码 |
| 程序集加载 | `System.Runtime.Loader`、`Assembly.Load*` | 可通过加载未检查的 DLL 绕过安全扫描 |
| 注册表 | `Microsoft.Win32.*` | 插件沙箱外的操作系统级系统访问 |

## Reason 字段的审计作用

`Reason` 字段作为能力声明的**审计跟踪**：

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Reason 的重要性：**
1. **安全审查**：审计人员可以验证声明的能力是否与实际插件行为匹配
2. **最小权限原则**：强制插件作者说明为什么需要每个能力
3. **合规性**：安全认证和事件调查所必需
4. **运行时监控**：安全工具可以在声明能力使用超出说明理由时发出警报

## 与 08-ForbiddenNetwork 的对比

| 方面 | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|------|-------------------|---------------------|
| 声明 | 无 | `[PluginCapability(Capability.Network)]` |
| 加载结果 | ❌ 被 PluginLoader 拒绝 | ✅ 成功加载 |
| HttpClient 使用 | 被 TypeRef 扫描阻止 | 被能力声明放行 |
| TcpClient 使用 | 被 TypeRef 扫描阻止 | 被能力声明放行 |
| Reason | 不适用 | 写入审计日志 |

**关键区别**：08-ForbiddenNetwork 展示**不声明**能力时使用网络类型的后果。13-CapabilityNetwork 展示声明式请求网络访问的**正确**方式。

## 安全最佳实践

1. **只声明需要的**：如果你只需要 HTTP 访问，不要因为可以就声明 Capability.Network——但注意 Capability.Network 是唯一的网络相关能力，没有更细粒度的选项
2. **优先使用 NetworkExecutor**：`NetworkExecutor` 是网络访问的受控入口，不需要任何能力声明
3. **提供清晰的 Reason**：模糊的理由如"网络访问"在安全审查时是危险信号
4. **记住不可声明的限制**：没有任何能力声明可以绕过 P/Invoke、Unsafe 或 Reflection.Emit 禁令

## 文件

- `Plugin.cs` — 声明 Capability.Network 的示例插件
- `README.md` — 英文
- `README.zh-CN.md` — 本文件（简体中文）
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 日本語
- `README.ko-KR.md` — 한국어
- `README.de-DE.md` — Deutsch
- `README.fr-FR.md` — Français
- `README.es-ES.md` — Español
- `README.it-IT.md` — Italiano
- `README.ru-RU.md` — Русский
- `README.pt-PT.md` — Português
- `README.pl-PL.md` — Polski
- `README.cs-CZ.md` — Čeština

## 相关示例

- **08-ForbiddenNetwork**：被阻止的网络操作反例
- **14-CapabilityFileIO**：声明式 FileIO 能力
- **15-CapabilityProcess**：声明式 Process 能力
- **16-CapabilityAI**：声明式 AI 服务能力
- **17-CapabilityStacked**：多权限叠加
- **18-CapabilityDenied**：不可声明能力反例
