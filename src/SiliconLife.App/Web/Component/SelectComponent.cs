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

namespace SiliconLife.App.Web.Component;

/// <summary>
/// Select dropdown component with optional searchable/filterable mode
/// </summary>
public class SelectComponent : ComponentBase
{
    private readonly List<(string Value, string Text, bool Disabled)> _options = new();
    private string _name = "";
    private string? _selectedValue;
    private bool _multiple = false;
    private bool _required = false;
    private bool _searchable = false;
    private string _placeholder = "";
    private string _noResultText = "No results found";

    public new SelectComponent Id(string id)
    {
        base.Id = id;
        return this;
    }

    public new SelectComponent Class(string className)
    {
        base.Class = string.IsNullOrEmpty(base.Class) ? className : $"{base.Class} {className}";
        return this;
    }

    public new SelectComponent Style(string style)
    {
        base.Style = string.IsNullOrEmpty(base.Style) ? style : $"{base.Style};{style}";
        return this;
    }

    public new SelectComponent Attr(string name, string value)
    {
        base.Attributes[name] = value;
        return this;
    }

    public SelectComponent Name(string name)
    {
        _name = name;
        return this;
    }

    public SelectComponent AddOption(string value, string text, bool disabled = false)
    {
        _options.Add((value, text, disabled));
        return this;
    }

    public SelectComponent AddOptions(IEnumerable<(string Value, string Text)> options)
    {
        foreach (var (value, text) in options)
        {
            _options.Add((value, text, false));
        }
        return this;
    }

    public SelectComponent Selected(string value)
    {
        _selectedValue = value;
        return this;
    }

    public SelectComponent Multiple(bool multiple = true)
    {
        _multiple = multiple;
        return this;
    }

    public SelectComponent Required(bool required = true)
    {
        _required = required;
        return this;
    }

    /// <summary>
    /// Enable searchable/filterable mode (not compatible with multiple selection)
    /// </summary>
    public SelectComponent Searchable(bool searchable = true)
    {
        _searchable = searchable;
        return this;
    }

    /// <summary>
    /// Set placeholder text for the search input
    /// </summary>
    public SelectComponent Placeholder(string placeholder)
    {
        _placeholder = placeholder;
        return this;
    }

    /// <summary>
    /// Set text displayed when no options match the search query
    /// </summary>
    public SelectComponent NoResultText(string text)
    {
        _noResultText = text;
        return this;
    }

    public override string Render()
    {
        if (_searchable && !_multiple)
        {
            return RenderSearchable();
        }

        return RenderStandard();
    }

    private string RenderStandard()
    {
        var select = H.Select();

        if (!string.IsNullOrEmpty(_name))
            select.Attr("name", _name);

        if (!string.IsNullOrEmpty(base.Id))
            select.Attr("id", base.Id);

        var classes = new List<string>();
        if (!string.IsNullOrEmpty(base.Class))
            classes.Add(base.Class);

        if (classes.Count > 0)
            select.Class(string.Join(" ", classes));

        if (!string.IsNullOrEmpty(base.Style))
            select.Attr("style", base.Style);

        if (_multiple)
            select.Attr("multiple", "multiple");

        if (_required)
            select.Attr("required", "required");

        foreach (var kvp in Attributes)
        {
            select.Attr(kvp.Key, kvp.Value);
        }

        foreach (var (value, text, disabled) in _options)
        {
            var option = H.Option()
                .Attr("value", H.Escape(value))
                .Text(H.Escape(text));

            if (value == _selectedValue)
                option.Attr("selected", "selected");

            if (disabled)
                option.Attr("disabled", "disabled");

            select.Add(option);
        }

        return select.Build();
    }

    private string RenderSearchable()
    {
        var baseId = string.IsNullOrEmpty(base.Id)
            ? "sls-" + Guid.NewGuid().ToString("N")[..8]
            : base.Id;
        base.Id = baseId;

        var wrapperId = baseId + "-wrapper";
        var searchInputId = baseId + "-search";
        var dropdownId = baseId + "-dropdown";

        var selectedText = "";
        var effectiveSelected = _selectedValue;
        if (string.IsNullOrEmpty(effectiveSelected))
        {
            foreach (var (value, _, disabled) in _options)
            {
                if (!disabled)
                {
                    effectiveSelected = value;
                    break;
                }
            }
        }
        foreach (var (value, text, _) in _options)
        {
            if (value == effectiveSelected)
            {
                selectedText = text;
                break;
            }
        }

        var hiddenAttrs = new StringBuilder();
        hiddenAttrs.Append("type=\"hidden\"");
        if (!string.IsNullOrEmpty(_name))
            hiddenAttrs.Append($" name=\"{H.Escape(_name)}\"");
        hiddenAttrs.Append($" id=\"{H.Escape(baseId)}\"");
        hiddenAttrs.Append($" value=\"{H.Escape(effectiveSelected ?? "")}\"");
        if (_required)
            hiddenAttrs.Append(" required=\"required\"");

        foreach (var kvp in Attributes)
        {
            hiddenAttrs.Append($" {H.Escape(kvp.Key)}=\"{H.Escape(kvp.Value)}\"");
        }

        var optionsHtml = new StringBuilder();
        foreach (var (value, text, disabled) in _options)
        {
            var selectedClass = value == effectiveSelected ? " selected" : "";
            var disabledAttr = disabled ? " disabled" : "";
            optionsHtml.AppendLine(
                $"    <div class=\"sl-select-search-option{selectedClass}\" data-value=\"{H.Escape(value)}\"{disabledAttr}>{H.Escape(text)}</div>");
        }

        var css = GetSearchableCss();
        var js = GetSearchableJs(baseId, wrapperId, searchInputId, dropdownId);

        var wrapperClass = "sl-select-search";
        if (!string.IsNullOrEmpty(base.Class))
            wrapperClass += " " + base.Class;

        var wrapperStyle = "";
        if (!string.IsNullOrEmpty(base.Style))
            wrapperStyle = $" style=\"{H.Escape(base.Style)}\"";

        return $@"<div class=""{H.Escape(wrapperClass)}"" id=""{H.Escape(wrapperId)}""{wrapperStyle}>
  <input {hiddenAttrs} />
  <div class=""sl-select-search-control"">
    <input type=""text"" class=""sl-select-search-input"" id=""{H.Escape(searchInputId)}"" placeholder=""{H.Escape(_placeholder)}"" value=""{H.Escape(selectedText)}"" autocomplete=""off"" />
    <span class=""sl-select-search-arrow"">&#9662;</span>
  </div>
  <div class=""sl-select-search-dropdown"" id=""{H.Escape(dropdownId)}"">
{optionsHtml}    <div class=""sl-select-search-no-result"" style=""display:none;"">{H.Escape(_noResultText)}</div>
  </div>
</div>
<style>{css}</style>
<script>{js}</script>";
    }

    public static string GetSearchableGlobalScript()
    {
        return @"<style>
.sl-select-search{position:relative;display:inline-block;width:100%;box-sizing:border-box}
.sl-select-search *{box-sizing:border-box}
.sl-select-search-control{position:relative;display:flex;align-items:center}
.sl-select-search-input{width:100%;padding:8px 30px 8px 12px;border:1px solid var(--border-color,#ccc);border-radius:6px;background:var(--bg-card,#fff);color:var(--text-primary,#333);font-size:14px;outline:none;transition:border-color .2s}
.sl-select-search-input:focus{border-color:var(--accent-primary,#4a90d9)}
.sl-select-search-arrow{position:absolute;right:10px;pointer-events:none;color:var(--text-secondary,#999);font-size:12px;transition:transform .2s}
.sl-select-search.open .sl-select-search-arrow{transform:rotate(180deg)}
.sl-select-search-dropdown{display:none;position:absolute;top:100%;left:0;right:0;max-height:250px;overflow-y:auto;background:var(--bg-card,#fff);border:1px solid var(--border-color,#ccc);border-radius:6px;margin-top:4px;z-index:1000;box-shadow:0 4px 12px rgba(0,0,0,.15)}
.sl-select-search-option{padding:8px 12px;cursor:pointer;color:var(--text-primary,#333);font-size:14px;transition:background .15s}
.sl-select-search-option:hover,.sl-select-search-option.highlighted{background:var(--bg-hover,#f0f0f0)}
.sl-select-search-option.selected{font-weight:600}
.sl-select-search-option[disabled]{opacity:.5;cursor:not-allowed;pointer-events:none}
.sl-select-search-no-result{padding:8px 12px;color:var(--text-secondary,#999);font-size:14px;text-align:center}
.sl-select-search-dropdown::-webkit-scrollbar{width:6px}
.sl-select-search-dropdown::-webkit-scrollbar-track{background:var(--bg-secondary,rgba(0,0,0,.05))}
.sl-select-search-dropdown::-webkit-scrollbar-thumb{background:var(--bg-hover,rgba(0,0,0,.15));border-radius:3px}
</style>
<script>
window.slSelectSearch_create=function(parentEl,cfg){
var w=document.createElement('div');
w.className='sl-select-search'+(cfg.className?' '+cfg.className:'');
var h=document.createElement('input');
h.type='hidden';
if(cfg.name)h.name=cfg.name;
if(cfg.id)h.id=cfg.id;
h.value=cfg.value||'';
var ctrl=document.createElement('div');
ctrl.className='sl-select-search-control';
var s=document.createElement('input');
s.type='text';
s.className='sl-select-search-input';
s.placeholder=cfg.placeholder||'';
s.autocomplete='off';
var d=document.createElement('div');
d.className='sl-select-search-dropdown';
var nr=document.createElement('div');
nr.className='sl-select-search-no-result';
nr.style.display='none';
nr.textContent=cfg.noResultText||'No results found';
var selText='';
var effectiveVal=cfg.value||'';
if(cfg.options){
var keys=Object.keys(cfg.options);
if(!effectiveVal&&keys.length)effectiveVal=keys[0];
h.value=effectiveVal;
for(var i=0;i<keys.length;i++){
var o=document.createElement('div');
o.className='sl-select-search-option';
o.setAttribute('data-value',keys[i]);
o.textContent=cfg.options[keys[i]];
if(keys[i]===effectiveVal){o.classList.add('selected');selText=cfg.options[keys[i]];}
d.appendChild(o);
}
}
d.appendChild(nr);
s.value=selText;
var arrow=document.createElement('span');
arrow.className='sl-select-search-arrow';
arrow.innerHTML='&#9662;';
ctrl.appendChild(s);
ctrl.appendChild(arrow);
w.appendChild(h);
w.appendChild(ctrl);
w.appendChild(d);
parentEl.appendChild(w);
var isOpen=false,hi=-1;
function getSelText(){
var all=d.querySelectorAll('.sl-select-search-option');
for(var i=0;i<all.length;i++)
if(all[i].getAttribute('data-value')===h.value)return all[i].textContent;
return '';
}
function filter(){
var q=s.value.toLowerCase(),c=0,all=d.querySelectorAll('.sl-select-search-option');
for(var i=0;i<all.length;i++){
var m=all[i].textContent.toLowerCase().indexOf(q)!==-1;
all[i].style.display=m?'':'none';
if(m)c++;
}
nr.style.display=c?'none':'';
hi=-1;clrHi();
}
function clrHi(){
var hl=d.querySelectorAll('.sl-select-search-option.highlighted');
for(var i=0;i<hl.length;i++)hl[i].classList.remove('highlighted');
}
function setHi(idx){
clrHi();
var vis=[];
var all=d.querySelectorAll('.sl-select-search-option');
for(var i=0;i<all.length;i++)
if(all[i].style.display!=='none'&&!all[i].hasAttribute('disabled'))vis.push(all[i]);
if(!vis.length)return;
idx=((idx%vis.length)+vis.length)%vis.length;
vis[idx].classList.add('highlighted');
vis[idx].scrollIntoView({block:'nearest'});
hi=idx;
}
function sel(o){
h.value=o.getAttribute('data-value');
s.value=o.textContent;
close();
h.dispatchEvent(new Event('change'));
if(cfg.onchange)cfg.onchange(h.value);
}
function open_(){
if(isOpen)return;
isOpen=true;
d.style.display='block';
w.classList.add('open');
s.select();
filter();
}
function close(){
isOpen=false;
d.style.display='none';
w.classList.remove('open');
hi=-1;clrHi();
s.value=getSelText();
}
s.addEventListener('focus',open_);
s.addEventListener('input',filter);
s.addEventListener('keydown',function(e){
if(e.key==='ArrowDown'){e.preventDefault();if(!isOpen)open_();setHi(hi+1);}
else if(e.key==='ArrowUp'){e.preventDefault();setHi(hi-1);}
else if(e.key==='Enter'){e.preventDefault();var hl=d.querySelector('.sl-select-search-option.highlighted');if(hl)sel(hl);}
else if(e.key==='Escape'){close();}
});
d.addEventListener('mousedown',function(e){
var o=e.target.closest('.sl-select-search-option');
if(o&&!o.hasAttribute('disabled')){e.preventDefault();sel(o);}
});
arrow.addEventListener('click',function(e){
e.stopPropagation();
if(isOpen)close();else open_();
});
document.addEventListener('click',function(e){
if(!w.contains(e.target))close();
});
return w;
};
</script>";
    }

    private static string GetSearchableCss()
    {
        return @"
.sl-select-search{position:relative;display:inline-block;width:100%;box-sizing:border-box}
.sl-select-search *{box-sizing:border-box}
.sl-select-search-control{position:relative;display:flex;align-items:center}
.sl-select-search-input{width:100%;padding:8px 30px 8px 12px;border:1px solid var(--border-color,#ccc);border-radius:6px;background:var(--bg-card,#fff);color:var(--text-primary,#333);font-size:14px;outline:none;transition:border-color .2s}
.sl-select-search-input:focus{border-color:var(--accent-primary,#4a90d9)}
.sl-select-search-arrow{position:absolute;right:10px;pointer-events:none;color:var(--text-secondary,#999);font-size:12px;transition:transform .2s}
.sl-select-search.open .sl-select-search-arrow{transform:rotate(180deg)}
.sl-select-search-dropdown{display:none;position:absolute;top:100%;left:0;right:0;max-height:250px;overflow-y:auto;background:var(--bg-card,#fff);border:1px solid var(--border-color,#ccc);border-radius:6px;margin-top:4px;z-index:1000;box-shadow:0 4px 12px rgba(0,0,0,.15)}
.sl-select-search-option{padding:8px 12px;cursor:pointer;color:var(--text-primary,#333);font-size:14px;transition:background .15s}
.sl-select-search-option:hover,.sl-select-search-option.highlighted{background:var(--bg-hover,#f0f0f0)}
.sl-select-search-option.selected{font-weight:600}
.sl-select-search-option[disabled]{opacity:.5;cursor:not-allowed;pointer-events:none}
.sl-select-search-no-result{padding:8px 12px;color:var(--text-secondary,#999);font-size:14px;text-align:center}
.sl-select-search-dropdown::-webkit-scrollbar{width:6px}
.sl-select-search-dropdown::-webkit-scrollbar-track{background:var(--bg-secondary,rgba(0,0,0,.05))}
.sl-select-search-dropdown::-webkit-scrollbar-thumb{background:var(--bg-hover,rgba(0,0,0,.15));border-radius:3px}
";
    }

    private static string GetSearchableJs(string baseId, string wrapperId, string searchInputId, string dropdownId)
    {
        return $@"
(function(){{
var w=document.getElementById('{wrapperId}');
if(!w)return;
var h=w.querySelector('input[type=hidden]'),
    s=document.getElementById('{searchInputId}'),
    d=document.getElementById('{dropdownId}'),
    nr=d.querySelector('.sl-select-search-no-result'),
    opts=function(){{return d.querySelectorAll('.sl-select-search-option');}},
    isOpen=false,hi=-1;

function getSelText(){{
    var all=opts();
    for(var i=0;i<all.length;i++)
        if(all[i].getAttribute('data-value')===h.value)return all[i].textContent;
    return '';
}}

function filter(){{
    var q=s.value.toLowerCase(),c=0,all=opts();
    for(var i=0;i<all.length;i++){{
        var m=all[i].textContent.toLowerCase().indexOf(q)!==-1;
        all[i].style.display=m?'':'none';
        if(m)c++;
    }}
    nr.style.display=c?'none':'';
    hi=-1;clrHi();
}}

function clrHi(){{
    var hl=d.querySelectorAll('.sl-select-search-option.highlighted');
    for(var i=0;i<hl.length;i++)hl[i].classList.remove('highlighted');
}}

function setHi(idx){{
    clrHi();
    var vis=[];
    var all=opts();
    for(var i=0;i<all.length;i++)
        if(all[i].style.display!=='none'&&!all[i].hasAttribute('disabled'))vis.push(all[i]);
    if(!vis.length)return;
    idx=((idx%vis.length)+vis.length)%vis.length;
    vis[idx].classList.add('highlighted');
    vis[idx].scrollIntoView({{block:'nearest'}});
    hi=idx;
}}

function sel(o){{
    h.value=o.getAttribute('data-value');
    s.value=o.textContent;
    close();
    h.dispatchEvent(new Event('change'));
}}

function open_(){{
    if(isOpen)return;
    isOpen=true;
    d.style.display='block';
    w.classList.add('open');
    s.select();
    filter();
}}

function close(){{
    isOpen=false;
    d.style.display='none';
    w.classList.remove('open');
    hi=-1;clrHi();
    s.value=getSelText();
}}

s.addEventListener('focus',open_);
s.addEventListener('input',filter);

s.addEventListener('keydown',function(e){{
    if(e.key==='ArrowDown'){{e.preventDefault();if(!isOpen)open_();setHi(hi+1);}}
    else if(e.key==='ArrowUp'){{e.preventDefault();setHi(hi-1);}}
    else if(e.key==='Enter'){{e.preventDefault();var hl=d.querySelector('.sl-select-search-option.highlighted');if(hl)sel(hl);}}
    else if(e.key==='Escape'){{close();}}
}});

d.addEventListener('mousedown',function(e){{
    var o=e.target.closest('.sl-select-search-option');
    if(o&&!o.hasAttribute('disabled')){{e.preventDefault();sel(o);}}
}});

w.querySelector('.sl-select-search-arrow').addEventListener('click',function(e){{
    e.stopPropagation();
    if(isOpen)close();else open_();
}});

document.addEventListener('click',function(e){{
    if(!w.contains(e.target))close();
}});
}})();
";
    }
}
