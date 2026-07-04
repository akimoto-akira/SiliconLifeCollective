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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\DocumentBase.cs
// 迁移变更：去除 XMLBase/XML 序列化相关方法
//           去除 [Editor]/[TypeConverter] WinForms 特性
//           去除 CoreTools/OutPut 等旧项目依赖
//           保留 WordBase/WordBaseWithChild/DocumentBase 核心类层次
//           WordBase.GetAllWordTypes() 反射保留（后续可改用 ITool 注册）
//           BuildWord/BuildDocument 保留基本框架
//           去除 MarkdownName/MarkdownDisable/WordDescription 等自定义特性中的旧依赖

using System.Collections;
using System.Reflection;
using System.Xml;
using SiliconLife.Collective;
using SiliconLife.Speedy;
using TravelCodeWikiWithAI.TCWTool;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 词条基类 / Entry base class
/// </summary>
public abstract class DocumentBase : GeoDataBase
{
    protected DocumentBase(GeoDataBase? Parent) : base(Parent) { }
    protected DocumentBase() { }

    /// <summary>
    /// 词条标题 / Entry title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    public override string ToString() => Title;

    /// <summary>
    /// 词条内容 / Entry contents
    /// </summary>
    public virtual WordBaseWithChild? Contents { get; set; }

    public override string? NeedNewString() => "请输入文档标题";

    public override bool PostNewString(string str)
    {
        Title = str;
        return true;
    }

    /// <summary>
    /// 重新构建内容 / Rebuild contents
    /// </summary>
    public virtual void ReBuild()
    {
        if (Contents != null)
        {
            Contents.ReBuild();
        }
    }

    public override Dictionary<string, string> BuildDocument(Dictionary<string, byte[]> files)
    {
        if (Title == null || Contents == null)
        {
            return new Dictionary<string, string>();
        }

        Dictionary<string, string> allLanguage = SysTool.GetAllLanguage();
        Dictionary<string, string> d = new Dictionary<string, string>();
        foreach (string b in allLanguage.Keys)
        {
            if (b == "*")
            {
                continue;
            }

            string c = Contents.BuildWord(b, files);
            d.Add(Title + "/" + b, c);
        }

        return d;
    }
}

/// <summary>
/// 词条标题特性 / Entry title attribute
/// </summary>
public class DocumentTitleAttribute : Attribute
{
    public string Title { get; set; }
    public DocumentTitleAttribute(string title) { Title = title; }
}

/// <summary>
/// 单词基类 / Word base class
/// 迁移自旧项目 WordBase，去除 XML 序列化和 WinForms 特性
/// </summary>
public class WordBase : GeoDataBase
{
    public WordBase(GeoDataBase? Parent) : base(Parent) { }
    public WordBase() { }

    /// <summary>
    /// 获取所有单词类型（通过反射）/ Get all word types (via reflection)
    /// 后续可改用 ServiceLocator.RegisterToolAssembly() 模式
    /// </summary>
    public static Type[] GetAllWordTypes()
    {
        var registry = ServiceLocator.Instance.TypeRegistry;
        if (registry != null)
        {
            return registry.FindSubtypesOf(typeof(WordBase)).ToArray();
        }
        return Array.Empty<Type>();
    }

    /// <summary>
    /// 重新构建单词 / Rebuild word
    /// </summary>
    public virtual void ReBuild()
    {
        PropertyInfo[] allProps = GetType().GetProperties();
        List<PropertyInfo> doProps = new List<PropertyInfo>();
        foreach (PropertyInfo prop in allProps)
        {
            if (prop.CanRead && prop.CanWrite)
            {
                doProps.Add(prop);
            }
        }

        foreach (PropertyInfo a in doProps)
        {
            object b = a.GetValue(this)!;
            if (b is WordBase c)
            {
                c.ReBuild();
            }
        }
    }

    /// <summary>
    /// 构建单词内容 / Build word content
    /// </summary>
    public virtual string BuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// 构建文件 / Build file
    /// </summary>
    public virtual Dictionary<string, byte[]> BuildFile()
    {
        return new Dictionary<string, byte[]>();
    }

    private static WordBase? CopyObject = null;
    private static int Type = 0;

    public static WordBase? GetCopyObject(out int type)
    {
        type = Type;
        return CopyObject;
    }

    public static void SetCopyObject(WordBase obj, int type)
    {
        CopyObject = obj;
        Type = type;
    }

    public override void CheckParent()
    {
        // WordBase 不需要 CheckParent / WordBase doesn't need CheckParent
    }
}

/// <summary>
/// 带子单词的单词基类 / Word base class with children
/// </summary>
public abstract class WordBaseWithChild : WordBase, IList<WordBase>
{
    private List<WordBase> _list = new List<WordBase>();

    public WordBaseWithChild(GeoDataBase? Parent) : base(Parent) { }
    public WordBaseWithChild() { }

    public IEnumerator<WordBase> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(WordBase item) => _list.Add(item);
    public void Clear() => _list.Clear();
    public bool Contains(WordBase item) => _list.Contains(item);
    public void CopyTo(WordBase[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
    public bool Remove(WordBase item) => _list.Remove(item);
    public int Count => _list.Count;
    public bool IsReadOnly => false;
    public int IndexOf(WordBase item) => _list.IndexOf(item);
    public void Insert(int index, WordBase item) => _list.Insert(index, item);
    public void RemoveAt(int index) => _list.RemoveAt(index);

    public WordBase this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }

    /// <summary>
    /// 分隔字符串 / Split string
    /// </summary>
    public string? SplitString { get; set; }

    /// <summary>
    /// 子字符串 / Child string
    /// </summary>
    public WordBaseWithChild? ChildString { get; set; }

    public override void ReBuild()
    {
        foreach (WordBase a in _list)
        {
            a.ReBuild();
        }
    }

    public override string BuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        List<string> parts = new List<string>();
        foreach (WordBase a in _list)
        {
            string b = a.BuildWord(languageCode, file);
            parts.Add(b);
        }

        string separator = SplitString ?? "\n";
        return string.Join(separator, parts);
    }

    public override string[] GetPath(int deth)
    {
        List<string> a = new List<string>();
        int d = 0;
        foreach (WordBase b in this)
        {
            a.Add(BasePath + "[" + d + "]");
            string[] c = b.GetPath((deth == -1) ? -1 : deth - 1);
            a.AddRange(c);
            d++;
        }

        return a.ToArray();
    }

    /// <summary>
    /// 从 XML 节点加载子单词列表 / Load child word list from XML node
    /// 搬运自细需求 WordBaseWithChild.LoadXMLWithNode
    /// </summary>
    public override bool LoadXMLWithNode(XmlNode node, SpeedyPack pack)
    {
        foreach (XmlNode a in node.ChildNodes)
        {
            switch (a.Name)
            {
                case "Word":
                    string? b = a.Attributes?["Type"]?.Value;
                    if (b == null) continue;
                    Type? c = FindType(b);
                    if (c == null) continue;
                    WordBase d = (WordBase)CreateChild(c, this);
                    if (!d.LoadXMLWithNode(a, pack))
                    {
                        throw new NotImplementedException();
                    }
                    Add(d);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return true;
    }

    /// <summary>
    /// 将子单词列表保存到 XML 节点 / Save child word list to XML node
    /// 搬运自细需求 WordBaseWithChild.SaveXMLWithNode
    /// </summary>
    public override bool SaveXMLWithNode(XmlNode node, XmlDocument document, SpeedyPack pack)
    {
        foreach (WordBase a in this)
        {
            XmlElement b = document.CreateElement("Word");
            b.SetAttribute("Type", a.GetType().FullName!);
            if (!a.SaveXMLWithNode(b, document, pack))
            {
                throw new NotImplementedException();
            }
            node.AppendChild(b);
        }

        return true;
    }
}

/// <summary>
/// 通用文本单词 / Plain text word
/// </summary>
public class WordText : WordBase
{
    public WordText() { }
    public WordText(GeoDataBase? Parent) : base(Parent) { }

    public string? Content { get; set; }

    public override string BuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        return Content ?? "";
    }
}

/// <summary>
/// 多语言单词 / Language word
/// </summary>
public class WordLanguage : WordBase
{
    public WordLanguage() { }
    public WordLanguage(GeoDataBase? Parent) : base(Parent) { }

    public LanguageData? Language { get; set; }

    public override string BuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        if (Language == null) return "";
        return Language[languageCode] ?? "";
    }
}

/// <summary>
/// 链接单词 / Link word
/// </summary>
public class WordLink : WordBaseWithChild
{
    public WordLink() { }
    public WordLink(GeoDataBase? Parent) : base(Parent) { }

    public WordBase? Display { get; set; }
    public WordBase? Target { get; set; }

    public override string BuildWord(string languageCode, Dictionary<string, byte[]> file)
    {
        string display = Display?.BuildWord(languageCode, file) ?? "";
        string target = Target?.BuildWord(languageCode, file) ?? "";
        return $"[[{target}|{display}]]";
    }
}

/// <summary>
/// Markdown 描述特性 / Markdown description attribute
/// </summary>
public class MarkdownNameAttribute : Attribute
{
    public string Name { get; set; }
    public MarkdownNameAttribute(string name) { Name = name; }
}

/// <summary>
/// Markdown 禁用特性 / Markdown disable attribute
/// </summary>
public class MarkdownDisableAttribute : Attribute
{
}

/// <summary>
/// 单词描述特性 / Word description attribute
/// </summary>
public class WordDescriptionAttribute : Attribute
{
    public string Description { get; set; }
    public WordDescriptionAttribute(string description) { Description = description; }
}

/// <summary>
/// 单词字典 / Word dictionary
/// 搬运自细需求 WordDic，继承 WordBase 并实现 IDictionary
/// </summary>
public class WordDic<K> : WordBase, IDictionary<K, WordBase>, IDictionary
{
    public WordDic(GeoDataBase? Parent) : base(Parent) { }
    public WordDic() { }

    private Dictionary<K, WordBase> _data = new Dictionary<K, WordBase>();

    public IEnumerator<KeyValuePair<K, WordBase>> GetEnumerator() => new Dictionary<K, WordBase>(_data).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    IDictionaryEnumerator IDictionary.GetEnumerator() => throw new NotImplementedException();

    public void Add(KeyValuePair<K, WordBase> item) => throw new NotImplementedException();
    public void Add(K key, WordBase value) => _data.Add(key, value);
    public void Add(object key, object? value) => Add((K)key, (WordBase)value!);
    public void Clear() => throw new NotImplementedException();
    public bool Contains(KeyValuePair<K, WordBase> item) => throw new NotImplementedException();
    public void CopyTo(KeyValuePair<K, WordBase>[] array, int arrayIndex) => throw new NotImplementedException();
    public bool Remove(KeyValuePair<K, WordBase> item) => throw new NotImplementedException();
    public void CopyTo(Array array, int index) => throw new NotImplementedException();

    public bool ContainsKey(K key) => _data.ContainsKey(key);
    public bool Remove(K key) => throw new NotImplementedException();
    public bool TryGetValue(K key, [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out WordBase value) => throw new NotImplementedException();

    public WordBase this[K key]
    {
        get => _data[key];
        set { if (_data.ContainsKey(key)) _data[key] = value; else throw new NotImplementedException(); }
    }

    public object? this[object key]
    {
        get => _data.ContainsKey((K)key) ? _data[(K)key] : null;
        set => _data[(K)key] = (WordBase)value!;
    }

    public ICollection<K> Keys => new List<K>(_data.Keys);
    public ICollection<WordBase> Values => _data.Values;
    ICollection IDictionary.Keys => new List<K>(_data.Keys);
    ICollection IDictionary.Values => _data.Values;
    public int Count => _data.Count;
    public bool IsReadOnly => false;
    public bool IsFixedSize => false;
    public bool IsSynchronized => false;
    public object SyncRoot { get; } = new object();
    public bool Contains(object key) => _data.ContainsKey((K)key);
    public void Remove(object key) => _data.Remove((K)key);

    public override string ToString() => "(" + _data.Count + ")";

    public override void AutoSetLanguage()
    {
        foreach (WordBase a in Values)
        {
            if (a != null) a.AutoSetLanguage();
        }
    }

    public override void ReBuild()
    {
        foreach (WordBase wb in Values)
        {
            if (wb != null) wb.ReBuild();
        }
    }

    /// <summary>
    /// 从 XML 节点加载字典数据 / Load dictionary data from XML node
    /// 搬运自细需求 WordDic.LoadXMLWithNode
    /// </summary>
    public override bool LoadXMLWithNode(XmlNode node, SpeedyPack pack)
    {
        foreach (XmlNode a in node.ChildNodes)
        {
            if (a.Name != "WordDic") continue;
            string? b = a.Attributes?["Key"]?.Value;
            string? c = a.Attributes?["IsNull"]?.Value;
            bool d = c != null && bool.Parse(c);

            if (d)
            {
                if (typeof(K) == typeof(string))
                {
                    Add((K)(object)b!, null!);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                string? e = a.Attributes?["Type"]?.Value;
                if (e == null) continue;
                Type? f = FindType(e);
                if (f == null) continue;
                WordBase g = (WordBase)ServiceLocator.Instance.ObjectFactory!.CreateInstance(f, new object?[] { this })!;
                if (!g.LoadXMLWithNode(a, pack))
                {
                    return false;
                }
                Add((K)(object)b!, g);
            }
        }

        return true;
    }

    /// <summary>
    /// 将字典数据保存到 XML 节点 / Save dictionary data to XML node
    /// 搬运自细需求 WordDic.SaveXMLWithNode
    /// </summary>
    public override bool SaveXMLWithNode(XmlNode node, XmlDocument document, SpeedyPack pack)
    {
        foreach (KeyValuePair<K, WordBase> a in this)
        {
            XmlElement b = document.CreateElement("WordDic");
            b.SetAttribute("Key", a.Key?.ToString() ?? "");
            if (a.Value == null)
            {
                b.SetAttribute("IsNull", "true");
                b.SetAttribute("Type", typeof(WordBase).FullName!);
            }
            else
            {
                b.SetAttribute("IsNull", "false");
                b.SetAttribute("Type", a.Value.GetType().FullName!);
                a.Value.SaveXMLWithNode(b, document, pack);
            }
            node.AppendChild(b);
        }

        return true;
    }
}
