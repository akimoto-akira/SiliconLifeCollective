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

using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

public class WorkNoteView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as WorkNoteViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        string pageTitle = vm.ProjectId.HasValue ? vm.Localization.ProjectWorkNotesPageHeader : vm.Localization.PageTitleWorkNotes;
        return RenderPage(vm.Skin, pageTitle, "work-notes", vm.Localization, body, GetScripts(vm.Localization, vm.BeingId, vm.ProjectId), GetStyles(), "work-notes");
    }

    private static H RenderBody(WorkNoteViewModel vm)
    {
        string headerText = vm.ProjectId.HasValue ? vm.Localization.ProjectWorkNotesPageHeader : vm.Localization.WorkNotesPageHeader;
        string totalLabel = vm.ProjectId.HasValue ? vm.Localization.ProjectWorkNotesTotalPages : vm.Localization.WorkNotesTotalPages;
        return H.Div(
            H.Div(
                H.H1(headerText),
                H.Div(
                    H.Span(string.Format(totalLabel, "")).Id("total-pages").Class("stat-value")
                ).Class("page-stat")
            ).Class("page-header"),
            H.Div().Id("notes-grid").Class("notes-grid"),
            H.Div().Id("note-detail").Class("note-detail")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".page-stat")
                .Property("margin-left", "16px")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".stat-value")
                .Property("font-weight", "bold")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".notes-grid")
                .Property("display", "grid")
                .Property("grid-template-columns", "repeat(auto-fill, minmax(300px, 1fr))")
                .Property("gap", "20px")
                .Property("margin-bottom", "30px")
            .EndSelector()
            .Selector(".note-card")
                .Property("background", "var(--bg-card)")
                .Property("padding", "20px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
                .Property("cursor", "pointer")
                .Property("transition", "transform 0.2s, box-shadow 0.2s")
            .EndSelector()
            .Selector(".note-card:hover")
                .Property("transform", "translateY(-4px)")
                .Property("box-shadow", "0 4px 16px rgba(0,0,0,0.12)")
            .EndSelector()
            .Selector(".note-title")
                .Property("font-size", "16px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".note-summary")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".note-meta")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".note-detail")
                .Property("display", "none")
            .EndSelector()
            .Selector(".note-detail.active")
                .Property("display", "block")
                .Property("position", "fixed")
                .Property("top", "0")
                .Property("left", "0")
                .Property("right", "0")
                .Property("bottom", "0")
                .Property("background", "var(--bg-primary)")
                .Property("z-index", "1000")
                .Property("padding", "20px")
                .Property("overflow-y", "auto")
            .EndSelector()
            .Selector(".note-detail-header")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "space-between")
                .Property("margin-bottom", "20px")
                .Property("padding-bottom", "15px")
                .Property("border-bottom", "1px solid var(--border)")
            .EndSelector()
            .Selector(".note-detail-title")
                .Property("font-size", "20px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".note-detail-close")
                .Property("padding", "8px 16px")
                .Property("background", "var(--accent-primary)")
                .Property("color", "white")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".note-detail-close:hover")
                .Property("opacity", "0.9")
            .EndSelector()
            .Selector(".note-detail-meta")
                .Property("margin-bottom", "20px")
                .Property("padding", "15px")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.05))")
                .Property("border-radius", "8px")
            .EndSelector()
            .Selector(".note-detail-meta p")
                .Property("margin", "0 0 8px 0")
            .EndSelector()
            .Selector(".note-detail-meta p:last-child")
                .Property("margin-bottom", "0")
            .EndSelector()
            .Selector(".note-detail-meta strong")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".note-detail-content")
                .Property("background", "var(--bg-card)")
                .Property("padding", "20px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".note-detail-content .markdown-body")
                .Property("font-size", "15px")
                .Property("line-height", "1.8")
            .EndSelector()
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "40px")
                .Property("color", "var(--text-secondary)")
            .EndSelector();
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc, Guid beingId, Guid? projectId = null)
    {
        string listUrl = projectId.HasValue
            ? $"/api/projects/{projectId}/work-notes/list"
            : "/api/work-notes/list?beingId=" + beingId;
        string readUrlPrefix = projectId.HasValue
            ? $"/api/projects/{projectId}/work-notes/read?pageNumber="
            : "/api/work-notes/read?beingId=" + beingId + "&pageNumber=";

        var cardBlock = Js.Block()
            .Add(() => Js.Const(() => "card", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "className"), () => Js.Str(() => "note-card")))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "onclick"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadNoteDetail").Invoke(() => Js.Id(() => "note").Prop(() => "pageNumber")))))
            .Add(() => Js.Let(() => "html", () => Js.Str(() => "<div class='note-title'>Page ").Op(() => "+", () => Js.Id(() => "note").Prop(() => "pageNumber")).Op(() => "+", () => Js.Str(() => "</div>"))))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Op(() => "+", () => Js.Str(() => "<div class='note-summary'>").Op(() => "+", () => Js.Id(() => "note").Prop(() => "summary")).Op(() => "+", () => Js.Str(() => "</div>")))))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Op(() => "+", () => Js.Str(() => "<div class='note-meta'>Updated: ").Op(() => "+", () => Js.Id(() => "note").Prop(() => "updatedAt")).Op(() => "+", () => Js.Str(() => "</div>")))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "innerHTML"), () => Js.Id(() => "html")))
            .Add(() => Js.Id(() => "grid").Call(() => "appendChild", () => Js.Id(() => "card")).Stmt());
    
        var loadNotesThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.Const(() => "grid", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "notes-grid"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "grid").Prop(() => "innerHTML"), () => Js.Str(() => "")).Stmt(),
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "total-pages")).Prop(() => "textContent"), () => Js.Id(() => "result").Prop(() => "total")).Stmt(),
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "result").Prop(() => "data").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                        {
                            Js.Assign(() => Js.Id(() => "grid").Prop(() => "innerHTML"), () => Js.Str(() => $"<div class='empty-state'>{loc.WorkNotesEmptyState}</div>")).Stmt()
                        }),
                        (null, new List<JsSyntax>
                        {
                            Js.Id(() => "result").Prop(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "note" }, () => (JsSyntax)cardBlock)).Stmt()
                        })
                    })
                }),
                (null, new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "notes-grid")).Prop(() => "innerHTML"), () => Js.Str(() => "<div class='empty-state'>Failed to load notes: </div>").Op(() => "+", () => Js.Id(() => "result").Prop(() => "error").Op(() => "||", () => Js.Str(() => "Unknown error")))).Stmt()
                })
            }));
    
        // Markdown rendering - render content area with marked library
        var renderMarkdownBodyBody = Js.Block()
            .Add(() => Js.Const(() => "elements", () => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => ".markdown-body[data-md-raw]"))))
            .Add(() => Js.Id(() => "elements").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "el" }, () => Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "el").Prop(() => "dataset").Prop(() => "mdRaw").Op(() => "&&", () => Js.Id(() => "el").Prop(() => "dataset").Prop(() => "mdRendered").Not()), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "el").Prop(() => "innerHTML"), () => Js.Ternary(
                            () => Js.Id(() => "typeof").Invoke(() => Js.Id(() => "marked")).Op(() => "!==", () => Js.Str(() => "undefined")),
                            () => Js.Id(() => "marked").Call(() => "parse", () => Js.Id(() => "el").Prop(() => "dataset").Prop(() => "mdRaw")),
                            () => Js.Id(() => "el").Prop(() => "dataset").Prop(() => "mdRaw"))),
                        Js.Assign(() => Js.Id(() => "el").Prop(() => "dataset").Prop(() => "mdRendered"), () => Js.Str(() => "1"))
                    })
                })))).Stmt());
    
        // Marked.js lazy loading and render Markdown body
        var markedLoadCheck = Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            (Js.Id(() => "typeof").Invoke(() => Js.Id(() => "marked")).Op(() => "===", () => Js.Str(() => "undefined")), new List<JsSyntax>
            {
                Js.Const(() => "mdScript", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "script"))),
                Js.Assign(() => Js.Id(() => "mdScript").Prop(() => "src"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/marked@15.0.12/marked.min.js")),
                Js.Assign(() => Js.Id(() => "mdScript").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "renderNoteDetailMarkdown").Invoke())),
                Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "mdScript")).Stmt()
            })
        });
    
        // Build close button click handler
        var closeBtnHandler = Js.Arrow(() => new List<string>(), () => (JsSyntax)Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "note-detail")).Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "active")).Stmt()));
    
        var loadNoteDetailThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.Const(() => "note", () => Js.Id(() => "result").Prop(() => "data")).Stmt(),
                    Js.Const(() => "panel", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "note-detail"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "panel").Prop(() => "innerHTML"), () => Js.Str(() => "")).Stmt(),
                    Js.Id(() => "panel").Prop(() => "classList").Call(() => "add", () => Js.Str(() => "active")).Stmt(),
                    // Header: title + close button
                    Js.Const(() => "header", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "header").Prop(() => "className"), () => Js.Str(() => "note-detail-header")).Stmt(),
                    Js.Const(() => "title", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "title").Prop(() => "className"), () => Js.Str(() => "note-detail-title")).Stmt(),
                    Js.Assign(() => Js.Id(() => "title").Prop(() => "textContent"), () => Js.Str(() => "Page ").Op(() => "+", () => Js.Id(() => "note").Prop(() => "pageNumber"))).Stmt(),
                    Js.Const(() => "closeBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "closeBtn").Prop(() => "className"), () => Js.Str(() => "note-detail-close")).Stmt(),
                    Js.Assign(() => Js.Id(() => "closeBtn").Prop(() => "textContent"), () => Js.Str(() => "\u00d7")).Stmt(),
                    Js.Id(() => "closeBtn").Call(() => "addEventListener", () => Js.Str(() => "click"), () => closeBtnHandler).Stmt(),
                    Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "title")).Stmt(),
                    Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "closeBtn")).Stmt(),
                    Js.Id(() => "panel").Call(() => "appendChild", () => Js.Id(() => "header")).Stmt(),
                    // Meta info
                    Js.Const(() => "meta", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "meta").Prop(() => "className"), () => Js.Str(() => "note-detail-meta")).Stmt(),
                    Js.Assign(() => Js.Id(() => "meta").Prop(() => "innerHTML"), () => Js.Str(() => "<p><strong>Summary:</strong> </p>").Op(() => "+", () => Js.Str(() => "<p><strong>Keywords:</strong> </p>")).Op(() => "+", () => Js.Str(() => "<p><strong>Updated:</strong> </p>"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "meta").Call(() => "querySelector", () => Js.Str(() => "p")).Prop(() => "innerHTML"), () => Js.Str(() => "<strong>Summary:</strong> ").Op(() => "+", () => Js.Id(() => "note").Prop(() => "summary"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "meta").Call(() => "querySelectorAll", () => Js.Str(() => "p")).Index(() => Js.Num(() => "1")).Prop(() => "innerHTML"), () => Js.Str(() => "<strong>Keywords:</strong> ").Op(() => "+", () => Js.Id(() => "note").Prop(() => "keywords"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "meta").Call(() => "querySelectorAll", () => Js.Str(() => "p")).Index(() => Js.Num(() => "2")).Prop(() => "innerHTML"), () => Js.Str(() => "<strong>Updated:</strong> ").Op(() => "+", () => Js.Id(() => "note").Prop(() => "updatedAt"))).Stmt(),
                    Js.Id(() => "panel").Call(() => "appendChild", () => Js.Id(() => "meta")).Stmt(),
                    // Content container with markdown-body
                    Js.Const(() => "contentWrap", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "contentWrap").Prop(() => "className"), () => Js.Str(() => "note-detail-content")).Stmt(),
                    Js.Const(() => "mdBody", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "mdBody").Prop(() => "className"), () => Js.Str(() => "markdown-body")).Stmt(),
                    Js.Assign(() => Js.Id(() => "mdBody").Prop(() => "dataset").Prop(() => "mdRaw"), () => Js.Id(() => "note").Prop(() => "content")).Stmt(),
                    Js.Id(() => "contentWrap").Call(() => "appendChild", () => Js.Id(() => "mdBody")).Stmt(),
                    Js.Id(() => "panel").Call(() => "appendChild", () => Js.Id(() => "contentWrap")).Stmt(),
                    // Render markdown
                    Js.Id(() => "renderNoteDetailMarkdown").Invoke().Stmt()
                })
            }));
    
        return Js.Block()
            .Add(() => Js.Const(() => "beingId", () => Js.Str(() => beingId.ToString())))
            .Add(() => Js.Func(() => "renderNoteDetailMarkdown", () => new List<string>(), () => renderMarkdownBodyBody))
            .Add(() => markedLoadCheck)
            .Add(() => Js.Func(() => "loadNotes", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => listUrl)).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)loadNotesThenBody)).Stmt())))
            .Add(() => Js.Func(() => "loadNoteDetail", () => new List<string> { "pageNumber" }, () => Js.Block()
                .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => readUrlPrefix).Op(() => "+", () => Js.Id(() => "pageNumber"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)loadNoteDetailThenBody)).Stmt())))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadNotes").Invoke())));
    }
}
