# PluginDemo-10: Anti-pattern di reflection proibita

## Panoramica

Questo plugin dimostra le operazioni di reflection **proibite** nel sistema di plugin SiliconLife. Serve come riferimento anti-pattern, mostrando cosa NON fare e fornendo le alternative corrette per ogni violazione.

## Perché la reflection è la minaccia principale?

L'elusione tramite reflection è la **minaccia più critica** per la scansione di sicurezza del PluginLoader. Mentre la scansione TypeRef cattura i riferimenti ai tipi diretti al momento della compilazione, i metodi di reflection possono risolvere tipi a **runtime** usando stringhe — completamente invisibili alla scansione statica dei metadati.

Se un plugin può chiamare `Type.GetType("System.IO.File, System.Runtime")`, può accedere a QUALSIASI tipo proibito senza che alcun riferimento appaia nella tabella TypeRef dei metadati PE.

## Quali metodi sono proibiti?

Tutti i metodi proibiti vengono rilevati tramite **scansione MemberRef** (non blocco a livello di namespace o tipo):

| Metodo proibito | Firma | Minaccia |
|----------------|-------|----------|
| `Type.GetType` | `System.Type::GetType(System.String)` | Risolvere tipo arbitrario per nome a runtime |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | Istanziare tipi arbitrari |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | Caricare assembly per nome/bytes |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | Caricare assembly da disco |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | Caricare assembly da percorso |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | Risoluzione tipo basata su stringhe |

## Cosa è sicuro?

Non tutta la reflection è proibita. I seguenti pattern sono **sicuri** perché referenziano tipi noti al momento della compilazione:

| Pattern sicuro | Esempio | Perché è sicuro |
|---------------|---------|-----------------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | Tipo noto a compilazione, visibile in TypeRef |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | Ispezione di tipo noto, nessun nuovo tipo |
| Vincoli generici | `FindSubtypesOf(typeof(BaseTool))` | Parametro generico è tipo di compilazione |
| `nameof()` | `nameof(MyClass.MyMethod)` | Stringa di compilazione, nessuna risoluzione a runtime |

**Distinzione chiave:**
- `typeof(X).Assembly` → **Sicuro** (riferimento di compilazione, scansionato da PluginLoader)
- `Assembly.Load("X")` → **Proibito** (stringa di runtime, elude tutte le scansioni)

## Come sostituire la reflection in sicurezza?

### Usare ITypeRegistry (Sostituisce Type.GetType + scansione AppDomain)

```csharp
// ❌ PROIBITO: Risolvere tipo per stringa a runtime
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ CORRETTO: Usare ITypeRegistry per trovare tipi registrati
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// Solo i tipi registrati durante OnLoad sono scopribili
```

### Usare IObjectFactory (Sostituisce Activator.CreateInstance)

```csharp
// ❌ PROIBITO: Creare istanza arbitraria
object? instance = Activator.CreateInstance(someType);

// ✅ CORRETTO: Usare IObjectFactory con factory registrata
var instance = objectFactory.CreateInstance<MyService>();
// Solo i tipi con factory registrate possono essere istanziati
```

## Violazioni dimostrate

### Violazione 1: Type.GetType(string)

```csharp
// ❌ PROIBITO
Type? fileType = Type.GetType("System.IO.File, System.Runtime");

// ✅ CORRETTO
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**MemberRef bloccata**: `System.Type::GetType(System.String)`

### Violazione 2: Activator.CreateInstance

```csharp
// ❌ PROIBITO
object? client = Activator.CreateInstance(httpClientType!);

// ✅ CORRETTO
var instance = objectFactory.CreateInstance<MyService>();
```

**MemberRef bloccata**: `System.Activator::CreateInstance`

### Violazione 3: Assembly.Load

```csharp
// ❌ PROIBITO
Assembly asm = Assembly.Load("System.Net.Http");

// ✅ CORRETTO
Assembly myAsm = typeof(MyPlugin).Assembly;  // Sicuro: noto a compilazione
```

**MemberRef bloccata**: `System.Reflection.Assembly::Load(System.String)`

### Violazione 4: Assembly.LoadFile / LoadFrom

```csharp
// ❌ PROIBITO
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");

// ✅ CORRETTO
// Tutte le dipendenze devono essere nella directory del plugin e scansionate da PluginLoader.
```

**MemberRef bloccata**: `System.Reflection.Assembly::LoadFile(System.String)`

### Violazione 5: Assembly.GetType(string)

```csharp
// ❌ PROIBITO
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ CORRETTO
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
```

**MemberRef bloccata**: `System.Reflection.Assembly::GetType(System.String)`

## Perché typeof(X).Assembly è sicuro e Assembly.Load no

| Operazione | Visibilità | Sicurezza |
|-----------|-----------|-----------|
| `typeof(X).Assembly` | Tipo X nella tabella TypeRef → PluginLoader lo scansiona | ✅ Sicuro |
| `Assembly.Load("X")` | Stringa "X" esiste solo a runtime → invisibile alla scansione TypeRef | ❌ Proibito |
| `obj.GetType()` | Restituisce tipo di istanza esistente → nessun nuovo tipo | ✅ Sicuro |
| `Type.GetType("X")` | Risolve tipo arbitrario da stringa → elude TypeRef | ❌ Proibito |

## Best practice

1. **Registrare tipi in OnLoad**: Usare `ITypeRegistry.RegisterType` / `RegisterFromAssembly`
2. **Usare IObjectFactory per creazione dinamica**: Mai usare `Activator.CreateInstance`
3. **Usare typeof(X).Assembly**: Accesso sicuro al proprio assembly
4. **Evitare nomi di tipo basati su stringhe**: Attiva la scansione stringhe IL
5. **Progettare per scopribilità statica**: Non visibile nei metadati = sospetto

## File

- `Plugin.cs` - Plugin di dimostrazione anti-pattern
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Questo file (Italiano)
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Esempi correlati

- **02-TypeRegistryUsage**: Uso corretto di ITypeRegistry
- **03-ObjectFactoryUsage**: Uso corretto di IObjectFactory
- **11-ForbiddenPInvoke**: P/Invoke e codice unsafe proibiti
- **12-ForbiddenStringBypass**: Tentativi di elusione tramite stringhe di reflection
