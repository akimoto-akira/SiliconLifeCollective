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
/// Implements the Cherokee traditional calendar system.
/// 13 months: first 12 have 28 days, 13th has 29 days (30 in leap years).
/// Epoch: JDN 1721426 (January 1, 1 CE).
/// Leap year follows Gregorian rules.
/// </summary>
public class CherokeeCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    private const int Epoch = 1721426;

    private const string Year   = "year";
    private const string Month  = "month";
    private const string Day    = "day";
    private const string Hour   = "hour";
    private const string Minute = "minute";
    private const string Second = "second";

    /// <inheritdoc/>
    public override string CalendarId   => "cherokee";

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarCherokeeName;

    /// <inheritdoc/>
    public override Dictionary<string, string> GetComponents() => new()
    {
        { Year,   loc.CalendarComponentYear   },
        { Month,  loc.CalendarComponentMonth  },
        { Day,    loc.CalendarComponentDay    },
        { Hour,   loc.CalendarComponentHour   },
        { Minute, loc.CalendarComponentMinute },
        { Second, loc.CalendarComponentSecond }
    };

    private static bool IsLeapYear(int year)
        => year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);

    private static int DaysInMonth(int year, int month)
    {
        if (month < 13) return 28;
        return IsLeapYear(year) ? 30 : 29;
    }

    private static int DaysInYear(int year) => IsLeapYear(year) ? 366 : 365;

    private static int CountLeapYears(int year)
    {
        if (year <= 0) return 0;
        return year / 4 - year / 100 + year / 400;
    }

    private static int GregorianToJdn(int year, int month, int day)
    {
        int a = (14 - month) / 12;
        int y = year + 4800 - a;
        int m = month + 12 * a - 3;
        return day + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045;
    }

    private static (int year, int month, int day) FromJdn(int jdn)
    {
        int remaining = jdn - Epoch;
        int year = 1;
        while (true)
        {
            int diy = DaysInYear(year);
            if (remaining < diy) break;
            remaining -= diy;
            year++;
        }
        int month = 1;
        while (month <= 13)
        {
            int dim = DaysInMonth(year, month);
            if (remaining < dim) break;
            remaining -= dim;
            month++;
        }
        return (year, month, remaining + 1);
    }

    /// <inheritdoc/>
    public override Dictionary<string, int> FromDateTime(DateTime dateTime)
    {
        int jdn = GregorianToJdn(dateTime.Year, dateTime.Month, dateTime.Day);
        var (year, month, day) = FromJdn(jdn);
        return new()
        {
            { Year,   year            },
            { Month,  month           },
            { Day,    day             },
            { Hour,   dateTime.Hour   },
            { Minute, dateTime.Minute },
            { Second, dateTime.Second }
        };
    }

    /// <inheritdoc/>
    public override bool TryGetComponentValueLocalization(string componentId, int componentValue, out string? result)
    {
        result = componentId switch
        {
            Month  => loc.GetCherokeeMonthName(componentValue),
            Year   => loc.FormatCherokeeYear(componentValue),
            Day    => loc.FormatCherokeeDay(componentValue),
            Hour   => loc.FormatGregorianHour(componentValue),
            Minute => loc.FormatGregorianMinute(componentValue),
            Second => loc.FormatGregorianSecond(componentValue),
            _      => null
        };
        return result != null;
    }

    /// <summary>
    /// Formats components as <c>yyyy-MM-dd HH:mm:ss</c>.
    /// </summary>
    public override string Format(Dictionary<string, int> components)
    {
        var h  = components.GetValueOrDefault(Hour,   0);
        var mi = components.GetValueOrDefault(Minute, 0);
        var s  = components.GetValueOrDefault(Second, 0);
        return $"{components[Year]:D4}-{components[Month]:D2}-{components[Day]:D2} {h:D2}:{mi:D2}:{s:D2}";
    }

    /// <summary>
    /// Parses a <c>yyyy-MM-dd HH:mm:ss</c> or <c>yyyy-MM-dd</c> string back into components.
    /// </summary>
    public override Dictionary<string, int> Parse(string value)
    {
        var parts = value.Split('-', ' ', ':');
        return new()
        {
            { Year,   int.Parse(parts[0]) },
            { Month,  int.Parse(parts[1]) },
            { Day,    int.Parse(parts[2]) },
            { Hour,   parts.Length > 3 ? int.Parse(parts[3]) : 0 },
            { Minute, parts.Length > 4 ? int.Parse(parts[4]) : 0 },
            { Second, parts.Length > 5 ? int.Parse(parts[5]) : 0 }
        };
    }

    /// <summary>
    /// Returns a localized human-readable string via <see cref="DefaultLocalizationBase.LocalizeCherokeeDate"/>.
    /// </summary>
    public override string Localize(Dictionary<string, int> components)
    {
        var h  = components.GetValueOrDefault(Hour,   0);
        var mi = components.GetValueOrDefault(Minute, 0);
        var s  = components.GetValueOrDefault(Second, 0);
        return loc.LocalizeCherokeeDate(components[Year], components[Month], components[Day], h, mi, s);
    }
}
