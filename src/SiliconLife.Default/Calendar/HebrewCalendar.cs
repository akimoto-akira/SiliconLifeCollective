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
/// Implements the Hebrew (Jewish) calendar system.
/// Epoch: JDN 347998.
/// </summary>
public class HebrewCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    private const int Epoch = 347998;

    private const string Year   = "year";
    private const string Month  = "month";
    private const string Day    = "day";
    private const string Hour   = "hour";
    private const string Minute = "minute";
    private const string Second = "second";

    /// <inheritdoc/>
    public override string CalendarId   => "hebrew";

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarHebrewName;

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
    {
        int r = (year - 1) % 19;
        return r is 2 or 5 or 7 or 10 or 13 or 16 or 18;
    }

    private static int MonthsInYear(int year) => IsLeapYear(year) ? 13 : 12;

    private static int DaysInYear(int year) => ElapsedDays(year + 1) - ElapsedDays(year);

    private static int DaysInMonth(int year, int month)
    {
        int monthsCount = MonthsInYear(year);
        if (month < 1 || month > monthsCount) return 0;

        if (IsLeapYear(year))
        {
            int[] days = [0, 30, 29, 30, 29, 30, 30, 29, 30, 29, 30, 29, 30, 29];
            if (month == 2) { int diy = DaysInYear(year); if (diy == 355 || diy == 385) return 30; }
            if (month == 3) { int diy = DaysInYear(year); if (diy == 353 || diy == 383) return 29; }
            return days[month];
        }
        else
        {
            int[] days = [0, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29];
            if (month == 2) { int diy = DaysInYear(year); if (diy == 355) return 30; }
            if (month == 3) { int diy = DaysInYear(year); if (diy == 353) return 29; }
            return days[month];
        }
    }

    private static int ElapsedDays(int year)
    {
        int cycles = (year - 1) / 19;
        int monthsElapsed = 235 * cycles;
        int yearsInCycle = (year - 1) % 19;
        for (int y = 1; y <= yearsInCycle; y++)
            monthsElapsed += IsLeapYear(y) ? 13 : 12;
        long parts = (long)monthsElapsed * 765433;
        int days = (int)(parts / 25920);
        int partsInDay = (int)(parts % 25920);
        int dow = days % 7;
        if (partsInDay >= 19440) { days++; dow = (dow + 1) % 7; }
        if (dow == 6 || dow == 2 || dow == 4) { days++; dow = (dow + 1) % 7; }
        if (dow == 1 && partsInDay >= 9924 && !IsLeapYear(year)) days += 2;
        if (dow == 0 && partsInDay >= 16789 && !IsLeapYear(year - 1)) days++;
        return days;
    }

    private static int ToJdn(int year, int month, int day)
    {
        int dayOfYear = 0;
        for (int m = 1; m < month; m++)
            dayOfYear += DaysInMonth(year, m);
        return Epoch + ElapsedDays(year) + dayOfYear + day - 1;
    }

    private static (int year, int month, int day) FromJdn(int jdn)
    {
        int year = Math.Max(1, (jdn - Epoch) / 366 + 1);
        while (ToJdn(year + 1, 1, 1) <= jdn) year++;
        while (ToJdn(year, 1, 1) > jdn) year--;
        int month = 1;
        int monthsCount = MonthsInYear(year);
        while (month < monthsCount && ToJdn(year, month + 1, 1) <= jdn) month++;
        int day = jdn - ToJdn(year, month, 1) + 1;
        return (year, month, day);
    }

    private static int GregorianToJdn(int year, int month, int day)
    {
        int a = (14 - month) / 12;
        int y = year + 4800 - a;
        int m = month + 12 * a - 3;
        return day + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045;
    }

    /// <inheritdoc/>
    public override Dictionary<string, int> FromDateTime(DateTime dateTime)
    {
        int jdn = GregorianToJdn(dateTime.Year, dateTime.Month, dateTime.Day);
        var (year, month, day) = FromJdn(jdn);
        return new()
        {
            { Year,   year              },
            { Month,  month             },
            { Day,    day               },
            { Hour,   dateTime.Hour     },
            { Minute, dateTime.Minute   },
            { Second, dateTime.Second   }
        };
    }

    /// <inheritdoc/>
    public override bool TryGetComponentValueLocalization(string componentId, int value, out string? result)
    {
        result = componentId switch
        {
            Year   => loc.FormatHebrewYear(value),
            Month  => loc.GetHebrewMonthName(value),
            Day    => loc.FormatHebrewDay(value),
            Hour   => loc.FormatGregorianHour(value),
            Minute => loc.FormatGregorianMinute(value),
            Second => loc.FormatGregorianSecond(value),
            _      => null
        };
        return result != null;
    }

    /// <summary>
    /// Formats components as <c>yyyy-MM-dd HH:mm:ss</c>.
    /// </summary>
    public override string Format(Dictionary<string, int> c)
    {
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return $"{c[Year]:D4}-{c[Month]:D2}-{c[Day]:D2} {h:D2}:{mi:D2}:{s:D2}";
    }

    /// <summary>
    /// Parses a <c>yyyy-MM-dd HH:mm:ss</c> or <c>yyyy-MM-dd</c> string back into components.
    /// </summary>
    public override Dictionary<string, int> Parse(string value)
    {
        var formats = new[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd" };
        var dt = DateTime.ParseExact(value, formats,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None);
        return FromDateTime(dt);
    }

    /// <summary>
    /// Returns a localized human-readable string via <see cref="DefaultLocalizationBase.LocalizeHebrewDate"/>.
    /// </summary>
    public override string Localize(Dictionary<string, int> c)
    {
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return loc.LocalizeHebrewDate(c[Year], c[Month], c[Day], h, mi, s);
    }
}
