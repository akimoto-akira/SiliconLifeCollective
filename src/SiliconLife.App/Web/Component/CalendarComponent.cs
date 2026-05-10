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

namespace SiliconLife.App.Web.Component;

/// <summary>
/// Calendar component
/// </summary>
public class CalendarComponent : ComponentBase
{
    private DateTime _date = DateTime.Today;
    private bool _showWeekNumbers = false;

    /// <summary>
    /// Set initial date
    /// </summary>
    public CalendarComponent Date(DateTime date)
    {
        _date = date;
        return this;
    }

    /// <summary>
    /// Show week numbers
    /// </summary>
    public CalendarComponent ShowWeekNumbers(bool show = true)
    {
        _showWeekNumbers = show;
        return this;
    }

    public override string Render()
    {
        var calendar = H.Div();

        if (!string.IsNullOrEmpty(Id))
            calendar.Id(Id);

        var classes = new List<string> { "calendar" };
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        calendar.Class(string.Join(" ", classes));

        if (Style != null && Style.HasInlineStyles)
            calendar.Style(Style);

        foreach (var kvp in Attributes)
        {
            calendar.Attr(kvp.Key, kvp.Value);
        }

        // Header
        var header = H.Div().Class("calendar-header");
        header.Add(H.Button().Text("◀").Class("calendar-prev"));
        header.Add(H.H3($"{_date:yyyy年MM月}").Class("calendar-title"));
        header.Add(H.Button().Text("▶").Class("calendar-next"));
        calendar.Add(header);

        // Weekday headers
        var weekdays = H.Div().Class("calendar-weekdays");
        var dayNames = new[] { "日", "一", "二", "三", "四", "五", "六" };
        foreach (var day in dayNames)
        {
            weekdays.Add(H.Div(day).Class("calendar-weekday"));
        }
        calendar.Add(weekdays);

        // Days grid
        var daysGrid = H.Div().Class("calendar-days");
        var firstDay = new DateTime(_date.Year, _date.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(_date.Year, _date.Month);
        var startDayOfWeek = (int)firstDay.DayOfWeek;

        // Empty cells before first day
        for (int i = 0; i < startDayOfWeek; i++)
        {
            daysGrid.Add(H.Div().Class("calendar-day empty"));
        }

        // Day cells
        for (int day = 1; day <= daysInMonth; day++)
        {
            var isToday = day == DateTime.Today.Day && 
                         _date.Month == DateTime.Today.Month && 
                         _date.Year == DateTime.Today.Year;
            
            var dayCell = H.Div(day.ToString()).Class("calendar-day" + (isToday ? " today" : ""));
            daysGrid.Add(dayCell);
        }

        calendar.Add(daysGrid);

        return calendar.Build();
    }
}
