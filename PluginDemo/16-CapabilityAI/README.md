# PluginDemo-16: Capability.AI — Declarative AI Service Permission

## Overview

This plugin demonstrates how to use `[PluginCapability(Capability.AI)]` to declare that a plugin requires access to the AI service. Unlike other capabilities, `Capability.AI` does **not** exempt any forbidden namespace — instead, it enables the host to inject an `IAIService` reference into the plugin.

## Key Concept: Capability.AI Does NOT Grant Network Access

`Capability.AI` is fundamentally different from the other capabilities:

| Capability | What it exempts | How it works |
|-----------|----------------|-------------|
| `Capability.Network` | `System.Net.*` namespaces | Relaxes TypeRef/ILString scan rules |
| `Capability.FileIO` | `System.IO` namespace | Relaxes TypeRef/ILString scan rules |
| `Capability.Process` | `Process*` types | Relaxes TypeRef/ILString scan rules |
| `Capability.AI` | **Nothing** | Enables `IAIService` injection by host |

`IAIService` lives in the `SiliconLife.Collective` namespace — it is never in any forbidden list. The capability declaration is an **opt-in signal** to the host that this plugin should receive the AI service reference.

## Capability Stacking: AI + Network

If your AI client needs direct network access (e.g., calling a remote AI endpoint), you must declare **both** capabilities:

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

See **17-CapabilityStacked** for full stacking examples.

## Controlled Entry Point Pattern

| Resource | Controlled Entry Point | Capability Needed |
|----------|----------------------|-------------------|
| Files | `PermissionedStreamFactory` | None |
| Network | `NetworkExecutor` | None |
| Process | `CommandLineExecutor` | None |
| Data store | `SpeedyPack` | None |
| AI Service | `IAIService` | `Capability.AI` |

`IAIService` is unique: it **requires** a capability declaration. This is because AI service access is an opt-in feature, not a default capability available to all plugins.

## Related Examples

- **17-CapabilityStacked**: Multiple capability stacking (Network + AI)
- **18-CapabilityDenied**: Undeclarable capability anti-pattern
