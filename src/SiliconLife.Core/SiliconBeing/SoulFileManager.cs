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
/// Manages loading and saving of silicon being soul files through IStorage interface.
/// Soul content is stored as a string value with key "soul.md" in the being's storage.
/// </summary>
public static class SoulFileManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(SoulFileManager));
    private const string SoulKey = "soul.md";

    /// <summary>
    /// Loads the soul content from a silicon being's storage
    /// </summary>
    /// <param name="storage">The storage instance for the silicon being</param>
    /// <returns>The soul content, or null if not found</returns>
    public static string? LoadSoul(IStorage storage)
    {
        try
        {
            string? content = storage.Read<string>(SoulKey);
            if (string.IsNullOrEmpty(content))
            {
                _logger.Debug(null, "Soul not found in storage");
                return null;
            }
            _logger.Info(null, "Soul loaded from storage, length={0}", content.Length);
            return content;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to load soul from storage", ex);
            return null;
        }
    }

    /// <summary>
    /// Saves the soul content to a silicon being's storage
    /// </summary>
    /// <param name="storage">The storage instance for the silicon being</param>
    /// <param name="soulContent">The soul content to save</param>
    /// <returns>True if successful, false otherwise</returns>
    public static bool SaveSoul(IStorage storage, string soulContent)
    {
        try
        {
            storage.Write(SoulKey, soulContent);
            _logger.Info(null, "Soul saved to storage, length={0}", soulContent.Length);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to save soul to storage", ex);
            return false;
        }
    }

    /// <summary>
    /// Checks if a soul exists in the given storage
    /// </summary>
    /// <param name="storage">The storage instance for the silicon being</param>
    /// <returns>True if the soul exists, false otherwise</returns>
    public static bool SoulExists(IStorage storage)
    {
        try
        {
            bool exists = storage.Exists(SoulKey);
            _logger.Trace(null, "Soul exists check in storage = {0}", exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to check soul existence in storage", ex);
            return false;
        }
    }
}
