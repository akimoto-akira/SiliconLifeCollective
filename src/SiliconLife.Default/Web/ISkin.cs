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

namespace SiliconLife.Default.Web;

public interface ISkin
{
    string Name { get; }
    HtmlBuilder RenderHtml(string content);
    HtmlBuilder RenderError(string message);
    CssBuilder GetStyles();
    JsBuilder GetScripts();

    HtmlBuilder RenderButton(string text, string variant = "primary", string size = "medium");
    HtmlBuilder RenderInput(string placeholder = "", string size = "medium", string? value = null);
    HtmlBuilder RenderTextarea(string placeholder = "", int rows = 4);
    HtmlBuilder RenderSelect(IEnumerable<string> options, string? selected = null);
    HtmlBuilder RenderCheckbox(string label, bool isChecked = false);
    HtmlBuilder RenderBadge(string text, string variant = "primary");
    HtmlBuilder RenderTag(string text);
    HtmlBuilder RenderCard(string title, string content);
    HtmlBuilder RenderAvatar(string text, string size = "medium");
    HtmlBuilder RenderBubble(string text, bool isMine = false);
    HtmlBuilder RenderSwitch(bool isChecked = false);
    HtmlBuilder RenderProgress(double value, string variant = "primary");
    HtmlBuilder RenderTabs(IEnumerable<string> tabs, int activeIndex = 0);
    HtmlBuilder RenderListItem(string title, string? subtitle = null, string? avatar = null, bool active = false);
    HtmlBuilder RenderDivider();
    HtmlBuilder RenderCode(string code);
    HtmlBuilder RenderStatCard(string label, string value, string variant = "primary");
    HtmlBuilder RenderBreadcrumb(IEnumerable<string> items);
    HtmlBuilder RenderTable(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows);
    HtmlBuilder RenderPagination(int totalPages, int currentPage = 1);
    HtmlBuilder RenderDropdown(string triggerText, IEnumerable<string> items);
    HtmlBuilder RenderStatusIndicator(string status);
    HtmlBuilder RenderQuote(string text);
    HtmlBuilder RenderInspirationCard(string icon, string text);
    CssBuilder GetThemeCss();
}

public class SkinManager
{
    private readonly Dictionary<string, ISkin> _skins = new();
    private ISkin? _currentSkin;

    public SkinManager()
    {
    }

    public void RegisterSkin(ISkin skin)
    {
        _skins[skin.Name] = skin;
    }

    public void SetSkin(string name)
    {
        if (_skins.TryGetValue(name, out var skin))
        {
            _currentSkin = skin;
        }
    }

    public ISkin? Current => _currentSkin;

    public IEnumerable<string> GetAvailableSkins() => _skins.Keys;
}
