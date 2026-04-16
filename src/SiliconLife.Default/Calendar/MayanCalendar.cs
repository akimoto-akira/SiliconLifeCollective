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
/// Implements the Mayan Long Count calendar system.
/// Epoch: JDN 584252. Components: baktun, katun, tun, uinal, kin.
/// </summary>
public class MayanCalendar(DefaultLocalizationBase loc) : CalendarBase
{
    private const int Epoch = 584252;

    private const string Baktun = "baktun";
    private const string Katun  = "katun";
    private const string Tun    = "tun";
    private const string Uinal  = "uinal";
    private const string Kin    = "kin";
    private const string Hour   = "hour";
    private const string Minute = "minute";
    private const string Second = "second";

    /// <inheritdoc/>
    public override string CalendarId   => "mayan";

    /// <inheritdoc/>
    public override string CalendarName => loc.CalendarMayanName;

    /// <inheritdoc/>
    public override Dictionary<string, string> GetComponents() => new()
    {
        { Baktun, loc.CalendarMayanBaktun    },
        { Katun,  loc.CalendarMayanKatun     },
        { Tun,    loc.CalendarMayanTun       },
        { Uinal,  loc.CalendarMayanUinal     },
        { Kin,    loc.CalendarMayanKin       },
        { Hour,   loc.CalendarComponentHour  },
        { Minute, loc.CalendarComponentMinute},
        { Second, loc.CalendarComponentSecond}
    };

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
        int jdn  = GregorianToJdn(dateTime.Year, dateTime.Month, dateTime.Day);
        int days = jdn - Epoch;
        int baktun = days / 144000; days %= 144000;
        int katun  = days / 7200;   days %= 7200;
        int tun    = days / 360;    days %= 360;
        int uinal  = days / 20;
        int kin    = days % 20;
        return new()
        {
            { Baktun, baktun          },
            { Katun,  katun           },
            { Tun,    tun             },
            { Uinal,  uinal           },
            { Kin,    kin             },
            { Hour,   dateTime.Hour   },
            { Minute, dateTime.Minute },
            { Second, dateTime.Second }
        };
    }

    /// <inheritdoc/>
    public override bool TryGetComponentValueLocalization(string componentId, int value, out string? result)
    {
        result = componentId switch
        {
            Hour   => loc.FormatGregorianHour(value),
            Minute => loc.FormatGregorianMinute(value),
            Second => loc.FormatGregorianSecond(value),
            _      => null
        };
        return result != null;
    }

    /// <summary>
    /// Formats components as <c>baktun.katun.tun.uinal.kin HH:mm:ss</c>.
    /// </summary>
    public override string Format(Dictionary<string, int> c)
    {
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return $"{c[Baktun]}.{c[Katun]}.{c[Tun]}.{c[Uinal]}.{c[Kin]} {h:D2}:{mi:D2}:{s:D2}";
    }

    /// <summary>
    /// Parses a <c>baktun.katun.tun.uinal.kin</c> or <c>baktun.katun.tun.uinal.kin HH:mm:ss</c> string.
    /// </summary>
    public override Dictionary<string, int> Parse(string value)
    {
        var spaceIdx = value.IndexOf(' ');
        var datePart = spaceIdx >= 0 ? value[..spaceIdx] : value;
        var timePart = spaceIdx >= 0 ? value[(spaceIdx + 1)..] : null;
        var p = datePart.Split('.');
        var t = timePart?.Split(':');
        return new()
        {
            { Baktun, int.Parse(p[0]) },
            { Katun,  int.Parse(p[1]) },
            { Tun,    int.Parse(p[2]) },
            { Uinal,  int.Parse(p[3]) },
            { Kin,    int.Parse(p[4]) },
            { Hour,   t != null && t.Length > 0 ? int.Parse(t[0]) : 0 },
            { Minute, t != null && t.Length > 1 ? int.Parse(t[1]) : 0 },
            { Second, t != null && t.Length > 2 ? int.Parse(t[2]) : 0 }
        };
    }

    /// <summary>
    /// Returns a localized human-readable string via <see cref="DefaultLocalizationBase.LocalizeMayanDate"/>.
    /// </summary>
    public override string Localize(Dictionary<string, int> c)
    {
        var h  = c.GetValueOrDefault(Hour,   0);
        var mi = c.GetValueOrDefault(Minute, 0);
        var s  = c.GetValueOrDefault(Second, 0);
        return loc.LocalizeMayanDate(c[Baktun], c[Katun], c[Tun], c[Uinal], c[Kin], h, mi, s);
    }
}
