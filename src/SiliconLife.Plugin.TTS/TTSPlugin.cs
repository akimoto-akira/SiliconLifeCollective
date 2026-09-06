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

namespace SiliconLife.Plugin.TTS;

/// <summary>
/// 批量语音合成插件。注册 tts 工具（由 ToolManager.ScanAllPluginAssemblies 自动发现，
/// 见 PluginLoader 加载后 DefaultSiliconBeingFactory 的扫描链路）、向 AI 请求注入 TTS
/// 预处理规则（TTSSystemContextContributor）、挂载队列监控 TickObject 到 MainLoop。
/// Worker 线程随 TTSQueueSingleton.Instance 创建并启动，由本插件的 OnStop 负责优雅停止。
/// </summary>
[PluginCapability(Capability.Network, Reason = "通过 HTTP/SSE 调用本机 IndexTTS2 服务（127.0.0.1:5000）进行合成、下载音频与清理历史")]
[PluginCapability(Capability.FileIO, Reason = "将合成的 WAV 音频写入用户指定的输出目录")]
public class TTSPlugin : IPlugin
{
    private readonly ILogger _logger = LogManager.Instance.GetLogger<TTSPlugin>();

    private TTSQueueTickObject? _queue;
    private TTSSystemContextContributor? _contributor;

    /// <inheritdoc/>
    public string Id => "com.siliconlife.tts-batch";

    /// <inheritdoc/>
    public string Version => "1.0.0";

    /// <inheritdoc/>
    public string GetName(Language language) => "批量语音合成";

    /// <inheritdoc/>
    public string GetDescription(Language language) =>
        "基于本地 IndexTTS2 的批量语音合成：逐句预处理后整批提交，一句一个 WAV（不合并），支持音色克隆与情感控制。";

    /// <inheritdoc/>
    public string GetAuthor(Language language) => "HoshinoKennji";

    /// <inheritdoc/>
    public void OnLoad()
    {
        // 1. 取全局队列单例（首次创建时内部启动 Worker 线程；autoRegister:false，不自动挂载）
        _queue = TTSQueueSingleton.Instance;

        // 2. 注册 System Context Contributor（向每次 AI 请求注入 TTS 预处理规则）
        _contributor = new TTSSystemContextContributor();
        ContextManager.RegisterSystemContextContributor(_contributor);

        // 3. 挂载队列监控 TickObject 到 MainLoop
        MainLoop.Register(_queue);

        _logger.Info(null, "TTS plugin loaded: worker started, queue tick registered");
    }

    /// <inheritdoc/>
    public void OnStart()
    {
        // 本机连接被拒绝时立即失败，不阻塞启动；服务未启动时由 tts 工具的报错信息引导用户启动
        bool healthy = new IndexTTSClient().CheckHealth();
        if (healthy)
        {
            _logger.Info(null, "IndexTTS2 service is reachable at {0}", IndexTTSClient.DefaultBaseUrl);
        }
        else
        {
            _logger.Warn(null,
                "IndexTTS2 service is NOT reachable at {0}; TTS jobs will fail until it is started (D:\\index-tts-2\\启动up主版本.bat)",
                IndexTTSClient.DefaultBaseUrl);
        }
    }

    /// <inheritdoc/>
    public void OnStop()
    {
        // 先摘除 Tick（防止 Shutdown 后 OnTick 误判 Worker 死亡触发重启），再优雅停止 Worker
        if (_queue != null)
        {
            MainLoop.Unregister(_queue);
            _queue.Shutdown();
        }
        _logger.Info(null, "TTS plugin stopped: worker shut down, queue tick unregistered");
    }

    /// <inheritdoc/>
    public void OnUnload()
    {
        if (_contributor != null)
        {
            ContextManager.UnregisterSystemContextContributor(_contributor);
            _contributor = null;
        }
        _logger.Info(null, "TTS plugin unloaded");
    }
}
