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
            _statusLabel.Text = $"内部调起";

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
            Filter = "Speedy Pack 文件 (*.spk)|*.spk|所有文件 (*.*)|*.*",
            Title = "打开 Speedy Pack 文件"
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
            _statusLabel.Text = "正在压缩...";
            await _currentPack.CompactAsync();
            _statusLabel.Text = "压缩完成";
            RefreshView();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"压缩失败: {ex.Message}", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            _statusLabel.Text = "压缩失败";
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

            // 初始化文件夹图标
            SetupTreeIcons();

            var fileName = Path.GetFileName(filePath);
            var fileInfo = new FileInfo(filePath);
            var sizeStr = FormatSize((int)fileInfo.Length);

            Text = $"Speedy Pack Manager - {fileName}";
            UpdateBreadcrumb();
            _statusLabel.Text = $"已加载: {fileName} ({sizeStr})";
            
            _btnRefresh.Enabled = true;
            _btnCompact.Enabled = true;

            RefreshView();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"打开文件失败: {ex.Message}", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            _statusLabel.Text = "打开文件失败";
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
                Text = "未打开文件",
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
    /// 创建文件夹图标并加载到 ImageList
    /// </summary>
    private void SetupTreeIcons()
    {
        // 创建 16x16 的文件夹图标
        var folderIcon = new Bitmap(16, 16);
        using (var g = Graphics.FromImage(folderIcon))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            // 文件夹主体（黄色）
            var bodyBrush = new SolidBrush(Color.FromArgb(255, 204, 102));
            var folderBody = new RectangleF(0, 4, 16, 11);
            g.FillRectangle(bodyBrush, folderBody);
            
            // 文件夹顶部（深黄色）
            var topBrush = new SolidBrush(Color.FromArgb(240, 180, 80));
            var folderTop = new RectangleF(2, 1, 12, 4);
            g.FillRectangle(topBrush, folderTop);
            
            // 标签凸起
            var tabBrush = new SolidBrush(Color.FromArgb(240, 180, 80));
            var tab = new RectangleF(2, 1, 6, 3);
            g.FillRectangle(tabBrush, tab);
            
            bodyBrush.Dispose();
            topBrush.Dispose();
            tabBrush.Dispose();
        }
        
        _treeImageList.Images.Add("folder", folderIcon);
        
        // 设置节点使用文件夹图标
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
            
            // 递归收集所有目录路径（BFS）
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
                // 提取最后一级目录名
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

            _itemCountLabel.Text = $"{_listView.Items.Count} 个项目";
        }
        finally
        {
            _listView.EndUpdate();
        }
    }



    private void TreeView_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (_currentPack == null || e.Node == null) return;

        // Root 节点不应该有点击反应
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
                _previewTextBox.Text = "选择一个文件以预览内容";
            }
        }
        else
        {
            _previewTextBox.Text = "选择一个文件以预览内容";
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
                _previewTextBox.Text = "无法读取内容";
                return;
            }

            var metadata = _currentPack.GetEntryMetadata(path);
            string content;
            
            if (metadata?.ContentType == "json" || metadata?.ContentType == "text")
            {
                content = System.Text.Encoding.UTF8.GetString(bytes);
                // Limit preview to 2000 characters
                if (content.Length > 2000)
                {
                    content = content.Substring(0, 2000) + "\n\n... [预览已截断]";
                }
            }
            else
            {
                content = $"[二进制数据 - {bytes.Length} 字节]\n\n十六进制预览:\n";
                var preview = bytes.Take(Math.Min(256, bytes.Length)).ToArray();
                content += BitConverter.ToString(preview).Replace("-", " ");
            }

            _previewTextBox.Text = content;
        }
        catch (Exception ex)
        {
            _previewTextBox.Text = $"读取内容时出错: {ex.Message}";
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
            
            if (metadata?.ContentType == "json" || metadata?.ContentType == "text")
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
