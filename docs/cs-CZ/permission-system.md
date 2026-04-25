# Systém Oprávnění

[English](../en/permission-system.md) | [中文文档](../zh-CN/permission-system.md) | [Texto](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md)

## Přehled

Systém oprávnění Silicon Life Collective používá **5-úrovňový řetězec ověřování** k zajištění, že všechny AI iniciované operace jsou autorizovány.

## Typy Oprávnění

| Typ | Popis |
|------|-------------|
| `NetworkAccess` | Odchozí HTTP/HTTPS požadavky |
| `CommandLine` | Provádění příkazů shellu |
| `FileAccess` | Operace se soubory a adresáři |
| `Function` | Volání citlivých funkcí |
| `DataAccess` | Přístup k systémovým nebo uživatelským datům |

## 5-Úrovňový Řetězec

1. **IsCurator** - Kurátor má vždy oprávnění (zkratka)
2. **UserFrequencyCache** - Vysoká frekvence schválení/zamítnutí uživatelem
3. **GlobalACL** - Globální seznam řízení přístupu
4. **IPermissionCallback** - Vlastní callback funkce oprávnění
5. **IPermissionAskHandler** - Dotaz uživatele v reálném čase

## Výsledky Oprávnění

| Výsledek | Popis |
|----------|-------|
| `Allowed` | Operace je povolena |
| `Denied` | Operace je zamítnuta |
| `AskUser` | Vyžadováno potvrzení uživatele |

## Soukromý Permission Manager

Každá křemíková bytost má svůj vlastní soukromý Permission Manager:

- Stav oprávnění není sdílen mezi bytostmi
- Každá bytost může mít vlastní pravidla
- Callback může být dynamicky změněn prostřednictvím kompilace

## GlobalACL

Sdílená tabulka pravidel persistovaná do storage:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

**Pravidla**:
- Pravidla jsou vyhodnocována v pořadí
- První shoda vyhrává
- Pouze Kurátor může upravovat GlobalACL

## User Frequency Cache

Dvě cache per-bytost (pouze v paměti):

- **HighAllow** - Zdroje často povolované uživatelem
- **HighDeny** - Zdroje často zamítané uživatelem

**Vlastnosti**:
- Prefix matching
- Konfigurovatelná expirace
- Ztrácejí se při restartu
- Výběr uživatele (ne automatická detekce)

## Permission Callback

### Výchozí Callback

Vestavěná implementace s:
- Síťový whitelist/blacklist
- Klasifikace CLI příkazů
- Bezpečnostní pravidla cest souborů

### Custom Callback

Bytosti mohou kompilovat vlastní callback:

```csharp
public PermissionResult CustomCallback(
    PermissionType type, 
    string resourcePath, 
    Guid callerId)
{
    // Vlastní logika oprávnění
    if (resourcePath.StartsWith("safe:"))
        return PermissionResult.Allowed;
    
    return PermissionResult.AskUser;
}
```

## Dotaz Uživatele

### Web UI

Interaktivní karta zobrazující:
- Typ a cestu zdroje
- Popis akce
- Tlačítka Povolit/Zamítnout
- Checkboxy "Vždy povolit/Zamítnout"

### IM Platformy

Náhodné 6místné kódy:
- Allow kód - pro autorizaci
- Deny kód - pro zamítnutí
- Jednorázové kódy

## Audit Log

Všechna rozhodnutí jsou zaznamenána:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows | Source:GlobalACL
```

## Best Practices

1. **Vždy kontrolujte oprávnění** před provedením operace
2. **Používejte HighAllow/HighDeny** pro často používané zdroje
3. **Udržujte GlobalACL** aktuální
4. **Monitorujte audit log** pro detekci anomálií
5. **Nastavte appropriate timeout** pro dotazy uživatele
