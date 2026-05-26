# Guia do Silicon Being

> **Versão: v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md)

## Visão Geral

Os Silicon Beings são agentes orientados por IA que podem pensar, agir e evoluir autonomamente.

## Arquitectura

### Separação Corpo-Cérebro

```
┌─────────────────────────────────────┐
│         Silicon Being               │
├──────────────────┬──────────────────┤
│   Corpo          │   Cérebro        │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Gestão de      │ • Carregar       │
│   estado         │   histórico      │
│ • Detecção de    │ • Invocar IA     │
│   activação      │ • Executar       │
│ • Ciclo de vida  │   ferramentas    │
│                  │ • Persistir      │
│                  │   respostas      │
└──────────────────┴──────────────────┘
```

## Ficheiro da Alma

### Estrutura

```markdown
# Being Name

## Personality
Describe the being's personality traits and characteristics.

## Capabilities
List what this being can do.

## Behavior Guidelines
Define how the being should behave in different situations.

## Knowledge Domain
Specify the being's area of expertise.
```

### Exemplo

```markdown
# Code Review Assistant

## Personality
You are a meticulous code reviewer with 10 years of experience.
You provide constructive feedback and always explain your reasoning.

## Capabilities
- Review code for bugs and best practices
- Suggest performance optimizations
- Explain complex algorithms
- Identify security vulnerabilities

## Behavior Guidelines
- Start with positive observations
- Provide specific examples
- Explain why changes are needed
- Be respectful and professional

## Knowledge Domain
Specialized in C#, .NET, and software architecture.
```

## Criar um Being

### Através da Web UI

1. Navegue para **Gestão de Beings**
2. Clique em **Criar Novo Being**
3. Preencha:
   - Nome
   - Conteúdo da Alma
   - Opções de configuração
4. Clique em **Criar**

### Através da API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## Ciclo de Vida do Being

### Estados de Actividade

Os Silicon Beings têm os seguintes estados de actividade:

| Estado | Descrição |
|------|------|
| `Idle` | Estado inactivo, aguardando activação do relógio |
| `SingleChat` | Em chat um-a-um |
| `GroupChat` | Em chat de grupo |
| `Task` | A executar tarefa |
| `Timer` | A executar temporizador |
| `Stopped` | Parado, devido a erros consecutivos ou paragem manual |

**Mecanismo do estado Stopped**:
- Quando um Silicon Being sofre 10 erros consecutivos, entra automaticamente no estado `Stopped`
- Após entrar no estado Stopped, o being não executa mais nenhuma tarefa
- Quando uma nova mensagem de chat chega, o contador de erros é reiniciado e o being retoma a execução
- Também pode ser reiniciado através de intervenção manual

### Transições de Estado

```
Idle → SingleChat → Idle (chat concluído)
Idle → GroupChat → Idle (chat de grupo concluído)
Idle → Task → Idle (tarefa concluída)
Idle → Timer → Idle (temporizador concluído)
Qualquer → Stopped (10 erros consecutivos)
Stopped → Idle (nova mensagem de chat ou reinício manual)
```

### Operações

- **Iniciar**: Inicializar e começar a processar
- **Parar**: Encerramento elegante
- **Reiniciar**: Recuperar do estado Stopped para o estado Idle

## Sistema de Tarefas

### Criar Tarefa

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Review the code",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### Estados da Tarefa

- `Pending` - Aguardando execução
- `Running` - Em execução
- `SubmittedForReview` - Submetida para revisão
- `UnderReview` - Em revisão
- `Rework` - Em retrabalho
- `Completed` - Concluída com sucesso
- `Failed` - Falha na execução
- `Cancelled` - Cancelada manualmente

## Sistema de Temporizadores

### Tipos de Temporizador

1. **Uma vez**: Executa uma vez após um atraso
2. **Intervalo**: Repete a intervalos fixos
3. **Cron**: Executa com base em expressões cron

### Exemplo

```csharp
// Executar a cada hora
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## Sistema de Memória

### Tipos de Memória

- **Curto prazo**: Contexto da conversa actual
- **Longo prazo**: Conhecimento e experiência persistidos
- **Episódica**: Eventos e interacções indexados por tempo

### Estrutura de Armazenamento

Versão Default:
```
data/
└── beings/
    └── {being-id}/
        ├── soul.md
        ├── memory/
        │   ├── short-term.json
        │   └── long-term/
        │       ├── 2026-04-20.json
        │       └── 2026-04-21.json
        └── tasks/
            └── task-history.json
```

Versão Fast (armazenamento SpeedyPack):
```
data/
├── speedy/
│   ├── {being-id}.spk       # Ficheiro de armazenamento SpeedyPack
│   └── {being-id}.spk.idx   # Ficheiro de índice
└── beings/
    └── {being-id}/
        └── soul.md
```

## Sistema de Notas de Trabalho

### Visão Geral

As notas de trabalho são o sistema de diário pessoal dos Silicon Beings, com design em formato de página, usado para registar progresso de trabalho, notas de aprendizagem, notas de projecto, etc.

### Características

- **Gestão por páginas**: Cada nota é uma página independente, acedida por número de página
- **Suporte Markdown**: O conteúdo suporta formato Markdown (texto, listas, tabelas, blocos de código)
- **Índice por palavras-chave**: Suporta a adição de palavras-chave às notas, facilitando a pesquisa
- **Funcção de resumo**: Cada nota tem um breve resumo para navegação rápida
- **Geração de directório**: Pode gerar uma visão geral do directório de todas as notas, ajudando a compreender o contexto geral
- **Timestamps**: Regista automaticamente os tempos de criação e actualização
- **Privado por defeito**: Apenas o próprio being pode aceder (o Curator pode gerir)

### Cenários de Utilização

1. **Registo de progresso do projecto**
   ```
   Resumo: Módulo de autenticação de utilizadores concluído
   Conteúdo: Implementada verificação JWT token, integração OAuth2, mecanismo de refresh token
   Palavras-chave: autenticação,JWT,OAuth2
   ```

2. **Notas de aprendizagem**
   ```
   Resumo: Aprender melhores práticas de programação assíncrona em C#
   Conteúdo: Precauções no uso de async/await, cenários de uso de ConfigureAwait...
   Palavras-chave: C#,assíncrono,melhores práticas
   ```

3. **Actas de reunião**
   ```
   Resumo: Discussão de requisitos do produto
   Conteúdo: Discutidos requisitos de novas funcionalidades, definida solução de implementação...
   Palavras-chave: produto,requisitos,reunião
   ```

### Uso Através de Ferramentas

Os beings podem gerir notas de trabalho através da ferramenta `work_note`:

```json
// Criar nota
{
  "action": "create",
  "summary": "Módulo de autenticação de utilizadores concluído",
  "content": "## Detalhes de Implementação\n\n- Usar JWT token\n- Suportar OAuth2",
  "keywords": "autenticação,JWT,OAuth2"
}

// Ler nota
{
  "action": "read",
  "page_number": 1
}

// Pesquisar notas
{
  "action": "search",
  "keyword": "autenticação",
  "max_results": 10
}
```

### Gestão Através da Web UI

1. Navegue para **Gestão de Beings** → Seleccione um being
2. Clique no separador **Notas de Trabalho**
3. Pode visualizar, pesquisar e editar notas
4. Suporta pré-visualização Markdown

## Sistema de Rede de Conhecimento

### Visão Geral

A rede de conhecimento é um sistema de representação e gestão de conhecimento baseado em estrutura de triplas (sujeito-predicado-objecto), usado para armazenar e gerir conhecimento estruturado.

### Conceitos Principais

#### Estrutura de Triplas

```
Sujeito (Subject) --Predicado (Predicate)--> Objecto (Object)
```

**Exemplos**:
- `Python` --`is_a`--> `programming_language`
- `Pequim` --`capital_of`--> `China`
- `Água` --`boiling_point`--> `100°C`

#### Confiança

Cada tripla de conhecimento tem uma pontuação de confiança (0.0-1.0), representando o nível de credibilidade do conhecimento:
- `1.0`: Absolutamente certo (por exemplo, teoremas matemáticos)
- `0.8-0.99`: Altamente confiável (por exemplo, factos verificados)
- `0.5-0.79`: Confiabilidade média (por exemplo, inferências ou hipóteses)
- `<0.5`: Baixa confiabilidade (por exemplo, especulações ou informações não verificadas)

#### Sistema de Tags

Suporta a adição de tags às triplas, facilitando a categorização e pesquisa:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### Operações de Conhecimento

#### 1. Adicionar Conhecimento

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programming", "language"]
}
```

#### 2. Consultar Conhecimento

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. Pesquisar Conhecimento

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. Descobrir Caminhos de Conhecimento

Encontrar caminhos de associação entre dois conceitos:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Retorna:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Validação de Conhecimento

Verificar a validade e consistência do conhecimento:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Estatísticas de Conhecimento

Obter informações estatísticas gerais da rede de conhecimento:
```json
{
  "action": "stats"
}
```

Retorna:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Cenários de Utilização

1. **Armazenamento de factos**
   - Armazenar factos objectivos e conhecimento comum
   - Exemplo: `Terra` --`is_a`--> `planeta`

2. **Relações entre conceitos**
   - Registar relações entre conceitos
   - Exemplo: `Herança` --`is_a`--> `conceito_de_programação_orientada_a_objectos`

3. **Acumulação de aprendizagem**
   - Os beings acumulam conhecimento continuamente através da aprendizagem
   - Formando um sistema de conhecimento estruturado

4. **Suporte ao raciocínio**
   - Descobrir relações indirectas através de caminhos de conhecimento
   - Suportar raciocínio e tomada de decisão baseados em conhecimento

### Gestão Através da Web UI

1. Navegue para a página **Rede de Conhecimento**
2. Visualize informações estatísticas do conhecimento
3. Pesquise e navegue no conhecimento
4. Visualização do grafo de relações de conhecimento (planeado)

## Operações do Navegador WebView (Novo)

### Visão Geral

Os Silicon Beings podem navegar autonomamente em páginas web, obter informações e executar operações web através da ferramenta de navegador WebView. O navegador funciona em modo headless, completamente invisível para o utilizador.

### Características

- **Isolamento individual**: Cada being possui uma instância de navegador independente, cookies e sessão
- **Modo headless**: Operação autónoma em segundo plano, invisível para o utilizador
- **Funcionalidade completa**: Suporta execução de JavaScript, renderização CSS, preenchimento de formulários, etc.
- **Controlo de segurança**: Todas as operações passam pela cadeia de verificação de permissões

### Operações Comuns

#### 1. Abrir o Navegador

```json
{
  "action": "open"
}
```

#### 2. Navegar para uma Página Web

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Obter Conteúdo da Página

```json
{
  "action": "get_page_text"
}
```

Retorna o conteúdo textual da página, para análise e compreensão pela IA.

#### 4. Clicar num Elemento

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. Introduzir Texto

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "palavra-chave de pesquisa"
}
```

#### 6. Executar JavaScript

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. Obter Captura de Ecrã

```json
{
  "action": "get_screenshot"
}
```

Retorna uma captura de ecrã da página (codificada em Base64), que pode ser usada para análise visual.

#### 8. Aguardar Aparição de Elemento

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Cenários de Utilização

1. **Obtenção de informação**
   - Navegar em sites de notícias para obter as últimas informações
   - Consultar documentação e materiais técnicos
   - Monitorizar alterações no conteúdo de páginas web

2. **Operações automatizadas**
   - Preencher e submeter formulários
   - Clicar em botões para accionar operações
   - Extrair dados de páginas web

3. **Análise de páginas web**
   - Analisar a estrutura e conteúdo da página
   - Extrair informações específicas
   - Análise visual através de capturas de ecrã

### Precauções

- As operações do navegador podem ser lentas, é necessário aguardar o carregamento completo da página
- Usar `wait_for_element` para garantir que o elemento apareceu antes de operar
- Respeitar os termos de uso e o robots.txt dos sites
- Evitar pedidos frequentes que possam resultar em bloqueio

## Melhores Práticas

### Escrita do Ficheiro da Alma

1. **Específico**: Traços de personalidade e limites claros
2. **Definir âmbito**: O que o being deve e não deve fazer
3. **Incluir exemplos**: Demonstrar os padrões de comportamento esperados
4. **Actualizar regularmente**: Evoluir a alma com base no desempenho

### Gestão de Tarefas

1. **Definir prioridades**: Usar prioridades (1-10)
2. **Definir prazos**: Sempre definir datas limite
3. **Monitorizar progresso**: Verificar regularmente o estado das tarefas
4. **Tratar falhas**: Implementar lógica de retry

### Optimização de Memória

1. **Limpar dados antigos**: Arquivar regularmente memórias antigas
2. **Indexar informações importantes**: Marcar informações-chave
3. **Usar armazenamento temporal**: Utilizar consultas por índice temporal

### Mecanismo de Desvanecimento da Memória

O sistema inclui o `MemoryFadeService`, um serviço de decaimento temporizado que simula a característica de esquecimento da memória biológica:

- **Decaimento automático**: A cada hora, aplica um algoritmo de decaimento de importância às entradas de memória de todos os Silicon Beings
- **Arquivamento automático**: Memórias com importância abaixo do limiar são automaticamente arquivadas, deixando de participar na pesquisa diária
- **Rastreamento estatístico**: Regista o número de ciclos de decaimento e o número de entradas com estado alterado

Isto significa que a memória dos Silicon Beings desvanece naturalmente ao longo do tempo, e informações importantes precisam de ser marcadas activamente como de alta importância através da ferramenta de memória, para evitar o arquivamento automático.

---

## Espaço de Trabalho de Projecto

### Visão Geral

O espaço de trabalho de projecto é um mecanismo de gestão de espaço que suporta a colaboração de múltiplos Silicon Beings. O Silicon Curator pode criar espaços de projecto, atribuir Silicon Beings aos projectos e atribuir-lhes funções.

### Ciclo de Vida do Projecto

```
Criação → Activo → Arquivado → Destruição
              ↑       |
              └─ Restaurar ┘
```

### Funções do Projecto

Os Silicon Beings podem ser atribuídos a funções específicas no projecto:

```json
{
  "action": "assign_role",
  "project_id": "project-uuid",
  "being_id": "being-uuid",
  "role_name": "developer"
}
```

### Notas de Trabalho do Projecto

As notas de trabalho dentro do espaço do projecto são públicas, e todos os membros do projecto podem aceder:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Módulo de autenticação de utilizadores concluído",
  "content": "## Detalhes de Implementação\n\n- Usar JWT token",
  "keywords": "autenticação,JWT"
}
```

### Tarefas do Projecto

As tarefas dentro do espaço do projecto suportam gestão completa do ciclo de vida:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "title": "Implementar autenticação de utilizadores",
  "priority": 5
}
```

### Fluxos de Trabalho do Projecto

Os projectos podem ser vinculados a modelos de fluxos de trabalho, impulsionando os fluxos de colaboração dos Silicon Beings:

- Os fluxos de trabalho são baseados em modelos de máquina de estados
- Suportam transições de estado orientadas por Tick
- Registo automático do histórico de transições de estado

### Isolamento de Permissões de Ferramentas

As permissões de ferramentas ao nível do projecto são independentes das permissões ao nível do Silicon Being, realizando o isolamento de permissões entre projectos. Por exemplo, um Silicon Being pode ter permissões de acesso à rede no Projecto A, mas pode ser restrito a permissões só de leitura no Projecto B.

## Resolução de Problemas

### O Being Não Inicia

**Verificar**:
- O Ficheiro da Alma existe e é válido
- O cliente de IA está configurado
- Há recursos de sistema suficientes

### O Being Para Inesperadamente

**Verificar**:
- Erros nos registos
- Disponibilidade do serviço de IA
- Utilização de memória

### A Tarefa Não É Executada

**Verificar**:
- O sistema de temporizadores está em execução
- Prioridade e agendamento da tarefa
- Configuração de permissões

## Próximos Passos

- 📚 Leia o [guia de arquitectura](architecture.md)
- 🛠️ Consulte o [guia de desenvolvimento](development-guide.md)
- 🚀 Consulte o [guia de início rápido](getting-started.md)
