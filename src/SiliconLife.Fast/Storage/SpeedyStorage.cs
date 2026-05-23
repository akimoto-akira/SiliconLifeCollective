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

using System.Text;
using SiliconLife.Collective;
using SiliconLife.Speedy;

namespace SiliconLife.Fast;

/// <summary>
/// <see cref="IStorage"/> adapter backed by a <see cref="SpeedyPack"/> (.spk) file.
/// Path mapping is identical to <c>FileSystemStorage</c>: keys without an extension
/// get <c>.json</c> appended; <c>.md</c> keys are stored/retrieved as raw UTF-8 text.
/// </summary>
/// <remarks>
/// The underlying <see cref="SpeedyPack"/> is the single instance owned by
/// <see cref="SpeedyPackRegistry"/>. Disposing this wrapper does <em>not</em> close
/// the pack — call <see cref="SpeedyPackRegistry.Dispose"/> during application shutdown.
/// </remarks>
public sealed class SpeedyStorage : IStorage, IDisposable
{
    private readonly SpeedyPack _pack;
    private readonly string _basePath;

    /// <summary>
    /// Wraps the single shared <see cref="SpeedyPack"/> from
    /// <see cref="SpeedyPackRegistry"/> as an <see cref="IStorage"/> implementation.
    /// </summary>
    /// <param name="basePath">The base path prefix for key isolation. Can be empty for global storage.</param>
    public SpeedyStorage(string basePath = "")
    {
        _pack = SpeedyPackRegistry.Pack;
        _basePath = basePath.TrimEnd('/', '\\');
        if (!string.IsNullOrEmpty(_basePath) && !_basePath.EndsWith("/"))
        {
            _basePath += "/";
        }
    }

    // ─── Path mapping ─────────────────────────────────────────────────────────

    private string MapKey(string key)
    {
        string safeKey = key.Replace("..", string.Empty);
        safeKey = safeKey.Replace('\\', '/');
        if (string.IsNullOrEmpty(Path.GetExtension(safeKey)))
            safeKey += ".json";
        
        // Prepend base path for isolation if configured
        if (!string.IsNullOrEmpty(_basePath))
        {
            safeKey = _basePath + safeKey;
        }
        
        return safeKey;
    }

    // ─── IStorage ─────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public T[] Read<T>(string key)
    {
        string mappedPath = MapKey(key);

        if (typeof(T) == typeof(string) && key.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            byte[]? rawBytes = _pack.Read(mappedPath);
            if (rawBytes is null) return Array.Empty<T>();
            return new T[] { (T)(object)Encoding.UTF8.GetString(rawBytes) };
        }

        var result = _pack.Read<T>(mappedPath);
        return result is not null ? new T[] { result } : Array.Empty<T>();
    }

    /// <inheritdoc/>
    public void Write<T>(string key, T data)
    {
        string mappedPath = MapKey(key);

        if (data is string textData && key.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            _pack.Write(mappedPath, Encoding.UTF8.GetBytes(textData));
            return;
        }

        _pack.Write<T>(mappedPath, data);
    }

    /// <inheritdoc/>
    public bool Exists(string key) => _pack.Exists(MapKey(key));

    /// <inheritdoc/>
    public void Delete(string key) => _pack.Delete(MapKey(key));

    /// <inheritdoc/>
    public IEnumerable<string> ListKeys(string prefix = "")
    {
        var keys = new List<string>();
        if (string.IsNullOrWhiteSpace(prefix))
        {
            return keys;
        }
        
        // List entries (files) in the prefix directory
        foreach (string entry in _pack.ListEntries(prefix))
        {
            // Remove the base path prefix to get the relative key
            string relativeKey = entry;
            if (!string.IsNullOrEmpty(_basePath) && relativeKey.StartsWith(_basePath))
            {
                relativeKey = relativeKey[_basePath.Length..];
            }
            keys.Add(relativeKey);
        }

        // List subdirectories
        foreach (string dir in _pack.ListDirectories(prefix))
        {
            // Remove the base path prefix to get the relative key
            string relativeKey = dir;
            if (!string.IsNullOrEmpty(_basePath) && relativeKey.StartsWith(_basePath))
            {
                relativeKey = relativeKey[_basePath.Length..];
            }
            // Ensure directory keys end with '/'
            if (!relativeKey.EndsWith("/"))
            {
                relativeKey += "/";
            }
            keys.Add(relativeKey);
        }

        return keys;
    }

    // ─── IDisposable ──────────────────────────────────────────────────────────

    /// <summary>
    /// No-op. The underlying <see cref="SpeedyPack"/> lifetime is controlled by
    /// <see cref="SpeedyPackRegistry.Dispose"/>.
    /// </summary>
    public void Dispose() { }
}
