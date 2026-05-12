![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Versione: v0.1.0-alpha** | **Silicon Life Collective** — Una piattaforma di collaborazione multi-agente basata su .NET 9, dove gli agenti IA sono chiamati **Silicon Beings** e possono auto-evolversi attraverso la compilazione dinamica Roslyn.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [Français](../fr-FR/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Čeština](../cs-CZ/README.md) | **Italiano** | [Polski](../pl-PL/README.md)

## 🌟 Funzionalità principali

### Sistema di agenti
- **Orchestrazione multi-agente** — Gestione centralizzata dal *Silicon Curator*, con meccanismo di pianificazione equa per time-slice controllato da orologio
- **Guidato da file dell'anima** — Ogni Silicon Being è controllato da un file prompt centrale (`soul.md`) che definisce una personalità unica e modelli di comportamento
- **Architettura Body-Brain** — Il *Body* (SiliconBeing) riceve i segni vitali e rileva gli scenari di attivazione; il *Brain* (ContextManager) è responsabile del caricamento della cronologia, delle chiamate IA, dell'esecuzione degli strumenti e della persistenza delle risposte
- **Capacità di auto-sviluppo** — Attraverso la tecnologia di compilazione dinamica Roslyn, i Silicon Beings possono riscrivere il proprio codice per realizzare l'evoluzione
- **Gestione degli stati di attività** — Supporto per quattro stati di attività: Idle (inattivo), Working (in lavoro), Error (errore), Stopped (arrestato). Entrata automatica nello stato Stopped dopo 10 errori consecutivi

### Sistema di plugin
- **Architettura di estensione tramite plugin** — Estensione delle funzionalità tramite l'interfaccia IPlugin, supporta il caricamento dinamico di DLL di plugin da una directory
- **Sandbox sicura** — Il caricatore di plugin esegue analisi di sicurezza rigorose, vieta l'accesso a System.IO, System.Net e altri namespace
- **Caricamento isolato** — Utilizzo di un AssemblyLoadContext personalizzato per il caricamento isolato, impedendo ai plugin di compromettere la stabilità del programma principale
- **Integrazione degli strumenti** — I plugin possono registrare strumenti personalizzati tramite l'interfaccia ITool, automaticamente integrati nel ciclo di chiamata degli strumenti

### Strumenti ed Esecuzione
- **24 strumenti integrati** — Coprono calendario, chat, configurazione, disco, rete, memoria, attività, timer, base di conoscenza, note di lavoro, browser WebView, hot reload, ecc.
- **Strumento di hot reload** — Supporta la compilazione automatica, l'aggiornamento dei file e il riavvio di SiliconLife.Fast durante l'esecuzione, senza intervento manuale
- **Ciclo di chiamata degli strumenti** — L'IA restituisce una chiamata di strumento → Esegue lo strumento → Restituisce i risultati all'IA → Continua il ciclo fino a una risposta in testo puro
- **Sicurezza dei permessi dell'esecutore** — Tutte le operazioni I/O passano attraverso una validazione rigorosa dei permessi tramite gli esecutori
  - Catena di permessi a 5 livelli: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Registrazione di audit completa di tutte le decisioni sui permessi

### IA e Conoscenza
- **Supporto per molteplici backend IA**
  - **Ollama** — Distribuzione di modelli locali, con API HTTP nativa
  - **Alibaba Cloud DashScope (Bailian)** — Servizio IA cloud, compatibile con API OpenAI, supporta 13+ modelli, distribuzione multi-regione
  - **Volcengine Ark (VolcengineArk)** — Servizio IA cloud di ByteDance, supporta modalità streaming e non-streaming, controllo di velocità integrato
- **32 sistemi di calendario** — Copertura completa dei principali calendari mondiali, inclusi calendario gregoriano, calendario lunare cinese, calendario islamico, calendario ebraico, calendario giapponese, calendario persiano, calendario maya, calendario storico cinese, ecc.
- **Sistema di rete di conoscenza** — Grafo della conoscenza basato su triple (soggetto-relazione-oggetto), supporta archiviazione, query e scoperta di percorsi

### Interfaccia Web
- **Interfaccia Web moderna** — Server HTTP integrato con aggiornamenti in tempo reale SSE
- **7 temi d'aspetto** — Versioni Admin, Chat, Creative, Dev, Alto contrasto, Light, Minimal, supporta rilevamento e commutazione automatici
- **20+ controller** — Gestione completa del sistema, chat, configurazione, funzionalità di monitoraggio
- **Zero dipendenze framework frontend** — HTML/CSS/JS generati lato server tramite `H`, `CssBuilder` e `JsBuilder`

### Internazionalizzazione e Localizzazione
- **Supporto completo per 30 implementazioni linguistiche**, che coprono 2 sistemi di scrittura e molteplici varianti regionali
  - **Cinese semplificato**: zh-CN (Cina continentale), zh-SG (Singapore), zh-MY (Malesia) (3 varianti)
  - **Cinese tradizionale**: zh-HK (Hong Kong), zh-TW (Taiwan), zh-MO (Macao) (3 varianti)
  - **Inglese**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 varianti)
  - **Spagnolo**: es-ES, es-MX (2 varianti)
  - **Tedesco**: de-DE, de-AT, de-CH, de-LU, de-LI (5 varianti)
  - **Francese**: fr-FR, fr-CA, fr-CH (3 varianti)
  - **Italiano**: it-IT (1 variante)
  - **Giapponese**: ja-JP | **Coreano**: ko-KR | **Ceco**: cs-CZ (3 varianti)

### Dati e Archiviazione
- **Archiviazione SpeedyPack ad alte prestazioni** — La versione Fast utilizza il motore di archiviazione proprietario .spk, mappatura delle directory in memoria + cache delle voci + coda di scrittura asincrona
- **Archiviazione su file system** — La versione Default utilizza un'archiviazione pura su file system JSON
- **Query per indice temporale** — Query efficienti per intervallo di tempo tramite l'interfaccia `ITimeStorage`
- **Compressione automatica** — SpeedyPack supporta la compressione automatica pianificata per recuperare lo spazio di archiviazione
- **Dipendenze minime** — La libreria principale dipende solo da Microsoft.CodeAnalysis.CSharp per la compilazione dinamica

## 🔄 Architettura a doppia versione

Questo progetto offre due versioni di implementazione per soddisfare diverse esigenze di scenari:

### SiliconLife.Default (Versione standard)
- **Posizionamento**: Implementazione standard, principalmente per la verifica della fattibilità architetturale
- **Modalità di esecuzione**: Applicazione console
- **Metodo di archiviazione**: Archiviazione JSON pura su file system
- **Scenari applicabili**: Elevati requisiti di sicurezza dei dati, risorse di memoria limitate, piccolo volume di dati
- **Caratteristiche**: Semplice e affidabile, persistenza immediata dei dati, nessun rischio di perdita di dati
- **Descrizione del ruolo**: Implementazione di riferimento per la verifica architetturale, adatta al primo contatto, al debug di sviluppo o agli scenari con priorità alla sicurezza dei dati
- **Comando di avvio**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (Versione ad alte prestazioni)
- **Posizionamento**: Versione principale di produzione
- **Modalità di esecuzione**: Applicazione Windows Forms (supporta la barra di stato di sistema)
- **Metodo di archiviazione**: Archiviazione in memoria SpeedyPack + persistenza batch asincrona (formato file .spk)
- **Scenari applicabili**: Alta concorrenza, bassa latenza, grandi volumi di dati
- **Caratteristiche**:
  - Ottimizzazione delle prestazioni estrema
  - Esecuzione in background nella barra di stato, monitoraggio in tempo reale tramite la finestra di stato della barra di stato
  - Motore SpeedyPack + compressione automatica che garantisce la sicurezza dei dati
  - Architettura Component UI, 30+ componenti dichiarativi
  - 7 temi d'aspetto, supporta rilevamento e commutazione automatici
  - Strumento di hot reload per aggiornamenti e riavvii online
- **Miglioramento delle prestazioni**: Latenza di lettura memoria ridotta di 1000x, latenza di scrittura ridotta di 15000x, capacità di elaborazione parallela aumentata di 50x
- **Descrizione del ruolo**: Implementazione pronta per la produzione con ottimizzazione approfondita, la scelta migliore per lo sfruttamento a lungo termine e i veri ambienti di produzione
- **Comando di avvio**: `dotnet run --project src/SiliconLife.Fast`

### Confronto delle versioni

| Caratteristica | SiliconLife.Default | SiliconLife.Fast |
|---------|---------------------|------------------|
| **Modalità di esecuzione** | Applicazione console | Applicazione Forms (barra di stato) |
| **Interfaccia utente** | Interfaccia Web (accesso browser) | Icona barra di stato + Finestra barra di stato + Interfaccia Web |
| **Barra di stato di sistema** | ❌ No | ✅ Supporta la minimizzazione nella barra di stato |
| **Esecuzione in background** | ❌ Termina alla chiusura della console | ✅ Esecuzione continua in background nella barra di stato |
| **Metodo di archiviazione** | Archiviazione JSON su file system | Archiviazione in memoria SpeedyPack + persistenza asincrona |
| **Motore di archiviazione** | I/O su file system | SiliconLife.Speedy (formato .spk) |
| **Latenza di lettura** | ~10ms (I/O su disco) | ~0.01ms (operazione in memoria) |
| **Latenza di scrittura** | ~15ms (scrittura sincrona) | ~0.001ms (scrittura asincrona) |
| **Concorrenza** | ~100 req/s | ~5000 req/s |
| **Utilizzo memoria** | ~200MB | ~500MB |
| **Sicurezza dei dati** | Estremamente elevata (persistenza immediata) | Elevata (persistenza asincrona + compressione automatica) |
| **Scenario consigliato** | Priorità alla sicurezza dei dati, piccoli dati | Priorità alle prestazioni, grandi dati, alta concorrenza |

## 🛠️ Stack tecnologico

| Componente | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 Windows |
| Linguaggio di programmazione | C# | C# |
| Tipo di applicazione | Applicazione console | Applicazione Windows Forms |
| Integrazione IA | Ollama (locale), Alibaba Cloud DashScope (cloud) | Ollama (locale), Alibaba Cloud DashScope (cloud), Volcengine Ark (cloud) |
| Archiviazione dati | File system (JSON + directory indice temporale) | SpeedyPack (formato .spk, mappatura in memoria + persistenza asincrona) |
| Server Web | HttpListener (.NET integrato) | HttpListener (.NET integrato) |
| Compilazione dinamica | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automazione browser | Playwright (WebView) | Playwright (WebView) |
| Sistema di plugin | ✅ Supportato (IPlugin + PluginLoader) | ✅ Supportato (IPlugin + PluginLoader) |
| Barra di stato di sistema | ❌ Non supportato | ✅ Supportato (NotifyIcon) |
| Licenza | Apache-2.0 | Apache-2.0 |

## 📁 Struttura del progetto

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Libreria principale (interfacce, classi astratte)
│   │   ├── AI/                            # Interfacce client IA, ContextManager, modelli messaggi
│   │   ├── Audit/                         # Sistema audit utilizzo token
│   │   ├── Chat/                          # Sistema chat, gestione sessioni, canali broadcast
│   │   ├── Compilation/                   # Compilazione dinamica, analisi sicurezza, crittografia codice
│   │   ├── Config/                        # Sistema gestione configurazione
│   │   ├── Executors/                     # Esecutori (disco, rete, riga comando)
│   │   ├── IM/                            # Interfacce provider messaggistica istantanea
│   │   ├── Knowledge/                     # Sistema rete conoscenza
│   │   ├── Localization/                  # Sistema localizzazione
│   │   ├── Logging/                       # Sistema logging
│   │   ├── Plugins/                       # Sistema plugin (interfaccia IPlugin, PluginLoader)
│   │   ├── Project/                       # Sistema gestione progetti
│   │   ├── Runtime/                       # Loop principale, oggetti orologio, host principale
│   │   ├── Security/                      # Sistema gestione permessi
│   │   ├── SiliconBeing/                  # Classe base Silicon Being, manager, factory
│   │   ├── Storage/                       # Interfacce storage
│   │   ├── Time/                          # Date incomplete (query per intervallo temporale)
│   │   ├── Tools/                         # Interfacce strumenti e tool manager
│   │   ├── WebView/                       # Interfacce browser WebView
│   │   └── ServiceLocator.cs              # Localizzatore servizi globale
│   │
│   ├── SiliconLife.Common/                # Implementazione comune (entrambe le versioni)
│   │   ├── AI/                            # Client IA e factory (Ollama, DashScope, VolcengineArk)
│   │   ├── Calendar/                      # 32 implementazioni calendari
│   │   ├── Localization/                  # Classe base localizzazione e 29 varianti linguistiche/regionali
│   │   ├── Resources/                     # File risorse condivisi
│   │   ├── Security/                      # Permission Manager
│   │   ├── SiliconBeing/                  # Implementazione standard Silicon Being
│   │   ├── Tools/                         # 23 strumenti comuni (incluso hot reload)
│   │   ├── Web/                           # Infrastruttura Web
│   │   └── WebView/                       # Implementazione Playwright WebView
│   │
│   ├── SiliconLife.App/                   # Livello applicativo (Web UI + aiuto, condiviso tra Default e Fast)
│   │   ├── Config/                        # Configurazione applicativa
│   │   ├── Data/                          # Directory dati
│   │   ├── Help/                          # Localizzazione documentazione aiuto (multilingue)
│   │   └── Web/                           # Implementazione interfaccia Web
│   │       ├── Component/                 # Libreria componenti UI (30+ componenti)
│   │       ├── Controllers/               # 22 controller
│   │       ├── Models/                    # ViewModel
│   │       ├── Views/                     # Viste HTML
│   │       └── Skins/                     # 7 temi d'aspetto
│   │
│   ├── SiliconLife.Default/               # Implementazione standard + punto ingresso (versione console)
│   │   ├── Program.cs                     # Punto ingresso (assemblaggio tutti i componenti)
│   │   ├── Config/                        # Dati configurazione standard
│   │   ├── IM/                            # Provider WebUI
│   │   ├── Knowledge/                     # Implementazione rete conoscenza
│   │   ├── Logging/                       # Implementazioni provider logging
│   │   ├── Project/                       # Implementazione sistema progetti
│   │   ├── Security/                      # Callback permessi standard
│   │   ├── Storage/                       # Implementazione storage su file system
│   │   └── Tools/                         # Strumenti specifici versione (HelpTool)
│   │
│   ├── SiliconLife.Fast/                  # Implementazione alte prestazioni + punto ingresso (versione Forms)
│   │   ├── Program.cs                     # Punto ingresso (applicazione Forms)
│   │   ├── Config/                        # Dati configurazione (condivisi con Default)
│   │   ├── IM/                            # Provider WebUI
│   │   ├── Knowledge/                     # Implementazione rete conoscenza (ottimizzata memoria)
│   │   ├── Logging/                       # Provider logging alte prestazioni
│   │   ├── Project/                       # Implementazione sistema progetti
│   │   ├── Security/                      # Callback permessi ottimizzati
│   │   ├── Storage/                       # Adattatore storage SpeedyPack
│   │   ├── Tools/                         # Strumenti specifici versione (HelpTool)
│   │   └── Tray/                          # Barra di stato sistema (29 varianti linguistiche)
│   │
│   ├── SiliconLife.Speedy/                # Motore storage alte prestazioni SpeedyPack
│   │   ├── SpeedyPack.cs                  # Classe principale (mappatura directory in memoria + cache + scrittura asincrona)
│   │   ├── SpeedyPackOptions.cs           # Opzioni configurazione (TTL cache, max voci, ecc.)
│   │   ├── IPackTransaction.cs            # Interfaccia transazione
│   │   ├── SpkFileInfo.cs                 # Informazioni file
│   │   └── Internal/                      # Implementazione interna
│       │   ├── DirectoryMap.cs            # Mappatura directory in memoria
│       │   ├── EntryCache.cs              # Cache voci
│       │   ├── FreeList.cs                # Gestione spazio libero
│       │   ├── PackFileReader.cs          # Lettore file pacchetto
│       │   ├── PackFileWriter.cs          # Scrittore file pacchetto
│       │   ├── WriteQueue.cs              # Coda scrittura asincrona
│       │   ├── WriteOperation.cs          # Operazione scrittura
│       │   ├── SpeedyTransaction.cs       # Implementazione transazione
│       │   ├── SpkHeader.cs              # Header file pacchetto
│       │   └── PathNormalizer.cs          # Normalizzazione percorso
│   │
│   └── SiliconLife.Speedy.Manager/        # Strumento gestione SpeedyPack (Windows Forms)
│       ├── MainForm.cs                    # Form principale
│       ├── Program.cs                     # Punto ingresso
│       └── slc.ico                        # Icona applicazione
│
├── docs/                                  # Documentazione multilingue
│   ├── zh-CN/                             # Cinese semplificato
│   ├── en/                                # Inglese
│   └── ...                                # Altre lingue
```

## 🏗️ Panoramica architettura

### Architettura scheduling
```
Loop principale (thread dedicato, watchdog + circuit breaker)
  └── Oggetto orologio (ordinato per priorità)
       └── Gestore Silicon Beings
            └── Esecutore Silicon Being (thread temporaneo, timeout + circuit breaker)
                 └── SiliconBeing.Tick()
                      └── ContextManager.Pensare()
                           └── Client-IA.Chat()
                                └── Loop chiamata strumenti → Persistenza nel sistema chat
```

### Architettura sicurezza
Tutte le operazioni I/O iniziate dall'IA devono attraversare una catena di sicurezza rigorosa:

```
Chiamata strumento → Esecutore → Gestore permessi → [IsCurator → Cache frequenza → GlobalACL → Callback → Richiesta utente]
```

## 🚀 Avvio rapido

### Prerequisiti

- **.NET 9 SDK** — [Link download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Backend IA** (scegline uno):
  - **Ollama**: [Installa Ollama](https://ollama.com) e scarica un modello (es. `ollama pull llama3`)
  - **Alibaba Cloud DashScope**: Ottieni una chiave API dalla [console DashScope](https://bailian.console.aliyun.com/)
  - **Volcengine Ark**: Ottieni una chiave API dalla [console Volcengine](https://console.volcengine.com/ark)

### Compilare il progetto

```bash
dotnet restore
dotnet build
```

### Eseguire il sistema

#### Metodo 1: Eseguire la versione Default (applicazione console)

```bash
dotnet run --project src/SiliconLife.Default
```

L'applicazione avvia il server Web e apre automaticamente l'interfaccia Web nel browser.

**Scenari applicabili**:
- ✅ Requisiti sicurezza dati estremamente elevati
- ✅ Risorse memoria limitate (RAM < 2GB)
- ✅ Piccolo volume dati, utilizzo a breve termine
- ✅ Fase sviluppo e debug

#### Metodo 2: Eseguire la versione Fast (applicazione Windows Forms)

```bash
dotnet run --project src/SiliconLife.Fast
```

L'applicazione si avvia in modalità Forms, si minimizza nella barra di stato di sistema e continua a funzionare in background.

**Scenari applicabili**:
- ✅ Scenari altamente paralleli (> 5 utenti)
- ✅ Grandi volumi dati (utilizzo oltre 3 mesi)
- ✅ Requisiti bassa latenza
- ✅ Esecuzione in background nella barra di stato richiesta

### Pubblicazione in file singolo

```bash
# Windows - Versione Default
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Versione Fast
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - Solo versione Default
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Solo versione Default
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Roadmap sviluppo

### ✅ Completato
- [x] Fase 1: Chat IA in console
- [x] Fase 2: Scheletro framework (loop principale + oggetti orologio + watchdog + circuit breaker)
- [x] Fase 3: Primo Silicon Being con file dell'anima (architettura Body-Brain)
- [x] Fase 4: Storage persistente (sistema chat + interfaccia Time Storage)
- [x] Fase 5: Sistema strumenti + esecutori
- [x] Fase 6: Sistema permessi (catena 5 livelli, log audit, GlobalACL)
- [x] Fase 7: Compilazione dinamica + auto-sviluppo (Roslyn)
- [x] Fase 8: Storage lungo termine + attività + timer
- [x] Fase 9: Host principale + collaborazione multi-agente
- [x] Fase 10: Interfaccia Web (HTTP + SSE, 20+ controller, 7 temi)
- [x] Fase 10.5: Miglioramenti incrementali (canali broadcast, audit token, 32 calendari, miglioramenti strumenti, localizzazione 29 lingue)
- [x] Fase 10.6: Completamento & Ottimizzazione (WebView, sistema aiuto, spazio progetti, rete conoscenza)
- [x] Fase 11: Motore storage SpeedyPack (sostituzione LiteDB, mappatura memoria, coda scrittura asincrona, compressione automatica)
- [x] Fase 12: Sistema plugin (interfaccia IPlugin, sandbox sicurezza PluginLoader, caricamento isolato, integrazione strumenti)

### 🚧 Pianificato
- [ ] Fase 13: Integrazione IM esterna (Feishu / WhatsApp / Telegram)
- [ ] Fase 14: Ecosistema competenze (marketplace plugin, distribuzione pacchetti competenze)

## 📚 Documentazione

- [Progettazione architetturale](architecture.md) — Design sistema, meccanismi scheduling, architettura componenti
- [Modello sicurezza](security.md) — Modello permessi, esecutori, sicurezza compilazione dinamica
- [Guida sviluppo](development-guide.md) — Sviluppo strumenti, guida estensione
- [Riferimento API](api-reference.md) — Documentazione endpoint API Web
- [Riferimento strumenti](tools-reference.md) — Descrizione dettagliata strumenti integrati
- [Guida interfaccia Web](web-ui-guide.md) — Guida utente interfaccia Web
- [Guida Silicon Being](silicon-being-guide.md) — Guida sviluppo agenti
- [Sistema permessi](permission-system.md) — Gestione permessi in dettaglio
- [Sistema calendario](calendar-system.md) — Descrizione 32 sistemi calendario
- [Avvio rapido](getting-started.md) — Guida dettagliata per principianti
- [Risoluzione problemi](troubleshooting.md) — Domande frequenti
- [Roadmap](roadmap.md) — Piano sviluppo completo
- [Changelog](changelog.md) — Cronologia aggiornamenti versione
- [Guida contribuzione](contributing.md) — Come partecipare al progetto

## 💡 Guida scelta versione

### Quale versione usare?

**SiliconLife.Default (implementazione standard — verifica fattibilità architetturale):**
- 📌 Scopri questo progetto per la prima volta e vuoi capire rapidamente l'architettura sistema
- 📌 Sei in fase sviluppo e debug e hai bisogno di un metodo esecuzione semplice e diretto

**SiliconLife.Fast (versione alte prestazioni — utilizzo produzione):**
- 📌 Hai bisogno di sfruttamento lungo termine e alte prestazioni
- 📌 Hai grandi volumi dati o requisiti alta concorrenza
- 📌 Vuoi che l'applicazione funzioni in background nella barra di stato sistema

## 🤝 Contribuire

Accogliamo con favore tutti i contributi! Per favore leggi la [guida contribuzione](contributing.md) per maggiori dettagli.

### Flusso lavoro sviluppo
1. Forka questo repository
2. Crea un branch per la funzionalità (`git checkout -b feature/AmazingFeature`)
3. Esegui commit delle modifiche (`git commit -m 'feat: add some AmazingFeature'`)
4. Pusha verso il branch (`git push origin feature/AmazingFeature`)
5. Invia una Pull Request

## 📄 Licenza

Questo progetto è sotto licenza Apache License 2.0 — vedi il file [LICENSE](../../LICENSE).

## 👨‍💻 Autore

**Hoshino Kennji**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Ringraziamenti

Grazie a tutti gli sviluppatori e fornitori piattaforme IA che hanno contribuito a questo progetto.

---

**Silicon Life Collective** — Rendere gli agenti IA veramente "viventi"
