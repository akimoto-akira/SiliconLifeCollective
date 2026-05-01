namespace SiliconLife.Speedy.Manager.Services;

/// <summary>
/// 打开 .spk 文件失败时抛出的异常。
/// 封装底层的 InvalidDataException 或 IOException。
/// </summary>
public class PackOpenException : Exception
{
    public PackOpenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
