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

namespace SiliconLife.Collective;

public interface IIMProvider
{
    event EventHandler<IMMessageEventArgs>? MessageReceived;
    event EventHandler<StreamChunkEventArgs>? StreamChunkReceived;
    event EventHandler? ExitRequested;

    Task StartAsync();
    Task StopAsync();
    Task SendMessageAsync(Guid senderId, Guid channelId, string content, string? thinking = null, string? senderName = null, int? promptTokens = null, int? completionTokens = null, int? totalTokens = null);

    /// <summary>
    /// Sends a streaming chunk to the channel.
    /// Used for real-time streaming responses like AI text generation.
    /// </summary>
    /// <param name="senderId">The sender ID</param>
    /// <param name="channelId">The channel ID</param>
    /// <param name="chunk">The stream chunk to send</param>
    Task SendStreamChunkAsync(Guid senderId, Guid channelId, StreamChunk chunk);

    /// <summary>
    /// Asks the user for a permission decision.
    /// Each IM subclass combines the structured parameters with its own localization
    /// to produce the user-facing prompt and collect the response.
    /// </summary>
    /// <param name="permissionType">The type of permission being requested</param>
    /// <param name="resource">The resource path (URL, file path, command, etc.)</param>
    /// <param name="allowCode">6-digit random code for allowing the operation</param>
    /// <param name="denyCode">6-digit random code for denying the operation</param>
    /// <returns>The user's decision result</returns>
    Task<AskPermissionResult> AskPermissionAsync(PermissionType permissionType, string resource, string allowCode, string denyCode);

    /// <summary>
    /// Sends a tool update event to the channel.
    /// Used for real-time notification when AI calls tools or tools return results.
    /// </summary>
    /// <param name="senderId">The sender ID</param>
    /// <param name="channelId">The channel ID</param>
    /// <param name="role">Message role (Assistant for tool call, Tool for tool result)</param>
    /// <param name="content">The message content</param>
    /// <param name="toolCallsJson">Serialized tool calls JSON (for Assistant role with tool calls)</param>
    /// <param name="toolCallId">Tool call ID (for Tool role result messages)</param>
    /// <param name="thinking">Optional thinking content</param>
    /// <param name="senderName">Optional sender name</param>
    Task SendToolUpdateAsync(Guid senderId, Guid channelId, string role, string content, string? toolCallsJson = null, string? toolCallId = null, string? thinking = null, string? senderName = null);
}

/// <summary>
/// Result of a permission ask interaction.
/// Carries both the decision and an optional cache preference.
/// </summary>
public class AskPermissionResult
{
    /// <summary>Whether the user allowed the operation</summary>
    public bool Allowed { get; init; }

    /// <summary>
    /// Whether the user chose to add this decision to the frequency cache.
    /// Only meaningful when the user actively selects this option.
    /// </summary>
    public bool AddToCache { get; init; }
}

public class IMMessageEventArgs : EventArgs
{
    public ChatMessage Message { get; }

    public IMMessageEventArgs(ChatMessage message)
    {
        Message = message;
    }
}

/// <summary>
/// Represents a single chunk in a streaming response.
/// Used for real-time streaming like AI text generation.
/// </summary>
public class StreamChunk
{
    /// <summary>
    /// Unique identifier for the stream session.
    /// All chunks in the same stream share this ID.
    /// </summary>
    public Guid StreamId { get; init; }

    /// <summary>
    /// The chunk content (partial text or data).
    /// </summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>
    /// Whether this is the final chunk of the stream.
    /// When true, the stream is complete.
    /// </summary>
    public bool IsFinal { get; init; }

    /// <summary>
    /// Optional thinking content (chain-of-thought) for this chunk.
    /// </summary>
    public string? Thinking { get; init; }

    /// <summary>
    /// Optional sequence number for ordering (0-based).
    /// </summary>
    public int? Sequence { get; init; }

    /// <summary>
    /// Optional token usage for the final chunk (only set on IsFinal=true chunks).
    /// </summary>
    public int? PromptTokens { get; init; }
    public int? CompletionTokens { get; init; }
    public int? TotalTokens { get; init; }

    /// <summary>
    /// Creates a start chunk to begin a new stream.
    /// </summary>
    public static StreamChunk Start(Guid streamId, string content = "", string? thinking = null)
    {
        return new StreamChunk
        {
            StreamId = streamId,
            Content = content,
            IsFinal = false,
            Thinking = thinking,
            Sequence = 0
        };
    }

    /// <summary>
    /// Creates a continuation chunk.
    /// </summary>
    public static StreamChunk Continue(Guid streamId, string content, int sequence, string? thinking = null)
    {
        return new StreamChunk
        {
            StreamId = streamId,
            Content = content,
            IsFinal = false,
            Thinking = thinking,
            Sequence = sequence
        };
    }

    /// <summary>
    /// Creates a final chunk to end the stream.
    /// </summary>
    public static StreamChunk End(Guid streamId, string content = "", int? sequence = null, string? thinking = null, int? promptTokens = null, int? completionTokens = null, int? totalTokens = null)
    {
        return new StreamChunk
        {
            StreamId = streamId,
            Content = content,
            IsFinal = true,
            Thinking = thinking,
            Sequence = sequence,
            PromptTokens = promptTokens,
            CompletionTokens = completionTokens,
            TotalTokens = totalTokens,
        };
    }
}

/// <summary>
/// Event arguments for stream chunk received events.
/// </summary>
public class StreamChunkEventArgs : EventArgs
{
    public Guid SenderId { get; }
    public Guid ChannelId { get; }
    public StreamChunk Chunk { get; }

    public StreamChunkEventArgs(Guid senderId, Guid channelId, StreamChunk chunk)
    {
        SenderId = senderId;
        ChannelId = channelId;
        Chunk = chunk;
    }
}
