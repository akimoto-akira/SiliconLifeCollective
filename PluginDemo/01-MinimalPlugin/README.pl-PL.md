# Demo Minimalnej Wtyczki

Minimalna implementacja `IPlugin` demonstrująca cykl życia wtyczki z zakodowanymi na sztywno wartościami.

## Przegląd interfejsu IPlugin

Każda wtyczka SiliconLife musi implementować interfejs `IPlugin` zdefiniowany w `SiliconLife.Collective`：

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Podsumowanie właściwości

| Członek | Typ | Opis |
|---------|-----|------|
| `Id` | `string` | Unikalny identyfikator, musi być stabilny między wersjami (np. `"com.siliconlife.demo.minimal"`) |
| `GetName(Language)` | `string` | Czytelna nazwa wyświetlana, lokalizowana przez wyliczenie `Language` |
| `Version` | `string` | Ciąg wersji semantycznej (np. `"1.0.0"`) |
| `GetDescription(Language)` | `string` | Krótki opis funkcjonalności wtyczki |
| `GetAuthor(Language)` | `string` | Nazwa autora lub organizacji |

## Kolejność wywołań cyklu życia

Host wywołuje metody cyklu życia w ścisłej kolejności：

```
OnLoad → OnStart → [Uruchomiona] → OnStop → OnUnload
```

| Metoda | Kiedy wywoływana | Typowe zastosowanie |
|--------|-----------------|---------------------|
| `OnLoad()` | Raz, gdy DLL wtyczki jest ładowana do hosta | Walidacja konfiguracji, rejestracja typów, przygotowanie zasobów |
| `OnStart()` | Gdy host jest w pełni uruchomiony i wszystkie wtyczki załadowane | Interakcja z innymi wtyczkami, uruchamianie zadań w tle |
| `OnStop()` | Gdy host zamyka się poprawnie | Zwolnienie zasobów, opróżnienie buforów, zapis stanu |
| `OnUnload()` | Gdy wtyczka jest usuwana z procesu hosta | Końcowe czyszczenie |

## Ta demo

Ta wtyczka zwraca zakodowane na sztywno wartości dla wszystkich właściwości i pozostawia metody cyklu życia puste. Jest to najprostszy punkt wyjścia do tworzenia wtyczek.

## Uwaga dotycząca bezpieczeństwa

Wtyczki są ładowane w izolowanym `AssemblyLoadContext` i skanowane pod kątem zabronionych odniesień do przestrzeni nazw (np. `System.IO`, `System.Net.Http`). Szczegóły zobacz[dokumentacja bezpieczeństwa](../../docs/pl-PL/security.md).
