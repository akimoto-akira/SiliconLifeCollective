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

namespace SiliconLife.Default;

/// <summary>
/// Interface for AI client factory to provide help documentation
/// </summary>
public interface IAIClientFactoryHelp
{
    /// <summary>
    /// Gets the help documentation topic ID for this AI client factory.
    /// Returns null if no help documentation is available.
    /// </summary>
    /// <returns>Help topic ID (e.g., "ollama-setup", "bailian-dashscope"), or null if not available</returns>
    string? GetHelpTopicId();
}
