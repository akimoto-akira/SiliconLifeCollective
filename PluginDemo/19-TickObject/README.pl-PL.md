# PluginDemo-19: TickObject — Zadanie okresowe w MainLoop

## Przegląd

Ten plugin demonstruje użycie `TickObject` do integracji z `MainLoop` dla logiki okresowej/ciągłej. TickObject to klasa bazowa dla obiektów, które mogą być tickowane przez pętlę główną MainLoop, stanowiąc ujednoliconą alternatywę dla `System.Threading.Timer` lub `Task.Delay`.

## Cykl życia TickObject

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → MainLoop.Register(this) wywoływane automatycznie w konstruktorze
    │
    ├── autoRegister=false → wywołać MainLoop.Register(this) ręcznie później
    │
    ▼
MainLoop.Tick() pętla
    │
    ├── Posortować wszystkie zarejestrowane TickObjects po Priority (rosnąco)
    ├── Akumulować elapsedTime dla każdego TickObject
    ├── Jeśli elapsedTime >= Interval → wywołać OnTick(deltaTime)
    │
    ├── Wyłącznik automatyczny: jeśli OnTick przekracza TickTimeout → zwiększyć licznik timeoutów
    │   └── Po maxTimeoutCount kolejnych timeoutach → 1-minutowe ostygnięcie
    │
    ▼
MainLoop.Unregister(tickObject) — czyszczenie w OnStop
```

## Kluczowe właściwości

| Właściwość | Typ | Domyślnie | Opis |
|------------|-----|----------|------|
| `Interval` | `TimeSpan` | Wymagane | Jak często wywoływane jest OnTick |
| `Priority` | `int` | 100 | Kolejność wykonania (niższa = wyższy priorytet) |
| `autoRegister` | `bool` | `true` | Automatyczna rejestracja w MainLoop w konstruktorze |

## Kluczowe metody

| Metoda | Opis |
|--------|------|
| `OnTick(TimeSpan deltaTime)` | Przesłonić, aby zaimplementować logikę okresową |
| `MainLoop.Register(TickObject)` | Ręczna rejestracja w MainLoop |
| `MainLoop.Unregister(TickObject)` | Usunięcie z MainLoop (czyszczenie) |

## Scenariusze demonstracyjne

### 1. Podstawowy timer (autoRegister=true)
```csharp
public class StatusTimer : TickObject
{
    public StatusTimer() : base(interval: TimeSpan.FromSeconds(5), autoRegister: true)
    {
        Priority = 100;
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        Console.WriteLine($"Tick, deltaTime={deltaTime.TotalMilliseconds:F0}ms");
    }
}
```

### 2. Rejestracja ręczna (autoRegister=false)
```csharp
// W konstruktorze: nie rejestrować automatycznie
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// W OnStart: zarejestrować ręcznie
MainLoop.Register(_heartbeatTimer);
```

### 3. Kolejność priorytetów
- `Priority = 10` → Wysoki priorytet, wykonywany jako pierwszy
- `Priority = 200` → Niski priorytet, wykonywany po

### 4. Czyszczenie
```csharp
// W OnStop: zawsze wyrejestrować, aby zapobiec wyciekom
MainLoop.Unregister(_statusTimer);
```

## Wyłącznik automatyczny MainLoop

MainLoop ma wbudowany wyłącznik automatyczny, aby zapobiec blokowaniu całej pętli przez wolne TickObjects:

1. Jeśli `OnTick` przekracza `TickTimeout` (domyślnie 1 sekunda) → licznik timeoutów rośnie
2. Po `maxTimeoutCount` (domyślnie 3) kolejnych timeoutach → wyłącznik automatyczny zadziała
3. Zadziałany TickObject jest **pomijany** przez 1-minutowe ostygnięcie
4. Po ostygnięciu TickObject dostaje kolejną szansę

## TickObject vs System.Threading.Timer

| Aspekt | TickObject + MainLoop | System.Threading.Timer |
|--------|----------------------|----------------------|
| Model wątków | Pojedynczy wątek pętli głównej | Wątki z puli wątków |
| Kolejność wykonania | Deterministyczna (według Priority) | Niedeterministyczna |
| Wyłącznik automatyczny | Wbudowany | Brak |
| Debugowanie | Łatwe (jednowątkowe) | Trudne (warunki wyścigu) |
| Zużycie zasobów | Minimalne (bez puli wątków) | Narzut puli wątków |
| Dokładność interwału | Best-effort (wpływ innych TickObjects) | Bardziej precyzyjny |

## Uwaga dotycząca bezpieczeństwa

TickObject sam w sobie **nie wymaga** deklaracji możliwości. Jest bezpiecznym, wbudowanym mechanizmem frameworka.

## Pliki

- `Plugin.cs` — Plugin demonstracyjny TickObject
- `README.md` — Ten plik (Angielski)
- `README.zh-CN.md` — Chiński uproszczony
- Tłumaczenia: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Powiązane przykłady

- **13-CapabilityNetwork**: Deklaracja Capability.Network
- **20-SpeedyPack**: Magazyn danych bez Capability.FileIO
