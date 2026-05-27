# Demo dipendenza affidabile

Dimostra l'utilizzo di `Newtonsoft.Json` — una libreria che internamente fa un uso intensivo della reflection — come assembly affidabile. Lo scanner di sicurezza del PluginLoader salta completamente gli assembly affidabili, permettendo ai plugin di referenziarli senza attivare violazioni.

## Meccanismo whitelist TrustedAssemblies

Il `PluginLoader` mantiene una whitelist statica di librerie open-source che sono **affidabili per impostazione predefinita**:

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // Serializzazione
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← Questa demo usa questa libreria
    "MessagePack",
    "YamlDotNet",

    // Logging
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // Accesso dati / mapping
    "Dapper", "AutoMapper",

    // Validazione e distribuzione messaggi
    "FluentValidation", "MediatR",
};
```

### Criteri di ammissione

Una libreria può essere aggiunta a `TrustedAssemblies` se soddisfa **tutti e tre** i criteri:

| # | Criterio | Motivazione |
|---|----------|-------------|
| 1 | Progetto open-source ampiamente utilizzato (MIT / Apache 2.0 / BSD) | Codice pubblicamente verificabile |
| 2 | Codice sorgente pubblicamente accessibile | La supervisione della comunità garantisce l'assenza di comportamenti dannosi |
| 3 | Pacchetto NuGet mantenuto da fornitore/comunità affidabile | Integrità della catena di fornitura |

### Base di identificazione

Lo scanner identifica gli assembly affidabili tramite il loro `AssemblyDefinition.Name` nei metadati PE — **non tramite il nome del file DLL**. Questo impedisce agli attaccanti di rinominare una DLL malevola in `Newtonsoft.Json.dll` per aggirare i controlli.

## CollectTrustedTypeRefs — Esenzione transitiva

Quando il PluginLoader carica una directory di plugin, esegue una scansione in due fasi:

```
Fase 1: CollectTrustedTypeRefs(pluginDir)
├── Enumerare tutti i file *.dll nella directory del plugin
├── Per ogni DLL: leggere i metadati PE → verificare AssemblyDefinition.Name
├── Se nome ∈ TrustedAssemblies:
│   └── Raccogliere TUTTE le voci TypeReference → coppie (namespace, typeName)
└── Restituisce: HashSet<(string Namespace, string Name)>

Fase 2: ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── Livello 0:   Uscita rapida whitelist (se DLL principale è affidabile → passa)
├── Livello 0.5: Esenzione transitiva (saltare TypeRefs nell'insieme trustedTypeRefs)
├── Livello 1:   Scansione tabella TypeRef
├── Livello 2:   Scansione tabella ExportedType
├── Livello 3:   Scansione tabella MemberRef (metodi pericolosi)
├── Livello 4:   Marcatori codice non sicuro + P/Invoke
└── Livello 5:   Scansione heap stringhe #US
```

### Perché l'esenzione transitiva è importante

Newtonsoft.Json referenzia internamente tipi come `System.Reflection.MemberInfo`, `System.IO.TextReader`, ecc. Quando il tuo plugin referenzia Newtonsoft.Json, il compilatore può incorporare questi TypeRef transitivi nella DLL del **tuo** plugin. Senza esenzione transitiva, il tuo plugin verrebbe segnalato per aver referenziato `System.IO.TextReader` — anche se non lo usi mai direttamente.

`CollectTrustedTypeRefs` risolve questo problema pre-raccogliendo tutti i TypeRef dalle DLL affidabili e marcandoli come "noti sicuri" durante la scansione principale.

## Come aggiungere una nuova dipendenza affidabile

Per aggiungere una nuova libreria alla whitelist:

1. Verificare che soddisfi i tre criteri di ammissione sopra indicati
2. Aggiungere una riga all'HashSet `TrustedAssemblies` in `PluginLoader.cs`:
   ```csharp
   "YourLibraryName",  // Breve descrizione del perché è affidabile
   ```
3. Posizionare la DLL della libreria nella directory del plugin (accanto alla DLL principale del plugin)
4. Lo scanner raccoglierà automaticamente i suoi TypeRef e li esenterà

> **⚠️ Importante:** Aggiungere una libreria a `TrustedAssemblies` significa che lo scanner **non** verificherà il suo codice interno. Aggiungere solo librerie di cui ci si fida completamente.

## Questa demo

Questo plugin usa Newtonsoft.Json senza alcuna dichiarazione `PluginCapability`:

| Funzione | Comportamento interno di Newtonsoft.Json | Perché funziona |
|----------|------------------------------------------|-----------------|
| `JsonConvert.SerializeObject` | Usa la reflection per enumerare le proprietà | La DLL Newtonsoft.Json passa la whitelist del livello 0 |
| `JsonConvert.DeserializeObject<T>` | Chiama `Activator.CreateInstance`, imposta proprietà via reflection | TypeRef transitivi esentati al livello 0.5 |
| Manipolazione `JObject` / `JArray` | Usa `System.Linq.Expressions`, dispatch dinamico | Tutti i ref interni raccolti da `CollectTrustedTypeRefs` |

### Differenza chiave con PluginCapability

| Meccanismo | Ambito | Caso d'uso |
|------------|--------|------------|
| `TrustedAssemblies` | Esenta un'intera **libreria** (e i suoi ref transitivi) dalla scansione | Dipendenze open-source note |
| `PluginCapability` | Esenta il **codice del tuo plugin** dai divieti di namespace specifici | Il plugin necessita accesso diretto a System.Net/IO/Process |

Un plugin che usa solo dipendenze affidabili **non ha bisogno** di alcuna dichiarazione `PluginCapability`. Lo scanner gestisce tutto automaticamente.

## Nota sulla sicurezza

Gli assembly affidabili sono esenti dalla scansione di sicurezza perché sono progetti open-source verificabili. Tuttavia, **il codice del tuo plugin** viene comunque completamente scansionato. Se il tuo plugin referenzia direttamente `System.IO.File` o `System.Net.Http.HttpClient`, sarà comunque bloccato — a meno che non dichiari il `PluginCapability` corrispondente. Vedi la [documentazione sulla sicurezza](../../docs/it-IT/security.md).
