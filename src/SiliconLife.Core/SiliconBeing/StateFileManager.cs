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
/// Manages loading and saving of silicon being state through IStorage interface.
/// State includes being name, AI client type, and AI configuration.
/// </summary>
public static class StateFileManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(StateFileManager));
    private const string StateKey = "state.json";

    /// <summary>
    /// Represents the persistable state of a silicon being
    /// </summary>
    public class BeingState
    {
        public string Name { get; set; } = string.Empty;
        public string AIClientType { get; set; } = string.Empty;
        public Dictionary<string, object> AIConfig { get; set; } = new();
    }

    /// <summary>
    /// Loads the being state from storage
    /// </summary>
    /// <param name="storage">The storage instance for the silicon being</param>
    /// <returns>The being state, or null if not found</returns>
    public static BeingState? LoadState(IStorage storage)
    {
        try
        {
            BeingState? state = storage.Read<BeingState>(StateKey);
            if (state == null)
            {
                _logger.Debug(null, "State not found in storage");
                return null;
            }
            _logger.Info(null, "State loaded from storage: Name={0}, AIClientType={1}", state.Name, state.AIClientType);
            return state;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to load state from storage", ex);
            return null;
        }
    }

    /// <summary>
    /// Saves the being state to storage
    /// </summary>
    /// <param name="storage">The storage instance for the silicon being</param>
    /// <param name="state">The state to save</param>
    /// <returns>True if successful, false otherwise</returns>
    public static bool SaveState(IStorage storage, BeingState state)
    {
        try
        {
            storage.Write(StateKey, state);
            _logger.Info(null, "State saved to storage: Name={0}, AIClientType={1}", state.Name, state.AIClientType);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to save state to storage", ex);
            return false;
        }
    }

    /// <summary>
    /// Checks if state exists in the given storage
    /// </summary>
    /// <param name="storage">The storage instance for the silicon being</param>
    /// <returns>True if the state exists, false otherwise</returns>
    public static bool StateExists(IStorage storage)
    {
        try
        {
            bool exists = storage.Exists(StateKey);
            _logger.Trace(null, "State exists check in storage = {0}", exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to check state existence in storage", ex);
            return false;
        }
    }
}
