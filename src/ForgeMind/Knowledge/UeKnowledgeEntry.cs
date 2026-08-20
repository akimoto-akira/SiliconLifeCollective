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

namespace ForgeMind.Knowledge;

/// <summary>
/// Well-known knowledge categories (the "subject" of the textbook).
/// </summary>
public static class KnowledgeCategory
{
    /// <summary>C++ implementation solutions.</summary>
    public const string Cpp = "cpp";

    /// <summary>Blueprint implementation solutions.</summary>
    public const string Blueprint = "blueprint";

    /// <summary>Material implementation solutions.</summary>
    public const string Material = "material";

    /// <summary>Other domains (editor workflow, profiling, configuration, ...).</summary>
    public const string Other = "other";
}

/// <summary>
/// Metadata of one knowledge entry (a textbook "lesson"), stored in a
/// per-version <c>index.json</c>. The full lesson body lives in a separate
/// Markdown file (<c>entries/{id}.md</c>) loaded on demand.
/// </summary>
public sealed class UeKnowledgeEntry
{
    /// <summary>Unique entry id within its version bucket (also the Markdown file stem).</summary>
    public string Id { get; set; } = "";

    /// <summary>Category: cpp / blueprint / material / other.</summary>
    public string Category { get; set; } = KnowledgeCategory.Other;

    /// <summary>Lesson title.</summary>
    public string Title { get; set; } = "";

    /// <summary>The question this lesson answers.</summary>
    public string Question { get; set; } = "";

    /// <summary>Search keywords.</summary>
    public string[] Keywords { get; set; } = [];

    /// <summary>Short conclusion the AI can quote directly.</summary>
    public string Answer { get; set; } = "";

    /// <summary>Whether the lesson requires the ForgeMindForUE companion plugin.</summary>
    public bool RequiresCompanion { get; set; }

    /// <summary>Ids of related entries.</summary>
    public string[] Related { get; set; } = [];
}

/// <summary>
/// Per-version index file (<c>{version}/index.json</c>) schema.
/// </summary>
internal sealed class KnowledgeIndex
{
    /// <summary>Version bucket the index belongs to.</summary>
    public string Version { get; set; } = UeVersion.UniversalBucket;

    /// <summary>Entries in this bucket.</summary>
    public List<UeKnowledgeEntry> Entries { get; set; } = [];
}
