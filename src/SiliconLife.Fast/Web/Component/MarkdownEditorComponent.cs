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

using WebJs = SiliconLife.Fast.Web.Js;

namespace SiliconLife.Fast.Web.Component;

/// <summary>
/// Markdown editor component with CodeMirror integration, live preview, and save functionality
/// </summary>
public class MarkdownEditorComponent : ComponentBase
{
    private string _markdown = "";
    private string _filePath = "";
    private bool _readOnly = false;
    private string _initialMode = "edit";
    private string _saveEndpoint = "";

    /// <summary>
    /// Set initial markdown content
    /// </summary>
    public MarkdownEditorComponent Markdown(string markdown)
    {
        _markdown = markdown;
        return this;
    }

    /// <summary>
    /// Set file path to display in toolbar
    /// </summary>
    public MarkdownEditorComponent FilePath(string filePath)
    {
        _filePath = filePath;
        return this;
    }

    /// <summary>
    /// Set read-only mode
    /// </summary>
    public MarkdownEditorComponent ReadOnly(bool readOnly = true)
    {
        _readOnly = readOnly;
        return this;
    }

    /// <summary>
    /// Set initial display mode: "edit", "preview", or "split"
    /// </summary>
    public MarkdownEditorComponent InitialMode(string mode)
    {
        _initialMode = mode;
        return this;
    }

    /// <summary>
    /// Set save endpoint URL for POST requests
    /// </summary>
    public MarkdownEditorComponent SaveEndpoint(string endpoint)
    {
        _saveEndpoint = endpoint;
        return this;
    }

    public override string Render()
    {
        // Sanitize editorId to ensure it's a valid JavaScript identifier
        var editorId = base.Id ?? "md-editor-" + Guid.NewGuid().ToString("N")[..8];
        return RenderWidget(editorId, _markdown, _filePath, _readOnly, _initialMode, _saveEndpoint).Build();
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
        
        var escapedMarkdown = EscapeCodeForWebJs(markdown);
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
            .Selector(".md-editor-preview-pane pre.hlWebJs")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.3))")
                .Property("padding", "16px")
                .Property("border-radius", "6px")
                .Property("overflow-x", "auto")
                .Property("margin", "1em 0")
                .Property("tab-size", "4")
            .EndSelector()
            .Selector(".md-editor-preview-pane .hlWebJs")
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

        JsBlock initBody = WebJs.Block()
            .Add(() => WebJs.Let(() => "textarea", () => WebJs.Id(() => "document").Call(() => "getElementById", () => WebJs.Str(() => textareaId))))
            .Add(() => WebJs.Let(() => "dirtyFlag", () => WebJs.Id(() => "document").Call(() => "getElementById", () => WebJs.Str(() => dirtyId))))
            .Add(() => WebJs.Let(() => "modeInput", () => WebJs.Id(() => "document").Call(() => "getElementById", () => WebJs.Str(() => modeId))))
            .Add(() => WebJs.Let(() => "widget", () => WebJs.Id(() => "document").Call(() => "getElementById", () => WebJs.Str(() => widgetId))))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "textarea").Not().Op(() => "||", () => WebJs.Id(() => "widget").Not()), new List<JsSyntax>
                {
                    WebJs.Return(() => WebJs.Str(() => ""))
                })
            }))
            // Set initial content to textarea before creating CodeMirror
            .Add(() => WebJs.Assign(() => WebJs.Id(() => "textarea").Prop(() => "value"), () => WebJs.Str(() => initialContent)))
            // Initialize CodeMirror editor
            .Add(() => WebJs.Let(() => "editor", () => WebJs.Id(() => "window").Prop(() => "CodeMirror").Call(() => "fromTextArea", () => WebJs.Id(() => "textarea"), () => WebJs.Obj()
                .Prop(() => "mode", () => WebJs.Str(() => "text/x-markdown"))
                .Prop(() => "lineNumbers", () => WebJs.Bool(() => true))
                .Prop(() => "lineWrapping", () => WebJs.Bool(() => true))
                .Prop(() => "tabSize", () => WebJs.Num(() => "4"))
                .Prop(() => "indentWithTabs", () => WebJs.Bool(() => true))
                .Prop(() => "theme", () => WebJs.Str(() => "default")))))
            .Add(() => WebJs.Id(() => "editor").Call(() => "on", () => WebJs.Str(() => "change"), () => WebJs.Arrow(() => new List<string> { "instance" }, () => WebJs.Block()
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "dirtyFlag").Prop(() => "value"), () => WebJs.Str(() => "1")))
                .Add(() => WebJs.Id(() => "instance").Call(() => "save").Stmt())
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "textarea").Prop(() => "value"), () => WebJs.Id(() => "instance").Call(() => "getValue")).Stmt()))))
            .Add(() => WebJs.Assign(() => WebJs.Id(() => "window").Index(() => WebJs.Str(() => safeEditorId)), () => WebJs.Obj()
                .Prop(() => "editor", () => WebJs.Id(() => "editor"))
                .Prop(() => "textarea", () => WebJs.Id(() => "textarea"))
                .Prop(() => "dirtyFlag", () => WebJs.Id(() => "dirtyFlag"))
                .Prop(() => "modeInput", () => WebJs.Id(() => "modeInput"))
                .Prop(() => "widget", () => WebJs.Id(() => "widget"))))
            // Refresh CodeMirror to ensure proper rendering
            .Add(() => WebJs.Id(() => "editor").Call(() => "refresh").Stmt())
            .Add(() => WebJs.Id(() => $"mdEditorSetMode_{safeEditorId}").Invoke(() => WebJs.Id(() => "modeInput").Prop(() => "value")).Stmt())
            .Add(() => WebJs.Id(() => $"mdEditorRender_{safeEditorId}").Invoke().Stmt());

        JsBlock renderBody = WebJs.Block()
            .Add(() => WebJs.Let(() => "state", () => WebJs.Id(() => "window").Index(() => WebJs.Str(() => safeEditorId))))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "state").Not(), new List<JsSyntax>
                {
                    WebJs.Return(() => WebJs.Str(() => ""))
                })
            }))
            .Add(() => WebJs.Let(() => "previewEl", () => WebJs.Id(() => "document").Call(() => "getElementById", () => WebJs.Str(() => previewId))))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "previewEl").Not(), new List<JsSyntax>
                {
                    WebJs.Return(() => WebJs.Str(() => ""))
                })
            }))
            // Get markdown content from CodeMirror editor
            .Add(() => WebJs.Let(() => "md", () => WebJs.Id(() => "state").Prop(() => "editor").Call(() => "getValue")))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "typeof").Invoke(() => WebJs.Id(() => "marked")).Op(() => "!==", () => WebJs.Str(() => "undefined")), new List<JsSyntax>
                {
                    // Render markdown to HTML
                    WebJs.Assign(() => WebJs.Id(() => "previewEl").Prop(() => "innerHTML"), () => WebJs.Id(() => "marked").Call(() => "parse", () => WebJs.Id(() => "md"))).Stmt(),
                    // Apply highlight.WebJs to code blocks in preview pane
                    WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (WebJs.Id(() => "typeof").Invoke(() => WebJs.Id(() => "hlWebJs")).Op(() => "!==", () => WebJs.Str(() => "undefined")), new List<JsSyntax>
                        {
                            WebJs.Id(() => "previewEl").Call(() => "querySelectorAll", () => WebJs.Str(() => "pre code")).Call(() => "forEach", () => WebJs.Arrow(() => new List<string> { "block" }, () =>
                                WebJs.Id(() => "hlWebJs").Call(() => "highlightElement", () => WebJs.Id(() => "block"))
                            )).Stmt()
                        })
                    })
                }),
                (null, new List<JsSyntax>
                {
                    // Fallback: just show the markdown as-is if marked is not loaded
                    WebJs.Assign(() => WebJs.Id(() => "previewEl").Prop(() => "innerHTML"), () => WebJs.Id(() => "md")).Stmt()
                })
            }));

        JsBlock setModeBody = WebJs.Block()
            .Add(() => WebJs.Let(() => "state", () => WebJs.Id(() => "window").Index(() => WebJs.Str(() => safeEditorId))))
            .Add(() => WebJs.Let(() => "widget", () => WebJs.Id(() => "state").Op(() => "&&", () => WebJs.Id(() => "state").Prop(() => "widget")).Op(() => "||", () => WebJs.Id(() => "document").Call(() => "getElementById", () => WebJs.Str(() => widgetId)))))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "widget").Not(), new List<JsSyntax>
                {
                    WebJs.Return(() => WebJs.Str(() => ""))
                })
            }))
            .Add(() => WebJs.Assign(() => WebJs.Id(() => "widget").Prop(() => "dataset").Prop(() => "mode"), () => WebJs.Id(() => "mode")))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "state").Not(), new List<JsSyntax>
                {
                    WebJs.Return(() => WebJs.Str(() => ""))
                })
            }))
            .Add(() => WebJs.Assign(() => WebJs.Id(() => "state").Prop(() => "modeInput").Prop(() => "value"), () => WebJs.Id(() => "mode")))
            .Add(() => WebJs.Let(() => "btns", () => WebJs.Id(() => "widget").Call(() => "querySelectorAll", () => WebJs.Str(() => ".md-editor-mode-btn"))))
            .Add(() => WebJs.Id(() => "btns").Call(() => "forEach", () => WebJs.Arrow(() => new List<string> { "btn" }, () => WebJs.Block()
                .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (WebJs.Id(() => "btn").Prop(() => "classList").Call(() => "contains", () => WebJs.Op(() => WebJs.Str(() => "md-editor-mode-"), () => "+", () => WebJs.Id(() => "mode"))), new List<JsSyntax>
                    {
                        WebJs.Id(() => "btn").Prop(() => "classList").Call(() => "add", () => WebJs.Str(() => "active")).Stmt()
                    }),
                    (null, new List<JsSyntax>
                    {
                        WebJs.Id(() => "btn").Prop(() => "classList").Call(() => "remove", () => WebJs.Str(() => "active")).Stmt()
                    })
                }))))
                .Stmt())
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "mode").Op(() => "===", () => WebJs.Str(() => "preview")).Op(() => "||", () => WebJs.Id(() => "mode").Op(() => "===", () => WebJs.Str(() => "split"))), new List<JsSyntax>
                {
                    WebJs.Id(() => $"mdEditorRender_{safeEditorId}").Invoke().Stmt()
                })
            }));

        JsBlock saveBody = WebJs.Block()
            .Add(() => WebJs.Let(() => "state", () => WebJs.Id(() => "window").Index(() => WebJs.Str(() => safeEditorId))))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "state").Not(), new List<JsSyntax>
                {
                    WebJs.Return(() => WebJs.Str(() => ""))
                })
            }))
            .Add(() => WebJs.Let(() => "md", () => WebJs.Id(() => "state").Prop(() => "editor").Call(() => "getValue")))
            .Add(() => WebJs.Let(() => "dirtyFlag", () => WebJs.Id(() => "state").Prop(() => "dirtyFlag")));

        if (!string.IsNullOrEmpty(saveEndpoint))
        {
            saveBody
                .Add(() => WebJs.Id(() => "fetch").Invoke(
                    () => WebJs.Str(() => saveEndpoint),
                    () => WebJs.Obj()
                        .Prop(() => "method", () => WebJs.Str(() => "POST"))
                        .Prop(() => "headers", () => WebJs.Obj().Prop(() => "Content-Type", () => WebJs.Str(() => "application/json")))
                        .Prop(() => "body", () => WebJs.Id(() => "JSON").Call(() => "stringify", () => WebJs.Obj().Prop(() => "markdown", () => WebJs.Id(() => "md")))))
                .Call(() => "then", () => WebJs.Arrow(() => new List<string> { "r" }, () => WebJs.Block()
                    .Add(() => WebJs.Return(() => WebJs.Id(() => "r").Call(() => "json")))))
                .Call(() => "then", () => WebJs.Arrow(() => new List<string> { "data" }, () => WebJs.Block()
                    .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (WebJs.Id(() => "data").Prop(() => "success"), new List<JsSyntax>
                        {
                            // Success: clear dirty flag
                            WebJs.Assign(() => WebJs.Id(() => "dirtyFlag").Prop(() => "value"), () => WebJs.Str(() => "0")).Stmt(),
                            WebJs.Id(() => "console").Call(() => "log", () => WebJs.Str(() => "Save successful")).Stmt()
                        }),
                        (null, new List<JsSyntax>
                        {
                            // Error: show detailed error message
                            WebJs.Id(() => "console").Call(() => "error", () => WebJs.Str(() => "Save failed:"), () => WebJs.Id(() => "data")).Stmt(),
                            // Build detailed error message
                            WebJs.Let(() => "errorMsg", () => WebJs.Id(() => "data").Prop(() => "error").Op(() => "||", () => WebJs.Str(() => "Save failed"))).Stmt(),
                            WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                            {
                                (WebJs.Id(() => "data").Prop(() => "details").Op(() => "!==", () => WebJs.Id(() => "undefined")).Op(() => "&&", () => WebJs.Id(() => "data").Prop(() => "details")), new List<JsSyntax>
                                {
                                    WebJs.Assign(() => WebJs.Id(() => "errorMsg"), () => WebJs.Op(() => WebJs.Id(() => "errorMsg"), () => "+", () => WebJs.Str(() => "\n\n"))).Stmt(),
                                    WebJs.Assign(() => WebJs.Id(() => "errorMsg"), () => WebJs.Op(() => WebJs.Id(() => "errorMsg"), () => "+", () => WebJs.Id(() => "data").Prop(() => "details"))).Stmt()
                                })
                            }).Stmt(),
                            WebJs.Id(() => "alert").Invoke(() => WebJs.Id(() => "errorMsg")).Stmt()
                        })
                    }))))
                .Call(() => "catch", () => WebJs.Arrow(() => new List<string> { "err" }, () => WebJs.Block()
                    .Add(() => WebJs.Id(() => "console").Call(() => "error", () => WebJs.Str(() => "Save error:"), () => WebJs.Id(() => "err")))
                    .Add(() => WebJs.Id(() => "alert").Invoke(() => WebJs.Op(() => WebJs.Str(() => "Save failed: "), () => "+", () => WebJs.Id(() => "err"))))))
                .Stmt());
        }
        else
        {
            saveBody
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "dirtyFlag").Prop(() => "value"), () => WebJs.Str(() => "0")));
        }

        return WebJs.Block()
            .Add(() => WebJs.Func(() => $"mdEditorInit_{safeEditorId}", () => new List<string>(), () => initBody))
            .Add(() => WebJs.Func(() => $"mdEditorRender_{safeEditorId}", () => new List<string>(), () => renderBody))
            .Add(() => WebJs.Func(() => $"mdEditorSetMode_{safeEditorId}", () => new List<string> { "mode" }, () => setModeBody))
            .Add(() => WebJs.Func(() => $"mdEditorSave_{safeEditorId}", () => new List<string>(), () => saveBody))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "typeof").Invoke(() => WebJs.Id(() => "CodeMirror")).Op(() => "!==", () => WebJs.Str(() => "undefined")), new List<JsSyntax>
                {
                    // CodeMirror is loaded, check if marked is also loaded
                    WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (WebJs.Id(() => "typeof").Invoke(() => WebJs.Id(() => "marked")).Op(() => "!==", () => WebJs.Str(() => "undefined")), new List<JsSyntax>
                        {
                            WebJs.Id(() => $"mdEditorInit_{safeEditorId}").Invoke().Stmt()
                        }),
                        (null, new List<JsSyntax>
                        {
                            // Load marked.WebJs and highlight.WebJs
                            WebJs.Let(() => "scriptMarked", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "script"))),
                            WebJs.Assign(() => WebJs.Id(() => "scriptMarked").Prop(() => "src"), () => WebJs.Str(() => "https://cdn.jsdelivr.net/npm/marked@15.0.12/marked.min.WebJs")),
                            WebJs.Assign(() => WebJs.Id(() => "scriptMarked").Prop(() => "onload"), () => WebJs.Arrow(() => new List<string>(), () => WebJs.Block()
                                .Add(() => WebJs.Let(() => "scriptHlWebJs", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "script"))))
                                .Add(() => WebJs.Assign(() => WebJs.Id(() => "scriptHlWebJs").Prop(() => "src"), () => WebJs.Str(() => "https://cdn.jsdelivr.net/npm/@highlightWebJs/cdn-assets@11.9.0/highlight.min.WebJs")))
                                .Add(() => WebJs.Assign(() => WebJs.Id(() => "scriptHlWebJs").Prop(() => "onload"), () => WebJs.Arrow(() => new List<string>(), () => WebJs.Id(() => $"mdEditorInit_{safeEditorId}").Invoke())))
                                .Add(() => WebJs.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => WebJs.Id(() => "scriptHlWebJs")).Stmt()))),
                            WebJs.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => WebJs.Id(() => "scriptMarked")).Stmt()
                        })
                    })
                }),
                (null, new List<JsSyntax>
                {
                    // Load CodeMirror first, then other libraries
                    // Load CodeMirror CSS
                    WebJs.Let(() => "linkCss", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "link"))),
                    WebJs.Assign(() => WebJs.Id(() => "linkCss").Prop(() => "rel"), () => WebJs.Str(() => "stylesheet")),
                    WebJs.Assign(() => WebJs.Id(() => "linkCss").Prop(() => "href"), () => WebJs.Str(() => "https://cdn.jsdelivr.net/npm/codemirror@5.65.16/lib/codemirror.min.css")),
                    WebJs.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => WebJs.Id(() => "linkCss")).Stmt(),
                    
                    // Load CodeMirror WebJs
                    WebJs.Let(() => "scriptCodeMirror", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "script"))),
                    WebJs.Assign(() => WebJs.Id(() => "scriptCodeMirror").Prop(() => "src"), () => WebJs.Str(() => "https://cdn.jsdelivr.net/npm/codemirror@5.65.16/lib/codemirror.min.WebJs")),
                    WebJs.Assign(() => WebJs.Id(() => "scriptCodeMirror").Prop(() => "onload"), () => WebJs.Arrow(() => new List<string>(), () => WebJs.Block()
                        // Load Markdown mode
                        .Add(() => WebJs.Let(() => "scriptMdMode", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "script"))))
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "scriptMdMode").Prop(() => "src"), () => WebJs.Str(() => "https://cdn.jsdelivr.net/npm/codemirror@5.65.16/mode/markdown/markdown.min.WebJs")))
                        // Load marked.WebJs
                        .Add(() => WebJs.Let(() => "scriptMarked", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "script"))))
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "scriptMarked").Prop(() => "src"), () => WebJs.Str(() => "https://cdn.jsdelivr.net/npm/marked@15.0.12/marked.min.WebJs")))
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "scriptMarked").Prop(() => "onload"), () => WebJs.Arrow(() => new List<string>(), () => WebJs.Block()
                            .Add(() => WebJs.Let(() => "scriptHlWebJs", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "script"))))
                            .Add(() => WebJs.Assign(() => WebJs.Id(() => "scriptHlWebJs").Prop(() => "src"), () => WebJs.Str(() => "https://cdn.jsdelivr.net/npm/@highlightWebJs/cdn-assets@11.9.0/highlight.min.WebJs")))
                            .Add(() => WebJs.Assign(() => WebJs.Id(() => "scriptHlWebJs").Prop(() => "onload"), () => WebJs.Arrow(() => new List<string>(), () => WebJs.Id(() => $"mdEditorInit_{safeEditorId}").Invoke())))
                            .Add(() => WebJs.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => WebJs.Id(() => "scriptHlWebJs")).Stmt()))))
                        .Add(() => WebJs.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => WebJs.Id(() => "scriptMarked")).Stmt())
                        .Add(() => WebJs.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => WebJs.Id(() => "scriptMdMode")).Stmt()))),
                    WebJs.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => WebJs.Id(() => "scriptCodeMirror")).Stmt()
                })
            }));
    }

    private static string EscapeCodeForWebJs(string code)
    {
        return code
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }
}
