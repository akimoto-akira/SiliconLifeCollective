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
/// Types of permissions that can be checked for AI-initiated operations.
/// </summary>
public enum PermissionType
{
    /// <summary>Network access (HTTP requests, API calls)</summary>
    NetworkAccess,

    /// <summary>Command-line execution (shell commands, processes)</summary>
    CommandLine,

    /// <summary>File system access (read, write, delete)</summary>
    FileAccess,

    /// <summary>Function invocation (custom code execution)</summary>
    Function,

    /// <summary>Data access (database, storage queries)</summary>
    DataAccess
}
