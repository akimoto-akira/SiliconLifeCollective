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

using System.Text;

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// 预写日志 (Write-Ahead Log) 管理器。
/// 在批次写入主 .spk 文件之前，先把操作以"记录 + CRC"的形式追加写入 .wal
/// 文件，并 fsync 到磁盘。只有当 .spk 文件的新 Header 槽位也成功落盘之后，
/// WAL 才会被截断。这样无论在哪一步崩溃，重启时都能通过重放 WAL 将主文件
/// 恢复到最近一次已提交批次之后的一致状态。
/// </summary>
/// <remarks>
/// 记录格式（小端字节序）：
///   [0..4)  Magic "WALR"            (uint32)
///   [4..5)  RecordVersion = 1       (byte)
///   [5..6)  Type: 1=Write,2=Delete,0xFF=Commit (byte)
///   [6..14) BatchId                 (int64)
///   Payload (depends on Type):
///     Write:  PathLen(uint16) + Path(UTF-8) + CtLen(byte) + CT(ASCII)
///             + DataLen(int32) + Data
///     Delete: PathLen(uint16) + Path(UTF-8)
///     Commit: (空)
///   [tail..tail+4) CRC32 covering all preceding bytes of this record.
///
/// 只有紧跟着 Commit 记录的那一批 Write/Delete 才算已提交；其余视作未完成，
/// 重启时将被跳过。
/// </remarks>
internal sealed class WriteAheadLog : IDisposable
{
    private const uint WalMagic = 0x524C4157u; // 'W''A''L''R' little-endian
    private const byte RecordVersion = 1;
    private const byte TypeWrite = 1;
    private const byte TypeDelete = 2;
    private const byte TypeCommit = 0xFF;

    private const int MaxSaneDataLength = 256 * 1024 * 1024; // 256MB 单条上限，防止坏数据导致天量分配

    private readonly string _filePath;
    private FileStream? _stream;
    private readonly object _lock = new();
    private long _nextBatchId = 1;
    private bool _disposed;

    public string FilePath => _filePath;

    public WriteAheadLog(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>打开（或创建）WAL 文件。</summary>
    public void Open()
    {
        lock (_lock)
        {
            if (_stream != null) return;
            _stream = new FileStream(
                _filePath,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.Read);
        }
    }

    /// <summary>
    /// 将一个批次的 Write/Delete 操作以及 Commit 标记原子性地追加到 WAL，
    /// 并强制刷盘。返回该批次的 BatchId。
    /// </summary>
    public long AppendBatch(IReadOnlyList<WriteOperation> ops)
    {
        lock (_lock)
        {
            if (_stream == null) throw new InvalidOperationException("WAL not opened.");
            if (_disposed) throw new ObjectDisposedException(nameof(WriteAheadLog));

            var batchId = _nextBatchId++;
            _stream.Position = _stream.Length;

            foreach (var op in ops)
            {
                switch (op)
                {
                    case WriteEntry we:
                        WriteWriteRecord(_stream, batchId, we);
                        break;
                    case DeleteEntry de:
                        WriteDeleteRecord(_stream, batchId, de);
                        break;
                }
            }

            WriteCommitRecord(_stream, batchId);
            _stream.Flush(flushToDisk: true);
            return batchId;
        }
    }

    /// <summary>
    /// 清空 WAL。应当在主文件的新 Header 槽位已 fsync 之后调用，意味着此前
    /// 的所有已提交批次都已真正生效。
    /// </summary>
    public void Truncate()
    {
        lock (_lock)
        {
            if (_stream == null) return;
            _stream.SetLength(0);
            _stream.Flush(flushToDisk: true);
            _nextBatchId = 1;
        }
    }

    /// <summary>
    /// 扫描 WAL，返回所有"已提交且完整"的批次（按 BatchId 升序）。
    /// 任何不完整 / CRC 失败 / 未跟随 Commit 的记录都会被静默丢弃。
    /// </summary>
    public IReadOnlyList<IReadOnlyList<WriteOperation>> RecoverCommittedBatches()
    {
        lock (_lock)
        {
            if (_stream == null) return Array.Empty<IReadOnlyList<WriteOperation>>();

            _stream.Position = 0;

            var pendingByBatch = new Dictionary<long, List<WriteOperation>>();
            var committed = new List<(long BatchId, List<WriteOperation> Ops)>();

            while (_stream.Position < _stream.Length)
            {
                var record = TryReadRecord(_stream);
                if (record == null)
                    break; // 遇到损坏 / 截断即停止，剩余视为未提交

                var (type, batchId, op) = record.Value;

                if (type == TypeCommit)
                {
                    if (pendingByBatch.TryGetValue(batchId, out var ops))
                    {
                        committed.Add((batchId, ops));
                        pendingByBatch.Remove(batchId);
                    }
                    else
                    {
                        // 允许空批次（没有 write/delete 只有 commit）— 不常见但视为已提交的空操作
                        committed.Add((batchId, new List<WriteOperation>()));
                    }
                }
                else if (op != null)
                {
                    if (!pendingByBatch.TryGetValue(batchId, out var ops))
                    {
                        ops = new List<WriteOperation>();
                        pendingByBatch[batchId] = ops;
                    }
                    ops.Add(op);
                }
            }

            committed.Sort((a, b) => a.BatchId.CompareTo(b.BatchId));
            var maxBatchId = committed.Count > 0 ? committed[^1].BatchId : 0;
            _nextBatchId = maxBatchId + 1;

            var result = new List<IReadOnlyList<WriteOperation>>(committed.Count);
            foreach (var (_, ops) in committed)
                result.Add(ops);
            return result;
        }
    }

    // ─── 记录写入 ─────────────────────────────────────────────────────────────

    private static void WriteWriteRecord(Stream stream, long batchId, WriteEntry we)
    {
        var pathBytes = Encoding.UTF8.GetBytes(we.NormalizedPath);
        var ct = string.IsNullOrEmpty(we.ContentType) ? "raw" : we.ContentType;
        var ctBytes = Encoding.ASCII.GetBytes(ct);
        if (ctBytes.Length > 255)
            throw new InvalidOperationException("ContentType too long for WAL record.");
        if (pathBytes.Length > ushort.MaxValue)
            throw new InvalidOperationException("Path too long for WAL record.");

        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        bw.Write(WalMagic);
        bw.Write(RecordVersion);
        bw.Write(TypeWrite);
        bw.Write(batchId);
        bw.Write((ushort)pathBytes.Length);
        bw.Write(pathBytes);
        bw.Write((byte)ctBytes.Length);
        bw.Write(ctBytes);
        bw.Write(we.Data.Length);
        bw.Write(we.Data);

        var body = ms.ToArray();
        var crc = Crc32.Compute(body);
        stream.Write(body, 0, body.Length);
        var crcBytes = BitConverter.GetBytes(crc);
        stream.Write(crcBytes, 0, 4);
    }

    private static void WriteDeleteRecord(Stream stream, long batchId, DeleteEntry de)
    {
        var pathBytes = Encoding.UTF8.GetBytes(de.NormalizedPath);
        if (pathBytes.Length > ushort.MaxValue)
            throw new InvalidOperationException("Path too long for WAL record.");

        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        bw.Write(WalMagic);
        bw.Write(RecordVersion);
        bw.Write(TypeDelete);
        bw.Write(batchId);
        bw.Write((ushort)pathBytes.Length);
        bw.Write(pathBytes);

        var body = ms.ToArray();
        var crc = Crc32.Compute(body);
        stream.Write(body, 0, body.Length);
        var crcBytes = BitConverter.GetBytes(crc);
        stream.Write(crcBytes, 0, 4);
    }

    private static void WriteCommitRecord(Stream stream, long batchId)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        bw.Write(WalMagic);
        bw.Write(RecordVersion);
        bw.Write(TypeCommit);
        bw.Write(batchId);

        var body = ms.ToArray();
        var crc = Crc32.Compute(body);
        stream.Write(body, 0, body.Length);
        var crcBytes = BitConverter.GetBytes(crc);
        stream.Write(crcBytes, 0, 4);
    }

    // ─── 记录读取 ─────────────────────────────────────────────────────────────

    /// <summary>尝试读取下一条记录。失败时返回 null 并回退 stream 位置。</summary>
    private static (byte Type, long BatchId, WriteOperation? Op)? TryReadRecord(Stream stream)
    {
        var startPos = stream.Position;
        try
        {
            var headerBuf = new byte[14];
            if (!TryReadFully(stream, headerBuf, 14)) { stream.Position = startPos; return null; }

            var magic = BitConverter.ToUInt32(headerBuf, 0);
            if (magic != WalMagic) { stream.Position = startPos; return null; }
            var ver = headerBuf[4];
            if (ver != RecordVersion) { stream.Position = startPos; return null; }
            var type = headerBuf[5];
            var batchId = BitConverter.ToInt64(headerBuf, 6);

            byte[] payload;
            WriteOperation? op = null;

            if (type == TypeWrite)
            {
                var pathLenBuf = new byte[2];
                if (!TryReadFully(stream, pathLenBuf, 2)) { stream.Position = startPos; return null; }
                var pathLen = BitConverter.ToUInt16(pathLenBuf, 0);
                var pathBuf = new byte[pathLen];
                if (pathLen > 0 && !TryReadFully(stream, pathBuf, pathLen)) { stream.Position = startPos; return null; }

                var ctLenInt = stream.ReadByte();
                if (ctLenInt < 0) { stream.Position = startPos; return null; }
                var ctLen = (byte)ctLenInt;
                var ctBuf = new byte[ctLen];
                if (ctLen > 0 && !TryReadFully(stream, ctBuf, ctLen)) { stream.Position = startPos; return null; }

                var dataLenBuf = new byte[4];
                if (!TryReadFully(stream, dataLenBuf, 4)) { stream.Position = startPos; return null; }
                var dataLen = BitConverter.ToInt32(dataLenBuf, 0);
                if (dataLen < 0 || dataLen > MaxSaneDataLength) { stream.Position = startPos; return null; }

                var dataBuf = new byte[dataLen];
                if (dataLen > 0 && !TryReadFully(stream, dataBuf, dataLen)) { stream.Position = startPos; return null; }

                using var ms = new MemoryStream();
                ms.Write(headerBuf, 0, 14);
                ms.Write(pathLenBuf, 0, 2);
                ms.Write(pathBuf, 0, pathLen);
                ms.WriteByte(ctLen);
                ms.Write(ctBuf, 0, ctLen);
                ms.Write(dataLenBuf, 0, 4);
                ms.Write(dataBuf, 0, dataLen);
                payload = ms.ToArray();

                var path = Encoding.UTF8.GetString(pathBuf);
                var ct = Encoding.ASCII.GetString(ctBuf);
                op = new WriteEntry(path, dataBuf, ct);
            }
            else if (type == TypeDelete)
            {
                var pathLenBuf = new byte[2];
                if (!TryReadFully(stream, pathLenBuf, 2)) { stream.Position = startPos; return null; }
                var pathLen = BitConverter.ToUInt16(pathLenBuf, 0);
                var pathBuf = new byte[pathLen];
                if (pathLen > 0 && !TryReadFully(stream, pathBuf, pathLen)) { stream.Position = startPos; return null; }

                using var ms = new MemoryStream();
                ms.Write(headerBuf, 0, 14);
                ms.Write(pathLenBuf, 0, 2);
                ms.Write(pathBuf, 0, pathLen);
                payload = ms.ToArray();

                op = new DeleteEntry(Encoding.UTF8.GetString(pathBuf));
            }
            else if (type == TypeCommit)
            {
                payload = new byte[14];
                Array.Copy(headerBuf, payload, 14);
            }
            else
            {
                stream.Position = startPos;
                return null;
            }

            var crcBuf = new byte[4];
            if (!TryReadFully(stream, crcBuf, 4)) { stream.Position = startPos; return null; }
            var storedCrc = BitConverter.ToUInt32(crcBuf, 0);
            var computedCrc = Crc32.Compute(payload);
            if (storedCrc != computedCrc) { stream.Position = startPos; return null; }

            return (type, batchId, op);
        }
        catch
        {
            stream.Position = startPos;
            return null;
        }
    }

    private static bool TryReadFully(Stream stream, byte[] buffer, int count)
    {
        var read = 0;
        while (read < count)
        {
            var n = stream.Read(buffer, read, count - read);
            if (n <= 0) return false;
            read += n;
        }
        return true;
    }

    public void Dispose()
    {
        lock (_lock)
        {
            if (_disposed) return;
            _disposed = true;
            try { _stream?.Flush(true); } catch { /* ignore */ }
            _stream?.Dispose();
            _stream = null;
        }
    }
}
