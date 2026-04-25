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
/// Browser status
/// </summary>
public record BrowserStatus
{
    public bool IsOpen { get; init; }
    public string? CurrentUrl { get; init; }
    public string? PageTitle { get; init; }
    public bool IsLoading { get; init; }
    public DateTime LastOperationTime { get; init; }
    public int TimeoutSetting { get; init; }
    public string? SessionId { get; init; }
}

/// <summary>
/// Element info
/// </summary>
public record ElementInfo
{
    public string TagName { get; init; } = "";
    public string? Text { get; init; }
    public Dictionary<string, string> Attributes { get; init; } = new();
    public bool IsVisible { get; init; }
    public bool IsEnabled { get; init; }
}

/// <summary>
/// Screenshot options
/// </summary>
public record ScreenshotOptions
{
    public ScreenshotFormat Format { get; init; } = ScreenshotFormat.Png;
    public int? X { get; init; }
    public int? Y { get; init; }
    public int? Width { get; init; }
    public int? Height { get; init; }
    public bool FullPage { get; init; }
}

/// <summary>
/// Screenshot format
/// </summary>
public enum ScreenshotFormat
{
    Png,
    Jpeg
}
