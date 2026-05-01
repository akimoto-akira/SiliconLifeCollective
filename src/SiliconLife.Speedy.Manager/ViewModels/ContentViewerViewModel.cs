using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SiliconLife.Speedy.Manager.Helpers;
using SiliconLife.Speedy.Manager.Services;

namespace SiliconLife.Speedy.Manager.ViewModels;

/// <summary>
/// 内容查看器 ViewModel，负责显示和编辑 Pack 条目内容。
/// 对应需求 3.1, 3.2, 3.4, 3.5, 5.1, 5.2, 5.4。
/// </summary>
public partial class ContentViewerViewModel : ObservableObject
{
    private const int MaxDisplayBytes = 1 * 1024 * 1024; // 1 MB

    private readonly IPackService _packService;
    private readonly INotificationService _notificationService;

    // 当前加载的条目信息
    private string _currentVirtualPath = string.Empty;
    private byte[]? _currentData;
    private string _currentContentType = string.Empty;

    // 进入编辑模式前的原始内容（用于取消编辑时恢复）
    private string _originalContent = string.Empty;

    public ContentViewerViewModel(IPackService packService, INotificationService notificationService)
    {
        _packService = packService;
        _notificationService = notificationService;
    }

    /// <summary>当前显示的内容文本（只读模式或编辑模式均使用此属性）。</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasContent))]
    private string _displayContent = string.Empty;

    /// <summary>是否处于编辑模式。</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EnterEditModeCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [NotifyCanExecuteChangedFor(nameof(CancelEditCommand))]
    private bool _isEditMode;

    /// <summary>编辑模式下是否有未保存的更改。</summary>
    [ObservableProperty]
    private bool _hasUnsavedChanges;

    /// <summary>当前是否以 JSON 格式化视图显示（false 表示十六进制/文本视图）。</summary>
    [ObservableProperty]
    private bool _isJsonView;

    /// <summary>内容是否因超过 1 MB 而被截断。</summary>
    [ObservableProperty]
    private bool _isTruncated;

    /// <summary>JSON 语法错误信息，无错误时为 null。</summary>
    [ObservableProperty]
    private string? _jsonError;

    /// <summary>是否有内容可显示。</summary>
    public bool HasContent => !string.IsNullOrEmpty(DisplayContent) || _currentData != null;

    /// <summary>
    /// 加载条目内容并根据 ContentType 决定默认视图。
    /// 超过 1 MB 时截断并设置 IsTruncated = true。
    /// </summary>
    /// <param name="virtualPath">条目虚拟路径。</param>
    /// <param name="data">条目原始字节数据。</param>
    /// <param name="contentType">内容类型（"json"、"raw"、"text"）。</param>
    public void LoadEntry(string virtualPath, byte[] data, string contentType)
    {
        // 如果处于编辑模式，先退出
        if (IsEditMode)
        {
            CancelEdit();
        }

        _currentVirtualPath = virtualPath;
        _currentContentType = contentType;

        // 处理截断
        if (data.Length > MaxDisplayBytes)
        {
            _currentData = data[..MaxDisplayBytes];
            IsTruncated = true;
        }
        else
        {
            _currentData = data;
            IsTruncated = false;
        }

        // 根据 ContentType 决定默认视图
        IsJsonView = contentType == "json";

        // 渲染内容
        RenderContent();

        HasUnsavedChanges = false;
        JsonError = null;
    }

    /// <summary>
    /// 清空查看器内容（关闭文件或取消选中时调用）。
    /// </summary>
    public void Clear()
    {
        _currentVirtualPath = string.Empty;
        _currentData = null;
        _currentContentType = string.Empty;
        _originalContent = string.Empty;
        DisplayContent = string.Empty;
        IsEditMode = false;
        HasUnsavedChanges = false;
        IsJsonView = false;
        IsTruncated = false;
        JsonError = null;
    }

    /// <summary>
    /// 进入编辑模式，保存当前内容作为原始内容以便取消时恢复。
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanEnterEditMode))]
    private void EnterEditMode()
    {
        _originalContent = DisplayContent;
        IsEditMode = true;
        HasUnsavedChanges = false;
    }

    private bool CanEnterEditMode() => !IsEditMode && _currentData != null;

    /// <summary>
    /// 保存编辑内容到 Pack 文件。
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        if (!IsEditMode || string.IsNullOrEmpty(_currentVirtualPath))
            return;

        // JSON 内容保存前验证语法
        if (_currentContentType == "json")
        {
            var (isValid, error) = JsonFormatter.Validate(DisplayContent);
            if (!isValid)
            {
                JsonError = error;
                return;
            }
        }

        try
        {
            var bytes = Encoding.UTF8.GetBytes(DisplayContent);
            await Task.Run(() => _packService.WriteEntry(_currentVirtualPath, bytes, _currentContentType));

            // 更新内部数据
            _currentData = bytes;
            _originalContent = DisplayContent;
            IsEditMode = false;
            HasUnsavedChanges = false;
            JsonError = null;
        }
        catch (Exception ex)
        {
            _notificationService.ShowError("保存失败", $"无法保存条目内容：{ex.Message}");
        }
    }

    private bool CanSave() => IsEditMode;

    /// <summary>
    /// 取消编辑，恢复进入编辑模式前的原始内容。
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanCancelEdit))]
    private void CancelEdit()
    {
        DisplayContent = _originalContent;
        IsEditMode = false;
        HasUnsavedChanges = false;
        JsonError = null;
    }

    private bool CanCancelEdit() => IsEditMode;

    /// <summary>
    /// 在 JSON 格式化视图和原始字节/文本视图之间切换。
    /// </summary>
    [RelayCommand]
    private void ToggleView()
    {
        IsJsonView = !IsJsonView;
        RenderContent();
    }

    /// <summary>
    /// 当 DisplayContent 在编辑模式下变化时，更新 HasUnsavedChanges 和 JSON 验证状态。
    /// </summary>
    partial void OnDisplayContentChanged(string value)
    {
        if (IsEditMode)
        {
            HasUnsavedChanges = value != _originalContent;

            // 实时 JSON 语法验证
            if (_currentContentType == "json")
            {
                var (isValid, error) = JsonFormatter.Validate(value);
                JsonError = isValid ? null : error;
            }
        }
    }

    /// <summary>
    /// 根据当前视图模式（JSON 或十六进制/文本）渲染内容到 DisplayContent。
    /// </summary>
    private void RenderContent()
    {
        if (_currentData == null || _currentData.Length == 0)
        {
            DisplayContent = string.Empty;
            return;
        }

        if (IsJsonView)
        {
            RenderJsonView();
        }
        else
        {
            RenderRawView();
        }
    }

    private void RenderJsonView()
    {
        if (_currentData == null) return;

        if (JsonFormatter.TryFormat(_currentData, out var formatted, out var error))
        {
            DisplayContent = formatted;
            JsonError = null;
        }
        else
        {
            // JSON 格式化失败时回退到 UTF-8 文本
            DisplayContent = TryDecodeUtf8(_currentData);
            JsonError = error;
        }
    }

    private void RenderRawView()
    {
        if (_currentData == null) return;

        if (_currentContentType == "text")
        {
            // text 类型优先显示 UTF-8 文本
            DisplayContent = TryDecodeUtf8(_currentData);
        }
        else
        {
            // raw 类型显示十六进制字节视图
            DisplayContent = FormatHex(_currentData);
        }
    }

    private static string TryDecodeUtf8(byte[] data)
    {
        try
        {
            return Encoding.UTF8.GetString(data);
        }
        catch
        {
            return FormatHex(data);
        }
    }

    private static string FormatHex(byte[] data)
    {
        const int bytesPerLine = 16;
        var sb = new StringBuilder();

        for (int i = 0; i < data.Length; i += bytesPerLine)
        {
            // 偏移量
            sb.Append($"{i:X8}  ");

            // 十六进制字节
            int lineEnd = Math.Min(i + bytesPerLine, data.Length);
            for (int j = i; j < lineEnd; j++)
            {
                sb.Append($"{data[j]:X2} ");
                if (j == i + 7) sb.Append(' '); // 中间分隔
            }

            // 补齐不足一行的空格
            int missing = bytesPerLine - (lineEnd - i);
            sb.Append(new string(' ', missing * 3 + (missing > 8 ? 1 : 0)));

            // ASCII 可打印字符
            sb.Append(" |");
            for (int j = i; j < lineEnd; j++)
            {
                char c = (char)data[j];
                sb.Append(c >= 0x20 && c < 0x7F ? c : '.');
            }
            sb.AppendLine("|");
        }

        return sb.ToString();
    }
}
