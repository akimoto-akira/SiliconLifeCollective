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
/// Knowledge triple data model (Subject-Predicate-Object)
/// Represents the basic information unit in the knowledge network
/// </summary>
public class KnowledgeTriple
{
    /// <summary>
    /// Subject (e.g., "Xiao Ming")
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Predicate/Relation (e.g., "likes")
    /// </summary>
    public string Predicate { get; set; } = string.Empty;

    /// <summary>
    /// Object (e.g., "apple")
    /// </summary>
    public string Object { get; set; } = string.Empty;

    /// <summary>
    /// Unique identifier for the triple
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Confidence level (0.0 - 1.0)
    /// </summary>
    public double Confidence { get; set; } = 1.0;

    /// <summary>
    /// Knowledge source
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Knowledge category/domain
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Tag list
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Additional properties
    /// </summary>
    public Dictionary<string, object> Properties { get; set; } = new();

    /// <summary>
    /// Validate whether the triple is valid
    /// </summary>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Subject) &&
               !string.IsNullOrWhiteSpace(Predicate) &&
               !string.IsNullOrWhiteSpace(Object);
    }

    /// <summary>
    /// Get string representation of the triple
    /// </summary>
    public override string ToString()
    {
        return $"({Subject}) -[{Predicate}]-> ({Object})";
    }

    /// <summary>
    /// Get hash code
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Subject, Predicate, Object);
    }

    /// <summary>
    /// Compare two triples for equality
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not KnowledgeTriple other)
            return false;

        return Subject == other.Subject &&
               Predicate == other.Predicate &&
               Object == other.Object;
    }
}
