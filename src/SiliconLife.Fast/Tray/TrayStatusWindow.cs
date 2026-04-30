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

using SiliconLife.Collective;
using SiliconLife.Fast.LiteDB;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace SiliconLife.Fast.Tray;

/// <summary>
/// Custom tray status popup window with tray icon management
/// Displays detailed application status information
/// </summary>
public class TrayStatusWindow : Form
{
    private readonly TrayLocalizationBase _localization;
    private readonly int _webPort;
    private readonly DateTime _startTime;
    private readonly System.Timers.Timer _updateTimer;
    private bool _isWindowShowing;
    private Panel _mainPanel = null!;
    private Label _lblTitle = null!;
    private Label _lblStatus = null!;
    private Label _lblUptime = null!;
    private Label _lblBeings = null!;
    private Label _lblBeingName = null!;
    private Label _lblAIModel = null!;
    private Label _lblMemory = null!;
    private Label _lblCPU = null!;
    private Label _lblWeb = null!;
    private NotifyIcon notifyIcon1;
    private System.ComponentModel.IContainer components;
    private ContextMenuStrip contextMenuStrip1;
    private Label _lblHint = null!;

    /// <summary>
    /// Event triggered when exit is requested from tray menu
    /// </summary>
    public event EventHandler? ExitRequested;

    /// <summary>
    /// Gets the current application status
    /// </summary>
    public string ApplicationStatus { get; private set; } = "Running";

    /// <summary>
    /// Initializes a new instance of the TrayStatusWindow
    /// </summary>
    public TrayStatusWindow(TrayLocalizationBase localization, int webPort)
    {
        _localization = localization;
        _webPort = webPort;
        _startTime = DateTime.Now;
        InitializeWindow();
        InitializeComponent();

        // Initialize context menu
        SetupContextMenu();
        
        // Start update timer
        _updateTimer = new System.Timers.Timer(1000);
        _updateTimer.Elapsed += OnTimerTick;
        _updateTimer.Start();
    }

    /// <summary>
    /// Sets up the context menu items
    /// </summary>
    private void SetupContextMenu()
    {
        contextMenuStrip1.Items.Clear();

        var openWebItem = new ToolStripMenuItem(_localization.OpenWebInterface);
        openWebItem.Click += (s, e) => OpenWebInterface();
        contextMenuStrip1.Items.Add(openWebItem);

        var dashboardItem = new ToolStripMenuItem(_localization.Dashboard);
        dashboardItem.Click += (s, e) => OpenDashboard();
        contextMenuStrip1.Items.Add(dashboardItem);

        var manageBeingsItem = new ToolStripMenuItem(_localization.ManageSiliconBeings);
        manageBeingsItem.Click += (s, e) => ManageBeings();
        contextMenuStrip1.Items.Add(manageBeingsItem);

        var configItem = new ToolStripMenuItem(_localization.Configuration);
        configItem.Click += (s, e) => OpenConfiguration();
        contextMenuStrip1.Items.Add(configItem);

        var liteDbItem = new ToolStripMenuItem(_localization.LiteDBManagement);
        liteDbItem.Click += (s, e) => OpenLiteDBManagement();
        contextMenuStrip1.Items.Add(liteDbItem);

        contextMenuStrip1.Items.Add(new ToolStripSeparator());

        var exitItem = new ToolStripMenuItem(_localization.Exit);
        exitItem.Click += (s, e) => RequestExit();
        contextMenuStrip1.Items.Add(exitItem);
    }

    /// <summary>
    /// Handles double-click on tray icon
    /// </summary>
    private void OnTrayIconDoubleClick(object? sender, MouseEventArgs e)
    {
        OpenWebInterface();
    }

    /// <summary>
    /// Handles mouse move over tray icon - show status window
    /// </summary>
    private void OnTrayIconMouseMove(object? sender, MouseEventArgs e)
    {
        if (!_isWindowShowing && !IsDisposed)
        {
            _isWindowShowing = true;
            var screenPos = Cursor.Position;
            ShowAt(screenPos);
            
            Task.Delay(5000).ContinueWith(_ =>
            {
                if (!IsDisposed && _isWindowShowing)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            Hide();
                            _isWindowShowing = false;
                        }));
                    }
                }
            });
        }
    }

    /// <summary>
    /// Opens the web interface in default browser
    /// </summary>
    private void OpenWebInterface()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = $"http://localhost:{_webPort}/",
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open web interface: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Opens the dashboard page
    /// </summary>
    private void OpenDashboard()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = $"http://localhost:{_webPort}/dashboard",
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open dashboard: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Opens the silicon beings management page
    /// </summary>
    private void ManageBeings()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = $"http://localhost:{_webPort}/beings",
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open beings management: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Opens the configuration page
    /// </summary>
    private void OpenConfiguration()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = $"http://localhost:{_webPort}/config",
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open configuration: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Opens the LiteDB management window as a modeless dialog.
    /// If a window already exists, it is brought to the front instead of creating a new one.
    /// </summary>
    private LiteDBAdminWindow? _liteDbWindow;
    private void OpenLiteDBManagement()
    {
        try
        {
            if (_liteDbWindow == null || _liteDbWindow.IsDisposed)
            {
                var localization = CreateLiteDBLocalization();
                _liteDbWindow = new LiteDBAdminWindow(localization);
                _liteDbWindow.FormClosed += (s, e) => _liteDbWindow = null;
                _liteDbWindow.Show();
            }
            else
            {
                if (_liteDbWindow.WindowState == FormWindowState.Minimized)
                    _liteDbWindow.WindowState = FormWindowState.Normal;
                _liteDbWindow.Activate();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open LiteDB management: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Requests application exit
    /// </summary>
    private void RequestExit()
    {
        notifyIcon1.Visible = false;
        ExitRequested?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Updates the application status
    /// </summary>
    public void UpdateStatus(string status)
    {
        ApplicationStatus = status;
    }

    /// <summary>
    /// Disposes the tray status window and cleans up resources
    /// </summary>
    public new void Dispose()
    {
        notifyIcon1.Visible = false;
        notifyIcon1.Dispose();
        contextMenuStrip1.Dispose();
        base.Dispose();
    }

    /// <summary>
    /// Timer tick handler - updates content safely across threads
    /// </summary>
    private void OnTimerTick(object? sender, System.Timers.ElapsedEventArgs e)
    {
        // Only update if window is showing and handle is created
        if (IsDisposed || !IsHandleCreated || !Visible)
            return;

        try
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateContent));
            }
            else
            {
                UpdateContent();
            }
        }
        catch
        {
            // Ignore timer update errors
        }
    }

    /// <summary>
    /// Initializes the window properties
    /// </summary>
    private void InitializeWindow()
    {
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        BackColor = Color.FromArgb(30, 30, 30); // Dark background
        Size = new Size(350, 280);
        Opacity = 0.95;
        TopMost = true;

        // Add rounded corners effect (basic)
        Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 10, 10));
        Icon = new Icon(new MemoryStream(SiliconLife.Common.icons.slc));
    }

    /// <summary>
    /// Initializes the UI components
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        notifyIcon1 = new NotifyIcon(components);
        contextMenuStrip1 = new ContextMenuStrip(components);
        
        _mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15),
            BackColor = Color.Transparent
        };

        // Title
        _lblTitle = new Label
        {
            Text = _localization.SoftwareName,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(15, 15)
        };

        // Separator
        var sep1 = CreateSeparator(15, 45);

        // Status
        _lblStatus = new Label
        {
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9),
            AutoSize = true,
            Location = new Point(15, 55)
        };

        // Uptime
        _lblUptime = new Label
        {
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9),
            AutoSize = true,
            Location = new Point(15, 77)
        };

        // Separator
        var sep2 = CreateSeparator(15, 102);

        // Silicon Beings count
        _lblBeings = new Label
        {
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9),
            AutoSize = true,
            Location = new Point(15, 112)
        };

        // Being name
        _lblBeingName = new Label
        {
            ForeColor = Color.FromArgb(200, 200, 200),
            Font = new Font("Segoe UI", 8),
            AutoSize = true,
            Location = new Point(30, 134)
        };

        // AI Model
        _lblAIModel = new Label
        {
            ForeColor = Color.FromArgb(200, 200, 200),
            Font = new Font("Segoe UI", 8),
            AutoSize = true,
            Location = new Point(30, 152)
        };

        // Separator
        var sep3 = CreateSeparator(15, 172);

        // Memory
        _lblMemory = new Label
        {
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9),
            AutoSize = true,
            Location = new Point(15, 182)
        };

        // CPU
        _lblCPU = new Label
        {
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9),
            AutoSize = true,
            Location = new Point(180, 182)
        };

        // Web
        _lblWeb = new Label
        {
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9),
            AutoSize = true,
            Location = new Point(15, 204)
        };

        // Separator
        var sep4 = CreateSeparator(15, 224);

        // Hint
        _lblHint = new Label
        {
            Text = $"{_localization.DoubleClick}: {_localization.OpenWebInterface}  |  {_localization.RightClick}: {_localization.ShowMenu}",
            ForeColor = Color.FromArgb(150, 150, 150),
            Font = new Font("Segoe UI", 7),
            AutoSize = true,
            Location = new Point(15, 234)
        };

        _mainPanel.Controls.Add(_lblTitle);
        _mainPanel.Controls.Add(sep1);
        _mainPanel.Controls.Add(_lblStatus);
        _mainPanel.Controls.Add(_lblUptime);
        _mainPanel.Controls.Add(sep2);
        _mainPanel.Controls.Add(_lblBeings);
        _mainPanel.Controls.Add(_lblBeingName);
        _mainPanel.Controls.Add(_lblAIModel);
        _mainPanel.Controls.Add(sep3);
        _mainPanel.Controls.Add(_lblMemory);
        _mainPanel.Controls.Add(_lblCPU);
        _mainPanel.Controls.Add(_lblWeb);
        _mainPanel.Controls.Add(sep4);
        _mainPanel.Controls.Add(_lblHint);

        Controls.Add(_mainPanel);

        // notifyIcon1
        notifyIcon1.ContextMenuStrip = contextMenuStrip1;
        notifyIcon1.Text = _localization.SoftwareName;
        notifyIcon1.Visible = true;
        notifyIcon1.MouseDoubleClick += new MouseEventHandler(OnTrayIconDoubleClick);
        notifyIcon1.MouseMove += new MouseEventHandler(OnTrayIconMouseMove);
        notifyIcon1.Icon = new Icon(new MemoryStream(SiliconLife.Common.icons.slc));

        // Initial content update
        UpdateContent();
    }

    /// <summary>
    /// Creates a separator line
    /// </summary>
    private Panel CreateSeparator(int x, int y)
    {
        return new Panel
        {
            BackColor = Color.FromArgb(60, 60, 60),
            Size = new Size(Width - 30, 1),
            Location = new Point(x, y)
        };
    }

    /// <summary>
    /// Updates the window content with current status
    /// </summary>
    private void UpdateContent()
    {
        if (IsDisposed || !IsHandleCreated) return;

        try
        {
            // Status
            _lblStatus.Text = $"{_localization.Status}: ● {GetApplicationStatus()}";

            // Uptime
            TimeSpan uptime = DateTime.Now - _startTime;
            _lblUptime.Text = $"{_localization.Uptime}: {uptime:hh\\:mm\\:ss}";

            // Silicon Beings
            int beingCount = GetActiveBeingCount();
            _lblBeings.Text = $"{_localization.SiliconBeings}: {beingCount} {_localization.Active}";

            // Being info
            var (name, model) = GetActiveBeingInfo();
            _lblBeingName.Text = string.IsNullOrEmpty(name) ? "" : $"{_localization.Name}: {name}";
            _lblAIModel.Text = string.IsNullOrEmpty(model) ? "" : $"{_localization.AIModel}: {model}";

            // Resources
            _lblMemory.Text = $"{_localization.Memory}: {GetMemoryUsage()}";
            _lblCPU.Text = $"{_localization.CPU}: {GetCpuUsage()}";
            _lblWeb.Text = $"{_localization.Web}: http://localhost:{_webPort}";
        }
        catch
        {
            // Ignore update errors
        }
    }

    /// <summary>
    /// Gets the current application status
    /// </summary>
    private string GetApplicationStatus()
    {
        return "Running"; // Can be extended to track actual status
    }

    /// <summary>
    /// Gets the count of active silicon beings
    /// </summary>
    private int GetActiveBeingCount()
    {
        try
        {
            return MainLoop.BeingManager.GetAllBeings().Count;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Gets information about the active silicon being
    /// </summary>
    private (string name, string model) GetActiveBeingInfo()
    {
        try
        {
            var beings = MainLoop.BeingManager.GetAllBeings();
            if (beings.Count == 0) return (string.Empty, string.Empty);

            var firstBeing = beings[0];
            string modelName = GetBeingModelName(firstBeing);
            
            return (firstBeing.Name, modelName);
        }
        catch
        {
            return (string.Empty, string.Empty);
        }
    }

    /// <summary>
    /// Gets the AI model name for a silicon being
    /// </summary>
    private string GetBeingModelName(SiliconBeingBase being)
    {
        try
        {
            if (being.AIClientConfig != null &&
                being.AIClientConfig.TryGetValue("model", out var model))
            {
                return model?.ToString() ?? "N/A";
            }
            return "N/A";
        }
        catch
        {
            return "N/A";
        }
    }

    /// <summary>
    /// Gets current memory usage
    /// </summary>
    private string GetMemoryUsage()
    {
        try
        {
            using var process = Process.GetCurrentProcess();
            long memoryBytes = process.WorkingSet64;
            double memoryMB = memoryBytes / (1024.0 * 1024.0);
            return $"{memoryMB:F0} MB";
        }
        catch
        {
            return "N/A";
        }
    }

    /// <summary>
    /// Gets current CPU usage percentage
    /// </summary>
    private string GetCpuUsage()
    {
        try
        {
            using var process = Process.GetCurrentProcess();
            TimeSpan totalProcessorTime = process.TotalProcessorTime;
            TimeSpan elapsed = DateTime.Now - process.StartTime;
            
            if (elapsed.TotalMilliseconds > 0)
            {
                double cpuPercent = (totalProcessorTime.TotalMilliseconds / elapsed.TotalMilliseconds) * 100;
                return $"{cpuPercent:F1}%";
            }
            return "0.0%";
        }
        catch
        {
            return "N/A";
        }
    }

    /// <summary>
    /// Shows the window at the specified location
    /// </summary>
    public void ShowAt(Point location)
    {
        Location = new Point(location.X - Width / 2, location.Y - Height - 10);
        Show();
    }

    /// <summary>
    /// Win32 API for creating rounded rectangles
    /// </summary>
    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

    /// <summary>
    /// Creates a LiteDBAdminLocalization instance based on the current configured language.
    /// </summary>
    private LiteDBAdminLocalization CreateLiteDBLocalization()
    {
        var language = Config.Instance?.Data?.Language ?? Language.EnUS;
        
        return language switch
        {
            Language.ZhCN => new LiteDBAdminLocalizationZhCN(),
            Language.ZhHK => new LiteDBAdminLocalizationZhHK(),
            Language.ZhTW => new LiteDBAdminLocalizationZhHK(), // Fallback to ZhHK
            Language.ZhMO => new LiteDBAdminLocalizationZhHK(), // Fallback to ZhHK
            Language.ZhSG => new LiteDBAdminLocalizationZhCN(), // Fallback to ZhCN
            Language.ZhMY => new LiteDBAdminLocalizationZhCN(), // Fallback to ZhCN
            
            Language.EnUS => new LiteDBAdminLocalizationEnUS(),
            Language.EnGB => new LiteDBAdminLocalizationEnUS(), // Fallback to EnUS
            Language.EnCA => new LiteDBAdminLocalizationEnUS(), // Fallback to EnUS
            Language.EnAU => new LiteDBAdminLocalizationEnUS(), // Fallback to EnUS
            Language.EnIN => new LiteDBAdminLocalizationEnUS(), // Fallback to EnUS
            Language.EnSG => new LiteDBAdminLocalizationEnUS(), // Fallback to EnUS
            Language.EnZA => new LiteDBAdminLocalizationEnUS(), // Fallback to EnUS
            Language.EnIE => new LiteDBAdminLocalizationEnUS(), // Fallback to EnUS
            Language.EnNZ => new LiteDBAdminLocalizationEnUS(), // Fallback to EnUS
            Language.EnMY => new LiteDBAdminLocalizationEnUS(), // Fallback to EnUS
            
            Language.JaJP => new LiteDBAdminLocalizationJaJP(),
            Language.KoKR => new LiteDBAdminLocalizationKoKR(),
            Language.EsES => new LiteDBAdminLocalizationEsES(),
            Language.EsMX => new LiteDBAdminLocalizationEsES(), // Fallback to EsES
            Language.CsCZ => new LiteDBAdminLocalizationCsCZ(),
            
            Language.DeDE => new LiteDBAdminLocalizationDeDE(),
            Language.DeAT => new LiteDBAdminLocalizationDeDE(), // Fallback to DeDE
            Language.DeCH => new LiteDBAdminLocalizationDeDE(), // Fallback to DeDE
            Language.DeLU => new LiteDBAdminLocalizationDeDE(), // Fallback to DeDE
            Language.DeLI => new LiteDBAdminLocalizationDeDE(), // Fallback to DeDE
            
            _ => new LiteDBAdminLocalizationEnUS() // Default to English
        };
    }
}

/// <summary>
/// Application context for tray-only application (no main form visible)
/// </summary>
public class TrayApplicationContext : ApplicationContext
{
    private readonly TrayStatusWindow _trayWindow;

    public TrayApplicationContext(TrayStatusWindow trayWindow)
    {
        _trayWindow = trayWindow;
        _trayWindow.ExitRequested += OnExitRequested;
    }

    private void OnExitRequested(object? sender, EventArgs e)
    {
        ExitThread();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _trayWindow.ExitRequested -= OnExitRequested;
            _trayWindow.Dispose();
        }
        base.Dispose(disposing);
    }
}
