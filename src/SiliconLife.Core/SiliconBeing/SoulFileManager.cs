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
/// Manages loading and saving of silicon being soul files
/// </summary>
public static class SoulFileManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(SoulFileManager));
    private const string SoulFileName = "soul.md";

    /// <summary>
    /// Loads the soul content from a silicon being's directory
    /// </summary>
    /// <param name="beingDirectory">The directory path of the silicon being</param>
    /// <returns>The soul content, or null if the file does not exist</returns>
    public static string? LoadSoul(string beingDirectory)
    {
        string soulFilePath = Path.Combine(beingDirectory, SoulFileName);

        if (!File.Exists(soulFilePath))
        {
            _logger.Debug(null, "Soul file not found: {0}", soulFilePath);
            return null;
        }

        try
        {
            string content = File.ReadAllText(soulFilePath);
            _logger.Info(null, "Soul loaded from {0}, length={1}", soulFilePath, content.Length);
            return content;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to load soul from {0}", soulFilePath, ex);
            return null;
        }
    }

    /// <summary>
    /// Saves the soul content to a silicon being's directory
    /// </summary>
    /// <param name="beingDirectory">The directory path of the silicon being</param>
    /// <param name="soulContent">The soul content to save</param>
    /// <returns>True if successful, false otherwise</returns>
    public static bool SaveSoul(string beingDirectory, string soulContent)
    {
        try
        {
            if (!Directory.Exists(beingDirectory))
            {
                Directory.CreateDirectory(beingDirectory);
            }

            string soulFilePath = Path.Combine(beingDirectory, SoulFileName);
            File.WriteAllText(soulFilePath, soulContent);
            _logger.Info(null, "Soul saved to {0}, length={1}", soulFilePath, soulContent.Length);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to save soul to {0}", Path.Combine(beingDirectory, SoulFileName), ex);
            return false;
        }
    }

    /// <summary>
    /// Checks if a soul file exists in the given directory
    /// </summary>
    /// <param name="beingDirectory">The directory path of the silicon being</param>
    /// <returns>True if the soul file exists, false otherwise</returns>
    public static bool SoulExists(string beingDirectory)
    {
        string soulFilePath = Path.Combine(beingDirectory, SoulFileName);
        bool exists = File.Exists(soulFilePath);
        _logger.Trace(null, "Soul file exists check: {0} = {1}", soulFilePath, exists);
        return exists;
    }
}
