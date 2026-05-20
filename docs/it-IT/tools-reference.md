# Riferimento degli strumenti

> **Versione : v0.2.0-alpha**

Questo documento presenta in dettaglio tutti gli strumenti integrati della piattaforma Silicon Life Collective.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [Français](../fr-FR/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | **Italiano**

## Panoramica

Il sistema di strumenti permette ai Silicon Beings di interagire con il mondo esterno tramite un'interfaccia standardizzata. Ogni strumento implementa l'interfaccia `ITool`, scoperto e registrato automaticamente da `ToolManager` tramite reflection.

### Categorie di strumenti

- **Strumenti di amministrazione sistema** — Configurazione, permessi, compilazione dinamica
- **Strumenti di comunicazione** — Chat, richieste di rete
- **Strumenti di storage dati** — Operazioni disco, database, memoria, note di lavoro
- **Strumenti di gestione del tempo** — Calendario, timer, compiti
- **Strumenti di sviluppo** — Esecuzione codice, consultazione log
- **Strumenti utilità** — Informazioni sistema, audit token, documentazione aiuto, rete di conoscenze
- **Strumenti browser** — Automazione browser WebView
- **Strumenti plugin** — Strumenti di terze parti registrati tramite il sistema di plugin

---

## Elenco degli strumenti integrati

### 1. Strumento calendario (CalendarTool)

**Nome dello strumento** : `calendar`

**Descrizione** : Conversione e calcolo di date per 32 sistemi di calendario.

**Operazioni supportate** :
- `now` — Ottenere l'ora corrente
- `format` — Formattare una data
- `add_days` — Aggiungere/sottrarre giorni
- `diff` — Calcolare la differenza tra due date
- `list_calendars` — Elencare tutti i calendari supportati
- `get_components` — Ottenere i componenti di una data
- `get_now_components` — Ottenere i componenti della data corrente
- `convert` — Convertire tra sistemi di calendario

**Sistemi di calendario supportati** (32) :
- Gregoriano (Gregorian)
- Calendario lunare cinese (Chinese Lunar)
- Calendario storico cinese (Chinese Historical) — Era Ganzhi, era imperiale
- Calendario islamico (Islamic)
- Calendario ebraico (Hebrew)
- Calendario giapponese (Japanese)
- Calendario persiano (Persian)
- Calendario maya (Mayan)
- Calendario buddista (Buddhist)
- Calendario tibetano (Tibetan)
- E altri 24 calendari...

**Esempio di utilizzo** :
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. Strumento chat (ChatTool)

**Nome dello strumento** : `chat`

**Descrizione** : Gestione delle sessioni di chat e invio di messaggi.

**Operazioni supportate** :
- `send_message` — Inviare un messaggio
- `get_messages` — Ottenere la cronologia dei messaggi
- `create_group` — Creare una chat di gruppo
- `add_member` — Aggiungere un membro al gruppo
- `remove_member` — Rimuovere un membro dal gruppo
- `get_chat_info` — Ottenere le informazioni della chat
- `terminate_chat` — Terminare la chat (letta senza risposta)

**Esempio di utilizzo** :
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "Ciao, collaboriamo !"
}
```

---

### 3. Strumento configurazione (ConfigTool)

**Nome dello strumento** : `config`

**Descrizione** : Lettura e modifica della configurazione di sistema.

**Operazioni supportate** :
- `read` — Leggere un parametro di configurazione
- `write` — Scrivere un parametro di configurazione
- `list` — Elencare tutte le configurazioni
- `get_ai_config` — Ottenere la configurazione del client IA
- `set_ai_config` — Impostare la configurazione del client IA

**Esempio di utilizzo** :
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Strumento curatore (CuratorTool) 🔒

**Nome dello strumento** : `curator`

**Requisito di permesso** : Riservato al Silicon Curator

**Descrizione** : Strumento di amministrazione sistema riservato al Silicon Curator.

**Operazioni supportate** :
- `create_being` — Creare un nuovo Silicon Being
- `list_beings` — Elencare tutti i Silicon Beings
- `get_being_info` — Ottenere le informazioni di un Being
- `assign_task` — Assegnare un compito
- `manage_permissions` — Gestire i permessi

**Esempio di utilizzo** :
```json
{
  "action": "create_being",
  "name": "Assistente",
  "soul_file": "assistant_soul.md"
}
```

---

### 5. Strumento database (DatabaseTool)

**Nome dello strumento** : `database`

**Descrizione** : Query e operazioni di database strutturato.

**Operazioni supportate** :
- `query` — Interrogare dati
- `insert` — Inserire dati
- `update` — Aggiornare dati
- `delete` — Eliminare dati
- `create_table` — Creare una tabella
- `list_tables` — Elencare tutte le tabelle

**Esempio di utilizzo** :
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. Strumento disco (DiskTool)

**Nome dello strumento** : `disk`

**Descrizione** : Operazioni sul filesystem e ricerca locale.

**Operazioni supportate** :
- `read` — Leggere un file
- `write` — Scrivere in un file
- `list` — Elencare una directory
- `delete` — Eliminare un file
- `create_directory` — Creare una directory
- `search_files` — Cercare file
- `search_content` — Cercare il contenuto di file
- `count_lines` — Contare le righe
- `read_lines` — Leggere righe specifiche
- `replace_text` — Sostituire testo

**Requisito di permesso** : `disk:read`, `disk:write`

**Esempio di utilizzo** :
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Strumento compilazione dinamica (DynamicCompileTool) 🔒

**Nome dello strumento** : `compile`

**Descrizione** : Compilazione dinamica di codice C# (per l'auto-evoluzione dei Silicon Beings).

**Operazioni supportate** :
- `compile_class` — Compilare una classe
- `compile_callback` — Compilare una funzione di callback di permesso
- `validate_code` — Validare la sicurezza del codice

**Meccanismi di sicurezza** :
- Controllo dei riferimenti in compilazione (esclusione degli assembly pericolosi)
- Analisi statica del codice a runtime
- Storage crittografato AES-256

**Esempio di utilizzo** :
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Strumento esecuzione codice (ExecuteCodeTool) 🔒

**Nome dello strumento** : `execute_code`

**Requisito di permesso** : Riservato al Silicon Curator

**Descrizione** : Compilare ed eseguire frammenti di codice C#.

**Operazioni supportate** :
- `run_script` — Eseguire uno script di codice

**Esempio di utilizzo** :
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. Strumento aiuto (HelpTool)

**Nome dello strumento** : `help`

**Descrizione** : Ottenere la documentazione di aiuto sistema e le guide all'uso.

**Operazioni supportate** :
- `get_topics` — Ottenere la lista degli argomenti di aiuto
- `get_topic` — Ottenere i dettagli di un argomento specifico
- `search` — Cercare nella documentazione di aiuto

**Esempio di utilizzo** :
```json
{
  "action": "get_topics"
}
```

---

### 10. Strumento rete di conoscenze (KnowledgeTool)

**Nome dello strumento** : `knowledge`

**Descrizione** : Operazioni sul grafo di conoscenze (basato su triplette : soggetto-relazione-oggetto).

**Operazioni supportate** :
- `add` — Aggiungere una tripletta di conoscenze
- `query` — Interrogare le conoscenze
- `update` — Aggiornare le conoscenze
- `delete` — Eliminare conoscenze
- `search` — Cercare conoscenze
- `get_path` — Ottenere un percorso di conoscenze
- `validate` — Validare le conoscenze
- `stats` — Ottenere statistiche

**Esempio di utilizzo** :
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

### 11. Strumento log (LogTool)

**Nome dello strumento** : `log`

**Descrizione** : Consultare la cronologia delle operazioni e delle conversazioni.

**Operazioni supportate** :
- `query_logs` — Consultare i log di sistema
- `query_conversations` — Consultare la cronologia delle conversazioni
- `get_stats` — Ottenere le statistiche dei log

**Esempio di utilizzo** :
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

### 12. Strumento memoria (MemoryTool)

**Nome dello strumento** : `memory`

**Descrizione** : Gestire la memoria a lungo termine e a breve termine dei Silicon Beings.

**Operazioni supportate** :
- `read` — Leggere la memoria
- `write` — Scrivere nella memoria
- `search` — Cercare nella memoria
- `delete` — Eliminare la memoria
- `list` — Elencare la memoria
- `get_stats` — Ottenere le statistiche della memoria
- `compress` — Comprimere la memoria

**Esempio di utilizzo** :
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

### 13. Strumento rete (NetworkTool)

**Nome dello strumento** : `network`

**Descrizione** : Effettuare richieste HTTP/HTTPS.

**Operazioni supportate** :
- `get` — Richiesta GET
- `post` — Richiesta POST
- `put` — Richiesta PUT
- `delete` — Richiesta DELETE
- `download` — Scaricare un file
- `upload` — Caricare un file

**Requisito di permesso** : `network:http`

**Esempio di utilizzo** :
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. Strumento permessi (PermissionTool) 🔒

**Nome dello strumento** : `permission`

**Requisito di permesso** : Riservato al Silicon Curator

**Descrizione** : Gestire i permessi e le liste di controllo di accesso.

**Operazioni supportate** :
- `query_permission` — Interrogare i permessi
- `manage_acl` — Gestire l'ACL globale
- `get_callback` — Ottenere la funzione di callback di permesso
- `set_callback` — Impostare la funzione di callback di permesso

**Esempio di utilizzo** :
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

### 15. Strumento progetto (ProjectTool)

**Nome dello strumento** : `project`

**Descrizione** : Gestire gli spazi di lavoro di progetto.

**Operazioni supportate** :
- `create` — Creare un progetto
- `list` — Elencare i progetti
- `get_info` — Ottenere le informazioni del progetto
- `update` — Aggiornare il progetto
- `archive` — Archiviare il progetto

**Esempio di utilizzo** :
```json
{
  "action": "create",
  "name": "Il Mio Progetto",
  "description": "Descrizione del progetto"
}
```

---

### 16. Strumento compiti di progetto (ProjectTaskTool)

**Nome dello strumento** : `project_task`

**Descrizione** : Gestire i compiti di progetto.

**Operazioni supportate** :
- `create` — Creare un compito
- `list` — Elencare i compiti
- `update` — Aggiornare un compito
- `complete` — Completare un compito
- `get_stats` — Ottenere le statistiche dei compiti

**Esempio di utilizzo** :
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Descrizione del compito",
  "priority": 5
}
```

---

### 17. Strumento note di lavoro di progetto (ProjectWorkNoteTool)

**Nome dello strumento** : `project_work_note`

**Descrizione** : Gestire le note di lavoro di progetto (pubbliche, simili a un quaderno di lavoro).

**Operazioni supportate** :
- `create` — Creare una nota
- `read` — Leggere una nota
- `update` — Aggiornare una nota
- `delete` — Eliminare una nota
- `list` — Elencare le note
- `search` — Cercare note
- `directory` — Generare una directory

**Esempio di utilizzo** :
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Modulo autenticazione utente completato",
  "content": "## Dettagli implementazione\n\n- Utilizzo di JWT token",
  "keywords": "autenticazione,JWT"
}
```

---

### 18. Strumento sistema (SystemTool)

**Nome dello strumento** : `system`

**Descrizione** : Ottenere le informazioni di sistema e l'utilizzo delle risorse.

**Operazioni supportate** :
- `info` — Ottenere le informazioni di sistema
- `resource_usage` — Ottenere l'utilizzo delle risorse
- `find_process` — Cercare un processo
- `list_beings` — Elencare i Silicon Beings

**Esempio di utilizzo** :
```json
{
  "action": "info"
}
```

---

### 19. Strumento compiti (TaskTool)

**Nome dello strumento** : `task`

**Descrizione** : Gestire i compiti personali dei Silicon Beings.

**Operazioni supportate** :
- `create` — Creare un compito
- `list` — Elencare i compiti
- `update` — Aggiornare un compito
- `complete` — Completare un compito
- `delete` — Eliminare un compito
- `get_dependencies` — Ottenere le dipendenze

**Esempio di utilizzo** :
```json
{
  "action": "create",
  "description": "Revisionare il codice",
  "priority": 5
}
```

---

### 20. Strumento timer (TimerTool)

**Nome dello strumento** : `timer`

**Descrizione** : Creare e gestire timer.

**Operazioni supportate** :
- `create` — Creare un timer
- `list` — Elencare i timer
- `delete` — Eliminare un timer
- `pause` — Mettere in pausa un timer
- `resume` — Riprendere un timer
- `get_execution_history` — Ottenere la cronologia di esecuzione

**Esempio di utilizzo** :
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "Promemoria orario"
}
```

---

### 21. Strumento audit token (TokenAuditTool) 🔒

**Nome dello strumento** : `token_audit`

**Requisito di permesso** : Riservato al Silicon Curator

**Descrizione** : Consultare e riassumere l'utilizzo dei token IA.

**Operazioni supportate** :
- `get_usage` — Ottenere le statistiche di utilizzo dei token
- `get_by_being` — Ottenere l'utilizzo per Being
- `get_by_model` — Ottenere l'utilizzo per modello
- `get_trend` — Ottenere le tendenze di utilizzo
- `export` — Esportare i dati

**Esempio di utilizzo** :
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. Strumento browser WebView (WebViewBrowserTool)

**Nome dello strumento** : `webview`

**Descrizione** : Automazione browser basata su Playwright.

**Operazioni supportate** :
- `open_browser` — Aprire il browser
- `close_browser` — Chiudere il browser
- `navigate` — Navigare verso un URL
- `click` — Cliccare su un elemento
- `input` — Inserire testo
- `get_page_text` — Ottenere il testo della pagina
- `get_screenshot` — Ottenere uno screenshot
- `execute_script` — Eseguire JavaScript
- `wait_for_element` — Attendere la comparsa di un elemento
- `get_browser_status` — Ottenere lo stato del browser

**Caratteristiche** :
- Istanza indipendente per ogni Silicon Being
- Cookie e sessioni completamente isolati
- Totalmente invisibile per l'utente (modalità headless)
- Supporto completo di JavaScript e CSS

**Esempio di utilizzo** :
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 23. Strumento note di lavoro (WorkNoteTool)

**Nome dello strumento** : `work_note`

**Descrizione** : Gestire le note di lavoro personali dei Silicon Beings (private, simili a un diario).

**Operazioni supportate** :
- `create` — Creare una nota
- `read` — Leggere una nota
- `update` — Aggiornare una nota
- `delete` — Eliminare una nota
- `list` — Elencare le note
- `search` — Cercare note
- `directory` — Generare una directory

**Esempio di utilizzo** :
```json
{
  "action": "create",
  "summary": "Modulo autenticazione utente completato",
  "content": "## Dettagli implementazione\n\n- Utilizzo di JWT token\n- Supporto OAuth2",
  "keywords": "autenticazione,JWT,OAuth2"
}
```

---

### 24. Strumento ricaricamento a caldo (HotReloadTool)

**Nome dello strumento** : `hot_reload`

**Descrizione** : Supporta la compilazione automatica, l'aggiornamento dei file e il riavvio di SiliconLife.Fast durante l'esecuzione, senza intervento manuale.

**Operazioni supportate** :
- `execute` — Esegue il processo completo di build, copia e riavvio
- `build_only` — Solo build del progetto, senza copia né riavvio

**Flusso di lavoro** :
1. Compila il progetto SiliconLife.Fast
2. Chiude gentilmente l'istanza Fast in esecuzione (tramite API HTTP)
3. Attende l'uscita del processo e il rilascio della porta
4. Copia l'output di build nella directory di destinazione (esclude i file HotReload)
5. Riavvia l'istanza Fast

**Caratteristiche** :
- Rilevamento e chiusura automatici del vecchio processo
- Copia sicura dei file (non sovrascrive HotReload.exe)
- Meccanismo di attesa rilascio porta
- Supporto di configurazione porta personalizzata

**Esempio di utilizzo** :
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**Descrizione dei parametri** :
- `project_path` : Percorso del progetto (relativo alla directory radice della soluzione)
- `source_path` : Directory di output di build
- `configuration` : Configurazione di build (Debug/Release)
- `port` : Porta Web dell'istanza Fast (default 8080)

**Note** :
- Applicabile solo alla versione SiliconLife.Fast
- Richiede HotReload.exe nella directory tools/HotReload
- Breve interruzione di servizio durante il riavvio (circa 3-5 secondi)

## Flusso di chiamata dello strumento

```
┌──────────┐
│   IA     │ Restituisce tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Cerca e valida i diritti di utilizzo
└────┬─────────┘
     ↓
┌──────────────┐
│ Permessi     │ Verifica la catena di permessi
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Esegue l'operazione di accesso alle risorse
└────┬─────────┘
     ↓
┌──────────┐
│   IA     │ Riceve il risultato dello strumento, continua la riflessione
└──────────┘
```

## Verifica dei permessi

Tutte le esecuzioni di strumenti passano attraverso la catena di permessi a 5 livelli :

1. **UserFrequencyCache** — Cache delle autorizzazioni/rifiuti frequenti dell'utente
2. **IPermissionCallback** — Funzione di callback di permesso personalizzata
3. **Giudizio ramificato** — IsCurator → IPermissionAskHandler (chiedi utente) | Non-curatore → GlobalACL (lista controllo accessi)

## Creare uno strumento personalizzato

### Passo 1 : Implementare l'interfaccia ITool

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

### Passo 2 : Aggiungere al progetto

Posizionare il file dello strumento nella directory `src/SiliconLife.Common/App/Tools/` (strumenti condivisi) o `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (strumenti specifici di una versione). `ToolManager` scoprirà e registrerà automaticamente lo strumento tramite reflection all'avvio.

### Passo 2a : Registrare uno strumento tramite plugin

È anche possibile registrare strumenti personalizzati tramite il sistema di plugin :

1. Implementare l'interfaccia `ITool` nel progetto del plugin
2. Compilare la DLL del plugin e posizionarla nella directory dei plugin
3. `ToolManager.ScanAllPluginAssemblies()` analizzerà automaticamente tutte le implementazioni ITool nei plugin caricati
4. Gli strumenti plugin sono soggetti allo stesso sistema di permessi

### Passo 3 : (Opzionale) Marcare come riservato al curatore

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Solo il Silicon Curator può accedervi
}
```

## Migliori pratiche

### 1. Validare sempre i parametri

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Parametro richiesto mancante : required_param");
}
```

### 2. Gestire gli errori con eleganza

```csharp
try
{
    // Eseguire l'operazione
}
catch (Exception ex)
{
    Logger.Error($"Strumento {Name} fallito : {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Rispettare il sistema di permessi

Non bypassare mai le verifiche dei permessi. Accedere sempre alle risorse tramite gli executor :

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. Fornire descrizioni chiare degli strumenti

Aiutare l'IA a capire quando e come usare lo strumento :

```csharp
public string Description => 
    "Utilizzato per convertire date tra diversi sistemi di calendario." +
    "Richiede i parametri 'date', 'from_calendar' e 'to_calendar'.";
```

## Risoluzione dei problemi

### Strumento non trovato

**Problema** : L'IA tenta di chiamare uno strumento che non esiste.

**Soluzione** :
- Verificare che il nome dello strumento corrisponda esattamente
- Verificare che il file dello strumento sia nella directory `Tools/`
- Ricostruire il progetto (`dotnet build`)

### Permesso negato

**Problema** : L'esecuzione dello strumento fallisce con un errore di permesso.

**Soluzione** :
- Consultare i log di audit dei permessi
- Verificare che il Silicon Being disponga dei permessi richiesti
- Consultare le impostazioni dell'ACL globale
- Se è un curatore, verificare se l'attributo `[SiliconManagerOnly]` è utilizzato

### L'esecuzione dello strumento restituisce un errore

**Problema** : Lo strumento viene eseguito ma restituisce un risultato di fallimento.

**Soluzione** :
- Verificare il messaggio di errore restituito dallo strumento
- Validare il formato dei parametri di input
- Consultare i log di sistema per informazioni dettagliate sull'errore
- Testare la funzionalità dello strumento indipendentemente

## Prossimi passi

- 📚 Leggere la [guida all'architettura](architecture.md)
- 🛠️ Consultare la [guida di sviluppo](development-guide.md)
- 🔒 Comprendere il [sistema di permessi](permission-system.md)
- 🚀 Consultare la [guida rapida](getting-started.md)
