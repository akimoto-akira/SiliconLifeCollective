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
/// French (Canada) localization implementation
/// Inherits from FrFR, override only if Canadian French has specific differences
/// </summary>
public class FrCA : FrFR
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "fr-CA";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "Français (Canada)";
}

/// <summary>
/// French (Switzerland) localization implementation
/// Inherits from FrFR, override only if Swiss French has specific differences
/// </summary>
public class FrCH : FrFR
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "fr-CH";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "Français (Suisse)";
}
