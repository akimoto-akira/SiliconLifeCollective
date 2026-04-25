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
