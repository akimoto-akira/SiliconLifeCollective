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
/// Implements the Inuit traditional calendar system.
/// 13 months: first 12 have 28 days, 13th has 29 days (30 in leap years).
/// Epoch: JDN 1721426 (January 1, 1 CE).
/// Leap year follows Gregorian rules.
/// </summary>
public class InuitCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    private const int Epoch = 1721426;

    private const string Year   = "year";
    private const string Month  = "month";
    private const string Day    = "day";
    private const string Hour   = "hour";
    private const string Minute = "minute";
    private const string Second = "second";

    /// <inheritdoc/>
    public override string CalendarId   => "inuit";

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarInuitName;

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
    public override Dictionary<string, int> FromDateTime(DateTime dt)
    {
        int jdn = GregorianToJdn(dt.Year, dt.Month, dt.Day);
        var (year, month, day) = FromJdn(jdn);
        return new()
        {
            { Year,   year      },
            { Month,  month     },
            { Day,    day       },
            { Hour,   dt.Hour   },
            { Minute, dt.Minute },
            { Second, dt.Second }
        };
    }

    /// <inheritdoc/>
    public override bool TryGetComponentValueLocalization(string componentId, int value, out string? result)
    {
        result = componentId switch
        {
            Year   => loc.FormatInuitYear(value),
            Month  => loc.GetInuitMonthName(value),
            Day    => loc.FormatInuitDay(value),
            Hour   => loc.FormatGregorianHour(value),
            Minute => loc.FormatGregorianMinute(value),
            Second => loc.FormatGregorianSecond(value),
            _      => null
        };
        return result != null;
    }

    /// <summary>Formats components as <c>yyyy-MM-dd HH:mm:ss</c>.</summary>
    public override string Format(Dictionary<string, int> c)
    {
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return $"{c[Year]:D4}-{c[Month]:D2}-{c[Day]:D2} {h:D2}:{mi:D2}:{s:D2}";
    }

    /// <summary>Parses a <c>yyyy-MM-dd HH:mm:ss</c> or <c>yyyy-MM-dd</c> string back into components.</summary>
    public override Dictionary<string, int> Parse(string value)
    {
        var formats = new[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd" };
        var dt = DateTime.ParseExact(value, formats,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None);
        return FromDateTime(dt);
    }

    /// <inheritdoc/>
    public override string Localize(Dictionary<string, int> c)
    {
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return loc.LocalizeInuitDate(c[Year], c[Month], c[Day], h, mi, s);
    }
}
