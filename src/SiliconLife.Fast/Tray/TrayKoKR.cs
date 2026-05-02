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
/// Korean tray localization implementation
/// </summary>
public class TrayKoKR : TrayLocalizationBase
{
    /// <summary>
    /// Gets the localized software name
    /// </summary>
    public override string SoftwareName => "실리콘 라이프 콜렉티브";

    /// <summary>
    /// Gets the status label
    /// </summary>
    public override string Status => "상태";

    /// <summary>
    /// Gets the uptime label
    /// </summary>
    public override string Uptime => "실행 시간";

    /// <summary>
    /// Gets the running status text
    /// </summary>
    public override string Running => "실행 중";

    /// <summary>
    /// Gets the shutting down status text
    /// </summary>
    public override string ShuttingDown => "종료 중";

    /// <summary>
    /// Gets the silicon beings label
    /// </summary>
    public override string SiliconBeings => "실리콘 생명체";

    /// <summary>
    /// Gets the active status text
    /// </summary>
    public override string Active => "활성";

    /// <summary>
    /// Gets the name label
    /// </summary>
    public override string Name => "이름";

    /// <summary>
    /// Gets the AI model label
    /// </summary>
    public override string AIModel => "AI 모델";

    /// <summary>
    /// Gets the memory label
    /// </summary>
    public override string Memory => "메모리";

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
    public override string DoubleClick => "더블클릭";

    /// <summary>
    /// Gets the right-click action text
    /// </summary>
    public override string RightClick => "우클릭";

    /// <summary>
    /// Gets the show menu text
    /// </summary>
    public override string ShowMenu => "메뉴 표시";

    /// <summary>
    /// Gets the open web interface menu item text
    /// </summary>
    public override string OpenWebInterface => "웹 인터페이스 열기";

    /// <summary>
    /// Gets the dashboard menu item text
    /// </summary>
    public override string Dashboard => "대시보드";

    /// <summary>
    /// Gets the manage silicon beings menu item text
    /// </summary>
    public override string ManageSiliconBeings => "실리콘 생명체 관리";

    /// <summary>
    /// Gets the configuration menu item text
    /// </summary>
    public override string Configuration => "설정";

    /// <summary>
    /// Gets the Speedy Pack Manager menu item text
    /// </summary>
    public override string SpeedyPackManager => "Speedy Pack 관리자";

    /// <summary>
    /// Gets the exit menu item text
    /// </summary>
    public override string Exit => "종료";

    /// <summary>
    /// Gets the web server startup error title
    /// </summary>
    public override string WebServerStartupErrorTitle => "웹 서버 시작 실패";

    /// <summary>
    /// Gets the web server startup error message
    /// </summary>
    public override string WebServerStartupErrorMessage => "웹 서버를 시작하지 못했습니다. 애플리케이션이 종료됩니다.\n\n오류: {0}";
}
