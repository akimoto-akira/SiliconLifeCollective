# Příručka přispívání

> **Verze: v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | **Čeština** | [Русский](../ru-RU/contributing.md)

Děkujeme za váš zájem přispět do SiliconLifeCollective!

## Přispívání do duálních verzí

Tento projekt má dvě implementační verze, můžete si vybrat směr příspěvku podle zájmu:

### SiliconLife.Default (výchozí verze)
- **Technologický stack**: .NET 9 konzolová aplikace
- **Směr příspěvku**: vývoj základních funkcí, implementace nástrojů, lokalizace, dokumentace
- **Vhodné pro**: všechny vývojáře

### SiliconLife.Fast (vysoce výkonná verze)
- **Technologický stack**: .NET 9 multiplatformní desktopová aplikace (Avalonia UI)
- **Směr příspěvku**: optimalizace výkonu, SpeedyPack úložiště, systémová lišta, bezzámková souběžnost
- **Vhodné pro**: vývojáře se zkušenostmi s desktopovým vývojem a zájmem o optimalizaci výkonu

> **Důležité upozornění**: Obě verze sdílejí projekty SiliconLife.Core a SiliconLife.Common, vylepšení základních rozhraní ovlivní obě verze současně.

## Kodex chování

Tento projekt se řídí licencí Apache 2.0. V všech interakcích zachovávejte respekt a profesionalitu.

---

## Rychlý start

### 1. Forkněte repozitář

Klikněte na tlačítko "Fork" na GitHubu pro vytvoření vlastní kopie.

### 2. Klonujte svůj Fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Nastavte vývojové prostředí

```bash
# Instalace .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# Obnovení závislostí
dotnet restore

# Sestavení projektu
dotnet build

# Spuštění testů
dotnet test
```

### 4. Vytvořte větev pro funkci

```bash
git checkout -b feature/your-feature-name
```

### 5. Vyberte vývojový projekt

Podle typu vašeho příspěvku vyberte vhodný projekt:

- **Základní rozhraní/abstraktní třídy** → upravte `SiliconLife.Core`
- **Sdílená implementace** → upravte `SiliconLife.Common`
- **Specifické pro verzi Default** → upravte `SiliconLife.Default`
- **Specifické pro verzi Fast** → upravte `SiliconLife.Fast`
- **Úložný engine** → upravte `SiliconLife.Speedy`
- **Správa úložiště** → upravte `SiliconLife.Speedy.Manager`
- **Vývoj zásuvných modulů** → upravte `SiliconLife.Core/Plugins`
- **Vícejazyčná dokumentace** → upravte adresář `docs/`

---

## Vývojový pracovní postup

### Styl kódu

- Dodržujte konvence C#
- Názvy tříd používejte v PascalCase
- Parametry metod používejte v camelCase
- Soukromá pole používejte s předponou `_camelCase`
- Všechna veřejná API musí mít XML dokumentaci

### Commit zprávy

Dodržujte formát **konvenčních commitů**:

```
<typ>(<rozsah>): <popis>
```

**Typy**:
- `feat`: Nová funkce
- `fix`: Oprava chyby
- `docs`: Změna dokumentace
- `style`: Formátování kódu
- `refactor`: Refaktoring kódu
- `test`: Změna testů
- `chore`: Změna sestavení/nástrojů

**Příklady**:
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### Provádění změn

1. **Psaní kódu**
   - Dodržujte existující vzory
   - Přidejte testy pro nové funkce
   - Aktualizujte dokumentaci

2. **Testování změn**
   ```bash
   # Spuštění všech testů
   dotnet test
   
   # Sestavení v režimu Release
   dotnet build --configuration Release
   ```

3. **Formátování kódu**
   ```bash
   dotnet format
   ```

4. **Potvrzení změn**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

5. **Push do vašeho Forku**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Vytvoření Pull Requestu**
   - Přejděte na původní repozitář
   - Klikněte na "Compare & pull request"
   - Vyplňte šablonu PR
   - Odešlete

---

## Průvodce Pull Requesty

### Název PR

Použijte stejný formát jako pro commit zprávy:
```
feat(localization): add Korean language support
```

### Popis PR

Zahrňte:

1. **Co** - Co tento PR dělá?
2. **Proč** - Proč je tato změna potřebná?
3. **Jak** - Jak jste to implementovali?
4. **Testování** - Jak jste to testovali?

### Příklad popisu PR

```markdown
## Co
Přidána korejská lokalizace pro všechny UI komponenty a dokumentaci.

## Proč
Rozšíření přístupnosti projektu pro korejské uživatele.

## Jak
- Vytvořen soubor KoKR.cs s lokalizací
- Přidáno 500+ překladových klíčů
- Aktualizovány všechny pohledy pro použití lokalizace
- Vytvořena korejská dokumentace v docs/ko-KR/

## Testování
- Ověřeno, že všechny UI prvky správně zobrazují korejštinu
- Testována funkce přepínání jazyků
- Překlady zkontrolovány s rodilým mluvčím
```

---

## Typy příspěvků

### 1. Opravy chyb

**Postup**:
1. Zkontrolujte existující Issues
2. Pokud neexistuje, vytvořte Issue
3. Opravte chybu
4. Přidejte testovací případ
5. Odešlete PR

**Požadavky**:
- Jasný popis chyby
- Kroky k reprodukci
- Test zabraňující regresi

### 2. Nové funkce

**Postup**:
1. Prodiskutujte funkci v Issues/Discussions
2. Získejte schválení od správce
3. Implementujte funkci
4. Přidejte komplexní testy
5. Aktualizujte dokumentaci
6. Odešlete PR

**Požadavky**:
- Návrh funkce schválen
- Kompletní testovací pokrytí
- Dokumentace aktualizována
- Zpětná kompatibilita

### 3. Dokumentace

**Postup**:
1. Identifikujte mezery v dokumentaci
2. Napište/aktualizujte dokumentaci
3. Odešlete PR

**Požadavky**:
- Jasné a stručné
- Včetně příkladů
- Podpora více jazyků (pokud applicable)

### 4. Refaktoring kódu

**Postup**:
1. Navrhněte refaktoring v Issue
2. Získejte schválení
3. Refaktorujte kód
4. Ujistěte se, že všechny testy procházejí
5. Odešlete PR

**Požadavky**:
- Žádná změna funkcionality
- Všechny testy procházejí
- Zlepšení kvality kódu
- Jasné vysvětlení

---

## Průvodce testováním

### Jednotkové testy

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // Uspořádání
    var service = new MyService();
    
    // Provedení
    var result = service.DoSomething();
    
    // Tvrzení
    Assert.IsTrue(result.Success);
}
```

### Integrační testy

Testování kompletních pracovních postupů:
- AI interakce
- Provádění nástrojů
- Ověřování oprávnění
- Operace úložiště

### Manuální testování

Pro změny UI:
- Testujte ve více prohlížečích
- Ověřte responzivní design
- Zkontrolujte přístupnost

---

## Průvodce dokumentací

### Komentáře v kódu

- Pro veškerá veřejná API používejte XML komentáře
- Pro složitou logiku používejte inline komentáře
- Komentáře v kódu používejte v angličtině

### Dokumentační soubory

- Umístěte do `docs/{language}/`
- Aktualizujte všechny jazykové verze
- Dodržujte existující strukturu

### Vícejazyčná dokumentace

Při přidávání dokumentace:
1. Nejprve vytvořte anglickou verzi
2. Přeložte do dalších jazyků
3. Udržujte obsah synchronizovaný

---

## Proces revize

### Na co se správci zaměřují

1. **Kvalita kódu**
   - Dodržování konvencí
   - Srozumitelnost a čitelnost
   - Dokumentace

2. **Testování**
   - Dostatečné pokrytí
   - Všechny testy procházejí
   - Pokrytí okrajových případů

3. **Dokumentace**
   - Aktualizována
   - Jasné vysvětlení
   - Vícejazyčná

4. **Kompatibilita**
   - Zpětně kompatibilní
   - Žádné breaking changes (pokud není oznámeno)
   - Dodržování sémantického verzování

### Časová osa revize

- Počáteční revize: 1-3 dny
- Integrace zpětné vazby: dle potřeby
- Sloučení: po schválení

---

## Časté otázky

### PR zamítnut

**Důvody**:
- Nedodržení pokynů
- Nedostatečné testování
- Neoznámené breaking changes
- Špatná kvalita kódu

**Řešení**:
- Řešte zpětnou vazbu
- Aktualizujte PR
- Znovu odešlete

### Konflikty při sloučení

**Řešení**:
```bash
# Aktualizace vaší větve
git fetch origin
git rebase origin/master

# Řešení konfliktů
# Upravte konfliktní soubory
git add .
git rebase --continue

# Force push
git push --force-with-lease
```

---

## Získání pomoci

### Zdroje

- **Dokumentace**: [docs/](../)
- **Problémy**: GitHub Issues
- **Diskuse**: GitHub Discussions
- **Kodex chování**: CODE_OF_CONDUCT.md

### Kontakt

- Vytvořte Issue pro chyby
- Zahajte Discussion pro dotazy
- Označte správce pro naléhavé záležitosti

---

## Poděkování

Přispěvatelé budou uznáni v:
- Sekci přispěvatelů v README.md
- Poznámkách k vydání
- Projektové dokumentaci

---

## Licence

Příspěvkem souhlasíte, že váš příspěvek bude licencován pod licencí Apache 2.0.

---

## Další kroky

- 📚 Přečtěte [dokumentaci](../)
- 🐛 Prohlédněte [otevřené Issues](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Zahajte [diskusi](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Forkněte a začněte přispívat!

Děkujeme za přispění do SiliconLifeCollective! 🎉
