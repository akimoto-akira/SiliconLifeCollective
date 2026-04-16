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
/// Implements the Yi ethnic solar calendar (彝历).
///
/// Year structure (365 / 366 days):
///   - Month  0 : New Year (大年)  — 3 days, winter solstice
///   - Months 1–5  : first half  — 5 × 36 days (180 days)
///   - Month 11 : Mid-Year (小年) — 2 days (3 in leap year), summer solstice
///   - Months 6–10 : second half — 5 × 36 days (180 days)
///
/// Each regular month (1–10) belongs to one of five seasons (木火土金水).
///   Odd months are male (公月), even months are female (母月).
/// Each 36-day month is divided into three xun (旬) of 12 days each.
///   Days within a xun are named by the 12 animals (虎兔龙蛇马羊猴鸡狗猪鼠牛).
///
/// Leap year rule: same as Gregorian (divisible by 4, except centuries unless by 400).
/// Yi year N starts at the winter solstice of Gregorian year N−1 (approx. Dec 22).
/// </summary>
public class YiCalendar : CalendarBase
{
    // ── Component IDs ──────────────────────────────────────────────────────────
    private const string Year   = "year";
    private const string Month  = "month";
    private const string Day    = "day";
    private const string Season = "season";
    private const string Xun    = "xun";
    private const string Hour   = "hour";
    private const string Minute = "minute";
    private const string Second = "second";

    private readonly DefaultLocalizationBase _loc;

    public YiCalendar(DefaultLocalizationBase loc) => _loc = loc;

    /// <inheritdoc/>
    public override string CalendarId   => "yi";

    /// <inheritdoc/>
    public override string CalendarName => _loc.CalendarYiName;

    /// <inheritdoc/>
    public override Dictionary<string, string> GetComponents() => new()
    {
        { Year,   _loc.CalendarComponentYear      },
        { Month,  _loc.CalendarComponentMonth     },
        { Day,    _loc.CalendarComponentDay       },
        { Season, _loc.CalendarComponentYiSeason  },
        { Xun,    _loc.CalendarComponentYiXun     },
        { Hour,   _loc.CalendarComponentHour      },
        { Minute, _loc.CalendarComponentMinute    },
        { Second, _loc.CalendarComponentSecond    }
    };

    // ── Leap-year & year-length helpers ───────────────────────────────────────

    private static bool IsLeapYear(int year)
        => year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);

    private static int MidYearDays(int year) => IsLeapYear(year) ? 3 : 2;

    // ── JDN helpers ───────────────────────────────────────────────────────────

    private static int GregorianToJdn(int year, int month, int day)
    {
        int a = (14 - month) / 12;
        int y = year + 4800 - a;
        int m = month + 12 * a - 3;
        return day + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045;
    }

    /// <summary>JDN of the winter solstice that opens Yi year <paramref name="yiYear"/> (≈ Dec 22 of Gregorian year N−1).</summary>
    private static int WinterSolsticeJdn(int yiYear)
        => GregorianToJdn(yiYear - 1, 12, 22);

    // ── Core conversion ───────────────────────────────────────────────────────

    private static (int year, int month, int day) FromJdn(int jdn)
    {
        // Estimate Gregorian year from JDN
        int a  = jdn + 32044;
        int b  = (4 * a + 3) / 146097;
        int c  = a - 146097 * b / 4;
        int d  = (4 * c + 3) / 1461;
        int e  = c - 1461 * d / 4;
        int mm = (5 * e + 2) / 153;
        int gregYear = 100 * b + d - 4800 + mm / 10;

        // Yi year N starts at winter solstice of Gregorian year N−1
        int yiYear = jdn >= WinterSolsticeJdn(gregYear + 1) ? gregYear + 1 : gregYear;
        if (yiYear < 1) yiYear = 1;

        int ws         = WinterSolsticeJdn(yiYear);
        int dayOfYear  = jdn - ws; // 0-based from winter solstice

        // Days 0-2 → New Year (month 0)
        if (dayOfYear < 3)
            return (yiYear, 0, dayOfYear + 1);
        dayOfYear -= 3;

        // Days 0-179 → months 1-5
        if (dayOfYear < 5 * 36)
            return (yiYear, dayOfYear / 36 + 1, dayOfYear % 36 + 1);
        dayOfYear -= 5 * 36;

        // Mid-Year (month 11)
        int midDays = MidYearDays(yiYear);
        if (dayOfYear < midDays)
            return (yiYear, 11, dayOfYear + 1);
        dayOfYear -= midDays;

        // Months 6-10
        return (yiYear, dayOfYear / 36 + 6, dayOfYear % 36 + 1);
    }

    // ── Season / Xun helpers ──────────────────────────────────────────────────

    /// <summary>Season index 0–4 for regular months 1–10; -1 for special months.</summary>
    private static int SeasonOf(int month)
        => month >= 1 && month <= 10 ? (month - 1) / 2 : -1;

    /// <summary>Xun index 0–2 for a day 1–36; -1 for special months.</summary>
    private static int XunOf(int day) => (day - 1) / 12;

    /// <summary>Animal index 0–11 within the xun for a day 1–36.</summary>
    private static int AnimalOf(int day) => (day - 1) % 12;

    // ── CalendarBase implementation ───────────────────────────────────────────

    /// <inheritdoc/>
    public override Dictionary<string, int> FromDateTime(DateTime dt)
    {
        int jdn = GregorianToJdn(dt.Year, dt.Month, dt.Day);
        var (year, month, day) = FromJdn(jdn);
        int season = SeasonOf(month);
        int xun    = month is 0 or 11 ? -1 : XunOf(day);
        return new()
        {
            { Year,   year        },
            { Month,  month       },
            { Day,    day         },
            { Season, season      },
            { Xun,    xun         },
            { Hour,   dt.Hour     },
            { Minute, dt.Minute   },
            { Second, dt.Second   }
        };
    }

    /// <inheritdoc/>
    public override bool TryGetComponentValueLocalization(string componentId, int value, out string? result)
    {
        result = componentId switch
        {
            Month  => _loc.GetYiMonthName(value),
            Season => _loc.GetYiSeasonName(value),
            Xun    => _loc.GetYiXunName(value),
            Day    => value >= 1 && value <= 36
                          ? _loc.GetYiDayAnimalName(AnimalOf(value))
                          : null,
            Hour   => _loc.FormatGregorianHour(value),
            Minute => _loc.FormatGregorianMinute(value),
            Second => _loc.FormatGregorianSecond(value),
            _      => null
        };
        return result != null;
    }

    /// <summary>
    /// Formats components as <c>yyyy-MM-dd HH:mm:ss</c>.
    /// Month 0 = New Year, month 11 = Mid-Year.
    /// </summary>
    public override string Format(Dictionary<string, int> c)
    {
        int h  = c.GetValueOrDefault(Hour,   0);
        int mi = c.GetValueOrDefault(Minute, 0);
        int s  = c.GetValueOrDefault(Second, 0);
        return $"{c[Year]:D4}-{c[Month]:D2}-{c[Day]:D2} {h:D2}:{mi:D2}:{s:D2}";
    }

    /// <summary>
    /// Parses a <c>yyyy-MM-dd HH:mm:ss</c> or <c>yyyy-MM-dd</c> string.
    /// Season and Xun are derived from month/day.
    /// </summary>
    public override Dictionary<string, int> Parse(string value)
    {
        var spaceIdx = value.IndexOf(' ');
        var datePart = spaceIdx >= 0 ? value[..spaceIdx] : value;
        var timePart = spaceIdx >= 0 ? value[(spaceIdx + 1)..] : null;
        var parts    = datePart.Split('-');
        int year     = int.Parse(parts[0]);
        int month    = int.Parse(parts[1]);
        int day      = int.Parse(parts[2]);
        var t        = timePart?.Split(':');
        return new()
        {
            { Year,   year                                                    },
            { Month,  month                                                   },
            { Day,    day                                                     },
            { Season, SeasonOf(month)                                         },
            { Xun,    month is 0 or 11 ? -1 : XunOf(day)                     },
            { Hour,   t != null && t.Length > 0 ? int.Parse(t[0]) : 0        },
            { Minute, t != null && t.Length > 1 ? int.Parse(t[1]) : 0        },
            { Second, t != null && t.Length > 2 ? int.Parse(t[2]) : 0        }
        };
    }

    /// <inheritdoc/>
    public override string Localize(Dictionary<string, int> c)
    {
        int h  = c.GetValueOrDefault(Hour,   0);
        int mi = c.GetValueOrDefault(Minute, 0);
        int s  = c.GetValueOrDefault(Second, 0);
        return _loc.LocalizeYiDate(c[Year], c[Month], c[Day], h, mi, s);
    }
}
