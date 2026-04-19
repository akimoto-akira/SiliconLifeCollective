// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Calendar and date calculation tool.
/// Supports multiple calendar systems derived from <see cref="CalendarBase"/>.
/// The desired calendar is selected via the optional <c>calendar</c> parameter (defaults to "gregorian").
/// </summary>
public class CalendarTool : ITool
{
    public string Name => "calendar";

    public string Description =>
        "Get current date/time information and perform date calculations using various calendar systems. " +
        "Actions: 'now' (current date/time), 'format' (format/localize a date), " +
        "'add_days' (add/subtract days), 'diff' (difference between two dates), " +
        "'get_components' (list component definitions of a calendar), " +
        "'get_now_components' (get current date/time broken down into calendar components), " +
        "'convert' (convert a date from one calendar system to another, requires calendar, calendar2, and date), " +
        "'list_calendars' (list all supported calendar systems).";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    public Dictionary<string, object> GetParameterSchema()
    {
        var calendarIds = BuildCalendarRegistry().Keys.ToArray<object>();

        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The action to perform.",
                    ["enum"] = new[] { "now", "format", "add_days", "diff", "list_calendars", "get_components", "get_now_components", "convert" }
                },
                ["calendar"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Calendar system to use (default: gregorian). For 'convert', this is the source calendar.",
                    ["enum"] = calendarIds
                },
                ["calendar2"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Target calendar system for 'convert'.",
                    ["enum"] = calendarIds
                },
                ["date"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Date string in ISO format (yyyy-MM-dd or yyyy-MM-dd HH:mm:ss). Used with format, add_days, diff, convert."
                },
                ["date2"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Second date for diff (yyyy-MM-dd or yyyy-MM-dd HH:mm:ss)."
                },
                ["days"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Number of days to add (positive) or subtract (negative). Used with add_days."
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
            return ToolResult.Failed("Missing 'action' parameter.");

        string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";

        try
        {
            if (action == "list_calendars")
                return ExecuteListCalendars();

            var registry = BuildCalendarRegistry();

            if (action == "convert")
                return ExecuteConvert(registry, parameters);

            string calendarId = parameters.TryGetValue("calendar", out var calObj)
                ? calObj?.ToString() ?? "gregorian"
                : "gregorian";

            if (!registry.TryGetValue(calendarId, out CalendarBase? calendar))
                return ToolResult.Failed($"Unknown calendar '{calendarId}'. Use list_calendars to see available options.");

            return action switch
            {
                "now"               => ExecuteNow(calendar),
                "format"            => ExecuteFormat(calendar, parameters),
                "add_days"          => ExecuteAddDays(calendar, parameters),
                "diff"              => ExecuteDiff(calendar, parameters),
                "get_components"    => ExecuteGetComponents(calendar),
                "get_now_components"=> ExecuteGetNowComponents(calendar),
                _                   => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Calendar error: {ex.Message}");
        }
    }

    // ── registry ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Builds the calendar registry for the current application language.
    /// Register additional <see cref="CalendarBase"/> subclasses here as they are implemented.
    /// </summary>
    public static Dictionary<string, CalendarBase> BuildCalendarRegistry()
    {
        DefaultLocalizationBase loc = GetLocalization();
        var registry = new Dictionary<string, CalendarBase>(StringComparer.OrdinalIgnoreCase);

        // ── registered calendars ──────────────────────────────────────────────
        Register(registry, new GregorianCalendar(loc));
        Register(registry, new BuddhistCalendar(loc));
        Register(registry, new JucheCalendar(loc));
        Register(registry, new RepublicOfChinaCalendar(loc));
        Register(registry, new ChulaSakaratCalendar(loc));
        Register(registry, new JulianCalendar(loc));
        Register(registry, new KhmerCalendar(loc));
        Register(registry, new ZoroastrianCalendar(loc));
        Register(registry, new FrenchRepublicanCalendar(loc));
        Register(registry, new CopticCalendar(loc));
        Register(registry, new EthiopianCalendar(loc));
        Register(registry, new IslamicCalendar(loc));
        Register(registry, new HebrewCalendar(loc));
        Register(registry, new PersianCalendar(loc));
        Register(registry, new IndianCalendar(loc));
        Register(registry, new SakaCalendar(loc));
        Register(registry, new VikramSamvatCalendar(loc));
        Register(registry, new JavaneseCalendar(loc));
        Register(registry, new MongolianCalendar(loc));
        Register(registry, new TibetanCalendar(loc));
        Register(registry, new MayanCalendar(loc));
        Register(registry, new CherokeeCalendar(loc));
        Register(registry, new InuitCalendar(loc));
        Register(registry, new RomanCalendar(loc));
        Register(registry, new ChineseLunarCalendar(loc));
        Register(registry, new VietnameseCalendar(loc));
        Register(registry, new JapaneseCalendar(loc));
        Register(registry, new SexagenaryCalendar(loc));
        Register(registry, new YiCalendar(loc));
        Register(registry, new DaiCalendar(loc));
        Register(registry, new DehongDaiCalendar(loc));
        // ─────────────────────────────────────────────────────────────────────

        return registry;
    }

    private static void Register(Dictionary<string, CalendarBase> registry, CalendarBase calendar)
        => registry[calendar.CalendarId] = calendar;

    // ── actions ───────────────────────────────────────────────────────────────

    private static ToolResult ExecuteListCalendars()
    {
        var registry = BuildCalendarRegistry();
        var lines = registry.Values.Select(c =>
        {
            var components = string.Join(", ", c.GetComponents().Select(kv => $"{kv.Key}({kv.Value})"));
            return $"  {c.CalendarId,-20} {c.CalendarName}\n    Components: {components}";
        });
        return ToolResult.Successful("Supported calendars:\n" + string.Join("\n", lines));
    }

    private static ToolResult ExecuteNow(CalendarBase calendar)
    {
        DateTime now = DateTime.Now;
        var components = calendar.FromDateTime(now);

        string result =
            $"Calendar: {calendar.CalendarName}\n" +
            $"Current date/time: {calendar.Localize(components)}\n" +
            $"ISO format: {calendar.Format(components)}\n" +
            $"Day of week: {now:dddd}\n" +
            $"Unix timestamp: {new DateTimeOffset(now).ToUnixTimeSeconds()}\n" +
            $"UTC: {calendar.Format(calendar.FromDateTime(DateTime.UtcNow))}";
        return ToolResult.Successful(result);
    }

    private static ToolResult ExecuteFormat(CalendarBase calendar, Dictionary<string, object> parameters)
    {
        DateTime date = GetDateParameter(parameters, "date", calendar) ?? DateTime.Now;
        var components = calendar.FromDateTime(date);
        return ToolResult.Successful(
            $"Calendar: {calendar.CalendarName}\n" +
            $"Localized: {calendar.Localize(components)}\n" +
            $"ISO: {calendar.Format(components)}");
    }

    private static ToolResult ExecuteAddDays(CalendarBase calendar, Dictionary<string, object> parameters)
    {
        DateTime date = GetDateParameter(parameters, "date", calendar) ?? DateTime.Now;

        if (!parameters.TryGetValue("days", out object? daysObj) || daysObj == null)
            return ToolResult.Failed("Missing 'days' parameter for add_days.");
        if (!int.TryParse(daysObj.ToString(), out int days))
            return ToolResult.Failed("'days' must be an integer.");

        DateTime result = date.AddDays(days);
        string op = days >= 0 ? $"+ {days}" : $"- {Math.Abs(days)}";
        return ToolResult.Successful(
            $"Calendar: {calendar.CalendarName}\n" +
            $"{calendar.Localize(calendar.FromDateTime(date))} {op} days = " +
            $"{calendar.Localize(calendar.FromDateTime(result))} ({result:dddd})");
    }

    private static ToolResult ExecuteDiff(CalendarBase calendar, Dictionary<string, object> parameters)
    {
        DateTime? date1 = GetDateParameter(parameters, "date", calendar);
        DateTime? date2 = GetDateParameter(parameters, "date2", calendar);

        if (date1 == null || date2 == null)
            return ToolResult.Failed("Both 'date' and 'date2' are required for diff.");

        TimeSpan diff = date2.Value - date1.Value;
        int totalDays = (int)diff.TotalDays;
        string sign = totalDays < 0 ? "-" : "";

        return ToolResult.Successful(
            $"Calendar: {calendar.CalendarName}\n" +
            $"From: {calendar.Localize(calendar.FromDateTime(date1.Value))}\n" +
            $"To:   {calendar.Localize(calendar.FromDateTime(date2.Value))}\n" +
            $"Difference: {sign}{Math.Abs(totalDays)} days");
    }

    private static ToolResult ExecuteGetComponents(CalendarBase calendar)
    {
        var components = calendar.GetComponents();
        var lines = components.Select(kv => $"  {kv.Key,-20} {kv.Value}");
        return ToolResult.Successful(
            $"Calendar: {calendar.CalendarName}\n" +
            $"Components:\n{string.Join("\n", lines)}");
    }

    private static ToolResult ExecuteGetNowComponents(CalendarBase calendar)
    {
        DateTime now = DateTime.Now;
        var components = calendar.FromDateTime(now);
        var componentDefs = calendar.GetComponents();
        var lines = components.Select(kv =>
        {
            string displayName = componentDefs.TryGetValue(kv.Key, out var name) ? name : kv.Key;
            string localized = calendar.TryGetComponentValueLocalization(kv.Key, kv.Value, out var loc) && loc != null
                ? $" ({loc})"
                : "";
            return $"  {displayName}: {kv.Value}{localized}";
        });
        return ToolResult.Successful(
            $"Calendar: {calendar.CalendarName}\n" +
            $"Current date/time components ({now:yyyy-MM-dd HH:mm:ss}):\n{string.Join("\n", lines)}");
    }

    private static ToolResult ExecuteConvert(Dictionary<string, CalendarBase> registry,
        Dictionary<string, object> parameters)
    {
        string srcId = parameters.TryGetValue("calendar", out var srcObj)
            ? srcObj?.ToString() ?? "gregorian" : "gregorian";
        string dstId = parameters.TryGetValue("calendar2", out var dstObj)
            ? dstObj?.ToString() ?? "" : "";

        if (string.IsNullOrEmpty(dstId))
            return ToolResult.Failed("Missing 'calendar2' parameter for convert.");
        if (!registry.TryGetValue(srcId, out CalendarBase? src))
            return ToolResult.Failed($"Unknown source calendar '{srcId}'.");
        if (!registry.TryGetValue(dstId, out CalendarBase? dst))
            return ToolResult.Failed($"Unknown target calendar '{dstId}'.");

        DateTime? date = GetDateParameter(parameters, "date", src);
        if (date == null)
            return ToolResult.Failed("Missing or unparseable 'date' parameter for convert.");

        var srcComponents = src.FromDateTime(date.Value);
        var dstComponents = dst.FromDateTime(date.Value);

        return ToolResult.Successful(
            $"Source  [{src.CalendarName}]: {src.Localize(srcComponents)} ({src.Format(srcComponents)})\n" +
            $"Target  [{dst.CalendarName}]: {dst.Localize(dstComponents)} ({dst.Format(dstComponents)})");
    }

    // ── helpers ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Gets the current localization provider for calendar systems.
    /// </summary>
    /// <returns>The DefaultLocalizationBase instance for the current language.</returns>
    public static DefaultLocalizationBase GetLocalization()
    {
        Language language = Config.Instance?.Data?.Language ?? Language.ZhCN;
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc;

        return LocalizationManager.Instance.GetLocalization(Language.ZhCN) as DefaultLocalizationBase
               ?? throw new InvalidOperationException("No DefaultLocalizationBase registered.");
    }

    /// <summary>
    /// Resolves a date string to a <see cref="DateTime"/>.
    /// First tries <see cref="CalendarBase.Parse"/> (native calendar format),
    /// then falls back to ISO / common formats via <see cref="DateTime.TryParse"/>.
    /// </summary>
    private static DateTime? GetDateParameter(Dictionary<string, object> parameters, string key,
        CalendarBase? calendar = null)
    {
        if (!parameters.TryGetValue(key, out object? dateObj) || dateObj == null)
            return null;

        string dateStr = dateObj.ToString()!;

        // 1. Try the calendar's own Parse (native format, e.g. "正月初一" for lunar)
        if (calendar != null)
        {
            try
            {
                var components = calendar.Parse(dateStr);
                // Parse succeeded — but CalendarBase.Parse returns components, not DateTime.
                // We need the calendar to convert back; use Format → DateTime.Parse as bridge.
                string iso = calendar.Format(components);
                if (DateTime.TryParse(iso, out DateTime fromCalendar))
                    return fromCalendar;
            }
            catch { /* fall through */ }
        }

        // 2. Standard ISO / common formats
        if (DateTime.TryParse(dateStr, out DateTime date))
            return date;

        string[] formats = { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd", "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss" };
        if (DateTime.TryParseExact(dateStr, formats, null,
                System.Globalization.DateTimeStyles.None, out date))
            return date;

        return null;
    }
}
