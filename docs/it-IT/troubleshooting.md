# Guida alla risoluzione dei problemi

> **Versione : v0.2.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [Français](../fr-FR/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md) | **Italiano**

## Problemi comuni

### Compilazione e build

#### Problema : Build fallita, dipendenze mancanti

**Sintomi** :
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Soluzione** :
```bash
dotnet restore
dotnet build
```

#### Problema : SDK .NET non trovato

**Sintomi** :
```
The .NET SDK could not be found
```

**Soluzione** :
1. Installare il SDK .NET 9 : https://dotnet.microsoft.com/download/dotnet/9.0
2. Verificare l'installazione :
```bash
dotnet --version
```

---

### Problemi di connessione IA

#### Problema : Connessione Ollama rifiutata

**Sintomi** :
```
Failed to connect to Ollama at http://localhost:11434
```

**Soluzione** :
```bash
# Verificare se Ollama funziona
ollama list

# Avviare Ollama
ollama serve

# Testare la connessione
curl http://localhost:11434/api/tags
```

#### Problema : Modello non trovato

**Sintomi** :
```
model "qwen2.5:7b" not found
```

**Soluzione** :
```bash
# Scaricare il modello necessario
ollama pull qwen2.5:7b

# Elencare i modelli disponibili
ollama list
```

#### Problema : Errore DashScope 404

**Sintomi** :
```
HTTP 404: Model not found
```

**Soluzione** :
1. Verificare la chiave API
2. Verificare il nome del modello con il catalogo DashScope
3. Verificare l'endpoint regionale
4. Verificare l'accesso dell'account al modello

#### Problema: Connessione Volcengine Ark fallita

**Sintomi**:
```
HTTP 401: Unauthorized
oppure
HTTP 404: Endpoint not found
```

**Soluzione** :
1. Verificare la chiave API
2. Verificare il formato dell'URL dell'endpoint (predefinito: `https://ark.cn-beijing.volces.com/api/v3/chat/completions`)
3. Verificare che il parametro Model utilizzi un ID endpoint di inferenza (es. `ep-20241212123456-abcde`), non un nome modello
4. Verificare l'accesso dell'account all'endpoint

---

### Problemi di esecuzione

#### Problema : Porta già in uso

**Sintomi** :
```
HttpListenerException: Address already in use
```

**Soluzione** :

**Windows** :
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac** :
```bash
lsof -ti:8080 | xargs kill -9
```

**Oppure cambiare la porta nella configurazione**.

#### Problema : Il Being non si avvia

**Sintomi** :
- Lo stato del Being mostra « Error »
- I log mostrano errori di inizializzazione

**Soluzione** :
1. Verificare che il file anima esista e sia valido
2. Verificare che il client IA sia configurato
3. Esaminare i log per errori specifici :
```bash
tail -f logs/*.log
```

#### Problema : Memoria insufficiente

**Sintomi** :
```
OutOfMemoryException
```

**Soluzione** :
1. **SiliconLife.Default** : Aumentare la dimensione dell'heap :
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast** : La versione Fast ha un consumo di memoria elevato (~500 MB). Se la memoria è costantemente limitata, si consiglia di :
   - Ridurre il numero di Silicon Beings funzionanti simultaneamente
   - Pulire i vecchi dati per liberare memoria

3. Pulire i vecchi dati :
```bash
# Archiviare i vecchi log
mv logs/ logs-archive/
mkdir logs

# Pulire il vecchio storage
# Tramite interfaccia Web : Gestione storage > Pulizia
```

> **Suggerimento** : SiliconLife.Default ha un basso consumo di memoria (~200 MB), adatto per ambienti con memoria limitata ; SiliconLife.Fast ha un consumo di memoria più elevato ma prestazioni migliori, adatto per ambienti di produzione.

---

### Problemi di permessi

#### Problema : Permesso negato

**Sintomi** :
```
Permission denied: disk:write
```

**Soluzione** :
1. Verificare i permessi attuali :
```bash
curl http://localhost:8080/api/permissions
```

2. Concedere il permesso :
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "prefix": "disk:write",
    "result": "Allowed",
    "duration": 3600
  }'
```

3. Oppure usare l'interfaccia Web : Gestione permessi

#### Problema : Il permesso non scade

**Sintomi** :
- Il permesso è ancora attivo dopo il termine di scadenza

**Soluzione** :
1. Verificare la sincronizzazione dell'orologio di sistema
2. Verificare che le voci nella cache frequenza non siano scadute
3. Svuotare la cache dei permessi

---

### Problemi dell'interfaccia Web

#### Problema : Interfaccia Web inaccessibile

**Sintomi** :
- Il browser mostra « Connection refused »

**Soluzione** :
1. Verificare che il server funzioni
2. Verificare l'URL corretto : `http://localhost:8080`
3. Verificare le impostazioni del firewall
4. Esaminare i log per errori di avvio

#### Problema : SSE non funziona

**Sintomi** :
- Gli aggiornamenti in tempo reale non appaiono
- La chat non viene streammata

**Soluzione** :
1. Verificare il supporto SSE del browser
2. Disabilitare il buffering del proxy per SSE
3. Verificare la stabilità della rete
4. Provare un altro browser

#### Problema : L'interfaccia sembra rotta

**Sintomi** :
- Stili incorretti
- Layout rotto

**Soluzione** :
1. Svuotare la cache del browser
2. Provare un altro skin : Impostazioni > Skin
3. Verificare la console del browser per errori
4. Disabilitare le estensioni del browser

---

### Problemi di storage

#### Problema : Dati non leggibili/scrivibili

**Sintomi** :
```
IOException: Access denied
```

**Soluzione** :
1. Verificare i permessi dei file
2. Verificare che il percorso di storage esista
3. Verificare lo spazio su disco
4. Eseguire con i permessi appropriati

#### Problema : Corruzione dati

**Sintomi** :
- Errori di parsing JSON
- Perdita di dati

**Soluzione** :
1. Ripristinare da un backup
2. Verificare l'integrità dello storage :
```bash
# Tramite interfaccia Web : Sistema > Verifica storage
```

3. Riparare manualmente i file corrotti

#### Problema : File di storage SpeedyPack corrotto (versione Fast)

**Sintomi** :
- Il file `.spk` non può essere caricato
- L'inizializzazione di SpeedyStorage fallisce

**Soluzione** :
1. Usare lo strumento `SiliconLife.Speedy.Manager` per verificare e riparare i file `.spk`
2. Verificare che il file di indice `.spk.idx` corrisponda al file `.spk`
3. Se il file di indice è corrotto, eliminare il file `.spk.idx` — il sistema ricreerà l'indice automaticamente
4. Ripristinare il file `.spk` da un backup

#### Problema : Auto-compattazione SpeedyPack fallita (versione Fast)

**Sintomi** :
- Il file `.spk` cresce continuamente
- Lo spazio su disco diventa insufficiente

**Soluzione** :
1. Verificare che `SpeedyPackAutoCompactor` funzioni correttamente
2. Attivare manualmente l'operazione di compattazione
3. Verificare la configurazione della soglia di compattazione
4. Usare lo strumento `SiliconLife.Speedy.Manager` per la compattazione manuale

---

### Problemi di esecuzione degli strumenti

#### Problema : Strumento non trovato

**Sintomi** :
```
Tool "xyz" not found
```

**Soluzione** :
1. Verificare che il nome dello strumento sia corretto
2. Verificare lo strumento nella directory degli strumenti
3. Ricostruire il progetto
4. Verificare che lo strumento sia implementato correttamente

#### Problema : Lo strumento restituisce un errore

**Sintomi** :
```
Tool execution failed: ...
```

**Soluzione** :
1. Esaminare i log dello strumento
2. Verificare i parametri di input
3. Testare lo strumento indipendentemente
4. Verificare i permessi

---

### Problemi di plugin

#### Problema : Caricamento plugin fallito

**Sintomi** :
```
Plugin load failed: Security check failed
```

**Soluzione** :
1. Verificare se il plugin referenzia namespace proibiti (`System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`)
2. Verificare che il plugin referenzi solo assembly dalla whitelist di fiducia
3. Verificare che il plugin implementi correttamente l'interfaccia `IPlugin`
4. Esaminare i log per i dettagli degli errori di verifica di sicurezza

#### Problema : Strumenti del plugin non registrati

**Sintomi** :
- Il plugin è caricato con successo, ma gli strumenti non appaiono nella lista

**Soluzione** :
1. Confermare che la classe dello strumento nel plugin implementi correttamente l'interfaccia `ITool`
2. Verificare che la classe dello strumento sia pubblica
3. Verificare che `ToolManager.ScanAllPluginAssemblies()` sia stato chiamato
4. Ricostruire il plugin e riavviare l'applicazione

---

### Problemi di note di lavoro

#### Problema : Impossibile creare una nota di lavoro

**Sintomi** :
```
Failed to create work note
```

**Soluzione** :
1. Verificare che il Being esista e funzioni
2. Verificare che il percorso di storage abbia i diritti di scrittura
3. Verificare che il contenuto non sia vuoto (contenuto richiesto)
4. Esaminare i log per i dettagli dell'errore

#### Problema : Ricerca di note senza risultati

**Sintomi** :
- La ricerca per parola chiave restituisce risultati vuoti
- Ma note pertinenti esistono sicuramente

**Soluzione** :
1. Verificare l'ortografia della parola chiave
2. Provare una parola chiave più generica
3. Verificare che la nota contenga la parola chiave (case-sensitive)
4. Aumentare il valore del parametro `max_results`

#### Problema : Generazione della directory delle note lenta

**Sintomi** :
- Tempo di risposta lungo per la generazione della directory
- Il Being ha molte note (>1000 pagine)

**Soluzione** :
1. È normale, deve scorrere tutte le note
2. Archiviare regolarmente le note vecchie
3. Usare la funzione di ricerca invece dello scorrimento della directory
4. Ottimizzazione prevista : meccanismo di cache della directory

---

### Problemi della rete di conoscenze

#### Problema : La query di conoscenze restituisce risultati vuoti

**Sintomi** :
```
No knowledge triples found
```

**Soluzione** :
1. Verificare l'ortografia del soggetto e del predicato
2. Verificare che la conoscenza sia stata aggiunta alla rete
3. Usare la funzione di ricerca per la corrispondenza fuzzy :
```json
{
  "action": "search",
  "query": "parola-chiave"
}
```

#### Problema : Ricerca del percorso di conoscenza fallita

**Sintomi** :
```
No path found between concepts
```

**Soluzione** :
1. Verificare che entrambi i concetti esistano nella rete di conoscenze
2. Verificare che esista un percorso di connessione (forse nessuna relazione diretta/indiretta)
3. Aggiungere più conoscenze per stabilire una connessione
4. Ridurre il limite di lunghezza del percorso (se impostato)

#### Problema : Convalida della conoscenza fallita

**Sintomi** :
```
Knowledge validation failed
```

**Soluzione** :
1. Verificare che il formato della tripletta sia corretto (soggetto, predicato, oggetto richiesti)
2. Verificare che il valore di confidenza sia nell'intervallo 0.0-1.0
3. Verificare le triplette duplicate
4. Esaminare i dettagli dell'errore di convalida per il problema specifico

#### Problema : Statistiche della rete di conoscenze imprecise

**Sintomi** :
- I numeri delle statistiche non sono quelli previsti
- Le statistiche non vengono aggiornate dopo l'aggiunta di conoscenze

**Soluzione** :
1. Le statistiche potrebbero richiedere alcuni secondi per aggiornarsi (cache)
2. Verificare che l'operazione di eliminazione sia stata eseguita con successo
3. Riavviare l'applicazione per forzare l'aggiornamento delle statistiche
4. Richiedere le statistiche tramite API

---

### Problemi di gestione del progetto

#### Problema : Impossibile creare un progetto

**Sintomi** :
```
Failed to create project
```

**Soluzione** :
1. Verificare che il nome del progetto non sia vuoto (richiesto)
2. Verificare che il nome del progetto non sia duplicato
3. Verificare che il percorso di storage abbia i diritti di scrittura
4. Esaminare i log per i dettagli dell'errore

#### Problema : Dati di progetto persi

**Sintomi** :
- Le informazioni del progetto non sono caricabili
- I file del progetto sono corrotti

**Soluzione** :
1. Verificare che la directory di storage del progetto esista
2. Ripristinare i dati del progetto da un backup
3. Verificare che il formato del file JSON sia corretto
4. Riparare manualmente i file di progetto corrotti

---

## Debug

### Attivare i log dettagliati

Modificare la configurazione :
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Esaminare i log

I log sono memorizzati in :
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

Visualizzazione in tempo reale :
```bash
tail -f logs/*.log
```

### Usare il debugger

**SiliconLife.Default (implementazione standard)** :
```bash
# Eseguire con il debugger
dotnet run --project src/SiliconLife.Default --configuration Debug

# Collegare il debugger
# Tramite IDE : Collega a processo > SiliconLife.Default
```

**SiliconLife.Fast (versione ad alte prestazioni)** :
```bash
# Eseguire con il debugger
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Collegare il debugger
# Tramite IDE : Collega a processo > SiliconLife.Fast
```

> **Raccomandazione** : Per il debug di sviluppo, usare SiliconLife.Default è raccomandato. Dopo la verifica architetturale riuscita, usare SiliconLife.Fast per il deployment in produzione.

---

## Problemi di prestazioni

### Tempi di risposta lenti

**Ottimizzare** :
1. Ridurre la complessità del modello IA
2. Attivare la cache
3. Pulire i vecchi dati
4. Aumentare le risorse di sistema

### Utilizzo CPU elevato

**Verificare** :
- Troppi Being in esecuzione
- Ciclo infinito negli strumenti
- Esecuzione frequente dei timer

**Soluzione** :
- Ridurre i Being paralleli
- Ottimizzare il codice degli strumenti
- Regolare l'intervallo dei timer

### Utilizzo memoria elevato

**Monitorare** :
```bash
# Tramite interfaccia Web : Dashboard > Memoria
```

**Ottimizzare** :
- Pulire i vecchi ricordi
- Ridurre la dimensione del contesto
- Implementare la paginazione

---

## Ottenere aiuto

### Consultare la documentazione

- [Guida rapida](getting-started.md)
- [Guida di sviluppo](development-guide.md)
- [Riferimento API](api-reference.md)
- [Guida all'architettura](architecture.md)

### Esaminare i log

Esaminare sempre prima i log per i dettagli degli errori.

### Supporto della comunità

- GitHub Issues : Segnalare bug
- Discussions : Fare domande
- Documentazione : Cercare soluzioni

---

## Procedure di emergenza

### Crash del sistema

1. Esaminare i log per la causa
2. Riavviare l'applicazione :

**SiliconLife.Default (implementazione standard)** :
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (versione principale di produzione)** :
```bash
dotnet run --project src/SiliconLife.Fast
```

3. Ripristinare da un backup se necessario

### Perdita di dati

1. Fermare immediatamente l'applicazione
2. Verificare i file di backup
3. Ripristinare i dati
4. Verificare l'integrità

### Violazione di sicurezza

1. Fermare tutti i Being
2. Revocare tutti i permessi
3. Esaminare i log di audit
4. Verificare il controllo di accesso
5. Riavviare con permessi limitati

---

## Prevenzione

### Buone pratiche

1. **Backup regolari**
   - Eseguire il backup della directory dati
   - Eseguire il backup della configurazione
   - Testare il processo di ripristino

2. **Monitorare le risorse**
   - Monitorare l'utilizzo CPU/memoria
   - Monitorare lo spazio su disco
   - Verificare la connessione di rete

3. **Rimanere aggiornati**
   - Aggiornare il SDK .NET
   - Aggiornare le dipendenze
   - Applicare le patch di sicurezza

4. **Testare le modifiche**
   - Testare prima in sviluppo
   - Usare il controllo di versione
   - Documentare le modifiche

---

## Prossimi passi

- 📚 Leggere la [guida all'architettura](architecture.md)
- 🛠️ Consultare la [guida di sviluppo](development-guide.md)
- 🚀 Vedere la [guida rapida](getting-started.md)
- 🔒 Consultare la [documentazione sulla sicurezza](security.md)
