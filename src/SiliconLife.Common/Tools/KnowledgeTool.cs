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

using SiliconLife.Collective;
using System.Text.Json;

using SiliconLife.Common.Localization;

namespace SiliconLife.Default.Knowledge;

/// <summary>
/// Knowledge network tool - provides unified entry point for AI to operate knowledge network
/// Implements ITool interface, supporting knowledge add, query, update, delete and other operations
/// </summary>
[ToolAction("add", "query", "update", "delete", "search", "get_path", "get_neighbors", "get_degree", "degree_distribution", "traverse", "has_cycle", "validate", "stats")]
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task | ToolScenarioFlag.Timer)]
public class KnowledgeTool : ITool
{
    private readonly IKnowledgeNetwork? _knowledgeNetwork;
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<KnowledgeTool>();

    /// <summary>
    /// Tool name
    /// </summary>
    public string Name => "knowledge";

    /// <summary>
    /// Tool description
    /// </summary>
    public string Description => "Knowledge network operation tool for adding, querying, updating, deleting and searching knowledge triples";

    /// <summary>
    /// Parameterless constructor - for reflection auto-registration
    /// Gets IKnowledgeNetwork instance via ServiceLocator
    /// </summary>
    public KnowledgeTool()
    {
        _knowledgeNetwork = ServiceLocator.Instance.Get<IKnowledgeNetwork>();
        
        if (_knowledgeNetwork == null)
        {
            _logger.Warn(null, "KnowledgeTool: IKnowledgeNetwork not found in ServiceLocator");
        }
    }

    /// <summary>
    /// Parameterized constructor - for manual creation and testing
    /// </summary>
    public KnowledgeTool(IKnowledgeNetwork knowledgeNetwork)
    {
        _knowledgeNetwork = knowledgeNetwork;
    }

    /// <summary>
    /// Get localized display name
    /// </summary>
    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    /// <summary>
    /// Get parameter schema
    /// </summary>
    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "add", "query", "update", "delete", "search", "get_path", "get_neighbors", "get_degree", "degree_distribution", "traverse", "has_cycle", "validate", "stats" },
                    ["description"] = "Operation type: add-query-update-delete-search-get_path-get_neighbors-get_degree-degree_distribution-traverse-has_cycle-validate-stats"
                },
                ["subject"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Subject (for add/query/update)"
                },
                ["predicate"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Predicate/relation (for add/query/update)"
                },
                ["object"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Object (for add/query/update)"
                },
                ["entry_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Knowledge entry ID (for update/delete/validate)"
                },
                ["search_term"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Search term (for search)"
                },
                ["start_entity"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Start entity (for get_path)"
                },
                ["end_entity"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Target entity (for get_path)"
                },
                ["max_depth"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum depth (for get_path)",
                    ["default"] = 5
                },
                ["confidence"] = new Dictionary<string, object>
                {
                    ["type"] = "number",
                    ["description"] = "Confidence level 0.0-1.0 (for add)",
                    ["default"] = 1.0
                },
                ["source"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Knowledge source (for add)"
                },
                ["category"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Knowledge category (for add)"
                },
                ["tags"] = new Dictionary<string, object>
                {
                    ["type"] = "array",
                    ["items"] = new Dictionary<string, object> { ["type"] = "string" },
                    ["description"] = "Tag list (for add)"
                },
                ["max_results"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum number of results (for search)",
                    ["default"] = 50
                },
                ["entity"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Entity name (for get_neighbors, get_degree, traverse)"
                },
                ["max_hops"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum number of hops (for get_neighbors)",
                    ["default"] = 1
                },
                ["direction"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "outgoing", "incoming", "both" },
                    ["description"] = "Direction for neighbor query: outgoing, incoming, or both",
                    ["default"] = "both"
                },
                ["traverse_method"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "bfs", "dfs" },
                    ["description"] = "Traversal method (for traverse)",
                    ["default"] = "bfs"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    /// <summary>
    /// Execute tool invocation
    /// </summary>
    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (_knowledgeNetwork == null)
        {
            return ToolResult.Failed("Knowledge network not initialized");
        }

        try
        {
            if (!parameters.TryGetValue("action", out var actionObj) || actionObj == null)
            {
                return ToolResult.Failed("Missing required parameter: action");
            }

            var action = actionObj.ToString()?.ToLowerInvariant();

            return action switch
            {
                "add" => ExecuteAddKnowledge(callerId, parameters),
                "query" => ExecuteQueryKnowledge(parameters),
                "update" => ExecuteUpdateKnowledge(callerId, parameters),
                "delete" => ExecuteDeleteKnowledge(callerId, parameters),
                "search" => ExecuteSearchKnowledge(parameters),
                "get_path" => ExecuteGetRelationshipPath(parameters),
                "get_neighbors" => ExecuteGetNeighbors(parameters),
                "get_degree" => ExecuteGetNodeDegree(parameters),
                "degree_distribution" => ExecuteGetDegreeDistribution(),
                "traverse" => ExecuteTraversal(parameters),
                "has_cycle" => ExecuteHasCycle(),
                "validate" => ExecuteValidateKnowledge(callerId, parameters),
                "stats" => ExecuteGetKnowledgeStats(),
                _ => ToolResult.Failed($"Unknown operation type: {action}")
            };
        }
        catch (Exception ex)
        {
            _logger.Error(callerId, "Knowledge tool execution failed: {0}", ex.Message);
            return ToolResult.Failed($"Knowledge tool execution failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Execute add knowledge
    /// </summary>
    private ToolResult ExecuteAddKnowledge(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("subject", out var subjectObj) ||
            !parameters.TryGetValue("predicate", out var predicateObj) ||
            !parameters.TryGetValue("object", out var objectObj))
        {
            return ToolResult.Failed("Adding knowledge requires subject, predicate, object parameters");
        }

        var triple = new KnowledgeTriple
        {
            Subject = subjectObj?.ToString() ?? string.Empty,
            Predicate = predicateObj?.ToString() ?? string.Empty,
            Object = objectObj?.ToString() ?? string.Empty,
            Confidence = parameters.TryGetValue("confidence", out var confObj) ? SafeConvertToDouble(confObj, 1.0) : 1.0,
            Source = parameters.TryGetValue("source", out var sourceObj) ? sourceObj?.ToString() ?? string.Empty : string.Empty,
            Category = parameters.TryGetValue("category", out var catObj) ? catObj?.ToString() ?? string.Empty : string.Empty
        };

        if (parameters.TryGetValue("tags", out var tagsObj) && tagsObj is List<object> tagsList)
        {
            triple.Tags = tagsList.Select(t => t?.ToString() ?? string.Empty).ToList();
        }

        var entry = _knowledgeNetwork.AddKnowledge(triple, callerId.ToString());

        return ToolResult.Successful("Knowledge added successfully", new
        {
            entry_id = entry.Id,
            subject = entry.Triple.Subject,
            predicate = entry.Triple.Predicate,
            object_value = entry.Triple.Object,
            confidence = entry.Triple.Confidence,
            created_at = entry.CreatedAt
        });
    }

    /// <summary>
    /// Execute query knowledge
    /// </summary>
    private ToolResult ExecuteQueryKnowledge(Dictionary<string, object> parameters)
    {
        var subject = parameters.TryGetValue("subject", out var sObj) ? sObj?.ToString() : null;
        var predicate = parameters.TryGetValue("predicate", out var pObj) ? pObj?.ToString() : null;
        var obj = parameters.TryGetValue("object", out var oObj) ? oObj?.ToString() : null;

        var results = _knowledgeNetwork.QueryKnowledge(subject, predicate, obj);

        var entries = results.Select(e => new
        {
            id = e.Id,
            subject = e.Triple.Subject,
            predicate = e.Triple.Predicate,
            object_value = e.Triple.Object,
            confidence = e.Triple.Confidence,
            validation_status = e.ValidationStatus.ToString(),
            created_at = e.CreatedAt,
            version = e.Version
        }).ToList();

        return ToolResult.Successful($"Queried {entries.Count} knowledge entries", new
        {
            count = entries.Count,
            entries
        });
    }

    /// <summary>
    /// Execute update knowledge
    /// </summary>
    private ToolResult ExecuteUpdateKnowledge(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("entry_id", out var entryIdObj) ||
            !parameters.TryGetValue("subject", out var subjectObj) ||
            !parameters.TryGetValue("predicate", out var predicateObj) ||
            !parameters.TryGetValue("object", out var objectObj))
        {
            return ToolResult.Failed("Updating knowledge requires entry_id, subject, predicate, object parameters");
        }

        var entryId = entryIdObj?.ToString() ?? string.Empty;
        var updatedTriple = new KnowledgeTriple
        {
            Subject = subjectObj?.ToString() ?? string.Empty,
            Predicate = predicateObj?.ToString() ?? string.Empty,
            Object = objectObj?.ToString() ?? string.Empty
        };

        var changeDescription = parameters.TryGetValue("change_description", out var descObj)
            ? descObj?.ToString() ?? "Update knowledge"
            : "Update knowledge";

        var success = _knowledgeNetwork.UpdateKnowledge(entryId, updatedTriple, callerId.ToString(), changeDescription);

        return ToolResult.Successful(success ? "Knowledge updated successfully" : "Knowledge update failed, entry does not exist", new { success });
    }

    /// <summary>
    /// Execute delete knowledge
    /// </summary>
    private ToolResult ExecuteDeleteKnowledge(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("entry_id", out var entryIdObj))
        {
            return ToolResult.Failed("Deleting knowledge requires entry_id parameter");
        }

        var entryId = entryIdObj?.ToString() ?? string.Empty;
        var success = _knowledgeNetwork.DeleteKnowledge(entryId, callerId.ToString());

        return ToolResult.Successful(success ? "Knowledge deleted successfully" : "Knowledge deletion failed, entry does not exist", new { success });
    }

    /// <summary>
    /// Execute search knowledge
    /// </summary>
    private ToolResult ExecuteSearchKnowledge(Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("search_term", out var searchTermObj))
        {
            return ToolResult.Failed("Searching knowledge requires search_term parameter");
        }

        var searchTerm = searchTermObj?.ToString() ?? string.Empty;
        var maxResults = parameters.TryGetValue("max_results", out var maxObj) ? SafeConvertToInt32(maxObj, 50) : 50;

        var results = _knowledgeNetwork.SearchKnowledge(searchTerm, maxResults);

        var entries = results.Select(e => new
        {
            id = e.Id,
            subject = e.Triple.Subject,
            predicate = e.Triple.Predicate,
            object_value = e.Triple.Object,
            confidence = e.Triple.Confidence,
            validation_status = e.ValidationStatus.ToString(),
            created_at = e.CreatedAt
        }).ToList();

        return ToolResult.Successful($"Searched {entries.Count} knowledge entries", new
        {
            search_term = searchTerm,
            count = entries.Count,
            entries
        });
    }

    /// <summary>
    /// Execute get relationship path
    /// </summary>
    private ToolResult ExecuteGetRelationshipPath(Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("start_entity", out var startObj) ||
            !parameters.TryGetValue("end_entity", out var endObj))
        {
            return ToolResult.Failed("Getting relationship path requires start_entity and end_entity parameters");
        }

        var startEntity = startObj?.ToString() ?? string.Empty;
        var endEntity = endObj?.ToString() ?? string.Empty;
        var maxDepth = parameters.TryGetValue("max_depth", out var depthObj) ? SafeConvertToInt32(depthObj, 5) : 5;

        var paths = _knowledgeNetwork.GetRelationshipPath(startEntity, endEntity, maxDepth);

        var pathResults = paths.Select(p => new
        {
            length = p.Length,
            path = p.ToString()
        }).ToList();

        return ToolResult.Successful($"Found {pathResults.Count} paths", new
        {
            count = pathResults.Count,
            paths = pathResults
        });
    }

    /// <summary>
    /// Execute validate knowledge
    /// </summary>
    private ToolResult ExecuteValidateKnowledge(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("entry_id", out var entryIdObj))
        {
            return ToolResult.Failed("Validating knowledge requires entry_id parameter");
        }

        var entryId = entryIdObj?.ToString() ?? string.Empty;
        var success = _knowledgeNetwork.ValidateKnowledge(entryId, callerId.ToString());

        return ToolResult.Successful(success ? "Knowledge validated successfully" : "Knowledge validation failed, entry does not exist", new { success });
    }

    /// <summary>
    /// Execute get statistics
    /// </summary>
    private ToolResult ExecuteGetKnowledgeStats()
    {
        var stats = _knowledgeNetwork.GetStatistics();

        return ToolResult.Successful("Statistics retrieved successfully", new
        {
            total_triples = stats.TotalTriples,
            total_subjects = stats.TotalSubjects,
            total_objects = stats.TotalObjects,
            total_predicates = stats.TotalPredicates,
            avg_relations_per_subject = Math.Round(stats.AverageRelationsPerSubject, 2),
            avg_relations_per_object = Math.Round(stats.AverageRelationsPerObject, 2)
        });
    }

    /// <summary>
    /// Execute get neighbors
    /// </summary>
    private ToolResult ExecuteGetNeighbors(Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("entity", out var entityObj))
        {
            return ToolResult.Failed("Getting neighbors requires entity parameter");
        }

        var entity = entityObj?.ToString() ?? string.Empty;
        var maxHops = parameters.TryGetValue("max_hops", out var hopsObj) ? SafeConvertToInt32(hopsObj, 1) : 1;
        var direction = parameters.TryGetValue("direction", out var dirObj) ? dirObj?.ToString() ?? "both" : "both";

        var neighbors = _knowledgeNetwork.GetNeighbors(entity, maxHops, direction);

        var neighborResults = neighbors.Select(n => new
        {
            entity = n.Entity,
            distance = n.Distance,
            predicate = n.Predicate,
            direction = n.Direction
        }).ToList();

        return ToolResult.Successful($"Found {neighborResults.Count} neighbors within {maxHops} hops", new
        {
            start_entity = entity,
            max_hops = maxHops,
            direction,
            count = neighborResults.Count,
            neighbors = neighborResults
        });
    }

    /// <summary>
    /// Execute get node degree
    /// </summary>
    private ToolResult ExecuteGetNodeDegree(Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("entity", out var entityObj))
        {
            return ToolResult.Failed("Getting node degree requires entity parameter");
        }

        var entity = entityObj?.ToString() ?? string.Empty;
        var degree = _knowledgeNetwork.GetNodeDegree(entity);

        return ToolResult.Successful("Node degree retrieved successfully", new
        {
            entity = degree.Entity,
            out_degree = degree.OutDegree,
            in_degree = degree.InDegree,
            total_degree = degree.TotalDegree
        });
    }

    /// <summary>
    /// Execute get degree distribution
    /// </summary>
    private ToolResult ExecuteGetDegreeDistribution()
    {
        var distribution = _knowledgeNetwork.GetDegreeDistribution();

        var topHubs = distribution.TopHubNodes.Select(n => new
        {
            entity = n.Entity,
            out_degree = n.OutDegree,
            in_degree = n.InDegree,
            total_degree = n.TotalDegree
        }).ToList();

        return ToolResult.Successful("Degree distribution retrieved successfully", new
        {
            total_nodes = distribution.TotalNodes,
            avg_out_degree = Math.Round(distribution.AverageOutDegree, 2),
            avg_in_degree = Math.Round(distribution.AverageInDegree, 2),
            max_out_degree = distribution.MaxOutDegree,
            max_in_degree = distribution.MaxInDegree,
            out_degree_distribution = distribution.OutDegreeDistribution,
            in_degree_distribution = distribution.InDegreeDistribution,
            top_hub_nodes = topHubs
        });
    }

    /// <summary>
    /// Execute traversal
    /// </summary>
    private ToolResult ExecuteTraversal(Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("entity", out var entityObj))
        {
            return ToolResult.Failed("Traversal requires entity parameter");
        }

        var entity = entityObj?.ToString() ?? string.Empty;
        var maxDepth = parameters.TryGetValue("max_depth", out var depthObj) ? SafeConvertToInt32(depthObj, 3) : 3;
        var method = parameters.TryGetValue("traverse_method", out var methodObj) ? methodObj?.ToString() ?? "bfs" : "bfs";

        var visited = method == "dfs"
            ? _knowledgeNetwork.DfsTraversal(entity, maxDepth)
            : _knowledgeNetwork.BfsTraversal(entity, maxDepth);

        return ToolResult.Successful($"Traversal ({method}) found {visited.Count} entities", new
        {
            start_entity = entity,
            method,
            max_depth = maxDepth,
            count = visited.Count,
            entities = visited
        });
    }

    /// <summary>
    /// Execute has cycle detection
    /// </summary>
    private ToolResult ExecuteHasCycle()
    {
        var hasCycle = _knowledgeNetwork.HasCycle();

        return ToolResult.Successful("Cycle detection completed", new
        {
            has_cycle = hasCycle
        });
    }

    /// <summary>
    /// Safely convert object to double, handling JsonElement
    /// </summary>
    private double SafeConvertToDouble(object? value, double defaultValue)
    {
        if (value == null)
            return defaultValue;

        try
        {
            if (value is JsonElement jsonElement)
            {
                return jsonElement.ValueKind == JsonValueKind.Number ? jsonElement.GetDouble() : defaultValue;
            }
            return Convert.ToDouble(value);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Safely convert object to int, handling JsonElement
    /// </summary>
    private int SafeConvertToInt32(object? value, int defaultValue)
    {
        if (value == null)
            return defaultValue;

        try
        {
            if (value is JsonElement jsonElement)
            {
                return jsonElement.ValueKind == JsonValueKind.Number ? jsonElement.GetInt32() : defaultValue;
            }
            return Convert.ToInt32(value);
        }
        catch
        {
            return defaultValue;
        }
    }
}
