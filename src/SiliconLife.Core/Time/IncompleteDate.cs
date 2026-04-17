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

namespace SiliconLife.Collective;

/// <summary>
/// Incomplete date/time — year is required, month/day/hour/minute/second are nullable
/// to represent a time range where lower components are unspecified.
/// </summary>
/// <para>
/// Use <see cref="Matches(DateTime)"/> to test whether a concrete <see cref="DateTime"/>
/// falls within the range described by the non-null components.
/// </para>
/// <example>
/// <c>new IncompleteDate(2026, month: 4)</c> represents any moment in April 2026.
/// </example>
public readonly struct IncompleteDate : IEquatable<IncompleteDate>, IComparable<IncompleteDate>
{
    /// <summary>Year (required).</summary>
    public int Year { get; }

    /// <summary>Month (1–12, null = unspecified).</summary>
    public int? Month { get; }

    /// <summary>Day (1–31, null = unspecified).</summary>
    public int? Day { get; }

    /// <summary>Hour (0–23, null = unspecified).</summary>
    public int? Hour { get; }

    /// <summary>Minute (0–59, null = unspecified).</summary>
    public int? Minute { get; }

    /// <summary>Second (0–59, null = unspecified).</summary>
    public int? Second { get; }

    /// <summary>
    /// Constructs an incomplete date. Year is required; pass null for any other
    /// component to leave it unspecified.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">A component is out of its valid range.</exception>
    public IncompleteDate(int year, int? month = null, int? day = null,
                          int? hour = null, int? minute = null, int? second = null)
    {
        if (year is < 1 or > 9999)
            throw new ArgumentOutOfRangeException(nameof(year));
        if (month is < 1 or > 12)
            throw new ArgumentOutOfRangeException(nameof(month));
        if (day is < 1 or > 31)
            throw new ArgumentOutOfRangeException(nameof(day));
        if (hour is < 0 or > 23)
            throw new ArgumentOutOfRangeException(nameof(hour));
        if (minute is < 0 or > 59)
            throw new ArgumentOutOfRangeException(nameof(minute));
        if (second is < 0 or > 59)
            throw new ArgumentOutOfRangeException(nameof(second));

        Year = year;
        Month = month;
        Day = day;
        Hour = hour;
        Minute = minute;
        Second = second;
    }

    /// <summary>
    /// Returns true if <paramref name="dateTime"/> matches all non-null components
    /// of this incomplete date.
    /// </summary>
    public bool Matches(DateTime dateTime)
    {
        if (dateTime.Year != Year) return false;
        if (Month.HasValue && dateTime.Month != Month.Value) return false;
        if (Day.HasValue && dateTime.Day != Day.Value) return false;
        if (Hour.HasValue && dateTime.Hour != Hour.Value) return false;
        if (Minute.HasValue && dateTime.Minute != Minute.Value) return false;
        if (Second.HasValue && dateTime.Second != Second.Value) return false;
        return true;
    }

    /// <summary>
    /// Computes the earliest and latest <see cref="DateTime"/> (inclusive on both ends)
    /// represented by this incomplete date.
    /// <para>
    /// Unspecified components are expanded to their min/max values; e.g. when only month
    /// is given, the range spans from the 1st to the last day of that month.
    /// </para>
    /// </summary>
    /// <param name="refMonth">Fallback month when month is unspecified (default 1).</param>
    public (DateTime Start, DateTime End) GetRange(int refMonth = 1)
    {
        int m = Month ?? refMonth;

        int startDay = 1;
        int endDay = DateTime.DaysInMonth(Year, m);
        if (Day.HasValue)
        {
            startDay = Day.Value;
            endDay = Day.Value;
        }

        var start = new DateTime(Year, m, startDay,
            Hour ?? 0, Minute ?? 0, Second ?? 0);

        var end = new DateTime(Year, m, endDay,
            Hour ?? 23, Minute ?? 59, Second ?? 59);

        return (start, end);
    }

    // ── Equality ──────────────────────────────────────

    public bool Equals(IncompleteDate other) =>
        Year == other.Year && Month == other.Month && Day == other.Day &&
        Hour == other.Hour && Minute == other.Minute && Second == other.Second;

    public override bool Equals(object? obj) => obj is IncompleteDate other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Year, Month, Day, Hour, Minute, Second);

    public static bool operator ==(IncompleteDate left, IncompleteDate right) => left.Equals(right);

    public static bool operator !=(IncompleteDate left, IncompleteDate right) => !left.Equals(right);

    // ── Comparison ────────────────────────────────────

    /// <summary>
    /// Compares two incomplete dates component by component from coarsest to finest.
    /// Unspecified (null) components are treated as their minimum value (year: n/a, month/day: 1, time: 0).
    /// </summary>
    public int CompareTo(IncompleteDate other)
    {
        int c = Year.CompareTo(other.Year);                              if (c != 0) return c;
        c = (Month  ?? 1).CompareTo(other.Month  ?? 1);                 if (c != 0) return c;
        c = (Day    ?? 1).CompareTo(other.Day    ?? 1);                 if (c != 0) return c;
        c = (Hour   ?? 0).CompareTo(other.Hour   ?? 0);                 if (c != 0) return c;
        c = (Minute ?? 0).CompareTo(other.Minute ?? 0);                 if (c != 0) return c;
        return (Second ?? 0).CompareTo(other.Second ?? 0);
    }

    public static bool operator < (IncompleteDate l, IncompleteDate r) => l.CompareTo(r) <  0;
    public static bool operator > (IncompleteDate l, IncompleteDate r) => l.CompareTo(r) >  0;
    public static bool operator <=(IncompleteDate l, IncompleteDate r) => l.CompareTo(r) <= 0;
    public static bool operator >=(IncompleteDate l, IncompleteDate r) => l.CompareTo(r) >= 0;

    /// <summary>Human-readable representation showing only the specified components.</summary>
    public override string ToString()
    {
        List<string> parts = [$"{Year}"];

        if (Month.HasValue) parts.Add($"{Month.Value:D2}");
        if (Day.HasValue) parts.Add($"{Day.Value:D2}");
        if (Hour.HasValue || Minute.HasValue || Second.HasValue)
            parts.Add($"T{Hour ?? 0:D2}:{Minute ?? 0:D2}:{Second ?? 0:D2}");

        return string.Join("-", parts);
    }
}
