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
/// Implements the Xishuangbanna Dai (Xiao Dai / Minor Dai) lunisolar calendar.
/// <para>
/// New Year falls on the first new moon on or after the vernal equinox (around April 13–15 Gregorian).
/// Epoch: Julian 638-03-21 (JDN 1954168). Leap years contain 13 new moons; the leap month is
/// inserted after regular month 9 (represented as month 10 in the component dictionary, with
/// <c>isLeap=1</c>). Supported Gregorian range: 2000 BCE – 4000 CE.
/// </para>
/// </summary>
public class DaiCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    // ── constants ─────────────────────────────────────────────────────────────

    /// <summary>Epoch JDN: Julian calendar 638-03-21.</summary>
    private const int EpochJdn = 1954168;

    /// <summary>Gregorian year = Dai year + 638.</summary>
    private const int EpochYearOffset = 638;

    /// <summary>UTC+8 for Yunnan.</summary>
    private const double TimezoneOffset = 8.0;

    // ── component keys ────────────────────────────────────────────────────────

    private const string Year    = "year";
    private const string Month   = "month";
    private const string Day     = "day";
    private const string IsLeap  = "isLeap";
    private const string Hour    = "hour";
    private const string Minute  = "minute";
    private const string Second  = "second";

    // ── year-structure cache ──────────────────────────────────────────────────

    private readonly Dictionary<int, DaiYearStructure> _cache = [];

    // ── CalendarBase ──────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public override string CalendarId   => "dai";

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarDaiName;

    /// <inheritdoc/>
    public override Dictionary<string, string> GetComponents() => new()
    {
        { Year,   loc.CalendarComponentYear   },
        { Month,  loc.CalendarComponentMonth  },
        { Day,    loc.CalendarComponentDay    },
        { IsLeap, loc.CalendarComponentIsLeap },
        { Hour,   loc.CalendarComponentHour   },
        { Minute, loc.CalendarComponentMinute },
        { Second, loc.CalendarComponentSecond }
    };

    /// <inheritdoc/>
    public override Dictionary<string, int> FromDateTime(DateTime dt)
    {
        int jdn = GregorianToJdn(dt.Year, dt.Month, dt.Day);

        // Estimate Dai year and search ±1 to find the correct year bracket.
        int est = (int)Math.Floor((jdn - EpochJdn) / 365.25);
        int daiYear = est;
        for (int y = est - 1; y <= est + 1; y++)
        {
            int ny0 = GetNewYearJdn(y);
            int ny1 = GetNewYearJdn(y + 1);
            if (jdn >= ny0 && jdn < ny1) { daiYear = y; break; }
        }

        var ys = GetYearStructure(daiYear);

        int daiMonth = 1, daiDay = 1;
        bool leap = false;
        for (int m = 0; m < ys.MonthStartJdns.Count; m++)
        {
            int end = m < ys.MonthStartJdns.Count - 1
                ? ys.MonthStartJdns[m + 1]
                : GetNewYearJdn(daiYear + 1);
            if (jdn >= ys.MonthStartJdns[m] && jdn < end)
            {
                // In a leap year the sequence is 1..9, leapMonth9, 10..12.
                // We store leap-month-9 at index 9 (0-based), regular month 10 at index 10, etc.
                if (ys.IsLeap && m == 9)
                {
                    daiMonth = 9;
                    leap = true;
                }
                else if (ys.IsLeap && m > 9)
                {
                    daiMonth = m; // index 10→month 10, 11→11, 12→12
                }
                else
                {
                    daiMonth = m + 1;
                }
                daiDay = jdn - ys.MonthStartJdns[m] + 1;
                break;
            }
        }

        return new()
        {
            { Year,   daiYear          },
            { Month,  daiMonth         },
            { Day,    daiDay           },
            { IsLeap, leap ? 1 : 0     },
            { Hour,   dt.Hour          },
            { Minute, dt.Minute        },
            { Second, dt.Second        }
        };
    }

    /// <inheritdoc/>
    public override bool TryGetComponentValueLocalization(string componentId, int value, out string? result)
    {
        result = componentId switch
        {
            Month  => loc.GetDaiMonthName(value),
            Day    => loc.FormatDaiDay(value),
            Hour   => loc.FormatGregorianHour(value),
            Minute => loc.FormatGregorianMinute(value),
            Second => loc.FormatGregorianSecond(value),
            _      => null
        };
        return result != null;
    }

    /// <summary>
    /// Formats components as <c>yyyy-MM-dd HH:mm:ss</c>;
    /// leap month 9 is written as <c>yyyy-L09-dd HH:mm:ss</c>.
    /// </summary>
    public override string Format(Dictionary<string, int> c)
    {
        bool leap = c.GetValueOrDefault(IsLeap) == 1;
        string leapMark = leap ? "L" : "";
        int h  = c.GetValueOrDefault(Hour,   0);
        int mi = c.GetValueOrDefault(Minute, 0);
        int s  = c.GetValueOrDefault(Second, 0);
        return $"{c[Year]:D4}-{leapMark}{c[Month]:D2}-{c[Day]:D2} {h:D2}:{mi:D2}:{s:D2}";
    }

    /// <summary>
    /// Parses a <c>yyyy-MM-dd</c>, <c>yyyy-LMM-dd</c>, or their datetime variants.
    /// </summary>
    public override Dictionary<string, int> Parse(string value)
    {
        int spaceIdx = value.IndexOf(' ');
        string datePart = spaceIdx >= 0 ? value[..spaceIdx] : value;
        string? timePart = spaceIdx >= 0 ? value[(spaceIdx + 1)..] : null;
        string[] parts = datePart.Split('-');
        bool isLeap = parts[1].StartsWith('L');
        string[]? t = timePart?.Split(':');
        return new()
        {
            { Year,   int.Parse(parts[0])                              },
            { Month,  int.Parse(isLeap ? parts[1][1..] : parts[1])    },
            { Day,    int.Parse(parts[2])                              },
            { IsLeap, isLeap ? 1 : 0                                   },
            { Hour,   t != null && t.Length > 0 ? int.Parse(t[0]) : 0 },
            { Minute, t != null && t.Length > 1 ? int.Parse(t[1]) : 0 },
            { Second, t != null && t.Length > 2 ? int.Parse(t[2]) : 0 }
        };
    }

    /// <inheritdoc/>
    public override string Localize(Dictionary<string, int> c)
    {
        bool leap = c.GetValueOrDefault(IsLeap) == 1;
        int h  = c.GetValueOrDefault(Hour,   0);
        int mi = c.GetValueOrDefault(Minute, 0);
        int s  = c.GetValueOrDefault(Second, 0);
        return loc.LocalizeDaiDate(c[Year], c[Month], c[Day], leap, h, mi, s);
    }

    // ── astronomical helpers ──────────────────────────────────────────────────

    /// <summary>
    /// Returns the JDN of Dai New Year for the given Dai year.
    /// New Year = first new moon on or after the vernal equinox (UTC+8),
    /// constrained to be within ±3 days of April 14; otherwise the new moon
    /// closest to April 14 is used.
    /// </summary>
    private int GetNewYearJdn(int daiYear)
    {
        int gregYear = daiYear + EpochYearOffset;
        double equinoxJde = GetVernalEquinox(gregYear);
        int equinoxJdn = (int)Math.Floor(equinoxJde + 0.5 + TimezoneOffset / 24.0);

        var moons = GetNewMoonsInRange(equinoxJdn - 30, equinoxJdn + 60);
        int targetJdn = GregorianToJdn(gregYear, 4, 14);

        // First new moon on or after equinox
        var candidate = moons.FirstOrDefault(m => m >= equinoxJdn);
        if (candidate == 0) candidate = moons.LastOrDefault();

        if (Math.Abs(candidate - targetJdn) <= 3)
            return candidate;

        // Fall back to the new moon closest to April 14
        return moons.MinBy(m => Math.Abs(m - targetJdn));
    }

    /// <summary>
    /// Builds and caches the year structure (month boundaries and leap status) for a Dai year.
    /// </summary>
    private DaiYearStructure GetYearStructure(int daiYear)
    {
        if (_cache.TryGetValue(daiYear, out var cached)) return cached;

        int ny0 = GetNewYearJdn(daiYear);
        int ny1 = GetNewYearJdn(daiYear + 1);

        var moons = GetNewMoonsInRange(ny0, ny1 - 1);
        bool isLeap = moons.Count >= 13;

        // Build month-start JDN list.
        // Leap year: months 1–9, leapMonth9, 10–12  (13 entries total)
        // Normal year: months 1–12                   (12 entries total)
        var starts = new List<int>();
        int count = isLeap ? 13 : 12;
        for (int i = 0; i < count && i < moons.Count; i++)
        {
            int idx = (isLeap && i >= 10) ? i + 1 : i; // skip index 9 for months 10–12
            if (idx < moons.Count) starts.Add(moons[idx]);
            else if (i < moons.Count) starts.Add(moons[i]);
        }
        // Insert leap-month-9 at position 9 (0-based) when leap
        if (isLeap && moons.Count >= 10)
        {
            starts.Clear();
            for (int i = 0; i < 9 && i < moons.Count; i++) starts.Add(moons[i]);   // months 1–9
            starts.Add(moons[9]);                                                     // leap month 9
            for (int i = 10; i < 13 && i < moons.Count; i++) starts.Add(moons[i]);  // months 10–12
        }

        // Calculate month lengths (29 or 30 days, clamped for safety)
        var lengths = new List<int>();
        for (int i = 0; i < starts.Count; i++)
        {
            int len = (i < starts.Count - 1 ? starts[i + 1] : ny1) - starts[i];
            lengths.Add(Math.Clamp(len, 29, 30));
        }

        var ys = new DaiYearStructure(daiYear, ny0, isLeap, starts, lengths);
        _cache[daiYear] = ys;
        return ys;
    }

    // ── astronomical approximations (Jean Meeus, Astronomical Algorithms) ────

    /// <summary>Approximate JDE of the vernal equinox for the given Gregorian year.</summary>
    private static double GetVernalEquinox(int year)
    {
        double y = (year - 2000.0) / 1000.0;
        // Mean equinox JDE (Meeus Table 27.a)
        double jde = 2451623.80984
                   + 365242.37404 * y
                   + 0.05169 * y * y
                   - 0.00411 * y * y * y
                   - 0.00057 * y * y * y * y;
        return jde;
    }

    /// <summary>
    /// Returns a list of new-moon JDNs (UTC+8) within [startJdn, endJdn].
    /// Uses the mean synodic month (29.53059 days) seeded from a known new moon.
    /// </summary>
    private static List<int> GetNewMoonsInRange(int startJdn, int endJdn)
    {
        // Known new moon: 2000-01-06 JDN 2451550 (JDE ≈ 2451550.1)
        const double KnownNewMoonJde = 2451550.1;
        const double SynodicMonth    = 29.530588853;

        double startJde = startJdn - TimezoneOffset / 24.0;
        double endJde   = endJdn   - TimezoneOffset / 24.0 + 1.0;

        double k0 = Math.Floor((startJde - KnownNewMoonJde) / SynodicMonth);
        var result = new List<int>();

        for (double k = k0 - 1; ; k++)
        {
            double jde = KnownNewMoonJde + k * SynodicMonth;
            int jdn = (int)Math.Floor(jde + 0.5 + TimezoneOffset / 24.0);
            if (jdn > endJdn) break;
            if (jdn >= startJdn) result.Add(jdn);
        }
        return result;
    }

    /// <summary>Converts a proleptic Gregorian date to JDN.</summary>
    private static int GregorianToJdn(int year, int month, int day)
    {
        int a = (14 - month) / 12;
        int y = year + 4800 - a;
        int m = month + 12 * a - 3;
        return day + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045;
    }
}

/// <summary>Cached year structure for a single Dai calendar year.</summary>
internal sealed class DaiYearStructure(
    int year,
    int newYearJdn,
    bool isLeap,
    List<int> monthStartJdns,
    List<int> monthLengths)
{
    public int       Year           { get; } = year;
    public int       NewYearJdn     { get; } = newYearJdn;
    public bool      IsLeap         { get; } = isLeap;
    public List<int> MonthStartJdns { get; } = monthStartJdns;
    public List<int> MonthLengths   { get; } = monthLengths;
}
