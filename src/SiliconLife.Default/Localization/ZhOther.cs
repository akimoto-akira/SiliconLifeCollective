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
/// Chinese (Simplified, Singapore) localization implementation
/// </summary>
public class ZhSG : ZhCN
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "zh-SG";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "简体中文（新加坡）";
}

/// <summary>
/// Chinese (Traditional, Macau) localization implementation
/// </summary>
public class ZhMO : ZhHK
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "zh-MO";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "繁體中文（澳門）";
}

/// <summary>
/// Chinese (Traditional, Taiwan) localization implementation
/// </summary>
public class ZhTW : ZhHK
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "zh-TW";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "繁體中文（台灣）";
}

/// <summary>
/// Chinese (Simplified, Malaysia) localization implementation
/// </summary>
public class ZhMY : ZhCN
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "zh-MY";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "简体中文（马来西亚）";
}
