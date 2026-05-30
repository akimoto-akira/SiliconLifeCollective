# Przewodnik rozwiązywania problemów

> **Wersja: v0.2.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md) | [Русский](../ru-RU/troubleshooting.md)

## Częste problemy

### Budowanie i kompilacja

#### Problem: budowanie nie powiodło się, brakujące zależności

**Objawy**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Rozwiązanie**:
```bash
dotnet restore
dotnet build
```

#### Problem: nie znaleziono .NET SDK

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

#### Problem: połączenie z Ollama odrzucone

**Objawy**:
```
Failed to connect to Ollama at http://localhost:11434
```

**Rozwiązanie**:
```bash
# Sprawdź, czy Ollama jest uruchomiona
ollama list

# Uruchom Ollama
ollama serve

# Przetestuj połączenie
curl http://localhost:11434/api/tags
```

#### Problem: nie znaleziono modelu

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

#### Problem: błąd 404 DashScope

**Objawy**:
```
HTTP 404: Model not found
```

**Rozwiązanie**:
1. Zweryfikuj poprawność klucza API
2. Sprawdź, czy nazwa modelu odpowiada katalogowi DashScope
3. Zweryfikuj poprawność punktu końcowego regionu
4. Sprawdź, czy konto ma dostęp do tego modelu

#### Problem: niepowodzenie połączenia Volcengine Ark

**Objawy**:
```
HTTP 401: Unauthorized
lub
HTTP 404: Endpoint not found
```

**Rozwiązanie**:
1. Zweryfikuj poprawność klucza API
2. Sprawdź, czy format URL punktu końcowego jest poprawny (domyślnie: `https://ark.cn-beijing.volces.com/api/v3/chat/completions`)
3. Upewnij się, że parametr Model używa ID punktu dostępowego wnioskowania (np. `ep-20241212123456-abcde`), a nie nazwy modelu
4. Sprawdź, czy konto ma dostęp do tego punktu dostępowego

---

### Problemy w czasie wykonywania

#### Problem: port jest już zajęty

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

#### Problem: istota nie może się uruchomić

**Objawy**:
- Stan istoty wyświetla „Error"
- Dzienniki pokazują niepowodzenie inicjalizacji

**Rozwiązanie**:
1. Sprawdź, czy Plik Duszy istnieje i jest prawidłowy
2. Zweryfikuj, że klient AI jest skonfigurowany
3. Sprawdź dzienniki, aby uzyskać szczegółowy błąd:
```bash
tail -f logs/*.log
```

#### Problem: brak pamięci

**Objawy**:
```
OutOfMemoryException
```

**Rozwiązanie**:
1. **SiliconLife.Default**: zwiększ rozmiar sterty:
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast**: wersja Fast sama w sobie zużywa więcej pamięci (~500 MB), jeśli pamięci stale brakuje, zaleca się:
   - Zmniejszenie liczby współbieżnych Istot Krzemowych
   - Czyszczenie starych danych w celu zwolnienia pamięci

3. Czyszczenie starych danych:
```bash
# Zarchiwizuj stare dzienniki
mv logs/ logs-archive/
mkdir logs

# Wyczyść stare wspomnienia
# Przez Web UI: Zarządzanie pamięcią > Czyszczenie
```

> **Wskazówka**: SiliconLife.Default zużywa mniej pamięci (~200 MB), co jest odpowiednie dla środowisk z ograniczoną pamięcią; SiliconLife.Fast zużywa więcej pamięci, ale oferuje lepszą wydajność, odpowiednią dla środowisk produkcyjnych.

---

### Problemy z uprawnieniami

#### Problem: uprawnienie odrzucone

**Objawy**:
```
Permission denied: FileAccess C:\Windows
```

**Rozwiązanie**:
1. Sprawdź bieżące uprawnienia:
```bash
curl http://localhost:8080/api/permissions/list
```

2. Nadaj uprawnienie:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

3. Lub użyj Web UI: Zarządzanie uprawnieniami

#### Problem: uprawnienia nie wygasają

**Objawy**:
- Uprawnienia są nadal ważne po czasie wygaśnięcia

**Rozwiązanie**:
1. Sprawdź synchronizację zegara systemowego
2. Zweryfikuj, że pole `expiresAt` jest poprawnie ustawione
3. Wyczyść pamięć podręczną uprawnień

---

### Problemy z Web UI

#### Problem: brak dostępu do Web UI

**Objawy**:
- Przeglądarka wyświetla „Connection refused"

**Rozwiązanie**:
1. Zweryfikuj, że serwer jest uruchomiony
2. Sprawdź poprawny URL: `http://localhost:8080`
3. Sprawdź ustawienia zapory sieciowej
4. Sprawdź dzienniki pod kątem błędów uruchamiania

#### Problem: SSE nie działa

**Objawy**:
- Aktualizacje w czasie rzeczywistym nie pojawiają się
- Czat nie jest strumieniowany

**Rozwiązanie**:
1. Sprawdź, czy przeglądarka obsługuje SSE
2. Wyłącz buforowanie proxy dla SSE
3. Sprawdź stabilność sieci
4. Spróbuj innej przeglądarki

#### Problem: UI wygląda uszkodzony

**Objawy**:
- Style są niepoprawne
- Układ jest zepsuty

**Rozwiązanie**:
1. Wyczyść pamięć podręczną przeglądarki
2. Spróbuj innej skórki: Ustawienia > Skórka
3. Sprawdź błędy w konsoli przeglądarki
4. Wyłącz rozszerzenia przeglądarki

---

### Problemy z przechowywaniem

#### Problem: nie można odczytać/zapisać danych

**Objawy**:
```
IOException: Access denied
```

**Rozwiązanie**:
1. Sprawdź uprawnienia plików
2. Zweryfikuj, że ścieżka przechowywania istnieje
3. Sprawdź miejsce na dysku
4. Uruchom z odpowiednimi uprawnieniami

#### Problem: uszkodzenie danych

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

#### Problem: uszkodzenie pliku przechowywania SpeedyPack (wersja Fast)

**Objawy**:
- Pliki `.spk` nie mogą być załadowane
- Inicjalizacja SpeedyStorage nie powiodła się

**Rozwiązanie**:
1. Użyj narzędzia `SiliconLife.Speedy.Manager` do sprawdzenia i naprawy plików `.spk`
2. Sprawdź, czy plik indeksu `.spk.idx` odpowiada plikowi `.spk`
3. Jeśli plik indeksu jest uszkodzony, usuń plik `.spk.idx` — system automatycznie odbuduje indeks
4. Przywróć plik `.spk` z kopii zapasowej

#### Problem: niepowodzenie automatycznej kompakcji SpeedyPack (wersja Fast)

**Objawy**:
- Pliki `.spk` stale rosną
- Brak miejsca na dysku

**Rozwiązanie**:
1. Sprawdź, czy `SpeedyPackAutoCompactor` działa poprawnie
2. Ręcznie wyzwól operację kompakcji
3. Sprawdź konfigurację progu kompakcji
4. Użyj narzędzia `SiliconLife.Speedy.Manager` do ręcznej kompakcji

---

### Problemy z wykonywaniem narzędzi

#### Problem: nie znaleziono narzędzia

**Objawy**:
```
Tool "xyz" not found
```

**Rozwiązanie**:
1. Zweryfikuj poprawność nazwy narzędzia
2. Sprawdź, czy narzędzie znajduje się w katalogu Tools
3. Odbuduj projekt
4. Sprawdź, czy narzędzie jest poprawnie zaimplementowane

#### Problem: narzędzie zwraca błąd

**Objawy**:
```
Tool execution failed: ...
```

**Rozwiązanie**:
1. Sprawdź dzienniki narzędzia
2. Zweryfikuj parametry wejściowe
3. Przetestuj narzędzie niezależnie
4. Sprawdź uprawnienia

---

### Problemy z wtyczkami

#### Problem: ładowanie wtyczki nie powiodło się

**Objawy**:
```
Plugin load failed: Security check failed
```

**Rozwiązanie**:
1. Sprawdź, czy wtyczka odwołuje się do niezdeklarowalnych zakazanych przestrzeni nazw (np. `System.Runtime.InteropServices`, `System.Reflection.Emit`, `Microsoft.CodeAnalysis`)
2. Jeśli wtyczka wymaga `System.IO` lub `System.Net.Http`, upewnij się, że zadeklarowała możliwości `FileIO` lub `Network` poprzez `[PluginCapability]`
3. Zweryfikuj, że wtyczka odwołuje się tylko do zestawów z białej listy zaufanych zestawów
4. Sprawdź, czy wtyczka poprawnie implementuje interfejs `IPlugin`
5. Zobacz dzienniki, aby uzyskać szczegółowe przyczyny niepowodzenia sprawdzania bezpieczeństwa

#### Problem: narzędzia wtyczki nie są zarejestrowane

**Objawy**:
- Wtyczka załadowana pomyślnie, ale narzędzia nie pojawiają się na liście narzędzi

**Rozwiązanie**:
1. Upewnij się, że klasa narzędzia w wtyczce poprawnie implementuje interfejs `ITool`
2. Sprawdź, czy klasa narzędzia jest publiczna
3. Zweryfikuj, czy `ToolManager.ScanAllPluginAssemblies()` zostało wywołane
4. Odbuduj wtyczkę i uruchom ponownie aplikację

---

### Problemy z notatkami pracy

#### Problem: nie można utworzyć notatki pracy

**Objawy**:
```
Failed to create work note
```

**Rozwiązanie**:
1. Sprawdź, czy istota istnieje i jest w stanie uruchomionym
2. Zweryfikuj, że ścieżka przechowywania ma uprawnienia do zapisu
3. Sprawdź, czy treść nie jest pusta (treść jest wymagana)
4. Zobacz dzienniki, aby uzyskać szczegółowe informacje o błędzie

#### Problem: wyszukiwanie notatek nie zwraca wyników

**Objawy**:
- Wyszukiwanie słowa kluczowego zwraca puste wyniki
- Ale na pewno istnieją powiązane notatki

**Rozwiązanie**:
1. Sprawdź poprawność pisowni słowa kluczowego
2. Spróbuj użyć bardziej ogólnego słowa kluczowego
3. Zweryfikuj, czy notatki zawierają to słowo kluczowe (wielkość liter ma znaczenie)
4. Zwiększ wartość parametru `max_results`

#### Problem: powolne generowanie spisu treści notatek

**Objawy**:
- Długi czas odpowiedzi podczas generowania spisu treści
- Istota ma dużą liczbę notatek (>1000 stron)

**Rozwiązanie**:
1. Jest to normalne zjawisko — wymaga przejścia przez wszystkie notatki
2. Rozważ regularne archiwizowanie starych notatek
3. Użyj funkcji wyszukiwania zamiast przeglądania spisu treści
4. Planowana optymalizacja: dodanie mechanizmu pamięci podręcznej spisu treści

---

### Problemy z Siecią Wiedzy

#### Problem: zapytanie wiedzy zwraca puste wyniki

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
  "query": "słowo_kluczowe"
}
```

#### Problem: niepowodzenie wyszukiwania ścieżki wiedzy

**Objawy**:
```
No path found between concepts
```

**Rozwiązanie**:
1. Zweryfikuj, że oba pojęcia istnieją w Sieci Wiedzy
2. Sprawdź, czy istnieje ścieżka powiązania (może nie być bezpośredniego lub pośredniego związku)
3. Spróbuj dodać więcej wiedzy, aby nawiązać połączenie
4. Zmniejsz limit długości ścieżki (jeśli jest ustawiony)

#### Problem: niepowodzenie walidacji wiedzy

**Objawy**:
```
Knowledge validation failed
```

**Rozwiązanie**:
1. Sprawdź, czy format trójki jest poprawny (podmiot, orzeczenie, dopełnienie są wymagane)
2. Zweryfikuj, że pewność jest w zakresie 0.0–1.0
3. Sprawdź, czy nie ma zduplikowanych trójek
4. Zobacz szczegóły błędu walidacji, aby poznać konkretny problem

#### Problem: niedokładne statystyki Sieci Wiedzy

**Objawy**:
- Liczby statystyczne nie zgadzają się z oczekiwaniami
- Statystyki nie są aktualizowane po dodaniu wiedzy

**Rozwiązanie**:
1. Statystyki mogą wymagać kilku sekund na aktualizację (pamięć podręczna)
2. Sprawdź, czy operacje usuwania zostały pomyślnie wykonane
3. Uruchom ponownie aplikację, aby wymusić odświeżenie statystyk
4. Ponownie odpytaj statystyki przez API

---

### Problemy z zarządzaniem projektami

#### Problem: nie można utworzyć projektu

**Objawy**:
```
Failed to create project
```

**Rozwiązanie**:
1. Sprawdź, czy nazwa projektu nie jest pusta (wymagane)
2. Zweryfikuj, że nazwa projektu nie jest zduplikowana
3. Sprawdź, czy ścieżka przechowywania ma uprawnienia do zapisu
4. Zobacz dzienniki, aby uzyskać szczegółowe informacje o błędzie

#### Problem: utrata danych projektu

**Objawy**:
- Informacje o projekcie nie mogą być załadowane
- Pliki projektu są uszkodzone

**Rozwiązanie**:
1. Sprawdź, czy katalog przechowywania projektu istnieje
2. Przywróć dane projektu z kopii zapasowej
3. Zweryfikuj, że format pliku JSON jest poprawny
4. Ręcznie napraw uszkodzony plik projektu

#### Problem: niepowodzenie przypisania roli w projekcie

**Objawy**:
```
Failed to assign role
```

**Rozwiązanie**:
1. Upewnij się, że Istota Krzemowa dołączyła do projektu
2. Sprawdź, czy nazwa roli jest prawidłowa
3. Zweryfikuj, czy operatorem jest Kurator Krzemowy
4. Zobacz dzienniki, aby uzyskać szczegółowe informacje o błędzie

#### Problem: przepływ pracy nie może się uruchomić

**Objawy**:
- Utworzenie instancji przepływu pracy nie powiodło się
- Przejścia stanów nie są wykonywane

**Rozwiązanie**:
1. Sprawdź, czy szablon przepływu pracy jest zdefiniowany
2. Zweryfikuj, że stan początkowy jest poprawnie ustawiony
3. Upewnij się, że projekt ma przypisany szablon przepływu pracy
4. Sprawdź dzienniki przepływu pracy pod kątem błędów przejść

---

### Problemy z uprawnieniami narzędzi

#### Problem: operacja narzędzia odrzucona

**Objawy**:
```
Tool operation denied: network:post
```

**Rozwiązanie**:
1. Sprawdź konfigurację uprawnień narzędzi Istoty Krzemowej:
```bash
curl http://localhost:8080/api/beings/tool-permissions?beingId=<id>
```

2. Zaktualizuj uprawnienia narzędzi:
```bash
curl -X PUT http://localhost:8080/api/beings/tool-permissions \
  -H "Content-Type: application/json" \
  -d '{
    "beingId": "being-uuid",
    "permissions": {
      "network:post": "allowed"
    }
  }'
```

3. Lub użyj Web UI: Istoty → Uprawnienia narzędzi

#### Problem: uprawnienia narzędzi projektu nie działają

**Objawy**:
- Uprawnienia narzędzi na poziomie projektu nie działają zgodnie z oczekiwaniami

**Rozwiązanie**:
1. Upewnij się, że uprawnienia na poziomie projektu są poprawnie skonfigurowane
2. Sprawdź, czy uprawnienia na poziomie Istoty Krzemowej i projektu nie są w konflikcie
3. Uprawnienia na poziomie projektu są niezależne od poziomu Istoty Krzemowej — stosowane jest przecięcie obu
4. Sprawdź dziennik audytu, aby potwierdzić wyniki sprawdzania uprawnień

---

## Debugowanie

### Włączenie szczegółowych dzienników

Edytuj konfigurację:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Sprawdzanie dzienników

Dzienniki są przechowywane w:
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

### Korzystanie z debugera

**SiliconLife.Default (domyślna implementacja)**:
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

> **Zalecenie**: na etapie debugowania rozwoju zaleca się używanie SiliconLife.Default; po weryfikacji architektury użyj SiliconLife.Fast do wdrożenia produkcyjnego.

---

## Problemy z wydajnością

### Wolny czas odpowiedzi

**Optymalizacje**:
1. Zmniejsz złożoność modelu AI
2. Włącz pamięć podręczną
3. Czyńć stare dane
4. Zwiększ zasoby systemowe

### Wysokie użycie CPU

**Sprawdź**:
- Zbyt wiele uruchomionych istot
- Nieskończone pętle w narzędziach
- Częste wykonywanie czasomierzy

**Rozwiązanie**:
- Zmniejsz liczbę współbieżnych istot
- Zoptymalizuj kod narzędzi
| Dostosuj interwały czasomierzy

### Wysokie użycie pamięci

**Monitorowanie**:
```bash
# Przez Web UI: Pulpit > Pamięć
```

**Optymalizacje**:
- Czyńć stare wspomnienia
- Zmniejsz rozmiar kontekstu
- Zaimplementuj stronicowanie

---

## Uzyskiwanie pomocy

### Przegląd dokumentacji

- [Przewodnik szybkiego startu](getting-started.md)
- [Przewodnik rozwoju](development-guide.md)
- [Referencja API](api-reference.md)
- [Przewodnik architektury](architecture.md)

### Sprawdzanie dzienników

Zawsze najpierw sprawdzaj dzienniki, aby uzyskać szczegóły błędów.

### Wsparcie społeczności

- GitHub Issues: zgłaszanie błędów
- Discussions: zadawanie pytań
- Dokumentacja: wyszukiwanie rozwiązań

---

## Procedury awaryjne

### Awaria systemu

1. Sprawdź dzienniki, aby ustalić przyczynę
2. Uruchom ponownie aplikację:

**SiliconLife.Default (domyślna implementacja)**:
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
3. Sprawdź dzienniki audytu
4. Przejrzyj kontrolę dostępu
5. Uruchom ponownie z ograniczonymi uprawnieniami

---

## Zapobieganie

### Najlepsze praktyki

1. **Regularne kopie zapasowe**
   - Twórz kopie zapasowe katalogu danych
   - Twórz kopie zapasowe konfiguracji
   - Testuj procedury przywracania

2. **Monitorowanie zasobów**
   - Monitoruj użycie CPU/pamięci
   - Monitoruj miejsce na dysku
   - Sprawdzaj połączenia sieciowe

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

- 📚 Przeczytaj [przewodnik architektury](architecture.md)
- 🛠️ Zobacz [przewodnik rozwoju](development-guide.md)
- 🚀 Zobacz [przewodnik szybkiego startu](getting-started.md)
- 🔒 Zobacz [dokumentację bezpieczeństwa](security.md)
