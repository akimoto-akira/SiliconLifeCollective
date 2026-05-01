using MessagePack;

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Handles all write operations on a .spk file:
/// creating the file, appending entries to the Data Region,
/// and rewriting the Directory Region on flush.
/// </summary>
internal sealed class PackFileWriter : IDisposable
{
    private readonly FileStream _stream;
    private bool _disposed;

    private PackFileWriter(FileStream stream)
    {
        _stream = stream;
    }

    // ─── Factory ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Creates a new .spk file at <paramref name="filePath"/>, writes the initial
    /// 32-byte header, and returns a <see cref="PackFileWriter"/> ready for use.
    /// If the file already exists it is overwritten.
    /// </summary>
    public static PackFileWriter Create(string filePath)
    {
        var stream = new FileStream(
            filePath,
            FileMode.Create,
            FileAccess.ReadWrite,
            FileShare.Read,
            bufferSize: 4096,
            useAsync: false);

        var writer = new PackFileWriter(stream);
        writer.WriteInitialHeader();
        return writer;
    }

    /// <summary>
    /// Opens an existing .spk file for read/write access.
    /// The caller is responsible for ensuring the file is a valid .spk file.
    /// </summary>
    public static PackFileWriter Open(string filePath)
    {
        var stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.ReadWrite,
            FileShare.Read,
            bufferSize: 4096,
            useAsync: false);

        return new PackFileWriter(stream);
    }

    // ─── Header ───────────────────────────────────────────────────────────────

    private void WriteInitialHeader()
    {
        _stream.Seek(0, SeekOrigin.Begin);
        using var bw = new BinaryWriter(_stream, System.Text.Encoding.UTF8, leaveOpen: true);
        var header = SpkHeader.CreateNew();
        header.WriteTo(bw);
        bw.Flush();
    }

    /// <summary>
    /// Reads the current header from the file.
    /// </summary>
    public SpkHeader ReadHeader()
    {
        _stream.Seek(0, SeekOrigin.Begin);
        using var br = new BinaryReader(_stream, System.Text.Encoding.UTF8, leaveOpen: true);
        return SpkHeader.ReadFrom(br);
    }

    /// <summary>
    /// Overwrites the header fields <see cref="SpkHeader.DirectoryOffset"/> and
    /// <see cref="SpkHeader.DirectoryLength"/> in-place (bytes 8–19 of the file).
    /// </summary>
    private void UpdateHeaderDirectoryInfo(long directoryOffset, int directoryLength)
    {
        // DirectoryOffset starts at byte 8, DirectoryLength at byte 16
        _stream.Seek(8, SeekOrigin.Begin);
        using var bw = new BinaryWriter(_stream, System.Text.Encoding.UTF8, leaveOpen: true);
        bw.Write(directoryOffset);   // 8 bytes
        bw.Write(directoryLength);   // 4 bytes
        bw.Flush();
    }

    // ─── Data Region ──────────────────────────────────────────────────────────

    /// <summary>
    /// Appends <paramref name="data"/> to the Data Region (at the current end of file,
    /// before any existing Directory Region).  The entry is prefixed with a 4-byte
    /// length field (int32, little-endian).
    /// </summary>
    /// <returns>
    /// A <see cref="DirectoryEntry"/> describing the new entry's location and metadata.
    /// </returns>
    public DirectoryEntry AppendEntry(string normalizedPath, byte[] data, string contentType)
    {
        // We always append at the current logical end of the data region.
        // The simplest strategy: seek to end of file and write there.
        // (The directory region, if present, will be overwritten by WriteDirectory later.)
        _stream.Seek(0, SeekOrigin.End);
        long offset = _stream.Position;

        using var bw = new BinaryWriter(_stream, System.Text.Encoding.UTF8, leaveOpen: true);
        bw.Write(data.Length);   // 4-byte length prefix
        bw.Write(data);
        bw.Flush();

        var now = DateTime.UtcNow;
        return new DirectoryEntry(
            Offset: offset + 4,   // offset points to the data bytes, after the length prefix
            Length: data.Length,
            ContentType: contentType,
            CreatedAt: now,
            UpdatedAt: now);
    }

    /// <summary>
    /// Appends <paramref name="data"/> to the Data Region, preserving the original
    /// <see cref="DirectoryEntry.CreatedAt"/> timestamp from <paramref name="existingEntry"/>.
    /// Used when updating an existing entry (old data is left as free space).
    /// </summary>
    public DirectoryEntry AppendEntryUpdate(string normalizedPath, byte[] data, string contentType,
        DirectoryEntry existingEntry)
    {
        _stream.Seek(0, SeekOrigin.End);
        long offset = _stream.Position;

        using var bw = new BinaryWriter(_stream, System.Text.Encoding.UTF8, leaveOpen: true);
        bw.Write(data.Length);
        bw.Write(data);
        bw.Flush();

        return new DirectoryEntry(
            Offset: offset + 4,
            Length: data.Length,
            ContentType: contentType,
            CreatedAt: existingEntry.CreatedAt,
            UpdatedAt: DateTime.UtcNow);
    }

    // ─── Directory Region ─────────────────────────────────────────────────────

    /// <summary>
    /// Serializes <paramref name="entries"/> with MessagePack, writes the bytes at the
    /// current end of file, then updates the Header's DirectoryOffset and DirectoryLength.
    /// </summary>
    public void WriteDirectory(Dictionary<string, DirectoryEntry> entries)
    {
        // Seek to end of file — the directory region is always written at the tail.
        _stream.Seek(0, SeekOrigin.End);
        long directoryOffset = _stream.Position;

        var bytes = MessagePackSerializer.Serialize(entries);

        using var bw = new BinaryWriter(_stream, System.Text.Encoding.UTF8, leaveOpen: true);
        bw.Write(bytes);
        bw.Flush();

        // Truncate the file to remove any old directory region that may have been
        // written beyond the new one (shouldn't happen in normal flow, but be safe).
        _stream.SetLength(_stream.Position);

        // Update the header to point to the new directory region.
        UpdateHeaderDirectoryInfo(directoryOffset, bytes.Length);
    }

    // ─── Flush ────────────────────────────────────────────────────────────────

    /// <summary>Flushes all buffered data to the underlying OS file.</summary>
    public void Flush()
    {
        _stream.Flush(flushToDisk: true);
    }

    // ─── IDisposable ──────────────────────────────────────────────────────────

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _stream.Flush();
        _stream.Dispose();
    }
}
