# 权限系统

> **版本：v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | **中文** | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## 概述

权限系统确保所有 AI 发起的操作都经过适当验证和审计。

## 权限验证链

```
┌─────────────────────────────────────────────┐
│          权限验证                            │
├─────────────────────────────────────────────┤
│  级别 1：UserFrequencyCache                  │
│  ↓ 高频用户决策缓存（HighDeny/HighAllow）    │
│  级别 2：IPermissionCallback                 │
│  ↓ 自定义逻辑（Allowed/Denied/AskUser）     │
│  级别 3：IsCurator?                          │
│  ↓ 是 → IPermissionAskHandler（询问用户）    │
│  ↓ 否 → GlobalACL → 默认拒绝                │
│  结果：允许或拒绝                            │
└─────────────────────────────────────────────┘
```

> **注意**：`PermissionManager.CheckPermission()` 的实际查询优先级为：
> 1. **UserFrequencyCache** — 首先检查高频用户决策缓存
> 2. **IPermissionCallback** — 评估自定义回调规则
> 3. **主理人分支** — 当回调返回 AskUser 或无回调时：
>    - **主理人** → `IPermissionAskHandler`（通过 IM 询问用户）
>    - **非主理人** → `GlobalACL` → 默认拒绝

## 级别 1：UserFrequencyCache

每个生命体的高频用户决策缓存（HighDeny/HighAllow），仅存在于内存中。

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny 优先于 HighAllow**
- **仅内存**：缓存不持久化，重启后丢失
- **可配置过期时间**：用户可以设置缓存条目的有效期

## 级别 2：IPermissionCallback

用于动态权限逻辑的自定义回调。

### DefaultPermissionCallback 默认实现

`DefaultPermissionCallback` 提供了全面的默认权限规则，包括：

#### 网络访问规则
- **环回地址**：允许 localhost, 127.0.0.1, ::1
- **私有 IP 地址**：
  - 192.168.x.x (Class C) - 允许
  - 10.x.x.x (Class A) - 允许
  - 172.16-31.x.x (Class B) - 询问用户
- **域名白名单**：
  - 搜索引擎：Google, Bing, DuckDuckGo, Yandex, Sogou 等
  - AI 服务：OpenAI, Anthropic, HuggingFace, Ollama 等
  - 开发者服务：GitHub, StackOverflow, npm, NuGet 等
  - 社交媒体：微博、知乎、Reddit、Discord 等
  - 视频平台：YouTube, Bilibili, 抖音、TikTok 等
  - **天气信息**：wttr.in
  - 政府网站：.gov, .go.jp, .go.kr
- **域名黑名单**：
  - AI 冒充网站：chatgpt, openai, deepseek 等仿冒域名
  - 恶意 AI 工具：wormgpt, darkgpt, fraudgpt 等
  - AI 内容农场和黑色市场相关域名

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

## 级别 3：分支判断（IsCurator / GlobalACL）

当回调返回 `AskUser` 或没有配置回调时，系统根据主理人身份进行分支：

### 主理人分支（IsCurator = true）

对于硅基主理人，系统通过即时通讯向用户请求决策：

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // 用户在 Web UI 中确认或拒绝
    }
}
```

### 非主理人分支（IsCurator = false）

对于非主理人生命体，系统检查全局访问控制列表。如果没有匹配的规则，默认拒绝请求。

### GlobalACL 结构

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

规则按顺序评估，第一个匹配的规则生效。只有硅基主理人可以修改全局 ACL。

### 资源格式

```
{type}:{path}

示例：
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

当主理人操作需要用户确认时，通过 `IPermissionAskHandler` 询问用户权限。

### IMPermissionAskHandler 实现

`IMPermissionAskHandler` 通过 Web UI 向用户发送权限请求：

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // 通过即时通讯向用户发送消息
        SendMessageAsync($"Allow {resource}?");

        // 等待用户响应
        var response = WaitForResponseAsync();

        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### PermissionRequestQueue 权限请求队列

`PermissionRequestQueue` 管理待处理的权限请求，支持异步等待用户响应：

- **请求入队** — 当权限链到达级别 5 时，创建一个 `TaskCompletionSource<AskPermissionResult>` 并入队
- **Web UI 展示** — 通过 `PermissionRequestController` 在 Web UI 中展示待处理的权限请求
- **用户响应** — 用户在 Web UI 中批准或拒绝，可选择缓存决策和设置缓存持续时间
- **缓存选项** — 用户可以将权限决策缓存 1 小时、24 小时、7 天或 30 天
- **超时机制** — 60 秒无响应自动关闭请求页面

## 审计系统

所有权限决策都被记录：

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

## 程序化权限评估

### EvaluatePermission API

`PermissionManager.EvaluatePermission()` 方法提供只读的权限预评估，不会触发用户提示。`PermissionTool` 使用此方法让 AI 在尝试操作前检查权限状态。

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**返回值**：三态 `PermissionResult`：
- `Allowed` - 操作被允许
- `Denied` - 操作被拒绝
- `AskUser` - 执行时需要用户确认

**评估顺序**：
1. **频率缓存** - 检查缓存的用户决策
2. **IPermissionCallback** - 自定义回调评估
3. **主理人状态** - 如果是主理人，返回 `AskUser`（需要确认）
4. **全局 ACL** - 检查访问控制规则
5. **默认** - 无匹配规则时拒绝

> **注意**：与完整权限链不同，`EvaluatePermission` **不会**调用 `IPermissionAskHandler`。它仅报告执行时的结果*将会是*什么。

## 管理权限

### 授予权限

**通过 Web UI**：
1. 导航到**权限管理**
2. 点击**添加规则**
3. 配置：
   - 用户
   - 资源
   - 允许/拒绝
   - 持续时间

**通过 API**：
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

### 撤销权限

通过 Web UI 的权限管理页面操作。

### 查看权限

```bash
curl http://localhost:8080/api/permissions/list
```

## 工具权限系统

除了操作级别的权限验证链，系统还提供了**工具权限**管理机制，用于控制硅基生命体可以使用哪些工具。

### 两级工具权限

工具权限分为两个级别：

1. **硅基生命体级别** — 控制单个硅基生命体可以使用哪些工具操作
2. **项目级别** — 控制项目空间内可用的工具操作，独立于硅基生命体级别的权限

### 工具权限配置

每个工具的每个操作都可以独立配置为允许或拒绝：

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

### 权限模板

系统提供预定义的工具权限模板，可快速应用到硅基生命体：

- **readonly** — 只读权限（允许读取操作，拒绝写入操作）
- **full** — 完整权限（允许所有操作）
- **restricted** — 受限权限（仅允许基本操作）

### Web UI 管理

通过 Web UI 管理工具权限：

- **硅基生命体工具权限页面** — `/beings/tool-permissions`
- **项目工具权限页面** — `/project/{id}/tool-permissions`

### API 端点

| 端点 | 方法 | 描述 |
|------|------|------|
| `/api/beings/tool-permissions` | GET | 获取硅基生命体工具权限 |
| `/api/beings/tool-permissions` | PUT | 更新硅基生命体工具权限 |
| `/api/beings/tool-permissions/templates` | GET | 获取权限模板列表 |
| `/api/beings/tool-permissions/apply-template` | POST | 应用权限模板 |
| `/api/projects/{id}/tool-permissions` | GET | 获取项目工具权限 |
| `/api/projects/{id}/tool-permissions` | PUT | 更新项目工具权限 |

---

## 最佳实践

### 1. 最小权限原则

仅授予所需的最小权限：

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. 使用时间限制权限

除非绝对必要，否则永远不要授予永久权限。

### 3. 监控权限日志

定期查看审计日志以了解：
- 拒绝的访问尝试
- 异常模式
- 权限升级

### 4. 实现自定义回调

对于复杂逻辑，使用 `IPermissionCallback`：

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // 基于时间的权限
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // 基于资源的权限
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## 常见场景

### 场景 1：AI 想要读取文件

```
AI："我需要读取 config.json"
↓
权限链：
1. UserFrequencyCache？无缓存决策
2. IPermissionCallback？返回 AskUser（未明确允许）
3. IsCurator？否 → 检查 GlobalACL
4. GlobalACL？找到规则：file:... = Allowed
5. 结果：允许
```

### 场景 2：AI 想要执行代码

```
AI："我想编译和运行代码"
↓
权限链：
1. UserFrequencyCache？无缓存决策
2. IPermissionCallback？返回 AskUser
3. IsCurator？是 → IPermissionAskHandler
4. 用户批准
5. 结果：允许
```

### 场景 3：缓存拒绝

```
AI："我需要访问 C:\Windows"
↓
权限链：
1. UserFrequencyCache？在 HighDeny 缓存中找到
2. 结果：拒绝（无需进一步检查）
```

## 故障排除

### 意外拒绝权限

**检查**：
1. 用户的 IsCurator 状态
2. 频率缓存中的 HighDeny 条目
3. GlobalACL 规则
4. 回调逻辑
5. 用户响应超时

### 权限未过期

**检查**：
- `expiresAt` 字段设置正确
- 时区正确
- 时钟同步

### 审计日志未记录

**检查**：
- 审计日志器已注册
- 存储后端可访问
- 磁盘空间充足

## 下一步

- 📚 阅读[架构指南](architecture.md)
- 🛠️ 查看[开发指南](development-guide.md)
- 🔒 查看[安全文档](security.md)
- 🚀 查看[快速开始指南](getting-started.md)
