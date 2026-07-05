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

namespace TravelCodeWikiWithAI.Cldr;

/// <summary>
/// CLDR 数字/货币信息 - 精简版，从 numbers.json 解析
/// </summary>
public class CldrNumberInfo
{
    public string? DefaultNumberingSystem;
    public Dictionary<string, CldrCurrencyInfo> Currencies = new();
}
