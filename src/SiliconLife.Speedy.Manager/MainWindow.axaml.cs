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

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using SiliconLife.Speedy;
using System.Collections.ObjectModel;
using System.IO;

namespace SiliconLife.Speedy.Manager;

public partial class MainWindow : Window
{
    private SpeedyPack? _currentPack;
    private string _currentFilePath = string.Empty;
    private string _currentPath = string.Empty;

    private readonly ObservableCollection<FileEntryItem> _fileItems = [];
    private readonly ObservableCollection<DirectoryTreeNode> _rootNodes = [];

    public MainWindow() : this(null) { }

    public MainWindow(SpeedyPack? pack)
    {
        InitializeComponent();

        FileList.ItemsSource = _fileItems;
        DirectoryTree.ItemsSource = _rootNodes;

        BtnOpen.Click += async (_, _) => await OpenFile_ClickAsync();
        BtnRefresh.Click += (_, _) => RefreshView();
        BtnCompact.Click += async (_, _) => await Compact_ClickAsync();

        DirectoryTree.SelectionChanged += DirectoryTree_SelectionChanged;

        KeyDown += MainWindow_KeyDown;

        if (pack != null)
        {
            _currentPack = pack;
            Title = "Speedy Pack Manager - Inside";
            UpdateBreadcrumb();
            StatusLabel.Text = "Internal launch";
            BtnRefresh.IsEnabled = true;
            BtnCompact.IsEnabled = false;
            RefreshView();
        }
    }

    private async void MainWindow_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyModifiers.HasFlag(KeyModifiers.Control) && e.Key == Key.O)
        {
            await OpenFile_ClickAsync();
            e.Handled = true;
        }
        else if (e.Key == Key.F5)
        {
            RefreshView();
            e.Handled = true;
        }
    }

    private async Task OpenFile_ClickAsync()
    {
        var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
        if (storageProvider == null) return;

        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Speedy Pack File",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("Speedy Pack Files") { Patterns = ["*.spk"] },
                new FilePickerFileType("All Files") { Patterns = ["*.*"] }
            ]
        });

        if (files.Count > 0)
        {
            var filePath = files[0].TryGetLocalPath();
            if (filePath != null)
            {
                await LoadPackAsync(filePath);
            }
        }
    }

    private async Task Compact_ClickAsync()
    {
        if (_currentPack == null) return;

        try
        {
            StatusLabel.Text = "Compacting...";
            await _currentPack.CompactAsync();
            StatusLabel.Text = "Compact completed";
            RefreshView();
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Compact failed: {ex.Message}");
            StatusLabel.Text = "Compact failed";
        }
    }

    private async Task LoadPackAsync(string filePath)
    {
        try
        {
            _currentPack?.Dispose();
            _currentPack = SpeedyPack.Open(filePath);
            _currentFilePath = filePath;
            _currentPath = string.Empty;

            var fileName = Path.GetFileName(filePath);
            var fileInfo = new FileInfo(filePath);
            var sizeStr = FormatSize((int)fileInfo.Length);

            Title = $"Speedy Pack Manager - {fileName}";
            UpdateBreadcrumb();
            StatusLabel.Text = $"Loaded: {fileName} ({sizeStr})";

            BtnRefresh.IsEnabled = true;
            BtnCompact.IsEnabled = true;

            RefreshView();
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Failed to open file: {ex.Message}");
            StatusLabel.Text = "Failed to open file";
        }
    }

    private void RefreshView()
    {
        if (_currentPack == null) return;

        RefreshTree();
        RefreshListView(_currentPath);
    }

    private void UpdateBreadcrumb()
    {
        BreadcrumbPanel.ItemsSource = null;
        var items = new ObservableCollection<BreadcrumbItem>();

        if (string.IsNullOrEmpty(_currentFilePath))
        {
            items.Add(new BreadcrumbItem("No file opened", null));
            BreadcrumbPanel.ItemsSource = items;
            return;
        }

        var fileName = Path.GetFileName(_currentFilePath);
        items.Add(new BreadcrumbItem(fileName, string.Empty));

        if (!string.IsNullOrEmpty(_currentPath))
        {
            var parts = _currentPath.Split('/');
            var accumulatedPath = string.Empty;

            foreach (var part in parts)
            {
                accumulatedPath = string.IsNullOrEmpty(accumulatedPath) ? part : $"{accumulatedPath}/{part}";
                items.Add(new BreadcrumbItem(part, accumulatedPath));
            }
        }

        BreadcrumbPanel.ItemsSource = items;
    }

    private void BreadcrumbItem_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is BreadcrumbItem item && item.Path != null)
        {
            _currentPath = item.Path;
            RefreshListView(_currentPath);
            UpdateBreadcrumb();
        }
    }

    private void RefreshTree()
    {
        _rootNodes.Clear();

        if (_currentPack == null) return;

        var allPaths = new HashSet<string>();
        var queue = new Queue<string>();
        queue.Enqueue(string.Empty);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var subDirs = _currentPack.ListDirectories(current);
            foreach (var sub in subDirs)
            {
                if (allPaths.Add(sub))
                {
                    queue.Enqueue(sub);
                }
            }
        }

        var rootNode = new DirectoryTreeNode(Path.GetFileName(_currentFilePath) ?? "Root", string.Empty);

        var dirTree = new Dictionary<string, DirectoryTreeNode>(StringComparer.Ordinal);
        foreach (var dir in allPaths.OrderBy(d => d))
        {
            var parts = dir.Split('/');
            var parentPath = string.Empty;
            DirectoryTreeNode? parentNode = rootNode;

            foreach (var part in parts)
            {
                var currentPath = string.IsNullOrEmpty(parentPath) ? part : $"{parentPath}/{part}";

                if (!dirTree.ContainsKey(currentPath))
                {
                    var node = new DirectoryTreeNode(part, currentPath);
                    parentNode?.Children.Add(node);
                    dirTree[currentPath] = node;
                }

                parentPath = currentPath;
                parentNode = dirTree[currentPath];
            }
        }

        rootNode.IsExpanded = true;
        _rootNodes.Add(rootNode);
    }

    private void RefreshListView(string directoryPath)
    {
        _fileItems.Clear();

        if (_currentPack == null) return;

        var directories = _currentPack.ListDirectories(directoryPath);

        foreach (var dir in directories)
        {
            var dirName = dir.Contains('/') ? dir.Split('/').Last() : dir;
            _fileItems.Add(new FileEntryItem(dirName, "Folder", string.Empty, string.Empty, "dir", dir));
        }

        var entries = _currentPack.ListEntries(directoryPath);

        foreach (var entry in entries)
        {
            var metadata = _currentPack.GetEntryMetadata(entry);
            if (metadata.HasValue)
            {
                var fileName = Path.GetFileName(entry);
                _fileItems.Add(new FileEntryItem(
                    fileName,
                    metadata.Value.ContentType.ToUpper(),
                    FormatSize(metadata.Value.Length),
                    metadata.Value.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    "entry",
                    entry));
            }
        }

        ItemCountLabel.Text = $"{_fileItems.Count} items";
    }

    private void DirectoryTree_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_currentPack == null) return;

        if (DirectoryTree.SelectedItem is DirectoryTreeNode node)
        {
            _currentPath = node.Path;
            RefreshListView(_currentPath);
            UpdateBreadcrumb();
        }
    }

    private void FileList_DoubleTapped(object? sender, TappedEventArgs e)
    {
        if (FileList.SelectedItem is not FileEntryItem item) return;

        if (item.ItemType == "dir")
        {
            _currentPath = item.ItemPath;
            RefreshListView(_currentPath);
            UpdateBreadcrumb();
        }
        else
        {
            ViewContent(item.ItemPath);
        }
    }

    private void FileList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (FileList.SelectedItem is FileEntryItem item && item.ItemType == "entry")
        {
            PreviewContent(item.ItemPath);
        }
        else
        {
            PreviewTextBox.Text = "Select a file to preview content";
        }
    }

    private void PreviewContent(string path)
    {
        if (_currentPack == null) return;

        try
        {
            var bytes = _currentPack.Read(path);
            if (bytes == null)
            {
                PreviewTextBox.Text = "Unable to read content";
                return;
            }

            var metadata = _currentPack.GetEntryMetadata(path);
            string content;

            if (metadata?.ContentType == "json")
            {
                content = System.Text.Encoding.UTF8.GetString(bytes);
                content = DecodeUnicodeEscapes(content);
                if (content.Length > 2000)
                {
                    content = content[..2000] + "\n\n... [Preview truncated]";
                }
            }
            else if (metadata?.ContentType == "text" || path.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            {
                content = System.Text.Encoding.UTF8.GetString(bytes);
                if (content.Length > 2000)
                {
                    content = content[..2000] + "\n\n... [Preview truncated]";
                }
            }
            else
            {
                content = $"[Binary Data - {bytes.Length} bytes]\n\nHex preview:\n";
                var preview = bytes.Take(Math.Min(256, bytes.Length)).ToArray();
                content += BitConverter.ToString(preview).Replace("-", " ");
            }

            PreviewTextBox.Text = content;
        }
        catch (Exception ex)
        {
            PreviewTextBox.Text = $"Error reading content: {ex.Message}";
        }
    }

    private async void ViewContent(string path)
    {
        if (_currentPack == null) return;

        try
        {
            var bytes = _currentPack.Read(path);
            if (bytes == null)
            {
                await ShowWarningAsync("Entry not found.");
                return;
            }

            var metadata = _currentPack.GetEntryMetadata(path);
            string content;

            if (metadata?.ContentType == "json")
            {
                content = System.Text.Encoding.UTF8.GetString(bytes);
                content = DecodeUnicodeEscapes(content);
            }
            else if (metadata?.ContentType == "text" || path.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            {
                content = System.Text.Encoding.UTF8.GetString(bytes);
            }
            else
            {
                content = $"[Binary Data - {bytes.Length} bytes]\n\nHex preview:\n";
                var preview = bytes.Take(Math.Min(256, bytes.Length)).ToArray();
                content += BitConverter.ToString(preview).Replace("-", " ");
            }

            var viewer = new ContentViewerWindow(Path.GetFileName(path), content);
            await viewer.ShowDialog(this);
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Error reading content: {ex.Message}");
        }
    }

    private static string FormatSize(int bytes)
    {
        if (bytes < 1024)
            return $"{bytes} B";
        if (bytes < 1024 * 1024)
            return $"{bytes / 1024.0:F2} KB";
        return $"{bytes / (1024.0 * 1024.0):F2} MB";
    }

    private static string DecodeUnicodeEscapes(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new System.Text.StringBuilder(input.Length);
        int i = 0;

        while (i < input.Length)
        {
            if (i + 5 < input.Length &&
                input[i] == '\\' &&
                input[i + 1] == 'u' &&
                IsHexDigit(input[i + 2]) &&
                IsHexDigit(input[i + 3]) &&
                IsHexDigit(input[i + 4]) &&
                IsHexDigit(input[i + 5]))
            {
                string hex = input.Substring(i + 2, 4);
                int codePoint = Convert.ToInt32(hex, 16);

                if (char.IsHighSurrogate((char)codePoint) &&
                    i + 11 < input.Length &&
                    input[i + 6] == '\\' &&
                    input[i + 7] == 'u')
                {
                    string lowHex = input.Substring(i + 8, 4);
                    if (IsHexDigit(lowHex[0]) && IsHexDigit(lowHex[1]) &&
                        IsHexDigit(lowHex[2]) && IsHexDigit(lowHex[3]))
                    {
                        int lowCodePoint = Convert.ToInt32(lowHex, 16);
                        char highSurrogate = (char)codePoint;
                        char lowSurrogate = (char)lowCodePoint;

                        if (char.IsLowSurrogate(lowSurrogate))
                        {
                            result.Append(highSurrogate);
                            result.Append(lowSurrogate);
                            i += 12;
                            continue;
                        }
                    }
                }

                result.Append((char)codePoint);
                i += 6;
            }
            else
            {
                result.Append(input[i]);
                i++;
            }
        }

        return result.ToString();
    }

    private static bool IsHexDigit(char c) =>
        char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');

    private async Task ShowErrorAsync(string message)
    {
        var dialog = new Window
        {
            Title = "Error",
            Width = 400,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var panel = new StackPanel { Margin = new Thickness(20) };
        panel.Children.Add(new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Thickness(0, 0, 0, 16)
        });

        var okBtn = new Button { Content = "OK", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
        okBtn.Click += (_, _) => dialog.Close();
        panel.Children.Add(okBtn);

        dialog.Content = panel;
        await dialog.ShowDialog(this);
    }

    private async Task ShowWarningAsync(string message)
    {
        var dialog = new Window
        {
            Title = "Warning",
            Width = 400,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var panel = new StackPanel { Margin = new Thickness(20) };
        panel.Children.Add(new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Thickness(0, 0, 0, 16)
        });

        var okBtn = new Button { Content = "OK", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
        okBtn.Click += (_, _) => dialog.Close();
        panel.Children.Add(okBtn);

        dialog.Content = panel;
        await dialog.ShowDialog(this);
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        _currentPack?.Dispose();
        base.OnClosing(e);
    }
}

public class DirectoryTreeNode
{
    public string Name { get; }
    public string Path { get; }
    public ObservableCollection<DirectoryTreeNode> Children { get; } = [];
    public bool IsExpanded { get; set; }

    public DirectoryTreeNode(string name, string path)
    {
        Name = name;
        Path = path;
    }
}

public class FileEntryItem
{
    public string Name { get; }
    public string Type { get; }
    public string Size { get; }
    public string Modified { get; }
    public string ItemType { get; }
    public string ItemPath { get; }

    public FileEntryItem(string name, string type, string size, string modified, string itemType, string itemPath)
    {
        Name = name;
        Type = type;
        Size = size;
        Modified = modified;
        ItemType = itemType;
        ItemPath = itemPath;
    }
}

public class BreadcrumbItem
{
    public string Label { get; }
    public string? Path { get; }

    public BreadcrumbItem(string label, string? path)
    {
        Label = label;
        Path = path;
    }
}
