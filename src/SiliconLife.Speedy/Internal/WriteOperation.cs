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

/// <summary>
/// Base class for write operations (write or delete).
/// </summary>
internal abstract class WriteOperation
{
    public string NormalizedPath { get; }

    protected WriteOperation(string normalizedPath)
    {
        NormalizedPath = normalizedPath;
    }
}

/// <summary>
/// Represents a write operation to be persisted.
/// </summary>
internal sealed class WriteEntry : WriteOperation
{
    public byte[] Data { get; }
    public string ContentType { get; }

    public WriteEntry(string normalizedPath, byte[] data, string contentType)
        : base(normalizedPath)
    {
        Data = data;
        ContentType = contentType;
    }
}

/// <summary>
/// Represents a delete operation to be persisted.
/// </summary>
internal sealed class DeleteEntry : WriteOperation
{
    public DeleteEntry(string normalizedPath)
        : base(normalizedPath)
    {
    }
}
