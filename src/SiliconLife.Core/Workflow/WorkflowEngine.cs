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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SiliconLife.Collective;

/// <summary>
/// Workflow engine core: manages templates and instances, executes Tick-driven state transitions.
/// </summary>
public class WorkflowEngine
{
    private readonly Dictionary<string, WorkflowTemplate> _templates = new();
    private readonly Dictionary<Guid, WorkflowInstance> _instances = new();
    private readonly List<WorkflowLog> _logs = new();
    private readonly ITimeStorage _storage;
    private readonly IServiceProvider _serviceProvider;
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<WorkflowEngine>();
    private readonly object _lock = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowEngine"/> class.
    /// </summary>
    public WorkflowEngine(ITimeStorage storage, IServiceProvider serviceProvider)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    #region Template Management

    /// <summary>
    /// Registers a workflow template.
    /// </summary>
    public void RegisterTemplate(WorkflowTemplate template)
    {
        if (string.IsNullOrEmpty(template.Name))
            throw new ArgumentException("Template name cannot be empty", nameof(template));

        lock (_lock)
        {
            _templates[template.Name] = template;
            _logger.Info(null, "Workflow template registered: {0} ({1} states, {2} transitions)",
                template.Name, template.States.Count, template.Transitions.Count);
        }
    }

    /// <summary>
    /// Gets a workflow template by name.
    /// </summary>
    public WorkflowTemplate? GetTemplate(string templateName)
    {
        lock (_lock)
        {
            return _templates.TryGetValue(templateName, out var template) ? template : null;
        }
    }

    /// <summary>
    /// Gets all registered templates.
    /// </summary>
    public List<WorkflowTemplate> GetAllTemplates()
    {
        lock (_lock)
        {
            return _templates.Values.ToList();
        }
    }

    #endregion

    #region Instance Management

    /// <summary>
    /// Creates and starts a new workflow instance.
    /// If the template defines RoleDefinitions, validates the role pool and logs warnings
    /// for any unsatisfied roles (does not block instance creation — roles may be assigned later).
    /// </summary>
    public async Task<WorkflowInstance> CreateInstanceAsync(
        Guid projectId,
        string templateName,
        string businessKey,
        Guid createdBy)
    {
        var template = GetTemplate(templateName);
        if (template == null)
            throw new ArgumentException($"Workflow template not found: {templateName}");

        var instance = new WorkflowInstance
        {
            ProjectId = projectId,
            TemplateName = templateName,
            BusinessKey = businessKey,
            CurrentState = template.InitialState,
            CreatedBy = createdBy
        };

        // Validate role pool against template's RoleDefinitions (non-blocking)
        ValidateRolePoolForInstance(template, instance);

        lock (_lock)
        {
            _instances[instance.Id] = instance;
        }

        SaveInstance(instance);

        _logger.Info(null, "Workflow instance created: {0} (template={1}, project={2}, key={3})",
            instance.Id, templateName, projectId, businessKey);

        return instance;
    }

    /// <summary>
    /// Validates the project's role assignments against the template's role definitions.
    /// Logs warnings for unsatisfied roles and records them in the instance metadata.
    /// Does not block instance creation — roles may be assigned after creation by the curator.
    /// </summary>
    private void ValidateRolePoolForInstance(WorkflowTemplate template, WorkflowInstance instance)
    {
        if (template.RoleDefinitions.Count == 0)
            return;

        // Attempt to read role assignments from ProjectSpace via ProjectManager
        var projectManager = _serviceProvider.GetService(typeof(IProjectManager)) as IProjectManager;
        if (projectManager == null)
        {
            _logger.Warn(null, "Cannot validate role pool: ProjectManager not available for workflow {0}", instance.Id);
            return;
        }

        var project = projectManager.GetProject(instance.ProjectId);
        if (project == null)
        {
            _logger.Warn(null, "Cannot validate role pool: Project {0} not found for workflow {1}",
                instance.ProjectId, instance.Id);
            return;
        }

        // Read RoleAssignments from ProjectSpace (available after task-347 is implemented).
        // Before task-347, this property doesn't exist yet, so we use reflection to safely check.
        var roleAssignments = GetRoleAssignmentsFromProject(project);
        if (roleAssignments == null)
        {
            // RoleAssignments not yet available (task-347 not implemented),
            // log info and skip validation
            _logger.Info(null, "Role assignments not yet available on project {0}, skipping role validation for workflow {1}",
                instance.ProjectId, instance.Id);

            // Record in metadata that role validation was skipped
            instance.Metadata["RoleValidationStatus"] = "Skipped_NoRoleAssignments";
            instance.Metadata["RequiredRoles"] = string.Join(", ", template.RoleDefinitions.Keys);
            return;
        }

        // Validate role assignments against template definitions
        if (!template.ValidateRoleAssignments(roleAssignments, out var unsatisfiedRoles))
        {
            _logger.Warn(null, "Role pool validation failed for workflow {0}: {1}",
                instance.Id, string.Join(", ", unsatisfiedRoles));

            instance.Metadata["RoleValidationStatus"] = "Unsatisfied";
            instance.Metadata["UnsatisfiedRoles"] = string.Join("; ", unsatisfiedRoles);
        }
        else
        {
            _logger.Info(null, "Role pool validation passed for workflow {0}", instance.Id);
            instance.Metadata["RoleValidationStatus"] = "Satisfied";
        }
    }

    /// <summary>
    /// Safely reads RoleAssignments from ProjectSpace using reflection.
    /// This method provides forward compatibility — it works whether or not
    /// ProjectSpace.RoleAssignments has been added (task-347).
    /// Once task-347 is implemented, this can be replaced with direct property access.
    /// </summary>
    private static Dictionary<string, List<Guid>>? GetRoleAssignmentsFromProject(ProjectSpace project)
    {
        var prop = typeof(ProjectSpace).GetProperty("RoleAssignments");
        if (prop == null)
            return null;

        try
        {
            return prop.GetValue(project) as Dictionary<string, List<Guid>>;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets a workflow instance by ID.
    /// </summary>
    public WorkflowInstance? GetInstance(Guid instanceId)
    {
        lock (_lock)
        {
            return _instances.TryGetValue(instanceId, out var instance) ? instance : null;
        }
    }

    /// <summary>
    /// Gets all workflow instances for a project.
    /// </summary>
    public List<WorkflowInstance> GetProjectInstances(Guid projectId)
    {
        lock (_lock)
        {
            return _instances.Values
                .Where(i => i.ProjectId == projectId && i.Status == "InProgress")
                .OrderByDescending(i => i.UpdatedAt)
                .ToList();
        }
    }

    /// <summary>
    /// Gets all workflow instances (active + terminal).
    /// </summary>
    public List<WorkflowInstance> GetAllInstances()
    {
        lock (_lock)
        {
            return _instances.Values
                .OrderByDescending(i => i.UpdatedAt)
                .ToList();
        }
    }

    #endregion

    #region Tick-Driven State Transition

    /// <summary>
    /// Executes one Tick: checks all active instances and triggers transitions.
    /// Called by MainLoop every 60 seconds.
    /// </summary>
    public async Task TickAsync()
    {
        List<WorkflowInstance> activeInstances;
        lock (_lock)
        {
            activeInstances = _instances.Values
                .Where(i => i.Status == "InProgress")
                .ToList();
        }

        foreach (var instance in activeInstances)
        {
            await TryTransitionAsync(instance);
        }
    }

    /// <summary>
    /// Attempts to transition a workflow instance.
    /// </summary>
    private async Task TryTransitionAsync(WorkflowInstance instance)
    {
        var template = GetTemplate(instance.TemplateName);
        if (template == null)
        {
            _logger.Warn(null, "Template not found for instance {0}, skipping", instance.Id);
            return;
        }

        // Find applicable transitions
        var applicableTransitions = template.Transitions
            .Where(t => t.FromState == instance.CurrentState)
            .OrderBy(t => t.Priority)
            .ToList();

        foreach (var transition in applicableTransitions)
        {
            try
            {
                bool conditionMet = await transition.Condition(instance, _serviceProvider);
                if (!conditionMet)
                    continue;

                // Execute transition
                string fromState = instance.CurrentState;
                instance.CurrentState = transition.ToState;
                instance.MarkProgress();

                // Execute action
                await transition.Action(instance, _serviceProvider);

                // Save and log
                SaveInstance(instance);
                AddLog(new WorkflowLog
                {
                    InstanceId = instance.Id,
                    FromState = fromState,
                    ToState = transition.ToState,
                    TransitionName = transition.TransitionName
                });

                _logger.Info(null, "Workflow transitioned: {0} [{1} -> {2}] ({3})",
                    instance.Id, fromState, transition.ToState, transition.TransitionName);

                // Check if terminal state reached
                if (template.TerminalStates.Contains(transition.ToState))
                {
                    instance.Status = "Completed";
                    SaveInstance(instance);
                    _logger.Info(null, "Workflow completed: {0} (reached terminal state: {1})",
                        instance.Id, transition.ToState);
                }

                return; // Only execute one transition per Tick
            }
            catch (Exception ex)
            {
                _logger.Error(null, $"Transition failed: {instance.Id} ({transition.TransitionName})", ex);
            }
        }

        // Check timeout
        await CheckTimeoutAsync(instance, template);
    }

    /// <summary>
    /// Checks if current transition has timed out.
    /// </summary>
    private async Task CheckTimeoutAsync(WorkflowInstance instance, WorkflowTemplate template)
    {
        var currentTransition = template.Transitions
            .FirstOrDefault(t => t.FromState == instance.CurrentState && t.TimeoutDays > 0);

        if (currentTransition == null)
            return;

        var daysSinceProgress = (DateTime.UtcNow - instance.LastProgressAt).TotalDays;
        if (daysSinceProgress > currentTransition.TimeoutDays)
        {
            _logger.Warn(null, "Workflow timeout detected: {0} (state={1}, days={2:F1}), triggering exception flow",
                instance.Id, instance.CurrentState, daysSinceProgress);

            // Mark instance as blocked due to timeout
            instance.Status = "Blocked";
            instance.Metadata["TimeoutAt"] = DateTime.UtcNow;
            instance.Metadata["TimeoutState"] = instance.CurrentState;
            instance.MarkProgress();
            SaveInstance(instance);

            // Notify the being that created this workflow
            try
            {
                var curator = SiliconBeingManager.GetCuratorBeing();
                if (curator != null)
                {
                    _logger.Info(null, "Notifying curator being {0} about workflow timeout: {1}",
                        curator.Id, instance.Id);
                }
            }
            catch (Exception notifyEx)
            {
                _logger.Error(null, "Failed to notify being about workflow timeout", notifyEx);
            }

            // Log timeout as a workflow event
            AddLog(new WorkflowLog
            {
                InstanceId = instance.Id,
                FromState = instance.CurrentState,
                ToState = instance.CurrentState,
                TransitionName = "Timeout"
            });
        }
    }

    #endregion

    #region Logging

    /// <summary>
    /// Gets workflow transition logs for an instance.
    /// </summary>
    public List<WorkflowLog> GetInstanceLogs(Guid instanceId)
    {
        lock (_lock)
        {
            return _logs
                .Where(l => l.InstanceId == instanceId)
                .OrderBy(l => l.CreatedAt)
                .ToList();
        }
    }

    /// <summary>
    /// Gets all workflow transition logs.
    /// </summary>
    public List<WorkflowLog> GetAllLogs()
    {
        lock (_lock)
        {
            return _logs
                .OrderByDescending(l => l.CreatedAt)
                .ToList();
        }
    }

    #endregion

    #region Persistence

    /// <summary>
    /// Saves a workflow instance to storage.
    /// </summary>
    private void SaveInstance(WorkflowInstance instance)
    {
        try
        {
            var key = $"workflow/instances/{instance.ProjectId}/{instance.Id}.json";
            _storage.Write(key, instance);
        }
        catch (Exception ex)
        {
            _logger.Error(null, $"Failed to save workflow instance: {instance.Id}", ex);
        }
    }

    /// <summary>
    /// Adds a workflow log to storage.
    /// </summary>
    private void AddLog(WorkflowLog log)
    {
        lock (_lock)
        {
            _logs.Add(log);
        }

        try
        {
            var key = $"workflow/logs/{log.InstanceId}/{log.Id}.json";
            _storage.Write(key, log);
        }
        catch (Exception ex)
        {
            _logger.Error(null, $"Failed to save workflow log: {log.Id}", ex);
        }
    }

    #endregion
}
