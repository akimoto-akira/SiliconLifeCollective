# PluginDemo-16: Capability.AI — Deklarative AI-Dienst-Berechtigung

## Übersicht

Dieses Plugin demonstriert die Verwendung von `[PluginCapability(Capability.AI)]` zur Deklaration, dass ein Plugin Zugriff auf den AI-Dienst benötigt. Im Gegensatz zu anderen Capabilities exempt `Capability.AI` **keinen** verbotenen Namespace — stattdessen ermöglicht es dem Host, eine `IAIService`-Referenz in das Plugin zu injizieren.

## Schlüsselkonzept: Capability.AI gewährt keinen Netzwerkzugriff

`Capability.AI` unterscheidet sich grundlegend von den anderen Capabilities:

| Capability | Was sie exempt | Wie sie funktioniert |
|-----------|--------------|---------------------|
| `Capability.Network` | `System.Net.*` Namespaces | Lockert TypeRef/ILString-Scan-Regeln |
| `Capability.FileIO` | `System.IO` Namespace | Lockert TypeRef/ILString-Scan-Regeln |
| `Capability.Process` | `Process*`-Typen | Lockert TypeRef/ILString-Scan-Regeln |
| `Capability.AI` | **Nichts** | Ermöglicht IAIService-Injektion durch Host |

`IAIService` befindet sich im `SiliconLife.Collective`-Namespace — er steht nie auf einer Verbotsliste. Die Capability-Deklaration ist ein **Opt-in-Signal** an den Host, dass dieses Plugin die AI-Dienst-Referenz erhalten soll.

## Capability-Stacking: AI + Network

Wenn Ihr AI-Client direkten Netzwerkzugriff benötigt (z. B. Aufruf eines Remote-AI-Endpunkts), müssen Sie **beide** Capabilities deklarieren:

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

Siehe **17-CapabilityStacked** für vollständige Stacking-Beispiele.

## Kontrolliertes Einstiegspunkt-Muster

| Ressource | Kontrollierter Einstiegspunkt | Capability nötig |
|----------|------------------------------|-----------------|
| Dateien | `PermissionedStreamFactory` | Keine |
| Netzwerk | `NetworkExecutor` | Keine |
| Prozesse | `CommandLineExecutor` | Keine |
| Datenspeicher | `SpeedyPack` | Keine |
| AI-Dienst | `IAIService` | `Capability.AI` |

`IAIService` ist einzigartig: Es **erfordert** eine Capability-Deklaration. Der AI-Dienstzugang ist ein Opt-in-Feature, keine Standardfähigkeit für alle Plugins.

## Dateien

- `Plugin.cs` — Demo-Plugin mit Capability.AI-Deklaration
- `README.md` — Diese Datei (Englisch)
- `README.zh-CN.md` — Vereinfachtes Chinesisch
- Übersetzungen: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Verwandte Beispiele

- **17-CapabilityStacked**: Mehrere Capability-Stacks (Network + AI)
- **18-CapabilityDenied**: Antimuster für nicht deklarierbare Fähigkeiten
