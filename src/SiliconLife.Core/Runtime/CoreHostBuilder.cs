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

public class CoreHostBuilder
{
    public ConfigDataBase? Config { get; private set; }
    public IAIClient? AIClient { get; private set; }
    public IStorage? Storage { get; private set; }
    public ITimeStorage? TimeStorage { get; private set; }
    public ChatSystem? ChatSystem { get; private set; }
    public AuditLogger? AuditLogger { get; private set; }
    public GlobalACL? GlobalAcl { get; private set; }
    public IIMProvider? IMProvider { get; private set; }
    public IMManager? IMManager { get; private set; }
    public ISiliconBeingFactory? BeingFactory { get; private set; }
    public DynamicBeingLoader? DynamicBeingLoader { get; private set; }

    public CoreHostBuilder SetConfig(ConfigDataBase config)
    {
        Config = config;
        return this;
    }

    public CoreHostBuilder SetAIClient(IAIClient client)
    {
        AIClient = client;
        return this;
    }

    public CoreHostBuilder SetStorage(IStorage storage)
    {
        Storage = storage;
        return this;
    }

    public CoreHostBuilder SetTimeStorage(ITimeStorage timeStorage)
    {
        TimeStorage = timeStorage;
        return this;
    }

    public CoreHostBuilder SetChatSystem(ChatSystem chatSystem)
    {
        ChatSystem = chatSystem;
        return this;
    }

    public CoreHostBuilder SetAuditLogger(AuditLogger auditLogger)
    {
        AuditLogger = auditLogger;
        return this;
    }

    public CoreHostBuilder SetGlobalACL(GlobalACL globalAcl)
    {
        GlobalAcl = globalAcl;
        return this;
    }

    public CoreHostBuilder SetIMProvider(IIMProvider provider)
    {
        IMProvider = provider;
        return this;
    }

    public CoreHostBuilder SetIMManager(IMManager imManager)
    {
        IMManager = imManager;
        return this;
    }

    public CoreHostBuilder SetBeingFactory(ISiliconBeingFactory factory)
    {
        BeingFactory = factory;
        return this;
    }

    public CoreHostBuilder SetDynamicBeingLoader(DynamicBeingLoader loader)
    {
        DynamicBeingLoader = loader;
        return this;
    }

    public CoreHost Build()
    {
        return new CoreHost(this);
    }
}
