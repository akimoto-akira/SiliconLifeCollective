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
/// Central log manager that manages logger providers and creates loggers.
/// This class provides a singleton instance for global access while supporting
/// dependency injection scenarios.
/// </summary>
public sealed class LogManager : IDisposable
{
    private static readonly Lazy<LogManager> _instance = new(() => new LogManager());
    private readonly List<ILoggerProvider> _providers = new();
    private readonly Dictionary<string, ILogger> _loggers = new();
    private readonly object _lock = new();

    /// <summary>
    /// Gets the singleton instance of <see cref="LogManager"/>.
    /// </summary>
    public static LogManager Instance => _instance.Value;

    /// <summary>
    /// Gets the global minimum log level from configuration.
    /// This affects all loggers that don't have an explicit minimum level set.
    /// </summary>
    public LogLevel GlobalMinimumLevel
    {
        get
        {
            ConfigDataBase? configData = Config.Instance.Data;
            if (configData != null)
            {
                return configData.MinimumLogLevel;
            }
            return LogLevel.Information;
        }
    }

    /// <summary>
    /// Gets the count of registered providers.
    /// </summary>
    public int ProviderCount
    {
        get
        {
            lock (_lock)
            {
                return _providers.Count;
            }
        }
    }

    private LogManager() { }

    /// <summary>
    /// Adds a logger provider to the manager.
    /// </summary>
    /// <param name="provider">The provider to add.</param>
    /// <returns>This instance for method chaining.</returns>
    public LogManager AddProvider(ILoggerProvider provider)
    {
        if (provider == null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        lock (_lock)
        {
            _providers.Add(provider);
        }

        return this;
    }

    /// <summary>
    /// Removes a logger provider from the manager.
    /// </summary>
    /// <param name="provider">The provider to remove.</param>
    /// <returns>True if the provider was removed; otherwise, false.</returns>
    public bool RemoveProvider(ILoggerProvider provider)
    {
        if (provider == null)
        {
            return false;
        }

        lock (_lock)
        {
            return _providers.Remove(provider);
        }
    }

    /// <summary>
    /// Gets or creates a logger for the specified category.
    /// </summary>
    /// <param name="category">The category name for the logger.</param>
    /// <returns>A logger instance for the specified category.</returns>
    public ILogger GetLogger(string category)
    {
        if (string.IsNullOrEmpty(category))
        {
            throw new ArgumentNullException(nameof(category));
        }

        lock (_lock)
        {
            if (!_loggers.TryGetValue(category, out ILogger? logger))
            {
                logger = new LoggerImpl(category, this);
                _loggers[category] = logger;
            }

            return logger;
        }
    }

    /// <summary>
    /// Gets or creates a logger for the specified type.
    /// The category name will be the fully qualified type name.
    /// </summary>
    /// <typeparam name="T">The type to use as the category.</typeparam>
    /// <returns>A logger instance for the specified type.</returns>
    public ILogger GetLogger<T>()
    {
        return GetLogger(typeof(T).FullName ?? typeof(T).Name);
    }

    /// <summary>
    /// Gets or creates a logger for the specified type.
    /// </summary>
    /// <param name="type">The type to use as the category.</param>
    /// <returns>A logger instance for the specified type.</returns>
    public ILogger GetLogger(Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return GetLogger(type.FullName ?? type.Name);
    }

    /// <summary>
    /// Distributes a log entry to all registered providers.
    /// </summary>
    /// <param name="entry">The log entry to distribute.</param>
    internal void DistributeLog(LogEntry entry)
    {
        List<ILoggerProvider> providersCopy;

        lock (_lock)
        {
            providersCopy = new List<ILoggerProvider>(_providers);
        }

        if (entry.Level < GlobalMinimumLevel)
        {
            return;
        }

        foreach (ILoggerProvider provider in providersCopy)
        {
            if (provider.IsEnabled(entry.Level))
            {
                try
                {
                    provider.WriteLog(entry);
                }
                catch
                {
                    // Provider failure should not affect other providers
                }
            }
        }
    }

    /// <summary>
    /// Flushes all registered providers.
    /// </summary>
    public void Flush()
    {
        List<ILoggerProvider> providersCopy;

        lock (_lock)
        {
            providersCopy = new List<ILoggerProvider>(_providers);
        }

        foreach (ILoggerProvider provider in providersCopy)
        {
            try
            {
                provider.Flush();
            }
            catch
            {
                // Provider failure should not affect other providers
            }
        }
    }

    /// <summary>
    /// Clears all registered providers and cached loggers.
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            foreach (ILoggerProvider provider in _providers)
            {
                try
                {
                    provider.Dispose();
                }
                catch
                {
                    // Ignore disposal errors
                }
            }

            _providers.Clear();
            _loggers.Clear();
        }
    }

    /// <summary>
    /// Releases all resources used by this <see cref="LogManager"/>.
    /// </summary>
    public void Dispose()
    {
        Clear();
    }

    /// <summary>
    /// Internal logger implementation that delegates to LogManager for distribution.
    /// </summary>
    private sealed class LoggerImpl : ILogger
    {
        private readonly string _category;
        private readonly LogManager _manager;
        private LogLevel _minimumLevel;

        public LoggerImpl(string category, LogManager manager)
        {
            _category = category;
            _manager = manager;
            _minimumLevel = LogLevel.Trace;
        }

        public string Category => _category;

        public LogLevel MinimumLevel
        {
            get => _minimumLevel;
            set => _minimumLevel = value;
        }

        public void Log(LogLevel level, string message)
        {
            if (!IsEnabled(level))
            {
                return;
            }

            LogEntry entry = new LogEntry(DateTime.UtcNow, level, _category, message);
            _manager.DistributeLog(entry);
        }

        public void Log(LogLevel level, string message, Exception exception)
        {
            if (!IsEnabled(level))
            {
                return;
            }

            LogEntry entry = new LogEntry(DateTime.UtcNow, level, _category, message, exception);
            _manager.DistributeLog(entry);
        }

        public void Log(LogLevel level, string format, params object[] args)
        {
            if (!IsEnabled(level))
            {
                return;
            }

            string message = args.Length > 0 ? string.Format(format, args) : format;
            LogEntry entry = new LogEntry(DateTime.UtcNow, level, _category, message);
            _manager.DistributeLog(entry);
        }

        public void Trace(string message) => Log(LogLevel.Trace, message);

        public void Trace(string format, params object[] args) => Log(LogLevel.Trace, format, args);

        public void Debug(string message) => Log(LogLevel.Debug, message);

        public void Debug(string format, params object[] args) => Log(LogLevel.Debug, format, args);

        public void Info(string message) => Log(LogLevel.Information, message);

        public void Info(string format, params object[] args) => Log(LogLevel.Information, format, args);

        public void Warn(string message) => Log(LogLevel.Warning, message);

        public void Warn(string message, Exception exception) => Log(LogLevel.Warning, message, exception);

        public void Warn(string format, params object[] args) => Log(LogLevel.Warning, format, args);

        public void Error(string message) => Log(LogLevel.Error, message);

        public void Error(string message, Exception exception) => Log(LogLevel.Error, message, exception);

        public void Error(string format, params object[] args) => Log(LogLevel.Error, format, args);

        public void Critical(string message) => Log(LogLevel.Critical, message);

        public void Critical(string message, Exception exception) => Log(LogLevel.Critical, message, exception);

        public void Critical(string format, params object[] args) => Log(LogLevel.Critical, format, args);

        public bool IsEnabled(LogLevel level)
        {
            if (level < _minimumLevel)
            {
                return false;
            }

            return level >= _manager.GlobalMinimumLevel;
        }
    }
}
