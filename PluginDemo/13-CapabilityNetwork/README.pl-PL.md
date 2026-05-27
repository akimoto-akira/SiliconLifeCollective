# PluginDemo-13: Capability.Network — Deklaratywne uprawnienie sieciowe

## Przegląd

Ten plugin demonstruje użycie `[PluginCapability(Capability.Network)]` do deklaracji dostępu do sieci. Dzięki tej deklaracji plugin uzyskuje dostęp do typów `System.Net.*`, które w przeciwnym razie zostałyby zablokowane przez skan bezpieczeństwa PluginLoader.

## Składnia deklaracji

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**Kluczowe elementy:**
- **Cel atrybutu**: Musi znajdować się na klasie, która bezpośrednio implementuje `IPlugin`
- **AllowMultiple = true**: Można układać wiele atrybutów `[PluginCapability]` (patrz 17-CapabilityStacked)
- **Pole Reason**: Czytelne dla człowieka wyjaśnienie zapisywane w dzienniku audytu bezpieczeństwa podczas ładowania. **Zdecydowanie zaleca się podawanie jasnego Reason dla wszystkich wtyczek produkcyjnych.**

## Jak PluginLoader przetwarza deklaracje możliwości

1. **Odczyt metadanych PE**: PluginLoader odczytuje deklaracje możliwości z tabeli CustomAttribute pliku PE **przed** rozpoczęciem skanowania bezpieczeństwa
2. **Złagodzenie reguł skanowania**: Zadeklarowane możliwości zwalniają odpowiadające im odwołania do typów z kontroli zabronionych przestrzeni nazw i zabronionych typów
3. **Rejestrowanie audytu**: Wszystkie deklaracje (w tym Reason) są zapisywane w dzienniku audytu bezpieczeństwa
4. **Niezadeklarowane możliwości**: P/Invoke, Unsafe, Reflection.Emit itp. pozostają zablokowane niezależnie od jakiejkolwiek deklaracji

## Zakres zwolnienia Capability.Network

### Zwolnienia TypeRef

Gdy `Capability.Network` jest zadeklarowana, następujące reguły zakazów oparte na przestrzeni nazw i typach są złagodzone:

| Zwolniona przestrzeń nazw | Dozwolone typy |
|--------------------------|---------------|
| `System.Net.Http` | `HttpClient`, `HttpRequestMessage`, `HttpResponseMessage` itd. |
| `System.Net.WebSockets` | `ClientWebSocket`, `WebSocket` itd. |
| `System.Net.Sockets` | `TcpClient`, `UdpClient`, `Socket` itd. |
| `System.Net.Mail` | `SmtpClient`, `MailMessage` itd. |
| `System.Net.NetworkInformation` | `Ping`, `NetworkInterface` itd. |
| `System.Net.Security` | `SslStream` itd. |
| `System.Net` (zakazy wg typu) | `HttpWebRequest`, `WebClient`, `Dns`, `FtpWebRequest` itd. |

### Zwolnienia ILString

Stałe łańcuchowe zaczynające się od tych prefiksów nie są oznaczane podczas skanowania sterty #US:
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### Co pozostaje zabronione

Nawet z `Capability.Network` te możliwości są **zawsze** zablokowane (niezadeklarowane możliwości):

| Kategoria | Zablokowane typy | Dlaczego niezadeklarowane |
|----------|-----------------|--------------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` | Nie można bezpiecznie audytować w czasie wykonania |
| Kod unsafe | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Omija gwarancje bezpieczeństwa typów |
| Emisja IL | `System.Reflection.Emit.*` | Może generować dowolny kod w czasie wykonania |
| Ładowanie zestawów | `System.Runtime.Loader`, `Assembly.Load*` | Może ominąć skan bezpieczeństwa ładując niesprawdzone DLL |
| Rejestr | `Microsoft.Win32.*` | Dostęp do systemu na poziomie OS poza piaskownicą wtyczki |

## Pole Reason — Rola audytu

Pole `Reason` służy jako **ścieżka audytu** dla deklaracji możliwości:

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Dlaczego Reason jest ważny:**
1. **Przegląd bezpieczeństwa**: Audytorzy mogą zweryfikować, czy zadeklarowane możliwości odpowiadają rzeczywistemu zachowaniu wtyczki
2. **Zasada najmniejszych uprawnień**: Zmusza autorów wtyczek do uzasadnienia potrzeby każdej możliwości
3. **Zgodność**: Wymagane dla certyfikacji bezpieczeństwa i dochodzeń w sprawie incydentów
4. **Monitorowanie w czasie wykonania**: Narzędzia bezpieczeństwa mogą ostrzegać, jeśli użycie zadeklarowanej możliwości przekracza podaną przyczynę

## Porównanie z 08-ForbiddenNetwork

| Aspekt | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|--------|-------------------|---------------------|
| Deklaracja | Brak | `[PluginCapability(Capability.Network)]` |
| Wynik ładowania | ❌ Odrzucony | ✅ Załadowany pomyślnie |
| Użycie HttpClient | Zablokowane przez skan TypeRef | Zwolnione przez deklarację |
| Użycie TcpClient | Zablokowane przez skan TypeRef | Zwolnione przez deklarację |
| Reason | Nie dotyczy | Zapisane w dzienniku audytu |

**Kluczowa różnica**: 08-ForbiddenNetwork pokazuje, co się dzieje, gdy używa się typów sieciowych **bez** deklarowania możliwości. 13-CapabilityNetwork pokazuje **prawidłowy** sposób deklaratywnego żądania dostępu do sieci.

## Najlepsze praktyki bezpieczeństwa

1. **Deklarować tylko to, co potrzebne**: Jeśli potrzebujesz tylko HTTP, nie deklaruj Capability.Network tylko dlatego że możesz — ale zauważ, że Capability.Network to jedyna możliwość związana z siecią; nie ma bardziej szczegółowych opcji
2. **Preferować NetworkExecutor**: `NetworkExecutor` to kontrolowany punkt wejścia dla dostępu do sieci i nie wymaga deklaracji możliwości
3. **Podać jasny Reason**: Niejasne powody jak „dostęp do sieci" to sygnał ostrzegawczy przy przeglądzie bezpieczeństwa
4. **Pamiętać o niezadeklarowanych ograniczeniach**: Żadna deklaracja możliwości nie może obejść zakazów P/Invoke, Unsafe lub Reflection.Emit

## Pliki

- `Plugin.cs` — Wtyczka demo deklarująca Capability.Network
- `README.md` — English
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 日本語
- `README.ko-KR.md` — 한국어
- `README.de-DE.md` — Deutsch
- `README.fr-FR.md` — Français
- `README.es-ES.md` — Español
- `README.it-IT.md` — Italiano
- `README.ru-RU.md` — Русский
- `README.pt-PT.md` — Português
- `README.pl-PL.md` — Ten plik (Polski)
- `README.cs-CZ.md` — Čeština

## Powiązane przykłady

- **08-ForbiddenNetwork**: Antywzorzec zablokowanych operacji sieciowych
- **14-CapabilityFileIO**: Deklaratywna możliwość FileIO
- **15-CapabilityProcess**: Deklaratywna możliwość Process
- **16-CapabilityAI**: Deklaratywna możliwość usługi AI
- **17-CapabilityStacked**: Układanie wielu możliwości w stos
- **18-CapabilityDenied**: Antywzorzec niezadeklarowanej możliwości
