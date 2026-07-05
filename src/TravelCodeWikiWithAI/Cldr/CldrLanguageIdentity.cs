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

// 迁移自细需求/TravelCodeWikiWithAI/TravelCodeWikiWithAI.Core/CLDR/CLDRLanguageData.cs
// 变更：去除 IDictionary 继承，纯值类型；字段命名改为 PascalCase

namespace TravelCodeWikiWithAI.Cldr;

/// <summary>
/// CLDR 语言标识结构体 - 唯一标识一个 CLDR 区域设置
/// Migrated from legacy CLDRLanguageIdentity, removed IDictionary inheritance
/// </summary>
public struct CldrLanguageIdentity : IEquatable<CldrLanguageIdentity>
{
    public string Language;   // e.g., "zh", "en"
    public string Script;     // e.g., "Hans", "Hant", "*" = any
    public string Territory;  // e.g., "CN", "US", "*" = any
    public string Variant;    // e.g., "*", specific variant

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Language) || Language == "*")
            return "*";

        string result = Language;
        if (!string.IsNullOrWhiteSpace(Script) && Script != "*")
            result += "-" + Script;
        if (!string.IsNullOrWhiteSpace(Territory) && Territory != "*")
            result += "-" + Territory;
        if (!string.IsNullOrWhiteSpace(Variant) && Variant != "*")
            result += "-" + Variant;

        return result;
    }

    public bool Equals(CldrLanguageIdentity other)
    {
        return ToString() == other.ToString();
    }

    public override bool Equals(object? obj)
    {
        return obj is CldrLanguageIdentity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode(StringComparison.OrdinalIgnoreCase);
    }

    public static bool operator ==(CldrLanguageIdentity left, CldrLanguageIdentity right) => left.Equals(right);
    public static bool operator !=(CldrLanguageIdentity left, CldrLanguageIdentity right) => !left.Equals(right);

    /// <summary>
    /// 获取父级语言标识（去除最具体的部分）
    /// </summary>
    public CldrLanguageIdentity? GetParent()
    {
        if (Variant != "*")
        {
            return new CldrLanguageIdentity
            {
                Language = Language,
                Script = Script,
                Territory = Territory,
                Variant = "*"
            };
        }

        if (Territory != "*")
        {
            return new CldrLanguageIdentity
            {
                Language = Language,
                Script = Script,
                Territory = "*",
                Variant = "*"
            };
        }

        if (Script != "*")
        {
            return new CldrLanguageIdentity
            {
                Language = Language,
                Script = "*",
                Territory = "*",
                Variant = "*"
            };
        }

        return null;
    }
}
