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
/// Manages loading and saving of silicon being custom code through IStorage interface.
/// Code is stored as encrypted bytes with key "code.enc" in the being's storage.
/// </summary>
public static class BeingCodeFileManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(BeingCodeFileManager));
    private const string CodeKey = "code.enc";

    /// <summary>
    /// Saves encrypted code to a silicon being's storage
    /// </summary>
    /// <param name="storage">The storage instance for the silicon being</param>
    /// <param name="encryptedCode">The encrypted code bytes to save</param>
    /// <returns>True if successful, false otherwise</returns>
    public static bool SaveCode(IStorage storage, byte[] encryptedCode)
    {
        try
        {
            storage.Write(CodeKey, encryptedCode);
            _logger.Info(null, "Being code saved to storage, size={0} bytes", encryptedCode.Length);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to save being code to storage", ex);
            return false;
        }
    }

    /// <summary>
    /// Loads encrypted code from a silicon being's storage
    /// </summary>
    /// <param name="storage">The storage instance for the silicon being</param>
    /// <returns>The encrypted code bytes, or null if not found</returns>
    public static byte[]? LoadCode(IStorage storage)
    {
        try
        {
            byte[][] codes = storage.Read<byte[]>(CodeKey);
            byte[]? code = codes.FirstOrDefault();
            if (code == null || code.Length == 0)
            {
                _logger.Debug(null, "Being code not found in storage");
                return null;
            }
            _logger.Info(null, "Being code loaded from storage, size={0} bytes", code.Length);
            return code;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to load being code from storage", ex);
            return null;
        }
    }

    /// <summary>
    /// Checks if custom code exists in the given storage
    /// </summary>
    /// <param name="storage">The storage instance for the silicon being</param>
    /// <returns>True if the code exists, false otherwise</returns>
    public static bool CodeExists(IStorage storage)
    {
        try
        {
            bool exists = storage.Exists(CodeKey);
            _logger.Trace(null, "Being code exists check in storage = {0}", exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to check being code existence in storage", ex);
            return false;
        }
    }

    /// <summary>
    /// Deletes custom code from a silicon being's storage
    /// </summary>
    /// <param name="storage">The storage instance for the silicon being</param>
    /// <returns>True if successful or code didn't exist, false on error</returns>
    public static bool DeleteCode(IStorage storage)
    {
        try
        {
            if (storage.Exists(CodeKey))
            {
                storage.Delete(CodeKey);
                _logger.Info(null, "Being code deleted from storage");
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to delete being code from storage", ex);
            return false;
        }
    }
}
