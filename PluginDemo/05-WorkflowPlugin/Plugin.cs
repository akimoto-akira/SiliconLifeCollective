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
using System.Threading.Tasks;
using SiliconLife.Collective;

namespace SiliconLife.Demo.WorkflowPlugin;

/// <summary>
/// Demonstrates implementing both <see cref="IPlugin"/> and <see cref="IWorkflowPlugin"/>
/// in a single class. This is the recommended pattern for plugins that want to contribute
/// workflow templates and inject transitions into existing templates.
/// <para>
/// <b>IWorkflowPlugin</b> has three members:
/// <list type="bullet">
///   <item><description><see cref="IWorkflowPlugin.PluginId"/> — unique identifier (must match <see cref="IPlugin.Id"/>)</description></item>
///   <item><description><see cref="IWorkflowPlugin.RegisterTemplates"/> — returns new workflow templates for the engine to register</description></item>
///   <item><description><see cref="IWorkflowPlugin.InjectTransitions"/> — injects additional transition rules into an existing template</description></item>
/// </list>
/// </para>
/// <para>
/// The host calls <see cref="RegisterTemplates"/> once during startup to collect all plugin-defined
/// templates. It then calls <see cref="InjectTransitions"/> for each registered template, allowing
/// plugins to extend workflows defined by other plugins or by the host itself.
/// </para>
/// </summary>
public class WorkflowPluginDemo : IPlugin, IWorkflowPlugin
{
    // ── IPlugin implementation ──────────────────────────────────────

    public string Id => "com.siliconlife.demo.workflowplugin";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Workflow Plugin Demo";
    public string GetDescription(Language language) =>
        "Demonstrates IWorkflowPlugin: RegisterTemplates creates a custom workflow, " +
        "InjectTransitions extends existing templates.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
        // Called once when the plugin DLL is loaded.
        // This is where you validate configuration and prepare resources.
        Console.WriteLine("[WorkflowPlugin] OnLoad: Plugin loaded, preparing workflow definitions.");
    }

    public void OnStart()
    {
        // Called after all plugins have been loaded.
        // At this point, the WorkflowEngine has already registered templates
        // returned by RegisterTemplates and applied InjectTransitions.
        Console.WriteLine("[WorkflowPlugin] OnStart: Plugin started. Workflow templates should be registered by now.");
    }

    public void OnStop()
    {
        // Called when the host is shutting down.
        Console.WriteLine("[WorkflowPlugin] OnStop: Plugin stopping.");
    }

    public void OnUnload()
    {
        // Called when the plugin is being unloaded.
        Console.WriteLine("[WorkflowPlugin] OnUnload: Plugin unloaded.");
    }

    // ── IWorkflowPlugin implementation ──────────────────────────────

    /// <summary>
    /// Must match <see cref="IPlugin.Id"/>. The host uses this to correlate
    /// the plugin with its workflow contributions.
    /// </summary>
    public string PluginId => Id;

    /// <summary>
    /// Returns a list of workflow templates that this plugin contributes.
    /// The host's <see cref="WorkflowEngine"/> will register each template
    /// via <see cref="WorkflowEngine.RegisterTemplate"/>.
    /// </summary>
    public List<WorkflowTemplate> RegisterTemplates()
    {
        Console.WriteLine("[WorkflowPlugin] RegisterTemplates: Creating 'PluginApproval' workflow template.");

        var template = new WorkflowTemplate
        {
            Name = "PluginApproval",
            Description = "A simple approval workflow for plugin submissions",
            States = new List<string>
            {
                "Submitted",    // Initial: plugin has been submitted
                "Reviewing",    // Under review by a screener
                "Approved",     // Approved — terminal state
                "Rejected"      // Rejected — terminal state
            },
            TerminalStates = new List<string>
            {
                "Approved",
                "Rejected"
            },
            RoleDefinitions = new Dictionary<string, RoleDefinition>
            {
                ["Screener"] = new RoleDefinition
                {
                    RoleName = "Screener",
                    Description = "Reviews plugin submissions for quality and security",
                    MinCount = 1,
                    MaxCount = 3
                }
            },
            Metadata = new Dictionary<string, object>
            {
                ["Category"] = "Approval",
                ["Version"] = "1.0"
            }
        };

        // Transition: Submitted → Reviewing (auto-transition when Screener role is satisfied)
        template.Transitions.Add(new Transition
        {
            TransitionName = "StartReview",
            FromState = "Submitted",
            ToState = "Reviewing",
            Priority = 0,
            TimeoutDays = 2,
            RequiredRoles = new List<string> { "Screener" },
            Condition = async (instance, serviceProvider) =>
            {
                // Auto-transition: always allow if Screener role is staffed
                // (RequiredRoles check is handled by WorkflowEngine before Condition is called)
                return true;
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["ReviewStarted"] = new
                {
                    StartedAt = DateTime.UtcNow,
                    StartedBy = "System"
                };
                instance.MarkProgress();
            }
        });

        // Transition: Reviewing → Approved
        template.Transitions.Add(new Transition
        {
            TransitionName = "Approve",
            FromState = "Reviewing",
            ToState = "Approved",
            Priority = 0,
            TimeoutDays = 5,
            RequiredRoles = new List<string> { "Screener" },
            Condition = async (instance, serviceProvider) =>
            {
                // In a real plugin, you would check if the screener has approved
                // (e.g., by checking a task in the task system)
                return instance.Metadata.ContainsKey("ApprovedBy");
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["ApprovalResult"] = new
                {
                    ApprovedAt = DateTime.UtcNow,
                    Status = "Approved"
                };
                instance.MarkProgress();
            }
        });

        // Transition: Reviewing → Rejected
        template.Transitions.Add(new Transition
        {
            TransitionName = "Reject",
            FromState = "Reviewing",
            ToState = "Rejected",
            Priority = 1,
            Condition = async (instance, serviceProvider) =>
            {
                // In a real plugin, check if the screener has rejected
                return instance.Metadata.ContainsKey("RejectedBy");
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["ApprovalResult"] = new
                {
                    RejectedAt = DateTime.UtcNow,
                    Status = "Rejected"
                };
                instance.MarkProgress();
            }
        });

        return new List<WorkflowTemplate> { template };
    }

    /// <summary>
    /// Called by the host for each registered workflow template.
    /// Plugins can use this to inject additional transitions into workflows
    /// defined by other plugins or by the host.
    /// <para>
    /// This example injects a "FastTrack" transition into the built-in "CodeReview"
    /// workflow template, allowing a direct path from Draft to Approved.
    /// </para>
    /// </summary>
    /// <param name="template">An existing workflow template to extend.</param>
    public void InjectTransitions(WorkflowTemplate template)
    {
        // Example: inject a "FastTrack" transition into the CodeReview workflow
        if (template.Name == "CodeReview")
        {
            Console.WriteLine("[WorkflowPlugin] InjectTransitions: Adding 'FastTrack' transition to CodeReview workflow.");

            template.Transitions.Add(new Transition
            {
                TransitionName = "FastTrack",
                FromState = "Draft",
                ToState = "Approved",
                Priority = 10, // Lower priority than SubmitForReview
                Condition = async (instance, serviceProvider) =>
                {
                    // Fast-track only if metadata contains a trusted flag
                    return instance.Metadata.ContainsKey("TrustedAuthor")
                        && instance.Metadata["TrustedAuthor"] is bool trusted
                        && trusted;
                },
                Action = async (instance, serviceProvider) =>
                {
                    instance.StageOutputs["FastTracked"] = new
                    {
                        FastTrackedAt = DateTime.UtcNow,
                        Reason = "Trusted author"
                    };
                    instance.MarkProgress();
                }
            });
        }
    }
}
