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
/// Default factory for creating silicon being instances.
/// Creates ToolManager and PermissionManager for each being.
/// </summary>
public class DefaultSiliconBeingFactory : ISiliconBeingFactory
{
    private readonly IAIClient _aiClient;
    private readonly IStorage _storage;
    private readonly ITimeStorage _timeStorage;
    private readonly string _dataDirectory;
    private readonly Guid _userId;
    private readonly IPermissionCallback? _permissionCallback;
    private readonly IPermissionAskHandler? _askHandler;

    /// <summary>
    /// Initializes a new instance of the DefaultSiliconBeingFactory class
    /// </summary>
    /// <param name="aiClient">The AI client to use for created beings</param>
    /// <param name="storage">The storage instance to use</param>
    /// <param name="dataDirectory">The base data directory</param>
    public DefaultSiliconBeingFactory(
        IAIClient aiClient,
        IStorage storage,
        ITimeStorage timeStorage,
        string dataDirectory)
        : this(aiClient, storage, timeStorage, dataDirectory, Guid.Empty, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the DefaultSiliconBeingFactory class with user ID
    /// </summary>
    public DefaultSiliconBeingFactory(
        IAIClient aiClient,
        IStorage storage,
        ITimeStorage timeStorage,
        string dataDirectory,
        Guid userId)
        : this(aiClient, storage, timeStorage, dataDirectory, userId, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance with permission components
    /// </summary>
    public DefaultSiliconBeingFactory(
        IAIClient aiClient,
        IStorage storage,
        ITimeStorage timeStorage,
        string dataDirectory,
        Guid userId,
        IPermissionCallback? permissionCallback,
        IPermissionAskHandler? askHandler)
    {
        _aiClient = aiClient;
        _storage = storage;
        _timeStorage = timeStorage;
        _dataDirectory = dataDirectory;
        _userId = userId;
        _permissionCallback = permissionCallback;
        _askHandler = askHandler;
    }

    /// <summary>
    /// Creates a silicon being with the specified ID and name.
    /// Automatically creates a ToolManager and PermissionManager.
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

        return CreateAndConfigureBeing(id, name, beingDirectory);
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

            return CreateAndConfigureBeing(id, name, beingDirectory);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Creates a DefaultSiliconBeing and configures its properties, ToolManager, and PermissionManager.
    /// Curators (IsCurator=true) get ALL tools; normal beings get only non-curator tools.
    /// The only difference between curator and normal being is tool access.
    /// </summary>
    private SiliconBeingBase CreateAndConfigureBeing(Guid id, string name, string beingDirectory)
    {
        string? soulContent = SoulFileManager.LoadSoul(beingDirectory);

        // Determine if this is the curator
        Guid curatorGuid = Config.Instance?.Data?.CuratorGuid ?? Guid.Empty;
        bool isCurator = id == curatorGuid;

        DefaultSiliconBeing being = new(id, name);
        being.AIClient = _aiClient;
        being.SoulContent = soulContent;
        being.UserId = _userId;

        // Create and configure ToolManager for this being
        // Curators get ALL tools (normal + curator-only); normal beings get only non-curator tools
        ToolManager toolManager = new ToolManager(curatorOnly: isCurator);
        if (isCurator)
        {
            toolManager.ScanAssemblyAll(typeof(DefaultSiliconBeingFactory).Assembly);
        }
        else
        {
            toolManager.ScanAssembly(typeof(DefaultSiliconBeingFactory).Assembly);
        }
        being.ToolManager = toolManager;

        // Create PermissionManager for this being
        GlobalACL? globalAcl = ServiceLocator.Instance.GlobalAcl;
        if (globalAcl != null)
        {
            PermissionManager pm = new PermissionManager(
                being,
                globalAcl,
                _permissionCallback,
                _askHandler);

            being.PermissionManager = pm;
            ServiceLocator.Instance.RegisterPermissionManager(id, pm);
        }

        // Create TimeStorage for this being (separate directory per being)
        string beingTimeStorageDir = Path.Combine(_dataDirectory, "SiliconManager", id.ToString(), "memory");
        ITimeStorage beingTimeStorage = new FileSystemTimeStorage(beingTimeStorageDir);
        being.TimeStorage = beingTimeStorage;

        // Create Memory, TaskSystem, TimerSystem for this being
        being.Memory = new Memory(beingTimeStorage);
        being.TaskSystem = new TaskSystem(_storage);
        being.TimerSystem = new TimerSystem(_storage);

        return being;
    }
}
