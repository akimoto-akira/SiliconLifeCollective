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

using System.Reflection;
using System.Runtime.Loader;

namespace SiliconLife.Collective;

public class PluginLoader
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<PluginLoader>();
    private readonly List<LoadedPlugin> _loadedPlugins = [];
    private readonly string _pluginDirectory;

    public PluginLoader(string pluginDirectory)
    {
        _pluginDirectory = pluginDirectory;
    }

    public IReadOnlyList<IPlugin> Plugins => _loadedPlugins.Select(p => p.Plugin).ToList();

    public void LoadAll()
    {
        if (!Directory.Exists(_pluginDirectory))
        {
            _logger.Warn(null, "Plugin directory does not exist: {0}", _pluginDirectory);
            return;
        }

        foreach (string subDir in Directory.GetDirectories(_pluginDirectory))
        {
            LoadPluginFromDirectory(subDir);
        }

        _logger.Info(null, "Loaded {0} plugin(s) from {1}", _loadedPlugins.Count, _pluginDirectory);
    }

    private void LoadPluginFromDirectory(string pluginDir)
    {
        string dirName = Path.GetFileName(pluginDir);
        string? dllPath = Directory.GetFiles(pluginDir, $"{dirName}.dll")
            .Concat(Directory.GetFiles(pluginDir, "*.dll"))
            .FirstOrDefault();

        if (dllPath == null)
        {
            _logger.Warn(null, "No DLL found in plugin directory: {0}", pluginDir);
            return;
        }

        try
        {
            var context = new AssemblyLoadContext(dirName, isCollectible: true);
            Assembly assembly = context.LoadFromAssemblyPath(dllPath);

            Type? pluginType = assembly.GetTypes()
                .FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t) && t != typeof(IPlugin) && !t.IsAbstract);

            if (pluginType == null)
            {
                _logger.Warn(null, "No IPlugin implementation found in {0}", dllPath);
                context.Unload();
                return;
            }

            IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginType)!;
            plugin.OnLoad();

            _loadedPlugins.Add(new LoadedPlugin(plugin, context, dllPath));
            _logger.Info(null, "Plugin loaded: {0} v{1} from {2}", plugin.Id, plugin.Version, dirName);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to load plugin from {0}: {1}", pluginDir, ex.Message);
        }
    }

    public void NotifyAllStarted()
    {
        foreach (var loaded in _loadedPlugins)
        {
            try
            {
                loaded.Plugin.OnStart();
                _logger.Debug(null, "Plugin started: {0}", loaded.Plugin.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(null, "Plugin OnStart failed for {0}: {1}", loaded.Plugin.Id, ex.Message);
            }
        }
    }

    public void NotifyAllStopping()
    {
        foreach (var loaded in _loadedPlugins)
        {
            try
            {
                loaded.Plugin.OnStop();
                _logger.Debug(null, "Plugin stopped: {0}", loaded.Plugin.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(null, "Plugin OnStop failed for {0}: {1}", loaded.Plugin.Id, ex.Message);
            }
        }
    }

    public void UnloadAll()
    {
        foreach (var loaded in _loadedPlugins)
        {
            try
            {
                loaded.Plugin.OnUnload();
                _logger.Debug(null, "Plugin unloaded: {0}", loaded.Plugin.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(null, "Plugin OnUnload failed for {0}: {1}", loaded.Plugin.Id, ex.Message);
            }
        }

        _loadedPlugins.Clear();
        _logger.Info(null, "All plugins unloaded");
    }

    private record LoadedPlugin(IPlugin Plugin, AssemblyLoadContext Context, string DllPath);
}
