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
