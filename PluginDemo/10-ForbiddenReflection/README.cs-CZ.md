# PluginDemo-10: Anti-vzor zakázaných reflexních operací

## Přehled

Tento plugin demonstruje **zakázané** reflexní operace v systému pluginů SiliconLife. Slouží jako reference anti-vzoru, ukazuje co NEDĚLAT a poskytuje správné alternativy pro každé porušení.

## Proč je reflexe hlavní hrozbou?

Obcházení reflexí je **nejkritičtější hrozba** pro bezpečnostní skenování PluginLoaderu. Zatímco skenování TypeRef zachytí přímé reference typů v době kompilace, metody reflexe mohou rozřešit typy za **běhu** pomocí řetězců — zcela neviditelné pro statické skenování metadat.

Pokud plugin může volat `Type.GetType("System.IO.File, System.Runtime")`, může přistupovat k JAKÉMUKOLIV zakázanému typu, aniž by se jakákoliv reference objevila v tabulce TypeRef PE metadat.

## Které metody jsou zakázány?

Všechny zakázané metody jsou detekovány přes **skenování MemberRef** (nikoliv blokování na úrovni jmenného prostoru nebo typu):

| Zakázaná metoda | Signatura | Hrozba |
|----------------|-----------|--------|
| `Type.GetType` | `System.Type::GetType(System.String)` | Rozřešení libovolného typu podle jména za běhu |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | Instanciace libovolných typů |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | Načtení assembly podle jména/bajtů |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | Načtení assembly z disku |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | Načtení assembly z cesty |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | Rozřešení typu na základě řetězce |

## Co je bezpečné?

Ne všechna reflexe je zakázána. Následující vzory jsou **bezpečné**, protože referencují typy známé v době kompilace:

| Bezpečný vzor | Příklad | Proč bezpečné |
|--------------|---------|--------------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | Typ známý v kompilaci, viditelný v TypeRef |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | Inspekce známého typu, žádné nové typy |
| Generická omezení | `FindSubtypesOf(typeof(BaseTool))` | Generický parametr je typ kompilace |
| `nameof()` | `nameof(MyClass.MyMethod)` | Řetězec kompilace, žádné rozřešení za běhu |

**Klíčové rozlišení:**
- `typeof(X).Assembly` → **Bezpečné** (reference kompilace, skenované PluginLoaderem)
- `Assembly.Load("X")` → **Zakázané** (řetězec běhu, obchází všechna skenování)

## Jak bezpečně nahradit reflexi?

### Použití ITypeRegistry (Nahrazuje Type.GetType + skenování AppDomain)

```csharp
// ❌ ZAKÁZÁNO: Rozřešení typu řetězcem za běhu
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ SPRÁVNĚ: Použití ITypeRegistry k vyhledání registrovaných typů
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// Pouze typy registrované během OnLoad jsou zjistitelné
```

### Použití IObjectFactory (Nahrazuje Activator.CreateInstance)

```csharp
// ❌ ZAKÁZÁNO: Vytvoření libovolné instance
object? instance = Activator.CreateInstance(someType);

// ✅ SPRÁVNĚ: Použití IObjectFactory s registrovanou továrnou
var instance = objectFactory.CreateInstance<MyService>();
// Pouze typy s registrovanými továrnami mohou být instanciovány
```

## Demonstrovaná porušení

### Porušení 1: Type.GetType(string)

```csharp
// ❌ ZAKÁZÁNO
Type? fileType = Type.GetType("System.IO.File, System.Runtime");

// ✅ SPRÁVNĚ
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**Blokovaná MemberRef**: `System.Type::GetType(System.String)`

### Porušení 2: Activator.CreateInstance

```csharp
// ❌ ZAKÁZÁNO
object? client = Activator.CreateInstance(httpClientType!);

// ✅ SPRÁVNĚ
var instance = objectFactory.CreateInstance<MyService>();
```

**Blokovaná MemberRef**: `System.Activator::CreateInstance`

### Porušení 3: Assembly.Load

```csharp
// ❌ ZAKÁZÁNO
Assembly asm = Assembly.Load("System.Net.Http");

// ✅ SPRÁVNĚ
Assembly myAsm = typeof(MyPlugin).Assembly;  // Bezpečné: známé v kompilaci
```

**Blokovaná MemberRef**: `System.Reflection.Assembly::Load(System.String)`

### Porušení 4: Assembly.LoadFile / LoadFrom

```csharp
// ❌ ZAKÁZÁNO
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");

// ✅ SPRÁVNĚ
// Všechny závislosti musí být v adresáři pluginu a skenovány PluginLoaderem.
```

**Blokovaná MemberRef**: `System.Reflection.Assembly::LoadFile(System.String)`

### Porušení 5: Assembly.GetType(string)

```csharp
// ❌ ZAKÁZÁNO
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ SPRÁVNĚ
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
```

**Blokovaná MemberRef**: `System.Reflection.Assembly::GetType(System.String)`

## Proč typeof(X).Assembly je bezpečné a Assembly.Load ne

| Operace | Viditelnost | Bezpečnost |
|---------|-----------|-----------|
| `typeof(X).Assembly` | Typ X v tabulce TypeRef → PluginLoader ho skenuje | ✅ Bezpečné |
| `Assembly.Load("X")` | Řetězec "X" existuje pouze za běhu → neviditelný pro sken TypeRef | ❌ Zakázané |
| `obj.GetType()` | Vrací typ existující instance → žádný nový typ | ✅ Bezpečné |
| `Type.GetType("X")` | Rozřeší libovolný typ z řetězce → obchází TypeRef | ❌ Zakázané |

## Osvědčené postupy

1. **Registrovat typy v OnLoad**: Používat `ITypeRegistry.RegisterType` / `RegisterFromAssembly`
2. **Používat IObjectFactory pro dynamické vytváření**: Nikdy nepoužívat `Activator.CreateInstance`
3. **Používat typeof(X).Assembly**: Bezpečný přístup k vlastní assembly
4. **Vyhýbat se názvům typů na základě řetězců**: Aktivuje skenování IL řetězců
5. **Navrhovat pro statickou zjistitelnost**: Neviditelné v metadatech = podezřelé

## Soubory

- `Plugin.cs` - Demonstrační plugin anti-vzoru
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Tento soubor (Čeština)

## Související příklady

- **02-TypeRegistryUsage**: Správné použití ITypeRegistry
- **03-ObjectFactoryUsage**: Správné použití IObjectFactory
- **11-ForbiddenPInvoke**: Zakázané P/Invoke a unsafe kód
- **12-ForbiddenStringBypass**: Pokusy o obcházení reflexí přes řetězce
