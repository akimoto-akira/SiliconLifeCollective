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

using System.Text.Json;
using ForgeMind.Bridge;
using SiliconLife.Collective;

namespace ForgeMind;

/// <summary>
/// Unreal Editor interaction tool ("编辑器交互").
/// Routes commands to a running editor through the ForgeMindForUE companion
/// over the TCP bridge. The action set is dynamic — it is whatever the
/// connected companion reports in its handshake ('list' shows it).
/// </summary>
public class UnrealEditorTool : ITool
{
    /// <summary>Per-command response limit (editor operations may be slow).</summary>
    private static readonly TimeSpan CommandTimeout = TimeSpan.FromSeconds(30);

    public string Name => "unreal_editor";

    public string Description =>
        "Interact with a running Unreal Editor through the ForgeMindForUE companion plugin. " +
        "Use action 'status' to check whether any editor is online, or 'list' to see connected " +
        "editors and their supported actions. " +
        "Action 'quit_editor' closes the editor gracefully - the companion plugin initiates the " +
        "shutdown from inside the editor: if the editor has unsaved assets, a prompt appears and " +
        "the editor proceeds to the memory-unload (shutdown) sequence only after the user decides; " +
        "with no unsaved assets it goes straight to unload. The host never kills the editor process. " +
        "set_actor_property: payload keys are 'name' (the actor name exactly as listed by " +
        "list_level_actors), 'property' and 'value'. Object-reference properties (UStaticMesh, " +
        "ACameraActor, ...) take an asset reference as 'value' - either the bare object path " +
        "(/Game/StarterContent/Props/SM_Chair.SM_Chair) or the exported form " +
        "(/Script/Engine.StaticMesh'/Game/.../SM_Chair.SM_Chair'); unloaded assets are loaded " +
        "automatically. For FText properties pass only the describing information as 'value' - " +
        "a plain string, {text, [namespace], [key]} for a gatherable localizable text, " +
        "{stringTable, key} for a string table reference, or {format, args} for a formatted text; " +
        "the companion assembles the full FText on the UE side. " +
        "create_blueprint: payload keys are 'parentClass' (reflection class name, e.g. Actor, " +
        "Pawn, Character - valid names via list_class_hierarchy) and 'path' (project asset path " +
        "to create the blueprint at, e.g. /Game/Blueprints/BP_Thing); the blueprint is created, " +
        "compiled and left modified (dirty) - the editor's save flow (save-all or the close " +
        "prompt) is what actually writes it to that path. " +
        "add_blueprint_variable: payload keys are 'path' (blueprint asset path, e.g. " +
        "/Game/Blueprints/BP_Thing), 'name' (variable name), 'type' (bool, byte, int, int64, " +
        "float, double, name, string, text, vector, rotator, transform, or a UClass name like " +
        "StaticMesh for an object reference) and optional 'default' (default value - a " +
        "scalar for plain types; for structs use a JSON object {\"X\":x,\"Y\":y,\"Z\":z} / " +
        "array [x,y,z] / \"x,y,z\" string for vectors, {\"Pitch\":p,\"Yaw\":y,\"Roll\":r} for " +
        "rotators and {\"Translation\":{X,Y,Z},\"Rotation\":{Pitch,Yaw,Roll},\"Scale\":{X,Y,Z}} " +
        "for transforms; int64 defaults beyond 2^53 must be passed as strings since JSON " +
        "numbers lose precision); also optional 'category' (variable category, default " +
        "'Default'), 'instanceEditable' and 'exposeOnSpawn' (bools); " +
        "the blueprint is recompiled and left modified (dirty). Struct defaults with " +
        "unknown member names still succeed, but the bad members are ignored and the " +
        "response carries a 'warnings' array listing them and the valid member names. " +
        "set_blueprint_variable_default: payload keys are 'path' (blueprint asset path), " +
        "'name' (variable name) and 'default' (new default value - same forms as " +
        "add_blueprint_variable: scalars for plain types, JSON object/array/string for " +
        "structs, or an object path like /Game/Props/SM_Chair.SM_Chair for object " +
        "references); the blueprint is recompiled " +
        "and left modified (dirty). Unknown struct member names are ignored and reported " +
        "in a 'warnings' array (same as add_blueprint_variable). " +
        "update_blueprint_variable: payload keys are 'path' (blueprint asset path) and " +
        "'name' (variable name), plus any subset of the optional settings - 'category' " +
        "(string), 'instanceEditable', 'private', 'config', 'transient', 'saveGame', " +
        "'advancedDisplay', 'deprecated', 'exposeOnSpawn', 'exposeToCinematics' " +
        "(booleans), 'deprecatedMessage' (string), 'replication' ('none' or " +
        "'replicated') and 'replicationCondition' (none, initialOnly, ownerOnly, " +
        "skipOwner, simulatedOnly, autonomousOnly, replayOnly, ...); only the provided " +
        "keys change, the blueprint is recompiled and left modified (dirty). " +
        "list_blueprint_variables: payload keys are 'path' (blueprint asset path) and " +
        "optional 'includeInherited' (bool, default false - also list variables inherited " +
        "from parent blueprints and C++ classes); returns 'variables' where each entry has " +
        "'name', 'type' (same vocabulary as add_blueprint_variable's 'type'), " +
        "'defaultValue', 'category' and the update_blueprint_variable settings " +
        "(instanceEditable, private, config, transient, saveGame, advancedDisplay, " +
        "deprecated, exposeOnSpawn, exposeToCinematics, replication - 'none'/'replicated'/" +
        "'repNotify', replicationCondition, plus repNotifyFunc/deprecatedMessage when set); " +
        "inherited entries additionally carry 'inheritedFrom' and cannot be changed via " +
        "update/remove_blueprint_variable. " +
        "remove_blueprint_variable: payload keys are 'path' (blueprint asset path) and " +
        "'name' (variable name); the removal is refused (inUse=true) while any graph still " +
        "references the variable, otherwise the blueprint is recompiled and left modified (dirty). " +
        "Other actions are forwarded to the companion verbatim with the optional 'payload'. " +
        "Requires 'path' (the .uproject file or its folder) unless exactly one editor is connected.";

    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN => "编辑器交互",
        Language.ZhHK => "編輯器交互",
        Language.JaJP => "エディタ操作",
        Language.KoKR => "에디터 조작",
        _ => "Editor Interaction"
    };

    public Dictionary<string, object> GetParameterSchema()
    {
        // Dynamic action list: the built-ins ('status'/'list') are always
        // available; forwarded actions come from whatever the connected
        // companions advertised in their handshake. Rebuilt on every call,
        // so the AI-visible schema tracks editors connecting/disconnecting.
        List<string> actions = ["status", "list"];
        try
        {
            if (ForgeMindPlugin.BridgeServer.IsRunning)
            {
                foreach (BridgeSession session in ForgeMindPlugin.BridgeServer.GetSessions())
                {
                    foreach (string command in session.Commands)
                    {
                        if (!actions.Contains(command))
                        {
                            actions.Add(command);
                        }
                    }
                }
            }
        }
        catch
        {
            // Server state is volatile (mid startup/shutdown); fall back to the built-ins
        }

        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Command name. 'status' reports whether any editor is online; " +
                                       "'list' shows connected editors; any other entry is a command " +
                                       "advertised by a connected companion. 'quit_editor' closes the " +
                                       "editor gracefully from inside it: if the editor has unsaved " +
                                       "assets, a prompt appears and the editor proceeds to the " +
                                       "memory-unload (shutdown) sequence only after the user decides; " +
                                       "with no unsaved assets it goes straight to unload. While the " +
                                       "save prompt is up the editor thread is frozen - do not probe " +
                                       "it during that window. " +
                                       "Never kill the editor process yourself. " +
                                       "The enum is rebuilt per request and reflects the companions " +
                                       "currently connected; no editor online means only 'status' and 'list'",
                    ["enum"] = actions.Cast<object>().ToArray()
                },
                ["path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Path to the .uproject file (or its folder) identifying which editor to talk to"
                },
                ["payload"] = new Dictionary<string, object>
                {
                    ["type"] = "object",
                    ["description"] = "Action-specific arguments forwarded to the companion"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj) ||
            string.IsNullOrWhiteSpace(actionObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj.ToString()!.Trim().ToLowerInvariant();

        if (action == "status")
            return ExecuteStatus();

        if (action == "list")
            return ExecuteList();

        // Resolve the target session
        BridgeSession? session = ResolveSession(parameters, out string? resolveError);
        if (session == null)
            return ToolResult.Failed(resolveError!);

        // Forward the action with its payload
        JsonElement? payload = null;
        if (parameters.TryGetValue("payload", out object? payloadObj))
        {
            payload = payloadObj is JsonElement element
                ? element
                : JsonSerializer.SerializeToElement(payloadObj);
        }

        try
        {
            BridgeMessage response = session.CallAsync(action, payload, CommandTimeout).GetAwaiter().GetResult();

            if (!string.IsNullOrEmpty(response.Error))
                return ToolResult.Failed($"Companion reported an error for '{action}': {response.Error}");

            return ToolResult.Successful(
                $"'{action}' completed on '{Path.GetFileName(session.ProjectFile)}'",
                new
                {
                    projectFile = session.ProjectFile,
                    action,
                    response = response.Payload
                });
        }
        catch (TimeoutException)
        {
            return ToolResult.Failed($"Editor did not respond to '{action}' within {CommandTimeout.TotalSeconds:0}s");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Bridge call '{action}' failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Reports editor online status. Always succeeds — the 'online' flag carries the answer,
    /// so callers can probe connectivity without distinguishing failure from offline.
    /// </summary>
    private static ToolResult ExecuteStatus()
    {
        if (!ForgeMindPlugin.BridgeServer.IsRunning)
        {
            return ToolResult.Successful("No editor is online - bridge server is not running", new
            {
                online = false,
                reason = "bridge server not running",
                sessions = Array.Empty<object>()
            });
        }

        BridgeSession[] sessions = ForgeMindPlugin.BridgeServer.GetSessions();
        bool online = sessions.Length > 0;

        return ToolResult.Successful(
            online ? $"{sessions.Length} editor(s) online" : "No editor is online",
            new
            {
                online,
                bridgePort = ForgeMindPlugin.BridgeServer.Port,
                sessions = sessions.Select(s => new
                {
                    projectFile = s.ProjectFile,
                    engineVersion = s.EngineVersion,
                    editorPid = s.EditorPid
                }).ToArray()
            });
    }

    /// <summary>
    /// Lists connected editors with their dynamic action sets.
    /// Fails when no editor is connected — callers probing for connectivity should use 'status'.
    /// </summary>
    private static ToolResult ExecuteList()
    {
        if (!ForgeMindPlugin.BridgeServer.IsRunning)
            return ToolResult.Failed("Bridge server is not running");

        BridgeSession[] sessions = ForgeMindPlugin.BridgeServer.GetSessions();
        if (sessions.Length == 0)
        {
            return ToolResult.Failed(
                "No editor is connected - use action 'status' to probe connectivity, " +
                "or launch the project first (unreal_launch) and check ForgeMindForUE");
        }

        return ToolResult.Successful($"{sessions.Length} editor(s) connected", new
        {
            bridgePort = ForgeMindPlugin.BridgeServer.Port,
            sessions = sessions.Select(s => new
            {
                projectFile = s.ProjectFile,
                engineVersion = s.EngineVersion,
                editorPid = s.EditorPid,
                actions = s.Commands
            }).ToArray()
        });
    }

    /// <summary>
    /// Picks the session to talk to: explicit 'path' wins; with exactly one
    /// connected session the path may be omitted.
    /// </summary>
    private static BridgeSession? ResolveSession(Dictionary<string, object> parameters, out string? error)
    {
        error = null;

        if (parameters.TryGetValue("path", out object? pathObj) &&
            !string.IsNullOrWhiteSpace(pathObj?.ToString()))
        {
            FileInfo? projectFile = UnrealProjectTool.ResolveProjectFile(pathObj.ToString()!);
            if (projectFile == null)
            {
                error = $"No valid .uproject found at '{pathObj}'";
                return null;
            }

            BridgeSession? session = ForgeMindPlugin.BridgeServer.GetSessionByProject(projectFile.FullName);
            if (session == null)
            {
                error = $"No companion is connected for '{projectFile.Name}' - " +
                        "verify the editor is running and ForgeMindForUE is enabled (unreal_project 'analyze' → companionPlugin)";
            }

            return session;
        }

        BridgeSession[] sessions = ForgeMindPlugin.BridgeServer.GetSessions();
        if (sessions.Length == 1)
            return sessions[0];

        error = sessions.Length == 0
            ? "No editor is connected — launch the project first (unreal_launch) or check ForgeMindForUE"
            : "Multiple editors are connected — provide 'path' to pick one";
        return null;
    }
}
