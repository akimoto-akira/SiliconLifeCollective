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

internal sealed class AuditEntryData
{
    public Guid CallerId { get; set; }
    public PermissionType PermissionType { get; set; }
    public string Resource { get; set; } = string.Empty;
    public PermissionResult Result { get; set; }
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// Audit logger for permission decisions.
/// Persists all permission checks to ITimeStorage for time-indexed querying.
/// </summary>
public class AuditLogger
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<AuditLogger>();
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
                var data = new AuditEntryData
                {
                    CallerId = callerId,
                    PermissionType = permissionType,
                    Resource = resource,
                    Result = result,
                    Reason = reason
                };

                _timeStorage.Write(AuditKey, entry.Timestamp, data);
                _logger.Debug(null, "Audit logged: {0} | {1} | {2} | {3}", result, permissionType, resource, reason);
            }
            catch (Exception ex)
            {
                _logger.Warn(null, "Failed to persist audit entry: {0}", ex.Message);
            }
        }

        string resultStr = result == PermissionResult.Allowed ? "ALLOW" : result == PermissionResult.Denied ? "DENY" : "ASK";
        Console.WriteLine($"[AUDIT] {resultStr} | {permissionType} | {resource} | {reason}");
    }

    /// <summary>
    /// Queries audit entries within a time range
    /// </summary>
    /// <param name="range">The time range to query (null means all entries without filtering).</param>
    /// <returns>List of matching audit entries</returns>
    public List<AuditEntry> Query(IncompleteDate? range)
    {
        if (_timeStorage == null)
        {
            return new List<AuditEntry>();
        }

        try
        {
            List<TimeEntry<AuditEntryData>> entries = _timeStorage.Query<AuditEntryData>(AuditKey, range);
            var results = new List<AuditEntry>();

            foreach (TimeEntry<AuditEntryData> entry in entries)
            {
                if (entry.Data == null) continue;

                results.Add(new AuditEntry(
                    entry.Timestamp,
                    entry.Data.CallerId,
                    entry.Data.PermissionType,
                    entry.Data.Resource,
                    entry.Data.Result,
                    entry.Data.Reason
                ));
            }

            return results;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to query audit entries: {0}", ex.Message);
            return new List<AuditEntry>();
        }
    }
}
