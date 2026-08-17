# Řešení problémů

> **Verze: v0.2.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | **Čeština** | [Русский](../ru-RU/troubleshooting.md)

## Časté problémy

### Sestavení a kompilace

#### Problém: Sestavení selhalo, chybí závislosti

**Příznaky**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Řešení**:
```bash
dotnet restore
dotnet build
```

#### Problém: .NET SDK nenalezeno

**Příznaky**:
```
The .NET SDK could not be found
```

**Řešení**:
1. Nainstalujte .NET 9 SDK: https://dotnet.microsoft.com/download/dotnet/9.0
2. Ověřte instalaci:
```bash
dotnet --version
```

---

### Problémy s AI připojením

#### Problém: Ollama připojení zamítnuto

**Příznaky**:
```
Failed to connect to Ollama at http://localhost:11434
```

**Řešení**:
```bash
# Zkontrolujte, zda Ollama běží
ollama list

# Spusťte Ollama
ollama serve

# Otestujte připojení
curl http://localhost:11434/api/tags
```

#### Problém: Model nenalezen

**Příznaky**:
```
model "qwen2.5:7b" not found
```

**Řešení**:
```bash
# Stáhněte požadovaný model
ollama pull qwen2.5:7b

# Seznam dostupných modelů
ollama list
```

#### Problém: Bailian 404 chyba

**Příznaky**:
```
HTTP 404: Model not found
```

**Řešení**:
1. Ověřte, že API klíč je správný
2. Zkontrolujte, že název modelu odpovídá katalogu Bailian
3. Ověřte, že regionální endpoint je správný
4. Zkontrolujte, že účet má přístup k modelu

#### Problém: Volcengine Ark připojení selhalo

**Příznaky**:
```
HTTP 401: Unauthorized
nebo
HTTP 404: Endpoint not found
```

**Řešení**:
1. Ověřte, že API klíč je správný
2. Zkontrolujte, že formát URL endpointu je správný (výchozí: `https://ark.cn-beijing.volces.com/api/v3/chat/completions`)
3. Potvrďte, že parametr Model používá ID inferenčního přístupového bodu (např. `ep-20241212123456-abcde`), nikoli název modelu
4. Zkontrolujte, že účet má přístup k přístupovému bodu

---

### Problémy za běhu

#### Problém: Port je již obsazen

**Příznaky**:
```
HttpListenerException: Address already in use
```

**Řešení**:

**Windows**:
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac**:
```bash
lsof -ti:8080 | xargs kill -9
```

**Nebo změňte port v konfiguraci**.

#### Problém: Bytost nelze spustit

**Příznaky**:
- Stav bytosti zobrazuje "Error"
- Protokoly ukazují selhání inicializace

**Řešení**:
1. Zkontrolujte, že Soubor Duše existuje a je platný
2. Ověřte, že AI klient je nakonfigurován
3. Zkontrolujte protokoly pro konkrétní chybu:
```bash
tail -f logs/*.log
```

#### Problém: Nedostatek paměti

**Příznaky**:
```
OutOfMemoryException
```

**Řešení**:
1. **SiliconLife.Default**: Zvětšete haldu:
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast**: Verze Fast má vyšší spotřebu paměti (~500MB), pokud paměť trvale nedostačuje, doporučuje se:
   - Snížit počet souběžných Křemíkových Bytostí
   - Vyčistit stará data pro uvolnění paměti

3. Vyčistěte stará data:
```bash
# Archivace starých protokolů
mv logs/ logs-archive/
mkdir logs

# Vyčištění starých pamětí
# Přes Web UI: Správa paměti > Vyčištění
```

> **Tip**: SiliconLife.Default má nižší spotřebu paměti (~200MB), vhodný pro prostředí s omezenou pamětí; SiliconLife.Fast má vyšší spotřebu paměti, ale lepší výkon, vhodný pro produkční prostředí.

---

### Problémy s oprávněními

#### Problém: Oprávnění zamítnuto

**Příznaky**:
```
Permission denied: FileAccess C:\Windows
```

**Řešení**:
1. Zkontrolujte aktuální oprávnění:
```bash
curl http://localhost:8080/api/permissions/list
```

2. Udělte oprávnění:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

3. Nebo použijte Web UI: Správa oprávnění

#### Problém: Oprávnění nevypršelo

**Příznaky**:
- Oprávnění je stále platné po uplynutí doby expirace

**Řešení**:
1. Zkontrolujte synchronizaci systémových hodin
2. Ověřte, že pole `expiresAt` je správně nastaveno
3. Vymažte mezipaměť oprávnění

---

### Problémy s Web UI

#### Problém: Nelze přistoupit k Web UI

**Příznaky**:
- Prohlížeč zobrazuje "Connection refused"

**Řešení**:
1. Ověřte, že server běží
2. Zkontrolujte správnou URL: `http://localhost:8080`
3. Zkontrolujte nastavení brány firewall
4. Zkontrolujte protokoly pro chyby při spuštění

#### Problém: SSE nefunguje

**Příznaky**:
- Aktualizace v reálném čase se nezobrazují
- Chat se nestreamuje

**Řešení**:
1. Zkontrolujte, že prohlížeč podporuje SSE
2. Zakažte proxy bufferování pro SSE
3. Zkontrolujte stabilitu sítě
4. Vyzkoušejte jiný prohlížeč

#### Problém: UI vypadá poškozeně

**Příznaky**:
- Styly nejsou správné
- Rozložení je rozbité

**Řešení**:
1. Vymažte mezipaměť prohlížeče
2. Vyzkoušejte jiný skin: Nastavení > Skiny
3. Zkontrolujte chyby v konzoli prohlížeče
4. Zakažte rozšíření prohlížeče

---

### Problémy s úložištěm

#### Problém: Nelze číst/zapisovat data

**Příznaky**:
```
IOException: Access denied
```

**Řešení**:
1. Zkontrolujte oprávnění souborů
2. Ověřte, že cesta úložiště existuje
3. Zkontrolujte místo na disku
4. Spusťte s příslušnými oprávněními

#### Problém: Poškození dat

**Příznaky**:
- Chyba parsování JSON
- Ztráta dat

**Řešení**:
1. Obnovte ze zálohy
2. Zkontrolujte integritu úložiště:
```bash
# Přes Web UI: Systém > Kontrola úložiště
```

3. Ručně opravte poškozené soubory

#### Problém: SpeedyPack soubor úložiště poškozen (verze Fast)

**Příznaky**:
- `.spk` soubor nelze načíst
- SpeedyStorage inicializace selhala

**Řešení**:
1. Použijte nástroj `SiliconLife.Speedy.Manager` pro kontrolu a opravu `.spk` souborů
2. Zkontrolujte, zda `.spk.idx` indexový soubor odpovídá `.spk` souboru
3. Pokud je indexový soubor poškozen, smažte `.spk.idx` soubor, systém automaticky obnoví index
4. Obnovte `.spk` soubor ze zálohy

#### Problém: SpeedyPack automatická komprimace selhala (verze Fast)

**Příznaky**:
- `.spk` soubor neustále roste
- Nedostatek místa na disku

**Řešení**:
1. Zkontrolujte, zda `SpeedyPackAutoCompactor` běží správně
2. Ručně spusťte komprimaci
3. Zkontrolujte konfiguraci prahu komprimace
4. Použijte nástroj `SiliconLife.Speedy.Manager` pro ruční komprimaci

---

### Problémy s prováděním nástrojů

#### Problém: Nástroj nenalezen

**Příznaky**:
```
Tool "xyz" not found
```

**Řešení**:
1. Ověřte, že název nástroje je správný
2. Zkontrolujte, že nástroj je v adresáři Tools
3. Znovu sestavte projekt
4. Zkontrolujte, že nástroj je správně implementován

#### Problém: Nástroj vrací chybu

**Příznaky**:
```
Tool execution failed: ...
```

**Řešení**:
1. Zkontrolujte protokoly nástroje
2. Ověřte vstupní parametry
3. Otestujte nástroj nezávisle
4. Zkontrolujte oprávnění

---

### Problémy se zásuvnými moduly

#### Problém: Načítání zásuvného modulu selhalo

**Příznaky**:
```
Plugin load failed: Security check failed
```

**Řešení**:
1. Zkontrolujte, zda zásuvný modul neodkazuje na nedeklarovatelné jmenné prostory (P/Invoke, Unsafe, Reflection Emit, `Microsoft.CodeAnalysis`)
2. Pokud zásuvný modul vyžaduje síťový nebo souborový přístup, ujistěte se, že deklaruje odpovídající schopnosti pomocí atributu `[PluginCapability]` (Network, FileIO, Process, AI)
3. Ověřte, že zásuvný modul odkazuje pouze na sestavení z whitelistu důvěryhodných sestavení
4. Zkontrolujte, že zásuvný modul správně implementuje rozhraní `IPlugin`
5. Zobrazte protokoly pro podrobné informace o selhání bezpečnostní kontroly

#### Problém: Nástroj zásuvného modulu není registrován

**Příznaky**:
- Zásuvný modul se načetl úspěšně, ale nástroj se neobjevil v seznamu nástrojů

**Řešení**:
1. Potvrďte, že třída nástroje v zásuvném modulu správně implementuje rozhraní `ITool`
2. Zkontrolujte, že třída nástroje je public
3. Ověřte, že `ToolManager.ScanAllPluginAssemblies()` byla volána
4. Znovu sestavte zásuvný modul a restartujte aplikaci

---

### Problémy s dovednostmi

#### Problém: Dovednost se neobjevuje v seznamu dovedností nebo není pro AI viditelná

**Příznaky**:
- Stránka dovedností Web UI uloží úspěšně, ale seznam nezobrazuje / AI nevolá danou dovednost

**Řešení**:
1. Zkontrolujte, zda `id` a `description` dovednosti nejsou prázdné (koncepty nejsou AI vystaveny)
2. Dovednosti s neúplnými metadaty (`NeedsCompletion`) nejsou injektovány do AI – doplňte YAML frontmatter metadata nebo nechte AI doplnit před uložením
3. Zkontrolujte, zda matice oprávnění nezakazuje `{skillId}:execute` (zakázané dovednosti nejsou pro AI viditelné)
4. Potvrďte, že globální přepínač `SkillEnabled` je true
5. Hot-reload trvá maximálně 30 sekund, počkejte a obnovte nebo restartujte

#### Problém: Spouštění dovednosti selhalo s "not in whitelist"

**Příznaky**:
```
Tool 'xxx' is not available in skill 'yyy' (not in whitelist)
```

**Řešení**:
- Přidejte daný nástroj do `tool_whitelist` dovednosti, nebo vyprázdněte whitelist pro zdědění všech nástrojů Bytosti

#### Problém: Dosažen limit počtu dovedností

**Příznaky**:
```
Custom skill limit reached (50)
```

**Řešení**:
1. Smažte nepoužívané vlastní dovednosti
2. Nebo zvyšte konfiguraci `MaxCustomSkillsPerBeing`

---

### Problémy s MCP

#### Problém: Připojení MCP serveru selhalo

**Příznaky**:
- Stav serveru ukazuje `error` nebo `disconnected`, `lastError` není prázdný

**Řešení**:
1. stdio server: potvrďte, že `command` je spustitelný (např. `npx` je v PATH), `arguments` jsou správné
2. http server: zkontrolujte, že `endpoint` URL je dostupný (firewall, proxy)
3. Na stránce /mcp klikněte na **Znovu připojit**
4. Zkontrolujte detaily `lastError`, běžné příčiny: příkaz neexistuje, nekompatibilní verze, endpoint 404

#### Problém: MCP nástroj není injektován do Bytosti

**Příznaky**:
- Server je připojen (`connected`), ale AI nemůže volat nástroj `mcp_xxx_yyy`

**Řešení**:
1. Potvrďte, že `enabled` serveru je true
2. Potvrďte, že globální přepínač `McpEnabled` je true
3. Zkontrolujte matici oprávnění: zda `mcp_{serverId}_{toolName}:execute` není zakázáno
4. V konverzaci s Bytostí lze použít nástroj `mcp` (`list_tools`) pro ověření skutečně injektovaných názvů nástrojů

#### Problém: Přidání serveru vrací chybu formátu ID

**Příznaky**:
```
Server id must contain only lowercase letters, digits and underscores
```

**Řešení**:
- ID serveru umožňuje pouze malá písmena, číslice a podtržítka (např. `filesystem`, `github_tools`)

---

### Problémy s IM platformou

#### Problém: Zprávy Feishu nejsou přijímány

**Řešení**:
1. Zkontrolujte konfiguraci odběru událostí na Feishu Open Platform – adresu callback a port (`listenPort` + `callbackPath`)
2. Potvrďte, že `Encrypt Key` / `Verification Token` odpovídá konfiguraci
3. Pro lokální vývoj lze použít průvodce OAuth autorizací (jedno-klik autorizace na konfigurační stránce); event callback vyžaduje dostupnost z veřejné sítě nebo použití tunelu
4. Zkontrolujte chyby ověření podpisu/dešifrování v protokolu

#### Problém: Vypršení OAuth autorizace

**Příznaky**:
- Stránka autorizace ukazuje stav `timeout`

**Řešení**:
1. Platnost autorizační relace je 5 minut, po vypršení znovu klikněte na tlačítko autorizace
2. Potvrďte, že callback adresa `/im/feishu/callback` je přístupná z Feishu (`redirectBaseUrl` je správně nakonfigurováno)
3. Zobrazení stavu na frontendu závisí na SSE, pokud je SSE odpojeno, lze použít fallback dotazování na `/im/{platform}/status`

#### Problém: Zástupný symbol `${ENV_VAR}` nebyl analyzován

**Příznaky**:
- Připojení IM platformy selhalo, konfigurační hodnota je stále text zástupného symbolu

**Řešení**:
1. Potvrďte, že proměnná prostředí byla nastavena před spuštěním procesu (restart aplikace pro uplatnění)
2. Zkontrolujte překlep v názvu proměnné (podporováno pouze `[A-Za-z_][A-Za-z0-9_]*`)
3. Poznámka: zachování zástupného symbolu v config.json je designové chování, analýza probíhá na kopii v paměti

#### Problém: Pouze jedna z více IM platforem přijímá zprávy

**Řešení**:
- Odchozí zprávy se vysílají na všechny aktivní platformy, selhání odeslání na jedné platformě je tiše izolováno – zkontrolujte, zda token dané platformy nevypršel (znovu autorizujte nebo aktualizujte klíč)

---

### Problémy s pracovními poznámkami

#### Problém: Nelze vytvořit pracovní poznámku

**Příznaky**:
```
Failed to create work note
```

**Řešení**:
1. Zkontrolujte, že bytost existuje a je v běžícím stavu
2. Ověřte, že cesta úložiště má práva zápisu
3. Zkontrolujte, že obsah není prázdný (obsah je povinný)
4. Zobrazte protokoly pro podrobné informace o chybě

#### Problém: Vyhledávání poznámek bez výsledků

**Příznaky**:
- Hledání klíčového slova vrací prázdné výsledky
- Ale jste si jistí, že relevantní poznámky existují

**Řešení**:
1. Zkontrolujte, že klíčové slovo je správně napsáno
2. Zkuste použít obecnější klíčové slovo
3. Ověřte, že poznámka obsahuje dané klíčové slovo (rozlišují se velká/malá písmena)
4. Zvyšte hodnotu parametru `max_results`

#### Problém: Generování obsahu poznámek je pomalé

**Příznaky**:
- Dlouhá doba odpovědi při generování obsahu
- Bytost má velké množství poznámek (>1000 stránek)

**Řešení**:
1. Toto je normální jev, vyžaduje procházení všech poznámek
2. Zvažte pravidelnou archivaci starých poznámek
3. Použijte funkci vyhledávání místo procházení obsahu
4. Plánovaná optimalizace: přidání mechanismu mezipaměti obsahu

---

### Problémy se znalostní sítí

#### Problém: Dotaz na znalosti vrací prázdné výsledky

**Příznaky**:
```
No knowledge triples found
```

**Řešení**:
1. Ověřte, že subjekt a predikát jsou správně napsány
2. Zkontrolujte, že znalost byla přidána do sítě
3. Použijte funkci vyhledávání pro fuzzy shodu:
```json
{
  "action": "search",
  "query": "klíčové slovo"
}
```

#### Problém: Hledání znalostní cesty selhalo

**Příznaky**:
```
No path found between concepts
```

**Řešení**:
1. Ověřte, že oba koncepty existují ve znalostní síti
2. Zkontrolujte, zda existuje asociační cesta (možná neexistuje přímý ani nepřímý vztah)
3. Zkuste přidat více znalostí pro vytvoření spojení
4. Snižte omezení délky cesty (pokud je nastaveno)

#### Problém: Validace znalostí selhala

**Příznaky**:
```
Knowledge validation failed
```

**Řešení**:
1. Zkontrolujte, že formát trojice je správný (subjekt, predikát, objekt jsou povinné)
2. Ověřte, že skóre důvěry je v rozsahu 0.0-1.0
3. Zkontrolujte, zda neexistují duplicitní trojice
4. Zobrazte detaily chyby validace pro konkrétní problém

#### Problém: Statistiky znalostní sítě jsou nepřesné

**Příznaky**:
- Statistiky neodpovídají očekávání
- Po přidání znalostí se statistiky neaktualizovaly

**Řešení**:
1. Statistiky se mohou aktualizovat s několikasekundovým zpožděním (mezipaměť)
2. Zkontrolujte, zda operace smazání nebyla úspěšně provedena
3. Restartujte aplikaci pro vynucení obnovení statistik
4. Znovu dotazujte statistiky přes API

---

### Problémy se správou projektů

#### Problém: Nelze vytvořit projekt

**Příznaky**:
```
Failed to create project
```

**Řešení**:
1. Zkontrolujte, že název projektu není prázdný (povinný)
2. Ověřte, že název projektu není duplicitní
3. Zkontrolujte, že cesta úložiště má práva zápisu
4. Zobrazte protokoly pro podrobné informace o chybě

#### Problém: Ztráta projektových dat

**Příznaky**:
- Informace o projektu nelze načíst
- Projektové soubory jsou poškozené

**Řešení**:
1. Zkontrolujte, že adresář úložiště projektu existuje
2. Obnovte projektová data ze zálohy
3. Ověřte, že formát JSON souborů je správný
4. Ručně opravte poškozené projektové soubory

#### Problém: Přiřazení projektové role selhalo

**Příznaky**:
```
Failed to assign role
```

**Řešení**:
1. Potvrďte, že Křemíková Bytost je členem projektu
2. Zkontrolujte, že název role je platný
3. Ověřte, že operátor je Kurátor Křemíku
4. Zobrazte protokoly pro podrobné informace o chybě

#### Problém: Pracovní postup nelze spustit

**Příznaky**:
- Vytvoření instance pracovního postupu selhalo
- Přechody stavů se neprovádějí

**Řešení**:
1. Zkontrolujte, že šablona pracovního postupu je definována
2. Ověřte, že počáteční stav je správně nastaven
3. Potvrďte, že projekt je vázán na šablonu pracovního postupu
4. Zkontrolujte protokoly pracovního postupu pro chyby přechodů

---

### Problémy s oprávněními nástrojů

#### Problém: Operace nástroje zamítnuta

**Příznaky**:
```
Tool operation denied: network:post
```

**Řešení**:
1. Zkontrolujte konfiguraci oprávnění nástrojů Křemíkové Bytosti:
```bash
curl http://localhost:8080/api/beings/tool-permissions?beingId=<id>
```

2. Aktualizujte oprávnění nástrojů:
```bash
curl -X PUT http://localhost:8080/api/beings/tool-permissions \
  -H "Content-Type: application/json" \
  -d '{
    "beingId": "being-uuid",
    "permissions": {
      "network:post": "allowed"
    }
  }'
```

3. Nebo použijte Web UI: Bytosti → Oprávnění nástrojů

#### Problém: Projektová oprávnění nástrojů nefungují

**Příznaky**:
- Oprávnění nástrojů na úrovni projektu nefungují podle očekávání

**Řešení**:
1. Potvrďte, že oprávnění na úrovni projektu jsou správně nakonfigurována
2. Zkontrolujte, zda nejsou v konfliktu oprávnění na úrovni Křemíkové Bytosti a projektu
3. Oprávnění na úrovni projektu jsou nezávislá na úrovni Křemíkové Bytosti, použije se průnik obou
4. Zobrazte auditní protokol pro potvrzení výsledků kontroly oprávnění

---

## Ladění

### Povolení podrobného protokolování

Upravte konfiguraci:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Kontrola protokolů

Protokoly jsou uloženy v:
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

Zobrazení v reálném čase:
```bash
tail -f logs/*.log
```

### Použití debuggeru

**SiliconLife.Default (výchozí implementace)**:
```bash
# Spuštění s debuggerem
dotnet run --project src/SiliconLife.Default --configuration Debug

# Připojení debuggeru
# Přes IDE: Attach to Process > SiliconLife.Default
```

**SiliconLife.Fast (vysoce výkonná verze)**:
```bash
# Spuštění s debuggerem
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Připojení debuggeru
# Přes IDE: Attach to Process > SiliconLife.Fast
```

> **Doporučení**: Ve fázi vývoje a ladění se doporučuje používat SiliconLife.Default, po ověření architektury přejít na SiliconLife.Fast pro produkční nasazení.

---

## Problémy s výkonem

### Pomalá doba odpovědi

**Optimalizace**:
1. Snížení složitosti AI modelu
2. Povolení mezipaměti
3. Vyčištění starých dat
4. Zvýšení systémových prostředků

### Vysoké využití CPU

**Kontrola**:
- Příliš mnoho běžících bytostí
- Nekonečné smyčky v nástrojích
- Časté spouštění časovačů

**Řešení**:
- Snížení počtu souběžných bytostí
- Optimalizace kódu nástrojů
- Úprava intervalů časovačů

### Vysoké využití paměti

**Monitorování**:
```bash
# Přes Web UI: Řídicí panel > Paměť
```

**Optimalizace**:
- Vyčištění starých pamětí
- Snížení velikosti kontextu
- Implementace stránkování

---

## Získání pomoci

### Zobrazení dokumentace

- [Příručka rychlého startu](getting-started.md)
- [Vývojářská příručka](development-guide.md)
- [API reference](api-reference.md)
- [Příručka architektury](architecture.md)

### Kontrola protokolů

Vždy nejprve zkontrolujte protokoly pro detaily o chybách.

### Komunitní podpora

- GitHub Issues: hlášení chyb
- Discussions: dotazy
- Dokumentace: hledání řešení

---

## Nouzové postupy

### Pád systému

1. Zkontrolujte protokoly pro příčinu
2. Restartujte aplikaci:

**SiliconLife.Default (výchozí implementace)**:
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (hlavní produkční verze)**:
```bash
dotnet run --project src/SiliconLife.Fast
```

3. V případě potřeby obnovte ze zálohy

### Ztráta dat

1. Okamžitě zastavte aplikaci
2. Zkontrolujte záložní soubory
3. Obnovte data
4. Ověřte integritu

### Bezpečnostní incident

1. Zastavte všechny bytosti
2. Odvolejte všechna oprávnění
3. Zkontrolujte auditní protokoly
4. Zkontrolujte řízení přístupu
5. Restartujte s omezenými oprávněními

---

## Prevence

### Osvědčené postupy

1. **Pravidelné zálohování**
   - Zálohujte datový adresář
   - Zálohujte konfiguraci
   - Testujte proces obnovy

2. **Monitorování prostředků**
   - Sledujte využití CPU/paměti
   - Monitorujte místo na disku
   - Kontrolujte síťové připojení

3. **Udržování aktualizací**
   - Aktualizujte .NET SDK
   - Aktualizujte závislosti
   - Aplikujte bezpečnostní opravy

4. **Testování změn**
   - Nejprve testujte ve vývoji
   - Používejte verzovací kontrolu
   - Zaznamenávejte změny

---

## Další kroky

- 📚 Přečtěte [příručku architektury](architecture.md)
- 🛠️ Prohlédněte [vývojářskou příručku](development-guide.md)
- 🚀 Prohlédněte [příručku rychlého startu](getting-started.md)
- 🔒 Prohlédněte [dokumentaci zabezpečení](security.md)
