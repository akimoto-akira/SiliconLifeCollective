// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SiliconLife.Collective;

/// <summary>
/// Result of an executor operation.
/// Supports both text and binary output.
/// </summary>
public class ExecutorResult
{
    /// <summary>
    /// Gets whether the operation was successful
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Gets the text output content (stdout, response body, file content, etc.)
    /// Null when the result is binary-only or the operation failed.
    /// </summary>
    public string? Output { get; }

    /// <summary>
    /// Gets the binary output content (e.g., image data, file bytes).
    /// Null when the result is text-only or the operation failed.
    /// When both <see cref="Output"/> and <see cref="BinaryOutput"/> are set,
    /// <see cref="Output"/> contains the Base64 encoding of <see cref="BinaryOutput"/>.
    /// </summary>
    public byte[]? BinaryOutput { get; }

    /// <summary>
    /// Gets the error message if the operation failed
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Gets the exit code (for command-line operations) or HTTP status code
    /// </summary>
    public int ExitCode { get; }

    /// <summary>
    /// Gets the MIME content type of the response (e.g., "image/png", "application/xml").
    /// Null when not applicable.
    /// </summary>
    public string? ContentType { get; }

    private ExecutorResult(bool success, string? output, byte[]? binaryOutput, string? error, int exitCode, string? contentType = null)
    {
        Success = success;
        Output = output;
        BinaryOutput = binaryOutput;
        Error = error;
        ExitCode = exitCode;
        ContentType = contentType;
    }

    /// <summary>
    /// Creates a successful result with text output
    /// </summary>
    public static ExecutorResult Successful(string? output = null, int exitCode = 0)
    {
        return new ExecutorResult(true, output, null, null, exitCode);
    }

    /// <summary>
    /// Creates a successful result with binary output.
    /// The <see cref="Output"/> property will contain the Base64 encoding of the binary data.
    /// </summary>
    public static ExecutorResult SuccessfulBinary(byte[] binaryOutput, string? contentType = null, int exitCode = 0)
    {
        string base64 = Convert.ToBase64String(binaryOutput);
        return new ExecutorResult(true, base64, binaryOutput, null, exitCode, contentType);
    }

    /// <summary>
    /// Creates a failed result
    /// </summary>
    public static ExecutorResult Failed(string error, int exitCode = -1)
    {
        return new ExecutorResult(false, null, null, error, exitCode);
    }
}
