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

using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// File system storage implementation
/// Maps keys to file paths and stores values as JSON-serialized bytes
/// </summary>
public class FileSystemStorage : IStorage
{
    private readonly string _baseDirectory;

    /// <summary>
    /// Initializes a new instance of the FileSystemStorage class
    /// </summary>
    /// <param name="baseDirectory">The base directory for storage</param>
    public FileSystemStorage(string baseDirectory)
    {
        _baseDirectory = baseDirectory;
        
        if (!Directory.Exists(_baseDirectory))
        {
            Directory.CreateDirectory(_baseDirectory);
        }
    }

    /// <summary>
    /// Reads data from storage by key
    /// </summary>
    /// <param name="key">The key to read</param>
    /// <returns>The data bytes, or null if not found</returns>
    public byte[]? Read(string key)
    {
        string filePath = GetFilePath(key);
        
        if (!File.Exists(filePath))
        {
            return null;
        }

        return File.ReadAllBytes(filePath);
    }

    /// <summary>
    /// Writes data to storage by key
    /// </summary>
    /// <param name="key">The key to write</param>
    /// <param name="data">The data bytes to write</param>
    public void Write(string key, byte[] data)
    {
        string filePath = GetFilePath(key);
        string? directory = Path.GetDirectoryName(filePath);

        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllBytes(filePath, data);
    }

    /// <summary>
    /// Checks if a key exists in storage
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key exists, false otherwise</returns>
    public bool Exists(string key)
    {
        string filePath = GetFilePath(key);
        return File.Exists(filePath);
    }

    /// <summary>
    /// Deletes data from storage by key
    /// </summary>
    /// <param name="key">The key to delete</param>
    public void Delete(string key)
    {
        string filePath = GetFilePath(key);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Converts a storage key to a file path
    /// </summary>
    /// <param name="key">The storage key</param>
    /// <returns>The file path</returns>
    private string GetFilePath(string key)
    {
        string safeKey = key.Replace("..", string.Empty).Replace("/", Path.DirectorySeparatorChar.ToString());
        return Path.Combine(_baseDirectory, safeKey);
    }
}
