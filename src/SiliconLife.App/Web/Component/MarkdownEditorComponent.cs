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
/// Markdown editor component with CodeMirror integration, live preview, and save functionality
/// </summary>
public class MarkdownEditorComponent : ComponentBase
{
    private string _markdown = "";
    private string _filePath = "";
    private bool _readOnly = false;
    private string _initialMode = "edit";
    private string _saveEndpoint = "";

    // CDN sources for editor libraries: primary + fallback mirror (jsdelivr is
    // unreliable in some regions). Even when every source fails the widget
    // degrades to a plain textarea instead of becoming unresponsive.
    private const string CodeMirrorCssUrl = "https://cdn.jsdelivr.net/npm/codemirror@5.65.16/lib/codemirror.min.css";
    private const string CodeMirrorCssFallbackUrl = "https://unpkg.com/codemirror@5.65.16/lib/codemirror.css";
    private const string CodeMirrorJsUrl = "https://cdn.jsdelivr.net/npm/codemirror@5.65.16/lib/codemirror.min.js";
    private const string CodeMirrorJsFallbackUrl = "https://unpkg.com/codemirror@5.65.16/lib/codemirror.js";
    private const string CodeMirrorMarkdownModeUrl = "https://cdn.jsdelivr.net/npm/codemirror@5.65.16/mode/markdown/markdown.min.js";
    private const string CodeMirrorMarkdownModeFallbackUrl = "https://unpkg.com/codemirror@5.65.16/mode/markdown/markdown.js";
    private const string MarkedJsUrl = "https://cdn.jsdelivr.net/npm/marked@15.0.12/marked.min.js";
    private const string MarkedJsFallbackUrl = "https://unpkg.com/marked@15.0.12/marked.min.js";
    private const string HighlightJsUrl = "https://cdn.jsdelivr.net/npm/@highlightjs/cdn-assets@11.9.0/highlight.min.js";
    private const string HighlightJsFallbackUrl = "https://unpkg.com/@highlightjs/cdn-assets@11.9.0/highlight.min.js";

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

    public override H Render()
    {
        var editorId = base.Id ?? "md-editor-" + Guid.NewGuid().ToString("N")[..8];
        return RenderWidget(editorId, _markdown, _filePath, _readOnly, _initialMode, _saveEndpoint);
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
                    H.Textarea().Id(textareaId).Class("md-editor-textarea").Placeholder("Write markdown here...").Text(markdown)
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
            .EndSelector()
            // Raw-text preview fallback (used when marked.js is unavailable)
            .Selector(".md-editor-preview-fallback")
                .Property("white-space", "pre-wrap")
                .Property("word-break", "break-word")
                .Property("font-family", "'JetBrains Mono', 'Fira Code', 'Consolas', monospace")
                .Property("font-size", "13px")
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
            // Initialize CodeMirror when available; otherwise keep the plain
            // textarea so edit/save/preview still work without CDN libraries
            .Add(() => WebJs.Let(() => "editor", () => WebJs.Null()))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "typeof").Invoke(() => WebJs.Id(() => "CodeMirror")).Op(() => "!==", () => WebJs.Str(() => "undefined")), new List<JsSyntax>
                {
                    WebJs.Assign(() => WebJs.Id(() => "editor"), () => WebJs.Id(() => "window").Prop(() => "CodeMirror").Call(() => "fromTextArea", () => WebJs.Id(() => "textarea"), () => WebJs.Obj()
                        .Prop(() => "mode", () => WebJs.Str(() => "text/x-markdown"))
                        .Prop(() => "lineNumbers", () => WebJs.Bool(() => true))
                        .Prop(() => "lineWrapping", () => WebJs.Bool(() => true))
                        .Prop(() => "tabSize", () => WebJs.Num(() => "4"))
                        .Prop(() => "indentWithTabs", () => WebJs.Bool(() => true))
                        .Prop(() => "theme", () => WebJs.Str(() => "default")))).Stmt(),
                    WebJs.Id(() => "editor").Call(() => "on", () => WebJs.Str(() => "change"), () => WebJs.Arrow(() => new List<string> { "instance" }, () => WebJs.Block()
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "dirtyFlag").Prop(() => "value"), () => WebJs.Str(() => "1")))
                        .Add(() => WebJs.Id(() => "instance").Call(() => "save").Stmt())
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "textarea").Prop(() => "value"), () => WebJs.Id(() => "instance").Call(() => "getValue")).Stmt()))).Stmt(),
                    WebJs.Id(() => "editor").Call(() => "refresh").Stmt()
                }),
                (null, new List<JsSyntax>
                {
                    // Plain textarea fallback: track edits via the input event
                    WebJs.Id(() => "textarea").Call(() => "addEventListener", () => WebJs.Str(() => "input"), () => WebJs.Arrow(() => new List<string>(), () => WebJs.Block()
                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "dirtyFlag").Prop(() => "value"), () => WebJs.Str(() => "1")).Stmt()))).Stmt()
                })
            }))
            .Add(() => WebJs.Assign(() => WebJs.Id(() => "window").Index(() => WebJs.Str(() => safeEditorId)), () => WebJs.Obj()
                .Prop(() => "editor", () => WebJs.Id(() => "editor"))
                .Prop(() => "textarea", () => WebJs.Id(() => "textarea"))
                .Prop(() => "dirtyFlag", () => WebJs.Id(() => "dirtyFlag"))
                .Prop(() => "modeInput", () => WebJs.Id(() => "modeInput"))
                .Prop(() => "widget", () => WebJs.Id(() => "widget"))))
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
            .Add(() => WebJs.Let(() => "md", () => WebJs.Ternary(() => WebJs.Id(() => "state").Prop(() => "editor"),
                () => WebJs.Id(() => "state").Prop(() => "editor").Call(() => "getValue"),
                () => WebJs.Id(() => "state").Prop(() => "textarea").Prop(() => "value"))))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "typeof").Invoke(() => WebJs.Id(() => "marked")).Op(() => "!==", () => WebJs.Str(() => "undefined")), new List<JsSyntax>
                {
                    // Render markdown to HTML
                    WebJs.Id(() => "previewEl").Prop(() => "classList").Call(() => "remove", () => WebJs.Str(() => "md-editor-preview-fallback")).Stmt(),
                    WebJs.Assign(() => WebJs.Id(() => "previewEl").Prop(() => "innerHTML"), () => WebJs.Id(() => "marked").Call(() => "parse", () => WebJs.Id(() => "md"))).Stmt(),
                    // Apply highlight.js to code blocks in preview pane
                    WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (WebJs.Id(() => "typeof").Invoke(() => WebJs.Id(() => "hljs")).Op(() => "!==", () => WebJs.Str(() => "undefined")), new List<JsSyntax>
                        {
                            WebJs.Id(() => "previewEl").Call(() => "querySelectorAll", () => WebJs.Str(() => "pre code")).Call(() => "forEach", () => WebJs.Arrow(() => new List<string> { "block" }, () =>
                                WebJs.Id(() => "hljs").Call(() => "highlightElement", () => WebJs.Id(() => "block"))
                            )).Stmt()
                        })
                    })
                }),
                (null, new List<JsSyntax>
                {
                    // Fallback: show the raw markdown as plain text when marked is unavailable
                    WebJs.Id(() => "previewEl").Prop(() => "classList").Call(() => "add", () => WebJs.Str(() => "md-editor-preview-fallback")).Stmt(),
                    WebJs.Assign(() => WebJs.Id(() => "previewEl").Prop(() => "textContent"), () => WebJs.Id(() => "md")).Stmt()
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
            .Add(() => WebJs.Let(() => "md", () => WebJs.Ternary(() => WebJs.Id(() => "state").Prop(() => "editor"),
                () => WebJs.Id(() => "state").Prop(() => "editor").Call(() => "getValue"),
                () => WebJs.Id(() => "state").Prop(() => "textarea").Prop(() => "value"))))
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
                            // Success: clear dirty flag and give visible feedback on the save button
                            WebJs.Assign(() => WebJs.Id(() => "dirtyFlag").Prop(() => "value"), () => WebJs.Str(() => "0")).Stmt(),
                            WebJs.Let(() => "saveBtn", () => WebJs.Id(() => "state").Prop(() => "widget").Call(() => "querySelector", () => WebJs.Str(() => ".md-editor-btn-save"))).Stmt(),
                            WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                            {
                                (WebJs.Id(() => "saveBtn"), new List<JsSyntax>
                                {
                                    WebJs.Assign(() => WebJs.Id(() => "saveBtn").Prop(() => "textContent"), () => WebJs.Str(() => "✅")).Stmt(),
                                    WebJs.Id(() => "window").Call(() => "setTimeout", () => WebJs.Arrow(() => new List<string>(), () => WebJs.Block()
                                        .Add(() => WebJs.Assign(() => WebJs.Id(() => "saveBtn").Prop(() => "textContent"), () => WebJs.Str(() => "💾")).Stmt())), () => WebJs.Num(() => "1500")).Stmt()
                                })
                            }).Stmt(),
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

        // Try each URL in order until one loads; call onfail if all fail.
        var tryNextScriptBody = WebJs.Block()
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "i").Op(() => ">=", () => WebJs.Id(() => "urls").Prop(() => "length")), new List<JsSyntax>
                {
                    WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (WebJs.Id(() => "onfail"), new List<JsSyntax>
                        {
                            WebJs.Id(() => "onfail").Invoke().Stmt()
                        })
                    }),
                    WebJs.Return(() => WebJs.Null())
                })
            }))
            .Add(() => WebJs.Const(() => "s", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "script"))))
            .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "src"), () => WebJs.Id(() => "urls").Index(() => WebJs.Id(() => "i"))))
            .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "onload"), () => WebJs.Id(() => "onload")))
            .Add(() => WebJs.Assign(() => WebJs.Id(() => "s").Prop(() => "onerror"), () => WebJs.Arrow(() => new List<string>(), () => WebJs.Block()
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "i"), () => WebJs.Op(() => WebJs.Id(() => "i"), () => "+", () => WebJs.Num(() => "1"))))
                .Add(() => WebJs.Id(() => "tryNext").Invoke().Stmt()))))
            .Add(() => WebJs.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => WebJs.Id(() => "s")).Stmt());

        JsBlock loadScriptBody = WebJs.Block()
            .Add(() => WebJs.Let(() => "i", () => WebJs.Num(() => "0")))
            .Add(() => WebJs.Func(() => "tryNext", () => new List<string>(), () => tryNextScriptBody))
            // Kick off the load chain OUTSIDE tryNext's body
            .Add(() => WebJs.Id(() => "tryNext").Invoke().Stmt());

        // Same retry logic for stylesheets (CSS failures are purely cosmetic).
        var tryNextCssBody = WebJs.Block()
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "i").Op(() => ">=", () => WebJs.Id(() => "urls").Prop(() => "length")), new List<JsSyntax>
                {
                    WebJs.Return(() => WebJs.Null())
                })
            }))
            .Add(() => WebJs.Const(() => "l", () => WebJs.Id(() => "document").Call(() => "createElement", () => WebJs.Str(() => "link"))))
            .Add(() => WebJs.Assign(() => WebJs.Id(() => "l").Prop(() => "rel"), () => WebJs.Str(() => "stylesheet")))
            .Add(() => WebJs.Assign(() => WebJs.Id(() => "l").Prop(() => "href"), () => WebJs.Id(() => "urls").Index(() => WebJs.Id(() => "i"))))
            .Add(() => WebJs.Assign(() => WebJs.Id(() => "l").Prop(() => "onerror"), () => WebJs.Arrow(() => new List<string>(), () => WebJs.Block()
                .Add(() => WebJs.Assign(() => WebJs.Id(() => "i"), () => WebJs.Op(() => WebJs.Id(() => "i"), () => "+", () => WebJs.Num(() => "1"))))
                .Add(() => WebJs.Id(() => "tryNext").Invoke().Stmt()))))
            .Add(() => WebJs.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => WebJs.Id(() => "l")).Stmt());

        JsBlock loadCssBody = WebJs.Block()
            .Add(() => WebJs.Let(() => "i", () => WebJs.Num(() => "0")))
            .Add(() => WebJs.Func(() => "tryNext", () => new List<string>(), () => tryNextCssBody))
            // Kick off the load chain OUTSIDE tryNext's body
            .Add(() => WebJs.Id(() => "tryNext").Invoke().Stmt());

        // marked / hljs only enhance preview rendering; they must never gate
        // editor initialization. Re-render the preview once marked arrives.
        JsBlock loadLibsBody = WebJs.Block()
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "typeof").Invoke(() => WebJs.Id(() => "marked")).Op(() => "===", () => WebJs.Str(() => "undefined")), new List<JsSyntax>
                {
                    WebJs.Id(() => $"mdEditorLoadScript_{safeEditorId}").Invoke(
                        () => WebJs.Array().Add(() => WebJs.Str(() => MarkedJsUrl)).Add(() => WebJs.Str(() => MarkedJsFallbackUrl)),
                        () => WebJs.Arrow(() => new List<string>(), () => WebJs.Id(() => $"mdEditorRender_{safeEditorId}").Invoke()),
                        () => WebJs.Null()).Stmt()
                })
            }))
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "typeof").Invoke(() => WebJs.Id(() => "hljs")).Op(() => "===", () => WebJs.Str(() => "undefined")), new List<JsSyntax>
                {
                    WebJs.Id(() => $"mdEditorLoadScript_{safeEditorId}").Invoke(
                        () => WebJs.Array().Add(() => WebJs.Str(() => HighlightJsUrl)).Add(() => WebJs.Str(() => HighlightJsFallbackUrl)),
                        () => WebJs.Null(),
                        () => WebJs.Null()).Stmt()
                })
            }));

        // Bootstrap: if CodeMirror already exists (multi-widget page) init now;
        // otherwise load it from the CDN chain and degrade to a plain textarea
        // when every source is unreachable.
        JsBlock bootstrapBody = WebJs.Block()
            .Add(() => WebJs.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (WebJs.Id(() => "typeof").Invoke(() => WebJs.Id(() => "CodeMirror")).Op(() => "!==", () => WebJs.Str(() => "undefined")), new List<JsSyntax>
                {
                    WebJs.Id(() => $"mdEditorLoadLibs_{safeEditorId}").Invoke().Stmt(),
                    WebJs.Id(() => $"mdEditorInit_{safeEditorId}").Invoke().Stmt()
                }),
                (null, new List<JsSyntax>
                {
                    WebJs.Id(() => $"mdEditorLoadCss_{safeEditorId}").Invoke(
                        () => WebJs.Array().Add(() => WebJs.Str(() => CodeMirrorCssUrl)).Add(() => WebJs.Str(() => CodeMirrorCssFallbackUrl))).Stmt(),
                    WebJs.Id(() => $"mdEditorLoadScript_{safeEditorId}").Invoke(
                        () => WebJs.Array().Add(() => WebJs.Str(() => CodeMirrorJsUrl)).Add(() => WebJs.Str(() => CodeMirrorJsFallbackUrl)),
                        () => WebJs.Arrow(() => new List<string>(), () => WebJs.Block()
                            .Add(() => WebJs.Id(() => $"mdEditorLoadScript_{safeEditorId}").Invoke(
                                () => WebJs.Array().Add(() => WebJs.Str(() => CodeMirrorMarkdownModeUrl)).Add(() => WebJs.Str(() => CodeMirrorMarkdownModeFallbackUrl)),
                                () => WebJs.Null(),
                                () => WebJs.Null()).Stmt())
                            .Add(() => WebJs.Id(() => $"mdEditorLoadLibs_{safeEditorId}").Invoke().Stmt())
                            .Add(() => WebJs.Id(() => $"mdEditorInit_{safeEditorId}").Invoke().Stmt())),
                        // All CDN sources unreachable: keep the plain textarea working
                        () => WebJs.Arrow(() => new List<string>(), () => WebJs.Id(() => $"mdEditorInit_{safeEditorId}").Invoke())).Stmt()
                })
            }));

        return WebJs.Block()
            .Add(() => WebJs.Func(() => $"mdEditorInit_{safeEditorId}", () => new List<string>(), () => initBody))
            .Add(() => WebJs.Func(() => $"mdEditorRender_{safeEditorId}", () => new List<string>(), () => renderBody))
            .Add(() => WebJs.Func(() => $"mdEditorSetMode_{safeEditorId}", () => new List<string> { "mode" }, () => setModeBody))
            .Add(() => WebJs.Func(() => $"mdEditorSave_{safeEditorId}", () => new List<string>(), () => saveBody))
            .Add(() => WebJs.Func(() => $"mdEditorLoadScript_{safeEditorId}", () => new List<string> { "urls", "onload", "onfail" }, () => loadScriptBody))
            .Add(() => WebJs.Func(() => $"mdEditorLoadCss_{safeEditorId}", () => new List<string> { "urls" }, () => loadCssBody))
            .Add(() => WebJs.Func(() => $"mdEditorLoadLibs_{safeEditorId}", () => new List<string>(), () => loadLibsBody))
            .Add(() => bootstrapBody);
    }
}
