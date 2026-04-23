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
/// Root host that owns the application lifecycle. Built via
/// <see cref="CoreHostBuilder"/>, it registers services into
/// <see cref="ServiceLocator"/>, starts the <see cref="MainLoop"/>,
/// and manages graceful shutdown.
/// </summary>
public class CoreHost
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<CoreHost>();
    private readonly CoreHostBuilder _builder;
    private CancellationTokenSource _shutdownCts = new();
    private Task? _imManagerTask;

    /// <summary>
    /// Initializes a new <see cref="CoreHost"/> from the given builder configuration.
    /// </summary>
    internal CoreHost(CoreHostBuilder builder)
    {
        _builder = builder;
    }

    /// <summary>Gets the chat system, or <c>null</c> if not configured.</summary>
    public ChatSystem? ChatSystem => _builder.ChatSystem;

    /// <summary>Gets the IM manager, or <c>null</c> if not configured.</summary>
    public IMManager? IMManager => _builder.IMManager;

    /// <summary>Gets the silicon-being manager from <see cref="MainLoop"/>.</summary>
    public SiliconBeingManager? BeingManager => MainLoop.BeingManager;

    /// <summary>Gets the configuration data, or <c>null</c> if not configured.</summary>
    public ConfigDataBase? Config => _builder.Config;

    /// <summary>Gets the storage backend, or <c>null</c> if not configured.</summary>
    public IStorage? Storage => _builder.Storage;

    /// <summary>
    /// Starts the host: registers all configured services into
    /// <see cref="ServiceLocator"/>, configures and starts the
    /// <see cref="MainLoop"/>, and activates the IM manager if present.
    /// </summary>
    public async Task StartAsync()
    {
        _logger.Info(null, "CoreHost starting...");

        if (_builder.ChatSystem != null)
        {
            ServiceLocator.Instance.Register<ChatSystem>(_builder.ChatSystem);
            _logger.Debug(null, "Registered service: {0}", nameof(ChatSystem));
        }
        if (_builder.IMManager != null)
        {
            ServiceLocator.Instance.Register<IMManager>(_builder.IMManager);
            _logger.Debug(null, "Registered service: {0}", nameof(IMManager));
        }
        if (MainLoop.BeingManager != null)
        {
            ServiceLocator.Instance.Register<SiliconBeingManager>(MainLoop.BeingManager);
            _logger.Debug(null, "Registered service: {0}", nameof(SiliconBeingManager));
        }
        if (_builder.AuditLogger != null)
        {
            ServiceLocator.Instance.Register<AuditLogger>(_builder.AuditLogger);
            _logger.Debug(null, "Registered service: {0}", nameof(AuditLogger));
        }
        if (_builder.GlobalAcl != null)
        {
            ServiceLocator.Instance.Register<GlobalACL>(_builder.GlobalAcl);
            _logger.Debug(null, "Registered service: {0}", nameof(GlobalACL));
        }
        if (_builder.BeingFactory != null)
        {
            ServiceLocator.Instance.Register<ISiliconBeingFactory>(_builder.BeingFactory);
            _logger.Debug(null, "Registered service: {0}", nameof(ISiliconBeingFactory));
        }
        if (_builder.DynamicBeingLoader != null)
        {
            ServiceLocator.Instance.Register<DynamicBeingLoader>(_builder.DynamicBeingLoader);
            _logger.Debug(null, "Registered service: {0}", nameof(DynamicBeingLoader));
        }
        if (_builder.IMProvider != null)
        {
            ServiceLocator.Instance.Register(_builder.IMProvider);
            _logger.Debug(null, "Registered service: {0}", _builder.IMProvider.GetType().Name);
        }
        if (_builder.TokenUsageAuditManager != null)
        {
            ServiceLocator.Instance.Register<ITokenUsageAudit>(_builder.TokenUsageAuditManager);
            ServiceLocator.Instance.Register<TokenUsageAuditManager>(_builder.TokenUsageAuditManager);
            _logger.Debug(null, "Registered service: {0}", nameof(TokenUsageAuditManager));
        }
        if (_builder.StreamCancellationManager != null)
        {
            ServiceLocator.Instance.Register<StreamCancellationManager>(_builder.StreamCancellationManager);
            _logger.Debug(null, "Registered service: {0}", nameof(StreamCancellationManager));
        }

        MainLoop.SetConfig(_builder.Config!);
        MainLoop.Start();
        _logger.Info(null, "MainLoop started");

        if (_builder.IMManager != null)
        {
            _imManagerTask = _builder.IMManager.StartAsync();
            _logger.Info(null, "IM manager started");
        }

        _logger.Info(null, "CoreHost started successfully");
        await Task.CompletedTask;
    }

    /// <summary>
    /// Stops the host gracefully: signals cancellation, stops the
    /// <see cref="MainLoop"/>, awaits the IM manager shutdown, and
    /// clears the <see cref="ServiceLocator"/>.
    /// </summary>
    public async Task StopAsync()
    {
        _logger.Info(null, "CoreHost stopping...");

        _shutdownCts.Cancel();

        MainLoop.Stop();
        _logger.Debug(null, "Stopped: MainLoop");

        if (_imManagerTask != null)
        {
            await _imManagerTask;
            _logger.Debug(null, "Stopped: IMManager");
        }

        ServiceLocator.Instance.Clear();
        _logger.Debug(null, "Stopped: ServiceLocator");
        _logger.Info(null, "CoreHost stopped");
    }
}
