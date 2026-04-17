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

using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// System information tool.
/// Queries processes and environment variables through CommandLineExecutor.
/// Verifies the executor pipeline.
/// </summary>
public class SystemTool : ITool
{
    public string Name => "system";

    public string Description =>
        "Query the host computer's operating system information (not the Silicon Life software system). " +
        "Actions: 'list_processes' (running processes), " +
        "'find_process' (search process by name, supports '?' single-char and '*' multi-char wildcards), " +
        "'get_env' (specific environment variable), 'get_env_all' (all environment variables), " +
        "'system_info' (OS and runtime information), " +
        "'resource_usage' (CPU and memory usage of the host).";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The action to perform: list_processes, find_process, get_env, get_env_all, system_info, resource_usage",
                    ["enum"] = new[] { "list_processes", "find_process", "get_env", "get_env_all", "system_info", "resource_usage" }
                },
                ["variable"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Environment variable name (for get_env action)"
                },
                ["pattern"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Process name pattern to search (for find_process action). Supports '?' (single char) and '*' (multiple chars) wildcards."
                },
                ["limit"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum number of processes to return for list_processes. Omit or set to -1 to return all processes."
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj?.ToString() ?? "";

        return action.ToLowerInvariant() switch
        {
            "list_processes" => ExecuteListProcesses(callerId, parameters),
            "find_process" => ExecuteFindProcess(callerId, parameters),
            "get_env" => ExecuteGetEnv(parameters),
            "get_env_all" => ExecuteGetEnvAll(),
            "system_info" => ExecuteSystemInfo(),
            "resource_usage" => ExecuteResourceUsage(),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteListProcesses(Guid callerId, Dictionary<string, object> parameters)
    {
        // Parse limit: -1 or absent means return all
        int limit = 30;
        if (parameters.TryGetValue("limit", out object? limitObj))
        {
            if (limitObj is long l) limit = (int)l;
            else if (int.TryParse(limitObj?.ToString(), out int parsed)) limit = parsed;
        }

        string command = OperatingSystem.IsWindows()
            ? "tasklist /FO CSV /NH"
            : "ps aux --no-headers";

        ExecutorRequest request = new(callerId, command, "list_processes");
        ExecutorResult result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));

        if (!result.Success)
            return ToolResult.Failed(result.Error ?? "Failed to list processes");

        string[] lines = (result.Output ?? "").Split('\n', StringSplitOptions.RemoveEmptyEntries);

        if (limit <= 0)
        {
            // Return all processes
            return ToolResult.Successful($"Total: {lines.Length} process(es)\n{string.Join("\n", lines)}");
        }

        string limitedOutput = lines.Length > limit
            ? string.Join("\n", lines.Take(limit)) + $"\n... ({lines.Length - limit} more processes, set limit=-1 to get all)"
            : string.Join("\n", lines);

        return ToolResult.Successful(limitedOutput);
    }

    private ToolResult ExecuteFindProcess(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("pattern", out object? patternObj) || string.IsNullOrWhiteSpace(patternObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'pattern' parameter for find_process");
        }

        string pattern = patternObj.ToString()!;

        string command = OperatingSystem.IsWindows()
            ? "tasklist /FO CSV /NH"
            : "ps aux --no-headers";

        ExecutorRequest request = new(callerId, command, "find_process");
        ExecutorResult result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));

        if (!result.Success)
            return ToolResult.Failed(result.Error ?? "Failed to list processes");

        string[] lines = (result.Output ?? "").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var matched = lines.Where(line =>
        {
            // Extract process name: first CSV field on Windows, 11th space-delimited field on Unix
            string procName = OperatingSystem.IsWindows()
                ? line.Split(',')[0].Trim('"')
                : line.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries).ElementAtOrDefault(10) ?? "";
            return WildcardMatch(procName, pattern, ignoreCase: true);
        }).ToList();

        if (matched.Count == 0)
            return ToolResult.Successful($"No processes found matching '{pattern}'");

        string output = string.Join("\n", matched);
        return ToolResult.Successful($"Found {matched.Count} process(es) matching '{pattern}':\n{output}");
    }

    private static bool WildcardMatch(string input, string pattern, bool ignoreCase = false)
    {
        StringComparison comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        // Convert wildcard pattern to a simple state-machine match
        int i = 0, p = 0, starIdx = -1, matchIdx = 0;
        while (i < input.Length)
        {
            if (p < pattern.Length && (pattern[p] == '?' || string.Compare(input, i, pattern, p, 1, comparison) == 0))
            {
                i++; p++;
            }
            else if (p < pattern.Length && pattern[p] == '*')
            {
                starIdx = p++;
                matchIdx = i;
            }
            else if (starIdx != -1)
            {
                p = starIdx + 1;
                i = ++matchIdx;
            }
            else
            {
                return false;
            }
        }

        while (p < pattern.Length && pattern[p] == '*') p++;
        return p == pattern.Length;
    }

    private ToolResult ExecuteGetEnv(Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("variable", out object? varObj) || string.IsNullOrWhiteSpace(varObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'variable' parameter for get_env");
        }

        string varName = varObj.ToString()!;
        string? value = Environment.GetEnvironmentVariable(varName);

        if (value != null)
        {
            return ToolResult.Successful($"{varName}={value}");
        }

        return ToolResult.Failed($"Environment variable '{varName}' not found");
    }

    private ToolResult ExecuteGetEnvAll()
    {
        var vars = Environment.GetEnvironmentVariables();
        var sorted = new List<string>();
        foreach (System.Collections.DictionaryEntry entry in vars)
        {
            sorted.Add($"{entry.Key}={entry.Value}");
        }
        sorted.Sort(StringComparer.OrdinalIgnoreCase);

        // Limit output
        string output = sorted.Count > 50
            ? string.Join("\n", sorted.Take(50)) + $"\n... ({sorted.Count - 50} more variables)"
            : string.Join("\n", sorted);

        return ToolResult.Successful(output);
    }

    private static ToolResult ExecuteSystemInfo()
    {
        string info = $"OS: {Environment.OSVersion.Platform} {Environment.OSVersion.VersionString}\n" +
            $"Machine: {Environment.MachineName}\n" +
            $".NET: {Environment.Version}\n" +
            $"Processor Count: {Environment.ProcessorCount}\n" +
            $"Working Directory: {Environment.CurrentDirectory}\n" +
            $"64-bit OS: {Environment.Is64BitOperatingSystem}\n" +
            $"64-bit Process: {Environment.Is64BitProcess}";

        return ToolResult.Successful(info);
    }

    private static ToolResult ExecuteResourceUsage()
    {
        return OperatingSystem.IsWindows()
            ? ExecuteResourceUsageWindows()
            : ExecuteResourceUsageUnix();
    }

    private static ToolResult ExecuteResourceUsageWindows()
    {
        // ── Memory via GlobalMemoryStatusEx ──────────────────────────────────
        var memStatus = new MEMORYSTATUSEX();
        memStatus.dwLength = (uint)System.Runtime.InteropServices.Marshal.SizeOf(memStatus);
        bool memOk = GlobalMemoryStatusEx(ref memStatus);

        string memInfo;
        if (memOk)
        {
            double totalGb  = memStatus.ullTotalPhys / 1024.0 / 1024.0 / 1024.0;
            double availGb  = memStatus.ullAvailPhys / 1024.0 / 1024.0 / 1024.0;
            double usedGb   = totalGb - availGb;
            memInfo =
                $"Memory Total:     {totalGb:F2} GB\n" +
                $"Memory Used:      {usedGb:F2} GB ({memStatus.dwMemoryLoad}%)\n" +
                $"Memory Available: {availGb:F2} GB";
        }
        else
        {
            memInfo = "Memory: unavailable";
        }

        // ── CPU via all-process TotalProcessorTime sampling ──────────────────
        TimeSpan cpuBefore = SampleAllProcessCpuTime();
        DateTime wallBefore = DateTime.UtcNow;
        System.Threading.Thread.Sleep(500);
        TimeSpan cpuAfter = SampleAllProcessCpuTime();
        DateTime wallAfter = DateTime.UtcNow;

        double wallMs = (wallAfter - wallBefore).TotalMilliseconds;
        double cpuMs  = (cpuAfter - cpuBefore).TotalMilliseconds;
        double cpuPct = wallMs > 0
            ? Math.Min(cpuMs / (wallMs * Environment.ProcessorCount) * 100.0, 100.0)
            : 0;

        string cpuInfo =
            $"CPU Usage (500 ms sample): {cpuPct:F1}%\n" +
            $"Logical Processors: {Environment.ProcessorCount}";

        return ToolResult.Successful($"{cpuInfo}\n\n{memInfo}");
    }

    private static ToolResult ExecuteResourceUsageUnix()
    {
        // ── Memory via /proc/meminfo ──────────────────────────────────────────
        string memInfo = "Memory: unavailable";
        try
        {
            string[] memLines = File.ReadAllLines("/proc/meminfo");
            long memTotalKb = ParseProcMemField(memLines, "MemTotal");
            long memAvailKb = ParseProcMemField(memLines, "MemAvailable");
            if (memTotalKb > 0)
            {
                double totalGb = memTotalKb / 1024.0 / 1024.0;
                double availGb = memAvailKb / 1024.0 / 1024.0;
                double usedGb  = totalGb - availGb;
                double usedPct = totalGb > 0 ? usedGb / totalGb * 100.0 : 0;
                memInfo =
                    $"Memory Total:     {totalGb:F2} GB\n" +
                    $"Memory Used:      {usedGb:F2} GB ({usedPct:F1}%)\n" +
                    $"Memory Available: {availGb:F2} GB";
            }
        }
        catch { /* /proc not available */ }

        // ── CPU via /proc/stat sampling ───────────────────────────────────────
        string cpuInfo = "CPU: unavailable";
        try
        {
            (long idle1, long total1) = ReadProcStatCpu();
            System.Threading.Thread.Sleep(500);
            (long idle2, long total2) = ReadProcStatCpu();

            long totalDiff = total2 - total1;
            long idleDiff  = idle2  - idle1;
            double cpuPct  = totalDiff > 0
                ? (1.0 - (double)idleDiff / totalDiff) * 100.0
                : 0;

            cpuInfo =
                $"CPU Usage (500 ms sample): {cpuPct:F1}%\n" +
                $"Logical Processors: {Environment.ProcessorCount}";
        }
        catch { /* /proc not available */ }

        return ToolResult.Successful($"{cpuInfo}\n\n{memInfo}");
    }

    private static TimeSpan SampleAllProcessCpuTime()
    {
        TimeSpan total = TimeSpan.Zero;
        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
        {
            try { total += p.TotalProcessorTime; }
            catch { /* access denied or process exited */ }
            finally { p.Dispose(); }
        }
        return total;
    }

    private static long ParseProcMemField(string[] lines, string field)
    {
        foreach (string line in lines)
        {
            if (line.StartsWith(field + ":", StringComparison.Ordinal))
            {
                string[] parts = line.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2 && long.TryParse(parts[1], out long val))
                    return val;
            }
        }
        return 0;
    }

    private static (long idle, long total) ReadProcStatCpu()
    {
        string firstLine = File.ReadLines("/proc/stat").First();
        // cpu  user nice system idle iowait irq softirq steal guest guest_nice
        string[] parts = firstLine.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries);
        long idle  = long.Parse(parts[4]);
        long total = parts.Skip(1).Sum(long.Parse);
        return (idle, total);
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    private struct MEMORYSTATUSEX
    {
        public uint  dwLength;
        public uint  dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
    private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);
}
