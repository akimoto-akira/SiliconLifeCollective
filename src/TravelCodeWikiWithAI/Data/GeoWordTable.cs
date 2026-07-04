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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoWordTable.cs
// 迁移变更：去除 XML 序列化（SaveXMLWithNode/LoadXMLWithNode → IStorage JSON）
//           去除 CoreTools.DataDir 依赖（GetPath() 已移除）
//           保留 Dictionary 核心 + IsZhExits/IsTableExits/GetString/Updata/Delete 方法

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 中-外文对照表 / Chinese-foreign language lookup table
/// 核心：Dictionary&lt;string, Dictionary&lt;string, string&gt;&gt; — 中文词 → 语言代码 → 外文翻译
/// </summary>
public class GeoWordTable : GeoDataBase
{
    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父对象 / Parent object</param>
    public GeoWordTable(GeoDataBase parent) : base(parent)
    {
    }

    /// <summary>
    /// 无参构造函数 / Parameterless Constructor
    /// </summary>
    public GeoWordTable() : base(null) { }

    private readonly Dictionary<string, Dictionary<string, string>> _data = new();

    /// <summary>
    /// 检查中文词是否存在 / Check if a Chinese term exists
    /// </summary>
    /// <param name="zh">中文词 / Chinese term</param>
    /// <returns>是否存在 / Whether it exists</returns>
    public bool IsZhExits(string zh)
    {
        return _data.ContainsKey(zh);
    }

    /// <summary>
    /// 检查对照项是否存在 / Check if a lookup entry exists
    /// </summary>
    /// <param name="zh">中文词 / Chinese term</param>
    /// <param name="code">语言代码 / Language code</param>
    /// <returns>是否存在 / Whether it exists</returns>
    public bool IsTableExits(string zh, string code)
    {
        if (!_data.ContainsKey(zh))
        {
            return false;
        }
        return _data[zh].ContainsKey(code);
    }

    /// <summary>
    /// 获取外文翻译 / Get foreign language translation
    /// </summary>
    /// <param name="zh">中文词 / Chinese term</param>
    /// <param name="code">语言代码 / Language code</param>
    /// <returns>翻译文本，不存在则返回 null / Translated text, or null if not found</returns>
    public string? GetString(string zh, string code)
    {
        if (!_data.ContainsKey(zh))
        {
            return null;
        }

        if (!_data[zh].ContainsKey(code))
        {
            return null;
        }

        return _data[zh][code];
    }

    /// <summary>
    /// 更新或添加对照项 / Update or add a lookup entry
    /// </summary>
    /// <param name="zh">中文词 / Chinese term</param>
    /// <param name="code">语言代码 / Language code</param>
    /// <param name="content">翻译内容 / Translation content</param>
    public void Updata(string zh, string code, string content)
    {
        if (!_data.ContainsKey(zh))
        {
            _data.Add(zh, new Dictionary<string, string>());
        }

        if (_data[zh].ContainsKey(code))
        {
            _data[zh][code] = content;
        }
        else
        {
            _data[zh].Add(code, content);
        }
    }

    /// <summary>
    /// 删除对照项 / Delete a lookup entry
    /// </summary>
    /// <param name="zh">中文词 / Chinese term</param>
    /// <param name="code">语言代码 / Language code</param>
    public void Delete(string zh, string code)
    {
        if (!_data.ContainsKey(zh))
        {
            return;
        }

        if (!_data[zh].ContainsKey(code))
        {
            return;
        }

        _data[zh].Remove(code);
    }
}
