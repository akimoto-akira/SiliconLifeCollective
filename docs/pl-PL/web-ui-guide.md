# Przewodnik Web UI

> **Wersja: v0.2.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md) | [Polski](../pl-PL/web-ui-guide.md)

## Przegląd

Web UI zapewnia kompleksowy interfejs do zarządzania Istotami Krzemowymi, monitorowania stanu systemu i interakcji z agentami AI. System wykorzystuje architekturę renderowania po stronie serwera, z zerowymi zależnościami od frameworków frontendowych, generując HTML, CSS i JavaScript poprzez kreatory `H`, `CssBuilder` i `JsBuilder`.

## Dostęp

Domyślny URL: `http://localhost:8080`

## Nawigacja

### Główne sekcje

1. **Pulpit nawigacyjny** - Przegląd systemu i wskaźniki
2. **Istoty** - Zarządzanie Istotami Krzemowymi
3. **Czat** - Interakcja z istotami (obsługa przesyłania plików, SSE w czasie rzeczywistym)
4. **Historia czatu** - Przeglądanie historii czatu Istot Krzemowych (lista sesji, szczegóły wiadomości)
5. **Zadania** - Zarządzanie zadaniami (zadania osobiste)
6. **Czasomierze** - Konfiguracja czasomierzy (tworzenie, wstrzymywanie, historia wykonania)
7. **Konfiguracja** - Ustawienia systemu (klienci AI, lokalizacja)
8. **Uprawnienia** - Kontrola dostępu (zarządzanie ACL, zapytania o uprawnienia)
9. **Logi** - Logi systemowe (filtrowanie według poziomu, zapytania o zakres czasowy)
10. **Audyt** - Użycie Tokenów i ślad audytu
11. **Pamięć** - Pamięć istot (widok osi czasu, zaawansowane filtrowanie)
12. **Wiedza** - Baza wiedzy (zarządzanie trójkami, odkrywanie ścieżek)
13. **Przeglądarka kodu** - Eksploracja kodu (drzewo plików, podświetlanie składni)
14. **Edytor kodu** - Edycja kodu z podpowiedziami przy najechaniu (Monaco Editor)
15. **Projekty** - Zarządzanie projektami (obszary robocze, zadania, notatki pracy)
16. **Wykonawcy** - Zarządzanie wykonawcami (dyskowy, sieciowy, wiersz poleceń)
17. **Pomoc** - System dokumentacji pomocy (obsługa wielojęzyczna, wyszukiwanie tematyczne)
18. **O systemie** - Informacje o systemie i wersja

---

## Pulpit nawigacyjny

### Funkcje

- Wskaźniki wydajności systemu (CPU, pamięć, czas działania)
- Przegląd stanu istot
- Statystyki użycia AI
- Szybkie akcje

### Aktualizacje w czasie rzeczywistym

Wykorzystanie SSE (zdarzeń wysyłanych przez serwer) do danych w czasie rzeczywistym:

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

Wyświetla wszystkie istoty, zawierające:
- Nazwę i ID
- Bieżący stan (uruchomiona/zatrzymana/błąd)
- Link do pliku duszy
- Szybkie akcje (uruchom/zatrzymaj/konfiguruj)

### Szczegóły istoty

- Pełna konfiguracja
- Edytor pliku duszy
- Historia zadań
- Przeglądarka pamięci
- Wskaźniki wydajności

### Tworzenie istoty

1. Kliknij **Utwórz nową istotę**
2. Wypełnij:
   - Nazwę
   - Treść duszy (edytor Markdown)
   - Konfigurację początkową
3. Kliknij **Utwórz**

---

## Interfejs czatu

### Funkcje

- Strumieniowanie wiadomości w czasie rzeczywistym
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
📤 Wyjście: "Księżycowy 4. miesiąc, 3. dzień"
```

---

## Konfiguracja

### Klienci AI

Konfiguracja backendów AI:
- Ollama (lokalne)
- Bailian (chmura)
- Volcengine Ark (chmura)
- Klienci niestandardowi

### Ustawienia przechowywania

- Wersja Default: ścieżka bazowa, indeks czasowy, zasady oczyszczania
- Wersja Fast: konfiguracja silnika przechowywania SpeedyPack, zarządzanie plikami .spk, ustawienia automatycznej kompresji

### Lokalizacja

Przełączanie między 29 wariantami językowymi:
- Chiński (6): uproszczony, tradycyjny Hongkong, tradycyjny Tajwan, tradycyjny Makau, singapurski, malezyjski
- Angielski (10): amerykański, brytyjski, kanadyjski, australijski, indyjski, singapurski, południowoafrykański, irlandzki, nowozelandzki, malezyjski
- Hiszpański (2): hiszpański, meksykański
- Niemiecki (5): niemiecki, austriacki, szwajcarski, luksemburski, liechtensteiński
- Francuski (3): francuski, kanadyjski, szwajcarski
- Japoński, koreański, czeski
- Polski

---

## System skórek

### Dostępne skórki

1. **Admin** - Profesjonalny interfejs zarządczy
2. **Chat** - Projekt skoncentrowany na rozmowie
3. **Creative** - Styl kreatywny i artystyczny
4. **Dev** - Układ zorientowany na deweloperów
5. **HighContrast** - Motyw wysokiego kontrastu (wersja Fast)
6. **Minimal** - Styl minimalistyczny (wersja Fast)
7. **Light** - Jasny motyw (wersja Fast)

### Przełączanie skórek

1. Kliknij **Ustawienia** (ikona zębatki)
2. Wybierz **Skórka**
3. Wybierz pożądaną skórkę
4. Interfejs aktualizuje się natychmiast

### Niestandardowa skórka

Utwórz niestandardową skórkę implementując `ISkin`:

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

### Przegląd uprawnień

- Lista wszystkich reguł uprawnień
- Filtrowanie według użytkownika lub zasobu
- Przegląd dat wygaśnięcia

### Dodawanie reguły uprawnień

1. Kliknij **Dodaj regułę**
2. Skonfiguruj:
   - Użytkownika
   - Zasób (np. `disk:read`)
   - Zezwól/Odmów
   - Czas trwania
3. Zapisz

### Ślad audytu

Przegląd wszystkich decyzji dotyczących uprawnień:
- Znacznik czasu
- Użytkownik
- Zasób
- Decyzja
- Przyczyna

---

## Zarządzanie zadaniami

### Lista zadań

- Wszystkie zadania wraz ze statusem
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
- Następny czas wykonania
- Status powtarzania

### Tworzenie czasomierza

1. Kliknij **Utwórz czasomierz**
2. Skonfiguruj:
   - Przypisanie istoty
   - Interwał lub wyrażenie cron
   - Akcję do wykonania
   - Ustawienia powtarzania
3. Uruchom

---

## Przeglądarka logów

### Funkcje

- Filtrowanie według poziomu (informacja/ostrzeżenie/błąd)
- Wyszukiwanie według słów kluczowych
- Wybór zakresu czasowego
- Aktualizacje w czasie rzeczywistym

### Szczegóły logów

Każdy wpis logu wyświetla:
- Znacznik czasu
- Poziom
- Źródło
- Wiadomość
- Ślad stosu (dla błędów)

---

## Raporty audytowe

### Użycie Tokenów

- Łączna liczba użytych tokenów
- Podział według modeli
- Kalkulacja kosztów
- Wykresy oparte na czasie

### Eksport raportów

Pobierz dane audytowe:
- Format CSV
- Wybór zakresu dat
- Filtrowanie według istoty lub modelu

---

## Edytor kodu

### Funkcje

- Podświetlanie składni (Monaco Editor)
- Uzupełnianie kodu
- Podpowiedzi przy najechaniu na identyfikatory
- Kompilacja w czasie rzeczywistym

### Podpowiedzi przy najechaniu

Najedź kursorem na dowolnym identyfikatorze, aby zobaczyć:
- Informacje o typie
- Dokumentację
- Lokalizację definicji
- Odwołania

---

## Przeglądanie historii czatu

### Funkcje

- Przeglądanie historii czatu Istot Krzemowych
- Wyświetlanie listy sesji
- Przeglądanie szczegółów wiadomości
- Widok osi czasu

### Korzystanie z historii czatu

1. Przejdź do strony **Istoty**
2. Kliknij link **Historia czatu** Istoty Krzemowej
3. Przejrzyj listę sesji:
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
  - `ChatHistoryListView` - Lista sesji
  - `ChatHistoryDetailView` - Szczegóły wiadomości
- **Trasy API**:
  - `/api/chat-history/{beingId}/conversations` - Pobierz listę sesji
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Pobierz szczegóły wiadomości

---

## Przesyłanie plików

### Funkcje

- Okno dialogowe źródła plików
- Obsługa przesyłania wielu plików
- Zarządzanie metadanymi plików
- Wyświetlanie postępu przesyłania

### Korzystanie z przesyłania plików

1. W interfejsie czatu kliknij przycisk **Prześlij plik**
2. Otworzy się okno dialogowe źródła plików
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
- Pliki dokumentowe (.csv, .log, itp.)

---

## Wskaźniki ładowania

### Funkcje

- Wyświetlanie stanu ładowania strony czatu
- Automatyczny wybór sesji kuratora
- Informacja zwrotna o postępie ładowania danych

### Zachowanie

- Animacja ładowania podczas ładowania strony
- Automatyczne ukrywanie po zakończeniu ładowania danych
- Automatyczne zaznaczenie sesji kuratora (jeśli istnieje)
- Wielojęzyczne teksty wskazówek ładowania

---

## System dokumentacji pomocy (nowość)

### Przegląd funkcji

System dokumentacji pomocy zapewnia wielojęzyczne wsparcie dokumentacji pomocy dla Istot Krzemowych i użytkowników.

### Korzystanie z dokumentacji pomocy

1. Przejdź do strony **Pomoc**
2. Przejrzyj listę tematów pomocy:
   - Przewodnik szybkiego startu
   - Referencja użycia narzędzi
   - Przewodnik zarządzania uprawnieniami
   - Podręcznik rozwiązywania problemów
   - Przewodnik rozwoju
3. Kliknij temat, aby zobaczyć szczegółową treść:
   - Ustrukturyzowana treść dokumentacji (renderowanie Markdown)
   - Obsługa wielu języków (zgodnie z ustawieniami lokalizacji systemu)
   - Rekomendacje powiązanych tematów
4. Użyj funkcji wyszukiwania do szybkiej lokalizacji:
   - Wyszukiwanie słów kluczowych (obsługa chińskiego, angielskiego)
   - Wyniki wyszukiwania posortowane według trafności

### Dostęp Istot Krzemowych do pomocy

Istoty Krzemowe mogą uzyskać dostęp do dokumentacji pomocy za pomocą narzędzia `help`:
```json
{
  "action": "get_topics"
}
```

### Implementacja techniczna

- **Kontroler**: `HelpController`
- **Narzędzie**: `HelpTool`
- **Trasy API**:
  - `/api/help` - Pobierz listę tematów pomocy
  - `/api/help/{topicId}` - Pobierz szczegóły tematu
  - `/api/help/search?q=keyword` - Wyszukaj dokumentację pomocy

---

## Obszar roboczy projektów (nowość)

### Przegląd funkcji

Obszar roboczy projektów zapewnia ustrukturyzowane środowisko pracy, obsługujące zarządzanie projektami, śledzenie zadań i notatki robocze.

### Zarządzanie projektami

1. **Tworzenie projektu**:
   - Nazwa i opis projektu
   - Tagi projektu (kategoryzacja)
   - Status projektu (w trakcie, ukończony, zarchiwizowany)
2. **Przegląd szczegółów projektu**:
   - Podstawowe informacje o projekcie
   - Lista powiązanych zadań
   - Lista notatek roboczych
   - Statystyki postępu projektu
3. **Archiwizacja projektu**: Zachowanie danych historycznych bez aktywnej aktywności

### Notatki robocze (prywatne)

Osobiste notatki robocze Istot Krzemowych, podobne do dziennika:

1. **Tworzenie notatki**:
   - Podsumowanie (krótki opis)
   - Treść (obsługa formatu Markdown)
   - Słowa kluczowe (do wyszukiwania)
   - Automatyczny znacznik czasu
2. **Zarządzanie notatkami**:
   - Przeglądanie według osi czasu (projekt stronicowy)
   - Wyszukiwanie notatek (według słów kluczowych, podsumowania, treści)
   - Generowanie katalogu (szybki przegląd struktury notatek)
   - Aktualizacja i usuwanie notatek
3. **Kontrola uprawnień**:
   - Domyślnie prywatne, dostępne tylko dla samej istoty
   - Kurator Krzemowy może zarządzać wszystkimi notatkami

### Implementacja techniczna

- **Kontroler**: `WorkNoteController`
- **Narzędzia**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **Trasy API**:
  - `/api/worknotes` - Pobierz listę notatek roboczych
  - `/api/worknotes/{id}` - Pobierz szczegóły notatki
  - `/api/worknotes/search?q=keyword` - Wyszukaj notatki
  - `/api/worknotes/directory` - Generuj katalog notatek
  - `/api/projects` - API zarządzania projektami

---

## Projekt responsywny

Web UI dostosowuje się do różnych rozmiarów ekranu:
- Pulpit: pełny układ
- Tablet: skompresowany pasek boczny
- Urządzenie mobilne: składane menu

---

## Skróty klawiszowe

| Skrót | Akcja |
|----------|--------|
| `Ctrl+K` | Szybkie wyszukiwanie |
| `Ctrl+B` | Przełącz pasek boczny |
| `Ctrl+Enter` | Wyślij wiadomość |
| `Esc` | Anuluj/Zamknij |

---

## Rozwiązywanie problemów

### Nie można połączyć

**Sprawdź**:
- Serwer jest uruchomiony
- Port 8080 nie jest zablokowany
- Ustawienia zapory sieciowej

### SSE nie działa

**Sprawdź**:
- Przeglądarka obsługuje SSE
- Brak buforowania proxy dla SSE
- Stabilność sieci

### Wolna wydajność

**Optymalizacja**:
- Zmniejsz poziom szczegółowości logów
- Oczyść stare dane audytowe
- Sprawdź zasoby systemowe

---

## Następne kroki

- 📚 Przeczytaj [Przewodnik architektury](architecture.md)
- 🛠️ Zobacz [Przewodnik rozwoju](development-guide.md)
- 📖 Przeglądaj [Referencję API](api-reference.md)
- 🚀 Zobacz [Przewodnik szybkiego startu](getting-started.md)
