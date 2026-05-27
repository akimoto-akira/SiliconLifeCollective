# PluginDemo-08: Verbotene Netzwerkoperationen Anti-Pattern

## Übersicht

Dieses Plugin demonstriert **VERBOTENE** Netzwerkoperationen im SiliconLife-Plugin-System. Es dient als Anti-Pattern-Referenz und zeigt, was NICHT getan werden sollte, mit korrekten Alternativen für jeden Verstoß.

## Warum ist direkter Netzwerkzugriff global verboten?

Direkte Netzwerkzugriffsmuster sind auf Plugin-Ebene blockiert:

1. **Verbindung zu bösartigen Servern**: Plugins könnten sich mit bösartigen Servern verbinden
2. **Datenexfiltration**: Plugins könnten sensible Daten aus der Sandbox leaken
3. **DNS-Rebinding-Angriffe**: Plugins könnten Sicherheitsprüfungen umgehen
4. **Netzwerk-ACL-Umgehung**: Direkter Netzwerkzugriff umgeht das globale ACL-System

## Verbotene Typen

Alle `System.Net`-Typen, die direkt auf das Netzwerk zugreifen, sind blockiert:

| Verbotener Typ | Blockierter Namespace | Risikostufe |
|----------|----------------|----------|
| `HttpClient` | `System.Net.Http` | 🔴 Kritisch |
| `TcpClient` | `System.Net.Sockets` | 🔴 Kritisch |
| `Socket` | `System.Net.Sockets` | 🔴 Kritisch |
| `Dns` | `System.Net` | 🔴 Kritisch |
| `WebClient` | `System.Net` | 🔴 Kritisch |

## Sichere Zugriffsmethoden

### NetworkExecutor (Empfohlen)

`NetworkExecutor` ist der **kontrollierte Einstiegspunkt** für Netzwerkoperationen:

```csharp
// ✅ KORREKT: Einfache GET-Anfrage
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

**Was NetworkExecutor bietet:**
1. Berechtigungsprüfung
2. Audit-Protokollierung
3. Leistungsschalter
4. Timeout-Kontrolle
5. Anforderungswarteschlange

## Demonstrerte Verstöße

### Verstoß 1: HttpClient

```csharp
// ❌ VERBOTEN
using var client = new HttpClient();

// ✅ KORREKT
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

### Verstoß 2: TcpClient

```csharp
// ❌ VERBOTEN
using var client = new TcpClient("example.com", 8080);

// ✅ KORREKT
// NetworkExecutor verwenden oder Capability.Network deklarieren
```

## PluginLoader-Sicherheitsmechanismus

PluginLoader scannt dieses Plugin und:
1. **TypeRef-Scanning**: Erkennt Verweise auf verbotene Typen
2. **MemberRef-Scanning**: Erkennt Aufrufe blockierter Methoden
3. **IL-String-Scanning**: Erkennt Reflexionsversuche
4. **Ablehnung**: Plugin wird beim Laden abgelehnt

## Dateien

- `Plugin.cs` - Anti-Pattern-Demonstrationsplugin
- `README.md` - Diese Datei (Englisch)
- `README.de-DE.md` - Diese Datei (Deutsch)
- Weitere Sprachversionen...

## Verwandte Beispiele

- **13-CapabilityNetwork**: Deklarative Netzwerk-Berechtigung
- **07-ForbiddenFileIO**: Verbotene Dateizugriffsmuster