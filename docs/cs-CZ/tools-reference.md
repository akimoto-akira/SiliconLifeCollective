# Reference Nástrojů

Tento dokument poskytuje podrobné informace o všech vestavěných nástrojích platformy Silicon Life Collective.

[English](../en/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | **Čeština**

## Přehled

Systém nástrojů umožňuje silikonovým bytostem interagovat s vnějším světem prostřednictvím standardizovaného rozhraní. Každý nástroj implementuje rozhraní `ITool` a je automaticky objeven a registrován `ToolManagerem` pomocí reflexe.

### Kategorie Nástrojů

- **Nástroje pro Správu Systému** — Konfigurace, oprávnění, dynamická kompilace
- **Komunikační Nástroje** — Chat, síťové požadavky
- **Nástroje pro Ukládání Dat** — Operace s disky, databáze, paměť, pracovní poznámky
- **Nástroje pro Správu Času** — Kalendář, časovače, úkoly
- **Vývojové Nástroje** — Spouštění kódu, dotazování logů
- **Utility Nástroje** — Systémové informace, audit tokenů, nápověda, znalostní síť
- **Nástroje Prohlížeče** — Automatizace WebView prohlížeče

---

## Seznam Vestavěných Nástrojů

### 1. Kalendářový Nástroj (CalendarTool)

**Název nástroje**: `calendar`

**Popis funkce**: Podpora převodu a výpočtu data pro 32 kalendářových systémů.

**Podporované operace**:
- `now` — Získat aktuální čas
- `format` — Formátovat datum
- `add_days` — Přičíst/odečíst dny
- `diff` — Vypočítat rozdíl dat
- `list_calendars` — Vypsat všechny podporované kalendáře
- `get_components` — Získat komponenty data
- `get_now_components` — Získat komponenty aktuálního času
- `convert` — Převod mezi kalendářovými systémy

**Podporované kalendářové systémy** (32 typů):
- Gregoriánský kalendář (Gregorian)
- Čínský lunární kalendář (Chinese Lunar)
- Čínský historický kalendář (Chinese Historical) — cyklické letopočty, éry panovníků
- Islámský kalendář (Islamic)
- Hebrejský kalendář (Hebrew)
- Japonský kalendář (Japanese)
- Perský kalendář (Persian)
- Mayský kalendář (Mayan)
- Buddhistský kalendář (Buddhist)
- Tibetský kalendář (Tibetan)
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

### 2. Chatovací Nástroj (ChatTool)

**Název nástroje**: `chat`

**Popis funkce**: Správa chatovacích relací a odesílání zpráv.

**Podporované operace**:
- `send_message` — Odeslat zprávu
- `get_messages` — Získat historické zprávy
- `create_group` — Vytvořit skupinový chat
- `add_member` — Přidat člena skupiny
- `remove_member` — Odebrat člena skupiny
- `get_chat_info` — Získat informace o chatu
- `terminate_chat` — Ukončit chat (přečteno bez odpovědi)

**Příklad použití**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "Ahoj, pojďme spolupracovat!"
}
```

---

### 3. Konfigurační Nástroj (ConfigTool)

**Název nástroje**: `config`

**Popis funkce**: Čtení a úprava systémové konfigurace.

**Podporované operace**:
- `read` — Číst konfigurační položku
- `write` — Zapisovat konfigurační položku
- `list` — Vypsat všechny konfigurace
- `get_ai_config` — Získat konfiguraci AI klienta
- `set_ai_config` — Nastavit konfiguraci AI klienta

**Příklad použití**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Nástroj Kurátora (CuratorTool) 🔒

**Název nástroje**: `curator`

**Požadavek na oprávnění**: Pouze pro silikonové kurátory

**Popis funkce**: Systémový správcovský nástroj vyhrazený pro silikonové kurátory.

**Podporované operace**:
- `create_being` — Vytvořit novou silikonovou bytost
- `list_beings` — Vypsat všechny silikonové bytosti
- `get_being_info` — Získat informace o bytosti
- `assign_task` — Přiřadit úkol
- `manage_permissions` — Spravovat oprávnění

**Příklad použití**:
```json
{
  "action": "create_being",
  "name": "Asistent",
  "soul_file": "assistant_soul.md"
}
```

---

### 5. Databázový Nástroj (DatabaseTool)

**Název nástroje**: `database`

**Popis funkce**: Dotazování a operace se strukturovanou databází.

**Podporované operace**:
- `query` — Dotazovat data
- `insert` — Vložit data
- `update` — Aktualizovat data
- `delete` — Smazat data
- `create_table` — Vytvořit tabulku
- `list_tables` — Vypsat všechny tabulky

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

### 6. Diskový Nástroj (DiskTool)

**Název nástroje**: `disk`

**Popis funkce**: Operace se souborovým systémem a lokální vyhledávání.

**Podporované operace**:
- `read` — Číst soubor
- `write` — Zapisovat soubor
- `list` — Vypsat adresář
- `delete` — Smazat soubor
- `create_directory` — Vytvořit adresář
- `search_files` — Vyhledat soubory
- `search_content` — Vyhledat obsah souborů
- `count_lines` — Spočítat řádky
- `read_lines` — Číst zadané řádky
- `replace_text` — Nahradit text

**Požadavek na oprávnění**: `disk:read`, `disk:write`

**Příklad použití**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Nástroj Dynamické Kompilace (DynamicCompileTool) 🔒

**Název nástroje**: `compile`

**Popis funkce**: Dynamická kompilace C# kódu (pro sebevývoj silikonových bytostí).

**Podporované operace**:
- `compile_class` — Kompilovat třídu
- `compile_callback` — Kompilovat funkci zpětného volání oprávnění
- `validate_code` — Ověřit bezpečnost kódu

**Bezpečnostní mechanismy**:
- Kontrola referencí při kompilaci (vyloučení nebezpečných sestav)
- Statické skenování kódu za běhu
- AES-256 šifrované ukládání

**Příklad použití**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Nástroj Spouštění Kódu (ExecuteCodeTool) 🔒

**Název nástroje**: `execute_code`

**Požadavek na oprávnění**: Pouze pro silikonové kurátory

**Popis funkce**: Kompilace a spuštění fragmentů C# kódu.

**Podporované operace**:
- `run_script` — Spustit kódový skript

**Příklad použití**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. Nástroj Nápovědy (HelpTool)

**Název nástroje**: `help`

**Popis funkce**: Získání nápovědní dokumentace a uživatelských příruček.

**Podporované operace**:
- `get_topics` — Získat seznam nápovědních témat
- `get_topic` — Získat detaily konkrétního tématu
- `search` — Vyhledat nápovědní dokumenty

**Příklad použití**:
```json
{
  "action": "get_topics"
}
```

---

### 10. Nástroj Znalostní Sítě (KnowledgeTool)

**Název nástroje**: `knowledge`

**Popis funkce**: Operace se znalostním grafem (založeno na triplech: subjekt-vztah-objekt).

**Podporované operace**:
- `add` — Přidat znalostní triple
- `query` — Dotazovat znalosti
- `update` — Aktualizovat znalosti
- `delete` — Smazat znalosti
- `search` — Vyhledat znalosti
- `get_path` — Získat cestu znalostí
- `validate` — Ověřit znalost
- `stats` — Získat statistiky

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

### 11. Logovací Nástroj (LogTool)

**Název nástroje**: `log`

**Popis funkce**: Dotazování operační historie a historie konverzací.

**Podporované operace**:
- `query_logs` — Dotazovat systémové logy
- `query_conversations` — Dotazovat historii konverzací
- `get_stats` — Získat statistiky logů

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

### 12. Paměťový Nástroj (MemoryTool)

**Název nástroje**: `memory`

**Popis funkce**: Správa dlouhodobé a krátkodobé paměti silikonových bytostí.

**Podporované operace**:
- `read` — Číst paměť
- `write` — Zapisovat paměť
- `search` — Vyhledat paměť
- `delete` — Smazat paměť
- `list` — Vypsat paměť
- `get_stats` — Získat statistiky paměti
- `compress` — Komprimovat paměť

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

### 13. Síťový Nástroj (NetworkTool)

**Název nástroje**: `network`

**Popis funkce**: Iniciování HTTP/HTTPS požadavků.

**Podporované operace**:
- `get` — GET požadavek
- `post` — POST požadavek
- `put` — PUT požadavek
- `delete` — DELETE požadavek
- `download` — Stáhnout soubor
- `upload` — Nahrát soubor

**Požadavek na oprávnění**: `network:http`

**Příklad použití**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. Nástroj Oprávnění (PermissionTool) 🔒

**Název nástroje**: `permission`

**Požadavek na oprávnění**: Pouze pro silikonové kurátory

**Popis funkce**: Správa oprávnění a seznamů řízení přístupu.

**Podporované operace**:
- `query_permission` — Dotazovat oprávnění
- `manage_acl` — Spravovat globální ACL
- `get_callback` — Získat funkci zpětného volání oprávnění
- `set_callback` — Nastavit funkci zpětného volání oprávnění

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

### 15. Projektový Nástroj (ProjectTool)

**Název nástroje**: `project`

**Popis funkce**: Správa projektových pracovních prostorů.

**Podporované operace**:
- `create` — Vytvořit projekt
- `list` — Vypsat projekty
- `get_info` — Získat informace o projektu
- `update` — Aktualizovat projekt
- `archive` — Archivovat projekt

**Příklad použití**:
```json
{
  "action": "create",
  "name": "Můj Projekt",
  "description": "Popis projektu"
}
```

---

### 16. Nástroj Projektových Úkolů (ProjectTaskTool)

**Název nástroje**: `project_task`

**Popis funkce**: Správa projektových úkolů.

**Podporované operace**:
- `create` — Vytvořit úkol
- `list` — Vypsat úkoly
- `update` — Aktualizovat úkol
- `complete` — Dokončit úkol
- `get_stats` — Získat statistiky úkolů

**Příklad použití**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Popis úkolu k dokončení",
  "priority": 5
}
```

---

### 17. Nástroj Projektových Pracovních Poznámek (ProjectWorkNoteTool)

**Název nástroje**: `project_work_note`

**Popis funkce**: Správa projektových pracovních poznámek (veřejné, podobné pracovnímu zápisníku).

**Podporované operace**:
- `create` — Vytvořit poznámku
- `read` — Číst poznámku
- `update` — Aktualizovat poznámku
- `delete` — Smazat poznámku
- `list` — Vypsat poznámky
- `search` — Vyhledat poznámky
- `directory` — Generovat obsah

**Příklad použití**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Dokončen modul autentizace uživatelů",
  "content": "## Implementační detaily\n\n- Použití JWT tokenu",
  "keywords": "autentizace,JWT"
}
```

---

### 18. Systémový Nástroj (SystemTool)

**Název nástroje**: `system`

**Popis funkce**: Získání systémových informací a využití zdrojů.

**Podporované operace**:
- `info` — Získat systémové informace
- `resource_usage` — Získat využití zdrojů
- `find_process` — Najít proces
- `list_beings` — Vypsat silikonové bytosti

**Příklad použití**:
```json
{
  "action": "info"
}
```

---

### 19. Úkolový Nástroj (TaskTool)

**Název nástroje**: `task`

**Popis funkce**: Správa osobních úkolů silikonových bytostí.

**Podporované operace**:
- `create` — Vytvořit úkol
- `list` — Vypsat úkoly
- `update` — Aktualizovat úkol
- `complete` — Dokončit úkol
- `delete` — Smazat úkol
- `get_dependencies` — Získat závislosti

**Příklad použití**:
```json
{
  "action": "create",
  "description": "Zkontrolovat kód",
  "priority": 5
}
```

---

### 20. Časovačový Nástroj (TimerTool)

**Název nástroje**: `timer`

**Popis funkce**: Vytváření a správa časovačů.

**Podporované operace**:
- `create` — Vytvořit časovač
- `list` — Vypsat časovače
- `delete` — Smazat časovač
- `pause` — Pozastavit časovač
- `resume` — Obnovit časovač
- `get_execution_history` — Získat historii provádění

**Příklad použití**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "Každou hodinu připomenout"
}
```

---

### 21. Nástroj Audit Tokenů (TokenAuditTool) 🔒

**Název nástroje**: `token_audit`

**Požadavek na oprávnění**: Pouze pro silikonové kurátory

**Popis funkce**: Dotazování a shrnutí využití AI tokenů.

**Podporované operace**:
- `get_usage` — Získat statistiky využití tokenů
- `get_by_being` — Získat využití podle bytosti
- `get_by_model` — Získat využití podle modelu
- `get_trend` — Získat trend využití
- `export` — Exportovat data

**Příklad použití**:
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. Nástroj WebView Prohlížeče (WebViewBrowserTool)

**Název nástroje**: `webview`

**Popis funkce**: Automatizace prohlížeče založená na Playwright.

**Podporované operace**:
- `open_browser` — Otevřít prohlížeč
- `close_browser` — Zavřít prohlížeč
- `navigate` — Navigovat na URL
- `click` — Kliknout na element
- `input` — Vložit text
- `get_page_text` — Získat text stránky
- `get_screenshot` — Získat screenshot
- `execute_script` — Spustit JavaScript
- `wait_for_element` — Čekat na výskyt elementu
- `get_browser_status` — Získat stav prohlížeče

**Vlastnosti**:
- Nezávislá instance pro každou silikonovou bytost
- Plně izolované cookies a relace
- Plně neviditelné pro uživatele (headless mód)
- Plná podpora JavaScriptu a CSS

**Příklad použití**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 23. Nástroj Pracovních Poznámek (WorkNoteTool)

**Název nástroje**: `work_note`

**Popis funkce**: Správa osobních pracovních poznámek silikonových bytostí (soukromé, podobné deníku).

**Podporované operace**:
- `create` — Vytvořit poznámku
- `read` — Číst poznámku
- `update` — Aktualizovat poznámku
- `delete` — Smazat poznámku
- `list` — Vypsat poznámky
- `search` — Vyhledat poznámky
- `directory` — Generovat obsah

**Příklad použití**:
```json
{
  "action": "create",
  "summary": "Dokončen modul autentizace uživatelů",
  "content": "## Implementační detaily\n\n- Použití JWT tokenu\n- Podpora OAuth2",
  "keywords": "autentizace,JWT,OAuth2"
}
```

---

## Proces Volání Nástrojů

```
┌──────────┐
│   AI     │ Vrátí tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Vyhledá a ověří použití nástroje
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Zkontroluje řetězec oprávnění
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Provede operaci přístupu ke zdroji
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ Přijme výsledek nástroje, pokračuje v myšlení
└──────────┘
```

## Ověřování Oprávnění

Všechna provádění nástrojů procházejí 5úrovňovým řetězcem oprávnění:

1. **IsCurator** — Silikonový kurátor obchází všechny kontroly
2. **UserFrequencyCache** — Cache vysoké frekvence povolení/zamítnutí uživatele
3. **GlobalACL** — Globální seznam řízení přístupu
4. **IPermissionCallback** — Vlastní funkce zpětného volání oprávnění
5. **IPermissionAskHandler** — Dotazování uživatele

## Vytvoření Vlastního Nástroje

### Krok 1: Implementace Rozhraní ITool

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

### Krok 2: Přidání do Projektu

Umístěte soubor nástroje do adresáře `src/SiliconLife.Default/Tools/`. `ToolManager` jej automaticky objeví a zaregistruje při spuštění pomocí reflexe.

### Krok 3: (Volitelné) Označení jako Vyhrazené pro Kurátora

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Přístupné pouze silikonovému kurátorovi
}
```

## Nejlepší Praktiky

### 1. Vždy Ověřujte Parametry

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Chybí požadovaný parametr: required_param");
}
```

### 2. Elegantní Zpracování Chyb

```csharp
try
{
    // Provést operaci
}
catch (Exception ex)
{
    Logger.Error($"Nástroj {Name} selhal: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Respektujte Systém Oprávnění

Nikdy neobcházejte kontroly oprávnění. Vždy přistupujte ke zdrojům prostřednictvím exekutorů:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. Poskytujte Jasné Popisy Nástrojů

Pomozte AI pochopit kdy a jak nástroj použít:

```csharp
public string Description => 
    "Používá se k převodu dat mezi různými kalendářovými systémy." +
    "Vyžaduje parametry 'date', 'from_calendar' a 'to_calendar'.";
```

## Řešení Problémů

### Nástroj Nenalezen

**Problém**: AI se pokouší volat neexistující nástroj.

**Řešení**:
- Zkontrolujte, zda název nástroje přesně odpovídá
- Ověřte, že soubor nástroje je v adresáři `Tools/`
- Znovu sestavte projekt (`dotnet build`)

### Oprávnění Odmítnuto

**Problém**: Provádění nástroje selhalo s chybou oprávnění.

**Řešení**:
- Zkontrolujte auditní logy oprávnění
- Ověřte, že silikonová bytost má požadované oprávnění
- Zkontrolujte nastavení globálního ACL
- Pokud jste kurátor, zkontrolujte použití atributu `[SiliconManagerOnly]`

### Nástroj Vrací Chybu

**Problém**: Nástroj se provedl, ale vrátil výsledek selhání.

**Řešení**:
- Zkontrolujte chybovou zprávu vrácenou nástrojem
- Ověřte, že vstupní parametry jsou správně formátovány
- Prohlédněte systémové logy pro podrobné informace o chybě
- Otestujte funkčnost nástroje samostatně

## Další Kroky

- 📚 Přečtěte si [Průvodce architekturou](architecture.md)
- 🛠️ Podívejte se na [Vývojářskou příručku](development-guide.md)
- 🔒 Pochopte [Systém oprávnění](permission-system.md)
- 🚀 Podívejte se na [Průvodce rychlým startem](getting-started.md)
