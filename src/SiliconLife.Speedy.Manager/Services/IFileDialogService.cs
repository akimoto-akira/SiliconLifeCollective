namespace SiliconLife.Speedy.Manager.Services;

/// <summary>
/// 文件对话框服务接口，抽象 WPF 系统文件对话框操作。
/// </summary>
public interface IFileDialogService
{
    /// <summary>
    /// 打开 .spk 文件选择对话框。
    /// </summary>
    /// <returns>用户选择的文件路径，取消时返回 null。</returns>
    string? OpenSpkFile();

    /// <summary>
    /// 打开文件保存对话框。
    /// </summary>
    /// <param name="defaultFileName">默认文件名（不含扩展名）。</param>
    /// <param name="defaultExtension">默认扩展名（如 ".json"）。</param>
    /// <returns>用户指定的保存路径，取消时返回 null。</returns>
    string? SaveFile(string defaultFileName, string defaultExtension);

    /// <summary>
    /// 打开目录选择对话框。
    /// </summary>
    /// <returns>用户选择的目录路径，取消时返回 null。</returns>
    string? SelectDirectory();

    /// <summary>
    /// 打开多文件选择对话框。
    /// </summary>
    /// <returns>用户选择的文件路径列表，取消时返回空列表。</returns>
    IReadOnlyList<string> OpenMultipleFiles();
}
