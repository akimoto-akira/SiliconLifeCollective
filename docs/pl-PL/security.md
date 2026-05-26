# Projekt bezpieczeństwa

> **Wersja: v0.2.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md) | [Русский](../ru-RU/security.md)

## Przegląd

Bezpieczeństwo Silicon Life Collective opiera się na modelu **obrony w głębi**. Zasada podstawowa: **wszystkie operacje I/O muszą przechodzić przez wykonawcę**, który wymusza sprawdzanie uprawnień przed wykonaniem.

```
Wywołanie narzędzia → Wykonawca → Menedżer Uprawnień → Pamięć podręczna częstotliwości → Wywołanie zwrotne → (IsCurator: zapytaj użytkownika | Non-curator: globalna ACL)
```

---

## Model uprawnień

### Typy uprawnień

| Typ | Opis |
|------|-------------|
| `NetworkAccess` | Wychodzące żądania HTTP/HTTPS |
| `CommandLine` | Wykonywanie poleceń powłoki |
| `FileAccess` | Operacje na plikach i katalogach |
| `Function` | Wywołania wrażliwych funkcji |
| `DataAccess` | Dostęp do danych systemowych lub użytkownika |

### Wyniki uprawnień

Każde sprawdzenie uprawnień zwraca jeden z trzech wyników:

| Wynik | Zachowanie |
|--------|----------|
| **Allowed (Dozwolone)** | Operacja jest natychmiast wykonywana |
| **Denied (Odrzucone)** | Operacja jest blokowana, rejestrowana w dzienniku audytu |
| **AskUser (Zapytaj użytkownika)** | Operacja jest wstrzymywana, wymaga potwierdzenia użytkownika |

### Rola specjalna: Kurator Krzemowy

Kurator Krzemowy posiada najwyższy poziom uprawnień (`IsCurator = true`). Gdy łańcuch uprawnień dociera do punktu rozgałęzienia, operacje kuratora są kierowane przez `IPermissionAskHandler` z prośbą o potwierdzenie użytkownika, zamiast automatycznie zwracać wynik „dozwolone". Istoty niebędące kuratorami odpytują globalną ACL.

### Prywatny menedżer uprawnień

Każda Istota Krzemowa posiada własną **prywatną instancję PermissionManager**. Stan uprawnień nie jest współdzielony między istotami.

---

## Przepływ weryfikacji uprawnień

Kolejność odpytywania: **1. Pamięć podręczna częstotliwości → 2. Funkcja wywołania zwrotnego → 3. Rozgałęzienie (IsCurator/GlobalACL)**

```
┌─────────────┐
│ Wywołanie    │
│ narzędzia    │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Wykonawca  │────▶│ Prywatny            │
│ (dysk/sieć/ │     │ menedżer uprawnień  │
│  wiersz     │     │ (dla każdej istoty) │
│  poleceń...)│     └────────┬────────────┘
└─────────────┘            │
                           ▼
                  ┌─────────────────┐
                  │ 1. Pamięć       │──dopasowanie──▶ Dozwolone / Odrzucone
                  │ podręczna       │
                  │ częstotliwości  │
                  │ (wys. odrzucenie│
                  │  > wys. pozwol.)│
                  └────────┬────────┘
                           │ Brak dopasowania
                           ▼
                  ┌─────────────────┐
                  │ 2. Funkcja      │
                  │ wywołania       │──▶ Dozwolone / Odrzucone / Zapytaj
                  │ zwrotnego       │    użytkownika
                  │ uprawnień       │
                  └────────┬────────┘
                           │ Zapytaj użytkownika
                           ▼
                  ┌─────────────────┐
                  │ 3. IsCurator?   │
                  └────────┬────────┘
                           │
                 ┌─────────┴─────────┐
                 │                   │
                 ▼ Tak               ▼ Nie
          ┌─────────────┐    ┌─────────────┐
          │ Zapytaj     │    │ Globalna    │
          │ użytkownika │    │ ACL         │
          │ (AskHandler)│    │ Odpytaj     │
          └─────────────┘    │ reguły      │
                             └─────────────┘
```

**Kluczowy punkt**: wykonawca widzi tylko wartość logiczną (dozwolone/odrzucone). Menedżer uprawnień wewnętrznie obsługuje decyzję trójstanową (dozwolone/odrzucone/zapytaj użytkownika) i rozwiązuje stan „zapytaj użytkownika" przed zwróceniem wyniku do wykonawcy.

---

## Wykonawcy (granica bezpieczeństwa)

Wykonawcy są **jedyną** ścieżką dla operacji I/O. Wymuszają:

### Niezależny wątek dyspozytora

Każdy wykonawca posiada **niezależny wątek dyspozytora**:

- Izolacja wątków między wykonawcami — zablokowanie wątku jednego wykonawcy nie wpływa na inne.
- Każdy wykonawca może mieć niezależne limity zasobów (CPU, pamięć itp.).
- Zarządzanie pulą wątków wątków wykonawcy.

### Kolejka żądań

Każdy wykonawca utrzymuje kolejkę żądań:

- Żądania są kierowane do odpowiedniego wykonawcy na podstawie typu.
- Obsługa kolejkowania z priorytetami.
- Kontrola limitu czasu dla każdego żądania.

### Blokowanie wątku podczas weryfikacji uprawnień

Gdy narzędzie inicjuje dostęp do zasobu:

1. Wykonawca odbiera żądanie i **blokuje swój wątek**.
2. Wykonawca odpytuje prywatny menedżer uprawnień istoty.
3. Jeśli wywołanie zwrotne zwraca „zapytaj użytkownika", wątek wykonawcy **pozostaje zablokowany**, czekając na odpowiedź użytkownika.
4. Istota widzi tylko wynik końcowy (sukces lub odrzucenie) — nigdy nie widzi stanu pośredniego „oczekujące" lub „czekające".
5. Tylko Kurator Krzemowy wyzwala rzeczywisty monit użytkownika. Zwykłe istoty synchronicznie odpytują globalną ACL bez blokowania.
6. Po przekroczeniu limitu czasu żądanie jest traktowane jako odrzucone, a blokada wątku jest zwalniana.

### Typy wykonawców

| Wykonawca | Zakres | Domyślny limit czasu |
|----------|-------|-----------------|
| `DiskExecutor` | Odczyt/zapis plików, operacje na katalogach | 30 sekund |
| `NetworkExecutor` | Żądania HTTP, połączenia WebSocket | 60 sekund |
| `CommandLineExecutor` | Wykonywanie poleceń powłoki | 120 sekund |

> **Uwaga**: `DynamicCompilationExecutor` (w przestrzeni nazw `SiliconLife.Core.Compilation`) odpowiada za kompilację w pamięci Roslyn i nie należy do kategorii wykonawców I/O, ale podlega tym samym ograniczeniom systemu uprawnień.

### Izolacja wyjątków i tolerancja błędów

- Wyjątek w jednym wykonawcy nie wpływa na inne wykonawce.
- Automatyczny restart wątku po awarii.
| Wyłącznik: tymczasowe zatrzymanie wykonawcy po kolejnych awariach w celu zapobiegania kaskadowym błędom.

---

## Globalna ACL (lista kontroli dostępu)

Współdzielona tabela reguł utrwalana w pamięci masowej, zarządzana wyłącznie przez Kuratora Krzemowego:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- Reguły są oceniane w kolejności; pierwsze dopasowanie wygrywa.
- Tylko Kurator Krzemowy może modyfikować globalną ACL (za pomocą dedykowanego narzędzia).
- Zmiany wchodzą w życie natychmiast.
- Globalna ACL **nie znajduje się** w powyższym łańcuchu priorytetów dla każdego zapytania — jest wewnętrznie referencjonowana przez funkcję wywołania zwrotnego.

---

## Pamięć podręczna częstotliwości użytkownika

Aby zmniejszyć liczbę powtarzających się monitów o uprawnienia, system utrzymuje dwie **dla każdej istoty, wyłącznie w pamięci** podręczne:

| Pamięć podręczna | Przeznaczenie |
|-------|---------|
| **HighAllow (wysokie pozwolenie)** | Zasoby często dozwolone przez użytkownika |
| **HighDeny (wysokie odrzucenie)** | Zasoby często odrzucane przez użytkownika |

### Jak to działa

- **Wybór użytkownika, nie automatyczne wykrywanie**: gdy wyzwalane jest zapytanie do użytkownika, użytkownik decyduje, czy dodać zasób do pamięci podręcznej.
- **Dopasowanie prefiksu**: obsługa dopasowania prefiksu ścieżki zasobu (np. `network:api.example.com/*`).
- **Priorytet**: wysokie odrzucenie ma wyższy priorytet niż wysokie pozwolenie.
- **Tylko w pamięci**: pamięć podręczna nie jest utrwalana. Utrata danych po ponownym uruchomieniu.
- **Konfigurowalna ważność**: użytkownik może ustawić okres ważności wpisów pamięci podręcznej.

### Przepływ aktualizacji pamięci podręcznej

1. Wywołanie zwrotne uprawnień zwraca `AskUser`.
2. System uprawnień wysyła zapytanie do systemu kart (Web UI lub komunikator).
3. Użytkownik podejmuje decyzję (dozwolone/odrzucone) i **wybiera, czy zapisać w pamięci podręcznej**.
4. System kart zwraca decyzję + flagę pamięci podręcznej.
5. System uprawnień aktualizuje odpowiednią listę pamięci podręcznej.
6. Przyszłe żądania pasujące do prefiksu pamięci podręcznej są natychmiast rozwiązywane.

---

## Mechanizm zapytania użytkownika

Gdy sprawdzenie uprawnień zwraca `AskUser`:

### Web UI: interaktywna karta

Frontend webowy natychmiast wyświetla **interaktywną kartę** pokazującą:

- Typ i ścieżkę zasobu
- Opis operacji
- Przyciski Dozwolone / Odrzucone
- Opcjonalne pola wyboru „Zawsze dozwolone" / „Zawsze odrzucone" (dodanie do pamięci podręcznej częstotliwości)

### Komunikator (bez obsługi kart): kod losowy

Dla platform komunikacyjnych nieobsługujących interaktywnych kart:

1. System generuje dwa losowe 6-cyfrowe kody: **kod pozwolenia** i **kod odrzucenia**.
2. Wysyłana jest wiadomość zawierająca informacje o zasobie i oba kody.
3. Użytkownik musi odpowiedzieć dokładnym kodem pozwolenia, aby autoryzować. Każda inna odpowiedź jest traktowana jako odrzucenie.
4. Kody są jednorazowego użytku, aby zapobiec atakom powtórkowym.

### Limit czasu

- Dla wszystkich żądań zapytania użytkownika ustawiony jest limit czasu.
- Po przekroczeniu limitu czasu żądanie jest traktowane jako **odrzucone**, a blokada wątku wykonawcy jest zwalniana.

---

## Bezpieczeństwo kompilacji dynamicznej

Samodoskonalenie (przepisywanie klas) wprowadza unikalne zagrożenia bezpieczeństwa. System łagodzi je za pomocą **strategii warstwowej**:

### Warstwa 1: kontrola referencji w czasie kompilacji (główna obrona)

- Kompilator otrzymuje tylko **listę dozwolonych referencji zestawów**.
- **Dozwolone**: `System.Runtime`, `System.Private.CoreLib`, zestawy projektu (interfejs ITool itp.)
- **Zablokowane**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices` itp.
- Jeśli kod odwołuje się do zablokowanego zestawu, **sam kompilator odrzuca** kod.
- Jest to bardziej niezawodne niż skanowanie w czasie wykonywania — niebezpieczne operacje są niemożliwe na poziomie typu.

### Warstwa 2: statyczna analiza w czasie wykonywania (obrona dodatkowa)

- Nawet po pomyślnej kompilacji kod poddawany jest statycznemu skanowaniu wzorców.
- Wykrywa niebezpieczne wzorce operacji (bezpośrednie I/O, wywołania systemowe itp.).
- Jeśli wykryto niebezpieczny kod, ładowanie jest odrzucane, a system powraca do domyślnej funkcjonalności.

### Ograniczenie dziedziczenia

Wszystkie niestandardowe klasy Istot Krzemowych **muszą** dziedziczyć po `SiliconBeingBase`. Kompilator wymusza to ograniczenie na poziomie typu.

### Szyfrowanie przechowywania

Skompilowany kod jest przechowywany na dysku w postaci zaszyfrowanej AES-256:

- **Derywacja klucza**: z GUID istoty (wielkimi literami) przy użyciu PBKDF2.
- **Niepowodzenie deszyfrowania**: powrót do domyślnej implementacji.
- **Rekompilacja w czasie wykonywania**: nowy kod jest najpierw kompilowany w pamięci; utrwalanie następuje dopiero po pomyślnej kompilacji i zastąpieniu instancji.

### Zastąpienie atomowe

Proces zastąpienia jest atomowy:

1. Kompilacja nowego kodu w pamięci → uzyskanie `Type`.
2. Utworzenie nowej instancji z `Type`.
3. Migracja stanu ze starej instancji do nowej.
4. Zamiana referencji.
5. Utrwalenie zaszyfrowanego kodu.

Jeśli którykolwiek krok nie powiedzie się, stara instancja pozostaje aktywna.

---

## Funkcja wywołania zwrotnego uprawnień

### Projekt

Każdy PermissionManager posiada **zmienną funkcji wywołania zwrotnego**:

- **Domyślnie**: wskazuje na wbudowaną domyślną funkcję uprawnień.
- **Po kompilacji dynamicznej**: zastępowana przez niestandardową funkcję uprawnień istoty.
- **Albo-albo**: w dowolnym momencie aktywna jest tylko jedna funkcja wywołania zwrotnego.
- **Niepowodzenie kompilacji**: nie wpływa na bieżące wywołanie zwrotne — domyślna lub ostatnia pomyślna funkcja niestandardowa pozostaje aktywna.

### Sygnatura wywołania zwrotnego

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Zwraca `Allowed`, `Denied` lub `AskUser`.

---

## Dziennik audytu

Wszystkie decyzje uprawnień są rejestrowane:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Dzienniki są utrwalane w pamięci masowej i dostępne do przeglądania przez Web UI (kontroler dzienników).

---

## Audyt wykorzystania tokenów

`TokenUsageAuditManager` zapewnia śledzenie zużycia tokenów AI związane z bezpieczeństwem:

- **Rejestrowanie każdego żądania** — każde wywołanie AI rejestruje ID istoty, model, tokeny promptu, tokeny uzupełnienia i znacznik czasowy.
- **Wykrywanie anomalii** — nietypowe wzorce zużycia tokenów mogą wskazywać na wstrzyknięcie promptu lub nadużycie zasobów.
- **Dostęp tylko dla kuratora** — `TokenAuditTool` (oznaczony `[SiliconManagerOnly]`) pozwala kuratorowi odpytywać i podsumowywać wykorzystanie tokenów.
- **Pulpit webowy** — `UsageController` udostępnia pulpit oparty na przeglądarce z wykresami trendów i eksportem danych.
- **Przechowywanie trwałe** — rekordy są przechowywane przez `ITimeStorage` do zapytań szeregów czasowych i analizy długoterminowej.

---

## Bezpieczeństwo wtyczek

System wtyczek wprowadza zagrożenia bezpieczeństwa związane z wykonywaniem kodu stron trzecich, łagodzone przez następujące mechanizmy:

### Bezpieczna piaskownica

`PluginLoader` wykonuje rygorystyczne skanowanie bezpieczeństwa podczas ładowania wtyczek:

1. **Sprawdzanie zakazanych przestrzeni nazw** — wtyczki nie mogą odwoływać się do następujących przestrzeni nazw:
   - `System.IO` — dostęp do systemu plików
   - `System.Net.Http` — żądania HTTP
   - `System.Net.WebSockets` — połączenia WebSocket
   - `System.Net.Sockets` — gniazda surowe
   - `Microsoft.CodeAnalysis` — API kompilatora

2. **Biała lista zaufanych zestawów** — referencje do następujących zestawów są dozwolone:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

3. **Sprawdzanie zakazanych typów** — skanowanie wtyczek pod kątem referencji do niebezpiecznych typów

4. **Sprawdzanie zakazanych składowych** — skanowanie wtyczek pod kątem wywołań niebezpiecznych metod

### Izolowane ładowanie

- Każda wtyczka jest ładowana izolowanie przy użyciu niestandardowego `AssemblyLoadContext`
- Typy i zestawy między wtyczkami nie zakłócają się nawzajem
- Podczas zwalniania wtyczki można zwolnić powiązane zasoby

### Ograniczenia uprawnień narzędzi

- Narzędzia zarejestrowane przez wtyczki za pomocą interfejsu `ITool` podlegają tym samym ograniczeniom systemu uprawnień
- Narzędzia wtyczek nie mogą ominąć łańcucha weryfikacji uprawnień
- Narzędzia wtyczek podlegają oznaczeniu `[SiliconManagerOnly]`

---

## Bezpieczeństwo uprawnień narzędzi

System uprawnień narzędzi zapewnia dodatkową warstwę bezpieczeństwa, kontrolując, które operacje narzędzi mogą być używane przez Istoty Krzemowe:

### Dwupoziomowa izolacja uprawnień

1. **Poziom Istoty Krzemowej** — każda Istota Krzemowa ma niezależną konfigurację uprawnień narzędzi
2. **Poziom projektu** — uprawnienia narzędzi w przestrzeni projektu są niezależne od poziomu istoty, realizując izolację uprawnień między projektami

### Szablony uprawnień

System udostępnia predefiniowane szablony uprawnień zapewniające linię bazową bezpieczeństwa:

- **readonly** — minimalne uprawnienia, zezwala tylko na operacje odczytu
- **restricted** — ograniczone uprawnienia, zezwala tylko na podstawowe operacje
- **full** — pełne uprawnienia (tylko dla kuratora)

### Cechy bezpieczeństwa

- **Domyślne odrzucenie** — operacje narzędzi nie wprost dozwolone są domyślnie odrzucane
- **Granularność operacji** — każda operacja każdego narzędzia jest kontrolowana niezależnie (np. `network:get` dozwolone, ale `network:post` odrzucone)
- **Zarządzanie przez kuratora** — uprawnieniami narzędzi może zarządzać tylko Kurator Krzemowy
- **Ślad audytu** — zmiany uprawnień narzędzi są rejestrowane w dzienniku audytu
