using SiliconLife.App.Web.Models;

namespace SiliconLife.App.Web.Views;

public class TaskExecutionDetailView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as TaskExecutionDetailViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        var scripts = GetScripts(vm);
        var styles = GetStyles();

        return RenderPage(vm.Skin, vm.Localization.TaskExecutionDetailTitle, "task-cycle-detail", vm.Localization, body, scripts, styles, "task");
    }

    private static H RenderBody(TaskExecutionDetailViewModel vm)
    {
        return H.Div(
            H.Div(
                H.A(vm.Localization.TaskExecutionBackToTasks).Href($"/task-cycles/{vm.TaskId}").Class("back-link"),
                H.H1(vm.Localization.TaskExecutionDetailHeader),
                H.P($"{string.Format(vm.Localization.TaskExecutionTaskName, vm.TaskName)} | Cycle #{vm.CycleIndex}").Class("page-subtitle")
            ).Class("page-header"),
            H.Div().Id("message-list").Class("message-list"),
            H.Div(
                H.Div("").Class("loading-spinner"),
                H.Div("Loading messages...").Class("loading-text")
            ).Id("loading-indicator").Class("loading-indicator")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return ChatHistoryDetailView.GetStylesInternal();
    }

    private static JsSyntax GetScripts(TaskExecutionDetailViewModel vm)
    {
        var apiUrl = $"/api/task-cycle/messages?cycleIndex={vm.CycleIndex}&taskId={vm.TaskId}";
        return ChatHistoryDetailView.GetScriptsStatic(vm.ToolDisplayNames, apiUrl, "No messages in this cycle");
    }
}
