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
        var allowedLoopback = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "localhost", "127.0.0.1", "::1"
        };
        if (allowedLoopback.Contains(host))
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

        var hostLower = host.ToLowerInvariant();

        // ===== Denied: Contains 匹配（优先级最高） =====
        var deniedContains = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // AI 仿冒站
            "chatgpt", "openai", "deepseek", "sora", "claude", "luma-ai", "kling-ai", "openclaw",
            // 恶意 AI 工具
            "wormgpt", "darkgpt", "fraudgpt", "ghostgpt", "oniongpt", "evilgpt", "hackgpt", "poisongpt",
            // 对抗性 AI
            "adversarial-ai", "prompt-jailbreak", "jailbreak-ai", "llm-hacking", "llm-hack", "llm-exploit",
            // AI 内容农场
            "ai-content-farm", "ai-slop", "auto-ai-write", "mass-ai-article",
            "ai-article-spam", "ai-spam", "ai-generator-free", "ai-essay-mill",
            // AI 数据黑市
            "ai-data-blackmarket", "api-key-market", "api-key-free",
            "llm-weight-seller", "llm-weight-free", "model-pirate",
            // AI 仿冒/诈骗
            "ai-official-fake", "ai-fake", "ai-vip", "ai-zh", "ai-cn", "ai-free-vip", "ai-premium-crack",
            // 其他
            "4399"
        };

        if (deniedContains.Any(hostLower.Contains))
        {
            return PermissionResult.Denied;
        }

        // 特殊处理：sakura-cat StartsWith
        if (hostLower.StartsWith("sakura-cat"))
        {
            return PermissionResult.Denied;
        }

        // ===== Allowed: EndsWith 匹配 =====
        var allowedEndsWith = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // 谷歌 / 必应 / 腾讯系 / 搜狗 / DuckDuckGo / Yandex / 微信 / 阿里系
            ".google.com", ".bing.com", ".qq.com", ".tencent.com", ".weixin.qq.com",
            ".sogou.com", ".duckduckgo.com", ".yandex.com", ".yandex.ru", ".wechat.com",
            ".alibaba.com", ".alibabacloud.com", ".aliyun.com", ".1688.com", ".alipay.com",
            ".tmall.com", ".dingtalk.com", ".taobao.com",
            // 哔哩哔哩 / niconico / Acfun / 抖音 / TikTok / 快手 / 小红书
            ".bilibili.com", ".bilivideo.com", ".nicovideo.jp", ".niconico.com",
            ".acfun.cn", ".douyin.com", ".tiktok.com", ".kuaishou.com", ".xiaohongshu.com",
            // AI 服务
            ".openai.com", ".anthropic.com", ".huggingface.co", ".ollama.com",
            ".tongyi.aliyun.com", ".qianwen.com", ".moonshot.cn", ".kimi.com",
            ".doubao.com", ".capcut.com", ".jianying.com", ".trae.cn", ".trae.ai",
            // 证券交易所 / 财经资讯 / 投资社区
            ".sse.com.cn", ".szse.cn", ".cninfo.com.cn",
            ".jrj.com.cn", ".stockstar.com", ".hexun.com",
            ".xueqiu.com", ".cls.cn", ".kaipanla.com", ".taoguba.com.cn",
            // 开发者服务
            ".github.com", ".githubusercontent.com", ".gitee.com",
            ".stackoverflow.com", ".npmjs.com", ".nuget.org", ".pypi.org", ".microsoft.com",
            // 游戏引擎
            ".unity.com", ".unity3d.com", ".unrealengine.com", ".epicgames.com", ".fab.com",
            // 世嘉
            ".sega.com",
            // 全球云服务平台
            ".azure.com", ".azurewebsites.net", ".cloud.google.com",
            ".digitalocean.com", ".heroku.com", ".vercel.com", ".netlify.com",
            // 全球开发与部署工具
            ".gitlab.com", ".bitbucket.org", ".docker.com", ".cloudflare.com",
            // 云服务与开发工具
            ".amazon.com", ".amazonaws.com", ".kiro.dev", ".codebuddy.cn",
            ".jetbrains.com", ".purelight.net.cn", ".w3school.com.cn",
            // 社交/资讯（中国大陆）
            ".weibo.com", ".zhihu.com", ".163.com", ".sina.com.cn",
            ".ifeng.com", ".xinhuanet.com", ".cctv.com",
            // 中国台湾媒体
            ".ctinews.com",
            // 日本媒体
            ".nhk.or.jp",
            // 韩国媒体
            ".kbs.co.kr", ".imbc.com", ".sbs.co.kr", ".ebs.co.kr",
            // 朝鲜媒体
            ".naenara.com.kp", ".rodong.rep.kp", ".youth.rep.kp",
            ".vok.rep.kp", ".pyongyangtimes.com.kp",
            // 全球社交协作平台
            ".reddit.com", ".discord.com", ".discordapp.com", ".slack.com",
            ".notion.so", ".figma.com", ".dropbox.com",
            // WhatsApp
            ".whatsapp.com",
            // 全球视频/音乐平台
            ".spotify.com", ".apple.com", ".vimeo.com",
            // 视频/媒体
            ".youtube.com", ".iqiyi.com", ".youku.com",
            // 地图
            ".openstreetmap.org",
            // 百科
            ".wikipedia.org", ".mediawiki.org"
        };

        if (allowedEndsWith.Any(host.EndsWith))
        {
            return PermissionResult.Allowed;
        }

        // ===== Allowed: 完全匹配 =====
        var allowedExact = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "google.com", "bing.com", "qq.com", "tencent.com", "sogou.com",
            "duckduckgo.com", "yandex.com", "yandex.ru", "wechat.com",
            "alibaba.com", "alibabacloud.com", "aliyun.com", "1688.com", "alipay.com",
            "tmall.com", "dingtalk.com", "taobao.com",
            "bilibili.com", "bilivideo.com", "nicovideo.jp", "niconico.com",
            "acfun.cn", "douyin.com", "tiktok.com", "kuaishou.com", "xiaohongshu.com",
            "openai.com", "anthropic.com", "huggingface.co", "ollama.com",
            "qianwen.com", "moonshot.cn", "kimi.com", "doubao.com",
            "capcut.com", "jianying.com", "trae.cn", "trae.ai",
            "github.com", "githubusercontent.com", "gitee.com",
            "stackoverflow.com", "npmjs.com", "nuget.org", "pypi.org", "microsoft.com",
            "unity.com", "unity3d.com", "unrealengine.com", "epicgames.com", "fab.com",
            "sega.com",
            "azure.com", "digitalocean.com", "heroku.com", "vercel.com", "netlify.com",
            "gitlab.com", "bitbucket.org", "docker.com", "cloudflare.com",
            "amazon.com", "amazonaws.com", "kiro.dev", "codebuddy.cn",
            "jetbrains.com", "purelight.net.cn", "w3school.com.cn",
            "weibo.com", "zhihu.com", "163.com", "ifeng.com",
            "xinhuanet.com", "cctv.com",
            "ctinews.com",
            "nhk.or.jp",
            "kbs.co.kr", "imbc.com", "sbs.co.kr", "ebs.co.kr",
            "chosonsinbo.com",
            "reddit.com", "discord.com", "discordapp.com", "slack.com",
            "notion.so", "figma.com", "dropbox.com",
            "whatsapp.com",
            "spotify.com", "apple.com", "vimeo.com",
            "youtube.com", "iqiyi.com", "youku.com",
            "openstreetmap.org",
            "wikipedia.org", "mediawiki.org"
        };

        if (allowedExact.Contains(host))
        {
            return PermissionResult.Allowed;
        }

        // ===== Allowed: Contains 特殊匹配 =====
        // 俄罗斯媒体
        if (hostLower.Contains("sputniknews"))
        {
            return PermissionResult.Allowed;
        }

        // 百科 - creativecommons
        if (hostLower.Contains("creativecommons"))
        {
            return PermissionResult.Allowed;
        }

        // 政府网站
        if (hostLower.Contains(".gov") || hostLower.EndsWith(".go.jp") || hostLower.EndsWith(".go.kr"))
        {
            return PermissionResult.Allowed;
        }

        // ===== Denied: 特殊匹配 =====
        // 朝鲜媒体 - chosonsinbo.com
        if (hostLower.Equals("chosonsinbo.com"))
        {
            return PermissionResult.Allowed;
        }

        // WIN (Taiwan) 和 Threads - Denied
        var deniedEndsWith = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".win.org.tw", ".threads.com"
        };

        var deniedExact = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "win.org.tw", "threads.com"
        };

        if (deniedEndsWith.Any(host.EndsWith) || deniedExact.Contains(host))
        {
            return PermissionResult.Denied;
        }

        // ===== AskUser: EndsWith 匹配 =====
        var askUserEndsWith = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // 证券交易平台
            ".htsc.com.cn", ".gtja.com", ".citics.com", ".cmschina.com", ".gf.com.cn",
            ".htsec.com", ".swhysc.com", ".dfzq.com.cn", ".guosen.com.cn", ".xyzq.com.cn",
            // 第三方交易平台
            ".10jqka.com.cn", ".eastmoney.com", ".tdx.com.cn", ".bloomberg.com",
            ".yahoo.com", ".finance.yahoo.com",
            // 游戏平台
            ".steampowered.com", ".ea.com", ".ubisoft.com", ".blizzard.com", ".nintendo.com",
            // 三立新闻网
            ".setn.com",
            // 海外社交/直播
            ".twitch.tv", ".facebook.com", ".x.com", ".gmail.com",
            ".instagram.com", ".lit.link"
        };

        if (askUserEndsWith.Any(host.EndsWith))
        {
            return PermissionResult.AskUser;
        }

        // ===== AskUser: 完全匹配 =====
        var askUserExact = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "bloomberg.com", "steampowered.com",
            "ea.com", "ubisoft.com", "blizzard.com", "nintendo.com",
            "setn.com",
            "twitch.tv", "facebook.com", "x.com", "gmail.com",
            "instagram.com", "lit.link"
        };

        if (askUserExact.Contains(host))
        {
            return PermissionResult.AskUser;
        }

        // 未匹配的网络访问，询问用户 / Unmatched network access, ask user
        return PermissionResult.AskUser;
    }



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
            // Windows 允许：只读/查询类命令
            var allowedStartsWithWin = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "dir ", "tree ", "tasklist ", "ipconfig ", "ping ", "tracert ",
                "set ", "sc query ", "findstr "
            };

            var allowedExactWin = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "tasklist", "ipconfig /all", "systeminfo", "whoami", "set", "path"
            };

            if (allowedStartsWithWin.Any(cmdLower.StartsWith) || allowedExactWin.Contains(cmdLower))
            {
                return PermissionResult.Allowed;
            }

            // Windows 禁止：危险/破坏性命令
            var deniedStartsWithWin = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "del ", "rmdir ", "format ", "diskpart", "reg delete "
            };

            if (deniedStartsWithWin.Any(cmdLower.StartsWith))
            {
                return PermissionResult.Denied;
            }
        }
        else if (OperatingSystem.IsLinux())
        {
            // Linux 允许：只读/查询类命令
            var allowedStartsWithLinux = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ls ", "tree ", "ps ", "ifconfig ", "ip ", "ping ", "traceroute ",
                "uname ", "cat ", "grep ", "find ", "df ", "du ", "systemctl status "
            };

            var allowedExactLinux = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ls", "tree", "ps", "top", "ifconfig", "uname", "whoami", "env", "df"
            };

            if (allowedStartsWithLinux.Any(cmdLower.StartsWith) || allowedExactLinux.Contains(cmdLower))
            {
                return PermissionResult.Allowed;
            }

            // Linux 禁止：危险/破坏性命令
            var deniedStartsWithLinux = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "rm ", "rmdir ", "mkfs ", "fdisk ", "dd ", "chmod ", "chown ", "chgrp "
            };

            if (deniedStartsWithLinux.Any(cmdLower.StartsWith))
            {
                return PermissionResult.Denied;
            }
        }
        else if (OperatingSystem.IsMacOS())
        {
            // macOS 允许：只读/查询类命令
            var allowedStartsWithMacOS = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ls ", "tree ", "ps ", "ifconfig ", "ping ", "traceroute ",
                "system_profiler ", "cat ", "grep ", "find ", "df ", "du ", "launchctl list "
            };

            var allowedExactMacOS = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ls", "tree", "ps", "top", "ifconfig", "system_profiler", "sw_vers",
                "whoami", "env", "df"
            };

            if (allowedStartsWithMacOS.Any(cmdLower.StartsWith) || allowedExactMacOS.Contains(cmdLower))
            {
                return PermissionResult.Allowed;
            }

            // macOS 禁止：危险/破坏性命令
            var deniedStartsWithMacOS = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "rm ", "rmdir ", "diskutil erasedisk ", "dd ", "chmod ", "chown ", "chgrp "
            };

            if (deniedStartsWithMacOS.Any(cmdLower.StartsWith))
            {
                return PermissionResult.Denied;
            }
        }

        // 未匹配的命令，询问用户 / Unmatched commands, ask user
        return PermissionResult.AskUser;
    }



    private PermissionResult EvaluateFileAccess(Guid callerId, string resourcePath)
    {
        // 最高优先级：危险文件扩展名直接拒绝，无论目录是否允许
        // Highest priority: deny dangerous file extensions regardless of directory permissions
        // .pfx: 证书私钥文件 / .key: 密钥文件 / .bat: Windows 批处理脚本 / .sh: Shell 脚本 / .reg: Windows 注册表文件
        var deniedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".pfx", ".key", ".bat", ".sh", ".reg"
        };

        var ext = Path.GetExtension(resourcePath).ToLowerInvariant();
        if (deniedExtensions.Contains(ext))
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
        var allowedUserPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Path.Combine(home, "Desktop"),
            Path.Combine(home, "Downloads"),
            Path.Combine(home, "Documents"),
            Path.Combine(home, "Pictures"),
            Path.Combine(home, "Music"),
            Path.Combine(home, OperatingSystem.IsMacOS() ? "Movies" : "Videos")
        };

        if (allowedUserPaths.Any(filePathLower.StartsWith))
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

            var allowedPublicPathsWin = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                publicDesktopPath, publicDocumentsPath, publicMusicPath,
                publicPicturesPath, publicVideosPath, publicDownloadsPath
            };

            if (allowedPublicPathsWin.Any(filePathLower.StartsWith))
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

            var deniedSystemPathsWin = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                winDir, sysDir, progDir, progX86Dir
            };

            if (deniedSystemPathsWin.Any(filePathLower.StartsWith))
            {
                return PermissionResult.Denied;
            }
        }
        else if (OperatingSystem.IsLinux())
        {
            // Linux 禁止：系统关键目录 / Linux deny: critical system directories
            // /etc /boot /sbin
            var deniedSystemPathsLinux = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "/etc/", "/boot/", "/sbin/"
            };

            if (deniedSystemPathsLinux.Any(filePathLower.StartsWith))
            {
                return PermissionResult.Denied;
            }
        }
        else if (OperatingSystem.IsMacOS())
        {
            // macOS 禁止：系统关键目录 / macOS deny: critical system directories
            // /System /Library /private/etc
            var deniedSystemPathsMacOS = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "/system/", "/library/", "/private/etc/"
            };

            if (deniedSystemPathsMacOS.Any(filePathLower.StartsWith))
            {
                return PermissionResult.Denied;
            }
        }

        // 未匹配的路径，询问用户 / Unmatched paths, ask user
        return PermissionResult.AskUser;
    }


}
