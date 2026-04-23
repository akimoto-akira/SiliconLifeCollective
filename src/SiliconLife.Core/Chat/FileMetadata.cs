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

namespace SiliconLife.Collective;

/// <summary>
/// File metadata attached to a <see cref="ChatMessage"/> when its
/// <see cref="ChatMessage.Type"/> is <see cref="MessageType.File"/>.
/// Describes the file's identity, size, MIME type and storage location.
/// </summary>
public class FileMetadata
{
    /// <summary>
    /// Gets or sets the file path (local absolute path or server-relative path).
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the original file name (with extension).
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file size in bytes.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of the file (e.g. "application/pdf").
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// Gets or sets whether the path refers to a local file on the same machine
    /// (typical when connecting from 127.0.0.1).
    /// </summary>
    public bool IsLocalPath { get; set; }

    /// <summary>
    /// Gets or sets the upload timestamp.
    /// </summary>
    public DateTime UploadedAt { get; set; } = DateTime.Now;
}
