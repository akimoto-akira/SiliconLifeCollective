# 安全 System.IO 示例

演示 `SystemIOAllowedTypes` 白名單中的 System.IO 類型：`MemoryStream`、`BinaryReader`/`BinaryWriter`、`GZipStream`。說明為什麼 `FileStream` 必須透過 `PermissionedStreamFactory` 取得。

## SystemIOAllowedTypes 白名單

插件執行時預設阻止 `System.IO` 命名空間，但豁免**不直接執行檔案 I/O** 的類型：

| 類別 | 允許的類型 | 安全原因 |
|------|-----------|---------|
| 流抽象 | `Stream` | 抽象基底類別，本身無 I/O |
| 記憶體流 | `MemoryStream` | 純記憶體操作 |
| 壓縮流 | `GZipStream`、`DeflateStream`、`ZLibStream` | 包裝另一個流，不開啟檔案 |
| 二進位包裝器 | `BinaryReader`、`BinaryWriter` | 包裝任意流，不開啟檔案 |
| 列舉 | `SeekOrigin`、`FileMode`、`FileAccess`、`FileShare`、`CompressionMode`、`CompressionLevel` | 僅值類型 |
| 例外 | `IOException`、`InvalidDataException`、`EndOfStreamException` | 僅錯誤類型 |

### 不在白名單中的類型

這些類型**直接存取檔案系統**，在插件程式碼中被**阻止**：

| 被阻止的類型 | 阻止原因 | 安全替代方案 |
|-------------|---------|------------|
| `FileStream` | 直接開啟檔案 | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | 靜態檔案操作 | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | 靜態目錄操作 | `SafePath`（權限檢查） |
| `FileInfo` | 封裝檔案路徑 | `SafePath` |
| `DirectoryInfo` | 封裝目錄路徑 | `SafePath` |
| `StreamReader` | 直接開啟檔案 | `PermissionedStreamFactory` + 包裝 `PermissionedStream` |
| `StreamWriter` | 直接開啟檔案 | `PermissionedStreamFactory` + 包裝 `PermissionedStream` |

## 為什麼 FileStream 必須透過 PermissionedStreamFactory

`FileStream` 直接開啟磁碟檔案——在插件系統中是重大安全風險。`PermissionedStreamFactory` 強制執行：

1. **權限檢查** — 呼叫者的 `PermissionManager` 必須授予該路徑的 `FileAccess` 權限
2. **稽核日誌** — 每次檔案開啟都記錄呼叫者的 being ID
3. **路徑驗證** — 空/無效路徑在任何 I/O 之前被拒絕

```
❌ new FileStream("path", FileMode.Open)           → 被 TypeRef 掃描器阻止
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → 權限檢查通過
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → 權限檢查通過
```

## 示例管線

本示例僅使用白名單類型構建完整的記憶體資料管線：

```
┌─────────────────────────────────────────────────────────────────┐
│  示例 1: MemoryStream                                           │
│  └─ 寫入位元組 → 讀取位元組 → 解碼字串                           │
│                                                                  │
│  示例 2: 壓縮管線                                                │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(壓縮) → MemoryStream(壓縮後)                    │
│     → GZipStream(解壓) → MemoryStream(原始)                      │
│     → UTF8 → string (往返驗證)                                   │
│                                                                  │
│  示例 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → 驗證往返                       │
└─────────────────────────────────────────────────────────────────┘
```

## 本示例

> **⚠️ 注意：** 本示例**僅**使用 `SystemIOAllowedTypes` 白名單類型，不執行任何檔案 I/O。檔案存取請參見 `PermissionedStreamFactory` API。

| 類別 | 角色 |
|------|------|
| `SafeSystemIOPlugin` | `IPlugin` 實作 — 演示安全的 System.IO 用法 |

## 安全說明

`System.IO` 命名空間被插件 TypeRef 掃描器阻止，只有白名單類型可通過。實際檔案存取必須使用 `PermissionedStreamFactory`，它會執行權限檢查和稽核日誌。詳見[安全文件](../../docs/zh-HK/security.md)。
