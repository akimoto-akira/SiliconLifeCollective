# PluginDemo-17：能力疊加 — 多個聲明式權限

## 概述

本插件演示在單個插件類上疊加多個 `[PluginCapability]` 特性。`PluginCapabilityAttribute` 的 `AllowMultiple = true`，可以聲明任意數量的能力。

## 疊加語法

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## PluginLoader 如何處理疊加能力

1. 從 PE 元資料 CustomAttribute 表讀取**所有**聲明
2. **合併**所有聲明能力的放行規則
3. 每個聲明**獨立記錄**其 Reason 欄位
4. 仍然**強制執行**不可聲明能力的禁令

## 合併放行規則

疊加 `Capability.Network` + `Capability.AI` 時：

| 來源 | 放行內容 |
|------|---------|
| Capability.Network | System.Net.Http.*、System.Net.WebSockets.*、System.Net.Sockets.*、System.Net.Mail.*、System.Net.NetworkInformation.*、System.Net.Security.*、System.Net（按型別禁止） |
| Capability.AI | IAIService 注入啟用 |
| **合併結果** | 插件可以同時使用 HttpClient 和 IAIService |

## 疊加不等於無限權限

即使疊加多個能力，以下始終被阻止：

- ❌ P/Invoke（`DllImport`、`Marshal`、`NativeMemory`）
- ❌ 不安全程式碼（`UnverifiableCodeAttribute`、`Unsafe`）
- ❌ IL 發射（`System.Reflection.Emit.*`）
- ❌ 組件載入（`System.Runtime.Loader`、`Assembly.Load*`）
- ❌ 登錄檔（`Microsoft.Win32.*`）

`Capability` 列舉中沒有對應這些能力的值——它們是**設計上不可聲明**的。

## 稽核追蹤

每個能力獨立記錄：

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## 檔案

- `Plugin.cs` — 疊加 Capability.Network + Capability.AI 的演示插件
- `README.md` — 本檔案（英文）
- `README.zh-CN.md` — 簡體中文
- 翻譯版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相關示例

- **13-CapabilityNetwork**：單個 Network 能力
- **16-CapabilityAI**：單個 AI 能力
- **18-CapabilityDenied**：不可聲明能力反例
