// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

namespace SiliconLife.Default.Help;

/// <summary>
/// Help topic metadata (language-independent)
/// </summary>
public static class HelpTopics
{
    public static List<HelpTopic> AllTopics = new()
    {
        new()
        {
            Id = "getting-started",
            PropertyName = nameof(HelpLocalizationBase.GettingStarted),
            TitlePropertyName = nameof(HelpLocalizationBase.GettingStarted_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.GettingStarted_Tags),
            Icon = "🚀"
        },
        new()
        {
            Id = "being-management",
            PropertyName = nameof(HelpLocalizationBase.BeingManagement),
            TitlePropertyName = nameof(HelpLocalizationBase.BeingManagement_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.BeingManagement_Tags),
            Icon = "🤖"
        },
        new()
        {
            Id = "chat-system",
            PropertyName = nameof(HelpLocalizationBase.ChatSystem),
            TitlePropertyName = nameof(HelpLocalizationBase.ChatSystem_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.ChatSystem_Tags),
            Icon = "💬"
        },
        new()
        {
            Id = "dashboard",
            PropertyName = nameof(HelpLocalizationBase.Dashboard),
            TitlePropertyName = nameof(HelpLocalizationBase.Dashboard_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.Dashboard_Tags),
            Icon = "📊"
        },
        new()
        {
            Id = "task",
            PropertyName = nameof(HelpLocalizationBase.Task),
            TitlePropertyName = nameof(HelpLocalizationBase.Task_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.Task_Tags),
            Icon = "📋"
        },
        new()
        {
            Id = "timer",
            PropertyName = nameof(HelpLocalizationBase.Timer),
            TitlePropertyName = nameof(HelpLocalizationBase.Timer_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.Timer_Tags),
            Icon = "⏰"
        },
        new()
        {
            Id = "permission",
            PropertyName = nameof(HelpLocalizationBase.Permission),
            TitlePropertyName = nameof(HelpLocalizationBase.Permission_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.Permission_Tags),
            Icon = "🔒"
        },
        new()
        {
            Id = "config",
            PropertyName = nameof(HelpLocalizationBase.Config),
            TitlePropertyName = nameof(HelpLocalizationBase.Config_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.Config_Tags),
            Icon = "⚙️"
        },
        new()
        {
            Id = "faq",
            PropertyName = nameof(HelpLocalizationBase.FAQ),
            TitlePropertyName = nameof(HelpLocalizationBase.FAQ_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.FAQ_Tags),
            Icon = "❓"
        },
        new()
        {
            Id = "memory",
            PropertyName = nameof(HelpLocalizationBase.Memory),
            TitlePropertyName = nameof(HelpLocalizationBase.Memory_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.Memory_Tags),
            Icon = "🧠"
        },
        new()
        {
            Id = "ollama-setup",
            PropertyName = nameof(HelpLocalizationBase.OllamaSetup),
            TitlePropertyName = nameof(HelpLocalizationBase.OllamaSetup_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.OllamaSetup_Tags),
            Icon = "📦"
        },
        new()
        {
            Id = "bailian-dashscope",
            PropertyName = nameof(HelpLocalizationBase.BailianDashScope),
            TitlePropertyName = nameof(HelpLocalizationBase.BailianDashScope_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.BailianDashScope_Tags),
            Icon = "☁️"
        },
        new()
        {
            Id = "ai-clients",
            PropertyName = nameof(HelpLocalizationBase.AIClients),
            TitlePropertyName = nameof(HelpLocalizationBase.AIClients_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.AIClients_Tags),
            Icon = "🧠"
        },
        new()
        {
            Id = "being-soul",
            PropertyName = nameof(HelpLocalizationBase.BeingSoul),
            TitlePropertyName = nameof(HelpLocalizationBase.BeingSoul_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.BeingSoul_Tags),
            Icon = "✨"
        },
        new()
        {
            Id = "audit-log",
            PropertyName = nameof(HelpLocalizationBase.AuditLog),
            TitlePropertyName = nameof(HelpLocalizationBase.AuditLog_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.AuditLog_Tags),
            Icon = "📈"
        },
        new()
        {
            Id = "knowledge-graph",
            PropertyName = nameof(HelpLocalizationBase.KnowledgeGraph),
            TitlePropertyName = nameof(HelpLocalizationBase.KnowledgeGraph_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.KnowledgeGraph_Tags),
            Icon = "📚"
        },
        new()
        {
            Id = "work-notes",
            PropertyName = nameof(HelpLocalizationBase.WorkNotes),
            TitlePropertyName = nameof(HelpLocalizationBase.WorkNotes_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.WorkNotes_Tags),
            Icon = "📝"
        },
        new()
        {
            Id = "projects",
            PropertyName = nameof(HelpLocalizationBase.Projects),
            TitlePropertyName = nameof(HelpLocalizationBase.Projects_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.Projects_Tags),
            Icon = "📁"
        },
        new()
        {
            Id = "logging",
            PropertyName = nameof(HelpLocalizationBase.Logging),
            TitlePropertyName = nameof(HelpLocalizationBase.Logging_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.Logging_Tags),
            Icon = "📝"
        }
    };

    /// <summary>
    /// Search help topics by query
    /// </summary>
    public static List<HelpTopic> Search(string query, HelpLocalizationBase helpLocalization)
    {
        if (string.IsNullOrWhiteSpace(query))
            return AllTopics;

        var terms = query.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var results = new List<(HelpTopic Topic, int Score)>();

        foreach (var topic in AllTopics)
        {
            var tags = topic.GetTags(helpLocalization);
            var score = 0;
            foreach (var term in terms)
            {
                score += tags.Count(t => t.Contains(term, StringComparison.OrdinalIgnoreCase));
            }
            if (score > 0)
                results.Add((topic, score));
        }

        return results.OrderByDescending(r => r.Score)
                      .Select(r => r.Topic)
                      .ToList();
    }

    /// <summary>
    /// Get topic by ID
    /// </summary>
    public static HelpTopic? GetById(string id)
    {
        return AllTopics.FirstOrDefault(t => t.Id == id);
    }
}

/// <summary>
/// Help topic metadata
/// </summary>
public class HelpTopic
{
    public string Id { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public string TitlePropertyName { get; set; } = string.Empty;
    public string TagsPropertyName { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Get localized tags for this topic
    /// </summary>
    public string[] GetTags(HelpLocalizationBase helpLocalization)
    {
        if (string.IsNullOrEmpty(TagsPropertyName))
        {
            return Array.Empty<string>();
        }

        var property = helpLocalization.GetType().GetProperty(TagsPropertyName);
        var tags = property?.GetValue(helpLocalization) as string[];

        return tags ?? Array.Empty<string>();
    }
}