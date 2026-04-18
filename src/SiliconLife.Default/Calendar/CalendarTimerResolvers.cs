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

namespace SiliconLife.Default;

/// <summary>
/// Factory methods for creating calendar-based timer delegates.
/// Bridges TimerSystem (Core) with CalendarBase (Default) by injecting
/// resolver/converter/checker delegates. All calendar-specific resolution
/// uses <see cref="CalendarBase.FromDateTime"/> generically — no calendar
/// gets special treatment. The only exception is the virtual "interval" calendar.
/// </summary>
public static class CalendarTimerResolvers
{
    private static readonly HashSet<string> TimeComponentKeys = new()
    {
        "hour", "minute", "second"
    };

    /// <summary>
    /// Creates a CalendarNextOccurrenceResolver that uses the CalendarBase registry.
    /// For real calendars, uses day-by-day search via <see cref="CalendarBase.FromDateTime"/>.
    /// For the virtual "interval" calendar, computes directly from TimeSpan conditions.
    /// </summary>
    /// <param name="registryFactory">Factory function that builds the calendar registry</param>
    /// <returns>A resolver delegate</returns>
    public static CalendarNextOccurrenceResolver CreateResolver(
        Func<Dictionary<string, CalendarBase>> registryFactory)
    {
        return (DateTime anchor, string calendarId,
                Dictionary<string, int> conditions) =>
        {
            if (calendarId == "interval")
            {
                return ResolveInterval(anchor, conditions);
            }

            Dictionary<string, CalendarBase> registry = registryFactory();
            if (!registry.TryGetValue(calendarId, out CalendarBase? calendar))
                return null;

            return ResolveCalendar(anchor, calendar, conditions);
        };
    }

    /// <summary>
    /// Creates a CalendarDateTimeConverter that uses the CalendarBase registry.
    /// </summary>
    /// <param name="registryFactory">Factory function that builds the calendar registry</param>
    /// <returns>A converter delegate</returns>
    public static CalendarDateTimeConverter CreateConverter(
        Func<Dictionary<string, CalendarBase>> registryFactory)
    {
        return (DateTime dateTime, string calendarId) =>
        {
            if (calendarId == "interval")
            {
                return new Dictionary<string, int>();
            }

            Dictionary<string, CalendarBase> registry = registryFactory();
            if (!registry.TryGetValue(calendarId, out CalendarBase? calendar))
                return null;

            return calendar.FromDateTime(dateTime);
        };
    }

    /// <summary>
    /// Creates a TimerPendingChecker with standard pending logic.
    /// </summary>
    /// <returns>A pending checker delegate</returns>
    public static TimerPendingChecker CreatePendingChecker()
    {
        return (TimerItem timer) =>
        {
            if (timer.Status != TimerStatus.Active)
                return false;

            return timer.ShouldTrigger();
        };
    }

    private static DateTime? ResolveInterval(
        DateTime anchor,
        Dictionary<string, int> conditions)
    {
        int days = conditions.TryGetValue("days", out int d) ? d : 0;
        int hours = conditions.TryGetValue("hours", out int h) ? h : 0;
        int minutes = conditions.TryGetValue("minutes", out int m) ? m : 0;
        int seconds = conditions.TryGetValue("seconds", out int s) ? s : 0;

        TimeSpan interval = new TimeSpan(days, hours, minutes, seconds);
        if (interval <= TimeSpan.Zero)
            return null;

        return anchor + interval;
    }

    private static DateTime? ResolveCalendar(
        DateTime anchor,
        CalendarBase calendar,
        Dictionary<string, int> conditions)
    {
        int hour = conditions.TryGetValue("hour", out int h) ? h : 0;
        int minute = conditions.TryGetValue("minute", out int m) ? m : 0;
        int second = conditions.TryGetValue("second", out int s) ? s : 0;
        TimeSpan tod = new TimeSpan(hour, minute, second);

        if (IsTimeOnlyCondition(conditions))
        {
            DateTime todayAtTod = anchor.Date.Add(tod);
            if (todayAtTod > anchor)
                return todayAtTod;
            return anchor.Date.AddDays(1).Add(tod);
        }

        DateTime candidate = anchor.Date.AddDays(1);
        DateTime limit = anchor.Date.AddDays(400);

        while (candidate <= limit)
        {
            Dictionary<string, int> components = calendar.FromDateTime(candidate);

            if (MatchesDateConditions(components, conditions))
            {
                return candidate.Date.Add(tod);
            }

            candidate = candidate.AddDays(1);
        }

        return null;
    }

    private static bool IsTimeOnlyCondition(Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> kv in conditions)
        {
            if (!TimeComponentKeys.Contains(kv.Key))
                return false;
        }
        return conditions.Count > 0;
    }

    private static bool MatchesDateConditions(
        Dictionary<string, int> components,
        Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> kv in conditions)
        {
            if (TimeComponentKeys.Contains(kv.Key))
                continue;

            if (!components.TryGetValue(kv.Key, out int value))
                return false;

            if (value != kv.Value)
                return false;
        }

        return true;
    }
}
