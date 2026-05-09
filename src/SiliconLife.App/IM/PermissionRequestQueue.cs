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

namespace SiliconLife.App.IM;

/// <summary>
/// Represents a pending permission request in the queue.
/// </summary>
public class PendingPermissionRequest
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
    /// <param name="sendAction">Action to trigger UI update for permission requests</param>
    public PermissionRequestQueue(Func<Task> sendAction)
    {
        _sendAction = sendAction;
    }

    /// <summary>
    /// Gets the current active request (null if none).
    /// </summary>
    public PendingPermissionRequest? GetActiveRequest()
    {
        lock (_lock)
        {
            return _activeRequest;
        }
    }

    /// <summary>
    /// Enqueues a new permission request.
    /// </summary>
    /// <param name="requestId">Unique request identifier</param>
    /// <param name="userId">User ID making the request</param>
    /// <param name="permissionType">Type of permission requested</param>
    /// <param name="resource">Resource being accessed</param>
    /// <param name="allowCode">6-digit allow code</param>
    /// <param name="denyCode">6-digit deny code</param>
    /// <returns>Task that completes when user responds</returns>
    public Task<AskPermissionResult> EnqueueRequestAsync(
        Guid requestId,
        Guid userId,
        PermissionType permissionType,
        string resource,
        string allowCode,
        string denyCode)
    {
        var tcs = new TaskCompletionSource<AskPermissionResult>();
        var timeoutCts = new CancellationTokenSource();
        bool shouldProcess;
        var request = new PendingPermissionRequest
        {
            RequestId = requestId,
            UserId = userId,
            PermissionType = permissionType,
            Resource = resource,
            AllowCode = allowCode,
            DenyCode = denyCode,
            Tcs = tcs,
            CreatedAt = DateTime.UtcNow,
            TimeoutCts = timeoutCts
        };

        lock (_lock)
        {
            _pendingQueue.Enqueue(request);

            // Start timeout timer (30 minutes)
            timeoutCts.CancelAfter(TimeSpan.FromMinutes(30));
            timeoutCts.Token.Register(() =>
            {
                bool shouldProcessNext = false;

                lock (_lock)
                {
                    if (_activeRequest?.RequestId == requestId)
                    {
                        _activeRequest = null;
                        _isProcessing = false;
                        shouldProcessNext = true;
                    }
                    else if (_pendingQueue.Any(r => r.RequestId == requestId))
                    {
                        // Remove from queue if not active
                        var tempList = _pendingQueue.ToList();
                        tempList.RemoveAll(r => r.RequestId == requestId);
                        _pendingQueue.Clear();
                        foreach (var item in tempList)
                        {
                            _pendingQueue.Enqueue(item);
                        }
                    }
                }

                if (!tcs.Task.IsCompleted)
                {
                    tcs.TrySetResult(new AskPermissionResult
                    {
                        Allowed = false
                    });
                }

                if (shouldProcessNext)
                {
                    _ = ProcessNextRequestAsync();
                }
            });

            shouldProcess = !_isProcessing;
        }

        _logger.Info(userId, "Permission request enqueued: {0} for {1}", permissionType, resource);

        // Process outside the lock to avoid nested lock + async issues
        if (shouldProcess)
        {
            _ = ProcessNextRequestAsync();
        }

        return tcs.Task;
    }

    /// <summary>
    /// Processes the next request in the queue.
    /// </summary>
    private async Task ProcessNextRequestAsync()
    {
        PendingPermissionRequest? nextRequest = null;

        lock (_lock)
        {
            if (_isProcessing || _pendingQueue.Count == 0)
                return;

            _isProcessing = true;
            nextRequest = _pendingQueue.Dequeue();
            _activeRequest = nextRequest;
        }

        try
        {
            if (nextRequest != null)
            {
                _logger.Info(nextRequest.UserId, "Processing permission request: {0} for {1}", 
                    nextRequest.PermissionType, nextRequest.Resource);

                // Trigger UI update
                await _sendAction();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(nextRequest?.UserId ?? Guid.Empty, "Error processing permission request: {0}", ex.Message);

            if (nextRequest != null && !nextRequest.Tcs.Task.IsCompleted)
            {
                nextRequest.Tcs.TrySetResult(new AskPermissionResult
                {
                    Allowed = false
                });
            }

            lock (_lock)
            {
                _isProcessing = false;
                _activeRequest = null;
            }

            // Process next request
            _ = ProcessNextRequestAsync();
        }
    }

    /// <summary>
    /// Completes the active request with the given result.
    /// </summary>
    /// <param name="result">The result to complete with</param>
    public void CompleteActiveRequest(AskPermissionResult result)
    {
        PendingPermissionRequest? request = null;

        lock (_lock)
        {
            if (_activeRequest != null)
            {
                request = _activeRequest;
                _activeRequest = null;
                _isProcessing = false;
            }
        }

        if (request != null)
        {
            request.TimeoutCts.Dispose();

            if (!request.Tcs.Task.IsCompleted)
            {
                request.Tcs.TrySetResult(result);
            }

            _logger.Info(request.UserId, "Permission request completed: {0} - {1}", 
                request.PermissionType, result.Allowed ? "ALLOWED" : "DENIED");

            // Process next request
            _ = ProcessNextRequestAsync();
        }
    }

    /// <summary>
    /// Gets the current queue status.
    /// </summary>
    /// <returns>Tuple of (active requests, pending count)</returns>
    public (int active, int pending) GetQueueStatus()
    {
        lock (_lock)
        {
            return (_activeRequest != null ? 1 : 0, _pendingQueue.Count);
        }
    }

    /// <summary>
    /// Cancels all pending requests.
    /// </summary>
    public void CancelAllRequests()
    {
        lock (_lock)
        {
            // Cancel active request
            if (_activeRequest != null && !_activeRequest.Tcs.Task.IsCompleted)
            {
                _activeRequest.Tcs.TrySetResult(new AskPermissionResult
                {
                    Allowed = false
                });
            }

            // Cancel pending requests
            while (_pendingQueue.Count > 0)
            {
                var request = _pendingQueue.Dequeue();
                if (!request.Tcs.Task.IsCompleted)
                {
                    request.Tcs.TrySetResult(new AskPermissionResult
                    {
                        Allowed = false,
                    });
                }
            }

            _activeRequest = null;
            _isProcessing = false;
        }
    }

    /// <summary>
    /// Enqueues a permission request and waits for the result.
    /// </summary>
    public async Task<AskPermissionResult> EnqueueAsync(PendingPermissionRequest request)
    {
        bool shouldProcess;

        lock (_lock)
        {
            _pendingQueue.Enqueue(request);

            // Start timeout timer
            request.TimeoutCts.CancelAfter(TimeSpan.FromMinutes(30));
            request.TimeoutCts.Token.Register(() =>
            {
                if (!request.Tcs.Task.IsCompleted)
                {
                    request.Tcs.TrySetResult(new AskPermissionResult
                    {
                        Allowed = false
                    });
                }
            });

            _logger.Info(request.UserId, "Permission request enqueued: {0} for {1}",
                request.PermissionType, request.Resource);

            shouldProcess = !_isProcessing;
        }

        // Process outside the lock to avoid nested lock + async issues
        if (shouldProcess)
        {
            _ = ProcessNextRequestAsync();
        }

        return await request.Tcs.Task;
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
                _logger.Warn(null, "Invalid permission response: userId={0}, expectedUserId={1}",
                    userId, request?.UserId);
                return;
            }
        }

        // Set result FIRST (before cancelling timeout, to avoid triggering HandleTimeout)
        var result = new AskPermissionResult
        {
            Allowed = allowed,
            AddToCache = addToCache,
            CacheDuration = cacheDuration
        };

        request.Tcs.TrySetResult(result);

        lock (_lock)
        {
            _activeRequest = null;
            _isProcessing = false;
        }

        request.TimeoutCts.Cancel();
        request.TimeoutCts.Dispose();
        _logger.Info(userId, "Permission request handled: {0} - {1}", request.PermissionType, allowed ? "ALLOWED" : "DENIED");

        // Process next request
        _ = ProcessNextRequestAsync();
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

            _activeRequest = null;
            _isProcessing = false;
        }

        if (!request.Tcs.Task.IsCompleted)
        {
            request.Tcs.TrySetResult(new AskPermissionResult
            {
                Allowed = false
            });
        }

        _logger.Warn(request.UserId, "Permission request timed out: {0}", request.RequestId);

        // Process next request
        _ = ProcessNextRequestAsync();
    }

    /// <summary>
    /// Called when a client connects via SSE. Triggers queue processing if there are pending requests,
    /// or re-sends the active request so the new client can see it.
    /// </summary>
    public void OnClientConnected()
    {
        bool shouldProcess;
        bool hasActiveRequest;

        lock (_lock)
        {
            hasActiveRequest = _activeRequest != null;
            shouldProcess = _pendingQueue.Count > 0 && !_isProcessing;
        }

        // If there is an active request waiting for user response,
        // re-send it so the newly connected client can display it.
        if (hasActiveRequest)
        {
            _ = _sendAction();
            _logger.Info(null, "Client connected, re-sending active permission request");
        }

        // If there are pending requests and nothing is being processed, start processing
        if (shouldProcess)
        {
            _ = ProcessNextRequestAsync();
        }

        _logger.Debug(null, "Client connected, queue processing triggered if needed");
    }
}