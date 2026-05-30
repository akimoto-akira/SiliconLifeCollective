# Przewodnik Web UI

> **Wersja: v0.2.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md) | [Русский](../ru-RU/web-ui-guide.md)

## Przegląd

Web UI zapewnia kompleksowy interfejs do zarządzania Istotami Krzemowymi, monitorowania stanu systemu i interakcji z agentami AI. System wykorzystuje architekturę czystego renderowania po stronie serwera, bez zależności od frameworków frontendowych, generując HTML, CSS i JavaScript przez konstruktory `H`, `CssBuilder` i `JsBuilder`.

## Dostęp

Domyślny URL: `http://localhost:8080`

## Nawigacja

### Główne sekcje

1. **Panel nawigacyjny** — przegląd systemu i wskaźniki
2. **Istoty** — zarządzanie Istotami Krzemowymi
3. **Czat** — interakcja z istotami (obsługa przesyłania plików, SSE w czasie rzeczywistym)
4. **Historia czatu** — przeglądanie historii czatu Istot Krzemowych (lista sesji, szczegóły wiadomości)
5. **Zadania** — zarządzanie zadaniami (zadania osobiste)
6. **Czasomierze** — konfiguracja czasomierzy (tworzenie, wstrzymywanie, historia wykonania)
7. **Konfiguracja** — ustawienia systemu (klienci AI, lokalizacja)
8. **Uprawnienia** — kontrola dostępu (zarządzanie ACL, zapytania o uprawnienia)
9. **Logi** — logi systemowe (filtrowanie według poziomu, zapytania o zakres czasu)
10. **Audyt** — użycie tokenów i ślad audytu
11. **Pamięć** — pamięć istot (widok osi czasu, zaawansowane filtrowanie)
12. **Wiedza** — baza wiedzy (zarządzanie trójkami, odkrywanie ścieżek)
13. **Przeglądarka kodu** — eksploracja kodu (drzewo plików, podświetlanie składni)
14. **Edytor kodu** — edycja kodu z podpowiedziami (Monaco Editor)
15. **Projekty** — zarządzanie projektami (obszar roboczy, zadania, notatki pracy)
16. **Wykonawcy** — zarządzanie wykonawcami (dyskowy, sieciowy, wiersza poleceń)
17. **Pomoc** — system dokumentacji pomocy (obsługa wielojęzyczna, wyszukiwanie tematów)
18. **O projekcie** — informacje o systemie i wersji

---

## Panel nawigacyjny

### Funkcje

- Wskaźniki wydajności systemu (CPU, pamięć, czas działania)
- Przegląd statusu istot
- Statystyki użycia AI
- Szybkie akcje

### Aktualizacje w czasie rzeczywistym

Wykorzystanie SSE (zdarzeń wysyłanych przez serwer) do pobierania danych w czasie rzeczywistym:

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Zarządzanie Istotami

### Lista istot

Wyświetla wszystkie istoty, zawierając:
- Nazwę i identyfikator
- Bieżący status (uruchomiona/zatrzymana/błąd)
- Link do Pliku Duszy
- Szybkie akcje (uruchom/zatrzymaj/konfiguruj)

### Szczegóły istoty

- Pełna konfiguracja
- Edytor Pliku Duszy
- Historia zadań
- Przeglądarka pamięci
- Wskaźniki wydajności

### Tworzenie istoty

1. Kliknij **Utwórz nową istotę**
2. Wypełnij:
   - Nazwa
   - Treść duszy (edytor Markdown)
   - Konfiguracja początkowa
3. Kliknij **Utwórz**

---

## Interfejs czatu

### Funkcje

- Strumień wiadomości w czasie rzeczywistym
- Historia wiadomości
- Obsługa wielu sesji
- Wizualizacja wywołań narzędzi

### Korzystanie z czatu

1. Wybierz istotę
2. Wpisz wiadomość
3. Obejrzyj strumieniową odpowiedź
4. Obserwuj wykonywanie narzędzi w czasie rzeczywistym

### Wyświetlanie wywołań narzędzi

Gdy AI wywołuje narzędzie:
```
🔧 Narzędzie: calendar
📥 Wejście: {"date": "2026-04-20"}
📤 Wyjście: "Czwarty miesiąc księżycowy, trzeci dzień"
```

---

## Konfiguracja

### Klienci AI

Konfiguracja backendów AI:
- Ollama (lokalna)
- Bailian (chmurowa)
- Volcengine Ark (chmurowa)
- Herdsman (lokalna/chmurowa, bez autoryzacji)
- Meituan LongCat (chmurowa)
- Qiniu Cloud AI (chmurowa)
- Niestandardowi klienci

### Ustawienia przechowywania

- Wersja Default: ścieżka bazowa, indeksowanie czasowe, zasady czyszczenia
- Wersja Fast: konfiguracja silnika przechowywania SpeedyPack, zarządzanie plikami .spk, ustawienia automatycznej kompakcji

### Lokalizacja

Przełączanie między 34 wariantami językowymi:
- Chiński (6): uproszczony, tradycyjny, Singapur, Makau, Tajwan, Malezja
- Angielski (10): amerykański, brytyjski, kanadyjski, australijski, indyjski, singapurski, południowoafrykański, irlandzki, nowozelandzki, malezyjski
- Hiszpański (2): hiszpański, meksykański
- Niemiecki (5): niemiecki, austriacki, szwajcarski, luksemburski, liechtensteiński
- Francuski (3): francuski, kanadyjski, szwajcarski
- Japoński, koreański, czeski
- Rosyjski, portugalski (2), włoski, niderlandzki, polski, szwedzki

---

## System skórek

### Dostępne skórki

1. **Admin** — profesjonalny interfejs zarządzania
2. **Chat** — design zorientowany na konwersację
3. **Creative** — styl kreatywny i artystyczny
4. **Dev** — układ zorientowany na deweloperów
5. **HighContrast** — motyw o wysokim kontraście (wersja Fast)
6. **Minimal** — styl minimalistyczny (wersja Fast)
7. **Light** — jasny motyw (wersja Fast)

### Przełączanie skórek

1. Kliknij **Ustawienia** (ikona zębatki)
2. Wybierz **Skórki**
3. Wybierz żądaną skórkę
4. Interfejs natychmiast się aktualizuje

### Niestandardowe skórki

Tworzenie niestandardowych skórek przez implementację `ISkin`:

```csharp
public class MySkin : ISkin
{
    public string Name => "MySkin";
    
    public string GetCss()
    {
        return ":root { --primary: #color; }";
    }
}
```

---

## Zarządzanie uprawnieniami

### Przeglądanie uprawnień

- Lista wszystkich reguł uprawnień
- Filtrowanie według użytkownika lub zasobu
- Przeglądanie dat wygaśnięcia

### Dodawanie reguł uprawnień

1. Kliknij **Dodaj regułę**
2. Skonfiguruj:
   - Typ uprawnień (np. `FileAccess`, `NetworkAccess`)
   - Prefiks zasobu (np. `C:\Projects`, `api.github.com`)
   - Zezwól/Odmów
   - Opis
3. Zapisz

### Ślad audytu

Przeglądanie wszystkich decyzji dotyczących uprawnień:
- Znacznik czasu
- Użytkownik
- Zasób
- Decyzja
- Przyczyna

### Zarządzanie uprawnieniami narzędzi

Zarządzanie uprawnieniami operacji narzędzi dla Istot Krzemowych i projektów:

1. **Uprawnienia narzędzi Istoty Krzemowej**:
   - Przejdź do **Istoty** → wybierz istotę → **Uprawnienia narzędzi**
   - Przeglądaj bieżącą konfigurację uprawnień
   - Ustaw zezwolenie/odmowę dla każdej operacji
   - Zastosuj szablon uprawnień (readonly/restricted/full)

2. **Uprawnienia narzędzi projektu**:
   - Przejdź do **Projekty** → wybierz projekt → **Uprawnienia narzędzi**
   - Uprawnienia narzędzi na poziomie projektu są niezależne od uprawnień na poziomie Istoty Krzemowej
   - Realizują izolację uprawnień między projektami

---

## Zarządzanie zadaniami

### Lista zadań

- Wszystkie zadania i ich status
- Filtrowanie według istoty lub statusu
- Wskaźniki priorytetu

### Szczegóły zadania

- Opis
- Priorytet
- Termin realizacji
- Historia wykonania
- Wynik

### Tworzenie zadania

1. Kliknij **Utwórz zadanie**
2. Wypełnij:
   - Przypisanie istoty
   - Opis
   - Priorytet (1-10)
   - Termin realizacji
3. Utwórz

---

## Zarządzanie czasomierzami

### Aktywne czasomierze

- Lista uruchomionych czasomierzy
- Czas następnego wykonania
- Status powtarzania

### Tworzenie czasomierza

1. Kliknij **Utwórz czasomierz**
2. Skonfiguruj:
   - Przypisanie istoty
   - Interwał lub wyrażenie cron
   - Akcja do wykonania
   - Ustawienia powtarzania
3. Uruchom

---

## Przeglądarka logów

### Funkcje

- Filtrowanie według poziomu (informacja/ostrzeżenie/błąd)
- Wyszukiwanie po słowach kluczowych
- Wybór zakresu czasu
- Aktualizacje w czasie rzeczywistym

### Szczegóły logu

Każdy wpis logu wyświetla:
- Znacznik czasu
- Poziom
- Źródło
- Wiadomość
- Ślad stosu (dla błędów)

---

## Raport audytu

### Użycie tokenów

- Całkowita liczba wykorzystanych tokenów
- Podział według modeli
- Kalkulacja kosztów
- Wykresy oparte na czasie

### Eksport raportu

Pobieranie danych audytu:
- Format CSV
- Wybór zakresu dat
- Filtrowanie według istoty lub modelu

---

## Edytor kodu

### Funkcje

- Podświetlanie składni (Monaco Editor)
- Uzupełnianie kodu
- Podpowiedzi dla identyfikatorów
- Kompilacja w czasie rzeczywistym

### Podpowiedzi

Najedź kursorem na dowolnym identyfikatorze, aby zobaczyć:
- Informacje o typie
- Dokumentację
- Lokalizację definicji
- Odwołania

---

## Przegląd historii czatu

### Funkcje

- Przeglądanie historii czatu Istot Krzemowych
- Wyświetlanie listy sesji
- Przeglądanie szczegółów wiadomości
- Widok osi czasu

### Korzystanie z historii czatu

1. Przejdź do strony **Istoty**
2. Kliknij link **Historia czatu** Istoty Krzemowej
3. Przeglądaj listę sesji:
   - Tytuł sesji
   - Czas utworzenia
   - Liczba wiadomości
4. Kliknij sesję, aby zobaczyć szczegóły:
   - Pełna historia wiadomości
   - Znaczniki czasu
   - Informacje o nadawcy
   - Rekordy wywołań narzędzi

### Implementacja techniczna

- **Kontroler**: `ChatHistoryController`
- **Model widoku**: `ChatHistoryViewModel`
- **Widoki**:
  - `ChatHistoryListView` — lista sesji
  - `ChatHistoryDetailView` — szczegóły wiadomości
- **Trasy API**:
  - `/api/chat-history/{beingId}/conversations` — pobranie listy sesji
  - `/api/chat-history/{beingId}/conversation/{conversationId}` — pobranie szczegółów wiadomości

---

## Przesyłanie plików

### Funkcje

- Okno dialogowe źródła pliku
- Obsługa wieloplikowego przesyłania
- Zarządzanie metadanymi pliku
- Wyświetlanie postępu przesyłania

### Korzystanie z przesyłania plików

1. W interfejsie czatu kliknij przycisk **Prześlij plik**
2. Otworzy się okno dialogowe źródła pliku
3. Wybierz źródło pliku:
   - Plik lokalny
   - Ścieżka systemu plików
4. Wybierz pliki (obsługa wielokrotnego wyboru)
5. Potwierdź przesyłanie
6. Informacje o pliku zostaną dołączone do wiadomości

### Obsługiwane typy plików

- Pliki tekstowe (.txt, .md, .json, .xml, itp.)
- Pliki kodu (.cs, .js, .py, .java, itp.)
- Pliki konfiguracyjne (.yml, .yaml, .ini, .conf, itp.)
- Pliki dokumentów (.csv, .log, itp.)

---

## Wskaźnik ładowania

### Funkcje

- Wyświetlanie statusu ładowania strony czatu
- Automatyczny wybór sesji Kuratora
- Informacja zwrotna o postępie ładowania danych

### Zachowanie

- Wyświetlanie animacji ładowania podczas ładowania strony
- Automatyczne ukrywanie po zakończeniu ładowania danych
- Automatyczne zaznaczenie sesji Kuratora (jeśli istnieje)
- Wielojęzyczne teksty wskazujące ładowanie

---

## System dokumentacji pomocy (nowość)

### Przegląd funkcji

System dokumentacji pomocy zapewnia wielojęzyczną obsługę dokumentacji pomocy dla Istot Krzemowych i użytkowników.

### Korzystanie z dokumentacji pomocy

1. Przejdź do strony **Pomoc**
2. Przeglądaj listę tematów pomocy:
   - Przewodnik szybkiego startu
   - Referencja korzystania z narzędzi
   - Przewodnik zarządzania uprawnieniami
   - Podręcznik rozwiązywania problemów
   - Przewodnik deweloperski
3. Kliknij temat, aby zobaczyć szczegółową treść:
   - Ustrukturyzowana treść dokumentacji (renderowanie Markdown)
   - Obsługa wielojęzyczna (zgodna z ustawieniami lokalizacji systemu)
   - Rekomendacje powiązanych tematów
4. Użyj funkcji wyszukiwania do szybkiej lokalizacji:
   - Wyszukiwanie po słowach kluczowych (obsługa chińskiego i angielskiego)
   - Wyniki wyszukiwania posortowane według trafności

### Dostęp Istot Krzemowych do pomocy

Istoty Krzemowe mogą uzyskać dostęp do dokumentacji pomocy przez narzędzie `help`:
```json
{
  "action": "get_topics"
}
```

### Implementacja techniczna

- **Kontroler**: `HelpController`
- **Narzędzie**: `HelpTool`
- **Trasy API**:
  - `/api/help` — pobranie listy tematów pomocy
  - `/api/help/{topicId}` — pobranie szczegółów tematu
  - `/api/help/search?q=keyword` — wyszukiwanie dokumentacji pomocy

---

## Obszar roboczy projektu (nowość)

### Przegląd funkcji

Obszar roboczy projektu zapewnia ustrukturyzowane środowisko pracy, obsługujące zarządzanie projektami, śledzenie zadań i notatki pracy.

### Zarządzanie projektami

1. **Tworzenie projektu**:
   - Nazwa i opis projektu
   - Tagi projektu (kategoryzacja)
   - Status projektu (w trakcie, ukończony, zarchiwizowany)
2. **Przegląd szczegółów projektu**:
   - Podstawowe informacje o projekcie
   - Lista powiązanych zadań
   - Lista notatek pracy
   - Statystyki postępu projektu
3. **Archiwizacja projektu**: zachowanie danych historycznych, ale projekt nie jest już aktywny
4. **Zarządzanie rolami projektowymi**:
   - Przypisywanie ról projektowych Istotom Krzemowym (np. developer, reviewer, manager)
   - Usuwanie przypisań ról
   - Przeglądanie listy członków i ról projektu
5. **Przepływ pracy projektu**:
   - Przeglądanie listy szablonów przepływu pracy
   - Powiązanie szablonu przepływu pracy z projektem
   - Przeglądanie statusu instancji przepływu pracy
   - Przeglądanie dzienników wykonania przepływu pracy

### Notatki pracy (prywatne)

Osobiste notatki pracy Istoty Krzemowej, podobne do dziennika:

1. **Tworzenie notatki**:
   - Podsumowanie (krótki opis)
   - Treść (obsługa formatowania Markdown)
   - Słowa kluczowe (do wyszukiwania)
   - Automatyczne rejestrowanie znacznika czasu
2. **Zarządzanie notatkami**:
   - Przeglądanie według osi czasu (projekt stronicowy)
   - Wyszukiwanie notatek (po słowach kluczowych, podsumowaniu, treści)
   - Generowanie spisu treści (szybki przegląd struktury notatek)
   - Aktualizacja i usuwanie notatek
3. **Kontrola uprawnień**:
   - Domyślnie prywatne, dostępne tylko dla samej istoty
   - Kurator Krzemowy może zarządzać wszystkimi notatkami

### Implementacja techniczna

- **Kontroler**: `WorkNoteController`
- **Narzędzia**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **Trasy API**:
  - `/api/worknotes` — pobranie listy notatek pracy
  - `/api/worknotes/{id}` — pobranie szczegółów notatki
  - `/api/worknotes/search?q=keyword` — wyszukiwanie notatek
  - `/api/worknotes/directory` — generowanie spisu treści notatek
  - `/api/projects` — API zarządzania projektami

---

## Projekt responsywny

Web UI dostosowuje się do różnych rozmiarów ekranu:
- Pulpit: pełny układ
- Tablet: zwinięty pasek boczny
- Mobilny: składane menu

---

## Skróty klawiszowe

| Skrót | Akcja |
|----------|--------|
| `Ctrl+K` | Szybkie wyszukiwanie |
| `Ctrl+B` | Przełączanie paska bocznego |
| `Ctrl+Enter` | Wysłanie wiadomości |
| `Esc` | Anuluj/Zamknij |

---

## Rozwiązywanie problemów

### Brak połączenia

**Sprawdź**:
- Serwer jest uruchomiony
- Port 8080 nie jest zablokowany
- Ustawienia zapory sieciowej

### SSE nie działa

**Sprawdź**:
- Przeglądarka obsługuje SSE
- Brak buforowania SSE przez proxy
- Stabilność sieci

### Niska wydajność

**Optymalizacja**:
- Zmniejsz szczegółowość logów
- Oczyść stare dane audytu
- Sprawdź zasoby systemowe

---

## Następne kroki

- 📚 Przeczytaj [przewodnik architektury](architecture.md)
- 🛠️ Zobacz [przewodnik deweloperski](development-guide.md)
- 📖 Przeglądaj [referencję API](api-reference.md)
- 🚀 Zobacz [przewodnik szybkiego startu](getting-started.md)
