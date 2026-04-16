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
/// Implements the Chinese Sexagenary Cycle (干支 Ganzhi) calendar.
/// Components: yearStem, yearBranch, monthStem, monthBranch, dayStem, dayBranch, hour, minute, second.
/// Based on the Chinese Lunar calendar year for year/month Ganzhi, and JDN for day Ganzhi.
/// </summary>
public class SexagenaryCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    // Reference: 1984 = 甲子年 (stem=0, branch=0)
    private const int RefYear = 1984;
    // Reference day: JDN 2451545 (2000-01-01) = 戊午 (stem=4, branch=6)
    private const int RefDayJdn    = 2451545;
    private const int RefDayStem   = 4;
    private const int RefDayBranch = 6;

    private const string YearStem    = "yearStem";
    private const string YearBranch  = "yearBranch";
    private const string MonthStem   = "monthStem";
    private const string MonthBranch = "monthBranch";
    private const string DayStem     = "dayStem";
    private const string DayBranch   = "dayBranch";
    private const string Hour        = "hour";
    private const string Minute      = "minute";
    private const string Second      = "second";

    /// <inheritdoc/>
    public override string CalendarId   => "sexagenary";

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarSexagenaryName;

    /// <inheritdoc/>
    public override Dictionary<string, string> GetComponents() => new()
    {
        { YearStem,    loc.CalendarComponentYearStem    },
        { YearBranch,  loc.CalendarComponentYearBranch  },
        { MonthStem,   loc.CalendarComponentMonthStem   },
        { MonthBranch, loc.CalendarComponentMonthBranch },
        { DayStem,     loc.CalendarComponentDayStem     },
        { DayBranch,   loc.CalendarComponentDayBranch   },
        { Hour,        loc.CalendarComponentHour        },
        { Minute,      loc.CalendarComponentMinute      },
        { Second,      loc.CalendarComponentSecond      }
    };

    private static (int stem, int branch) YearGanzhi(int lunarYear)
    {
        int offset = lunarYear - RefYear;
        int stem   = ((offset % 10) + 10) % 10;
        int branch = ((offset % 12) + 12) % 12;
        return (stem, branch);
    }

    private static (int stem, int branch) MonthGanzhi(int lunarYear, int lunarMonth)
    {
        var (yearStem, _) = YearGanzhi(lunarYear);
        int firstMonthStem = (yearStem * 2 + 2) % 10;
        int monthStem   = (firstMonthStem + lunarMonth - 1) % 10;
        int monthBranch = (lunarMonth + 1) % 12;
        return (monthStem, monthBranch);
    }

    private static (int stem, int branch) DayGanzhi(int jdn)
    {
        int offset = jdn - RefDayJdn;
        int stem   = ((RefDayStem   + offset) % 10 + 10) % 10;
        int branch = ((RefDayBranch + offset) % 12 + 12) % 12;
        return (stem, branch);
    }

    private static int GregorianToJdn(int year, int month, int day)
    {
        int a = (14 - month) / 12;
        int y = year + 4800 - a;
        int m = month + 12 * a - 3;
        return day + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045;
    }

    private static int ApproxLunarYear(int gregYear, int month)
        => month <= 1 ? gregYear - 1 : gregYear;

    /// <inheritdoc/>
    public override Dictionary<string, int> FromDateTime(DateTime dt)
    {
        int jdn        = GregorianToJdn(dt.Year, dt.Month, dt.Day);
        int lunarYear  = ApproxLunarYear(dt.Year, dt.Month);
        int lunarMonth = dt.Month;

        var (ys, yb) = YearGanzhi(lunarYear);
        var (ms, mb) = MonthGanzhi(lunarYear, lunarMonth);
        var (ds, db) = DayGanzhi(jdn);

        return new()
        {
            { YearStem,    ys        },
            { YearBranch,  yb        },
            { MonthStem,   ms        },
            { MonthBranch, mb        },
            { DayStem,     ds        },
            { DayBranch,   db        },
            { Hour,        dt.Hour   },
            { Minute,      dt.Minute },
            { Second,      dt.Second }
        };
    }

    /// <inheritdoc/>
    public override bool TryGetComponentValueLocalization(string componentId, int value, out string? result)
    {
        result = componentId switch
        {
            YearStem or MonthStem or DayStem       => loc.GetSexagenaryStemName(value),
            YearBranch or MonthBranch or DayBranch => loc.GetSexagenaryBranchName(value),
            Hour   => loc.FormatGregorianHour(value),
            Minute => loc.FormatGregorianMinute(value),
            Second => loc.FormatGregorianSecond(value),
            _      => null
        };
        return result != null;
    }

    /// <summary>
    /// Formats components as <c>YS-YB-MS-MB-DS-DB HH:mm:ss</c> (indices).
    /// </summary>
    public override string Format(Dictionary<string, int> c)
    {
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return $"{c[YearStem]}-{c[YearBranch]}-{c[MonthStem]}-{c[MonthBranch]}-{c[DayStem]}-{c[DayBranch]} {h:D2}:{mi:D2}:{s:D2}";
    }

    /// <summary>
    /// Parses a <c>YS-YB-MS-MB-DS-DB</c> or <c>YS-YB-MS-MB-DS-DB HH:mm:ss</c> string.
    /// </summary>
    public override Dictionary<string, int> Parse(string value)
    {
        var spaceIdx = value.IndexOf(' ');
        var datePart = spaceIdx >= 0 ? value[..spaceIdx] : value;
        var timePart = spaceIdx >= 0 ? value[(spaceIdx + 1)..] : null;
        var p = datePart.Split('-');
        var t = timePart?.Split(':');
        return new()
        {
            { YearStem,    int.Parse(p[0]) },
            { YearBranch,  int.Parse(p[1]) },
            { MonthStem,   int.Parse(p[2]) },
            { MonthBranch, int.Parse(p[3]) },
            { DayStem,     int.Parse(p[4]) },
            { DayBranch,   int.Parse(p[5]) },
            { Hour,   t != null && t.Length > 0 ? int.Parse(t[0]) : 0 },
            { Minute, t != null && t.Length > 1 ? int.Parse(t[1]) : 0 },
            { Second, t != null && t.Length > 2 ? int.Parse(t[2]) : 0 }
        };
    }

    /// <inheritdoc/>
    public override string Localize(Dictionary<string, int> c)
    {
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return loc.LocalizeSexagenaryDate(
            c[YearStem], c[YearBranch], c[MonthStem], c[MonthBranch], c[DayStem], c[DayBranch],
            h, mi, s);
    }
}
