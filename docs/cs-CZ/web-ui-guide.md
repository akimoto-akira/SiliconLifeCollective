# Příručka Web UI

> **Verze: v0.2.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | **Čeština** | [Русский](../ru-RU/web-ui-guide.md)

## Přehled

Web UI poskytuje komplexní rozhraní pro správu Křemíkových Bytostí, sledování stavu systému a interakci s AI agenty. Systém využívá čistě serverovou architekturu renderování, s nulovou závislostí na frontendovém frameworku, a generuje HTML, CSS a JavaScript pomocí builderů `H`, `CssBuilder` a `JsBuilder`.

## Přístup

Výchozí URL: `http://localhost:8080`

## Navigace

### Hlavní sekce

1. **Řídicí panel** - Přehled systému a metriky
2. **Bytosti** - Správa Křemíkových Bytostí
3. **Chat** - Interakce s bytostmi (podpora nahrávání souborů, SSE v reálném čase)
4. **Historie chatu** - Zobrazení historie chatu Křemíkových Bytostí (seznam relací, detaily zpráv)
5. **Úkoly** - Správa úkolů (osobní úkoly)
6. **Časovače** - Konfigurace časovačů (vytvoření, pozastavení, historie provádění)
7. **Konfigurace** - Nastavení systému (AI klienti, lokalizace)
8. **Oprávnění** - Řízení přístupu (správa ACL, dotazy na oprávnění)
9. **Protokoly** - Systémové protokoly (filtrování podle úrovně, dotazy na časové rozsahy)
10. **Audit** - Využití Tokenů a auditní stopa
11. **Paměť** - Paměť bytostí (časová osa, pokročilé filtrování)
12. **Znalosti** - Znalostní báze (správa trojic, objevování cest)
13. **Prohlížeč kódu** - Průzkum kódu (strom souborů, zvýraznění syntaxe)
14. **Editor kódu** - Editace kódu s plovoucími tipy (Monaco Editor)
15. **Projekty** - Správa projektů (pracovní prostor, úkoly, pracovní poznámky)
16. **Exekutoři** - Správa exekutorů (disk, síť, příkazový řádek)
17. **Nápověda** - Systém dokumentace nápovědy (vícejazyčná podpora, vyhledávání témat)
18. **O projektu** - Informace o systému a verze

---

## Řídicí panel

### Funkce

- Metriky systémového výkonu (CPU, paměť, doba běhu)
- Přehled stavu bytostí
- Statistiky využití AI
- Rychlé akce

### Aktualizace v reálném čase

Použití SSE (Server-Sent Events) pro data v reálném čase:

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Správa bytostí

### Seznam bytostí

Zobrazuje všechny bytosti, včetně:
- Název a ID
- Aktuální stav (běžící/zastavený/chyba)
- Odkaz na Soubor Duše
- Rychlé akce (spuštění/zastavení/konfigurace)

### Detaily bytosti

- Kompletní konfigurace
- Editor Souboru Duše
- Historie úkolů
- Prohlížeč paměti
- Metriky výkonu

### Vytvoření bytosti

1. Klikněte na **Vytvořit novou bytost**
2. Vyplňte:
   - Název
   - Obsah duše (Markdown editor)
   - Počáteční konfigurace
3. Klikněte na **Vytvořit**

---

## Chatovací rozhraní

### Funkce

- Stream zpráv v reálném čase
- Historie zpráv
- Podpora více relací
- Vizualizace volání nástrojů

### Používání chatu

1. Vyberte bytost
2. Zadejte zprávu
3. Zobrazte streamovanou odpověď
4. Sledujte provádění nástrojů v reálném čase

### Zobrazení volání nástrojů

Když AI volá nástroj:
```
🔧 Nástroj: calendar
📥 Vstup: {"date": "2026-04-20"}
📤 Výstup: "Čínský lunární 3. den 4. měsíce"
```

---

## Konfigurace

### AI klienti

Konfigurace AI backendu:
- Ollama (lokální)
- Bailian (cloud)
- Volcengine Ark (cloud)
- Herdsman (lokální/cloud, bez autentizace)
- Meituan LongCat (cloud)
- Qiniu Cloud AI (cloud)
- DeepSeek (cloud, thinking mode, 1M kontext)
- Zhipu AI / GLM (cloud, thinking, vision, 1M kontext)
- Baidu Qianfan / Ernie (cloud, bezplatné modely, 131K kontext)
- Tencent Hunyuan (cloud, duální endpoint, 262K kontext)
- MiniMax (cloud, domácí/mezinárodní endpoint, 1M kontext)
- Moonshot / Kimi (cloud, thinking, multimodální, 262K kontext)
- SiliconFlow (cloud, agregátor 100+ modelů, 1M kontext)
- Vlastní klienti

### Nastavení úložiště

- Verze Default: základní cesta, časové indexování, strategie čištění
- Verze Fast: konfigurace úložného enginu SpeedyPack, správa .spk souborů, nastavení automatické komprimace

### Lokalizace

Přepínání mezi 34 jazykovými variantami:
- Čínština (6 variant): zjednodušená, tradiční, singapurská, macajská, tchajwanská, malajsijská
- Angličtina (10 variant): americká, britská, kanadská, australská, indická, singapurská, jihoafrická, irská, novozélandská, malajsijská
- Španělština (2 varianty): španělská, mexická
- Němčina (5 variant): německá, rakouská, švýcarská, lucemburská, lichtenštejnská
- Francouzština (3 varianty): francouzská, kanadská, švýcarská
- Japonština, korejština, čeština
- Ruština, portugalština (2 varianty), italština, nizozemština, polština, švédština

---

## Systém skinů

### Dostupné skiny

1. **Admin** - Profesionální administrativní rozhraní
2. **Chat** - Na konverzaci zaměřený design
3. **Creative** - Kreativní a umělecký styl
4. **Dev** - Rozhraní pro vývojáře
5. **HighContrast** - Téma s vysokým kontrastem (verze Fast)
6. **Minimal** - Minimalistický styl (verze Fast)
7. **Light** - Světlé téma (verze Fast)

### Přepínání skinů

1. Klikněte na **Nastavení** (ikona ozubeného kola)
2. Vyberte **Skiny**
3. Zvolte požadovaný skin
4. Rozhraní se okamžitě aktualizuje

### Vlastní skiny

Vytvoření vlastního skinu implementací `ISkin`:

```csharp
public class MySkin : ISkin
{
    public string Name => "MySkin";
    
    public string GetCss()
    {
        return ":root { --primary: #color; }";
    }
}
```

---

## Správa oprávnění

### Zobrazení oprávnění

- Seznam všech pravidel oprávnění
- Filtrování podle uživatele nebo prostředku
- Zobrazení dat vypršení

### Přidání pravidla oprávnění

1. Klikněte na **Přidat pravidlo**
2. Nakonfigurujte:
   - Typ oprávnění (např. `FileAccess`, `NetworkAccess`)
   - Předpona prostředku (např. `C:\Projects`, `api.github.com`)
   - Povolit/Zamítnout
   - Popis
3. Uložte

### Auditní stopa

Zobrazení všech rozhodnutí o oprávněních:
- Časové razítko
- Uživatel
- Prostředek
- Rozhodnutí
- Důvod

### Správa oprávnění nástrojů

Správa oprávnění operací nástrojů pro Křemíkové Bytosti a projekty:

1. **Oprávnění nástrojů Křemíkové Bytosti**:
   - Přejděte na **Bytosti** → vyberte bytost → **Oprávnění nástrojů**
   - Zobrazení aktuální konfigurace oprávnění
   - Nastavení povolit/zamítnout pro jednotlivé operace
   - Aplikace šablony oprávnění (readonly/restricted/full)

2. **Oprávnění nástrojů projektu**:
   - Přejděte na **Projekty** → vyberte projekt → **Oprávnění nástrojů**
   - Oprávnění nástrojů na úrovni projektu jsou nezávislá na úrovni Křemíkové Bytosti
   - Dosahování izolace oprávnění mezi projekty

---

## Správa úkolů

### Seznam úkolů

- Všechny úkoly a jejich stavy
- Filtrování podle bytosti nebo stavu
- Indikátory priority

### Detaily úkolu

- Popis
- Priorita
- Termín dokončení
- Historie provádění
- Výstup výsledku

### Vytvoření úkolu

1. Klikněte na **Vytvořit úkol**
2. Vyplňte:
   - Přiřazení bytosti
   - Popis
   - Priorita (1-10)
   - Termín dokončení
3. Vytvořte

---

## Správa časovačů

### Aktivní časovače

- Seznam běžících časovačů
- Čas dalšího provedení
- Stav opakování

### Vytvoření časovače

1. Klikněte na **Vytvořit časovač**
2. Nakonfigurujte:
   - Přiřazení bytosti
   - Interval nebo cron výraz
   - Akce k provedení
   - Nastavení opakování
3. Spusťte

---

## Prohlížeč protokolů

### Funkce

- Filtrování podle úrovně (informace/varování/chyba)
- Vyhledávání podle klíčových slov
- Výběr časového rozsahu
- Aktualizace v reálném čase

### Detaily protokolu

Každá položka protokolu zobrazuje:
- Časové razítko
- Úroveň
- Zdroj
- Zpráva
- Trasování zásobníku (pro chyby)

---

## Auditní reporty

### Využití Tokenů

- Celkové použité tokeny
- Rozdělení podle modelu
- Výpočet nákladů
- Grafy založené na čase

### Export reportů

Stahování auditních dat:
- Formát CSV
- Výběr rozsahu dat
- Filtrování podle bytosti nebo modelu

---

## Editor kódu

### Funkce

- Zvýraznění syntaxe (Monaco Editor)
- Doplňování kódu
- Plovoucí tipy pro identifikátory
- Kompilace v reálném čase

### Plovoucí tipy

Najetím myší na jakýkoliv identifikátor zobrazíte:
- Informace o typu
- Dokumentaci
- Místo definice
- Reference

---

## Zobrazení historie chatu

### Funkce

- Procházení historie chatu Křemíkových Bytostí
- Zobrazení seznamu relací
- Zobrazení detailů zpráv
- Zobrazení na časové ose

### Používání historie chatu

1. Přejděte na stránku **Bytosti**
2. Klikněte na odkaz **Historie chatu** Křemíkové Bytosti
3. Zobrazte seznam relací:
   - Název relace
   - Čas vytvoření
   - Počet zpráv
4. Klikněte na relaci pro zobrazení detailů:
   - Kompletní historie zpráv
   - Časová razítka
   - Informace o odesílateli
   - Záznamy volání nástrojů

### Technická implementace

- **Kontroler**: `ChatHistoryController`
- **Model pohledu**: `ChatHistoryViewModel`
- **Pohledy**:
  - `ChatHistoryListView` - Seznam relací
  - `ChatHistoryDetailView` - Detaily zpráv
- **API trasy**:
  - `/api/chat-history/{beingId}/conversations` - Získání seznamu relací
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Získání detailů zpráv

---

## Nahrávání souborů

### Funkce

- Dialog zdroje souborů
- Podpora nahrávání více souborů
- Správa metadat souborů
- Zobrazení průběhu nahrávání

### Používání nahrávání souborů

1. V chatovacím rozhraní klikněte na tlačítko **Nahrát soubor**
2. Otevře se dialog zdroje souborů
3. Vyberte zdroj souborů:
   - Lokální soubor
   - Cesta souborového systému
4. Vyberte soubory (podpora vícenásobného výběru)
5. Potvrďte nahrání
6. Informace o souboru budou připojeny ke zprávě

### Podporované typy souborů

- Textové soubory (.txt, .md, .json, .xml, atd.)
- Kódové soubory (.cs, .js, .py, .java, atd.)
- Konfigurační soubory (.yml, .yaml, .ini, .conf, atd.)
- Dokumentové soubory (.csv, .log, atd.)

---

## Indikátor načítání

### Funkce

- Zobrazení stavu načítání chatovací stránky
- Automatický výběr relace Kurátora
- Zpětná vazba o průběhu načítání dat

### Chování

- Při načítání stránky se zobrazí animace načítání
- Po dokončení načítání dat se automaticky skryje
- Relace Kurátora je automaticky vybrána (pokud existuje)
- Vícejazyčné texty indikátoru načítání

---

## Systém dokumentace nápovědy (nové)

### Přehled funkcí

Systém dokumentace nápovědy poskytuje vícejazyčnou podporu dokumentace pro Křemíkové Bytosti a uživatele.

### Používání dokumentace nápovědy

1. Přejděte na stránku **Nápověda**
2. Zobrazte seznam témat nápovědy:
   - Příručka rychlého startu
   - Reference použití nástrojů
   - Příručka správy oprávnění
   - Příručka řešení problémů
   - Vývojářská příručka
3. Klikněte na téma pro zobrazení detailního obsahu:
   - Strukturovaný obsah dokumentace (vykreslování Markdown)
   - Vícejazyčná podpora (následuje systémové nastavení lokalizace)
   - Doporučení souvisejících témat
4. Použijte funkci vyhledávání pro rychlou navigaci:
   - Vyhledávání podle klíčových slov (podpora češtiny, angličtiny)
   - Výsledky vyhledávání seřazené podle relevance

### Přístup Křemíkových Bytostí k nápovědě

Křemíkové Bytosti mohou přistupovat k dokumentaci nápovědy pomocí nástroje `help`:
```json
{
  "action": "get_topics"
}
```

### Technická implementace

- **Kontroler**: `HelpController`
- **Nástroj**: `HelpTool`
- **API trasy**:
  - `/api/help` - Získání seznamu témat nápovědy
  - `/api/help/{topicId}` - Získání detailů tématu
  - `/api/help/search?q=keyword` - Vyhledávání v dokumentaci nápovědy

---

## Projektový pracovní prostor (nové)

### Přehled funkcí

Projektový pracovní prostor poskytuje strukturované pracovní prostředí, podporuje správu projektů, sledování úkolů a pracovní poznámky.

### Správa projektů

1. **Vytvoření projektu**:
   - Název a popis projektu
   - Štítky projektu (kategorizace)
   - Stav projektu (probíhající, dokončený, archivovaný)
2. **Zobrazení detailů projektu**:
   - Základní informace o projektu
   - Seznam souvisejících úkolů
   - Seznam pracovních poznámek
   - Statistiky postupu projektu
3. **Archivace projektu**: Zachování historických dat, ale projekt již není aktivní
4. **Správa projektových rolí**:
   - Přiřazení projektových rolí Křemíkovým Bytostem (např. developer, reviewer, manager)
   - Odebrání přiřazení rolí
   - Zobrazení členů projektu a seznamu rolí
5. **Projektové pracovní postupy**:
   - Zobrazení seznamu šablon pracovních postupů
   - Vázání šablony pracovního postupu na projekt
   - Zobrazení stavu instance pracovního postupu
   - Zobrazení protokolů provádění pracovního postupu

### Pracovní poznámky (soukromé)

Osobní pracovní poznámky Křemíkové Bytosti, podobné deníku:

1. **Vytvoření poznámky**:
   - Shrnutí (krátký popis)
   - Obsah (podpora formátu Markdown)
   - Klíčová slova (pro vyhledávání)
   - Automatické zaznamenávání časových razítek
2. **Správa poznámek**:
   - Procházení na časové ose (stránkový design)
   - Vyhledávání poznámek (podle klíčových slov, shrnutí, obsahu)
   - Generování obsahu (rychlý přehled struktury poznámek)
   - Aktualizace a mazání poznámek
3. **Řízení přístupu**:
   - Výchozí soukromé, pouze bytost sama má přístup
   - Kurátor Křemíku může spravovat všechny poznámky

### Technická implementace

- **Kontroler**: `WorkNoteController`
- **Nástroje**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **API trasy**:
  - `/api/worknotes` - Získání seznamu pracovních poznámek
  - `/api/worknotes/{id}` - Získání detailů poznámky
  - `/api/worknotes/search?q=keyword` - Vyhledávání poznámek
  - `/api/worknotes/directory` - Generování obsahu poznámek
  - `/api/projects` - API správy projektů

---

## Responzivní design

Web UI se přizpůsobuje různým velikostem obrazovky:
- Desktop: kompletní rozložení
- Tablet: komprimovaný postranní panel
- Mobil: sbalitelné menu

---

## Klávesové zkratky

| Zkratka | Akce |
|----------|--------|
| `Ctrl+K` | Rychlé vyhledávání |
| `Ctrl+B` | Přepnutí postranního panelu |
| `Ctrl+Enter` | Odeslání zprávy |
| `Esc` | Zrušit/Zavřít |

---

## Řešení problémů

### Nelze se připojit

**Zkontrolujte**:
- Server běží
- Port 8080 není blokován
- Nastavení brány firewall

### SSE nefunguje

**Zkontrolujte**:
- Prohlížeč podporuje SSE
- Žádný proxy nebufferuje SSE
- Stabilita sítě

### Pomalý výkon

**Optimalizace**:
- Snížení úrovně detailů protokolů
- Vyčištění starých auditních dat
- Kontrola systémových prostředků

---

## Další kroky

- 📚 Přečtěte [příručku architektury](architecture.md)
- 🛠️ Prohlédněte [vývojářskou příručku](development-guide.md)
- 📖 Prozkoumejte [API referenci](api-reference.md)
- 🚀 Prohlédněte [příručku rychlého startu](getting-started.md)
