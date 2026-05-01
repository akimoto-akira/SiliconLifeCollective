namespace SiliconLife.Speedy.Manager.Models;

public record PackFileInfo(
    string FilePath,
    long FileSize,
    string Magic,
    ushort Version,
    ushort Flags,
    long DirectoryOffset,
    int DirectoryLength,
    int TotalEntries,
    int JsonEntries,
    int RawEntries,
    int TextEntries
);
