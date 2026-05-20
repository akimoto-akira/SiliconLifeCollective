# Guia de desenvolvimento

> **Versão: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [Français](../fr-FR/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | [Italiano](../it-IT/development-guide.md) | [Polski](../pl-PL/development-guide.md) | **Português**

## Visão geral da arquitetura

O SiliconLifeCollective segue uma **arquitetura corpo-cérebro**, com uma separação rigorosa entre as interfaces principais e as implementações padrão.

### Estrutura do projeto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces, classes abstratas, infraestrutura comum
│   ├── SiliconLife.Common/          # Implementações partilhadas (comuns a ambas as versões)
│   ├── SiliconLife.Default/         # Implementação padrão, ponto de entrada (verificação de viabilidade)
│   ├── SiliconLife.Fast/            # Implementação de alto desempenho, ponto de entrada (versão produção)
│   ├── SiliconLife.Speedy/          # Motor de armazenamento de alto desempenho SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Ferramenta de gestão SpeedyPack (Avalonia UI)
└── docs/                            # Documentação multilingue
```

**Direção das dependências**:
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core` (unidirecional)

**Descrição dos papéis das versões**:
- **SiliconLife.Default**: Implementação padrão, principalmente para verificação de viabilidade arquitetural. Fornece uma implementação de armazenamento em sistema de ficheiros simples e fiável, adequada para depuração de desenvolvimento e verificação arquitetural.
- **SiliconLife.Fast**: Versão principal de produção. Com base na arquitetura verificada pelo Default, adota o armazenamento em memória SpeedyPack + persistência assíncrona, oferecendo uma otimização de desempenho extrema, a melhor escolha para exploração a longo prazo e verdadeiros ambientes de produção.

## Conceitos fundamentais

### 1. Silicon Being

Cada agente IA é composto por:
- **Corpo** (`DefaultSiliconBeing`): Mantém o estado de sobrevivência, deteta os cenários de ativação
- **Cérebro** (`ContextManager`): Carrega o histórico, chama a IA, executa as ferramentas, persiste as respostas

### 2. Sistema de ferramentas

As ferramentas são automaticamente descobertas e registadas através de reflexão:

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Sistema de permissões

Cadeia de verificação de permissões de 5 níveis:

```
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curador: GlobalACL)
```

### 4. Localizador de serviços

Registo e obtenção global de serviços:
```csharp
// Registar
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Obter
var client = ServiceLocator.Instance.Get<IAIClient>();
```

---

## Adicionar uma nova ferramenta

### 1. Criar a classe da ferramenta

Criar a nova classe em `src/SiliconLife.Common/Tools/` (ferramentas partilhadas por ambas as versões):

> **Nota**: `SiliconLife.Default` e `SiliconLife.Fast` já não possuem diretórios `Tools/` independentes. Todas as ferramentas partilhadas estão unificadas em `SiliconLife.Common/Tools/`.

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Descrição do que esta ferramenta faz";

    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        var param1 = call.Parameters["param1"]?.ToString();

        var result = await DoSomething(param1);

        return new ToolResult
        {
            Success = true,
            Output = result
        };
    }
}
```

### 2. Registar a ferramenta

A ferramenta é automaticamente descoberta pelo `ToolManager` através de reflexão — sem necessidade de registo manual!

### 3. (Opcional) Marcar como apenas para administradores

```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

---

## Adicionar um novo cliente IA

### 1. Implementar IAIClient em `src/SiliconLife.Common/AI/`

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";

    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        var response = await CallMyAPI(request);

        return new AIResponse
        {
            Content = response.Message,
            ToolCalls = response.ToolCalls,
            Usage = response.Usage
        };
    }

    public async IAsyncEnumerable<string> StreamChatAsync(AIRequest request)
    {
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

### 2. Criar a fábrica

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

### 3. A fábrica é automaticamente descoberta e registada.

---

## Adicionar suporte de localização

### 1. Criar o ficheiro de localização

```csharp
public class XxXX : DefaultLocalizationBase
{
    public override string LanguageCode => "xx-XX";
    public override string LanguageName => "Nome do Idioma (Região)";
    // Implementar todas as propriedades abstratas
}
```

### 2. Registar no Program.cs

```csharp
LocalizationManager.Instance.Register<XxXX>(Language.XxXX);
```

---

## Convenções de código

### Nomenclatura

| Elemento | Convenção | Exemplo |
|----------|-----------|---------|
| Classes | PascalCase | `SiliconBeingManager` |
| Interfaces | PascalCase com prefixo I | `ISiliconBeingFactory` |
| Métodos | PascalCase | `ExecuteAsync` |
| Parâmetros | camelCase | `beingId` |
| Campos privados | _camelCase | `_logger` |
| Constantes | PascalCase | `MaxRetryCount` |

### Padrões assíncronos

- Todos os métodos assíncronos devem terminar com `Async`
- Usar `Task<T>` para métodos que retornam valores
- Usar `Task` para métodos sem retorno
- Evitar `.Result` e `.Wait()`

### Tratamento de erros

- Usar exceções específicas em vez de `Exception` genérica
- Registar erros com o logger
- Nunca silenciar exceções sem justificação

---

## Próximos passos

- 📚 Ler a [documentação de arquitetura](architecture.md)
- 🔒 Compreender o [sistema de permissões](permission-system.md)
- 🌐 Ler o [guia da interface Web](web-ui-guide.md)
- 📖 Consultar a [referência da API](api-reference.md)
