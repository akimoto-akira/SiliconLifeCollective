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

public class CreativeSkin : ISkin
{
    public string Code => "creative";
    public string Name => "Creative";

    public SkinPreviewInfo PreviewInfo => new()
    {
        Icon = "\u270f\ufe0f",
        Description = "Warm artistic",
        BackgroundColor = "#fdf6e3",
        SecondaryBgColor = "#eee8d5",
        CardColor = "#fffef9",
        AccentColor = "#d4a574",
        TextColor = "#5c4b37",
        BorderColor = "#e8dfd0"
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
            .Property("font-family", "'Georgia', 'Songti SC', serif")
            .Property("background", "var(--bg-primary)")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "15px")
            .Property("padding", "24px")
            .EndSelector()
            .Selector(".container")
            .Property("max-width", "1200px")
            .Property("margin", "0 auto")
            .EndSelector()
            .Selector(".error")
            .Property("background", "var(--bg-card)")
            .Property("padding", "40px")
            .Property("border-radius", "16px")
            .Property("text-align", "center")
            .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".error h1")
            .Property("color", "var(--accent-primary)")
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
            .WithVariable("bg-primary", "#fdf6e3")
            .WithVariable("bg-card", "#fffef9")
            .WithVariable("bg-secondary", "#fffef9")
            .WithVariable("bg-tertiary", "#f5ebe0")
            .WithVariable("bg-sidebar", "#f5ebe0")
            .WithVariable("border", "#e8dfd0")
            .WithVariable("border-color", "#e8dfd0")
            .WithVariable("text-primary", "#5c4b37")
            .WithVariable("text-secondary", "#8b7355")
            .WithVariable("accent-primary", "#d4a574")
            .WithVariable("accent-color", "#d4a574")
            .WithVariable("accent-secondary", "#c9956c")
            .WithVariable("accent-warm", "#e8c9a0")
            .WithVariable("accent-success", "#8fbc8f")
            .WithVariable("accent-error", "#c0392b")
            .Selector(".btn")
            .Property("padding", "12px 24px")
            .Property("border-radius", "12px")
            .Property("border", "none")
            .Property("cursor", "pointer")
            .Property("font-size", "14px")
            .Property("font-weight", "500")
            .Property("transition", "all 0.2s")
            .Property("font-family", "inherit")
            .EndSelector()
            .Selector(".btn-primary")
            .Property("background", "var(--accent-primary)")
            .Property("color", "white")
            .Property("box-shadow", "0 2px 8px rgba(212,165,116,0.3)")
            .EndSelector()
            .Selector(".btn-primary:hover")
            .Property("transform", "translateY(-2px)")
            .Property("box-shadow", "0 4px 12px rgba(212,165,116,0.4)")
            .EndSelector()
            .Selector(".btn-secondary")
            .Property("background", "var(--bg-sidebar)")
            .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".btn-success")
            .Property("background", "var(--accent-success)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".btn-outline")
            .Property("background", "transparent")
            .Property("border", "2px solid var(--accent-primary)")
            .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".input")
            .Property("background", "var(--bg-primary)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "12px")
            .Property("padding", "14px 18px")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "15px")
            .Property("font-family", "inherit")
            .Property("transition", "all 0.2s")
            .EndSelector()
            .Selector(".input:focus")
            .Property("border-color", "var(--accent-primary)")
            .Property("box-shadow", "0 0 0 3px rgba(212,165,116,0.2)")
            .EndSelector()
            .Selector(".textarea")
            .Property("min-height", "100px")
            .Property("resize", "vertical")
            .Property("border-radius", "16px")
            .EndSelector()
            .Selector(".select")
            .Property("background", "var(--bg-primary)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "12px")
            .Property("padding", "14px 18px")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "15px")
            .Property("font-family", "inherit")
            .EndSelector()
            .Selector(".card")
            .Property("background", "var(--bg-card)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "16px")
            .Property("padding", "20px")
            .Property("box-shadow", "0 2px 12px rgba(0,0,0,0.04)")
            .EndSelector()
            .Selector(".card-header")
            .Property("font-size", "17px")
            .Property("font-weight", "600")
            .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".card-body")
            .Property("font-size", "14px")
            .Property("color", "var(--text-secondary)")
            .Property("line-height", "1.7")
            .EndSelector()
            .Selector(".card-elevated")
            .Property("background", "var(--bg-card)")
            .Property("border-radius", "20px")
            .Property("padding", "24px")
            .Property("box-shadow", "0 4px 20px rgba(0,0,0,0.06)")
            .EndSelector()
            .Selector(".badge")
            .Property("display", "inline-block")
            .Property("padding", "6px 14px")
            .Property("border-radius", "20px")
            .Property("font-size", "12px")
            .Property("font-weight", "500")
            .EndSelector()
            .Selector(".badge-primary")
            .Property("background", "var(--accent-warm)")
            .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".badge-success")
            .Property("background", "var(--accent-success)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".avatar")
            .Property("width", "48px")
            .Property("height", "48px")
            .Property("border-radius", "50%")
            .Property("background", "var(--bg-card)")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .Property("font-size", "22px")
            .Property("box-shadow", "0 2px 10px rgba(0,0,0,0.08)")
            .EndSelector()
            .Selector(".bubble")
            .Property("background", "var(--bg-card)")
            .Property("padding", "16px 20px")
            .Property("border-radius", "24px")
            .Property("font-size", "15px")
            .Property("max-width", "320px")
            .Property("line-height", "1.7")
            .Property("box-shadow", "0 2px 10px rgba(0,0,0,0.04)")
            .EndSelector()
            .Selector(".bubble.mine")
            .Property("background", "var(--accent-warm)")
            .EndSelector()
            .Selector(".switch")
            .Property("width", "52px")
            .Property("height", "28px")
            .Property("background", "var(--border)")
            .Property("border-radius", "14px")
            .Property("position", "relative")
            .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".progress")
            .Property("height", "10px")
            .Property("background", "var(--bg-sidebar)")
            .Property("border-radius", "5px")
            .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".progress-bar")
            .Property("height", "100%")
            .Property("background", "linear-gradient(90deg, var(--accent-primary), var(--accent-warm))")
            .Property("border-radius", "5px")
            .EndSelector()
            .Selector(".tab")
            .Property("padding", "12px 20px")
            .Property("border-bottom", "3px solid transparent")
            .Property("cursor", "pointer")
            .Property("font-size", "14px")
            .Property("color", "var(--text-secondary)")
            .Property("transition", "all 0.2s")
            .EndSelector()
            .Selector(".tag")
            .Property("display", "inline-flex")
            .Property("align-items", "center")
            .Property("gap", "4px")
            .Property("padding", "6px 12px")
            .Property("background", "var(--bg-sidebar)")
            .Property("border-radius", "20px")
            .Property("font-size", "12px")
            .EndSelector()
            .Selector(".list-item")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("gap", "14px")
            .Property("padding", "14px")
            .Property("border-radius", "14px")
            .Property("cursor", "pointer")
            .Property("transition", "all 0.2s")
            .EndSelector()
            .Selector(".list-item:hover")
            .Property("background", "rgba(212,165,116,0.15)")
            .EndSelector()
            .Selector(".inspiration-card")
            .Property("background", "linear-gradient(135deg, var(--bg-card) 0%, var(--bg-sidebar) 100%)")
            .Property("border-radius", "20px")
            .Property("padding", "24px")
            .EndSelector()
            .Selector(".inspiration-icon")
            .Property("font-size", "32px")
            .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".inspiration-text")
            .Property("font-style", "italic")
            .Property("font-size", "16px")
            .Property("line-height", "1.8")
            .EndSelector()
            .Selector(".quote")
            .Property("border-left", "4px solid var(--accent-primary)")
            .Property("padding-left", "16px")
            .Property("margin", "16px 0")
            .Property("font-style", "italic")
            .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".checkbox-box")
            .Property("width", "22px")
            .Property("height", "22px")
            .Property("border", "2px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("background", "var(--bg-card)")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .EndSelector()
            .Selector(".checkbox-box.checked")
            .Property("background", "var(--accent-primary)")
            .Property("border-color", "var(--accent-primary)")
            .Property("color", "white")
            .EndSelector();
    }
}
