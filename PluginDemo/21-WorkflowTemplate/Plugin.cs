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
using SiliconLife.Collective;

namespace SiliconLife.Demo.WorkflowTemplate;

/// <summary>
/// Demonstrates a complete business workflow using <see cref="WorkflowTemplate"/>.
/// Unlike the simpler 05-WorkflowPlugin demo (which only shows interface implementation),
/// this example creates a full "PluginOnboarding" workflow with:
/// <list type="bullet">
///   <item><description>5 states with a realistic state machine flow</description></item>
///   <item><description>Multiple transitions with async Condition and Action delegates</description></item>
///   <item><description>RoleDefinitions with MinCount/MaxCount constraints</description></item>
///   <item><description>RequiredRoles for declarative role-based gating</description></item>
///   <item><description>TimeoutDays for stale-state detection</description></item>
///   <item><description>InjectTransitions to extend an existing workflow</description></item>
/// </list>
/// </summary>
public class WorkflowTemplateDemo : IPlugin, IWorkflowPlugin
{
    // ── IPlugin implementation ──────────────────────────────────────

    public string Id => "com.siliconlife.demo.workflowtemplate";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Workflow Template Demo";
    public string GetDescription(Language language) =>
        "Demonstrates a complete business workflow: PluginOnboarding state machine " +
        "with roles, timeouts, async conditions, and transition injection.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
        Console.WriteLine("[WorkflowTemplate] OnLoad: Preparing PluginOnboarding workflow definition.");
    }

    public void OnStart()
    {
        Console.WriteLine("[WorkflowTemplate] OnStart: Workflow templates registered. " +
            "WorkflowTickObject will drive TickAsync() every 60 seconds.");
    }

    public void OnStop()
    {
        Console.WriteLine("[WorkflowTemplate] OnStop: Plugin stopping.");
    }

    public void OnUnload()
    {
        Console.WriteLine("[WorkflowTemplate] OnUnload: Plugin unloaded.");
    }

    // ── IWorkflowPlugin implementation ──────────────────────────────

    /// <summary>
    /// Must match <see cref="IPlugin.Id"/>.
    /// </summary>
    public string PluginId => Id;

    /// <summary>
    /// Registers the "PluginOnboarding" workflow template.
    /// This template models the lifecycle of a plugin submission:
    /// Submitted → Screening → Testing → Approved/Rejected
    /// </summary>
    public List<WorkflowTemplate> RegisterTemplates()
    {
        Console.WriteLine("[WorkflowTemplate] RegisterTemplates: Creating 'PluginOnboarding' workflow.");

        var template = new WorkflowTemplate
        {
            Name = "PluginOnboarding",
            Description = "Complete plugin onboarding workflow with screening, testing, and approval stages",
            States = new List<string>
            {
                "Submitted",    // Initial: plugin has been submitted for review
                "Screening",    // Under initial screening by a Screener
                "Testing",      // Under functional/security testing by a Tester
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
                    Description = "Reviews plugin submissions for code quality, API compliance, and documentation",
                    MinCount = 1,
                    MaxCount = 3
                },
                ["Tester"] = new RoleDefinition
                {
                    RoleName = "Tester",
                    Description = "Performs functional and security testing on screened plugins",
                    MinCount = 1,
                    MaxCount = 0  // Unlimited testers
                }
            },
            Metadata = new Dictionary<string, object>
            {
                ["Category"] = "Onboarding",
                ["Version"] = "1.0",
                ["CreatedBy"] = "WorkflowTemplateDemo"
            }
        };

        // ── Transition 1: Submitted → Screening ────────────────────────
        // Auto-transitions when a Screener is assigned and submission is valid
        template.Transitions.Add(new Transition
        {
            TransitionName = "BeginScreening",
            FromState = "Submitted",
            ToState = "Screening",
            Priority = 0,
            TimeoutDays = 2,  // If stuck in Submitted for 2 days, mark as Blocked
            RequiredRoles = new List<string> { "Screener" },
            Condition = async (instance, serviceProvider) =>
            {
                // Check that plugin metadata has required submission fields
                if (!instance.Metadata.ContainsKey("PluginName"))
                    return false;
                if (!instance.Metadata.ContainsKey("PluginVersion"))
                    return false;

                // Screener role check is handled declaratively via RequiredRoles
                return true;
            },
            Action = async (instance, serviceProvider) =>
            {
                // Record screening start time and assign to first available screener
                instance.StageOutputs["ScreeningStarted"] = new
                {
                    StartedAt = DateTime.UtcNow,
                    PluginName = instance.Metadata.GetValueOrDefault("PluginName", "Unknown"),
                    AssignedBy = "WorkflowEngine"
                };
                instance.MarkProgress();

                Console.WriteLine($"[WorkflowTemplate] Plugin '{instance.BusinessKey}' moved to Screening.");
            }
        });

        // ── Transition 2: Screening → Testing ──────────────────────────
        // Transitions when screening passes (screener marks as passed)
        template.Transitions.Add(new Transition
        {
            TransitionName = "PassScreening",
            FromState = "Screening",
            ToState = "Testing",
            Priority = 0,
            TimeoutDays = 3,  // 3-day timeout for screening stage
            RequiredRoles = new List<string> { "Tester" },
            Condition = async (instance, serviceProvider) =>
            {
                // Check if screener has marked this as passed
                if (!instance.Metadata.ContainsKey("ScreeningResult"))
                    return false;

                var result = instance.Metadata["ScreeningResult"]?.ToString();
                return result == "Passed";
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["ScreeningCompleted"] = new
                {
                    CompletedAt = DateTime.UtcNow,
                    Result = "Passed",
                    ScreenedBy = instance.Metadata.GetValueOrDefault("ScreenedBy", "Unknown")
                };
                instance.MarkProgress();

                Console.WriteLine($"[WorkflowTemplate] Plugin '{instance.BusinessKey}' passed screening, moved to Testing.");
            }
        });

        // ── Transition 3: Screening → Rejected ─────────────────────────
        // Transitions when screening fails
        template.Transitions.Add(new Transition
        {
            TransitionName = "FailScreening",
            FromState = "Screening",
            ToState = "Rejected",
            Priority = 1,  // Lower priority than PassScreening
            Condition = async (instance, serviceProvider) =>
            {
                if (!instance.Metadata.ContainsKey("ScreeningResult"))
                    return false;

                var result = instance.Metadata["ScreeningResult"]?.ToString();
                return result == "Failed";
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["RejectionDetails"] = new
                {
                    RejectedAt = DateTime.UtcNow,
                    Stage = "Screening",
                    Reason = instance.Metadata.GetValueOrDefault("RejectionReason", "Did not meet quality standards")
                };
                instance.MarkProgress();

                Console.WriteLine($"[WorkflowTemplate] Plugin '{instance.BusinessKey}' rejected at Screening stage.");
            }
        });

        // ── Transition 4: Testing → Approved ────────────────────────────
        // Transitions when all tests pass; requires Screener role to be satisfied
        template.Transitions.Add(new Transition
        {
            TransitionName = "ApprovePlugin",
            FromState = "Testing",
            ToState = "Approved",
            Priority = 0,
            TimeoutDays = 5,
            // RequiredRoles: the Screener role must be satisfied for final approval
            // This demonstrates cross-stage role gating
            RequiredRoles = new List<string> { "Screener" },
            Condition = async (instance, serviceProvider) =>
            {
                // Check that testing has passed
                if (!instance.Metadata.ContainsKey("TestResult"))
                    return false;

                var testResult = instance.Metadata["TestResult"]?.ToString();
                if (testResult != "AllPassed")
                    return false;

                // Verify test coverage meets minimum threshold
                if (instance.Metadata.TryGetValue("TestCoverage", out var coverageObj)
                    && coverageObj is double coverage)
                {
                    return coverage >= 0.8; // Require 80% test coverage
                }

                // If no coverage data, accept test pass alone
                return true;
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["ApprovalResult"] = new
                {
                    ApprovedAt = DateTime.UtcNow,
                    TestResult = "AllPassed",
                    ApprovedBy = "WorkflowEngine"
                };
                instance.MarkProgress();

                Console.WriteLine($"[WorkflowTemplate] Plugin '{instance.BusinessKey}' APPROVED!");
            }
        });

        // ── Transition 5: Testing → Rejected ────────────────────────────
        // Transitions when tests fail
        template.Transitions.Add(new Transition
        {
            TransitionName = "FailTesting",
            FromState = "Testing",
            ToState = "Rejected",
            Priority = 1,
            Condition = async (instance, serviceProvider) =>
            {
                if (!instance.Metadata.ContainsKey("TestResult"))
                    return false;

                var testResult = instance.Metadata["TestResult"]?.ToString();
                return testResult == "Failed";
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["RejectionDetails"] = new
                {
                    RejectedAt = DateTime.UtcNow,
                    Stage = "Testing",
                    Reason = instance.Metadata.GetValueOrDefault("TestFailureReason", "Test suite did not pass"),
                    FailedTests = instance.Metadata.GetValueOrDefault("FailedTestCount", 0)
                };
                instance.MarkProgress();

                Console.WriteLine($"[WorkflowTemplate] Plugin '{instance.BusinessKey}' rejected at Testing stage.");
            }
        });

        return new List<WorkflowTemplate> { template };
    }

    /// <summary>
    /// Injects additional transitions into existing workflow templates.
    /// This example injects an "ExpediteReview" transition into the built-in "CodeReview"
    /// workflow, allowing urgent code to skip from Draft directly to Reviewing with
    /// a priority flag.
    /// </summary>
    public void InjectTransitions(WorkflowTemplate template)
    {
        if (template.Name == "CodeReview")
        {
            Console.WriteLine("[WorkflowTemplate] InjectTransitions: Adding 'ExpediteReview' to CodeReview workflow.");

            template.Transitions.Add(new Transition
            {
                TransitionName = "ExpediteReview",
                FromState = "Draft",
                ToState = "Reviewing",
                Priority = 5,  // Higher priority number = lower priority than normal SubmitForReview
                Condition = async (instance, serviceProvider) =>
                {
                    // Only allow expedited review for urgent items
                    return instance.Metadata.ContainsKey("Urgent")
                        && instance.Metadata["Urgent"] is bool urgent
                        && urgent;
                },
                Action = async (instance, serviceProvider) =>
                {
                    instance.StageOutputs["Expedited"] = new
                    {
                        ExpeditedAt = DateTime.UtcNow,
                        Reason = "Marked as urgent"
                    };
                    instance.Metadata["ReviewPriority"] = "High";
                    instance.MarkProgress();
                },
                Metadata = new Dictionary<string, object>
                {
                    ["InjectedBy"] = "WorkflowTemplateDemo",
                    ["Purpose"] = "Allow urgent code reviews to skip normal queuing"
                }
            });
        }
    }
}
