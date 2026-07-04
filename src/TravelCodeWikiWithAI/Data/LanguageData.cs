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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\LanguageData.cs
// 迁移变更：去除 XMLBase 继承，不再继承 IDictionary（改为组合模式）
//           去除 ICLIOutput、ICustomTypeDescriptor 接口
//           去除 [TypeConverter]/[Editor] WinForms 特性
//           去除 OllamaClient/AutoSetLanguage()（硅基人自主翻译）
//           去除 CLDRCtrl/CoreTools 依赖，语言验证改用 SysTool
//           去除 LanguagePropertyDescriptor/LanguageBoolPropertyDescriptor（WinForms PropertyGrid 专用）
//           保留数据存储能力和 OSM 标签解析功能

using System.Collections;
using System.Xml;
using TravelCodeWikiWithAI.TCWTool;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 多语言数据类，存储地理实体的多语言名称 / Multilingual data class that stores multilingual names of geographic entities
/// 迁移自旧项目 LanguageData，去除 XMLBase/ICLIOutput/ICustomTypeDescriptor/Ollama
/// </summary>
public class LanguageData
{
    private Dictionary<string, string> _data = new Dictionary<string, string>();

    /// <summary>
    /// 父对象引用 / Parent object reference
    /// </summary>
    public object? Parent { get; set; }

    /// <summary>
    /// 是否包含常规语言代码（非通配符 "*"）/ Whether this instance contains normal (non-wildcard) language codes
    /// 当为 true 时表示至少包含一种具体语言代码（如 zh-cn、en），而非仅靠通配符 "*"
    /// When true, indicates at least one concrete language code (e.g., zh-cn, en) exists, not just wildcard "*"
    /// </summary>
    public bool HasNormalCode => _data.Keys.Any(k => k != "*");

    /// <summary>
    /// 是否已包含全部语言代码的翻译 / Whether this instance contains translations for ALL language codes from SysTool.GetAllLanguage()
    /// 当为 true 时，告知其他程序本实例无需补充翻译；为 false 时表示仍有语言缺失
    /// When true, signals to other programs that no further translation is needed; false means some languages are still missing
    /// </summary>
    public bool HasAllCode
    {
        get
        {
            if (_data.Count == 0) return false;
            Dictionary<string, string> all = SysTool.GetAllLanguage();
            foreach (string code in all.Keys)
            {
                if (!_data.ContainsKey(code)) return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 是否不自动翻译 / Whether to skip auto translation
    /// </summary>
    public bool NoAutoSet { get; set; }

    /// <summary>
    /// 获取/设置指定语言代码的文本 / Get/set text for specified language code
    /// </summary>
    public string? this[string key]
    {
        get
        {
            if (_data.Count == 0)
            {
                return null;
            }

            // 直接查找 / Direct lookup
            if (_data.ContainsKey(key))
            {
                return _data[key];
            }

            // 回退逻辑：先查找通配符，再查找第一个可用值
            // Fallback logic: check wildcard first, then first available value
            if (_data.Count == 1)
            {
                if (_data.ContainsKey("*"))
                {
                    return _data["*"];
                }
                else
                {
                    string[] b = _data.Keys.ToArray();
                    return _data[b[0]];
                }
            }

            // 如果 key 不存在，尝试通配符回退 / If key not found, try wildcard fallback
            if (_data.ContainsKey("*"))
            {
                return _data["*"];
            }
            else if (_data.ContainsKey("zh-cn"))
            {
                return _data["zh-cn"];
            }
            else if (_data.ContainsKey("en"))
            {
                return _data["en"];
            }
            else if (key != "*")
            {
                return this["*"];
            }

            return null;
        }
        set
        {
            if (value == null) return;

            if (_data.ContainsKey(key))
            {
                _data[key] = value;
            }
            else
            {
                _data.Add(key, value);
            }
        }
    }

    /// <summary>
    /// 语言条目数量 / Number of language entries
    /// </summary>
    public int Count => _data.Count;

    /// <summary>
    /// 所有语言代码 / All language codes
    /// </summary>
    public ICollection<string> Keys => _data.Keys;

    /// <summary>
    /// 所有语言值 / All language values
    /// </summary>
    public ICollection<string> Values => new List<string>(_data.Values);

    /// <summary>
    /// 是否包含指定语言代码 / Whether contains specified language code
    /// </summary>
    public bool ContainsKey(string key)
    {
        return _data.ContainsKey(key);
    }

    /// <summary>
    /// 添加语言条目 / Add language entry
    /// </summary>
    public void Add(string key, string value)
    {
        this[key] = value;
    }

    /// <summary>
    /// 添加键值对 / Add key-value pair
    /// </summary>
    public void Add(KeyValuePair<string, string> item)
    {
        Add(item.Key, item.Value);
    }

    /// <summary>
    /// 设置指定语言的文本 / Set text for specified language
    /// </summary>
    public bool SetLanguage(string code, string content)
    {
        Add(code, content);
        return true;
    }

    /// <summary>
    /// 设置简体中文文本 / Set Simplified Chinese text
    /// </summary>
    public bool SetZhHans(string content)
    {
        SetLanguage("zh", content);
        SetLanguage("zh-cn", content);
        SetLanguage("zh-hans", content);
        return true;
    }

    /// <summary>
    /// 设置所有语言的文本 / Set text for all languages
    /// </summary>
    public bool SetAllLanguage(string content)
    {
        Dictionary<string, string> a = SysTool.GetAllLanguage();
        foreach (string b in a.Keys)
        {
            if (!SetLanguage(b, content))
            {
                throw new NotImplementedException();
            }
        }

        return true;
    }

    /// <summary>
    /// 获取路径数组（用于属性遍历）/ Get path array (for property traversal)
    /// </summary>
    public string[] GetPath(int deth)
    {
        List<string> result = new List<string>();
        string b = "";
        if (Parent is GeoDataBase c)
        {
            b = c.GetObjectPath(this);
        }

        foreach (string a in _data.Keys)
        {
            result.Add(b + "[" + a + "]");
        }

        return result.ToArray();
    }

    /// <summary>
    /// 从 OSM 标签创建 LanguageData / Create LanguageData from OSM tags
    /// </summary>
    public static LanguageData? CreateWithTags(Dictionary<string, string> tags, bool fullName = false)
    {
        if (fullName)
        {
            if (TestTagsPre(tags, "official_name"))
            {
                Dictionary<string, string> b = GetTagsWithLanguage(tags, "official_name");
                return CombinTag(b);
            }
        }
        else
        {
            if (TestTagsPre(tags, "name"))
            {
                Dictionary<string, string> a = GetTagsWithLanguage(tags, "name");
                return CombinTag(a);
            }
        }

        return null;
    }

    private static bool TestTagsPre(Dictionary<string, string> tag, string pre)
    {
        foreach (string a in tag.Keys)
        {
            if (a == pre)
            {
                return true;
            }

            if (a.StartsWith(pre + ":"))
            {
                return true;
            }
        }

        return false;
    }

    private static Dictionary<string, string> GetTagsWithLanguage(Dictionary<string, string> tag, string pre)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        foreach (KeyValuePair<string, string> a in tag)
        {
            if (a.Key == pre)
            {
                result.Add("*", a.Value);
            }
            else if (a.Key.StartsWith(pre + ":"))
            {
                string b = a.Key.Substring(pre.Length + 1);
                result.Add(b, a.Value);
            }
        }

        return result;
    }

    private static LanguageData CombinTag(Dictionary<string, string> tag)
    {
        Dictionary<string, string> a = SysTool.GetAllLanguage();
        LanguageData result = new LanguageData();
        foreach (KeyValuePair<string, string> b in tag)
        {
            if (b.Key == "*")
            {
                result.Add(b.Key, b.Value);
            }
            else
            {
                if (a.ContainsKey(b.Key))
                {
                    result.Add(b);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 复制当前 LanguageData / Copy current LanguageData
    /// </summary>
    public LanguageData? Copy()
    {
        LanguageData a = new LanguageData();
        if (_data.ContainsKey("zh-cn"))
        {
            a._data.Add("zh-cn", _data["zh-cn"]);
        }
        else
        {
            if (_data.ContainsKey("*"))
            {
                a._data.Add("*", _data["*"]);
            }
        }

        if (a.Count == 0)
        {
            return null;
        }

        return a;
    }

    /// <summary>
    /// 获取枚举器 / Get enumerator
    /// </summary>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return new Dictionary<string, string>(_data).GetEnumerator();
    }

    /// <summary>
    /// 隐式转换：从 IDictionary 转换为 LanguageData / Implicit conversion from IDictionary to LanguageData
    /// </summary>
    public static implicit operator LanguageData?(Dictionary<string, string> dictionary)
    {
        if (dictionary == null)
        {
            return null;
        }

        LanguageData result = new LanguageData();
        Dictionary<string, string> allLanguage = SysTool.GetAllLanguage();

        foreach (KeyValuePair<string, string> kvp in dictionary)
        {
            if (!allLanguage.ContainsKey(kvp.Key) && kvp.Key != "*")
            {
                continue; // 跳过无效语言代码 / Skip invalid language codes
            }

            result._data.Add(kvp.Key, kvp.Value);
        }

        return result;
    }

    public override string ToString()
    {
        if (_data.Count == 0)
        {
            return "(空)";
        }

        if (ContainsKey("zh-cn"))
        {
            return this["zh-cn"] ?? "";
        }

        if (ContainsKey("*"))
        {
            return this["*"] ?? "";
        }

        string k = _data.Keys.ToArray()[0];
        return this[k] ?? "";
    }

    /// <summary>
    /// 从 XML 节点加载语言数据 / Load language data from XML node
    /// 搬运自细需求 LanguageData.LoadXMLWithNode
    /// </summary>
    public bool LoadXMLWithNode(XmlNode node)
    {
        foreach (XmlNode a in node.ChildNodes)
        {
            if (a is not XmlElement b) continue;
            if (b.Name == "Language")
            {
                string c = b.GetAttribute("Key");
                XmlText? d = b.ChildNodes.Count > 0 ? b.ChildNodes[0] as XmlText : null;
                if (d != null)
                {
                    _data.Add(c, d.Value!);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        if (node.Attributes != null)
        {
            List<string> n = new List<string>();
            foreach (XmlAttribute o in node.Attributes!.Cast<XmlAttribute>())
            {
                n.Add(o.Name);
            }

            if (n.Contains("NoAutoSet"))
            {
                string m = node.Attributes!["NoAutoSet"]!.Value;
                bool p = bool.Parse(m);
                NoAutoSet = p;
            }
        }

        return true;
    }

    /// <summary>
    /// 将语言数据保存到 XML 节点 / Save language data to XML node
    /// 搬运自细需求 LanguageData.SaveXMLWithNode
    /// </summary>
    public bool SaveXMLWithNode(XmlNode node, XmlDocument document)
    {
        foreach (KeyValuePair<string, string> d in _data)
        {
            XmlElement a = document.CreateElement("Language");
            a.SetAttribute("Key", d.Key);
            XmlText b = document.CreateTextNode(d.Value);
            a.AppendChild(b);
            node.AppendChild(a);
        }

        XmlElement c = node as XmlElement;
        if (c != null)
        {
            c.SetAttribute("HasNormalCode", HasNormalCode.ToString());
            c.SetAttribute("HasAllCode", HasAllCode.ToString());
            c.SetAttribute("NoAutoSet", NoAutoSet.ToString());
        }

        return true;
    }
}
