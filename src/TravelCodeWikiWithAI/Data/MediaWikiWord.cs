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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.MediaWiki\MediaWikiWord.cs
// 迁移变更：
//   1. 去除 System.Drawing / System.Drawing.Imaging（WinForms/GDI+ 依赖）
//   2. 去除 System.Xml（XML 序列化）→ SaveXMLWithNode/LoadXMLWithNode 全部移除
//   3. 去除 System.ComponentModel.Description 特性（WinForms PropertyGrid）
//   4. CoreTools → SysTool（GetBaseLanguage/GetAllLanguage）
//   5. CoreTools.ToHexColor(Color) → 直接使用十六进制字符串 BackgroudColorHex
//   6. LanguageParse 内联到本文件（去除 TravelCodeWikiWithAI.Core 命名空间）
//   7. MediaWikiImage.ReBuild() 改为存根（去除 SvgConverter/Bitmap/Http 依赖）
//   8. MediaWikiImage.option 枚举提取为顶层 MediaWikiImageOption
//   9. Vector2DD → double Longitude / double Latitude
//  10. System.Drawing.Size → int MapWidth / int MapHeight
//  11. CurrencySelect → string SourceCurrencyCode / string TargetCurrencyCode
//  12. MD5CryptoServiceProvider → System.Security.Cryptography.MD5.HashData()
//  13. GeoProject.Self 依赖的方法抛 NotImplementedException（待 ITool 实现）
//  14. MediaWikiTable 去除 SaveXMLWithNode/LoadXMLWithNode/AutoSetLanguage
//  15. MediaWikiChildWord 的 _parent 类型使用 GeoDataBase 强制转换（而非直接访问 GeoDataBase 字段）
//  16. WordDic<string> 的 CheckParent 改为迭代方式（不再有 _parent 字段）

using System.Security.Cryptography;
using System.Text;
using TravelCodeWikiWithAI.TCWTool;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// MediaWiki 词汇基类 / MediaWiki word base class
/// </summary>
public abstract class MediaWikiWord : WordBase
{
    protected MediaWikiWord(GeoDataBase Parent) : base(Parent) { }

    public override string BuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        return MWBuildWord(languageCode, file);
    }

    public abstract string MWBuildWord(string languageCode, Dictionary<string, byte[]> file);
}

[WordDescription("无翻译纯文本")]
public class MediaWikiNoLanguage : MediaWikiWord
{
    public MediaWikiNoLanguage(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => Content;
    public string Content { get; set; } = string.Empty;
    public override string ToString() => Content;
    public override string NeedNewString() => "请输入文本";
    public override bool PostNewString(string str) { if (str == null) return false; Content = str; return true; }
    public override void CheckParent() { }
}

[WordDescription("忽略翻译文本")]
public class MediaWikiIgnoreLanguage : MediaWikiWord
{
    public MediaWikiIgnoreLanguage(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        if (!IsIgnored(languageCode)) return Content[languageCode] ?? "";
        return "";
    }

    public LanguageData Content { get; set; }
    public string IgnoredLanguages { get; set; }
    public override string ToString() => Content?.ToString() ?? "";
    public override string NeedNewString() => "请输入文本内容";
    public override bool PostNewString(string str) => throw new NotImplementedException();

    public bool IsIgnored(string languageCode)
    {
        if (string.IsNullOrWhiteSpace(IgnoredLanguages)) return false;
        foreach (string b in IgnoredLanguages.Split(';'))
            if (languageCode.StartsWith(b)) return true;
        return false;
    }

    public override void CheckParent() { }
}

[WordDescription("有翻译文本")]
public class MediaWikiLanguage : MediaWikiWord
{
    public MediaWikiLanguage(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        if (LanguageData == null) return "";
        string a = LanguageData[languageCode];
        if (a == null) return "";

        string[] b = LanguageParse.GetParams(a);
        if (b.Length == 0) return a;

        Dictionary<string, string> p = new();
        if (Parameters != null)
        {
            foreach (KeyValuePair<string, WordBase> c in Parameters)
            {
                if (c.Value is WordText wt && wt.Content != null)
                {
                    p[c.Key] = wt.Content;
                }
            }
        }

        return LanguageParse.I18N(a, p);
    }

    public LanguageData LanguageData { get; set; }
    public override string ToString() => LanguageData?.ToString() ?? "";
    public WordDic<string> Parameters { get; set; }
    public override string NeedNewString() => "请输入简体中文";
    public override bool PostNewString(string str)
    {
        if (str == null) return false;
        LanguageData = new LanguageData();
        LanguageData.SetZhHans(str);
        return true;
    }

    public override void ReBuild()
    {
        if (LanguageData == null) return;
        foreach (string a in LanguageData.Values)
        {
            string[] b = LanguageParse.GetParams(a);
            if (b == null || b.Length == 0) continue;
            Parameters ??= new WordDic<string>();
            foreach (string c in b)
                if (!Parameters.ContainsKey(c)) Parameters.Add(c, null!);
        }
    }

    public override void CheckParent() { }
}

[WordDescription("子文本")]
public class MediaWikiChildWord : WordBaseWithChild
{
    public MediaWikiChildWord(GeoDataBase Parent) : base(Parent) { }

    public override string BuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        StringBuilder sb = new();
        foreach (WordBase wb in this)
            if (wb != null) sb.Append(wb.BuildWord(languageCode, file));
        return sb.ToString();
    }

    public string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => BuildWord(languageCode, file);
}

[WordDescription("段落及内容")]
public class MediaWikiSection : MediaWikiWord
{
    public MediaWikiSection(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        GeoDataBase a = _parent as GeoDataBase;
        int b = 2;
        while (a != null)
        {
            if (a is MediaWikiSection) b++;
            a = a._parent as GeoDataBase;
        }

        StringBuilder c = new();
        for (int d = 0; d < b; d++) c.Append('=');
        c.Append(Title.MWBuildWord(languageCode, file));
        for (int e = 0; e < b; e++) c.Append('=');
        c.Append('\n');
        if (Content != null) c.Append(Content.BuildWord(languageCode, file));
        c.Append('\n');
        return c.ToString();
    }

    public MediaWikiWord Title { get; set; }
    public MediaWikiChildWord Content { get; set; }
    public override string ToString() => Title == null ? "(空)" : Title.ToString();

    public override void CheckParent()
    {
        if (Title != null) { Title._parent = this; Title.CheckParent(); }
        if (Content != null) { Content._parent = this; Content.CheckParent(); }
    }
}

[WordDescription("加粗")]
public class MediaWikiBold : MediaWikiWord
{
    public MediaWikiBold(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
        => "'''" + Content.MWBuildWord(languageCode, file) + "'''";

    public MediaWikiWord Content { get; set; }
    public override string ToString() => Content == null ? "(空)" : "```" + Content + "```";

    public override void CheckParent()
    {
        if (Content != null) { Content._parent = this; Content.CheckParent(); }
    }
}

[WordDescription("C#内置语言")]
public class MediaWikiCSharpLanguage : MediaWikiWord
{
    public MediaWikiCSharpLanguage(GeoDataBase Parent) : base(Parent) { }

    // 旧项目依赖 GeoProject.Self.Translation.Base 全局单例，新项目待 ITool 实现
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
        => throw new NotImplementedException("MediaWikiCSharpLanguage requires GeoProject, to be implemented via ITool");

    public MediaWikiCSharpLanguageType type { get; set; }
    public override string ToString() => "C#{" + type + "}";
    public override void CheckParent() { }
}

public enum MediaWikiCSharpLanguageType { SiteName, CompoeyName, URL, PageName }

[WordDescription("无序列表")]
public class MediaWikiNoSortList : MediaWikiChildWord
{
    public MediaWikiNoSortList(GeoDataBase Parent) : base(Parent) { }

    public override string BuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        StringBuilder sb = new();
        GeoDataBase a = _parent as GeoDataBase;
        int b = 1;
        while (a != null) { if (a is MediaWikiNoSortList) b++; a = a._parent as GeoDataBase; }

        foreach (WordBase c in this)
        {
            if (c is not MediaWikiNoSortList) { for (int d = 0; d < b; d++) sb.Append('*'); sb.Append(' '); }
            sb.Append(c.BuildWord(languageCode, file));
            if (c is not MediaWikiNoSortList) sb.Append('\n');
        }
        return sb.ToString();
    }
}

[WordDescription("有序列表")]
public class MediaWikiSortList : MediaWikiChildWord
{
    public MediaWikiSortList(GeoDataBase Parent) : base(Parent) { }

    public override string BuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        StringBuilder sb = new();
        GeoDataBase a = _parent as GeoDataBase;
        int b = 1;
        while (a != null) { if (a is MediaWikiNoSortList) b++; a = a._parent as GeoDataBase; }

        foreach (WordBase c in this) { for (int d = 0; d < b; d++) sb.Append('#'); sb.Append(c.BuildWord(languageCode, file)); sb.Append('\n'); }
        return sb.ToString();
    }
}

[WordDescription("内部链接")]
public class MediaWikiLink : MediaWikiWord
{
    public MediaWikiLink(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
        => Display == null ? throw new NotImplementedException() : "[[" + DocTitle.MWBuildWord(languageCode, file) + "|" + Display.MWBuildWord(languageCode, file) + "]]";

    public MediaWikiWord DocTitle { get; set; }
    public MediaWikiWord Display { get; set; }
    public override string ToString() => "[[" + DocTitle + "|" + Display + "]]";

    public override void CheckParent()
    {
        if (DocTitle != null) { DocTitle._parent = this; DocTitle.CheckParent(); }
        if (Display != null) { Display._parent = this; Display.CheckParent(); }
    }
}

[WordDescription("外部链接")]
public class MediaWikiExternalLink : MediaWikiWord
{
    public MediaWikiExternalLink(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
        => Display == null ? throw new NotImplementedException() : "[" + URL.MWBuildWord(languageCode, file) + " " + Display.MWBuildWord(languageCode, file) + "]";

    public MediaWikiWord URL { get; set; }
    public MediaWikiWord Display { get; set; }
    public override string ToString() => (Display?.ToString() ?? "") + " " + (URL?.ToString() ?? "");

    public override void CheckParent()
    {
        if (Display != null) { Display._parent = this; Display.CheckParent(); }
        if (URL != null) { URL._parent = this; URL.CheckParent(); }
    }
}

[WordDescription("系统消息")]
public class MediaWikiSystemMessage : MediaWikiWord
{
    public MediaWikiSystemMessage(GeoDataBase Parent) : base(Parent) { }
    public string SystemMessageName { get; set; }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => "{{int:" + SystemMessageName + "}}";
    public override string ToString() => "{{int:" + SystemMessageName + "}}";
    public override void CheckParent() { }
}

[WordDescription("E-Mail")]
public class MediaWikiMail : MediaWikiWord
{
    public MediaWikiMail(GeoDataBase Parent) : base(Parent) { }
    public string EMail { get; set; }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => "[mailto:" + EMail + " " + EMail + "]";
    public override string ToString() => "[mailto:" + EMail + "]";
    public override void CheckParent() { }
}

[WordDescription("Main Page Div")]
public class MediaWikiMainDiv : MediaWikiWord
{
    public MediaWikiMainDiv(GeoDataBase Parent) : base(Parent) { }

    // 旧项目使用 CoreTools.ToHexColor(Color)，新项目直接用十六进制字符串
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        MediaWikiTemplateInclude mwti = new(this);
        mwti.TemplateName = new MediaWikiNoLanguage(mwti) { Content = "Main Page Div/" + languageCode };
        mwti.Parameters = new Dictionary<string, MediaWikiWord>
        {
            { "title", Title },
            { "ContentTempName", new MediaWikiNoLanguage(mwti) { Content = TempName + "/" + languageCode } },
            { "bcolor", new MediaWikiNoLanguage(mwti) { Content = BackgroudColorHex } }
        };
        return mwti.MWBuildWord(languageCode, file) + "\n";
    }

    public MediaWikiWord Title { get; set; }
    public string TempName { get; set; }
    // 旧项目类型 System.Drawing.Color，新项目改为十六进制字符串
    public string BackgroudColorHex { get; set; } = "#4478BA";
    public override string ToString() => Title?.ToString() ?? "";
    public override void CheckParent() { if (Title != null) { Title._parent = this; Title.CheckParent(); } }
}

[WordDescription("语言代码循环")]
public class MediaWikiLanguageLoop : MediaWikiWord
{
    public MediaWikiLanguageLoop(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        List<string> currentLanguage = new();
        if (AllLanguage) currentLanguage.AddRange(SysTool.GetAllLanguage().Keys);
        else currentLanguage.AddRange(SysTool.GetBaseLanguage());
        if (!HasNormal) currentLanguage.Remove("*");

        StringBuilder e = new();
        foreach (string b in currentLanguage)
        {
            StringBuilder c = new();
            foreach (MediaWikiWord d in Content) c.Append(d.MWBuildWord(b, file));
            e.Append(c);
        }
        return e.ToString();
    }

    public bool AllLanguage { get; set; }
    public bool HasNormal;
    public MediaWikiChildWord Content { get; set; }
    public override string ToString() => (AllLanguage ? "1" : "0") + "|" + (HasNormal ? "1" : "0") + "|" + ((Content == null) ? 0 : Content.Count);

    public override void CheckParent() { if (Content != null) { Content._parent = this; Content.CheckParent(); } }
}

[WordDescription("跨语言链接")]
public class MediaWikiInterwikiLink : MediaWikiWord
{
    public MediaWikiInterwikiLink(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => "[[" + Target.MWBuildWord(languageCode, file) + "|" + Display.MWBuildWord(languageCode, file) + "]]";
    public MediaWikiWord Target { get; set; }
    public MediaWikiWord Display { get; set; }

    public override void CheckParent()
    {
        if (Target != null) { Target._parent = this; Target.CheckParent(); }
        if (Display != null) { Display._parent = this; Display.CheckParent(); }
    }
}

[WordDescription("语言显示名称")]
public class MediaWikiLanguageDisplay : MediaWikiWord
{
    public MediaWikiLanguageDisplay(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => "{{#language:" + languageCode + "}}";
    public override void CheckParent() { }
}

[WordDescription("用户组人数")]
public class MediaWikiNumInGroup : MediaWikiWord
{
    public MediaWikiNumInGroup(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
    public string Group { get; set; } = "*";
    public override void CheckParent() { }
}

[WordDescription("全站页面总数")]
public class MediaWikiNumberOfPages : MediaWikiWord
{
    public MediaWikiNumberOfPages(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => "{{NUMBEROFPAGES}}";
    public override void CheckParent() { }
}

[WordDescription("MediaWiki换行符")]
public class MediaWikiNewLine : MediaWikiWord
{
    public MediaWikiNewLine(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => "\n";
    public override string ToString() => "NewLine";
    public override void CheckParent() { }
}

public class MediaWikiSelfMedia : MediaWikiWord
{
    public MediaWikiSelfMedia(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
    public string URL { get; set; }
    public MediaWikiSelfMediaType Type { get; set; }
}

public enum MediaWikiSelfMediaType { bilibili, xhs, dy, weibo, WeChatVideo, Kuai, YouToBe, X, AcFun, FaceBook, Instagram }

public class MediaWikiTemplateInclude : MediaWikiWord
{
    public MediaWikiTemplateInclude(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        if (TemplateName == null) return null;
        StringBuilder sb = new();
        sb.Append("{{");
        sb.Append(TemplateName.MWBuildWord(languageCode, file));
        if (Parameters != null)
            foreach (string p in Parameters.Keys) { sb.Append('|' + p + '='); sb.Append(Parameters[p].MWBuildWord(languageCode, file)); }
        sb.Append("}}");
        return sb.ToString();
    }

    public MediaWikiWord TemplateName { get; set; }
    public Dictionary<string, MediaWikiWord> Parameters { get; set; }
}

[WordDescription("地理引用")]
public class MediaWikiGeoInclude : MediaWikiWord
{
    public MediaWikiGeoInclude(GeoDataBase Parent) : base(Parent) { }
    // 旧项目依赖 GeoProject.Self.World.GetObject(ID)，新项目待 ITool 实现
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException("MediaWikiGeoInclude requires GeoProject, to be implemented via ITool");
    public string ID { get; set; }
    public override string ToString() => "[GeoRef:" + (ID ?? "?") + "]";
    public override void CheckParent() { }
}

[WordDescription("SWITCH语句")]
public class MediaWikiSwitch : MediaWikiWord
{
    public MediaWikiSwitch(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
    public MediaWikiWord Test { get; set; }
    public MediaWikiWord Default { get; set; }
    public Dictionary<string, MediaWikiWord> Case { get; set; }
}

[WordDescription("重定向页面")]
public class MediaWikiRedirect : MediaWikiDocument
{
    public MediaWikiRedirect(GeoDataBase Parent) : base(Parent) { }
    public string Target { get; set; }
}

public abstract class MediaWikiHtml : MediaWikiWord
{
    public MediaWikiHtml(GeoDataBase Parent) : base(Parent) { }
    public Dictionary<string, MediaWikiWord> Attributes { get; set; }
    public MediaWikiChildWord Content { get; set; }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
    protected abstract string GetHtmlTag();
}

[WordDescription("DIV")]
public class MediaWikiDiv : MediaWikiHtml
{
    public MediaWikiDiv(GeoDataBase Parent) : base(Parent) { }
    protected override string GetHtmlTag() => throw new NotImplementedException();
}

[WordDescription("模版内参数解析")]
public class MediaWikiTemplateParam : MediaWikiWord
{
    public MediaWikiTemplateParam(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
    public string ParamName { get; set; }
    public MediaWikiWord DefaultValue { get; set; }
}

[WordDescription("SPAN")]
public class MediaWikiSpan : MediaWikiHtml
{
    public MediaWikiSpan(GeoDataBase Parent) : base(Parent) { }
    protected override string GetHtmlTag() => throw new NotImplementedException();
}

[WordDescription("货币转换")]
public class MediaWikiCurrencyConverter : MediaWikiWord
{
    public MediaWikiCurrencyConverter(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        List<string> a = new() { Amount.ToString(), SourceCurrencyCode };
        if (!string.IsNullOrEmpty(TargetCurrencyCode)) { /* 待实现 */ }
        return "{{#exchange:" + string.Join("|", a) + "}}";
    }

    public decimal Amount { get; set; }
    // 旧项目类型为 CurrencySelect，新项目改为 string
    public string SourceCurrencyCode { get; set; }
    public string TargetCurrencyCode { get; set; }
    public int xs { get; set; } = -1;
    public override string ToString() => string.IsNullOrWhiteSpace(SourceCurrencyCode) ? "空白币种" : SourceCurrencyCode + "(" + Amount + ")";
    public override void CheckParent() { }
}

[WordDescription("多语言条件语句")]
public class MediaWikiMultiLanguageSwitch : MediaWikiWord
{
    public MediaWikiMultiLanguageSwitch(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
    public Dictionary<string, MediaWikiWord> LanguageContents { get; set; }
    public MediaWikiWord Default { get; set; }
}

[WordDescription("表格")]
public class MediaWikiTable : MediaWikiWord
{
    public MediaWikiTable(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        StringBuilder a = new();
        string b = "{|";
        if (!string.IsNullOrEmpty(TableClass)) b += " class=\"" + TableClass + "\"";
        if (!string.IsNullOrEmpty(TableStyle)) b += " style=\"" + TableStyle + "\"";
        a.Append(b + "\n");
        if (Caption != null) a.Append("|+ " + Caption.MWBuildWord(languageCode, file) + "\n");
        if (HeadRow != null) a.Append(HeadRow.MWBuildWord(languageCode, file) + "\n");
        if (Rows != null) foreach (MediaWikiTableRow d in Rows) a.Append(d.MWBuildWord(languageCode, file));
        a.Append("|}");
        return a.ToString();
    }

    public MediaWikiWord Caption { get; set; }
    public string TableClass { get; set; }
    public string TableStyle { get; set; }
    public List<MediaWikiTableRow> Rows { get; set; }
    public MediaWikiTableRow HeadRow { get; set; }

    public MediaWikiTableRow AddHeaderRow() { var a = new MediaWikiTableRow(this); HeadRow = a; a.IsHeader = true; return a; }
    public MediaWikiTableRow AddDataRow() { var a = new MediaWikiTableRow(this); Rows ??= new(); Rows.Add(a); return a; }
    public void RemoveRow(int index) => throw new NotImplementedException();

    public override void CheckParent()
    {
        if (Caption != null) { Caption._parent = this; Caption.CheckParent(); }
        if (HeadRow != null) { HeadRow._parent = this; HeadRow.CheckParent(); }
        if (Rows != null) foreach (var a in Rows) { a._parent = this; a.CheckParent(); }
    }
}

[WordDescription("表格行")]
public class MediaWikiTableRow : MediaWikiWord
{
    public MediaWikiTableRow(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        StringBuilder a = new();
        a.Append("|-\n");
        string b = IsHeader ? "!" : "|";
        if (Cells != null) foreach (WordBase c in Cells) { var d = c as MediaWikiWord; a.Append(b + " " + d.MWBuildWord(languageCode, file) + "\n"); }
        return a.ToString();
    }

    public bool IsHeader { get; set; }
    public MediaWikiChildWord Cells { get; set; }
    public string RowStyle { get; set; }
    public string RowClass { get; set; } = "";

    public MediaWikiTableCell AddCell(WordBase content = null)
    {
        var a = new MediaWikiTableCell(this) { IsHeader = IsHeader, Content = content };
        Cells ??= new MediaWikiChildWord(this);
        Cells.Add(a);
        return a;
    }

    public void RemoveCell(int index) => throw new NotImplementedException();
    public void SetRowStyle(string cssClass = null, string style = null) => throw new NotImplementedException();

    public override void CheckParent()
    {
        if (Cells != null) foreach (WordBase a in Cells) if (a != null) { a._parent = this; a.CheckParent(); }
    }

    public override string ToString() => Cells == null ? "" : string.Join("|", Cells.Select(c => c.ToString()));
}

[WordDescription("表格单元格")]
public class MediaWikiTableCell : MediaWikiWord
{
    public MediaWikiTableCell(GeoDataBase Parent) : base(Parent) { IsHeader = false; ColSpan = 1; RowSpan = 1; CellStyle = ""; CellClass = ""; }

    public bool IsHeader { get; set; }
    public WordBase Content { get; set; }
    public int ColSpan { get; set; }
    public int RowSpan { get; set; }
    public string CellStyle { get; set; }
    public string CellClass { get; set; }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        if (!string.IsNullOrEmpty(CellClass) || !string.IsNullOrEmpty(CellStyle) || ColSpan != 1 || RowSpan != 1) throw new NotImplementedException();
        if (Content != null) { string a = Content.BuildWord(languageCode, file); return (Content is MediaWikiChildWord) ? "\n" + a : a; }
        return " ";
    }

    public void SetContent(MediaWikiWord content) => Content = content;
    public void SetCellProperties(int? colSpan = null, int? rowSpan = null, string cssClass = null, string style = null) { if (colSpan > 0) ColSpan = colSpan.Value; if (rowSpan > 0) RowSpan = rowSpan.Value; if (cssClass != null) CellClass = cssClass; if (style != null) CellStyle = style; }
    public void ToggleCellType(bool isHeader) => IsHeader = isHeader;
    public void SetSpan(int colSpan, int rowSpan) { if (colSpan > 0) ColSpan = colSpan; if (rowSpan > 0) RowSpan = rowSpan; }
    public void ClearContent() => Content = null;
    public bool IsEmpty() => Content == null;

    public override void CheckParent() { if (Content != null) { Content._parent = this; Content.CheckParent(); } }
    public override string ToString() => Content?.ToString() ?? "";
}

[WordDescription("内联代码")]
public class MediaWikiInlineCode : MediaWikiWord
{
    public MediaWikiInlineCode(GeoDataBase Parent) : base(Parent) { }
    public MediaWikiWord Content { get; set; }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
}

[WordDescription("代码块")]
public class MediaWikiCodeBlock : MediaWikiWord
{
    public MediaWikiCodeBlock(GeoDataBase Parent) : base(Parent) { }
    public MediaWikiWord Content { get; set; }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
}

[WordDescription("语法高亮代码")]
public class MediaWikiSyntaxHighlight : MediaWikiWord
{
    public MediaWikiSyntaxHighlight(GeoDataBase Parent) : base(Parent) { }
    public MediaWikiWord? Content { get; set; }
    public string Language { get; set; } = "text";
    public bool LineNumbers { get; set; }
    public int StartLine { get; set; } = 1;
    public string HighlightLines { get; set; }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
}

[WordDescription("图片")]
public class MediaWikiImage : MediaWikiWord
{
    public MediaWikiImage(GeoDataBase Parent) : base(Parent) { }

    public string OrgPath { get; set; }
    public WordBase Description { get; set; }
    public byte[] OrgData { get; set; }
    public byte[] ImageData { get; set; }
    public int Size { get; set; }
    public MediaWikiImageOption Option { get; set; }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        if (ImageData == null || ImageData.Length == 0) { /* 新项目：图片数据应通过 ITool 预先准备好 */ }
        if (ImageData == null || ImageData.Length == 0) return "[[File:missing.png|" + Size + "px]]";

        List<string> a = new();
        byte[] c = MD5.HashData(ImageData);
        string d = Convert.ToHexString(c);
        a.Add("File:" + d + ".png");
        if (Option != MediaWikiImageOption.None) a.Add(Option.ToString());
        a.Add(Size + "px");

        string e = "[[" + string.Join('|', a) + "]]";
        if (file.ContainsKey(d + ".png")) file[d + ".png"] = ImageData;
        else file.Add(d + ".png", ImageData);
        return e;
    }

    // 旧项目通过 Http.Get + SvgConverter + Bitmap 处理图片，新项目改为存根
    public override void ReBuild() { }

    public override void CheckParent() { if (Description != null) { Description._parent = this; Description.CheckParent(); } }
    public override Dictionary<string, byte[]> BuildFile() => throw new NotImplementedException();
}

public enum MediaWikiImageOption { None, border, frame, thumb, frameless }

[WordDescription("固定月份单词")]
public class MediaWikiFixedMonth : MediaWikiWord
{
    public MediaWikiFixedMonth(GeoDataBase Parent) : base(Parent) { }
    public int Month { get; set; }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
}

[WordDescription("固定日期单词")]
public class MediaWikiFixedDay : MediaWikiWord
{
    public MediaWikiFixedDay(GeoDataBase Parent) : base(Parent) { }
    public int Day { get; set; }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
}

[WordDescription("时区")]
public class MediaWikiTimeZone : MediaWikiWord
{
    public MediaWikiTimeZone(GeoDataBase Parent) : base(Parent) { }
    public TimeZoneType type { get; set; }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => type switch
    {
        TimeZoneType.UTC => "UTC+0", TimeZoneType.UTC_Plus_1 => "UTC+1", TimeZoneType.UTC_Plus_2 => "UTC+2",
        TimeZoneType.UTC_Plus_3 => "UTC+3", TimeZoneType.UTC_Plus_3_30 => "UTC+3:30", TimeZoneType.UTC_Plus_4 => "UTC+4",
        TimeZoneType.UTC_Plus_4_30 => "UTC+4:30", TimeZoneType.UTC_Plus_5 => "UTC+5", TimeZoneType.UTC_Plus_5_30 => "UTC+5:30",
        TimeZoneType.UTC_Plus_5_45 => "UTC+5:45", TimeZoneType.UTC_Plus_6 => "UTC+6", TimeZoneType.UTC_Plus_6_30 => "UTC+6:30",
        TimeZoneType.UTC_Plus_7 => "UTC+7", TimeZoneType.UTC_Plus_8 => "UTC+8", TimeZoneType.UTC_Plus_9 => "UTC+9",
        TimeZoneType.UTC_Plus_9_30 => "UTC+9:30", TimeZoneType.UTC_Plus_10 => "UTC+10", TimeZoneType.UTC_Plus_10_30 => "UTC+10:30",
        TimeZoneType.UTC_Plus_11 => "UTC+11", TimeZoneType.UTC_Plus_12 => "UTC+12",
        TimeZoneType.UTC_Minus_1 => "UTC-1", TimeZoneType.UTC_Minus_2 => "UTC-2", TimeZoneType.UTC_Minus_3 => "UTC-3",
        TimeZoneType.UTC_Minus_3_30 => "UTC-3:30", TimeZoneType.UTC_Minus_4 => "UTC-4", TimeZoneType.UTC_Minus_5 => "UTC-5",
        TimeZoneType.UTC_Minus_6 => "UTC-6", TimeZoneType.UTC_Minus_7 => "UTC-7", TimeZoneType.UTC_Minus_8 => "UTC-8",
        TimeZoneType.UTC_Minus_9 => "UTC-9", TimeZoneType.UTC_Minus_9_30 => "UTC-9:30", TimeZoneType.UTC_Minus_10 => "UTC-10",
        TimeZoneType.UTC_Minus_11 => "UTC-11", TimeZoneType.UTC_Minus_12 => "UTC-12",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "不支持的时区类型")
    };

    public override void CheckParent() { }
    public override string ToString() => MWBuildWord("*", null);
}

public enum TimeZoneType
{
    UTC, UTC_Plus_1, UTC_Plus_2, UTC_Plus_3, UTC_Plus_3_30, UTC_Plus_4, UTC_Plus_4_30,
    UTC_Plus_5, UTC_Plus_5_30, UTC_Plus_5_45, UTC_Plus_6, UTC_Plus_6_30, UTC_Plus_7,
    UTC_Plus_8, UTC_Plus_9, UTC_Plus_9_30, UTC_Plus_10, UTC_Plus_10_30, UTC_Plus_11, UTC_Plus_12,
    UTC_Minus_1, UTC_Minus_2, UTC_Minus_3, UTC_Minus_3_30, UTC_Minus_4, UTC_Minus_5,
    UTC_Minus_6, UTC_Minus_7, UTC_Minus_8, UTC_Minus_9, UTC_Minus_9_30, UTC_Minus_10, UTC_Minus_11, UTC_Minus_12
}

[WordDescription("自媒体信息列表")]
public class MediaWikiSelfMediaList : MediaWikiWord
{
    public MediaWikiSelfMediaList(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException();
    public string bilibili { get; set; } public string xhs { get; set; } public string dy { get; set; }
    public string weibo { get; set; } public string WeChatVideo { get; set; } public string Kuai { get; set; }
    public string YouToBe { get; set; } public string X { get; set; } public string AcFun { get; set; }
    public string FaceBook { get; set; } public string Instagram { get; set; }
}

[WordDescription("自媒体账号列表")]
public class MediaWikiSelfMediaAccountList : MediaWikiWord
{
    public MediaWikiSelfMediaAccountList(GeoDataBase Parent) : base(Parent) { }
    // 旧项目依赖 GeoProject.Self.Translation.Base，新项目待 ITool 实现
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => throw new NotImplementedException("MediaWikiSelfMediaAccountList requires GeoProject translation data, to be implemented via ITool");
    public string bilibili { get; set; } public string bilibiliAccount { get; set; }
    public string xhs { get; set; } public string xhsAccount { get; set; }
    public string dy { get; set; } public string dyAccount { get; set; }
    public string weibo { get; set; } public string weiboAccount { get; set; }
    public string WeChatVideo { get; set; } public string WeChatVideoAccount { get; set; }
    public string Kuai { get; set; } public string KuaiAccount { get; set; }
    public string YouToBe { get; set; } public string YouToBeAccount { get; set; }
    public string X { get; set; } public string XAccount { get; set; }
    public string AcFun { get; set; } public string AcFunAccount { get; set; }
    public string FaceBook { get; set; } public string FaceBookAccount { get; set; }
    public string Instagram { get; set; } public string InstagramAccount { get; set; }
    public override void CheckParent() { }
}

[WordDescription("多语言链接")]
public class MediaWikiMulityLanguageLink : MediaWikiWord
{
    public MediaWikiMulityLanguageLink(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        if (_parent is not MediaWikiDocument) throw new NotImplementedException();
        MediaWikiDocument d = _parent as MediaWikiDocument;
        string[] a = SysTool.GetBaseLanguage();
        StringBuilder c = new();
        foreach (string b in a) if (b != "*") c.Append("[[" + b + ":" + d.Title + "/" + b + "]]\n");
        return c.ToString();
    }
}

[WordDescription("并列文本链接")]
public class MediaWikiCombinString : MediaWikiWord
{
    public MediaWikiCombinString(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        if (ChildString == null) return "";
        List<string> a = new();
        foreach (WordBase b in ChildString) a.Add((b as MediaWikiWord).MWBuildWord(languageCode, file));
        return string.Join(SplitString, a);
    }

    public string SplitString { get; set; }
    public MediaWikiChildWord ChildString { get; set; }

    public override string ToString()
    {
        if (ChildString == null) return "(空)";
        return string.Join(SplitString, ChildString.Select(b => b.ToString()));
    }

    public override void CheckParent() { if (ChildString != null) { ChildString._parent = this; ChildString.CheckParent(); } }
}

[WordDescription("当前语言代码")]
public class MediaWikiCurrentLanguageCode : MediaWikiWord
{
    public MediaWikiCurrentLanguageCode(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => languageCode;
    public override void CheckParent() { }
}

[WordDescription("鼠标悬停提示")]
public class MediaWikiHoverTip : MediaWikiWord
{
    public MediaWikiHoverTip(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => "<span title=\"" + TipContent.MWBuildWord(languageCode, file) + "\">" + Content.MWBuildWord(languageCode, file) + "</span>";
    public MediaWikiWord TipContent { get; set; }
    public MediaWikiWord Content { get; set; }

    public override void CheckParent()
    {
        if (TipContent != null) { TipContent._parent = this; TipContent.CheckParent(); }
        if (Content != null) { Content._parent = this; Content.CheckParent(); }
    }
}

[WordDescription("日期格式化显示")]
public class MediaWikiDate : MediaWikiWord
{
    public MediaWikiDate(GeoDataBase Parent) : base(Parent) { }
    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file) => "{{#formatdate:" + Date.Year + "-" + Date.Month + "-" + Date.Day + "|ymd}}";
    public DateTime Date { get; set; }
    public override void CheckParent() { }
}

[WordDescription("地理信息编码")]
public class MediaWikiGeo : MediaWikiWord
{
    public MediaWikiGeo(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        string a = "{{#geo:" + Longitude + "," + Latitude + "|" + Zoom + "|";
        if (LocationName.ContainsKey(languageCode)) a += LocationName[languageCode]; else a += LocationName["*"];
        if (!string.IsNullOrEmpty(Page)) a += "|" + Page + "/" + languageCode;
        return a + "}}";
    }

    // 旧项目类型 Vector2DD，新项目改为 double
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int Zoom { get; set; }
    public LanguageData LocationName { get; set; }
    public string Page { get; set; }
    public override void CheckParent() { }
}

[WordDescription("地图")]
public class MediaWikiMap : MediaWikiWord
{
    public MediaWikiMap(GeoDataBase Parent) : base(Parent) { }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        string a = "{{#map:" + Longitude + "," + Latitude + "|" + Zoom + "|";
        if (LocationName.ContainsKey(languageCode)) a += LocationName[languageCode]; else a += LocationName["*"];
        a += "|" + Page + "/" + languageCode + "|" + MapWidth + "x" + MapHeight + "}}";
        return a;
    }

    // 旧项目类型 Vector2DD，新项目改为 double
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int Zoom { get; set; }
    public LanguageData LocationName { get; set; }
    public string Page { get; set; }
    // 旧项目类型 System.Drawing.Size，新项目改为 int
    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
    public override void CheckParent() { }
}

[WordDescription("车牌号")]
public class MediaWikiLicensePlate : MediaWikiWord
{
    public MediaWikiLicensePlate(GeoDataBase Parent) : base(Parent) { PlateType = LicensePlateType.ChinaSmallVehicle; Size = LicensePlateSize.Normal; }

    public override string MWBuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        if (string.IsNullOrWhiteSpace(PlateNumber)) return "";
        StringBuilder sb = new();
        sb.Append($"<span class=\"license-plate {GetPlateClass()}\" style=\"{GetPlateStyle()}\">");
        sb.Append(FormatPlateNumber());
        sb.Append("</span>");
        return sb.ToString();
    }

    public string PlateNumber { get; set; }
    public LicensePlateType PlateType { get; set; }
    public LicensePlateSize Size { get; set; }
    public bool ShowBorder { get; set; } = true;

    private string GetPlateClass() => PlateType switch
    {
        LicensePlateType.ChinaSmallVehicle => "plate-blue", LicensePlateType.ChinaLargeVehicle or LicensePlateType.ChinaTrailer or LicensePlateType.ChinaCoach => "plate-yellow",
        LicensePlateType.ChinaNewEnergySmall => "plate-green-small", LicensePlateType.ChinaNewEnergyLarge => "plate-green-large",
        LicensePlateType.ChinaEmbassy or LicensePlateType.ChinaConsulate or LicensePlateType.ChinaHongKongMacao => "plate-black",
        LicensePlateType.ChinaPolice or LicensePlateType.ChinaEmergency or LicensePlateType.ChinaArmedPolice or LicensePlateType.ChinaMilitary or LicensePlateType.ChinaMilitaryLarge => "plate-white",
        _ => "plate-default"
    };

    private string GetPlateStyle()
    {
        StringBuilder style = new();
        style.Append("display: inline-block; font-family: 'Arial Black', sans-serif; font-weight: bold; padding: 4px 8px; border-radius: 4px; ");
        if (ShowBorder) style.Append("border: 2px solid #000; ");
        style.Append(Size switch { LicensePlateSize.Small => "font-size: 12px; ", LicensePlateSize.Large => "font-size: 20px; ", _ => "font-size: 16px; " });
        style.Append(PlateType switch
        {
            LicensePlateType.ChinaSmallVehicle => "background: linear-gradient(to bottom, #0066cc, #0052a3); color: white; ",
            LicensePlateType.ChinaLargeVehicle or LicensePlateType.ChinaTrailer or LicensePlateType.ChinaCoach => "background: linear-gradient(to bottom, #ffcc00, #e6b800); color: black; ",
            LicensePlateType.ChinaNewEnergySmall => "background: linear-gradient(to bottom, white, #90EE90); color: black; ",
            LicensePlateType.ChinaNewEnergyLarge => "background: linear-gradient(to right, #ffcc00 0%, #ffcc00 35%, #90EE90 35%, #90EE90 100%); color: black; ",
            LicensePlateType.ChinaEmbassy or LicensePlateType.ChinaConsulate or LicensePlateType.ChinaHongKongMacao => "background: black; color: white; ",
            _ => "background: white; color: black; "
        });
        return style.ToString();
    }

    private string FormatPlateNumber()
    {
        if (string.IsNullOrWhiteSpace(PlateNumber)) return "";
        string f = PlateNumber;
        switch (PlateType)
        {
            case LicensePlateType.ChinaNewEnergySmall or LicensePlateType.ChinaNewEnergyLarge:
                if (f.Length >= 3 && !f.Contains("·")) f = f.Insert(2, "·");
                break;
            case LicensePlateType.ChinaTrailer:
                if (f.Length >= 3) { string rc = f[..2], n = f[2..]; if (!n.EndsWith("挂")) n += "挂"; f = $"<span style='display:block;vertical-align:bottom;padding-bottom:2px;'>{rc}</span><span style='display:block;'>{n}</span>"; }
                else if (!f.EndsWith("挂")) f += "挂";
                break;
            case LicensePlateType.ChinaCoach: if (!f.EndsWith("学")) f += "<span style='color:red;'>学</span>"; break;
            case LicensePlateType.ChinaPolice: f = f.EndsWith("警") ? f[..^1] + "<span style='color:red;'>警</span>" : f + "<span style='color:red;'>警</span>"; break;
            case LicensePlateType.ChinaEmergency: f = f.EndsWith("应急") ? f[..^2] + "<span style='color:red;'>应急</span>" : f + "<span style='color:red;'>应急</span>"; break;
            case LicensePlateType.ChinaArmedPolice:
                if (f.StartsWith("WJ") && f.Length >= 8) { string p = f[2..3], n = f[3..]; f = $"<div style='display:inline-block;'><div style='height:2px;background:red;'></div><div style='height:2px;background:red;margin-top:2px;'></div><div style='padding:4px 8px;'><span style='display:block;'><span style='color:red;'>WJ</span> 🛡️ <span style='color:red;'>{p}</span></span><span style='display:block;'>{n}</span></div><div style='height:2px;background:red;margin-bottom:2px;'></div><div style='height:2px;background:red;'></div></div>"; }
                break;
            case LicensePlateType.ChinaMilitary:
                if (f.Contains("·")) { string[] pts = f.Split('·'); if (pts.Length == 2) f = $"<span style='color:red;'>{pts[0]}</span><span style='color:red;'>·</span>{pts[1]}"; }
                else if (f.Length >= 7) f = $"<span style='color:red;'>{f[..2]}</span><span style='color:red;'>·</span>{f[2..]}";
                break;
            case LicensePlateType.ChinaMilitaryLarge: if (f.Length >= 7) f = $"<span style='display:block;color:red;'>{f[..2]}</span><span style='display:block;'>{f[2..]}</span>"; break;
            case LicensePlateType.ChinaEmbassy: if (!f.EndsWith("使")) f += "<span style='color:white;'>使</span>"; break;
            case LicensePlateType.ChinaConsulate: if (!f.EndsWith("领")) f += "<span style='color:white;'>领</span>"; break;
            case LicensePlateType.ChinaHongKongMacao: f = f.Contains("港") ? f.Replace("港", "<span style='color:white;'>港</span>") : f.Replace("澳", "<span style='color:white;'>澳</span>"); break;
        }
        return f;
    }

    public override string ToString() => string.IsNullOrWhiteSpace(PlateNumber) ? "(空车牌)" : $"{PlateNumber} ({PlateType})";
    public override void CheckParent() { }
}

public enum LicensePlateType
{
    ChinaSmallVehicle, ChinaLargeVehicle, ChinaTrailer, ChinaNewEnergySmall, ChinaNewEnergyLarge,
    ChinaEmbassy, ChinaConsulate, ChinaHongKongMacao, ChinaCoach, ChinaPolice, ChinaEmergency,
    ChinaArmedPolice, ChinaMilitary, ChinaMilitaryLarge
}

public enum LicensePlateSize { Small, Normal, Large }

// ========== 内联 LanguageParse（替代旧项目 TravelCodeWikiWithAI.Core.LanguageParse）==========
internal static class LanguageParse
{
    public static string I18N(string content, Dictionary<string, string> param)
    {
        if (content == null) return null;
        if (param == null || param.Count == 0) return content;

        StringBuilder sb = new();
        int i = 0;
        char sk = ' ';
        List<LanguageInfo> lis = new();
        foreach (char c in content)
        {
            if (c == '{') { i++; if (i == 2) { if (sb.Length != 0) lis.Add(new() { Content = sb.ToString(), Need = false }); sb = new(); } }
            else if (c == '}') { if (i < 2) { if (sk == '}' && i == 1) i--; else sb.Append(c); } else { sb.Append(c); i = 0; } }
            else { sb.Append(c); }
            sk = c;
        }
        if (sb.Length != 0) lis.Add(new() { Content = sb.ToString(), Need = sk == '}' });

        sb = new();
        foreach (var li in lis) sb.Append(li.Need ? (param.ContainsKey(li.Content) ? param[li.Content] : "((" + li.Content + "))") : li.Content);
        return sb.ToString();
    }

    public static string[] GetParams(string content)
    {
        var a = GetLanguageInfos(content);
        if (a == null) return [];
        List<string> b = new();
        foreach (var li in a) if (li.Need) b.Add(li.Content);
        return b.ToArray();
    }

    private static List<LanguageInfo> GetLanguageInfos(string content)
    {
        if (content == null) return null;
        List<LanguageInfo> result = new();
        StringBuilder sb = new();
        foreach (char a in content)
        {
            if (a == '{') { result.Add(new() { Content = sb.ToString(), Need = false }); sb = new(); }
            else if (a == '}') { result.Add(new() { Content = sb.ToString(), Need = true }); sb = new(); }
            else sb.Append(a);
        }
        if (sb.Length != 0) result.Add(new() { Content = sb.ToString(), Need = false });
        return result;
    }
}

internal struct LanguageInfo
{
    public string Content;
    public bool Need;
}
