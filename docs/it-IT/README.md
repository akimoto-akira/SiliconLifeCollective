![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Versione: v0.2.0-alpha** | **Silicon Life Collective** — Una piattaforma di collaborazione multi-agente basata su .NET 9, dove gli agenti AI sono chiamati **Esseri di Silicio**, che realizzano l'auto-evoluzione attraverso la compilazione dinamica Roslyn.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | **Italiano** | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md) | [Русский](../ru-RU/README.md)

## 🌟 Caratteristiche Principali

### Sistema di Agenti
- **Orchestrazione multi-agente** — Gestito uniformemente dal *Curatore di Silicio*, utilizza un meccanismo di pianificazione equo basato su time-slot guidato da clock
- **Guidato dal File dell'Anima** — Ogni Essere di Silicio è guidato da un file di prompt principale (`soul.md`), che definisce personalità e modelli comportamentali unici
- **Architettura Corpo-Cervello** — Il *Corpo* (SiliconBeing) mantiene i segni vitali e rileva gli scenari di attivazione; il *Cervello* (ContextManager) è responsabile del caricamento della cronologia, della chiamata all'AI, dell'esecuzione degli strumenti e della persistenza delle risposte
- **Capacità di auto-evoluzione** — Attraverso la tecnologia di compilazione dinamica Roslyn, gli Esseri di Silicio possono riscrivere il proprio codice per realizzare l'evoluzione
- **Gestione dello stato di attività** — Supporta nove stati di attività: Idle (inattivo), SingleChat (chat 1:1), GroupChat (chat di gruppo), Task (attività), Timer (timer), Broadcast (broadcast), Project (progetto), MemoryCompression (compressione memoria), Stopped (fermato); dopo 10 errori consecutivi entra automaticamente nello stato Stopped

### Sistema di Plugin
- **Architettura di estensione tramite plugin** — Implementa l'estensione delle funzionalità attraverso l'interfaccia IPlugin, supporta il caricamento dinamico di DLL di plugin da directory
- **Dichiarazione delle capacità dei plugin** — I plugin dichiarano le capacità necessarie (Network, FileIO, Process, AI) tramite l'attributo `[PluginCapability]`, e il caricatore riduce le regole di scansione di sicurezza di conseguenza; le capacità non dichiarabili (P/Invoke, Unsafe, Reflection Emit, ecc.) sono sempre bloccate
- **Caricamento isolato** — Utilizza AssemblyLoadContext personalizzato per il caricamento isolato, impedendo ai plugin di influenzare la stabilità del programma principale
- **Integrazione degli strumenti** — I plugin possono registrare strumenti personalizzati tramite l'interfaccia ITool, integrandosi automaticamente nel ciclo di chiamata degli strumenti

### Strumenti ed Esecuzione
- **24 strumenti integrati** — Coprono calendario, chat, configurazione, disco, rete, memoria, attività, timer, knowledge base, note di lavoro, area di lavoro del progetto, browser WebView, ecc.
- **Isolamento dello scenario degli strumenti** — Ogni strumento dichiara gli scenari disponibili tramite l'attributo `ToolScenario` (Chat, Task, Timer, MemoryCompression, Project), l'attributo `ChatOnly` limita gli strumenti all'uso solo nello scenario di chat
  - **Interfaccia delle capacità IAIClient** — I client AI dichiarano le capacità di modalità streaming, chiamate strumenti, finestra di contesto, visione e audio, e il ContextManager adatta il comportamento di conseguenza
- **Ciclo di chiamata degli strumenti** — L'AI restituisce una chiamata di strumento → esegue lo strumento → il risultato viene restituito all'AI → continua il ciclo finché non viene restituita una risposta di testo puro
- **Sicurezza Esecutore-Permesso** — Tutte le operazioni I/O passano attraverso una rigorosa verifica dei permessi tramite l'Esecutore
  - Catena di verifica dei permessi a 3 livelli: UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → rifiuto predefinito)
  - Registro di audit completo che registra tutte le decisioni sui permessi

### AI e Conoscenza
- **Supporto multi-backend AI**
  - **Ollama** — Distribuzione di modelli locali, utilizza l'API HTTP nativa
  - **Alibaba Cloud Bailian (DashScope)** — Servizio AI cloud, compatibile con l'API OpenAI, supporta 13+ modelli, distribuzione multi-regione
  - **Volcengine Ark** — Servizio AI cloud di ByteDance, supporta modalità streaming e non streaming, controllo della velocità integrato
  - **Herdsman** — Motore di inferenza senza autenticazione, compatibile con il formato API OpenAI
  - **Meituan LongCat** — Modello di grandi dimensioni sviluppato autonomamente da Meituan, compatibile con il formato API OpenAI, autenticazione con chiave API
  - **Qiniu Cloud AI** — Servizio di inferenza di modelli di grandi dimensioni di Qiniu Cloud, compatibile con il formato API OpenAI, autenticazione con chiave API
- **32 sistemi di calendario** — Copertura completa dei principali calendari del mondo, inclusi gregoriano, lunare cinese, islamico, ebraico, giapponese, persiano, maya, calendari storici cinesi, ecc.
- **Sistema di Rete di Conoscenza** — Grafo di conoscenza basato su triple (soggetto-relazione-oggetto), supporta archiviazione, interrogazione e scoperta di percorsi
- **Area di Lavoro del Progetto** — Gestione dello spazio del progetto, supporta creazione/archiviazione/distruzione di progetti, assegnazione di ruoli, note di lavoro, tracciamento delle attività e isolamento dei permessi degli strumenti
- **Motore di Workflow** — Motore di macchina a stati basato su modelli, supporta modelli di workflow personalizzati, transizioni di stato, esecuzione guidata da Tick e gestione del ciclo di vita delle istanze
- **Meccanismo di dissolvenza della memoria** — Servizio di decadimento temporizzato (MemoryFadeService), applica automaticamente il decadimento dell'importanza e l'archiviazione automatica delle memorie di tutti gli Esseri di Silicio ogni ora

### Interfaccia Web
- **Web UI moderna** — Server HTTP integrato, supporta aggiornamenti in tempo reale SSE
- **7 temi skin** — Gestione, Chat, Creativo, Sviluppo, Alto Contrasto, Chiaro, Minimalista, supporta scoperta automatica e cambio
- **24 controller** — Funzionalità complete di gestione del sistema, chat, configurazione, monitoraggio
- **Zero dipendenze da framework frontend** — Genera HTML/CSS/JS lato server tramite `H`, `CssBuilder` e `JsBuilder`

### Internazionalizzazione e Localizzazione
- **34 varianti linguistiche** pienamente supportate, che coprono 2 sistemi di scrittura e multiple varianti regionali
  - **Cinese semplificato**: zh-CN (Cina continentale), zh-SG (Singapore), zh-MY (Malesia) (3 varianti)
  - **Cinese tradizionale**: zh-HK (Hong Kong), zh-TW (Taiwan), zh-MO (Macau) (3 varianti)
  - **Inglese**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 varianti)
  - **Spagnolo**: es-ES, es-MX (2 varianti)
  - **Tedesco**: de-DE, de-AT, de-CH, de-LU, de-LI (5 varianti)
  - **Francese**: fr-FR, fr-CA, fr-CH (3 varianti)
  - **Giapponese**: ja-JP | **Coreano**: ko-KR | **Ceco**: cs-CZ (3 varianti)
  - **Italiano**: it-IT | **Polacco**: pl-PL | **Portoghese**: pt-PT, pt-BR (4 varianti)

### Dati e Archiviazione
- **Archiviazione ad alte prestazioni SpeedyPack** — La versione Fast utilizza il motore di archiviazione .spk proprietario, mappatura delle directory in memoria + cache delle voci + coda di scrittura asincrona
- **Archiviazione su file system** — La versione Default utilizza l'archiviazione JSON su puro file system
- **Interrogazione con indice temporale** — Supporta interrogazioni efficienti per intervallo di tempo tramite l'interfaccia `ITimeStorage`
- **Compressione automatica** — SpeedyPack supporta la compressione automatica periodica, recuperando spazio libero
- **Dipendenze minime** — La libreria principale dipende solo da Microsoft.CodeAnalysis.CSharp per la compilazione dinamica

## 🔄 Architettura a Doppia Versione

Questo progetto fornisce due versioni di implementazione per soddisfare diverse esigenze di scenario:

### SiliconLife.Default (Versione Predefinita)
- **Posizionamento**: Implementazione predefinita, utilizzata principalmente per verificare la fattibilità dell'architettura
- **Modalità di esecuzione**: Applicazione console
- **Metodo di archiviazione**: Archiviazione JSON su puro file system
- **Scenari applicabili**: Scenari con alti requisiti di sicurezza dei dati, risorse di memoria limitate, piccola quantità di dati
- **Caratteristiche**: Semplice e affidabile, persistenza dei dati immediata, nessun rischio di perdita di dati in memoria
- **Descrizione del ruolo**: Come implementazione di riferimento per la verifica dell'architettura, adatta per il primo contatto, lo sviluppo e il debug o scenari con priorità alla sicurezza dei dati
- **Comando di avvio**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (Versione ad Alte Prestazioni)
- **Posizionamento**: Versione di produzione raccomandata
- **Modalità di esecuzione**: Applicazione desktop (area di notifica di Windows/macOS / finestra di stato su Linux)
- **Metodo di archiviazione**: Archiviazione in memoria SpeedyPack + persistenza batch asincrona (formato file .spk)
- **Scenari applicabili**: Scenari ad alta concorrenza, bassa latenza, grandi volumi di dati
- **Supporto piattaforma**: Windows/macOS (funzionalità complete, inclusa area di notifica), Linux (finestra di stato, senza icona nell'area di notifica)
- **Caratteristiche**:
  - Ottimizzazione delle prestazioni estrema
  - Esecuzione in background nell'area di notifica di Windows/macOS, supporta monitoraggio in tempo reale della finestra di stato dell'area di notifica; la finestra di stato di Linux si mostra direttamente
  - Motore SpeedyPack + compressione automatica garantiscono la sicurezza dei dati
  - Architettura Component UI, 27 componenti dichiarativi
  - 7 temi skin, supporta scoperta automatica e cambio
  - Strumento di ricarica a caldo supporta aggiornamento e riavvio online → Linux apre automaticamente il browser per accedere alla Web UI, supporta il parametro `--no-tray`
- **Miglioramento delle prestazioni**: Latenza di lettura dell'archiviazione ridotta di 1000 volte, latenza di scrittura ridotta di 15000 volte, capacità di elaborazione concorrente aumentata di 50 volte
- **Descrizione del ruolo**: Implementazione di livello produttivo profondamente ottimizzata, prima scelta per esecuzione a lungo termine e ambienti di produzione effettivi
- **Comando di avvio**: `dotnet run --project src/SiliconLife.Fast`

### Confronto tra le Versioni

| Caratteristica | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **Modalità di esecuzione** | Applicazione console | Applicazione desktop (area di notifica di Windows/macOS / finestra di stato su Linux) |
| **Interfaccia utente** | Web UI (accesso tramite browser) | Windows/macOS: icona area di notifica + finestra area di notifica + Web UI; Linux: finestra di stato + Web UI |
| **Area di notifica di sistema** | ❌ Nessuna | ✅ Windows/macOS supporta la minimizzazione nell'area di notifica; Linux senza icona nell'area di notifica |
| **Esecuzione in background** | ❌ La chiusura della console termina l'applicazione | ✅ Windows/macOS esecuzione continua in background nell'area di notifica; Linux esecuzione nella finestra di stato |
| **Metodo di archiviazione** | Archiviazione JSON su file system | Archiviazione in memoria SpeedyPack + persistenza asincrona |
| **Motore di archiviazione** | I/O su file system | SiliconLife.Speedy (formato .spk) |
| **Latenza di lettura** | ~10ms (I/O disco) | ~0.01ms (operazione in memoria) |
| **Latenza di scrittura** | ~15ms (scrittura sincrona) | ~0.001ms (scrittura asincrona) |
| **Capacità concorrente** | ~100 req/s | ~5000 req/s |
| **Utilizzo di memoria** | ~200MB | ~500MB |
| **Sicurezza dei dati** | Molto alta (persistenza immediata) | Alta (persistenza asincrona + compressione automatica) |
| **Scenari applicabili** | Priorità alla sicurezza dei dati, piccola quantità di dati | Priorità alle prestazioni, grandi volumi di dati, alta concorrenza |

## 🛠️ Stack Tecnologico

| Componente | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| Linguaggio di programmazione | C# | C# |
| Tipo di applicazione | Applicazione console | Applicazione desktop (area di notifica di Windows/macOS / finestra di stato su Linux) |
| Integrazione AI | Ollama (locale), Alibaba Cloud Bailian (cloud), Volcengine Ark (cloud), Herdsman, Meituan LongCat, Qiniu Cloud AI | Ollama (locale), Alibaba Cloud Bailian (cloud), Volcengine Ark (cloud), Herdsman, Meituan LongCat, Qiniu Cloud AI |
| Archiviazione dati | File system (JSON + directory con indice temporale) | SpeedyPack (formato .spk, mappatura in memoria + persistenza asincrona) |
| Server Web | HttpListener (.NET integrato) | HttpListener (.NET integrato) |
| Compilazione dinamica | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automazione browser | Playwright (WebView) | Playwright (WebView) |
| Sistema di plugin | ✅ Supportato (IPlugin + PluginLoader) | ✅ Supportato (IPlugin + PluginLoader) |
| Area di notifica di sistema | ❌ Non supportata | ✅ Windows/macOS supportato (NotifyIcon); Linux senza icona nell'area di notifica |
| Licenza | Apache-2.0 | Apache-2.0 |

## 📁 Struttura del Progetto

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Libreria principale (interfacce, classi astratte)
│   │   ├── AI/                            # Interfacce client AI, Gestore del Contesto, modelli di messaggi
│   │   ├── Audit/                         # Sistema di audit utilizzo Token
│   │   ├── Chat/                          # Sistema di chat, gestione sessioni, canale di trasmissione
│   │   ├── Compilation/                   # Compilazione dinamica, scansione di sicurezza, crittografia del codice
│   │   ├── Config/                        # Sistema di gestione della configurazione
│   │   ├── Executors/                     # Esecutori (disco, rete, riga di comando)
│   │   ├── IM/                            # Interfaccia del fornitore di messaggistica istantanea
│   │   ├── Knowledge/                     # Sistema di rete di conoscenza
│   │   ├── Localization/                  # Sistema di localizzazione
│   │   ├── Logging/                       # Sistema di registrazione
│   │   ├── Plugins/                       # Sistema di plugin (interfaccia IPlugin, caricatore PluginLoader)
│   │   ├── Project/                       # Sistema di gestione dei progetti
│   │   ├── Runtime/                       # Ciclo Principale, oggetti clock, Host Principale
│   │   ├── Security/                      # Sistema di gestione dei permessi
│   │   ├── SiliconBeing/                  # Classe base Essere di Silicio, gestore, fabbrica
│   │   ├── Storage/                       # Interfacce di archiviazione
│   │   ├── Time/                          # Data incompleta (interrogazione per intervallo di tempo)
│   │   ├── Tools/                         # Interfaccia degli strumenti e Gestore degli Strumenti
│   │   ├── WebView/                       # Interfaccia browser WebView
│   │   ├── Workflow/                      # Motore di workflow (modelli, istanze, transizioni di stato)
│   │   └── ServiceLocator.cs              # Localizzatore di servizi globale
│   │
│   ├── SiliconLife.Common/                # Implementazione condivisa (comune a entrambe le versioni)
│   │   ├── AI/                            # Client e fabbriche AI (Ollama, DashScope, VolcengineArk, Herdsman, LongCat, QiniuAI)
│   │   ├── Calendar/                      # 32 implementazioni di calendario
│   │   ├── Localization/                  # Classe base di localizzazione e 34 implementazioni di varianti linguistiche/regionali
│   │   ├── Resources/                     # File di risorse condivise
│   │   ├── Security/                      # Gestore dei Permessi
│   │   ├── SiliconBeing/                  # Implementazione predefinita dell'Essere di Silicio
│   │   ├── Tools/                         # 23 implementazioni di strumenti comuni
│   │   ├── Web/                           # Infrastruttura Web
│   │   └── WebView/                       # Implementazione Playwright WebView
│   │
│   ├── SiliconLife.App/                   # Livello applicazione (Web UI + documentazione di aiuto, condiviso tra Default e Fast)
│   │   ├── Config/                        # Configurazione dell'applicazione
│   │   ├── Data/                          # Directory dei dati
│   │   ├── Help/                          # Localizzazione della documentazione di aiuto (multilingue)
│   │   ├── Tools/                         # HelpTool (strumento di consultazione della documentazione di aiuto)
│   │   └── Web/                           # Implementazione Web UI
│   │       ├── Component/                 # Libreria di componenti UI (27 componenti)
│   │       ├── Controllers/               # 24 controller
│   │       ├── Models/                    # Modelli di vista
│   │       ├── Views/                     # Viste HTML
│   │       └── Skins/                     # 7 temi skin
│   │
│   ├── SiliconLife.Default/               # Implementazione predefinita + punto di ingresso dell'applicazione (versione console)
│   │   ├── Program.cs                     # Punto di ingresso (assemblaggio di tutti i componenti)
│   │   ├── Config/                        # Dati di configurazione predefiniti
│   │   ├── Knowledge/                     # Implementazione della rete di conoscenza
│   │   ├── Logging/                       # Implementazione del fornitore di registro (console + file system)
│   │   ├── Project/                       # Implementazione del sistema di progetto
│   │   └── Storage/                       # Implementazione dell'archiviazione su file system
│   │
│   ├── SiliconLife.Fast/                  # Implementazione ad alte prestazioni + punto di ingresso dell'applicazione (versione finestra)
│   │   ├── Program.cs                     # Punto di ingresso (applicazione finestra)
│   │   ├── App.axaml / App.cs             # Definizione applicazione Avalonia
│   │   ├── Config/                        # Dati di configurazione (condivisi con Default)
│   │   ├── Knowledge/                     # Implementazione della rete di conoscenza (ottimizzazione memoria)
│   │   ├── Logging/                       # Fornitore di registro ad alte prestazioni
│   │   ├── Project/                       # Implementazione del sistema di progetto
│   │   ├── Storage/                       # Adattatore di archiviazione SpeedyPack
│   │   └── Tray/                          # Area di notifica di sistema (localizzazione per 34 varianti linguistiche)
│   │
│   ├── SiliconLife.Speedy/                # Motore di archiviazione ad alte prestazioni SpeedyPack
│   │   ├── SpeedyPack.cs                  # Classe principale (mappatura directory in memoria + cache + scrittura asincrona)
│   │   ├── SpeedyPackOptions.cs           # Opzioni di configurazione (TTL cache, numero massimo di voci, ecc.)
│   │   ├── IPackTransaction.cs            # Interfaccia transazione
│   │   ├── SpkFileInfo.cs                 # Informazioni sul file
│   │   └── Internal/                      # Implementazione interna
│   │       ├── DirectoryMap.cs            # Mappatura directory in memoria
│   │       ├── EntryCache.cs              # Cache delle voci
│   │       ├── FreeList.cs                # Gestione dello spazio libero
│   │       ├── PackFileReader.cs          # Lettore di file pacchetto
│   │       ├── PackFileWriter.cs          # Scrittore di file pacchetto
│   │       ├── WriteQueue.cs              # Coda di scrittura asincrona
│   │       ├── WriteOperation.cs          # Operazione di scrittura
│   │       ├── SpeedyTransaction.cs       # Implementazione della transazione
│   │       ├── SpkHeader.cs               # Intestazione del file pacchetto
│   │       └── PathNormalizer.cs          # Normalizzazione del percorso
│   │
│   └── SiliconLife.Speedy.Manager/        # Strumento di gestione SpeedyPack (Avalonia UI)
│       ├── MainForm.cs                    # Finestra principale
│       ├── Program.cs                     # Punto di ingresso
│       └── slc.ico                        # Icona dell'applicazione
│
├── docs/                                  # Documentazione multilingue
│   ├── zh-CN/                             # Documentazione in cinese semplificato
│   ├── en/                                # Documentazione in inglese
│   └── ...                                # Documentazione in altre lingue
│
└── 总文档/                                 # Documenti di requisiti e architettura
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ Panoramica dell'Architettura

### Architettura di Pianificazione
```
Ciclo Principale (thread dedicato, Cane da Guardia + Interruttore)
  └── Oggetto Clock (ordinato per priorità)
       └── Gestore di Esseri di Silicio
            └── Runner Essere di Silicio (thread temporaneo, timeout + Interruttore)
                 └── Essere di Silicio.Tick()
                      └── Gestore del Contesto.Pensa()
                           └── Client AI.Chat()
                                └── Ciclo di chiamata degli strumenti → Persistenza nel Sistema di Chat
```

### Architettura di Sicurezza
Tutte le operazioni I/O avviate dall'AI devono passare attraverso una rigida catena di sicurezza:

```
Chiamata strumento → Esecutore → Gestore dei Permessi → [Cache frequenza → Callback → (IsCurator: chiedi all'utente | Non-curator: ACL Globale)]
```

## 🚀 Inizio Rapido

### Prerequisiti

- **.NET 9 SDK** — [Link di download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Backend AI** (sceglierne uno):
  - **Ollama**: [Installa Ollama](https://ollama.com) e scarica un modello (ad esempio `ollama pull llama3`)
  - **Alibaba Cloud Bailian**: Ottieni una chiave API dalla [console Bailian](https://bailian.console.aliyun.com/)
  - **Volcengine Ark**: Ottieni una chiave API dalla [console Volcengine](https://console.volcengine.com/ark)

### Compila il Progetto

```bash
dotnet restore
dotnet build
```

### Esegui il Sistema

#### Metodo 1: Esegui la versione Default (applicazione console)

```bash
dotnet run --project src/SiliconLife.Default
```

L'applicazione avvierà il server Web e aprirà automaticamente la Web UI nel browser.

**Scenari applicabili**:
- ✅ Requisiti di sicurezza dei dati molto elevati
- ✅ Risorse di memoria limitate (RAM < 2GB)
- ✅ Piccola quantità di dati, utilizzo a breve termine
- ✅ Fase di sviluppo e debug

#### Metodo 2: Esegui la versione Fast (applicazione desktop)

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS**: L'applicazione si avvierà in modalità finestra, si minimizzerà nell'area di notifica di sistema e continuerà a funzionare in background.

**Linux**: L'applicazione mostrerà una finestra di stato (senza icona nell'area di notifica di sistema) e aprirà automaticamente il browser per accedere alla Web UI. È anche possibile utilizzare il parametro `--no-tray` per saltare l'apertura automatica del browser:

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

**Scenari applicabili**:
- ✅ Scenari ad alta concorrenza (> 5 utenti)
- ✅ Grandi volumi di dati (utilizzo superiore a 3 mesi)
- ✅ Necessità di risposta a bassa latenza
- ✅ Necessità di esecuzione in background nell'area di notifica

### Pubblicazione come File Singolo

```bash
# Windows - Versione Default
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Versione Fast
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - Versione Default
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux - Versione Fast
dotnet publish src/SiliconLife.Fast -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Versione Default
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS - Versione Fast
dotnet publish src/SiliconLife.Fast -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Roadmap di Sviluppo

### ✅ Completato
- [x] Fase 1: Chat AI da console
- [x] Fase 2: Scheletro del framework (Ciclo Principale + Oggetto Clock + Cane da Guardia + Interruttore)
- [x] Fase 3: Primo Essere di Silicio con File dell'Anima (architettura Corpo-Cervello)
- [x] Fase 4: Memoria persistente (Sistema di Chat + interfaccia Archiviazione Temporale)
- [x] Fase 5: Sistema di strumenti + Esecutori
- [x] Fase 6: Sistema di permessi (catena a 5 livelli, logger di audit, Lista di Controllo degli Accessi Globale)
- [x] Fase 7: Compilazione dinamica + auto-evoluzione (Roslyn)
- [x] Fase 8: Memoria a lungo termine + attività + timer
- [x] Fase 9: Host Principale + collaborazione multi-agente
- [x] Fase 10: Web UI (HTTP + SSE, 24 controller, 7 skin)
- [x] Fase 10.5: Miglioramenti incrementali (canale di trasmissione, audit Token, 32 calendari, miglioramento strumenti, localizzazione per 34 varianti linguistiche)
- [x] Fase 10.6: Perfezionamento e ottimizzazione (WebView, sistema di aiuto, area di lavoro del progetto, rete di conoscenza, motore di workflow)
- [x] Fase 11: Motore di archiviazione SpeedyPack (sostituisce LiteDB, mappatura in memoria, coda di scrittura asincrona, compressione automatica)
- [x] Fase 12: Sistema di plugin (interfaccia IPlugin, dichiarazione delle capacità PluginLoader, caricamento isolato, integrazione strumenti)

### 🚧 In Pianificazione
- [ ] Fase 13: Integrazione messaggistica istantanea esterna (Feishu / WhatsApp / Telegram)
- [ ] Fase 14: Ecosistema di competenze (marketplace di plugin, distribuzione di pacchetti di competenze)

## 📚 Documentazione

- [Progettazione dell'architettura](architecture.md) — Progettazione del sistema, meccanismo di pianificazione, architettura dei componenti
- [Modello di sicurezza](security.md) — Modello di permessi, Esecutori, sicurezza della compilazione dinamica
- [Guida allo sviluppo](development-guide.md) — Sviluppo di strumenti, guida all'estensione
- [Riferimento API](api-reference.md) — Documentazione degli endpoint Web API
- [Riferimento strumenti](tools-reference.md) — Descrizione dettagliata degli strumenti integrati
- [Guida Web UI](web-ui-guide.md) — Guida all'uso dell'interfaccia Web
- [Guida all'Essere di Silicio](silicon-being-guide.md) — Guida allo sviluppo degli agenti
- [Sistema di permessi](permission-system.md) — Spiegazione dettagliata della gestione dei permessi
- [Sistema di calendario](calendar-system.md) — Descrizione dei 32 sistemi di calendario
- [Inizio rapido](getting-started.md) — Guida introduttiva dettagliata
- [Risoluzione dei problemi](troubleshooting.md) — Domande frequenti
- [Roadmap](roadmap.md) — Piano di sviluppo completo
- [Registro delle modifiche](changelog.md) — Cronologia degli aggiornamenti delle versioni
- [Guida ai contributi](contributing.md) — Come partecipare al progetto

## 🤝 Partecipa con un Contributo

Accogliamo ogni forma di contributo! Per i dettagli, consulta la [Guida ai contributi](contributing.md).

### Flusso di lavoro di sviluppo
1. Fai fork di questo repository
2. Crea un branch per la funzionalità (`git checkout -b feature/AmazingFeature`)
3. Conferma le modifiche (`git commit -m 'feat: add some AmazingFeature'`)
4. Pusha nel branch (`git push origin feature/AmazingFeature`)
5. Invia una Pull Request

## 💡 Guida alla Scelta della Versione

### Quale versione dovrei usare?

**SiliconLife.Default (Implementazione predefinita — verifica della fattibilità dell'architettura):**
- 📌 È la prima volta che entri in contatto con questo progetto e vuoi comprendere rapidamente l'architettura del sistema
- 📌 Stai sviluppando e facendo debug, hai bisogno di un modo di esecuzione semplice e diretto
- 📌 La sicurezza dei dati è la tua priorità principale
- 📌 La memoria del tuo sistema è inferiore a 4GB
- 📌 Hai bisogno di uso singolo o una piccola quantità di dati

**SiliconLife.Fast (Versione di produzione raccomandata):**
- ⚡ Hai bisogno di un ambiente di produzione stabile a lungo termine
- ⚡ Conosci già l'architettura del sistema e sei pronto per la distribuzione effettiva
- ⚡ Devi supportare l'accesso concorrente di più utenti
- ⚡ Hai bisogno dell'esecuzione in background nell'area di notifica di sistema
- ⚡ Cerchi l'esperienza di prestazioni estreme

> **Consiglio generale**: SiliconLife.Default è adatto come verifica dell'architettura e esperienza iniziale; per ambienti di produzione effettivi, si raccomanda vivamente l'uso di SiliconLife.Fast.

### Posso migrare da Default a Fast?

**Assolutamente sì!** Le due versioni condividono lo stesso:
- ✅ Formato dei file di configurazione (config.json)
- ✅ Interfacce degli strumenti
- ✅ Configurazione dei Being
- ✅ Interfaccia Web UI

**Passaggi di migrazione:**
1. Esegui il backup della directory dei dati di Default
2. Avvia la versione Fast utilizzando la stessa directory dei dati
3. Fast importerà automaticamente i dati esistenti nel motore di archiviazione SpeedyPack
4. Dopo aver verificato che le funzionalità funzionano correttamente, puoi utilizzare la versione Fast quotidianamente

### Le due versioni possono coesistere?

**Sì!** Si raccomanda la seguente strategia di distribuzione:

**Strategia 1: Default per la verifica, Fast per la produzione**
```
Ambiente di sviluppo/verifica: SiliconLife.Default (verifica dell'architettura, debug delle funzionalità)
Ambiente di produzione: SiliconLife.Fast (alte prestazioni, esecuzione in background, elaborazione richieste in tempo reale)
```

**Strategia 2: Fast per l'esecuzione principale, Default per backup periodici**
```
SiliconLife.Fast (uso quotidiano, elaborazione richieste in tempo reale)
    ↓ Backup periodico
SiliconLife.Default (archiviazione dati freddi, sicurezza dei dati come rete di salvataggio)
```

## 📄 Licenza

Questo progetto è concesso in licenza sotto la Apache License 2.0 — vedi il file [LICENSE](../../LICENSE) per i dettagli.

## 👨‍💻 Autore

**天源垦骥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Ringraziamenti

Grazie a tutti gli sviluppatori e ai fornitori di piattaforme AI che hanno contribuito a questo progetto.

---

**Silicon Life Collective** — Fai in modo che gli agenti AI "vivano" davvero
