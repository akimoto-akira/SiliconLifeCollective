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
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using SiliconLife.Fast.Tray;
using System;

namespace SiliconLife.Fast;

public class App : Application
{
    private static TrayIcon? _trayIcon;
    private static TrayStatusWindow? _trayWindow;
    private static TrayLocalizationBase? _localization;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            await Program.StartAsync(desktop.Args ?? Array.Empty<string>());
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static int _webPort = 8080;

    public static void SetStatusWindow(TrayStatusWindow window)
    {
        _trayWindow = window;
    }

    public static void InitializeTray(TrayStatusWindow trayWindow, string iconPath, int webPort, TrayLocalizationBase localization)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        _trayWindow = trayWindow;
        _webPort = webPort;
        _localization = localization;

        try
        {
            _trayIcon = new TrayIcon
            {
                Icon = new WindowIcon(iconPath),
                ToolTipText = $"{localization.SoftwareName} (Port: {webPort})",
                IsVisible = true
            };

            _trayIcon.Clicked += OnTrayIconClicked;

            var nativeMenu = new NativeMenu();

            var showStatusItem = new NativeMenuItem { Header = localization.ShowStatus };
            showStatusItem.Click += (s, e) => ShowStatusWindow();

            var openWebItem = new NativeMenuItem { Header = localization.OpenWebInterface };
            openWebItem.Click += (s, e) => OpenWebInterface();

            var exitItem = new NativeMenuItem { Header = localization.Exit };
            exitItem.Click += (s, e) => RequestExit();

            nativeMenu.Items.Add(showStatusItem);
            nativeMenu.Items.Add(openWebItem);
            nativeMenu.Items.Add(new NativeMenuItemSeparator());
            nativeMenu.Items.Add(exitItem);

            _trayIcon.Menu = nativeMenu;

            System.Diagnostics.Debug.WriteLine($"TrayIcon initialized: {iconPath}, Port: {webPort}");
        }
        catch (Exception ex)
        {
            // TrayIcon initialization may fail on some Linux environments
            System.Diagnostics.Debug.WriteLine($"TrayIcon initialization failed: {ex.Message}");
            Console.WriteLine($"[WARN] Tray icon not available: {ex.Message}");
            Console.WriteLine($"[INFO] Use Web UI at http://localhost:{webPort}/ for management");
        }
    }

    private static void OnTrayIconClicked(object? sender, EventArgs e)
    {
        ShowStatusWindow();
    }

    private static void ShowStatusWindow()
    {
        if (_trayWindow == null)
            return;

        Dispatcher.UIThread.Post(() =>
        {
            if (_trayWindow.IsVisible)
            {
                _trayWindow.Hide();
            }
            else
            {
                _trayWindow.Show();
                _trayWindow.Activate();
            }
        });
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
