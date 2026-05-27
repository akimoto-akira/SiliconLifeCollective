# PluginDemo-15: Capability.Process — Declarative Process Permission

## Overview

This plugin demonstrates how to use `[PluginCapability(Capability.Process)]` to declare that a plugin requires the ability to launch child processes. By declaring this capability, the plugin gains access to `System.Diagnostics.Process` and related types.

## Declaration Syntax

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Capability.Process Exemption Scope

### TypeRef Exemptions

Only Process-related types under `System.Diagnostics` are exempted:

| Exempted Type | Usage |
|--------------|-------|
| `Process` | Start, manage, and monitor child processes |
| `ProcessStartInfo` | Configure process startup parameters |
| `ProcessThread` | Access process thread information |
| `ProcessModule` | Access process module information |
| `ProcessPriorityClass` | Set process priority |
| `ProcessWindowStyle` | Configure process window style |

Types always allowed (never in forbidden list): `Stopwatch`, `Debug`, `Trace`, `Activity`

### ILString Exemptions

- Strings starting with `"System.Diagnostics.Process"` are not flagged

## Comparison with 09-ForbiddenProcess

| Aspect | 09-ForbiddenProcess | 15-CapabilityProcess |
|--------|-------------------|---------------------|
| Declaration | None | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ REJECTED | ✅ ALLOWED |
| ProcessStartInfo | ❌ REJECTED | ✅ ALLOWED |

## Recommended: CommandLineExecutor

Even with `Capability.Process`, `CommandLineExecutor` is recommended when possible:

| Feature | CommandLineExecutor | Direct Process |
|---------|-------------------|---------------|
| Capability needed | No | Yes |
| Sandboxing | Command allowlist | None |
| Timeouts | Built-in | Manual |
| Output capture | Structured | Manual |
| Audit logging | Automatic | Manual |

Use `Capability.Process` + direct `Process` when you need fine-grained control over I/O streams, process events, or when CommandLineExecutor's allowlist is too restrictive.

## Security Best Practices

1. **Prefer CommandLineExecutor**: Use controlled entry point when possible
2. **Provide clear Reason**: "Launch build tools for CI pipeline" vs vague "process access"
3. **Validate all input**: Never pass untrusted input directly to ProcessStartInfo
4. **Use WaitForExit**: Always wait for process completion to prevent zombie processes
5. **Redirect streams**: Set `RedirectStandardOutput = true` and `UseShellExecute = false`

## Related Examples

- **09-ForbiddenProcess**: Anti-pattern showing blocked process operations
- **18-CapabilityDenied**: Undeclarable capability anti-pattern
