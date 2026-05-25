# Przewodnik współtworzenia

> **Wersja: v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md) | [Polski](../pl-PL/contributing.md)

Dziękujemy za zainteresowanie współtworzeniem SiliconLifeCollective!

## Współtworzenie dwóch wersji

Ten projekt ma dwie wersje implementacji, możesz wybrać kierunek współtworzenia w zależności od zainteresowań:

### SiliconLife.Default (wersja domyślna)
- **Stos technologiczny**: aplikacja konsolowa .NET 9
- **Kierunek współtworzenia**: rozwój rdzennych funkcji, implementacja narzędzi, lokalizacja, dokumentacja
- **Odpowiednie dla**: wszystkich programistów

### SiliconLife.Fast (wersja wysokowydajna)
- **Stos technologiczny**: aplikacja okienkowa Windows .NET 9
- **Kierunek współtworzenia**: optymalizacja wydajności, przechowywanie SpeedyPack, zasobnik systemowy, współbieżność bez blokad
- **Odpowiednie dla**: programistów z doświadczeniem w rozwoju Windows, zainteresowanych optymalizacją wydajności

> **Ważna uwaga**: Obie wersje współdzielą projekty SiliconLife.Core i SiliconLife.Common, ulepszenia rdzennych interfejsów wpłyną na obie wersje jednocześnie.

## Kodeks postępowania

Ten projekt jest zgodny z licencją Apache 2.0. Bądźcie pełni szacunku i profesjonalni we wszystkich interakcjach.

---

## Szybki start

### 1. Fork repozytorium

Kliknij przycisk "Fork" na GitHub, aby utworzyć własną kopię.

### 2. Klonowanie Twojego Forka

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Konfiguracja środowiska programistycznego

```bash
# Zainstaluj .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# Przywróć zależności
dotnet restore

# Zbuduj projekt
dotnet build

# Uruchom testy
dotnet test
```

### 4. Utworzenie gałęzi funkcjonalnej

```bash
git checkout -b feature/nazwa-twojej-funkcji
```

### 5. Wybór projektu programistycznego

W zależności od typu współtworzenia wybierz odpowiedni projekt:

- **Interfejsy rdzenne/klasy abstrakcyjne** → modyfikuj `SiliconLife.Core`
- **Współdzielona implementacja** → modyfikuj `SiliconLife.Common`
- **Specyficzne dla wersji Default** → modyfikuj `SiliconLife.Default`
- **Specyficzne dla wersji Fast** → modyfikuj `SiliconLife.Fast`
- **Silnik przechowywania** → modyfikuj `SiliconLife.Speedy`
- **Narzędzie zarządzania przechowywaniem** → modyfikuj `SiliconLife.Speedy.Manager`
- **Rozwój wtyczek** → modyfikuj `SiliconLife.Core/Plugins`
- **Dokumentacja wielojęzyczna** → modyfikuj katalog `docs/`

---

## Przepływ pracy programistycznej

### Styl kodu

- Przestrzegaj konwencji kodowania C#
- Nazwy klas używają PascalCase
- Parametry metod używają camelCase
- Prywatne pola używają `_camelCase`
- Wszystkie publiczne API muszą mieć dokumentację XML

### Komunikaty zatwierdzeń

Przestrzegaj formatu **Konwencjonalnych Zatwierdzeń**:

```
<typ>(<zakres>): <opis>
```

**Typy**:
- `feat`: nowa funkcja
- `fix`: poprawka błędu
- `docs`: zmiany dokumentacji
- `style`: formatowanie kodu
- `refactor`: refaktoring kodu
- `test`: zmiany testów
- `chore`: zmiany budowania/narzędzi

**Przykłady**:
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### Wprowadzanie zmian

1. **Pisanie kodu**
   - Przestrzegaj istniejących wzorców
   - Dodaj testy dla nowych funkcji
   - Aktualizuj dokumentację

2. **Testowanie zmian**
   ```bash
   # Uruchom wszystkie testy
   dotnet test

   # Buduj w trybie wydania
   dotnet build --configuration Release
   ```

3. **Formatowanie kodu**
   ```bash
   dotnet format
   ```

4. **Zatwierdzanie zmian**
   ```bash
   git add .
   git commit -m "feat(zakres): opis"
   ```

5. **Wypychanie do Forka**
   ```bash
   git push origin feature/nazwa-twojej-funkcji
   ```

6. **Utworzenie żądania pobrania**
   - Przejdź do oryginalnego repozytorium
   - Kliknij "Compare & pull request"
   - Wypełnij szablon PR
   - Prześlij

---

## Przewodnik żądań pobrania

### Tytuł PR

Użyj tego samego formatu co komunikaty zatwierdzeń:
```
feat(localization): add Korean language support
```

### Opis PR

Uwzględnij:

1. **Co** - co robi ten PR?
2. **Dlaczego** - dlaczego ta zmiana jest potrzebna?
3. **Jak** - jak to zaimplementowałeś?
4. **Testy** - jak to przetestowałeś?

### Przykład opisu PR

```markdown
## Co
Dodanie koreańskiej lokalizacji dla wszystkich komponentów UI i dokumentacji.

## Dlaczego
Rozszerzenie dostępności projektu dla użytkowników koreańskich.

## Jak
- Utworzenie pliku lokalizacji KoKR.cs
- Dodanie 500+ kluczy tłumaczeń
- Aktualizacja wszystkich widoków do użycia lokalizacji
- Utworzenie koreańskiej dokumentacji w docs/ko-KR/

## Testy
- Weryfikacja poprawnego wyświetlania koreańskiego tekstu we wszystkich elementach UI
- Testowanie funkcji przełączania języka
- Przegląd tłumaczeń z rodzimym użytkownikiem
```

---

## Typy współtworzenia

### 1. Poprawki błędów

**Proces**:
1. Sprawdź istniejące zgłoszenia
2. Jeśli nie istnieje, utwórz zgłoszenie
3. Napraw błąd
4. Dodaj przypadki testowe
5. Prześlij PR

**Wymagania**:
- Jasno opisz błąd
- Kroki reprodukcji
- Testy zapobiegające regresji

### 2. Nowe funkcje

**Proces**:
1. Omów funkcję w Issues/Discussions
2. Uzyskaj aprobatę opiekuna
3. Zaimplementuj funkcję
4. Dodaj kompleksowe testy
5. Zaktualizuj dokumentację
6. Prześlij PR

**Wymagania**:
- Propozycja funkcji została zatwierdzona
- Pełne pokrycie testami
- Dokumentacja zaktualizowana
- Zgodność wsteczna

### 3. Dokumentacja

**Proces**:
1. Zidentyfikuj luki w dokumentacji
2. Napisz/zaktualizuj dokumentację
3. Prześlij PR

**Wymagania**:
- Jasna i zwięzła
- Uwzględnij przykłady
- Obsługa wielu języków, jeśli ma zastosowanie

### 4. Refaktoring kodu

**Proces**:
1. Zaproponuj refaktoring w zgłoszeniu
2. Uzyskaj aprobatę
3. Refaktoryzuj kod
4. Upewnij się, że wszystkie testy przechodzą
5. Prześlij PR

**Wymagania**:
- Brak zmian funkcjonalnych
- Wszystkie testy przechodzą
- Poprawa jakości kodu
- Jasne wyjaśnienie

---

## Przewodnik testowania

### Testy jednostkowe

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // Arrange
    var service = new MyService();

    // Act
    var result = service.DoSomething();

    // Assert
    Assert.IsTrue(result.Success);
}
```

### Testy integracyjne

Testowanie pełnych przepływów pracy:
- Interakcja AI
- Wykonywanie narzędzi
- Weryfikacja uprawnień
- Operacje przechowywania

### Testy manualne

Dla zmian UI:
- Testuj w wielu przeglądarkach
- Weryfikuj projekt responsywny
- Sprawdzaj dostępność

---

## Przewodnik dokumentacji

### Komentarze w kodzie

- Wszystkie publiczne API używają komentarzy XML
- Złożona logika używa komentarzy inline
- Komentarze w kodzie w języku angielskim

### Pliki dokumentacji

- Umieszczaj w `docs/{language}/`
- Aktualizuj wszystkie wersje językowe
- Przestrzegaj istniejącej struktury

### Dokumentacja wielojęzyczna

Podczas dodawania dokumentacji:
1. Najpierw utwórz wersję angielską
2. Przetłumacz na inne języki
3. Utrzymuj synchronizację treści

---

## Proces przeglądu

### Czego szukają opiekunowie

1. **Jakość kodu**
   - Przestrzeganie konwencji
   - Jasny i czytelny
   - Dobrze udokumentowany

2. **Testy**
   - Odpowiednie pokrycie
   - Wszystkie testy przechodzą
   - Pokrycie przypadków brzegowych

3. **Dokumentacja**
   - Zaktualizowana
   - Jasne wyjaśnienia
   - Wielojęzyczna

4. **Zgodność**
   - Zgodność wsteczna
   - Brak zmian łamiących (chyba że zgłoszonych)
   - Przestrzeganie wersjonowania semantycznego

### Oś czasu przeglądu

- Wstępny przegląd: 1-3 dni
- Integracja opinii: w razie potrzeby
- Scalanie: po zatwierdzeniu

---

## Często zadawane pytania

### PR zostało odrzucone

**Przyczyny**:
- Nieprzestrzeganie wytycznych
- Niewystarczające testy
- Niezgłoszone zmiany łamiące
- Słaba jakość kodu

**Rozwiązanie**:
- Rozwiąż opinie
- Zaktualizuj PR
- Prześlij ponownie

### Konflikty scalania

**Rozwiązanie**:
```bash
# Zaktualizuj swoją gałąź
git fetch origin
git rebase origin/master

# Rozwiąż konflikty
# Edytuj pliki z konfliktami
git add .
git rebase --continue

# Wymuś wypchnięcie
git push --force-with-lease
```

---

## Uzyskiwanie pomocy

### Zasoby

- **Dokumentacja**: [docs/](../)
- **Zgłoszenia**: GitHub Issues
- **Dyskusje**: GitHub Discussions
- **Kodeks postępowania**: CODE_OF_CONDUCT.md

### Kontakt

- Utwórz zgłoszenie dla błędu
- Rozpocznij dyskusję dla pytań
- Oznacz opiekuna dla pilnych spraw

---

## Podziękowania

Współtwórcy otrzymają uznanie w:
- Sekcji współtwórców README.md
- Notkach wydania
- Dokumentacji projektu

---

## Licencja

Wnosząc swój wkład, zgadzasz się, że będzie on licencjonowany na licencji Apache 2.0.

---

## Następne kroki

- 📚 Przeczytaj [dokumentację](../)
- 🐛 Zobacz [otwarte zgłoszenia](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Rozpocznij [dyskusję](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Forkuj i zacznij współtworzyć!

Dziękujemy za współtworzenie SiliconLifeCollective! 🎉
