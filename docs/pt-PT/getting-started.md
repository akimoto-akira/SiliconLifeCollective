# Guia de introdução

> **Versão: v0.2.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [Français](../fr-FR/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md) | [Русский](../ru-RU/getting-started.md) | [Italiano](../it-IT/getting-started.md) | [Polski](../pl-PL/getting-started.md) | **Português**

## Escolher uma versão

Este projeto oferece duas versões de implementação:

### SiliconLife.Default (Versão padrão)
- **Posicionamento**: Implementação padrão, principalmente para verificação de viabilidade arquitetural
- **Modo de execução**: Aplicação de consola
- **Armazenamento**: Armazenamento JSON em sistema de ficheiros
- **Cenário de uso**: Prioridade à segurança dos dados, pequeno volume de dados, depuração de desenvolvimento, verificação arquitetural
- **Suporte de plataforma**: Windows, Linux, macOS
- **Descrição do papel**: Implementação de referência para verificação arquitetural, oferece uma execução simples e fiável, adequada para primeiro contacto ou depuração de desenvolvimento

### SiliconLife.Fast (Versão de alto desempenho)
- **Posicionamento**: Versão principal de produção
- **Modo de execução**: Aplicação desktop (Windows/macOS bandeja do sistema / Linux janela de estado)
- **Armazenamento**: Armazenamento em memória SpeedyPack + persistência assíncrona (formato de ficheiro .spk)
- **Cenário de uso**: Alta concorrência, baixa latência, grande volume de dados, exploração em produção a longo prazo
- **Suporte de plataforma**: Windows/macOS (funcionalidades completas, incluindo bandeja do sistema), Linux (janela de estado, sem ícone na bandeja)
- **Descrição do papel**: Implementação pronta para produção com otimização aprofundada, a melhor escolha para exploração a longo prazo e verdadeiros ambientes de produção

> **Recomendação para principiantes**: Os novos utilizadores devem começar com **SiliconLife.Default** para verificar rapidamente a viabilidade arquitetural. Após familiarizarem-se com o sistema, recomenda-se vivamente a migração para **SiliconLife.Fast**.

## Pré-requisitos

- **.NET 9 SDK** - [Descarregar](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Descarregar](https://git-scm.com/)
- **Ollama** (opcional, para IA local) - [Descarregar](https://ollama.com/)
- **Chave API DashScope** (opcional, para IA na nuvem) - [Solicitar](https://bailian.console.aliyun.com/)
- **Chave API Volcengine Ark** (opcional, para IA na nuvem) - [Solicitar](https://console.volcengine.com/ark)

## Arranque rápido

### 1. Clonar o repositório

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Compilar o projeto

```bash
dotnet build
```

### 3. Configurar o backend IA

Edita `src/SiliconLife.Default/Config/DefaultConfigData.cs` ou altera a configuração em tempo de execução através da interface Web.

#### Opção A: Ollama (local)

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

#### Opção B: DashScope (nuvem)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "a-tua-chave-api-aqui",
      "Model": "qwen-plus",
      "Region": "beijing"
    }
  }
}
```

> **Regiões disponíveis**: `beijing` (Pequim), `virginia` (Virgínia), `singapore` (Singapura), `hongkong` (Hong Kong), `frankfurt` (Frankfurt)

#### Opção C: Volcengine Ark (nuvem)

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "a-tua-chave-api-aqui",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **Nota**: O parâmetro Model do Volcengine Ark aceita um ID de endpoint de inferência (ex. `ep-20241212123456-abcde`), não um nome de modelo.

### 4. Executar a aplicação

#### Executar a versão Default

```bash
cd src/SiliconLife.Default
dotnet run
```

O servidor Web inicia em `http://localhost:8080`

#### Executar a versão Fast

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: A aplicação inicia em modo Forms, minimizada na bandeja do sistema, com o servidor Web também em `http://localhost:8080`

**Linux**: A aplicação mostra uma janela de estado (sem ícone na bandeja do sistema) e abre automaticamente o browser para aceder à Web UI. Podes usar o parâmetro `--no-tray` para saltar a abertura automática do browser:

```bash
dotnet run -- --no-tray
```

### 5. Aceder à interface Web

Abre um browser e acede a:

```
http://localhost:8080
```

Verás um painel com:
- Gestão de Silicon Beings
- Interface de chat
- Painel de configuração
- Monitorização do sistema

## O teu primeiro Silicon Being

### Criar o teu primeiro Being

1. Na interface Web, navega para **Gestão de Beings**
2. Clica em **Criar novo Being**
3. Configura o ficheiro da alma (`soul.md`) com personalidade e comportamento
4. Inicia o Being

### Exemplo de soul.md

```markdown
# O Meu Primeiro Silicon Being

## Personalidade
Tu és um assistente útil especializado em revisão de código.

## Capacidades
- Rever a qualidade do código
- Sugerir melhorias
- Explicar conceitos complexos

## Comportamento
- Fornecer sempre feedback construtivo
- Usar exemplos claros
- Ser conciso mas exaustivo
```

## Perguntas frequentes

### Ligação Ollama recusada

**Problema**: Impossível ligar-se ao Ollama em `http://localhost:11434`

**Solução**:
```bash
# Verificar se o Ollama está em execução
ollama list

# Iniciar o Ollama se necessário
ollama serve
```

### Modelo não encontrado

**Problema**: `model "qwen2.5:7b" not found`

**Solução**:
```bash
# Descarregar o modelo necessário
ollama pull qwen2.5:7b
```

### Porta já ocupada

**Problema**: `HttpListenerException: Address already in use`

**Solução**:
- Alterar a porta na configuração
- Ou terminar o processo na porta 8080:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Próximos passos

- 📚 Ler o [guia de arquitetura](architecture.md) para compreender o design do sistema
- 🛠️ Consultar o [guia de desenvolvimento](development-guide.md) para estender o sistema
- 📖 Explorar a [referência da API](api-reference.md) para detalhes de integração
- 🔒 Consultar a [documentação de segurança](security.md) para o sistema de permissões
- 🧰 Navegar pela [referência de ferramentas](tools-reference.md) para todas as ferramentas integradas
- 🌐 Ler o [guia da interface Web](web-ui-guide.md) para as funcionalidades da interface

## Estrutura do projeto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces principais e classes abstratas
│   ├── SiliconLife.Common/          # Implementação comum (usada por ambas as versões)
│   ├── SiliconLife.App/             # Camada de aplicação partilhada entre Default e Fast
│   ├── SiliconLife.Default/         # Implementação padrão + ponto de entrada (versão consola)
│   ├── SiliconLife.Fast/            # Implementação de alto desempenho + ponto de entrada (versão Forms)
│   ├── SiliconLife.Speedy/          # Motor de armazenamento de alto desempenho SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Ferramenta de gestão SpeedyPack (Avalonia UI)
├── docs/                            # Documentação (multilingue, 33 variantes linguísticas)
│   ├── en/                          # Inglês
│   ├── zh-CN/                       # Chinês simplificado
│   ├── zh-HK/                       # Chinês tradicional
│   ├── de-DE/                       # Alemão
│   ├── fr-FR/                       # Francês
│   ├── es-ES/                       # Espanhol
│   ├── ja-JP/                       # Japonês
│   ├── ko-KR/                       # Coreano
│   ├── cs-CZ/                       # Checo
│   ├── it-IT/                       # Italiano
│   ├── pl-PL/                       # Polaco
│   └── pt-PT/                       # Português
├── 总文档/                           # Documentos de requisitos e arquitetura (Chinês)
└── README.md                        # Visão geral do projeto
```

## Precisas de ajuda?

- 📖 Consultar o [sistema de documentação de ajuda](web-ui-guide.md#帮助文档系统新增) (suporte multilingue)
- 📚 Ler a [documentação completa](docs/)
- 🐛 Reportar problemas no [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Participar nas discussões da comunidade
