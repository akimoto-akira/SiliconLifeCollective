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

using System.Text.Json;
using SiliconLife.Speedy;

namespace TravelCodeWikiWithAI.Cldr;

/// <summary>
/// CLDR 数据提供者 - 直接从 cldr.spk 读取原始 CLDR JSON
/// spk 内部路径前缀为 cldr-json/，与 cldr-json 文件夹结构完全一致
/// 不使用任何 System.IO / Reflection / Network API
/// </summary>
public sealed class CldrDataProvider
{
    private readonly SpeedyPack _pack;

    /// <summary>spk 内部路径前缀</summary>
    private const string Prefix = "cldr-json";

    // 惰性缓存
    private Dictionary<string, string>? _allLanguages;
    private List<CldrLanguageIdentity>? _allIdentities;
    private Dictionary<string, string>? _likelySubtags;
    private Dictionary<string, CldrNumberInfo>? _numberCache;

    public CldrDataProvider(SpeedyPack pack) => _pack = pack;

    // ─── 语言相关 ───────────────────────────────────────────

    /// <summary>
    /// 获取所有支持的语言代码及其显示名称（用 en 语言显示）
    /// 替代旧 SysTool.GetAllLanguage()，数据源从 PHPText.resx 改为 CLDR
    /// </summary>
    public Dictionary<string, string> GetAllLanguages()
    {
        if (_allLanguages != null) return _allLanguages;

        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var identities = GetAllIdentities();
        var likelySubtags = GetLikelySubtags();

        // 尝试从 en 的 localeDisplayNames 读取语言显示名
        var enLanguages = ReadLanguageDisplayNames("en");

        foreach (var id in identities)
        {
            string code = id.ToString();
            if (code == "*") continue;

            // 从 en 的语言显示名中查找
            string displayName = code;
            if (enLanguages != null && enLanguages.TryGetValue(id.Language, out var name))
            {
                displayName = name;
            }

            result[code] = displayName;
        }

        // 补充 likelySubtags 中的语言代码（有些语言没有自己的 identity XML 但在 likelySubtags 中出现）
        foreach (var kvp in likelySubtags)
        {
            if (!result.ContainsKey(kvp.Key))
            {
                string displayName = kvp.Key;
                if (enLanguages != null)
                {
                    // 尝试从语言代码中提取基础语言
                    string baseLang = kvp.Key.Split('-')[0].ToLower();
                    if (enLanguages.TryGetValue(baseLang, out var name))
                        displayName = name;
                }
                result[kvp.Key] = displayName;
            }
        }

        _allLanguages = result;
        return result;
    }

    /// <summary>
    /// 获取指定源语言在目标语言中的显示名称
    /// 替代旧 CLDRCtrl.GetLanguageDisplayName()
    /// </summary>
    public string GetLanguageDisplayName(string sourceCode, string targetCode)
    {
        var languages = ReadLanguageDisplayNames(targetCode);
        if (languages != null)
        {
            // 直接查找完整代码
            if (languages.TryGetValue(sourceCode, out var name))
                return name;

            // 查找基础语言代码
            string baseLang = sourceCode.Split('-')[0].ToLower();
            if (languages.TryGetValue(baseLang, out name))
                return name;
        }

        // fallback: 尝试 en
        if (targetCode != "en")
        {
            var enLanguages = ReadLanguageDisplayNames("en");
            if (enLanguages != null)
            {
                string baseLang = sourceCode.Split('-')[0].ToLower();
                if (enLanguages.TryGetValue(baseLang, out var name))
                    return name;
            }
        }

        return sourceCode;
    }

    /// <summary>
    /// 根据语言代码查找 CldrLanguageIdentity
    /// 替代旧 CLDRCtrl.FindLanguage()
    /// </summary>
    public CldrLanguageIdentity? FindLanguage(string code)
    {
        var likelySubtags = GetLikelySubtags();

        // likelySubtags 可以将简写（如 "zh-CN"）展开为完整标签（如 "zh-Hans-CN"）
        string lookupCode = code;
        if (likelySubtags.TryGetValue(code, out var expanded))
        {
            lookupCode = expanded;
        }

        // 解析 code 为 identity 组件
        var identity = ParseCodeToIdentity(lookupCode);
        if (identity == null) return null;

        // 验证是否在已知 identity 列表中
        var allIdentities = GetAllIdentities();
        foreach (var id in allIdentities)
        {
            if (id.Language.Equals(identity.Value.Language, StringComparison.OrdinalIgnoreCase))
            {
                bool match = true;
                if (id.Script != "*" && identity.Value.Script != "*" &&
                    !id.Script.Equals(identity.Value.Script, StringComparison.OrdinalIgnoreCase))
                    match = false;
                if (id.Territory != "*" && identity.Value.Territory != "*" &&
                    !id.Territory.Equals(identity.Value.Territory, StringComparison.OrdinalIgnoreCase))
                    match = false;

                if (match) return id;
            }
        }

        // 未在 identity 列表中找到，返回解析结果
        return identity;
    }

    // ─── 货币相关 ───────────────────────────────────────────

    /// <summary>
    /// 获取指定区域的数字/货币信息
    /// 替代旧 CLDRCtrl.FindNumber()
    /// </summary>
    public CldrNumberInfo? GetNumberInfo(string localeCode)
    {
        if (_numberCache != null && _numberCache.TryGetValue(localeCode, out var cached))
            return cached;

        string jsonPath = $"{Prefix}/cldr-numbers-full/main/{localeCode}/numbers.json";
        var doc = ReadJsonFromPack(jsonPath);
        if (doc == null) return null;

        var info = ParseNumberInfo(doc);
        doc.Dispose();

        (_numberCache ??= new())[localeCode] = info;
        return info;
    }

    // ─── 区域显示名称 ───────────────────────────────────────

    /// <summary>
    /// 获取地区显示名称（如 "CN" 在 zh-CN 中显示为 "中国"）
    /// </summary>
    public string GetTerritoryDisplayName(string territoryCode, string targetLocale)
    {
        var territories = ReadTerritoryDisplayNames(targetLocale);
        if (territories != null && territories.TryGetValue(territoryCode, out var name))
            return name;

        // fallback en
        if (targetLocale != "en")
        {
            var enTerritories = ReadTerritoryDisplayNames("en");
            if (enTerritories != null && enTerritories.TryGetValue(territoryCode, out name))
                return name;
        }

        return territoryCode;
    }

    // ─── 基础设施 ───────────────────────────────────────────

    /// <summary>从 SpeedyPack 读取原始 JSON 并解析为 JsonDocument</summary>
    private JsonDocument? ReadJsonFromPack(string path)
    {
        var bytes = _pack.Read(path);
        if (bytes == null) return null;
        return JsonDocument.Parse(bytes);
    }

    /// <summary>获取所有语言标识（从 common/main/*.xml 解析的 identity 信息，已预存在 spk 中）</summary>
    private List<CldrLanguageIdentity> GetAllIdentities()
    {
        if (_allIdentities != null) return _allIdentities;

        var result = new List<CldrLanguageIdentity>();

        // 从 likelySubtags 中提取所有语言标识
        var likelySubtags = GetLikelySubtags();
        foreach (var kvp in likelySubtags)
        {
            var id = ParseCodeToIdentity(kvp.Value);
            if (id != null && id.Value.Language != "*")
            {
                result.Add(id.Value);
            }
        }

        _allIdentities = result;
        return result;
    }

    /// <summary>获取 LikelySubtags 映射</summary>
    public Dictionary<string, string> GetLikelySubtags()
    {
        if (_likelySubtags != null) return _likelySubtags;

        string jsonPath = $"{Prefix}/cldr-core/supplemental/likelySubtags.json";
        var doc = ReadJsonFromPack(jsonPath);
        if (doc == null)
        {
            _likelySubtags = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            return _likelySubtags;
        }

        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        try
        {
            // JSON 结构: { "supplemental": { "likelySubtags": { "zh": "zh-Hans-CN", ... } } }
            if (doc.RootElement.TryGetProperty("supplemental", out var supplemental) &&
                supplemental.TryGetProperty("likelySubtags", out var subtags))
            {
                foreach (var prop in subtags.EnumerateObject())
                {
                    result[prop.Name] = prop.Value.GetString() ?? prop.Name;
                }
            }
        }
        finally
        {
            doc.Dispose();
        }

        _likelySubtags = result;
        return result;
    }

    /// <summary>
    /// 从 localeDisplayNames.json 中读取语言显示名称
    /// 返回 Dictionary: languageCode → displayName
    /// </summary>
    private Dictionary<string, string>? ReadLanguageDisplayNames(string localeCode)
    {
        string jsonPath = $"{Prefix}/cldr-localenames-full/main/{localeCode}/localeDisplayNames.json";
        var doc = ReadJsonFromPack(jsonPath);
        if (doc == null) return null;

        try
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // JSON 结构: { "main": { "{localeCode}": { "localeDisplayNames": { "languages": { ... } } } } }
            if (doc.RootElement.TryGetProperty("main", out var main))
            {
                foreach (var localeProp in main.EnumerateObject())
                {
                    if (localeProp.Value.TryGetProperty("localeDisplayNames", out var displayNames) &&
                        displayNames.TryGetProperty("languages", out var languages))
                    {
                        foreach (var langProp in languages.EnumerateObject())
                        {
                            result[langProp.Name] = langProp.Value.GetString() ?? langProp.Name;
                        }
                    }
                }
            }

            return result;
        }
        finally
        {
            doc.Dispose();
        }
    }

    /// <summary>
    /// 从 localeDisplayNames.json 中读取地区显示名称
    /// 返回 Dictionary: territoryCode → displayName
    /// </summary>
    private Dictionary<string, string>? ReadTerritoryDisplayNames(string localeCode)
    {
        string jsonPath = $"{Prefix}/cldr-localenames-full/main/{localeCode}/localeDisplayNames.json";
        var doc = ReadJsonFromPack(jsonPath);
        if (doc == null) return null;

        try
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (doc.RootElement.TryGetProperty("main", out var main))
            {
                foreach (var localeProp in main.EnumerateObject())
                {
                    if (localeProp.Value.TryGetProperty("localeDisplayNames", out var displayNames) &&
                        displayNames.TryGetProperty("territories", out var territories))
                    {
                        foreach (var terrProp in territories.EnumerateObject())
                        {
                            result[terrProp.Name] = terrProp.Value.GetString() ?? terrProp.Name;
                        }
                    }
                }
            }

            return result;
        }
        finally
        {
            doc.Dispose();
        }
    }

    /// <summary>
    /// 从 numbers.json 解析货币信息
    /// </summary>
    private CldrNumberInfo ParseNumberInfo(JsonDocument doc)
    {
        var info = new CldrNumberInfo();

        try
        {
            if (!doc.RootElement.TryGetProperty("main", out var main)) return info;

            foreach (var localeProp in main.EnumerateObject())
            {
                if (!localeProp.Value.TryGetProperty("numbers", out var numbers)) continue;

                // defaultNumberingSystem
                if (numbers.TryGetProperty("defaultNumberingSystem", out var dns))
                    info.DefaultNumberingSystem = dns.GetString();

                // currencies
                if (numbers.TryGetProperty("currencies", out var currencies))
                {
                    foreach (var currProp in currencies.EnumerateObject())
                    {
                        var ci = new CldrCurrencyInfo();
                        foreach (var field in currProp.Value.EnumerateObject())
                        {
                            switch (field.Name)
                            {
                                case "symbol":
                                    ci.Symbol = field.Value.GetString();
                                    break;
                                case "symbol-alt-narrow":
                                    ci.SymbolAltNarrow = field.Value.GetString();
                                    break;
                                case "symbol-alt-variant":
                                    ci.SymbolAltVariant = field.Value.GetString();
                                    break;
                                case "displayName":
                                    ci.DisplayName = field.Value.GetString();
                                    break;
                                case "displayName-count-one":
                                    ci.DisplayNameCountOne = field.Value.GetString();
                                    break;
                                case "displayName-count-other":
                                    ci.DisplayNameCountOther = field.Value.GetString();
                                    break;
                                case "displayName-count-zero":
                                    ci.DisplayNameCountZero = field.Value.GetString();
                                    break;
                                case "displayName-count-two":
                                    ci.DisplayNameCountTwo = field.Value.GetString();
                                    break;
                                case "displayName-count-few":
                                    ci.DisplayNameCountFew = field.Value.GetString();
                                    break;
                                case "displayName-count-many":
                                    ci.DisplayNameCountMany = field.Value.GetString();
                                    break;
                            }
                        }
                        info.Currencies[currProp.Name] = ci;
                    }
                }
            }
        }
        catch
        {
            // 容错：解析失败返回部分结果
        }

        return info;
    }

    /// <summary>
    /// 将语言代码字符串解析为 CldrLanguageIdentity
    /// 逻辑迁移自旧 CLDRCtrl.FindLanguage() 中的解析代码
    /// </summary>
    private static CldrLanguageIdentity? ParseCodeToIdentity(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return null;

        string[] parts = code.ToLower().Split('-');
        if (parts.Length == 0) return null;

        // 第一段：语言代码（2-3 个小写字母）
        string language = parts[0];
        if (language.Length < 2 || language.Length > 3) return null;
        foreach (char c in language)
            if (c < 'a' || c > 'z') return null;

        string script = "*";
        string territory = "*";
        string variant = "*";

        for (int i = 1; i < parts.Length; i++)
        {
            // Script: 4 个小写字母（首字母大写形式，但此处用小写比较）
            if (script == "*" && parts[i].Length == 4)
            {
                bool allAlpha = true;
                foreach (char c in parts[i])
                    if (c < 'a' || c > 'z') { allAlpha = false; break; }
                if (allAlpha)
                {
                    script = parts[i];
                    continue;
                }
            }

            // Territory: 2 个字母 或 3 个数字
            if (territory == "*")
            {
                if (parts[i].Length == 2 || parts[i].Length == 3)
                {
                    territory = parts[i];
                    continue;
                }
            }

            // Variant: 其余部分
            if (variant == "*")
            {
                variant = parts[i];
            }
        }

        return new CldrLanguageIdentity
        {
            Language = language,
            Script = script,
            Territory = territory,
            Variant = variant
        };
    }
}
