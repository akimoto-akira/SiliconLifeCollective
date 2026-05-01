using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SiliconLife.Speedy.Manager.Models;

public partial class PackEntryNode : ObservableObject
{
    public string Name { get; }
    public string FullPath { get; }
    public bool IsDirectory { get; }
    public ObservableCollection<PackEntryNode> Children { get; } = new();

    [ObservableProperty]
    private bool _isExpanded;

    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private bool _isHighlighted;

    [ObservableProperty]
    private bool _isLoaded;

    public PackEntryNode(string fullPath, bool isDirectory)
    {
        FullPath = fullPath;
        IsDirectory = isDirectory;

        var lastSlash = fullPath.LastIndexOf('/');
        Name = lastSlash >= 0 ? fullPath[(lastSlash + 1)..] : fullPath;
    }
}
