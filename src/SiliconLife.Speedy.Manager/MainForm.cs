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

using SiliconLife.Speedy;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SiliconLife.Speedy.Manager;

public partial class MainForm : Form
{
    private SpeedyPack? _currentPack;
    private string _currentFilePath = string.Empty;
    private string _currentPath = string.Empty;

    public MainForm(SpeedyPack pack = null)
    {
        InitializeComponent();
        SetupEventHandlers();
        if(pack != null)
        {
            _currentPack = pack;
            Text = $"Speedy Pack Manager - Inside";
            UpdateBreadcrumb();
            _statusLabel.Text = $"Internal launch";

            _btnRefresh.Enabled = true;
            _btnCompact.Enabled = false;

            RefreshView();
        }
    }

    private void SetupEventHandlers()
    {
        // Tree View events
        _treeView.AfterSelect += TreeView_AfterSelect;
        
        // List View events
        _listView.DoubleClick += ListView_DoubleClick;
        _listView.SelectedIndexChanged += ListView_SelectedIndexChanged;
        
        // Ribbon buttons
        _btnOpen.Click += async (s, e) => await OpenFile_ClickAsync();
        _btnRefresh.Click += (s, e) => RefreshView();
        _btnCompact.Click += async (s, e) => await Compact_ClickAsync();
        
        // Context menu events
        _treeRefreshMenuItem.Click += (s, e) => RefreshView();
        _listRefreshMenuItem.Click += (s, e) => RefreshView();
        
        // Keyboard shortcuts
        KeyPreview = true;
        KeyDown += MainForm_KeyDown;
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.O)
        {
            _ = OpenFile_ClickAsync();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.F5)
        {
            RefreshView();
            e.Handled = true;
        }
    }

    private async Task OpenFile_ClickAsync()
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "Speedy Pack Files (*.spk)|*.spk|All Files (*.*)|*.*",
            Title = "Open Speedy Pack File"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            await LoadPackAsync(dialog.FileName);
        }
    }

    private async Task Compact_ClickAsync()
    {
        if (_currentPack == null) return;

        try
        {
            _statusLabel.Text = "Compacting...";
            await _currentPack.CompactAsync();
            _statusLabel.Text = "Compact completed";
            RefreshView();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Compact failed: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            _statusLabel.Text = "Compact failed";
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

            // Initialize folder icons
            SetupTreeIcons();

            var fileName = Path.GetFileName(filePath);
            var fileInfo = new FileInfo(filePath);
            var sizeStr = FormatSize((int)fileInfo.Length);

            Text = $"Speedy Pack Manager - {fileName}";
            UpdateBreadcrumb();
            _statusLabel.Text = $"Loaded: {fileName} ({sizeStr})";
            
            _btnRefresh.Enabled = true;
            _btnCompact.Enabled = true;

            RefreshView();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open file: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            _statusLabel.Text = "Failed to open file";
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
        _breadcrumbFlow.Controls.Clear();
        
        if (string.IsNullOrEmpty(_currentFilePath))
        {
            _breadcrumbFlow.Controls.Add(new Label
            {
                Text = "No file opened",
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(64, 64, 64),
                Margin = new Padding(0, 4, 0, 0)
            });
            return;
        }
        
        // Add file name as first breadcrumb
        var fileNameBtn = CreateBreadcrumbButton(Path.GetFileName(_currentFilePath));
        fileNameBtn.Click += (s, e) =>
        {
            _currentPath = string.Empty;
            RefreshListView(string.Empty);
            UpdateBreadcrumb();
        };
        _breadcrumbFlow.Controls.Add(fileNameBtn);
        
        // Add path segments
        if (!string.IsNullOrEmpty(_currentPath))
        {
            var parts = _currentPath.Split('/');
            var accumulatedPath = string.Empty;
            
            foreach (var part in parts)
            {
                _breadcrumbFlow.Controls.Add(new Label
                {
                    Text = "›",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 12F),
                    ForeColor = Color.FromArgb(160, 160, 160),
                    Margin = new Padding(4, 2, 4, 0)
                });
                
                accumulatedPath = string.IsNullOrEmpty(accumulatedPath) ? part : $"{accumulatedPath}/{part}";
                var partBtn = CreateBreadcrumbButton(part);
                var capturedPath = accumulatedPath;
                partBtn.Click += (s, e) =>
                {
                    _currentPath = capturedPath;
                    RefreshListView(capturedPath);
                    UpdateBreadcrumb();
                };
                _breadcrumbFlow.Controls.Add(partBtn);
            }
        }
    }
    
    private Button CreateBreadcrumbButton(string text)
    {
        var btn = new Button
        {
            Text = text,
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(240, 240, 240) },
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(0, 102, 204),
            AutoSize = true,
            Cursor = Cursors.Hand,
            Padding = new Padding(4, 2, 4, 2)
        };
        return btn;
    }

    /// <summary>
    /// Create folder icon and load it into ImageList
    /// </summary>
    private void SetupTreeIcons()
    {
        // Create a 16x16 folder icon
        var folderIcon = new Bitmap(16, 16);
        using (var g = Graphics.FromImage(folderIcon))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            // Folder body (yellow)
            var bodyBrush = new SolidBrush(Color.FromArgb(255, 204, 102));
            var folderBody = new RectangleF(0, 4, 16, 11);
            g.FillRectangle(bodyBrush, folderBody);
            
            // Folder tab (dark yellow)
            var topBrush = new SolidBrush(Color.FromArgb(240, 180, 80));
            var folderTop = new RectangleF(2, 1, 12, 4);
            g.FillRectangle(topBrush, folderTop);
            
            // Tab protrusion
            var tabBrush = new SolidBrush(Color.FromArgb(240, 180, 80));
            var tab = new RectangleF(2, 1, 6, 3);
            g.FillRectangle(tabBrush, tab);
            
            bodyBrush.Dispose();
            topBrush.Dispose();
            tabBrush.Dispose();
        }
        
        _treeImageList.Images.Add("folder", folderIcon);
        
        // Set nodes to use folder icon
        _treeView.ImageIndex = 0;
        _treeView.SelectedImageIndex = 0;
    }

    private void RefreshTree()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(RefreshTree));
            return;
        }

        _treeView.BeginUpdate();
        try
        {
            _treeView.Nodes.Clear();
            
            // Recursively collect all directory paths (BFS)
            var allPaths = new HashSet<string>();
            var queue = new Queue<string>();
            queue.Enqueue(string.Empty);
            
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var subDirs = _currentPack!.ListDirectories(current);
                foreach (var sub in subDirs)
                {
                    if (allPaths.Add(sub))
                    {
                        queue.Enqueue(sub);
                    }
                }
            }
            
            var root = _treeView.Nodes.Add(_currentFilePath, "Root");

            // Build directory tree
            var dirTree = new Dictionary<string, TreeNode>(StringComparer.Ordinal);
            foreach (var dir in allPaths.OrderBy(d => d))
            {
                var parts = dir.Split('/');
                var parentPath = string.Empty;
                TreeNode? parentNode = root;

                foreach (var part in parts)
                {
                    var currentPath = string.IsNullOrEmpty(parentPath) ? part : $"{parentPath}/{part}";
                    
                    if (!dirTree.ContainsKey(currentPath))
                    {
                        var node = new TreeNode(part)
                        {
                            ImageIndex = 0,
                            SelectedImageIndex = 0
                        };
                        parentNode?.Nodes.Add(node);
                        dirTree[currentPath] = node;
                    }
                    
                    parentPath = currentPath;
                    parentNode = dirTree[currentPath];
                }
            }

            root.Expand();
        }
        finally
        {
            _treeView.EndUpdate();
        }
    }

    private void RefreshListView(string directoryPath)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<string>(RefreshListView), directoryPath);
            return;
        }

        _listView.BeginUpdate();
        try
        {
            _listView.Items.Clear();

            // Add subdirectories
            var directories = _currentPack!.ListDirectories(directoryPath);
            
            foreach (var dir in directories)
            {
                // Extract the last-level directory name
                var dirName = dir.Contains('/') 
                    ? dir.Split('/').Last() 
                    : dir;
                var item = new ListViewItem(dirName)
                {
                    Tag = ("dir", dir)
                };
                item.SubItems.Add("Folder");
                item.SubItems.Add(string.Empty);
                item.SubItems.Add(string.Empty);
                _listView.Items.Add(item);
            }

            // Add entries
            var entries = _currentPack.ListEntries(directoryPath);
            
            foreach (var entry in entries)
            {
                var metadata = _currentPack.GetEntryMetadata(entry);
                if (metadata.HasValue)
                {
                    var fileName = Path.GetFileName(entry);
                    
                    var item = new ListViewItem(fileName)
                    {
                        Tag = ("entry", entry)
                    };
                    item.SubItems.Add(metadata.Value.ContentType.ToUpper());
                    item.SubItems.Add(FormatSize(metadata.Value.Length));
                    item.SubItems.Add(metadata.Value.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    _listView.Items.Add(item);
                }
            }

            _itemCountLabel.Text = $"{_listView.Items.Count} items";
        }
        finally
        {
            _listView.EndUpdate();
        }
    }



    private void TreeView_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (_currentPack == null || e.Node == null) return;

        // Root node should not respond to click events
        if (e.Node.Text == "Root")
        {
            return;
        }

        var path = GetNodePath(e.Node);
        _currentPath = path;
        RefreshListView(path);
        UpdateBreadcrumb();
    }

    private string GetNodePath(TreeNode node)
    {
        var parts = new List<string>();
        var current = node;

        while (current != null && current.Text != "Root")
        {
            parts.Insert(0, current.Text);
            current = current.Parent;
        }

        return string.Join("/", parts);
    }

    private void ListView_DoubleClick(object? sender, EventArgs e)
    {
        if (_listView.SelectedItems.Count == 0) return;

        var selectedItem = _listView.SelectedItems[0];
        if (selectedItem.Tag is ValueTuple<string, string> tag)
        {
            if (tag.Item1 == "dir")
            {
                // Navigate into directory
                _currentPath = tag.Item2;
                RefreshListView(_currentPath);
                UpdateBreadcrumb();
            }
            else
            {
                // View file content
                ViewContent(tag.Item2);
            }
        }
    }

    private void ViewContent_Click(object? sender, EventArgs e)
    {
        if (_listView.SelectedItems.Count == 0) return;

        var selectedItem = _listView.SelectedItems[0];
        if (selectedItem.Tag is ValueTuple<string, string> tag && tag.Item1 == "entry")
        {
            ViewContent(tag.Item2);
        }
    }
    
    private void ListView_SelectedIndexChanged(object? sender, EventArgs e)
    {
        // Update preview panel when selection changes
        if (_listView.SelectedItems.Count == 1)
        {
            var selectedItem = _listView.SelectedItems[0];
            if (selectedItem.Tag is ValueTuple<string, string> tag && tag.Item1 == "entry")
            {
                PreviewContent(tag.Item2);
            }
            else
            {
                _previewTextBox.Text = "Select a file to preview content";
            }
        }
        else
        {
            _previewTextBox.Text = "Select a file to preview content";
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
                _previewTextBox.Text = "Unable to read content";
                return;
            }

            var metadata = _currentPack.GetEntryMetadata(path);
            string content;
            
            if (metadata?.ContentType == "json")
            {
                // Read as UTF-8 string first
                content = System.Text.Encoding.UTF8.GetString(bytes);
                
                // Decode Unicode escape sequences (\uXXXX) to actual characters
                content = DecodeUnicodeEscapes(content);
                
                // Limit preview to 2000 characters
                if (content.Length > 2000)
                {
                    content = content.Substring(0, 2000) + "\n\n... [Preview truncated]";
                }
            }
            else if (metadata?.ContentType == "text")
            {
                content = System.Text.Encoding.UTF8.GetString(bytes);
                // Limit preview to 2000 characters
                if (content.Length > 2000)
                {
                    content = content.Substring(0, 2000) + "\n\n... [Preview truncated]";
                }
            }
            else
            {
                content = $"[Binary Data - {bytes.Length} bytes]\n\nHex preview:\n";
                var preview = bytes.Take(Math.Min(256, bytes.Length)).ToArray();
                content += BitConverter.ToString(preview).Replace("-", " ");
            }

            _previewTextBox.Text = content;
        }
        catch (Exception ex)
        {
            _previewTextBox.Text = $"Error reading content: {ex.Message}";
        }
    }
    
    private void ViewContent(string path)
    {
        if (_currentPack == null) return;

        try
        {
            var bytes = _currentPack.Read(path);
            if (bytes == null)
            {
                MessageBox.Show("Entry not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var metadata = _currentPack.GetEntryMetadata(path);
            string content;
            
            if (metadata?.ContentType == "json")
            {
                // Read as UTF-8 string first
                content = System.Text.Encoding.UTF8.GetString(bytes);
                
                // Decode Unicode escape sequences (\uXXXX) to actual characters
                content = DecodeUnicodeEscapes(content);
            }
            else if (metadata?.ContentType == "text")
            {
                content = System.Text.Encoding.UTF8.GetString(bytes);
            }
            else
            {
                content = $"[Binary Data - {bytes.Length} bytes]\n\nHex preview:\n";
                var preview = bytes.Take(Math.Min(256, bytes.Length)).ToArray();
                content += BitConverter.ToString(preview).Replace("-", " ");
            }

            using var viewer = new ContentViewerForm(Path.GetFileName(path), content);
            viewer.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error reading content: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    /// <summary>
    /// Decode Unicode escape sequences (\uXXXX) in a JSON string to actual characters
    /// </summary>
    private static string DecodeUnicodeEscapes(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new System.Text.StringBuilder(input.Length);
        int i = 0;
        
        while (i < input.Length)
        {
            // Check if this is a Unicode escape sequence
            if (i + 5 < input.Length && 
                input[i] == '\\' && 
                input[i + 1] == 'u' &&
                IsHexDigit(input[i + 2]) &&
                IsHexDigit(input[i + 3]) &&
                IsHexDigit(input[i + 4]) &&
                IsHexDigit(input[i + 5]))
            {
                // Extract hexadecimal value
                string hex = input.Substring(i + 2, 4);
                int codePoint = Convert.ToInt32(hex, 16);
                
                // Handle surrogate pairs
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
                            result.Append(char.ConvertFromUtf32(char.ConvertToUtf32(highSurrogate, lowSurrogate)));
                            i += 12;
                            continue;
                        }
                    }
                }
                
                // Regular character
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

    /// <summary>
    /// Check if a character is a hexadecimal digit
    /// </summary>
    private static bool IsHexDigit(char c)
    {
        return (c >= '0' && c <= '9') ||
               (c >= 'a' && c <= 'f') ||
               (c >= 'A' && c <= 'F');
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _currentPack?.Dispose();
        base.OnFormClosing(e);
    }
}

/// <summary>
/// Simple content viewer dialog
/// </summary>
internal sealed class ContentViewerForm : Form
{
    public ContentViewerForm(string title, string content)
    {
        Text = $"View: {title}";
        Size = new Size(800, 600);
        StartPosition = FormStartPosition.CenterParent;

        var textBox = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Both,
            Text = content,
            Font = new Font("Consolas", 10)
        };

        Controls.Add(textBox);
    }
}
