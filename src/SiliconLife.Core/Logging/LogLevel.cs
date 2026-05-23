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
/// Defines logging severity levels.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Logs that contain the most detailed messages.
    /// These messages may contain sensitive application data.
    /// </summary>
    Trace = 0,

    /// <summary>
    /// Logs that are used for interactive investigation during development.
    /// These logs should primarily contain information useful for debugging.
    /// </summary>
    Debug = 1,

    /// <summary>
    /// Logs that track the general flow of the application.
    /// These logs should have long-term value.
    /// </summary>
    Information = 2,

    /// <summary>
    /// Logs that highlight an abnormal or unexpected event in the application flow,
    /// but do not otherwise cause the application execution to stop.
    /// </summary>
    Warning = 3,

    /// <summary>
    /// Logs that highlight when the current flow of execution is stopped due to a failure.
    /// These should indicate a failure in the current activity, not an application-wide failure.
    /// </summary>
    Error = 4,

    /// <summary>
    /// Logs that describe an unrecoverable application or system crash,
    /// or a catastrophic failure that requires immediate attention.
    /// </summary>
    Critical = 5,

    /// <summary>
    /// Not used for writing log messages. Specifies that a logging category should not write any messages.
    /// </summary>
    None = 6
}
