# CS 原始碼編譯載入模式範例

一個從原始 `.cs` 原始碼檔案而非預編譯 DLL 載入的外掛程式，示範 PluginLoader 的 CS 原始碼編譯模式（由 task-389 引入）。

## CS 原始碼模式運作原理

當 PluginLoader 掃描外掛目錄時發現**沒有 DLL**，它自動進入 CS 原始碼模式：

```
1. PluginLoader 掃描外掛目錄 → 無 DLL
2. 進入 CS 原始碼模式
3. 發現 cs.txt → 逐行讀取，只載入列出的 .cs 檔案
   （無 cs.txt → 載入目錄下所有 *.cs 檔案）
4. 掃描同級 DLL → 受信 DLL 直接新增為參照；
   非受信 DLL 需通過 ScanForbiddenReferences 安全掃描才能作為參照
5. CompilationCore（受限模式）編譯 .cs 檔案為記憶體 DLL
6. 記憶體 DLL 位元組寫入臨時檔案，走 ScanForbiddenReferences 安全掃描
7. 掃描通過 → 反射尋找 IPlugin 實作 → 實例化
8. 日誌顯示："Plugin loaded [CS-Source]: {Id} v{Version} from {DirName}"
```

## cs.txt — 選擇性載入白名單

`cs.txt` 檔案指定要編譯哪些 `.cs` 檔案，每行一個檔名：

```
Plugin.cs
```

- **列出的檔案**：被編譯和載入（如 `Plugin.cs`）
- **未列出的檔案**：編譯器忽略（如 `Helpers.cs`）
- **以 `#` 開頭的行**：視為註解
- **空行**：忽略
- **沒有 cs.txt**：載入目錄下所有 `*.cs` 檔案

本範例故意在 cs.txt 中只列出 `Plugin.cs`。`Helpers.cs` 存在於目錄中但**不**參與編譯，用於示範選擇性載入機制。

## 目錄結構

```
22-CSSourceMode/
├── Plugin.cs          — IPlugin 實作（在 cs.txt 中 → 被編譯）
├── Helpers.cs         — 輔助類別（不在 cs.txt 中 → 不被編譯）
├── cs.txt             — 白名單，指定編譯哪些 .cs 檔案
├── README.md          — 英文文件
├── README.zh-HK.md    — 繁體中文文件
└── ...                — 其他語言 README
```

## CS 原始碼模式 vs DLL 模式

| 方面 | DLL 模式 | CS 原始碼模式 |
|------|---------|--------------|
| 外掛格式 | 預編譯 `.dll` | 原始 `.cs` 原始碼檔案 |
| 載入觸發 | 外掛目錄中找到 DLL | 無 DLL，但有 `.cs` 檔案 |
| 編譯時機 | 建置時 | 載入時由 PluginLoader 編譯 |
| 效能 | 無編譯開銷 | 啟動時有 Roslyn 編譯開銷 |
| 安全掃描 | 直接 PE 元資料掃描 | 編譯 → 臨時 DLL → PE 元資料掃描 |
| 日誌前綴 | `Plugin loaded:` | `Plugin loaded [CS-Source]:` |
| 適用場景 | 生產部署 | 開發迭代 |

## 與 01-MinimalPlugin 的對比

| 方面 | 01-MinimalPlugin | 22-CSSourceMode |
|------|------------------|-----------------|
| 外掛格式 | 預編譯 DLL | CS 原始碼檔案 |
| 載入路徑 | 標準 DLL 載入 | CS 原始碼編譯 |
| 功能 | 等價 | 等價 |
| IPlugin.Id | `com.siliconlife.demo.minimal` | `com.siliconlife.demo.cssource` |

## 錯誤處理

| 場景 | 行為 |
|------|------|
| 無 DLL，無 .cs 檔案 | 警告："No DLL and no CS source files found" |
| 編譯錯誤 | 錯誤：記錄詳細診斷資訊 |
| 安全掃描失敗 | 錯誤：列出所有違規項，外掛被拒絕 |
| cs.txt 條目未找到 | 警告："cs.txt entry not found or not a .cs file" |
| 同級 DLL 掃描失敗 | 警告：DLL 不作為參照，編譯繼續 |

## 安全說明

CS 原始碼模式外掛接受與 DLL 模式外掛**相同的安全掃描**。編譯後的組件被寫入臨時 DLL 檔案，使用 `ScanForbiddenReferences` 掃描——與預編譯 DLL 接受的掃描完全相同。所有禁止的命名空間/類型/成員/字串規則同等適用。

外掛仍在隔離的上下文中載入，並掃描禁止的命名空間參照（如 `System.IO`、`System.Net.Http`）。詳見[安全文件](../../docs/zh-HK/security.md)。
