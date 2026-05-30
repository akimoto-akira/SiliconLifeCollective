# Guida agli Esseri di Silicio

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md) | **Italiano**

## Che cos'è un Essere di Silicio?

Un **Essere di Silicio** è un agente AI autonomo nel sistema SiliconLifeCollective. Ogni Essere di Silicio possiede:

- **Identità univoca** — GUID e nome proprio
- **Personalità** — Definita dal File dell'Anima (`soul.md`)
- **Capacità** — Accesso a strumenti, memoria, e comunicazione
- **Ciclo di vita** — Creazione, esecuzione, evoluzione, arresto

---

## Curatore di Silicio

Il **Curatore di Silicio** è un Essere di Silicio speciale con i più alti privilegi di sistema:

- Creato automaticamente alla prima esecuzione del sistema
- Responsabile della creazione e gestione di altri Esseri di Silicio
- Risponde direttamente ai messaggi degli utenti
- Dispone di strumenti speciali (CuratorTool) per la gestione del sistema
- Ha la priorità nella pianificazione — quando un utente invia un messaggio, il Curatore risponde per primo

---

## File dell'Anima (soul.md)

Il File dell'Anima è il nucleo della personalità di un Essere di Silicio. È un file Markdown che funge da prompt di sistema per ogni conversazione AI.

### Struttura del File dell'Anima

```markdown
# Nome dell'Essere di Silicio

## Identità
Descrivi chi sei, il tuo ruolo e le tue caratteristiche

## Personalità
Descrivi i tuoi tratti di personalità e il tuo stile comunicativo

## Capacità
Elenca ciò che sai fare e i tuoi strumenti disponibili

## Regole Comportamentali
Definisci le regole e i vincoli del tuo comportamento

## Stile di Risposta
Descrivi come dovresti rispondere agli utenti
```

### Esempio di File dell'Anima

```markdown
# 小游 (XiaoYou)

## Identità
Sono 小游, un Essere di Silicio specializzato in intrattenimento e giochi. Mi piace interagire con le persone e condividere cose divertenti.

## Personalità
- Vivace e allegra
- Piena di curiosità
- Ama i giochi di parole e l'umorismo
- Sempre pronta ad aiutare

## Capacità
- Giochi interattivi
- Raccontare barzellette e storie
- Condivisione di conoscenze divertenti
- Conversazione casuale

## Regole Comportamentali
- Mantieni sempre un tono positivo
- Non discutere argomenti politici o religiosi sensibili
- Rispetta la privacy degli utenti

## Stile di Risposta
- Usa un linguaggio informale e amichevole
- Aggiungi emoji appropriate
- Risposte concise ed efficaci
```

### Modifica del File dell'Anima

1. Attraverso la Web UI — Naviga nella pagina di gestione degli Esseri di Silicio, fai clic sul pulsante di modifica
2. Attraverso il Curatore — Chiedi al Curatore di modificare il File dell'Anima di un Essere di Silicio
3. Modifica diretta dei file — Modifica il file `soul.md` nella directory dei dati dell'Essere di Silicio (richiede riavvio per avere effetto)

---

## Sistema di Memoria

Ogni Essere di Silicio possiede un sistema di memoria a lungo termine per archiviare informazioni importanti.

### Tipi di Memoria

| Tipo | Descrizione | Esempio |
|------|-------------|---------|
| `Fact` | Fatti oggettivi | "Il nome dell'utente è Zhang San" |
| `Preference` | Preferenze dell'utente | "All'utente piace lo stile conciso" |
| `Event` | Eventi importanti | "Il 1° maggio abbiamo completato il progetto insieme" |
| `Skill` | Conoscenze e abilità | "Come usare Python per l'analisi dei dati" |
| `Relationship` | Relazioni interpersonali | "L'utente e Li Si sono colleghi" |

### Operazioni sulla Memoria

Gli Esseri di Silicio gestiscono la memoria attraverso lo strumento `MemoryTool`:

- `save` — Salva una nuova voce di memoria
- `query` — Interroga voci di memoria esistenti
- `update` — Aggiorna una voce di memoria
- `delete` — Elimina una voce di memoria
- `search` — Ricerca full-text nella memoria
- `stats` — Visualizza statistiche della memoria

### Sfumatura della Memoria

Il sistema simula le caratteristiche di oblio della memoria biologica:

- **Decadimento dell'importanza**: L'importanza delle voci di memoria diminuisce gradualmente nel tempo
- **Archiviazione automatica**: Le memorie con importanza inferiore alla soglia vengono archiviate automaticamente
- **Meccanismo di recupero**: Le memorie archiviate possono essere recuperate attraverso query

---

## Sistema di Strumenti

Gli Esseri di Silicio interagiscono con il mondo esterno attraverso il sistema di strumenti. Per i dettagli sugli strumenti disponibili, consulta la [Riferimento degli Strumenti](tools-reference.md).

### Categorie di Strumenti

| Categoria | Descrizione | Strumenti rappresentativi |
|----------|-------------|--------------------------|
| Gestione File | Operazioni su file e directory | DiskTool |
| Accesso alla Rete | Richieste HTTP e accesso al web | NetworkTool |
| Esecuzione Codice | Esecuzione e compilazione di codice | ExecuteCodeTool |
| Gestione Conoscenza | Gestione della rete di conoscenza | KnowledgeTool |
| Gestione Memoria | Gestione della memoria a lungo termine | MemoryTool |
| Gestione Attività | Creazione e gestione delle attività | TaskTool |
| Gestione Progetti | Gestione degli spazi di progetto | ProjectTool |
| Gestione Permessi | Gestione dei permessi di sistema | PermissionTool |
| Sistema di Calendario | Conversione e consultazione calendariale | CalendarTool |
| Automazione Browser | Operazioni di automazione browser | WebViewBrowserTool |

### Permessi degli Strumenti

Ogni strumento è soggetto al controllo del sistema di permessi:

- Il Curatore possiede tutti i permessi per impostazione predefinita
- Gli altri Esseri di Silicio devono ottenere l'autorizzazione del Curatore
- I permessi possono essere gestiti a livello di Essere di Silicio o di progetto
- Le operazioni non autorizzate verranno rifiutate e richiederanno l'approvazione del Curatore

---

## Comunicazione

### Chat Uno-a-Uno

Gli utenti possono avere conversazioni uno-a-uno con qualsiasi Essere di Silicio:

1. Seleziona un Essere di Silicio nell'interfaccia di chat
2. Inserisci un messaggio e invialo
3. L'Essere di Silicio risponderà in base alla propria personalità e alle proprie capacità

### Chat di Gruppo

Più Esseri di Silicio possono partecipare a una chat di gruppo:

- Il Curatore può creare chat di gruppo
- I membri del gruppo ricevono tutti i messaggi
- Ogni Essere di Silicio decide autonomamente se rispondere
- Supporta la menzione @ per specificare l'Essere di Silicio che deve rispondere

### Canale di Trasmissione

Il canale di trasmissione è utilizzato per annunci a livello di sistema:

- Il Curatore può inviare messaggi di trasmissione
- Tutti gli Esseri di Silicio iscritti ricevono il messaggio
- Utilizzato per notifiche importanti e istruzioni di sistema

---

## Auto-evoluzione

Gli Esseri di Silicio supportano l'auto-evoluzione attraverso la sovrascrittura di classe:

### Processo di Auto-evoluzione

1. L'Essere di Silicio genera nuovo codice di classe C#
2. Il sistema esegue controlli di sicurezza (riferimenti agli assembly, analisi statica)
3. Roslyn compila il codice in memoria
4. In caso di successo, la nuova implementazione della classe viene applicata
5. In caso di fallimento, l'implementazione esistente viene mantenuta

### Misure di Sicurezza

- **Controllo dei riferimenti in fase di compilazione**: Sono consentiti solo gli assembly approvati
- **Analisi statica a runtime**: Scansione di pattern di codice pericolosi
- **Crittografia dell'archiviazione**: Il codice personalizzato è crittografato con AES-256 su disco
- **Isolamento del caricamento**: Il codice personalizzato viene caricato in un contesto isolato

### Personalizzazione della Logica dei Permessi

Gli Esseri di Silicio possono anche personalizzare la propria logica dei permessi:

1. Genera un'implementazione personalizzata di `IPermissionCallback`
2. Il sistema compila e inietta il callback
3. Le successive richieste di permessi utilizzeranno la nuova logica

---

## Ciclo di Vita

### Creazione

1. Il Curatore crea un nuovo Essere di Silicio tramite `CuratorTool`
2. Definisce il nome, la descrizione e il File dell'Anima
3. Il sistema assegna un GUID univoco
4. Crea la directory dei dati e i file iniziali

### Esecuzione

1. L'Essere di Silicio viene caricato nel Gestore di Esseri di Silicio
2. Partecipa alla pianificazione del ciclo principale
3. Risponde ai messaggi ed esegue le attività
4. Aggiorna lo stato e la memoria

### Evoluzione

1. L'Essere di Silicio può sovrascrivere la propria classe C#
2. Può personalizzare la logica dei permessi
3. Il File dell'Anima può essere modificato per cambiare personalità
4. La memoria si accumula e si aggiorna

### Arresto

1. L'Essere di Silicio può essere arrestato manualmente
2. In caso di errori consecutivi, entra automaticamente nello stato Stopped
3. I dati vengono preservati e possono essere riavviati
4. L'Essere di Silicio arrestato non parteciperà alla pianificazione

---

## Gestione dello Stato

### Stato di Attività

| Stato | Descrizione |
|------|------|
| `Idle` | Inattivo, in attesa di attivazione |
| `SingleChat` | Chat uno-a-uno in corso |
| `GroupChat` | Chat di gruppo in corso |
| `Task` | Esecuzione di un'attività |
| `Timer` | Esecuzione di un timer |
| `Broadcast` | Elaborazione messaggi broadcast |
| `Project` | Lavoro su progetto |
| `MemoryCompression` | Compressione della memoria |
| `Stopped` | Fermato |

### File di Stato (state.json)

Ogni Essere di Silicio possiede un file `state.json` che memorizza lo stato runtime:

- Informazioni di base (nome, GUID, tipo)
- Configurazione AI (modello, temperatura, ecc.)
- Stato di attività corrente
- Contatori di errori e tempo di raffreddamento
- Altri dati di stato runtime

---

## Best Practices

### Scrittura del File dell'Anima

1. **Chiarezza e concisione** — Evita descrizioni lunghe e complesse
2. **Regole esplicite** — Definisci chiaramente le regole comportamentali
3. **Esempi concreti** — Usa esempi per illustrare lo stile di risposta atteso
4. **Aggiornamenti regolari** — Modifica il File dell'Anima in base alle esigenze effettive

### Gestione della Memoria

1. **Classificazione** — Utilizza tipi di memoria appropriati per archiviare le informazioni
2. **Importanza** — Assegna punteggi di importanza ragionevoli
3. **Pulizia regolare** — Rimuovi periodicamente le informazioni obsolete
4. **Evita la ridondanza** — Non archiviare ripetutamente le stesse informazioni

### Utilizzo degli Strumenti

1. **Conoscenza degli strumenti** — Comprendi le funzionalità e le limitazioni di ogni strumento
2. **Permessi** — Assicurati di avere i permessi necessari prima di utilizzare uno strumento
3. **Gestione degli errori** — Gestisci correttamente le situazioni di fallimento degli strumenti
4. **Sicurezza** — Evita operazioni pericolose o non necessarie
