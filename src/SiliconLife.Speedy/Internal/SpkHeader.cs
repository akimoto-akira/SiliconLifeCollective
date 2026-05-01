namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Represents the fixed 32-byte header at the start of every .spk file.
/// Layout:
///   [0..3]   Magic       — ASCII "SPKY" (4 bytes)
///   [4..5]   Version     — uint16 (2 bytes)
///   [6..7]   Flags       — uint16 (2 bytes)
///   [8..15]  DirectoryOffset — int64 (8 bytes)
///   [16..19] DirectoryLength — int32 (4 bytes)
///   [20..31] Reserved    — 12 bytes (zeroed)
/// </summary>
internal struct SpkHeader
{
    public const int Size = 32;
    public const string MagicString = "SPKY";
    public const ushort CurrentVersion = 1;

    private static readonly byte[] MagicBytes = "SPKY"u8.ToArray();

    public byte[] Magic;        // 4 bytes
    public ushort Version;      // 2 bytes
    public ushort Flags;        // 2 bytes
    public long DirectoryOffset;  // 8 bytes
    public int DirectoryLength;   // 4 bytes
    // 12 reserved bytes (not stored as a field — written as zeros)

    /// <summary>
    /// Creates a default header for a new .spk file.
    /// DirectoryOffset is set to 32 (immediately after the header),
    /// DirectoryLength is 0 (empty directory).
    /// </summary>
    public static SpkHeader CreateNew() => new()
    {
        Magic = MagicBytes,
        Version = CurrentVersion,
        Flags = 0,
        DirectoryOffset = Size,   // data region starts right after header
        DirectoryLength = 0
    };

    /// <summary>
    /// Writes this header to the given <see cref="BinaryWriter"/>.
    /// The writer's stream position is advanced by exactly 32 bytes.
    /// </summary>
    public readonly void WriteTo(BinaryWriter writer)
    {
        writer.Write(Magic);                // 4 bytes
        writer.Write(Version);              // 2 bytes
        writer.Write(Flags);               // 2 bytes
        writer.Write(DirectoryOffset);     // 8 bytes
        writer.Write(DirectoryLength);     // 4 bytes
        writer.Write(new byte[12]);        // 12 reserved bytes
    }

    /// <summary>
    /// Reads a header from the given <see cref="BinaryReader"/>.
    /// Throws <see cref="InvalidDataException"/> if the magic bytes are wrong.
    /// </summary>
    public static SpkHeader ReadFrom(BinaryReader reader)
    {
        var magic = reader.ReadBytes(4);
        if (magic.Length != 4 ||
            magic[0] != MagicBytes[0] || magic[1] != MagicBytes[1] ||
            magic[2] != MagicBytes[2] || magic[3] != MagicBytes[3])
        {
            throw new InvalidDataException(
                $"Invalid .spk file: expected magic 'SPKY', got '{System.Text.Encoding.ASCII.GetString(magic)}'.");
        }

        var version = reader.ReadUInt16();
        var flags = reader.ReadUInt16();
        var directoryOffset = reader.ReadInt64();
        var directoryLength = reader.ReadInt32();
        reader.ReadBytes(12); // consume reserved bytes

        return new SpkHeader
        {
            Magic = magic,
            Version = version,
            Flags = flags,
            DirectoryOffset = directoryOffset,
            DirectoryLength = directoryLength
        };
    }
}
