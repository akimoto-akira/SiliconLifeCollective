# 工具參考

> **版本：v0.2.0-alpha**

[English](../en/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | **繁體中文** | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md)

## 概述

工具系統允許矽基生命體透過標準化介面與外部世界互動。每個工具實現 `ITool` 介面，由 `ToolManager` 透過反射自動發現和註冊。

### 工具分類

- **系統管理工具** — 設定、權限、動態編譯
- **通訊工具** — 聊天、網路請求
- **資料儲存工具** — 磁碟操作、資料庫、記憶、工作筆記
- **時間管理工具** — 日曆、定時器、任務
- **開發工具** — 程式碼執行、日誌查詢
- **實用工具** — 系統資訊、Token 審計、說明文件、知識網路
- **瀏覽器工具** — WebView 瀏覽器自動化
- **外掛程式工具** — 透過外掛程式系統註冊的第三方工具

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

### 15. 專案工具 (ProjectTool)

**工具名稱**: `project`

**功能描述**: 管理專案工作區。

**支援的操作**:
- `create` — 建立專案
- `list` — 列出專案
- `get_info` — 獲取專案資訊
- `update` — 更新專案
- `archive` — 封存專案

**使用範例**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "專案描述"
}
```

---

### 16. 專案任務工具 (ProjectTaskTool)

**工具名稱**: `project_task`

**功能描述**: 管理專案任務。

**支援的操作**:
- `create` — 建立任務
- `list` — 列出任務
- `update` — 更新任務
- `complete` — 完成任務
- `get_stats` — 獲取任務統計

**使用範例**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "任務描述",
  "priority": 5
}
```

---

### 17. 專案工作筆記工具 (ProjectWorkNoteTool)

**工具名稱**: `project_work_note`

**功能描述**: 管理專案工作筆記（公開，類似工作筆記本）。

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
  "project_id": "project-uuid",
  "summary": "完成使用者認證模組",
  "content": "## 實作詳情\n\n- 使用 JWT Token",
  "keywords": "authentication,JWT"
}
```

---

### 18. 系統工具 (SystemTool)

**工具名稱**: `system`

**功能描述**: 獲取系統資訊和資源使用狀況。

**支援的操作**:
- `info` — 獲取系統資訊
- `resource_usage` — 獲取資源使用狀況
- `find_process` — 查找程序
- `list_beings` — 列出矽基生命體

**使用範例**:
```json
{
  "action": "info"
}
```

---

### 19. 任務工具 (TaskTool)

**工具名稱**: `task`

**功能描述**: 管理矽基生命體個人任務。

**支援的操作**:
- `create` — 建立任務
- `list` — 列出任務
- `update` — 更新任務
- `complete` — 完成任務
- `delete` — 刪除任務
- `get_dependencies` — 獲取依賴關係

**使用範例**:
```json
{
  "action": "create",
  "description": "審查程式碼",
  "priority": 5
}
```

---

### 20. 定時器工具 (TimerTool)

**工具名稱**: `timer`

**功能描述**: 建立和管理定時器。

**支援的操作**:
- `create` — 建立定時器
- `list` — 列出定時器
- `delete` — 刪除定時器
- `pause` — 暫停定時器
- `resume` — 恢復定時器
- `get_execution_history` — 獲取執行歷史

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

### 21. Token 審計工具 (TokenAuditTool) 🔒

**工具名稱**: `token_audit`

**權限要求**: 僅限矽基主理人使用

**功能描述**: 查詢和彙總 AI Token 使用量。

**支援的操作**:
- `get_usage` — 獲取 Token 使用統計
- `get_by_being` — 按生命體獲取使用量
- `get_by_model` — 按模型獲取使用量
- `get_trend` — 獲取使用趨勢
- `export` — 匯出資料

**使用範例**:
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. WebView 瀏覽器工具 (WebViewBrowserTool)

**工具名稱**: `webview`

**功能描述**: 基於 Playwright 的瀏覽器自動化。

**支援的操作**:
- `open_browser` — 開啟瀏覽器
- `close_browser` — 關閉瀏覽器
- `navigate` — 導航到 URL
- `click` — 點擊元素
- `input` — 輸入文字
- `get_page_text` — 獲取頁面文字
- `get_screenshot` — 獲取螢幕截圖
- `execute_script` — 執行 JavaScript
- `wait_for_element` — 等待元素
- `get_browser_status` — 獲取瀏覽器狀態

**特性**:
- 每個矽基生命體獨立實例
- 完全隔離的 Cookie 和工作階段
- 對使用者完全不可見（無頭模式）
- 完整的 JavaScript 和 CSS 支援

**使用範例**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 23. 工作筆記工具 (WorkNoteTool)

**工具名稱**: `work_note`

**功能描述**: 管理矽基生命體個人工作筆記（私有，類似日記）。

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
  "content": "## 實作詳情\n\n- 使用 JWT Token\n- 支援 OAuth2",
  "keywords": "authentication,JWT,OAuth2"
}
```

---

### 24. 熱重載工具 (HotReloadTool)

**工具名稱**: `hot_reload`

**功能描述**: 支援 SiliconLife.Fast 在執行中自動編譯、更新檔案並重啟，無需手動干預。

**支援的操作**:
- `execute` — 執行完整的構建、複製和重啟流程
- `build_only` — 僅構建專案，不複製和重啟

**工作流程**:
1. 編譯 SiliconLife.Fast 專案
2. 優雅關閉當前執行的 Fast 實例（透過 HTTP API）
3. 等待程序退出和連接埠釋放
4. 複製構建輸出到目標目錄（跳過 HotReload 自身檔案）
5. 重新啟動 Fast 實例

**特性**:
- 自動檢測並關閉舊程序
- 安全檔案複製（不覆蓋 HotReload.exe）
- 連接埠釋放等待機制
- 支援自訂連接埠設定

**使用示例**:
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**引數說明**:
- `project_path`: 專案路徑（相對於解決方案根目錄）
- `source_path`: 構建輸出目錄
- `configuration`: 建構配置（Debug/Release）
- `port`: Fast 實例的 Web 連接埠（預設 8080）

**注意事項**:
- 僅適用於 SiliconLife.Fast 版本
- 需要 HotReload.exe 在 tools/HotReload 目錄下
- 重啟過程中會有短暫的服務中斷（約 3-5 秒）

---

## 權限驗證

所有工具執行都經過 3 級權限鏈：

1. **UserFrequencyCache** — 快取的使用者高頻允許/拒絕決策
2. **IPermissionCallback** — 自訂權限回呼函式
3. **主理人分支** — 如果回呼返回 AskUser 或無回呼：
   - **主理人** → `IPermissionAskHandler`（透過 IM 詢問使用者）
   - **非主理人** → `GlobalACL` → 預設拒絕

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

## 建立自訂工具

### 步驟 1: 實現 ITool 介面

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

### 步驟 2: 添加到專案

將工具檔案放置在 `src/SiliconLife.Common/Tools/` 目錄中（兩個版本共用的共享工具）。`ToolManager` 會在啟動時透過反射自動發現並註冊。

### 步驟 2a: 透過外掛程式註冊工具

也可以透過外掛程式系統註冊自訂工具：

1. 在外掛程式專案中實現 `ITool` 介面
2. 編譯外掛程式 DLL 並放入外掛程式目錄
3. `ToolManager.ScanAllPluginAssemblies()` 會自動掃描所有已載入外掛程式中的 ITool 實現
4. 外掛程式工具受相同的權限系統約束

### 步驟 3: （可選）標記為主理人專用

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // 僅矽基主理人可存取
}
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

永遠不要繞過權限檢查。始終透過執行器存取資源：

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. 提供清晰的工具描述

幫助 AI 理解何時以及如何使用工具：

```csharp
public string Description => 
    "用於在不同日曆系統之間轉換日期。" +
    "需要 'date'、'from_calendar' 和 'to_calendar' 參數。";
```

---

## 故障排除

### 找不到工具

**問題**：AI 嘗試呼叫不存在的工具。

**解決方案**：
- 檢查工具名稱是否完全匹配
- 確認工具檔案在 `Tools/` 目錄中
- 重新建構專案（`dotnet build`）

### 權限被拒絕

**問題**：工具執行失敗並返回權限錯誤。

**解決方案**：
- 檢查權限稽核日誌
- 確認矽基生命體具有所需權限
- 檢視全域 ACL 設定
- 如果是主理人，檢查是否使用了 `[SiliconManagerOnly]` 屬性

### 工具執行返回錯誤

**問題**：工具執行但返回失敗結果。

**解決方案**：
- 檢查工具返回的錯誤訊息
- 確認輸入參數格式正確
- 檢視系統日誌以獲取詳細錯誤資訊
- 獨立測試工具功能

---

## 下一步

- 📚 閱讀[架構指南](architecture.md)
- 🛠️ 查看[開發指南](development-guide.md)
- 🔒 了解[權限系統](permission-system.md)
- 🚀 查看[快速開始指南](getting-started.md)
