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
        // CodeEditorView 是组件视图，不直接渲染完整页面
        // 应使用 RenderWidget 方法获取组件 HTML
        return string.Empty;
    }

    /// <summary>
    /// 渲染代码编辑器组件（HTML + Scripts）
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
        // 不要预先转义！H.Value() 内部的 EscapeAttr 会自动处理 HTML 属性转义
        // 如果预先转义，会导致双重转义：< → &lt; → &amp;lt;
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

        // Monaco Editor 的语言映射
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

        JsBlock requireCallbackBody = Js.Block()
            // 使用 monaco.editor.createModel 创建带语言支持的模型
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
                .Prop(() => "bracketPairColorization", () => Js.Obj().Prop(() => "enabled", () => Js.Bool(() => true))))))
            .Add(() => Js.Id(() => "editor").Call(() => "onDidChangeModelContent", () => Js.Arrow(() => new List<string>(), () => Js.Block()
                .Add(() => Js.Assign(() => Js.Id(() => "dirtyFlag").Prop(() => "value"), () => Js.Str(() => "1"))))).Stmt())
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
        // HTML 属性需要 HTML 实体编码
        // 浏览器会自动解码，JavaScript 读取 value 时会得到原始字符串（包括真正的换行符）
        return code
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }

    private static string EscapeCodeForJs(string code)
    {
        // 必须按照正确的顺序转义：先转义反斜杠，再转义其他字符
        // 否则会导致双重转义（例如 \n 变成 \\n 再变成 \\\\n）
        return code
            .Replace("\\", "\\\\")  // 第一步：转义反斜杠
            .Replace("\"", "\\\"")  // 第二步：转义双引号
            .Replace("\n", "\\n")   // 第三步：转义换行符
            .Replace("\r", "\\r")   // 第四步：转义回车符
            .Replace("\t", "\\t");  // 第五步：转义制表符
    }
}
