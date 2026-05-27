# PluginDemo-18: Capability verweigert — Nicht deklarierbare Capability Anti-Pattern

## Übersicht

Dieses Plugin ist ein **Anti-Pattern**, das demonstriert, dass die Deklaration einer Capability die Verbote nicht deklarierbarer Capabilities NICHT umgeht. Selbst mit `[PluginCapability(Capability.Network)]` bleiben P/Invoke, Unsafe, Reflection.Emit und Registry-Zugriff **immer** blockiert.

## Deklarierbare vs. nicht deklarierbare Capabilities

### ✅ Deklarierbar (Capability-Enum-Werte existieren)

| Capability | Was sie exempt |
|-----------|--------------|
| `Capability.Network` | System.Net.* Namespaces und typbezogene Verbote |
| `Capability.FileIO` | System.IO Namespace (über Whitelist hinaus) |
| `Capability.Process` | Process*-Typen unter System.Diagnostics |
| `Capability.AI` | Ermöglicht IAIService-Injektion (kein TypeRef-Exempt) |

### ❌ Nicht deklarierbar (KEIN Capability-Enum-Wert existiert)

| Kategorie | Blockierte Typen | Warum nicht deklarierbar |
|----------|-----------------|------------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory`, `NativeLibrary` | Arbiträrer nativer Code zur Laufzeit nicht auditierbar |
| Unsafe-Code | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Umgeht CLR-Typsicherheit und Grenzprüfung |
| IL-Emission | `System.Reflection.Emit.*` | Kann zur Laufzeit arbitrisches IL generieren |
| Assembly-Laden | `System.Runtime.Loader`, `Assembly.Load*` | Kann ungeskannte DLLs laden, Sicherheitsscan umgehen |
| Registrierung | `Microsoft.Win32.*` | OS-Level-Systemzugriff außerhalb des Plugin-Sandboxes |
| Dynamische Kompilierung | `Microsoft.CodeAnalysis.*` | Kann arbitrischen Code kompilieren und ausführen |
| Gefährliche Reflexion | `Type.GetType(string)`, `Activator.CreateInstance` | Kann verbotene Typen per String instanziieren |

## Warum diese Capabilities nicht deklarierbar sind

Der fundamentale Grund: **Sie können zur Laufzeit nicht sicher auditiert werden.**

1. **P/Invoke**: Sobald nativer Code aufgerufen wird, hat die CLR keine Sichtbarkeit — keine Sicherheitsgarantien
2. **Unsafe**: Umgeht das Typsicherheitssystem, auf dem das Plugin-Sicherheitsmodell beruht
3. **Reflection.Emit**: Kann zur Laufzeit neues IL generieren, das nie von PluginLoader gescannt wurde
4. **AssemblyLoadContext**: Kann DLLs laden, die nie sicherheitlich gescannt wurden
5. **Registry**: Bietet Zugriff auf OS-Level-Konfiguration außerhalb des Plugin-Sandboxes

## PluginLoaders „Deklaration ungültig"-Verarbeitung

Wenn PluginLoader eine Capability-Deklaration erkennt:

1. Liest den int32-Enum-Wert aus dem CustomAttribute-Blob
2. Prüft `Enum.IsDefined(typeof(Capability), value)`
3. Wenn der Wert kein definiertes Capability-Mitglied ist → **stillschweigend ignoriert**
4. Wenn der Wert definiert ist → Befreiungsregeln werden angewendet
5. **Nicht deklarierbare Prüfungen werden IMMER erzwungen** unabhängig von deklarierten Capabilities

Dies verhindert, dass Plugins „zukünftige" Capabilities deklarieren, die noch nicht existieren.

## Vergleich mit 13-CapabilityNetwork

| Aspekt | 13-CapabilityNetwork (positiv) | 18-CapabilityDenied (Anti-Pattern) |
|--------|-------------------------------|-----------------------------------|
| Deklaration | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| HttpClient nutzen | ✅ Exempt | ✅ Exempt |
| DllImport nutzen | N/A | ❌ IMMER blockiert |
| Unsafe nutzen | N/A | ❌ IMMER blockiert |
| Ladeergebnis | ✅ GELADEN | ❌ ABGELEHNT |

## Dateien

- `Plugin.cs` — Demo-Plugin als Anti-Pattern für nicht deklarierbare Capabilities
- `README.md` — Diese Datei (Englisch)
- `README.zh-CN.md` — Vereinfachtes Chinesisch
- Übersetzungen: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Verwandte Beispiele

- **13-CapabilityNetwork**: Positives Beispiel für Capability.Network
- **11-ForbiddenPInvoke**: P/Invoke-Antimuster (keine Capability hilft)
- **10-ForbiddenReflection**: Reflection-Antimuster (keine Capability hilft)
