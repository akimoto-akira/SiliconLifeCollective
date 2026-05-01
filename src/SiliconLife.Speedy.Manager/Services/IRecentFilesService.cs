namespace SiliconLife.Speedy.Manager.Services;

/// <summary>
/// 最近文件列表服务接口，管理最近打开的 .spk 文件记录。
/// </summary>
public interface IRecentFilesService
{
    /// <summary>
    /// 获取最近打开的文件路径列表，按最近访问时间降序排列，最多 10 条。
    /// </summary>
    IReadOnlyList<string> GetRecentFiles();

    /// <summary>
    /// 添加一条最近文件记录。若路径已存在则移至首位；超过 10 条时移除最旧记录。
    /// </summary>
    /// <param name="path">文件路径。</param>
    void AddRecentFile(string path);

    /// <summary>
    /// 移除指定路径的最近文件记录。
    /// </summary>
    /// <param name="path">文件路径。</param>
    void RemoveRecentFile(string path);
}
