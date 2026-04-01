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
/// Default factory for creating silicon being instances
/// </summary>
public class DefaultSiliconBeingFactory : ISiliconBeingFactory
{
    private readonly IAIClient _aiClient;
    private readonly IStorage _storage;
    private readonly string _dataDirectory;
    private readonly ChatSystem? _chatSystem;
    private readonly IMManager? _imManager;
    private readonly Guid _userId;

    /// <summary>
    /// Initializes a new instance of the DefaultSiliconBeingFactory class
    /// </summary>
    /// <param name="aiClient">The AI client to use for created beings</param>
    /// <param name="storage">The storage instance to use</param>
    /// <param name="dataDirectory">The base data directory</param>
    public DefaultSiliconBeingFactory(
        IAIClient aiClient,
        IStorage storage,
        string dataDirectory)
        : this(aiClient, storage, dataDirectory, null, null, Guid.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the DefaultSiliconBeingFactory class with ChatSystem and IMManager
    /// </summary>
    public DefaultSiliconBeingFactory(
        IAIClient aiClient,
        IStorage storage,
        string dataDirectory,
        ChatSystem chatSystem,
        IMManager imManager,
        Guid userId)
    {
        _aiClient = aiClient;
        _storage = storage;
        _dataDirectory = dataDirectory;
        _chatSystem = chatSystem;
        _imManager = imManager;
        _userId = userId;
    }

    /// <summary>
    /// Creates a silicon being with the specified ID and name
    /// </summary>
    /// <param name="id">The unique identifier for the silicon being</param>
    /// <param name="name">The name of the silicon being</param>
    /// <returns>The created silicon being instance</returns>
    public SiliconBeingBase CreateBeing(Guid id, string name)
    {
        string beingDirectory = Path.Combine(_dataDirectory, "SiliconManager", id.ToString());

        if (!Directory.Exists(beingDirectory))
        {
            Directory.CreateDirectory(beingDirectory);
        }

        string? soulContent = SoulFileManager.LoadSoul(beingDirectory);

        if (_chatSystem != null && _imManager != null)
        {
            return new DefaultSiliconBeing(id, name, _aiClient, beingDirectory, soulContent, _chatSystem, _imManager, _userId);
        }

        return new DefaultSiliconBeing(id, name, _aiClient, beingDirectory, soulContent);
    }

    /// <summary>
    /// Loads a silicon being from its directory
    /// </summary>
    /// <param name="beingDirectory">The directory path of the silicon being</param>
    /// <returns>The loaded silicon being instance, or null if loading fails</returns>
    public SiliconBeingBase? LoadBeing(string beingDirectory)
    {
        try
        {
            string directoryName = Path.GetFileName(beingDirectory);

            if (!Guid.TryParse(directoryName, out Guid id))
            {
                return null;
            }

            string stateFilePath = Path.Combine(beingDirectory, "state.json");

            string name = "Unknown";

            if (File.Exists(stateFilePath))
            {
                try
                {
                    string json = File.ReadAllText(stateFilePath);
                    Dictionary<string, string>? state = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    if (state != null && state.ContainsKey("name"))
                    {
                        name = state["name"];
                    }
                }
                catch
                {
                }
            }

            string? soulContent = SoulFileManager.LoadSoul(beingDirectory);

            if (_chatSystem != null && _imManager != null)
            {
                return new DefaultSiliconBeing(id, name, _aiClient, beingDirectory, soulContent, _chatSystem, _imManager, _userId);
            }

            return new DefaultSiliconBeing(id, name, _aiClient, beingDirectory, soulContent);
        }
        catch
        {
            return null;
        }
    }
}
