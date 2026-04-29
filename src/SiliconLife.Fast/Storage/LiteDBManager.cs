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

using LiteDB;

namespace SiliconLife.Fast;

/// <summary>
/// Static manager for LiteDB connection and collections
/// </summary>
internal static class LiteDBManager
{
    private static LiteDatabase? _db;
    private static bool _initialized;
    private static readonly object _lock = new();
    
    // Collections
    private static ILiteCollection<AppConfig>? _configCollection;
    private static ILiteCollection<StorageRecord>? _storageCollection;
    private static ILiteCollection<TimeRecord>? _timeStorageCollection;
    private static ILiteCollection<WorkNoteRecord>? _workNoteCollection;

    /// <summary>
    /// Gets the LiteDatabase instance
    /// </summary>
    public static LiteDatabase Instance => _db ?? throw new InvalidOperationException("LiteDB not initialized. Call Initialize() first.");

    /// <summary>
    /// Initializes LiteDB with the specified database path
    /// </summary>
    /// <param name="dbPath">Path to the database file</param>
    public static void Initialize(string dbPath)
    {
        if (_initialized) return;

        lock (_lock)
        {
            if (_initialized) return;

            // Ensure directory exists
            string? directory = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create connection string with WAL mode and shared access
            var connectionString = new ConnectionString
            {
                Filename = dbPath,
                Connection = ConnectionType.Shared
            };

            _db = new LiteDatabase(connectionString);

            // Initialize collections
            _configCollection = _db.GetCollection<AppConfig>("app_config");
            _storageCollection = _db.GetCollection<StorageRecord>("storage");
            _timeStorageCollection = _db.GetCollection<TimeRecord>("time_storage");
            _workNoteCollection = _db.GetCollection<WorkNoteRecord>("work_notes");

            // Create indexes for better query performance
            _storageCollection.EnsureIndex(x => x.Key);
            _timeStorageCollection.EnsureIndex(x => x.Key);
            _timeStorageCollection.EnsureIndex(x => x.Timestamp);
            _workNoteCollection.EnsureIndex(x => x.NoteId);
            _workNoteCollection.EnsureIndex(x => x.OwnerType);
            _workNoteCollection.EnsureIndex(x => x.OwnerId);

            _initialized = true;
        }
    }

    /// <summary>
    /// Gets application configuration from LiteDB
    /// </summary>
    public static AppConfig GetConfig()
    {
        EnsureInitialized();
        var config = _configCollection!.FindOne(x => x.ConfigType == "Default");
        return config ?? new AppConfig();
    }

    /// <summary>
    /// Saves application configuration to LiteDB
    /// </summary>
    public static void SaveConfig(AppConfig config)
    {
        EnsureInitialized();
        config.UpdatedAt = DateTime.UtcNow;
        _configCollection!.Upsert(config);
    }

    /// <summary>
    /// Gets a collection by name
    /// </summary>
    public static ILiteCollection<T> GetCollection<T>(string name)
    {
        EnsureInitialized();
        return _db!.GetCollection<T>(name);
    }

    /// <summary>
    /// Gets the storage collection
    /// </summary>
    internal static ILiteCollection<StorageRecord> StorageCollection
    {
        get
        {
            EnsureInitialized();
            return _storageCollection!;
        }
    }

    /// <summary>
    /// Gets the time storage collection
    /// </summary>
    internal static ILiteCollection<TimeRecord> TimeStorageCollection
    {
        get
        {
            EnsureInitialized();
            return _timeStorageCollection!;
        }
    }

    /// <summary>
    /// Gets the work note collection
    /// </summary>
    internal static ILiteCollection<WorkNoteRecord> WorkNoteCollection
    {
        get
        {
            EnsureInitialized();
            return _workNoteCollection!;
        }
    }

    /// <summary>
    /// Shuts down LiteDB and releases resources
    /// </summary>
    public static void Shutdown()
    {
        if (!_initialized) return;

        lock (_lock)
        {
            if (!_initialized) return;

            _db?.Dispose();
            _db = null;
            _initialized = false;
        }
    }

    private static void EnsureInitialized()
    {
        if (!_initialized)
        {
            throw new InvalidOperationException("LiteDB not initialized. Call Initialize() first.");
        }
    }
}
