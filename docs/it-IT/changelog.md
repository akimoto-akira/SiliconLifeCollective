# Registro delle modifiche

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [Français](../fr-FR/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | **Italiano**

Tutte le modifiche importanti di questo progetto saranno documentate in questo file.

Il formato è basato su [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
e questo progetto aderisce alla [Gestione semantica delle versioni](https://semver.org/spec/v2.0.0.html).

---

## Informazioni su questo registro delle modifiche

### Doppia versione del progetto

Questo progetto offre due versioni di implementazione :

- **SiliconLife.Default** : Implementazione standard, principalmente per la verifica di fattibilità architettonica. Applicazione console, storage JSON su filesystem.
- **SiliconLife.Fast** : Versione principale di produzione. Applicazione Windows Forms, storage in memoria SpeedyPack + persistenza asincrona, ottimizzazione approfondita delle prestazioni.

Entrambe le versioni condividono le stesse interfacce e funzionalità, differendo solo nell'implementazione dello storage e nella modalità di esecuzione. SiliconLife.Default funge da riferimento per la verifica architettonica, SiliconLife.Fast è la versione principale raccomandata per la produzione.

### Origine del progetto

- Questo progetto è iniziato il 20 marzo 2026.
- Prima di questo progetto, una demo di verifica è fallita a causa di una progettazione architetturale inadeguata, rendendo impossibile l'integrazione con più piattaforme IA.

### Strumenti AI IDE utilizzati

#### Kiro (Amazon AWS)
- Il progetto è stato inizialmente mantenuto da Kiro, avviato in modalità Spec.
- Kiro è un ambiente di sviluppo IA agentic costruito da Amazon AWS.
- Basato su Code OSS (VS Code), supporta le impostazioni VS Code e i plugin compatibili con Open VSX.
- Flusso di sviluppo guidato dalle specifiche per la codifica IA strutturata.

#### Comate AI IDE / 文心快码 (Baidu)
- Utilizzato occasionalmente per la scrittura e la documentazione.
- Comate AI IDE è uno strumento di ambiente di sviluppo nativo IA pubblicato da Baidu Wenxin il 23 giugno 2025.
- Primo IDE IA multimodale e multi-agente collaborativo del settore.
- Funzionalità inclusa la conversione design-codice e la codifica assistita da IA su tutto il flusso.
- Alimentato dal modello Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Utilizzato da ottobre 2025 ad aprile 2026.
- IDE IA con generazione intelligente di codice e gestione di progetto.

#### Qoder (Alibaba)
- Utilizzato per la manutenzione del progetto dal 18 aprile 2026.
- Piattaforma di codifica IA che supporta l'analisi del codice, la generazione di documentazione e la collaborazione multi-agente.

#### CatPaw (Meituan)
- Utilizzato in combinazione con Qoder dal 6 maggio 2026.
- Basato sui modelli LongCat sviluppati internamente da Meituan, con potenti capacità di refactoring completo dell'architettura del codice.

### Documentazione dei requisiti

- La documentazione dei requisiti di questo progetto non è pubblica.
- I requisiti sono stati validati in modo iterativo da oltre 12 piattaforme IA internazionali e grandi serie di modelli, producendo oltre 2000 righe di documentazione dei requisiti guidata dalle user story, quasi incomprensibile per gli umani.

---

## [Non pubblicato]

### 2026-05-12

#### Visualizzazioni Web del sistema attività
- `0891b3c` - Aggiungere visualizzazioni dettaglio e cronologia esecuzione attività
  - Aggiunto TaskExecutionDetailView vista dettaglio esecuzione attività
  - Aggiunto TaskExecutionHistoryView vista cronologia esecuzione attività
  - TaskController aggiunte interfacce di query dettaglio e cronologia esecuzione
  - Aggiunto TaskViewModel modello vista attività
  - TaskCenter centro attività migliorato
  - TaskSystem sistema attività aggiornato
  - 9 lingue localizzazione aggiunti chiavi relative alle attività
  - 26 file modificati, 803 inserimenti(+), 55 cancellazioni(-)

### 2026-05-11

#### Refactoring dell'architettura dei componenti Web
- `5e687ad` - Migrare il rendering dei componenti da stringa a H-tree
  - ComponentBase metodo di rendering migrato dal pattern stringa alla struttura H-tree
  - Tutti i 28 componenti adattati alla nuova architettura di rendering (A, Accordion, Button, Calendar, Card, Chart, ecc.)
  - SelectComponent grande refactoring (889 righe migliorate)
  - Controller e viste aggiornati di conseguenza
  - 33 file modificati, 667 inserimenti(+), 435 cancellazioni(-)

- `bfd332d` - Migrare Style da stringa a stili inline CssBuilder
  - Aggiunto CssBuilder costruttore di stili
  - ComponentBase sistema di stili migrato da stringa a CssBuilder strutturato
  - LoadingComponent significativamente migliorato (103 righe aggiunte)
  - ConfigController, LogController, MemoryController migrazione stili controller
  - ChatView, ConfigView, LogView, MemoryView migrazione stili viste
  - 37 file modificati, 351 inserimenti(+), 157 cancellazioni(-)

#### Ottimizzazione del sistema di archiviazione
- `d67a7ee` - Ottimizzare QueryLatest per grandi set di dati
  - SpeedyTimeStorage QueryLatest ottimizzazione delle prestazioni del metodo
  - SpeedyLoggerProvider provider di logging migliorato
  - 2 file modificati, 44 inserimenti(+), 5 cancellazioni(-)

#### Refactoring del sistema calendario
- `9629f88` - Estrarre TimerExecution e migliorare le viste Web del timer
  - TimerSystem logica TimerExecution estratta (175 righe rimosse)
  - SelectComponent significativamente migliorato (427 righe migliorate)
  - TimerController e viste del timer migliorate
  - ContextManager gestore di contesto aggiornato
  - 12 file modificati, 458 inserimenti(+), 267 cancellazioni(-)

#### Localizzazione
- `5d8ca79` - Aggiungere chiave di localizzazione LogsLoading
  - 9 lingue aggiunta chiave LogsLoading
  - DefaultLocalizationBase classe base aggiunta definizione
  - 11 file modificati, 15 inserimenti(+)

### 2026-05-10

#### Refactoring del sistema attività
- `54394f6` - Unire il sistema attività con i cicli della cronologia chat
  - ProjectTaskSystem sistema attività del progetto significativamente semplificato (411 righe refattorizzate)
  - TaskSystem sistema attività semplificato (254 righe refattorizzate)
  - TaskCenter centro attività refattorizzato (188 righe migliorate)
  - ContextManager gestore di contesto ottimizzato (347 righe refattorizzate)
  - DefaultSiliconBeing essere di silicio migliorato
  - TimerSystem sistema timer integrato con le attività
  - IWorkNoteStorage interfaccia aggiornata
  - SpeedyWorkNoteStorage e FileSystemWorkNoteStorage adattati
  - 16 file modificati, 648 inserimenti(+), 897 cancellazioni(-)

### 2026-05-09

#### Miglioramento dell'interfaccia Web
- `bc50dd7` - Migliorare la vista chat e aggiungere funzionalità di audit
  - Aggiunto AuditController controller di audit (261 righe)
  - Aggiunto AuditView vista di audit (379 righe)
  - Aggiunto AuditViewModel modello vista di audit
  - ChatView vista chat significativamente migliorata (171 righe migliorate)
  - ChatController controller chat aggiornato
  - MarkdownEditorComponent componente migliorato
  - InitController controller di inizializzazione migliorato
  - ChatSystem sistema chat funzionalità aggiunte
  - 14 file modificati, 1030 inserimenti(+), 112 cancellazioni(-)

- `c9babce` - Migliorare il rendering delle chiamate degli strumenti nella vista chat
  - ChatView rendering del blocco di chiamata degli strumenti migliorato
  - 1 file modificato, 54 inserimenti(+), 11 cancellazioni(-)

#### Sistema scenari strumenti IA
- `ff2eddd` - Implementare il sistema di filtraggio scenari strumenti
  - Aggiunto ToolScenarioAttribute attributo scenario strumenti (36 righe)
  - Aggiunto ChatOnlyAttribute attributo scenario solo chat (19 righe)
  - ToolManager gestore strumenti aggiunto filtraggio scenari (40 righe)
  - ContextManager gestore di contesto adattato al filtraggio scenari
  - 4 file modificati, 115 inserimenti(+), 30 cancellazioni(-)

- `5709a33` - Aggiungere attributi scenario alle classi strumenti
  - 24 classi strumenti aggiunte annotazioni attributo ToolScenario
  - Inclusi calendario, chat, configurazione, curatore, database, disco, compilazione dinamica, ecc.
  - 24 file modificati, 46 inserimenti(+), 20 cancellazioni(-)

#### Refactoring del sistema attività
- `2f19a5f` - Ristrutturare il sistema attività con TaskCenter e TaskEnumerator
  - Aggiunto TaskCenter centro attività (235 righe)
  - Aggiunto TaskEnumerator enumeratore attività (297 righe)
  - TaskSystem sistema attività refattorizzato e semplificato
  - DefaultSiliconBeing essere di silicio adattato alla nuova architettura
  - DefaultSiliconBeingFactory fabbrica aggiornata
  - SiliconBeingBase classe base migliorata
  - 7 file modificati, 796 inserimenti(+), 275 cancellazioni(-)

#### Migrazione del sistema permessi
- `a06ed09` - Migrare il sistema IM e permessi al progetto App
  - PermissionRequestQueue migrato da Default/Fast al progetto App (443 righe aggiunte)
  - Rimosso WebUIProvider versione Default (403 righe cancellate)
  - Rimosso HelpTool versione Default (194 righe cancellate)
  - Rimossi PermissionRequestQueue duplicati da Default/Fast
  - Rimosso IMPermissionAskHandler versione Default
  - PermissionRequestController controller aggiornato
  - 14 file modificati, 496 inserimenti(+), 1183 cancellazioni(-)

#### Ottimizzazione del contesto IA
- `4c8aaff` - Ottimizzare il gestore di contesto e migliorare il localizzatore di servizi
  - ContextManager gestore di contesto semplificato e ottimizzato
  - ServiceLocator localizzatore di servizi migliorato (36 righe aggiunte)
  - ToolManager gestore strumenti migliorato (34 righe aggiunte)
  - DashScopeClient e VolcengineArkClient client migliorati
  - Esecutori (CommandLine, Disk, Network) aggiornati
  - 8 file modificati, 116 inserimenti(+), 98 cancellazioni(-)

#### Localizzazione
- `5c5eef7` - Aggiungere chiavi di localizzazione audit e attività
  - DefaultLocalizationBase aggiunte 127 righe di definizioni di localizzazione
  - 9 lingue aggiunte chiavi relative ad audit e attività (26 righe ciascuna)
  - 11 file modificati, 387 inserimenti(+)

#### Configurazione del progetto
- `2067db6` - Aggiornare le configurazioni del progetto e le regole gitignore
  - Regole .gitignore aggiornate
  - DefaultConfigData e Fast DefaultConfigData configurazione migliorate
  - SpeedyWorkNoteStorage archiviazione migliorata
  - SpeedyPack nucleo migliorato
  - 5 file modificati, 32 inserimenti(+), 6 cancellazioni(-)

### 2026-05-07

#### Localizzazione italiana
- `8adc18c` - Aggiungere il supporto alla localizzazione italiana e aggiornare la documentazione multilingue
  - Aggiunta localizzazione it-IT italiana
  - Aggiunta implementazione localizzazione ItIT (1909 righe)
  - Aggiunto ChineseHistoricalItIT supporto italiano calendario storico cinese (586 righe)
  - Aggiunta TrayItIT localizzazione italiana vassoio di sistema (135 righe)
  - Aggiunto set completo di documentazione italiana (14 documenti: README, riferimento API, architettura, sistema calendario, registro modifiche, guida contributi, ecc.)
  - Aggiornate architettura, guida sviluppo, guida introduttiva, ecc. per tutte le versioni linguistiche
  - Language enumerazione lingue aggiunto italiano
  - 86 file modificati, 11573 inserimenti(+), 769 cancellazioni(-)

#### Sincronizzazione della documentazione
- `12a5deb` - Aggiornare la documentazione multilingue per architettura, registro modifiche e guida dell'essere di silicio
  - 8 lingue README aggiornato
  - 8 lingue documentazione architettura aggiornata
  - 8 lingue registro modifiche aggiornato
  - 8 lingue guida dell'essere di silicio aggiornata
  - 8 lingue riferimento strumenti aggiornato
  - Glossario ristrutturato
  - 46 file modificati, 1697 inserimenti(+), 442 cancellazioni(-)

### 2026-05-06

#### Refactoring massivo dei moduli
- `eeb3be6` - Refactoring e riorganizzazione massiva dei moduli
  - Ristrutturazione del progetto SiliconLife.App
  - Riorganizzazione del progetto SiliconLife.Fast
  - Riorganizzazione del progetto SiliconLife.Default
  - Riorganizzazione dei moduli condivisi SiliconLife.Common
  - Riorganizzazione dei moduli principali SiliconLife.Core
  - Riorganizzazione del motore di storage SiliconLife.Speedy
  - Riorganizzazione degli strumenti di gestione SiliconLife.Speedy.Manager
  - 119 file modificati, 6926 righe aggiunte, 3066 righe eliminate

### 2026-05-04

#### Client IA
- `24d2c86` - Aggiunta di VolcengineArkClient e sostituzione di Audit con Usage tracking
  - Nuovo client AI VolcengineArkClient Volcengine Ark
  - Supporto delle modalità streaming e non-streaming
  - Controllo di velocità doppio integrato (controllo proprio + limitazione del server)
  - Compatibile con il protocollo API OpenAI
  - Sostituzione del sistema Audit con Usage tracking
  - 24 file modificati, 802 righe aggiunte, 21 righe eliminate

#### Sistema di strumenti
- `f27650a` - Aggiunta dello strumento di ricaricamento a caldo per il riavvio automatico di Fast
  - Nuovo strumento HotReloadTool di ricaricamento a caldo
  - Supporto della compilazione in linea, dell'aggiornamento e del riavvio di SiliconLife.Fast
  - Nuovo programma di aggiornamento autonomo HotReload.exe
  - Meccanismo di copia sicura dei file (non si sovrascrive)
  - Chiusura gentile e attesa del rilascio della porta
  - 9 file modificati, 581 righe aggiunte

#### Localizzazione
- `6a5aad8` - Aggiornamento di tutti i file e aggiunta del supporto di localizzazione francese
  - Nuova localizzazione fr-FR francese
  - Aggiornamento di tutte le versioni linguistiche
  - Traduzione francese della documentazione di aiuto
  - Traduzione francese dell'interfaccia
  - 100+ file modificati

### 2026-05-03

#### Infrastruttura del progetto
- `2664b0c` - Aggiornamento dell'infrastruttura del progetto e delle dipendenze
  - SiliconLife.Speedy.Manager aggiunge un'interfaccia di gestione WPF (MainForm.Designer.cs, MainForm.resx)
  - Nuova risorsa icona slc.ico (1.5 MB)
  - PluginLoader migliora considerevolmente lo scan di sicurezza (622 righe aggiunte)
  - Nuova PermissionedStreamFactory fabbrica di stream con permessi (779 righe)
  - Nuova PermissionRequestQueue coda di richieste di permessi (versioni Default e Fast)
  - Nuovo DebugLoggerProvider fornitore di log di debug
  - ConfigDataBase classe base di configurazione migliorata
  - ToolManager aggiunge la funzionalità di scan degli strumenti plugin (ScanAllPluginAssemblies)
  - SiliconBeingManager gestione del ciclo di vita migliorata
  - DashScopeClient client AI Alibaba Cloud considerevolmente migliorato (227 righe aggiunte)
  - DefaultSiliconBeingFactory fabbrica migliorata
  - Aggiornamento delle viste e dei controller Web (ChatView, WorkNoteView, PermissionRequestController)
  - 9 lingue di localizzazione aggiungono nuove chiavi
  - 35 file modificati, 28080 righe aggiunte, 336 righe eliminate

### 2026-05-02

#### Miglioramento del client IA
- `c16f99f` - Aggiornamento del client IA, dell'interfaccia Web e dei componenti di storage
  - DashScopeClient client Alibaba Cloud considerevolmente migliorato
  - SpeedyPackAutoCompactor ottimizzazione del compattatore automatico
  - ViewBase di vista Web e BeingView migliorati
  - 6 file modificati, 240 righe aggiunte, 81 righe eliminate

#### Sistema di plugin
- `242dc98` - Aggiunta della lista dei plugin nella pagina Informazioni
  - AboutController aggiunta della visualizzazione delle informazioni plugin
  - AboutViewModel aggiunta del modello dati plugin
  - AboutView aggiunta del rendering della lista plugin
  - 9 lingue di localizzazione aggiungono le chiavi relative ai plugin
  - 14 file modificati, 160 righe aggiunte, 1 riga eliminata

#### Ottimizzazione IA
- `147f8f4` - Semplificazione del testo di prompt della memoria contestuale
  - ContextManager ottimizzazione dei prompt IA
  - 1 file modificato, 1 riga aggiunta, 1 riga eliminata

#### Ottimizzazione storage Speedy
- `8bda2d3` - Aggiornamento dello storage Speedy e dell'implementazione del controller di memoria
  - SpeedyPackAutoCompactor correzione dell'intervallo
  - SpeedyTimeStorage ottimizzazione della gestione dei percorsi
  - MemoryController miglioramento del controller di memoria
  - SpeedyPack.Manager aggiornamento dell'interfaccia
  - 4 file modificati, 21 righe aggiunte, 18 righe eliminate

#### Miglioramento barra di sistema
- `8972654` - Miglioramento del supporto di localizzazione della finestra di stato della barra di sistema
  - 9 lingue di localizzazione della barra di sistema aggiungono la voce di gestione Speedy
  - TrayStatusWindow aggiunta di una voce di menu di gestione Speedy
  - 11 file modificati, 72 righe aggiunte

#### Ottimizzazione Speedy.Manager
- `6f5db09` - Ottimizzazione dell'interfaccia SpeedyPack Manager e dei componenti interni
  - Rifacimento dell'interfaccia MainForm
  - Ottimizzazione della gestione memoria FreeList
  - Miglioramento della coda di scrittura WriteQueue
  - Ottimizzazione del core SpeedyPack
  - 5 file modificati, 96 righe aggiunte, 88 righe eliminate

#### Miglioramento del sistema di storage
- `57f9d5d` - Miglioramento del sistema di storage, aggiunta della compattazione automatica e del supporto delle date incomplete
  - Nuovo SpeedyPackAutoCompactor timer di compattazione automatica (intervallo di 30 minuti)
  - SpeedyPackRegistry gestore singleton migliorato
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage adattamenti migliorati
  - SpeedyPack aggiunta di FreeList gestione spazio libero (149 righe)
  - PackFileWriter riscrittura e ottimizzazione
  - WriteOperation, WriteQueue coda di scrittura migliorata
  - SpeedyPackOptions estensione delle opzioni di configurazione
  - IncompleteDate aggiunta di metodi di confronto
  - PluginLoader miglioramento del caricatore di plugin
  - Aggiornamento del flusso di inizializzazione di Program.cs per le versioni Default e Fast
  - DefaultConfigData semplificazione dei dati di configurazione
  - KnowledgeNetwork razionalizzazione della rete di conoscenze
  - ChatController, MemoryController ottimizzazione dei controller
  - SpeedyPack.Manager MainForm miglioramento delle funzionalità
  - 22 file modificati, 639 righe aggiunte, 253 righe eliminate

#### Aggiornamento Speedy.Manager
- `b04ed33` - Aggiornamento dei file Speedy.Manager

### 2026-05-01

#### Rifacimento architetturale : Speedy Storage sostituisce LiteDB
- `6600972` - Sostituzione di LiteDB con Speedy Storage, aggiunta del sistema di plugin e del progetto Speedy
  - **Nuovo progetto SiliconLife.Speedy** : Motore di storage .spk ad alte prestazioni
    - SpeedyPack classe principale (489 righe) : mappatura directory in memoria + cache di voci + coda di scrittura asincrona
    - SpeedyPackOptions classe di configurazione : TTL cache, numero max voci in cache, modalità sola lettura
    - IPackTransaction interfaccia di transazione : supporto operazioni di scrittura atomiche
    - SpkFileInfo classe informazioni file
    - Directory Internal : DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Dipendenza MessagePack 3.1.4 per serializzazione binaria (compressione LZ4)
  - **Nuovo progetto SiliconLife.Speedy.Manager** : Strumento di gestione WPF
    - Architettura MVVM : MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel, ecc.
    - Strati di servizi : PackService, FileDialogService, RecentFilesService, NotificationService
    - Convertitori : BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - Viste : MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - Dialoghi : FileInfoDialog, ImportDialog, NewEntryDialog
  - **Migrazione dello storage SiliconLife.Fast** : LiteDB → SpeedyPack
    - Nuovo SpeedyStorage (adattatore IStorage)
    - Nuovo SpeedyTimeStorage (adattatore ITimeStorage)
    - Nuovo SpeedyWorkNoteStorage (adattatore IWorkNoteStorage)
    - Nuovo SpeedyPackRegistry (gestore singleton a livello di processo)
    - Nuovo SpeedyPackAutoCompactor (timer di compattazione automatica)
    - Eliminazione delle implementazioni di storage LiteDB (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Eliminazione del codice relativo alla finestra di gestione LiteDB
  - **Sistema di plugin** :
    - Nuova interfaccia IPlugin (Core/Plugins/IPlugin.cs)
    - Nuovo PluginLoader caricatore di plugin (Core/Plugins/PluginLoader.cs)
    - Supporto del caricamento di DLL di plugin da una directory
    - Scan di sicurezza : verifica dei namespace proibiti (System.IO, System.Net, Microsoft.CodeAnalysis, ecc.)
    - Whitelist di assembly fidati (Google.Protobuf, Newtonsoft.Json, MessagePack, ecc.)
    - Caricamento isolato tramite AssemblyLoadContext personalizzato
    - ToolManager aggiunge il metodo ScanAllPluginAssemblies
    - CoreHost integrazione del caricatore di plugin
  - 119 file modificati, 6926 righe aggiunte, 3066 righe eliminate

#### Miglioramento dei Silicon Beings
- `3aef4c3` - Aggiunta dello stato di attività Stopped e miglioramento della gestione degli errori
  - Nuovo stato Stopped per i Silicon Beings
  - Miglioramento della gestione degli errori e del meccanismo di recupero

#### Aggiornamento della localizzazione
- `513c65d` - Aggiornamento di tutte le versioni linguistiche e della documentazione
  - Nuovo componente MarkdownEditorComponent (625 righe)
  - Nuovo componente DetailsComponent (130 righe)
  - Nuovo componente AccordionComponent (285 righe)
  - BeingController, ChatController, MemoryController, PermissionController aggiornamento dei controller
  - BeingView, ChatView, MemoryView, SoulEditorView rifacimento delle viste
  - Eliminazione del vecchio MarkdownEditorView
  - InitController migrazione verso i componenti
  - 115 file modificati, 5761 righe aggiunte, 2362 righe eliminate

### 2026-04-30

#### Funzionalità della barra di sistema
- `101b203` - Implementazione della finestra di stato della barra di sistema e ApplicationContext
  - Nuove risorse icone barra di sistema (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implementazione di TrayStatusWindow finestra di stato
  - Supporto di localizzazione della barra di sistema in 9 lingue (TrayCsCZ, TrayDeDE, TrayEnUS, ecc.)
  - TrayLocalizationBase classe base astratta
  - 24 file modificati, 27995 righe aggiunte, 1 riga eliminata (incluse le risorse)

#### Architettura UI a componenti
- `e61cfaa` - Completamento dell'architettura UI a componenti, implementazione di 24 componenti
  - Fase MVP (8) : ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Seconda fase (6) : Accordion, Card, Tabs, Table, Modal, Message
  - Terza fase (5) : Calendar, Tree, Chart, FileUpload, RichText
  - Nuove classi ausiliarie Js, Behavior, DomUpdate
  - 25 file modificati, 2666 righe aggiunte

- `7449e51` - Miglioramento del sistema di componenti e aggiunta di nuovi temi skin
  - Miglioramento dei componenti A, Button, Div, Form, Input, ecc.
  - 3 nuovi temi skin : HighContrast (contrasto elevato), Light (chiaro), Minimal (minimalista)
  - Aggiornamento degli skin esistenti (Admin, Chat, Creative, Dev)
  - InitController migrazione verso i componenti
  - 32 file modificati, 1466 righe aggiunte, 1238 righe eliminate

- `1ba8636` - Inizio della migrazione di InitController in componenti (in corso)
  - 9 file modificati, 574 righe aggiunte, 145 righe eliminate

#### Unificazione del sistema di storage
- `895dff9` - Unificazione di soul.md e state.json per utilizzare l'interfaccia IStorage
  - DefaultSiliconBeing utilizza IStorage per leggere/scrivere i file anima e di stato
  - Nuovo StateFileManager gestore di file di stato
  - SoulFileManager rifacimento per adattare IStorage
  - 8 file modificati, 201 righe aggiunte, 116 righe eliminate

#### Miglioramento gestione LiteDB
- `a34bef4` - Aggiunta di LiteDBManager e miglioramento della localizzazione della barra di sistema
  - Menu della barra di sistema aggiunta di una voce di gestione LiteDB
  - Aggiornamento della localizzazione della barra di sistema in 9 lingue
  - 10 file modificati, 196 righe aggiunte

- `c4a79ca` - Aggiunta di una fabbrica di localizzazione sensibile alla lingua per la finestra di gestione LiteDB
  - 1 file modificato, 78 righe aggiunte

- `5ebc55e` - Conversione di LiteDBAdminLocalization in classe base astratta
  - 10 file modificati, 1356 righe aggiunte

#### Correzione del sistema di configurazione
- `2da5256` - Aggiunta del metodo astratto ConfigExists e correzione dei record di configurazione LiteDB duplicati
  - ConfigDataBase aggiunta del metodo ConfigExists
  - La versione Fast di DefaultConfigData implementa la verifica di esistenza della configurazione LiteDB
  - Correzione del problema di chiavi di configurazione LiteDB duplicate
  - 9 file modificati, 210 righe aggiunte, 2 righe eliminate

#### Ottimizzazione della chat e delle viste
- `d3618ec` - Ottimizzazione delle sessioni di chat, del sistema di storage, del modello temporale e della classe base delle viste
  - BroadcastChannel, GroupChatSession, SingleChatSession ottimizzati
  - ITimeStorage aggiunta di metodi di query
  - FileSystemStorage e LiteDBStorage aggiornati in modo sincrono
  - ViewBase rifacimento e ottimizzazione (versioni Default e Fast)
  - 11 file modificati, 622 righe aggiunte, 392 righe eliminate

### 2026-04-29

#### Rifacimento architetturale : Estrazione del modulo condiviso
- `a102428` - Migrazione dei moduli condivisi da SiliconLife.Default a SiliconLife.Common
  - Estrazione di 32 implementazioni di calendari verso il progetto Common
  - Estrazione delle classi base di localizzazione e di 21 implementazioni linguistiche verso il progetto Common
  - Estrazione del gestore di permessi e dell'implementazione predefinita del Silicon Being verso il progetto Common
  - Estrazione di 23 implementazioni di strumenti integrati verso il progetto Common
  - Estrazione dell'implementazione Playwright WebView verso il progetto Common
  - Aggiornamento dei namespace verso SiliconLife.Collective
  - 122 file modificati, 586 righe aggiunte, 343 righe eliminate

#### Miglioramento della qualità del codice
- `17566fe` - Sostituzione di Console.WriteLine con il sistema di logging nei progetti Core, Common e Default
  - ContextManager, AuditLogger, DefaultConfigData e altri 6 file aggiornati
  - Unificazione dell'uso dell'interfaccia ILogger, miglioramento della manutenibilità del codice
  - 6 file modificati, 12 righe aggiunte, 8 righe eliminate

#### Versione ad alte prestazioni SiliconLife.Fast
- `54a0307` - Aggiunta del progetto SiliconLife.Fast e correzione della compilazione
  - Punto di ingresso completo dell'applicazione Windows Forms
  - Supporto della barra di sistema (NotifyIcon)
  - Porting di tutti i controller Web UI (20+)
  - Porting di tutti i componenti di vista Web
  - Porting di 4 temi skin (Admin, Chat, Creative, Dev)
  - 125 file modificati, 61186 righe aggiunte

#### Sincronizzazione della documentazione multilingua
- `265fde8` - Sincronizzazione della documentazione di architettura a doppia versione verso tutte le lingue
  - Aggiornamento di architecture.md, changelog.md in 7 lingue
  - Aggiornamento di contributing.md in 6 lingue
  - Aggiornamento di getting-started.md, roadmap.md in 7 lingue
  - 47 file modificati, 1214 righe aggiunte, 38 righe eliminate

#### Sistema di storage LiteDB (versione Fast)
- `4704862` - Aggiunta delle dipendenze e dell'infrastruttura LiteDB
  - Nuovo LiteDBManager classe di gestione
  - Nuovo LiteDBModels modello dati
  - 3 file modificati, 252 righe aggiunte

- `4220036` - Implementazione delle classi di storage LiteDB
  - LiteDBStorage : implementazione dell'interfaccia IStorage
  - LiteDBTimeStorage : implementazione dell'interfaccia ITimeStorage
  - LiteDBWorkNoteStorage : implementazione dell'interfaccia IWorkNoteStorage
  - 3 file modificati, 581 righe aggiunte

- `38ebd23` - Migrazione della configurazione e del sistema di logging verso LiteDB
  - DefaultConfigData adattamento allo storage LiteDB
  - Nuovo LiteDBLoggerProvider fornitore di log
  - 2 file modificati, 203 righe aggiunte, 67 righe eliminate

- `e687157` - Migrazione della rete di conoscenze dal filesystem a LiteDB
  - KnowledgeNetwork rifacimento completo, utilizzo di LiteDB per lo storage delle triplette
  - 1 file modificato, 231 righe aggiunte, 72 righe eliminate

- `4220169` - Integrazione dello storage LiteDB in Program e ProjectManager
  - Program.cs inizializzazione dello storage LiteDB
  - ProjectManager adattamento allo storage di note di lavoro LiteDB
  - 2 file modificati, 40 righe aggiunte, 17 righe eliminate

- `5f3a709` - Eliminazione delle implementazioni di storage file obsolete
  - Eliminazione di FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage, ecc.
  - 6 file modificati, 1518 righe eliminate

- `e1a4ef2` - docs: aggiunta dell'identificatore di versione v0.1.0-alpha a tutta la documentazione
  - 127 file modificati, 2297 righe aggiunte, 2471 righe eliminate

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Rifacimento del sistema di storage
- `8dd26e3` - Unificazione dell'interfaccia ITimeStorage con IncompleteDate e aggiunta di API di query gerarchica
  - Eliminazione dei metodi di overload DateTime dell'interfaccia ITimeStorage, unificazione con IncompleteDate
  - IncompleteDate aggiunta del metodo di confronto CompareTo(DateTime) e del metodo Expand()
  - Nuove API di query gerarchica GetEarliestTimestamp(), GetLatestTimestamp()
  - Nuovi metodi HasSummary() e QueryWithLevel(), supporto di query per livello temporale
  - Memory.cs rifacimento dell'algoritmo di compressione, utilizzo della nuova API di query gerarchica per migliorare l'efficienza
  - FileSystemTimeStorage.cs implementazione completa dei nuovi metodi dell'interfaccia
  - Aggiornamento sincrono di tutti i chiamanti : ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, ecc.
  - Aggiornamento del sistema di strumenti : HelpTool, LogTool, TokenAuditTool adattamento alla nuova interfaccia
  - Aggiornamento dei controller Web : AuditController, ChatController, ChatHistoryController adattamento alla nuova interfaccia
  - 41 file modificati, 1820 righe aggiunte, 903 righe eliminate

### 2026-04-27

#### Miglioramento del sistema di documentazione di aiuto
- `9989d79` - Aggiornamento della localizzazione, del sistema di aiuto e delle viste Web
  - Nuovo IAIClientFactoryHelp.cs interfaccia di documentazione di aiuto della fabbrica di client IA
  - Traduzione completa di tutta la documentazione di aiuto in 9 lingue
  - HelpTopics.cs aggiunta di 40 definizioni di argomenti di aiuto
  - Aggiornamento completo delle viste Web : InitController, AuditView, ConfigView, KnowledgeView, LogView, ecc.
  - Miglioramento del sistema di localizzazione : aggiunta di nuove chiavi di localizzazione per tutte le versioni linguistiche
  - Aggiornamento della fabbrica di client IA : miglioramento di DashScopeClientFactory, OllamaClientFactory
  - 30 file modificati, 10086 righe aggiunte, 15 righe eliminate

#### Nuovo contenuto della documentazione di aiuto
- `e7afe94` - Aggiunta della documentazione di aiuto sui file anima e sui log di audit
  - Nuova documentazione di aiuto sulla gestione dei file anima
  - Nuova documentazione di aiuto sui log di audit
  - HelpTopics.cs aggiunta di definizioni di argomenti
  - HelpView.cs rifacimento importante, miglioramento della logica di rendering della documentazione
  - PermissionView.cs rifacimento, miglioramento dell'interfaccia di gestione dei permessi
  - Miglioramento dei moduli principali : SiliconBeingManager, TaskSystem, ToolManager
  - TaskTool.cs rifacimento, miglioramento della gestione dei compiti
  - Aggiornamento completo delle viste Web : tutti i componenti di vista aggiornati in modo sincrono
  - HelpController.cs semplificazione, ottimizzazione della logica del controller
  - 30 file modificati, 7100 righe aggiunte, 897 righe eliminate

### 2026-04-26

#### Sistema di documentazione di aiuto
- `07895d7` - Miglioramento del sistema di documentazione di aiuto, aggiunta di 3 documenti e traduzione in 9 lingue
  - Aggiunta delle guide del sistema di memoria, dell'installazione Ollama e dell'uso della piattaforma Alibaba Cloud Bailian
  - Traduzione completa di 10 documenti di aiuto in 9 lingue
  - Semplificazione della logica di rendering di HelpView
  - 18 file modificati, 14418 righe aggiunte, 1364 righe eliminate

#### Localizzazione tedesca
- `0cfd8a1` - Aggiunta del supporto completo di localizzazione tedesca (de-DE)
  - File di localizzazione tedesca completo
  - Aggiunta del supporto tedesco per il calendario storico cinese
  - Aggiunta della traduzione tedesca della documentazione di aiuto
  - Sincronizzazione completa di tutta la documentazione in 9 lingue
  - 135 file modificati, 26186 righe aggiunte, 14371 righe eliminate

#### Sincronizzazione della documentazione
- `3aada7d` - Sincronizzazione della documentazione cinese tradizionale (zh-HK) con il cinese semplificato
  - 3 file modificati, 519 righe aggiunte, 422 righe eliminate
- `2f6abff` - Aggiunta della localizzazione del nome visualizzato dello strumento di aiuto per tutte le lingue
  - 7 file modificati, 47 righe aggiunte, 7 righe eliminate

#### Rifacimento del sistema di conoscenze
- `60944fe` - Unificazione del namespace verso SiliconLife.Collective
  - 8 file modificati, 5 righe aggiunte, 8 righe eliminate
- `69c51c5` - Aggiunta del sistema di documentazione di aiuto e traduzione dei commenti del codice in inglese
  - 29 file modificati, 3385 righe aggiunte, 22 righe eliminate

### 2026-04-25

#### Automazione browser WebView
- `41757c3` - Implementazione dell'automazione del browser WebView multipiattaforma basata su Playwright
  - 6 file modificati, 1152 righe aggiunte

#### Aggiornamento della documentazione
- `0ff797b` - Aggiunta della documentazione KnowledgeTool e WorkNoteTool (7 lingue)
  - 28 file modificati, 4983 righe aggiunte
- `ad77415` - Aggiornamento di tutti i file changelog, aggiunta dei record di cronologia Git del 2026-04-25
  - 7 file modificati, 168 righe aggiunte

#### Gestione dello spazio di lavoro di progetto
- `785c551` - Implementazione della gestione dello spazio di lavoro di progetto, con note di lavoro e sistema di compiti
  - Nuovo sistema di gestione dello spazio di lavoro di progetto
  - Funzionalità di note di lavoro per monitorare l'avanzamento del progetto
  - Integrazione del sistema di gestione dei compiti
  - 29 file modificati, 4256 righe aggiunte, 36 righe eliminate

#### Localizzazione ceca
- `b4bbf39` - Aggiunta della localizzazione ceca completa (cs-CZ) e aggiornamento di tutta la documentazione linguistica
  - 116 file modificati, 4933 righe aggiunte, 222 righe eliminate
- `faf078f` - Correzione degli errori di compilazione della localizzazione ceca
  - 3 file modificati, 910 righe aggiunte, 1 riga eliminata

#### Miglioramento del sistema di conoscenze
- `20adaac` - Aggiunta di KnowledgeTool e supporto di localizzazione completo
  - 34 file modificati, 2331 righe aggiunte, 56 righe eliminate

### 2026-04-24

#### Miglioramento del sistema di gestione della memoria
- `c7b2ecc` - Miglioramento della gestione della memoria, aggiunta del filtraggio avanzato, delle statistiche e della vista dettagliata
  - Nuova funzionalità di filtraggio avanzato della memoria
  - Implementazione della funzionalità di statistiche della memoria
  - Aggiunta della pagina di vista dettagliata della memoria
  - Supporto di localizzazione multilingua (6 lingue)
  - 13 file modificati, 840 righe aggiunte, 86 righe eliminate

#### Estensione del sistema di permessi
- `4489ad6` - Aggiunta del servizio meteo wttr.in alla whitelist di rete
  - Aggiornamento sincrono completo della documentazione multilingua (6 lingue)
  - 14 file modificati, 417 righe aggiunte, 1 riga eliminata

#### Correzione dell'interfaccia Web
- `d9d72e9` - Correzione del problema di priorità CSS del modale di dettaglio delle note di lavoro
  - 19 file modificati, 1744 righe aggiunte, 6 righe eliminate

#### Ottimizzazione della cronologia chat
- `db48c51` - Aggiunta della coda di messaggi di chat, dei metadati di file e del supporto di annullamento dello stream
  - 4 file modificati, 357 righe aggiunte

#### Supporto upload file
- `28fb344` - Implementazione del dialogo di origine file e del supporto upload file
  - 3 file modificati, 1100 righe aggiunte, 2 righe eliminate
- `1d3e2cc` - Aggiunta delle stringhe di localizzazione del dialogo di origine file (6 lingue)
  - 6 file modificati, 30 righe aggiunte

#### Aggiornamento della documentazione
- `8111e92` - Aggiunta del link Wiki nella sezione repository del README
  - 1 file modificato, 3 righe aggiunte, 1 riga eliminata

### 2026-04-22

#### Localizzazione della documentazione
- `66c11eb` - Traduzione dei commenti cinesi in inglese e aggiornamento di tutti i changelog
  - 11 file modificati, 373 righe aggiunte, 163 righe eliminate

#### Miglioramento dei messaggi SSE
- `b574b2b` - Aggiunta di senderName per i messaggi storici per l'identificazione IA
  - 1 file modificato, 9 righe aggiunte

#### Funzionalità di chat
- `601fc14` - Aggiunta dell'operazione mark_read per la marcatura di fine sessione
  - 7 file modificati, 196 righe aggiunte, 36 righe eliminate

#### Ottimizzazione del sistema di strumenti
- `7a03a19` - Miglioramento della flessibilità di query di conversazione di LogTool
  - 1 file modificato, 57 righe aggiunte, 24 righe eliminate

#### Miglioramento della localizzazione
- `0a8d750` - Aggiunta di un prompt di sistema generico per il comportamento proattivo dei Silicon Beings
  - 8 file modificati, 460 righe aggiunte, 48 righe eliminate

#### Rifacimento del sistema di logging
- `2b771f3` - Disaccoppiamento di LogController dalle I/O file, aggiunta dell'API di lettura dei log
  - 4 file modificati, 172 righe aggiunte, 137 righe eliminate
- `12da302` - Aggiunta di un filtro per Silicon Being per la vista dei log
  - 9 file modificati, 147 righe aggiunte, 10 righe eliminate
- `8f6cb1e` - Aggiunta del parametro beingId all'interfaccia ILogger, separazione dei log sistema/Silicon Beings
  - 47 file modificati, 524 righe aggiunte, 490 righe eliminate

#### Miglioramento del sistema di permessi
- `4c747ad` - Rifacimento di PermissionTool, ExecuteCodeTool, aggiunta dell'API EvaluatePermission
  - 18 file modificati, 680 righe aggiunte, 492 righe eliminate

#### Correzioni di bug
- `1c96e99` - Correzione del fallimento di search_files e search_content durante la ricerca nella directory radice
  - 1 file modificato, 98 righe aggiunte, 41 righe eliminate

#### Integrazione degli strumenti
- `135710d` - Eliminazione di SearchTool, spostamento della ricerca locale in DiskTool
  - 2 file modificati, 185 righe aggiunte, 365 righe eliminate

#### Estensione del sistema di strumenti
- `70ce7fb` - Implementazione di DatabaseTool per le query di database strutturate
  - 1 file modificato, 382 righe aggiunte
- `be29a09` - Implementazione di LogTool per le query di cronologia delle operazioni e delle conversazioni
  - 1 file modificato, 298 righe aggiunte
- `4ea7702` - Implementazione di PermissionTool per la gestione dinamica dei permessi
  - 1 file modificato, 457 righe aggiunte
- `1384ff4` - Implementazione di ExecuteCodeTool per l'esecuzione di codice multi-linguaggio
  - 1 file modificato, 477 righe aggiunte
- `82d1e11` - Implementazione di SearchTool per la ricerca di informazioni
  - 1 file modificato, 363 righe aggiunte

#### Ottimizzazione dell'interfaccia Web
- `0675c45` - Ottimizzazione della colorazione dei blocchi di codice markdown nel pannello di anteprima
  - 1 file modificato, 4 righe aggiunte, 23 righe eliminate
- `702b3f3` - Miglioramento della vista dei compiti, aggiunta di badge di stato e visualizzazione metadati
  - 8 file modificati, 221 righe aggiunte, 9 righe eliminate
- `6ed9a79` - Miglioramento dello storage dei messaggi di chat e del rendering delle viste
  - 8 file modificati, 140 righe aggiunte, 29 righe eliminate

### 2026-04-21

#### Correzioni di bug
- `c6b518b` - Correzione della trasmissione dei messaggi del timer e dello storage dei messaggi di chat
  - 3 file modificati, 297 righe aggiunte, 124 righe eliminate

#### Gestione della configurazione
- `4305769` - Aggiunta di .gitattributes per la gestione delle terminazioni di riga
  - 1 file modificato, 32 righe aggiunte

#### Miglioramenti dell'interfaccia Web
- `188c6f8` - Registrazione della route API della lista dei compiti e aggiunta della visualizzazione stato vuoto
  - 2 file modificati, 35 righe aggiunte, 2 righe eliminate
- `634e8ca` - Aggiunta di un link di ritorno alla lista sulla pagina dei permessi
  - 1 file modificato, 16 righe aggiunte
- `6ba591d` - Aggiunta di un editor di configurazione IA indipendente per i Silicon Beings
  - 11 file modificati, 842 righe aggiunte, 18 righe eliminate
- `0a826f5` - Aggiunta di una notifica di salvataggio riuscito nell'editor di codice
  - 1 file modificato, 9 righe aggiunte, 2 righe eliminate
- `2940373` - Miglioramento dell'interfaccia Web, aggiunta di suggerimenti di codice e miglioramenti UI
  - 11 file modificati, 1054 righe aggiunte, 75 righe eliminate

#### Correzione del sistema di permessi
- `592c7ab` - Correzione dell'istanziazione dei callback e dell'ordine di registrazione
  - 2 file modificati, 38 righe aggiunte, 7 righe eliminate

#### Miglioramento della sicurezza
- `833ead2` - Aggiunta della validazione dei riferimenti assembly per la compilazione dinamica
  - 4 file modificati, 135 righe aggiunte, 8 righe eliminate

#### Miglioramento del sistema di permessi
- `5879621` - Aggiunta della pre-validazione di compilazione dei callback di permesso e miglioramento della gestione degli errori
  - 21 file modificati, 617 righe aggiunte, 26 righe eliminate

#### Aggiornamento della documentazione
- `4dbf659` - Aggiornamento del changelog a v0.5.1, sostituzione degli URL GitHub placeholder, aggiunta del mirror Gitee, localizzazione del nome Bilibili per lingua, aggiornamento dell'email
  - 32 file modificati, 489 righe aggiunte, 180 righe eliminate

#### Configurazione e punto di ingresso
- `0fc1693` - Aggiornamento del punto di ingresso del programma e della configurazione del progetto
  - 2 file modificati, 7 righe aggiunte

#### Rifacimento del sistema di permessi
- `ea9179a` - Miglioramento dell'implementazione del sistema di permessi
  - 5 file modificati, 358 righe aggiunte, 152 righe eliminate

#### Correzioni di bug
- `928a96d` - Correzione dell'implementazione del calcolo del calendario
  - 4 file modificati, 12 righe aggiunte, 12 righe eliminate

#### IA e calendario
- `646813e` - Miglioramento dell'implementazione della fabbrica di client IA
  - 2 file modificati, 21 righe aggiunte, 20 righe eliminate

#### Localizzazione
- `7940d9c` - Aggiunta del supporto di localizzazione coreana
  - 7 file modificati, 2424 righe aggiunte, 10 righe eliminate
- `4ff98ad` - Rifacimento della documentazione, supporto multilingua
  - 81 file modificati, 23818 righe aggiunte, 1886 righe eliminate

### 2026-04-20

#### Miglioramento delle funzionalità principali
- `28905b5` - Supporto multilingua completo, fabbrica di client IA, sistema di permessi e impostazioni di localizzazione
  - Sistema di logging con gestore, voci e diversi livelli di logging
  - Sistema di audit dei token per interrogare e monitorare l'utilizzo dei token
  - Fabbrica di client IA con scoperta automatica di diverse piattaforme IA
  - Sistema di callback di permessi con proprio storage
  - Implementazione del logger console
  - Supporto multilingua inglese e cinese semplificato
  - WebUI messenger con WebSocket per chat in tempo reale
  - Miglioramento del Silicon Being predefinito con localizzazione
  - 39 file modificati, 4670 righe aggiunte, 175 righe eliminate

### 2026-04-19

#### Timer e calendari
- `c933fd8` - Aggiornamento della localizzazione, del sistema di timer, delle viste Web e aggiunta di strumenti
  - Migliore gestore di localizzazione
  - Sistema di pianificazione dei compiti temporizzati
  - Configurazione IA e gestione del contesto
  - Strumento calendario che supporta 32 tipi di calendari
  - Controller Web per l'API calendario
  - Strumento di gestione dei compiti
  - 46 file modificati, 4018 righe aggiunte, 975 righe eliminate

**Miglioramenti architetturali**
- Rifacimento dell'architettura delle viste Web per supportare meglio gli skin
- Miglioramento del sistema di gestione dei Being con una migliore gestione degli stati

### 2026-04-18

- `9f585e1` - Aggiornamento della localizzazione, del sistema di timer, delle viste Web e aggiunta di strumenti
  - Miglioramenti dei timer e della pianificazione
  - Migliori viste Web con componenti UI migliorati
  - Più implementazioni di strumenti
  - 57 file modificati, 3328 righe aggiunte, 389 righe eliminate

### 2026-04-17

- `9b71fcd` - Aggiornamento dei moduli principali, aggiunta della documentazione zh-HK, dei canali di broadcast, dello strumento di configurazione e della vista Web di audit
  - Canale di broadcast per far discutere insieme più Silicon Beings
  - Sistema di strumento di configurazione
  - Vista Web di audit
  - Documentazione cinese tradizionale
  - 42 file modificati, 3533 righe aggiunte, 268 righe eliminate

### 2026-04-16

- `5040f05` - Aggiornamento dei moduli principali e predefiniti
  - Ottimizzazione dei moduli e correzioni di bug
  - Aggiornamenti e miglioramenti delle implementazioni
  - 58 file modificati, 9916 righe aggiunte, 111 righe eliminate

### 2026-04-15

- `3efab5f` - Aggiornamento di diversi moduli : IA, Chat, IM, Strumenti, Web, Localizzazione, Storage
  - Miglioramenti del client IA
  - Miglioramenti del sistema di chat
  - Aggiornamento dei fornitori di messaggistica
  - Ottimizzazione del sistema di strumenti
  - Miglioramenti dell'infrastruttura Web
  - Ottimizzazione della localizzazione
  - Aggiornamento del sistema di storage
  - 33 file modificati, 788 righe aggiunte, 232 righe eliminate

### 2026-04-14

- `4241a2f` - Funzionalità di chat fondamentalmente completata, ottimizzazione upload UI
  - Funzionalità del sistema di chat completata
  - Ottimizzazione UI upload file
  - 16 file modificati, 1234 righe aggiunte, 102 righe eliminate

### 2026-04-13

- `c498c31` - Aggiornamento del codice
  - Miglioramenti e ottimizzazioni generali del codice
  - 32 file modificati, 1045 righe aggiunte, 546 righe eliminate

### 2026-04-12

#### Documentazione e localizzazione
- `2161002` - Rifacimento della documentazione e miglioramento della localizzazione
  - 17 file modificati, 982 righe aggiunte, 92 righe eliminate
- `03d94e4` - Miglioramento del sistema di configurazione e della localizzazione
  - 25 file modificati, 1378 righe aggiunte, 154 righe eliminate
- `9976a35` - Aggiunta della pagina Informazioni e della localizzazione
  - 14 file modificati, 699 righe aggiunte, 44 righe eliminate

#### Chat e viste Web
- `0c8ccfc` - Miglioramento del sistema di chat, della localizzazione e delle viste Web
  - 13 file modificati, 402 righe aggiunte, 56 righe eliminate
- `a8f1342` - Rifacimento del livello di comunicazione Web, passaggio da WebSocket a SSE
  - 27 file modificati, 793 righe aggiunte, 935 righe eliminate

### 2026-04-11

#### Sistema di logging
- `e8fe259` - Aggiunta del sistema di logging e ottimizzazione del codice
  - 37 file modificati, 624 righe aggiunte, 91 righe eliminate
- `f01c519` - Aggiunta del sistema di logging, aggiornamento dell'interfaccia IA e delle viste Web
  - 31 file modificati, 1758 righe aggiunte, 63 righe eliminate

### 2026-04-10

- `4962924` - Miglioramento del gestore WebSocket, della vista chat e dell'interazione messenger
  - Miglioramenti del gestore di contesto
  - Miglioramenti del sistema di chat
  - Aggiornamento dell'interfaccia del fornitore di messaggistica
  - Rifacimento del fornitore WebUI
  - Aggiornamento del costruttore JavaScript e del router
  - Ottimizzazione della vista chat
  - Miglioramenti del gestore WebSocket
  - 9 file modificati, 365 righe aggiunte, 134 righe eliminate

### 2026-04-09

- `f9302bf` - Miglioramento dell'interfaccia del fornitore di messaggistica, del sistema di chat e dell'interazione Web UI
  - Estensione dell'interfaccia del fornitore di messaggistica
  - Miglioramenti dei messaggi di chat e del sistema
  - Ottimizzazione del gestore di contesto
  - Miglioramento del Silicon Being predefinito
  - Miglioramenti della vista chat Web UI
  - Aggiornamento del gestore WebSocket
  - 10 file modificati, 427 righe aggiunte, 93 righe eliminate

### 2026-04-07

- `6831ee8` - Rifacimento delle viste Web e del costruttore JavaScript
  - Rifacimento completo dei controller Web
  - Riscrittura completa del costruttore JavaScript
  - Aggiornamento di tutti i componenti di vista
  - Miglioramenti del sistema di skin
  - Miglioramento dell'architettura della classe base delle viste
  - 23 file modificati, 2004 righe aggiunte, 1983 righe eliminate

### 2026-04-05

- `41e97fb` - Aggiornamento di diversi moduli principali e controller Web
  - Miglioramenti del gestore di contesto
  - Sistema di chat e gestione delle sessioni
  - Rifacimento del localizzatore di servizi
  - Aggiornamento della classe base e del gestore di Silicon Beings
  - Aggiornamento completo dei controller Web (17 controller)
  - Miglioramento della fabbrica di Silicon Beings predefinita
  - 31 file modificati, 681 righe aggiunte, 326 righe eliminate
- `67988d4` - Miglioramento del modulo Web UI, aggiunta della vista executor, pulizia delle viste e dei moduli principali
  - 61 file modificati, 3148 righe aggiunte, 3726 righe eliminate

### 2026-04-04

- `b58bb1c` - Aggiunta del controller di inizializzazione e rifacimento del modulo Web
  - Controller di inizializzazione
  - Rifacimento del modulo di configurazione
  - Aggiornamento del modulo di localizzazione
  - Miglioramenti del sistema di skin
  - Miglioramenti del router
  - 29 file modificati, 1269 righe aggiunte, 289 righe eliminate
- `f03ac0b` - Aggiunta del modulo Web UI, miglioramento della funzionalità messenger
  - 60 file modificati, 8481 righe aggiunte, 165 righe eliminate

### 2026-04-03

- `192e57b` - Aggiornamento della struttura del progetto e dei componenti di esecuzione principali
  - 22 file modificati, 446 righe aggiunte, 179 righe eliminate
- `59faec8` - Aggiornamento delle implementazioni principali e predefinite
  - 25 file modificati, 3056 righe aggiunte, 18 righe eliminate
- `d488485` - Aggiunta della compilazione dinamica e del modulo di strumenti del Curator
  - 19 file modificati, 1727 righe aggiunte, 11 righe eliminate
- `753d1d9` - Aggiunta del modulo di sicurezza, aggiornamento degli executor, fornitori di messaggistica, localizzazione e strumenti
  - 29 file modificati, 2352 righe aggiunte, 93 righe eliminate
- `a378697` - Completamento della fase 5 - Sistema di strumenti + Executor
  - 41 file modificati, 2651 righe aggiunte, 363 righe eliminate

### 2026-04-02

- `e6ad94b` - Correzione del fallimento del caricamento della cronologia chat durante l'eliminazione del file di configurazione nei test
  - 4 file modificati, 49 righe aggiunte, 45 righe eliminate
- `daa56f5` - Completamento della fase 4 : Memoria persistente (sistema di chat + canale messenger)
  - 29 file modificati, 2051 righe aggiunte, 538 righe eliminate

### 2026-04-01

- `bbe2dbb` - Correzione del caricamento della configurazione e del routing dei messaggi del servizio di chat
  - 27 file modificati, 1633 righe aggiunte, 147 righe eliminate
- `2fa6305` - Implementazione della fase 2 : Quadro del ciclo principale e sistema di oggetti timer
  - 9 file modificati, 594 righe aggiunte, 41 righe eliminate
- `32b99a1` - Implementazione della fase 1 - Funzionalità di chat di base
  - 19 file modificati, 1185 righe aggiunte
- `358e368` - Commit iniziale : documentazione del progetto e licenza
  - 10 file modificati, 1873 righe aggiunte
