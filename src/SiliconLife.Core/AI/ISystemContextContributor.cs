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
/// Extension point that lets plugins contribute additional system-message
/// content to every AI request built by <see cref="ContextManager"/>.
/// Register implementations via
/// <see cref="ContextManager.RegisterSystemContextContributor"/> (typically
/// from <c>IPlugin.OnLoad</c>) and remove them on unload.
/// Contributions are added right after the scenario context, treated as
/// always-preserved content and counted against the token budget.
/// </summary>
public interface ISystemContextContributor
{
    /// <summary>
    /// Gets the unique identifier of this contributor (used for deduplication
    /// and diagnostics).
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Produces additional system-message content for the given being.
    /// Called once per AI request; return <c>null</c> or an empty string to
    /// contribute nothing for this request. Implementations must be fast and
    /// must not throw — exceptions are swallowed and logged by the caller.
    /// </summary>
    /// <param name="being">The silicon being the request is built for</param>
    /// <returns>System-message text, or <c>null</c>/empty to skip</returns>
    string? GetSystemContext(SiliconBeingBase being);
}
