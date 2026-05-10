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

namespace SiliconLife.App.Web.Component;

public class LoadingComponent : ComponentBase
{
    private string _text = "";
    private bool _active = true;

    public LoadingComponent() { }

    public LoadingComponent(string text)
    {
        _text = text;
    }

    public new LoadingComponent Id(string id)
    {
        base.Id = id;
        return this;
    }

    public new LoadingComponent Class(string className)
    {
        base.Class = string.IsNullOrEmpty(base.Class) ? className : $"{base.Class} {className}";
        return this;
    }

    public LoadingComponent Text(string text)
    {
        _text = text;
        return this;
    }

    public LoadingComponent Active(bool active)
    {
        _active = active;
        return this;
    }

    public override H ToH()
    {
        var cssClass = "loading-indicator";
        if (_active) cssClass += " loading-indicator-active";
        if (!string.IsNullOrEmpty(base.Class)) cssClass += $" {base.Class}";

        var el = H.Div(
            H.Div("").Class("loading-spinner"),
            H.Div(_text).Class("loading-text")
        ).Class(cssClass);

        if (!string.IsNullOrEmpty(base.Id)) el.Id(base.Id);
        if (base.Style != null && base.Style.HasInlineStyles) el.Style(base.Style);
        foreach (var kvp in Attributes) el.Attr(kvp.Key, kvp.Value);

        return el;
    }

    public override string Render() => ToH().Build();

    public static CssBuilder AddStyles(CssBuilder css)
    {
        return css
            .Selector(".loading-indicator")
                .Property("display", "none")
                .Property("flex-direction", "column")
                .Property("align-items", "center")
                .Property("justify-content", "center")
                .Property("padding", "40px 20px")
                .Property("color", "var(--text-secondary)")
                .Property("gap", "16px")
            .EndSelector()
            .Selector(".loading-indicator-active")
                .Property("display", "flex")
            .EndSelector()
            .Selector(".loading-spinner")
                .Property("width", "40px")
                .Property("height", "40px")
                .Property("border", "3px solid var(--border)")
                .Property("border-top-color", "var(--accent-primary)")
                .Property("border-radius", "50%")
                .Property("animation", "loading-spin 1s linear infinite")
            .EndSelector()
            .Keyframes("loading-spin", kf => kf
                .At("0%", p => p.Property("transform", "rotate(0deg)"))
                .At("100%", p => p.Property("transform", "rotate(360deg)")))
            .Selector(".loading-text")
                .Property("font-size", "14px")
                .Property("font-weight", "500")
            .EndSelector();
    }
}
