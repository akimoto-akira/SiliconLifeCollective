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

public class ChatSkin : ISkin
{
    public string Code => "chat";
    public string Name => "Chat";

    public SkinPreviewInfo PreviewInfo => new()
    {
        Icon = "\U0001f4ac",
        Description = "Modern dark blue",
        BackgroundColor = "#1a1a2e",
        SecondaryBgColor = "#16213e",
        CardColor = "#16213e",
        AccentColor = "#4d96ff",
        TextColor = "#eaeaea",
        BorderColor = "#0f3460"
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
            .WithVariable("bg-primary", "#1a1a2e")
            .WithVariable("bg-card", "#16213e")
            .WithVariable("border", "#0f3460")
            .WithVariable("text-primary", "#eaeaea")
            .WithVariable("text-secondary", "#a0a0a0")
            .WithVariable("accent-primary", "#4d96ff")
            .WithVariable("accent-success", "#6bcb77")
            .WithVariable("accent-warning", "#ffd93d")
            .WithVariable("accent-error", "#ff6b6b")
            .WithVariable("accent-info", "#4ecdc4")
            .Selector("*")
            .Property("box-sizing", "border-box")
            .Property("margin", "0")
            .Property("padding", "0")
            .EndSelector()
            .Selector("body")
            .Property("font-family", "-apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif")
            .Property("background", "var(--bg-primary)")
            .Property("color", "var(--text-primary)")
            .Property("padding", "24px")
            .EndSelector()
            .Selector(".container")
            .Property("max-width", "1200px")
            .Property("margin", "0 auto")
            .EndSelector()
            .Selector(".error")
            .Property("background", "var(--bg-card)")
            .Property("padding", "40px")
            .Property("border-radius", "12px")
            .Property("text-align", "center")
            .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".error h1")
            .Property("color", "var(--accent-error)")
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
            .WithVariable("bg-primary", "#1a1a2e")
            .WithVariable("bg-card", "#16213e")
            .WithVariable("bg-secondary", "#1e3a5f")
            .WithVariable("bg-tertiary", "#0f3460")
            .WithVariable("border", "#0f3460")
            .WithVariable("border-color", "#0f3460")
            .WithVariable("text-primary", "#eaeaea")
            .WithVariable("text-secondary", "#a0a0a0")
            .WithVariable("accent-primary", "#4d96ff")
            .WithVariable("accent-color", "#4d96ff")
            .WithVariable("accent-success", "#6bcb77")
            .WithVariable("accent-warning", "#ffd93d")
            .WithVariable("accent-error", "#ff6b6b")
            .WithVariable("accent-info", "#4ecdc4");
    }
}
