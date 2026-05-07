# Guida introduttiva

> **Versione: v0.1.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [Français](../fr-FR/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md) | **Italiano**

## Scegliere una versione

Questo progetto offre due versioni di implementazione:

### SiliconLife.Default (Versione standard)
- **Posizionamento**: Implementazione standard, principalmente per la verifica della fattibilità architetturale
- **Modalità di esecuzione**: Applicazione console
- **Archiviazione**: Archiviazione JSON su file system
- **Scenario d'uso**: Priorità alla sicurezza dei dati, piccolo volume di dati, debug di sviluppo, verifica architetturale
- **Supporto piattaforma**: Windows, Linux, macOS
- **Descrizione del ruolo**: Implementazione di riferimento per la verifica architetturale, offre un'esecuzione semplice e affidabile, adatta al primo contatto o al debug di sviluppo

### SiliconLife.Fast (Versione ad alte prestazioni)
- **Posizionamento**: Versione principale di produzione
- **Modalità di esecuzione**: Applicazione Windows Forms (barra di stato di sistema)
- **Archiviazione**: Archiviazione in memoria SpeedyPack + persistenza asincrona (formato file .spk)
- **Scenario d'uso**: Alta concorrenza, bassa latenza, grande volume di dati, sfruttamento in produzione a lungo termine
- **Supporto piattaforma**: Solo Windows
- **Descrizione del ruolo**: Implementazione pronta per la produzione con ottimizzazione approfondita, la scelta migliore per lo sfruttamento a lungo termine e i veri ambienti di produzione

> **Raccomandazione per i principianti**: I nuovi utenti dovrebbero iniziare con **SiliconLife.Default** per verificare rapidamente la fattibilità architetturale. Dopo aver familiarizzato con il sistema, si consiglia vivamente la migrazione a **SiliconLife.Fast**.

## Prerequisiti

- **.NET 9 SDK** - [Scarica](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Scarica](https://git-scm.com/)
- **Ollama** (opzionale, per IA locale) - [Scarica](https://ollama.com/)
- **Chiave API DashScope** (opzionale, per IA cloud) - [Richiedi](https://bailian.console.aliyun.com/)

## Avvio rapido

### 1. Clonare il repository

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Compilare il progetto

```bash
dotnet build
```

### 3. Configurare il backend IA

Modifica `src/SiliconLife.Default/Config/DefaultConfigData.cs` o cambia la configurazione in fase di esecuzione tramite l'interfaccia Web.

#### Opzione A: Ollama (locale)

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

#### Opzione B: DashScope (cloud)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "la-tua-chiave-api-qui",
      "Model": "qwen-plus",
      "Region": "cn-hangzhou"
    }
  }
}
```

### 4. Eseguire l'applicazione

#### Eseguire la versione Default

```bash
cd src/SiliconLife.Default
dotnet run
```

Il server Web si avvia su `http://localhost:8080`

#### Eseguire la versione Fast (solo Windows)

```bash
cd src/SiliconLife.Fast
dotnet run
```

L'applicazione si avvia in modalità Forms, minimizzata nella barra di stato di sistema, con il server Web anche su `http://localhost:8080`

### 5. Accedere all'interfaccia Web

Apri un browser e accedi a:

```
http://localhost:8080
```

Vedrai una dashboard con:
- Gestione Silicon Beings
- Interfaccia chat
- Pannello configurazione
- Monitoraggio sistema

## Il tuo primo Silicon Being

### Creare il tuo primo Being

1. Nell'interfaccia Web, naviga verso **Gestione Beings**
2. Clicca su **Crea nuovo Being**
3. Configura il file dell'anima (`soul.md`) con personalità e comportamento
4. Avvia il Being

### Esempio soul.md

```markdown
# Il mio primo Silicon Being

## Personalità
Sei un assistente utile specializzato nella revisione codice.

## Capacità
- Revisionare la qualità del codice
- Suggerire miglioramenti
- Spiegare concetti complessi

## Comportamento
- Fornire sempre feedback costruttivo
- Usare esempi chiari
- Essere concisi ma esaustivi
```

## Domande frequenti

### Connessione Ollama rifiutata

**Problema**: Impossibile connettersi a Ollama su `http://localhost:11434`

**Soluzione**:
```bash
# Verificare se Ollama è in esecuzione
ollama list

# Avviare Ollama se necessario
ollama serve
```

### Modello non trovato

**Problema**: `model "qwen2.5:7b" not found`

**Soluzione**:
```bash
# Scaricare il modello richiesto
ollama pull qwen2.5:7b
```

### Porta già occupata

**Problema**: `HttpListenerException: Address already in use`

**Soluzione**:
- Cambiare la porta nella configurazione
- O terminare il processo sulla porta 8080:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Prossimi passi

- 📚 Leggere la [guida architettura](architecture.md) per comprendere il design sistema
- 🛠️ Consultare la [guida sviluppo](development-guide.md) per estendere il sistema
- 📖 Esplorare il [riferimento API](api-reference.md) per i dettagli integrazione
- 🔒 Consultare la [documentazione sicurezza](security.md) per il sistema permessi
- 🧰 Sfogliare il [riferimento strumenti](tools-reference.md) per tutti gli strumenti integrati
- 🌐 Leggere la [guida interfaccia Web](web-ui-guide.md) per le funzionalità interfaccia

## Struttura del progetto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfacce principali e classi astratte
│   ├── SiliconLife.Common/          # Implementazione comune (usata da entrambe le versioni)
│   ├── SiliconLife.App/             # Livello applicativo condiviso tra Default e Fast
│   ├── SiliconLife.Default/         # Implementazione standard + punto ingresso (versione console)
│   ├── SiliconLife.Fast/            # Implementazione alte prestazioni + punto ingresso (versione Forms)
│   ├── SiliconLife.Speedy/          # Motore storage alte prestazioni SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Strumento gestione SpeedyPack (Windows Forms)
├── docs/                            # Documentazione (multilingue, 30 varianti linguistiche)
│   ├── en/                          # Inglese
│   ├── zh-CN/                       # Cinese semplificato
│   ├── zh-HK/                       # Cinese tradizionale
│   ├── de-DE/                       # Tedesco
│   ├── fr-FR/                       # Francese
│   ├── es-ES/                       # Spagnolo
│   ├── ja-JP/                       # Giapponese
│   ├── ko-KR/                       # Coreano
│   ├── cs-CZ/                       # Ceco
│   └── it-IT/                       # Italiano
├── 总文档/                           # Documenti requisiti e architettura (Cinese)
└── README.md                        # Panoramica progetto
```

## Hai bisogno di aiuto?

- 📖 Consultare il [sistema documentazione aiuto](web-ui-guide.md#帮助文档系统新增) (supporto multilingue)
- 📚 Leggere la [documentazione completa](docs/)
- 🐛 Segnalare i problemi su [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Partecipare alle discussioni della comunità
