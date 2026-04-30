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
/// Table component
/// </summary>
public class TableComponent : ComponentBase
{
    private readonly List<string> _headers = new();
    private readonly List<List<string>> _rows = new();
    private bool _bordered = false;
    private bool _striped = false;

    /// <summary>
    /// Add table headers
    /// </summary>
    public TableComponent AddHeader(params string[] headers)
    {
        _headers.AddRange(headers);
        return this;
    }

    /// <summary>
    /// Add a row of data
    /// </summary>
    public TableComponent AddRow(params string[] cells)
    {
        _rows.Add(new List<string>(cells));
        return this;
    }

    /// <summary>
    /// Add multiple rows of data
    /// </summary>
    public TableComponent AddRows(IEnumerable<string[]> rows)
    {
        foreach (var row in rows)
        {
            _rows.Add(new List<string>(row));
        }
        return this;
    }

    /// <summary>
    /// Set bordered style
    /// </summary>
    public TableComponent Bordered(bool bordered = true)
    {
        _bordered = bordered;
        return this;
    }

    /// <summary>
    /// Set striped style
    /// </summary>
    public TableComponent Striped(bool striped = true)
    {
        _striped = striped;
        return this;
    }

    public override string Render()
    {
        var table = H.Table();

        if (!string.IsNullOrEmpty(Id))
            table.Id(Id);

        var classes = new List<string>();
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        if (_bordered)
            classes.Add("table-bordered");
        if (_striped)
            classes.Add("table-striped");
        
        if (classes.Count > 0)
            table.Class(string.Join(" ", classes));

        if (!string.IsNullOrEmpty(Style))
            table.Style(Style);

        foreach (var kvp in Attributes)
        {
            table.Attr(kvp.Key, kvp.Value);
        }

        // Table head
        if (_headers.Count > 0)
        {
            var thead = H.Thead();
            var headerRow = H.Tr();
            foreach (var header in _headers)
            {
                headerRow.Add(H.Th(H.Escape(header)));
            }
            thead.Add(headerRow);
            table.Add(thead);
        }

        // Table body
        if (_rows.Count > 0)
        {
            var tbody = H.Tbody();
            foreach (var row in _rows)
            {
                var tr = H.Tr();
                foreach (var cell in row)
                {
                    tr.Add(H.Td(H.Escape(cell)));
                }
                tbody.Add(tr);
            }
            table.Add(tbody);
        }

        return table.Build();
    }
}
