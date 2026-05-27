# Demo: Vertrauenswürdige Abhängigkeit

Demonstriert die Verwendung von `Newtonsoft.Json` — einer Bibliothek, die intern intensiv Reflection nutzt — als vertrauenswürdige Assembly. Der PluginLoader-Sicherheitsscanner überspringt vertrauenswürdige Assemblies vollständig und erlaubt Plugins, sie ohne Auslösung von Verstößen zu referenzieren.

## TrustedAssemblies-Whitelist-Mechanismus

Der `PluginLoader` pflegt eine statische Whitelist von Open-Source-Bibliotheken, die **standardmäßig vertrauenswürdig** sind:

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // Serialisierung
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← Dieses Demo verwendet diese Bibliothek
    "MessagePack",
    "YamlDotNet",

    // Protokollierung
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // Datenzugriff / Mapping
    "Dapper", "AutoMapper",

    // Validierung und Nachrichtenverteilung
    "FluentValidation", "MediatR",
};
```

### Aufnahmekriterien

Eine Bibliothek kann zu `TrustedAssemblies` hinzugefügt werden, wenn sie **alle drei** Kriterien erfüllt:

| # | Kriterium | Begründung |
|---|-----------|-----------|
| 1 | Weit verbreitetes Open-Source-Projekt (MIT / Apache 2.0 / BSD) | Öffentlich auditierbarer Code |
| 2 | Öffentlich einsehbarer Quellcode | Community-Aufsicht stellt sicher, dass kein bösartiges Verhalten vorliegt |
| 3 | Gepflegtes NuGet-Paket von vertrauenswürdigem Anbieter/Community | Integrität der Lieferkette |

### Identifikationsgrundlage

Der Scanner identifiziert vertrauenswürdige Assemblies anhand ihres `AssemblyDefinition.Name` in PE-Metadaten — **nicht anhand des DLL-Dateinamens**. Dies verhindert, dass Angreifer eine bösartige DLL in `Newtonsoft.Json.dll` umbenennen, um Prüfungen zu umgehen.

## CollectTrustedTypeRefs — Transitive Ausnahme

Wenn der PluginLoader ein Plugin-Verzeichnis lädt, führt er ein zweiphasiges Scanning durch:

```
Phase 1: CollectTrustedTypeRefs(pluginDir)
├── Aufzählung aller *.dll-Dateien im Plugin-Verzeichnis
├── Für jede DLL: PE-Metadaten lesen → AssemblyDefinition.Name prüfen
├── Falls Name ∈ TrustedAssemblies:
│   └── ALLE TypeReference-Einträge sammeln → (Namespace, TypeName)-Paare
└── Rückgabe: HashSet<(string Namespace, string Name)>

Phase 2: ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── Schicht 0:   Whitelist-Schnellausgang (falls Haupt-DLL vertrauenswürdig → bestanden)
├── Schicht 0.5: Transitive Ausnahme (TypeRefs in trustedTypeRefs-Set überspringen)
├── Schicht 1:   TypeRef-Tabellen-Scan
├── Schicht 2:   ExportedType-Tabellen-Scan
├── Schicht 3:   MemberRef-Tabellen-Scan (gefährliche Methoden)
├── Schicht 4:   Unsichere Code-Marker + P/Invoke
└── Schicht 5:   #US-String-Heap-Scan
```

### Warum die transitive Ausnahme wichtig ist

Newtonsoft.Json referenziert intern Typen wie `System.Reflection.MemberInfo`, `System.IO.TextReader` usw. Wenn Ihr Plugin Newtonsoft.Json referenziert, kann der Compiler diese transitiven TypeRefs in **Ihre** Plugin-DLL einbetten. Ohne transitive Ausnahme würde Ihr Plugin wegen der Referenzierung von `System.IO.TextReader` als Verstoß markiert — obwohl Sie es nie direkt verwenden.

`CollectTrustedTypeRefs` löst dies, indem alle TypeRefs aus vertrauenswürdigen DLLs vorab gesammelt und während des Hauptscans als „bekannt sicher" markiert werden.

## Wie man eine neue vertrauenswürdige Abhängigkeit hinzufügt

Um eine neue Bibliothek zur Whitelist hinzuzufügen:

1. Überprüfen Sie, ob sie die drei oben genannten Aufnahmekriterien erfüllt
2. Fügen Sie eine Zeile zum `TrustedAssemblies`-HashSet in `PluginLoader.cs` hinzu:
   ```csharp
   "YourLibraryName",  // Kurze Beschreibung warum vertrauenswürdig
   ```
3. Platzieren Sie die Bibliotheks-DLL im Plugin-Verzeichnis (neben der Haupt-DLL des Plugins)
4. Der Scanner sammelt automatisch deren TypeRefs und gewährt Ausnahmen

> **⚠️ Wichtig:** Das Hinzufügen einer Bibliothek zu `TrustedAssemblies` bedeutet, dass der Scanner ihren internen Code **nicht** prüft. Fügen Sie nur Bibliotheken hinzu, denen Sie vollständig vertrauen.

## Dieses Demo

Dieses Plugin verwendet Newtonsoft.Json ohne jegliche `PluginCapability`-Deklaration:

| Funktion | Internes Verhalten von Newtonsoft.Json | Warum es funktioniert |
|----------|----------------------------------------|----------------------|
| `JsonConvert.SerializeObject` | Verwendet Reflection zur Aufzählung von Eigenschaften | Newtonsoft.Json-DLL besteht Schicht-0-Whitelist |
| `JsonConvert.DeserializeObject<T>` | Ruft `Activator.CreateInstance` auf, setzt Eigenschaften via Reflection | Transitive TypeRefs in Schicht 0.5 ausgenommen |
| `JObject`/`JArray`-Manipulation | Verwendet `System.Linq.Expressions`, dynamischen Dispatch | Alle internen Refs von `CollectTrustedTypeRefs` gesammelt |

### Hauptunterschied zu PluginCapability

| Mechanismus | Umfang | Anwendungsfall |
|-------------|--------|----------------|
| `TrustedAssemblies` | Nimmt eine gesamte **Bibliothek** (und ihre transitiven Refs) vom Scanning aus | Bekannte Open-Source-Abhängigkeiten |
| `PluginCapability` | Nimmt Ihren **Plugin-Code** von bestimmten Namespace-Verboten aus | Plugin benötigt direkten Zugriff auf System.Net/IO/Process |

Ein Plugin, das nur vertrauenswürdige Abhängigkeiten verwendet, benötigt **keine** `PluginCapability`-Deklaration. Der Scanner erledigt alles automatisch.

## Sicherheitshinweis

Vertrauenswürdige Assemblies sind vom Sicherheitsscan ausgenommen, da sie auditierbare Open-Source-Projekte sind. Ihr **Plugin-Code** wird jedoch weiterhin vollständig gescannt. Wenn Ihr Plugin direkt auf `System.IO.File` oder `System.Net.Http.HttpClient` verweist, wird es weiterhin blockiert — es sei denn, Sie deklarieren die entsprechende `PluginCapability`. Siehe [Sicherheitsdokumentation](../../docs/de-DE/security.md).
