# Projekt bezpieczeństwa

> **Wersja: v0.2.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md) | [Polski](../pl-PL/security.md)

## Przegląd

Bezpieczeństwo Silicon Life Collective opiera się na modelu **obrony warstwowej**. Kluczowa zasada: **wszystkie operacje I/O muszą przechodzić przez wykonawców**, którzy wymuszają sprawdzanie uprawnień przed wykonaniem.

```
Wywołanie narzędzia → Wykonawca → Menedżer uprawnień → Pamięć podręczna wysokiego odmówienia → Pamięć podręczna wysokiego zezwolenia → Wywołanie zwrotne → Zapytanie użytkownika
```

---

## Model uprawnień

### Typy uprawnień

| Typ | Opis |
|------|-------------|
| `NetworkAccess` | Wychodzące żądania HTTP/HTTPS |
| `CommandLine` | Wykonanie poleceń Shell |
| `FileAccess` | Operacje na plikach i katalogach |
| `Function` | Wywołania wrażliwych funkcji |
| `DataAccess` | Dostęp do danych systemowych lub użytkownika |

### Wyniki uprawnień

Każde sprawdzenie uprawnień zwraca jeden z trzech wyników:

| Wynik | Zachowanie |
|--------|----------|
| **Allowed (Zezwolono)** | Operacja jest natychmiast wykonywana |
| **Denied (Odmówiono)** | Operacja jest blokowana, rejestrowana w dzienniku audytu |
| **AskUser (Zapytaj użytkownika)** | Operacja jest wstrzymana, wymaga potwierdzenia użytkownika |

### Specjalna rola: Kurator Krzemowy

Kurator Krzemowy posiada najwyższy poziom uprawnień (`IsCurator = true`). Sprawdzanie uprawnień Kuratora jest zwierane do **zezwolenia**, chyba że użytkownik jawnie nadpisze.

### Prywatny menedżer uprawnień

Każda Istota Krzemowa ma swoją własną **prywatną instancję PermissionManager**. Stan uprawnień nie jest współdzielony między istotami.

---

## Przepływ weryfikacji uprawnień

Priorytet zapytania: **1. Wysokie odmówienie użytkownika → 2. Wysokie zezwolenie użytkownika → 3. Funkcja wywołania zwrotnego**

```
┌─────────────┐
│ Wywołanie    │
│ narzędzia    │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Wykonawca  │────▶│ Prywatny menedżer   │
│ (dyskowy/   │     │ uprawnień           │
│ sieciowy/   │     │ (dla każdej istoty) │
│ wiersz      │     └────────┬────────────┘
│ poleceń...) │              │
└─────────────┘              ▼
                    ┌─────────────────┐
                    │ 1. IsCurator?   │──Tak──▶ Zezwól
                    └────────┬────────┘
                             │ Nie
                             ▼
                    ┌─────────────────┐
                    │ 2. Wysokie      │──Dopasowanie──▶ Odmów
                    │ odmówienie      │
                    │ użytkownika     │
                    │ (pamięć podręczna)│
                    └────────┬────────┘
                             │ Brak dopasowania
                             ▼
                    ┌─────────────────┐
                    │ 3. Wysokie      │──Dopasowanie──▶ Zezwól
                    │ zezwolenie      │
                    │ użytkownika     │
                    │ (pamięć podręczna)│
                    └────────┬────────┘
                             │ Brak dopasowania
                             ▼
                    ┌─────────────────┐
                    │ 4. Funkcja      │
                    │ wywołania       │──▶ Zezwól / Odmów / Zapytaj użytkownika
                    │ zwrotnego       │
                    │ uprawnień       │
                    └─────────────────┘
```

**Kluczowy punkt**: Wykonawca widzi tylko wartość logiczną (zezwól/odmów). Menedżer uprawnień wewnętrznie obsługuje decyzję trójstanową (zezwól/odmów/zapytaj użytkownika) i rozstrzyga zapytanie użytkownika przed zwróceniem do wykonawcy.

---

## Wykonawcy (granica bezpieczeństwa)

Wykonawcy są **jedyną** ścieżką dla operacji I/O. Wymuszają:

### Niezależny wątek dyspozytorski

Każdy wykonawca posiada **niezależny wątek dyspozytorski**:

- Izolacja wątków między wykonawcami — zablokowanie wątku jednego wykonawcy nie wpływa na inne.
- Każdy wykonawca może ustawić niezależne limity zasobów (CPU, pamięć itp.).
- Zarządzanie pulą wątków wykonawcy.

### Kolejka żądań

Każdy wykonawca utrzymuje kolejkę żądań:

- Żądania są kierowane do odpowiedniego wykonawcy według typu.
- Obsługa kolejkowania priorytetowego.
- Kontrola limitu czasu dla każdego żądania.

### Blokowanie wątku weryfikacji uprawnień

Gdy narzędzie inicjuje dostęp do zasobów:

1. Wykonawca odbiera żądanie i **blokuje swój wątek**.
2. Wykonawca odpytuje prywatny menedżer uprawnień istoty.
3. Jeśli wywołanie zwrotne zwraca zapytanie użytkownika, wątek wykonawcy **pozostaje zablokowany** w oczekiwaniu na odpowiedź użytkownika.
4. Istota widzi tylko wynik końcowy (sukces lub odmowa) — nigdy nie widzi pośredniego stanu "oczekujące" lub "czekające".
5. Tylko Kurator Krzemowy wyzwala prawdziwe monity użytkownika. Zwykłe istoty synchronicznie odpytują globalne ACL bez blokowania.
6. Po przekroczeniu czasu, żądanie jest traktowane jako odmowa, a blokada wątku jest zwalniana.

### Typy wykonawców

| Wykonawca | Zakres | Domyślny limit czasu |
|----------|-------|-----------------|
| `DiskExecutor` | Odczyt/zapis plików, operacje na katalogach | 30 sekund |
| `NetworkExecutor` | Żądania HTTP, połączenia WebSocket | 60 sekund |
| `CommandLineExecutor` | Wykonanie poleceń Shell | 120 sekund |
| `DynamicCompilationExecutor` | Kompilacja w pamięci Roslyn | 60 sekund |

### Izolacja wyjątków i odporność na błędy

- Wyjątek jednego wykonawcy nie wpływa na inne wykonawców.
- Automatyczny restart po awarii wątku.
- Bezpiecznik: tymczasowe zatrzymanie wykonawcy po kolejnych niepowodzeniach w celu zapobiegania kaskadowym awariom.

---

## Globalne ACL (lista kontroli dostępu)

Współdzielona tabela reguł utrwalana w przechowywaniu, zarządzana tylko przez Kuratora Krzemowego:

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
- Tylko Kurator Krzemowy może modyfikować globalne ACL (przez swoje dedykowane narzędzia).
- Zmiany wchodzą w życie natychmiast.
- Globalne ACL **nie jest** w powyższym łańcuchu priorytetów dla każdego zapytania — jest wewnętrznie referencjonowane przez funkcję wywołania zwrotnego.

---

## Pamięć podręczna częstotliwości użytkownika

Aby zmniejszyć powtarzające się monity o uprawnienia, system utrzymuje dwie **dla każdej istoty, tylko w pamięci** pamięci podręczne:

| Pamięć podręczna | Cel |
|-------|---------|
| **HighAllow (Wysokie zezwolenie)** | Zasoby często zezwalane przez użytkownika |
| **HighDeny (Wysokie odmówienie)** | Zasoby często odmawiane przez użytkownika |

### Jak to działa

- **Wybór użytkownika, nie automatyczne wykrywanie**: gdy wyzwalane jest zapytanie użytkownika, użytkownik wybiera, czy dodać zasób do pamięci podręcznej.
- **Dopasowanie prefiksu**: obsługa dopasowania prefiksu ścieżki zasobu (np. `network:api.example.com/*`).
- **Priorytet**: wysokie odmówienie ma wyższy priorytet niż wysokie zezwolenie.
- **Tylko w pamięci**: pamięć podręczna nie jest utrwalana. Utrata po restarcie.
- **Konfigurowalna ważność**: użytkownik może ustawić okres ważności wpisów pamięci podręcznej.

### Przepływ aktualizacji pamięci podręcznej

1. Wywołanie zwrotne uprawnień zwraca `AskUser`.
2. System uprawnień wysyła zapytanie do systemu kart (Web UI lub komunikacja natychmiastowa).
3. Użytkownik podejmuje decyzję (zezwól/odmów) i **wybiera, czy buforować**.
4. System kart zwraca decyzję + flagę buforowania.
5. System uprawnień aktualizuje odpowiednią listę pamięci podręcznej.
6. Przyszłe żądania dopasowujące prefiks pamięci podręcznej są natychmiast rozstrzygane.

---

## Mechanizm zapytania użytkownika

Gdy sprawdzenie uprawnień zwraca `AskUser`:

### Web UI: Interaktywna karta

Frontend Web natychmiast wyświetla **interaktywną kartę**, pokazującą:

- Typ zasobu i ścieżkę
- Opis operacji
- Przyciski Zezwól / Odmów
- Opcjonalne pole wyboru "Zawsze zezwalaj" / "Zawsze odmawiaj" (dodaje do pamięci podręcznej częstotliwości)

### Komunikacja natychmiastowa (bez obsługi kart): Kod losowy

Dla platform komunikacyjnych nieobsługujących interaktywnych kart:

1. System generuje dwa losowe 6-cyfrowe kody: **kod zezwolenia** i **kod odmowy**.
2. Wysyła wiadomość zawierającą informacje o zasobie i oba kody.
3. Użytkownik musi odpowiedzieć dokładnym kodem zezwolenia, aby autoryzować. Każda inna odpowiedź jest traktowana jako odmowa.
4. Kody są jednorazowe, aby zapobiec atakom powtórzeń.

### Limit czasu

- Dla wszystkich żądań zapytania użytkownika ustawiony jest limit czasu.
- Po przekroczeniu czasu żądanie jest traktowane jako **odmowa**, a blokada wątku wykonawcy jest zwalniana.

---

## Bezpieczeństwo kompilacji dynamicznej

Samewolucja (przepisywanie klas) wprowadza unikalne zagrożenia bezpieczeństwa. System łagodzi je za pomocą **strategii warstwowej**:

### Warstwa 1: Kontrola referencji w czasie kompilacji (główna obrona)

- Kompilator otrzymuje tylko **listę dozwolonych referencji zestawów**.
- **Dozwolone**: `System.Runtime`, `System.Private.CoreLib`, zestawy projektu (interfejs ITool itp.)
- **Zablokowane**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices` itp.
- Jeśli kod odwołuje się do zablokowanego zestawu, **sam kompilator odrzuca** kod.
- Jest to bardziej niezawodne niż skanowanie w czasie wykonywania — niebezpieczne operacje są niemożliwe na poziomie typu.

### Warstwa 2: Statyczna analiza w czasie wykonywania (obrona wtórna)

- Nawet po pomyślnej kompilacji kod poddawany jest skanowaniu wzorców statycznych.
- Wykrywa niebezpieczne wzorce operacji (bezpośrednie I/O, wywołania systemowe itp.).
- Jeśli zostanie wykryty niebezpieczny kod, ładowanie jest odrzucane, a system powraca do domyślnej funkcjonalności.

### Ograniczenie dziedziczenia

Wszystkie niestandardowe klasy Istot Krzemowych **muszą** dziedziczyć po `SiliconBeingBase`. Kompilator wymusza to ograniczenie na poziomie typu.

### Szyfrowane przechowywanie

Skompilowany kod jest przechowywany na dysku w formie zaszyfrowanej AES-256:

- **Derywacja klucza**: z GUID istoty (wielkimi literami) przy użyciu PBKDF2.
- **Niepowodzenie deszyfrowania**: powrót do domyślnej implementacji.
- **Ponowna kompilacja w czasie wykonywania**: nowy kod jest najpierw kompilowany w pamięci; utrwalanie następuje dopiero po pomyślnej kompilacji i zastąpieniu instancji.

### Atomowe zastąpienie

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
- **Po kompilacji dynamicznej**: nadpisywana przez niestandardową funkcję uprawnień istoty.
- **Albo-albo**: w dowolnym momencie tylko jedno wywołanie zwrotne jest aktywne.
- **Niepowodzenie kompilacji**: nie wpływa na bieżące wywołanie zwrotne — domyślna lub ostatnia udana niestandardowa funkcja pozostaje aktywna.

### Sygnatura wywołania zwrotnego

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Zwraca `Allowed`, `Denied` lub `AskUser`.

---

## Dziennik audytu

Wszystkie decyzje dotyczące uprawnień są rejestrowane:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Dzienniki są utrwalane w przechowywaniu i można je przeglądać przez Web UI (kontroler dzienników).

---

## Audyt użycia Tokenów

`TokenUsageAuditManager` zapewnia śledzenie zużycia tokenów AI związane z bezpieczeństwem:

- **Rejestracja każdego żądania** — każde wywołanie AI rejestruje ID istoty, model, tokeny podpowiedzi, tokeny uzupełnienia i znacznik czasu.
- **Wykrywanie anomalii** — nietypowe wzorce zużycia tokenów mogą wskazywać na wstrzyknięcie podpowiedzi lub nadużycie zasobów.
- **Dostęp tylko dla kuratora** — `TokenAuditTool` (oznaczony jako `[SiliconManagerOnly]`) pozwala kuratorowi na odpytywanie i podsumowywanie użycia tokenów.
- **Pulpit Web** — `UsageController` zapewnia pulpit nawigacyjny oparty na przeglądarce, z wykresami trendów i eksportem danych.
- **Przechowywanie utrwalone** — rekordy są przechowywane przez `ITimeStorage`, umożliwiając zapytania szeregów czasowych i analizę długoterminową.

---

## Bezpieczeństwo wtyczek

System wtyczek wprowadza zagrożenia bezpieczeństwa związane z wykonywaniem kodu stron trzecich, łagodzone przez następujące mechanizmy:

### Bezpieczna piaskownica

`PluginLoader` wykonuje rygorystyczne skanowanie bezpieczeństwa podczas ładowania wtyczek:

1. **Sprawdzanie zabronionych przestrzeni nazw** — wtyczki nie mogą odwoływać się do następujących przestrzeni nazw:
   - `System.IO` — dostęp do systemu plików
   - `System.Net.Http` — żądania HTTP
   - `System.Net.WebSockets` — połączenia WebSocket
   - `System.Net.Sockets` — surowe gniazda
   - `Microsoft.CodeAnalysis` — API kompilatora

2. **Biała lista zaufanych zestawów** — referencje do następujących zestawów są dozwolone:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

3. **Sprawdzanie zabronionych typów** — skanowanie wtyczek pod kątem referencji do niebezpiecznych typów

4. **Sprawdzanie zabronionych członków** — skanowanie wtyczek pod kątem wywołań niebezpiecznych metod

### Izolowane ładowanie

- Każda wtyczka jest ładowana izolowanie przy użyciu niestandardowego `AssemblyLoadContext`
- Typy i zestawy między wtyczkami nie zakłócają się nawzajem
- Zwolnienie wtyczki może zwolnić powiązane zasoby

### Ograniczenia uprawnień narzędzi

- Narzędzia zarejestrowane przez wtyczki poprzez interfejs `ITool` podlegają tym samym ograniczeniom systemu uprawnień
- Narzędzia wtyczek nie mogą ominąć łańcucha uprawnień 5 poziomów
- Narzędzia wtyczek podlegają oznaczeniu `[SiliconManagerOnly]`
