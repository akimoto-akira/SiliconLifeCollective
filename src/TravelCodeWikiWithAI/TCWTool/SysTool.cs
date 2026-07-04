using TravelCodeWikiWithAI.Cldr;

namespace TravelCodeWikiWithAI.TCWTool;

public static class SysTool
{
    /// <summary>
    /// 从 PHPText.resx 解析语言列表（降级方案，与旧代码逻辑一致）
    /// </summary>
    public static Dictionary<string, string> GetAllLanguage()
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        string php = PHPText.LanguageName;
        string[] line = php.Split('\n');
        Func<string, string> removeT = delegate(string content)
        {
            int a = content.IndexOf('\'');
            int b = content.IndexOf('\'', a + 1);
            string c = content.Substring(a + 1, b - a - 1);
            return c;
        };
        foreach (string l in line)
        {
            if (l.Contains("=>"))
            {
                string a = l.Trim();
                int b = a.IndexOf("=>");
                string c = a.Substring(0, b);
                string d = a.Substring(b + 2);
                string e = removeT(c);
                string f = removeT(d);
                result.Add(e, f);
            }
        }

        return result;
    }

    public static string[] GetBaseLanguage()
    {
        return new string[]
            { "*", "en", "en-ca", "en-gb", "zh", "zh-cn", "zh-hans", "zh-hant", "zh-hk", "zh-mo", "zh-my", "zh-sg" };
    }
}
