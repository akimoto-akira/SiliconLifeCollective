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

namespace SiliconLife.Plugin.TTS;

/// <summary>
/// 逐句合成任务状态。
/// </summary>
public enum TTSJobStatus
{
    /// <summary>队列中等待。</summary>
    Pending,

    /// <summary>Worker 正在处理。</summary>
    Processing,

    /// <summary>合成成功。</summary>
    Completed,

    /// <summary>失败（超过重试次数）。</summary>
    Failed
}

/// <summary>
/// 情感控制配置。
/// Method 为 "text" 时使用 Description；为 "vector" 时使用 Vector（8 维数组，Tool 层格式，
/// 投递 IndexTTS2 API 前由 IndexTTSClient 转为对象格式，键名
/// happy/angry/sad/fearful/disgusted/melancholic/surprised/calm）。
/// </summary>
public class EmotionConfig
{
    /// <summary>情感控制方式："text" 或 "vector"。</summary>
    public string Method { get; set; } = "text";

    /// <summary>情感文字描述，如 "平静地叙述"（Method=text 时必填）。</summary>
    public string? Description { get; set; }

    /// <summary>
    /// 8 维情感向量：[喜, 怒, 哀, 惧, 厌恶, 低落, 惊喜, 平静]，
    /// 每维 0~1.4，总和 ≤1.5（Method=vector 时必填）。
    /// </summary>
    public double[]? Vector { get; set; }
}

/// <summary>
/// 批次提交时的单个句子项。
/// </summary>
public class TTSSentenceItem
{
    /// <summary>处理后的文本（已完成数字/专有名词转换）。</summary>
    public string Text { get; set; } = "";

    /// <summary>情感配置（可选，省略时跟随音色参考音频的情感）。</summary>
    public EmotionConfig? Emotion { get; set; }
}

/// <summary>
/// 批量合成请求。
/// </summary>
public class TTSBatchRequest
{
    /// <summary>IndexTTS2 已保存的音色 ID。</summary>
    public string VoiceId { get; set; } = "";

    /// <summary>输出目录。</summary>
    public string OutputDir { get; set; } = "";

    /// <summary>待合成的句子列表。</summary>
    public List<TTSSentenceItem> Sentences { get; set; } = new();
}

/// <summary>
/// 单句合成任务（队列元素）。
/// </summary>
public class TTSJob
{
    /// <summary>所属批次 ID。</summary>
    public Guid BatchId { get; set; }

    /// <summary>批次内句子序号（从 0 开始）。</summary>
    public int Index { get; set; }

    /// <summary>待合成文本。</summary>
    public string Text { get; set; } = "";

    /// <summary>情感配置（可选）。</summary>
    public EmotionConfig? Emotion { get; set; }

    /// <summary>音色 ID。</summary>
    public string VoiceId { get; set; } = "";

    /// <summary>输出目录。</summary>
    public string OutputDir { get; set; } = "";

    /// <summary>已重试次数（单句立即重试上限 3 次）。</summary>
    public int RetryCount { get; set; }

    /// <summary>当前状态。</summary>
    public TTSJobStatus Status { get; set; } = TTSJobStatus.Pending;

    /// <summary>生成的本地 WAV 文件路径。</summary>
    public string? OutputFilePath { get; set; }

    /// <summary>失败原因。</summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// 整体进度快照（get_status 返回，快速，不含逐句详情）。
/// </summary>
public class BatchStatus
{
    /// <summary>队列中待处理任务数。</summary>
    public int QueueLength { get; set; }

    /// <summary>Worker 线程是否存活。</summary>
    public bool IsAlive { get; set; }

    /// <summary>是否正在处理任务。</summary>
    public bool IsProcessing { get; set; }

    /// <summary>当前正在处理的句子序号（-1 表示空闲）。</summary>
    public int CurrentJobIndex { get; set; }

    /// <summary>累计成功句数。</summary>
    public int CompletedCount { get; set; }

    /// <summary>当前失败句数。</summary>
    public int FailedCount { get; set; }
}

/// <summary>
/// 逐句状态详情（get_task_detail 返回）。
/// </summary>
public class TaskDetailResult
{
    /// <summary>批次 ID。</summary>
    public Guid BatchId { get; set; }

    /// <summary>批次总句数。</summary>
    public int TotalSentences { get; set; }

    /// <summary>逐句状态。</summary>
    public List<SentenceStatus> Sentences { get; set; } = new();
}

/// <summary>
/// 单句状态条目。
/// </summary>
public class SentenceStatus
{
    /// <summary>批次内句子序号（从 0 开始）。</summary>
    public int Index { get; set; }

    /// <summary>文本预览（超过 50 字截断）。</summary>
    public string Text { get; set; } = "";

    /// <summary>状态：Pending / Processing / Completed / Failed。</summary>
    public string Status { get; set; } = "";

    /// <summary>生成的 WAV 文件路径。</summary>
    public string? OutputFilePath { get; set; }

    /// <summary>失败原因。</summary>
    public string? Error { get; set; }
}

/// <summary>
/// IndexTTS2 已保存音色信息（GET /api/saved-voices 返回）。
/// </summary>
public class SavedVoice
{
    /// <summary>音色 ID。</summary>
    public string Id { get; set; } = "";

    /// <summary>音色名称（合成请求 textContent 中 voiceId 使用该值）。</summary>
    public string Name { get; set; } = "";

    /// <summary>前端展示颜色。</summary>
    public string? Color { get; set; }

    /// <summary>预览音频相对路径。</summary>
    public string? PreviewAudioPath { get; set; }
}
