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

using SiliconLife.Collective;
using SiliconLife.Common.Localization;

namespace SiliconLife.Common.Calendar;

/// <summary>
/// Abstract base class for calendar systems.
/// Provides a common interface for date conversion and calendar-specific information.
/// </summary>
public abstract class CalendarBase
{
    /// <summary>
    /// Gets the unique identifier of this calendar system (e.g., "gregorian", "lunar").
    /// </summary>
    public abstract string CalendarId { get; }

    /// <summary>
    /// Gets the display name of this calendar system.
    /// </summary>
    public abstract string CalendarName { get; }

    /// <summary>
    /// Gets the components of this calendar system, keyed by component ID with localized display names as values.
    /// </summary>
    public abstract Dictionary<string, string> GetComponents();

    /// <summary>
    /// Converts a <see cref="DateTime"/> to the component values of this calendar system,
    /// keyed by component ID with their numeric values.
    /// </summary>
    /// <param name="dateTime">The date and time to convert.</param>
    /// <returns>A dictionary mapping component IDs to their numeric values.</returns>
    public abstract Dictionary<string, int> FromDateTime(DateTime dateTime);

    /// <summary>
    /// Tries to get the localized string for a component value.
    /// </summary>
    /// <param name="componentId">The component identifier.</param>
    /// <param name="componentValue">The numeric value of the component.</param>
    /// <param name="result">The localized string if found; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if a localized value was found; otherwise <see langword="false"/>.</returns>
    public abstract bool TryGetComponentValueLocalization(string componentId, int componentValue, out string? result);

    /// <summary>
    /// Formats a set of calendar component values into a string representation.
    /// </summary>
    /// <param name="components">The component values keyed by component ID.</param>
    /// <returns>A string representation of the date in this calendar system.</returns>
    public abstract string Format(Dictionary<string, int> components);

    /// <summary>
    /// Parses a string representation back into calendar component values.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>A dictionary mapping component IDs to their numeric values.</returns>
    public abstract Dictionary<string, int> Parse(string value);

    /// <summary>
    /// Localizes a set of calendar component values into a human-readable string,
    /// using <see cref="TryGetComponentValueLocalization"/> for each component.
    /// </summary>
    /// <param name="components">The component values keyed by component ID.</param>
    /// <returns>A localized string representation of the date.</returns>
    public abstract string Localize(Dictionary<string, int> components);

    /// <summary>
    /// Builds a registry of all available calendar systems.
    /// </summary>
    /// <returns>A dictionary mapping calendar IDs to calendar instances.</returns>
    public static Dictionary<string, CalendarBase> BuildCalendarRegistry()
    {
        DefaultLocalizationBase loc = GetLocalization();
        var registry = new Dictionary<string, CalendarBase>(StringComparer.OrdinalIgnoreCase);

        // ── registered calendars ──────────────────────────────────────────────
        Register(registry, new GregorianCalendar(loc));
        Register(registry, new BuddhistCalendar(loc));
        Register(registry, new JucheCalendar(loc));
        Register(registry, new RepublicOfChinaCalendar(loc));
        Register(registry, new ChulaSakaratCalendar(loc));
        Register(registry, new JulianCalendar(loc));
        Register(registry, new KhmerCalendar(loc));
        Register(registry, new ZoroastrianCalendar(loc));
        Register(registry, new FrenchRepublicanCalendar(loc));
        Register(registry, new CopticCalendar(loc));
        Register(registry, new EthiopianCalendar(loc));
        Register(registry, new IslamicCalendar(loc));
        Register(registry, new HebrewCalendar(loc));
        Register(registry, new PersianCalendar(loc));
        Register(registry, new IndianCalendar(loc));
        Register(registry, new SakaCalendar(loc));
        Register(registry, new VikramSamvatCalendar(loc));
        Register(registry, new JavaneseCalendar(loc));
        Register(registry, new MongolianCalendar(loc));
        Register(registry, new TibetanCalendar(loc));
        Register(registry, new MayanCalendar(loc));
        Register(registry, new CherokeeCalendar(loc));
        Register(registry, new InuitCalendar(loc));
        Register(registry, new RomanCalendar(loc));
        Register(registry, new ChineseLunarCalendar(loc));
        Register(registry, new VietnameseCalendar(loc));
        Register(registry, new JapaneseCalendar(loc));
        Register(registry, new SexagenaryCalendar(loc));
        Register(registry, new YiCalendar(loc));
        Register(registry, new DaiCalendar(loc));
        Register(registry, new DehongDaiCalendar(loc));
        // ─────────────────────────────────────────────────────────────────────

        return registry;
    }

    private static void Register(Dictionary<string, CalendarBase> registry, CalendarBase calendar)
        => registry[calendar.CalendarId] = calendar;

    /// <summary>
    /// Gets the current localization instance.
    /// </summary>
    public static DefaultLocalizationBase GetLocalization()
    {
        Language language = Config.Instance?.Data?.Language ?? Language.ZhCN;
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc;

        return LocalizationManager.Instance.GetLocalization(Language.ZhCN) as DefaultLocalizationBase
               ?? throw new InvalidOperationException("No DefaultLocalizationBase registered.");
    }
}