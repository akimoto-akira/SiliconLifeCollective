using System.IO;
using SiliconLife.Speedy;
using SiliconLife.Speedy.Manager.Models;

namespace SiliconLife.Speedy.Manager.Services;

/// <summary>
/// 封装 SpeedyPack API，管理 .spk 文件实例的生命周期与所有 CRUD 操作。
/// </summary>
public sealed class PackService : IPackService, IDisposable
{
    private SpeedyPack? _pack;
    private bool _disposed;

    // ─── 状态属性 ─────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public bool IsOpen => _pack is not null;

    /// <inheritdoc/>
    public bool IsReadOnly { get; private set; }

    /// <inheritdoc/>
    public string? CurrentFilePath { get; private set; }

    // ─── 文件操作 ─────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task OpenAsync(string filePath, bool readOnly = false)
    {
        // 关闭已打开的文件
        if (_pack is not null)
            await CloseAsync().ConfigureAwait(false);

        try
        {
            var options = new SpeedyPackOptions { ReadOnly = readOnly };
            _pack = SpeedyPack.Open(filePath, options);
            CurrentFilePath = filePath;
            IsReadOnly = readOnly;
        }
        catch (InvalidDataException ex)
        {
            throw new PackOpenException(
                $"文件 '{filePath}' 不是有效的 .spk 格式。", ex);
        }
        catch (IOException ex)
        {
            throw new PackOpenException(
                $"无法打开文件 '{filePath}'：{ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task CloseAsync()
    {
        if (_pack is null) return;

        try
        {
            await _pack.FlushAsync().ConfigureAwait(false);
        }
        finally
        {
            _pack.Dispose();
            _pack = null;
            CurrentFilePath = null;
            IsReadOnly = false;
        }
    }

    // ─── 目录浏览 ─────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public IReadOnlyList<string> ListEntries(string directoryPath = "")
    {
        EnsureOpen();
        return _pack!.ListEntries(directoryPath);
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> ListDirectories(string directoryPath = "")
    {
        EnsureOpen();
        return _pack!.ListDirectories(directoryPath);
    }

    // ─── 条目 CRUD ────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public byte[]? ReadEntry(string path)
    {
        EnsureOpen();
        return _pack!.Read(path);
    }

    /// <inheritdoc/>
    public void WriteEntry(string path, byte[] data, string contentType)
    {
        EnsureOpen();
        _pack!.Write(path, data, contentType);
    }

    /// <inheritdoc/>
    public void DeleteEntry(string path)
    {
        EnsureOpen();
        _pack!.Delete(path);
    }

    /// <inheritdoc/>
    public bool EntryExists(string path)
    {
        EnsureOpen();
        return _pack!.Exists(path);
    }

    // ─── 元数据 ───────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public EntryMetadata? GetMetadata(string path)
    {
        EnsureOpen();
        var meta = _pack!.GetEntryMetadata(path);
        if (meta is null) return null;

        return new EntryMetadata(
            VirtualPath: path,
            ContentType: meta.Value.ContentType,
            Length: meta.Value.Length,
            CreatedAt: meta.Value.CreatedAt,
            UpdatedAt: meta.Value.UpdatedAt);
    }

    /// <inheritdoc/>
    public PackFileInfo GetFileInfo()
    {
        EnsureOpen();
        var info = _pack!.GetFileInfo();

        return new PackFileInfo(
            FilePath: info.FilePath,
            FileSize: info.FileSize,
            Magic: info.Magic,
            Version: info.Version,
            Flags: info.Flags,
            DirectoryOffset: info.DirectoryOffset,
            DirectoryLength: info.DirectoryLength,
            TotalEntries: info.TotalEntries,
            JsonEntries: info.JsonEntries,
            RawEntries: info.RawEntries,
            TextEntries: info.TextEntries);
    }

    // ─── 批量操作 ─────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task ImportFilesAsync(IEnumerable<(string filePath, string virtualPath)> files)
    {
        EnsureOpen();

        var fileList = files.ToList();
        using var transaction = _pack!.BeginTransaction();

        try
        {
            foreach (var (filePath, virtualPath) in fileList)
            {
                byte[] data;
                try
                {
                    data = await File.ReadAllBytesAsync(filePath).ConfigureAwait(false);
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    transaction.Rollback();
                    throw new IOException(
                        $"读取文件 '{filePath}' 失败：{ex.Message}", ex);
                }

                // 根据扩展名推断内容类型
                var contentType = InferContentType(filePath);
                transaction.Write(virtualPath, data);
            }

            transaction.Commit();
        }
        catch
        {
            if (!transaction.IsCommitted)
                transaction.Rollback();
            throw;
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task ExportEntryAsync(string virtualPath, string targetFilePath)
    {
        EnsureOpen();

        var data = _pack!.Read(virtualPath)
            ?? throw new InvalidOperationException($"条目 '{virtualPath}' 不存在。");

        var directory = Path.GetDirectoryName(targetFilePath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        await File.WriteAllBytesAsync(targetFilePath, data).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task ExportDirectoryAsync(string virtualDirectoryPath, string targetDirectory)
    {
        EnsureOpen();

        Directory.CreateDirectory(targetDirectory);

        // 递归收集所有条目
        var allEntries = CollectAllEntries(virtualDirectoryPath);

        foreach (var entryPath in allEntries)
        {
            var data = _pack!.Read(entryPath);
            if (data is null) continue;

            // 计算相对路径，保留层级结构
            var relativePath = string.IsNullOrEmpty(virtualDirectoryPath)
                ? entryPath
                : entryPath[(virtualDirectoryPath.Length + 1)..];

            // 将虚拟路径分隔符转换为系统路径分隔符
            var localRelativePath = relativePath.Replace('/', Path.DirectorySeparatorChar);
            var targetFilePath = Path.Combine(targetDirectory, localRelativePath);

            var fileDirectory = Path.GetDirectoryName(targetFilePath);
            if (!string.IsNullOrEmpty(fileDirectory))
                Directory.CreateDirectory(fileDirectory);

            await File.WriteAllBytesAsync(targetFilePath, data).ConfigureAwait(false);
        }
    }

    // ─── 维护操作 ─────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task FlushAsync()
    {
        EnsureOpen();
        return _pack!.FlushAsync();
    }

    /// <inheritdoc/>
    public Task CompactAsync()
    {
        EnsureOpen();
        return _pack!.CompactAsync();
    }

    // ─── IDisposable ──────────────────────────────────────────────────────────

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _pack?.Dispose();
        _pack = null;
    }

    // ─── 私有辅助方法 ─────────────────────────────────────────────────────────

    private void EnsureOpen()
    {
        if (_pack is null)
            throw new InvalidOperationException("未打开任何 Pack 文件。");
    }

    /// <summary>递归收集指定虚拟目录下的所有条目路径。</summary>
    private List<string> CollectAllEntries(string directoryPath)
    {
        var result = new List<string>();

        // 直接子条目
        result.AddRange(_pack!.ListEntries(directoryPath));

        // 递归子目录
        foreach (var subDir in _pack.ListDirectories(directoryPath))
            result.AddRange(CollectAllEntries(subDir));

        return result;
    }

    /// <summary>根据文件扩展名推断内容类型。</summary>
    private static string InferContentType(string filePath)
    {
        var ext = Path.GetExtension(filePath).ToLowerInvariant();
        return ext switch
        {
            ".json" => "json",
            ".txt" or ".md" or ".xml" or ".csv" or ".yaml" or ".yml" => "text",
            _ => "raw"
        };
    }
}
