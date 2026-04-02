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
/// Unified request model for executor operations.
/// Executors provide a security boundary for AI-initiated IO operations.
/// </summary>
public class ExecutorRequest
{
    /// <summary>
    /// Gets the GUID of the silicon being that initiated this request
    /// </summary>
    public Guid CallerId { get; }

    /// <summary>
    /// Gets the resource path (URL, file path, command, etc.)
    /// </summary>
    public string ResourcePath { get; }

    /// <summary>
    /// Gets the type of operation (e.g., "http_get", "read_file", "run_command")
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the parameters for the operation
    /// </summary>
    public Dictionary<string, object> Parameters { get; }

    public ExecutorRequest(Guid callerId, string resourcePath, string type, Dictionary<string, object>? parameters = null)
    {
        CallerId = callerId;
        ResourcePath = resourcePath;
        Type = type;
        Parameters = parameters ?? new();
    }
}
