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

namespace SiliconLife.Default;

/// <summary>
/// IM-based permission ask handler.
/// Generates random allow/deny codes, delegates to
/// <see cref="IIMProvider.AskPermissionAsync"/> —
/// how to prompt and collect the user's response is entirely up to the IM subclass.
/// </summary>
public class IMPermissionAskHandler : IPermissionAskHandler
{
    private readonly IIMProvider _imProvider;

    /// <summary>
    /// Creates an IM-based permission ask handler.
    /// </summary>
    /// <param name="imProvider">The IM provider that handles user interaction</param>
    public IMPermissionAskHandler(IIMProvider imProvider)
    {
        _imProvider = imProvider;
    }

    /// <summary>
    /// Generates two 6-digit random codes, asks the user via the IM provider,
    /// and returns the decision including optional cache preference.
    /// </summary>
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        string allowCode = Random.Shared.Next(100000, 999999).ToString();
        string denyCode = Random.Shared.Next(100000, 999999).ToString();

        return _imProvider.AskPermissionAsync(permissionType, resource, allowCode, denyCode).GetAwaiter().GetResult();
    }
}
