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
/// Read/write entry point for .spk data packages. Internally integrates:
///   · In-memory <see cref="DirectoryMap"/> (directory index)
///   · <see cref="EntryCache"/> (TTL/LRU cache, supports pin)
///   · <see cref="WriteAheadLog"/> (write-ahead log, ensures batch atomicity)
///   · <see cref="PackFileWriter"/> dual-buffer Header commit protocol
/// To achieve low-latency reads, asynchronous writes, and file consistency on both normal and abnormal process exit.
/// </summary>
/// <remarks>
/// Crash consistency is guaranteed by three lines of defense:
///   1. Data writes follow Copy-on-Write: new data and new Directory are written to new locations, old versions
///      remain readable until Header switch;
///   2. Dual Header slots are written alternately and fsynced, ensuring at least one slot is in a consistent state at any time;
///   3. WAL records each batch of pending operations (including data bodies); if the main file's Header switch is not yet complete,
///      this batch can be reapplied to the main file by replaying WAL upon restart.
/// Thread safety: public members can be called concurrently across threads.
/// </remarks>
public sealed class SpeedyPack : IDisposable
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
        MaxDepth = 128
    };

    static SpeedyPack()
    {
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
    private WriteAheadLog? _wal;

    private bool _disposed;
    private readonly SemaphoreSlim _compactLock = new(1, 1);

    private SpeedyPack(
        string filePath,
        SpeedyPackOptions options,
        DirectoryMap directoryMap,
        EntryCache entryCache,
        PackFileReader reader,
        PackFileWriter? writer,
        WriteQueue? writeQueue,
        WriteAheadLog? wal)
    {
        _filePath = filePath;
        _options = options;
        _directoryMap = directoryMap;
        _entryCache = entryCache;
        _reader = reader;
        _writer = writer;
        _writeQueue = writeQueue;
        _wal = wal;
    }

    /// <summary>
    /// Opens an existing .spk file and loads its Directory; creates automatically if the file does not exist.
    /// The following crash recovery actions are performed on open:
    ///   1. If legacy v1 format is detected, migrate to v2 in place first;
    ///   2. Read dual Header slots and select the valid one with the largest Sequence as the active Header;
    ///   3. Scan .wal, replay all "committed but not yet applied" batches and clear WAL.
    /// </summary>
    public static SpeedyPack Open(string filePath, SpeedyPackOptions? options = null)
    {
        options ??= new SpeedyPackOptions();

        var directoryMap = new DirectoryMap();
        var entryCache = new EntryCache(options.MaxCacheEntries, options.EntryCacheTtl);

        if (!File.Exists(filePath))
        {
            EnsureDirectoryExists(filePath);
            var newWriter = PackFileWriter.Create(filePath);
            var newReader = PackFileReader.Open(filePath);
            WriteAheadLog? newWal = null;
            WriteQueue? newQueue = null;
            if (!options.ReadOnly)
            {
                newWal = new WriteAheadLog(filePath + ".wal");
                newWal.Open();
                newWal.Truncate(); // 清理任何残留
                newQueue = new WriteQueue(newWriter, directoryMap, entryCache, newWal);
            }
            return new SpeedyPack(filePath, options, directoryMap, entryCache,
                newReader, options.ReadOnly ? null : newWriter, newQueue, newWal);
        }

        // 1. Detect legacy v1 files and migrate in place.
        UpgradeLegacyFileIfNeeded(filePath);

        // 2. Read active Header + load Directory.
        var reader = PackFileReader.Open(filePath);
        var active = reader.TryReadActiveHeader()
            ?? throw new InvalidDataException(
                $"Both header slots of '{filePath}' are corrupted. " +
                $"Recovery from WAL alone is not supported when both headers are unreadable.");
        var directory = reader.LoadDirectory(active.Header);
        directoryMap.LoadFrom(directory);

        if (options.ReadOnly)
        {
            return new SpeedyPack(filePath, options, directoryMap, entryCache,
                reader, writer: null, writeQueue: null, wal: null);
        }

        // 3. Open writer (rebuild FreeList).
        var writer = PackFileWriter.Open(filePath, directory, active.Header, active.Slot);

        // 4. Open WAL and recover any committed but not yet applied batches.
        var wal = new WriteAheadLog(filePath + ".wal");
        wal.Open();
        var pendingBatches = wal.RecoverCommittedBatches();

        var writeQueue = new WriteQueue(writer, directoryMap, entryCache, wal);

        if (pendingBatches.Count > 0)
        {
            // Hand off pending recovery batches to WriteQueue for normal processing: will re-AppendBatch,
            // write main file, switch Header, Truncate WAL. Replaying already-applied batches is
            // idempotent (will write to new locations and overwrite Directory entries).
            foreach (var batch in pendingBatches)
            {
                if (batch.Count > 0)
                    writeQueue.EnqueueBatch(batch);
            }
            try { writeQueue.FlushAsync().Wait(TimeSpan.FromSeconds(30)); }
            catch { /* 忽略超时 —— 下次启动会继续恢复 */ }
        }
        else
        {
            // No pending recovery batches but WAL file may contain fragments (half-batch with unwritten Commit).
            wal.Truncate();
        }

        return new SpeedyPack(filePath, options, directoryMap, entryCache,
            reader, writer, writeQueue, wal);
    }

    /// <summary>
    /// Force creates a new .spk file (overwrites if already exists), and cleans up the corresponding .wal.
    /// </summary>
    public static SpeedyPack Create(string filePath, SpeedyPackOptions? options = null)
    {
        options ??= new SpeedyPackOptions();

        EnsureDirectoryExists(filePath);

        // Clean up possible WAL residue before overwrite creation.
        var walPath = filePath + ".wal";
        if (File.Exists(walPath))
        {
            try { File.Delete(walPath); } catch { /* ignore */ }
        }

        var directoryMap = new DirectoryMap();
        var entryCache = new EntryCache(options.MaxCacheEntries, options.EntryCacheTtl);

        var writer = PackFileWriter.Create(filePath);
        var reader = PackFileReader.Open(filePath);
        var wal = new WriteAheadLog(walPath);
        wal.Open();
        var writeQueue = new WriteQueue(writer, directoryMap, entryCache, wal);

        return new SpeedyPack(filePath, options, directoryMap, entryCache,
            reader, writer, writeQueue, wal);
    }

    /// <summary>Writes raw byte data. Cache is updated synchronously, disk persistence is asynchronous.</summary>
    public void Write(string path, ReadOnlySpan<byte> data)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);
        var bytes = data.ToArray();

        _entryCache.Set(normalizedPath, bytes, pinned: true);
        _writeQueue!.Enqueue(new WriteEntry(normalizedPath, bytes, "raw"));
    }

    /// <summary>Writes raw byte data (byte[] overload).</summary>
    public void Write(string path, byte[] data) => Write(path, data.AsSpan());

    /// <summary>
    /// Writes bytes with explicit contentType ("raw" / "json" / "text").
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

    /// <summary>Serializes to JSON and then writes.</summary>
    public void Write<T>(string path, T value)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, _jsonOptions);

        _entryCache.Set(normalizedPath, bytes, pinned: true);
        _writeQueue!.Enqueue(new WriteEntry(normalizedPath, bytes, "json"));
    }

    /// <summary>Reads raw bytes; returns null if not exists.</summary>
    public byte[]? Read(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);

        if (_entryCache.TryGet(normalizedPath, out var cached))
            return cached;

        if (!_directoryMap.TryGet(normalizedPath, out var entry))
            return null;

        var bytes = _reader.ReadAt(entry.Offset, entry.Length);
        _entryCache.Set(normalizedPath, bytes, pinned: false);
        return bytes;
    }

    /// <summary>Reads and deserializes JSON value; returns default if not exists.</summary>
    public T? Read<T>(string path)
    {
        var bytes = Read(path);
        if (bytes is null) return default;
        return JsonSerializer.Deserialize<T>(bytes, _jsonOptions);
    }

    /// <summary>Checks if the path exists.</summary>
    public bool Exists(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);
        
        // First check the entry cache (for recently written items that haven't been persisted yet)
        if (_entryCache.TryGet(normalizedPath, out _))
            return true;
            
        // Then check the directory map (for persisted items)
        return _directoryMap.TryGet(normalizedPath, out _);
    }

    /// <summary>Deletes the specified path; cache is invalidated synchronously, disk persistence is asynchronous.</summary>
    public void Delete(string path)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);

        _directoryMap.TryGet(normalizedPath, out var oldEntry);
        _entryCache.Invalidate(normalizedPath);
        _directoryMap.Remove(normalizedPath);
        _writeQueue!.Enqueue(new DeleteEntry(normalizedPath) { OldEntry = oldEntry });
    }

    /// <summary>Returns entry metadata (ContentType, length, timestamp).</summary>
    public (string ContentType, int Length, DateTime CreatedAt, DateTime UpdatedAt)? GetEntryMetadata(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);
        if (!_directoryMap.TryGet(normalizedPath, out var entry))
            return null;

        return (entry.ContentType, entry.Length, entry.CreatedAt, entry.UpdatedAt);
    }

    /// <summary>Returns file header and overall statistics.</summary>
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

    /// <summary>Lists direct child entries (paths) of the specified directory.</summary>
    public IReadOnlyList<string> ListEntries(string directoryPath = "")
    {
        var normalizedPath = PathNormalizer.Normalize(directoryPath);
        return _directoryMap.ListChildren(normalizedPath);
    }

    /// <summary>Lists direct subdirectories of the specified directory.</summary>
    public IReadOnlyList<string> ListDirectories(string directoryPath = "")
    {
        var normalizedPath = PathNormalizer.Normalize(directoryPath);
        return _directoryMap.ListDirectories(normalizedPath);
    }

    /// <summary>Begins a transaction. Operations are buffered until Commit persists them all at once.</summary>
    public IPackTransaction BeginTransaction()
    {
        ThrowIfReadOnly();
        return new SpeedyTransaction(this);
    }

    /// <summary>Waits for all enqueued writes to be persisted.</summary>
    public Task FlushAsync()
    {
        if (_writeQueue is null) return Task.CompletedTask;
        return _writeQueue.FlushAsync();
    }

    /// <summary>
    /// Compacts and rewrites the file, removing FreeList holes. Uses a temporary file during the process, atomically replaces on success.
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
                            bytes = cached;
                        else
                            bytes = _reader.ReadAt(oldEntry.Offset, oldEntry.Length);

                        var newEntry = tempWriter.AppendEntryUpdate(
                            normalizedPath, bytes, oldEntry.ContentType, oldEntry);
                        newEntries[normalizedPath] = newEntry;
                    }

                    tempWriter.WriteDirectoryAndCommit(newEntries);
                    tempWriter.Flush();
                }

                // Close old writer / WAL / reader, preparing for replacement.
                _writeQueue!.Dispose();
                _reader.Dispose();
                _writer!.Dispose();
                _wal?.Dispose();

                File.Move(tempPath, _filePath, overwrite: true);

                // Original WAL also needs to be cleared (its contents become meaningless after compaction).
                var walPath = _filePath + ".wal";
                if (File.Exists(walPath))
                {
                    try { File.Delete(walPath); } catch { /* ignore */ }
                }

                _directoryMap.LoadFrom(newEntries);

                _reader = PackFileReader.Open(_filePath);
                var active = _reader.TryReadActiveHeader()
                    ?? throw new InvalidDataException("Compacted file has no valid header slot.");
                _writer = PackFileWriter.Open(_filePath, newEntries, active.Header, active.Slot);

                _wal = new WriteAheadLog(walPath);
                _wal.Open();
                _writeQueue = new WriteQueue(_writer, _directoryMap, _entryCache, _wal);
            }
            catch
            {
                if (File.Exists(tempPath))
                {
                    try { File.Delete(tempPath); } catch { /* ignore */ }
                }
                throw;
            }
        }
        finally
        {
            _compactLock.Release();
        }
    }

    /// <summary>
    /// Closes all file handles after clearing all pending writes. Even if Flush times out, committed batches in WAL
    /// will be replayed on next Open, so committed data will not be lost.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (_writeQueue != null)
        {
            try
            {
                var flushTask = _writeQueue.FlushAsync();
                flushTask.Wait(TimeSpan.FromSeconds(10));
            }
            catch { /* ignore */ }

            try { _writeQueue.Dispose(); } catch { /* ignore */ }
        }

        try { _reader.Dispose(); } catch { /* ignore */ }
        try { _writer?.Dispose(); } catch { /* ignore */ }
        try { _wal?.Dispose(); } catch { /* ignore */ }

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
    /// For use by <see cref="SpeedyTransaction.Commit"/>: atomically applies a batch of operations
    /// to both the main cache and the write queue.
    /// </summary>
    internal void ApplyTransactionBatch(IEnumerable<WriteOperation> ops)
    {
        var opList = ops as IReadOnlyList<WriteOperation> ?? ops.ToList();

        foreach (var op in opList)
        {
            switch (op)
            {
                case WriteEntry write:
                    _entryCache.Set(write.NormalizedPath, write.Data, pinned: true);
                    break;
                case DeleteEntry delete:
                    if (_directoryMap.TryGet(delete.NormalizedPath, out var existing))
                        delete.OldEntry = existing;
                    _entryCache.Invalidate(delete.NormalizedPath);
                    _directoryMap.Remove(delete.NormalizedPath);
                    break;
            }
        }

        _writeQueue!.EnqueueBatch(opList);
    }

    // ─── Legacy v1 Migration ─────────────────────────────────────────────────────

    /// <summary>
    /// Detects if the file is in v1 legacy format; if so, migrates it to v2 dual Header format.
    /// Migration is done via "temporary file + atomic File.Move", the original file is not damaged during the process.
    /// </summary>
    private static void UpgradeLegacyFileIfNeeded(string filePath)
    {
        // First check the header in read-only mode to avoid competing for exclusive access with subsequent PackFileReader.
        SpkHeader? v1Header;
        Dictionary<string, DirectoryEntry>? legacyDirectory = null;
        using (var probe = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            // If either v2 slot is valid, the file is considered v2, return directly.
            var slotA = SpkHeader.TryReadSlot(probe, 0);
            var slotB = SpkHeader.TryReadSlot(probe, 1);
            if (slotA != null || slotB != null) return;

            // Otherwise try reading v1 legacy header.
            v1Header = SpkHeader.TryReadLegacyV1(probe);
            if (v1Header == null) return; // Not v1 nor valid v2, handled by upper layer

            if (v1Header.DirectoryLength > 0)
            {
                probe.Position = v1Header.DirectoryOffset;
                var buf = new byte[v1Header.DirectoryLength];
                var read = 0;
                while (read < buf.Length)
                {
                    var n = probe.Read(buf, read, buf.Length - read);
                    if (n <= 0) break;
                    read += n;
                }
                legacyDirectory = MessagePackSerializer.Deserialize<Dictionary<string, DirectoryEntry>>(buf)
                                  ?? new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);
            }
            else
            {
                legacyDirectory = new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);
            }
        }

        // Rewrite all entries to a temporary v2 file.
        var tmpPath = filePath + ".v2upgrade.tmp";
        if (File.Exists(tmpPath))
        {
            try { File.Delete(tmpPath); } catch { /* ignore */ }
        }

        using (var legacyReader = PackFileReader.Open(filePath))
        using (var newWriter = PackFileWriter.Create(tmpPath))
        {
            var newEntries = new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);
            foreach (var (path, oldEntry) in legacyDirectory)
            {
                var data = legacyReader.ReadAt(oldEntry.Offset, oldEntry.Length);
                var newEntry = newWriter.AppendEntryUpdate(path, data, oldEntry.ContentType, oldEntry);
                newEntries[path] = newEntry;
            }
            newWriter.WriteDirectoryAndCommit(newEntries);
            newWriter.Flush();
        }

        // Atomically replace original file; keep original file intact on failure.
        File.Move(tmpPath, filePath, overwrite: true);

        // Clean up possible old .wal after migration (v1 has no WAL, but just in case of residue).
        var walPath = filePath + ".wal";
        if (File.Exists(walPath))
        {
            try { File.Delete(walPath); } catch { /* ignore */ }
        }
    }
}
