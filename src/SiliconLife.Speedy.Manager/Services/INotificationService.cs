namespace SiliconLife.Speedy.Manager.Services;

/// <summary>
/// 通知服务接口，抽象用户提示对话框操作。
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// 显示错误提示对话框。
    /// </summary>
    /// <param name="title">对话框标题。</param>
    /// <param name="message">错误消息内容。</param>
    void ShowError(string title, string message);

    /// <summary>
    /// 显示信息提示对话框。
    /// </summary>
    /// <param name="title">对话框标题。</param>
    /// <param name="message">信息内容。</param>
    void ShowInfo(string title, string message);

    /// <summary>
    /// 显示确认对话框，等待用户选择。
    /// </summary>
    /// <param name="title">对话框标题。</param>
    /// <param name="message">确认消息内容。</param>
    /// <returns>用户点击"确认"时返回 true，点击"取消"时返回 false。</returns>
    Task<bool> ShowConfirmAsync(string title, string message);
}
