# Guida all'interfaccia Web

> **Versione: v0.1.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [Français](../fr-FR/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md) | **Italiano**

## Panoramica

L'interfaccia Web fornisce un'interfaccia completa per gestire i Silicon Beings, monitorare lo stato del sistema e interagire con gli agenti IA. Il sistema utilizza un'architettura di rendering puramente lato server senza dipendenze da framework frontend, generando HTML, CSS e JavaScript tramite i builder `H`, `CssBuilder` e `JsBuilder`.

## Accesso

URL predefinito: `http://localhost:8080`

## Navigazione

### Sezioni principali

1. **Dashboard** - Panoramica sistema e metriche
2. **Beings** - Gestione dei Silicon Beings
3. **Chat** - Interazione con i Beings (con upload file, SSE in diretta)
4. **Cronologia chat** - Visualizzare cronologia chat dei Silicon Beings (elenco sessioni, dettagli messaggi)
5. **Attività** - Gestione attività (attività personali)
6. **Timer** - Configurazione timer (creare, pausa, cronologia esecuzione)
7. **Configurazione** - Impostazioni sistema (client IA, localizzazione)
8. **Permessi** - Controllo accessi (gestione ACL, richieste permessi)
9. **Log** - Log sistema (filtrare per livello, query per intervallo temporale)
10. **Audit** - Utilizzo token e traccia audit
11. **Memoria** - Memoria Beings (vista cronologica, filtraggio avanzato)
12. **Conoscenze** - Base conoscenza (gestione triplette, ricerca percorsi)
13. **Browser codice** - Esplorazione codice (albero file, colorazione sintassi)
14. **Editor codice** - Modifica codice con suggerimenti al passaggio (Monaco Editor)
15. **Progetti** - Gestione progetti (spazi lavoro, attività, note di lavoro)
16. **Esecutori** - Gestione esecutori (disco, rete, riga comando)
17. **Aiuto** - Sistema documentazione aiuto (supporto multilingue, ricerca per argomento)
18. **Informazioni** - Informazioni sistema e versione

---

## Dashboard

### Funzionalità

- Metriche prestazioni sistema (CPU, memoria, tempo funzionamento)
- Panoramica stato Beings
- Statistiche utilizzo IA
- Azioni rapide

### Aggiornamenti in tempo reale

Utilizza SSE (Server-Sent Events) per i dati in tempo reale:

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Gestione Beings

### Elenco Beings

Mostra tutti i Beings con:
- Nome e ID
- Stato corrente (In esecuzione/Arrestato/Errore)
- Collegamento al file dell'anima
- Azioni rapide (Avvia/Ferma/Configura)

### Dettagli Being

- Configurazione completa
- Editor file dell'anima
- Cronologia attività
- Visualizzatore memoria
- Metriche prestazioni

### Creare un Being

1. Cliccare su **Crea nuovo Being**
2. Compilare:
   - Nome
   - Contenuto anima (editor Markdown)
   - Configurazione iniziale
3. Cliccare su **Crea**

---

## Interfaccia chat

### Funzionalità

- Flusso messaggi in tempo reale
- Cronologia messaggi
- Supporto multi-sessione
- Visualizzazione chiamate strumento

### Usare la chat

1. Selezionare un Being
2. Scrivere un messaggio
3. Vedere la risposta in streaming
4. Osservare l'esecuzione strumenti in tempo reale

### Visualizzazione chiamate strumento

Quando l'IA chiama uno strumento:
```
🔧 Strumento: calendar
📥 Input: {"date": "2026-04-20"}
📤 Output: "农历四月初三"
```

---

## Configurazione

### Client IA

Configurare il backend IA:
- Ollama (locale)
- DashScope (cloud)
- Volcengine Ark (cloud)
- Client personalizzati

### Impostazioni storage

- Versione Default: Percorso base, indice temporale, strategia pulizia
- Versione Fast: Configurazione motore storage SpeedyPack, gestione file .spk, impostazioni auto-compattazione

### Localizzazione

Passare tra 30 varianti linguistiche:
- Cinese (6): Semplificato, Tradizionale, Singapore, Macao, Taiwan, Malesia
- Inglese (10): US, UK, Canadese, Australiano, Indiano, Singapore, Sudafricano, Irlandese, Neozelandese, Malesiano
- Tedesco (5): Germania, Austria, Svizzera, Lussemburgo, Liechtenstein
- Francese (3): Francia, Canada, Svizzera
- Italiano (1): Italia
- Giapponese, Coreano, Ceco

---

## Sistema di skin

### Skin disponibili

1. **Admin** - Interfaccia amministrazione professionale
2. **Chat** - Design centrato sulla conversazione
3. **Creative** - Stile creativo e artistico
4. **Dev** - Layout orientato sviluppatore
5. **HighContrast** - Alto contrasto (versione Fast)
6. **Minimal** - Stile minimalista (versione Fast)
7. **Light** - Tema chiaro (versione Fast)

### Cambiare skin

1. Cliccare su **Impostazioni** (icona ingranaggio)
2. Selezionare **Skin**
3. Scegliere lo skin desiderato
4. L'interfaccia si aggiorna immediatamente

### Skin personalizzato

Creare skin personalizzati implementando `ISkin`:

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

## Gestione permessi

### Visualizzare permessi

- Elencare tutte le regole permessi
- Filtrare per utente o risorsa
- Visualizzare data scadenza

### Aggiungere regola permesso

1. Cliccare su **Aggiungi regola**
2. Configurare:
   - Utente
   - Risorsa (es. `disk:read`)
   - Consenti/Nega
   - Durata
3. Salvare

### Traccia audit

Visualizzare tutte le decisioni permessi:
- Timestamp
- Utente
- Risorsa
- Decisione
- Motivo

---

## Gestione attività

### Elenco attività

- Tutte le attività con stato
- Filtrare per Being o stato
- Indicatori priorità

### Dettagli attività

- Descrizione
- Priorità
- Data scadenza
- Cronologia esecuzione
- Risultato output

### Creare attività

1. Cliccare su **Crea attività**
2. Compilare:
   - Assegnazione Being
   - Descrizione
   - Priorità (1-10)
   - Data scadenza
3. Creare

---

## Gestione timer

### Timer attivi

- Elenco timer in corso
- Prossima ora esecuzione
- Stato ripetizione

### Creare timer

1. Cliccare su **Crea timer**
2. Configurare:
   - Assegnazione Being
   - Intervallo o espressione Cron
   - Azione da eseguire
   - Parametri ripetizione
3. Avviare

---

## Visualizzatore log

### Funzionalità

- Filtrare per livello (Info/Avviso/Errore)
- Cercare per parola chiave
- Selezione intervallo temporale
- Aggiornamenti in tempo reale

### Dettagli log

Ogni voce di log mostra:
- Timestamp
- Livello
- Sorgente
- Messaggio
- Traccia stack (per errori)

---

## Report audit

### Utilizzo token

- Totale token utilizzati
- Ripartizione per modello
- Calcolo costi
- Grafici temporali

### Esportare report

Scaricare dati audit:
- Formato CSV
- Selezione intervallo date
- Filtrare per Being o modello

---

## Editor codice

### Funzionalità

- Colorazione sintassi (Monaco Editor)
- Completamento codice
- Suggerimenti al passaggio per identificatori
- Compilazione in tempo reale

### Suggerimenti al passaggio

Passare il mouse su un identificatore per:
- Informazioni tipo
- Documentazione
- Posizione definizione
- Riferimenti

---

## Visualizzazione cronologia chat

### Funzionalità

- Sfogliare cronologia chat dei Silicon Beings
- Visualizzazione elenco sessioni
- Vedere dettagli messaggi
- Vista cronologica

### Usare cronologia chat

1. Navigare verso la pagina **Beings**
2. Cliccare sul link **Cronologia chat** del Silicon Being
3. Visualizzare l'elenco sessioni:
   - Titolo sessione
   - Ora creazione
   - Numero messaggi
4. Cliccare su una sessione per i dettagli:
   - Cronologia completa messaggi
   - Timestamp
   - Informazioni mittente
   - Registro chiamate strumento

### Implementazione tecnica

- **Controller**: `ChatHistoryController`
- **ViewModel**: `ChatHistoryViewModel`
- **Viste**:
  - `ChatHistoryListView` - Elenco sessioni
  - `ChatHistoryDetailView` - Dettagli messaggi
- **Route API**:
  - `/api/chat-history/{beingId}/conversations` - Ottenere elenco sessioni
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Ottenere dettagli messaggi

---

## Upload file

### Funzionalità

- Dialogo selezione file
- Supporto upload multi-file
- Gestione metadati file
- Indicatore progressione upload

### Usare upload file

1. Cliccare sul pulsante **Upload file** nella chat
2. Il dialogo selezione file si apre
3. Scegliere la sorgente del file:
   - File locale
   - Percorso file system
4. Selezionare i file (selezione multipla supportata)
5. Confermare l'upload
6. Le informazioni del file vengono allegate al messaggio

### Tipi file supportati

- File testo (.txt, .md, .json, .xml, ecc.)
- File codice (.cs, .js, .py, .java, ecc.)
- File configurazione (.yml, .yaml, .ini, .conf, ecc.)
- File documenti (.csv, .log, ecc.)

---

## Indicatore caricamento

### Funzionalità

- Visualizzazione stato caricamento sulla pagina chat
- Selezione automatica sessione Curator
- Feedback progressione caricamento dati

### Comportamento

- Animazione caricamento al caricamento pagina
- Nascosto automaticamente dopo caricamento dati
- Sessione Curator automaticamente selezionata (se presente)
- Testi caricamento multilingue

---

## Sistema documentazione aiuto (Nuovo)

### Panoramica funzionalità

Il sistema di documentazione aiuto fornisce supporto aiuto multilingue per i Silicon Beings e gli utenti.

### Usare l'aiuto

1. Navigare verso la pagina **Aiuto**
2. Visualizzare l'elenco argomenti aiuto:
   - Guida avvio rapido
   - Riferimento utilizzo strumenti
   - Guida gestione permessi
   - Guida risoluzione problemi
   - Guida sviluppo
3. Cliccare su un argomento per i dettagli:
   - Contenuto documentazione strutturato (rendering Markdown)
   - Supporto multilingue (segue localizzazione sistema)
   - Raccomandazioni argomenti correlati
4. Funzione ricerca per localizzazione rapida:
   - Ricerca per parola chiave (supporta cinese, inglese)
   - Risultati ricerca ordinati per pertinenza

### Aiuto per i Silicon Beings

I Silicon Beings possono accedere all'aiuto tramite lo strumento `help`:
```json
{
  "action": "get_topics"
}
```

### Implementazione tecnica

- **Controller**: `HelpController`
- **Strumento**: `HelpTool`
- **Route API**:
  - `/api/help` - Ottenere elenco argomenti aiuto
  - `/api/help/{topicId}` - Ottenere dettagli argomento
  - `/api/help/search?q=keyword` - Cercare nella documentazione aiuto

---

## Spazio progetto (Nuovo)

### Panoramica funzionalità

Lo spazio progetto fornisce un ambiente di lavoro strutturato con gestione progetti, monitoraggio attività e note di lavoro.

### Gestione progetti

1. **Creare progetto**:
   - Nome e descrizione progetto
   - Tag progetto (categorizzazione)
   - Stato progetto (Attivo, Completato, Archiviato)
2. **Visualizzare dettagli progetto**:
   - Informazioni base progetto
   - Elenco attività associate
   - Elenco note di lavoro
   - Statistiche progressione progetto
3. **Archiviare progetto**: Conservare dati storici ma non essere più attivo

### Note di lavoro (Privato)

Note di lavoro personali dei Silicon Beings, simili a un diario:

1. **Creare nota**:
   - Riepilogo (descrizione breve)
   - Contenuto (supporto Markdown)
   - Parole chiave (per ricerca)
   - Timestamp automatico
2. **Gestire note**:
   - Navigazione cronologica (design a pagine)
   - Cercare note (per parola chiave, riepilogo, contenuto)
   - Generare indice (visualizzazione rapida struttura)
   - Aggiornare e cancellare note
3. **Controllo permessi**:
   - Privato per impostazione predefinita, accessibile solo al Being stesso
   - Il Silicon Curator può gestire tutte le note

### Implementazione tecnica

- **Controller**: `WorkNoteController`
- **Strumenti**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **Route API**:
  - `/api/worknotes` - Ottenere elenco note lavoro
  - `/api/worknotes/{id}` - Ottenere dettagli nota
  - `/api/worknotes/search?q=keyword` - Cercare note
  - `/api/worknotes/directory` - Generare indice note
  - `/api/projects` - API gestione progetti

---

## Design responsive

L'interfaccia Web si adatta a diverse dimensioni schermo:
- Desktop: Layout completo
- Tablet: Barra laterale compressa
- Mobile: Menu richiudibile

---

## Scorciatoie tastiera

| Scorciatoia | Azione |
|-----------|--------|
| `Ctrl+K` | Ricerca rapida |
| `Ctrl+B` | Attiva/disattiva barra laterale |
| `Ctrl+Invio` | Invia messaggio |
| `Esc` | Annulla/Chiudi |

---

## Risoluzione problemi

### Impossibile connettersi

**Verificare**:
- Il server è in esecuzione
- La porta 8080 non è bloccata
- Le impostazioni firewall

### SSE non funziona

**Verificare**:
- Il browser supporta SSE
- Nessun proxy bufferizza SSE
- Stabilità rete

### Prestazioni lente

**Ottimizzare**:
- Ridurre livello dettaglio log
- Pulire vecchi dati audit
- Verificare risorse sistema

---

## Prossimi passi

- 📚 Leggere la [guida architettura](architecture.md)
- 🛠️ Consultare la [guida sviluppo](development-guide.md)
- 📖 Esplorare il [riferimento API](api-reference.md)
- 🚀 Vedere la [guida avvio rapido](getting-started.md)
