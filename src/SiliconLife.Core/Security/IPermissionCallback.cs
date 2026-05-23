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
/// Callback interface for permission decisions.
/// Implementations provide domain-specific permission rules
/// (e.g., network whitelists, file path safety rules).
/// </summary>
public interface IPermissionCallback
{
    /// <summary>
    /// Evaluates a permission request and returns a decision.
    /// Return <see cref="PermissionResult.Allowed"/> to permit,
    /// <see cref="PermissionResult.Denied"/> to reject,
    /// or <see cref="PermissionResult.AskUser"/> to defer to user.
    /// </summary>
    /// <param name="callerId">The GUID of the silicon being requesting access</param>
    /// <param name="permissionType">The type of permission being checked</param>
    /// <param name="resource">The resource path (URL, file path, command, etc.)</param>
    /// <returns>The permission decision</returns>
    PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource);
}
