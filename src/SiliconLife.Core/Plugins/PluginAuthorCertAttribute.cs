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
/// Marks a plugin assembly as authored by a certified (first-party) author.
/// Apply this attribute at the <b>assembly level</b>.
/// </summary>
/// <remarks>
/// <para>The <see cref="PluginLoader"/> reads this declaration directly from PE metadata
/// (the CustomAttribute table) before running the security scan. When the supplied
/// <see cref="CertGuid"/> matches the expected value derived from the assembly name
/// (HMACSHA256 with the host's built-in salt, first 16 bytes interpreted as a GUID),
/// the plugin is treated as first-party and <b>all security scan rules are bypassed</b>.
/// Capability declarations (<see cref="PluginCapabilityAttribute"/>) are still read and
/// honored for runtime permission bookkeeping.</para>
/// <para>A certificate GUID is bound to a single assembly name — a GUID valid for one
/// plugin does not grant bypass to any other plugin.</para>
/// <para><b>Example:</b></para>
/// <code>
/// [assembly: PluginAuthorCert("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx")]
/// </code>
/// </remarks>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public sealed class PluginAuthorCertAttribute : Attribute
{
    /// <summary>
    /// Initializes a new certified-author declaration with the certificate GUID.
    /// </summary>
    /// <param name="certGuid">
    /// The certificate GUID issued for this exact assembly name. Any format accepted
    /// by <see cref="Guid.Parse(string)"/> is fine; comparison is value-based.
    /// </param>
    public PluginAuthorCertAttribute(string certGuid)
    {
        CertGuid = certGuid;
    }

    /// <summary>
    /// Gets the certificate GUID string as declared in the assembly metadata.
    /// </summary>
    public string CertGuid { get; }
}
