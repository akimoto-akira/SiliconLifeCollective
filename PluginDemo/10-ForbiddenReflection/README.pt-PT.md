# PluginDemo-10: Anti-padrão de reflexão proibida

## Visão geral

Este plugin demonstra operações de reflexão **proibidas** no sistema de plugins SiliconLife. Serve como referência de anti-padrão, mostrando o que NÃO fazer e fornecendo as alternativas corretas para cada violação.

## Por que a reflexão é a ameaça principal?

A evasão por reflexão é a **ameaça mais crítica** para a varredura de segurança do PluginLoader. Enquanto a varredura TypeRef captura referências diretas de tipos em tempo de compilação, os métodos de reflexão podem resolver tipos em **tempo de execução** usando strings — completamente invisíveis à varredura estática de metadados.

Se um plugin pode chamar `Type.GetType("System.IO.File, System.Runtime")`, pode aceder a QUALQUER tipo proibido sem que nenhuma referência apareça na tabela TypeRef dos metadados PE.

## Quais métodos são proibidos?

Todos os métodos proibidos são detetados via **varredura MemberRef** (não bloqueio ao nível de namespace ou tipo):

| Método proibido | Assinatura | Ameaça |
|----------------|-----------|--------|
| `Type.GetType` | `System.Type::GetType(System.String)` | Resolver tipo arbitrário por nome em tempo de execução |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | Instanciar tipos arbitrários |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | Carregar assembly por nome/bytes |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | Carregar assembly do disco |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | Carregar assembly de caminho |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | Resolução de tipo baseada em strings |

## O que é seguro?

Nem toda a reflexão é proibida. Os seguintes padrões são **seguros** porque referenciam tipos conhecidos em tempo de compilação:

| Padrão seguro | Exemplo | Por que é seguro |
|--------------|---------|-----------------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | Tipo conhecido em compilação, visível em TypeRef |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | Inspeção de tipo conhecido, sem novos tipos |
| Restrições genéricas | `FindSubtypesOf(typeof(BaseTool))` | Parâmetro genérico é tipo de compilação |
| `nameof()` | `nameof(MyClass.MyMethod)` | String de compilação, sem resolução em execução |

**Distinção chave:**
- `typeof(X).Assembly` → **Seguro** (referência de compilação, varrido pelo PluginLoader)
- `Assembly.Load("X")` → **Proibido** (string de execução, contorna todas as varreduras)

## Como substituir a reflexão de forma segura?

### Usar ITypeRegistry (Substitui Type.GetType + varredura AppDomain)

```csharp
// ❌ PROIBIDO: Resolver tipo por string em tempo de execução
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ CORRETO: Usar ITypeRegistry para encontrar tipos registados
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// Apenas tipos registados durante OnLoad são descobríveis
```

### Usar IObjectFactory (Substitui Activator.CreateInstance)

```csharp
// ❌ PROIBIDO: Criar instância arbitrária
object? instance = Activator.CreateInstance(someType);

// ✅ CORRETO: Usar IObjectFactory com fábrica registada
var instance = objectFactory.CreateInstance<MyService>();
// Apenas tipos com fábricas registadas podem ser instanciados
```

## Violações demonstradas

### Violação 1: Type.GetType(string)

```csharp
// ❌ PROIBIDO
Type? fileType = Type.GetType("System.IO.File, System.Runtime");

// ✅ CORRETO
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**MemberRef bloqueada**: `System.Type::GetType(System.String)`

### Violação 2: Activator.CreateInstance

```csharp
// ❌ PROIBIDO
object? client = Activator.CreateInstance(httpClientType!);

// ✅ CORRETO
var instance = objectFactory.CreateInstance<MyService>();
```

**MemberRef bloqueada**: `System.Activator::CreateInstance`

### Violação 3: Assembly.Load

```csharp
// ❌ PROIBIDO
Assembly asm = Assembly.Load("System.Net.Http");

// ✅ CORRETO
Assembly myAsm = typeof(MyPlugin).Assembly;  // Seguro: conhecido em compilação
```

**MemberRef bloqueada**: `System.Reflection.Assembly::Load(System.String)`

### Violação 4: Assembly.LoadFile / LoadFrom

```csharp
// ❌ PROIBIDO
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");

// ✅ CORRETO
// Todas as dependências devem estar no diretório do plugin e ser varridas pelo PluginLoader.
```

**MemberRef bloqueada**: `System.Reflection.Assembly::LoadFile(System.String)`

### Violação 5: Assembly.GetType(string)

```csharp
// ❌ PROIBIDO
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ CORRETO
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
```

**MemberRef bloqueada**: `System.Reflection.Assembly::GetType(System.String)`

## Por que typeof(X).Assembly é seguro e Assembly.Load não é

| Operação | Visibilidade | Segurança |
|---------|-------------|-----------|
| `typeof(X).Assembly` | Tipo X na tabela TypeRef → PluginLoader varre-o | ✅ Seguro |
| `Assembly.Load("X")` | String "X" só existe em execução → invisível à varredura TypeRef | ❌ Proibido |
| `obj.GetType()` | Retorna tipo de instância existente → nenhum tipo novo | ✅ Seguro |
| `Type.GetType("X")` | Resolve tipo arbitrário de string → contorna TypeRef | ❌ Proibido |

## Melhores práticas

1. **Registar tipos em OnLoad**: Usar `ITypeRegistry.RegisterType` / `RegisterFromAssembly`
2. **Usar IObjectFactory para criação dinâmica**: Nunca usar `Activator.CreateInstance`
3. **Usar typeof(X).Assembly**: Acesso seguro à própria assembly
4. **Evitar nomes de tipo baseados em strings**: Ativa a varredura de strings IL
5. **Projetar para descobribilidade estática**: Não visível em metadados = suspeito

## Ficheiros

- `Plugin.cs` - Plugin de demonstração anti-padrão
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Este ficheiro (Português)
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Exemplos relacionados

- **02-TypeRegistryUsage**: Uso correto de ITypeRegistry
- **03-ObjectFactoryUsage**: Uso correto de IObjectFactory
- **11-ForbiddenPInvoke**: P/Invoke e código unsafe proibidos
- **12-ForbiddenStringBypass**: Tentativas de evasão por reflexão via strings
