# Início Rápido

> **Versão: v0.2.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md) | [Русский](../ru-RU/getting-started.md)

## Escolher a Versão

Este projecto oferece duas versões de implementação:

### SiliconLife.Default (Versão Padrão)
- **Posicionamento**: Implementação padrão, usada principalmente para verificar a viabilidade da arquitectura
- **Modo de execução**: Aplicação de consola
- **Método de armazenamento**: Armazenamento JSON em sistema de ficheiros
- **Cenários aplicáveis**: Prioridade de segurança de dados, pequeno volume de dados, depuração de desenvolvimento, verificação de arquitectura
- **Suporte de plataforma**: Windows, Linux, macOS
- **Descrição do papel**: Como implementação de referência para verificação da arquitectura, fornece uma forma de execução simples e fiável, adequada para primeiro contacto com o projecto ou para depuração de desenvolvimento

### SiliconLife.Fast (Versão de Alto Desempenho)
- **Posicionamento**: Versão de produção recomendada
- **Modo de execução**: Aplicação de ambiente de trabalho (Bandeja do sistema Windows/macOS / Janela de estado Linux)
- **Método de armazenamento**: Armazenamento em memória SpeedyPack + persistência assíncrona (formato de ficheiro .spk)
- **Cenários aplicáveis**: Alta concorrência, baixa latência, grande volume de dados, execução de produção a longo prazo
- **Suporte de plataforma**: Windows/macOS (funcionalidade completa, incluindo bandeja do sistema), Linux (janela de estado, sem ícone de bandeja)
- **Descrição do papel**: Implementação de nível de produção profundamente optimizada, sendo a escolha preferida para execução prolongada e ambientes de produção reais

> **Sugestão para iniciantes**: Para a primeira utilização, recomenda-se começar pelo **SiliconLife.Default** para verificar rapidamente a viabilidade da arquitectura; após familiarizar-se com o sistema, recomenda-se fortemente a migração para **SiliconLife.Fast** como versão de execução do ambiente de produção.

## Pré-requisitos

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Download](https://git-scm.com/)
- **Ollama** (opcional, para IA local) - [Download](https://ollama.com/)
- **Chave API DashScope** (opcional, para IA na nuvem) - [Solicitar](https://bailian.console.aliyun.com/)
- **Chave API Volcengine Ark** (opcional, para IA na nuvem) - [Solicitar](https://console.volcengine.com/ark)
- **Herdsman** (opcional, motor de inferência local/nuvem) - sem autenticação, compatível com o formato OpenAI API
- **Chave API Meituan LongCat** (opcional, para IA na nuvem) - autenticação por API Key
- **Chave API Qiniu Cloud AI** (opcional, para IA na nuvem) - autenticação por API Key
- **Chave API DeepSeek** (opcional, para IA na nuvem) - [Solicitar](https://platform.deepseek.com/)
- **Chave API Zhipu AI** (opcional, para IA na nuvem) - [Solicitar](https://open.bigmodel.cn/)
- **Chave API Baidu Qianfan** (opcional, para IA na nuvem) - [Solicitar](https://qianfan.baidubce.com/)
- **Chave API Tencent Hunyuan** (opcional, para IA na nuvem) - [Solicitar](https://cloud.tencent.com/product/hunyuan)
- **Chave API MiniMax** (opcional, para IA na nuvem) - [Solicitar](https://api.minimaxi.com/)
- **Chave API Moonshot** (opcional, para IA na nuvem) - [Solicitar](https://platform.moonshot.cn/)
- **Chave API SiliconFlow** (opcional, para IA na nuvem) - [Solicitar](https://siliconflow.cn/)

## Início Rápido

### 1. Clonar o Repositório

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Compilar o Projecto

```bash
dotnet build
```

### 3. Configurar o Backend de IA

Edite `src/SiliconLife.Default/Config/DefaultConfigData.cs` ou modifique a configuração em tempo de execução através da Web UI.

#### Opção A: Ollama (Local)

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    }
  }
}
```

#### Opção B: DashScope (Nuvem)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "beijing"
    }
  }
}
```

> **Regiões disponíveis**: `beijing` (Pequim), `virginia` (Virgínia), `singapore` (Singapura), `hongkong` (Hong Kong), `frankfurt` (Frankfurt)

#### Opção C: Volcengine Ark (Nuvem)

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **Nota**: O parâmetro Model do Volcengine Ark aceita o ID do endpoint de inferência (por exemplo `ep-20241212123456-abcde`), e não o nome do modelo.

#### Opção D: Herdsman (Local/Nuvem)

```json
{
  "AIClients": {
    "Herdsman": {
      "Endpoint": "http://localhost:8000",
      "Model": "your-model-name"
    }
  }
}
```

> **Características**: sem autenticação, compatível com o formato OpenAI API, suporte a chamadas de ferramentas e conteúdo de raciocínio.

#### Opção E: Meituan LongCat (Nuvem)

```json
{
  "AIClients": {
    "LongCat": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://api.longcat.ai/v1/chat/completions",
      "Model": "your-model-name"
    }
  }
}
```

#### Opção F: Qiniu Cloud AI (Nuvem)

```json
{
  "AIClients": {
    "QiniuAI": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://api.qiniu.com/v1/chat/completions",
      "Model": "your-model-name"
    }
  }
}
```

#### Opção G: DeepSeek (Nuvem)

```json
{
  "AIClients": {
    "DeepSeek": {
      "ApiKey": "your-api-key-here",
      "Model": "deepseek-chat"
    }
  }
}
```

> **Características**: Suporta modo thinking (reasoning_content), janela de contexto de 1.048.576 tokens.

#### Opção H: Zhipu GLM (Nuvem)

```json
{
  "AIClients": {
    "Zhipu": {
      "ApiKey": "your-api-key-here",
      "Model": "glm-4-plus"
    }
  }
}
```

> **Características**: Suporta modo thinking, visão por modelo, janela de contexto de 1.048.576 tokens.

#### Opção I: Baidu Qianfan/Wenxin (Nuvem)

```json
{
  "AIClients": {
    "Ernie": {
      "ApiKey": "your-api-key-here",
      "Model": "ernie-4.0-8k"
    }
  }
}
```

> **Características**: Plataforma Baidu Qianfan, janela de contexto de 131.072 tokens.

#### Opção J: Tencent Hunyuan (Nuvem)

```json
{
  "AIClients": {
    "Hunyuan": {
      "ApiKey": "your-api-key-here",
      "Model": "hy3"
    }
  }
}
```

> **Características**: Duplo endpoint (TokenHub recomendado / Legacy), modelo hy3 recomendado, janela de contexto de 262.144 tokens.

#### Opção K: MiniMax (Nuvem)

```json
{
  "AIClients": {
    "MiniMax": {
      "ApiKey": "your-api-key-here",
      "Model": "MiniMax-Text-01"
    }
  }
}
```

> **Características**: Janela de contexto de 1.048.576 tokens.

#### Opção L: Moonshot/Kimi (Nuvem)

```json
{
  "AIClients": {
    "Moonshot": {
      "ApiKey": "your-api-key-here",
      "Model": "moonshot-v1-auto"
    }
  }
}
```

> **Características**: Janela de contexto de 262.144 tokens.

#### Opção M: SiliconFlow (Nuvem)

```json
{
  "AIClients": {
    "SiliconFlow": {
      "ApiKey": "your-api-key-here",
      "Model": "Qwen/Qwen2.5-7B-Instruct"
    }
  }
}
```

> **Características**: Plataforma de agregação, suporta obtenção dinâmica da lista de modelos disponíveis, janela de contexto de 1.048.576 tokens.

### 4. Executar a Aplicação

#### Executar a Versão Default

```bash
cd src/SiliconLife.Default
dotnet run
```

O servidor Web será iniciado em `http://localhost:8080`

#### Executar a Versão Fast

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: A aplicação iniciará em modo de janela, minimizando para a bandeja do sistema, com o servidor Web igualmente em `http://localhost:8080`

**Linux**: A aplicação exibirá uma janela de estado (sem ícone de bandeja do sistema) e abrirá automaticamente o navegador para aceder à Web UI. Também pode usar o parâmetro `--no-tray` para saltar a abertura automática do navegador:

```bash
dotnet run -- --no-tray
```

### 5. Aceder à Web UI

Abra o navegador e navegue para:

```
http://localhost:8080
```

Verá um painel que inclui:
- Gestão de Silicon Beings
- Interface de chat
- Painel de configuração
- Monitorização do sistema

## O Primeiro Silicon Being

### Criar o Seu Primeiro Being

1. Na Web UI, navegue para **Gestão de Beings**
2. Clique em **Criar Novo Being**
3. Configure o Ficheiro da Alma (`soul.md`) com personalidade e comportamento
4. Inicie o being

### Exemplo de soul.md

```markdown
# My First Silicon Being

## Personality
You are a helpful assistant specializing in code review.

## Capabilities
- Review code quality
- Suggest improvements
- Explain complex concepts

## Behavior
- Always provide constructive feedback
- Use clear examples
- Be concise but thorough
```

## Perguntas Frequentes

### Ligação Ollama Recusada

**Problema**: Não é possível ligar ao Ollama em `http://localhost:11434`

**Solução**:
```bash
# Verificar se o Ollama está em execução
ollama list

# Iniciar o Ollama, se necessário
ollama serve
```

### Modelo Não Encontrado

**Problema**: `model "qwen2.5:7b" not found`

**Solução**:
```bash
# Obter o modelo necessário
ollama pull qwen2.5:7b
```

### Porta Já em Uso

**Problema**: `HttpListenerException: Address already in use`

**Solução**:
- Altere a porta na configuração
- Ou termine o processo que utiliza a porta 8080:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Próximos Passos

- 📚 Leia o [guia de arquitectura](architecture.md) para compreender o desenho do sistema
- 🛠️ Consulte o [guia de desenvolvimento](development-guide.md) para expandir o sistema
- 📖 Explore a [referência API](api-reference.md) para detalhes de integração
- 🔒 Consulte a [documentação de segurança](security.md) para compreender o sistema de permissões
- 🧰 Consulte a [referência de ferramentas](tools-reference.md) para conhecer todas as ferramentas incorporadas
- 🌐 Consulte o [guia da Web UI](web-ui-guide.md) para conhecer as funcionalidades da interface

## Estrutura do Projecto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces e classes abstractas principais
│   ├── SiliconLife.Common/          # Implementação partilhada (comum a ambas as versões)
│   ├── SiliconLife.App/             # Camada de aplicação partilhada entre Default e Fast
│   ├── SiliconLife.Default/         # Implementação padrão + ponto de entrada (versão consola)
│   ├── SiliconLife.Fast/            # Implementação de alto desempenho + ponto de entrada (versão janela)
│   ├── SiliconLife.Speedy/          # Motor de armazenamento de alto desempenho SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Ferramenta de gestão SpeedyPack (Avalonia UI)
├── docs/                            # Documentação (multilingue, 34 variantes linguísticas)
│   ├── en/                          # Inglês
│   ├── zh-CN/                       # Chinês simplificado
│   ├── zh-HK/                       # Chinês tradicional
│   ├── es-ES/                       # Espanhol
│   ├── ja-JP/                       # Japonês
│   ├── ko-KR/                       # Coreano
│   └── cs-CZ/                       # Checo
├── 总文档/                           # Documentação de requisitos e arquitectura (chinês)
└── README.md                        # Descrição do projecto
```

## Precisa de Ajuda?

- 📖 Consulte o [sistema de documentação de ajuda](web-ui-guide.md#帮助文档系统新增) (suporte multilingue)
- 📚 Leia a [documentação completa](docs/)
- 🐛 Reporte problemas no [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Participe nas discussões da comunidade
