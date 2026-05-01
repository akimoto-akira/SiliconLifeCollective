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

namespace SiliconLife.Speedy;

/// <summary>
/// Represents an atomic transaction over a SpeedyPack instance.
/// Operations are buffered locally until Commit is called.
/// Disposing without committing automatically rolls back.
/// </summary>
public interface IPackTransaction : IDisposable
{
    /// <summary>Writes raw bytes to path within the transaction.</summary>
    void Write(string path, ReadOnlySpan<byte> data);

    /// <summary>Writes raw bytes to path within the transaction.</summary>
    void Write(string path, byte[] data);

    /// <summary>Serializes value as JSON and writes it to path.</summary>
    void Write<T>(string path, T value);

    /// <summary>Marks path for deletion within the transaction.</summary>
    void Delete(string path);

    /// <summary>
    /// Atomically commits all buffered operations to the main cache and write queue.
    /// After commit, changes are immediately visible to readers.
    /// </summary>
    void Commit();

    /// <summary>
    /// Discards all buffered operations. The main cache and file are unaffected.
    /// </summary>
    void Rollback();

    /// <summary>Whether Commit has been called successfully.</summary>
    bool IsCommitted { get; }
}
