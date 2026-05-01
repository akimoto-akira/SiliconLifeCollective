using System.Text.Json;
using SiliconLife.Speedy.Internal;

namespace SiliconLife.Speedy;

/// <summary>
/// The primary entry point for reading and writing data in a .spk pack file.
/// Combines an in-memory <see cref="DirectoryMap"/>, an <see cref="EntryCache"/>,
/// and an asynchronous <see cref="WriteQueue"/> to deliver low-latency reads and
/// non-blocking writes.
/// </summary>
/// <remarks>
/// Thread-safe: all public members may be called concurrently from multiple threads.
/// </remarks>
public sealed class SpeedyPack : IDisposable
{
    // ─── Fields ───────────────────────────────────────────────────────────────

    private readonly string _filePath;
    private readonly SpeedyPackOptions _options;
    private readonly DirectoryMap _directoryMap;
    private readonly EntryCache _entryCache;

    // These are mutable so CompactAsync can reinitialize them after replacing the file.
    private WriteQueue? _writeQueue;       // null when ReadOnly
    private PackFileWriter? _writer;       // null when ReadOnly
    private PackFileReader _reader;

    private bool _disposed;

    // Compact operations must be serialized; reads/writes can proceed concurrently.
    private readonly SemaphoreSlim _compactLock = new(1, 1);

    // ─── Private constructor ──────────────────────────────────────────────────

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

    // ─── 7.1: Factory methods ─────────────────────────────────────────────────

    /// <summary>
    /// Opens an existing .spk file, loading its directory index into memory.
    /// If the file does not exist, a new empty pack is created automatically (AC-1.2).
    /// </summary>
    /// <param name="filePath">Path to the .spk file.</param>
    /// <param name="options">Optional configuration. Defaults are used when null.</param>
    public static SpeedyPack Open(string filePath, SpeedyPackOptions? options = null)
    {
        options ??= new SpeedyPackOptions();

        var directoryMap = new DirectoryMap();
        var entryCache = new EntryCache(options.MaxCacheEntries, options.EntryCacheTtl);

        if (File.Exists(filePath))
        {
            // Open existing file — load directory index
            var reader = PackFileReader.Open(filePath);
            var directory = reader.LoadDirectory();
            directoryMap.LoadFrom(directory);

            if (options.ReadOnly)
            {
                return new SpeedyPack(filePath, options, directoryMap, entryCache,
                    reader, writer: null, writeQueue: null);
            }

            var writer = PackFileWriter.Open(filePath);
            var writeQueue = new WriteQueue(writer, directoryMap);
            return new SpeedyPack(filePath, options, directoryMap, entryCache,
                reader, writer, writeQueue);
        }
        else
        {
            // File does not exist — create a new empty pack (AC-1.2)
            EnsureDirectoryExists(filePath);
            var writer = PackFileWriter.Create(filePath);
            var reader = PackFileReader.Open(filePath);
            var writeQueue = new WriteQueue(writer, directoryMap);
            return new SpeedyPack(filePath, options, directoryMap, entryCache,
                reader, writer, writeQueue);
        }
    }

    /// <summary>
    /// Force-creates a new .spk file, overwriting any existing file (AC-1.3).
    /// </summary>
    /// <param name="filePath">Path to the .spk file.</param>
    /// <param name="options">Optional configuration. Defaults are used when null.</param>
    public static SpeedyPack Create(string filePath, SpeedyPackOptions? options = null)
    {
        options ??= new SpeedyPackOptions();

        EnsureDirectoryExists(filePath);

        var directoryMap = new DirectoryMap();
        var entryCache = new EntryCache(options.MaxCacheEntries, options.EntryCacheTtl);

        // Force-create (overwrites if exists)
        var writer = PackFileWriter.Create(filePath);
        var reader = PackFileReader.Open(filePath);
        var writeQueue = new WriteQueue(writer, directoryMap);

        return new SpeedyPack(filePath, options, directoryMap, entryCache,
            reader, writer, writeQueue);
    }

    // ─── 7.2: Write ───────────────────────────────────────────────────────────

    /// <summary>
    /// Writes raw bytes to <paramref name="path"/>.
    /// The cache is updated synchronously; file persistence is asynchronous (AC-5.1).
    /// </summary>
    public void Write(string path, ReadOnlySpan<byte> data)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);
        var bytes = data.ToArray();

        // AC-4.1: Update cache synchronously so the value is immediately readable
        _entryCache.Set(normalizedPath, bytes);

        // AC-5.1: Enqueue for async persistence
        _writeQueue!.Enqueue(new WriteEntry(normalizedPath, bytes, "raw"));
    }

    /// <summary>
    /// Writes raw bytes to <paramref name="path"/>.
    /// Convenience overload for <c>byte[]</c> to avoid ambiguity with the generic overload.
    /// </summary>
    public void Write(string path, byte[] data) => Write(path, data.AsSpan());

    /// <summary>
    /// Writes bytes to <paramref name="path"/> with an explicit <paramref name="contentType"/>.
    /// Valid content types are "raw", "json", and "text".
    /// The cache is updated synchronously; file persistence is asynchronous.
    /// </summary>
    public void Write(string path, byte[] data, string contentType)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);

        _entryCache.Set(normalizedPath, data);
        _writeQueue!.Enqueue(new WriteEntry(normalizedPath, data, contentType));
    }

    /// <summary>
    /// Serializes <paramref name="value"/> as JSON and writes it to <paramref name="path"/>.
    /// The cache is updated synchronously; file persistence is asynchronous (AC-5.1).
    /// </summary>
    public void Write<T>(string path, T value)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);

        // AC-4.1: Update cache synchronously
        _entryCache.Set(normalizedPath, bytes);

        // AC-5.1: Enqueue for async persistence
        _writeQueue!.Enqueue(new WriteEntry(normalizedPath, bytes, "json"));
    }

    // ─── 7.3: Read ────────────────────────────────────────────────────────────

    /// <summary>
    /// Reads the raw bytes stored at <paramref name="path"/>.
    /// Returns <c>null</c> if the path does not exist.
    /// </summary>
    public byte[]? Read(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);

        // AC-4.2: Check cache first
        if (_entryCache.TryGet(normalizedPath, out var cached))
            return cached;

        // Cache miss — check directory index
        if (!_directoryMap.TryGet(normalizedPath, out var entry))
            return null;

        // Load from file
        var bytes = _reader.ReadAt(entry.Offset, entry.Length);

        // Populate cache for future reads
        _entryCache.Set(normalizedPath, bytes);

        return bytes;
    }

    /// <summary>
    /// Reads and deserializes the JSON value stored at <paramref name="path"/>.
    /// Returns <c>default</c> if the path does not exist.
    /// </summary>
    public T? Read<T>(string path)
    {
        var bytes = Read(path);
        if (bytes is null)
            return default;

        return JsonSerializer.Deserialize<T>(bytes);
    }

    // ─── 7.4: Exists / Delete ─────────────────────────────────────────────────

    /// <summary>
    /// Returns <c>true</c> if an entry exists at <paramref name="path"/> (AC-2.3).
    /// </summary>
    public bool Exists(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);
        return _directoryMap.TryGet(normalizedPath, out _);
    }

    /// <summary>
    /// Deletes the entry at <paramref name="path"/>.
    /// Silent no-op if the path does not exist (AC-2.4).
    /// The cache entry is invalidated synchronously (AC-4.5).
    /// </summary>
    public void Delete(string path)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);

        // AC-4.5: Invalidate cache synchronously
        _entryCache.Invalidate(normalizedPath);

        // Remove from directory map immediately so Exists() returns false right away
        _directoryMap.Remove(normalizedPath);

        // Enqueue async persistence of the delete (rewrites directory region)
        _writeQueue!.Enqueue(new DeleteEntry(normalizedPath));
    }

    // ─── 7.4b: GetMetadata ────────────────────────────────────────────────────

    /// <summary>
    /// Returns metadata for the entry at <paramref name="path"/>, or <c>null</c> if not found.
    /// Returns a tuple of (ContentType, Length, CreatedAt, UpdatedAt).
    /// </summary>
    public (string ContentType, int Length, DateTime CreatedAt, DateTime UpdatedAt)? GetEntryMetadata(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);
        if (!_directoryMap.TryGet(normalizedPath, out var entry))
            return null;

        return (entry.ContentType, entry.Length, entry.CreatedAt, entry.UpdatedAt);
    }

    // ─── 7.4c: GetFileInfo ────────────────────────────────────────────────────

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

    // ─── 7.5: ListEntries / ListDirectories ───────────────────────────────────

    /// <summary>
    /// Returns the normalized paths of all direct child entries under
    /// <paramref name="directoryPath"/> (AC-3.1, AC-3.3, AC-3.4).
    /// No disk I/O — the directory index is always in memory.
    /// </summary>
    public IReadOnlyList<string> ListEntries(string directoryPath = "")
    {
        var normalizedPath = PathNormalizer.Normalize(directoryPath);
        return _directoryMap.ListChildren(normalizedPath);
    }

    /// <summary>
    /// Returns the normalized paths of all direct sub-directories under
    /// <paramref name="directoryPath"/> (AC-3.2, AC-3.4).
    /// No disk I/O — the directory index is always in memory.
    /// </summary>
    public IReadOnlyList<string> ListDirectories(string directoryPath = "")
    {
        var normalizedPath = PathNormalizer.Normalize(directoryPath);
        return _directoryMap.ListDirectories(normalizedPath);
    }

    // ─── Transaction ──────────────────────────────────────────────────────────

    /// <summary>
    /// Begins a new transaction. Operations are buffered until
    /// <see cref="IPackTransaction.Commit"/> is called (AC-6.1).
    /// </summary>
    public IPackTransaction BeginTransaction()
    {
        ThrowIfReadOnly();
        return new SpeedyTransaction(this);
    }

    // ─── 7.6: FlushAsync / CompactAsync ──────────────────────────────────────

    /// <summary>
    /// Waits until all enqueued write operations have been persisted to disk (AC-5.4).
    /// </summary>
    public Task FlushAsync()
    {
        if (_writeQueue is null)
            return Task.CompletedTask;

        return _writeQueue.FlushAsync();
    }

    /// <summary>
    /// Flushes all pending writes, then rewrites the file compacting away free/deleted
    /// space. All entries remain readable before and after (AC-7.4).
    /// </summary>
    public async Task CompactAsync()
    {
        ThrowIfReadOnly();

        await _compactLock.WaitAsync().ConfigureAwait(false);
        try
        {
            // Flush all pending writes first so the file is consistent
            await FlushAsync().ConfigureAwait(false);

            // Snapshot the current live entries
            var snapshot = _directoryMap.Snapshot();

            // Write all live entries to a temp file, then replace the original
            var tempPath = _filePath + ".compact.tmp";
            try
            {
                var newEntries = new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);

                using (var tempWriter = PackFileWriter.Create(tempPath))
                {
                    foreach (var (normalizedPath, oldEntry) in snapshot)
                    {
                        // Read the current bytes (from cache or file)
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

                    // Write the directory region to the temp file
                    tempWriter.WriteDirectory(newEntries);
                    tempWriter.Flush();
                }

                // Dispose the current write queue, reader, and writer before replacing the file
                _writeQueue!.Dispose();
                _reader.Dispose();
                _writer!.Dispose();

                // Replace the original file with the compacted temp file
                File.Move(tempPath, _filePath, overwrite: true);

                // Reload the directory map with new offsets
                _directoryMap.LoadFrom(newEntries);

                // Reinitialize reader, writer, and write queue with the new file
                _reader = PackFileReader.Open(_filePath);
                _writer = PackFileWriter.Open(_filePath);
                _writeQueue = new WriteQueue(_writer, _directoryMap);
            }
            catch
            {
                // Clean up temp file on failure
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

    // ─── 7.7: Dispose ─────────────────────────────────────────────────────────

    /// <summary>
    /// Flushes all pending writes, then closes all file handles (AC-1.5, AC-5.5).
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        // AC-5.5: Drain the write queue before closing
        _writeQueue?.Dispose();
        _reader.Dispose();
        _writer?.Dispose();
        _compactLock.Dispose();
    }

    // ─── Private helpers ──────────────────────────────────────────────────────

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

    // ─── Internal transaction support ────────────────────────────────────────

    /// <summary>
    /// Called by <see cref="SpeedyTransaction.Commit"/> to apply a batch of operations
    /// atomically to the main cache and write queue (AC-6.3).
    /// </summary>
    internal void ApplyTransactionBatch(IEnumerable<WriteOperation> ops)
    {
        // Materialize once so we can iterate twice
        var opList = ops as IReadOnlyList<WriteOperation> ?? ops.ToList();

        // Update cache synchronously first
        foreach (var op in opList)
        {
            switch (op)
            {
                case WriteEntry write:
                    _entryCache.Set(write.NormalizedPath, write.Data);
                    break;
                case DeleteEntry delete:
                    _entryCache.Invalidate(delete.NormalizedPath);
                    _directoryMap.Remove(delete.NormalizedPath);
                    break;
            }
        }

        // Enqueue all operations as a batch for async persistence
        _writeQueue!.EnqueueBatch(opList);
    }
}
