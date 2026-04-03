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

public class CoreHost
{
    private readonly CoreHostBuilder _builder;
    private CancellationTokenSource _shutdownCts = new();
    private Task? _imManagerTask;

    internal CoreHost(CoreHostBuilder builder)
    {
        _builder = builder;
    }

    public ChatSystem? ChatSystem => _builder.ChatSystem;
    public IMManager? IMManager => _builder.IMManager;
    public SiliconBeingManager? BeingManager => MainLoop.BeingManager;
    public ConfigDataBase? Config => _builder.Config;
    public IStorage? Storage => _builder.Storage;

    public async Task StartAsync()
    {
        if (_builder.ChatSystem != null)
            ServiceLocator.Instance.Register<ChatSystem>(_builder.ChatSystem);
        if (_builder.IMManager != null)
            ServiceLocator.Instance.Register<IMManager>(_builder.IMManager);
        if (MainLoop.BeingManager != null)
            ServiceLocator.Instance.Register<SiliconBeingManager>(MainLoop.BeingManager);
        if (_builder.AuditLogger != null)
            ServiceLocator.Instance.Register<AuditLogger>(_builder.AuditLogger);
        if (_builder.GlobalAcl != null)
            ServiceLocator.Instance.Register<GlobalACL>(_builder.GlobalAcl);
        if (_builder.BeingFactory != null)
            ServiceLocator.Instance.Register<ISiliconBeingFactory>(_builder.BeingFactory);
        if (_builder.DynamicBeingLoader != null)
            ServiceLocator.Instance.Register<DynamicBeingLoader>(_builder.DynamicBeingLoader);

        MainLoop.SetConfig(_builder.Config!);
        MainLoop.Start();

        if (_builder.IMManager != null)
        {
            _imManagerTask = _builder.IMManager.StartAsync();
        }

        await Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        _shutdownCts.Cancel();

        MainLoop.Stop();

        if (_imManagerTask != null)
        {
            await _imManagerTask;
        }

        ServiceLocator.Instance.Clear();
    }
}
