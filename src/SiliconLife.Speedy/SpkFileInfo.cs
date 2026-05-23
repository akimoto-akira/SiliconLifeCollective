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

namespace SiliconLife.Speedy;

/// <summary>
/// Header and statistics information about a .spk file.
/// </summary>
public record SpkFileInfo(
    string FilePath,
    long FileSize,
    string Magic,
    ushort Version,
    ushort Flags,
    long DirectoryOffset,
    int DirectoryLength,
    int TotalEntries,
    int JsonEntries,
    int RawEntries,
    int TextEntries
);
