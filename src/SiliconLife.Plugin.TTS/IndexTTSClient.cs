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

using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Plugin.TTS;

/// <summary>
/// IndexTTS2（Up主版 Flask API）HTTP 客户端封装。
/// 不实现 IAIClient（现有 13 家客户端均为 LLM 对话客户端），仅目录位置同级、独立 HttpClient 封装。
/// 额外职责：情感向量 Tool 层数组 → API 对象格式转换、SSE 流式响应解析、WAV 下载、服务端历史文件清理。
/// </summary>
public class IndexTTSClient
{
    /// <summary>默认服务地址（本机 Up主版 app.py）。</summary>
    public const string DefaultBaseUrl = "http://127.0.0.1:5000";

    /// <summary>
    /// 情感向量转对象格式时的固定键名顺序（与 Tool 层 8 维数组 [喜,怒,哀,惧,厌恶,低落,惊喜,平静] 一一对应；
    /// 注意第四维 API 键名是 fearful 不是 afraid）。
    /// </summary>
    private static readonly string[] EmotionVectorKeys =
        ["happy", "angry", "sad", "fearful", "disgusted", "melancholic", "surprised", "calm"];

    private static readonly HttpClient _httpClient = new()
    {
        // 非合成请求（健康检查、音色管理等）的全局兜底超时；
        // 合成请求按句子字数×5秒动态计算超时（见 Synthesize 方法）
        Timeout = TimeSpan.FromMinutes(30)
    };

    /// <summary>
    /// 根据句子字数计算合成超时：字数×5秒，下限 10 秒，上限 30 分钟。
    /// Restart 的 Join 依赖该超时兜底，防止 Worker 线程永久挂起。
    /// </summary>
    /// <param name="text">待合成的句子文本</param>
    /// <returns>合成请求的超时时长</returns>
    private static TimeSpan GetSynthesizeTimeout(string text)
    {
        int charCount = string.IsNullOrEmpty(text) ? 0 : text.Length;
        int seconds = charCount * 5;
        seconds = Math.Max(seconds, 10);   // 下限 10 秒
        seconds = Math.Min(seconds, 1800); // 上限 30 分钟（1800 秒）
        return TimeSpan.FromSeconds(seconds);
    }

    private static readonly ILogger _logger = LogManager.Instance.GetLogger<IndexTTSClient>();

    private readonly string _baseUrl;

    /// <summary>
    /// 创建 IndexTTS2 客户端。
    /// </summary>
    /// <param name="baseUrl">服务地址，默认 http://127.0.0.1:5000</param>
    public IndexTTSClient(string baseUrl = DefaultBaseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
    }

    /// <summary>
    /// 健康检查（POST /api/health）。
    /// </summary>
    /// <returns>服务是否可用</returns>
    public bool CheckHealth()
    {
        try
        {
            using var response = _httpClient.PostAsync($"{_baseUrl}/api/health", content: null).GetAwaiter().GetResult();
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "IndexTTS2 health check failed: {0}", ex.Message);
            return false;
        }
    }

    /// <summary>
    /// 列出 IndexTTS2 已保存的可用音色（GET /api/saved-voices）。
    /// </summary>
    /// <returns>音色列表（Name 即合成请求中的 voiceId）</returns>
    public List<SavedVoice> ListVoices()
    {
        using var response = _httpClient.GetAsync($"{_baseUrl}/api/saved-voices").GetAwaiter().GetResult();
        response.EnsureSuccessStatusCode();

        string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        using JsonDocument document = JsonDocument.Parse(body);

        var voices = new List<SavedVoice>();
        if (document.RootElement.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement element in document.RootElement.EnumerateArray())
            {
                voices.Add(new SavedVoice
                {
                    Id = element.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? "" : "",
                    Name = element.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? "" : "",
                    Color = element.TryGetProperty("color", out var colorProp) ? colorProp.GetString() : null,
                    PreviewAudioPath = element.TryGetProperty("preview_audio_path", out var previewProp) ? previewProp.GetString() : null
                });
            }
        }
        return voices;
    }

    /// <summary>
    /// 合成单个句子（POST /api/synthesize，multipart/form-data + SSE 流式响应），
    /// 从 completed 事件的 audio_url 下载 WAV 并另存为 {输出目录}/{序号:0000}.wav。
    /// </summary>
    /// <param name="job">合成任务</param>
    /// <exception cref="InvalidOperationException">服务返回 error 事件或未产生音频</exception>
    /// <exception cref="HttpRequestException">HTTP 层失败</exception>
    public void Synthesize(TTSJob job)
    {
        Directory.CreateDirectory(job.OutputDir);
        string localPath = Path.Combine(job.OutputDir, $"{job.Index:0000}.wav");

        // 1. 组装 textContent（单元素数组：服务端"拼接"即该句本身）
        string textContent = BuildTextContent(job);

        // 2. 组装 multipart 表单
        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(textContent, Encoding.UTF8), "textContent");
        form.Add(new StringContent(job.VoiceId, Encoding.UTF8), "default_voice_id");

        // 3. 发起 SSE 请求并解析（超时按字数×5秒动态计算）
        var timeout = GetSynthesizeTimeout(job.Text);
        using var cts = new CancellationTokenSource(timeout);

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/api/synthesize");
        request.Content = form;
        request.Headers.Accept.ParseAdd("text/event-stream");

        using var response = _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token).GetAwaiter().GetResult();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"IndexTTS2 /api/synthesize 返回 HTTP {(int)response.StatusCode}（服务未启动或引擎未加载）");
        }

        string? audioUrl = ParseSseStream(response, job);

        if (string.IsNullOrEmpty(audioUrl))
        {
            throw new InvalidOperationException("IndexTTS2 未返回 audio_url（合成流异常结束）");
        }

        // 4. 下载 WAV 并另存
        DownloadFile(audioUrl, localPath);
        job.OutputFilePath = localPath;
    }

    /// <summary>
    /// 注册音色第 1 步：上传本地参考音频提取音色特征（POST /api/extract-voice-feature，multipart）。
    /// 特征缓存在服务端内存（服务重启即失效），须尽快完成后续试听/保存步骤。
    /// </summary>
    /// <param name="audioFilePath">本地参考音频文件路径</param>
    /// <returns>特征标识符（feature_identifier，供试听/保存步骤引用）</returns>
    /// <exception cref="FileNotFoundException">本地音频文件不存在</exception>
    /// <exception cref="InvalidOperationException">服务返回错误</exception>
    public string ExtractVoiceFeature(string audioFilePath)
    {
        if (!File.Exists(audioFilePath))
        {
            throw new FileNotFoundException("参考音频文件不存在", audioFilePath);
        }

        using var form = new MultipartFormDataContent();
        using FileStream stream = File.OpenRead(audioFilePath);
        using var fileContent = new StreamContent(stream);
        form.Add(fileContent, "referenceAudioFile", Path.GetFileName(audioFilePath));

        using var response = _httpClient.PostAsync($"{_baseUrl}/api/extract-voice-feature", form).GetAwaiter().GetResult();
        string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"提取音色特征失败（HTTP {(int)response.StatusCode}）: {ExtractApiError(body)}");
        }

        using JsonDocument document = JsonDocument.Parse(body);
        if (!document.RootElement.TryGetProperty("feature_identifier", out JsonElement idProp) ||
            idProp.ValueKind != JsonValueKind.String)
        {
            throw new InvalidOperationException("提取音色特征失败：响应中缺少 feature_identifier");
        }
        return idProp.GetString()!;
    }

    /// <summary>
    /// 注册音色第 2 步：用特征合成试听音频（POST /api/synthesize-preview-for-feature）。
    /// 含单句推理（数秒~几十秒），会临时占用引擎，须与 Worker 合成任务互斥执行。
    /// 注意：服务端 app.py 在调用 infer() 时用 **data 展开整个请求体 JSON，
    /// 已修复为在展开前过滤掉与显式参数冲突的键（text, feature_identifier, spk_audio_prompt, output_path），
    /// 避免此前 "infer() got multiple values for keyword argument 'text'" 的 HTTP 500。
    /// </summary>
    /// <param name="featureIdentifier">第 1 步返回的特征标识符</param>
    /// <param name="text">试听文本</param>
    /// <returns>试听音频的相对 URL 与服务端文件引用（保存步骤的 preview_audio_ref）</returns>
    /// <exception cref="InvalidOperationException">服务返回错误</exception>
    public (string AudioUrl, string PreviewAudioRef) PreviewForFeature(string featureIdentifier, string text)
    {
        var payload = JsonSerializer.Serialize(new { feature_identifier = featureIdentifier, text });
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");

        using var response = _httpClient.PostAsync($"{_baseUrl}/api/synthesize-preview-for-feature", content).GetAwaiter().GetResult();
        string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"生成试听音频失败（HTTP {(int)response.StatusCode}）: {ExtractApiError(body)}");
        }

        using JsonDocument document = JsonDocument.Parse(body);
        JsonElement root = document.RootElement;
        if (!root.TryGetProperty("preview_audio_backend_ref", out JsonElement refProp) ||
            refProp.ValueKind != JsonValueKind.String)
        {
            throw new InvalidOperationException("生成试听音频失败：响应中缺少 preview_audio_backend_ref");
        }
        string audioUrl = root.TryGetProperty("audio_url", out JsonElement urlProp) ? urlProp.GetString() ?? "" : "";
        return (audioUrl, refProp.GetString()!);
    }

    /// <summary>
    /// 注册音色第 3 步：保存为持久音色（POST /api/save-voice-feature）。
    /// previewAudioRef 提供时，试听音频将成为该音色的 preview.wav（后续合成的参考音频）；
    /// 服务端并在保存后从内存缓存移除该特征（同标识符不可重复保存）。
    /// </summary>
    /// <param name="name">音色名（合成请求中的 voiceId）</param>
    /// <param name="featureIdentifier">第 1 步返回的特征标识符</param>
    /// <param name="previewAudioRef">第 2 步返回的试听音频引用</param>
    /// <returns>保存后的音色元数据</returns>
    /// <exception cref="InvalidOperationException">服务返回错误</exception>
    public SavedVoice SaveVoiceFeature(string name, string featureIdentifier, string previewAudioRef)
    {
        var payload = JsonSerializer.Serialize(new
        {
            name,
            feature_identifier = featureIdentifier,
            preview_audio_ref = previewAudioRef
        });
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");

        using var response = _httpClient.PostAsync($"{_baseUrl}/api/save-voice-feature", content).GetAwaiter().GetResult();
        string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"保存音色失败（HTTP {(int)response.StatusCode}）: {ExtractApiError(body)}");
        }

        using JsonDocument document = JsonDocument.Parse(body);
        JsonElement root = document.RootElement;
        return new SavedVoice
        {
            Id = root.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? "" : "",
            Name = root.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? "" : "",
            Color = root.TryGetProperty("color", out var colorProp) ? colorProp.GetString() : null,
            PreviewAudioPath = root.TryGetProperty("preview_audio_path", out var previewProp) ? previewProp.GetString() : null
        };
    }

    /// <summary>
    /// 删除已保存的音色（DELETE /api/saved-voices/&lt;voice_id&gt;）。
    /// 服务端会删除该音色关联的三个文件（.cond_mel.npy、.meta.json、.preview.wav）。
    /// </summary>
    /// <param name="voiceId">音色名称（即 list_voices 返回的 Name 字段）</param>
    /// <exception cref="InvalidOperationException">服务返回错误或音色不存在</exception>
    public void DeleteVoice(string voiceId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/api/saved-voices/{Uri.EscapeDataString(voiceId)}");
        using var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
        string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"删除音色失败（HTTP {(int)response.StatusCode}）: {ExtractApiError(body)}");
        }
    }

    /// <summary>
    /// 清理服务端 static/outputs/ 下的历史音频（每次合成会留下 WAV + JSON 两个文件，
    /// 客户端下载另存后服务端不自动清理；DELETE /api/history-audios/all）。
    /// 仅在批次全部完成、且各句已下载到本地之后调用。
    /// </summary>
    public void CleanupHistoryAudios()
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/api/history-audios/all");
        using var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
        if (!response.IsSuccessStatusCode)
        {
            _logger.Warn(null, "IndexTTS2 history cleanup failed: HTTP {0}", (int)response.StatusCode);
        }
    }

    /// <summary>
    /// 组装 textContent JSON：单元素数组 [{text, voiceId, emotion?}]。
    /// 情感向量从 Tool 层数组格式转为 API 对象格式（键名见 EmotionVectorKeys）。
    /// </summary>
    private static string BuildTextContent(TTSJob job)
    {
        var segment = new Dictionary<string, object>
        {
            ["text"] = job.Text,
            ["voiceId"] = job.VoiceId
        };

        if (job.Emotion != null)
        {
            var emotion = new Dictionary<string, object> { ["method"] = job.Emotion.Method };
            if (job.Emotion.Method == "vector")
            {
                var vectorObject = new Dictionary<string, double>();
                double[]? vector = job.Emotion.Vector;
                if (vector != null)
                {
                    for (int i = 0; i < vector.Length && i < EmotionVectorKeys.Length; i++)
                    {
                        vectorObject[EmotionVectorKeys[i]] = vector[i];
                    }
                }
                emotion["vector"] = vectorObject;
            }
            else
            {
                emotion["description"] = job.Emotion.Description ?? "";
            }
            segment["emotion"] = emotion;
        }

        return JsonSerializer.Serialize(new List<object> { segment });
    }

    /// <summary>
    /// 解析 SSE 流：跟踪 event/data 行，error 事件抛异常，completed 事件取 audio_url。
    /// 单句任务正常时序为：progress → completed；失败时为 error（可能后随 no-segments error）。
    /// </summary>
    private static string? ParseSseStream(HttpResponseMessage response, TTSJob job)
    {
        string? currentEvent = null;
        string? audioUrl = null;
        var errors = new List<string>();

        using Stream stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
        using var reader = new StreamReader(stream, Encoding.UTF8);

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith("event:", StringComparison.Ordinal))
            {
                currentEvent = line["event:".Length..].Trim();
            }
            else if (line.StartsWith("data:", StringComparison.Ordinal))
            {
                string data = line["data:".Length..].Trim();
                if (string.IsNullOrEmpty(data))
                {
                    continue;
                }

                try
                {
                    using JsonDocument payload = JsonDocument.Parse(data);
                    switch (currentEvent)
                    {
                        case "error":
                            errors.Add(payload.RootElement.TryGetProperty("error", out var errorProp)
                                ? errorProp.GetString() ?? "unknown error"
                                : data);
                            break;
                        case "completed":
                            audioUrl = payload.RootElement.TryGetProperty("audio_url", out var urlProp)
                                ? urlProp.GetString()
                                : null;
                            break;
                        default:
                            // progress 等事件：忽略
                            break;
                    }
                }
                catch (JsonException)
                {
                    // 非 JSON data 行：忽略
                }
            }
        }

        if (audioUrl == null)
        {
            string detail = errors.Count > 0
                ? string.Join("; ", errors)
                : "no completed event received";
            throw new InvalidOperationException($"IndexTTS2 合成失败（句 #{job.Index} “{Truncate(job.Text, 30)}”）: {detail}");
        }

        if (errors.Count > 0)
        {
            // completed 已产生（error 之后服务端仍完成拼接），记录但不失败
            _logger.Warn(null, "IndexTTS2 SSE stream contained {0} error event(s): {1}",
                errors.Count, string.Join("; ", errors));
        }

        return audioUrl;
    }

    /// <summary>
    /// 从服务端下载 WAV 到本地路径。
    /// </summary>
    private void DownloadFile(string relativeUrl, string localPath)
    {
        string url = relativeUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
            ? relativeUrl
            : $"{_baseUrl}{relativeUrl}";

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        byte[] bytes = _httpClient.GetByteArrayAsync(url, cts.Token).GetAwaiter().GetResult();
        File.WriteAllBytes(localPath, bytes);
    }

    /// <summary>
    /// 从 JSON 错误响应体中提取 error 字段（非 JSON 或缺失时返回原文）。
    /// </summary>
    private static string ExtractApiError(string body)
    {
        try
        {
            using JsonDocument document = JsonDocument.Parse(body);
            if (document.RootElement.TryGetProperty("error", out JsonElement errorProp))
            {
                return errorProp.GetString() ?? body;
            }
        }
        catch (JsonException)
        {
            // 非 JSON 响应体：返回原文
        }
        return body;
    }

    private static string Truncate(string text, int maxLength)
    {
        return text.Length <= maxLength ? text : text[..maxLength] + "...";
    }
}
