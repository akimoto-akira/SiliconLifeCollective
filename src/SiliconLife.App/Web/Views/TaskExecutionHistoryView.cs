using SiliconLife.App.Web.Models;

namespace SiliconLife.App.Web.Views;

public class TaskExecutionHistoryView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as TaskExecutionHistoryViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.TaskExecutionHistoryTitle, "task-cycle-history", vm.Localization, body, GetScripts(vm), GetStyles(), "task");
    }

    private static H RenderBody(TaskExecutionHistoryViewModel vm)
    {
        return H.Div(
            H.Div(
                H.A(vm.Localization.TaskExecutionBackToTasks).Href("/tasks").Class("back-link"),
                H.H1(vm.Localization.TaskExecutionHistoryHeader),
                H.P(string.Format(vm.Localization.TaskExecutionTaskName, vm.TaskName)).Class("page-subtitle")
            ).Class("page-header"),
            H.Div().Id("execution-list").Class("execution-list")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".back-link")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("font-weight", "bold")
                .Property("transition", "color 0.2s")
            .EndSelector()
            .Selector(".back-link:hover")
                .Property("color", "var(--accent-secondary, var(--accent-primary))")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".page-subtitle")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-top", "8px")
            .EndSelector()
            .Selector(".execution-list")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "12px")
                .Property("margin-top", "20px")
            .EndSelector()
            .Selector(".execution-item")
                .Property("background", "var(--bg-card)")
                .Property("padding", "16px")
                .Property("border-radius", "8px")
                .Property("border", "1px solid var(--border)")
                .Property("cursor", "pointer")
                .Property("transition", "transform 0.2s, box-shadow 0.2s")
            .EndSelector()
            .Selector(".execution-item:hover")
                .Property("transform", "translateY(-2px)")
                .Property("box-shadow", "0 4px 12px rgba(0,0,0,0.1)")
            .EndSelector()
            .Selector(".execution-header")
                .Property("display", "flex")
                .Property("justify-content", "space-between")
                .Property("align-items", "center")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".execution-state")
                .Property("display", "inline-block")
                .Property("padding", "4px 12px")
                .Property("border-radius", "12px")
                .Property("font-size", "12px")
            .EndSelector()
            .Selector(".execution-state.pending")
                .Property("background", "rgba(245,158,11,0.15)")
                .Property("color", "#f59e0b")
            .EndSelector()
            .Selector(".execution-state.running")
                .Property("background", "rgba(59,130,246,0.15)")
                .Property("color", "#3b82f6")
            .EndSelector()
            .Selector(".execution-state.completed")
                .Property("background", "rgba(16,185,129,0.15)")
                .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".execution-state.failed")
                .Property("background", "rgba(239,68,68,0.15)")
                .Property("color", "var(--accent-error, #ef4444)")
            .EndSelector()
            .Selector(".execution-state.cancelled")
                .Property("background", "rgba(107,114,128,0.15)")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".execution-info")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "4px")
            .EndSelector()
            .Selector(".execution-info-label")
                .Property("font-weight", "bold")
                .Property("margin-right", "8px")
            .EndSelector()
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "40px")
                .Property("color", "var(--text-secondary)")
            .EndSelector();
    }

    private static JsSyntax GetScripts(TaskExecutionHistoryViewModel vm)
    {
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "item", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Const(() => "stateClass", () => Js.Id(() => "e").Prop(() => "state").Call(() => "toLowerCase")))
            .Add(() => Js.Const(() => "stateText", () => Js.Id(() => "e").Prop(() => "state").Call(() => "charAt", () => Js.Num(() => "0")).Call(() => "toUpperCase").Call(() => "concat", () => Js.Id(() => "e").Prop(() => "state").Call(() => "slice", () => Js.Num(() => "1")))))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "className"), () => Js.Str(() => "execution-item")))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "onclick"), () => Js.Arrow(
                () => new List<string>(),
                () => Js.Assign(
                    () => Js.Id(() => "window").Prop(() => "location"),
                    () => Js.Str(() => "/task-cycle/")
                        .Op(() => "+", () => Js.Id(() => "e").Prop(() => "cycleIndex"))
                        .Op(() => "+", () => Js.Str(() => "?taskId="))
                        .Op(() => "+", () => Js.Id(() => "taskId"))
                )
            )))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "innerHTML"), () => BuildItemHtml()))
            .Add(() => Js.Id(() => "list").Call(() => "appendChild", () => Js.Id(() => "item")).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "execution-list"))))
            .Add(() => Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "data").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "<div class='empty-state'>")
                        .Op(() => "+", () => (JsSyntax)Js.Id(() => "emptyMessage"))
                        .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>")))
                }),
                (null, new List<JsSyntax>
                {
                    Js.Id(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "e" }, () => forEachBody)).Stmt()
                })
            }));

        var loadBody = Js.Block()
            .Add(() => Js.Const(() => "emptyMessage", () => Js.Str(() => vm.Localization.TaskExecutionNoRecords)))
            .Add(() => Js.Id(() => "fetch")
                .Invoke(() => Js.Str(() => "/api/task-cycles/list?taskId=").Op(() => "+", () => Js.Id(() => "taskId")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Const(() => "taskId", () => Js.Id(() => "window").Prop(() => "location").Prop(() => "pathname").Call(() => "split", () => Js.Str(() => "/")).Index(() => Js.Num(() => "2"))))
            .Add(() => Js.Func(() => "loadExecutions", () => new List<string>(), () => loadBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadExecutions").Invoke())));
    }

    private static JsSyntax BuildItemHtml()
    {
        return Js.Str(() => "<div class='execution-header'><span class='execution-state ")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "stateClass"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "stateText"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span></div><div class='execution-info'><span class='execution-info-label'>Started:</span>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "e").Prop(() => "startedAt"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"))
            .Op(() => "+", () => Js.Ternary(
                () => Js.Id(() => "e").Prop(() => "endedAt"),
                () => Js.Str(() => "<div class='execution-info'><span class='execution-info-label'>Ended:</span>")
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "e").Prop(() => "endedAt"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>")),
                () => Js.Str(() => "")
            ))
            .Op(() => "+", () => Js.Str(() => "<div class='execution-info'><span class='execution-info-label'>Rounds:</span>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "e").Prop(() => "roundCount"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => " | Messages: "))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "e").Prop(() => "messageCount"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"));
    }
}
