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

namespace SiliconLife.Fast.IM;

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
            _logger.Error(null, "Failed to send permission request: {0}, error: {1}, stack: {2}", 
                request.RequestId, ex.Message, ex.StackTrace);
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
            _logger.Debug(null, "HandleResponse called: userId={0}, allowed={1}, activeRequest={2}", 
                userId, allowed, request?.RequestId.ToString() ?? "null");

            // Validate response matches active request
            if (request == null || request.UserId != userId)
            {
                _logger.Warn(null, "Received permission response for wrong user or no active request. Expected user: {0}, got: {1}",
                    _activeRequest?.UserId, userId);
                return;
            }

            // Set result FIRST (before cancelling timeout, to avoid triggering HandleTimeout)
            var result = new AskPermissionResult
            {
                Allowed = allowed,
                AddToCache = addToCache,
                CacheDuration = cacheDuration
            };
            
            if (!request.Tcs.TrySetResult(result))
            {
                _logger.Warn(null, "Permission request already completed: {0}", request.RequestId);
                return;
            }

            // Dispose timeout to prevent any callback execution
            request.TimeoutCts.Dispose();

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
            _logger.Debug(null, "HandleTimeout called: request={0}, isActiveRequest={1}", 
                request.RequestId, (_activeRequest == request).ToString());

            // If this request is not the active one, it's already handled
            if (_activeRequest != request)
            {
                _logger.Debug(null, "HandleTimeout: request is not active, skipping");
                return;
            }

            _logger.Debug(null, "Permission request timed out: {0}", request.RequestId);

            // Set timeout result (denied) - use TrySetResult to avoid exception if already completed
            if (!request.Tcs.TrySetResult(new AskPermissionResult { Allowed = false }))
            {
                _logger.Debug(null, "Permission request already completed before timeout: {0}", request.RequestId);
                return;
            }

            // Dispose timeout
            request.TimeoutCts.Dispose();

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
            // Case 1: Has active request waiting for response → resend to new client
            if (_activeRequest != null && !_activeRequest.TimeoutCts.IsCancellationRequested)
            {
                _logger.Info(null, "Client connected, resending active permission request: {0}", _activeRequest.RequestId);
                _ = _sendAction();  // Resend active request
                return;
            }

            // Case 2: No active request, but queue has pending items → start processing
            if (_pendingQueue.Count > 0 && !_isProcessing)
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
