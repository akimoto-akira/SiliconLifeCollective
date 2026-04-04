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

using System.Diagnostics;
using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Default.Web;

[WebCode]
public class DashboardController : Controller
{
    private readonly SiliconBeingManager _beingManager;
    private readonly ChatSystem _chatSystem;

    public DashboardController(SiliconBeingManager beingManager, ChatSystem chatSystem)
    {
        _beingManager = beingManager;
        _chatSystem = chatSystem;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/dashboard";
        
        if (path == "/dashboard" || path == "/dashboard/index")
        {
            Index();
        }
        else if (path == "/api/dashboard/stats")
        {
            GetStats();
        }
        else if (path == "/api/dashboard/metrics")
        {
            GetMetrics();
        }
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var html = HtmlBuilder.Create()
            .DocType()
            .Html()
            .Head()
                .MetaCharset()
                .MetaViewport()
                .Title("仪表盘 - Silicon Life Collective")
                .Style(GetDashboardStyles())
                .Script(GetDashboardScripts())
            .EndBlock()
            .Body()
                .Div()
                    .Class("dashboard-container")
                    .Div()
                        .Class("sidebar")
                        .H2("导航")
                        .Raw(@"
                            <ul>
                                <li><a href=""/dashboard"">仪表盘</a></li>
                                <li><a href=""/chat"">聊天</a></li>
                                <li><a href=""/beings"">硅基人</a></li>
                                <li><a href=""/tasks"">任务</a></li>
                                <li><a href=""/permissions"">权限</a></li>
                                <li><a href=""/logs"">日志</a></li>
                                <li><a href=""/config"">配置</a></li>
                            </ul>
                        ")
                    .EndBlock()
                    .Div()
                        .Class("main-content")
                        .Div()
                            .Class("header")
                            .H1("仪表盘")
                        .EndBlock()
                        .Div()
                            .Class("stats-grid")
                            .Raw(@"
                                <div class=""stat-card"">
                                    <h3>硅基人数量</h3>
                                    <div class=""stat-value"" id=""being-count"">0</div>
                                </div>
                                <div class=""stat-card"">
                                    <h3>活跃硅基人</h3>
                                    <div class=""stat-value"" id=""active-beings"">0</div>
                                </div>
                                <div class=""stat-card"">
                                    <h3>运行时间</h3>
                                    <div class=""stat-value"" id=""uptime"">00:00:00</div>
                                </div>
                                <div class=""stat-card"">
                                    <h3>内存占用</h3>
                                    <div class=""stat-value"" id=""memory"">0 MB</div>
                                </div>
                            ")
                        .EndBlock()
                        .Div()
                            .Class("chart-section")
                            .Raw(@"
                                <h2>消息频率</h2>
                                <div class=""chart-container"">
                                    <svg id=""message-chart"" viewBox=""0 0 800 300"" preserveAspectRatio=""xMidYMid meet""></svg>
                                </div>
                            ")
                        .EndBlock()
                    .EndBlock()
                .EndBlock()
            .EndBlock()
            .Build();

        RenderHtml(html);
    }

    private void GetStats()
    {
        var beings = _beingManager.GetAllBeings();
        var process = Process.GetCurrentProcess();
        var uptime = DateTime.Now - process.StartTime;
        
        var stats = new
        {
            beingCount = beings.Count,
            activeBeings = beings.Count(b => !b.IsIdle),
            uptime = uptime.ToString(@"hh\:mm\:ss"),
            memoryMB = Math.Round(process.WorkingSet64 / 1024.0 / 1024.0, 2)
        };
        
        RenderJson(stats);
    }

    private void GetMetrics()
    {
        var metrics = new
        {
            timestamps = Enumerable.Range(0, 20).Select(i => DateTime.Now.AddMinutes(-19 + i).ToString("HH:mm")).ToList(),
            messageCounts = Enumerable.Range(0, 20).Select(_ => Random.Shared.Next(0, 100)).ToList()
        };
        
        RenderJson(metrics);
    }

    private string GetDashboardStyles()
    {
        return @"
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body { font-family: Arial, sans-serif; background: #f5f7fa; }
            .dashboard-container { display: flex; min-height: 100vh; }
            .sidebar { width: 220px; background: white; padding: 20px; border-right: 1px solid #e1e4e8; }
            .sidebar h2 { font-size: 18px; margin-bottom: 20px; color: #333; }
            .sidebar ul { list-style: none; }
            .sidebar li { margin-bottom: 10px; }
            .sidebar a { color: #555; text-decoration: none; padding: 10px 15px; display: block; border-radius: 6px; transition: background 0.2s; }
            .sidebar a:hover { background: #f0f4f8; color: #2196F3; }
            .main-content { flex: 1; padding: 30px; }
            .header { margin-bottom: 30px; }
            .header h1 { font-size: 28px; color: #333; }
            .stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 20px; margin-bottom: 30px; }
            .stat-card { background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
            .stat-card h3 { font-size: 14px; color: #666; margin-bottom: 10px; }
            .stat-value { font-size: 32px; font-weight: bold; color: #2196F3; }
            .chart-section { background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
            .chart-section h2 { font-size: 18px; margin-bottom: 20px; color: #333; }
            .chart-container { width: 100%; }
            .chart-container svg { width: 100%; height: 300px; }
        ";
    }

    private string GetDashboardScripts()
    {
        return @"
            function updateStats() {
                fetch('/api/dashboard/stats')
                    .then(r => r.json())
                    .then(data => {
                        document.getElementById('being-count').textContent = data.beingCount;
                        document.getElementById('message-count').textContent = data.totalMessages;
                        document.getElementById('uptime').textContent = data.uptime;
                        document.getElementById('memory').textContent = data.memoryMB + ' MB';
                    });
            }

            function updateChart() {
                fetch('/api/dashboard/metrics')
                    .then(r => r.json())
                    .then(data => {
                        var svg = document.getElementById('message-chart');
                        var width = 800;
                        var height = 300;
                        var padding = 40;
                        
                        var maxVal = Math.max(...data.messageCounts, 10);
                        var xStep = (width - padding * 2) / (data.timestamps.length - 1);
                        
                        var points = data.messageCounts.map((val, i) => {
                            var x = padding + i * xStep;
                            var y = height - padding - (val / maxVal) * (height - padding * 2);
                            return x + ',' + y;
                        }).join(' ');
                        
                        var polyline = '<polyline points=\'' + points + '\' fill=\'none\' stroke=\'#2196F3\' stroke-width=\'2\' />';
                        var axes = '<line x1=\'' + padding + '\' y1=\'' + (height - padding) + '\' x2=\'' + (width - padding) + '\' y2=\'' + (height - padding) + '\' stroke=\'#ddd\' stroke-width=\'1\' />';
                        axes += '<line x1=\'' + padding + '\' y1=\'' + padding + '\' x2=\'' + padding + '\' y2=\'' + (height - padding) + '\' stroke=\'#ddd\' stroke-width=\'1\' />';
                        
                        svg.innerHTML = axes + polyline;
                    });
            }

            setInterval(updateStats, 5000);
            setInterval(updateChart, 10000);
            
            window.onload = function() {
                updateStats();
                updateChart();
            };
        ";
    }
}
