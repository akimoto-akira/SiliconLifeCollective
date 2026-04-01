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
/// Storage interface for key-value data persistence
/// </summary>
public interface IStorage
{
    /// <summary>
    /// Reads data from storage by key
    /// </summary>
    /// <param name="key">The key to read</param>
    /// <returns>The data bytes, or null if not found</returns>
    byte[]? Read(string key);

    /// <summary>
    /// Writes data to storage by key
    /// </summary>
    /// <param name="key">The key to write</param>
    /// <param name="data">The data bytes to write</param>
    void Write(string key, byte[] data);

    /// <summary>
    /// Checks if a key exists in storage
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key exists, false otherwise</returns>
    bool Exists(string key);

    /// <summary>
    /// Deletes data from storage by key
    /// </summary>
    /// <param name="key">The key to delete</param>
    void Delete(string key);
}
