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

using System.Text.Json;
using System.Text.Json.Serialization;
using SiliconLife.Collective;
using SiliconLife.Default.Storage;

namespace SiliconLife.Default;

/// <summary>
/// File system storage implementation
/// Maps keys to file paths and stores values as JSON-serialized bytes
/// </summary>
public class FileSystemStorage : IStorage
{
    private readonly string _baseDirectory;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        Converters = { new FlexibleEnumConverter() }
    };

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
    /// Reads data from storage by key and deserializes to type T
    /// </summary>
    /// <typeparam name="T">The type to deserialize to</typeparam>
    /// <param name="key">The key to read</param>
    /// <returns>The deserialized data, or default if not found</returns>
    public T? Read<T>(string key)
    {
        string filePath = GetFilePath(key);

        if (!File.Exists(filePath))
        {
            return default;
        }

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }

    /// <summary>
    /// Writes data to storage by key with automatic JSON serialization
    /// </summary>
    /// <typeparam name="T">The type of data to serialize</typeparam>
    /// <param name="key">The key to write</param>
    /// <param name="data">The data to write</param>
    public void Write<T>(string key, T data)
    {
        string filePath = GetFilePath(key);
        string? directory = Path.GetDirectoryName(filePath);

        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonSerializer.Serialize(data, _jsonOptions);
        File.WriteAllText(filePath, json);
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
