# Reference Nástrojů

[English](../en/tools-reference.md) | [中文文档](../zh-CN/tools-reference.md) | [Texto](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md)

## Přehled

Nástroje jsou automaticky objevovány prostřednictvím reflexe a registrovány do ToolManageru každé bytosti.

## Built-in Nástroje

### CalendarTool

Operace kalendáře.

**Operace**:
- `now` - Aktuální datum
- `format` - Formátování data
- `add_days` - Přidání dnů
- `diff` - Rozdíl mezi daty
- `list_calendars` - Seznam kalendářů
- `get_components` - Komponenty data
- `get_now_components` - Komponenty aktuálního data
- `convert` - Převod mezi kalendáři

**Podporované kalendáře**: 32 kalendářních systémů

### ChatTool

Komunikace mezi bytostmi.

**Operace**:
- `send_message` - Odeslání zprávy
- `get_messages` - Získání zpráv
- `create_session` - Vytvoření relace
- `list_sessions` - Seznam relací

### ConfigTool

Správa konfigurace (pouze Kurátor).

**Operace**:
- `read` - Čtení konfigurace
- `update` - Aktualizace konfigurace
- `reset` - Reset na výchozí

### CuratorTool

Nástroje správy Kurátora (pouze Kurátor).

**Operace**:
- `list_beings` - Seznam bytostí
- `create_being` - Vytvoření bytosti
- `get_code` - Získání kódu bytosti
- `reset` - Reset bytosti

### DiskTool

Operace se soubory.

**Operace**:
- `read_file` - Čtení souboru
- `write_file` - Zápis souboru
- `list_directory` - Výpis adresáře
- `delete` - Smazání
- `count_lines` - Počet řádků
- `read_lines` - Čtení řádků
- `clear_file` - Vymazání souboru
- `replace_lines` - Nahrazení řádků
- `replace_text` - Nahrazení textu
- `replace_text_all` - Nahrazení všeho textu
- `list_drives` - Seznam disků
- `search_files` - Vyhledávání souborů
- `search_content` - Vyhledávání obsahu

### MemoryTool

Správa dlouhodobé paměti.

**Operace**:
- `store` - Uložení paměti
- `retrieve` - Získání paměti
- `search` - Vyhledávání pamětí
- `compress` - Komprese pamětí
- `delete` - Smazání paměti
- `stats` - Statistiky pamětí

### NetworkTool

Síťové požadavky.

**Operace**:
- `http_get` - HTTP GET požadavek
- `http_post` - HTTP POST požadavek
- `download` - Stažení souboru

### SystemTool

Systémové informace.

**Operace**:
- `get_system_info` - Informace o systému
- `get_process_list` - Seznam procesů
- `find_process` - Vyhledávání procesů (wildcard podpora)
- `resource_usage` - Využití zdrojů

### TaskTool

Správa úkolů.

**Operace**:
- `create` - Vytvoření úkolu
- `update` - Aktualizace úkolu
- `complete` - Dokončení úkolu
- `list` - Seznam úkolů
- `delete` - Smazání úkolu

### TimerTool

Správa časovačů.

**Operace**:
- `set_alarm` - Nastavení alarmu
- `set_timer` - Nastavení časovače
- `cancel` - Zrušení časovače
- `list` - Seznam časovačů

### TokenAuditTool

Audit použití tokenů (pouze Kurátor).

**Operace**:
- `query` - Dotaz použití tokenů
- `summary` - Souhrn použití
- `export` - Export dat

## Vytvoření Custom Nástroje

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    public string Description => "Popis nástroje";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Implementace
        return new ToolResult 
        { 
            Success = true, 
            Output = "výsledek" 
        };
    }
}
```

## [SiliconManagerOnly] Atribut

Označuje nástroje přístupné pouze Kurátorovi:

```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### 16. Nástroj Síťe Znalostí

**Název**: `knowledge`

**Popis**: Nástroj pro operace se sítí znalostí, pro přidávání, dotazování, aktualizaci, mazání a vyhledávání trojic znalostí.

**Akce**: `add`, `query`, `update`, `delete`, `search`, `get_path`, `validate`, `stats`

**Parametry** (add - přidat znalost):
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**Parametry** (query - dotaz na znalost):
```json
{
  "action": "query",
  "subject": "Python",
  "predicate": "is_a"
}
```

**Parametry** (search - vyhledat znalost):
```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

**Parametry** (get_path - získat cestu znalostí):
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

**Parametry** (stats - statistiky):
```json
{
  "action": "stats"
}
```

**Funkce**:
- Reprezentace znalostí založená na struktuře trojice (subjekt-predikát-objekt)
- Podpora skóre důvěryhodnosti znalostí
- Podpora klasifikace a vyhledávání podle štítků
- Podpora objevování cest znalostí (asociativní cesty mezi dvěma body)
- Podpora validace znalostí a kontroly integrity
- Trvalé úložiště do souborového systému

**Oprávnění**: Všechny bytosti mohou používat.

### 17. Nástroj Pracovních Poznámek

**Název**: `work_note`

**Popis**: Správa pracovních poznámek pro křemíkové bytosti. Pracovní poznámky používají stránkový design, podobný osobnímu deníku (ve výchozím nastavení soukromé).

**Akce**: `create`, `read`, `update`, `delete`, `list`, `directory`, `search`

**Parametry** (create - vytvořit poznámku):
```json
{
  "action": "create",
  "summary": "Modul ověřování uživatele dokončen",
  "content": "## Detaily implementace\n\n- Použití JWT tokenu\n- Podpora OAuth2\n- Přidán mechanismus obnovovacího tokenu",
  "keywords": "authentication,JWT,OAuth2"
}
```

**Parametry** (read - číst poznámku):
```json
{
  "action": "read",
  "page_number": 1
}
```

Nebo použít note_id:
```json
{
  "action": "read",
  "note_id": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Parametry** (update - aktualizovat poznámku):
```json
{
  "action": "update",
  "page_number": 1,
  "content": "## Aktualizovaný obsah\n\nPřidány unit testy",
  "summary": "Modul ověřování uživatele a testy dokončeny"
}
```

**Parametry** (list - seznam všech poznámek):
```json
{
  "action": "list"
}
```

**Parametry** (directory - generovat adresář poznámek):
```json
{
  "action": "directory"
}
```

**Parametry** (search - vyhledat poznámky):
```json
{
  "action": "search",
  "keyword": "authentication",
  "max_results": 10
}
```

**Funkce**:
- Stránkový design, každá stránka spravována samostatně
- Podpora shrnutí, obsahu, klíčových slov
- Podpora vyhledávání podle klíčových slov
- Podpora generování přehledu adresáře (pro porozumění kontextu)
- Podpora formátu Markdown (text, seznamy, tabulky, bloky kódu)
- Automatické zaznamenávání časových razítek
- Ve výchozím nastavení soukromé, přístup pouze pro bytost

**Oprávnění**: Bytosti přistupují ke svým vlastním pracovním poznámkám, kurátor může spravovat všechny poznámky.

## Návratové Hodnoty

### ToolResult

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}
```

## Best Practices

1. **Vždy kontrolujte oprávnění**
2. **Poskytujte jasné chybové zprávy**
3. **Validujte vstupní parametry**
4. **Používejte async/await**
5. **Dokumentujte váš nástroj**
