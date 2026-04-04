// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Text;
using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

public abstract class ViewBase
{
    protected readonly StringBuilder Sb = new();

    public abstract string Render(object model);

    protected string EscapeHtml(string text)
    {
        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }

    protected string RenderPage(ISkin skin, string title, string activeMenu, string bodyContent,
        string? inlineScripts = null, string? inlineStyles = null)
    {
        var sb = new StringBuilder();
        var themeCss = skin.GetThemeCss().Build();
        var baseCss = skin.GetStyles().Build();

        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<meta charset=\"utf-8\">");
        sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        sb.AppendLine($"<title>{EscapeHtml(title)}</title>");
        sb.AppendLine($"<style>{baseCss}</style>");
        sb.AppendLine($"<style>{GetShellCss()}{GetCommonCss()}</style>");
        if (!string.IsNullOrEmpty(themeCss))
            sb.AppendLine($"<style>{themeCss}</style>");
        if (!string.IsNullOrEmpty(inlineStyles))
            sb.AppendLine($"<style>{inlineStyles}</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");

        sb.AppendLine("<div class=\"shell\">");
        sb.AppendLine(RenderHeader());
        sb.AppendLine("<div class=\"shell-body\">");
        sb.AppendLine(RenderSidebar(activeMenu));
        sb.AppendLine($"<main class=\"shell-content\">{bodyContent}</main>");
        sb.AppendLine("</div>");
        sb.AppendLine("</div>");

        if (!string.IsNullOrEmpty(inlineScripts))
            sb.AppendLine($"<script>{inlineScripts}</script>");

        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }

    private static string RenderHeader()
    {
        return @"
<header class=""shell-header"">
    <div class=""shell-brand"">🜲 硅基生命群</div>
    <div class=""shell-header-actions"">
        <a class=""shell-header-link"" href=""/config"">⚙</a>
    </div>
</header>";
    }

    private static string RenderSidebar(string activeMenu)
    {
        var items = new (string Id, string Icon, string Label, string Href)[]
        {
            ("chat", "💬", "聊天", "/chat"),
            ("dashboard", "📊", "仪表盘", "/dashboard"),
            ("beings", "🧠", "硅基人", "/beings"),
            ("tasks", "📋", "任务", "/tasks"),
            ("memory", "📂", "记忆", "/memory"),
            ("knowledge", "📚", "知识", "/knowledge"),
            ("projects", "📁", "项目", "/project"),
            ("logs", "📝", "日志", "/logs"),
            ("config", "⚙", "配置", "/config"),
        };

        var sb = new StringBuilder();
        sb.AppendLine("<aside class=\"shell-sidebar\">");
        foreach (var (id, icon, label, href) in items)
        {
            var active = id == activeMenu ? " active" : "";
            sb.AppendLine($"<a class=\"shell-menu-item{active}\" href=\"{href}\">");
            sb.AppendLine($"<span class=\"shell-menu-icon\">{icon}</span>");
            sb.AppendLine($"<span class=\"shell-menu-text\">{label}</span>");
            sb.AppendLine("</a>");
        }
        sb.AppendLine("</aside>");
        return sb.ToString();
    }

    protected static string GetShellCss()
    {
        return @"
.shell { display: flex; flex-direction: column; height: 100vh; overflow: hidden; background: var(--bg-primary); color: var(--text-primary); font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif; }
.shell-header { display: flex; align-items: center; justify-content: space-between; padding: 0 20px; height: 48px; background: var(--bg-card); border-bottom: 1px solid var(--border); flex-shrink: 0; }
.shell-brand { font-size: 16px; font-weight: bold; }
.shell-header-actions { display: flex; gap: 12px; align-items: center; }
.shell-header-link { color: var(--text-secondary); text-decoration: none; font-size: 18px; cursor: pointer; transition: color 0.2s; }
.shell-header-link:hover { color: var(--accent-primary); }
.shell-body { display: flex; flex: 1; overflow: hidden; }
.shell-sidebar { width: 200px; background: var(--bg-card); border-right: 1px solid var(--border); padding: 12px 0; overflow-y: auto; flex-shrink: 0; }
.shell-content { flex: 1; overflow: hidden; display: flex; flex-direction: column; }
.shell-menu-item { display: flex; align-items: center; gap: 10px; padding: 10px 20px; color: var(--text-secondary); text-decoration: none; font-size: 14px; cursor: pointer; transition: all 0.2s; border-left: 3px solid transparent; }
.shell-menu-item:hover { color: var(--text-primary); background: var(--bg-secondary, rgba(255,255,255,0.05)); }
.shell-menu-item.active { color: var(--accent-primary); border-left-color: var(--accent-primary); background: var(--bg-secondary, rgba(255,255,255,0.05)); }
.shell-menu-icon { width: 20px; text-align: center; flex-shrink: 0; }
@media (max-width: 768px) {
    .shell-sidebar { width: 60px; padding: 8px 0; }
    .shell-menu-text { display: none; }
    .shell-menu-item { justify-content: center; padding: 10px; }
    .shell-menu-icon { width: auto; }
}
@media (max-width: 480px) {
    .shell-sidebar { display: none; }
}
";
    }

    protected static string GetCommonCss()
    {
        return @"
.page-content { flex: 1; overflow-y: auto; padding: 24px; }
.page-header { margin-bottom: 24px; }
.page-header h1 { font-size: 24px; font-weight: 600; }
.page-header-actions { display: flex; gap: 10px; margin-top: 12px; }
.card { background: var(--bg-card); padding: 20px; border-radius: 12px; border: 1px solid var(--border); margin-bottom: 20px; }
.card h3 { font-size: 16px; margin-bottom: 12px; }
.btn { display: inline-block; padding: 10px 20px; background: var(--accent-primary); color: #fff; border: none; border-radius: 6px; cursor: pointer; text-decoration: none; font-size: 14px; transition: opacity 0.2s; }
.btn:hover { opacity: 0.85; }
.btn-danger { background: var(--accent-error, #ff6b6b); }
.btn-sm { padding: 6px 14px; font-size: 13px; }
.form-group { margin-bottom: 15px; }
.form-group label { display: block; margin-bottom: 5px; font-weight: 500; }
.form-group input, .form-group textarea, .form-group select { width: 100%; padding: 10px; border: 1px solid var(--border); border-radius: 6px; background: var(--bg-card); color: var(--text-primary); box-sizing: border-box; font-size: 14px; }
.form-group input:focus, .form-group textarea:focus, .form-group select:focus { outline: none; border-color: var(--accent-primary); }
table { width: 100%; border-collapse: collapse; }
table th, table td { padding: 12px; text-align: left; border-bottom: 1px solid var(--border); }
table th { font-weight: 600; }
table tbody tr:hover { background: var(--bg-secondary, rgba(255,255,255,0.03)); }
.badge { display: inline-block; padding: 4px 12px; border-radius: 12px; font-size: 12px; font-weight: 500; }
.badge-success { background: rgba(107,203,119,0.15); color: var(--accent-success, #6bcb77); }
.badge-warning { background: rgba(255,217,61,0.15); color: var(--accent-warning, #ffd93d); }
.badge-error { background: rgba(255,107,107,0.15); color: var(--accent-error, #ff6b6b); }
.filter-bar { display: flex; gap: 10px; margin-bottom: 20px; align-items: center; flex-wrap: wrap; }
.filter-bar select, .filter-bar input, .filter-bar button { padding: 8px 12px; border: 1px solid var(--border); border-radius: 6px; background: var(--bg-card); color: var(--text-primary); font-size: 14px; }
.filter-bar button { background: var(--accent-primary); color: #fff; border: none; cursor: pointer; }
.pagination { display: flex; gap: 8px; margin-top: 20px; align-items: center; justify-content: center; }
.pagination a, .pagination span { padding: 8px 14px; border: 1px solid var(--border); border-radius: 6px; text-decoration: none; color: var(--text-primary); background: var(--bg-card); }
.pagination a:hover { background: var(--accent-primary); color: #fff; border-color: var(--accent-primary); }
.search-bar { display: flex; gap: 10px; margin-bottom: 20px; }
.search-bar input { flex: 1; padding: 10px 14px; border: 1px solid var(--border); border-radius: 6px; background: var(--bg-card); color: var(--text-primary); }
.search-bar button { padding: 10px 20px; background: var(--accent-primary); color: #fff; border: none; border-radius: 6px; cursor: pointer; }
.stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr)); gap: 16px; margin-bottom: 24px; }
.stat-card { background: var(--bg-card); padding: 20px; border-radius: 12px; border: 1px solid var(--border); }
.stat-card h3 { font-size: 13px; color: var(--text-secondary); margin-bottom: 8px; }
.stat-value { font-size: 28px; font-weight: bold; }
.alert { padding: 15px; border-radius: 6px; margin-bottom: 20px; }
.alert-success { background: var(--accent-success, #6bcb77); color: #fff; }
.alert-error { background: var(--accent-error, #ff6b6b); color: #fff; }
.alert-warning { background: var(--accent-warning, #ffd93d); color: #333; }
";
    }
}
