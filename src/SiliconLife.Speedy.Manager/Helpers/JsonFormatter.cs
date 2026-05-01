using System.Text;
using System.Text.Json;

namespace SiliconLife.Speedy.Manager.Helpers;

/// <summary>
/// JSON 格式化与验证工具（对应需求 3.1, 5.2）
/// </summary>
public static class JsonFormatter
{
    private static readonly JsonSerializerOptions _writeOptions = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// 将 JSON 字节数组格式化为缩进文本。
    /// </summary>
    /// <param name="data">原始 JSON 字节数组（UTF-8 编码）</param>
    /// <param name="formatted">格式化后的 JSON 字符串；失败时为空字符串</param>
    /// <param name="error">错误信息；成功时为 null</param>
    /// <returns>格式化是否成功</returns>
    public static bool TryFormat(byte[] data, out string formatted, out string? error)
    {
        if (data is null || data.Length == 0)
        {
            formatted = string.Empty;
            error = "输入数据为空。";
            return false;
        }

        try
        {
            // Parse then re-serialize with indentation to normalize formatting
            var document = JsonDocument.Parse(data);
            formatted = JsonSerializer.Serialize(document.RootElement, _writeOptions);
            error = null;
            return true;
        }
        catch (JsonException ex)
        {
            formatted = string.Empty;
            error = $"JSON 语法错误：{ex.Message}";
            return false;
        }
        catch (Exception ex)
        {
            formatted = string.Empty;
            error = $"格式化失败：{ex.Message}";
            return false;
        }
    }

    /// <summary>
    /// 验证字符串是否为合法的 JSON 语法。
    /// </summary>
    /// <param name="json">待验证的 JSON 字符串</param>
    /// <returns>(IsValid, Error) 元组；合法时 Error 为 null</returns>
    public static (bool IsValid, string? Error) Validate(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return (false, "JSON 字符串不能为空。");

        try
        {
            JsonDocument.Parse(json);
            return (true, null);
        }
        catch (JsonException ex)
        {
            return (false, $"JSON 语法错误：{ex.Message}");
        }
    }
}
