# PluginDemo-17: Capability Stacking — Multiple Declarative Permissions

## Overview

This plugin demonstrates stacking multiple `[PluginCapability]` attributes on a single plugin class. `PluginCapabilityAttribute` has `AllowMultiple = true`, so you can declare as many capabilities as needed.

## Stacking Syntax

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## How PluginLoader Processes Stacked Capabilities

1. **Reads all declarations** from PE metadata CustomAttribute table
2. **Merges exemption rules** from all declared capabilities
3. **Logs each declaration independently** with its own Reason field
4. **Still enforces undeclarable capability bans** regardless of stacking

## Merged Exemption Rules

When stacking `Capability.Network` + `Capability.AI`:

| Source | Exemption |
|--------|----------|
| Capability.Network | System.Net.Http.*, System.Net.WebSockets.*, System.Net.Sockets.*, System.Net.Mail.*, System.Net.NetworkInformation.*, System.Net.Security.*, System.Net (per-type bans) |
| Capability.AI | IAIService injection enabled |
| **Combined** | Plugin can use HttpClient AND IAIService |

## Stacking Does NOT Grant Unlimited Power

Even with multiple stacked capabilities, these remain **always blocked**:

- ❌ P/Invoke (`DllImport`, `Marshal`, `NativeMemory`)
- ❌ Unsafe code (`UnverifiableCodeAttribute`, `Unsafe`)
- ❌ IL Emission (`System.Reflection.Emit.*`)
- ❌ Assembly Loading (`System.Runtime.Loader`, `Assembly.Load*`)
- ❌ Registry (`Microsoft.Win32.*`)

No `Capability` enum value exists for these — they are **undeclarable** by design.

## Audit Trail for Stacked Capabilities

Each capability is logged independently:

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## Related Examples

- **13-CapabilityNetwork**: Single Network capability
- **16-CapabilityAI**: Single AI capability
- **18-CapabilityDenied**: Undeclarable capability anti-pattern
