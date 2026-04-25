// Copyright 2026 Silicon Life Collective
//
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
/// Knowledge entry - complete knowledge unit containing triple and metadata
/// </summary>
public class KnowledgeEntry
{
    /// <summary>
    /// Unique identifier for the knowledge entry
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Core triple
    /// </summary>
    public KnowledgeTriple Triple { get; set; } = new();

    /// <summary>
    /// Version number (for version management)
    /// </summary>
    public int Version { get; set; } = 1;

    /// <summary>
    /// Creator identifier
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last modifier identifier
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;

    /// <summary>
    /// Creation time
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last modification time
    /// </summary>
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Validation status (Unverified/Verified/Rejected)
    /// </summary>
    public KnowledgeValidationStatus ValidationStatus { get; set; } = KnowledgeValidationStatus.Unverified;

    /// <summary>
    /// Validator list
    /// </summary>
    public List<string> ValidatedBy { get; set; } = new();

    /// <summary>
    /// Access permission level
    /// </summary>
    public KnowledgeAccessLevel AccessLevel { get; set; } = KnowledgeAccessLevel.Public;

    /// <summary>
    /// Change history log
    /// </summary>
    public List<KnowledgeChangeLog> ChangeHistory { get; set; } = new();

    /// <summary>
    /// Related entry references
    /// </summary>
    public List<string> RelatedEntryIds { get; set; } = new();

    /// <summary>
    /// Add change log entry
    /// </summary>
    public void AddChangeLog(string modifier, string changeDescription)
    {
        ChangeHistory.Add(new KnowledgeChangeLog
        {
            ModifiedBy = modifier,
            ChangeDescription = changeDescription,
            ModifiedAt = DateTime.UtcNow,
            Version = ++Version
        });
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = modifier;
    }
}

/// <summary>
/// Knowledge validation status
/// </summary>
public enum KnowledgeValidationStatus
{
    /// <summary>
    /// Unverified
    /// </summary>
    Unverified,

    /// <summary>
    /// Verified
    /// </summary>
    Verified,

    /// <summary>
    /// Rejected
    /// </summary>
    Rejected
}

/// <summary>
/// Knowledge access level
/// </summary>
public enum KnowledgeAccessLevel
{
    /// <summary>
    /// Public - accessible to all silicon beings
    /// </summary>
    Public,

    /// <summary>
    /// Restricted - requires specific permissions
    /// </summary>
    Restricted,

    /// <summary>
    /// Private - accessible only to creator
    /// </summary>
    Private
}

/// <summary>
/// Knowledge change log
/// </summary>
public class KnowledgeChangeLog
{
    /// <summary>
    /// Modifier
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;

    /// <summary>
    /// Modification time
    /// </summary>
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Change description
    /// </summary>
    public string ChangeDescription { get; set; } = string.Empty;

    /// <summary>
    /// Version number
    /// </summary>
    public int Version { get; set; }
}
