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
/// Implements the Gregorian (solar) calendar system.
/// Components: year, month, day, hour, minute, second, weekday.
/// </summary>
public class GregorianCalendar : CalendarBase
{
    // Component IDs
    private const string Year    = "year";
    private const string Month   = "month";
    private const string Day     = "day";
    private const string Hour    = "hour";
    private const string Minute  = "minute";
    private const string Second  = "second";
    private const string Weekday = "weekday";

    private readonly DefaultLocalizationBase _localization;

    /// <summary>
    /// Initializes a new instance of <see cref="GregorianCalendar"/> with the given localization provider.
    /// </summary>
    /// <param name="localization">The localization provider used for month names and component labels.</param>
    public GregorianCalendar(DefaultLocalizationBase localization)
    {
        _localization = localization;
    }

    /// <inheritdoc/>
    public override string CalendarId => "gregorian";

    /// <inheritdoc/>
    public override string CalendarName => _localization.CalendarGregorianName;

    /// <inheritdoc/>
    public override Dictionary<string, string> GetComponents() => new()
    {
        { Year,    _localization.CalendarComponentYear    },
        { Month,   _localization.CalendarComponentMonth   },
        { Day,     _localization.CalendarComponentDay     },
        { Hour,    _localization.CalendarComponentHour    },
        { Minute,  _localization.CalendarComponentMinute  },
        { Second,  _localization.CalendarComponentSecond  },
        { Weekday, _localization.CalendarComponentWeekday }
    };

    /// <inheritdoc/>
    public override Dictionary<string, int> FromDateTime(DateTime dateTime) => new()
    {
        { Year,    dateTime.Year              },
        { Month,   dateTime.Month             },
        { Day,     dateTime.Day               },
        { Hour,    dateTime.Hour              },
        { Minute,  dateTime.Minute            },
        { Second,  dateTime.Second            },
        { Weekday, (int)dateTime.DayOfWeek    }
    };

    /// <inheritdoc/>
    public override bool TryGetComponentValueLocalization(string componentId, int componentValue, out string? result)
    {
        result = componentId switch
        {
            Month   => _localization.GetGregorianMonthName(componentValue),
            Year    => _localization.FormatGregorianYear(componentValue),
            Day     => _localization.FormatGregorianDay(componentValue),
            Hour    => _localization.FormatGregorianHour(componentValue),
            Minute  => _localization.FormatGregorianMinute(componentValue),
            Second  => _localization.FormatGregorianSecond(componentValue),
            Weekday => _localization.GetGregorianWeekdayName(componentValue),
            _       => null
        };
        return result != null;
    }

    /// <summary>
    /// Formats components as <c>yyyy-MM-dd HH:mm:ss</c>.
    /// </summary>
    public override string Format(Dictionary<string, int> components)
    {
        var y  = components[Year];
        var mo = components[Month];
        var d  = components[Day];
        var h  = components.GetValueOrDefault(Hour,   0);
        var mi = components.GetValueOrDefault(Minute, 0);
        var s  = components.GetValueOrDefault(Second, 0);
        return $"{y:D4}-{mo:D2}-{d:D2} {h:D2}:{mi:D2}:{s:D2}";
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
    /// Returns a localized human-readable string via <see cref="DefaultLocalizationBase.LocalizeGregorianDateTime"/>.
    /// </summary>
    public override string Localize(Dictionary<string, int> components)
    {
        var y  = components[Year];
        var mo = components[Month];
        var d  = components[Day];
        var h  = components.GetValueOrDefault(Hour,   0);
        var mi = components.GetValueOrDefault(Minute, 0);
        var s  = components.GetValueOrDefault(Second, 0);
        return _localization.LocalizeGregorianDateTime(y, mo, d, h, mi, s);
    }
}
