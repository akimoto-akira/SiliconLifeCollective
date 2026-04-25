// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

using SiliconLife.Default.Help;

namespace SiliconLife.Default.Web.Models;

/// <summary>
/// View model for help documentation pages.
/// </summary>
public class HelpViewModel : ViewModelBase
{
    /// <summary>
    /// List of all help topics
    /// </summary>
    public List<HelpTopic> Topics { get; set; } = new();

    /// <summary>
    /// Current selected topic
    /// </summary>
    public HelpTopic? CurrentTopic { get; set; }

    /// <summary>
    /// Rendered HTML content of the current document
    /// </summary>
    public string ContentHtml { get; set; } = string.Empty;

    /// <summary>
    /// Search query string
    /// </summary>
    public string SearchQuery { get; set; } = string.Empty;

    /// <summary>
    /// Whether this is a search result page
    /// </summary>
    public bool IsSearchResult { get; set; }

    /// <summary>
    /// Previous topic in the list (for navigation)
    /// </summary>
    public HelpTopic? PreviousTopic { get; set; }

    /// <summary>
    /// Next topic in the list (for navigation)
    /// </summary>
    public HelpTopic? NextTopic { get; set; }
}
