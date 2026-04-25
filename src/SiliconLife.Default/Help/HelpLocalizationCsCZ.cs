// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

using SiliconLife.Collective;

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
    public override string TaskTimer_Title => "Úlohy a časovače";
    public override string Permission_Title => "Správa oprávnění";
    public override string Config_Title => "Konfigurace";
    public override string FAQ_Title => "Často kladené otázky";

    public override string[] GettingStarted_Tags => new[] { "instalace", "spuštění", "nastavení", "rychlý start", "začínáme", "zahájení", "inicializace", "konfigurace" };
    public override string[] BeingManagement_Tags => new[] { "bytost", "vytvořit", "konfigurace", "správa bytostí", "křemíková bytost", "profil", "nastavení", "spravovat" };
    public override string[] ChatSystem_Tags => new[] { "chat", "zpráva", "konverzace", "chatovací systém", "dialog", "komunikace", "rozhovor", "diskuse" };
    public override string[] TaskTimer_Tags => new[] { "úloha", "časovač", "plánování", "úlohy a časovače", "cron", "automatizace", "opakující se", "připomínka" };
    public override string[] Permission_Tags => new[] { "oprávnění", "zabezpečení", "řízení přístupu", "správa oprávnění", "autentizace", "autorizace", "soukromí", "ochrana" };
    public override string[] Config_Tags => new[] { "konfigurace", "nastavení", "možnosti", "předvolby", "přizpůsobení", "systém", "parametry", "volby" };
    public override string[] FAQ_Tags => new[] { "časté otázky", "nápověda", "problémy", "podpora", "řešení problémů", "průvodce", "asistence", "odpovědi" };
    
    public override string GettingStarted => @"
# Rychlý start

## Spuštění systému

Spusťte program v příkazovém řádku. Systém se automaticky spustí a bude naslouchat na portu 8080.

```bash
dotnet run
```

## Přístup k webovému rozhraní

Otevřete prohlížeč a navštivte `http://localhost:8080` pro přístup k webovému rozhraní pro správu.

## První spuštění

Při prvním spuštění systém automaticky vytvoří Křemíkového kurátora. Budete potřebovat:

1. Nastavit jméno kurátora
2. Konfigurovat soubor duše (prompt)
3. Vybrat AI model

## Základní operace

- **Řídicí panel**: Zobrazit stav systému a statistiky
- **Bytosti**: Vytvářet a spravovat křemíkové bytosti
- **Chat**: Konverzovat s křemíkovými bytostmi
- **Úlohy**: Nastavit plánované úlohy
- **Konfigurace**: Upravit nastavení systému
- **Nápověda**: Zobrazit tuto dokumentaci

## Klávesové zkratky

- `F1` - Otevřít dokumentaci nápovědy
- `Ctrl+F` - Zaměřit vyhledávací pole
";

    public override string BeingManagement => @"
# Správa bytostí

## Vytvoření křemíkové bytosti

1. Přejděte na stránku ""Bytosti""
2. Klikněte na ""Vytvořit novou bytost""
3. Vyplňte následující informace:
   - **Jméno**: Zobrazovaný název bytosti
   - **Soubor duše**: Hlavní prompt určující chování (podporuje Markdown)
   - **AI model**: Vyberte AI model k použití
4. Klikněte na ""Vytvořit"" pro dokončení

## Konfigurace souboru duše

Soubor duše je hlavním promptem křemíkové bytosti, určující její vzorce chování, osobnostní rysy a schopnosti.

### Pokyny pro psaní

```markdown
# Nastavení role

Jste profesionální programátorský asistent, specializující se na:
- Vývoj v C#
- Návrh architektury
- Kontrolu kódu

# Pokyny pro chování

1. Vždy poskytujte spustitelné příklady kódu
2. Vysvětlujte klíčové části kódu
3. Poskytujte doporučení pro osvědčené postupy
```

## Správa bytostí

- **Upravit**: Změnit jméno, soubor duše a další konfigurace
- **Smazat**: Trvale smazat bytost (nevratné)
- **Klonovat**: Vytvořit novou verzi na základě existující bytosti
";

    public override string ChatSystem => @"
# Chatovací systém

## Zahájení konverzace

1. Vyberte křemíkovou bytost pro chat
2. Napište zprávu do vstupního pole
3. Stiskněte Enter nebo klikněte na tlačítko odeslat

## Funkce chatu

- **Odpověď v reálném čase**: Streamovaný výstup pomocí technologie SSE
- **Volání nástrojů**: AI může volat nástroje k provádění operací
- **Kontextová paměť**: Ukládá historii konverzace
- **Více jazyků**: Podporuje konverzace ve více jazycích

## Použití nástrojů

Křemíkové bytosti mohou automaticky volat nástroje pro:
- Dotazování na informace z kalendáře
- Správu systémových konfigurací
- Spouštění kódu
- Přístup k souborovému systému (vyžaduje oprávnění)

## Přerušení konverzace

Pokud AI přemýšlí, můžete:
- Kliknout na tlačítko ""Zastavit""
- Nebo odeslat novou zprávu pro automatické přerušení
";

    public override string TaskTimer => @"
# Úlohy a časovače

## Vytvoření plánovaných úloh

1. Přejděte na stránku ""Úlohy""
2. Klikněte na ""Nová úloha""
3. Konfigurujte úlohu:
   - **Název úlohy**: Popisný název
   - **Spouštěč**: Cron výraz
   - **Akce**: Vyberte operaci k provedení
   - **Cílová bytost**: Vyberte vykonavatele

## Cron výraz

```
minuta (0-59)
| hodina (0-23)
| | den v měsíci (1-31)
| | | měsíc (1-12)
| | | | den v týdnu (0-6)
| | | | |
* * * * *
```

### Příklady

- `0 * * * *` - Každou hodinu celou hodinu
- `0 9 * * *` - Každý den v 9:00
- `*/5 * * * *` - Každých 5 minut
- `0 9 * * 1-5` - Pracovní dny v 9:00

## Správa úloh

- **Povolit/Zakázat**: Dočasně deaktivovat úlohy
- **Upravit**: Změnit konfiguraci úlohy
- **Smazat**: Odstranit úlohy
- **Historie provádění**: Zobrazit záznamy o provádění úloh
";

    public override string Permission => @"
# Správa oprávnění

## Úrovně oprávnění

Systém používá 5úrovňovou kontrolu oprávnění:

1. **IsCurator**: Kurátor má nejvyšší oprávnění
2. **UserFrequencyCache**: Limity frekvence uživatelů
3. **GlobalACL**: Globální seznam řízení přístupu
4. **IPermissionCallback**: Vlastní zpětné volání oprávnění
5. **IPermissionAskHandler**: Zeptat se na oprávnění uživatele

## Typy oprávnění

- **Čtení**: Zobrazit informace a data
- **Zápis**: Upravit a vytvořit data
- **Spouštění**: Spouštět nástroje a příkazy
- **Správa**: Spravovat systémové konfigurace

## Konfigurace oprávnění

1. Přejděte na stránku ""Konfigurace""
2. Vyberte ""Nastavení oprávnění""
3. Konfigurujte oprávnění:
   - Povolit/zakázat specifické operace
   - Nastavit limity frekvence
   - Konfigurovat bílou/černou listinu

## Doporučení pro zabezpečení

- Pravidelně kontrolujte konfigurace oprávnění
- Omezte přístup k citlivým operacím
- Povolte protokolování operací
- Dodržujte princip nejmenších oprávnění
";

    public override string Config => @"
# Konfigurace

## Systémová konfigurace

### Konfigurace AI modelu

```json
{
  ""AI"": {
    ""DefaultProvider"": ""ollama"",
    ""Models"": {
      ""ollama"": {
        ""Endpoint"": ""http://localhost:11434"",
        ""Model"": ""llama3""
      }
    }
  }
}
```

### Síťová konfigurace

- **Port**: Výchozí 8080
- **Hostitel**: Výchozí localhost
- **HTTPS**: Volitelné

### Konfigurace úložiště

- **Adresář dat**: Umístění ukládání dat křemíkových bytostí
- **Úroveň protokolu**: Debug/Info/Warning/Error
- **Strategie zálohování**: Frekvence automatického zálohování

## Témata vzhledu

Systém podporuje více rozhraní témat:

- **Light**: Světlé téma (výchozí)
- **Dark**: Tmavé téma
- **Custom**: Vytvořte si vlastní téma

## Lokalizace

Podporované jazyky:
- Zjednodušená čínština (zh-CN)
- Tradiční čínština (zh-HK)
- Angličtina (en-US, en-GB)
- Japonština (ja-JP)
- Korejština (ko-KR)
- Španělština (es-ES, es-MX)
- Čeština (cs-CZ)
";

    public override string FAQ => @"
# Často kladené otázky

## Systém

### O: Co dělat, když se systém nespustí?

A: Zkontrolujte následující:
1. Je port 8080 již používán?
2. Je runtime .NET 9 správně nainstalován?
3. Zkontrolujte soubory protokolu pro podrobné chybové zprávy

### O: Jak změnit naslouchající port?

A: Upravte `WebHost:Port` v konfiguračním souboru nebo použijte argumenty příkazového řádku.

### O: Kde jsou uložena data?

A: Výchozí umístění je ve složce `data` v kořenovém adresáři křemíkové bytosti.

## AI

### O: AI odpovídá pomalu, co dělat?

A: Možné příčiny:
1. Velký model vyžaduje více výpočetních zdrojů
2. Síťová latence (při použití cloudových modelů)
3. Zvažte použití lokálních modelů (jako Ollama)

### O: Jak přepnout AI modely?

A: Upravte `AI:DefaultProvider` v konfiguračním souboru nebo vyberte různé modely v konfiguraci bytosti.

### O: AI nemůže volat nástroje?

A: Zkontrolujte:
1. Jsou nástroje správně registrovány?
2. Umožňují oprávnění volání nástrojů?
3. Podporuje AI model volání nástrojů?

## Bytosti

### O: Jak zálohovat data bytosti?

A: Zkopírujte všechny soubory z adresáře bytosti do umístění zálohy.

### O: Mohu importovat/exportovat soubory duše?

A: Ano. Můžete importovat/exportovat soubory duše ve formátu Markdown na stránce úprav bytosti.

### O: Jak klonovat bytost?

A: Vyberte ""Klonovat"" v seznamu bytostí, systém vytvoří kopii, kterou můžete upravit.

## Získání pomoci

Pokud výše uvedené nevyřeší váš problém:

1. Zkontrolujte [dokumentaci projektu](https://github.com/your-repo/docs)
2. Odešlete issue k nahlášení problémů
3. Připojte se ke komunitním diskusím
";
    
    #endregion
}
