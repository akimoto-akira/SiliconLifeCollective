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

using SiliconLife.Help;

namespace SiliconLife.App.Web.Models;

/// <summary>
/// View model for help documentation pages.
/// </summary>
public class HelpViewModel : ViewModelBase
{
    /// <summary>
    /// List of all help topics
    /// </summary>
    public List<HelpTopic> Topics { get; set; } = new();

    /// <summary>
    /// Current selected topic
    /// </summary>
    public HelpTopic? CurrentTopic { get; set; }

    /// <summary>
    /// Rendered HTML content of the current document
    /// </summary>
    public string ContentHtml { get; set; } = string.Empty;

    /// <summary>
    /// Search query string
    /// </summary>
    public string SearchQuery { get; set; } = string.Empty;

    /// <summary>
    /// Whether this is a search result page
    /// </summary>
    public bool IsSearchResult { get; set; }

    /// <summary>
    /// Previous topic in the list (for navigation)
    /// </summary>
    public HelpTopic? PreviousTopic { get; set; }

    /// <summary>
    /// Next topic in the list (for navigation)
    /// </summary>
    public HelpTopic? NextTopic { get; set; }
}
