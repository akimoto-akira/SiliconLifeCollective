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
/// Tool call information for chat messages
/// </summary>
public class ToolCallInfo
{
    public string Name { get; set; } = "";
    public string? Target { get; set; }
    public bool Success { get; set; }
}

/// <summary>
/// Chat message component supporting user/AI messages, thinking, tool calls, and token stats
/// </summary>
public class ChatMessageComponent : ComponentBase
{
    private bool _isUser;
    private string _text = "";
    private string? _thinking;
    private string? _senderName;
    private string? _time;
    private List<ToolCallInfo>? _toolCalls;
    private int? _promptTokens;
    private int? _completionTokens;
    private int? _totalTokens;
    private string _userDisplayName = "User";
    private string _defaultBeingName = "AI";
    private string _thinkingSummary = "Thinking";
    private string _toolCallsSummaryFormat = "Tool Calls ({0})";

    /// <summary>
    /// Set whether this is a user message
    /// </summary>
    public ChatMessageComponent IsUser(bool isUser)
    {
        _isUser = isUser;
        return this;
    }

    /// <summary>
    /// Set message text content
    /// </summary>
    public ChatMessageComponent Text(string text)
    {
        _text = text;
        return this;
    }

    /// <summary>
    /// Set thinking content (AI only)
    /// </summary>
    public ChatMessageComponent Thinking(string? thinking)
    {
        _thinking = thinking;
        return this;
    }

    /// <summary>
    /// Set sender display name (AI only)
    /// </summary>
    public ChatMessageComponent SenderName(string? senderName)
    {
        _senderName = senderName;
        return this;
    }

    /// <summary>
    /// Set message time string
    /// </summary>
    public ChatMessageComponent Time(string? time)
    {
        _time = time;
        return this;
    }

    /// <summary>
    /// Set tool calls list (AI only)
    /// </summary>
    public ChatMessageComponent ToolCalls(List<ToolCallInfo>? toolCalls)
    {
        _toolCalls = toolCalls;
        return this;
    }

    /// <summary>
    /// Set prompt tokens count
    /// </summary>
    public ChatMessageComponent PromptTokens(int? count)
    {
        _promptTokens = count;
        return this;
    }

    /// <summary>
    /// Set completion tokens count
    /// </summary>
    public ChatMessageComponent CompletionTokens(int? count)
    {
        _completionTokens = count;
        return this;
    }

    /// <summary>
    /// Set total tokens count
    /// </summary>
    public ChatMessageComponent TotalTokens(int? count)
    {
        _totalTokens = count;
        return this;
    }

    /// <summary>
    /// Set user display name
    /// </summary>
    public ChatMessageComponent UserDisplayName(string name)
    {
        _userDisplayName = name;
        return this;
    }

    /// <summary>
    /// Set default being name
    /// </summary>
    public ChatMessageComponent DefaultBeingName(string name)
    {
        _defaultBeingName = name;
        return this;
    }

    /// <summary>
    /// Set thinking section summary text
    /// </summary>
    public ChatMessageComponent ThinkingSummary(string summary)
    {
        _thinkingSummary = summary;
        return this;
    }

    /// <summary>
    /// Set tool calls summary format (use {0} for count placeholder)
    /// </summary>
    public ChatMessageComponent ToolCallsSummaryFormat(string format)
    {
        _toolCallsSummaryFormat = format;
        return this;
    }

    public override H Render()
    {
        if (_isUser)
        {
            return RenderUserMessage();
        }
        return RenderBeingMessage();
    }

    private H RenderUserMessage()
    {
        var bubble = H.Div(_text).Class("msg-user-bubble");
        var content = H.Div(bubble).Class("msg-user-content");
        
        if (!string.IsNullOrEmpty(_time))
            content.Add(H.Div(_time).Class("msg-time"));
        
        var avatar = H.Div(
            H.Div("U").Class("msg-avatar-icon"),
            H.Div(_userDisplayName).Class("msg-avatar-name")
        ).Class("msg-user-avatar");
        
        var wrapper = H.Div(content, avatar).Class("msg-user");
        
        if (!string.IsNullOrEmpty(Id))
            wrapper.Id(Id);
        if (!string.IsNullOrEmpty(Class))
            wrapper.Class(Class);
        if (Style != null && Style.HasInlineStyles)
            wrapper.Style(Style);
        foreach (var kvp in Attributes)
            wrapper.Attr(kvp.Key, kvp.Value);
        
        return wrapper;
    }

    private H RenderBeingMessage()
    {
        var beingDisplayName = !string.IsNullOrEmpty(_senderName) ? _senderName : _defaultBeingName;
        var avatar = H.Div(
            H.Div(beingDisplayName.Substring(0, 1)).Class("msg-avatar-icon"),
            H.Div(beingDisplayName).Class("msg-avatar-name")
        ).Class("msg-being-avatar");

        var children = new List<object>();

        if (!string.IsNullOrEmpty(_senderName))
            children.Add(H.Div(_senderName).Class("msg-being-sender"));

        var bodyChildren = new List<object>();

        // Thinking content
        if (!string.IsNullOrEmpty(_thinking))
        {
            bodyChildren.Add(H.Details(
                H.Summary(_thinkingSummary),
                H.Div(_thinking!).Class("msg-thinking-content")
            ).Class("msg-collapsible"));
        }

        // Main text (markdown)
        if (!string.IsNullOrEmpty(_text))
            bodyChildren.Add(H.Div().Class("msg-being-text markdown-body").Data("md-raw", _text));

        // Tool calls
        if (_toolCalls != null && _toolCalls.Count > 0)
        {
            var toolItems = new List<object>();
            foreach (var tool in _toolCalls)
            {
                var icon = tool.Success ? "✓" : "✗";
                var cls = tool.Success ? "msg-tool-success" : "msg-tool-fail";
                var target = !string.IsNullOrEmpty(tool.Target) ? $" · {tool.Target}" : "";
                toolItems.Add(H.Div(
                    H.Span(icon).Class($"tool-status {cls}"),
                    H.Span($"{tool.Name}{target}")
                ).Class("msg-tool-item"));
            }
            
            var summaryText = string.Format(_toolCallsSummaryFormat, _toolCalls.Count);
            bodyChildren.Add(H.Details(
                H.Summary(summaryText),
                H.Div(toolItems.ToArray()).Class("msg-tools-list")
            ).Class("msg-collapsible"));
        }

        children.Add(H.Div(bodyChildren.ToArray()).Class("msg-being-body"));

        // Token stats
        if (_promptTokens.HasValue || _completionTokens.HasValue || _totalTokens.HasValue)
        {
            var prompt = _promptTokens ?? 0;
            var completion = _completionTokens ?? 0;
            var total = _totalTokens ?? 0;
            children.Add(H.Div($"Token: ↑{prompt} ↓{completion} Σ{total}").Class("msg-token-stats"));
        }

        // Time
        if (!string.IsNullOrEmpty(_time))
            children.Add(H.Div(_time).Class("msg-time"));

        var content = H.Div(
            H.Div(children.ToArray()).Class("msg-being-card")
        ).Class("msg-being-content");

        var wrapper = H.Div(avatar, content).Class("msg-being");
        
        if (!string.IsNullOrEmpty(Id))
            wrapper.Id(Id);
        if (!string.IsNullOrEmpty(Class))
            wrapper.Class(Class);
        if (Style != null && Style.HasInlineStyles)
            wrapper.Style(Style);
        foreach (var kvp in Attributes)
            wrapper.Attr(kvp.Key, kvp.Value);
        
        return wrapper;
    }

    public string Build() => Render().Build();
}
