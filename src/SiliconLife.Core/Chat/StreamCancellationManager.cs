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

using System.Collections.Concurrent;

namespace SiliconLife.Collective;

/// <summary>
/// Manages cancellation tokens for active AI stream responses.
/// Allows external cancellation of in-progress AI thinking by channel ID.
/// Thread-safe via <see cref="ConcurrentDictionary{TKey,TValue}"/>.
/// </summary>
public class StreamCancellationManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<StreamCancellationManager>();
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _activeTokens = new();

    /// <summary>
    /// Creates and registers a new <see cref="CancellationToken"/> for the given channel.
    /// If a token already exists for the channel (stale from a previous request), it is cancelled and disposed first.
    /// </summary>
    /// <param name="channelId">The channel ID to associate the token with.</param>
    /// <returns>A <see cref="CancellationToken"/> that will be cancelled when <see cref="CancelStream"/> is called.</returns>
    public CancellationToken CreateToken(Guid channelId)
    {
        // Cancel and dispose any stale token for this channel
        if (_activeTokens.TryRemove(channelId, out var oldCts))
        {
            try { oldCts.Cancel(); } catch { /* ignore */ }
            try { oldCts.Dispose(); } catch { /* ignore */ }
            _logger.Debug(null, "Replaced stale cancellation token for channel {0}", channelId);
        }

        var cts = new CancellationTokenSource();
        _activeTokens[channelId] = cts;
        _logger.Debug(null, "Created cancellation token for channel {0}", channelId);
        return cts.Token;
    }

    /// <summary>
    /// Attempts to cancel an active AI stream for the specified channel.
    /// Removes and disposes the cancellation token source.
    /// </summary>
    /// <param name="channelId">The channel ID whose stream should be cancelled.</param>
    /// <returns><c>true</c> if a token was found and cancelled; <c>false</c> otherwise.</returns>
    public bool CancelStream(Guid channelId)
    {
        if (_activeTokens.TryRemove(channelId, out var cts))
        {
            try
            {
                cts.Cancel();
                _logger.Info(null, "Stream cancelled for channel {0}", channelId);
            }
            catch (ObjectDisposedException)
            {
                _logger.Debug(null, "Token already disposed for channel {0}", channelId);
            }
            finally
            {
                cts.Dispose();
            }
            return true;
        }

        _logger.Debug(null, "No active token found for channel {0}", channelId);
        return false;
    }

    /// <summary>
    /// Marks the stream as completed (finished normally) and removes the token.
    /// Called when the AI finishes processing without being cancelled.
    /// </summary>
    /// <param name="channelId">The channel ID whose stream completed normally.</param>
    public void CompleteStream(Guid channelId)
    {
        if (_activeTokens.TryRemove(channelId, out var cts))
        {
            cts.Dispose();
            _logger.Debug(null, "Stream completed normally for channel {0}", channelId);
        }
    }

    /// <summary>
    /// Checks whether a channel currently has an active (in-progress) AI stream.
    /// </summary>
    /// <param name="channelId">The channel ID to check.</param>
    /// <returns><c>true</c> if there is an active token for this channel.</returns>
    public bool HasActiveStream(Guid channelId)
    {
        return _activeTokens.ContainsKey(channelId);
    }

    /// <summary>
    /// Cancels all active streams and disposes all tokens.
    /// Typically called during shutdown.
    /// </summary>
    public void CancelAll()
    {
        foreach (var kvp in _activeTokens)
        {
            try { kvp.Value.Cancel(); } catch { /* ignore */ }
            try { kvp.Value.Dispose(); } catch { /* ignore */ }
        }
        _activeTokens.Clear();
        _logger.Info(null, "All streams cancelled and tokens disposed");
    }
}
