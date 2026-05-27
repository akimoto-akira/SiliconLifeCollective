# Minimal Plugin Demo

A minimal `IPlugin` implementation that demonstrates the plugin lifecycle with hardcoded values.

## IPlugin Interface Overview

Every SiliconLife plugin must implement the `IPlugin` interface defined in `SiliconLife.Collective`:

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Property Summary

| Member | Type | Description |
|--------|------|-------------|
| `Id` | `string` | Unique identifier, must be stable across versions (e.g. `"com.siliconlife.demo.minimal"`) |
| `GetName(Language)` | `string` | Human-readable display name, localized by `Language` enum |
| `Version` | `string` | Semantic version string (e.g. `"1.0.0"`) |
| `GetDescription(Language)` | `string` | Short description of what the plugin does |
| `GetAuthor(Language)` | `string` | Author or organization name |

## Lifecycle Call Order

The host calls lifecycle methods in a strict sequence:

```
OnLoad → OnStart → [running] → OnStop → OnUnload
```

| Method | When Called | Typical Use |
|--------|-----------|-------------|
| `OnLoad()` | Once, when the plugin DLL is loaded into the host | Validate configuration, register types, prepare resources |
| `OnStart()` | When the host has fully started and all plugins are loaded | Interact with other plugins, start background tasks |
| `OnStop()` | When the host is shutting down gracefully | Release resources, flush buffers, save state |
| `OnUnload()` | When the plugin is being unloaded from the host process | Final cleanup |

## This Demo

This plugin returns hardcoded values for all properties and leaves lifecycle methods empty, serving as the simplest possible starting point for plugin development.

## Security Note

Plugins are loaded in an isolated `AssemblyLoadContext` and scanned for forbidden namespace references (e.g. `System.IO`, `System.Net.Http`). See the [Security Documentation](../../docs/en/security.md) for details.
