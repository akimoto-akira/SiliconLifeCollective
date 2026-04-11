// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Concurrent;

namespace SiliconLife.Collective;

/// <summary>
/// Abstract base class for executors.
/// Executors provide a security boundary for AI-initiated operations.
/// All disk, network, and command-line operations initiated by AI tools
/// must pass through executors.
/// </summary>
public abstract class ExecutorBase : IDisposable
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ExecutorBase>();
    private readonly BlockingCollection<ExecutorRequest> _requestQueue;
    private Thread? _workerThread;
    private volatile bool _isRunning;
    private bool _disposed;

    /// <summary>
    /// Gets or sets the default timeout for operations
    /// </summary>
    public TimeSpan DefaultTimeout { get; protected set; }

    /// <summary>
    /// Gets whether the executor processing thread is running
    /// </summary>
    public bool IsRunning => _isRunning;

    /// <summary>
    /// Gets the number of pending requests in the queue
    /// </summary>
    public int QueueCount => _requestQueue.Count;

    /// <summary>
    /// Initializes a new instance of the ExecutorBase class
    /// </summary>
    /// <param name="defaultTimeout">The default timeout for operations</param>
    /// <param name="queueCapacity">Maximum capacity of the request queue</param>
    protected ExecutorBase(TimeSpan defaultTimeout, int queueCapacity = 100)
    {
        DefaultTimeout = defaultTimeout;
        _requestQueue = new BlockingCollection<ExecutorRequest>(queueCapacity);
    }

    /// <summary>
    /// Executes a request synchronously with timeout
    /// </summary>
    /// <param name="request">The executor request</param>
    /// <param name="timeout">Optional timeout override</param>
    /// <returns>The execution result</returns>
    public ExecutorResult Execute(ExecutorRequest request, TimeSpan? timeout = null)
    {
        TimeSpan actualTimeout = timeout ?? DefaultTimeout;
        _logger.Debug("Executing request: {Request}", request);

        try
        {
            Task<ExecutorResult> task = Task.Run(() => ExecuteCore(request));
            if (task.Wait(actualTimeout))
            {
                _logger.Debug("Execution completed successfully");
                return task.Result;
            }
            _logger.Warn("Execution timed out after {Timeout}ms", actualTimeout.TotalMilliseconds);
            return ExecutorResult.Failed("Operation timed out");
        }
        catch (AggregateException ex)
        {
            Exception? inner = ex.InnerException;
            _logger.Error("Execution failed: {Exception}", ex);
            return ExecutorResult.Failed(inner?.Message ?? ex.Message);
        }
    }

    /// <summary>
    /// Enqueues a request for asynchronous processing
    /// </summary>
    /// <param name="request">The executor request</param>
    /// <returns>True if successfully enqueued</returns>
    public bool Enqueue(ExecutorRequest request)
    {
        if (!_isRunning)
        {
            _logger.Warn("Cannot enqueue: executor not running");
            return false;
        }
        bool result = _requestQueue.TryAdd(request);
        _logger.Debug("Request enqueued, queue size={Size}", _requestQueue.Count);
        return result;
    }

    /// <summary>
    /// Core execution logic to be implemented by derived classes
    /// </summary>
    /// <param name="request">The executor request</param>
    /// <returns>The execution result</returns>
    protected abstract ExecutorResult ExecuteCore(ExecutorRequest request);

    /// <summary>
    /// Starts the background processing thread for queued requests
    /// </summary>
    public virtual void Start()
    {
        if (_isRunning) return;
        _isRunning = true;
        _workerThread = new Thread(ProcessQueue)
        {
            IsBackground = true,
            Name = $"Executor-{GetType().Name}"
        };
        _workerThread.Start();
        _logger.Info("Executor started");
    }

    /// <summary>
    /// Stops the background processing thread
    /// </summary>
    public virtual void Stop()
    {
        _logger.Info("Executor stopped");
        _isRunning = false;
        _requestQueue.CompleteAdding();
        _workerThread?.Join(3000);
    }

    private void ProcessQueue()
    {
        foreach (ExecutorRequest request in _requestQueue.GetConsumingEnumerable())
        {
            if (!_isRunning) break;
            try
            {
                ExecuteCore(request);
            }
            catch
            {
                // Queue processing errors are logged but don't crash the processor
            }
        }
    }

    /// <summary>
    /// Releases resources used by the executor
    /// </summary>
    public virtual void Dispose()
    {
        if (_disposed) return;
        Stop();
        _requestQueue.Dispose();
        _disposed = true;
    }
}
