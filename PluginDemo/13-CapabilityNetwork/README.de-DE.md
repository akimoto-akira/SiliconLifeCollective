# PluginDemo-13: Capability.Network — Deklarative Netzwerkberechtigung

## Übersicht

Dieses Plugin demonstriert die Verwendung von `[PluginCapability(Capability.Network)]` zur Deklaration des Netzwerkzugriffs. Mit dieser Deklaration erhält das Plugin Zugriff auf `System.Net.*`-Typen, die sonst vom PluginLoader-Sicherheitsscan blockiert würden.

## Deklarationssyntax

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**Schlüsselelemente:**
- **Attributziel**: Muss auf der Klasse stehen, die `IPlugin` direkt implementiert
- **AllowMultiple = true**: Mehrere `[PluginCapability]`-Attribute können gestapelt werden (siehe 17-CapabilityStacked)
- **Reason-Feld**: Menschenlesbare Erklärung, die beim Laden in das Sicherheitsaudit-Log geschrieben wird. **Für alle Produktions-Plugins wird dringend empfohlen, einen klaren Reason anzugeben.**

## Wie PluginLoader Capability-Deklarationen verarbeitet

1. **PE-Metadaten-Lesevorgang**: PluginLoader liest Capability-Deklarationen aus der CustomAttribute-Tabelle der PE-Datei **bevor** der Sicherheitsscan beginnt
2. **Scanregeln lockern**: Deklarierte Capabilities befreien entsprechende Typreferenzen von Namespace- und Typ-Verbotsprüfungen
3. **Audit-Logging**: Alle Deklarationen (einschließlich Reason) werden in das Sicherheitsaudit-Log geschrieben
4. **Nicht deklarierbare Capabilities**: P/Invoke, Unsafe, Reflection.Emit usw. bleiben unabhängig von jeder Deklaration blockiert

## Capability.Network-Befreiungsbereich

### TypeRef-Befreiungen

Wenn `Capability.Network` deklariert wird, werden die folgenden namespace- und typbasierten Verbotsregeln gelockert:

| Befreiter Namespace | Erlaubte Typen |
|-------------------|---------------|
| `System.Net.Http` | `HttpClient`, `HttpRequestMessage`, `HttpResponseMessage` usw. |
| `System.Net.WebSockets` | `ClientWebSocket`, `WebSocket` usw. |
| `System.Net.Sockets` | `TcpClient`, `UdpClient`, `Socket` usw. |
| `System.Net.Mail` | `SmtpClient`, `MailMessage` usw. |
| `System.Net.NetworkInformation` | `Ping`, `NetworkInterface` usw. |
| `System.Net.Security` | `SslStream` usw. |
| `System.Net` (typspezifische Verbote) | `HttpWebRequest`, `WebClient`, `Dns`, `FtpWebRequest` usw. |

### ILString-Befreiungen

Zeichenfolgenkonstanten, die mit diesen Präfixen beginnen, werden beim #US-Heap-Scan nicht markiert:
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### Was weiterhin verboten bleibt

Selbst mit `Capability.Network` bleiben diese **immer** blockiert (nicht deklarierbare Capabilities):

| Kategorie | Blockierte Typen | Warum nicht deklarierbar |
|----------|-----------------|------------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` | Kann zur Laufzeit nicht sicher geprüft werden |
| Unsafe-Code | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Umgeht Typsicherheitsgarantien |
| IL-Emission | `System.Reflection.Emit.*` | Kann beliebigen Code zur Laufzeit generieren |
| Assembly-Laden | `System.Runtime.Loader`, `Assembly.Load*` | Kann ungeprüfte DLLs laden und Sicherheitsscan umgehen |
| Registry | `Microsoft.Win32.*` | Betriebssystemzugriff außerhalb der Plugin-Sandbox |

## Reason-Feld — Audit-Rolle

Das `Reason`-Feld dient als **Audit-Trail** für Capability-Deklarationen:

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Warum Reason wichtig ist:**
1. **Sicherheitsüberprüfung**: Prüfer können verifizieren, dass deklarierte Capabilities mit dem tatsächlichen Plugin-Verhalten übereinstimmen
2. **Prinzip der geringsten Rechte**: Zwingt Plugin-Autoren, die Notwendigkeit jeder Capability zu rechtfertigen
3. **Compliance**: Erforderlich für Sicherheitszertifizierungen und Vorfalluntersuchungen
4. **Laufzeitüberwachung**: Sicherheitstools können warnen, wenn die Nutzung der deklarierten Capability den angegebenen Grund übersteigt

## Vergleich mit 08-ForbiddenNetwork

| Aspekt | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|--------|-------------------|---------------------|
| Deklaration | Keine | `[PluginCapability(Capability.Network)]` |
| Ladeergebnis | ❌ Abgelehnt | ✅ Erfolgreich geladen |
| HttpClient-Nutzung | Durch TypeRef-Scan blockiert | Durch Capability befreit |
| TcpClient-Nutzung | Durch TypeRef-Scan blockiert | Durch Capability befreit |
| Reason | Nicht zutreffend | In Audit-Log geschrieben |

**Wichtiger Unterschied**: 08-ForbiddenNetwork zeigt, was passiert, wenn man Netzwerktypen **ohne** Capability-Deklaration verwendet. 13-CapabilityNetwork zeigt den **korrekten** Weg, Netzwerkzugriff deklarativ anzufordern.

## Sicherheitsbest Practices

1. **Nur Nötiges deklarieren**: Wenn Sie nur HTTP benötigen, deklarieren Sie nicht Capability.Network nur weil es möglich ist — beachten Sie jedoch, dass Capability.Network die einzige netzwerkbezogene Capability ist; es gibt keine feingranulareren Optionen
2. **NetworkExecutor bevorzugen**: `NetworkExecutor` ist der kontrollierte Einstiegspunkt für Netzwerkzugriff und erfordert keine Capability-Deklaration
3. **Klaren Reason angeben**: Vage Gründe wie "Netzwerkzugriff" sind bei Sicherheitsreviews ein Warnsignal
4. **Nicht deklarierbare Limits beachten**: Keine Capability-Deklaration kann P/Invoke-, Unsafe- oder Reflection.Emit-Verbote umgehen

## Dateien

- `Plugin.cs` — Demo-Plugin mit Capability.Network-Deklaration
- `README.md` — English
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 日本語
- `README.ko-KR.md` — 한국어
- `README.de-DE.md` — Diese Datei (Deutsch)
- `README.fr-FR.md` — Français
- `README.es-ES.md` — Español
- `README.it-IT.md` — Italiano
- `README.ru-RU.md` — Русский
- `README.pt-PT.md` — Português
- `README.pl-PL.md` — Polski
- `README.cs-CZ.md` — Čeština

## Verwandte Beispiele

- **08-ForbiddenNetwork**: Anti-Pattern mit blockierten Netzwerkoperationen
- **14-CapabilityFileIO**: Deklarative FileIO-Capability
- **15-CapabilityProcess**: Deklarative Process-Capability
- **16-CapabilityAI**: Deklarative AI-Dienst-Capability
- **17-CapabilityStacked**: Mehrere Capability-Stapeln
- **18-CapabilityDenied**: Nicht deklarierbare Capability Anti-Pattern
