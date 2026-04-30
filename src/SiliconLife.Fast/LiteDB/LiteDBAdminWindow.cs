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

using System.Drawing;
using System.Windows.Forms;
using LiteDB;

namespace SiliconLife.Fast.LiteDB;

/// <summary>
/// WinForms administration window for LiteDB: browse collections, view documents,
/// and perform CRUD operations on both collections and documents.
/// All data access is routed through <see cref="LiteDBManager"/> so the window
/// has no direct dependency on the underlying <c>LiteDatabase</c> instance.
/// </summary>
public class LiteDBAdminWindow : Form
{
    private const int PageSize = 200;

    private readonly LiteDBAdminLocalization _l10n;

    private ListBox _collectionList = null!;
    private DataGridView _documentGrid = null!;
    private StatusStrip _statusBar = null!;
    private ToolStripStatusLabel _statusLabel = null!;
    private Button _btnRefreshCollections = null!;
    private Button _btnNewCollection = null!;
    private Button _btnDropCollection = null!;
    private Button _btnRenameCollection = null!;
    private Button _btnAddDoc = null!;
    private Button _btnEditDoc = null!;
    private Button _btnDeleteDoc = null!;
    private Button _btnRefreshDocs = null!;
    private Label _lblCollectionInfo = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteDBAdminWindow"/> class.
    /// </summary>
    public LiteDBAdminWindow(LiteDBAdminLocalization? localization = null)
    {
        _l10n = localization ?? new LiteDBAdminLocalizationEnUS();
        InitializeWindow();
        InitializeComponents();
        ReloadCollections();
    }

    private void InitializeWindow()
    {
        Text = _l10n.WindowTitle;
        Size = new Size(1000, 640);
        MinimumSize = new Size(720, 480);
        StartPosition = FormStartPosition.CenterScreen;
        try
        {
            Icon = new Icon(new MemoryStream(SiliconLife.Common.icons.slc));
        }
        catch
        {
            // Icon resource may be unavailable in some build variants; ignore.
        }
    }

    private void InitializeComponents()
    {
        // ---- Split container ----
        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterDistance = 240,
            FixedPanel = FixedPanel.Panel1
        };

        // ---- Left: collection toolbar + list ----
        var leftLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        leftLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var leftToolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(4)
        };

        _btnRefreshCollections = CreateButton(_l10n.Refresh, (s, e) => ReloadCollections());
        _btnNewCollection = CreateButton(_l10n.NewCollection, (s, e) => OnNewCollection());
        _btnDropCollection = CreateButton(_l10n.DropCollection, (s, e) => OnDropCollection());
        _btnRenameCollection = CreateButton(_l10n.RenameCollection, (s, e) => OnRenameCollection());
        leftToolbar.Controls.Add(_btnRefreshCollections);
        leftToolbar.Controls.Add(_btnNewCollection);
        leftToolbar.Controls.Add(_btnDropCollection);
        leftToolbar.Controls.Add(_btnRenameCollection);

        _collectionList = new ListBox
        {
            Dock = DockStyle.Fill,
            IntegralHeight = false
        };
        _collectionList.SelectedIndexChanged += (s, e) => ReloadDocuments();

        leftLayout.Controls.Add(leftToolbar, 0, 0);
        leftLayout.Controls.Add(_collectionList, 0, 1);
        split.Panel1.Controls.Add(leftLayout);

        // ---- Right: document toolbar + grid ----
        var rightLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3
        };
        rightLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rightLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var rightToolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(4)
        };
        _btnRefreshDocs = CreateButton(_l10n.Refresh, (s, e) => ReloadDocuments());
        _btnAddDoc = CreateButton(_l10n.AddDocument, (s, e) => OnAddDocument());
        _btnEditDoc = CreateButton(_l10n.EditDocument, (s, e) => OnEditDocument());
        _btnDeleteDoc = CreateButton(_l10n.DeleteDocument, (s, e) => OnDeleteDocument());
        rightToolbar.Controls.Add(_btnRefreshDocs);
        rightToolbar.Controls.Add(_btnAddDoc);
        rightToolbar.Controls.Add(_btnEditDoc);
        rightToolbar.Controls.Add(_btnDeleteDoc);

        _lblCollectionInfo = new Label
        {
            Dock = DockStyle.Fill,
            AutoSize = false,
            Height = 22,
            Padding = new Padding(6, 4, 4, 0),
            Text = _l10n.NoCollectionSelected
        };

        _documentGrid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible = false
        };
        _documentGrid.CellDoubleClick += (s, e) =>
        {
            if (e.RowIndex >= 0) OnEditDocument();
        };

        rightLayout.Controls.Add(rightToolbar, 0, 0);
        rightLayout.Controls.Add(_lblCollectionInfo, 0, 1);
        rightLayout.Controls.Add(_documentGrid, 0, 2);
        split.Panel2.Controls.Add(rightLayout);

        Controls.Add(split);

        // ---- Status bar ----
        _statusLabel = new ToolStripStatusLabel { Text = _l10n.Ready };
        _statusBar = new StatusStrip();
        _statusBar.Items.Add(_statusLabel);
        Controls.Add(_statusBar);
    }

    private static Button CreateButton(string text, EventHandler click)
    {
        var btn = new Button
        {
            Text = text,
            AutoSize = true,
            Margin = new Padding(2)
        };
        btn.Click += click;
        return btn;
    }

    // ==================== Collection operations ====================

    private void ReloadCollections()
    {
        try
        {
            string? previous = _collectionList.SelectedItem as string;
            _collectionList.BeginUpdate();
            _collectionList.Items.Clear();
            foreach (string name in LiteDBManager.GetCollectionNames())
            {
                _collectionList.Items.Add(name);
            }
            _collectionList.EndUpdate();

            if (previous != null && _collectionList.Items.Contains(previous))
            {
                _collectionList.SelectedItem = previous;
            }
            else if (_collectionList.Items.Count > 0)
            {
                _collectionList.SelectedIndex = 0;
            }
            else
            {
                _documentGrid.DataSource = null;
                _documentGrid.Columns.Clear();
                _lblCollectionInfo.Text = _l10n.NoCollectionSelected;
            }
            SetStatus(string.Format(_l10n.StatusCollectionsLoaded, _collectionList.Items.Count));
        }
        catch (Exception ex)
        {
            ShowError(_l10n.ErrorLoadCollections, ex);
        }
    }

    private void OnNewCollection()
    {
        string? name = PromptForText(_l10n.NewCollection, _l10n.PromptCollectionName, string.Empty);
        if (string.IsNullOrWhiteSpace(name)) return;

        try
        {
            if (LiteDBManager.CollectionExists(name))
            {
                MessageBox.Show(this, _l10n.ErrorCollectionExists, _l10n.WindowTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LiteDBManager.CreateCollection(name);
            ReloadCollections();
            _collectionList.SelectedItem = name;
            SetStatus(string.Format(_l10n.StatusCollectionCreated, name));
        }
        catch (Exception ex)
        {
            ShowError(_l10n.ErrorCreateCollection, ex);
        }
    }

    private void OnDropCollection()
    {
        string? name = _collectionList.SelectedItem as string;
        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show(this, _l10n.NoCollectionSelected, _l10n.WindowTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show(this,
            string.Format(_l10n.ConfirmDropCollection, name),
            _l10n.WindowTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (result != DialogResult.Yes) return;

        try
        {
            LiteDBManager.DropCollection(name);
            ReloadCollections();
            SetStatus(string.Format(_l10n.StatusCollectionDropped, name));
        }
        catch (Exception ex)
        {
            ShowError(_l10n.ErrorDropCollection, ex);
        }
    }

    private void OnRenameCollection()
    {
        string? oldName = _collectionList.SelectedItem as string;
        if (string.IsNullOrEmpty(oldName))
        {
            MessageBox.Show(this, _l10n.NoCollectionSelected, _l10n.WindowTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        string? newName = PromptForText(_l10n.RenameCollection, _l10n.PromptCollectionNewName, oldName);
        if (string.IsNullOrWhiteSpace(newName) || newName == oldName) return;

        try
        {
            if (!LiteDBManager.RenameCollection(oldName, newName))
            {
                MessageBox.Show(this, _l10n.ErrorRenameCollection, _l10n.WindowTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ReloadCollections();
            _collectionList.SelectedItem = newName;
            SetStatus(string.Format(_l10n.StatusCollectionRenamed, oldName, newName));
        }
        catch (Exception ex)
        {
            ShowError(_l10n.ErrorRenameCollection, ex);
        }
    }

    // ==================== Document operations ====================

    private void ReloadDocuments()
    {
        string? name = _collectionList.SelectedItem as string;
        _documentGrid.DataSource = null;
        _documentGrid.Columns.Clear();

        if (string.IsNullOrEmpty(name))
        {
            _lblCollectionInfo.Text = _l10n.NoCollectionSelected;
            return;
        }

        try
        {
            int total = LiteDBManager.GetCollectionCount(name);
            var docs = LiteDBManager.GetDocuments(name, 0, PageSize);

            _lblCollectionInfo.Text = string.Format(
                _l10n.CollectionInfoFormat, name, total, Math.Min(docs.Count, PageSize));

            _documentGrid.Columns.Clear();
            _documentGrid.Columns.Add("_id", "_id");
            _documentGrid.Columns.Add("json", _l10n.ColumnJson);
            _documentGrid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            _documentGrid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            foreach (var doc in docs)
            {
                string idText = doc.ContainsKey("_id") ? doc["_id"].ToString() : "(null)";
                string json = JsonSerializer.Serialize(doc);
                _documentGrid.Rows.Add(idText, json);
            }

            SetStatus(string.Format(_l10n.StatusDocumentsLoaded, docs.Count, name));
        }
        catch (Exception ex)
        {
            ShowError(_l10n.ErrorLoadDocuments, ex);
        }
    }

    private void OnAddDocument()
    {
        string? name = _collectionList.SelectedItem as string;
        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show(this, _l10n.NoCollectionSelected, _l10n.WindowTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        string template = "{\n    \n}";
        string? json = PromptForJson(_l10n.AddDocument, _l10n.PromptDocumentJson, template);
        if (string.IsNullOrWhiteSpace(json)) return;

        try
        {
            var doc = ParseBsonDocument(json);
            LiteDBManager.InsertDocument(name, doc);
            ReloadDocuments();
            SetStatus(_l10n.StatusDocumentInserted);
        }
        catch (Exception ex)
        {
            ShowError(_l10n.ErrorInsertDocument, ex);
        }
    }

    private void OnEditDocument()
    {
        string? name = _collectionList.SelectedItem as string;
        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show(this, _l10n.NoCollectionSelected, _l10n.WindowTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (_documentGrid.CurrentRow == null)
        {
            MessageBox.Show(this, _l10n.NoDocumentSelected, _l10n.WindowTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        string currentJson = _documentGrid.CurrentRow.Cells["json"].Value?.ToString() ?? "{}";
        string? edited = PromptForJson(_l10n.EditDocument, _l10n.PromptDocumentJson, currentJson);
        if (string.IsNullOrWhiteSpace(edited)) return;

        try
        {
            var doc = ParseBsonDocument(edited);
            if (!doc.ContainsKey("_id") || doc["_id"].IsNull)
            {
                MessageBox.Show(this, _l10n.ErrorMissingId, _l10n.WindowTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!LiteDBManager.UpdateDocument(name, doc))
            {
                MessageBox.Show(this, _l10n.ErrorUpdateDocument, _l10n.WindowTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ReloadDocuments();
            SetStatus(_l10n.StatusDocumentUpdated);
        }
        catch (Exception ex)
        {
            ShowError(_l10n.ErrorUpdateDocument, ex);
        }
    }

    private void OnDeleteDocument()
    {
        string? name = _collectionList.SelectedItem as string;
        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show(this, _l10n.NoCollectionSelected, _l10n.WindowTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (_documentGrid.CurrentRow == null)
        {
            MessageBox.Show(this, _l10n.NoDocumentSelected, _l10n.WindowTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        string currentJson = _documentGrid.CurrentRow.Cells["json"].Value?.ToString() ?? "{}";
        BsonDocument doc;
        try
        {
            doc = ParseBsonDocument(currentJson);
        }
        catch (Exception ex)
        {
            ShowError(_l10n.ErrorDeleteDocument, ex);
            return;
        }
        if (!doc.ContainsKey("_id") || doc["_id"].IsNull)
        {
            MessageBox.Show(this, _l10n.ErrorMissingId, _l10n.WindowTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = MessageBox.Show(this,
            string.Format(_l10n.ConfirmDeleteDocument, doc["_id"]),
            _l10n.WindowTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (result != DialogResult.Yes) return;

        try
        {
            if (!LiteDBManager.DeleteDocument(name, doc["_id"]))
            {
                MessageBox.Show(this, _l10n.ErrorDeleteDocument, _l10n.WindowTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ReloadDocuments();
            SetStatus(_l10n.StatusDocumentDeleted);
        }
        catch (Exception ex)
        {
            ShowError(_l10n.ErrorDeleteDocument, ex);
        }
    }

    // ==================== Helpers ====================

    private static BsonDocument ParseBsonDocument(string json)
    {
        var value = JsonSerializer.Deserialize(json);
        if (value is not BsonDocument doc)
            throw new FormatException("Input is not a JSON object");
        return doc;
    }

    private void SetStatus(string text)
    {
        _statusLabel.Text = text;
    }

    private void ShowError(string title, Exception ex)
    {
        SetStatus(ex.Message);
        MessageBox.Show(this, $"{title}{Environment.NewLine}{ex.Message}", _l10n.WindowTitle,
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private string? PromptForText(string title, string prompt, string defaultValue)
    {
        using var dialog = new TextPromptDialog(title, prompt, defaultValue, multiline: false);
        return dialog.ShowDialog(this) == DialogResult.OK ? dialog.Value : null;
    }

    private string? PromptForJson(string title, string prompt, string defaultValue)
    {
        using var dialog = new TextPromptDialog(title, prompt, defaultValue, multiline: true);
        return dialog.ShowDialog(this) == DialogResult.OK ? dialog.Value : null;
    }

    // ==================== Nested prompt dialog ====================

    private sealed class TextPromptDialog : Form
    {
        private readonly TextBox _textBox;

        public string Value => _textBox.Text;

        public TextPromptDialog(string title, string prompt, string defaultValue, bool multiline)
        {
            Text = title;
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = multiline;
            Size = multiline ? new Size(640, 420) : new Size(480, 180);
            MinimumSize = new Size(360, 140);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var lbl = new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                Text = prompt
            };
            _textBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = multiline,
                ScrollBars = multiline ? ScrollBars.Both : ScrollBars.None,
                AcceptsReturn = multiline,
                AcceptsTab = multiline,
                Font = multiline ? new Font("Consolas", 9) : SystemFonts.DefaultFont,
                Text = defaultValue
            };
            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true
            };
            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true };
            var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, AutoSize = true };
            buttons.Controls.Add(ok);
            buttons.Controls.Add(cancel);

            layout.Controls.Add(lbl, 0, 0);
            layout.Controls.Add(_textBox, 0, 1);
            layout.Controls.Add(buttons, 0, 2);
            Controls.Add(layout);

            AcceptButton = multiline ? null : ok;
            CancelButton = cancel;
        }
    }
}
