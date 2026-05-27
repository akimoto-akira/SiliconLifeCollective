# PluginDemo-09: 禁止的進程操作反例

## 概述

本插件演示了 SiliconLife 插件系統中**被禁止**的進程執行操作。它作為反例參考，展示哪些操作不可做，並為每個違規提供正確的替代方案。

## 為什麼禁止 Process 類型？

`System.Diagnostics.Process` 和 `ProcessStartInfo` 在插件中被封禁，因為直接進程執行存在嚴重安全風險：

1. **任意命令執行**：插件可以在無審計或權限檢查的情況下運行任何命令
2. **惡意軟件啟動**：惡意插件可以執行不需要的應用程式或腳本
3. **系統資源存取**：進程可以存取插件沙箱之外的敏感系統資源
4. **無命令驗證**：直接 Process.Start 沒有內建的命令注入防護
5. **無審計追蹤**：直接進程操作繞過了插件安全審計系統
6. **權限提升**：可能以高於插件應有權限的級別生成進程

## 哪些類型被禁止？

只有 Process 相關類型被禁止，**不是整個 System.Diagnostics 命名空間**：

| 禁止的類型 | 被攔截的方法 | 風險等級 |
|------------|-------------|---------|
| `Process` | `Start()`、`Kill()`、`WaitForExit()` | 🔴 嚴重 |
| `ProcessStartInfo` | 建構函式、所有屬性 | 🔴 嚴重 |
| `Process` | `StandardInput`、`StandardOutput`、`StandardError` | 🔴 嚴重 |
| `Process` | `GetProcesses()`、`GetProcessesByName()` | 🟡 高 |

## 哪些類型被允許？

其他不涉及進程執行的 `System.Diagnostics` 類型仍然可用：

| 允許的類型 | 用途 | 為什麼安全 |
|-----------|------|-----------|
| `Stopwatch` | 計時測量 | 不涉及進程執行 |
| `Debug` | 偵錯輸出 | 無安全風險 |
| `Trace` | 追蹤/日誌 | 無安全風險 |
| `PerformanceCounter` | 效能監控 | 唯讀、受審計 |

## 如何安全執行命令？

### 使用 CommandLineExecutor（唯一安全的方式）

`CommandLineExecutor` 是插件中命令執行的**受控入口**：

```csharp
// ✅ 正確：執行命令
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"錯誤: {result.Error}");
}
```

**CommandLineExecutor 提供的能力：**
1. **命令注入防護**：攔截危險分隔符（`||`、`&&`、`|`、`&`、`;`）
2. **逾時強制**：預設 30 秒逾時（可配置）
3. **審計日誌**：所有命令執行均被記錄供安全審查
4. **輸出擷取**：自動擷取標準輸出和標準錯誤
5. **跨平台支援**：Windows 上使用 `cmd.exe`，Unix 上使用 `/bin/bash`
6. **錯誤處理**：回傳帶有成功/失敗狀態的結構化結果

## 演示的違規操作

本插件展示了 5 種常見的進程執行違規：

### 違規 1：Process.Start

```csharp
// ❌ 禁止
Process.Start("notepad.exe");

// ✅ 正確
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**被攔截的 TypeRef**：`System.Diagnostics.Process::Start(System.String)`

### 違規 2：ProcessStartInfo

```csharp
// ❌ 禁止
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ 正確
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**被攔截的 TypeRef**：`System.Diagnostics.ProcessStartInfo::.ctor()`

### 違規 3：帶參數的 Process

```csharp
// ❌ 禁止
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ 正確
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**被攔截的 TypeRef**：`System.Diagnostics.Process::Start(ProcessStartInfo)`

### 違規 4：進程輸出重導向

```csharp
// ❌ 禁止
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ 正確
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**被攔截的 TypeRef**：`System.Diagnostics.Process::StandardOutput`

### 違規 5：Process.Kill

```csharp
// ❌ 禁止
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ 正確
// 出於安全原因，CommandLineExecutor 不支援終止進程。
// 如有需要請聯繫系統管理員。
```

**被攔截的 TypeRef**：`System.Diagnostics.Process::Kill()`

## 為什麼只禁止 Process 而非整個 System.Diagnostics？

插件系統採用**精準打擊**安全策略：

- **只攔截危險類型**：Process/ProcessStartInfo 能執行任意程式碼
- **允許安全類型**：Stopwatch、Debug、Trace 無安全隱患
- **最小影響**：開發者仍然可以使用不構成風險的診斷工具
- **清晰邊界**：只有能生成/終止進程的類型才被禁止

這與 `System.IO` 不同——後者**整體被禁止**，因為大多數 IO 類型直接存取檔案系統。

## 與其他示例的對比

| 示例 | 焦點 | 所需權限 |
|------|------|---------|
| **09-ForbiddenProcess** | 禁止的進程模式（本示例） | 不適用（被攔截） |
| **15-CapabilityProcess** | 宣告式 Process 權限繞過 | `Capability.Process` |

## PluginLoader 安全機制

當 PluginLoader 掃描此插件時：

1. **TypeRef 掃描**：偵測對禁止的 `Process`/`ProcessStartInfo` 類型的參考
2. **MemberRef 掃描**：偵測對被攔截方法的呼叫（如 `Process.Start`）
3. **IL 字串掃描**：偵測基於字串的反射嘗試載入禁止類型
4. **拒絕**：插件在載入期間被拒絕，並給出詳細錯誤訊息

## 最佳實踐

1. **始終使用 CommandLineExecutor**：永遠不要直接使用 `Process.Start`
2. **設定合理逾時**：防止命令無限掛起
3. **檢查結果**：使用輸出前始終驗證 `result.Success`
4. **淨化輸入**：永遠不要將使用者輸入直接傳遞給命令
5. **必要時宣告 Capability**：如果確實需要不受限制的進程執行，請宣告 `Capability.Process`（見 15-CapabilityProcess）

## 文件

- `Plugin.cs` - 反例演示插件
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 本文件（繁體中文）
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## 相關示例

- **08-ForbiddenNetwork**：禁止的網路操作
- **15-CapabilityProcess**：宣告式 Process 權限
- **10-ForbiddenReflection**：禁止的反射操作
- **12-ForbiddenStringBypass**：基於字串的反射繞過嘗試
