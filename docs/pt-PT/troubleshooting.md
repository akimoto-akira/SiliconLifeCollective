# Guia de resolução de problemas

> **Versão: v0.2.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [Français](../fr-FR/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md) | [Italiano](../it-IT/troubleshooting.md) | [Polski](../pl-PL/troubleshooting.md) | **Português**

## Problemas comuns

### Compilação e build

#### Problema: Build falhada, dependências em falta

**Sintomas**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Solução**:
```bash
dotnet restore
dotnet build
```

#### Problema: SDK .NET não encontrado

**Sintomas**:
```
The .NET SDK could not be found
```

**Solução**:
1. Instalar o SDK .NET 9: https://dotnet.microsoft.com/download/dotnet/9.0
2. Verificar a instalação:
```bash
dotnet --version
```

---

### Problemas de ligação IA

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
# Descarregar o modelo necessário
ollama pull qwen2.5:7b

# Listar os modelos disponíveis
ollama list
```

#### Problema: Erro DashScope 404

**Sintomas**:
```
HTTP 404: Model not found
```

**Solução**:
1. Verificar a chave API
2. Verificar o nome do modelo com o catálogo DashScope
3. Verificar o endpoint regional
4. Verificar o acesso da conta ao modelo

#### Problema: Ligação Volcengine Ark falhada

**Sintomas**:
```
HTTP 401: Unauthorized
ou
HTTP 404: Endpoint not found
```

**Solução**:
1. Verificar a chave API
2. Verificar o formato do URL do endpoint (predefinido: `https://ark.cn-beijing.volces.com/api/v3/chat/completions`)
3. Verificar que o parâmetro Model utiliza um ID de endpoint de inferência (ex. `ep-20241212123456-abcde`), não um nome de modelo
4. Verificar o acesso da conta ao endpoint

---

### Problemas de execução

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

**Ou alterar a porta na configuração**.

#### Problema: O Being não inicia

**Sintomas**:
- O estado do Being mostra "Error"
- Os logs mostram erros de inicialização

**Solução**:
1. Verificar que o ficheiro da alma existe e é válido
2. Verificar que o cliente IA está configurado
3. Examinar os logs para erros específicos:
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

2. **SiliconLife.Fast**: A versão Fast tem um consumo de memória elevado (~500 MB). Se a memória for constantemente limitada, recomenda-se:
   - Reduzir o número de Silicon Beings em execução simultânea
   - Limpar dados antigos para libertar memória

3. Limpar dados antigos:
```bash
# Arquivar logs antigos
mv logs/ logs-archive/
mkdir logs

# Limpar armazenamento antigo
# Através da interface Web: Gestão de armazenamento > Limpeza
```

> **Dica**: SiliconLife.Default tem um baixo consumo de memória (~200 MB), adequado para ambientes com memória limitada; SiliconLife.Fast tem um consumo de memória mais elevado mas melhor desempenho, adequado para ambientes de produção.

---

### Problemas de permissões

#### Problema: Permissão negada

**Sintomas**:
```
Permission denied: disk:write
```

**Solução**:
1. Verificar as permissões atuais:
```bash
curl http://localhost:8080/api/permissions
```

2. Conceder permissão:
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

3. Ou usar a interface Web: Gestão de permissões

#### Problema: A permissão não expira

**Sintomas**:
- A permissão ainda está ativa após o termo de expiração

**Solução**:
1. Verificar a sincronização do relógio do sistema
2. Verificar que o campo `expiresAt` está definido corretamente
3. Limpar a cache de permissões

---

### Problemas da interface Web

#### Problema: Interface Web inacessível

**Sintomas**:
- O browser mostra "Connection refused"

**Solução**:
1. Verificar que o servidor está em execução
2. Verificar o URL correto: `http://localhost:8080`
3. Verificar as definições da firewall
4. Examinar os logs para erros de arranque

#### Problema: SSE não funciona

**Sintomas**:
- As atualizações em tempo real não aparecem
- O chat não é transmitido em streaming

**Solução**:
1. Verificar o suporte SSE do browser
2. Desativar o buffering do proxy para SSE
3. Verificar a estabilidade da rede
4. Experimentar outro browser

#### Problema: A interface parece quebrada

**Sintomas**:
- Estilos incorretos
- Layout partido

**Solução**:
1. Limpar a cache do browser
2. Experimentar outro tema: Definições > Tema
3. Verificar a consola do browser para erros
4. Desativar as extensões do browser

---

### Problemas de armazenamento

#### Problema: Dados não legíveis/escritíveis

**Sintomas**:
```
IOException: Access denied
```

**Solução**:
1. Verificar as permissões dos ficheiros
2. Verificar que o caminho de armazenamento existe
3. Verificar o espaço em disco
4. Executar com as permissões adequadas

#### Problema: Corrupção de dados

**Sintomas**:
- Erros de parsing JSON
- Perda de dados

**Solução**:
1. Restaurar a partir de um backup
2. Verificar a integridade do armazenamento:
```bash
# Através da interface Web: Sistema > Verificar armazenamento
```

3. Reparar manualmente os ficheiros corrompidos

#### Problema: Ficheiro de armazenamento SpeedyPack corrompido (versão Fast)

**Sintomas**:
- O ficheiro `.spk` não pode ser carregado
- A inicialização do SpeedyStorage falha

**Solução**:
1. Usar a ferramenta `SiliconLife.Speedy.Manager` para verificar e reparar ficheiros `.spk`
2. Verificar que o ficheiro de índice `.spk.idx` corresponde ao ficheiro `.spk`
3. Se o ficheiro de índice estiver corrompido, eliminar o ficheiro `.spk.idx` — o sistema recriará o índice automaticamente
4. Restaurar o ficheiro `.spk` a partir de um backup

#### Problema: Auto-compactação SpeedyPack falhada (versão Fast)

**Sintomas**:
- O ficheiro `.spk` cresce continuamente
- O espaço em disco torna-se insuficiente

**Solução**:
1. Verificar que o `SpeedyPackAutoCompactor` funciona corretamente
2. Acionar manualmente a operação de compactação
3. Verificar a configuração do limiar de compactação
4. Usar a ferramenta `SiliconLife.Speedy.Manager` para compactação manual

---

### Problemas de execução de ferramentas

#### Problema: Ferramenta não encontrada

**Sintomas**:
```
Tool "xyz" not found
```

**Solução**:
1. Verificar que o nome da ferramenta está correto
2. Verificar a ferramenta no diretório de ferramentas
3. Reconstruir o projeto
4. Verificar que a ferramenta está implementada corretamente

#### Problema: A ferramenta retorna um erro

**Sintomas**:
```
Tool execution failed: ...
```

**Solução**:
1. Examinar os logs da ferramenta
2. Verificar os parâmetros de entrada
3. Testar a ferramenta independentemente
4. Verificar as permissões

---

### Problemas de plugins

#### Problema: Carregamento de plugin falhado

**Sintomas**:
```
Plugin load failed: Security check failed
```

**Solução**:
1. Verificar se o plugin referencia namespaces proibidos (`System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`)
2. Verificar que o plugin referencia apenas assemblies da whitelist de confiança
3. Verificar que o plugin implementa corretamente a interface `IPlugin`
4. Examinar os logs para os detalhes dos erros de verificação de segurança

#### Problema: Ferramentas do plugin não registadas

**Sintomas**:
- O plugin carregou com sucesso, mas as ferramentas não aparecem na lista

**Solução**:
1. Confirmar que a classe da ferramenta no plugin implementa corretamente a interface `ITool`
2. Verificar que a classe da ferramenta é pública
3. Verificar que `ToolManager.ScanAllPluginAssemblies()` foi chamado
4. Reconstruir o plugin e reiniciar a aplicação

---

### Problemas de notas de trabalho

#### Problema: Impossível criar uma nota de trabalho

**Sintomas**:
```
Failed to create work note
```

**Solução**:
1. Verificar que o Being existe e funciona
2. Verificar que o caminho de armazenamento tem direitos de escrita
3. Verificar que o conteúdo não está vazio (conteúdo obrigatório)
4. Examinar os logs para os detalhes do erro

#### Problema: Pesquisa de notas sem resultados

**Sintomas**:
- A pesquisa por palavra-chave retorna resultados vazios
- Mas notas relevantes certamente existem

**Solução**:
1. Verificar a ortografia da palavra-chave
2. Experimentar uma palavra-chave mais genérica
3. Verificar que a nota contém a palavra-chave (sensível a maiúsculas/minúsculas)
4. Aumentar o valor do parâmetro `max_results`

#### Problema: Geração do diretório de notas lenta

**Sintomas**:
- Tempo de resposta longo para a geração do diretório
- O Being tem muitas notas (>1000 páginas)

**Solução**:
1. É normal, precisa de percorrer todas as notas
2. Arquivar regularmente notas antigas
3. Usar a função de pesquisa em vez de percorrer o diretório
4. Otimização planeada: mecanismo de cache do diretório

---

### Problemas da rede de conhecimentos

#### Problema: A consulta de conhecimentos retorna resultados vazios

**Sintomas**:
```
No knowledge triples found
```

**Solução**:
1. Verificar a ortografia do sujeito e do predicado
2. Verificar que o conhecimento foi adicionado à rede
3. Usar a função de pesquisa para correspondência difusa:
```json
{
  "action": "search",
  "query": "palavra-chave"
}
```

#### Problema: Pesquisa do caminho de conhecimento falhada

**Sintomas**:
```
No path found between concepts
```

**Solução**:
1. Verificar que ambos os conceitos existem na rede de conhecimentos
2. Verificar que existe um caminho de ligação (talvez não haja relação direta/indireta)
3. Adicionar mais conhecimentos para estabelecer uma ligação
4. Reduzir o limite de comprimento do caminho (se definido)

#### Problema: Validação do conhecimento falhada

**Sintomas**:
```
Knowledge validation failed
```

**Solução**:
1. Verificar que o formato da tripla está correto (sujeito, predicado, objeto obrigatórios)
2. Verificar que o valor de confiança está no intervalo 0.0-1.0
3. Verificar triplas duplicadas
4. Examinar os detalhes do erro de validação para o problema específico

#### Problema: Estatísticas da rede de conhecimentos imprecisas

**Sintomas**:
- Os números das estatísticas não são os esperados
- As estatísticas não são atualizadas após adicionar conhecimentos

**Solução**:
1. As estatísticas podem demorar alguns segundos a ser atualizadas (cache)
2. Verificar que a operação de eliminação foi executada com sucesso
3. Reiniciar a aplicação para forçar a atualização das estatísticas
4. Solicitar as estatísticas através da API

---

### Problemas de gestão de projetos

#### Problema: Impossível criar um projeto

**Sintomas**:
```
Failed to create project
```

**Solução**:
1. Verificar que o nome do projeto não está vazio (obrigatório)
2. Verificar que o nome do projeto não está duplicado
3. Verificar que o caminho de armazenamento tem direitos de escrita
4. Examinar os logs para os detalhes do erro

#### Problema: Dados do projeto perdidos

**Sintomas**:
- As informações do projeto não são carregáveis
- Os ficheiros do projeto estão corrompidos

**Solução**:
1. Verificar que o diretório de armazenamento do projeto existe
2. Restaurar os dados do projeto a partir de um backup
3. Verificar que o formato do ficheiro JSON está correto
4. Reparar manualmente os ficheiros de projeto corrompidos

---

## Depuração

### Ativar logs detalhados

Modificar a configuração:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Examinar os logs

Os logs estão armazenados em:
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

### Usar o depurador

**SiliconLife.Default (implementação padrão)**:
```bash
# Executar com o depurador
dotnet run --project src/SiliconLife.Default --configuration Debug

# Anexar o depurador
# Através do IDE: Anexar ao processo > SiliconLife.Default
```

**SiliconLife.Fast (versão de alto desempenho)**:
```bash
# Executar com o depurador
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Anexar o depurador
# Através do IDE: Anexar ao processo > SiliconLife.Fast
```

> **Recomendação**: Para depuração de desenvolvimento, recomenda-se usar SiliconLife.Default. Após verificação arquitetural bem-sucedida, usar SiliconLife.Fast para implementação em produção.

---

## Problemas de desempenho

### Tempos de resposta lentos

**Otimizar**:
1. Reduzir a complexidade do modelo IA
2. Ativar a cache
3. Limpar dados antigos
4. Aumentar os recursos do sistema

### Utilização de CPU elevada

**Verificar**:
- Demasiados Beings em execução
- Ciclo infinito nas ferramentas
- Execução frequente dos temporizadores

**Solução**:
- Reduzir os Beings paralelos
- Otimizar o código das ferramentas
- Ajustar o intervalo dos temporizadores

### Utilização de memória elevada

**Monitorizar**:
```bash
# Através da interface Web: Dashboard > Memória
```

**Otimizar**:
- Limpar memórias antigas
- Reduzir o tamanho do contexto
- Implementar paginação

---

## Obter ajuda

### Consultar a documentação

- [Guia de introdução](getting-started.md)
- [Guia de desenvolvimento](development-guide.md)
- [Referência da API](api-reference.md)
- [Guia de arquitetura](architecture.md)

### Examinar os logs

Examinar sempre primeiro os logs para os detalhes dos erros.

### Suporte da comunidade

- GitHub Issues: Reportar bugs
- Discussions: Fazer perguntas
- Documentação: Procurar soluções

---

## Procedimentos de emergência

### Falha do sistema

1. Examinar os logs para a causa
2. Reiniciar a aplicação:

**SiliconLife.Default (implementação padrão)**:
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (versão principal de produção)**:
```bash
dotnet run --project src/SiliconLife.Fast
```

3. Restaurar a partir de um backup se necessário

### Perda de dados

1. Parar imediatamente a aplicação
2. Verificar os ficheiros de backup
3. Restaurar os dados
4. Verificar a integridade

### Violação de segurança

1. Parar todos os Beings
2. Revogar todas as permissões
3. Examinar os logs de auditoria
4. Verificar o controlo de acesso
5. Reiniciar com permissões limitadas

---

## Prevenção

### Boas práticas

1. **Backups regulares**
   - Fazer backup do diretório de dados
   - Fazer backup da configuração
   - Testar o processo de restauração

2. **Monitorizar os recursos**
   - Monitorizar a utilização de CPU/memória
   - Monitorizar o espaço em disco
   - Verificar a ligação de rede

3. **Manter atualizado**
   - Atualizar o SDK .NET
   - Atualizar as dependências
   - Aplicar patches de segurança

4. **Testar as alterações**
   - Testar primeiro em desenvolvimento
   - Usar o controlo de versões
   - Documentar as alterações

---

## Próximos passos

- 📚 Ler o [guia de arquitetura](architecture.md)
- 🛠️ Consultar o [guia de desenvolvimento](development-guide.md)
- 🚀 Ver o [guia de introdução](getting-started.md)
- 🔒 Consultar a [documentação de segurança](security.md)
