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

namespace SiliconLife.Help;

/// <summary>
/// Abstract base class for help documentation localization.
/// Defines all help document content as abstract properties.
/// </summary>
public abstract class HelpLocalizationBase
{
    #region Help Documents (Markdown Content)

    /// <summary>Getting Started</summary>
    public abstract string GettingStarted { get; }

    /// <summary>Being Management</summary>
    public abstract string BeingManagement { get; }

    /// <summary>Chat System</summary>
    public abstract string ChatSystem { get; }

    /// <summary>Dashboard</summary>
    public abstract string Dashboard { get; }

    /// <summary>Tasks</summary>
    public abstract string Task { get; }

    /// <summary>Timers</summary>
    public abstract string Timer { get; }

    /// <summary>Permission Management</summary>
    public abstract string Permission { get; }

    /// <summary>Configuration</summary>
    public abstract string Config { get; }

    /// <summary>FAQ</summary>
    public abstract string FAQ { get; }

    /// <summary>Memory System</summary>
    public abstract string Memory { get; }

    /// <summary>Ollama Setup</summary>
    public abstract string OllamaSetup { get; }

    /// <summary>Bailian DashScope</summary>
    public abstract string BailianDashScope { get; }

    /// <summary>Volcengine Ark</summary>
    public abstract string VolcengineArk { get; }

    /// <summary>Herdsman Setup</summary>
    public abstract string HerdsmanSetup { get; }

    /// <summary>LongCat Setup</summary>
    public abstract string LongCatSetup { get; }

    /// <summary>Qiniu AI Setup</summary>
    public abstract string QiniuAISetup { get; }

    /// <summary>DeepSeek Setup</summary>
    public abstract string DeepSeekSetup { get; }

    /// <summary>Zhipu Setup</summary>
    public abstract string ZhipuSetup { get; }

    /// <summary>Moonshot Setup</summary>
    public abstract string MoonshotSetup { get; }

    /// <summary>SiliconFlow Setup</summary>
    public abstract string SiliconFlowSetup { get; }

    /// <summary>MiniMax Setup</summary>
    public abstract string MiniMaxSetup { get; }

    /// <summary>Ernie Setup</summary>
    public abstract string ErnieSetup { get; }

    /// <summary>Hunyuan Setup</summary>
    public abstract string HunyuanSetup { get; }

    /// <summary>AI Clients</summary>
    public abstract string AIClients { get; }

    /// <summary>Being Soul</summary>
    public abstract string BeingSoul { get; }

    /// <summary>Audit Log</summary>
    public abstract string AuditLog { get; }

    /// <summary>Knowledge Graph</summary>
    public abstract string KnowledgeGraph { get; }

    /// <summary>Work Notes</summary>
    public abstract string WorkNotes { get; }

    /// <summary>Projects</summary>
    public abstract string Projects { get; }

    /// <summary>Logging System</summary>
    public abstract string Logging { get; }

    /// <summary>Skills</summary>
    public abstract string Skills { get; }

    #endregion

    #region Help Document Titles (Display Titles)

    /// <summary>Getting Started Title</summary>
    public abstract string GettingStarted_Title { get; }

    /// <summary>Being Management Title</summary>
    public abstract string BeingManagement_Title { get; }

    /// <summary>Chat System Title</summary>
    public abstract string ChatSystem_Title { get; }

    /// <summary>Dashboard Title</summary>
    public abstract string Dashboard_Title { get; }

    /// <summary>Tasks Title</summary>
    public abstract string Task_Title { get; }

    /// <summary>Timers Title</summary>
    public abstract string Timer_Title { get; }

    /// <summary>Permission Management Title</summary>
    public abstract string Permission_Title { get; }

    /// <summary>Configuration Title</summary>
    public abstract string Config_Title { get; }

    /// <summary>FAQ Title</summary>
    public abstract string FAQ_Title { get; }

    /// <summary>Memory System Title</summary>
    public abstract string Memory_Title { get; }

    /// <summary>Ollama Setup Title</summary>
    public abstract string OllamaSetup_Title { get; }

    /// <summary>Bailian DashScope Title</summary>
    public abstract string BailianDashScope_Title { get; }

    /// <summary>Volcengine Ark Title</summary>
    public abstract string VolcengineArk_Title { get; }

    /// <summary>Herdsman Setup Title</summary>
    public abstract string HerdsmanSetup_Title { get; }

    /// <summary>LongCat Setup Title</summary>
    public abstract string LongCatSetup_Title { get; }

    /// <summary>Qiniu AI Setup Title</summary>
    public abstract string QiniuAISetup_Title { get; }

    /// <summary>DeepSeek Setup Title</summary>
    public abstract string DeepSeekSetup_Title { get; }

    /// <summary>Zhipu Setup Title</summary>
    public abstract string ZhipuSetup_Title { get; }

    /// <summary>Moonshot Setup Title</summary>
    public abstract string MoonshotSetup_Title { get; }

    /// <summary>SiliconFlow Setup Title</summary>
    public abstract string SiliconFlowSetup_Title { get; }

    /// <summary>MiniMax Setup Title</summary>
    public abstract string MiniMaxSetup_Title { get; }

    /// <summary>Ernie Setup Title</summary>
    public abstract string ErnieSetup_Title { get; }

    /// <summary>Hunyuan Setup Title</summary>
    public abstract string HunyuanSetup_Title { get; }

    /// <summary>AI Clients Title</summary>
    public abstract string AIClients_Title { get; }

    /// <summary>Being Soul Title</summary>
    public abstract string BeingSoul_Title { get; }

    /// <summary>Audit Log Title</summary>
    public abstract string AuditLog_Title { get; }

    /// <summary>Knowledge Graph Title</summary>
    public abstract string KnowledgeGraph_Title { get; }

    /// <summary>Work Notes Title</summary>
    public abstract string WorkNotes_Title { get; }

    /// <summary>Projects Title</summary>
    public abstract string Projects_Title { get; }

    /// <summary>Logging System Title</summary>
    public abstract string Logging_Title { get; }

    /// <summary>Skills Title</summary>
    public abstract string Skills_Title { get; }

    #endregion

    #region Help Document Tags (Search Tags)

    /// <summary>Getting Started Tags</summary>
    public abstract string[] GettingStarted_Tags { get; }

    /// <summary>Being Management Tags</summary>
    public abstract string[] BeingManagement_Tags { get; }

    /// <summary>Chat System Tags</summary>
    public abstract string[] ChatSystem_Tags { get; }

    /// <summary>Dashboard Tags</summary>
    public abstract string[] Dashboard_Tags { get; }

    /// <summary>Tasks Tags</summary>
    public abstract string[] Task_Tags { get; }

    /// <summary>Timers Tags</summary>
    public abstract string[] Timer_Tags { get; }

    /// <summary>Permission Management Tags</summary>
    public abstract string[] Permission_Tags { get; }

    /// <summary>Configuration Tags</summary>
    public abstract string[] Config_Tags { get; }

    /// <summary>FAQ Tags</summary>
    public abstract string[] FAQ_Tags { get; }

    /// <summary>Memory System Tags</summary>
    public abstract string[] Memory_Tags { get; }

    /// <summary>Ollama Setup Tags</summary>
    public abstract string[] OllamaSetup_Tags { get; }

    /// <summary>Bailian DashScope Tags</summary>
    public abstract string[] BailianDashScope_Tags { get; }

    /// <summary>Volcengine Ark Tags</summary>
    public abstract string[] VolcengineArk_Tags { get; }

    /// <summary>Herdsman Setup Tags</summary>
    public abstract string[] HerdsmanSetup_Tags { get; }

    /// <summary>LongCat Setup Tags</summary>
    public abstract string[] LongCatSetup_Tags { get; }

    /// <summary>Qiniu AI Setup Tags</summary>
    public abstract string[] QiniuAISetup_Tags { get; }

    /// <summary>DeepSeek Setup Tags</summary>
    public abstract string[] DeepSeekSetup_Tags { get; }

    /// <summary>Zhipu Setup Tags</summary>
    public abstract string[] ZhipuSetup_Tags { get; }

    /// <summary>Moonshot Setup Tags</summary>
    public abstract string[] MoonshotSetup_Tags { get; }

    /// <summary>SiliconFlow Setup Tags</summary>
    public abstract string[] SiliconFlowSetup_Tags { get; }

    /// <summary>MiniMax Setup Tags</summary>
    public abstract string[] MiniMaxSetup_Tags { get; }

    /// <summary>Ernie Setup Tags</summary>
    public abstract string[] ErnieSetup_Tags { get; }

    /// <summary>Hunyuan Setup Tags</summary>
    public abstract string[] HunyuanSetup_Tags { get; }

    /// <summary>AI Clients Tags</summary>
    public abstract string[] AIClients_Tags { get; }

    /// <summary>Being Soul Tags</summary>
    public abstract string[] BeingSoul_Tags { get; }

    /// <summary>Audit Log Tags</summary>
    public abstract string[] AuditLog_Tags { get; }

    /// <summary>Knowledge Graph Tags</summary>
    public abstract string[] KnowledgeGraph_Tags { get; }

    /// <summary>Work Notes Tags</summary>
    public abstract string[] WorkNotes_Tags { get; }

    /// <summary>Projects Tags</summary>
    public abstract string[] Projects_Tags { get; }

    /// <summary>Logging System Tags</summary>
    public abstract string[] Logging_Tags { get; }

    /// <summary>Skills Tags</summary>
    public abstract string[] Skills_Tags { get; }

    #endregion
}