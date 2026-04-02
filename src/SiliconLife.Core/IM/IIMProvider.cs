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

public interface IIMProvider
{
    event EventHandler<IMMessageEventArgs>? MessageReceived;
    event EventHandler? ExitRequested;

    Task StartAsync();
    Task StopAsync();
    Task SendMessageAsync(Guid senderId, Guid receiverId, string content);

    /// <summary>
    /// Asks the user for a permission decision.
    /// Each IM subclass combines the structured parameters with its own localization
    /// to produce the user-facing prompt and collect the response.
    /// </summary>
    /// <param name="permissionType">The type of permission being requested</param>
    /// <param name="resource">The resource path (URL, file path, command, etc.)</param>
    /// <param name="allowCode">6-digit random code for allowing the operation</param>
    /// <param name="denyCode">6-digit random code for denying the operation</param>
    /// <returns>The user's decision result</returns>
    Task<AskPermissionResult> AskPermissionAsync(PermissionType permissionType, string resource, string allowCode, string denyCode);
}

/// <summary>
/// Result of a permission ask interaction.
/// Carries both the decision and an optional cache preference.
/// </summary>
public class AskPermissionResult
{
    /// <summary>Whether the user allowed the operation</summary>
    public bool Allowed { get; init; }

    /// <summary>
    /// Whether the user chose to add this decision to the frequency cache.
    /// Only meaningful when the user actively selects this option.
    /// </summary>
    public bool AddToCache { get; init; }
}

public class IMMessageEventArgs : EventArgs
{
    public ChatMessage Message { get; }

    public IMMessageEventArgs(ChatMessage message)
    {
        Message = message;
    }
}
