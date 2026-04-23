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
using SiliconLife.Default.Web.Models;
using SiliconLife.Default.Web.Views;

namespace SiliconLife.Default.Web;

[WebCode]
public class ChatHistoryController : Controller
{
    private readonly SiliconBeingManager _beingManager;
    private readonly ChatSystem _chatSystem;
    private readonly SkinManager _skinManager;
    private readonly Guid _userId;

    public ChatHistoryController()
    {
        var locator = ServiceLocator.Instance;
        _beingManager = locator.BeingManager!;
        _chatSystem = locator.ChatSystem!;
        _skinManager = locator.GetService<SkinManager>()!;
        _userId = Config.Instance.Data.UserGuid;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/chat-history";

        if (path == "/chat-history")
            Index();
        else if (path == "/chat-history-detail")
            Detail();
        else if (path == "/api/chat-history/conversations")
            GetConversations();
        else if (path == "/api/chat-history/messages")
            GetMessages();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var beingIdStr = Request.QueryString["beingId"];
        if (string.IsNullOrEmpty(beingIdStr) || !Guid.TryParse(beingIdStr, out var beingId))
        {
            Response.StatusCode = 400;
            RenderHtml("<h1>Invalid Being ID</h1>");
            return;
        }

        var being = _beingManager.GetBeing(beingId);
        if (being == null)
        {
            Response.StatusCode = 404;
            RenderHtml("<h1>Being Not Found</h1>");
            return;
        }

        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new ChatHistoryListView();
        var vm = new ChatHistoryListViewModel
        {
            Skin = skin,
            ActiveMenu = "beings",
            BeingId = beingId,
            BeingName = being.Name
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetConversations()
    {
        try
        {
            var beingIdStr = Request.QueryString["beingId"];
            if (string.IsNullOrEmpty(beingIdStr) || !Guid.TryParse(beingIdStr, out var beingId))
            {
                RenderJson(new { conversations = Array.Empty<object>() });
                return;
            }

            var sessions = _chatSystem.GetSessionsForUser(beingId, new[] { _userId });
            var beingDict = _beingManager.GetAllBeings().ToDictionary(b => b.Id);
            var userNickname = Config.Instance?.Data?.UserNickname ?? "User";

            var result = sessions.Select(s =>
            {
                var messages = s.GetMessages(0, 1);
                var lastMsg = messages.LastOrDefault();
                var otherMembers = s.Members.Where(m => m != beingId).ToList();

                string participants = string.Join(", ", otherMembers.Select(m =>
                {
                    if (m == _userId) return userNickname;
                    if (beingDict.TryGetValue(m, out var b)) return b.Name;
                    return m.ToString("N");
                }));

                return new
                {
                    id = s.Id.ToString(),
                    participants = participants,
                    lastMessage = lastMsg?.Content?.Substring(0, Math.Min(50, lastMsg.Content.Length)) ?? "",
                    lastMessageTime = lastMsg?.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    messageCount = s.GetMessages(0, int.MaxValue).Count
                };
            }).OrderByDescending(c => c.lastMessageTime).ToList();

            RenderJson(new { conversations = result });
        }
        catch (Exception ex)
        {
            RenderJson(new { conversations = Array.Empty<object>(), error = ex.Message });
        }
    }

    private void Detail()
    {
        var sessionIdStr = Request.QueryString["sessionId"];
        var beingIdStr = Request.QueryString["beingId"];

        if (string.IsNullOrEmpty(sessionIdStr) || !Guid.TryParse(sessionIdStr, out var sessionId))
        {
            Response.StatusCode = 400;
            RenderHtml("<h1>Invalid Session ID</h1>");
            return;
        }

        var session = _chatSystem.GetSession(sessionId);
        if (session == null)
        {
            Response.StatusCode = 404;
            RenderHtml("<h1>Session Not Found</h1>");
            return;
        }

        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new ChatHistoryDetailView();
        
        // Get the being to access tool manager
        var being = _beingManager.GetBeing(Guid.TryParse(beingIdStr, out var bid) ? bid : Guid.Empty);
        
        // Build tool display names dictionary
        var toolDisplayNames = new Dictionary<string, string>();
        if (being != null)
        {
            var language = Config.Instance?.Data?.Language ?? Language.ZhCN;
            foreach (var toolName in being.ToolManager.GetToolNames())
            {
                if (toolDisplayNames.ContainsKey(toolName)) continue;
                var tool = being.ToolManager.GetTool(toolName);
                if (tool != null)
                    toolDisplayNames[toolName] = tool.GetDisplayName(language);
            }
        }
        
        var vm = new ChatHistoryDetailViewModel
        {
            Skin = skin,
            ActiveMenu = "beings",
            SessionId = sessionId,
            BeingId = Guid.TryParse(beingIdStr, out var bid2) ? bid2 : Guid.Empty,
            ToolDisplayNames = toolDisplayNames
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetMessages()
    {
        try
        {
            var sessionIdStr = Request.QueryString["sessionId"];
            if (string.IsNullOrEmpty(sessionIdStr) || !Guid.TryParse(sessionIdStr, out var sessionId))
            {
                RenderJson(new { messages = Array.Empty<object>() });
                return;
            }

            var session = _chatSystem.GetSession(sessionId);
            if (session == null)
            {
                RenderJson(new { messages = Array.Empty<object>() });
                return;
            }

            var beings = _beingManager.GetAllBeings();
            var beingDict = beings.ToDictionary(b => b.Id);
            var messages = session.GetMessages(0, int.MaxValue);

            // Pair tool calls with their results
            var toolCallMap = new Dictionary<string, int>(); // toolCallId -> index in result list
            var result = new List<dynamic>();

            foreach (var m in messages)
            {
                var userNickname = Config.Instance?.Data?.UserNickname ?? "User";
                var senderBeing = beingDict.TryGetValue(m.SenderId, out var b) ? b : null;
                var senderName = senderBeing?.Name ?? (m.SenderId == _userId ? userNickname : m.SenderId.ToString("N"));

                if (m.Role == MessageRole.Tool && !string.IsNullOrEmpty(m.ToolCallId))
                {
                    // This is a tool result, find matching tool call
                    if (toolCallMap.TryGetValue(m.ToolCallId, out var toolCallIndex))
                    {
                        // Merge tool call with result by creating a new object
                        var original = result[toolCallIndex];
                        result[toolCallIndex] = new
                        {
                            id = original.id,
                            senderId = original.senderId,
                            content = original.content,
                            thinking = original.thinking,
                            role = original.role,
                            senderName = original.senderName,
                            timestamp = original.timestamp,
                            toolCallsJson = original.toolCallsJson,
                            toolCallId = original.toolCallId,
                            toolResult = m.Content,
                            toolResultTimestamp = m.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                    }
                    else
                    {
                        // Tool result without matching call, show separately
                        result.Add(new
                        {
                            id = m.Id.ToString(),
                            senderId = m.SenderId.ToString(),
                            content = m.Content,
                            thinking = (string?)null,
                            role = "Tool",
                            senderName = senderName,
                            timestamp = m.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                            toolCallsJson = (string?)null,
                            toolCallId = m.ToolCallId,
                            toolResult = (string?)null,
                            toolResultTimestamp = (string?)null
                        });
                    }
                }
                else if (!string.IsNullOrEmpty(m.ToolCallsJson))
                {
                    // This is a tool call, store it for later merging
                    var toolCallData = new
                    {
                        id = m.Id.ToString(),
                        senderId = m.SenderId.ToString(),
                        content = m.Content,
                        thinking = m.Thinking,
                        role = m.Role.ToString(),
                        senderName = senderName,
                        timestamp = m.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                        toolCallsJson = m.ToolCallsJson,
                        toolCallId = (string?)null,
                        toolResult = (string?)null,
                        toolResultTimestamp = (string?)null
                    } as dynamic;

                    // Extract the first tool call ID from toolCallsJson for matching
                    var toolCallKey = m.Id.ToString(); // fallback to message ID
                    try
                    {
                        var toolCalls = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(m.ToolCallsJson);
                        if (toolCalls != null && toolCalls.Count > 0 && toolCalls[0].ContainsKey("Id"))
                        {
                            toolCallKey = toolCalls[0]["Id"]?.ToString() ?? m.Id.ToString();
                        }
                    }
                    catch
                    {
                        // If parsing fails, use message ID as fallback
                    }

                    toolCallMap[toolCallKey] = result.Count;
                    result.Add(toolCallData);
                }
                else
                {
                    // Regular message (User or Assistant without tool calls)
                    result.Add(new
                    {
                        id = m.Id.ToString(),
                        senderId = m.SenderId.ToString(),
                        content = m.Content,
                        thinking = m.Thinking,
                        role = m.Role.ToString(),
                        senderName = senderName,
                        timestamp = m.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                        toolCallsJson = (string?)null,
                        toolCallId = (string?)null,
                        toolResult = (string?)null,
                        toolResultTimestamp = (string?)null
                    });
                }
            }

            RenderJson(new { messages = result });
        }
        catch (Exception ex)
        {
            RenderJson(new { messages = Array.Empty<object>(), error = ex.Message });
        }
    }
}
