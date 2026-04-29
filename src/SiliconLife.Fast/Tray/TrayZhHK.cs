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
/// Chinese (Hong Kong) tray localization implementation
/// </summary>
public class TrayZhHK : TrayLocalizationBase
{
    /// <summary>
    /// Gets the fast edition name
    /// </summary>
    public override string SoftwareName => "矽基生命群";

    /// <summary>
    /// Gets the status label
    /// </summary>
    public override string Status => "狀態";

    /// <summary>
    /// Gets the uptime label
    /// </summary>
    public override string Uptime => "運行時間";

    /// <summary>
    /// Gets the running status text
    /// </summary>
    public override string Running => "運行中";

    /// <summary>
    /// Gets the shutting down status text
    /// </summary>
    public override string ShuttingDown => "關閉中";

    /// <summary>
    /// Gets the silicon beings label
    /// </summary>
    public override string SiliconBeings => "硅基生命體";

    /// <summary>
    /// Gets the active status text
    /// </summary>
    public override string Active => "活躍";

    /// <summary>
    /// Gets the name label
    /// </summary>
    public override string Name => "名稱";

    /// <summary>
    /// Gets the AI model label
    /// </summary>
    public override string AIModel => "AI 模型";

    /// <summary>
    /// Gets the memory label
    /// </summary>
    public override string Memory => "記憶體";

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
    public override string DoubleClick => "雙擊";

    /// <summary>
    /// Gets the right-click action text
    /// </summary>
    public override string RightClick => "右鍵";

    /// <summary>
    /// Gets the show menu text
    /// </summary>
    public override string ShowMenu => "顯示選單";

    /// <summary>
    /// Gets the open web interface menu item text
    /// </summary>
    public override string OpenWebInterface => "開啟 Web 介面";

    /// <summary>
    /// Gets the dashboard menu item text
    /// </summary>
    public override string Dashboard => "儀表板";

    /// <summary>
    /// Gets the manage silicon beings menu item text
    /// </summary>
    public override string ManageSiliconBeings => "管理硅基生命體";

    /// <summary>
    /// Gets the configuration menu item text
    /// </summary>
    public override string Configuration => "設定";

    /// <summary>
    /// Gets the exit menu item text
    /// </summary>
    public override string Exit => "退出應用程式";
}
