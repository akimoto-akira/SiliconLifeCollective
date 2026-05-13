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

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using SiliconLife.Collective;
using SiliconLife.Common.SiliconBeing;

namespace SiliconLife.Fast.Tray;

/// <summary>
/// Avalonia-based tray status popup window
/// </summary>
public partial class TrayStatusWindow : Window
{
    private readonly TrayLocalizationBase _localization;
    private readonly int _webPort;
    private readonly DateTime _startTime;
    private readonly DispatcherTimer _updateTimer;
    private readonly bool _isLinux;
    private bool _isWindowShowing;
    
    // UI Controls
    private TextBlock? _titleText;
    private TextBlock? _statusText;
    private TextBlock? _uptimeText;
    private TextBlock? _beingsText;
    private TextBlock? _beingNameText;
    private TextBlock? _aiModelText;
    private TextBlock? _memoryText;
    private TextBlock? _cpuText;
    private TextBlock? _webText;

    /// <summary>
    /// Event triggered when exit is requested from tray menu
    /// </summary>
    public event EventHandler? ExitRequested;

    /// <summary>
    /// Gets the current application status
    /// </summary>
    public string ApplicationStatus { get; private set; } = "Running";

    public TrayStatusWindow(TrayLocalizationBase localization, int webPort)
    {
        _localization = localization;
        _webPort = webPort;
        _startTime = DateTime.Now;
        
        // Detect platform
        _isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        
        // Set up window properties
        Title = _localization.SoftwareName;
        Width = 350;
        Height = 280;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Topmost = false;
        CanResize = false;
        SystemDecorations = SystemDecorations.Full; // Show title bar with close button
        Background = Avalonia.Media.SolidColorBrush.Parse("#1E1E1E");
        Opacity = 0.95;
        
        // Linux: Keep window always visible, don't auto-hide
        if (_isLinux)
        {
            Topmost = true; // Keep window on top
        }
        
        // Set window icon
        try
        {
            string iconPath = Path.Combine(AppContext.BaseDirectory, "slc.ico");
            if (File.Exists(iconPath))
            {
                Icon = new WindowIcon(iconPath);
            }
        }
        catch { }
        
        // Load AXAML
        try
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
            System.Diagnostics.Debug.WriteLine("AXAML loaded successfully");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AXAML load failed: {ex.Message}");
            Console.WriteLine($"[ERROR] AXAML load failed: {ex.Message}");
        }
        
        // Start update timer (1 second interval)
        _updateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _updateTimer.Tick += (s, e) => UpdateContent();
        _updateTimer.Start();
        
        // Handle right-click for context menu
        AddHandler(InputElement.PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel);
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var properties = e.GetCurrentPoint(this).Properties;
        
        if (properties.IsRightButtonPressed)
        {
            // Right click: show context menu at mouse position
            ShowContextMenu();
            e.Handled = true;
        }
        else if (e.ClickCount == 2)
        {
            // Double click: open web interface
            OpenWebInterface();
        }
    }

    private void ShowContextMenu()
    {
        var menu = new ContextMenu();
        
        menu.Items.Add(new MenuItem { Header = _localization.OpenWebInterface });
        menu.Items.Add(new MenuItem { Header = _localization.Dashboard });
        menu.Items.Add(new MenuItem { Header = _localization.ManageSiliconBeings });
        menu.Items.Add(new MenuItem { Header = _localization.Configuration });
        menu.Items.Add(new Separator());
        menu.Items.Add(new MenuItem { Header = _localization.Exit });
        
        // Attach click handlers
        ((MenuItem)menu.Items[0]).Click += (s, e) => OpenWebInterface();
        ((MenuItem)menu.Items[1]).Click += (s, e) => OpenDashboard();
        ((MenuItem)menu.Items[2]).Click += (s, e) => ManageBeings();
        ((MenuItem)menu.Items[3]).Click += (s, e) => OpenConfiguration();
        ((MenuItem)menu.Items[5]).Click += (s, e) => RequestExit();
        
        menu.Open(this);
    }

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
            Console.WriteLine($"Failed to open web interface: {ex.Message}");
        }
    }

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
            Console.WriteLine($"Failed to open dashboard: {ex.Message}");
        }
    }

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
            Console.WriteLine($"Failed to open beings management: {ex.Message}");
        }
    }

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
            Console.WriteLine($"Failed to open configuration: {ex.Message}");
        }
    }

    public void RequestExit()
    {
        ExitRequested?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateStatus(string status)
    {
        ApplicationStatus = status;
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        
        try
        {
            // Find controls
            _titleText = this.FindControl<TextBlock>("TitleText");
            _statusText = this.FindControl<TextBlock>("StatusText");
            _uptimeText = this.FindControl<TextBlock>("UptimeText");
            _beingsText = this.FindControl<TextBlock>("BeingsText");
            _beingNameText = this.FindControl<TextBlock>("BeingNameText");
            _aiModelText = this.FindControl<TextBlock>("AIModelText");
            _memoryText = this.FindControl<TextBlock>("MemoryText");
            _cpuText = this.FindControl<TextBlock>("CPUText");
            _webText = this.FindControl<TextBlock>("WebText");
            
            System.Diagnostics.Debug.WriteLine($"Controls found: Title={_titleText != null}, Status={_statusText != null}");
            UpdateContent();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"OnOpened error: {ex.Message}");
            Console.WriteLine($"[ERROR] Failed to initialize window controls: {ex.Message}");
        }
    }

    private void UpdateContent()
    {
        if (!IsVisible) return;
        
        Dispatcher.UIThread.Post(() =>
        {
            try
            {
                // Status
                if (_statusText != null)
                    _statusText.Text = $"{_localization.Status}: ● {GetApplicationStatus()}";

                // Uptime
                TimeSpan uptime = DateTime.Now - _startTime;
                if (_uptimeText != null)
                    _uptimeText.Text = $"{_localization.Uptime}: {uptime:hh\\:mm\\:ss}";

                // Silicon Beings
                int beingCount = GetActiveBeingCount();
                if (_beingsText != null)
                    _beingsText.Text = $"{_localization.SiliconBeings}: {beingCount} {_localization.Active}";

                // Being info
                var (name, model) = GetActiveBeingInfo();
                if (_beingNameText != null)
                    _beingNameText.Text = string.IsNullOrEmpty(name) ? "" : $"{_localization.Name}: {name}";
                if (_aiModelText != null)
                    _aiModelText.Text = string.IsNullOrEmpty(model) ? "" : $"{_localization.AIModel}: {model}";

                // Resources
                if (_memoryText != null)
                    _memoryText.Text = $"{_localization.Memory}: {GetMemoryUsage()}";
                if (_cpuText != null)
                    _cpuText.Text = $"{_localization.CPU}: {GetCpuUsage()}";
                if (_webText != null)
                    _webText.Text = $"{_localization.Web}: http://localhost:{_webPort}";
            }
            catch
            {
                // Ignore update errors
            }
        });
    }

    private string GetApplicationStatus() => "Running";

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

    public void ShowAt(PixelPoint location)
    {
        Position = new PixelPoint(location.X - (int)(Width / 2), location.Y - (int)Height - 10);
        Show();
        _isWindowShowing = true;
        
        // Auto-hide after 5 seconds
        Task.Delay(5000).ContinueWith(_ =>
        {
            if (_isWindowShowing && IsVisible)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    Hide();
                    _isWindowShowing = false;
                });
            }
        });
    }

    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);
        
        if (_isLinux)
        {
            // Linux: Show confirmation dialog when closing window
            e.Cancel = true;
            
            var result = await ShowMessageBoxAsync(
                _localization.SoftwareName,
                "确定要退出程序吗？\n\n选择\"是\"将关闭应用程序。",
                MessageBoxButton.YesNo);
            
            if (result == MessageBoxResult.Yes)
            {
                // User confirmed exit
                _updateTimer.Stop();
                RequestExit();
            }
            // else: Keep window open
        }
        else
        {
            // Windows/macOS: Hide window (tray icon available)
            e.Cancel = true;
            Hide();
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        _updateTimer.Stop();
        base.OnClosed(e);
    }

    private async Task<MessageBoxResult> ShowMessageBoxAsync(string title, string message, MessageBoxButton buttons)
    {
        var tcs = new TaskCompletionSource<MessageBoxResult>();
        
        Dispatcher.UIThread.Post(() =>
        {
            var box = new Window
            {
                Title = title,
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                SystemDecorations = SystemDecorations.Full,
                Topmost = true,
                Background = Avalonia.Media.SolidColorBrush.Parse("#2D2D2D")
            };

            var grid = new Grid
            {
                Margin = new Avalonia.Thickness(20)
            };
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // Message
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // Buttons

            var textBlock = new TextBlock
            {
                Text = message,
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Foreground = Avalonia.Media.Brushes.White,
                FontSize = 14,
                Margin = new Avalonia.Thickness(0, 0, 0, 20)
            };
            Grid.SetRow(textBlock, 0);

            var buttonPanel = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Spacing = 10
            };
            Grid.SetRow(buttonPanel, 1);

            var yesButton = new Button
            {
                Content = "是",
                Width = 80,
                Background = Avalonia.Media.SolidColorBrush.Parse("#E74C3C"),
                Foreground = Avalonia.Media.Brushes.White
            };
            yesButton.Click += (s, e) =>
            {
                box.Close();
                tcs.SetResult(MessageBoxResult.Yes);
            };

            var noButton = new Button
            {
                Content = "否",
                Width = 80,
                Background = Avalonia.Media.SolidColorBrush.Parse("#3498DB"),
                Foreground = Avalonia.Media.Brushes.White
            };
            noButton.Click += (s, e) =>
            {
                box.Close();
                tcs.SetResult(MessageBoxResult.No);
            };

            buttonPanel.Children.Add(yesButton);
            buttonPanel.Children.Add(noButton);
            grid.Children.Add(textBlock);
            grid.Children.Add(buttonPanel);

            box.Content = grid;
            box.ShowDialog(this);
        });

        return await tcs.Task;
    }
}

public enum MessageBoxResult
{
    Yes,
    No
}

public enum MessageBoxButton
{
    YesNo
}
