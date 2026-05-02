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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        _statusStrip = new System.Windows.Forms.StatusStrip();
        _statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
        _itemCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
        _breadcrumbPanel = new System.Windows.Forms.Panel();
        _breadcrumbFlow = new System.Windows.Forms.FlowLayoutPanel();
        _ribbonPanel = new System.Windows.Forms.Panel();
        _btnOpen = new System.Windows.Forms.Button();
        _lblFileGroup = new System.Windows.Forms.Label();
        _ribbonSeparator = new System.Windows.Forms.Panel();
        _btnRefresh = new System.Windows.Forms.Button();
        _btnCompact = new System.Windows.Forms.Button();
        _lblViewGroup = new System.Windows.Forms.Label();
        _contentPanel = new System.Windows.Forms.Panel();
        _mainSplitContainer = new System.Windows.Forms.SplitContainer();
        _treePanel = new System.Windows.Forms.Panel();
        _treeView = new System.Windows.Forms.TreeView();
        _treeContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
        _treeRefreshMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        _treeImageList = new System.Windows.Forms.ImageList(components);
        _rightSplitContainer = new System.Windows.Forms.SplitContainer();
        _listView = new System.Windows.Forms.ListView();
        _colName = new System.Windows.Forms.ColumnHeader();
        _colType = new System.Windows.Forms.ColumnHeader();
        _colSize = new System.Windows.Forms.ColumnHeader();
        _colModified = new System.Windows.Forms.ColumnHeader();
        _listContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
        _listViewContentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        _listSeparator = new System.Windows.Forms.ToolStripSeparator();
        _listRefreshMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        _previewPanel = new System.Windows.Forms.Panel();
        _previewTextBox = new System.Windows.Forms.TextBox();
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
        _statusStrip.BackColor = System.Drawing.Color.White;
        _statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
        _statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _statusLabel, _itemCountLabel });
        _statusStrip.Location = new System.Drawing.Point(0, 787);
        _statusStrip.Name = "_statusStrip";
        _statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
        _statusStrip.Size = new System.Drawing.Size(1474, 42);
        _statusStrip.TabIndex = 2;
        // 
        // _statusLabel
        // 
        _statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
        _statusLabel.Name = "_statusLabel";
        _statusLabel.Size = new System.Drawing.Size(1369, 32);
        _statusLabel.Spring = true;
        _statusLabel.Text = "Ready";
        _statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // _itemCountLabel
        // 
        _itemCountLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
        _itemCountLabel.Name = "_itemCountLabel";
        _itemCountLabel.Size = new System.Drawing.Size(92, 32);
        _itemCountLabel.Text = "0 items";
        // 
        // _breadcrumbPanel
        // 
        _breadcrumbPanel.BackColor = System.Drawing.Color.White;
        _breadcrumbPanel.Controls.Add(_breadcrumbFlow);
        _breadcrumbPanel.Dock = System.Windows.Forms.DockStyle.Top;
        _breadcrumbPanel.Location = new System.Drawing.Point(0, 123);
        _breadcrumbPanel.Name = "_breadcrumbPanel";
        _breadcrumbPanel.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
        _breadcrumbPanel.Size = new System.Drawing.Size(1474, 76);
        _breadcrumbPanel.TabIndex = 3;
        // 
        // _breadcrumbFlow
        // 
        _breadcrumbFlow.AutoScroll = true;
        _breadcrumbFlow.Dock = System.Windows.Forms.DockStyle.Fill;
        _breadcrumbFlow.Location = new System.Drawing.Point(8, 4);
        _breadcrumbFlow.Name = "_breadcrumbFlow";
        _breadcrumbFlow.Size = new System.Drawing.Size(1458, 68);
        _breadcrumbFlow.TabIndex = 0;
        _breadcrumbFlow.WrapContents = false;
        // 
        // _ribbonPanel
        // 
        _ribbonPanel.BackColor = System.Drawing.Color.White;
        _ribbonPanel.Controls.Add(_btnOpen);
        _ribbonPanel.Controls.Add(_lblFileGroup);
        _ribbonPanel.Controls.Add(_ribbonSeparator);
        _ribbonPanel.Controls.Add(_btnRefresh);
        _ribbonPanel.Controls.Add(_btnCompact);
        _ribbonPanel.Controls.Add(_lblViewGroup);
        _ribbonPanel.Dock = System.Windows.Forms.DockStyle.Top;
        _ribbonPanel.Location = new System.Drawing.Point(0, 0);
        _ribbonPanel.Name = "_ribbonPanel";
        _ribbonPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
        _ribbonPanel.Size = new System.Drawing.Size(1474, 123);
        _ribbonPanel.TabIndex = 4;
        // 
        // _btnOpen
        // 
        _btnOpen.BackColor = System.Drawing.Color.Transparent;
        _btnOpen.Cursor = System.Windows.Forms.Cursors.Hand;
        _btnOpen.FlatAppearance.BorderSize = 0;
        _btnOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)((byte)230)), ((int)((byte)230)), ((int)((byte)230)));
        _btnOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)((byte)240)), ((int)((byte)240)), ((int)((byte)240)));
        _btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        _btnOpen.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        _btnOpen.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)32)), ((int)((byte)32)), ((int)((byte)32)));
        _btnOpen.Location = new System.Drawing.Point(16, 12);
        _btnOpen.Name = "_btnOpen";
        _btnOpen.Size = new System.Drawing.Size(146, 56);
        _btnOpen.TabIndex = 0;
        _btnOpen.Text = "📂 Open";
        _btnOpen.UseVisualStyleBackColor = false;
        // 
        // _lblFileGroup
        // 
        _lblFileGroup.Font = new System.Drawing.Font("Segoe UI", 8F);
        _lblFileGroup.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)96)), ((int)((byte)96)), ((int)((byte)96)));
        _lblFileGroup.Location = new System.Drawing.Point(16, 72);
        _lblFileGroup.Name = "_lblFileGroup";
        _lblFileGroup.Size = new System.Drawing.Size(100, 31);
        _lblFileGroup.TabIndex = 1;
        _lblFileGroup.Text = "File";
        _lblFileGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // _ribbonSeparator
        // 
        _ribbonSeparator.BackColor = System.Drawing.Color.FromArgb(((int)((byte)220)), ((int)((byte)220)), ((int)((byte)220)));
        _ribbonSeparator.Location = new System.Drawing.Point(161, 8);
        _ribbonSeparator.Name = "_ribbonSeparator";
        _ribbonSeparator.Size = new System.Drawing.Size(1, 84);
        _ribbonSeparator.TabIndex = 2;
        // 
        // _btnRefresh
        // 
        _btnRefresh.BackColor = System.Drawing.Color.Transparent;
        _btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
        _btnRefresh.Enabled = false;
        _btnRefresh.FlatAppearance.BorderSize = 0;
        _btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)((byte)230)), ((int)((byte)230)), ((int)((byte)230)));
        _btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)((byte)240)), ((int)((byte)240)), ((int)((byte)240)));
        _btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        _btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        _btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)32)), ((int)((byte)32)), ((int)((byte)32)));
        _btnRefresh.Location = new System.Drawing.Point(164, 12);
        _btnRefresh.Name = "_btnRefresh";
        _btnRefresh.Size = new System.Drawing.Size(132, 56);
        _btnRefresh.TabIndex = 3;
        _btnRefresh.Text = "🔄 Refresh";
        _btnRefresh.UseVisualStyleBackColor = false;
        // 
        // _btnCompact
        // 
        _btnCompact.BackColor = System.Drawing.Color.Transparent;
        _btnCompact.Cursor = System.Windows.Forms.Cursors.Hand;
        _btnCompact.Enabled = false;
        _btnCompact.FlatAppearance.BorderSize = 0;
        _btnCompact.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)((byte)230)), ((int)((byte)230)), ((int)((byte)230)));
        _btnCompact.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)((byte)240)), ((int)((byte)240)), ((int)((byte)240)));
        _btnCompact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        _btnCompact.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        _btnCompact.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)32)), ((int)((byte)32)), ((int)((byte)32)));
        _btnCompact.Location = new System.Drawing.Point(302, 12);
        _btnCompact.Name = "_btnCompact";
        _btnCompact.Size = new System.Drawing.Size(148, 56);
        _btnCompact.TabIndex = 4;
        _btnCompact.Text = "🗜️ Compact";
        _btnCompact.UseVisualStyleBackColor = false;
        // 
        // _lblViewGroup
        // 
        _lblViewGroup.Font = new System.Drawing.Font("Segoe UI", 8F);
        _lblViewGroup.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)96)), ((int)((byte)96)), ((int)((byte)96)));
        _lblViewGroup.Location = new System.Drawing.Point(236, 72);
        _lblViewGroup.Name = "_lblViewGroup";
        _lblViewGroup.Size = new System.Drawing.Size(148, 31);
        _lblViewGroup.TabIndex = 5;
        _lblViewGroup.Text = "View";
        _lblViewGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // _contentPanel
        // 
        _contentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
        _contentPanel.Controls.Add(_mainSplitContainer);
        _contentPanel.Location = new System.Drawing.Point(0, 0);
        _contentPanel.Name = "_contentPanel";
        _contentPanel.Padding = new System.Windows.Forms.Padding(4);
        _contentPanel.Size = new System.Drawing.Size(1474, 829);
        _contentPanel.TabIndex = 5;
        // 
        // _mainSplitContainer
        // 
        _mainSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
        _mainSplitContainer.BackColor = System.Drawing.Color.FromArgb(((int)((byte)230)), ((int)((byte)230)), ((int)((byte)230)));
        _mainSplitContainer.Location = new System.Drawing.Point(7, 201);
        _mainSplitContainer.Name = "_mainSplitContainer";
        // 
        // _mainSplitContainer.Panel1
        // 
        _mainSplitContainer.Panel1.Controls.Add(_treePanel);
        // 
        // _mainSplitContainer.Panel2
        // 
        _mainSplitContainer.Panel2.Controls.Add(_rightSplitContainer);
        _mainSplitContainer.Size = new System.Drawing.Size(1455, 583);
        _mainSplitContainer.SplitterDistance = 235;
        _mainSplitContainer.TabIndex = 5;
        // 
        // _treePanel
        // 
        _treePanel.Controls.Add(_treeView);
        _treePanel.Dock = System.Windows.Forms.DockStyle.Fill;
        _treePanel.Location = new System.Drawing.Point(0, 0);
        _treePanel.Name = "_treePanel";
        _treePanel.Padding = new System.Windows.Forms.Padding(0, 4, 4, 4);
        _treePanel.Size = new System.Drawing.Size(235, 583);
        _treePanel.TabIndex = 0;
        // 
        // _treeView
        // 
        _treeView.BackColor = System.Drawing.Color.FromArgb(((int)((byte)248)), ((int)((byte)248)), ((int)((byte)248)));
        _treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
        _treeView.ContextMenuStrip = _treeContextMenu;
        _treeView.Dock = System.Windows.Forms.DockStyle.Fill;
        _treeView.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        _treeView.ImageIndex = 0;
        _treeView.ImageList = _treeImageList;
        _treeView.ItemHeight = 22;
        _treeView.LineColor = System.Drawing.Color.FromArgb(((int)((byte)160)), ((int)((byte)160)), ((int)((byte)160)));
        _treeView.Location = new System.Drawing.Point(0, 4);
        _treeView.Name = "_treeView";
        _treeView.SelectedImageIndex = 0;
        _treeView.Size = new System.Drawing.Size(231, 575);
        _treeView.TabIndex = 0;
        // 
        // _treeContextMenu
        // 
        _treeContextMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
        _treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _treeRefreshMenuItem });
        _treeContextMenu.Name = "_treeContextMenu";
        _treeContextMenu.Size = new System.Drawing.Size(176, 42);
        // 
        // _treeRefreshMenuItem
        // 
        _treeRefreshMenuItem.Name = "_treeRefreshMenuItem";
        _treeRefreshMenuItem.Size = new System.Drawing.Size(175, 38);
        _treeRefreshMenuItem.Text = "Refresh";
        // 
        // _treeImageList
        // 
        _treeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
        _treeImageList.ImageSize = new System.Drawing.Size(16, 16);
        _treeImageList.TransparentColor = System.Drawing.Color.Transparent;
        // 
        // _rightSplitContainer
        // 
        _rightSplitContainer.BackColor = System.Drawing.Color.FromArgb(((int)((byte)230)), ((int)((byte)230)), ((int)((byte)230)));
        _rightSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
        _rightSplitContainer.Location = new System.Drawing.Point(0, 0);
        _rightSplitContainer.Name = "_rightSplitContainer";
        // 
        // _rightSplitContainer.Panel1
        // 
        _rightSplitContainer.Panel1.Controls.Add(_listView);
        // 
        // _rightSplitContainer.Panel2
        // 
        _rightSplitContainer.Panel2.Controls.Add(_previewPanel);
        _rightSplitContainer.Size = new System.Drawing.Size(1216, 583);
        _rightSplitContainer.SplitterDistance = 977;
        _rightSplitContainer.TabIndex = 0;
        // 
        // _listView
        // 
        _listView.AllowColumnReorder = true;
        _listView.BackColor = System.Drawing.Color.White;
        _listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
        _listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { _colName, _colType, _colSize, _colModified });
        _listView.ContextMenuStrip = _listContextMenu;
        _listView.Dock = System.Windows.Forms.DockStyle.Fill;
        _listView.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        _listView.FullRowSelect = true;
        _listView.Location = new System.Drawing.Point(0, 0);
        _listView.Name = "_listView";
        _listView.Size = new System.Drawing.Size(977, 583);
        _listView.TabIndex = 0;
        _listView.UseCompatibleStateImageBehavior = false;
        _listView.View = System.Windows.Forms.View.Details;
        // 
        // _colName
        // 
        _colName.Name = "_colName";
        _colName.Text = "Name";
        _colName.Width = 320;
        // 
        // _colType
        // 
        _colType.Name = "_colType";
        _colType.Text = "Type";
        _colType.Width = 120;
        // 
        // _colSize
        // 
        _colSize.Name = "_colSize";
        _colSize.Text = "Size";
        _colSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        _colSize.Width = 100;
        // 
        // _colModified
        // 
        _colModified.Name = "_colModified";
        _colModified.Text = "Modified";
        _colModified.Width = 180;
        // 
        // _listContextMenu
        // 
        _listContextMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
        _listContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _listViewContentMenuItem, _listSeparator, _listRefreshMenuItem });
        _listContextMenu.Name = "_listContextMenu";
        _listContextMenu.Size = new System.Drawing.Size(244, 86);
        // 
        // _listViewContentMenuItem
        // 
        _listViewContentMenuItem.Name = "_listViewContentMenuItem";
        _listViewContentMenuItem.Size = new System.Drawing.Size(243, 38);
        _listViewContentMenuItem.Text = "View Content";
        // 
        // _listSeparator
        // 
        _listSeparator.Name = "_listSeparator";
        _listSeparator.Size = new System.Drawing.Size(240, 6);
        // 
        // _listRefreshMenuItem
        // 
        _listRefreshMenuItem.Name = "_listRefreshMenuItem";
        _listRefreshMenuItem.Size = new System.Drawing.Size(243, 38);
        _listRefreshMenuItem.Text = "Refresh";
        // 
        // _previewPanel
        // 
        _previewPanel.BackColor = System.Drawing.Color.FromArgb(((int)((byte)250)), ((int)((byte)250)), ((int)((byte)250)));
        _previewPanel.Controls.Add(_previewTextBox);
        _previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        _previewPanel.Location = new System.Drawing.Point(0, 0);
        _previewPanel.Name = "_previewPanel";
        _previewPanel.Padding = new System.Windows.Forms.Padding(8);
        _previewPanel.Size = new System.Drawing.Size(235, 583);
        _previewPanel.TabIndex = 0;
        // 
        // _previewTextBox
        // 
        _previewTextBox.BackColor = System.Drawing.Color.FromArgb(((int)((byte)250)), ((int)((byte)250)), ((int)((byte)250)));
        _previewTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
        _previewTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
        _previewTextBox.Font = new System.Drawing.Font("Consolas", 10F);
        _previewTextBox.Location = new System.Drawing.Point(8, 8);
        _previewTextBox.Multiline = true;
        _previewTextBox.Name = "_previewTextBox";
        _previewTextBox.ReadOnly = true;
        _previewTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        _previewTextBox.Size = new System.Drawing.Size(219, 567);
        _previewTextBox.TabIndex = 0;
        _previewTextBox.Text = "Select a file to preview content";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)251)), ((int)((byte)251)), ((int)((byte)251)));
        ClientSize = new System.Drawing.Size(1474, 829);
        Controls.Add(_statusStrip);
        Controls.Add(_breadcrumbPanel);
        Controls.Add(_ribbonPanel);
        Controls.Add(_contentPanel);
        Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
        MinimumSize = new System.Drawing.Size(1100, 700);
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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

    // Status bar
    private StatusStrip _statusStrip;
    private ToolStripStatusLabel _statusLabel;
    private ToolStripStatusLabel _itemCountLabel;
    
    // Breadcrumb navigation
    private Panel _breadcrumbPanel;
    private FlowLayoutPanel _breadcrumbFlow;
    
    // Ribbon toolbar
    private Panel _ribbonPanel;
    private Button _btnOpen;
    private Button _btnRefresh;
    private Button _btnCompact;
    private Label _lblFileGroup;
    private Label _lblViewGroup;
    private Panel _ribbonSeparator;
    
    // Main split container
    private Panel _contentPanel;  // Content container
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
    
    // Context menus
    private ContextMenuStrip _treeContextMenu;
    private ToolStripMenuItem _treeRefreshMenuItem;
    private ContextMenuStrip _listContextMenu;
    private ToolStripMenuItem _listViewContentMenuItem;
    private ToolStripSeparator _listSeparator;
    private ToolStripMenuItem _listRefreshMenuItem;
}
