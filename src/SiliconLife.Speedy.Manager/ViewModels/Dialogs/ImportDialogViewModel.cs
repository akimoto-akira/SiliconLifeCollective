using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SiliconLife.Speedy.Manager.Services;

namespace SiliconLife.Speedy.Manager.ViewModels.Dialogs;

/// <summary>
/// 导入文件对话框 ViewModel。
/// 对应需求 7.1, 7.2。
/// </summary>
public partial class ImportDialogViewModel : ObservableObject
{
    private readonly IFileDialogService _fileDialogService;

    public ImportDialogViewModel(IFileDialogService fileDialogService)
    {
        _fileDialogService = fileDialogService;
    }

    /// <summary>已选择的文件路径列表。</summary>
    public ObservableCollection<string> SelectedFiles { get; } = new();

    /// <summary>目标虚拟路径前缀（默认为当前选中目录路径）。</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private string _virtualPathPrefix = string.Empty;

    /// <summary>是否已确认。</summary>
    public bool IsConfirmed { get; private set; }

    /// <summary>
    /// 浏览并选择多个文件。
    /// </summary>
    [RelayCommand]
    private void BrowseFiles()
    {
        var files = _fileDialogService.OpenMultipleFiles();
        foreach (var file in files)
        {
            if (!SelectedFiles.Contains(file))
                SelectedFiles.Add(file);
        }

        ConfirmCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// 从列表中移除指定文件。
    /// </summary>
    [RelayCommand]
    private void RemoveFile(string filePath)
    {
        SelectedFiles.Remove(filePath);
        ConfirmCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// 确认导入命令：至少选择了一个文件时可用。
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanConfirm))]
    private void Confirm()
    {
        IsConfirmed = true;
        DialogCloseRequested?.Invoke(true);
    }

    private bool CanConfirm() => SelectedFiles.Count > 0;

    /// <summary>
    /// 取消命令。
    /// </summary>
    [RelayCommand]
    private void Cancel()
    {
        IsConfirmed = false;
        DialogCloseRequested?.Invoke(false);
    }

    /// <summary>请求关闭对话框的事件，参数为对话框结果（true=确认，false=取消）。</summary>
    public event Action<bool>? DialogCloseRequested;

    /// <summary>
    /// 构建导入映射列表：(文件系统路径, 虚拟路径)。
    /// 虚拟路径 = 前缀 + "/" + 文件名（前缀为空时直接使用文件名）。
    /// </summary>
    public IEnumerable<(string filePath, string virtualPath)> BuildImportMappings()
    {
        foreach (var filePath in SelectedFiles)
        {
            var fileName = Path.GetFileName(filePath);
            var virtualPath = string.IsNullOrEmpty(VirtualPathPrefix)
                ? fileName
                : $"{VirtualPathPrefix.TrimEnd('/')}/{fileName}";

            yield return (filePath, virtualPath);
        }
    }
}
