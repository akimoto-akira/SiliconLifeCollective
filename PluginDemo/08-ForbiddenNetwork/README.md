# PluginDemo-08: Forbidden Network Access Anti-Pattern

## Overview

This plugin demonstrates **FORBIDDEN** network operations in the SiliconLife plugin system. It serves as an anti-pattern reference, showing what NOT to do and providing correct alternatives for each violation.

## Why is Direct Network Access Globally Banned?

The entire direct network access pattern is blocked at the plugin level because it poses severe security risks:

1. **Malicious Server Connections**: Plugins could connect to malicious servers to receive attack commands
2. **Data Exfiltration**: Plugins could exfiltrate sensitive data from the sandbox to external servers
3. **DNS Rebinding Attacks**: Plugins could bypass security checks through DNS manipulation
4. **Network ACL Bypass**: Direct network access bypasses the Global ACL and permission system
5. **No Audit Trail**: Direct network operations bypass the plugin security audit system
6. **Resource Exhaustion**: Uncontrolled network requests could overwhelm external services

## What Types are Forbidden?

All `System.Net` types that directly access the network are blocked:

| Forbidden Type | Blocked Namespace | Risk Level |
|----------------|-------------------|------------|
| `HttpClient` | `System.Net.Http` | 🔴 Critical |
| `HttpWebRequest/HttpWebResponse` | `System.Net` | 🔴 Critical |
| `TcpClient` | `System.Net.Sockets` | 🔴 Critical |
| `UdpClient` | `System.Net.Sockets` | 🔴 Critical |
| `Socket` | `System.Net.Sockets` | 🔴 Critical |
| `Dns` | `System.Net` | 🔴 Critical |
| `SmtpClient` | `System.Net.Mail` | 🔴 Critical |
| `WebClient` | `System.Net` | 🔴 Critical |
| `ClientWebSocket` | `System.Net.WebSockets` | 🔴 Critical |
| `SslStream` | `System.Net.Security` | 🔴 Critical |

## How to Access Network Safely?

### NetworkExecutor (Recommended for All Network Operations)

`NetworkExecutor` is the **controlled entry point** for network operations in plugins:

```csharp
// ✅ CORRECT: Simple GET request
var getResult = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data",
    Parameters = new Dictionary<string, object>
    {
        { "method", "GET" }
    }
});

// ✅ CORRECT: POST request with body
var postResult = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/submit",
    Parameters = new Dictionary<string, object>
    {
        { "method", "POST" },
        { "body", "{\"key\": \"value\"}" }
    }
});

// ✅ CORRECT: Request with custom headers
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

**What NetworkExecutor Provides:**

1. **Permission Checking**: Ensures network access is within allowed scope (workspace restrictions + Global ACL)
2. **Audit Logging**: All network access is recorded for security review
3. **Circuit Breaker**: Automatically stops executor after consecutive failures to prevent cascade
4. **Timeout Control**: Default 30-second timeout prevents hanging connections
5. **Request Queuing**: Prevents resource exhaustion from too many concurrent requests
6. **DNS Handling**: Secure DNS resolution without exposing to plugins

### Executor Types Comparison

| Executor | Scope | Default Timeout |
|----------|-------|-----------------|
| `DiskExecutor` | File read/write, directory operations | 30 seconds |
| `NetworkExecutor` | HTTP requests, WebSocket connections | 60 seconds |
| `CommandLineExecutor` | Shell command execution | 120 seconds |

## Violations Demonstrated

This plugin shows 9 common network operation violations:

### Violation 1: HttpClient

```csharp
// ❌ FORBIDDEN
using var client = new HttpClient();
var response = await client.GetStringAsync("https://api.example.com/data");

// ✅ CORRECT
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

**Blocked TypeRef**: `System.Net.Http.HttpClient`

### Violation 2: HttpWebRequest

```csharp
// ❌ FORBIDDEN
var request = WebRequest.Create("https://api.example.com");
var response = request.GetResponse();

// ✅ CORRECT
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

**Blocked TypeRef**: `System.Net.HttpWebRequest`

### Violation 3: TcpClient

```csharp
// ❌ FORBIDDEN
using var client = new TcpClient("example.com", 8080);
var stream = client.GetStream();

// ✅ CORRECT
// Use NetworkExecutor for HTTP/HTTPS endpoints
// For raw TCP, declare Capability.Network
```

**Blocked TypeRef**: `System.Net.Sockets.TcpClient`

### Violation 4: UdpClient

```csharp
// ❌ FORBIDDEN
using var udp = new UdpClient();
udp.Send(data, data.Length, "example.com", 9000);

// ✅ CORRECT
// UDP requires Capability.Network declaration
```

**Blocked TypeRef**: `System.Net.Sockets.UdpClient`

### Violation 5: Socket

```csharp
// ❌ FORBIDDEN
var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Connect("example.com", 80);

// ✅ CORRECT
// Use NetworkExecutor for standard protocols
// Declare Capability.Network for custom socket usage
```

**Blocked TypeRef**: `System.Net.Sockets.Socket`

### Violation 6: Dns

```csharp
// ❌ FORBIDDEN
var hostEntry = Dns.GetHostEntry("example.com");

// ✅ CORRECT
// NetworkExecutor handles DNS internally
// Use NetworkExecutor.Execute with target URL
```

**Blocked TypeRef**: `System.Net.Dns`

### Violation 7: SmtpClient

```csharp
// ❌ FORBIDDEN
using var smtp = new SmtpClient("smtp.example.com", 587);
smtp.Send(mailMessage);

// ✅ CORRECT
// Use email API service with Capability.Network declaration
```

**Blocked TypeRef**: `System.Net.Mail.SmtpClient`

### Violation 8: WebClient

```csharp
// ❌ FORBIDDEN
using var client = new WebClient();
var data = client.DownloadString("https://api.example.com");

// ✅ CORRECT
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

**Blocked TypeRef**: `System.Net.WebClient`

### Violation 9: ClientWebSocket

```csharp
// ❌ FORBIDDEN
using var ws = new ClientWebSocket();
await ws.ConnectAsync(uri, CancellationToken.None);

// ✅ CORRECT
// WebSocket support depends on NetworkExecutor implementation
// May require Capability.Network declaration
```

**Blocked TypeRef**: `System.Net.WebSockets.ClientWebSocket`

## Comparison with Other Examples

| Example | Focus | Permission Required |
|---------|-------|---------------------|
| **08-ForbiddenNetwork** | Forbidden network access patterns (this example) | N/A (blocked) |
| **13-CapabilityNetwork** | Declaring Network capability to access network | `Capability.Network` |

**Key Difference:**
- **08-ForbiddenNetwork**: Shows what you CANNOT do (direct network access)
- **13-CapabilityNetwork**: Shows how to DECLARATIVELY request network access permission

## PluginLoader Security Mechanism

When PluginLoader scans this plugin:

1. **TypeRef Scanning**: Detects references to forbidden `System.Net.*` types
2. **MemberRef Scanning**: Detects calls to blocked methods
3. **IL String Scanning**: Detects string-based reflection attempts to load forbidden types
4. **Rejection**: Plugin is rejected during loading with detailed error message

**Cannot be bypassed by:**
- String concatenation (`"System.Net" + ".Http"`)
- Reflection (`Type.GetType("System.Net.Http.HttpClient")`)
- Dynamic loading (`Assembly.Load`)
- Obfuscation or encryption

## Best Practices

1. **Always use NetworkExecutor**: For all HTTP/HTTPS requests, use NetworkExecutor
2. **Declare Capability.Network if Necessary**: If you need raw network access, declare `Capability.Network` (see 13-CapabilityNetwork)
3. **Avoid Direct System.Net Types**: Never instantiate `HttpClient`, `TcpClient`, `Socket` directly
4. **Remember**: NetworkExecutor handles DNS securely — plugins should not use `Dns` class directly
5. **Respect Timeouts**: Network operations through Executor have built-in timeout protection

## Files

- `Plugin.cs` - Anti-pattern demonstration plugin
- `README.md` - This file (English)
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

## Related Examples

- **13-CapabilityNetwork**: Declarative Network capability (correct way to request network access)
- **07-ForbiddenFileIO**: Forbidden file access patterns
- **15-CapabilityProcess**: Declaring Process capability