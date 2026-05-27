# PluginDemo-08: 禁止的网络操作反例

## 概述

本插件演示了 SiliconLife 插件系统中**禁止**的网络操作。它作为反模式参考，展示不应该做什么，并为每个违规提供正确的替代方案。

## 为什么直接网络访问被全局禁止？

整个直接网络访问模式在插件级别被阻止，因为它存在严重的安全风险：

1. **连接恶意服务器**：插件可能连接到恶意服务器接收攻击命令
2. **数据泄露**：插件可能将敏感数据从沙箱泄露到外部服务器
3. **DNS 重绑定攻击**：插件可能通过 DNS 操作绕过安全检查
4. **绕过网络 ACL**：直接网络访问绕过全局 ACL 和权限系统
5. **无审计追踪**：直接网络操作绕过插件安全审计系统
6. **资源耗尽**：不受控的网络请求可能压垮外部服务

## 禁止的类型

所有直接访问网络的 `System.Net` 类型都被阻止：

| 禁止类型 | 阻止的命名空间 | 风险等级 |
|----------|----------------|----------|
| `HttpClient` | `System.Net.Http` | 🔴 严重 |
| `HttpWebRequest/HttpWebResponse` | `System.Net` | 🔴 严重 |
| `TcpClient` | `System.Net.Sockets` | 🔴 严重 |
| `UdpClient` | `System.Net.Sockets` | 🔴 严重 |
| `Socket` | `System.Net.Sockets` | 🔴 严重 |
| `Dns` | `System.Net` | 🔴 严重 |
| `SmtpClient` | `System.Net.Mail` | 🔴 严重 |
| `WebClient` | `System.Net` | 🔴 严重 |
| `ClientWebSocket` | `System.Net.WebSockets` | 🔴 严重 |
| `SslStream` | `System.Net.Security` | 🔴 严重 |

## 如何安全地访问网络？

### NetworkExecutor（推荐用于所有网络操作）

`NetworkExecutor` 是插件网络操作的**受控入口点**：

```csharp
// ✅ 正确：简单 GET 请求
var getResult = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data",
    Parameters = new Dictionary<string, object>
    {
        { "method", "GET" }
    }
});

// ✅ 正确：带请求体的 POST 请求
var postResult = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/submit",
    Parameters = new Dictionary<string, object>
    {
        { "method", "POST" },
        { "body", "{\"key\": \"value\"}" }
    }
});

// ✅ 正确：带自定义请求头的请求
var resultWithHeaders = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data",
    Parameters = new Dictionary<string, object>
    {
        { "method", "GET" },
        { "headers", new Dictionary<string, string> 
            { 
                { "Authorization", "Bearer token123" },
                { "Accept", "application/json" }
            } 
        }
    }
});
```

**NetworkExecutor 提供的功能：**

1. **权限检查**：确保网络访问在允许范围内（工作区限制 + 全局 ACL）
2. **审计日志**：所有网络访问都会被记录以供安全审查
3. **熔断器**：连续失败后自动停止执行器以防止级联故障
4. **超时控制**：默认 30 秒超时可防止连接挂起
5. **请求排队**：防止过多并发请求导致资源耗尽
6. **DNS 处理**：安全的 DNS 解析，不暴露给插件

### 执行器类型对比

| 执行器 | 范围 | 默认超时 |
|--------|------|----------|
| `DiskExecutor` | 文件读写、目录操作 | 30 秒 |
| `NetworkExecutor` | HTTP 请求、WebSocket 连接 | 60 秒 |
| `CommandLineExecutor` | Shell 命令执行 | 120 秒 |

## 演示的违规

本插件展示了 9 种常见的网络操作违规：

### 违规 1：HttpClient

```csharp
// ❌ 禁止
using var client = new HttpClient();
var response = await client.GetStringAsync("https://api.example.com/data");

// ✅ 正确
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

**阻止的 TypeRef**：`System.Net.Http.HttpClient`

### 违规 2：HttpWebRequest

```csharp
// ❌ 禁止
var request = WebRequest.Create("https://api.example.com");
var response = request.GetResponse();

// ✅ 正确
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

**阻止的 TypeRef**：`System.Net.HttpWebRequest`

### 违规 3：TcpClient

```csharp
// ❌ 禁止
using var client = new TcpClient("example.com", 8080);
var stream = client.GetStream();

// ✅ 正确
// 使用 NetworkExecutor 处理 HTTP/HTTPS 端点
// 对于原始 TCP，需要声明 Capability.Network
```

**阻止的 TypeRef**：`System.Net.Sockets.TcpClient`

### 违规 4：UdpClient

```csharp
// ❌ 禁止
using var udp = new UdpClient();
udp.Send(data, data.Length, "example.com", 9000);

// ✅ 正确
// UDP 需要 Capability.Network 声明
```

**阻止的 TypeRef**：`System.Net.Sockets.UdpClient`

### 违规 5：Socket

```csharp
// ❌ 禁止
var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Connect("example.com", 80);

// ✅ 正确
// 使用 NetworkExecutor 处理标准协议
// 自定义 socket 使用需声明 Capability.Network
```

**阻止的 TypeRef**：`System.Net.Sockets.Socket`

### 违规 6：Dns

```csharp
// ❌ 禁止
var hostEntry = Dns.GetHostEntry("example.com");

// ✅ 正确
// NetworkExecutor 内部处理 DNS
// 使用 NetworkExecutor.Execute 时指定目标 URL 即可
```

**阻止的 TypeRef**：`System.Net.Dns`

### 违规 7：SmtpClient

```csharp
// ❌ 禁止
using var smtp = new SmtpClient("smtp.example.com", 587);
smtp.Send(mailMessage);

// ✅ 正确
// 使用邮件 API 服务并声明 Capability.Network
```

**阻止的 TypeRef**：`System.Net.Mail.SmtpClient`

### 违规 8：WebClient

```csharp
// ❌ 禁止
using var client = new WebClient();
var data = client.DownloadString("https://api.example.com");

// ✅ 正确
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

**阻止的 TypeRef**：`System.Net.WebClient`

### 违规 9：ClientWebSocket

```csharp
// ❌ 禁止
using var ws = new ClientWebSocket();
await ws.ConnectAsync(uri, CancellationToken.None);

// ✅ 正确
// WebSocket 支持取决于 NetworkExecutor 的实现
// 可能需要声明 Capability.Network
```

**阻止的 TypeRef**：`System.Net.WebSockets.ClientWebSocket`

## 与其他示例的对比

| 示例 | 重点 | 所需权限 |
|------|------|----------|
| **08-ForbiddenNetwork** | 禁止的网络访问模式（本示例） | 不适用（被阻止） |
| **13-CapabilityNetwork** | 声明 Network 权限以访问网络 | `Capability.Network` |

**主要区别：**
- **08-ForbiddenNetwork**：展示不应该做什么（直接网络访问）
- **13-CapabilityNetwork**：展示如何**声明式**请求网络访问权限

## PluginLoader 安全机制

当 PluginLoader 扫描此插件时：

1. **TypeRef 扫描**：检测对禁止的 `System.Net.*` 类型的引用
2. **MemberRef 扫描**：检测对阻止方法的调用
3. **IL 字符串扫描**：检测通过字符串反射加载禁止类型的尝试
4. **拒绝**：插件在加载时被拒绝，并显示详细错误信息

**无法通过以下方式绕过：**
- 字符串拼接（`"System.Net" + ".Http"`）
- 反射（`Type.GetType("System.Net.Http.HttpClient")`）
- 动态加载（`Assembly.Load`）
- 混淆或加密

## 最佳实践

1. **始终使用 NetworkExecutor**：所有 HTTP/HTTPS 请求都应通过 NetworkExecutor
2. **必要时声明 Capability.Network**：如果需要原始网络访问，声明 `Capability.Network`（参见 13-CapabilityNetwork）
3. **避免直接使用 System.Net 类型**：永远不要直接实例化 `HttpClient`、`TcpClient`、`Socket`
4. **记住**：NetworkExecutor 安全处理 DNS——插件不应直接使用 `Dns` 类
5. **尊重超时**：通过执行器的网络操作有内置的超时保护

## 文件

- `Plugin.cs` - 反模式演示插件
- `README.md` - 本文件（英文）
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## 相关示例

- **13-CapabilityNetwork**：声明式 Network 权限（请求网络访问的正确方式）
- **07-ForbiddenFileIO**：禁止的文件访问模式
- **15-CapabilityProcess**：声明 Process 权限