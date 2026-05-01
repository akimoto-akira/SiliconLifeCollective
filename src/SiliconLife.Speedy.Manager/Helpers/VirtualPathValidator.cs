namespace SiliconLife.Speedy.Manager.Helpers;

/// <summary>
/// 虚拟路径验证结果
/// </summary>
public record ValidationResult(bool IsValid, string? ErrorMessage)
{
    public static ValidationResult Success() => new(true, null);
    public static ValidationResult Failure(string errorMessage) => new(false, errorMessage);
}

/// <summary>
/// 验证 Virtual_Path 的合法性（对应需求 4.2）
/// </summary>
public static class VirtualPathValidator
{
    /// <summary>
    /// 验证虚拟路径是否合法。
    /// 规则：
    /// 1. 不能为空或纯空白
    /// 2. 不能包含反斜杠 \
    /// 3. 不能包含路径遍历段 ..
    /// 4. 不能以 / 开头或结尾
    /// 5. 不能包含连续的 //
    /// </summary>
    public static ValidationResult Validate(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return ValidationResult.Failure("路径不能为空或纯空白。");

        if (path.Contains('\\'))
            return ValidationResult.Failure("路径不能包含反斜杠 \\。");

        if (path.Contains(".."))
            return ValidationResult.Failure("路径不能包含路径遍历段 ..。");

        if (path.StartsWith('/'))
            return ValidationResult.Failure("路径不能以 / 开头。");

        if (path.EndsWith('/'))
            return ValidationResult.Failure("路径不能以 / 结尾。");

        if (path.Contains("//"))
            return ValidationResult.Failure("路径不能包含连续的 //。");

        return ValidationResult.Success();
    }
}
