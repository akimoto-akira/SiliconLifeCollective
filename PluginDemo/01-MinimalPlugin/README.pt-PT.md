# Demo de Plugin Mínimo

Uma implementação mínima de `IPlugin` que demonstra o ciclo de vida do plugin com valores hardcoded.

## Visão geral da interface IPlugin

Cada plugin SiliconLife deve implementar a interface `IPlugin` definida em `SiliconLife.Collective`：

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

### Resumo das propriedades

| Membro | Tipo | Descrição |
|--------|------|-----------|
| `Id` | `string` | Identificador único, deve ser estável entre versões (ex：`"com.siliconlife.demo.minimal"`) |
| `GetName(Language)` | `string` | Nome de exibição legível, localizado pela enumeração `Language` |
| `Version` | `string` | Cadeia de versão semântica (ex：`"1.0.0"`) |
| `GetDescription(Language)` | `string` | Breve descrição da funcionalidade do plugin |
| `GetAuthor(Language)` | `string` | Nome do autor ou organização |

## Ordem de chamada do ciclo de vida

O host chama os métodos do ciclo de vida numa ordem estrita：

```
OnLoad → OnStart → [Em execução] → OnStop → OnUnload
```

| Método | Quando é chamado | Uso típico |
|--------|-----------------|------------|
| `OnLoad()` | Uma vez, quando a DLL do plugin é carregada no host | Validar configuração, registar tipos, preparar recursos |
| `OnStart()` | Quando o host está totalmente iniciado e todos os plugins carregados | Interagir com outros plugins, iniciar tarefas em segundo plano |
| `OnStop()` | Quando o host encerra normalmente | Libertar recursos, descarregar buffers, guardar estado |
| `OnUnload()` | Quando o plugin é descarregado do processo do host | Limpeza final |

## Esta demo

Este plugin devolve valores hardcoded para todas as propriedades e deixa os métodos do ciclo de vida vazios. É o ponto de partida mais simples para o desenvolvimento de plugins.

## Nota de segurança

Os plugins são carregados num `AssemblyLoadContext` isolado e analisados quanto a referências a espaços de nomes proibidos (ex：`System.IO`, `System.Net.Http`). Consulte a[documentação de segurança](../../docs/pt-PT/security.md) para mais detalhes.
