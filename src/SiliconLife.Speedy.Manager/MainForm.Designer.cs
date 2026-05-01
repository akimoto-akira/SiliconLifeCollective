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

namespace SiliconLife.Speedy.Manager;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        _currentPack?.Dispose();
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        _statusStrip = new StatusStrip();
        _statusLabel = new ToolStripStatusLabel();
        _itemCountLabel = new ToolStripStatusLabel();
        _breadcrumbPanel = new Panel();
        _breadcrumbFlow = new FlowLayoutPanel();
        _ribbonPanel = new Panel();
        _btnOpen = new Button();
        _lblFileGroup = new Label();
        _ribbonSeparator = new Panel();
        _btnRefresh = new Button();
        _btnCompact = new Button();
        _lblViewGroup = new Label();
        _contentPanel = new Panel();
        _mainSplitContainer = new SplitContainer();
        _treePanel = new Panel();
        _treeView = new TreeView();
        _treeImageList = new ImageList(components);
        _treeContextMenu = new ContextMenuStrip(components);
        _treeRefreshMenuItem = new ToolStripMenuItem();
        _rightSplitContainer = new SplitContainer();
        _listView = new ListView();
        _colName = new ColumnHeader();
        _colType = new ColumnHeader();
        _colSize = new ColumnHeader();
        _colModified = new ColumnHeader();
        _listContextMenu = new ContextMenuStrip(components);
        _listViewContentMenuItem = new ToolStripMenuItem();
        _listSeparator = new ToolStripSeparator();
        _listRefreshMenuItem = new ToolStripMenuItem();
        _previewPanel = new Panel();
        _previewTextBox = new TextBox();
        _statusStrip.SuspendLayout();
        _breadcrumbPanel.SuspendLayout();
        _ribbonPanel.SuspendLayout();
        _contentPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_mainSplitContainer).BeginInit();
        _mainSplitContainer.Panel1.SuspendLayout();
        _mainSplitContainer.Panel2.SuspendLayout();
        _mainSplitContainer.SuspendLayout();
        _treePanel.SuspendLayout();
        _treeContextMenu.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_rightSplitContainer).BeginInit();
        _rightSplitContainer.Panel1.SuspendLayout();
        _rightSplitContainer.Panel2.SuspendLayout();
        _rightSplitContainer.SuspendLayout();
        _listContextMenu.SuspendLayout();
        _previewPanel.SuspendLayout();
        SuspendLayout();
        // 
        // _statusStrip
        // 
        _statusStrip.BackColor = Color.White;
        _statusStrip.ImageScalingSize = new Size(32, 32);
        _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _itemCountLabel });
        _statusStrip.Location = new Point(0, 787);
        _statusStrip.Name = "_statusStrip";
        _statusStrip.Padding = new Padding(1, 0, 12, 0);
        _statusStrip.Size = new Size(1474, 42);
        _statusStrip.TabIndex = 2;
        // 
        // _statusLabel
        // 
        _statusLabel.Font = new Font("Segoe UI", 9F);
        _statusLabel.Name = "_statusLabel";
        _statusLabel.Size = new Size(1352, 32);
        _statusLabel.Spring = true;
        _statusLabel.Text = "就绪";
        _statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _itemCountLabel
        // 
        _itemCountLabel.Font = new Font("Segoe UI", 9F);
        _itemCountLabel.Name = "_itemCountLabel";
        _itemCountLabel.Size = new Size(109, 32);
        _itemCountLabel.Text = "0 个项目";
        // 
        // _breadcrumbPanel
        // 
        _breadcrumbPanel.BackColor = Color.White;
        _breadcrumbPanel.Controls.Add(_breadcrumbFlow);
        _breadcrumbPanel.Dock = DockStyle.Top;
        _breadcrumbPanel.Location = new Point(0, 123);
        _breadcrumbPanel.Name = "_breadcrumbPanel";
        _breadcrumbPanel.Padding = new Padding(8, 4, 8, 4);
        _breadcrumbPanel.Size = new Size(1474, 76);
        _breadcrumbPanel.TabIndex = 3;
        // 
        // _breadcrumbFlow
        // 
        _breadcrumbFlow.AutoScroll = true;
        _breadcrumbFlow.Dock = DockStyle.Fill;
        _breadcrumbFlow.Location = new Point(8, 4);
        _breadcrumbFlow.Name = "_breadcrumbFlow";
        _breadcrumbFlow.Size = new Size(1458, 68);
        _breadcrumbFlow.TabIndex = 0;
        _breadcrumbFlow.WrapContents = false;
        // 
        // _ribbonPanel
        // 
        _ribbonPanel.BackColor = Color.White;
        _ribbonPanel.Controls.Add(_btnOpen);
        _ribbonPanel.Controls.Add(_lblFileGroup);
        _ribbonPanel.Controls.Add(_ribbonSeparator);
        _ribbonPanel.Controls.Add(_btnRefresh);
        _ribbonPanel.Controls.Add(_btnCompact);
        _ribbonPanel.Controls.Add(_lblViewGroup);
        _ribbonPanel.Dock = DockStyle.Top;
        _ribbonPanel.Location = new Point(0, 0);
        _ribbonPanel.Name = "_ribbonPanel";
        _ribbonPanel.Padding = new Padding(12, 8, 12, 8);
        _ribbonPanel.Size = new Size(1474, 123);
        _ribbonPanel.TabIndex = 4;
        // 
        // _btnOpen
        // 
        _btnOpen.BackColor = Color.Transparent;
        _btnOpen.Cursor = Cursors.Hand;
        _btnOpen.FlatAppearance.BorderSize = 0;
        _btnOpen.FlatAppearance.MouseDownBackColor = Color.FromArgb(230, 230, 230);
        _btnOpen.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
        _btnOpen.FlatStyle = FlatStyle.Flat;
        _btnOpen.Font = new Font("Segoe UI", 9.5F);
        _btnOpen.ForeColor = Color.FromArgb(32, 32, 32);
        _btnOpen.Location = new Point(16, 12);
        _btnOpen.Name = "_btnOpen";
        _btnOpen.Size = new Size(146, 56);
        _btnOpen.TabIndex = 0;
        _btnOpen.Text = "📂 打开";
        _btnOpen.UseVisualStyleBackColor = false;
        // 
        // _lblFileGroup
        // 
        _lblFileGroup.Font = new Font("Segoe UI", 8F);
        _lblFileGroup.ForeColor = Color.FromArgb(96, 96, 96);
        _lblFileGroup.Location = new Point(16, 72);
        _lblFileGroup.Name = "_lblFileGroup";
        _lblFileGroup.Size = new Size(100, 31);
        _lblFileGroup.TabIndex = 1;
        _lblFileGroup.Text = "文件";
        _lblFileGroup.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _ribbonSeparator
        // 
        _ribbonSeparator.BackColor = Color.FromArgb(220, 220, 220);
        _ribbonSeparator.Location = new Point(161, 8);
        _ribbonSeparator.Name = "_ribbonSeparator";
        _ribbonSeparator.Size = new Size(1, 84);
        _ribbonSeparator.TabIndex = 2;
        // 
        // _btnRefresh
        // 
        _btnRefresh.BackColor = Color.Transparent;
        _btnRefresh.Cursor = Cursors.Hand;
        _btnRefresh.Enabled = false;
        _btnRefresh.FlatAppearance.BorderSize = 0;
        _btnRefresh.FlatAppearance.MouseDownBackColor = Color.FromArgb(230, 230, 230);
        _btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
        _btnRefresh.FlatStyle = FlatStyle.Flat;
        _btnRefresh.Font = new Font("Segoe UI", 9.5F);
        _btnRefresh.ForeColor = Color.FromArgb(32, 32, 32);
        _btnRefresh.Location = new Point(164, 12);
        _btnRefresh.Name = "_btnRefresh";
        _btnRefresh.Size = new Size(132, 56);
        _btnRefresh.TabIndex = 3;
        _btnRefresh.Text = "🔄刷新";
        _btnRefresh.UseVisualStyleBackColor = false;
        // 
        // _btnCompact
        // 
        _btnCompact.BackColor = Color.Transparent;
        _btnCompact.Cursor = Cursors.Hand;
        _btnCompact.Enabled = false;
        _btnCompact.FlatAppearance.BorderSize = 0;
        _btnCompact.FlatAppearance.MouseDownBackColor = Color.FromArgb(230, 230, 230);
        _btnCompact.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
        _btnCompact.FlatStyle = FlatStyle.Flat;
        _btnCompact.Font = new Font("Segoe UI", 9.5F);
        _btnCompact.ForeColor = Color.FromArgb(32, 32, 32);
        _btnCompact.Location = new Point(302, 12);
        _btnCompact.Name = "_btnCompact";
        _btnCompact.Size = new Size(148, 56);
        _btnCompact.TabIndex = 4;
        _btnCompact.Text = "🗜️压缩";
        _btnCompact.UseVisualStyleBackColor = false;
        // 
        // _lblViewGroup
        // 
        _lblViewGroup.Font = new Font("Segoe UI", 8F);
        _lblViewGroup.ForeColor = Color.FromArgb(96, 96, 96);
        _lblViewGroup.Location = new Point(236, 72);
        _lblViewGroup.Name = "_lblViewGroup";
        _lblViewGroup.Size = new Size(148, 31);
        _lblViewGroup.TabIndex = 5;
        _lblViewGroup.Text = "视图";
        _lblViewGroup.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _contentPanel
        // 
        _contentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _contentPanel.Controls.Add(_mainSplitContainer);
        _contentPanel.Location = new Point(0, 0);
        _contentPanel.Name = "_contentPanel";
        _contentPanel.Padding = new Padding(4);
        _contentPanel.Size = new Size(1474, 829);
        _contentPanel.TabIndex = 5;
        // 
        // _mainSplitContainer
        // 
        _mainSplitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _mainSplitContainer.BackColor = Color.FromArgb(230, 230, 230);
        _mainSplitContainer.Location = new Point(7, 201);
        _mainSplitContainer.Name = "_mainSplitContainer";
        // 
        // _mainSplitContainer.Panel1
        // 
        _mainSplitContainer.Panel1.Controls.Add(_treePanel);
        // 
        // _mainSplitContainer.Panel2
        // 
        _mainSplitContainer.Panel2.Controls.Add(_rightSplitContainer);
        _mainSplitContainer.Size = new Size(1455, 583);
        _mainSplitContainer.SplitterDistance = 235;
        _mainSplitContainer.TabIndex = 5;
        // 
        // _treePanel
        // 
        _treePanel.Controls.Add(_treeView);
        _treePanel.Dock = DockStyle.Fill;
        _treePanel.Location = new Point(0, 0);
        _treePanel.Name = "_treePanel";
        _treePanel.Padding = new Padding(0, 4, 4, 4);
        _treePanel.Size = new Size(235, 583);
        _treePanel.TabIndex = 0;
        // 
        // _treeView
        // 
        _treeView.BackColor = Color.FromArgb(248, 248, 248);
        _treeView.BorderStyle = BorderStyle.None;
        _treeView.ContextMenuStrip = _treeContextMenu;
        _treeView.Dock = DockStyle.Fill;
        _treeView.Font = new Font("Segoe UI", 9.5F);
        _treeView.ImageList = _treeImageList;
        _treeView.ItemHeight = 22;
        _treeView.LineColor = Color.FromArgb(160, 160, 160);
        _treeView.Location = new Point(0, 4);
        _treeView.Name = "_treeView";
        _treeView.ShowLines = true;
        _treeView.ShowPlusMinus = true;
        _treeView.ShowRootLines = true;
        _treeView.Size = new Size(231, 575);
        _treeView.TabIndex = 0;
        // 
        // _treeImageList
        // 
        _treeImageList.ColorDepth = ColorDepth.Depth32Bit;
        _treeImageList.ImageSize = new Size(16, 16);
        _treeImageList.TransparentColor = Color.Transparent;
        // 
        // _treeContextMenu
        // 
        _treeContextMenu.ImageScalingSize = new Size(32, 32);
        _treeContextMenu.Items.AddRange(new ToolStripItem[] { _treeRefreshMenuItem });
        _treeContextMenu.Name = "_treeContextMenu";
        _treeContextMenu.Size = new Size(137, 42);
        // 
        // _treeRefreshMenuItem
        // 
        _treeRefreshMenuItem.Name = "_treeRefreshMenuItem";
        _treeRefreshMenuItem.Size = new Size(136, 38);
        _treeRefreshMenuItem.Text = "刷新";
        // 
        // _rightSplitContainer
        // 
        _rightSplitContainer.BackColor = Color.FromArgb(230, 230, 230);
        _rightSplitContainer.Dock = DockStyle.Fill;
        _rightSplitContainer.Location = new Point(0, 0);
        _rightSplitContainer.Name = "_rightSplitContainer";
        // 
        // _rightSplitContainer.Panel1
        // 
        _rightSplitContainer.Panel1.Controls.Add(_listView);
        // 
        // _rightSplitContainer.Panel2
        // 
        _rightSplitContainer.Panel2.Controls.Add(_previewPanel);
        _rightSplitContainer.Size = new Size(1216, 583);
        _rightSplitContainer.SplitterDistance = 977;
        _rightSplitContainer.TabIndex = 0;
        // 
        // _listView
        // 
        _listView.AllowColumnReorder = true;
        _listView.BackColor = Color.White;
        _listView.BorderStyle = BorderStyle.None;
        _listView.Columns.AddRange(new ColumnHeader[] { _colName, _colType, _colSize, _colModified });
        _listView.ContextMenuStrip = _listContextMenu;
        _listView.Dock = DockStyle.Fill;
        _listView.Font = new Font("Segoe UI", 9.5F);
        _listView.FullRowSelect = true;
        _listView.Location = new Point(0, 0);
        _listView.Name = "_listView";
        _listView.Size = new Size(977, 583);
        _listView.TabIndex = 0;
        _listView.UseCompatibleStateImageBehavior = false;
        _listView.View = View.Details;
        // 
        // _colName
        // 
        _colName.Text = "名称";
        _colName.Width = 320;
        // 
        // _colType
        // 
        _colType.Text = "类型";
        _colType.Width = 120;
        // 
        // _colSize
        // 
        _colSize.Text = "大小";
        _colSize.Width = 100;
        _colSize.TextAlign = HorizontalAlignment.Right;
        // 
        // _colModified
        // 
        _colModified.Text = "修改时间";
        _colModified.Width = 180;
        // 
        // _listContextMenu
        // 
        _listContextMenu.ImageScalingSize = new Size(32, 32);
        _listContextMenu.Items.AddRange(new ToolStripItem[] { _listViewContentMenuItem, _listSeparator, _listRefreshMenuItem });
        _listContextMenu.Name = "_listContextMenu";
        _listContextMenu.Size = new Size(185, 86);
        // 
        // _listViewContentMenuItem
        // 
        _listViewContentMenuItem.Name = "_listViewContentMenuItem";
        _listViewContentMenuItem.Size = new Size(184, 38);
        _listViewContentMenuItem.Text = "查看内容";
        // 
        // _listSeparator
        // 
        _listSeparator.Name = "_listSeparator";
        _listSeparator.Size = new Size(181, 6);
        // 
        // _listRefreshMenuItem
        // 
        _listRefreshMenuItem.Name = "_listRefreshMenuItem";
        _listRefreshMenuItem.Size = new Size(184, 38);
        _listRefreshMenuItem.Text = "刷新";
        // 
        // _previewPanel
        // 
        _previewPanel.BackColor = Color.FromArgb(250, 250, 250);
        _previewPanel.Controls.Add(_previewTextBox);
        _previewPanel.Dock = DockStyle.Fill;
        _previewPanel.Location = new Point(0, 0);
        _previewPanel.Name = "_previewPanel";
        _previewPanel.Padding = new Padding(8);
        _previewPanel.Size = new Size(235, 583);
        _previewPanel.TabIndex = 0;
        // 
        // _previewTextBox
        // 
        _previewTextBox.BackColor = Color.FromArgb(250, 250, 250);
        _previewTextBox.BorderStyle = BorderStyle.None;
        _previewTextBox.Dock = DockStyle.Fill;
        _previewTextBox.Font = new Font("Consolas", 10F);
        _previewTextBox.Location = new Point(8, 8);
        _previewTextBox.Multiline = true;
        _previewTextBox.Name = "_previewTextBox";
        _previewTextBox.ReadOnly = true;
        _previewTextBox.ScrollBars = ScrollBars.Both;
        _previewTextBox.Size = new Size(219, 567);
        _previewTextBox.TabIndex = 0;
        _previewTextBox.Text = "选择一个文件以预览内容";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(192F, 192F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(251, 251, 251);
        ClientSize = new Size(1474, 829);
        Controls.Add(_statusStrip);
        Controls.Add(_breadcrumbPanel);
        Controls.Add(_ribbonPanel);
        Controls.Add(_contentPanel);
        MinimumSize = new Size(1100, 700);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Speedy Pack Manager";
        _statusStrip.ResumeLayout(false);
        _statusStrip.PerformLayout();
        _breadcrumbPanel.ResumeLayout(false);
        _ribbonPanel.ResumeLayout(false);
        _contentPanel.ResumeLayout(false);
        _mainSplitContainer.Panel1.ResumeLayout(false);
        _mainSplitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_mainSplitContainer).EndInit();
        _mainSplitContainer.ResumeLayout(false);
        _treePanel.ResumeLayout(false);
        _treeContextMenu.ResumeLayout(false);
        _rightSplitContainer.Panel1.ResumeLayout(false);
        _rightSplitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_rightSplitContainer).EndInit();
        _rightSplitContainer.ResumeLayout(false);
        _listContextMenu.ResumeLayout(false);
        _previewPanel.ResumeLayout(false);
        _previewPanel.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    // 状态栏
    private StatusStrip _statusStrip;
    private ToolStripStatusLabel _statusLabel;
    private ToolStripStatusLabel _itemCountLabel;
    
    // 面包屑导航
    private Panel _breadcrumbPanel;
    private FlowLayoutPanel _breadcrumbFlow;
    
    // Ribbon 工具栏
    private Panel _ribbonPanel;
    private Button _btnOpen;
    private Button _btnRefresh;
    private Button _btnCompact;
    private Label _lblFileGroup;
    private Label _lblViewGroup;
    private Panel _ribbonSeparator;
    
    // 主分割容器
    private Panel _contentPanel;  // 内容容器
    private SplitContainer _mainSplitContainer;
    private Panel _treePanel;
    private TreeView _treeView;
    private ImageList _treeImageList;
    private SplitContainer _rightSplitContainer;
    private ListView _listView;
        private ColumnHeader _colName;
        private ColumnHeader _colType;
        private ColumnHeader _colSize;
        private ColumnHeader _colModified;
    private Panel _previewPanel;
    private TextBox _previewTextBox;
    
    // 右键菜单
    private ContextMenuStrip _treeContextMenu;
    private ToolStripMenuItem _treeRefreshMenuItem;
    private ContextMenuStrip _listContextMenu;
    private ToolStripMenuItem _listViewContentMenuItem;
    private ToolStripSeparator _listSeparator;
    private ToolStripMenuItem _listRefreshMenuItem;
}
