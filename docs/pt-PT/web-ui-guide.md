# Guia da interface Web

> **Versão: v0.2.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [Français](../fr-FR/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md) | [Русский](../ru-RU/web-ui-guide.md) | [Italiano](../it-IT/web-ui-guide.md) | [Polski](../pl-PL/web-ui-guide.md) | **Português**

## Visão geral

A interface Web fornece um painel abrangente para gerir Silicon Beings, monitorizar o estado do sistema e interagir com agentes IA. O sistema adota uma arquitetura de renderização puramente do lado do servidor, sem dependências de frameworks frontend, gerando HTML, CSS e JavaScript através dos construtores `H`, `CssBuilder` e `JsBuilder`.

## Acesso

URL por defeito: `http://localhost:8080`

## Navegação

### Secções principais

1. **Painel** — Visão geral e métricas do sistema
2. **Beings** — Gestão dos Silicon Beings
3. **Chat** — Interação com os Beings (suporta upload de ficheiros, SSE em tempo real)
4. **Histórico de chat** — Ver histórico de conversações dos Silicon Beings (lista de sessões, detalhes das mensagens)
5. **Tarefas** — Gestão de tarefas (tarefas pessoais)
6. **Temporizadores** — Configuração de temporizadores (criar, pausar, histórico de execução)
7. **Configuração** — Definições do sistema (clientes IA, localização)
8. **Permissões** — Controlo de acesso (gestão ACL, consulta de permissões)
9. **Registos** — Registos do sistema (filtrar por nível, consultar por intervalo de tempo)
10. **Auditoria** — Utilização de tokens e registo de auditoria
11. **Memória** — Memória dos Beings (vista de timeline, filtragem avançada)
12. **Conhecimento** — Base de conhecimento (gestão de tríades, descoberta de caminhos)
13. **Navegador de código** — Exploração de código (árvore de ficheiros, realce de sintaxe)
14. **Editor de código** — Edição de código com sugestões flutuantes (Monaco Editor)
15. **Projetos** — Gestão de projetos (área de trabalho, tarefas, notas de trabalho)
16. **Executor** — Gestão do executor (disco, rede, linha de comandos)
17. **Ajuda** — Sistema de documentação de ajuda (suporte multilingue, pesquisa por tópicos)
18. **Sobre** — Informações do sistema e versão

---

## Painel

### Funcionalidades

- Métricas de desempenho do sistema (CPU, memória, tempo de atividade)
- Visão geral do estado dos Beings
- Estatísticas de utilização da IA
- Ações rápidas

### Atualizações em tempo real

Utilizar SSE (Server-Sent Events) para obter dados em tempo real:

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Gestão de Silicon Beings

### Lista de Beings

Mostra todos os Silicon Beings, incluindo:
- Nome e ID
- Estado atual (em execução / parado / erro)
- Ligação ao ficheiro da alma
- Ações rápidas (iniciar / parar / configurar)

### Detalhes do Being

- Configuração completa
- Editor do ficheiro da alma
- Histórico de tarefas
- Visualizador de memória
- Métricas de desempenho

### Criar Being

1. Clicar em **Criar novo Being**
2. Preencher:
   - Nome
   - Conteúdo da alma (editor Markdown)
   - Configuração inicial
3. Clicar em **Criar**

---

## Interface de chat

### Funcionalidades

- Fluxo de mensagens em tempo real
- Histórico de mensagens
- Suporte para múltiplas sessões
- Visualização de chamadas de ferramentas

### Utilizar o chat

1. Selecionar um Being
2. Introduzir a mensagem
3. Ver a resposta em streaming
4. Ver a execução de ferramentas em tempo real

### Visualização de chamadas de ferramentas

Quando a IA chama uma ferramenta:
```
🔧 Ferramenta: calendar
📥 Entrada: {"date": "2026-04-20"}
📤 Saída: "农历四月初三"
```

---

## Configuração

### Clientes IA

Configurar os backends IA:
- Ollama (local)
- DashScope (nuvem)
- Volcengine Ark (nuvem)
- Clientes personalizados

### Definições de armazenamento

- Versão Default: caminho base, índice temporal, política de limpeza
- Versão Fast: configuração do motor de armazenamento SpeedyPack, gestão de ficheiros .spk, definições de compressão automática

### Localização

Alternar entre 29 variantes linguísticas:
- Chinês (6 variantes): Simplificado, Tradicional, Singapura, Macau, Taiwan, Malásia
- Inglês (10 variantes): EUA, Reino Unido, Canadá, Austrália, Índia, Singapura, África do Sul, Irlanda, Nova Zelândia, Malásia
- Espanhol (2 variantes): Espanha, México
- Alemão (5 variantes): Alemanha, Áustria, Suíça, Luxemburgo, Liechtenstein
- Francês (3 variantes): França, Canadá, Suíça
- Japonês, Coreano, Checo

---

## Sistema de skins

### Skins disponíveis

1. **Admin** — Interface de gestão profissional
2. **Chat** — Design centrado na conversação
3. **Creative** — Estilo criativo e artístico
4. **Dev** — Layout orientado ao programador
5. **HighContrast** — Tema de alto contraste (versão Fast)
6. **Minimal** — Estilo minimalista (versão Fast)
7. **Light** — Tema claro (versão Fast)

### Trocar de skin

1. Clicar em **Definições** (ícone de engrenagem)
2. Selecionar **Skin**
3. Selecionar a skin desejada
4. A interface atualiza imediatamente

### Skin personalizada

Criar uma skin personalizada através da implementação de `ISkin`:

```csharp
public class MySkin : ISkin
{
    public string Name => "MySkin";

    public string GetCss()
    {
        return ":root { --primary: #color; }";
    }
}
```

---

## Gestão de permissões

### Ver permissões

- Listar todas as regras de permissão
- Filtrar por utilizador ou recurso
- Ver datas de expiração

### Adicionar regra de permissão

1. Clicar em **Adicionar regra**
2. Configurar:
   - Utilizador
   - Recurso (por exemplo `disk:read`)
   - Permitir / Negar
   - Duração
3. Guardar

### Registo de auditoria

Ver todas as decisões de permissão:
- Timestamp
- Utilizador
- Recurso
- Decisão
- Motivo

---

## Gestão de tarefas

### Lista de tarefas

- Todas as tarefas e respetivos estados
- Filtrar por Being ou estado
- Indicadores de prioridade

### Detalhes da tarefa

- Descrição
- Prioridade
- Data limite
- Histórico de execução
- Resultado

### Criar tarefa

1. Clicar em **Criar tarefa**
2. Preencher:
   - Being atribuído
   - Descrição
   - Prioridade (1-10)
   - Data limite
3. Criar

---

## Gestão de temporizadores

### Temporizadores ativos

- Lista de temporizadores em execução
- Próxima execução
- Estado de repetição

### Criar temporizador

1. Clicar em **Criar temporizador**
2. Configurar:
   - Being atribuído
   - Intervalo ou expressão cron
   - Ação a executar
   - Definições de repetição
3. Iniciar

---

## Visualizador de registos

### Funcionalidades

- Filtrar por nível (Informação / Aviso / Erro)
- Pesquisar por palavra-chave
- Seleção de intervalo de tempo
- Atualizações em tempo real

### Detalhes do registo

Cada entrada de registo mostra:
- Timestamp
- Nível
- Origem
- Mensagem
- Stack trace (para erros)

---

## Relatório de auditoria

### Utilização de tokens

- Total de tokens utilizados
- Discriminação por modelo
- Cálculo de custos
- Gráficos baseados no tempo

### Exportar relatório

Descarregar dados de auditoria:
- Formato CSV
- Seleção de intervalo de datas
- Filtrar por Being ou modelo

---

## Editor de código

### Funcionalidades

- Realce de sintaxe (Monaco Editor)
- Autocompletar de código
- Sugestões flutuantes para identificadores
- Compilação em tempo real

### Sugestões flutuantes

Passar o rato sobre qualquer identificador para ver:
- Informação de tipo
- Documentação
- Localização da definição
- Referências

---

## Histórico de chat

### Funcionalidades

- Navegação no histórico de conversações dos Silicon Beings
- Lista de sessões
- Detalhes das mensagens
- Vista de timeline

### Utilizar o histórico de chat

1. Navegar para a página **Beings**
2. Clicar na ligação **Histórico de chat** do Silicon Being
3. Ver a lista de sessões:
   - Título da sessão
   - Data de criação
   - Número de mensagens
4. Clicar na sessão para ver detalhes:
   - Histórico completo de mensagens
   - Timestamps
   - Informação do remetente
   - Registos de chamadas de ferramentas

### Implementação técnica

- **Controlador**: `ChatHistoryController`
- **Modelo de vista**: `ChatHistoryViewModel`
- **Vistas**:
  - `ChatHistoryListView` — Lista de sessões
  - `ChatHistoryDetailView` — Detalhes das mensagens
- **Rotas da API**:
  - `/api/chat-history/{beingId}/conversations` — Obter lista de sessões
  - `/api/chat-history/{beingId}/conversation/{conversationId}` — Obter detalhes das mensagens

---

## Upload de ficheiros

### Funcionalidades

- Diálogo de origem de ficheiros
- Suporte para upload de múltiplos ficheiros
- Gestão de metadados de ficheiros
- Indicação de progresso do upload

### Utilizar o upload de ficheiros

1. Na interface de chat, clicar no botão **Upload de ficheiro**
2. O diálogo de origem de ficheiros abre
3. Selecionar a origem do ficheiro:
   - Ficheiro local
   - Caminho no sistema de ficheiros
4. Selecionar ficheiros (suporta seleção múltipla)
5. Confirmar o upload
6. A informação do ficheiro será anexada à mensagem

### Tipos de ficheiro suportados

- Ficheiros de texto (.txt, .md, .json, .xml, etc.)
- Ficheiros de código (.cs, .js, .py, .java, etc.)
- Ficheiros de configuração (.yml, .yaml, .ini, .conf, etc.)
- Ficheiros de documentos (.csv, .log, etc.)

---

## Indicadores de carregamento

### Funcionalidades

- Apresentação do estado de carregamento na página de chat
- Seleção automática da sessão do Curator
- Feedback do progresso de carregamento de dados

### Comportamento

- Animação de carregamento ao carregar a página
- Ocultação automática após conclusão do carregamento de dados
- Sessão do Curator selecionada automaticamente (se existir)
- Texto de carregamento multilingue

---

## Sistema de documentação de ajuda

### Visão geral

O sistema de documentação de ajuda fornece suporte multilingue de documentação para Silicon Beings e utilizadores.

### Utilizar a documentação de ajuda

1. Navegar para a página **Ajuda**
2. Ver a lista de tópicos de ajuda:
   - Guia de arranque rápido
   - Referência de utilização de ferramentas
   - Guia de gestão de permissões
   - Manual de resolução de problemas
   - Guia de desenvolvimento
3. Clicar no tópico para ver conteúdo detalhado:
   - Conteúdo documental estruturado (renderização Markdown)
   - Suporte multilingue (segue as definições de localização do sistema)
   - Recomendação de tópicos relacionados
4. Utilizar a função de pesquisa para localizar rapidamente:
   - Pesquisa por palavra-chave (suporta chinês e inglês)
   - Resultados ordenados por relevância

### Silicon Beings acedem à ajuda

Os Silicon Beings podem aceder à documentação de ajuda através da ferramenta `help`:
```json
{
  "action": "get_topics"
}
```

### Implementação técnica

- **Controlador**: `HelpController`
- **Ferramenta**: `HelpTool`
- **Rotas da API**:
  - `/api/help` — Obter lista de tópicos de ajuda
  - `/api/help/{topicId}` — Obter detalhes do tópico
  - `/api/help/search?q=keyword` — Pesquisar documentação de ajuda

---

## Área de trabalho de projetos

### Visão geral

A área de trabalho de projetos fornece um ambiente de trabalho estruturado, suportando gestão de projetos, acompanhamento de tarefas e notas de trabalho.

### Gestão de projetos

1. **Criar projeto**:
   - Nome e descrição do projeto
   - Etiquetas do projeto (categorização)
   - Estado do projeto (em curso, concluído, arquivado)
2. **Ver detalhes do projeto**:
   - Informação básica do projeto
   - Lista de tarefas associadas
   - Lista de notas de trabalho
   - Estatísticas de progresso do projeto
3. **Arquivar projeto**: Manter dados históricos sem estar ativo

### Notas de trabalho (privadas)

Notas de trabalho pessoais dos Silicon Beings, semelhantes a um diário:

1. **Criar nota**:
   - Resumo (descrição breve)
   - Conteúdo (suporta formato Markdown)
   - Palavras-chave (para pesquisa)
   - Registo automático de timestamp
2. **Gerir notas**:
   - Navegar por timeline (design paginado)
   - Pesquisar notas (por palavra-chave, resumo, conteúdo)
   - Gerar índice (navegação rápida pela estrutura das notas)
   - Atualizar e eliminar notas
3. **Controlo de permissões**:
   - Privadas por defeito, apenas o próprio Being tem acesso
   - O Curator pode gerir todas as notas

### Implementação técnica

- **Controlador**: `WorkNoteController`
- **Ferramentas**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **Rotas da API**:
  - `/api/worknotes` — Obter lista de notas de trabalho
  - `/api/worknotes/{id}` — Obter detalhes da nota
  - `/api/worknotes/search?q=keyword` — Pesquisar notas
  - `/api/worknotes/directory` — Gerar índice de notas
  - `/api/projects` — API de gestão de projetos

---

## Componentes da interface

### Componentes declarativos

A interface Web utiliza uma arquitetura Component UI com 27 componentes declarativos:

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

## Design responsivo

A interface Web adapta-se a diferentes tamanhos de ecrã:
- Desktop: Layout completo
- Tablet: Barra lateral comprimida
- Mobile: Menu recolhível

---

## Atalhos de teclado

| Atalho | Ação |
|----------|--------|
| `Ctrl+K` | Pesquisa rápida |
| `Ctrl+B` | Alternar barra lateral |
| `Ctrl+Enter` | Enviar mensagem |
| `Esc` | Cancelar / Fechar |

---

## Resolução de problemas

### Impossível ligar

**Verificar**:
- O servidor está em execução
- A porta 8080 não está bloqueada
- Definições da firewall

### SSE não funciona

**Verificar**:
- O browser suporta SSE
- Nenhum proxy está a armazenar em buffer o SSE
- Estabilidade da rede

### Desempenho lento

**Otimizar**:
- Reduzir o nível de detalhe dos registos
- Limpar dados de auditoria antigos
- Verificar os recursos do sistema

---

## Próximos passos

- 📚 Ler o [guia de arquitetura](architecture.md)
- 🛠️ Consultar o [guia de desenvolvimento](development-guide.md)
- 📖 Explorar a [referência da API](api-reference.md)
- 🚀 Ver o [guia de introdução](getting-started.md)
