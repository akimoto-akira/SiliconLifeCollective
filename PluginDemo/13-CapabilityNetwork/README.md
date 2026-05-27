# PluginDemo-13: Capability.Network — Declarative Network Permission

## Overview

This plugin demonstrates how to use `[PluginCapability(Capability.Network)]` to declare that a plugin requires network access. By declaring this capability, the plugin gains access to `System.Net.*` types that would otherwise be blocked by PluginLoader's security scan.

## PluginCapability Declaration Syntax

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**Key elements:**
- **Attribute target**: Must be on the class that directly implements `IPlugin`
- **AllowMultiple = true**: You can stack multiple `[PluginCapability]` attributes (see 17-CapabilityStacked)
- **Reason field**: Human-readable explanation written to the security audit log at load time. **Providing a clear reason is strongly recommended for all production plugins.**

## How PluginLoader Processes Capability Declarations

1. **PE Metadata Read**: PluginLoader reads capability declarations from the CustomAttribute table in the PE file **before** the security scan begins
2. **Scan Rule Relaxation**: Declared capabilities exempt corresponding type references from forbidden-namespace and forbidden-type checks
3. **Audit Logging**: All declarations (including Reason) are written to the security audit log
4. **Undeclarable Capabilities**: P/Invoke, Unsafe, Reflection.Emit, etc. remain blocked regardless of any declaration

## Capability.Network Exemption Scope

### TypeRef Exemptions

When `Capability.Network` is declared, the following namespace-based and type-based forbidden rules are relaxed:

| Exempted Namespace | Allowed Types |
|-------------------|---------------|
| `System.Net.Http` | `HttpClient`, `HttpRequestMessage`, `HttpResponseMessage`, etc. |
| `System.Net.WebSockets` | `ClientWebSocket`, `WebSocket`, etc. |
| `System.Net.Sockets` | `TcpClient`, `UdpClient`, `Socket`, etc. |
| `System.Net.Mail` | `SmtpClient`, `MailMessage`, etc. |
| `System.Net.NetworkInformation` | `Ping`, `NetworkInterface`, etc. |
| `System.Net.Security` | `SslStream`, etc. |
| `System.Net` (per-type bans) | `HttpWebRequest`, `WebClient`, `Dns`, `FtpWebRequest`, etc. |

### ILString Exemptions

String constants starting with these prefixes are not flagged in #US heap scanning:
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### What Remains Forbidden

Even with `Capability.Network`, these are **always** blocked (undeclarable capabilities):

| Category | Blocked Types | Why Undeclarable |
|----------|--------------|------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` | Cannot be safely audited at runtime |
| Unsafe code | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Bypasses type safety guarantees |
| IL Emission | `System.Reflection.Emit.*` | Can generate arbitrary code at runtime |
| Assembly Loading | `System.Runtime.Loader`, `Assembly.Load*` | Can bypass security scan by loading unchecked DLLs |
| Registry | `Microsoft.Win32.*` | OS-level system access outside plugin sandbox |

## Reason Field — Audit Role

The `Reason` field serves as an **audit trail** for capability declarations:

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Why Reason matters:**
1. **Security review**: Auditors can verify that declared capabilities match actual plugin behavior
2. **Principle of least privilege**: Forces plugin authors to justify why they need each capability
3. **Compliance**: Required for security certifications and incident investigations
4. **Runtime monitoring**: Security tools can alert if declared capability usage exceeds stated reason

## Comparison with 08-ForbiddenNetwork

| Aspect | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|--------|-------------------|---------------------|
| Declaration | None | `[PluginCapability(Capability.Network)]` |
| Load result | ❌ REJECTED by PluginLoader | ✅ LOADED successfully |
| HttpClient usage | Blocked by TypeRef scan | Exempted by capability |
| TcpClient usage | Blocked by TypeRef scan | Exempted by capability |
| Reason | N/A | Written to audit log |

**Key difference**: 08-ForbiddenNetwork shows what happens when you try to use network types **without** declaring the capability. 13-CapabilityNetwork shows the **correct** way to declaratively request network access.

## Security Best Practices

1. **Declare only what you need**: If you only need HTTP, don't declare Capability.Network just because you can — but note that Capability.Network is the only network-related capability; there are no finer-grained options
2. **Use NetworkExecutor when possible**: `NetworkExecutor` is the controlled entry point for network access and doesn't require any capability declaration
3. **Provide a clear Reason**: Vague reasons like "network access" are a red flag during security review
4. **Remember undeclarable limits**: No capability declaration can bypass P/Invoke, Unsafe, or Reflection.Emit bans

## Files

- `Plugin.cs` — Demo plugin declaring Capability.Network
- `README.md` — This file (English)
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 日本語
- `README.ko-KR.md` — 한국어
- `README.de-DE.md` — Deutsch
- `README.fr-FR.md` — Français
- `README.es-ES.md` — Español
- `README.it-IT.md` — Italiano
- `README.ru-RU.md` — Русский
- `README.pt-PT.md` — Português
- `README.pl-PL.md` — Polski
- `README.cs-CZ.md` — Čeština

## Related Examples

- **08-ForbiddenNetwork**: Anti-pattern showing blocked network operations
- **14-CapabilityFileIO**: Declarative FileIO capability
- **15-CapabilityProcess**: Declarative Process capability
- **16-CapabilityAI**: Declarative AI service capability
- **17-CapabilityStacked**: Multiple capability stacking
- **18-CapabilityDenied**: Undeclarable capability anti-pattern
