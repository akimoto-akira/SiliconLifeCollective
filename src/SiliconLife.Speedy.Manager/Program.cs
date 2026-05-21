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

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace SiliconLife.Speedy.Manager;

internal static class Program
{
    internal static string? FilePathArgument { get; private set; }
    internal static bool ReadOnlyArgument { get; private set; }

    [STAThread]
    static void Main(string[] args)
    {
        ParseArguments(args);
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    private static void ParseArguments(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] is "--readonly" or "-r")
            {
                ReadOnlyArgument = true;
            }
            else if (!args[i].StartsWith("-"))
            {
                FilePathArgument = args[i];
            }
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
