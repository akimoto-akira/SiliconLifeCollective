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

using System.Text;
using SiliconLife.Collective;

namespace SiliconLife.Default.Web.Models;

public class PermissionViewModel : ViewModelBase
{
    public Guid BeingId { get; set; }
    public string CurrentCallbackCode { get; set; } = string.Empty;

    public string GenerateDefaultTemplateCode()
    {
        var loc = LocalizationManager.Instance.GetLocalization(Config.Instance?.Data?.Language ?? Language.ZhCN);
        var defaultLoc = loc as DefaultLocalizationBase;

        var sb = new StringBuilder();

        GenerateHeader(sb, defaultLoc);
        GenerateConstructor(sb, defaultLoc);
        GenerateEvaluateMethod(sb, defaultLoc);
        GenerateEvaluateNetworkMethod(sb, defaultLoc);
        GenerateEvaluateCommandLineMethod(sb, defaultLoc);
        GenerateEvaluateFileAccessMethod(sb, defaultLoc);
        GenerateFooter(sb);

        return sb.ToString();
    }

    private string GetCallbackClassName()
    {
        return $"PermissionCallback_{BeingId:N}";
    }

    private static string Comment(DefaultLocalizationBase? loc, string key)
        => loc?.GetPermissionRuleComment(key) ?? key;

    private void GenerateHeader(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("using System.Net;");
        sb.AppendLine("using System.Net.Sockets;");
        sb.AppendLine("using SiliconLife.Collective;");
        sb.AppendLine();
        sb.AppendLine("namespace SiliconLife.Default;");
        sb.AppendLine();

        if (loc != null)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"/// {loc.PermissionCallbackClassSummary}");
            sb.AppendLine($"/// {loc.PermissionCallbackClassSummary2}");
            sb.AppendLine("/// </summary>");
        }

        sb.AppendLine($"public class {GetCallbackClassName()} : IPermissionCallback");
        sb.AppendLine("{");
        sb.AppendLine("    private readonly string _appDataDirectory;");
        sb.AppendLine();
    }

    private void GenerateConstructor(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        if (loc != null)
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {loc.PermissionCallbackConstructorSummary}");
            sb.AppendLine($"    /// {loc.PermissionCallbackConstructorSummary2}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    /// <param name=\"appDataDirectory\">{loc.PermissionCallbackConstructorParam}</param>");
        }

        sb.AppendLine($"    public {GetCallbackClassName()}(string appDataDirectory)");
        sb.AppendLine("    {");
        sb.AppendLine("        _appDataDirectory = appDataDirectory?.ToLowerInvariant() ?? string.Empty;");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private void GenerateEvaluateMethod(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        if (loc != null)
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {loc.PermissionCallbackEvaluateSummary}");
            sb.AppendLine("    /// </summary>");
        }

        sb.AppendLine("    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)");
        sb.AppendLine("    {");
        sb.AppendLine($"        // {Comment(loc, "NetRuleNetworkAccess")}");
        sb.AppendLine("        if (permissionType == PermissionType.NetworkAccess && !string.IsNullOrWhiteSpace(resource))");
        sb.AppendLine("        {");
        sb.AppendLine("            return EvaluateNetwork(resource);");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine($"        // {Comment(loc, "NetRuleCommandLine")}");
        sb.AppendLine("        if (permissionType == PermissionType.CommandLine && !string.IsNullOrWhiteSpace(resource))");
        sb.AppendLine("        {");
        sb.AppendLine("            return EvaluateCommandLine(callerId, resource);");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine($"        // {Comment(loc, "NetRuleFileAccess")}");
        sb.AppendLine("        if (permissionType == PermissionType.FileAccess && !string.IsNullOrWhiteSpace(resource))");
        sb.AppendLine("        {");
        sb.AppendLine("            return EvaluateFileAccess(callerId, resource);");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine($"        // {loc?.PermissionRuleOtherTypesDefault ?? "Other permission types default to allowed / 其他权限类型默认放行"}");
        sb.AppendLine("        return permissionType switch");
        sb.AppendLine("        {");
        sb.AppendLine("            PermissionType.Function => PermissionResult.Allowed,");
        sb.AppendLine("            PermissionType.DataAccess => PermissionResult.Allowed,");
        sb.AppendLine("            _ => PermissionResult.AskUser");
        sb.AppendLine("        };");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private void GenerateEvaluateNetworkMethod(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("    private PermissionResult EvaluateNetwork(string resourcePath)");
        sb.AppendLine("    {");

        sb.AppendLine($"        // {Comment(loc, "NetRuleNoProtocol")}");
        sb.AppendLine("        if (!resourcePath.Contains(':'))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.AskUser;");
        sb.AppendLine("        }");
        sb.AppendLine();

        sb.AppendLine("        string host;");
        sb.AppendLine("        if (Uri.TryCreate(resourcePath, UriKind.Absolute, out var uri))");
        sb.AppendLine("        {");
        sb.AppendLine("            host = uri.Host;");
        sb.AppendLine("        }");
        sb.AppendLine("        else");
        sb.AppendLine("        {");
        sb.AppendLine("            host = resourcePath.Trim().Split(':')[0];");
        sb.AppendLine("        }");
        sb.AppendLine();

        sb.AppendLine($"        // {Comment(loc, "NetRuleLoopback")}");
        sb.AppendLine("        var allowedLoopback = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("        {");
        sb.AppendLine("            \"localhost\", \"127.0.0.1\", \"::1\"");
        sb.AppendLine("        };");
        sb.AppendLine("        if (allowedLoopback.Contains(host))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Allowed;");
        sb.AppendLine("        }");
        sb.AppendLine();

        GeneratePrivateIpBlock(sb, loc);
        GenerateNetworkDeniedContainsBlock(sb, loc);
        GenerateNetworkAllowedEndsWithBlock(sb, loc);
        GenerateNetworkAllowedExactBlock(sb, loc);
        GenerateNetworkAllowedSpecialBlock(sb, loc);
        GenerateNetworkDeniedSpecialBlock(sb, loc);
        GenerateNetworkAskUserBlock(sb, loc);

        sb.AppendLine($"        // {Comment(loc, "NetRuleUnmatched")}");
        sb.AppendLine("        return PermissionResult.AskUser;");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private void GeneratePrivateIpBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine($"        // {Comment(loc, "NetRulePrivateIPMatch")}");
        sb.AppendLine("        if (IPAddress.TryParse(host, out var ipAddr)");
        sb.AppendLine("            && ipAddr.AddressFamily == AddressFamily.InterNetwork)");
        sb.AppendLine("        {");
        sb.AppendLine("            var bytes = ipAddr.GetAddressBytes();");
        sb.AppendLine();
        sb.AppendLine($"            // {Comment(loc, "NetRulePrivateC")}");
        sb.AppendLine("            if (bytes[0] == 192 && bytes[1] == 168)");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Allowed;");
        sb.AppendLine("            }");
        sb.AppendLine();
        sb.AppendLine($"            // {Comment(loc, "NetRulePrivateA")}");
        sb.AppendLine("            if (bytes[0] == 10)");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Allowed;");
        sb.AppendLine("            }");
        sb.AppendLine();
        sb.AppendLine($"            // {Comment(loc, "NetRulePrivateB")}");
        sb.AppendLine("            if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.AskUser;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateNetworkDeniedContainsBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("        var hostLower = host.ToLowerInvariant();");
        sb.AppendLine();
        sb.AppendLine("        // ===== Denied: Contains =====");
        sb.AppendLine("        var deniedContains = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("        {");

        sb.AppendLine($"            // {Comment(loc, "NetRulePhishingAI")}");
        sb.AppendLine("            \"chatgpt\", \"openai\", \"deepseek\", \"sora\", \"claude\", \"luma-ai\", \"kling-ai\", \"openclaw\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleMaliciousAI")}");
        sb.AppendLine("            \"wormgpt\", \"darkgpt\", \"fraudgpt\", \"ghostgpt\", \"oniongpt\", \"evilgpt\", \"hackgpt\", \"poisongpt\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleAdversarialAI")}");
        sb.AppendLine("            \"adversarial-ai\", \"prompt-jailbreak\", \"jailbreak-ai\", \"llm-hacking\", \"llm-hack\", \"llm-exploit\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleAIContentFarm")}");
        sb.AppendLine("            \"ai-content-farm\", \"ai-slop\", \"auto-ai-write\", \"mass-ai-article\",");
        sb.AppendLine("            \"ai-article-spam\", \"ai-spam\", \"ai-generator-free\", \"ai-essay-mill\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleAIBlackMarket")}");
        sb.AppendLine("            \"ai-data-blackmarket\", \"api-key-market\", \"api-key-free\",");
        sb.AppendLine("            \"llm-weight-seller\", \"llm-weight-free\", \"model-pirate\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleAIFakeScam")}");
        sb.AppendLine("            \"ai-official-fake\", \"ai-fake\", \"ai-vip\", \"ai-zh\", \"ai-cn\", \"ai-free-vip\", \"ai-premium-crack\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleOtherBlacklist")}");
        sb.AppendLine("            \"4399\"");

        sb.AppendLine("        };");
        sb.AppendLine();
        sb.AppendLine("        if (deniedContains.Any(hostLower.Contains))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Denied;");
        sb.AppendLine("        }");
        sb.AppendLine();

        sb.AppendLine("        // sakura-cat StartsWith");
        sb.AppendLine("        if (hostLower.StartsWith(\"sakura-cat\"))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Denied;");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateNetworkAllowedEndsWithBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("        // ===== Allowed: EndsWith =====");
        sb.AppendLine("        var allowedEndsWith = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("        {");

        sb.AppendLine($"            // {Comment(loc, "NetRuleDomainWhitelist1")}");
        sb.AppendLine("            \".google.com\", \".bing.com\", \".qq.com\", \".tencent.com\", \".weixin.qq.com\",");
        sb.AppendLine("            \".sogou.com\", \".duckduckgo.com\", \".yandex.com\", \".yandex.ru\", \".wechat.com\",");
        sb.AppendLine("            \".alibaba.com\", \".alibabacloud.com\", \".aliyun.com\", \".1688.com\", \".alipay.com\",");
        sb.AppendLine("            \".tmall.com\", \".dingtalk.com\", \".taobao.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleVideoPlatforms")}");
        sb.AppendLine("            \".bilibili.com\", \".bilivideo.com\", \".nicovideo.jp\", \".niconico.com\",");
        sb.AppendLine("            \".acfun.cn\", \".douyin.com\", \".tiktok.com\", \".kuaishou.com\", \".xiaohongshu.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleAIServices")}");
        sb.AppendLine("            \".openai.com\", \".anthropic.com\", \".huggingface.co\", \".ollama.com\",");
        sb.AppendLine("            \".tongyi.aliyun.com\", \".qianwen.com\", \".moonshot.cn\", \".kimi.com\",");
        sb.AppendLine("            \".doubao.com\", \".capcut.com\", \".jianying.com\", \".trae.cn\", \".trae.ai\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleStockExchanges")}");
        sb.AppendLine("            \".sse.com.cn\", \".szse.cn\", \".cninfo.com.cn\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleFinancialNews")}");
        sb.AppendLine("            \".jrj.com.cn\", \".stockstar.com\", \".hexun.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleInvestCommunity")}");
        sb.AppendLine("            \".xueqiu.com\", \".cls.cn\", \".kaipanla.com\", \".taoguba.com.cn\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleDevServices")}");
        sb.AppendLine("            \".github.com\", \".githubusercontent.com\", \".gitee.com\",");
        sb.AppendLine("            \".stackoverflow.com\", \".npmjs.com\", \".nuget.org\", \".pypi.org\", \".microsoft.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleGameEngines")}");
        sb.AppendLine("            \".unity.com\", \".unity3d.com\", \".unrealengine.com\", \".epicgames.com\", \".fab.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleSEGA")}");
        sb.AppendLine("            \".sega.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleCloudServices")}");
        sb.AppendLine("            \".azure.com\", \".azurewebsites.net\", \".cloud.google.com\",");
        sb.AppendLine("            \".digitalocean.com\", \".heroku.com\", \".vercel.com\", \".netlify.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleDevDeployTools")}");
        sb.AppendLine("            \".gitlab.com\", \".bitbucket.org\", \".docker.com\", \".cloudflare.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleCloudDevTools")}");
        sb.AppendLine("            \".amazon.com\", \".amazonaws.com\", \".kiro.dev\", \".codebuddy.cn\",");
        sb.AppendLine("            \".jetbrains.com\", \".purelight.net.cn\", \".w3school.com.cn\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleChinaSocialNews")}");
        sb.AppendLine("            \".weibo.com\", \".zhihu.com\", \".163.com\", \".sina.com.cn\",");
        sb.AppendLine("            \".ifeng.com\", \".xinhuanet.com\", \".cctv.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleTaiwanMediaCTI")}");
        sb.AppendLine("            \".ctinews.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleJapanMedia")}");
        sb.AppendLine("            \".nhk.or.jp\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleKoreanMedia")}");
        sb.AppendLine("            \".kbs.co.kr\", \".imbc.com\", \".sbs.co.kr\", \".ebs.co.kr\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleDPRKMedia")}");
        sb.AppendLine("            \".naenara.com.kp\", \".rodong.rep.kp\", \".youth.rep.kp\",");
        sb.AppendLine("            \".vok.rep.kp\", \".pyongyangtimes.com.kp\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleGlobalSocialCollab")}");
        sb.AppendLine("            \".reddit.com\", \".discord.com\", \".discordapp.com\", \".slack.com\",");
        sb.AppendLine("            \".notion.so\", \".figma.com\", \".dropbox.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleWhatsApp")}");
        sb.AppendLine("            \".whatsapp.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleGlobalVideoMusic")}");
        sb.AppendLine("            \".spotify.com\", \".apple.com\", \".vimeo.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleVideoMedia")}");
        sb.AppendLine("            \".youtube.com\", \".iqiyi.com\", \".youku.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleMaps")}");
        sb.AppendLine("            \".openstreetmap.org\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleEncyclopedia")}");
        sb.AppendLine("            \".wikipedia.org\", \".mediawiki.org\"");

        sb.AppendLine("        };");
        sb.AppendLine();
        sb.AppendLine("        if (allowedEndsWith.Any(host.EndsWith))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Allowed;");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateNetworkAllowedExactBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("        // ===== Allowed: Exact =====");
        sb.AppendLine("        var allowedExact = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("        {");

        sb.AppendLine("            \"google.com\", \"bing.com\", \"qq.com\", \"tencent.com\", \"sogou.com\",");
        sb.AppendLine("            \"duckduckgo.com\", \"yandex.com\", \"yandex.ru\", \"wechat.com\",");
        sb.AppendLine("            \"alibaba.com\", \"alibabacloud.com\", \"aliyun.com\", \"1688.com\", \"alipay.com\",");
        sb.AppendLine("            \"tmall.com\", \"dingtalk.com\", \"taobao.com\",");
        sb.AppendLine("            \"bilibili.com\", \"bilivideo.com\", \"nicovideo.jp\", \"niconico.com\",");
        sb.AppendLine("            \"acfun.cn\", \"douyin.com\", \"tiktok.com\", \"kuaishou.com\", \"xiaohongshu.com\",");
        sb.AppendLine("            \"openai.com\", \"anthropic.com\", \"huggingface.co\", \"ollama.com\",");
        sb.AppendLine("            \"qianwen.com\", \"moonshot.cn\", \"kimi.com\", \"doubao.com\",");
        sb.AppendLine("            \"capcut.com\", \"jianying.com\", \"trae.cn\", \"trae.ai\",");
        sb.AppendLine("            \"github.com\", \"githubusercontent.com\", \"gitee.com\",");
        sb.AppendLine("            \"stackoverflow.com\", \"npmjs.com\", \"nuget.org\", \"pypi.org\", \"microsoft.com\",");
        sb.AppendLine("            \"unity.com\", \"unity3d.com\", \"unrealengine.com\", \"epicgames.com\", \"fab.com\",");
        sb.AppendLine("            \"sega.com\",");
        sb.AppendLine("            \"azure.com\", \"digitalocean.com\", \"heroku.com\", \"vercel.com\", \"netlify.com\",");
        sb.AppendLine("            \"gitlab.com\", \"bitbucket.org\", \"docker.com\", \"cloudflare.com\",");
        sb.AppendLine("            \"amazon.com\", \"amazonaws.com\", \"kiro.dev\", \"codebuddy.cn\",");
        sb.AppendLine("            \"jetbrains.com\", \"purelight.net.cn\", \"w3school.com.cn\",");
        sb.AppendLine("            \"weibo.com\", \"zhihu.com\", \"163.com\", \"ifeng.com\",");
        sb.AppendLine("            \"xinhuanet.com\", \"cctv.com\",");
        sb.AppendLine("            \"ctinews.com\",");
        sb.AppendLine("            \"nhk.or.jp\",");
        sb.AppendLine("            \"kbs.co.kr\", \"imbc.com\", \"sbs.co.kr\", \"ebs.co.kr\",");
        sb.AppendLine("            \"chosonsinbo.com\",");
        sb.AppendLine("            \"reddit.com\", \"discord.com\", \"discordapp.com\", \"slack.com\",");
        sb.AppendLine("            \"notion.so\", \"figma.com\", \"dropbox.com\",");
        sb.AppendLine("            \"whatsapp.com\",");
        sb.AppendLine("            \"spotify.com\", \"apple.com\", \"vimeo.com\",");
        sb.AppendLine("            \"youtube.com\", \"iqiyi.com\", \"youku.com\",");
        sb.AppendLine("            \"openstreetmap.org\",");
        sb.AppendLine("            \"wikipedia.org\", \"mediawiki.org\"");

        sb.AppendLine("        };");
        sb.AppendLine();
        sb.AppendLine("        if (allowedExact.Contains(host))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Allowed;");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateNetworkAllowedSpecialBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("        // ===== Allowed: Contains =====");
        sb.AppendLine($"        // {Comment(loc, "NetRuleRussianMedia")}");
        sb.AppendLine("        if (hostLower.Contains(\"sputniknews\"))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Allowed;");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine($"        // {Comment(loc, "NetRuleEncyclopedia")} - creativecommons");
        sb.AppendLine("        if (hostLower.Contains(\"creativecommons\"))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Allowed;");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine($"        // {Comment(loc, "NetRuleGovWebsites")}");
        sb.AppendLine("        if (hostLower.Contains(\".gov\") || hostLower.EndsWith(\".go.jp\") || hostLower.EndsWith(\".go.kr\"))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Allowed;");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateNetworkDeniedSpecialBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("        // ===== Allowed: Special =====");
        sb.AppendLine($"        // {Comment(loc, "NetRuleDPRKMedia")} - chosonsinbo.com");
        sb.AppendLine("        if (hostLower.Equals(\"chosonsinbo.com\"))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Allowed;");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine($"        // {Comment(loc, "NetRuleTaiwanWIN")}");
        sb.AppendLine($"        // {Comment(loc, "NetRuleThreads")}");
        sb.AppendLine("        var deniedEndsWith = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("        {");
        sb.AppendLine("            \".win.org.tw\", \".threads.com\"");
        sb.AppendLine("        };");
        sb.AppendLine();
        sb.AppendLine("        var deniedExact = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("        {");
        sb.AppendLine("            \"win.org.tw\", \"threads.com\"");
        sb.AppendLine("        };");
        sb.AppendLine();
        sb.AppendLine("        if (deniedEndsWith.Any(host.EndsWith) || deniedExact.Contains(host))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Denied;");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateNetworkAskUserBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("        // ===== AskUser: EndsWith =====");
        sb.AppendLine("        var askUserEndsWith = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("        {");

        sb.AppendLine($"            // {Comment(loc, "NetRuleSecuritiesTrading")}");
        sb.AppendLine("            \".htsc.com.cn\", \".gtja.com\", \".citics.com\", \".cmschina.com\", \".gf.com.cn\",");
        sb.AppendLine("            \".htsec.com\", \".swhysc.com\", \".dfzq.com.cn\", \".guosen.com.cn\", \".xyzq.com.cn\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleThirdPartyTrading")}");
        sb.AppendLine("            \".10jqka.com.cn\", \".eastmoney.com\", \".tdx.com.cn\", \".bloomberg.com\",");
        sb.AppendLine("            \".yahoo.com\", \".finance.yahoo.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleGamePlatforms")}");
        sb.AppendLine("            \".steampowered.com\", \".ea.com\", \".ubisoft.com\", \".blizzard.com\", \".nintendo.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleTaiwanMediaSET")}");
        sb.AppendLine("            \".setn.com\",");
        sb.AppendLine($"            // {Comment(loc, "NetRuleOverseasSocial")}");
        sb.AppendLine("            \".twitch.tv\", \".facebook.com\", \".x.com\", \".gmail.com\",");
        sb.AppendLine("            \".instagram.com\", \".lit.link\"");

        sb.AppendLine("        };");
        sb.AppendLine();
        sb.AppendLine("        if (askUserEndsWith.Any(host.EndsWith))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.AskUser;");
        sb.AppendLine("        }");
        sb.AppendLine();

        sb.AppendLine("        // ===== AskUser: Exact =====");
        sb.AppendLine("        var askUserExact = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("        {");

        sb.AppendLine("            \"bloomberg.com\", \"steampowered.com\",");
        sb.AppendLine("            \"ea.com\", \"ubisoft.com\", \"blizzard.com\", \"nintendo.com\",");
        sb.AppendLine("            \"setn.com\",");
        sb.AppendLine("            \"twitch.tv\", \"facebook.com\", \"x.com\", \"gmail.com\",");
        sb.AppendLine("            \"instagram.com\", \"lit.link\"");

        sb.AppendLine("        };");
        sb.AppendLine();
        sb.AppendLine("        if (askUserExact.Contains(host))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.AskUser;");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateEvaluateCommandLineMethod(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("    private PermissionResult EvaluateCommandLine(Guid callerId, string resourcePath)");
        sb.AppendLine("    {");
        sb.AppendLine("        var cmd = resourcePath.Trim();");
        sb.AppendLine();

        sb.AppendLine($"        // {Comment(loc, "CmdRuleSeparatorDetect")}");
        sb.AppendLine("        // Common: |, ||, &&");
        sb.AppendLine("        // Windows: &");
        sb.AppendLine("        // Linux/macOS: ;");
        sb.AppendLine("        string[] separators;");
        sb.AppendLine("        if (OperatingSystem.IsWindows())");
        sb.AppendLine("        {");
        sb.AppendLine("            separators = [\"||\", \"&&\", \"|\", \"&\"];");
        sb.AppendLine("        }");
        sb.AppendLine("        else");
        sb.AppendLine("        {");
        sb.AppendLine("            separators = [\"||\", \"&&\", \"|\", \";\"];");
        sb.AppendLine("        }");
        sb.AppendLine();

        sb.AppendLine("        bool hasMultipleCommands = false;");
        sb.AppendLine("        foreach (var sep in separators)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (cmd.Contains(sep))");
        sb.AppendLine("            {");
        sb.AppendLine("                hasMultipleCommands = true;");
        sb.AppendLine("                break;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();

        sb.AppendLine("        if (hasMultipleCommands)");
        sb.AppendLine("        {");
        sb.AppendLine("            var parts = cmd.Split(separators, StringSplitOptions.RemoveEmptyEntries);");
        sb.AppendLine("            var hasAskUser = false;");
        sb.AppendLine();
        sb.AppendLine("            foreach (var part in parts)");
        sb.AppendLine("            {");
        sb.AppendLine("                var subResult = Evaluate(callerId, PermissionType.CommandLine, part.Trim());");
        sb.AppendLine("                if (subResult == PermissionResult.Denied)");
        sb.AppendLine("                {");
        sb.AppendLine("                    return PermissionResult.Denied;");
        sb.AppendLine("                }");
        sb.AppendLine("                if (subResult == PermissionResult.AskUser)");
        sb.AppendLine("                {");
        sb.AppendLine("                    hasAskUser = true;");
        sb.AppendLine("                }");
        sb.AppendLine("            }");
        sb.AppendLine();
        sb.AppendLine("            if (hasAskUser)");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.AskUser;");
        sb.AppendLine("            }");
        sb.AppendLine();
        sb.AppendLine("            return PermissionResult.Allowed;");
        sb.AppendLine("        }");
        sb.AppendLine();

        sb.AppendLine("        var cmdLower = cmd.ToLowerInvariant();");
        sb.AppendLine();

        GenerateCommandLineWindowsBlock(sb, loc);
        GenerateCommandLineLinuxBlock(sb, loc);
        GenerateCommandLineMacBlock(sb, loc);

        sb.AppendLine($"        // {Comment(loc, "CmdRuleUnmatched")}");
        sb.AppendLine("        return PermissionResult.AskUser;");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private void GenerateCommandLineWindowsBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("        if (OperatingSystem.IsWindows())");
        sb.AppendLine("        {");
        sb.AppendLine($"            // {Comment(loc, "CmdRuleWinAllow")}");
        sb.AppendLine("            var allowedStartsWithWin = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                \"dir \", \"tree \", \"tasklist \", \"ipconfig \", \"ping \", \"tracert \",");
        sb.AppendLine("                \"set \", \"sc query \", \"findstr \"");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            var allowedExactWin = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                \"tasklist\", \"ipconfig /all\", \"systeminfo\", \"whoami\", \"set\", \"path\"");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            if (allowedStartsWithWin.Any(cmdLower.StartsWith) || allowedExactWin.Contains(cmdLower))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Allowed;");
        sb.AppendLine("            }");
        sb.AppendLine();
        sb.AppendLine($"            // {Comment(loc, "CmdRuleWinDeny")}");
        sb.AppendLine("            var deniedStartsWithWin = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                \"del \", \"rmdir \", \"format \", \"diskpart\", \"reg delete \"");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            if (deniedStartsWithWin.Any(cmdLower.StartsWith))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Denied;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateCommandLineLinuxBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("        else if (OperatingSystem.IsLinux())");
        sb.AppendLine("        {");
        sb.AppendLine($"            // {Comment(loc, "CmdRuleLinuxAllow")}");
        sb.AppendLine("            var allowedStartsWithLinux = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                \"ls \", \"tree \", \"ps \", \"ifconfig \", \"ip \", \"ping \", \"traceroute \",");
        sb.AppendLine("                \"uname \", \"cat \", \"grep \", \"find \", \"df \", \"du \", \"systemctl status \"");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            var allowedExactLinux = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                \"ls\", \"tree\", \"ps\", \"top\", \"ifconfig\", \"uname\", \"whoami\", \"env\", \"df\"");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            if (allowedStartsWithLinux.Any(cmdLower.StartsWith) || allowedExactLinux.Contains(cmdLower))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Allowed;");
        sb.AppendLine("            }");
        sb.AppendLine();
        sb.AppendLine($"            // {Comment(loc, "CmdRuleLinuxDeny")}");
        sb.AppendLine("            var deniedStartsWithLinux = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                \"rm \", \"rmdir \", \"mkfs \", \"fdisk \", \"dd \", \"chmod \", \"chown \", \"chgrp \"");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            if (deniedStartsWithLinux.Any(cmdLower.StartsWith))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Denied;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateCommandLineMacBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("        else if (OperatingSystem.IsMacOS())");
        sb.AppendLine("        {");
        sb.AppendLine($"            // {Comment(loc, "CmdRuleMacAllow")}");
        sb.AppendLine("            var allowedStartsWithMacOS = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                \"ls \", \"tree \", \"ps \", \"ifconfig \", \"ping \", \"traceroute \",");
        sb.AppendLine("                \"system_profiler \", \"cat \", \"grep \", \"find \", \"df \", \"du \", \"launchctl list \"");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            var allowedExactMacOS = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                \"ls\", \"tree\", \"ps\", \"top\", \"ifconfig\", \"system_profiler\", \"sw_vers\",");
        sb.AppendLine("                \"whoami\", \"env\", \"df\"");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            if (allowedStartsWithMacOS.Any(cmdLower.StartsWith) || allowedExactMacOS.Contains(cmdLower))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Allowed;");
        sb.AppendLine("            }");
        sb.AppendLine();
        sb.AppendLine($"            // {Comment(loc, "CmdRuleMacDeny")}");
        sb.AppendLine("            var deniedStartsWithMacOS = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                \"rm \", \"rmdir \", \"diskutil erasedisk \", \"dd \", \"chmod \", \"chown \", \"chgrp \"");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            if (deniedStartsWithMacOS.Any(cmdLower.StartsWith))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Denied;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateEvaluateFileAccessMethod(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("    private PermissionResult EvaluateFileAccess(Guid callerId, string resourcePath)");
        sb.AppendLine("    {");

        sb.AppendLine($"        // {Comment(loc, "FileRuleDangerousExt")}");
        sb.AppendLine("        // .pfx: 证书私钥文件 / .key: 密钥文件 / .bat: Windows 批处理脚本 / .sh: Shell 脚本 / .reg: Windows 注册表文件");
        sb.AppendLine("        var deniedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("        {");
        sb.AppendLine("            \".pfx\", \".key\", \".bat\", \".sh\", \".reg\"");
        sb.AppendLine("        };");
        sb.AppendLine();
        sb.AppendLine("        var ext = Path.GetExtension(resourcePath).ToLowerInvariant();");
        sb.AppendLine("        if (deniedExtensions.Contains(ext))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Denied;");
        sb.AppendLine("        }");
        sb.AppendLine();

        sb.AppendLine("        string filePath;");
        sb.AppendLine("        try");
        sb.AppendLine("        {");
        sb.AppendLine("            filePath = Path.GetFullPath(resourcePath);");
        sb.AppendLine("        }");
        sb.AppendLine("        catch");
        sb.AppendLine("        {");
        sb.AppendLine($"            // {Comment(loc, "FileRuleInvalidPath")}");
        sb.AppendLine("            return PermissionResult.AskUser;");
        sb.AppendLine("        }");
        sb.AppendLine("        var filePathLower = filePath.ToLowerInvariant();");
        sb.AppendLine();

        GenerateFileAccessAssemblyBlock(sb, loc);
        GenerateFileAccessAppDataBlock(sb, loc);
        GenerateFileAccessUserFoldersBlock(sb, loc);
        GenerateFileAccessPublicFoldersBlock(sb, loc);
        GenerateFileAccessSystemDirsBlock(sb, loc);

        sb.AppendLine($"        // {Comment(loc, "FileRuleUnmatched")}");
        sb.AppendLine("        return PermissionResult.AskUser;");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private void GenerateFileAccessAssemblyBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine($"        // {Comment(loc, "FileRuleDenyAssemblyDir")}");
        sb.AppendLine("        var assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)?.ToLowerInvariant();");
        sb.AppendLine("        if (!string.IsNullOrEmpty(assemblyDir) && filePathLower.StartsWith(assemblyDir))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Denied;");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateFileAccessAppDataBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine($"        // {Comment(loc, "FileRuleDenyAppDataDir")}");
        sb.AppendLine($"        // {Comment(loc, "FileRuleAllowOwnTemp")}");
        sb.AppendLine("        if (!string.IsNullOrEmpty(_appDataDirectory))");
        sb.AppendLine("        {");
        sb.AppendLine($"            // {Comment(loc, "FileRuleOwnTemp")}");
        sb.AppendLine("            var ownTempDir = Path.Combine(_appDataDirectory, \"SiliconManager\", callerId.ToString(), \"Temp\").ToLowerInvariant();");
        sb.AppendLine("            if (filePathLower.StartsWith(ownTempDir))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Allowed;");
        sb.AppendLine("            }");
        sb.AppendLine();
        sb.AppendLine($"            // {Comment(loc, "FileRuleDenyOtherDataDir")}");
        sb.AppendLine("            if (filePathLower.StartsWith(_appDataDirectory))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Denied;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateFileAccessUserFoldersBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine($"        // {Comment(loc, "FileRuleUserFolders")}");
        sb.AppendLine("        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToLowerInvariant();");
        sb.AppendLine("        var allowedUserPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("        {");
        sb.AppendLine("            Path.Combine(home, \"Desktop\"),");
        sb.AppendLine("            Path.Combine(home, \"Downloads\"),");
        sb.AppendLine("            Path.Combine(home, \"Documents\"),");
        sb.AppendLine("            Path.Combine(home, \"Pictures\"),");
        sb.AppendLine("            Path.Combine(home, \"Music\"),");
        sb.AppendLine("            Path.Combine(home, OperatingSystem.IsMacOS() ? \"Movies\" : \"Videos\")");
        sb.AppendLine("        };");
        sb.AppendLine();
        sb.AppendLine("        if (allowedUserPaths.Any(filePathLower.StartsWith))");
        sb.AppendLine("        {");
        sb.AppendLine("            return PermissionResult.Allowed;");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateFileAccessPublicFoldersBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine($"        // {Comment(loc, "FileRulePublicFolders")}");
        sb.AppendLine("        if (OperatingSystem.IsWindows())");
        sb.AppendLine("        {");
        sb.AppendLine("            string publicDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory).ToLowerInvariant();");
        sb.AppendLine("            string publicDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments).ToLowerInvariant();");
        sb.AppendLine("            string publicMusicPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic).ToLowerInvariant();");
        sb.AppendLine("            string publicPicturesPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures).ToLowerInvariant();");
        sb.AppendLine("            string publicVideosPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos).ToLowerInvariant();");
        sb.AppendLine("            string publicRootPath = Path.GetDirectoryName(publicDesktopPath)!.ToLowerInvariant();");
        sb.AppendLine("            string publicDownloadsPath = Path.Combine(publicRootPath, \"Downloads\").ToLowerInvariant();");
        sb.AppendLine();
        sb.AppendLine("            var allowedPublicPathsWin = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                publicDesktopPath, publicDocumentsPath, publicMusicPath,");
        sb.AppendLine("                publicPicturesPath, publicVideosPath, publicDownloadsPath");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            if (allowedPublicPathsWin.Any(filePathLower.StartsWith))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Allowed;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("        else if (OperatingSystem.IsMacOS())");
        sb.AppendLine("        {");
        sb.AppendLine("            string sharedPath = \"/users/shared\";");
        sb.AppendLine("            if (filePathLower.StartsWith(sharedPath))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Allowed;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateFileAccessSystemDirsBlock(StringBuilder sb, DefaultLocalizationBase? loc)
    {
        sb.AppendLine("        if (OperatingSystem.IsWindows())");
        sb.AppendLine("        {");
        sb.AppendLine($"            // {Comment(loc, "FileRuleWinDenySystem")}");
        sb.AppendLine("            var winDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows).ToLowerInvariant();");
        sb.AppendLine("            var sysDir = Environment.GetFolderPath(Environment.SpecialFolder.System).ToLowerInvariant();");
        sb.AppendLine("            var progDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).ToLowerInvariant();");
        sb.AppendLine("            var progX86Dir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86).ToLowerInvariant();");
        sb.AppendLine();
        sb.AppendLine("            var deniedSystemPathsWin = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                winDir, sysDir, progDir, progX86Dir");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            if (deniedSystemPathsWin.Any(filePathLower.StartsWith))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Denied;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("        else if (OperatingSystem.IsLinux())");
        sb.AppendLine("        {");
        sb.AppendLine($"            // {Comment(loc, "FileRuleLinuxDenySystem")}");
        sb.AppendLine("            var deniedSystemPathsLinux = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                \"/etc/\", \"/boot/\", \"/sbin/\"");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            if (deniedSystemPathsLinux.Any(filePathLower.StartsWith))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Denied;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("        else if (OperatingSystem.IsMacOS())");
        sb.AppendLine("        {");
        sb.AppendLine($"            // {Comment(loc, "FileRuleMacDenySystem")}");
        sb.AppendLine("            var deniedSystemPathsMacOS = new HashSet<string>(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("            {");
        sb.AppendLine("                \"/system/\", \"/library/\", \"/private/etc/\"");
        sb.AppendLine("            };");
        sb.AppendLine();
        sb.AppendLine("            if (deniedSystemPathsMacOS.Any(filePathLower.StartsWith))");
        sb.AppendLine("            {");
        sb.AppendLine("                return PermissionResult.Denied;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private void GenerateFooter(StringBuilder sb)
    {
        sb.AppendLine("}");
    }
}
