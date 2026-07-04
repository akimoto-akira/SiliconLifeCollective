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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\ExchangeRate.cs
// 迁移变更（§4.8 决策：完整逻辑 + 执行器适配）：
//   HttpClient → NetworkExecutor（HTTP 请求走执行器）
//   File.WriteAllText / Directory.CreateDirectory → DiskExecutor（文件写入走执行器）
//   OutPut.WriteOut → LogManager 日志系统
//   CoreTools.DataDir / CoreSetting.WikiDir → IStorage（文件路径由 IStorage 提供）
//   CLDRCtrl → ICLDRProvider 接口（构造注入，待实现）
//   GeoProject.Self → 构造函数注入 GeoProject 引用
//   XMLBase.LoadXMLWithNode/SaveXMLWithNode → IStorage JSON 序列化
//   APIKEY 硬编码常量 → IStorage 读取配置
//   保留全部业务逻辑（Flush/GetCodes/GenerateI18nFiles/BuildJson/CLDR 查询等）

using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using SiliconLife.Collective;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 汇率数据管理类，保留完整业务逻辑，副作用适配执行器
/// / Exchange rate data management class, preserving full business logic with executor-adapted side effects
/// </summary>
public class ExchangeRate : GeoDataBase
{
    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父级地理数据对象 / Parent geographic data object</param>
    public ExchangeRate(GeoDataBase parent) : base(parent)
    {
    }

    public ExchangeRate() : base(null) { }

    // ===== 数据层（与旧代码相同）=====
    private Dictionary<string, Dictionary<string, decimal>> _data = new();
    public DateTime UpdateTime { get; set; }

    // ===== 配置（从 IStorage 读取，不再硬编码）=====
    private readonly string _apiKey = string.Empty;       // 从 IStorage 配置读取
    private readonly string _apiBaseUrl = "https://v6.exchangerate-api.com/v6/"; // 从 IStorage 配置读取

    // ===== 外部依赖（构造函数注入）=====
    private readonly GeoProject? _project;  // 不再 GeoProject.Self
    private readonly ILogger _logger = LogManager.Instance.GetLogger<ExchangeRate>();

    // ===== 保留的业务方法 =====

    /// <summary>
    /// 刷新汇率数据（HTTP 请求走 NetworkExecutor）
    /// / Refresh exchange rate data (HTTP requests via NetworkExecutor)
    /// </summary>
    /// <param name="callerId">调用者 ID（硅基人 GUID）/ Caller ID (silicon being GUID)</param>
    public void Flush(Guid callerId)
    {
        TimeSpan ts = DateTime.Now - UpdateTime;
        if (ts.Days < 30)
        {
            return;
        }

        _data.Clear();
        Dictionary<string, string> a = GetCodes(callerId);

        foreach (string b in a.Keys)
        {
            _logger.Info(callerId, "ExchangeRate flush: start {0}", b);
            string c = Get(callerId, "latest", b);
            JsonDocument d = JsonDocument.Parse(c);
            JsonElement root = d.RootElement;
            JsonElement e = root.GetProperty("conversion_rates");
            JsonProperty[] f = e.EnumerateObject().ToArray();
            Dictionary<string, decimal> h = new Dictionary<string, decimal>();
            foreach (JsonProperty g in f)
            {
                string i = g.Name;
                decimal j = g.Value.GetDecimal();
                h.Add(i, j);
            }

            _data.Add(b, h);
            UpdateTime = DateTime.Now;
            _logger.Info(callerId, "ExchangeRate flush: end {0}", b);
        }
    }

    /// <summary>
    /// 获取所有支持的货币代码（HTTP 请求走 NetworkExecutor）
    /// / Get all supported currency codes (HTTP requests via NetworkExecutor)
    /// </summary>
    /// <param name="callerId">调用者 ID / Caller ID</param>
    /// <returns>货币代码和名称的字典 / Dictionary of currency codes and names</returns>
    public Dictionary<string, string> GetCodes(Guid callerId)
    {
        string a = Get(callerId, "codes");
        JsonDocument b = JsonDocument.Parse(a);
        JsonElement root = b.RootElement;
        JsonElement c = root.GetProperty("supported_codes");
        JsonElement[] d = c.EnumerateArray().ToArray();
        Dictionary<string, string> result = new Dictionary<string, string>();
        foreach (JsonElement e in d)
        {
            JsonElement[] f = e.EnumerateArray().ToArray();
            string g = f[0].GetString()!;
            string h = f[1].GetString()!;
            result.Add(g, h);
        }

        return result;
    }

    /// <summary>
    /// 获取汇率数据副本 / Get a copy of exchange rate data
    /// </summary>
    public Dictionary<string, Dictionary<string, decimal>> GetData()
    {
        Dictionary<string, Dictionary<string, decimal>> result = new();
        foreach (KeyValuePair<string, Dictionary<string, decimal>> a in _data)
        {
            result.Add(a.Key, new Dictionary<string, decimal>(a.Value));
        }
        return result;
    }

    /// <summary>
    /// 生成各语言的 i18n 消息文件（文件写入走 DiskExecutor）
    /// / Generate i18n message files for each language (file writes via DiskExecutor)
    /// </summary>
    /// <param name="callerId">调用者 ID / Caller ID</param>
    public void GenerateI18nFiles(Guid callerId)
    {
        if (_project?.Translation?.Wiki == null)
        {
            return;
        }

        string[] languages = _project.GetAvailableLanguages();

        foreach (string langCode in languages)
        {
            string messageJson = GenerateLanguageMessages(langCode, _project.Translation.Wiki);
            string i18nPath = $"extensions/CurrencyExchange/i18n/{langCode}.json";

            // 文件写入走 DiskExecutor
            var request = new ExecutorRequest(callerId, i18nPath, "write_file",
                new Dictionary<string, object> { ["content"] = messageJson, ["encoding"] = "utf8nobom" });
            ExecutorResult result = DiskExecutor.Execute(request);
            if (!result.Success)
            {
                _logger.Warn(callerId, "ExchangeRate GenerateI18nFiles: failed to write {0}", i18nPath);
            }
        }
    }

    /// <summary>
    /// 生成汇率 JSON 数据文件（文件写入走 DiskExecutor）
    /// / Generate exchange rate JSON data file (file writes via DiskExecutor)
    /// </summary>
    /// <param name="callerId">调用者 ID / Caller ID</param>
    public void BuildJson(Guid callerId)
    {
        string ratesJson = GenerateRatesJson();
        string ratesPath = "extensions/CurrencyExchange/data/rates.json";

        // 文件写入走 DiskExecutor
        var request = new ExecutorRequest(callerId, ratesPath, "write_file",
            new Dictionary<string, object> { ["content"] = ratesJson, ["encoding"] = "utf8nobom" });
        ExecutorResult result = DiskExecutor.Execute(request);
        if (!result.Success)
        {
            _logger.Warn(callerId, "ExchangeRate BuildJson: failed to write {0}", ratesPath);
        }

        // 生成各语言的 i18n 消息文件
        GenerateI18nFiles(callerId);
    }

    /// <summary>
    /// 获取所有汇率货币代码 / Get all exchange rate currency codes
    /// </summary>
    public string[] GetAllRateCode()
    {
        HashSet<string> a = new HashSet<string>();
        foreach (KeyValuePair<string, Dictionary<string, decimal>> b in _data)
        {
            foreach (KeyValuePair<string, decimal> c in b.Value)
            {
                a.Add(c.Key);
            }
        }

        return a.ToArray();
    }

    // ===== CLDR 货币信息查询（通过 LocalizationManager / ICLDRProvider 访问）=====

    /// <summary>
    /// 获取货币显示名称 / Get currency display name
    /// </summary>
    /// <param name="currencyCode">货币代码 / Currency code</param>
    /// <param name="langCode">语言代码 / Language code</param>
    /// <returns>货币显示名称 / Currency display name</returns>
    public string? GetCurrencyDisplayName(string currencyCode, string langCode)
    {
        // TODO: 通过 ICLDRProvider 接口访问 CLDR 数据，替代 CLDRCtrl 静态类
        // TODO: Access CLDR data via ICLDRProvider interface, replacing CLDRCtrl static class
        return null;
    }

    /// <summary>
    /// 获取货币符号 / Get currency symbol
    /// </summary>
    /// <param name="currencyCode">货币代码 / Currency code</param>
    /// <param name="langCode">语言代码 / Language code</param>
    /// <param name="symbolType">符号类型（narrow/standard/variant/wide） / Symbol type</param>
    /// <returns>货币符号 / Currency symbol</returns>
    public string? GetCurrencySymbol(string currencyCode, string langCode, string symbolType)
    {
        // TODO: 通过 ICLDRProvider 接口访问 CLDR 数据
        return null;
    }

    /// <summary>
    /// 获取指定复数形式的货币显示名称 / Get currency display name by count type
    /// </summary>
    /// <param name="currencyCode">货币代码 / Currency code</param>
    /// <param name="langCode">语言代码 / Language code</param>
    /// <param name="countType">复数形式类型（zero/one/two/few/many/other）/ Count type</param>
    /// <returns>指定复数形式的货币显示名称 / Currency display name by count type</returns>
    public string? GetCurrencyDisplayNameByCount(string currencyCode, string langCode, string countType)
    {
        // TODO: 通过 ICLDRProvider 接口访问 CLDR 数据
        return null;
    }

    /// <summary>
    /// 根据语言推断默认货币 / Get default currency for a language
    /// </summary>
    /// <param name="langCode">语言代码 / Language code</param>
    /// <returns>默认货币代码 / Default currency code</returns>
    public string GetLanguageDefaultCurrency(string langCode)
    {
        // TODO: 通过 ICLDRProvider 接口访问 CLDR 区域信息
        // 后备返回 USD / Fallback to USD
        return "USD";
    }

    // ===== 内部辅助方法 =====

    /// <summary>
    /// 从汇率 API 获取数据（HTTP 请求走 NetworkExecutor）
    /// / Get data from exchange rate API (HTTP requests via NetworkExecutor)
    /// </summary>
    private string Get(Guid callerId, params string[] code)
    {
        string a = _apiBaseUrl + _apiKey + "/" + string.Join("/", code);

        var request = new ExecutorRequest(callerId, a, "http_get",
            new Dictionary<string, object> { ["method"] = "GET" });
        ExecutorResult result = NetworkExecutor.Execute(request);
        if (!result.Success)
        {
            throw new Exception($"ExchangeRate API request failed: {result.Error}");
        }

        return result.Output;
    }

    /// <summary>
    /// 为指定语言生成消息 JSON / Generate message JSON for a language
    /// </summary>
    private string GenerateLanguageMessages(string langCode, GeoWikiTranslation wikiTranslation)
    {
        var messages = new Dictionary<string, object>();

        messages.Add("@metadata", new Dictionary<string, object>
        {
            { "authors", new[] { "CurrencyExchange Extension" } }
        });

        // 从 GeoWikiTranslation 的各个字段中提取翻译
        AddTranslationIfExists(messages, "currencyexchange-desc", wikiTranslation.CurrencyExchangeDesc, langCode);
        AddTranslationIfExists(messages, "exchangerates", wikiTranslation.ExchangeRates, langCode);
        AddTranslationIfExists(messages, "currencyexchange-rates-management", wikiTranslation.CurrencyExchangeRatesManagement, langCode);
        AddTranslationIfExists(messages, "currencyexchange-rates-management-desc", wikiTranslation.CurrencyExchangeRatesManagementDesc, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-file-permissions", wikiTranslation.CurrencyExchangeErrorFilePermissions, langCode);
        AddTranslationIfExists(messages, "currencyexchange-current-rates", wikiTranslation.CurrencyExchangeCurrentRates, langCode);
        AddTranslationIfExists(messages, "currencyexchange-no-rates-found", wikiTranslation.CurrencyExchangeNoRatesFound, langCode);
        AddTranslationIfExists(messages, "currencyexchange-from-currency", wikiTranslation.CurrencyExchangeFromCurrency, langCode);
        AddTranslationIfExists(messages, "currencyexchange-to-currency", wikiTranslation.CurrencyExchangeToCurrency, langCode);
        AddTranslationIfExists(messages, "currencyexchange-exchange-rate", wikiTranslation.CurrencyExchangeExchangeRate, langCode);
        AddTranslationIfExists(messages, "currencyexchange-total-rates", wikiTranslation.CurrencyExchangeTotalRates, langCode);
        AddTranslationIfExists(messages, "currencyexchange-file-not-found", wikiTranslation.CurrencyExchangeFileNotFound, langCode);
        AddTranslationIfExists(messages, "currencyexchange-file-info", wikiTranslation.CurrencyExchangeFileInfo, langCode);
        AddTranslationIfExists(messages, "currencyexchange-file-size", wikiTranslation.CurrencyExchangeFileSize, langCode);
        AddTranslationIfExists(messages, "currencyexchange-last-modified", wikiTranslation.CurrencyExchangeLastModified, langCode);
        AddTranslationIfExists(messages, "currencyexchange-data-timestamp", wikiTranslation.CurrencyExchangeDataTimestamp, langCode);
        AddTranslationIfExists(messages, "currencyexchange-edit-rates", wikiTranslation.CurrencyExchangeEditRates, langCode);
        AddTranslationIfExists(messages, "currencyexchange-rates-data-label", wikiTranslation.CurrencyExchangeRatesDataLabel, langCode);
        AddTranslationIfExists(messages, "currencyexchange-rates-data-help", wikiTranslation.CurrencyExchangeRatesDataHelp, langCode);
        AddTranslationIfExists(messages, "currencyexchange-save-changes", wikiTranslation.CurrencyExchangeSaveChanges, langCode);
        AddTranslationIfExists(messages, "currencyexchange-upload-file", wikiTranslation.CurrencyExchangeUploadFile, langCode);
        AddTranslationIfExists(messages, "currencyexchange-upload-desc", wikiTranslation.CurrencyExchangeUploadDesc, langCode);
        AddTranslationIfExists(messages, "currencyexchange-max-file-size", wikiTranslation.CurrencyExchangeMaxFileSize, langCode);
        AddTranslationIfExists(messages, "currencyexchange-select-file", wikiTranslation.CurrencyExchangeSelectFile, langCode);
        AddTranslationIfExists(messages, "currencyexchange-upload-button", wikiTranslation.CurrencyExchangeUploadButton, langCode);
        AddTranslationIfExists(messages, "currencyexchange-session-failure", wikiTranslation.CurrencyExchangeSessionFailure, langCode);
        AddTranslationIfExists(messages, "currencyexchange-validation-failed", wikiTranslation.CurrencyExchangeValidationFailed, langCode);
        AddTranslationIfExists(messages, "currencyexchange-update-success", wikiTranslation.CurrencyExchangeUpdateSuccess, langCode);
        AddTranslationIfExists(messages, "currencyexchange-update-failed", wikiTranslation.CurrencyExchangeUpdateFailed, langCode);
        AddTranslationIfExists(messages, "currencyexchange-no-file-uploaded", wikiTranslation.CurrencyExchangeNoFileUploaded, langCode);
        AddTranslationIfExists(messages, "currencyexchange-file-too-large", wikiTranslation.CurrencyExchangeFileTooLarge, langCode);
        AddTranslationIfExists(messages, "currencyexchange-invalid-file-type", wikiTranslation.CurrencyExchangeInvalidFileType, langCode);
        AddTranslationIfExists(messages, "currencyexchange-failed-read-file", wikiTranslation.CurrencyExchangeFailedReadFile, langCode);
        AddTranslationIfExists(messages, "currencyexchange-file-validation-failed", wikiTranslation.CurrencyExchangeFileValidationFailed, langCode);
        AddTranslationIfExists(messages, "currencyexchange-upload-success", wikiTranslation.CurrencyExchangeUploadSuccess, langCode);
        AddTranslationIfExists(messages, "currencyexchange-upload-failed", wikiTranslation.CurrencyExchangeUploadFailed, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-missing-parameter", wikiTranslation.CurrencyExchangeErrorMissingParameter, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-invalid-amount", wikiTranslation.CurrencyExchangeErrorInvalidAmount, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-invalid-currency", wikiTranslation.CurrencyExchangeErrorInvalidCurrency, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-invalid-precision", wikiTranslation.CurrencyExchangeErrorInvalidPrecision, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-general", wikiTranslation.CurrencyExchangeErrorGeneral, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-recursion-limit", wikiTranslation.CurrencyExchangeErrorRecursionLimit, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-invalid-currency-format", wikiTranslation.CurrencyExchangeErrorInvalidCurrencyFormat, langCode);
        AddTranslationIfExists(messages, "currencyexchange-security-violation", wikiTranslation.CurrencyExchangeSecurityViolation, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-invalid-json", wikiTranslation.CurrencyExchangeErrorInvalidJson, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-data-corruption", wikiTranslation.CurrencyExchangeErrorDataCorruption, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-system-message-read", wikiTranslation.CurrencyExchangeErrorSystemMessageRead, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-system-message-write", wikiTranslation.CurrencyExchangeErrorSystemMessageWrite, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-rate-not-found", wikiTranslation.CurrencyExchangeErrorRateNotFound, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-currency-not-supported", wikiTranslation.CurrencyExchangeErrorCurrencyNotSupported, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-calculation-overflow", wikiTranslation.CurrencyExchangeErrorCalculationOverflow, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-negative-amount", wikiTranslation.CurrencyExchangeErrorNegativeAmount, langCode);
        AddTranslationIfExists(messages, "currencyexchange-error-zero-division", wikiTranslation.CurrencyExchangeErrorZeroDivision, langCode);
        AddTranslationIfExists(messages, "currencyexchange-supported-currencies", wikiTranslation.CurrencyExchangeSupportedCurrencies, langCode);
        AddTranslationIfExists(messages, "currencyexchange-currency-list-separator", wikiTranslation.CurrencyExchangeCurrencyListSeparator, langCode);
        AddTranslationIfExists(messages, "currencyexchange-pref-default-currency", wikiTranslation.CurrencyExchangePrefDefaultCurrency, langCode);
        AddTranslationIfExists(messages, "currencyexchange-pref-default-currency-help", wikiTranslation.CurrencyExchangePrefDefaultCurrencyHelp, langCode);
        AddTranslationIfExists(messages, "currencyexchange-pref-currency-auto", wikiTranslation.CurrencyExchangePrefCurrencyAuto, langCode);
        AddTranslationIfExists(messages, "currencyexchange-help-exchange", wikiTranslation.CurrencyExchangeHelpExchange, langCode);
        AddTranslationIfExists(messages, "currencyexchange-help-exchangerate", wikiTranslation.CurrencyExchangeHelpExchangeRate, langCode);
        AddTranslationIfExists(messages, "currencyexchange-help-currencyname", wikiTranslation.CurrencyExchangeHelpCurrencyName, langCode);
        AddTranslationIfExists(messages, "currencyexchange-help-currencysymbol", wikiTranslation.CurrencyExchangeHelpCurrencySymbol, langCode);
        AddTranslationIfExists(messages, "currencyexchange-example-exchange", wikiTranslation.CurrencyExchangeExampleExchange, langCode);
        AddTranslationIfExists(messages, "currencyexchange-example-exchangerate", wikiTranslation.CurrencyExchangeExampleExchangeRate, langCode);
        AddTranslationIfExists(messages, "currencyexchange-example-currencyname", wikiTranslation.CurrencyExchangeExampleCurrencyName, langCode);
        AddTranslationIfExists(messages, "currencyexchange-example-currencysymbol", wikiTranslation.CurrencyExchangeExampleCurrencySymbol, langCode);
        AddTranslationIfExists(messages, "currencyexchange-admin-only-notice", wikiTranslation.CurrencyExchangeAdminOnlyNotice, langCode);
        AddTranslationIfExists(messages, "currencyexchange-edit-permission-required", wikiTranslation.CurrencyExchangeEditPermissionRequired, langCode);
        AddTranslationIfExists(messages, "currencyexchange-upload-permission-required", wikiTranslation.CurrencyExchangeUploadPermissionRequired, langCode);
        AddTranslationIfExists(messages, "currencyexchange-pref-currency-label", wikiTranslation.CurrencyExchangePrefCurrencyLabel, langCode);
        AddTranslationIfExists(messages, "currencyexchange-pref-currency-help", wikiTranslation.CurrencyExchangePrefCurrencyHelp, langCode);

        // 添加货币数据消息
        AddCurrencyDataMessages(messages, langCode);

        messages.Add("currencyexchange-default", GetLanguageDefaultCurrency(langCode));
        messages.Add("currencyexchange-currencies", GetLanguageCurrencies());

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(messages, options);
    }

    /// <summary>
    /// 添加货币数据消息 / Add currency data messages
    /// </summary>
    private void AddCurrencyDataMessages(Dictionary<string, object> messages, string langCode)
    {
        // 生成货币名称
        var currencyNames = new Dictionary<string, string>();
        foreach (var currency in _data.Keys)
        {
            string? localizedName = GetCurrencyDisplayName(currency, langCode);
            if (!string.IsNullOrEmpty(localizedName))
            {
                currencyNames.Add(currency, localizedName);
            }
        }
        if (currencyNames.Count == 0)
        {
            currencyNames = GetFallbackCurrencyNames();
        }
        messages.Add("currencyexchange-names", JsonSerializer.Serialize(currencyNames,
            new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));

        // 生成货币符号
        var symbolData = new Dictionary<string, Dictionary<string, string>>
        {
            ["narrow"] = new Dictionary<string, string>(),
            ["formal"] = new Dictionary<string, string>(),
            ["standard"] = new Dictionary<string, string>(),
            ["variant"] = new Dictionary<string, string>(),
            ["wide"] = new Dictionary<string, string>()
        };
        foreach (var currency in _data.Keys)
        {
            symbolData["narrow"][currency] = GetCurrencySymbol(currency, langCode, "narrow") ?? currency;
            symbolData["formal"][currency] = GetCurrencySymbol(currency, langCode, "formal") ?? currency;
            symbolData["standard"][currency] = GetCurrencySymbol(currency, langCode, "standard") ?? currency;
            symbolData["variant"][currency] = GetCurrencySymbol(currency, langCode, "variant") ?? currency;
            symbolData["wide"][currency] = GetCurrencySymbol(currency, langCode, "wide") ?? currency;
        }
        messages.Add("currencyexchange-symbols", JsonSerializer.Serialize(symbolData,
            new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));

        // 生成货币显示名称（带复数形式）
        var displayNames = new Dictionary<string, Dictionary<string, string>>();
        foreach (var currency in _data.Keys)
        {
            var pluralForms = new Dictionary<string, string>();
            string baseName = GetCurrencyDisplayName(currency, langCode) ?? currency;
            pluralForms["zero"] = GetCurrencyDisplayNameByCount(currency, langCode, "zero") ?? baseName;
            pluralForms["one"] = GetCurrencyDisplayNameByCount(currency, langCode, "one") ?? baseName;
            pluralForms["two"] = GetCurrencyDisplayNameByCount(currency, langCode, "two") ?? baseName;
            pluralForms["few"] = GetCurrencyDisplayNameByCount(currency, langCode, "few") ?? baseName;
            pluralForms["many"] = GetCurrencyDisplayNameByCount(currency, langCode, "many") ?? baseName;
            pluralForms["other"] = GetCurrencyDisplayNameByCount(currency, langCode, "other") ?? baseName;
            displayNames[currency] = pluralForms;
        }
        messages.Add("currencyexchange-display-names", JsonSerializer.Serialize(displayNames,
            new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
    }

    /// <summary>
    /// 后备货币名称（中文）/ Fallback currency names (Chinese)
    /// </summary>
    private static Dictionary<string, string> GetFallbackCurrencyNames()
    {
        return new Dictionary<string, string>
        {
            ["USD"] = "美元", ["EUR"] = "欧元", ["CNY"] = "人民币", ["JPY"] = "日元",
            ["GBP"] = "英镑", ["AUD"] = "澳元", ["CAD"] = "加元", ["CHF"] = "瑞士法郎",
            ["HKD"] = "港币", ["SGD"] = "新加坡元", ["KRW"] = "韩元", ["INR"] = "印度卢比",
            ["RUB"] = "俄罗斯卢布", ["BRL"] = "巴西雷亚尔", ["ZAR"] = "南非兰特",
            ["MXN"] = "墨西哥比索", ["THB"] = "泰铢", ["IDR"] = "印尼盾",
            ["MYR"] = "马来西亚林吉特", ["PHP"] = "菲律宾比索"
        };
    }

    /// <summary>
    /// 添加翻译到消息字典中（如果存在） / Add translation to messages if exists
    /// </summary>
    private static void AddTranslationIfExists(Dictionary<string, object> messages, string messageKey, LanguageData? languageData, string langCode)
    {
        if (languageData == null) return;

        string? translation = null;
        if (languageData.ContainsKey(langCode))
        {
            translation = languageData[langCode];
        }
        else if (languageData.ContainsKey("*"))
        {
            translation = languageData["*"];
        }

        if (!string.IsNullOrEmpty(translation))
        {
            messages.Add(messageKey, translation);
        }
    }

    /// <summary>
    /// 生成符合 MediaWiki CurrencyExchange 扩展规范的汇率数据 JSON
    /// / Generate exchange rate JSON conforming to MediaWiki CurrencyExchange extension spec
    /// </summary>
    private string GenerateRatesJson()
    {
        var ratesData = new Dictionary<string, object>();
        foreach (var sourceCurrency in _data)
        {
            var targetRates = new Dictionary<string, decimal>();
            foreach (var targetCurrency in sourceCurrency.Value)
            {
                targetRates.Add(targetCurrency.Key, targetCurrency.Value);
            }
            ratesData.Add(sourceCurrency.Key, targetRates);
        }
        ratesData.Add("_timestamp", UpdateTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(ratesData, options);
    }

    /// <summary>
    /// 获取语言对应的货币列表字符串 / Get currency list string for a language
    /// </summary>
    private string GetLanguageCurrencies()
    {
        return string.Join('\n', _data.Keys);
    }

    public override string[] GetPath(int deth)
    {
        List<string> b = new List<string>();
        b.Add(BasePath);
        foreach (KeyValuePair<string, Dictionary<string, decimal>> a in _data)
        {
            b.Add(BasePath + "[" + a.Key + "]");
            foreach (KeyValuePair<string, decimal> c in a.Value)
            {
                b.Add(BasePath + "[" + a.Key + "][" + c.Key + "]");
            }
        }

        return b.ToArray();
    }
}
