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
/// Factory interface for creating AI client instances
/// </summary>
public interface IAIClientFactory
{
    /// <summary>
    /// Creates an AI client instance based on the provided configuration
    /// </summary>
    /// <param name="endpoint">The AI service endpoint URL</param>
    /// <param name="defaultModel">The default model name</param>
    /// <returns>An AI client instance</returns>
    IAIClient CreateClient(string endpoint, string defaultModel);
}
