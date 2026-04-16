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
/// Implements the Japanese era (Nengo) calendar system.
/// Converts to/from Gregorian using era definitions.
/// Components: era, year (within era), month, day, hour, minute, second.
/// </summary>
public class JapaneseCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    private record Era(string Name, string Kanji, int StartYear, int StartMonth, int StartDay, int? EndYear, int? EndMonth, int? EndDay);

    private static readonly Era[] Eras =
    [
        new("Reiwa",  "令和", 2019, 5,  1,  null, null, null),
        new("Heisei", "平成", 1989, 1,  8,  2019, 4,   30),
        new("Showa",  "昭和", 1926, 12, 25, 1989, 1,   7),
        new("Taisho", "大正", 1912, 7,  30, 1926, 12,  24),
        new("Meiji",  "明治", 1868, 1,  25, 1912, 7,   29),
    ];

    private const string EraKey = "era";
    private const string Year   = "year";
    private const string Month  = "month";
    private const string Day    = "day";
    private const string Hour   = "hour";
    private const string Minute = "minute";
    private const string Second = "second";

    /// <inheritdoc/>
    public override string CalendarId   => "japanese";

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarJapaneseName;

    /// <inheritdoc/>
    public override Dictionary<string, string> GetComponents() => new()
    {
        { EraKey, loc.CalendarComponentEra    },
        { Year,   loc.CalendarComponentYear   },
        { Month,  loc.CalendarComponentMonth  },
        { Day,    loc.CalendarComponentDay    },
        { Hour,   loc.CalendarComponentHour   },
        { Minute, loc.CalendarComponentMinute },
        { Second, loc.CalendarComponentSecond }
    };

    private static Era? FindEra(int gregYear, int month, int day)
    {
        foreach (var era in Eras)
        {
            bool afterStart = gregYear > era.StartYear
                || (gregYear == era.StartYear && month > era.StartMonth)
                || (gregYear == era.StartYear && month == era.StartMonth && day >= era.StartDay);
            bool beforeEnd = era.EndYear == null
                || gregYear < era.EndYear
                || (gregYear == era.EndYear && month < era.EndMonth)
                || (gregYear == era.EndYear && month == era.EndMonth && day <= era.EndDay);
            if (afterStart && beforeEnd) return era;
        }
        return null;
    }

    private static int GregorianToJdn(int year, int month, int day)
    {
        int a = (14 - month) / 12;
        int y = year + 4800 - a;
        int m = month + 12 * a - 3;
        return day + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045;
    }

    /// <inheritdoc/>
    public override Dictionary<string, int> FromDateTime(DateTime dt)
    {
        var era = FindEra(dt.Year, dt.Month, dt.Day);
        int eraYear  = era != null ? dt.Year - era.StartYear + 1 : dt.Year;
        int eraIndex = era != null ? Array.IndexOf(Eras, era) : -1;
        return new()
        {
            { EraKey, eraIndex   },
            { Year,   eraYear    },
            { Month,  dt.Month   },
            { Day,    dt.Day     },
            { Hour,   dt.Hour    },
            { Minute, dt.Minute  },
            { Second, dt.Second  }
        };
    }

    /// <inheritdoc/>
    public override bool TryGetComponentValueLocalization(string componentId, int value, out string? result)
    {
        result = componentId switch
        {
            EraKey => loc.GetJapaneseEraName(value),
            Year   => loc.FormatJapaneseYear(value),
            Month  => loc.GetGregorianMonthName(value),
            Day    => loc.FormatJapaneseDay(value),
            Hour   => loc.FormatGregorianHour(value),
            Minute => loc.FormatGregorianMinute(value),
            Second => loc.FormatGregorianSecond(value),
            _      => null
        };
        return result != null;
    }

    /// <summary>Formats components as <c>EraName YY-MM-DD HH:mm:ss</c>.</summary>
    public override string Format(Dictionary<string, int> c)
    {
        int eraIdx = c.GetValueOrDefault(EraKey, -1);
        string eraName = eraIdx >= 0 && eraIdx < Eras.Length ? Eras[eraIdx].Name : "Unknown";
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return $"{eraName} {c[Year]:D2}-{c[Month]:D2}-{c[Day]:D2} {h:D2}:{mi:D2}:{s:D2}";
    }

    /// <summary>Parses a <c>EraName YY-MM-DD</c> or <c>EraName YY-MM-DD HH:mm:ss</c> string.</summary>
    public override Dictionary<string, int> Parse(string value)
    {
        var tokens = value.Split(' ');
        if (tokens.Length < 2) throw new FormatException($"Invalid Japanese date format: {value}");
        string eraName = tokens[0];
        var parts = tokens[1].Split('-');
        var t = tokens.Length > 2 ? tokens[2].Split(':') : null;
        int eraIdx = Array.FindIndex(Eras, e => e.Name.Equals(eraName, StringComparison.OrdinalIgnoreCase));
        return new()
        {
            { EraKey, eraIdx                                           },
            { Year,   int.Parse(parts[0])                             },
            { Month,  int.Parse(parts[1])                             },
            { Day,    int.Parse(parts[2])                             },
            { Hour,   t != null && t.Length > 0 ? int.Parse(t[0]) : 0 },
            { Minute, t != null && t.Length > 1 ? int.Parse(t[1]) : 0 },
            { Second, t != null && t.Length > 2 ? int.Parse(t[2]) : 0 }
        };
    }

    /// <inheritdoc/>
    public override string Localize(Dictionary<string, int> c)
    {
        int eraIdx = c.GetValueOrDefault(EraKey, -1);
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return loc.LocalizeJapaneseDate(eraIdx, c[Year], c[Month], c[Day], h, mi, s);
    }
}
