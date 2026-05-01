using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SiliconLife.Speedy.Manager.Services;

/// <summary>
/// 将最近文件列表持久化到 %APPDATA%\SiliconLife\SpeedyPackManager\recent-files.json 的服务实现。
/// 最多保留 10 条记录，按最近访问时间降序排列，重复路径自动去重并移至首位。
/// </summary>
public sealed class RecentFilesService : IRecentFilesService
{
    private const int MaxRecentFiles = 10;

    private static readonly string StoragePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SiliconLife",
        "SpeedyPackManager",
        "recent-files.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    // 内存中的有序列表（索引 0 = 最近访问）
    private readonly List<string> _files;
    private readonly object _lock = new();

    public RecentFilesService()
    {
        _files = LoadFromDisk();
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> GetRecentFiles()
    {
        lock (_lock)
        {
            return _files.ToList().AsReadOnly();
        }
    }

    /// <inheritdoc/>
    public void AddRecentFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return;

        lock (_lock)
        {
            // 去重：若已存在则先移除
            _files.RemoveAll(f => string.Equals(f, path, StringComparison.OrdinalIgnoreCase));

            // 插入到首位（最近访问）
            _files.Insert(0, path);

            // 超过上限时截断
            if (_files.Count > MaxRecentFiles)
                _files.RemoveRange(MaxRecentFiles, _files.Count - MaxRecentFiles);

            SaveToDisk();
        }
    }

    /// <inheritdoc/>
    public void RemoveRecentFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return;

        lock (_lock)
        {
            var removed = _files.RemoveAll(f =>
                string.Equals(f, path, StringComparison.OrdinalIgnoreCase));

            if (removed > 0)
                SaveToDisk();
        }
    }

    // ─── 私有辅助方法 ─────────────────────────────────────────────────────────

    private static List<string> LoadFromDisk()
    {
        try
        {
            if (!File.Exists(StoragePath))
                return new List<string>();

            var json = File.ReadAllText(StoragePath);
            var data = JsonSerializer.Deserialize<RecentFilesData>(json, JsonOptions);
            return data?.Files ?? new List<string>();
        }
        catch
        {
            // 文件损坏或读取失败时返回空列表，不影响应用启动
            return new List<string>();
        }
    }

    private void SaveToDisk()
    {
        try
        {
            var directory = Path.GetDirectoryName(StoragePath)!;
            Directory.CreateDirectory(directory);

            var data = new RecentFilesData { Files = new List<string>(_files) };
            var json = JsonSerializer.Serialize(data, JsonOptions);
            File.WriteAllText(StoragePath, json);
        }
        catch
        {
            // 写入失败时静默忽略，不影响主功能
        }
    }

    // ─── 序列化模型 ───────────────────────────────────────────────────────────

    private sealed class RecentFilesData
    {
        [JsonPropertyName("files")]
        public List<string> Files { get; set; } = new();
    }
}
