# Přispívání

> **Verze: v0.1.0-alpha**

[English](../en/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Deutsch](../de-DE/contributing.md) | **Čeština**

Děkujeme vám za zájem přispět do SiliconLifeCollective!

## Kodex Chování

Tento projekt následuje licenci Apache 2.0. Ve všech interakcích buďte respektující a profesionální.

---

## Rychlý Start

### 1. Fork Repozitáře

Klikněte na tlačítko "Fork" na GitHubu pro vytvoření vlastní kopie.

### 2. Naklonujte Váš Fork

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

### 4. Vytvořte Větev Funkce

```bash
git checkout -b feature/vase-nazev-funkce
```

---

## Vývojový Workflow

### Styl Kódu

- Dodržujte konvence kódování C#
- Názvy tříd používejte PascalCase
- Parametry metod používejte camelCase
- Soukromá pole používejte `_camelCase`
- Všechna veřejná API musí mít XML dokumentaci

### Zprávy o Commitu

Následujte formát **Conventional Commits**:

```
<typ>(<rozsah>): <popis>
```

**Typy**:
- `feat`: Nová funkce
- `fix`: Oprava chyby
- `docs`: Změny dokumentace
- `style`: Formátování kódu
- `refactor`: Refaktorizace kódu
- `test`: Změny testů
- `chore`: Změny sestavení/nástrojů

**Příklady**:
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### Provádění Změn

1. **Pište kód**
   - Dodržujte existující vzory
   - Přidejte testy pro nové funkce
   - Aktualizujte dokumentaci

2. **Otestujte své změny**
   ```bash
   # Spusťte všechny testy
   dotnet test
   
   # Sestavte v režimu Release
   dotnet build --configuration Release
   ```

3. **Formátujte kód**
   ```bash
   dotnet format
   ```

4. **Commitujte změny**
   ```bash
   git add .
   git commit -m "feat(rozsah): popis"
   ```

5. **Pushněte na váš Fork**
   ```bash
   git push origin feature/vase-nazev-funkce
   ```

6. **Vytvořte Pull Request**
   - Přejděte na původní repozitář
   - Klikněte na "Compare & pull request"
   - Vyplňte šablonu PR
   - Odešlete

---

## Průvodce Pull Requesty

### Název PR

Použijte stejný formát jako zprávy commitu:
```
feat(localization): add Korean language support
```

### Popis PR

Zahrňte:

1. **Co** - Co tento PR dělá?
2. **Proč** - Proč je tato změna potřebná?
3. **Jak** - Jak jste to implementovali?
4. **Testování** - Jak jste to testovali?

### Příklad Popisu PR

```markdown
## Co
Přidání korejské lokalizace pro všechny UI komponenty a dokumentaci.

## Proč
Rozšíření přístupnosti projektu pro korejské uživatele.

## Jak
- Vytvořil KoKR lokalizační soubory
- Aktualizoval všechny UI řetězce
- Přeložil dokumentaci
- Přidal testy

## Testování
- Ručně ověřeno v Web UI
- Spuštěny všechny existující testy
- Otestováno přepínání jazyků
```

---

## Směrnice pro Kód

### Struktura Projektu

```
src/
├── SiliconLife.Core/      # Rozhraní a abstrakce
│   ├── AI/                # AI klientská rozhraní
│   ├── Chat/              # Systém chatu
│   ├── Config/            # Konfigurační modely
│   ├── Tools/             # Rozhraní nástrojů
│   └── ...
└── SiliconLife.Default/   # Konkrétní implementace
    ├── AI/                # AI klienti
    ├── Tools/             # Implementace nástrojů
    ├── Web/               # Web UI
    └── ...
```

### Přidávání Nových Funkcí

1. **Rozhraní nejprve v Core**
   ```csharp
   // SiliconLife.Core/INovaFunkce.cs
   public interface INovaFunkce
   {
       Task<string> ProveďAsync(string vstup);
   }
   ```

2. **Implementace v Default**
   ```csharp
   // SiliconLife.Default/NovaFunkce.cs
   public class NovaFunkce : INovaFunkce
   {
       public async Task<string> ProveďAsync(string vstup)
       {
           // Implementace
       }
   }
   ```

3. **Přidejte testy**
   ```csharp
   [Fact]
   public async Task ProveďAsync_VraciOcekavanyVysledek()
   {
       // Test
   }
   ```

4. **Aktualizujte dokumentaci**

---

## Přidávání Nových Nástrojů

1. Vytvořte nový soubor v `src/SiliconLife.Default/Tools/`:

```csharp
public class MujVlastniNastroj : ITool
{
    public string Name => "muj_nastroj";
    public string Description => "Popis toho, co tento nástroj dělá";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Logika nástroje
        return new ToolResult { Success = true, Output = "výsledek" };
    }
}
```

2. Nástroj je automaticky objeven prostřednictvím reflexe.

3. (Volitelné) Označte jako pouze pro kurátora:
```csharp
[SiliconManagerOnly]
public class AdminNastroj : ITool { ... }
```

---

## Přidávání Nových Jazyků

1. Vytvořte nový adresář v `docs/`:
```bash
mkdir docs/fr-FR
```

2. Zkopírujte existující dokumenty:
```bash
cp docs/en/* docs/fr-FR/
```

3. Přeložte všechny dokumenty

4. Aktualizujte odkazy na dokumenty v každém souboru

5. Přidejte lokalizaci Web UI:
```csharp
// SiliconLife.Default/Localization/FrFR.cs
public class FrFR : LocalizationBase
{
    // Implementace
}
```

---

## Přidávání Nových Skinů

1. Vytvořte nový soubor v `src/SiliconLife.Default/Web/Skins/`:

```csharp
public class MujSkin : ISkin
{
    public string Name => "MujSkin";
    public string Description => "Popis skinu";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #barva;
            }
        ";
    }
}
```

2. Skin je automaticky objeven Správcem skinů.

---

## Testování

### Spouštění Testů

```bash
# Všechny testy
dotnet test

# Konkrétní testovací projekt
dotnet test tests/SiliconLife.Core.Tests/

# S detailním výstupem
dotnet test --logger "console;verbosity=detailed"
```

### Psaní Testů

```csharp
public class MujNastrojTests
{
    [Fact]
    public async Task ExecuteAsync_PlatnyVstup_VraciSuccess()
    {
        // Arrange
        var tool = new MujNastroj();
        var call = new ToolCall { Parameters = new Dictionary<string, object>() };
        
        // Act
        var result = await tool.ExecuteAsync(call);
        
        // Assert
        Assert.True(result.Success);
    }
}
```

---

## Dokumentace

### Aktualizace Dokumentace

Při přidávání nových funkcí:

1. **Aktualizujte relevantní .md soubory** v `docs/`
2. **Přidejte příklady** použití
3. **Dokumentujte API** změny
4. **Aktualizujte všechny jazykové verze** (nebo označte k překladu)

### Styl Dokumentace

- Používejte Markdown formátování
- Zahrňte příklady kódu
- Popište parametry a návratové hodnoty
- Uveďte případy použití

---

## Proces Revize

### Co Hledáme

- **Funkčnost**: Kód dělá to, co má?
- **Kvalita**: Dodržuje standardy kódování?
- **Testy**: Jsou zahrnuty testy?
- **Dokumentace**: Je aktualizována dokumentace?
- **Výkon**: Žádné zbytečné režie?
- **Bezpečnost**: Žádné bezpečnostní problémy?

### Čas Revize

- Většina PR je revizována do 48 hodin
- Složité změny mohou trvat déle
- Buďte trpěliví a respektujte čas recenzentů

---

## Časté Otázky

### Mohu přispět bez znalosti C#?

Ano! Můžete pomoci s:
- Překlady dokumentace
- Testování a hlášení chyb
- Návrhy funkcí
- Vylepšení dokumentace

### Jak dlouho trvá schválení PR?

Obvykle 1-3 dny pro jednoduché změny, déle pro komplexní funkce.

### Mohu přispět k existující funkci?

Ano! Otevřete issue pro diskusi o navrhovaných změnách.

### Jak nahlásit chybu?

1. Vyhledejte existující issues
2. Pokud nenalezeno, vytvořte nové issue
3. Zahrňte kroky k reprodukci
4. Připojte relevantní logy

---

## Licence

Přispíváním souhlasíte s tím, že vaše příspěvky budou licencovány pod Apache 2.0.

---

## Poděkování

Děkujeme všem přispěvatelům, kteří pomáhají zlepšovat SiliconLifeCollective!

**Začněte přispívat ještě dnes!** 🚀
