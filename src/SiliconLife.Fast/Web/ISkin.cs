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

namespace SiliconLife.Fast.Web;

/// <summary>
/// Skin preview data for the init page skin selector.
/// Provides color palette, icon, and description.
/// </summary>
public class SkinPreviewInfo
{
    /// <summary>Emoji icon representing the skin</summary>
    public string Icon { get; init; } = "";

    /// <summary>Short description shown below the skin name</summary>
    public string Description { get; init; } = "";

    /// <summary>Primary background color</summary>
    public string BackgroundColor { get; init; } = "#0f172a";

    /// <summary>Secondary background color (sidebar, header)</summary>
    public string SecondaryBgColor { get; init; } = "#1e293b";

    /// <summary>Card background color</summary>
    public string CardColor { get; init; } = "#334155";

    /// <summary>Primary accent color (buttons, links)</summary>
    public string AccentColor { get; init; } = "#3b82f6";

    /// <summary>Text color (foreground)</summary>
    public string TextColor { get; init; } = "#f1f5f9";

    /// <summary>Border color</summary>
    public string BorderColor { get; init; } = "#475569";
}

/// <summary>
/// Skin interface defining the visual theme and layout structure.
/// Based on enterprise-grade UI design (Azure Portal / Alibaba Cloud Console style).
/// </summary>
public interface ISkin
{
    /// <summary>Skin code identifier (e.g., "enterprise", "light", "geek")</summary>
    string Code { get; }

    /// <summary>Skin display name</summary>
    string Name { get; }

    /// <summary>Preview information for skin selector</summary>
    SkinPreviewInfo PreviewInfo { get; }

    /// <summary>
    /// Get CSS variables for this skin theme.
    /// Returns CSS variable definitions like:
    /// :root { --bg-primary: #0f172a; --accent-primary: #3b82f6; ... }
    /// </summary>
    CssBuilder GetThemeVariables();

    /// <summary>
    /// Render the complete page layout with sidebar, header, and content area.
    /// This is the main entry point for page rendering.
    /// </summary>
    /// <param name="content">The page content to render in the main area</param>
    /// <returns>Complete HTML structure</returns>
    H RenderLayout(H content);

    /// <summary>
    /// Render error page layout.
    /// </summary>
    /// <param name="message">Error message component</param>
    /// <returns>Error page HTML</returns>
    H RenderErrorPage(H message);

    /// <summary>
    /// Get additional custom styles (beyond theme variables).
    /// Use this for skin-specific component styling.
    /// </summary>
    CssBuilder GetCustomStyles();

    /// <summary>
    /// Get skin-specific JavaScript.
    /// Use this for theme-specific interactions (e.g., dark mode toggle).
    /// </summary>
    JsSyntax GetScripts();
}

/// <summary>
/// Manages skin registration and discovery.
/// </summary>
public class SkinManager
{
    private readonly Dictionary<string, Type> _skins = new();
    private readonly Dictionary<string, string> _skinNames = new();

    public SkinManager()
    {
    }

    /// <summary>
    /// Register a skin type.
    /// </summary>
    /// <param name="skinType">Type implementing ISkin</param>
    public void RegisterSkin(Type skinType)
    {
        if (!typeof(ISkin).IsAssignableFrom(skinType))
            throw new ArgumentException("Type must implement ISkin");

        var skin = (ISkin)Activator.CreateInstance(skinType)!;
        _skins[skin.Code] = skinType;
        _skinNames[skin.Code] = skin.Name;
    }

    /// <summary>
    /// Automatically discover and register all ISkin implementations in the given assembly.
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

    /// <summary>
    /// Get a skin instance by code.
    /// </summary>
    public ISkin? GetSkin(string code)
    {
        if (_skins.TryGetValue(code, out var skinType))
        {
            return (ISkin)Activator.CreateInstance(skinType)!;
        }
        return null;
    }

    /// <summary>
    /// Get the current active skin from configuration.
    /// </summary>
    public ISkin? GetSkin()
    {
        if (Config.Instance.Data is DefaultConfigData defaultConfig && !string.IsNullOrEmpty(defaultConfig.WebSkin))
        {
            return GetSkin(defaultConfig.WebSkin);
        }
        return null;
    }

    /// <summary>
    /// Get all registered skin codes.
    /// </summary>
    public IEnumerable<string> GetAvailableSkins() => _skins.Keys;

    /// <summary>
    /// Get skin name by code.
    /// </summary>
    public string? GetSkinName(string code) =>
        _skinNames.GetValueOrDefault(code);
}
