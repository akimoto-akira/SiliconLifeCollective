# PluginDemo-08: Anti-vzorec Zakázaných Síťových Operací

## Přehled

Tento plugin demonstruje **ZAKÁZANÉ** síťové operace v systému pluginů SiliconLife. Slouží jako reference anti-vzorce, ukazující, co NEDĚLAT, a poskytující správné alternativy.

## Proč je přímý síťový přístup globálně zakázán?

Vzorce přímého síťového přístupu jsou blokovány na úrovni pluginu:

1. **Připojení ke škodlivým serverům**: Pluginy se mohou připojovat ke škodlivým serverům
2. **Exfiltrace dat**: Pluginy mohou vynášet citlivá data z pískoviště
3. **Útoky DNS Rebinding**: Pluginy mohou obcházet bezpečnostní kontroly
4. **Obcházení síťového ACL**: Přímý síťový přístup obchází globální systém ACL

## Zakázané typy

Všechny typy `System.Net`, které přímo přistupují k síti, jsou blokovány:

| Zakázaný typ | Blokovaný prostor názvů | Úroveň rizika |
|----------|----------------|----------|
| `HttpClient` | `System.Net.Http` | 🔴 Kritické |
| `TcpClient` | `System.Net.Sockets` | 🔴 Kritické |
| `Socket` | `System.Net.Sockets` | 🔴 Kritické |
| `Dns` | `System.Net` | 🔴 Kritické |
| `WebClient` | `System.Net` | 🔴 Kritické |

## Bezpečné metody přístupu

### NetworkExecutor (Doporučeno)

`NetworkExecutor` je **controlovaný vstupní bod** pro síťové operace:

```csharp
// ✅ SPRÁVNĚ: Jednoduchý požadavek GET
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

**Co NetworkExecutor poskytuje:**
1. Kontrola oprávnění
2. Protokolování auditu
3. Jistič
4. Kontrola časového limitu
5. Fronta požadavků

## Ukázané porušení

### Porušení 1: HttpClient

```csharp
// ❌ ZAKÁZÁNO
using var client = new HttpClient();

// ✅ SPRÁVNĚ
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

### Porušení 2: TcpClient

```csharp
// ❌ ZAKÁZÁNO
using var client = new TcpClient("example.com", 8080);

// ✅ SPRÁVNĚ
// Použít NetworkExecutor nebo deklarovat Capability.Network
```

## Bezpečnostní mechanismus PluginLoader

PluginLoader analyzuje tento plugin a:
1. **Skenování TypeRef**: Detekuje odkazy na zakázané typy
2. **Skenování MemberRef**: Detekuje volání blokovaných metod
3. **Skenování IL řetězců**: Detekuje pokusy o reflexi
4. **Odmítnutí**: Plugin je odmítnut při načítání

## Soubory

- `Plugin.cs` - Demo plugin anti-vzorce
- `README.md` - Tento soubor (Angličtina)
- `README.cs-CZ.md` - Tento soubor (Čeština)
- Další jazykové verze...

## Související příklady

- **13-CapabilityNetwork**: Deklarativní síťová schopnost
- **07-ForbiddenFileIO**: Zakázané vzorce přístupu k souborům