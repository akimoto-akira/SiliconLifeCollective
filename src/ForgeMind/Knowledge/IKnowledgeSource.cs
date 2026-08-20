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
/// Storage backend for the versioned UE knowledge base.
/// Both backends share one virtual layout:
/// <c>{version}/index.json</c> (metadata) and <c>{version}/entries/{id}.md</c> (body).
/// </summary>
internal interface IKnowledgeSource : IDisposable
{
    /// <summary>Human-readable backend description (for diagnostics).</summary>
    string Description { get; }

    /// <summary>Lists all available version buckets.</summary>
    IReadOnlyList<UeVersion> ListVersions();

    /// <summary>Lists the entry metadata of one version bucket.</summary>
    IReadOnlyList<UeKnowledgeEntry> ListEntries(UeVersion version);

    /// <summary>Reads the Markdown body of one entry; null when missing.</summary>
    string? ReadEntryBody(UeVersion version, string id);
}
