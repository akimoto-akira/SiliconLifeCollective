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

namespace SiliconLife.Demo.CSSourceMode;

/// <summary>
/// This file is NOT listed in cs.txt, so it will NOT be compiled by PluginLoader.
/// It exists solely to demonstrate the cs.txt selective loading mechanism.
///
/// If you want this file to be included in compilation, add "Helpers.cs" to cs.txt.
/// If you want ALL .cs files to be compiled (no selective loading), delete cs.txt entirely.
/// </summary>
public static class SimpleHelper
{
    public static string Greet(string name)
    {
        return $"Hello, {name}! (from SimpleHelper)";
    }
}
