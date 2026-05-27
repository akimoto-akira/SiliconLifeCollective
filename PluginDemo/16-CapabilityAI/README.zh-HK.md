# PluginDemo-16：Capability.AI — 聲明式 AI 服務權限

## 概述

本插件演示如何使用 `[PluginCapability(Capability.AI)]` 聲明插件需要存取 AI 服務。與其他能力不同，`Capability.AI` **不會**放行任何禁止命名空間——而是使宿主可以向插件注入 `IAIService` 參考。

## 核心概念：Capability.AI 不授予網路存取

`Capability.AI` 與其他能力有本質區別：

| 能力 | 放行內容 | 運作方式 |
|------|---------|---------|
| `Capability.Network` | `System.Net.*` 命名空間 | 放寬 TypeRef/ILString 掃描規則 |
| `Capability.FileIO` | `System.IO` 命名空間 | 放寬 TypeRef/ILString 掃描規則 |
| `Capability.Process` | `Process*` 型別 | 放寬 TypeRef/ILString 掃描規則 |
| `Capability.AI` | **無** | 使能宿主注入 IAIService |

`IAIService` 位於 `SiliconLife.Collective` 命名空間——它從不在任何禁止列表中。能力聲明是向宿主發出的**選擇加入信號**，表示此插件應接收 AI 服務參考。

## 能力疊加：AI + Network

如果 AI 客戶端需要直接網路存取（例如呼叫遠端 AI 端點），必須聲明**兩個**能力：

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

詳見 **17-CapabilityStacked** 的完整疊加示例。

## 受控入口模式

| 資源 | 受控入口 | 需要能力聲明 |
|------|---------|------------|
| 檔案 | `PermissionedStreamFactory` | 無 |
| 網路 | `NetworkExecutor` | 無 |
| 程序 | `CommandLineExecutor` | 無 |
| 資料儲存 | `SpeedyPack` | 無 |
| AI 服務 | `IAIService` | `Capability.AI` |

`IAIService` 是獨特的：它**需要**能力聲明。這是因為 AI 服務存取是選擇加入功能，不是所有插件的預設能力。

## 檔案

- `Plugin.cs` — 聲明 Capability.AI 的演示插件
- `README.md` — 本檔案（英文）
- `README.zh-CN.md` — 簡體中文
- 翻譯版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相關示例

- **17-CapabilityStacked**：多權限疊加（Network + AI）
- **18-CapabilityDenied**：不可聲明能力反例
