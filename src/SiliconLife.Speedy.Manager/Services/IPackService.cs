using SiliconLife.Speedy.Manager.Models;

namespace SiliconLife.Speedy.Manager.Services;

/// <summary>
/// 封装 SpeedyPack API 的服务接口，管理 .spk 文件的生命周期与所有 CRUD 操作。
/// </summary>
public interface IPackService
{
    // ─── 状态属性 ─────────────────────────────────────────────────────────────

    /// <summary>当前是否已打开 Pack 文件。</summary>
    bool IsOpen { get; }

    /// <summary>当前 Pack 文件是否以只读模式打开。</summary>
    bool IsReadOnly { get; }

    /// <summary>当前打开的 Pack 文件路径，未打开时为 null。</summary>
    string? CurrentFilePath { get; }

    // ─── 文件操作 ─────────────────────────────────────────────────────────────

    /// <summary>
    /// 打开指定路径的 .spk 文件。
    /// </summary>
    /// <param name="filePath">文件路径。</param>
    /// <param name="readOnly">是否以只读模式打开。</param>
    /// <exception cref="PackOpenException">文件格式无效或无法访问时抛出。</exception>
    Task OpenAsync(string filePath, bool readOnly = false);

    /// <summary>
    /// 关闭当前打开的 Pack 文件，先执行 Flush 确保数据持久化。
    /// </summary>
    Task CloseAsync();

    // ─── 目录浏览 ─────────────────────────────────────────────────────────────

    /// <summary>返回指定目录下的直接子条目路径列表。</summary>
    IReadOnlyList<string> ListEntries(string directoryPath = "");

    /// <summary>返回指定目录下的直接子目录路径列表。</summary>
    IReadOnlyList<string> ListDirectories(string directoryPath = "");

    // ─── 条目 CRUD ────────────────────────────────────────────────────────────

    /// <summary>读取指定路径的条目原始字节，不存在时返回 null。</summary>
    byte[]? ReadEntry(string path);

    /// <summary>将字节数据写入指定路径，同时指定内容类型。</summary>
    void WriteEntry(string path, byte[] data, string contentType);

    /// <summary>删除指定路径的条目。</summary>
    void DeleteEntry(string path);

    /// <summary>判断指定路径的条目是否存在。</summary>
    bool EntryExists(string path);

    // ─── 元数据 ───────────────────────────────────────────────────────────────

    /// <summary>获取指定路径条目的元数据，不存在时返回 null。</summary>
    EntryMetadata? GetMetadata(string path);

    /// <summary>获取当前 Pack 文件的整体信息与统计数据。</summary>
    PackFileInfo GetFileInfo();

    // ─── 批量操作 ─────────────────────────────────────────────────────────────

    /// <summary>
    /// 将多个文件系统文件批量导入为 Pack 条目，使用事务保证原子性。
    /// </summary>
    /// <param name="files">文件路径与目标虚拟路径的映射集合。</param>
    Task ImportFilesAsync(IEnumerable<(string filePath, string virtualPath)> files);

    /// <summary>将指定虚拟路径的条目导出到文件系统。</summary>
    Task ExportEntryAsync(string virtualPath, string targetFilePath);

    /// <summary>将指定虚拟目录下的所有条目批量导出到文件系统目录，保留层级结构。</summary>
    Task ExportDirectoryAsync(string virtualDirectoryPath, string targetDirectory);

    // ─── 维护操作 ─────────────────────────────────────────────────────────────

    /// <summary>等待所有待写入操作持久化到磁盘。</summary>
    Task FlushAsync();

    /// <summary>压缩 Pack 文件，清除已删除条目占用的磁盘空间。</summary>
    Task CompactAsync();
}
