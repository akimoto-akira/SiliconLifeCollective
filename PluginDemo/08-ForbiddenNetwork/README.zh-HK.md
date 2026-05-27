# PluginDemo-08: 禁止的網絡操作反例

## 概述

本插件演示了 SiliconLife 插件系統中**禁止**的網絡操作。它作為反模式參考，展示不應該做什麼，並為每個違規提供正確的替代方案。

## 為什麼直接網絡訪問被全局禁止？

整個直接網絡訪問模式在插件級別被阻止，因為它存在嚴重的安全風險：

1. **連接惡意服務器**：插件可能連接到惡意服務器接收攻擊命令
2. **數據洩漏**：插件可能將敏感數據從沙箱洩露到外部服務器
3. **DNS 重綁定攻擊**：插件可能通過 DNS 操作繞過安全檢查
4. **繞過網絡 ACL**：直接網絡訪問繞過全局 ACL 和權限系統
5. **無審計追蹤**：直接網絡操作繞過插件安全審計系統
6. **資源耗盡**：不受控的網絡請求可能壓垮外部服務

## 禁止的類型

所有直接訪問網絡的 `System.Net` 類型都被阻止：

| 禁止類型 | 阻止的命名空間 | 風險等級 |
|----------|----------------|----------|
| `HttpClient` | `System.Net.Http` | 🔴 嚴重 |
| `HttpWebRequest/HttpWebResponse` | `System.Net` | 🔴 嚴重 |
| `TcpClient` | `System.Net.Sockets` | 🔴 嚴重 |
| `UdpClient` | `System.Net.Sockets` | 🔴 嚴重 |
| `Socket` | `System.Net.Sockets` | 🔴 嚴重 |
| `Dns` | `System.Net` | 🔴 嚴重 |
| `SmtpClient` | `System.Net.Mail` | 🔴 嚴重 |
| `WebClient` | `System.Net` | 🔴 嚴重 |
| `ClientWebSocket` | `System.Net.WebSockets` | 🔴 嚴重 |
| `SslStream` | `System.Net.Security` | 🔴 嚴重 |

## 如何安全地訪問網絡？

### NetworkExecutor（推薦用於所有網絡操作）

`NetworkExecutor` 是插件網絡操作的**受控入口點**：

```csharp
// ✅ 正確：簡單 GET 請求
var getResult = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data",
    Parameters = new Dictionary<string, object>
    {
        { "method", "GET" }
    }
});
```

**NetworkExecutor 提供的功能：**

1. **權限檢查**：確保網絡訪問在允許範圍內
2. **審計日誌**：所有網絡訪問都會被記錄
3. **熔斷器**：連續失敗後自動停止以防止級聯故障
4. **超時控制**：默認 30 秒超時
5. **請求排隊**：防止資源耗盡
6. **DNS 處理**：安全的 DNS 解析

## 演示的違規

本插件展示了 9 種常見的網絡操作違規：

### 違規 1：HttpClient

```csharp
// ❌ 禁止
using var client = new HttpClient();
var response = await client.GetStringAsync("https://api.example.com/data");

// ✅ 正確
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

### 違規 2：TcpClient

```csharp
// ❌ 禁止
using var client = new TcpClient("example.com", 8080);

// ✅ 正確
// 使用 NetworkExecutor 或聲明 Capability.Network
```

### 違規 3：Socket

```csharp
// ❌ 禁止
var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

// ✅ 正確
// 使用 NetworkExecutor
```

### 違規 4：Dns

```csharp
// ❌ 禁止
var hostEntry = Dns.GetHostEntry("example.com");

// ✅ 正確
// NetworkExecutor 內部處理 DNS
```

## 與其他示例的對比

| 示例 | 重點 | 所需權限 |
|------|------|----------|
| **08-ForbiddenNetwork** | 禁止的網絡訪問模式 | 不適用（被阻止） |
| **13-CapabilityNetwork** | 聲明 Network 權限 | `Capability.Network` |

## PluginLoader 安全機制

當 PluginLoader 掃描此插件時會：
1. **TypeRef 掃描**：檢測對禁止的 `System.Net.*` 類型的引用
2. **MemberRef 掃描**：檢測對阻止方法的調用
3. **IL 字符串掃描**：檢測反射加載禁止類型的嘗試
4. **拒絕**：插件在加載时被拒絕

## 最佳實踐

1. **始終使用 NetworkExecutor**：所有 HTTP/HTTPS 請求都應通過 NetworkExecutor
2. **必要時聲明 Capability.Network**：如果需要原始網絡訪問
3. **避免直接使用 System.Net 類型**
4. **尊重超時**

## 文件

- `Plugin.cs` - 反模式演示插件
- `README.md` - 本文件（英文）
- `README.zh-CN.md` - 簡體中文
- `README.zh-HK.md` - 繁體中文（本文件）
- 其他語言 README 文件...

## 相關示例

- **13-CapabilityNetwork**：聲明式 Network 權限
- **07-ForbiddenFileIO**：禁止的文件訪問模式