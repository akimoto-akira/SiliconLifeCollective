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
/// Implements the Julian calendar system.
/// </summary>
public class JulianCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    private const string Year   = "year";
    private const string Month  = "month";
    private const string Day    = "day";
    private const string Hour   = "hour";
    private const string Minute = "minute";
    private const string Second = "second";

    /// <inheritdoc/>
    public override string CalendarId   => "julian";

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarJulianName;

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

    private static int ToJdn(int year, int month, int day)
    {
        int a = (14 - month) / 12;
        int y = year + 4800 - a;
        int m = month + 12 * a - 3;
        return day + (153 * m + 2) / 5 + 365 * y + y / 4 - 32083;
    }

    private static (int year, int month, int day) FromJdn(int jdn)
    {
        int c = jdn + 32082;
        int d = (4 * c + 3) / 1461;
        int e = c - (1461 * d) / 4;
        int m = (5 * e + 2) / 153;
        int day   = e - (153 * m + 2) / 5 + 1;
        int month = m + 3 - 12 * (m / 10);
        int year  = d - 4800 + m / 10;
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
    public override bool TryGetComponentValueLocalization(string componentId, int componentValue, out string? result)
    {
        result = componentId switch
        {
            Year   => loc.FormatJulianYear(componentValue),
            Month  => loc.GetGregorianMonthName(componentValue),
            Day    => loc.FormatJulianDay(componentValue),
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
    /// Returns a localized human-readable string via <see cref="DefaultLocalizationBase.LocalizeJulianDate"/>.
    /// </summary>
    public override string Localize(Dictionary<string, int> c)
    {
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return loc.LocalizeJulianDate(c[Year], c[Month], c[Day], h, mi, s);
    }
}
