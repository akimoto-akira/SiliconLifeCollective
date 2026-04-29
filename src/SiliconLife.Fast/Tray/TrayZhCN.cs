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

namespace SiliconLife.Fast.Tray;

/// <summary>
/// Chinese (Simplified) tray localization implementation
/// </summary>
public class TrayZhCN : TrayLocalizationBase
{
    /// <summary>
    /// Gets the localized software name
    /// </summary>
    public override string SoftwareName => "硅基生命群";

    /// <summary>
    /// Gets the status label
    /// </summary>
    public override string Status => "状态";

    /// <summary>
    /// Gets the uptime label
    /// </summary>
    public override string Uptime => "运行时间";

    /// <summary>
    /// Gets the running status text
    /// </summary>
    public override string Running => "运行中";

    /// <summary>
    /// Gets the shutting down status text
    /// </summary>
    public override string ShuttingDown => "关闭中";

    /// <summary>
    /// Gets the silicon beings label
    /// </summary>
    public override string SiliconBeings => "硅基生命体";

    /// <summary>
    /// Gets the active status text
    /// </summary>
    public override string Active => "活跃";

    /// <summary>
    /// Gets the name label
    /// </summary>
    public override string Name => "名称";

    /// <summary>
    /// Gets the AI model label
    /// </summary>
    public override string AIModel => "AI 模型";

    /// <summary>
    /// Gets the memory label
    /// </summary>
    public override string Memory => "内存";

    /// <summary>
    /// Gets the CPU label
    /// </summary>
    public override string CPU => "CPU";

    /// <summary>
    /// Gets the web label
    /// </summary>
    public override string Web => "Web";

    /// <summary>
    /// Gets the double-click action text
    /// </summary>
    public override string DoubleClick => "双击";

    /// <summary>
    /// Gets the right-click action text
    /// </summary>
    public override string RightClick => "右键";

    /// <summary>
    /// Gets the show menu text
    /// </summary>
    public override string ShowMenu => "显示菜单";

    /// <summary>
    /// Gets the open web interface menu item text
    /// </summary>
    public override string OpenWebInterface => "打开 Web 界面";

    /// <summary>
    /// Gets the dashboard menu item text
    /// </summary>
    public override string Dashboard => "仪表板";

    /// <summary>
    /// Gets the manage silicon beings menu item text
    /// </summary>
    public override string ManageSiliconBeings => "管理硅基生命体";

    /// <summary>
    /// Gets the configuration menu item text
    /// </summary>
    public override string Configuration => "配置设置";

    /// <summary>
    /// Gets the exit menu item text
    /// </summary>
    public override string Exit => "退出应用";
}
