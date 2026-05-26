# Guia de Desenvolvimento

> **Versão: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | [Русский](../ru-RU/development-guide.md)

## Visão Geral da Arquitectura

O SiliconLifeCollective segue uma **arquitectura corpo-cérebro**, com separação rigorosa entre interfaces principais e implementações padrão.

### Estrutura do Projecto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces, classes abstractas, infraestrutura comum
│   ├── SiliconLife.Common/          # Implementação partilhada (comum a ambas as versões)
│   ├── SiliconLife.Default/         # Implementação padrão, ponto de entrada (verificação de viabilidade da arquitectura)
│   ├── SiliconLife.Fast/            # Implementação de alto desempenho, ponto de entrada (versão de produção recomendada)
│   ├── SiliconLife.Speedy/          # Motor de armazenamento de alto desempenho SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Ferramenta de gestão SpeedyPack (Avalonia UI)
└── docs/                            # Documentação multilingue
```

**Direcção das dependências**:
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core` (unidireccional)

**Descrição do papel das versões**:
- **SiliconLife.Default**: Implementação padrão, usada principalmente para verificar a viabilidade da arquitectura. Fornece uma implementação de armazenamento em sistema de ficheiros simples e fiável, adequada para depuração de desenvolvimento e verificação de arquitectura.
- **SiliconLife.Fast**: Versão de produção recomendada. Com base na arquitectura verificada pela versão Default, adopta armazenamento em memória SpeedyPack + persistência assíncrona, fornecendo optimização de desempenho extrema, sendo a escolha preferida para execução prolongada e ambientes de produção reais.

## Conceitos Principais

### 1. Silicon Being

Cada agente de IA é composto por:
- **Corpo** (`DefaultSiliconBeing`): Mantém o estado de actividade, detecta cenários de activação
- **Cérebro** (`ContextManager`): Carrega histórico, invoca a IA, executa ferramentas, persiste respostas

### 2. Sistema de Ferramentas

As ferramentas são descobertas e registadas automaticamente via reflexão:

```csharp
// Todas as ferramentas implementam a interface ITool
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Sistema de Permissões

Cadeia de verificação de permissões de 3 níveis:
```
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → negação por defeito)
```

### 4. Localizador de Serviços

Registo e recuperação global de serviços:
```csharp
// Registar
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Obter
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Sistema de Extensão

### Adicionar Nova Ferramenta

1. Criar nova classe em `src/SiliconLife.Common/Tools/` (ferramentas partilhadas entre as duas versões):

> **Nota**: `SiliconLife.Default` e `SiliconLife.Fast` já não têm directórios `Tools/` independentes; todas as ferramentas partilhadas estão uniformemente em `SiliconLife.Common/Tools/`.

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Descrição do que esta ferramenta faz";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Analisar parâmetros
        var param1 = call.Parameters["param1"]?.ToString();
        
        // Executar lógica
        var result = await DoSomething(param1);
        
        // Retornar resultado
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. A ferramenta é descoberta automaticamente via reflexão — sem necessidade de registo manual!

3. (Opcional) Marcar como exclusiva do administrador:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

4. (Opcional) Marcar cenários disponíveis da ferramenta:
```csharp
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class MyTool : ITool { ... }
```

5. (Opcional) Marcar como disponível apenas no cenário de chat:
```csharp
[ChatOnly]
public class HelpTool : ITool { ... }
```

6. (Opcional) Marcar como disponível apenas no cenário de projecto:
```csharp
[ToolScenario(ToolScenarioFlag.Project)]
[SiliconManagerOnly]
public class ProjectWorkTool : ITool { ... }
```

### Adicionar Novo Cliente de IA

1. Implementar `IAIClient` em `src/SiliconLife.Common/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Chamar a sua API de IA
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
        // Implementar streaming
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. Criar fábrica:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. A fábrica é descoberta e registada automaticamente.

### Adicionar Novo Backend de Armazenamento

1. Implementar `IStorage` e `ITimeStorage` em `src/SiliconLife.Default/Storage/` (implementação em sistema de ficheiros) ou `src/SiliconLife.Fast/Storage/` (adaptadores SpeedyPack):

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // Ler da sua base de dados
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // Escrever na sua base de dados
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // Consulta por índice temporal
    }
}
```

### Adicionar Novo Plugin

1. Criar um projecto de biblioteca de classes, implementando a interface `IPlugin`:

```csharp
using SiliconLife.Collective;
using SiliconLife.Collective.Localization;
using SiliconLife.Collective.Tools;

public class MyPlugin : IPlugin
{
    public string Id => "my-plugin";
    public string Version => "1.0.0";
    
    public string GetName(Language language) => "My Plugin";
    public string GetDescription(Language language) => "A custom plugin";
    public string GetAuthor(Language language) => "Author Name";
    
    public void OnLoad() { }
    public void OnStart() { }
    public void OnStop() { }
    public void OnUnload() { }
}
```

2. (Opcional) Implementar a interface `ITool` no plugin para registar ferramentas personalizadas:

```csharp
public class MyPluginTool : ITool
{
    public string Name => "my_plugin_tool";
    public string Description => "A tool provided by my plugin";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        return new ToolResult { Success = true, Output = "Done" };
    }
}
```

3. Colocar a DLL compilada no directório de plugins, o `PluginLoader` carregá-la-á automaticamente.

> **Restrições de segurança**: Os plugins não podem referenciar os namespaces `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`, etc. Os plugins são carregados de forma isolada através de `AssemblyLoadContext`.

### Adicionar Nova Skin

1. Implementar `ISkin` em `src/SiliconLife.App/Web/Skins/`:

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "A custom skin description";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
            }
            /* Your custom styles */
        ";
    }
}
```

2. A skin é descoberta automaticamente pelo `SkinManager`.

## Guia de Estilo de Código

### Convenções de Nomenclatura

- **Classes**: PascalCase, com prefixo funcional (por exemplo `DefaultSiliconBeing`)
- **Interfaces**: Começam com `I` (por exemplo `IAIClient`, `ITool`)
- **Implementações**: Terminam com o nome da interface (por exemplo `OllamaClient` implementa `IAIClient`)
- **Ferramentas**: Terminam com `Tool` (por exemplo `CalendarTool`, `ChatTool`)
- **Modelos de vista**: Terminam com `ViewModel` (por exemplo `BeingViewModel`)

### Organização do Código

```
SiliconLife.Common/
├── AI/                    # Implementação de clientes e fábricas de IA
├── Calendar/              # 32 implementações de calendário
├── Localization/          # Classe base de localização e 34 implementações de variantes linguísticas
├── Security/              # Gestor de permissões
├── SiliconBeing/          # Implementação padrão do Silicon Being
├── Tools/                 # Ferramentas incorporadas partilhadas (25)
├── Web/                   # Infraestrutura Web
└── WebView/               # Implementação Playwright WebView

SiliconLife.App/          # Camada de aplicação partilhada entre Default e Fast
├── Config/                # Configuração da aplicação
├── Help/                  # Localização da documentação de ajuda
├── Project/               # Sistema de projectos (motor de fluxos de trabalho, funções de projecto)
└── Web/                   # Implementação da Web UI
    ├── Component/         # 27 componentes UI
    ├── Controllers/       # 24 controladores de rotas
    ├── Models/            # Modelos de vista
    ├── Views/             # Vistas HTML
    └── Skins/             # 7 temas de skin

SiliconLife.Default/      # Directórios específicos da versão
├── Config/                # Dados de configuração padrão
├── Knowledge/             # Implementação da rede de conhecimento
├── Logging/               # Implementação do fornecedor de registos (consola + sistema de ficheiros)
├── Project/               # Implementação do sistema de projectos
└── Storage/               # Implementação do armazenamento em sistema de ficheiros

SiliconLife.Fast/         # Directórios específicos da versão
├── Config/                # Dados de configuração da versão Fast
├── Logging/               # Implementação do fornecedor de registos (consola + sistema de ficheiros)
├── Storage/               # Adaptadores de armazenamento SpeedyPack
└── Tray/                  # Localização da bandeja do sistema
```

### Documentação

- Todas as APIs públicas devem ter comentários de documentação XML
- Todos os ficheiros fonte usam cabeçalho de licença Apache 2.0
- Aproveitar as funcionalidades do .NET 9 (usings implícitos, tipos de referência anuláveis)

## Fluxo de Trabalho de Desenvolvimento

### 1. Configurar o Ambiente de Desenvolvimento

```bash
# Clonar o repositório
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Restaurar dependências
dotnet restore

# Compilar
dotnet build
```

### 2. Executar Testes

```bash
# Executar todos os testes
dotnet test

# Executar projecto de testes específico
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Depurar

```bash
# Executar com saída de depuração
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Formatar Código

```bash
# Formatar código
dotnet format
```

## Construir Funcionalidades Personalizadas

### Exemplo: Adicionar Calendário Personalizado

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Sua lógica de conversão
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Conversão inversa
        return new GregorianDate(year, month, day);
    }
}
```

### Exemplo: Adicionar Executor Personalizado

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

### Exemplo: Adicionar Modelo de Fluxo de Trabalho Personalizado

```csharp
public class MyWorkflowTemplate : WorkflowTemplate
{
    public override string Name => "my_workflow";
    public override string Description => "A custom workflow template";
    
    public override void DefineStates()
    {
        AddState("start", "Iniciar", isInitial: true);
        AddState("processing", "Em processamento");
        AddState("review", "Em revisão");
        AddState("done", "Concluído", isFinal: true);
    }
    
    public override void DefineTransitions()
    {
        AddTransition("start", "processing", "Iniciar processamento");
        AddTransition("processing", "review", "Submeter para revisão");
        AddTransition("review", "done", "Revisão aprovada");
        AddTransition("review", "processing", "Revisão rejeitada");
    }
}
```

### Exemplo: Adicionar Função de Projecto

As funções de projecto são geridas através das operações `assign_role` e `remove_role` do `ProjectTool`. O nome da função é uma string personalizada, usada para distinguir as responsabilidades dos Silicon Beings nos fluxos de trabalho e atribuição de tarefas.

## Guia de Testes

### Testes Unitários

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Arranjar
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object> 
            { 
                ["param1"] = "test" 
            }
        };
        
        // Actuar
        var result = await tool.ExecuteAsync(call);
        
        // Verificar
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### Testes de Integração

Testar fluxos completos:
1. A IA retorna chamada de ferramenta
2. A ferramenta executa
3. O resultado é alimentado de volta à IA
4. A IA retorna a resposta final

## Considerações de Desempenho

### Sistema de Armazenamento

- A versão Default usa armazenamento JSON baseado em ficheiros
- A versão Fast usa o motor de armazenamento em memória SpeedyPack (formato .spk)
- O SpeedyPack usa mapeamento de directórios em memória + cache de entradas + fila de escrita assíncrona
- Consultas por índice temporal usam a interface `ITimeStorage`

### Escalonador do Ciclo Principal

- Escalonamento justo por fatias de tempo orientado por relógio
- Temporizador watchdog para detectar operações bloqueadas
- Circuit breaker para prevenir falhas em cascata

## Melhores Práticas

### 1. Validar Sempre as Permissões

Qualquer operação iniciada pela IA deve passar pela cadeia de permissões:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return Result.Denied("Permission denied");
}
```

### 2. Usar o Localizador de Serviços

Registar e recuperar serviços globalmente:

```csharp
// Durante a inicialização
ServiceLocator.Instance.Register<ICustomService>(myService);

// Quando necessário
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Seguir a Separação Corpo-Cérebro

- O corpo trata do estado e activação
- O cérebro trata da interacção com a IA e execução de ferramentas

### 4. Implementar Tratamento de Erros Adequado

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

## Guia de Contribuição

1. Fazer fork do repositório
2. Criar ramo de funcionalidade (`git checkout -b feature/amazing-feature`)
3. Submeter as suas alterações usando commits convencionais
4. Empurrar para o ramo (`git push origin feature/amazing-feature`)
5. Abrir um Pull Request

### Formato das Mensagens de Commit

```
<type>(<scope>): <description>

Exemplos:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Próximos Passos

- 📚 Leia o [guia de arquitectura](architecture.md)
- 📖 Explore a [referência API](api-reference.md)
- 🔒 Consulte a [documentação de segurança](security.md)
- 🚀 Consulte o [guia de início rápido](getting-started.md)
