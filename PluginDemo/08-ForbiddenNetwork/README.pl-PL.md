# PluginDemo-08: Anty-wzorzec Zakazanych Operacji Sieciowych

## Przegląd

Ten plugin demonstruje **ZAKAZANE** operacje sieciowe w systemie wtyczek SiliconLife. Służy jako odniesienie anty-wzorca, pokazując, czego NIE robić, i dostarczając prawidłowe alternatywy.

## Dlaczego bezpośredni dostęp do sieci jest globalnie zakazany?

Wzorce bezpośredniego dostępu do sieci są blokowane na poziomie wtyczki:

1. **Połączenie ze złośliwymi serwerami**: Wtyczki mogą łączyć się ze złośliwymi serwerami
2. **Eksfiltracja danych**: Wtyczki mogą wyciekać wrażliwe dane z piaskownicy
3. **Ataki DNS Rebinding**: Wtyczki mogą obchodzić kontrole bezpieczeństwa
4. **Ominięcie ACL sieci**: Bezpośredni dostęp do sieci pomija globalny system ACL

## Zakazane typy

Wszystkie typy `System.Net`, które bezpośrednio uzyskują dostęp do sieci, są blokowane:

| Zakazany typ | Blokowana przestrzeń nazw | Poziom ryzyka |
|----------|----------------|----------|
| `HttpClient` | `System.Net.Http` | 🔴 Krytyczny |
| `TcpClient` | `System.Net.Sockets` | 🔴 Krytyczny |
| `Socket` | `System.Net.Sockets` | 🔴 Krytyczny |
| `Dns` | `System.Net` | 🔴 Krytyczny |
| `WebClient` | `System.Net` | 🔴 Krytyczny |

## Bezpieczne metody dostępu

### NetworkExecutor (Zalecane)

`NetworkExecutor` to **kontrolowany punkt wejścia** dla operacji sieciowych:

```csharp
// ✅ POPRAWNE: Proste żądanie GET
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

**Co zapewnia NetworkExecutor:**
1. Sprawdzanie uprawnień
2. Rejestrowanie audytu
3. Wyłącznik automatyczny
4. Kontrola limitu czasu
5. Kolejka żądań

## Zademonstrowane naruszenia

### Naruszenie 1: HttpClient

```csharp
// ❌ ZAKAZANE
using var client = new HttpClient();

// ✅ POPRAWNE
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

### Naruszenie 2: TcpClient

```csharp
// ❌ ZAKAZANE
using var client = new TcpClient("example.com", 8080);

// ✅ POPRAWNE
// Użyj NetworkExecutor lub zadeklaruj Capability.Network
```

## Mechanizm bezpieczeństwa PluginLoader

PluginLoader analizuje ten plugin i:
1. **Skanowanie TypeRef**: Wykrywa odniesienia do zakazanych typów
2. **Skanowanie MemberRef**: Wykrywa wywołania zablokowanych metod
3. **Skanowanie ciągu IL**: Wykrywa próby refleksji
4. **Odrzucenie**: Plugin jest odrzucany podczas ładowania

## Pliki

- `Plugin.cs` - Plugin demonstracyjny anty-wzorca
- `README.md` - Ten plik (Angielski)
- `README.pl-PL.md` - Ten plik (Polski)
- Inne wersje językowe...

## Powiązane przykłady

- **13-CapabilityNetwork**: Deklaratywna możliwość sieciowa
- **07-ForbiddenFileIO**: Zakazane wzorce dostępu do plików