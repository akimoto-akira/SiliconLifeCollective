// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

using SiliconLife.Collective;
using SiliconLife.Default.Help;
using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

/// <summary>
/// View for help documentation pages.
/// </summary>
public class HelpView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as HelpViewModel;
        if (vm == null) return string.Empty;

        var title = vm.CurrentTopic != null 
            ? $"{GetTopicTitle(vm, vm.CurrentTopic)} - {vm.Localization.Help_Title}" 
            : vm.Localization.Help_Title;

        var body = RenderBody(vm);
        return RenderPage(vm.Skin, title, "help", vm.Localization, body, GetScripts(), GetStyles());
    }

    private H RenderBody(HelpViewModel vm)
    {
        var content = vm.CurrentTopic != null 
            ? RenderTopicContent(vm) 
            : RenderTopicList(vm);

        return H.Div(
            H.Div(
                H.H1(vm.Localization.Help_Title),
                RenderSearchBox(vm)
            ).Class("page-header"),
            H.Div(
                RenderTopicSidebar(vm),
                H.Div(content).Class("help-content")
            ).Class("help-layout")
        ).Class("page-content");
    }

    private H RenderSearchBox(HelpViewModel vm)
    {
        return H.Div(
            H.Create("input")
                .Attr("type", "text")
                .Id("help-search")
                .Attr("placeholder", vm.Localization.Help_Search)
                .Class("search-input")
        ).Class("search-box");
    }

    private H RenderTopicSidebar(HelpViewModel vm)
    {
        var topics = vm.Topics.Select(topic =>
        {
            var isActive = vm.CurrentTopic?.Id == topic.Id;
            return H.A(
                H.Span(topic.Icon).Class("topic-icon"),
                H.Span(GetTopicTitle(vm, topic))
            )
            .Href($"/help/{topic.Id}")
            .Class(isActive ? "topic-item active" : "topic-item");
        }).ToArray();

        return H.Div(
            H.H3(vm.Localization.Help_DocList),
            H.Div(topics).Class("topic-list")
        ).Class("help-sidebar");
    }

    private H RenderTopicList(HelpViewModel vm)
    {
        var helpLocalization = GetHelpLocalization(vm);
        var cards = vm.Topics.Select(topic =>
            H.Div(
                H.A(
                    H.Span(topic.Icon).Class("topic-icon-large"),
                    H.H3(GetTopicTitle(vm, topic)),
                    H.P(string.Join(", ", topic.GetTags(helpLocalization).Take(5)))
                ).Href($"/help/{topic.Id}")
            ).Class("topic-card")
        ).ToArray();

        return H.Div(
            H.H2(vm.Localization.Help_DocList),
            H.Div(cards).Class("topic-grid")
        );
    }

    private H RenderTopicContent(HelpViewModel vm)
    {
        if (vm.CurrentTopic == null)
            return H.P("Topic not found");

        var navigation = RenderNavigation(vm);

        return H.Div(
            navigation,
            H.Div(vm.ContentHtml).Class("markdown-content"),
            navigation
        );
    }

    private H RenderNavigation(HelpViewModel vm)
    {
        var prev = vm.PreviousTopic != null 
            ? H.A($"← {vm.Localization.Help_Previous}")
                .Href($"/help/{vm.PreviousTopic.Id}")
                .Class("nav-link")
            : H.Span();

        var next = vm.NextTopic != null
            ? H.A($"{vm.Localization.Help_Next} →")
                .Href($"/help/{vm.NextTopic.Id}")
                .Class("nav-link")
            : H.Span();

        return H.Div(prev, next).Class("topic-navigation");
    }

    /// <summary>
    /// Get localized title for a topic
    /// </summary>
    private string GetTopicTitle(HelpViewModel vm, HelpTopic topic)
    {
        // Get title from help localization using reflection
        var helpLocalization = GetHelpLocalization(vm);
        if (string.IsNullOrEmpty(topic.TitlePropertyName))
        {
            // Fallback to old behavior if TitlePropertyName is not set
            return topic.Id.Replace("-", " ").ToTitleCase();
        }

        var property = helpLocalization.GetType().GetProperty(topic.TitlePropertyName);
        var title = property?.GetValue(helpLocalization) as string;
        
        return string.IsNullOrEmpty(title) ? topic.Id.Replace("-", " ").ToTitleCase() : title;
    }

    /// <summary>
    /// Get help localization instance for current language
    /// </summary>
    private HelpLocalizationBase GetHelpLocalization(HelpViewModel vm)
    {
        var currentLang = ((DefaultConfigData)Config.Instance.Data).Language;
        return HelpLocalizationFactory.Create(currentLang);
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            // Layout
            .Selector(".help-layout")
                .Property("display", "flex")
                .Property("gap", "20px")
            .EndSelector()
            
            // Sidebar
            .Selector(".help-sidebar")
                .Property("width", "250px")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".topic-list")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "8px")
            .EndSelector()
            .Selector(".topic-item")
                .Property("padding", "10px 15px")
                .Property("border-radius", "6px")
                .Property("text-decoration", "none")
                .Property("color", "var(--text-color)")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "10px")
            .EndSelector()
            .Selector(".topic-item:hover")
                .Property("background-color", "var(--hover-bg)")
            .EndSelector()
            .Selector(".topic-item.active")
                .Property("background-color", "var(--primary-color)")
                .Property("color", "white")
            .EndSelector()
            .Selector(".topic-icon")
                .Property("font-size", "1.2em")
            .EndSelector()
            
            // Content
            .Selector(".help-content")
                .Property("flex", "1")
                .Property("min-width", "0")
            .EndSelector()
            .Selector(".markdown-content")
                .Property("line-height", "1.6")
            .EndSelector()
            .Selector(".markdown-content h1")
                .Property("font-size", "2em")
                .Property("margin-bottom", "20px")
                .Property("border-bottom", "2px solid var(--border-color)")
                .Property("padding-bottom", "10px")
            .EndSelector()
            .Selector(".markdown-content h2")
                .Property("font-size", "1.5em")
                .Property("margin-top", "30px")
                .Property("margin-bottom", "15px")
            .EndSelector()
            .Selector(".markdown-content h3")
                .Property("font-size", "1.25em")
                .Property("margin-top", "25px")
                .Property("margin-bottom", "10px")
            .EndSelector()
            .Selector(".markdown-content p")
                .Property("margin-bottom", "15px")
            .EndSelector()
            .Selector(".markdown-content ul, .markdown-content ol")
                .Property("margin-bottom", "15px")
                .Property("padding-left", "30px")
            .EndSelector()
            .Selector(".markdown-content li")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".markdown-content pre")
                .Property("background-color", "var(--code-bg)")
                .Property("padding", "15px")
                .Property("border-radius", "6px")
                .Property("overflow-x", "auto")
                .Property("margin-bottom", "15px")
            .EndSelector()
            .Selector(".markdown-content code")
                .Property("font-family", "'Courier New', monospace")
                .Property("font-size", "0.9em")
            .EndSelector()
            
            // Topic cards (index page)
            .Selector(".topic-grid")
                .Property("display", "grid")
                .Property("grid-template-columns", "repeat(auto-fill, minmax(250px, 1fr))")
                .Property("gap", "20px")
                .Property("margin-top", "20px")
            .EndSelector()
            .Selector(".topic-card")
                .Property("padding", "20px")
                .Property("border-radius", "8px")
                .Property("border", "1px solid var(--border-color)")
                .Property("transition", "transform 0.2s, box-shadow 0.2s")
            .EndSelector()
            .Selector(".topic-card:hover")
                .Property("transform", "translateY(-4px)")
                .Property("box-shadow", "0 4px 12px rgba(0,0,0,0.1)")
            .EndSelector()
            .Selector(".topic-card a")
                .Property("text-decoration", "none")
                .Property("color", "var(--text-color)")
                .Property("display", "block")
            .EndSelector()
            .Selector(".topic-icon-large")
                .Property("font-size", "2.5em")
                .Property("display", "block")
                .Property("margin-bottom", "10px")
            .EndSelector()
            .Selector(".topic-card h3")
                .Property("margin", "10px 0")
            .EndSelector()
            .Selector(".topic-card p")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "0.85em")
            .EndSelector()
            
            // Navigation
            .Selector(".topic-navigation")
                .Property("display", "flex")
                .Property("justify-content", "space-between")
                .Property("margin", "30px 0")
                .Property("padding-top", "20px")
                .Property("border-top", "1px solid var(--border-color)")
            .EndSelector()
            .Selector(".nav-link")
                .Property("padding", "10px 20px")
                .Property("border-radius", "6px")
                .Property("background-color", "var(--primary-color)")
                .Property("color", "white")
                .Property("text-decoration", "none")
            .EndSelector()
            .Selector(".nav-link:hover")
                .Property("opacity", "0.9")
            .EndSelector()
            
            // Search
            .Selector(".search-box")
                .Property("margin-top", "15px")
            .EndSelector()
            .Selector(".search-input")
                .Property("width", "100%")
                .Property("max-width", "400px")
                .Property("padding", "10px 15px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("font-size", "1em")
            .EndSelector();
    }

    private static JsSyntax GetScripts()
    {
        // Build search handler body
        var searchHandlerBody = Js.Block()
            .Add(() => Js.Const(() => "query", () => Js.Id(() => "e").Prop(() => "target").Prop(() => "value")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "query").Prop(() => "length").Op(() => "<", () => Js.Num(() => "2")), new List<JsSyntax>
                {
                    Js.Return(() => Js.Id(() => "undefined"))
                })
            }))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/help/search?q=").Op(() => "+", () => Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "query"))))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => Js.Block()
                    .Add(() => Js.Id(() => "console").Call(() => "log", () => Js.Id(() => "data")).Stmt())
                    // TODO: Display search results
                )).Stmt());

        // Build main script
        return Js.Block()
            .Add(() => Js.Const(() => "searchInput", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "help-search"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "searchInput"), new List<JsSyntax>
                {
                    Js.Id(() => "searchInput").Call(() => "addEventListener", () => Js.Str(() => "input"), () => Js.Arrow(() => new List<string> { "e" }, () => searchHandlerBody)).Stmt()
                })
            }));
    }
}

// Extension method for title case
public static class StringExtensions
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
