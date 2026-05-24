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

namespace TravelCodeWikiWithAI.TravelCodeWikiWithAIWorkflow;

/// <summary>
/// 旅游编码维基发布工作流模板。
/// 工作流阶段：DataImport -> QualityCheck -> ContentReview -> WikiPublish -> Published
///                                  -> Rejected -> Rework -> QualityCheck
/// </summary>
public static class TravelCodeWikiPublishWorkflow
{
    /// <summary>
    /// 创建旅游编码维基发布工作流模板。
    /// </summary>
    public static WorkflowTemplate CreateTemplate()
    {
        var template = new WorkflowTemplate
        {
            Name = "TravelCodeWikiPublish",
            Description = "旅游编码维基内容发布工作流：数据导入 -> 质量检查 -> 内容审核 -> 维基发布",
            States = new List<string>
            {
                "DataImport",        // 数据导入阶段
                "QualityCheck",      // 质量检查阶段
                "ContentReview",     // 内容审核阶段
                "Rejected",          // 审核拒绝
                "Rework",            // 返工修改
                "WikiPublish",       // 维基发布阶段
                "Published"          // 已发布（终止状态）
            },
            TerminalStates = new List<string>
            {
                "Published"
            }
        };

        // 声明工作流所需的角色定义
        template.RoleDefinitions = new Dictionary<string, RoleDefinition>
        {
            ["POIClassifier"] = new RoleDefinition
            {
                RoleName = "POIClassifier",
                Description = "POI分类人员，负责将OSM POI数据归类并挂载到地理实体树",
                MinCount = 1,
                MaxCount = 0
            },
            ["CodeAssigner"] = new RoleDefinition
            {
                RoleName = "CodeAssigner",
                Description = "编码分配人员，负责为地理实体分配ISO/自定义标识编码",
                MinCount = 1,
                MaxCount = 0
            },
            ["ContentWriter"] = new RoleDefinition
            {
                RoleName = "ContentWriter",
                Description = "内容创作人员，负责为地理实体编写结构化MediaWiki富文本文章",
                MinCount = 1,
                MaxCount = 0
            },
            ["Translator"] = new RoleDefinition
            {
                RoleName = "Translator",
                Description = "翻译人员，负责将地理实体内容翻译为多语言版本",
                MinCount = 1,
                MaxCount = 0
            }
        };

        // 转移 1: DataImport -> QualityCheck（数据导入完成后自动进入质量检查）
        template.Transitions.Add(new Transition
        {
            TransitionName = "CompleteDataImport",
            FromState = "DataImport",
            ToState = "QualityCheck",
            Priority = 0,
            TimeoutDays = 2,
            Condition = async (instance, serviceProvider) =>
            {
                // 检查数据导入任务是否完成
                var taskSystem = GetTaskSystem(instance, serviceProvider);
                if (taskSystem == null) return false;

                var importTasks = instance.CurrentTaskIds
                    .Select(id => taskSystem.Get(id))
                    .Where(t => t != null && t.Title.Contains("数据导入"))
                    .ToList();

                return importTasks.Count > 0 && importTasks.All(t => t!.Status == SiliconLife.Collective.TaskStatus.Completed);
            },
            Action = async (instance, serviceProvider) =>
            {
                // 创建质量检查任务
                CreateTaskForStage(instance, serviceProvider, "OSM数据质量检查", "检查导入的OSM数据完整性和准确性", "CodeAssigner");
                instance.MarkProgress();
            }
        });

        // 转移 2: QualityCheck -> ContentReview（质量检查通过后进入内容审核）
        template.Transitions.Add(new Transition
        {
            TransitionName = "PassQualityCheck",
            FromState = "QualityCheck",
            ToState = "ContentReview",
            Priority = 0,
            TimeoutDays = 3,
            Condition = async (instance, serviceProvider) =>
            {
                var taskSystem = GetTaskSystem(instance, serviceProvider);
                if (taskSystem == null) return false;

                var qualityTasks = instance.CurrentTaskIds
                    .Select(id => taskSystem.Get(id))
                    .Where(t => t != null && t.Title.Contains("质量检查"))
                    .ToList();

                return qualityTasks.Count > 0 && qualityTasks.All(t => t!.Status == SiliconLife.Collective.TaskStatus.Completed);
            },
            Action = async (instance, serviceProvider) =>
            {
                // 创建内容审核任务
                CreateTaskForStage(instance, serviceProvider, "维基内容审核", "审核旅游编码维基内容的准确性和完整性", "ContentWriter");
                instance.StageOutputs["QualityCheckPassed"] = new
                {
                    CheckedAt = DateTime.UtcNow,
                    Result = "Passed"
                };
                instance.MarkProgress();
            }
        });

        // 转移 3: QualityCheck -> Rejected（质量检查不通过）
        template.Transitions.Add(new Transition
        {
            TransitionName = "FailQualityCheck",
            FromState = "QualityCheck",
            ToState = "Rejected",
            Priority = 1,
            Condition = async (instance, serviceProvider) =>
            {
                var taskSystem = GetTaskSystem(instance, serviceProvider);
                if (taskSystem == null) return false;

                var qualityTasks = instance.CurrentTaskIds
                    .Select(id => taskSystem.Get(id))
                    .Where(t => t != null && t.Title.Contains("质量检查"))
                    .ToList();

                return qualityTasks.Any(t => t!.Status == SiliconLife.Collective.TaskStatus.Failed);
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["QualityCheckFailed"] = new
                {
                    FailedAt = DateTime.UtcNow,
                    Reason = "数据质量检查未通过"
                };
                instance.MarkProgress();
            }
        });

        // 转移 4: ContentReview -> WikiPublish（内容审核通过后准备发布）
        template.Transitions.Add(new Transition
        {
            TransitionName = "ApproveContent",
            FromState = "ContentReview",
            ToState = "WikiPublish",
            Priority = 0,
            TimeoutDays = 5,
            Condition = async (instance, serviceProvider) =>
            {
                var taskSystem = GetTaskSystem(instance, serviceProvider);
                if (taskSystem == null) return false;

                var reviewTasks = instance.CurrentTaskIds
                    .Select(id => taskSystem.Get(id))
                    .Where(t => t != null && t.Title.Contains("内容审核"))
                    .ToList();

                return reviewTasks.Count > 0 && reviewTasks.All(t => t!.Status == SiliconLife.Collective.TaskStatus.Completed);
            },
            Action = async (instance, serviceProvider) =>
            {
                // 创建维基发布任务
                CreateTaskForStage(instance, serviceProvider, "发布到MediaWiki", "将审核通过的内容发布到MediaWiki服务器", "Translator");
                instance.StageOutputs["ContentApproved"] = new
                {
                    ApprovedAt = DateTime.UtcNow,
                    ApprovedBy = "Content Reviewer"
                };
                instance.MarkProgress();
            }
        });

        // 转移 5: ContentReview -> Rejected（内容审核不通过）
        template.Transitions.Add(new Transition
        {
            TransitionName = "RejectContent",
            FromState = "ContentReview",
            ToState = "Rejected",
            Priority = 1,
            Condition = async (instance, serviceProvider) =>
            {
                var taskSystem = GetTaskSystem(instance, serviceProvider);
                if (taskSystem == null) return false;

                var reviewTasks = instance.CurrentTaskIds
                    .Select(id => taskSystem.Get(id))
                    .Where(t => t != null && t.Title.Contains("内容审核"))
                    .ToList();

                return reviewTasks.Any(t => t!.Status == SiliconLife.Collective.TaskStatus.Failed);
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["ContentRejected"] = new
                {
                    RejectedAt = DateTime.UtcNow,
                    Reason = "内容审核未通过"
                };
                instance.MarkProgress();
            }
        });

        // 转移 6: Rejected -> Rework（自动进入返工）
        template.Transitions.Add(new Transition
        {
            TransitionName = "StartRework",
            FromState = "Rejected",
            ToState = "Rework",
            Priority = 0,
            Condition = async (_, _) => true, // 自动转移
            Action = async (instance, serviceProvider) =>
            {
                // 创建返工任务
                CreateTaskForStage(instance, serviceProvider, "数据返工修正", "修正质量检查或内容审核中发现的问题"); // No specific role for rework
                instance.MarkProgress();
            }
        });

        // 转移 7: Rework -> QualityCheck（返工完成后重新检查）
        template.Transitions.Add(new Transition
        {
            TransitionName = "CompleteRework",
            FromState = "Rework",
            ToState = "QualityCheck",
            Priority = 0,
            Condition = async (instance, serviceProvider) =>
            {
                var taskSystem = GetTaskSystem(instance, serviceProvider);
                if (taskSystem == null) return false;

                var reworkTasks = instance.CurrentTaskIds
                    .Select(id => taskSystem.Get(id))
                    .Where(t => t != null && t.Title.Contains("返工"))
                    .ToList();

                return reworkTasks.Count > 0 && reworkTasks.All(t => t!.Status == SiliconLife.Collective.TaskStatus.Completed);
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["ReworkCompleted"] = new
                {
                    CompletedAt = DateTime.UtcNow
                };
                instance.MarkProgress();
            }
        });

        // 转移 8: WikiPublish -> Published（发布完成）
        template.Transitions.Add(new Transition
        {
            TransitionName = "CompleteWikiPublish",
            FromState = "WikiPublish",
            ToState = "Published",
            Priority = 0,
            TimeoutDays = 2,
            Condition = async (instance, serviceProvider) =>
            {
                var taskSystem = GetTaskSystem(instance, serviceProvider);
                if (taskSystem == null) return false;

                var publishTasks = instance.CurrentTaskIds
                    .Select(id => taskSystem.Get(id))
                    .Where(t => t != null && t.Title.Contains("发布"))
                    .ToList();

                return publishTasks.Count > 0 && publishTasks.All(t => t!.Status == SiliconLife.Collective.TaskStatus.Completed);
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.StageOutputs["Published"] = new
                {
                    PublishedAt = DateTime.UtcNow,
                    WikiUrl = instance.Metadata.GetValueOrDefault("WikiUrl", "N/A")
                };
                instance.MarkProgress();
            }
        });

        return template;
    }

    /// <summary>
    /// 获取项目任务系统。
    /// </summary>
    private static ProjectTaskSystem? GetTaskSystem(WorkflowInstance instance, IServiceProvider serviceProvider)
    {
        var projectManager = serviceProvider.GetService(typeof(IProjectManager)) as IProjectManager;
        if (projectManager == null) return null;

        return projectManager.GetTaskSystem(instance.ProjectId);
    }

    /// <summary>
    /// 为指定阶段创建任务。
    /// 如果指定了角色名，则从项目角色池中轮询选择执行者；
    /// 角色池为空时创建任务但不指派执行者（等待主理人分配角色）。
    /// </summary>
    private static void CreateTaskForStage(
        WorkflowInstance instance, IServiceProvider serviceProvider,
        string title, string description, string? roleName = null)
    {
        var taskSystem = GetTaskSystem(instance, serviceProvider);
        if (taskSystem == null) return;

        var projectManager = serviceProvider.GetService(typeof(IProjectManager)) as IProjectManager;
        var project = projectManager?.GetProject(instance.ProjectId);

        // Resolve executor from role pool if roleName is specified
        Guid? executorGuid = null;
        string roleInfo = "";

        if (!string.IsNullOrEmpty(roleName) && project != null)
        {
            executorGuid = SelectExecutorFromRole(instance, project, roleName);

            if (executorGuid.HasValue)
            {
                var being = ServiceLocator.Instance.BeingManager?.GetBeing(executorGuid.Value);
                string beingName = being?.Name ?? executorGuid.Value.ToString();
                roleInfo = $"\n\n**角色分配**：此任务需要「{roleName}」角色，已自动分配给 {beingName}。";
            }
            else
            {
                roleInfo = $"\n\n**角色未分配**：此任务需要「{roleName}」角色，但角色池中暂无人员。请主理人使用 assign_role 分配角色后重新触发工作流。";
            }
        }

        // Build project member list info
        string projectInfo = "";
        if (project != null && project.AssignedBeings.Count > 0)
        {
            projectInfo = "\n\n**项目成员列表**：\n";
            foreach (var beingId in project.AssignedBeings)
            {
                var being = ServiceLocator.Instance.BeingManager?.GetBeing(beingId);
                if (being != null)
                {
                    string role = being.IsCurator ? "项目经理" : "成员";
                    projectInfo += $"- {being.Name} ({beingId}) - {role}\n";
                }
            }
        }

        // Create task with resolved executor (or unassigned if role pool is empty)
        var task = taskSystem.Create(
            title: title,
            description: $"{description}{roleInfo}{projectInfo}",
            assigneeGuid: executorGuid ?? instance.CreatedBy,
            executorGuid: executorGuid ?? instance.CreatedBy,
            reviewerGuid: null,
            priority: 50
        );

        // Add task ID to workflow instance
        instance.CurrentTaskIds.Add(task.Id);
        
        // Record task creation metadata
        instance.Metadata[$"Task_{task.Id}_CreatedByWorkflow"] = "true";
        instance.Metadata[$"Task_{task.Id}_NeedsAIDecision"] = executorGuid.HasValue ? "false" : "true";
        instance.Metadata[$"Task_{task.Id}_TriggeredBy"] = "02"; // System broadcast entity
        if (!string.IsNullOrEmpty(roleName))
        {
            instance.Metadata[$"Task_{task.Id}_RequiredRole"] = roleName;
            instance.Metadata[$"Task_{task.Id}_ExecutorAssigned"] = executorGuid.HasValue ? "true" : "false";
        }
    }

    /// <summary>
    /// 使用轮询策略从项目角色池中选择一个执行者。
    /// 每个角色维护独立的轮询计数器（存储在 WorkflowInstance.Metadata 中），
    /// 确保同一角色的任务均匀分配给池中的硅基人。
    /// </summary>
    private static Guid? SelectExecutorFromRole(WorkflowInstance instance, ProjectSpace project, string roleName)
    {
        if (!project.RoleAssignments.TryGetValue(roleName, out var beings) || beings.Count == 0)
            return null;

        // Get or initialize round-robin counter for this role
        string counterKey = $"RoundRobin_{roleName}";
        int counter = 0;
        if (instance.Metadata.TryGetValue(counterKey, out var counterObj) && counterObj is int c)
        {
            counter = c;
        }

        // Select being using round-robin
        int index = counter % beings.Count;
        var selectedBeing = beings[index];

        // Update counter for next round
        instance.Metadata[counterKey] = counter + 1;

        return selectedBeing;
    }
}
