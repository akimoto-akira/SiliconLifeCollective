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
/// Represents the staffing status of a workflow role.
/// Used by LocalizationBase.GetRoleStaffingStatusName() for localized display.
/// </summary>
public enum RoleStaffingStatus
{
    /// <summary>
    /// Not enough beings assigned (below MinCount)
    /// </summary>
    Understaffed,

    /// <summary>
    /// Too many beings assigned (above MaxCount)
    /// </summary>
    Overstaffed,

    /// <summary>
    /// Role is fully staffed (at MaxCount limit)
    /// </summary>
    Full,

    /// <summary>
    /// Role has sufficient staffing (at or above MinCount, below MaxCount or unlimited)
    /// </summary>
    Sufficient
}

/// <summary>
/// Defines a role within a workflow template.
/// Roles represent functional responsibilities that silicon beings can be assigned to
/// within a workflow (e.g., POIClassifier, CodeAssigner, ContentWriter, Translator).
/// </summary>
public class RoleDefinition
{
    /// <summary>
    /// Gets or sets the unique role name within the workflow template (e.g., "POIClassifier", "Translator").
    /// </summary>
    public string RoleName { get; set; } = "";

    /// <summary>
    /// Gets or sets the human-readable description of this role's responsibilities.
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// Gets or sets the minimum number of silicon beings required for this role.
    /// The workflow cannot proceed if the role pool has fewer than this number.
    /// Defaults to 1.
    /// </summary>
    public int MinCount { get; set; } = 1;

    /// <summary>
    /// Gets or sets the maximum number of silicon beings allowed for this role.
    /// 0 means unlimited. Defaults to 0 (unlimited).
    /// </summary>
    public int MaxCount { get; set; } = 0;

    /// <summary>
    /// Validates whether the given number of assigned beings satisfies this role's requirements.
    /// </summary>
    /// <param name="assignedCount">The number of beings currently assigned to this role.</param>
    /// <returns>True if the count is within [MinCount, MaxCount] range.</returns>
    public bool IsSatisfied(int assignedCount)
    {
        if (assignedCount < MinCount)
            return false;

        if (MaxCount > 0 && assignedCount > MaxCount)
            return false;

        return true;
    }

    /// <summary>
    /// Gets the staffing status enum for the given assigned count.
    /// Use this with LocalizationBase.GetRoleStaffingStatusName() for localized display.
    /// </summary>
    /// <param name="assignedCount">The number of beings currently assigned to this role.</param>
    /// <returns>The staffing status enum value.</returns>
    public RoleStaffingStatus GetStaffingStatus(int assignedCount)
    {
        if (assignedCount < MinCount)
            return RoleStaffingStatus.Understaffed;

        if (MaxCount > 0 && assignedCount > MaxCount)
            return RoleStaffingStatus.Overstaffed;

        if (MaxCount > 0 && assignedCount == MaxCount)
            return RoleStaffingStatus.Full;

        return RoleStaffingStatus.Sufficient;
    }

    /// <summary>
    /// Gets a formatted staffing detail string using the localization system.
    /// Falls back to English if localization is not available.
    /// </summary>
    /// <param name="assignedCount">The number of beings currently assigned to this role.</param>
    /// <returns>A localized human-readable status string.</returns>
    public string GetStatusText(int assignedCount)
    {
        var status = GetStaffingStatus(assignedCount);

        try
        {
            var loc = LocalizationManager.Instance.GetLocalization(
                Config.Instance?.Data?.Language ?? Language.ZhCN);
            return loc.GetRoleStaffingStatusText(status, MinCount, MaxCount, assignedCount);
        }
        catch
        {
            // Fallback to English if localization not available
            return status switch
            {
                RoleStaffingStatus.Understaffed => $"Understaffed (need {MinCount}, have {assignedCount})",
                RoleStaffingStatus.Overstaffed => $"Overstaffed (max {MaxCount}, have {assignedCount})",
                RoleStaffingStatus.Full => $"Full ({assignedCount}/{MaxCount})",
                RoleStaffingStatus.Sufficient => $"Sufficient ({assignedCount}/{MinCount}+)",
                _ => $"{status} ({assignedCount})"
            };
        }
    }

    public override string ToString() => $"{RoleName}: {Description} (min={MinCount}, max={(MaxCount > 0 ? MaxCount.ToString() : "unlimited")})";
}
