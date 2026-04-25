// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

using SiliconLife.Collective;
using SiliconLife.Default.Help;
using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web;

/// <summary>
/// Controller for help documentation pages.
/// </summary>
[WebCode]
public class HelpController : Controller
{
    private DefaultLocalizationBase _localization;

    public HelpController()
    {
        _localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(
            ((DefaultConfigData)Config.Instance.Data).Language);
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/help";

        if (path == "/help" || path == "/help/index")
            Index();
        else if (path.StartsWith("/help/"))
            ShowTopic(path.Substring(6)); // Remove "/help/"
        else if (path == "/api/help/search")
            Search();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    /// <summary>
    /// Show help index page with all topics
    /// </summary>
    private void Index()
    {
        var helpLocalization = GetHelpLocalization();

        var vm = new HelpViewModel
        {
            Topics = HelpTopics.AllTopics,
            Skin = GetSkin(),
            ActiveMenu = "help"
        };

        var view = new Views.HelpView();
        var html = view.Render(vm);
        RenderHtml(html);
    }

    /// <summary>
    /// Show specific help topic
    /// </summary>
    private void ShowTopic(string topicId)
    {
        var topic = HelpTopics.GetById(topicId);
        if (topic == null)
        {
            Response.StatusCode = 404;
            RenderHtml("<h1>404 - Topic Not Found</h1>");
            return;
        }

        var helpLocalization = GetHelpLocalization();

        // Get document content by property name
        var content = GetDocumentByProperty(helpLocalization, topic.PropertyName);
        var htmlContent = SimpleMarkdownToHtml(content);

        // Find previous and next topics
        var currentIndex = HelpTopics.AllTopics.IndexOf(topic);
        var previousTopic = currentIndex > 0 ? HelpTopics.AllTopics[currentIndex - 1] : null;
        var nextTopic = currentIndex < HelpTopics.AllTopics.Count - 1 ? HelpTopics.AllTopics[currentIndex + 1] : null;

        var vm = new HelpViewModel
        {
            Topics = HelpTopics.AllTopics,
            CurrentTopic = topic,
            ContentHtml = htmlContent,
            Skin = GetSkin(),
            ActiveMenu = "help",
            PreviousTopic = previousTopic,
            NextTopic = nextTopic
        };

        var view = new Views.HelpView();
        var html = view.Render(vm);
        RenderHtml(html);
    }

    /// <summary>
    /// Search help topics
    /// </summary>
    private void Search()
    {
        var query = Request.QueryString["q"] ?? "";
        var helpLocalization = GetHelpLocalization();
        var results = HelpTopics.Search(query, helpLocalization);

        RenderJson(new
        {
            query = query,
            count = results.Count,
            results = results.Select(t => new
            {
                id = t.Id,
                icon = t.Icon,
                propertyName = t.PropertyName
            }).ToList()
        });
    }

    /// <summary>
    /// Get help localization for current language
    /// </summary>
    private HelpLocalizationBase GetHelpLocalization()
    {
        var currentLang = ((DefaultConfigData)Config.Instance.Data).Language;
        return HelpLocalizationFactory.Create(currentLang);
    }

    /// <summary>
    /// Get document content by property name using reflection
    /// </summary>
    private string GetDocumentByProperty(HelpLocalizationBase help, string propertyName)
    {
        var property = help.GetType().GetProperty(propertyName);
        return property?.GetValue(help) as string ?? string.Empty;
    }

    /// <summary>
    /// Simple Markdown to HTML converter
    /// </summary>
    private string SimpleMarkdownToHtml(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return string.Empty;

        var lines = markdown.Split('\n');
        var html = new System.Text.StringBuilder();
        var inCodeBlock = false;
        var inList = false;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            // Code blocks
            if (trimmedLine.StartsWith("```"))
            {
                if (inCodeBlock)
                {
                    html.AppendLine("</code></pre>");
                    inCodeBlock = false;
                }
                else
                {
                    if (inList)
                    {
                        html.AppendLine("</ul>");
                        inList = false;
                    }
                    html.AppendLine("<pre><code>");
                    inCodeBlock = true;
                }
                continue;
            }

            if (inCodeBlock)
            {
                html.AppendLine(System.Net.WebUtility.HtmlEncode(line));
                continue;
            }

            // Headers
            if (trimmedLine.StartsWith("# "))
            {
                if (inList)
                {
                    html.AppendLine("</ul>");
                    inList = false;
                }
                html.AppendLine($"<h1>{trimmedLine.Substring(2)}</h1>");
            }
            else if (trimmedLine.StartsWith("## "))
            {
                if (inList)
                {
                    html.AppendLine("</ul>");
                    inList = false;
                }
                html.AppendLine($"<h2>{trimmedLine.Substring(3)}</h2>");
            }
            else if (trimmedLine.StartsWith("### "))
            {
                if (inList)
                {
                    html.AppendLine("</ul>");
                    inList = false;
                }
                html.AppendLine($"<h3>{trimmedLine.Substring(4)}</h3>");
            }
            // Lists
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* "))
            {
                if (!inList)
                {
                    html.AppendLine("<ul>");
                    inList = true;
                }
                html.AppendLine($"<li>{trimmedLine.Substring(2)}</li>");
            }
            // Numbered lists
            else if (System.Text.RegularExpressions.Regex.IsMatch(trimmedLine, @"^\d+\.\s"))
            {
                if (!inList)
                {
                    html.AppendLine("<ol>");
                    inList = true;
                }
                var match = System.Text.RegularExpressions.Regex.Match(trimmedLine, @"^\d+\.\s(.+)");
                if (match.Success)
                {
                    html.AppendLine($"<li>{match.Groups[1].Value}</li>");
                }
            }
            // Empty lines
            else if (string.IsNullOrEmpty(trimmedLine))
            {
                if (inList)
                {
                    html.AppendLine("</ul></ol>".Contains("<ul>") ? "</ul>" : "</ol>");
                    inList = false;
                }
                html.AppendLine("<br/>");
            }
            // Regular paragraphs
            else
            {
                if (inList)
                {
                    html.AppendLine("</ul>");
                    inList = false;
                }
                html.AppendLine($"<p>{trimmedLine}</p>");
            }
        }

        // Close any open tags
        if (inCodeBlock)
            html.AppendLine("</code></pre>");
        if (inList)
            html.AppendLine("</ul>");

        return html.ToString();
    }

    /// <summary>
    /// Get current skin
    /// </summary>
    private ISkin GetSkin()
    {
        var skinManager = ServiceLocator.Instance.GetService<SkinManager>();
        return skinManager?.GetSkin() ?? new Skins.ChatSkin();
    }
}
