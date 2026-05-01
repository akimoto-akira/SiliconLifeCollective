namespace SiliconLife.Speedy.Manager.Models;

public record EntryMetadata(
    string VirtualPath,
    string ContentType,
    int Length,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
