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
/// Implements the Vietnamese Lunar calendar system.
/// Identical to Chinese Lunar calendar in date calculation, but uses Vietnamese zodiac (Cat instead of Rabbit).
/// </summary>
public class VietnameseCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    // Same lunar data as Chinese calendar
    private static readonly int[] LunarData =
    [
        0x04bd8,0x04ae0,0x0a570,0x054d5,0x0d260,0x0d950,0x16554,0x056a0,0x09ad0,0x055d2,
        0x04ae0,0x0a5b6,0x0a4d0,0x0d250,0x1d255,0x0b540,0x0d6a0,0x0ada2,0x095b0,0x14977,
        0x04970,0x0a4b0,0x0b4b5,0x06a50,0x06d40,0x1ab54,0x02b60,0x09570,0x052f2,0x04970,
        0x06566,0x0d4a0,0x0ea50,0x06e95,0x05ad0,0x02b60,0x186e3,0x092e0,0x1c8d7,0x0c950,
        0x0d4a0,0x1d8a6,0x0b550,0x056a0,0x1a5b4,0x025d0,0x092d0,0x0d2b2,0x0a950,0x0b557,
        0x06ca0,0x0b550,0x15355,0x04da0,0x0a5b0,0x14573,0x052b0,0x0a9a8,0x0e950,0x06aa0,
        0x0aea6,0x0ab50,0x04b60,0x0aae4,0x0a570,0x05260,0x0f263,0x0d950,0x05b57,0x056a0,
        0x096d0,0x04dd5,0x04ad0,0x0a4d0,0x0d4d4,0x0d250,0x0d558,0x0b540,0x0b6a0,0x195a6,
        0x095b0,0x049b0,0x0a974,0x0a4b0,0x0b27a,0x06a50,0x06d40,0x0af46,0x0ab60,0x09570,
        0x04af5,0x04970,0x064b0,0x074a3,0x0ea50,0x06b58,0x055c0,0x0ab60,0x096d5,0x092e0,
        0x0c960,0x0d954,0x0d4a0,0x0da50,0x07552,0x056a0,0x0abb7,0x025d0,0x092d0,0x0cab5,
        0x0a950,0x0b4a0,0x0baa4,0x0ad50,0x055d9,0x04ba0,0x0a5b0,0x15176,0x052b0,0x0a930,
        0x07954,0x06aa0,0x0ad50,0x05b52,0x04b60,0x0a6e6,0x0a4e0,0x0d260,0x0d950,0x16554,
        0x05aa0,0x076a3,0x096d0,0x04afb,0x04ad0,0x0a4d0,0x1d0b6,0x0d250,0x0d520,0x0dd45,
        0x0b5a0,0x056d0,0x055b2,0x049b0,0x0a577,0x0a4b0,0x0aa50,0x1b255,0x06d20,0x0ada0,
        0x14b63,0x09370,0x049f8,0x04970,0x064b0,0x168a6,0x0ea50,0x06b20,0x1a6c4,0x0aae0,
        0x0a2e0,0x0d2e3,0x0c960,0x0d557,0x0d4a0,0x0da50,0x05d55,0x056a0,0x0a6d0,0x055d4,
        0x052d0,0x0a9b8,0x0a950,0x0b4a0,0x0b6a6,0x0ad50,0x055a0,0x0aba4,0x0a5b0,0x052b0,
        0x0b273,0x06930,0x07337,0x06aa0,0x0ad50,0x14b55,0x04b60,0x0a570,0x054e4,0x0d160,
        0x0e968,0x0d520,0x0daa0,0x16aa6,0x056d0,0x04ae0,0x0a9d4,0x0a2d0,0x0d150,0x0f252,
        0x0d520,
    ];

    private const int Lunar1900Jdn = 2415051;

    private const string Year   = "year";
    private const string Month  = "month";
    private const string Day    = "day";
    private const string IsLeap = "isLeap";
    private const string Zodiac = "zodiac";
    private const string Hour   = "hour";
    private const string Minute = "minute";
    private const string Second = "second";

    /// <inheritdoc/>
    public override string CalendarId   => "vietnamese";

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarVietnameseName;

    /// <inheritdoc/>
    public override Dictionary<string, string> GetComponents() => new()
    {
        { Year,   loc.CalendarComponentYear   },
        { Month,  loc.CalendarComponentMonth  },
        { Day,    loc.CalendarComponentDay    },
        { IsLeap, loc.CalendarComponentIsLeap },
        { Zodiac, loc.CalendarComponentZodiac },
        { Hour,   loc.CalendarComponentHour   },
        { Minute, loc.CalendarComponentMinute },
        { Second, loc.CalendarComponentSecond }
    };

    private static int GetLeapMonth(int year)
    {
        if (year < 1900 || year > 2100) return 0;
        return LunarData[year - 1900] & 0x0F;
    }

    private static int GetDaysInMonth(int year, int month, bool isLeap)
    {
        if (year < 1900 || year > 2100 || month < 1 || month > 12) return 0;
        int data = LunarData[year - 1900];
        if (isLeap) return (data & 0x10000) != 0 ? 30 : 29;
        return (data & (0x10000 >> month)) != 0 ? 30 : 29;
    }

    private static int GetDaysInYear(int year)
    {
        int total = 0;
        for (int m = 1; m <= 12; m++) total += GetDaysInMonth(year, m, false);
        int leap = GetLeapMonth(year);
        if (leap > 0) total += GetDaysInMonth(year, leap, true);
        return total;
    }

    private static (int year, int month, int day, bool isLeap) FromJdn(int jdn)
    {
        int offset = jdn - Lunar1900Jdn;
        int year = 1900;
        while (true)
        {
            int diy = GetDaysInYear(year);
            if (offset < diy) break;
            offset -= diy;
            year++;
        }
        int leapMonth = GetLeapMonth(year);
        int month = 1;
        bool leap = false;
        while (true)
        {
            int dim = GetDaysInMonth(year, month, false);
            if (offset < dim) break;
            offset -= dim;
            if (month == leapMonth && leapMonth > 0)
            {
                int leapDim = GetDaysInMonth(year, month, true);
                if (offset < leapDim) { leap = true; break; }
                offset -= leapDim;
            }
            month++;
            if (month > 12) break;
        }
        return (year, month, offset + 1, leap);
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
        int jdn = GregorianToJdn(dt.Year, dt.Month, dt.Day);
        var (year, month, day, isLeap) = FromJdn(jdn);
        int zodiacIdx = (year - 4) % 12;
        if (zodiacIdx < 0) zodiacIdx += 12;
        return new()
        {
            { Year,   year              },
            { Month,  month             },
            { Day,    day               },
            { IsLeap, isLeap ? 1 : 0   },
            { Zodiac, zodiacIdx         },
            { Hour,   dt.Hour           },
            { Minute, dt.Minute         },
            { Second, dt.Second         }
        };
    }

    /// <inheritdoc/>
    public override bool TryGetComponentValueLocalization(string componentId, int value, out string? result)
    {
        result = componentId switch
        {
            Month  => loc.GetVietnameseMonthName(value),
            Zodiac => loc.GetVietnameseZodiacName(value),
            Hour   => loc.FormatGregorianHour(value),
            Minute => loc.FormatGregorianMinute(value),
            Second => loc.FormatGregorianSecond(value),
            _      => null
        };
        return result != null;
    }

    /// <summary>Formats components as <c>yyyy-MM-dd HH:mm:ss</c> or <c>yyyy-LMM-dd HH:mm:ss</c> for leap months.</summary>
    public override string Format(Dictionary<string, int> c)
    {
        string leapMark = c.GetValueOrDefault(IsLeap) == 1 ? "L" : "";
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return $"{c[Year]:D4}-{leapMark}{c[Month]:D2}-{c[Day]:D2} {h:D2}:{mi:D2}:{s:D2}";
    }

    /// <summary>Parses a <c>yyyy-MM-dd</c>, <c>yyyy-LMM-dd</c>, or their datetime variants.</summary>
    public override Dictionary<string, int> Parse(string value)
    {
        var spaceIdx = value.IndexOf(' ');
        var datePart = spaceIdx >= 0 ? value[..spaceIdx] : value;
        var timePart = spaceIdx >= 0 ? value[(spaceIdx + 1)..] : null;
        var parts = datePart.Split('-');
        bool isLeap = parts[1].StartsWith('L');
        var t = timePart?.Split(':');
        return new()
        {
            { Year,   int.Parse(parts[0])                             },
            { Month,  int.Parse(isLeap ? parts[1][1..] : parts[1])   },
            { Day,    int.Parse(parts[2])                             },
            { IsLeap, isLeap ? 1 : 0                                  },
            { Hour,   t != null && t.Length > 0 ? int.Parse(t[0]) : 0 },
            { Minute, t != null && t.Length > 1 ? int.Parse(t[1]) : 0 },
            { Second, t != null && t.Length > 2 ? int.Parse(t[2]) : 0 }
        };
    }

    /// <inheritdoc/>
    public override string Localize(Dictionary<string, int> c)
    {
        bool isLeap = c.GetValueOrDefault(IsLeap) == 1;
        int zodiac  = c.GetValueOrDefault(Zodiac, 0);
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return loc.LocalizeVietnameseDate(c[Year], c[Month], c[Day], isLeap, zodiac, h, mi, s);
    }
}
