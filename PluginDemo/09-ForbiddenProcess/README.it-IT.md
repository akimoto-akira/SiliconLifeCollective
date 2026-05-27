# PluginDemo-09: Anti-pattern delle operazioni di processo vietate

## Panoramica

Questo plugin dimostra le operazioni di esecuzione di processi **vietate** nel sistema di plugin SiliconLife. Serve come riferimento anti-pattern, mostrando cosa NON fare e fornendo alternative corrette per ogni violazione.

## Perché i tipi Process sono vietati?

`System.Diagnostics.Process` e `ProcessStartInfo` sono bloccati nei plugin perché l'esecuzione diretta di processi presenta gravi rischi di sicurezza:

1. **Esecuzione arbitraria di comandi**: I plugin potrebbero eseguire qualsiasi comando senza audit o verifica dei permessi
2. **Lancio di malware**: Plugin malevoli potrebbero eseguire applicazioni o script indesiderati
3. **Accesso alle risorse di sistema**: I processi potrebbero accedere a risorse sensibili al di fuori della sandbox del plugin
4. **Nessuna validazione dei comandi**: Process.Start diretto non ha protezione integrata contro l'iniezione di comandi
5. **Nessuna traccia di audit**: Le operazioni dirette sui processi aggirano il sistema di audit di sicurezza
6. **Escalation dei privilegi**: Potrebbe generare processi con privilegi superiori a quelli del plugin

## Quali tipi sono vietati?

Solo i tipi relativi a Process sono vietati, **NON l'intero namespace System.Diagnostics**:

| Tipo vietato | Metodo bloccato | Livello di rischio |
|-------------|----------------|-------------------|
| `Process` | `Start()`, `Kill()`, `WaitForExit()` | 🔴 Critico |
| `ProcessStartInfo` | Costruttore, tutte le proprietà | 🔴 Critico |
| `Process` | `StandardInput`, `StandardOutput`, `StandardError` | 🔴 Critico |
| `Process` | `GetProcesses()`, `GetProcessesByName()` | 🟡 Alto |

## Quali tipi sono consentiti?

Altri tipi `System.Diagnostics` che non coinvolgono l'esecuzione di processi rimangono disponibili:

| Tipo consentito | Utilizzo | Perché è sicuro |
|----------------|----------|----------------|
| `Stopwatch` | Misurazione del tempo | Nessuna esecuzione di processi |
| `Debug` | Output di debug | Nessun rischio di sicurezza |
| `Trace` | Tracciamento/logging | Nessun rischio di sicurezza |
| `PerformanceCounter` | Monitoraggio prestazioni | Solo lettura, auditato |

## Come eseguire comandi in sicurezza?

### Usare CommandLineExecutor (l'unico modo sicuro)

`CommandLineExecutor` è il **punto di ingresso controllato** per l'esecuzione di comandi nei plugin:

```csharp
// ✅ CORRETTO: Eseguire un comando
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"Errore: {result.Error}");
}
```

**Cosa fornisce CommandLineExecutor:**
1. **Protezione contro l'iniezione di comandi**: Blocca separatori pericolosi (`||`, `&&`, `|`, `&`, `;`)
2. **Applicazione del timeout**: Timeout predefinito di 30 secondi (configurabile)
3. **Log di audit**: Tutte le esecuzioni di comandi vengono registrate per revisione di sicurezza
4. **Cattura dell'output**: Cattura automatica di stdout e stderr
5. **Supporto multipiattaforma**: Usa `cmd.exe` su Windows, `/bin/bash` su Unix
6. **Gestione degli errori**: Restituisce risultato strutturato con stato successo/fallimento

## Violazioni dimostrate

Questo plugin mostra 5 violazioni comuni di esecuzione di processi:

### Violazione 1: Process.Start

```csharp
// ❌ VIETATO
Process.Start("notepad.exe");

// ✅ CORRETTO
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**TypeRef bloccato**: `System.Diagnostics.Process::Start(System.String)`

### Violazione 2: ProcessStartInfo

```csharp
// ❌ VIETATO
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ CORRETTO
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**TypeRef bloccato**: `System.Diagnostics.ProcessStartInfo::.ctor()`

### Violazione 3: Process con argomenti

```csharp
// ❌ VIETATO
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ CORRETTO
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**TypeRef bloccato**: `System.Diagnostics.Process::Start(ProcessStartInfo)`

### Violazione 4: Reindirizzamento output del processo

```csharp
// ❌ VIETATO
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ CORRETTO
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**TypeRef bloccato**: `System.Diagnostics.Process::StandardOutput`

### Violazione 5: Process.Kill

```csharp
// ❌ VIETATO
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ CORRETTO
// Per motivi di sicurezza, CommandLineExecutor non supporta la terminazione dei processi.
// Contattare l'amministratore di sistema se necessario.
```

**TypeRef bloccato**: `System.Diagnostics.Process::Kill()`

## Perché solo Process e non tutto System.Diagnostics?

Il sistema di plugin adotta un approccio **chirurgico** alla sicurezza:

- **Bloccare solo i tipi pericolosi**: Process/ProcessStartInfo permettono l'esecuzione di codice arbitrario
- **Consentire i tipi sicuri**: Stopwatch, Debug, Trace non hanno implicazioni di sicurezza
- **Minimizzare l'impatto**: Gli sviluppatori possono continuare a usare strumenti diagnostici senza rischi
- **Confine chiaro**: Solo i tipi che possono creare/terminare processi sono vietati

## Meccanismo di sicurezza PluginLoader

Quando PluginLoader analizza questo plugin:

1. **Scansione TypeRef**: Rileva riferimenti ai tipi vietati `Process`/`ProcessStartInfo`
2. **Scansione MemberRef**: Rileva chiamate a metodi bloccati (es: `Process.Start`)
3. **Scansione IL String**: Rileva tentativi di riflessione basati su stringhe
4. **Rifiuto**: Il plugin viene rifiutato durante il caricamento con messaggio di errore dettagliato

## Best Practice

1. **Usare sempre CommandLineExecutor**: Mai usare `Process.Start` direttamente
2. **Impostare timeout ragionevoli**: Evitare che i comandi si blocchino indefinitamente
3. **Verificare i risultati**: Verificare sempre `result.Success` prima di usare l'output
4. **Sanificare l'input**: Mai passare input utente direttamente ai comandi
5. **Dichiarare Capability se necessario**: Se serve esecuzione di processi senza restrizioni, dichiarare `Capability.Process` (vedi 15-CapabilityProcess)

## File

- `Plugin.cs` - Plugin dimostrativo anti-pattern
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Questo file (Italiano)
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Esempi correlati

- **08-ForbiddenNetwork**: Operazioni di rete vietate
- **15-CapabilityProcess**: Permesso dichiarativo Process
- **10-ForbiddenReflection**: Operazioni di riflessione vietate
- **12-ForbiddenStringBypass**: Tentativi di aggiramento tramite riflessione basata su stringhe
