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

using SiliconLife.Collective;

namespace SiliconLife.Common.IM;

/// <summary>
/// 描述 IM 平台的单个配置字段（用于动态生成前端表单，字段定义与 config.json 中
/// <see cref="IMPlatformConfig.Config"/> 的键完全对应，保证配置零迁移）。
/// </summary>
public sealed class ConfigFieldSpec
{
    /// <summary>配置字典中的键名（如 appId、appSecret）。</summary>
    public string Key { get; init; } = string.Empty;

    /// <summary>显示名（用作本地化键或直接显示）。</summary>
    public string Label { get; init; } = string.Empty;

    /// <summary>输入类型："text" | "password" | "number"。</summary>
    public string Type { get; init; } = "text";

    /// <summary>是否必填。</summary>
    public bool Required { get; init; }

    /// <summary>输入框占位提示。</summary>
    public string? Placeholder { get; init; }

    /// <summary>是否为密钥类字段（不应明文回显/记录日志）。</summary>
    public bool IsSecret { get; init; }
}

/// <summary>
/// IM 平台元数据：平台标识、显示名、支持的授权模式、配置字段 schema、
/// OAuth 端点模板以及 Provider 工厂委托。
/// </summary>
public sealed class IMProviderMetadata
{
    /// <summary>平台标识（如 "webui"、"feishu"、"wecom"、"dingtalk"）。</summary>
    public string PlatformId { get; init; } = string.Empty;

    /// <summary>平台显示名。</summary>
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>支持的授权模式，至少含 "manual"；支持 OAuth 授权的平台再加 "oauth"。</summary>
    public List<string> AuthModes { get; init; } = new() { "manual" };

    /// <summary>平台配置字段 schema（与 WebUI 动态表单及 config.json 键一致）。</summary>
    public List<ConfigFieldSpec> ConfigFields { get; init; } = new();

    /// <summary>
    /// OAuth 授权页 URL 模板，含 {clientId}、{redirectUri}、{state} 占位符；仅 oauth 平台填写。
    /// </summary>
    public string? AuthorizeUrlTemplate { get; init; }

    /// <summary>OAuth 换取 token 的端点 URL 模板；仅 oauth 平台填写。</summary>
    public string? TokenUrlTemplate { get; init; }

    /// <summary>首次 OAuth 授权是否要求公网可达的回调地址。</summary>
    public bool NeedsPublicCallback { get; init; }

    /// <summary>官方开放平台/开发者后台链接（用于设置页"官方文档"跳转）；无则为 null。</summary>
    public string? HelpUrl { get; init; }

    /// <summary>接入帮助文案的本地化键（如 "IMHelp_feishu"），由 UI 层经本地化解析后展示。</summary>
    public string? HelpKey { get; init; }

    /// <summary>
    /// Provider 工厂委托。webui 依赖 App 层的 Router，无法在 Common 层构造，
    /// 因此其 CreateProvider 为 null，由宿主（Program.cs）保留特判创建。
    /// </summary>
    public Func<IMPlatformConfig, IIMProvider>? CreateProvider { get; init; }
}

/// <summary>
/// IM 平台元数据静态注册表。消除 Program.cs 中的字符串 switch 重复，
/// 并作为 WebUI 平台选项、动态表单 schema 的唯一数据来源。
/// </summary>
public static class IMProviderRegistry
{
    private static readonly List<IMProviderMetadata> _providers = new()
    {
        new IMProviderMetadata
        {
            PlatformId = "webui",
            DisplayName = "Web UI",
            AuthModes = new List<string> { "manual" },
            ConfigFields = new List<ConfigFieldSpec>(),
            NeedsPublicCallback = false,
            HelpUrl = null,
            HelpKey = "IMHelp_webui",
            // webui 依赖 App 层 Router，由宿主特判创建
            CreateProvider = null,
        },
        new IMProviderMetadata
        {
            PlatformId = "feishu",
            DisplayName = "飞书 (Feishu)",
            AuthModes = new List<string> { "manual", "oauth" },
            ConfigFields = new List<ConfigFieldSpec>
            {
                new() { Key = "appId", Label = "App ID", Type = "text", Required = true, Placeholder = "" },
                new() { Key = "appSecret", Label = "App Secret", Type = "password", Required = true, Placeholder = "", IsSecret = true },
                new() { Key = "verificationToken", Label = "Verification Token", Type = "text", Required = true, Placeholder = "" },
                new() { Key = "encryptKey", Label = "Encrypt Key", Type = "text", Required = false, Placeholder = "" },
                new() { Key = "callbackPath", Label = "Callback Path", Type = "text", Required = false, Placeholder = "/feishu/callback" },
                new() { Key = "listenPort", Label = "Listen Port", Type = "number", Required = false, Placeholder = "8080" },
            },
            AuthorizeUrlTemplate = "https://open.feishu.cn/open-apis/authen/v1/authorize?app_id={clientId}&redirect_uri={redirectUri}&state={state}",
            TokenUrlTemplate = "https://open.feishu.cn/open-apis/authen/v2/oauth/token",
            NeedsPublicCallback = false, // 飞书允许 localhost 回调
            HelpUrl = "https://open.feishu.cn/app",
            HelpKey = "IMHelp_feishu",
            CreateProvider = cfg => new FeishuIMProvider(cfg.Config),
        },
        new IMProviderMetadata
        {
            PlatformId = "wecom",
            DisplayName = "企业微信 (WeCom)",
            // OAuth 端点模板补齐后再开放 oauth 模式，避免 UI 暴露不可用的扫码授权入口
            AuthModes = new List<string> { "manual" },
            ConfigFields = new List<ConfigFieldSpec>
            {
                new() { Key = "corpId", Label = "Corp ID", Type = "text", Required = true, Placeholder = "" },
                new() { Key = "appSecret", Label = "App Secret", Type = "password", Required = true, Placeholder = "", IsSecret = true },
                new() { Key = "agentId", Label = "Agent ID", Type = "number", Required = true, Placeholder = "" },
                new() { Key = "token", Label = "Token", Type = "text", Required = true, Placeholder = "" },
                new() { Key = "encodingAESKey", Label = "Encoding AES Key", Type = "text", Required = true, Placeholder = "" },
                new() { Key = "callbackPath", Label = "Callback Path", Type = "text", Required = false, Placeholder = "/wecom/callback" },
                new() { Key = "listenPort", Label = "Listen Port", Type = "number", Required = false, Placeholder = "8080" },
            },
            // 企业微信 OAuth 端点模板留待授权向导实现时填充
            AuthorizeUrlTemplate = null,
            TokenUrlTemplate = null,
            NeedsPublicCallback = true,
            HelpUrl = "https://work.weixin.qq.com/wework_admin/frame#apps",
            HelpKey = "IMHelp_wecom",
            CreateProvider = cfg => new WeComIMProvider(cfg.Config),
        },
        new IMProviderMetadata
        {
            PlatformId = "dingtalk",
            DisplayName = "钉钉 (DingTalk)",
            // OAuth 端点模板补齐后再开放 oauth 模式，避免 UI 暴露不可用的扫码授权入口
            AuthModes = new List<string> { "manual" },
            ConfigFields = new List<ConfigFieldSpec>
            {
                new() { Key = "appKey", Label = "App Key", Type = "text", Required = true, Placeholder = "" },
                new() { Key = "appSecret", Label = "App Secret", Type = "password", Required = true, Placeholder = "", IsSecret = true },
                new() { Key = "robotCode", Label = "Robot Code", Type = "text", Required = true, Placeholder = "" },
                new() { Key = "eventMode", Label = "Event Mode", Type = "text", Required = false, Placeholder = "stream" },
                new() { Key = "callbackPath", Label = "Callback Path", Type = "text", Required = false, Placeholder = "/dingtalk/callback" },
                new() { Key = "listenPort", Label = "Listen Port", Type = "number", Required = false, Placeholder = "8080" },
            },
            // 钉钉 OAuth 端点模板留待授权向导实现时填充
            AuthorizeUrlTemplate = null,
            TokenUrlTemplate = null,
            NeedsPublicCallback = true,
            HelpUrl = "https://open-dev.dingtalk.com",
            HelpKey = "IMHelp_dingtalk",
            CreateProvider = cfg => new DingTalkIMProvider(cfg.Config),
        },
    };

    /// <summary>
    /// 获取全部已注册平台的元数据（保持注册顺序）。
    /// </summary>
    public static IReadOnlyList<IMProviderMetadata> GetAll() => _providers;

    /// <summary>
    /// 按平台标识获取元数据；未注册时返回 null。
    /// </summary>
    public static IMProviderMetadata? Get(string platformId)
    {
        return _providers.FirstOrDefault(p =>
            string.Equals(p.PlatformId, platformId, StringComparison.OrdinalIgnoreCase));
    }
}
