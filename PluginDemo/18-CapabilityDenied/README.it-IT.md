# PluginDemo-18: Capacità negata — Anti-pattern di capacità non dichiarabile

## Panoramica

Questo plugin è un **anti-pattern** che dimostra che dichiarare una capacità NON aggira i divieti di capacità non dichiarabili. Anche con `[PluginCapability(Capability.Network)]`, P/Invoke, Unsafe, Reflection.Emit e l'accesso al registro rimangono **sempre** bloccati.

## Capacità dichiarabili vs. non dichiarabili

### ✅ Dichiarabili (esistono valori dell'enum Capability)

| Capacità | Cosa esenta |
|---------|-----------|
| `Capability.Network` | Namespace System.Net.* e divieti per tipo |
| `Capability.FileIO` | Namespace System.IO (oltre la whitelist) |
| `Capability.Process` | Tipi Process* sotto System.Diagnostics |
| `Capability.AI` | Abilita iniezione IAIService (nessuna esenzione TypeRef) |

### ❌ Non dichiarabili (NESSUN valore dell'enum Capability esiste)

| Categoria | Tipi bloccati | Perché non dichiarabile |
|----------|-------------|----------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory`, `NativeLibrary` | Impossibile auditare codice nativo arbitrario a runtime |
| Codice unsafe | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Aggira la sicurezza dei tipi CLR e il controllo dei limiti |
| Emissione IL | `System.Reflection.Emit.*` | Può generare IL arbitrario a runtime |
| Caricamento assembly | `System.Runtime.Loader`, `Assembly.Load*` | Può caricare DLL non scansionate, aggirando la scansione di sicurezza |
| Registro | `Microsoft.Win32.*` | Accesso di sistema a livello OS fuori dal sandbox del plugin |
| Compilazione dinamica | `Microsoft.CodeAnalysis.*` | Può compilare ed eseguire codice arbitrario |
| Riflessione pericolosa | `Type.GetType(string)`, `Activator.CreateInstance` | Può istanziare tipi proibiti tramite stringa |

## Perché queste capacità non possono essere dichiarate

Il motivo fondamentale: **non possono essere auditati in modo sicuro a runtime.**

1. **P/Invoke**: Una volta chiamato il codice nativo, il CLR non ha visibilità — nessuna garanzia di sicurezza
2. **Unsafe**: Aggira il sistema di sicurezza dei tipi da cui dipende il modello di sicurezza del plugin
3. **Reflection.Emit**: Può generare nuovo IL a runtime mai scansionato da PluginLoader
4. **AssemblyLoadContext**: Può caricare DLL mai scansionate per la sicurezza
5. **Registro**: Fornisce accesso alla configurazione a livello OS fuori dal sandbox del plugin

## Elaborazione «dichiarazione non valida» di PluginLoader

Quando PluginLoader incontra una dichiarazione di capacità:

1. Legge il valore enum int32 dal blob CustomAttribute
2. Verifica `Enum.IsDefined(typeof(Capability), value)`
3. Se il valore non è un membro Capability definito → **ignorato silenziosamente**
4. Se il valore è definito → le regole di esenzione vengono applicate
5. **I controlli non dichiarabili sono SEMPRE applicati** indipendentemente da qualsiasi capacità dichiarata

Questo impedisce ai plugin di dichiarare capacità «future» che non esistono ancora.

## Confronto con 13-CapabilityNetwork

| Aspetto | 13-CapabilityNetwork (positivo) | 18-CapabilityDenied (anti-pattern) |
|---------|-------------------------------|-----------------------------------|
| Dichiarazione | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| Usa HttpClient | ✅ Esentato | ✅ Esentato |
| Usa DllImport | N/A | ❌ SEMPRE bloccato |
| Usa Unsafe | N/A | ❌ SEMPRE bloccato |
| Risultato caricamento | ✅ CARICATO | ❌ RIFIUTATO |

## File

- `Plugin.cs` — Plugin demo anti-pattern di capacità non dichiarabile
- `README.md` — Questo file (Inglese)
- `README.zh-CN.md` — Cinese semplificato
- Traduzioni: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Esempi correlati

- **13-CapabilityNetwork**: Esempio positivo di Capability.Network
- **11-ForbiddenPInvoke**: Anti-pattern P/Invoke (nessuna capacità può aiutare)
- **10-ForbiddenReflection**: Anti-pattern Reflection (nessuna capacità può aiutare)
