using System.Windows;

namespace SiliconLife.Speedy.Manager.Services;

/// <summary>
/// 使用 WPF MessageBox 实现的通知服务。
/// </summary>
public sealed class NotificationService : INotificationService
{
    /// <inheritdoc/>
    public void ShowError(string title, string message)
    {
        MessageBox.Show(
            message,
            title,
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }

    /// <inheritdoc/>
    public void ShowInfo(string title, string message)
    {
        MessageBox.Show(
            message,
            title,
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    /// <inheritdoc/>
    public Task<bool> ShowConfirmAsync(string title, string message)
    {
        var result = MessageBox.Show(
            message,
            title,
            MessageBoxButton.OKCancel,
            MessageBoxImage.Question);

        return Task.FromResult(result == MessageBoxResult.OK);
    }
}
