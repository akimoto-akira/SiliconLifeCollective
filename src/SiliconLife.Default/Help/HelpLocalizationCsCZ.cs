// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

namespace SiliconLife.Default.Help;

/// <summary>
/// Czech help documentation implementation
/// Implementace nápovědy v českém jazyce
/// </summary>
public class HelpLocalizationCsCZ : HelpLocalizationBase
{
    #region Help Documents
    
    public override string GettingStarted_Title => "Rychlý start";
    public override string BeingManagement_Title => "Správa bytostí";
    public override string ChatSystem_Title => "Chatovací systém";
    public override string Dashboard_Title => "Nástěnka";
    public override string Task_Title => "Úlohy";
    public override string Timer_Title => "Časovače";
    public override string Permission_Title => "Správa oprávnění";
    public override string Config_Title => "Konfigurace";
    public override string FAQ_Title => "Často kladené otázky";
    public override string Memory_Title => "Paměťový systém";
    public override string OllamaSetup_Title => "Instalace Ollama a stahování modelů";
    public override string BailianDashScope_Title => "Průvodce používáním platformy Alibaba Cloud Bailian";
    public override string AIClients_Title => "Konfigurace AI klienta";
    
    public override string BeingSoul_Title => "Soubor Duše";
    public override string AuditLog_Title => "Protokol Auditů";
    public override string KnowledgeGraph_Title => "Graf Znalostí";
    public override string WorkNotes_Title => "Pracovní Poznámky";
    public override string Projects_Title => "Správa Projektů";
    public override string Logging_Title => "Systém Protokolování";

    public override string[] GettingStarted_Tags => new[] { "instalace", "spuštění", "začínáme", "rychlý start", "první použití", "inicializace", "provoz", "konfigurace prostředí" };
    public override string[] BeingManagement_Tags => new[] { "bytost", "vytvořit", "konfigurace", "správa bytostí", "silicon being", "profil", "nastavení", "správa" };
    public override string[] ChatSystem_Tags => new[] { "chat", "konverzace", "zpráva", "chatovací systém", "komunikace", "dialog", "rozhovor", "diskuse" };
    public override string[] Dashboard_Tags => new[] { "nástěnka", "monitor", "statistiky", "stav", "systém", "frekvence zpráv", "doba běhu", "paměť" };
    public override string[] Task_Tags => new[] { "úloha", "práce", "provedení", "priorita", "závislost", "stav", "automatizace", "správa" };
    public override string[] Timer_Tags => new[] { "časovač", "plánování", "spouštění", "perioda", "kalendář", "připomínka", "automatický", "časování" };
    public override string[] Permission_Tags => new[] { "oprávnění", "zabezpečení", "řízení přístupu", "správa oprávnění", "autorizace", "ochrana soukromí", "ochrana", "ověření" };
    public override string[] Config_Tags => new[] { "konfigurace", "nastavení", "možnosti", "správa konfigurace", "předvolby", "přizpůsobení", "systém", "parametry" };
    public override string[] FAQ_Tags => new[] { "časté otázky", "nápověda", "problémy", "podpora", "řešení problémů", "průvodce", "asistence", "odpovědi" };
    public override string[] Memory_Tags => new[] { "paměť", "historie", "záznam", "paměťový systém", "aktivita", "vystopování", "vyhledávání", "protokol" };
    public override string[] OllamaSetup_Tags => new[] { "Ollama", "instalace", "model", "stažení", "lokální AI", "konfigurace", "nastavení", "spuštění" };
    public override string[] BailianDashScope_Tags => new[] { "Bailian", "DashScope", "Alibaba Cloud", "cloudová AI", "API", "konfigurace", "model", "placené" };
    public override string[] AIClients_Tags => new[] { "AI klient", "AI služba", "model", "konfigurace", "lokální", "cloud", "Ollama", "DashScope", "nastavení" };
    
    public override string[] BeingSoul_Tags => new[] { "soubor duše", "osobnost", "prompt", "role", "chování", "konfigurace", "charakter", "pracovní průvodce", "systémový prompt" };
    public override string[] AuditLog_Tags => new[] { "protokol auditů", "Token", "statistiky využití", "monitor", "spotřeba", "analýza", "trend", "export", "CSV" };
    public override string[] KnowledgeGraph_Tags => new[] { "graf znalostí", "znalosti", "vizualizace", "trojice", "entita", "relace", "síť", "učení", "správa" };
    public override string[] WorkNotes_Tags => new[] { "pracovní poznámky", "poznámky", "záznamy", "deník", "protokol", "Markdown", "klíčová slova", "verze", "vyhledávání" };
    public override string[] Projects_Tags => new[] { "správa projektů", "projekt", "spolupráce", "úkol", "člen", "archiv", "tým", "pracovní prostor", "pokrok" };
    public override string[] Logging_Tags => new[] { "systém protokolování", "protokol", "záznamy", "debug", "chyba", "varování", "monitor", "sledování", "konzole", "soubor" };
    
    public override string GettingStarted => @"
# Rychlý start

## Spuštění systému

### Spuštění dvojitým kliknutím (doporučeno)

Najděte soubor programu a dvojklikem jej spusťte:
- **Windows**: `SiliconLife.Default.exe`
- Systém se automaticky spustí a **automaticky otevře prohlížeč**

Je to tak jednoduché! Není potřeba žádná konfigurace.

## První použití

Při prvním spuštění systém **automaticky dokončí veškerou inicializaci**:
- ✅ Automaticky vytvoří Silicon Curatora
- ✅ Použije vestavěný soubor duše (prompt)
- ✅ Automaticky uloží konfiguraci
- ✅ Všechny služby jsou automaticky připraveny

Stačí počkat, až se otevře prohlížeč, a můžete začít používat!

## Přehled rozhraní

Rozhraní systému se skládá ze dvou hlavních částí:

### Levý navigační panel

Obsahuje následující funkční moduly:

- **💬 Chat** - Konverzace se Silicon Being
- **📊 Nástěnka** - Zobrazení stavu systému
- **🧠 Silicon Being** - Zobrazení a správa Silicon Being
- **🔍 Audit** - Zobrazení záznamů operací
- **📚 Znalostní báze** - Správa znalostního grafu
- **📁 Projekt** - Správa kódových projektů
- **📝 Protokol** - Zobrazení systémových protokolů
- **⚙ Konfigurace** - Nastavení systému
- **❓ Nápověda** - Tento dokument
- **ℹ O systému** - Informace o systému

### Hlavní oblast obsahu

Zobrazuje obsah aktuální stránky, který se mění podle vybraného funkčního modulu.

## Rychlé začít

### 1. Konverzace se Silicon Being

Toto je nejčastěji používaná funkce:

1. Klikněte na ikonu **💬 Chat** vlevo
2. V levém seznamu vyberte Silicon Being (výchozí je Silicon Curator)
3. Do vstupního pole dole zadejte zprávu
4. Stiskněte `Enter` pro odeslání
5. AI vám odpoví v reálném čase

**Tip:** 
- Stiskněte `Shift + Enter` pro nový řádek
- Kliknutím na tlačítko ⏹ můžete zastavit odpověď AI

### 2. Zobrazení informací o Silicon Being

Chcete-li zobrazit podrobné informace o Silicon Being:

1. Klikněte na ikonu **🧠 Silicon Being** vlevo
2. Klikněte na libovolnou kartu Silicon Being
3. Vpravo se zobrazí podrobné informace:
   - Stav (nečinný/běžící)
   - Počet časovačů a úloh
   - Odkazy na paměť, oprávnění, historii chatu atd.

### 3. Úprava nastavení systému

Potřebujete-li upravit konfiguraci systému:

1. Klikněte na ikonu **⚙ Konfigurace** vlevo
2. Najděte položku konfigurace, kterou chcete upravit
3. Klikněte na tlačítko ""Upravit""
4. Zadejte novou hodnotu a uložte

**Běžná nastavení:**
- Změna jazyka rozhraní
- Změna motivu vzhledu
- Úprava modelu AI
- Změna přístupového portu

## Nahrávání souborů

Nechte AI analyzovat obsah souborů:

1. V rozhraní chatu klikněte na tlačítko **📁**
2. Zadejte úplnou cestu k souboru
   - Například: `C:\Users\VaseJmeno\Dokumenty\zprava.pdf`
3. Klikněte na ""Potvrdit nahrání""
4. AI soubor načte a analyzuje

**Podporované typy souborů:**
- Textové soubory: .txt, .md, .json
- Kódové soubory: .cs, .js, .py
- Konfigurační soubory: .yml, .yaml
- Ostatní: .csv, .log atd.

## Zobrazení historie konverzací

Chcete-li zkontrolovat předchozí konverzace:

1. Přejděte na stránku **🧠 Silicon Being**
2. Klikněte na Silicon Being, kterého chcete zobrazit
3. Klikněte na odkaz ""Historie chatu""
4. Procházejte všechny historické relace

## Získání nápovědy

Pokud narazíte na problémy:

- **Zobrazit nápovědu**: Klikněte na ikonu **❓ Nápověda** vlevo
- **Zobrazit protokoly**: Klikněte na ikonu **📝 Protokol** vlevo
- **Restartovat systém**: Mnoho problémů lze vyřešit restartem

## Další kroky

Nyní, když znáte základní operace, můžete:

- 📖 Přečíst si další dokumentaci nápovědy pro podrobnosti o funkcích
- 💬 Konverzovat se Silicon Curatorem a nechat ho pomoci s úkoly
- ⚙ Prozkoumat možnosti konfigurace a přizpůsobit si systém

Příjemné používání!
";

    public override string BeingManagement => @"
# Správa bytostí

## Co je Silicon Being?

Silicon Being je hlavní entitou systému. Každý Silicon Being je samostatným AI agentem s:
- **Souborem duše**: Hlavní prompt definující vzorce chování, osobnost a schopnosti
- **Paměťovým systémem**: Ukládá historii konverzací a důležité informace
- **Systémem úloh**: Provádí naplánované úlohy a automatizované operace
- **Sadou nástrojů**: Různé funkční nástroje, které může volat

## Zobrazení Silicon Being

### Seznam Silicon Being

Na stránce ""Silicon Being"" uvidíte všechny Silicon Being zobrazené jako karty:
- **Název**: Zobrazovaný název Silicon Being
- **Stav**: Nečinný (zelený) nebo Běžící (modrý)
- **Typ**: Pokud je načten vlastní kompilovaný kód, zobrazí se štítek typu

### Zobrazení detailů Silicon Being

Kliknutím na libovolnou kartu Silicon Being se vpravo zobrazí podrobné informace:
- **ID**: Jedinečný identifikátor Silicon Being
- **Stav**: Aktuální stav běhu
- **Vlastní kompilace**: Zda je načten vlastní kód
- **Počet časovačů**: Klikněte pro správu časovačů
- **Počet úloh**: Klikněte pro zobrazení seznamu úloh
- **Paměť**: Klikněte pro zobrazení paměťového systému
- **Oprávnění**: Klikněte pro zobrazení konfigurace oprávnění
- **Historie chatu**: Zobrazení záznamů historických konverzací
- **Pracovní poznámky**: Zobrazení pracovních poznámek
- **AI klient**: Klikněte pro zobrazení a úpravu konfigurace AI
- **Soubor duše**: Klikněte pro zobrazení a úpravu promptu

## Úprava Silicon Being

### Úprava souboru duše

Soubor duše určuje vzorce chování a rozsah schopností Silicon Being.

1. Na stránce detailů Silicon Being klikněte na odkaz ""Soubor duše""
2. Otevře se editor souboru duše (podporuje formát Markdown)
3. Upravte obsah promptu
4. Uložte změny

### Úprava konfigurace AI

Každému Silicon Being můžete nakonfigurovat samostatnou AI službu:

1. Na stránce detailů Silicon Being klikněte na odkaz ""AI klient""
2. Vyberte typ AI klienta (např. Ollama, OpenAI atd.)
3. Nakonfigurujte parametry jako koncový bod API, model, klíč atd.
4. Po uložení se změny projeví okamžitě

## Průvodce psaním souboru duše

### Základní struktura

```markdown
# Nastavení role

Jste [popis role], specializující se na:
- Dovednost 1
- Dovednost 2
- Dovednost 3

# Pokyny pro chování

1. Pokyn 1
2. Pokyn 2
3. Pokyn 3

# Pracovní postup

Při přijetí úkolu:
1. Pochopit požadavek
2. Analyzovat plán
3. Provést operace
4. Vrátit výsledek
```

### Doporučení pro psaní

1. **Jasně definujte roli**: Jasně definujte odpovědnosti a odbornou oblast Silicon Being
2. **Nastavte hranice chování**: Uveďte, co lze dělat a co by se nemělo dělat
3. **Poskytněte pracovní postup**: Vede Silicon Being, jak zpracovávat úkoly
4. **Použijte formát Markdown**: Podporuje nadpisy, seznamy, bloky kódu atd.

### Příklad: Asistent programování

```markdown
# Nastavení role

Jste profesionální asistent pro full-stack vývoj, specializující se na:
- Vývoj v C# / .NET
- Návrh architektury a kontrolu kódu
- Návrh a optimalizaci databází
- Webový frontend vývoj

# Pokyny pro chování

1. Vždy poskytujte spustitelné příklady kódu
2. Vysvětlujte klíčovou logiku kódu a návrh
3. Poskytujte doporučení pro osvědčené postupy
4. Pokud si nejste jisti, jasně to uživateli sdělte

# Standardy kódu

- Dodržujte principy SOLID
- Používejte jasné názvy
- Přidávejte nezbytné komentáře
- Zvažte zpracování chyb a okrajové případy
```

## Stav Silicon Being

### Stav běhu

- **Nečinný**: Čeká na úkoly nebo konverzace (zelený indikátor)
- **Běžící**: Právě provádí úkol nebo konverzaci (modrý indikátor)

### Monitorování Silicon Being

Na nástěnce můžete zobrazit:
- Celkový počet Silicon Being
- Stav provádění úloh pro jednotlivé Silicon Being
- Statistiky využití zdrojů

## Osvědčené postupy

1. **Oddělení odpovědností**: Různé Silicon Being jsou odpovědné za různé oblasti (např. asistent programování, asistent zákaznického servisu, datová analýza atd.)
2. **Průběžná optimalizace**: Průběžně optimalizujte soubor duše na základě skutečné zpětné vazby z používání a zlepšujte výkon Silicon Being
3. **Zálohování konfigurace**: Doporučuje se zálohovat soubor duše důležitých Silicon Being

## Odstraňování problémů

### O: Silicon Being nereaguje?

Zkontrolujte:
1. Běží služba AI správně
2. Funguje síťové připojení
3. Je soubor duše správně nakonfigurován
4. Zkontrolujte systémové protokoly pro podrobné chybové zprávy

### O: Jak změnit model AI pro Silicon Being?

Na stránce detailů Silicon Being klikněte na odkaz ""AI klient"", vyberte nový model AI a nakonfigurujte jej. Po uložení se změny projeví okamžitě. Nové konverzace budou používat nový model.

### O: Chování Silicon Being neodpovídá očekávání?

1. Zkontrolujte, zda je soubor duše jasný a srozumitelný
2. Přidejte další pokyny pro chování a omezení
3. Poskytněte konkrétní pokyny pro pracovní postup
4. Testujte a průběžně optimalizujte
";

    public override string ChatSystem => @"
# Chatovací systém

## Zahájení konverzace

1. Klikněte na ikonu **💬 Chat** v levém navigačním panelu
2. V levém seznamu vyberte Silicon Being, se kterým chcete konverzovat
3. Do vstupního pole dole zadejte zprávu
4. Stiskněte klávesu `Enter` nebo klikněte na tlačítko ""Odeslat""
5. AI vám odpoví v reálném čase (text se zobrazuje po znacích)

## Popis rozhraní

### Rozložení rozhraní

- **Levý seznam**: Zobrazuje všechny Silicon Being, kliknutím přepnete partnera konverzace
- **Střední oblast**: Zobrazuje zprávy konverzace
  - Vaše zprávy se zobrazují vpravo
  - Odpovědi AI se zobrazují vlevo
- **Vstupní oblast dole**: Vstup zpráv a tlačítko odeslání

### Popis tlačítek

- **Tlačítko Odeslat**: Odešle zprávu, kterou jste zadali
- **Tlačítko ⏹ Zastavit**: Zobrazí se při odpovědi AI, kliknutím přerušíte odpověď AI
- **Tlačítko 📁 Soubor**: Nahrajte soubor pro analýzu AI

## Základní operace

### Odeslání zprávy

- Po zadání zprávy stiskněte `Enter` pro odeslání
- Stiskněte `Shift + Enter` pro nový řádek

### Zastavení odpovědi

Pokud AI právě odpovídá, můžete:
- Kliknout na tlačítko ""⏹ Zastavit""
- Nebo odeslat novou zprávu (automaticky přeruší aktuální odpověď)

### Nahrávání souborů

Nechte AI analyzovat obsah souborů:

1. Klikněte na tlačítko **📁** vedle vstupního pole
2. Do vyskakovacího panelu zadejte cestu k souboru
   - Například: `C:\Users\VaseJmeno\Dokumenty\zprava.pdf`
3. Klikněte na ""Potvrdit nahrání""
4. AI soubor načte a analyzuje

**Podporované typy souborů**:
- Textové soubory: .txt, .md, .json, .xml
- Kódové soubory: .cs, .js, .py, .java atd.
- Konfigurační soubory: .yml, .yaml, .ini, .conf
- Ostatní soubory: .csv, .log atd.

## Funkce konverzace

### Zobrazení v reálném čase

Odpovědi AI se zobrazují po znacích, nemusíte čekat na kompletní odpověď, abyste viděli obsah.

### Vícekolová konverzace

- Systém automaticky ukládá historii konverzací
- AI si pamatuje, co bylo řečeno dříve
- Můžete přímo odkazovat na předchozí konverzace

### Volání nástrojů

AI může během konverzace automaticky volat nástroje pro:
- Dotazování na kalendář
- Správu systémové konfigurace
- Spuštění kódu
- Čtení souborů
- Vyhledávání nápovědy
- Vytváření poznámek
- Dotazování na paměť

Když AI volá nástroje, uvidíte název nástroje a výsledek provedení.

### Vícejazyčná konverzace

Můžete konverzovat s AI v libovolném jazyce, AI automaticky odpoví ve stejném jazyce.

## Zobrazení historie konverzací

Pokud chcete zobrazit minulé záznamy konverzací:

1. Klikněte na ikonu **🧠 Silicon Being** v levém navigačním panelu
2. Klikněte na kartu Silicon Being, kterého chcete zobrazit
3. V detailu vpravo najděte odkaz ""Historie chatu""
4. Klikněte pro zobrazení všech historických relací

## Časté otázky

### O: Co dělat, když AI odpovídá pomalu?

**Možné příčiny**:
- Použitý model je velký, vyžaduje více výpočetního času
- Síťová latence (při použití cloudových modelů)
- Historie konverzace je velmi dlouhá

**Řešení**:
- Zkuste použít lokální modely (jako Ollama)
- Vyberte lehčí model

### O: AI nevolá nástroje?

**Zkontrolujte následující**:
1. Je nástroj povolen?
2. Existují nějaká omezení oprávnění?
3. Podporuje model AI volání nástrojů?

### O: Jak nahrát soubor?

Klikněte na tlačítko ""📁"" vedle vstupního pole, zadejte úplnou cestu k souboru (např. `C:\Dokumenty\soubor.pdf`) a klikněte na ""Potvrdit nahrání"".

### O: Jak zobrazit předchozí konverzace?

Na stránce ""Silicon Being"" klikněte na odkaz ""Historie chatu"" příslušného Silicon Being, kde můžete zobrazit všechny historické relace.

## Doporučení pro používání

1. **Jasně vyjadřujte**: Popisujte své potřeby jasným jazykem
2. **Ptejte se postupně**: Rozdělte složité otázky na několik menších otázek
3. **Poskytněte kontext**: V případě potřeby uveďte relevantní informace na pozadí
4. **Využijte nahrávání souborů**: Když potřebujete, aby AI analyzovala soubory, přímo uveďte cestu k souboru
5. **Sledujte volání nástrojů**: Věnujte pozornost nástrojům, které AI volá, a ujistěte se, že operace odpovídají očekávání
";

    public override string Dashboard => @"
# Nástěnka

## Přehled

Nástěnka je vaše centrum pro sledování systému, které vám umožňuje na první pohled pochopit stav provádění silicon beings. Pomocí intuitivních statistických karet a grafů můžete kdykoli sledovat celkovou situaci systému.

## Hlavní funkce

- **Sledování v reálném čase**: Zobrazuje počet silicon beings, aktivní stav a využití systémových zdrojů
- **Statistiky zpráv**: Zobrazuje frekvenci nedávných chatových zpráv ve formě grafu
- **Automatická aktualizace**: Data se automaticky aktualizují, není nutná ruční operace

## Popis rozhraní

### Statistické karty

V horní části stránky se zobrazují 4 statistické karty, které zobrazují klíčové systémové informace:

| Karta | Popis |
|------|------|
| 🧠 Celkem Silicon Beings | Celkový počet všech silicon beings, které jste vytvořili |
| ⚡ Aktivní Silicon Beings | Počet silicon beings, které právě pracují nebo zpracovávají úlohy |
| ⏱️ Doba běhu systému | Doba, po kterou program běží od spuštění |
| 💾 Využití paměti | Velikost paměti počítače, kterou program aktuálně používá |

### Graf frekvence zpráv

Pod statistickými kartami se zobrazuje sloupcový graf, který zobrazuje počet zpráv v každém časovém bodě za posledních 20 minut:
- Čím vyšší sloupec, tím častější byla interakce zpráv v tomto období
- Pokud je sloupec prázdný, znamená to, že v tomto období nedošlo k žádné interakci zpráv

## Jak přistoupit

1. Spusťte program Silicon Life Collective

2. Otevřete prohlížeč a přistupte k webovému rozhraní programu

3. Klikněte na položku nabídky **📊 Nástěnka** v levém navigačním panelu

## Jak porozumět datům

### Aktivita Silicon Beings

- Pokud je aktivní počet blízký celkovému: znamená to, že většina beings je zaměstnána prací
- Pokud je aktivní počet 0: znamená to, že všechny beings odpočívají a čekají na nové úlohy

### Využití paměti
- Běžný rozsah: 50-300 MB (závisí na počtu vytvořených beings a délce historie konverzací)
- Pokud zjistíte, že paměť neustále roste a překračuje 500 MB, doporučuje se restartovat program

### Trend frekvence zpráv
- Sledujte změny výšky sloupců, abyste pochopili, v kterou dobu systém používáte nejčastěji

## Často kladené otázky

### O: Proč se statistiky neaktualizují?

**A:** Zkontrolujte následující body:
1. Zda nejsou v konzole prohlížeče chyby JavaScriptu
2. Zda jsou síťové požadavky normální (F12 → karta Network)
3. Zda backendová služba běží normálně
4. Zkuste obnovit stránku (F5)

### O: Graf se zobrazuje prázdný nebo bez dat?

**A:** Možné důvody:
1. Systém byl právě spuštěn, ještě nejsou žádné záznamy interakcí zpráv
2. Za posledních 20 minut nebyly generovány žádné zprávy
3. Služba ChatSystem nebyla správně inicializována

### O: Doba běhu se zobrazuje nesprávně?

**A:** Doba běhu se počítá od spuštění aplikace, pokud se zobrazuje abnormálně:
1. Zkontrolujte, zda je systémový čas správný
2. Restartujte aplikaci pro resetování časování

### O: Jak ručně aktualizovat data?

**A:** Aktuální verze se automaticky aktualizuje, pokud potřebujete ruční aktualizaci:
- Stiskněte F5 pro obnovení celé stránky
- Nebo stiskněte Ctrl+F5 pro vynucené obnovení (vymazání mezipaměti)

## Doporučení pro použití

### Denní sledování

1. **Pravidelně kontrolujte**: Doporučuje se otevírat každý den, abyste pochopili stav provádění systému
2. **Sledujte trendy**: Pomocí grafu frekvence zpráv pochopíte, v kterou dobu jej používáte nejčastěji
3. **Sledujte paměť**: Pokud využití paměti překročí 500 MB, můžete zvážit restartování programu
4. **Porozumějte aktivitě**: Podle počtu aktivních beings posuďte, zda systém funguje normálně

### Doporučení pro optimalizaci

1. **Kontrolujte počet beings**: Vytvoření příliš mnoha beings zabere více paměti
2. **Čistěte staré konverzace**: Pravidelně čistěte nepotřebnou historii konverzací pro uvolnění úložného prostoru
3. **Kombinujte se systémovými nástroji**: Můžete sledovat systémové zdroje společně se správcem úloh počítače

### Co dělat při problémech?

Pokud se nástěnka nezobrazuje správně, můžete zkusit následující kroky:

1. **Zkontrolujte prohlížeč**: Otevřete konzoli prohlížeče (stiskněte F12), zkontrolujte, zda nejsou chybové zprávy
2. **Zkontrolujte síťové připojení**: Potvrďte, že program běží a síť je normální
3. **Zkontrolujte protokoly programu**: Zkontrolujte, zda program nemá abnormální protokoly
4. **Restartujte program**: Pokud žádná z výše uvedených metod nefunguje, zkuste program zavřít a znovu otevřít
";

    public override string Task => @"
# Úlohy

## Přehled

Systém úloh a časovačů zaznamenává automatizované provádění Silicon Being. Můžete zobrazit seznam úloh a stav časovačů a zjistit, co Silicon Being dělá a kdy provádí operace.

## Systém úloh

### Co jsou úlohy?

Úlohy jsou pracovní položky, které Silicon Being právě provádí nebo dokončil, například:
- Zpracování úloh automaticky vytvořených AI
- Pracovní položky generované systémem
- Provádění úloh spuštěných časovači

### Zobrazení seznamu úloh

**Metoda 1: Zobrazení všech úloh**

1. Klikněte na ikonu ""Úlohy"" v levém navigačním panelu (pokud je k dispozici)
2. Stránka zobrazí seznam úloh všech Silicon Being

**Metoda 2: Zobrazení úloh konkrétního Silicon Being**

1. Přejděte na stránku **🧠 Silicon Being**
2. Klikněte na Silicon Being, kterého chcete zobrazit
3. V detailu najděte odkaz ""Úlohy""
4. Klikněte pro vstup na stránku úloh

### Informace o úlohách

Každá úloha zobrazuje následující informace:

- **Název úlohy**: Název úlohy
- **Stav**:
  - Čekající (žlutý)
  - Běžící (modrý)
  - Dokončený (zelený)
  - Selhal (červený)
  - Zrušený (šedý)
- **Priorita**: Úroveň priority úlohy
- **Přiřazeno**: Silicon Being provádějící tuto úlohu
- **Čas vytvoření**: Čas vytvoření úlohy
- **Popis**: Podrobný popis úlohy

### Popis stavů úloh

- **Čekající**: Úloha byla vytvořena a čeká na provedení
- **Běžící**: Úloha se právě provádí
- **Dokončený**: Úloha byla úspěšně dokončena
- **Selhal**: Úloha selhala, můžete zobrazit chybové zprávy
- **Zrušený**: Úloha byla zrušena

## Systém časovačů

### Co jsou časovače?

Časovače jsou mechanismy automatického spouštění, které umožňují Silicon Being provádět operace v určeném čase. Systém používá kalendářní systém k definování spouštěcích podmínek.

### Zobrazení seznamu časovačů

**Metoda 1: Zobrazení všech časovačů**

1. Klikněte na ikonu ""Časovače"" v levém navigačním panelu (pokud je k dispozici)
2. Stránka zobrazí seznam časovačů všech Silicon Being

**Metoda 2: Zobrazení časovačů konkrétního Silicon Being**

1. Přejděte na stránku **🧠 Silicon Being**
2. Klikněte na Silicon Being, kterého chcete zobrazit
3. V detailu najděte odkaz ""Časovače""
4. Klikněte pro vstup na stránku časovačů

### Informace o časovačích

Každý časovač zobrazuje následující informace:

- **Název časovače**: Identifikátor časovače
- **Stav**: Běžící nebo zastavený
- **Typ**: Typ spuštění časovače
- **Čas spuštění**: Příští čas spuštění
- **Kalendářní systém**: Použitý kalendář (např. gregoriánský, lunární atd.)
- **Počet spuštění**: Celkový počet spuštění
- **Čas vytvoření**: Čas vytvoření časovače
- **Poslední spuštění**: Čas posledního spuštění

### Typy časovačů

Systém podporuje různé způsoby spuštění:

- **Intervalové spuštění**: Spustí se každých X časových jednotek
  - Například: každé 2 hodiny, každých 30 minut
  
- **Kalendářní spuštění**: Spustí se podle kalendářních podmínek
  - Například: každý den v 9:00, každé pondělí, 1. každý měsíc
  - Podporuje gregoriánský, lunární a další kalendářní systémy

## Zobrazení historie provádění

### Historie provádění časovačů

Zjistěte stav provádění časovačů:

1. Přejděte na stránku časovačů
2. Najděte časovač, který chcete zobrazit
3. Klikněte na odkaz ""Historie provádění""
4. Zobrazte všechny záznamy spuštění

### Detaily provádění

Podrobné informace o každém provedení:

1. V historii provádění najděte konkrétní provedení
2. Klikněte pro zobrazení detailů
3. Můžete zobrazit:
   - Čas provedení
   - Výsledek provedení
   - Související zprávy konverzace
   - Chybové zprávy (pokud selhalo)

### Zprávy o provádění

Zobrazte kompletní konverzaci během konkrétního provedení:

1. Na stránce detailů provedení najděte odkaz ""Zprávy""
2. Zobrazte kompletní konverzaci mezi AI a uživatelem
3. Zjistěte, jak AI zpracovala toto spuštění

## Časté otázky

### O: Jak vytvořit novou úlohu?

**A:** Úlohy jsou automaticky generovány systémem, manuální vytváření není podporováno. Když Silicon Being potřebuje provést určitou práci, automaticky vytvoří úlohu.

### O: Jak vytvořit nový časovač?

**A:** Časovače jsou automaticky spravovány Silicon Being, manuální vytváření není podporováno. Silicon Being nastaví časovače podle potřeby k provádění pravidelných úloh.

### O: Mohu smazat úlohy nebo časovače?

**A:** Systém neposkytuje funkci manuálního mazání. Úlohy a časovače jsou automaticky spravovány Silicon Being.

### O: Co dělat, když úloha zobrazuje ""Selhal""?

**Doporučení:**
1. Zkontrolujte chybové zprávy úlohy
2. Zjistěte příčinu selhání
3. Pokud je to dočasný problém, úloha se může opakovat
4. Pokud selhání přetrvává, konverzujte se Silicon Being a zjistěte situaci

### O: Časovač se nespustil?

**Zkontrolujte:**
1. Je časovač ve stavu běhu?
2. Jsou splněny spouštěcí podmínky?
3. Běží Silicon Being normálně?
4. Zkontrolujte historii provádění a zjistěte situaci

### O: Jak zjistit, co Silicon Being právě dělá?

**Metody:**
1. Zobrazte seznam úloh a zjistěte aktuálně prováděné úlohy
2. Zobrazte seznam časovačů a zjistěte nadcházející operace
3. Zobrazte historii provádění a zjistěte minulé aktivity
4. Přímo konverzujte se Silicon Being a zeptejte se

### O: Co znamená priorita úlohy?

**A:** Priorita označuje důležitost úlohy. Čím menší číslo, tím vyšší priorita. Úlohy s vysokou prioritou budou zpracovány přednostně.

## Doporučení pro používání

1. **Pravidelně kontrolujte**: Zjistěte stav automatizovaného provádění Silicon Being
2. **Sledujte selhání úloh**: Včas zpracujte abnormální situace
3. **Zobrazujte historii provádění**: Zjistěte pracovní vzorce AI
4. **Kombinujte s konverzací**: Diskutujte se Silicon Being o stavu úloh a časovačů

## Technické informace

### Ukládání dat

Data úloh a časovačů jsou uložena v systémovém datovém adresáři a přidružena k Silicon Being:
```
data/
  beings/
    {ID Silicon Being}/
      tasks/      （Data úloh）
      timers/     （Data časovačů）
```

### Automatická správa

Systém automaticky:
- Vytváří a spravuje úlohy
- Spouští časovače
- Zaznamenává historii provádění
- Čistí expirovaná data

Nemusíte nic manuálně spravovat, systém se postará o vše.
";

    public override string Timer => @"
# Časovače

## Co jsou časovače?

Časovače jsou automatický mechanismus upozornění křemíkových bytostí. Když nastane nastavený čas, křemíková bytost automaticky provede příslušný úkol.

## Dva typy časovačů

### Jednorázový časovač

Časovač, který se spustí pouze jednou. Po spuštění automaticky končí.

**Vhodné scénáře:**
- Upozornění, abyste v určitý čas něco udělali
- Proveďte jednorázový úkol v určité datum

### Periodický časovač

Časovač, který se bude opakovaně spouštět. Po každém spuštění systém automaticky vypočítá čas dalšího spuštění.

**Vhodné scénáře:**
- Úkoly spouštěné denně ve stanovený čas (např. ranní zpráva každé ráno v 9 hodin)
- Úkoly spouštěné pravidelně každý týden nebo měsíc
- Úkoly spouštěné pravidelně podle lunárních svátků (např. každý lunární Nový rok)

## Stavy časovačů

Časovače mají čtyři stavy:

| Stav | Popis |
|------|------|
| **Běží** | Časovač pracuje normálně a čeká na spuštění |
| **Pozastaveno** | Časovač je dočasně zastaven a nebude se spouštět |
| **Spuštěno** | Jednorázový časovač dokončil spuštění |
| **Zrušeno** | Časovač byl zrušen a již se nespustí |

## Jak zobrazit časovače?

### Zobrazení časovačů konkrétní křemíkové bytosti

1. Přejděte na stránku **🧠 Křemíková bytost**
2. Vyberte křemíkovou bytost, kterou chcete zobrazit
3. Najděte možnost ""Časovače"" a můžete zobrazit všechny časovače této bytosti

## Informace zobrazené u časovačů

Při zobrazení časovačů můžete vidět následující informace:

| Položka | Popis |
|------|------|
| **Název** | Název časovače |
| **Popis** | Podrobný popis časovače (pokud existuje) |
| **Stav** | Aktuální stav (Běží, Pozastaveno atd.) |
| **Typ** | Jednorázový nebo periodický |
| **Další čas spuštění** | Konkrétní čas, kdy se časovač příště spustí |
| **Kalendářní systém** | Používaný kalendář (např. gregoriánský, lunární atd.) |
| **Počet spuštění** | Kolikrát se časovač již spustil |
| **Čas vytvoření** | Kdy byl časovač vytvořen |
| **Poslední čas spuštění** | Čas posledního spuštění (pokud již byl spuštěn) |

## Podporované kalendářní systémy

Časovače podporují různé kalendářní systémy, včetně:

- **Gregoriánský kalendář** (Gregorian): Mezinárodně používaný solární kalendář
- **Lunární kalendář** (Chinese Lunar): Tradiční čínský lunární kalendář
- **Další kalendářní systémy**: Islámský kalendář, kalendář Nebeských kmenů atd.
- **Intervalový kalendář** (Interval): Spouštění v pevných časových intervalech (např. každé 2 hodiny)

## Historie spuštění časovačů

Při každém spuštění časovače systém zaznamená podrobné informace o provedení.

### Zobrazení historie spuštění

1. Vyberte konkrétní časovač v seznamu časovačů
2. Zobrazte historii provádění tohoto časovače
3. Můžete vidět podrobnosti každého spuštění:
   - Čas spuštění
   - Stav provedení (úspěch, selhání atd.)
   - Zprávy z konverzace během provádění (pokud existují)
   - Informace o chybě (pokud provádění selhalo)

### Stavy historie provádění

Každé provedení má následující stavy:

| Stav | Popis |
|------|------|
| **Nespusteno** | Provedení ještě nezačalo |
| **Zahájeno** | Oznámení o zahájení bylo odesláno |
| **Probíhá** | Úkol se provádí |
| **Dokončeno** | Úkol byl úspěšně dokončen |
| **Selhalo** | Provedení úkolu selhalo |

## Často kladené otázky

### Q: Jak vytvořit nový časovač?

**A:** Časovače jsou automaticky spravovány křemíkovými bytostmi. Křemíkové bytosti vytvářejí časovače podle potřeby k provádění pravidelných úkolů.

### Q: Mohu odstranit nebo pozastavit časovače?

**A:** Ano. Systém podporuje následující operace:
- **Pozastavit časovač**: Dočasně zastavit spouštění časovače, lze obnovit podle potřeby
- **Obnovit časovač**: Obnovit pozastavený časovač do běžícího stavu. Pokud během pozastavení došlo k vynechání času spuštění, systém automaticky vypočítá čas dalšího spuštění
- **Zrušit časovač**: Trvale zrušit časovač, již se nespustí
- **Odstranit časovač**: Úplně odstranit časovač ze systému

### Q: Co dělat, když se časovač nespustí?

**Zkontrolujte následující:**
1. Zda je časovač ve stavu **Běží** (ne Pozastaveno nebo Zrušeno)
2. Zda nastal čas spuštění (zkontrolujte ""Další čas spuštění"")
3. Zda křemíková bytost běží normálně

### Q: Jak zjistit, jak časovače fungují?

**Metody:**
1. Zobrazte seznam časovačů a zjistěte nadcházející operace
2. Zobrazte historii provádění a porozumějte minulým aktivitám
3. Zeptejte se přímo křemíkové bytosti v konverzaci.

### Q: Jak periodický časovač vypočítává čas dalšího spuštění?

**A:** Po každém spuštění periodického časovače systém automaticky vypočítá čas dalšího spuštění podle kalendářního systému a nastavených podmínek. Například:
- Pokud je nastaven gregoriánský ""každý den v 9 hodin"", systém se spustí každý den v 9 hodin a poté vypočítá 9 hodin dalšího dne jako čas dalšího spuštění
- Pokud je nastaven lunární ""každý rok první den prvního měsíce"", systém se spustí během lunárního Nového roku a poté vypočítá datum dalšího lunárního Nového roku

## Doporučení pro použití

### Denní monitorování

1. **Pravidelně kontrolujte**: Pochopte automatické provádění křemíkových bytostí
2. **Sledujte historii provádění**: Pochopte pracovní režim AI
3. **Kontrolujte anomálie**: Zkontrolujte, zda existují záznamy o selhání provádění

### Doporučení pro optimalizaci

1. **Kombinujte s konverzací**: Diskutujte o stavu časovačů s křemíkovou bytostí
2. **Sledujte výsledky provádění**: Pochopte efekt časovačů prostřednictvím historie provádění
3. **Upravte strategie**: V případě potřeby nechte křemíkovou bytost upravit nastavení časovačů prostřednictvím konverzace
";

    public override string Permission => @"
# Správa oprávnění

## Co je systém oprávnění?

Systém oprávnění chrání bezpečnost vašeho systému a zabraňuje AI v provádění neautorizovaných operací. Když se AI pokusí provést určité operace (jako přístup k souborům, spouštění příkazů atd.), systém zkontroluje, zda je to povoleno.

## Jak fungují oprávnění?

### Automatické vyskakovací okno oprávnění

Když se AI pokusí provést operaci vyžadující oprávnění, systém zobrazí vyskakovací okno a zeptá se vás:

**Obsah vyskakovacího okna zahrnuje:**
- Typ oprávnění (např. přístup k souborům, spouštění příkazů atd.)
- Požadovaný zdroj (např. cesta k souboru)
- Podrobné informace

**Můžete vybrat:**
- **Povolit**: Provést tuto operaci
- **Zamítnout**: Zablokovat tuto operaci

### Pořadí ověřování oprávnění

Systém kontroluje oprávnění v následujícím pořadí:

1. **Silicon Curator**: Pokud operuje Curator, automaticky povolí
2. **Frekvenční omezení**: Zabraňuje velkému množství požadavků v krátkém čase
3. **Globální pravidla**: Přednastavená pravidla povolení/zamítnutí
4. **Vlastní pravidla**: Pravidla oprávnění, která jste napsali (pokud existují)
5. **Dotaz na uživatele**: Pokud výše uvedené nemohou rozhodnout, zobrazí se vyskakovací okno a zeptá se vás

## Vestavěná pravidla oprávnění

Systém má přednastavena některá bezpečná pravidla oprávnění:

### Pravidla přístupu k souborům

**Povolený přístup:**
- Vlastní dočasný adresář Silicon Being
- Běžné složky uživatele (Plocha, Stažené soubory, Dokumenty, Obrázky, Hudba, Videa)
- Veřejné složky uživatelů

**Zakázaný přístup:**
- Klíčové systémové adresáře (systémové složky Windows, /etc /boot atd. v Linuxu)
- Datové adresáře jiných Silicon Being

**Nepřiřazené cesty:**
- Zobrazí se vyskakovací okno a zeptá se, zda chcete povolit

## Vlastní pravidla oprávnění (pokročilá funkce)

Pokud potřebujete jemnější kontrolu oprávnění, můžete napsat vlastní pravidla oprávnění.

### Přístup na stránku úpravy oprávnění

1. Přejděte na stránku **🧠 Silicon Being**
2. Klikněte na Silicon Being, kterého chcete nakonfigurovat
3. V detailu najděte odkaz ""Oprávnění""
4. Vstupte do editoru kódu oprávnění

### Editor kódu oprávnění

Editor oprávnění je rozhraní pro úpravu kódu, které podporuje:
- Zvýraznění syntaxe kódu C#
- Automatické doplňování kódu
- Automatické ukládání
- Bezpečnostní skenování (zabrání škodlivému kódu)

**Způsob ukládání:**
- Klikněte na tlačítko ""Uložit"" v editoru
- Systém nejprve zkompiluje a zkontroluje
- Po úspěšném bezpečnostním skenování se změny projeví

### Výchozí šablona

Pokud ještě nemáte vlastní kód oprávnění, systém poskytne výchozí šablonu. Můžete ji upravit.

## Zobrazení pravidel oprávnění

### Zobrazení aktuálního seznamu pravidel

1. Vstupte na stránku úpravy oprávnění
2. Stránka zobrazí všechna pravidla oprávnění pro tohoto Silicon Being
3. Každé pravidlo obsahuje:
   - Typ oprávnění
   - Cesta k zdroji
   - Povolit/Zamítnout
   - Popis

## Historie požadavků na oprávnění

Všechny požadavky na oprávnění jsou zaznamenány v auditním protokolu:

1. Klikněte na ikonu **🔍 Audit** vlevo
2. Filtrujte záznamy související s oprávněními
3. Zobrazte historické požadavky a vaše rozhodnutí

## Časté otázky

### O: Proč byla operace AI zamítnuta?

**Možné příčiny:**
- Operace je v pravidle zamítnutí
- Bylo aktivováno frekvenční omezení
- Dříve jste vybrali zamítnutí

**Řešení:**
1. Zkontrolujte auditní protokol a zjistěte konkrétní důvod
2. V případě potřeby upravte pravidla oprávnění
3. Proveďte operaci znovu

### O: Co dělat, když je příliš mnoho vyskakovacích oken oprávnění?

**Doporučení:**
- Pro běžné bezpečné operace zvažte napsání vlastních pravidel pro automatické povolení
- Zkontrolujte, zda můžete upravit pravidla a snížit počet vyskakovacích oken

### O: Je vlastní kód oprávnění nebezpečný?

**Bezpečnostní záruka:**
- Kód bude prošel bezpečnostním skenováním
- Škodlivý kód bude zamítnut
- Selhání kompilace se neprojeví

**Doporučení:**
- Pokud neznáte programování, doporučujeme používat výchozí pravidla
- Před úpravou zálohujte původní kód
- Otestujte před použitím v produkčním prostředí

### O: Chybná konfigurace oprávnění způsobila nepoužitelnost?

**Řešení:**
1. Operujte jako Silicon Curator (Curator má nejvyšší oprávnění)
2. Smažte vlastní kód oprávnění (vymažte kód a uložte)
3. Systém obnoví výchozí pravidla

### O: Mohu nastavit různá oprávnění pro různé Silicon Being?

**A:** Ano. Každý Silicon Being má samostatnou konfiguraci oprávnění, která se vzájemně neovlivňují.

## Bezpečnostní doporučení

1. **Opatrně povolujte citlivé operace**: Jako mazání souborů, spouštění příkazů atd.
2. **Pravidelně kontrolujte auditní protokol**: Zjistěte historii operací AI
3. **Náhodně neměňte pravidla oprávnění**: Pokud nerozumíte jejich dopadu
4. **Udržujte systém aktualizovaný**: Získejte nejnovější bezpečnostní ochranu

## Popis typů oprávnění

Systém podporuje následující typy oprávnění:

- **Síťový přístup**: AI se pokouší přístup k síťovým zdrojům
- **Spouštění příkazů**: AI se pokouší spustit programy příkazového řádku
- **Přístup k souborům**: AI se pokouší číst nebo zapisovat soubory
- **Volání funkcí**: AI se pokouší volat specifické funkce
- **Přístup k datům**: AI se pokouší přístup k systémovým datům

Každý typ má různou úroveň bezpečnosti a způsob zpracování.
";

    public override string Config => @"
# Konfigurace

## Co je správa konfigurace?

Stránka správy konfigurace vám umožňuje upravit různá nastavení systému, včetně AI služeb, sítě, jazyka, motivu rozhraní atd.

## Jak používat stránku konfigurace?

1. Klikněte na ikonu **⚙ Konfigurace** v levém navigačním panelu
2. Stránka zobrazí několik skupin konfigurace, každá skupina obsahuje několik položek konfigurace
3. Najděte položku konfigurace, kterou chcete upravit, a klikněte na tlačítko ""Upravit"" vpravo
4. Do vyskakovacího editačního pole zadejte novou hodnotu
5. Klikněte na tlačítko ""Uložit""

## Popis skupin konfigurace

### Základní nastavení

Obsahuje základní konfiguraci systému:

- **Datový adresář**: Umístění složky pro uložení všech systémových dat
  - Výchozí hodnota: `./data`
  - Doporučení: Pokud nemáte speciální požadavky, ponechte výchozí

- **Jazyk**: Jazyk zobrazení rozhraní systému
  - Podpora: zjednodušená čínština, tradiční čínština, angličtina, japonština, korejština, němčina, španělština atd.
  - Po změně: Stránka se automaticky obnoví a použije nový jazyk

### Nastavení AI

Konfigurace připojení a modelu AI služby:

- **Typ AI klienta**: Vyberte AI službu k použití
  - Ollama (lokální běh, doporučeno)
  - OpenAI (cloudová služba)
  - Ostatní služby kompatibilní s OpenAI API

- **Konfigurace AI**: Podrobná konfigurace AI služby
  - `endpoint`: Adresa API (např. `http://localhost:11434`)
  - `model`: Název modelu k použití (např. `qwen3.5:cloud`)
  - `temperature`: Míra kreativity odpovědi (0-1, výchozí 0.7)
  - `maxTokens`: Maximální délka odpovědi (výchozí 4096)

**Úprava konfigurace AI**:
1. Klikněte na tlačítko upravit pro ""Konfigurace AI""
2. Otevře se editor slovníku
3. Můžete přidat, upravit nebo odstranit položky konfigurace
4. Klikněte na ""Uložit"" pro projevení změn

### Nastavení běhu

Řízení chování systému během běhu:

- **Časový limit provedení**: Maximální čas provedení pro jednu úlohu
  - Výchozí: 10 minut
  - Doporučení: Pokud úkoly nejsou příliš složité, ponechte výchozí

- **Maximální počet timeoutů**: Kolikrát po sobě jdoucích timeoutů aktivuje ochranný mechanismus
  - Výchozí: 3krát
  - Funkce: Zabraňuje nekonečnému opakování systému

- **Časový limit watchdogu**: Po jaké době nečinnosti se systém restartuje
  - Výchozí: 10 minut
  - Funkce: Automaticky obnoví zaseknutý systém

- **Minimální úroveň protokolu**: Které úrovně protokolů zaznamenávat
  - Trace: Nejpodrobnější (obsahuje všechny ladicí informace)
  - Debug: Ladicí informace
  - Info: Obecné informace (doporučeno)
  - Warning: Pouze upozornění
  - Error: Pouze chyby

### Nastavení Web

Konfigurace parametrů webového serveru:

- **Web port**: Port pro přístup k systému
  - Výchozí: 8080
  - Přístupová adresa: `http://localhost:8080`
  - Po změně: Je třeba restartovat systém, aby se změny projevily

- **Povolit přístup z intranetu**: Zda povolit přístup jiným zařízením v místní síti
  - Vypnuto (výchozí): Pouze místní zařízení mohou přistupovat
  - Zapnuto: Jiná zařízení ve stejné síti mohou také přistupovat
  - Poznámka: Po zapnutí jsou vyžadována oprávnění správce

- **Web motiv**: Motiv rozhraní
  - Můžete vybrat různé motivy pro změnu vzhledu rozhraní
  - Změny se projeví okamžitě

### Nastavení uživatele

- **Přezdívka uživatele**: Vaše zobrazované jméno v systému
  - Výchozí: User
  - Můžete změnit na libovolné jméno, které se vám líbí

## Úprava položek konfigurace

### Způsoby úpravy různých typů

Systém zobrazí různé editační rozhraní podle typu položky konfigurace:

**Textový typ**:
- Zobrazí textové vstupní pole
- Přímo zadejte novou hodnotu

**Číselný typ**:
- Zobrazí číselné vstupní pole
- Můžete zadat celá čísla nebo desetinná čísla

**Booleovský typ (ano/ne)**:
- Zobrazí zaškrtávací políčko
- Zaškrtnuto znamená ""ano"", nezaškrtnuto znamená ""ne""

**Typ výčtu (rozevírací seznam)**:
- Zobrazí rozevírací seznam
- Vyberte jednu z přednastavených možností

**Časový interval**:
- Zobrazí čtyři vstupní pole: dny, hodiny, minuty, sekundy
- Vyplňte odpovídající hodnoty

**Adresářová cesta**:
- Zobrazí vstupní pole pro cestu a tlačítko ""Procházet""
- Kliknutím na ""Procházet"" můžete vybrat složku
- Můžete také přímo zadat cestu

**Typ slovníku (klíč-hodnota)**:
- Zobrazí editor klíč-hodnota
- Můžete přidat více řádků klíč-hodnota
- Kliknutím na tlačítko ""Přidat"" přidáte nový řádek
- Kliknutím na tlačítko ""Smazat"" odstraníte řádek

### Uložení konfigurace

- Po každé úpravě konfigurace klikněte na ""Uložit""
- Většina konfigurací se projeví okamžitě
- Některé konfigurace (jako port) vyžadují restart systému

## Časté otázky

### O: Po změně portu nemohu přistupovat k systému?

**Řešení**:
1. Zkontrolujte, zda není port používán jiným programem
2. Potvrďte, zda brána firewall umožňuje tento port
3. Přistupujte pomocí nového portu: `http://localhost:novy-port`

### O: Jak obnovit výchozí konfiguraci?

**Metoda 1**: Manuální úprava
1. Vstupte na stránku konfigurace
2. Postupně změňte všechny položky konfigurace zpět na výchozí hodnoty

**Metoda 2**: Smazání konfiguračního souboru
1. Zavřete systém
2. Smažte soubor `config.json`
3. Restartujte systém (automaticky vytvoří výchozí konfiguraci)

### O: Co dělat, když připojení AI selže?

**Zkontrolujte následující**:
1. Běží služba AI správně
2. Je adresa koncového bodu správná
3. Pokud je to cloudová služba, je klíč API správný
4. Funguje síťové připojení správně

**Řešení**:
1. Vstupte do skupiny ""Nastavení AI""
2. Klikněte na tlačítko upravit pro ""Konfigurace AI""
3. Zkontrolujte, zda jsou `endpoint` a `model` správné
4. Po úpravě uložte

### O: Kdy se projeví změny konfigurace?

- **Okamžitě**: Jazyk, motiv, konfigurace AI, přezdívka uživatele atd.
- **Vyžaduje restart**: Web port, nastavení přístupu z intranetu

### O: Kde je konfigurační soubor?

Konfigurační soubor se nachází v souboru `config.json` v kořenovém adresáři běhu systému.

## Doporučení pro používání

1. **Opatrně upravujte**: Udržujte výchozí hodnoty pro položky konfigurace, které si nejste jisti
2. **Zaznamenávejte změny**: Po úpravě konfigurace zaznamenejte obsah a důvod změny
3. **Zálohujte konfiguraci**: Před důležitými úpravami můžete zkopírovat soubor `config.json` jako zálohu
4. **Testovací prostředí**: Pokud je to možné, nejprve ověřte konfiguraci v testovacím prostředí
5. **Bezpečnost především**: Před zapnutím přístupu z intranetu se ujistěte o bezpečnosti sítě
";

    public override string FAQ => @"
# Časté otázky

## Začínáme

### O: Jak spustit systém?

**A:** Dvojklikem na soubor programu spustíte systém. Systém automaticky otevře prohlížeč a vstoupí do rozhraní.

### O: Co musím udělat při prvním spuštění?

**A:** Nic! Systém automaticky dokončí inicializaci, včetně vytvoření Silicon Curator. Stačí počkat, až se otevře prohlížeč, a můžete začít používat.

### O: Po spuštění systému se neotevřel prohlížeč?

**A:** Ručně přistupte na `http://localhost:8080`.

## AI konverzace

### O: Co dělat, když AI odpovídá pomalu?

**Možné příčiny:**
- Použitý model je velký
- Síťová latence (při použití cloudové AI)
- Historie konverzace je velmi dlouhá

**Řešení:**
- Použijte lokální AI službu (jako Ollama)
- Vyberte lehčí model

### O: Odpověď AI neodpovídá očekávání?

**Doporučení:**
1. Zkontrolujte, zda je soubor duše jasný a srozumitelný
2. Poskytněte více informací na pozadí během konverzace
3. Zkuste přesněji popsat své potřeby

### O: AI nevolá nástroje?

**Zkontrolujte:**
1. Je nástroj povolen?
2. Existují nějaká omezení oprávnění?
3. Podporuje model AI volání nástrojů?

### O: Jak nechat AI analyzovat soubor?

**Metoda:**
1. V rozhraní chatu klikněte na tlačítko ""📁 Soubor""
2. Zadejte úplnou cestu k souboru (např. `C:\Dokumenty\zprava.pdf`)
3. Klikněte na ""Potvrdit nahrání""
4. AI soubor načte a analyzuje

## Silicon Being

### O: Jak vytvořit nový Silicon Being?

**A:** Systém aktuálně nepodporuje přímé vytváření Silicon Being. Silicon Curator může vytvářet a spravovat další Silicon Being, můžete konverzovat s Curatorem a nechat ho pomoci s vytvořením.

### O: Jak změnit chování Silicon Being?

**Metoda:**
1. Vstupte na stránku ""Silicon Being""
2. Klikněte na Silicon Being, kterého chcete upravit
3. Klikněte na odkaz ""Soubor duše""
4. Upravte obsah promptu
5. Uložte

### O: Jak nakonfigurovat různou AI pro Silicon Being?

**Metoda:**
1. Vstupte na stránku ""Silicon Being""
2. Klikněte na cílový Silicon Being
3. Klikněte na odkaz ""AI klient""
4. Vyberte AI službu a nakonfigurujte
5. Uložte

### O: Silicon Being nereaguje?

**Zkontrolujte:**
1. Běží služba AI správně
2. Funguje síťové připojení
3. Zkontrolujte systémové protokoly pro podrobné chyby

## Nastavení systému

### O: Jak změnit jazyk systému?

**Metoda:**
1. Klikněte na ikonu ""⚙ Konfigurace"" vlevo
2. Najděte položku konfigurace ""Jazyk""
3. Klikněte na ""Upravit""
4. Vyberte jazyk z rozevíracího seznamu
5. Uložte (stránka se automaticky obnoví)

### O: Jak změnit motiv rozhraní?

**Metoda:**
1. Vstupte na stránku ""Konfigurace""
2. Najděte položku konfigurace ""Web motiv""
3. Klikněte na ""Upravit""
4. Vyberte motiv, který se vám líbí
5. Uložte

### O: Jak změnit přístupový port?

**Metoda:**
1. Vstupte na stránku ""Konfigurace""
2. Najděte položku konfigurace ""Web port""
3. Klikněte na ""Upravit""
4. Zadejte nový číslo portu (např. 9000)
5. Uložte a restartujte systém

**Poznámka:** Po změně portu musíte používat nový port pro přístup, např. `http://localhost:9000`

### O: Jak povolit přístup jiným zařízením v místní síti?

**Metoda:**
1. Vstupte na stránku ""Konfigurace""
2. Najděte položku konfigurace ""Povolit přístup z intranetu""
3. Klikněte na ""Upravit""
4. Zaškrtněte ""Ano""
5. Uložte

**Poznámka:** Vyžaduje oprávnění správce, po změně mohou jiná zařízení přistupovat prostřednictvím `http://vase-IP:8080`

## Historie chatu

### O: Jak zobrazit minulé konverzace?

**Metoda:**
1. Vstupte na stránku ""Silicon Being""
2. Klikněte na Silicon Being, kterého chcete zobrazit
3. V detailu najděte odkaz ""Historie chatu""
4. Klikněte pro vstup a procházení všech historických relací

### O: Jak smazat historii konverzací?

**A:** Systém aktuálně neposkytuje funkci mazání historie konverzací. Historie konverzací se automaticky ukládá, aby Silicon Being mohl pamatovat předchozí obsah konverzací.

## Data a úložiště

### O: Kde jsou uložena data?

**A:** Ve výchozím nastavení jsou uložena ve složce `data` v adresáři běhu programu.

### O: Jak zálohovat data?

**Metoda:** Zkopírujte celou složku `data` na bezpečné místo.

### O: Jak migrovat na nový počítač?

**Kroky:**
1. Zavřete systém
2. Zkopírujte celou složku `data`
3. Nainstalujte systém na nový počítač
4. Umístěte složku `data` do adresáře programu na novém počítači
5. Spusťte systém

## Konfigurační soubor

### O: Kde je konfigurační soubor?

**A:** V souboru `config.json` v adresáři běhu programu.

### O: Mohu přímo upravovat konfigurační soubor?

**A:** Ano, ale nedoporučuje se. Doporučujeme upravovat prostřednictvím stránky konfigurace webového rozhraní, což je bezpečnější a méně náchylné k chybám.

### O: Co dělat, když jsem špatně upravil konfiguraci?

**Řešení:**
1. Zavřete systém
2. Smažte soubor `config.json`
3. Restartujte systém (automaticky vytvoří výchozí konfiguraci)

**Nebo:** Pokud máte zálohu, můžete obnovit zálohovaný konfigurační soubor.

## Výkon

### O: Systém běží pomalu?

**Doporučení:**
- Použijte lokální AI službu (jako Ollama)
- Vyberte lehčí model AI
- Snižte počet současně běžících úloh

### O: Vysoké využití paměti?

**Doporučení:**
- Použijte lehčí model AI
- Pravidelně čistěte nepotřebná data

## Získání nápovědy

### O: Co dělat, když narazím na problémy?

**Doporučené kroky:**
1. **Zobrazte dokumentaci nápovědy**: Klikněte na ikonu ""❓ Nápověda"" vlevo
2. **Zobrazte protokoly**: Na stránce ""📝 Protokol"" zobrazte systémové protokoly
3. **Restartujte systém**: Mnoho problémů lze vyřešit restartem

### O: Jak zobrazit systémové protokoly?

**Metoda:**
1. Klikněte na ikonu ""📝 Protokol"" vlevo
2. Procházejte seznam protokolů
3. Můžete filtrovat podle úrovně (chyby, upozornění atd.)

## Ostatní otázky

### O: Které jazyky systém podporuje?

**A:** Podporuje zjednodušenou čínštinu, tradiční čínštinu, angličtinu, japonštinu, korejštinu, němčinu, španělštinu a mnoho dalších jazyků.

### O: Potřebuji připojení k internetu pro používání?

**A:** Závisí na AI službě, kterou používáte:
- **Lokální AI (jako Ollama)**: Nepotřebuje připojení k internetu
- **Cloudová AI (jako OpenAI)**: Potřebuje připojení k internetu

### O: Je systém bezpečný?

**A:** Ano. Systém má vestavěný mechanismus správy oprávnění, všechny operace AI jsou ověřovány oprávněními a citlivé operace vyžadují vaše potvrzení.

### O: Mohu přizpůsobit funkce?

**A:** Systém podporuje rozšíření funkcí psaním kódu, ale to vyžaduje určité znalosti programování. Běžní uživatelé by měli používat funkce poskytované systémem.
";

    public override string Memory => @"
# Paměťový systém

## Co je paměťový systém?

Paměťový systém zaznamenává celou historii aktivit Silicon Being, včetně konverzací, volání nástrojů, systémových událostí atd. Prostřednictvím paměťového systému můžete zjistit, co Silicon Being dělal, kdy to dělal a jaké byly výsledky.

## Jak přistupovat k paměťovému systému?

Vstupte prostřednictvím stránky Silicon Being:

1. Klikněte na ikonu **🧠 Silicon Being** vlevo
2. Klikněte na kartu Silicon Being, kterého chcete zobrazit
3. V detailu vpravo najděte odkaz ""Paměť""
4. Klikněte pro vstup na stránku paměti

## Popis stránky paměti

### Rozložení stránky

- **Nahoře**: Výběr Silicon Being a statistické informace
- **Oblast filtrů**: Typ, čas, klíčová slova a další podmínky filtru
- **Oblast seznamu**: Zobrazuje seznam položek paměti
- **Oblast detailů**: Po kliknutí na položku paměti se zobrazí podrobný obsah

### Typy paměti

Systém zaznamenává následující typy paměti:

- **Konverzace**: Obsah konverzací mezi uživatelem a AI
- **Volání nástrojů**: Záznamy provádění volání nástrojů AI
- **Systémové události**: Důležité události běhu systému
- **Shrnutí**: Komprimovaná shrnutí konverzací nebo událostí

## Zobrazení paměti

### Procházení seznamu paměti

1. Vyberte Silicon Being, kterého chcete zobrazit
2. Stránka zobrazí seznam paměti tohoto Silicon Being
3. Každá paměť zobrazuje:
   - Ikona typu
   - Shrnutí obsahu
   - Čas
   - Stav (úspěch/selhání)

### Zobrazení detailů paměti

Kliknutím na libovolnou položku paměti se zobrazí:
- Kompletní obsah
- Časové razítko
- Související parametry
- Výsledek provedení (pokud jde o volání nástroje)

### Vystopování původního kontextu

Pro některé položky paměti systém poskytuje funkci ""vystopování"":
1. Klikněte na tlačítko ""Vystopovat"" v detailu paměti
2. Systém zobrazí kompletní kontext v době vzniku této paměti
3. Pomůže vám pochopit, proč AI tenkrát tak jednala

## Filtrování paměti

### Filtrování podle typu

Klikněte na filtr typu a vyberte typ paměti k zobrazení:
- Pouze konverzace
- Pouze volání nástrojů
- Pouze systémové události
- Pouze shrnutí

### Filtrování podle času

Můžete vybrat časové období:
- Zadejte počáteční datum
- Zadejte koncové datum
- Zobrazí se pouze paměť v tomto časovém období

### Vyhledávání klíčových slov

Do vyhledávacího pole zadejte klíčová slova:
- Podporuje čínštinu a angličtinu
- Vyhledává veškerý obsah paměti
- Po zadání se automaticky zobrazí výsledky shody

**Tipy pro vyhledávání:**
- Použití konkrétních klíčových slov snáze najde výsledky
- Můžete kombinovat filtr typu a času
- Pokud je výsledků příliš mnoho, zkuste konkrétnější klíčová slova

### Zobrazení shrnutí nebo původních záznamů

- **Zobrazit vše**: Zobrazí veškerou paměť
- **Pouze shrnutí**: Zobrazí pouze komprimované záznamy shrnutí
- **Pouze původní**: Zobrazí pouze původní podrobné záznamy

## Statistiky paměti

V horní části stránky se zobrazují statistické informace:
- Celkový počet pamětí
- Počet pamětí jednotlivých typů
- Využití úložiště

Prostřednictvím těchto statistik můžete zjistit:
- Úroveň aktivity Silicon Being
- Hlavní typy prováděných aktivit
- Zda je potřeba vyčistit starou paměť

## Stránkování

Pokud je pamětí mnoho, systém je zobrazí stránkovaně:
- Na jedné stránce se ve výchozím nastavení zobrazuje 20 záznamů
- Pomocí tlačítek čísel stránek přejděte na další stránku
- Můžete upravit počet zobrazení na stránku

## Časté otázky

### O: Jak najít konkrétní konverzaci?

**Metoda:**
1. Do vyhledávacího pole zadejte klíčová slova z konverzace
2. Filtr typu vyberte ""Konverzace""
3. Pokud znáte přibližný čas, můžete nastavit časové období
4. Procházejte výsledky vyhledávání

### O: Co dělat, když paměť zabírá příliš mnoho místa?

**Doporučení:**
- Paměť je automaticky spravována, obvykle není potřeba manuální zásah
- Systém vytváří shrnutí pro kompresi historických záznamů
- Pokud opravdu potřebujete, můžete kontaktovat správce systému

### O: Mohu smazat paměť?

**A:** Systém neposkytuje funkci mazání paměti. Paměť je důležitou historií Silicon Being, uchování paměti pomáhá AI lépe rozumět a odpovídat na otázky.

### O: Mohu exportovat paměť?

**A:** Aktuální verze nepodporuje funkci exportu. Data paměti jsou uložena v systémovém datovém adresáři.

### O: Proč jsou některé paměti ""Shrnutí""?

**A:** Systém automaticky komprimuje delší konverzace nebo události do shrnutí, aby ušetřil úložný prostor a zvýšil efektivitu dotazů. Shrnutí zachovává klíčové informace, ale vynechává detaily.

### O: Jak zobrazit podrobné informace o volání nástrojů AI?

**Metoda:**
1. Filtr typu vyberte ""Volání nástrojů""
2. Najděte odpovídající záznam volání nástroje
3. Klikněte pro zobrazení podrobných informací
4. Můžete zobrazit název nástroje, parametry, výsledek provedení atd.

### O: Vyhledávání paměti nenajde výsledky?

**Doporučení:**
1. Zkontrolujte, zda jsou klíčová slova správná
2. Zkuste použít jiná klíčová slova
3. Zkontrolujte, zda je časové období nastaveno správně
4. Potvrďte, zda je vybraný Silicon Being správný
5. Zkuste nezadávat podmínky filtru a zobrazit veškerou paměť

## Doporučení pro používání

1. **Pravidelně kontrolujte**: Zjistěte stav aktivit Silicon Being
2. **Dobře používejte filtry**: Rychle najděte potřebné informace
3. **Použijte vystopování**: Pochopte proces rozhodování AI
4. **Sledujte statistiky**: Zjistěte stav běhu systému

## Technické informace

### Ukládání dat

Data paměti jsou uložena v systémovém datovém adresáři:
```
data/
  beings/
    {ID Silicon Being}/
      memory/
        （Soubory paměti）
```

### Automatická správa

Systém automaticky:
- Zaznamenává důležité aktivity
- Vytváří shrnutí konverzací
- Udržuje časový index
- Optimalizuje výkon dotazů

Nemusíte manuálně spravovat paměť, systém se postará o vše.
";

    public override string OllamaSetup => @"
# Instalace Ollama a stahování modelů

## Co je Ollama?

Ollama je open-source nástroj pro běh lokálních AI modelů, který vám umožňuje provozovat velké jazykové modely na vašem počítači bez připojení k internetu (po stažení modelu).

**Výhody:**
- Zcela lokální běh, ochrana soukromí
- Podporuje různé AI modely
- Snadná instalace a používání
- Zdarma a open-source

## Stažení a instalace Ollama

### Systém Windows

**Krok 1: Stažení instalačního balíčku**

Navštivte oficiální stránku pro stažení Ollama:
- URL: https://ollama.com/download
- Automaticky se stáhne instalační balíček pro Windows (ollama-setup.exe)

**Krok 2: Spuštění instalačního programu**

1. Dvojklikem na stažený soubor `ollama-setup.exe`
2. Podle pokynů instalačního průvodce dokončete instalaci
3. Po dokončení instalace se Ollama automaticky spustí

**Krok 3: Ověření instalace**

1. Otevřete příkazový řádek (stiskněte `Win + R`, zadejte `cmd`, stiskněte Enter)
2. Zadejte následující příkaz:
   ```
   ollama --version
   ```
3. Pokud se zobrazí číslo verze, instalace byla úspěšná

### Systém Mac

**Metoda 1: Stažení instalačního balíčku**

1. Navštivte https://ollama.com/download
2. Stáhněte instalační balíček pro Mac
3. Dvojklikem na instalační balíček a přetáhněte do složky Aplikace

**Metoda 2: Instalace pomocí terminálu**

Otevřete terminál (Terminal) a zadejte:
```bash
brew install ollama
```

**Ověření instalace:**
```bash
ollama --version
```

### Systém Linux

**Jednoduchý instalační příkaz:**

Otevřete terminál a spusťte:
```bash
curl -fsSL https://ollama.com/install.sh | sh
```

**Ověření instalace:**
```bash
ollama --version
```

## Spuštění Ollama

### Windows

- Po instalaci se Ollama automaticky spustí
- Ikona Ollama je vidět v systémové liště (vpravo dole)
- Kliknutím pravým tlačítkem na ikonu můžete spravovat

### Mac / Linux

V terminálu spusťte:
```bash
ollama serve
```

Nebo přímo spusťte:
```bash
ollama
```

Tím se otevře interaktivní menu.

## Stahování a spouštění modelů

### Co je model?

Model je ""mozkem"" AI, který určuje schopnosti AI. Různé modely mají různé charakteristiky:
- **Různá velikost**: Čím větší model, tím silnější schopnosti, ale vyžaduje více paměti
- **Různé specializace**: Některé jsou dobré v konverzacích, jiné v programování

### IQ modelu (jednotka B)

""IQ"" modelu AI se obvykle vyjadřuje pomocí **B (Billion, miliarda parametrů)**:
- **7B-8B**: Základní úroveň, zvládá jednoduché úkoly, ale v komplexních scénářích může výkon selhávat
- **13B-14B**: Střední úroveň, dobrý výkon pro většinu každodenních úkolů
- **32B a výše**: Vysoká úroveň, silnější schopnosti komplexního uvažování a pochopení dlouhého textu

**Tento systém doporučuje používat modely s 8B a více pro lepší uživatelský zážitek.**

### Lokální model vs. cloudový model

Ollama podporuje dva způsoby běhu modelů:

**Lokální model:**
- Soubor modelu se stáhne do vašeho počítače
- Zcela lokální běh, po stažení není potřeba internet
- Omezeno konfigurací vašeho hardwaru (paměť, grafická karta)
- Obvykle 4B-70B parametrů
- Zdarma, bez omezení

**Cloudový model:**
- Model běží na cloudovém serveru Ollama
- Stačí stáhnout identifikátor modelu (velmi malý)
- Může spouštět ultrav velké modely, které domácí počítač nezvládne (obvykle 200B a více)
- Vyžaduje připojení k internetu
- Má omezení využití (**obnovuje se týdně**)
- Stačí zapnout cloudovou funkci klienta Ollama

### Doporučená konfigurace hardwaru

### Doporučené modely

Následují běžně používané zdarma modely:

| Název modelu | IQ | Velikost | Charakteristiky | Vhodné scénáře |
|---------|------|------|------|---------|
| **qwen3.5:8b** | 8B | cca 4-5GB | Silné schopnosti v čínštině, dobrý celkový výkon | Každodenní konverzace, psaní, překlad |
| **qwen3.5:14b** | 14B | cca 8-9GB | Silnější schopnosti v čínštině, zlepšené uvažování | Komplexní úkoly, zpracování dlouhého textu |
| **qwen3.5:32b** | 32B | cca 18-20GB | Vysoký IQ, vynikající komplexní uvažování | Profesionální úkoly, hluboká analýza |
| **llama3:8b** | 8B | cca 4-5GB | Silné schopnosti v angličtině, dobrá univerzálnost | Anglické konverzace, univerzální úkoly |
| **llama3:70b** | 70B | cca 40GB | Ultravysoký IQ, top v angličtině | Vysoce obtížné anglické úkoly |
| **gemma3:4b** | 4B | cca 2-3GB | Lehký, rychlý | Rychlé odezvy, počítače s nízkou konfigurací |
| **gemma3:12b** | 12B | cca 7-8GB | Vyvážený výkon a zdroje | Každodenní používání |
| **mistral:7b** | 7B | cca 4GB | Vyvážený výkon a rychlost | Univerzální scénáře |
| **codellama:7b** | 7B | cca 4GB | Dobrý v programování | Generování kódu, ladění |
| **codellama:13b** | 13B | cca 7-8GB | Silnější schopnosti programování | Komplexní úkoly s kódem |

**Doporučení pro uživatele čínštiny: qwen3.5:8b nebo qwen3.5:14b**

### Stažení modelu

**Metoda 1: Stažení pomocí příkazového řádku**

Otevřete terminál (nebo příkazový řádek) a zadejte:

```bash
ollama pull qwen3.5
```

Systém automaticky stáhne model, což bude chvíli trvat (závisí na rychlosti sítě a velikosti modelu).

**Metoda 2: Spuštění a automatické stažení**

```bash
ollama run qwen3.5
```

Pokud model není stažen, automaticky začne stahování.

### Spuštění modelu

Po stažení spusťte model:

```bash
ollama run qwen3.5
```

Tím se otevře interaktivní rozhraní konverzace, kde můžete přímo konverzovat s AI.

**Příklad konverzace:**
```
>>> Ahoj!
Ahoj! Jsem Qwen, jak vám mohu pomoci?

>>> Napište báseň o jaru
Jarní vítr vanoucí, květy rozkvétají,
Zelení plné zahrady voní.
Vlaštovky se vracejí hledat staré hnízdo,
Lidský svět v dubnu je krásný čas.
```

Stiskněte `Ctrl + D` nebo zadejte `/bye` pro ukončení konverzace.

### Zobrazení stažených modelů

```bash
ollama list
```

Zobrazí seznam všech stažených modelů.

### Smazání nepotřebných modelů

```bash
ollama rm qwen3.5
```

## Používání Ollama v Silicon Life

### Konfigurace připojení

1. Ujistěte se, že Ollama běží
2. Otevřete systém Silicon Life
3. Vstupte na stránku **⚙ Konfigurace**
4. Najděte ""Typ AI klienta"" a vyberte `OllamaClient`
5. V ""Konfigurace AI"" nastavte:
   - **endpoint**: `http://localhost:11434` (výchozí)
   - **model**: `qwen3.5` (nebo jiný model, který jste stáhli)
6. Uložte konfiguraci

### Testování připojení

1. Vstupte na stránku **💬 Chat**
2. Vyberte Silicon Being
3. Odešlete zprávu
4. Pokud obdržíte odpověď, připojení bylo úspěšné

## Časté otázky

### O: Co dělat, když je stahování Ollama pomalé?

**Řešení:**
- Soubory modelů jsou obvykle velké (2-8GB), stažení trvá určitý čas
- Ujistěte se, že je síťové připojení stabilní
- Můžete stahovat v noci nebo když je síť nevytížená

### O: Co dělat, když se stahování přeruší?

**Řešení:**
Znovu spusťte příkaz pro stažení, bude pokračovat ve stahování:
```bash
ollama pull qwen3.5
```

### O: Jak zjistit, jak velký model můj počítač zvládne?

**Doporučení pro párování paměti a velikosti modelu:**
- **4GB paměti**: Doporučuje se model pod 2GB (cca 2B-3B)
- **8GB paměti**: Může spustit model 4GB (cca 7B-8B)
- **16GB paměti**: Může spustit model 8GB (cca 13B-14B)
- **32GB paměti**: Může spustit model 16GB, ale bude znatelně zpomalený a zahřívat se (cca 32B)
- **64GB a více**: Může plynule spouštět větší modely

**Důležité upozornění:**
- Notebook s 32GB paměti při spuštění modelu kolem 16B zaznamená **znatelné zpomalení** a **zvýšené zahřívání**
- Toto není chyba, ale normální projev nedostatečných hardwarových zdrojů
- **Doporučení**: V tomto případě vyberte menší model (8B-14B) nebo upgradujte na vyšší konfiguraci hardwaru

**Začněte testovat s lehkým modelem**, pokud běží plynule, zkuste větší model.

### O: Co dělat, když se Ollama nespustí?

**Zkontrolujte:**
1. Je port 11434 používán jiným programem?
2. Přeinstalujte Ollama
3. Zobrazte protokol Ollama pro chybové zprávy

### O: Co dělat, když je model pomalý?

**Doporučení:**
- Použijte menší model (např. gemma3 místo qwen3.5)
- Zavřete další programy zabírající paměť
- Zkontrolujte, zda konfigurace počítače splňuje požadavky

### O: Mohu v Silicon Life používat více modelů současně?

**A:** Ano. Stáhněte více modelů v Ollama a poté v konfiguraci AI Silicon Being v Silicon Life vyberte různé modely pro různé Silicon Being.

### O: Potřebuje Ollama připojení k internetu?

**A:** 
- **Při stahování modelu**: Ano, potřebuje internet
- **Při běhu modelu**: Ne, po stažení modelu není potřeba internet

### O: Kolik místa na disku model zabírá?

**A:** 
- Malý model: cca 2-4GB
- Střední model: cca 4-8GB
- Velký model: 8GB a více

Doporučuje se ponechat dostatek místa na disku.

## Získání další nápovědy

- **Oficiální web Ollama**: https://ollama.com
- **Dokumentace Ollama**: https://docs.ollama.com

## Další kroky

Po instalaci Ollama a stažení modelu můžete:
- Nakonfigurovat a používat lokální AI v Silicon Life
- Užívat si zcela lokální AI služby
- Chránit vaše soukromí a bezpečnost dat

Příjemné používání!
";

    public override string BailianDashScope => @"
# Průvodce používáním platformy Alibaba Cloud Bailian

## Co je Alibaba Cloud Bailian?

Alibaba Cloud Bailian (DashScope) je platforma pro velké modely poskytovaná Alibaba Cloud, která nabízí různé vysoce kvalitní AI modely, včetně Tongyi Qianwen, DeepSeek, GLM, Kimi atd.

**Výhody:**
- Vysoký IQ modelu (až několik set B)
- Není potřeba lokální hardware, běží v cloudu
- Podporuje různé špičkové AI modely
- Platba podle využití, náklady pod kontrolou
- Kompatibilní s formátem OpenAI API

## Registrace a aktivace služby

### Krok 1: Registrace účtu Alibaba Cloud

1. Navštivte oficiální web Alibaba Cloud: https://www.aliyun.com
2. Klikněte na ""Registrovat zdarma""
3. Podle pokynů dokončete registraci (podporuje registraci pomocí telefonu, e-mailu)
4. Dokončete ověření skutečné identity (potřebujete Alipay nebo bankovní kartu)

### Krok 2: Aktivace služby Bailian

1. Přihlaste se do konzoly Alibaba Cloud
2. Vyhledejte ""Bailian"" nebo ""DashScope""
3. Klikněte pro vstup na stránku produktu Bailian
4. Klikněte na ""Aktivovat nyní""
5. Přečtěte si a souhlaste se smlouvou o službách
6. Dokončete aktivaci

### Krok 3: Získání API Key

1. Vstupte do konzoly Bailian
2. V levém menu najděte ""Správa API Key"" nebo ""Správa klíčů""
3. Klikněte na ""Vytvořit API Key""
4. Pojmenujte klíč (např. ""SiliconLife"")
5. Zkopírujte a uložte API Key (**zobrazí se pouze jednou, uložte jej bezpečně**)

## Konfigurace Bailian v Silicon Life

### Kroky konfigurace

1. Otevřete systém Silicon Life
2. Vstupte na stránku **⚙ Konfigurace**
3. Najděte ""Typ AI klienta"" a vyberte `DashScopeClient`
4. V ""Konfigurace AI"" vyplňte:
   - **API Key**: Vložte zkopírovaný API Key
   - **Region (oblast)**: Vyberte oblast serveru (např. beijing)
   - **Model (model)**: Vyberte model (**po vyplnění API Key a výběru oblasti systém automaticky načte všechny dostupné modely v této oblasti**)
5. Uložte konfiguraci

**Tip:**
- Nejprve musíte vyplnit API Key a vybrat oblast, poté se načte rozbalovací seznam modelů
- Pokud se načtení seznamu modelů nezdaří, zobrazí se seznam doporučených modelů

### Výběr oblasti

Systém podporuje následující oblasti:

| Oblast | Umístění | Popis |
|------|------|------|
| **beijing** | Peking, Čína | Výchozí doporučení, rychlý přístup v pevninské Číně |
| **virginia** | Virginie, USA | Vhodné pro zahraniční uživatele |
| **singapore** | Singapur | Asijsko-pacifická oblast |
| **hongkong** | Hongkong, Čína | Asijsko-pacifická oblast |
| **frankfurt** | Frankfurt, Německo | Evropská oblast |

**Doporučení pro výběr oblasti:**
- **Uživatelé v pevninské Číně**: Vyberte beijing (Peking), nejrychlejší přístup, **ale podpora překladu je slabší**
- **Potřebujete vysoce kvalitní překlad**: Vyberte singapore (Singapur) nebo hongkong (Hongkong), lepší efekt překladu, uživatelé v pevninské Číně mohou také přistupovat
- **Zahraniční uživatelé**: Vyberte oblast nejblíže vám

### Výběr modelu

Při konfiguraci modelu systém automaticky načte seznam dostupných modelů z platformy Bailian. Pokud se načtení nezdaří, zobrazí se doporučené modely:

**Doporučené modely:**

| Název modelu | IQ | Charakteristiky | Vhodné scénáře |
|---------|------|------|---------|
| **qwen3-max** | Ultrav velký | Nejsilnější verze Tongyi Qianwen | Komplexní uvažování, profesionální úkoly |
| **qwen3.6-plus** | Velký | Vyvážený výkon a náklady | Každodenní používání (doporučeno) |
| **qwen3.6-flash** | Střední | Rychlý, nízké náklady | Rychlá odezva |
| **qwen-max** | Ultrav velký | Předchozí vlajkový model | Vysoce obtížné úkoly |
| **qwen-plus** | Velký | Vyvážený výkon | Univerzální scénáře |
| **qwen-turbo** | Střední | Nejrychlejší | Jednoduché úkoly |
| **qwen3-coder-plus** | Velký | Dobrý v programování | Generování kódu, ladění |
| **qwq-plus** | Velký | Silné schopnosti uvažování | Matematika, logické uvažování |
| **deepseek-v3.2** | Ultrav velký | Nejnovější verze DeepSeek | Silné celkové schopnosti |
| **deepseek-r1** | Ultrav velký | Specializovaný na uvažování | Komplexní uvažování |
| **glm-5.1** | Velký | Model Zhipu AI | Scénáře v čínštině |
| **kimi-k2.5** | Velký | Model Moonshot | Zpracování dlouhého textu |
| **llama-4-maverick** | Velký | Open-source model Meta | Scénáře v angličtině |

**Doporučení pro uživatele čínštiny: qwen3.6-plus nebo qwen3-max**

## Popis poplatků

### Způsob účtování

Platforma Bailian používá model **platby podle využití**:
- Účtování podle počtu tokenů
- Různé modely mají různé ceny
- Čím vyšší IQ modelu, tím dražší

### Zdarma kvóta

- Noví uživatelé obvykle mají zdarma zkušební kvótu
- Některé modely mají zdarma kvótu volání
- Konkrétní kvóta podle oznámení platformy Bailian

### Zobrazení využití

1. Přihlaste se do konzoly Bailian
2. Vstupte do ""Dotaz na využití"" nebo ""Správa účtů""
3. Zobrazte počet volání a náklady

### Doporučení pro úsporu nákladů

- Vyberte vhodný model (nemusí být nejdražší)
- Vyhněte se odesílání příliš dlouhého textu
- Pravidelně kontrolujte využití a kontrolujte náklady

### Popis statistiky využití tokenů

**Důležité upozornění:** Protože API odpověď platformy Alibaba Cloud Bailian (DashScope) **nestabilně vrací pole využití tokenů**, tento systém **nemůže statisticky sledovat** využití tokenů při používání modelů Bailian.

**To znamená:**
- Systém nezobrazuje, kolik tokenů bylo použito
- Nelze zobrazit historii spotřeby tokenů v systému
- Nelze provádět analýzu nákladů podle využití tokenů

**Jak zobrazit využití:**
- Přihlaste se do konzoly Bailian pro zobrazení skutečného využití a nákladů
- Konzola Bailian poskytuje podrobné statistiky volání a informace o účtech

## Časté otázky

### O: Kde získat API Key?

**A:** 
1. Přihlaste se do konzoly Alibaba Cloud Bailian
2. Najděte ""Správa API Key""
3. Vytvořte nový API Key
4. Zkopírujte a bezpečně uložte

### O: Co dělat, když se API Key prozradí?

**A:**
1. Okamžitě se přihlaste do konzoly Bailian
2. Smažte prozrazený API Key
3. Vytvořte nový API Key
4. Aktualizujte konfiguraci v Silicon Life

### O: Která oblast je nejlepší?

**A:**
- **Uživatelé v pevninské Číně**: Vyberte beijing (Peking), nejrychlejší
- **Zahraniční uživatelé**: Vyberte oblast nejblíže vám
- Oblast neovlivňuje kvalitu modelu, pouze rychlost přístupu

### O: Proč se načtení seznamu modelů nezdařilo?

**Možné příčiny:**
1. API Key není správný nebo vypršel
2. Problém se síťovým připojením
3. Výpadek služby Bailian

**Řešení:**
1. Zkontrolujte, zda je API Key správný
2. Zkontrolujte síťové připojení
3. Zkuste to později

### O: Mohu používat více modelů?

**A:** Ano. V Silicon Life můžete nakonfigurovat různé modely Bailian pro různé Silicon Being.

### O: Jaký je rozdíl mezi Bailian a Ollama?

| Vlastnost | Bailian (DashScope) | Ollama |
|------|------------------|--------|
| Umístění běhu | Cloud | Lokální počítač |
| Požadavky na hardware | Žádné | Vyžaduje vyšší konfiguraci |
| Velikost modelu | Až několik set B | Obvykle 4B-70B |
| Poplatky | Platba podle využití | Zdarma |
| Internet | Vyžaduje internet | Po stažení není potřeba internet |
| Soukromí | Data se odesílají do cloudu | Zcela lokální |

**Doporučení pro výběr:**
- Nízká konfigurace hardwaru → Vyberte Bailian
- Potřebujete ultravysoký IQ model → Vyberte Bailian
- Dbáte na soukromí, nechcete platit → Vyberte Ollama
- Používání v prostředí bez internetu → Vyberte Ollama

### O: Co dělat, když volání selže?

**Zkontrolujte:**
1. Je API Key správný
2. Je dostatečný zůstatek účtu
3. Nebyla překročena zdarma kvóta
4. Zobrazte chybové zprávy v konzoli Bailian

### O: Jak kontrolovat náklady?

**Doporučení:**
1. Nastavte upozornění na rozpočet (konzola Bailian)
2. Pravidelně kontrolujte využití
3. Vyberte modely s dobrým poměrem cena/výkon
4. Vyhněte se častému volání velkých modelů

## Osvědčené postupy

### 1. Výběr vhodného modelu

- **Každodenní konverzace**: qwen3.6-plus (vyvážený)
- **Komplexní uvažování**: qwen3-max nebo deepseek-r1
- **Programovací úkoly**: qwen3-coder-plus
- **Rychlá odezva**: qwen3.6-flash nebo qwen-turbo

### 2. Správa API Key

- Bezpečně uchovávejte, nesdílejte veřejně
- Pravidelně obměňujte (vytvořte nový, smažte starý)
- Vytvořte různé klíče pro různé účely

### 3. Monitorování využití

- Kontrolujte využití jednou týdně
- Nastavte upozornění na rozpočet
- Včas prošetřete abnormální využití

### 4. Optimalizace používání

- Zkrátit vstupní obsah, snížit zbytečný text
- Rozumně nastavit délku historie konverzací
- Použijte vhodný model, nesnažte se slepě o velké modely

## Získání další nápovědy

- **Oficiální web Bailian**: https://bailian.console.aliyun.com
- **Dokumentace API**: https://help.aliyun.com/zh/model-studio
- **Seznam modelů**: https://help.aliyun.com/zh/model-studio/models
- **Popis cen**: https://www.aliyun.com/price/product#/dashscope
- **Technická podpora**: Odešlete tiket nebo kontaktujte zákaznický servis Alibaba Cloud

## Další kroky

Po konfiguraci Bailian můžete:
- Používat vysoce kvalitní cloudové AI modely v Silicon Life
- Zažít AI služby s ultravysokým IQ
- Nemusíte se starat o konfiguraci lokálního hardwaru

Příjemné používání!
";
    
    public override string AIClients => @"
# Konfigurace AI klienta

## Přehled

AI klienti jsou ""mozkové konektory"" systému Silicon Life, odpovědné za komunikaci s modely umělé inteligence. Systém podporuje více AI klientů a můžete si vybrat vhodnou službu podle svých potřeb.

## Podporovaní AI klienti

Systém podporuje více AI klientů a můžete si vybrat vhodnou službu podle svých potřeb.

### Lokální AI klient

**Vlastnosti:**
- 🏠 **Lokální běh**: AI modely běží na vašem počítači
- 🔒 **Soukromí a bezpečnost**: Data nejsou nahrávána do cloudu
- 💰 **Zcela zdarma**: Žádná omezení použití
- ⚡ **Rychlá odpověď**: Žádné síťové zpoždění (lokální modely)

**Vhodné scénáře:**
- Uživatelé dbající na soukromí
- Lepší konfigurace počítače (doporučeno 16GB+ RAM)
- Neomezené použití AI
- Špatné síťové podmínky

**Požadavky na konfiguraci:**
- Je třeba nainstalovat odpovídající AI servisní software
- Je třeba stáhnout soubory AI modelů (obvykle 4-20GB)
- Doporučeno 16GB+ RAM, vyhrazená grafická karta je lepší

### Cloudový AI klient

**Vlastnosti:**
- ☁️ **Cloudová služba**: AI modely běží na vzdálených serverech
- 🚀 **Výkonný**: Může používat ultra velké modely (200B+ parametrů)
- 💳 **Platba za použití**: Má zdarma kvótu, po překročení se účtuje podle použití
- 🌍 **Podpora více regionů**: Může vybrat servery blíže k vám

**Vhodné scénáře:**
- Nižší konfigurace počítače
- Potřeba používat špičkové modely
- Příležitostné použití, nízké použití
- Chcete rychle začít bez lokální konfigurace

**Požadavky na konfiguraci:**
- Potřebujete účet a API klíč pro odpovídající platformu
- Potřebujete internetové připojení
- Má omezení kvóty použití (může se pravidelně obnovovat)

## Jak vybrat AI klienta?

## Výběrový Flowchart

```
Jaká je vaše konfigurace počítače?
├─ Vysoká konfigurace (16GB+ RAM)
│  └─ Dbáte na soukromí?
│     ├─ Ano → Vyberte lokální klient (např. Ollama)
│     └─ Ne → Oba jsou v pořádku
└─ Nízká konfigurace (8GB nebo méně)
   └─ Vyberte cloudový klient (např. DashScope)
```

### Srovnávací tabulka

| Vlastnost | Lokální klient | Cloudový klient |
|------|--------------|-----------------|
| Obtížnost instalace | Střední (je třeba nainstalovat software a modely) | Jednoduchá (potřebujete pouze API klíč) |
| Provozní náklady | Zdarma (spotřebovává elektřinu) | Má zdarma kvótu, po překročení účtování |
| Ochrana soukromí | ⭐⭐⭐⭐⭐ Zcela lokální | ⭐⭐⭐ Data procházejí cloudem |
| Výběr modelů | Omezeno konfigurací počítače | Může vybrat různé velké modely |
| Síťový požadavek | Pouze při stahování modelů | Vždy potřebné |
| Rychlost odpovědi | Rychlá (lokální) | Závisí na síti |
| Omezení použití | Žádná omezení | Má omezení kvóty |

## Konfigurovat AI klienta

### Krok 1: Vstoupit na konfigurační stránku

1. Otevřete systém Silicon Life
2. Klikněte na menu **⚙ Konfigurace** v horním navigačním panelu

### Krok 2: Vybrat typ AI klienta

1. Najděte možnost **""Typ AI klienta""** na konfigurační stránce
2. Vyberte požadovaný klient z rozbalovací nabídky:
   - Lokální klient (např. `OllamaClient`)
   - Cloudový klient (např. `DashScopeClient`)

### Krok 3: Vyplnit konfigurační informace

Po vybrání klienta se dole objeví odpovídající konfigurační položky:

#### Konfigurace lokálního klienta (např. Ollama)

| Konfigurační položka | Popis | Výchozí hodnota | Příklad |
|--------|------|--------|------|
| **endpoint** | Adresa AI služby | Závisí na konkrétní službě | např. `http://localhost:11434` |
| **model** | Název modelu k použití | Závisí na konkrétní službě | např. `qwen3.5:8b` |
| **temperature** | Úroveň kreativity (0-1) | `0.7` | `0.5` konzervativnější, `0.9` kreativnější |
| **maxTokens** | Maximální délka odpovědi | `2048` | `4096` umožňuje delší odpovědi |

**Příklad konfigurace:**
```
Typ AI klienta: Typ lokálního klienta (např. OllamaClient)
endpoint: http://localhost:11434 (vyplnit podle skutečné adresy služby)
model: qwen3.5:8b (vyplnit podle staženého modelu)
temperature: 0.7
maxTokens: 2048
```

#### Konfigurace cloudového klienta (např. DashScope)

| Konfigurační položka | Popis | Výchozí hodnota | Příklad |
|--------|------|--------|------|
| **apiKey** | API klíč | Žádný | Poskytnuto konkrétní platformou |
| **region** | Region serveru | Závisí na konkrétní platformě | např. `beijing` |
| **model** | Model k použití | Závisí na konkrétní platformě | např. `qwen3.6-plus` |
| **temperature** | Úroveň kreativity (0-1) | `0.7` | `0.5` |
| **maxTokens** | Maximální délka odpovědi | `2048` | `4096` |

**Dostupné regiony:**

| Kód regionu | Umístění | Vhodní uživatelé |
|---------|------|---------|
| `beijing` | Peking, Čína | Uživatelé pevninské Číny (doporučeno) |
| `singapore` | Singapur | Uživatelé jihovýchodní Asie |
| `hongkong` | Hongkong, Čína | Uživatelé Hongkongu, Macaa, Tchaj-wanu |
| `virginia` | USA | Severoameričtí uživatelé |
| `frankfurt` | Německo | Evropští uživatelé |

**Dostupné modely:**

| Název modelu | Vlastnosti | Vhodné scénáře |
|---------|------|---------|
| `qwen3.6-plus` | Vyvážený výkon (doporučeno) | Denní použití |
| `qwen3-max` | Nejsilnější schopnost | Komplexní úlohy |
| `qwen3.6-flash` | Rychlá odpověď | Jednoduché otázky a odpovědi |
| `qwen-max` | Vlajková loď předchozí generace | Komplexní usuzování |
| `qwen-plus` | Vylepšený předchozí generace | Obecné scénáře |
| `qwen-turbo` | Rychlý předchozí generace | Jednoduché úlohy |
| `qwen3-coder-plus` | Specifický pro programování | Generování kódu |
| `qwq-plus` | Specifický pro usuzování | Matematika, logika |
| `deepseek-v3.2` | Model třetí strany | Obecné scénáře |
| `deepseek-r1` | Usuzovací model | Hluboké myšlení |
| `glm-5.1` | Model Zhipu | Čínské scénáře |
| `kimi-k2.5` | Dlouhý kontext | Zpracování dlouhého textu |
| `llama-4-maverick` | Model Meta | Anglické scénáře |

**Příklad konfigurace:**
```
Typ AI klienta: Typ cloudového klienta (např. DashScopeClient)
apiKey: Váš API klíč (získán z odpovídající platformy)
region: beijing (vyberte nejbližší region)
model: qwen3.6-plus (vyberte podle dostupných modelů)
temperature: 0.7
maxTokens: 2048
```

### Krok 4: Uložit konfiguraci

1. Po vyplnění všech potřebných informací
2. Klikněte na tlačítko **""Uložit konfiguraci""** v dolní části stránky
3. Systém oznámí, že ukládání bylo úspěšné

### Krok 5: Otestovat připojení

1. Klikněte na menu **💬 Chat** v horním navigačním panelu
2. Vyberte silicon bytost
3. Pošlete testovací zprávu, např. ""Ahoj""
4. Pokud obdržíte odpověď, konfigurace byla úspěšná

## Často kladené otázky

### O1: Nevím, kterého klienta vybrat?

**Návrhy:**
- Pokud jste začátečník, doporučuje se nejprve použít **cloudový klient**, jednoduchá konfigurace, rychlý start
- Pokud vážíte soukromí nebo máte lepší konfiguraci počítače, vyberte **lokální klient**

### O2: Mohu používat dva klienty současně?

Ne. Systém může používat pouze jednoho AI klienta najednou. Ale můžete kdykoli přepnout na konfigurační stránce.

### O3: Ztratí se chatovací záznamy po přepnutí klientů?

Ne. Chatovací záznamy jsou uloženy v systému a jsou nezávislé na AI klientovi. I po přepnutí můžete zobrazit historické konverzace.

### O4: Lokální klient zobrazuje chybu připojení?

**Řešení:**
1. Potvrďte, že odpovídající AI servisní software běží (zkontrolujte systémový panel nebo procesy)
2. Zkontrolujte, zda je adresa endpoint správná (odkazujte na výchozí konfiguraci této služby)
3. Přistupte k adrese služby v prohlížeči, měli byste vidět odpověď
4. Potvrďte, že modely jsou staženy: použijte příkaz konkrétní služby k zobrazení seznamu modelů

### O5: Cloudový klient zobrazuje chybu ověření?

**Řešení:**
1. Zkontrolujte, zda je API klíč správný (formát odkazuje na požadavky odpovídající platformy)
2. Potvrďte, že účet není v prodlení
3. Zkontrolujte, zda nebyla překročena kvóta použití
4. Znovu vygenerujte API klíč a aktualizujte konfiguraci

### O6: Jak získat API klíč pro cloudový klient?

**Obecné kroky:**
1. Navštivte webovou stránku konzoly odpovídajícího poskytovatele AI služby
2. Přihlaste se ke svému účtu
3. Vstupte na stránku ""Správa klíčů"" nebo ""Správa API""
4. Klikněte na ""Vytvořit klíč"" nebo ""Generovat API klíč""
5. Zkopírujte vygenerovaný API klíč a řádně jej uložte

### O7: Co dělat, pokud je odpověď velmi pomalá?

**Lokální klient:**
- Zkontrolujte využití zdrojů počítače (CPU, RAM, grafická karta)
- Zkuste použít menší model (např. 8B místo 32B)
- Zavřete jiné programy spotřebovávající zdroje

**Cloudový klient:**
- Zkontrolujte kvalitu síťového připojení
- Zkuste vybrat bližší region serveru
- Vyhněte se špičkovým hodinám sítě

### O8: Co je parametr temperature?

temperature řídí kreativitu odpovědí AI:
- **0.0-0.3**: Velmi konzervativní, předvídatelné odpovědi, vhodné pro faktické otázky
- **0.4-0.7**: Vyvážený režim, vhodný pro denní konverzaci (doporučeno)
- **0.8-1.0**: Velmi kreativní, různorodé odpovědi, vhodné pro kreativní psaní

### O9: Kolik by mělo být nastaveno maxTokens?

- **1024**: Krátké odpovědi, vhodné pro jednoduché otázky a odpovědi
- **2048**: Střední délka, vhodná pro obecnou konverzaci (doporučeno)
- **4096+**: Dlouhé odpovědi, vhodné pro komplexní úlohy nebo generování dlouhého textu

Poznámka: Čím větší nastavení, tím více zdrojů a času spotřebuje.

### O10: Mohou různé bytosti používat různé klienty?

Ano. Každá silicon bytost může nezávisle konfigurovat typ AI klienta.

**Metoda nastavení:**
1. Vstupte na stránku správy bytostí
2. Vyberte bytost ke konfiguraci
3. Nastavte AIClientType v konfiguraci této bytosti
4. Pokud bytost není nastavena, použije se globální konfigurace

## Osvědčené postupy

### 1. Vybrat modely založené na úlohách

- **Denní konverzace**: Použijte malé až střední modely (8B-14B nebo rychlé modely)
- **Komplexní analýza**: Použijte velké modely (32B+ nebo pokročilé modely)
- **Generování kódu**: Použijte vyhrazené programovací modely
- **Kreativní psaní**: Použijte vyšší temperature (0.8-0.9)

### 2. Optimalizovat náklady (Cloudový klient)

- Pravidelně kontrolujte použití, abyste se vyhnuli překročení免费 kvóty
- Použijte rychlé modely pro jednoduché úlohy
- Použijte pokročilé modely pro komplexní úlohy
- Nastavte maxTokens rozumně, abyste se vyhnuli plýtvání

### 3. Zlepšit výkon (Lokální klient)

- Prioritně stahujte často používané modely, abyste se vyhnuli stahování za běhu
- Udržujte AI službu rezidentní pro snížení doby spuštění
- Použijte akceleraci grafické karty (pokud máte grafickou kartu NVIDIA)
- Pravidelně čistěte nepoužívané modely pro uvolnění místa

### 4. Doporučení pro bezpečnost

- Nesdílejte API klíč s ostatními
- Pravidelně měňte API klíč
- Lokální klienti jsou omezeni pouze na lokální přístup, nevystavujte veřejné síti
- Zálohujte důležité konfigurační soubory

## Získat nápovědu

Pokud narazíte na problémy:
1. Zkontrolujte sekci [Často kladené otázky](#často-kladené-otázky)
2. Zkontrolujte [Správa konfigurace](./config) pro instrukce systémové konfigurace
3. Zkontrolujte systémové protokoly pro podrobnosti o chybě
";
    
    public override string BeingSoul => @"
# Soubor Duše

## Přehled

Soubor Duše je základní konfigurační soubor Silicon Bytosti, určující **osobnost, vzorce chování, profesionální schopnosti a pracovní metody** každé bytosti.

Můžete chápat soubor duše jako **""nastavení osobnosti""** nebo **""pracovní manuál""** bytosti. Automaticky se načítá během každé AI konverzace a řídí silicon bytost, aby myslela a jednala způsobem, který očekáváte.

## Role Souboru Duše

Soubor duše je **hnací silou** silicon bytosti a definuje:

- 🎭 **Pozicování Role**: Kdo je tato silicon bytost, v jakých oblastech vyniká
- 📋 **Pokyny pro Chování**: Jak by měla odpovídat uživatelům, jakým principům následovat
- 🔄 **Pracovní Postup**: Jak zpracovávat úlohy po jejich přijetí, v kolika krocích
- ⚠️ **Hranice Chování**: Co lze dělat, co by se nemělo dělat
- 💡 **Profesionální Požadavky**: Standardy kódu, formát výstupu, styl jazyka atd.

## Jak Upravit Soubor Duše

### Úprava prostřednictvím Webového Rozhraní

1. Přejděte na stránku **Silicon Bytosti**
2. Klikněte na kartu silicon bytosti, kterou chcete upravit
3. Klikněte na odkaz **Soubor Duše**
4. Upravte obsah v Markdown editoru
5. Klikněte na tlačítko **Uložit**

### Úprava prostřednictvím AI Asistenta

Můžete také přímo konverzovat se **Silicon Kurátorem** a požádat jej o pomoc při úpravě souboru duše:

```
Prosím, pomozte mi upravit soubor duše programovacího asistenta pro přidání podpory vývoje v Python
```

Silicon Kurátor vám pomůže aktualizovat obsah souboru duše.

## Průvodce Psaním Souborů Duše

### Základní Struktura

Soubory duše se píší ve **formátu Markdown**. Doporučuje se následující struktura:

```markdown
# Nastavení Role

Jste [popis role], specializovaný na:
- Dovednost 1
- Dovednost 2
- Dovednost 3

# Pokyny pro Chování

1. Pokyn 1
2. Pokyn 2
3. Pokyn 3

# Pracovní Postup

Při přijetí úlohy:
1. Porozumět požadavkům
2. Analyzovat přístup
3. Provést operace
4. Nahlásit výsledky

# Standardy Kódu

- Následovat určité standardy kódování
- Poskytovat potřebné komentáře
- Zohlednit hraniční případy
```

### Tipy pro Psaní

1. **Jasná Definice Role**: Jasně určete odpovědnosti a oblasti odbornosti silicon bytosti
2. **Nastavit Hranice Chování**: Vysvětlete, co lze dělat a co by se nemělo dělat
3. **Poskytnout Pracovní Postup**: Řiďte silicon bytost, jak zpracovávat úlohy
4. **Použít Formát Markdown**: Podporuje nadpisy, seznamy, bloky kódu atd.
5. **Být Konkrétní, Ne Vágní**: Použijte konkrétní příklady místo abstraktních popisů

## Praktické Příklady

### Příklad 1: Programovací Asistent

```markdown
# Nastavení Role

Jste profesionální full-stack vývojový asistent, specializovaný na:
- Vývoj C# / .NET
- Návrh architektury a recenzování kódu
- Návrh a optimalizace databází
- Vývoj webového frontendu

# Pokyny pro Chování

1. Vždy poskytovat spustitelné příklady kódu
2. Vysvětlovat klíčovou logiku kódu a designové myšlení
3. Poskytovat doporučení osvědčených postupů
4. Při nejistotě jasně informovat uživatele

# Standardy Kódu

- Následovat principy SOLID
- Používat jasné pojmenování
- Přidávat potřebné komentáře
- Zohlednit zpracování výjimek a hraniční případy
```

### Příklad 2: Asistent Zákaznického Servisu

```markdown
# Nastavení Role

Jste přátelský asistent zákaznického servisu, odpovědný za:
- Odpovídání na časté otázky uživatelů
- Zpracování jednoduchých stížností
- Vedení uživatelů prostřednictvím operací
- Shromažďování feedbacku od uživatelů

# Pokyny pro Chování

1. Vždy zůstat zdvořilý a trpělivý
2. Vysvětlovat jednoduchým, snadno pochopitelným jazykem
3. Při nemožnosti řešení promptně předat lidskému agentovi
4. Zaznamenávat otázky a feedback uživatelů

# Pracovní Postup

1. Pozdravit uživatele
2. Porozumět potřebám uživatele
3. Poskytnout řešení
4. Potvrdit, zda je problém vyřešen
5. Poděkovat uživateli a ukončit konverzaci
```

### Příklad 3: Asistent Analýzy Dat

```markdown
# Nastavení Role

Jste expert na analýzu dat, specializovaný na:
- Čištění a předzpracování dat
- Statistickou analýzu a vizualizaci
- Predikci trendů a detekci anomálií
- Generování zpráv o datech

# Požadavky na Výstup

1. Poskytovat jasné závěry analýzy
2. Používat grafy pro pomocné vysvětlení
3. Anotovat zdroje dat a předpoklady
4. Poskytnout akční doporučení
```

## Umístění Ukládání Souboru Duše

Soubor duše každé silicon bytosti je uložen v jejím datovém adresáři:

```
DataDirectory/SiliconManager/{BeingGUID}/soul.md
```

Systém automaticky spravuje tento soubor a nemusíte ručně operovat souborový systém.

## Často Kladené Otázky

### O: Budou změny v souboru duše účinné okamžitě?

**A:** Ano, budou účinné okamžitě po uložení. Příště, když silicon bytost odpoví, použije nový soubor duše.

### O: Existuje limit velikosti pro soubory duše?

**A:** Neexistuje přísný limit velikosti, ale doporučuje se udržovat jej v rozumném rozsahu (do několika tisíc slov). Příliš dlouhé soubory duše mohou ovlivnit rychlost odpovědi.

### O: Mohu zcela odstranit soubor duše?

**A:** Nedoporučuje se odstraňovat soubor duše. Pokud je obsah prázdný, silicon bytost ztratí pokyny pro chování a může produkovat neočekávané odpovědi.

### O: Jak zálohuji soubor duše?

**A:** Doporučuje se pravidelně zálohovat soubory duše důležitých silicon bytostí. Můžete:
1. Kopírovat obsah do lokálního souboru prostřednictvím webového rozhraní
2. Použít exportní funkci systému (pokud je podporována)
3. Přímo zálohovat datový adresář

### O: Jaký je vztah mezi souborem duše a paměťovým systémem?

**A:** Soubor duše definuje **dlouhodobé vzorce chování**, zatímco paměťový systém zaznamenává **krátkodobou historii konverzací**. Spolupracují:
- Soubor duše: Říká silicon bytosti ""jakou roli máte""
- Paměťový systém: Říká silicon bytosti ""o čem jsme dříve mluvili""

### O: Mohou různé silicon bytosti používat stejný soubor duše?

**A:** Ano, ale nedoporučuje se. Každá silicon bytost by měla mít jedinečné pozicování role, aby se předešlo funkční duplikaci.

## Osvědčené Postupy

1. **Průběžná Optimalizace**: Průběžně optimalizujte soubor duše na základě skutečné uživatelské zpětné vazby
2. **Správa Verzí**: Zálohujte aktuální verzi před významnými úpravami
3. **Testovací Ověření**: Otestujte efekt prostřednictvím konverzace po úpravě
4. **Udržovat Stručný**: Vyjadřujte základní požadavky stručným jazykem
5. **Vyhnout se Rozporům**: Ujistěte se, že nejsou konflikty mezi různými pokyny
6. **Pravidelná Revize**: Pravidelně kontrolujte, zda je soubor duše stále aplikovatelný

## Řešení Problémů

### Problém: Chování silicon bytosti nesplňuje očekávání

**Kontrolní Seznam:**
1. Je obsah souboru duše jasný a přesný?
2. Přidali jste dostatečné pokyny pro chování?
3. Existují protichůdné instrukce?
4. Poskytli jste specifický pracovní postup?

**Řešení:**
1. Přepsat vágní popisy
2. Přidat více specifických pokynů pro chování
3. Poskytnout příklady k ilustraci očekávaného výstupu
4. Testovat a průběžně optimalizovat

### Problém: Nepodařilo se uložit soubor duše

**Možné Příčiny:**
1. Problémy s oprávněními souborového systému
2. Nedostatek místa na disku
3. Soubor je obsazen jiným procesem

**Řešení:**
1. Zkontrolovat systémové protokoly pro podrobné informace o chybě
2. Potvrdit, že datový adresář je zapisovatelný
3. Zkusit znovu po restartu systému

## Související Funkce

- 🤖 [Správa Bytostí](being-management) - Vytvářet a spravovat silicon bytosti
- 💬 [Chatovací Systém](chat-system) - Konverzovat se silicon bytostmi
- 🧠 [Konfigurace AI Klienta](ai-clients) - Konfigurovat služby AI
- 📝 [Paměťový Systém](memory) - Spravovat historii konverzací
";

    public override string AuditLog => @"
# Protokol Auditů

## Přehled

Protokol auditů (Audit Log) je **systém monitorování využití Tokenů** platformy Silicon Life, který vám pomáhá sledovat a spravovat spotřebu AI volání všech silicon bytostí.

S protokolem auditů můžete:
- 📊 **Zobrazit statistiky spotřeby Tokenů**: Zjistit, kolik Tokenů každá silicon bytost spotřebovala
- 📈 **Analyzovat trendy využití**: Zobrazit změny využití Tokenů podle hodin, dnů, měsíců
- 🔍 **Filtrovat a porovnávat**: Filtrovat data podle časového rozsahu, silicon bytosti, typu AI klienta
- 💾 **Exportovat data**: Exportovat auditní data do souboru CSV pro další analýzu

**Co je Token?**  
Token je základní jednotka, se kterou modely AI zpracovávají text. Každé volání AI silicon bytostí spotřebuje Tokeny:
- **Prompt Tokens**: Text odeslaný AI (včetně souborů duše, historie zpráv, uživatelských zpráv atd.)
- **Completion Tokens**: Text vygenerovaný AI odpovědí
- **Total Tokens**: Celkem spotřebovaných Tokenů (Prompt + Completion)

## Přístup k Protokolu Auditů

### Vstup na Stránku Auditů

1. Spustit platformu Silicon Life
2. Kliknout na ""Audity"" (ikona 📊) v levém navigačním menu
3. Vstoupit na řídicí panel protokolu auditů

### Požadavky na Oprávnění

- 📊 **Zobrazit protokol auditů**: Oprávnění Curator (kurátor)
- 🔒 Běžné silicon bytosti nemohou přistupovat k funkci auditů
- To zajišťuje, že pouze správci mohou zobrazit globální využití Tokenů

## Funkce Řídicího Panelu Auditů

### 📈 Graf Trendů

Horní část stránky auditů zobrazuje **graf trendu využití Tokenů**, který obsahuje:

- **Výběr časového rozsahu**:
  - Dnes (Today): Zobrazit dnešní využití Tokenů po hodinách
  - Tento týden (Week): Zobrazit trend využití Tokenů tento týden
  - Tento měsíc (Month): Zobrazit využití Tokenů tento měsíc po dnech
  - Tento rok (Year): Zobrazit využití Tokenů tento rok po měsících

- **Data trendů**:
  - Spojnicový graf zobrazuje změnu množství využití Tokenů v čase
  - Najetí myší na datový bod zobrazí konkrétní hodnotu
  - Pomáhá objevovat špičky a propady využití

### 📊 Souhrn Statistik

Řídicí panel zobrazuje klíčová statistická data:

- **Celkový počet požadavků**: Celkový počet AI volání
- **Počet úspěšných/neúspěšných**: Kolik AI volání bylo úspěšných a kolik selhalo
- **Spotřeba Tokenů**:
  - Celkový počet vstupních Tokenů (Prompt Tokens)
  - Celkový počet výstupních Tokenů (Completion Tokens)
  - Celková spotřeba Tokenů (Total Tokens)

### 🔍 Funkce Filtrů

Můžete použít filtry k přesnému zobrazení specifických dat:

- **Filtrovat podle silicon bytosti**: Zobrazit využití Tokenů konkrétní silicon bytosti
- **Filtrovat podle AI klienta**: Zobrazit využití konkrétní AI služby (jako Ollama, Alibaba Cloud Bailian)
- **Kombinované filtry**: Použít více filtračních podmínek současně

### 📋 Detailní Kategorizace

#### Statistiky podle Typu AI Klienta

Zobrazit využití Tokenů každého AI klienta:
- OllamaClient (lokální služba Ollama)
- DashScopeClient (Alibaba Cloud Bailian)
- Další nakonfigurované AI klienty

Pomáhá vám pochopit:
- Která AI služba se používá nejvíce
- Porovnání využití lokálních modelů a cloudových modelů
- Zda je třeba upravit konfiguraci AI klienta

#### Statistiky podle Silicon Bytosti

Zobrazit využití Tokenů každé silicon bytosti:
- Název silicon bytosti
- Celková spotřeba Tokenů
- Počet požadavků

Pomáhá vám objevit:
- Které silicon bytosti jsou nejaktivnější
- Zda je spotřeba Tokenů rozumná
- Zda je třeba upravit frekvenci využití některých bytostí

### 💾 Export Dat

Kliknutím na tlačítko ""Exportovat CSV"" můžete:

- Exportovat všechna auditní data pod aktuálními filtračními podmínkami do souboru CSV
- Soubor CSV obsahuje:
  - Časové razítko
  - ID silicon bytosti
  - Typ AI klienta
  - Počet vstupních Tokenů
  - Počet výstupních Tokenů
  - Celkový počet Tokenů
  - Zda bylo úspěšné

**Scénáře využití**:
- Hlubší analýza dat v Excelu
- Vytváření vlastních reportů
- Dlouhodobé uchování auditních záznamů
- Sdílení dat s dalšími členy týmu

## Scénáře Využití

### Scénář 1: Monitorování Spotřeby Tokenů

**Požadavek**: Chcete vědět celkovou spotřebu AI služeb tento měsíc

**Kroky operace**:
1. Vstoupit na stránku auditů
2. Vybrat ""Tento měsíc"" v časovém rozsahu
3. Zobrazit statistiku ""Celková spotřeba Tokenů""
4. Zobrazit graf trendů a pochopit denní změny spotřeby

**Výsledek**: Můžete jasně vidět, kolik Tokenů bylo celkem spotřebováno tento měsíc a které dny měly nejvyšší využití.

### Scénář 2: Řešení Abnormální Spotřeby

**Požadavek**: Zjistili jste abnormálně vysoké náklady na AI službu a chcete zjistit příčinu

**Kroky operace**:
1. Vstoupit na stránku auditů
2. Vybrat časové období abnormálních nákladů
3. Zobrazit statistiky podle silicon bytosti
4. Najít silicon bytost s nejvyšší spotřebou
5. Kliknout na tuto bytost a zobrazit detailní trend
6. Pokud je to nutné, exportovat CSV pro hlubší analýzu

**Výsledek**: Můžete identifikovat, která silicon bytost spotřebovala velké množství Tokenů v jakém čase, a dále zkontrolovat, zda je její soubor duše nebo konfigurace úkolů rozumná.

### Scénář 3: Porovnání Různých AI Služeb

**Požadavek**: Chcete porovnat využití lokálního Ollama a Alibaba Cloud Bailian

**Kroky operace**:
1. Vstoupit na stránku auditů
2. Vybrat ""Tento měsíc"" v časovém rozsahu
3. Zobrazit statistiky podle typu AI klienta
4. Porovnat využití Tokenů OllamaClient a DashScopeClient

**Výsledek**: Můžete pochopit:
- Poměr využití lokálních modelů a cloudových modelů
- Která služba je ekonomičtější
- Zda je třeba upravit výchozí AI klienta

### Scénář 4: Monitorování Konkrétní Silicon Bytosti

**Požadavek**: Vytvořili jste nového programovacího asistenta a chcete monitorovat, zda je jeho využití Tokenů rozumné

**Kroky operace**:
1. Vstoupit na stránku auditů
2. Vybrat ID nebo název tohoto programovacího asistenta ve filtru
3. Zobrazit jeho trend využití Tokenů
4. Porovnat s využitím jiných silicon bytostí

**Výsledek**: Můžete vyhodnotit efektivitu tohoto programovacího asistenta a určit, zda je třeba optimalizovat jeho soubor duše nebo způsob práce.

## Nástroj Auditů (AI Volání)

Silicon bytosti mohou také používat nástroj **TokenAuditTool** k dotazování na využití Tokenů:

### Dotaz na Souhrn Tokenů

**Příklad příkazu**:
```
Dotaz na využití Tokenů tento měsíc
```

**Vrácené informace**:
- Celkový počet požadavků
- Počet úspěšných/neúspěšných
- Počet vstupních/výstupních/celkových Tokenů
- Statistiky podle AI klienta
- Statistiky podle silicon bytosti

### Dotaz na Trend Využití

**Příklad příkazu**:
```
Zobrazit trend využití Tokenů tento týden
```

**Vrácené informace**:
- Množství využití Tokenů podle dne nebo hodiny
- Pomáhá vám pochopit vzorce využití

### Parametry Nástroje

TokenAuditTool podporuje následující parametry:

- `time_range`: Časový rozsah (today/week/month/year)
- `client_type`: Typ AI klienta (volitelné)
- `being_id`: ID silicon bytosti (volitelné)

**Poznámka**: Toto je nástroj `[SiliconManagerOnly]`, který mohou používat pouze curátoři.

## Vysvětlení Dat

### Obsah Záznamů

Každé AI volání zaznamenává následující informace:

- **Časové razítko**: Čas AI volání (UTC)
- **ID silicon bytosti**: Bytost, která iniciovala volání
- **Typ AI klienta**: Použitá AI služba (např. OllamaClient)
- **Počet vstupních Tokenů**: Počet Tokenů textu odeslaného AI
- **Počet výstupních Tokenů**: Počet Tokenů textu odpovědi AI
- **Celkový počet Tokenů**: Vstup + výstup
- **ID relace**: Přináležející chatová relace
- **Zda úspěšné**: Zda bylo AI volání úspěšné

### Ukládání Dat

- Auditní záznamy používají **časově indexované ukládání** (ITimeStorage)
- Podporuje efektivní dotazy podle časového rozsahu
- Data jsou trvale uložena a neztratí se po restartu

### Vysvětlení Výpočtu Tokenů

**Co je Token?**
- Token je základní jednotka, se kterou modely AI zpracovávají text
- Jeden Token se rovná přibližně 0.75 anglického slova nebo 1-2 čínským znakům
- Výpočet Tokenů se mírně liší podle různých modelů AI

**Proč je třeba monitorovat Tokeny?**
- Cloudové AI služby (jako Alibaba Cloud Bailian) účtují podle Tokenů
- Pochopení využití Tokenů pomáhá kontrolovat náklady
- Objevování abnormálních vzorců využití (jako nekonečné cykly volání)
- Optimalizace konfigurace a souborů duše silicon bytostí

## Časté Otázky

### Otázka: Proč nevidím stránku auditů?

**Odpověď**: Funkce auditů vyžaduje oprávnění Curator (kurátor). Potvrďte prosím:
- Jste správce nebo kurátor platformy
- Silicon bytosti nemají oprávnění zobrazovat protokol auditů
- Pokud máte problém s oprávněním, kontaktujte správce platformy

### Otázka: Auditní data zobrazují ""Žádná data""?

**Odpověď**: Možné důvody:
- V zvoleném časovém rozsahu nejsou žádná AI volání
- Filtrační podmínky jsou příliš přísné, nejsou žádné odpovídající záznamy
- Silicon bytost ještě neměla dialog s AI

**Řešení**:
- Zkuste upravit časový rozsah (např. vybrat ""Tento měsíc"" místo ""Dnes"")
- Vymazat filtrační podmínky
- Nejprve provést několik dialogů se silicon bytostí

### Otázka: Proč využití Tokenů náhle vzrostlo?

**Odpověď**: Možné důvody:
- Přidáno velké množství záznamů dialogů (historie se prodlužuje, Prompt Token se zvyšuje)
- Soubor duše se stal velmi komplexním
- Některá silicon bytost často volá AI
- Úkoly nebo časovače AI se provádějí ve velkém množství

**Metoda řešení**:
- Použít filtr k zobrazení konkrétní silicon bytosti
- Zobrazit graf trendů a určit konkrétní časový bod
- Zkontrolovat soubor duše a konfiguraci úkolů této silicon bytosti
- Exportovat CSV pro detailní analýzu

### Otázka: Jakým softwarem lze otevřít exportovaný soubor CSV?

**Odpověď**: Soubor CSV lze otevřít následujícím softwarem:
- Microsoft Excel
- Google Sheets
- LibreOffice Calc
- Jakýkoli textový editor (jako Poznámkový blok, VS Code)

### Otázka: Jak dlouho se budou auditní data uchovávat?

**Odpověď**: 
- Auditní záznamy používají trvalé ukládání do souborového systému
- Ve výchozím nastavení se data budou uchovávat trvale
- Pokud potřebujete vyčistit stará data, můžete ručně smazat soubory auditních záznamů v adresáři úložiště
- Doporučuje se pravidelně exportovat důležitá data jako zálohu

### Otázka: Proč některá AI volání nejsou zaznamenána?

**Odpověď**: Možné důvody:
- AI klient nevrátil informace o využití Tokenů
- Služba záznamu Tokenů nebyla správně inicializována
- Volání selhalo a nebyla generována statistika Tokenů

**Poznámka**: Pouze AI odpovědi obsahující informace o Tokenech budou zaznamenány.

### Otázka: Mohu smazat auditní záznamy?

**Odpověď**: 
- Aktuální verze nepodporuje mazání auditních záznamů prostřednictvím rozhraní
- Auditní záznamy jsou uloženy v adresáři časového indexu souborového systému
- Pokud potřebujete vyčistit, můžete ručně smazat auditní soubory v adresáři úložiště
- **Doporučuje se nejprve exportovat zálohu před smazáním**

## Osvědčené Postupy

### 1. Pravidelně Monitorovat Využití Tokenů

- 📅 Zobrazit řídicí panel auditů jednou týdně nebo měsíčně
- 📊 Sledovat trend využití Tokenů, včas řešit abnormality
- 💰 Pokud používáte cloudové AI služby, sledovat vztah mezi náklady a využitím Tokenů

### 2. Použít Funkci Filtrů k Přesnému Umístění

- 🔍 Když objevíte problém, nejprve použijte filtr k zúžení rozsahu
- 🎯 Filtrovat podle silicon bytosti, určit konkrétní bytost
- 🤖 Filtrovat podle AI klienta, porovnat využití různých služeb

### 3. Pravidelně Exportovat Data

- 💾 Exportovat auditní data jednou měsíčně jako zálohu
- 📈 Vytvářet vlastní grafy a reporty v Excelu
- 📋 Dlouhodobě uchovávat data pro roční srovnávací analýzu

### 4. Optimalizovat Využití Tokenů

Optimalizovat podle auditních dat:
- 📝 Zjednodušit soubory duše (snížit zbytečná nastavení)
- 🗂️ Kontrolovat množství paměti (vyhnout se příliš dlouhé historii)
- ⏱️ Upravit frekvenci časovačů (snížit zbytečná AI volání)
- 🎯 Optimalizovat popis úkolů (zvýšit efektivitu, snížit opakovaná volání)

### 5. Nastavit Varování Využití

- 📊 Sledovat abnormální fluktuace využití Tokenů
- 🔔 Pokud zjistíte, že některá silicon bytost najednou spotřebovává velké množství Tokenů, včas zkontrolujte
- 🛑 Pokud je to nutné, pozastavit tuto bytost a řešit problém

## Související Funkce

- [Správa Silicon Bytostí](/help/being-management) - Spravovat vytváření a konfiguraci silicon bytostí
- [Systém Úkolů](/help/task) - Konfigurovat a spravovat časované úkoly silicon bytostí
- [Systém Paměti](/help/memory) - Pochopit, jak paměť ovlivňuje využití Tokenů
- [Správa Oprávnění](/help/permission) - Pochopit oprávnění curatora a řízení přístupu
- [Správa Konfigurace](/help/config) - Konfigurovat AI klienty a další nastavení systému
";
    
    public override string KnowledgeGraph => @"
# Graf Znalostí

## Přehled

Graf znalostí je **systém správy a vizualizace znalostí** pro křemíkové bytosti, zobrazující znalosti naučené a nahromaděné křemíkovými bytostmi grafickým způsobem. Prostřednictvím intuitivních uzlů a připojení můžete jasně vidět vztahy mezi znalostmi.

## Co je Trojice Znalostí?

Základní jednotkou grafu znalostí je **trojice znalostí**, používající strukturu ""subjekt-predikát-objekt"":

```
(subjekt) -[vztah]-> (objekt)
```

**Příklady:**
- `(Xiao Ming) -[má rád]-> (jablka)`
- `(Python) -[je]-> (programovací jazyk)`
- `(Země) -[obíhá]-> (Slunce)`

Tato struktura umožňuje křemíkovým bytostem chápat a organizovat znalosti jako lidé.

## Přístup ke Grafu Znalostí

### Vstupní Bod

1. Klikněte na ikonu **📚 Databáze Znalostí** v levém navigačním panelu
2. Systém automaticky načte a zobrazí stránku vizualizace grafu znalostí

### Rozložení Rozhraní

Stránka grafu znalostí obsahuje:

- **Horní Titulek**: Zobrazuje ""Vizualizace Grafu Znalostí""
- **Panel Statistik**: Zobrazuje počet uzlů, počet hran a další statistiky
- **Hlavní Oblast Plátna**: Zobrazuje vizualizaci grafu znalostí
- **Indikátor Načítání**: Zobrazuje zprávu o načítání během načítání dat

## Vizualizace Grafu

### Uzly

Uzly představují **entity** (subjekty nebo objekty) ve znalostech:

- **Kruhové Uzly**: Každý uzel představuje entitu
- **Štítky Uzlu**: Zobrazují název entity
- **Velikost Uzlu**: Automaticky upravena podle počtu připojení (více připojení = větší uzel)
- **Barvy Uzlu**:
  - Modrý okraj: Běžné uzly
  - Zvýrazněné: Při najetí myší

### Hrany

Hrany představují **vztahy** (predikáty) ve znalostech:

- **Čáry se Šipkami**: Indikují vztahy od subjektu k objektu
- **Barva Čáry**: Poloprůhledná modrá `rgba(100, 150, 255, 0.6)`
- **Značky Šipek**: Jasně zobrazují směr vztahu

### Algoritmus Rozložení

Graf znalostí používá **kruhový algoritmus rozložení**:

- Všechny uzly jsou rovnoměrně rozloženy na kruhových drahách
- Přiměřené rozestupy mezi uzly
- Automaticky se přizpůsobuje velikosti plátna

## Interaktivní Funkce

### Operace Myši

- **Najetí na Uzel**: Uzel se zvýrazní pro snadnou identifikaci
- **Zobrazit Vztahy**: Pochopit směr vztahů prostřednictvím šipek
- **Přiblížit Plátno**: Nativní funkce přiblížení prohlížeče (Ctrl + kolečko myši)

### Načítání Dat

- **Automatické Načítání**: Automaticky načítá data znalostí ze systému při vstupu na stránku
- **Aktualizace v Reálném Čase**: Znovu navštívit stránku pro nejnovější znalosti
- **Indikátor Načítání**: Zobrazuje ""Načítání dat grafu znalostí..."" během načítání

## Správa Znalostí

### Jak se Generují Znalosti?

Křemíkové bytosti akumulují znalosti prostřednictvím:

1. **Učení Konverzací**: Extrahování klíčových informací z konverzací s uživateli
2. **Volání Nástrojů**: Přidávání znalostí prostřednictvím KnowledgeTool
3. **Analýzy Souborů**: Analýza obsahu nahraných souborů pro extrakci znalostí
4. **Provádění Úkolů**: Akumulace doménových znalostí během provádění úkolů

### Operace KnowledgeTool

Křemíkoví kurátoři nebo křemíkové bytosti mohou požádat AI o provedení operací znalostí prostřednictvím konverzace:

**Přidat Znalost:**
```
Prosím přidej znalost: (Python) -[je]-> (programovací jazyk)
```

**Dotaz na Znalost:**
```
Najít všechny znalosti o ""Python""
```

**Vyhledat Znalost:**
```
Vyhledat znalosti obsahující ""programování""
```

**Najít Cestu Vztahu:**
```
Najít cestu vztahu mezi ""Python"" a ""AI""
```

### Atributy Znalostí

Každý záznam znalostí obsahuje bohatá metadata:

- **Jedinečné ID**: Jedinečný identifikátor pro každou znalost
- **Číslo Verze**: Podporuje aktualizace znalostí a správu verzí
- **Důvěryhodnost**: 0.0-1.0, indikující spolehlivost znalosti
- **Zdroj**: Zdroj znalosti (konverzace, soubor, nástroj atd.)
- **Kategorie**: Doména nebo kategorie, do které znalost patří
- **Štítky**: Vlastní štítky pro snadnou klasifikaci a vyhledávání
- **Tvůrce**: Křemíková bytost, která přidala tuto znalost
- **Čas Vytvoření**: Časové razítko, kdy byla znalost přidána
- **Stav Ověření**: Neověřeno/Ověřeno/Odmítnuto

## Statistiky

Stránka grafu znalostí zobrazuje následující statistiky:

- **Celkem Uzlu**: Počet všech entit v grafu znalostí
- **Celkem Hran**: Počet vztahů mezi entitami
- **Záznamy Znalostí**: Celkový počet trojic znalostí v systému

Tyto statistiky vám pomáhají pochopit úroveň akumulace znalostí křemíkových bytostí.

## Případy Použití

### 1. Vizualizace Znalostí

Intuitivně zobrazit, co se křemíkové bytosti naučily:

1. Vstoupit na stránku grafu znalostí
2. Pozorovat rozložení uzlů a připojení
3. Pochopit strukturu organizace znalostí a asociace

### 2. Ověřování Znalostí

Zkontrolovat výsledky učení křemíkových bytostí:

1. Zobrazit uzly znalostí v konkrétních doménách
2. Ověřit přesnost znalostí
3. Doplnit nebo opravit znalosti prostřednictvím konverzace

### 3. Průzkum Znalostí

Objevit asociace mezi znalostmi:

1. Pozorovat vysoce připojené uzly (core koncepty)
2. Sledovat cesty vztahů k objevení nepřímých asociací
3. Identifikovat mezery ve znalostech k vedení dalšího učení

## Často Kladené Otázky

### O: Proč je graf znalostí prázdný?

**A:** Pokud se křemíkové bytosti ještě nenaučily žádné znalosti, graf se zobrazí prázdný. Můžete přidat znalosti prostřednictvím:
- Konverzace s křemíkovými bytostmi, aby se naučily nové informace
- Nahrání souborů pro analýzu křemíkovými bytostmi
- Přímé požádání křemíkových bytostí o přidání konkrétních znalostí

### O: Aktualizuje se graf znalostí automaticky?

**A:** Data grafu znalostí jsou ukládána v reálném čase, ale stránka musí být aktualizována pro zobrazení nejnovějších dat. Poté, co se křemíkové bytosti naučí nové znalosti, znovu navštivte stránku grafu znalostí pro zobrazení aktualizací.

### O: Jak zobrazit, když je příliš mnoho uzlů?

**A:** Když je objem znalostí velký:
- Použít přiblížení prohlížeče (Ctrl + kolečko myši)
- Zaměřit se na vysoce připojené core uzly
- Vyhledat znalosti o konkrétních tématech prostřednictvím dotazovacích nástrojů

### O: Jak smazat nesprávné znalosti?

**A:** Můžete požádat křemíkové bytosti o smazání nebo opravu znalostí prostřednictvím konverzace:
```
Prosím smaž nesprávné znalosti o ""...""
```
nebo
```
Prosím aktualizuj znalost: (stará znalost) na (nová znalost)
```

### O: Kde jsou uložena data grafu znalostí?

**A:** Data znalostí jsou uložena v datovém adresáři systému ve formátu JSON. Systém automaticky spravuje ukládání, indexování a dotazování znalostí.

### O: Mohu exportovat graf znalostí?

**A:** V současnosti se graf znalostí používá hlavně pro online vizualizaci. Pro export dat znalostí můžete požádat křemíkové bytosti o dotazování prostřednictvím nástrojů a generování zpráv.

## Doporučené Postupy

### 1. Pravidelně Kontrolovat Akumulaci Znalostí

- Týdně kontrolovat graf znalostí pro pochopení pokroku v učení křemíkových bytostí
- Zaměřit se na nově přidané uzly a vztahy
- Ověřit přesnost důležitých znalostí

### 2. Vést Učení Znalostí

- Proaktivně poskytovat vysoce kvalitní vstupy znalostí
- Opravovat nesprávné znalosti pro udržení přesnosti databáze znalostí
- Přidávat kategorie a štítky ke znalostem pro snazší správu

### 3. Používat Graf Znalostí k Asistenci Konverzacím

- Odkazovat na existující znalosti v konverzacích
- Požádat křemíkové bytosti o usuzování založené na existujících znalostech
- Prozkoumávat asociace mezi znalostmi k objevení nových poznatků

### 4. Kontrola Kvality Znalostí

- Věnovat pozornost skóre důvěryhodnosti znalostí
- Ověřit přesnost kritických znalostí
- Včas aktualizovat zastaralé nebo nesprávné znalosti

## Související Dokumentace

- [Správa Bytostí](/help/being-management) - Spravovat vytváření a konfiguraci křemíkových bytostí
- [Chatovací Systém](/help/chat-system) - Konverzovat s křemíkovými bytostmi k učení znalostí
- [Systém Úkolů](/help/task) - Konfigurovat úkoly učení znalostí
- [Systém Paměti](/help/memory) - Pochopit, jak znalosti ovlivňují paměť konverzace
";
    
    public override string WorkNotes => @"
# Pracovní Poznámky

## Přehled

Pracovní Poznámky jsou**osobní systém zaznamenávání znalostí** Křemíkových Bytostí, podobný digitálnímu deníku nebo pracovnímu protokolu. Každá Křemíková Bytost může vytvářet, spravovat a vyhledávat své vlastní pracovní poznámky, zaznamenávat zkušenosti z učení, pracovní poznatky, myšlenky a úvahy.

Pracovní Poznámky podporují dva režimy:
- **Osobní Pracovní Poznámky**: Vlastnictví Křemíkové Bytosti, ve výchozím nastavení soukromé
- **Projektové Pracovní Poznámky**: Vlastnictví projektového prostoru, členové projektu mohou spolupracovat

## Hlavní Funkce

### Hlavní Vlastnosti

- **Záznam na Stránky**: Každá poznámka je samostatná stránka, uspořádaná podle čísla stránky
- **Podpora Markdown**: Obsah podporuje formát Markdown, umožňuje úpravy bohatého textu
- **Štítky Klíčových Slov**: Lze přidat klíčová slova k poznámkám, usnadňuje vyhledávání a klasifikaci
- **Řízení Verzí**: Každá úprava automaticky aktualizuje číslo verze, zaznamenává historii změn
- **Sledování Autora**: Zaznamenává informace o tvůrci a posledním editorovi
- **Zobrazení Časové Osy**: Zobrazit všechny poznámky v chronologickém pořadí
- **Fulltextové Vyhledávání**: Vyhledávat poznámky podle klíčových slov, shrnutí nebo obsahu

### Dva Typy Pracovních Poznámek

| Typ | Vlastnictví | Výchozí Viditelnost | Použití |
|------|-----------|---------------------|---------|
| Osobní Pracovní Poznámky | Křemíková Bytost | Soukromé | Osobní deník, poznámky z učení, záznam myšlenek |
| Projektové Pracovní Poznámky | Projektový prostor | Veřejné | Pokrok projektu, záznam spolupráce, zápisy z porad |

## Přístup k Pracovním Poznámám

### Vstupní Body

**Osobní Pracovní Poznámky**:
1. Klikněte na ikonu **📝 Pracovní Poznámky** v levém navigačním panelu
2. Nebo najděte vstupní bod pracovních poznámek na stránce detailů Křemíkové Bytosti

**Projektové Pracovní Poznámky**:
1. Vstupte na stránku detailů projektu
2. Klikněte na záložku **Pracovní Poznámky**

### Popis Rozhraní

Stránka pracovních poznámek obsahuje dvě hlavní oblasti:

**Oblast Mřížky Poznámek**:
- Zobrazuje všechny poznámky ve formě karet
- Každá karta zobrazuje číslo stránky, shrnutí, čas aktualizace
- Efekt animace vznosu při najetí myší
- Klikněte na kartu pro otevření detailů poznámky

**Oblast Detailů Poznámky**:
- Zobrazuje celý obsah poznámky na celou obrazovku
- Zobrazuje metadata (shrnutí, klíčová slova, čas aktualizace)
- Obsah je vykreslen ve formátu Markdown
- Klikněte na tlačítko zavřít pro návrat do seznamu

## Vytvořit Poznámky

### Vytvořit pomocí AI Nástrojů

Křemíkové Bytosti mohou vytvářet poznámky prostřednictvím konverzace s AI:

**Vytvořit Osobní Poznámku**:
```
Prosím, vytvořte pro mě pracovní poznámku:
- Shrnutí: Naučil jsem se koncept grafu znalostí
- Obsah: Graf znalostí je způsob reprezentace znalostí ve struktuře grafu...
- Klíčová slova: graf znalostí, AI, učení
```

**Vytvořit Projektovou Poznámku**:
```
Prosím, vytvořte pracovní poznámku v projektu XXX:
- Shrnutí: Aktualizace pokroku projektu - 1. týden
- Obsah: Tento týden byla dokončena fáze analýzy požadavků...
- Klíčová slova: pokrok, týdenní zpráva
```

### Povinná Pole Poznámky

- **Shrnutí**: Stručný popis obsahu poznámky (povinné)
- **Obsah**: Podrobný obsah poznámky, podporuje Markdown (povinné)
- **Klíčová Slova**: Klíčová slova oddělená čárkami (volitelné)
### Informace Zaznamenané Automaticky

- **Čas Vytvoření**: Automaticky zaznamenává čas UTC
- **Tvůrce**: Automaticky zaznamenává GUID tvůrce
- **Číslo Stránky**: Systém automaticky přiděluje jedinečné číslo stránky
- **Číslo Verze**: Počáteční verze je 1

## Zobrazit a Spravovat Poznámky

### Procházet Seznam Poznámek

1. Otevřete stránku pracovních poznámek
2. Zobrazte mřížku karet poznámek
3. Horní část stránky zobrazuje celkový počet stránek
4. Karty jsou uspořádány podle čísla stránky

### Zobrazit Detaily Poznámky

1. Klikněte na libovolnou kartu poznámky
2. Detaily poznámky se rozbalí na celou obrazovku
3. Zobrazit následující informace: titul čísla stránky, shrnutí, klíčová slova, čas aktualizace, celý obsah (vykreslení Markdown)
4. Klikněte na **×** v pravém horním rohu pro zavření detailů

### Informace na Kartách Poznámek

Každá karta zobrazuje:
- **Page X**: Identifikátor čísla stránky
- **Obsah Shrnutí**: Stručný popis poznámky
- **Čas Aktualizace**: Čas poslední změny

## Vyhledávat Poznámky

### Metody Vyhledávání

**Vyhledávat pomocí AI Nástrojů**: Vyhledat pracovní poznámky obsahující klíčová slova
**Rozsah Vyhledávání**: Shrnutí poznámky, obsah poznámky, pole klíčových slov

### Generovat Rejstřík Poznámek

Můžete požádat AI o vygenerování rejstříku všech poznámek:
```
Prosím, generujte rejstřík mých pracovních poznámek
```

## Aktualizovat Poznámky

### Upravit Obsah Poznámky

Aktualizujte poznámky pomocí AI nástrojů, můžete aktualizovat pouze shrnutí, obsah nebo klíčová slova samostatně

### Vlastnosti Aktualizace

- **Selektivní Aktualizace**: Lze aktualizovat pouze některá pole
- **Inkrementace Verze**: Každá úprava automaticky zvyšuje číslo verze +1
- **Sledování Změn**: Zaznamenává GUID posledního editora
- **Aktualizace Času**: Čas aktualizace se automaticky obnovuje

## Odstranit Poznámky

### Operace Odstranění

Odstraňte poznámky pomocí AI nástrojů, je vyžadováno číslo stránky nebo ID poznámky

### Potvrzení Odstranění

- Po odstranění nelze obnovit, postupujte opatrně
- Operace odstranění je zaznamenána v protokolu auditů

## Správa Oprávnění

### Oprávnění Osobních Pracovních Poznámek

- **Tvůrce**: Úplná kontrola (vytvořit, zobrazit, upravit, odstranit)
- **Kurator Křemíku**: Může spravovat poznámky všech Křemíkových Bytostí
- **Ostatní Uživatelé**: Žádné oprávnění k přístupu

### Oprávnění Projektových Pracovních Poznámek

- **Členové Projektu**: Mohou vytvářet, zobrazovat, upravovat projektové poznámky
- **Nečlenové Projektu**: Žádné oprávnění k přístupu
- **Kurator Křemíku**: Může spravovat všechny projektové poznámky

## Scénáře Použití

### Záznam Osobního Učení

Zaznamenat proces učení nových znalostí, přidat shrnutí, obsah, klíčová slova

### Sledování Pokroku Projektu

Zaznamenat týdenní pokrok projektu, sledovat dokončení úkolů

### Shrnutí Pracovních Zkušeností

Shrnout pracovní zkušenosti a ponaučení, uložit pro budoucí referenci

### Záznam Zápisů z Porad

Zaznamenat obsah projektových porad, uložit rozhodnutí a výsledky diskusí

## Osvědčené Postupy

### 1. Standardizovaně Psát Shrnutí

- Omezit shrnutí na méně než 20 znaků, jasně vyjádřit jádrový obsah poznámky

### 2. Rozumně Používat Klíčová Slova

- Použít 3-5 klíčových slov, oddělených čárkami

### 3. Strukturovat Obsah

- Použít hierarchii nadpisů Markdown, použít seznamy pro důležité informace

### 4. Pravidelně Organizovat Poznámky

- Pravidelně generovat přehled rejstříku, odstranit expirované nebo zbytečné poznámky

### 5. Spolupráce na Projektových Poznámách

- Jasně zaznamenat informace o autorovi, při úpravách vysvětlit důvod změn

## Často Kladené Otázky

### O1: Jaký je rozdíl mezi pracovními poznámkami a systémem paměti?

**O**: Pracovní poznámky jsou aktivně zaznamenávaný strukturovaný obsah, podobný deníku; systém paměti ukládá fragmenty konverzací a fakta uložená automaticky nebo ručně.

### O2: Mohu chránit poznámky heslem?

**O**: Aktuální verze nepodporuje ochranu heslem, ale přístup je řízen systémem oprávnění.

### O3: Je zde limit pro počet poznámek?

**O**: Neexistuje žádný tvrdý limit, ale doporučuje se pravidelně organizovat pro udržení kvality poznámek.

### O4: Mohu exportovat poznámky?

**O**: Aktuální verze nepodporuje přímý export, ale data poznámek lze získat prostřednictvím API.

### O5: Mohu zobrazit historii verzí poznámek?

**O**: Aktuální verze zachovává pouze nejnovější verzi, neukládá historické verze.

### O6: Lze převádět projektové poznámky a osobní poznámky mezi sebou?

**O**: Přímý převod není podporován, můžete vytvořit novou poznámku a zkopírovat obsah.

## Související Dokumentace

- [Správa Křemíkových Bytostí](/help/being-management) - Pochopit, jak spravovat Křemíkové Bytosti
- [Správa Projektů](/help/projects) - Pochopit projektový prostor a spolupráci
- [Správa Úkolů](/help/task) - Pochopit přidělování a sledování úkolů
- [Graf Znalostí](/help/knowledge-graph) - Pochopit vizualizaci znalostí
- [Správa Oprávnění](/help/permission) - Pochopit mechanismus řízení oprávnění
";
    
    public override string Projects => @"
# Správa Projektů

## Přehled

Správa Projektů je**spolupracovní pracovní prostor** systému Křemíkových Bytostí, který poskytuje prostředí pro společnou práci více Křemíkových Bytostí. Prostřednictvím projektového prostoru může Kurator Křemíku organizovat a spravovat týmy Křemíkových Bytostí, přidělovat úkoly, sledovat pokrok a dosahovat komplexních cílů spolupráce.

Projekty jsou centrální organizační jednotkou systému Křemíkových Bytostí, každý projekt má nezávislý systém úkolů, pracovní poznámky a seznam členů.

## Hlavní Funkce

### Hlavní Vlastnosti

- **Řízení Životního Cyklu Projektu**: Vytvořit, archivovat, obnovit, odstranit projekty
- **Správa Členů**: Přidělit a odstranit členy projektu
- **Spolupráce na Úkolech**: Systém správy úkolů výhradně pro projekt
- **Pracovní Poznámky**: Sdílené pracovní poznámky na úrovni projektu
- **Informace o Projektu**: Správa metadat jako název, popis, stav
- **Mechanismus Archivace**: Podporuje archivaci a obnovu projektů, chrání historická data

### Stav Projektu

| Stav | Popis | Proveditelné Akce |
|------|-------|------------------|
| Aktivní | Projekt v normálním provozu | Všechny akce |
| Archivován | Pozastavený projekt, data zachována | Obnovit, zobrazit |
| Odstraněn | Trvale odstraněný projekt | Žádné |

## Přístup ke Správě Projektů

### Vstupní Body

**Funkce Výhradně pro Kuratora Křemíku**:
- Nástroje správy projektů jsou dostupné pouze pro Kuratora Křemíku
- Požádat AI o provedení operací správy projektů prostřednictvím konverzace
- Zobrazit přidružení projektů v rozhraní správy Křemíkových Bytostí

### Stránka Projektu

Každý projekt má vyhrazenou stránku:
1. Navštivte `/project/{ID Projektu}` pro zobrazení detailů projektu
2. Zobrazit základní informace o projektu (název, popis, stav)
3. Přistup k pracovním poznámkám projektu
4. Přistup k seznamu úkolů projektu
5. Zobrazit seznam členů projektu

## Vytvořit Projekt

### Operace Vytvoření

Vytvořit projekt pomocí AI nástrojů:
```
Prosím, vytvořte nový projekt:
- Název: Projekt Vývoje AI Asistenta
- Popis: Vyvinout jádrové funkce systému inteligentního asistenta
```

### Informace o Projektu

Požadováno při vytvoření projektu:
- **Název Projektu**: Jasně vyjádřit cíl projektu (povinné)
- **Popis Projektu**: Podrobně popsat obsah projektu (volitelné, ale doporučené)

### Automatické Nastavení Po Vytvoření

- **ID Projektu**: Systém automaticky generuje jedinečné GUID
- **Čas Vytvoření**: Zaznamenává čas vytvoření UTC
- **Počáteční Stav**: Aktivní stav
- **Seznam Členů**: Zpočátku prázdný, potřebuje následné přidělení

## Spravovat Členy Projektu

### Přidělit Členy do Projektu

Přidat Křemíkovou Bytost do projektu:
```
Prosím, přidělte Křemíkovou Bytost XXX do projektu YYY
```

Požadováno:
- **ID Projektu**: GUID cílového projektu
- **ID Křemíkové Bytosti**: GUID Křemíkové Bytosti k přidělení

### Odstranit Členy z Projektu

Odstranit Křemíkovou Bytost z projektu:
```
Prosím, odstraňte Křemíkovou Bytost XXX z projektu YYY
```

### Zobrazit Členy Projektu

Vypsat všechny členy projektu:
```
Prosím, vypište všechny členy projektu XXX
```

### Oprávnění Členů

- **Členové Projektu**: Mohou vytvářet projektové úkoly, psát projektové poznámky
- **Nečlenové Projektu**: Žádný přístup k projektovým zdrojům
- **Kurator Křemíku**: Může spravovat členy všech projektů

## Aktualizovat Informace o Projektu

### Změnit Název Projektu

```
Prosím, změňte název projektu XXX na nový název projektu
```

### Změnit Popis Projektu

```
Prosím, změňte popis projektu XXX na nový popis projektu
```

### Aktualizovat Název a Popis Současně

```
Prosím, aktualizujte projekt XXX:
- Název: Nový název
- Popis: Nový popis
```

## Řízení Životního Cyklu Projektu

### Archivovat Projekt

Archivovat dočasně neaktivní projekty:
```
Prosím, archivujte projekt XXX
```

**Efekt Archivace**:
- Stav projektu se změní na ""Archivován""
- Data projektu jsou plně zachována
- Členové zůstávají přidruženi
- Lze obnovit ze seznamu archivů

**Scénáře Archivace**:
- Projekt dočasně pozastaven
- Fázový projekt dokončen
- Potřeba organizovat seznam projektů

### Obnovit Projekt

Obnovit projekt z archivovaného stavu:
```
Prosím, obnovte projekt XXX
```

**Efekt Obnovení**:
- Stav projektu se vrátí na ""Aktivní""
- Všechna data jsou plně obnovena
- Vztahy členů zůstávají nezměněny
- Všechny funkce lze normálně používat

### Odstranit Projekt

Trvale odstranit projekt (neobnovitelný):
```
Prosím, odstraňte projekt XXX
```

**Efekt Odstranění**:
- Data projektu jsou trvale odstraněna
- Související úkoly a pracovní poznámky jsou vyčištěny
- Vztahy členů jsou rozpuštěny
- **Akci nelze vrátit zpět**

**Potvrdit Před Odstraněním**:
- Potvrdit, že projekt již není potřeba
- Zálohovat důležitá data (pokud je to nutné)
- Upozornit členy projektu

### Zobrazit Seznam a Detaily Projektu

**Vypsat Všechny Aktivní Projekty**:
```
Prosím, vypište všechny projekty
```

**Včetně Archivovaných Projektů**:
```
Prosím, vypište všechny projekty, včetně archivovaných
```

### Zobrazit Detaily Projektu

Získat kompletní informace o projektu:
```
Prosím, získejte detaily projektu XXX
```

Vrácené informace:
- ID projektu
- Název projektu
- Popis projektu
- Stav projektu
- Čas vytvoření
- Seznam členů
- Statistiky úkolů

## Funkce Spolupráce na Projektu

### Projektové Úkoly

Každý projekt má nezávislý systém úkolů:

**Vytvořit Projektový Úkol**:
```
Prosím, vytvořte úkol v projektu XXX:
- Titul: Implementovat funkci přihlášení uživatele
- Popis: Dokončit modul autentizace a autorizace uživatelů
- Priorita: 1
```

**Priorita Úkolů**:
- Menší číslo = vyšší priorita
- Priorita 1: Nejvyšší priorita
- Priorita 5: Normální priorita

**Tok Stavů Úkolů**:
1. **Čekající** (Pending): Úkol vytvořen, čeká na spuštění
2. **Běží** (Running): Úkol spuštěn
3. **Dokončen** (Completed): Úkol úspěšně dokončen
4. **Selhal** (Failed): Provádění úkolu selhalo
5. **Zrušen** (Cancelled): Úkol zrušen

### Projektové Pracovní Poznámky

Každý projekt má sdílený systém pracovních poznámek:

**Vytvořit Projektovou Poznámku**:
```
Prosím, vytvořte pracovní poznámku v projektu XXX:
- Shrnutí: Aktualizace pokroku projektu - 1. týden
- Obsah: Tento týden byla dokončena analýza požadavků...
- Klíčová slova: pokrok, týdenní zpráva
```

**Vlastnosti Projektových Poznámek**:
- Všichni členové projektu mohou vytvářet a upravovat
- Organizováno podle čísla stránky, vhodné pro sledování
- Zaznamenává informace o autorovi, sledování příspěvků
- Podporuje formát Markdown

### Porovnání Projektových a Osobních Zdrojů

| Typ Zdroje | Osobní | Projekt |
|------------|--------|---------|
| Pracovní Poznámky | Soukromý deník | Sdílené spolupracovní poznámky |
| Úkoly | Osobní úkoly | Projektové spolupracovní úkoly |
| Graf Znalostí | Osobní znalosti | Projektové znalosti (budoucnost) |
| Paměť | Osobní paměť | Projektová paměť (budoucnost) |

## Scénáře Použití

### Projekt Vývoje Softwaru

**Scénář**: Organizovat tým pro vývoj softwaru

**Příklad**:
```
1. Vytvořit projekt: Vývoj webové aplikace
2. Přidělit členy: Vývoj frontend, vývoj backend, testování
3. Vytvořit úkoly:
   - Implementovat modul autentizace uživatelů (priorita 1)
   - Navrhnout architekturu databáze (priorita 1)
   - Vytvořit dokumentaci API (priorita 2)
4. Týdně aktualizovat projektové poznámky pro záznam pokroku
5. Sledovat dokončení úkolů
```

### Výzkumný Projekt

**Scénář**: Spolupracovat na výzkumu AI

**Příklad**:
```
1. Vytvořit projekt: Výzkum optimalizace grafu znalostí
2. Přidělit členy: Výzkumník A, Výzkumník B
3. Vytvořit úkoly:
   - Literární rešerše (priorita 1)
   - Experiment vylepšení algoritmu (priorita 2)
   - Vytvořit výzkumnou zprávu (priorita 3)
4. Zaznamenat experimentální poznámky v projektu
5. Archivovat dokončené výzkumné fáze
```

### Vzdělávací Projekt

**Scénář**: Organizovat studijní skupinu

**Příklad**:
```
1. Vytvořit projekt: Studijní skupina Python
2. Přidělit členy: Student A, Student B, Mentor
3. Vytvořit úkoly:
   - 1. týden: Naučit se základní syntaxi (priorita 1)
   - 2. týden: Objektově orientované programování (priorita 2)
   - 3. týden: Projektová praxe (priorita 3)
4. Zaznamenat poznámky z učení a reflexe
5. Označit dokončené milníky učení
```

## Osvědčené Postupy

### 1. Konvence Pojmenování Projektů

- Používat jasné názvy projektů
- Zahrnout typ nebo cíl projektu
- Vyhnout se nejednoznačným zkratkám

**Dobré Příklady**:
- ""Vývoj Backend Webové Aplikace""
- ""Výzkum Optimalizace AI Modelu""
- ""Organizace Technických Dokumentů 2024""

**Špatné Příklady**:
- ""Projekt 1""
- ""Test""
- ""Nový Projekt""

### 2. Podrobně Popsat Projekt

- Podrobně popsat cíl a rozsah projektu
- Zaznamenat důležité milníky
- Popsat očekávané výsledky

### 3. Rozumně Přidělit Členy

- Přidělit členy na základě dovedností a rolí
- Vyhnout se tomu, aby se jeden člen účastnil příliš mnoha projektů
- Pravidelně kontrolovat účast členů

### 4. Správa Priority Úkolů

- Nastavit vysokou prioritu pro důležité úkoly (1-2)
- Nastavit střední prioritu pro normální úkoly (3-4)
- Nastavit nízkou prioritu pro volitelné úkoly (5+)

### 5. Pravidelně Aktualizovat Projektové Poznámky

- Aktualizovat pokrok alespoň jednou týdně
- Zaznamenat důležitá rozhodnutí a změny
- Zachovat kontinuitu a integritu poznámek

### 6. Včas Archivovat Projekty

- Včas archivovat dokončené projekty
- Zvážit archivaci dlouhodobě neaktivních projektů
- Potvrdit, že data jsou uložena před archivací

### 7. Opatrně Používat Funkci Odstranění

- Potvrdit dvakrát před odstraněním
- Zvážit nejprve archivaci a pozorování po určitou dobu
- Potvrdit, že historická data nejsou potřeba

## Správa Oprávnění

### Oprávnění Správy Projektů

- **Kurator Křemíku**:
  - Vytvořit, archivovat, obnovit, odstranit projekty
  - Přidělit a odstranit členy projektu
  - Aktualizovat informace o projektu
  - Zobrazit všechny projekty

- **Členové Projektu**:
  - Zobrazit informace o projektu
  - Vytvořit a upravit projektové úkoly
  - Vytvořit a upravit projektové poznámky
  - Nemůže spravovat samotný projekt

- **Nečlenové Projektu**:
  - Žádný přístup k projektovým zdrojům
  - Nemůže zobrazit informace o projektu

### Oprávnění Nástrojů

- Nástroj `project`: Označen `[SiliconManagerOnly]`, pouze Kurator Křemíku
- Nástroj `project_task`: Dostupný pro členy projektu a Kuratora Křemíku
- Nástroj `project_work_note`: Dostupný pro členy projektu a Kuratora Křemíku

## Často Kladené Otázky

### O1: Je zde horní limit pro počet projektů?

**O**: Aktuální verze nemá žádný tvrdý limit. Doporučuje se však udržovat přiměřený počet projektů pro usnadnění správy. Příliš mnoho projektů může způsobit zmatek ve správě.

### O2: Zabírají archivované projekty úložný prostor?

**O**: Ano, všechna data archivovaných projektů (úkoly, poznámky atd.) jsou zachována. Pokud je úložný prostor omezený, můžete zvážit odstranění projektů, které již nejsou potřeba.

### O3: Mohu patřit do více projektů současně?

**O**: Ano. Křemíková Bytost může být přidělena do více projektů současně a přepínat mezi různými projekty.

### O4: Mohou členové projektu opustit projekt sami?

**O**: Aktuální verze nepodporuje samostatné opuštění členů. Kurator Křemíku musí provést operaci odstranění.

### O5: Lze přidávat úkoly do archivovaných projektů?

**O**: Ne. Archivované projekty jsou ve stavu pouze pro čtení a musí být obnoveny do aktivního stavu pro pokračující přidávání nebo úpravy.

### O6: Při odstranění projektu se odstraní i úkoly a poznámky?

**O**: Ano. Odstranění projektu vyčistí všechna související data (úkoly, pracovní poznámky atd.). Tato akce není obnovitelná.

### O7: Mohu kopírovat projekt?

**O**: Aktuální verze nepodporuje přímé kopírování projektů. Musíte ručně vytvořit nový projekt a znovu nakonfigurovat členy a úkoly.

### O8: Mají projekty termín dokončení?

**O**: Aktuální verze nevynucuje nastavení termínů. Můžete však zaznamenat plánovaný harmonogram v popisu projektu nebo spravovat čas prostřednictvím úkolů.

## Související Dokumentace

- [Správa Úkolů](/help/task) - Pochopit správu osobních úkolů
- [Pracovní Poznámky](/help/work-notes) - Pochopit systém pracovních poznámek
- [Správa Křemíkových Bytostí](/help/being-management) - Pochopit, jak spravovat Křemíkové Bytosti
- [Správa Oprávnění](/help/permission) - Pochopit mechanismus řízení oprávnění
- [Protokol Auditů](/help/audit-log) - Zobrazit protokol akcí projektu
";
    
    public override string Logging => @"
# Systém Protokolování

## Přehled

Systém protokolování je klíčovou infrastrukturou platformy Silicon Life Collective, používanou k zaznamenávání stavu provozu systému, chování křemíkových bytostí, chybových zpráv a ladicích dat. Systém současně vypisuje na konzoli a do souborového systému a poskytuje flexibilní filtrování podle úrovně protokolu.

## Hlavní Funkce

- **Víceúrovňové Protokolování**: Podporuje 6 úrovní protokolu (Trace, Debug, Information, Warning, Error, Critical)
- **Více Výstupních Cílů**: Současně podporuje barevný výstup na konzoli a perzistentní úložiště v souborovém systému
- **Chytré Filtrování**: Podporuje filtrování podle úrovně protokolu, časového rozsahu a křemíkové bytosti
- **Kategorizovaná Správa**: Organizuje protokoly podle kategorie pro snazší lokalizaci problémů
- **Zaznamenávání Výjimek**: Automaticky zaznamenává stopy zásobníku výjimek pro pomoc při řešení problémů

## Popis Úrovní Protokolu

Systém definuje následující úrovně protokolu (v pořadí rostoucí závažnosti):

| Úroveň | Popis | Použití |
|--------|-------|---------|
| **Trace** | Nejpodrobnější protokoly | Ladění vývoje, obsahuje citlivá data |
| **Debug** | Ladicí informace | Interaktivní vyšetřování, ladicí účely |
| **Information** | Obecné informace | Sledování běžného toku aplikace, dlouhodobá hodnota |
| **Warning** | Varovné zprávy | Abnormální nebo neočekávané události, ale nezastavuje provádění |
| **Error** | Chybové zprávy | Aktuální tok provádění zastaven kvůli selhání |
| **Critical** | Kritické chyby | Pád systému nebo katastrofální selhání, vyžaduje okamžité řešení |
| **None** | Žádné protokolování | Používá se k zakázání protokolování pro konkrétní kategorii |

## Průvodce Používáním

### Kde zobrazit protokoly?

Systém poskytuje dva způsoby zobrazení protokolů:

1. **Konzolové Protokoly**: Při spuštění programu terminálové okno zobrazuje barevné protokoly v reálném čase
   - Různé barvy reprezentují různé úrovně závažnosti (červená=chyba, žlutá=varování atd.)
   - Vhodné pro ladění vývoje a monitorování v reálném čase

2. **Souborové Protokoly**: Všechny protokoly jsou automaticky ukládány do adresáře `data/Log/`
   - Trvale uloženy, historii lze zobrazit kdykoli
   - Vhodné pro sledování problémů a následnou analýzu

## Možnosti Konfigurace

### Úprava Podrobnosti Protokolů

Můžete nastavit podrobnost protokolů v konfiguračním souboru:

- **Během Vývoje/Ladění**: Nastavit na ""Debug"" nebo ""Trace"" pro zobrazení podrobnějších informací
- **Denní Používání**: Nastavit na ""Information"" pro zaznamenávání běžných operací
- **Prostředí Produkce**: Nastavit na ""Warning"" pro zaznamenávání pouze problémů a chyb

## Zobrazení a Filtrování Protokolů

Soubory protokolů jsou uloženy v adresáři `data/Log/`, automaticky organizovány podle času pro snadné vyhledávání historie.

## Formát Zobrazení Protokolů

### Styl Zobrazení na Konzoli

```
[2026-04-27 10:30:00.123] [INFO] [Category] [Being:guid] Zpráva protokolu
```

- Časové razítko: Přesné na milisekundy
- Úroveň: Zarovnáno vpravo, šířka 4 znaky
- Kategorie: Identifikátor zdroje protokolu
- ID Křemíkové Bytosti: Zobrazeno pouze když se protokol vztahuje ke konkrétní bytosti

### Atributy Záznamu Protokolu

Každý záznam protokolu obsahuje následující informace:

- **BeingId**: ID přidružené křemíkové bytosti (prázdné pro systémové protokoly)
- **Timestamp**: Čas vytvoření protokolu (UTC)
- **Level**: Úroveň protokolu
- **Category**: Kategorie protokolu
- **Message**: Zpráva protokolu
- **Exception**: Informace o výjimce (pokud existuje)

## Osvědčené Postupy

### 1. Výběr Vhodných Úrovní Protokolu

- **Vývojové Prostředí**: Používat úrovně Debug nebo Trace
- **Prostředí Produkce**: Používat úrovně Information nebo Warning
- **Řešení Problémů**: Dočasně snížit na úroveň Debug

### 2. Na které protokoly se zaměřit?

Doporučuje se重点关注 následující situace:
- Informace o spuštění a vypnutí systému
- Vytváření křemíkových bytostí, anomálie za běhu a vypnutí
- Selhání ověřování oprávnění a bezpečnostní problémy
- Selhání volání AI a anomálie odpovědí
- Chyby provádění nástrojů a časové limity

### 3. Vyhnout se Dopadu na Výkon Nadměrným Protokolováním

Pokud jsou soubory protokolů příliš velké, můžete zvýšit nastavení podrobnosti protokolů (např. změnit na ""Warning"" nebo ""Error"") pro snížení množství záznamů.

### 4. Ochrana Citlivých Informací

Systém NEZAZNAMENÁVÁ následující citlivé informace do protokolů:
- Hesla a klíče
- Osobní identifikační údaje
- Klíče a tokeny API
- Připojovací řetězce databáze

## Často Kladené Otázky

### Otázka: Proč nevidím některé zprávy protokolu?

**Odpověď**: Možné příčiny:
1. Nastavení podrobnosti protokolů je příliš vysoké, filtruje některé informace (lze upravit na ""Debug"" nebo ""Trace"" v konfiguraci)
2. Systém se nespustil správně, funkce protokolování neinicializována

### Otázka: Jak zobrazit podrobnější protokoly?

**Odpověď**: Změňte nastavení podrobnosti protokolů v konfiguračním souboru na ""Debug"" nebo ""Trace"", poté restartujte systém.

### Otázka: Kde jsou soubory protokolů?

**Odpověď**: Soubory protokolů jsou uloženy v adresáři `data/Log/`, indexovány podle času.

## Řešení Problémů

### Problém: Nelze najít historické záznamy protokolů

**Příznaky**: Nelze zobrazit minulé protokoly

**Řešení**:
1. Zkontrolovat, zda adresář `data/Log/` existuje a má oprávnění ke čtení/zápisu
2. Potvrdit, že je k dispozici dostatek místa na disku

## Poznámky

- Systém protokolování zajišťuje stabilní provoz; selhání jednoho výstupního cíle neovlivní ostatní cíle
- Pokud jsou soubory protokolů příliš velké, můžete zvýšit nastavení podrobnosti protokolů (např. změnit na ""Warning"" nebo ""Error"") pro snížení objemu protokolů
- Při zápisu velkého množství protokolů se doporučuje přiměřeně zvýšit úroveň protokolu
";
    
    #endregion
}
