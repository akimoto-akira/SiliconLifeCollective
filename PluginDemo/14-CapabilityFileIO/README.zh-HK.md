# PluginDemo-14：Capability.FileIO — 聲明式檔案 I/O 權限

## 概述

本插件演示如何使用 `[PluginCapability(Capability.FileIO)]` 聲明插件需要直接檔案系統存取權限。聲明此能力後，插件可以存取所有 `System.IO` 型別，不再受 `SystemIOAllowedTypes` 白名單限制。

## PluginCapability 聲明語法

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Capability.FileIO 的運作方式

1. **預設狀態**：`System.IO` 命名空間被整體禁止，僅 `SystemIOAllowedTypes` 白名單型別可用（MemoryStream、BinaryReader、GZipStream 等）
2. **聲明後**：整個 `System.IO` 命名空間禁令被解除——File、FileStream、Directory、StreamReader(string) 等全部可存取
3. **ILString 放行**：以 `"System.IO."` 開頭的字串常數不會被標記
4. **不可聲明限制**：P/Invoke、Unsafe、Reflection.Emit 等仍被阻止

## Capability.FileIO 放行範圍

### TypeRef 放行

所有 `System.IO` 型別都被放行：

| 類別 | 放行型別 |
|------|---------|
| 檔案操作 | `File`、`FileInfo` |
| 目錄操作 | `Directory`、`DirectoryInfo` |
| 串流型別 | `FileStream`、`StreamReader(path)`、`StreamWriter(path)` |
| 檔案系統 | `FileSystemWatcher`、`DriveInfo`、`Path` |

### ILString 放行

- 以 `"System.IO."` 開頭的字串不會被標記

### 仍被禁止的能力

| 類別 | 仍被阻止 |
|------|---------|
| P/Invoke | `DllImportAttribute`、`Marshal`、`NativeMemory` |
| 不安全程式碼 | `UnverifiableCodeAttribute`、`Unsafe` |
| IL 發射 | `System.Reflection.Emit.*` |
| 組件載入 | `System.Runtime.Loader`、`Assembly.Load*` |
| 登錄檔 | `Microsoft.Win32.*` |

## 與其他示例的對比

| 示例 | 聲明 | 檔案存取 | 說明 |
|------|------|---------|------|
| **04-SafeSystemIO** | 無 | MemoryStream、BinaryReader、GZipStream | 僅使用白名單型別 |
| **07-ForbiddenFileIO** | 無 | ❌ 被拒絕 | 反例展示 |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ 完整 System.IO 存取 | 本示例 |
| **20-SpeedyPack** | 無 | 透過 SpeedyPack API（無需 Capability） | 推薦的資料儲存方式 |

## 檔案存取優先級

1. **SpeedyPack** — 無需能力聲明。內建快取、WAL、交易。**推薦用於結構化資料儲存。**
2. **PermissionedStreamFactory** — 無需能力聲明。帶稽核的存取，含路徑驗證和存取控制。
3. **Capability.FileIO + 直接 System.IO** — 僅在上述方案不滿足需求時使用。

## 為什麼推薦 PermissionedStreamFactory / SpeedyPack？

即使擁有 `Capability.FileIO`，仍然推薦使用受控入口，因為：

1. **稽核追蹤**：所有存取都被記錄和追蹤
2. **路徑驗證**：防止目錄遍歷攻擊（`../`）
3. **存取控制**：工作區邊界強制執行
4. **資源追蹤**：防止串流洩漏和資源耗盡
5. **合規性**：使用受控存取模式更容易通過安全審查

## 安全最佳實踐

1. **僅在真正需要時聲明 FileIO**：能用 SpeedyPack 或 PermissionedStreamFactory 嗎？
2. **提供清晰的 Reason**："Direct log file access for audit trail" 比 "file access" 更好
3. **自行驗證路徑**：即使有 Capability.FileIO，使用前也要驗證所有檔案路徑
4. **使用 using 陳述式**：始終釋放 FileStream/StreamReader/StreamWriter
5. **最小權限原則**：只聲明插件真正需要的能力

## 檔案

- `Plugin.cs` — 聲明 Capability.FileIO 的演示插件
- `README.md` — 本檔案（英文）
- `README.zh-CN.md` — 簡體中文
- 翻譯版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相關示例

- **04-SafeSystemIO**：允許的記憶體 System.IO 型別（無需聲明）
- **07-ForbiddenFileIO**：被阻止的檔案操作反例
- **20-SpeedyPack**：無需任何能力聲明的推薦資料儲存
- **18-CapabilityDenied**：不可聲明能力反例
