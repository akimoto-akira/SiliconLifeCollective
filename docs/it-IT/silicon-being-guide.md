# Guida ai Silicon Beings

> **Versione: v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [Français](../fr-FR/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md) | **Italiano**

## Panoramica

I Silicon Beings sono agenti guidati dall'IA capaci di pensare, agire ed evolversi autonomamente.

## Architettura

### Separazione Body-Brain

```
┌─────────────────────────────────────┐
│         Silicon Being               │
├──────────────────┬──────────────────┤
│   Body           │   Brain          │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Gestione stato │ • Caricare cronologia  │
│ • Rilevamento    │ • Chiamare l'IA   │
│   attivazioni    │ • Eseguire strumenti│
│ • Ciclo di vita  │ • Persistere risposta│
└──────────────────┴──────────────────┘
```

## File dell'anima

### Struttura

```markdown
# Nome del Being

## Personality
Descrivi i tratti della personalità e le caratteristiche del being.

## Capabilities
Elenca cosa questo being può fare.

## Behavior Guidelines
Definisci come il being deve comportarsi in diverse situazioni.

## Knowledge Domain
Specifica il dominio di competenza del being.
```

### Esempio

```markdown
# Assistente revisione codice

## Personality
Sei un revisore del codice meticoloso con 10 anni di esperienza.
Fornisci feedback costruttivi e spiega sempre il tuo ragionamento.

## Capabilities
- Revisionare il codice per bug e buone pratiche
- Suggerire ottimizzazioni delle prestazioni
- Spiegare algoritmi complessi
- Identificare vulnerabilità di sicurezza

## Behavior Guidelines
- Iniziare con osservazioni positive
- Fornire esempi specifici
- Spiegare perché le modifiche sono necessarie
- Essere rispettoso e professionale

## Knowledge Domain
Specializzato in C#, .NET e architettura software.
```

## Creare un Being

### Tramite l'interfaccia Web

1. Navigare verso **Gestione Beings**
2. Cliccare su **Crea nuovo Being**
3. Compilare:
   - Nome
   - Contenuto anima
   - Opzioni configurazione
4. Cliccare su **Crea**

### Tramite API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistente",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## Ciclo di vita di un Being

### Stati di attività

I Silicon Beings hanno i seguenti stati di attività:

| Stato | Descrizione |
|------|------|
| `Idle` | Stato inattivo, in attesa del trigger dell'orologio |
| `Working` | In esecuzione di un ciclo richiesta IA + chiamata strumento |
| `Error` | Si è verificato un errore durante l'esecuzione |
| `Stopped` | Arrestato, a causa di errori consecutivi o arresto manuale |

**Meccanismo stato Stopped**:
- Quando un Silicon Being subisce 10 errori consecutivi, entra automaticamente nello stato `Stopped`
- Una volta nello stato Stopped, il Being non eseguirà più alcuna attività
- È richiesto un intervento manuale per riavviare

### Transizioni di stato

```
Idle → Working → Idle (terminazione normale)
Working → Error → Working (recupero errore)
Working → Stopped (10 errori consecutivi o arresto manuale)
Stopped → Idle (riavvio)
```

### Operazioni

- **Avvia**: Inizializzare e iniziare l'elaborazione
- **Ferma**: Arresto graduale
- **Riavvia**: Ritorno allo stato Idle dallo stato Stopped

## Sistema di attività

### Creare un'attività

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

### Stati delle attività

- `Pending` - In attesa di esecuzione
- `Running` - In corso di esecuzione
- `Completed` - Completato con successo
- `Failed` - Esecuzione fallita
- `Cancelled` - Annullato manualmente

## Sistema di timer

### Tipi di timer

1. **Puntuale**: Esecuzione singola dopo un ritardo
2. **Intervallo**: Ripetizione a intervalli fissi
3. **Cron**: Esecuzione basata su espressione Cron

### Esempio

```csharp
// Eseguire ogni ora
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## Sistema di memoria

### Tipi di memoria

- **Breve termine**: Contesto di conversazione attuale
- **Lungo termine**: Conoscenze ed esperienze persistite
- **Episodica**: Eventi e interazioni indicizzati nel tempo

### Struttura archiviazione

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
│   ├── {being-id}.spk       # File archiviazione SpeedyPack
│   └── {being-id}.spk.idx   # File indice
└── beings/
    └── {being-id}/
        └── soul.md
```

## Sistema di note di lavoro

### Panoramica

Le note di lavoro sono un sistema di diario personale dei Silicon Beings con un design a pagine per registrare la progressione del lavoro, le esperienze di apprendimento, le note di progetto, ecc.

### Funzionalità

- **Gestione pagine**: Ogni nota è una pagina distinta, accessibile per numero di pagina
- **Supporto Markdown**: Il contenuto supporta il formato Markdown (testo, elenchi, tabelle, blocchi codice)
- **Indice per parole chiave**: Le note possono essere etichettate con parole chiave per la ricerca
- **Riepilogo**: Ogni nota ha un breve riepilogo per navigazione rapida
- **Generazione indice**: Può generare un indice di tutte le note per una panoramica
- **Timestamp**: Registrazione automatica delle date di creazione e aggiornamento
- **Privato per default**: Solo il Being vi ha accesso (il Curator può gestire)

### Scenari di utilizzo

1. **Documentare la progressione del progetto**
   ```
   Riepilogo: Modulo autenticazione utente completato
   Contenuto: Verifica JWT, integrazione OAuth2, meccanismo refresh token implementati
   Parole chiave: autenticazione,JWT,OAuth2
   ```

2. **Note di apprendimento**
   ```
   Riepilogo: Buone pratiche programmazione asincrona C# apprese
   Contenuto: Note su async/await, casi d'uso di ConfigureAwait...
   Parole chiave: C#,Async,Buone pratiche
   ```

3. **Verbali riunione**
   ```
   Riepilogo: Riunione requisiti prodotto
   Contenuto: Nuovi requisiti funzionalità discussi, approccio implementazione definito...
   Parole chiave: prodotto,requisiti,riunione
   ```

### Utilizzo tramite strumento

I Beings possono gestire le loro note di lavoro tramite lo strumento `work_note`:

```json
// Creare una nota
{
  "action": "create",
  "summary": "Modulo autenticazione utente completato",
  "content": "## Dettagli implementazione\n\n- Utilizzo JWT token\n- Supporto OAuth2",
  "keywords": "autenticazione,JWT,OAuth2"
}

// Leggere una nota
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

### Gestione tramite interfaccia Web

1. Navigare verso **Gestione Beings** → Selezionare un Being
2. Cliccare sulla scheda **Note di lavoro**
3. Visualizzare, cercare, modificare le note
4. Anteprima Markdown supportata

---

## Sistema di rete di conoscenza

### Panoramica

La rete di conoscenza è un sistema di rappresentazione e gestione delle conoscenze basato su una struttura a triplette (Soggetto-Predicato-Oggetto) per l'archiviazione e la gestione di conoscenze strutturate.

### Concetti chiave

#### Struttura a triplette

```
Soggetto (Subject) --Predicato (Predicate)--> Oggetto (Object)
```

**Esempi**:
- `Python` --`is_a`--> `programming_language`
- `Parigi` --`capital_of`--> `Francia`
- `acqua` --`boiling_point`--> `100°C`

#### Valore di confidenza

Ogni tripletta di conoscenza ha un valore di confidenza (0.0-1.0) che indica l'affidabilità della conoscenza:
- `1.0` : Assolutamente certo (come teoremi matematici)
- `0.8-0.99` : Alta confidenza (come fatti verificati)
- `0.5-0.79` : Confidenza media (come inferenze o ipotesi)
- `<0.5` : Bassa confidenza (come congetture o informazioni non verificate)

#### Sistema di tag

Supporta l'aggiunta di tag alle triplette per la classificazione e la ricerca:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### Operazioni di conoscenza

#### 1. Aggiungere conoscenza

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

#### 2. Consultare conoscenza

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. Cercare conoscenze

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. Scoprire percorsi di conoscenza

Trova i percorsi di connessione tra due concetti:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Risultato:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Validare conoscenza

Verifica la validità e la coerenza della conoscenza:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Statistiche rete conoscenza

Ottiene le statistiche globali della rete di conoscenza:
```json
{
  "action": "stats"
}
```

Risultato:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Scenari di utilizzo

1. **Archiviazione fatti**
   - Archiviare fatti oggettivi e conoscenze generali
   - Esempio: `Terra` --`is_a`--> `pianeta`

2. **Relazioni tra concetti**
   - Registrare le relazioni tra concetti
   - Esempio: `ereditarietà` --`is_a`--> `concetto_programmazione_orientata_oggetti`

3. **Accumulo apprendimento**
   - I Beings accumulano continuamente conoscenze attraverso l'apprendimento
   - Formano sistemi di conoscenze strutturati

4. **Supporto inferenza**
   - Scoprire relazioni indirette tramite i percorsi di conoscenza
   - Supportare l'inferenza e il processo decisionale basati sulla conoscenza

### Gestione tramite interfaccia Web

1. Navigare verso la pagina **Rete di conoscenza**
2. Visualizzare le statistiche di conoscenza
3. Cercare e sfogliare le conoscenze
4. Visualizzare il diagramma di relazioni di conoscenza (previsto)

---

## Operazioni browser WebView (Nuovo)

### Panoramica

I Silicon Beings possono navigare autonomamente sul Web, recuperare informazioni ed eseguire operazioni Web tramite lo strumento browser WebView. Il browser funziona in modalità headless, totalmente invisibile per l'utente.

### Funzionalità

- **Isolamento individuale**: Ogni Being ha la propria istanza del browser, cookie e sessioni
- **Modalità headless**: Operazione autonoma in background, invisibile per l'utente
- **Funzionalità completa**: Supporta esecuzione JavaScript, rendering CSS, compilazione moduli, ecc.
- **Controllo sicurezza**: Tutte le operazioni devono passare attraverso la catena di permessi

### Operazioni comuni

#### 1. Aprire il browser

```json
{
  "action": "open_browser"
}
```

#### 2. Navigare verso un sito Web

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Recuperare contenuto pagina

```json
{
  "action": "get_page_text"
}
```

Restituisce il contenuto testuale della pagina per l'analisi e la comprensione da parte dell'IA.

#### 4. Cliccare su un elemento

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. Inserire testo

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "parola chiave ricerca"
}
```

#### 6. Eseguire JavaScript

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. Ottenere screenshot

```json
{
  "action": "get_screenshot"
}
```

Restituisce uno screenshot della pagina (codificato in Base64), utilizzabile per analisi visiva.

#### 8. Attendere elemento

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Scenari di utilizzo

1. **Raccolta informazioni**
   - Navigare su siti di notizie per informazioni aggiornate
   - Consultare documentazione e risorse tecniche
   - Monitorare cambiamenti di contenuto di siti Web

2. **Operazioni automatizzate**
   - Compilare e inviare moduli
   - Cliccare pulsanti per operazioni
   - Estrarre dati Web

3. **Analisi Web**
   - Analizzare struttura e contenuto pagine
   - Estrarre informazioni specifiche
   - Analisi visiva tramite screenshot

### Note

- Le operazioni del browser possono essere lente, in attesa del caricamento pagine
- Utilizzare `wait_for_element` per assicurarsi che l'elemento sia presente
- Rispettare i termini di utilizzo e il file robots.txt dei siti Web
- Evitare richieste frequenti per prevenire bann

---

## Buone pratiche

### Scrivere un file dell'anima

1. **Concreto**: Tratti personalità e limiti chiari
2. **Definire il perimetro**: Cosa il Being deve e non deve fare
3. **Includere esempi**: Mostrare modelli di comportamento attesi
4. **Aggiornare regolarmente**: Far evolvere l'anima in base alle prestazioni

### Gestione attività

1. **Definire priorità**: Utilizzare priorità (1-10)
2. **Definire scadenze**: Impostare sempre una data limite
3. **Monitorare progressione**: Verificare regolarmente stato attività
4. **Gestire errori**: Implementare logica di retry

### Ottimizzazione memoria

1. **Pulire vecchi dati**: Archiviare regolarmente vecchi ricordi
2. **Indicizzare informazioni importanti**: Marcare informazioni chiave
3. **Utilizzare archiviazione temporale**: Sfruttare query per indice temporale

## Risoluzione problemi

### Il Being non si avvia

**Verificare**:
- Il file dell'anima esiste ed è valido
- Il client IA è configurato
- Le risorse sistema sono sufficienti

### Il Being si ferma inaspettatamente

**Verificare**:
- Gli errori nei log
- La disponibilità del servizio IA
- L'utilizzo della memoria

### Le attività non vengono eseguite

**Verificare**:
- Il sistema di timer funziona
- La priorità e la pianificazione delle attività
- Le impostazioni dei permessi

## Prossimi passi

- 📚 Leggere la [guida architettura](architecture.md)
- 🛠️ Consultare la [guida sviluppo](development-guide.md)
- 🚀 Vedere la [guida avvio rapido](getting-started.md)
