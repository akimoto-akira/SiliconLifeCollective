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

using System.Text.RegularExpressions;

namespace SiliconLife.Collective;

/// <summary>
/// TickObject wrapper for auto-trigger skills (TriggerMode = Auto,
/// AutoTriggerCondition = "schedule"). Checks the schedule condition on each
/// tick and executes the skill on a background task when due.
/// Supported schedule formats (skill.Metadata["schedule"]):
///   - "HH:mm"                        → daily at the given time
///   - "N m" / "N h" / "N d"          → every N minutes / hours / days
///   - "M H * * *" (cron subset)      → daily/periodic at H:M (day fields must be *)
/// </summary>
public class AutoSkillTickObject : TickObject
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<AutoSkillTickObject>();
    private static readonly Regex DailyTimeRegex = new(@"^(\d{1,2}):(\d{2})$", RegexOptions.Compiled);
    private static readonly Regex IntervalRegex = new(@"^(\d+)\s*(s|sec|m|min|h|hour|d|day)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex CronRegex = new(@"^(\S+)\s+(\S+)\s+\*\s+\*\s+\*$", RegexOptions.Compiled);

    private readonly SkillDefinition _skill;
    private readonly SiliconBeingBase _being;
    private readonly Func<DateTime, DateTime> _nextRunCalculator;
    private DateTime _nextRun;
    private volatile bool _isExecuting;

    /// <summary>Gets the id of the wrapped skill.</summary>
    public string SkillId => _skill.Id;

    /// <summary>
    /// Creates an auto-trigger tick object. Throws ArgumentException when the
    /// schedule format is not supported.
    /// </summary>
    /// <param name="skill">The skill definition (TriggerMode must be Auto)</param>
    /// <param name="being">The owning being</param>
    /// <param name="checkInterval">Schedule check interval (default 30s)</param>
    public AutoSkillTickObject(SkillDefinition skill, SiliconBeingBase being, TimeSpan? checkInterval = null)
        : base(checkInterval ?? TimeSpan.FromSeconds(30), autoRegister: true)
    {
        _skill = skill;
        _being = being;

        string schedule = GetSchedule(skill);
        if (!TryParseSchedule(schedule, out _nextRunCalculator))
        {
            throw new ArgumentException(
                $"Unsupported schedule format '{schedule}' for auto skill '{skill.Id}'. " +
                "Supported: \"HH:mm\", \"N m|h|d\", \"M H * * *\".");
        }

        _nextRun = _nextRunCalculator(DateTime.Now);
        _logger.Info(being.Id, "Auto skill '{0}' scheduled, first run at {1:yyyy-MM-dd HH:mm:ss}", skill.Id, _nextRun);
    }

    private static string GetSchedule(SkillDefinition skill)
    {
        if (skill.Metadata.TryGetValue("schedule", out var value) && value != null)
        {
            return value.ToString() ?? "";
        }
        throw new ArgumentException($"Auto skill '{skill.Id}' has no 'schedule' entry in Metadata.");
    }

    /// <summary>
    /// Parses a schedule expression into a next-run calculator.
    /// Supported: "HH:mm" (daily), "N s|m|h|d" (interval), "M H * * *" (cron time-of-day subset).
    /// </summary>
    internal static bool TryParseSchedule(string schedule, out Func<DateTime, DateTime> calculator)
    {
        schedule = schedule.Trim();

        var daily = DailyTimeRegex.Match(schedule);
        if (daily.Success)
        {
            int hour = int.Parse(daily.Groups[1].Value);
            int minute = int.Parse(daily.Groups[2].Value);
            if (hour < 24 && minute < 60)
            {
                calculator = now =>
                {
                    var candidate = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
                    return candidate > now ? candidate : candidate.AddDays(1);
                };
                return true;
            }
        }

        var interval = IntervalRegex.Match(schedule);
        if (interval.Success)
        {
            int amount = int.Parse(interval.Groups[1].Value);
            TimeSpan span = interval.Groups[2].Value.ToLowerInvariant() switch
            {
                "s" or "sec" => TimeSpan.FromSeconds(amount),
                "m" or "min" => TimeSpan.FromMinutes(amount),
                "h" or "hour" => TimeSpan.FromHours(amount),
                "d" or "day" => TimeSpan.FromDays(amount),
                _ => TimeSpan.FromMinutes(amount)
            };
            calculator = now => now + span;
            return true;
        }

        var cron = CronRegex.Match(schedule);
        if (cron.Success)
        {
            string minuteField = cron.Groups[1].Value;
            string hourField = cron.Groups[2].Value;

            // "*/n" step or "*" or specific value
            if (TryParseCronField(minuteField, out int? minuteStep, out int? minuteValue) &&
                TryParseCronField(hourField, out int? hourStep, out int? hourValue))
            {
                calculator = now => ComputeCronNext(now, minuteStep, minuteValue, hourStep, hourValue);
                return true;
            }
        }

        calculator = _ => DateTime.MaxValue;
        return false;
    }

    private static bool TryParseCronField(string field, out int? step, out int? value)
    {
        step = null;
        value = null;

        if (field == "*") return true;

        if (field.StartsWith("*/") && int.TryParse(field[2..], out int s) && s > 0)
        {
            step = s;
            return true;
        }

        if (int.TryParse(field, out int v)) { value = v; return true; }

        return false;
    }

    private static DateTime ComputeCronNext(DateTime now, int? minuteStep, int? minuteValue, int? hourStep, int? hourValue)
    {
        // Interval-based minute step (e.g. */15 * * * *)
        if (minuteStep.HasValue && hourStep == null && hourValue == null)
        {
            return now.AddMinutes(minuteStep.Value);
        }
        if (hourStep.HasValue)
        {
            return now.AddHours(hourStep.Value);
        }

        // Daily at H:M (e.g. 0 9 * * *)
        if (minuteValue.HasValue || hourValue.HasValue)
        {
            int hour = hourValue ?? 0;
            int minute = minuteValue ?? 0;
            if (hour < 24 && minute < 60)
            {
                var candidate = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
                return candidate > now ? candidate : candidate.AddDays(1);
            }
        }

        return now.AddMinutes(1);
    }

    /// <summary>
    /// Called by MainLoop: triggers the skill when the schedule condition is met.
    /// Execution runs on a background task so a long-running skill does not
    /// block the main loop.
    /// </summary>
    protected override void OnTick(TimeSpan deltaTime)
    {
        if (_isExecuting || DateTime.Now < _nextRun)
        {
            return;
        }

        _nextRun = _nextRunCalculator(DateTime.Now);

        if (_being.SkillManager == null || !SkillManager.SkillEnabled)
        {
            return;
        }

        _isExecuting = true;
        _logger.Info(_being.Id, "Auto skill '{0}' triggered by schedule", _skill.Id);

        _ = Task.Run(() =>
        {
            try
            {
                _being.SkillManager!.ExecuteSkill(_skill.Id, null, _being);
            }
            catch (Exception ex)
            {
                _logger.Error(_being.Id, "Auto skill '{0}' execution failed: {1}", _skill.Id, ex.Message, ex);
            }
            finally
            {
                _isExecuting = false;
            }
        });
    }
}
