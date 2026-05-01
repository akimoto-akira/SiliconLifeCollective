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

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Atomic transaction implementation for batch operations.
/// </summary>
internal sealed class SpeedyTransaction : IPackTransaction
{
    private readonly SpeedyPack _pack;
    private readonly List<WriteOperation> _operations = new();
    private bool _disposed;

    public bool IsCommitted { get; private set; }

    public SpeedyTransaction(SpeedyPack pack)
    {
        _pack = pack;
    }

    public void Write(string path, ReadOnlySpan<byte> data)
    {
        ThrowIfDisposed();
        var normalizedPath = PathNormalizer.Normalize(path);
        _operations.Add(new WriteEntry(normalizedPath, data.ToArray(), "raw"));
    }

    public void Write(string path, byte[] data)
    {
        Write(path, data.AsSpan());
    }

    public void Write<T>(string path, T value)
    {
        ThrowIfDisposed();
        var normalizedPath = PathNormalizer.Normalize(path);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        _operations.Add(new WriteEntry(normalizedPath, bytes, "json"));
    }

    public void Delete(string path)
    {
        ThrowIfDisposed();
        var normalizedPath = PathNormalizer.Normalize(path);
        _operations.Add(new DeleteEntry(normalizedPath));
    }

    /// <summary>
    /// Atomically commits all buffered operations to the main pack.
    /// </summary>
    public void Commit()
    {
        ThrowIfDisposed();

        if (IsCommitted)
            throw new InvalidOperationException("Transaction already committed.");

        // Apply all operations atomically to the pack
        _pack.ApplyTransactionBatch(_operations);
        IsCommitted = true;
    }

    /// <summary>
    /// Discards all buffered operations without affecting the main pack.
    /// </summary>
    public void Rollback()
    {
        ThrowIfDisposed();
        _operations.Clear();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        // Auto-rollback if not committed
        if (!IsCommitted)
            _operations.Clear();

        _disposed = true;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(SpeedyTransaction));
    }
}
