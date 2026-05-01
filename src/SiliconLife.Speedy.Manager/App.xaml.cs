using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using SiliconLife.Speedy.Manager.Services;
using SiliconLife.Speedy.Manager.ViewModels;
using SiliconLife.Speedy.Manager.ViewModels.Dialogs;

namespace SiliconLife.Speedy.Manager;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // ─── Register global unhandled exception handlers ───────────────────
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

        // ─── Configure DI container ─────────────────────────────────────────
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();

        // ─── Create and show MainWindow ─────────────────────────────────────
        var mainWindow = new MainWindow();
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainWindow.DataContext = mainViewModel;
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // ─── Services (Singleton) ────────────────────────────────────────────
        services.AddSingleton<IPackService, PackService>();
        services.AddSingleton<IFileDialogService, FileDialogService>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<IRecentFilesService, RecentFilesService>();

        // ─── ViewModels ──────────────────────────────────────────────────────
        services.AddTransient<MainViewModel>();
        services.AddSingleton<DirectoryTreeViewModel>();
        services.AddSingleton<ContentViewerViewModel>();
        services.AddSingleton<MetadataPanelViewModel>();
        services.AddSingleton<StatusBarViewModel>();

        // ─── Dialog ViewModel factories ──────────────────────────────────────
        services.AddTransient<NewEntryDialogViewModel>();
        services.AddTransient<ImportDialogViewModel>();
        services.AddTransient<FileInfoDialogViewModel>();

        services.AddSingleton<Func<NewEntryDialogViewModel>>(
            sp => () => sp.GetRequiredService<NewEntryDialogViewModel>());
        services.AddSingleton<Func<ImportDialogViewModel>>(
            sp => () => sp.GetRequiredService<ImportDialogViewModel>());
        services.AddSingleton<Func<FileInfoDialogViewModel>>(
            sp => () => sp.GetRequiredService<FileInfoDialogViewModel>());
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show(
            $"发生未处理的异常：\n\n{e.Exception.Message}",
            "SpeedyPack Manager — 错误",
            MessageBoxButton.OK,
            MessageBoxImage.Error);

        e.Handled = true;
    }

    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        e.SetObserved();

        Application.Current?.Dispatcher.Invoke(() =>
        {
            MessageBox.Show(
                $"发生未处理的异步异常：\n\n{e.Exception.InnerException?.Message ?? e.Exception.Message}",
                "SpeedyPack Manager — 错误",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        });
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }
}
