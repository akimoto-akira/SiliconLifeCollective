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
using SiliconLife.Collective;
using SiliconLife.Default.Storage;

namespace SiliconLife.Default;

/// <summary>
/// File system storage implementation.
/// Each key maps to a single file; the value is stored as a single-line JSON record.
/// </summary>
public class FileSystemStorage : IStorage
{
    private readonly string _baseDirectory;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        Converters = { new FlexibleEnumConverter() }
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemStorage"/> class.
    /// </summary>
    /// <param name="baseDirectory">The base directory for storage.</param>
    public FileSystemStorage(string baseDirectory)
    {
        _baseDirectory = baseDirectory;

        if (!Directory.Exists(_baseDirectory))
        {
            Directory.CreateDirectory(_baseDirectory);
        }
    }

    /// <summary>
    /// Reads data from storage by key and deserializes to type <typeparamref name="T"/>.
    /// The file is expected to contain a single-line JSON record.
    /// </summary>
    public T? Read<T>(string key)
    {
        string filePath = GetFilePath(key);

        if (!File.Exists(filePath))
            return default;

        // Read only the first non-empty line
        foreach (string line in File.ReadLines(filePath))
        {
            if (!string.IsNullOrWhiteSpace(line))
                return JsonSerializer.Deserialize<T>(line, _jsonOptions);
        }

        return default;
    }

    /// <summary>
    /// Writes data to storage by key as a single-line JSON record,
    /// overwriting any existing content.
    /// </summary>
    public void Write<T>(string key, T data)
    {
        string filePath = GetFilePath(key);
        string? directory = Path.GetDirectoryName(filePath);

        if (directory != null && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        string line = JsonSerializer.Serialize(data, _jsonOptions);
        File.WriteAllText(filePath, line + Environment.NewLine);
    }

    /// <summary>
    /// Checks if a key exists in storage.
    /// </summary>
    public bool Exists(string key)
    {
        return File.Exists(GetFilePath(key));
    }

    /// <summary>
    /// Deletes data from storage by key.
    /// </summary>
    public void Delete(string key)
    {
        string filePath = GetFilePath(key);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    /// <summary>
    /// Converts a storage key to a file path.
    /// Path traversal sequences are stripped; keys without an extension get <c>.json</c>.
    /// </summary>
    private string GetFilePath(string key)
    {
        string safeKey = key.Replace("..", string.Empty)
                            .Replace("/", Path.DirectorySeparatorChar.ToString());

        if (string.IsNullOrEmpty(Path.GetExtension(safeKey)))
            safeKey += ".json";

        return Path.Combine(_baseDirectory, safeKey);
    }
}
