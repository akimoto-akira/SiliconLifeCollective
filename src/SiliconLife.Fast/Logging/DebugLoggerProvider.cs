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

using SiliconLife.Collective;
using SiliconLife.Common;

namespace SiliconLife.Fast.Logging;

/// <summary>
/// Debug 日志提供者，在 Debug 模式下将 Warn 及以上级别的日志输出到 Debug.Log
/// </summary>
public sealed class DebugLoggerProvider : ILoggerProvider
{
    private LogLevel _minimumLevel = LogLevel.Warning;

    public string Name => "Debug";

    public LogLevel MinimumLevel
    {
        get => _minimumLevel;
        set => _minimumLevel = value;
    }

    public DebugLoggerProvider()
    {
    }

    public void WriteLog(LogEntry entry)
    {
        if (entry == null || !IsEnabled(entry.Level))
            return;

#if DEBUG
        var debugMessage = $"[{entry.Level.ToString().ToUpperInvariant()}] [{entry.Category}] {entry.Message}";
        if (entry.Exception != null)
        {
            debugMessage += $"\nException: {entry.Exception}";
        }
        
        Debug.Log(debugMessage);
#endif
    }

    public bool IsEnabled(LogLevel level) =>
        level >= _minimumLevel && level != LogLevel.None;

    public void Flush() { }

    public void Dispose() { }
}
