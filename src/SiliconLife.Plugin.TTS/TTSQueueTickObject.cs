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
using SiliconLife.Collective;

namespace SiliconLife.Plugin.TTS;

/// <summary>
/// TickObject：监控 TTS Worker 线程健康 + 暴露状态给 tts 工具的 get_status。
/// 本身不做任何耗时操作（微秒级完成，永不触发 Circuit Breaker），重活全部在 Worker 线程。
/// </summary>
public class TTSQueueTickObject : TickObject
{
    /// <summary>单句立即重试次数上限（间隔 2 秒）。</summary>
    private const int MaxRetryCount = 3;

    /// <summary>单句立即重试间隔。</summary>
    private static readonly TimeSpan RetryInterval = TimeSpan.FromSeconds(2);

    private readonly ConcurrentQueue<TTSJob> _queue = new();
    private readonly TTSWorker _worker;
    private readonly IndexTTSClient _client;
    private readonly BatchStatus _status = new();

    /// <summary>失败句列表。Worker 线程写、Tool 线程读写，必须用并发容器。</summary>
    private readonly ConcurrentBag<TTSJob> _failed = new();

    /// <summary>全部任务追踪（Key: "{batchId}:{index}"）。</summary>
    private readonly ConcurrentDictionary<string, TTSJob> _allJobs = new();

    /// <summary>已触发过服务端清理的批次（防重复清理）。</summary>
    private readonly ConcurrentDictionary<Guid, bool> _cleanedBatches = new();

    /// <summary>批次提交时间（用于定位最近批次与生成进度摘要）。</summary>
    private readonly ConcurrentDictionary<Guid, DateTime> _batchSubmitTimes = new();

    private readonly ILogger _logger = LogManager.Instance.GetLogger<TTSQueueTickObject>();

    /// <summary>
    /// 创建 TTS 队列 TickObject（每 2 秒监控一次）。
    /// </summary>
    /// <param name="client">IndexTTS2 客户端</param>
    public TTSQueueTickObject(IndexTTSClient? client = null)
        : base(TimeSpan.FromSeconds(2), autoRegister: false)
    {
        // autoRegister: false —— 由 TTSPlugin.OnLoad 注册 / OnUnload 注销，与插件生命周期对齐
        Priority = 200; // 低优先级（低值=高优先级，默认 100），不打扰 Being 主循环
        _client = client ?? new IndexTTSClient();
        _worker = new TTSWorker(_queue, OnJobCompleted, OnJobFailed, _client);
        _worker.Start();
    }

    /// <inheritdoc/>
    protected override void OnTick(TimeSpan deltaTime)
    {
        _status.QueueLength = _queue.Count;
        _status.IsAlive = _worker.IsAlive;
        _status.CurrentJobIndex = _worker.CurrentIndex;
        _status.IsProcessing = _worker.IsProcessing;
        _status.CompletedCount = _worker.CompletedCount;
        _status.FailedCount = _failed.Count;

        if (!_worker.IsAlive)
        {
            _logger.Error(null, "TTS Worker 线程异常退出，重启中...");
            _worker.Restart(); // Restart 会将线程中途死亡时丢失的 in-flight 任务重新入队
        }
    }

    /// <summary>
    /// 提交整批任务（全量预处理后一次性投递）。
    /// </summary>
    /// <param name="request">批量请求（句子列表 + 音色 + 输出目录）</param>
    /// <returns>批次 ID</returns>
    /// <exception cref="ArgumentException">句子列表为空</exception>
    public Guid SubmitBatch(TTSBatchRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Sentences.Count == 0)
        {
            throw new ArgumentException("Sentences list is empty", nameof(request));
        }

        Guid batchId = Guid.NewGuid();
        _batchSubmitTimes[batchId] = DateTime.Now;
        for (int i = 0; i < request.Sentences.Count; i++)
        {
            var job = new TTSJob
            {
                BatchId = batchId,
                Index = i,
                Text = request.Sentences[i].Text,
                Emotion = request.Sentences[i].Emotion,
                VoiceId = request.VoiceId,
                OutputDir = request.OutputDir,
                Status = TTSJobStatus.Pending
            };
            _allJobs.TryAdd($"{batchId}:{i}", job);
            _queue.Enqueue(job);
        }

        _logger.Info(null, "TTS batch {0} submitted: {1} sentence(s), voice='{2}', output='{3}'",
            batchId, request.Sentences.Count, request.VoiceId, request.OutputDir);
        return batchId;
    }

    /// <summary>
    /// 提交单句任务（preview_sentence 试听用，独立批次）。
    /// </summary>
    /// <returns>批次 ID（单句，序号 0）</returns>
    public Guid SubmitSingle(string text, string voiceId, EmotionConfig? emotion, string outputDir)
    {
        return SubmitBatch(new TTSBatchRequest
        {
            VoiceId = voiceId,
            OutputDir = outputDir,
            Sentences = [new TTSSentenceItem { Text = text, Emotion = emotion }]
        });
    }

    /// <summary>
    /// 整体进度快照（快速，不含逐句详情）。
    /// </summary>
    public BatchStatus GetStatus() => _status;

    /// <summary>
    /// 逐句状态详情（get_task_detail 用）。多批任务共存时按 batchIdFilter 过滤，
    /// 未指定时取最近提交的批次。
    /// </summary>
    public TaskDetailResult GetTaskDetail(Guid? batchIdFilter = null)
    {
        Guid batchId = batchIdFilter ?? GetLatestBatchId();

        return new TaskDetailResult
        {
            BatchId = batchId,
            TotalSentences = _allJobs.Values.Count(j => j.BatchId == batchId),
            Sentences = _allJobs.Values
                .Where(j => j.BatchId == batchId)
                .OrderBy(j => j.Index)
                .Select(j => new SentenceStatus
                {
                    Index = j.Index,
                    Text = j.Text.Length > 50 ? j.Text[..50] + "..." : j.Text,
                    Status = j.Status.ToString(),
                    OutputFilePath = j.OutputFilePath,
                    Error = j.ErrorMessage
                })
                .ToList()
        };
    }

    /// <summary>
    /// 最近提交的批次 ID（无批次时返回 Guid.Empty）。
    /// </summary>
    public Guid GetLatestBatchId()
    {
        return _batchSubmitTimes.Count == 0
            ? Guid.Empty
            : _batchSubmitTimes.OrderByDescending(kv => kv.Value).First().Key;
    }

    /// <summary>
    /// 生成各批次进度摘要（get_status 的消息文本用，默认最近 3 个批次）。
    /// </summary>
    /// <param name="maxBatches">最多返回的批次数</param>
    public List<string> GetBatchSummaries(int maxBatches = 3)
    {
        return _batchSubmitTimes
            .OrderByDescending(kv => kv.Value)
            .Take(maxBatches)
            .Select(kv =>
            {
                List<TTSJob> jobs = _allJobs.Values.Where(j => j.BatchId == kv.Key).ToList();
                int completed = jobs.Count(j => j.Status == TTSJobStatus.Completed);
                int processing = jobs.Count(j => j.Status == TTSJobStatus.Processing);
                int failed = jobs.Count(j => j.Status == TTSJobStatus.Failed);
                return $"批次 {kv.Key:N}（{kv.Value:HH:mm:ss} 提交）: {completed}/{jobs.Count} 完成, " +
                       $"{processing} 进行中, {failed} 失败";
            })
            .ToList();
    }

    /// <summary>
    /// 获取失败句列表。
    /// </summary>
    public List<TTSJob> GetFailedJobs() => _failed.ToList();

    /// <summary>
    /// 统一重试所有失败句。每句只再尝试一次（不重置 RetryCount，保持 ≥3：
    /// 失败即直接回到失败列表）。跳过 retry_sentence 等途径已改状态的陈旧条目，
    /// 防止重复入队。
    /// </summary>
    /// <returns>重新入队的句数</returns>
    public int RetryFailed()
    {
        int count = 0;
        while (_failed.TryTake(out TTSJob? job))
        {
            if (job.Status != TTSJobStatus.Failed)
            {
                // retry_sentence 已重置过该句（重给预算）——陈旧条目，跳过
                continue;
            }
            job.ErrorMessage = null;
            job.Status = TTSJobStatus.Pending;
            _queue.Enqueue(job);
            count++;
        }
        return count;
    }

    /// <summary>
    /// 重试指定句子。重置 RetryCount=0（重新给满单句重试预算），仅对 Failed 状态生效。
    /// </summary>
    /// <param name="batchId">批次 ID</param>
    /// <param name="sentenceIndex">句子序号（从 0 开始）</param>
    /// <returns>是否成功入队</returns>
    public bool RetrySentence(Guid batchId, int sentenceIndex)
    {
        if (!_allJobs.TryGetValue($"{batchId}:{sentenceIndex}", out TTSJob? job))
        {
            return false;
        }
        if (job.Status != TTSJobStatus.Failed)
        {
            return false;
        }

        job.RetryCount = 0;
        job.ErrorMessage = null;
        job.Status = TTSJobStatus.Pending;
        _queue.Enqueue(job);
        return true;
    }

    /// <summary>
    /// 停止 Worker（插件卸载时调用）。
    /// </summary>
    public void Shutdown()
    {
        _worker.Shutdown();
    }

    /// <summary>
    /// 单句成功回调（Worker 线程调用，不得抛异常）。
    /// </summary>
    private void OnJobCompleted(TTSJob job)
    {
        try
        {
            job.Status = TTSJobStatus.Completed;

            if (_allJobs.TryGetValue($"{job.BatchId}:{job.Index}", out TTSJob? tracked))
            {
                tracked.Status = TTSJobStatus.Completed;
                tracked.OutputFilePath = job.OutputFilePath;
            }

            CheckBatchCompletion(job.BatchId);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "OnJobCompleted callback failed for batch {0} index {1}: {2}",
                job.BatchId, job.Index, ex.Message);
        }
    }

    /// <summary>
    /// 单句失败回调（Worker 线程调用，不得抛异常）。
    /// 与决策一致：单句立即重试 3 次（间隔 2 秒），3 次全失败才记入失败列表。
    /// </summary>
    private void OnJobFailed(TTSJob job, Exception ex)
    {
        try
        {
            if (job.RetryCount < MaxRetryCount)
            {
                job.RetryCount++;
                _logger.Warn(null, "TTS 句 #{0} 失败（第 {1}/{2} 次），{3} 后重试: {4}",
                    job.Index, job.RetryCount, MaxRetryCount, RetryInterval.TotalSeconds, ex.Message);

                Task.Delay(RetryInterval).ContinueWith(_ =>
                {
                    job.Status = TTSJobStatus.Pending;
                    _queue.Enqueue(job);
                });
            }
            else
            {
                job.Status = TTSJobStatus.Failed;
                job.ErrorMessage = ex.Message;
                _failed.Add(job);

                if (_allJobs.TryGetValue($"{job.BatchId}:{job.Index}", out TTSJob? tracked))
                {
                    tracked.Status = TTSJobStatus.Failed;
                    tracked.ErrorMessage = ex.Message;
                }

                _logger.Error(null, "TTS 句 #{0} 重试 {1} 次后仍失败: {2}", job.Index, MaxRetryCount, ex.Message);

                CheckBatchCompletion(job.BatchId);
            }
        }
        catch (Exception callbackEx)
        {
            _logger.Error(null, "OnJobFailed callback failed for batch {0} index {1}: {2}",
                job.BatchId, job.Index, callbackEx.Message);
        }
    }

    /// <summary>
    /// 检查批次是否全部完成（全部句为 Completed/Failed），
    /// 完成则记录日志并异步触发服务端历史文件清理
    /// （每次合成在服务端 static/outputs/ 留下 WAV + JSON 两个文件，不清理会无限堆积；
    /// 此时各句均已下载到本地，删除服务端文件是安全的）。
    /// </summary>
    private void CheckBatchCompletion(Guid batchId)
    {
        List<TTSJob> batchJobs = _allJobs.Values.Where(j => j.BatchId == batchId).ToList();
        if (batchJobs.Count == 0 || batchJobs.Any(j => j.Status is not (TTSJobStatus.Completed or TTSJobStatus.Failed)))
        {
            return;
        }
        if (!_cleanedBatches.TryAdd(batchId, true))
        {
            return;
        }

        int completed = batchJobs.Count(j => j.Status == TTSJobStatus.Completed);
        int failed = batchJobs.Count(j => j.Status == TTSJobStatus.Failed);
        _logger.Info(null, "TTS batch {0} finished: {1} completed, {2} failed", batchId, completed, failed);

        Task.Run(() =>
        {
            try
            {
                _client.CleanupHistoryAudios();
                _logger.Debug(null, "IndexTTS2 server-side history cleaned after batch {0}", batchId);
            }
            catch (Exception ex)
            {
                _logger.Warn(null, "IndexTTS2 history cleanup after batch {0} failed: {1}", batchId, ex.Message);
            }
        });
    }
}

/// <summary>
/// TTS 队列单例。Tool（由 ToolManager.ScanAllPluginAssemblies 无参构造实例化）与
/// 插件生命周期之间共享同一个队列实例。
/// </summary>
public static class TTSQueueSingleton
{
    private static readonly Lazy<TTSQueueTickObject> _instance = new(() => new TTSQueueTickObject());

    /// <summary>获取全局唯一的 TTS 队列实例。</summary>
    public static TTSQueueTickObject Instance => _instance.Value;
}
