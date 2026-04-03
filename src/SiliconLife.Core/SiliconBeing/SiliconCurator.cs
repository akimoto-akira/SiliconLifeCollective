// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SiliconLife.Collective;

/// <summary>
/// Summary information about a silicon being.
/// </summary>
public sealed class BeingSummary
{
    public Guid Id { get; }
    public string Name { get; }
    public bool IsCurator { get; }
    public bool IsCustomCompiled { get; }
    public string? CustomTypeName { get; }
    public bool IsIdle { get; }

    public BeingSummary(Guid id, string name, bool isCurator, bool isCustomCompiled,
        string? customTypeName, bool isIdle)
    {
        Id = id;
        Name = name;
        IsCurator = isCurator;
        IsCustomCompiled = isCustomCompiled;
        CustomTypeName = customTypeName;
        IsIdle = isIdle;
    }
}

/// <summary>
/// Result of a curator operation (compile, save, etc.).
/// </summary>
public sealed class CuratorOperationResult
{
    public bool Success { get; }
    public string Message { get; }
    public List<string> Errors { get; }

    public CuratorOperationResult(bool success, string message, List<string>? errors = null)
    {
        Success = success;
        Message = message;
        Errors = errors ?? [];
    }
}
