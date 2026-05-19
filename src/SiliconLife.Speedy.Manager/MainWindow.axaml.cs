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
using System.Linq;

namespace SiliconLife.Speedy.Manager;

public partial class MainWindow : Window
{
    private SpeedyPack? _currentPack;
    private string _currentFilePath = string.Empty;
    private string _currentPath = string.Empty;
    private bool _isImporting;

    private readonly ObservableCollection<DirectoryTreeNode> _rootNodes = [];

    public MainWindow() : this(null) { }

    public MainWindow(SpeedyPack? pack)
    {
        InitializeComponent();

        DirectoryTree.ItemsSource = _rootNodes;

        BtnNew.Click += async (_, _) => await NewFile_ClickAsync();
        BtnOpen.Click += async (_, _) => await OpenFile_ClickAsync();
        BtnImportFile.Click += async (_, _) => await ImportFile_ClickAsync();
        BtnRefresh.Click += (_, _) => RefreshView();
        BtnCompact.Click += async (_, _) => await Compact_ClickAsync();

        DirectoryTree.SelectionChanged += DirectoryTree_SelectionChanged;

        MenuImportFolder.Click += async (_, _) => await ImportFolder_ClickAsync();
        MenuExportEntry.Click += async (_, _) => await ExportEntry_ClickAsync();

        KeyDown += MainWindow_KeyDown;

        if (pack != null)
        {
            _currentPack = pack;
            Title = "Speedy Pack Manager - Inside";
            UpdateBreadcrumb();
            StatusLabel.Text = "Internal launch";
            BtnRefresh.IsEnabled = true;
            BtnCompact.IsEnabled = false;
            BtnImportFile.IsEnabled = true;
            RefreshView();
        }
    }

    private async void MainWindow_KeyDown(object? sender, KeyEventArgs e)
    {
        if (_isImporting) return;

        if (e.KeyModifiers.HasFlag(KeyModifiers.Control) && e.Key == Key.N)
        {
            await NewFile_ClickAsync();
            e.Handled = true;
        }
        else if (e.KeyModifiers.HasFlag(KeyModifiers.Control) && e.Key == Key.O)
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

    // ─── New File ──────────────────────────────────────────────────────────────

    private async Task NewFile_ClickAsync()
    {
        if (_isImporting) return;

        var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
        if (storageProvider == null) return;

        var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Create New Speedy Pack File",
            SuggestedFileName = "Untitled",
            FileTypeChoices =
            [
                new FilePickerFileType("Speedy Pack Files") { Patterns = ["*.spk"] }
            ]
        });

        if (file == null) return;

        var filePath = file.TryGetLocalPath();
        if (filePath == null) return;

        // Ensure the file has .spk extension
        if (!filePath.EndsWith(".spk", StringComparison.OrdinalIgnoreCase))
            filePath += ".spk";

        try
        {
            _currentPack?.Dispose();
            _currentPack = SpeedyPack.Create(filePath);
            _currentFilePath = filePath;
            _currentPath = string.Empty;

            var fileName = Path.GetFileName(filePath);

            Title = $"Speedy Pack Manager - {fileName}";
            UpdateBreadcrumb();
            StatusLabel.Text = $"Created: {fileName}";

            BtnRefresh.IsEnabled = true;
            BtnCompact.IsEnabled = true;
            BtnImportFile.IsEnabled = true;

            RefreshView();
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Failed to create file: {ex.Message}");
            StatusLabel.Text = "Failed to create file";
        }
    }

    private async Task OpenFile_ClickAsync()
    {
        if (_isImporting) return;

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
        if (_currentPack == null || _isImporting) return;

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
            BtnImportFile.IsEnabled = true;

            RefreshView();
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Failed to open file: {ex.Message}");
            StatusLabel.Text = "Failed to open file";
        }
    }

    // ─── Import File ────────────────────────────────────────────────────────────

    private async Task ImportFile_ClickAsync()
    {
        if (_currentPack == null || _isImporting) return;

        var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
        if (storageProvider == null) return;

        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Files to Import",
            AllowMultiple = true
        });

        if (files.Count == 0) return;

        try
        {
            StatusLabel.Text = $"Importing {files.Count} file(s)...";

            foreach (var file in files)
            {
                var localPath = file.TryGetLocalPath();
                if (localPath == null) continue;

                var fileName = Path.GetFileName(localPath);
                var packPath = string.IsNullOrEmpty(_currentPath)
                    ? fileName
                    : $"{_currentPath}/{fileName}";

                await using var stream = await file.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var data = ms.ToArray();

                var contentType = InferContentType(fileName);
                _currentPack.Write(packPath, data, contentType);
            }

            await _currentPack.FlushAsync();
            StatusLabel.Text = $"Imported {files.Count} file(s)";
            RefreshView();
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Import failed: {ex.Message}");
            StatusLabel.Text = "Import failed";
        }
    }

    // ─── Import Folder (Tree Context Menu) ──────────────────────────────────────

    private async Task ImportFolder_ClickAsync()
    {
        if (_currentPack == null) return;

        var selectedNode = DirectoryTree.SelectedItem as DirectoryTreeNode;
        var targetPath = selectedNode?.Path ?? string.Empty;

        var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
        if (storageProvider == null) return;

        var folders = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Folder to Import",
            AllowMultiple = false
        });

        if (folders.Count == 0) return;

        var folderPath = folders[0].TryGetLocalPath();
        if (folderPath == null) return;

        var folderName = Path.GetFileName(folderPath);
        var importRoot = string.IsNullOrEmpty(targetPath)
            ? folderName
            : $"{targetPath}/{folderName}";

        // Phase 1: Scan total file count
        var progressWindow = new ProgressWindow($"Importing '{folderName}'");
        progressWindow.SetIndeterminate("Scanning files...");
        _isImporting = true;

        try
        {
            // Show modal progress dialog — blocks main window interaction
            var dialogTask = progressWindow.ShowDialog(this);

            // Give the dialog a moment to render
            await Task.Delay(50);

            var totalFiles = CountFiles(folderPath);

            if (totalFiles == 0)
            {
                progressWindow.Complete("No files found to import.");
                await Task.Delay(800);
                progressWindow.Close();
                await dialogTask;
                _isImporting = false;
                StatusLabel.Text = "No files found to import.";
                return;
            }

            progressWindow.ProgressBar.Maximum = totalFiles;
            progressWindow.ProgressBar.IsIndeterminate = false;
            progressWindow.ProgressBar.Value = 0;
            progressWindow.StatusLabel.Text = $"0 / {totalFiles} files...";

            // Phase 2: Import with progress
            var totalImported = 0;

            await Task.Run(() =>
            {
                ImportDirectoryRecursive(folderPath, importRoot, (fileName) =>
                {
                    totalImported++;
                    var captured = totalImported;
                    Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                    {
                        progressWindow.ProgressBar.Value = captured;
                        progressWindow.StatusLabel.Text = $"{captured} / {totalFiles} — {fileName}";
                    });
                });
            });

            await _currentPack.FlushAsync();

            progressWindow.Complete($"Imported {totalImported} file(s).");
            _isImporting = false;

            StatusLabel.Text = $"Imported {totalImported} file(s) from '{folderName}'";
            RefreshView();

            // Brief pause so the user can see the completion message
            await Task.Delay(600);
            progressWindow.Close();
            await dialogTask;
        }
        catch (Exception ex)
        {
            try { progressWindow.Close(); } catch { /* ignore */ }
            await ShowErrorAsync($"Folder import failed: {ex.Message}");
            StatusLabel.Text = "Folder import failed";
        }
        finally
        {
            _isImporting = false;
        }
    }

    private static int CountFiles(string path)
    {
        var count = Directory.GetFiles(path).Length;
        foreach (var dir in Directory.GetDirectories(path))
            count += CountFiles(dir);
        return count;
    }

    private void ImportDirectoryRecursive(string physicalDir, string packDir, Action<string> onFileImported)
    {
        var pack = _currentPack!;

        foreach (var file in Directory.GetFiles(physicalDir))
        {
            var fileName = Path.GetFileName(file) ?? file;
            var packPath = $"{packDir}/{fileName}";
            var data = File.ReadAllBytes(file);
            var contentType = InferContentType(fileName);
            pack.Write(packPath, data, contentType);
            onFileImported(fileName);
        }

        foreach (var dir in Directory.GetDirectories(physicalDir))
        {
            var dirName = Path.GetFileName(dir) ?? dir;
            var subPackDir = $"{packDir}/{dirName}";
            ImportDirectoryRecursive(dir, subPackDir, onFileImported);
        }
    }

    // ─── Export Entry (Tree Context Menu) ───────────────────────────────────────

    private async Task ExportEntry_ClickAsync()
    {
        if (_currentPack == null || _isImporting) return;

        var selectedNode = DirectoryTree.SelectedItem as DirectoryTreeNode;
        if (selectedNode == null)
        {
            await ShowWarningAsync("Please select a directory or file in the tree first.");
            return;
        }

        var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
        if (storageProvider == null) return;

        if (selectedNode.IsFile)
        {
            await ExportSingleFileAsync(selectedNode.Path, storageProvider);
        }
        else
        {
            await ExportDirectoryAsync(selectedNode.Path, selectedNode.Name, storageProvider);
        }
    }

    private async Task ExportSingleFileAsync(string packPath, IStorageProvider storageProvider)
    {
        var bytes = _currentPack!.Read(packPath);
        if (bytes == null)
        {
            await ShowWarningAsync("Entry not found or cannot be read.");
            return;
        }

        var fileName = Path.GetFileName(packPath);
        var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Export File",
            SuggestedFileName = fileName
        });

        if (file == null) return;

        try
        {
            await using var stream = await file.OpenWriteAsync();
            await stream.WriteAsync(bytes);
            await stream.FlushAsync();

            StatusLabel.Text = $"Exported: {fileName}";
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Export failed: {ex.Message}");
        }
    }

    private async Task ExportDirectoryAsync(string packDir, string dirName, IStorageProvider storageProvider)
    {
        var folders = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Export Destination Folder",
            AllowMultiple = false
        });

        if (folders.Count == 0) return;

        var destPath = folders[0].TryGetLocalPath();
        if (destPath == null) return;

        try
        {
            StatusLabel.Text = $"Exporting '{dirName}'...";

            var exportedCount = 0;
            await Task.Run(() =>
            {
                exportedCount = ExportDirectoryRecursive(packDir, Path.Combine(destPath, dirName));
            });

            StatusLabel.Text = $"Exported {exportedCount} file(s) to '{dirName}'";
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Export failed: {ex.Message}");
            StatusLabel.Text = "Export failed";
        }
    }

    private int ExportDirectoryRecursive(string packDir, string physicalDir)
    {
        var count = 0;
        Directory.CreateDirectory(physicalDir);

        // Export entries in this directory
        foreach (var entry in _currentPack!.ListEntries(packDir))
        {
            var bytes = _currentPack.Read(entry);
            if (bytes == null) continue;

            var fileName = Path.GetFileName(entry);
            var filePath = Path.Combine(physicalDir, fileName);
            File.WriteAllBytes(filePath, bytes);
            count++;
        }

        // Recursively export subdirectories
        foreach (var subDir in _currentPack.ListDirectories(packDir))
        {
            var dirName = subDir.Contains('/') ? subDir.Split('/').Last() : subDir;
            count += ExportDirectoryRecursive(subDir, Path.Combine(physicalDir, dirName));
        }

        return count;
    }

    // ─── Content Type Inference ─────────────────────────────────────────────────

    private static string InferContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".json" or ".jsonl" => "json",
            ".txt" or ".md" or ".csv" or ".xml" or ".yaml" or ".yml" or ".log" or ".ini" or ".cfg" or ".conf" or ".toml" => "text",
            _ => "raw"
        };
    }

    private void RefreshView()
    {
        if (_currentPack == null || _isImporting) return;

        RefreshTree();
        var dirName = string.IsNullOrEmpty(_currentPath)
            ? Path.GetFileName(_currentFilePath) ?? "Root"
            : _currentPath.Split('/').Last();
        PreviewDirectorySummary(_currentPath, dirName);
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
            UpdateBreadcrumb();
            PreviewDirectorySummary(item.Path, item.Label);
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
        dirTree[string.Empty] = rootNode;

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

        // Add file entries as leaf nodes under their parent directories
        foreach (var dir in allPaths.Append(string.Empty))
        {
            if (!dirTree.TryGetValue(dir, out var parentNode)) continue;

            var entries = _currentPack.ListEntries(dir);
            foreach (var entry in entries)
            {
                var fileName = Path.GetFileName(entry) ?? entry;
                var meta = _currentPack.GetEntryMetadata(entry);
                var contentType = meta?.ContentType ?? "raw";
                var fileNode = new DirectoryTreeNode(fileName, entry, isFile: true, contentType: contentType);
                parentNode.Children.Add(fileNode);
            }
        }

        rootNode.IsExpanded = true;
        _rootNodes.Add(rootNode);
    }

    private void DirectoryTree_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_currentPack == null || _isImporting) return;

        if (DirectoryTree.SelectedItem is DirectoryTreeNode node)
        {
            if (node.IsFile)
            {
                PreviewContent(node.Path);
            }
            else
            {
                _currentPath = node.Path;
                UpdateBreadcrumb();
                PreviewDirectorySummary(node.Path, node.Name);
            }
        }
    }

    private void PreviewDirectorySummary(string path, string name)
    {
        if (_currentPack == null) return;

        try
        {
            var subDirs = _currentPack.ListDirectories(path);
            var entries = _currentPack.ListEntries(path);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"📁 {name}");
            sb.AppendLine($"Path: {(string.IsNullOrEmpty(path) ? "/" : path)}");
            sb.AppendLine();
            sb.AppendLine($"Folders: {subDirs.Count}");
            sb.AppendLine($"Files:   {entries.Count}");

            if (entries.Count > 0)
            {
                long totalSize = 0;
                var typeCounts = new Dictionary<string, int>();
                foreach (var entry in entries)
                {
                    var meta = _currentPack.GetEntryMetadata(entry);
                    if (meta.HasValue)
                    {
                        totalSize += meta.Value.Length;
                        var ct = meta.Value.ContentType.ToUpper();
                        typeCounts.TryGetValue(ct, out var c);
                        typeCounts[ct] = c + 1;
                    }
                }

                sb.AppendLine($"Size:    {FormatSize((int)totalSize)}");
                sb.AppendLine();
                sb.AppendLine("Content types:");
                foreach (var (type, count) in typeCounts.OrderBy(t => t.Key))
                {
                    sb.AppendLine($"  {type}: {count}");
                }
            }

            PreviewTextBox.Text = sb.ToString();
        }
        catch (Exception ex)
        {
            PreviewTextBox.Text = $"Error reading directory info: {ex.Message}";
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
    public bool IsFile { get; }
    public string ContentType { get; }
    public ObservableCollection<DirectoryTreeNode> Children { get; } = [];
    public bool IsExpanded { get; set; }

    public DirectoryTreeNode(string name, string path, bool isFile = false, string contentType = "")
    {
        Name = name;
        Path = path;
        IsFile = isFile;
        ContentType = contentType;
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
