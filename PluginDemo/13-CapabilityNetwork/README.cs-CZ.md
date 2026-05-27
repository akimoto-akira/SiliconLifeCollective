# PluginDemo-13: Capability.Network — Deklarativní síťové oprávnění

## Přehled

Tento plugin demonstruje použití `[PluginCapability(Capability.Network)]` k deklaraci přístupu k síti. S touto deklarací plugin získá přístup k typům `System.Net.*`, které by jinak byly zablokovány bezpečnostním skenem PluginLoader.

## Syntaxe deklarace

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**Klíčové prvky:**
- **Cíl atributu**: Musí být na třídě, která přímo implementuje `IPlugin`
- **AllowMultiple = true**: Lze skládat více atributů `[PluginCapability]` (viz 17-CapabilityStacked)
- **Pole Reason**: Čitelné vysvětlení pro člověka zapisované do bezpečnostního auditního logu při načítání. **Důrazně se doporučuje poskytnout jasný Reason pro všechny produkční pluginy.**

## Jak PluginLoader zpracovává deklarace schopností

1. **Čtení PE metadat**: PluginLoader čte deklarace schopností z tabulky CustomAttribute PE souboru **před** zahájením bezpečnostního skenu
2. **Uvolnění pravidel skenování**: Deklarované schopnosti osvobozují odpovídající odkazy na typy od kontrol zakázaných jmenných prostorů a zakázaných typů
3. **Auditní logování**: Všechny deklarace (včetně Reason) jsou zapsány do bezpečnostního auditního logu
4. **Nedeklarovatelné schopnosti**: P/Invoke, Unsafe, Reflection.Emit atd. zůstávají zablokovány bez ohledu na jakoukoli deklaraci

## Rozsah výjimky Capability.Network

### Výjimky TypeRef

Když je deklarována `Capability.Network`, následující pravidla zákazů založená na jmenných prostorech a typech jsou uvolněna:

| Výjimka namespace | Povolené typy |
|-------------------|--------------|
| `System.Net.Http` | `HttpClient`, `HttpRequestMessage`, `HttpResponseMessage` atd. |
| `System.Net.WebSockets` | `ClientWebSocket`, `WebSocket` atd. |
| `System.Net.Sockets` | `TcpClient`, `UdpClient`, `Socket` atd. |
| `System.Net.Mail` | `SmtpClient`, `MailMessage` atd. |
| `System.Net.NetworkInformation` | `Ping`, `NetworkInterface` atd. |
| `System.Net.Security` | `SslStream` atd. |
| `System.Net` (zákazy podle typu) | `HttpWebRequest`, `WebClient`, `Dns`, `FtpWebRequest` atd. |

### Výjimky ILString

Řetězcové konstanty začínající těmito předponami nejsou označovány při skenování haldy #US:
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### Co zůstává zakázáno

I s `Capability.Network` tyto schopnosti jsou **vždy** zablokovány (nedeklarovatelné schopnosti):

| Kategorie | Zablokované typy | Proč nedeklarovatelné |
|----------|-----------------|---------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` | Nelze bezpečně auditovat za běhu |
| Unsafe kód | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Obchází záruky bezpečnosti typů |
| IL emise | `System.Reflection.Emit.*` | Může generovat libovolný kód za běhu |
| Načítání assembly | `System.Runtime.Loader`, `Assembly.Load*` | Může obejít bezpečnostní sken načtením neověřených DLL |
| Registr | `Microsoft.Win32.*` | Přístup k systému na úrovni OS mimo sandbox pluginu |

## Pole Reason — Auditní role

Pole `Reason` slouží jako **auditní stopa** pro deklarace schopností:

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Proč je Reason důležitý:**
1. **Bezpečnostní revize**: Auditoři mohou ověřit, že deklarované schopnosti odpovídají skutečnému chování pluginu
2. **Princip nejmenších oprávnění**: Nutí autory pluginů odůvodnit potřebu každé schopnosti
3. **Shoda**: Vyžadováno pro bezpečnostní certifikace a vyšetřování incidentů
4. **Monitorování za běhu**: Bezpečnostní nástroje mohou varovat, pokud využití deklarované schopnosti přesahuje uvedený důvod

## Porovnání s 08-ForbiddenNetwork

| Aspekt | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|--------|-------------------|---------------------|
| Deklarace | Žádná | `[PluginCapability(Capability.Network)]` |
| Výsledek načtení | ❌ Zamítnut | ✅ Úspěšně načten |
| Použití HttpClient | Zablokováno skenem TypeRef | Vynato deklarací |
| Použití TcpClient | Zablokováno skenem TypeRef | Vynato deklarací |
| Reason | Nelze použít | Zapsáno do auditního logu |

**Klíčový rozdíl**: 08-ForbiddenNetwork ukazuje, co se stane, když se použijí síťové typy **bez** deklarace schopnosti. 13-CapabilityNetwork ukazuje **správný** způsob deklarativního vyžádání síťového přístupu.

## Doporučené bezpečnostní postupy

1. **Deklarovat pouze potřebné**: Pokud potřebujete jen HTTP, nedeklarujte Capability.Network jen protože to jde — ale všimněte si, že Capability.Network je jediná síťová schopnost; neexistují jemnější možnosti
2. **Preferovat NetworkExecutor**: `NetworkExecutor` je řízený vstupní bod pro síťový přístup a nevyžaduje žádnou deklaraci schopnosti
3. **Poskytnout jasný Reason**: Nejasné důvody jako „přístup k síti" jsou varovným signálem při bezpečnostní revizi
4. **Respektovat nedeklarovatelná omezení**: Žádná deklarace schopnosti nemůže obejít zákazy P/Invoke, Unsafe nebo Reflection.Emit

## Soubory

- `Plugin.cs` — Demo plugin deklarující Capability.Network
- `README.md` — English
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 日本語
- `README.ko-KR.md` — 한국어
- `README.de-DE.md` — Deutsch
- `README.fr-FR.md` — Français
- `README.es-ES.md` — Español
- `README.it-IT.md` — Italiano
- `README.ru-RU.md` — Русский
- `README.pt-PT.md` — Português
- `README.pl-PL.md` — Polski
- `README.cs-CZ.md` — Tento soubor (Čeština)

## Související příklady

- **08-ForbiddenNetwork**: Antivzor blokovaných síťových operací
- **14-CapabilityFileIO**: Deklarativní schopnost FileIO
- **15-CapabilityProcess**: Deklarativní schopnost Process
- **16-CapabilityAI**: Deklarativní schopnost služby AI
- **17-CapabilityStacked**: Skládání více schopností
- **18-CapabilityDenied**: Antivzor nedeklarovatelné schopnosti
