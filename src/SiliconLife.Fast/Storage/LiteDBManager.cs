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

    // ==================== Collection Management ====================

    /// <summary>
    /// Gets all user collection names in the database (excludes system collections)
    /// </summary>
    public static IReadOnlyList<string> GetCollectionNames()
    {
        EnsureInitialized();
        return _db!.GetCollectionNames().OrderBy(n => n).ToList();
    }

    /// <summary>
    /// Checks whether a collection with the specified name exists
    /// </summary>
    public static bool CollectionExists(string name)
    {
        EnsureInitialized();
        if (string.IsNullOrWhiteSpace(name)) return false;
        return _db!.CollectionExists(name);
    }

    /// <summary>
    /// Gets the document count of the specified collection
    /// </summary>
    public static int GetCollectionCount(string name)
    {
        EnsureInitialized();
        if (string.IsNullOrWhiteSpace(name)) return 0;
        if (!_db!.CollectionExists(name)) return 0;
        return _db.GetCollection(name).Count();
    }

    /// <summary>
    /// Creates a new collection if it does not already exist.
    /// LiteDB creates collections lazily; this inserts and removes a probe document
    /// to ensure the collection is materialised immediately.
    /// </summary>
    public static bool CreateCollection(string name)
    {
        EnsureInitialized();
        if (string.IsNullOrWhiteSpace(name)) return false;
        if (_db!.CollectionExists(name)) return false;

        var col = _db.GetCollection(name);
        var probe = new BsonDocument { ["_placeholder"] = true };
        col.Insert(probe);
        col.Delete(probe["_id"]);
        return true;
    }

    /// <summary>
    /// Drops the collection with the specified name
    /// </summary>
    public static bool DropCollection(string name)
    {
        EnsureInitialized();
        if (string.IsNullOrWhiteSpace(name)) return false;
        return _db!.DropCollection(name);
    }

    /// <summary>
    /// Renames a collection
    /// </summary>
    public static bool RenameCollection(string oldName, string newName)
    {
        EnsureInitialized();
        if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName)) return false;
        if (!_db!.CollectionExists(oldName)) return false;
        if (_db.CollectionExists(newName)) return false;
        return _db.RenameCollection(oldName, newName);
    }

    // ==================== Document CRUD ====================

    /// <summary>
    /// Gets documents from the specified collection with pagination support.
    /// Returns raw BsonDocuments so the admin UI can render any schema.
    /// </summary>
    public static IReadOnlyList<BsonDocument> GetDocuments(string collectionName, int skip = 0, int limit = 100)
    {
        EnsureInitialized();
        if (string.IsNullOrWhiteSpace(collectionName)) return Array.Empty<BsonDocument>();
        if (!_db!.CollectionExists(collectionName)) return Array.Empty<BsonDocument>();
        if (skip < 0) skip = 0;
        if (limit <= 0) limit = 100;

        return _db.GetCollection(collectionName)
            .Find(Query.All(), skip, limit)
            .ToList();
    }

    /// <summary>
    /// Gets a single document by its _id from the specified collection
    /// </summary>
    public static BsonDocument? GetDocumentById(string collectionName, BsonValue id)
    {
        EnsureInitialized();
        if (string.IsNullOrWhiteSpace(collectionName) || id == null || id.IsNull) return null;
        if (!_db!.CollectionExists(collectionName)) return null;
        return _db.GetCollection(collectionName).FindById(id);
    }

    /// <summary>
    /// Inserts a document into the specified collection.
    /// If the document has no _id it will be auto-generated.
    /// </summary>
    public static BsonValue InsertDocument(string collectionName, BsonDocument document)
    {
        EnsureInitialized();
        if (string.IsNullOrWhiteSpace(collectionName)) throw new ArgumentException("Collection name is required", nameof(collectionName));
        ArgumentNullException.ThrowIfNull(document);
        return _db!.GetCollection(collectionName).Insert(document);
    }

    /// <summary>
    /// Updates a document in the specified collection (keyed by _id).
    /// Returns true when a matching document was updated.
    /// </summary>
    public static bool UpdateDocument(string collectionName, BsonDocument document)
    {
        EnsureInitialized();
        if (string.IsNullOrWhiteSpace(collectionName)) return false;
        ArgumentNullException.ThrowIfNull(document);
        if (!_db!.CollectionExists(collectionName)) return false;
        if (!document.ContainsKey("_id") || document["_id"].IsNull)
            throw new InvalidOperationException("Document must contain a non-null _id for update");
        return _db.GetCollection(collectionName).Update(document);
    }

    /// <summary>
    /// Upserts (insert or update) a document in the specified collection.
    /// </summary>
    public static bool UpsertDocument(string collectionName, BsonDocument document)
    {
        EnsureInitialized();
        if (string.IsNullOrWhiteSpace(collectionName)) return false;
        ArgumentNullException.ThrowIfNull(document);
        return _db!.GetCollection(collectionName).Upsert(document);
    }

    /// <summary>
    /// Deletes a document from the specified collection by its _id
    /// </summary>
    public static bool DeleteDocument(string collectionName, BsonValue id)
    {
        EnsureInitialized();
        if (string.IsNullOrWhiteSpace(collectionName) || id == null || id.IsNull) return false;
        if (!_db!.CollectionExists(collectionName)) return false;
        return _db.GetCollection(collectionName).Delete(id);
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
