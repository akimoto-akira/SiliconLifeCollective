// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Text.RegularExpressions;

namespace SiliconLife.Collective;

/// <summary>
/// Result of static security analysis on source code.
/// </summary>
public sealed class SecurityScanResult
{
    /// <summary>Whether the code passed all security checks</summary>
    public bool Passed { get; }

    /// <summary>List of detected violations (empty if passed)</summary>
    public List<string> Violations { get; }

    /// <summary>Severity of the worst violation found</summary>
    public string WorstSeverity { get; }

    public SecurityScanResult(bool passed, List<string> violations, string worstSeverity = "none")
    {
        Passed = passed;
        Violations = violations;
        WorstSeverity = worstSeverity;
    }
}

/// <summary>
/// Static security scanner for dynamically compiled code.
/// Detects dangerous patterns: direct IO access, system calls, reflection abuse, etc.
/// </summary>
public static class SecurityScanner
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(SecurityScanner));

    /// <summary>
    /// Dangerous patterns that are absolutely forbidden.
    /// Each entry: (pattern, description, severity)
    /// </summary>
    private static readonly (Regex Pattern, string Description, string Severity)[] BannedPatterns =
    [
        // Direct file system access bypassing executors
        (new Regex(@"System\.IO\.File\.\w+", RegexOptions.Compiled), "Direct file IO access (File.*). Use tools/executors instead.", "critical"),
        (new Regex(@"System\.IO\.Directory\.\w+", RegexOptions.Compiled), "Direct directory IO access (Directory.*). Use tools/executors instead.", "critical"),

        // Process invocation bypassing executors
        (new Regex(@"System\.Diagnostics\.Process\.\w+", RegexOptions.Compiled), "Direct process invocation (Process.*). Use CommandLineExecutor instead.", "critical"),

        // Network access bypassing executors
        (new Regex(@"System\.Net\.HttpWebRequest", RegexOptions.Compiled), "Direct HTTP request. Use NetworkExecutor instead.", "critical"),
        (new Regex(@"System\.Net\.WebClient", RegexOptions.Compiled), "Direct WebClient usage. Use NetworkExecutor instead.", "critical"),

        // Reflection abuse — loading assemblies, invoking arbitrary methods
        (new Regex(@"Assembly\.LoadFrom", RegexOptions.Compiled), "Dynamic assembly loading from file path.", "high"),
        (new Regex(@"Assembly\.LoadFile", RegexOptions.Compiled), "Loading assembly from file.", "high"),
        (new Regex(@"Activator\.CreateInstance", RegexOptions.Compiled), "Uncontrolled instance creation via reflection.", "medium"),

        // Dangerous runtime operations
        (new Regex(@"Environment\.Exit", RegexOptions.Compiled), "Forced process termination.", "critical"),
        (new Regex(@"AppDomain", RegexOptions.Compiled), "AppDomain manipulation.", "high"),
        (new Regex(@"Thread\.\w*Abort", RegexOptions.Compiled), "Thread abort (deprecated/dangerous).", "high"),

        // P/Invoke — calling native code
        (new Regex(@"DllImport", RegexOptions.Compiled), "P/Invoke native code interop.", "high"),

        // Unsafe code blocks
        (new Regex(@"\bunsafe\b", RegexOptions.Compiled), "Unsafe code block.", "medium"),

        // Accessing private/internal members of the host via reflection
        (new Regex(@"BindingFlags\.(NonPublic|Instance)", RegexOptions.Compiled), "Accessing non-public members via reflection.", "high"),
    ];

    /// <summary>
    /// Allowed assembly names that dynamically compiled code can reference.
    /// References to any assembly not in this list will be rejected.
    /// </summary>
    public static readonly HashSet<string> AllowedAssemblyNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "System.Runtime",
        "System.Private.CoreLib",
        "System.Console",
        "System.Linq",
        "System.Collections",
        "System.Collections.Concurrent",
        "System.Collections.Generic",
        "System.Threading",
        "System.Threading.Tasks",
        "System.Text.Json",
        "System.Text.RegularExpressions",
        "System.Net.Primitives",
        "System.IO.FileSystem.Primitives",
        "System.Memory",
        "System.ObjectModel",
        "System.Globalization",
        "System.Diagnostics.Debug",
        "netstandard",
        "SiliconLife.Core",
    };

    /// <summary>
    /// Scans source code for dangerous patterns.
    /// </summary>
    /// <param name="sourceCode">The source code to scan</param>
    /// <returns>Security scan result with all violations found</returns>
    public static SecurityScanResult Scan(string sourceCode)
    {
        ArgumentNullException.ThrowIfNull(sourceCode);

        _logger.Debug("Security scanning source code, length={Length}", sourceCode.Length);

        var violations = new List<string>();
        string worstSeverity = "none";

        foreach (var (pattern, description, severity) in BannedPatterns)
        {
            if (pattern.IsMatch(sourceCode))
            {
                violations.Add($"[{severity.ToUpperInvariant()}] {description} (pattern: {pattern})");
                _logger.Warn("Security violation: [{Severity}] {Description}", severity.ToUpperInvariant(), description);

                if (GetSeverityLevel(severity) > GetSeverityLevel(worstSeverity))
                {
                    worstSeverity = severity;
                }
            }
        }

        if (violations.Count == 0)
        {
            _logger.Debug("Security scan passed");
        }

        return new SecurityScanResult(violations.Count == 0, violations, worstSeverity);
    }

    /// <summary>
    /// Validates that all referenced assemblies are in the allowed list.
    /// </summary>
    /// <param name="referencedAssemblies">Assembly names referenced by the code</param>
    /// <returns>Tuple of (isValid, unauthorizedAssemblies)</returns>
    public static (bool IsValid, List<string> Unauthorized) ValidateReferences(IEnumerable<string> referencedAssemblies)
    {
        var unauthorized = new List<string>();

        foreach (string assembly in referencedAssemblies)
        {
            string cleanName = assembly.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
                ? assembly[..^4]
                : assembly;

            if (!AllowedAssemblyNames.Contains(cleanName))
            {
                unauthorized.Add(cleanName);
                _logger.Warn("Unauthorized assembly reference: {AssemblyName}", cleanName);
            }
        }

        return (unauthorized.Count == 0, unauthorized);
    }

    private static int GetSeverityLevel(string severity) => severity switch
    {
        "critical" => 3,
        "high" => 2,
        "medium" => 1,
        _ => 0
    };
}
