# Reference nástrojů

> **Verze: v0.2.0-alpha**

Tento dokument podrobně popisuje všechny vestavěné nástroje platformy Silicon Life Collective.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | **Čeština** | [Русский](../ru-RU/tools-reference.md)

## Přehled

Systém nástrojů umožňuje Křemíkovým Bytostem interagovat s vnějším světem prostřednictvím standardizovaného rozhraní. Každý nástroj implementuje rozhraní `ITool` a je automaticky objevován a registrován `ToolManager` pomocí reflexe.

### Klasifikace nástrojů

- **Nástroje správy systému** — konfigurace, oprávnění, dynamická kompilace, správa Kurátora
- **Komunikační nástroje** — chat, síťové požadavky
- **Nástroje datového úložiště** — diskové operace, databáze, paměť, pracovní poznámky
- **Nástroje správy času** — kalendář, časovače, úkoly
- **Vývojářské nástroje** — spouštění kódu, dotazy na protokoly
- **Užitkové nástroje** — systémové informace, audit Tokenů, dokumentace nápovědy, znalostní síť
- **Nástroje prohlížeče** — automatizace WebView prohlížeče
- **Projektové nástroje** — správa projektů, projektové úkoly, projektové pracovní poznámky, projektová práce
- **Nástroje zásuvných modulů** — nástroje třetích stran registrované prostřednictvím systému zásuvných modulů

### Systém scénářů nástrojů

Každý nástroj deklaruje své dostupné scénáře pomocí atributu `[ToolScenario]`:

| Příznak scénáře | Hodnota | Popis |
|----------|------|------|
| `Chat` | `1 << 0` | Scénář chatu (když uživatel konverzuje s Křemíkovou Bytostí) |
| `Task` | `1 << 1` | Scénář úkolu (když Křemíková Bytost provádí úkol) |
| `Timer` | `1 << 2` | Scénář časovače (když Křemíková Bytost provádí časovaný úkol) |
| `MemoryCompression` | `1 << 3` | Scénář komprese paměti |
| `Project` | `1 << 4` | Projektový scénář (režim ThinkOnProject) |
| `All` | Všechny výše uvedené | Všechny scénáře jsou dostupné |

Kromě toho nástroje označené atributem `[ChatOnly]` jsou dostupné pouze ve scénáři chatu (např. HelpTool) a neobjevují se ve scénářích úkolů a časovačů.

---

## Seznam vestavěných nástrojů

### 1. Kalendářní nástroj (CalendarTool)

**Název nástroje**: `calendar`

**Popis funkce**: Podporuje konverzi a výpočty dat pro 32 kalendářních systémů.

**Podporované operace**:
- `now` — Získání aktuálního času
- `format` — Formátování data
- `add_days` — Přičítání/odčítání dnů
- `diff` — Výpočet rozdílu dat
- `list_calendars` — Seznam všech podporovaných kalendářů
- `get_components` — Získání komponent data
- `get_now_components` — Získání komponent aktuálního času
- `convert` — Konverze mezi kalendářními systémy

**Podporované kalendářní systémy** (32):
- Gregoriánský (Gregorian)
- Čínský lunární (Chinese Lunar)
- Čínský historický (Chinese Historical) — ganzhi letopočet, éry císařů
- Islámský (Islamic)
- Hebrejský (Hebrew)
- Japonský (Japanese)
- Perský (Persian)
- Mayský (Mayan)
- Buddhistský (Buddhist)
- Tibetský (Tibetan)
- A 24 dalších kalendářů...

**Příklad použití**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. Chatovací nástroj (ChatTool)

**Název nástroje**: `chat`

**Popis funkce**: Správa chatovacích relací a odesílání zpráv.

**Podporované operace**:
- `send_message` — Odeslání zprávy
- `get_messages` — Získání historických zpráv
- `create_group` — Vytvoření skupinového chatu
- `add_member` — Přidání člena do skupiny
- `remove_member` — Odebrání člena ze skupiny
- `get_chat_info` — Získání informací o chatu
- `terminate_chat` — Ukončení chatu (přečteno bez odpovědi)

**Příklad použití**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "Ahoj, pojďme spolupracovat!"
}
```

---

### 3. Konfigurační nástroj (ConfigTool)

**Název nástroje**: `config`

**Popis funkce**: Čtení a úprava systémové konfigurace.

**Podporované operace**:
- `read` — Čtení konfigurační položky
- `write` — Zápis konfigurační položky
- `list` — Seznam všech konfigurací
- `get_ai_config` — Získání konfigurace AI klienta
- `set_ai_config` — Nastavení konfigurace AI klienta

**Příklad použití**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Nástroj Kurátora (CuratorTool) 🔒

**Název nástroje**: `silicon_manager`

**Požadavek na oprávnění**: Pouze pro Kurátora Křemíku (`[SiliconManagerOnly]`)

**Dostupné scénáře**: Chat, Task, Timer

**Popis funkce**: Nástroj správy systému určený výhradně pro Kurátora Křemíku, pro správu vytváření, prohlížení a resetování Křemíkových Bytostí.

**Podporované operace**:
- `list_beings` — Seznam všech Křemíkových Bytostí a jejich stavů
- `create_being` — Vytvoření nové Křemíkové Bytosti (vyžaduje parametry `name` a `soul`)
- `get_code` — Zobrazení vlastního zdrojového kódu Křemíkové Bytosti
- `reset` — Resetování Křemíkové Bytosti na výchozí implementaci

**Příklad použití**:
```json
{
  "action": "create_being",
  "name": "Asistent",
  "soul": "Jsi užitečný asistent..."
}
```

---

### 5. Databázový nástroj (DatabaseTool)

**Název nástroje**: `database`

**Popis funkce**: Strukturované databázové dotazy a operace.

**Podporované operace**:
- `query` — Dotazování dat
- `insert` — Vkládání dat
- `update` — Aktualizace dat
- `delete` — Mazání dat
- `create_table` — Vytvoření tabulky
- `list_tables` — Seznam všech tabulek

**Příklad použití**:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. Diskový nástroj (DiskTool)

**Název nástroje**: `disk`

**Popis funkce**: Operace souborového systému a lokální vyhledávání.

**Podporované operace**:
- `read` — Čtení souboru
- `write` — Zápis souboru
- `list` — Výpis adresáře
- `delete` — Mazání souboru
- `create_directory` — Vytvoření adresáře
- `search_files` — Vyhledávání souborů
- `search_content` — Vyhledávání obsahu souborů
- `count_lines` — Počítání řádků
- `read_lines` — Čtení určených řádků
- `replace_text` — Nahrazování textu

**Požadavek na oprávnění**: `FileAccess`

**Příklad použití**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Nástroj dynamické kompilace (DynamicCompileTool) 🔒

**Název nástroje**: `compile`

**Popis funkce**: Dynamická kompilace kódu C# (pro sebeevoluci Křemíkové Bytosti).

**Podporované operace**:
- `compile_class` — Kompilace třídy
- `compile_callback` — Kompilace funkce zpětného volání oprávnění
- `validate_code` — Validace bezpečnosti kódu

**Bezpečnostní mechanismy**:
- Kontrola referencí při kompilaci (vyloučení nebezpečných sestavení)
- Statická analýza kódu za běhu
- Šifrované úložiště AES-256

**Příklad použití**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Nástroj spouštění kódu (ExecuteCodeTool) 🔒

**Název nástroje**: `execute_code`

**Požadavek na oprávnění**: Pouze pro Kurátora Křemíku

**Popis funkce**: Kompilace a spuštění fragmentu kódu C#.

**Podporované operace**:
- `run_script` — Spuštění skriptu

**Příklad použití**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. Nástroj nápovědy (HelpTool)

**Název nástroje**: `help`

**Dostupné scénáře**: Chat (`[ChatOnly]`, dostupné pouze ve scénáři chatu)

**Popis funkce**: Vyhledávání a získávání obsahu systémové dokumentace nápovědy, umožňuje AI dotazovat se na způsoby použití systémových funkcí.

**Podporované operace**:
- `list` — Seznam všech ID témat nápovědy
- `search` — Vyhledávání v dokumentaci nápovědy podle klíčových slov
- `get` — Získání obsahu dokumentace nápovědy pro zadané ID

**Příklad použití**:
```json
{
  "action": "search",
  "keyword": "oprávnění"
}
```

---

### 10. Nástroj znalostní sítě (KnowledgeTool)

**Název nástroje**: `knowledge`

**Popis funkce**: Operace se znalostním grafem (založeno na trojicích: subjekt-relace-objekt).

**Podporované operace**:
- `add` — Přidání trojice znalostí
- `query` — Dotazování znalostí
- `update` — Aktualizace znalostí
- `delete` — Mazání znalostí
- `search` — Vyhledávání znalostí
- `get_path` — Získání znalostní cesty
- `validate` — Validace znalostí
- `stats` — Získání statistických informací

**Příklad použití**:
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

### 11. MCP dotazovací nástroj (McpTool)

**Název nástroje**: `mcp`

**Popis funkce**: Dotazování stavu integrace MCP (Model Context Protocol) — připojené externí servery, nástroje, které poskytují, a jak je volat. Toto je nástroj pouze pro čtení: přidávání/odstraňování serverů může provádět pouze uživatel prostřednictvím Web UI, AI nemůže měnit seznam serverů.

**Podporované operace**:
- `status` — Globální přehled (stav povolení, počet serverů, počet nástrojů)
- `list_servers` — Seznam nakonfigurovaných serverů (včetně stavu připojení a počtu nástrojů)
- `list_tools` — Seznam dostupných nástrojů (s prefixem `mcp_{server}_{tool}`, popisem a schématem parametrů; volitelný filtr `server_id` pro jeden server)

**Příklad použití**:
```json
{
  "action": "list_tools",
  "server_id": "filesystem",
  "include_schema": true
}
```

**MCP obalové nástroje**: Každý nástroj poskytovaný připojeným MCP serverem je dynamicky registrován jako nezávislý nástroj pro Křemíkové Bytosti, s formátem názvu `mcp_{serverId}_{toolName}` (např. `mcp_filesystem_read_file`). AI je může volat přímo podle prefixu jako běžné nástroje, bez nutnosti procházet tímto dotazovacím nástrojem. Obalové nástroje se v matici oprávnění prezentují jako jediná akce `execute` a mohou být individuálně zakázány.

**Scénář**: Všechny scénáře (`All`)

---

### 12. Protokolový nástroj (LogTool)

**Název nástroje**: `log`

**Popis funkce**: Dotazování historie operací a historie konverzací.

**Podporované operace**:
- `query_logs` — Dotazování systémových protokolů
- `query_conversations` — Dotazování historie konverzací
- `get_stats` — Získání statistik protokolů

**Příklad použití**:
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

### 13. Nástroj paměti (MemoryTool)

**Název nástroje**: `memory`

**Popis funkce**: Správa dlouhodobé a krátkodobé paměti Křemíkové Bytosti.

**Podporované operace**:
- `read` — Čtení paměti
- `write` — Zápis paměti
- `search` — Vyhledávání paměti
- `delete` — Mazání paměti
- `list` — Seznam pamětí
- `get_stats` — Získání statistik paměti
- `compress` — Komprese paměti

**Příklad použití**:
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

### 14. Síťový nástroj (NetworkTool)

**Název nástroje**: `network`

**Popis funkce**: Iniciování HTTP/HTTPS požadavků.

**Podporované operace**:
- `get` — GET požadavek
- `post` — POST požadavek
- `put` — PUT požadavek
- `delete` — DELETE požadavek
- `download` — Stažení souboru
- `upload` — Nahrání souboru

**Požadavek na oprávnění**: `network:http`

**Příklad použití**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 15. Nástroj oprávnění (PermissionTool) 🔒

**Název nástroje**: `permission`

**Požadavek na oprávnění**: Pouze pro Kurátora Křemíku

**Popis funkce**: Správa oprávnění a seznamů řízení přístupu.

**Podporované operace**:
- `query_permission` — Dotazování oprávnění
- `manage_acl` — Správa globálního ACL
- `get_callback` — Získání funkce zpětného volání oprávnění
- `set_callback` — Nastavení funkce zpětného volání oprávnění

**Příklad použití**:
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

### 16. Projektový nástroj (ProjectTool) 🔒

**Název nástroje**: `project`

**Požadavek na oprávnění**: Pouze pro Kurátora Křemíku (`[SiliconManagerOnly]`)

**Dostupné scénáře**: Chat, Task, Timer

**Popis funkce**: Správa projektového pracovního prostoru, podpora správy životního cyklu projektu, přiřazování členů a správa rolí.

**Podporované operace**:
- `create` — Vytvoření nového projektového prostoru
- `archive` — Archivace projektu
- `restore` — Obnovení archivovaného projektu
- `destroy` — Zničení projektu a vyčištění dat (není obnovitelné)
- `list` — Seznam všech projektů
- `get` — Získání detailů projektu
- `assign` — Přiřazení Křemíkové Bytosti do projektu
- `remove` — Odebrání Křemíkové Bytosti z projektu
- `update` — Aktualizace názvu/popisu projektu
- `list-workflow-templates` — Seznam dostupných šablon pracovních postupů
- `assign_role` — Přiřazení projektové role Křemíkové Bytosti
- `remove_role` — Odebrání projektové role Křemíkové Bytosti
- `list_roles` — Seznam přiřazení rolí projektu

**Příklad použití**:
```json
{
  "action": "create",
  "name": "Můj projekt",
  "description": "Popis projektu"
}
```

---

### 17. Nástroj projektových úkolů (ProjectTaskTool)

**Název nástroje**: `project_task`

**Dostupné scénáře**: Chat, Task, Timer

**Popis funkce**: Správa úkolů v projektovém prostoru, podpora kompletního životního cyklu úkolů.

**Podporované operace**:
- `create` — Vytvoření projektového úkolu
- `list` — Seznam projektových úkolů
- `get` — Získání detailů úkolu
- `update` — Aktualizace názvu/popisu/priority úkolu
- `assign` — Přiřazení zodpovědné osoby k úkolu
- `remove_assignee` — Odebrání zodpovědné osoby z úkolu
- `start` — Zahájení úkolu
- `complete` — Označení úkolu jako dokončeného
- `fail` — Označení úkolu jako selhaného
- `cancel` — Zrušení úkolu
- `delete` — Smazání úkolu
- `stats` — Získání statistik úkolů

**Příklad použití**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Popis úkolu",
  "priority": 5
}
```

---

### 18. Nástroj projektových pracovních poznámek (ProjectWorkNoteTool)

**Název nástroje**: `project_work_note`

**Dostupné scénáře**: Chat, Task, Timer

**Popis funkce**: Správa pracovních poznámek v projektovém prostoru (veřejné, podobné pracovnímu sešitu), podpora stránkového správy poznámek.

**Podporované operace**:
- `create` — Vytvoření stránky poznámky (vyžaduje `project_id`, `summary` a `content`, volitelně `keywords`)
- `read` — Čtení stránky poznámky (vyžaduje `project_id` a `page_number` nebo `note_id`)
- `update` — Aktualizace stránky poznámky (vyžaduje `project_id`, `page_number` a `content`, volitelně `summary` a `keywords`)
- `delete` — Smazání stránky poznámky (vyžaduje `project_id` a `page_number` nebo `note_id`)
- `list` — Seznam všech stránek poznámek projektu
- `directory` — Generování obsahu/přehledu poznámek
- `search` — Vyhledávání poznámek podle klíčových slov (vyžaduje `project_id` a `keyword`, volitelně `max_results`)

**Příklad použití**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Dokončení modulu uživatelské autentizace",
  "content": "## Detaily implementace\n\n- Použití JWT token",
  "keywords": "autentizace,JWT"
}
```

---

### 19. Nástroj projektové práce (ProjectWorkTool) 🔒

**Název nástroje**: `project_work`

**Požadavek na oprávnění**: Pouze pro Kurátora Křemíku (`[SiliconManagerOnly]`)

**Dostupné scénáře**: Project (`[ToolScenario(ToolScenarioFlag.Project)]`, dostupné pouze v projektovém scénáři)

**Popis funkce**: Nástroj projektových pracovních operací, určený pro Kurátora ke správě projektových pracovních postupů ve scénáři ThinkOnProject.

**Podporované operace**:
- `create-task` — Vytvoření projektového úkolu
- `assign-task` — Přiřazení Křemíkové Bytosti k úkolu
- `chat` — Odeslání zprávy do skupinového chatu projektu
- `broadcast` — Vysílání zprávy do projektového kanálu
- `complete` — Označení projektu jako dokončeného
- `status` — Získání stavu projektu

**Příklad použití**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "Implementace uživatelské autentizace"
}
```

---

### 20. Nástroj dovedností (SkillTool)

**Název nástroje**: `skill`

**Popis funkce**: Správa dovedností Křemíkových Bytostí (znovupoužitelné jednotky schopností „orchestrace nástrojů + šablona výzev"), podpora vytváření, výpisu, aktualizace, odstranění, importu a exportu. Chybějící metadata (id, popis, schéma parametrů atd.) jsou automaticky doplněna AI.

**Podporované operace**:
- `create` — Vytvoření nové dovednosti (vyžaduje `id` a `system_prompt`, volitelně `description`, `parameter_schema`, `tool_whitelist`, `tags`, `max_tool_round`, `timeout`, `on_complete`, `trigger_mode`, `auto_trigger_condition`)
- `list` — Seznam všech dostupných dovedností (včetně shrnutí)
- `update` — Aktualizace existující dovednosti pomocí parametrů (vyžaduje `skill_id`)
- `update_from_md` — Aktualizace dovednosti z řetězce Markdown (YAML front matter + tělo výzvy)
- `delete` — Odstranění dovednosti (vyžaduje `skill_id`)
- `export` — Export dovednosti do JSON (vyžaduje `skill_id`)
- `export_md` — Export dovednosti do Markdown (vyžaduje `skill_id`)
- `import` — Import dovednosti z JSON (vyžaduje `json`)
- `import_md` — Import dovednosti z Markdown (vyžaduje `markdown`)

**Příklad použití**:
```json
{
  "action": "create",
  "id": "daily_news_digest",
  "description": "Vyhledat dnešní technologické zprávy a vygenerovat shrnutí",
  "system_prompt": "Použijte nástroj network k vyhledání nejnovějších zpráv o {topic} a vygenerujte shrnutí o 500 slovech.",
  "parameter_schema": {
    "type": "object",
    "properties": {
      "topic": { "type": "string", "description": "Téma zpráv" }
    },
    "required": ["topic"]
  },
  "tool_whitelist": ["network", "work_note"],
  "trigger_mode": "Auto",
  "auto_trigger_condition": "schedule",
  "metadata": { "schedule": "0 9 * * *" }
}
```

**Oprávnění úprav**: Kurátor Křemíku může upravovat všechny dovednosti; běžné bytosti mohou upravovat pouze dovednosti se zdrojem `Being` nebo `User` (nemohou upravovat vestavěné dovednosti a dovednosti zásuvných modulů).

**Omezení počtu**: Počet vlastních dovedností každé bytosti je omezen konfigurací `MaxCustomSkillsPerBeing` (výchozí 50).

**Scénář**: Všechny scénáře (`All`)

> Úplný popis systému dovedností (spouštěcí režimy, whitelist, hot-reload, automatické plánování atd.) viz [Průvodce Křemíkovou Bytostí](silicon-being-guide.md#systém-dovedností).

---

### 21. Systémový nástroj (SystemTool)

**Název nástroje**: `system`

**Popis funkce**: Získávání systémových informací a využití prostředků.

**Podporované operace**:
- `info` — Získání systémových informací
- `resource_usage` — Získání využití prostředků
- `find_process` — Vyhledání procesu
- `list_beings` — Seznam Křemíkových Bytostí

**Příklad použití**:
```json
{
  "action": "info"
}
```

---

### 22. Nástroj úkolů (TaskTool)

**Název nástroje**: `task`

**Popis funkce**: Správa osobních úkolů Křemíkové Bytosti.

**Podporované operace**:
- `create` — Vytvoření úkolu
- `list` — Seznam úkolů
- `update` — Aktualizace úkolu
- `complete` — Dokončení úkolu
- `delete` — Smazání úkolu
- `get_dependencies` — Získání závislostí

**Příklad použití**:
```json
{
  "action": "create",
  "description": "Kontrola kódu",
  "priority": 5
}
```

---

### 23. Nástroj časovačů (TimerTool)

**Název nástroje**: `timer`

**Popis funkce**: Vytváření a správa časovačů.

**Podporované operace**:
- `create` — Vytvoření časovače
- `list` — Seznam časovačů
- `delete` — Smazání časovače
- `pause` — Pozastavení časovače
- `resume` — Obnovení časovače
- `get_execution_history` — Získání historie provádění

**Příklad použití**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "Hodinová připomínka"
}
```

---

### 24. Nástroj auditu Tokenů (TokenAuditTool) 🔒 🔒

**Název nástroje**: `token_audit`

**Požadavek na oprávnění**: Pouze pro Kurátora Křemíku (`[SiliconManagerOnly]`)

**Dostupné scénáře**: Chat, Task, Timer

**Popis funkce**: Dotazování na statistiky a trendy využití AI Tokenů.

**Podporované operace**:
- `summary` — Získání souhrnných statistik využití Tokenů
- `trend` — Získání datových bodů trendu využití Tokenů

**Podporované časové rozsahy**:
- `today` — Posledních 24 hodin
- `week` — Posledních 7×24 hodin
- `month` — Statistiky po dnech
- `year` — Statistiky po měsících

**Příklad použití**:
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 25. Nástroj WebView prohlížeče (WebViewBrowserTool)

**Název nástroje**: `webview_browser`

**Dostupné scénáře**: Chat, Task, Timer

**Popis funkce**: Automatizace prohlížeče založená na Playwright, poskytující kompletní schopnosti navigace, interakce a extrakce dat z webových stránek.

**Podporované operace**:
- `open` — Otevření prohlížeče
- `close` — Zavření prohlížeče
- `navigate` — Navigace na URL
- `click` — Kliknutí na prvek
- `input` — Zadání textu
- `scroll` — Posouvání stránky
- `execute_script` — Spuštění JavaScriptu
- `get_page_text` — Získání textu stránky
- `get_screenshot` — Získání snímku obrazovky
- `wait_for_element` — Čekání na výskyt prvku
- `get_element_info` — Získání informací o prvku
- `upload_file` — Nahrání souboru
- `get_browser_status` — Získání stavu prohlížeče
- `set_timeout` — Nastavení časového limitu
- `clear_session` — Vymazání relace prohlížeče

**Vlastnosti**:
- Nezávislá instance pro každou Křemíkovou Bytost
- Zcela izolované cookies a relace
- Zcela neviditelné pro uživatele (headless režim)
- Kompletní podpora JavaScript a CSS

**Příklad použití**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 26. Nástroj pracovních poznámek (WorkNoteTool)

**Název nástroje**: `work_note`

**Popis funkce**: Správa osobních pracovních poznámek Křemíkové Bytosti (soukromé, podobné deníku).

**Podporované operace**:
- `create` — Vytvoření poznámky
- `read` — Čtení poznámky
- `update` — Aktualizace poznámky
- `delete` — Smazání poznámky
- `list` — Seznam poznámek
- `search` — Vyhledávání poznámek
- `directory` — Generování obsahu

**Příklad použití**:
```json
{
  "action": "create",
  "summary": "Dokončení modulu uživatelské autentizace",
  "content": "## Detaily implementace\n\n- Použití JWT token\n- Podpora OAuth2",
  "keywords": "autentizace,JWT,OAuth2"
}
```

---

## Průběh volání nástroje

```
┌──────────┐
│   AI     │ Vrací tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Vyhledání a ověření oprávnění k použití nástroje
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Kontrola řetězce oprávnění
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Provedení operace přístupu k prostředkům
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ Přijetí výsledku nástroje, pokračování v uvažování
└──────────┘
```

## Ověřování oprávnění

Všechny provádění nástrojů procházejí řetězcem ověřování oprávnění:

1. **UserFrequencyCache** — mezipaměť častých uživatelských rozhodnutí (HighDeny má přednost před HighAllow)
2. **IPermissionCallback** — vlastní funkce zpětného volání oprávnění (Allowed/Denied/AskUser)
3. **Větev IsCurator** — Kurátor se ptá uživatele prostřednictvím IPermissionAskHandler; ne-Kurátor kontroluje GlobalACL, při žádném odpovídajícím pravidle je výchozí zamítnutí

## Vytvoření vlastního nástroje

### Krok 1: Implementace rozhraní ITool

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "Popis nástroje";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "Popis parametru" }
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

### Krok 2: Přidání do projektu

Umístěte soubor nástroje do adresáře `src/SiliconLife.Common/Tools/` (sdílené nástroje) nebo do adresáře `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (nástroje specifické pro verzi). `ToolManager` nástroj automaticky objeví a zaregistruje pomocí reflexe při spuštění.

### Krok 2a: Registrace nástroje prostřednictvím zásuvného modulu

Vlastní nástroje lze také registrovat prostřednictvím systému zásuvných modulů:

1. Implementujte rozhraní `ITool` v projektu zásuvného modulu
2. Zkompilujte DLL zásuvného modulu a vložte jej do adresáře zásuvných modulů
3. `ToolManager.ScanAllPluginAssemblies()` automaticky skenuje implementace ITool ve všech načtených zásuvných modulech
4. Nástroje zásuvných modulů podléhají stejnému systému oprávnění

### Krok 3: (Volitelné) Označení jako určené pouze pro Kurátora

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Přístupné pouze Kurátorem Křemíku
}
```

### Alternativa: Dovednosti a MCP nástroje

Kromě psaní tříd nástrojů v C# existují dva způsoby rozšíření bez nutnosti kompilace:

- **Dovednosti (Skill)**: Vytvořením kombinace „orchestrace nástrojů + šablona výzev" prostřednictvím Web UI nebo nástroje `skill`, vhodné pro zapouzdření častých pracovních postupů do znovupoužitelných schopností. Viz [Průvodce Křemíkovou Bytostí — Systém dovedností](silicon-being-guide.md#systém-dovedností).
- **MCP servery**: Po konfiguraci externích MCP serverů v Web UI jsou jejich nástroje automaticky injektovány ve formátu `mcp_{serverId}_{toolName}`, bez nutnosti psát jakýkoliv kód. Viz [Průvodce Web UI — Správa MCP](web-ui-guide.md).

## Osvědčené postupy

### 1. Vždy validujte parametry

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Chybí povinný parametr: required_param");
}
```

### 2. Elegantně zpracovávejte chyby

```csharp
try
{
    // Provedení operace
}
catch (Exception ex)
{
    Logger.Error($"Nástroj {Name} selhal: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Respektujte systém oprávnění

Nikdy neobcházejte kontrolu oprávnění. Vždy přistupujte k prostředkům prostřednictvím exekutoru:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
}
```

### 4. Poskytujte jasné popisy nástrojů

Pomozte AI pochopit, kdy a jak nástroj používat:

```csharp
public string Description => 
    "Používá se pro konverzi dat mezi různými kalendářními systémy." +
    "Vyžaduje parametry 'date', 'from_calendar' a 'to_calendar'.";
```

## Řešení problémů

### Nástroj nenalezen

**Problém**: AI se pokouší zavolat neexistující nástroj.

**Řešení**:
- Zkontrolujte, zda se název nástroje přesně shoduje
- Ověřte, že soubor nástroje je v adresáři `Tools/`
- Znovu sestavte projekt (`dotnet build`)

### Oprávnění zamítnuto

**Problém**: Provedení nástroje selhalo, vrácena chyba oprávnění.

**Řešení**:
- Zkontrolujte auditní protokol oprávnění
- Ověřte, že Křemíková Bytost má požadovaná oprávnění
- Zkontrolujte nastavení globálního ACL
- Pokud jde o Kurátora, zkontrolujte, zda je použit atribut `[SiliconManagerOnly]`

### Provedení nástroje vrací chybu

**Problém**: Nástroj se provedl, ale vrátil neúspěšný výsledek.

**Řešení**:
- Zkontrolujte chybovou zprávu vrácenou nástrojem
- Ověřte správný formát vstupních parametrů
- Zkontrolujte systémové protokoly pro podrobné informace o chybě
- Otestujte funkci nástroje nezávisle

## Další kroky

- 📚 Přečtěte [příručku architektury](architecture.md)
- 🛠️ Prohlédněte [vývojářskou příručku](development-guide.md)
- 🔒 Přečtěte o [systému oprávnění](permission-system.md)
- 🚀 Prohlédněte [příručku rychlého startu](getting-started.md)
