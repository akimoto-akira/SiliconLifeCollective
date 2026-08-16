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

using SiliconLife.App.Web.Component;
using SiliconLife.App.Web.Models;

namespace SiliconLife.App.Web.Views;

/// <summary>
/// Skill management page: skill list (left) + Markdown editor (right).
/// The editor saves through /api/skills/update-md; the skill id is taken
/// from the YAML front matter, so the built-in save button upserts skills.
/// </summary>
public class SkillView : ViewBase
{
    private const string EditorId = "skillEditor"; // camelCase: valid JS identifier

    public override string Render(object model)
    {
        var vm = model as SkillViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleSkills, "beings", vm.Localization, body, GetScripts(vm), GetStyles(), helpTopicId: "skills");
    }

    private static H RenderBody(SkillViewModel vm)
    {
        var editorWidget = MarkdownEditorComponent.RenderWidget(
            EditorId,
            string.Empty,
            $"skills.md ({vm.BeingName})",
            readOnly: false,
            initialMode: "edit",
            saveEndpoint: $"/api/skills/update-md?beingId={vm.BeingId}"
        );

        return H.Div(
            H.Div(
                H.A(vm.Localization.SkillsBackToBeings).Href("/beings").Class("back-link"),
                H.H1(vm.Localization.SkillsPageHeader).Class("skills-title"),
                H.P(vm.Localization.SkillsPageSubtitle).Class("skills-subtitle"),
                H.Span(string.Empty).Id("skill-stat").Class("stat-value")
            ).Class("page-header"),
            H.Div(
                H.Button(vm.Localization.SkillBtnNew).Id("btn-new-skill").Class("toolbar-btn primary"),
                H.Button(vm.Localization.SkillBtnImportMd).Id("btn-import-md").Class("toolbar-btn"),
                H.Button(vm.Localization.SkillBtnImportJson).Id("btn-import-json").Class("toolbar-btn"),
                H.Button(vm.Localization.SkillBtnRefresh).Id("btn-refresh-skills").Class("toolbar-btn")
            ).Class("skills-toolbar"),
            H.Div(
                H.Div().Id("skill-list").Class("skill-list"),
                H.Div(
                    H.Div(string.Empty).Id("current-skill-label").Class("current-skill-label"),
                    editorWidget
                ).Class("skill-editor-pane")
            ).Class("skills-layout")
        ).Class("page-content skills-page");
    }

    private static CssBuilder GetStyles()
    {
        return MarkdownEditorComponent.GetWidgetStyles()
            .Selector(".skills-page .page-header")
                .Property("margin-bottom", "16px")
            .EndSelector()
            .Selector(".back-link")
                .Property("display", "inline-block")
                .Property("margin-bottom", "12px")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("font-size", "14px")
                .Property("font-weight", "500")
            .EndSelector()
            .Selector(".back-link:hover")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".skills-title")
                .Property("font-size", "28px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin", "0 0 6px 0")
            .EndSelector()
            .Selector(".skills-subtitle")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("margin", "0 0 8px 0")
            .EndSelector()
            .Selector(".stat-value")
                .Property("font-size", "13px")
                .Property("font-weight", "bold")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".skills-toolbar")
                .Property("display", "flex")
                .Property("gap", "10px")
                .Property("margin-bottom", "16px")
            .EndSelector()
            .Selector(".toolbar-btn")
                .Property("padding", "8px 14px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("font-size", "13px")
                .Property("transition", "all 0.15s")
            .EndSelector()
            .Selector(".toolbar-btn:hover")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".toolbar-btn.primary")
                .Property("background", "var(--accent-primary)")
                .Property("color", "white")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".skills-layout")
                .Property("display", "flex")
                .Property("gap", "16px")
                .Property("align-items", "flex-start")
            .EndSelector()
            .Selector(".skill-list")
                .Property("flex", "0 0 340px")
                .Property("max-height", "calc(100vh - 220px)")
                .Property("overflow-y", "auto")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "12px")
            .EndSelector()
            .Selector(".skill-card")
                .Property("background", "var(--bg-card)")
                .Property("padding", "14px")
                .Property("border-radius", "10px")
                .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".skill-card-title")
                .Property("font-size", "15px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "6px")
                .Property("word-break", "break-all")
            .EndSelector()
            .Selector(".skill-badge")
                .Property("display", "inline-block")
                .Property("font-size", "11px")
                .Property("font-weight", "normal")
                .Property("color", "var(--text-secondary)")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.05))")
                .Property("border-radius", "4px")
                .Property("padding", "2px 6px")
                .Property("margin-left", "8px")
            .EndSelector()
            .Selector(".skill-card-desc")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "10px")
                .Property("line-height", "1.5")
            .EndSelector()
            .Selector(".skill-card-actions")
                .Property("display", "flex")
                .Property("gap", "6px")
                .Property("flex-wrap", "wrap")
            .EndSelector()
            .Selector(".skill-btn")
                .Property("padding", "4px 10px")
                .Property("font-size", "12px")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "5px")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".skill-btn:hover")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".skill-btn.danger")
                .Property("color", "#d32f2f")
                .Property("border-color", "#d32f2f55")
            .EndSelector()
            .Selector(".skill-btn.danger:hover")
                .Property("background", "#d32f2f11")
            .EndSelector()
            .Selector(".skill-editor-pane")
                .Property("flex", "1")
                .Property("min-width", "0")
            .EndSelector()
            .Selector(".current-skill-label")
                .Property("font-size", "13px")
                .Property("color", "var(--accent-primary)")
                .Property("margin-bottom", "8px")
                .Property("min-height", "18px")
            .EndSelector()
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "40px")
                .Property("color", "var(--text-secondary)")
            .EndSelector();
    }

    private static JsSyntax GetScripts(SkillViewModel vm)
    {
        var loc = vm.Localization;
        string beingId = vm.BeingId.ToString();

        string listUrl = "/api/skills/list?beingId=" + beingId;
        string getMdUrlPrefix = "/api/skills/get-md?beingId=" + beingId + "&skillId=";
        string deleteUrl = "/api/skills/delete?beingId=" + beingId;
        string importMdUrl = "/api/skills/import-md?beingId=" + beingId;
        string importJsonUrl = "/api/skills/import?beingId=" + beingId;
        string testUrl = "/api/skills/test?beingId=" + beingId;
        string exportUrlPrefix = "/api/skills/export?beingId=" + beingId + "&skillId=";
        string exportMdUrlPrefix = "/api/skills/export-md?beingId=" + beingId + "&skillId=";

        string templateMarkdown = string.Join("\n", new[]
        {
            "---",
            "id: my_new_skill",
            "description: Describe what this skill does in one sentence.",
            "version: 1.0.0",
            "tags: []",
            "tool_whitelist: []",
            "max_tool_round: 5",
            "timeout: 60s",
            "on_complete: write_memory",
            "trigger_mode: manual",
            "---",
            "",
            "# My New Skill",
            "",
            "You are a helpful assistant. Use the {param} placeholder to receive skill arguments.",
            "Describe the task steps and expected output format here.",
        });

        // ----- skill card builder (runs per skill inside forEach) -----
        var cardBlock = Js.Block()
            // card + title (with badge)
            .Add(() => Js.Const(() => "card", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "className"), () => Js.Str(() => "skill-card")))
            .Add(() => Js.Const(() => "titleEl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "titleEl").Prop(() => "className"), () => Js.Str(() => "skill-card-title")))
            .Add(() => Js.Assign(() => Js.Id(() => "titleEl").Prop(() => "textContent"), () => Js.Id(() => "skill").Prop(() => "id")))
            .Add(() => Js.Const(() => "badge", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "span"))))
            .Add(() => Js.Assign(() => Js.Id(() => "badge").Prop(() => "className"), () => Js.Str(() => "skill-badge")))
            .Add(() => Js.Assign(() => Js.Id(() => "badge").Prop(() => "textContent"),
                () => Js.Id(() => "skill").Prop(() => "version").Op(() => "+", () => Js.Str(() => " · "))
                    .Op(() => "+", () => Js.Id(() => "skill").Prop(() => "source"))
                    .Op(() => "+", () => Js.Str(() => " · "))
                    .Op(() => "+", () => Js.Id(() => "skill").Prop(() => "triggerMode"))))
            .Add(() => Js.Id(() => "titleEl").Call(() => "appendChild", () => Js.Id(() => "badge")).Stmt())
            .Add(() => Js.Id(() => "card").Call(() => "appendChild", () => Js.Id(() => "titleEl")).Stmt())
            // description
            .Add(() => Js.Const(() => "descEl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "descEl").Prop(() => "className"), () => Js.Str(() => "skill-card-desc")))
            .Add(() => Js.Assign(() => Js.Id(() => "descEl").Prop(() => "textContent"),
                () => Js.Id(() => "skill").Prop(() => "description").Op(() => "||", () => Js.Str(() => ""))))
            .Add(() => Js.Id(() => "card").Call(() => "appendChild", () => Js.Id(() => "descEl")).Stmt())
            // action buttons
            .Add(() => Js.Const(() => "actions", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "actions").Prop(() => "className"), () => Js.Str(() => "skill-card-actions")))
            // edit
            .Add(() => Js.Const(() => "editBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "editBtn").Prop(() => "className"), () => Js.Str(() => "skill-btn")))
            .Add(() => Js.Assign(() => Js.Id(() => "editBtn").Prop(() => "textContent"), () => Js.Str(() => loc.SkillBtnEdit)))
            .Add(() => Js.Id(() => "editBtn").Call(() => "addEventListener", () => Js.Str(() => "click"),
                () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "editSkill").Invoke(() => Js.Id(() => "skill").Prop(() => "id")))).Stmt())
            .Add(() => Js.Id(() => "actions").Call(() => "appendChild", () => Js.Id(() => "editBtn")).Stmt())
            // test
            .Add(() => Js.Const(() => "testBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "testBtn").Prop(() => "className"), () => Js.Str(() => "skill-btn")))
            .Add(() => Js.Assign(() => Js.Id(() => "testBtn").Prop(() => "textContent"), () => Js.Str(() => loc.SkillBtnTest)))
            .Add(() => Js.Id(() => "testBtn").Call(() => "addEventListener", () => Js.Str(() => "click"),
                () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "testSkill").Invoke(() => Js.Id(() => "skill").Prop(() => "id")))).Stmt())
            .Add(() => Js.Id(() => "actions").Call(() => "appendChild", () => Js.Id(() => "testBtn")).Stmt())
            // export json
            .Add(() => Js.Const(() => "jsonBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "jsonBtn").Prop(() => "className"), () => Js.Str(() => "skill-btn")))
            .Add(() => Js.Assign(() => Js.Id(() => "jsonBtn").Prop(() => "textContent"), () => Js.Str(() => loc.SkillBtnExportJson)))
            .Add(() => Js.Id(() => "jsonBtn").Call(() => "addEventListener", () => Js.Str(() => "click"),
                () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "exportJsonSkill").Invoke(() => Js.Id(() => "skill").Prop(() => "id")))).Stmt())
            .Add(() => Js.Id(() => "actions").Call(() => "appendChild", () => Js.Id(() => "jsonBtn")).Stmt())
            // export markdown
            .Add(() => Js.Const(() => "mdBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "mdBtn").Prop(() => "className"), () => Js.Str(() => "skill-btn")))
            .Add(() => Js.Assign(() => Js.Id(() => "mdBtn").Prop(() => "textContent"), () => Js.Str(() => loc.SkillBtnExportMd)))
            .Add(() => Js.Id(() => "mdBtn").Call(() => "addEventListener", () => Js.Str(() => "click"),
                () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "exportMdSkill").Invoke(() => Js.Id(() => "skill").Prop(() => "id")))).Stmt())
            .Add(() => Js.Id(() => "actions").Call(() => "appendChild", () => Js.Id(() => "mdBtn")).Stmt())
            // delete
            .Add(() => Js.Const(() => "delBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "delBtn").Prop(() => "className"), () => Js.Str(() => "skill-btn danger")))
            .Add(() => Js.Assign(() => Js.Id(() => "delBtn").Prop(() => "textContent"), () => Js.Str(() => loc.SkillBtnDelete)))
            .Add(() => Js.Id(() => "delBtn").Call(() => "addEventListener", () => Js.Str(() => "click"),
                () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "deleteSkill").Invoke(() => Js.Id(() => "skill").Prop(() => "id")))).Stmt())
            .Add(() => Js.Id(() => "actions").Call(() => "appendChild", () => Js.Id(() => "delBtn")).Stmt())
            .Add(() => Js.Id(() => "card").Call(() => "appendChild", () => Js.Id(() => "actions")).Stmt())
            .Add(() => Js.Id(() => "list").Call(() => "appendChild", () => Js.Id(() => "card")).Stmt());

        // ----- loadSkills: render list + stats -----
        var loadThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "skill-list"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "")).Stmt(),
                    Js.Const(() => "customCount", () => Js.Id(() => "result").Prop(() => "data").Call(() => "filter",
                        () => Js.Arrow(() => new List<string> { "s" }, () => (JsSyntax)Js.Id(() => "s").Prop(() => "source").Op(() => "===", () => Js.Str(() => "being"))
                            .Op(() => "||", () => Js.Id(() => "s").Prop(() => "source").Op(() => "===", () => Js.Str(() => "user"))))).Prop(() => "length")).Stmt(),
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "skill-stat")).Prop(() => "textContent"),
                        () => Js.Id(() => "statFormat")
                            .Call(() => "replace", () => Js.Str(() => "{0}"), () => Js.Id(() => "result").Prop(() => "total"))
                            .Call(() => "replace", () => Js.Str(() => "{1}"), () => Js.Id(() => "customCount"))
                            .Call(() => "replace", () => Js.Str(() => "{2}"), () => Js.Str(() => vm.MaxCustomSkills.ToString()))).Stmt(),
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "result").Prop(() => "data").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                        {
                            Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"),
                                () => Js.Str(() => $"<div class='empty-state'>{loc.SkillsEmptyState}</div>")).Stmt()
                        }),
                        (null, new List<JsSyntax>
                        {
                            Js.Id(() => "result").Prop(() => "data").Call(() => "forEach",
                                () => Js.Arrow(() => new List<string> { "skill" }, () => (JsSyntax)cardBlock)).Stmt()
                        })
                    })
                }),
                (null, new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "skill-list")).Prop(() => "innerHTML"),
                        () => Js.Str(() => $"<div class='empty-state'>{loc.SkillsEmptyState}</div>")).Stmt(),
                    Js.Id(() => "window").Call(() => "alert",
                        () => Js.Str(() => loc.SkillLoadFailedFormat).Call(() => "replace", () => Js.Str(() => "{0}"),
                            () => Js.Id(() => "result").Prop(() => "error").Op(() => "||", () => Js.Str(() => "")))).Stmt()
                })
            }));

        // ----- editSkill: load markdown into editor -----
        var editThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.Const(() => "state", () => Js.Id(() => "window").Index(() => Js.Str(() => EditorId))).Stmt(),
                    // Guard against uninitialised editor state; fall back to the
                    // plain textarea when CodeMirror libraries never loaded
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "state"), new List<JsSyntax>
                        {
                            Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                            {
                                (Js.Id(() => "state").Prop(() => "editor"), new List<JsSyntax>
                                {
                                    Js.Id(() => "state").Prop(() => "editor").Call(() => "setValue", () => Js.Id(() => "result").Prop(() => "data").Prop(() => "markdown")).Stmt()
                                }),
                                (null, new List<JsSyntax>
                                {
                                    Js.Assign(() => Js.Id(() => "state").Prop(() => "textarea").Prop(() => "value"),
                                        () => Js.Id(() => "result").Prop(() => "data").Prop(() => "markdown")).Stmt()
                                })
                            })
                        })
                    }),
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "current-skill-label")).Prop(() => "textContent"),
                        () => Js.Str(() => loc.SkillCurrentEditingFormat).Call(() => "replace", () => Js.Str(() => "{0}"),
                            () => Js.Id(() => "result").Prop(() => "data").Prop(() => "skillId"))).Stmt()
                }),
                (null, new List<JsSyntax>
                {
                    Js.Id(() => "window").Call(() => "alert",
                        () => Js.Str(() => loc.SkillLoadFailedFormat).Call(() => "replace", () => Js.Str(() => "{0}"),
                            () => Js.Id(() => "result").Prop(() => "error").Op(() => "||", () => Js.Str(() => "")))).Stmt()
                })
            }));

        // ----- shared POST helper: postJson(url, bodyObj) -> refresh list on success -----
        var postJsonThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "result").Prop(() => "data").Prop(() => "message"), new List<JsSyntax>
                        {
                            Js.Id(() => "window").Call(() => "alert", () => Js.Id(() => "result").Prop(() => "data").Prop(() => "message")).Stmt()
                        })
                    }),
                    Js.Id(() => "loadSkills").Invoke().Stmt()
                }),
                (null, new List<JsSyntax>
                {
                    Js.Id(() => "window").Call(() => "alert",
                        () => Js.Id(() => "result").Prop(() => "error").Op(() => "||", () => Js.Str(() => loc.SkillRequestFailed))).Stmt()
                })
            }));

        var postJson = Js.Func(() => "postJson", () => new List<string> { "url", "bodyObj" }, () => Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Id(() => "url"), () => Js.Obj()
                .Prop(() => "method", () => Js.Str(() => "POST"))
                .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "bodyObj"))))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)postJsonThenBody))
                .Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => (JsSyntax)Js.Block()
                    .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Request failed:"), () => Js.Id(() => "err")).Stmt())
                    .Add(() => Js.Id(() => "window").Call(() => "alert",
                        () => Js.Str(() => loc.SkillRequestFailed).Op(() => "+", () => Js.Str(() => " ")).Op(() => "+", () => Js.Id(() => "err"))).Stmt())))
                .Stmt()));

        // ----- testSkill: prompt for parameter JSON, run, alert result -----
        var testThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.Id(() => "window").Call(() => "alert",
                        () => Js.Str(() => loc.SkillTestResultPrefix).Op(() => "+",
                            () => Js.Id(() => "result").Prop(() => "data").Prop(() => "message").Op(() => "||", () => Js.Str(() => "")))).Stmt()
                }),
                (null, new List<JsSyntax>
                {
                    Js.Id(() => "window").Call(() => "alert",
                        () => Js.Str(() => loc.SkillTestResultPrefix).Op(() => "+",
                            () => Js.Id(() => "result").Prop(() => "error").Op(() => "||", () => Js.Str(() => "")))).Stmt()
                })
            }));

        var testSkillFunc = Js.Func(() => "testSkill", () => new List<string> { "skillId" }, () => Js.Block()
            .Add(() => Js.Const(() => "raw", () => Js.Id(() => "window").Call(() => "prompt", () => Js.Str(() => loc.SkillPromptTestParams))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "raw").Op(() => "===", () => Js.Null()), new List<JsSyntax>
                {
                    Js.Return(() => Js.Null())
                })
            }))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => testUrl), () => Js.Obj()
                .Prop(() => "method", () => Js.Str(() => "POST"))
                .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj()
                    .Prop(() => "skillId", () => Js.Id(() => "skillId"))
                    .Prop(() => "parametersJson", () => Js.Id(() => "raw").Call(() => "trim")))))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)testThenBody))
                .Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => (JsSyntax)Js.Block()
                    .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Test failed:"), () => Js.Id(() => "err")).Stmt())))
                .Stmt()));

        var editorScripts = MarkdownEditorComponent.GetWidgetScripts(
            EditorId,
            initialContent: string.Empty,
            readOnly: false,
            saveEndpoint: $"/api/skills/update-md?beingId={beingId}"
        );

        return Js.Block()
            // constants
            .Add(() => Js.Const(() => "statFormat", () => Js.Str(() => loc.SkillsStatFormat)))
            .Add(() => Js.Const(() => "newSkillLabel", () => Js.Str(() => loc.SkillNewSkillLabel)))
            .Add(() => Js.Const(() => "templateMarkdown", () => Js.Str(() => templateMarkdown)))
            // loadSkills
            .Add(() => Js.Func(() => "loadSkills", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => listUrl))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json")))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)loadThenBody))
                    .Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => (JsSyntax)Js.Block()
                        .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Failed to load skills:"), () => Js.Id(() => "err")).Stmt()))).Stmt())))
            // editSkill
            .Add(() => Js.Func(() => "editSkill", () => new List<string> { "skillId" }, () => Js.Block()
                .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => getMdUrlPrefix).Op(() => "+", () => Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "skillId"))))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json")))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)editThenBody))
                    .Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => (JsSyntax)Js.Block()
                        .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Failed to load skill:"), () => Js.Id(() => "err")).Stmt()))).Stmt())))
            // newSkill
            .Add(() => Js.Func(() => "newSkill", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Const(() => "state", () => Js.Id(() => "window").Index(() => Js.Str(() => EditorId))))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "state"), new List<JsSyntax>
                    {
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (Js.Id(() => "state").Prop(() => "editor"), new List<JsSyntax>
                            {
                                Js.Id(() => "state").Prop(() => "editor").Call(() => "setValue", () => Js.Id(() => "templateMarkdown")).Stmt()
                            }),
                            (null, new List<JsSyntax>
                            {
                                Js.Assign(() => Js.Id(() => "state").Prop(() => "textarea").Prop(() => "value"), () => Js.Id(() => "templateMarkdown")).Stmt()
                            })
                        })
                    })
                }))
                .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "current-skill-label")).Prop(() => "textContent"), () => Js.Id(() => "newSkillLabel")).Stmt())))
            // import markdown (file picker)
            .Add(() => Js.Func(() => "importMarkdown", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Const(() => "input", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
                .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "type"), () => Js.Str(() => "file")))
                .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "accept"), () => Js.Str(() => ".md,text/markdown")))
                .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "onchange"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                    .Add(() => Js.Const(() => "file", () => Js.Id(() => "input").Prop(() => "files").Index(() => Js.Num(() => "0"))))
                    .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "file").Not(), new List<JsSyntax>
                        {
                            Js.Return(() => Js.Null())
                        })
                    }))
                    .Add(() => Js.Const(() => "reader", () => Js.New(() => Js.Id(() => "FileReader"))))
                    .Add(() => Js.Assign(() => Js.Id(() => "reader").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                        .Add(() => Js.Id(() => "postJson").Invoke(() => Js.Str(() => importMdUrl), () => Js.Obj().Prop(() => "markdown", () => Js.Id(() => "reader").Prop(() => "result"))).Stmt()))))
                    .Add(() => Js.Id(() => "reader").Call(() => "readAsText", () => Js.Id(() => "file")).Stmt()))))
                .Add(() => Js.Id(() => "input").Call(() => "click").Stmt())))
            // import json (file picker)
            .Add(() => Js.Func(() => "importJsonSkill", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Const(() => "input", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
                .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "type"), () => Js.Str(() => "file")))
                .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "accept"), () => Js.Str(() => ".json,application/json")))
                .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "onchange"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                    .Add(() => Js.Const(() => "file", () => Js.Id(() => "input").Prop(() => "files").Index(() => Js.Num(() => "0"))))
                    .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "file").Not(), new List<JsSyntax>
                        {
                            Js.Return(() => Js.Null())
                        })
                    }))
                    .Add(() => Js.Const(() => "reader", () => Js.New(() => Js.Id(() => "FileReader"))))
                    .Add(() => Js.Assign(() => Js.Id(() => "reader").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                        .Add(() => Js.Id(() => "postJson").Invoke(() => Js.Str(() => importJsonUrl), () => Js.Obj().Prop(() => "json", () => Js.Id(() => "reader").Prop(() => "result"))).Stmt()))))
                    .Add(() => Js.Id(() => "reader").Call(() => "readAsText", () => Js.Id(() => "file")).Stmt()))))
                .Add(() => Js.Id(() => "input").Call(() => "click").Stmt())))
            // delete
            .Add(() => Js.Func(() => "deleteSkill", () => new List<string> { "skillId" }, () => Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "window").Call(() => "confirm",
                        () => Js.Str(() => loc.SkillConfirmDeleteFormat).Call(() => "replace", () => Js.Str(() => "{0}"), () => Js.Id(() => "skillId"))).Not(), new List<JsSyntax>
                    {
                        Js.Return(() => Js.Null())
                    })
                }))
                .Add(() => Js.Id(() => "postJson").Invoke(() => Js.Str(() => deleteUrl), () => Js.Obj().Prop(() => "skillId", () => Js.Id(() => "skillId"))).Stmt())))
            // exports
            .Add(() => Js.Func(() => "exportJsonSkill", () => new List<string> { "skillId" }, () => Js.Block()
                .Add(() => Js.Id(() => "window").Call(() => "open", () => Js.Str(() => exportUrlPrefix).Op(() => "+", () => Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "skillId"))), () => Js.Str(() => "_blank")).Stmt())))
            .Add(() => Js.Func(() => "exportMdSkill", () => new List<string> { "skillId" }, () => Js.Block()
                .Add(() => Js.Id(() => "window").Call(() => "open", () => Js.Str(() => exportMdUrlPrefix).Op(() => "+", () => Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "skillId"))), () => Js.Str(() => "_blank")).Stmt())))
            // shared helper + test
            .Add(() => postJson)
            .Add(() => testSkillFunc)
            // toolbar bindings + initial load
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "document").Prop(() => "readyState").Op(() => "===", () => Js.Str(() => "loading")), new List<JsSyntax>
                {
                    Js.Id(() => "document").Call(() => "addEventListener", () => Js.Str(() => "DOMContentLoaded"), () => Js.Id(() => "bindToolbar")).Stmt()
                }),
                (null, new List<JsSyntax>
                {
                    Js.Id(() => "bindToolbar").Invoke().Stmt()
                })
            }))
            .Add(() => Js.Func(() => "bindToolbar", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btn-new-skill")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Id(() => "newSkill")).Stmt())
                .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btn-import-md")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Id(() => "importMarkdown")).Stmt())
                .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btn-import-json")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Id(() => "importJsonSkill")).Stmt())
                .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btn-refresh-skills")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Id(() => "loadSkills")).Stmt())
                .Add(() => Js.Id(() => "loadSkills").Invoke().Stmt())))
            // markdown editor widget scripts
            .Add(() => editorScripts);
    }
}
