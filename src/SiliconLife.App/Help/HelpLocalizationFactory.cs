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

namespace SiliconLife.Help;

/// <summary>
/// Factory for creating help localization instances by language code.
/// </summary>
public static class HelpLocalizationFactory
{
    /// <summary>
    /// Create help localization instance by Language enum
    /// </summary>
    /// <param name="language">Language enum value</param>
    /// <returns>Help localization instance</returns>
    public static HelpLocalizationBase Create(Language language) => language switch
    {
        // Chinese variants
        Language.ZhCN => new HelpLocalizationZhCN(),
        Language.ZhHK => new HelpLocalizationZhHK(),
        Language.ZhSG => new HelpLocalizationZhSG(),
        Language.ZhMO => new HelpLocalizationZhMO(),
        Language.ZhTW => new HelpLocalizationZhTW(),
        Language.ZhMY => new HelpLocalizationZhMY(),
        
        // English variants
        Language.EnUS => new HelpLocalizationEnUS(),
        Language.EnGB => new HelpLocalizationEnGB(),
        Language.EnCA => new HelpLocalizationEnCA(),
        Language.EnAU => new HelpLocalizationEnAU(),
        Language.EnIN => new HelpLocalizationEnIN(),
        Language.EnSG => new HelpLocalizationEnSG(),
        Language.EnZA => new HelpLocalizationEnZA(),
        Language.EnIE => new HelpLocalizationEnIE(),
        Language.EnNZ => new HelpLocalizationEnNZ(),
        Language.EnMY => new HelpLocalizationEnMY(),
        
        // Other languages
        Language.JaJP => new HelpLocalizationJaJP(),
        Language.KoKR => new HelpLocalizationKoKR(),
        Language.EsES => new HelpLocalizationEsES(),
        Language.EsMX => new HelpLocalizationEsMX(),
        Language.CsCZ => new HelpLocalizationCsCZ(),
        Language.PlPL => new HelpLocalizationPlPL(),
        
        // German variants
        Language.DeDE => new HelpLocalizationDeDE(),
        Language.DeAT => new HelpLocalizationDeAT(),
        Language.DeCH => new HelpLocalizationDeCH(),
        Language.DeLU => new HelpLocalizationDeLU(),
        Language.DeLI => new HelpLocalizationDeLI(),
        
        // French variants
        Language.FrFR => new HelpLocalizationFrFR(),
        Language.FrCA => new HelpLocalizationFrCA(),
        Language.FrCH => new HelpLocalizationFrCH(),
        
        _ => new HelpLocalizationEnUS() // Default to English
    };
}