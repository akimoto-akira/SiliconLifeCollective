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

public class DevSkin : ISkin
{
    public string Code => "dev";
    public string Name => "Dev";

    public SkinPreviewInfo PreviewInfo => new()
    {
        Icon = "\u2699\ufe0f",
        Description = "Geek dark theme",
        BackgroundColor = "#0d1117",
        SecondaryBgColor = "#161b22",
        CardColor = "#161b22",
        AccentColor = "#58a6ff",
        TextColor = "#c9d1d9",
        BorderColor = "#30363d"
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
        return H.PageElement("Error - Silicon Life Collective",
            new object[]
            {
                H.Style(GetStyles() + GetThemeCss().Build()),
            },
            new object[]
            {
                H.Div(
                    H.Div(
                        H.H1("ERROR"),
                        message,
                        H.A("Back to Home").Href("/")
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
            .Property("font-family", "'SF Mono', 'Consolas', 'Monaco', monospace")
            .Property("background", "var(--bg-primary)")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "13px")
            .Property("padding", "24px")
            .EndSelector()
            .Selector(".container")
            .Property("max-width", "1200px")
            .Property("margin", "0 auto")
            .EndSelector()
            .Selector(".error")
            .Property("background", "var(--bg-secondary)")
            .Property("padding", "40px")
            .Property("border-radius", "6px")
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
            .WithVariable("bg-primary", "#0d1117")
            .WithVariable("bg-secondary", "#161b22")
            .WithVariable("bg-tertiary", "#21262d")
            .WithVariable("bg-card", "#161b22")
            .WithVariable("border", "#30363d")
            .WithVariable("border-color", "#30363d")
            .WithVariable("text-primary", "#c9d1d9")
            .WithVariable("text-secondary", "#8b949e")
            .WithVariable("accent-primary", "#58a6ff")
            .WithVariable("accent-color", "#58a6ff")
            .WithVariable("accent-success", "#3fb950")
            .WithVariable("accent-warning", "#d29922")
            .WithVariable("accent-error", "#f85149")
            .WithVariable("accent-info", "#58a6ff")
            .Selector(".btn")
            .Property("padding", "6px 12px")
            .Property("border-radius", "4px")
            .Property("border", "1px solid var(--border)")
            .Property("background", "var(--bg-tertiary)")
            .Property("color", "var(--text-primary)")
            .Property("cursor", "pointer")
            .Property("font-size", "12px")
            .Property("font-family", "inherit")
            .EndSelector()
            .Selector(".btn-primary")
            .Property("background", "var(--accent-primary)")
            .Property("border-color", "var(--accent-primary)")
            .Property("color", "#0d1117")
            .EndSelector()
            .Selector(".input")
            .Property("background", "var(--bg-primary)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "4px")
            .Property("padding", "8px 10px")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "12px")
            .Property("font-family", "inherit")
            .EndSelector()
            .Selector(".card")
            .Property("background", "var(--bg-primary)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("padding", "12px")
            .EndSelector()
            .Selector(".badge")
            .Property("display", "inline-block")
            .Property("padding", "2px 8px")
            .Property("border-radius", "10px")
            .Property("font-size", "11px")
            .EndSelector()
            .Selector(".badge-primary")
            .Property("background", "rgba(88, 166, 255, 0.2)")
            .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".badge-success")
            .Property("background", "rgba(63, 185, 80, 0.2)")
            .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".avatar")
            .Property("width", "32px")
            .Property("height", "32px")
            .Property("border-radius", "4px")
            .Property("background", "var(--bg-tertiary)")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .Property("font-size", "14px")
            .EndSelector()
            .Selector(".bubble")
            .Property("background", "var(--bg-tertiary)")
            .Property("padding", "8px 12px")
            .Property("border-radius", "6px")
            .Property("font-size", "12px")
            .Property("max-width", "280px")
            .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".bubble.mine")
            .Property("background", "#1c2a3a")
            .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".switch")
            .Property("width", "36px")
            .Property("height", "20px")
            .Property("background", "var(--bg-tertiary)")
            .Property("border-radius", "10px")
            .Property("position", "relative")
            .Property("cursor", "pointer")
            .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".progress")
            .Property("height", "6px")
            .Property("background", "var(--bg-tertiary)")
            .Property("border-radius", "3px")
            .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".progress-bar")
            .Property("height", "100%")
            .Property("background", "var(--accent-primary)")
            .Property("border-radius", "3px")
            .EndSelector()
            .Selector(".progress-success .progress-bar")
            .Property("background", "var(--accent-success)")
            .EndSelector()
            .Selector(".tab")
            .Property("padding", "8px 12px")
            .Property("border-bottom", "2px solid transparent")
            .Property("cursor", "pointer")
            .Property("font-size", "12px")
            .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".tag")
            .Property("display", "inline-flex")
            .Property("align-items", "center")
            .Property("gap", "4px")
            .Property("padding", "2px 6px")
            .Property("background", "var(--bg-tertiary)")
            .Property("border-radius", "4px")
            .Property("font-size", "11px")
            .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".list-item")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("gap", "8px")
            .Property("padding", "6px 8px")
            .Property("border-radius", "4px")
            .Property("cursor", "pointer")
            .Property("font-size", "12px")
            .EndSelector()
            .Selector(".list-item:hover")
            .Property("background", "var(--bg-tertiary)")
            .EndSelector()
            .Selector(".status-dot")
            .Property("width", "8px")
            .Property("height", "8px")
            .Property("border-radius", "50%")
            .Property("display", "inline-block")
            .EndSelector()
            .Selector(".status-online")
            .Property("background", "var(--accent-success)")
            .EndSelector()
            .Selector(".status-offline")
            .Property("background", "var(--accent-error)")
            .EndSelector()
            .Selector(".status-busy")
            .Property("background", "var(--accent-warning)")
            .EndSelector();
    }
}
