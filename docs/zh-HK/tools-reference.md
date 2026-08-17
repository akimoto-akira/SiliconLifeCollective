# 工具參考

> **版本：v0.2.0-alpha**

本文件詳細介紹 Silicon Life Collective 平台的所有內建工具。

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | **繁體中文** | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Русский](../ru-RU/tools-reference.md)

## 概述

工具系統允許矽基生命體透過標準化介面與外部世界互動。每個工具實作 `ITool` 介面，由 `ToolManager` 透過反射自動發現和註冊。

### 工具分類

- **系統管理工具** — 設定、權限、動態編譯、主理人管理
- **通訊工具** — 聊天、網路請求
- **資料儲存工具** — 磁碟操作、資料庫、記憶、工作筆記
- **時間管理工具** — 日曆、定時器、任務
- **開發工具** — 程式碼執行、日誌查詢
- **實用工具** — 系統資訊、Token 審計、說明文件、知識網絡
- **瀏覽器工具** — WebView 瀏覽器自動化
- **專案工具** — 專案管理、專案任務、專案工作筆記、專案工作
- **擴充工具** — MCP 外部伺服器工具、技能管理
- **外掛程式工具** — 透過外掛程式系統註冊的第三方工具

### 工具場景系統

每個工具透過 `[ToolScenario]` 屬性宣告其可用場景：

| 場景標誌 | 值 | 描述 |
|----------|------|-------------|
| `Chat` | `1 << 0` | 聊天場景（使用者與矽基生命體對話時） |
| `Task` | `1 << 1` | 任務場景（矽基生命體執行任務時） |
| `Timer` | `1 << 2` | 定時器場景（矽基生命體執行定時任務時） |
| `MemoryCompression` | `1 << 3` | 記憶壓縮場景 |
| `Project` | `1 << 4` | 專案場景（ThinkOnProject 模式） |
| `All` | 上述所有 | 所有場景均可用 |

此外，`[ChatOnly]` 屬性標記的工具僅在聊天場景可用（如 HelpTool），不會出現在任務和定時器場景中。

---

## 內建工具列表

### 1. 日曆工具 (CalendarTool)

**工具名稱**: `calendar`

**功能描述**: 支援 32 種日曆系統的日期轉換和計算。

**支援的操作**:
- `now` — 取得目前時間
- `format` — 格式化日期
- `add_days` — 日期加減
- `diff` — 計算日期差
- `list_calendars` — 列出所有支援的日曆
- `get_components` — 取得日期元件
- `get_now_components` — 取得目前時間元件
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

**可用場景**: Chat（`[ChatOnly]`，僅在聊天場景可用）

**功能描述**: 管理聊天會話和訊息傳送。

**支援的操作**:
- `send` — 傳送訊息到指定會話
- `mark_read` — 標記訊息為已讀

**使用範例**:
```json
{
  "action": "send",
  "channel_id": "session-uuid",
  "content": "你好，讓我們協作吧！"
}
```

---

### 3. 設定工具 (ConfigTool)

**工具名稱**: `config`

**功能描述**: 讀取系統設定資訊。

**支援的操作**:
- `get_all` — 取得所有設定項
- `get_group` — 取得指定分組的設定項
- `get_field` — 取得指定設定欄位
- `get_enum_values` — 取得列舉類型的可選值（如可用模型、區域等）

**使用範例**:
```json
{
  "action": "get_field",
  "group": "AIClients.Ollama",
  "field": "Model"
}
```

---

### 4. 主理人工具 (CuratorTool) 🔒

**工具名稱**: `silicon_manager`

**權限要求**: 僅限矽基主理人使用（`[SiliconManagerOnly]`）

**可用場景**: Chat、Task、Timer

**功能描述**: 矽基主理人專用的系統管理工具，用於管理矽基生命體的建立、檢視和重置。

**支援的操作**:
- `list_beings` — 列出所有矽基生命體及其狀態
- `create_being` — 建立新矽基生命體（需要 `name` 和 `soul` 參數）
- `get_code` — 檢視矽基生命體的自訂原始碼
- `reset` — 將矽基生命體重置為預設實作

**使用範例**:
```json
{
  "action": "create_being",
  "name": "助手",
  "soul": "你是一個有用的助手..."
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
- `create_table` — 建立資料表
- `list_tables` — 列出所有資料表

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

**功能描述**: 檔案系統操作和本機搜尋。

**支援的操作**:
- `read` — 讀取檔案
- `write` — 寫入檔案
- `list` — 列出目錄
- `delete` — 刪除檔案
- `create_directory` — 建立目錄
- `search_files` — 搜尋檔案
- `search_content` — 搜尋檔案內容
- `count_lines` — 統計檔案行數
- `read_lines` — 讀取指定行範圍
- `replace_text` — 替換文字
- `clear_file` — 清空檔案內容
- `replace_lines` — 替換指定行範圍
- `append` — 附加內容到檔案

**權限要求**: `FileAccess`

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
- 編譯時引用控制（排除危險組件）
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

### 9. 幫助工具 (HelpTool)

**工具名稱**: `help`

**可用場景**: Chat（`[ChatOnly]`，僅在聊天場景可用）

**功能描述**: 搜尋和取得系統說明文件內容，允許 AI 查詢系統功能使用方法。

**支援的操作**:
- `list` — 列出所有說明主題 ID
- `search` — 按關鍵詞搜尋說明文件
- `get` — 取得指定 ID 的說明文件內容

**使用範例**:
```json
{
  "action": "search",
  "keyword": "權限"
}
```

---

### 10. 知識網絡工具 (KnowledgeTool)

**工具名稱**: `knowledge`

**功能描述**: 知識圖譜操作（基於三元組：主體-關係-客體）。

**支援的操作**:
- `add` — 新增知識三元組
- `query` — 查詢知識
- `update` — 更新知識
- `delete` — 刪除知識
- `search` — 搜尋知識
- `get_path` — 取得知識路徑
- `validate` — 驗證知識
- `stats` — 取得統計資訊

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

### 11. MCP 查詢工具 (McpTool)

**工具名稱**: `mcp`

**功能描述**: 查詢 MCP（Model Context Protocol）整合狀態——已連線的外部伺服器、它們提供的工具以及如何呼叫。這是唯讀工具：伺服器的新增/刪除只能由使用者透過 Web UI 完成，AI 無法修改伺服器清單。

**支援的操作**:
- `status` — 全域概覽（啟用狀態、伺服器數量、工具數量）
- `list_servers` — 列出已設定的伺服器（含連線狀態和工具數量）
- `list_tools` — 列出可用工具（帶 `mcp_{server}_{tool}` 前綴名、描述和參數 schema；可選 `server_id` 過濾單一伺服器）

**使用範例**:
```json
{
  "action": "list_tools",
  "server_id": "filesystem",
  "include_schema": true
}
```

**MCP 包裝工具**: 每個已連線 MCP 伺服器提供的工具會以獨立工具形式動態註冊到矽基生命體，命名格式為 `mcp_{serverId}_{toolName}`（如 `mcp_filesystem_read_file`）。AI 可以像呼叫普通工具一樣直接按前綴名呼叫它們，無需透過本查詢工具中轉。包裝工具在權限矩陣中以單一 `execute` 動作呈現，可被逐個停用。

**場景**: 所有場景（`All`）

---

### 12. 日誌工具 (LogTool)

**工具名稱**: `log`

**功能描述**: 查詢操作歷史、工具呼叫歷史和對話歷史。

**支援的操作**:
- `query_operations` — 查詢操作歷史
- `query_tool_calls` — 查詢工具呼叫歷史
- `query_conversations` — 查詢對話歷史
- `export` — 匯出日誌
- `get_system_info` — 取得系統資訊

**使用範例**:
```json
{
  "action": "query_operations",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-26T23:59:59Z"
}
```

---

### 13. 記憶工具 (MemoryTool)

**工具名稱**: `memory`

**功能描述**: 管理矽基生命體的長期和短期記憶。

**支援的操作**:
- `add` — 新增記憶
- `recent` — 取得最近的記憶
- `query` — 搜尋記憶
- `stats` — 取得記憶統計

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

### 14. 網路工具 (NetworkTool)

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

### 15. 權限工具 (PermissionTool) 🔒

**工具名稱**: `permission`

**權限要求**: 僅限矽基主理人使用

**功能描述**: 管理權限和存取控制清單。

**支援的操作**:
- `query_permission` — 查詢權限
- `manage_acl` — 管理全域 ACL
- `get_callback` — 取得權限回呼函式
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

### 16. 專案工具 (ProjectTool) 🔒

**工具名稱**: `project`

**權限要求**: 僅限矽基主理人使用（`[SiliconManagerOnly]`）

**可用場景**: Chat、Task、Timer

**功能描述**: 管理專案工作區，支援專案生命週期管理、成員分配和角色管理。

**支援的操作**:
- `create` — 建立新專案空間
- `archive` — 封存專案
- `restore` — 還原已封存的專案
- `destroy` — 銷毀專案並清理資料（不可復原）
- `list` — 列出所有專案
- `get` — 取得專案詳情
- `assign` — 將矽基生命體分配到專案
- `remove` — 從專案中移除矽基生命體
- `update` — 更新專案名稱/描述
- `list-workflow-templates` — 列出可用的工作流程範本
- `assign_role` — 為矽基生命體分配專案角色
- `remove_role` — 移除矽基生命體的專案角色
- `list_roles` — 列出專案的角色分配

**使用範例**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "專案描述"
}
```

---

### 17. 專案任務工具 (ProjectTaskTool)

**工具名稱**: `project_task`

**可用場景**: Chat、Task、Timer

**功能描述**: 管理專案空間內的任務，支援完整的任務生命週期。

**支援的操作**:
- `create` — 建立專案任務
- `list` — 列出專案任務
- `get` — 取得任務詳情
- `update` — 更新任務標題/描述/優先順序
- `assign` — 為任務分配負責人
- `remove_assignee` — 移除任務負責人
- `start` — 開始任務
- `complete` — 標記任務完成
- `fail` — 標記任務失敗
- `cancel` — 取消任務
- `delete` — 刪除任務
- `stats` — 取得任務統計

**使用範例**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "完成任務描述",
  "priority": 5
}
```

---

### 18. 專案工作筆記工具 (ProjectWorkNoteTool)

**工具名稱**: `project_work_note`

**可用場景**: Chat、Task、Timer

**功能描述**: 管理專案空間內的工作筆記（公開，類似工作本），支援頁面式筆記管理。

**支援的操作**:
- `create` — 建立筆記頁面（需要 `project_id`、`summary` 和 `content`，可選 `keywords`）
- `read` — 讀取筆記頁面（需要 `project_id` 和 `page_number` 或 `note_id`）
- `update` — 更新筆記頁面（需要 `project_id`、`page_number` 和 `content`，可選 `summary` 和 `keywords`）
- `delete` — 刪除筆記頁面（需要 `project_id` 和 `page_number` 或 `note_id`）
- `list` — 列出專案的所有筆記頁面摘要
- `directory` — 產生筆記目錄/概覽
- `search` — 按關鍵詞搜尋筆記（需要 `project_id` 和 `keyword`，可選 `max_results`）

**使用範例**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "完成使用者認證模組",
  "content": "## 實作細節\n\n- 使用 JWT token",
  "keywords": "認證,JWT"
}
```

---

### 19. 專案工作工具 (ProjectWorkTool) 🔒

**工具名稱**: `project_work`

**權限要求**: 僅限矽基主理人使用（`[SiliconManagerOnly]`）

**可用場景**: Project（`[ToolScenario(ToolScenarioFlag.Project)]`，僅在專案場景可用）

**功能描述**: 專案工作操作工具，用於主理人在 ThinkOnProject 場景中管理專案工作流程。

**支援的操作**:
- `create-task` — 建立專案任務
- `assign-task` — 為任務分配矽基生命體
- `chat` — 傳送訊息到專案群聊
- `broadcast` — 廣播訊息到專案頻道
- `complete` — 標記專案為已完成
- `status` — 取得專案狀態

**使用範例**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "實作使用者認證"
}
```

---

### 20. 技能工具 (SkillTool)

**工具名稱**: `skill`

**功能描述**: 管理矽基生命體的技能（可重複使用的「工具編排 + 提示詞範本」能力單元），支援建立、列出、更新、刪除、匯入匯出。缺失的中繼資料（id、描述、參數 schema 等）會由 AI 自動補全。

**支援的操作**:
- `create` — 建立新技能（需要 `id` 和 `system_prompt`，可選 `description`、`parameter_schema`、`tool_whitelist`、`tags`、`max_tool_round`、`timeout`、`on_complete`、`trigger_mode`、`auto_trigger_condition`）
- `list` — 列出所有可用技能（含摘要）
- `update` — 透過參數更新已有技能（需要 `skill_id`）
- `update_from_md` — 從 Markdown 字串更新技能（YAML 前置中繼資料 + 提示詞內文）
- `delete` — 刪除技能（需要 `skill_id`）
- `export` — 匯出技能為 JSON（需要 `skill_id`）
- `export_md` — 匯出技能為 Markdown（需要 `skill_id`）
- `import` — 從 JSON 匯入技能（需要 `json`）
- `import_md` — 從 Markdown 匯入技能（需要 `markdown`）

**使用範例**:
```json
{
  "action": "create",
  "id": "daily_news_digest",
  "description": "搜尋今日科技新聞並產生摘要",
  "system_prompt": "請使用 network 工具搜尋 {topic} 的最新新聞，並產生一份 500 字摘要。",
  "parameter_schema": {
    "type": "object",
    "properties": {
      "topic": { "type": "string", "description": "新聞主題" }
    },
    "required": ["topic"]
  },
  "tool_whitelist": ["network", "work_note"],
  "trigger_mode": "Auto",
  "auto_trigger_condition": "schedule",
  "metadata": { "schedule": "0 9 * * *" }
}
```

**修改權限**: 矽基主理人可修改所有技能；普通生命體只能修改來源為 `Being` 或 `User` 的技能（不能修改內建與外掛程式技能）。

**數量限制**: 每個生命體的自訂技能數受設定 `MaxCustomSkillsPerBeing`（預設 50）限制。

**場景**: 所有場景（`All`）

> 關於技能系統（觸發模式、白名單、熱重載、自動排程等）的完整說明，參見 [矽基生命體指南](silicon-being-guide.md#技能系統)。

---

### 21. 系統工具 (SystemTool)

**工具名稱**: `system`

**功能描述**: 取得系統資訊和資源使用情況。

**支援的操作**:
- `info` — 取得系統資訊
- `resource_usage` — 取得資源使用情況
- `find_process` — 尋找處理程序
- `list_beings` — 列出矽基生命體

**使用範例**:
```json
{
  "action": "info"
}
```

---

### 22. 任務工具 (TaskTool)

**工具名稱**: `task`

**功能描述**: 管理矽基生命體個人任務。

**支援的操作**:
- `create` — 建立任務
- `list` — 列出任務
- `update` — 更新任務
- `complete` — 完成任務
- `delete` — 刪除任務
- `get_dependencies` — 取得相依關係

**使用範例**:
```json
{
  "action": "create",
  "description": "審查程式碼",
  "priority": 5
}
```

---

### 23. 定時器工具 (TimerTool)

**工具名稱**: `timer`

**功能描述**: 建立和管理定時器。

**支援的操作**:
- `create` — 建立定時器
- `list` — 列出定時器
- `delete` — 刪除定時器
- `pause` — 暫停定時器
- `resume` — 恢復定時器
- `get_execution_history` — 取得執行歷史

**使用範例**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "每小時提醒"
}
```

---

### 24. Token 審計工具 (TokenAuditTool) 🔒

**工具名稱**: `token_audit`

**權限要求**: 僅限矽基主理人使用（`[SiliconManagerOnly]`）

**可用場景**: Chat、Task、Timer

**功能描述**: 查詢 AI Token 使用統計和趨勢資料。

**支援的操作**:
- `summary` — 取得 Token 使用彙總統計
- `trend` — 取得 Token 使用趨勢資料點

**支援的時間範圍**:
- `today` — 最近 24 小時
- `week` — 最近 7×24 小時
- `month` — 按天統計
- `year` — 按月統計

**使用範例**:
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 25. WebView 瀏覽器工具 (WebViewBrowserTool)

**工具名稱**: `webview_browser`

**可用場景**: Chat、Task、Timer

**功能描述**: 基於 Playwright 的瀏覽器自動化操作，提供完整的網頁導覽、互動和資料提取能力。

**支援的操作**:
- `open` — 開啟瀏覽器
- `close` — 關閉瀏覽器
- `navigate` — 導覽到 URL
- `click` — 點擊元素
- `input` — 輸入文字
- `scroll` — 捲動頁面
- `execute_script` — 執行 JavaScript
- `get_page_text` — 取得頁面文字
- `get_screenshot` — 取得螢幕截圖
- `wait_for_element` — 等待元素出現
- `get_element_info` — 取得元素資訊
- `upload_file` — 上傳檔案
- `get_browser_status` — 取得瀏覽器狀態
- `set_timeout` — 設定逾時時間
- `clear_session` — 清除瀏覽器會話

**特性**:
- 每個矽基生命體獨立實例
- 完全隔離的 Cookie 和會話
- 使用者完全不可見（無頭模式）
- 完整 JavaScript 和 CSS 支援

**使用範例**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 26. 工作筆記工具 (WorkNoteTool)

**工具名稱**: `work_note`

**功能描述**: 管理矽基生命體個人工作筆記（私有，類似日記本）。

**支援的操作**:
- `create` — 建立筆記
- `read` — 讀取筆記
- `update` — 更新筆記
- `delete` — 刪除筆記
- `list` — 列出筆記
- `search` — 搜尋筆記
- `directory` — 產生目錄

**使用範例**:
```json
{
  "action": "create",
  "summary": "完成使用者認證模組",
  "content": "## 實作細節\n\n- 使用 JWT token\n- 支援 OAuth2",
  "keywords": "認證,JWT,OAuth2"
}
```

---

## 工具呼叫流程

```
┌──────────┐
│   AI     │ 傳回 tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ 查找和驗證工具使用權
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

## 權限驗證

所有工具執行都透過權限驗證鏈：

1. **UserFrequencyCache** — 高頻使用者決策快取（HighDeny 優先於 HighAllow）
2. **IPermissionCallback** — 自訂權限回呼函式（Allowed/Denied/AskUser）
3. **IsCurator 分支** — 主理人透過 IPermissionAskHandler 詢問使用者；非主理人查詢 GlobalACL，無匹配規則則預設拒絕

## 建立自訂工具

### 步驟 1: 實作 ITool 介面

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "工具描述";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "參數說明" }
        }
    };
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        try
        {
            var param1 = call.Parameters["param1"]?.ToString();
            var result = await DoWork(param1);
            
            return new ToolResult
            {
                Success = true,
                Output = result
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}
```

### 步驟 2: 新增到專案

將工具檔案放置在 `src/SiliconLife.Common/Tools/` 目錄中（共享工具）或 `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` 目錄中（版本特定工具）。`ToolManager` 會在啟動時透過反射自動發現和註冊。

### 步驟 2a: 透過外掛程式註冊工具

也可以透過外掛程式系統註冊自訂工具：

1. 在外掛程式專案中實作 `ITool` 介面
2. 編譯外掛程式 DLL 並放入外掛程式目錄
3. `ToolManager.ScanAllPluginAssemblies()` 會自動掃描所有已載入外掛程式中的 ITool 實作
4. 外掛程式工具受相同的權限系統約束

### 步驟 3: （可選）標記為主理人專用

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // 僅矽基主理人可存取
}
```

### 替代方案：技能與 MCP 工具

除撰寫 C# 工具類別外，還有兩種無需編譯的擴充方式：

- **技能（Skill）**：透過 Web UI 或 `skill` 工具建立「工具編排 + 提示詞範本」組合，適合把常用工作流程封裝為可重複使用的能力。參見 [矽基生命體指南 — 技能系統](silicon-being-guide.md#技能系統)。
- **MCP 伺服器**：在 Web UI 設定外部 MCP 伺服器後，其工具自動以 `mcp_{serverId}_{toolName}` 形式注入，無需撰寫任何程式碼。參見 [Web UI 指南 — MCP 管理](web-ui-guide.md)。

## 最佳實務

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

永遠不要繞過權限檢查。始終透過執行器存取資源：

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
}
```

### 4. 提供清晰的工具描述

協助 AI 理解何時以及如何使用工具：

```csharp
public string Description => 
    "用於在不同日曆系統之間轉換日期。" +
    "需要提供 'date'、'from_calendar' 和 'to_calendar' 參數。";
```

## 故障排除

### 工具未找到

**問題**: AI 嘗試呼叫不存在的工具。

**解決方案**:
- 檢查工具名稱是否完全匹配
- 驗證工具檔案在 `Tools/` 目錄中
- 重新建置專案 (`dotnet build`)

### 權限被拒絕

**問題**: 工具執行失敗，傳回權限錯誤。

**解決方案**:
- 檢查權限審計日誌
- 驗證矽基生命體具有所需權限
- 檢視全域 ACL 設定
- 如果是主理人，檢查是否使用了 `[SiliconManagerOnly]` 標記

### 工具執行傳回錯誤

**問題**: 工具執行但傳回失敗結果。

**解決方案**:
- 檢查工具傳回的錯誤訊息
- 驗證輸入參數格式正確
- 檢視系統日誌取得詳細錯誤資訊
- 獨立測試工具功能

## 下一步

- 📚 閱讀[架構指南](architecture.md)
- 🛠️ 檢視[開發指南](development-guide.md)
- 🔒 了解[權限系統](permission-system.md)
- 🚀 檢視[快速入門指南](getting-started.md)
