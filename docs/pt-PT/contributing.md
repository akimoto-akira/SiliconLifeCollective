# Guia de Contribuição

> **Versão: v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md)

Obrigado pelo seu interesse em contribuir para o SiliconLifeCollective!

## Contribuição em Duas Versões

Este projecto tem duas versões de implementação, e pode escolher a direcção de contribuição com base nos seus interesses:

### SiliconLife.Default (Versão Padrão)
- **Stack tecnológica**: Aplicação de consola .NET 9
- **Direcção de contribuição**: Desenvolvimento de funcionalidades principais, implementação de ferramentas, localização, documentação
- **Público-alvo**: Todos os programadores

### SiliconLife.Fast (Versão de Alto Desempenho)
- **Stack tecnológica**: Aplicação de ambiente de trabalho multiplataforma .NET 9 (Avalonia UI)
- **Direcção de contribuição**: Optimização de desempenho, armazenamento SpeedyPack, bandeja do sistema, concorrência sem locks
- **Público-alvo**: Programadores com experiência em desenvolvimento de ambiente de trabalho e interesse em optimização de desempenho

> **Nota importante**: Ambas as versões partilham os projectos SiliconLife.Core e SiliconLife.Common, pelo que melhorias nas interfaces principais afectarão ambas as versões simultaneamente.

## Código de Conduta

Este projecto segue a licença Apache 2.0. Mantenha respeito e profissionalismo em todas as interacções.

---

## Início Rápido

### 1. Fazer Fork do Repositório

Clique no botão "Fork" no GitHub para criar a sua própria cópia.

### 2. Clonar o Seu Fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Configurar o Ambiente de Desenvolvimento

```bash
# Instalar .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# Restaurar dependências
dotnet restore

# Compilar o projecto
dotnet build

# Executar testes
dotnet test
```

### 4. Criar um Ramo de Funcionalidade

```bash
git checkout -b feature/your-feature-name
```

### 5. Escolher o Projecto de Desenvolvimento

Escolha o projecto adequado com base no tipo de contribuição:

- **Interfaces/classes abstractas principais** → Modificar `SiliconLife.Core`
- **Implementação partilhada** → Modificar `SiliconLife.Common`
- **Específico da versão Default** → Modificar `SiliconLife.Default`
- **Específico da versão Fast** → Modificar `SiliconLife.Fast`
- **Motor de armazenamento** → Modificar `SiliconLife.Speedy`
- **Ferramenta de gestão de armazenamento** → Modificar `SiliconLife.Speedy.Manager`
- **Desenvolvimento de plugins** → Modificar `SiliconLife.Core/Plugins`
- **Documentação multilingue** → Modificar o directório `docs/`

---

## Fluxo de Trabalho de Desenvolvimento

### Estilo de Código

- Seguir as convenções de codificação C#
- Nomes de classes em PascalCase
- Parâmetros de métodos em camelCase
- Campos privados em `_camelCase`
- Todas as APIs públicas devem ter documentação XML

### Mensagens de Commit

Seguir o formato **Conventional Commits**:

```
<type>(<scope>): <description>
```

**Tipos**:
- `feat`: Nova funcionalidade
- `fix`: Correcção de bug
- `docs`: Alteração de documentação
- `style`: Formatação de código
- `refactor`: Refactorização de código
- `test`: Alteração de testes
- `chore`: Alteração de build/ferramentas

**Exemplos**:
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### Fazer Alterações

1. **Escrever código**
   - Seguir os padrões existentes
   - Adicionar testes para novas funcionalidades
   - Actualizar documentação

2. **Testar as suas alterações**
   ```bash
   # Executar todos os testes
   dotnet test
   
   # Compilar em modo Release
   dotnet build --configuration Release
   ```

3. **Formatar o código**
   ```bash
   dotnet format
   ```

4. **Submeter as alterações**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

5. **Empurrar para o seu Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Criar um Pull Request**
   - Ir para o repositório original
   - Clicar em "Compare & pull request"
   - Preencher o template do PR
   - Submeter

---

## Guia de Pull Requests

### Título do PR

Usar o mesmo formato das mensagens de commit:
```
feat(localization): add Korean language support
```

### Descrição do PR

Incluir:

1. **O quê** - O que este PR faz?
2. **Porquê** - Porque é que esta alteração é necessária?
3. **Como** - Como implementou?
4. **Testes** - Como testou?

### Exemplo de Descrição do PR

```markdown
## O quê
Adicionar localização em coreano para todos os componentes UI e documentação.

## Porquê
Expandir a acessibilidade do projecto para utilizadores coreanos.

## Como
- Criar ficheiro de localização KoKR.cs
- Adicionar 500+ chaves de tradução
- Actualizar todas as vistas para usar localização
- Criar documentação em coreano em docs/ko-KR/

## Testes
- Verificar que todos os elementos UI exibem coreano correctamente
- Testar a funcionalidade de troca de idioma
- Rever traduções com falantes nativos
```

---

## Tipos de Contribuição

### 1. Correcção de Bugs

**Processo**:
1. Verificar issues existentes
2. Criar um issue se não existir
3. Corrigir o bug
4. Adicionar casos de teste
5. Submeter PR

**Requisitos**:
- Descrição clara do bug
- Passos para reproduzir
- Teste para prevenir regressão

### 2. Nova Funcionalidade

**Processo**:
1. Discutir a funcionalidade em Issues/Discussions
2. Obter aprovação do maintainer
3. Implementar a funcionalidade
4. Adicionar testes abrangentes
5. Actualizar documentação
6. Submeter PR

**Requisitos**:
- Proposta de funcionalidade aprovada
- Cobertura de testes completa
- Documentação actualizada
- Compatibilidade retroactiva

### 3. Documentação

**Processo**:
1. Identificar lacunas na documentação
2. Escrever/actualizar documentação
3. Submeter PR

**Requisitos**:
- Claro e conciso
- Incluir exemplos
- Suportar múltiplos idiomas quando aplicável

### 4. Refactorização de Código

**Processo**:
1. Propor refactorização em Issue
2. Obter aprovação
3. Refactorizar o código
4. Garantir que todos os testes passam
5. Submeter PR

**Requisitos**:
- Sem alteração de funcionalidade
- Todos os testes passam
- Melhorar a qualidade do código
- Explicação clara

---

## Guia de Testes

### Testes Unitários

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // Arrange
    var service = new MyService();
    
    // Act
    var result = service.DoSomething();
    
    // Assert
    Assert.IsTrue(result.Success);
}
```

### Testes de Integração

Testar fluxos de trabalho completos:
- Interação com IA
- Execução de ferramentas
- Verificação de permissões
- Operações de armazenamento

### Testes Manuais

Para alterações na UI:
- Testar em múltiplos navegadores
- Verificar design responsivo
- Verificar acessibilidade

---

## Guia de Documentação

### Comentários no Código

- Usar comentários XML para todas as APIs públicas
- Usar comentários inline para lógica complexa
- Comentários de código em inglês

### Ficheiros de Documentação

- Colocar em `docs/{language}/`
- Actualizar todas as versões linguísticas
- Seguir a estrutura existente

### Documentação Multilingue

Ao adicionar documentação:
1. Criar primeiro a versão em inglês
2. Traduzir para outros idiomas
3. Manter o conteúdo sincronizado

---

## Processo de Revisão

### O que os Maintainers Verificam

1. **Qualidade do código**
   - Seguir convenções
   - Claro e legível
   - Bem documentado

2. **Testes**
   - Cobertura adequada
   - Todos os testes passam
   - Cobrir casos limite

3. **Documentação**
   - Actualizada
   - Explicações claras
   - Multilingue

4. **Compatibilidade**
   - Compatível retroactivamente
   - Sem alterações disruptivas (a menos que notificado)
   - Seguir versionamento semântico

### Timeline de Revisão

- Revisão inicial: 1-3 dias
- Integração de feedback: conforme necessário
- Merge: após aprovação

---

## Perguntas Frequentes

### PR Rejeitado

**Razões**:
- Não seguir as guidelines
- Testes insuficientes
- Alterações disruptivas não notificadas
- Qualidade de código fraca

**Solução**:
- Resolver o feedback
- Actualizar o PR
- Re-submeter

### Conflitos de Merge

**Solução**:
```bash
# Actualizar o seu ramo
git fetch origin
git rebase origin/master

# Resolver conflitos
# Editar ficheiros em conflito
git add .
git rebase --continue

# Force push
git push --force-with-lease
```

---

## Obter Ajuda

### Recursos

- **Documentação**: [docs/](../)
- **Issues**: GitHub Issues
- **Discussões**: GitHub Discussions
- **Código de Conduta**: CODE_OF_CONDUCT.md

### Contacto

- Criar um Issue para bugs
- Iniciar uma Discussion para questões
- Marcar maintainers para assuntos urgentes

---

## Agradecimentos

Os contribuidores serão reconhecidos em:
- Secção de contribuidores do README.md
- Notas de lançamento
- Documentação do projecto

---

## Licença

Ao contribuir, concorda que as suas contribuições serão licenciadas sob a licença Apache 2.0.

---

## Próximos Passos

- 📚 Leia a [documentação](../)
- 🐛 Consulte as [issues abertas](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Inicie uma [discussão](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Faça fork e comece a contribuir!

Obrigado por contribuir para o SiliconLifeCollective! 🎉
