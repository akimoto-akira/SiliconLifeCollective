using MessagePack;

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Handles all read operations on a .spk file:
/// validating the header, loading the directory index, and reading entry bytes.
/// </summary>
internal sealed class PackFileReader : IDisposable
{
    private readonly FileStream _stream;
    private bool _disposed;

    private PackFileReader(FileStream stream)
    {
        _stream = stream;
    }

    // ─── Factory ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Opens an existing .spk file for reading, validates the header magic bytes,
    /// and returns a <see cref="PackFileReader"/> ready for use.
    /// </summary>
    /// <exception cref="InvalidDataException">
    /// Thrown when the file does not start with the expected "SPKY" magic bytes.
    /// </exception>
    public static PackFileReader Open(string filePath)
    {
        var stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite,
            bufferSize: 4096,
            useAsync: false);

        var reader = new PackFileReader(stream);
        // Validate header immediately on open
        reader.ReadHeader();
        return reader;
    }

    // ─── Header ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Reads and validates the 32-byte header from the beginning of the file.
    /// </summary>
    /// <exception cref="InvalidDataException">
    /// Thrown when the magic bytes do not match "SPKY".
    /// </exception>
    public SpkHeader ReadHeader()
    {
        _stream.Seek(0, SeekOrigin.Begin);
        using var br = new BinaryReader(_stream, System.Text.Encoding.UTF8, leaveOpen: true);
        return SpkHeader.ReadFrom(br);
    }

    // ─── Directory Region ─────────────────────────────────────────────────────

    /// <summary>
    /// Reads the Directory Region from the file using the offsets stored in the header,
    /// deserializes it with MessagePack, and returns the full directory dictionary.
    /// </summary>
    /// <returns>
    /// A dictionary mapping normalized paths to their <see cref="DirectoryEntry"/> metadata.
    /// Returns an empty dictionary if <see cref="SpkHeader.DirectoryLength"/> is 0.
    /// </returns>
    public Dictionary<string, DirectoryEntry> LoadDirectory()
    {
        var header = ReadHeader();

        if (header.DirectoryLength == 0)
            return new Dictionary<string, DirectoryEntry>();

        _stream.Seek(header.DirectoryOffset, SeekOrigin.Begin);
        var bytes = new byte[header.DirectoryLength];
        _stream.ReadExactly(bytes);

        return MessagePackSerializer.Deserialize<Dictionary<string, DirectoryEntry>>(bytes);
    }

    // ─── Data Region ──────────────────────────────────────────────────────────

    /// <summary>
    /// Seeks to <paramref name="offset"/> in the file and reads exactly
    /// <paramref name="length"/> bytes, returning them as a new array.
    /// </summary>
    /// <param name="offset">
    /// The byte offset of the entry's data (as stored in <see cref="DirectoryEntry.Offset"/>).
    /// This points directly to the data bytes, not the 4-byte length prefix.
    /// </param>
    /// <param name="length">Number of bytes to read.</param>
    /// <returns>The raw entry bytes.</returns>
    /// <exception cref="InvalidDataException">
    /// Thrown when fewer bytes than expected are available at the given offset.
    /// </exception>
    public byte[] ReadAt(long offset, int length)
    {
        _stream.Seek(offset, SeekOrigin.Begin);
        var buffer = new byte[length];
        int totalRead = 0;
        while (totalRead < length)
        {
            int read = _stream.Read(buffer, totalRead, length - totalRead);
            if (read == 0)
                throw new InvalidDataException(
                    $"Unexpected end of file reading {length} bytes at offset {offset}. " +
                    $"Only {totalRead} bytes were available.");
            totalRead += read;
        }
        return buffer;
    }

    // ─── IDisposable ──────────────────────────────────────────────────────────

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _stream.Dispose();
    }
}
