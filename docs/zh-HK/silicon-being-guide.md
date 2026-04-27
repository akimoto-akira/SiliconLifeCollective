# 硅基生命體指南

> **版本：v0.1.0-alpha**

[English](../en/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | **繁體中文** | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md)

## 概述

硅基生命体是 AI 驅動程式的智能体，可以自主思考、行動和進化。

## 架構

### 身體-大腦分離

```
┌─────────────────────────────────────┐
│         硅基生命体                   │
├──────────────────┬──────────────────┤
│   身體            │   大腦            │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • 狀態管理        │ • 載入歷史記录    │
│ • 触發检测        │ • 调用 AI         │
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

### 示例

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

## 建立生命体

### 通過 Web UI

1. 導航到**生命体管理**
2. 点擊**建立新生命体**
3. 填寫：
   - 名称
   - 靈魂內容
   - 設定選項
4. 点擊**建立**

### 通過 API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## 生命体生命週期

### 狀態

```
Created → Starting → Running → Stopping → Stopped
                    ↓
                  Error
```

### 操作

- **啟動**：初始化並開始處理
- **停止**：優雅關閉
- **暂停**：临时挂起（保持狀態）
- **復原**：從暂停狀態继续

## 工作系統

### 建立工作

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

### 工作狀態

- `Pending` - 等待執行
- `Running` - 正在執行
- `Completed` - 成功完成
- `Failed` - 執行失敗
- `Cancelled` - 手動取消

## 定时器系統

### 定时器類型

1. **一次性**：延迟後執行一次
2. **间隔**：以固定间隔重复執行
3. **Cron**：基於 cron 表達式執行

### 示例

```csharp
// 每小时執行
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

- **短期**：當前對话上下文
- **長期**：持久化知識和经驗
- **情景**：時間索引的事件和交互

### 儲存結構

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

## 工作筆記系統

### 概述

工作筆記是硅基生命體的個人日記系統，採用頁式設計，用於記錄工作進展、學習心得、專案筆記等。

### 特性

- **頁式管理**：每條筆記獨立成頁，按頁碼訪問
- **Markdown 支持**：內容支持 Markdown 格式（文本、列表、表格、程式碼區塊）
- **關鍵詞索引**：支持為筆記添加關鍵詞，便於搜索
- **摘要功能**：每條筆記有簡短摘要，快速瀏覽
- **目錄生成**：可生成所有筆記的目錄概覽，幫助理解整體上下文
- **時間戳**：自動記錄創建和更新時間
- **默認私有**：僅生命體自身可訪問（主理人可管理）

### 使用場景

1. **專案進展記錄**
   ```
   摘要：完成用戶認證模組
   內容：實現了 JWT token 驗證、OAuth2 整合、刷新 token 機制
   關鍵詞：認證,JWT,OAuth2
   ```

2. **學習筆記**
   ```
   摘要：學習 C# 異步程式設計最佳實踐
   內容：async/await 使用注意事項、ConfigureAwait 的使用場景...
   關鍵詞：C#,異步,最佳實踐
   ```

3. **會議紀要**
   ```
   摘要：產品需求討論會
   內容：討論了新功能需求，確定了實現方案...
   關鍵詞：產品,需求,會議
   ```

### 通過工具使用

生命體可以通過 `work_note` 工具管理工作筆記：

```json
// 創建筆記
{
  "action": "create",
  "summary": "完成用戶認證模組",
  "content": "## 實現細節\n\n- 使用 JWT token\n- 支持 OAuth2",
  "keywords": "認證,JWT,OAuth2"
}

// 讀取筆記
{
  "action": "read",
  "page_number": 1
}

// 搜索筆記
{
  "action": "search",
  "keyword": "認證",
  "max_results": 10
}
```

### 通過 Web UI 管理

1. 導航到**生命體管理** → 選擇生命體
2. 點擊**工作筆記**標籤頁
3. 可以查看、搜索、編輯筆記
4. 支持 Markdown 預覽

## 知識網絡系統

### 概述

知識網絡是基於三元組結構（主語-謂語-賓語）的知識表示和管理系統，用於儲存和管理結構化的知識。

### 核心概念

#### 三元組結構

```
主語 (Subject) --謂語 (Predicate)--> 賓語 (Object)
```

**示例**：
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

支持為三元組添加標籤，便於分類和搜索：
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### 知識操作

#### 1. 添加知識

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

#### 3. 搜索知識

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

返回：
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

獲取知識網絡的整體統計資訊：
```json
{
  "action": "stats"
}
```

返回：
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
   - 示例：`地球` --`is_a`--> `行星`

2. **概念關係**
   - 記錄概念之間的關係
   - 示例：`繼承` --`is_a`--> `面向物件程式設計概念`

3. **學習積累**
   - 生命體通過學習不斷積累知識
   - 形成結構化的知識體系

4. **推理支持**
   - 通過知識路徑發現間接關係
   - 支持基於知識的推理和決策

### 通過 Web UI 管理

1. 導航到**知識網絡**頁面
2. 查看知識統計資訊
3. 搜索和瀏覽知識
4. 視覺化知識關係圖（計劃中）

## 最佳实践

### 靈魂檔案编寫

1. **具体**：清晰的個性特征和邊界
2. **定義范围**：生命体应该和不应该做什麼
3. **包含示例**：展示預期的行為模式
4. **定期更新**：根据表现進化靈魂

### 工作管理

1. **設定優先級**：使用優先級（1-10）
2. **定義截止日期**：始终設定截止日期
3. **監控進度**：定期檢查工作狀態
4. **處理失敗**：實現重试逻辑

### 記憶最佳化

1. **清理舊資料**：定期歸档舊記憶
2. **索引重要資訊**：标記關键資訊
3. **使用時間儲存**：利用時間索引查詢

## 故障排除

### 生命体无法啟動

**檢查**：
- 靈魂檔案存在且有效
- AI 客戶端已設定
- 系統資源充足

### 生命体意外停止

**檢查**：
- 記錄中的錯誤
- AI 服務可用性
- 記憶體使用

### 工作未執行

**檢查**：
- 定时器系統正在執行
- 工作優先級和計畫
- 權限設定

## 下一步

- 📚 阅读[架構指南](architecture.md)
- 🛠️ 查看[開發指南](development-guide.md)
- 🚀 查看[快速開始指南](getting-started.md)
