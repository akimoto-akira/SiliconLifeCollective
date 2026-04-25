# Průvodce Křemíkovou Bytostí

[English](../en/silicon-being-guide.md) | [中文文档](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md)

## Co je Křemíková Bytost?

Křemíková bytost je autonomní AI agent s:
- Vlastní identitou a osobností
- Schopností učit se a vyvíjet se
- Přístupem k nástrojům a zdrojům
- Dlouhodobou pamětí

## Architektura Tělo-Mozek

### Tělo (DefaultSiliconBeing)

Udržuje stav a detekuje scénáře:

```csharp
public class DefaultSiliconBeing : SiliconBeingBase
{
    public override async Task Tick()
    {
        // Kontrola nepřečtených zpráv
        // Detekce spuštěcích scénářů
        // Vytvoření ContextManager
        // Provedení jednoho kola
    }
}
```

### Mozek (ContextManager)

Zpracovává AI interakce:

1. Načtení souboru duše
2. Načtení historie chatu
3. Odeslání požadavku AI
4. Smyčka volání nástrojů
5. Persistování odpovědi

## Soubor Duše (soul.md)

Markdown soubor definující:

```markdown
# Jméno Bytosti

## Osobnost
Popis osobnosti

## Schopnosti
- Schopnost 1
- Schopnost 2

## Chování
- Pravidlo chování 1
- Pravidlo chování 2
```

## Životní Cyklus

### Vytvoření

1. Generování GUID
2. Vytvoření adresáře
3. Vytvoření soul.md
4. Registrace do SiliconBeingManager

### Provádění

```
Hlavní smyčka → Clock tick → SiliconBeingManager → SiliconBeingRunner → Bytost.Tick()
```

### Sebe-Evoluce

1. AI generuje nový kód
2. Kompilace v paměti
3. Bezpečnostní scanning
4. Atomická výměna instance
5. Persistování šifrovaného kódu

## Nástroje

Každá bytost má přístup k nástrojům:

| Nástroj | Použití |
|---------|---------|
| CalendarTool | Operace kalendáře |
| ChatTool | Komunikace s jinými bytostmi |
| DiskTool | Operace se soubory |
| MemoryTool | Správa paměti |
| NetworkTool | Síťové požadavky |
| SystemTool | Systémové informace |
| TaskTool | Správa úkolů |
| TimerTool | Nastavení časovačů |

## Paměť

### Typy Paměti

- **Krátkodobá** - Nedávné interakce
- **Dlouhodobá** - Důležité informace
- **Komprimovaná** - Sloučené podobné paměti

### Operace Paměti

```json
{
  "action": "store",
  "type": "long_term",
  "content": "Důležitá informace"
}
```

## Úkoly a Časovače

### Úkoly

- Jednorázové úkoly
- DAG závislé úkoly
- Prioritní scheduling

### Časovače

- Jednorázové alarmy
- Periodické časovače
- Přesnost na milisekundy

## Multi-Bytost Spolupráce

### Komunikace

```csharp
// Odeslání zprávy jiné bytosti
await ChatTool.SendMessageAsync(
    targetBeingId,
    "Ahoj, potřebuji pomoc!"
);
```

### Broadcast Channel

Systémová oznámení pro všechny přihlášené bytosti.

## Best Practices

1. **Definujte jasnou osobnost** v soul.md
2. **Používejte nástroje efektivně**
3. **Ukládejte důležité informace** do paměti
4. **Monitorujte oprávnění** v audit logu
5. **Testujte sebe-evoluci** pečlivě

## Řešení Problémů

### Bytost Neodpovídá

- Zkontrolujte, zda běží
- Podívejte se na logy
- Ověřte oprávnění

### Chyby Kompilace

- Zkontrolujte syntaxi kódu
- Ověřte dědění SiliconBeingBase
- Zkontrolujte povolené assembly

### Paměť Plná

- Komprimujte paměti
- Smažte staré záznamy
- Zvyšte limity

## Další Informace

- [Architektura](architecture.md)
- [Bezpečnost](security.md)
- [API Reference](api-reference.md)
- [Vývojový Průvodce](development-guide.md)

## Systém Pracovních Poznámek

### Přehled

Pracovní poznámky jsou osobní deníkový systém pro křemíkové bytosti, používající stránkový design k zaznamenávání pracovního postupu, poznámek k učení, projektových poznámek atd.

### Funkce

- **Správa stránek**: Každá poznámka je nezávislá stránka, přístupná podle čísla stránky
- **Podpora Markdown**: Obsah podporuje formát Markdown (text, seznamy, tabulky, bloky kódu)
- **Indexace klíčových slov**: Podpora přidávání klíčových slov k poznámkám pro snadné vyhledávání
- **Funkce shrnutí**: Každá poznámka má krátké shrnutí pro rychlé procházení
- **Generování adresáře**: Může generovat přehled adresáře všech poznámek, pomáhá pochopit celkový kontext
- **Časová razítka**: Automatické zaznamenávání časů vytvoření a aktualizace
- **Ve výchozím nastavení soukromé**: Přístup pouze pro bytost (kurátor může spravovat)

### Případy Použití

1. **Záznam Postupu Projektu**
   ```
   Shrnutí: Modul ověřování uživatele dokončen
   Obsah: Implementováno ověřování JWT tokenu, integrace OAuth2, mechanismus obnovovacího tokenu
   Klíčová slova: authentication,JWT,OAuth2
   ```

2. **Poznámky k Učení**
   ```
   Shrnutí: Učení osvědčených postupů C# async programování
   Obsah: Upozornění k použití async/await, scénáře použití ConfigureFlags...
   Klíčová slova: C#,async,best practices
   ```

3. **Zápisy ze Schůzek**
   ```
   Shrnutí: Schůzka o požadavcích na produkt
   Obsah: Diskutovány požadavky na nové funkce, určen plán implementace...
   Klíčová slova: product,requirements,meeting
   ```

### Použití Prostřednictvím Nástrojů

Bytosti mohou spravovat pracovní poznámky pomocí nástroje `work_note`:

```json
// Vytvořit poznámku
{
  "action": "create",
  "summary": "Modul ověřování uživatele dokončen",
  "content": "## Detaily implementace\n\n- Použití JWT tokenu\n- Podpora OAuth2",
  "keywords": "authentication,JWT,OAuth2"
}

// Číst poznámku
{
  "action": "read",
  "page_number": 1
}

// Vyhledat poznámky
{
  "action": "search",
  "keyword": "authentication",
  "max_results": 10
}
```

### Správa Prostřednictvím Web UI

1. Přejděte na **Správa Bytostí** → Vyberte bytost
2. Klikněte na záložku **Pracovní Poznámky**
3. Lze zobrazit, vyhledávat, upravovat poznámky
4. Podpora náhledu Markdown

## Systém Sítě Znalostí

### Přehled

Síť znalostí je systém reprezentace a správy znalostí založený na struktuře trojice (subjekt-predikát-objekt), používaný k ukládání a správě strukturovaných znalostí.

### Základní Koncepty

#### Struktura Trojice

```
Subjekt (Subject) --Predikát (Predicate)--> Objekt (Object)
```

**Příklady**:
- `Python` --`is_a`--> `programming_language`
- `Peking` --`capital_of`--> `China`
- `Voda` --`boiling_point`--> `100°C`

#### Důvěryhodnost

Každá trojice znalostí má skóre důvěryhodnosti (0.0-1.0), udávající spolehlivost znalosti:
- `1.0`: Absolutně jisté (matematické věty atd.)
- `0.8-0.99`: Vysoká důvěryhodnost (ověřená fakta atd.)
- `0.5-0.79`: Střední důvěryhodnost (odvození nebo hypotézy atd.)
- `<0.5`: Nízká důvěryhodnost (domněnky nebo neověřené informace atd.)

#### Systém Štítků

Podpora přidávání štítků k trojicím pro snadnou klasifikaci a vyhledávání:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### Operace se Znalostmi

#### 1. Přidat Znalost

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

#### 2. Dotaz na Znalost

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. Vyhledat Znalost

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. Objevit Cestu Znalostí

Najít asociativní cesty mezi dvěma koncepty:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Vrátí:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Validovat Znalost

Kontrola platnosti a konzistence znalosti:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Statistiky Znalostí

Získat celkové statistiky sítě znalostí:
```json
{
  "action": "stats"
}
```

Vrátí:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Případy Použití

1. **Ukládání Faktů**
   - Ukládání objektivních faktů a common sense
   - Příklad: `Země` --`is_a`--> `Planeta`

2. **Vztahy Konceptů**
   - Zaznamenávání vztahů mezi koncepty
   - Příklad: `Dědičnost` --`is_a`--> `Koncept objektově orientovaného programování`

3. **Akumulace Učení**
   - Bytosti neustále akumulují znalosti prostřednictvím učení
   - Vytváření strukturovaných systémů znalostí

4. **Podpora Odvozování**
   - Objevování nepřímých vztahů prostřednictvím cest znalostí
   - Podpora odvozování a rozhodování založeného na znalostech

### Správa Prostřednictvím Web UI

1. Přejděte na stránku **Síť Znalostí**
2. Zobrazit statistiky znalostí
3. Vyhledávat a procházet znalosti
4. Vizualizace grafu vztahů znalostí (plánováno)
