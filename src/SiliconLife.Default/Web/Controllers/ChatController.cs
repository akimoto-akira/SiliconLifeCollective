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

using System.Net;
using System.Text;
using System.Text.Json;
using SiliconLife.Collective;
using SiliconLife.Default.Web.Models;
using SiliconLife.Default.Web.Views;

namespace SiliconLife.Default.Web;

[WebCode]
public class ChatController : Controller
{
    private readonly SiliconBeingManager _beingManager;
    private readonly ChatSystem _chatSystem;
    private readonly SkinManager _skinManager;
    private readonly Guid _userId;
    private string _currentPath = "";

    public ChatController()
    {
        var locator = ServiceLocator.Instance;
        _beingManager = locator.BeingManager!;
        _chatSystem = locator.ChatSystem!;
        _skinManager = locator.GetService<SkinManager>()!;
        _userId = Config.Instance.Data.UserGuid;
    }

    public override void Handle()
    {
        _currentPath = Request.Url?.AbsolutePath ?? "/chat";

        if (_currentPath == "/chat" || _currentPath == "/chat/index")
        {
            Index();
        }
        else if (_currentPath == "/api/chat/conversations")
        {
            GetConversations();
        }
        else if (_currentPath == "/api/chat/messages")
        {
            GetMessages();
        }
        else if (_currentPath == "/api/chat/history")
        {
            GetHistory();
        }
        else if (_currentPath == "/api/chat/send")
        {
            SendMessage();
        }
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new ChatView();

        var vm = new ChatViewModel
        {
            Skin = skin,
            ActiveMenu = "chat",
            UserId = _userId,
        };

        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetConversations()
    {
        try
        {
            var conversations = new List<object>();
            var beings = _beingManager.GetAllBeings();
            var beingDict = beings.ToDictionary(b => b.Id);
            var userNickname = Config.Instance?.Data?.UserNickname ?? "User";

            Console.WriteLine($"[ChatController] GetConversations: {beings.Count} beings, userId={_userId}");

            foreach (var session in _chatSystem.GetSessionsForUser(_userId, _beingManager.GetAllBeings().Select(b => b.Id)))
            {
                var messages = session.GetMessages(0, 1);
                var lastMsg = messages.LastOrDefault();

                string displayName;
                string type;

                if (session.Type == SessionType.SingleChat)
                {
                    type = "single";
                    // Find the other participant and set partner name for session display
                    var otherId = session.Members.FirstOrDefault(id => id != _userId);
                    if (otherId != Guid.Empty && beingDict.TryGetValue(otherId, out var being))
                    {
                        var loc = LocalizationManager.Instance.GetLocalization(Config.Instance?.Data?.Language ?? Language.ZhCN);
                        displayName = loc.GetSingleChatDisplayName(being.Name);
                    }
                    else if (otherId != Guid.Empty)
                    {
                        displayName = userNickname;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    type = "group";
                    var memberNames = session.Members
                        .Select(id => id == _userId ? userNickname : beingDict.GetValueOrDefault(id)?.Name ?? id.ToString("N").Substring(0, 8))
                        .ToArray();
                    displayName = string.Join(", ", memberNames);
                }

                conversations.Add(new
                {
                    sessionId = session.Id.ToString(),
                    type,
                    displayName,
                    lastMessage = lastMsg?.Content ?? "",
                    lastTime = lastMsg?.Timestamp.ToString("HH:mm") ?? ""
                });
            }

            Console.WriteLine($"[ChatController] GetConversations: returning {conversations.Count} conversations");
            RenderJson(conversations);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ChatController] GetConversations ERROR: {ex}");
            RenderJson(new { error = ex.Message });
        }
    }

    private void GetMessages()
    {
        var beingIdStr = Request.QueryString["beingId"];

        if (string.IsNullOrEmpty(beingIdStr) || !Guid.TryParse(beingIdStr, out var beingId))
        {
            RenderJson(Array.Empty<object>());
            return;
        }

        var messages = _chatSystem.GetMessages(_userId, beingId);

        var result = messages.Select(m => new
        {
            id = m.Id.ToString(),
            senderId = m.SenderId.ToString(),
            content = m.Content,
            timestamp = m.Timestamp.ToString("HH:mm"),
            isOwn = m.SenderId == _userId
        });

        RenderJson(result);
    }

    private void GetHistory()
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
            var messages = session.GetMessages(0, 50);

            var result = messages.Select(m =>
            {
                var senderBeing = m.SenderId != _userId && beingDict.TryGetValue(m.SenderId, out var b) ? b : null;
                return new
                {
                    isUser = m.SenderId == _userId,
                    senderName = senderBeing?.Name ?? "",
                    text = m.Content,
                    thinking = m.Thinking,
                    toolCalls = string.IsNullOrEmpty(m.ToolCallsJson) ? null
                        : System.Text.Json.JsonSerializer.Deserialize<List<ToolCallInfo>>(m.ToolCallsJson),
                    time = m.Timestamp.ToString("HH:mm")
                };
            });

            RenderJson(new { messages = result });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ChatController] GetHistory ERROR: {ex}");
            RenderJson(new { messages = Array.Empty<object>(), error = ex.Message });
        }
    }

    private void SendMessage()
    {
        try
        {
            var body = GetJsonBody<SendRequest>();
            if (body == null || !Guid.TryParse(body.SessionId, out var sessionId) || string.IsNullOrEmpty(body.Message))
            {
                RenderJson(new { success = false, error = "Invalid request" });
                return;
            }

            var session = _chatSystem.GetSession(sessionId);
            if (session == null)
            {
                RenderJson(new { success = false, error = "Session not found" });
                return;
            }

            // Build message: receiver is the first other member in the session
            var receiverId = session.Members.FirstOrDefault(id => id != _userId);
            if (receiverId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "No receiver in session" });
                return;
            }

            _chatSystem.AddMessage(_userId, receiverId, body.Message);

            // Retrieve pending AI replies from beings in this session
            var reply = session.Members
                .Where(id => id != _userId)
                .SelectMany(beingId => _chatSystem.GetPendingMessages(beingId))
                .FirstOrDefault();

            RenderJson(new
            {
                success = true,
                reply = reply != null ? new
                {
                    isUser = false,
                    senderName = _beingManager.GetBeing(reply.SenderId)?.Name ?? "",
                    text = reply.Content,
                    thinking = reply.Thinking,
                    toolCalls = string.IsNullOrEmpty(reply.ToolCallsJson) ? null
                        : System.Text.Json.JsonSerializer.Deserialize<List<ToolCallInfo>>(reply.ToolCallsJson),
                    time = reply.Timestamp.ToString("HH:mm")
                } : null
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ChatController] SendMessage ERROR: {ex}");
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private class SendRequest
    {
        public string? SessionId { get; set; }
        public string? Message { get; set; }
    }

    private class ToolCallInfo
    {
        public string Name { get; set; } = "";
        public string? Target { get; set; }
        public bool Success { get; set; }
    }
}
