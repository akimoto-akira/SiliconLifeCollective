# 工具參考

[English](../en/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | **繁體中文** | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md)

## 概述

工具系統允許矽基生命體通過標準化介面與外部世界互動。每個工具實現 `ITool` 介面，由 `ToolManager` 通過反射自動發現和註冊。

### 工具分類

- **系統管理工具** — 設定、權限、動態編譯
- **通訊工具** — 聊天、網路請求
- **資料儲存工具** — 磁碟操作、資料庫、記憶、工作筆記
- **時間管理工具** — 日曆、定時器、任務
- **開發工具** — 程式碼執行、日誌查詢
- **實用工具** — 系統資訊、Token 審計、說明文件、知識網路
- **瀏覽器工具** — WebView 瀏覽器自動化

---

## 內建工具列表

### 1. 日曆工具 (CalendarTool)

**工具名稱**: `calendar`

**功能描述**: 支援 32 種日曆系統的日期轉換和計算。

**支援的操作**:
- `now` — 獲取當前時間
- `format` — 格式化日期
- `add_days` — 日期加減
- `diff` — 計算日期差
- `list_calendars` — 列出所有支援的日曆
- `get_components` — 獲取日期元件
- `get_now_components` — 獲取當前時間元件
- `convert` — 日曆系統間轉換

**支援的日曆系統** (32 種):
- 公曆 (Gregorian)
- 中國農曆 (Chinese Lunar)
- 中國歷史曆法 (Chinese Historical) — 干支紀年、帝王年號
- 伊斯蘭曆 (Islamic)
- 希伯來曆 (Hebrew)
- 日本曆 (Japanese)
- 波斯曆 (Persian)
- 瑪雅曆 (Mayan)
- 佛曆 (Buddhist)
- 藏曆 (Tibetan)
- 等 24 種其他日曆...

**使用範例**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. 聊天工具 (ChatTool)

**工具名稱**: `chat`

**功能描述**: 管理聊天會話和訊息發送。

**支援的操作**:
- `send_message` — 發送訊息
- `get_messages` — 獲取歷史訊息
- `create_group` — 建立群聊
- `add_member` — 添加群成員
- `remove_member` — 移除群成員
- `get_chat_info` — 獲取聊天資訊
- `terminate_chat` — 終止聊天（已讀不回）

**使用範例**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "你好，讓我們協作吧！"
}
```

---

### 3. 設定工具 (ConfigTool)

**工具名稱**: `config`

**功能描述**: 讀取和修改系統設定。

**支援的操作**:
- `read` — 讀取設定項
- `write` — 寫入設定項
- `list` — 列出所有設定
- `get_ai_config` — 獲取 AI 客戶端設定
- `set_ai_config` — 設定 AI 客戶端設定

**使用範例**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. 主理人工具 (CuratorTool) 🔒

**工具名稱**: `curator`

**權限要求**: 僅限矽基主理人使用

**功能描述**: 矽基主理人專用的系統管理工具。

**支援的操作**:
- `create_being` — 建立新矽基生命體
- `list_beings` — 列出所有矽基生命體
- `get_being_info` — 獲取生命體資訊
- `assign_task` — 分派任務
- `manage_permissions` — 管理權限

**使用範例**:
```json
{
  "action": "create_being",
  "name": "助手",
  "soul_file": "assistant_soul.md"
}
```

---

### 5. 資料庫工具 (DatabaseTool)

**工具名稱**: `database`

**功能描述**: 結構化資料庫查詢和操作。

**支援的操作**:
- `query` — 查詢資料
- `insert` — 插入資料
- `update` — 更新資料
- `delete` — 刪除資料
- `create_table` — 建立表
- `list_tables` — 列出所有表

**使用範例**:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. 磁碟工具 (DiskTool)

**工具名稱**: `disk`

**功能描述**: 檔案系統操作和本地搜尋。

**支援的操作**:
- `read` — 讀取檔案
- `write` — 寫入檔案
- `list` — 列出目錄
- `delete` — 刪除檔案
- `create_directory` — 建立目錄
- `search_files` — 搜尋檔案
- `search_content` — 搜尋檔案內容
- `count_lines` — 統計行數
- `read_lines` — 讀取指定行
- `replace_text` — 替換文字

**權限要求**: `disk:read`, `disk:write`

**使用範例**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. 動態編譯工具 (DynamicCompileTool) 🔒

**工具名稱**: `compile`

**功能描述**: 動態編譯 C# 程式碼（用於矽基生命體自我進化）。

**支援的操作**:
- `compile_class` — 編譯類別
- `compile_callback` — 編譯權限回呼函式
- `validate_code` — 驗證程式碼安全性

**安全機制**:
- 編譯時引用控制（排除危險裝配）
- 執行時靜態程式碼掃描
- AES-256 加密儲存

**使用範例**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. 程式碼執行工具 (ExecuteCodeTool) 🔒

**工具名稱**: `execute_code`

**權限要求**: 僅限矽基主理人使用

**功能描述**: 編譯並執行 C# 程式碼片段。

**支援的操作**:
- `run_script` — 執行程式碼腳本

**使用範例**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. 說明工具 (HelpTool)

**工具名稱**: `help`

**功能描述**: 獲取系統說明文件和使用指南。

**支援的操作**:
- `get_topics` — 獲取說明主題列表
- `get_topic` — 獲取特定主題詳情
- `search` — 搜尋說明文件

**使用範例**:
```json
{
  "action": "get_topics"
}
```

---

### 10. 知識網路工具 (KnowledgeTool)

**工具名稱**: `knowledge`

**功能描述**: 知識圖譜操作（基於三元組：主體-關係-客體）。

**支援的操作**:
- `add` — 添加知識三元組
- `query` — 查詢知識
- `update` — 更新知識
- `delete` — 刪除知識
- `search` — 搜尋知識
- `get_path` — 獲取知識路徑
- `validate` — 驗證知識
- `stats` — 獲取統計資訊

**使用範例**:
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95
}
```

---

### 11. 日誌工具 (LogTool)

**工具名稱**: `log`

**功能描述**: 查詢操作歷史和對話歷史。

**支援的操作**:
- `query_logs` — 查詢系統日誌
- `query_conversations` — 查詢對話歷史
- `get_stats` — 獲取日誌統計

**使用範例**:
```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-26T23:59:59Z",
  "level": "info"
}
```

---

### 12. 記憶工具 (MemoryTool)

**工具名稱**: `memory`

**功能描述**: 管理矽基生命體的長期和短期記憶。

**支援的操作**:
- `read` — 讀取記憶
- `write` — 寫入記憶
- `search` — 搜尋記憶
- `delete` — 刪除記憶
- `list` — 列出記憶
- `get_stats` — 獲取記憶統計
- `compress` — 壓縮記憶

**使用範例**:
```json
{
  "action": "read",
  "key": "important_fact",
  "time_range": {
    "start": "2026-04-01",
    "end": "2026-04-26"
  }
}
```

---

### 13. 網路工具 (NetworkTool)

**工具名稱**: `network`

**功能描述**: 發起 HTTP/HTTPS 請求。

**支援的操作**:
- `get` — GET 請求
- `post` — POST 請求
- `put` — PUT 請求
- `delete` — DELETE 請求
- `download` — 下載檔案
- `upload` — 上傳檔案

**權限要求**: `network:http`

**使用範例**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. 權限工具 (PermissionTool) 🔒

**工具名稱**: `permission`

**權限要求**: 僅限矽基主理人使用

**功能描述**: 管理權限和存取控制清單。

**支援的操作**:
- `query_permission` — 查詢權限
- `manage_acl` — 管理全域 ACL
- `get_callback` — 獲取權限回呼函式
- `set_callback` — 設定權限回呼函式

**使用範例**:
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow"
}
```

---

### 15-23. 其他工具

系統還包含以下工具：

15. **ProjectTool** - 專案工作區管理
16. **ProjectTaskTool** - 專案任務管理
17. **ProjectWorkNoteTool** - 專案工作筆記
18. **SystemTool** - 系統資訊和資源監控
19. **TaskTool** - 個人任務管理
20. **TimerTool** - 定時器管理
21. **TokenAuditTool** 🔒 - Token 使用審計
22. **WebViewBrowserTool** - 瀏覽器自動化（基於 Playwright）
23. **WorkNoteTool** - 個人工作筆記

---

## 工具調用流程

```
┌──────────┐
│   AI     │ 返回 tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ 尋找和驗證工具使用權
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ 檢查權限鏈
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ 執行資源存取操作
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ 接收工具結果，繼續思考
└──────────┘
```

## 最佳實踐

### 1. 始終驗證參數

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("缺少必需參數: required_param");
}
```

### 2. 優雅處理錯誤

```csharp
try
{
    // 執行操作
}
catch (Exception ex)
{
    Logger.Error($"工具 {Name} 執行失敗: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. 尊重權限系統

永遠不要繞過權限檢查。始終通過執行器存取資源。

---

## 下一步

- 📚 閱讀[架構指南](architecture.md)
- 🛠️ 查看[開發指南](development-guide.md)
- 🔒 了解[權限系統](permission-system.md)
- 🚀 查看[快速開始指南](getting-started.md)
