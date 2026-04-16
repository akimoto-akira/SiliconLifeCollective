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
/// Implements the Vikram Samvat calendar system.
/// Epoch: JDN 1700709. VS year = Gregorian year + 57.
/// </summary>
public class VikramSamvatCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    private const int Epoch = 1700709;

    private const string Year   = "year";
    private const string Month  = "month";
    private const string Day    = "day";
    private const string Hour   = "hour";
    private const string Minute = "minute";
    private const string Second = "second";

    /// <inheritdoc/>
    public override string CalendarId   => "vikram_samvat";

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarVikramSamvatName;

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

    private static bool IsLeapYear(int vsYear)
    {
        int gregYear = vsYear - 57;
        return gregYear % 4 == 0 && (gregYear % 100 != 0 || gregYear % 400 == 0);
    }

    private static int DaysInMonth(int vsYear, int month)
    {
        if (month <= 6) return 31;
        if (month <= 11) return 30;
        return IsLeapYear(vsYear) ? 30 : 29;
    }

    private static int CountLeapYears(int year)
    {
        int count = 0;
        for (int y = 1; y <= year; y++)
            if (IsLeapYear(y)) count++;
        return count;
    }

    private static int ToJdn(int year, int month, int day)
    {
        int daysInCompleteMonths = 0;
        for (int m = 1; m < month; m++)
            daysInCompleteMonths += DaysInMonth(year, m);
        return Epoch + CountLeapYears(year - 1) + (year - 1) * 365 + daysInCompleteMonths + day - 1;
    }

    private static (int year, int month, int day) FromJdn(int jdn)
    {
        int year = Math.Max(1, (jdn - Epoch) / 365 + 1);
        while (ToJdn(year + 1, 1, 1) <= jdn) year++;
        while (ToJdn(year, 1, 1) > jdn) year--;
        int month = 1;
        while (month < 12 && ToJdn(year, month + 1, 1) <= jdn) month++;
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
            Year   => loc.FormatVikramSamvatYear(value),
            Month  => loc.GetIndianMonthName(value),
            Day    => loc.FormatVikramSamvatDay(value),
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
        return loc.LocalizeVikramSamvatDate(c[Year], c[Month], c[Day], h, mi, s);
    }
}
