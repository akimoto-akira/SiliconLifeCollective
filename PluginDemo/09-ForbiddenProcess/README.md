# PluginDemo-09: Forbidden Process Anti-Pattern

## Overview

This plugin demonstrates **FORBIDDEN** process execution operations in the SiliconLife plugin system. It serves as an anti-pattern reference, showing what NOT to do and providing correct alternatives for each violation.

## Why are Process Types Forbidden?

`System.Diagnostics.Process` and `ProcessStartInfo` are blocked from plugins because direct process execution poses severe security risks:

1. **Arbitrary Command Execution**: Plugins could run any command without audit or permission checks
2. **Malware Launch**: Malicious plugins could execute unwanted applications or scripts
3. **System Resource Access**: Processes could access sensitive system resources outside the plugin sandbox
4. **No Command Validation**: Direct Process.Start has no built-in protection against command injection
5. **No Audit Trail**: Direct process operations bypass the plugin security audit system
6. **Privilege Escalation**: Could spawn processes with higher privileges than the plugin should have

## What Types are Forbidden?

Only Process-related types are forbidden, **NOT the entire System.Diagnostics namespace**:

| Forbidden Type | Blocked Method | Risk Level |
|----------------|----------------|------------|
| `Process` | `Start()`, `Kill()`, `WaitForExit()` | 🔴 Critical |
| `ProcessStartInfo` | Constructor, all properties | 🔴 Critical |
| `Process` | `StandardInput`, `StandardOutput`, `StandardError` | 🔴 Critical |
| `Process` | `GetProcesses()`, `GetProcessesByName()` | 🟡 High |

## What Types are Allowed?

Other `System.Diagnostics` types that don't involve process execution remain available:

| Allowed Type | Usage | Why Safe |
|--------------|-------|----------|
| `Stopwatch` | Timing measurements | No process execution |
| `Debug` | Debug output | No security risk |
| `Trace` | Tracing/logging | No security risk |
| `PerformanceCounter` | Performance monitoring | Read-only, audited |

## How to Execute Commands Safely?

### Use CommandLineExecutor (The Only Safe Way)

`CommandLineExecutor` is the **controlled entry point** for command execution in plugins:

```csharp
// ✅ CORRECT: Execute a command
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"Error: {result.Error}");
}
```

**What CommandLineExecutor Provides:**
1. **Command Injection Protection**: Blocks dangerous separators (`||`, `&&`, `|`, `&`, `;`)
2. **Timeout Enforcement**: Default 30-second timeout (configurable)
3. **Audit Logging**: All command execution is recorded for security review
4. **Output Capture**: Automatically captures stdout and stderr
5. **Cross-Platform Support**: Uses `cmd.exe` on Windows, `/bin/bash` on Unix
6. **Error Handling**: Returns structured result with success/failure status

## Violations Demonstrated

This plugin shows 5 common process execution violations:

### Violation 1: Process.Start

```csharp
// ❌ FORBIDDEN
Process.Start("notepad.exe");

// ✅ CORRECT
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**Blocked TypeRef**: `System.Diagnostics.Process::Start(System.String)`

### Violation 2: ProcessStartInfo

```csharp
// ❌ FORBIDDEN
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ CORRECT
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**Blocked TypeRef**: `System.Diagnostics.ProcessStartInfo::.ctor()`

### Violation 3: Process with Arguments

```csharp
// ❌ FORBIDDEN
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ CORRECT
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**Blocked TypeRef**: `System.Diagnostics.Process::Start(ProcessStartInfo)`

### Violation 4: Process Output Redirection

```csharp
// ❌ FORBIDDEN
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ CORRECT
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**Blocked TypeRef**: `System.Diagnostics.Process::StandardOutput`

### Violation 5: Process.Kill

```csharp
// ❌ FORBIDDEN
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ CORRECT
// Process killing is not supported through CommandLineExecutor
// for security reasons. Contact system administrator if needed.
```

**Blocked TypeRef**: `System.Diagnostics.Process::Kill()`

## Why Only Process is Blocked, Not Entire System.Diagnostics?

The plugin system takes a **surgical approach** to security:

- **Block only dangerous types**: Process/ProcessStartInfo enable arbitrary code execution
- **Allow safe types**: Stopwatch, Debug, Trace have no security implications
- **Minimize impact**: Developers can still use diagnostic tools that don't pose risks
- **Clear boundary**: Only types that can spawn/kill processes are forbidden

This is different from `System.IO` which is **entirely blocked** because most IO types directly access the file system.

## Comparison with Other Examples

| Example | Focus | Permission Required |
|---------|-------|---------------------|
| **09-ForbiddenProcess** | Forbidden process patterns (this example) | N/A (blocked) |
| **15-CapabilityProcess** | Declaring Process capability to bypass restrictions | `Capability.Process` |

**Key Difference:**
- **09-ForbiddenProcess**: Shows what you CANNOT do (direct process execution)
- **15-CapabilityProcess**: Shows how to DECLARATIVELY request process execution permission

## PluginLoader Security Mechanism

When PluginLoader scans this plugin:

1. **TypeRef Scanning**: Detects references to forbidden `Process`/`ProcessStartInfo` types
2. **MemberRef Scanning**: Detects calls to blocked methods (e.g., `Process.Start`)
3. **IL String Scanning**: Detects string-based reflection attempts to load forbidden types
4. **Rejection**: Plugin is rejected during loading with detailed error message

**Cannot be bypassed by:**
- String concatenation (`"System.Diagnostics" + ".Process"`)
- Reflection (`Type.GetType("System.Diagnostics.Process")`)
- Dynamic loading (`Assembly.Load`)
- Obfuscation or encryption

These bypass attempts are caught by IL-level scanning (see **12-ForbiddenStringBypass**).

## Security Notes

1. **Multi-Command Execution Blocked**: Commands like `"cmd1 && cmd2"` are rejected
2. **Dangerous Separators Detected**: `||`, `&&`, `|`, `&`, `;` are all blocked
3. **All Commands Logged**: Full audit trail for security review
4. **Permission Level**: Commands run with the plugin's permission level
5. **No Process Killing**: CommandLineExecutor doesn't support killing processes

## Best Practices

1. **Always Use CommandLineExecutor**: Never use `Process.Start` directly
2. **Set Reasonable Timeouts**: Prevent commands from hanging indefinitely
3. **Check Results**: Always verify `result.Success` before using output
4. **Sanitize Input**: Never pass user input directly to commands
5. **Declare Capability if Necessary**: If you truly need unrestricted process execution, declare `Capability.Process` (see 15-CapabilityProcess)

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

- **08-ForbiddenNetwork**: Forbidden network operations
- **15-CapabilityProcess**: Declarative Process capability
- **10-ForbiddenReflection**: Forbidden reflection operations
- **12-ForbiddenStringBypass**: String-based reflection bypass attempts
