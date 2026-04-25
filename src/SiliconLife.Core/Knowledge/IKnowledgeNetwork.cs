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
/// Knowledge network interface - defined in Core layer
/// Provides complete knowledge network operation capabilities
/// </summary>
public interface IKnowledgeNetwork
{
    /// <summary>
    /// Initialize the knowledge network
    /// </summary>
    /// <param name="storagePath">Storage path</param>
    void Initialize(string storagePath);

    /// <summary>
    /// Add a knowledge triple
    /// </summary>
    /// <param name="triple">Knowledge triple</param>
    /// <param name="createdBy">Creator</param>
    /// <returns>Knowledge entry</returns>
    KnowledgeEntry AddKnowledge(KnowledgeTriple triple, string createdBy);

    /// <summary>
    /// Update a knowledge entry
    /// </summary>
    /// <param name="entryId">Entry ID</param>
    /// <param name="updatedTriple">Updated triple</param>
    /// <param name="modifiedBy">Modifier</param>
    /// <param name="changeDescription">Change description</param>
    /// <returns>Whether the update was successful</returns>
    bool UpdateKnowledge(string entryId, KnowledgeTriple updatedTriple, string modifiedBy, string changeDescription);

    /// <summary>
    /// Delete a knowledge entry
    /// </summary>
    /// <param name="entryId">Entry ID</param>
    /// <param name="deletedBy">Deleter</param>
    /// <returns>Whether the deletion was successful</returns>
    bool DeleteKnowledge(string entryId, string deletedBy);

    /// <summary>
    /// Query knowledge entry by ID
    /// </summary>
    /// <param name="entryId">Entry ID</param>
    /// <returns>Knowledge entry, or null if not found</returns>
    KnowledgeEntry? GetKnowledgeById(string entryId);

    /// <summary>
    /// Exact match query for triples
    /// </summary>
    /// <param name="subject">Subject</param>
    /// <param name="predicate">Predicate</param>
    /// <param name="obj">Object</param>
    /// <returns>List of matching knowledge entries</returns>
    List<KnowledgeEntry> QueryKnowledge(string? subject = null, string? predicate = null, string? obj = null);

    /// <summary>
    /// Fuzzy search for knowledge
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <param name="maxResults">Maximum number of results</param>
    /// <returns>List of matching knowledge entries</returns>
    List<KnowledgeEntry> SearchKnowledge(string searchTerm, int maxResults = 50);

    /// <summary>
    /// Get relationship path between two entities
    /// </summary>
    /// <param name="startEntity">Start entity</param>
    /// <param name="endEntity">Target entity</param>
    /// <param name="maxDepth">Maximum search depth</param>
    /// <returns>List of relationship paths</returns>
    List<RelationshipPath> GetRelationshipPath(string startEntity, string endEntity, int maxDepth = 5);

    /// <summary>
    /// Validate a knowledge entry
    /// </summary>
    /// <param name="entryId">Entry ID</param>
    /// <param name="validator">Validator</param>
    /// <returns>Whether the validation was successful</returns>
    bool ValidateKnowledge(string entryId, string validator);

    /// <summary>
    /// Reject a knowledge entry
    /// </summary>
    /// <param name="entryId">Entry ID</param>
    /// <param name="rejector">Rejector</param>
    /// <returns>Whether the rejection was successful</returns>
    bool RejectKnowledge(string entryId, string rejector);

    /// <summary>
    /// Get knowledge network statistics
    /// </summary>
    /// <returns>Graph statistics</returns>
    GraphStatistics GetStatistics();

    /// <summary>
    /// BFS traversal
    /// </summary>
    /// <param name="startEntity">Start entity</param>
    /// <param name="maxDepth">Maximum depth</param>
    /// <returns>List of visited entities</returns>
    List<string> BfsTraversal(string startEntity, int maxDepth = 3);

    /// <summary>
    /// DFS traversal
    /// </summary>
    /// <param name="startEntity">Start entity</param>
    /// <param name="maxDepth">Maximum depth</param>
    /// <returns>List of visited entities</returns>
    List<string> DfsTraversal(string startEntity, int maxDepth = 3);

    /// <summary>
    /// Detect cycles
    /// </summary>
    /// <returns>Whether a cycle exists</returns>
    bool HasCycle();

    /// <summary>
    /// Save knowledge network to file system
    /// </summary>
    void Save();

    /// <summary>
    /// Load knowledge network from file system
    /// </summary>
    void Load();

    /// <summary>
    /// Backup knowledge network
    /// </summary>
    /// <param name="backupPath">Backup path</param>
    void Backup(string backupPath);

    /// <summary>
    /// Restore knowledge network from backup
    /// </summary>
    /// <param name="backupPath">Backup path</param>
    void Restore(string backupPath);
}

/// <summary>
/// Relationship path
/// </summary>
public class RelationshipPath
{
    /// <summary>
    /// Sequence of nodes in the path
    /// </summary>
    public List<string> Nodes { get; set; } = new();

    /// <summary>
    /// Sequence of relations in the path
    /// </summary>
    public List<string> Relations { get; set; } = new();

    /// <summary>
    /// Path length
    /// </summary>
    public int Length => Nodes.Count - 1;

    /// <summary>
    /// String representation of the path
    /// </summary>
    public override string ToString()
    {
        if (Nodes.Count == 0)
            return "[]";

        var parts = new List<string> { Nodes[0] };
        for (int i = 0; i < Relations.Count; i++)
        {
            parts.Add($"-{Relations[i]}->");
            parts.Add(Nodes[i + 1]);
        }
        return string.Join(" ", parts);
    }
}
