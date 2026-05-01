using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SiliconLife.Speedy.Manager.Helpers;

namespace SiliconLife.Speedy.Manager.ViewModels.Dialogs;

/// <summary>
/// 新建条目对话框 ViewModel。
/// 对应需求 4.1, 4.2, 4.5。
/// </summary>
public partial class NewEntryDialogViewModel : ObservableObject
{
    /// <summary>新条目的虚拟路径。</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PathValidationError))]
    [NotifyPropertyChangedFor(nameof(IsPathValid))]
    [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private string _virtualPath = string.Empty;

    /// <summary>内容类型（"json"、"raw"、"text"）。</summary>
    [ObservableProperty]
    private string _contentType = "json";

    /// <summary>条目内容文本。</summary>
    [ObservableProperty]
    private string _content = string.Empty;

    /// <summary>路径验证错误信息，路径合法时为 null。</summary>
    public string? PathValidationError
    {
        get
        {
            if (string.IsNullOrEmpty(VirtualPath))
                return null; // 未输入时不显示错误

            var result = VirtualPathValidator.Validate(VirtualPath);
            return result.IsValid ? null : result.ErrorMessage;
        }
    }

    /// <summary>当前路径是否合法。</summary>
    public bool IsPathValid => !string.IsNullOrEmpty(VirtualPath) && VirtualPathValidator.Validate(VirtualPath).IsValid;

    /// <summary>对话框是否已确认（用于 View 关闭判断）。</summary>
    public bool IsConfirmed { get; private set; }

    /// <summary>
    /// 确认命令：验证通过后标记为已确认。
    /// </summary>
    [RelayCommand(CanExecute = nameof(IsPathValid))]
    private void Confirm()
    {
        IsConfirmed = true;
        DialogCloseRequested?.Invoke(true);
    }

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
    /// 可选内容类型列表（供 ComboBox 绑定）。
    /// </summary>
    public static IReadOnlyList<string> ContentTypes { get; } = ["json", "text", "raw"];
}
