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
        // Network access allow rules
        if (permissionType == PermissionType.NetworkAccess && !string.IsNullOrWhiteSpace(resource))
        {
            return EvaluateNetwork(resource);
        }

        // Command line rules (cross-platform)
        if (permissionType == PermissionType.CommandLine && !string.IsNullOrWhiteSpace(resource))
        {
            return EvaluateCommandLine(callerId, resource);
        }

        // File access rules (cross-platform)
        if (permissionType == PermissionType.FileAccess && !string.IsNullOrWhiteSpace(resource))
        {
            return EvaluateFileAccess(callerId, resource);
        }

        // Other permission types default to allowed
        return permissionType switch
        {
            PermissionType.Function => PermissionResult.Allowed,
            PermissionType.DataAccess => PermissionResult.Allowed,
            _ => PermissionResult.AskUser
        };
    }



    private PermissionResult EvaluateNetwork(string resourcePath)
    {
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

        // Allow loopback addresses
        var allowedLoopback = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "localhost", "127.0.0.1", "::1"
        };
        if (allowedLoopback.Contains(host))
        {
            return PermissionResult.Allowed;
        }

        // Private IP address range matching (validate IPv4 format first)
        if (IPAddress.TryParse(host, out var ipAddr)
            && ipAddr.AddressFamily == AddressFamily.InterNetwork)
        {
            var bytes = ipAddr.GetAddressBytes();

            // Allow Class C private addresses (192.168.0.0/16)
            if (bytes[0] == 192 && bytes[1] == 168)
            {
                return PermissionResult.Allowed;
            }

            // Allow Class A private addresses (10.0.0.0/8)
            if (bytes[0] == 10)
            {
                return PermissionResult.Allowed;
            }

            // Allow Class B private addresses selectively (172.16.0.0/12, i.e. 172.16.* ~ 172.31.*)
            if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
            {
                return PermissionResult.AskUser;
            }
        }

        var hostLower = host.ToLowerInvariant();

        // ===== Denied: Contains match (highest priority) =====
        var deniedContains = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // AI impersonation sites
            "chatgpt", "openai", "deepseek", "sora", "claude", "luma-ai", "kling-ai", "openclaw",
            // Malicious AI tools
            "wormgpt", "darkgpt", "fraudgpt", "ghostgpt", "oniongpt", "evilgpt", "hackgpt", "poisongpt",
            // Adversarial AI
            "adversarial-ai", "prompt-jailbreak", "jailbreak-ai", "llm-hacking", "llm-hack", "llm-exploit",
            // AI content farms
            "ai-content-farm", "ai-slop", "auto-ai-write", "mass-ai-article",
            "ai-article-spam", "ai-spam", "ai-generator-free", "ai-essay-mill",
            // AI data black market
            "ai-data-blackmarket", "api-key-market", "api-key-free",
            "llm-weight-seller", "llm-weight-free", "model-pirate",
            // AI impersonation/scam
            "ai-official-fake", "ai-fake", "ai-vip", "ai-zh", "ai-cn", "ai-free-vip", "ai-premium-crack",
            // Other
            "4399"
        };

        if (deniedContains.Any(hostLower.Contains))
        {
            return PermissionResult.Denied;
        }

        // Special handling: sakura-cat StartsWith
        if (hostLower.StartsWith("sakura-cat"))
        {
            return PermissionResult.Denied;
        }

        // ===== Allowed: EndsWith match =====
        var allowedEndsWith = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Google / Bing / Tencent / Sogou / DuckDuckGo / Yandex / WeChat / Alibaba
            ".google.com", ".bing.com", ".qq.com", ".tencent.com", ".weixin.qq.com",
            ".sogou.com", ".duckduckgo.com", ".yandex.com", ".yandex.ru", ".wechat.com",
            ".alibaba.com", ".alibabacloud.com", ".aliyun.com", ".1688.com", ".alipay.com",
            ".tmall.com", ".dingtalk.com", ".taobao.com",
            // Bilibili / niconico / Acfun / Douyin / TikTok / Kuaishou / Xiaohongshu
            ".bilibili.com", ".bilivideo.com", ".nicovideo.jp", ".niconico.com",
            ".acfun.cn", ".douyin.com", ".tiktok.com", ".kuaishou.com", ".xiaohongshu.com",
            // AI services
            ".openai.com", ".anthropic.com", ".huggingface.co", ".ollama.com",
            ".tongyi.aliyun.com", ".qianwen.com", ".moonshot.cn", ".kimi.com",
            ".doubao.com", ".capcut.com", ".jianying.com", ".trae.cn", ".trae.ai",
            // Stock exchanges / financial news / investment communities
            ".sse.com.cn", ".szse.cn", ".cninfo.com.cn",
            ".jrj.com.cn", ".stockstar.com", ".hexun.com",
            ".xueqiu.com", ".cls.cn", ".kaipanla.com", ".taoguba.com.cn",
            // Developer services
            ".github.com", ".githubusercontent.com", ".gitee.com",
            ".stackoverflow.com", ".npmjs.com", ".nuget.org", ".pypi.org", ".microsoft.com",
            // Game engines
            ".unity.com", ".unity3d.com", ".unrealengine.com", ".epicgames.com", ".fab.com",
            // Sega
            ".sega.com",
            // Global cloud service platforms
            ".azure.com", ".azurewebsites.net", ".cloud.google.com",
            ".digitalocean.com", ".heroku.com", ".vercel.com", ".netlify.com",
            // Global development and deployment tools
            ".gitlab.com", ".bitbucket.org", ".docker.com", ".cloudflare.com",
            // Cloud services and development tools
            ".amazon.com", ".amazonaws.com", ".kiro.dev", ".codebuddy.cn",
            ".jetbrains.com", ".purelight.net.cn", ".w3school.com.cn",
            // Social/news (Mainland China)
            ".weibo.com", ".zhihu.com", ".163.com", ".sina.com.cn",
            ".ifeng.com", ".xinhuanet.com", ".cctv.com",
            // Taiwan media
            ".ctinews.com",
            // Japanese media
            ".nhk.or.jp",
            // Korean media
            ".kbs.co.kr", ".imbc.com", ".sbs.co.kr", ".ebs.co.kr",
            // North Korean media
            ".naenara.com.kp", ".rodong.rep.kp", ".youth.rep.kp",
            ".vok.rep.kp", ".pyongyangtimes.com.kp",
            // Global social collaboration platforms
            ".reddit.com", ".discord.com", ".discordapp.com", ".slack.com",
            ".notion.so", ".figma.com", ".dropbox.com",
            // WhatsApp
            ".whatsapp.com",
            // Global video/music platforms
            ".spotify.com", ".apple.com", ".vimeo.com",
            // Video/media
            ".youtube.com", ".iqiyi.com", ".youku.com",
            // Maps
            ".openstreetmap.org",
            // Encyclopedia
            ".wikipedia.org", ".mediawiki.org"
        };

        if (allowedEndsWith.Any(host.EndsWith))
        {
            return PermissionResult.Allowed;
        }

        // ===== Allowed: Exact match =====
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
            "wttr.in",
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

        // ===== Allowed: Contains special match =====
        // Russian media
        if (hostLower.Contains("sputniknews"))
        {
            return PermissionResult.Allowed;
        }

        // Weather information source
        if (hostLower.Equals("wttr.in") || hostLower.EndsWith(".wttr.in"))
        {
            return PermissionResult.Allowed;
        }

        // Encyclopedia - creativecommons
        if (hostLower.Contains("creativecommons"))
        {
            return PermissionResult.Allowed;
        }

        // Government websites
        if (hostLower.Contains(".gov") || hostLower.EndsWith(".go.jp") || hostLower.EndsWith(".go.kr"))
        {
            return PermissionResult.Allowed;
        }

        // ===== Denied: Special match =====
        // North Korean media - chosonsinbo.com
        if (hostLower.Equals("chosonsinbo.com"))
        {
            return PermissionResult.Allowed;
        }

        // WIN (Taiwan) and Threads - Denied
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

        // ===== AskUser: EndsWith match =====
        var askUserEndsWith = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Securities trading platforms
            ".htsc.com.cn", ".gtja.com", ".citics.com", ".cmschina.com", ".gf.com.cn",
            ".htsec.com", ".swhysc.com", ".dfzq.com.cn", ".guosen.com.cn", ".xyzq.com.cn",
            // Third-party trading platforms
            ".10jqka.com.cn", ".eastmoney.com", ".tdx.com.cn", ".bloomberg.com",
            ".yahoo.com", ".finance.yahoo.com",
            // Game platforms
            ".steampowered.com", ".ea.com", ".ubisoft.com", ".blizzard.com", ".nintendo.com",
            // Sanli News
            ".setn.com",
            // Overseas social/live streaming
            ".twitch.tv", ".facebook.com", ".x.com", ".gmail.com",
            ".instagram.com", ".lit.link"
        };

        if (askUserEndsWith.Any(host.EndsWith))
        {
            return PermissionResult.AskUser;
        }

        // ===== AskUser: Exact match =====
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

        // Unmatched network access, ask user
        return PermissionResult.AskUser;
    }



    private PermissionResult EvaluateCommandLine(Guid callerId, string resourcePath)
    {
        var cmd = resourcePath.Trim();

        // Detect pipe and multi-command separators, split and verify each
        // Common: |, ||, &&
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

        // Check if contains any separator
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
            // Split into multiple commands
            var parts = cmd.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var hasAskUser = false;

            foreach (var part in parts)
            {
                var subResult = Evaluate(callerId, PermissionType.CommandLine, part.Trim());
                if (subResult == PermissionResult.Denied)
                {
                    // If any sub-command is denied, deny the entire command
                    return PermissionResult.Denied;
                }
                if (subResult == PermissionResult.AskUser)
                {
                    hasAskUser = true;
                }
            }

            // No denials, but some need asking, ask user
            if (hasAskUser)
            {
                return PermissionResult.AskUser;
            }

            // All allowed
            return PermissionResult.Allowed;
        }

        var cmdLower = cmd.ToLowerInvariant();

        if (OperatingSystem.IsWindows())
        {
            // Windows allow: read-only/query commands
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

            // Windows deny: dangerous/destructive commands
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
            // Linux allow: read-only/query commands
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

            // Linux deny: dangerous/destructive commands
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
            // macOS allow: read-only/query commands
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

            // macOS deny: dangerous/destructive commands
            var deniedStartsWithMacOS = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "rm ", "rmdir ", "diskutil erasedisk ", "dd ", "chmod ", "chown ", "chgrp "
            };

            if (deniedStartsWithMacOS.Any(cmdLower.StartsWith))
            {
                return PermissionResult.Denied;
            }
        }

        // Unmatched commands, ask user
        return PermissionResult.AskUser;
    }



    private PermissionResult EvaluateFileAccess(Guid callerId, string resourcePath)
    {
        // Highest priority: deny dangerous file extensions regardless of directory permissions
        // .pfx: certificate private key / .key: key file / .bat: Windows batch script / .sh: Shell script / .reg: Windows registry file
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
            // Cannot resolve to absolute path, ask user
            return PermissionResult.AskUser;
        }
        var filePathLower = filePath.ToLowerInvariant();

        // Deny: current assembly directory
        var assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)?.ToLowerInvariant();
        if (!string.IsNullOrEmpty(assemblyDir) && filePathLower.StartsWith(assemblyDir))
        {
            return PermissionResult.Denied;
        }

        // Deny: application data directory (from constructor)
        // But allow: own Temp directory
        if (!string.IsNullOrEmpty(_appDataDirectory))
        {
            // Allow: own Temp directory
            var ownTempDir = Path.Combine(_appDataDirectory, "SiliconManager", callerId.ToString(), "Temp").ToLowerInvariant();
            if (filePathLower.StartsWith(ownTempDir))
            {
                return PermissionResult.Allowed;
            }

            // Deny: other paths in data directory (including other silicon beings' directories)
            if (filePathLower.StartsWith(_appDataDirectory))
            {
                return PermissionResult.Denied;
            }
        }

        // Allow: common user folders
        // Desktop / Downloads / Documents / Pictures / Music / Videos
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

        // Allow: public/common user folders
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
            // Linux deny: critical system directories
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
            // macOS deny: critical system directories
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

        // Unmatched paths, ask user
        return PermissionResult.AskUser;
    }


}
