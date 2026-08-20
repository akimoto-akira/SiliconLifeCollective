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

using System.Text;
using ForgeMind.Bridge;
using SiliconLife.Collective;

namespace ForgeMind;

/// <summary>
/// Example <see cref="ISystemContextContributor"/> implementation: reports the
/// live state of the ForgeMind UE bridge as a system message so the AI knows
/// which Unreal editors are currently connected and which bridge commands
/// they accept. Contributes nothing while the bridge is not running.
/// </summary>
public class ForgeMindSystemContext : ISystemContextContributor
{
    public string Id => "com.siliconlife.forgemind.bridge";

    public string? GetSystemContext(SiliconBeingBase being)
    {
        ForgeMindBridgeServer server = ForgeMindPlugin.BridgeServer;
        if (!server.IsRunning)
        {
            return null;
        }

        StringBuilder sb = new();
        sb.AppendLine("[ForgeMind UE Bridge Status]");

        var sessions = server.GetSessionSnapshot();
        if (sessions.Count == 0)
        {
            sb.AppendLine("No Unreal Editor is connected to the ForgeMind bridge right now.");
            sb.AppendLine("Bridge-driven editor actions (e.g. listing actors, spawning, property edits) are unavailable until an editor connects.");
            AppendKnowledgeReminder(sb);
            return sb.ToString();
        }

        sb.AppendLine($"Connected Unreal Editors: {sessions.Count}");
        foreach (var (projectFile, engineVersion, commands, isReady) in sessions)
        {
            sb.AppendLine($"- {projectFile} (UE {engineVersion}, {(isReady ? "ready" : "handshaking")})");
            if (isReady && commands.Length > 0)
            {
                sb.AppendLine($"  Supported bridge commands: {string.Join(", ", commands)}");
            }
        }
        sb.AppendLine("Use the ForgeMind editor tools to operate on connected editors; command names above are the exact action identifiers.");
        AppendKnowledgeReminder(sb);
        return sb.ToString();
    }

    /// <summary>
    /// Standing reminder: consult the curated UE knowledge base (unreal_knowledge
    /// tool) before answering or acting on Unreal Engine related questions.
    /// </summary>
    private static void AppendKnowledgeReminder(StringBuilder sb)
    {
        sb.AppendLine("Reminder: when handling any Unreal Engine related question or task, " +
            "first query the UE knowledge base via the 'unreal_knowledge' tool (actions: list / search / get) " +
            "for curated answers and implementation guidance before relying on general knowledge.");
    }
}
