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

namespace SiliconLife.Default.Web.Views;

public class MarkdownEditorView : ViewBase
{
    public override string Render(object model)
    {
        return string.Empty;
    }

    public static H RenderWidget(string editorId, string markdown, string filePath = "",
        bool readOnly = false, string initialMode = "edit",
        string saveEndpoint = "")
    {
        // Sanitize editorId to ensure it's a valid JavaScript identifier
        // Replace hyphens and other invalid characters with underscores
        var safeEditorId = new string(editorId.Select(c => char.IsLetterOrDigit(c) || c == '_' ? c : '_').ToArray());
        if (char.IsDigit(safeEditorId[0]))
            safeEditorId = "_" + safeEditorId;  // Identifiers can't start with digits
        
        var escapedMarkdown = EscapeCodeForJs(markdown);
        var textareaId = safeEditorId + "-textarea";
        var previewId = safeEditorId + "-preview";

        var toolbarChildren = new List<object>();
        if (!string.IsNullOrEmpty(filePath))
        {
            toolbarChildren.Add(H.Span(filePath).Class("md-editor-filename"));
        }

        toolbarChildren.Add(H.Div(
            H.Button("✏️").Class("md-editor-mode-btn md-editor-mode-edit").OnClick($"mdEditorSetMode_{safeEditorId}('edit')"),
            H.Button("👁").Class("md-editor-mode-btn md-editor-mode-preview").OnClick($"mdEditorSetMode_{safeEditorId}('preview')"),
            H.Button("📋").Class("md-editor-mode-btn md-editor-mode-split").OnClick($"mdEditorSetMode_{safeEditorId}('split')")
        ).Class("md-editor-mode-group"));

        if (!readOnly)
        {
            toolbarChildren.Add(H.Button("💾").Class("md-editor-btn-save").OnClick($"mdEditorSave_{safeEditorId}()"));
        }

        return H.Div(
            H.Div(toolbarChildren.ToArray()).Class("md-editor-toolbar"),
            H.Div(
                H.Div(
                    H.Textarea().Id(textareaId).Class("md-editor-textarea").Placeholder("Write markdown here...")
                ).Class("md-editor-edit-pane"),
                H.Div().Id(previewId).Class("md-editor-preview-pane")
            ).Class("md-editor-body"),
            H.Input().Id(safeEditorId + "-dirty-flag").Attr("type", "hidden").Value("0"),
            H.Input().Id(safeEditorId + "-mode-hidden").Attr("type", "hidden").Value(initialMode)
        ).Class("md-editor-widget").Id(safeEditorId);
    }

    public static CssBuilder GetWidgetStyles()
    {
        return CssBuilder.Create()
            .Selector(".md-editor-widget")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "8px")
                .Property("overflow", "hidden")
                .Property("background", "var(--bg-card)")
                .Property("height", "100%")
            .EndSelector()
            .Selector(".md-editor-toolbar")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "10px")
                .Property("padding", "8px 14px")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.05))")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("font-size", "13px")
            .EndSelector()
            .Selector(".md-editor-filename")
                .Property("color", "var(--text-primary)")
                .Property("font-weight", "500")
                .Property("flex", "1")
                .Property("overflow", "hidden")
                .Property("text-overflow", "ellipsis")
                .Property("white-space", "nowrap")
            .EndSelector()
            .Selector(".md-editor-mode-group")
                .Property("display", "flex")
                .Property("gap", "2px")
                .Property("background", "var(--bg-primary, rgba(0,0,0,0.2))")
                .Property("border-radius", "6px")
                .Property("padding", "2px")
            .EndSelector()
            .Selector(".md-editor-mode-btn")
                .Property("background", "none")
                .Property("border", "none")
                .Property("cursor", "pointer")
                .Property("font-size", "14px")
                .Property("padding", "4px 10px")
                .Property("border-radius", "4px")
                .Property("transition", "background 0.2s, color 0.2s")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".md-editor-mode-btn:hover")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.1))")
            .EndSelector()
            .Selector(".md-editor-mode-btn.active")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
            .EndSelector()
            .Selector(".md-editor-btn-save")
                .Property("background", "none")
                .Property("border", "none")
                .Property("cursor", "pointer")
                .Property("font-size", "18px")
                .Property("padding", "2px 6px")
                .Property("border-radius", "4px")
                .Property("transition", "background 0.2s")
            .EndSelector()
            .Selector(".md-editor-btn-save:hover")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.1))")
            .EndSelector()
            .Selector(".md-editor-body")
                .Property("display", "flex")
                .Property("flex", "1")
                .Property("overflow", "hidden")
                .Property("min-height", "300px")
            .EndSelector()
            .Selector(".md-editor-edit-pane")
                .Property("flex", "1")
                .Property("display", "flex")
                .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".md-editor-textarea")
                .Property("flex", "1")
                .Property("resize", "none")
                .Property("border", "none")
                .Property("outline", "none")
                .Property("padding", "16px")
                .Property("font-family", "'JetBrains Mono', 'Fira Code', 'Consolas', monospace")
                .Property("font-size", "14px")
                .Property("line-height", "1.6")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("tab-size", "4")
            .EndSelector()
            .Selector(".md-editor-edit-pane .CodeMirror")
                .Property("height", "100%")
                .Property("font-size", "14px")
                .Property("font-family", "'JetBrains Mono', 'Fira Code', 'Consolas', monospace")
                .Property("line-height", "1.6")
                .Property("tab-size", "4")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".md-editor-edit-pane .CodeMirror-gutters")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.05))")
                .Property("border-right", "1px solid var(--border)")
            .EndSelector()
            .Selector(".md-editor-edit-pane .CodeMirror-linenumber")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".md-editor-edit-pane .CodeMirror-cursor")
                .Property("border-left-color", "var(--text-primary)")
            .EndSelector()
            .Selector(".md-editor-preview-pane")
                .Property("flex", "1")
                .Property("overflow-y", "auto")
                .Property("padding", "16px 24px")
                .Property("line-height", "1.7")
                .Property("color", "var(--text-primary)")
                .Property("display", "none")
            .EndSelector()
            .Selector(".md-editor-preview-pane h1")
                .Property("font-size", "2em")
                .Property("margin", "0.67em 0")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("padding-bottom", "0.3em")
            .EndSelector()
            .Selector(".md-editor-preview-pane h2")
                .Property("font-size", "1.5em")
                .Property("margin", "0.83em 0")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("padding-bottom", "0.3em")
            .EndSelector()
            .Selector(".md-editor-preview-pane h3")
                .Property("font-size", "1.25em")
                .Property("margin", "1em 0")
            .EndSelector()
            .Selector(".md-editor-preview-pane h4, .md-editor-preview-pane h5, .md-editor-preview-pane h6")
                .Property("margin", "1em 0")
            .EndSelector()
            .Selector(".md-editor-preview-pane p")
                .Property("margin", "0.8em 0")
            .EndSelector()
            .Selector(".md-editor-preview-pane code")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.1))")
                .Property("padding", "2px 6px")
                .Property("border-radius", "3px")
                .Property("font-size", "0.9em")
                .Property("font-family", "'JetBrains Mono', 'Fira Code', 'Consolas', monospace")
            .EndSelector()
            .Selector(".md-editor-preview-pane pre")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.3))")
                .Property("padding", "16px")
                .Property("border-radius", "6px")
                .Property("overflow-x", "auto")
                .Property("margin", "1em 0")
            .EndSelector()
            .Selector(".md-editor-preview-pane pre code")
                .Property("background", "none")
                .Property("padding", "0")
            .EndSelector()
            .Selector(".md-editor-preview-pane pre.hljs")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.3))")
                .Property("padding", "16px")
                .Property("border-radius", "6px")
                .Property("overflow-x", "auto")
                .Property("margin", "1em 0")
                .Property("tab-size", "4")
            .EndSelector()
            .Selector(".md-editor-preview-pane .hljs")
                .Property("display", "block")
                .Property("overflow-x", "auto")
                .Property("padding", "0")
                .Property("background", "transparent")
            .EndSelector()
            .Selector(".md-editor-preview-pane blockquote")
                .Property("border-left", "4px solid var(--accent-primary)")
                .Property("margin", "1em 0")
                .Property("padding", "0.5em 1em")
                .Property("color", "var(--text-secondary)")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.03))")
                .Property("border-radius", "0 4px 4px 0")
            .EndSelector()
            .Selector(".md-editor-preview-pane ul, .md-editor-preview-pane ol")
                .Property("padding-left", "2em")
                .Property("margin", "0.5em 0")
            .EndSelector()
            .Selector(".md-editor-preview-pane table")
                .Property("border-collapse", "collapse")
                .Property("width", "100%")
                .Property("margin", "1em 0")
            .EndSelector()
            .Selector(".md-editor-preview-pane th, .md-editor-preview-pane td")
                .Property("border", "1px solid var(--border)")
                .Property("padding", "8px 12px")
                .Property("text-align", "left")
            .EndSelector()
            .Selector(".md-editor-preview-pane th")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.05))")
                .Property("font-weight", "600")
            .EndSelector()
            .Selector(".md-editor-preview-pane img")
                .Property("max-width", "100%")
                .Property("border-radius", "6px")
            .EndSelector()
            .Selector(".md-editor-preview-pane a")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
            .EndSelector()
            .Selector(".md-editor-preview-pane a:hover")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".md-editor-preview-pane hr")
                .Property("border", "none")
                .Property("border-top", "1px solid var(--border)")
                .Property("margin", "1.5em 0")
            .EndSelector()
            .Selector(".md-editor-widget[data-mode=\"preview\"] .md-editor-edit-pane")
                .Property("display", "none")
            .EndSelector()
            .Selector(".md-editor-widget[data-mode=\"preview\"] .md-editor-preview-pane")
                .Property("display", "block")
            .EndSelector()
            .Selector(".md-editor-widget[data-mode=\"edit\"] .md-editor-edit-pane")
                .Property("display", "flex")
            .EndSelector()
            .Selector(".md-editor-widget[data-mode=\"edit\"] .md-editor-preview-pane")
                .Property("display", "none")
            .EndSelector()
            .Selector(".md-editor-widget[data-mode=\"split\"] .md-editor-edit-pane")
                .Property("display", "flex")
                .Property("flex", "1")
            .EndSelector()
            .Selector(".md-editor-widget[data-mode=\"split\"] .md-editor-preview-pane")
                .Property("display", "block")
                .Property("flex", "1")
                .Property("border-left", "1px solid var(--border)")
            .EndSelector();
    }

    public static JsSyntax GetWidgetScripts(string editorId, string initialContent = "", bool readOnly = false,
        string saveEndpoint = "")
    {
        // Sanitize editorId to match RenderWidget
        var safeEditorId = new string(editorId.Select(c => char.IsLetterOrDigit(c) || c == '_' ? c : '_').ToArray());
        if (char.IsDigit(safeEditorId[0]))
            safeEditorId = "_" + safeEditorId;
        
        string textareaId = safeEditorId + "-textarea";
        string previewId = safeEditorId + "-preview";
        string dirtyId = safeEditorId + "-dirty-flag";
        string modeId = safeEditorId + "-mode-hidden";
        string widgetId = safeEditorId;

        JsBlock initBody = Js.Block()
            .Add(() => Js.Let(() => "textarea", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => textareaId))))
            .Add(() => Js.Let(() => "dirtyFlag", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => dirtyId))))
            .Add(() => Js.Let(() => "modeInput", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => modeId))))
            .Add(() => Js.Let(() => "widget", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => widgetId))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "textarea").Not().Op(() => "||", () => Js.Id(() => "widget").Not()), new List<JsSyntax>
                {
                    Js.Return(() => Js.Str(() => ""))
                })
            }))
            // Set initial content to textarea before creating CodeMirror
            .Add(() => Js.Assign(() => Js.Id(() => "textarea").Prop(() => "value"), () => Js.Str(() => initialContent)))
            // Initialize CodeMirror editor
            .Add(() => Js.Let(() => "editor", () => Js.Id(() => "window").Prop(() => "CodeMirror").Call(() => "fromTextArea", () => Js.Id(() => "textarea"), () => Js.Obj()
                .Prop(() => "mode", () => Js.Str(() => "text/x-markdown"))
                .Prop(() => "lineNumbers", () => Js.Bool(() => true))
                .Prop(() => "lineWrapping", () => Js.Bool(() => true))
                .Prop(() => "tabSize", () => Js.Num(() => "4"))
                .Prop(() => "indentWithTabs", () => Js.Bool(() => true))
                .Prop(() => "theme", () => Js.Str(() => "default")))))
            .Add(() => Js.Id(() => "editor").Call(() => "on", () => Js.Str(() => "change"), () => Js.Arrow(() => new List<string> { "instance" }, () => Js.Block()
                .Add(() => Js.Assign(() => Js.Id(() => "dirtyFlag").Prop(() => "value"), () => Js.Str(() => "1")))
                .Add(() => Js.Id(() => "instance").Call(() => "save").Stmt())
                .Add(() => Js.Assign(() => Js.Id(() => "textarea").Prop(() => "value"), () => Js.Id(() => "instance").Call(() => "getValue")).Stmt()))))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Index(() => Js.Str(() => safeEditorId)), () => Js.Obj()
                .Prop(() => "editor", () => Js.Id(() => "editor"))
                .Prop(() => "textarea", () => Js.Id(() => "textarea"))
                .Prop(() => "dirtyFlag", () => Js.Id(() => "dirtyFlag"))
                .Prop(() => "modeInput", () => Js.Id(() => "modeInput"))
                .Prop(() => "widget", () => Js.Id(() => "widget"))))
            // Refresh CodeMirror to ensure proper rendering
            .Add(() => Js.Id(() => "editor").Call(() => "refresh").Stmt())
            .Add(() => Js.Id(() => $"mdEditorSetMode_{safeEditorId}").Invoke(() => Js.Id(() => "modeInput").Prop(() => "value")).Stmt())
            .Add(() => Js.Id(() => $"mdEditorRender_{safeEditorId}").Invoke().Stmt());

        JsBlock renderBody = Js.Block()
            .Add(() => Js.Let(() => "state", () => Js.Id(() => "window").Index(() => Js.Str(() => safeEditorId))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "state").Not(), new List<JsSyntax>
                {
                    Js.Return(() => Js.Str(() => ""))
                })
            }))
            .Add(() => Js.Let(() => "previewEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => previewId))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "previewEl").Not(), new List<JsSyntax>
                {
                    Js.Return(() => Js.Str(() => ""))
                })
            }))
            // Get markdown content from CodeMirror editor
            .Add(() => Js.Let(() => "md", () => Js.Id(() => "state").Prop(() => "editor").Call(() => "getValue")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "typeof").Invoke(() => Js.Id(() => "marked")).Op(() => "!==", () => Js.Str(() => "undefined")), new List<JsSyntax>
                {
                    // Configure marked to use highlight.js for code highlighting (only if hljs is available)
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "typeof").Invoke(() => Js.Id(() => "hljs")).Op(() => "!==", () => Js.Str(() => "undefined")), new List<JsSyntax>
                        {
                            // Configure marked with highlight function
                            Js.Assign(
                                () => Js.Id(() => "window").Prop(() => "markedHighlight"),
                                () => Js.Arrow(() => new List<string> { "code", "lang" }, () => Js.Block()
                                    .Add(() => Js.Let(() => "result", () => Js.Ternary(
                                        () => Js.Id(() => "lang").Op(() => "&&", () => Js.Id(() => "hljs").Call(() => "getLanguage", () => Js.Id(() => "lang"))),
                                        () => Js.Id(() => "hljs").Call(() => "highlight", () => Js.Id(() => "code"), () => Js.Obj().Prop(() => "language", () => Js.Id(() => "lang"))).Prop(() => "value"),
                                        () => Js.Id(() => "hljs").Call(() => "highlightAuto", () => Js.Id(() => "code")).Prop(() => "value")
                                    )))
                                    .Add(() => Js.Return(() => Js.Id(() => "result")))))
                            .Stmt(),
                            Js.Id(() => "marked").Call(() => "setOptions", () => Js.Obj()
                                .Prop(() => "highlight", () => Js.Id(() => "window").Prop(() => "markedHighlight")))
                            .Stmt()
                        })
                    }),
                    // Render markdown to HTML
                    Js.Assign(() => Js.Id(() => "previewEl").Prop(() => "innerHTML"), () => Js.Id(() => "marked").Call(() => "parse", () => Js.Id(() => "md"))).Stmt(),
                    // Apply highlight.js to all code blocks if hljs is available
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "typeof").Invoke(() => Js.Id(() => "hljs")).Op(() => "!==", () => Js.Str(() => "undefined")), new List<JsSyntax>
                        {
                            Js.Id(() => "hljs").Call(() => "highlightAll").Stmt()
                        })
                    })
                }),
                (null, new List<JsSyntax>
                {
                    // Fallback: just show the markdown as-is if marked is not loaded
                    Js.Assign(() => Js.Id(() => "previewEl").Prop(() => "innerHTML"), () => Js.Id(() => "md")).Stmt()
                })
            }));

        JsBlock setModeBody = Js.Block()
            .Add(() => Js.Let(() => "state", () => Js.Id(() => "window").Index(() => Js.Str(() => safeEditorId))))
            .Add(() => Js.Let(() => "widget", () => Js.Id(() => "state").Op(() => "&&", () => Js.Id(() => "state").Prop(() => "widget")).Op(() => "||", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => widgetId)))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "widget").Not(), new List<JsSyntax>
                {
                    Js.Return(() => Js.Str(() => ""))
                })
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "widget").Prop(() => "dataset").Prop(() => "mode"), () => Js.Id(() => "mode")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "state").Not(), new List<JsSyntax>
                {
                    Js.Return(() => Js.Str(() => ""))
                })
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "state").Prop(() => "modeInput").Prop(() => "value"), () => Js.Id(() => "mode")))
            .Add(() => Js.Let(() => "btns", () => Js.Id(() => "widget").Call(() => "querySelectorAll", () => Js.Str(() => ".md-editor-mode-btn"))))
            .Add(() => Js.Id(() => "btns").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "btn" }, () => Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "btn").Prop(() => "classList").Call(() => "contains", () => Js.Op(() => Js.Str(() => "md-editor-mode-"), () => "+", () => Js.Id(() => "mode"))), new List<JsSyntax>
                    {
                        Js.Id(() => "btn").Prop(() => "classList").Call(() => "add", () => Js.Str(() => "active")).Stmt()
                    }),
                    (null, new List<JsSyntax>
                    {
                        Js.Id(() => "btn").Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "active")).Stmt()
                    })
                }))))
                .Stmt())
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "mode").Op(() => "===", () => Js.Str(() => "preview")).Op(() => "||", () => Js.Id(() => "mode").Op(() => "===", () => Js.Str(() => "split"))), new List<JsSyntax>
                {
                    Js.Id(() => $"mdEditorRender_{safeEditorId}").Invoke().Stmt()
                })
            }));

        JsBlock saveBody = Js.Block()
            .Add(() => Js.Let(() => "state", () => Js.Id(() => "window").Index(() => Js.Str(() => safeEditorId))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "state").Not(), new List<JsSyntax>
                {
                    Js.Return(() => Js.Str(() => ""))
                })
            }))
            .Add(() => Js.Let(() => "md", () => Js.Id(() => "state").Prop(() => "editor").Call(() => "getValue")))
            .Add(() => Js.Let(() => "dirtyFlag", () => Js.Id(() => "state").Prop(() => "dirtyFlag")));

        if (!string.IsNullOrEmpty(saveEndpoint))
        {
            saveBody
                .Add(() => Js.Id(() => "fetch").Invoke(
                    () => Js.Str(() => saveEndpoint),
                    () => Js.Obj()
                        .Prop(() => "method", () => Js.Str(() => "POST"))
                        .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                        .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj().Prop(() => "markdown", () => Js.Id(() => "md")))))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Block()
                    .Add(() => Js.Return(() => Js.Id(() => "r").Call(() => "json")))))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => Js.Block()
                    .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "data").Prop(() => "success"), new List<JsSyntax>
                        {
                            // Success: clear dirty flag
                            Js.Assign(() => Js.Id(() => "dirtyFlag").Prop(() => "value"), () => Js.Str(() => "0")).Stmt(),
                            Js.Id(() => "console").Call(() => "log", () => Js.Str(() => "Save successful")).Stmt()
                        }),
                        (null, new List<JsSyntax>
                        {
                            // Error: show error message
                            Js.Id(() => "console").Call(() => "error", () => Js.Id(() => "data").Prop(() => "error").Op(() => "||", () => Js.Str(() => "Save failed"))).Stmt(),
                            Js.Id(() => "alert").Call(() => "invoke", () => Js.Id(() => "data").Prop(() => "error").Op(() => "||", () => Js.Str(() => "Save failed"))).Stmt()
                        })
                    }))))
                .Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => Js.Block()
                    .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Save error:"), () => Js.Id(() => "err")))
                    .Add(() => Js.Id(() => "alert").Call(() => "invoke", () => Js.Op(() => Js.Str(() => "Save failed: "), () => "+", () => Js.Id(() => "err"))))))
                .Stmt());
        }
        else
        {
            saveBody
                .Add(() => Js.Assign(() => Js.Id(() => "dirtyFlag").Prop(() => "value"), () => Js.Str(() => "0")));
        }

        return Js.Block()
            .Add(() => Js.Func(() => $"mdEditorInit_{safeEditorId}", () => new List<string>(), () => initBody))
            .Add(() => Js.Func(() => $"mdEditorRender_{safeEditorId}", () => new List<string>(), () => renderBody))
            .Add(() => Js.Func(() => $"mdEditorSetMode_{safeEditorId}", () => new List<string> { "mode" }, () => setModeBody))
            .Add(() => Js.Func(() => $"mdEditorSave_{safeEditorId}", () => new List<string>(), () => saveBody))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "typeof").Invoke(() => Js.Id(() => "CodeMirror")).Op(() => "!==", () => Js.Str(() => "undefined")), new List<JsSyntax>
                {
                    // CodeMirror is loaded, check if marked is also loaded
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "typeof").Invoke(() => Js.Id(() => "marked")).Op(() => "!==", () => Js.Str(() => "undefined")), new List<JsSyntax>
                        {
                            Js.Id(() => $"mdEditorInit_{safeEditorId}").Invoke().Stmt()
                        }),
                        (null, new List<JsSyntax>
                        {
                            // Load marked.js and highlight.js
                            Js.Let(() => "scriptMarked", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "script"))),
                            Js.Assign(() => Js.Id(() => "scriptMarked").Prop(() => "src"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/marked@15.0.12/marked.min.js")),
                            Js.Assign(() => Js.Id(() => "scriptMarked").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                                .Add(() => Js.Let(() => "scriptHljs", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "script"))))
                                .Add(() => Js.Assign(() => Js.Id(() => "scriptHljs").Prop(() => "src"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/@highlightjs/cdn-assets@11.9.0/highlight.min.js")))
                                .Add(() => Js.Assign(() => Js.Id(() => "scriptHljs").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => $"mdEditorInit_{safeEditorId}").Invoke())))
                                .Add(() => Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "scriptHljs")).Stmt()))),
                            Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "scriptMarked")).Stmt()
                        })
                    })
                }),
                (null, new List<JsSyntax>
                {
                    // Load CodeMirror first, then other libraries
                    // Load CodeMirror CSS
                    Js.Let(() => "linkCss", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "link"))),
                    Js.Assign(() => Js.Id(() => "linkCss").Prop(() => "rel"), () => Js.Str(() => "stylesheet")),
                    Js.Assign(() => Js.Id(() => "linkCss").Prop(() => "href"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/codemirror@5.65.16/lib/codemirror.min.css")),
                    Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "linkCss")).Stmt(),
                    
                    // Load CodeMirror JS
                    Js.Let(() => "scriptCodeMirror", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "script"))),
                    Js.Assign(() => Js.Id(() => "scriptCodeMirror").Prop(() => "src"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/codemirror@5.65.16/lib/codemirror.min.js")),
                    Js.Assign(() => Js.Id(() => "scriptCodeMirror").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                        // Load Markdown mode
                        .Add(() => Js.Let(() => "scriptMdMode", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "script"))))
                        .Add(() => Js.Assign(() => Js.Id(() => "scriptMdMode").Prop(() => "src"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/codemirror@5.65.16/mode/markdown/markdown.min.js")))
                        // Load marked.js
                        .Add(() => Js.Let(() => "scriptMarked", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "script"))))
                        .Add(() => Js.Assign(() => Js.Id(() => "scriptMarked").Prop(() => "src"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/marked@15.0.12/marked.min.js")))
                        .Add(() => Js.Assign(() => Js.Id(() => "scriptMarked").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                            .Add(() => Js.Let(() => "scriptHljs", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "script"))))
                            .Add(() => Js.Assign(() => Js.Id(() => "scriptHljs").Prop(() => "src"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/@highlightjs/cdn-assets@11.9.0/highlight.min.js")))
                            .Add(() => Js.Assign(() => Js.Id(() => "scriptHljs").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => $"mdEditorInit_{safeEditorId}").Invoke())))
                            .Add(() => Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "scriptHljs")).Stmt()))))
                        .Add(() => Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "scriptMarked")).Stmt())
                        .Add(() => Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "scriptMdMode")).Stmt()))),
                    Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "scriptCodeMirror")).Stmt()
                })
            }));
    }

    private static string EscapeCodeForJs(string code)
    {
        return code
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }
}
