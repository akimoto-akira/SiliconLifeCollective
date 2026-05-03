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
using System.Threading.Tasks;

namespace SiliconLife.Collective;

/// <summary>
/// Code review workflow template example.
/// States: Draft -> Reviewing -> Approved -> Merged
///                                 -> Rejected -> Rework -> Reviewing
/// </summary>
public static class CodeReviewWorkflow
{
    /// <summary>
    /// Creates the code review workflow template.
    /// </summary>
    public static WorkflowTemplate CreateTemplate()
    {
        var template = new WorkflowTemplate
        {
            Name = "CodeReview",
            Description = "Code review workflow with approval and rework cycles",
            States = new List<string>
            {
                "Draft",
                "Reviewing",
                "Approved",
                "Rejected",
                "Rework",
                "Merged"
            },
            TerminalStates = new List<string>
            {
                "Merged"
            }
        };

        // Transition: Draft -> Reviewing (when code is submitted for review)
        template.Transitions.Add(new Transition
        {
            TransitionName = "SubmitForReview",
            FromState = "Draft",
            ToState = "Reviewing",
            Priority = 0,
            TimeoutDays = 3,
            Condition = async (instance, serviceProvider) =>
            {
                // Check if PR has code changes
                var projectManager = serviceProvider.GetService(typeof(IProjectManager)) as IProjectManager;
                if (projectManager == null) return false;

                // TODO: Check if code review tasks are created
                return instance.CurrentTaskIds.Count > 0;
            },
            Action = async (instance, serviceProvider) =>
            {
                // Create review tasks for reviewers
                var projectManager = serviceProvider.GetService(typeof(IProjectManager)) as IProjectManager;
                if (projectManager == null) return;

                var taskSystem = projectManager.GetTaskSystem(instance.ProjectId);
                if (taskSystem == null) return;

                // Get reviewers from metadata
                if (instance.Metadata.TryGetValue("Reviewers", out var reviewersObj) && reviewersObj is List<Guid> reviewers)
                {
                    foreach (var reviewer in reviewers)
                    {
                        taskSystem.Create(
                            $"Code Review: {instance.BusinessKey}",
                            $"Review the code changes in {instance.BusinessKey}",
                            instance.CreatedBy,
                            new List<Guid> { reviewer },
                            priority: 50
                        );
                    }
                }

                instance.MarkProgress();
            }
        });

        // Transition: Reviewing -> Approved (when all reviewers approve)
        template.Transitions.Add(new Transition
        {
            TransitionName = "Approve",
            FromState = "Reviewing",
            ToState = "Approved",
            Priority = 0,
            TimeoutDays = 5,
            Condition = async (instance, serviceProvider) =>
            {
                var projectManager = serviceProvider.GetService(typeof(IProjectManager)) as IProjectManager;
                if (projectManager == null) return false;

                var taskSystem = projectManager.GetTaskSystem(instance.ProjectId);
                if (taskSystem == null) return false;

                // Check if all review tasks are completed
                var reviewTasks = instance.CurrentTaskIds
                    .Select(id => taskSystem.Get(id))
                    .Where(t => t != null)
                    .ToList();

                return reviewTasks.Count > 0 && reviewTasks.All(t => t!.Status == TaskStatus.Completed);
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["ReviewApproved"] = new
                {
                    ApprovedAt = DateTime.UtcNow,
                    ApprovedBy = "System"
                };
                instance.MarkProgress();
            }
        });

        // Transition: Reviewing -> Rejected (when any reviewer rejects)
        template.Transitions.Add(new Transition
        {
            TransitionName = "Reject",
            FromState = "Reviewing",
            ToState = "Rejected",
            Priority = 1,
            Condition = async (instance, serviceProvider) =>
            {
                var projectManager = serviceProvider.GetService(typeof(IProjectManager)) as IProjectManager;
                if (projectManager == null) return false;

                var taskSystem = projectManager.GetTaskSystem(instance.ProjectId);
                if (taskSystem == null) return false;

                // Check if any review task is rejected
                var reviewTasks = instance.CurrentTaskIds
                    .Select(id => taskSystem.Get(id))
                    .Where(t => t != null)
                    .ToList();

                // TODO: Add rejection status to TaskItem
                return reviewTasks.Any(t => t!.Status == TaskStatus.Failed);
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.MarkProgress();
            }
        });

        // Transition: Rejected -> Rework (auto transition)
        template.Transitions.Add(new Transition
        {
            TransitionName = "StartRework",
            FromState = "Rejected",
            ToState = "Rework",
            Priority = 0,
            Condition = async (_, _) => true, // Auto transition
            Action = async (instance, serviceProvider) =>
            {
                // Create rework task for original author
                var projectManager = serviceProvider.GetService(typeof(IProjectManager)) as IProjectManager;
                if (projectManager == null) return;

                var taskSystem = projectManager.GetTaskSystem(instance.ProjectId);
                if (taskSystem == null) return;

                taskSystem.Create(
                    $"Rework Required: {instance.BusinessKey}",
                    $"Address review comments and fix issues",
                    instance.CreatedBy,
                    new List<Guid> { instance.CreatedBy },
                    priority: 60
                );

                instance.MarkProgress();
            }
        });

        // Transition: Rework -> Reviewing (when rework is completed)
        template.Transitions.Add(new Transition
        {
            TransitionName = "Resubmit",
            FromState = "Rework",
            ToState = "Reviewing",
            Priority = 0,
            Condition = async (instance, serviceProvider) =>
            {
                var projectManager = serviceProvider.GetService(typeof(IProjectManager)) as IProjectManager;
                if (projectManager == null) return false;

                var taskSystem = projectManager.GetTaskSystem(instance.ProjectId);
                if (taskSystem == null) return false;

                // Check if rework task is completed
                var reworkTasks = instance.CurrentTaskIds
                    .Select(id => taskSystem.Get(id))
                    .Where(t => t != null && t.Title.Contains("Rework Required"))
                    .ToList();

                return reworkTasks.Any(t => t!.Status == TaskStatus.Completed);
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.MarkProgress();
            }
        });

        // Transition: Approved -> Merged (when code is merged)
        template.Transitions.Add(new Transition
        {
            TransitionName = "Merge",
            FromState = "Approved",
            ToState = "Merged",
            Priority = 0,
            Condition = async (instance, serviceProvider) =>
            {
                // TODO: Check if code is actually merged (e.g., git merge completed)
                return instance.Metadata.ContainsKey("MergedAt");
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["Merged"] = new
                {
                    MergedAt = DateTime.UtcNow,
                    MergedBy = "System"
                };
                instance.MarkProgress();
            }
        });

        return template;
    }
}
