# Przewodnik współtworzenia

> **Wersja: v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md)

Dziękujemy za zainteresowanie współtworzeniem projektu SiliconLifeCollective!

## Współtworzenie dwóch wersji

Projekt posiada dwie wersje implementacji — możesz wybrać kierunek współtworzenia zgodnie ze swoimi zainteresowaniami:

### SiliconLife.Default (wersja domyślna)
- **Stos technologiczny**: aplikacja konsolowa .NET 9
- **Kierunek współtworzenia**: rozwój funkcji podstawowych, implementacja narzędzi, lokalizacja, dokumentacja
- **Dla kogo**: wszyscy programiści

### SiliconLife.Fast (wersja wysokowydajna)
- **Stos technologiczny**: wieloplatformowa aplikacja desktopowa .NET 9 (Avalonia UI)
- **Kierunek współtworzenia**: optymalizacja wydajności, pamięć masowa SpeedyPack, zasobnik systemowy, współbieżność bez blokad
- **Dla kogo**: programiści z doświadczeniem w tworzeniu aplikacji desktopowych, zainteresowani optymalizacją wydajności

> **Ważna uwaga**: obie wersje współdzielą projekty SiliconLife.Core i SiliconLife.Common, więc ulepszenia interfejsów podstawowych wpływają jednocześnie na obie wersje.

## Kodeks postępowania

Projekt jest objęty licencją Apache 2.0. Wszystkie interakcje powinny być pełne szacunku i profesjonalne.

---

## Szybki start

### 1. Fork repozytorium

Kliknij przycisk „Fork" na GitHub, aby utworzyć własną kopię.

### 2. Sklonuj swój Fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Skonfiguruj środowisko programistyczne

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

### 4. Utwórz gałąź funkcji

```bash
git checkout -b feature/your-feature-name
```

### 5. Wybierz projekt do rozwoju

W zależności od typu współtworzenia wybierz odpowiedni projekt:

- **Interfejsy/klasy abstrakcyjne podstawowe** → modyfikuj `SiliconLife.Core`
- **Współdzielone implementacje** → modyfikuj `SiliconLife.Common`
- **Specyficzne dla wersji Default** → modyfikuj `SiliconLife.Default`
- **Specyficzne dla wersji Fast** → modyfikuj `SiliconLife.Fast`
- **Silnik przechowywania** → modyfikuj `SiliconLife.Speedy`
- **Narzędzie zarządzania przechowywaniem** → modyfikuj `SiliconLife.Speedy.Manager`
- **Tworzenie wtyczek** → modyfikuj `SiliconLife.Core/Plugins`
- **Dokumentacja wielojęzyczna** → modyfikuj katalog `docs/`

---

## Przepływ pracy programistycznej

### Styl kodu

- Przestrzegaj konwencji kodowania C#
- Nazwy klas używają PascalCase
- Parametry metod używają camelCase
- Pola prywatne używają `_camelCase`
- Wszystkie publiczne API muszą posiadać dokumentację XML

### Komunikaty zatwierdzeń

Przestrzegaj formatu **Conventional Commits**:

```
<type>(<scope>): <description>
```

**Typy**:
- `feat`: nowa funkcja
- `fix`: poprawka błędu
- `docs`: zmiany dokumentacji
- `style`: formatowanie kodu
- `refactor`: refaktoryzacja kodu
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

1. **Pisz kod**
   - Przestrzegaj istniejących wzorców
   - Dodaj testy dla nowych funkcji
   - Zaktualizuj dokumentację

2. **Testuj swoje zmiany**
   ```bash
   # Uruchom wszystkie testy
   dotnet test

   # Zbuduj w trybie wydania
   dotnet build --configuration Release
   ```

3. **Formatuj kod**
   ```bash
   dotnet format
   ```

4. **Zatwierdź zmiany**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

5. **Wypchnij do swojego Forka**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Utwórz żądanie wciągnięcia**
   - Przejdź do oryginalnego repozytorium
   - Kliknij „Compare & pull request"
   - Wypełnij szablon PR
   - Prześlij

---

## Przewodnik żądań wciągnięcia

### Tytuł PR

Użyj tego samego formatu co komunikaty zatwierdzeń:
```
feat(localization): add Korean language support
```

### Opis PR

Uwzględnij:

1. **Co** — co robi ten PR?
2. **Dlaczego** — dlaczego ta zmiana jest potrzebna?
3. **Jak** — jak zostało to zaimplementowane?
4. **Testy** — jak zostało przetestowane?

### Przykład opisu PR

```markdown
## Co
Dodanie lokalizacji koreańskiej dla wszystkich komponentów UI i dokumentacji.

## Dlaczego
Rozszerzenie dostępności projektu dla użytkowników koreańskich.

## Jak
- Utworzono plik lokalizacji KoKR.cs
- Dodano ponad 500 kluczy tłumaczeń
- Zaktualizowano wszystkie widoki do użycia lokalizacji
- Utworzono dokumentację koreańską w docs/ko-KR/

## Testy
- Zweryfikowano, że wszystkie elementy UI poprawnie wyświetlają język koreański
- Przetestowano funkcję przełączania języka
- Przegląd tłumaczeń przez native speakera
```

---

## Typy współtworzenia

### 1. Poprawka błędu

**Proces**:
1. Sprawdź istniejące zgłoszenia
2. Jeśli nie istnieje, utwórz zgłoszenie
3. Napraw błąd
4. Dodaj przypadki testowe
5. Prześlij PR

**Wymagania**:
- Jasny opis błędu
- Kroki reprodukcji
- Test zapobiegający regresji

### 2. Nowa funkcja

**Proces**:
1. Omów funkcję w Issues/Discussions
2. Uzyskaj aprobatę opiekuna
3. Zaimplementuj funkcję
4. Dodaj kompleksowe testy
5. Zaktualizuj dokumentację
6. Prześlij PR

**Wymagania**:
- Propozycja funkcji zatwierdzona
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
- Zawiera przykłady
- Obsługuje wiele języków, jeśli dotyczy

### 4. Refaktoryzacja kodu

**Proces**:
1. Zaproponuj refaktoryzację w Issue
2. Uzyskaj aprobatę
3. Przeprowadź refaktoryzację
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

Testuj pełne przepływy pracy:
- Interakcja z AI
- Wykonywanie narzędzi
- Weryfikacja uprawnień
- Operacje przechowywania

### Testy ręczne

Dla zmian UI:
- Testuj w wielu przeglądarkach
- Weryfikuj responsywny design
- Sprawdzaj dostępność

---

## Przewodnik dokumentacji

### Komentarze w kodzie

- Używaj komentarzy XML dla wszystkich publicznych API
- Używaj komentarzy liniowych dla złożonej logiki
- Komentarze w kodzie pisz w języku angielskim

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
   - Czytelność i przejrzystość
   - Kompletna dokumentacja

2. **Testy**
   - Wystarczające pokrycie
   - Wszystkie testy przechodzą
   - Pokrycie przypadków brzegowych

3. **Dokumentacja**
   - Zaktualizowana
   - Jasne wyjaśnienia
   - Wielojęzyczna

4. **Zgodność**
   - Zgodność wsteczna
   - Brak zmian przełamujących (chyba że zgłoszonych)
   - Przestrzeganie wersjonowania semantycznego

### Oś czasu przeglądu

- Wstępny przegląd: 1–3 dni
- Integracja uwag: według potrzeb
- Scalanie: po zatwierdzeniu

---

## Często zadawane pytania

### PR zostało odrzucone

**Przyczyny**:
- Nieprzestrzeganie wytycznych
- Niewystarczające testy
- Niezgłoszone zmiany przełamujące
- Słaba jakość kodu

**Rozwiązanie**:
- Rozwiąż uwagi
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

- Twórz Issue dla błędów
- Rozpocznij Discussion dla pytań
- Oznacz opiekunów dla pilnych spraw

---

## Podziękowania

Współtwórcy otrzymają uznanie w:
- Sekcji współtwórców w README.md
- Notkach wydania
- Dokumentacji projektu

---

## Licencja

Wnosząc swój wkład, zgadzasz się, że będzie on objęty licencją Apache 2.0.

---

## Następne kroki

- 📚 Przeczytaj [dokumentację](../)
- 🐛 Zobacz [otwarte zgłoszenia](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Rozpocznij [dyskusję](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Zrób Fork i zacznij współtworzyć!

Dziękujemy za współtworzenie projektu SiliconLifeCollective!🎉
