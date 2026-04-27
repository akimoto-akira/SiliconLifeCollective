# Průvodce Řešením Problémů

> **Verze: v0.1.0-alpha**

[English](../en/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | **Čeština**

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
1. Zvyšte velikost haldy:
```bash
dotnet run --server.gcHeapCount 4
```

2. Vyčistěte stará data:
```bash
# Archivujte staré logy
mv logs/ logs-archive/
mkdir logs

# Vyčistěte staré vzpomínky
# Prostřednictvím Web UI: Správa Paměti > Vyčistit
```

---

### Problémy s Oprávněním

#### Problém: Operace Trvale Zamítnuta

**Příznaky**:
```
Permission Denied: disk:write
```

**Řešení**:
1. Zkontrolujte globální ACL:
```bash
curl http://localhost:8080/api/permissions
```

2. Ověřte stav IsCurator uživatele
3. Zkontroluje HighDeny cache
4. Zkontrolujte auditní logy:
```bash
curl http://localhost:8080/api/logs?level=error
```

#### Problém: Dotaz Uživatele se Nikdy Nezobrazí

**Příznaky**:
- Operace zůstává ve stavu "Čeká"
- Žádný prompt se nezobrazí

**Řešení**:
1. Ověřte, že IPermissionAskHandler je registrován
2. Zkontrolujte, že komunikační kanál je aktivní
3. Ověřte, že není překročen časový limit

---

### Problémy s Nástroji

#### Problém: Nástroj Nenalezen

**Příznaky**:
```
Tool 'calendar' not found
```

**Řešení**:
1. Zkontrolujte, že soubor nástroje je ve složce `Tools/`
2. Ověřte, že nástroj implementuje `ITool`
3. Znovu sestavte projekt:
```bash
dotnet build
```

4. Zkontrolujte logy pro chyby načítání:
```bash
grep -i "tool" logs/*.log
```

#### Problém: Nástroj Vrací Chybu

**Příznaky**:
```
Tool execution failed: Invalid parameters
```

**Řešení**:
1. Zkontrolujte formát parametrů
2. Ověřte, že všechny požadované parametry jsou přítomny
3. Zkontrolujte dokumentaci nástroje pro správné použití
4. Otestujte nástroj izolovaně

---

### Webové UI Problémy

#### Problém: Nelze se Připojit k Web UI

**Příznaky**:
- Prohlížeč zobrazuje "Nelze se připojit"
- Chyba CORS

**Řešení**:
1. Ověřte, že server běží
2. Zkontrolujte, že port 8080 není blokován firewallem
3. Ověřte konfiguraci serveru:
```bash
curl http://localhost:8080/api/status
```

#### Problém: SSE nefunguje

**Příznaky**:
- Chat se neaktualizuje v reálném čase
- Žádné streamované odpovědi

**Řešení**:
1. Zkontrolujte podporu prohlížeče pro Server-Sent Events
2. Ověřte, že žádný proxy server nebufferuje SSE
3. Zkontrolujte konzoli prohlížeče pro chyby
4. Ověřte, že server podporuje SSE

---

### Problémy s Kalendářem

#### Problém: Nesprávný Převod Data

**Příznaky**:
- Převedené datum je nesprávné
- Chybějící přestupné měsíce

**Řešení**:
1. Ověřte, že je zadán správný kalendářový systém
2. Zkontrolujte formát data (YYYY-MM-DD)
3. Ověřte, že datum existuje v cílovém kalendáři
4. Zkontrolujte logy pro varování

---

### Problémy s Pamětí

#### Problém: Bytost Zapomíná Kontext

**Příznaky**:
- Bytost si nepamatuje předchozí konverzace
- Ztráta kontextu mezi relacemi

**Řešení**:
1. Ověřte, že úložiště je přístupné
2. Zkontrolujte logy pro chyby čtení/zápisu
3. Ověřte, že paměť není poškozena:
```bash
# Prostřednictvím Web UI: Správa Paměti > Ověřit
```

4. Zvažte kompresi paměti pro starší záznamy

---

## Pokročilé Řešení Problémů

### Povolení Podrobného Logování

```bash
# Nastavte úroveň logování na Debug
export LOG_LEVEL=Debug

# Nebo v konfiguraci:
{
  "Logging": {
    "LogLevel": "Debug"
  }
}
```

### Analyzátor Výkonu

```bash
# Sledujte využití zdrojů
dotnet counters monitor --process-id <PID>

# Profilujte aplikaci
dotnet trace collect --process-id <PID>
```

### ladění Dynamické Kompilace

```csharp
// Povolte výstup kompilace
var compiler = new DynamicCompilationExecutor
{
    EmitDebugInformation = true,
    LogCompiledCode = true
};
```

### Kontrola Zdraví Systému

```bash
# Získejte stav systému
curl http://localhost:8080/api/status

# Získejte metriky dashboardu
curl http://localhost:8080/api/dashboard

# Zkontrolujte aktivní bytosti
curl http://localhost:8080/api/beings
```

---

## Získání Pomoci

### Logy

Logy jsou umístěny ve:
```
logs/
├── system.log
├── beings/
│   ├── {being-id-1}.log
│   └── {being-id-2}.log
└── audit.log
```

### Komunitní Podpora

- **GitHub Issues**: https://github.com/akimoto-akira/SiliconLifeCollective/issues
- **Diskuse**: GitHub Discussions
- **Dokumentace**: docs/ adresář

### Poskytování Informací o Chybě

Při hlášení problému uveďte:

1. **Verze**: `dotnet --version`
2. **OS**: Windows/Linux/Mac
3. **Logy**: Příslušné části logů
4. **Kroky k Reprodukci**: Jak problém reprodukovat
5. **Očekávané Chování**: Co by se mělo stát
6. **Skutečné Chování**: Co se skutečně stalo

---

## Další Kroky

- 📚 Přečtěte si [Průvodce Architektury](architecture.md)
- 🛠️ Podívejte se na [Vývojářskou Příručku](development-guide.md)
- 📖 Přečtěte si [Referenci API](api-reference.md)
- 🚀 Začněte s [Průvodcem Rychlým Startem](getting-started.md)
