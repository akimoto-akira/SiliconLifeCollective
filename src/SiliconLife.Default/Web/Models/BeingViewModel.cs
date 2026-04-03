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

namespace SiliconLife.Default.Web.Models;

public class BeingViewModel : ViewModelBase
{
    public List<BeingItem> Beings { get; set; } = new();
    public List<string> BeingTypes { get; set; } = new();
}

public class BeingItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = "idle";
    public DateTime CreatedAt { get; set; }
    public DateTime LastActiveAt { get; set; }
    public int TaskCount { get; set; }
}
