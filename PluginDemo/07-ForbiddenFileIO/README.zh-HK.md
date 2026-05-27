# 禁止的檔案 I/O 操作反例

演示插件系統中**禁止的**檔案 I/O 操作。本示例作為反例參考，展示不應該做什麼，並為每個違規行為提供正確的替代方案。

## 為什麼 System.IO 被全域禁止？

整個 `System.IO` 命名空間在插件級別被阻止，因為直接檔案存取存在嚴重安全風險：

1. **未授權檔案存取**：插件可能讀取工作區外的敏感檔案（密碼、金鑰、個人資料）
2. **檔案覆寫攻擊**：惡意插件可能覆寫關鍵的系統或設定檔案
3. **目錄遍歷**：插件可能使用 `../` 路徑逃離工作區邊界
4. **資源耗盡**：不受控制的檔案建立可能填滿磁碟空間
5. **無稽核追蹤**：直接檔案操作繞過插件安全稽核系統

## 禁止的類型

所有直接存取檔案系統的 `System.IO` 類型都被阻止：

| 禁止的類型 | 被阻止的方法 | 風險級別 |
|-----------|-------------|---------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` 等 | 🔴 嚴重 |
| `FileStream` | 帶檔案路徑的建構函式 | 🔴 嚴重 |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 嚴重 |
| `StreamReader` | 帶檔案路徑（字串）的建構函式 | 🔴 嚴重 |
| `StreamWriter` | 帶檔案路徑（字串）的建構函式 | 🔴 嚴重 |
| `FileInfo` | 所有方法 | 🔴 嚴重 |
| `DirectoryInfo` | 所有方法 | 🔴 嚴重 |

## 允許的類型（白名單例外）

執行**純記憶體操作**（不直接存取檔案系統）的類型被允許：

| 允許的類型 | 用途 | 為什麼安全 |
|-----------|------|-----------|
| `MemoryStream` | 記憶體位元組流 | 無檔案系統存取 |
| `BinaryReader` | 從現有流讀取 | 包裝流，不開啟檔案 |
| `BinaryWriter` | 寫入現有流 | 包裝流，不建立檔案 |
| `GZipStream` | 壓縮/解壓縮 | 包裝流，無檔案存取 |
| `StreamReader` | 帶 `Stream` 參數的建構函式 | 包裝受稽核流時安全 |
| `StreamWriter` | 帶 `Stream` 參數的建構函式 | 包裝受稽核流時安全 |

詳見 **04-SafeSystemIO** 示例。

## 如何透過 PermissionedStreamFactory 安全存取檔案

`PermissionedStreamFactory` 是插件中檔案操作的**受控入口**：

```csharp
// ✅ 正確：讀取檔案
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ 正確：寫入檔案
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("日誌資料");
```

**PermissionedStreamFactory 提供：**
1. **路徑驗證**：防止目錄遍歷攻擊（`../`）
2. **權限檢查**：確保檔案在允許的工作區內
3. **稽核日誌**：所有檔案存取都被記錄以供安全審查
4. **資源清理**：追蹤開啟的流並防止洩漏

## 本示例中的違規行為

### 違規 1：File.ReadAllText

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ 正確替代
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### 違規 2：File.WriteAllText

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ 正確替代
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### 違規 3：直接 FileStream

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ 正確替代
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### 違規 4：Directory.GetFiles

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ 正確替代（使用 SpeedyPack）
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### 違規 5：帶直接路徑的 StreamReader

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ 正確替代
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## 與其他示例的對比

| 示例 | 重點 | 所需權限 |
|------|------|---------|
| **04-SafeSystemIO** | 允許的記憶體類型（MemoryStream、GZipStream） | 無 |
| **07-ForbiddenFileIO** | 禁止的檔案存取模式（本示例） | 不適用（被阻止） |
| **14-CapabilityFileIO** | 宣告 FileIO 能力以繞過限制 | `Capability.FileIO` |

## PluginLoader 安全掃描機制

當 PluginLoader 掃描此插件時：

1. **TypeRef 掃描**：偵測對禁止的 `System.IO` 類型的參照
2. **MemberRef 掃描**：偵測對被阻止方法的呼叫
3. **IL 字串掃描**：偵測基於字串的反射繞過嘗試
4. **拒絕**：插件在載入期間被拒絕，並提供詳細錯誤訊息

字串拼接、反射、動態載入、混淆等繞過手段均無效——這些會被 IL 級掃描捕獲（參見 **12-ForbiddenStringBypass**）。

## 安全說明

若你確實需要不受限制的檔案存取，可以宣告 `Capability.FileIO`（參見 14-CapabilityFileIO）。但最佳實踐是：
- 優先使用 **SpeedyPack** 儲存結構化資料（無需任何權限宣告）
- 必須存取檔案時使用 **PermissionedStreamFactory**（受控入口）
- 僅在上述方案無法滿足需求時才宣告 `Capability.FileIO`
