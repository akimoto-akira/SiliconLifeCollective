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
/// Pure logic with no external dependencies — used to verify the tool system pipeline.
/// </summary>
public class CalendarTool : ITool
{
    public string Name => "calendar";

    public string Description =>
        "Get current date/time information, day of week, and perform date calculations. " +
        "Actions: 'now' (current date/time), 'day_of_week' (current day of week), " +
        "'format' (format a date), 'add_days' (add days to a date), 'diff' (difference between dates).";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The action to perform: now, day_of_week, add_days, diff, format",
                    ["enum"] = new[] { "now", "day_of_week", "add_days", "diff", "format" }
                },
                ["date"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "A date string (format: yyyy-MM-dd or yyyy-MM-dd HH:mm:ss), used with day_of_week, add_days, diff, format"
                },
                ["days"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Number of days to add (positive or negative), used with add_days"
                },
                ["date2"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Second date for diff calculation (format: yyyy-MM-dd)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj?.ToString() ?? "";

        try
        {
            return action.ToLowerInvariant() switch
            {
                "now" => ExecuteNow(),
                "day_of_week" => ExecuteDayOfWeek(parameters),
                "add_days" => ExecuteAddDays(parameters),
                "diff" => ExecuteDiff(parameters),
                "format" => ExecuteFormat(parameters),
                _ => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Calendar error: {ex.Message}");
        }
    }

    private ToolResult ExecuteNow()
    {
        DateTime now = DateTime.Now;
        string result = $"Current date and time: {now:yyyy-MM-dd HH:mm:ss}\n" +
            $"Day of week: {now:dddd}\n" +
            $"Unix timestamp: {new DateTimeOffset(now).ToUnixTimeSeconds()}\n" +
            $"UTC: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}";
        return ToolResult.Successful(result);
    }

    private ToolResult ExecuteDayOfWeek(Dictionary<string, object> parameters)
    {
        DateTime date = GetDateParameter(parameters, "date") ?? DateTime.Now;
        return ToolResult.Successful($"{date:yyyy-MM-dd} is {date:dddd} (day {(int)date.DayOfWeek + 1} of the week)");
    }

    private ToolResult ExecuteAddDays(Dictionary<string, object> parameters)
    {
        DateTime date = GetDateParameter(parameters, "date") ?? DateTime.Now;

        if (!parameters.TryGetValue("days", out object? daysObj) || daysObj == null)
        {
            return ToolResult.Failed("Missing 'days' parameter for add_days");
        }

        if (!int.TryParse(daysObj.ToString(), out int days))
        {
            return ToolResult.Failed("'days' must be an integer");
        }

        DateTime result = date.AddDays(days);
        return ToolResult.Successful($"Adding {days} days to {date:yyyy-MM-dd} = {result:yyyy-MM-dd} ({result:dddd})");
    }

    private ToolResult ExecuteDiff(Dictionary<string, object> parameters)
    {
        DateTime? date1 = GetDateParameter(parameters, "date");
        DateTime? date2 = GetDateParameter(parameters, "date2");

        if (date1 == null || date2 == null)
        {
            return ToolResult.Failed("Both 'date' and 'date2' are required for diff");
        }

        TimeSpan diff = date2.Value - date1.Value;
        string sign = diff.TotalDays < 0 ? "-" : "";
        int absDays = Math.Abs((int)diff.TotalDays);

        return ToolResult.Successful(
            $"Difference between {date1.Value:yyyy-MM-dd} and {date2.Value:yyyy-MM-dd}: {sign}{absDays} days");
    }

    private ToolResult ExecuteFormat(Dictionary<string, object> parameters)
    {
        DateTime date = GetDateParameter(parameters, "date") ?? DateTime.Now;
        return ToolResult.Successful($"Formatted: {date:yyyy-MM-dd dddd HH:mm:ss}");
    }

    private static DateTime? GetDateParameter(Dictionary<string, object> parameters, string key)
    {
        if (!parameters.TryGetValue(key, out object? dateObj) || dateObj == null)
        {
            return null;
        }

        string dateStr = dateObj.ToString()!;
        if (DateTime.TryParse(dateStr, out DateTime date))
        {
            return date;
        }

        // Try common formats
        string[] formats = { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd", "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss" };
        if (DateTime.TryParseExact(dateStr, formats, null, System.Globalization.DateTimeStyles.None, out date))
        {
            return date;
        }

        return null;
    }
}
