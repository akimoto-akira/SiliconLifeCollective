# PluginDemo-17: Capability-Stacking — Mehrere deklarative Berechtigungen

## Übersicht

Dieses Plugin demonstriert das Stapeln mehrerer `[PluginCapability]`-Attribute auf einer einzelnen Plugin-Klasse. `PluginCapabilityAttribute` hat `AllowMultiple = true`, sodass Sie so viele Capabilities deklarieren können wie nötig.

## Stacking-Syntax

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## Wie PluginLoader gestackte Capabilities verarbeitet

1. Liest **alle** Deklarationen aus der PE-Metadaten CustomAttribute-Tabelle
2. **Führt zusammen**: Befreiungsregeln aller deklarierten Capabilities
3. **Protokolliert unabhängig**: Jede Deklaration mit eigenem Reason-Feld
4. **Erzwingt weiterhin**: Verbote nicht deklarierbarer Capabilities unabhängig vom Stacking

## Zusammengeführte Befreiungsregeln

Beim Stacking von `Capability.Network` + `Capability.AI`:

| Quelle | Befreiung |
|--------|----------|
| Capability.Network | System.Net.Http.*, System.Net.WebSockets.*, System.Net.Sockets.*, System.Net.Mail.*, System.Net.NetworkInformation.*, System.Net.Security.*, System.Net (typbezogene Verbote) |
| Capability.AI | IAIService-Injektion aktiviert |
| **Kombiniert** | Plugin kann HttpClient UND IAIService verwenden |

## Stacking gewährt keine unbegrenzte Macht

Auch mit mehreren gestackten Capabilities bleiben diese **immer blockiert**:

- ❌ P/Invoke (`DllImport`, `Marshal`, `NativeMemory`)
- ❌ Unsicherer Code (`UnverifiableCodeAttribute`, `Unsafe`)
- ❌ IL-Emission (`System.Reflection.Emit.*`)
- ❌ Assembly-Laden (`System.Runtime.Loader`, `Assembly.Load*`)
- ❌ Registrierung (`Microsoft.Win32.*`)

Für diese existiert kein `Capability`-Enum-Wert — sie sind **konstruktionsbedingt nicht deklarierbar**.

## Audit-Trail für gestackte Capabilities

Jede Capability wird unabhängig protokolliert:

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## Dateien

- `Plugin.cs` — Demo-Plugin mit gestackten Capability.Network + Capability.AI
- `README.md` — Diese Datei (Englisch)
- `README.zh-CN.md` — Vereinfachtes Chinesisch
- Übersetzungen: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Verwandte Beispiele

- **13-CapabilityNetwork**: Einzelne Network-Capability
- **16-CapabilityAI**: Einzelne AI-Capability
- **18-CapabilityDenied**: Antimuster für nicht deklarierbare Fähigkeiten
