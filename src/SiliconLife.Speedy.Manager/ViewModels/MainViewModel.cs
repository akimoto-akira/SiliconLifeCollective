using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SiliconLife.Speedy.Manager.Models;
using SiliconLife.Speedy.Manager.Services;
using SiliconLife.Speedy.Manager.ViewModels.Dialogs;

namespace SiliconLife.Speedy.Manager.ViewModels;

/// <summary>
/// 主窗口 ViewModel，协调所有子 ViewModel 和 Service，管理全局应用状态。
/// 对应需求 1.1–1.6, 9.1, 9.3–9.5, 12.1–12.3。
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly IPackService _packService;
    private readonly IFileDialogService _fileDialogService;
    private readonly INotificationService _notificationService;
    private readonly IRecentFilesService _recentFilesService;

    // ─── 子 ViewModel ─────────────────────────────────────────────────────────

    public DirectoryTreeViewModel TreeViewModel { get; }
    public ContentViewerViewModel ContentViewModel { get; }
    public MetadataPanelViewModel MetadataViewModel { get; }
    public StatusBarViewModel StatusViewModel { get; }

    // ─── 对话框 ViewModel 工厂（由 DI 提供，每次使用时创建新实例）
    private readonly Func<NewEntryDialogViewModel> _newEntryDialogFactory;
    private readonly Func<ImportDialogViewModel> _importDialogFactory;
    private readonly Func<FileInfoDialogViewModel> _fileInfoDialogFactory;

    public MainViewModel(
        IPackService packService,
        IFileDialogService fileDialogService,
        INotificationService notificationService,
        IRecentFilesService recentFilesService,
        DirectoryTreeViewModel treeViewModel,
        ContentViewerViewModel contentViewModel,
        MetadataPanelViewModel metadataViewModel,
        StatusBarViewModel statusViewModel,
        Func<NewEntryDialogViewModel> newEntryDialogFactory,
        Func<ImportDialogViewModel> importDialogFactory,
        Func<FileInfoDialogViewModel> fileInfoDialogFactory)
    {
        _packService = packService;
        _fileDialogService = fileDialogService;
        _notificationService = notificationService;
        _recentFilesService = recentFilesService;

        TreeViewModel = treeViewModel;
        ContentViewModel = contentViewModel;
        MetadataViewModel = metadataViewModel;
        StatusViewModel = statusViewModel;

        _newEntryDialogFactory = newEntryDialogFactory;
        _importDialogFactory = importDialogFactory;
        _fileInfoDialogFactory = fileInfoDialogFactory;

        // 订阅目录树事件
        TreeViewModel.SelectedNodeChanged += OnTreeNodeSelected;
        TreeViewModel.NewEntryRequested += OnNewEntryRequested;
        TreeViewModel.ImportFilesRequested += OnImportFilesRequested;

        // 初始化最近文件列表
        RefreshRecentFiles();
    }

    // ─── 全局状态属性 ─────────────────────────────────────────────────────────

    /// <summary>窗口标题（含文件名和只读标识）。</summary>
    [ObservableProperty]
    private string _windowTitle = "SpeedyPack Manager";

    /// <summary>当前是否已打开 Pack 文件。</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CloseFileCommand))]
    [NotifyCanExecuteChangedFor(nameof(FlushCommand))]
    [NotifyCanExecuteChangedFor(nameof(CompactCommand))]
    [NotifyCanExecuteChangedFor(nameof(ShowFileInfoCommand))]
    private bool _isPackOpen;

    /// <summary>当前文件是否以只读模式打开。</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(FlushCommand))]
    [NotifyCanExecuteChangedFor(nameof(CompactCommand))]
    private bool _isReadOnly;

    /// <summary>是否正在执行耗时操作（如 Compact），期间阻止其他操作。</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OpenFileCommand))]
    [NotifyCanExecuteChangedFor(nameof(OpenFileReadOnlyCommand))]
    [NotifyCanExecuteChangedFor(nameof(CloseFileCommand))]
    [NotifyCanExecuteChangedFor(nameof(FlushCommand))]
    [NotifyCanExecuteChangedFor(nameof(CompactCommand))]
    [NotifyCanExecuteChangedFor(nameof(ShowFileInfoCommand))]
    [NotifyCanExecuteChangedFor(nameof(OpenRecentFileCommand))]
    private bool _isBusy;

    /// <summary>最近打开的文件路径列表（最多 10 条）。</summary>
    public ObservableCollection<string> RecentFiles { get; } = new();

    // ─── 命令 ─────────────────────────────────────────────────────────────────

    /// <summary>打开文件（读写模式）。</summary>
    [RelayCommand(CanExecute = nameof(CanOpenFile))]
    private async Task OpenFileAsync()
    {
        var path = _fileDialogService.OpenSpkFile();
        if (path == null) return;

        await OpenFileInternalAsync(path, readOnly: false);
    }

    private bool CanOpenFile() => !IsBusy;

    /// <summary>以只读模式打开文件。</summary>
    [RelayCommand(CanExecute = nameof(CanOpenFile))]
    private async Task OpenFileReadOnlyAsync()
    {
        var path = _fileDialogService.OpenSpkFile();
        if (path == null) return;

        await OpenFileInternalAsync(path, readOnly: true);
    }

    /// <summary>关闭当前文件。</summary>
    [RelayCommand(CanExecute = nameof(CanCloseFile))]
    private async Task CloseFileAsync()
    {
        if (!IsPackOpen) return;

        try
        {
            await _packService.CloseAsync();
        }
        catch (Exception ex)
        {
            _notificationService.ShowError("关闭失败", $"关闭文件时发生错误：{ex.Message}");
        }
        finally
        {
            ClearAllViewModels();
        }
    }

    private bool CanCloseFile() => IsPackOpen && !IsBusy;

    /// <summary>刷新到磁盘（Flush）。</summary>
    [RelayCommand(CanExecute = nameof(CanFlush))]
    private async Task FlushAsync()
    {
        StatusViewModel.SetStatus("正在刷新...");
        try
        {
            await _packService.FlushAsync();
            StatusViewModel.SetStatus($"刷新完成 ({DateTime.Now:HH:mm:ss})");
        }
        catch (Exception ex)
        {
            StatusViewModel.SetStatus("刷新失败");
            _notificationService.ShowError("刷新失败", $"无法刷新到磁盘：{ex.Message}");
        }
    }

    private bool CanFlush() => IsPackOpen && !IsReadOnly && !IsBusy;

    /// <summary>压缩文件（Compact）。</summary>
    [RelayCommand(CanExecute = nameof(CanCompact))]
    private async Task CompactAsync()
    {
        bool confirmed = await _notificationService.ShowConfirmAsync(
            "确认压缩",
            "压缩操作将重写整个文件，可能耗时较长。操作期间无法进行其他操作。\n\n确定要继续吗？");

        if (!confirmed) return;

        var fileInfoBefore = _packService.GetFileInfo();
        IsBusy = true;
        StatusViewModel.SetStatus("正在压缩文件...");

        try
        {
            await _packService.CompactAsync();

            var fileInfoAfter = _packService.GetFileInfo();
            StatusViewModel.UpdateFromFileInfo(fileInfoAfter);

            var savedBytes = fileInfoBefore.FileSize - fileInfoAfter.FileSize;
            StatusViewModel.SetStatus(
                $"压缩完成：{fileInfoBefore.FileSize:N0} B → {fileInfoAfter.FileSize:N0} B（节省 {savedBytes:N0} B）");

            TreeViewModel.RefreshTree();
        }
        catch (Exception ex)
        {
            StatusViewModel.SetStatus("压缩失败");
            _notificationService.ShowError("压缩失败", $"压缩操作失败：{ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanCompact() => IsPackOpen && !IsReadOnly && !IsBusy;

    /// <summary>显示文件信息对话框。</summary>
    [RelayCommand(CanExecute = nameof(CanShowFileInfo))]
    private void ShowFileInfo()
    {
        var vm = _fileInfoDialogFactory();
        vm.LoadFromFileInfo(_packService.GetFileInfo());
        ShowFileInfoDialogRequested?.Invoke(vm);
    }

    private bool CanShowFileInfo() => IsPackOpen && !IsBusy;

    /// <summary>打开最近文件。</summary>
    [RelayCommand(CanExecute = nameof(CanOpenFile))]
    private async Task OpenRecentFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return;
        await OpenFileInternalAsync(filePath, readOnly: false);
    }

    // ─── 事件（供 View 层订阅以显示对话框）────────────────────────────────────

    /// <summary>请求显示新建条目对话框。</summary>
    public event Action<NewEntryDialogViewModel>? ShowNewEntryDialogRequested;

    /// <summary>请求显示导入文件对话框。</summary>
    public event Action<ImportDialogViewModel>? ShowImportDialogRequested;

    /// <summary>请求显示文件信息对话框。</summary>
    public event Action<FileInfoDialogViewModel>? ShowFileInfoDialogRequested;

    // ─── 内部方法 ─────────────────────────────────────────────────────────────

    private async Task OpenFileInternalAsync(string filePath, bool readOnly)
    {
        // 如果已有文件打开，先关闭
        if (IsPackOpen)
        {
            await CloseFileAsync();
        }

        try
        {
            await _packService.OpenAsync(filePath, readOnly);

            IsPackOpen = true;
            IsReadOnly = readOnly;

            // 更新子 ViewModel
            var fileInfo = _packService.GetFileInfo();
            StatusViewModel.UpdateFromFileInfo(fileInfo);
            StatusViewModel.IsReadOnly = readOnly;
            TreeViewModel.RefreshTree();

            // 更新最近文件列表
            _recentFilesService.AddRecentFile(filePath);
            RefreshRecentFiles();

            // 更新窗口标题
            UpdateWindowTitle(filePath, readOnly);
        }
        catch (Exception ex)
        {
            _notificationService.ShowError("打开失败", $"无法打开文件：{ex.Message}");
        }
    }

    private void OnTreeNodeSelected(PackEntryNode? node)
    {
        if (node == null)
        {
            ContentViewModel.Clear();
            MetadataViewModel.Clear();
            return;
        }

        if (node.IsDirectory)
        {
            ContentViewModel.Clear();

            // 加载目录统计信息
            var entries = _packService.ListEntries(node.FullPath);
            var dirs = _packService.ListDirectories(node.FullPath);
            MetadataViewModel.LoadDirectoryInfo(node.FullPath, entries.Count, dirs.Count);
        }
        else
        {
            // 加载条目内容
            var data = _packService.ReadEntry(node.FullPath);
            var metadata = _packService.GetMetadata(node.FullPath);

            if (data != null && metadata != null)
            {
                ContentViewModel.LoadEntry(node.FullPath, data, metadata.ContentType);
                MetadataViewModel.LoadMetadata(metadata);
            }
        }
    }

    private void OnNewEntryRequested(string defaultPath)
    {
        var vm = _newEntryDialogFactory();
        vm.VirtualPath = defaultPath;
        ShowNewEntryDialogRequested?.Invoke(vm);
    }

    private void OnImportFilesRequested(string defaultPrefix)
    {
        var vm = _importDialogFactory();
        vm.VirtualPathPrefix = defaultPrefix;
        ShowImportDialogRequested?.Invoke(vm);
    }

    private void ClearAllViewModels()
    {
        IsPackOpen = false;
        IsReadOnly = false;
        WindowTitle = "SpeedyPack Manager";

        TreeViewModel.Clear();
        ContentViewModel.Clear();
        MetadataViewModel.Clear();
        StatusViewModel.Clear();
    }

    private void RefreshRecentFiles()
    {
        RecentFiles.Clear();
        foreach (var path in _recentFilesService.GetRecentFiles())
            RecentFiles.Add(path);
    }

    private void UpdateWindowTitle(string filePath, bool readOnly)
    {
        var fileName = Path.GetFileName(filePath);
        WindowTitle = readOnly
            ? $"{fileName} [只读] — SpeedyPack Manager"
            : $"{fileName} — SpeedyPack Manager";
    }
}
