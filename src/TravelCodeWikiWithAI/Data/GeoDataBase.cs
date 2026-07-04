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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoDataBase.cs
// 迁移变更：去除 XMLBase/ICLIOutput 继承，改继承 GeoDataNode
//           去除 [Editor]/[TypeConverter] WinForms 特性
//           去除 using TravelCodeWikiWithAI.Core 等旧命名空间
//           GetPath()/GetObject() 等方法保留为类自身方法（无接口约束）
//           LoadXMLWithNode/SaveXMLWithFile 等 XML 方法已移除
//           AutoSetLanguage() 中对 XMLBase.NewFile()/SaveXML() 的调用已移除

using System.Collections;
using System.Reflection;
using System.Xml;
using SiliconLife.Collective;
using SiliconLife.Speedy;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 地理数据基类，提供数据对象的基础功能 / Geographic data base class that provides basic functionality for data objects
/// 迁移自旧项目 GeoDataBase，去除 XMLBase/ICLIOutput，改继承 GeoDataNode
/// </summary>
public abstract class GeoDataBase : GeoDataNode
{
    /// <summary>
    /// 获取对象路径数组 / Get the path array of the object
    /// </summary>
    /// <param name="deth">深度参数，-1表示无限深度 / Depth parameter, -1 means infinite depth</param>
    /// <returns>路径字符串数组 / Array of path strings</returns>
    public virtual string[] GetPath(int deth)
    {
        PropertyInfo[] pis = GetType().GetProperties();
        List<PropertyInfo> doProps = [];
        List<string> result = [BasePath];
        deth = (deth == -1) ? -1 : deth - 1;
        foreach (PropertyInfo pi in pis)
        {
            if (pi.CanRead && pi.CanWrite)
            {
                MethodInfo[] mias = pi.GetAccessors();
                if (!mias[0].IsStatic)
                {
                    doProps.Add(pi);
                }
            }
        }

        foreach (PropertyInfo a in doProps)
        {
            Type b = a.PropertyType;
            if (b.IsValueType)
            {
                result.Add(BasePath + "." + a.Name);
            }
            else
            {
                object c = a.GetValue(this);
                if (deth == 0)
                {
                    if (string.IsNullOrWhiteSpace(BasePath))
                    {
                        result.Add(a.Name);
                    }
                    else
                    {
                        result.Add(BasePath + "." + a.Name);
                    }
                }
                else
                {
                    if (c == null)
                    {
                        result.Add(BasePath + "." + a.Name);
                    }
                    else if (b == typeof(string))
                    {
                        result.Add(BasePath + "." + a.Name);
                    }
                    else if (b.IsSubclassOf(typeof(GeoDataBase)))
                    {
                        string[] f = ((GeoDataBase)c).GetPath(deth);
                        result.AddRange(f);
                    }
                    else if (b == typeof(LanguageData))
                    {
                        LanguageData h = (LanguageData)c;
                        string[] g = h.GetPath((deth == -1) ? -1 : deth - 1);
                        result.AddRange(g);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        return [.. result];
    }

    /// <summary>
    /// 根据路径获取对象 / Get object by path
    /// </summary>
    /// <param name="path">对象路径 / Object path</param>
    /// <returns>找到的对象 / Found object</returns>
    public virtual object? GetObject(string path)
    {
        if (BasePath == path)
        {
            return this;
        }
        else
        {
            PropertyInfo[] a = GetType().GetProperties();
            foreach (PropertyInfo b in a)
            {
                if (b.CanRead && b.CanWrite)
                {
                    if (!b.PropertyType.IsValueType)
                    {
                        if (b.PropertyType.IsSubclassOf(typeof(GeoDataBase)))
                        {
                            GeoDataBase? c = b.GetValue(this) as GeoDataBase;
                            if (c != this && c != null)
                            {
                                object? d = c.GetObject(path);
                                if (d != null)
                                {
                                    return d;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }

    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="Parent">父对象 / Parent object</param>
    public GeoDataBase(GeoDataBase? Parent)
    {
        _parent = Parent;
    }

    /// <summary>
    /// 无参构造函数 / Parameterless constructor
    /// </summary>
    public GeoDataBase() { }

    /// <summary>
    /// 创建子对象 / Create child object
    /// 返回类型为 object?，因为 LanguageData 不再继承 GeoDataNode
    /// Return type is object? because LanguageData no longer inherits GeoDataNode
    /// </summary>
    public override object? CreateChild(Type t, params object[] args)
    {
        if (t == typeof(LanguageData))
        {
            LanguageData ld = new()
            {
                Parent = args[0]
            };
            return ld;
        }
        else
        {
            ConstructorInfo[] cis = t.GetConstructors();
            int needPath = 0;

            foreach (ConstructorInfo a in cis)
            {
                ParameterInfo[] b = a.GetParameters();
                if (b.Length == 1)
                {
                    ParameterInfo c = b[0];
                    if (c.ParameterType == typeof(GeoDataBase) || c.ParameterType == typeof(GeoDataNode))
                    {
                        needPath = 1; // 需要GeoDataBase参数
                    }
                    else if (c.ParameterType == typeof(GeoLocation))
                    {
                        needPath = 2; // 需要GeoLocation参数
                    }
                }
            }

            GeoDataBase? gdb = null;
            var factory = ServiceLocator.Instance.ObjectFactory;
            if (needPath == 1)
            {
                gdb = (GeoDataBase?)factory?.CreateInstance(t, args[0])!;
            }
            else if (needPath == 2)
            {
                if (args[0] is GeoLocation gl)
                {
                    gdb = (GeoDataBase?)factory?.CreateInstance(t, gl)!;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                if (t.IsSubclassOf(typeof(GeoDataBase)))
                {
                    gdb = (GeoDataBase?)factory?.CreateInstance(t)!;
                }
                else
                {
                    return (GeoDataNode?)factory?.CreateInstance(t)!;
                }
            }

            return gdb;
        }
    }

    /// <summary>
    /// 自动设置语言数据（递归遍历子对象，触发硅基人翻译）
    /// / Automatically set language data (traverse child objects recursively, trigger silicon being translation)
    /// 注意：旧项目中此方法调用 OllamaClient 翻译，新项目中翻译由硅基人通过 ITool 自主完成
    /// / Note: In the legacy project this method called OllamaClient for translation.
    ///   In the new project, translation is done by silicon beings via ITool autonomously.
    /// </summary>
    public virtual void AutoSetLanguage()
    {
        PropertyInfo[] pis = GetType().GetProperties();
        List<PropertyInfo> ps = [];
        foreach (PropertyInfo pi in pis)
        {
            if (pi.CanRead && pi.CanWrite)
            {
                MethodInfo[] mis = pi.GetAccessors();
                if (!mis[0].IsStatic)
                {
                    ps.Add(pi);
                }
            }
        }

        foreach (PropertyInfo a in ps)
        {
            Type b = a.PropertyType;
            if (b.IsSubclassOf(typeof(GeoDataBase)))
            {
                GeoDataBase? c = (GeoDataBase?)a.GetValue(this);
                if (c == null)
                {
                    continue;
                }

                c.AutoSetLanguage();
            }
            else if (b == typeof(LanguageData))
            {
                LanguageData? d = (LanguageData?)a.GetValue(this);
                if (d != null)
                {
                    // 新项目：不再调用 d.AutoSetLanguage()，硅基人自主翻译
                    // In the new project: no longer call d.AutoSetLanguage(), silicon beings translate autonomously
                }
            }
        }
    }

    /// <summary>
    /// 获取对象在父对象中的路径 / Get the path of object in parent object
    /// </summary>
    /// <param name="obj">要查找路径的对象 / Object to find path for</param>
    /// <returns>对象路径字符串 / Object path string</returns>
    public override string GetObjectPath(object obj)
    {
        string a = BasePath ?? "";
        PropertyInfo[] ps = GetType().GetProperties();

        foreach (PropertyInfo b in ps)
        {
            // 仅检查可读写属性 / Only check readable and writable properties
            if (!b.CanWrite || !b.CanRead)
            {
                continue;
            }

            object e = b.GetValue(this);
            if (e == obj)
            {
                if (string.IsNullOrEmpty(a))
                {
                    return b.Name;
                }
                else
                {
                    return a + "." + b.Name;
                }
            }
        }

        return "unPath";
    }

    /// <summary>
    /// 获取需要新建的字符串 / Get string that needs to be created
    /// </summary>
    public virtual string? NeedNewString()
    {
        return null;
    }

    /// <summary>
    /// 处理新建的字符串 / Process newly created string
    /// </summary>
    public virtual bool PostNewString(string str)
    {
        return false;
    }

    /// <summary>
    /// 构建文档字典 / Build document dictionary
    /// </summary>
    public virtual Dictionary<string, string> BuildDocument(Dictionary<string, byte[]> files)
    {
        PropertyInfo[] pis = GetType().GetProperties();
        List<PropertyInfo> ps = [];
        foreach (PropertyInfo pi in pis)
        {
            if (pi.CanRead && pi.CanWrite)
            {
                MethodInfo[] mis = pi.GetAccessors();
                if (!mis[0].IsStatic)
                {
                    ps.Add(pi);
                }
            }
        }

        Dictionary<string, string> a = new Dictionary<string, string>();

        foreach (PropertyInfo pi in ps)
        {
            object c = pi.GetValue(this);
            if (c is GeoDataBase d)
            {
                if (d is not WordBase)
                {
                    Dictionary<string, string> b = d.BuildDocument(files);
                    foreach (KeyValuePair<string, string> e in b)
                    {
                        if (a.ContainsKey(e.Key))
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            a.Add(e.Key, e.Value);
                        }
                    }
                }
            }
        }

        return a;
    }

    public virtual GeoDataBase? GetParent()
    {
        return _parent as GeoDataBase;
    }

    public virtual void RemoveSame()
    {
        PropertyInfo[] pis = GetType().GetProperties();
        List<PropertyInfo> ps = [];
        foreach (PropertyInfo pi in pis)
        {
            if (pi.CanRead && pi.CanWrite)
            {
                MethodInfo[] mis = pi.GetAccessors();
                if (!mis[0].IsStatic)
                {
                    ps.Add(pi);
                }
            }
        }

        foreach (PropertyInfo a in ps)
        {
            Type b = a.PropertyType;
            if (b.IsSubclassOf(typeof(GeoDataBase)))
            {
                GeoDataBase? c = (GeoDataBase?)a.GetValue(this);
                if (c == null)
                {
                    continue;
                }

                c.RemoveSame();
            }
        }
    }

    public virtual GeoDataBase Copy()
    {
        PropertyInfo[] pis = GetType().GetProperties();
        List<PropertyInfo> ps = [];
        foreach (PropertyInfo pi in pis)
        {
            if (pi.CanRead && pi.CanWrite)
            {
                MethodInfo[] mis = pi.GetAccessors();
                if (!mis[0].IsStatic)
                {
                    ps.Add(pi);
                }
            }
        }

        GeoDataBase d = (ServiceLocator.Instance.ObjectFactory?.CreateInstance(GetType(), new object?[] { null }) as GeoDataBase)!;
        foreach (PropertyInfo a in ps)
        {
            Type b = a.PropertyType;
            if (b.IsSubclassOf(typeof(GeoDataBase)))
            {
                GeoDataBase? c = (GeoDataBase?)a.GetValue(this);
                if (c == null)
                {
                    continue;
                }

                GeoDataBase e = c.Copy();
                a.SetValue(d, e);
            }
            else if (b == typeof(LanguageData))
            {
                LanguageData? f = a.GetValue(this) as LanguageData;
                if (f != null)
                {
                    LanguageData? g = f.Copy();
                    a.SetValue(d, g);
                }
            }
            else if (b == typeof(string))
            {
                string? h = a.GetValue(this) as string;
                a.SetValue(d, h);
            }
            else
            {
                if (b.IsValueType)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        return d;
    }

    public virtual void CheckParent()
    {
        PropertyInfo[] allProps = GetType().GetProperties();
        List<PropertyInfo> doProps = new List<PropertyInfo>();
        foreach (PropertyInfo prop in allProps)
        {
            if (prop.CanRead && prop.CanWrite)
            {
                MethodInfo[] mis = prop.GetAccessors();
                if (!mis[0].IsStatic)
                {
                    doProps.Add(prop);
                }
            }
        }

        foreach (PropertyInfo b in doProps)
        {
            Type c = b.PropertyType;

            if (!c.IsValueType)
            {
                if (c.IsSubclassOf(typeof(GeoDataBase)))
                {
                    GeoDataBase? d = b.GetValue(this) as GeoDataBase;
                    if (d == null)
                    {
                        continue;
                    }

                    d._parent = this;
                    d.CheckParent();
                }
                else if (c == typeof(LanguageData))
                {
                    LanguageData? e = b.GetValue(this) as LanguageData;
                    if (e == null)
                    {
                        continue;
                    }

                    e.Parent = this;
                }
            }
        }
    }

    // ========== SpeedyPack 持久化方法（搬运自 XMLBase，文件 I/O 替换为 SpeedyPack） ==========

    /// <summary>
    /// 获取对象的存储键 / Get the storage key of the object
    /// 替代旧项目的 GetPath() + NewFile() / Replaces legacy GetPath() + NewFile()
    /// 返回 null 表示内嵌于父对象（对应 NewFile()=false）
    /// 返回字符串表示独立存储（对应 NewFile()=true），字符串即存储路径
    /// </summary>
    public virtual string? GetStorageKey() => throw new NotImplementedException();

    /// <summary>
    /// 从 SpeedyPack 加载对象（入口）/ Load object from SpeedyPack (entry point)
    /// 替代旧项目的 LoadXML() / Replaces legacy LoadXML()
    /// </summary>
    public static T? LoadFromPack<T>(SpeedyPack pack, string key) where T : GeoDataBase
    {
        byte[]? data = pack.Read(key);
        if (data == null) return null;

        XmlDocument doc = new XmlDocument();
        doc.Load(new MemoryStream(data));
        XmlElement root = doc.DocumentElement;
        if (root == null) return null;

        string typeName = root.Name;
        Type? t = FindType(typeName);
        if (t == null) return null;

        GeoDataBase obj = (GeoDataBase)ServiceLocator.Instance.ObjectFactory!.CreateInstance(t)!;
        if (!obj.LoadXMLWithNode(root, pack))
        {
            return null;
        }

        obj.CheckParent();
        return (T)obj;
    }

    /// <summary>
    /// 保存对象到 SpeedyPack（入口）/ Save object to SpeedyPack (entry point)
    /// 替代旧项目的 SaveXML() / Replaces legacy SaveXML()
    /// </summary>
    public void SaveToPack(SpeedyPack pack)
    {
        string? key = GetStorageKey();
        if (key == null) return;

        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement(GetType().FullName!);
        if (!SaveXMLWithNode(root, doc, pack))
        {
            return;
        }

        doc.AppendChild(root);
        MemoryStream ms = new MemoryStream();
        doc.Save(ms);
        pack.Write(key, ms.ToArray());
    }

    /// <summary>
    /// 从 XML 节点加载数据 / Load data from XML node
    /// 搬运自 XMLBase.LoadXMLWithNode，FilePath 改为从 SpeedyPack 加载
    /// </summary>
    public virtual bool LoadXMLWithNode(XmlNode node, SpeedyPack pack)
    {
        XmlElement element = node as XmlElement;
        if (element == null) return false;

        PropertyInfo[] allProps = GetType().GetProperties();
        List<PropertyInfo> doProps = new List<PropertyInfo>();
        foreach (PropertyInfo prop in allProps)
        {
            if (prop.CanRead && prop.CanWrite)
            {
                MethodInfo[] mias = prop.GetAccessors();
                if (mias[0].IsStatic) continue;
                doProps.Add(prop);
            }
        }

        Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();
        foreach (PropertyInfo prop in doProps)
        {
            dic[prop.Name] = prop;
        }

        foreach (XmlNode xn in element.ChildNodes)
        {
            string a = xn.Name;
            if (!dic.ContainsKey(a)) continue;

            Type b = dic[a].PropertyType;

            if (b.IsValueType)
            {
                if (b.IsEnum)
                {
                    XmlText? k = xn.ChildNodes.Count > 0 ? xn.ChildNodes[0] as XmlText : null;
                    if (k != null)
                    {
                        object l = Enum.Parse(b, k.Value!);
                        dic[a].SetValue(this, l);
                    }
                }
                else if (b == typeof(bool))
                {
                    XmlText? p = xn.ChildNodes.Count > 0 ? xn.ChildNodes[0] as XmlText : null;
                    if (p != null)
                    {
                        bool q = bool.Parse(p.Value!);
                        dic[a].SetValue(this, q);
                    }
                }
                else if (b == typeof(long))
                {
                    XmlText? m = xn.ChildNodes.Count > 0 ? xn.ChildNodes[0] as XmlText : null;
                    if (m != null)
                    {
                        long n = long.Parse(m.Value!);
                        dic[a].SetValue(this, n);
                    }
                }
                else
                {
                    MethodInfo[] r = b.GetMethods();
                    MethodInfo[] u = r.Where(v => v.Name == "Parse" && v.GetParameters().Length == 1).ToArray();
                    if (u.Length == 1)
                    {
                        XmlText? t = xn.ChildNodes.Count > 0 ? xn.ChildNodes[0] as XmlText : null;
                        if (t != null)
                        {
                            object s = u[0].Invoke(null, new object[] { t.Value! })!;
                            dic[a].SetValue(this, s);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException($"LoadXMLWithNode: 不支持的值类型 {b.FullName}");
                    }
                }
            }
            else
            {
                if (b == typeof(Dictionary<string, string>))
                {
                    Dictionary<string, string>? k = dic[xn.Name].GetValue(this) as Dictionary<string, string>;
                    if (k == null)
                    {
                        k = new Dictionary<string, string>();
                        dic[xn.Name].SetValue(this, k);
                    }

                    string? l = xn.Attributes?["Key"]?.Value;
                    string? m = xn.Attributes?["Value"]?.Value;
                    if (l != null && m != null)
                    {
                        k.Add(l, m);
                    }
                }
                else
                {
                    bool e = false;
                    try
                    {
                        string? d = xn.Attributes?["IsNull"]?.Value;
                        if (d != null) e = bool.Parse(d);
                    }
                    catch { }

                    if (b == typeof(string))
                    {
                        if (e) continue;
                        XmlText? st = xn.ChildNodes.Count > 0 ? xn.ChildNodes[0] as XmlText : null;
                        if (st != null)
                        {
                            dic[a].SetValue(this, st.Value);
                        }
                    }
                    else if (b.IsSubclassOf(typeof(GeoDataBase)))
                    {
                        if (e) continue;

                        string? c = xn.Attributes?["Type"]?.Value;
                        if (c == null) continue;
                        Type? k = FindType(c);
                        if (k == null) continue;

                        GeoDataBase f = (GeoDataBase)CreateChild(k, this);
                        XmlAttribute? h = xn.Attributes?["StorageRef"];
                        if (h == null)
                        {
                            if (!f.LoadXMLWithNode(xn, pack))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            string refKey = h.Value;
                            byte[]? refData = pack.Read(refKey);
                            if (refData != null)
                            {
                                XmlDocument refDoc = new XmlDocument();
                                refDoc.Load(new MemoryStream(refData));
                                XmlElement refRoot = refDoc.DocumentElement;
                                if (refRoot != null)
                                {
                                    if (!f.LoadXMLWithNode(refRoot, pack))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }

                        dic[a].SetValue(this, f);
                    }
                    else if (b == typeof(LanguageData))
                    {
                        if (e) continue;
                        LanguageData? ld = new LanguageData();
                        ld.Parent = this;
                        XmlAttribute? h = xn.Attributes?["StorageRef"];
                        if (h == null)
                        {
                            ld.LoadXMLWithNode(xn);
                        }
                        else
                        {
                            string refKey = h.Value;
                            byte[]? refData = pack.Read(refKey);
                            if (refData != null)
                            {
                                XmlDocument refDoc = new XmlDocument();
                                refDoc.Load(new MemoryStream(refData));
                                XmlElement refRoot = refDoc.DocumentElement;
                                if (refRoot != null)
                                {
                                    ld.LoadXMLWithNode(refRoot);
                                }
                            }
                        }

                        dic[a].SetValue(this, ld);
                    }
                    else if (b.IsGenericType)
                    {
                        Type g = b.GetGenericTypeDefinition();
                        if (g == typeof(List<>))
                        {
                            Type h = b.GenericTypeArguments[0];
                            PropertyInfo[] i = h.GetProperties();
                            Dictionary<string, PropertyInfo> j = new Dictionary<string, PropertyInfo>();
                            foreach (PropertyInfo k in i)
                            {
                                j[k.Name] = k;
                            }

                            object? m = dic[a].GetValue(this);
                            if (m == null) throw new NotImplementedException();

                            IList q = (IList)m;
                            object n = ServiceLocator.Instance.ObjectFactory!.CreateInstance(h)!;
                            foreach (XmlAttribute l in xn.Attributes?.Cast<XmlAttribute>() ?? Array.Empty<XmlAttribute>())
                            {
                                if (j.ContainsKey(l.Name))
                                {
                                    Type o = j[l.Name].PropertyType;
                                    if (o == typeof(long))
                                    {
                                        j[l.Name].SetValue(n, long.Parse(l.Value));
                                    }
                                    else if (o.IsEnum)
                                    {
                                        object p = Enum.Parse(o, l.Value);
                                        j[l.Name].SetValue(n, p);
                                    }
                                    else if (o == typeof(string))
                                    {
                                        j[l.Name].SetValue(n, l.Value);
                                    }
                                    else
                                    {
                                        throw new NotImplementedException();
                                    }
                                }
                            }

                            q.Add(n);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 将数据保存到 XML 节点 / Save data to XML node
    /// 搬运自 XMLBase.SaveXMLWithNode，FilePath 改为 StorageRef（存储键）
    /// </summary>
    public virtual bool SaveXMLWithNode(XmlNode node, XmlDocument document, SpeedyPack pack)
    {
        XmlElement element = node as XmlElement;
        if (element == null) return false;

        PropertyInfo[] allProps = GetType().GetProperties();
        List<PropertyInfo> doProps = new List<PropertyInfo>();
        foreach (PropertyInfo prop in allProps)
        {
            if (prop.CanRead && prop.CanWrite)
            {
                MethodInfo[] mis = prop.GetAccessors();
                if (mis[0].IsStatic) continue;
                doProps.Add(prop);
            }
        }

        foreach (PropertyInfo b in doProps)
        {
            Type c = b.PropertyType;

            if (c.IsValueType)
            {
                if (c.IsEnum)
                {
                    XmlElement f = document.CreateElement(b.Name);
                    f.SetAttribute("Type", c.FullName!);
                    Enum? g = b.GetValue(this) as Enum;
                    if (g != null)
                    {
                        XmlText i = document.CreateTextNode(g.ToString());
                        f.AppendChild(i);
                    }
                    element.AppendChild(f);
                }
                else if (b == typeof(bool))
                {
                    XmlElement f = document.CreateElement(b.Name);
                    f.SetAttribute("Type", c.FullName!);
                    object? val = b.GetValue(this);
                    if (val != null)
                    {
                        XmlText i = document.CreateTextNode(val.ToString()!);
                        f.AppendChild(i);
                    }
                    element.AppendChild(f);
                }
                else if (b == typeof(long))
                {
                    XmlElement f = document.CreateElement(b.Name);
                    f.SetAttribute("Type", c.FullName!);
                    object? val = b.GetValue(this);
                    if (val != null)
                    {
                        XmlText i = document.CreateTextNode(val.ToString()!);
                        f.AppendChild(i);
                    }
                    element.AppendChild(f);
                }
                else
                {
                    XmlElement d = document.CreateElement(b.Name);
                    d.SetAttribute("Type", c.FullName!);
                    object? value = b.GetValue(this);
                    if (value != null)
                    {
                        XmlText e = document.CreateTextNode(value.ToString()!);
                        d.AppendChild(e);
                    }
                    element.AppendChild(d);
                }
            }
            else
            {
                if (c == typeof(string))
                {
                    XmlElement se = document.CreateElement(b.Name);
                    se.SetAttribute("Type", c.FullName!);
                    string? stringValue = b.GetValue(this) as string;
                    if (stringValue == null)
                    {
                        se.SetAttribute("IsNull", "true");
                    }
                    else
                    {
                        se.SetAttribute("IsNull", "false");
                        XmlText st = document.CreateTextNode(stringValue);
                        se.AppendChild(st);
                    }
                    element.AppendChild(se);
                }
                else if (c.IsSubclassOf(typeof(GeoDataBase)))
                {
                    XmlElement xmlbasenode = document.CreateElement(b.Name);
                    GeoDataBase? xmlobj = b.GetValue(this) as GeoDataBase;
                    string? j = xmlobj?.GetType().FullName ?? c.FullName;
                    xmlbasenode.SetAttribute("Type", j!);

                    if (xmlobj == null)
                    {
                        xmlbasenode.SetAttribute("IsNull", "true");
                    }
                    else
                    {
                        xmlbasenode.SetAttribute("IsNull", "false");
                        string? childKey = xmlobj.GetStorageKey();
                        if (childKey == null)
                        {
                            if (!xmlobj.SaveXMLWithNode(xmlbasenode, document, pack))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            xmlbasenode.SetAttribute("StorageRef", childKey);
                            xmlobj.SaveToPack(pack);
                        }
                    }

                    element.AppendChild(xmlbasenode);
                }
                else if (c == typeof(LanguageData))
                {
                    XmlElement xmlbasenode = document.CreateElement(b.Name);
                    LanguageData? ld = b.GetValue(this) as LanguageData;

                    if (ld == null)
                    {
                        xmlbasenode.SetAttribute("Type", typeof(LanguageData).FullName!);
                        xmlbasenode.SetAttribute("IsNull", "true");
                    }
                    else
                    {
                        xmlbasenode.SetAttribute("Type", typeof(LanguageData).FullName!);
                        xmlbasenode.SetAttribute("IsNull", "false");
                        ld.SaveXMLWithNode(xmlbasenode, document);
                    }

                    element.AppendChild(xmlbasenode);
                }
                else
                {
                    object? h = b.GetValue(this);
                    if (h is IDictionary i)
                    {
                        foreach (DictionaryEntry j in i)
                        {
                            XmlElement k = document.CreateElement(b.Name);
                            k.SetAttribute("Key", j.Key?.ToString() ?? "");
                            k.SetAttribute("Value", j.Value?.ToString() ?? "");
                            element.AppendChild(k);
                        }
                    }
                    else if (h == null)
                    {
                    }
                    else if (h is IList j)
                    {
                        foreach (object k in j)
                        {
                            XmlElement n = document.CreateElement("Refs");
                            if (k is GeoDataBase o)
                            {
                                o.SaveXMLWithNode(n, document, pack);
                            }
                            element.AppendChild(n);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 根据类型名称查找类型 / Find type by type name
    /// 搬运自 XMLBase.FindType
    /// </summary>
    protected static Type? FindType(string name, bool force = false)
    {
        var registry = ServiceLocator.Instance.TypeRegistry;
        if (registry != null)
        {
            if (!force)
            {
                int c = name.IndexOf("`");
                if (c != -1)
                {
                    int d = name.IndexOf('[');
                    if (d != -1)
                    {
                        return registry.FindType(name);
                    }
                }
            }
            return registry.FindType(name);
        }
        return null;
    }
}
