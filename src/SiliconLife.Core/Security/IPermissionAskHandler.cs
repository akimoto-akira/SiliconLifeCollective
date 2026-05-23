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
/// Interface for handling permission prompts when
/// <see cref="PermissionResult.AskUser"/> is returned.
/// </summary>
public interface IPermissionAskHandler
{
    /// <summary>
    /// Prompts the user for a permission decision.
    /// The IM layer combines permissionType and resource with its own localization
    /// to produce the user-facing prompt.
    /// </summary>
    /// <param name="callerId">The GUID of the silicon being requesting access</param>
    /// <param name="permissionType">The type of permission</param>
    /// <param name="resource">The resource path</param>
    /// <returns>The user's decision, including optional cache preference</returns>
    AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource);
}
