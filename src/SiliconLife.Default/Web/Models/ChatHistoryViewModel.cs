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

public class ChatHistoryListViewModel : ViewModelBase
{
    public Guid BeingId { get; set; }
    public string BeingName { get; set; } = string.Empty;
}

public class ChatHistoryDetailViewModel : ViewModelBase
{
    public Guid SessionId { get; set; }
    public Guid BeingId { get; set; }
    
    /// <summary>
    /// Maps tool Name (e.g. "calendar") to its localized display name.
    /// Populated at render time by ChatHistoryController; used by ChatHistoryDetailView to inject a
    /// client-side lookup table so the frontend can display localized tool names.
    /// </summary>
    public Dictionary<string, string> ToolDisplayNames { get; set; } = new();
}
