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

namespace SiliconLife.Collective;

/// <summary>
/// Response from AI service
/// </summary>
public class AIResponse
{
    /// <summary>
    /// Gets or sets the model name used for generation
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the generated content
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of tokens in the prompt
    /// </summary>
    public int? PromptTokens { get; set; }

    /// <summary>
    /// Gets or sets the number of tokens in the completion
    /// </summary>
    public int? CompletionTokens { get; set; }

    /// <summary>
    /// Gets or sets the total number of tokens used
    /// </summary>
    public int? TotalTokens { get; set; }

    /// <summary>
    /// Gets or sets whether the response was successful
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// Gets or sets the error message if the request failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful response with the specified content
    /// </summary>
    public static AIResponse Successful(string content, string model = "")
    {
        return new AIResponse
        {
            Content = content,
            Model = model,
            Success = true
        };
    }

    /// <summary>
    /// Creates a failed response with the specified error message
    /// </summary>
    public static AIResponse Failed(string errorMessage)
    {
        return new AIResponse
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}
