# Guida agli Esseri di Silicio

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md) | **Italiano**

## Che cos'è un Essere di Silicio?

Un **Essere di Silicio** è un agente AI autonomo nel sistema SiliconLifeCollective. Ogni Essere di Silicio possiede:

- **Identità univoca** — GUID e nome proprio
- **Personalità** — Definita dal File dell'Anima (`soul.md`)
- **Capacità** — Accesso a strumenti, memoria, e comunicazione
- **Ciclo di vita** — Creazione, esecuzione, evoluzione, arresto

---

## Architettura

### Separazione Corpo-Cervello

```
┌─────────────────────────────────────┐
│       Essere di Silicio              │
├──────────────────┬──────────────────┤
│   Corpo          │   Cervello        │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Gestione stato │ • Carica storico  │
│ • Rilev. trigger │ • Invoca AI       │
│ • Ciclo di vita  │ • Esegui strum.   │
│                  │ • Persist. risp.  │
└──────────────────┴──────────────────┘
```

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

## Creazione di un Essere

### Tramite Web UI

1. Naviga in **Gestione Esseri di Silicio**
2. Fai clic su **Crea nuovo Essere di Silicio**
3. Compila:
   - Nome
   - Contenuto del File dell'Anima
   - Opzioni di configurazione
4. Fai clic su **Crea**

### Tramite API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

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

### Struttura di Archiviazione

Versione Default:
```
data/
└── beings/
    └── {being-id}/
        ├── soul.md
        ├── memory/
        │   ├── short-term.json
        │   └── long-term/
        │       ├── 2026-04-20.json
        │       └── 2026-04-21.json
        └── tasks/
            └── task-history.json
```

Versione Fast (archiviazione SpeedyPack):
```
data/
├── speedy/
│   ├── {being-id}.spk       # File di archiviazione SpeedyPack
│   └── {being-id}.spk.idx   # File di indice
└── beings/
    └── {being-id}/
        └── soul.md
```

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

## Sistema di Note di Lavoro

### Panoramica

Le note di lavoro sono il sistema di diario personale dell'Essere di Silicio, con design a pagine, utilizzato per registrare progressi di lavoro, apprendimenti, note di progetto, ecc.

### Caratteristiche

- **Gestione a pagine**: Ogni nota è una pagina indipendente, accessibile per numero di pagina
- **Supporto Markdown**: Il contenuto supporta il formato Markdown (testo, liste, tabelle, blocchi di codice)
- **Indice per parole chiave**: Supporta l'aggiunta di parole chiave alle note, facilitando la ricerca
- **Funzione di riassunto**: Ogni nota ha un breve riassunto per una consultazione rapida
- **Generazione indice**: Può generare una panoramica dell'indice di tutte le note, aiutando a comprendere il contesto generale
- **Timestamp**: Registra automaticamente data di creazione e aggiornamento
- **Privato per impostazione predefinita**: Accessibile solo dall'Essere stesso (il Curatore può gestirlo)

### Casi d'Uso

1. **Registrazione progressi di progetto**
   ```
   Riassunto: Completato modulo autenticazione utente
   Contenuto: Implementata verifica JWT token, integrazione OAuth2, meccanismo refresh token
   Parole chiave: autenticazione,JWT,OAuth2
   ```

2. **Note di apprendimento**
   ```
   Riassunto: Apprendere le best practice della programmazione asincrona in C#
   Contenuto: Precauzioni per l'uso di async/await, scenari di utilizzo di ConfigureAwait...
   Parole chiave: C#,asincrono,best practice
   ```

3. **Verbale di riunione**
   ```
   Riassunto: Discussione requisiti di prodotto
   Contenuto: Discussi i requisiti delle nuove funzionalità, determinata la soluzione di implementazione...
   Parole chiave: prodotto,requisiti,riunione
   ```

### Utilizzo tramite Strumenti

L'Essere può gestire le note di lavoro tramite lo strumento `work_note`:

```json
// Creare nota
{
  "action": "create",
  "summary": "Completato modulo autenticazione utente",
  "content": "## Dettagli implementazione\n\n- Uso di JWT token\n- Supporto OAuth2",
  "keywords": "autenticazione,JWT,OAuth2"
}

// Leggere nota
{
  "action": "read",
  "page_number": 1
}

// Cercare note
{
  "action": "search",
  "keyword": "autenticazione",
  "max_results": 10
}
```

### Gestione tramite Web UI

1. Naviga in **Gestione Esseri di Silicio** → Seleziona un Essere
2. Fai clic sulla scheda **Note di lavoro**
3. Puoi visualizzare, cercare e modificare le note
4. Supporta l'anteprima Markdown

---

## Sistema di Rete di Conoscenza

### Panoramica

La rete di conoscenza è un sistema di rappresentazione e gestione della conoscenza basato sulla struttura a tripla (soggetto-predicato-oggetto), utilizzato per memorizzare e gestire conoscenza strutturata.

### Concetti Chiave

#### Struttura a Tripla

```
Soggetto (Subject) --Predicato (Predicate)--> Oggetto (Object)
```

**Esempio**:
- `Python` --`is_a`--> `programming_language`
- `Pechino` --`capital_of`--> `Cina`
- `acqua` --`boiling_point`--> `100°C`

#### Livello di Confidenza

Ogni tripla di conoscenza ha un punteggio di confidenza (0.0-1.0) che indica l'affidabilità della conoscenza:
- `1.0`: Assolutamente certo (es. teoremi matematici)
- `0.8-0.99`: Altamente affidabile (es. fatti verificati)
- `0.5-0.79`: Affidabilità media (es. inferenze o ipotesi)
- `<0.5`: Bassa affidabilità (es. congetture o informazioni non verificate)

#### Sistema di Tag

Supporta l'aggiunta di tag alle triple, facilitando la classificazione e la ricerca:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### Operazioni sulla Conoscenza

#### 1. Aggiungere Conoscenza

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programming", "language"]
}
```

#### 2. Interrogare Conoscenza

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. Cercare Conoscenza

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. Scoprire Percorsi di Conoscenza

Trova il percorso di associazione tra due concetti:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Restituisce:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Validazione della Conoscenza

Verifica la validità e la coerenza della conoscenza:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Statistiche della Conoscenza

Ottieni le informazioni statistiche complessive della rete di conoscenza:
```json
{
  "action": "stats"
}
```

Restituisce:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Casi d'Uso

1. **Memorizzazione di fatti**
   - Memorizza fatti oggettivi e conoscenze comuni
   - Esempio: `Terra` --`is_a`--> `pianeta`

2. **Relazioni tra concetti**
   - Registra le relazioni tra concetti
   - Esempio: `ereditarietà` --`is_a`--> `concetto_di_programmazione_OOP`

3. **Accumulo di apprendimento**
   - L'Essere accumula continuamente conoscenza attraverso l'apprendimento
   - Forma un sistema di conoscenza strutturato

4. **Supporto al ragionamento**
   - Scopre relazioni indirette tramite percorsi di conoscenza
   - Supporta ragionamento e decisioni basati sulla conoscenza

### Gestione tramite Web UI

1. Naviga alla pagina **Rete di Conoscenza**
2. Visualizza le informazioni statistiche della conoscenza
3. Cerca e sfoglia la conoscenza
4. Grafico di visualizzazione delle relazioni di conoscenza (in pianificazione)

---

## Operazioni del Browser WebView (Nuovo)

### Panoramica

Gli Esseri di Silicio possono navigare autonomamente sul web, ottenere informazioni ed eseguire operazioni web tramite lo strumento browser WebView. Il browser opera in modalità headless, completamente invisibile all'utente.

### Caratteristiche

- **Isolamento individuale**: Ogni Essere possiede un'istanza browser indipendente, Cookie e sessione propri
- **Modalità headless**: Operazione autonoma in background, invisibile all'utente
- **Funzionalità complete**: Supporta esecuzione JavaScript, rendering CSS, compilazione moduli, ecc.
- **Controllo di sicurezza**: Tutte le operazioni devono superare la catena di verifica dei permessi

### Operazioni Comuni

#### 1. Aprire il Browser

```json
{
  "action": "open"
}
```

#### 2. Navigare a una Pagina Web

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Ottenere il Contenuto della Pagina

```json
{
  "action": "get_page_text"
}
```

Restituisce il contenuto testuale della pagina, per l'analisi e la comprensione da parte dell'AI.

#### 4. Cliccare su un Elemento

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. Inserire Testo

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "parola chiave di ricerca"
}
```

#### 6. Eseguire JavaScript

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. Ottenere uno Screenshot

```json
{
  "action": "get_screenshot"
}
```

Restituisce uno screenshot della pagina (codifica Base64), utilizzabile per l'analisi visiva.

#### 8. Attendere la Comparsa di un Elemento

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Casi d'Uso

1. **Acquisizione di informazioni**
   - Naviga siti di notizie per ottenere le ultime informazioni
   - Consulta documentazione e materiale tecnico
   - Monitora le modifiche al contenuto delle pagine web

2. **Operazioni automatizzate**
   - Compila moduli e inviali
   - Clicca pulsanti per attivare operazioni
   - Estrae dati dalle pagine web

3. **Analisi web**
   - Analizza struttura e contenuto della pagina
   - Estrae informazioni specifiche
   - Analisi visiva tramite screenshot della pagina

### Note

- Le operazioni del browser possono essere lente, è necessario attendere il caricamento completo della pagina
- Usa `wait_for_element` per assicurarsi che l'elemento sia presente prima di operare
- Rispetta i termini di utilizzo del sito e il robots.txt
- Evita richieste frequenti per non essere bloccati

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

### Transizioni di Stato

```
Idle → SingleChat → Idle (chat completata)
Idle → GroupChat → Idle (chat di gruppo completata)
Idle → Task → Idle (attività completata)
Idle → Timer → Idle (timer completato)
Idle → Broadcast → Idle (broadcast completato)
Idle → Project → Idle (lavoro di progetto completato)
Idle → MemoryCompression → Idle (compressione memoria completata)
Qualsiasi → Stopped (10 errori consecutivi)
Stopped → Idle (nuovo messaggio di chat o riavvio manuale)
```

### Operazioni

- **Avvio**: Inizializzazione e avvio dell'elaborazione
- **Arresto**: Chiusura graceful
- **Riavvio**: Ripristino dallo stato Stopped allo stato Idle

## Sistema di Attività

### Creazione di un'Attività

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Review the code",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### Stato dell'Attività

- `Pending` - In attesa di esecuzione
- `Running` - In esecuzione
- `SubmittedForReview` - Inviato per revisione
- `UnderReview` - In revisione
- `Rework` - Rielaborazione modifiche
- `Completed` - Completato con successo
- `Failed` - Esecuzione fallita
- `Cancelled` - Annullato manualmente

## Sistema di Timer

### Tipi di Timer

1. **Una volta**: Esegue una sola volta dopo un ritardo
2. **Intervallo**: Ripete l'esecuzione a intervalli fissi
3. **Cron**: Esegue in base a espressioni cron

### Esempio

```csharp
// Esegue ogni ora
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## Sistema di Competenze

Le Competenze (Skill) sono unità di capacità riutilizzabili degli Esseri di Silicio — incapsulano "orchestrazione di strumenti + template di prompt" in una funzione dichiarabile, evolvibile e automaticamente pianificabile, che l'AI richiama come un normale strumento.

### Struttura della Competenza

| Elemento | Descrizione |
|------|------|
| `id` / `description` | Identificatore univoco e descrizione in una frase (mostrata all'AI, determina quando l'AI sceglie la competenza) |
| `parameter_schema` | JSON Schema dei parametri, dichiara ogni placeholder `{param}` utilizzato nel prompt |
| `system_prompt_template` | Template del prompt di sistema, i placeholder vengono riempiti con i parametri in fase di esecuzione |
| `tool_whitelist` | Lista degli strumenti consentiti durante l'esecuzione (vuoto = eredita tutti gli strumenti dell'Essere) |
| `max_tool_round` / `timeout` | Limite di round di strumenti e timeout (soggetti a clamp dei limiti globali) |
| `on_complete` | Azione di completamento: `none` / `write_memory` / `notify_curator` / `broadcast` |
| `trigger_mode` | `Manual` (l'AI richiama autonomamente) o `Auto` + pianificazione `schedule` |

### Quattro Fonti

- **Builtin** — Integrato nel framework (`summarize_document` riassunto documento, `code_review` revisione codice, `research_topic` ricerca argomento)
- **Plugin** — Registrato tramite `ISkillProvider`
- **Being** — Creato dall'Essere a runtime tramite lo strumento `skill`
- **User** — Creato dall'utente tramite la pagina di gestione competenze della Web UI

### Modalità di Attivazione

1. **Manuale (Manual)**: La competenza viene iniettata come definizione di strumento ordinaria nella richiesta AI, l'AI decide quando richiamarla; il lato pianificazione instrada con priorità le chiamate con lo stesso nome alla competenza
2. **Automatica (Auto + schedule)**: L'espressione di pianificazione è memorizzata in `metadata.schedule`, supporta tre formati:
   - `"09:30"` — Orario fisso giornaliero
   - `"6h"` / `"30 m"` / `"2 d"` — Periodo di intervallo
   - `"0 9 * * *"` / `"*/15 * * * *"` — Subset di cron

### Scrittura in Markdown

Le competenze sono memorizzate in Markdown (`skills/{id}.md`, frontmatter YAML + corpo del prompt):

```markdown
---
id: daily_news_digest
description: Cerca le notizie tecnologiche di oggi e genera un riassunto
tool_whitelist: [network, work_note]
on_complete: write_memory
---

Usa lo strumento network per cercare le ultime notizie su {topic}, genera un riassunto di 500 parole e salvalo nelle note di lavoro.
```

È possibile scrivere solo il corpo (omettendo il YAML): al salvataggio l'AI completerà automaticamente i metadati come id, description, schema dei parametri — i campi già compilati dall'utente non vengono mai sovrascritti.

### Autogestione dell'Essere

L'Essere può gestire il proprio repository di competenze tramite lo strumento `skill`:

```json
{ "action": "list" }
{ "action": "create", "id": "my_skill", "system_prompt": "...", "description": "..." }
{ "action": "update_from_md", "skill_id": "my_skill", "markdown": "..." }
{ "action": "delete", "skill_id": "my_skill" }
```

### Ricaricamento a Caldo ed Evoluzione

- L'Essere rileva ogni 30 secondi le modifiche alla directory `skills/` (confronto di impronte), le modifiche dalla Web UI o da altri Esseri hanno effetto automatico, senza necessità di riavvio
- Ogni aggiornamento di competenza archivia automaticamente una versione storica in `skills/archive/{id}/{version}.md`, formando una storia evolutiva della competenza
- Il numero di competenze personalizzate è limitato da una quota (`MaxCustomSkillsPerBeing`, default 50)

### Misura di Sicurezza dell'Esecuzione

- Permesso di azione `execute` a livello di competenza (può essere disabilitato dalla matrice dei permessi, se disabilitato l'AI non la vede)
- I parametri di esecuzione sono sottoposti a clamp dei limiti globali: round ≤ `GlobalMaxToolRound` (default 10), timeout ≤ `GlobalSkillTimeoutSeconds` (default 300 secondi)
- Le competenze non possono richiamare se stesse ricorsivamente
- Le chiamate a strumenti fuori dalla whitelist falliscono direttamente

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

### Gestione delle Attività

1. **Imposta priorità** — Usa livelli di priorità (1-10)
2. **Definisci scadenze** — Imposta sempre una data di scadenza
3. **Monitora i progressi** — Controlla regolarmente lo stato delle attività
4. **Gestisci i fallimenti** — Implementa logiche di ripetizione

### Ottimizzazione della Memoria

1. **Pulisci i dati obsoleti** — Archivia periodicamente le vecchie memorie
2. **Indica le informazioni importanti** — Contrassegna le informazioni chiave
3. **Usa l'archiviazione temporale** — Sfrutta l'indicizzazione temporale per le query

### Meccanismo di Sfumatura della Memoria

Il sistema integra un servizio di decadimento timer `MemoryFadeService` che simula le caratteristiche di oblio della memoria biologica:

- **Decadimento automatico** — Applica ogni ora un algoritmo di decadimento dell'importanza a tutte le voci di memoria degli Esseri di Silicio
- **Archiviazione automatica** — Le memorie con importanza inferiore alla soglia vengono archiviate automaticamente e non partecipano più al recupero quotidiano
- **Tracciamento statistico** — Registra il numero di cicli di decadimento e le voci con stato modificato

Ciò significa che la memoria degli Esseri di Silicio sfuma naturalmente nel tempo; le informazioni importanti devono essere attivamente contrassegnate con alta importanza tramite lo strumento di memoria, per evitare l'archiviazione automatica.

---

## Area di Lavoro del Progetto

### Panoramica

L'area di lavoro del progetto è un meccanismo di gestione degli spazi che supporta la collaborazione tra multipli Esseri di Silicio. Il Curatore di Silicio può creare spazi di progetto, assegnare Esseri di Silicio ai progetti e attribuire loro dei ruoli.

### Ciclo di Vita del Progetto

```
Creazione → Attivo → Archiviato → Distrutto
               ↑       |
               └─ Ripristino ┘
```

### Ruoli del Progetto

Agli Esseri di Silicio possono essere assegnati ruoli specifici all'interno del progetto:

```json
{
  "action": "assign_role",
  "project_id": "project-uuid",
  "being_id": "being-uuid",
  "role_name": "developer"
}
```

### Note di Lavoro del Progetto

Le note di lavoro all'interno dello spazio di progetto sono pubbliche e accessibili a tutti i membri del progetto:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Completato modulo autenticazione utente",
  "content": "## Dettagli implementazione\n\n- Uso di JWT token",
  "keywords": "autenticazione,JWT"
}
```

### Attività del Progetto

Le attività nello spazio di progetto supportano una gestione completa del ciclo di vita:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "title": "Implementare autenticazione utente",
  "priority": 5
}
```

### Workflow del Progetto

I progetti possono essere collegati a template di workflow per guidare i processi collaborativi degli Esseri di Silicio:

- I workflow sono basati su template di macchina a stati
- Supportano transizioni di stato guidate da Tick
- Registrano automaticamente i log delle transizioni di stato

### Isolamento dei Permessi degli Strumenti

I permessi degli strumenti a livello di progetto sono indipendenti dai permessi a livello di Essere di Silicio, realizzando l'isolamento dei permessi tra progetti. Ad esempio, un Essere di Silicio potrebbe avere permessi di accesso alla rete nel Progetto A, ma essere limitato a permessi di sola lettura nel Progetto B.

## Risoluzione dei Problemi

### Impossibile Avviare l'Essere

**Verifica**:
- Il File dell'Anima esiste ed è valido
- Il client AI è configurato
- Le risorse di sistema sono sufficienti

### Arresto Imprevisto dell'Essere

**Verifica**:
- Errori nei log
- Disponibilità del servizio AI
- Utilizzo della memoria

### Attività Non Eseguita

**Verifica**:
- Il sistema di timer è in esecuzione
- Priorità e pianificazione delle attività
- Impostazioni dei permessi

## Prossimi Passi

- 📚 Leggi la [Guida all'Architettura](architecture.md)
- 🛠️ Consulta la [Guida allo Sviluppo](development-guide.md)
- 🚀 Visualizza la [Guida Rapida](getting-started.md)
