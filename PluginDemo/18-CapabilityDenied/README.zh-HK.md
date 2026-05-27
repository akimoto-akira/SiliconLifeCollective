# PluginDemo-18：能力被拒 — 不可聲明能力反例

## 概述

本插件是**反例**，演示聲明能力**不能**繞過不可聲明能力的禁令。即使聲明了 `[PluginCapability(Capability.Network)]`，P/Invoke、Unsafe、Reflection.Emit 和登錄檔存取**始終**被阻止。

## 可聲明 vs. 不可聲明能力

### ✅ 可聲明（Capability 列舉值存在）

| 能力 | 放行內容 |
|------|---------|
| `Capability.Network` | System.Net.* 命名空間和型別級禁止 |
| `Capability.FileIO` | System.IO 命名空間（超出白名單） |
| `Capability.Process` | System.Diagnostics 下 Process* 型別 |
| `Capability.AI` | 使能 IAIService 注入（無 TypeRef 放行） |

### ❌ 不可聲明（無 Capability 列舉值）

| 類別 | 被阻止型別 | 不可聲明原因 |
|------|-----------|------------|
| P/Invoke | `DllImportAttribute`、`Marshal`、`NativeMemory`、`NativeLibrary` | 執行時無法稽核任意本地程式碼 |
| Unsafe 程式碼 | `UnverifiableCodeAttribute`、`System.Runtime.CompilerServices.Unsafe` | 繞過 CLR 型別安全和邊界檢查 |
| IL 發射 | `System.Reflection.Emit.*` | 可在執行時生成任意 IL |
| 組件載入 | `System.Runtime.Loader`、`Assembly.Load*` | 可載入未掃描的 DLL，繞過安全掃描 |
| 登錄檔 | `Microsoft.Win32.*` | 插件沙箱外的 OS 級系統存取 |
| 動態編譯 | `Microsoft.CodeAnalysis.*` | 可編譯並執行任意程式碼 |
| 危險反射 | `Type.GetType(string)`、`Activator.CreateInstance` | 可透過字串實例化被禁止的型別 |

## 為什麼這些能力不可聲明

根本原因：**執行時無法安全稽核。**

1. **P/Invoke**：一旦呼叫本地程式碼，CLR 無法追蹤執行內容——無安全保證
2. **Unsafe**：繞過插件安全模型所依賴的型別安全系統
3. **Reflection.Emit**：可在執行時生成未經 PluginLoader 掃描的新 IL
4. **AssemblyLoadContext**：可載入未經安全掃描的 DLL
5. **Registry**：提供對插件沙箱外的 OS 級設定的存取

## PluginLoader 的「聲明無效」處理

當 PluginLoader 遇到能力聲明時：

1. 從 CustomAttribute blob 中讀取 int32 列舉值
2. 檢查 `Enum.IsDefined(typeof(Capability), value)`
3. 如果值不是已定義的 Capability 成員 → **靜默忽略**
4. 如果值已定義 → 套用放行規則
5. **不可聲明檢查始終強制執行**，無論聲明了什麼能力

這防止插件聲明尚不存在的「未來」能力。

## 與 13-CapabilityNetwork 的對比

| 方面 | 13-CapabilityNetwork（正例） | 18-CapabilityDenied（反例） |
|------|---------------------------|---------------------------|
| 聲明 | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| 使用 HttpClient | ✅ 放行 | ✅ 放行 |
| 使用 DllImport | 不適用 | ❌ 始終阻止 |
| 使用 Unsafe | 不適用 | ❌ 始終阻止 |
| 載入結果 | ✅ 成功 | ❌ 被拒絕 |

## 檔案

- `Plugin.cs` — 演示不可聲明能力反例的插件
- `README.md` — 本檔案（英文）
- `README.zh-CN.md` — 簡體中文
- 翻譯版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相關示例

- **13-CapabilityNetwork**：Capability.Network 正例
- **11-ForbiddenPInvoke**：P/Invoke 反例（無能力可幫助）
- **10-ForbiddenReflection**：Reflection 反例（無能力可幫助）
