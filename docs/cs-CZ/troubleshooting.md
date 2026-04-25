# Řešení Problémů

[English](../en/troubleshooting.md) | [中文文档](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md)

## Časté Problémy

### Ollama Připojení

#### Problém: Connection Refused

**Chyba**: `Unable to connect to http://localhost:11434`

**Řešení**:
```bash
# Zkontrolujte, zda Ollama běží
ollama list

# Pokud Ollama neběží, spusťte jej
ollama serve
```

#### Problém: Model Not Found

**Chyba**: `model "qwen2.5:7b" not found`

**Řešení**:
```bash
# Stáhněte požadovaný model
ollama pull qwen2.5:7b

# Zkontrolujte dostupné modely
ollama list
```

### Port Already in Use

#### Problém: Port 8080 Obsazen

**Chyba**: `HttpListenerException: Address already in use`

**Řešení**:

**Windows**:
```bash
# Najděte proces používající port 8080
netstat -ano | findstr :8080

# Ukončete proces
taskkill /PID <PID> /F
```

**Linux/Mac**:
```bash
# Najděte a ukončete proces
lsof -ti:8080 | xargs kill -9
```

**Alternativa**: Změňte port v konfiguraci.

### Kompilace Selhala

#### Problém: Build Errors

**Chyba**: Různé chyby při `dotnet build`

**Řešení**:
```bash
# Vyčistěte build artefakty
dotnet clean

# Obnovte dependencies
dotnet restore

# Zkuste znovu build
dotnet build
```

#### Problém: Missing Dependencies

**Chyba**: `NU1101: Unable to find package`

**Řešení**:
```bash
# Vyčistěte NuGet cache
dotnet nuget locals all --clear

# Obnovte dependencies
dotnet restore
```

### Permission Issues

#### Problém: Permission Denied

**Chyba**: Bytost nemůže přistoupit k zdroji

**Diagnostika**:
1. Zkontrolujte audit log ve Web UI
2. Ověřte, zda je zdroj v High Deny cache
3. Zkontrolujte GlobalACL pravidla

**Řešení**:
- Pokud je to Kurátor: Zkontrolujte `IsCurator` flag
- Pro běžné bytosti: Přidejte zdroj do High Allow cache nebo upravte GlobalACL

### Dynamická Kompilace Selhala

#### Problém: Code Compilation Failed

**Chyba**: `Compilation failed: [chyby]`

**Možné příčiny**:
1. Kód odkazuje na blokované assembly (System.IO, Reflection atd.)
2. Kód nedědí `SiliconBeingBase`
3. Syntaktické chyby v generovaném kódu

**Řešení**:
- Zkontrolujte, že kód používá pouze povolené assembly
- Ověřte, že třída dědí `SiliconBeingBase`
- Opravte syntaktické chyby v kódu

### Web UI Problémy

#### Problém: SSE Connection Lost

**Příznaky**: Chat se neaktualizuje v reálném čase

**Řešení**:
1. Zkontrolujte síťové připojení
2. Ověřte, že server běží
3. Zkontrolujte browser konzoli pro chyby
4. SSE se automaticky reconnectuje - počkejte několik sekund

#### Problém: Page Not Loading

**Příznaky**: Web UI se nenačte nebo zobrazuje chyby

**Řešení**:
```bash
# Zkontrolujte, zda server běží
# Podívejte se na výstup konzole pro chyby

# Restartujte aplikaci
# Ctrl+C a znovu dotnet run
```

### Paměť a Výkon

#### Problém: High Memory Usage

**Příznaky**: Aplikace používá hodně paměti

**Řešení**:
1. Zkontrolujte počet aktivních bytostí
2. Zkontrolujte velikost chatové historie
3. Zvažte kompresi pamětí:
   ```json
   {
     "action": "compress_memories"
   }
   ```

#### Problém: Slow Response

**Příznaky**: AI odpovědi jsou pomalé

**Možné příčiny**:
1. Model je příliš velký pro hardware
2. Příliš mnoho bytostí běžících současně
3. Síťová latence (pro cloud AI)

**Řešení**:
- Použijte menší model
- Snižte počet aktivních bytostí
- Zkontrolujte síťové připojení pro cloud AI

### Log Analýza

#### Kde najít logy

Logy jsou uloženy v:
```
data/
└── SiliconManager/
    └── Logs/
        ├── system.log
        └── {being-id}.log
```

#### Zobrazení logů ve Web UI

1. Přejděte na **Log** v navigaci
2. Vyfiltrujte podle bytosti (volitelné)
3. Prohlédněte si záznamy

### Emergency Recovery

#### Reset Konfigurace

Pokud je konfigurace poškozena:

```bash
# Zálohujte aktuální konfiguraci
cp config.json config.json.backup

# Smažte konfiguraci (bude regenerována s výchozími hodnotami)
rm config.json

# Restartujte aplikaci
dotnet run
```

#### Reset Bytosti

Pokud je bytost v nefunkčním stavu:

1. Zastavte aplikaci
2. Najděte adresář bytosti v `data/SiliconManager/{being-guid}/`
3. Zálohujte `code.enc` (pokud existuje)
4. Smažte `code.enc` a `permission.enc`
5. Restartujte aplikaci - bytost se vrátí k výchozí implementaci

## Získání Pomoci

### Debug Mode

Spusťte v debug režimu pro detailní výstup:

```bash
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### System Information

Při hlášení problému zahrňte:

```bash
# Verze .NET
dotnet --version

# Verze OS
# Windows: winver
# Linux: lsb_release -a
# Mac: sw_vers

# Verze Ollama (pokud používáte)
ollama --version
```

### Problémy s Pracovními Poznámками

#### Problém: Nelze Vytvořit Pracovní Poznámku

**Příznaky**:
```
Failed to create work note
```

**Řešení**:
1. Zkontrolujte, zda bytost existuje a je spuštěna
2. Ověřte, že cesta úložiště má oprávnění k zápisu
3. Zkontrolujte, zda obsah není prázdný (obsah je vyžadován)
4. Zkontrolujte logy pro podrobné informace o chybě

#### Problém: Vyhledávání Poznámek Nevrací Výsledky

**Příznaky**:
- Vyhledávání klíčového slova vrací prázdné výsledky
- Ale jistě existují související poznámky

**Řešení**:
1. Zkontrolujte správnost pravopisu klíčového slova
2. Zkuste obecnější klíčové slovo
3. Ověřte, že poznámka obsahuje toto klíčové slovo (rozlišuje velká/malá písmena)
4. Zvyšte hodnotu parametru `max_results`

#### Problém: Generování Adresáře Poznámek Je Pomalé

**Příznaky**:
- Dlouhá doba odezvy při generování adresáře
- Bytost má mnoho poznámek (>1000 stránek)

**Řešení**:
1. Toto je normální, potřebuje iterovat všechny poznámky
2. Zvažte pravidelné archivování starých poznámek
3. Použijte funkci vyhledávání místo procházení adresáře
4. Plánovaná optimalizace: přidat mechanismus ukládání adresáře do mezipaměti

---

### Problémy se Sítí Znalostí

#### Problém: Dotaz na Znalost Vrací Prázdné Výsledky

**Příznaky**:
```
No knowledge triples found
```

**Řešení**:
1. Ověřte pravopis subjektu a predikátu
2. Zkontrolujte, zda byla znalost přidána do sítě
3. Použijte funkci vyhledávání pro fuzzy matching:
```json
{
  "action": "search",
  "query": "klíčové slovo"
}
```

#### Problém: Hledání Cesty Znalostí Selhává

**Příznaky**:
```
No path found between concepts
```

**Řešení**:
1. Ověřte, že oba koncepty existují v síti znalostí
2. Zkontrolujte, zda existuje asociativní cesta (může neexistovat přímý nebo nepřímý vztah)
3. Zkuste přidat více znalostí pro navázání spojení
4. Snižte limit délky cesty (pokud je nakonfigurován)

#### Problém: Validace Znalostí Selhává

**Příznaky**:
```
Knowledge validation failed
```

**Řešení**:
1. Zkontrolujte, zda je formát trojice správný (subjekt, predikát, objekt jsou vyžadovány)
2. Ověřte, že důvěryhodnost je v rozsahu 0.0-1.0
3. Zkontrolujte duplicity trojic
4. Zkontrolujte podrobnosti chyby validace pro pochopení konkrétního problému

#### Problém: Statistiky Sítě Znalostí Jsou Nepřesné

**Příznaky**:
- Čísla statistik neodpovídají očekávání
- Statistiky se neaktualizují po přidání znalosti

**Řešení**:
1. Statistiky mohou trvat několik sekund k aktualizaci (mezipaměť)
2. Zkontrolujte, zda byla operace smazání úspěšně provedena
3. Restartujte aplikaci pro vynucení obnovení statistik
4. Znovu dotazujte informace o statistikách prostřednictvím API

---

### Problémy se Správou Projektů

#### Problém: Nelze Vytvořit Projekt

**Příznaky**:
```
Failed to create project
```

**Řešení**:
1. Zkontrolujte, zda název projektu není prázdný (vyžadováno)
2. Ověřte, že název projektu není duplicitní
3. Zkontrolujte, že cesta úložiště má oprávnění k zápisu
4. Zkontrolujte logy pro podrobné informace o chybě

#### Problém: Ztráta Dat Projektu

**Příznaky**:
- Nelze načíst informace o projektu
- Soubor projektu je poškozen

**Řešení**:
1. Zkontrolujte, zda existuje adresář úložiště projektu
2. Obnovte data projektu ze zálohy
3. Ověřte, že formát JSON souboru je správný
4. Ručně opravte poškozený soubor projektu

---

### Contact

- **GitHub Issues**: https://github.com/akimoto-akira/SiliconLifeCollective/issues
- **Documentation**: docs/cs-CZ/
- **License**: Apache 2.0
