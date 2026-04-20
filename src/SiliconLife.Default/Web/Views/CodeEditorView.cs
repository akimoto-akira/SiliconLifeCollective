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

public class CodeEditorView : ViewBase
{
    public override string Render(object model)
    {
        // CodeEditorView is a component view, does not render a complete page directly
        // Should use RenderWidget method to get component HTML
        return string.Empty;
    }

    /// <summary>
    /// Renders the code editor component (HTML + Scripts)
    /// </summary>
    public static (H Html, JsSyntax Scripts) RenderEditor(string editorId, string code, string language = "csharp",
        string filePath = "", bool readOnly = false, string theme = "vs-dark",
        bool minimap = true, bool lineNumbers = true, bool wordWrap = true,
        string saveEndpoint = "")
    {
        var html = RenderWidget(editorId, code, language, filePath, readOnly, theme, minimap, lineNumbers, wordWrap, saveEndpoint);
        var scripts = GetWidgetScripts(editorId, language, theme, readOnly, minimap, lineNumbers, wordWrap, saveEndpoint);
        return (html, scripts);
    }

    public static H RenderWidget(string editorId, string code, string language = "csharp",
        string filePath = "", bool readOnly = false, string theme = "vs-dark",
        bool minimap = true, bool lineNumbers = true, bool wordWrap = true,
        string saveEndpoint = "")
    {
        // Do NOT pre-escape! EscapeAttr inside H.Value() will automatically handle HTML attribute escaping
        // Pre-escaping would cause double escaping: < → &lt; → &amp;lt;
        var editorContainerId = editorId + "-container";

        var toolbarChildren = new List<object>();
        if (!string.IsNullOrEmpty(filePath))
        {
            toolbarChildren.Add(H.Span(filePath).Class("code-editor-filename"));
        }
        toolbarChildren.Add(H.Span(language.ToUpperInvariant()).Class("code-editor-lang-badge"));
        if (!readOnly)
        {
            toolbarChildren.Add(H.Button("💾").Class("code-editor-btn-save").OnClick($"codeEditorSave_{editorId}()"));
        }

        return H.Div(
            H.Div(toolbarChildren.ToArray()).Class("code-editor-toolbar"),
            H.Div().Id(editorContainerId).Class("code-editor-container"),
            H.Input().Id(editorId + "-code-hidden").Attr("type", "hidden").Value(code),
            H.Input().Id(editorId + "-dirty-flag").Attr("type", "hidden").Value("0")
        ).Class("code-editor-widget").Id(editorId);
    }

    public static CssBuilder GetWidgetStyles()
    {
        return CssBuilder.Create()
            .Selector(".code-editor-widget")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "8px")
                .Property("overflow", "hidden")
                .Property("background", "var(--bg-card)")
            .EndSelector()
            .Selector(".code-editor-toolbar")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "10px")
                .Property("padding", "8px 14px")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.05))")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("font-size", "13px")
            .EndSelector()
            .Selector(".code-editor-filename")
                .Property("color", "var(--text-primary)")
                .Property("font-weight", "500")
                .Property("flex", "1")
                .Property("overflow", "hidden")
                .Property("text-overflow", "ellipsis")
                .Property("white-space", "nowrap")
            .EndSelector()
            .Selector(".code-editor-lang-badge")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("padding", "2px 8px")
                .Property("border-radius", "4px")
                .Property("font-size", "11px")
                .Property("font-weight", "600")
                .Property("letter-spacing", "0.5px")
            .EndSelector()
            .Selector(".code-editor-btn-save")
                .Property("background", "none")
                .Property("border", "none")
                .Property("cursor", "pointer")
                .Property("font-size", "18px")
                .Property("padding", "2px 6px")
                .Property("border-radius", "4px")
                .Property("transition", "background 0.2s")
            .EndSelector()
            .Selector(".code-editor-btn-save:hover")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.1))")
            .EndSelector()
            .Selector(".code-editor-container")
                .Property("flex", "1")
                .Property("min-height", "300px")
                .Property("width", "100%")
            .EndSelector()
            .Selector(".code-hover-tip")
                .Property("font-family", "var(--font-family, 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif)")
                .Property("font-size", "13px")
                .Property("line-height", "1.4")
                .Property("max-width", "400px")
                .Property("background", "var(--bg-tooltip, #2d2d30)")
                .Property("color", "var(--text-tooltip, #d4d4d4)")
                .Property("border", "1px solid var(--border-tooltip, #454545)")
                .Property("border-radius", "4px")
                .Property("padding", "8px 12px")
                .Property("box-shadow", "0 2px 8px rgba(0,0,0,0.3)")
            .EndSelector()
            .Selector(".code-hover-tip .tip-header")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "6px")
                .Property("margin-bottom", "6px")
                .Property("padding-bottom", "6px")
                .Property("border-bottom", "1px solid var(--border-tooltip, #454545)")
            .EndSelector()
            .Selector(".code-hover-tip .tip-type")
                .Property("padding", "2px 6px")
                .Property("border-radius", "3px")
                .Property("font-size", "11px")
                .Property("font-weight", "600")
                .Property("text-transform", "uppercase")
            .EndSelector()
            .Selector(".code-hover-tip .tip-type.variable")
                .Property("background", "var(--accent-variable, #4ec9b0)")
                .Property("color", "#000")
            .EndSelector()
            .Selector(".code-hover-tip .tip-type.function")
                .Property("background", "var(--accent-function, #dcdcaa)")
                .Property("color", "#000")
            .EndSelector()
            .Selector(".code-hover-tip .tip-type.class")
                .Property("background", "var(--accent-class, #4ec9b0)")
                .Property("color", "#000")
            .EndSelector()
            .Selector(".code-hover-tip .tip-type.keyword")
                .Property("background", "var(--accent-keyword, #c586c0)")
                .Property("color", "#fff")
            .EndSelector()
            .Selector(".code-hover-tip .tip-type.identifier")
                .Property("background", "var(--accent-identifier, #9cdcfe)")
                .Property("color", "#000")
            .EndSelector()
            .Selector(".code-hover-tip .tip-word")
                .Property("font-weight", "600")
                .Property("color", "var(--text-primary-tooltip, #ffffff)")
            .EndSelector()
            .Selector(".code-hover-tip .tip-content p")
                .Property("margin", "0 0 6px 0")
                .Property("color", "var(--text-secondary-tooltip, #cccccc)")
            .EndSelector()
            .Selector(".code-hover-tip .tip-meta")
                .Property("display", "flex")
                .Property("gap", "12px")
                .Property("font-size", "11px")
                .Property("color", "var(--text-meta-tooltip, #888888)")
                .Property("border-top", "1px solid var(--border-tooltip, #454545)")
                .Property("padding-top", "4px")
            .EndSelector();
    }

    public static JsSyntax GetWidgetScripts(string editorId, string language = "csharp",
        string theme = "vs-dark", bool readOnly = false,
        bool minimap = true, bool lineNumbers = true, bool wordWrap = true,
        string saveEndpoint = "")
    {
        string containerId = editorId + "-container";
        string hiddenId = editorId + "-code-hidden";
        string dirtyId = editorId + "-dirty-flag";
    
        // Monaco Editor language mapping
        var monacoLanguageMap = new Dictionary<string, string>
        {
            { "csharp", "csharp" },
            { "cs", "csharp" },
            { "javascript", "javascript" },
            { "js", "javascript" },
            { "typescript", "typescript" },
            { "ts", "typescript" },
            { "python", "python" },
            { "py", "python" },
            { "html", "html" },
            { "css", "css" },
            { "json", "json" },
            { "xml", "xml" },
            { "sql", "sql" },
            { "markdown", "markdown" },
            { "md", "markdown" },
            { "java", "java" },
            { "cpp", "cpp" },
            { "c", "c" },
            { "go", "go" },
            { "rust", "rust" },
            { "php", "php" },
            { "ruby", "ruby" },
            { "yaml", "yaml" },
            { "yml", "yaml" }
        };
    
        var monacoLanguage = monacoLanguageMap.ContainsKey(language.ToLower()) 
            ? monacoLanguageMap[language.ToLower()] 
            : "plaintext";

        // Editor cache key name
        var editorGuidKey = editorId + "_editorGuid";
        var updateTimerKey = editorId + "_updateTimer";

        // Debounced update code block
        var updateFetchBody = Js.Block()
            .Add(() => Js.Let(() => "code", () => Js.Id(() => "editor").Call(() => "getValue")))
            .Add(() => Js.Id(() => "fetch").Invoke(
                () => Js.Str(() => "/api/code/update"),
                () => Js.Obj()
                    .Prop(() => "method", () => Js.Str(() => "POST"))
                    .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                    .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj()
                        .Prop(() => "editorGuid", () => Js.Id(() => "window").Index(() => Js.Str(() => editorGuidKey)))
                        .Prop(() => "code", () => Js.Id(() => "code")))))
            .Stmt());
    
        JsBlock requireCallbackBody = Js.Block()
            // Use monaco.editor.createModel to create language-supported model
            .Add(() => Js.Let(() => "model", () => Js.Id(() => "monaco").Prop(() => "editor").Call(() => "createModel", () => Js.Id(() => "initialCode"), () => Js.Str(() => monacoLanguage))))
            .Add(() => Js.Let(() => "editor", () => Js.Id(() => "monaco").Prop(() => "editor").Call(() => "create", () => Js.Id(() => "containerEl"), () => Js.Obj()
                .Prop(() => "model", () => Js.Id(() => "model"))
                .Prop(() => "theme", () => Js.Str(() => theme))
                .Prop(() => "readOnly", () => Js.Bool(() => readOnly))
                .Prop(() => "minimap", () => Js.Obj().Prop(() => "enabled", () => Js.Bool(() => minimap)))
                .Prop(() => "lineNumbers", () => Js.Str(() => lineNumbers ? "on" : "off"))
                .Prop(() => "wordWrap", () => Js.Str(() => wordWrap ? "on" : "off"))
                .Prop(() => "automaticLayout", () => Js.Bool(() => true))
                .Prop(() => "scrollBeyondLastLine", () => Js.Bool(() => false))
                .Prop(() => "fontSize", () => Js.Num(() => "14"))
                .Prop(() => "tabSize", () => Js.Num(() => "4"))
                .Prop(() => "renderWhitespace", () => Js.Str(() => "selection"))
                .Prop(() => "bracketPairColorization", () => Js.Obj().Prop(() => "enabled", () => Js.Bool(() => true)))
                .Prop(() => "hover", () => Js.Obj()
                    .Prop(() => "enabled", () => Js.Bool(() => true))
                    .Prop(() => "delay", () => Js.Num(() => "300"))))))
            .Add(() =>
            {
                // Register editor to backend cache
                var registerEditor = Js.Block()
                    .Add(() => Js.Id(() => "fetch").Invoke(
                        () => Js.Str(() => "/api/code/register"),
                        () => Js.Obj()
                            .Prop(() => "method", () => Js.Str(() => "POST"))
                            .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                            .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj().Prop(() => "language", () => Js.Str(() => monacoLanguage))))
                    ).Call(() => "then", () => Js.Arrow(() => new List<string> { "response" },
                        () => Js.Id(() => "response").Call(() => "json")))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => Js.Block()
                        // Save editorGuid
                        .Add(() => Js.Assign(() => Js.Id(() => "window").Index(() => Js.Str(() => editorGuidKey)), () => Js.Id(() => "data").Prop(() => "editorGuid")))
                        // Immediately upload current code to backend cache
                        .Add(() => Js.Let(() => "initialCode", () => Js.Id(() => "editor").Call(() => "getValue")))
                        .Add(() => Js.Id(() => "fetch").Invoke(
                            () => Js.Str(() => "/api/code/update"),
                            () => Js.Obj()
                                .Prop(() => "method", () => Js.Str(() => "POST"))
                                .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                                .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj()
                                    .Prop(() => "editorGuid", () => Js.Id(() => "data").Prop(() => "editorGuid"))
                                    .Prop(() => "code", () => Js.Id(() => "initialCode"))))))
                    )).Stmt());
    
                // Custom hover provider (using POST request)
                var hoverContentItem = Js.Obj()
                    .Prop(() => "value", () => Js.Id(() => "html"))
                    .Prop(() => "supportHtml", () => Js.Bool(() => true))
                    .Prop(() => "isTrusted", () => Js.Bool(() => true));
                    
                var hoverRange = Js.New(() => Js.Id(() => "monaco").Prop(() => "Range"),
                    () => Js.Id(() => "position").Prop(() => "lineNumber"),
                    () => Js.Id(() => "word").Prop(() => "startColumn"),
                    () => Js.Id(() => "position").Prop(() => "lineNumber"),
                    () => Js.Id(() => "word").Prop(() => "endColumn"));
                    
                var hoverResult = Js.Obj()
                    .Prop(() => "range", () => hoverRange)
                    .Prop(() => "contents", () => Js.Array()
                        .Add(() => hoverContentItem));
                
                var fetchHoverBody = Js.Obj()
                    .Prop(() => "method", () => Js.Str(() => "POST"))
                    .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                    .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj()
                        .Prop(() => "editorGuid", () => Js.Id(() => "window").Index(() => Js.Str(() => editorGuidKey)))
                        .Prop(() => "word", () => Js.Id(() => "word").Prop(() => "word"))
                        .Prop(() => "language", () => Js.Str(() => monacoLanguage))
                        .Prop(() => "line", () => Js.Id(() => "position").Prop(() => "lineNumber"))
                        .Prop(() => "column", () => Js.Id(() => "position").Prop(() => "column"))));

                var retryFetchHover = Js.Id(() => "fetch").Invoke(
                    () => Js.Str(() => "/api/code/hover"),
                    () => fetchHoverBody)
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "retryResponse" },
                        () => Js.Id(() => "retryResponse").Call(() => "text")));

                var updateBodyForRetry = Js.Obj()
                    .Prop(() => "method", () => Js.Str(() => "POST"))
                    .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                    .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj()
                        .Prop(() => "editorGuid", () => Js.Id(() => "window").Index(() => Js.Str(() => editorGuidKey)))
                        .Prop(() => "code", () => Js.Id(() => "code"))));

                var fetchHover = Js.Id(() => "fetch").Invoke(
                    () => Js.Str(() => "/api/code/hover"),
                    () => fetchHoverBody)
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "response" }, () => Js.Block()
                        .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (Js.Id(() => "response").Prop(() => "status").Op(() => "===", () => Js.Num(() => "404")), new List<JsSyntax>
                            {
                                Js.Let(() => "code", () => Js.Id(() => "editor").Call(() => "getValue")),
                                Js.Id(() => "fetch").Invoke(
                                    () => Js.Str(() => "/api/code/update"),
                                    () => updateBodyForRetry)
                                    .Call(() => "then", () => Js.Arrow(() => new List<string>(), () => retryFetchHover)).Stmt()
                            })
                        }))
                        .Add(() => Js.Return(() => Js.Id(() => "response").Call(() => "text")))))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "html" }, () => hoverResult));
                
                var hoverBody = Js.Block()
                    .Add(() => Js.Let(() => "word", () => Js.Id(() => "model").Call(() => "getWordAtPosition", () => Js.Id(() => "position"))))
                    .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "word").Not(), new List<JsSyntax>
                        {
                            Js.Return(() => Js.Null())
                        })
                    }))
                    // Check if editorGuid is already registered (async registration may not be complete)
                    .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "window").Index(() => Js.Str(() => editorGuidKey)).Op(() => "===", () => Js.Str(() => "")), new List<JsSyntax>
                        {
                            // editorGuid is empty, return null directly (no Roslyn analysis)
                            Js.Return(() => Js.Null())
                        })
                    }))
                    .Add(() => Js.Return(() => fetchHover));
                
                var hoverProvider = Js.Obj()
                    .Prop(() => "provideHover", () => Js.Arrow(
                        () => new List<string> { "model", "position" },
                        () => hoverBody));
                
                return Js.Block()
                    .Add(() => Js.Assign(() => Js.Id(() => "window").Index(() => Js.Str(() => editorGuidKey)), () => Js.Str(() => "")))
                    .Add(() => registerEditor.Stmt())
                    .Add(() => Js.Id(() => "monaco").Prop(() => "languages").Call(() => "registerHoverProvider",
                        () => Js.Str(() => monacoLanguage),
                        () => hoverProvider).Stmt());
            })
            .Add(() => Js.Id(() => "editor").Call(() => "onDidChangeModelContent", () => Js.Arrow(() => new List<string>(), () => Js.Block()
                .Add(() => Js.Assign(() => Js.Id(() => "dirtyFlag").Prop(() => "value"), () => Js.Str(() => "1")))
                // Trigger debounced update
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "window").Index(() => Js.Str(() => updateTimerKey)).Op(() => "!==", () => Js.Id(() => "undefined")), new List<JsSyntax>
                    {
                        Js.Id(() => "clearTimeout").Invoke(() => Js.Id(() => "window").Index(() => Js.Str(() => updateTimerKey))).Stmt()
                    })
                }))
                .Add(() => Js.Assign(() => Js.Id(() => "window").Index(() => Js.Str(() => updateTimerKey)), () => Js.Id(() => "setTimeout").Invoke(
                    () => Js.Arrow(() => new List<string>(), () => updateFetchBody),
                    () => Js.Num(() => "1000"))))
            )).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "window").Index(() => Js.Str(() => editorId)), () => Js.Id(() => "editor")));

        JsBlock initBody = Js.Block()
            .Add(() => Js.Let(() => "containerEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => containerId))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "containerEl").Not(), new List<JsSyntax>
                {
                    Js.Return(() => Js.Str(() => ""))
                })
            }))
            .Add(() => Js.Let(() => "hiddenInput", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => hiddenId))))
            .Add(() => Js.Let(() => "dirtyFlag", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => dirtyId))))
            .Add(() => Js.Let(() => "initialCode", () => Js.Id(() => "hiddenInput").Prop(() => "value")))
            .Add(() => Js.Id(() => "require").Call(() => "config", () => Js.Obj()
                .Prop(() => "paths", () => Js.Obj()
                    .Prop(() => "vs", () => Js.Str(() => "https://cdn.jsdelivr.net/npm/monaco-editor@0.52.2/min/vs")))).Stmt())
            .Add(() => Js.Id(() => "require").Invoke(
                () => Js.Array().Add(() => Js.Str(() => "vs/editor/editor.main")),
                () => Js.Arrow(() => new List<string>(), () => requireCallbackBody)
            ).Stmt());

        JsBlock saveBody = Js.Block()
            .Add(() => Js.Let(() => "editor", () => Js.Id(() => "window").Index(() => Js.Str(() => editorId))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "editor").Not(), new List<JsSyntax>
                {
                    Js.Return(() => Js.Str(() => ""))
                })
            }))
            .Add(() => Js.Let(() => "code", () => Js.Id(() => "editor").Call(() => "getValue")))
            .Add(() => Js.Let(() => "dirtyFlag", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => dirtyId))));

        if (!string.IsNullOrEmpty(saveEndpoint))
        {
            saveBody
                .Add(() => Js.Id(() => "fetch").Invoke(
                    () => Js.Str(() => saveEndpoint),
                    () => Js.Obj()
                        .Prop(() => "method", () => Js.Str(() => "POST"))
                        .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                        .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj().Prop(() => "code", () => Js.Id(() => "code"))))
                ).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => Js.Block()
                    .Add(() => Js.Assign(() => Js.Id(() => "dirtyFlag").Prop(() => "value"), () => Js.Str(() => "0"))))).Stmt());
        }
        else
        {
            saveBody
                .Add(() => Js.Assign(() => Js.Id(() => "dirtyFlag").Prop(() => "value"), () => Js.Str(() => "0")));
        }

        return Js.Block()
            .Add(() => Js.Func(() => $"codeEditorInit_{editorId}", () => new List<string>(), () => initBody))
            .Add(() => Js.Func(() => $"codeEditorSave_{editorId}", () => new List<string>(), () => saveBody))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "typeof").Invoke(() => Js.Id(() => "require")).Op(() => "!==", () => Js.Str(() => "undefined")), new List<JsSyntax>
                {
                    Js.Id(() => $"codeEditorInit_{editorId}").Invoke().Stmt()
                }),
                (null, new List<JsSyntax>
                {
                    Js.Let(() => "loaderScript", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "script"))),
                    Js.Assign(() => Js.Id(() => "loaderScript").Prop(() => "src"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/monaco-editor@0.52.2/min/vs/loader.js")),
                    Js.Assign(() => Js.Id(() => "loaderScript").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => $"codeEditorInit_{editorId}").Invoke())),
                    Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "loaderScript")).Stmt()
                })
            }));
    }

    private static string EscapeForHtmlAttribute(string code)
    {
        // HTML attributes need HTML entity encoding
        // Browser will auto-decode, JavaScript reading value will get original string (including real newlines)
        return code
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }

    private static string EscapeCodeForJs(string code)
    {
        // Must escape in correct order: first backslash, then other characters
        // Otherwise will cause double escaping (e.g., \n becomes \\n then becomes \\\\n)
        return code
            .Replace("\\", "\\\\")  // Step 1: Escape backslash
            .Replace("\"", "\\\"")  // Step 2: Escape double quote
            .Replace("\n", "\\n")   // Step 3: Escape newline
            .Replace("\r", "\\r")   // Step 4: Escape carriage return
            .Replace("\t", "\\t");  // Step 5: Escape tab
    }
}
