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

namespace SiliconLife.Collective;

/// <summary>
/// Result of an executor operation
/// </summary>
public class ExecutorResult
{
    /// <summary>
    /// Gets whether the operation was successful
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Gets the output content (stdout, response body, file content, etc.)
    /// </summary>
    public string? Output { get; }

    /// <summary>
    /// Gets the error message if the operation failed
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Gets the exit code (for command-line operations)
    /// </summary>
    public int ExitCode { get; }

    private ExecutorResult(bool success, string? output, string? error, int exitCode)
    {
        Success = success;
        Output = output;
        Error = error;
        ExitCode = exitCode;
    }

    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static ExecutorResult Successful(string? output = null, int exitCode = 0)
    {
        return new ExecutorResult(true, output, null, exitCode);
    }

    /// <summary>
    /// Creates a failed result
    /// </summary>
    public static ExecutorResult Failed(string error, int exitCode = -1)
    {
        return new ExecutorResult(false, null, error, exitCode);
    }
}
