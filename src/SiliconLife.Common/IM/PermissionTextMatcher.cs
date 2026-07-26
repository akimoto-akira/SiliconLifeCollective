// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Text.RegularExpressions;

namespace SiliconLife.Common.IM;

/// <summary>
/// 权限码文本匹配工具。
/// 用于在不支持交互卡片的平台上，通过用户回复的文本中匹配6位数字码。
/// </summary>
internal static class PermissionTextMatcher
{
    /// <summary>
    /// 尝试从用户回复中匹配权限码。
    /// </summary>
    /// <param name="userReply">用户回复的文本</param>
    /// <param name="allowCode">允许码</param>
    /// <param name="denyCode">拒绝码</param>
    /// <param name="allowed">匹配到的结果（允许=true，拒绝=false）</param>
    /// <returns>是否成功匹配到任一码</returns>
    public static bool TryMatch(
        string userReply,
        string allowCode,
        string denyCode,
        out bool allowed)
    {
        allowed = false;
        if (string.IsNullOrWhiteSpace(userReply))
            return false;

        var match = Regex.Match(userReply, @"\d{6}");
        if (!match.Success)
            return false;

        string code = match.Value;
        if (code == allowCode) { allowed = true; return true; }
        if (code == denyCode) { allowed = false; return true; }
        return false;
    }
}