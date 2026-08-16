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

namespace SiliconLife.Common.Skills;

/// <summary>
/// Built-in skill definitions shipped with the framework.
/// Registered for every being by DefaultSiliconBeingFactory.
/// </summary>
public static class BuiltinSkills
{
    /// <summary>Returns all built-in skill definitions.</summary>
    public static List<SkillDefinition> GetAllSkills()
    {
        return new List<SkillDefinition>
        {
            CreateSummarizeDocument(),
            CreateCodeReview(),
            CreateResearchTopic(),
        };
    }

    /// <summary>summarize_document — reads a document, extracts key points, optionally updates the knowledge graph.</summary>
    private static SkillDefinition CreateSummarizeDocument() => new()
    {
        Id = "summarize_document",
        Description = "Read a document from the given URL or file path, extract key points, and optionally update the knowledge graph. Returns a structured summary.",
        DisplayNameKey = "SkillSummarizeDocument",
        Version = "1.0.0",
        Tags = new List<string> { "document", "analysis", "knowledge" },
        ParameterSchema = new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["source"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "URL or file path of the document"
                },
                ["max_points"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum key points (default 10)"
                },
                ["update_knowledge"] = new Dictionary<string, object>
                {
                    ["type"] = "boolean",
                    ["description"] = "Whether to update the knowledge graph"
                },
            },
            ["required"] = new[] { "source" },
        },
        SystemPromptTemplate =
            "You are a document analyst. Read the document from {source}, extract up to {max_points} key points, " +
            "and return a structured summary. If {update_knowledge} is true, use the knowledge tool to add new facts.",
        ToolWhitelist = new List<string> { "disk", "network", "knowledge" },
        MaxToolRound = 5,
        Timeout = TimeSpan.FromSeconds(60),
        OnCompleteAction = "write_memory",
        Source = SkillSource.Builtin,
        TriggerMode = SkillTriggerMode.Manual,
    };

    /// <summary>code_review — reads code files and produces a review report with severity ratings.</summary>
    private static SkillDefinition CreateCodeReview() => new()
    {
        Id = "code_review",
        Description = "Read code files from a directory, analyze for bugs, style issues, and improvement opportunities. Returns a review report with severity ratings.",
        DisplayNameKey = "SkillCodeReview",
        Version = "1.0.0",
        Tags = new List<string> { "code", "review", "quality" },
        ParameterSchema = new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Directory or file path to review"
                },
                ["severity"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "all", "critical", "warning" },
                    ["description"] = "Minimum severity level"
                },
            },
            ["required"] = new[] { "path" },
        },
        SystemPromptTemplate =
            "You are a senior code reviewer. Read the code at {path}, analyze for bugs, style issues, and improvements. " +
            "Report findings with severity {severity} or higher. Use the disk tool to read files and the work_note tool to record the review report.",
        ToolWhitelist = new List<string> { "disk", "work_note" },
        MaxToolRound = 8,
        Timeout = TimeSpan.FromSeconds(120),
        OnCompleteAction = "write_memory",
        Source = SkillSource.Builtin,
        TriggerMode = SkillTriggerMode.Manual,
    };

    /// <summary>research_topic — searches the web, reads pages and synthesizes a cited research report.</summary>
    private static SkillDefinition CreateResearchTopic() => new()
    {
        Id = "research_topic",
        Description = "Research a topic by searching the web, reading relevant pages, and synthesizing findings into a structured report with citations.",
        DisplayNameKey = "SkillResearchTopic",
        Version = "1.0.0",
        Tags = new List<string> { "research", "web", "report" },
        ParameterSchema = new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["topic"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Research topic"
                },
                ["depth"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "brief", "standard", "deep" },
                    ["description"] = "Research depth"
                },
            },
            ["required"] = new[] { "topic" },
        },
        SystemPromptTemplate =
            "You are a research analyst. Research the topic '{topic}' with {depth} depth. Use the network tool to search and read web pages, " +
            "the knowledge tool to check existing knowledge, and the work_note tool to record findings. Cite sources.",
        ToolWhitelist = new List<string> { "network", "knowledge", "work_note", "memory" },
        MaxToolRound = 10,
        Timeout = TimeSpan.FromSeconds(180),
        OnCompleteAction = "write_memory",
        Source = SkillSource.Builtin,
        TriggerMode = SkillTriggerMode.Manual,
    };
}
