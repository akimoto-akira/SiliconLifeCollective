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

using System;
using System.Collections.Generic;

namespace SiliconLife.Collective;

/// <summary>
/// Simple service provider implementation for workflow engine.
/// Delegates to ServiceLocator for service resolution.
/// </summary>
public class ServiceProvider : IServiceProvider
{
    /// <summary>
    /// Gets a service of the specified type.
    /// </summary>
    public object? GetService(Type serviceType)
    {
        try
        {
            // Use reflection to call ServiceLocator.Instance.Get<T>()
            var method = typeof(ServiceLocator).GetMethod("Get")?.MakeGenericMethod(serviceType);
            return method?.Invoke(ServiceLocator.Instance, null);
        }
        catch
        {
            return null;
        }
    }
}
