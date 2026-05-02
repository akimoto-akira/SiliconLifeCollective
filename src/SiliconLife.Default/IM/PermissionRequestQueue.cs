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

using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Default.IM;

/// <summary>
/// Represents a pending permission request in the queue.
/// </summary>
internal class PendingPermissionRequest
{
    public Guid RequestId { get; set; }
    public Guid UserId { get; set; }
    public PermissionType PermissionType { get; set; }
    public string Resource { get; set; } = string.Empty;
    public string AllowCode { get; set; } = string.Empty;
    public string DenyCode { get; set; } = string.Empty;
    public TaskCompletionSource<AskPermissionResult> Tcs { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public CancellationTokenSource TimeoutCts { get; set; } = new();
}

/// <summary>
/// Permission request queue manager.
/// Ensures permission requests are processed sequentially and handles timeout/cleanup.
/// </summary>
internal class PermissionRequestQueue
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<PermissionRequestQueue>();
    private readonly Queue<PendingPermissionRequest> _pendingQueue = new();
    private readonly object _lock = new();
    private readonly Func<Task> _sendAction;
    private PendingPermissionRequest? _activeRequest;
    private bool _isProcessing;

    /// <summary>
    /// Creates a new permission request queue.
    /// </summary>
    /// <param name="sendAction">Action to send the active request to frontend</param>
    public PermissionRequestQueue(Func<Task> sendAction)
    {
        _sendAction = sendAction ?? throw new ArgumentNullException(nameof(sendAction));
    }

    /// <summary>
    /// Enqueues a permission request and waits for the result.
    /// </summary>
    public async Task<AskPermissionResult> EnqueueAsync(PendingPermissionRequest request)
    {
        lock (_lock)
        {
            _pendingQueue.Enqueue(request);
            _logger.Debug(null, "Permission request enqueued: {0}, queue size: {1}", 
                request.RequestId, _pendingQueue.Count);

            // If no active request, start processing
            if (_activeRequest == null && !_isProcessing)
            {
                _ = ProcessNextAsync();
            }
        }

        // Wait for the request to complete
        return await request.Tcs.Task;
    }

    /// <summary>
    /// Processes the next request in the queue.
    /// </summary>
    private async Task ProcessNextAsync()
    {
        PendingPermissionRequest? request;

        lock (_lock)
        {
            if (_pendingQueue.Count == 0)
            {
                _isProcessing = false;
                return;
            }

            _isProcessing = true;
            request = _pendingQueue.Dequeue();
            _activeRequest = request;
        }

        // Check if already timed out
        if (request.TimeoutCts.IsCancellationRequested)
        {
            _logger.Debug(null, "Permission request already timed out: {0}", request.RequestId);
            HandleTimeout(request);
            await ProcessNextAsync();
            return;
        }

        // Send to frontend
        try
        {
            await _sendAction();
            _logger.Info(null, "Permission request sent to frontend: {0}", request.RequestId);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to send permission request: {0}, error: {1}", 
                request.RequestId, ex.Message);
            HandleTimeout(request);
            await ProcessNextAsync();
        }
    }

    /// <summary>
    /// Handles user response to the active permission request.
    /// </summary>
    public void HandleResponse(Guid userId, bool allowed, bool addToCache, TimeSpan? cacheDuration)
    {
        PendingPermissionRequest? request;

        lock (_lock)
        {
            request = _activeRequest;

            // Validate response matches active request
            if (request == null || request.UserId != userId)
            {
                _logger.Warn(null, "Received permission response for wrong user or no active request. Expected user: {0}, got: {1}",
                    _activeRequest?.UserId, userId);
                return;
            }

            // Cancel timeout timer
            request.TimeoutCts.Cancel();

            // Set result
            var result = new AskPermissionResult
            {
                Allowed = allowed,
                AddToCache = addToCache,
                CacheDuration = cacheDuration
            };
            request.Tcs.SetResult(result);

            // Clear active request
            _activeRequest = null;

            _logger.Info(null, "Permission request resolved: {0}, allowed: {1}", request.RequestId, allowed);
        }

        // Process next request in queue
        _ = ProcessNextAsync();
    }

    /// <summary>
    /// Handles timeout for a permission request.
    /// </summary>
    public void HandleTimeout(PendingPermissionRequest request)
    {
        lock (_lock)
        {
            // If this request is not the active one, it's already handled
            if (_activeRequest != request)
            {
                return;
            }

            _logger.Warn(null, "Permission request timed out: {0}", request.RequestId);

            // Set timeout result (denied)
            request.Tcs.SetResult(new AskPermissionResult { Allowed = false });

            // Clear active request
            _activeRequest = null;
        }

        // Process next request in queue
        _ = ProcessNextAsync();
    }

    /// <summary>
    /// Called when a client connects via SSE. Triggers queue processing if there are pending requests.
    /// </summary>
    public void OnClientConnected()
    {
        lock (_lock)
        {
            // If there's no active request and queue has items, start processing
            if (_activeRequest == null && _pendingQueue.Count > 0 && !_isProcessing)
            {
                _logger.Info(null, "Client connected, triggering permission queue processing");
                _ = ProcessNextAsync();
            }
        }
    }

    /// <summary>
    /// Gets the current active request (for sending to frontend).
    /// </summary>
    public PendingPermissionRequest? GetActiveRequest()
    {
        lock (_lock)
        {
            return _activeRequest;
        }
    }

    /// <summary>
    /// Gets the current queue size.
    /// </summary>
    public int GetQueueSize()
    {
        lock (_lock)
        {
            return _pendingQueue.Count;
        }
    }
}
