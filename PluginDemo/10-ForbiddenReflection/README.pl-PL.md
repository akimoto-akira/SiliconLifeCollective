# PluginDemo-10: Anty-wzorzec zabronionych operacji refleksji

## Przegląd

Ten plugin demonstruje **zabronione** operacje refleksji w systemie pluginów SiliconLife. Służy jako referencja anty-wzorca, pokazując czego NIE należy robić i dostarczając prawidłowe alternatywy dla każdego naruszenia.

## Dlaczego refleksja jest głównym zagrożeniem?

Obejście przez refleksję to **najbardziej krytyczne zagrożenie** dla skanowania bezpieczeństwa PluginLoadera. Podczas gdy skanowanie TypeRef przechwytuje bezpośrednie referencje typów w czasie kompilacji, metody refleksji mogą rozwiązywać typy w **czasie wykonania** używając łańcuchów znaków — całkowicie niewidoczne dla statycznego skanowania metadanych.

Jeśli plugin może wywołać `Type.GetType("System.IO.File, System.Runtime")`, może uzyskać dostęp do DOWOLNEGO zabronionego typu bez żadnej referencji w tabeli TypeRef metadanych PE.

## Jakie metody są zabronione?

Wszystkie zabronione metody są wykrywane przez **skanowanie MemberRef** (nie blokowanie na poziomie przestrzeni nazw lub typu):

| Zabroniona metoda | Sygnatura | Zagrożenie |
|------------------|-----------|-----------|
| `Type.GetType` | `System.Type::GetType(System.String)` | Rozwiązanie dowolnego typu po nazwie w runtime |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | Instancjacja dowolnych typów |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | Ładowanie assembly po nazwie/bajtach |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | Ładowanie assembly z dysku |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | Ładowanie assembly ze ścieżki |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | Rozwiązanie typu oparte na łańcuchach |

## Co jest bezpieczne?

Nie cała refleksja jest zabroniona. Następujące wzorce są **bezpieczne**, ponieważ referencują typy znane w czasie kompilacji:

| Bezpieczny wzorzec | Przykład | Dlaczego bezpieczne |
|-------------------|---------|-------------------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | Typ znany w kompilacji, widoczny w TypeRef |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | Inspekcja znanego typu, bez nowych typów |
| Ograniczenia generyczne | `FindSubtypesOf(typeof(BaseTool))` | Parametr generyczny to typ kompilacji |
| `nameof()` | `nameof(MyClass.MyMethod)` | Łańcuch kompilacji, bez rozwiązania w runtime |

**Kluczowe rozróżnienie:**
- `typeof(X).Assembly` → **Bezpieczne** (referencja kompilacji, skanowane przez PluginLoader)
- `Assembly.Load("X")` → **Zabronione** (łańcuch runtime, omija wszystkie skany)

## Jak bezpiecznie zastąpić refleksję?

### Użyj ITypeRegistry (Zastępuje Type.GetType + skanowanie AppDomain)

```csharp
// ❌ ZABRONIONE: Rozwiązanie typu przez łańcuch w runtime
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ PRAWIDŁOWO: Użyj ITypeRegistry do wyszukiwania zarejestrowanych typów
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// Tylko typy zarejestrowane podczas OnLoad są wykrywalne
```

### Użyj IObjectFactory (Zastępuje Activator.CreateInstance)

```csharp
// ❌ ZABRONIONE: Tworzenie dowolnej instancji
object? instance = Activator.CreateInstance(someType);

// ✅ PRAWIDŁOWO: Użyj IObjectFactory z zarejestrowaną fabryką
var instance = objectFactory.CreateInstance<MyService>();
// Tylko typy z zarejestrowanymi fabrykami mogą być instancjowane
```

## Zademonstrowane naruszenia

### Naruszenie 1: Type.GetType(string)

```csharp
// ❌ ZABRONIONE
Type? fileType = Type.GetType("System.IO.File, System.Runtime");

// ✅ PRAWIDŁOWO
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**Zablokowana MemberRef**: `System.Type::GetType(System.String)`

### Naruszenie 2: Activator.CreateInstance

```csharp
// ❌ ZABRONIONE
object? client = Activator.CreateInstance(httpClientType!);

// ✅ PRAWIDŁOWO
var instance = objectFactory.CreateInstance<MyService>();
```

**Zablokowana MemberRef**: `System.Activator::CreateInstance`

### Naruszenie 3: Assembly.Load

```csharp
// ❌ ZABRONIONE
Assembly asm = Assembly.Load("System.Net.Http");

// ✅ PRAWIDŁOWO
Assembly myAsm = typeof(MyPlugin).Assembly;  // Bezpieczne: znane w kompilacji
```

**Zablokowana MemberRef**: `System.Reflection.Assembly::Load(System.String)`

### Naruszenie 4: Assembly.LoadFile / LoadFrom

```csharp
// ❌ ZABRONIONE
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");

// ✅ PRAWIDŁOWO
// Wszystkie zależności muszą znajdować się w katalogu pluginu i być skanowane przez PluginLoader.
```

**Zablokowana MemberRef**: `System.Reflection.Assembly::LoadFile(System.String)`

### Naruszenie 5: Assembly.GetType(string)

```csharp
// ❌ ZABRONIONE
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ PRAWIDŁOWO
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
```

**Zablokowana MemberRef**: `System.Reflection.Assembly::GetType(System.String)`

## Dlaczego typeof(X).Assembly jest bezpieczne a Assembly.Load nie

| Operacja | Widoczność | Bezpieczeństwo |
|---------|-----------|---------------|
| `typeof(X).Assembly` | Typ X w tabeli TypeRef → PluginLoader go skanuje | ✅ Bezpieczne |
| `Assembly.Load("X")` | Łańcuch "X" istnieje tylko w runtime → niewidoczny dla skanu TypeRef | ❌ Zabronione |
| `obj.GetType()` | Zwraca typ istniejącej instancji → żaden nowy typ | ✅ Bezpieczne |
| `Type.GetType("X")` | Rozwiązuje dowolny typ z łańcucha → omija TypeRef | ❌ Zabronione |

## Najlepsze praktyki

1. **Rejestrować typy w OnLoad**: Używać `ITypeRegistry.RegisterType` / `RegisterFromAssembly`
2. **Używać IObjectFactory do dynamicznego tworzenia**: Nigdy nie używać `Activator.CreateInstance`
3. **Używać typeof(X).Assembly**: Bezpieczny dostęp do własnego assembly
4. **Unikać nazw typów opartych na łańcuchach**: Aktywuje skanowanie łańcuchów IL
5. **Projektować dla statycznej wykrywalności**: Niewidoczne w metadanych = podejrzane

## Pliki

- `Plugin.cs` - Plugin demonstracyjny anty-wzorca
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
- `README.pl-PL.md` - Ten plik (Polski)
- `README.cs-CZ.md` - Čeština

## Powiązane przykłady

- **02-TypeRegistryUsage**: Prawidłowe użycie ITypeRegistry
- **03-ObjectFactoryUsage**: Prawidłowe użycie IObjectFactory
- **11-ForbiddenPInvoke**: Zabronione P/Invoke i kod unsafe
- **12-ForbiddenStringBypass**: Próby obejścia przez refleksję opartą na łańcuchach
