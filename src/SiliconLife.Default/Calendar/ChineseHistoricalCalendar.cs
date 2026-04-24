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

namespace SiliconLife.Default;

/// <summary>
/// Chinese Historical Calendar implementation
/// 
/// This calendar system represents Chinese historical dating using dynasty names,
/// era names, and the Republic of China and Common Era periods.
/// 
/// Supported periods:
/// - Ancient dynasties (Xia, Shang, Zhou, Qin, Han, etc.)
/// - Medieval dynasties (Sui, Tang, Song, Yuan, Ming, Qing)
/// - Republic of China (1912-1949)
/// - People's Republic of China (1949-present)
/// 
/// Calendar code: CHC
/// </summary>
public class ChineseHistoricalCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    private const string Code = "chinese_historical";
    private const string Year = "year";
    private const string Month = "month";
    private const string Day = "day";
    private const string Dynasty = "dynasty";
    private const string Era = "era";
    private const string Hour = "hour";
    private const string Minute = "minute";
    private const string Second = "second";

    private readonly ChineseHistoricalEraDatabase _eraDatabase = new();

    /// <inheritdoc/>
    public override string CalendarId => Code;

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarChineseHistoricalName;

    /// <inheritdoc/>
    public override Dictionary<string, string> GetComponents() => new()
    {
        { Year, loc.CalendarComponentYear },
        { Month, loc.CalendarComponentMonth },
        { Day, loc.CalendarComponentDay },
        { Dynasty, loc.CalendarComponentDynasty },
        { Era, loc.CalendarComponentEra },
        { Hour, loc.CalendarComponentHour },
        { Minute, loc.CalendarComponentMinute },
        { Second, loc.CalendarComponentSecond }
    };

    /// <inheritdoc/>
    public override Dictionary<string, int> FromDateTime(DateTime dt)
    {
        var chineseDate = FromGregorian(dt.Year, dt.Month, dt.Day);
        return new Dictionary<string, int>
        {
            { Year, chineseDate.Year },
            { Month, chineseDate.Month },
            { Day, chineseDate.Day },
            { Dynasty, 0 }, // Dynasty is stored as string in metadata
            { Era, 0 }, // Era is stored as string in metadata
            { Hour, dt.Hour },
            { Minute, dt.Minute },
            { Second, dt.Second }
        };
    }

    /// <inheritdoc/>
    public override bool TryGetComponentValueLocalization(string componentId, int value, out string? result)
    {
        result = componentId switch
        {
            Year => null, // Year formatting depends on dynasty/era context
            Month => loc.GetChineseHistoricalMonthName(value),
            Day => loc.FormatChineseHistoricalDay(value),
            Hour => loc.FormatGregorianHour(value),
            Minute => loc.FormatGregorianMinute(value),
            Second => loc.FormatGregorianSecond(value),
            _ => null
        };
        return result != null;
    }

    /// <inheritdoc/>
    public override string Format(Dictionary<string, int> components)
    {
        var year = components.GetValueOrDefault(Year, 1);
        var month = components.GetValueOrDefault(Month, 1);
        var day = components.GetValueOrDefault(Day, 1);

        return $"{year}-{month:D2}-{day:D2}";
    }

    /// <inheritdoc/>
    public override Dictionary<string, int> Parse(string value)
    {
        var formats = new[] { "yyyy-MM-dd", "yyyy-M-d" };
        var dt = DateTime.ParseExact(value, formats,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None);
        return FromDateTime(dt);
    }

    /// <inheritdoc/>
    public override string Localize(Dictionary<string, int> components)
    {
        var year = components.GetValueOrDefault(Year, 1);
        var month = components.GetValueOrDefault(Month, 1);
        var day = components.GetValueOrDefault(Day, 1);

        var monthName = loc.GetChineseHistoricalMonthName(month);
        var dayName = loc.FormatChineseHistoricalDay(day);

        return $"{year}{loc.CalendarComponentYear}{monthName}{dayName}";
    }

    /// <summary>
    /// Convert Chinese historical date to Gregorian date
    /// </summary>
    private DateTime ToGregorian(int year, int month, int day, string dynasty, string? era)
    {
        var dynastyLower = dynasty.ToLowerInvariant();

        // Special handling for ROC and PRC
        if (dynastyLower is "roc" or "min-guo")
        {
            var gregorianYearRoc = 1911 + year;
            return new DateTime(gregorianYearRoc, month, day);
        }

        if (dynastyLower is "prc" or "gong-yuan")
        {
            return new DateTime(year, month, day);
        }

        // Historical dynasty with era name
        if (era == null)
            throw new ArgumentException("Era name is required for historical dynasties");

        var eraData = _eraDatabase.GetEra(dynasty, era);
        if (eraData == null)
            throw new ArgumentException($"Unknown era: {era} in dynasty {dynasty}");

        var firstYear = eraData.FirstYear;
        var gregorianYearHistorical = eraData.StartYear + (year - firstYear);

        return new DateTime(gregorianYearHistorical, month, day);
    }

    /// <summary>
    /// Convert Gregorian date to Chinese historical date
    /// </summary>
    private (int Year, int Month, int Day, string Dynasty, string? Era) FromGregorian(int year, int month, int day)
    {
        // Check for Common Era period (1949-10-01 onwards)
        if (year > 1949 || (year == 1949 && month >= 10))
        {
            return (year, month, day, "PRC", null);
        }

        // Check for ROC period (1912-1949-09-30)
        if (year >= 1912 && (year < 1949 || (year == 1949 && month < 10)))
        {
            var rocYear = year - 1911;
            return (rocYear, month, day, "ROC", null);
        }

        // Determine the lunar year for accurate era year calculation
        var lunarYear = GetLunarYearForDate(year, month, day);

        // Find the era for this lunar year
        var eraData = _eraDatabase.GetEraByGregorianDate(lunarYear, month, day);

        if (eraData == null)
        {
            return (year, month, day, "Unknown", null);
        }

        // Calculate year within the era using lunar year
        var firstYear = eraData.FirstYear;
        var eraYear = firstYear + (lunarYear - eraData.StartYear);

        return (eraYear, month, day, eraData.DynastyKey, eraData.EraKey);
    }

    /// <summary>
    /// Get lunar year for a given Gregorian date
    /// Uses simple approximation: dates before mid-February belong to previous lunar year
    /// </summary>
    private static int GetLunarYearForDate(int year, int month, int day)
    {
        if (month == 1)
            return year - 1;
        
        if (month == 2 && day < 10)
            return year - 1;

        return year;
    }

    /// <summary>
    /// Format ordinal number for English (1st, 2nd, 3rd, etc.)
    /// </summary>
    private static string FormatOrdinalNumber(int number)
    {
        if (number % 100 >= 11 && number % 100 <= 13)
            return $"{number}th";

        return (number % 10) switch
        {
            1 => $"{number}st",
            2 => $"{number}nd",
            3 => $"{number}rd",
            _ => $"{number}th"
        };
    }
}
