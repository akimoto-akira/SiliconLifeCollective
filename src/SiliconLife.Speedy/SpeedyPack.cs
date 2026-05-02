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

using System.Text.Json;
using MessagePack;
using SiliconLife.Speedy.Internal;

namespace SiliconLife.Speedy;

/// <summary>
/// The primary entry point for reading and writing data in a .spk pack file.
/// Combines an in-memory DirectoryMap, an EntryCache,
/// and an asynchronous WriteQueue to deliver low-latency reads and
/// non-blocking writes.
/// </summary>
/// <remarks>
/// Thread-safe: all public members may be called concurrently from multiple threads.
/// </remarks>
public sealed class SpeedyPack : IDisposable
{
    // Shared JSON options used for serialization and deserialization.
    // PropertyNameCaseInsensitive is required so that readonly structs with
    // parameterized constructors (e.g. IncompleteDate) can be deserialized
    // correctly — constructor parameters are camelCase while JSON property
    // names are PascalCase.
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // Static constructor to configure MessagePack serializers
    static SpeedyPack()
    {
        // Configure MessagePack to handle DateTime properly
        var resolver = MessagePack.Resolvers.CompositeResolver.Create(
            MessagePack.Resolvers.BuiltinResolver.Instance,
            MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        
        var options = MessagePackSerializerOptions.Standard
            .WithResolver(resolver)
            .WithCompression(MessagePackCompression.Lz4Block);
        
        MessagePackSerializer.DefaultOptions = options;
    }

    private readonly string _filePath;
    private readonly SpeedyPackOptions _options;
    private readonly DirectoryMap _directoryMap;
    private readonly EntryCache _entryCache;

    private WriteQueue? _writeQueue;
    private PackFileWriter? _writer;
    private PackFileReader _reader;

    private bool _disposed;
    private readonly SemaphoreSlim _compactLock = new(1, 1);

    private SpeedyPack(
        string filePath,
        SpeedyPackOptions options,
        DirectoryMap directoryMap,
        EntryCache entryCache,
        PackFileReader reader,
        PackFileWriter? writer,
        WriteQueue? writeQueue)
    {
        _filePath = filePath;
        _options = options;
        _directoryMap = directoryMap;
        _entryCache = entryCache;
        _reader = reader;
        _writer = writer;
        _writeQueue = writeQueue;
    }

    /// <summary>
    /// Opens an existing .spk file, loading its directory index into memory.
    /// If the file does not exist, a new empty pack is created automatically.
    /// </summary>
    public static SpeedyPack Open(string filePath, SpeedyPackOptions? options = null)
    {
        options ??= new SpeedyPackOptions();

        var directoryMap = new DirectoryMap();
        var entryCache = new EntryCache(options.MaxCacheEntries, options.EntryCacheTtl);

        if (File.Exists(filePath))
        {
            var reader = PackFileReader.Open(filePath);
            var directory = reader.LoadDirectory();
            directoryMap.LoadFrom(directory);

            if (options.ReadOnly)
            {
                return new SpeedyPack(filePath, options, directoryMap, entryCache,
                    reader, writer: null, writeQueue: null);
            }

            // 传入 directory 使 PackFileWriter 能正确重建 FreeList。
            var writer = PackFileWriter.Open(filePath, directory);
            var writeQueue = new WriteQueue(writer, directoryMap, entryCache);
            return new SpeedyPack(filePath, options, directoryMap, entryCache,
                reader, writer, writeQueue);
        }
        else
        {
            EnsureDirectoryExists(filePath);
            var writer = PackFileWriter.Create(filePath);
            var reader = PackFileReader.Open(filePath);
            var writeQueue = new WriteQueue(writer, directoryMap, entryCache);
            return new SpeedyPack(filePath, options, directoryMap, entryCache,
                reader, writer, writeQueue);
        }
    }

    /// <summary>
    /// Force-creates a new .spk file, overwriting any existing file.
    /// </summary>
    public static SpeedyPack Create(string filePath, SpeedyPackOptions? options = null)
    {
        options ??= new SpeedyPackOptions();

        EnsureDirectoryExists(filePath);

        var directoryMap = new DirectoryMap();
        var entryCache = new EntryCache(options.MaxCacheEntries, options.EntryCacheTtl);

        var writer = PackFileWriter.Create(filePath);
        var reader = PackFileReader.Open(filePath);
        var writeQueue = new WriteQueue(writer, directoryMap, entryCache);

        return new SpeedyPack(filePath, options, directoryMap, entryCache,
            reader, writer, writeQueue);
    }

    /// <summary>
    /// Writes raw bytes to path.
    /// The cache is updated synchronously; file persistence is asynchronous.
    /// </summary>
    public void Write(string path, ReadOnlySpan<byte> data)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);
        var bytes = data.ToArray();

        // Pin the cache entry so it survives LRU/TTL eviction until the
        // WriteQueue has persisted it to disk and updated the DirectoryMap.
        _entryCache.Set(normalizedPath, bytes, pinned: true);
        _writeQueue!.Enqueue(new WriteEntry(normalizedPath, bytes, "raw"));
    }

    /// <summary>
    /// Writes raw bytes to path. Convenience overload for byte[].
    /// </summary>
    public void Write(string path, byte[] data) => Write(path, data.AsSpan());

    /// <summary>
    /// Writes bytes to path with an explicit contentType.
    /// Valid content types are "raw", "json", and "text".
    /// </summary>
    public void Write(string path, byte[] data, string contentType)
    {
        ThrowIfReadOnly();
        if (data is null) throw new ArgumentNullException(nameof(data));
        if (string.IsNullOrEmpty(contentType)) contentType = "raw";
        var normalizedPath = PathNormalizer.Normalize(path);

        _entryCache.Set(normalizedPath, data, pinned: true);
        _writeQueue!.Enqueue(new WriteEntry(normalizedPath, data, contentType));
    }

    /// <summary>
    /// Serializes value as JSON and writes it to path.
    /// The cache is updated synchronously; file persistence is asynchronous.
    /// </summary>
    public void Write<T>(string path, T value)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, _jsonOptions);

        _entryCache.Set(normalizedPath, bytes, pinned: true);
        _writeQueue!.Enqueue(new WriteEntry(normalizedPath, bytes, "json"));
    }

    /// <summary>
    /// Reads the raw bytes stored at path.
    /// Returns null if the path does not exist.
    /// </summary>
    public byte[]? Read(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);

        // Cache (includes pinned, not-yet-persisted writes) wins first.
        if (_entryCache.TryGet(normalizedPath, out var cached))
            return cached;

        if (!_directoryMap.TryGet(normalizedPath, out var entry))
            return null;

        var bytes = _reader.ReadAt(entry.Offset, entry.Length);
        // Populate cache as a regular (non-pinned) entry subject to TTL/LRU.
        _entryCache.Set(normalizedPath, bytes, pinned: false);

        return bytes;
    }

    /// <summary>
    /// Reads and deserializes the JSON value stored at path.
    /// Returns default if the path does not exist.
    /// </summary>
    public T? Read<T>(string path)
    {
        var bytes = Read(path);
        if (bytes is null)
            return default;

        return JsonSerializer.Deserialize<T>(bytes, _jsonOptions);
    }

    /// <summary>
    /// Returns true if an entry exists at path.
    /// </summary>
    public bool Exists(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);
        return _directoryMap.TryGet(normalizedPath, out _);
    }

    /// <summary>
    /// Deletes the entry at path.
    /// Silent no-op if the path does not exist.
    /// The cache entry is invalidated synchronously.
    /// </summary>
    public void Delete(string path)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);

        // 同步捕获旧条目，以便 WriteQueue 后续归还其占用的空间给 FreeList；
        // 随后再同步移除 DirectoryMap / Invalidate 缓存保证读可见性。
        _directoryMap.TryGet(normalizedPath, out var oldEntry);

        _entryCache.Invalidate(normalizedPath);
        _directoryMap.Remove(normalizedPath);
        _writeQueue!.Enqueue(new DeleteEntry(normalizedPath) { OldEntry = oldEntry });
    }

    /// <summary>
    /// Returns metadata for the entry at path, or null if not found.
    /// Returns a tuple of (ContentType, Length, CreatedAt, UpdatedAt).
    /// </summary>
    public (string ContentType, int Length, DateTime CreatedAt, DateTime UpdatedAt)? GetEntryMetadata(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);
        if (!_directoryMap.TryGet(normalizedPath, out var entry))
            return null;

        return (entry.ContentType, entry.Length, entry.CreatedAt, entry.UpdatedAt);
    }

    /// <summary>
    /// Returns header and statistics information about the current .spk file.
    /// </summary>
    public SpkFileInfo GetFileInfo()
    {
        var header = _reader.ReadHeader();
        var snapshot = _directoryMap.Snapshot();

        int jsonEntries = 0, rawEntries = 0, textEntries = 0;
        foreach (var entry in snapshot.Values)
        {
            switch (entry.ContentType)
            {
                case "json": jsonEntries++; break;
                case "raw": rawEntries++; break;
                case "text": textEntries++; break;
            }
        }

        var fileSize = new FileInfo(_filePath).Length;

        return new SpkFileInfo(
            FilePath: _filePath,
            FileSize: fileSize,
            Magic: System.Text.Encoding.ASCII.GetString(header.Magic),
            Version: header.Version,
            Flags: header.Flags,
            DirectoryOffset: header.DirectoryOffset,
            DirectoryLength: header.DirectoryLength,
            TotalEntries: snapshot.Count,
            JsonEntries: jsonEntries,
            RawEntries: rawEntries,
            TextEntries: textEntries);
    }

    /// <summary>
    /// Returns the normalized paths of all direct child entries under directoryPath.
    /// No disk I/O — the directory index is always in memory.
    /// </summary>
    public IReadOnlyList<string> ListEntries(string directoryPath = "")
    {
        var normalizedPath = PathNormalizer.Normalize(directoryPath);
        return _directoryMap.ListChildren(normalizedPath);
    }

    /// <summary>
    /// Returns the normalized paths of all direct sub-directories under directoryPath.
    /// No disk I/O — the directory index is always in memory.
    /// </summary>
    public IReadOnlyList<string> ListDirectories(string directoryPath = "")
    {
        var normalizedPath = PathNormalizer.Normalize(directoryPath);
        return _directoryMap.ListDirectories(normalizedPath);
    }

    /// <summary>
    /// Begins a new transaction. Operations are buffered until
    /// IPackTransaction.Commit is called.
    /// </summary>
    public IPackTransaction BeginTransaction()
    {
        ThrowIfReadOnly();
        return new SpeedyTransaction(this);
    }

    /// <summary>
    /// Waits until all enqueued write operations have been persisted to disk.
    /// </summary>
    public Task FlushAsync()
    {
        if (_writeQueue is null)
            return Task.CompletedTask;

        return _writeQueue.FlushAsync();
    }

    /// <summary>
    /// Flushes all pending writes, then rewrites the file compacting away free/deleted
    /// space. All entries remain readable before and after.
    /// </summary>
    public async Task CompactAsync()
    {
        ThrowIfReadOnly();

        await _compactLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await FlushAsync().ConfigureAwait(false);

            var snapshot = _directoryMap.Snapshot();

            var tempPath = _filePath + ".compact.tmp";
            try
            {
                var newEntries = new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);

                using (var tempWriter = PackFileWriter.Create(tempPath))
                {
                    foreach (var (normalizedPath, oldEntry) in snapshot)
                    {
                        byte[] bytes;
                        if (_entryCache.TryGet(normalizedPath, out var cached))
                        {
                            bytes = cached;
                        }
                        else
                        {
                            bytes = _reader.ReadAt(oldEntry.Offset, oldEntry.Length);
                        }

                        var newEntry = tempWriter.AppendEntryUpdate(
                            normalizedPath, bytes, oldEntry.ContentType, oldEntry);
                        newEntries[normalizedPath] = newEntry;
                    }

                    tempWriter.WriteDirectory(newEntries);
                    tempWriter.Flush();
                }

                _writeQueue!.Dispose();
                _reader.Dispose();
                _writer!.Dispose();

                File.Move(tempPath, _filePath, overwrite: true);

                _directoryMap.LoadFrom(newEntries);

                _reader = PackFileReader.Open(_filePath);
                // Compact 后新文件中只有 live 条目，FreeList 不会有旧碎片，
                // 仍然需要传入 directory 以正确构造头部/目录区域的占用记录。
                _writer = PackFileWriter.Open(_filePath, newEntries);
                _writeQueue = new WriteQueue(_writer, _directoryMap, _entryCache);
            }
            catch
            {
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
                throw;
            }
        }
        finally
        {
            _compactLock.Release();
        }
    }

    /// <summary>
    /// Flushes all pending writes, then closes all file handles.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        // Drain pending writes first so the .spk file on disk is consistent.
        if (_writeQueue != null)
        {
            try
            {
                var flushTask = _writeQueue.FlushAsync();
                flushTask.Wait(TimeSpan.FromSeconds(10));
                if (!flushTask.IsCompletedSuccessfully)
                {
                    // If flush didn't complete, try to force process remaining operations
                    try { _writeQueue.Dispose(); } catch { /* ignore */ }
                }
            }
            catch
            {
                // Ignore exceptions during flush
            }
        }

        _reader.Dispose();
        _writer?.Dispose();
        _compactLock.Dispose();
    }

    private static void EnsureDirectoryExists(string filePath)
    {
        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
    }

    private void ThrowIfReadOnly()
    {
        if (_options.ReadOnly)
            throw new InvalidOperationException("This SpeedyPack instance is read-only.");
    }

    /// <summary>
    /// Called by SpeedyTransaction.Commit to apply a batch of operations
    /// atomically to the main cache and write queue.
    /// </summary>
    internal void ApplyTransactionBatch(IEnumerable<WriteOperation> ops)
    {
        var opList = ops as IReadOnlyList<WriteOperation> ?? ops.ToList();

        foreach (var op in opList)
        {
            switch (op)
            {
                case WriteEntry write:
                    // Pin until the WriteQueue persists the batch.
                    _entryCache.Set(write.NormalizedPath, write.Data, pinned: true);
                    break;
                case DeleteEntry delete:
                    // 捕获旧条目传给 WriteQueue，以便后续归还其 FreeList 空间。
                    if (_directoryMap.TryGet(delete.NormalizedPath, out var existing))
                        delete.OldEntry = existing;
                    _entryCache.Invalidate(delete.NormalizedPath);
                    _directoryMap.Remove(delete.NormalizedPath);
                    break;
            }
        }

        _writeQueue!.EnqueueBatch(opList);
    }
}
