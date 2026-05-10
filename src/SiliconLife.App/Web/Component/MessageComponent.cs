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

namespace SiliconLife.App.Web.Component;

/// <summary>
/// Message component for SSE push
/// </summary>
public class MessageComponent : ComponentBase
{
    private string _content = "";
    private string _sender = "";
    private DateTime? _time;
    private string _avatar = "";

    /// <summary>
    /// Set message content
    /// </summary>
    public MessageComponent Content(string content)
    {
        _content = content;
        return this;
    }

    /// <summary>
    /// Set sender name
    /// </summary>
    public MessageComponent Sender(string sender)
    {
        _sender = sender;
        return this;
    }

    /// <summary>
    /// Set message time
    /// </summary>
    public MessageComponent Time(DateTime time)
    {
        _time = time;
        return this;
    }

    /// <summary>
    /// Set avatar URL
    /// </summary>
    public MessageComponent Avatar(string avatarUrl)
    {
        _avatar = avatarUrl;
        return this;
    }

    public override string Render()
    {
        var message = H.Div();

        if (!string.IsNullOrEmpty(Id))
            message.Id(Id);

        var classes = new List<string> { "message" };
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        message.Class(string.Join(" ", classes));

        if (Style != null && Style.HasInlineStyles)
            message.Style(Style);

        foreach (var kvp in Attributes)
        {
            message.Attr(kvp.Key, kvp.Value);
        }

        // Avatar
        if (!string.IsNullOrEmpty(_avatar))
        {
            message.Add(H.Img()
                .Attr("src", H.Escape(_avatar))
                .Class("message-avatar")
                .Attr("alt", "avatar"));
        }

        // Message body
        var bodyDiv = H.Div().Class("message-body");

        // Sender and time
        if (!string.IsNullOrEmpty(_sender) || _time.HasValue)
        {
            var metaDiv = H.Div().Class("message-meta");
            if (!string.IsNullOrEmpty(_sender))
            {
                metaDiv.Add(H.Span(H.Escape(_sender)).Class("message-sender"));
            }
            if (_time.HasValue)
            {
                metaDiv.Add(H.Span(_time.Value.ToString("HH:mm:ss")).Class("message-time"));
            }
            bodyDiv.Add(metaDiv);
        }

        // Content
        bodyDiv.Add(H.Div(H.Escape(_content)).Class("message-content"));

        message.Add(bodyDiv);

        return message.Build();
    }

    /// <summary>
    /// Quickly build message HTML (for SSE push)
    /// </summary>
    public string Build()
    {
        return Render();
    }
}
