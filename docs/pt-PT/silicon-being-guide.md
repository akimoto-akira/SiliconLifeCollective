# Guia dos Silicon Beings

> **Versão: v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [Français](../fr-FR/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md) | [Italiano](../it-IT/silicon-being-guide.md) | [Polski](../pl-PL/silicon-being-guide.md) | **Português**

## Visão geral

Os Silicon Beings são agentes orientados pela IA capazes de pensar, agir e evoluir autonomamente.

## Arquitetura

### Separação Corpo-Cérebro

```
┌─────────────────────────────────────┐
│         Silicon Being               │
├──────────────────┬──────────────────┤
│   Corpo          │   Cérebro        │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Gestão estado  │ • Carregar histórico│
│ • Deteção de     │ • Chamar a IA    │
│   ativações      │ • Executar ferramentas│
│ • Ciclo de vida  │ • Persistir resposta│
└──────────────────┴──────────────────┘
```

## Ficheiro da alma

### Estrutura

```markdown
# Nome do Being

## Personalidade
Descreve os traços de personalidade e características do being.

## Capacidades
Lista o que este being pode fazer.

## Diretrizes de comportamento
Define como o being deve comportar-se em diferentes situações.

## Domínio de conhecimento
Especifica o domínio de especialização do being.
```

### Exemplo

```markdown
# Assistente de revisão de código

## Personalidade
És um revisor de código meticuloso com 10 anos de experiência.
Forneces feedback construtivo e explicas sempre o teu raciocínio.

## Capacidades
- Rever código para bugs e boas práticas
- Sugerir otimizações de desempenho
- Explicar algoritmos complexos
- Identificar vulnerabilidades de segurança

## Diretrizes de comportamento
- Começar com observações positivas
- Fornecer exemplos específicos
- Explicar porque as alterações são necessárias
- Ser respeitoso e profissional

## Domínio de conhecimento
Especializado em C#, .NET e arquitetura de software.
```

## Criar um Being

### Através da interface Web

1. Navegar para **Gestão de Beings**
2. Clicar em **Criar novo Being**
3. Preencher:
   - Nome
   - Conteúdo da alma
   - Opções de configuração
4. Clicar em **Criar**

### Através da API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistente",
    "soul": "# Personalidade\nÉs útil..."
  }'
```

## Ciclo de vida de um Being

### Estados de atividade

Os Silicon Beings têm os seguintes estados de atividade:

| Estado | Descrição |
|--------|-------------|
| `Idle` | Estado inativo, à espera do trigger do relógio |
| `Working` | Em execução de um ciclo de pedido IA + chamada de ferramenta |
| `Error` | Ocorreu um erro durante a execução |
| `Stopped` | Parado, devido a erros consecutivos ou paragem manual |

**Mecanismo do estado Stopped**:
- Quando um Silicon Being sofre 10 erros consecutivos, entra automaticamente no estado `Stopped`
- Uma vez no estado Stopped, o Being não executará mais nenhuma atividade
- É necessária intervenção manual para reiniciar

### Transições de estado

```
Idle → Working → Idle (terminação normal)
Working → Error → Working (recuperação de erro)
Working → Stopped (10 erros consecutivos ou paragem manual)
Stopped → Idle (reinício)
```

### Operações

- **Iniciar**: Inicializar e começar o processamento
- **Parar**: Encerramento gradual
- **Reiniciar**: Retorno ao estado Idle a partir do estado Stopped

## Sistema de tarefas

### Criar uma tarefa

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Rever o código",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### Estados das tarefas

- `Pending` - À espera de execução
- `Running` - Em curso de execução
- `Completed` - Concluída com sucesso
- `Failed` - Execução falhada
- `Cancelled` - Cancelada manualmente

## Sistema de temporizadores

### Tipos de temporizadores

1. **Pontual**: Execução única após um atraso
2. **Intervalo**: Repetição a intervalos fixos
3. **Cron**: Execução baseada em expressão Cron

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

## Sistema de memória

### Tipos de memória

- **Curto prazo**: Contexto de conversação atual
- **Longo prazo**: Conhecimentos e experiências persistidos
- **Episódica**: Eventos e interações indexados no tempo

### Estrutura de armazenamento

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

## Sistema de notas de trabalho

### Visão geral

As notas de trabalho são um sistema de diário pessoal dos Silicon Beings com um design paginado para registar a progressão do trabalho, experiências de aprendizagem, notas de projeto, etc.

### Funcionalidades

- **Gestão de páginas**: Cada nota é uma página distinta, acessível por número de página
- **Suporte Markdown**: O conteúdo suporta formato Markdown (texto, listas, tabelas, blocos de código)
- **Índice por palavras-chave**: As notas podem ser etiquetadas com palavras-chave para pesquisa
- **Resumo**: Cada nota tem um breve resumo para navegação rápida
- **Geração de índice**: Pode gerar um índice de todas as notas para uma visão geral
- **Timestamps**: Registo automático das datas de criação e atualização
- **Privado por defeito**: Apenas o Being tem acesso (o Curator pode gerir)

### Cenários de utilização

1. **Documentar a progressão do projeto**
   ```
   Resumo: Módulo de autenticação de utilizador concluído
   Conteúdo: Verificação JWT, integração OAuth2, mecanismo de refresh token implementados
   Palavras-chave: autenticação,JWT,OAuth2
   ```

2. **Notas de aprendizagem**
   ```
   Resumo: Boas práticas de programação assíncrona C# aprendidas
   Conteúdo: Notas sobre async/await, casos de uso de ConfigureAwait...
   Palavras-chave: C#,Async,Boas práticas
   ```

3. **Atas de reunião**
   ```
   Resumo: Reunião de requisitos do produto
   Conteúdo: Novos requisitos de funcionalidades discutidos, abordagem de implementação definida...
   Palavras-chave: produto,requisitos,reunião
   ```

### Utilização através de ferramenta

Os Beings podem gerir as suas notas de trabalho através da ferramenta `work_note`:

```json
// Criar uma nota
{
  "action": "create",
  "summary": "Módulo de autenticação de utilizador concluído",
  "content": "## Detalhes da implementação\n\n- Utilização de JWT token\n- Suporte OAuth2",
  "keywords": "autenticação,JWT,OAuth2"
}

// Ler uma nota
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

### Gestão através da interface Web

1. Navegar para **Gestão de Beings** → Selecionar um Being
2. Clicar no separador **Notas de trabalho**
3. Visualizar, pesquisar, editar notas
4. Pré-visualização Markdown suportada

---

## Sistema de rede de conhecimentos

### Visão geral

A rede de conhecimentos é um sistema de representação e gestão de conhecimentos baseado numa estrutura de triplas (Sujeito-Predicado-Objeto) para o armazenamento e gestão de conhecimentos estruturados.

### Conceitos-chave

#### Estrutura de triplas

```
Sujeito (Subject) --Predicado (Predicate)--> Objeto (Object)
```

**Exemplos**:
- `Python` --`is_a`--> `programming_language`
- `Paris` --`capital_of`--> `França`
- `água` --`boiling_point`--> `100°C`

#### Valor de confiança

Cada tripla de conhecimento tem um valor de confiança (0.0-1.0) que indica a fiabilidade do conhecimento:
- `1.0`: Absolutamente certo (como teoremas matemáticos)
- `0.8-0.99`: Alta confiança (como factos verificados)
- `0.5-0.79`: Confiança média (como inferências ou hipóteses)
- `<0.5`: Baixa confiança (como conjeturas ou informações não verificadas)

#### Sistema de etiquetas

Suporta a adição de etiquetas às triplas para classificação e pesquisa:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### Operações de conhecimento

#### 1. Adicionar conhecimento

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

#### 2. Consultar conhecimento

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. Pesquisar conhecimentos

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. Descobrir caminhos de conhecimento

Encontra os caminhos de ligação entre dois conceitos:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Resultado:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Validar conhecimento

Verifica a validade e coerência do conhecimento:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Estatísticas da rede de conhecimentos

Obtém as estatísticas globais da rede de conhecimentos:
```json
{
  "action": "stats"
}
```

Resultado:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Cenários de utilização

1. **Armazenamento de factos**
   - Armazenar factos objetivos e conhecimentos gerais
   - Exemplo: `Terra` --`is_a`--> `planeta`

2. **Relações entre conceitos**
   - Registar as relações entre conceitos
   - Exemplo: `herança` --`is_a`--> `conceito_programação_orientada_objetos`

3. **Acumulação de aprendizagem**
   - Os Beings acumulam continuamente conhecimentos através da aprendizagem
   - Formam sistemas de conhecimentos estruturados

4. **Suporte à inferência**
   - Descobrir relações indiretas através dos caminhos de conhecimento
   - Suportar a inferência e a tomada de decisão baseada em conhecimento

### Gestão através da interface Web

1. Navegar para a página **Rede de conhecimentos**
2. Visualizar as estatísticas de conhecimento
3. Pesquisar e navegar pelos conhecimentos
4. Visualizar o diagrama de relações de conhecimento (planeado)

---

## Operações do browser WebView (Novo)

### Visão geral

Os Silicon Beings podem navegar autonomamente na Web, recuperar informações e executar operações Web através da ferramenta de browser WebView. O browser funciona em modo headless, totalmente invisível para o utilizador.

### Funcionalidades

- **Isolamento individual**: Cada Being tem a sua própria instância do browser, cookies e sessões
- **Modo headless**: Operação autónoma em segundo plano, invisível para o utilizador
- **Funcionalidade completa**: Suporta execução de JavaScript, renderização CSS, preenchimento de formulários, etc.
- **Controlo de segurança**: Todas as operações devem passar pela cadeia de permissões

### Operações comuns

#### 1. Abrir o browser

```json
{
  "action": "open_browser"
}
```

#### 2. Navegar para um site Web

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Recuperar conteúdo da página

```json
{
  "action": "get_page_text"
}
```

Retorna o conteúdo textual da página para análise e compreensão pela IA.

#### 4. Clicar num elemento

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. Inserir texto

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

#### 7. Obter captura de ecrã

```json
{
  "action": "get_screenshot"
}
```

Retorna uma captura de ecrã da página (codificada em Base64), utilizável para análise visual.

#### 8. Aguardar elemento

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Cenários de utilização

1. **Recolha de informações**
   - Navegar em sites de notícias para informações atualizadas
   - Consultar documentação e recursos técnicos
   - Monitorizar alterações de conteúdo de sites Web

2. **Operações automatizadas**
   - Preencher e submeter formulários
   - Clicar em botões para operações
   - Extrair dados Web

3. **Análise Web**
   - Analisar a estrutura e conteúdo das páginas
   - Extrair informações específicas
   - Análise visual através de capturas de ecrã

### Notas

- As operações do browser podem ser lentas, aguardando o carregamento das páginas
- Utilizar `wait_for_element` para garantir que o elemento está presente
- Respeitar os termos de utilização e o ficheiro robots.txt dos sites Web
- Evitar pedidos frequentes para prevenir banimentos

---

## Boas práticas

### Escrever um ficheiro da alma

1. **Concreto**: Traços de personalidade e limites claros
2. **Definir o perímetro**: O que o Being deve e não deve fazer
3. **Incluir exemplos**: Mostrar padrões de comportamento esperados
4. **Atualizar regularmente**: Fazer a alma evoluir com base no desempenho

### Gestão de tarefas

1. **Definir prioridades**: Utilizar prioridades (1-10)
2. **Definir prazos**: Definir sempre uma data limite
3. **Monitorizar a progressão**: Verificar regularmente o estado das tarefas
4. **Gerir erros**: Implementar lógica de retry

### Otimização da memória

1. **Limpar dados antigos**: Arquivar regularmente memórias antigas
2. **Indexar informações importantes**: Marcar informações-chave
3. **Utilizar armazenamento temporal**: Aproveitar consultas por índice temporal

## Resolução de problemas

### O Being não inicia

**Verificar**:
- O ficheiro da alma existe e é válido
- O cliente IA está configurado
- Os recursos do sistema são suficientes

### O Being para inesperadamente

**Verificar**:
- Os erros nos logs
- A disponibilidade do serviço IA
- A utilização da memória

### As tarefas não são executadas

**Verificar**:
- O sistema de temporizadores funciona
- A prioridade e o agendamento das tarefas
- As definições de permissões

## Próximos passos

- 📚 Ler o [guia de arquitetura](architecture.md)
- 🛠️ Consultar o [guia de desenvolvimento](development-guide.md)
- 🚀 Ver o [guia de introdução](getting-started.md)
