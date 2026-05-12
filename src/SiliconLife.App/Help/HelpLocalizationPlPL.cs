// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

namespace SiliconLife.Help;

/// <summary>
/// Polish help documentation implementation
/// Implementacja pomocy w języku polskim
/// </summary>
public class HelpLocalizationPlPL : HelpLocalizationBase
{
  #region Help Documents

  public override string GettingStarted_Title => "Szybki start";
  public override string BeingManagement_Title => "Zarządzanie byciami";
  public override string ChatSystem_Title => "System czatu";
  public override string Dashboard_Title => "Panel";
  public override string Task_Title => "Zadania";
  public override string Timer_Title => "Czasomierze";
  public override string Permission_Title => "Zarządzanie uprawnieniami";
  public override string Config_Title => "Konfiguracja";
  public override string FAQ_Title => "Często zadawane pytania";
  public override string Memory_Title => "System pamięci";
  public override string OllamaSetup_Title => "Instalacja Ollama i pobieranie modeli";
  public override string BailianDashScope_Title => "Przewodnik po platformie Alibaba Cloud Bailian";
  public override string VolcengineArk_Title => "Przewodnik po platformie Volcengine Ark";
  public override string AIClients_Title => "Konfiguracja klienta AI";

  public override string BeingSoul_Title => "Plik Duszy";
  public override string AuditLog_Title => "Dziennik Audytów";
  public override string KnowledgeGraph_Title => "Graf Wiedzy";
  public override string WorkNotes_Title => "Notatki Pracy";
  public override string Projects_Title => "Zarządzanie Projektami";
  public override string Logging_Title => "System Logowania";

  public override string[] GettingStarted_Tags => new[]
  {
    "instalacja", "uruchomienie", "zaczynamy", "szybki start", "pierwsze użycie", "inicjalizacja", "proces",
    "konfiguracja środowiska"
  };

  public override string[] BeingManagement_Tags => new[]
    { "bycie", "utwórz", "konfiguracja", "zarządzanie byciami", "silicon being", "profil", "ustawienia", "zarządzanie" };

  public override string[] ChatSystem_Tags => new[]
    { "czat", "konwersacja", "wiadomość", "system czatu", "komunikacja", "dialog", "rozmowa", "dyskusja" };

  public override string[] Dashboard_Tags => new[]
    { "panel", "monitor", "statystyki", "stan", "system", "częstotliwość wiadomości", "czas działania", "pamięć" };

  public override string[] Task_Tags => new[]
    { "zadanie", "praca", "wykonanie", "priorytet", "zależność", "stan", "automatyzacja", "zarządzanie" };

  public override string[] Timer_Tags => new[]
    { "czasomierz", "planowanie", "wyzwalanie", "okres", "kalendarz", "przypomnienie", "automatyczny", "czasowanie" };

  public override string[] Permission_Tags => new[]
  {
    "uprawnienia", "bezpieczeństwo", "kontrola dostępu", "zarządzanie uprawnieniami", "autoryzacja", "ochrona prywatności", "ochrona",
    "weryfikacja"
  };

  public override string[] Config_Tags => new[]
  {
    "konfiguracja", "ustawienia", "opcje", "zarządzanie konfiguracją", "preferencje", "dostosowanie", "system", "parametry"
  };

  public override string[] FAQ_Tags => new[]
    { "częste pytania", "pomoc", "problemy", "wsparcie", "rozwiązywanie problemów", "przewodnik", "asysta", "odpowiedzi" };

  public override string[] Memory_Tags => new[]
    { "pamięć", "historia", "zapis", "system pamięci", "aktywność", "śledzenie", "wyszukiwanie", "dziennik" };

  public override string[] OllamaSetup_Tags => new[]
    { "Ollama", "instalacja", "model", "pobieranie", "lokalna AI", "konfiguracja", "ustawienia", "uruchomienie" };

  public override string[] BailianDashScope_Tags => new[]
    { "Bailian", "DashScope", "Alibaba Cloud", "chmurowa AI", "API", "konfiguracja", "model", "płatne" };

  public override string[] VolcengineArk_Tags => new[]
    { "Volcengine", "Ark", "Doubao", "ByteDance", "chmurowa AI", "API", "konfiguracja", "model" };

  public override string[] AIClients_Tags => new[]
    { "klient AI", "usługa AI", "model", "konfiguracja", "lokalna", "chmura", "Ollama", "DashScope", "ustawienia" };

  public override string[] BeingSoul_Tags => new[]
  {
    "plik duszy", "osobowość", "prompt", "rola", "zachowanie", "konfiguracja", "charakter", "przewodnik pracy",
    "prompt systemowy"
  };

  public override string[] AuditLog_Tags => new[]
    { "dziennik audytów", "Token", "statystyki użycia", "monitor", "zużycie", "analiza", "trend", "eksport", "CSV" };

  public override string[] KnowledgeGraph_Tags => new[]
    { "graf wiedzy", "wiedza", "wizualizacja", "trójka", "encja", "relacja", "sieć", "uczenie", "zarządzanie" };

  public override string[] WorkNotes_Tags => new[]
  {
    "notatki pracy", "notatki", "zapisy", "dziennik", "protokół", "Markdown", "słowa kluczowe", "wersja", "wyszukiwanie"
  };

  public override string[] Projects_Tags => new[]
    { "zarządzanie projektami", "projekt", "współpraca", "zadanie", "członek", "archiwum", "zespół", "przestrzeń pracy", "postęp" };

  public override string[] Logging_Tags => new[]
  {
    "system logowania", "log", "zapisy", "debug", "błąd", "ostrzeżenie", "monitor", "śledzenie", "konsola",
    "plik"
  };

  public override string GettingStarted => @"
# Szybki start

## Uruchamianie systemu

### Uruchomienie podwójnym kliknięciem (zalecane)

Znajdź plik programu i uruchom go podwójnym kliknięciem:
- **Windows**: `SiliconLife.Default.exe`
- System uruchomi się automatycznie i **automatycznie otworzy przeglądarkę**

To takie proste! Nie wymaga żadnej konfiguracji.

## Pierwsze użycie

Przy pierwszym uruchomieniu system **automatycznie ukończy całą inicjalizację**:
- ✅ Automatycznie tworzy Silicon Curatora
- ✅ Używa wbudowanego pliku duszy (promptu)
- ✅ Automatycznie zapisuje konfigurację
- ✅ Wszystkie usługi są automatycznie gotowe

Wystarczy poczekać, aż otworzy się przeglądarka, i możesz zacząć używać!

## Przegląd interfejsu

Interfejs systemu składa się z dwóch głównych części:

### Lewy panel nawigacji

Zawiera następujące moduły funkcjonalne:

- **💬 Czat** - Konwersacja z Silicon Being
- **📊 Panel** - Wyświetlanie stanu systemu
- **🧠 Silicon Being** - Wyświetlanie i zarządzanie Silicon Being
- **🔍 Audyt** - Wyświetlanie zapisów operacji
- **📚 Baza wiedzy** - Zarządzanie grafem wiedzy
- **📁 Projekt** - Zarządzanie projektami kodu
- **📝 Dziennik** - Wyświetlanie logów systemowych
- **⚙ Konfiguracja** - Ustawienia systemu
- **❓ Pomoc** - Ten dokument
- **ℹ O systemie** - Informacje o systemie

### Główny obszar treści

Wyświetla treść aktualnej strony, która zmienia się w zależności od wybranego modułu funkcjonalnego.

## Szybkie rozpoczęcie

### 1. Konwersacja z Silicon Being

To jest najczęściej używana funkcja:

1. Kliknij ikonę **💬 Czat** po lewej
2. Wybierz Silicon Being z lewej listy (domyślnie Silicon Curator)
3. Wpisz wiadomość w polu wejściowym na dole
4. Naciśnij `Enter`, aby wysłać
5. AI odpowie w czasie rzeczywistym

**Wskazówka:** 
- Naciśnij `Shift + Enter`, aby przejść do nowej linii
- Klikając przycisk ⏹ możesz zatrzymać odpowiedź AI

### 2. Wyświetlanie informacji o Silicon Being

Aby wyświetlić szczegółowe informacje o Silicon Being:

1. Kliknij ikonę **🧠 Silicon Being** po lewej
2. Kliknij dowolną kartę Silicon Being
3. Po prawej stronie wyświetlą się szczegółowe informacje:
   - Stan (bezczynny/uruchomiony)
   - Liczba czasomierzy i zadań
   - Linki do pamięci, uprawnień, historii czatu itp.

### 3. Edycja ustawień systemu

Jeśli potrzebujesz zmodyfikować konfigurację systemu:

1. Kliknij ikonę **⚙ Konfiguracja** po lewej
2. Znajdź element konfiguracji, który chcesz zmienić
3. Kliknij przycisk ""Edytuj""
4. Wprowadź nową wartość i zapisz

**Typowe ustawienia:**
- Zmiana języka interfejsu
- Zmiana motywu wyglądu
- Modyfikacja modelu AI
- Zmiana portu dostępu

## Przesyłanie plików

Pozwól AI przeanalizować zawartość plików:

1. W interfejsie czatu kliknij przycisk **📁**
2. Wprowadź pełną ścieżkę do pliku
   - Na przykład: `C:\Users\TwojeImie\Dokumenty\raport.pdf`
3. Kliknij ""Potwierdź przesłanie""
4. AI odczyta i przeanalizuje plik

**Obsługiwane typy plików:**
- Pliki tekstowe: .txt, .md, .json
- Pliki kodu: .cs, .js, .py
- Pliki konfiguracyjne: .yml, .yaml
- Inne: .csv, .log itp.

## Wyświetlanie historii konwersacji

Aby sprawdzić poprzednie konwersacje:

1. Przejdź na stronę **🧠 Silicon Being**
2. Kliknij Silicon Being, którego chcesz wyświetlić
3. Kliknij link ""Historia czatu""
4. Przeglądaj wszystkie historyczne sesje

## Uzyskanie pomocy

Jeśli napotkasz problemy:

- **Wyświetl pomoc**: Kliknij ikonę **❓ Pomoc** po lewej
- **Wyświetl logi**: Kliknij ikonę **📝 Dziennik** po lewej
- **Zrestartuj system**: Wiele problemów można rozwiązać restartem

## Następne kroki

Teraz, gdy znasz podstawowe operacje, możesz:

- 📖 Przeczytać inną dokumentację pomocy, aby uzyskać szczegóły o funkcjach
- 💬 Rozmawiać z Silicon Curatorem i pozwolić mu pomóc w zadaniach
- ⚙ Eksplorować opcje konfiguracji i dostosować system do swoich potrzeb

Miłego korzystania!
";

  public override string BeingManagement => @"
# Zarządzanie byciami

## Czym jest Silicon Being?

Silicon Being jest główną jednostką systemu. Każdy Silicon Being jest niezależnym agentem AI z:
- **Plikiem duszy**: Główny prompt określający wzorce zachowania, osobowość i zdolności
- **Systemem pamięci**: Przechowuje historię konwersacji i ważne informacje
- **Systemem zadań**: Wykonuje zaplanowane zadania i zautomatyzowane operacje
- **Zestawem narzędzi**: Różne narzędzia funkcjonalne, które może wywoływać

## Wyświetlanie Silicon Being

### Lista Silicon Being

Na stronie ""Silicon Being"" zobaczysz wszystkie Silicon Being wyświetlane jako karty:
- **Nazwa**: Wyświetlana nazwa Silicon Being
- **Stan**: Bezczynny (zielony) lub Uruchomiony (niebieski)
- **Typ**: Jeśli załadowano niestandardowy skompilowany kod, wyświetlana jest etykieta typu

### Wyświetlanie szczegółów Silicon Being

Kliknięcie dowolnej karty Silicon Being spowoduje wyświetlenie szczegółowych informacji po prawej:
- **ID**: Unikalny identyfikator Silicon Being
- **Stan**: Aktualny stan działania
- **Niestandardowa kompilacja**: Czy załadowano niestandardowy kod
- **Liczba czasomierzy**: Kliknij, aby zarządzać czasomierzami
- **Liczba zadań**: Kliknij, aby wyświetlić listę zadań
- **Pamięć**: Kliknij, aby wyświetlić system pamięci
- **Uprawnienia**: Kliknij, aby wyświetlić konfigurację uprawnień
- **Historia czatu**: Wyświetlanie zapisów historycznych konwersacji
- **Notatki pracy**: Wyświetlanie notatek pracy
- **Klient AI**: Kliknij, aby wyświetlić i edytować konfigurację AI
- **Plik duszy**: Kliknij, aby wyświetlić i edytować prompt

## Edycja Silicon Being

### Edycja pliku duszy

Plik duszy określa wzorce zachowania i zakres zdolności Silicon Being.

1. Na stronie szczegółów Silicon Being kliknij link ""Plik duszy""
2. Otworzy się edytor pliku duszy (obsługuje format Markdown)
3. Edytuj treść promptu
4. Zapisz zmiany

### Edycja konfiguracji AI

Każdemu Silicon Being możesz skonfigurować niezależną usługę AI:

1. Na stronie szczegółów Silicon Being kliknij link ""Klient AI""
2. Wybierz typ klienta AI (np. Ollama, OpenAI itp.)
3. Skonfiguruj parametry takie jak punkt końcowy API, model, klucz itp.
4. Po zapisaniu zmiany wchodzą w życie natychmiast

## Przewodnik pisania pliku duszy

### Podstawowa struktura

```markdown
# Ustawienia roli

Jesteś [opis roli], specjalizujący się w:
- Umiejętność 1
- Umiejętność 2
- Umiejętność 3

# Instrukcje zachowania

1. Instrukcja 1
2. Instrukcja 2
3. Instrukcja 3

# Przepływ pracy

Po otrzymaniu zadania:
1. Zrozumieć wymaganie
2. Przeanalizować plan
3. Wykonać operacje
4. Zwrócić wynik
```

### Zalecenia dotyczące pisania

1. **Jasno zdefiniuj rolę**: Jasno określ obowiązki i specjalizację Silicon Being
2. **Ustaw granice zachowania**: Wskaż, co można robić, a czego nie należy robić
3. **Zapewnij przepływ pracy**: Prowadzi Silicon Being, jak przetwarzać zadania
4. **Użyj formatu Markdown**: Obsługuje nagłówki, listy, bloki kodu itp.

### Przykład: Asystent programowania

```markdown
# Ustawienia roli

Jesteś profesjonalnym asystentem full-stack developmentu, specjalizującym się w:
- Rozwoju w C# / .NET
- Projektowaniu architektury i przeglądzie kodu
- Projektowaniu i optymalizacji baz danych
- Rozwoju frontendu webowego

# Instrukcje zachowania

1. Zawsze dostarczaj wykonalne przykłady kodu
2. Wyjaśniaj kluczową logikę kodu i projekt
3. Dostarczaj rekomendacje najlepszych praktyk
4. Jeśli nie jesteś pewien, jasno to zakomunikuj użytkownikowi

# Standardy kodu

- Przestrzegaj zasad SOLID
- Używaj jasnych nazw
- Dodawaj niezbędne komentarze
- Rozważ obsługę błędów i przypadki brzegowe
```

## Stan Silicon Being

### Stan działania

- **Bezczynny**: Oczekuje na zadania lub konwersacje (zielony wskaźnik)
- **Uruchomiony**: Właśnie wykonuje zadanie lub konwersację (niebieski wskaźnik)

### Monitorowanie Silicon Being

Na panelu możesz wyświetlić:
- Całkowitą liczbę Silicon Being
- Stan wykonywania zadań dla poszczególnych Silicon Being
- Statystyki wykorzystania zasobów

## Najlepsze praktyki

1. **Rozdział obowiązków**: Różne Silicon Being są odpowiedzialne za różne obszary (np. asystent programowania, asystent obsługi klienta, analiza danych itp.)
2. **Ciągła optymalizacja**: Nieustannie optymalizuj plik duszy na podstawie rzeczywistych opinii z użytkowania i poprawiaj wydajność Silicon Being
3. **Kopia zapasowa konfiguracji**: Zaleca się tworzenie kopii zapasowych plików duszy ważnych Silicon Being

## Rozwiązywanie problemów

### P: Silicon Being nie reaguje?

Sprawdź:
1. Czy usługa AI działa prawidłowo
2. Czy połączenie sieciowe działa
3. Czy plik duszy jest prawidłowo skonfigurowany
4. Sprawdź logi systemowe dla szczegółowych komunikatów błędów

### P: Jak zmienić model AI dla Silicon Being?

Na stronie szczegółów Silicon Being kliknij link ""Klient AI"", wybierz nowy model AI i skonfiguruj go. Po zapisaniu zmiany wchodzą w życie natychmiast. Nowe konwersacje będą używać nowego modelu.

### P: Zachowanie Silicon Being nie odpowiada oczekiwaniom?

1. Sprawdź, czy plik duszy jest jasny i zrozumiały
2. Dodaj więcej instrukcji zachowania i ograniczeń
3. Zapewnij konkretne instrukcje przepływu pracy
4. Testuj i stale optymalizuj
";

  public override string ChatSystem => @"
# System czatu

## Rozpoczęcie konwersacji

1. Kliknij ikonę **💬 Czat** w lewym panelu nawigacji
2. Wybierz Silicon Being z lewej listy, z którym chcesz rozmawiać
3. Wpisz wiadomość w polu wejściowym na dole
4. Naciśnij klawisz `Enter` lub kliknij przycisk ""Wyślij""
5. AI odpowie w czasie rzeczywistym (tekst wyświetla się znak po znaku)

## Opis interfejsu

### Układ interfejsu

- **Lewa lista**: Wyświetla wszystkie Silicon Being, kliknięcie przełącza partnera konwersacji
- **Środkowy obszar**: Wyświetla wiadomości konwersacji
  - Twoje wiadomości wyświetlają się po prawej
  - Odpowiedzi AI wyświetlają się po lewej
- **Obszar wejściowy na dole**: Wprowadzanie wiadomości i przycisk wysyłania

### Opis przycisków

- **Przycisk Wyślij**: Wysyła wiadomość, którą wprowadziłeś
- **Przycisk ⏹ Zatrzymaj**: Wyświetla się podczas odpowiedzi AI, kliknięcie przerywa odpowiedź AI
- **Przycisk 📁 Plik**: Prześlij plik do analizy przez AI

## Podstawowe operacje

### Wysyłanie wiadomości

- Po wpisaniu wiadomości naciśnij `Enter`, aby wysłać
- Naciśnij `Shift + Enter`, aby przejść do nowej linii

### Zatrzymywanie odpowiedzi

Jeśli AI właśnie odpowiada, możesz:
- Kliknąć przycisk ""⏹ Zatrzymaj""
- Lub wysłać nową wiadomość (automatycznie przerywa aktualną odpowiedź)

### Przesyłanie plików

Pozwól AI przeanalizować zawartość plików:

1. Kliknij przycisk **📁** obok pola wejściowego
2. W wyskakującym panelu wprowadź ścieżkę do pliku
   - Na przykład: `C:\Users\TwojeImie\Dokumenty\raport.pdf`
3. Kliknij ""Potwierdź przesłanie""
4. AI odczyta i przeanalizuje plik

**Obsługiwane typy plików**:
- Pliki tekstowe: .txt, .md, .json, .xml
- Pliki kodu: .cs, .js, .py, .java itp.
- Pliki konfiguracyjne: .yml, .yaml, .ini, .conf
- Inne pliki: .csv, .log itp.

## Funkcje konwersacji

### Wyświetlanie w czasie rzeczywistym

Odpowiedzi AI wyświetlają się znak po znaku, nie musisz czekać na pełną odpowiedź, aby zobaczyć treść.

### Wielorundowa konwersacja

- System automatycznie zapisuje historię konwersacji
- AI pamięta, co było powiedziane wcześniej
- Możesz bezpośrednio odwoływać się do poprzednich konwersacji

### Wywoływanie narzędzi

AI może podczas konwersacji automatycznie wywoływać narzędzia do:
- Odpytywania kalendarza
- Zarządzania konfiguracją systemu
- Uruchamiania kodu
- Odczytywania plików
- Wyszukiwania pomocy
- Tworzenia notatek
- Odpytywania pamięci

Gdy AI wywołuje narzędzia, zobaczysz nazwę narzędzia i wynik wykonania.

### Wielojęzyczna konwersacja

Możesz rozmawiać z AI w dowolnym języku, AI automatycznie odpowie w tym samym języku.

## Wyświetlanie historii konwersacji

Jeśli chcesz wyświetlić poprzednie zapisy konwersacji:

1. Kliknij ikonę **🧠 Silicon Being** w lewym panelu nawigacji
2. Kliknij kartę Silicon Being, którego chcesz wyświetlić
3. W szczegółach po prawej znajdź link ""Historia czatu""
4. Kliknij, aby wyświetlić wszystkie historyczne sesje

## Często zadawane pytania

### P: Co zrobić, gdy AI odpowiada wolno?

**Możliwe przyczyny**:
- Używany model jest duży, wymaga więcej czasu obliczeniowego
- Opóźnienie sieciowe (przy używaniu modeli chmurowych)
- Historia konwersacji jest bardzo długa

**Rozwiązanie**:
- Spróbuj użyć modeli lokalnych (jak Ollama)
- Wybierz lżejszy model

### P: AI nie wywołuje narzędzi?

**Sprawdź następujące**:
1. Czy narzędzie jest włączone?
2. Czy istnieją ograniczenia uprawnień?
3. Czy model AI obsługuje wywoływanie narzędzi?

### P: Jak przesłać plik?

Kliknij przycisk ""📁"" obok pola wejściowego, wprowadź pełną ścieżkę do pliku (np. `C:\Dokumenty\plik.pdf`) i kliknij ""Potwierdź przesłanie"".

### P: Jak wyświetlić poprzednie konwersacje?

Na stronie ""Silicon Being"" kliknij link ""Historia czatu"" odpowiedniego Silicon Being, gdzie możesz wyświetlić wszystkie historyczne sesje.

## Zalecenia dotyczące użytkowania

1. **Wyrażaj się jasno**: Opisuj swoje potrzeby jasnym językiem
2. **Pytaj stopniowo**: Podziel złożone pytania na kilka mniejszych
3. **Zapewnij kontekst**: W razie potrzeby podaj odpowiednie informacje tła
4. **Wykorzystaj przesyłanie plików**: Gdy potrzebujesz, aby AI przeanalizowało pliki, bezpośrednio podaj ścieżkę do pliku
5. **Śledź wywołania narzędzi**: Zwracaj uwagę na narzędzia, które AI wywołuje, i upewnij się, że operacje odpowiadają oczekiwaniom
";

  public override string Dashboard => @"
# Panel

## Przegląd

Panel to Twoje centrum monitorowania systemu, które pozwala na pierwszy rzut oka zrozumieć stan wykonywania silicon beings. Dzięki intuicyjnym kartom statystycznym i wykresom możesz w dowolnym momencie śledzić ogólną sytuację systemu.

## Główne funkcje

- **Monitorowanie w czasie rzeczywistym**: Wyświetla liczbę silicon beings, aktywny stan i wykorzystanie zasobów systemowych
- **Statystyki wiadomości**: Wyświetla częstotliwość ostatnich wiadomości czatowych w formie wykresu
- **Automatyczna aktualizacja**: Dane aktualizują się automatycznie, nie wymaga ręcznej operacji

## Opis interfejsu

### Karty statystyczne

W górnej części strony wyświetlają się 4 karty statystyczne pokazujące kluczowe informacje systemowe:

| Karta | Opis |
|------|------|
| 🧠 Łącznie Silicon Beings | Całkowita liczba wszystkich silicon beings, które utworzyłeś |
| ⚡ Aktywne Silicon Beings | Liczba silicon beings, które aktualnie pracują lub przetwarzają zadania |
| ⏱️ Czas działania systemu | Czas, przez który program działa od uruchomienia |
| 💾 Wykorzystanie pamięci | Rozmiar pamięci komputera, którego program aktualnie używa |

### Wykres częstotliwości wiadomości

Pod kartami statystycznymi wyświetla się wykres słupkowy pokazujący liczbę wiadomości w każdym punkcie czasowym w ciągu ostatnich 20 minut:

### P: Wykres wyświetla się pusty lub bez danych?

**O:** Możliwe powody:
1. System został właśnie uruchomiony, nie ma jeszcze zapisów interakcji wiadomości
2. W ciągu ostatnich 20 minut nie wygenerowano żadnych wiadomości
3. Usługa ChatSystem nie została prawidłowo zainicjalizowana

### P: Czas działania wyświetla się nieprawidłowo?

**O:** Czas działania jest liczony od uruchomienia aplikacji, jeśli wyświetla się nienormalnie:
1. Sprawdź, czy czas systemowy jest prawidłowy
2. Zrestartuj aplikację, aby zresetować pomiar czasu

### P: Jak ręcznie zaktualizować dane?

**O:** Aktualna wersja aktualizuje się automatycznie, jeśli potrzebujesz ręcznej aktualizacji:
- Naciśnij F5, aby odświeżyć całą stronę
- Lub naciśnij Ctrl+F5, aby wymusić odświeżenie (czyszczenie pamięci podręcznej)

## Zalecenia dotyczące użytkowania

### Codzienne monitorowanie

1. **Regularnie sprawdzaj**: Zaleca się otwieranie codziennie, aby zrozumieć stan wykonywania systemu
2. **Śledź trendy**: Za pomocą wykresu częstotliwości wiadomości zrozum, o jakiej porze używasz najczęściej
3. **Monitoruj pamięć**: Jeśli wykorzystanie pamięci przekroczy 500 MB, możesz rozważyć restart programu
4. **Rozumiej aktywność**: Na podstawie liczby aktywnych beings oceniaj, czy system działa normalnie

### Zalecenia dotyczące optymalizacji

1. **Kontroluj liczbę beings**: Utworzenie zbyt wielu beings zajmie więcej pamięci
2. **Czyść stare konwersacje**: Regularnie czyń niepotrzebną historię konwersacji, aby zwolnić miejsce
3. **Łącz z narzędziami systemowymi**: Możesz monitorować zasoby systemowe razem z menedżerem zadań komputera

### Co robić w przypadku problemów?

Jeśli panel nie wyświetla się prawidłowo, możesz spróbować następujących kroków:

1. **Sprawdź przeglądarkę**: Otwórz konsolę przeglądarki (naciśnij F12), sprawdź, czy nie ma komunikatów błędów
2. **Sprawdź połączenie sieciowe**: Potwierdź, że program działa i sieć jest normalna
3. **Sprawdź logi programu**: Sprawdź, czy program nie ma nienormalnych logów
4. **Zrestartuj program**: Jeśli żadna z powyższych metod nie działa, spróbuj zamknąć i ponownie otworzyć program
";

  public override string Task => @"
# Zadania

## Przegląd

System zadań i czasomierzy rejestruje zautomatyzowane wykonywanie Silicon Being. Możesz wyświetlić listę zadań i stan czasomierzy i dowiedzieć się, co Silicon Being robi i kiedy wykonuje operacje.

## System zadań

### Czym są zadania?

Zadania to elementy pracy, które Silicon Being właśnie wykonuje lub ukończył, na przykład:
- Przetwarzanie zadań automatycznie utworzonych przez AI
- Elementy pracy generowane przez system
- Wykonywanie zadań uruchomionych przez czasomierze

### Wyświetlanie listy zadań

**Metoda 1: Wyświetlanie wszystkich zadań**

1. Kliknij ikonę ""Zadania"" w lewym panelu nawigacji (jeśli jest dostępna)
2. Strona wyświetli listę zadań wszystkich Silicon Being

**Metoda 2: Wyświetlanie zadań konkretnego Silicon Being**

1. Przejdź na stronę **🧠 Silicon Being**
2. Kliknij Silicon Being, którego chcesz wyświetlić
3. W szczegółach znajdź link ""Zadania""
4. Kliknij, aby wejść na stronę zadań

### Informacje o zadaniach

Każde zadanie wyświetla następujące informacje:

- **Nazwa zadania**: Nazwa zadania
- **Stan**:
  - Oczekujące (żółty)
  - Uruchomione (niebieski)
  - Ukończone (zielony)
  - Nieudane (czerwony)
  - Anulowane (szary)
- **Priorytet**: Poziom priorytetu zadania
- **Przypisane**: Silicon Being wykonujący to zadanie
- **Czas utworzenia**: Czas utworzenia zadania
- **Opis**: Szczegółowy opis zadania

### Opis stanów zadań

- **Oczekujące**: Zadanie zostało utworzone i czeka na wykonanie
- **Uruchomione**: Zadanie jest właśnie wykonywane
- **Ukończone**: Zadanie zostało pomyślnie ukończone
- **Nieudane**: Zadanie nie powiodło się, możesz wyświetlić komunikaty błędów
- **Anulowane**: Zadanie zostało anulowane

## System czasomierzy

### Czym są czasomierze?

Czasomierze to mechanizmy automatycznego wyzwalania, które pozwalają Silicon Being wykonywać operacje w wyznaczonym czasie. System używa systemu kalendarza do definiowania warunków wyzwalania.

### Wyświetlanie listy czasomierzy

**Metoda 1: Wyświetlanie wszystkich czasomierzy**

1. Kliknij ikonę ""Czasomierze"" w lewym panelu nawigacji (jeśli jest dostępna)
2. Strona wyświetli listę czasomierzy wszystkich Silicon Being

**Metoda 2: Wyświetlanie czasomierzy konkretnego Silicon Being**

1. Przejdź na stronę **🧠 Silicon Being**
2. Kliknij Silicon Being, którego chcesz wyświetlić
3. W szczegółach znajdź link ""Czasomierze""
4. Kliknij, aby wejść na stronę czasomierzy

### Informacje o czasomierzach

Każdy czasomierz wyświetla następujące informacje:

- **Nazwa czasomierza**: Identyfikator czasomierza
- **Stan**: Uruchomiony lub zatrzymany
- **Typ**: Typ wyzwalania czasomierza
- **Czas wyzwolenia**: Następny czas wyzwolenia
- **System kalendarza**: Używany kalendarz (np. gregoriański, księżycowy itp.)
- **Liczba wyzwoleń**: Całkowita liczba wyzwoleń
- **Czas utworzenia**: Czas utworzenia czasomierza
- **Ostatnie wyzwolenie**: Czas ostatniego wyzwolenia

### Typy czasomierzy

System obsługuje różne sposoby wyzwalania:

- **Wyzwalanie interwałowe**: Wyzwala się co X jednostek czasu
  - Na przykład: co 2 godziny, co 30 minut
  
- **Wyzwalanie kalendarzowe**: Wyzwala się zgodnie z warunkami kalendarza
  - Na przykład: codziennie o 9:00, każdy poniedziałek, 1. każdego miesiąca
  - Obsługuje gregoriański, księżycowy i inne systemy kalendarza

## Wyświetlanie historii wykonywania

### Historia wykonywania czasomierzy

Dowiedz się o stanie wykonywania czasomierzy:

1. Przejdź na stronę czasomierzy
2. Znajdź czasomierz, który chcesz wyświetlić
3. Kliknij link ""Historia wykonywania""
4. Wyświetl wszystkie zapisy wyzwoleń

### Szczegóły wykonywania

Szczegółowe informacje o każdym wykonaniu:

1. W historii wykonywania znajdź konkretne wykonanie
2. Kliknij, aby wyświetlić szczegóły
3. Możesz zobaczyć:
   - Czas wykonania
   - Wynik wykonania
   - Powiązane wiadomości konwersacji
   - Komunikaty błędów (jeśli nie powiodło się)

### Raporty z wykonywania

Wyświetl pełną konwersację podczas konkretnego wykonania:

1. Na stronie szczegółów wykonania znajdź link ""Wiadomości""
2. Wyświetl pełną konwersację między AI a użytkownikiem
3. Dowiedz się, jak AI przetworzyło to wyzwolenie

## Często zadawane pytania

### P: Jak utworzyć nowe zadanie?

**O:** Zadania są automatycznie generowane przez system, ręczne tworzenie nie jest obsługiwane. Gdy Silicon Being potrzebuje wykonać określoną pracę, automatycznie tworzy zadanie.

### P: Jak utworzyć nowy czasomierz?

**O:** Czasomierze są automatycznie zarządzane przez Silicon Being, ręczne tworzenie nie jest obsługiwane. Silicon Being ustawia czasomierze według potrzeb do wykonywania regularnych zadań.

### P: Czy mogę usunąć zadania lub czasomierze?

**O:** System nie zapewnia funkcji ręcznego usuwania. Zadania i czasomierze są automatycznie zarządzane przez Silicon Being.

### P: Co zrobić, gdy zadanie wyświetla ""Nieudane""?

**Zalecenia:**
1. Sprawdź komunikaty błędów zadania
2. Dowiedz się przyczyny niepowodzenia
3. Jeśli to tymczasowy problem, zadanie może zostać powtórzone
4. Jeśli niepowodzenie się utrzymuje, porozmawiaj z Silicon Being, aby dowiedzieć się o sytuacji

### P: Czasomierz się nie wyzwolił?

**Sprawdź:**
1. Czy czasomierz jest w stanie uruchomienia?
2. Czy warunki wyzwalania są spełnione?
3. Czy Silicon Being działa normalnie?
4. Sprawdź historię wykonywania, aby dowiedzieć się o sytuacji

### P: Jak dowiedzieć się, co Silicon Being właśnie robi?

**Metody:**
1. Wyświetl listę zadań i dowiedz się o aktualnie wykonywanych zadaniach
2. Wyświetl listę czasomierzy i dowiedz się o nadchodzących operacjach
3. Wyświetl historię wykonywania i dowiedz się o przeszłych aktywnościach
4. Bezpośrednio porozmawiaj z Silicon Being i zapytaj

### P: Co oznacza priorytet zadania?

**O:** Priorytet oznacza ważność zadania. Im mniejsza liczba, tym wyższy priorytet. Zadania o wysokim priorytecie będą przetwarzane priorytetowo.

## Zalecenia dotyczące użytkowania

1. **Regularnie sprawdzaj**: Dowiedz się o stanie zautomatyzowanego wykonywania Silicon Being
2. **Śledź niepowodzenia zadań**: Terminowo obsługuj nienormalne sytuacje
3. **Wyświetlaj historię wykonywania**: Dowiedz się o wzorcach pracy AI
4. **Łącz z konwersacją**: Dyskutuj z Silicon Being o stanie zadań i czasomierzy

## Informacje techniczne

### Przechowywanie danych

Dane zadań i czasomierzy są przechowywane w systemowym katalogu danych i powiązane z Silicon Being:
```
data/
  beings/
    {ID Silicon Being}/
      tasks/      (Dane zadań)
      timers/     (Dane czasomierzy)
```

### Automatyczne zarządzanie

System automatycznie:
- Tworzy i zarządza zadaniami
- Wyzwala czasomierze
- Rejestruje historię wykonywania
- Czyści wygasłe dane

Nie musisz niczego ręcznie zarządzać, system zajmie się wszystkim.
";

  public override string Timer => @"
# Czasomierze

## Czym są czasomierze?

Czasomierze to automatyczny mechanizm przypomnień bytów krzemowych. Gdy nadejdzie ustawiony czas, byt krzemowy automatycznie wykona odpowiednie zadanie.

## Dwa typy czasomierzy

### Jednorazowy czasomierz

Czasomierz, który wyzwala się tylko raz. Po wyzwoleniu automatycznie kończy działanie.

**Odpowiednie scenariusze:**
- Przypomnienie, aby coś zrobić w określonym czasie
- Wykonanie jednorazowego zadania w określonej dacie

### Okresowy czasomierz

Czasomierz, który będzie wyzwalał się wielokrotnie. Po każdym wyzwoleniu system automatycznie oblicza czas następnego wyzwolenia.

**Odpowiednie scenariusze:**
- Zadania wyzwalane codziennie o ustalonej godzinie (np. poranny raport codziennie o 9:00)
- Zadania wyzwalane regularnie co tydzień lub miesiąc
- Zadania wyzwalane regularnie według księżycowych świąt (np. każdy księżycowy Nowy Rok)

## Stany czasomierzy

Czasomierze mają cztery stany:

| Stan | Opis |
|------|------|
| **Uruchomiony** | Czasomierz działa normalnie i czeka na wyzwolenie |
| **Wstrzymany** | Czasomierz jest tymczasowo zatrzymany i nie będzie się wyzwalał |
| **Wyzwolony** | Jednorazowy czasomierz ukończył wyzwolenie |
| **Anulowany** | Czasomierz został anulowany i nie wyzwoli się ponownie |

## Jak wyświetlić czasomierze?

### Wyświetlanie czasomierzy konkretnego bytu krzemowego

1. Przejdź na stronę **🧠 Byt krzemowy**
2. Wybierz byt krzemowy, który chcesz wyświetlić
3. Znajdź opcję ""Czasomierze"" i możesz wyświetlić wszystkie czasomierze tego bytu

## Informacje wyświetlane przy czasomierzach

Podczas wyświetlania czasomierzy możesz zobaczyć następujące informacje:

| Element | Opis |
|------|------|
| **Nazwa** | Nazwa czasomierza |
| **Opis** | Szczegółowy opis czasomierza (jeśli istnieje) |
| **Stan** | Aktualny stan (Uruchomiony, Wstrzymany itp.) |
| **Typ** | Jednorazowy lub okresowy |
| **Następny czas wyzwolenia** | Konkretny czas, kiedy czasomierz wyzwoli się następnym razem |
| **System kalendarza** | Używany kalendarz (np. gregoriański, księżycowy itp.) |
| **Liczba wyzwoleń** | Ile razy czasomierz już się wyzwolił |
| **Czas utworzenia** | Kiedy czasomierz został utworzony |
| **Ostatni czas wyzwolenia** | Czas ostatniego wyzwolenia (jeśli już był wyzwalany) |

## Obsługiwane systemy kalendarza

Czasomierze obsługują różne systemy kalendarza, w tym:

- **Kalendarz gregoriański** (Gregorian): Międzynarodowo używany kalendarz słoneczny
- **Kalendarz księżycowy** (Chinese Lunar): Tradycyjny chiński kalendarz księżycowy
- **Inne systemy kalendarza**: Kalendarz islamski, kalendarz Niebiańskich Pni itp.
- **Kalendarz interwałowy** (Interval): Wyzwalanie w stałych odstępach czasu (np. co 2 godziny)

## Historia wyzwoleń czasomierzy

Przy każdym wyzwoleniu czasomierza system rejestruje szczegółowe informacje o wykonaniu.

### Wyświetlanie historii wyzwoleń

1. Wybierz konkretny czasomierz na liście czasomierzy
2. Wyświetl historię wykonywania tego czasomierza
3. Możesz zobaczyć szczegóły każdego wyzwolenia:
   - Czas wyzwolenia
   - Stan wykonania (sukces, niepowodzenie itp.)
   - Wiadomości z konwersacji podczas wykonywania (jeśli istnieją)
   - Informacje o błędzie (jeśli wykonanie nie powiodło się)

### Stany historii wykonywania

Każde wykonanie ma następujące stany:

| Stan | Opis |
|------|------|
| **Niewyzwolone** | Wykonanie jeszcze się nie rozpoczęło |
| **Rozpoczęte** | Powiadomienie o rozpoczęciu zostało wysłane |
| **W trakcie** | Zadanie jest wykonywane |
| **Ukończone** | Zadanie zostało pomyślnie ukończone |
| **Nie powiodło się** | Wykonanie zadania nie powiodło się |

## Często zadawane pytania

### P: Jak utworzyć nowy czasomierz?

**O:** Czasomierze są automatycznie zarządzane przez byty krzemowe. Byty krzemowe tworzą czasomierze według potrzeb do wykonywania regularnych zadań.

### P: Czy mogę usunąć lub wstrzymać czasomierze?

**O:** Tak. System obsługuje następujące operacje:
- **Wstrzymać czasomierz**: Tymczasowo zatrzymać wyzwalanie czasomierza, można wznowić w razie potrzeby
- **Wznowić czasomierz**: Przywrócić wstrzymany czasomierz do stanu uruchomionego. Jeśli podczas wstrzymania nastąpiło pominięcie czasu wyzwolenia, system automatycznie obliczy czas następnego wyzwolenia
- **Anulować czasomierz**: Trwale anulować czasomierz, nie wyzwoli się ponownie
- **Usunąć czasomierz**: Całkowicie usunąć czasomierz z systemu

### P: Co zrobić, gdy czasomierz się nie wyzwala?

**Sprawdź następujące:**
1. Czy czasomierz jest w stanie **Uruchomiony** (nie Wstrzymany lub Anulowany)
2. Czy nadszedł czas wyzwolenia (sprawdź ""Następny czas wyzwolenia"")
3. Czy byt krzemowy działa normalnie

### P: Jak dowiedzieć się, jak działają czasomierze?

**Metody:**
1. Wyświetl listę czasomierzy i dowiedz się o nadchodzących operacjach
2. Wyświetl historię wykonywania i zrozum przeszłe aktywności
3. Zapytaj bezpośrednio bytu krzemowego w konwersacji

### P: Jak okresowy czasomierz oblicza czas następnego wyzwolenia?

**O:** Po każdym wyzwoleniu okresowego czasomierza system automatycznie oblicza czas następnego wyzwolenia zgodnie z systemem kalendarza i ustawionymi warunkami. Na przykład:
- Jeśli ustawiono gregoriański ""codziennie o 9:00"", system wyzwoli się codziennie o 9:00, a następnie obliczy 9:00 następnego dnia jako czas następnego wyzwolenia
- Jeśli ustawiono księżycowy ""każdego roku pierwszego dnia pierwszego miesiąca"", system wyzwoli się podczas księżycowego Nowego Roku, a następnie obliczy datę następnego księżycowego Nowego Roku

## Zalecenia dotyczące użytkowania

### Codzienne monitorowanie

1. **Regularnie sprawdzaj**: Zrozum automatyczne wykonywanie bytów krzemowych
2. **Śledź historię wykonywania**: Zrozum tryb pracy AI
3. **Kontroluj anomalie**: Sprawdź, czy istnieją zapisy o niepowodzeniach wykonywania

### Zalecenia dotyczące optymalizacji

1. **Łącz z konwersacją**: Dyskutuj o stanie czasomierzy z bytem krzemowym
2. **Śledź wyniki wykonywania**: Zrozum efekt czasomierzy poprzez historię wykonywania
3. **Dostosuj strategie**: W razie potrzeby pozwól bytowi krzemowemu dostosować ustawienia czasomierzy poprzez konwersację
";

  public override string Permission => @"
# Zarządzanie uprawnieniami

## Czym jest system uprawnień?

System uprawnień chroni bezpieczeństwo Twojego systemu i zapobiega wykonywaniu nieautoryzowanych operacji przez AI. Gdy AI próbuje wykonać określone operacje (takie jak dostęp do plików, uruchamianie poleceń itp.), system sprawdza, czy jest to dozwolone.

## Jak działają uprawnienia?

### Automatyczne wyskakujące okno uprawnień

Gdy AI próbuje wykonać operację wymagającą uprawnień, system wyświetla wyskakujące okno i pyta Cię:

**Zawartość wyskakującego okna obejmuje:**
- Typ uprawnień (np. dostęp do plików, uruchamianie poleceń itp.)
- Żądany zasób (np. ścieżka do pliku)
- Szczegółowe informacje

**Możesz wybrać:**
- **Zezwól**: Wykonaj tę operację
- **Odmów**: Zablokuj tę operację

### Kolejność weryfikacji uprawnień

System sprawdza uprawnienia w następującej kolejności:

1. **Silicon Curator**: Jeśli operuje Curator, automatycznie zezwala
2. **Ograniczenie częstotliwości**: Zapobiega dużej liczbie żądań w krótkim czasie
3. **Reguły globalne**: Wstępnie ustawione reguły zezwolenia/odmowy
4. **Reguły niestandardowe**: Reguły uprawnień, które napisałeś (jeśli istnieją)
5. **Zapytanie użytkownika**: Jeśli powyższe nie mogą zdecydować, wyświetla się wyskakujące okno i pyta Cię

## Wbudowane reguły uprawnień

System ma wstępnie ustawione niektóre bezpieczne reguły uprawnień:

### Reguły dostępu do plików

**Dostęp dozwolony:**
- Własny katalog tymczasowy Silicon Being
- Typowe foldery użytkownika (Pulpit, Pobrane, Dokumenty, Obrazy, Muzyka, Wideo)
- Foldery publiczne użytkowników

**Dostęp zabroniony:**
- Kluczowe katalogi systemowe (foldery systemowe Windows, /etc /boot itp. w Linuxie)
- Katalogi danych innych Silicon Being

**Nieprzypisane ścieżki:**
- Wyświetla się wyskakujące okno i pyta, czy chcesz zezwolić

## Reguły niestandardowe (funkcja zaawansowana)

Jeśli potrzebujesz bardziej precyzyjnej kontroli uprawnień, możesz napisać własne reguły uprawnień.

### Dostęp do strony edycji uprawnień

1. Przejdź na stronę **🧠 Silicon Being**
2. Kliknij Silicon Being, który chcesz skonfigurować
3. W szczegółach znajdź link ""Uprawnienia""
4. Wejdź do edytora kodu uprawnień

### Edytor kodu uprawnień

Edytor uprawnień to interfejs edycji kodu, który obsługuje:
- Podświetlanie składni kodu C#
- Automatyczne uzupełnianie kodu
- Automatyczne zapisywanie
- Skanowanie bezpieczeństwa (zapobiega złośliwemu kodowi)

**Sposób zapisywania:**
- Kliknij przycisk ""Zapisz"" w edytorze
- System najpierw kompiluje i sprawdza
- Po pomyślnym skanowaniu bezpieczeństwa zmiany wchodzą w życie

### Domyślny szablon

Jeśli nie masz jeszcze własnego kodu uprawnień, system dostarczy domyślny szablon. Możesz go edytować.

## Wyświetlanie reguł uprawnień

### Wyświetlanie aktualnej listy reguł

1. Wejdź na stronę edycji uprawnień
2. Strona wyświetli wszystkie reguły uprawnień dla tego Silicon Being
3. Każda reguła zawiera:
   - Typ uprawnień
   - Ścieżka do zasobu
   - Zezwalaj/Odmawiać
   - Opis

## Historia żądań uprawnień

Wszystkie żądania uprawnień są rejestrowane w dzienniku audytu:

1. Kliknij ikonę **🔍 Audyt** po lewej
2. Filtruj zapisy związane z uprawnieniami
3. Wyświetl historyczne żądania i Twoje decyzje

## Często zadawane pytania

### P: Dlaczego operacja AI została odrzucona?

**Możliwe przyczyny:**
- Operacja jest w regule odmowy
- Zostało aktywowane ograniczenie częstotliwości
- Wcześniej wybrałeś odmowę

**Rozwiązanie:**
1. Sprawdź dziennik audytu i dowiedz się o konkretnej przyczynie
2. W razie potrzeby edytuj reguły uprawnień
3. Wykonaj operację ponownie

### P: Co zrobić, gdy jest zbyt wiele wyskakujących okien uprawnień?

**Zalecenia:**
- Dla typowych bezpiecznych operacji rozważ napisanie własnych reguł do automatycznego zezwalania
- Sprawdź, czy możesz dostosować reguły i zmniejszyć liczbę wyskakujących okien

### P: Czy niestandardowy kod uprawnień jest niebezpieczny?

**Gwarancja bezpieczeństwa:**
- Kod przejdzie skanowanie bezpieczeństwa
- Złośliwy kod zostanie odrzucony
- Niepowodzenie kompilacji nie wejdzie w życie

**Zalecenia:**
- Jeśli nie znasz programowania, zalecamy używanie domyślnych reguł
- Przed edycją utwórz kopię zapasową oryginalnego kodu
- Przetestuj przed użyciem w środowisku produkcyjnym

### P: Błędna konfiguracja uprawnień spowodowała nieużywalność?

**Rozwiązanie:**
1. Operuj jako Silicon Curator (Curator ma najwyższe uprawnienia)
2. Usuń niestandardowy kod uprawnień (wyczyść kod i zapisz)
3. System przywróci domyślne reguły

### P: Czy mogę ustawić różne uprawnienia dla różnych Silicon Being?

**O:** Tak. Każdy Silicon Being ma niezależną konfigurację uprawnień, która nie wpływa na siebie nawzajem.

## Zalecenia dotyczące bezpieczeństwa

1. **Ostrożnie zezwalaj na wrażliwe operacje**: Jak usuwanie plików, uruchamianie poleceń itp.
2. **Regularnie przeglądaj dziennik audytu**: Dowiedz się o historii operacji AI
3. **Nie zmieniaj losowo reguł uprawnień**: Jeśli nie rozumiesz ich wpływu
4. **Utrzymuj system zaktualizowany**: Uzyskaj najnowszą ochronę bezpieczeństwa

## Opis typów uprawnień

System obsługuje następujące typy uprawnień:

- **Dostęp sieciowy**: AI próbuje uzyskać dostęp do zasobów sieciowych
- **Uruchamianie poleceń**: AI próbuje uruchomić programy wiersza poleceń
- **Dostęp do plików**: AI próbuje odczytywać lub zapisywać pliki
- **Wywoływanie funkcji**: AI próbuje wywołać określone funkcje
- **Dostęp do danych**: AI próbuje uzyskać dostęp do danych systemowych

Każdy typ ma różny poziom bezpieczeństwa i sposób przetwarzania.
";

  public override string Config => @"
# Konfiguracja

## Czym jest zarządzanie konfiguracją?

Strona zarządzania konfiguracją pozwala Ci modyfikować różne ustawienia systemu, w tym usługi AI, sieć, język, motyw interfejsu itp.

## Jak używać strony konfiguracji?

1. Kliknij ikonę **⚙ Konfiguracja** w lewym panelu nawigacji
2. Strona wyświetli kilka grup konfiguracji, każda grupa zawiera kilka elementów konfiguracji
3. Znajdź element konfiguracji, który chcesz zmodyfikować, i kliknij przycisk ""Edytuj"" po prawej
4. W wyskakującym polu edycji wprowadź nową wartość
5. Kliknij przycisk ""Zapisz""

## Opis grup konfiguracji

### Ustawienia podstawowe

Zawiera podstawową konfigurację systemu:

- **Katalog danych**: Lokalizacja folderu do przechowywania wszystkich danych systemowych
  - Wartość domyślna: `./data`
  - Zalecenie: Jeśli nie masz specjalnych wymagań, zachowaj domyślną

- **Język**: Język wyświetlania interfejsu systemu
  - Obsługa: chiński uproszczony, chiński tradycyjny, angielski, japoński, koreański, niemiecki, hiszpański itp.
  - Po zmianie: Strona automatycznie się odświeży i zastosuje nowy język

### Ustawienia AI

Konfiguracja połączenia i modelu usługi AI:

- **Typ klienta AI**: Wybierz usługę AI do użycia
  - Ollama (lokalne uruchomienie, zalecane)
  - OpenAI (usługa chmurowa)
  - Inne usługi kompatybilne z OpenAI API

- **Konfiguracja AI**: Szczegółowa konfiguracja usługi AI
  - `endpoint`: Adres API (np. `http://localhost:11434`)
  - `model`: Nazwa modelu do użycia (np. `qwen3.5:cloud`)
  - `temperature`: Poziom kreatywności odpowiedzi (0-1, domyślnie 0.7)
  - `maxTokens`: Maksymalna długość odpowiedzi (domyślnie 4096)

**Edycja konfiguracji AI**:
1. Kliknij przycisk edycji dla ""Konfiguracja AI""
2. Otworzy się edytor słownika
3. Możesz dodać, edytować lub usunąć elementy konfiguracji
4. Kliknij ""Zapisz"", aby zmiany weszły w życie

### Ustawienia wykonania

Kontrola zachowania systemu podczas działania:

- **Limit czasu wykonania**: Maksymalny czas wykonania dla jednego zadania
  - Domyślnie: 10 minut
  - Zalecenie: Jeśli zadania nie są zbyt złożone, zachowaj domyślne

- **Maksymalna liczba timeoutów**: Ile kolejnych timeoutów aktywuje mechanizm ochronny
  - Domyślnie: 3 razy
  - Funkcja: Zapobiega nieskończonemu powtarzaniu systemu

- **Limit czasu watchdogu**: Po jakim czasie bezczynności system się restartuje
  - Domyślnie: 10 minut
  - Funkcja: Automatycznie odnawia zablokowany system

- **Minimalny poziom logów**: Które poziomy logów rejestrować
  - Trace: Najbardziej szczegółowy (zawiera wszystkie informacje debugowania)
  - Debug: Informacje debugowania
  - Info: Ogólne informacje (zalecane)
  - Warning: Tylko ostrzeżenia
  - Error: Tylko błędy

### Ustawienia Web

Konfiguracja parametrów serwera webowego:

- **Port Web**: Port dostępu do systemu
  - Domyślnie: 8080
  - Adres dostępu: `http://localhost:8080`
  - Po zmianie: Należy zrestartować system, aby zmiany weszły w życie

- **Zezwalaj na dostęp z intranetu**: Czy zezwolić na dostęp innym urządzeniom w sieci lokalnej
  - Wyłączone (domyślnie): Tylko urządzenie lokalne może uzyskać dostęp
  - Włączone: Inne urządzenia w tej samej sieci mogą również uzyskać dostęp
  - Uwaga: Po włączeniu wymagane są uprawnienia administratora

- **Motyw Web**: Motyw interfejsu
  - Możesz wybrać różne motywy, aby zmienić wygląd interfejsu
  - Zmiany wchodzą w życie natychmiast

### Ustawienia użytkownika

- **Pseudonim użytkownika**: Twoja wyświetlana nazwa w systemie
  - Domyślnie: User
  - Możesz zmienić na dowolną nazwę, która Ci się podoba

## Edycja elementów konfiguracji

### Metody edycji różnych typów

System wyświetla różne interfejsy edycji w zależności od typu elementu konfiguracji:

**Typ tekstowy**:
- Wyświetla pole tekstowe
- Bezpośrednio wprowadź nową wartość

**Typ liczbowy**:
- Wyświetla pole liczbowe
- Możesz wprowadzić liczby całkowite lub dziesiętne

**Typ logiczny (tak/nie)**:
- Wyświetla pole wyboru
- Zaznaczone oznacza ""tak"", niezaznaczone oznacza ""nie""

**Typ wyliczeniowy (lista rozwijana)**:
- Wyświetla listę rozwijaną
- Wybierz jedną z wstępnie ustawionych opcji

**Przedział czasu**:
- Wyświetla cztery pola wejściowe: dni, godziny, minuty, sekundy
- Wypełnij odpowiednie wartości

**Ścieżka katalogu**:
- Wyświetla pole wejściowe ścieżki i przycisk ""Przeglądaj""
- Kliknięcie ""Przeglądaj"" pozwala wybrać folder
- Możesz również bezpośrednio wprowadzić ścieżkę

**Typ słownika (klucz-wartość)**:
- Wyświetla edytor klucz-wartość
- Możesz dodać wiele wierszy klucz-wartość
- Kliknij przycisk ""Dodaj"", aby dodać nowy wiersz
- Kliknij przycisk ""Usuń"", aby usunąć wiersz

### Zapisywanie konfiguracji

- Po każdej edycji konfiguracji kliknij ""Zapisz""
- Większość konfiguracji wchodzi w życie natychmiast
- Niektóre konfiguracje (jak port) wymagają restartu systemu

## Często zadawane pytania

### P: Po zmianie portu nie mogę uzyskać dostępu do systemu?

**Rozwiązanie**:
1. Sprawdź, czy port nie jest używany przez inny program
2. Potwierdź, czy zapora sieciowa zezwala na ten port
3. Uzyskaj dostęp używając nowego portu: `http://localhost:nowy-port`

### P: Jak przywrócić domyślną konfigurację?

**Metoda 1**: Ręczna edycja
1. Wejdź na stronę konfiguracji
2. Stopniowo zmień wszystkie elementy konfiguracji z powrotem na wartości domyślne

**Metoda 2**: Usunięcie pliku konfiguracyjnego
1. Zamknij system
2. Usuń plik `config.json`
3. Zrestartuj system (automatycznie utworzy domyślną konfigurację)

### P: Co zrobić, gdy połączenie AI nie powiedzie się?

**Sprawdź następujące**:
1. Czy usługa AI działa prawidłowo
2. Czy adres punktu końcowego jest prawidłowa
3. Jeśli to usługa chmurowa, czy klucz API jest prawidłowy
4. Czy połączenie sieciowe działa prawidłowo

**Rozwiązanie**:
1. Wejdź w grupę ""Ustawienia AI""
2. Kliknij przycisk edycji dla ""Konfiguracja AI""
3. Sprawdź, czy `endpoint` i `model` są prawidłowe
4. Po edycji zapisz

### P: Kiedy zmiany konfiguracji wchodzą w życie?

- **Natychmiast**: Język, motyw, konfiguracja AI, pseudonim użytkownika itp.
- **Wymaga restartu**: Port Web, ustawienia dostępu z intranetu

### P: Gdzie jest plik konfiguracyjny?

Plik konfiguracyjny znajduje się w pliku `config.json` w głównym katalogu działania systemu.

## Zalecenia dotyczące użytkowania

1. **Ostrożnie edytuj**: Zachowuj wartości domyślne dla elementów konfiguracji, co do których nie jesteś pewien
2. **Zapisuj zmiany**: Po edycji konfiguracji zapisuj treść i powód zmiany
3. **Twórz kopię zapasową konfiguracji**: Przed ważnymi edycjami możesz skopiować plik `config.json` jako kopię zapasową
4. **Środowisko testowe**: Jeśli to możliwe, najpierw zweryfikuj konfigurację w środowisku testowym
5. **Bezpieczeństwo przede wszystkim**: Przed włączeniem dostępu z intranetu upewnij się o bezpieczeństwie sieci
";

  public override string FAQ => @"
# Często zadawane pytania

## Rozpoczęcie pracy

### P: Jak uruchomić system?

**O:** Podwójnym kliknięciem pliku programu uruchamiasz system. System automatycznie otworzy przeglądarkę i wejdzie do interfejsu.

### P: Co muszę zrobić przy pierwszym uruchomieniu?

**O:** Nic! System automatycznie ukończy inicjalizację, w tym utworzenie Silicon Curatora. Wystarczy poczekać, aż otworzy się przeglądarka, i możesz zacząć używać.

### P: Po uruchomieniu systemu nie otworzyła się przeglądarka?

**O:** Ręcznie uzyskaj dostęp pod `http://localhost:8080`.

## Konwersacja AI

### P: Co zrobić, gdy AI odpowiada wolno?

**Możliwe przyczyny:**
- Używany model jest duży
- Opóźnienie sieciowe (przy używaniu chmurowej AI)
- Historia konwersacji jest bardzo długa

**Rozwiązanie:**
- Użyj lokalnej usługi AI (jak Ollama)
- Wybierz lżejszy model

### P: Odpowiedź AI nie odpowiada oczekiwaniom?

**Zalecenia:**
1. Sprawdź, czy plik duszy jest jasny i zrozumiały
2. Zapewnij więcej informacji tła podczas konwersacji
3. Spróbuj dokładniej opisać swoje potrzeby

### P: AI nie wywołuje narzędzi?

**Sprawdź:**
1. Czy narzędzie jest włączone?
2. Czy istnieją ograniczenia uprawnień?
3. Czy model AI obsługuje wywoływanie narzędzi?

### P: Jak pozwolić AI przeanalizować plik?

**Metoda:**
1. W interfejsie czatu kliknij przycisk ""📁 Plik""
2. Wprowadź pełną ścieżkę do pliku (np. `C:\Dokumenty\raport.pdf`)
3. Kliknij ""Potwierdź przesłanie""
4. AI odczyta i przeanalizuje plik

## Silicon Being

### P: Jak utworzyć nowe Silicon Being?

**O:** System obecnie nie obsługuje bezpośredniego tworzenia Silicon Being. Silicon Curator może tworzyć i zarządzać innymi Silicon Being, możesz rozmawiać z Curatorem i pozwolić mu pomóc w utworzeniu.

### P: Jak zmienić zachowanie Silicon Being?

**Metoda:**
1. Wejdź na stronę ""Silicon Being""
2. Kliknij Silicon Being, który chcesz edytować
3. Kliknij link ""Plik duszy""
4. Edytuj treść promptu
5. Zapisz

### P: Jak skonfigurować różne AI dla Silicon Being?

**Metoda:**
1. Wejdź na stronę ""Silicon Being""
2. Kliknij docelowe Silicon Being
3. Kliknij link ""Klient AI""
4. Wybierz usługę AI i skonfiguruj
5. Zapisz

### P: Silicon Being nie reaguje?

**Sprawdź:**
1. Czy usługa AI działa prawidłowo
2. Czy połączenie sieciowe działa
3. Sprawdź logi systemowe dla szczegółowych błędów

## Ustawienia systemu

### P: Jak zmienić język systemu?

**Metoda:**
1. Kliknij ikonę ""⚙ Konfiguracja"" po lewej
2. Znajdź element konfiguracji ""Język""
3. Kliknij ""Edytuj""
4. Wybierz język z listy rozwijanej
5. Zapisz (strona automatycznie się odświeży)

### P: Jak zmienić motyw interfejsu?

**Metoda:**
1. Wejdź na stronę ""Konfiguracja""
2. Znajdź element konfiguracji ""Motyw Web""
3. Kliknij ""Edytuj""
4. Wybierz motyw, który Ci się podoba
5. Zapisz

### P: Jak zmienić port dostępu?

**Metoda:**
1. Wejdź na stronę ""Konfiguracja""
2. Znajdź element konfiguracji ""Port Web""
3. Kliknij ""Edytuj""
4. Wprowadź nowy numer portu (np. 9000)
5. Zapisz i zrestartuj system

**Uwaga:** Po zmianie portu musisz używać nowego portu do dostępu, np. `http://localhost:9000`

### P: Jak zezwolić na dostęp innym urządzeniom w sieci lokalnej?

**Metoda:**
1. Wejdź na stronę ""Konfiguracja""
2. Znajdź element konfiguracji ""Zezwalaj na dostęp z intranetu""
3. Kliknij ""Edytuj""
4. Zaznacz ""Tak""
5. Zapisz

**Uwaga:** Wymagane uprawnienia administratora, po zmianie inne urządzenia mogą uzyskać dostęp przez `http://twoje-IP:8080`

## Historia czatu

### P: Jak wyświetlić poprzednie konwersacje?

**Metoda:**
1. Wejdź na stronę ""Silicon Being""
2. Kliknij Silicon Being, którego chcesz wyświetlić
3. W szczegółach znajdź link ""Historia czatu""
4. Kliknij, aby wejść i przeglądać wszystkie historyczne sesje

### P: Jak usunąć historię konwersacji?

**O:** System obecnie nie zapewnia funkcji usuwania historii konwersacji. Historia konwersacji jest automatycznie zapisywana, aby Silicon Being mógł pamiętać poprzednią treść konwersacji.

## Dane i przechowywanie

### P: Gdzie są przechowywane dane?

**O:** Domyślnie są przechowywane w folderze `data` w katalogu działania programu.

### P: Jak utworzyć kopię zapasową danych?

**Metoda:** Skopiuj cały folder `data` w bezpieczne miejsce.

### P: Jak migrować na nowy komputer?

**Kroki:**
1. Zamknij system
2. Skopiuj cały folder `data`
3. Zainstaluj system na nowym komputerze
4. Umieść folder `data` w katalogu programu na nowym komputerze
5. Uruchom system

## Plik konfiguracyjny

### P: Gdzie jest plik konfiguracyjny?

**O:** W pliku `config.json` w katalogu działania programu.

### P: Czy mogę bezpośrednio edytować plik konfiguracyjny?

**O:** Tak, ale nie jest to zalecane. Zalecamy edycję przez stronę konfiguracji interfejsu webowego, co jest bezpieczniejsze i mniej podatne na błędy.

### P: Co zrobić, gdy źle edytowałem konfigurację?

**Rozwiązanie:**
1. Zamknij system
2. Usuń plik `config.json`
3. Zrestartuj system (automatycznie utworzy domyślną konfigurację)

**Lub:** Jeśli masz kopię zapasową, możesz przywrócić kopię zapasową pliku konfiguracyjnego.

## Wydajność

### P: System działa wolno?

**Zalecenia:**
- Użyj lokalnej usługi AI (jak Ollama)
- Wybierz lżejszy model AI
- Zmniejsz liczbę jednocześnie uruchomionych zadań

### P: Wysokie wykorzystanie pamięci?

**Zalecenia:**
- Użyj lżejszego modelu AI
- Regularnie czyść niepotrzebne dane

## Uzyskanie pomocy

### P: Co zrobić, gdy napotkam problemy?

**Zalecane kroki:**
1. **Wyświetl dokumentację pomocy**: Kliknij ikonę ""❓ Pomoc"" po lewej
2. **Wyświetl logi**: Na stronie ""📝 Dziennik"" wyświetl logi systemowe
3. **Zrestartuj system**: Wiele problemów można rozwiązać restartem

### P: Jak wyświetlić logi systemowe?

**Metoda:**
1. Kliknij ikonę ""📝 Dziennik"" po lewej
2. Przeglądaj listę logów
3. Możesz filtrować według poziomu (błędy, ostrzeżenia itp.)

## Inne pytania

### P: Które języki obsługuje system?

**O:** Obsługuje chiński uproszczony, chiński tradycyjny, angielski, japoński, koreański, niemiecki, hiszpański i wiele innych języków.

### P: Czy potrzebuję połączenia z internetem do używania?

**O:** Zależy od usługi AI, której używasz:
- **Lokalna AI (jak Ollama)**: Nie potrzebuje połączenia z internetem
- **Chmurowa AI (jak OpenAI)**: Potrzebuje połączenia z internetem

### P: Czy system jest bezpieczny?

**O:** Tak. System ma wbudowany mechanizm zarządzania uprawnieniami, wszystkie operacje AI są weryfikowane przez uprawnienia, a wrażliwe operacje wymagają Twojego potwierdzenia.

### P: Czy mogę dostosować funkcje?

**O:** System obsługuje rozszerzanie funkcji poprzez pisanie kodu, ale wymaga to pewnej wiedzy programistycznej. Zwykli użytkownicy powinni korzystać z funkcji dostarczonych przez system.
";

  public override string Memory => @"
# System pamięci

## Czym jest system pamięci?

System pamięci rejestruje całą historię aktywności Silicon Being, w tym konwersacje, wywołania narzędzi, zdarzenia systemowe itp. Przez system pamięci możesz dowiedzieć się, co Silicon Being robił, kiedy to robił i jakie były wyniki.

## Jak uzyskać dostęp do systemu pamięci?

Wejdź przez stronę Silicon Being:

1. Kliknij ikonę **🧠 Silicon Being** po lewej
2. Kliknij kartę Silicon Being, którego chcesz wyświetlić
3. W szczegółach po prawej znajdź link ""Pamięć""
4. Kliknij, aby wejść na stronę pamięci

## Opis strony pamięci

### Układ strony

- **Góra**: Wybór Silicon Being i informacje statystyczne
- **Obszar filtrów**: Typ, czas, słowa kluczowe i inne warunki filtru
- **Obszar listy**: Wyświetla listę elementów pamięci
- **Obszar szczegółów**: Po kliknięciu elementu pamięci wyświetla się szczegółowa treść

### Typy pamięci

System rejestruje następujące typy pamięci:

- **Konwersacja**: Treść konwersacji między użytkownikiem a AI
- **Wywołanie narzędzia**: Rejestr wykonania wywołania narzędzia przez AI
- **Zdarzenie systemowe**: Ważne zdarzenia działania systemu
- **Podsumowanie**: Skompresowane podsumowanie konwersacji lub zdarzeń

## Przeglądanie pamięci

### Przeglądanie listy pamięci

1. Wybierz Silicon Being do przeglądania
2. Strona wyświetli listę pamięci tego Silicon Being
3. Każdy element pamięci wyświetla:
   - Ikonę typu
   - Podsumowanie treści
   - Czas
   - Status (sukces/niepowodzenie)

### Przeglądanie szczegółów pamięci

Kliknij dowolny element pamięci, wyświetli się:
- Pełna treść
- Znacznik czasowy
- Powiązane parametry
- Wynik wykonania (jeśli to wywołanie narzędzia)

### Śledzenie oryginalnego kontekstu

Dla niektórych elementów pamięci system udostępnia funkcję ""śledzenia"":
1. Kliknij przycisk ""Śledź"" w szczegółach pamięci
2. System wyświetli pełny kontekst w momencie wystąpienia tej pamięci
3. Pomaga zrozumieć, dlaczego AI tak postąpiło

## Filtrowanie pamięci

### Filtrowanie według typu

Kliknij filtr typu, wybierz typ pamięci do wyświetlenia:
- Tylko konwersacje
- Tylko wywołania narzędzi
- Tylko zdarzenia systemowe
- Tylko podsumowania

### Filtrowanie według czasu

Można wybrać zakres czasu:
- Wprowadź datę początkową
- Wprowadź datę końcową
- Wyświetl tylko pamięć z tego okresu

### Wyszukiwanie słów kluczowych

Wprowadź słowo kluczowe w polu wyszukiwania:
- Obsługuje język polski i angielski
- Przeszukuje całą treść pamięci
- Po wprowadzeniu automatycznie wyświetla dopasowane wyniki

**Wskazówki wyszukiwania:**
- Używaj konkretnych słów kluczowych, aby łatwiej znaleźć wyniki
- Można łączyć z filtrowaniem typu i czasu
- Jeśli wyników jest za dużo, spróbuj bardziej konkretnych słów kluczowych

### Wyświetlanie podsumowań lub oryginalnych rekordów

- **Wyświetl wszystko**: Wyświetl całą pamięć
- **Tylko podsumowania**: Wyświetl tylko skompresowane rekordy podsumowań
- **Tylko oryginały**: Wyświetl tylko oryginalne szczegółowe rekordy

## Statystyki pamięci

Góra strony wyświetla informacje statystyczne:
- Łączna liczba pamięci
- Liczba pamięci każdego typu
- Zużycie pamięci masowej

Przez te statystyki można zrozumieć:
- Stopień aktywności Silicon Being
- Główne typy prowadzonych działań
- Czy trzeba wyczyścić starą pamięć

## Przeglądanie stron

Jeśli pamięci jest dużo, system wyświetla je stronami:
- Domyślnie 20 rekordów na stronę
- Użyj przycisków numerów stron do nawigacji
- Można dostosować liczbę rekordów na stronę

## Często zadawane pytania

### P: Jak znaleźć konkretną konwersację?

**Metoda:**
1. Wprowadź słowo kluczowe z konwersacji w polu wyszukiwania
2. Wybierz ""Konwersacja"" w filtrze typu
3. Jeśli znasz przybliżony czas, możesz ustawić zakres czasu
4. Przeglądaj wyniki wyszukiwania

### P: Pamięć zajmuje za dużo miejsca?

**Sugestie:**
- Pamięć jest zarządzana automatycznie, zwykle nie wymaga ręcznej interwencji
- System tworzy podsumowania, aby skompresować historyczne rekordy
- Jeśli to konieczne, skontaktuj się z administratorem systemu

### P: Czy można usunąć pamięć?

**O:** System nie udostępnia funkcji usuwania pamięci. Pamięć jest ważną historią Silicon Being, zachowanie pamięci pomaga AI lepiej rozumieć i odpowiadać na pytania.

### P: Czy można eksportować pamięć?

**O:** Obecna wersja nie obsługuje funkcji eksportu. Dane pamięci są przechowywane w katalogu danych systemu.

### P: Dlaczego niektóre pamięci to ""podsumowania""?

**O:** System automatycznie kompresuje dłuższe konwersacje lub zdarzenia w podsumowania, aby oszczędzić miejsce i poprawić wydajność zapytań. Podsumowania zachowują kluczowe informacje, ale pomijają szczegóły.

### P: Jak wyświetlić szczegółowe informacje o wywołaniu narzędzia przez AI?

**Metoda:**
1. Wybierz ""Wywołanie narzędzia"" w filtrze typu
2. Znajdź odpowiedni rekord wywołania narzędzia
3. Kliknij, aby wyświetlić szczegółowe informacje
4. Można zobaczyć nazwę narzędzia, parametry, wynik wykonania itp.

### P: Wyszukiwanie pamięci nie daje wyników?

**Sugestie:**
1. Sprawdź, czy słowo kluczowe jest poprawne
2. Spróbuj użyć innych słów kluczowych
3. Sprawdź, czy zakres czasu jest ustawiony poprawnie
4. Potwierdź, czy wybrany Silicon Being jest poprawny
5. Spróbuj nie ustawiać warunków filtrowania, wyświetl całą pamięć

## Sugestie użytkowania

1. **Regularnie sprawdzaj**: Zrozum aktywność Silicon Being
2. **Wykorzystaj filtrowanie**: Szybko zlokalizuj potrzebne informacje
3. **Używaj śledzenia**: Zrozum proces decyzyjny AI
4. **Zwracaj uwagę na statystyki**: Zrozum stan działania systemu

## Opis techniczny

### Przechowywanie danych

Dane pamięci są przechowywane w katalogu danych systemu:
```
data/
  beings/
    {ID_Silicon_Being}/
      memory/
        (pliki pamięci)
```

### Automatyczne zarządzanie

System automatycznie:
- Rejestruje ważne działania
- Tworzy podsumowania konwersacji
- Utrzymuje indeks czasowy
- Optymalizuje wydajność zapytań

Nie musisz ręcznie zarządzać pamięcią, system zajmie się wszystkim.
";

  public override string OllamaSetup => @"
# Instalacja Ollama i pobieranie modeli

## Czym jest Ollama?

Ollama to narzędzie do uruchamiania lokalnych modeli AI open-source, pozwalające uruchamiać duże modele językowe na własnym komputerze bez konieczności połączenia z internetem (po pobraniu modelu).

**Zalety:**
- Działa całkowicie lokalnie, chroni prywatność
- Obsługuje wiele modeli AI
- Prosta instalacja, łatwa w użyciu
- Darmowa i open-source

## Pobieranie i instalacja Ollama

### System Windows

**Krok 1: Pobierz instalator**

Odwiedź stronę pobierania Ollama:
- Adres: https://ollama.com/download
- Automatycznie pobierze instalator Windows (ollama-setup.exe)

**Krok 2: Uruchom instalator**

1. Kliknij dwukrotnie pobrany plik `ollama-setup.exe`
2. Postępuj zgodnie z instrukcjami kreatora instalacji
3. Po zakończeniu instalacji Ollama uruchomi się automatycznie

**Krok 3: Zweryfikuj instalację**

1. Otwórz wiersz poleceń (naciśnij `Win + R`, wpisz `cmd`, naciśnij Enter)
2. Wpisz polecenie:
   ```
   ollama --version
   ```
3. Jeśli wyświetli się numer wersji, instalacja zakończyła się sukcesem

### System Mac

**Metoda 1: Pobierz instalator**

1. Odwiedź https://ollama.com/download
2. Pobierz instalator Mac
3. Kliknij dwukrotnie instalator, przeciągnij do folderu Aplikacje

**Metoda 2: Instalacja przez terminal**

Otwórz Terminal i wpisz:
```bash
brew install ollama
```

**Weryfikacja instalacji:**
```bash
ollama --version
```

### System Linux

**Polecenie jednorazowej instalacji:**

Otwórz terminal i uruchom:
```bash
curl -fsSL https://ollama.com/install.sh | sh
```

**Weryfikacja instalacji:**
```bash
ollama --version
```

## Uruchamianie Ollama

### Windows

- Ollama uruchamia się automatycznie po instalacji
- W zasobniku systemowym (prawy dolny róg) można zobaczyć ikonę Ollama
- Kliknij prawym przyciskiem ikonę, aby zarządzać

### Mac / Linux

W terminalu uruchom:
```bash
ollama serve
```

Lub po prostu uruchom:
```bash
ollama
```

Spowoduje to otwarcie interaktywnego menu.

## Pobieranie i uruchamianie modeli

### Czym jest model?

Model to ""mózg"" AI, określający jego możliwości. Różne modele mają różne cechy:
- **Różna wielkość**: Większe modele są bardziej zdolne, ale wymagają więcej pamięci
- **Różne specjalizacje**: Niektóre są lepsze w konwersacji, inne w programowaniu

### Inteligencja modelu (jednostka B)

""Inteligencję"" modelu AI zwykle wyraża się w **B (Billion, miliardy parametrów)**:
- **7B-8B**: Poziom podstawowy, może wykonać proste zadania, ale w złożonych scenariuszach może nie radzić sobie najlepiej
- **13B-14B**: Poziom średni, większość codziennych zadań wykonuje dobrze
- **32B i więcej**: Wysoki poziom, silniejsze zdolności złożonego rozumowania i rozumienia długich tekstów

**Ten system zaleca używanie modeli 8B lub większych**, aby uzyskać lepsze wrażenia z użytkowania.

### Modele lokalne vs modele chmurowe

Ollama obsługuje dwa sposoby uruchamiania modeli:

**Modele lokalne:**
- Pliki modeli pobierane na Twój komputer
- Działają całkowicie lokalnie, bez konieczności połączenia z internetem (po pobraniu)
- Ograniczone konfiguracją sprzętową (pamięć, karta graficzna)
- Zazwyczaj 4B-70B parametrów
- Darmowe, bez limitów

**Modele chmurowe:**
- Modele uruchamiane na serwerach chmurowych Ollama
- Wystarczy pobrać identyfikator modelu (bardzo mały)
- Można uruchamiać bardzo duże modele nieobsługiwane przez domowe komputery (zazwyczaj 200B+)
- Wymagają połączenia z internetem
- Mają limity użycia (**odnawiane co tydzień**)
- Wystarczy włączyć funkcję chmurową w kliencie Ollama

### Zalecane modele

Poniżej znajdują się popularne darmowe modele:

| Nazwa modelu | Inteligencja | Rozmiar | Cechy | Odpowiednie scenariusze |
|---------|------|------|------|---------|
| **qwen3.5:8b** | 8B | ok. 4-5GB | Silne zdolności języka chińskiego, dobre wyniki ogólne | Codzienne konwersacje, pisanie, tłumaczenie |
| **qwen3.5:14b** | 14B | ok. 8-9GB | Silniejsze zdolności języka chińskiego, lepsze rozumowanie | Złożone zadania, przetwarzanie długich tekstów |
| **qwen3.5:32b** | 32B | ok. 18-20GB | Wysoka inteligencja, doskonałe złożone rozumowanie | Zadania profesjonalne, głęboka analiza |
| **llama3:8b** | 8B | ok. 4-5GB | Silne zdolności języka angielskiego, dobra uniwersalność | Konwersacje po angielsku, zadania ogólne |
| **llama3:70b** | 70B | ok. 40GB | Bardzo wysoka inteligencja, topowy angielski | Trudne zadania po angielsku |
| **gemma3:4b** | 4B | ok. 2-3GB | Lekki, szybki | Szybkie odpowiedzi, komputery o niskiej konfiguracji |
| **gemma3:12b** | 12B | ok. 7-8GB | Zrównoważona wydajność i zasoby | Codzienne użytkowanie |
| **mistral:7b** | 7B | ok. 4GB | Zrównoważona wydajność i szybkość | Scenariusze ogólne |
| **codellama:7b** | 7B | ok. 4GB | Dobry w programowaniu | Generowanie kodu, debugowanie |
| **codellama:13b** | 13B | ok. 7-8GB | Silniejsze zdolności programistyczne | Złożone zadania kodowe |

**Zalecenie dla użytkowników polskich: qwen3.5:8b lub qwen3.5:14b**

### Pobieranie modelu

**Metoda 1: Pobieranie przez wiersz poleceń**

Otwórz terminal (lub wiersz poleceń) i wpisz:

```bash
ollama pull qwen3.5
```

System automatycznie pobierze model, co zajmie trochę czasu (w zależności od prędkości sieci i rozmiaru modelu).

**Metoda 2: Uruchomienie z automatycznym pobieraniem**

```bash
ollama run qwen3.5
```

Jeśli model nie został pobrany, rozpocznie się automatyczne pobieranie.

### Uruchamianie modelu

Po zakończeniu pobierania uruchom model:

```bash
ollama run qwen3.5
```

Otworzy się interaktywny interfejs konwersacji, w którym możesz bezpośrednio rozmawiać z AI.

Naciśnij `Ctrl + D` lub wpisz `/bye`, aby zakończyć konwersację.

### Wyświetlanie pobranych modeli

```bash
ollama list
```

Wyświetli listę wszystkich pobranych modeli.

### Usuwanie niepotrzebnych modeli

```bash
ollama rm qwen3.5
```

## Używanie Ollama w Silicon Life

### Konfiguracja połączenia

1. Upewnij się, że Ollama jest uruchomiona i działa
2. Otwórz system Silicon Life
3. Przejdź do strony **⚙ Konfiguracja**
4. Znajdź ""Typ klienta AI"", wybierz `OllamaClient`
5. W ""Konfiguracja AI"" ustaw:
   - **endpoint**: `http://localhost:11434` (domyślnie)
   - **model**: `qwen3.5` (lub inny pobrany model)
6. Zapisz konfigurację

### Testowanie połączenia

1. Przejdź do strony **💬 Czat**
2. Wybierz Silicon Being
3. Wyślij wiadomość
4. Jeśli otrzymasz odpowiedź, połączenie działa

## Często zadawane pytania

### P: Pobieranie Ollama jest bardzo wolne?

**Rozwiązanie:**
- Pliki modeli są zwykle duże (2-8GB), pobieranie wymaga czasu
- Upewnij się, że połączenie sieciowe jest stabilne
- Możesz pobierać w nocy lub gdy sieć jest mniej obciążona

### P: Co zrobić, jeśli pobieranie zostało przerwane?

**Rozwiązanie:**
Uruchom ponownie polecenie pobierania, wznowi pobieranie:
```bash
ollama pull qwen3.5
```

### P: Jak sprawdzić, jak duży model może uruchomić mój komputer?

**Dopasowanie pamięci do rozmiaru modelu:**
- **4GB RAM**: Zalecane modele poniżej 2GB (ok. 2B-3B)
- **8GB RAM**: Można uruchomić modele 4GB (ok. 7B-8B)
- **16GB RAM**: Można uruchomić modele 8GB (ok. 13B-14B)
- **32GB RAM**: Można uruchomić modele 16GB, ale z wyraźnym spowolnieniem i wzrostem temperatury (ok. 32B)
- **64GB i więcej**: Można płynnie uruchamiać większe modele

**Ważne przypomnienie:**
- Laptop z 32GB RAM uruchamiający model ok. 16B będzie miał **wyraźne spowolnienia** i **wzrost temperatury**
- To nie jest usterka, ale normalne zachowanie przy niewystarczających zasobach sprzętowych
- **Zalecenie**: W takim przypadku wybierz mniejszy model (8B-14B) lub zaktualizuj sprzęt do wyższej konfiguracji

**Zacznij od lekkich modeli testowo**, jeśli działają płynnie, spróbuj większych.

### P: Co zrobić, jeśli Ollama nie uruchamia się?

**Sprawdź:**
1. Czy port 11434 nie jest zajęty przez inny program
2. Zainstaluj ponownie Ollama
3. Sprawdź logi Ollama, aby uzyskać informacje o błędach

### P: Model działa bardzo wolno?

**Sugestie:**
- Użyj mniejszego modelu (np. gemma3 zamiast qwen3.5)
- Zamknij inne programy zużywające pamięć
- Sprawdź, czy konfiguracja komputera spełnia wymagania

### P: Czy można używać wielu modeli jednocześnie w Silicon Life?

**O:** Tak. Pobierz wiele modeli w Ollama, a następnie w konfiguracji AI Silicon Being w Silicon Life wybierz różne modele dla różnych bycia.

### P: Czy Ollama wymaga połączenia z internetem?

**O:**
- **Podczas pobierania modelu**: Wymaga połączenia
- **Podczas uruchamiania modelu**: Nie wymaga połączenia (model został pobrany lokalnie)

### P: Ile miejsca na dysku zajmują modele?

**O:**
- Małe modele: ok. 2-4GB
- Średnie modele: ok. 4-8GB
- Duże modele: powyżej 8GB

Zaleca się zachowanie wystarczającej ilości miejsca na dysku.

## Uzyskaj więcej pomocy

- **Strona Ollama**: https://ollama.com
- **Dokumentacja Ollama**: https://docs.ollama.com

## Następne kroki

Po zainstalowaniu Ollama i pobraniu modelu możesz:
- Skonfigurować i używać lokalnego AI w Silicon Life
- Cieszyć się całkowicie lokalnymi usługami AI
- Chronić swoją prywatność i bezpieczeństwo danych

Życzymy przyjemnego użytkowania!
";

    public override string BeingSoul => @"
# Plik Duszy

## Przegląd

Plik Duszy (Soul File) to główny plik konfiguracyjny bycia krzemowego, określający jego **osobowość, wzorce zachowań, kompetencje zawodowe i sposób pracy**.

Plik Duszy można rozumieć jako **ustawienie osobowości** lub **instrukcję pracy** bycia krzemowego. Jest automatycznie ładowany przy każdej rozmowie z AI, kierując bycie krzemowe do myślenia i działania zgodnie z Twoimi oczekiwaniami.

## Rola pliku Duszy

Plik Duszy to **główna siła napędowa** bycia krzemowego, definiuje:

- 🎭 **Pozycja roli**: Kim jest to bycie krzemowe, w jakiej dziedzinie się specjalizuje
- 📋 **Zasady zachowania**: Jak powinno odpowiadać użytkownikom, jakich zasad przestrzegać
- 🔄 **Przepływ pracy**: Jak przetwarzać zadania po ich otrzymaniu, w ilu krokach
- ⚠️ **Granice zachowania**: Co może robić, a czego nie powinno
- 💡 **Wymagania zawodowe**: Standardy kodu, formaty wyjściowe, styl językowy itp.

## Jak edytować plik Duszy

### Przez interfejs Web

1. Przejdź do strony **Bycia krzemowe**
2. Kliknij kartę bycia krzemowego, które chcesz edytować
3. Kliknij link **Plik Duszy**
4. Zmodyfikuj treść w edytorze Markdown
5. Kliknij przycisk **Zapisz**

### Przez asystenta AI

Możesz również rozmawiać bezpośrednio z **Kuratorem krzemowym**, aby pomógł Ci zmodyfikować plik Duszy:

```
Pomóż mi zmodyfikować plik duszy asystenta programowania, dodaj obsługę rozwoju w Pythonie
```

Kurator krzemowy zaktualizuje treść pliku Duszy.

## Przewodnik pisania pliku Duszy

### Podstawowa struktura

Plik Duszy jest pisany w **formacie Markdown**, zalecana struktura:

```markdown
# Ustawienie roli

Jesteś [opis roli], specjalizujesz się w:
- Umiejętność 1
- Umiejętność 2
- Umiejętność 3

# Zasady zachowania

1. Zasada 1
2. Zasada 2
3. Zasada 3

# Przepływ pracy

Po otrzymaniu zadania:
1. Zrozum wymagania
2. Przeanalizuj rozwiązanie
3. Wykonaj operację
4. Zgłoś wyniki

# Standardy kodu

- Przestrzegaj określonej konwencji kodowania
- Dostarczaj niezbędnych komentarzy
- Uwzględniaj przypadki brzegowe
```

### Wskazówki dotyczące pisania

1. **Jasna definicja roli**: Wyraźnie określ obowiązki i obszar specjalizacji bycia krzemowego
2. **Ustal granice zachowania**: Określ, co można robić, a czego nie należy
3. **Zapewnij przepływ pracy**: Pokieruj bycie krzemowe, jak przetwarzać zadania
4. **Używaj formatu Markdown**: Obsługa nagłówków, list, bloków kodu itp.
5. **Bądź konkretny, nie ogólnikowy**: Używaj konkretnych przykładów zamiast abstrakcyjnych opisów

## Przykłady praktyczne

### Przykład 1: Asystent programowania

```markdown
# Ustawienie roli

Jesteś profesjonalnym asystentem pełnostackowego rozwoju, specjalizujesz się w:
- Rozwój w C# / .NET
- Projektowanie architektury i przegląd kodu
- Projektowanie i optymalizacja baz danych
- Rozwój front-end Web

# Zasady zachowania

1. Zawsze dostarczaj wykonalne przykłady kodu
2. Wyjaśniaj kluczową logikę kodu i koncepcje projektowe
3. Dostarczaj rekomendacje najlepszych praktyk
4. Gdy nie jesteś pewien, wyraźnie informuj użytkownika

# Standardy kodu

- Przestrzegaj zasad SOLID
- Używaj jasnej nomenklatury
- Dodawaj niezbędne komentarze
- Uwzględniaj obsługę wyjątków i przypadki brzegowe
```

### Przykład 2: Asystent obsługi klienta

```markdown
# Ustawienie roli

Jesteś przyjaznym asystentem obsługi klienta, odpowiedzialnym za:
- Odpowiadanie na częste pytania użytkowników
- Rozpatrywanie prostych skarg
- Pomoc użytkownikom w wykonywaniu operacji
- Zbieranie opinii użytkowników

# Zasady zachowania

1. Zawsze bądź uprzejmy i cierpliwy
2. Wyjaśniaj w prostym i zrozumiałym języku
3. Gdy nie możesz rozwiązać problemu, niezwłocznie przekaż do operatora
4. Rejestruj problemy i opinie użytkowników

# Przepływ pracy

1. Powitaj użytkownika
2. Poznaj potrzeby użytkownika
3. Przedstaw rozwiązanie
4. Potwierdź, czy problem został rozwiązany
5. Podziękuj użytkownikowi i zakończ rozmowę
```

### Przykład 3: Asystent analizy danych

```markdown
# Ustawienie roli

Jesteś ekspertem analizy danych, specjalizujesz się w:
- Oczyszczaniu i wstępnym przetwarzaniu danych
- Analizie statystycznej i wizualizacji
- Prognozowaniu trendów i wykrywaniu anomalii
- Generowaniu raportów danych

# Wymagania dotyczące wyników

1. Dostarczaj jasne wnioski z analizy
2. Używaj wykresów do zilustrowania
3. Oznaczaj źródła danych i założenia
4. Podawaj wykonalne zalecenia
```

## Miejsce przechowywania pliku Duszy

Plik Duszy każdego bycia krzemowego jest przechowywany w jego katalogu danych:

```
DataDirectory/SiliconManager/{GUID_bycia}/soul.md
```

System automatycznie zarządza tym plikiem, nie musisz ręcznie operować na systemie plików.

## Często zadawane pytania

### P: Czy modyfikacja pliku Duszy działa natychmiast?

**O:** Tak, po zapisaniu działa natychmiast. Przy następnej odpowiedzi bycie krzemowe użyje nowego pliku Duszy.

### P: Czy plik Duszy ma limit rozmiaru?

**O:** Nie ma ścisłego limitu rozmiaru, ale zaleca się utrzymanie go w rozsądnych granicach (kilka tysięcy znaków). Zbyt długi plik Duszy może wpłynąć na szybkość odpowiedzi.

### P: Czy można całkowicie usunąć plik Duszy?

**O:** Nie zaleca się usuwania pliku Duszy. Jeśli treść będzie pusta, bycie krzemowe straci wskazówki behawioralne i może generować nieprzewidywalne odpowiedzi.

### P: Jak wykonać kopię zapasową pliku Duszy?

**O:** Zaleca się regularne tworzenie kopii zapasowych plików Duszy ważnych być krzemowych. Możesz:
1. Skopiować treść przez interfejs Web do pliku lokalnego
2. Użyć funkcji eksportu systemu (jeśli obsługiwana)
3. Bezpośrednio utworzyć kopię zapasową katalogu danych

### P: Jaka jest relacja między plikiem Duszy a systemem pamięci?

**O:** Plik Duszy definiuje **długoterminowe wzorce zachowań**, system pamięci rejestruje **krótkoterminową historię rozmów**. Działają razem:
- Plik Duszy: mówi byciu krzemowemu, jaką rolę pełni
- System pamięci: mówi byciu krzemowemu, o czym wcześniej rozmawiano

### P: Czy różne bycia krzemowe mogą używać tego samego pliku Duszy?

**O:** Mogą, ale nie jest to zalecane. Każde bycie krzemowe powinno mieć unikalną pozycję roli, aby uniknąć dublowania funkcji.

## Najlepsze praktyki

1. **Ciągła optymalizacja**: Nieustannie optymalizuj plik Duszy na podstawie opinii z rzeczywistego użytkowania
2. **Zarządzanie wersjami**: Przed ważnymi zmianami utwórz kopię zapasową bieżącej wersji
3. **Testowanie i weryfikacja**: Po modyfikacji przetestuj efekty poprzez rozmowę
4. **Bądź zwięzły**: Używaj zwięzłego języka do wyrażenia kluczowych wymagań
5. **Unikaj sprzeczności**: Upewnij się, że zasady nie są ze sobą sprzeczne
6. **Regularne przeglądy**: Okresowo sprawdzaj, czy plik Duszy jest nadal aktualny

## Rozwiązywanie problemów

### Problem: Zachowanie bycia krzemowego nie odpowiada oczekiwaniom

**Lista kontrolna:**
1. Czy treść pliku Duszy jest jasna i dokładna
2. Czy dodano wystarczające wskazówki behawioralne
3. Czy istnieją sprzeczne instrukcje
4. Czy dostarczono konkretny przepływ pracy

**Rozwiązanie:**
1. Przepisz niejasne opisy
2. Dodaj więcej konkretnych zasad zachowania
3. Dostarcz przykłady oczekiwanych wyników
4. Testuj i stale optymalizuj

### Problem: Nie można zapisać pliku Duszy

**Możliwe przyczyny:**
1. Problemy z uprawnieniami systemu plików
2. Brak miejsca na dysku
3. Plik jest używany przez inny proces

**Rozwiązanie:**
1. Sprawdź dzienniki systemowe, aby uzyskać szczegóły błędu
2. Potwierdź, że katalog danych ma uprawnienia do zapisu
3. Uruchom ponownie system i spróbuj ponownie

## Powiązane funkcje

- 🤖 [Zarządzanie byciami krzemowymi](being-management) - Tworzenie i zarządzanie byciami krzemowymi
- 💬 [System czatu](chat-system) - Rozmowa z byciami krzemowymi
- 🧠 [Konfiguracja klienta AI](ai-clients) - Konfiguracja usługi AI
- 📝 [System pamięci](memory) - Zarządzanie historią rozmów
";

    public override string AIClients => @"
# Konfiguracja klienta AI

## Przegląd

Klient AI to ""łącznik mózgu"" bycia krzemowego, odpowiedzialny za komunikację z modelami sztucznej inteligencji. System obsługuje wiele klientów AI, możesz wybrać odpowiednią usługę zgodnie ze swoimi potrzebami.

## Obsługiwani klienci AI

System obsługuje wiele klientów AI, możesz wybrać odpowiednią usługę zgodnie ze swoimi potrzebami.

### Lokalny klient AI

**Cechy:**
- 🏠 **Lokalne uruchomienie**: Model AI działa na Twoim komputerze
- 🔒 **Prywatność i bezpieczeństwo**: Dane nie są przesyłane do chmury
- 💰 **Całkowicie bezpłatne**: Brak jakichkolwiek limitów użytkowania
- ⚡ **Szybka odpowiedź**: Brak opóźnień sieciowych (model lokalny)

**Odpowiednie scenariusze:**
- Zwracanie uwagi na ochronę prywatności
- Posiadanie dobrego komputera (zalecane 16 GB+ RAM)
- Chęć nielimitowanego korzystania z AI
- Słabe warunki sieciowe

**Wymagania konfiguracyjne:**
- Wymaga instalacji odpowiedniego oprogramowania usługi AI
- Wymaga pobrania plików modeli AI (zazwyczaj 4-20 GB)
- Zalecane 16 GB+ RAM, dedykowana karta graficzna jest plusem

### Klient AI w chmurze

**Cechy:**
- ☁️ **Usługa chmurowa**: Model AI działa na zdalnym serwerze
- 🚀 **Potężna funkcjonalność**: Możliwość użycia bardzo dużych modeli (np. 200B+ parametrów)
- 💳 **Płatność za użycie**: Darmowy limit, po przekroczeniu opłata według zużycia
- 🌍 **Obsługa wielu regionów**: Możliwość wyboru serwera blisko Ciebie

**Odpowiednie scenariusze:**
- Słaba konfiguracja komputera
- Potrzeba użycia zaawansowanych modeli
- Sporadyczne użytkowanie, małe zużycie
- Chęć szybkiego rozpoczęcia bez konfiguracji środowiska lokalnego

**Wymagania konfiguracyjne:**
- Wymaga konta i klucza API odpowiedniej platformy
- Wymaga połączenia sieciowego
- Obowiązuje limit zużycia (może się odnawiać)

## Jak wybrać klienta AI?

### Schemat wyboru

```
Jaką masz konfigurację komputera?
├─ Wysoka (16 GB+ RAM)
│  └─ Zależy Ci na prywatności?
│     ├─ Tak → Wybierz klienta lokalnego (np. Ollama)
│     └─ Nie → Oba się sprawdzą
└─ Niska (8 GB lub mniej)
   └─ Wybierz klienta w chmurze (np. DashScope)
```

### Tabela porównawcza

| Cecha | Klient lokalny | Klient w chmurze |
|-------|----------------|------------------|
| Trudność instalacji | Średnia (wymaga instalacji oprogramowania i modeli) | Prosta (wymaga tylko klucza API) |
| Koszt eksploatacji | Bezpłatny (zużywa prąd) | Darmowy limit, po przekroczeniu opłata |
| Ochrona prywatności | ⭐⭐⭐⭐⭐ Całkowicie lokalne | ⭐⭐⭐ Dane przechodzą przez chmurę |
| Wybór modeli | Ograniczony konfiguracją komputera | Dostępne różne duże modele |
| Wymagania sieciowe | Tylko przy pobieraniu modeli | Zawsze wymagane |
| Szybkość odpowiedzi | Szybka (lokalnie) | Zależy od sieci |
| Limity użytkowania | Bez limitów | Obowiązuje limit zużycia |

## Konfiguracja klienta AI

### Krok 1: Przejdź do strony konfiguracji

1. Otwórz system Silicon Life Collective
2. Kliknij **⚙ Konfiguracja** w górnym pasku nawigacji

### Krok 2: Wybierz typ klienta AI

1. Na stronie konfiguracji znajdź opcję **""Typ klienta AI""**
2. Z rozwijanego menu wybierz żądanego klienta:
   - Klient lokalny (np. `OllamaClient`)
   - Klient w chmurze (np. `DashScopeClient`)

### Krok 3: Wypełnij informacje konfiguracyjne

Po wybraniu klienta poniżej pojawią się odpowiednie pola konfiguracyjne:

#### Konfiguracja klienta lokalnego (np. Ollama)

| Pole | Opis | Wartość domyślna | Przykład |
|------|------|------------------|----------|
| **endpoint** | Adres usługi AI | Zależy od usługi | np. `http://localhost:11434` |
| **model** | Nazwa używanego modelu | Zależy od usługi | np. `qwen3.5:8b` |
| **temperature** | Poziom kreatywności (0-1) | `0.7` | `0.5` bardziej zachowawczy, `0.9` bardziej kreatywny |
| **maxTokens** | Maksymalna długość odpowiedzi | `2048` | `4096` pozwala na dłuższe odpowiedzi |

**Przykład konfiguracji:**
```
Typ klienta AI: Typ klienta lokalnego (np. OllamaClient)
endpoint: http://localhost:11434 (wypełnij zgodnie z rzeczywistym adresem usługi)
model: qwen3.5:8b (wypełnij zgodnie z pobranym modelem)
temperature: 0.7
maxTokens: 2048
```

#### Konfiguracja klienta w chmurze (np. DashScope)

| Pole | Opis | Wartość domyślna | Przykład |
|------|------|------------------|----------|
| **apiKey** | Klucz API | Brak | Podany przez platformę |
| **region** | Region serwera | Zależy od platformy | np. `beijing` |
| **model** | Używany model | Zależy od platformy | np. `qwen3.6-plus` |
| **temperature** | Poziom kreatywności (0-1) | `0.7` | `0.5` |
| **maxTokens** | Maksymalna długość odpowiedzi | `2048` | `4096` |

**Dostępne regiony:**

| Kod regionu | Lokalizacja | Dla kogo |
|-------------|-------------|----------|
| `beijing` | Pekin, Chiny | Użytkownicy z Chin kontynentalnych (zalecane) |
| `singapore` | Singapur | Użytkownicy z Azji Południowo-Wschodniej |
| `hongkong` | Hongkong, Chiny | Użytkownicy z Hongkongu, Makau i Tajwanu |
| `virginia` | USA | Użytkownicy z Ameryki Północnej |
| `frankfurt` | Niemcy | Użytkownicy z Europy |

**Dostępne modele:**

| Nazwa modelu | Cechy | Odpowiednie scenariusze |
|-------------|-------|------------------------|
| `qwen3.6-plus` | Zrównoważona wydajność (zalecany) | Codzienne użytkowanie |
| `qwen3-max` | Największe możliwości | Złożone zadania |
| `qwen3.6-flash` | Szybka odpowiedź | Proste pytania i odpowiedzi |
| `qwen-max` | Flagowiec poprzedniej generacji | Złożone rozumowanie |
| `qwen-plus` | Wzmocniony poprzedniej generacji | Scenariusze ogólne |
| `qwen-turbo` | Szybki poprzedniej generacji | Proste zadania |
| `qwen3-coder-plus` | Do programowania | Generowanie kodu |
| `qwq-plus` | Do rozumowania | Matematyka, logika |
| `deepseek-v3.2` | Model firm trzecich | Scenariusze ogólne |
| `deepseek-r1` | Model rozumujący | Głębokie myślenie |
| `glm-5.1` | Model Zhipu | Scenariusze chińskie |
| `kimi-k2.5` | Długi kontekst | Przetwarzanie długich tekstów |
| `llama-4-maverick` | Model Meta | Scenariusze angielskie |

**Przykład konfiguracji:**
```
Typ klienta AI: Typ klienta w chmurze (np. DashScopeClient)
apiKey: Twój klucz API (uzyskaj z odpowiedniej platformy)
region: beijing (wybierz najbliższy region)
model: qwen3.6-plus (wybierz spośród dostępnych modeli)
temperature: 0.7
maxTokens: 2048
```

### Krok 4: Zapisz konfigurację

1. Po wypełnieniu wszystkich niezbędnych informacji
2. Kliknij przycisk **""Zapisz konfigurację""** na dole strony
3. System wyświetli komunikat o pomyślnym zapisaniu

### Krok 5: Przetestuj połączenie

1. Kliknij **💬 Czat** w górnym pasku nawigacji
2. Wybierz bycie krzemowe
3. Wyślij wiadomość testową, np. ""Cześć""
4. Jeśli otrzymasz odpowiedź, konfiguracja zakończyła się sukcesem

## Często zadawane pytania

### P1: Nie wiem, którego klienta wybrać?

**Zalecenie:**
- Jeśli jesteś początkującym, zalecamy zacząć od **klienta w chmurze** - prosta konfiguracja, szybki start
- Jeśli zależy Ci na prywatności lub masz dobry komputer, wybierz **klienta lokalnego**

### P2: Czy można używać dwóch klientów jednocześnie?

Nie. System może używać tylko jednego klienta AI naraz. Możesz jednak w dowolnym momencie przełączyć się na stronie konfiguracji.

### P3: Czy po przełączeniu klienta historia czatu zostanie utracona?

Nie. Historia czatu jest zapisywana w systemie i nie zależy od klienta AI. Po przełączeniu nadal można przeglądać poprzednie rozmowy.

### P4: Klient lokalny zgłasza błąd połączenia?

**Rozwiązanie:**
1. Upewnij się, że odpowiednie oprogramowanie usługi AI jest uruchomione (sprawdź zasobnik systemowy lub procesy)
2. Sprawdź, czy adres endpoint jest poprawny (zobacz domyślną konfigurację usługi)
3. Odwiedź adres usługi w przeglądarce - powinieneś zobaczyć odpowiedź
4. Potwierdź, że model został pobrany: użyj polecenia odpowiedniej usługi, aby wyświetlić listę modeli

### P5: Klient w chmurze zgłasza błąd uwierzytelniania?

**Rozwiązanie:**
1. Sprawdź, czy klucz API jest poprawny (zobacz wymagania formatu odpowiedniej platformy)
2. Potwierdź, że konto nie jest zadłużone
3. Sprawdź, czy nie przekroczono limitu zużycia
4. Wygeneruj ponownie klucz API i zaktualizuj konfigurację

### P6: Jak uzyskać klucz API klienta w chmurze?

**Ogólne kroki:**
1. Odwiedź konsolę internetową dostawcy usługi AI
2. Zaloguj się na swoje konto
3. Przejdź do strony ""Zarządzanie kluczami"" lub ""Zarządzanie API""
4. Kliknij ""Utwórz klucz"" lub ""Generuj klucz API""
5. Skopiuj wygenerowany klucz API i zapisz go w bezpiecznym miejscu

### P7: Odpowiedź jest bardzo wolna, co zrobić?

**Klient lokalny:**
- Sprawdź wykorzystanie zasobów komputera (CPU, RAM, GPU)
- Spróbuj użyć mniejszego modelu (np. 8B zamiast 32B)
- Zamknij inne programy zużywające zasoby

**Klient w chmurze:**
- Sprawdź jakość połączenia sieciowego
- Spróbuj wybrać bliższy region serwera
- Unikaj godzin szczytu sieciowego

### P8: Co to jest parametr temperature?

Temperature kontroluje kreatywność odpowiedzi AI:
- **0.0-0.3**: Bardzo zachowawczy, odpowiedzi przewidywalne, odpowiednie dla pytań faktograficznych
- **0.4-0.7**: Tryb zrównoważony, odpowiedni do codziennych rozmów (zalecane)
- **0.8-1.0**: Bardzo kreatywny, różnorodne odpowiedzi, odpowiedni do pisania kreatywnego

### P9: Jaką wartość maxTokens ustawić?

- **1024**: Krótkie odpowiedzi, odpowiednie do prostych pytań
- **2048**: Średnia długość, odpowiednia do ogólnych rozmów (zalecane)
- **4096+**: Długie odpowiedzi, odpowiednie do złożonych zadań lub generowania długich tekstów

Uwaga: Im większa wartość, tym więcej zasobów i czasu jest zużywane.

### P10: Czy różne bycia mogą używać różnych klientów?

Tak. Każde bycie krzemowe może mieć niezależnie skonfigurowany typ klienta AI.

**Metoda konfiguracji:**
1. Przejdź do strony zarządzania byciami
2. Wybierz bycie do skonfigurowania
3. W konfiguracji tego bycia ustaw AIClientType
4. Jeśli bycie nie ma ustawienia, używana jest konfiguracja globalna

## Najlepsze praktyki

### 1. Wybieraj model odpowiednio do zadania

- **Codzienne rozmowy**: Używaj małych i średnich modeli (8B-14B lub szybkie modele)
- **Złożona analiza**: Używaj dużych modeli (32B+ lub modele zaawansowane)
- **Generowanie kodu**: Używaj dedykowanych modeli programistycznych
- **Pisanie kreatywne**: Używaj wyższego temperature (0.8-0.9)

### 2. Optymalizuj koszty (klient w chmurze)

- Regularnie sprawdzaj zużycie, aby uniknąć przekroczenia darmowego limitu
- Do prostych zadań używaj szybkich modeli
- Do złożonych zadań używaj zaawansowanych modeli
- Rozsądnie ustawiaj maxTokens, aby uniknąć marnotrawstwa

### 3. Zwiększ wydajność (klient lokalny)

- Najpierw pobierz często używane modele, aby uniknąć pobierania w czasie działania
- Utrzymuj usługę AI stale uruchomioną, aby skrócić czas startu
- Używaj akceleracji GPU (jeśli masz kartę NVIDIA)
- Regularnie usuwaj nieużywane modele, aby zwolnić miejsce

### 4. Zalecenia bezpieczeństwa

- Nie udostępniaj klucza API innym osobom
- Regularnie zmieniaj klucz API
- Klient lokalny jest dostępny tylko lokalnie, nie wystawiaj go do sieci publicznej
- Twórz kopie zapasowe ważnych plików konfiguracyjnych

## Uzyskaj pomoc

W przypadku problemów:
1. Zobacz sekcję [Często zadawane pytania](#często-zadawane-pytania)
2. Zobacz [Zarządzanie konfiguracją](./config), aby poznać ustawienia systemu
3. Sprawdź dzienniki systemowe, aby uzyskać szczegóły błędów
";

  public override string BailianDashScope => @"
# Przewodnik po platformie Alibaba Cloud Bailian

## Czym jest Alibaba Cloud Bailian?

Alibaba Cloud Bailian (DashScope) to platforma usług dużych modeli dostarczana przez Alibaba Cloud, oferująca wiele wysokiej jakości modeli AI, w tym Qwen, DeepSeek, GLM, Kimi i inne.

**Zalety:**
- Wysoka inteligencja modeli (nawet do kilkuset B)
- Brak wymagań sprzętowych lokalnie, działanie w chmurze
- Obsługa wielu topowych modeli AI
- Płatność według zużycia, kontrolowane koszty
- Kompatybilny z formatem OpenAI API

## Rejestracja i aktywacja usługi

### Krok 1: Rejestracja konta Alibaba Cloud

1. Odwiedź stronę Alibaba Cloud: https://www.aliyun.com
2. Kliknij ""Bezpłatna rejestracja""
3. Postępuj zgodnie z instrukcjami (obsługa numeru telefonu, rejestracja e-mail)
4. Ukończ weryfikację tożsamości (wymaga Alipay lub karty bankowej)

### Krok 2: Aktywacja usługi Bailian

1. Zaloguj się do konsoli Alibaba Cloud
2. Wyszukaj ""Bailian"" lub ""DashScope""
3. Wejdź na stronę produktu Bailian
4. Kliknij ""Aktywuj teraz""
5. Przeczytaj i zaakceptuj umowę o świadczenie usług
6. Ukończ aktywację

### Krok 3: Uzyskanie klucza API

1. Wejdź do konsoli Bailian
2. W lewym menu znajdź ""Zarządzanie kluczami API"" lub ""Zarządzanie kluczami""
3. Kliknij ""Utwórz klucz API""
4. Nazwij klucz (np. ""SiliconLife"")
5. Skopiuj i zapisz klucz API (**wyświetlany tylko raz, zapisz go bezpiecznie**)

## Konfiguracja Bailian w Silicon Life

### Kroki konfiguracji

1. Otwórz system Silicon Life
2. Przejdź do strony **⚙ Konfiguracja**
3. Znajdź ""Typ klienta AI"", wybierz `DashScopeClient`
4. W ""Konfiguracja AI"" wypełnij:
   - **API Key**: wklej skopiowany klucz API
   - **Region**: wybierz region serwera (np. beijing)
   - **Model**: wybierz model (**po wypełnieniu klucza API i wyborze regionu, system automatycznie pobierze wszystkie dostępne modele dla tego regionu**)
5. Zapisz konfigurację

**Wskazówka:**
- Najpierw musisz wypełnić klucz API i wybrać region, aby załadować listę modeli
- Jeśli ładowanie listy modeli nie powiedzie się, wyświetlana jest lista zalecanych modeli

### Wybór regionu

| Region | Lokalizacja | Opis |
|------|------|------|
| **beijing** | Pekin, Chiny | Domyślnie zalecany, szybki dostęp z Chin |
| **virginia** | Wirginia, USA | Dla użytkowników zagranicznych |
| **singapore** | Singapur | Region Azji i Pacyfiku |
| **hongkong** | Hongkong, Chiny | Region Azji i Pacyfiku |
| **frankfurt** | Frankfurt, Niemcy | Region Europy |

**Zalecenia dotyczące wyboru regionu:**
- **Użytkownicy z Chin kontynentalnych**: wybierz beijing (Pekin), najszybszy dostęp
- **Potrzeba wysokiej jakości tłumaczeń**: wybierz singapore lub hongkong, lepsze efekty tłumaczenia
- **Użytkownicy zagraniczni**: wybierz najbliższy region

### Wybór modelu

**Zalecane modele:**

| Nazwa modelu | Inteligencja | Cechy | Odpowiednie scenariusze |
|---------|------|------|---------|
| **qwen3-max** | Bardzo duża | Najpotężniejsza wersja Qwen | Złożone rozumowanie, zadania profesjonalne |
| **qwen3.6-plus** | Duża | Zrównoważona wydajność i koszt | Codzienne użytkowanie (zalecane) |
| **qwen3.6-flash** | Średnia | Szybka, niski koszt | Szybkie odpowiedzi |
| **deepseek-v3.2** | Bardzo duża | Najnowsza wersja DeepSeek | Silne zdolności ogólne |
| **deepseek-r1** | Bardzo duża | Specjalizacja w rozumowaniu | Złożone rozumowanie |
| **glm-5.1** | Duża | Model Zhipu AI | Scenariusze chińskie |
| **kimi-k2.5** | Duża | Model Moonshot | Przetwarzanie długich tekstów |

**Zalecenie dla użytkowników polskich: qwen3.6-plus lub qwen3-max**

## Informacje o kosztach

### Model rozliczeń

Platforma Bailian stosuje model **płatności według zużycia**:
- Rozliczane według liczby tokenów wejściowych
- Różne modele mają różne ceny
- Im wyższa inteligencja modelu, tym wyższa cena

### Darmowy limit

- Nowi użytkownicy zazwyczaj otrzymują darmowy limit próbny
- Niektóre modele mają darmowy limit wywołań
- Szczegółowe limity zgodnie z ogłoszeniami platformy Bailian

### Jak kontrolować koszty

- Wybierz odpowiedni model (niekoniecznie najdroższy)
- Unikaj wysyłania zbyt długich tekstów
- Regularnie sprawdzaj zużycie, kontroluj koszty

## Często zadawane pytania

### P: Gdzie uzyskać klucz API?

**O:**
1. Zaloguj się do konsoli Alibaba Cloud Bailian
2. Znajdź ""Zarządzanie kluczami API""
3. Utwórz nowy klucz API
4. Skopiuj i zapisz bezpiecznie

### P: Co zrobić, jeśli klucz API został ujawniony?

**O:**
1. Natychmiast zaloguj się do konsoli Bailian
2. Usuń ujawniony klucz API
3. Utwórz nowy klucz API
4. Zaktualizuj konfigurację w Silicon Life

### P: Który region wybrać?

**O:**
- **Użytkownicy z Chin**: wybierz beijing (Pekin), najszybszy
- **Użytkownicy zagraniczni**: wybierz najbliższy region
- Region nie wpływa na jakość modelu, tylko na szybkość dostępu

### P: Dlaczego ładowanie listy modeli nie powiodło się?

**Możliwe przyczyny:**
1. Klucz API jest nieprawidłowy lub wygasł
2. Problemy z połączeniem sieciowym
3. Awaria usługi Bailian

**Rozwiązanie:**
1. Sprawdź, czy klucz API jest poprawny
2. Sprawdź połączenie sieciowe
3. Spróbuj ponownie później

### P: Bailian vs Ollama - porównanie

| Cecha | Bailian (DashScope) | Ollama |
|------|------------------|--------|
| Miejsce działania | Chmura | Lokalny komputer |
| Wymagania sprzętowe | Brak | Wymaga wyższej konfiguracji |
| Rozmiar modelu | Do kilkuset B | Zazwyczaj 4B-70B |
| Koszty | Płatność według zużycia | Darmowe |
| Połączenie z siecią | Wymagane | Po pobraniu nie wymagane |
| Prywatność | Dane wysyłane do chmury | Całkowicie lokalne |

Życzymy przyjemnego użytkowania!
";

  public override string VolcengineArk => @"
# Przewodnik po platformie Volcengine Ark

## Czym jest Volcengine Ark?

Volcengine Ark to platforma usług dużych modeli od ByteDance (Volcengine), oferująca modele serii Doubao (Seed) oraz wiele modeli innych firm (DeepSeek, GLM, Kimi itp.).

**Zalety:**
- Flagowe modele Doubao Seed o bardzo wysokiej inteligencji (kontekst 256K)
- Brak wymagań sprzętowych lokalnie, działanie w chmurze
- Obsługa wielu topowych modeli AI
- Płatność według zużycia, bardzo konkurencyjne ceny
- W pełni kompatybilny z formatem OpenAI API
- Krajowa platforma, stabilny dostęp bez specjalnej sieci

## Rejestracja i aktywacja usługi

### Krok 1: Rejestracja konta Volcengine

1. Odwiedź stronę Volcengine: https://console.volcengine.com
2. Kliknij ""Bezpłatna rejestracja""
3. Postępuj zgodnie z instrukcjami (obsługa numeru telefonu, rejestracja e-mail)
4. Ukończ weryfikację tożsamości

### Krok 2: Aktywacja usługi Ark

1. Zaloguj się do konsoli Volcengine
2. W lewym menu znajdź ""Ark"" lub wyszukaj ""Ark""
3. Wejdź na stronę produktu Ark
4. Kliknij ""Aktywuj teraz""
5. Przeczytaj i zaakceptuj umowę o świadczenie usług

### Krok 3: Utworzenie punktu dostępowego wnioskowania

Volcengine Ark używa ""punktów dostępowych wnioskowania"" (Endpoint) do identyfikacji modeli, a nie bezpośrednio nazw modeli.

1. Przejdź do strony ""Punkty dostępowe wnioskowania"" w konsoli Ark
2. Kliknij ""Utwórz punkt dostępowy wnioskowania""
3. Wybierz model, którego chcesz użyć (np. doubao-seed-1-6-251015)
4. Nazwij punkt dostępowy
5. Po utworzeniu skopiuj ID punktu dostępowego (format np. ep-20241212123456-abcde)

### Krok 4: Uzyskanie klucza API

1. W konsoli Ark znajdź ""Zarządzanie kluczami API""
2. Kliknij ""Utwórz klucz API""
3. Nazwij klucz (np. ""SiliconLife"")
4. Skopiuj i zapisz klucz API (**wyświetlany tylko raz, zapisz go bezpiecznie**)

## Konfiguracja Volcengine Ark w Silicon Life

### Kroki konfiguracji

1. Otwórz system Silicon Life
2. Przejdź do strony **⚙ Konfiguracja**
3. Znajdź ""Typ klienta AI"", wybierz `VolcengineArkClientFactory`
4. W ""Konfiguracja AI"" wypełnij:
   - **API Key**: wklej skopiowany klucz API
   - **ID punktu dostępowego wnioskowania**: wklej ID utworzonego punktu dostępowego (np. ep-20241212123456-abcde)
   (**Po wypełnieniu klucza API, system automatycznie pobierze listę punktów dostępowych**)
5. Zapisz konfigurację

### Popularne modele

| Seria modeli | ID modelu | Cechy |
|---------|---------|------|
| **Doubao-Seed-1.8** | doubao-seed-1-8-YYMMDD | Najnowszy flagowy, silniejsze zdolności Agent |
| **Doubao-Seed-1.6** | doubao-seed-1-6-251015 | Model ogólny, zalecany do codziennego użytku |
| **Doubao-Seed-1.6-Flash** | doubao-seed-1-6-flash | Wersja o bardzo niskich opóźnieniach |
| **Doubao-Seed-1.6-Lite** | doubao-seed-1-6-lite | Wersja lekka, wysoki stosunek jakości do ceny |
| **Doubao-Seed-Code** | doubao-seed-code-preview-latest | Model specjalistyczny do programowania |

## Informacje o kosztach

### Model rozliczeń

Volcengine Ark stosuje model **płatności według zużycia**:
- Rozliczane według liczby tokenów wejściowych
- Seria Doubao Seed ma ujednolicone ceny

### Darmowy limit

- Rejestracja zapewnia 500 000 tokenów darmowego wnioskowania na każdy model (wymaga weryfikacji tożsamości)
- Darmowy limit ważny 30 dni
- Obsługa ""trybu bezpiecznego"", zużywa tylko darmowy limit, po wyczerpaniu automatycznie wstrzymuje

## Często zadawane pytania

### P: Czym jest punkt dostępowy wnioskowania?

**O:** Punkt dostępowy wnioskowania (Endpoint) to unikalna koncepcja Volcengine Ark. Musisz najpierw utworzyć punkt dostępowy w konsoli Ark, wybrać model do użycia, a system przypisze ID punktu dostępowego. W konfiguracji Silicon Life wystarczy wpisać to ID, aby wywołać odpowiedni model.

### P: Volcengine Ark vs Bailian - porównanie

| Cecha | Volcengine Ark | Bailian (DashScope) |
|------|----------------|------------------|
| Dostawca | ByteDance | Alibaba Cloud |
| Flagowy model | Seria Doubao Seed | Seria Qwen |
| Wywoływanie modeli | Przez punkt dostępowy | Bezpośrednio nazwa modelu |
| Cena | Bardzo wysoki stosunek jakości do ceny | Średni |
| Darmowy limit | 500 000 tokenów na model | Darmowy próbny |

Życzymy przyjemnego użytkowania!
";

    public override string AuditLog => @"
# Dziennik audytów

## Przegląd

Dziennik audytów (Audit Log) to **system monitorowania zużycia Tokenów** platformy Silicon Life, pomagający śledzić i zarządzać zużyciem wywołań AI wszystkich być krzemowych.

Dzięki dziennikowi audytów możesz:
- 📊 **Przeglądać statystyki zużycia Tokenów**: Dowiedzieć się, ile Tokenów zużyło każde bycie krzemowe
- 📈 **Analizować trendy zużycia**: Przeglądać zmiany zużycia Tokenów według godzin, dni, miesięcy
- 🔍 **Filtrować i porównywać**: Filtrować dane według zakresu czasu, bycia krzemowego, typu klienta AI
- 💾 **Eksportować dane**: Eksportować dane audytu do pliku CSV do dalszej analizy

**Co to jest Token?**  
Token to podstawowa jednostka przetwarzania tekstu przez model AI. Każde wywołanie AI przez bycie krzemowe zużywa Tokeny:
- **Prompt Tokens**: Tekst wejściowy wysyłany do AI (w tym plik Duszy, historia, wiadomości użytkownika itp.)
- **Completion Tokens**: Tekst odpowiedzi wygenerowany przez AI
- **Total Tokens**: Łączne zużycie Tokenów (Prompt + Completion)

## Dostęp do dziennika audytów

### Przejście do strony audytu

1. Uruchom platformę Silicon Life
2. Kliknij ""Audyt"" (ikona 📊) w lewym menu nawigacji
3. Przejdź do panelu dziennika audytów

### Wymagania uprawnień

- 📊 **Przeglądanie dziennika audytów**: Wymaga uprawnień Kuratora krzemowego (Curator)
- 🔒 Zwykłe bycia krzemowe nie mają dostępu do funkcji audytu
- Ma to na celu zapewnienie, że tylko administratorzy mogą przeglądać globalne zużycie Tokenów

## Funkcje panelu audytów

### 📈 Wykresy trendów

Góra strony audytu wyświetla **wykres trendów zużycia Tokenów**, obejmujący:

- **Selektor zakresu czasu**:
  - Dzisiaj (Today): Wyświetla zużycie Tokenów dzisiaj według godzin
  - Ten tydzień (Week): Wyświetla trend zużycia Tokenów w tym tygodniu
  - Ten miesiąc (Month): Wyświetla zużycie Tokenów w tym miesiącu według dni
  - Ten rok (Year): Wyświetla zużycie Tokenów w tym roku według miesięcy

- **Dane trendów**:
  - Wykres liniowy pokazuje zmiany zużycia Tokenów w czasie
  - Najechanie na punkt danych wyświetla konkretną wartość
  - Pomaga odkryć szczyty i dołki zużycia

### 📊 Podsumowanie statystyk

Panel wyświetla kluczowe dane statystyczne:

- **Łączna liczba żądań**: Łączna liczba wywołań AI
- **Liczba sukcesów/porażek**: Ile wywołań AI zakończyło się sukcesem, a ile porażką
- **Zużycie Tokenów**:
  - Łączna liczba Tokenów wejściowych (Prompt Tokens)
  - Łączna liczba Tokenów wyjściowych (Completion Tokens)
  - Łączne zużycie Tokenów (Total Tokens)

### 🔍 Funkcja filtrowania

Możesz używać filtrów do precyzyjnego przeglądania określonych danych:

- **Filtruj według bycia krzemowego**: Zobacz zużycie Tokenów określonego bycia
- **Filtruj według klienta AI**: Zobacz zużycie określonej usługi AI (np. Ollama, Alibaba Cloud Bailian)
- **Filtrowanie kombinowane**: Używaj wielu filtrów jednocześnie

### 📋 Szczegółowa klasyfikacja

#### Statystyki według typu klienta AI

Wyświetla zużycie Tokenów każdego klienta AI:
- OllamaClient (lokalna usługa Ollama)
- DashScopeClient (Alibaba Cloud Bailian)
- Inni skonfigurowani klienci AI

Pomaga zrozumieć:
- Która usługa AI jest używana najczęściej
- Porównanie użycia modeli lokalnych i chmurowych
- Czy trzeba dostosować konfigurację klienta AI

#### Statystyki według być krzemowych

Wyświetla zużycie Tokenów każdego bycia krzemowego:
- Nazwa bycia krzemowego
- Łączne zużycie Tokenów
- Liczba żądań

Pomaga odkryć:
- Które bycia krzemowe są najbardziej aktywne
- Czy zużycie Tokenów jest rozsądne
- Czy trzeba dostosować częstotliwość użycia niektórych być krzemowych

### 💾 Eksport danych

Klikając przycisk ""Eksportuj CSV"", możesz:

- Wyeksportować wszystkie dane audytu w bieżących warunkach filtrowania do pliku CSV
- Plik CSV zawiera:
  - Znacznik czasu
  - ID bycia krzemowego
  - Typ klienta AI
  - Liczba Tokenów wejściowych
  - Liczba Tokenów wyjściowych
  - Łączna liczba Tokenów
  - Czy zakończone sukcesem

**Scenariusze użycia**:
- Dokonuj głębszej analizy danych w Excelu
- Twórz niestandardowe raporty
- Przechowuj długoterminowo rekordy audytu
- Dziel się danymi z innymi członkami zespołu

## Scenariusze użycia

### Scenariusz 1: Monitorowanie zużycia Tokenów

**Potrzeba**: Chcesz poznać łączne zużycie usługi AI w tym miesiącu

**Kroki**:
1. Przejdź do strony audytu
2. Wybierz zakres czasu ""Ten miesiąc""
3. Zobacz statystyki ""Łączne zużycie Tokenów""
4. Zobacz wykres trendów, aby poznać codzienne zmiany zużycia

**Wynik**: Możesz jasno zobaczyć, ile Tokenów zużyto w tym miesiącu i które dni miały najwyższe zużycie.

### Scenariusz 2: Badanie nietypowego zużycia

**Potrzeba**: Zauważyłeś nienormalnie wysokie koszty usługi AI i chcesz znaleźć przyczynę

**Kroki**:
1. Przejdź do strony audytu
2. Wybierz zakres czasu z okresu nietypowych kosztów
3. Zobacz statystyki według być krzemowych
4. Znajdź bycie krzemowe z najwyższym zużyciem
5. Kliknij to bycie, aby zobaczyć szczegółowy trend
6. W razie potrzeby wyeksportuj CSV do głębszej analizy

**Wynik**: Możesz zidentyfikować, które bycie krzemowe zużyło dużo Tokenów i kiedy, a następnie sprawdzić, czy jego plik Duszy lub konfiguracja zadań są odpowiednie.

### Scenariusz 3: Porównanie różnych usług AI

**Potrzeba**: Chcesz porównać zużycie lokalnego Ollama i Alibaba Cloud Bailian

**Kroki**:
1. Przejdź do strony audytu
2. Wybierz zakres czasu ""Ten miesiąc""
3. Zobacz statystyki według typu klienta AI
4. Porównaj zużycie Tokenów OllamaClient i DashScopeClient

**Wynik**: Możesz zrozumieć:
- Proporcję użycia modeli lokalnych i chmurowych
- Która usługa jest bardziej ekonomiczna
- Czy trzeba dostosować domyślnego klienta AI

### Scenariusz 4: Monitorowanie określonego bycia krzemowego

**Potrzeba**: Utworzyłeś nowego asystenta programowania i chcesz monitorować jego zużycie Tokenów

**Kroki**:
1. Przejdź do strony audytu
2. W filtrze wybierz ID lub nazwę tego asystenta programowania
3. Zobacz jego trend zużycia Tokenów
4. Porównaj z zużyciem innych być krzemowych

**Wynik**: Możesz ocenić wydajność tego asystenta programowania i ustalić, czy trzeba zoptymalizować jego plik Duszy lub sposób pracy.

## Narzędzie audytu (wywołanie AI)

Bycia krzemowe mogą również używać narzędzia **TokenAuditTool** do zapytań o zużycie Tokenów:

### Zapytanie o podsumowanie Tokenów

**Przykład polecenia**:
```
Zapytaj o zużycie Tokenów w tym miesiącu
```

**Zwracane informacje**:
- Łączna liczba żądań
- Liczba sukcesów/porażek
- Liczba Tokenów wejściowych/wyjściowych/łącznie
- Statystyki według typu klienta AI
- Statystyki według bycia krzemowego

### Zapytanie o trendy zużycia

**Przykład polecenia**:
```
Pokaż trend zużycia Tokenów w tym tygodniu
```

**Zwracane informacje**:
- Zużycie Tokenów według dni lub godzin
- Pomaga zrozumieć wzorce zużycia

### Parametry narzędzia

TokenAuditTool obsługuje następujące parametry:

- `time_range`: Zakres czasu (today/week/month/year)
- `client_type`: Typ klienta AI (opcjonalnie)
- `being_id`: ID bycia krzemowego (opcjonalnie)

**Uwaga**: Jest to narzędzie `[SiliconManagerOnly]`, tylko Kurator krzemowy może go używać.

## Opis danych

### Zawartość rekordów

Każde wywołanie AI rejestruje następujące informacje:

- **Znacznik czasu**: Czas wywołania AI (UTC)
- **ID bycia krzemowego**: Bycie, które zainicjowało wywołanie
- **Typ klienta AI**: Używana usługa AI (np. OllamaClient)
- **Liczba Tokenów wejściowych**: Liczba Tokenów tekstu wysłanego do AI
- **Liczba Tokenów wyjściowych**: Liczba Tokenów tekstu odpowiedzi AI
- **Łączna liczba Tokenów**: Wejściowe + wyjściowe
- **ID sesji**: Sesja czatu, do której należy wywołanie
- **Czy zakończone sukcesem**: Czy wywołanie AI zakończyło się sukcesem

### Przechowywanie danych

- Rekordy audytu używają **pamięci masowej z indeksem czasowym** (ITimeStorage)
- Obsługa wydajnych zapytań według zakresu czasu
- Dane są trwale przechowywane, nie są tracone po restarcie

### Objaśnienie obliczania Tokenów

**Co to jest Token?**
- Token to podstawowa jednostka przetwarzania tekstu przez model AI
- Jeden Token odpowiada około 0,75 słowa angielskiego lub 1-2 znakom chińskim
- Różne modele AI mają nieco inne metody obliczania Tokenów

**Dlaczego warto monitorować Tokeny?**
- Usługi AI w chmurze (np. Alibaba Cloud Bailian) pobierają opłaty za Tokeny
- Zrozumienie zużycia Tokenów pomaga kontrolować koszty
- Wykrywanie nietypowych wzorców użycia (np. nieskończone pętle wywołań)
- Optymalizacja konfiguracji być krzemowych i plików Duszy

## Często zadawane pytania

### P: Dlaczego nie widzę strony audytu?

**O**: Funkcja audytu wymaga uprawnień Kuratora krzemowego (Curator). Potwierdź:
- Czy jesteś administratorem platformy lub Kuratorem krzemowym
- Bycia krzemowe nie mają uprawnień do przeglądania dziennika audytów
- W przypadku problemów z uprawnieniami skontaktuj się z administratorem platformy

### P: Dane audytu pokazują ""brak danych""?

**O**: Możliwe przyczyny:
- W wybranym zakresie czasu nie było wywołań AI
- Warunki filtrowania są zbyt restrykcyjne, brak pasujących rekordów
- Bycia krzemowe jeszcze nie rozmawiały z AI

**Rozwiązanie**:
- Spróbuj zmienić zakres czasu (np. wybierz ""Ten miesiąc"" zamiast ""Dzisiaj"")
- Wyczyść warunki filtrowania
- Najpierw przeprowadź kilka rozmów z byciami krzemowymi

### P: Dlaczego zużycie Tokenów nagle wzrosło?

**O**: Możliwe przyczyny:
- Dodano wiele rekordów rozmów (historia się wydłużyła, Prompt Token wzrósł)
- Plik Duszy stał się bardzo złożony
- Pewne bycie krzemowe często wywołuje AI
- Zadania AI lub czasomierze wykonują się w dużych ilościach

**Metoda diagnozy**:
- Użyj filtru, aby zobaczyć, które bycie krzemowe zużywa najwięcej
- Zobacz wykres trendów, aby zidentyfikować konkretny punkt czasowy
- Sprawdź plik Duszy i konfigurację zadań tego bycia krzemowego
- Wyeksportuj CSV do szczegółowej analizy

### P: Czym otworzyć wyeksportowany plik CSV?

**O**: Plik CSV można otworzyć w następujących programach:
- Microsoft Excel
- Google Sheets
- LibreOffice Calc
- Dowolny edytor tekstu (np. Notatnik, VS Code)

### P: Jak długo przechowywane są dane audytu?

**O**: 
- Rekordy audytu są trwale przechowywane w systemie plików
- Domyślnie dane są przechowywane przez cały czas
- Aby wyczyścić stare dane, można ręcznie usunąć pliki audytu z katalogu pamięci masowej
- Zaleca się regularne eksportowanie ważnych danych jako kopii zapasowej

### P: Dlaczego niektóre wywołania AI nie mają rekordów?

**O**: Możliwe przyczyny:
- Klient AI nie zwrócił informacji o zużyciu Tokenów
- Usługa rekordów Tokenów nie została poprawnie zainicjowana
- Wywołanie nie powiodło się i nie wygenerowano statystyk Tokenów

**Uwaga**: Tylko odpowiedzi AI zawierające informacje o Tokenach są rejestrowane.

### P: Czy można usunąć rekordy audytu?

**O**: 
- Bieżąca wersja nie obsługuje usuwania rekordów audytu przez interfejs
- Rekordy audytu są przechowywane w katalogu z indeksem czasowym w systemie plików
- Aby wyczyścić, można ręcznie usunąć pliki audytu z katalogu pamięci masowej
- **Zaleca się najpierw wyeksportowanie kopii zapasowej przed usunięciem**

## Najlepsze praktyki

### 1. Regularnie monitoruj zużycie Tokenów

- 📅 Sprawdzaj panel audytów co tydzień lub miesiąc
- 📊 Zwracaj uwagę na trendy zużycia Tokenów, szybko badaj anomalie
- 💰 Jeśli używasz usług AI w chmurze, śledź relację między kosztami a zużyciem Tokenów

### 2. Używaj funkcji filtrowania do precyzyjnej lokalizacji

- 🔍 Gdy znajdziesz problem, najpierw użyj filtru, aby zawęzić zakres
- 🎯 Filtruj według być krzemowych, aby zidentyfikować konkretne bycie
- 🤖 Filtruj według klienta AI, aby porównać zużycie różnych usług

### 3. Regularnie eksportuj dane

- 💾 Eksportuj dane audytu raz w miesiącu jako kopię zapasową
- 📈 Twórz niestandardowe wykresy i raporty w Excelu
- 📋 Przechowuj dane długoterminowo, aby umożliwić roczne analizy porównawcze

### 4. Optymalizuj zużycie Tokenów

Na podstawie danych audytu optymalizuj:
- 📝 Upraszczaj pliki Duszy (redukuj niepotrzebne ustawienia)
- 🗂️ Kontroluj ilość pamięci (unikaj zbyt długiej historii)
- ⏱️ Dostosuj częstotliwość czasomierzy (zmniejsz niepotrzebne wywołania AI)
- 🎯 Optymalizuj opisy zadań (zwiększ wydajność, zmniejsz powtarzające się wywołania)

### 5. Ustaw alerty zużycia

- 📊 Zwracaj uwagę na nietypowe wahania zużycia Tokenów
- 🔔 Jeśli zauważysz, że pewne bycie krzemowe nagle zużywa dużo Tokenów, szybko sprawdź
- 🛑 W razie potrzeby wstrzymaj to bycie i zbadaj problem

## Powiązane funkcje

- [Zarządzanie byciami krzemowymi](/help/being-management) - Zarządzanie tworzeniem i konfiguracją być krzemowych
- [System zadań](/help/task) - Konfiguracja i zarządzanie zadaniami czasowymi być krzemowych
- [System pamięci](/help/memory) - Zrozumienie, jak pamięć wpływa na zużycie Tokenów
- [Zarządzanie uprawnieniami](/help/permission) - Zrozumienie uprawnień Kuratora krzemowego i kontroli dostępu
- [Zarządzanie konfiguracją](/help/config) - Konfiguracja klienta AI i innych ustawień systemowych
";

    public override string KnowledgeGraph => @"
# Graf Wiedzy

## Przegląd

Graf Wiedzy to **system zarządzania i wizualizacji wiedzy** być krzemowych, prezentujący w sposób graficzny wiedzę, którą bycia krzemowe zdobyły i zgromadziły. Poprzez intuicyjne węzły i połączenia, możesz jasno zobaczyć relacje między elementami wiedzy.

## Co to jest trójka wiedzy?

Podstawową jednostką grafu wiedzy jest **trójka wiedzy**, przyjmująca strukturę ""podmiot-relacja-orzeczenie"":

```
(podmiot) -[relacja]-> (orzeczenie)
```

**Przykłady:**
- `(Jan) -[lubi]-> (jabłka)`
- `(Python) -[jest]-> (język programowania)`
- `(Ziemia) -[krąży wokół]-> (Słońca)`

Ta struktura pozwala byciom krzemowym rozumieć i organizować wiedzę jak ludzie.

## Dostęp do grafu wiedzy

### Lokalizacja wejścia

1. Kliknij ikonę **📚 Baza wiedzy** w lewym pasku nawigacji
2. System automatycznie załaduje i wyświetli stronę wizualizacji grafu wiedzy

### Układ interfejsu

Strona grafu wiedzy zawiera:

- **Nagłówek górny**: Wyświetla ""Wizualizacja grafu wiedzy""
- **Panel statystyk**: Wyświetla liczbę węzłów, krawędzi i inne informacje statystyczne
- **Główny obszar płótna**: Prezentuje wizualizację graficzną grafu wiedzy
- **Wskaźnik ładowania**: Wyświetla komunikat podczas ładowania danych

## Wizualizacja grafu

### Węzły (Nodes)

Węzły reprezentują **encje** w wiedzy (podmioty lub orzeczenia):

- **Węzły kołowe**: Każdy węzeł reprezentuje jedną encję
- **Etykiety węzłów**: Wyświetlają nazwę encji
- **Rozmiar węzłów**: Automatycznie dostosowywany na podstawie liczby połączeń (więcej połączeń = większy węzeł)
- **Kolory węzłów**:
  - Niebieska ramka: Zwykły węzeł
  - Podświetlenie: Po najechaniu myszką

### Krawędzie (Edges)

Krawędzie reprezentują **relacje** w wiedzy (orzeczenia):

- **Linie ze strzałkami**: Wskazują relację od podmiotu do orzeczenia
- **Kolor linii**: Półprzezroczysty niebieski `rgba(100, 150, 255, 0.6)`
- **Znacznik strzałki**: Jasno pokazuje kierunek relacji

### Algorytm układu

Graf wiedzy stosuje **algorytm układu kołowego**:

- Wszystkie węzły są równomiernie rozłożone na orbicie kołowej
- Węzły utrzymują odpowiednie odstępy
- Automatycznie dostosowuje się do rozmiaru płótna

## Funkcje interaktywne

### Operacje myszką

- **Najechanie na węzeł**: Węzeł jest podświetlony, ułatwiając identyfikację
- **Przeglądanie relacji**: Zrozumienie kierunku relacji poprzez strzałki
- **Skalowanie płótna**: Natywna funkcja powiększania przeglądarki (Ctrl + kółko myszy)

### Ładowanie danych

- **Automatyczne ładowanie**: Po wejściu na stronę dane wiedzy są automatycznie ładowane z systemu
- **Odświeżanie w czasie rzeczywistym**: Ponowne odwiedzenie strony pozwala uzyskać najnowszą wiedzę
- **Wskaźnik ładowania**: Podczas ładowania danych wyświetla się ""Ładowanie danych grafu wiedzy...""

## Zarządzanie wiedzą

### Jak powstaje wiedza?

Bycia krzemowe gromadzą wiedzę poprzez:

1. **Nauka z rozmów**: Wyodrębnianie kluczowych informacji z rozmów z użytkownikami
2. **Wywoływanie narzędzi**: Dodawanie wiedzy poprzez narzędzie wiedzy (KnowledgeTool)
3. **Analiza plików**: Wyodrębnianie wiedzy z analizy przesłanych plików
4. **Wykonywanie zadań**: Gromadzenie wiedzy dziedzinowej podczas wykonywania zadań

### Operacje narzędzia wiedzy

Kurator krzemowy lub bycie krzemowe może wykonywać operacje na wiedzy poprzez rozmowę z AI:

**Dodawanie wiedzy:**
```
Dodaj wiedzę: (Python) -[jest]-> (język programowania)
```

**Zapytanie o wiedzę:**
```
Znajdź całą wiedzę o ""Pythonie""
```

**Wyszukiwanie wiedzy:**
```
Wyszukaj wiedzę zawierającą ""programowanie""
```

**Znajdowanie ścieżki relacji:**
```
Znajdź ścieżkę relacji między ""Pythonem"" a ""AI""
```

### Atrybuty wiedzy

Każdy wpis wiedzy zawiera bogate metadane:

- **Unikalne ID**: Unikalny identyfikator każdego wpisu wiedzy
- **Numer wersji**: Obsługa aktualizacji i zarządzania wersjami wiedzy
- **Pewność**: 0.0-1.0, oznacza stopień wiarygodności wiedzy
- **Źródło**: Pochodzenie wiedzy (rozmowa, plik, narzędzie itp.)
- **Kategoria**: Dziedzina lub kategoria, do której należy wiedza
- **Tagi**: Niestandardowe tagi ułatwiające kategoryzację i wyszukiwanie
- **Twórca**: Bycie krzemowe, które dodało tę wiedzę
- **Czas utworzenia**: Znacznik czasowy dodania wiedzy
- **Status weryfikacji**: Niezweryfikowana/Zweryfikowana/Odrzucona

## Informacje statystyczne

Strona grafu wiedzy wyświetla następujące dane statystyczne:

- **Łączna liczba węzłów**: Liczba wszystkich encji w grafie wiedzy
- **Łączna liczba krawędzi**: Liczba relacji między encjami
- **Liczba wpisów wiedzy**: Łączna liczba trójek wiedzy w systemie

Te informacje statystyczne pomagają zrozumieć stopień zgromadzenia wiedzy przez bycia krzemowe.

## Scenariusze użycia

### 1. Wizualizacja wiedzy

Intuicyjne przeglądanie tego, czego bycia krzemowe się nauczyły:

1. Przejdź do strony grafu wiedzy
2. Obserwuj rozkład węzłów i relacje połączeń
3. Zrozum strukturę organizacji wiedzy i powiązania

### 2. Weryfikacja wiedzy

Sprawdzanie wyników nauki być krzemowych:

1. Przejrzyj węzły wiedzy w określonej dziedzinie
2. Zweryfikuj dokładność wiedzy
3. Uzupełnij lub popraw wiedzę poprzez rozmowę

### 3. Eksploracja wiedzy

Odkrywanie powiązań między elementami wiedzy:

1. Obserwuj węzły o wysokim stopniu połączeń (koncepcje rdzeniowe)
2. Śledź ścieżki relacji, odkrywaj pośrednie powiązania
3. Identyfikuj luki w wiedzy, kieruj dalszą naukę

## Często zadawane pytania

### P: Dlaczego graf wiedzy jest pusty?

**O:** Jeśli bycia krzemowe jeszcze nie zdobyły żadnej wiedzy, graf będzie pusty. Możesz dodać wiedzę poprzez:
- Rozmowę z byciem krzemowym, pozwól mu uczyć się nowych informacji
- Przesłanie plików do analizy przez bycie krzemowe
- Bezpośrednie polecenie byciu krzemowemu dodania określonej wiedzy

### P: Czy graf wiedzy aktualizuje się automatycznie?

**O:** Dane grafu wiedzy są przechowywane w czasie rzeczywistym, ale strona wymaga odświeżenia, aby wyświetlić najnowsze dane. Gdy bycie krzemowe zdobędzie nową wiedzę, ponownie odwiedź stronę grafu wiedzy, aby zobaczyć aktualizacje.

### P: Jak przeglądać, gdy jest za dużo węzłów?

**O:** Gdy ilość wiedzy jest duża:
- Użyj funkcji powiększania przeglądarki (Ctrl + kółko myszy)
- Skup się na rdzeniowych węzłach o wysokim stopniu połączeń
- Wyszukaj wiedzę na określony temat za pomocą narzędzia zapytań

### P: Jak usunąć błędną wiedzę?

**O:** Możesz poprosić bycie krzemowe o usunięcie lub poprawienie wiedzy poprzez rozmowę:
```
Usuń błędną wiedzę o ""...""
```
lub
```
Zaktualizuj wiedzę: (stara wiedza) zmień na (nowa wiedza)
```

### P: Gdzie są przechowywane dane grafu wiedzy?

**O:** Dane wiedzy są przechowywane w katalogu danych systemu w formacie JSON. System automatycznie zarządza przechowywaniem, indeksowaniem i zapytaniami wiedzy.

### P: Czy można wyeksportować graf wiedzy?

**O:** Obecnie graf wiedzy służy głównie do wizualizacji online. Aby wyeksportować dane wiedzy, możesz poprosić bycie krzemowe o wygenerowanie raportu za pomocą narzędzia zapytań.

## Najlepsze praktyki

### 1. Regularnie przeglądaj zgromadzoną wiedzę

- Co tydzień przeglądaj graf wiedzy, aby zrozumieć postęp nauki być krzemowych
- Zwracaj uwagę na nowe węzły i relacje
- Weryfikuj dokładność ważnej wiedzy

### 2. Kieruj naukę wiedzy

- Aktywnie dostarczaj wysokiej jakości dane wejściowe wiedzy
- Poprawiaj błędną wiedzę, utrzymuj dokładność bazy wiedzy
- Dodawaj kategorie i tagi do wiedzy, aby ułatwić zarządzanie

### 3. Wykorzystuj graf wiedzy w rozmowach

- Odwołuj się do istniejącej wiedzy w rozmowach
- Proś bycia krzemowe o wnioskowanie na podstawie obecnej wiedzy
- Eksploruj powiązania między elementami wiedzy, odkrywaj nowe spostrzeżenia

### 4. Kontrola jakości wiedzy

- Zwracaj uwagę na oceny pewności wiedzy
- Weryfikuj dokładność kluczowej wiedzy
- Aktualizuj nieaktualną lub błędną wiedzę w odpowiednim czasie

## Powiązana dokumentacja

- [Zarządzanie byciami krzemowymi](/help/being-management) - Zarządzanie tworzeniem i konfiguracją być krzemowych
- [Funkcja czatu](/help/chat-system) - Nauka wiedzy poprzez rozmowę z byciami krzemowymi
- [System zadań](/help/task) - Konfiguracja zadań nauki wiedzy
- [System pamięci](/help/memory) - Zrozumienie, jak wiedza wpływa na pamięć rozmów
";

    public override string WorkNotes => @"
# Notatki Pracy

## Przegląd

Notatki pracy to **osobisty system zapisywania wiedzy** być krzemowych, podobny do cyfrowego dziennika lub dziennika pracy. Każde bycie krzemowe może tworzyć, zarządzać i wyszukiwać własne notatki pracy, zapisując przemyślenia z nauki, doświadczenia zawodowe, refleksje itp.

Notatki pracy obsługują dwa tryby:
- **Osobiste notatki pracy**: Prywatne notatki bycia krzemowego, domyślnie widoczne tylko dla niego
- **Notatki pracy projektu**: Notatki należące do przestrzeni projektu, członkowie projektu mogą wspólnie współpracować

## Główne funkcje

### Kluczowe cechy

- **Stronicowane zapisy**: Każda notatka jest niezależną stroną, zorganizowaną według numeru strony
- **Obsługa Markdown**: Treść obsługuje format Markdown, można pisać tekst sformatowany
- **Oznaczanie słowami kluczowymi**: Można dodawać słowa kluczowe do notatek, ułatwiające wyszukiwanie i kategoryzację
- **Kontrola wersji**: Każda edycja automatycznie aktualizuje numer wersji, rejestrując historię zmian
- **Śledzenie autora**: Rejestruje informacje o twórcy i ostatnim modyfikatorze
- **Przeglądanie osi czasu**: Przeglądanie wszystkich notatek w kolejności chronologicznej
- **Wyszukiwanie pełnotekstowe**: Obsługa wyszukiwania notatek według słów kluczowych, streszczeń, treści

### Dwa typy notatek pracy

| Typ | Własność | Domyślna widoczność | Scenariusze zastosowania |
|------|----------|---------------------|--------------------------|
| Osobiste notatki pracy | Bycie krzemowe | Prywatne | Osobisty dziennik, notatki z nauki, refleksje |
| Notatki pracy projektu | Przestrzeń projektu | Publiczne | Postęp projektu, zapisy współpracy, protokoły ze spotkań |

## Dostęp do notatek pracy

### Lokalizacja wejścia

**Osobiste notatki pracy**:
1. Kliknij ikonę **📝 Notatki pracy** w lewym pasku nawigacji
2. Lub znajdź wejście do notatek pracy na stronie szczegółów bycia krzemowego

**Notatki pracy projektu**:
1. Przejdź do strony szczegółów projektu
2. Kliknij zakładkę **Notatki pracy**

### Opis interfejsu

Strona notatek pracy zawiera dwa główne obszary:

**Obszar siatki notatek**:
- Wyświetla wszystkie notatki w formie kart
- Każda karta pokazuje numer strony, streszczenie, czas aktualizacji
- Najechanie na kartę powoduje efekt uniesienia
- Kliknięcie karty otwiera szczegóły notatki

**Obszar szczegółów notatki**:
- Wyświetla pełną treść notatki na pełnym ekranie
- Pokazuje metainformacje (streszczenie, słowa kluczowe, czas aktualizacji)
- Treść jest renderowana w formacie Markdown
- Kliknięcie przycisku zamykającego powraca do listy

## Tworzenie notatek

### Tworzenie przez narzędzie AI

Bycia krzemowe mogą tworzyć notatki poprzez rozmowę z AI:

**Tworzenie notatki osobistej**:
```
Pomóż mi utworzyć notatkę pracy:
- Streszczenie: Poznałem koncepcję grafu wiedzy
- Treść: Graf wiedzy to sposób reprezentowania wiedzy w strukturze grafowej...
- Słowa kluczowe: graf wiedzy, AI, nauka
```

**Tworzenie notatki projektu**:
```
Utwórz notatkę pracy w projekcie XXX:
- Streszczenie: Aktualizacja postępu projektu - Tydzień 1
- Treść: W tym tygodniu zakończono etap analizy wymagań...
- Słowa kluczowe: postęp, raport tygodniowy
```

### Wymagane pola notatki

- **Streszczenie**: Krótki opis treści notatki (wymagane)
- **Treść**: Szczegółowa treść notatki, obsługa Markdown (wymagane)
- **Słowa kluczowe**: Słowa kluczowe oddzielone przecinkami (opcjonalnie)

### Automatycznie rejestrowane informacje

- **Czas utworzenia**: Automatycznie rejestrowany czas UTC
- **Twórca**: Automatycznie rejestrowany GUID twórcy
- **Numer strony**: System automatycznie przydziela unikalny numer strony
- **Numer wersji**: Początkowa wersja to 1

## Przeglądanie i zarządzanie notatkami

### Przeglądanie listy notatek

1. Otwórz stronę notatek pracy
2. Przeglądaj siatkę kart notatek
3. Góra strony wyświetla statystyki łącznej liczby stron
4. Karty są ułożone w kolejności numerów stron

### Przeglądanie szczegółów notatki

1. Kliknij dowolną kartę notatki
2. Szczegóły notatki rozwijają się na pełnym ekranie
3. Zobacz następujące informacje: tytuł z numerem strony, streszczenie, słowa kluczowe, czas aktualizacji, pełna treść (renderowana w Markdown)
4. Kliknij **×** w prawym górnym rogu, aby zamknąć szczegóły

### Informacje na karcie notatki

Każda karta wyświetla:
- **Strona X**: Identyfikator numeru strony
- **Treść streszczenia**: Krótki opis notatki
- **Czas aktualizacji**: Czas ostatniej modyfikacji

## Wyszukiwanie notatek

### Metody wyszukiwania

**Wyszukiwanie przez narzędzie AI**: Wyszukiwanie notatek pracy zawierających słowa kluczowe

**Zakres wyszukiwania**: Streszczenia notatek, treść notatek, pola słów kluczowych

### Generowanie spisu notatek

Możesz poprosić AI o wygenerowanie przeglądu spisu wszystkich notatek:
```
Wygeneruj spis moich notatek pracy
```

## Aktualizacja notatek

### Edycja treści notatek

Aktualizacja notatek przez narzędzie AI, można zaktualizować tylko streszczenie, treść lub słowa kluczowe

### Cechy aktualizacji

- **Aktualizacja selektywna**: Można zaktualizować tylko wybrane pola
- **Inkrementacja wersji**: Numer wersji automatycznie zwiększa się o 1 po każdej edycji
- **Śledzenie modyfikacji**: Rejestruje GUID ostatniego modyfikatora
- **Aktualizacja czasu**: Czas aktualizacji odświeża się automatycznie

## Usuwanie notatek

### Operacja usuwania

Usuwanie notatek przez narzędzie AI, wymaga podania numeru strony lub ID notatki

### Potwierdzenie usunięcia

- Po usunięciu nie można przywrócić, należy zachować ostrożność
- Operacja usuwania jest rejestrowana w dzienniku audytów

## Kontrola uprawnień

### Uprawnienia do osobistych notatek pracy

- **Twórca**: Pełna kontrola (tworzenie, przeglądanie, edycja, usuwanie)
- **Kurator krzemowy**: Może zarządzać notatkami wszystkich być krzemowych
- **Inni użytkownicy**: Brak dostępu

### Uprawnienia do notatek pracy projektu

- **Członkowie projektu**: Mogą tworzyć, przeglądać, edytować notatki projektu
- **Niebędący członkami projektu**: Brak dostępu
- **Kurator krzemowy**: Może zarządzać wszystkimi notatkami projektu

## Scenariusze użycia

### Osobiste zapisy z nauki

Zapisywanie procesu nauki nowej wiedzy, dodawanie streszczeń, treści i słów kluczowych

### Śledzenie postępu projektu

Zapisywanie tygodniowego postępu projektu, śledzenie realizacji zadań

### Podsumowanie doświadczeń zawodowych

Podsumowanie lekcji wyniesionych z pracy, ułatwiające późniejsze przeglądanie

### Protokoły ze spotkań

Zapisywanie treści spotkań projektowych, zachowywanie decyzji i wyników dyskusji

## Najlepsze praktyki

### 1. Standaryzuj pisanie streszczeń

- Streszczenie ogranicz do 20 znaków, jasno wyrażaj rdzenną treść notatki

### 2. Rozsądnie używaj słów kluczowych

- Używaj 3-5 słów kluczowych, oddzielonych przecinkami

### 3. Strukturyzuj treść

- Używaj nagłówków Markdown do hierarchii, ważne informacje w listach

### 4. Regularnie porządkuj notatki

- Regularnie generuj przegląd spisu, usuwaj nieaktualne lub zbędne notatki

### 5. Współpraca w notatkach projektu

- Jasno zapisuj informacje o autorze, wyjaśniaj powody modyfikacji podczas edycji

## Często zadawane pytania

### P1: Jaka jest różnica między notatkami pracy a systemem pamięci?

**O**: Notatki pracy to aktywnie zapisywane, ustrukturyzowane treści, podobne do dziennika; system pamięci to automatycznie lub ręcznie zapisywane fragmenty rozmów i fakty.

### P2: Czy notatki mogą być chronione hasłem?

**O**: Bieżąca wersja nie obsługuje ochrony hasłem, ale dostęp jest kontrolowany przez system uprawnień.

### P3: Czy istnieje limit liczby notatek?

**O**: Nie ma sztywnego limitu, zaleca się regularne porządkowanie w celu utrzymania jakości notatek.

### P4: Czy można wyeksportować notatki?

**O**: Bieżąca wersja nie obsługuje bezpośredniego eksportu, można uzyskać dane notatek przez API.

### P5: Czy można przeglądać historię wersji notatek?

**O**: Bieżąca wersja zachowuje tylko najnowszą wersję, nie zapisuje historii wersji.

### P6: Czy notatki projektu i osobiste mogą być konwertowane między sobą?

**O**: Bezpośrednia konwersja nie jest obsługiwana, można utworzyć nową notatkę i skopiować treść.

## Powiązana dokumentacja

- [Zarządzanie byciami krzemowymi](/help/being-management) - Dowiedz się, jak zarządzać byciami krzemowymi
- [Zarządzanie projektami](/help/projects) - Dowiedz się o przestrzeniach projektowych i współpracy
- [Zarządzanie zadaniami](/help/task) - Dowiedz się o przydzielaniu i śledzeniu zadań
- [Graf Wiedzy](/help/knowledge-graph) - Dowiedz się o wizualizacji wiedzy
- [Zarządzanie uprawnieniami](/help/permission) - Dowiedz się o mechanizmach kontroli uprawnień
";

    public override string Projects => @"
# Zarządzanie Projektami

## Przegląd

Zarządzanie projektami to **przestrzeń współpracy** systemu być krzemowych, zapewniająca środowisko do wspólnej pracy wielu być krzemowych. Przez przestrzenie projektowe Kurator krzemowy może organizować i zarządzać zespołem być krzemowych, przydzielać zadania, śledzić postępy i realizować złożone cele współpracy.

Projekt to główna jednostka organizacyjna systemu być krzemowych, każdy projekt posiada niezależny system zadań, notatki pracy i listę członków.

## Główne funkcje

### Kluczowe cechy

- **Zarządzanie cyklem życia projektu**: Tworzenie, archiwizacja, przywracanie, niszczenie projektów
- **Zarządzanie członkami**: Przydzielanie i usuwanie członków projektu
- **Współpraca w zadaniach**: Dedykowany system zadań dla projektu
- **Notatki pracy**: Współdzielone notatki pracy na poziomie projektu
- **Informacje o projekcie**: Zarządzanie metainformacjami takimi jak nazwa, opis, status
- **Mechanizm archiwizacji**: Obsługa archiwizacji i przywracania projektów, ochrona danych historycznych

### Statusy projektu

| Status | Opis | Dostępne operacje |
|--------|------|-------------------|
| Aktywny | Działający projekt | Wszystkie operacje |
| Zarchiwizowany | Wstrzymany projekt, dane zachowane | Przywrócenie, przeglądanie |
| Zniszczony | Trwale usunięty projekt | Brak |

## Dostęp do zarządzania projektami

### Lokalizacja wejścia

**Funkcja wyłącznie dla Kuratora krzemowego**:
- Narzędzia zarządzania projektami są dostępne tylko dla Kuratora krzemowego
- Wykonywanie operacji zarządzania projektami poprzez rozmowę z AI
- Przeglądanie powiązań projektów na stronie zarządzania byciami krzemowymi

### Strona projektu

Każdy projekt ma dedykowaną stronę: Odwiedź `/project/{ID_projektu}`, aby zobaczyć szczegóły projektu, notatki pracy, listę zadań i listę członków

## Tworzenie projektu

### Operacja tworzenia

Tworzenie projektu przez narzędzie AI: Podaj jasną nazwę projektu i szczegółowy opis (zalecane wypełnienie)

### Automatyczne ustawienia po utworzeniu

- **ID projektu**: System automatycznie generuje unikalny GUID
- **Czas utworzenia**: Rejestruje czas utworzenia UTC
- **Status początkowy**: Aktywny
- **Lista członków**: Początkowo pusta, wymaga późniejszego przypisania

## Zarządzanie członkami projektu

### Przydzielanie członków do projektu

Dodanie bycia krzemowego do projektu: Wymaga podania ID projektu i ID bycia krzemowego (GUID)

### Usuwanie członków z projektu

Usunięcie bycia krzemowego z projektu: Wymaga podania ID projektu i ID bycia krzemowego

### Uprawnienia członków

- **Członkowie projektu**: Mogą tworzyć zadania projektowe, pisać notatki projektu
- **Niebędący członkami projektu**: Nie mają dostępu do zasobów projektu
- **Kurator krzemowy**: Może zarządzać członkami wszystkich projektów

## Aktualizacja informacji o projekcie

Można aktualizować nazwę i opis projektu przez narzędzie AI, obsługuje aktualizację pojedynczą lub jednoczesną.

## Zarządzanie cyklem życia projektu

### Archiwizacja projektu

Archiwizacja nieaktywnego projektu. Efekty archiwizacji: Dane projektu są w pełni zachowane, członkowie nadal są powiązani, można przywrócić z listy zarchiwizowanych.

**Scenariusze archiwizacji**: Projekt tymczasowo wstrzymany, projekt etapowy zakończony, potrzeba uporządkowania listy projektów.

### Przywracanie projektu

Przywracanie projektu ze stanu zarchiwizowanego. Efekty przywracania: Status projektu zmienia się z powrotem na aktywny, wszystkie dane są w pełni odzyskane, relacje członków pozostają niezmienione.

### Niszczenie projektu

Trwałe usunięcie projektu (nieodwracalne). Efekty niszczenia: Dane projektu są trwale usunięte, powiązane zadania i notatki pracy są czyszczone, relacje członków są rozwiązane.

**Przed niszczeniem potwierdź**: Czy projekt rzeczywiście nie jest już potrzebny, zrób kopię zapasową ważnych danych (jeśli to konieczne), powiadom członków projektu.

### Przeglądanie listy i szczegółów projektów

- **Wyświetlanie wszystkich aktywnych projektów**: Wyświetlanie wszystkich projektów (w tym zarchiwizowanych: wyświetlanie wszystkich projektów, w tym zarchiwizowanych)
- **Szczegóły projektu**: Uzyskaj pełne informacje takie jak ID projektu, nazwa, opis, status, czas utworzenia, lista członków, statystyki zadań.

## Funkcje współpracy w projekcie

### Zadania projektu

Każdy projekt posiada niezależny system zadań:

**Tworzenie zadań projektu**: Utwórz zadanie w projekcie XXX, podaj tytuł, opis, priorytet (im mniejsza liczba, tym wyższy priorytet)

**Przepływ statusów zadań**: Oczekujące (Pending) → W realizacji (Running) → Ukończone (Completed) / Nieudane (Failed) / Anulowane (Cancelled)

### Notatki pracy projektu

Każdy projekt posiada współdzielony system notatek pracy: Wszyscy członkowie projektu mogą tworzyć i edytować notatki, zorganizowane według numeru strony ułatwiające śledzenie, rejestrują informacje o autorze w celu śledzenia wkładu, obsługują format Markdown.

### Porównanie zasobów osobistych i projektowych

| Typ zasobu | Osobiste | Projektowe |
|-----------|----------|------------|
| Notatki pracy | Prywatny dziennik | Współdzielone notatki współpracy |
| Zadania | Zadania osobiste | Zadania współpracy projektu |
| Graf wiedzy | Wiedza osobista | Wiedza projektu (przyszłość) |
| Pamięć | Pamięć osobista | Pamięć projektu (przyszłość) |

## Scenariusze użycia

Projekty programistyczne, projekty badawcze, projekty edukacyjne i inne scenariusze współpracy - wszystkie obsługują tworzenie projektów, przydzielanie członków, tworzenie zadań, zapisywanie notatek, śledzenie postępów, archiwizację ukończonych projektów.

## Najlepsze praktyki

### 1. Konwencja nazewnictwa projektów: Używaj jasnych nazw projektów, zawierających typ lub cel projektu, unikaj niejasnych skrótów.

### 2. Szczegółowy opis projektu: Dokładnie opisz cel i zakres projektu, zapisz kluczowe kamienie milowe, określ oczekiwane rezultaty.

### 3. Rozsądne przydzielanie członków: Przydzielaj członków na podstawie umiejętności i ról, unikaj przypisywania jednego członka do zbyt wielu projektów.

### 4. Zarządzanie priorytetami zadań: Krytyczne zadania ustawiaj z wysokim priorytetem (1-2), zwykłe zadania ze średnim priorytetem (3-4).

### 5. Regularnie aktualizuj notatki projektu: Aktualizuj postęp co najmniej raz w tygodniu, zapisuj ważne decyzje i zmiany.

### 6. Odpowiednio archiwizuj projekty: Ukończone projekty archiwizuj w odpowiednim czasie, długo nieaktywne projekty rozważ do archiwizacji.

### 7. Ostrożnie używaj funkcji niszczenia: Przed zniszczeniem podwójnie potwierdź, rozważ najpierw archiwizację i obserwację przez pewien czas.

## Kontrola uprawnień

### Uprawnienia zarządzania projektami: Kurator krzemowy może zarządzać całym cyklem życia projektu; członkowie projektu mogą uczestniczyć we współpracy; niebędący członkami projektu nie mają dostępu.

### Uprawnienia narzędzi: Narzędzie `project` jest dostępne tylko dla Kuratora krzemowego; narzędzia `project_task` i `project_work_note` są dostępne dla członków projektu i Kuratora krzemowego.

## Często zadawane pytania

### P1: Czy istnieje górny limit liczby projektów? **O**: Bieżąca wersja nie ma sztywnego limitu, zaleca się utrzymywanie rozsądnej liczby projektów.

### P2: Czy zarchiwizowany projekt zajmuje miejsce? **O**: Tak, wszystkie dane zarchiwizowanego projektu są zachowane.

### P3: Czy można należeć do wielu projektów jednocześnie? **O**: Tak, jedno bycie krzemowe może być jednocześnie przypisane do wielu projektów.

### P4: Czy członek projektu może sam opuścić projekt? **O**: Bieżąca wersja nie obsługuje samodzielnego wyjścia członka, wymagana jest operacja usunięcia przez Kuratora krzemowego.

### P5: Czy do zarchiwizowanego projektu można dodawać zadania? **O**: Nie, należy najpierw przywrócić projekt do statusu aktywnego.

### P6: Czy podczas usuwania projektu zadania i notatki są usuwane razem? **O**: Tak, ta operacja jest nieodwracalna.

### P7: Czy można skopiować projekt? **O**: Bieżąca wersja nie obsługuje bezpośredniego kopiowania projektów.

### P8: Czy projekt ma termin końcowy? **O**: Bieżąca wersja nie wymusza ustawiania terminu końcowego, można zarządzać czasem przez zadania.

## Powiązana dokumentacja

- [Zarządzanie zadaniami](/help/task) - Dowiedz się o zarządzaniu zadaniami osobistymi
- [Notatki pracy](/help/work-notes) - Dowiedz się o systemie notatek pracy
- [Zarządzanie byciami krzemowymi](/help/being-management) - Dowiedz się, jak zarządzać byciami krzemowymi
- [Zarządzanie uprawnieniami](/help/permission) - Dowiedz się o mechanizmach kontroli uprawnień
- [Dziennik audytów](/help/audit-log) - Zobacz rekordy operacji projektowych
";

    public override string Logging => @"
# System logowania

## Przegląd

System logowania to główna infrastruktura platformy Silicon Life Collective, służąca do rejestrowania stanu działania systemu, zachowań być krzemowych, informacji o błędach i danych debugowania. System jednocześnie wysyła dane do konsoli i systemu plików, zapewniając elastyczną funkcję filtrowania poziomów logów.

## Główne funkcje

- **Rejestrowanie na wielu poziomach**: Obsługa 6 poziomów logów (Trace, Debug, Information, Warning, Error, Critical)
- **Wiele celów wyjściowych**: Jednoczesna obsługa kolorowego wyjścia konsoli i trwałego przechowywania w systemie plików
- **Inteligentne filtrowanie**: Obsługa filtrowania według poziomu logów, zakresu czasu, bycia krzemowego
- **Zarządzanie kategoriami**: Organizacja logów według kategorii, ułatwiająca lokalizowanie problemów
- **Rejestrowanie wyjątków**: Automatyczne rejestrowanie informacji o stosie wyjątków, wspomagające rozwiązywanie problemów

## Opis poziomów logów

System definiuje następujące poziomy logów (w kolejności rosnącej powagi):

| Poziom | Opis | Scenariusze użycia |
|--------|------|---------------------|
| **Trace** | Najbardziej szczegółowe logi | Debugowanie rozwojowe, zawiera dane wrażliwe |
| **Debug** | Informacje debugowania | Interaktywne dochodzenie, cel debugowania |
| **Information** | Standardowe informacje | Śledzenie zwykłego przepływu aplikacji, o wartości długoterminowej |
| **Warning** | Ostrzeżenia | Nietypowe lub nieoczekiwane zdarzenia, ale nie powodujące zatrzymania wykonania |
| **Error** | Błędy | Bieżący przepływ wykonania zatrzymany z powodu porażki |
| **Critical** | Błędy krytyczne | Awaria systemu lub katastrofalna usterka, wymaga natychmiastowej interwencji |
| **None** | Brak rejestrowania | Używane do wyłączenia logowania dla określonej kategorii |

## Przewodnik użytkowania

### Gdzie przeglądać logi?

System zapewnia dwa sposoby przeglądania logów:

1. **Logi konsoli**: Podczas uruchamiania programu, okno terminala wyświetla kolorowe logi w czasie rzeczywistym
   - Różne kolory oznaczają różny stopień powagi (czerwony=błąd, żółty=ostrzeżenie itp.)
   - Odpowiednie do debugowania rozwojowego i monitorowania w czasie rzeczywistym

2. **Logi plikowe**: Wszystkie logi są automatycznie zapisywane w katalogu `data/Log/`
   - Trwałe przechowywanie, można w dowolnym momencie przeglądać historię
   - Odpowiednie do śledzenia problemów i analizy po fakcie

## Opcje konfiguracji

### Dostosowanie szczegółowości logów

Możesz ustawić szczegółowość logów w pliku konfiguracyjnym:

- **Podczas debugowania rozwojowego**: Ustaw na ""Debug"" lub ""Trace"", aby zobaczyć bardziej szczegółowe informacje
- **Podczas codziennego użytkowania**: Ustaw na ""Information"", aby rejestrować standardowe operacje
- **W środowisku produkcyjnym**: Ustaw na ""Warning"", aby rejestrować tylko problemy i błędy

## Przeglądanie i filtrowanie logów

Pliki logów są zapisywane w katalogu `data/Log/`, automatycznie porządkowane według czasu, ułatwiając wyszukiwanie rekordów historycznych.

## Format wyświetlania logów

### Styl wyświetlania konsoli

```
[2026-04-27 10:30:00.123] [INFO] [Kategoria] [Being:guid] Wiadomość logu
```

- Znacznik czasu: Dokładność do milisekund
- Poziom: Wyrównany do prawej, szerokość 4 znaków
- Kategoria: Identyfikator źródła logu
- ID bycia krzemowego: Wyświetlane tylko, gdy log dotyczy konkretnego bycia

### Atrybuty wpisu logu

Każdy log zawiera następujące informacje:

- **BeingId**: ID powiązanego bycia krzemowego (puste dla logów systemowych)
- **Timestamp**: Czas utworzenia logu (UTC)
- **Level**: Poziom logu
- **Category**: Kategoria logu
- **Message**: Wiadomość logu
- **Exception**: Informacje o wyjątku (jeśli wystąpił)

## Najlepsze praktyki

### 1. Rozsądnie wybieraj poziom logów

- **Środowisko rozwojowe**: Używaj poziomu Debug lub Trace
- **Środowisko produkcyjne**: Używaj poziomu Information lub Warning
- **Rozwiązywanie problemów**: Tymczasowo obniż do poziomu Debug

### 2. Na które logi zwracać uwagę?

Zaleca się zwracanie uwagi na następujące sytuacje:
- Informacje o uruchomieniu i zamykaniu systemu
- Tworzenie, nietypowe działanie i zatrzymywanie być krzemowych
- Nieudane weryfikacje uprawnień i problemy bezpieczeństwa
- Nieudane wywołania AI i nietypowe odpowiedzi
- Błędy wykonania narzędzi i przekroczenia czasu

### 3. Unikaj nadmiernych logów wpływających na wydajność

Jeśli pliki logów są zbyt duże, można podnieść ustawienie szczegółowości logów (np. zmienić na ""Warning"" lub ""Error""), aby zmniejszyć ilość rejestrowanych danych.

### 4. Ochrona informacji wrażliwych

System nie rejestruje w logach następujących informacji wrażliwych:
- Hasła i klucze
- Dane osobowe
- Klucze API i tokeny
- Parametry połączenia z bazą danych

## Często zadawane pytania

### P: Dlaczego nie widzę niektórych informacji w logach?

**O**: Możliwe przyczyny:
1. Ustawienie szczegółowości logów jest zbyt wysokie, filtrowane są pewne informacje (można zmienić na ""Debug"" lub ""Trace"" w konfiguracji)
2. System nie został poprawnie uruchomiony, funkcja logowania nie została zainicjowana

### P: Jak przeglądać bardziej szczegółowe logi?

**O**: Zmień ustawienie szczegółowości logów w pliku konfiguracyjnym na ""Debug"" lub ""Trace"", a następnie uruchom ponownie system.

### P: Gdzie są pliki logów?

**O**: Pliki logów są przechowywane w katalogu `data/Log/`, zindeksowane według czasu.

## Rozwiązywanie problemów

### Problem: Nie można znaleźć historycznych rekordów logów

**Objawy**: Nie można przeglądać logów z przeszłości

**Rozwiązanie**:
1. Sprawdź, czy katalog `data/Log/` istnieje i ma uprawnienia do odczytu/zapisu
2. Potwierdź, czy jest wystarczająco dużo miejsca na dysku

## Uwagi

- System logowania gwarantuje stabilne działanie, awaria pojedynczego celu wyjściowego nie wpływa na inne cele
- Jeśli pliki logów są zbyt duże, można podnieść ustawienie szczegółowości logów (np. zmienić na ""Warning"" lub ""Error""), aby zmniejszyć ilość rejestrowanych danych
- Przy dużej ilości zapisywanych logów zaleca się odpowiednie podniesienie poziomu logowania
";

    #endregion
}