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

using WebJs = SiliconLife.App.Web.Js;

namespace SiliconLife.App.Web.Component;

/// <summary>
/// Select dropdown component with optional searchable/filterable mode
/// </summary>
public class SelectComponent : ComponentBase
{
    private readonly List<(string Value, string Text, bool Disabled)> _options = new();
    private string _name = "";
    private string? _selectedValue;
    private bool _multiple = false;
    private bool _required = false;
    private bool _searchable = false;
    private string _placeholder = "";
    private string _noResultText = "No results found";
    private string _hint = "";

    public new SelectComponent Id(string id)
    {
        base.Id = id;
        return this;
    }

    public new SelectComponent Class(string className)
    {
        base.Class = string.IsNullOrEmpty(base.Class) ? className : $"{base.Class} {className}";
        return this;
    }

    public new SelectComponent Style(CssBuilder style)
    {
        if (base.Style == null) base.Style = style; else base.Style.MergeInlineFrom(style);
        return this;
    }

    public new SelectComponent Attr(string name, string value)
    {
        base.Attributes[name] = value;
        return this;
    }

    public SelectComponent Name(string name)
    {
        _name = name;
        return this;
    }

    public SelectComponent AddOption(string value, string text, bool disabled = false)
    {
        _options.Add((value, text, disabled));
        return this;
    }

    public SelectComponent AddOptions(IEnumerable<(string Value, string Text)> options)
    {
        foreach (var (value, text) in options)
        {
            _options.Add((value, text, false));
        }
        return this;
    }

    public SelectComponent Selected(string value)
    {
        _selectedValue = value;
        return this;
    }

    public SelectComponent Multiple(bool multiple = true)
    {
        _multiple = multiple;
        return this;
    }

    public SelectComponent Required(bool required = true)
    {
        _required = required;
        return this;
    }

    /// <summary>
    /// Enable searchable/filterable mode (not compatible with multiple selection)
    /// </summary>
    public SelectComponent Searchable(bool searchable = true)
    {
        _searchable = searchable;
        return this;
    }

    /// <summary>
    /// Set placeholder text for the search input
    /// </summary>
    public SelectComponent Placeholder(string placeholder)
    {
        _placeholder = placeholder;
        return this;
    }

    /// <summary>
    /// Set text displayed when no options match the search query
    /// </summary>
    public SelectComponent NoResultText(string text)
    {
        _noResultText = text;
        return this;
    }

    public SelectComponent Hint(string hint)
    {
        _hint = hint;
        return this;
    }

    public override H Render()
    {
        if (_searchable && !_multiple)
        {
            return RenderSearchable();
        }

        return RenderStandard();
    }

    private H RenderStandard()
    {
        var select = H.Select();

        if (!string.IsNullOrEmpty(_name))
            select.Attr("name", _name);

        if (!string.IsNullOrEmpty(base.Id))
            select.Attr("id", base.Id);

        var classes = new List<string>();
        if (!string.IsNullOrEmpty(base.Class))
            classes.Add(base.Class);

        if (classes.Count > 0)
            select.Class(string.Join(" ", classes));

        if (base.Style != null && base.Style.HasInlineStyles)
            select.Attr("style", base.Style.BuildInline());

        if (_multiple)
            select.Attr("multiple", "multiple");

        if (_required)
            select.Attr("required", "required");

        foreach (var kvp in Attributes)
        {
            select.Attr(kvp.Key, kvp.Value);
        }

        foreach (var (value, text, disabled) in _options)
        {
            var option = H.Option()
                .Attr("value", H.Escape(value))
                .Text(H.Escape(text));

            if (value == _selectedValue)
                option.Attr("selected", "selected");

            if (disabled)
                option.Attr("disabled", "disabled");

            select.Add(option);
        }

        return select;
    }

    private H RenderSearchable()
    {
        var baseId = string.IsNullOrEmpty(base.Id)
            ? "sls-" + Guid.NewGuid().ToString("N")[..8]
            : base.Id;
        base.Id = baseId;

        var wrapperId = baseId + "-wrapper";
        var searchInputId = baseId + "-search";
        var dropdownId = baseId + "-dropdown";

        var selectedText = "";
        var effectiveSelected = _selectedValue;
        if (string.IsNullOrEmpty(effectiveSelected))
        {
            foreach (var (value, _, disabled) in _options)
            {
                if (!disabled)
                {
                    effectiveSelected = value;
                    break;
                }
            }
        }
        foreach (var (value, text, _) in _options)
        {
            if (value == effectiveSelected)
            {
                selectedText = text;
                break;
            }
        }

        var wrapperClass = "sl-select-search";
        if (!string.IsNullOrEmpty(base.Class))
            wrapperClass += " " + base.Class;

        var wrapper = H.Div().Class(wrapperClass).Id(wrapperId);
        if (base.Style != null && base.Style.HasInlineStyles)
            wrapper.Attr("style", base.Style.BuildInline());

        var hiddenInput = H.Input().Attr("type", "hidden").Id(baseId).Value(effectiveSelected ?? "");
        if (!string.IsNullOrEmpty(_name))
            hiddenInput.Attr("name", _name);
        if (_required)
            hiddenInput.Attr("required", "required");
        foreach (var kvp in Attributes)
            hiddenInput.Attr(kvp.Key, kvp.Value);
        wrapper.Add(hiddenInput);

        var control = H.Div().Class("sl-select-search-control");
        control.Add(H.Input()
            .Attr("type", "text")
            .Class("sl-select-search-input")
            .Id(searchInputId)
            .Placeholder(_placeholder)
            .Value(selectedText)
            .Attr("autocomplete", "off"));
        control.Add(H.Span("&#9662;").Class("sl-select-search-arrow"));
        wrapper.Add(control);

        var dropdown = H.Div().Class("sl-select-search-dropdown").Id(dropdownId);
        foreach (var (value, text, disabled) in _options)
        {
            var optionDiv = H.Div(H.Escape(text))
                .Class("sl-select-search-option" + (value == effectiveSelected ? " selected" : ""))
                .Data("value", value);
            if (disabled)
                optionDiv.Attr("disabled", "disabled");
            dropdown.Add(optionDiv);
        }
        dropdown.Add(H.Div(H.Escape(_noResultText)).Class("sl-select-search-no-result").Attr("style", "display:none"));
        if (!string.IsNullOrEmpty(_hint))
        {
            dropdown.Add(H.Div(H.Escape(_hint)).Class("sl-select-search-hint"));
        }
        wrapper.Add(dropdown);

        var css = GetSearchableCss().Build();
        var js = GetSearchableJs(baseId, wrapperId, searchInputId, dropdownId);
        wrapper.Add(H.Style(css));
        wrapper.Add(H.Script(js));

        return wrapper;
    }

    public static H GetSearchableGlobalScript()
    {
        var js = WebJs.Block()
            .Add(() => WebJs.Assign(
                () => WebJs.Id(() => "window").Prop(() => "slSelectSearch_create"),
                () => WebJs.Arrow(() => new List<string> { "parentEl", "cfg" }, () => WebJs.Block()
                    .Add(() => WebJs.Let(() => "w", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "div"))))
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "w").Prop(() => "className"), () => WebJs.Str(() => "sl-select-search").Op(() => "+", () => WebJs.Ternary(() => WebJs.Id(() => "cfg").Prop(() => "className"), () => WebJs.Str(() => " ").Op(() => "+", () => WebJs.Id(() => "cfg").Prop(() => "className")), () => WebJs.Str(() => "")))).Stmt())
                    .Add(() => WebJs.Let(() => "h", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "input"))))
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "h").Prop(() => "type"), () => WebJs.Str(() => "hidden")).Stmt())
                    .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (WebJs.Id(() => "cfg").Prop(() => "name"), new List<JsSyntax>
                        {
                            WebJs.Assign(() => WebJs.Id(() => "h").Prop(() => "name"), () => WebJs.Id(() => "cfg").Prop(() => "name")).Stmt()
                        })
                    }))
                    .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (WebJs.Id(() => "cfg").Prop(() => "id"), new List<JsSyntax>
                        {
                            WebJs.Assign(() => WebJs.Id(() => "h").Prop(() => "id"), () => WebJs.Id(() => "cfg").Prop(() => "id")).Stmt()
                        })
                    }))
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "h").Prop(() => "value"), () => WebJs.Id(() => "cfg").Prop(() => "value").Op(() => "||", () => WebJs.Str(() => ""))).Stmt())
                    .Add(() => WebJs.Let(() => "ctrl", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "div"))))
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "ctrl").Prop(() => "className"), () => WebJs.Str(() => "sl-select-search-control")).Stmt())
                    .Add(() => WebJs.Let(() => "s", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "input"))))
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "type"), () => WebJs.Str(() => "text")).Stmt())
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "className"), () => WebJs.Str(() => "sl-select-search-input")).Stmt())
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "placeholder"), () => WebJs.Id(() => "cfg").Prop(() => "placeholder").Op(() => "||", () => WebJs.Str(() => ""))).Stmt())
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "autocomplete"), () => WebJs.Str(() => "off")).Stmt())
                    .Add(() => WebJs.Let(() => "d", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "div"))))
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "d").Prop(() => "className"), () => WebJs.Str(() => "sl-select-search-dropdown")).Stmt())
                    .Add(() => WebJs.Let(() => "nr", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "div"))))
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "nr").Prop(() => "className"), () => WebJs.Str(() => "sl-select-search-no-result")).Stmt())
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "nr").Prop(() => "style").Prop(() => "display"), () => WebJs.Str(() => "none")).Stmt())
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "nr").Prop(() => "textContent"), () => WebJs.Id(() => "cfg").Prop(() => "noResultText").Op(() => "||", () => WebJs.Str(() => "No results found"))).Stmt())
                    .Add(() => WebJs.Let(() => "selText", () => WebJs.Str(() => "")))
                    .Add(() => WebJs.Let(() => "effectiveVal", () => WebJs.Id(() => "cfg").Prop(() => "value").Op(() => "||", () => WebJs.Str(() => ""))))
                    .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (WebJs.Id(() => "cfg").Prop(() => "options"), new List<JsSyntax>
                        {
                            WebJs.Let(() => "keys", () => WebJs.Id(() => "Object").Call(() => "keys", () => WebJs.Id(() => "cfg").Prop(() => "options"))),
                            WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                            {
                                (WebJs.Id(() => "effectiveVal").Not().Op(() => "&&", () => WebJs.Id(() => "keys").Prop(() => "length")), new List<JsSyntax>
                                {
                                    WebJs.Assign(() => WebJs.Id(() => "effectiveVal"), () => WebJs.Id(() => "keys").Index(() => WebJs.Num(() => "0"))).Stmt()
                                })
                            }),
                            WebJs.Assign(() => WebJs.Id(() => "h").Prop(() => "value"), () => WebJs.Id(() => "effectiveVal")).Stmt(),
                            WebJs.For(
                                () => WebJs.Let(() => "i", () => WebJs.Num(() => "0")),
                                () => WebJs.Id(() => "i").Op(() => "<", () => WebJs.Id(() => "keys").Prop(() => "length")),
                                () => WebJs.Assign(() => WebJs.Id(() => "i"), () => WebJs.Id(() => "i").Op(() => "+", () => WebJs.Num(() => "1"))),
                                () => WebJs.Block()
                                    .Add(() => WebJs.Let(() => "o", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "div"))))
                                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "o").Prop(() => "className"), () => WebJs.Str(() => "sl-select-search-option")).Stmt())
                                    .Add(() => WebJs.Id(() => "o").Call(() => "setAttribute", () => WebJs.Str(() => "data-value"), () => WebJs.Id(() => "keys").Index(() => WebJs.Id(() => "i"))).Stmt())
                                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "o").Prop(() => "textContent"), () => WebJs.Id(() => "cfg").Prop(() => "options").Index(() => WebJs.Id(() => "keys").Index(() => WebJs.Id(() => "i")))).Stmt())
                                    .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                                    {
                                        (WebJs.Id(() => "keys").Index(() => WebJs.Id(() => "i")).Op(() => "===", () => WebJs.Id(() => "effectiveVal")), new List<JsSyntax>
                                        {
                                            WebJs.Id(() => "o").Prop(() => "classList").Call(() => "add", () => WebJs.Str(() => "selected")).Stmt(),
                                            WebJs.Assign(() => WebJs.Id(() => "selText"), () => WebJs.Id(() => "cfg").Prop(() => "options").Index(() => WebJs.Id(() => "keys").Index(() => WebJs.Id(() => "i")))).Stmt()
                                        })
                                    }))
                                    .Add(() => WebJs.Id(() => "d").Call(() => "appendChild", () => WebJs.Id(() => "o")).Stmt()))
                        })
                    }))
                    .Add(() => WebJs.Id(() => "d").Call(() => "appendChild", () => WebJs.Id(() => "nr")).Stmt())
                    .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (WebJs.Id(() => "cfg").Prop(() => "hint"), new List<JsSyntax>
                        {
                            WebJs.Let(() => "ht", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "div"))),
                            WebJs.Assign(() => WebJs.Id(() => "ht").Prop(() => "className"), () => WebJs.Str(() => "sl-select-search-hint")).Stmt(),
                            WebJs.Assign(() => WebJs.Id(() => "ht").Prop(() => "textContent"), () => WebJs.Id(() => "cfg").Prop(() => "hint")).Stmt(),
                            WebJs.Id(() => "d").Call(() => "appendChild", () => WebJs.Id(() => "ht")).Stmt()
                        })
                    }))
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "value"), () => WebJs.Id(() => "selText")).Stmt())
                    .Add(() => WebJs.Let(() => "arrow", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "span"))))
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "arrow").Prop(() => "className"), () => WebJs.Str(() => "sl-select-search-arrow")).Stmt())
                    .Add(() => WebJs.Assign(() => WebJs.Id(() => "arrow").Prop(() => "innerHTML"), () => WebJs.Str(() => "&#9662;")).Stmt())
                    .Add(() => WebJs.Id(() => "ctrl").Call(() => "appendChild", () => WebJs.Id(() => "s")).Stmt())
                    .Add(() => WebJs.Id(() => "ctrl").Call(() => "appendChild", () => WebJs.Id(() => "arrow")).Stmt())
                    .Add(() => WebJs.Id(() => "w").Call(() => "appendChild", () => WebJs.Id(() => "h")).Stmt())
                    .Add(() => WebJs.Id(() => "w").Call(() => "appendChild", () => WebJs.Id(() => "ctrl")).Stmt())
                    .Add(() => WebJs.Id(() => "w").Call(() => "appendChild", () => WebJs.Id(() => "d")).Stmt())
                    .Add(() => WebJs.Id(() => "parentEl").Call(() => "appendChild", () => WebJs.Id(() => "w")).Stmt())
                    .Add(() => WebJs.Let(() => "isOpen", () => WebJs.Bool(() => false)))
                    .Add(() => WebJs.Let(() => "hi", () => WebJs.Num(() => "-1")))
                    .Add(() => WebJs.Func(() => "getSelText", () => new List<string>(), () => WebJs.Block()
                        .Add(() => WebJs.Let(() => "all", () => WebJs.Id(() => "d").Call(() => "querySelectorAll", () => WebJs.Str(() => ".sl-select-search-option"))))
                        .Add(() => WebJs.For(
                            () => WebJs.Let(() => "i", () => WebJs.Num(() => "0")),
                            () => WebJs.Id(() => "i").Op(() => "<", () => WebJs.Id(() => "all").Prop(() => "length")),
                            () => WebJs.Assign(() => WebJs.Id(() => "i"), () => WebJs.Id(() => "i").Op(() => "+", () => WebJs.Num(() => "1"))),
                            () => WebJs.Block()
                                .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                                {
                                    (WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Call(() => "getAttribute", () => WebJs.Str(() => "data-value")).Op(() => "===", () => WebJs.Id(() => "h").Prop(() => "value")), new List<JsSyntax>
                                    {
                                        WebJs.Return(() => WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Prop(() => "textContent"))
                                    })
                                }))))
                        .Add(() => WebJs.Return(() => WebJs.Str(() => "")))))
                    .Add(() => WebJs.Func(() => "filter", () => new List<string>(), () => WebJs.Block()
                        .Add(() => WebJs.Let(() => "q", () => WebJs.Id(() => "s").Prop(() => "value").Call(() => "toLowerCase")))
                        .Add(() => WebJs.Let(() => "c", () => WebJs.Num(() => "0")))
                        .Add(() => WebJs.Let(() => "all", () => WebJs.Id(() => "d").Call(() => "querySelectorAll", () => WebJs.Str(() => ".sl-select-search-option"))))
                        .Add(() => WebJs.For(
                            () => WebJs.Let(() => "i", () => WebJs.Num(() => "0")),
                            () => WebJs.Id(() => "i").Op(() => "<", () => WebJs.Id(() => "all").Prop(() => "length")),
                            () => WebJs.Assign(() => WebJs.Id(() => "i"), () => WebJs.Id(() => "i").Op(() => "+", () => WebJs.Num(() => "1"))),
                            () => WebJs.Block()
                                .Add(() => WebJs.Let(() => "m", () => WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Prop(() => "textContent").Call(() => "toLowerCase").Call(() => "indexOf", () => WebJs.Id(() => "q")).Op(() => "!==", () => WebJs.Num(() => "-1"))))
                                .Add(() => WebJs.Assign(() => WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Prop(() => "style").Prop(() => "display"), () => WebJs.Ternary(() => WebJs.Id(() => "m"), () => WebJs.Str(() => ""), () => WebJs.Str(() => "none"))).Stmt())
                                .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                                {
                                    (WebJs.Id(() => "m"), new List<JsSyntax>
                                    {
                                        WebJs.Assign(() => WebJs.Id(() => "c"), () => WebJs.Id(() => "c").Op(() => "+", () => WebJs.Num(() => "1"))).Stmt()
                                    })
                                }))))
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "nr").Prop(() => "style").Prop(() => "display"), () => WebJs.Ternary(() => WebJs.Id(() => "c"), () => WebJs.Str(() => "none"), () => WebJs.Str(() => ""))).Stmt())
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "hi"), () => WebJs.Num(() => "-1")))
                        .Add(() => WebJs.Id(() => "clrHi").Invoke().Stmt())))
                    .Add(() => WebJs.Func(() => "clrHi", () => new List<string>(), () => WebJs.Block()
                        .Add(() => WebJs.Let(() => "hl", () => WebJs.Id(() => "d").Call(() => "querySelectorAll", () => WebJs.Str(() => ".sl-select-search-option.highlighted"))))
                        .Add(() => WebJs.For(
                            () => WebJs.Let(() => "i", () => WebJs.Num(() => "0")),
                            () => WebJs.Id(() => "i").Op(() => "<", () => WebJs.Id(() => "hl").Prop(() => "length")),
                            () => WebJs.Assign(() => WebJs.Id(() => "i"), () => WebJs.Id(() => "i").Op(() => "+", () => WebJs.Num(() => "1"))),
                            () => WebJs.Id(() => "hl").Index(() => WebJs.Id(() => "i")).Prop(() => "classList").Call(() => "remove", () => WebJs.Str(() => "highlighted")).Stmt()))))
                    .Add(() => WebJs.Func(() => "setHi", () => new List<string> { "idx" }, () => WebJs.Block()
                        .Add(() => WebJs.Id(() => "clrHi").Invoke().Stmt())
                        .Add(() => WebJs.Let(() => "vis", () => WebJs.New(() => WebJs.Id(() => "Array"))))
                        .Add(() => WebJs.Let(() => "all", () => WebJs.Id(() => "d").Call(() => "querySelectorAll", () => WebJs.Str(() => ".sl-select-search-option"))))
                        .Add(() => WebJs.For(
                            () => WebJs.Let(() => "i", () => WebJs.Num(() => "0")),
                            () => WebJs.Id(() => "i").Op(() => "<", () => WebJs.Id(() => "all").Prop(() => "length")),
                            () => WebJs.Assign(() => WebJs.Id(() => "i"), () => WebJs.Id(() => "i").Op(() => "+", () => WebJs.Num(() => "1"))),
                            () => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                            {
                                (WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Prop(() => "style").Prop(() => "display").Op(() => "!==", () => WebJs.Str(() => "none")).Op(() => "&&", () => WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Call(() => "hasAttribute", () => WebJs.Str(() => "disabled")).Not()), new List<JsSyntax>
                                {
                                    WebJs.Id(() => "vis").Call(() => "push", () => WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i"))).Stmt()
                                })
                            })))
                        .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (WebJs.Id(() => "vis").Prop(() => "length").Not(), new List<JsSyntax>
                            {
                                WebJs.Return(() => WebJs.Str(() => ""))
                            })
                        }))
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "idx"), () => WebJs.Id(() => "idx").Op(() => "%", () => WebJs.Id(() => "vis").Prop(() => "length")).Paren().Op(() => "+", () => WebJs.Id(() => "vis").Prop(() => "length")).Paren().Op(() => "%", () => WebJs.Id(() => "vis").Prop(() => "length"))).Stmt())
                        .Add(() => WebJs.Id(() => "vis").Index(() => WebJs.Id(() => "idx")).Prop(() => "classList").Call(() => "add", () => WebJs.Str(() => "highlighted")).Stmt())
                        .Add(() => WebJs.Id(() => "vis").Index(() => WebJs.Id(() => "idx")).Call(() => "scrollIntoView", () => WebJs.Obj().Prop(() => "block", () => WebJs.Str(() => "nearest"))).Stmt())
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "hi"), () => WebJs.Id(() => "idx")).Stmt())))
                    .Add(() => WebJs.Func(() => "sel", () => new List<string> { "o" }, () => WebJs.Block()
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "h").Prop(() => "value"), () => WebJs.Id(() => "o").Call(() => "getAttribute", () => WebJs.Str(() => "data-value"))).Stmt())
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "value"), () => WebJs.Id(() => "o").Prop(() => "textContent")).Stmt())
                        .Add(() => WebJs.Id(() => "close").Invoke().Stmt())
                        .Add(() => WebJs.Id(() => "h").Call(() => "dispatchEvent", () => WebJs.New(() => WebJs.Id(() => "Event"), () => WebJs.Str(() => "change"))).Stmt())
                        .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (WebJs.Id(() => "cfg").Prop(() => "onchange"), new List<JsSyntax>
                            {
                                WebJs.Id(() => "cfg").Prop(() => "onchange").Invoke(() => WebJs.Id(() => "h").Prop(() => "value")).Stmt()
                            })
                        }))))
                    .Add(() => WebJs.Func(() => "open_", () => new List<string>(), () => WebJs.Block()
                        .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (WebJs.Id(() => "isOpen"), new List<JsSyntax>
                            {
                                WebJs.Return(() => WebJs.Str(() => ""))
                            })
                        }))
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "isOpen"), () => WebJs.Bool(() => true)))
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "d").Prop(() => "style").Prop(() => "display"), () => WebJs.Str(() => "block")).Stmt())
                        .Add(() => WebJs.Id(() => "w").Prop(() => "classList").Call(() => "add", () => WebJs.Str(() => "open")).Stmt())
                        .Add(() => WebJs.Id(() => "s").Call(() => "select").Stmt())
                        .Add(() => WebJs.Id(() => "filter").Invoke().Stmt())))
                    .Add(() => WebJs.Func(() => "close", () => new List<string>(), () => WebJs.Block()
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "isOpen"), () => WebJs.Bool(() => false)))
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "d").Prop(() => "style").Prop(() => "display"), () => WebJs.Str(() => "none")).Stmt())
                        .Add(() => WebJs.Id(() => "w").Prop(() => "classList").Call(() => "remove", () => WebJs.Str(() => "open")).Stmt())
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "hi"), () => WebJs.Num(() => "-1")))
                        .Add(() => WebJs.Id(() => "clrHi").Invoke().Stmt())
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "value"), () => WebJs.Id(() => "getSelText").Invoke()).Stmt())))
                    .Add(() => WebJs.Id(() => "s").Call(() => "addEventListener", () => WebJs.Str(() => "focus"), () => WebJs.Id(() => "open_")).Stmt())
                    .Add(() => WebJs.Id(() => "s").Call(() => "addEventListener", () => WebJs.Str(() => "input"), () => WebJs.Id(() => "filter")).Stmt())
                    .Add(() => WebJs.Id(() => "s").Call(() => "addEventListener", () => WebJs.Str(() => "keydown"), () => WebJs.Arrow(() => new List<string> { "e" }, () => WebJs.Block()
                        .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (WebJs.Id(() => "e").Prop(() => "key").Op(() => "===", () => WebJs.Str(() => "ArrowDown")), new List<JsSyntax>
                            {
                                WebJs.Id(() => "e").Call(() => "preventDefault").Stmt(),
                                WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                                {
                                    (WebJs.Id(() => "isOpen").Not(), new List<JsSyntax>
                                    {
                                        WebJs.Id(() => "open_").Invoke().Stmt()
                                    })
                                }),
                                WebJs.Id(() => "setHi").Invoke(() => WebJs.Id(() => "hi").Op(() => "+", () => WebJs.Num(() => "1"))).Stmt()
                            }),
                            (WebJs.Id(() => "e").Prop(() => "key").Op(() => "===", () => WebJs.Str(() => "ArrowUp")), new List<JsSyntax>
                            {
                                WebJs.Id(() => "e").Call(() => "preventDefault").Stmt(),
                                WebJs.Id(() => "setHi").Invoke(() => WebJs.Id(() => "hi").Op(() => "-", () => WebJs.Num(() => "1"))).Stmt()
                            }),
                            (WebJs.Id(() => "e").Prop(() => "key").Op(() => "===", () => WebJs.Str(() => "Enter")), new List<JsSyntax>
                            {
                                WebJs.Id(() => "e").Call(() => "preventDefault").Stmt(),
                                WebJs.Let(() => "hl", () => WebJs.Id(() => "d").Call(() => "querySelector", () => WebJs.Str(() => ".sl-select-search-option.highlighted"))),
                                WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                                {
                                    (WebJs.Id(() => "hl"), new List<JsSyntax>
                                    {
                                        WebJs.Id(() => "sel").Invoke(() => WebJs.Id(() => "hl")).Stmt()
                                    })
                                })
                            }),
                            (WebJs.Id(() => "e").Prop(() => "key").Op(() => "===", () => WebJs.Str(() => "Escape")), new List<JsSyntax>
                            {
                                WebJs.Id(() => "close").Invoke().Stmt()
                            })
                        })))).Stmt())
                    .Add(() => WebJs.Id(() => "d").Call(() => "addEventListener", () => WebJs.Str(() => "mousedown"), () => WebJs.Arrow(() => new List<string> { "e" }, () => WebJs.Block()
                        .Add(() => WebJs.Let(() => "o", () => WebJs.Id(() => "e").Prop(() => "target").Call(() => "closest", () => WebJs.Str(() => ".sl-select-search-option"))))
                        .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (WebJs.Id(() => "o").Op(() => "&&", () => WebJs.Id(() => "o").Call(() => "hasAttribute", () => WebJs.Str(() => "disabled")).Not()), new List<JsSyntax>
                            {
                                WebJs.Id(() => "e").Call(() => "preventDefault").Stmt(),
                                WebJs.Id(() => "sel").Invoke(() => WebJs.Id(() => "o")).Stmt()
                            })
                        })))).Stmt())
                    .Add(() => WebJs.Id(() => "arrow").Call(() => "addEventListener", () => WebJs.Str(() => "click"), () => WebJs.Arrow(() => new List<string> { "e" }, () => WebJs.Block()
                        .Add(() => WebJs.Id(() => "e").Call(() => "stopPropagation").Stmt())
                        .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (WebJs.Id(() => "isOpen"), new List<JsSyntax>
                            {
                                WebJs.Id(() => "close").Invoke().Stmt()
                            }),
                            (null, new List<JsSyntax>
                            {
                                WebJs.Id(() => "open_").Invoke().Stmt()
                            })
                        })))).Stmt())
                    .Add(() => WebJs.Id(() => "document").Call(() => "addEventListener", () => WebJs.Str(() => "click"), () => WebJs.Arrow(() => new List<string> { "e" }, () => WebJs.Block()
                        .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (WebJs.Id(() => "w").Call(() => "contains", () => WebJs.Id(() => "e").Prop(() => "target")).Not(), new List<JsSyntax>
                            {
                                WebJs.Id(() => "close").Invoke().Stmt()
                            })
                        })))).Stmt())
                    .Add(() => WebJs.Return(() => WebJs.Id(() => "w"))))));

        return H.Div(
            H.Style(GetSearchableCss()),
            H.Script(js)
        );
    }

    public static CssBuilder GetSearchableCss()
    {
        return CssBuilder.Create()
            .Selector(".sl-select-search")
                .Property("position", "relative")
                .Property("display", "inline-block")
                .Property("width", "100%")
                .Property("box-sizing", "border-box")
            .EndSelector()
            .Selector(".sl-select-search *")
                .Property("box-sizing", "border-box")
            .EndSelector()
            .Selector(".sl-select-search-control")
                .Property("position", "relative")
                .Property("display", "flex")
                .Property("align-items", "center")
            .EndSelector()
            .Selector(".sl-select-search-input")
                .Property("width", "100%")
                .Property("padding", "8px 30px 8px 12px")
                .Property("border", "1px solid var(--border-color,#ccc)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card,#fff)")
                .Property("color", "var(--text-primary,#333)")
                .Property("font-size", "14px")
                .Property("outline", "none")
                .Property("transition", "border-color .2s")
            .EndSelector()
            .Selector(".sl-select-search-input:focus")
                .Property("border-color", "var(--accent-primary,#4a90d9)")
            .EndSelector()
            .Selector(".sl-select-search-arrow")
                .Property("position", "absolute")
                .Property("right", "10px")
                .Property("pointer-events", "none")
                .Property("color", "var(--text-secondary,#999)")
                .Property("font-size", "12px")
                .Property("transition", "transform .2s")
            .EndSelector()
            .Selector(".sl-select-search.open .sl-select-search-arrow")
                .Property("transform", "rotate(180deg)")
            .EndSelector()
            .Selector(".sl-select-search-dropdown")
                .Property("display", "none")
                .Property("position", "absolute")
                .Property("top", "100%")
                .Property("left", "0")
                .Property("right", "0")
                .Property("max-height", "250px")
                .Property("overflow-y", "auto")
                .Property("background", "var(--bg-card,#fff)")
                .Property("border", "1px solid var(--border-color,#ccc)")
                .Property("border-radius", "6px")
                .Property("margin-top", "4px")
                .Property("z-index", "1000")
                .Property("box-shadow", "0 4px 12px rgba(0,0,0,.15)")
            .EndSelector()
            .Selector(".sl-select-search-option")
                .Property("padding", "8px 12px")
                .Property("cursor", "pointer")
                .Property("color", "var(--text-primary,#333)")
                .Property("font-size", "14px")
                .Property("transition", "background .15s")
            .EndSelector()
            .Selector(".sl-select-search-option:hover,.sl-select-search-option.highlighted")
                .Property("background", "var(--bg-hover,#f0f0f0)")
            .EndSelector()
            .Selector(".sl-select-search-option.selected")
                .Property("font-weight", "600")
            .EndSelector()
            .Selector(".sl-select-search-option[disabled]")
                .Property("opacity", ".5")
                .Property("cursor", "not-allowed")
                .Property("pointer-events", "none")
            .EndSelector()
            .Selector(".sl-select-search-no-result")
                .Property("padding", "8px 12px")
                .Property("color", "var(--text-secondary,#999)")
                .Property("font-size", "14px")
                .Property("text-align", "center")
            .EndSelector()
            .Selector(".sl-select-search-hint")
                .Property("padding", "6px 12px")
                .Property("color", "var(--text-secondary,#999)")
                .Property("font-size", "12px")
                .Property("text-align", "center")
                .Property("border-top", "1px solid var(--border-color,rgba(0,0,0,.1))")
                .Property("background", "var(--bg-secondary,rgba(0,0,0,.02))")
            .EndSelector()
            .Selector(".sl-select-search-dropdown::-webkit-scrollbar")
                .Property("width", "6px")
            .EndSelector()
            .Selector(".sl-select-search-dropdown::-webkit-scrollbar-track")
                .Property("background", "var(--bg-secondary,rgba(0,0,0,.05))")
            .EndSelector()
            .Selector(".sl-select-search-dropdown::-webkit-scrollbar-thumb")
                .Property("background", "var(--bg-hover,rgba(0,0,0,.15))")
                .Property("border-radius", "3px")
            .EndSelector();
    }

    private static string GetSearchableJs(string baseId, string wrapperId, string searchInputId, string dropdownId)
    {
        var body = WebJs.Block()
            .Add(() => WebJs.Let(() => "w", () => WebJs.Id(() => "document").Call(() => "getElementById", () => WebJs.Str(() => wrapperId))))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "w").Not(), new List<JsSyntax>
                {
                    WebJs.Return(() => WebJs.Str(() => ""))
                })
            }))
            .Add(() => WebJs.Let(() => "h", () => WebJs.Id(() => "w").Call(() => "querySelector", () => WebJs.Str(() => "input[type=hidden]"))))
            .Add(() => WebJs.Let(() => "s", () => WebJs.Id(() => "document").Call(() => "getElementById", () => WebJs.Str(() => searchInputId))))
            .Add(() => WebJs.Let(() => "d", () => WebJs.Id(() => "document").Call(() => "getElementById", () => WebJs.Str(() => dropdownId))))
            .Add(() => WebJs.Let(() => "nr", () => WebJs.Id(() => "d").Call(() => "querySelector", () => WebJs.Str(() => ".sl-select-search-no-result"))))
            .Add(() => WebJs.Const(() => "opts", () => WebJs.Arrow(() => new List<string>(), () => WebJs.Id(() => "d").Call(() => "querySelectorAll", () => WebJs.Str(() => ".sl-select-search-option")))))
            .Add(() => WebJs.Let(() => "isOpen", () => WebJs.Bool(() => false)))
            .Add(() => WebJs.Let(() => "hi", () => WebJs.Num(() => "-1")))

            .Add(() => WebJs.Func(() => "getSelText", () => new List<string>(), () => WebJs.Block()
                .Add(() => WebJs.Let(() => "all", () => WebJs.Id(() => "opts").Invoke()))
                .Add(() => WebJs.For(
                    () => WebJs.Let(() => "i", () => WebJs.Num(() => "0")),
                    () => WebJs.Id(() => "i").Op(() => "<", () => WebJs.Id(() => "all").Prop(() => "length")),
                    () => WebJs.Assign(() => WebJs.Id(() => "i"), () => WebJs.Id(() => "i").Op(() => "+", () => WebJs.Num(() => "1"))),
                    () => WebJs.Block()
                        .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Call(() => "getAttribute", () => WebJs.Str(() => "data-value")).Op(() => "===", () => WebJs.Id(() => "h").Prop(() => "value")), new List<JsSyntax>
                            {
                                WebJs.Return(() => WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Prop(() => "textContent"))
                            })
                        }))))
                .Add(() => WebJs.Return(() => WebJs.Str(() => "")))))

            .Add(() => WebJs.Func(() => "filter", () => new List<string>(), () => WebJs.Block()
                .Add(() => WebJs.Let(() => "q", () => WebJs.Id(() => "s").Prop(() => "value").Call(() => "toLowerCase")))
                .Add(() => WebJs.Let(() => "c", () => WebJs.Num(() => "0")))
                .Add(() => WebJs.Let(() => "all", () => WebJs.Id(() => "opts").Invoke()))
                .Add(() => WebJs.For(
                    () => WebJs.Let(() => "i", () => WebJs.Num(() => "0")),
                    () => WebJs.Id(() => "i").Op(() => "<", () => WebJs.Id(() => "all").Prop(() => "length")),
                    () => WebJs.Assign(() => WebJs.Id(() => "i"), () => WebJs.Id(() => "i").Op(() => "+", () => WebJs.Num(() => "1"))),
                    () => WebJs.Block()
                        .Add(() => WebJs.Let(() => "m", () => WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Prop(() => "textContent").Call(() => "toLowerCase").Call(() => "indexOf", () => WebJs.Id(() => "q")).Op(() => "!==", () => WebJs.Num(() => "-1"))))
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Prop(() => "style").Prop(() => "display"), () => WebJs.Ternary(() => WebJs.Id(() => "m"), () => WebJs.Str(() => ""), () => WebJs.Str(() => "none"))).Stmt())
                        .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (WebJs.Id(() => "m"), new List<JsSyntax>
                            {
                                WebJs.Assign(() => WebJs.Id(() => "c"), () => WebJs.Id(() => "c").Op(() => "+", () => WebJs.Num(() => "1"))).Stmt()
                            })
                        }))))
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "nr").Prop(() => "style").Prop(() => "display"), () => WebJs.Ternary(() => WebJs.Id(() => "c"), () => WebJs.Str(() => "none"), () => WebJs.Str(() => ""))).Stmt())
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "hi"), () => WebJs.Num(() => "-1")))
                .Add(() => WebJs.Id(() => "clrHi").Invoke().Stmt())))

            .Add(() => WebJs.Func(() => "clrHi", () => new List<string>(), () => WebJs.Block()
                .Add(() => WebJs.Let(() => "hl", () => WebJs.Id(() => "d").Call(() => "querySelectorAll", () => WebJs.Str(() => ".sl-select-search-option.highlighted"))))
                .Add(() => WebJs.For(
                    () => WebJs.Let(() => "i", () => WebJs.Num(() => "0")),
                    () => WebJs.Id(() => "i").Op(() => "<", () => WebJs.Id(() => "hl").Prop(() => "length")),
                    () => WebJs.Assign(() => WebJs.Id(() => "i"), () => WebJs.Id(() => "i").Op(() => "+", () => WebJs.Num(() => "1"))),
                    () => WebJs.Id(() => "hl").Index(() => WebJs.Id(() => "i")).Call(() => "classList").Call(() => "remove", () => WebJs.Str(() => "highlighted")).Stmt()))))

            .Add(() => WebJs.Func(() => "setHi", () => new List<string> { "idx" }, () => WebJs.Block()
                .Add(() => WebJs.Id(() => "clrHi").Invoke().Stmt())
                .Add(() => WebJs.Let(() => "vis", () => WebJs.New(() => WebJs.Id(() => "Array"))))
                .Add(() => WebJs.Let(() => "all", () => WebJs.Id(() => "opts").Invoke()))
                .Add(() => WebJs.For(
                    () => WebJs.Let(() => "i", () => WebJs.Num(() => "0")),
                    () => WebJs.Id(() => "i").Op(() => "<", () => WebJs.Id(() => "all").Prop(() => "length")),
                    () => WebJs.Assign(() => WebJs.Id(() => "i"), () => WebJs.Id(() => "i").Op(() => "+", () => WebJs.Num(() => "1"))),
                    () => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Prop(() => "style").Prop(() => "display").Op(() => "!==", () => WebJs.Str(() => "none")).Op(() => "&&", () => WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i")).Call(() => "hasAttribute", () => WebJs.Str(() => "disabled")).Not()), new List<JsSyntax>
                        {
                            WebJs.Id(() => "vis").Call(() => "push", () => WebJs.Id(() => "all").Index(() => WebJs.Id(() => "i"))).Stmt()
                        })
                    })))
                .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (WebJs.Id(() => "vis").Prop(() => "length").Not(), new List<JsSyntax>
                    {
                        WebJs.Return(() => WebJs.Str(() => ""))
                    })
                }))
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "idx"), () => WebJs.Id(() => "idx").Op(() => "%", () => WebJs.Id(() => "vis").Prop(() => "length")).Paren().Op(() => "+", () => WebJs.Id(() => "vis").Prop(() => "length")).Paren().Op(() => "%", () => WebJs.Id(() => "vis").Prop(() => "length"))).Stmt())
                .Add(() => WebJs.Id(() => "vis").Index(() => WebJs.Id(() => "idx")).Call(() => "classList").Call(() => "add", () => WebJs.Str(() => "highlighted")).Stmt())
                .Add(() => WebJs.Id(() => "vis").Index(() => WebJs.Id(() => "idx")).Call(() => "scrollIntoView", () => WebJs.Obj().Prop(() => "block", () => WebJs.Str(() => "nearest"))).Stmt())))

            .Add(() => WebJs.Func(() => "sel", () => new List<string> { "o" }, () => WebJs.Block()
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "h").Prop(() => "value"), () => WebJs.Id(() => "o").Call(() => "getAttribute", () => WebJs.Str(() => "data-value"))).Stmt())
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "value"), () => WebJs.Id(() => "o").Prop(() => "textContent")).Stmt())
                .Add(() => WebJs.Id(() => "close").Invoke().Stmt())
                .Add(() => WebJs.Id(() => "h").Call(() => "dispatchEvent", () => WebJs.New(() => WebJs.Id(() => "Event"), () => WebJs.Str(() => "change"))).Stmt())))

            .Add(() => WebJs.Func(() => "open_", () => new List<string>(), () => WebJs.Block()
                .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (WebJs.Id(() => "isOpen"), new List<JsSyntax>
                    {
                        WebJs.Return(() => WebJs.Str(() => ""))
                    })
                }))
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "isOpen"), () => WebJs.Bool(() => true)))
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "d").Prop(() => "style").Prop(() => "display"), () => WebJs.Str(() => "block")).Stmt())
                .Add(() => WebJs.Id(() => "w").Prop(() => "classList").Call(() => "add", () => WebJs.Str(() => "open")).Stmt())
                .Add(() => WebJs.Id(() => "s").Call(() => "select").Stmt())
                .Add(() => WebJs.Id(() => "filter").Invoke().Stmt())))

            .Add(() => WebJs.Func(() => "close", () => new List<string>(), () => WebJs.Block()
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "isOpen"), () => WebJs.Bool(() => false)))
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "d").Prop(() => "style").Prop(() => "display"), () => WebJs.Str(() => "none")).Stmt())
                .Add(() => WebJs.Id(() => "w").Prop(() => "classList").Call(() => "remove", () => WebJs.Str(() => "open")).Stmt())
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "hi"), () => WebJs.Num(() => "-1")))
                .Add(() => WebJs.Id(() => "clrHi").Invoke().Stmt())
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "value"), () => WebJs.Id(() => "getSelText").Invoke()).Stmt())))

            .Add(() => WebJs.Id(() => "s").Call(() => "addEventListener", () => WebJs.Str(() => "focus"), () => WebJs.Id(() => "open_")).Stmt())
            .Add(() => WebJs.Id(() => "s").Call(() => "addEventListener", () => WebJs.Str(() => "input"), () => WebJs.Id(() => "filter")).Stmt())

            .Add(() => WebJs.Id(() => "s").Call(() => "addEventListener", () => WebJs.Str(() => "keydown"), () => WebJs.Arrow(() => new List<string> { "e" }, () => WebJs.Block()
                .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (WebJs.Id(() => "e").Prop(() => "key").Op(() => "===", () => WebJs.Str(() => "ArrowDown")), new List<JsSyntax>
                    {
                        WebJs.Id(() => "e").Call(() => "preventDefault").Stmt(),
                        WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (WebJs.Id(() => "isOpen").Not(), new List<JsSyntax>
                            {
                                WebJs.Id(() => "open_").Invoke().Stmt()
                            })
                        }),
                        WebJs.Id(() => "setHi").Invoke(() => WebJs.Id(() => "hi").Op(() => "+", () => WebJs.Num(() => "1"))).Stmt()
                    }),
                    (WebJs.Id(() => "e").Prop(() => "key").Op(() => "===", () => WebJs.Str(() => "ArrowUp")), new List<JsSyntax>
                    {
                        WebJs.Id(() => "e").Call(() => "preventDefault").Stmt(),
                        WebJs.Id(() => "setHi").Invoke(() => WebJs.Id(() => "hi").Op(() => "-", () => WebJs.Num(() => "1"))).Stmt()
                    }),
                    (WebJs.Id(() => "e").Prop(() => "key").Op(() => "===", () => WebJs.Str(() => "Enter")), new List<JsSyntax>
                    {
                        WebJs.Id(() => "e").Call(() => "preventDefault").Stmt(),
                        WebJs.Let(() => "hl", () => WebJs.Id(() => "d").Call(() => "querySelector", () => WebJs.Str(() => ".sl-select-search-option.highlighted"))),
                        WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (WebJs.Id(() => "hl"), new List<JsSyntax>
                            {
                                WebJs.Id(() => "sel").Invoke(() => WebJs.Id(() => "hl")).Stmt()
                            })
                        })
                    }),
                    (WebJs.Id(() => "e").Prop(() => "key").Op(() => "===", () => WebJs.Str(() => "Escape")), new List<JsSyntax>
                    {
                        WebJs.Id(() => "close").Invoke().Stmt()
                    })
                })))).Stmt())

            .Add(() => WebJs.Id(() => "d").Call(() => "addEventListener", () => WebJs.Str(() => "mousedown"), () => WebJs.Arrow(() => new List<string> { "e" }, () => WebJs.Block()
                .Add(() => WebJs.Let(() => "o", () => WebJs.Id(() => "e").Prop(() => "target").Call(() => "closest", () => WebJs.Str(() => ".sl-select-search-option"))))
                .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (WebJs.Id(() => "o").Op(() => "&&", () => WebJs.Id(() => "o").Call(() => "hasAttribute", () => WebJs.Str(() => "disabled")).Not()), new List<JsSyntax>
                    {
                        WebJs.Id(() => "e").Call(() => "preventDefault").Stmt(),
                        WebJs.Id(() => "sel").Invoke(() => WebJs.Id(() => "o")).Stmt()
                    })
                })))).Stmt())

            .Add(() => WebJs.Id(() => "w").Call(() => "querySelector", () => WebJs.Str(() => ".sl-select-search-arrow")).Call(() => "addEventListener", () => WebJs.Str(() => "click"), () => WebJs.Arrow(() => new List<string> { "e" }, () => WebJs.Block()
                .Add(() => WebJs.Id(() => "e").Call(() => "stopPropagation").Stmt())
                .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (WebJs.Id(() => "isOpen"), new List<JsSyntax>
                    {
                        WebJs.Id(() => "close").Invoke().Stmt()
                    }),
                    (null, new List<JsSyntax>
                    {
                        WebJs.Id(() => "open_").Invoke().Stmt()
                    })
                })))).Stmt())

            .Add(() => WebJs.Id(() => "document").Call(() => "addEventListener", () => WebJs.Str(() => "click"), () => WebJs.Arrow(() => new List<string> { "e" }, () => WebJs.Block()
                .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (WebJs.Id(() => "w").Call(() => "contains", () => WebJs.Id(() => "e").Prop(() => "target")).Not(), new List<JsSyntax>
                    {
                        WebJs.Id(() => "close").Invoke().Stmt()
                    })
                })))).Stmt());

        return "(function(){" + body.Build() + "})();";
    }
}
