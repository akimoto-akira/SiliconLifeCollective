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

public class AdminSkin : ISkin
{
    public string Code => "admin";
    public string Name => "Admin";

    public SkinPreviewInfo PreviewInfo => new()
    {
        Icon = "\U0001f4ca",
        Description = "Enterprise console",
        BackgroundColor = "#f4f6f8",
        SecondaryBgColor = "#ffffff",
        CardColor = "#ffffff",
        AccentColor = "#0366d6",
        TextColor = "#24292e",
        BorderColor = "#e1e4e8"
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
            .Property("font-family", "-apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif")
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
            .Property("background", "var(--bg-white)")
            .Property("padding", "40px")
            .Property("border-radius", "8px")
            .Property("text-align", "center")
            .Property("border", "1px solid var(--border)")
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
            .WithVariable("bg-primary", "#f4f6f8")
            .WithVariable("bg-secondary", "#ffffff")
            .WithVariable("bg-tertiary", "#e9ecef")
            .WithVariable("bg-white", "#ffffff")
            .WithVariable("bg-card", "#ffffff")
            .WithVariable("bg-sidebar", "#1a1a2e")
            .WithVariable("border", "#e1e4e8")
            .WithVariable("border-color", "#e1e4e8")
            .WithVariable("text-primary", "#24292e")
            .WithVariable("text-secondary", "#586069")
            .WithVariable("text-inverse", "#ffffff")
            .WithVariable("accent-primary", "#0366d6")
            .WithVariable("accent-color", "#0366d6")
            .WithVariable("accent-success", "#28a745")
            .WithVariable("accent-warning", "#f9a825")
            .WithVariable("accent-danger", "#d32f2f")
            .WithVariable("accent-error", "#d32f2f")
            .Selector(".btn")
            .Property("padding", "8px 16px")
            .Property("border-radius", "6px")
            .Property("border", "1px solid var(--border)")
            .Property("background", "var(--bg-white)")
            .Property("color", "var(--text-primary)")
            .Property("cursor", "pointer")
            .Property("font-size", "13px")
            .Property("font-weight", "500")
            .EndSelector()
            .Selector(".btn-primary")
            .Property("background", "var(--accent-primary)")
            .Property("border-color", "var(--accent-primary)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".btn-success")
            .Property("background", "var(--accent-success)")
            .Property("border-color", "var(--accent-success)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".btn-warning")
            .Property("background", "var(--accent-warning)")
            .Property("border-color", "var(--accent-warning)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".btn-danger")
            .Property("background", "var(--accent-danger)")
            .Property("border-color", "var(--accent-danger)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".btn-outline")
            .Property("background", "transparent")
            .Property("border-color", "var(--border)")
            .EndSelector()
            .Selector(".input")
            .Property("background", "var(--bg-white)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("padding", "10px 12px")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "14px")
            .EndSelector()
            .Selector(".select")
            .Property("background", "var(--bg-white)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("padding", "10px 12px")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "14px")
            .EndSelector()
            .Selector(".card")
            .Property("background", "var(--bg-white)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "8px")
            .Property("padding", "16px")
            .EndSelector()
            .Selector(".stat-card")
            .Property("background", "var(--bg-white)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "8px")
            .Property("padding", "20px")
            .EndSelector()
            .Selector(".stat-value")
            .Property("font-size", "28px")
            .Property("font-weight", "600")
            .EndSelector()
            .Selector(".stat-label")
            .Property("font-size", "13px")
            .Property("color", "var(--text-secondary)")
            .Property("margin-top", "4px")
            .EndSelector()
            .Selector(".badge")
            .Property("display", "inline-block")
            .Property("padding", "4px 10px")
            .Property("border-radius", "12px")
            .Property("font-size", "12px")
            .Property("font-weight", "500")
            .EndSelector()
            .Selector(".badge-primary")
            .Property("background", "rgba(3, 102, 214, 0.1)")
            .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".badge-success")
            .Property("background", "rgba(40, 167, 69, 0.1)")
            .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".badge-warning")
            .Property("background", "rgba(249, 168, 37, 0.1)")
            .Property("color", "var(--accent-warning)")
            .EndSelector()
            .Selector(".badge-danger")
            .Property("background", "rgba(211, 47, 47, 0.1)")
            .Property("color", "var(--accent-danger)")
            .EndSelector()
            .Selector(".breadcrumb")
            .Property("font-size", "14px")
            .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".table")
            .Property("width", "100%")
            .Property("border-collapse", "collapse")
            .EndSelector()
            .Selector(".table th, .table td")
            .Property("padding", "12px")
            .Property("text-align", "left")
            .Property("border-bottom", "1px solid var(--border)")
            .Property("font-size", "13px")
            .EndSelector()
            .Selector(".table th")
            .Property("background", "var(--bg-primary)")
            .Property("font-weight", "600")
            .EndSelector()
            .Selector(".nav-item")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("gap", "10px")
            .Property("padding", "10px 16px")
            .Property("color", "rgba(255,255,255,0.7)")
            .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".sidebar")
            .Property("width", "200px")
            .Property("background", "var(--bg-sidebar)")
            .Property("color", "var(--text-inverse)")
            .Property("padding", "16px 0")
            .Property("border-radius", "8px")
            .EndSelector()
            .Selector(".switch")
            .Property("width", "44px")
            .Property("height", "24px")
            .Property("background", "var(--border)")
            .Property("border-radius", "12px")
            .Property("position", "relative")
            .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".progress")
            .Property("height", "8px")
            .Property("background", "var(--bg-primary)")
            .Property("border-radius", "4px")
            .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".progress-bar")
            .Property("height", "100%")
            .Property("background", "var(--accent-primary)")
            .Property("border-radius", "4px")
            .EndSelector()
            .Selector(".tab")
            .Property("padding", "12px 20px")
            .Property("border-bottom", "2px solid transparent")
            .Property("cursor", "pointer")
            .Property("font-size", "14px")
            .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".tag")
            .Property("display", "inline-flex")
            .Property("align-items", "center")
            .Property("gap", "4px")
            .Property("padding", "4px 8px")
            .Property("background", "var(--bg-primary)")
            .Property("border-radius", "4px")
            .Property("font-size", "12px")
            .EndSelector()
            .Selector(".dropdown-menu")
            .Property("position", "absolute")
            .Property("top", "100%")
            .Property("left", "0")
            .Property("background", "var(--bg-white)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("padding", "4px 0")
            .Property("min-width", "120px")
            .EndSelector()
            .Selector(".dropdown-item")
            .Property("padding", "8px 16px")
            .Property("font-size", "13px")
            .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".page-btn")
            .Property("width", "32px")
            .Property("height", "32px")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("cursor", "pointer")
            .Property("font-size", "13px")
            .EndSelector()
            .Selector(".checkbox-box")
            .Property("width", "18px")
            .Property("height", "18px")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "4px")
            .Property("background", "var(--bg-white)")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .EndSelector()
            .Selector(".avatar")
            .Property("width", "40px")
            .Property("height", "40px")
            .Property("border-radius", "50%")
            .Property("background", "var(--border)")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .EndSelector()
            .Selector(".bubble")
            .Property("background", "var(--bg-primary)")
            .Property("padding", "12px 16px")
            .Property("border-radius", "18px")
            .Property("font-size", "14px")
            .Property("max-width", "300px")
            .EndSelector()
            .Selector(".divider")
            .Property("height", "1px")
            .Property("background", "var(--border)")
            .Property("margin", "16px 0")
            .EndSelector();
    }
}
