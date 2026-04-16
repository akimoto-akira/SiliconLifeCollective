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
/// Implements the Indian National Calendar (Saka) system.
/// Epoch: JDN 1749995 (March 22, 79 CE). Saka year = Gregorian year - 78.
/// </summary>
public class IndianCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    private const int Epoch = 1749995;

    private const string Year   = "year";
    private const string Month  = "month";
    private const string Day    = "day";
    private const string Hour   = "hour";
    private const string Minute = "minute";
    private const string Second = "second";

    /// <inheritdoc/>
    public override string CalendarId   => "indian";

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarIndianName;

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

    private static bool IsGregorianLeap(int gregorianYear)
        => gregorianYear % 4 == 0 && (gregorianYear % 100 != 0 || gregorianYear % 400 == 0);

    private static bool IsLeapYear(int sakaYear) => IsGregorianLeap(sakaYear + 78);

    private static int DaysInMonth(int sakaYear, int month)
    {
        if (month == 1) return IsLeapYear(sakaYear) ? 31 : 30;
        if (month <= 6) return 31;
        return 30;
    }

    private static int GregorianToJdn(int year, int month, int day)
    {
        int a = (14 - month) / 12;
        int y = year + 4800 - a;
        int m = month + 12 * a - 3;
        return day + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045;
    }

    private static int GetSakaNewYearJdn(int sakaYear)
    {
        int gregYear = sakaYear + 78;
        int newYearDay = IsGregorianLeap(gregYear) ? 21 : 22;
        return GregorianToJdn(gregYear, 3, newYearDay);
    }

    private static int ToJdn(int sakaYear, int month, int day)
    {
        int dayOfYear = 0;
        for (int m = 1; m < month; m++)
            dayOfYear += DaysInMonth(sakaYear, m);
        return GetSakaNewYearJdn(sakaYear) + dayOfYear + day - 1;
    }

    private static (int year, int month, int day) FromJdn(int jdn)
    {
        int sakaYear = (jdn - Epoch) / 365 + 1;
        while (GetSakaNewYearJdn(sakaYear + 1) <= jdn) sakaYear++;
        while (GetSakaNewYearJdn(sakaYear) > jdn) sakaYear--;
        int month = 1;
        while (month < 12 && ToJdn(sakaYear, month + 1, 1) <= jdn) month++;
        int day = jdn - ToJdn(sakaYear, month, 1) + 1;
        return (sakaYear, month, day);
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
            Year   => loc.FormatIndianYear(value),
            Month  => loc.GetIndianMonthName(value),
            Day    => loc.FormatIndianDay(value),
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
    /// Returns a localized human-readable string via <see cref="DefaultLocalizationBase.LocalizeIndianDate"/>.
    /// </summary>
    public override string Localize(Dictionary<string, int> c)
    {
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return loc.LocalizeIndianDate(c[Year], c[Month], c[Day], h, mi, s);
    }
}
