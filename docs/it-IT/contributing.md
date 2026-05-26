# Guida ai Contributi

> **Versione: v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md) | **Italiano**

Grazie per il tuo interesse nel contribuire a SiliconLifeCollective!

## Contributi alla Doppia Versione

Questo progetto ha due versioni di implementazione; puoi scegliere la direzione di contribuzione in base ai tuoi interessi:

### SiliconLife.Default (Versione Predefinita)
- **Stack tecnologico**: Applicazione console .NET 9
- **Direzioni di contribuzione**: Sviluppo di funzionalità core, implementazione di strumenti, localizzazione, documentazione
- **Adatto a**: Tutti gli sviluppatori

### SiliconLife.Fast (Versione ad Alte Prestazioni)
- **Stack tecnologico**: Applicazione desktop multipiattaforma .NET 9 (Avalonia UI)
- **Direzioni di contribuzione**: Ottimizzazione delle prestazioni, archiviazione SpeedyPack, area di notifica di sistema, concorrenza senza lock
- **Adatto a**: Sviluppatori con esperienza di sviluppo desktop e interesse per l'ottimizzazione delle prestazioni

> **Nota importante**: Le due versioni condividono i progetti SiliconLife.Core e SiliconLife.Common; i miglioramenti alle interfacce core influenzano entrambe le versioni contemporaneamente.

## Codice di Condotta

Questo progetto segue la licenza Apache 2.0. Mantieni un atteggiamento rispettoso e professionale in tutte le interazioni.

---

## Inizio Rapido

### 1. Fork del Repository

Clicca sul pulsante "Fork" su GitHub per creare la tua copia.

### 2. Clona il tuo Fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Configura l'Ambiente di Sviluppo

```bash
# Installa .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# Ripristina le dipendenze
dotnet restore

# Compila il progetto
dotnet build

# Esegui i test
dotnet test
```

### 4. Crea un Branch di Funzionalità

```bash
git checkout -b feature/your-feature-name
```

### 5. Scegli il Progetto di Sviluppo

Scegli il progetto appropriato in base al tipo di contributo:

- **Interfacce core/classi astratte** → Modifica `SiliconLife.Core`
- **Implementazioni condivise** → Modifica `SiliconLife.Common`
- **Specifico della versione Default** → Modifica `SiliconLife.Default`
- **Specifico della versione Fast** → Modifica `SiliconLife.Fast`
- **Motore di archiviazione** → Modifica `SiliconLife.Speedy`
- **Strumento di gestione archiviazione** → Modifica `SiliconLife.Speedy.Manager`
- **Sviluppo di plugin** → Modifica `SiliconLife.Core/Plugins`
- **Documentazione multilingua** → Modifica la directory `docs/`

---

## Flusso di Lavoro di Sviluppo

### Stile del Codice

- Segui le convenzioni di codifica C#
- I nomi delle classi usano PascalCase
- I parametri dei metodi usano camelCase
- I campi privati usano `_camelCase`
- Tutte le API pubbliche devono avere documentazione XML

### Messaggi di Commit

Segui il formato dei **Commit Convenzionali**:

```
<type>(<scope>): <description>
```

**Tipi**:
- `feat`: Nuova funzionalità
- `fix`: Correzione di bug
- `docs`: Modifiche alla documentazione
- `style`: Formattazione del codice
- `refactor`: Rifattorizzazione del codice
- `test`: Modifiche ai test
- `chore`: Modifiche alla build/strumenti

**Esempi**:
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### Apportare Modifiche

1. **Scrivi il codice**
   - Segui i pattern esistenti
   - Aggiungi test per le nuove funzionalità
   - Aggiorna la documentazione

2. **Testa le tue modifiche**
   ```bash
   # Esegui tutti i test
   dotnet test
   
   # Compila in modalità Release
   dotnet build --configuration Release
   ```

3. **Formatta il codice**
   ```bash
   dotnet format
   ```

4. **Committa le modifiche**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

5. **Pusha sul tuo Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Crea una Pull Request**
   - Vai al repository originale
   - Clicca su "Compare & pull request"
   - Compila il template PR
   - Invia

---

## Guida alle Pull Request

### Titolo della PR

Usa lo stesso formato dei messaggi di commit:
```
feat(localization): add Korean language support
```

### Descrizione della PR

Includi:

1. **Cosa** - Cosa fa questa PR?
2. **Perché** - Perché è necessaria questa modifica?
3. **Come** - Come l'hai implementata?
4. **Test** - Come è stata testata?

### Esempio di Descrizione PR

```markdown
## Cosa
Aggiunta la localizzazione coreana per tutti i componenti UI e la documentazione.

## Perché
Espandere l'accessibilità del progetto agli utenti di lingua coreana.

## Come
- Creato il file di localizzazione KoKR.cs
- Aggiunti 500+ tasti di traduzione
- Aggiornate tutte le viste per utilizzare la localizzazione
- Creata la documentazione coreana in docs/ko-KR/

## Test
- Verificato che tutti gli elementi UI mostrino correttamente il coreano
- Testata la funzionalità di cambio lingua
- Revisionate le traduzioni con madrelingua
```

---

## Tipi di Contributi

### 1. Correzione di Bug

**Processo**:
1. Controlla le issue esistenti
2. Crea una issue se non esiste
3. Correggi il bug
4. Aggiungi casi di test
5. Invia una PR

**Requisiti**:
- Descrizione chiara del bug
- Passaggi per riprodurlo
- Test per prevenire regressioni

### 2. Nuove Funzionalità

**Processo**:
1. Discuti la funzionalità nelle Issues/Discussioni
2. Ottieni l'approvazione dei manutentori
3. Implementa la funzionalità
4. Aggiungi test completi
5. Aggiorna la documentazione
6. Invia una PR

**Requisiti**:
- Proposta di funzionalità approvata
- Copertura completa dei test
- Documentazione aggiornata
- Compatibilità con le versioni precedenti

### 3. Documentazione

**Processo**:
1. Identifica le lacune nella documentazione
2. Scrivi/aggiorna la documentazione
3. Invia una PR

**Requisiti**:
- Chiaro e conciso
- Includi esempi
- Supporto multilingua se applicabile

### 4. Rifattorizzazione del Codice

**Processo**:
1. Proponi la rifattorizzazione in una Issue
2. Ottieni l'approvazione
3. Rifattorizza il codice
4. Assicurati che tutti i test passino
5. Invia una PR

**Requisiti**:
- Nessun cambiamento di funzionalità
- Tutti i test passano
- Miglioramento della qualità del codice
- Spiegazione chiara

---

## Guida ai Test

### Test Unitari

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

### Test di Integrazione

Testa il flusso di lavoro completo:
- Interazione AI
- Esecuzione degli strumenti
- Verifica dei permessi
- Operazioni di archiviazione

### Test Manuale

Per le modifiche UI:
- Testa su browser multipli
- Verifica il design responsivo
- Controlla l'accessibilità

---

## Guida alla Documentazione

### Commenti al Codice

- Usa commenti XML per tutte le API pubbliche
- Usa commenti inline per la logica complessa
- I commenti al codice sono in inglese

### File di Documentazione

- Posiziona in `docs/{language}/`
- Aggiorna tutte le versioni linguistiche
- Segui la struttura esistente

### Documentazione Multilingua

Quando aggiungi documentazione:
1. Crea prima la versione inglese
2. Traduci nelle altre lingue
3. Mantieni i contenuti sincronizzati

---

## Processo di Revisione

### Cosa Controllano i Manutentori

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
   - Nessuna modifica dirompente (a meno che non notificata)
   - Segue il versionamento semantico

### Tempistiche di Revisione

- Revisione iniziale: 1-3 giorni
- Integrazione del feedback: secondo necessità
- Merge: dopo l'approvazione

---

## Domande Frequenti

### PR Rifiutata

**Motivi**:
- Non segue le linee guida
- Test insufficienti
- Modifiche dirompenti non notificate
- Qualità del codice scarsa

**Soluzione**:
- Risolvi i feedback
- Aggiorna la PR
- Invia di nuovo

### Conflitti di Merge

**Soluzione**:
```bash
# Aggiorna il tuo branch
git fetch origin
git rebase origin/master

# Risolvi i conflitti
# Modifica i file in conflitto
git add .
git rebase --continue

# Force push
git push --force-with-lease
```

---

## Ottenere Aiuto

### Risorse

- **Documentazione**: [docs/](../)
- **Issue**: GitHub Issues
- **Discussioni**: GitHub Discussions
- **Codice di condotta**: CODE_OF_CONDUCT.md

### Contatti

- Crea una Issue per i bug
- Avvia una Discussion per domande
- Tagga i manutentori per questioni urgenti

---

## Riconoscimenti

I contributori saranno riconosciuti in:
- Sezione contributori del README.md
- Note di rilascio
- Documentazione del progetto

---

## Licenza

Contribuendo, accetti che i tuoi contributi saranno concessi sotto la licenza Apache 2.0.

---

## Prossimi Passi

- 📚 Leggi la [documentazione](../)
- 🐛 Controlla le [issue aperte](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Inizia una [discussione](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Fai un fork e inizia a contribuire!

Grazie per il tuo contributo a SiliconLifeCollective! 🎉
