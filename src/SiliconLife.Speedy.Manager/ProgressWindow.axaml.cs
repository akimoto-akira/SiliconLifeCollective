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

using Avalonia.Controls;

namespace SiliconLife.Speedy.Manager;

public partial class ProgressWindow : Window
{
    public ProgressWindow()
    {
        InitializeComponent();
    }

    public ProgressWindow(string title) : this()
    {
        TitleLabel.Text = title;
    }

    public void UpdateProgress(int current, int total, string detail)
    {
        ProgressBar.Maximum = total;
        ProgressBar.Value = current;
        StatusLabel.Text = detail;
    }

    public void SetIndeterminate(string detail)
    {
        ProgressBar.IsIndeterminate = true;
        StatusLabel.Text = detail;
    }

    public void Complete(string message)
    {
        ProgressBar.IsIndeterminate = false;
        ProgressBar.Value = ProgressBar.Maximum;
        StatusLabel.Text = message;
    }
}
