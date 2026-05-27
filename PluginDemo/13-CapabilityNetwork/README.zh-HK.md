# PluginDemo-13：Capability.Network — 聲明式網路權限

## 概述

本插件演示如何使用 `[PluginCapability(Capability.Network)]` 聲明插件需要網路存取權限。聲明此能力後，插件可以存取 `System.Net.*` 型別，否則這些型別會被 PluginLoader 安全掃描封鎖。

## PluginCapability 聲明語法

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**關鍵要素：**
- **特性目標**：必須標註在直接實作 `IPlugin` 的類別上
- **AllowMultiple = true**：可以疊加多個 `[PluginCapability]` 特性（參見 17-CapabilityStacked）
- **Reason 欄位**：人類可讀的說明，在載入時寫入安全稽核日誌。**強烈建議所有生產插件提供清晰的 Reason。**

## PluginLoader 如何處理能力聲明

1. **PE 元資料讀取**：PluginLoader 在安全掃描開始**之前**從 PE 檔案的 CustomAttribute 表讀取能力聲明
2. **掃描規則放寬**：聲明的能力免除對應型別參考的禁止命名空間和禁止型別檢查
3. **稽核日誌**：所有聲明（包含 Reason）寫入安全稽核日誌
4. **不可聲明的 capability**：P/Invoke、Unsafe、Reflection.Emit 等無論是否聲明都始終被封鎖

## Capability.Network 放行範圍

### TypeRef 放行

聲明 `Capability.Network` 後，以下基於命名空間和型別的禁止規則被放寬：

| 放行命名空間 | 允許的型別 |
|-------------|-----------|
| `System.Net.Http` | `HttpClient`、`HttpRequestMessage`、`HttpResponseMessage` 等 |
| `System.Net.WebSockets` | `ClientWebSocket`、`WebSocket` 等 |
| `System.Net.Sockets` | `TcpClient`、`UdpClient`、`Socket` 等 |
| `System.Net.Mail` | `SmtpClient`、`MailMessage` 等 |
| `System.Net.NetworkInformation` | `Ping`、`NetworkInterface` 等 |
| `System.Net.Security` | `SslStream` 等 |
| `System.Net`（型別級禁止） | `HttpWebRequest`、`WebClient`、`Dns`、`FtpWebRequest` 等 |

### ILString 放行

以這些前綴開頭的字串常數不會在 #US 堆掃描中被標記：
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### 仍被禁止的 capability

即使聲明了 `Capability.Network`，以下能力**始終**被封鎖（不可聲明的 capability）：

| 類別 | 被封鎖的型別 | 原因 |
|------|------------|------|
| P/Invoke | `DllImportAttribute`、`Marshal`、`NativeMemory` | 執行時無法安全稽核 |
| Unsafe 程式碼 | `UnverifiableCodeAttribute`、`System.Runtime.CompilerServices.Unsafe` | 繞過型別安全保證 |
| IL 發射 | `System.Reflection.Emit.*` | 可在執行時產生任意程式碼 |
| 組件載入 | `System.Runtime.Loader`、`Assembly.Load*` | 可透過載入未檢查的 DLL 繞過安全掃描 |
| 登錄檔 | `Microsoft.Win32.*` | 外掛沙箱外的作業系統級系統存取 |

## Reason 欄位的稽核作用

`Reason` 欄位作為能力聲明的**稽核追蹤**：

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Reason 的重要性：**
1. **安全審查**：稽核人員可以驗證聲明的 capability 是否與實際插件行為一致
2. **最小權限原則**：強制插件作者說明為什麼需要每個能力
3. **合規性**：安全認證和事件調查所必需
4. **執行時監控**：安全工具可以在聲明能力使用超出說明理由時發出警報

## 與 08-ForbiddenNetwork 的對比

| 方面 | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|------|-------------------|---------------------|
| 聲明 | 無 | `[PluginCapability(Capability.Network)]` |
| 載入結果 | ❌ 被 PluginLoader 拒絕 | ✅ 成功載入 |
| HttpClient 使用 | 被 TypeRef 掃描封鎖 | 被能力聲明放行 |
| TcpClient 使用 | 被 TypeRef 掃描封鎖 | 被能力聲明放行 |
| Reason | 不適用 | 寫入稽核日誌 |

**關鍵區別**：08-ForbiddenNetwork 展示**不聲明**能力時使用網路型別的後果。13-CapabilityNetwork 展示聲明式請求網路存取的**正確**方式。

## 安全最佳實踐

1. **只聲明需要的**：如果你只需要 HTTP，不要因為可以就聲明 Capability.Network——但注意 Capability.Network 是唯一的網路相關能力，沒有更細粒度的選項
2. **優先使用 NetworkExecutor**：`NetworkExecutor` 是網路存取的受控入口，不需要任何能力聲明
3. **提供清晰的 Reason**：模糊的理由如「網路存取」在安全審查時是危險信號
4. **記住不可聲明的限制**：沒有任何能力聲明可以繞過 P/Invoke、Unsafe 或 Reflection.Emit 禁令

## 檔案

- `Plugin.cs` — 聲明 Capability.Network 的示例插件
- `README.md` — English
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 本檔案（繁體中文）
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

## 相關示例

- **08-ForbiddenNetwork**：被封鎖的網路操作反例
- **14-CapabilityFileIO**：聲明式 FileIO 能力
- **15-CapabilityProcess**：聲明式 Process 能力
- **16-CapabilityAI**：聲明式 AI 服務能力
- **17-CapabilityStacked**：多權限疊加
- **18-CapabilityDenied**：不可聲明能力反例
