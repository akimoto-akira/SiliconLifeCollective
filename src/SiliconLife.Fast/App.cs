using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using SiliconLife.Fast.Tray;

namespace SiliconLife.Fast;

public class App : Application
{
    private static TrayIcon? _trayIcon;
    private static TrayStatusWindow? _trayWindow;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Initialize core application (Avalonia is ready now)
            await Program.StartAsync();
            
            // Show window after initialization
            if (_trayWindow != null)
            {
                _trayWindow.Show();
                System.Diagnostics.Debug.WriteLine($"Status window shown");
            }
        }
        
        base.OnFrameworkInitializationCompleted();
    }

    private static int _webPort = 8080; // Default fallback

    /// <summary>
    /// Set the status window (called by Program.StartAsync)
    /// </summary>
    public static void SetStatusWindow(TrayStatusWindow window)
    {
        _trayWindow = window;
    }

    /// <summary>
    /// Initialize system tray icon (cross-platform) - NOT USED for now
    /// </summary>
    public static void InitializeTray(TrayStatusWindow trayWindow, string iconPath, int webPort)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        _trayWindow = trayWindow;
        _webPort = webPort;

        // Create tray icon
        _trayIcon = new TrayIcon
        {
            Icon = new WindowIcon(iconPath),
            ToolTipText = $"SiliconLife Fast (Port: {webPort})",
            IsVisible = true
        };

        // Create native menu for tray (Avalonia 11.x API)
        var nativeMenu = new NativeMenu();
        
        var openWebItem = new NativeMenuItem { Header = "Open Web Interface" };
        openWebItem.Click += (s, e) => OpenWebInterface();
        
        var exitItem = new NativeMenuItem { Header = "Exit" };
        exitItem.Click += (s, e) => RequestExit();

        nativeMenu.Items.Add(openWebItem);
        nativeMenu.Items.Add(new NativeMenuItemSeparator());
        nativeMenu.Items.Add(exitItem);

        _trayIcon.Menu = nativeMenu;

        System.Diagnostics.Debug.WriteLine($"TrayIcon initialized: {iconPath}, Port: {webPort}");
    }

    private static void OpenWebInterface()
    {
        try
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = $"http://localhost:{_webPort}/",
                    UseShellExecute = true
                }
            };
            process.Start();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to open web interface on port {_webPort}: {ex.Message}");
        }
    }

    private static void RequestExit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Dispatcher.UIThread.Post(() => desktop.Shutdown());
        }
    }
}
