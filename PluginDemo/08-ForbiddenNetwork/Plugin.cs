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

using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.Security;
using System.Net.WebSockets;
using System.Text;
using SiliconLife.Collective;

namespace SiliconLife.Demo.ForbiddenNetwork;

/// <summary>
/// ⚠️ ANTI-PATTERN: Demonstrates network operations that are FORBIDDEN in plugins.
/// 
/// Direct network access types are globally banned from plugins:
/// - System.Net.Http.HttpClient
/// - System.Net.HttpWebRequest/HttpWebResponse
/// - System.Net.Sockets.TcpClient, UdpClient, Socket
/// - System.Net.Mail.SmtpClient
/// - System.Net.Dns
/// - System.Net.WebClient
/// - System.Net.Security.SslStream (when used directly)
/// - System.Net.WebSockets.ClientWebSocket
/// 
/// These operations bypass the plugin security audit and could:
/// 1. Make requests to malicious servers
/// 2. Exfiltrate sensitive data from the sandbox
/// 3. Perform DNS rebinding attacks
/// 4. Bypass network ACLs and restrictions
/// 
/// ✅ CORRECT APPROACH: Use NetworkExecutor for all network operations.
/// NetworkExecutor provides permission checking, audit logging, and circuit breaker protection.
/// 
/// This plugin demonstrates what NOT to do. Each violation is marked with
/// ⚠️ VIOLATION comment and shows the correct alternative.
/// </summary>
public class ForbiddenNetworkPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.forbiddennetwork";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Forbidden Network Access Anti-Pattern";
    public string GetDescription(Language language) =>
        "Demonstrates FORBIDDEN network operations and their correct alternatives. " +
        "Shows why direct network access is banned and how to use NetworkExecutor.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== FORBIDDEN NETWORK ACCESS ANTI-PATTERNS ==========");
        Console.WriteLine("⚠️  This plugin demonstrates operations that will be BLOCKED by PluginLoader.\n");

        // NOTE: These methods are commented out because they would cause compilation errors
        // when the plugin is loaded through PluginLoader (due to TypeRef scanning).
        // In a real scenario, PluginLoader would reject this plugin during loading.

        DemonstrateHttpClient();
        DemonstrateHttpWebRequest();
        DemonstrateTcpClient();
        DemonstrateUdpClient();
        DemonstrateSocket();
        DemonstrateDns();
        DemonstrateSmtpClient();
        DemonstrateWebClient();
        DemonstrateClientWebSocket();

        Console.WriteLine("\n========== CORRECT ALTERNATIVES ==========");
        DemonstrateCorrectApproach();
    }

    /// <summary>
    /// ⚠️ VIOLATION: HttpClient
    /// TypeRef blocked: System.Net.Http.HttpClient
    /// </summary>
    private void DemonstrateHttpClient()
    {
        Console.WriteLine("[Violation 1] HttpClient");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Net.Http.HttpClient");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     using var client = new HttpClient();");
        Console.WriteLine("     var response = await client.GetStringAsync(\"https://api.example.com/data\");");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     var result = NetworkExecutor.Execute(new ExecutorRequest");
        Console.WriteLine("     {");
        Console.WriteLine("         ResourcePath = \"https://api.example.com/data\",");
        Console.WriteLine("         Parameters = { { \"method\", \"GET\" } }");
        Console.WriteLine("     });");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: HttpWebRequest/HttpWebResponse
    /// TypeRef blocked: System.Net.HttpWebRequest, System.Net.HttpWebResponse
    /// </summary>
    private void DemonstrateHttpWebRequest()
    {
        Console.WriteLine("[Violation 2] HttpWebRequest");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Net.HttpWebRequest");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     var request = WebRequest.Create(\"https://api.example.com\");");
        Console.WriteLine("     var response = request.GetResponse();");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     var result = NetworkExecutor.Execute(new ExecutorRequest");
        Console.WriteLine("     {");
        Console.WriteLine("         ResourcePath = \"https://api.example.com\"");
        Console.WriteLine("     });");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: TcpClient
    /// TypeRef blocked: System.Net.Sockets.TcpClient
    /// </summary>
    private void DemonstrateTcpClient()
    {
        Console.WriteLine("[Violation 3] TcpClient");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Net.Sockets.TcpClient");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     using var client = new TcpClient(\"example.com\", 8080);");
        Console.WriteLine("     var stream = client.GetStream();");
        Console.WriteLine("     stream.Read(buffer, 0, buffer.Length);");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // Use NetworkExecutor for HTTP/HTTPS endpoints");
        Console.WriteLine("     // For raw TCP, request Capability.Network with justification");
        Console.WriteLine("     var result = NetworkExecutor.Execute(new ExecutorRequest");
        Console.WriteLine("     {");
        Console.WriteLine("         ResourcePath = \"tcp://example.com:8080\"");
        Console.WriteLine("     });");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: UdpClient
    /// TypeRef blocked: System.Net.Sockets.UdpClient
    /// </summary>
    private void DemonstrateUdpClient()
    {
        Console.WriteLine("[Violation 4] UdpClient");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Net.Sockets.UdpClient");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     using var udp = new UdpClient();");
        Console.WriteLine("     udp.Send(data, data.Length, \"example.com\", 9000);");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // UDP is not supported through NetworkExecutor");
        Console.WriteLine("     // If UDP access is required, declare Capability.Network");
        Console.WriteLine("     // and implement a custom network handler with permission checks");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Socket
    /// TypeRef blocked: System.Net.Sockets.Socket
    /// </summary>
    private void DemonstrateSocket()
    {
        Console.WriteLine("[Violation 5] Socket");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Net.Sockets.Socket");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);");
        Console.WriteLine("     socket.Connect(\"example.com\", 80);");
        Console.WriteLine("     socket.Send(buffer);");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // Use NetworkExecutor for standard protocols");
        Console.WriteLine("     // For custom socket usage, declare Capability.Network");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Dns
    /// TypeRef blocked: System.Net.Dns
    /// </summary>
    private void DemonstrateDns()
    {
        Console.WriteLine("[Violation 6] Dns");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Net.Dns");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     var hostEntry = Dns.GetHostEntry(\"example.com\");");
        Console.WriteLine("     var ip = hostEntry.AddressList[0];");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // DNS queries are handled internally by NetworkExecutor");
        Console.WriteLine("     // Use NetworkExecutor.Execute with the target URL");
        Console.WriteLine("     // NetworkExecutor will handle DNS resolution securely");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: SmtpClient
    /// TypeRef blocked: System.Net.Mail.SmtpClient
    /// </summary>
    private void DemonstrateSmtpClient()
    {
        Console.WriteLine("[Violation 7] SmtpClient");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Net.Mail.SmtpClient");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     using var smtp = new SmtpClient(\"smtp.example.com\", 587);");
        Console.WriteLine("     smtp.Send(mailMessage);");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // Email should be sent through a dedicated email service API");
        Console.WriteLine("     // Declare Capability.Network and use an email API (e.g., SendGrid, Mailgun)");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: WebClient
    /// TypeRef blocked: System.Net.WebClient
    /// </summary>
    private void DemonstrateWebClient()
    {
        Console.WriteLine("[Violation 8] WebClient");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Net.WebClient");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     using var client = new WebClient();");
        Console.WriteLine("     var data = client.DownloadString(\"https://api.example.com\");");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     var result = NetworkExecutor.Execute(new ExecutorRequest");
        Console.WriteLine("     {");
        Console.WriteLine("         ResourcePath = \"https://api.example.com\"");
        Console.WriteLine("     });");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: ClientWebSocket
    /// TypeRef blocked: System.Net.WebSockets.ClientWebSocket
    /// </summary>
    private void DemonstrateClientWebSocket()
    {
        Console.WriteLine("[Violation 9] ClientWebSocket");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Net.WebSockets.ClientWebSocket");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     using var ws = new ClientWebSocket();");
        Console.WriteLine("     await ws.ConnectAsync(uri, CancellationToken.None);");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // WebSocket support depends on NetworkExecutor implementation");
        Console.WriteLine("     // If WebSocket is required, declare Capability.Network");
        Console.WriteLine("     // and use a WebSocket-compatible API endpoint");
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates the CORRECT way to perform network operations in plugins.
/// </summary>
    private void DemonstrateCorrectApproach()
    {
        Console.WriteLine("[Correct Approach] Using NetworkExecutor");
        Console.WriteLine("  ✅ This is the SAFE way to perform network operations:");
        Console.WriteLine();
        Console.WriteLine("     // NetworkExecutor provides:");
        Console.WriteLine("     // 1. Permission checking (workspace and ACL restrictions)");
        Console.WriteLine("     // 2. Audit logging (all access is recorded)");
        Console.WriteLine("     // 3. Circuit breaker (prevents cascade failures)");
        Console.WriteLine("     // 4. Timeout control (default 30 seconds)");
        Console.WriteLine("     // 5. Request queuing (prevents resource exhaustion)");
        Console.WriteLine();
        Console.WriteLine("     // GET request");
        Console.WriteLine("     var getResult = NetworkExecutor.Execute(new ExecutorRequest");
        Console.WriteLine("     {");
        Console.WriteLine("         ResourcePath = \"https://api.example.com/data\",");
        Console.WriteLine("         Parameters = new Dictionary<string, object>");
        Console.WriteLine("         {");
        Console.WriteLine("             { \"method\", \"GET\" }");
        Console.WriteLine("         }");
        Console.WriteLine("     });");
        Console.WriteLine();
        Console.WriteLine("     // POST request with body and headers");
        Console.WriteLine("     var postResult = NetworkExecutor.Execute(new ExecutorRequest");
        Console.WriteLine("     {");
        Console.WriteLine("         ResourcePath = \"https://api.example.com/submit\",");
        Console.WriteLine("         Parameters = new Dictionary<string, object>");
        Console.WriteLine("         {");
        Console.WriteLine("             { \"method\", \"POST\" },");
        Console.WriteLine("             { \"body\", \"{\\\"key\\\": \\\"value\\\"}\" },");
        Console.WriteLine("             { \"headers\", new Dictionary<string, string> { { \"Content-Type\", \"application/json\" } } }");
        Console.WriteLine("         }");
        Console.WriteLine("     });");
        Console.WriteLine();
        Console.WriteLine("  📚 If you need unrestricted network access, declare Capability.Network:");
        Console.WriteLine("     [PluginCapability(Capability.Network, Reason = \"API endpoint access\")]");
        Console.WriteLine("     public class MyNetworkPlugin : IPlugin { ... }");
        Console.WriteLine();
        Console.WriteLine("  📚 See 13-CapabilityNetwork for a complete example of declared network access.");
        Console.WriteLine();
    }

    public void OnStop()
    {
        Console.WriteLine("\n[ForbiddenNetwork] Plugin stopped. No actual network operations were performed.");
    }

    public void OnUnload()
    {
    }
}