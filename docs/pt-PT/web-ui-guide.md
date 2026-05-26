# Guia da Web UI

> **Versão: v0.2.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md) | [Русский](../ru-RU/web-ui-guide.md)

## Visão Geral

A Web UI fornece uma interface abrangente para gerir Silicon Beings, monitorizar o estado do sistema e interagir com agentes de IA. O sistema adopta uma arquitectura de renderização pura do lado do servidor, com zero dependência de frameworks frontend, gerando HTML, CSS e JavaScript através dos construtores `H`, `CssBuilder` e `JsBuilder`.

## Acesso

URL predefinido: `http://localhost:8080`

## Navegação

### Secções Principais

1. **Painel** - Visão geral e métricas do sistema
2. **Beings** - Gestão dos Silicon Beings
3. **Chat** - Interacção com os beings (suporte a carregamento de ficheiros, SSE em tempo real)
4. **Histórico de Chat** - Visualizar o histórico de chat dos Silicon Beings (lista de sessões, detalhes de mensagens)
5. **Tarefas** - Gestão de tarefas (tarefas pessoais)
6. **Temporizadores** - Configuração de temporizadores (criar, pausar, histórico de execução)
7. **Configuração** - Definições do sistema (clientes de IA, localização)
8. **Permissões** - Controlo de acesso (gestão ACL, consulta de permissões)
9. **Registos** - Registos do sistema (filtro por nível, consulta por intervalo de tempo)
10. **Auditoria** - Utilização de tokens e registo de auditoria
11. **Memória** - Memória dos beings (vista de linha temporal, filtragem avançada)
12. **Conhecimento** - Base de conhecimento (gestão de triplas, descoberta de caminhos)
13. **Navegador de Código** - Exploração de código (árvore de ficheiros, destaque de sintaxe)
14. **Editor de Código** - Edição de código com dicas flutuantes (Monaco Editor)
15. **Projecto** - Gestão de projectos (espaço de trabalho, tarefas, notas de trabalho)
16. **Executores** - Gestão de executores (disco, rede, linha de comandos)
17. **Ajuda** - Sistema de documentação de ajuda (suporte multilingue, pesquisa por tópicos)
18. **Sobre** - Informações do sistema e versão

---

## Painel

### Funcionalidades

- Métricas de desempenho do sistema (CPU, memória, tempo de execução)
- Visão geral do estado dos beings
- Estatísticas de utilização da IA
- Acções rápidas

### Actualizações em Tempo Real

Usar SSE (Server-Sent Events) para obter dados em tempo real:

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Gestão de Beings

### Lista de Beings

Mostra todos os beings, incluindo:
- Nome e ID
- Estado actual (Em Execução / Parado / Erro)
- Ligação ao Ficheiro da Alma
- Acções rápidas (Iniciar / Parar / Configurar)

### Detalhes do Being

- Configuração completa
- Editor do Ficheiro da Alma
- Histórico de tarefas
- Visualizador de memória
- Métricas de desempenho

### Criar Being

1. Clicar em **Criar Novo Being**
2. Preencher:
   - Nome
   - Conteúdo da Alma (editor Markdown)
   - Configuração inicial
3. Clicar em **Criar**

---

## Interface de Chat

### Funcionalidades

- Fluxo de mensagens em tempo real
- Histórico de mensagens
- Suporte a múltiplas sessões
- Visualização de chamadas de ferramentas

### Usar o Chat

1. Seleccionar um being
2. Introduzir mensagem
3. Ver resposta em streaming
4. Ver execução de ferramentas em tempo real

### Visualização de Chamadas de Ferramentas

Quando a IA chama uma ferramenta:
```
🔧 Ferramenta: calendar
📥 Entrada: {"date": "2026-04-20"}
📤 Saída: "Lunar 3 do 4º mês"
```

---

## Configuração

### Clientes de IA

Configurar o backend de IA:
- Ollama (local)
- DashScope (nuvem)
- Volcengine Ark (nuvem)
- Clientes personalizados

### Definições de Armazenamento

- Versão Default: Caminho base, índice temporal, política de limpeza
- Versão Fast: Configuração do motor de armazenamento SpeedyPack, gestão de ficheiros .spk, definições de compactação automática

### Localização

Alternar entre 34 variantes linguísticas:
- Chinês (6 variantes): Chinês simplificado, Chinês tradicional, Chinês de Singapura, Chinês de Macau, Chinês de Taiwan, Chinês da Malásia
- Inglês (10 variantes): Americano, Britânico, Canadiano, Australiano, Indiano, Singapurano, Sul-africano, Irlandês, Neozelandês, Inglês da Malásia
- Espanhol (2 variantes): Espanha, México
- Alemão (5 variantes): Alemanha, Áustria, Suíça, Luxemburgo, Liechtenstein
- Francês (3 variantes): França, Canadá, Suíça
- Japonês, Coreano, Checo
- Russo, Português (2 variantes), Italiano, Holandês, Polaco, Sueco

---

## Sistema de Skins

### Skins Disponíveis

1. **Admin** - Interface de administração profissional
2. **Chat** - Desenho centrado na conversação
3. **Creative** - Estilo criativo e artístico
4. **Dev** - Layout orientado ao programador
5. **HighContrast** - Tema de alto contraste (versão Fast)
6. **Minimal** - Estilo minimalista (versão Fast)
7. **Light** - Tema claro (versão Fast)

### Trocar de Skin

1. Clicar em **Definições** (ícone de engrenagem)
2. Seleccionar **Skin**
3. Escolher a skin desejada
4. A interface actualiza imediatamente

### Skin Personalizada

Criar uma skin personalizada implementando `ISkin`:

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

## Gestão de Permissões

### Ver Permissões

- Listar todas as regras de permissões
- Filtrar por utilizador ou recurso
- Ver datas de expiração

### Adicionar Regra de Permissão

1. Clicar em **Adicionar Regra**
2. Configurar:
   - Tipo de permissão (por exemplo `FileAccess`, `NetworkAccess`)
   - Prefixo do recurso (por exemplo `C:\Projects`, `api.github.com`)
   - Permitir / Negar
   - Descrição
3. Guardar

### Registo de Auditoria

Ver todas as decisões de permissões:
- Timestamp
- Utilizador
- Recurso
- Decisão
- Motivo

### Gestão de Permissões de Ferramentas

Gerir permissões de operações de ferramentas dos Silicon Beings e projectos:

1. **Permissões de ferramentas do Silicon Being**:
   - Navegar para **Beings** → Seleccionar being → **Permissões de Ferramentas**
   - Ver configuração actual de permissões
   - Definir permitir/negar por operação
   - Aplicar modelo de permissões (readonly/restricted/full)

2. **Permissões de ferramentas do projecto**:
   - Navegar para **Projecto** → Seleccionar projecto → **Permissões de Ferramentas**
   - As permissões de ferramentas ao nível do projecto são independentes do nível do Silicon Being
   - Realizar isolamento de permissões entre projectos

---

## Gestão de Tarefas

### Lista de Tarefas

- Todas as tarefas e o seu estado
- Filtrar por being ou estado
- Indicadores de prioridade

### Detalhes da Tarefa

- Descrição
- Prioridade
- Data limite
- Histórico de execução
- Resultado de saída

### Criar Tarefa

1. Clicar em **Criar Tarefa**
2. Preencher:
   - Atribuição ao being
   - Descrição
   - Prioridade (1-10)
   - Data limite
3. Criar

---

## Gestão de Temporizadores

### Temporizadores Activos

- Lista de temporizadores em execução
- Próximo tempo de execução
- Estado de repetição

### Criar Temporizador

1. Clicar em **Criar Temporizador**
2. Configurar:
   - Atribuição ao being
   - Intervalo ou expressão cron
   - Acção a executar
   - Definições de repetição
3. Iniciar

---

## Visualizador de Registos

### Funcionalidades

- Filtrar por nível (Informação / Aviso / Erro)
- Pesquisar por palavra-chave
- Selecção de intervalo de tempo
- Actualizações em tempo real

### Detalhes dos Registos

Cada entrada de registo mostra:
- Timestamp
- Nível
- Origem
- Mensagem
- Stack trace (para erros)

---

## Relatório de Auditoria

### Utilização de Tokens

- Total de tokens utilizados
- Decomposição por modelo
- Cálculo de custos
- Gráficos baseados no tempo

### Exportar Relatório

Descarregar dados de auditoria:
- Formato CSV
- Selecção de intervalo de datas
- Filtrar por being ou modelo

---

## Editor de Código

### Funcionalidades

- Destaque de sintaxe (Monaco Editor)
- Autocompletar de código
- Dicas flutuantes para identificadores
- Compilação em tempo real

### Dicas Flutuantes

Passar o rato sobre qualquer identificador para ver:
- Informação de tipo
- Documentação
- Localização da definição
- Referências

---

## Visualização do Histórico de Chat

### Funcionalidades

- Navegação no histórico de chat dos Silicon Beings
- Apresentação da lista de sessões
- Visualização dos detalhes das mensagens
- Vista de linha temporal

### Usar o Histórico de Chat

1. Navegar para a página **Beings**
2. Clicar na ligação **Histórico de Chat** do Silicon Being
3. Ver a lista de sessões:
   - Título da sessão
   - Hora de criação
   - Número de mensagens
4. Clicar na sessão para ver detalhes:
   - Histórico completo de mensagens
   - Timestamps
   - Informação do remetente
   - Registos de chamadas de ferramentas

### Implementação Técnica

- **Controlador**: `ChatHistoryController`
- **Modelo de vista**: `ChatHistoryViewModel`
- **Vistas**:
  - `ChatHistoryListView` - Lista de sessões
  - `ChatHistoryDetailView` - Detalhes das mensagens
- **Rotas da API**:
  - `/api/chat-history/{beingId}/conversations` - Obter lista de sessões
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Obter detalhes das mensagens

---

## Carregamento de Ficheiros

### Funcionalidades

- Diálogo de origem de ficheiros
- Suporte a carregamento de múltiplos ficheiros
- Gestão de metadados de ficheiros
- Indicador de progresso do carregamento

### Usar o Carregamento de Ficheiros

1. Clicar no botão **Carregar Ficheiro** na interface de chat
2. O diálogo de origem de ficheiros abre
3. Seleccionar a origem do ficheiro:
   - Ficheiro local
   - Caminho no sistema de ficheiros
4. Seleccionar ficheiros (suporte a selecção múltipla)
5. Confirmar carregamento
6. A informação do ficheiro será anexada à mensagem

### Tipos de Ficheiro Suportados

- Ficheiros de texto (.txt, .md, .json, .xml, etc.)
- Ficheiros de código (.cs, .js, .py, .java, etc.)
- Ficheiros de configuração (.yml, .yaml, .ini, .conf, etc.)
- Ficheiros de documento (.csv, .log, etc.)

---

## Indicador de Carregamento

### Funcionalidades

- Visualização do estado de carregamento da página de chat
- Selecção automática da sessão do Curator
- Feedback do progresso de carregamento de dados

### Comportamento

- Mostrar animação de carregamento durante o carregamento da página
- Ocultar automaticamente após conclusão do carregamento de dados
- Sessão do Curator seleccionada automaticamente (se existir)
- Texto de dica de carregamento multilingue

---

## Sistema de Documentação de Ajuda (Novo)

### Visão Geral das Funcionalidades

O sistema de documentação de ajuda fornece suporte de documentação multilingue para Silicon Beings e utilizadores.

### Usar a Documentação de Ajuda

1. Navegar para a página **Ajuda**
2. Ver a lista de tópicos de ajuda:
   - Guia de início rápido
   - Referência de utilização de ferramentas
   - Guia de gestão de permissões
   - Manual de resolução de problemas
   - Guia de desenvolvimento
3. Clicar num tópico para ver conteúdo detalhado:
   - Conteúdo documental estruturado (renderização Markdown)
   - Suporte multilingue (segue as definições de localização do sistema)
   - Recomendação de tópicos relacionados
4. Usar a funcionalidade de pesquisa para localizar rapidamente:
   - Pesquisa por palavra-chave (suporta chinês e inglês)
   - Resultados de pesquisa ordenados por relevância

### Silicon Beings Acedem à Ajuda

Os Silicon Beings podem aceder à documentação de ajuda através da ferramenta `help`:
```json
{
  "action": "get_topics"
}
```

### Implementação Técnica

- **Controlador**: `HelpController`
- **Ferramenta**: `HelpTool`
- **Rotas da API**:
  - `/api/help` - Obter lista de tópicos de ajuda
  - `/api/help/{topicId}` - Obter detalhes do tópico
  - `/api/help/search?q=keyword` - Pesquisar documentação de ajuda

---

## Espaço de Trabalho de Projecto (Novo)

### Visão Geral das Funcionalidades

O espaço de trabalho de projecto fornece um ambiente de trabalho estruturado, suportando gestão de projectos, acompanhamento de tarefas e notas de trabalho.

### Gestão de Projectos

1. **Criar projecto**:
   - Nome e descrição do projecto
   - Tags do projecto (categorização)
   - Estado do projecto (Em curso, Concluído, Arquivado)
2. **Ver detalhes do projecto**:
   - Informação básica do projecto
   - Lista de tarefas associadas
   - Lista de notas de trabalho
   - Estatísticas de progresso do projecto
3. **Arquivar projecto**: Manter dados históricos mas deixar de estar activo
4. **Gestão de funções do projecto**:
   - Atribuir funções de projecto aos Silicon Beings (por exemplo developer, reviewer, manager)
   - Remover atribuições de funções
   - Ver lista de membros e funções do projecto
5. **Fluxos de trabalho do projecto**:
   - Ver lista de modelos de fluxos de trabalho
   - Vincular modelo de fluxo de trabalho ao projecto
   - Ver estado da instância do fluxo de trabalho
   - Ver registos de execução do fluxo de trabalho

### Notas de Trabalho (Privadas)

Notas de trabalho pessoais dos Silicon Beings, semelhantes a um diário:

1. **Criar nota**:
   - Resumo (descrição breve)
   - Conteúdo (suporta formato Markdown)
   - Palavras-chave (para pesquisa)
   - Registo automático de timestamps
2. **Gerir notas**:
   - Navegar por linha temporal (design por páginas)
   - Pesquisar notas (por palavra-chave, resumo, conteúdo)
   - Gerar directório (navegação rápida da estrutura das notas)
   - Actualizar e eliminar notas
3. **Controlo de permissões**:
   - Privadas por defeito, apenas o próprio being pode aceder
   - O Silicon Curator pode gerir todas as notas

### Implementação Técnica

- **Controlador**: `WorkNoteController`
- **Ferramentas**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **Rotas da API**:
  - `/api/worknotes` - Obter lista de notas de trabalho
  - `/api/worknotes/{id}` - Obter detalhes da nota
  - `/api/worknotes/search?q=keyword` - Pesquisar notas
  - `/api/worknotes/directory` - Gerar directório de notas
  - `/api/projects` - API de gestão de projectos

---

## Design Responsivo

A Web UI adapta-se a diferentes tamanhos de ecrã:
- Desktop: Layout completo
- Tablet: Barra lateral comprimida
- Mobile: Menu recolhível

---

## Atalhos de Teclado

| Atalho | Acção |
|----------|--------|
| `Ctrl+K` | Pesquisa rápida |
| `Ctrl+B` | Alternar barra lateral |
| `Ctrl+Enter` | Enviar mensagem |
| `Esc` | Cancelar / Fechar |

---

## Resolução de Problemas

### Não é Possível Ligar

**Verificar**:
- O servidor está em execução
- A porta 8080 não está bloqueada
- Definições da firewall

### SSE Não Funciona

**Verificar**:
- O navegador suporta SSE
- Nenhum proxy está a fazer buffer do SSE
- Estabilidade da rede

### Desempenho Lento

**Optimizar**:
- Reduzir o nível de detalhe dos registos
- Limpar dados de auditoria antigos
- Verificar recursos do sistema

---

## Próximos Passos

- 📚 Leia o [guia de arquitectura](architecture.md)
- 🛠️ Consulte o [guia de desenvolvimento](development-guide.md)
- 📖 Explore a [referência API](api-reference.md)
- 🚀 Consulte o [guia de início rápido](getting-started.md)
