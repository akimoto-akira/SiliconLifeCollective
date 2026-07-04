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
using CollectiveTaskStatus = SiliconLife.Collective.TaskStatus;

namespace TravelCodeWikiWithAI.TravelCodeWikiWithAIWorkflow;

/// <summary>
/// 旅游编码维基发布工作流模板 — 7步流程状态机。
///
/// 主线流程（对应 WikiPublicationTick 的7步调度）：
///   DataImport → POIClassify → CodeAssign → ContentCreation → Translation → WikiPublish → Published
///
/// 异常流程：
///   任意工作阶段 → Rejected（任务失败）→ Rework → 回到被拒绝的阶段（ReworkTargetState）
///
/// 转移条件：基于 TaskSystem 中对应阶段任务的完成状态。
/// 角色定义：POIClassifier（步骤3）、CodeAssigner（步骤4）、ContentWriter（步骤5）、Translator（步骤6）。
/// </summary>
public static class TravelCodeWikiPublishWorkflow
{
    // ===== Metadata Keys =====
    private const string ReworkTargetStateKey = "ReworkTargetState";
    private const string RejectedFromStateKey = "RejectedFromState";

    // ===== TaskType constants (aligned with WikiPublicationTick) =====
    private const string TaskType_POIClassification = "POIClassification";
    private const string TaskType_CodeAssignment = "CodeAssignment";
    private const string TaskType_ContentCreation = "ContentCreation";
    private const string TaskType_LanguageTranslation = "LanguageTranslation";
    private const string TaskType_WikiPublish = "WikiPublish";

    /// <summary>
    /// 创建旅游编码维基发布工作流模板。
    /// </summary>
    public static WorkflowTemplate CreateTemplate()
    {
        var template = new WorkflowTemplate
        {
            Name = "TravelCodeWikiPublish",
            Description = "旅游编码维基内容发布工作流（7步流程）：数据导入 → POI分类 → 编码分配 → 内容编写 → 翻译 → 维基发布 → 已发布",
            States = new List<string>
            {
                "DataImport",        // 步骤1-2：数据导入阶段（PBF加载+地理实体构建）
                "POIClassify",       // 步骤3：POI分类阶段
                "CodeAssign",        // 步骤4：编码分配阶段
                "ContentCreation",   // 步骤5：内容编写阶段
                "Translation",       // 步骤6：翻译阶段
                "WikiPublish",       // 步骤7：维基发布阶段
                "Rejected",          // 审核拒绝
                "Rework",            // 返工修改
                "Published"          // 已发布（终止状态）
            },
            TerminalStates = new List<string>
            {
                "Published"
            }
        };

        // ===== 角色定义（4角色映射7步操作） =====
        template.RoleDefinitions = new Dictionary<string, RoleDefinition>
        {
            ["POIClassifier"] = new RoleDefinition
            {
                RoleName = "POIClassifier",
                Description = "POI分类人员，负责将OSM POI数据归类并挂载到地理实体树（步骤3）",
                MinCount = 1,
                MaxCount = 0
            },
            ["CodeAssigner"] = new RoleDefinition
            {
                RoleName = "CodeAssigner",
                Description = "编码分配人员，负责为地理实体分配ISO/自定义标识编码（步骤4）",
                MinCount = 1,
                MaxCount = 0
            },
            ["ContentWriter"] = new RoleDefinition
            {
                RoleName = "ContentWriter",
                Description = "内容创作人员，负责为地理实体编写结构化MediaWiki富文本文章（步骤5）",
                MinCount = 1,
                MaxCount = 0
            },
            ["Translator"] = new RoleDefinition
            {
                RoleName = "Translator",
                Description = "翻译人员，负责将地理实体内容翻译为多语言版本（步骤6）",
                MinCount = 1,
                MaxCount = 0
            }
        };

        // =====================================================================
        // 主线转移：DataImport → POIClassify → CodeAssign → ContentCreation
        //          → Translation → WikiPublish → Published
        // =====================================================================

        // 转移: DataImport → POIClassify（数据导入完成，进入POI分类）
        template.Transitions.Add(new Transition
        {
            TransitionName = "CompleteDataImport",
            FromState = "DataImport",
            ToState = "POIClassify",
            Priority = 0,
            TimeoutDays = 2,
            RequiredRoles = new List<string> { "POIClassifier" },
            Condition = async (instance, serviceProvider) =>
            {
                return true;
            },
            Action = async (instance, serviceProvider) =>
            {
                CreateTaskForStage(instance, serviceProvider,
                    "POI分类", "使用GeoDataTool将OSM POI数据归类并挂载到地理实体树",
                    "POIClassifier", TaskType_POIClassification, "3");
                instance.MarkProgress();
            }
        });

        // 转移: POIClassify → CodeAssign（POI分类完成，进入编码分配）
        template.Transitions.Add(new Transition
        {
            TransitionName = "CompletePOIClassify",
            FromState = "POIClassify",
            ToState = "CodeAssign",
            Priority = 0,
            TimeoutDays = 3,
            RequiredRoles = new List<string> { "CodeAssigner" },
            Condition = async (instance, serviceProvider) =>
            {
                return AreStageTasksCompleted(instance, serviceProvider, TaskType_POIClassification);
            },
            Action = async (instance, serviceProvider) =>
            {
                CreateTaskForStage(instance, serviceProvider,
                    "编码分配", "使用GeoDataTool的assign_code动作为地理实体分配标识编码",
                    "CodeAssigner", TaskType_CodeAssignment, "4");
                instance.MarkProgress();
            }
        });

        // 转移: CodeAssign → ContentCreation（编码分配完成，进入内容编写）
        template.Transitions.Add(new Transition
        {
            TransitionName = "CompleteCodeAssign",
            FromState = "CodeAssign",
            ToState = "ContentCreation",
            Priority = 0,
            TimeoutDays = 2,
            RequiredRoles = new List<string> { "ContentWriter" },
            Condition = async (instance, serviceProvider) =>
            {
                return AreStageTasksCompleted(instance, serviceProvider, TaskType_CodeAssignment);
            },
            Action = async (instance, serviceProvider) =>
            {
                CreateTaskForStage(instance, serviceProvider,
                    "内容编写", "使用GeoContentTool为地理实体编写结构化MediaWiki富文本文章",
                    "ContentWriter", TaskType_ContentCreation, "5");
                instance.MarkProgress();
            }
        });

        // 转移: ContentCreation → Translation（内容编写完成，进入翻译）
        template.Transitions.Add(new Transition
        {
            TransitionName = "CompleteContentCreation",
            FromState = "ContentCreation",
            ToState = "Translation",
            Priority = 0,
            TimeoutDays = 5,
            RequiredRoles = new List<string> { "Translator" },
            Condition = async (instance, serviceProvider) =>
            {
                return AreStageTasksCompleted(instance, serviceProvider, TaskType_ContentCreation);
            },
            Action = async (instance, serviceProvider) =>
            {
                CreateTaskForStage(instance, serviceProvider,
                    "翻译", "使用GeoLanguageTool将地理实体内容翻译为多语言版本",
                    "Translator", TaskType_LanguageTranslation, "6");
                instance.MarkProgress();
            }
        });

        // 转移: Translation → WikiPublish（翻译完成，进入维基发布）
        template.Transitions.Add(new Transition
        {
            TransitionName = "CompleteTranslation",
            FromState = "Translation",
            ToState = "WikiPublish",
            Priority = 0,
            TimeoutDays = 5,
            Condition = async (instance, serviceProvider) =>
            {
                return AreStageTasksCompleted(instance, serviceProvider, TaskType_LanguageTranslation);
            },
            Action = async (instance, serviceProvider) =>
            {
                CreateTaskForStage(instance, serviceProvider,
                    "发布到MediaWiki", "将完成的内容发布到MediaWiki服务器",
                    null, TaskType_WikiPublish, "7");
                instance.MarkProgress();
            }
        });

        // 转移: WikiPublish → Published（发布完成，终止状态）
        template.Transitions.Add(new Transition
        {
            TransitionName = "CompleteWikiPublish",
            FromState = "WikiPublish",
            ToState = "Published",
            Priority = 0,
            TimeoutDays = 2,
            Condition = async (instance, serviceProvider) =>
            {
                return AreStageTasksCompleted(instance, serviceProvider, TaskType_WikiPublish);
            },
            Action = async (instance, serviceProvider) =>
            {
                instance.MarkProgress();
            }
        });

        // =====================================================================
        // 异常转移：各阶段失败 → Rejected
        // =====================================================================

        // POIClassify 失败
        template.Transitions.Add(new Transition
        {
            TransitionName = "FailPOIClassify",
            FromState = "POIClassify",
            ToState = "Rejected",
            Priority = 1,
            Condition = async (instance, serviceProvider) =>
            {
                return HasStageTaskFailed(instance, serviceProvider, TaskType_POIClassification);
            },
            Action = async (instance, serviceProvider) =>
            {
                RecordRejection(instance, "POIClassify", "POI分类任务失败");
            }
        });

        // CodeAssign 失败
        template.Transitions.Add(new Transition
        {
            TransitionName = "FailCodeAssign",
            FromState = "CodeAssign",
            ToState = "Rejected",
            Priority = 1,
            Condition = async (instance, serviceProvider) =>
            {
                return HasStageTaskFailed(instance, serviceProvider, TaskType_CodeAssignment);
            },
            Action = async (instance, serviceProvider) =>
            {
                RecordRejection(instance, "CodeAssign", "编码分配任务失败");
            }
        });

        // ContentCreation 失败
        template.Transitions.Add(new Transition
        {
            TransitionName = "FailContentCreation",
            FromState = "ContentCreation",
            ToState = "Rejected",
            Priority = 1,
            Condition = async (instance, serviceProvider) =>
            {
                return HasStageTaskFailed(instance, serviceProvider, TaskType_ContentCreation);
            },
            Action = async (instance, serviceProvider) =>
            {
                RecordRejection(instance, "ContentCreation", "内容编写任务失败");
            }
        });

        // Translation 失败
        template.Transitions.Add(new Transition
        {
            TransitionName = "FailTranslation",
            FromState = "Translation",
            ToState = "Rejected",
            Priority = 1,
            Condition = async (instance, serviceProvider) =>
            {
                return HasStageTaskFailed(instance, serviceProvider, TaskType_LanguageTranslation);
            },
            Action = async (instance, serviceProvider) =>
            {
                RecordRejection(instance, "Translation", "翻译任务失败");
            }
        });

        // WikiPublish 失败
        template.Transitions.Add(new Transition
        {
            TransitionName = "FailWikiPublish",
            FromState = "WikiPublish",
            ToState = "Rejected",
            Priority = 1,
            Condition = async (instance, serviceProvider) =>
            {
                return HasStageTaskFailed(instance, serviceProvider, TaskType_WikiPublish);
            },
            Action = async (instance, serviceProvider) =>
            {
                RecordRejection(instance, "WikiPublish", "维基发布任务失败");
            }
        });

        // =====================================================================
        // 返工流程：Rejected → Rework → 回到被拒绝的阶段
        // =====================================================================

        // Rejected → Rework（自动进入返工）
        template.Transitions.Add(new Transition
        {
            TransitionName = "StartRework",
            FromState = "Rejected",
            ToState = "Rework",
            Priority = 0,
            Condition = async (_, _) => true,
            Action = async (instance, serviceProvider) =>
            {
                var targetState = instance.Metadata.GetValueOrDefault(RejectedFromStateKey, "POIClassify") as string
                    ?? "POIClassify";
                instance.Metadata[ReworkTargetStateKey] = targetState;

                CreateTaskForStage(instance, serviceProvider,
                    $"返工修正（{targetState}）",
                    $"修正{targetState}阶段中发现的问题，完成后将重新进入{targetState}阶段",
                    null);

                instance.MarkProgress();
            }
        });

        // Rework → POIClassify（返工完成后回到POIClassify）
        template.Transitions.Add(new Transition
        {
            TransitionName = "ReworkToPOIClassify",
            FromState = "Rework",
            ToState = "POIClassify",
            Priority = 0,
            RequiredRoles = new List<string> { "POIClassifier" },
            Condition = async (instance, serviceProvider) =>
            {
                return IsReworkCompleted(instance, serviceProvider)
                    && instance.Metadata.TryGetValue(ReworkTargetStateKey, out var target)
                    && target as string == "POIClassify";
            },
            Action = async (instance, serviceProvider) =>
            {
                CreateTaskForStage(instance, serviceProvider,
                    "POI分类", "使用GeoDataTool将OSM POI数据归类并挂载到地理实体树",
                    "POIClassifier", TaskType_POIClassification, "3");
                RecordReworkCompletion(instance, "POIClassify");
            }
        });

        // Rework → CodeAssign（返工完成后回到CodeAssign）
        template.Transitions.Add(new Transition
        {
            TransitionName = "ReworkToCodeAssign",
            FromState = "Rework",
            ToState = "CodeAssign",
            Priority = 0,
            RequiredRoles = new List<string> { "CodeAssigner" },
            Condition = async (instance, serviceProvider) =>
            {
                return IsReworkCompleted(instance, serviceProvider)
                    && instance.Metadata.TryGetValue(ReworkTargetStateKey, out var target)
                    && target as string == "CodeAssign";
            },
            Action = async (instance, serviceProvider) =>
            {
                CreateTaskForStage(instance, serviceProvider,
                    "编码分配", "使用GeoDataTool的assign_code动作为地理实体分配标识编码",
                    "CodeAssigner", TaskType_CodeAssignment, "4");
                RecordReworkCompletion(instance, "CodeAssign");
            }
        });

        // Rework → ContentCreation（返工完成后回到ContentCreation）
        template.Transitions.Add(new Transition
        {
            TransitionName = "ReworkToContentCreation",
            FromState = "Rework",
            ToState = "ContentCreation",
            Priority = 0,
            RequiredRoles = new List<string> { "ContentWriter" },
            Condition = async (instance, serviceProvider) =>
            {
                return IsReworkCompleted(instance, serviceProvider)
                    && instance.Metadata.TryGetValue(ReworkTargetStateKey, out var target)
                    && target as string == "ContentCreation";
            },
            Action = async (instance, serviceProvider) =>
            {
                CreateTaskForStage(instance, serviceProvider,
                    "内容编写", "使用GeoContentTool为地理实体编写结构化MediaWiki富文本文章",
                    "ContentWriter", TaskType_ContentCreation, "5");
                RecordReworkCompletion(instance, "ContentCreation");
            }
        });

        // Rework → Translation（返工完成后回到Translation）
        template.Transitions.Add(new Transition
        {
            TransitionName = "ReworkToTranslation",
            FromState = "Rework",
            ToState = "Translation",
            Priority = 0,
            RequiredRoles = new List<string> { "Translator" },
            Condition = async (instance, serviceProvider) =>
            {
                return IsReworkCompleted(instance, serviceProvider)
                    && instance.Metadata.TryGetValue(ReworkTargetStateKey, out var target)
                    && target as string == "Translation";
            },
            Action = async (instance, serviceProvider) =>
            {
                CreateTaskForStage(instance, serviceProvider,
                    "翻译", "使用GeoLanguageTool将地理实体内容翻译为多语言版本",
                    "Translator", TaskType_LanguageTranslation, "6");
                RecordReworkCompletion(instance, "Translation");
            }
        });

        // Rework → WikiPublish（返工完成后回到WikiPublish）
        template.Transitions.Add(new Transition
        {
            TransitionName = "ReworkToWikiPublish",
            FromState = "Rework",
            ToState = "WikiPublish",
            Priority = 0,
            Condition = async (instance, serviceProvider) =>
            {
                return IsReworkCompleted(instance, serviceProvider)
                    && instance.Metadata.TryGetValue(ReworkTargetStateKey, out var target)
                    && target as string == "WikiPublish";
            },
            Action = async (instance, serviceProvider) =>
            {
                CreateTaskForStage(instance, serviceProvider,
                    "发布到MediaWiki", "将完成的内容发布到MediaWiki服务器",
                    null, TaskType_WikiPublish, "7");
                RecordReworkCompletion(instance, "WikiPublish");
            }
        });

        return template;
    }

    // =====================================================================
    // 条件检查辅助方法
    // =====================================================================

    /// <summary>
    /// 检查指定 TaskType 的当前阶段任务是否全部完成。
    /// 基于 TaskSystem 中对应任务的完成状态（与 WikiPublicationTick 的 TaskType 一致）。
    /// </summary>
    private static bool AreStageTasksCompleted(WorkflowInstance instance, IServiceProvider serviceProvider, string taskType)
    {
        var taskSystem = GetTaskSystem(instance, serviceProvider);
        if (taskSystem == null) return false;

        var stageTasks = instance.CurrentTaskIds
            .Select(id => taskSystem.Get(id))
            .Where(t => t != null && t.Metadata.TryGetValue("TaskType", out var type) && type == taskType)
            .ToList();

        return stageTasks.Count > 0 && stageTasks.All(t => t!.Status == CollectiveTaskStatus.Completed);
    }

    /// <summary>
    /// 检查指定 TaskType 的当前阶段任务是否有失败。
    /// </summary>
    private static bool HasStageTaskFailed(WorkflowInstance instance, IServiceProvider serviceProvider, string taskType)
    {
        var taskSystem = GetTaskSystem(instance, serviceProvider);
        if (taskSystem == null) return false;

        return instance.CurrentTaskIds
            .Select(id => taskSystem.Get(id))
            .Any(t => t != null
                && t.Metadata.TryGetValue("TaskType", out var type) && type == taskType
                && t.Status == CollectiveTaskStatus.Failed);
    }

    /// <summary>
    /// 检查返工任务是否完成。
    /// </summary>
    private static bool IsReworkCompleted(WorkflowInstance instance, IServiceProvider serviceProvider)
    {
        var taskSystem = GetTaskSystem(instance, serviceProvider);
        if (taskSystem == null) return false;

        var reworkTasks = instance.CurrentTaskIds
            .Select(id => taskSystem.Get(id))
            .Where(t => t != null && t.Title.Contains("返工"))
            .ToList();

        return reworkTasks.Count > 0 && reworkTasks.All(t => t!.Status == CollectiveTaskStatus.Completed);
    }

    // =====================================================================
    // 状态记录辅助方法
    // =====================================================================

    /// <summary>
    /// 记录拒绝信息，保存被拒绝的来源阶段用于返工路由。
    /// </summary>
    private static void RecordRejection(WorkflowInstance instance, string fromState, string reason)
    {
        instance.Metadata[RejectedFromStateKey] = fromState;
        instance.StageOutputs[$"{fromState}Rejected"] = new
        {
            RejectedAt = DateTime.UtcNow,
            RejectedFrom = fromState,
            Reason = reason
        };
        instance.MarkProgress();
    }

    /// <summary>
    /// 记录返工完成信息，清理当前任务列表。
    /// </summary>
    private static void RecordReworkCompletion(WorkflowInstance instance, string targetState)
    {
        instance.StageOutputs["ReworkCompleted"] = new
        {
            CompletedAt = DateTime.UtcNow,
            ReturnToState = targetState
        };
        // 清理返工前的任务ID，为新一轮阶段的任务ID腾出空间
        instance.CurrentTaskIds.Clear();
        instance.MarkProgress();
    }

    // =====================================================================
    // 任务创建与角色分配
    // =====================================================================

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
    /// 分配人员后通过项目广播频道发送提醒消息。
    /// </summary>
    private static void CreateTaskForStage(
        WorkflowInstance instance, IServiceProvider serviceProvider,
        string title, string description, string? roleName = null,
        string? taskType = null, string? step = null)
    {
        var taskSystem = GetTaskSystem(instance, serviceProvider);
        if (taskSystem == null) return;

        var projectManager = serviceProvider.GetService(typeof(IProjectManager)) as IProjectManager;
        var project = projectManager?.GetProject(instance.ProjectId);

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

        var task = taskSystem.Create(
            title: title,
            description: $"{description}{roleInfo}",
            assigneeGuid: executorGuid ?? instance.CreatedBy,
            executorGuid: executorGuid ?? instance.CreatedBy,
            reviewerGuid: null,
            priority: 50
        );

        if (!string.IsNullOrEmpty(taskType))
        {
            task.Metadata["TaskType"] = taskType;
        }
        if (!string.IsNullOrEmpty(step))
        {
            task.Metadata["Step"] = step;
        }

        instance.CurrentTaskIds.Add(task.Id);

        instance.Metadata[$"Task_{task.Id}_CreatedByWorkflow"] = "true";
        instance.Metadata[$"Task_{task.Id}_NeedsAIDecision"] = executorGuid.HasValue ? "false" : "true";
        instance.Metadata[$"Task_{task.Id}_TriggeredBy"] = "02";
        if (!string.IsNullOrEmpty(roleName))
        {
            instance.Metadata[$"Task_{task.Id}_RequiredRole"] = roleName;
            instance.Metadata[$"Task_{task.Id}_ExecutorAssigned"] = executorGuid.HasValue ? "true" : "false";
        }

        TaskCenter.Instance.UpdateTask(task);

        NotifyRoleAssignment(project, task, executorGuid, roleName, title);
    }

    /// <summary>
    /// 通过项目广播频道发送角色分配提醒消息。
    /// 仅在成功分配人员时发送，未分配人员时不发送。
    /// </summary>
    private static void NotifyRoleAssignment(ProjectSpace? project, TaskItem task, Guid? executorGuid, string? roleName, string taskTitle)
    {
        if (project == null || !executorGuid.HasValue || string.IsNullOrEmpty(roleName))
            return;

        var chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null || !project.BroadcastChannelId.HasValue)
            return;

        var being = ServiceLocator.Instance.BeingManager?.GetBeing(executorGuid.Value);
        string beingName = being?.Name ?? executorGuid.Value.ToString();

        string notification = $"📋 新任务分配通知\n" +
                              $"任务：{taskTitle}\n" +
                              $"角色：{roleName}\n" +
                              $"执行人：{beingName}\n" +
                              $"请及时处理。";

        chatSystem.AddMessage(Guid.Empty, project.BroadcastChannelId.Value, notification);
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
