# Průvodce Řešením Problémů

> **Verze: v0.1.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | **Čeština**

## Časté Problémy

### Sestavování a Kompilace

#### Problém: Sestavení selhalo, chybějící závislosti

**Příznaky**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Řešení**:
```bash
dotnet restore
dotnet build
```

#### Problém: .NET SDK Nenalezen

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

### Problémy s Připojením AI

#### Problém: Ollama Odmítnutí Připojení

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

#### Problém: Model Nenalezen

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

#### Problém: Bailian 404 Chyba

**Příznaky**:
```
HTTP 404: Model not found
```

**Řešení**:
1. Ověřte, že API klíč je správný
2. Zkontrolujte, že název modelu odpovídá katalogu Bailian
3. Ověřte, že koncový bod regionu je správný
4. Zkontrolujte, že účet má přístup k modelu

---

### Problémy za Běhu

#### Problém: Port Již Používán

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

#### Problém: Bytost se Nemůže Spustit

**Příznaky**:
- Stav bytosti zobrazuje "Error"
- Logy zobrazují selhání inicializace

**Řešení**:
1. Zkontrolujte, že soubor duše existuje a je platný
2. Ověřte, že AI klient je nakonfigurován
3. Zkontrolujte logy pro konkrétní chybu:
```bash
tail -f logs/*.log
```

#### Problém: Nedostatek Paměti

**Příznaky**:
```
OutOfMemoryException
```

**Řešení**:
1. **SiliconLife.Default**: Zvyšte velikost haldy:
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast**: Verze Fast sama o sobě má vysokou spotřebu paměti (~500MB). Pokud je paměť dlouhodobě nedostatečná, doporučuje se:
   - Snížit počet souběžně běžících silikonových bytostí
   - Vyčistit stará data pro uvolnění paměti

3. Vyčistěte stará data:
```bash
# Archivujte staré logy
mv logs/ logs-archive/
mkdir logs

# Vyčistěte staré vzpomínky
# Prostřednictvím Web UI: Správa Paměti > Vyčistit
```

> **Tip**: SiliconLife.Default má nízkou spotřebu paměti (~200MB), vhodné pro prostředí s omezenou pamětí; SiliconLife.Fast má vyšší spotřebu paměti, ale lepší výkon, vhodné pro produkční prostředí.

---

### Problémy s Oprávněním

#### Problém: Oprávnění Zamítnuto

**Příznaky**:
```
Permission denied: disk:write
```

**Řešení**:
1. Zkontrolujte aktuální oprávnění:
```bash
curl http://localhost:8080/api/permissions
```

2. Udělte oprávnění:
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

3. Nebo použijte Web UI: Správa Oprávnění

#### Problém: Oprávnění Nevyprší

**Příznaky**:
- Oprávnění zůstávají platná po uplynutí doby platnosti

**Řešení**:
1. Zkontrolujte synchronizaci systémových hodin
2. Ověřte, že pole `expiresAt` je správně nastaveno
3. Vymažte cache oprávnění

---

### Problémy s Web UI

#### Problém: Nelze Přistupovat k Web UI

**Příznaky**:
- Prohlížeč zobrazuje "Connection refused"

**Řešení**:
1. Ověřte, že server běží
2. Zkontrolujte správnou URL: `http://localhost:8080`
3. Zkontrolujte nastavení firewallu
4. Zkontrolujte logy pro chyby při spuštění

#### Problém: SSE Nefunguje

**Příznaky**:
- Aktualizace v reálném čase se nezobrazují
- Chat se nestreamuje

**Řešení**:
1. Zkontrolujte, že prohlížeč podporuje SSE
2. Zakažte proxy bufferování pro SSE
3. Zkontrolujte stabilitu sítě
4. Zkuste jiný prohlížeč

#### Problém: UI Vypadá Poškozeně

**Příznaky**:
- Styly jsou nesprávné
- Rozložení je rozbité

**Řešení**:
1. Vymažte cache prohlížeče
2. Zkuste jiný skin: Nastavení > Skin
3. Zkontrolujte chyby v konzoli prohlížeče
4. Zakažte rozšíření prohlížeče

---

### Problémy s Úložištěm

#### Problém: Nelze Číst/Zapisovat Data

**Příznaky**:
```
IOException: Access denied
```

**Řešení**:
1. Zkontrolujte oprávnění souborů
2. Ověřte, že cesta k úložišti existuje
3. Zkontrolujte místo na disku
4. Spusťte s příslušnými oprávněními

#### Problém: Poškození Dat

**Příznaky**:
- Chyby parsování JSON
- Ztráta dat

**Řešení**:
1. Obnovte ze zálohy
2. Zkontrolujte integritu úložiště:
```bash
# Prostřednictvím Web UI: Systém > Kontrola Úložiště
```

3. Ručně opravte poškozené soubory

#### Problém: Poškození Souboru SpeedyPack Úložiště (Verze Fast)

**Příznaky**:
- Soubor `.spk` nelze načíst
- Inicializace SpeedyStorage selhává

**Řešení**:
1. Použijte nástroj `SiliconLife.Speedy.Manager` pro kontrolu a opravu `.spk` souborů
2. Zkontrolujte, zda soubor `.spk.idx` odpovídá souboru `.spk`
3. Pokud je indexový soubor poškozen, smažte soubor `.spk.idx`, systém automaticky obnoví index
4. Obnovte soubor `.spk` ze zálohy

#### Problém: Selhání Automatické Komprese SpeedyPack (Verze Fast)

**Příznaky**:
- Soubor `.spk` neustále roste
- Nedostatek místa na disku

**Řešení**:
1. Zkontrolujte, zda `SpeedyPackAutoCompactor` běží správně
2. Ručně spusťte kompresní operaci
3. Zkontrolujte konfiguraci prahu komprese
4. Použijte nástroj `SiliconLife.Speedy.Manager` pro ruční kompresi

---

### Problémy s Prováděním Nástrojů

#### Problém: Nástroj Nenalezen

**Příznaky**:
```
Tool "xyz" not found
```

**Řešení**:
1. Ověřte, že název nástroje je správný
2. Zkontrolujte, že nástroj je v adresáři Tools
3. Znovu sestavte projekt
4. Zkontrolujte, že nástroj je správně implementován

#### Problém: Nástroj Vrací Chybu

**Příznaky**:
```
Tool execution failed: ...
```

**Řešení**:
1. Zkontrolujte logy nástroje
2. Ověřte vstupní parametry
3. Otestujte nástroj izolovaně
4. Zkontrolujte oprávnění

---

### Problémy s Pluginy

#### Problém: Načtení Pluginu Selhalo

**Příznaky**:
```
Plugin load failed: Security check failed
```

**Řešení**:
1. Zkontrolujte, zda plugin neodkazuje na zakázané jmenné prostory (`System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`)
2. Ověřte, že plugin odkazuje pouze na sestavení ze seznamu důvěryhodných sestavení
3. Zkontrolujte, že plugin správně implementuje rozhraní `IPlugin`
4. Zkontrolujte logy pro podrobnosti o selhání bezpečnostní kontroly

#### Problém: Nástroje Pluginu Nejsou Registrovány

**Příznaky**:
- Plugin se úspěšně načetl, ale nástroje se neobjevují v seznamu nástrojů

**Řešení**:
1. Potvrďte, že třídy nástrojů v pluginu správně implementují rozhraní `ITool`
2. Zkontrolujte, že třídy nástrojů jsou public
3. Ověřte, že `ToolManager.ScanAllPluginAssemblies()` byl volán
4. Znovu sestavte plugin a restartujte aplikaci

---

### Problémy s Pracovními Poznámkami

#### Problém: Nelze Vytvořit Pracovní Poznámku

**Příznaky**:
```
Failed to create work note
```

**Řešení**:
1. Zkontrolujte, že bytost existuje a je v běžícím stavu
2. Ověřte, že cesta k úložišti má oprávnění k zápisu
3. Zkontrolujte, že obsah není prázdný (obsah je povinný)
4. Zkontrolujte logy pro podrobné informace o chybě

#### Problém: Vyhledávání Poznámek Bez Výsledků

**Příznaky**:
- Hledání klíčového slova vrací prázdné výsledky
- Ale jsou k dispozici relevantní poznámky

**Řešení**:
1. Zkontrolujte, zda je klíčové slovo napsáno správně
2. Zkuste použít obecnější klíčové slovo
3. Ověřte, že poznámka obsahuje dané klíčové slovo (rozlišují se velká a malá písmena)
4. Zvyšte hodnotu parametru `max_results`

#### Problém: Pomalé Generování Obsahu Poznámek

**Příznaky**:
- Dlouhá doba odpovědi při generování obsahu
- Bytost má velké množství poznámek (>1000 stránek)

**Řešení**:
1. Toto je normální jev, vyžaduje procházení všech poznámek
2. Zvažte pravidelnou archivaci starých poznámek
3. Použijte funkci vyhledávání místo procházení obsahu
4. Plánovaná optimalizace: přidání mechanismu cache obsahu

---

### Problémy se Znalostní Sítí

#### Problém: Dotaz na Znalosti Vrací Prázdné Výsledky

**Příznaky**:
```
No knowledge triples found
```

**Řešení**:
1. Ověřte psaní subjektu a predikátu
2. Zkontrolujte, zda byly znalosti přidány do sítě
3. Použijte funkci vyhledávání pro fuzzy shodu:
```json
{
  "action": "search",
  "query": "klíčové slovo"
}
```

#### Problém: Selhání Hledání Cesty ve Znalostech

**Příznaky**:
```
No path found between concepts
```

**Řešení**:
1. Ověřte, že oba koncepty existují ve znalostní síti
2. Zkontrolujte, zda existuje spojovací cesta (nemusí existovat přímá ani nepřímá relace)
3. Zkuste přidat více znalostí pro vytvoření spojení
4. Snižte limit délky cesty (pokud je nastaven)

#### Problém: Selhání Validace Znalostí

**Příznaky**:
```
Knowledge validation failed
```

**Řešení**:
1. Zkontrolujte, že formát tripletu je správný (subjekt, predikát, objekt jsou povinné)
2. Ověřte, že spolehlivost je v rozsahu 0.0-1.0
3. Zkontrolujte, zda neexistují duplicitní triplety
4. Zkontrolujte podrobnosti chyby validace pro konkrétní problém

#### Problém: Nepřesné Statistiky Znalostní Sítě

**Příznaky**:
- Statistiky neodpovídají očekávání
- Statistiky se neaktualizují po přidání znalostí

**Řešení**:
1. Statistiky se mohou aktualizovat s několikasekundovým zpožděním (cache)
2. Zkontrolujte, zda nebyly nějaké operace smazání neúspěšné
3. Restartujte aplikaci pro vynucení obnovení statistik
4. Znovu dotazujte statistiky prostřednictvím API

---

### Problémy se Správou Projektů

#### Problém: Nelze Vytvořit Projekt

**Příznaky**:
```
Failed to create project
```

**Řešení**:
1. Zkontrolujte, že název projektu není prázdný (povinný)
2. Ověřte, že název projektu není duplicitní
3. Zkontrolujte, že cesta k úložišti má oprávnění k zápisu
4. Zkontrolujte logy pro podrobné informace o chybě

#### Problém: Ztráta Dat Projektu

**Příznaky**:
- Informace o projektu nelze načíst
- Soubory projektu jsou poškozeny

**Řešení**:
1. Zkontrolujte, že adresář úložiště projektu existuje
2. Obnovte data projektu ze zálohy
3. Ověřte, že formát JSON souboru je správný
4. Ručně opravte poškozené soubory projektu

---

## Ladění

### Povolení Podrobného Logování

Upravte konfiguraci:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Kontrola Logů

Logy jsou uloženy v:
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

### Použití Debuggeru

**SiliconLife.Default (Výchozí Implementace)**:
```bash
# Spuštění s debuggerem
dotnet run --project src/SiliconLife.Default --configuration Debug

# Připojení debuggeru
# Prostřednictvím IDE: Připojit k Procesu > SiliconLife.Default
```

**SiliconLife.Fast (Vysoce Výkonná Verze)**:
```bash
# Spuštění s debuggerem
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Připojení debuggeru
# Prostřednictvím IDE: Připojit k Procesu > SiliconLife.Fast
```

> **Doporučení**: Ve fázi vývoje a ladění se doporučuje používat SiliconLife.Default, po ověření architektury použijte SiliconLife.Fast pro produkční nasazení.

---

### Problémy s Výkonem

#### Pomalá Doba Odpovědi

**Optimalizace**:
1. Snížte složitost AI modelu
2. Povolte cache
3. Vyčistěte stará data
4. Zvyšte systémové zdroje

#### Vysoké Využití CPU

**Kontrola**:
- Běží příliš mnoho bytostí
- Nekonečná smyčka v nástrojích
- Časté spouštění časovačů

**Řešení**:
- Snižte počet souběžných bytostí
- Optimalizujte kód nástrojů
- Upravte intervaly časovačů

#### Vysoké Využití Paměti

**Monitorování**:
```bash
# Prostřednictvím Web UI: Dashboard > Paměť
```

**Optimalizace**:
- Vyčistěte staré vzpomínky
- Snižte velikost kontextu
- Implementujte stránkování

---

## Získání Pomoci

### Zobrazení Dokumentace

- [Průvodce Rychlým Startem](getting-started.md)
- [Vývojářská Příručka](development-guide.md)
- [Reference API](api-reference.md)
- [Průvodce Architekturou](architecture.md)

### Kontrola Logů

Vždy nejprve zkontrolujte logy pro podrobnosti o chybách.

### Komunitní Podpora

- GitHub Issues: Nahlášení bugů
- Discussions: Dotazy
- Dokumentace: Hledání řešení

---

### Nouzové Postupy

#### Pád Systému

1. Zkontrolujte logy pro příčinu
2. Restartujte aplikaci:

**SiliconLife.Default (Výchozí Implementace)**:
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (Hlavní Produkční Verze)**:
```bash
dotnet run --project src/SiliconLife.Fast
```

3. V případě potřeby obnovte ze zálohy

#### Ztráta Dat

1. Okamžitě zastavte aplikaci
2. Zkontrolujte záložní soubory
3. Obnovte data
4. Ověřte integritu

#### Bezpečnostní Incident

1. Zastavte všechny bytosti
2. Odvolejte všechna oprávnění
3. Zkontrolujte auditní logy
4. Zkontrolujte řízení přístupu
5. Restartujte s omezenými oprávněními

---

### Prevence

### Nejlepší Praktiky

1. **Pravidelné Zálohování**
   - Zálohujte datový adresář
   - Zálohujte konfiguraci
   - Testujte proces obnovy

2. **Monitorování Zdrojů**
   - Sledujte využití CPU/paměti
   - Monitorujte místo na disku
   - Zkontrolujte síťové připojení

3. **Udržování Aktualizací**
   - Aktualizujte .NET SDK
   - Aktualizujte závislosti
   - Aplikujte bezpečnostní záplaty

4. **Testování Změn**
   - Nejprve testujte ve vývoji
   - Používejte verzování
   - Dokumentujte změny

---

## Další Kroky

- 📚 Přečtěte si [Průvodce Architekturou](architecture.md)
- 🛠️ Podívejte se na [Vývojářskou Příručku](development-guide.md)
- 🚀 Podívejte se na [Průvodce Rychlým Startem](getting-started.md)
- 🔒 Podívejte se na [Bezpečnostní Dokumentaci](security.md)
