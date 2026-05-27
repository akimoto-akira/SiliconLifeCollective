# PluginDemo-10: Verbotene Reflection-Operationen (Anti-Pattern)

## Übersicht

Dieses Plugin demonstriert **verbotene** Reflection-Operationen im SiliconLife-Plugin-System. Es dient als Anti-Pattern-Referenz und zeigt, was NICHT getan werden sollte, sowie die korrekten Alternativen für jeden Verstoß.

## Warum ist Reflection die Kernbedrohung?

Reflection-Umgehung ist die **kritischste Bedrohung** für das Sicherheits-Scanning des PluginLoaders. Während TypeRef-Scanning direkte Typreferenzen zur Kompilierzeit erfasst, können Reflection-Methoden Typen zur **Laufzeit** mittels Zeichenketten auflösen — vollständig unsichtbar für statisches Metadaten-Scanning.

Wenn ein Plugin `Type.GetType("System.IO.File, System.Runtime")` aufrufen kann, kann es auf JEDEN verbotenen Typ zugreifen, ohne dass eine Referenz in der TypeRef-Tabelle der PE-Metadaten erscheint.

## Welche Methoden sind verboten?

Alle verbotenen Methoden werden über **MemberRef-Scanning** erkannt (keine Namespace- oder Typ-Blockierung):

| Verbotene Methode | Signatur | Bedrohung |
|-------------------|----------|-----------|
| `Type.GetType` | `System.Type::GetType(System.String)` | Beliebigen Typ zur Laufzeit per Name auflösen |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | Beliebige Typen instanziieren |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | Assembly nach Name/Bytes laden |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | Assembly von Festplatte laden |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | Assembly von Pfad laden |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | Zeichenkettenbasierte Typauflösung |

## Was ist sicher?

Nicht alle Reflection ist verboten. Folgende Muster sind **sicher**, da sie kompilierzeitbekannte Typen referenzieren:

| Sicheres Muster | Beispiel | Warum sicher |
|-----------------|----------|--------------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | Typ ist zur Kompilierzeit bekannt, sichtbar in TypeRef |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | Bekannten Typ inspizieren, keine neuen Typen |
| `typeof(X).GetMethods()` | `typeof(IPlugin).GetMethods()` | Member-Inspektion bekannter Typen |
| Generische Einschränkungen | `FindSubtypesOf(typeof(BaseTool))` | Generischer Parameter ist Kompilierzeit-Typ |
| `nameof()` | `nameof(MyClass.MyMethod)` | Kompilierzeit-Zeichenkette, keine Laufzeitauflösung |

**Wichtige Unterscheidung:**
- `typeof(X).Assembly` → **Sicher** (Kompilierzeit-Referenz, vom PluginLoader gescannt)
- `Assembly.Load("X")` → **Verboten** (Laufzeit-Zeichenkette, umgeht alle Scans)

## Wie kann Reflection sicher ersetzt werden?

### ITypeRegistry verwenden (Ersetzt Type.GetType + AppDomain-Scanning)

```csharp
// ❌ VERBOTEN: Typ zur Laufzeit per Zeichenkette auflösen
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ KORREKT: ITypeRegistry zur Suche registrierter Typen verwenden
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// Nur während OnLoad registrierte Typen sind auffindbar
```

### IObjectFactory verwenden (Ersetzt Activator.CreateInstance)

```csharp
// ❌ VERBOTEN: Beliebige Instanz erzeugen
object? instance = Activator.CreateInstance(someType);

// ✅ KORREKT: IObjectFactory mit registrierter Factory verwenden
var instance = objectFactory.CreateInstance<MyService>();
// Nur Typen mit registrierten Factories können instanziiert werden
```

## Demonstrierte Verstöße

### Verstoß 1: Type.GetType(string)

```csharp
// ❌ VERBOTEN
Type? fileType = Type.GetType("System.IO.File, System.Runtime");

// ✅ KORREKT
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**Blockierte MemberRef**: `System.Type::GetType(System.String)`

### Verstoß 2: Activator.CreateInstance

```csharp
// ❌ VERBOTEN
object? client = Activator.CreateInstance(httpClientType!);

// ✅ KORREKT
var instance = objectFactory.CreateInstance<MyService>();
```

**Blockierte MemberRef**: `System.Activator::CreateInstance`

### Verstoß 3: Assembly.Load

```csharp
// ❌ VERBOTEN
Assembly asm = Assembly.Load("System.Net.Http");

// ✅ KORREKT
Assembly myAsm = typeof(MyPlugin).Assembly;  // Sicher: kompilierzeitbekannt
```

**Blockierte MemberRef**: `System.Reflection.Assembly::Load(System.String)`

### Verstoß 4: Assembly.LoadFile / LoadFrom

```csharp
// ❌ VERBOTEN
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");

// ✅ KORREKT
// Alle Abhängigkeiten müssen im Plugin-Verzeichnis liegen und vom PluginLoader gescannt werden.
```

**Blockierte MemberRef**: `System.Reflection.Assembly::LoadFile(System.String)`

### Verstoß 5: Assembly.GetType(string)

```csharp
// ❌ VERBOTEN
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ KORREKT
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
```

**Blockierte MemberRef**: `System.Reflection.Assembly::GetType(System.String)`

## Warum typeof(X).Assembly sicher und Assembly.Load unsicher ist

| Operation | Sichtbarkeit | Sicherheit |
|-----------|-------------|------------|
| `typeof(X).Assembly` | Typ X in TypeRef-Tabelle → PluginLoader scannt ihn | ✅ Sicher |
| `Assembly.Load("X")` | Zeichenkette "X" nur zur Laufzeit → für TypeRef-Scan unsichtbar | ❌ Verboten |
| `obj.GetType()` | Gibt Typ der bestehenden Instanz zurück → kein neuer Typ | ✅ Sicher |
| `Type.GetType("X")` | Löst beliebigen Typ aus Zeichenkette auf → umgeht TypeRef | ❌ Verboten |

## Best Practices

1. **Typen in OnLoad registrieren**: `ITypeRegistry.RegisterType` / `RegisterFromAssembly` verwenden
2. **IObjectFactory für dynamische Erzeugung verwenden**: Niemals `Activator.CreateInstance` benutzen
3. **typeof(X).Assembly nutzen**: Sicherer Zugriff auf die eigene Assembly
4. **Zeichenkettenbasierte Typnamen vermeiden**: Löst IL-String-Scanning aus
5. **Für statische Auffindbarkeit entwerfen**: Nicht in Metadaten sichtbar = verdächtig

## Dateien

- `Plugin.cs` - Anti-Pattern-Demo-Plugin
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Diese Datei (Deutsch)
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Verwandte Beispiele

- **02-TypeRegistryUsage**: Korrekte ITypeRegistry-Verwendung
- **03-ObjectFactoryUsage**: Korrekte IObjectFactory-Verwendung
- **11-ForbiddenPInvoke**: Verbotene P/Invoke- und unsafe-Operationen
- **12-ForbiddenStringBypass**: Zeichenkettenbasierte Reflection-Umgehungsversuche
