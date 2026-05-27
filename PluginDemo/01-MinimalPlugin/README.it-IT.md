# Demo Plugin Minimale

Un'implementazione minima di `IPlugin` che dimostra il ciclo di vita del plugin con valori hardcoded.

## Panoramica dell'interfaccia IPlugin

Ogni plugin SiliconLife deve implementare l'interfaccia `IPlugin` definita in `SiliconLife.Collective`：

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Riepilogo delle proprietà

| Membro | Tipo | Descrizione |
|--------|------|-------------|
| `Id` | `string` | Identificatore univoco, deve essere stabile tra le versioni (es：`"com.siliconlife.demo.minimal"`) |
| `GetName(Language)` | `string` | Nome visualizzato leggibile, localizzato dall'enumerazione `Language` |
| `Version` | `string` | Stringa di versione semantica (es：`"1.0.0"`) |
| `GetDescription(Language)` | `string` | Breve descrizione della funzionalità del plugin |
| `GetAuthor(Language)` | `string` | Nome dell'autore o dell'organizzazione |

## Ordine di chiamata del ciclo di vita

L'host chiama i metodi del ciclo di vita in un ordine rigoroso：

```
OnLoad → OnStart → [In esecuzione] → OnStop → OnUnload
```

| Metodo | Quando viene chiamato | Uso tipico |
|--------|----------------------|------------|
| `OnLoad()` | Una volta, quando la DLL del plugin viene caricata nell'host | Validare la configurazione, registrare i tipi, preparare le risorse |
| `OnStart()` | Quando l'host è completamente avviato e tutti i plugin sono caricati | Interagire con altri plugin, avviare attività in background |
| `OnStop()` | Quando l'host si arresta normalmente | Rilasciare risorse, svuotare i buffer, salvare lo stato |
| `OnUnload()` | Quando il plugin viene scaricato dal processo host | Pulizia finale |

## Questa demo

Questo plugin restituisce valori hardcoded per tutte le proprietà e lascia vuoti i metodi del ciclo di vita. È il punto di partenza più semplice per lo sviluppo di plugin.

## Nota di sicurezza

I plugin vengono caricati in un `AssemblyLoadContext` isolato e scansionati per riferimenti a namespace vietati (es：`System.IO`, `System.Net.Http`). Vedere la[documentazione sulla sicurezza](../../docs/it-IT/security.md) per i dettagli.
