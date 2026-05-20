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

using SiliconLife.Collective;

namespace SiliconLife.Common.Localization;

/// <summary>
/// Portuguese (Brazil) localization implementation
/// Inherits from PtPT, override only if Brazilian Portuguese has specific differences
/// </summary>
public class PtBR : PtPT
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "pt-BR";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "Português (Brasil)";
}
