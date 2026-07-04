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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoSpecialAdministrativeRegion.cs
// 迁移变更：去除 OSMapi 依赖（FlushOsmSub 改为空壳）
//           去除 MediaWikiTool 依赖（GetWikiDocuments() 改为自行实现）
//           去除 Console.WriteLine（改为注释说明，AutoSetLanguage 保留语义）
//           保留全部数据属性（sub 列表）

using System.ComponentModel;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 地理特别行政区类，表示特别行政区级别的地理位置 / Geographic Special Administrative Region class representing SAR-level geographic locations
/// </summary>
public class GeoSpecialAdministrativeRegion : GeoLocation
{
    /// <summary>
    /// 地理位置标识符 / Geographic location identifier
    /// </summary>
    public override string ID { get; set; } = string.Empty;

    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父地理位置（通常为国家）/ Parent geographic location (usually country)</param>
    public GeoSpecialAdministrativeRegion(GeoLocation parent) : base(parent)
    {
    }

    /// <summary>
    /// 基础路径，返回父级的完整标识符 / Base path, returns parent's full identifier
    /// </summary>
    public override string BasePath
    {
        get
        {
            if (Parent == null)
            {
                return "world";
            }
            return Parent.FullID;
        }
    }

    /// <summary>
    /// 县市列表 / County/city list
    /// </summary>
    [Description("县市")]
    public GeoList<GeoLocation>? sub { get; set; }

    /// <summary>
    /// 字符串表示，优先显示名称 / String representation, prioritize displaying name
    /// </summary>
    public override string ToString()
    {
        if (Name == null || Name.Count == 0)
        {
            return "(空)";
        }
        return Name.ToString();
    }

    public override IGeoList GetSubArea()
    {
        return sub ?? new GeoList<GeoLocation>(this);
    }

    public override IGeoList GetAttractions()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 获取维基文档列表 / Get wiki documents list
    /// 在基类文档后追加子区域列表段
    /// </summary>
    public override DocumentBase[] GetWikiDocuments(Dictionary<string, byte[]> file)
    {
        DocumentBase[] baseDocs = base.GetWikiDocuments(file);
        if (baseDocs.Length == 0) return baseDocs;

        MediaWikiDocument doc = (MediaWikiDocument)baseDocs[0];

        // 添加子区域列表段
        if (sub != null && sub.Count > 0)
        {
            LanguageData subTitle = new LanguageData();
            subTitle.SetZhHans("子区域");

            MediaWikiSection subSection = new MediaWikiSection(doc)
            {
                Title = new MediaWikiLanguage(doc) { LanguageData = subTitle },
                Content = new MediaWikiChildWord(doc)
            };

            MediaWikiNoSortList subList = new MediaWikiNoSortList(subSection);
            foreach (GeoLocation child in sub)
            {
                subList.Add(CreateGeoLink(subList, child));
            }
            subSection.Content.Add(subList);
            doc.Contents.Add(subSection);
        }

        return baseDocs;
    }

    /// <summary>
    /// 自动设置语言的特别行政区实现 / SAR implementation of auto-setting language
    /// 特别行政区通常有多种官方语言（如香港：中/英；澳门：中/葡）
    /// SARs usually have multiple official languages (e.g., HK: zh/en; MO: zh/pt)
    /// </summary>
    public override void AutoSetLanguage()
    {
        // 新项目：由硅基人通过 ITool 设置语言，不再自动处理
        // In the new project: languages are set by silicon beings via ITool, no longer automatic
        base.AutoSetLanguage();
    }
}
