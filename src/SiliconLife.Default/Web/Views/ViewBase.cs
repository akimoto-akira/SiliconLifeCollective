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

    protected string RenderCommonHead(ViewModelBase model)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<meta charset=\"utf-8\">");
        sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        
        if (!string.IsNullOrEmpty(model.Title))
            sb.AppendLine($"<title>{EscapeHtml(model.Title)}</title>");
        
        foreach (var meta in model.Meta)
            sb.AppendLine($"<meta name=\"{EscapeHtml(meta.Key)}\" content=\"{EscapeHtml(meta.Value)}\">");
        
        foreach (var style in model.Styles)
            sb.AppendLine($"<link rel=\"stylesheet\" href=\"{EscapeHtml(style)}\">");
        
        if (model.Styles.Count == 0)
            sb.AppendLine("<style>" + GetDefaultStyles() + "</style>");
        
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        return sb.ToString();
    }

    protected string RenderSidebar()
    {
        return @"
<div class=""sidebar"">
    <h2>导航</h2>
    <ul>
        <li><a href=""/dashboard"">仪表盘</a></li>
        <li><a href=""/chat"">聊天</a></li>
        <li><a href=""/beings"">硅基人</a></li>
        <li><a href=""/tasks"">任务</a></li>
        <li><a href=""/permissions"">权限</a></li>
        <li><a href=""/logs"">日志</a></li>
        <li><a href=""/config"">配置</a></li>
        <li><a href=""/knowledge"">知识库</a></li>
        <li><a href=""/memory"">记忆</a></li>
        <li><a href=""/code-browser"">代码浏览</a></li>
        <li><a href=""/projects"">项目</a></li>
    </ul>
</div>";
    }

    protected string RenderFooter()
    {
        return @"
<footer class=""footer"">
    <p>&copy; 2026 Silicon Life Collective. All rights reserved.</p>
</footer>
</body>
</html>";
    }

    protected virtual string GetDefaultStyles()
    {
        return @"
* { box-sizing: border-box; margin: 0; padding: 0; }
body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; background: #f5f7fa; line-height: 1.6; }
.dashboard-container { display: flex; min-height: 100vh; }
.sidebar { width: 240px; background: #2c3e50; color: white; padding: 20px; }
.sidebar h2 { font-size: 18px; margin-bottom: 20px; }
.sidebar ul { list-style: none; }
.sidebar li { margin-bottom: 10px; }
.sidebar a { color: #ecf0f1; text-decoration: none; transition: color 0.3s; }
.sidebar a:hover { color: #3498db; }
.main-content { flex: 1; padding: 30px; }
.header { margin-bottom: 30px; }
.header h1 { font-size: 28px; color: #2c3e50; }
.stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 20px; margin-bottom: 30px; }
.stat-card { background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
.stat-card h3 { font-size: 14px; color: #7f8c8d; margin-bottom: 10px; }
.stat-value { font-size: 32px; font-weight: bold; color: #2c3e50; }
.container { max-width: 1200px; margin: 0 auto; padding: 20px; }
.card { background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); margin-bottom: 20px; }
.btn { display: inline-block; padding: 10px 20px; background: #3498db; color: white; border: none; border-radius: 6px; cursor: pointer; text-decoration: none; }
.btn:hover { background: #2980b9; }
.btn-danger { background: #e74c3c; }
.btn-danger:hover { background: #c0392b; }
.footer { text-align: center; padding: 20px; color: #7f8c8d; }
.form-group { margin-bottom: 15px; }
.form-group label { display: block; margin-bottom: 5px; font-weight: 500; }
.form-group input, .form-group textarea, .form-group select { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 6px; }
table { width: 100%; border-collapse: collapse; }
table th, table td { padding: 12px; text-align: left; border-bottom: 1px solid #eee; }
table th { background: #f8f9fa; font-weight: 600; }
.alert { padding: 15px; border-radius: 6px; margin-bottom: 20px; }
.alert-success { background: #d4edda; color: #155724; }
.alert-error { background: #f8d7da; color: #721c24; }
.alert-warning { background: #fff3cd; color: #856404; }
";
    }

    protected virtual string GetDefaultScripts()
    {
        return @"
function $(id) { return document.getElementById(id); }
function $$(selector) { return document.querySelectorAll(selector); }
function showAlert(message, type) {
    var alert = document.createElement('div');
    alert.className = 'alert alert-' + (type || 'success');
    alert.textContent = message;
    document.body.insertBefore(alert, document.body.firstChild);
    setTimeout(function() { alert.remove(); }, 3000);
}
function apiCall(url, options) {
    return fetch(url, Object.assign({ headers: { 'Content-Type': 'application/json' } }, options))
        .then(function(r) { return r.json(); });
}
";
    }
}
