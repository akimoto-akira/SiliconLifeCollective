// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

using System.Text;
using SiliconLife.Collective;
using SiliconLife.Default.Help;

namespace SiliconLife.Default;

/// <summary>
/// Help documentation lookup tool for AI.
/// Allows AI to search and retrieve help documentation content.
/// </summary>
public class HelpTool : ITool
{
    public string Name => "help";

    public string Description =>
        "Search and retrieve help documentation. Use this tool to look up how to use system features. " +
        "Actions: 'list' (list all topic IDs), 'search' (search by keywords), 'get' (get specific document by ID).";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return "Help";
    }

    public Dictionary<string, object> GetParameterSchema()
    {
        var topicIds = HelpTopics.AllTopics.Select(t => (object)t.Id).ToArray();

        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Action to perform: 'list' (list all topic IDs), 'search' (search by keywords), or 'get' (get specific document by ID)",
                    ["enum"] = new[] { "list", "search", "get" }
                },
                ["query"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Search query or topic ID (e.g., 'getting-started', 'being-management', 'permission')"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        var action = parameters.TryGetValue("action", out var actionObj) ? actionObj?.ToString() ?? "list" : "list";
        var query = parameters.TryGetValue("query", out var queryObj) ? queryObj?.ToString() ?? "" : "";

        // Get current language help localization
        var currentLang = ((DefaultConfigData)Config.Instance.Data).Language;
        var helpLocalization = HelpLocalizationFactory.Create(currentLang);

        if (action == "list")
        {
            return ToolResult.Successful(ListAllTopics());
        }
        else if (action == "search")
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return ToolResult.Failed("Error: 'query' parameter is required for 'search' action.");
            }
            return ToolResult.Successful(SearchTopics(query));
        }
        else if (action == "get")
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return ToolResult.Failed("Error: 'query' parameter is required for 'get' action.");
            }
            return ToolResult.Successful(GetDocument(query, helpLocalization));
        }
        else
        {
            return ToolResult.Failed($"Error: Unknown action '{action}'. Use 'list', 'search', or 'get'.");
        }
    }

    /// <summary>
    /// List all available help topic IDs
    /// </summary>
    private string ListAllTopics()
    {
        var currentLang = ((DefaultConfigData)Config.Instance.Data).Language;
        var helpLocalization = HelpLocalizationFactory.Create(currentLang);
        
        var sb = new StringBuilder();
        sb.AppendLine($"Available help documents ({HelpTopics.AllTopics.Count} total):");
        sb.AppendLine();

        foreach (var topic in HelpTopics.AllTopics)
        {
            var displayName = topic.Id.Replace("-", " ").ToTitleCase();
            sb.AppendLine($"- **{topic.Id}** {topic.Icon} - {displayName}");
        }

        sb.AppendLine();
        sb.AppendLine("Use action='get' with a topic ID to read the full document content.");
        sb.AppendLine("Example: action='get', query='getting-started'");
        sb.AppendLine();
        sb.AppendLine("Use action='search' with keywords to find relevant documents.");
        sb.AppendLine("Example: action='search', query='permission management'");

        return sb.ToString();
    }

    /// <summary>
    /// Search help topics by keywords
    /// </summary>
    private string SearchTopics(string query)
    {
        var currentLang = ((DefaultConfigData)Config.Instance.Data).Language;
        var helpLocalization = HelpLocalizationFactory.Create(currentLang);
        var results = HelpTopics.Search(query, helpLocalization);

        if (results.Count == 0)
        {
            return $"No help documents found for '{query}'. Try different keywords or use action='get' with a specific topic ID.";
        }

        var sb = new StringBuilder();
        sb.AppendLine($"Found {results.Count} help document(s) for '{query}':");
        sb.AppendLine();

        foreach (var topic in results)
        {
            var displayName = topic.Id.Replace("-", " ").ToTitleCase();
            sb.AppendLine($"- **{topic.Id}** {topic.Icon} - {displayName}");
        }

        sb.AppendLine();
        sb.AppendLine("Use action='get' with a topic ID to read the full document content.");
        sb.AppendLine("Example: action='get', query='getting-started'");

        return sb.ToString();
    }

    /// <summary>
    /// Get specific document content by ID
    /// </summary>
    private string GetDocument(string topicId, HelpLocalizationBase helpLocalization)
    {
        var topic = HelpTopics.GetById(topicId);
        if (topic == null)
        {
            var availableTopics = string.Join(", ", HelpTopics.AllTopics.Select(t => t.Id));
            return $"Document '{topicId}' not found.\n\nAvailable topics: {availableTopics}\n\nUse action='search' to find documents by keywords.";
        }

        // Get document content by property name using reflection
        var property = helpLocalization.GetType().GetProperty(topic.PropertyName);
        var content = property?.GetValue(helpLocalization) as string;

        if (string.IsNullOrEmpty(content))
        {
            return $"Document '{topicId}' exists but has no content yet.";
        }

        return content;
    }
}

// Extension method for title case (if not already defined)
public static class HelpToolStringExtensions
{
    public static string ToTitleCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        var words = str.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
        }
        return string.Join(" ", words);
    }
}
