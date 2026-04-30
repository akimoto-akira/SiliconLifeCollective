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

namespace SiliconLife.Fast.Web.Skins;

/// <summary>
/// Minimal theme - distraction-free writing experience
/// Target: Writers, bloggers, focused work
/// </summary>
public class MinimalSkin : ISkin
{
    public string Code => "minimal";
    public string Name => "Minimal";

    public SkinPreviewInfo PreviewInfo => new()
    {
        Icon = "\u2728",
        Description = "Distraction-free",
        BackgroundColor = "#fafafa",
        SecondaryBgColor = "#f5f5f5",
        CardColor = "#ffffff",
        AccentColor = "#1a1a1a",
        TextColor = "#333333",
        BorderColor = "#e0e0e0"
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
            .Property("font-family", "'Georgia', 'Microsoft YaHei', serif")
            .Property("background", "var(--bg-primary)")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "16px")
            .Property("line-height", "1.8")
            .Property("padding", "40px 24px")
            .EndSelector()
            .Selector(".container")
            .Property("max-width", "800px")
            .Property("margin", "0 auto")
            .EndSelector()
            .Selector(".error")
            .Property("background", "var(--bg-card)")
            .Property("padding", "60px 40px")
            .Property("border-radius", "4px")
            .Property("text-align", "center")
            .Property("border", "1px solid var(--border-color)")
            .EndSelector()
            .Selector(".error h1")
            .Property("color", "var(--accent-primary)")
            .Property("margin-bottom", "24px")
            .Property("font-weight", "400")
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
            .WithVariable("bg-primary", "#fafafa")
            .WithVariable("bg-secondary", "#f5f5f5")
            .WithVariable("bg-card", "#ffffff")
            .WithVariable("border-color", "#e0e0e0")
            .WithVariable("text-primary", "#333333")
            .WithVariable("text-secondary", "#666666")
            .WithVariable("accent-primary", "#1a1a1a")
            .WithVariable("accent-success", "#2e7d32")
            .WithVariable("accent-warning", "#ed6c02")
            .WithVariable("accent-danger", "#d32f2f");
    }
}
