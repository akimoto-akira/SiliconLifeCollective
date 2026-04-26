# Web UI Průvodce

[English](../en/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | **Čeština**

## Přehled

Web UI poskytuje komplexní rozhraní pro správu silikonových bytostí, monitorování stavu systému a interakci s AI agenty. Systém používá čistou serverovou architekturu vykreslování, nulovou závislost na frontendovém frameworku, generuje HTML, CSS a JavaScript prostřednictvím konstruktorů `H`, `CssBuilder` a `JsBuilder`.

## Přístup

Výchozí URL: `http://localhost:8080`

## Navigace

### Hlavní Sekce

1. **Dashboard** — Přehled systému a metriky
2. **Bytosti** — Správa silikonových bytostí
3. **Chat** — Interakce s bytostmi (podpora nahrávání souborů, SSE v reálném čase)
4. **Historie Chatu** — Zobrazení historie chatu silikonových bytostí (seznam relací, detaily zpráv)
5. **Úkoly** — Správa úkolů (osobní úkoly)
6. **Časovače** — Konfigurace časovačů (vytváření, pozastavení, historie provádění)
7. **Konfigurace** — Nastavení systému (AI klienti, lokalizace)
8. **Oprávnění** — Řízení přístupu (správa ACL, dotazování oprávnění)
9. **Logy** — Systémové logy (filtrování podle úrovně, dotazování podle časového rozsahu)
10. **Audit** — Využití tokenů a auditní stopy
11. **Paměť** — Paměť bytostí (zobrazení časové osy, pokročilé filtrování)
12. **Znalosti** — Znalostní báze (správa tripletů, objevování cest)
13. **Prohlížeč Kódu** — Průzkum kódu (strom souborů, zvýraznění syntaxe)
14. **Editor Kódu** — Úprava kódu s plovoucími nápovědami (Monaco Editor)
15. **Projekty** — Správa projektů (pracovní prostory, úkoly, pracovní poznámky)
16. **Exekutory** — Správa exekutorů (disk, síť, příkazový řádek)
17. **Nápověda** — Systém nápovědní dokumentace (vícejazyčná podpora, vyhledávání témat)
18. **O Aplikaci** — Informace o systému a verze

---

## Dashboard

### Funkce

- Metriky výkonu systému (CPU, paměť, doba běhu)
- Přehled stavu bytostí
- Statistiky využití AI
- Rychlé akce

### Aktualizace v Reálném Čase

Použijte SSE (Server-Sent Events) pro získání dat v reálném čase:

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Správa Bytostí

### Seznam Bytostí

Zobrazuje všechny bytosti včetně:
- Jména a ID
- Aktuálního stavu (Běží/Zastaveno/Chyba)
- Odkazu na soubor duše
- Rychlých akcí (Spustit/Zastavit/Konfigurovat)

### Detaily Bytosti

- Kompletní konfigurace
- Editor souboru duše
- Historie úkolů
- Prohlížeč paměti
- Metriky výkonu

### Vytvoření Bytosti

1. Klikněte na **Vytvořit Novou Bytost**
2. Vyplňte:
   - Jméno
   - Obsah duše (Markdown editor)
   - Počáteční konfigurace
3. Klikněte na **Vytvořit**

---

## Rozhraní Chatu

### Funkce

- Proud zpráv v reálném čase
- Historie zpráv
- Podpora více relací
- Vizualizace volání nástrojů

### Používání Chatu

1. Vyberte bytost
2. Zadejte zprávu
3. Zobrazte odpověď ve streamu
4. Sledujte provádění nástrojů v reálném čase

### Zobrazení Volání Nástrojů

Když AI volá nástroj:
```
🔧 Nástroj: calendar
📥 Vstup: {"date": "2026-04-20"}
📤 Výstup: "Lunární duben třetí den"
```

---

## Konfigurace

### AI Klienti

Konfigurace AI backendu:
- Ollama (lokální)
- Bailian (cloudový)
- Vlastní klienti

### Nastavení Úložiště

- Základní cesta
- Časový index
- Strategie čištění

### Lokalizace

Přepínejte mezi 21 jazykovými variantami:
- Čínština (6 variant): Zjednodušená čínština, Tradiční čínština, Singapurská čínština, Macajská čínština, Tchajwanská čínština, Malajsijská čínština
- Angličtina (10 variant): Americká, Britská, Kanadská, Australská, Indická, Singapurská, Jihhoafrická, Irská, Novozélandská, Malajsijská angličtina
- Španělština (2 varianty): Španělská, Mexická
- Japonština, Korejština, Čeština

---

## Systém Skinů

### Dostupné Skiny

1. **Admin** — Profesionální správcovské rozhraní
2. **Chat** — Design zaměřený na konverzaci
3. **Creative** — Kreativní a umělecký styl
4. **Dev** — Rozložení orientované na vývojáře

### Přepnutí Skinu

1. Klikněte na **Nastavení** (ikona ozubeného kolečka)
2. Vyberte **Skin**
3. Vyberte požadovaný skin
4. Rozhraní se okamžitě aktualizuje

### Vlastní Skin

Vytvořte vlastní skin implementací `ISkin`:

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

## Správa Oprávnění

### Zobrazení Oprávnění

- Seznam všech pravidel oprávnění
- Filtrování podle uživatele nebo zdroje
- Zobrazení data vypršení platnosti

### Přidání Pravidla Oprávnění

1. Klikněte na **Přidat Pravidlo**
2. Konfigurujte:
   - Uživatel
   - Zdroj (např. `disk:read`)
   - Povolit/Zamítnout
   - Doba trvání
3. Uložit

### Auditní Stopa

Zobrazit všechna rozhodnutí o oprávněních:
- Časové razítko
- Uživatel
- Zdroj
- Rozhodnutí
- Důvod

---

## Správa Úkolů

### Seznam Úkolů

- Všechny úkoly a jejich stavy
- Filtrování podle bytosti nebo stavu
- Indikátor priority

### Detaily Úkolu

- Popis
- Priorita
- Termín splnění
- Historie provádění
- Výstup výsledku

### Vytvoření Úkolu

1. Klikněte na **Vytvořit Úkol**
2. Vyplňte:
   - Přiřazení bytosti
   - Popis
   - Priorita (1-10)
   - Termín splnění
3. Vytvořit

---

## Správa Časovačů

### Aktivní Časovače

- Seznam běžících časovačů
- Příští čas provedení
- Stav opakování

### Vytvoření Časovače

1. Klikněte na **Vytvořit Časovač**
2. Konfigurujte:
   - Přiřazení bytosti
   - Interval nebo cron výraz
   - Akce k provedení
   - Nastavení opakování
3. Spustit

---

## Prohlížeč Logů

### Funkce

- Filtrování podle úrovně (Info/Warning/Error)
- Vyhledávání podle klíčového slova
- Výběr časového rozsahu
- Aktualizace v reálném čase

### Detaily Logu

Každý záznam logu zobrazuje:
- Časové razítko
- Úroveň
- Zdroj
- Zpráva
- Sledování zásobníku (pro chyby)

---

## Auditní Zprávy

### Využití Tokenů

- Celkový počet použitých tokenů
- Rozdělení podle modelu
- Výpočet nákladů
- Grafy založené na čase

### Export Zpráv

Stáhněte si auditní data:
- Formát CSV
- Výběr časového rozsahu
- Filtrování podle bytosti nebo modelu

---

## Editor Kódu

### Funkce

- Zvýraznění syntaxe (Monaco Editor)
- Doplňování kódu
- Plovoucí nápovědy pro identifikátory
- Kompilace v reálném čase

### Plovoucí Nápovědy

Najeďte myší na libovolný identifikátor pro zobrazení:
- Informace o typu
- Dokumentace
- Umístění definice
- Reference

---

## Prohlížení Historie Chatu

### Funkce

- Prohlížení historie chatu silikonových bytostí
- Zobrazení seznamu relací
- Zobrazení detailů zpráv
- Zobrazení časové osy

### Používání Historie Chatu

1. Navigujte na stránku **Bytosti**
2. Klikněte na odkaz **Historie Chatu** u silikonové bytosti
3. Zobrazte seznam relací:
   - Název relace
   - Čas vytvoření
   - Počet zpráv
4. Klikněte na relaci pro zobrazení detailů:
   - Kompletní historie zpráv
   - Časová razítka
   - Informace o odesílateli
   - Záznamy volání nástrojů

### Technická Implementace

- **Kontroler**: `ChatHistoryController`
- **View Model**: `ChatHistoryViewModel`
- **Pohledy**:
  - `ChatHistoryListView` — Seznam relací
  - `ChatHistoryDetailView` — Detaily zpráv
- **API trasy**:
  - `/api/chat-history/{beingId}/conversations` — Získat seznam relací
  - `/api/chat-history/{beingId}/conversation/{conversationId}` — Získat detaily zpráv

---

## Nahrávání Souborů

### Funkce

- Dialog zdroje souborů
- Podpora nahrávání více souborů
- Správa metadat souborů
- Zobrazení průběhu nahrávání

### Používání Nahrávání Souborů

1. Klikněte na tlačítko **Nahrát Soubor** v rozhraní chatu
2. Otevře se dialog zdroje souborů
3. Vyberte zdroj souborů:
   - Lokální soubory
   - Cesta k souborovému systému
4. Vyberte soubory (podpora více výběrů)
5. Potvrďte nahrávání
6. Informace o souboru budou připojeny ke zprávě

### Podporované Typy Souborů

- Textové soubory (.txt, .md, .json, .xml, atd.)
- Kódové soubory (.cs, .js, .py, .java, atd.)
- Konfigurační soubory (.yml, .yaml, .ini, .conf, atd.)
- Dokumentové soubory (.csv, .log, atd.)

---

## Indikátor Načítání

### Funkce

- Zobrazení stavu načítání stránky chatu
- Automatický výběr relace kurátora
- Zpětná vazba o průběhu načítání dat

### Chování

- Při načítání stránky se zobrazí animační načítání
- Po dokončení načítání dat se automaticky skryje
- Relace kurátora je automaticky vybrána (pokud existuje)
- Vícejazyčný text výzvy k načítání

---

## Systém Nápovědní Dokumentace (Nové)

### Přehled Funkcí

Systém nápovědní dokumentace poskytuje vícejazyčnou podporu nápovědních dokumentů pro silikonové bytosti a uživatele.

### Používání Nápovědní Dokumentace

1. Navigujte na stránku **Nápověda**
2. Zobrazte seznam nápovědních témat:
   - Průvodce rychlým startem
   - Reference používání nástrojů
   - Průvodce správou oprávnění
   - Příručka řešení problémů
   - Vývojářská příručka
3. Klikněte na téma pro zobrazení podrobného obsahu:
   - Strukturovaný obsah dokumentu (vykreslování Markdown)
   - Vícejazyčná podpora (sleduje nastavení lokalizace systému)
   - Doporučení souvisejících témat
4. Použijte funkci vyhledávání pro rychlé nalezení:
   - Vyhledávání klíčových slov (podpora čínštiny, angličtiny)
   - Výsledky vyhledávání řazené podle relevance

### Přístup Silikonových Bytostí k Nápovědě

Silikonové bytosti mohou přistupovat k nápovědní dokumentaci prostřednictvím nástroje `help`:
```json
{
  "action": "get_topics"
}
```

### Technická Implementace

- **Kontroler**: `HelpController`
- **Nástroj**: `HelpTool`
- **API trasy**:
  - `/api/help` — Získat seznam nápovědních témat
  - `/api/help/{topicId}` — Získat detaily tématu
  - `/api/help/search?q=keyword` — Vyhledat nápovědní dokumenty

---

## Projektový Pracovní Prostor (Nové)

### Přehled Funkcí

Projektový pracovní prostor poskytuje strukturované pracovní prostředí podporující správu projektů, sledování úkolů a pracovní poznámky.

### Správa Projektů

1. **Vytvoření Projektu**:
   - Jméno a popis projektu
   - Štítky projektu (kategorizace)
   - Stav projektu (probíhá, dokončen, archivován)
2. **Zobrazení Detailů Projektu**:
   - Základní informace o projektu
   - Seznam přidružených úkolů
   - Seznam pracovních poznámek
   - Statistiky postupu projektu
3. **Archivace Projektu**: Zachování historických dat, ale již není aktivní

### Pracovní Poznámky (Soukromé)

Osobní pracovní poznámky silikonových bytostí, podobné deníku:

1. **Vytvoření Poznámky**:
   - Shrnutí (stručný popis)
   - Obsah (podpora formátu Markdown)
   - Klíčová slova (pro vyhledávání)
   - Automatické zaznamenání časového razítka
2. **Správa Poznámek**:
   - Prohlížení podle časové osy (stránkování)
   - Vyhledávání poznámek (podle klíčových slov, shrnutí, obsahu)
   - Generování obsahu (rychlé prohlížení struktury poznámek)
   - Aktualizace a mazání poznámek
3. **Řízení Oprávnění**:
   - Výchozí soukromé, přístupné pouze bytosti samotné
   - Silikonový kurátor může spravovat všechny poznámky

### Technická Implementace

- **Kontroler**: `WorkNoteController`
- **Nástroje**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **API trasy**:
  - `/api/worknotes` — Získat seznam pracovních poznámek
  - `/api/worknotes/{id}` — Získat detaily poznámky
  - `/api/worknotes/search?q=keyword` — Vyhledat poznámky
  - `/api/worknotes/directory` — Generovat obsah poznámek
  - `/api/projects` — API správy projektů

---

## Responzivní Design

Web UI se přizpůsobuje různým velikostem obrazovek:
- Desktop: Kompletní rozložení
- Tablet: Komprimovaný postranní panel
- Mobilní: Skládací menu

---

## Klávesové Zkratky

| Zkratka | Akce |
|----------|--------|
| `Ctrl+K` | Rychlé vyhledávání |
| `Ctrl+B` | Přepnout postranní panel |
| `Ctrl+Enter` | Odeslat zprávu |
| `Esc` | Zrušit/Zavřít |

---

## Řešení Problémů

### Nelze se Připojit

**Zkontrolujte**:
- Server běží
- Port 8080 není blokován
- Nastavení firewallu

### SSE Nefunguje

**Zkontrolujte**:
- Prohlížeč podporuje SSE
- Žádný proxy server nebufferuje SSE
- Stabilita sítě

### Pomalý Výkon

**Optimalizujte**:
- Snižte podrobnost logů
- Vyčistěte stará auditní data
- Zkontrolujte systémové zdroje

---

## Další Kroky

- 📚 Přečtěte si [Průvodce architekturou](architecture.md)
- 🛠️ Podívejte se na [Vývojářskou příručku](development-guide.md)
- 📖 Prozkoumejte [Referenci API](api-reference.md)
- 🚀 Podívejte se na [Průvodce rychlým startem](getting-started.md)
