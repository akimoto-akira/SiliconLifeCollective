# Guida alla Risoluzione dei Problemi

> **Versione: v0.2.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md) | [Русский](../ru-RU/troubleshooting.md) | **Italiano**

## Problemi Comuni

### Compilazione e Build

#### Problema: Build fallita, dipendenze mancanti

**Sintomi**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Soluzione**:
```bash
dotnet restore
dotnet build
```

#### Problema: .NET SDK non trovato

**Sintomi**:
```
The .NET SDK could not be found
```

**Soluzione**:
1. Installa .NET 9 SDK: https://dotnet.microsoft.com/download/dotnet/9.0
2. Verifica l'installazione:
```bash
dotnet --version
```

---

### Problemi di Connessione AI

#### Problema: Connessione Ollama rifiutata

**Sintomi**:
```
Failed to connect to Ollama at http://localhost:11434
```

**Soluzione**:
```bash
# Verifica che Ollama sia in esecuzione
ollama list

# Avvia Ollama
ollama serve

# Testa la connessione
curl http://localhost:11434/api/tags
```

#### Problema: Modello non trovato

**Sintomi**:
```
model "qwen2.5:7b" not found
```

**Soluzione**:
```bash
# Scarica il modello richiesto
ollama pull qwen2.5:7b

# Elenca i modelli disponibili
ollama list
```

#### Problema: Errore 404 Bailian

**Sintomi**:
```
HTTP 404: Model not found
```

**Soluzione**:
1. Verifica che la chiave API sia corretta
2. Controlla che il nome del modello corrisponda alla directory Bailian
3. Verifica che l'endpoint regionale sia corretto
4. Controlla che l'account abbia accesso al modello

#### Problema: Connessione Volcengine Ark fallita

**Sintomi**:
```
HTTP 401: Unauthorized
oppure
HTTP 404: Endpoint not found
```

**Soluzione**:
1. Verifica che la chiave API sia corretta
2. Controlla che il formato dell'URL dell'endpoint sia corretto (predefinito: `https://ark.cn-beijing.volces.com/api/v3/chat/completions`)
3. Conferma che il parametro Model utilizzi l'ID del punto di accesso all'inferenza (es. `ep-20241212123456-abcde`), non il nome del modello
4. Verifica che l'account abbia accesso al punto di accesso

---

### Problemi di Runtime

#### Problema: Porta già in uso

**Sintomi**:
```
HttpListenerException: Address already in use
```

**Soluzione**:

**Windows**:
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac**:
```bash
lsof -ti:8080 | xargs kill -9
```

**Oppure cambia la porta nella configurazione**.

#### Problema: L'essere non si avvia

**Sintomi**:
- Lo stato dell'essere mostra "Error"
- I log mostrano un fallimento dell'inizializzazione

**Soluzione**:
1. Controlla che il File dell'Anima esista e sia valido
2. Verifica che il client AI sia configurato
3. Controlla i log per l'errore specifico:
```bash
tail -f logs/*.log
```

#### Problema: Memoria insufficiente

**Sintomi**:
```
OutOfMemoryException
```

**Soluzione**:
1. **SiliconLife.Default**: Aumenta la dimensione dell'heap:
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast**: La versione Fast ha un utilizzo di memoria intrinsecamente più alto (~500MB); se la memoria continua a essere insufficiente, si consiglia di:
   - Ridurre il numero di Esseri di Silicio concorrenti
   - Pulire i vecchi dati per liberare memoria

3. Pulisci i vecchi dati:
```bash
# Archivia i vecchi log
mv logs/ logs-archive/
mkdir logs

# Pulisci le vecchie memorie
# Tramite Web UI: Gestione memoria > Pulizia
```

> **Suggerimento**: SiliconLife.Default ha un utilizzo di memoria inferiore (~200MB), adatto per ambienti con memoria limitata; SiliconLife.Fast ha un utilizzo di memoria superiore ma prestazioni migliori, adatto per ambienti di produzione.

---

### Problemi di Permessi

#### Problema: Permesso negato

**Sintomi**:
```
Permission denied: FileAccess C:\Windows
```

**Soluzione**:
1. Controlla i permessi correnti:
```bash
curl http://localhost:8080/api/permissions/list
```

2. Concedi il permesso:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

3. Oppure utilizza la Web UI: Gestione permessi

#### Problema: I permessi non scadono

**Sintomi**:
- I permessi rimangono validi dopo il tempo di scadenza

**Soluzione**:
1. Controlla la sincronizzazione dell'orologio di sistema
2. Verifica che il campo `expiresAt` sia impostato correttamente
3. Cancella la cache dei permessi

---

### Problemi della Web UI

#### Problema: Impossibile accedere alla Web UI

**Sintomi**:
- Il browser mostra "Connection refused"

**Soluzione**:
1. Verifica che il server sia in esecuzione
2. Controlla l'URL corretto: `http://localhost:8080`
3. Controlla le impostazioni del firewall
4. Controlla i log per errori di avvio

#### Problema: SSE non funziona

**Sintomi**:
- Gli aggiornamenti in tempo reale non appaiono
- La chat non trasmette in streaming

**Soluzione**:
1. Verifica che il browser supporti SSE
2. Disabilita il buffering del proxy per SSE
3. Controlla la stabilità della rete
4. Prova un browser diverso

#### Problema: L'UI appare danneggiata

**Sintomi**:
- Stili non corretti
- Layout interrotto

**Soluzione**:
1. Cancella la cache del browser
2. Prova una skin diversa: Impostazioni > Skin
3. Controlla gli errori nella console del browser
4. Disabilita le estensioni del browser

---

### Problemi di Archiviazione

#### Problema: Impossibile leggere/scrivere dati

**Sintomi**:
```
IOException: Access denied
```

**Soluzione**:
1. Controlla i permessi dei file
2. Verifica che il percorso di archiviazione esista
3. Controlla lo spazio su disco
4. Esegui con i permessi appropriati

#### Problema: Dati danneggiati

**Sintomi**:
- Errori di analisi JSON
- Dati mancanti

**Soluzione**:
1. Ripristina da backup
2. Controlla l'integrità dell'archiviazione:
```bash
# Tramite Web UI: Sistema > Controllo archiviazione
```

3. Ripara manualmente i file danneggiati

#### Problema: File di archiviazione SpeedyPack danneggiati (versione Fast)

**Sintomi**:
- I file `.spk` non possono essere caricati
- Inizializzazione di SpeedyStorage fallita

**Soluzione**:
1. Utilizza lo strumento `SiliconLife.Speedy.Manager` per controllare e riparare i file `.spk`
2. Verifica che il file di indice `.spk.idx` corrisponda al file `.spk`
3. Se il file di indice è danneggiato, elimina il file `.spk.idx`, il sistema ricostruirà automaticamente l'indice
4. Ripristina i file `.spk` dal backup

#### Problema: Compressione automatica SpeedyPack fallita (versione Fast)

**Sintomi**:
- I file `.spk` continuano a crescere
- Spazio su disco insufficiente

**Soluzione**:
1. Verifica che `SpeedyPackAutoCompactor` funzioni correttamente
2. Attiva manualmente l'operazione di compressione
3. Controlla la configurazione della soglia di compressione
4. Utilizza lo strumento `SiliconLife.Speedy.Manager` per la compressione manuale

---

### Problemi di Esecuzione degli Strumenti

#### Problema: Strumento non trovato

**Sintomi**:
```
Tool "xyz" not found
```

**Soluzione**:
1. Verifica che il nome dello strumento sia corretto
2. Controlla che lo strumento sia nella directory Tools
3. Ricompila il progetto
4. Verifica che lo strumento sia implementato correttamente

#### Problema: Lo strumento restituisce un errore

**Sintomi**:
```
Tool execution failed: ...
```

**Soluzione**:
1. Controlla i log dello strumento
2. Verifica i parametri di input
3. Testa lo strumento indipendentemente
4. Controlla i permessi

---

### Problemi dei Plugin

#### Problema: Caricamento del plugin fallito

**Sintomi**:
```
Plugin load failed: Security check failed
```

**Soluzione**:
1. Controlla che il plugin non faccia riferimento a namespace proibiti (`System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`)
2. Verifica che il plugin faccia riferimento solo ad assembly nella whitelist degli assembly attendibili
3. Verifica che il plugin implementi correttamente l'interfaccia `IPlugin`
4. Consulta i log per i dettagli sul fallimento del controllo di sicurezza

#### Problema: Strumenti del plugin non registrati

**Sintomi**:
- Il plugin si carica con successo ma gli strumenti non compaiono nell'elenco

**Soluzione**:
1. Conferma che la classe dello strumento nel plugin implementi correttamente l'interfaccia `ITool`
2. Controlla che la classe dello strumento sia pubblica
3. Verifica che `ToolManager.ScanAllPluginAssemblies()` venga chiamato
4. Ricompila il plugin e riavvia l'applicazione

---

### Problemi delle Note di Lavoro

#### Problema: Impossibile creare note di lavoro

**Sintomi**:
```
Failed to create work note
```

**Soluzione**:
1. Controlla che l'essere esista e sia in esecuzione
2. Verifica che il percorso di archiviazione abbia i permessi di scrittura
3. Controlla che il contenuto non sia vuoto (il contenuto è obbligatorio)
4. Consulta i log per informazioni dettagliate sull'errore

#### Problema: La ricerca nelle note non restituisce risultati

**Sintomi**:
- La ricerca per parole chiave restituisce risultati vuoti
- Ma si è certi che esistano note pertinenti

**Soluzione**:
1. Controlla che l'ortografia delle parole chiave sia corretta
2. Prova con parole chiave più generiche
3. Verifica che le note contengano la parola chiave (distingue maiuscole/minuscole)
4. Aumenta il valore del parametro `max_results`

#### Problema: Generazione dell'indice delle note lenta

**Sintomi**:
- Tempo di risposta lungo durante la generazione dell'indice
- L'essere ha un gran numero di note (>1000 pagine)

**Soluzione**:
1. Questo è normale, richiede l'attraversamento di tutte le note
2. Considera l'archiviazione periodica delle vecchie note
3. Usa la funzione di ricerca invece della navigazione per indice
4. Ottimizzazione pianificata: aggiunta di un meccanismo di cache dell'indice

---

### Problemi della Rete di Conoscenza

#### Problema: La query di conoscenza restituisce risultati vuoti

**Sintomi**:
```
No knowledge triples found
```

**Soluzione**:
1. Verifica l'ortografia del soggetto e del predicato
2. Controlla che la conoscenza sia stata aggiunta alla rete
3. Usa la funzione di ricerca per la corrispondenza fuzzy:
```json
{
  "action": "search",
  "query": "parola chiave"
}
```

#### Problema: Ricerca del percorso di conoscenza fallita

**Sintomi**:
```
No path found between concepts
```

**Soluzione**:
1. Verifica che entrambi i concetti esistano nella rete di conoscenza
2. Controlla se esiste un percorso di associazione (potrebbe non esserci una relazione diretta o indiretta)
3. Prova ad aggiungere più conoscenza per stabilire connessioni
4. Riduci il limite di lunghezza del percorso (se impostato)

#### Problema: Validazione della conoscenza fallita

**Sintomi**:
```
Knowledge validation failed
```

**Soluzione**:
1. Controlla che il formato della tripla sia corretto (soggetto, predicato, oggetto sono obbligatori)
2. Verifica che la confidenza sia nell'intervallo 0.0-1.0
3. Controlla se ci sono triple duplicate
4. Consulta i dettagli dell'errore di validazione per comprendere il problema specifico

#### Problema: Statistiche della rete di conoscenza imprecise

**Sintomi**:
- I numeri statistici non corrispondono alle aspettative
- Le statistiche non si aggiornano dopo l'aggiunta di conoscenza

**Soluzione**:
1. Le statistiche potrebbero richiedere alcuni secondi per aggiornarsi (cache)
2. Controlla se le operazioni di eliminazione non sono state eseguite con successo
3. Riavvia l'applicazione per forzare l'aggiornamento delle statistiche
4. Interroga nuovamente le statistiche tramite API

---

### Problemi di Gestione dei Progetti

#### Problema: Impossibile creare un progetto

**Sintomi**:
```
Failed to create project
```

**Soluzione**:
1. Controlla che il nome del progetto non sia vuoto (obbligatorio)
2. Verifica che il nome del progetto non sia duplicato
3. Controlla che il percorso di archiviazione abbia i permessi di scrittura
4. Consulta i log per informazioni dettagliate sull'errore

#### Problema: Dati del progetto persi

**Sintomi**:
- Le informazioni del progetto non possono essere caricate
- I file del progetto sono danneggiati

**Soluzione**:
1. Controlla che la directory di archiviazione del progetto esista
2. Ripristina i dati del progetto dal backup
3. Verifica che il formato dei file JSON sia corretto
4. Ripara manualmente i file del progetto danneggiati

#### Problema: Assegnazione del ruolo di progetto fallita

**Sintomi**:
```
Failed to assign role
```

**Soluzione**:
1. Conferma che l'Essere di Silicio sia stato aggiunto al progetto
2. Controlla che il nome del ruolo sia valido
3. Verifica che l'operatore sia il Curatore di Silicio
4. Consulta i log per informazioni dettagliate sull'errore

#### Problema: Il workflow non si avvia

**Sintomi**:
- Creazione dell'istanza del workflow fallita
- Le transizioni di stato non vengono eseguite

**Soluzione**:
1. Controlla che il template del workflow sia definito
2. Verifica che lo stato iniziale sia impostato correttamente
3. Conferma che il progetto sia collegato a un template di workflow
4. Controlla i log del workflow per errori di transizione

---

### Problemi dei Permessi degli Strumenti

#### Problema: Operazione dello strumento negata

**Sintomi**:
```
Tool operation denied: network:post
```

**Soluzione**:
1. Controlla la configurazione dei permessi degli strumenti dell'Essere di Silicio:
```bash
curl http://localhost:8080/api/beings/tool-permissions?beingId=<id>
```

2. Aggiorna i permessi degli strumenti:
```bash
curl -X PUT http://localhost:8080/api/beings/tool-permissions \
  -H "Content-Type: application/json" \
  -d '{
    "beingId": "being-uuid",
    "permissions": {
      "network:post": "allowed"
    }
  }'
```

3. Oppure utilizza la Web UI: Esseri → Permessi strumenti

#### Problema: I permessi degli strumenti di progetto non funzionano

**Sintomi**:
- I permessi degli strumenti a livello di progetto non funzionano come previsto

**Soluzione**:
1. Conferma che i permessi a livello di progetto siano configurati correttamente
2. Controlla se ci sono conflitti tra i permessi a livello di Essere di Silicio e a livello di progetto
3. I permessi a livello di progetto sono indipendenti dal livello dell'Essere di Silicio; si applica l'intersezione dei due
4. Consulta i log di audit per confermare i risultati del controllo dei permessi

---

## Debug

### Abilitare i Log Dettagliati

Modifica la configurazione:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Controllare i Log

I log sono archiviati in:
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

Visualizzazione in tempo reale:
```bash
tail -f logs/*.log
```

### Utilizzare il Debugger

**SiliconLife.Default (implementazione predefinita)**:
```bash
# Esegui con il debugger
dotnet run --project src/SiliconLife.Default --configuration Debug

# Collega il debugger
# Tramite IDE: Connetti al processo > SiliconLife.Default
```

**SiliconLife.Fast (versione ad alte prestazioni)**:
```bash
# Esegui con il debugger
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Collega il debugger
# Tramite IDE: Connetti al processo > SiliconLife.Fast
```

> **Suggerimento**: Durante la fase di sviluppo e debug, si consiglia di utilizzare SiliconLife.Default; dopo la validazione dell'architettura, utilizzare SiliconLife.Fast per il deployment di produzione.

---

## Problemi di Prestazioni

### Tempi di Risposta Lenti

**Ottimizzazione**:
1. Riduci la complessità del modello AI
2. Abilita la cache
3. Pulisci i vecchi dati
4. Aumenta le risorse di sistema

### Utilizzo CPU Elevato

**Verifica**:
- Troppi esseri in esecuzione
- Cicli infiniti negli strumenti
- Esecuzione frequente dei timer

**Soluzione**:
- Riduci gli esseri concorrenti
- Ottimizza il codice degli strumenti
- Regola gli intervalli dei timer

### Utilizzo Memoria Elevato

**Monitoraggio**:
```bash
# Tramite Web UI: Dashboard > Memoria
```

**Ottimizzazione**:
- Pulisci le vecchie memorie
- Riduci la dimensione del contesto
- Implementa la paginazione

---

## Ottenere Aiuto

### Consultare la Documentazione

- [Guida Rapida](getting-started.md)
- [Guida allo Sviluppo](development-guide.md)
- [Riferimento API](api-reference.md)
- [Guida all'Architettura](architecture.md)

### Controllare i Log

Controlla sempre prima i log per i dettagli sugli errori.

### Supporto della Community

- GitHub Issues: Segnala bug
- Discussions: Fai domande
- Documentazione: Cerca soluzioni

---

## Procedure di Emergenza

### Crash di Sistema

1. Controlla i log per la causa
2. Riavvia l'applicazione:

**SiliconLife.Default (implementazione predefinita)**:
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (versione di produzione raccomandata)**:
```bash
dotnet run --project src/SiliconLife.Fast
```

3. Se necessario, ripristina dal backup

### Perdita di Dati

1. Ferma immediatamente l'applicazione
2. Controlla i file di backup
3. Ripristina i dati
4. Verifica l'integrità

### Vulnerabilità di Sicurezza

1. Ferma tutti gli esseri
2. Revoca tutti i permessi
3. Controlla i log di audit
4. Verifica i controlli di accesso
5. Riavvia con permessi limitati

---

## Prevenzione

### Best Practices

1. **Backup regolari**
   - Esegui il backup della directory dei dati
   - Esegui il backup della configurazione
   - Testa il processo di ripristino

2. **Monitoraggio delle risorse**
   - Sorveglia l'utilizzo CPU/memoria
   - Monitora lo spazio su disco
   - Controlla le connessioni di rete

3. **Mantieni aggiornato**
   - Aggiorna .NET SDK
   - Aggiorna le dipendenze
   - Applica le patch di sicurezza

4. **Testa le modifiche**
   - Testa prima in sviluppo
   - Usa il controllo di versione
   - Documenta le modifiche

---

## Prossimi Passi

- 📚 Leggi la [Guida all'Architettura](architecture.md)
- 🛠️ Consulta la [Guida allo Sviluppo](development-guide.md)
- 🚀 Consulta la [Guida Rapida](getting-started.md)
- 🔒 Consulta la [Documentazione sulla Sicurezza](security.md)
