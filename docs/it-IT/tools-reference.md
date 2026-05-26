# Riferimento degli Strumenti

> **Versione: v0.2.0-alpha**

Questo documento descrive in dettaglio tutti gli strumenti integrati della piattaforma Silicon Life Collective.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Русский](../ru-RU/tools-reference.md) | **Italiano**

## Panoramica

Il sistema degli strumenti permette agli Esseri di Silicio di interagire con il mondo esterno attraverso interfacce standardizzate. Ogni strumento implementa l'interfaccia `ITool`, e viene automaticamente scoperto e registrato da `ToolManager` tramite reflection.

### Categorie di Strumenti

- **Strumenti di gestione di sistema** — Configurazione, permessi, compilazione dinamica, gestione del Curatore
- **Strumenti di comunicazione** — Chat, richieste di rete
- **Strumenti di archiviazione dati** — Operazioni su disco, database, memoria, note di lavoro
- **Strumenti di gestione del tempo** — Calendario, timer, attività
- **Strumenti di sviluppo** — Esecuzione codice, query log
- **Strumenti di utilità** — Informazioni di sistema, audit token, documentazione di aiuto, rete di conoscenza
- **Strumenti browser** — Automazione browser WebView
- **Strumenti di progetto** — Gestione progetti, attività di progetto, note di lavoro di progetto, lavoro di progetto
- **Strumenti plugin** — Strumenti di terze parti registrati tramite il sistema di plugin

### Sistema degli Scenari degli Strumenti

Ogni strumento dichiara i propri scenari di utilizzo tramite l'attributo `[ToolScenario]`:

| Flag scenario | Valore | Descrizione |
|----------|------|-------------|
| `Chat` | `1 << 0` | Scenario chat (quando l'utente conversa con un Essere di Silicio) |
| `Task` | `1 << 1` | Scenario attività (quando l'Essere di Silicio esegue un'attività) |
| `Timer` | `1 << 2` | Scenario timer (quando l'Essere di Silicio esegue un'attività pianificata) |
| `MemoryCompression` | `1 << 3` | Scenario compressione della memoria |
| `Project` | `1 << 4` | Scenario progetto (modalità ThinkOnProject) |
| `All` | Tutti i precedenti | Disponibile in tutti gli scenari |

Inoltre, gli strumenti contrassegnati con l'attributo `[ChatOnly]` sono disponibili solo nello scenario chat (es. HelpTool), e non compaiono negli scenari attività e timer.

---

## Elenco degli Strumenti Integrati

### 1. Strumento Calendario (CalendarTool)

**Nome strumento**: `calendar`

**Descrizione**: Supporta la conversione e il calcolo delle date per 32 sistemi calendariali.

**Operazioni supportate**:
- `now` — Ottieni l'ora corrente
- `format` — Formatta una data
- `add_days` — Aggiungi/sottrai giorni
- `diff` — Calcola la differenza tra date
- `list_calendars` — Elenca tutti i calendari supportati
- `get_components` — Ottieni i componenti della data
- `get_now_components` — Ottieni i componenti dell'ora corrente
- `convert` — Converti tra sistemi calendariali

**Sistemi calendariali supportati** (32):
- Gregoriano (Gregorian)
- Lunare cinese (Chinese Lunar)
- Storico cinese (Chinese Historical) — Cronologia Ganzhi, ere imperiali
- Islamico (Islamic)
- Ebraico (Hebrew)
- Giapponese (Japanese)
- Persiano (Persian)
- Maya (Mayan)
- Buddhista (Buddhist)
- Tibetano (Tibetan)
- E altri 24 calendari...

**Esempio di utilizzo**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. Strumento Chat (ChatTool)

**Nome strumento**: `chat`

**Descrizione**: Gestione delle sessioni di chat e dell'invio di messaggi.

**Operazioni supportate**:
- `send_message` — Invia un messaggio
- `get_messages` — Ottieni messaggi cronologici
- `create_group` — Crea una chat di gruppo
- `add_member` — Aggiungi un membro al gruppo
- `remove_member` — Rimuovi un membro dal gruppo
- `get_chat_info` — Ottieni informazioni sulla chat
- `terminate_chat` — Termina la chat (letto senza risposta)

**Esempio di utilizzo**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "Ciao, collaboriamo!"
}
```

---

### 3. Strumento Configurazione (ConfigTool)

**Nome strumento**: `config`

**Descrizione**: Lettura e modifica della configurazione di sistema.

**Operazioni supportate**:
- `read` — Leggi un elemento di configurazione
- `write` — Scrivi un elemento di configurazione
- `list` — Elenca tutte le configurazioni
- `get_ai_config` — Ottieni la configurazione del client AI
- `set_ai_config` — Imposta la configurazione del client AI

**Esempio di utilizzo**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Strumento Curatore (CuratorTool) 🔒

**Nome strumento**: `silicon_manager`

**Requisito di permesso**: Solo per il Curatore di Silicio (`[SiliconManagerOnly]`)

**Scenari disponibili**: Chat, Attività, Timer

**Descrizione**: Strumento di gestione di sistema dedicato al Curatore di Silicio, per la gestione della creazione, visualizzazione e ripristino degli Esseri di Silicio.

**Operazioni supportate**:
- `list_beings` — Elenca tutti gli Esseri di Silicio e il loro stato
- `create_being` — Crea un nuovo Essere di Silicio (richiede i parametri `name` e `soul`)
- `get_code` — Visualizza il codice sorgente personalizzato di un Essere di Silicio
- `reset` — Ripristina un Essere di Silicio all'implementazione predefinita

**Esempio di utilizzo**:
```json
{
  "action": "create_being",
  "name": "Assistente",
  "soul": "Sei un assistente utile..."
}
```

---

### 5. Strumento Database (DatabaseTool)

**Nome strumento**: `database`

**Descrizione**: Query e operazioni su database strutturato.

**Operazioni supportate**:
- `query` — Interroga dati
- `insert` — Inserisci dati
- `update` — Aggiorna dati
- `delete` — Elimina dati
- `create_table` — Crea tabella
- `list_tables` — Elenca tutte le tabelle

**Esempio di utilizzo**:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. Strumento Disco (DiskTool)

**Nome strumento**: `disk`

**Descrizione**: Operazioni sul file system e ricerca locale.

**Operazioni supportate**:
- `read` — Leggi file
- `write` — Scrivi file
- `list` — Elenca directory
- `delete` — Elimina file
- `create_directory` — Crea directory
- `search_files` — Cerca file
- `search_content` — Cerca contenuto dei file
- `count_lines` — Conta le righe
- `read_lines` — Leggi righe specifiche
- `replace_text` — Sostituisci testo

**Requisito di permesso**: `FileAccess`

**Esempio di utilizzo**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Strumento Compilazione Dinamica (DynamicCompileTool) 🔒

**Nome strumento**: `compile`

**Descrizione**: Compilazione dinamica di codice C# (per l'auto-evoluzione degli Esseri di Silicio).

**Operazioni supportate**:
- `compile_class` — Compila una classe
- `compile_callback` — Compila una funzione di callback dei permessi
- `validate_code` — Verifica la sicurezza del codice

**Meccanismi di sicurezza**:
- Controllo dei riferimenti in fase di compilazione (esclusione assembly pericolosi)
- Analisi statica del codice a runtime
- Archiviazione crittografata AES-256

**Esempio di utilizzo**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Strumento Esecuzione Codice (ExecuteCodeTool) 🔒

**Nome strumento**: `execute_code`

**Requisito di permesso**: Solo per il Curatore di Silicio

**Descrizione**: Compila ed esegue frammenti di codice C#.

**Operazioni supportate**:
- `run_script` — Esegui uno script di codice

**Esempio di utilizzo**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. Strumento Aiuto (HelpTool)

**Nome strumento**: `help`

**Scenari disponibili**: Chat (`[ChatOnly]`, disponibile solo nello scenario chat)

**Descrizione**: Cerca e ottieni contenuti della documentazione di aiuto del sistema, permette all'AI di interrogare i metodi di utilizzo delle funzionalità di sistema.

**Operazioni supportate**:
- `list` — Elenca tutti gli ID degli argomenti di aiuto
- `search` — Cerca nella documentazione di aiuto per parole chiave
- `get` — Ottieni il contenuto della documentazione di aiuto per un ID specificato

**Esempio di utilizzo**:
```json
{
  "action": "search",
  "keyword": "permessi"
}
```

---

### 10. Strumento Rete di Conoscenza (KnowledgeTool)

**Nome strumento**: `knowledge`

**Descrizione**: Operazioni sul grafo di conoscenza (basato su triple: soggetto-relazione-oggetto).

**Operazioni supportate**:
- `add` — Aggiungi una tripla di conoscenza
- `query` — Interroga la conoscenza
- `update` — Aggiorna la conoscenza
- `delete` — Elimina la conoscenza
- `search` — Cerca nella conoscenza
- `get_path` — Ottieni il percorso della conoscenza
- `validate` — Verifica la conoscenza
- `stats` — Ottieni statistiche

**Esempio di utilizzo**:
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95
}
```

---

### 11. Strumento Log (LogTool)

**Nome strumento**: `log`

**Descrizione**: Query della cronologia delle operazioni e delle conversazioni.

**Operazioni supportate**:
- `query_logs` — Interroga i log di sistema
- `query_conversations` — Interroga la cronologia delle conversazioni
- `get_stats` — Ottieni statistiche dei log

**Esempio di utilizzo**:
```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-26T23:59:59Z",
  "level": "info"
}
```

---

### 12. Strumento Memoria (MemoryTool)

**Nome strumento**: `memory`

**Descrizione**: Gestione della memoria a lungo e breve termine degli Esseri di Silicio.

**Operazioni supportate**:
- `read` — Leggi la memoria
- `write` — Scrivi nella memoria
- `search` — Cerca nella memoria
- `delete` — Elimina la memoria
- `list` — Elenca la memoria
- `get_stats` — Ottieni statistiche della memoria
- `compress` — Comprimi la memoria

**Esempio di utilizzo**:
```json
{
  "action": "read",
  "key": "important_fact",
  "time_range": {
    "start": "2026-04-01",
    "end": "2026-04-26"
  }
}
```

---

### 13. Strumento Rete (NetworkTool)

**Nome strumento**: `network`

**Descrizione**: Esecuzione di richieste HTTP/HTTPS.

**Operazioni supportate**:
- `get` — Richiesta GET
- `post` — Richiesta POST
- `put` — Richiesta PUT
- `delete` — Richiesta DELETE
- `download` — Scarica file
- `upload` — Carica file

**Requisito di permesso**: `network:http`

**Esempio di utilizzo**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. Strumento Permessi (PermissionTool) 🔒

**Nome strumento**: `permission`

**Requisito di permesso**: Solo per il Curatore di Silicio

**Descrizione**: Gestione dei permessi e delle liste di controllo degli accessi.

**Operazioni supportate**:
- `query_permission` — Interroga i permessi
- `manage_acl` — Gestisci la ACL globale
- `get_callback` — Ottieni la funzione di callback dei permessi
- `set_callback` — Imposta la funzione di callback dei permessi

**Esempio di utilizzo**:
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow"
}
```

---

### 15. Strumento Progetto (ProjectTool) 🔒

**Nome strumento**: `project`

**Requisito di permesso**: Solo per il Curatore di Silicio (`[SiliconManagerOnly]`)

**Scenari disponibili**: Chat, Attività, Timer

**Descrizione**: Gestione dell'area di lavoro di progetto, supporta la gestione del ciclo di vita del progetto, l'assegnazione dei membri e la gestione dei ruoli.

**Operazioni supportate**:
- `create` — Crea un nuovo spazio di progetto
- `archive` — Archivia un progetto
- `restore` — Ripristina un progetto archiviato
- `destroy` — Distruggi un progetto e pulisci i dati (irreversibile)
- `list` — Elenca tutti i progetti
- `get` — Ottieni i dettagli del progetto
- `assign` — Assegna un Essere di Silicio al progetto
- `remove` — Rimuovi un Essere di Silicio dal progetto
- `update` — Aggiorna il nome/descrizione del progetto
- `list-workflow-templates` — Elenca i template di workflow disponibili
- `assign_role` — Assegna un ruolo di progetto a un Essere di Silicio
- `remove_role` — Rimuovi il ruolo di progetto di un Essere di Silicio
- `list_roles` — Elenca le assegnazioni dei ruoli del progetto

**Esempio di utilizzo**:
```json
{
  "action": "create",
  "name": "Il mio Progetto",
  "description": "Descrizione del progetto"
}
```

---

### 16. Strumento Attività di Progetto (ProjectTaskTool)

**Nome strumento**: `project_task`

**Scenari disponibili**: Chat, Attività, Timer

**Descrizione**: Gestione delle attività nello spazio di progetto, supporta il ciclo di vita completo delle attività.

**Operazioni supportate**:
- `create` — Crea un'attività di progetto
- `list` — Elenca le attività di progetto
- `get` — Ottieni i dettagli dell'attività
- `update` — Aggiorna titolo/descrizione/priorità dell'attività
- `assign` — Assegna un responsabile all'attività
- `remove_assignee` — Rimuovi il responsabile dell'attività
- `start` — Inizia l'attività
- `complete` — Contrassegna l'attività come completata
- `fail` — Contrassegna l'attività come fallita
- `cancel` — Annulla l'attività
- `delete` — Elimina l'attività
- `stats` — Ottieni statistiche delle attività

**Esempio di utilizzo**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Descrizione del completamento dell'attività",
  "priority": 5
}
```

---

### 17. Strumento Note di Lavoro di Progetto (ProjectWorkNoteTool)

**Nome strumento**: `project_work_note`

**Scenari disponibili**: Chat, Attività, Timer

**Descrizione**: Gestione delle note di lavoro nello spazio di progetto (pubbliche, simili a un quaderno di lavoro), supporta la gestione di note a pagine.

**Operazioni supportate**:
- `create` — Crea una pagina di note (richiede `project_id`, `summary` e `content`, opzionale `keywords`)
- `read` — Leggi una pagina di note (richiede `project_id` e `page_number` o `note_id`)
- `update` — Aggiorna una pagina di note (richiede `project_id`, `page_number` e `content`, opzionali `summary` e `keywords`)
- `delete` — Elimina una pagina di note (richiede `project_id` e `page_number` o `note_id`)
- `list` — Elenca i riepiloghi di tutte le pagine di note del progetto
- `directory` — Genera un indice/panoramica delle note
- `search` — Cerca note per parole chiave (richiede `project_id` e `keyword`, opzionale `max_results`)

**Esempio di utilizzo**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Completamento modulo autenticazione utente",
  "content": "## Dettagli di implementazione\n\n- Utilizzo JWT token",
  "keywords": "autenticazione,JWT"
}
```

---

### 18. Strumento Lavoro di Progetto (ProjectWorkTool) 🔒

**Nome strumento**: `project_work`

**Requisito di permesso**: Solo per il Curatore di Silicio (`[SiliconManagerOnly]`)

**Scenari disponibili**: Progetto (`[ToolScenario(ToolScenarioFlag.Project)]`, disponibile solo nello scenario progetto)

**Descrizione**: Strumento per le operazioni di lavoro di progetto, utilizzato dal Curatore per gestire i flussi di lavoro di progetto nello scenario ThinkOnProject.

**Operazioni supportate**:
- `create-task` — Crea un'attività di progetto
- `assign-task` — Assegna un Essere di Silicio all'attività
- `chat` — Invia un messaggio alla chat di gruppo del progetto
- `broadcast` — Trasmetti un messaggio al canale del progetto
- `complete` — Contrassegna il progetto come completato
- `status` — Ottieni lo stato del progetto

**Esempio di utilizzo**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "Implementare l'autenticazione utente"
}
```

---

### 19. Strumento Sistema (SystemTool)

**Nome strumento**: `system`

**Descrizione**: Ottieni informazioni di sistema e utilizzo delle risorse.

**Operazioni supportate**:
- `info` — Ottieni informazioni di sistema
- `resource_usage` — Ottieni l'utilizzo delle risorse
- `find_process` — Trova un processo
- `list_beings` — Elenca gli Esseri di Silicio

**Esempio di utilizzo**:
```json
{
  "action": "info"
}
```

---

### 20. Strumento Attività (TaskTool)

**Nome strumento**: `task`

**Descrizione**: Gestione delle attività personali degli Esseri di Silicio.

**Operazioni supportate**:
- `create` — Crea un'attività
- `list` — Elenca le attività
- `update` — Aggiorna un'attività
- `complete` — Completa un'attività
- `delete` — Elimina un'attività
- `get_dependencies` — Ottieni le dipendenze

**Esempio di utilizzo**:
```json
{
  "action": "create",
  "description": "Revisionare il codice",
  "priority": 5
}
```

---

### 21. Strumento Timer (TimerTool)

**Nome strumento**: `timer`

**Descrizione**: Creazione e gestione dei timer.

**Operazioni supportate**:
- `create` — Crea un timer
- `list` — Elenca i timer
- `delete` — Elimina un timer
- `pause` — Metti in pausa un timer
- `resume` — Riprendi un timer
- `get_execution_history` — Ottieni la cronologia di esecuzione

**Esempio di utilizzo**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "Promemoria orario"
}
```

---

### 22. Strumento Audit Token (TokenAuditTool) 🔒

**Nome strumento**: `token_audit`

**Requisito di permesso**: Solo per il Curatore di Silicio (`[SiliconManagerOnly]`)

**Scenari disponibili**: Chat, Attività, Timer

**Descrizione**: Query delle statistiche e dei dati di tendenza sull'utilizzo dei token AI.

**Operazioni supportate**:
- `summary` — Ottieni statistiche riepilogative sull'utilizzo dei token
- `trend` — Ottieni i punti dati di tendenza dell'utilizzo dei token

**Intervalli temporali supportati**:
- `today` — Ultime 24 ore
- `week` — Ultimi 7×24 ore
- `month` — Statistiche giornaliere
- `year` — Statistiche mensili

**Esempio di utilizzo**:
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 23. Strumento Browser WebView (WebViewBrowserTool)

**Nome strumento**: `webview_browser`

**Scenari disponibili**: Chat, Attività, Timer

**Descrizione**: Automazione browser basata su Playwright, fornisce capacità complete di navigazione web, interazione ed estrazione dati.

**Operazioni supportate**:
- `open` — Apri il browser
- `close` — Chiudi il browser
- `navigate` — Naviga a un URL
- `click` — Clicca su un elemento
- `input` — Inserisci testo
- `scroll` — Scorri la pagina
- `execute_script` — Esegui JavaScript
- `get_page_text` — Ottieni il testo della pagina
- `get_screenshot` — Ottieni uno screenshot
- `wait_for_element` — Attendi la comparsa di un elemento
- `get_element_info` — Ottieni informazioni sull'elemento
- `upload_file` — Carica un file
- `get_browser_status` — Ottieni lo stato del browser
- `set_timeout` — Imposta il timeout
- `clear_session` — Cancella la sessione del browser

**Caratteristiche**:
- Istanza indipendente per ogni Essere di Silicio
- Cookie e sessione completamente isolati
- Completamente invisibile all'utente (modalità headless)
- Supporto completo a JavaScript e CSS

**Esempio di utilizzo**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 24. Strumento Note di Lavoro (WorkNoteTool)

**Nome strumento**: `work_note`

**Descrizione**: Gestione delle note di lavoro personali degli Esseri di Silicio (private, simili a un diario).

**Operazioni supportate**:
- `create` — Crea una nota
- `read` — Leggi una nota
- `update` — Aggiorna una nota
- `delete` — Elimina una nota
- `list` — Elenca le note
- `search` — Cerca nelle note
- `directory` — Genera un indice

**Esempio di utilizzo**:
```json
{
  "action": "create",
  "summary": "Completamento modulo autenticazione utente",
  "content": "## Dettagli di implementazione\n\n- Utilizzo JWT token\n- Supporto OAuth2",
  "keywords": "autenticazione,JWT,OAuth2"
}
```

---

### 25. Strumento Hot Reload (HotReloadTool)

**Nome strumento**: `hot_reload`

**Descrizione**: Supporta la compilazione automatica, l'aggiornamento dei file e il riavvio di SiliconLife.Fast durante l'esecuzione, senza intervento manuale.

**Operazioni supportate**:
- `execute` — Esegui il flusso completo di compilazione, copia e riavvio
- `build_only` — Compila solo il progetto, senza copiare e riavviare

**Flusso di lavoro**:
1. Compila il progetto SiliconLife.Fast
2. Chiudi elegantemente l'istanza Fast attualmente in esecuzione (tramite API HTTP)
3. Attendi l'uscita del processo e il rilascio della porta
4. Copia l'output della compilazione nella directory di destinazione (salta i file di HotReload stesso)
5. Riavvia l'istanza Fast

**Caratteristiche**:
- Rilevamento automatico e chiusura dei processi vecchi
- Copia sicura dei file (non sovrascrive HotReload.exe)
- Meccanismo di attesa per il rilascio della porta
- Supporto per configurazione personalizzata della porta

**Esempio di utilizzo**:
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**Descrizione dei parametri**:
- `project_path`: Percorso del progetto (relativo alla directory root della soluzione)
- `source_path`: Directory di output della compilazione
- `configuration`: Configurazione di compilazione (Debug/Release)
- `port`: Porta Web dell'istanza Fast (predefinita 8080)

**Note**:
- Applicabile solo alla versione SiliconLife.Fast
- Richiede HotReload.exe nella directory tools/HotReload
- Ci sarà una breve interruzione del servizio durante il riavvio (circa 3-5 secondi)

---

## Flusso di Chiamata degli Strumenti

```
┌──────────┐
│   AI     │ Restituisce tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Cerca e verifica i diritti di utilizzo dello strumento
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Verifica la catena dei permessi
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Esegue le operazioni di accesso alle risorse
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ Riceve i risultati dello strumento, continua a ragionare
└──────────┘
```

## Verifica dei Permessi

Tutte le esecuzioni degli strumenti passano attraverso la catena di verifica dei permessi:

1. **UserFrequencyCache** — Cache delle decisioni utente ad alta frequenza (HighDeny ha la precedenza su HighAllow)
2. **IPermissionCallback** — Funzione di callback dei permessi personalizzata (Allowed/Denied/AskUser)
3. **Ramo IsCurator** — Il Curatore chiede all'utente tramite IPermissionAskHandler; i non-Curatore interrogano la GlobalACL, in assenza di regole corrispondenti il rifiuto è predefinito

## Creazione di Strumenti Personalizzati

### Passo 1: Implementare l'interfaccia ITool

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "Descrizione dello strumento";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "Descrizione del parametro" }
        }
    };
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        try
        {
            var param1 = call.Parameters["param1"]?.ToString();
            var result = await DoWork(param1);
            
            return new ToolResult
            {
                Success = true,
                Output = result
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}
```

### Passo 2: Aggiungere al Progetto

Posizionare il file dello strumento nella directory `src/SiliconLife.Common/Tools/` (strumenti condivisi) o nella directory `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (strumenti specifici della versione). `ToolManager` lo scoprirà e registrerà automaticamente tramite reflection all'avvio.

### Passo 2a: Registrare Strumenti tramite Plugin

È anche possibile registrare strumenti personalizzati tramite il sistema di plugin:

1. Implementare l'interfaccia `ITool` nel progetto del plugin
2. Compilare la DLL del plugin e posizionarla nella directory dei plugin
3. `ToolManager.ScanAllPluginAssemblies()` scansionerà automaticamente tutte le implementazioni ITool nei plugin caricati
4. Gli strumenti dei plugin sono soggetti allo stesso sistema di permessi

### Passo 3: (Opzionale) Contrassegnare come Esclusivo del Curatore

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Accessibile solo al Curatore di Silicio
}
```

## Best Practices

### 1. Verificare Sempre i Parametri

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Parametro richiesto mancante: required_param");
}
```

### 2. Gestire gli Errori in Modo Elegante

```csharp
try
{
    // Esegui operazione
}
catch (Exception ex)
{
    Logger.Error($"Esecuzione strumento {Name} fallita: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Rispettare il Sistema di Permessi

Non aggirare mai i controlli dei permessi. Accedi sempre alle risorse tramite l'esecutore:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
}
```

### 4. Fornire Descrizioni Chiare degli Strumenti

Aiuta l'AI a capire quando e come utilizzare lo strumento:

```csharp
public string Description => 
    "Utilizzato per convertire date tra diversi sistemi calendariali." +
    "Richiede i parametri 'date', 'from_calendar' e 'to_calendar'.";
```

## Risoluzione dei Problemi

### Strumento Non Trovato

**Problema**: L'AI tenta di chiamare uno strumento inesistente.

**Soluzione**:
- Verifica che il nome dello strumento corrisponda esattamente
- Verifica che il file dello strumento sia nella directory `Tools/`
- Ricompila il progetto (`dotnet build`)

### Permesso Negato

**Problema**: L'esecuzione dello strumento fallisce, restituisce un errore di permesso.

**Soluzione**:
- Controlla i log di audit dei permessi
- Verifica che l'Essere di Silicio abbia i permessi necessari
- Controlla le impostazioni della ACL globale
- Se si tratta del Curatore, verifica se è stato utilizzato il marcatore `[SiliconManagerOnly]`

### Esecuzione dello Strumento Restituisce Errore

**Problema**: Lo strumento viene eseguito ma restituisce un risultato di fallimento.

**Soluzione**:
- Controlla il messaggio di errore restituito dallo strumento
- Verifica che il formato dei parametri di input sia corretto
- Consulta i log di sistema per informazioni dettagliate sull'errore
- Testa la funzionalità dello strumento in modo indipendente

## Prossimi Passi

- 📚 Leggi la [Guida all'Architettura](architecture.md)
- 🛠️ Consulta la [Guida allo Sviluppo](development-guide.md)
- 🔒 Comprendi il [Sistema di Permessi](permission-system.md)
- 🚀 Consulta la [Guida Rapida](getting-started.md)
