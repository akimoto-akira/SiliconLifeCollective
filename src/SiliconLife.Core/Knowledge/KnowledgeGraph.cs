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

namespace SiliconLife.Core.Knowledge;

/// <summary>
/// Knowledge graph - uses adjacency list structure to store knowledge network
/// Supports efficient graph traversal operations
/// </summary>
public class KnowledgeGraph
{
    /// <summary>
    /// Subject index: Subject -> (Predicate -> List<Object>)
    /// Used for fast lookup of all relations for a subject
    /// </summary>
    public Dictionary<string, Dictionary<string, List<string>>> SubjectIndex { get; set; } = new();

    /// <summary>
    /// Object index: Object -> (Predicate -> List<Subject>)
    /// Used for fast lookup of all relations for an object
    /// </summary>
    public Dictionary<string, Dictionary<string, List<string>>> ObjectIndex { get; set; } = new();

    /// <summary>
    /// Predicate index: Predicate -> List<(Subject, Object)>
    /// Used for fast lookup of all triples for a predicate
    /// </summary>
    public Dictionary<string, List<(string Subject, string Object)>> PredicateIndex { get; set; } = new();

    /// <summary>
    /// All triple ID list
    /// </summary>
    public HashSet<string> TripleIds { get; set; } = new();

    /// <summary>
    /// Graph statistics
    /// </summary>
    public GraphStatistics Statistics { get; set; } = new();

    /// <summary>
    /// Add triple to adjacency list
    /// </summary>
    public void AddTriple(string subject, string predicate, string obj, string tripleId)
    {
        // Add to subject index
        if (!SubjectIndex.ContainsKey(subject))
            SubjectIndex[subject] = new Dictionary<string, List<string>>();

        if (!SubjectIndex[subject].ContainsKey(predicate))
            SubjectIndex[subject][predicate] = new List<string>();

        if (!SubjectIndex[subject][predicate].Contains(obj))
            SubjectIndex[subject][predicate].Add(obj);

        // Add to object index
        if (!ObjectIndex.ContainsKey(obj))
            ObjectIndex[obj] = new Dictionary<string, List<string>>();

        if (!ObjectIndex[obj].ContainsKey(predicate))
            ObjectIndex[obj][predicate] = new List<string>();

        if (!ObjectIndex[obj][predicate].Contains(subject))
            ObjectIndex[obj][predicate].Add(subject);

        // Add to predicate index
        if (!PredicateIndex.ContainsKey(predicate))
            PredicateIndex[predicate] = new List<(string, string)>();

        var pair = (subject, obj);
        if (!PredicateIndex[predicate].Contains(pair))
            PredicateIndex[predicate].Add(pair);

        // Add to ID set
        TripleIds.Add(tripleId);

        // Update statistics
        UpdateStatistics();
    }

    /// <summary>
    /// Remove triple from adjacency list
    /// </summary>
    public void RemoveTriple(string subject, string predicate, string obj, string tripleId)
    {
        // Remove from subject index
        if (SubjectIndex.ContainsKey(subject) && SubjectIndex[subject].ContainsKey(predicate))
        {
            SubjectIndex[subject][predicate].Remove(obj);
            if (SubjectIndex[subject][predicate].Count == 0)
                SubjectIndex[subject].Remove(predicate);
            if (SubjectIndex[subject].Count == 0)
                SubjectIndex.Remove(subject);
        }

        // Remove from object index
        if (ObjectIndex.ContainsKey(obj) && ObjectIndex[obj].ContainsKey(predicate))
        {
            ObjectIndex[obj][predicate].Remove(subject);
            if (ObjectIndex[obj][predicate].Count == 0)
                ObjectIndex[obj].Remove(predicate);
            if (ObjectIndex[obj].Count == 0)
                ObjectIndex.Remove(obj);
        }

        // Remove from predicate index
        if (PredicateIndex.ContainsKey(predicate))
        {
            var pair = (subject, obj);
            PredicateIndex[predicate].Remove(pair);
            if (PredicateIndex[predicate].Count == 0)
                PredicateIndex.Remove(predicate);
        }

        // Remove from ID set
        TripleIds.Remove(tripleId);

        // Update statistics
        UpdateStatistics();
    }

    /// <summary>
    /// Query all relations for a subject
    /// </summary>
    public List<(string Predicate, string Object)> GetSubjectRelations(string subject)
    {
        var results = new List<(string, string)>();

        if (SubjectIndex.TryGetValue(subject, out var predicates))
        {
            foreach (var (predicate, objects) in predicates)
            {
                foreach (var obj in objects)
                {
                    results.Add((predicate, obj));
                }
            }
        }

        return results;
    }

    /// <summary>
    /// Query all relations for an object
    /// </summary>
    public List<(string Predicate, string Subject)> GetObjectRelations(string obj)
    {
        var results = new List<(string, string)>();

        if (ObjectIndex.TryGetValue(obj, out var predicates))
        {
            foreach (var (predicate, subjects) in predicates)
            {
                foreach (var subject in subjects)
                {
                    results.Add((predicate, subject));
                }
            }
        }

        return results;
    }

    /// <summary>
    /// Query all triples for a specific predicate
    /// </summary>
    public List<(string Subject, string Object)> GetPredicateRelations(string predicate)
    {
        if (PredicateIndex.TryGetValue(predicate, out var relations))
            return new List<(string, string)>(relations);

        return new List<(string, string)>();
    }

    /// <summary>
    /// Get all unique subjects
    /// </summary>
    public HashSet<string> GetAllSubjects()
    {
        return new HashSet<string>(SubjectIndex.Keys);
    }

    /// <summary>
    /// Get all unique objects
    /// </summary>
    public HashSet<string> GetAllObjects()
    {
        return new HashSet<string>(ObjectIndex.Keys);
    }

    /// <summary>
    /// Get all unique predicates
    /// </summary>
    public HashSet<string> GetAllPredicates()
    {
        return new HashSet<string>(PredicateIndex.Keys);
    }

    /// <summary>
    /// Update statistics
    /// </summary>
    private void UpdateStatistics()
    {
        Statistics.TotalTriples = TripleIds.Count;
        Statistics.TotalSubjects = SubjectIndex.Count;
        Statistics.TotalObjects = ObjectIndex.Count;
        Statistics.TotalPredicates = PredicateIndex.Count;
    }

    /// <summary>
    /// Clear the graph
    /// </summary>
    public void Clear()
    {
        SubjectIndex.Clear();
        ObjectIndex.Clear();
        PredicateIndex.Clear();
        TripleIds.Clear();
        Statistics = new GraphStatistics();
    }
}

/// <summary>
/// Graph statistics
/// </summary>
public class GraphStatistics
{
    /// <summary>
    /// Total number of triples
    /// </summary>
    public int TotalTriples { get; set; }

    /// <summary>
    /// Number of unique subjects
    /// </summary>
    public int TotalSubjects { get; set; }

    /// <summary>
    /// Number of unique objects
    /// </summary>
    public int TotalObjects { get; set; }

    /// <summary>
    /// Number of unique predicates
    /// </summary>
    public int TotalPredicates { get; set; }

    /// <summary>
    /// Average number of relations per subject
    /// </summary>
    public double AverageRelationsPerSubject => TotalSubjects > 0 ? (double)TotalTriples / TotalSubjects : 0;

    /// <summary>
    /// Average number of relations per object
    /// </summary>
    public double AverageRelationsPerObject => TotalObjects > 0 ? (double)TotalTriples / TotalObjects : 0;
}
