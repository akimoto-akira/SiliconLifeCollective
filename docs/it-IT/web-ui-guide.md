# Guida Web UI

> **Versione: v0.2.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md) | [Русский](../ru-RU/web-ui-guide.md) | **Italiano**

## Panoramica

La Web UI fornisce un'interfaccia completa per la gestione degli Esseri di Silicio, il monitoraggio dello stato del sistema e l'interazione con gli agenti AI. Il sistema adotta un'architettura di rendering puramente lato server, senza dipendenze da framework frontend, generando HTML, CSS e JavaScript tramite i builder `H`, `CssBuilder` e `JsBuilder`.

## Accesso

URL predefinito: `http://localhost:8080`

## Navigazione

### Sezioni Principali

1. **Dashboard** - Panoramica di sistema e metriche
2. **Esseri** - Gestione degli Esseri di Silicio
3. **Chat** - Interazione con gli esseri (supporto caricamento file, SSE in tempo reale)
4. **Cronologia Chat** - Visualizzazione della cronologia chat degli Esseri di Silicio (elenco sessioni, dettagli messaggi)
5. **Attività** - Gestione attività (attività personali)
6. **Timer** - Configurazione timer (creazione, pausa, cronologia esecuzione)
7. **Configurazione** - Impostazioni di sistema (client AI, localizzazione)
8. **Permessi** - Controllo degli accessi (gestione ACL, query permessi)
9. **Log** - Log di sistema (filtro per livello, query per intervallo temporale)
10. **Audit** - Utilizzo dei token e traccia di audit
11. **Memoria** - Memoria degli esseri (vista timeline, filtro avanzato)
12. **Conoscenza** - Base di conoscenza (gestione triple, scoperta percorsi)
13. **Browser Codice** - Esplorazione codice (albero file, evidenziazione sintassi)
14. **Editor Codice** - Modifica codice con tooltip sospesi (Monaco Editor)
15. **Progetti** - Gestione progetti (area di lavoro, attività, note di lavoro)
16. **Esecutori** - Gestione esecutori (disco, rete, riga di comando)
17. **Aiuto** - Sistema di documentazione di aiuto (supporto multilingua, ricerca per argomento)
18. **Informazioni** - Informazioni di sistema e versione

---

## Dashboard

### Funzionalità

- Metriche delle prestazioni di sistema (CPU, memoria, tempo di attività)
- Panoramica dello stato degli esseri
- Statistiche di utilizzo AI
- Azioni rapide

### Aggiornamenti in Tempo Reale

Utilizza SSE (Server-Sent Events) per ottenere dati in tempo reale:

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Gestione degli Esseri

### Elenco degli Esseri

Mostra tutti gli esseri, includendo:
- Nome e ID
- Stato corrente (in esecuzione/fermato/errore)
- Link al File dell'Anima
- Azioni rapide (avvio/arresto/configurazione)

### Dettagli dell'Essere

- Configurazione completa
- Editor del File dell'Anima
- Cronologia delle attività
- Visualizzatore della memoria
- Metriche delle prestazioni

### Creazione di un Essere

1. Fai clic su **Crea nuovo essere**
2. Compila:
   - Nome
   - Contenuto dell'anima (editor Markdown)
   - Configurazione iniziale
3. Fai clic su **Crea**

---

## Interfaccia di Chat

### Funzionalità

- Flusso di messaggi in tempo reale
- Cronologia dei messaggi
- Supporto multi-sessione
- Visualizzazione delle chiamate strumento

### Utilizzo della Chat

1. Seleziona un essere
2. Inserisci un messaggio
3. Visualizza la risposta in streaming
4. Osserva l'esecuzione degli strumenti in tempo reale

### Visualizzazione delle Chiamate Strumento

Quando l'AI chiama uno strumento:
```
🔧 Strumento: calendar
📥 Input: {"date": "2026-04-20"}
📤 Output: "Terzo giorno del quarto mese lunare"
```

---

## Configurazione

### Client AI

Configura il backend AI:
- Ollama (locale)
- Bailian (cloud)
- Volcengine Ark (cloud)
- Herdsman (locale/cloud, senza autenticazione)
- Meituan LongCat (cloud)
- Qiniu Cloud AI (cloud)
- Client personalizzato

### Impostazioni di Archiviazione

- Versione Default: percorso base, indice temporale, criteri di pulizia
- Versione Fast: configurazione del motore di archiviazione SpeedyPack, gestione file .spk, impostazioni compressione automatica

### Localizzazione

Passa tra 34 varianti linguistiche:
- Cinese (6): semplificato, tradizionale, Singapore, Macao, Taiwan, Malesia
- Inglese (10): americano, britannico, canadese, australiano, indiano, singaporese, sudafricano, irlandese, neozelandese, malese
- Spagnolo (2): Spagna, Messico
- Tedesco (5): Germania, Austria, Svizzera, Lussemburgo, Liechtenstein
- Francese (3): Francia, Canada, Svizzera
- Giapponese, coreano, ceco
- Russo, portoghese (2), italiano, olandese, polacco, svedese

---

## Sistema Skin

### Skin Disponibili

1. **Admin** - Interfaccia di amministrazione professionale
2. **Chat** - Design incentrato sulla conversazione
3. **Creative** - Stile creativo e artistico
4. **Dev** - Layout orientato allo sviluppatore
5. **HighContrast** - Tema ad alto contrasto (versione Fast)
6. **Minimal** - Stile minimalista (versione Fast)
7. **Light** - Tema luminoso (versione Fast)

### Cambio Skin

1. Fai clic su **Impostazioni** (icona ingranaggio)
2. Seleziona **Skin**
3. Scegli la skin desiderata
4. L'interfaccia si aggiorna immediatamente

### Skin Personalizzata

Crea una skin personalizzata implementando `ISkin`:

```csharp
public class MySkin : ISkin
{
    public string Name => "MySkin";
    
    public string GetCss()
    {
        return ":root { --primary: #color; }";
    }
}
```

---

## Gestione dei Permessi

### Visualizzazione dei Permessi

- Elenca tutte le regole di permesso
- Filtra per utente o risorsa
- Visualizza le date di scadenza

### Aggiunta di una Regola di Permesso

1. Fai clic su **Aggiungi regola**
2. Configura:
   - Tipo di permesso (es. `FileAccess`, `NetworkAccess`)
   - Prefisso risorsa (es. `C:\Projects`, `api.github.com`)
   - Consenti/Nega
   - Descrizione
3. Salva

### Traccia di Audit

Visualizza tutte le decisioni sui permessi:
- Timestamp
- Utente
- Risorsa
- Decisione
- Motivo

### Gestione Permessi Strumenti

Gestisci i permessi delle operazioni degli strumenti per Esseri di Silicio e progetti:

1. **Permessi strumenti Essere di Silicio**:
   - Naviga verso **Esseri** → seleziona un essere → **Permessi strumenti**
   - Visualizza la configurazione dei permessi correnti
   - Imposta consenti/nega per ogni operazione
   - Applica template di permessi (sola lettura/restrittivo/completo)

2. **Permessi strumenti progetto**:
   - Naviga verso **Progetti** → seleziona un progetto → **Permessi strumenti**
   - I permessi degli strumenti a livello di progetto sono indipendenti dal livello dell'Essere di Silicio
   - Implementa l'isolamento dei permessi tra progetti

---

## Gestione delle Attività

### Elenco delle Attività

- Tutte le attività e il loro stato
- Filtro per essere o stato
- Indicatori di priorità

### Dettagli dell'Attività

- Descrizione
- Priorità
- Data di scadenza
- Cronologia di esecuzione
- Output dei risultati

### Creazione di un'Attività

1. Fai clic su **Crea attività**
2. Compila:
   - Assegnazione essere
   - Descrizione
   - Priorità (1-10)
   - Data di scadenza
3. Crea

---

## Gestione dei Timer

### Timer Attivi

- Elenco dei timer in esecuzione
- Prossima esecuzione
- Stato di ripetizione

### Creazione di un Timer

1. Fai clic su **Crea timer**
2. Configura:
   - Assegnazione essere
   - Intervallo o espressione cron
   - Azione da eseguire
   - Impostazioni di ripetizione
3. Avvia

---

## Visualizzatore Log

### Funzionalità

- Filtro per livello (informazione/avviso/errore)
- Ricerca per parole chiave
- Selezione intervallo temporale
- Aggiornamenti in tempo reale

### Dettagli dei Log

Ogni voce di log mostra:
- Timestamp
- Livello
- Origine
- Messaggio
- Traccia dello stack (per gli errori)

---

## Report di Audit

### Utilizzo dei Token

- Token totali utilizzati
- Dettaglio per modello
- Calcolo dei costi
- Grafici basati sul tempo

### Esportazione dei Report

Scarica i dati di audit:
- Formato CSV
- Selezione intervallo di date
- Filtro per essere o modello

---

## Editor di Codice

### Funzionalità

- Evidenziazione della sintassi (Monaco Editor)
- Completamento del codice
- Tooltip sospesi per gli identificatori
- Compilazione in tempo reale

### Tooltip Sospesi

Passa il mouse su qualsiasi identificatore per visualizzare:
- Informazioni sul tipo
- Documentazione
- Posizione della definizione
- Riferimenti

---

## Visualizzazione Cronologia Chat

### Funzionalità

- Navigazione della cronologia chat degli Esseri di Silicio
- Visualizzazione dell'elenco delle sessioni
- Visualizzazione dei dettagli dei messaggi
- Vista timeline

### Utilizzo della Cronologia Chat

1. Naviga verso la pagina **Esseri**
2. Fai clic sul link **Cronologia chat** dell'Essere di Silicio
3. Visualizza l'elenco delle sessioni:
   - Titolo della sessione
   - Data di creazione
   - Numero di messaggi
4. Fai clic su una sessione per visualizzare i dettagli:
   - Cronologia completa dei messaggi
   - Timestamp
   - Informazioni sul mittente
   - Record delle chiamate strumento

### Implementazione Tecnica

- **Controller**: `ChatHistoryController`
- **Modello di vista**: `ChatHistoryViewModel`
- **Viste**:
  - `ChatHistoryListView` - Elenco delle sessioni
  - `ChatHistoryDetailView` - Dettagli dei messaggi
- **Route API**:
  - `/api/chat-history/{beingId}/conversations` - Ottieni l'elenco delle sessioni
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Ottieni i dettagli dei messaggi

---

## Caricamento File

### Funzionalità

- Dialogo della sorgente file
- Supporto per caricamento multi-file
- Gestione dei metadati dei file
- Visualizzazione dell'avanzamento del caricamento

### Utilizzo del Caricamento File

1. Fai clic sul pulsante **Carica file** nell'interfaccia di chat
2. Si apre il dialogo della sorgente file
3. Seleziona la sorgente file:
   - File locale
   - Percorso del file system
4. Seleziona i file (supporto selezione multipla)
5. Conferma il caricamento
6. Le informazioni sul file verranno allegate al messaggio

### Tipi di File Supportati

- File di testo (.txt, .md, .json, .xml, ecc.)
- File di codice (.cs, .js, .py, .java, ecc.)
- File di configurazione (.yml, .yaml, .ini, .conf, ecc.)
- File di documento (.csv, .log, ecc.)

---

## Indicatore di Caricamento

### Funzionalità

- Visualizzazione dello stato di caricamento della pagina chat
- Selezione automatica della sessione del Curatore
- Feedback sull'avanzamento del caricamento dei dati

### Comportamento

- Mostra un'animazione di caricamento durante il caricamento della pagina
- Si nasconde automaticamente al completamento del caricamento dei dati
- La sessione del Curatore viene selezionata automaticamente (se esiste)
- Testo di suggerimento di caricamento multilingua

---

## Sistema di Documentazione di Aiuto (Nuovo)

### Panoramica delle Funzionalità

Il sistema di documentazione di aiuto fornisce supporto per la documentazione multilingua agli Esseri di Silicio e agli utenti.

### Utilizzo della Documentazione di Aiuto

1. Naviga verso la pagina **Aiuto**
2. Visualizza l'elenco degli argomenti di aiuto:
   - Guida rapida
   - Riferimento per l'utilizzo degli strumenti
   - Guida alla gestione dei permessi
   - Manuale di risoluzione dei problemi
   - Guida allo sviluppo
3. Fai clic su un argomento per visualizzare il contenuto dettagliato:
   - Contenuto documentale strutturato (rendering Markdown)
   - Supporto multilingua (segue le impostazioni di localizzazione del sistema)
   - Raccomandazione di argomenti correlati
4. Utilizza la funzione di ricerca per individuare rapidamente:
   - Ricerca per parole chiave (supporto cinese, inglese)
   - Risultati di ricerca ordinati per rilevanza

### Accesso all'Aiuto da parte degli Esseri di Silicio

Gli Esseri di Silicio possono accedere alla documentazione di aiuto tramite lo strumento `help`:
```json
{
  "action": "get_topics"
}
```

### Implementazione Tecnica

- **Controller**: `HelpController`
- **Strumento**: `HelpTool`
- **Route API**:
  - `/api/help` - Ottieni l'elenco degli argomenti di aiuto
  - `/api/help/{topicId}` - Ottieni i dettagli di un argomento
  - `/api/help/search?q=keyword` - Cerca nella documentazione di aiuto

---

## Area di Lavoro di Progetto (Nuovo)

### Panoramica delle Funzionalità

L'area di lavoro di progetto fornisce un ambiente di lavoro strutturato che supporta la gestione dei progetti, il tracciamento delle attività e le note di lavoro.

### Gestione dei Progetti

1. **Creazione di un progetto**:
   - Nome e descrizione del progetto
   - Tag del progetto (categorizzazione)
   - Stato del progetto (in corso, completato, archiviato)
2. **Visualizzazione dei dettagli del progetto**:
   - Informazioni di base del progetto
   - Elenco delle attività associate
   - Elenco delle note di lavoro
   - Statistiche sull'avanzamento del progetto
3. **Archiviazione del progetto**: Mantiene i dati storici ma non è più attivo
4. **Gestione dei ruoli di progetto**:
   - Assegnazione di ruoli di progetto agli Esseri di Silicio (es. developer, reviewer, manager)
   - Rimozione delle assegnazioni dei ruoli
   - Visualizzazione dei membri del progetto e dell'elenco dei ruoli
5. **Workflow di progetto**:
   - Visualizzazione dell'elenco dei template di workflow
   - Associazione di un template di workflow al progetto
   - Visualizzazione dello stato dell'istanza del workflow
   - Visualizzazione dei log di esecuzione del workflow

### Note di Lavoro (Private)

Note di lavoro personali degli Esseri di Silicio, simili a un diario:

1. **Creazione di una nota**:
   - Riepilogo (breve descrizione)
   - Contenuto (supporta formato Markdown)
   - Parole chiave (per la ricerca)
   - Registrazione automatica del timestamp
2. **Gestione delle note**:
   - Navigazione per timeline (design a pagine)
   - Ricerca delle note (per parole chiave, riepilogo, contenuto)
   - Generazione dell'indice (visualizzazione rapida della struttura delle note)
   - Aggiornamento ed eliminazione delle note
3. **Controllo dei permessi**:
   - Private per impostazione predefinita, accessibili solo all'essere stesso
   - Il Curatore di Silicio può gestire tutte le note

### Implementazione Tecnica

- **Controller**: `WorkNoteController`
- **Strumenti**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **Route API**:
  - `/api/worknotes` - Ottieni l'elenco delle note di lavoro
  - `/api/worknotes/{id}` - Ottieni i dettagli di una nota
  - `/api/worknotes/search?q=keyword` - Cerca nelle note
  - `/api/worknotes/directory` - Genera l'indice delle note
  - `/api/projects` - API di gestione dei progetti

---

## Design Responsivo

La Web UI si adatta a diverse dimensioni dello schermo:
- Desktop: Layout completo
- Tablet: Barra laterale compressa
- Mobile: Menu comprimibile

---

## Scelte Rapide da Tastiera

| Scelta rapida | Azione |
|----------|--------|
| `Ctrl+K` | Ricerca rapida |
| `Ctrl+B` | Attiva/disattiva la barra laterale |
| `Ctrl+Enter` | Invia messaggio |
| `Esc` | Annulla/chiudi |

---

## Risoluzione dei Problemi

### Impossibile Connettersi

**Verifica**:
- Il server è in esecuzione
- La porta 8080 non è bloccata
- Impostazioni del firewall

### SSE Non Funziona

**Verifica**:
- Il browser supporta SSE
- Nessun proxy sta bufferizzando SSE
- Stabilità della rete

### Prestazioni Lente

**Ottimizzazione**:
- Riduci il livello di dettaglio dei log
- Pulisci i vecchi dati di audit
- Verifica le risorse di sistema

---

## Prossimi Passi

- 📚 Leggi la [Guida all'Architettura](architecture.md)
- 🛠️ Consulta la [Guida allo Sviluppo](development-guide.md)
- 📖 Esplora il [Riferimento API](api-reference.md)
- 🚀 Consulta la [Guida Rapida](getting-started.md)
