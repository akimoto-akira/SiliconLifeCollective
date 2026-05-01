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
/// Directory entry metadata describing a data block stored in a .spk file.
/// </summary>
[MessagePackObject]
internal sealed class DirectoryEntry
{
    /// <summary>
    /// Offset of the data block in the file (4K aligned).
    /// </summary>
    [Key(0)]
    public long Offset { get; set; }

    /// <summary>
    /// Length of the data block in bytes.
    /// </summary>
    [Key(1)]
    public int Length { get; set; }

    /// <summary>
    /// Content type: "raw", "json", or "text".
    /// </summary>
    [Key(2)]
    public string ContentType { get; set; } = "raw";

    /// <summary>
    /// Creation timestamp (UTC).
    /// </summary>
    [Key(3)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last update timestamp (UTC).
    /// </summary>
    [Key(4)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
