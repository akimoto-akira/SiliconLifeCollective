using MessagePack;

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Metadata for a single entry stored in the .spk Data Region.
/// Serialized with MessagePack as part of the Directory Region.
/// </summary>
[MessagePackObject(AllowPrivate = true)]
internal record DirectoryEntry(
    [property: Key(0)] long Offset,         // byte offset of the entry's data in the Data Region
    [property: Key(1)] int Length,          // byte count of the entry's data
    [property: Key(2)] string ContentType,  // "json" | "raw" | "text"
    [property: Key(3)] DateTime CreatedAt,
    [property: Key(4)] DateTime UpdatedAt
);
