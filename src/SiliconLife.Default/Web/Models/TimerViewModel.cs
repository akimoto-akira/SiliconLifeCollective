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

public class TimerViewModel : ViewModelBase
{
    public List<TimerItemModel> Timers { get; set; } = new();
    public Guid? BeingId { get; set; }
    public string BeingName { get; set; } = string.Empty;
}

public class TimerItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = "once";
    public string Status { get; set; } = "active";
    public DateTime TriggerTime { get; set; }
    public string Interval { get; set; } = string.Empty;
    public int TimesTriggered { get; set; }
    public int? MaxTriggers { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastTriggeredAt { get; set; }
}
