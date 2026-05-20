# Guia da interface Web

> **Versão: v0.2.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [Français](../fr-FR/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md) | [Italiano](../it-IT/web-ui-guide.md) | [Polski](../pl-PL/web-ui-guide.md) | **Português**

## Visão geral

O SiliconLifeCollective fornece uma interface Web integrada para gerir e interagir com os Silicon Beings. A interface Web é servida pelo servidor HTTP integrado e está acessível em `http://localhost:8080`.

---

## Funcionalidades da interface

### Painel principal

O painel principal mostra uma visão geral do sistema:

- **Estado dos Beings** — Número de Beings em execução/parados
- **Utilização de tokens** — Tokens IA consumidos hoje
- **Desempenho do sistema** — CPU, memória, tempo de atividade
- **Atividade recente** — Últimas ações dos Beings

### Gestão de Silicon Beings

#### Lista de Beings

A página de gestão dos Beings mostra todos os Silicon Beings registados:

| Coluna | Descrição |
|--------|-------------|
| Nome | Nome de exibição do Being |
| Estado | idle / running / waiting_permission / stopped |
| Última atividade | Timestamp da última ação |
| Tokens | Tokens consumidos na sessão atual |

#### Operações

- **Iniciar** — Iniciar um Being parado
- **Parar** — Parar um Being em execução
- **Editar** — Modificar o ficheiro da alma do Being
- **Eliminar** — Remover um Being

### Sistema de chat

#### Tipos de chat

| Tipo | Descrição |
|------|-------------|
| Chat individual | Conversação um-a-um com um Silicon Being |
| Chat de grupo | Conversação com múltiplos Beings |
| Canal de broadcast | Mensagens de broadcast para todos os Beings |

#### Funcionalidades do chat

- **Streaming em tempo real** — As respostas da IA são transmitidas em tempo real via SSE
- **Raciocínio visível** — A cadeia de pensamento da IA pode ser exibida
- **Chamadas de ferramenta** — As chamadas de ferramenta da IA são visíveis
- **Histórico** — Navegação no histórico de conversações

### Painel de configuração

#### Configuração IA

Configurar os backends IA:

| Campo | Descrição |
|-------|-------------|
| Tipo de cliente | Ollama / DashScope / VolcengineArk |
| Endpoint | URL do serviço IA |
| Modelo | Nome do modelo IA |
| Chave API | Chave de autenticação (serviços na nuvem) |
| Temperatura | Temperatura de geração (0-2) |
| Tokens máximos | Número máximo de tokens na resposta |

#### Configuração do sistema

| Campo | Descrição |
|-------|-------------|
| Porta HTTP | Porta do servidor Web (por defeito: 8080) |
| Caminho dos dados | Diretório base de armazenamento |
| Idioma | Idioma da interface |
| Nível de registo | Nível de verbosidade dos logs |

### Página de informações

A página de informações mostra:

- **Versão do sistema** — Número de versão atual
- **Runtime** — Informações do runtime .NET
- **Plugins carregados** — Lista dos plugins registados
- **Estado do sistema** — Estado de saúde dos componentes

---

## Sistema de documentação de ajuda

### Visão geral

O sistema de documentação de ajuda fornece documentação integrada e pesquisável, suportando múltiplos idiomas:

- **Pesquisa de texto completo** — Pesquisa em todos os tópicos de ajuda
- **Multilingue** — Suporta 11+ idiomas
- **Navegação por categorias** — Organização por tópicos
- **Atualização em tempo real** — Os tópicos são carregados a partir de ficheiros Markdown

### Aceder à documentação de ajuda

1. Na interface Web, clica no ícone **?** no canto superior direito
2. Ou navega para `/help` no browser
3. Usa a barra de pesquisa para encontrar tópicos específicos

### Categorias da documentação

| Categoria | Descrição |
|-----------|-------------|
| Introdução | Guias de arranque rápido |
| Funcionalidades | Descrição das funcionalidades |
| Configuração | Guias de configuração |
| API | Documentação da API |
| Resolução de problemas | Soluções para problemas comuns |

---

## Personalização da interface

### Temas

A interface Web suporta 7 temas visuais:

| Tema | Descrição |
|------|-------------|
| Claro | Tema claro padrão |
| Escuro | Tema escuro padrão |
| Azul | Tema azul |
| Verde | Tema verde |
| Roxo | Tema roxo |
| Laranja | Tema laranja |
| Alto contraste | Tema de alto contraste |

A deteção automática do tema do sistema é suportada.

### Idiomas

A interface Web suporta os seguintes idiomas:

| Código | Idioma |
|--------|---------|
| en | English |
| zh-CN | 中文（简体） |
| zh-HK | 繁體中文 |
| de-DE | Deutsch |
| fr-FR | Français |
| es-ES | Español |
| ja-JP | 日本語 |
| ko-KR | 한국어 |
| cs-CZ | Čeština |
| it-IT | Italiano |
| pl-PL | Polski |
| pt-PT | Português |

---

## Componentes da interface

### Componentes declarativos

A interface Web utiliza uma arquitetura Component UI com 30+ componentes declarativos:

| Componente | Descrição |
|-----------|-------------|
| `TextComponent` | Exibição de texto |
| `ButtonComponent` | Botão interativo |
| `InputComponent` | Campo de entrada de texto |
| `SelectComponent` | Menu dropdown de seleção |
| `TableComponent` | Exibição de dados tabulares |
| `CardComponent` | Cartão de conteúdo |
| `ModalComponent` | Janela modal |
| `TabComponent` | Separadores de navegação |
| `ChartComponent` | Gráficos de dados |
| `FormComponent` | Formulário com validação |

### Hot reload

A interface Web suporta hot reload para atualizações em tempo real:

- Atualização dos componentes sem recarregar a página
- Reinício online dos serviços
- Aplicação imediata das alterações de configuração

---

## API da interface Web

### Endpoints principais

| Endpoint | Método | Descrição |
|----------|--------|-------------|
| `/` | GET | Página principal |
| `/chat` | GET | Página de chat |
| `/beings` | GET | Página de gestão dos Beings |
| `/config` | GET | Página de configuração |
| `/about` | GET | Página de informações |
| `/help` | GET | Documentação de ajuda |
| `/permission` | GET | Página de permissões |

### Endpoints da API

| Endpoint | Método | Descrição |
|----------|--------|-------------|
| `/api/chat/conversations` | GET | Obter a lista de conversações |
| `/api/chat/messages` | GET | Obter o histórico de mensagens |
| `/api/chat/send` | POST | Enviar uma mensagem |
| `/api/chat/stream` | GET | Stream SSE |
| `/api/beings` | GET | Obter a lista de Beings |
| `/api/beings/{id}` | GET | Obter os detalhes de um Being |
| `/api/beings/{id}/start` | POST | Iniciar um Being |
| `/api/beings/{id}/stop` | POST | Parar um Being |
| `/api/config` | GET | Obter a configuração |
| `/api/config` | POST | Atualizar a configuração |
| `/api/dashboard` | GET | Dados do painel |
| `/api/status` | GET | Estado do sistema |

---

## Resolução de problemas

### A interface Web não carrega

**Problema**: O browser não consegue aceder a `http://localhost:8080`

**Soluções**:
1. Verificar se a aplicação está em execução
2. Verificar se a porta 8080 não está ocupada
3. Tentar aceder a `http://127.0.0.1:8080`

### As respostas do chat são lentas

**Problema**: As respostas da IA demoram muito

**Soluções**:
1. Verificar a ligação ao serviço IA
2. Reduzir o parâmetro `maxTokens`
3. Usar um modelo mais rápido

### Os caracteres são exibidos incorretamente

**Problema**: Caracteres ilegíveis na interface

**Soluções**:
1. Verificar a codificação do browser (UTF-8)
2. Alterar o idioma da interface
3. Limpar a cache do browser

---

## Próximos passos

- 🚀 Ler o [guia de introdução](getting-started.md)
- 📚 Consultar a [documentação de arquitetura](architecture.md)
- 📖 Explorar a [referência da API](api-reference.md)
- 🛠️ Ler o [guia de desenvolvimento](development-guide.md)
