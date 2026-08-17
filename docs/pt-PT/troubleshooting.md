# Guia de Resolução de Problemas

> **Versão: v0.2.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md) | [Русский](../ru-RU/troubleshooting.md)

## Perguntas Frequentes

### Compilação e Build

#### Problema: Build falha, dependências em falta

**Sintomas**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Solução**:
```bash
dotnet restore
dotnet build
```

#### Problema: .NET SDK não encontrado

**Sintomas**:
```
The .NET SDK could not be found
```

**Solução**:
1. Instalar .NET 9 SDK: https://dotnet.microsoft.com/download/dotnet/9.0
2. Verificar a instalação:
```bash
dotnet --version
```

---

### Problemas de Ligação à IA

#### Problema: Ligação Ollama recusada

**Sintomas**:
```
Failed to connect to Ollama at http://localhost:11434
```

**Solução**:
```bash
# Verificar se o Ollama está em execução
ollama list

# Iniciar o Ollama
ollama serve

# Testar a ligação
curl http://localhost:11434/api/tags
```

#### Problema: Modelo não encontrado

**Sintomas**:
```
model "qwen2.5:7b" not found
```

**Solução**:
```bash
# Obter o modelo necessário
ollama pull qwen2.5:7b

# Listar modelos disponíveis
ollama list
```

#### Problema: Erro 404 do DashScope

**Sintomas**:
```
HTTP 404: Model not found
```

**Solução**:
1. Verificar se a chave API está correcta
2. Confirmar se o nome do modelo corresponde ao directório DashScope
3. Verificar se o endpoint regional está correcto
4. Confirmar se a conta tem acesso ao modelo

#### Problema: Falha na ligação ao Volcengine Ark

**Sintomas**:
```
HTTP 401: Unauthorized
ou
HTTP 404: Endpoint not found
```

**Solução**:
1. Verificar se a chave API está correcta
2. Confirmar se o formato do URL do Endpoint está correcto (predefinição: `https://ark.cn-beijing.volces.com/api/v3/chat/completions`)
3. Confirmar que o parâmetro Model usa o ID do endpoint de inferência (por exemplo `ep-20241212123456-abcde`), e não o nome do modelo
4. Confirmar se a conta tem acesso ao endpoint

---

### Problemas em Tempo de Execução

#### Problema: Porta já em uso

**Sintomas**:
```
HttpListenerException: Address already in use
```

**Solução**:

**Windows**:
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac**:
```bash
lsof -ti:8080 | xargs kill -9
```

**Ou alterar a porta na configuração.**

#### Problema: O being não inicia

**Sintomas**:
- O estado do being mostra "Error"
- Os registos mostram falha na inicialização

**Solução**:
1. Verificar se o Ficheiro da Alma existe e é válido
2. Confirmar se o cliente de IA está configurado
3. Verificar os registos para obter o erro específico:
```bash
tail -f logs/*.log
```

#### Problema: Memória insuficiente

**Sintomas**:
```
OutOfMemoryException
```

**Solução**:
1. **SiliconLife.Default**: Aumentar o tamanho do heap:
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast**: A versão Fast tem um uso de memória base mais elevado (~500MB), se a memória continuar insuficiente, recomenda-se:
   - Reduzir o número de Silicon Beings concorrentes
   - Limpar dados antigos para libertar memória

3. Limpar dados antigos:
```bash
# Arquivar registos antigos
mv logs/ logs-archive/
mkdir logs

# Limpar memórias antigas
# Através da Web UI: Gestão de Memória > Limpar
```

> **Dica**: SiliconLife.Default tem um uso de memória mais baixo (~200MB), adequado para ambientes com memória limitada; SiliconLife.Fast tem um uso de memória mais elevado mas melhor desempenho, adequado para ambientes de produção.

---

### Problemas de Permissões

#### Problema: Permissão negada

**Sintomas**:
```
Permission denied: FileAccess C:\Windows
```

**Solução**:
1. Verificar as permissões actuais:
```bash
curl http://localhost:8080/api/permissions/list
```

2. Conceder permissão:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

3. Ou usar a Web UI: Gestão de Permissões

#### Problema: Permissão não expira

**Sintomas**:
- A permissão permanece válida após o tempo de expiração

**Solução**:
1. Verificar a sincronização do relógio do sistema
2. Confirmar se o campo `expiresAt` está definido correctamente
3. Limpar o cache de permissões

---

### Problemas da Web UI

#### Problema: Não é possível aceder à Web UI

**Sintomas**:
- O navegador mostra "Connection refused"

**Solução**:
1. Verificar se o servidor está em execução
2. Confirmar o URL correcto: `http://localhost:8080`
3. Verificar as definições da firewall
4. Verificar os registos para erros de inicialização

#### Problema: SSE não funciona

**Sintomas**:
- As actualizações em tempo real não aparecem
- O chat não faz streaming

**Solução**:
1. Verificar se o navegador suporta SSE
2. Desactivar o buffer de proxy para SSE
3. Verificar a estabilidade da rede
4. Experimentar um navegador diferente

#### Problema: A UI parece corrompida

**Sintomas**:
- Estilos incorrectos
- Layout quebrado

**Solução**:
1. Limpar o cache do navegador
2. Experimentar uma skin diferente: Definições > Skin
3. Verificar erros na consola do navegador
4. Desactivar extensões do navegador

---

### Problemas de Armazenamento

#### Problema: Não é possível ler/escrever dados

**Sintomas**:
```
IOException: Access denied
```

**Solução**:
1. Verificar permissões de ficheiros
2. Confirmar se o caminho de armazenamento existe
3. Verificar o espaço em disco
4. Executar com permissões adequadas

#### Problema: Dados corrompidos

**Sintomas**:
- Erros de análise JSON
- Dados em falta

**Solução**:
1. Restaurar a partir de backup
2. Verificar a integridade do armazenamento:
```bash
# Através da Web UI: Sistema > Verificação de Armazenamento
```

3. Reparar manualmente ficheiros corrompidos

#### Problema: Ficheiro de armazenamento SpeedyPack corrompido (versão Fast)

**Sintomas**:
- O ficheiro `.spk` não consegue carregar
- A inicialização do SpeedyStorage falha

**Solução**:
1. Usar a ferramenta `SiliconLife.Speedy.Manager` para verificar e reparar ficheiros `.spk`
2. Verificar se o ficheiro de índice `.spk.idx` corresponde ao ficheiro `.spk`
3. Se o ficheiro de índice estiver corrompido, eliminar o ficheiro `.spk.idx`, o sistema reconstruirá o índice automaticamente
4. Restaurar o ficheiro `.spk` a partir de backup

#### Problema: Falha na compactação automática do SpeedyPack (versão Fast)

**Sintomas**:
- O ficheiro `.spk` continua a crescer
- Espaço em disco insuficiente

**Solução**:
1. Verificar se o `SpeedyPackAutoCompactor` está a funcionar correctamente
2. Accionar manualmente a operação de compactação
3. Verificar a configuração do limiar de compactação
4. Usar a ferramenta `SiliconLife.Speedy.Manager` para compactação manual

---

### Problemas de Execução de Ferramentas

#### Problema: Ferramenta não encontrada

**Sintomas**:
```
Tool "xyz" not found
```

**Solução**:
1. Verificar se o nome da ferramenta está correcto
2. Confirmar se a ferramenta está no directório Tools
3. Reconstruir o projecto
4. Verificar se a ferramenta está correctamente implementada

#### Problema: A ferramenta retorna erro

**Sintomas**:
```
Tool execution failed: ...
```

**Solução**:
1. Verificar os registos da ferramenta
2. Validar os parâmetros de entrada
3. Testar a ferramenta independentemente
4. Verificar as permissões

---

### Problemas de Plugins

#### Problema: Falha no carregamento do plugin

**Sintomas**:
```
Plugin load failed: Security check failed
```

**Solução**:
1. Verificar se o plugin referencia namespaces proibidos não declaráveis (por exemplo, `System.Runtime.InteropServices`, `System.Reflection.Emit`, `Microsoft.CodeAnalysis`)
2. Se o plugin necessita de `System.IO` ou `System.Net.Http`, confirmar que declarou as capacidades `FileIO` ou `Network` através de `[PluginCapability]`
3. Confirmar que o plugin apenas referencia assemblies na lista branca de assemblies fiáveis
4. Verificar se o plugin implementa correctamente a interface `IPlugin`
5. Consultar os registos para obter detalhes sobre a falha na verificação de segurança

#### Problema: Ferramentas do plugin não registadas

**Sintomas**:
- O plugin carrega com sucesso mas as ferramentas não aparecem na lista de ferramentas

**Solução**:
1. Confirmar que as classes de ferramentas no plugin implementam correctamente a interface `ITool`
2. Verificar se as classes de ferramentas são public
3. Confirmar se `ToolManager.ScanAllPluginAssemblies()` foi chamado
4. Reconstruir o plugin e reiniciar a aplicação

---

### Problemas de Competências

#### Problema: A competência não aparece na lista de competências ou não é visível para a IA

**Sintomas**:
- A página de competências da Web UI guarda com sucesso, mas a lista não exibe / a IA não invoca a competência

**Solução**:
1. Verificar se `id` e `description` da competência não estão vazios (rascunhos não são expostos à IA)
2. Competências com metadados incompletos (`NeedsCompletion`) não são injectadas na IA — preencha os metadados YAML frontmatter ou deixe a IA preencher antes de guardar
3. Verificar se a matriz de permissões desactiva `{skillId}:execute` (competências desactivadas são invisíveis para a IA)
4. Confirmar que o interruptor global `SkillEnabled` está definido como true
5. O hot reload demora até 30 segundos a surtir efeito; aguarde e actualize ou reinicie

#### Problema: A execução da competência falha com "not in whitelist"

**Sintomas**:
```
Tool 'xxx' is not available in skill 'yyy' (not in whitelist)
```

**Solução**:
- Adicionar a ferramenta à `tool_whitelist` da competência, ou limpar a lista de permissões para herdar todas as ferramentas do being

#### Problema: Limite de número de competências atingido

**Sintomas**:
```
Custom skill limit reached (50)
```

**Solução**:
1. Eliminar competências personalizadas que já não são necessárias
2. Ou aumentar a configuração `MaxCustomSkillsPerBeing`

---

### Problemas de MCP

#### Problema: Falha na ligação ao servidor MCP

**Sintomas**:
- O estado do servidor mostra `error` ou `disconnected`, `lastError` não está vazio

**Solução**:
1. Servidor stdio: confirmar que `command` é executável (ex.: `npx` no PATH), `arguments` correctos
2. Servidor http: verificar se o URL `endpoint` é alcançável (firewall, proxy)
3. Clicar em **Reconectar** na página /mcp
4. Consultar os detalhes de `lastError`; causas comuns incluem comando inexistente, versão incompatível, endpoint 404

#### Problema: Ferramentas MCP não injectadas no being

**Sintomas**:
- O servidor está ligado (`connected`) mas a IA não consegue invocar a ferramenta `mcp_xxx_yyy`

**Solução**:
1. Confirmar que `enabled` do servidor está como true
2. Confirmar que o interruptor global `McpEnabled` está como true
3. Verificar a matriz de permissões: `mcp_{serverId}_{toolName}:execute` está desactivado?
4. Na conversa do being, pode usar a ferramenta `mcp` (`list_tools`) para verificar os nomes das ferramentas efectivamente injectadas

#### Problema: Adicionar servidor retorna erro de formato de ID

**Sintomas**:
```
Server id must contain only lowercase letters, digits and underscores
```

**Solução**:
- O ID do servidor só permite letras minúsculas, dígitos e sublinhados (ex.: `filesystem`, `github_tools`)

---

### Problemas de Plataformas IM

#### Problema: Mensagens do Feishu não recebidas

**Solução**:
1. Verificar a configuração da subscrição de eventos da plataforma Feishu Open — endereço de callback e porta (`listenPort` + `callbackPath`)
2. Confirmar que a `Encrypt Key` / `Verification Token` corresponde à configuração
3. Para desenvolvimento local, pode usar o assistente de autorização OAuth (autorização com um clique na página de configuração); o callback de eventos requer acessibilidade pública ou túnel de intranet
4. Consultar erros de verificação de assinatura / desencriptação nos registos

#### Problema: Timeout da autorização OAuth

**Sintomas**:
- A página de autorização mostra o estado `timeout`

**Solução**:
1. A sessão de autorização é válida por 5 minutos; após o timeout, clicar novamente no botão de autorização
2. Confirmar que o endereço de callback `/im/feishu/callback` é acessível pelo Feishu (configuração `redirectBaseUrl` correcta)
3. A apresentação do estado no frontend depende do SSE; se o SSE se desligar, pode usar polling de `/im/{platform}/status` como fallback

#### Problema: Marcador `${ENV_VAR}` não resolvido

**Sintomas**:
- A ligação da plataforma IM falha; o valor de configuração ainda é texto do marcador

**Solução**:
1. Confirmar que a variável de ambiente foi definida antes de iniciar o processo (reiniciar a aplicação para entrar em vigor)
2. Verificar a ortografia do nome da variável (apenas `[A-Za-z_][A-Za-z0-9_]*` é suportado)
3. Nota: manter os marcadores no config.json é um comportamento de desenho; a resolução ocorre numa cópia em memória

#### Problema: Apenas uma de múltiplas plataformas IM recebe mensagens

**Solução**:
- As mensagens de saída são difundidas para todas as plataformas activas; falhas de envio numa única plataforma são isoladas silenciosamente — verifique se o token dessa plataforma expirou (reautorizar ou actualizar a chave)

---

### Problemas de Notas de Trabalho

#### Problema: Não é possível criar notas de trabalho

**Sintomas**:
```
Failed to create work note
```

**Solução**:
1. Verificar se o being existe e está em execução
2. Confirmar se o caminho de armazenamento tem permissões de escrita
3. Verificar se o conteúdo está vazio (o conteúdo é obrigatório)
4. Consultar os registos para obter detalhes do erro

#### Problema: Pesquisa de notas sem resultados

**Sintomas**:
- A pesquisa por palavra-chave retorna resultados vazios
- Mas tem a certeza de que existem notas relevantes

**Solução**:
1. Verificar se a ortografia da palavra-chave está correcta
2. Tentar usar uma palavra-chave mais genérica
3. Confirmar se as notas contêm a palavra-chave (sensível a maiúsculas/minúsculas)
4. Aumentar o valor do parâmetro `max_results`

#### Problema: Geração do directório de notas lenta

**Sintomas**:
- O tempo de resposta é longo ao gerar o directório
- O being tem um grande número de notas (>1000 páginas)

**Solução**:
1. Este é um comportamento normal, é necessário percorrer todas as notas
2. Considerar arquivar regularmente notas antigas
3. Usar a funcionalidade de pesquisa em vez de navegar pelo directório
4. Optimização planeada: adicionar mecanismo de cache do directório

---

### Problemas da Rede de Conhecimento

#### Problema: Consulta de conhecimento retorna resultados vazios

**Sintomas**:
```
No knowledge triples found
```

**Solução**:
1. Verificar a ortografia do sujeito e do predicado
2. Confirmar se o conhecimento foi adicionado à rede
3. Usar a funcionalidade de pesquisa para correspondência difusa:
```json
{
  "action": "search",
  "query": "palavra-chave"
}
```

#### Problema: Falha na descoberta de caminhos de conhecimento

**Sintomas**:
```
No path found between concepts
```

**Solução**:
1. Verificar se ambos os conceitos existem na rede de conhecimento
2. Confirmar se existe um caminho de associação (pode não haver relação directa ou indirecta)
3. Tentar adicionar mais conhecimento para estabelecer ligações
4. Reduzir o limite de comprimento do caminho (se definido)

#### Problema: Falha na validação do conhecimento

**Sintomas**:
```
Knowledge validation failed
```

**Solução**:
1. Verificar se o formato da tripla está correcto (sujeito, predicado, objecto são obrigatórios)
2. Confirmar se a confiança está no intervalo 0.0-1.0
3. Verificar se existem triplas duplicadas
4. Consultar os detalhes do erro de validação para compreender o problema específico

#### Problema: Estatísticas da rede de conhecimento imprecisas

**Sintomas**:
- Os números estatísticos não correspondem ao esperado
- As estatísticas não são actualizadas após adicionar conhecimento

**Solução**:
1. As estatísticas podem precisar de alguns segundos para serem actualizadas (cache)
2. Verificar se alguma operação de eliminação não foi executada com sucesso
3. Reiniciar a aplicação para forçar a actualização das estatísticas
4. Re-consultar as estatísticas via API

---

### Problemas de Gestão de Projectos

#### Problema: Não é possível criar projecto

**Sintomas**:
```
Failed to create project
```

**Solução**:
1. Verificar se o nome do projecto está vazio (obrigatório)
2. Confirmar se o nome do projecto não está duplicado
3. Verificar se o caminho de armazenamento tem permissões de escrita
4. Consultar os registos para obter detalhes do erro

#### Problema: Dados do projecto perdidos

**Sintomas**:
- As informações do projecto não conseguem carregar
- Ficheiros do projecto corrompidos

**Solução**:
1. Verificar se o directório de armazenamento do projecto existe
2. Restaurar dados do projecto a partir de backup
3. Confirmar se o formato do ficheiro JSON está correcto
4. Reparar manualmente ficheiros de projecto corrompidos

#### Problema: Falha na atribuição de funções do projecto

**Sintomas**:
```
Failed to assign role
```

**Solução**:
1. Confirmar se o Silicon Being se juntou ao projecto
2. Verificar se o nome da função é válido
3. Confirmar se o operador é o Silicon Curator
4. Consultar os registos para obter detalhes do erro

#### Problema: O fluxo de trabalho não inicia

**Sintomas**:
- A criação da instância do fluxo de trabalho falha
- As transições de estado não são executadas

**Solução**:
1. Verificar se o modelo de fluxo de trabalho está definido
2. Confirmar se o estado inicial está correctamente definido
3. Confirmar se o projecto está vinculado a um modelo de fluxo de trabalho
4. Verificar os registos do fluxo de trabalho para obter erros de transição

---

### Problemas de Permissões de Ferramentas

#### Problema: Operação de ferramenta negada

**Sintomas**:
```
Tool operation denied: network:post
```

**Solução**:
1. Verificar a configuração de permissões de ferramentas do Silicon Being:
```bash
curl http://localhost:8080/api/beings/tool-permissions?beingId=<id>
```

2. Actualizar as permissões de ferramentas:
```bash
curl -X PUT http://localhost:8080/api/beings/tool-permissions \
  -H "Content-Type: application/json" \
  -d '{
    "beingId": "being-uuid",
    "permissions": {
      "network:post": "allowed"
    }
  }'
```

3. Ou usar a Web UI: Beings → Permissões de Ferramentas

#### Problema: Permissões de ferramentas do projecto não produzem efeito

**Sintomas**:
- As permissões de ferramentas ao nível do projecto não funcionam como esperado

**Solução**:
1. Confirmar se as permissões ao nível do projecto estão correctamente configuradas
2. Verificar se há conflito entre as permissões ao nível do Silicon Being e ao nível do projecto
3. As permissões ao nível do projecto são independentes das permissões ao nível do Silicon Being, ambas são intersectadas
4. Consultar os registos de auditoria para confirmar os resultados da verificação de permissões

---

## Depuração

### Activar Registos Detalhados

Editar a configuração:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Verificar Registos

Os registos são armazenados em:
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

Visualização em tempo real:
```bash
tail -f logs/*.log
```

### Usar o Depurador

**SiliconLife.Default (Implementação Padrão)**:
```bash
# Executar com depurador
dotnet run --project src/SiliconLife.Default --configuration Debug

# Anexar depurador
# Através do IDE: Anexar ao Processo > SiliconLife.Default
```

**SiliconLife.Fast (Versão de Alto Desempenho)**:
```bash
# Executar com depurador
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Anexar depurador
# Através do IDE: Anexar ao Processo > SiliconLife.Fast
```

> **Recomendação**: Durante a fase de depuração de desenvolvimento, recomenda-se usar o SiliconLife.Default; após a verificação da arquitectura, usar o SiliconLife.Fast para implantação de produção.

---

## Problemas de Desempenho

### Tempo de Resposta Lento

**Optimizações**:
1. Reduzir a complexidade do modelo de IA
2. Activar cache
3. Limpar dados antigos
4. Aumentar os recursos do sistema

### Utilização de CPU Elevada

**Verificar**:
- Demasiados beings em execução
- Loops infinitos nas ferramentas
- Execução frequente de temporizadores

**Solução**:
- Reduzir beings concorrentes
- Optimizar o código das ferramentas
- Ajustar os intervalos dos temporizadores

### Utilização de Memória Elevada

**Monitorizar**:
```bash
# Através da Web UI: Dashboard > Memória
```

**Optimizações**:
- Limpar memórias antigas
- Reduzir o tamanho do contexto
- Implementar paginação

---

## Obter Ajuda

### Consultar Documentação

- [Guia de Início Rápido](getting-started.md)
- [Guia de Desenvolvimento](development-guide.md)
- [Referência API](api-reference.md)
- [Guia de Arquitectura](architecture.md)

### Verificar Registos

Verifique sempre os registos primeiro para obter detalhes dos erros.

### Suporte da Comunidade

- GitHub Issues: Reportar bugs
- Discussions: Fazer perguntas
- Documentação: Pesquisar soluções

---

## Procedimentos de Emergência

### Falha do Sistema

1. Verificar os registos para identificar a causa
2. Reiniciar a aplicação:

**SiliconLife.Default (Implementação Padrão)**:
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (Versão de Produção Recomendada)**:
```bash
dotnet run --project src/SiliconLife.Fast
```

3. Se necessário, restaurar a partir de backup

### Perda de Dados

1. Parar a aplicação imediatamente
2. Verificar ficheiros de backup
3. Restaurar dados
4. Verificar integridade

### Vulnerabilidade de Segurança

1. Parar todos os beings
2. Revogar todas as permissões
3. Verificar registos de auditoria
4. Rever controlos de acesso
5. Reiniciar com permissões restritas

---

## Prevenção

### Melhores Práticas

1. **Backups regulares**
   - Fazer backup do directório de dados
   - Fazer backup da configuração
   - Testar o processo de restauração

2. **Monitorizar recursos**
   - Monitorizar o uso de CPU/memória
   - Monitorizar o espaço em disco
   - Verificar ligações de rede

3. **Manter actualizado**
   - Actualizar o .NET SDK
   - Actualizar dependências
   - Aplicar patches de segurança

4. **Testar alterações**
   - Testar primeiro em desenvolvimento
   - Usar controlo de versões
   - Registar as alterações

---

## Próximos Passos

- 📚 Leia o [guia de arquitectura](architecture.md)
- 🛠️ Consulte o [guia de desenvolvimento](development-guide.md)
- 🚀 Consulte o [guia de início rápido](getting-started.md)
- 🔒 Consulte a [documentação de segurança](security.md)
