# 權限系統

> **版本：v0.2.0-alpha**

[English](../en/permission-system.md) | [中文](../zh-CN/permission-system.md) | **繁體中文** | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/api-reference.md)

## 概述

權限系統確保所有 AI 發起的操作都經過適當驗證和稽核。

## 3 級權限鏈

```
┌─────────────────────────────────────────────┐
│          權限驗證                            │
├─────────────────────────────────────────────┤
│  級別 1：UserFrequencyCache                  │
│  ↓ 快取的使用者決策（HighDeny/HighAllow）    │
│  級別 2：IPermissionCallback                 │
│  ↓ 自訂邏輯（Allowed/Denied/AskUser）       │
│  級別 3：IsCurator?                          │
│  ↓ 是 → IPermissionAskHandler（詢問使用者）  │
│  ↓ 否 → GlobalACL → 預設拒絕                │
│  結果：允許或拒絕                            │
└─────────────────────────────────────────────┘
```

> **注意**：`PermissionManager.CheckPermission()` 的實際查詢優先順序為：
> 1. **UserFrequencyCache** — 首先檢查快取的高頻使用者決策
> 2. **IPermissionCallback** — 評估自訂回呼規則
> 3. **主理人分支** — 如果回呼返回 AskUser 或無回呼：
>    - **主理人** → `IPermissionAskHandler`（透過 IM 提示使用者）
>    - **非主理人** → `GlobalACL` → 預設拒絕

## 級別 1：UserFrequencyCache

每個生命體的記憶體快取，存儲高頻使用者決策（HighDeny/HighAllow）。

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny** 優先於 **HighAllow**
- **僅存於記憶體**：快取不會持久化，重啟後遺失
- **可設定過期時間**：使用者可為快取條目設定有效期

## 級別 2：IPermissionCallback

用於動態權限邏輯的自訂回呼。

### DefaultPermissionCallback 預設實作

`DefaultPermissionCallback` 提供了全面的預設權限規則，包括：

#### 網絡存取規則
- **環回位址**：允許 localhost, 127.0.0.1, ::1
- **私有 IP 位址**：
  - 192.168.x.x (Class C) - 允許
  - 10.x.x.x (Class A) - 允許
  - 172.16-31.x.x (Class B) - 詢問使用者
- **域名白名單**：
  - 搜尋引擎：Google, Bing, DuckDuckGo, Yandex, Sogou 等
  - AI 服務：OpenAI, Anthropic, HuggingFace, Ollama 等
  - 開發者服務：GitHub, StackOverflow, npm, NuGet 等
  - 社交媒體：微博、知乎、Reddit、Discord 等
  - 影片平台：YouTube, Bilibili, 抖音、TikTok 等
  - **天氣資訊**：wttr.in
  - 政府網站：.gov, .go.jp, .go.kr
- **域名黑名單**：
  - AI 冒充網站：chatgpt, openai, deepseek 等仿冒域名
  - 惡意 AI 工具：wormgpt, darkgpt, fraudgpt 等
  - AI 內容農場和黑色市場相關域名

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }
        
        return PermissionResult.AskUser;
    }
}
```

## 級別 3：主理人分支（IsCurator → AskHandler / GlobalACL）

當回呼返回 `AskUser` 或未配置回呼時，系統根據主理人狀態進行分支：

### 主理人路徑：IPermissionAskHandler

對於矽基主理人，系統透過 IM 提示使用者做出決定。

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        SendMessage($"Allow {resource}?");

        var response = WaitForResponse();

        return new AskPermissionResult
        {
            Allowed = response.Approved,
            AddToCache = response.AddToCache,
            CacheDuration = response.CacheDuration
        };
    }
}
```

### 非主理人路徑：GlobalACL → 預設拒絕

對於非主理人生命體，系統檢查全域存取控制列表。如果沒有匹配的規則，請求預設被拒絕。

### GlobalACL 結構

```json
{
  "rules": [
    {
      "prefix": "network:api.github.com",
      "result": "Allowed"
    },
    {
      "prefix": "file:C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

規則按順序評估；首個匹配的規則生效。僅矽基主理人可以修改全域 ACL。

### 資源格式

```
{type}:{path}

範例：
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

當主理人操作需要使用者確認時，透過 `IPermissionAskHandler` 詢問使用者權限。

### IMPermissionAskHandler 實作

`IMPermissionAskHandler` 透過 Web UI 向使用者發送權限請求：

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        SendMessage($"Allow {resource}?");

        var response = WaitForResponse();

        return new AskPermissionResult
        {
            Allowed = response.Approved,
            AddToCache = response.AddToCache,
            CacheDuration = response.CacheDuration
        };
    }
}
```

### PermissionRequestQueue 權限請求佇列

`PermissionRequestQueue` 管理待處理的權限請求，支援非同步等待使用者回應：

- **請求入隊** — 當權限鏈到達主理人分支時，建立一個 `TaskCompletionSource<AskPermissionResult>` 並入隊
- **Web UI 展示** — 透過 `PermissionRequestController` 在 Web UI 中展示待處理的權限請求
- **使用者回應** — 使用者在 Web UI 中批准或拒絕，可選擇快取決策和設定快取持續時間
- **快取選項** — 使用者可以將權限決策快取 1 小時、24 小時、7 天或 30 天
- **逾時機制** — 60 秒無回應自動關閉請求頁面

## 稽核系統

所有權限決策都被記錄：

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "level": "GlobalACL",
  "reason": "Explicit rule granted"
}
```

## 程式化權限評估

### EvaluatePermission API

`PermissionManager.EvaluatePermission()` 方法提供唯讀的權限預評估，不會觸發使用者提示。`PermissionTool` 使用此方法讓 AI 在嘗試操作前檢查權限狀態。

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**回傳值**：三態 `PermissionResult`：
- `Allowed` - 操作被允許
- `Denied` - 操作被拒絕
- `AskUser` - 執行時需要使用者確認

**評估順序**：
1. **UserFrequencyCache** - 檢查快取的使用者決策
2. **IPermissionCallback** - 自訂回呼評估
3. **主理人分支** - 如果是主理人，回傳 `AskUser`（需要確認）；如果是非主理人，檢查 **GlobalACL**，然後預設拒絕

> **注意**：與完整權限鏈不同，`EvaluatePermission` **不會**呼叫 `IPermissionAskHandler`。它僅報告執行時的結果*將會是*什麼。

## 管理權限

### 授予權限

**透過 Web UI**：
1. 導航到**權限管理**
2. 點擊**新增規則**
3. 配置：
   - 使用者
   - 資源
   - 允許/拒絕
   - 持續時間

**透過 API**：
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### 撤銷權限

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### 檢視權限

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## 最佳實踐

### 1. 最小權限原則

僅授予所需的最小權限：

```json
{
  "resource": "disk:read",  // 不是 disk:*
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // 始終設定過期
}
```

### 2. 使用時間限制權限

除非絕對必要，否則永遠不要授予永久權限。

### 3. 監控權限日誌

定期檢視稽核日誌以了解：
- 拒絕的存取嘗試
- 異常模式
- 權限升級

### 4. 實作自訂回呼

對於複雜邏輯，使用 `IPermissionCallback`：

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // 基於時間的權限
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // 基於資源的權限
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## 常見場景

### 場景 1：AI 想要讀取檔案

```
AI："我需要讀取 config.json"
↓
權限鏈：
1. UserFrequencyCache？無快取決策
2. IPermissionCallback？返回 AskUser（未明確允許）
3. IsCurator？否 → 檢查 GlobalACL
4. GlobalACL？找到規則：file:... = 允許
5. 結果：允許
```

### 場景 2：AI 想要執行程式碼

```
AI："我想編譯和執行程式碼"
↓
權限鏈：
1. UserFrequencyCache？無快取決策
2. IPermissionCallback？返回 AskUser
3. IsCurator？是 → IPermissionAskHandler
4. 使用者批准
5. 結果：允許
```

### 場景 3：快取拒絕

```
AI："我需要存取 C:\Windows"
↓
權限鏈：
1. UserFrequencyCache？在 HighDeny 快取中找到
2. 結果：拒絕（無需進一步檢查）
```

## 故障排除

### 意外拒絕權限

**檢查**：
1. 使用者的 IsCurator 狀態
2. 速率限制設定
3. GlobalACL 規則
4. 回呼邏輯
5. 使用者回應逾時

### 權限未過期

**檢查**：
- `expiresAt` 欄位設定正確
- 時區正確
- 時鐘同步

### 稽核日誌未記錄

**檢查**：
- 稽核日誌器已註冊
- 儲存後端可存取
- 磁碟空間充足

## 下一步

- 📚 閱讀[架構指南](architecture.md)
- 🛠️ 查看[開發指南](development-guide.md)
- 🔒 查看[安全文件](security.md)
- 🚀 查看[快速開始指南](getting-started.md)
