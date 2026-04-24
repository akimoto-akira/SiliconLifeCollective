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

namespace SiliconLife.Default.Web.Models;

/// <summary>
/// View model for timer execution history list
/// </summary>
public class TimerExecutionHistoryViewModel : ViewModelBase
{
    public Guid TimerId { get; set; }
    public string TimerName { get; set; } = "";
    public List<TimerExecutionItem> Executions { get; set; } = new();
}

/// <summary>
/// Timer execution item for list display
/// </summary>
public class TimerExecutionItem
{
    public string ExecutionId { get; set; } = "";
    public string TriggeredAt { get; set; } = "";
    public string? CompletedAt { get; set; }
    public string State { get; set; } = "";
    public int StepCount { get; set; }
    public int MessageCount { get; set; }
}

/// <summary>
/// View model for single timer execution detail (message history)
/// </summary>
public class TimerExecutionDetailViewModel : ViewModelBase
{
    public Guid TimerId { get; set; }
    public string ExecutionId { get; set; } = "";
    public string TimerName { get; set; } = "";
    public string TriggeredAt { get; set; } = "";
    public string? CompletedAt { get; set; }
    public string State { get; set; } = "";
    public List<Models.ChatMessage> Messages { get; set; } = new();
    public Dictionary<string, string> ToolDisplayNames { get; set; } = new();
}
