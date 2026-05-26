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
- **項目工具** — 項目管理、項目任務、項目工作筆記、項目工作
- **插件工具** — 透過插件系統註冊的第三方工具

### 工具場景系統

每個工具透過 `[ToolScenario]` 屬性宣告其可用場景：

| 場景標誌 | 值 | 描述 |
|----------|------|-------------|
| `Chat` | `1 << 0` | 聊天場景（使用者與矽基生命體對話時） |
| `Task` | `1 << 1` | 任務場景（矽基生命體執行任務時） |
| `Timer` | `1 << 2` | 定時器場景（矽基生命體執行定時任務時） |
| `MemoryCompression` | `1 << 3` | 記憶壓縮場景 |
| `Project` | `1 << 4` | 項目場景（ThinkOnProject 模式） |
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

**功能描述**: 管理聊天會話和訊息傳送。

**支援的操作**:
- `send_message` — 傳送訊息
- `get_messages` — 取得歷史訊息
- `create_group` — 建立群聊
- `add_member` — 新增群成員
- `remove_member` — 移除群成員
- `get_chat_info` — 取得聊天資訊
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
- `get_ai_config` — 取得 AI 用戶端設定
- `set_ai_config` — 設定 AI 用戶端設定

**使用範例**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. 主理人工具 (CuratorTool) 🔒

**工具名稱**: `silicon_manager`

**權限要求**: 僅限矽基主理人使用（`[SiliconManagerOnly]`）

**可用場景**: Chat、Task、Timer

**功能描述**: 矽基主理人專用的系統管理工具，用於管理矽基生命體的建立、檢視和重設。

**支援的操作**:
- `list_beings` — 列出所有矽基生命體及其狀態
- `create_being` — 建立新矽基生命體（需要 `name` 和 `soul` 參數）
- `get_code` — 檢視矽基生命體的自訂原始碼
- `reset` — 將矽基生命體重設為預設實作

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

**功能描述**: 檔案系統操作和本機搜尋。

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
- 執行時期靜態程式碼掃描
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
- `run_script` — 執行程式碼指令碼

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

### 11. 日誌工具 (LogTool)

**工具名稱**: `log`

**功能描述**: 查詢操作歷史和對話歷史。

**支援的操作**:
- `query_logs` — 查詢系統日誌
- `query_conversations` — 查詢對話歷史
- `get_stats` — 取得日誌統計

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
- `get_stats` — 取得記憶統計
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

### 15. 項目工具 (ProjectTool) 🔒

**工具名稱**: `project`

**權限要求**: 僅限矽基主理人使用（`[SiliconManagerOnly]`）

**可用場景**: Chat、Task、Timer

**功能描述**: 管理項目工作區，支援項目生命週期管理、成員分配和角色管理。

**支援的操作**:
- `create` — 建立新項目空間
- `archive` — 歸檔項目
- `restore` — 還原已歸檔的項目
- `destroy` — 銷毀項目並清理資料（不可還原）
- `list` — 列出所有項目
- `get` — 取得項目詳情
- `assign` — 將矽基生命體分配到項目
- `remove` — 從項目中移除矽基生命體
- `update` — 更新項目名稱/描述
- `list-workflow-templates` — 列出可用的工作流範本
- `assign_role` — 為矽基生命體分配項目角色
- `remove_role` — 移除矽基生命體的項目角色
- `list_roles` — 列出項目的角色分配

**使用範例**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "項目描述"
}
```

---

### 16. 項目任務工具 (ProjectTaskTool)

**工具名稱**: `project_task`

**可用場景**: Chat、Task、Timer

**功能描述**: 管理項目空間內的任務，支援完整的任務生命週期。

**支援的操作**:
- `create` — 建立項目任務
- `list` — 列出項目任務
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

### 17. 項目工作筆記工具 (ProjectWorkNoteTool)

**工具名稱**: `project_work_note`

**可用場景**: Chat、Task、Timer

**功能描述**: 管理項目空間內的工作筆記（公開，類似工作本），支援頁面式筆記管理。

**支援的操作**:
- `create` — 建立筆記頁面（需要 `project_id`、`summary` 和 `content`，可選 `keywords`）
- `read` — 讀取筆記頁面（需要 `project_id` 和 `page_number` 或 `note_id`）
- `update` — 更新筆記頁面（需要 `project_id`、`page_number` 和 `content`，可選 `summary` 和 `keywords`）
- `delete` — 刪除筆記頁面（需要 `project_id` 和 `page_number` 或 `note_id`）
- `list` — 列出項目的所有筆記頁面摘要
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

### 18. 項目工作工具 (ProjectWorkTool) 🔒

**工具名稱**: `project_work`

**權限要求**: 僅限矽基主理人使用（`[SiliconManagerOnly]`）

**可用場景**: Project（`[ToolScenario(ToolScenarioFlag.Project)]`，僅在項目場景可用）

**功能描述**: 項目工作操作工具，用於主理人在 ThinkOnProject 場景中管理項目工作流。

**支援的操作**:
- `create-task` — 建立項目任務
- `assign-task` — 為任務分配矽基生命體
- `chat` — 傳送訊息到項目群聊
- `broadcast` — 廣播訊息到項目頻道
- `complete` — 標記項目為已完成
- `status` — 取得項目狀態

**使用範例**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "實作使用者認證"
}
```

---

### 19. 系統工具 (SystemTool)

**工具名稱**: `system`

**功能描述**: 取得系統資訊和資源使用情況。

**支援的操作**:
- `info` — 取得系統資訊
- `resource_usage` — 取得資源使用情況
- `find_process` — 查找處理程序
- `list_beings` — 列出矽基生命體

**使用範例**:
```json
{
  "action": "info"
}
```

---

### 20. 任務工具 (TaskTool)

**工具名稱**: `task`

**功能描述**: 管理矽基生命體個人任務。

**支援的操作**:
- `create` — 建立任務
- `list` — 列出任務
- `update` — 更新任務
- `complete` — 完成任務
- `delete` — 刪除任務
- `get_dependencies` — 取得依賴關係

**使用範例**:
```json
{
  "action": "create",
  "description": "審查程式碼",
  "priority": 5
}
```

---

### 21. 定時器工具 (TimerTool)

**工具名稱**: `timer`

**功能描述**: 建立和管理定時器。

**支援的操作**:
- `create` — 建立定時器
- `list` — 列出定時器
- `delete` — 刪除定時器
- `pause` — 暫停定時器
- `resume` — 還原定時器
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

### 22. Token 審計工具 (TokenAuditTool) 🔒

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

### 23. WebView 瀏覽器工具 (WebViewBrowserTool)

**工具名稱**: `webview_browser`

**可用場景**: Chat、Task、Timer

**功能描述**: 基於 Playwright 的瀏覽器自動化操作，提供完整的網頁導航、互動和資料提取能力。

**支援的操作**:
- `open` — 開啟瀏覽器
- `close` — 關閉瀏覽器
- `navigate` — 導航到 URL
- `click` — 點擊元素
- `input` — 輸入文字
- `scroll` — 捲動頁面
- `execute_script` — 執行 JavaScript
- `get_page_text` — 取得頁面文字
- `get_screenshot` — 取得截圖
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

### 24. 工作筆記工具 (WorkNoteTool)

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

### 25. 熱重載工具 (HotReloadTool)

**工具名稱**: `hot_reload`

**功能描述**: 支援 SiliconLife.Fast 在執行中自動編譯、更新檔案並重啟，無需手動干預。

**支援的操作**:
- `execute` — 執行完整的建構、複製和重啟流程
- `build_only` — 僅建構專案，不複製和重啟

**工作流程**:
1. 編譯 SiliconLife.Fast 專案
2. 優雅關閉目前執行的 Fast 實例（透過 HTTP API）
3. 等待處理程序結束和連接埠釋放
4. 複製建構輸出到目標目錄（跳過 HotReload 自身檔案）
5. 重新啟動 Fast 實例

**特性**:
- 自動偵測並關閉舊處理程序
- 安全檔案複製（不覆寫 HotReload.exe）
- 連接埠釋放等待機制
- 支援自訂連接埠設定

**使用範例**:
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**參數說明**:
- `project_path`: 專案路徑（相對於解決方案根目錄）
- `source_path`: 建構輸出目錄
- `configuration`: 建構設定（Debug/Release）
- `port`: Fast 實例的 Web 連接埠（預設 8080）

**注意事項**:
- 僅適用於 SiliconLife.Fast 版本
- 需要 HotReload.exe 在 tools/HotReload 目錄下
- 重啟過程中會有短暫的服務中斷（約 3-5 秒）

---

## 工具呼叫流程

```
┌──────────┐
│   AI     │ 返回 tool_calls
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
            ["param1"] = new { type = "string", description = "参数说明" }
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

將工具檔案放置在 `src/SiliconLife.Common/Tools/` 目錄中（共享工具）或 `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` 目錄中（版本特定工具）。`ToolManager` 會在啟動時透過反射自動發現並註冊。

### 步驟 2a: 透過插件註冊工具

也可以透過插件系統註冊自訂工具：

1. 在插件專案中實作 `ITool` 介面
2. 編譯插件 DLL 並放入插件目錄
3. `ToolManager.ScanAllPluginAssemblies()` 會自動掃描所有已載入插件中的 ITool 實作
4. 插件工具受相同的權限系統約束

### 步驟 3: （可選）標記為主理人專用

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // 仅硅基主理人可访问
}
```

## 最佳實務

### 1. 始終驗證參數

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("缺少必需参数: required_param");
}
```

### 2. 優雅處理錯誤

```csharp
try
{
    // 执行操作
}
catch (Exception ex)
{
    Logger.Error($"工具 {Name} 执行失败: {ex.Message}");
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

幫助 AI 理解何時以及如何使用工具：

```csharp
public string Description =>
    "用于在不同日历系统之间转换日期。" +
    "需要提供 'date'、'from_calendar' 和 'to_calendar' 参数。";
```

## 故障排除

### 工具未找到

**問題**: AI 嘗試呼叫不存在的工具。

**解決方案**:
- 檢查工具名稱是否完全匹配
- 驗證工具檔案在 `Tools/` 目錄中
- 重新建構專案 (`dotnet build`)

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
- 🚀 檢視[快速開始指南](getting-started.md)
