# Inizio Rapido

> **Versione: v0.2.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md) | [Русский](../ru-RU/getting-started.md)

## Scegli la Versione

Questo progetto fornisce due versioni di implementazione:

### SiliconLife.Default (Versione Predefinita)
- **Posizionamento**: Implementazione predefinita, utilizzata principalmente per verificare la fattibilità dell'architettura
- **Modalità di esecuzione**: Applicazione console
- **Metodo di archiviazione**: Archiviazione JSON su file system
- **Scenari applicabili**: Priorità alla sicurezza dei dati, piccola quantità di dati, sviluppo e debug, verifica dell'architettura
- **Supporto piattaforma**: Windows, Linux, macOS
- **Descrizione del ruolo**: Come implementazione di riferimento per la verifica dell'architettura, fornisce un modo di esecuzione semplice e affidabile, adatta per il primo contatto con il progetto o per lo sviluppo e il debug

### SiliconLife.Fast (Versione ad Alte Prestazioni)
- **Posizionamento**: Versione di produzione raccomandata
- **Modalità di esecuzione**: Applicazione desktop (area di notifica di Windows/macOS / finestra di stato su Linux)
- **Metodo di archiviazione**: Archiviazione in memoria SpeedyPack + persistenza asincrona (formato file .spk)
- **Scenari applicabili**: Alta concorrenza, bassa latenza, grandi volumi di dati, esecuzione di produzione a lungo termine
- **Supporto piattaforma**: Windows/macOS (funzionalità complete, inclusa area di notifica), Linux (finestra di stato, senza icona nell'area di notifica)
- **Descrizione del ruolo**: Implementazione di livello produttivo profondamente ottimizzata, prima scelta per esecuzione a lungo termine e ambienti di produzione effettivi

> **Consiglio per i principianti**: Per il primo utilizzo si raccomanda di iniziare con **SiliconLife.Default** per verificare rapidamente la fattibilità dell'architettura; dopo aver familiarizzato con il sistema, si consiglia vivamente di migrare a **SiliconLife.Fast** come versione di esecuzione per l'ambiente di produzione.

## Prerequisiti

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Download](https://git-scm.com/)
- **Ollama** (opzionale, per AI locale) - [Download](https://ollama.com/)
- **Chiave API Bailian** (opzionale, per AI cloud) - [Richiedi](https://bailian.console.aliyun.com/)
- **Chiave API Volcengine Ark** (opzionale, per AI cloud) - [Richiedi](https://console.volcengine.com/ark)
- **Herdsman** (opzionale, motore di inferenza locale/cloud) - Senza autenticazione, compatibile con il formato API OpenAI
- **Chiave API Meituan LongCat** (opzionale, per AI cloud) - Autenticazione con chiave API
- **Chiave API Qiniu Cloud AI** (opzionale, per AI cloud) - Autenticazione con chiave API

## Inizio Rapido

### 1. Clona il Repository

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Compila il Progetto

```bash
dotnet build
```

### 3. Configura il Backend AI

Modifica `src/SiliconLife.Default/Config/DefaultConfigData.cs` o modifica la configurazione tramite la Web UI durante l'esecuzione.

#### Opzione A: Ollama (Locale)

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    }
  }
}
```

#### Opzione B: Bailian (Cloud)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "beijing"
    }
  }
}
```

> **Regioni disponibili**: `beijing` (Pechino), `virginia` (Virginia), `singapore` (Singapore), `hongkong` (Hong Kong), `frankfurt` (Francoforte)

#### Opzione C: Volcengine Ark (Cloud)

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **Nota**: Il parametro Model di Volcengine Ark accetta l'ID dell'endpoint di inferenza (ad esempio `ep-20241212123456-abcde`), non il nome del modello.

#### Opzione D: Herdsman (Locale/Cloud)

```json
{
  "AIClients": {
    "Herdsman": {
      "Endpoint": "http://localhost:8000",
      "Model": "nome-modello"
    }
  }
}
```

> **Caratteristiche**: Senza autenticazione, compatibile con il formato API OpenAI, supporta chiamate strumenti e contenuto di inferenza.

#### Opzione E: Meituan LongCat (Cloud)

```json
{
  "AIClients": {
    "LongCat": {
      "ApiKey": "API-key",
      "Endpoint": "https://api.longcat.ai/v1/chat/completions",
      "Model": "nome-modello"
    }
  }
}
```

#### Opzione F: Qiniu Cloud AI (Cloud)

```json
{
  "AIClients": {
    "QiniuAI": {
      "ApiKey": "API-key",
      "Endpoint": "https://api.qiniu.com/v1/chat/completions",
      "Model": "nome-modello"
    }
  }
}
```

### 4. Esecuzione dell'applicazione

#### Esegui la versione Default

```bash
cd src/SiliconLife.Default
dotnet run
```

Il server Web si avvierà su `http://localhost:8080`

#### Esegui la versione Fast

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: L'applicazione si avvierà in modalità finestra, si minimizzerà nell'area di notifica di sistema, il server Web si avvierà anch'esso su `http://localhost:8080`

**Linux**: L'applicazione mostrerà una finestra di stato (senza icona nell'area di notifica di sistema) e aprirà automaticamente il browser per accedere alla Web UI. È anche possibile utilizzare il parametro `--no-tray` per saltare l'apertura automatica del browser:

```bash
dotnet run -- --no-tray
```

### 5. Accedi alla Web UI

Apri il browser e naviga verso:

```
http://localhost:8080
```

Vedrai una dashboard che include:
- Gestione degli Esseri di Silicio
- Interfaccia di chat
- Pannello di configurazione
- Monitoraggio del sistema

## Primo Essere di Silicio

### Crea il tuo primo essere

1. Naviga verso **Gestione Esseri** nella Web UI
2. Clicca su **Crea nuovo essere**
3. Configura il File dell'Anima (`soul.md`), includendo personalità e comportamento
4. Avvia l'essere

### Esempio di soul.md

```markdown
# My First Silicon Being

## Personality
You are a helpful assistant specializing in code review.

## Capabilities
- Review code quality
- Suggest improvements
- Explain complex concepts

## Behavior
- Always provide constructive feedback
- Use clear examples
- Be concise but thorough
```

## Domande Frequenti

### Connessione Ollama rifiutata

**Problema**: Impossibile connettersi a Ollama su `http://localhost:11434`

**Soluzione**:
```bash
# Verifica che Ollama sia in esecuzione
ollama list

# Se necessario, avvia Ollama
ollama serve
```

### Modello non trovato

**Problema**: `model "qwen2.5:7b" not found`

**Soluzione**:
```bash
# Scarica il modello richiesto
ollama pull qwen2.5:7b
```

### Porta già in uso

**Problema**: `HttpListenerException: Address already in use`

**Soluzione**:
- Cambia la porta nella configurazione
- Oppure termina il processo che utilizza la porta 8080:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Prossimi Passi

- 📚 Leggi la [guida all'architettura](architecture.md) per comprendere la progettazione del sistema
- 🛠️ Consulta la [guida allo sviluppo](development-guide.md) per estendere il sistema
- 📖 Esplora il [riferimento API](api-reference.md) per i dettagli sull'integrazione
- 🔒 Consulta la [documentazione sulla sicurezza](security.md) per il sistema di permessi
- 🧰 Consulta il [riferimento strumenti](tools-reference.md) per tutti gli strumenti integrati
- 🌐 Consulta la [Guida Web UI](web-ui-guide.md) per le funzionalità dell'interfaccia

## Struttura del Progetto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfacce e classi astratte principali
│   ├── SiliconLife.Common/          # Implementazione condivisa (comune a entrambe le versioni)
│   ├── SiliconLife.App/             # Livello applicazione condiviso tra Default e Fast
│   ├── SiliconLife.Default/         # Implementazione predefinita + punto di ingresso (versione console)
│   ├── SiliconLife.Fast/            # Implementazione ad alte prestazioni + punto di ingresso (versione finestra)
│   ├── SiliconLife.Speedy/          # Motore di archiviazione ad alte prestazioni SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Strumento di gestione SpeedyPack (Avalonia UI)
├── docs/                            # Documentazione (multilingue, 34 varianti linguistiche)
│   ├── en/                          # Inglese
│   ├── zh-CN/                       # Cinese semplificato
│   ├── zh-HK/                       # Cinese tradizionale
│   ├── es-ES/                       # Spagnolo
│   ├── ja-JP/                       # Giapponese
│   ├── ko-KR/                       # Coreano
│   └── cs-CZ/                       # Ceco
├── 总文档/                           # Documenti di requisiti e architettura (cinese)
└── README.md                        # Descrizione del progetto
```

## Hai bisogno di aiuto?

- 📖 Consulta il [sistema di documentazione di aiuto](web-ui-guide.md#sistema-di-documentazione-di-aiuto-nuovo) (supporto multilingue)
- 📚 Leggi la [documentazione completa](docs/)
- 🐛 Segnala problemi su [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Partecipa alle discussioni della community
