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
/// Fluent builder for constructing a <see cref="CoreHost"/> instance.
/// Call each <c>Set*</c> method to configure components, then invoke
/// <see cref="Build"/> to produce the host.
/// </summary>
public class CoreHostBuilder
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<CoreHostBuilder>();

    /// <summary>Gets the configuration data attached to this builder.</summary>
    public ConfigDataBase? Config { get; private set; }

    /// <summary>Gets the AI client attached to this builder.</summary>
    public IAIClient? AIClient { get; private set; }

    /// <summary>Gets the general-purpose storage attached to this builder.</summary>
    public IStorage? Storage { get; private set; }

    /// <summary>Gets the time-series storage attached to this builder.</summary>
    public ITimeStorage? TimeStorage { get; private set; }

    /// <summary>Gets the chat system attached to this builder.</summary>
    public ChatSystem? ChatSystem { get; private set; }

    /// <summary>Gets the audit logger attached to this builder.</summary>
    public AuditLogger? AuditLogger { get; private set; }

    /// <summary>Gets the global access-control list attached to this builder.</summary>
    public GlobalACL? GlobalAcl { get; private set; }

    /// <summary>Gets the IM provider attached to this builder.</summary>
    public IIMProvider? IMProvider { get; private set; }

    /// <summary>Gets the IM manager attached to this builder.</summary>
    public IMManager? IMManager { get; private set; }

    /// <summary>Gets the silicon-being factory attached to this builder.</summary>
    public ISiliconBeingFactory? BeingFactory { get; private set; }

    /// <summary>Gets the dynamic being loader attached to this builder.</summary>
    public DynamicBeingLoader? DynamicBeingLoader { get; private set; }

    /// <summary>Gets the token usage audit manager attached to this builder.</summary>
    public TokenUsageAuditManager? TokenUsageAuditManager { get; private set; }

    /// <summary>Sets the configuration data.</summary>
    public CoreHostBuilder SetConfig(ConfigDataBase config)
    {
        Config = config;
        _logger.Debug("Set Config");
        return this;
    }

    /// <summary>Sets the AI client used for LLM interactions.</summary>
    public CoreHostBuilder SetAIClient(IAIClient client)
    {
        AIClient = client;
        _logger.Debug("Set AIClient: {0}", client?.GetType().Name ?? "null");
        return this;
    }

    /// <summary>Sets the general-purpose storage backend.</summary>
    public CoreHostBuilder SetStorage(IStorage storage)
    {
        Storage = storage;
        _logger.Debug("Set Storage: {0}", storage?.GetType().Name ?? "null");
        return this;
    }

    /// <summary>Sets the time-series storage backend.</summary>
    public CoreHostBuilder SetTimeStorage(ITimeStorage timeStorage)
    {
        TimeStorage = timeStorage;
        _logger.Debug("Set TimeStorage: {0}", timeStorage?.GetType().Name ?? "null");
        return this;
    }

    /// <summary>Sets the chat system.</summary>
    public CoreHostBuilder SetChatSystem(ChatSystem chatSystem)
    {
        ChatSystem = chatSystem;
        _logger.Debug("Set ChatSystem");
        return this;
    }

    /// <summary>Sets the audit logger.</summary>
    public CoreHostBuilder SetAuditLogger(AuditLogger auditLogger)
    {
        AuditLogger = auditLogger;
        _logger.Debug("Set AuditLogger");
        return this;
    }

    /// <summary>Sets the global access-control list.</summary>
    public CoreHostBuilder SetGlobalACL(GlobalACL globalAcl)
    {
        GlobalAcl = globalAcl;
        _logger.Debug("Set GlobalACL");
        return this;
    }

    /// <summary>Sets the instant-messaging provider.</summary>
    public CoreHostBuilder SetIMProvider(IIMProvider provider)
    {
        IMProvider = provider;
        _logger.Debug("Set IMProvider: {0}", provider?.GetType().Name ?? "null");
        return this;
    }

    /// <summary>Sets the IM manager.</summary>
    public CoreHostBuilder SetIMManager(IMManager imManager)
    {
        IMManager = imManager;
        _logger.Debug("Set IMManager");
        return this;
    }

    /// <summary>Sets the silicon-being factory.</summary>
    public CoreHostBuilder SetBeingFactory(ISiliconBeingFactory factory)
    {
        BeingFactory = factory;
        _logger.Debug("Set BeingFactory: {0}", factory?.GetType().Name ?? "null");
        return this;
    }

    /// <summary>Sets the dynamic being loader.</summary>
    public CoreHostBuilder SetDynamicBeingLoader(DynamicBeingLoader loader)
    {
        DynamicBeingLoader = loader;
        _logger.Debug("Set DynamicBeingLoader");
        return this;
    }

    /// <summary>Sets the token usage audit manager.</summary>
    public CoreHostBuilder SetTokenUsageAuditManager(TokenUsageAuditManager manager)
    {
        TokenUsageAuditManager = manager;
        _logger.Debug("Set TokenUsageAuditManager");
        return this;
    }

    /// <summary>
    /// Builds and returns a new <see cref="CoreHost"/> using the configured components.
    /// </summary>
    public CoreHost Build()
    {
        int componentCount = 0;
        if (Config != null) componentCount++;
        if (AIClient != null) componentCount++;
        if (Storage != null) componentCount++;
        if (TimeStorage != null) componentCount++;
        if (ChatSystem != null) componentCount++;
        if (AuditLogger != null) componentCount++;
        if (GlobalAcl != null) componentCount++;
        if (IMProvider != null) componentCount++;
        if (IMManager != null) componentCount++;
        if (BeingFactory != null) componentCount++;
        if (DynamicBeingLoader != null) componentCount++;
        if (TokenUsageAuditManager != null) componentCount++;

        _logger.Info("Building CoreHost with {0} components", componentCount);
        return new CoreHost(this);
    }
}
