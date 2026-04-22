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
    /// Reads log entries from the registered providers that support reading.
    /// Returns an empty list if no providers support reading or if reading is not implemented.
    /// </summary>
    /// <param name="startTime">Optional start time filter</param>
    /// <param name="endTime">Optional end time filter</param>
    /// <param name="beingId">Optional being ID filter. Null means all beings.</param>
    /// <param name="systemOnly">If true, only return system-level logs (BeingId is null)</param>
    /// <param name="levelFilter">Optional log level filter</param>
    /// <param name="maxCount">Maximum number of entries to return. 0 means no limit.</param>
    /// <returns>List of log entries matching the filters, ordered by timestamp descending</returns>
    public List<LogEntry> ReadLogs(
        DateTime? startTime = null,
        DateTime? endTime = null,
        Guid? beingId = null,
        bool systemOnly = false,
        LogLevel? levelFilter = null,
        int maxCount = 0)
    {
        List<ILoggerProvider> providersCopy;

        lock (_lock)
        {
            providersCopy = new List<ILoggerProvider>(_providers);
        }

        var allLogs = new List<LogEntry>();

        foreach (var provider in providersCopy)
        {
            if (provider is ILogReader logReader)
            {
                try
                {
                    var logs = logReader.ReadLogs(startTime, endTime, beingId, systemOnly, levelFilter, maxCount);
                    allLogs.AddRange(logs);
                }
                catch
                {
                    // Provider read failure should not affect other providers
                }
            }
        }

        // Sort by timestamp descending and apply maxCount
        allLogs.Sort((a, b) => b.Timestamp.CompareTo(a.Timestamp));
        
        if (maxCount > 0 && allLogs.Count > maxCount)
        {
            allLogs = allLogs.Take(maxCount).ToList();
        }

        return allLogs;
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

        public void Log(Guid? beingId, LogLevel level, string message)
        {
            if (!IsEnabled(level))
            {
                return;
            }

            LogEntry entry = new LogEntry(beingId, DateTime.UtcNow, level, _category, message);
            _manager.DistributeLog(entry);
        }

        public void Log(Guid? beingId, LogLevel level, string message, Exception exception)
        {
            if (!IsEnabled(level))
            {
                return;
            }

            LogEntry entry = new LogEntry(beingId, DateTime.UtcNow, level, _category, message, exception);
            _manager.DistributeLog(entry);
        }

        public void Log(Guid? beingId, LogLevel level, string format, params object[] args)
        {
            if (!IsEnabled(level))
            {
                return;
            }

            string message = args.Length > 0 ? string.Format(format, args) : format;
            LogEntry entry = new LogEntry(beingId, DateTime.UtcNow, level, _category, message);
            _manager.DistributeLog(entry);
        }

        public void Trace(Guid? beingId, string message) => Log(beingId, LogLevel.Trace, message);

        public void Trace(Guid? beingId, string format, params object[] args) => Log(beingId, LogLevel.Trace, format, args);

        public void Debug(Guid? beingId, string message) => Log(beingId, LogLevel.Debug, message);

        public void Debug(Guid? beingId, string format, params object[] args) => Log(beingId, LogLevel.Debug, format, args);

        public void Info(Guid? beingId, string message) => Log(beingId, LogLevel.Information, message);

        public void Info(Guid? beingId, string format, params object[] args) => Log(beingId, LogLevel.Information, format, args);

        public void Warn(Guid? beingId, string message) => Log(beingId, LogLevel.Warning, message);

        public void Warn(Guid? beingId, string message, Exception exception) => Log(beingId, LogLevel.Warning, message, exception);

        public void Warn(Guid? beingId, string format, params object[] args) => Log(beingId, LogLevel.Warning, format, args);

        public void Error(Guid? beingId, string message) => Log(beingId, LogLevel.Error, message);

        public void Error(Guid? beingId, string message, Exception exception) => Log(beingId, LogLevel.Error, message, exception);

        public void Error(Guid? beingId, string format, params object[] args) => Log(beingId, LogLevel.Error, format, args);

        public void Critical(Guid? beingId, string message) => Log(beingId, LogLevel.Critical, message);

        public void Critical(Guid? beingId, string message, Exception exception) => Log(beingId, LogLevel.Critical, message, exception);

        public void Critical(Guid? beingId, string format, params object[] args) => Log(beingId, LogLevel.Critical, format, args);

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
