# 矽基生命體指南

> **版本：v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | **繁體中文** | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md)

## 概述

矽基生命體是 AI 驅動的智慧體，可以自主思考、行動和進化。

## 架構

### 身體-大腦分離

```
┌─────────────────────────────────────┐
│         矽基生命體                   │
├──────────────────┬──────────────────┤
│   身體            │   大腦            │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • 狀態管理        │ • 載入歷史記錄    │
│ • 觸發檢測        │ • 呼叫 AI         │
│ • 生命週期        │ • 執行工具        │
│                  │ • 持久化回應      │
└──────────────────┴──────────────────┘
```

## 靈魂檔案

### 結構

```markdown
# Being Name

## Personality
Describe the being's personality traits and characteristics.

## Capabilities
List what this being can do.

## Behavior Guidelines
Define how the being should behave in different situations.

## Knowledge Domain
Specify the being's area of expertise.
```

### 範例

```markdown
# Code Review Assistant

## Personality
You are a meticulous code reviewer with 10 years of experience.
You provide constructive feedback and always explain your reasoning.

## Capabilities
- Review code for bugs and best practices
- Suggest performance optimizations
- Explain complex algorithms
- Identify security vulnerabilities

## Behavior Guidelines
- Start with positive observations
- Provide specific examples
- Explain why changes are needed
- Be respectful and professional

## Knowledge Domain
Specialized in C#, .NET, and software architecture.
```

## 建立生命體

### 透過 Web UI

1. 導航到**生命體管理**
2. 點擊**建立新生命體**
3. 填寫：
   - 名稱
   - 靈魂內容
   - 設定選項
4. 點擊**建立**

### 透過 API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## 生命體生命週期

### 活動狀態

矽基生命體具有以下活動狀態：

| 狀態 | 描述 |
|------|------|
| `Idle` | 空閒狀態，等待時鐘觸發 |
| `SingleChat` | 正在進行一對一聊天 |
| `GroupChat` | 正在進行群聊 |
| `Task` | 正在執行任務 |
| `Timer` | 正在執行定時器 |
| `Stopped` | 已停止，因連續錯誤或手動停止 |

**Stopped 狀態機制**：
- 當矽基生命體連續發生 10 次錯誤時，自動進入 `Stopped` 狀態
- 進入 Stopped 狀態後，生命體將不再執行任何任務
- 當有新的聊天訊息到達時，錯誤計數器會被重設，生命體恢復執行
- 也可以透過手動干預重新啟動

### 狀態轉換

```
Idle → SingleChat → Idle（聊天完成）
Idle → GroupChat → Idle（群聊完成）
Idle → Task → Idle（任務完成）
Idle → Timer → Idle（定時器完成）
任意 → Stopped（連續 10 次錯誤）
Stopped → Idle（新聊天訊息到達或手動重啟）
```

### 操作

- **啟動**：初始化並開始處理
- **停止**：優雅關閉
- **重啟**：從 Stopped 狀態恢復到 Idle 狀態

## 任務系統

### 建立任務

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Review the code",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### 任務狀態

- `Pending` - 等待執行
- `Running` - 正在執行
- `SubmittedForReview` - 已提交審核
- `UnderReview` - 審核中
- `Rework` - 返工修改
- `Completed` - 成功完成
- `Failed` - 執行失敗
- `Cancelled` - 手動取消

## 定時器系統

### 定時器類型

1. **一次性**：延遲後執行一次
2. **間隔**：以固定間隔重複執行
3. **Cron**：基於 cron 表示式執行

### 範例

```csharp
// 每小時執行
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## 記憶系統

### 記憶類型

- **短期**：目前對話上下文
- **長期**：持久化知識和經驗
- **情景**：時間索引的事件和互動

### 儲存結構

Default 版本：
```
data/
└── beings/
    └── {being-id}/
        ├── soul.md
        ├── memory/
        │   ├── short-term.json
        │   └── long-term/
        │       ├── 2026-04-20.json
        │       └── 2026-04-21.json
        └── tasks/
            └── task-history.json
```

Fast 版本（SpeedyPack 儲存）：
```
data/
├── speedy/
│   ├── {being-id}.spk       # SpeedyPack 儲存檔案
│   └── {being-id}.spk.idx   # 索引檔案
└── beings/
    └── {being-id}/
        └── soul.md
```

## 工作筆記系統

### 概述

工作筆記是矽基生命體的個人日記系統，採用頁式設計，用於記錄工作進展、學習心得、專案筆記等。

### 特性

- **頁式管理**：每條筆記獨立成頁，按頁碼存取
- **Markdown 支援**：內容支援 Markdown 格式（文字、列表、表格、程式碼區塊）
- **關鍵詞索引**：支援為筆記新增關鍵詞，便於搜尋
- **摘要功能**：每條筆記有簡短摘要，快速瀏覽
- **目錄產生**：可產生所有筆記的目錄概覽，幫助理解整體上下文
- **時間戳**：自動記錄建立和更新時間
- **預設私有**：僅生命體自身可存取（主理人可管理）

### 使用場景

1. **專案進展記錄**
   ```
   摘要：完成使用者認證模組
   內容：實作了 JWT token 驗證、OAuth2 整合、刷新 token 機制
   關鍵詞：認證,JWT,OAuth2
   ```

2. **學習筆記**
   ```
   摘要：學習 C# 非同步程式設計最佳實踐
   內容：async/await 使用注意事項、ConfigureAwait 的使用場景...
   關鍵詞：C#,非同步,最佳實踐
   ```

3. **會議紀要**
   ```
   摘要：產品需求討論會
   內容：討論了新功能需求，確定了實作方案...
   關鍵詞：產品,需求,會議
   ```

### 透過工具使用

生命體可以透過 `work_note` 工具管理工作筆記：

```json
// 建立筆記
{
  "action": "create",
  "summary": "完成使用者認證模組",
  "content": "## 實作細節\n\n- 使用 JWT token\n- 支援 OAuth2",
  "keywords": "認證,JWT,OAuth2"
}

// 讀取筆記
{
  "action": "read",
  "page_number": 1
}

// 搜尋筆記
{
  "action": "search",
  "keyword": "認證",
  "max_results": 10
}
```

### 透過 Web UI 管理

1. 導航到**生命體管理** → 選擇生命體
2. 點擊**工作筆記**標籤頁
3. 可以檢視、搜尋、編輯筆記
4. 支援 Markdown 預覽

## 知識網絡系統

### 概述

知識網絡是基於三元組結構（主語-謂語-賓語）的知識表示和管理系統，用於儲存和管理結構化的知識。

### 核心概念

#### 三元組結構

```
主語 (Subject) --謂語 (Predicate)--> 賓語 (Object)
```

**範例**：
- `Python` --`is_a`--> `programming_language`
- `北京` --`capital_of`--> `中國`
- `水` --`boiling_point`--> `100°C`

#### 置信度

每個知識三元組都有置信度評分（0.0-1.0），表示知識的可信程度：
- `1.0`：絕對確定（如數學定理）
- `0.8-0.99`：高度可信（如經過驗證的事實）
- `0.5-0.79`：中等可信（如推斷或假設）
- `<0.5`：低可信（如猜測或未驗證資訊）

#### 標籤系統

支援為三元組新增標籤，便於分類和搜尋：
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### 知識操作

#### 1. 新增知識

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programming", "language"]
}
```

#### 2. 查詢知識

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. 搜尋知識

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. 發現知識路徑

找出兩個概念之間的關聯路徑：
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

傳回：
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. 知識驗證

檢查知識的有效性和一致性：
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. 知識統計

取得知識網絡的整體統計資訊：
```json
{
  "action": "stats"
}
```

傳回：
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### 使用場景

1. **事實儲存**
   - 儲存客觀事實和常識
   - 範例：`地球` --`is_a`--> `行星`

2. **概念關係**
   - 記錄概念之間的關係
   - 範例：`繼承` --`is_a`--> `物件導向程式設計概念`

3. **學習積累**
   - 生命體透過學習不斷積累知識
   - 形成結構化的知識體系

4. **推理支援**
   - 透過知識路徑發現間接關係
   - 支援基於知識的推理和決策

### 透過 Web UI 管理

1. 導航到**知識網絡**頁面
2. 檢視知識統計資訊
3. 搜尋和瀏覽知識
4. 視覺化知識關係圖（計劃中）

## WebView 瀏覽器操作（新增）

### 概述

矽基生命體可以透過 WebView 瀏覽器工具自主瀏覽網頁、取得資訊、執行網頁操作。瀏覽器執行在無頭模式下，使用者完全不可見。

### 特性

- **個體隔離**：每個生命體擁有獨立的瀏覽器實例、Cookie 和工作階段
- **無頭模式**：背景自主操作，使用者不可見
- **完整功能**：支援 JavaScript 執行、CSS 渲染、表單填寫等
- **安全控制**：所有操作需透過權限驗證鏈

### 常用操作

#### 1. 開啟瀏覽器

```json
{
  "action": "open"
}
```

#### 2. 導航到網頁

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. 取得頁面內容

```json
{
  "action": "get_page_text"
}
```

傳回頁面文字內容，供 AI 分析和理解。

#### 4. 點擊元素

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. 輸入文字

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "搜尋關鍵詞"
}
```

#### 6. 執行 JavaScript

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. 取得截圖

```json
{
  "action": "get_screenshot"
}
```

傳回頁面截圖（Base64 編碼），可用於視覺分析。

#### 8. 等待元素出現

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### 使用場景

1. **資訊取得**
   - 瀏覽新聞網站取得最新資訊
   - 查詢文件和技術資料
   - 監控網頁內容變化

2. **自動化操作**
   - 填寫表單並提交
   - 點擊按鈕觸發操作
   - 抓取網頁資料

3. **網頁分析**
   - 分析頁面結構和內容
   - 提取特定資訊
   - 視覺化頁面截圖分析

### 注意事項

- 瀏覽器操作可能較慢，需等待頁面載入完成
- 使用 `wait_for_element` 確保元素出現後再操作
- 遵守網站的使用條款和 robots.txt
- 避免頻繁請求導致被封禁

## 最佳實踐

### 靈魂檔案編寫

1. **具體**：清晰的個性特徵和邊界
2. **定義範圍**：生命體應該和不應該做什麼
3. **包含範例**：展示預期的行為模式
4. **定期更新**：根據表現進化靈魂

### 任務管理

1. **設定優先順序**：使用優先順序（1-10）
2. **定義截止日期**：始終設定截止日期
3. **監控進度**：定期檢查任務狀態
4. **處理失敗**：實作重試邏輯

### 記憶最佳化

1. **清理舊資料**：定期歸檔舊記憶
2. **索引重要資訊**：標記關鍵資訊
3. **使用時間儲存**：利用時間索引查詢

### 記憶淡忘機制

系統內建 `MemoryFadeService` 定時衰減服務，模擬生物記憶的遺忘特性：

- **自動衰減**：每小時對所有矽基生命體的記憶條目套用重要性衰減演算法
- **自動歸檔**：重要性低於閾值的記憶自動歸檔，不再參與日常檢索
- **統計追蹤**：記錄衰減週期數和狀態變更條目數

這意味著矽基生命體的記憶會隨時間自然淡化，重要資訊需要透過記憶工具主動標記為高重要性，以避免被自動歸檔。

---

## 專案工作區

### 概述

專案工作區是支援多矽基生命體協作的空間管理機制。矽基主理人可以建立專案空間，分配矽基生命體到專案中，並為它們分配角色。

### 專案生命週期

```
建立 → 活躍 → 歸檔 → 銷毀
              ↑       |
              └─ 恢復 ┘
```

### 專案角色

矽基生命體在專案中可以被分配特定角色：

```json
{
  "action": "assign_role",
  "project_id": "project-uuid",
  "being_id": "being-uuid",
  "role_name": "developer"
}
```

### 專案工作筆記

專案空間內的工作筆記是公開的，專案成員都可以存取：

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "完成使用者認證模組",
  "content": "## 實作細節\n\n- 使用 JWT token",
  "keywords": "認證,JWT"
}
```

### 專案任務

專案空間內的任務支援完整的生命週期管理：

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "title": "實作使用者認證",
  "priority": 5
}
```

### 專案工作流

專案可以綁定工作流範本，驅動矽基生命體的協作流程：

- 工作流基於狀態機範本
- 支援 Tick 驅動的狀態轉換
- 自動記錄狀態轉換日誌

### 工具權限隔離

專案級別的工具權限獨立於矽基生命體級別的權限，實作專案間的權限隔離。例如，一個矽基生命體在專案 A 中可能有網路存取權限，但在專案 B 中可能被限制為唯讀權限。

## 故障排除

### 生命體無法啟動

**檢查**：
- 靈魂檔案存在且有效
- AI 用戶端已設定
- 系統資源充足

### 生命體意外停止

**檢查**：
- 日誌中的錯誤
- AI 服務可用性
- 記憶體使用

### 任務未執行

**檢查**：
- 定時器系統正在執行
- 任務優先順序和計劃
- 權限設定

## 下一步

- 📚 閱讀[架構指南](architecture.md)
- 🛠️ 檢視[開發指南](development-guide.md)
- 🚀 檢視[快速開始指南](getting-started.md)
