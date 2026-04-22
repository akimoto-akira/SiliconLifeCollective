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

using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Information retrieval tool providing search capabilities.
/// Supports web search, web content extraction, knowledge queries, and local file search.
/// </summary>
public class SearchTool : ITool
{
    public string Name => "search";

    public string Description =>
        "Search for information from various sources. Actions: search (web search), " +
        "extract (web content extraction), ask_knowledge (knowledge base query), " +
        "search_local (local file search)";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

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
                    ["description"] = "Action to perform: search, extract, ask_knowledge, search_local",
                    ["enum"] = new[] { "search", "extract", "ask_knowledge", "search_local" }
                },
                ["query"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Search query (for search action)"
                },
                ["count"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Number of results to return (for search action), default 5"
                },
                ["language"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Search result language (for search action), default 'zh-CN'"
                },
                ["url"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "URL to extract content from (for extract action)"
                },
                ["max_length"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum length of extracted content (for extract action), default 5000"
                },
                ["topic"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Topic to query knowledge base (for ask_knowledge action)"
                },
                ["source"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Knowledge source like 'wikipedia', 'wikidata' (for ask_knowledge action)"
                },
                ["keyword"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Search keyword (for search_local action)"
                },
                ["directory"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Directory to search in (for search_local action), default current working directory"
                },
                ["pattern"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "File matching pattern like '*.cs', '*.json' (for search_local action)"
                },
                ["search_content"] = new Dictionary<string, object>
                {
                    ["type"] = "boolean",
                    ["description"] = "Whether to search file content, default false (for search_local action)"
                },
                ["max_results"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum number of results to return (for search_local action), default 50"
                },
                ["case_sensitive"] = new Dictionary<string, object>
                {
                    ["type"] = "boolean",
                    ["description"] = "Whether to be case sensitive (for search_local action), default false"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj) || string.IsNullOrWhiteSpace(actionObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj.ToString()!.ToLowerInvariant();

        return action switch
        {
            "search" => ExecuteSearch(callerId, parameters),
            "extract" => ExecuteExtract(callerId, parameters),
            "ask_knowledge" => ExecuteAskKnowledge(callerId, parameters),
            "search_local" => ExecuteSearchLocal(callerId, parameters),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteSearch(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("query", out object? queryObj) || string.IsNullOrWhiteSpace(queryObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'query' parameter for search action");
        }

        string query = queryObj.ToString()!;
        int count = 5;
        string language = "zh-CN";

        if (parameters.TryGetValue("count", out object? countObj) && countObj != null)
        {
            if (int.TryParse(countObj.ToString(), out int parsedCount))
            {
                count = parsedCount;
            }
        }

        if (parameters.TryGetValue("language", out object? langObj) && langObj != null)
        {
            language = langObj.ToString()!;
        }

        // For now, return a placeholder since we don't have a search API key configured
        // In a real implementation, this would call a search API like Bing, Google, etc.
        return ToolResult.Successful($"Web search for '{query}' (count: {count}, language: {language}) - " +
                                   "Note: Search API not configured yet. This is a placeholder response.");
    }

    private ToolResult ExecuteExtract(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("url", out object? urlObj) || string.IsNullOrWhiteSpace(urlObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'url' parameter for extract action");
        }

        string url = urlObj.ToString()!;
        int maxLength = 5000;

        if (parameters.TryGetValue("max_length", out object? maxLenObj) && maxLenObj != null)
        {
            if (int.TryParse(maxLenObj.ToString(), out int parsedMaxLen))
            {
                maxLength = parsedMaxLen;
            }
        }

        // Validate URL
        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return ToolResult.Failed("URL must start with http:// or https://");
        }

        // For now, return a placeholder since we don't have an HTML parser configured
        // In a real implementation, this would use AngleSharp or similar to extract content
        return ToolResult.Successful($"Web content extraction from '{url}' (max length: {maxLength}) - " +
                                   "Note: HTML parser not configured yet. This is a placeholder response.");
    }

    private ToolResult ExecuteAskKnowledge(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("topic", out object? topicObj) || string.IsNullOrWhiteSpace(topicObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'topic' parameter for ask_knowledge action");
        }

        string topic = topicObj.ToString()!;
        string source = "wikipedia";

        if (parameters.TryGetValue("source", out object? sourceObj) && sourceObj != null)
        {
            source = sourceObj.ToString()!;
        }

        // For now, return a placeholder
        // In a real implementation, this would query Wikipedia API or other knowledge bases
        return ToolResult.Successful($"Knowledge query for '{topic}' from source '{source}' - " +
                                   "Note: Knowledge base API not configured yet. This is a placeholder response.");
    }

    private ToolResult ExecuteSearchLocal(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("keyword", out object? keywordObj) || string.IsNullOrWhiteSpace(keywordObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'keyword' parameter for search_local action");
        }

        string keyword = keywordObj.ToString()!;
        string directory = parameters.TryGetValue("directory", out object? dirObj) && dirObj != null
            ? dirObj.ToString()!
            : Directory.GetCurrentDirectory();
        string pattern = parameters.TryGetValue("pattern", out object? patternObj) && patternObj != null
            ? patternObj.ToString()!
            : "*.*";
        bool searchContent = parameters.TryGetValue("search_content", out object? searchContentObj) && searchContentObj != null
            && bool.TryParse(searchContentObj.ToString(), out bool parsedSearchContent) && parsedSearchContent;
        int maxResults = 50;
        bool caseSensitive = false;

        if (parameters.TryGetValue("max_results", out object? maxResultsObj) && maxResultsObj != null)
        {
            if (int.TryParse(maxResultsObj.ToString(), out int parsedMaxResults))
            {
                maxResults = parsedMaxResults;
            }
        }

        if (parameters.TryGetValue("case_sensitive", out object? caseSensitiveObj) && caseSensitiveObj != null)
        {
            if (bool.TryParse(caseSensitiveObj.ToString(), out bool parsedCaseSensitive))
            {
                caseSensitive = parsedCaseSensitive;
            }
        }

        try
        {
            if (searchContent)
            {
                return SearchFileContent(keyword, directory, pattern, maxResults, caseSensitive);
            }
            else
            {
                return SearchFileNames(keyword, directory, pattern, maxResults, caseSensitive);
            }
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Local search failed: {ex.Message}");
        }
    }

    private ToolResult SearchFileNames(string keyword, string directory, string pattern, int maxResults, bool caseSensitive)
    {
        var results = new List<string>();
        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        try
        {
            var files = Directory.EnumerateFiles(directory, pattern, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (fileName.Contains(keyword, comparison))
                {
                    results.Add($"{file}");
                    if (results.Count >= maxResults)
                        break;
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Skip directories we don't have access to
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Error searching files: {ex.Message}");
        }

        if (results.Count == 0)
        {
            return ToolResult.Successful($"No files found matching keyword '{keyword}' in {directory}");
        }

        return ToolResult.Successful($"Found {results.Count} files matching keyword '{keyword}':\n" +
                                   string.Join("\n", results));
    }

    private ToolResult SearchFileContent(string keyword, string directory, string pattern, int maxResults, bool caseSensitive)
    {
        var results = new List<string>();
        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        try
        {
            var files = Directory.EnumerateFiles(directory, pattern, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    var content = File.ReadAllText(file);
                    var lines = content.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(keyword, comparison))
                        {
                            results.Add($"{file}:{i + 1}: {lines[i].Trim()}");
                            if (results.Count >= maxResults)
                                break;
                        }
                    }
                }
                catch
                {
                    // Skip files we can't read
                }

                if (results.Count >= maxResults)
                    break;
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Skip directories we don't have access to
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Error searching file contents: {ex.Message}");
        }

        if (results.Count == 0)
        {
            return ToolResult.Successful($"No file contents found matching keyword '{keyword}' in {directory}");
        }

        return ToolResult.Successful($"Found {results.Count} matches for keyword '{keyword}':\n" +
                                   string.Join("\n", results.Take(maxResults)));
    }
}
