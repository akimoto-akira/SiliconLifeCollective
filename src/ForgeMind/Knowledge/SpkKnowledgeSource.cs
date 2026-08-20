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

using System.Text;
using SiliconLife.Speedy;

namespace ForgeMind.Knowledge;

/// <summary>
/// Release-stage knowledge backend: reads the textbook from a dedicated
/// read-only SpeedyPack (<c>.spk</c>) with the same virtual layout as the
/// folder backend — <c>{version}/index.json</c> and <c>{version}/entries/{id}.md</c>.
/// </summary>
internal sealed class SpkKnowledgeSource : IKnowledgeSource
{
    private readonly SpeedyPack _pack;

    public SpkKnowledgeSource(string spkPath)
    {
        _pack = SpeedyPack.Open(spkPath, new SpeedyPackOptions { ReadOnly = true });
    }

    public string Description => "spk:" + _pack.GetFileInfo().FilePath;

    public IReadOnlyList<UeVersion> ListVersions()
    {
        try
        {
            return _pack.ListDirectories("")
                .Select(dir => new UeVersion(dir.TrimEnd('/')))
                .Where(version => version.IsValidBucketName)
                .ToArray();
        }
        catch
        {
            return [];
        }
    }

    public IReadOnlyList<UeKnowledgeEntry> ListEntries(UeVersion version)
    {
        if (!version.IsValidBucketName)
            return [];

        try
        {
            byte[]? data = _pack.Read(version.Bucket + "/index.json");
            return data == null ? [] : KnowledgeJson.ParseIndex(Encoding.UTF8.GetString(data)).Entries;
        }
        catch
        {
            return [];
        }
    }

    public string? ReadEntryBody(UeVersion version, string id)
    {
        if (!version.IsValidBucketName || !UeVersion.IsValidId(id))
            return null;

        try
        {
            byte[]? data = _pack.Read($"{version.Bucket}/entries/{id}.md");
            return data == null ? null : Encoding.UTF8.GetString(data);
        }
        catch
        {
            return null;
        }
    }

    public void Dispose() => _pack.Dispose();
}
