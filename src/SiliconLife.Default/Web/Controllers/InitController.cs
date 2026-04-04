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

using SiliconLife.Collective;

namespace SiliconLife.Default.Web;

public class InitController : Controller
{
    private readonly DefaultConfigData _configData;
    private readonly DefaultLocalizationBase _localization;
    private readonly SkinManager _skinManager;
    private readonly Action _onInitialized;

    public InitController(DefaultConfigData configData, DefaultLocalizationBase localization, SkinManager skinManager, Action onInitialized)
    {
        _configData = configData;
        _localization = localization;
        _skinManager = skinManager;
        _onInitialized = onInitialized;
    }

    public override void Handle()
    {
        if (Request.HttpMethod == "POST")
        {
            HandlePost();
        }
        else if (Request.Url?.AbsolutePath == "/init/browse" || Request.Url?.AbsolutePath == "/init/browse/")
        {
            HandleBrowse();
        }
        else
        {
            ShowForm();
        }
    }

    private void ShowForm()
    {
        ShowFormInternal(null);
    }

    private void ShowFormWithError(string error)
    {
        ShowFormInternal(error);
    }

    private void ShowFormInternal(string? error)
    {
        var html = HtmlBuilder.Create()
            .DocType()
            .Html()
            .Head()
                .MetaCharset()
                .MetaViewport()
                .Title($"{_localization.InitPageTitle} - Silicon Life Collective")
                .Style(GetStyles())
                .Raw(GetSkinSwitchScript())
            .EndBlock()
            .Body()
                .Div()
                    .Class("init-container")
                    .Div()
                        .Class("init-card")
                        .H1("Silicon Life Collective")
                        .P(_localization.InitDescription)
                        .Div()
                            .Class("form-error")
                            .P(error ?? "")
                        .EndBlock()
                        .Form("/init", "post")
                            .Class("init-form")
                            .Attr("onsubmit", "return validateInitForm()")
                            .Div()
                                .Class("form-group")
                                .Label(_localization.InitNicknameLabel, "nickname")
                                .InputText("nickname", _localization.InitNicknamePlaceholder, _configData.UserNickname)
                                .Attr("required", "required")
                            .EndBlock()
                            .Div()
                                .Class("form-group")
                                .Label(_localization.InitEndpointLabel, "ollamaEndpoint")
                                .InputText("ollamaEndpoint", _localization.InitEndpointPlaceholder, _configData.OllamaEndpoint)
                            .EndBlock()
                            .Div()
                                .Class("form-group")
                                .Label(_localization.InitDataDirectoryLabel, "dataDirectory")
                                .Raw(BuildDataDirectoryInput())
                            .EndBlock()
                            .Div()
                                .Class("form-group")
                                .Label(_localization.InitSkinLabel, "webSkin")
                                .Raw(BuildSkinCards())
                                .Raw(BuildPreviewSection())
                            .EndBlock()
                            .Div()
                                .Class("form-actions")
                                .Button(_localization.InitSubmitButton, "submit")
                            .EndBlock()
                        .EndBlock()
                        .Div()
                            .Class("init-footer")
                            .P(_localization.InitFooterHint)
                        .EndBlock()
                    .EndBlock()
                .EndBlock()
            .EndBlock()
            .Build();

        RenderHtml(html);
    }

    private void HandleBrowse()
    {
        var dir = GetQueryValue("dir");
        string? basePath;
        bool showDrives = false;

        if (string.IsNullOrEmpty(dir) || dir == "/")
        {
            basePath = AppDomain.CurrentDomain.BaseDirectory;
        }
        else if (dir == "__drives__")
        {
            basePath = null;
            showDrives = true;
        }
        else
        {
            basePath = Path.GetFullPath(dir);
        }

        var result = new List<object>();
        try
        {
            if (showDrives)
            {
                foreach (var drive in DriveInfo.GetDrives()
                    .Where(d => d.IsReady && d.DriveType == DriveType.Fixed || d.DriveType == DriveType.Removable || d.DriveType == DriveType.Network)
                    .OrderBy(d => d.Name))
                {
                    var root = drive.RootDirectory.FullName;
                    result.Add(new { name = drive.Name.TrimEnd('\\'), path = root.Replace('\\', '/'), isParent = false });
                }
            }
            else if (Directory.Exists(basePath))
            {
                var isRoot = Path.GetPathRoot(basePath) == basePath;
                if (!isRoot)
                {
                    var parent = Directory.GetParent(basePath)?.FullName;
                    if (parent != null)
                    {
                        result.Add(new { name = "..", path = parent.Replace('\\', '/'), isParent = true });
                    }
                }
                else
                {
                    result.Add(new { name = "..", path = "__drives__", isParent = true });
                }

                foreach (var d in Directory.GetDirectories(basePath)
                    .OrderBy(d => d)
                    .Take(50))
                {
                    var name = Path.GetFileName(d);
                    result.Add(new { name, path = d.Replace('\\', '/'), isParent = false });
                }
            }
        }
        catch
        {
            // ignore access errors
        }

        RenderJson(new { directories = result, currentPath = basePath?.Replace('\\', '/') ?? "" });
    }

    private void HandlePost()
    {
        var form = ParseFormData();

        var nickname = form.GetValueOrDefault("nickname", "").Trim();
        if (string.IsNullOrEmpty(nickname))
        {
            ShowFormWithError(_localization.InitNicknameRequiredError);
            return;
        }

        var dataDir = form.GetValueOrDefault("dataDirectory", "").Trim();
        if (string.IsNullOrEmpty(dataDir))
        {
            ShowFormWithError(_localization.InitDataDirectoryRequiredError);
            return;
        }

        _configData.UserNickname = nickname;
        _configData.DataDirectory = dataDir;

        var endpoint = form.GetValueOrDefault("ollamaEndpoint", "").Trim();
        if (!string.IsNullOrEmpty(endpoint))
        {
            _configData.OllamaEndpoint = endpoint;
        }

        var skin = form.GetValueOrDefault("webSkin", "").Trim();
        _configData.WebSkin = string.IsNullOrEmpty(skin) ? null! : skin;

        _configData.SaveConfig();
        _onInitialized();

        Redirect("/");
    }

    private string BuildDataDirectoryInput()
    {
        var currentDir = _configData.DataDirectory;
        return $"<div class=\"dir-input-row\"><input type=\"text\" name=\"dataDirectory\" id=\"dataDirInput\" value=\"{currentDir}\" placeholder=\"{_localization.InitDataDirectoryPlaceholder}\"><button type=\"button\" class=\"dir-browse-btn\" onclick=\"openDirBrowser()\">{_localization.InitDataDirectoryBrowse}</button></div><div id=\"dirBrowser\" class=\"dir-browser\" style=\"display:none;\"></div>";
    }

    private string BuildSkinCards()
    {
        var skins = _skinManager.GetAvailableSkins()
            .Select(c => (Code: c, Skin: _skinManager.GetSkin(c)!))
            .OrderBy(s => s.Code)
            .ToList();

        var currentSkin = _configData.WebSkin ?? skins.FirstOrDefault().Code;

        var sb = new System.Text.StringBuilder();
        sb.Append("<div class=\"skin-grid\">");
        foreach (var (code, skin) in skins)
        {
            var p = skin.PreviewInfo;
            var selected = code == currentSkin ? " selected" : "";
            sb.Append($"<div class=\"skin-option{selected}\" data-skin=\"{code}\" onclick=\"selectSkin('{code}')\" ");
            sb.Append($"style=\"border-color:{p.BorderColor};background:linear-gradient(135deg,{p.BackgroundColor} 0%,{p.CardColor} 100%);color:{p.TextColor};\">");
            sb.Append($"<div class=\"skin-icon\">{p.Icon}</div>");
            sb.Append($"<div class=\"skin-name\">{skin.Name}</div>");
            sb.Append($"<div class=\"skin-desc\">{p.Description}</div>");
            sb.Append("<div class=\"skin-colors\">");
            sb.Append($"<span class=\"color-dot\" style=\"background:{p.BackgroundColor};\"></span>");
            sb.Append($"<span class=\"color-dot\" style=\"background:{p.CardColor};\"></span>");
            sb.Append($"<span class=\"color-dot\" style=\"background:{p.AccentColor};\"></span>");
            sb.Append("</div>");
            sb.Append("</div>");
        }
        sb.Append("</div>");
        sb.Append($"<input type=\"hidden\" name=\"webSkin\" id=\"skinInput\" value=\"{currentSkin}\">");
        return sb.ToString();
    }

    private string BuildPreviewSection()
    {
        var skins = _skinManager.GetAvailableSkins()
            .Select(c => (Code: c, Skin: _skinManager.GetSkin(c)!))
            .OrderBy(s => s.Code)
            .ToList();
        var currentSkin = _configData.WebSkin ?? skins.FirstOrDefault().Code;
        var p = _skinManager.GetSkin(currentSkin)?.PreviewInfo ?? skins.First().Skin.PreviewInfo;

        var sb = new System.Text.StringBuilder();
        sb.Append("<div class=\"skin-preview-section\">");
        sb.Append("<h3 class=\"skin-preview-title\">Preview</h3>");
        sb.Append("<div id=\"preview\" class=\"skin-preview-box\" ");
        sb.Append($"style=\"background:{p.BackgroundColor};color:{p.TextColor};border-color:{p.BorderColor};\">");
        sb.Append("<div class=\"preview-inner\">");
        sb.Append($"<div class=\"preview-card\" style=\"background:{p.CardColor};border-color:{p.BorderColor};\">");
        sb.Append($"<h4>{_localization.InitSkinPreviewCardTitle}</h4>");
        sb.Append($"<p>{_localization.InitSkinPreviewCardContent}</p>");
        sb.Append("</div>");
        sb.Append("<div class=\"preview-btns\">");
        sb.Append($"<button class=\"preview-btn\" style=\"background:{p.AccentColor};color:#fff;\">{_localization.InitSkinPreviewPrimaryBtn}</button>");
        sb.Append($"<button class=\"preview-btn\" style=\"background:{p.AccentColor};opacity:0.6;color:#fff;\">{_localization.InitSkinPreviewSecondaryBtn}</button>");
        sb.Append("</div>");
        sb.Append("</div>");
        sb.Append("</div>");
        sb.Append("</div>");
        return sb.ToString();
    }

    private string GetSkinSwitchScript()
    {
        var skins = _skinManager.GetAvailableSkins()
            .Select(c => (Code: c, Skin: _skinManager.GetSkin(c)!))
            .OrderBy(s => s.Code)
            .ToList();

        var entries = new List<string>();
        foreach (var (code, skin) in skins)
        {
            var p = skin.PreviewInfo;
            entries.Add($"\"{code}\":{{bg:\"{p.BackgroundColor}\",card:\"{p.CardColor}\",accent:\"{p.AccentColor}\",text:\"{p.TextColor}\",border:\"{p.BorderColor}\"}}");
        }

        return $"<script>const skinData={{{string.Join(',', entries)}}};function selectSkin(code){{document.querySelectorAll('.skin-option').forEach(el=>el.classList.remove('selected'));document.querySelector('[data-skin=\"'+code+'\"]').classList.add('selected');document.getElementById('skinInput').value=code;const s=skinData[code],pv=document.getElementById('preview');pv.style.background=s.bg;pv.style.color=s.text;pv.style.borderColor=s.border;pv.querySelectorAll('.preview-card').forEach(c=>{{c.style.background=s.card;c.style.borderColor=s.border;}});pv.querySelectorAll('.preview-btn').forEach(b=>{{b.style.background=s.accent;}});}}function validateInitForm(){{var n=document.getElementById('nickname'),d=document.getElementById('dataDirInput');if(!n.value.trim()){{n.focus();return false;}}if(!d.value.trim()){{d.focus();return false;}}return true;}}let dirBrowserOpen=false;function openDirBrowser(){{const db=document.getElementById('dirBrowser');if(dirBrowserOpen){{db.style.display='none';dirBrowserOpen=false;return;}}dirBrowserOpen=true;db.style.display='block';browseDir(document.getElementById('dataDirInput').value||'/');}}function browseDir(path){{fetch('/init/browse?dir='+encodeURIComponent(path)).then(r=>r.json()).then(data=>{{const db=document.getElementById('dirBrowser');let html='<div class=\"dir-header\"><span class=\"dir-current\">'+data.currentPath+'</span></div><div class=\"dir-list\">';data.directories.forEach(d=>{{html+='<div class=\"dir-item'+(d.isParent?' dir-parent':'')+'\" onclick=\"browseDir(\\''+d.path+'\\')\"><span class=\"dir-icon\">'+(d.isParent?'\u2190':'\U0001f4c1')+'</span><span>'+d.name+'</span></div>';}});html+='</div>';db.innerHTML=html;}});}}</script>";
    }

    private Dictionary<string, string> ParseFormData()
    {
        var result = new Dictionary<string, string>();
        var body = GetRequestBody();

        if (string.IsNullOrEmpty(body))
            return result;

        foreach (var pair in body.Split('&'))
        {
            var parts = pair.Split('=', 2);
            if (parts.Length == 2)
            {
                var key = Uri.UnescapeDataString(parts[0]);
                var value = Uri.UnescapeDataString(parts[1]);
                result[key] = value;
            }
        }

        return result;
    }

    private static string GetStyles()
    {
        return @"
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body {
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                background: linear-gradient(135deg, #0f172a 0%, #1e293b 100%);
                min-height: 100vh;
                display: flex;
                align-items: center;
                justify-content: center;
                color: #e2e8f0;
            }
            .init-container {
                width: 100%;
                max-width: 460px;
                padding: 20px;
            }
            .init-card {
                background: rgba(30, 41, 59, 0.8);
                border: 1px solid rgba(148, 163, 184, 0.1);
                border-radius: 16px;
                padding: 40px 32px;
                backdrop-filter: blur(20px);
                max-width: 600px;
                width: 100%;
            }
            .init-card h1 {
                font-size: 24px;
                font-weight: 600;
                margin-bottom: 8px;
                color: #f1f5f9;
            }
            .init-card > p {
                font-size: 14px;
                color: #94a3b8;
                margin-bottom: 32px;
            }
            .init-form {
                display: flex;
                flex-direction: column;
                gap: 20px;
            }
            .form-group {
                display: flex;
                flex-direction: column;
                gap: 6px;
            }
            .form-group label {
                font-size: 13px;
                font-weight: 500;
                color: #cbd5e1;
            }
            .form-group input {
                width: 100%;
                padding: 10px 14px;
                border: 1px solid rgba(148, 163, 184, 0.2);
                border-radius: 8px;
                background: rgba(15, 23, 42, 0.6);
                color: #f1f5f9;
                font-size: 14px;
                outline: none;
                transition: border-color 0.2s;
            }
            .form-group input::placeholder {
                color: #64748b;
            }
            .form-group input:focus {
                border-color: #3b82f6;
            }
            .dir-input-row {
                display: flex;
                gap: 8px;
            }
            .dir-input-row input {
                flex: 1;
            }
            .dir-browse-btn {
                padding: 10px 16px;
                border: 1px solid rgba(148, 163, 184, 0.3);
                border-radius: 8px;
                background: rgba(51, 65, 85, 0.6);
                color: #cbd5e1;
                font-size: 13px;
                cursor: pointer;
                transition: background 0.2s;
                white-space: nowrap;
            }
            .dir-browse-btn:hover {
                background: rgba(51, 65, 85, 0.9);
            }
            .dir-browser {
                margin-top: 8px;
                border: 1px solid rgba(148, 163, 184, 0.2);
                border-radius: 8px;
                background: rgba(15, 23, 42, 0.8);
                max-height: 220px;
                overflow-y: auto;
            }
            .dir-header {
                padding: 10px 12px;
                border-bottom: 1px solid rgba(148, 163, 184, 0.15);
                position: sticky;
                top: 0;
                background: rgba(15, 23, 42, 0.95);
            }
            .dir-current {
                font-size: 12px;
                color: #94a3b8;
                font-family: 'SF Mono', Consolas, monospace;
                overflow: hidden;
                text-overflow: ellipsis;
                white-space: nowrap;
                display: block;
            }
            .dir-list { padding: 4px 0; }
            .dir-item {
                display: flex;
                align-items: center;
                gap: 8px;
                padding: 8px 12px;
                cursor: pointer;
                font-size: 13px;
                color: #cbd5e1;
                transition: background 0.15s;
            }
            .dir-item:hover { background: rgba(59, 130, 246, 0.1); }
            .dir-parent { color: #94a3b8; }
            .dir-icon { font-size: 14px; width: 20px; text-align: center; }
            .dir-browser::-webkit-scrollbar { width: 6px; }
            .dir-browser::-webkit-scrollbar-track { background: transparent; }
            .dir-browser::-webkit-scrollbar-thumb { background: rgba(148,163,184,0.3); border-radius: 3px; }
            .skin-grid {
                display: grid;
                grid-template-columns: repeat(4, 1fr);
                gap: 12px;
                margin-top: 6px;
            }
            .skin-option {
                cursor: pointer;
                transition: all 0.3s ease;
                text-align: center;
                padding: 14px 8px;
                border-radius: 10px;
                border: 2px solid transparent;
                position: relative;
                user-select: none;
            }
            .skin-option:hover {
                transform: translateY(-3px);
                box-shadow: 0 6px 16px rgba(0,0,0,0.25);
            }
            .skin-option.selected::after {
                content: '\2713';
                position: absolute;
                top: 6px;
                right: 8px;
                font-size: 14px;
                width: 20px;
                height: 20px;
                border-radius: 50%;
                display: flex;
                align-items: center;
                justify-content: center;
            }
            .skin-icon { font-size: 24px; margin-bottom: 6px; }
            .skin-name { font-weight: 600; font-size: 13px; margin-bottom: 2px; }
            .skin-desc { font-size: 11px; opacity: 0.7; }
            .skin-colors { margin-top: 8px; display: flex; justify-content: center; gap: 4px; }
            .color-dot { width: 10px; height: 10px; border-radius: 50%; display: inline-block; }
            .skin-preview-section { margin-top: 12px; }
            .skin-preview-title { font-size: 13px; font-weight: 500; color: #cbd5e1; margin-bottom: 8px; }
            .skin-preview-box {
                border-radius: 10px;
                border: 1px solid;
                padding: 20px;
                transition: all 0.3s ease;
                min-height: 160px;
            }
            .preview-inner { display: flex; flex-direction: column; gap: 12px; }
            .preview-card {
                padding: 14px;
                border-radius: 8px;
                border: 1px solid;
                transition: all 0.3s ease;
            }
            .preview-card h4 { font-size: 14px; font-weight: 600; margin-bottom: 6px; }
            .preview-card p { font-size: 12px; opacity: 0.8; line-height: 1.5; }
            .preview-btns { display: flex; gap: 8px; flex-wrap: wrap; }
            .preview-btn {
                padding: 6px 14px;
                border-radius: 6px;
                border: none;
                cursor: pointer;
                font-size: 12px;
                transition: all 0.3s ease;
            }
            .form-error {
                background: rgba(239, 68, 68, 0.1);
                border: 1px solid rgba(239, 68, 68, 0.3);
                border-radius: 8px;
                padding: 10px 14px;
                margin-bottom: 8px;
            }
            .form-error p {
                font-size: 13px;
                color: #fca5a5;
            }
            .form-actions {
                margin-top: 8px;
            }
            .form-actions button {
                width: 100%;
                padding: 12px;
                border: none;
                border-radius: 8px;
                background: #3b82f6;
                color: white;
                font-size: 15px;
                font-weight: 500;
                cursor: pointer;
                transition: background 0.2s;
            }
            .form-actions button:hover {
                background: #2563eb;
            }
            .init-footer {
                margin-top: 24px;
                text-align: center;
            }
            .init-footer p {
                font-size: 12px;
                color: #64748b;
            }
        ";
    }
}
