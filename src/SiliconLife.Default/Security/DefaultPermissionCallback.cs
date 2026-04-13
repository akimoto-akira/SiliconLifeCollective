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

using System.Net;
using System.Net.Sockets;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Default permission callback implementation.
/// Domain-specific permission rules fully aligned with dpf.txt specification.
/// Covers: network (whitelist/blacklist/IP ranges), command line (cross-platform),
/// file access (dangerous extensions, system dirs, user dirs), and fallback defaults.
/// </summary>
public class DefaultPermissionCallback : IPermissionCallback
{
    private readonly string _appDataDirectory;

    /// <summary>
    /// Creates a DefaultPermissionCallback with the application data directory.
    /// The app data directory is used for:
    /// - Blocking access to the data directory (except own Temp subfolder)
    /// - Deriving per-being data directories for Temp allow rule
    /// </summary>
    /// <param name="appDataDirectory">The global application data directory path</param>
    public DefaultPermissionCallback(string appDataDirectory)
    {
        _appDataDirectory = appDataDirectory?.ToLowerInvariant() ?? string.Empty;
    }

    /// <summary>
    /// Evaluates a permission request using default rules (dpf.txt specification).
    /// </summary>
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        // 网络操作放行规则 / Network access allow rules
        if (permissionType == PermissionType.NetworkAccess && !string.IsNullOrWhiteSpace(resource))
        {
            return EvaluateNetwork(resource);
        }

        // 命令行规则（跨平台）/ Command line rules (cross-platform)
        if (permissionType == PermissionType.CommandLine && !string.IsNullOrWhiteSpace(resource))
        {
            return EvaluateCommandLine(callerId, resource);
        }

        // 文件访问规则（跨平台）/ File access rules (cross-platform)
        if (permissionType == PermissionType.FileAccess && !string.IsNullOrWhiteSpace(resource))
        {
            return EvaluateFileAccess(callerId, resource);
        }

        // 其他权限类型默认放行 / Other permission types default to allowed
        return permissionType switch
        {
            PermissionType.Function => PermissionResult.Allowed,
            PermissionType.DataAccess => PermissionResult.Allowed,
            _ => PermissionResult.AskUser
        };
    }

    #region Network Rules (dpf.txt lines 9-621)

    private PermissionResult EvaluateNetwork(string resourcePath)
    {
        // 不包含协议名（无冒号），无法判断来源，询问用户
        // No protocol scheme (no colon), cannot determine origin, ask user
        if (!resourcePath.Contains(':'))
        {
            return PermissionResult.AskUser;
        }

        string host;
        if (Uri.TryCreate(resourcePath, UriKind.Absolute, out var uri))
        {
            host = uri.Host;
        }
        else
        {
            host = resourcePath.Trim().Split(':')[0];
        }

        // 本地回环地址放行（localhost / 127.0.0.1 / ::1）
        // Allow loopback addresses
        if (string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "127.0.0.1", StringComparison.Ordinal)
            || string.Equals(host, "::1", StringComparison.Ordinal))
        {
            return PermissionResult.Allowed;
        }

        // 内网 IP 地址段匹配（先验证是否为合法 IPv4 地址）
        // Private IP address range matching (validate IPv4 format first)
        if (IPAddress.TryParse(host, out var ipAddr)
            && ipAddr.AddressFamily == AddressFamily.InterNetwork)
        {
            var bytes = ipAddr.GetAddressBytes();

            // 内网C类地址放行（192.168.0.0/16）
            if (bytes[0] == 192 && bytes[1] == 168)
            {
                return PermissionResult.Allowed;
            }

            // 内网A类地址放行（10.0.0.0/8）
            if (bytes[0] == 10)
            {
                return PermissionResult.Allowed;
            }

            // 内网B类地址选择性放行（172.16.0.0/12，即 172.16.* ~ 172.31.*）
            if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
            {
                return PermissionResult.AskUser;
            }
        }

        // 允许访问的外部域名白名单 / Allowed external domain whitelist
        // 谷歌 / 必应 / 腾讯系 / 搜狗 / DuckDuckGo / Yandex / 微信 / 阿里系
        if (host.EndsWith(".google.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "google.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".bing.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "bing.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".qq.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "qq.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".tencent.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "tencent.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".weixin.qq.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".sogou.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "sogou.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".duckduckgo.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "duckduckgo.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".yandex.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "yandex.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".yandex.ru", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "yandex.ru", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".wechat.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "wechat.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".alibaba.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "alibaba.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".alibabacloud.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "alibabacloud.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".aliyun.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "aliyun.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".1688.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "1688.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".alipay.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "alipay.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".tmall.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "tmall.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".dingtalk.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "dingtalk.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".taobao.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "taobao.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 哔哩哔哩 / niconico / Acfun / 抖音 / TikTok / 快手 / 小红书
        if (host.EndsWith(".bilibili.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "bilibili.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".bilivideo.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "bilivideo.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".nicovideo.jp", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "nicovideo.jp", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".niconico.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "niconico.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".acfun.cn", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "acfun.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".douyin.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "douyin.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".tiktok.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "tiktok.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".kuaishou.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "kuaishou.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".xiaohongshu.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "xiaohongshu.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // AI 服务 / AI Services
        // OpenAI / Anthropic / HuggingFace / Ollama / 通义千问 / Kimi / 豆包 / 剪映 / Trae IDE
        if (host.EndsWith(".openai.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "openai.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".anthropic.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "anthropic.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".huggingface.co", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "huggingface.co", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".ollama.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "ollama.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".tongyi.aliyun.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".qianwen.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "qianwen.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".moonshot.cn", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "moonshot.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".kimi.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "kimi.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".doubao.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "doubao.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".capcut.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "capcut.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".jianying.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "jianying.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".trae.cn", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "trae.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".trae.ai", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "trae.ai", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 钓鱼/仿冒网站黑名单（关键词模糊匹配，规则宽松以覆盖更多近似网站）
        // Phishing & fake sites blacklist (fuzzy keyword match, loose rules)
        var hostLower = host.ToLowerInvariant();

        // AI 仿冒站 / AI phishing sites
        if (hostLower.Contains("chatgpt")
            || hostLower.Contains("openai")
            || hostLower.Contains("deepseek")
            || hostLower.Contains("sora")
            || hostLower.Contains("claude")
            || hostLower.Contains("luma-ai")
            || hostLower.Contains("kling-ai")
            || hostLower.Contains("openclaw"))
        {
            return PermissionResult.Denied;
        }

        // 恶意 AI 工具 / Malicious AI tools
        if (hostLower.Contains("wormgpt")
            || hostLower.Contains("darkgpt")
            || hostLower.Contains("fraudgpt")
            || hostLower.Contains("ghostgpt")
            || hostLower.Contains("oniongpt")
            || hostLower.Contains("evilgpt")
            || hostLower.Contains("hackgpt")
            || hostLower.Contains("poisongpt"))
        {
            return PermissionResult.Denied;
        }

        // 对抗性 AI / 提示词越狱 / LLM 攻击站点
        // Adversarial AI / prompt jailbreak / LLM hacking sites
        if (hostLower.Contains("adversarial-ai")
            || hostLower.Contains("prompt-jailbreak")
            || hostLower.Contains("jailbreak-ai")
            || hostLower.Contains("llm-hacking")
            || hostLower.Contains("llm-hack")
            || hostLower.Contains("llm-exploit"))
        {
            return PermissionResult.Denied;
        }

        // AI 内容农场 / AI 垃圾内容 / AI content farm & slop
        if (hostLower.Contains("ai-content-farm")
            || hostLower.Contains("ai-slop")
            || hostLower.Contains("auto-ai-write")
            || hostLower.Contains("mass-ai-article")
            || hostLower.Contains("ai-article-spam")
            || hostLower.Contains("ai-spam")
            || hostLower.Contains("ai-generator-free")
            || hostLower.Contains("ai-essay-mill"))
        {
            return PermissionResult.Denied;
        }

        // AI 数据黑市 / API 密钥黑市 / LLM 权重贩卖
        // AI data black market / API key market / LLM weight seller
        if (hostLower.Contains("ai-data-blackmarket")
            || hostLower.Contains("api-key-market")
            || hostLower.Contains("api-key-free")
            || hostLower.Contains("llm-weight-seller")
            || hostLower.Contains("llm-weight-free")
            || hostLower.Contains("model-pirate"))
        {
            return PermissionResult.Denied;
        }

        // AI 仿冒/诈骗通用关键词 / AI fake & scam generic keywords
        if (hostLower.Contains("ai-official-fake")
            || hostLower.Contains("ai-fake")
            || hostLower.Contains("ai-vip")
            || hostLower.Contains("ai-zh")
            || hostLower.Contains("ai-cn")
            || hostLower.Contains("ai-free-vip")
            || hostLower.Contains("ai-premium-crack"))
        {
            return PermissionResult.Denied;
        }

        // 其他黑名单站点 / Other blacklisted sites
        // sakura-cat: 不应被AI访问的网站 / 4399: 游戏内混杂病毒
        if (hostLower.StartsWith("sakura-cat")
            || hostLower.Contains("4399"))
        {
            return PermissionResult.Denied;
        }

        // 证券交易平台（需询问用户）/ Securities trading platforms (ask user)
        // 华泰证券 / 国泰君安 / 中信证券 / 招商证券 / 广发证券 / 海通证券 / 申万宏源 / 东方证券 / 国信证券 / 兴业证券
        if (host.EndsWith(".htsc.com.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".gtja.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".citics.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".cmschina.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".gf.com.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".htsec.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".swhysc.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".dfzq.com.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".guosen.com.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".xyzq.com.cn", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.AskUser;
        }

        // 第三方交易平台（需询问用户）/ Third-party trading platforms (ask user)
        // 同花顺 / 东方财富 / 通达信
        if (host.EndsWith(".10jqka.com.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".eastmoney.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".tdx.com.cn", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.AskUser;
        }

        // 证券交易所（仅行情）/ Stock exchanges (quotes only)
        // 上海证券交易所 / 深圳证券交易所 / 巨潮资讯网
        if (host.EndsWith(".sse.com.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".szse.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".cninfo.com.cn", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 财经资讯（仅行情）/ Financial news (quotes only)
        // 金融界 / 证券之星 / 和讯网
        if (host.EndsWith(".jrj.com.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".stockstar.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".hexun.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 投资社区（仅资讯）/ Investment community (news only)
        // 雪球 / 财联社 / 开盘啦 / 淘股吧
        if (host.EndsWith(".xueqiu.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".cls.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".kaipanla.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".taoguba.com.cn", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 开发者服务 / Developer services
        // GitHub / Gitee / StackOverflow / npm / NuGet / PyPI / 微软
        if (host.EndsWith(".github.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "github.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".githubusercontent.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "githubusercontent.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".gitee.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "gitee.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".stackoverflow.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "stackoverflow.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".npmjs.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "npmjs.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".nuget.org", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "nuget.org", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".pypi.org", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "pypi.org", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".microsoft.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "microsoft.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 游戏引擎 / Game engines
        // Unity / 虚幻引擎 / Epic Games / Fab资源商店
        if (host.EndsWith(".unity.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "unity.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".unity3d.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "unity3d.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".unrealengine.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "unrealengine.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".epicgames.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "epicgames.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".fab.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "fab.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 游戏平台 / Game platforms — Steam 需询问用户
        if (host.EndsWith(".steampowered.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "steampowered.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.AskUser;
        }

        // 世嘉(SEGA，日本) / SEGA (Japan)
        if (host.EndsWith(".sega.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "sega.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 云服务与开发工具 / Cloud services & dev tools
        // 亚马逊 / AWS / Kiro IDE / CodeBuddy IDE / JetBrains / 纯光工作室 / W3School中文
        if (host.EndsWith(".amazon.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "amazon.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".amazonaws.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "amazonaws.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".kiro.dev", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "kiro.dev", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".codebuddy.cn", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "codebuddy.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".jetbrains.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "jetbrains.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".purelight.net.cn", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "purelight.net.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".w3school.com.cn", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "w3school.com.cn", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 社交/资讯（中国大陆）/ Social & news (mainland China)
        // 微博 / 知乎 / 网易 / 新浪 / 凤凰网 / 新华社 / 中央电视台
        if (host.EndsWith(".weibo.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "weibo.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".zhihu.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "zhihu.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".163.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "163.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".sina.com.cn", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".ifeng.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "ifeng.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".xinhuanet.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "xinhuanet.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".cctv.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "cctv.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 中国台湾媒体 / Taiwan media
        // 中天新闻网(中天电视台) / CTI News (CTI TV)
        if (host.EndsWith(".ctinews.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "ctinews.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 三立新闻网(三立民视) — 需询问用户 / SET News (SET TV) — ask user
        if (host.EndsWith(".setn.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "setn.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.AskUser;
        }

        // 网络内容防护机构(中国台湾，有拦截风险) — 禁止
        // WIN (Taiwan, risk of being blocked) — deny
        if (host.EndsWith(".win.org.tw", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "win.org.tw", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Denied;
        }

        // 日本媒体 / Japanese media
        // NHK(日本放送协会) / NHK (Japan Broadcasting Corporation)
        if (host.EndsWith(".nhk.or.jp", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "nhk.or.jp", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 俄罗斯媒体 / Russian media
        // 俄罗斯卫星通讯社(各国站点) / Sputnik News (all regions)
        if (hostLower.Contains("sputniknews"))
        {
            return PermissionResult.Allowed;
        }

        // 韩国媒体 / Korean media
        // KBS / MBC / SBS / EBS
        if (host.EndsWith(".kbs.co.kr", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "kbs.co.kr", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".imbc.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "imbc.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".sbs.co.kr", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "sbs.co.kr", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".ebs.co.kr", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "ebs.co.kr", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 朝鲜媒体 / DPRK media
        // 我的国家 / 劳动新闻 / 青年前卫 / 朝鲜之声 / 平壤时报 / 朝鲜新报
        if (host.EndsWith(".naenara.com.kp", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".rodong.rep.kp", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".youth.rep.kp", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".vok.rep.kp", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".pyongyangtimes.com.kp", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".chosonsinbo.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "chosonsinbo.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 各国政府网站（通配 .gov 域名）/ Government websites (wildcard .gov domains)
        // 覆盖：.gov.cn, .gov.tw, .gov.uk, .gov.jp, .gov, .go.jp, .go.kr 等
        if (hostLower.Contains(".gov")
            || hostLower.EndsWith(".go.jp")
            || hostLower.EndsWith(".go.kr"))
        {
            return PermissionResult.Allowed;
        }

        // 海外社交/直播（需询问用户）/ Overseas social & streaming (ask user)
        // Twitch / Facebook / X / Gmail / Instagram / lit.link
        if (host.EndsWith(".twitch.tv", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "twitch.tv", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".facebook.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "facebook.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".x.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "x.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".gmail.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "gmail.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".instagram.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "instagram.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".lit.link", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "lit.link", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.AskUser;
        }

        // WhatsApp(Meta) — 允许 / Allow
        if (host.EndsWith(".whatsapp.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "whatsapp.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // Threads(Meta) — 禁止 / Deny
        if (host.EndsWith(".threads.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "threads.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Denied;
        }

        // 视频/媒体 / Video & media
        // YouTube / 爱奇艺 / 优酷
        if (host.EndsWith(".youtube.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "youtube.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".iqiyi.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "iqiyi.com", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".youku.com", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "youku.com", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 地图 / Maps
        // 开放街道地图 / OpenStreetMap (OSM)
        if (host.EndsWith(".openstreetmap.org", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "openstreetmap.org", StringComparison.OrdinalIgnoreCase))
        {
            return PermissionResult.Allowed;
        }

        // 百科 / Encyclopedia
        // 维基百科 / MediaWiki / 知识共享(CC)
        if (host.EndsWith(".wikipedia.org", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "wikipedia.org", StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".mediawiki.org", StringComparison.OrdinalIgnoreCase)
            || string.Equals(host, "mediawiki.org", StringComparison.OrdinalIgnoreCase)
            || hostLower.Contains("creativecommons"))
        {
            return PermissionResult.Allowed;
        }

        // 未匹配的网络访问，询问用户 / Unmatched network access, ask user
        return PermissionResult.AskUser;
    }

    #endregion

    #region Command Line Rules (dpf.txt lines 623-874)

    private PermissionResult EvaluateCommandLine(Guid callerId, string resourcePath)
    {
        var cmd = resourcePath.Trim();

        // 检测管道符和多命令分隔符，拆分逐条验证
        // Detect pipe and multi-command separators, split and verify each
        // 通用 / Common: |, ||, &&
        // Windows: &
        // Linux/macOS: ;
        string[] separators;
        if (OperatingSystem.IsWindows())
        {
            separators = ["||", "&&", "|", "&"];
        }
        else
        {
            separators = ["||", "&&", "|", ";"];
        }

        // 检查是否包含任何分隔符 / Check if contains any separator
        bool hasMultipleCommands = false;
        foreach (var sep in separators)
        {
            if (cmd.Contains(sep))
            {
                hasMultipleCommands = true;
                break;
            }
        }

        if (hasMultipleCommands)
        {
            // 拆分为多条命令 / Split into multiple commands
            var parts = cmd.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var hasAskUser = false;

            foreach (var part in parts)
            {
                var subResult = Evaluate(callerId, PermissionType.CommandLine, part.Trim());
                if (subResult == PermissionResult.Denied)
                {
                    // 有任何一条被禁止，直接禁止整条命令
                    // If any sub-command is denied, deny the entire command
                    return PermissionResult.Denied;
                }
                if (subResult == PermissionResult.AskUser)
                {
                    hasAskUser = true;
                }
            }

            // 没有禁止的，但有需要询问的，询问用户
            if (hasAskUser)
            {
                return PermissionResult.AskUser;
            }

            // 全部允许 / All allowed
            return PermissionResult.Allowed;
        }

        var cmdLower = cmd.ToLowerInvariant();

        if (OperatingSystem.IsWindows())
        {
            // Windows 允许：只读/查询类命令 / Windows allow: read-only / query commands
            // dir / tree / tasklist / ipconfig / ping / tracert / systeminfo / whoami / set / path / sc query / findstr
            if (cmdLower.StartsWith("dir ")
                || cmdLower.StartsWith("tree ")
                || string.Equals(cmdLower, "tasklist")
                || cmdLower.StartsWith("tasklist ")
                || string.Equals(cmdLower, "ipconfig /all")
                || cmdLower.StartsWith("ipconfig ")
                || cmdLower.StartsWith("ping ")
                || cmdLower.StartsWith("tracert ")
                || string.Equals(cmdLower, "systeminfo")
                || string.Equals(cmdLower, "whoami")
                || string.Equals(cmdLower, "set")
                || cmdLower.StartsWith("set ")
                || string.Equals(cmdLower, "path")
                || cmdLower.StartsWith("sc query ")
                || cmdLower.StartsWith("findstr "))
            {
                return PermissionResult.Allowed;
            }

            // Windows 禁止：危险/破坏性命令 / Windows deny: dangerous / destructive commands
            // del / rmdir / format / diskpart / reg delete
            if (cmdLower.StartsWith("del ")
                || cmdLower.StartsWith("rmdir ")
                || cmdLower.StartsWith("format ")
                || cmdLower.StartsWith("diskpart")
                || cmdLower.StartsWith("reg delete "))
            {
                return PermissionResult.Denied;
            }
        }
        else if (OperatingSystem.IsLinux())
        {
            // Linux 允许：只读/查询类命令 / Linux allow: read-only / query commands
            // ls / tree / ps / top / ifconfig / ip / ping / traceroute / uname / whoami / env / cat / grep / find / df / du / systemctl status
            if (cmdLower.StartsWith("ls ")
                || string.Equals(cmdLower, "ls")
                || cmdLower.StartsWith("tree ")
                || string.Equals(cmdLower, "tree")
                || cmdLower.StartsWith("ps ")
                || string.Equals(cmdLower, "ps")
                || string.Equals(cmdLower, "top")
                || cmdLower.StartsWith("ifconfig ")
                || string.Equals(cmdLower, "ifconfig")
                || cmdLower.StartsWith("ip ")
                || cmdLower.StartsWith("ping ")
                || cmdLower.StartsWith("traceroute ")
                || cmdLower.StartsWith("uname ")
                || string.Equals(cmdLower, "uname")
                || string.Equals(cmdLower, "whoami")
                || string.Equals(cmdLower, "env")
                || cmdLower.StartsWith("cat ")
                || cmdLower.StartsWith("grep ")
                || cmdLower.StartsWith("find ")
                || cmdLower.StartsWith("df ")
                || string.Equals(cmdLower, "df")
                || cmdLower.StartsWith("du ")
                || cmdLower.StartsWith("systemctl status "))
            {
                return PermissionResult.Allowed;
            }

            // Linux 禁止：危险/破坏性命令 / Linux deny: dangerous / destructive commands
            // rm / rmdir / mkfs / fdisk / dd / chmod / chown / chgrp
            if (cmdLower.StartsWith("rm ")
                || cmdLower.StartsWith("rmdir ")
                || cmdLower.StartsWith("mkfs ")
                || cmdLower.StartsWith("fdisk ")
                || cmdLower.StartsWith("dd ")
                || cmdLower.StartsWith("chmod ")
                || cmdLower.StartsWith("chown ")
                || cmdLower.StartsWith("chgrp "))
            {
                return PermissionResult.Denied;
            }
        }
        else if (OperatingSystem.IsMacOS())
        {
            // macOS 允许：只读/查询类命令 / macOS allow: read-only / query commands
            // ls / tree / ps / top / ifconfig / ping / traceroute / system_profiler / sw_vers / whoami / env / cat / grep / find / df / du / launchctl list
            if (cmdLower.StartsWith("ls ")
                || string.Equals(cmdLower, "ls")
                || cmdLower.StartsWith("tree ")
                || string.Equals(cmdLower, "tree")
                || cmdLower.StartsWith("ps ")
                || string.Equals(cmdLower, "ps")
                || string.Equals(cmdLower, "top")
                || cmdLower.StartsWith("ifconfig ")
                || string.Equals(cmdLower, "ifconfig")
                || cmdLower.StartsWith("ping ")
                || cmdLower.StartsWith("traceroute ")
                || cmdLower.StartsWith("system_profiler ")
                || string.Equals(cmdLower, "system_profiler")
                || string.Equals(cmdLower, "sw_vers")
                || string.Equals(cmdLower, "whoami")
                || string.Equals(cmdLower, "env")
                || cmdLower.StartsWith("cat ")
                || cmdLower.StartsWith("grep ")
                || cmdLower.StartsWith("find ")
                || cmdLower.StartsWith("df ")
                || string.Equals(cmdLower, "df")
                || cmdLower.StartsWith("du ")
                || cmdLower.StartsWith("launchctl list "))
            {
                return PermissionResult.Allowed;
            }

            // macOS 禁止：危险/破坏性命令 / macOS deny: dangerous / destructive commands
            // rm / rmdir / diskutil eraseDisk / dd / chmod / chown / chgrp
            if (cmdLower.StartsWith("rm ")
                || cmdLower.StartsWith("rmdir ")
                || cmdLower.StartsWith("diskutil erasedisk ")
                || cmdLower.StartsWith("dd ")
                || cmdLower.StartsWith("chmod ")
                || cmdLower.StartsWith("chown ")
                || cmdLower.StartsWith("chgrp "))
            {
                return PermissionResult.Denied;
            }
        }

        // 未匹配的命令，询问用户 / Unmatched commands, ask user
        return PermissionResult.AskUser;
    }

    #endregion

    #region File Access Rules (dpf.txt lines 876-1008)

    private PermissionResult EvaluateFileAccess(Guid callerId, string resourcePath)
    {
        // 最高优先级：危险文件扩展名直接拒绝，无论目录是否允许
        // Highest priority: deny dangerous file extensions regardless of directory permissions
        // .pfx: 证书私钥文件 / .key: 密钥文件 / .bat: Windows 批处理脚本 / .sh: Shell 脚本 / .reg: Windows 注册表文件
        var ext = Path.GetExtension(resourcePath).ToLowerInvariant();
        if (ext is ".pfx" or ".key" or ".bat" or ".sh" or ".reg")
        {
            return PermissionResult.Denied;
        }

        string filePath;
        try
        {
            filePath = Path.GetFullPath(resourcePath);
        }
        catch
        {
            // 无法解析为绝对路径，询问用户 / Cannot resolve to absolute path, ask user
            return PermissionResult.AskUser;
        }
        var filePathLower = filePath.ToLowerInvariant();

        // 禁止：当前程序集目录 / Deny: current assembly directory
        var assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)?.ToLowerInvariant();
        if (!string.IsNullOrEmpty(assemblyDir) && filePathLower.StartsWith(assemblyDir))
        {
            return PermissionResult.Denied;
        }

        // 禁止：应用数据目录（从构造函数获取）
        // Deny: application data directory (from constructor)
        // 但允许：自己的 Temp 目录 / But allow: own Temp directory
        if (!string.IsNullOrEmpty(_appDataDirectory))
        {
            // 允许：自己的 Temp 目录（dataDirectory/SiliconManager/{callerId}/Temp）
            // Allow: own Temp directory
            var ownTempDir = Path.Combine(_appDataDirectory, "SiliconManager", callerId.ToString(), "Temp").ToLowerInvariant();
            if (filePathLower.StartsWith(ownTempDir))
            {
                return PermissionResult.Allowed;
            }

            // 禁止：数据目录其他路径（包括其他硅基人的目录）
            // Deny: other paths in data directory (including other silicon life's directories)
            if (filePathLower.StartsWith(_appDataDirectory))
            {
                return PermissionResult.Denied;
            }
        }

        // 允许：用户常用文件夹 / Allow: common user folders
        // 桌面 / 下载 / 文档 / 图片 / 音乐 / 视频
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToLowerInvariant();
        var desktopPath = Path.Combine(home, "Desktop").ToLowerInvariant();
        var downloadsPath = Path.Combine(home, "Downloads").ToLowerInvariant();
        var documentsPath = Path.Combine(home, "Documents").ToLowerInvariant();
        var picturesPath = Path.Combine(home, "Pictures").ToLowerInvariant();
        var musicPath = Path.Combine(home, "Music").ToLowerInvariant();
        var videosPath = Path.Combine(home, OperatingSystem.IsMacOS() ? "Movies" : "Videos").ToLowerInvariant();

        if (filePathLower.StartsWith(desktopPath)
            || filePathLower.StartsWith(downloadsPath)
            || filePathLower.StartsWith(documentsPath)
            || filePathLower.StartsWith(picturesPath)
            || filePathLower.StartsWith(musicPath)
            || filePathLower.StartsWith(videosPath))
        {
            return PermissionResult.Allowed;
        }

        // 允许：公用用户文件夹 / Allow: public/common user folders
        // Windows: C:\Users\Public\Desktop, Documents, Downloads, Music, Pictures, Videos
        // macOS: /Users/Shared
        // Linux: no standard public folders, skip
        if (OperatingSystem.IsWindows())
        {
            string publicDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory).ToLowerInvariant();
            string publicDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments).ToLowerInvariant();
            string publicMusicPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic).ToLowerInvariant();
            string publicPicturesPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures).ToLowerInvariant();
            string publicVideosPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos).ToLowerInvariant();
            string publicRootPath = Path.GetDirectoryName(publicDesktopPath)!.ToLowerInvariant();
            string publicDownloadsPath = Path.Combine(publicRootPath, "Downloads").ToLowerInvariant();

            if (filePathLower.StartsWith(publicDesktopPath)
                || filePathLower.StartsWith(publicDocumentsPath)
                || filePathLower.StartsWith(publicMusicPath)
                || filePathLower.StartsWith(publicPicturesPath)
                || filePathLower.StartsWith(publicVideosPath)
                || filePathLower.StartsWith(publicDownloadsPath))
            {
                return PermissionResult.Allowed;
            }
        }
        else if (OperatingSystem.IsMacOS())
        {
            string sharedPath = "/users/shared";
            if (filePathLower.StartsWith(sharedPath))
            {
                return PermissionResult.Allowed;
            }
        }

        if (OperatingSystem.IsWindows())
        {
            // Windows 禁止：系统关键目录（不一定在C盘）
            // Windows deny: critical system directories (not necessarily on C drive)
            var winDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows).ToLowerInvariant();
            var sysDir = Environment.GetFolderPath(Environment.SpecialFolder.System).ToLowerInvariant();
            var progDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).ToLowerInvariant();
            var progX86Dir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86).ToLowerInvariant();

            if (filePathLower.StartsWith(winDir)
                || filePathLower.StartsWith(sysDir)
                || filePathLower.StartsWith(progDir)
                || filePathLower.StartsWith(progX86Dir))
            {
                return PermissionResult.Denied;
            }
        }
        else if (OperatingSystem.IsLinux())
        {
            // Linux 禁止：系统关键目录 / Linux deny: critical system directories
            // /etc /boot /sbin
            if (filePathLower.StartsWith("/etc/")
                || filePathLower.StartsWith("/boot/")
                || filePathLower.StartsWith("/sbin/"))
            {
                return PermissionResult.Denied;
            }
        }
        else if (OperatingSystem.IsMacOS())
        {
            // macOS 禁止：系统关键目录 / macOS deny: critical system directories
            // /System /Library /private/etc
            if (filePathLower.StartsWith("/system/")
                || filePathLower.StartsWith("/library/")
                || filePathLower.StartsWith("/private/etc/"))
            {
                return PermissionResult.Denied;
            }
        }

        // 未匹配的路径，询问用户 / Unmatched paths, ask user
        return PermissionResult.AskUser;
    }

    #endregion
}
