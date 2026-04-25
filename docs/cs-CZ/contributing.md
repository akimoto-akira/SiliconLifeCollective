# Přispívání

[English](../en/contributing.md) | [中文文档](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md)

Děkujeme za váš zájem přispět do SiliconLifeCollective!

## Code of Conduct

Tento projekt následuje Apache 2.0 licenci. Ve všech interakcích buďte respektující a profesionální.

---

## Rychlý Start

### 1. Forkněte Repozitář

Klikněte na tlačítko "Fork" na GitHubu pro vytvoření vaší vlastní kopie.

### 2. Klonujte Váš Fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Nastavte Vývojové Prostředí

```bash
# Nainstalujte .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# Obnovte závislosti
dotnet restore

# Sestavte projekt
dotnet build

# Spusťte testy
dotnet test
```

### 4. Vytvořte Feature Branch

```bash
git checkout -b feature/nazev-vasi-funkce
```

---

## Vývojový Workflow

### Styl Kódu

- Následujte C# konvence kódování
- Používejte PascalCase pro názvy tříd
- Používejte camelCase pro parametry metod
- Používejte `_camelCase` pro privátní pole
- Všechna veřejná API musí mít XML dokumentaci

### Commit Zprávy

Následujte formát **Conventional Commits**:

```
<type>(<scope>): <description>
```

**Typy**:
- `feat`: Nová funkce
- `fix`: Oprava chyby
- `docs`: Změny dokumentace
- `style`: Formátování kódu
- `refactor`: Refaktoring kódu
- `test`: Změny testů
- `chore`: Změny build/nástrojů

**Příklady**:
```bash
feat(localization): přidání podpory korejského jazyka
fix(permission): oprava null pointer v callbacku
docs: aktualizace průvodce přispíváním
refactor(web): zjednodušení struktury controllerů
```

### Provádění Změn

1. **Pište kód**
   - Následujte existující patterny
   - Přidejte testy pro nové funkce
   - Aktualizujte dokumentaci

2. **Testujte vaše změny**
   ```bash
   # Spusťte všechny testy
   dotnet test
   
   # Build v release módu
   dotnet build --configuration Release
   ```

3. **Formátujte kód**
   ```bash
   dotnet format
   ```

4. **Commitněte změny**
   ```bash
   git add .
   git commit -m "feat(scope): popis"
   ```

5. **Pushněte na váš Fork**
   ```bash
   git push origin feature/nazev-vasi-funkce
   ```

6. **Vytvořte Pull Request**
   - Přejděte na originální repozitář
   - Klikněte na "Compare & pull request"
   - Vyplňte PR šablonu
   - Odešlete

---

## Pull Request Průvodce

### PR Název

Použijte stejný formát jako commit zprávy:
```
feat(localization): přidání podpory korejského jazyka
```

### PR Popis

Zahrňte:

1. **Co** - Co tento PR dělá?
2. **Proč** - Proč je tato změna potřebná?
3. **Jak** - Jak jste to implementovali?
4. **Testování** - Jak jste to testovali?

### Příklad PR Popisu

```markdown
## Co
Přidání korejské lokalizace pro všechny UI komponenty a dokumentaci.

## Proč
Rozšíření přístupnosti projektu pro korejské uživatele.

## Jak
- Vytvoření KoKR.cs lokalizačního souboru
- Přidání 500+ překladových klíčů
- Aktualizace všech pohledů pro použití lokalizace
- Vytvoření korejské dokumentace v docs/ko-KR/

## Testování
- Ověření, že všechny UI elementy správně zobrazují korejštinu
- Testování funkce přepínání jazyka
- Revize překladů s rodilým mluvčím
```

---

## Typy Příspěvků

### 1. Opravy Chyb

**Workflow**:
1. Zkontrolujte existující issues
2. Vytvořte issue pokud neexistuje
3. Opravte chybu
4. Přidejte testovací případy
5. Odešlete PR

**Požadavky**:
- Jasný popis chyby
- Kroky pro reprodukci
- Testy proti regresi

### 2. Nové Funkce

**Workflow**:
1. Diskutujte o funkci v Issues/Discussions
2. Získejte schválení od maintainerů
3. Implementujte funkci
4. Přidejte komplexní testy
5. Aktualizujte dokumentaci
6. Odešlete PR

**Požadavky**:
- Schválený návrh funkce
- Kompletní pokrytí testy
- Aktualizovaná dokumentace
- Zpětně kompatibilní

### 3. Dokumentace

**Workflow**:
1. Identifikujte mezery v dokumentaci
2. Napište/aktualizujte dokumentaci
3. Odešlete PR

**Požadavky**:
- Jasné a stručné
- Zahrnující příklady
- Vícejazyčné pokud je to vhodné

### 4. Refaktoring Kódu

**Workflow**:
1. Navrhněte refaktoring v Issue
2. Získejte schválení
3. Refaktorujte kód
4. Ujistěte se, že všechny testy procházejí
5. Odešlete PR

**Požadavky**:
- Žádné funkční změny
- Všechny testy procházejí
- Zlepšení kvality kódu
- Jasné vysvětlení

---

## Testovací Průvodce

### Unit Testy

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // Arrange
    var service = new MyService();
    
    // Act
    var result = service.DoSomething();
    
    // Assert
    Assert.IsTrue(result.Success);
}
```

### Integrační Testy

Testujte kompletní workflow:
- AI interakce
- Provádění nástrojů
- Ověřování oprávnění
- Operace úložiště

### Manuální Testy

Pro UI změny:
- Testujte ve více prohlížečích
- Ověřte responzivní design
- Zkontrolujte přístupnost

---

## Průvodce Dokumentací

### Komentáře Kódu

- XML komentáře pro všechna veřejná API
- Inline komentáře pro komplexní logiku
- Angličtina pro komentáře kódu

### Dokumentační Soubory

- Umístěte do `docs/{language}/`
- Aktualizujte všechny jazykové verze
- Následujte existující strukturu

### Vícejazyčná Dokumentace

Při přidávání dokumentace:
1. Nejprve vytvořte anglickou verzi
2. Přeložte do ostatních jazyků
3. Udržujte obsah synchronizovaný

---

## Review Proces

### Co Maintaineři Kontrolují

1. **Kvalita Kódu**
   - Následuje konvence
   - Čitelný a čistý
   - Dobře zdokumentovaný

2. **Testy**
   - Dobře pokryté
   - Všechny testy procházejí
   - Coverage hraničních případů

3. **Dokumentace**
   - Aktualizovaná
   - Jasně vysvětlená
   - Vícejazyčná

4. **Kompatibilita**
   - Zpětně kompatibilní
   - Žádné breaking changes (pokud nejsou oznámeny)
   - Následuje semantic versioning

### Review Timeline

- Počáteční review: 1-3 dny
- Implementace feedbacku: dle potřeby
- Merge: po schválení

---

## FAQ

### PR Zamítnuto

**Důvody**:
- Následování průvodců
- Nedostatečné testy
- Neoznámené breaking changes
- Špatná kvalita kódu

**Řešení**:
- Adresujte feedback
- Aktualizujte PR
- Odešlete znovu

### Merge Konflikty

**Řešení**:
```bash
# Aktualizujte vaši branch
git fetch origin
git rebase origin/master

# Vyřešte konflikty
# Upravte konfliktní soubory
git add .
git rebase --continue

# Force push
git push --force-with-lease
```

---

## Získání Pomoci

### Zdroje

- **Dokumentace**: [docs/](../)
- **Issues**: GitHub Issues
- **Diskuse**: GitHub Discussions
- **Code of Conduct**: CODE_OF_CONDUCT.md

### Kontakt

- Vytvořte Issue pro chyby
- Spusťte Discussion pro otázky
- Označte maintainery pro urgentní záležitosti

---

## Poděkování

Přispěvatelé budou uznáni na:
- Sekci přispěvatelů v README.md
- Release notes
- Projektové dokumentaci

---

## Licence

Přispíváním souhlasíte, že váš příspěvek bude licencován pod Apache 2.0 licencí.

---

## Další Kroky

- 📚 Přečtěte si [Dokumentaci](../)
- 🐛 Podívejte se na [Otevřené Issues](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Spusťte [Diskusi](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Forkněte a začněte přispívat!

Děkujeme za váš příspěvek do SiliconLifeCollective! 🎉
