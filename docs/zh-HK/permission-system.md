# 權限系統

> **版本：v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | **繁體中文** | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## 概述

權限系統確保所有 AI 發起的操作都經過適當驗證和審計。

## 權限驗證鏈

```
┌─────────────────────────────────────────────┐
│          權限驗證                            │
├─────────────────────────────────────────────┤
│  级别 1：UserFrequencyCache                  │
│  ↓ 高頻使用者決策快取（HighDeny/HighAllow）  │
│  级别 2：IPermissionCallback                 │
│  ↓ 自訂邏輯（Allowed/Denied/AskUser）       │
│  级别 3：IsCurator?                          │
│  ↓ 是 → IPermissionAskHandler（詢問使用者）  │
│  ↓ 否 → GlobalACL → 預設拒絕                │
│  結果：允許或拒絕                            │
└─────────────────────────────────────────────┘
```

> **注意**：`PermissionManager.CheckPermission()` 的實際查詢優先順序為：
> 1. **UserFrequencyCache** — 首先檢查高頻使用者決策快取
> 2. **IPermissionCallback** — 評估自訂回呼規則
> 3. **主理人分支** — 當回呼傳回 AskUser 或無回呼時：
>    - **主理人** → `IPermissionAskHandler`（透過 IM 詢問使用者）
>    - **非主理人** → `GlobalACL` → 預設拒絕

## 级别 1：UserFrequencyCache

每個生命體的高頻使用者決策快取（HighDeny/HighAllow），僅存在於記憶體中。

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny 優先於 HighAllow**
- **僅記憶體**：快取不持久化，重啟後遺失
- **可設定過期時間**：使用者可以設定快取條目的有效期

## 级别 2：IPermissionCallback

用於動態權限邏輯的自訂回呼。

### DefaultPermissionCallback 預設實作

`DefaultPermissionCallback` 提供了全面的預設權限規則，包括：

#### 網路存取規則
- **環回位址**：允許 localhost, 127.0.0.1, ::1
- **私有 IP 位址**：
  - 192.168.x.x (Class C) - 允許
  - 10.x.x.x (Class A) - 允許
  - 172.16-31.x.x (Class B) - 詢問使用者
- **域名白名單**：
  - 搜尋引擎：Google, Bing, DuckDuckGo, Yandex, Sogou 等
  - AI 服務：OpenAI, Anthropic, HuggingFace, Ollama 等
  - 開發者服務：GitHub, StackOverflow, npm, NuGet 等
  - 社群媒體：微博、知乎、Reddit、Discord 等
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

## 级别 3：分支判斷（IsCurator / GlobalACL）

當回呼傳回 `AskUser` 或沒有設定回呼時，系統根據主理人身份進行分支：

### 主理人分支（IsCurator = true）

對於矽基主理人，系統透過即時通訊向使用者請求決策：

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // 使用者在 Web UI 中確認或拒絕
    }
}
```

### 非主理人分支（IsCurator = false）

對於非主理人生命體，系統檢查全域存取控制清單。如果沒有匹配的規則，預設拒絕請求。

### GlobalACL 結構

```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed"
    },
    {
      "permissionType": "FileAccess",
      "resourcePrefix": "C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

規則按順序評估，第一個匹配的規則生效。只有矽基主理人可以修改全域 ACL。

### 資源格式

```
{type}:{path}

示例：
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

當主理人操作需要使用者確認時，透過 `IPermissionAskHandler` 詢問使用者權限。

### IMPermissionAskHandler 實作

`IMPermissionAskHandler` 透過 Web UI 向使用者傳送權限請求：

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // 透過即時通訊向使用者傳送訊息
        SendMessageAsync($"Allow {resource}?");

        // 等待使用者回應
        var response = WaitForResponseAsync();

        return response.Approved
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### PermissionRequestQueue 權限請求佇列

`PermissionRequestQueue` 管理待處理的權限請求，支援非同步等待使用者回應：

- **請求入隊** — 當權限鏈到達级别 5 時，建立一個 `TaskCompletionSource<AskPermissionResult>` 並入隊
- **Web UI 展示** — 透過 `PermissionRequestController` 在 Web UI 中展示待處理的權限請求
- **使用者回應** — 使用者在 Web UI 中批准或拒絕，可選擇快取決策和設定快取持續時間
- **快取選項** — 使用者可以將權限決策快取 1 小時、24 小時、7 天或 30 天
- **逾時機制** — 60 秒無回應自動關閉請求頁面

## 審計系統

所有權限決策都被記錄：

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "callerId": "being-uuid",
  "permissionType": "FileAccess",
  "resource": "C:\\data\\config.json",
  "result": "Allowed",
  "reason": "Global ACL"
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

**傳回值**：三態 `PermissionResult`：
- `Allowed` - 操作被允許
- `Denied` - 操作被拒絕
- `AskUser` - 執行時需要使用者確認

**評估順序**：
1. **頻率快取** - 檢查快取的使用者決策
2. **IPermissionCallback** - 自訂回呼評估
3. **主理人狀態** - 如果是主理人，傳回 `AskUser`（需要確認）
4. **全域 ACL** - 檢查存取控制規則
5. **預設** - 無匹配規則時拒絕

> **注意**：與完整權限鏈不同，`EvaluatePermission` **不會**呼叫 `IPermissionAskHandler`。它僅報告執行時的結果*將會是*什麼。

## 管理權限

### 授予權限

**透過 Web UI**：
1. 導航到**權限管理**
2. 點擊**新增規則**
3. 設定：
   - 使用者
   - 資源
   - 允許/拒絕
   - 持續時間

**透過 API**：
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

### 撤銷權限

透過 Web UI 的權限管理頁面操作。

### 檢視權限

```bash
curl http://localhost:8080/api/permissions/list
```

## 工具權限系統

除了操作级别的權限驗證鏈，系統還提供了**工具權限**管理機制，用於控制矽基生命體可以使用哪些工具。

### 兩級工具權限

工具權限分為兩個级别：

1. **矽基生命體级别** — 控制單個矽基生命體可以使用哪些工具操作
2. **專案级别** — 控制專案空間內可用的工具操作，獨立於矽基生命體级别的權限

### 工具權限設定

每個工具的每個操作都可以獨立設定為允許或拒絕：

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network:get": "allowed",
    "network:post": "denied",
    "disk:read": "allowed",
    "disk:write": "denied",
    "database:query": "allowed"
  }
}
```

### 權限模板

系統提供預先定義的工具權限模板，可快速套用到矽基生命體：

- **readonly** — 唯讀權限（允許讀取操作，拒絕寫入操作）
- **full** — 完整權限（允許所有操作）
- **restricted** — 受限權限（僅允許基本操作）

### Web UI 管理

透過 Web UI 管理工具權限：

- **矽基生命體工具權限頁面** — `/beings/tool-permissions`
- **專案工具權限頁面** — `/project/{id}/tool-permissions`

### API 端點

| 端點 | 方法 | 描述 |
|------|------|------|
| `/api/beings/tool-permissions` | GET | 取得矽基生命體工具權限 |
| `/api/beings/tool-permissions` | PUT | 更新矽基生命體工具權限 |
| `/api/beings/tool-permissions/templates` | GET | 取得權限模板清單 |
| `/api/beings/tool-permissions/apply-template` | POST | 套用權限模板 |
| `/api/projects/{id}/tool-permissions` | GET | 取得專案工具權限 |
| `/api/projects/{id}/tool-permissions` | PUT | 更新專案工具權限 |

---

## 最佳實踐

### 1. 最小權限原則

僅授予所需的最小權限：

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. 使用時間限制權限

除非絕對必要，否則永遠不要授予永久權限。

### 3. 監控權限日誌

定期檢視審計日誌以了解：
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
2. IPermissionCallback？傳回 AskUser（未明確允許）
3. IsCurator？否 → 檢查 GlobalACL
4. GlobalACL？找到規則：file:... = Allowed
5. 結果：允許
```

### 場景 2：AI 想要執行程式碼

```
AI："我想編譯和執行程式碼"
↓
權限鏈：
1. UserFrequencyCache？無快取決策
2. IPermissionCallback？傳回 AskUser
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
2. 頻率快取中的 HighDeny 條目
3. GlobalACL 規則
4. 回呼邏輯
5. 使用者回應逾時

### 權限未過期

**檢查**：
- `expiresAt` 欄位設定正確
- 時區正確
- 時鐘同步

### 審計日誌未記錄

**檢查**：
- 審計日誌器已註冊
- 儲存後端可存取
- 磁碟空間充足

## 下一步

- 📚 閱讀[架構指南](architecture.md)
- 🛠️ 檢視[開發指南](development-guide.md)
- 🔒 檢視[安全文件](security.md)
- 🚀 檢視[快速開始指南](getting-started.md)
