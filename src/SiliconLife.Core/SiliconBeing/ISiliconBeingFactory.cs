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
/// Interface for creating silicon being instances
/// </summary>
public interface ISiliconBeingFactory
{
    /// <summary>
    /// Creates a silicon being with the specified ID and name
    /// </summary>
    /// <param name="id">The unique identifier for the silicon being</param>
    /// <param name="name">The name of the silicon being</param>
    /// <returns>The created silicon being instance</returns>
    SiliconBeingBase CreateBeing(Guid id, string name);

    /// <summary>
    /// Loads a silicon being from its directory
    /// </summary>
    /// <param name="beingDirectory">The directory path of the silicon being</param>
    /// <returns>The loaded silicon being instance, or null if loading fails</returns>
    SiliconBeingBase? LoadBeing(string beingDirectory);
}
