# PluginDemo-15：Capability.Process — 聲明式程序權限

## 概述

本插件演示如何使用 `[PluginCapability(Capability.Process)]` 聲明插件需要啟動子程序的能力。聲明此能力後，插件可以存取 `System.Diagnostics.Process` 及相關型別。

## 聲明語法

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Capability.Process 放行範圍

### TypeRef 放行

僅放行 `System.Diagnostics` 下 Process 相關型別：

| 放行型別 | 用途 |
|---------|------|
| `Process` | 啟動、管理和監控子程序 |
| `ProcessStartInfo` | 配置程序啟動參數 |
| `ProcessThread` | 存取程序執行緒資訊 |
| `ProcessModule` | 存取程序模組資訊 |
| `ProcessPriorityClass` | 設定程序優先級 |
| `ProcessWindowStyle` | 配置程序視窗樣式 |

始終允許的型別（從不在禁止列表中）：`Stopwatch`、`Debug`、`Trace`、`Activity`

### ILString 放行

- 以 `"System.Diagnostics.Process"` 開頭的字串不會被標記

## 與 09-ForbiddenProcess 的對比

| 方面 | 09-ForbiddenProcess | 15-CapabilityProcess |
|------|-------------------|---------------------|
| 聲明 | 無 | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ 被拒絕 | ✅ 允許 |
| ProcessStartInfo | ❌ 被拒絕 | ✅ 允許 |

## 推薦：CommandLineExecutor

即使有 `Capability.Process`，推薦優先使用 `CommandLineExecutor`：

| 特性 | CommandLineExecutor | 直接 Process |
|------|-------------------|-------------|
| 需要能力聲明 | 否 | 是 |
| 沙箱 | 命令白名單 | 無 |
| 超時 | 內建 | 手動 |
| 輸出捕獲 | 結構化 | 手動 |
| 稽核日誌 | 自動 | 手動 |

當需要對 I/O 串流進行精細控制、處理程序事件，或 CommandLineExecutor 的命令白名單過於嚴格時，才使用 `Capability.Process` + 直接 `Process`。

## 安全最佳實踐

1. **優先使用 CommandLineExecutor**：盡可能使用受控入口
2. **提供清晰的 Reason**："Launch build tools for CI pipeline" 而非模糊的 "process access"
3. **驗證所有輸入**：永遠不要將不受信任的輸入直接傳遞給 ProcessStartInfo
4. **使用 WaitForExit**：始終等待程序完成以防殭屍程序
5. **重導向串流**：設定 `RedirectStandardOutput = true` 和 `UseShellExecute = false`

## 檔案

- `Plugin.cs` — 聲明 Capability.Process 的演示插件
- `README.md` — 本檔案（英文）
- `README.zh-CN.md` — 簡體中文
- 翻譯版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相關示例

- **09-ForbiddenProcess**：被阻止的程序操作反例
- **18-CapabilityDenied**：不可聲明能力反例
