# PluginDemo-18: Możliwość odrzucona — Antywzorzec niedeklarowalnej możliwości

## Przegląd

Ten plugin jest **antywzorcem** pokazującym, że deklarowanie możliwości NIE omija zakazów niedeklarowalnych możliwości. Nawet z `[PluginCapability(Capability.Network)]`, P/Invoke, Unsafe, Reflection.Emit i dostęp do rejestru pozostają **zawsze** zablokowane.

## Możliwości deklarowalne vs. niedeklarowalne

### ✅ Deklarowalne (istnieją wartości wyliczenia Capability)

| Możliwość | Co zwalnia |
|----------|-----------|
| `Capability.Network` | Przestrzenie nazw System.Net.* i zakazy wg typu |
| `Capability.FileIO` | Przestrzeń nazw System.IO (poza białą listą) |
| `Capability.Process` | Typy Process* pod System.Diagnostics |
| `Capability.AI` | Umożliwia wstrzyknięcie IAIService (bez zwolnienia TypeRef) |

### ❌ Niedeklarowalne (NIE istnieje wartość wyliczenia Capability)

| Kategoria | Zablokowane typy | Dlaczego niedeklarowalne |
|----------|-----------------|------------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory`, `NativeLibrary` | Nie można audytować arbitralnego kodu natywnego w czasie wykonania |
| Niebezpieczny kod | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Omija bezpieczeństwo typów CLR i sprawdzanie granic |
| Emisja IL | `System.Reflection.Emit.*` | Może generować arbitralny IL w czasie wykonania |
| Ładowanie zestawów | `System.Runtime.Loader`, `Assembly.Load*` | Może ładować nieskanowane DLL, omijając skanowanie bezpieczeństwa |
| Rejestr | `Microsoft.Win32.*` | Dostęp do systemu na poziomie OS poza piaskownicą pluginu |
| Kompilacja dynamiczna | `Microsoft.CodeAnalysis.*` | Może kompilować i wykonywać arbitralny kod |
| Niebezpieczna refleksja | `Type.GetType(string)`, `Activator.CreateInstance` | Może tworzyć instancje zabronionych typów przez ciąg |

## Dlaczego te możliwości nie mogą być zadeklarowane

Podstawowa przyczyna: **nie można ich bezpiecznie audytować w czasie wykonania.**

1. **P/Invoke**: Gdy kod natywny zostanie wywołany, CLR nie ma widoczności — brak gwarancji bezpieczeństwa
2. **Unsafe**: Omija system bezpieczeństwa typów, od którego zależy model bezpieczeństwa pluginu
3. **Reflection.Emit**: Może generować nowy IL w czasie wykonania, nigdy nie skanowany przez PluginLoader
4. **AssemblyLoadContext**: Może ładować DLL, które nigdy nie zostały skanowane pod kątem bezpieczeństwa
5. **Registry**: Zapewnia dostęp do konfiguracji na poziomie OS poza piaskownicą pluginu

## Przetwarzanie „nieprawidłowej deklaracji" przez PluginLoader

Gdy PluginLoader napotyka deklarację możliwości:

1. Odczytuje wartość wyliczenia int32 z blobu CustomAttribute
2. Sprawdza `Enum.IsDefined(typeof(Capability), value)`
3. Jeśli wartość nie jest zdefiniowanym członkiem Capability → **cicho ignorowana**
4. Jeśli wartość jest zdefiniowana → stosowane są reguły zwolnień
5. **Kontrola niedeklarowalnych możliwości jest ZAWSZE egzekwowana** niezależnie od zadeklarowanych możliwości

To zapobiega deklarowaniu przez pluginy „przyszłych" możliwości, które jeszcze nie istnieją.

## Porównanie z 13-CapabilityNetwork

| Aspekt | 13-CapabilityNetwork (pozytywny) | 18-CapabilityDenied (antywzorzec) |
|--------|---------------------------------|----------------------------------|
| Deklaracja | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| Używa HttpClient | ✅ Zwolnione | ✅ Zwolnione |
| Używa DllImport | Nie dotyczy | ❌ ZAWSZE zablokowane |
| Używa Unsafe | Nie dotyczy | ❌ ZAWSZE zablokowane |
| Wynik ładowania | ✅ ZAŁADOWANY | ❌ ODRZUCONY |

## Pliki

- `Plugin.cs` — Plugin demonstracyjny antywzorca niedeklarowalnej możliwości
- `README.md` — Ten plik (Angielski)
- `README.zh-CN.md` — Chiński uproszczony
- Tłumaczenia: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Powiązane przykłady

- **13-CapabilityNetwork**: Pozytywny przykład Capability.Network
- **11-ForbiddenPInvoke**: Antywzorzec P/Invoke (żadna możliwość nie pomoże)
- **10-ForbiddenReflection**: Antywzorzec Reflection (żadna możliwość nie pomoże)
