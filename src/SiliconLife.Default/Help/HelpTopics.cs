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
            Id = "task-timer",
            PropertyName = nameof(HelpLocalizationBase.TaskTimer),
            TitlePropertyName = nameof(HelpLocalizationBase.TaskTimer_Title),
            TagsPropertyName = nameof(HelpLocalizationBase.TaskTimer_Tags),
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
