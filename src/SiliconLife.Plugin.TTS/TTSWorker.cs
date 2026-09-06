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
/// Worker 线程：阻塞调用 IndexTTS2 合成。GPU 场景下必须保证串行
/// （Flask 引擎的参考音频缓存非线程安全，并发请求会互相污染音色缓存，
/// 串行是正确性要求，不只是 GPU 友好）。
/// </summary>
public class TTSWorker
{
    private readonly ConcurrentQueue<TTSJob> _queue;
    private readonly Action<TTSJob> _onCompleted;
    private readonly Action<TTSJob, Exception> _onFailed;
    private readonly IndexTTSClient _client;
    private Thread _thread;
    private volatile bool _shutdown;
    private TTSJob? _current;

    /// <summary>当前正在处理的任务（空闲时为 null）。</summary>
    public TTSJob? Current => _current;

    /// <summary>Worker 线程是否存活。</summary>
    public bool IsAlive => _thread?.IsAlive ?? false;

    /// <summary>是否正在处理任务。</summary>
    public bool IsProcessing => _current != null;

    /// <summary>当前处理句子的序号（空闲时 -1）。</summary>
    public int CurrentIndex => _current?.Index ?? -1;

    /// <summary>累计成功句数（仅 Worker 线程更新）。</summary>
    public int CompletedCount { get; private set; }

    /// <summary>
    /// 创建 Worker。
    /// </summary>
    /// <param name="queue">任务队列（与 TTSQueueTickObject 共享）</param>
    /// <param name="onCompleted">单句成功回调（不得抛异常，由队列侧保证安全）</param>
    /// <param name="onFailed">单句失败回调（不得抛异常，由队列侧保证安全）</param>
    /// <param name="client">IndexTTS2 客户端</param>
    public TTSWorker(ConcurrentQueue<TTSJob> queue, Action<TTSJob> onCompleted,
        Action<TTSJob, Exception> onFailed, IndexTTSClient client)
    {
        _queue = queue;
        _onCompleted = onCompleted;
        _onFailed = onFailed;
        _client = client;
        _thread = null!;
    }

    /// <summary>
    /// 启动 Worker 线程。
    /// </summary>
    public void Start()
    {
        _thread = new Thread(() =>
        {
            while (!_shutdown)
            {
                if (_queue.TryDequeue(out TTSJob? job))
                {
                    _current = job;
                    job.Status = TTSJobStatus.Processing;
                    try
                    {
                        ProcessJob(job);
                        CompletedCount++;
                        _onCompleted(job);
                    }
                    catch (Exception ex)
                    {
                        _onFailed(job, ex);
                    }
                    finally
                    {
                        _current = null;
                    }
                }
                else
                {
                    Thread.Sleep(200); // 队列空时歇一会
                }
            }
        })
        { IsBackground = true, Name = "TTS-Worker" };
        _thread.Start();
    }

    /// <summary>
    /// 处理单个任务：调用 IndexTTS2 HTTP API 合成并下载 WAV 到本地。
    /// 由 Worker 线程同步执行，可阻塞几秒到几十秒。
    /// </summary>
    private void ProcessJob(TTSJob job)
    {
        // POST /api/synthesize（multipart）→ 解析 SSE（单句任务一个 progress + 一个 completed）
        // → 从 completed 的 audio_url 下载 WAV → 另存为 job.OutputDir/{Index:0000}.wav
        _client.Synthesize(job);
    }

    /// <summary>
    /// 崩溃恢复：由 TickObject.OnTick（MainLoop 线程）在发现 Worker 死亡时调用，天然串行、无重入。
    /// 标记关闭后由守护线程等老线程处理完当前任务自然退出（不设 Join 超时，由合成请求的
    /// 动态超时（字数×5秒，上限 30 分钟）兜底防永久挂起），再复位并启动新线程；线程中途死亡时丢失的 in-flight 任务重新入队。
    /// 注意：不能简单地 Join(短超时) 后复位 _shutdown 并 Start() —— 对正在推理的线程（单句可达
    /// 几十秒）必然超时，随后复位会产生第二个消费线程，而老线程因 _shutdown 已复位永不退出，
    /// 双线程并发消费队列违反串行原则。
    /// </summary>
    public void Restart()
    {
        Thread? oldThread = _thread;
        _shutdown = true;
        TTSJob? lostJob = _current;

        new Thread(() =>
        {
            try
            {
                oldThread?.Join();
            }
            catch (Exception joinEx)
            {
                // Join 被中断等极端情况：记录后直接继续启动新线程
                LogManager.Instance.GetLogger<TTSWorker>().Warn(null,
                    "TTS Worker restart join interrupted: {0}", joinEx.Message);
            }
            _shutdown = false;
            if (lostJob != null)
            {
                lostJob.Status = TTSJobStatus.Pending;
                _queue.Enqueue(lostJob);
            }
            Start();
        })
        { IsBackground = true, Name = "TTS-Worker-Restart" }.Start();
    }

    /// <summary>
    /// 请求 Worker 停止（插件卸载时调用）。等待当前任务最多 graceSeconds 秒。
    /// 队列中未处理的任务不持久化，重启后需整批重跑（已知限制）。
    /// </summary>
    /// <param name="graceSeconds">当前任务的等待上限（秒）</param>
    public void Shutdown(int graceSeconds = 30)
    {
        _shutdown = true;
        if (_thread is { IsAlive: true })
        {
            if (!_thread.Join(TimeSpan.FromSeconds(graceSeconds)))
            {
                LogManager.Instance.GetLogger<TTSWorker>().Warn(null,
                    "TTS Worker did not stop within {0}s; abandoning current job", graceSeconds);
            }
        }
    }
}
