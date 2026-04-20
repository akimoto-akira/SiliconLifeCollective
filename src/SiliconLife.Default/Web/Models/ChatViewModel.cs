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

namespace SiliconLife.Default.Web.Models;

public class ChatViewModel : ViewModelBase
{
    public List<ChatSessionItem> Sessions { get; set; } = new();
    public List<ChatMessage> Messages { get; set; } = new();
    public Guid UserId { get; set; }
    public Guid? CurrentBeingId { get; set; }
    public string CurrentBeingName { get; set; } = string.Empty;

    /// <summary>
    /// Maps tool Name (e.g. "calendar") to its localized display name.
    /// Populated at render time by ChatController; used by ChatView to inject a
    /// client-side lookup table so the frontend never needs to call the server again.
    /// </summary>
    public Dictionary<string, string> ToolDisplayNames { get; set; } = new();
}

public class ChatSessionItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastMessage { get; set; } = string.Empty;
    public DateTime LastMessageAt { get; set; }
    public int UnreadCount { get; set; }
}
