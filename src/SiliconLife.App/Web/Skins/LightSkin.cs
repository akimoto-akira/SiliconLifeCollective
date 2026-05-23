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
/// Light office theme - comfortable for reading and writing
/// Target: Copywriters, content creators, office workers
/// </summary>
public class LightSkin : ISkin
{
    public string Code => "light";
    public string Name => "Light Office";

    public SkinPreviewInfo PreviewInfo => new()
    {
        Icon = "\u2600\ufe0f",
        Description = "Clean light theme",
        BackgroundColor = "#ffffff",
        SecondaryBgColor = "#f8fafc",
        CardColor = "#f1f5f9",
        AccentColor = "#2563eb",
        TextColor = "#0f172a",
        BorderColor = "#e2e8f0"
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
            .Property("font-size", "14px")
            .Property("padding", "24px")
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
            .Property("border", "1px solid var(--border-color)")
            .EndSelector()
            .Selector(".error h1")
            .Property("color", "var(--accent-danger)")
            .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".error a")
            .Property("color", "var(--accent-primary)")
            .Property("text-decoration", "none")
            .EndSelector();
    }

    public JsSyntax GetScripts()
    {
        return new JsBlock();
    }

    public CssBuilder GetThemeCss()
    {
        return CssBuilder.Create()
            .WithVariable("bg-primary", "#ffffff")
            .WithVariable("bg-secondary", "#f8fafc")
            .WithVariable("bg-card", "#f1f5f9")
            .WithVariable("border-color", "#e2e8f0")
            .WithVariable("text-primary", "#0f172a")
            .WithVariable("text-secondary", "#475569")
            .WithVariable("accent-primary", "#2563eb")
            .WithVariable("accent-success", "#10b981")
            .WithVariable("accent-warning", "#f59e0b")
            .WithVariable("accent-danger", "#ef4444");
    }
}
