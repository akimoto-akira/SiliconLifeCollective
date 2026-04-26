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

namespace SiliconLife.Default;

/// <summary>
/// German (Austria) localization implementation
/// Inherits from DeDE, override only if Austrian German has specific differences
/// </summary>
public class DeAT : DeDE
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "de-AT";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "Deutsch (Österreich)";
}

/// <summary>
/// German (Switzerland) localization implementation
/// Inherits from DeDE, override only if Swiss German has specific differences
/// </summary>
public class DeCH : DeDE
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "de-CH";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "Deutsch (Schweiz)";
}

/// <summary>
/// German (Luxembourg) localization implementation
/// Inherits from DeDE, override only if Luxembourg German has specific differences
/// </summary>
public class DeLU : DeDE
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "de-LU";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "Deutsch (Luxemburg)";
}

/// <summary>
/// German (Liechtenstein) localization implementation
/// Inherits from DeDE, override only if Liechtenstein German has specific differences
/// </summary>
public class DeLI : DeDE
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "de-LI";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "Deutsch (Liechtenstein)";
}
