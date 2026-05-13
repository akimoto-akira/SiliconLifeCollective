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
using SiliconLife.Help;
using SiliconLife.App.Data;
using SiliconLife.App.Web.Models;
using SiliconLife.App.Web;

using SiliconLife.Common.Localization;

namespace SiliconLife.App.Web.Controllers;

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
            Config.Instance.Data.Language);
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/help";

        // Resolve language and skin from query parameters (override config)
        ResolveLocalizationFromQuery();
        ResolveSkinFromQuery();

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
    /// Resolve language from query parameter (?lang=zh-CN)
    /// </summary>
    private void ResolveLocalizationFromQuery()
    {
        var langParam = GetQueryValue("lang");
        if (!string.IsNullOrEmpty(langParam) && Enum.TryParse<Language>(langParam, ignoreCase: true, out var lang))
        {
            // Temporarily override the language in config
            var configData = Config.Instance.Data;
            configData.Language = lang;
        }
    }

    /// <summary>
    /// Resolve skin from query parameter (?skin=ChatSkin)
    /// </summary>
    private void ResolveSkinFromQuery()
    {
        var skinParam = GetQueryValue("skin");
        if (!string.IsNullOrEmpty(skinParam))
        {
            // Temporarily override the skin in config
            // Disabled for shared module - WebSkin property not available in base type
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

        // Get document content by property name (return raw markdown, not HTML)
        var content = GetDocumentByProperty(helpLocalization, topic.PropertyName);

        // Find previous and next topics
        var currentIndex = HelpTopics.AllTopics.IndexOf(topic);
        var previousTopic = currentIndex > 0 ? HelpTopics.AllTopics[currentIndex - 1] : null;
        var nextTopic = currentIndex < HelpTopics.AllTopics.Count - 1 ? HelpTopics.AllTopics[currentIndex + 1] : null;

        var vm = new HelpViewModel
        {
            Topics = HelpTopics.AllTopics,
            CurrentTopic = topic,
            ContentHtml = content, // Pass raw markdown, will be rendered by marked.js
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
        var currentLang = Config.Instance.Data.Language;
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
    /// Get current skin
    /// </summary>
    private ISkin GetSkin()
    {
        var skinManager = ServiceLocator.Instance.GetService<SkinManager>();
        return skinManager?.GetSkin() ?? new Skins.ChatSkin();
    }
}
