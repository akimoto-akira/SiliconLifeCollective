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
/// Japanese tray localization implementation
/// </summary>
public class TrayJaJP : TrayLocalizationBase
{
    /// <summary>
    /// Gets the localized software name
    /// </summary>
    public override string SoftwareName => "シリコンライフコレクティブ";

    /// <summary>
    /// Gets the status label
    /// </summary>
    public override string Status => "ステータス";

    /// <summary>
    /// Gets the uptime label
    /// </summary>
    public override string Uptime => "稼働時間";

    /// <summary>
    /// Gets the running status text
    /// </summary>
    public override string Running => "実行中";

    /// <summary>
    /// Gets the shutting down status text
    /// </summary>
    public override string ShuttingDown => "シャットダウン中";

    /// <summary>
    /// Gets the silicon beings label
    /// </summary>
    public override string SiliconBeings => "シリコン生命体";

    /// <summary>
    /// Gets the active status text
    /// </summary>
    public override string Active => "アクティブ";

    /// <summary>
    /// Gets the name label
    /// </summary>
    public override string Name => "名前";

    /// <summary>
    /// Gets the AI model label
    /// </summary>
    public override string AIModel => "AI モデル";

    /// <summary>
    /// Gets the memory label
    /// </summary>
    public override string Memory => "メモリ";

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
    public override string DoubleClick => "ダブルクリック";

    /// <summary>
    /// Gets the right-click action text
    /// </summary>
    public override string RightClick => "右クリック";

    /// <summary>
    /// Gets the show menu text
    /// </summary>
    public override string ShowMenu => "メニューを表示";

    /// <summary>
    /// Gets the open web interface menu item text
    /// </summary>
    public override string OpenWebInterface => "Web インターフェースを開く";

    /// <summary>
    /// Gets the dashboard menu item text
    /// </summary>
    public override string Dashboard => "ダッシュボード";

    /// <summary>
    /// Gets the manage silicon beings menu item text
    /// </summary>
    public override string ManageSiliconBeings => "シリコン生命体の管理";

    /// <summary>
    /// Gets the configuration menu item text
    /// </summary>
    public override string Configuration => "設定";

    /// <summary>
    /// Gets the exit menu item text
    /// </summary>
    public override string Exit => "終了";

    /// <summary>
    /// Gets the web server startup error title
    /// </summary>
    public override string WebServerStartupErrorTitle => "Web サーバー起動失敗";

    /// <summary>
    /// Gets the web server startup error message
    /// </summary>
    public override string WebServerStartupErrorMessage => "Web サーバーの起動に失敗しました。アプリケーションを終了します。\n\nエラー: {0}";
}
