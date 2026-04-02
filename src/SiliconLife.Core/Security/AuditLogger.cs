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
/// Audit log entry for a permission decision.
/// </summary>
public sealed class AuditEntry
{
    /// <summary>The timestamp of the decision</summary>
    public DateTime Timestamp { get; }

    /// <summary>The GUID of the silicon being that made the request</summary>
    public Guid CallerId { get; }

    /// <summary>The permission type</summary>
    public PermissionType PermissionType { get; }

    /// <summary>The resource path</summary>
    public string Resource { get; }

    /// <summary>The decision result</summary>
    public PermissionResult Result { get; }

    /// <summary>The reason for the decision</summary>
    public string Reason { get; }

    public AuditEntry(DateTime timestamp, Guid callerId, PermissionType permissionType, string resource, PermissionResult result, string reason)
    {
        Timestamp = timestamp;
        CallerId = callerId;
        PermissionType = permissionType;
        Resource = resource;
        Result = result;
        Reason = reason;
    }
}

/// <summary>
/// Audit logger for permission decisions.
/// Persists all permission checks to ITimeStorage for time-indexed querying.
/// </summary>
public class AuditLogger
{
    private readonly ITimeStorage? _timeStorage;
    private const string AuditKey = "audit";

    /// <summary>
    /// Creates an AuditLogger without persistence (in-memory only, for testing)
    /// </summary>
    public AuditLogger()
    {
    }

    /// <summary>
    /// Creates an AuditLogger with ITimeStorage persistence
    /// </summary>
    /// <param name="timeStorage">The time-indexed storage for persistence</param>
    public AuditLogger(ITimeStorage timeStorage)
    {
        _timeStorage = timeStorage;
    }

    /// <summary>
    /// Logs a permission decision. Persists to ITimeStorage if available.
    /// </summary>
    /// <param name="callerId">The GUID of the requesting silicon being</param>
    /// <param name="permissionType">The permission type</param>
    /// <param name="resource">The resource path</param>
    /// <param name="result">The decision result</param>
    /// <param name="reason">The reason for the decision</param>
    public void Log(Guid callerId, PermissionType permissionType, string resource, PermissionResult result, string reason)
    {
        var entry = new AuditEntry(DateTime.UtcNow, callerId, permissionType, resource, result, reason);

        if (_timeStorage != null)
        {
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, string>
                {
                    ["caller"] = callerId.ToString(),
                    ["type"] = permissionType.ToString(),
                    ["resource"] = resource,
                    ["result"] = result.ToString(),
                    ["reason"] = reason
                });

                _timeStorage.Write(AuditKey, entry.Timestamp, System.Text.Encoding.UTF8.GetBytes(json));
            }
            catch
            {
                // Audit log failure should not block the operation
            }
        }

        // Always log to console for visibility
        string resultStr = result == PermissionResult.Allowed ? "ALLOW" : result == PermissionResult.Denied ? "DENY" : "ASK";
        Console.WriteLine($"[AUDIT] {resultStr} | {permissionType} | {resource} | {reason}");
    }

    /// <summary>
    /// Queries audit entries within a time range
    /// </summary>
    /// <param name="range">The time range to query</param>
    /// <returns>List of matching audit entries</returns>
    public List<AuditEntry> Query(IncompleteDate range)
    {
        if (_timeStorage == null)
        {
            return new List<AuditEntry>();
        }

        try
        {
            List<TimeEntry> entries = _timeStorage.Query(AuditKey, range);
            var results = new List<AuditEntry>();

            foreach (TimeEntry entry in entries)
            {
                try
                {
                    string json = System.Text.Encoding.UTF8.GetString(entry.Data);
                    var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    if (data == null) continue;

                    results.Add(new AuditEntry(
                        entry.Timestamp,
                        Guid.Parse(data.GetValueOrDefault("caller", Guid.Empty.ToString())),
                        Enum.Parse<PermissionType>(data.GetValueOrDefault("type", "FileAccess")),
                        data.GetValueOrDefault("resource", ""),
                        Enum.Parse<PermissionResult>(data.GetValueOrDefault("result", "Denied")),
                        data.GetValueOrDefault("reason", "")
                    ));
                }
                catch
                {
                    // Skip malformed entries
                }
            }

            return results;
        }
        catch
        {
            return new List<AuditEntry>();
        }
    }
}
