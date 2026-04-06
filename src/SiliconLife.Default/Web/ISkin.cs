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

namespace SiliconLife.Default.Web;

/// <summary>
/// Provides skin preview data (color palette, icon, description) for the init page skin selector.
/// </summary>
public class SkinPreviewInfo
{
    /// <summary>Emoji icon representing the skin</summary>
    public string Icon { get; init; } = "";

    /// <summary>Short description shown below the skin name</summary>
    public string Description { get; init; } = "";

    /// <summary>Primary color palette for the preview card (background, card, accent)</summary>
    public string BackgroundColor { get; init; } = "#1e293b";

    public string CardColor { get; init; } = "#0f172a";

    public string AccentColor { get; init; } = "#3b82f6";

    /// <summary>Text color (foreground)</summary>
    public string TextColor { get; init; } = "#f1f5f9";

    /// <summary>Border color for the preview card</summary>
    public string BorderColor { get; init; } = "#334155";
}

public interface ISkin
{
    string Code { get; }
    string Name { get; }

    /// <summary>
    /// Gets preview information for displaying in the skin selector on the init page.
    /// </summary>
    SkinPreviewInfo PreviewInfo { get; }
    H RenderHtml(H content);
    H RenderError(H message);
    CssBuilder GetStyles();
    JsSyntax GetScripts();

    H RenderButton(string text, string variant = "primary", string size = "medium");
    H RenderInput(string placeholder = "", string size = "medium", string? value = null);
    H RenderTextarea(string placeholder = "", int rows = 4);
    H RenderSelect(IEnumerable<string> options, string? selected = null);
    H RenderCheckbox(string label, bool isChecked = false);
    H RenderBadge(string text, string variant = "primary");
    H RenderTag(string text);
    H RenderCard(string title, string content);
    H RenderAvatar(string text, string size = "medium");
    H RenderBubble(string text, bool isMine = false);
    H RenderSwitch(bool isChecked = false);
    H RenderProgress(double value, string variant = "primary");
    H RenderTabs(IEnumerable<string> tabs, int activeIndex = 0);
    H RenderListItem(string title, string? subtitle = null, string? avatar = null, bool active = false);
    H RenderDivider();
    H RenderCode(string code);
    H RenderStatCard(string label, string value, string variant = "primary");
    H RenderBreadcrumb(IEnumerable<string> items);
    H RenderTable(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows);
    H RenderPagination(int totalPages, int currentPage = 1);
    H RenderDropdown(string triggerText, IEnumerable<string> items);
    H RenderStatusIndicator(string status);
    H RenderQuote(string text);
    H RenderInspirationCard(string icon, string text);
    CssBuilder GetThemeCss();
}

public class SkinManager
{
    private readonly Dictionary<string, Type> _skins = new();
    private readonly Dictionary<string, string> _skinNames = new();

    public SkinManager()
    {
    }

    public void RegisterSkin(Type skinType)
    {
        if (!typeof(ISkin).IsAssignableFrom(skinType))
            throw new ArgumentException("Type must implement ISkin");

        var skin = (ISkin)Activator.CreateInstance(skinType)!;
        _skins[skin.Code] = skinType;
        _skinNames[skin.Code] = skin.Name;
    }

    /// <summary>
    /// Automatically discovers and registers all ISkin implementations in the given assembly.
    /// </summary>
    public void DiscoverSkins(System.Reflection.Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (typeof(ISkin).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
            {
                RegisterSkin(type);
            }
        }
    }

    public ISkin? GetSkin(string code)
    {
        if (_skins.TryGetValue(code, out var skinType))
        {
            return (ISkin)Activator.CreateInstance(skinType)!;
        }
        return null;
    }

    public ISkin? GetSkin()
    {
        if (Config.Instance.Data is DefaultConfigData defaultConfig && !string.IsNullOrEmpty(defaultConfig.WebSkin))
        {
            return GetSkin(defaultConfig.WebSkin);
        }
        return null;
    }

    public IEnumerable<string> GetAvailableSkins() => _skins.Keys;

    public string? GetSkinName(string code) =>
        _skinNames.GetValueOrDefault(code);
}
