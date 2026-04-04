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

namespace SiliconLife.Default.Web;

[WebCode]
public class ChatController : Controller
{
    private readonly SiliconBeingManager _beingManager;
    private readonly ChatSystem _chatSystem;
    private readonly Guid _userId;
    private string _currentPath = "";

    public ChatController(SiliconBeingManager beingManager, ChatSystem chatSystem, Guid userId)
    {
        _beingManager = beingManager;
        _chatSystem = chatSystem;
        _userId = userId;
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
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var html = HtmlBuilder.Create()
            .DocType()
            .Html()
            .Head()
                .MetaCharset()
                .MetaViewport()
                .Title("Chat - Silicon Life Collective")
                .Style(GetChatStyles())
                .Script(GetChatScripts())
            .EndBlock()
            .Body()
                .Div()
                    .Class("chat-container")
                    .Div()
                        .Class("chat-sidebar")
                        .H2("硅基人")
                        .Div()
                            .Class("beings-list")
                            .Id("beings-list")
                        .EndBlock()
                    .EndBlock()
                    .Div()
                        .Class("chat-main")
                        .Div()
                            .Class("chat-header")
                            .Id("chat-header")
                            .Text("选择会话开始聊天")
                        .EndBlock()
                        .Div()
                            .Class("chat-messages")
                            .Id("chat-messages")
                        .EndBlock()
                        .Div()
                            .Class("chat-input-area")
                            .Textarea("message-input", "输入消息...")
                            .Div()
                                .Class("send-button")
                                .Button("发送")
                                .OnClick("sendMessage()")
                            .EndBlock()
                        .EndBlock()
                    .EndBlock()
                .EndBlock()
            .EndBlock()
            .Build();

        RenderHtml(html);
    }

    private void GetConversations()
    {
        var conversations = new List<object>();
        var beings = _beingManager.GetAllBeings();

        foreach (var being in beings)
        {
            var lastMessage = _chatSystem.GetMessages(_userId, being.UserId).LastOrDefault();
            
            conversations.Add(new
            {
                beingId = being.Id.ToString(),
                beingName = being.Name,
                lastMessage = lastMessage?.Content ?? "",
                lastTime = lastMessage?.Timestamp.ToString("HH:mm") ?? ""
            });
        }

        RenderJson(conversations);
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

    private Guid GetChannelGuid(Guid userId, Guid beingId)
    {
        var ids = new[] { userId, beingId }.OrderBy(x => x).ToArray();
        var combined = $"{ids[0]}-{ids[1]}";
        return Guid.ParseExact(combined.Replace("-", "").Substring(0, 32), "x");
    }

    private string GetChatStyles()
    {
        return @"
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body { font-family: Arial, sans-serif; background: #f5f5f5; height: 100vh; }
            .chat-container { display: flex; height: 100vh; }
            .chat-sidebar { width: 250px; background: white; border-right: 1px solid #ddd; padding: 20px; overflow-y: auto; }
            .chat-sidebar h2 { font-size: 18px; margin-bottom: 15px; color: #333; }
            .being-item { padding: 12px; border-radius: 8px; cursor: pointer; margin-bottom: 8px; transition: background 0.2s; }
            .being-item:hover { background: #f0f0f0; }
            .being-item.active { background: #e3f2fd; }
            .being-name { font-weight: bold; color: #333; }
            .being-last-message { font-size: 12px; color: #666; margin-top: 4px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
            .chat-main { flex: 1; display: flex; flex-direction: column; }
            .chat-header { padding: 15px 20px; background: white; border-bottom: 1px solid #ddd; font-size: 18px; font-weight: bold; }
            .chat-messages { flex: 1; overflow-y: auto; padding: 20px; display: flex; flex-direction: column; gap: 10px; }
            .message { max-width: 70%; padding: 10px 15px; border-radius: 12px; }
            .message.own { align-self: flex-end; background: #2196F3; color: white; }
            .message.other { align-self: flex-start; background: white; border: 1px solid #ddd; }
            .message-time { font-size: 10px; opacity: 0.7; margin-top: 4px; }
            .chat-input-area { padding: 15px 20px; background: white; border-top: 1px solid #ddd; display: flex; gap: 10px; }
            .chat-input-area textarea { flex: 1; padding: 10px; border: 1px solid #ddd; border-radius: 8px; resize: none; font-family: inherit; }
            .chat-input-area button { padding: 10px 20px; background: #2196F3; color: white; border: none; border-radius: 8px; cursor: pointer; }
            .chat-input-area button:hover { background: #1976D2; }
        ";
    }

    private string GetChatScripts()
    {
        return $@"
            let currentBeingId = null;
            let ws = null;

            function connectWebSocket() {{
                ws = new WebSocket('ws://' + location.host + '/ws');
                ws.onmessage = function(event) {{
                    var data = JSON.parse(event.data);
                    if (data.type === 'chat' && data.senderId !== '{_userId}') {{
                        if (currentBeingId && data.senderId === currentBeingId) {{
                            addMessage(data.content, false, data.timestamp);
                        }}
                        loadConversations();
                    }}
                }};
            }}

            function loadConversations() {{
                fetch('/api/chat/conversations')
                    .then(r => r.json())
                    .then(data => {{
                        var list = document.getElementById('beings-list');
                        list.innerHTML = '';
                        data.forEach(c => {{
                            var div = document.createElement('div');
                            div.className = 'being-item' + (c.beingId === currentBeingId ? ' active' : '');
                            div.onclick = () => selectBeing(c.beingId, c.beingName);
                            div.innerHTML = '<div class=""being-name"">' + c.beingName + '</div><div class=""being-last-message"">' + (c.lastMessage || '暂无消息') + '</div>';
                            list.appendChild(div);
                        }});
                    }});
            }}

            function selectBeing(beingId, beingName) {{
                currentBeingId = beingId;
                document.getElementById('chat-header').textContent = beingName;
                document.getElementById('chat-messages').innerHTML = '';
                loadMessages(beingId);
                loadConversations();
            }}

            function loadMessages(beingId) {{
                fetch('/api/chat/messages?beingId=' + beingId)
                    .then(r => r.json())
                    .then(data => {{
                        var container = document.getElementById('chat-messages');
                        container.innerHTML = '';
                        data.forEach(m => {{
                            addMessage(m.content, m.isOwn, m.timestamp);
                        }});
                    }});
            }}

            function addMessage(content, isOwn, time) {{
                var container = document.getElementById('chat-messages');
                var div = document.createElement('div');
                div.className = 'message ' + (isOwn ? 'own' : 'other');
                div.innerHTML = content + '<div class=""message-time"">' + time + '</div>';
                container.appendChild(div);
                container.scrollTop = container.scrollHeight;
            }}

            function sendMessage() {{
                var input = document.getElementById('message-input');
                var content = input.value.trim();
                if (!content || !currentBeingId) return;

                var message = {{
                    type: 'chat',
                    senderId: '{_userId}',
                    receiverId: currentBeingId,
                    content: content,
                    timestamp: new Date().toLocaleTimeString('zh-CN', {{ hour: '2-digit', minute: '2-digit' }})
                }};

                ws.send(JSON.stringify(message));
                addMessage(content, true, message.timestamp);
                input.value = '';
            }}

            document.getElementById('message-input').addEventListener('keypress', function(e) {{
                if (e.key === 'Enter' && !e.shiftKey) {{
                    e.preventDefault();
                    sendMessage();
                }}
            }});

            window.onload = function() {{
                connectWebSocket();
                loadConversations();
            }};
        ";
    }
}
