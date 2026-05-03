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
using System.Threading.Tasks;

namespace SiliconLife.Collective;

/// <summary>
/// Tick object that drives the workflow engine on a regular interval.
/// </summary>
public class WorkflowTickObject : TickObject
{
    private readonly WorkflowEngine _engine;
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<WorkflowTickObject>();

    /// <summary>
    /// Initializes a new instance of the WorkflowTickObject class.
    /// </summary>
    /// <param name="engine">The workflow engine to drive</param>
    /// <param name="interval">Tick interval (default: 60 seconds)</param>
    public WorkflowTickObject(WorkflowEngine engine, TimeSpan? interval = null)
        : base(interval ?? TimeSpan.FromSeconds(60), autoRegister: true)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        Priority = 90; // Lower priority than beings (100), but higher than background tasks
        _logger.Info(null, "WorkflowTickObject registered with interval={0}s", interval?.TotalSeconds ?? 60);
    }

    /// <summary>
    /// Executes one tick of the workflow engine.
    /// </summary>
    protected override async void OnTick(TimeSpan deltaTime)
    {
        try
        {
            await _engine.TickAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Workflow tick failed", ex);
        }
    }
}
