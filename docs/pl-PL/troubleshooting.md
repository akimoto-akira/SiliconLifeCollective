# Przewodnik rozwiązywania problemów

> **Wersja: v0.2.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md) | [Polski](../pl-PL/troubleshooting.md)

## Często zadawane pytania

### Budowanie i kompilacja

#### Problem: Budowanie nie powiodło się, brakujące zależności

**Objawy**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Rozwiązanie**:
```bash
dotnet restore
dotnet build
```

#### Problem: Nie znaleziono .NET SDK

**Objawy**:
```
The .NET SDK could not be found
```

**Rozwiązanie**:
1. Zainstaluj .NET 9 SDK: https://dotnet.microsoft.com/download/dotnet/9.0
2. Zweryfikuj instalację:
```bash
dotnet --version
```

---

### Problemy z połączeniem AI

#### Problem: Odrzucono połączenie z Ollama

**Objawy**:
```
Failed to connect to Ollama at http://localhost:11434
```

**Rozwiązanie**:
```bash
# Sprawdź, czy Ollama jest uruchomione
ollama list

# Uruchom Ollama
ollama serve

# Przetestuj połączenie
curl http://localhost:11434/api/tags
```

#### Problem: Nie znaleziono modelu

**Objawy**:
```
model "qwen2.5:7b" not found
```

**Rozwiązanie**:
```bash
# Pobierz wymagany model
ollama pull qwen2.5:7b

# Wyświetl dostępne modele
ollama list
```

#### Problem: Błąd 404 Bailian

**Objawy**:
```
HTTP 404: Model not found
```

**Rozwiązanie**:
1. Zweryfikuj poprawność klucza API
2. Sprawdź, czy nazwa modelu jest zgodna z katalogiem Bailian
3. Zweryfikuj poprawność punktu końcowego regionalnego
4. Sprawdź, czy konto ma dostęp do tego modelu

#### Problem: Niepowodzenie połączenia Volcengine Ark

**Objawy**:
```
HTTP 401: Unauthorized
lub
HTTP 404: Endpoint not found
```

**Rozwiązanie**:
1. Zweryfikuj poprawność klucza API
2. Sprawdź poprawność formatu URL punktu końcowego (domyślnie: `https://ark.cn-beijing.volces.com/api/v3/chat/completions`)
3. Potwierdź, że parametr Model używa identyfikatora punktu dostępowego wnioskowania (np. `ep-20241212123456-abcde`), a nie nazwy modelu
4. Sprawdź, czy konto ma dostęp do tego punktu dostępowego

---

### Problemy w czasie działania

#### Problem: Port jest zajęty

**Objawy**:
```
HttpListenerException: Address already in use
```

**Rozwiązanie**:

**Windows**:
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac**:
```bash
lsof -ti:8080 | xargs kill -9
```

**Lub zmień port w konfiguracji**.

#### Problem: Istota nie może się uruchomić

**Objawy**:
- Stan istoty wyświetla "Error"
- Logi pokazują niepowodzenie inicjalizacji

**Rozwiązanie**:
1. Sprawdź, czy plik duszy istnieje i jest prawidłowy
2. Zweryfikuj, czy klient AI jest skonfigurowany
3. Sprawdź logi w poszukiwaniu konkretnych błędów:
```bash
tail -f logs/*.log
```

#### Problem: Brak pamięci

**Objawy**:
```
OutOfMemoryException
```

**Rozwiązanie**:
1. **SiliconLife.Default**: zwiększ rozmiar sterty:
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast**: wersja Fast sama w sobie zużywa więcej pamięci (~500MB), jeśli pamięć stale brakuje, zaleca się:
   - Zmniejszenie liczby współbieżnych Istot Krzemowych
   - Oczyszczenie starych danych w celu zwolnienia pamięci

3. Oczyszczenie starych danych:
```bash
# Archiwizacja starych logów
mv logs/ logs-archive/
mkdir logs

# Oczyszczenie starych wspomnień
# Przez Web UI: Zarządzanie pamięcią > Oczyszczanie
```

> **Wskazówka**: SiliconLife.Default ma niższe zużycie pamięci (~200MB), odpowiednie dla środowisk z ograniczoną pamięcią; SiliconLife.Fast ma wyższe zużycie pamięci, ale lepszą wydajność, odpowiednie dla środowisk produkcyjnych.

---

### Problemy z uprawnieniami

#### Problem: Odmowa uprawnień

**Objawy**:
```
Permission denied: disk:write
```

**Rozwiązanie**:
1. Sprawdź bieżące uprawnienia:
```bash
curl http://localhost:8080/api/permissions/list?beingId=being-uuid
```

2. Nadaj uprawnienia:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "beingId": "being-uuid",
    "permissionType": "Disk",
    "resourcePrefix": "disk:write",
    "result": "Allowed"
  }'
```

3. Lub użyj Web UI: Zarządzanie uprawnieniami

#### Problem: Uprawnienia nie wygasają

**Objawy**:
- Uprawnienia są nadal ważne po czasie wygaśnięcia

**Rozwiązanie**:
1. Sprawdź synchronizację zegara systemowego
2. Zweryfikuj, czy pole `expiresAt` jest ustawione poprawnie
3. Wyczyść pamięć podręczną uprawnień

---

### Problemy z Web UI

#### Problem: Brak dostępu do Web UI

**Objawy**:
- Przeglądarka wyświetla "Connection refused"

**Rozwiązanie**:
1. Zweryfikuj, że serwer jest uruchomiony
2. Sprawdź poprawny URL: `http://localhost:8080`
3. Sprawdź ustawienia zapory sieciowej
4. Sprawdź logi w poszukiwaniu błędów uruchamiania

#### Problem: SSE nie działa

**Objawy**:
- Aktualizacje w czasie rzeczywistym nie pojawiają się
- Czat nie jest przesyłany strumieniowo

**Rozwiązanie**:
1. Sprawdź, czy przeglądarka obsługuje SSE
2. Wyłącz buforowanie proxy dla SSE
3. Sprawdź stabilność sieci
4. Spróbuj użyć innej przeglądarki

#### Problem: Interfejs wygląda uszkodzony

**Objawy**:
- Style są nieprawidłowe
- Układ jest zepsuty

**Rozwiązanie**:
1. Wyczyść pamięć podręczną przeglądarki
2. Spróbuj użyć innego motywu: Ustawienia > Motyw
3. Sprawdź błędy w konsoli przeglądarki
4. Wyłącz rozszerzenia przeglądarki

---

### Problemy z przechowywaniem danych

#### Problem: Nie można odczytać/zapisać danych

**Objawy**:
```
IOException: Access denied
```

**Rozwiązanie**:
1. Sprawdź uprawnienia plików
2. Zweryfikuj, czy ścieżka przechowywania istnieje
3. Sprawdź miejsce na dysku
4. Uruchom z odpowiednimi uprawnieniami

#### Problem: Uszkodzenie danych

**Objawy**:
- Błędy parsowania JSON
- Utrata danych

**Rozwiązanie**:
1. Przywróć z kopii zapasowej
2. Sprawdź integralność przechowywania:
```bash
# Przez Web UI: System > Sprawdzanie przechowywania
```

3. Ręcznie napraw uszkodzone pliki

#### Problem: Uszkodzenie pliku przechowywania SpeedyPack (wersja Fast)

**Objawy**:
- Plik `.spk` nie może zostać załadowany
- Inicjalizacja SpeedyStorage nie powiodła się

**Rozwiązanie**:
1. Użyj narzędzia `SiliconLife.Speedy.Manager` do sprawdzenia i naprawy plików `.spk`
2. Sprawdź, czy plik indeksu `.spk.idx` jest zgodny z plikiem `.spk`
3. Jeśli plik indeksu jest uszkodzony, usuń plik `.spk.idx`, system automatycznie odbuduje indeks
4. Przywróć plik `.spk` z kopii zapasowej

#### Problem: Niepowodzenie automatycznej kompresji SpeedyPack (wersja Fast)

**Objawy**:
- Plik `.spk` stale rośnie
- Brak miejsca na dysku

**Rozwiązanie**:
1. Sprawdź, czy `SpeedyPackAutoCompactor` działa poprawnie
2. Ręcznie wyzwól operację kompresji
3. Sprawdź konfigurację progu kompresji
4. Użyj narzędzia `SiliconLife.Speedy.Manager` do ręcznej kompresji

---

### Problemy z wykonywaniem narzędzi

#### Problem: Nie znaleziono narzędzia

**Objawy**:
```
Tool "xyz" not found
```

**Rozwiązanie**:
1. Zweryfikuj poprawność nazwy narzędzia
2. Sprawdź, czy narzędzie znajduje się w katalogu Tools
3. Przebuduj projekt
4. Sprawdź, czy narzędzie jest poprawnie zaimplementowane

#### Problem: Narzędzie zwraca błąd

**Objawy**:
```
Tool execution failed: ...
```

**Rozwiązanie**:
1. Sprawdź logi narzędzia
2. Zweryfikuj parametry wejściowe
3. Przetestuj narzędzie niezależnie
4. Sprawdź uprawnienia

---

### Problemy z wtyczkami

#### Problem: Niepowodzenie ładowania wtyczki

**Objawy**:
```
Plugin load failed: Security check failed
```

**Rozwiązanie**:
1. Sprawdź, czy wtyczka odwołuje się do zabronionych przestrzeni nazw (`System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`)
2. Zweryfikuj, czy wtyczka odwołuje się tylko do zestawów z białej listy zaufanych zestawów
3. Sprawdź, czy wtyczka poprawnie implementuje interfejs `IPlugin`
4. Przejrzyj logi, aby uzyskać szczegółowe przyczyny niepowodzenia sprawdzania bezpieczeństwa

#### Problem: Narzędzia wtyczki nie są zarejestrowane

**Objawy**:
- Wtyczka została załadowana pomyślnie, ale narzędzia nie pojawiają się na liście narzędzi

**Rozwiązanie**:
1. Potwierdź, że klasy narzędzi w wtyczce poprawnie implementują interfejs `ITool`
2. Sprawdź, czy klasy narzędzi są publiczne
3. Zweryfikuj, czy `ToolManager.ScanAllPluginAssemblies()` zostało wywołane
4. Przebuduj wtyczkę i uruchom ponownie aplikację

---

### Problemy z notatkami roboczymi

#### Problem: Nie można utworzyć notatki roboczej

**Objawy**:
```
Failed to create work note
```

**Rozwiązanie**:
1. Sprawdź, czy istota istnieje i jest w stanie działania
2. Zweryfikuj, czy ścieżka przechowywania ma uprawnienia do zapisu
3. Sprawdź, czy treść nie jest pusta (treść jest wymagana)
4. Przejrzyj logi, aby uzyskać szczegółowe informacje o błędach

#### Problem: Wyszukiwanie notatek nie zwraca wyników

**Objawy**:
- Wyszukiwanie słów kluczowych zwraca puste wyniki
- Ale na pewno istnieją powiązane notatki

**Rozwiązanie**:
1. Sprawdź poprawność pisowni słów kluczowych
2. Spróbuj użyć bardziej ogólnych słów kluczowych
3. Zweryfikuj, czy notatki zawierają to słowo kluczowe (wielkość liter ma znaczenie)
4. Zwiększ wartość parametru `max_results`

#### Problem: Wolne generowanie katalogu notatek

**Objawy**:
- Długi czas odpowiedzi podczas generowania katalogu
- Istota ma dużą liczbę notatek (>1000 stron)

**Rozwiązanie**:
1. Jest to normalne zjawisko, wymaga przejścia przez wszystkie notatki
2. Rozważ regularne archiwizowanie starych notatek
3. Użyj funkcji wyszukiwania zamiast przeglądania katalogu
4. Planowana optymalizacja: dodanie mechanizmu buforowania katalogu

---

### Problemy z siecią wiedzy

#### Problem: Zapytanie o wiedzę zwraca puste wyniki

**Objawy**:
```
No knowledge triples found
```

**Rozwiązanie**:
1. Zweryfikuj pisownię podmiotu i orzeczenia
2. Sprawdź, czy wiedza została dodana do sieci
3. Użyj funkcji wyszukiwania do dopasowania rozmytego:
```json
{
  "action": "search",
  "query": "słowo kluczowe"
}
```

#### Problem: Niepowodzenie wyszukiwania ścieżki wiedzy

**Objawy**:
```
No path found between concepts
```

**Rozwiązanie**:
1. Zweryfikuj, czy oba pojęcia istnieją w sieci wiedzy
2. Sprawdź, czy istnieje ścieżka powiązania (może nie być bezpośredniego lub pośredniego związku)
3. Spróbuj dodać więcej wiedzy, aby nawiązać połączenie
4. Zmniejsz limit długości ścieżki (jeśli jest ustawiony)

#### Problem: Niepowodzenie walidacji wiedzy

**Objawy**:
```
Knowledge validation failed
```

**Rozwiązanie**:
1. Sprawdź, czy format trójki jest poprawny (podmiot, orzeczenie, dopełnienie są wymagane)
2. Zweryfikuj, czy pewność siebie jest w zakresie 0.0-1.0
3. Sprawdź, czy nie ma zduplikowanych trójek
4. Przejrzyj szczegóły błędu walidacji, aby zrozumieć konkretny problem

#### Problem: Nieprawidłowe statystyki sieci wiedzy

**Objawy**:
- Liczby statystyczne nie są zgodne z oczekiwaniami
- Statystyki nie są aktualizowane po dodaniu wiedzy

**Rozwiązanie**:
1. Statystyki mogą wymagać kilku sekund na aktualizację (pamięć podręczna)
2. Sprawdź, czy operacje usuwania zostały pomyślnie wykonane
3. Uruchom ponownie aplikację, aby wymusić odświeżenie statystyk
4. Ponownie zapytaj o statystyki przez API

---

### Problemy z zarządzaniem projektami

#### Problem: Nie można utworzyć projektu

**Objawy**:
```
Failed to create project
```

**Rozwiązanie**:
1. Sprawdź, czy nazwa projektu nie jest pusta (wymagana)
2. Zweryfikuj, czy nazwa projektu nie jest zduplikowana
3. Sprawdź, czy ścieżka przechowywania ma uprawnienia do zapisu
4. Przejrzyj logi, aby uzyskać szczegółowe informacje o błędach

#### Problem: Utrata danych projektu

**Objawy**:
- Informacje o projekcie nie mogą być załadowane
- Pliki projektu są uszkodzone

**Rozwiązanie**:
1. Sprawdź, czy katalog przechowywania projektu istnieje
2. Przywróć dane projektu z kopii zapasowej
3. Zweryfikuj, czy format pliku JSON jest poprawny
4. Ręcznie napraw uszkodzone pliki projektu

---

## Debugowanie

### Włączanie szczegółowych logów

Edytuj konfigurację:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Sprawdzanie logów

Logi są przechowywane w:
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

Podgląd w czasie rzeczywistym:
```bash
tail -f logs/*.log
```

### Używanie debugera

**SiliconLife.Default (implementacja domyślna)**:
```bash
# Uruchom z debugerem
dotnet run --project src/SiliconLife.Default --configuration Debug

# Dołącz debuger
# Przez IDE: Dołącz do procesu > SiliconLife.Default
```

**SiliconLife.Fast (wersja wysokowydajna)**:
```bash
# Uruchom z debugerem
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Dołącz debuger
# Przez IDE: Dołącz do procesu > SiliconLife.Fast
```

> **Zalecenie**: Na etapie debugowania rozwoju zaleca się używanie SiliconLife.Default, a po weryfikacji architektury używać SiliconLife.Fast do wdrożenia produkcyjnego.

---

## Problemy z wydajnością

### Wolny czas odpowiedzi

**Optymalizacja**:
1. Zmniejsz złożoność modelu AI
2. Włącz buforowanie
3. Oczyść stare dane
4. Zwiększ zasoby systemowe

### Wysokie użycie CPU

**Sprawdź**:
- Uruchomiono zbyt wiele istot
- Nieskończona pętla w narzędziach
- Częste wykonywanie timerów

**Rozwiązanie**:
- Zmniejsz liczbę współbieżnych istot
- Zoptymalizuj kod narzędzi
- Dostosuj interwały timerów

### Wysokie użycie pamięci

**Monitorowanie**:
```bash
# Przez Web UI: Pulpit nawigacyjny > Pamięć
```

**Optymalizacja**:
- Oczyść stare wspomnienia
- Zmniejsz rozmiar kontekstu
- Wdróż stronicowanie

---

## Uzyskiwanie pomocy

### Przegląd dokumentacji

- [Przewodnik szybkiego startu](getting-started.md)
- [Przewodnik rozwoju](development-guide.md)
- [Referencja API](api-reference.md)
- [Przewodnik architektury](architecture.md)

### Sprawdzanie logów

Zawsze najpierw sprawdzaj logi, aby uzyskać szczegóły błędów.

### Wsparcie społeczności

- GitHub Issues: zgłaszanie błędów
- Discussions: zadawanie pytań
- Dokumentacja: wyszukiwanie rozwiązań

---

## Procedury awaryjne

### Awaria systemu

1. Sprawdź logi, aby ustalić przyczynę
2. Uruchom ponownie aplikację:

**SiliconLife.Default (implementacja domyślna)**:
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (rekomendowana wersja produkcyjna)**:
```bash
dotnet run --project src/SiliconLife.Fast
```

3. W razie potrzeby przywróć z kopii zapasowej

### Utrata danych

1. Natychmiast zatrzymaj aplikację
2. Sprawdź pliki kopii zapasowej
3. Przywróć dane
4. Zweryfikuj integralność

### Luka bezpieczeństwa

1. Zatrzymaj wszystkie istoty
2. Odwołaj wszystkie uprawnienia
3. Sprawdź logi audytu
4. Przejrzyj kontrolę dostępu
5. Uruchom ponownie z ograniczonymi uprawnieniami

---

## Zapobieganie

### Najlepsze praktyki

1. **Regularne kopie zapasowe**
   - Twórz kopie zapasowe katalogu danych
   - Twórz kopie zapasowe konfiguracji
   - Testuj proces przywracania

2. **Monitorowanie zasobów**
   - Monitoruj użycie CPU/pamięci
   - Monitoruj miejsce na dysku
   - Sprawdzaj połączenie sieciowe

3. **Utrzymywanie aktualności**
   - Aktualizuj .NET SDK
   - Aktualizuj zależności
   - Stosuj poprawki bezpieczeństwa

4. **Testowanie zmian**
   - Najpierw testuj w środowisku deweloperskim
   - Używaj kontroli wersji
   - Dokumentuj zmiany

---

## Następne kroki

- 📚 Przeczytaj [Przewodnik architektury](architecture.md)
- 🛠️ Zobacz [Przewodnik rozwoju](development-guide.md)
- 🚀 Zobacz [Przewodnik szybkiego startu](getting-started.md)
- 🔒 Zobacz [Dokumentację bezpieczeństwa](security.md)
