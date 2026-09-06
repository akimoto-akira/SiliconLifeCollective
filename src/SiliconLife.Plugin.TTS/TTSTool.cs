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
using System.Text.Json;
using SiliconLife.Collective;
using SiliconLife.Common.Localization;

namespace SiliconLife.Plugin.TTS;

/// <summary>
/// 批量语音合成工具。AI 提交一个句子数组，逐句合成 WAV（一句一个文件，不合并）。
/// 长耗时合成由 TTS Worker 线程串行执行（GPU 场景且引擎缓存非线程安全，串行是正确性要求），
/// 工具本身只做轻量的提交与状态查询，快进快出。
/// </summary>
[ToolAction("submit_batch", "get_status", "get_task_detail", "retry_failed", "retry_sentence", "preview_sentence", "list_voices", "register_voice", "delete_voice")]
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task | ToolScenarioFlag.Timer)]
public class TTSTool : ITool
{
    /// <summary>单句文本长度上限（超过易触发 IndexTTS2 的 max_mel_tokens 截断，与官方 WebUI 默认分句 Token 数对齐）。</summary>
    private const int MaxSentenceLength = 120;

    /// <summary>情感向量维度。</summary>
    private const int EmotionVectorLength = 8;

    /// <summary>情感向量单维上限。</summary>
    private const double EmotionVectorMax = 1.4;

    /// <summary>情感向量总和上限。</summary>
    private const double EmotionVectorSumMax = 1.5;

    private readonly TTSQueueTickObject _queue;
    private readonly IndexTTSClient _client;

    /// <summary>
    /// 无参构造（ToolManager.ScanAllPluginAssemblies 通过无参构造实例化）。
    /// </summary>
    public TTSTool() : this(TTSQueueSingleton.Instance, new IndexTTSClient())
    {
    }

    /// <summary>
    /// 注入构造（测试/显式注册用）。
    /// </summary>
    public TTSTool(TTSQueueTickObject queue, IndexTTSClient client)
    {
        _queue = queue;
        _client = client;
    }

    /// <inheritdoc/>
    public string Name => "tts";

    /// <inheritdoc/>
    public string Description =>
        "批量语音合成工具（基于本地 IndexTTS2）。Actions: 'submit_batch' (提交句子数组，一句一个 WAV), " +
        "'get_status' (查看整体进度快照), " +
        "'get_task_detail' (逐句查看状态：待处理/进行中/已完成/失败), " +
        "'retry_failed' (重试所有失败的句子), " +
        "'retry_sentence' (重试指定序号的句子), 'preview_sentence' (试听单句，后台排队合成), " +
        "'list_voices' (列出 IndexTTS2 已保存的可用音色), " +
        "'register_voice' (上传本地参考音频注册新音色，需队列空闲时执行), " +
        "'delete_voice' (删除指定音色，需队列空闲时执行)";

    /// <inheritdoc/>
    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
        {
            return defaultLoc.GetToolDisplayName(Name);
        }
        return Name;
    }

    /// <inheritdoc/>
    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "操作类型：submit_batch 提交任务 / get_status 查看整体进度 / get_task_detail 逐句查看状态 / retry_failed 重试全部失败 / retry_sentence 重试单句 / preview_sentence 试听 / list_voices 列出可用音色 / register_voice 注册新音色 / delete_voice 删除音色",
                    ["enum"] = new[] { "submit_batch", "get_status", "get_task_detail", "retry_failed", "retry_sentence", "preview_sentence", "list_voices", "register_voice", "delete_voice" }
                },
                // submit_batch 参数
                ["voice_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "IndexTTS2 中已保存的音色 ID（对应 /api/saved-voices 中的 name，可先用 list_voices 查询）"
                },
                ["sentences"] = new Dictionary<string, object>
                {
                    ["type"] = "array",
                    ["description"] = "待合成的句子数组（submit_batch 必填），每句含 text + 情感配置。全部句子应一次性提交，不要逐句调用",
                    ["items"] = new Dictionary<string, object>
                    {
                        ["type"] = "object",
                        ["properties"] = new Dictionary<string, object>
                        {
                            ["text"] = new Dictionary<string, object>
                            {
                                ["type"] = "string",
                                ["description"] = "处理后的文本（已完成数字/专名转换），单句最长 120 字"
                            },
                            ["emotion"] = new Dictionary<string, object>
                            {
                                ["type"] = "object",
                                ["description"] = "情感控制（可选，省略时跟随音色参考音频的情感）",
                                ["properties"] = new Dictionary<string, object>
                                {
                                    ["method"] = new Dictionary<string, object>
                                    {
                                        ["type"] = "string",
                                        ["enum"] = new[] { "text", "vector" },
                                        ["description"] = "\"text\" 用文字描述情感，\"vector\" 用 8 维向量"
                                    },
                                    ["description"] = new Dictionary<string, object>
                                    {
                                        ["type"] = "string",
                                        ["description"] = "情感文字描述，如 \"平静地叙述\"、\"兴奋地感叹\""
                                    },
                                    ["vector"] = new Dictionary<string, object>
                                    {
                                        ["type"] = "array",
                                        ["description"] = "8 维情感向量：[喜,怒,哀,惧,厌恶,低落,惊喜,平静]，每维 0~1.4，总和≤1.5。此为 Tool 层数组格式，投递 API 前由 IndexTTSClient 转为对象格式（键名 happy/angry/sad/fearful/disgusted/melancholic/surprised/calm）",
                                        ["items"] = new Dictionary<string, object> { ["type"] = "number", ["minimum"] = 0, ["maximum"] = 1.4 },
                                        ["minItems"] = 8,
                                        ["maxItems"] = 8
                                    }
                                }
                            }
                        },
                        ["required"] = new[] { "text" }
                    }
                },
                ["output_dir"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "输出目录（可选，为空时由 AI 按日期和文档名生成）"
                },
                // retry_sentence / preview_sentence 参数
                ["sentence_index"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "retry_sentence 时指定要重试的句子序号（从 0 开始）"
                },
                // register_voice 参数
                ["audio_path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "register_voice 时本地参考音频文件路径（wav/mp3 等，建议 5~15 秒、干净人声、与目标音色一致）"
                },
                ["name"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "register_voice 时的新音色名（注册后作为 submit_batch/preview_sentence 的 voice_id 使用）；delete_voice 时指定要删除的音色名"
                },
                ["preview_text"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "register_voice 时可选的试听文本（默认 \"你好，这是一段试听语音。\"；生成的试听音频将成为该音色的参考音频）"
                },
                // get_task_detail / retry_sentence / preview_sentence 参数
                ["batch_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "批任务 ID（submit_batch 返回值）。多批任务共存时必须指定，否则按最近提交的批次处理"
                },
                // preview_sentence 参数
                ["text"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "preview_sentence 时要试听的文本（单句）"
                },
                ["emotion"] = new Dictionary<string, object>
                {
                    ["type"] = "object",
                    ["description"] = "preview_sentence 时可选的情感配置（结构同 sentences[].emotion）",
                    ["properties"] = new Dictionary<string, object>
                    {
                        ["method"] = new Dictionary<string, object>
                        {
                            ["type"] = "string",
                            ["enum"] = new[] { "text", "vector" }
                        },
                        ["description"] = new Dictionary<string, object> { ["type"] = "string" },
                        ["vector"] = new Dictionary<string, object>
                        {
                            ["type"] = "array",
                            ["items"] = new Dictionary<string, object> { ["type"] = "number" },
                            ["minItems"] = 8,
                            ["maxItems"] = 8
                        }
                    }
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    /// <inheritdoc/>
    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        string action = parameters.TryGetValue("action", out object? actionObj)
            ? actionObj?.ToString() ?? ""
            : "";

        try
        {
            return action switch
            {
                "submit_batch" => ExecuteSubmitBatch(callerId, parameters),
                "get_status" => ExecuteGetStatus(callerId),
                "get_task_detail" => ExecuteGetTaskDetail(callerId, parameters),
                "retry_failed" => ExecuteRetryFailed(callerId),
                "retry_sentence" => ExecuteRetrySentence(callerId, parameters),
                "preview_sentence" => ExecutePreviewSentence(callerId, parameters),
                "list_voices" => ExecuteListVoices(),
                "register_voice" => ExecuteRegisterVoice(parameters),
                "delete_voice" => ExecuteDeleteVoice(parameters),
                _ => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"TTS operation failed: {ex.Message}");
        }
    }

    private ToolResult ExecuteSubmitBatch(Guid callerId, Dictionary<string, object> parameters)
    {
        // 1. 解析 voice_id、sentences、output_dir
        if (!parameters.TryGetValue("voice_id", out object? voiceObj) ||
            string.IsNullOrWhiteSpace(voiceObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'voice_id' parameter（可先用 list_voices 查询可用音色）");
        }
        string voiceId = voiceObj!.ToString()!;

        if (!parameters.TryGetValue("sentences", out object? sentencesObj) || sentencesObj is null)
        {
            return ToolResult.Failed("Missing 'sentences' parameter");
        }

        List<TTSSentenceItem>? sentences = ParseSentences(sentencesObj, out List<string> violations);
        if (sentences == null || sentences.Count == 0)
        {
            return ToolResult.Failed("Missing or empty 'sentences' array");
        }

        // 2. 验证所有句子的格式合法（单句 ≤120 字；vector 为 8 维数组、每维 0~1.4、总和 ≤1.5）
        if (violations.Count > 0)
        {
            return ToolResult.Failed(
                $"句子校验失败（共 {violations.Count} 处，已拒绝整批提交，修正后重新提交）:\n" +
                string.Join("\n", violations.Select(v => $"  - {v}")));
        }

        string outputDir = parameters.TryGetValue("output_dir", out object? dirObj) &&
                           !string.IsNullOrWhiteSpace(dirObj?.ToString())
            ? dirObj!.ToString()!
            : Path.Combine(AppContext.BaseDirectory, "TTSOutput",
                DateTime.Now.ToString("yyyyMMdd_HHmmss"));
        outputDir = Path.GetFullPath(outputDir);

        // 3. 将整批任务投递到 TTSQueue（vector 数组 → API 对象格式的转换在 IndexTTSClient 内完成）
        Guid batchId = _queue.SubmitBatch(new TTSBatchRequest
        {
            VoiceId = voiceId,
            OutputDir = outputDir,
            Sentences = sentences
        });

        // 4. 返回 batch_id 和总句数
        return ToolResult.Successful(
            $"已提交批量合成任务：批次 {batchId:N}，共 {sentences.Count} 句，音色 '{voiceId}'，" +
            $"输出目录 {outputDir}（每句一个 WAV，文件名 {0:0000}.wav 起递增编号）。" +
            $"用 action=get_status 查询整体进度，action=get_task_detail&batch_id={batchId:N} 查看逐句状态。",
            JsonSerializer.Serialize(new { batch_id = batchId.ToString("N"), total = sentences.Count, output_dir = outputDir }));
    }

    private ToolResult ExecuteGetStatus(Guid callerId)
    {
        BatchStatus status = _queue.GetStatus();
        List<string> batchSummaries = _queue.GetBatchSummaries();

        string message = $"TTS 队列状态: 待处理 {status.QueueLength} 句, Worker {(status.IsAlive ? "正常" : "异常")}, " +
                         $"累计完成 {status.CompletedCount} 句, 当前失败 {status.FailedCount} 句" +
                         (status.IsProcessing ? $", 正在处理第 {status.CurrentJobIndex + 1} 句" : "");
        if (batchSummaries.Count > 0)
        {
            message += "\n" + string.Join("\n", batchSummaries);
        }

        return ToolResult.Successful(message, JsonSerializer.Serialize(status));
    }

    private ToolResult ExecuteGetTaskDetail(Guid callerId, Dictionary<string, object> parameters)
    {
        Guid? batchId = ParseBatchId(parameters);
        TaskDetailResult details = _queue.GetTaskDetail(batchId);

        if (details.TotalSentences == 0)
        {
            return ToolResult.Successful("没有找到任务（队列为空或批次不存在）");
        }

        // 逐句返回：序号、状态（Pending/Processing/Completed/失败原因）、输出文件路径
        List<string> lines =
        [
            $"批次 {details.BatchId:N}: 共 {details.TotalSentences} 句",
            .. details.Sentences.Select(s => s.Status switch
            {
                "Completed" => $"  ✅ #{s.Index} {s.Text} → {s.OutputFilePath}",
                "Processing" => $"  🔄 #{s.Index} {s.Text}",
                "Failed" => $"  ❌ #{s.Index} {s.Text} 失败: {s.Error}",
                _ => $"  ⏳ #{s.Index} {s.Text}"
            })
        ];

        return ToolResult.Successful(string.Join("\n", lines), JsonSerializer.Serialize(details));
    }

    private ToolResult ExecuteRetryFailed(Guid callerId)
    {
        int retried = _queue.RetryFailed();
        return retried == 0
            ? ToolResult.Successful("没有需要重试的失败句子")
            : ToolResult.Successful($"已重新入队 {retried} 个失败句子（每句再尝试一次，失败会立即回到失败列表）");
    }

    private ToolResult ExecuteRetrySentence(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("sentence_index", out object? indexObj) ||
            !int.TryParse(indexObj?.ToString(), out int sentenceIndex) || sentenceIndex < 0)
        {
            return ToolResult.Failed("Missing or invalid 'sentence_index' parameter（从 0 开始）");
        }

        Guid? batchId = ParseBatchId(parameters);
        Guid effectiveBatchId = batchId ?? _queue.GetLatestBatchId();
        if (effectiveBatchId == Guid.Empty)
        {
            return ToolResult.Failed("没有任何批次任务");
        }

        bool enqueued = _queue.RetrySentence(effectiveBatchId, sentenceIndex);
        return enqueued
            ? ToolResult.Successful($"已重新入队批次 {effectiveBatchId:N} 第 {sentenceIndex + 1} 句（重新给满 3 次重试预算）")
            : ToolResult.Failed($"批次 {effectiveBatchId:N} 第 {sentenceIndex + 1} 句不存在或不是失败状态（只能重试 Failed 句子）");
    }

    private ToolResult ExecutePreviewSentence(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("text", out object? textObj) ||
            string.IsNullOrWhiteSpace(textObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'text' parameter");
        }
        string text = textObj!.ToString()!;
        if (text.Length > MaxSentenceLength)
        {
            return ToolResult.Failed($"试听文本过长（{text.Length} 字 > 上限 {MaxSentenceLength} 字）");
        }

        if (!parameters.TryGetValue("voice_id", out object? voiceObj) ||
            string.IsNullOrWhiteSpace(voiceObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'voice_id' parameter（可先用 list_voices 查询可用音色）");
        }
        string voiceId = voiceObj!.ToString()!;

        EmotionConfig? emotion = null;
        if (parameters.TryGetValue("emotion", out object? emotionObj) && emotionObj is not null)
        {
            emotion = ParseEmotion(emotionObj, out List<string> violations);
            if (violations.Count > 0)
            {
                return ToolResult.Failed(string.Join("; ", violations));
            }
        }

        string outputDir = parameters.TryGetValue("output_dir", out object? dirObj) &&
                           !string.IsNullOrWhiteSpace(dirObj?.ToString())
            ? Path.GetFullPath(dirObj!.ToString()!)
            : Path.Combine(AppContext.BaseDirectory, "TTSOutput",
                $"preview_{DateTime.Now:yyyyMMdd_HHmmss}");

        // 试听同样走串行队列，避免与正在运行的批次并发访问引擎（引擎缓存非线程安全）
        Guid batchId = _queue.SubmitSingle(text, voiceId, emotion, outputDir);

        return ToolResult.Successful(
            $"已提交试听任务（批次 {batchId:N}，单句，输出 {outputDir}\\0000.wav）。" +
            $"用 action=get_task_detail&batch_id={batchId:N} 查询进度，完成后从 OutputFilePath 获取文件。",
            JsonSerializer.Serialize(new { batch_id = batchId.ToString("N"), output_dir = outputDir }));
    }

    private ToolResult ExecuteListVoices()
    {
        try
        {
            List<SavedVoice> voices = _client.ListVoices();
            if (voices.Count == 0)
            {
                return ToolResult.Successful(
                    "IndexTTS2 中没有已保存的音色。请先通过 Up主版网页或三步 API（提取特征 → 试听 → 保存）创建音色。");
            }

            string message = "IndexTTS2 已保存音色（submit_batch 的 voice_id 使用 name 字段值）:\n" +
                             string.Join("\n", voices.Select(v => $"  - {v.Name}（id: {v.Id}）"));

            return ToolResult.Successful(message, JsonSerializer.Serialize(voices));
        }
        catch (Exception ex)
        {
            return ToolResult.Failed(
                $"无法连接 IndexTTS2 服务（{IndexTTSClient.DefaultBaseUrl}）: {ex.Message}。" +
                "请确认已启动 D:\\index-tts-2\\启动up主版本.bat");
        }
    }

    private ToolResult ExecuteRegisterVoice(Dictionary<string, object> parameters)
    {
        // 1. 解析参数：audio_path、name（必填），preview_text（可选）
        if (!parameters.TryGetValue("audio_path", out object? audioObj) ||
            string.IsNullOrWhiteSpace(audioObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'audio_path' parameter（本地参考音频文件路径）");
        }
        string audioPath = Path.GetFullPath(audioObj!.ToString()!);

        if (!parameters.TryGetValue("name", out object? nameObj) ||
            string.IsNullOrWhiteSpace(nameObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'name' parameter（新音色名）");
        }
        string voiceName = nameObj!.ToString()!.Trim();

        string previewText = parameters.TryGetValue("preview_text", out object? previewObj) &&
                             !string.IsNullOrWhiteSpace(previewObj?.ToString())
            ? previewObj!.ToString()!
            : "你好，这是一段试听语音。";

        // 2. 队列空闲检查：注册三步链（提取特征→试听合成→保存）含引擎推理，且服务端 /api/synthesize
        //    会清空 temp_audio（删除已上传的原始音频），必须与 Worker 合成任务互斥执行
        BatchStatus status = _queue.GetStatus();
        if (status.IsProcessing || status.QueueLength > 0)
        {
            return ToolResult.Failed(
                "队列正在处理合成任务（或还有待处理句子），无法注册音色（注册流程与合成共用引擎，并发会冲突）。" +
                "请等待任务完成（action=get_status 查询）后重试。");
        }

        // 3. 三步链：提取特征 → 生成试听 → 保存为持久音色（任何一步失败整体失败，重试会从头执行）
        string featureId = _client.ExtractVoiceFeature(audioPath);
        (_, string previewRef) = _client.PreviewForFeature(featureId, previewText);
        SavedVoice voice = _client.SaveVoiceFeature(voiceName, featureId, previewRef);

        return ToolResult.Successful(
            $"音色注册成功：'{voice.Name}'（voice_id: {voice.Name}）。" +
            $"已生成试听音频作为该音色的参考音频。现在可在 submit_batch / preview_sentence 的 voice_id 中使用。" +
            $"用 action=list_voices 可随时查看全部音色。",
            JsonSerializer.Serialize(voice));
    }

    private ToolResult ExecuteDeleteVoice(Dictionary<string, object> parameters)
    {
        // 1. 解析参数：name（必填，即要删除的音色名）
        if (!parameters.TryGetValue("name", out object? nameObj) ||
            string.IsNullOrWhiteSpace(nameObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'name' parameter（要删除的音色名，可先用 list_voices 查询）");
        }
        string voiceName = nameObj!.ToString()!.Trim();

        // 2. 队列空闲检查：删除音色会移除服务端文件，与正在进行的合成任务（可能引用该音色）存在冲突
        BatchStatus status = _queue.GetStatus();
        if (status.IsProcessing || status.QueueLength > 0)
        {
            return ToolResult.Failed(
                "队列正在处理合成任务（或还有待处理句子），无法删除音色（正在使用的音色被删除会导致合成失败）。" +
                "请等待任务完成（action=get_status 查询）后重试。");
        }

        // 3. 调用服务端 DELETE /api/saved-voices/<voice_id>
        _client.DeleteVoice(voiceName);

        return ToolResult.Successful(
            $"音色 '{voiceName}' 已删除。用 action=list_voices 可查看剩余可用音色。",
            JsonSerializer.Serialize(new { deleted_voice = voiceName }));
    }

    /// <summary>
    /// 解析批次 ID 参数（可选；未提供时由队列按最近批次处理）。
    /// </summary>
    private static Guid? ParseBatchId(Dictionary<string, object> parameters)
    {
        if (parameters.TryGetValue("batch_id", out object? batchObj) &&
            Guid.TryParse(batchObj?.ToString(), out Guid batchId))
        {
            return batchId;
        }
        return null;
    }

    /// <summary>
    /// 解析 sentences 数组（兼容 JsonElement 数组与 ConvertParameters 后的 List/Dictionary 形式）。
    /// 返回 null 表示结构无效；violations 收集校验错误（单句长度、vector 维度/上限/总和）。
    /// 注意：ToolManager.ConvertParameters 在调用 Execute 前会递归将 JsonElement 转为原生类型，
    /// 因此 sentences 到达此处时可能是 List&lt;object?&gt;（每元素为 Dictionary&lt;string, object?&gt;），
    /// 也可能仍为 JsonElement（测试/直调场景），两种形式都必须支持。
    /// </summary>
    private static List<TTSSentenceItem>? ParseSentences(object sentencesObj, out List<string> violations)
    {
        violations = [];
        var result = new List<TTSSentenceItem>();

        // 形式 1：JsonElement 数组（测试/直调场景，未经 ConvertParameters）
        if (sentencesObj is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement item in jsonElement.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object)
                {
                    violations.Add($"句子条目必须是对象: {item}");
                    continue;
                }

                if (!item.TryGetProperty("text", out JsonElement textProp) ||
                    textProp.ValueKind != JsonValueKind.String ||
                    string.IsNullOrWhiteSpace(textProp.GetString()))
                {
                    violations.Add("每个句子必须包含非空 'text' 字段");
                    continue;
                }

                string text = textProp.GetString()!;
                if (text.Length > MaxSentenceLength)
                {
                    violations.Add($"句 #{result.Count} “{Truncate(text, 20)}” 长度 {text.Length} 字超过上限 {MaxSentenceLength} 字（请按子句或逗号进一步拆分）");
                }

                EmotionConfig? emotion = null;
                if (item.TryGetProperty("emotion", out JsonElement emotionProp) &&
                    emotionProp.ValueKind == JsonValueKind.Object)
                {
                    emotion = ParseEmotionFromJson(emotionProp, out List<string> emotionViolations);
                    violations.AddRange(emotionViolations.Select(v => $"句 #{result.Count} {v}"));
                }

                result.Add(new TTSSentenceItem { Text = text, Emotion = emotion });
            }

            return result;
        }

        // 形式 2：ConvertParameters 后的 List<object?>（正常 MCP 调用路径）
        if (sentencesObj is System.Collections.IList list)
        {
            foreach (object? item in list)
            {
                if (item is not Dictionary<string, object?> dict)
                {
                    violations.Add($"句子条目必须是对象: {item}");
                    continue;
                }

                if (!dict.TryGetValue("text", out object? textObj) ||
                    textObj is not string text ||
                    string.IsNullOrWhiteSpace(text))
                {
                    violations.Add("每个句子必须包含非空 'text' 字段");
                    continue;
                }

                if (text.Length > MaxSentenceLength)
                {
                    violations.Add($"句 #{result.Count} “{Truncate(text, 20)}” 长度 {text.Length} 字超过上限 {MaxSentenceLength} 字（请按子句或逗号进一步拆分）");
                }

                EmotionConfig? emotion = null;
                if (dict.TryGetValue("emotion", out object? emotionObj) &&
                    emotionObj is Dictionary<string, object?> emotionDict)
                {
                    emotion = ParseEmotionFromDict(emotionDict, out List<string> emotionViolations);
                    violations.AddRange(emotionViolations.Select(v => $"句 #{result.Count} {v}"));
                }

                result.Add(new TTSSentenceItem { Text = text, Emotion = emotion });
            }

            return result;
        }

        return null;
    }

    /// <summary>
    /// 从 JsonElement 解析单个情感配置对象（测试/直调场景）。
    /// </summary>
    private static EmotionConfig? ParseEmotionFromJson(JsonElement emotionElement, out List<string> violations)
    {
        violations = [];

        string method = "text";
        if (emotionElement.TryGetProperty("method", out JsonElement methodProp) &&
            methodProp.ValueKind == JsonValueKind.String)
        {
            method = methodProp.GetString() ?? "text";
        }

        var config = new EmotionConfig { Method = method };

        if (method == "vector")
        {
            if (!emotionElement.TryGetProperty("vector", out JsonElement vectorProp) ||
                vectorProp.ValueKind != JsonValueKind.Array)
            {
                violations.Add("method=vector 时必须提供 8 维数组 'vector'");
                return null;
            }

            var vector = new List<double>();
            foreach (JsonElement item in vectorProp.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.Number && item.TryGetDouble(out double value))
                {
                    vector.Add(value);
                }
                else
                {
                    violations.Add("vector 数组元素必须是数字");
                }
            }

            if (vector.Count != EmotionVectorLength)
            {
                violations.Add($"vector 必须为 {EmotionVectorLength} 维，实际 {vector.Count} 维");
            }

            foreach (double value in vector)
            {
                if (value < 0 || value > EmotionVectorMax)
                {
                    violations.Add($"vector 单维值 {value} 超出范围 0~{EmotionVectorMax}");
                }
            }

            double sum = vector.Sum();
            if (sum > EmotionVectorSumMax)
            {
                violations.Add($"vector 总和 {sum:F2} 超过上限 {EmotionVectorSumMax}");
            }

            config.Vector = vector.ToArray();
        }
        else
        {
            if (!emotionElement.TryGetProperty("description", out JsonElement descProp) ||
                descProp.ValueKind != JsonValueKind.String ||
                string.IsNullOrWhiteSpace(descProp.GetString()))
            {
                violations.Add("method=text 时必须提供非空 'description' 字段");
                return null;
            }
            config.Description = descProp.GetString();
        }

        return config;
    }

    /// <summary>
    /// 从 Dictionary（ConvertParameters 后的原生类型形式）解析情感配置。
    /// </summary>
    private static EmotionConfig? ParseEmotionFromDict(Dictionary<string, object?> emotionDict, out List<string> violations)
    {
        violations = [];

        string method = "text";
        if (emotionDict.TryGetValue("method", out object? methodObj) && methodObj is string methodStr)
        {
            method = methodStr;
        }

        var config = new EmotionConfig { Method = method };

        if (method == "vector")
        {
            if (!emotionDict.TryGetValue("vector", out object? vectorObj) ||
                vectorObj is not System.Collections.IList vectorList)
            {
                violations.Add("method=vector 时必须提供 8 维数组 'vector'");
                return null;
            }

            var vector = new List<double>();
            foreach (object? item in vectorList)
            {
                if (item is double d)
                    vector.Add(d);
                else if (item is int i)
                    vector.Add(i);
                else if (item is long l)
                    vector.Add(l);
                else if (item is JsonElement je && je.ValueKind == JsonValueKind.Number && je.TryGetDouble(out double jv))
                    vector.Add(jv);
                else
                    violations.Add("vector 数组元素必须是数字");
            }

            if (vector.Count != EmotionVectorLength)
            {
                violations.Add($"vector 必须为 {EmotionVectorLength} 维，实际 {vector.Count} 维");
            }

            foreach (double value in vector)
            {
                if (value < 0 || value > EmotionVectorMax)
                {
                    violations.Add($"vector 单维值 {value} 超出范围 0~{EmotionVectorMax}");
                }
            }

            double sum = vector.Sum();
            if (sum > EmotionVectorSumMax)
            {
                violations.Add($"vector 总和 {sum:F2} 超过上限 {EmotionVectorSumMax}");
            }

            config.Vector = vector.ToArray();
        }
        else
        {
            if (!emotionDict.TryGetValue("description", out object? descObj) ||
                descObj is not string desc ||
                string.IsNullOrWhiteSpace(desc))
            {
                violations.Add("method=text 时必须提供非空 'description' 字段");
                return null;
            }
            config.Description = desc;
        }

        return config;
    }

    /// <summary>
    /// 解析单个情感配置对象（自动分派 JsonElement 或 Dictionary 形式）。
    /// </summary>
    private static EmotionConfig? ParseEmotion(object emotionObj, out List<string> violations)
    {
        if (emotionObj is JsonElement je && je.ValueKind == JsonValueKind.Object)
        {
            return ParseEmotionFromJson(je, out violations);
        }
        if (emotionObj is Dictionary<string, object?> dict)
        {
            return ParseEmotionFromDict(dict, out violations);
        }
        violations = ["emotion 参数格式无效（必须是对象）"];
        return null;
    }

    private static string Truncate(string text, int maxLength)
    {
        return text.Length <= maxLength ? text : text[..maxLength] + "...";
    }
}
