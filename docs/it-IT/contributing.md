# Guida al contributo

> **Versione : v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [Français](../fr-FR/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md) | **Italiano**

Grazie per il tuo interesse nel contribuire a SiliconLifeCollective !

## Contributi a doppia versione

Questo progetto ha due versioni di implementazione. Puoi contribuire secondo i tuoi interessi :

### SiliconLife.Default (Versione standard)
- **Stack tecnologico** : Applicazione console .NET 9
- **Direzione del contributo** : Sviluppo funzionalità principali, implementazione strumenti, localizzazione, documentazione
- **Pubblico target** : Tutti gli sviluppatori

### SiliconLife.Fast (Versione ad alte prestazioni)
- **Stack tecnologico** : Applicazione desktop Avalonia UI .NET 9
- **Direzione del contributo** : Ottimizzazione prestazioni, storage SpeedyPack, barra di sistema, concorrenza senza lock
- **Pubblico target** : Sviluppatori con esperienza Windows e interesse per l'ottimizzazione delle prestazioni

> **Nota importante** : Entrambe le versioni condividono i progetti SiliconLife.Core e SiliconLife.Common. I miglioramenti alle interfacce principali influenzano entrambe le versioni.

## Codice di condotta

Questo progetto segue la licenza Apache 2.0. Mantieni rispetto e professionalità in tutte le interazioni.

---

## Avvio rapido

### 1. Fare il fork del repository

Clicca sul pulsante « Fork » su GitHub per creare la tua copia.

### 2. Clonare il tuo fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Configurare l'ambiente di sviluppo

```bash
# Installare .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# Ripristinare le dipendenze
dotnet restore

# Compilare il progetto
dotnet build

# Eseguire i test
dotnet test
```

### 4. Creare un ramo di funzionalità

```bash
git checkout -b feature/nome-della-tua-funzionalità
```

### 5. Scegliere il progetto di sviluppo

In base al tuo tipo di contributo, scegli il progetto appropriato :

- **Interfacce principali/classi astratte** → Modificare `SiliconLife.Core`
- **Implementazioni condivise** → Modificare `SiliconLife.Common`
- **Specifico per versione Default** → Modificare `SiliconLife.Default`
- **Specifico per versione Fast** → Modificare `SiliconLife.Fast`
- **Motore di storage** → Modificare `SiliconLife.Speedy`
- **Strumento di gestione storage** → Modificare `SiliconLife.Speedy.Manager`
- **Sviluppo plugin** → Modificare `SiliconLife.Core/Plugins`
- **Documentazione multilingua** → Modificare la directory `docs/`

---

## Flusso di lavoro di sviluppo

### Stile del codice

- Seguire le convenzioni C#
- Nomi di classi in PascalCase
- Parametri di metodi in camelCase
- Campi privati in `_camelCase`
- Tutte le API pubbliche devono avere documentazione XML

### Messaggi di commit

Seguire il formato dei **commit convenzionali** :

```
<type>(<ambito>): <descrizione>
```

**Tipi** :
- `feat` : Nuova funzionalità
- `fix` : Correzione bug
- `docs` : Modifica documentazione
- `style` : Formattazione codice
- `refactor` : Refactoring codice
- `test` : Modifica test
- `chore` : Modifica build/strumenti

**Esempi** :
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### Effettuare modifiche

1. **Scrivere il codice**
   - Seguire i modelli esistenti
   - Aggiungere test per le nuove funzionalità
   - Aggiornare la documentazione

2. **Testare le modifiche**
   ```bash
   # Eseguire tutti i test
   dotnet test
   
   # Compilare in modalità release
   dotnet build --configuration Release
   ```

3. **Formattare il codice**
   ```bash
   dotnet format
   ```

4. **Commit delle modifiche**
   ```bash
   git add .
   git commit -m "feat(ambito): descrizione"
   ```

5. **Push verso il tuo fork**
   ```bash
   git push origin feature/nome-della-tua-funzionalità
   ```

6. **Creare una Pull Request**
   - Andare al repository originale
   - Cliccare su « Compare & pull request »
   - Compilare il modello di PR
   - Inviare

---

## Guida alle Pull Request

### Titolo PR

Usare lo stesso formato dei messaggi di commit :
```
feat(localization): add Korean language support
```

### Descrizione PR

Includere :

1. **Cosa** - Cosa fa questa PR ?
2. **Perché** - Perché questo cambiamento è necessario ?
3. **Come** - Come lo hai implementato ?
4. **Test** - Come è stato testato ?

### Esempio di descrizione PR

```markdown
## Cosa
Aggiunta della localizzazione coreana per tutti i componenti UI e la documentazione.

## Perché
Ampliare l'accessibilità del progetto agli utenti coreani.

## Come
- Creazione del file di localizzazione KoKR.cs
- Aggiunta di 500+ chiavi di traduzione
- Aggiornamento di tutte le viste per utilizzare la localizzazione
- Creazione della documentazione coreana in docs/ko-KR/

## Test
- Verifica che tutti gli elementi UI visualizzino correttamente il coreano
- Test della funzionalità di cambio lingua
- Revisione delle traduzioni con un madrelingua
```

---

## Tipi di contributi

### 1. Correzione bug

**Processo** :
1. Verificare le issue esistenti
2. Creare una issue se non esiste
3. Correggere il bug
4. Aggiungere casi di test
5. Inviare una PR

**Requisiti** :
- Descrizione chiara del bug
- Passaggi di riproduzione
- Test per prevenire regressioni

### 2. Nuove funzionalità

**Processo** :
1. Discutere la funzionalità in Issues/Discussions
2. Ottenere l'approvazione dei manutentori
3. Implementare la funzionalità
4. Aggiungere test completi
5. Aggiornare la documentazione
6. Inviare una PR

**Requisiti** :
- Proposta di funzionalità approvata
- Copertura di test completa
- Documentazione aggiornata
- Compatibilità verso l'alto

### 3. Documentazione

**Processo** :
1. Identificare le lacune nella documentazione
2. Scrivere/aggiornare la documentazione
3. Inviare una PR

**Requisiti** :
- Chiaro e conciso
- Includere esempi
- Supporto multilingua se applicabile

### 4. Refactoring del codice

**Processo** :
1. Proporre il refactoring in una Issue
2. Ottenere l'approvazione
3. Refactoring del codice
4. Assicurarsi che tutti i test passino
5. Inviare una PR

**Requisiti** :
- Nessun cambiamento di funzionalità
- Tutti i test passano
- Miglioramento della qualità del codice
- Spiegazione chiara

---

## Guida ai test

### Test unitari

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

### Test di integrazione

Testare i flussi di lavoro completi :
- Interazione IA
- Esecuzione strumenti
- Convalida permessi
- Operazioni di storage

### Test manuali

Per le modifiche UI :
- Testare in più browser
- Verificare il design responsive
- Verificare l'accessibilità

---

## Guida alla documentazione

### Commenti del codice

- Usare i commenti XML per tutte le API pubbliche
- Usare commenti in linea per la logica complessa
- I commenti del codice devono essere in inglese

### File di documentazione

- Posizionare in `docs/{lingua}/`
- Aggiornare tutte le versioni linguistiche
- Seguire la struttura esistente

### Documentazione multilingua

Quando si aggiunge documentazione :
1. Creare prima la versione inglese
2. Tradurre nelle altre lingue
3. Mantenere il contenuto sincronizzato

---

## Processo di revisione

### Cosa controllano i manutentori

1. **Qualità del codice**
   - Segue le convenzioni
   - Chiaro e leggibile
   - Ben documentato

2. **Test**
   - Copertura adeguata
   - Tutti i test passano
   - Copre i casi limite

3. **Documentazione**
   - Aggiornata
   - Spiegazioni chiare
   - Multilingua

4. **Compatibilità**
   - Compatibile con le versioni precedenti
   - Nessuna modifica sostanziale (salvo notifica)
   - Segue la gestione semantica delle versioni

### Tempi di revisione

- Revisione iniziale : 1-3 giorni
- Integrazione feedback : secondo necessità
- Fusione : dopo approvazione

---

## Domande frequenti

### PR respinta

**Motivi** :
- Non segue le linee guida
- Test insufficienti
- Modifiche sostanziali non notificate
- Scarsa qualità del codice

**Soluzioni** :
- Risolvere i feedback
- Aggiornare la PR
- Inviare nuovamente

### Conflitti di fusione

**Soluzioni** :
```bash
# Aggiornare il tuo ramo
git fetch origin
git rebase origin/master

# Risolvere i conflitti
# Modificare i file in conflitto
git add .
git rebase --continue

# Push forzato
git push --force-with-lease
```

---

## Ottenere aiuto

### Risorse

- **Documentazione** : [docs/](../)
- **Issues** : GitHub Issues
- **Discussions** : GitHub Discussions
- **Codice di condotta** : CODE_OF_CONDUCT.md

### Contatto

- Creare una Issue per i bug
- Avviare una Discussione per le domande
- Menzionare i manutentori per domande urgenti

---

## Riconoscimenti

I contributori saranno riconosciuti in :
- La sezione contributori del README.md
- Le note di rilascio
- La documentazione del progetto

---

## Licenza

Contribuendo, accetti che i tuoi contributi siano sotto licenza Apache 2.0.

---

## Prossimi passi

- 📚 Leggere la [documentazione](../)
- 🐛 Vedere le [issue aperte](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Avviare una [discussione](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Fare il fork e iniziare a contribuire !

Grazie per contribuire a SiliconLifeCollective ! 🎉
