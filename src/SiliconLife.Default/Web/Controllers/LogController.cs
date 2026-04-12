// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Text.Json;
using SiliconLife.Collective;
using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web;

[WebCode]
public class LogController : Controller
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    private const string LogFolderName = "Log";
    private const string DefaultKey = "default";
    private const int DefaultPageSize = 100;

    private readonly SkinManager _skinManager;

    public LogController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/logs";

        if (path == "/logs" || path == "/logs/index")
            Index();
        else if (path == "/api/logs/list")
            GetList();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.LogView();
        var vm = new Models.LogViewModel { Skin = skin, ActiveMenu = "logs" };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetList()
    {
        var levelFilter = GetQueryValue("level", "");
        var startDateStr = GetQueryValue("startDate", "");
        var endDateStr = GetQueryValue("endDate", "");
        var page = int.TryParse(GetQueryValue("page", "1"), out int p) ? p : 1;
        var pageSize = int.TryParse(GetQueryValue("pageSize", DefaultPageSize.ToString()), out int ps) ? ps : DefaultPageSize;

        var logs = new List<LogItem>();
        var logDirectory = Path.Combine(Config.Instance.Data.DataDirectory.FullName, LogFolderName, DefaultKey);
        
        if (!Directory.Exists(logDirectory))
        {
            RenderJson(new { logs = new List<LogItem>(), page = 1, pageSize, total = 0, totalPages = 0 });
            return;
        }

        DateTime? startTime = null;
        DateTime? endTime = null;
        
        if (!string.IsNullOrEmpty(startDateStr) && DateTime.TryParse(startDateStr, out var start))
        {
            startTime = start;
        }
        
        if (!string.IsNullOrEmpty(endDateStr) && DateTime.TryParse(endDateStr, out var end))
        {
            endTime = end;
        }

        foreach (string file in Directory.GetFiles(logDirectory, "*.json", SearchOption.AllDirectories))
        {
            try
            {
                if (!TryParseTimestampFromFile(file, out DateTime fileTime))
                    continue;

                if (startTime.HasValue && fileTime < startTime.Value)
                    continue;

                if (endTime.HasValue && fileTime > endTime.Value)
                    continue;

                var data = File.ReadAllBytes(file);
                var dto = JsonSerializer.Deserialize<LogEntryDto>(System.Text.Encoding.UTF8.GetString(data), JsonOptions);
                if (dto == null) continue;

                var levelStr = GetLevelString(dto.Level);

                if (!string.IsNullOrEmpty(levelFilter) && 
                    !string.Equals(levelStr, levelFilter, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                logs.Add(new LogItem
                {
                    Timestamp = dto.Timestamp,
                    Level = levelStr,
                    Category = dto.Category,
                    Message = dto.Message,
                    Details = dto.Exception
                });
            }
            catch
            {
                // Skip malformed entries
            }
        }

        logs = logs.OrderByDescending(l => l.Timestamp).ToList();

        var total = logs.Count;
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        var pagedLogs = logs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        RenderJson(new
        {
            logs = pagedLogs,
            page,
            pageSize,
            total,
            totalPages
        });
    }

    private static bool TryParseTimestampFromFile(string filePath, out DateTime result)
    {
        result = default;
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        if (!int.TryParse(fileName, out int second) || second is < 0 or > 59)
            return false;

        string? dir = Path.GetDirectoryName(filePath);
        if (dir == null) return false;

        if (!TryIntSegment(dir, 0, out int minute) || minute is < 0 or > 59) return false;
        if (!TryIntSegment(dir, 1, out int hour) || hour is < 0 or > 23) return false;
        if (!TryIntSegment(dir, 2, out int day) || day is < 1 or > 31) return false;
        if (!TryIntSegment(dir, 3, out int month) || month is < 1 or > 12) return false;
        if (!TryIntSegment(dir, 4, out int year)) return false;

        try
        {
            result = new DateTime(year, month, day, hour, minute, second);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string GetLevelString(object? level)
    {
        if (level == null) return "Unknown";

        if (level is JsonElement jsonElement)
        {
            if (jsonElement.ValueKind == JsonValueKind.Number && jsonElement.TryGetInt32(out int levelInt))
            {
                if (Enum.IsDefined(typeof(LogLevel), levelInt))
                    return ((LogLevel)levelInt).ToString();
            }
            return jsonElement.ToString() ?? "Unknown";
        }

        if (level is int intLevel && Enum.IsDefined(typeof(LogLevel), intLevel))
            return ((LogLevel)intLevel).ToString();

        if (level is LogLevel logLevel)
            return logLevel.ToString();

        return level.ToString() ?? "Unknown";
    }

    private static bool TryIntSegment(string path, int depth, out int value)
    {
        value = 0;
        for (int i = 0; i < depth; i++)
        {
            string? parent = Path.GetDirectoryName(path);
            if (parent == null) return false;
            path = parent;
        }
        return int.TryParse(Path.GetFileName(path), out value);
    }

    private sealed record LogEntryDto(
        DateTime Timestamp,
        object Level,
        string Category,
        string Message,
        string? Exception
    );
}
