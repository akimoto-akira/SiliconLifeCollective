# Minimales Plugin-Demo

Eine minimale `IPlugin`-Implementierung, die den Plugin-Lebenszyklus mit hartcodierten Werten demonstriert.

## IPlugin-Schnittstelle im Überblick

Jedes SiliconLife-Plugin muss die in `SiliconLife.Collective` definierte `IPlugin`-Schnittstelle implementieren：

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Eigenschaftsübersicht

| Mitglied | Typ | Beschreibung |
|----------|-----|------|
| `Id` | `string` | Eindeutiger Bezeichner, muss versionsübergreifend stabil sein (z.B. `"com.siliconlife.demo.minimal"`) |
| `GetName(Language)` | `string` | Menschenlesbarer Anzeigename, lokalisiert durch `Language`-Enum |
| `Version` | `string` | Semantische Versionszeichenkette (z.B. `"1.0.0"`) |
| `GetDescription(Language)` | `string` | Kurzbeschreibung der Plugin-Funktionalität |
| `GetAuthor(Language)` | `string` | Autor- oder Organisationsname |

## Lebenszyklus-Aufrufreihenfolge

Der Host ruft Lebenszyklusmethoden in einer strengen Reihenfolge auf：

```
OnLoad → OnStart → [Laufend] → OnStop → OnUnload
```

| Methode | Wann aufgerufen | Typische Verwendung |
|---------|----------------|---------------------|
| `OnLoad()` | Einmal, wenn die Plugin-DLL in den Host geladen wird | Konfiguration validieren, Typen registrieren, Ressourcen vorbereiten |
| `OnStart()` | Wenn der Host vollständig gestartet und alle Plugins geladen sind | Mit anderen Plugins interagieren, Hintergrundaufgaben starten |
| `OnStop()` | Wenn der Host ordnungsgemäß herunterfährt | Ressourcen freigeben, Puffer leeren, Zustand speichern |
| `OnUnload()` | Wenn das Plugin aus dem Host entladen wird | Endgültige Bereinigung |

## Dieses Demo

Dieses Plugin gibt für alle Eigenschaften hartcodierte Werte zurück und lässt Lebenszyklusmethoden leer. Es ist der einfachste Ausgangspunkt für die Plugin-Entwicklung.

## Sicherheitshinweis

Plugins werden in einem isolierten `AssemblyLoadContext` geladen und auf verbotene Namespace-Referenzen (z.B. `System.IO`, `System.Net.Http`) gescannt. Details siehe[Sicherheitsdokumentation](../../docs/de-DE/security.md).
