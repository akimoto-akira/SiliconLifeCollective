using System.Text.RegularExpressions;

namespace SiliconLife.Collective;

public static class MentionParser
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(nameof(MentionParser));

    private static readonly Regex MentionPattern = new(@"@(\S+)", RegexOptions.Compiled);
    private static readonly HashSet<string> AllMentionKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "all", "everyone", "所有人", "大家"
    };

    public static List<Guid> ParseMentionedIds(string content, List<Guid> sessionMembers)
    {
        var mentionedIds = new List<Guid>();
        if (string.IsNullOrWhiteSpace(content) || sessionMembers == null || sessionMembers.Count == 0)
        {
            return mentionedIds;
        }

        SiliconBeingManager? beingManager = ServiceLocator.Instance.BeingManager;
        if (beingManager == null)
        {
            return mentionedIds;
        }

        var beingDict = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        foreach (var being in beingManager.GetAllBeings())
        {
            if (sessionMembers.Contains(being.Id))
            {
                beingDict[being.Name] = being.Id;
            }
        }

        var userNickname = Config.Instance?.Data?.UserNickname;
        Guid userId = Config.Instance?.Data?.UserGuid ?? Guid.Empty;
        if (!string.IsNullOrEmpty(userNickname) && userId != Guid.Empty && sessionMembers.Contains(userId))
        {
            beingDict[userNickname!] = userId;
        }

        MatchCollection matches = MentionPattern.Matches(content);
        foreach (Match match in matches)
        {
            string mentionName = match.Groups[1].Value;

            if (AllMentionKeywords.Contains(mentionName))
            {
                mentionedIds.Add(Guid.Empty);
                continue;
            }

            if (beingDict.TryGetValue(mentionName, out Guid beingId))
            {
                if (!mentionedIds.Contains(beingId))
                {
                    mentionedIds.Add(beingId);
                }
            }
        }

        if (mentionedIds.Count > 0)
        {
            _logger.Debug(null, "Parsed {0} mention(s) from content: [{1}]",
                mentionedIds.Count, string.Join(", ", mentionedIds.Select(id => id == Guid.Empty ? "@all" : id.ToString("N").Substring(0, 8))));
        }

        return mentionedIds;
    }

    public static bool IsMentioned(Guid beingId, List<Guid> mentionedIds)
    {
        if (mentionedIds == null || mentionedIds.Count == 0)
        {
            return false;
        }

        if (mentionedIds.Contains(Guid.Empty))
        {
            return true;
        }

        return mentionedIds.Contains(beingId);
    }
}
