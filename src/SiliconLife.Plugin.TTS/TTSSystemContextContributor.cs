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
/// 向每次 AI 请求注入 TTS 预处理规则（需求文档 §11.3）。
/// 让 AI 在对话中需要语音合成时，知道如何分句、转换数字/专名、推断情感、调用 tts 工具。
/// 纯静态文本、微秒级返回，满足 GetSystemContext 的快进快出要求。
/// </summary>
public class TTSSystemContextContributor : ISystemContextContributor
{
    /// <inheritdoc/>
    public string Id => "tts_preprocessor_rules";

    /// <inheritdoc/>
    public string? GetSystemContext(SiliconBeingBase being)
    {
        return """
            ## 批量语音合成能力（tts 工具可用）

            当用户要求把文档/文字稿合成语音时，按以下流程执行：

            ### 预处理规则

            **1. 分句**：按句号、问号、叹号、省略号切分句子。引号内不拆分。单句最长 120 字（超过易触发 IndexTTS2 的 max_mel_tokens 截断）。

            **2. 数字转汉字**（根据上下文判断最自然的读法）：
            - 年份：2026 → 二〇二六 或 二零二六（语境判断）
            - 版本号：9 → 九，.NET → dot NET
            - 范围：200-220 → 两百到两百二十
            - 大数：2150 → 两千一百五十

            **3. 专有名词处理**（三层规则）：
            - 约定读音（直接替换）：C# → C Sharp、F# → F Sharp
            - 常见技术缩写（保持英语，让 TTS 读）：.NET / GitHub / ChatGPT / MCP / SSE / API / HTTP
            - 生僻词（自行判断，拿不准则在回复里列出让用户确认）

            **4. 情感推断**：为每句选择最合适的情感，优先用文字描述（如 "平静地叙述"），复杂情感用 8 维向量。

            ### 工作流程

            1. 用 `disk` 工具读取目标 Markdown 文档
            2. 按上述规则逐句预处理
            3. 将不确定项（年份读法、生僻专名）列出来请用户确认
            4. 全部确认后，用 `tts` 工具的 `submit_batch` action 提交句子数组
            5. 任务提交后，用 `get_status` 查询进度

            ### 情感的 8 维向量速查

            顺序固定：[喜, 怒, 哀, 惧, 厌恶, 低落, 惊喜, 平静]
            - "太厉害了！" → 惊喜 [0,0,0,0,0,0,1.0,0]
            - "真是荒谬……" → 厌恶 [0,0,0,0,0.8,0,0,0]
            - "为什么？" → 疑问平静 [0,0,0,0,0,0,0.5,0.5]

            默认：[0,0,0,0,0,0,0,1]（平静）

            ### 约束

            - Markdown 符号清除但保留文字
            - 保证每句"读起来像人说话"
            - 全部预处理完再一次性提交（不要逐句调 tts）
            """;
    }
}
