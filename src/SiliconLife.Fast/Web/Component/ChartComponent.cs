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

namespace SiliconLife.Fast.Web.Component;

/// <summary>
/// Chart component (placeholder for chart library integration)
/// </summary>
public class ChartComponent : ComponentBase
{
    private string _chartType = "bar";
    private readonly List<(string Label, double Value)> _data = new();
    private string _title = "";

    /// <summary>
    /// Set chart type (bar/line/pie)
    /// </summary>
    public ChartComponent Type(string type)
    {
        _chartType = type;
        return this;
    }

    /// <summary>
    /// Add data point
    /// </summary>
    public ChartComponent AddData(string label, double value)
    {
        _data.Add((label, value));
        return this;
    }

    /// <summary>
    /// Set chart title
    /// </summary>
    public ChartComponent Title(string title)
    {
        _title = title;
        return this;
    }

    public override string Render()
    {
        var chart = H.Div();

        if (!string.IsNullOrEmpty(Id))
            chart.Id(Id);

        var classes = new List<string> { "chart" };
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        chart.Class(string.Join(" ", classes));

        if (!string.IsNullOrEmpty(Style))
            chart.Style(Style);

        foreach (var kvp in Attributes)
        {
            chart.Attr(kvp.Key, kvp.Value);
        }

        // Chart container with data attributes for JS chart library
        chart.Attr("data-chart-type", _chartType);
        chart.Attr("data-chart-data", System.Text.Json.JsonSerializer.Serialize(_data));

        if (!string.IsNullOrEmpty(_title))
        {
            chart.Add(H.H3(H.Escape(_title)).Class("chart-title"));
        }

        // Placeholder div for chart rendering
        chart.Add(H.Div().Class("chart-canvas"));

        return chart.Build();
    }
}
