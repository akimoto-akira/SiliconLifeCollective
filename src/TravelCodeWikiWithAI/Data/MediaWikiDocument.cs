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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.MediaWiki\MediaWikiDocument.cs
// 迁移变更：CoreTools.GetBaseLanguage() → SysTool.GetBaseLanguage()
//           CoreTools.GetAllLanguage() → SysTool.GetAllLanguage()
//           保留 BuildDocument 核心逻辑（MediaWiki 文档按语言代码分页输出）

using System.Linq;
using System.Text;
using TravelCodeWikiWithAI.TCWTool;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// MediaWiki 文档类，继承自文档基类 / MediaWiki document class that inherits from document base
/// </summary>
public class MediaWikiDocument : DocumentBase
{
    /// <summary>
    /// 构造函数，初始化 MediaWiki 文档 / Constructor to initialize MediaWiki document
    /// </summary>
    /// <param name="Parent">父级地理数据库对象 / Parent geographic database object</param>
    public MediaWikiDocument(GeoDataBase Parent) : base(Parent)
    {
    }

    public override Dictionary<string, string> BuildDocument(Dictionary<string, byte[]> file)
    {
        CheckParent();
        Dictionary<string, string> f = new Dictionary<string, string>();
        string[] a = SysTool.GetBaseLanguage();
        foreach (string b in a)
        {
            if (b != "*")
            {
                StringBuilder e = new StringBuilder();
                if (Contents != null)
                {
                    foreach (WordBase c in Contents)
                    {
                        if (c is MediaWikiWord d)
                        {
                            e.Append(d.MWBuildWord(b, file));
                        }
                        else if (c is MediaWikiChildWord i)
                        {
                            e.Append(i.MWBuildWord(b, file));
                        }
                    }
                }

                if (!Title.Contains(':'))
                {
                    if (Contents == null || !Contents.Any(l => l is MediaWikiMulityLanguageLink))
                    {
                        MediaWikiMulityLanguageLink m = new MediaWikiMulityLanguageLink(this);
                        e.Append(m.MWBuildWord(b, file));
                    }
                }

                string g = Title + "/" + b;
                f.Add(g, e.ToString());
            }
        }

        // 生成语言索引页 / Generate language index page
        Dictionary<string, string> j = SysTool.GetAllLanguage();
        StringBuilder h = new StringBuilder();
        h.Append("/** Autorun Language Script **/\n\n");
        foreach (string i in a)
        {
            if (i != "*")
            {
                h.Append("* [[:" + Title + "/" + i + "|" + j[i] + "]]\n");
            }
        }

        if (!Title.Contains(":"))
        {
            h.Append("\n");
            foreach (string k in a)
            {
                if (k != "*")
                {
                    h.Append("[[" + k + ":" + Title + "/" + k + "]]\n");
                }
            }
        }

        f.Add(Title, h.ToString());
        return f;
    }
}
