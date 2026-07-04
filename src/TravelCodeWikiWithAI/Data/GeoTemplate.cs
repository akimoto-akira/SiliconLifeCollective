// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoTemplate.cs
// 迁移变更：原样迁移，零改动 — 纯静态字符串，无特殊依赖

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 地理模板类，用于生成 MediaWiki 模板调用代码 / Geographic template class for generating MediaWiki template invocation code
/// </summary>
public static class GeoTemplate
{
    /// <summary>
    /// 获取车牌模板 / Get license plate template
    /// </summary>
    public static (string title, string content) LicensePlate
    {
        get
        {
            string title = "Template:LicensePlate";
            string content = @"<span class=""license-plate license-plate-{{{type|small}}} {{{subtype|}}}"">
  {{#if:{{{structure|}}}|
    <span class=""top-section"">
      <span class=""license-plate-region"">{{{region|}}}</span>
      {{{toptext|}}}
    </span>
    <span class=""bottom-section"">{{{format|XXXXX}}}</span>
  |
    <span class=""license-plate-region"">{{{region|}}}</span>
    <span class=""license-plate-format"">{{{format|XXXXX}}}</span>
  }}
</span><noinclude>
== 使用说明 ==
此模板用于显示中国车牌样式。

=== 参数 ===
* '''region''': 地区代码（如：京A、粤B、川A）
* '''format''': 车牌号码（如：12345、D12345）
* '''type''': 车牌类型，可选值：
** '''small''' - 小型汽车号牌（蓝底白字，默认）
** '''new-energy-small''' - 小型新能源汽车号牌（白绿渐变）
** '''new-energy-large''' - 大型新能源汽车号牌（黄绿渐变）
** '''large''' - 大型汽车号牌（黄底黑字）
** '''trailer''' - 挂车号牌（黄底黑字，带挂字）
** '''embassy''' - 使馆号牌（黑底白字，带使字）
** '''consulate''' - 领馆号牌（黑底白字，带领字）
** '''hk-macau''' - 港澳出入境车牌（黑底白字）
** '''training''' - 教练车车牌（黄底黑字，带学字）
** '''police''' - 警车号牌（白底黑字，红色警字）
** '''emergency''' - 应急救援车辆号牌（白底黑字，红色应急字）
** '''armed-police''' - 武警车牌（白底双层结构）
** '''military-small''' - 小型军车车牌（白底单行）
** '''military-large''' - 大型军车车牌（白底双层）
* '''subtype''': 子类型（可选）
** 对于港澳车牌：'''hk''' 或 '''macau'''
* '''structure''': 是否为双层结构（武警、大型军车使用）
* '''toptext''': 上半部分文字（双层结构使用，如：WJ 警徽 京）

=== 示例 ===

==== 小型汽车号牌（蓝牌） ====
<pre>
{{{{LicensePlate|region=京A|format=12345|type=small}}}}
</pre>
{{{{LicensePlate|region=京A|format=12345|type=small}}}}

==== 小型新能源汽车号牌 ====
<pre>
{{{{LicensePlate|region=京A|format=D12345|type=new-energy-small}}}}
</pre>
{{{{LicensePlate|region=京A|format=D12345|type=new-energy-small}}}}

==== 大型新能源汽车号牌 ====
<pre>
{{{{LicensePlate|region=粤B|format=D12345|type=new-energy-large}}}}
</pre>
{{{{LicensePlate|region=粤B|format=D12345|type=new-energy-large}}}}

==== 大型汽车号牌（黄牌） ====
<pre>
{{{{LicensePlate|region=川A|format=66666|type=large}}}}
</pre>
{{{{LicensePlate|region=川A|format=66666|type=large}}}}

==== 挂车号牌 ====
<pre>
{{{{LicensePlate|region=京A|format=1234|type=trailer}}}}
</pre>

==== 使馆号牌 ====
<pre>
{{{{LicensePlate|region=123|format=456|type=embassy}}}}
</pre>

==== 领馆号牌 ====
<pre>
{{{{LicensePlate|region=京123|format=45|type=consulate}}}}
</pre>

==== 港澳出入境车牌 ====
<pre>
{{{{LicensePlate|region=粤Z|format=1234|type=hk-macau|subtype=hk}}}}
{{{{LicensePlate|region=粤Z|format=5678|type=hk-macau|subtype=macau}}}}
</pre>

==== 教练车车牌 ====
<pre>
{{{{LicensePlate|region=京A|format=1234|type=training}}}}
</pre>

==== 警车号牌 ====
<pre>
{{{{LicensePlate|region=京A|format=1234|type=police}}}}
</pre>

==== 应急救援车辆号牌 ====
<pre>
{{{{LicensePlate|region=京|format=S1234|type=emergency}}}}
</pre>

==== 武警车牌 ====
<pre>
{{{{LicensePlate|region=|toptext=WJ 警徽 京|format=12345|type=armed-police|structure=1}}}}
</pre>

==== 小型军车车牌 ====
<pre>
{{{{LicensePlate|region=AB|format=12345|type=military-small}}}}
</pre>

==== 大型军车车牌 ====
<pre>
{{{{LicensePlate|region=|toptext=AB|format=12345|type=military-large|structure=1}}}}
</pre>

[[Category:模板]]
[[Category:车牌模板]]
</noinclude>";
            return (title, content);
        }
    }

    /// <summary>
    /// 获取公共CSS / Get common CSS
    /// </summary>
    public static (string title, string content) CommonCSS
    {
        get
        {
            string title = "MediaWiki:Common.css";
            string content = @"/* 中国车牌样式 - Chinese License Plate Styles */

/* 基础车牌容器 */
.license-plate {
    display: inline-block;
    font-family: 'Arial Black', 'Microsoft YaHei', sans-serif;
    font-weight: bold;
    padding: 8px 12px;
    border: 2px solid #000;
    border-radius: 4px;
    text-align: center;
    line-height: 1.2;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    white-space: nowrap;
}

/* 小型新能源汽车号牌 - Small New Energy Vehicle Plate */
.license-plate-new-energy-small {
    background: linear-gradient(to bottom, #ffffff 0%, #90EE90 100%);
    color: #000000;
}

/* 大型新能源汽车号牌 - Large New Energy Vehicle Plate */
.license-plate-new-energy-large {
    background: linear-gradient(to right, #FFD700 0%, #FFD700 30%, #90EE90 30%, #90EE90 100%);
    color: #000000;
}

/* 大型汽车号牌 - Large Vehicle Plate */
.license-plate-new-energy-large {
    background-color: #FFD700;
    color: #000000;
}

/* 挂车号牌 - Trailer Plate */
.license-plate-trailer {
    background-color: #FFD700;
    color: #000000;
}

.license-plate-trailer::after {
    content: '挂';
    margin-left: 4px;
}

/* 小型汽车号牌 - Small Vehicle Plate */
.license-plate-small {
    background-color: #0066CC;
    color: #FFFFFF;
}

/* 使馆号牌 - Embassy Plate */
.license-plate-embassy {
    background-color: #000000;
    color: #FFFFFF;
}

.license-plate-embassy::after {
    content: '使';
    margin-left: 4px;
}

/* 领馆号牌 - Consulate Plate */
.license-plate-consulate {
    background-color: #000000;
    color: #FFFFFF;
}

.license-plate-consulate::after {
    content: '领';
    margin-left: 4px;
}

/* 港澳出入境车牌 - Hong Kong/Macau Cross-border Plate */
.license-plate-hk-macau {
    background-color: #000000;
    color: #FFFFFF;
}

.license-plate-hk-macau.hk::after {
    content: '港';
    margin-left: 4px;
}

.license-plate-hk-macau.macau::after {
    content: '澳';
    margin-left: 4px;
}

/* 教练车车牌 - Driving School Plate */
.license-plate-training {
    background-color: #FFD700;
    color: #000000;
}

.license-plate-training::after {
    content: '学';
    margin-left: 4px;
}

/* 警车号牌 - Police Plate */
.license-plate-police {
    background-color: #FFFFFF;
    color: #000000;
    border-color: #000000;
}

.license-plate-police::after {
    content: '警';
    margin-left: 4px;
    color: #FF0000;
}

/* 应急救援车辆号牌 - Emergency Rescue Plate */
.license-plate-emergency {
    background-color: #FFFFFF;
    color: #000000;
    border-color: #000000;
}

.license-plate-emergency .type-indicator {
    color: #FF0000;
}

.license-plate-emergency::after {
    content: '应急';
    margin-left: 4px;
    color: #FF0000;
}

/* 武警车牌 - Armed Police Plate */
.license-plate-armed-police {
    background-color: #FFFFFF;
    color: #000000;
    border: 2px solid #FF0000;
    display: flex;
    flex-direction: column;
    padding: 4px 8px;
}

.license-plate-armed-police .top-section {
    color: #FF0000;
    font-size: 0.9em;
    border-bottom: 1px solid #FF0000;
    padding-bottom: 2px;
    margin-bottom: 2px;
}

.license-plate-armed-police .bottom-section {
    color: #000000;
    font-size: 1em;
}

/* 军车车牌（小型）- Military Plate (Small) */
.license-plate-military-small {
    background-color: #FFFFFF;
    color: #000000;
    border-color: #000000;
}

.license-plate-military-small .prefix {
    color: #FF0000;
}

/* 军车车牌（大型）- Military Plate (Large) */
.license-plate-military-large {
    background-color: #FFFFFF;
    color: #000000;
    border: 2px solid #000000;
    display: flex;
    flex-direction: column;
    padding: 4px 8px;
}

.license-plate-military-large .top-section {
    color: #FF0000;
    font-size: 0.9em;
    border-bottom: 1px solid #000000;
    padding-bottom: 2px;
    margin-bottom: 2px;
}

.license-plate-military-large .bottom-section {
    color: #000000;
    font-size: 1em;
}

/* 通用样式 */
.license-plate-region {
    margin-right: 4px;
}

.license-plate-format {
    letter-spacing: 1px;
}

/* 响应式设计 */
@media (max-width: 768px) {
    .license-plate {
        padding: 6px 10px;
        font-size: 0.9em;
    }
}

/* 打印样式 */
@media print {
    .license-plate {
        box-shadow: none;
        border: 1px solid #000;
    }
}

/* 兼容旧版样式 */
/* 中国燃油车牌 - 蓝底白字 */
.license-plate-CN.license-plate-fuel {
    background-color: #0066cc;
    color: #ffffff;
}

/* 中国电动车牌 - 绿底黑字 */
.license-plate-CN.license-plate-electric {
    background-color: #00cc66;
    color: #000000;
}

/* 美国车牌 - 白底黑字（通用） */
.license-plate-US {
    background-color: #ffffff;
    color: #000000;
    border-color: #333;
}

/* 欧盟车牌 - 白底黑字，左侧蓝条 */
.license-plate-EU {
    background: linear-gradient(to right, #003399 12%, #ffffff 12%);
    color: #000000;
    border-color: #000;
}";
            return (title, content);
        }
    }
}
