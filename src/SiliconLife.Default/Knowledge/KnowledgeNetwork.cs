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

using SiliconLife.Collective;
using System.Text.Json;

namespace SiliconLife.Default.Knowledge;

/// <summary>
/// Knowledge network manager - Default implementation
/// Implements IKnowledgeNetwork interface, providing complete knowledge network operation capabilities
/// </summary>
public class KnowledgeNetwork : IKnowledgeNetwork
{
    private readonly Dictionary<string, KnowledgeEntry> _entries = new();
    private readonly KnowledgeGraph _graph = new();
    private string _storagePath = string.Empty;
    private readonly ReaderWriterLockSlim _lock = new();

    /// <summary>
    /// Initialize the knowledge network
    /// </summary>
    public void Initialize(string storagePath)
    {
        _storagePath = storagePath;

        if (!Directory.Exists(storagePath))
        {
            Directory.CreateDirectory(storagePath);
        }

        Load();
    }

    /// <summary>
    /// Add a knowledge triple
    /// </summary>
    public KnowledgeEntry AddKnowledge(KnowledgeTriple triple, string createdBy)
    {
        _lock.EnterWriteLock();
        try
        {
            if (!triple.IsValid())
                throw new ArgumentException("Invalid knowledge triple: subject, predicate, and object cannot be empty");

            var entry = new KnowledgeEntry
            {
                Triple = triple,
                CreatedBy = createdBy,
                ModifiedBy = createdBy,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            _entries[entry.Id] = entry;
            _graph.AddTriple(triple.Subject, triple.Predicate, triple.Object, entry.Id);

            Save();
            return entry;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Update a knowledge entry
    /// </summary>
    public bool UpdateKnowledge(string entryId, KnowledgeTriple updatedTriple, string modifiedBy, string changeDescription)
    {
        _lock.EnterWriteLock();
        try
        {
            if (!_entries.TryGetValue(entryId, out var entry))
                return false;

            if (!updatedTriple.IsValid())
                throw new ArgumentException("Invalid updated knowledge triple");

            // Remove old triple from graph
            _graph.RemoveTriple(entry.Triple.Subject, entry.Triple.Predicate, entry.Triple.Object, entryId);

            // Update entry
            entry.Triple = updatedTriple;
            entry.AddChangeLog(modifiedBy, changeDescription);

            // Add new triple to graph
            _graph.AddTriple(updatedTriple.Subject, updatedTriple.Predicate, updatedTriple.Object, entryId);

            Save();
            return true;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Delete a knowledge entry
    /// </summary>
    public bool DeleteKnowledge(string entryId, string deletedBy)
    {
        _lock.EnterWriteLock();
        try
        {
            if (!_entries.TryGetValue(entryId, out var entry))
                return false;

            // Remove from graph
            _graph.RemoveTriple(entry.Triple.Subject, entry.Triple.Predicate, entry.Triple.Object, entryId);

            // Remove from entry dictionary
            _entries.Remove(entryId);

            Save();
            return true;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Query knowledge entry by ID
    /// </summary>
    public KnowledgeEntry? GetKnowledgeById(string entryId)
    {
        _lock.EnterReadLock();
        try
        {
            return _entries.TryGetValue(entryId, out var entry) ? entry : null;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Exact match query for triples
    /// </summary>
    public List<KnowledgeEntry> QueryKnowledge(string? subject = null, string? predicate = null, string? obj = null)
    {
        _lock.EnterReadLock();
        try
        {
            var results = _entries.Values.Where(entry =>
            {
                var triple = entry.Triple;
                var match = true;

                if (!string.IsNullOrWhiteSpace(subject))
                    match = match && triple.Subject.Contains(subject, StringComparison.OrdinalIgnoreCase);

                if (!string.IsNullOrWhiteSpace(predicate))
                    match = match && triple.Predicate.Contains(predicate, StringComparison.OrdinalIgnoreCase);

                if (!string.IsNullOrWhiteSpace(obj))
                    match = match && triple.Object.Contains(obj, StringComparison.OrdinalIgnoreCase);

                return match;
            }).ToList();

            return results;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Fuzzy search for knowledge
    /// </summary>
    public List<KnowledgeEntry> SearchKnowledge(string searchTerm, int maxResults = 50)
    {
        _lock.EnterReadLock();
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<KnowledgeEntry>();

            var term = searchTerm.ToLower();
            var results = _entries.Values
                .Where(entry =>
                    entry.Triple.Subject.ToLower().Contains(term) ||
                    entry.Triple.Predicate.ToLower().Contains(term) ||
                    entry.Triple.Object.ToLower().Contains(term) ||
                    entry.Triple.Tags.Any(t => t.ToLower().Contains(term)) ||
                    entry.Triple.Category.ToLower().Contains(term))
                .Take(maxResults)
                .ToList();

            return results;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Get relationship path between two entities
    /// </summary>
    public List<RelationshipPath> GetRelationshipPath(string startEntity, string endEntity, int maxDepth = 5)
    {
        _lock.EnterReadLock();
        try
        {
            var paths = new List<RelationshipPath>();
            var visited = new HashSet<string>();
            var path = new List<string> { startEntity };
            var relations = new List<string>();

            FindPaths(startEntity, endEntity, maxDepth, visited, path, relations, paths);

            return paths;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Validate a knowledge entry
    /// </summary>
    public bool ValidateKnowledge(string entryId, string validator)
    {
        _lock.EnterWriteLock();
        try
        {
            if (!_entries.TryGetValue(entryId, out var entry))
                return false;

            if (!entry.ValidatedBy.Contains(validator))
            {
                entry.ValidatedBy.Add(validator);
            }

            entry.ValidationStatus = KnowledgeValidationStatus.Verified;
            entry.AddChangeLog(validator, "Validation approved");

            Save();
            return true;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Reject a knowledge entry
    /// </summary>
    public bool RejectKnowledge(string entryId, string rejector)
    {
        _lock.EnterWriteLock();
        try
        {
            if (!_entries.TryGetValue(entryId, out var entry))
                return false;

            entry.ValidationStatus = KnowledgeValidationStatus.Rejected;
            entry.AddChangeLog(rejector, "Rejected");

            Save();
            return true;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get knowledge network statistics
    /// </summary>
    public GraphStatistics GetStatistics()
    {
        _lock.EnterReadLock();
        try
        {
            return _graph.Statistics;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// BFS traversal
    /// </summary>
    public List<string> BfsTraversal(string startEntity, int maxDepth = 3)
    {
        _lock.EnterReadLock();
        try
        {
            var visited = new HashSet<string>();
            var queue = new Queue<(string Entity, int Depth)>();
            var result = new List<string>();

            queue.Enqueue((startEntity, 0));
            visited.Add(startEntity);

            while (queue.Count > 0)
            {
                var (current, depth) = queue.Dequeue();
                result.Add(current);

                if (depth >= maxDepth)
                    continue;

                var relations = _graph.GetSubjectRelations(current);
                foreach (var (_, obj) in relations)
                {
                    if (!visited.Contains(obj))
                    {
                        visited.Add(obj);
                        queue.Enqueue((obj, depth + 1));
                    }
                }
            }

            return result;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// DFS traversal
    /// </summary>
    public List<string> DfsTraversal(string startEntity, int maxDepth = 3)
    {
        _lock.EnterReadLock();
        try
        {
            var visited = new HashSet<string>();
            var result = new List<string>();

            DfsRecursive(startEntity, 0, maxDepth, visited, result);

            return result;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Detect cycles
    /// </summary>
    public bool HasCycle()
    {
        _lock.EnterReadLock();
        try
        {
            var visited = new HashSet<string>();
            var recursionStack = new HashSet<string>();

            foreach (var subject in _graph.GetAllSubjects())
            {
                if (HasCycleRecursive(subject, visited, recursionStack))
                {
                    return true;
                }
            }

            return false;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Save knowledge network to file system
    /// </summary>
    public void Save()
    {
        if (string.IsNullOrEmpty(_storagePath))
            throw new InvalidOperationException("Knowledge network not initialized");

        var entriesPath = Path.Combine(_storagePath, "entries.json");
        var graphPath = Path.Combine(_storagePath, "graph.json");

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var entriesJson = JsonSerializer.Serialize(_entries.Values, options);
        File.WriteAllText(entriesPath, entriesJson);

        var graphData = new
        {
            SubjectIndex = _graph.SubjectIndex,
            ObjectIndex = _graph.ObjectIndex,
            PredicateIndex = _graph.PredicateIndex,
            TripleIds = _graph.TripleIds.ToList()
        };
        var graphJson = JsonSerializer.Serialize(graphData, options);
        File.WriteAllText(graphPath, graphJson);
    }

    /// <summary>
    /// Load knowledge network from file system
    /// </summary>
    public void Load()
    {
        if (string.IsNullOrEmpty(_storagePath))
            throw new InvalidOperationException("Knowledge network not initialized");

        var entriesPath = Path.Combine(_storagePath, "entries.json");
        var graphPath = Path.Combine(_storagePath, "graph.json");

        _lock.EnterWriteLock();
        try
        {
            if (File.Exists(entriesPath))
            {
                var entriesJson = File.ReadAllText(entriesPath);
                var readOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var entries = JsonSerializer.Deserialize<List<KnowledgeEntry>>(entriesJson, readOptions);

                _entries.Clear();
                if (entries != null)
                {
                    foreach (var entry in entries)
                    {
                        _entries[entry.Id] = entry;
                    }
                }
            }

            if (File.Exists(graphPath))
            {
                var graphJson = File.ReadAllText(graphPath);
                var graphReadOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var graphData = JsonSerializer.Deserialize<GraphData>(graphJson, graphReadOptions);

                _graph.Clear();
                if (graphData != null)
                {
                    _graph.SubjectIndex = graphData.SubjectIndex ?? new();
                    _graph.ObjectIndex = graphData.ObjectIndex ?? new();
                    _graph.PredicateIndex = graphData.PredicateIndex ?? new();
                    _graph.TripleIds = new HashSet<string>(graphData.TripleIds ?? new());
                    _graph.Statistics.TotalTriples = _graph.TripleIds.Count;
                    _graph.Statistics.TotalSubjects = _graph.SubjectIndex.Count;
                    _graph.Statistics.TotalObjects = _graph.ObjectIndex.Count;
                    _graph.Statistics.TotalPredicates = _graph.PredicateIndex.Count;
                }
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Backup knowledge network
    /// </summary>
    public void Backup(string backupPath)
    {
        if (string.IsNullOrEmpty(_storagePath))
            throw new InvalidOperationException("Knowledge network not initialized");

        if (!Directory.Exists(backupPath))
        {
            Directory.CreateDirectory(backupPath);
        }

        var entriesPath = Path.Combine(_storagePath, "entries.json");
        var graphPath = Path.Combine(_storagePath, "graph.json");

        if (File.Exists(entriesPath))
        {
            File.Copy(entriesPath, Path.Combine(backupPath, "entries.json"), true);
        }

        if (File.Exists(graphPath))
        {
            File.Copy(graphPath, Path.Combine(backupPath, "graph.json"), true);
        }
    }

    /// <summary>
    /// Restore knowledge network from backup
    /// </summary>
    public void Restore(string backupPath)
    {
        if (string.IsNullOrEmpty(_storagePath))
            throw new InvalidOperationException("Knowledge network not initialized");

        var entriesPath = Path.Combine(backupPath, "entries.json");
        var graphPath = Path.Combine(backupPath, "graph.json");

        if (File.Exists(entriesPath))
        {
            File.Copy(entriesPath, Path.Combine(_storagePath, "entries.json"), true);
        }

        if (File.Exists(graphPath))
        {
            File.Copy(graphPath, Path.Combine(_storagePath, "graph.json"), true);
        }

        Load();
    }

    #region Private helper methods

    /// <summary>
    /// DFS recursive traversal
    /// </summary>
    private void DfsRecursive(string entity, int depth, int maxDepth, HashSet<string> visited, List<string> result)
    {
        if (depth > maxDepth || visited.Contains(entity))
            return;

        visited.Add(entity);
        result.Add(entity);

        var relations = _graph.GetSubjectRelations(entity);
        foreach (var (_, obj) in relations)
        {
            if (!visited.Contains(obj))
            {
                DfsRecursive(obj, depth + 1, maxDepth, visited, result);
            }
        }
    }

    /// <summary>
    /// Cycle detection recursive
    /// </summary>
    private bool HasCycleRecursive(string entity, HashSet<string> visited, HashSet<string> recursionStack)
    {
        if (recursionStack.Contains(entity))
            return true;

        if (visited.Contains(entity))
            return false;

        visited.Add(entity);
        recursionStack.Add(entity);

        var relations = _graph.GetSubjectRelations(entity);
        foreach (var (_, obj) in relations)
        {
            if (HasCycleRecursive(obj, visited, recursionStack))
                return true;
        }

        recursionStack.Remove(entity);
        return false;
    }

    /// <summary>
    /// Find relationship paths
    /// </summary>
    private void FindPaths(string current, string target, int maxDepth, HashSet<string> visited,
                          List<string> path, List<string> relations, List<RelationshipPath> paths)
    {
        if (path.Count > maxDepth + 1)
            return;

        if (current == target && path.Count > 1)
        {
            paths.Add(new RelationshipPath
            {
                Nodes = new List<string>(path),
                Relations = new List<string>(relations)
            });
            return;
        }

        var neighbors = _graph.GetSubjectRelations(current);
        foreach (var (predicate, obj) in neighbors)
        {
            if (!visited.Contains(obj))
            {
                visited.Add(obj);
                path.Add(obj);
                relations.Add(predicate);

                FindPaths(obj, target, maxDepth, visited, path, relations, paths);

                path.RemoveAt(path.Count - 1);
                relations.RemoveAt(relations.Count - 1);
                visited.Remove(obj);
            }
        }
    }

    #endregion
}

/// <summary>
/// Graph data serialization helper class
/// </summary>
file class GraphData
{
    public Dictionary<string, Dictionary<string, List<string>>> SubjectIndex { get; set; } = new();
    public Dictionary<string, Dictionary<string, List<string>>> ObjectIndex { get; set; } = new();
    public Dictionary<string, List<(string Subject, string Object)>> PredicateIndex { get; set; } = new();
    public List<string> TripleIds { get; set; } = new();
}
