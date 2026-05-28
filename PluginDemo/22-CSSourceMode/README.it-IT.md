# Demo della Modalità di Caricamento tramite Compilazione del Codice Sorgente CS

Un plugin caricato da file sorgente `.cs` grezzi invece di una DLL precompilata, che dimostra la modalità di compilazione del codice sorgente CS di PluginLoader (introdotta da task-389).

## Come Funziona la Modalità Codice Sorgente CS

Quando PluginLoader scansiona una directory di plugin e **non trova DLL**, entra automaticamente nella modalità codice sorgente CS:

```
1. PluginLoader scansiona la directory → nessuna DLL
2. Entra nella modalità codice sorgente CS
3. cs.txt trovato → legge riga per riga, carica solo i file .cs elencati
   (Nessun cs.txt → carica tutti i file *.cs nella directory)
4. Scansione delle DLL sibling → le DLL fidate vengono aggiunte direttamente come riferimenti;
   le DLL non fidate devono superare ScanForbiddenReferences
5. CompilationCore (modalità ristretta) compila i file .cs in DLL in memoria
6. I byte della DLL in memoria vengono scritti in un file temporaneo per la scansione ScanForbiddenReferences
7. Scansione superata → riflessione trova l'implementazione IPlugin → istanziazione
8. Log mostra: "Plugin loaded [CS-Source]: {Id} v{Version} from {DirName}"
```

## cs.txt — Whitelist di Caricamento Selettivo

Il file `cs.txt` specifica quali file `.cs` compilare, un nome file per riga:

```
Plugin.cs
```

- **File elencati**: Compilati e caricati (es: `Plugin.cs`)
- **File non elencati**: Ignorati dal compilatore (es: `Helpers.cs`)
- **Righe che iniziano con `#`**: Trattate come commenti
- **Righe vuote**: Ignorate
- **Nessun cs.txt**: Tutti i file `*.cs` nella directory vengono caricati

## Modalità Codice Sorgente CS vs Modalità DLL

| Aspetto | Modalità DLL | Modalità Codice Sorgente CS |
|---------|-------------|----------------------------|
| Formato del plugin | DLL precompilata `.dll` | File sorgente `.cs` grezzi |
| Trigger di caricamento | DLL trovata nella directory | Nessuna DLL, file `.cs` presenti |
| Compilazione | Al momento della build | Al momento del caricamento da PluginLoader |
| Prestazioni | Nessun overhead di compilazione | Overhead di compilazione Roslyn all'avvio |
| Scansione di sicurezza | Scansione diretta dei metadati PE | Compilazione → DLL temporanea → Scansione metadati PE |
| Prefisso del log | `Plugin loaded:` | `Plugin loaded [CS-Source]:` |
| Ideale per | Distribuzione in produzione | Iterazione di sviluppo |

## Gestione degli Errori

| Scenario | Comportamento |
|----------|--------------|
| Nessuna DLL, nessun file .cs | Avviso: "No DLL and no CS source files found" |
| Errori di compilazione | Errore: Messaggi diagnostici dettagliati registrati |
| Scansione di sicurezza fallita | Errore: Tutte le violazioni elencate, plugin rifiutato |
| Voce cs.txt non trovata | Avviso: "cs.txt entry not found or not a .cs file" |
| Scansione DLL sibling fallita | Avviso: DLL non aggiunta come riferimento, compilazione continua |

## Nota di Sicurezza

I plugin in modalità codice sorgente CS subiscono la **stessa scansione di sicurezza** dei plugin in modalità DLL. L'assembly compilato viene scritto in un file DLL temporaneo e scansionato con `ScanForbiddenReferences` — la stessa scansione che ricevono le DLL precompilate. Tutte le regole su spazi dei nomi/tipi/membri/stringhe proibiti si applicano in modo identico.

I plugin vengono comunque caricati in un contesto isolato e scansionati per riferimenti a spazi dei nomi proibiti (es: `System.IO`, `System.Net.Http`). Vedere la [Documentazione di Sicurezza](../../docs/it-IT/security.md) per i dettagli.
