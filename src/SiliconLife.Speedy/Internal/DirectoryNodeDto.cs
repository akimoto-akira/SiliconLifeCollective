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

namespace SiliconLife.Speedy.Internal;

using MessagePack;

/// <summary>
/// Hierarchical directory tree node used as the on-disk representation (v3 format)
/// of the .spk Directory area. Each node holds an optional entry (when the path
/// itself is a key) and a map of child segments to their sub-trees.
/// Storing the directory as a tree (instead of a flat path->entry dictionary)
/// keeps path segments deduplicated on disk and lets <see cref="DirectoryMap"/>
/// rebuild its in-memory tree directly without per-entry insertion.
/// </summary>
[MessagePackObject(AllowPrivate = true)]
internal sealed class DirectoryNodeDto
{
    /// <summary>
    /// Entry stored at this exact path, or null when this node is only a
    /// directory prefix of deeper entries.
    /// </summary>
    [Key(0)]
    public DirectoryEntry? Entry { get; set; }

    /// <summary>
    /// Child segments (single path component each) mapped to their sub-trees.
    /// Null (rather than empty) when the node has no children, to save disk space.
    /// </summary>
    [Key(1)]
    public Dictionary<string, DirectoryNodeDto>? Children { get; set; }
}
