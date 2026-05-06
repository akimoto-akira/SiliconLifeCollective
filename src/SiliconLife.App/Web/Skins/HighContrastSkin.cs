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

using SiliconLife.App.Web;

namespace SiliconLife.App.Web.Skins;

/// <summary>
/// High contrast theme - accessibility focused
/// Target: Users with visual impairments, long-hour usage
/// </summary>
public class HighContrastSkin : ISkin
{
    public string Code => "high-contrast";
    public string Name => "High Contrast";

    public SkinPreviewInfo PreviewInfo => new()
    {
        Icon = "\u267f",
        Description = "Accessibility mode",
        BackgroundColor = "#000000",
        SecondaryBgColor = "#1a1a1a",
        CardColor = "#2d2d2d",
        AccentColor = "#00ffff",
        TextColor = "#ffffff",
        BorderColor = "#ffffff"
    };

    // New interface methods
    public CssBuilder GetThemeVariables() => GetThemeCss();

    public H RenderLayout(H content) => RenderHtml(content);

    public H RenderErrorPage(H message) => RenderError(message);

    public CssBuilder GetCustomStyles() => GetStyles();

    public H RenderHtml(H content)
    {
        return H.PageElement("Silicon Life Collective",
            new object[]
            {
                H.Style(GetStyles() + GetThemeCss().Build()),
                H.Script(GetScripts().Build()),
            },
            new object[]
            {
                H.Div(content).Class("container"),
            });
    }

    public H RenderError(H message)
    {
        return H.PageElement("错误 - Silicon Life Collective",
            new object[]
            {
                H.Style(GetStyles() + GetThemeCss().Build()),
            },
            new object[]
            {
                H.Div(
                    H.Div(
                        H.H1("出错了"),
                        message,
                        H.A("返回首页").Href("/")
                    ).Class("error")
                ).Class("container"),
            });
    }

    public CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector("*")
            .Property("box-sizing", "border-box")
            .Property("margin", "0")
            .Property("padding", "0")
            .EndSelector()
            .Selector("body")
            .Property("font-family", "-apple-system, BlinkMacSystemFont, 'Segoe UI', 'Microsoft YaHei', sans-serif")
            .Property("background", "var(--bg-primary)")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "16px")
            .Property("padding", "24px")
            .Property("line-height", "1.6")
            .EndSelector()
            .Selector(".container")
            .Property("max-width", "1200px")
            .Property("margin", "0 auto")
            .EndSelector()
            .Selector(".error")
            .Property("background", "var(--bg-card)")
            .Property("padding", "40px")
            .Property("border-radius", "8px")
            .Property("text-align", "center")
            .Property("border", "2px solid var(--border-color)")
            .EndSelector()
            .Selector(".error h1")
            .Property("color", "var(--accent-danger)")
            .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".error a")
            .Property("color", "var(--accent-primary)")
            .Property("text-decoration", "underline")
            .EndSelector();
    }

    public JsSyntax GetScripts()
    {
        return new JsBlock();
    }

    public CssBuilder GetThemeCss()
    {
        return CssBuilder.Create()
            .WithVariable("bg-primary", "#000000")
            .WithVariable("bg-secondary", "#1a1a1a")
            .WithVariable("bg-card", "#2d2d2d")
            .WithVariable("border-color", "#ffffff")
            .WithVariable("text-primary", "#ffffff")
            .WithVariable("text-secondary", "#cccccc")
            .WithVariable("accent-primary", "#00ffff")
            .WithVariable("accent-success", "#00ff00")
            .WithVariable("accent-warning", "#ffff00")
            .WithVariable("accent-danger", "#ff0000");
    }
}
