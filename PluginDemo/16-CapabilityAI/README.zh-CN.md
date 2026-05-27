# PluginDemo-16: Capability.AI — 声明式 AI 服务权限

## 概述

本插件演示如何使用 `[PluginCapability(Capability.AI)]` 声明插件需要访问 AI 服务。与其他能力不同，`Capability.AI` **不会**放行任何禁止命名空间——而是使宿主可以向插件注入 `IAIService` 引用。

## 核心概念：Capability.AI 不授予网络访问

`Capability.AI` 与其他能力有本质区别：

| 能力 | 放行内容 | 工作方式 |
|------|---------|---------|
| `Capability.Network` | `System.Net.*` 命名空间 | 放宽 TypeRef/ILString 扫描规则 |
| `Capability.FileIO` | `System.IO` 命名空间 | 放宽 TypeRef/ILString 扫描规则 |
| `Capability.Process` | `Process*` 类型 | 放宽 TypeRef/ILString 扫描规则 |
| `Capability.AI` | **无** | 使能宿主注入 IAIService |

`IAIService` 位于 `SiliconLife.Collective` 命名空间——它从不在任何禁止列表中。能力声明是向宿主发出的**选择加入信号**，表示此插件应接收 AI 服务引用。

## 能力叠加：AI + Network

如果 AI 客户端需要直接网络访问（例如调用远程 AI 端点），必须声明**两个**能力：

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

详见 **17-CapabilityStacked** 的完整叠加示例。

## 受控入口模式

| 资源 | 受控入口 | 需要能力声明 |
|------|---------|------------|
| 文件 | `PermissionedStreamFactory` | 无 |
| 网络 | `NetworkExecutor` | 无 |
| 进程 | `CommandLineExecutor` | 无 |
| 数据存储 | `SpeedyPack` | 无 |
| AI 服务 | `IAIService` | `Capability.AI` |

`IAIService` 是独特的：它**需要**能力声明。这是因为 AI 服务访问是选择加入功能，不是所有插件的默认能力。

## 文件

- `Plugin.cs` — 声明 Capability.AI 的演示插件
- `README.md` — 本文件（英文）
- `README.zh-CN.md` — 简体中文
- 翻译版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相关示例

- **17-CapabilityStacked**：多权限叠加（Network + AI）
- **18-CapabilityDenied**：不可声明能力反例
