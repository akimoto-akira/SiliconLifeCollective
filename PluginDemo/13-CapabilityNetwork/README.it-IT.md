# PluginDemo-13: Capability.Network — Permesso di rete dichiarativo

## Panoramica

Questo plugin dimostra l'uso di `[PluginCapability(Capability.Network)]` per dichiarare l'accesso alla rete. Con questa dichiarazione, il plugin può accedere ai tipi `System.Net.*` che altrimenti sarebbero bloccati dalla scansione di sicurezza di PluginLoader.

## Sintassi di dichiarazione

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**Elementi chiave:**
- **Target dell'attributo**: Deve essere sulla classe che implementa direttamente `IPlugin`
- **AllowMultiple = true**: È possibile impilare più attributi `[PluginCapability]` (vedere 17-CapabilityStacked)
- **Campo Reason**: Spiegazione leggibile dall'uomo scritta nel registro di audit di sicurezza al caricamento. **Fornire un Reason chiaro è fortemente raccomandato per tutti i plugin in produzione.**

## Come PluginLoader elabora le dichiarazioni di capacità

1. **Lettura metadati PE**: PluginLoader legge le dichiarazioni di capacità dalla tabella CustomAttribute del file PE **prima** che inizi la scansione di sicurezza
2. **Rilassamento delle regole di scansione**: Le capacità dichiarate esentano i riferimenti di tipo corrispondenti dai controlli di namespace e tipi proibiti
3. **Registrazione di audit**: Tutte le dichiarazioni (incluso Reason) sono scritte nel registro di audit di sicurezza
4. **Capacità non dichiarabili**: P/Invoke, Unsafe, Reflection.Emit, ecc. rimangono bloccati indipendentemente da qualsiasi dichiarazione

## Ambito di esenzione di Capability.Network

### Esenzioni TypeRef

Quando `Capability.Network` è dichiarata, le seguenti regole di divieto basate su namespace e tipi sono rilassate:

| Namespace esentato | Tipi consentiti |
|-------------------|----------------|
| `System.Net.Http` | `HttpClient`, `HttpRequestMessage`, `HttpResponseMessage`, ecc. |
| `System.Net.WebSockets` | `ClientWebSocket`, `WebSocket`, ecc. |
| `System.Net.Sockets` | `TcpClient`, `UdpClient`, `Socket`, ecc. |
| `System.Net.Mail` | `SmtpClient`, `MailMessage`, ecc. |
| `System.Net.NetworkInformation` | `Ping`, `NetworkInterface`, ecc. |
| `System.Net.Security` | `SslStream`, ecc. |
| `System.Net` (divieti per tipo) | `HttpWebRequest`, `WebClient`, `Dns`, `FtpWebRequest`, ecc. |

### Esenzioni ILString

Le costanti di stringa che iniziano con questi prefissi non vengono contrassegnate nella scansione dell'heap #US:
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### Cosa rimane proibito

Anche con `Capability.Network`, queste capacità sono **sempre** bloccate (capacità non dichiarabili):

| Categoria | Tipi bloccati | Perché non dichiarabile |
|----------|-------------|----------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` | Non può essere auditato in modo sicuro a runtime |
| Codice unsafe | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Aggira le garanzie di sicurezza dei tipi |
| Emissione IL | `System.Reflection.Emit.*` | Può generare codice arbitrario a runtime |
| Caricamento assembly | `System.Runtime.Loader`, `Assembly.Load*` | Può aggirare la scansione di sicurezza caricando DLL non verificate |
| Registro | `Microsoft.Win32.*` | Accesso di sistema a livello OS fuori dal sandbox del plugin |

## Campo Reason — Ruolo di audit

Il campo `Reason` serve come **traccia di audit** per le dichiarazioni di capacità:

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Perché Reason è importante:**
1. **Revisione di sicurezza**: Gli auditor possono verificare che le capacità dichiarate corrispondano al comportamento effettivo del plugin
2. **Principio del minimo privilegio**: Obbliga gli autori di plugin a giustificare la necessità di ogni capacità
3. **Conformità**: Necessario per certificazioni di sicurezza e indagini su incidenti
4. **Monitoraggio a runtime**: Gli strumenti di sicurezza possono avvisare se l'utilizzo della capacità dichiarata supera il motivo indicato

## Confronto con 08-ForbiddenNetwork

| Aspetto | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|---------|-------------------|---------------------|
| Dichiarazione | Nessuna | `[PluginCapability(Capability.Network)]` |
| Risultato caricamento | ❌ Rifiutato | ✅ Caricato con successo |
| Uso di HttpClient | Bloccato dalla scansione TypeRef | Esentato dalla capacità |
| Uso di TcpClient | Bloccato dalla scansione TypeRef | Esentato dalla capacità |
| Reason | Non applicabile | Scritto nel registro di audit |

**Differenza chiave**: 08-ForbiddenNetwork mostra cosa succede quando si usano tipi di rete **senza** dichiarare la capacità. 13-CapabilityNetwork mostra il modo **corretto** di richiedere dichiarativamente l'accesso alla rete.

## Buone pratiche di sicurezza

1. **Dichiarare solo il necessario**: Se hai bisogno solo di HTTP, non dichiarare Capability.Network solo perché puoi — ma nota che Capability.Network è l'unica capacità relativa alla rete; non ci sono opzioni più granulari
2. **Preferire NetworkExecutor**: `NetworkExecutor` è il punto di ingresso controllato per l'accesso alla rete e non richiede alcuna dichiarazione di capacità
3. **Fornire un Reason chiaro**: Le ragioni vaghe come "accesso alla rete" sono un segnale d'allarme nelle revisioni di sicurezza
4. **Rispettare i limiti non dichiarabili**: Nessuna dichiarazione di capacità può aggirare i divieti di P/Invoke, Unsafe o Reflection.Emit

## File

- `Plugin.cs` — Plugin demo che dichiara Capability.Network
- `README.md` — English
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 日本語
- `README.ko-KR.md` — 한국어
- `README.de-DE.md` — Deutsch
- `README.fr-FR.md` — Français
- `README.es-ES.md` — Español
- `README.it-IT.md` — Questo file (Italiano)
- `README.ru-RU.md` — Русский
- `README.pt-PT.md` — Português
- `README.pl-PL.md` — Polski
- `README.cs-CZ.md` — Čeština

## Esempi correlati

- **08-ForbiddenNetwork**: Anti-pattern che mostra operazioni di rete bloccate
- **14-CapabilityFileIO**: Capacità FileIO dichiarativa
- **15-CapabilityProcess**: Capacità Process dichiarativa
- **16-CapabilityAI**: Capacità di servizio IA dichiarativa
- **17-CapabilityStacked**: Impilamento di capacità multiple
- **18-CapabilityDenied**: Anti-pattern di capacità non dichiarabile
