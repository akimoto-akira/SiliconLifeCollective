# 硅基生命體指南

[English](silicon-being-guide.md) | [简体中文](docs/zh-CN/silicon-being-guide.md) | [繁體中文](docs/zh-HK/silicon-being-guide.md) | [Español](docs/es-ES/silicon-being-guide.md) | [日本語](docs/ja-JP/silicon-being-guide.md) | [한국어](docs/ko-KR/silicon-being-guide.md) | [Čeština](docs/cs-CZ/silicon-being-guide.md)

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
