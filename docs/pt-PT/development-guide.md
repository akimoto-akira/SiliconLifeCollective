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
│   └── SiliconLife.Speedy.Manager/  # Ferramenta de gestão SpeedyPack (Windows Forms)
└── docs/                            # Documentação multilingue
```

**Direção das dependências**:
- `SiliconLife.Default` → `SiliconLife.Core` + `SiliconLife.Common` + `SiliconLife.App`
- `SiliconLife.Fast` → `SiliconLife.Core` + `SiliconLife.Common` + `SiliconLife.App` + `SiliconLife.Speedy`
- `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.App` → `SiliconLife.Core` + `SiliconLife.Common`

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

- **Classes**: PascalCase com prefixo funcional (ex: `DefaultSiliconBeing`)
- **Interfaces**: Começar com `I` (ex: `IAIClient`, `ITool`)
- **Implementações**: Terminar com o nome da interface (ex: `OllamaClient` implementa `IAIClient`)
- **Ferramentas**: Terminar com `Tool` (ex: `CalendarTool`, `ChatTool`)
- **ViewModels**: Terminar com `ViewModel` (ex: `BeingViewModel`)

### Organização do código

```
SiliconLife.Common/
├── AI/                    # Clientes IA e implementações de fábrica
├── Calendar/              # 32 implementações de calendário
├── Localization/          # Classes base de localização e 29 implementações de idioma
├── Security/              # Gestor de permissões
├── SiliconBeing/          # Implementação padrão do silicon being
├── Tools/                 # Ferramentas incorporadas partilhadas
├── Web/                   # Infraestrutura Web
└── WebView/               # Implementação Playwright WebView

SiliconLife.App/          # Camada de aplicação partilhada pelo Default e Fast
├── Config/                # Configuração da aplicação
├── Help/                  # Localização da documentação de ajuda
└── Web/                   # Implementação da interface Web
    ├── Component/         # Biblioteca de componentes UI
    ├── Controllers/       # Controladores de rotas
    ├── Models/            # View models
    ├── Views/             # Vistas HTML
    └── Skins/             # Temas de pele

SiliconLife.Default/      # Diretório específico da versão
├── Config/                # Dados de configuração predefinidos
├── IM/                    # Fornecedor WebUI
├── Knowledge/             # Implementação da rede de conhecimentos
├── Logging/               # Implementações de fornecedor de logs
├── Project/               # Implementação do sistema de projetos
├── Security/              # Callbacks de permissão predefinidos
├── Storage/               # Implementação de armazenamento em sistema de ficheiros
└── Tools/                 # Ferramentas específicas da versão (HelpTool)
```

### Documentação

- Todas as APIs públicas devem ter comentários de documentação XML
- Todos os ficheiros de código-fonte usam cabeçalho de licença Apache 2.0
- Aproveitar as funcionalidades do .NET 9 (implicit usings, nullable reference types)

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

## Fluxo de trabalho de desenvolvimento

### 1. Configurar o ambiente de desenvolvimento

```bash
# Clonar o repositório
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Restaurar dependências
dotnet restore

# Compilar
dotnet build
```

### 2. Executar testes

```bash
# Executar todos os testes
dotnet test

# Executar projeto de testes específico
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Depurar

```bash
# Executar com saída de depuração
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Formatar código

```bash
# Formatar código
dotnet format
```

---

## Construir funcionalidades personalizadas

### Exemplo: Adicionar um calendário personalizado

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";

    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Lógica de conversão
        return new CalendarDate(year, month, day);
    }

    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Conversão inversa
        return new GregorianDate(year, month, day);
    }
}
```

### Exemplo: Adicionar um executor personalizado

```csharp
// Os executores são atualmente classes estáticas (DiskExecutor, NetworkExecutor, CommandLineExecutor)
// que verificam permissões via ServiceLocator antes de executar.
// ExecutorBase fornece uma classe abstrata base com suporte a thread em segundo plano e fila de pedidos.

public static class CustomExecutor
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    public static ExecutorResult Execute(ExecutorRequest request, TimeSpan? timeout = null)
    {
        // Verificar permissão primeiro
        PermissionManager? pm = ServiceLocator.Instance.GetPermissionManager(request.CallerId);
        if (pm == null || !pm.CheckPermission(request.CallerId, PermissionType.Function, request.ResourcePath))
        {
            return ExecutorResult.Failed($"Permission denied: {request.ResourcePath}");
        }

        TimeSpan actualTimeout = timeout ?? DefaultTimeout;

        try
        {
            Task<ExecutorResult> task = Task.Run(() => ExecuteCore(request));
            if (task.Wait(actualTimeout))
            {
                return task.Result;
            }
            return ExecutorResult.Failed("Operation timed out");
        }
        catch (AggregateException ex)
        {
            return ExecutorResult.Failed(ex.InnerException?.Message ?? ex.Message);
        }
    }

    private static ExecutorResult ExecuteCore(ExecutorRequest request)
    {
        // Executar operação
        var result = PerformOperation(request);
        return ExecutorResult.Successful(result);
    }
}
```

---

## Diretrizes de teste

### Testes unitários

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var tool = new MyCustomTool();
        var call = new ToolCall
        {
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object>
            {
                ["param1"] = "test"
            }
        };

        // Act
        var result = await tool.ExecuteAsync(call);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### Testes de integração

Testar fluxos completos:
1. A IA retorna tool_calls
2. A ferramenta executa
3. Os resultados são devolvidos à IA
4. A IA retorna a resposta final

---

## Considerações de desempenho

### Sistema de armazenamento

- A versão Default usa armazenamento JSON baseado em ficheiros
- A versão Fast usa o motor de armazenamento em memória SpeedyPack (formato .spk)
- SpeedyPack usa mapeamento de diretórios em memória + cache de entradas + fila de escrita assíncrona
- Consultas indexadas por tempo usam a interface `ITimeStorage`

### Programador do ciclo principal

- Escalonamento justo por fatia de tempo baseado em relógio
- Temporizadores watchdog para detetar operações bloqueadas
- Disjuntores para prevenir falhas em cascata

---

## Melhores práticas

### 1. Sempre validar permissões

Qualquer operação iniciada pela IA deve passar pela cadeia de permissões:

```csharp
PermissionManager? pm = ServiceLocator.Instance.GetPermissionManager(callerId);
if (pm == null || !pm.CheckPermission(callerId, permissionType, resource))
{
    return ExecutorResult.Failed("Permission denied");
}
```

### 2. Usar o Service Locator

Registar e obter serviços globalmente:

```csharp
// Durante a inicialização
ServiceLocator.Instance.Register<ICustomService>(myService);

// Quando necessário
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Seguir a separação Corpo-Cérebro

- O Corpo trata do estado e dos acionadores
- O Cérebro trata da interação com a IA e execução de ferramentas

### 4. Implementar tratamento de erros adequado

```csharp
try
{
    var result = await operation();
    return Result.Success(result);
}
catch (Exception ex)
{
    Logger.Error($"Operation failed: {ex.Message}");
    return Result.Failure(ex.Message);
}
```

---

## Diretrizes de contribuição

1. Fazer fork do repositório
2. Criar o ramo da funcionalidade (`git checkout -b feature/amazing-feature`)
3. Fazer commit das alterações usando conventional commits
4. Enviar para o ramo (`git push origin feature/amazing-feature`)
5. Abrir um Pull Request

### Formato das mensagens de commit

```
<tipo>(<âmbito>): <descrição>

Exemplos:
feat(tool): adicionar ferramenta de calendário personalizada
fix(permission): corrigir null pointer no callback
docs: atualizar guia de desenvolvimento
```

---

## Próximos passos

- 📚 Ler a [documentação de arquitetura](architecture.md)
- 📖 Explorar a [referência da API](api-reference.md)
- 🔒 Rever a [documentação de segurança](security.md)
- 🚀 Ver o [guia de início rápido](getting-started.md)
