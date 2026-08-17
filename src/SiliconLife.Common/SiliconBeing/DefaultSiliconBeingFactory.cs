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
using SiliconLife.Common.AI;
using SiliconLife.Common.Calendar;
using SiliconLife.Common.Skills;

namespace SiliconLife.Common.SiliconBeing;

/// <summary>
/// Default factory for creating silicon being instances.
/// Creates ToolManager and PermissionManager for each being.
/// Uses <see cref="ServiceLocator.BeingPathResolver"/> to construct per-being storage paths,
/// keeping this layer free of file-system or SpeedyPack-specific path logic.
/// </summary>
public class DefaultSiliconBeingFactory : ISiliconBeingFactory
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DefaultSiliconBeingFactory>();
    private readonly Dictionary<string, object> _globalAIConfig;
    private readonly IStorage _storage;
    private readonly ITimeStorage _timeStorage;
    private readonly IPermissionCallback? _permissionCallback;
    private readonly IPermissionAskHandler? _askHandler;

    public DefaultSiliconBeingFactory(
        Dictionary<string, object> globalAIConfig,
        IStorage storage,
        ITimeStorage timeStorage)
        : this(globalAIConfig, storage, timeStorage, null, null)
    {
    }

    public DefaultSiliconBeingFactory(
        Dictionary<string, object> globalAIConfig,
        IStorage storage,
        ITimeStorage timeStorage,
        IPermissionCallback? permissionCallback,
        IPermissionAskHandler? askHandler)
    {
        _globalAIConfig = globalAIConfig;
        _storage = storage;
        _timeStorage = timeStorage;
        _permissionCallback = permissionCallback;
        _askHandler = askHandler;
    }

    private static Func<Guid, string> ResolvePathResolver()
    {
        return ServiceLocator.Instance.BeingPathResolver
            ?? throw new InvalidOperationException("BeingPathResolver not registered in ServiceLocator");
    }

    public SiliconBeingBase CreateBeing(Guid id, string name)
    {
        if (id == Guid.Empty)
        {
            id = Guid.NewGuid();
        }

        string beingDirectory = ResolvePathResolver()(id);
        return CreateAndConfigureBeing(id, name, beingDirectory);
    }

    public SiliconBeingBase? LoadBeing(string beingDirectory)
    {
        try
        {
            string directoryName = beingDirectory.TrimEnd('/', '\\').Split('/', '\\').Last();

            if (!Guid.TryParse(directoryName, out Guid id))
            {
                return null;
            }

            string resolvedDirectory = ResolvePathResolver()(id);
            DefaultSiliconBeing being = (DefaultSiliconBeing)CreateAndConfigureBeing(id, "", resolvedDirectory);
            being.LoadState();
            return being;
        }
        catch
        {
            return null;
        }
    }

    public IEnumerable<Guid> DiscoverPersistedBeingIds()
    {
        var ids = new List<Guid>();

        try
        {
            var storageFactory = ServiceLocator.Instance.StorageFactory;
            if (storageFactory != null)
            {
                IStorage rootStorage = storageFactory("");
                foreach (string key in rootStorage.ListKeys("SiliconManager"))
                {
                    string segment = key.TrimEnd('/').Split('/').Last();
                    if (Guid.TryParse(segment, out Guid id))
                    {
                        ids.Add(id);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "DiscoverPersistedBeingIds: error listing storage keys: {0}", ex.Message);
        }

        _logger.Info(null, "DiscoverPersistedBeingIds: found {0} persisted being(s)", ids.Count);
        return ids;
    }

    private SiliconBeingBase CreateAndConfigureBeing(Guid id, string name, string beingDirectory)
    {
        Guid curatorGuid = Config.Instance?.Data?.CuratorGuid ?? Guid.Empty;
        bool isCurator = id == curatorGuid;

        DefaultSiliconBeing being = new(id, name);
        being.BeingDirectory = beingDirectory;

        var storageFactory = ServiceLocator.Instance.StorageFactory
            ?? throw new InvalidOperationException("StorageFactory not registered in ServiceLocator");
        IStorage beingStorage = storageFactory(beingDirectory);
        being.Storage = beingStorage;

        string? soulContent = SoulFileManager.LoadSoul(beingStorage);
        being.SoulContent = soulContent;

        if (!being.LoadState() && !string.IsNullOrEmpty(name))
        {
            being.SaveState();
        }

        try
        {
            being.AIClient = CreateAIClientForBeing(being);
        }
        catch (Exception ex)
        {
            _logger.Warn(Guid.Empty, "Being {0}: failed to create AI client, will retry on next tick. Error: {1}", name, ex.Message);
        }

        ToolManager toolManager = new ToolManager(curatorOnly: isCurator);
        if (isCurator)
        {
            toolManager.ScanAssemblyAll(typeof(DefaultSiliconBeingFactory).Assembly);
            foreach (var asm in ServiceLocator.Instance.ToolAssemblies)
                toolManager.ScanAssemblyAll(asm);
            toolManager.ScanAllPluginAssembliesAll();
        }
        else
        {
            toolManager.ScanAssembly(typeof(DefaultSiliconBeingFactory).Assembly);
            foreach (var asm in ServiceLocator.Instance.ToolAssemblies)
                toolManager.ScanAssembly(asm);
            toolManager.ScanAllPluginAssemblies();
        }
        being.ToolManager = toolManager;

        // Skill system: builtin skills + persisted skills + plugin-provided skills
        SkillManager skillManager = new();
        foreach (var skill in BuiltinSkills.GetAllSkills())
        {
            skillManager.RegisterSkill(skill);
        }
        skillManager.RefreshFromStorage(beingStorage);
        skillManager.ScanAllPluginAssemblies();
        being.SkillManager = skillManager;
        skillManager.SyncAutoSkillTickObjects(being);

        // MCP tools: register wrapper tools from enabled MCP servers
        // (filtered per being: AllowedBeingIds / AllowedTools / permissions)
        if (McpManager.McpEnabled)
        {
            McpManager.Instance.EnsureLoaded();
            McpManager.Instance.ConnectPendingServers();
            McpManager.Instance.SyncToolsForBeing(being);
        }

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

        var timeStorageFactory = ServiceLocator.Instance.TimeStorageFactory
            ?? throw new InvalidOperationException("TimeStorageFactory not registered in ServiceLocator");
        ITimeStorage beingTimeStorage = timeStorageFactory(beingDirectory);
        being.TimeStorage = beingTimeStorage;

        being.Memory = new Memory(beingTimeStorage);
        being.TaskSystem = new TaskSystem(being);
        being.TaskEnumerator = new TaskEnumerator(id);

        Func<Dictionary<string, CalendarBase>> registryFactory = CalendarBase.BuildCalendarRegistry;
        CalendarNextOccurrenceResolver resolver = CalendarTimerResolvers.CreateResolver(registryFactory);
        CalendarDateTimeConverter converter = CalendarTimerResolvers.CreateConverter(registryFactory);
        TimerPendingChecker pendingChecker = CalendarTimerResolvers.CreatePendingChecker();
        being.TimerSystem = new TimerSystem(being, beingStorage, resolver, converter, pendingChecker);

        var workNoteStorageFactory = ServiceLocator.Instance.WorkNoteStorageFactory
            ?? throw new InvalidOperationException("WorkNoteStorageFactory not registered in ServiceLocator");
        IWorkNoteStorage workNoteStorage = workNoteStorageFactory(beingDirectory);
        being.WorkNoteSystem = new WorkNoteSystem(workNoteStorage, id);

        return being;
    }

    private IAIClient CreateAIClientForBeing(DefaultSiliconBeing being)
    {
        Dictionary<string, object> configToUse;
        string clientType;

        if (being.AIClientConfig != null && being.AIClientConfig.Count > 0)
        {
            configToUse = being.AIClientConfig;
            clientType = ResolveClientType(being.AIClientType);
        }
        else
        {
            configToUse = _globalAIConfig;
            clientType = ResolveClientType(null);
        }

        if (configToUse == null || configToUse.Count == 0)
        {
            throw new InvalidOperationException($"Being {being.Name}: no AI config available");
        }

        IAIClientFactory factory = CreateFactoryByType(clientType);
        IAIClient client = factory.CreateClient(configToUse);

        return client;
    }

    private static string ResolveClientType(string? beingType)
    {
        if (!string.IsNullOrEmpty(beingType))
            return beingType;
        var globalType = Config.Instance?.Data?.AIClientType;
        if (!string.IsNullOrEmpty(globalType))
            return globalType;
        return "OllamaClient";
    }

    private IAIClientFactory CreateFactoryByType(string clientType)
    {
        if (clientType.EndsWith("Factory"))
            clientType = clientType.Substring(0, clientType.Length - 7);

        return clientType switch
        {
            "OllamaClient" => new OllamaClientFactory(),
            "DashScopeClient" => new DashScopeClientFactory(),
            "VolcengineArkClient" => new VolcengineArkClientFactory(),
            "HerdsmanClient" => new HerdsmanClientFactory(),
            "LongCatClient" => new LongCatClientFactory(),
            "QiniuAIClient" => new QiniuAIClientFactory(),
            "DeepSeekClient" => new DeepSeekClientFactory(),
            "ZhipuClient" => new ZhipuClientFactory(),
            "MoonshotClient" => new MoonshotClientFactory(),
            "SiliconFlowClient" => new SiliconFlowClientFactory(),
            "MiniMaxClient" => new MiniMaxClientFactory(),
            "ErnieClient" => new ErnieClientFactory(),
            "HunyuanClient" => new HunyuanClientFactory(),
            _ => throw new NotSupportedException($"AI client type '{clientType}' is not supported")
        };
    }
}
